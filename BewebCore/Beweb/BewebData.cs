#define Util
#define TestExtensions
#define FileSystem
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;
//using Models;


namespace Beweb {
	/// <summary>
	/// Summary description for DataAccess
	/// </summary>
	public static class BewebData {
		//public string LastCreateSQL="";

		public class TransConnection : IDisposable {
			public DbConnection Connection;
			public DbTransaction Transaction;
			bool isInProgress = true;
			public TransConnection(string connectionString) {
				this.Connection = CreateNewConnection(connectionString, true);
				this.Transaction = Connection.BeginTransaction(IsolationLevel.Snapshot);
			}
			public void Commit() {
				isInProgress = false;
				try {
					Transaction.Commit();
					Connection.Close();
				} finally {
					Transaction.Dispose();
					Connection.Dispose();
				}
			}
			public void Rollback() {
				isInProgress = false;
				try {
					Transaction.Rollback();
					Connection.Close();
				} finally {
					Transaction.Dispose();
					Connection.Dispose();
				}
			}

			public void Dispose() {
				try {
					Connection.Close();
				} finally {
					Transaction.Dispose();
					Connection.Dispose();
				}
			}

			public DbCommand CreateCommand(string sqlStatement) {
				var command = Connection.CreateCommand();
				command.CommandText = sqlStatement;
				command.CommandTimeout = 30;
				command.Transaction = Transaction;
				return command;
			}
		}

		// TODO: add static methods BeginTrans and CommitTrans - modify all data access methods to be transaction-aware
		// BeginTrans should open a connection and save it in Web.PageGlobals
		// CommitTrans should close the connection
		// All data access methods will need to check whether there is a transaction active and if so, use the active connection. If not, they can open & close their own connections as current.
		// Many of the data access methods currently just pass in a connectionstring - these will need to change to passing the connection object.
		// The methods that automatically close the connections when done will need to only do this if no transaction active.

		public static TransConnection BeginTransaction(string connectionStringOrSettingName, bool isConnectionString) {
			string connectionString = (!isConnectionString) ? GetConnectionString(connectionStringOrSettingName) : connectionStringOrSettingName;
			//if (Web.PageGlobals.Contains("BewebData.Connection."+connectionString)) {
			//  throw new BewebDataException("Cannot begin transaction. Transaction already in progress on this connection");
			//}
			var trans = new TransConnection(connectionString);
			//Web.PageGlobals["BewebData.Connection."+connectionString] = trans;
			return trans;
		}

		//public static void CommitTransaction(string connectionStringOrSettingName, bool isConnectionString) {
		//string connectionString = (!isConnectionString) ? GetConnectionString(connectionStringOrSettingName) : connectionStringOrSettingName;
		//var trans = (TransConnection)Web.PageGlobals["BewebData.Connection."+connectionString];
		//if (trans==null) {
		//  throw new BewebDataException("Cannot Commit. Transaction not in progress on this connection");
		//}
		//trans.Commit();
		//Web.PageGlobals.Remove("BewebData.Connection."+connectionString);
		//}

		/// <summary>
		/// Create a new DbConnection given for the default connection string (specified in web config).
		/// Creates SqlConnection, OleDbConnection or OdbcConnection (depending on provider specified in web config).
		/// Opens and then returns the connection. You must close it later with .Close()
		/// </summary>
		/// <returns>the new database connection</returns>
		public static DbConnection CreateNewConnection() {
			return CreateNewConnection("", false);
		}

		/// <summary>
		/// Create a new DbConnection given connection string name (where connection strings are specified in web config).
		/// Creates SqlConnection, OleDbConnection or OdbcConnection (depending on provider specified in web config).
		/// Just returns the connection.
		/// </summary>
		/// <param name="connectionStringOrSettingName">Either full connection string, or name of connection string setting specified in web config</param>
		/// <param name="isConnectionString">If true, the first param is the full connection string. If false, the first param as a setting name in the web config that contains the connection string.</param>
		/// <returns>the new database connection</returns>
		public static DbConnection CreateNewConnection(string connectionStringOrSettingName, bool isConnectionString) {
			string connectionString = (!isConnectionString) ? GetConnectionString(connectionStringOrSettingName) : connectionStringOrSettingName;
			string providerName = (!isConnectionString) ? GetConnectionStringProviderName(connectionStringOrSettingName) : "";
			DbConnection newConnection;
			if (providerName == "System.Data.SqlClient" || isConnectionString) {
				newConnection = new SqlConnection(connectionString);
			} else if (providerName == "System.Data.OleDb") {
				newConnection = new OleDbConnection(connectionString);
			} else {
				newConnection = new OdbcConnection(connectionString);
			}
			newConnection.Open();
			return newConnection;
		}

		#region GetConnectionString
		/// <summary>
		/// Gets the correct connection string from the web.config based on the server the website is running on.
		/// Assumes the default connection string called "ConnectionString"+[server name].
		/// </summary>
		/// <returns>The connection string</returns>
		public static string GetConnectionString() {
			return GetConnectionString("");
		}

		/// <summary>
		/// Gets a named connection string from the web.config.
		/// It will first look for the name plus the server code (DEV, STG, LVE) and then the setting name by itself.
		/// </summary>
		/// <returns>The connection string</returns>
		public static string GetConnectionString(string connectionStringSettingName) {
			return GetConnectionStringSetting(connectionStringSettingName).ConnectionString;
		}

		public static string GetConnectionStringProviderName(string connectionStringSettingName) {
			return GetConnectionStringSetting(connectionStringSettingName).ProviderName;
		}

		/// <summary>
		/// Get a connection string setting from web.config settings.
		/// Setting name is normally the default "ConnectionString" (just pass "" for default), it could be different if there is more than one db connection string.
		/// </summary>
		/// <param name="connectionStringSettingName"></param>
		/// <returns></returns>
		public static ConnectionStringSettings GetConnectionStringSetting(string connectionStringSettingName) {
			if (connectionStringSettingName == null || connectionStringSettingName == "") {
				connectionStringSettingName = "ConnectionString";
			}

			// decoupled from Beweb.Web
			var isRequestInitialised = true;
			if (HttpContext.Current == null) {
				isRequestInitialised = false;
			} else {
				try {
					var r = HttpContext.Current.Request;
				} catch (HttpException) {
					isRequestInitialised = false;
				}
			}

			ConnectionStringSettings setting = null;
			if (isRequestInitialised) {
				// first try named hostname
				setting = ConfigurationManager.ConnectionStrings[connectionStringSettingName + "_" + HttpContext.Current.Request.Url.Host];
			} else {
				setting = ConfigurationManager.ConnectionStrings[connectionStringSettingName + "_" + System.Environment.MachineName];
			}
#if Util
			// try DEV, STG, LVE
			if (setting == null) {
				setting = ConfigurationManager.ConnectionStrings[connectionStringSettingName + Util.ServerIs()];
			}
#endif
			// try live suffix
			if (setting == null) {
				setting = ConfigurationManager.ConnectionStrings[connectionStringSettingName + "LVE"];
			}
			// try no suffix
			if (setting == null) {
				setting = ConfigurationManager.ConnectionStrings[connectionStringSettingName];
			}
			// make sure it exists
			if (setting == null) {
				throw new Exception("Connection String setting named [" + connectionStringSettingName + "] was not found in web config.");
			}
			return setting;
		}

		#endregion

		// uses fast datareader - good for multiple rows
		#region GetRecords
		private static void OnGetReaderSelecting(object sender, SqlDataSourceCommandEventArgs e) {
			e.Command.CommandTimeout = 50; //seconds
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <param name="selectParameters"></param>
		/// <returns></returns>
		/// <example>
		/*
				using(SqlDataReader sdr = BewebData.GetRecords( new Sql())){
					if(sdr != null) {
						foreach (DbDataRecord dr in sdr) {
							String thisEmail = dr["Email"].ToString();
						}
						sdr.Close();
					}
				}
		 */
		/// </example>
		public static SqlDataReader GetRecords(string sqlStatement, ParameterCollection selectParameters) {
			return GetRecords(sqlStatement, selectParameters, null);
		}
		[Obsolete("Deprecated - use another getrecords")]
		public static SqlDataReader GetRecords(string sqlStatement, ParameterCollection selectParameters, int timeout) {
			return GetRecords(sqlStatement, selectParameters, timeout, null);
		}
		[Obsolete("Deprecated - use another getrecords")]
		public static SqlDataReader GetRecords(string sqlStatement, ParameterCollection selectParameters, int timeout, string connectionStringName) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataReader;
			ds.CancelSelectOnNullParameter = false;
			if (String.IsNullOrEmpty(connectionStringName)) {
				ds.ConnectionString = GetConnectionString();
			} else {
				ds.ConnectionString = GetConnectionString(connectionStringName);
			}
			ds.SelectCommand = sqlStatement;
			ds.Selecting += delegate(object sender, SqlDataSourceSelectingEventArgs e) { e.Command.CommandTimeout = timeout; };

			foreach (Parameter sp in selectParameters) {
				ds.SelectParameters.Add(sp);
			}
			SqlDataReader dr;
			try {
				dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetRecords: " + e.Message + " in [" + sqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new Exception(errorMessage);
			}
			return dr;
		}

		public static SqlDataReader GetRecords(string sqlStatement, ParameterCollection selectParameters, string connectionString) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataReader;
			ds.CancelSelectOnNullParameter = false;
			ds.ConnectionString = (String.IsNullOrEmpty(connectionString)) ? GetConnectionString() : connectionString;

			ds.Selecting += OnGetReaderSelecting;				//sort out the command timeout 

			ds.SelectCommand = sqlStatement;
			foreach (Parameter sp in selectParameters) {
				ds.SelectParameters.Add(sp);
			}
			SqlDataReader dr;
			try {
				dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetRecords: " + e.Message + " in [" + sqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new Exception(errorMessage);
			}
			return dr;
		}
		public static SqlDataReader GetRecords(string sqlStatement, Parameter selectParameter) {
			return GetRecords(sqlStatement, selectParameter, null);
		}
		public static SqlDataReader GetRecords(string sqlStatement, Parameter selectParameter, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(selectParameter);
			return GetRecords(sqlStatement, pc, connectionString);
		}
		[Obsolete("Deprecated - use new Sql(sql).GetReader() instead")]
		public static SqlDataReader GetRecords(string sqlStatement) {
			ParameterCollection pc = new ParameterCollection();
			// don't bother adding any parameters for this overload
			return GetRecords(sqlStatement, pc);
		}
		[Obsolete("Deprecated - use new Sql(sql).GetReader() instead")]
		public static DbDataReader GetRecords(Sql sqlStatement) {
			return GetRecords(sqlStatement, null);
		}
		[Obsolete("Deprecated - use new Sql(sql){SqlConnectionString=connectionString}.GetReader() instead")]
		public static DbDataReader GetRecords(Sql sql, string connectionString) {
			//ParameterCollection pc = new ParameterCollection();
			// don't bother adding any parameters for this overload
			//return GetRecords(sqlStatement.ToString(), pc, connectionString);
			if (connectionString != null) sql.SqlConnectionString = connectionString;
			return sql.GetReader();
		}
		#endregion

		// slower dataset is better if data manipulation like sorting, filtering etc need to be done
		#region GetDataSet
		//EXAMPLE of GetDataSet use
		//DataSet ds = BewebData.GetDataSet("SELECT * FROM ProductCategory ORDER BY SortPosition ASC");
		//foreach (DataRow dr in ds.Tables[0].Rows)
		//{
		//		Console.WriteLine(dr[0].ToString());
		//}
		public static DataSet GetDataSet(string SqlStatement, ParameterCollection selectParameters) {
			return GetDataSet(SqlStatement, selectParameters, null);
		}
		public static DataSet GetDataSet(string SqlStatement, ParameterCollection selectParameters, string connectionString) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataSet;
			ds.CancelSelectOnNullParameter = false;
			ds.Selecting += OnGetReaderSelecting;				//sort out the command timeout 
			ds.ConnectionString = (String.IsNullOrEmpty(connectionString)) ? GetConnectionString() : connectionString;
			ds.SelectCommand = SqlStatement;
			foreach (Parameter sp in selectParameters) {
				ds.SelectParameters.Add(sp);
			}

			DataView view;
			try {
				view = (DataView)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetDataSet: " + e.Message + " in [" + SqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new Exception(errorMessage);
			}

			// -----------------
			// to return a dataset we need to do this conversion
			DataSet dataSet = new DataSet();
			if (view == null) {
				dataSet = null;
			} else {
				DataTable table = view.ToTable();
				dataSet.Tables.Add(table);
			}
			// -----------------		
			return dataSet;
		}
		public static DataSet GetDataSet(string SqlStatement, Parameter selectParameter) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(selectParameter);
			return GetDataSet(SqlStatement, pc);
		}
		public static DataSet GetDataSet(string SqlStatement) {
			ParameterCollection pc = new ParameterCollection();
			// don't bother adding any parameters for this overload
			return GetDataSet(SqlStatement, pc);
		}
		public static DataSet GetDataSet(Sql sqlStatement) {
			return GetDataSet(sqlStatement, null);
		}
		public static DataSet GetDataSet(Sql sqlStatement, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			// don't bother adding any parameters for this overload
			return GetDataSet(sqlStatement.ToString(), pc, connectionString);
		}
		#endregion

		// just returns the value as a string - nothing more
		#region GetValue
		public static String GetValue(Sql sqlStatement) {
			return GetValue(sqlStatement, (string)null);
		}
		public static String GetValue(Sql sqlStatement, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			return GetValue(sqlStatement.ToString(), pc, connectionString);
		}
		[Obsolete("Deprecated - use GetValue(Sql) instead")]
		public static String GetValue(string sqlStatement) {
			return GetValue(sqlStatement, (string)null);
		}
		public static String GetValue(string sqlStatement, bool IHaveCheckedTheSQLStringForInjection) {
			if (!IHaveCheckedTheSQLStringForInjection) { throw new Exception("You need to check the sql string"); }
			return GetValue(sqlStatement, (string)null);
		}
		[Obsolete("Deprecated - use GetValue(Sql) instead")]
		public static String GetValue(string sqlStatement, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			return GetValue(sqlStatement, pc, connectionString);
		}

		public static String GetValue(string sqlStatement, Parameter selectParameter) {
			return GetValue(sqlStatement, selectParameter, null);
		}

		public static String GetValue(string sqlStatement, Parameter selectParameter, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(selectParameter);
			return GetValue(sqlStatement, pc, connectionString);
		}

		public static String GetValue(string SqlStatement, ParameterCollection selectParameters) {
			return GetValue(SqlStatement, selectParameters, null);
		}

		public static String GetValue(string SqlStatement, ParameterCollection selectParameters, string connectionString) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataReader;
			ds.CancelSelectOnNullParameter = false;
			if (connectionString + "" != "") {
				if (connectionString.Contains("Initial Catalog")) {
					ds.ConnectionString = connectionString;
				} else {
					ds.ConnectionString = GetConnectionString(connectionString);			//connectionString is the name of the connectionString field in web_connectionstrings.config
				}
			} else {
				ds.ConnectionString = GetConnectionString();
			}

			ds.Selecting += OnGetReaderSelecting;				//sort out the command timeout 
			ds.SelectCommand = SqlStatement;
			foreach (Parameter sp in selectParameters) {
				ds.SelectParameters.Add(sp);
			}
			String returnValue = "";

			try {
				using (SqlDataReader dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty)) {

					if (dr != null) {
						foreach (DbDataRecord view in dr) {
							returnValue = view[0].ToString();
							break;
						}
						dr.Close();
					}
				}
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetValue: " + e.Message + " in [" + SqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new ProgrammingErrorException(errorMessage, e);
			}

			return returnValue;
		}


		public static String GetValue(SqlDataSource sds) {
			String returnValue = "";
			sds.DataSourceMode = SqlDataSourceMode.DataReader;
			sds.Selecting += OnGetReaderSelecting;				//sort out the command timeout 

			try {
				using (SqlDataReader sdr = (SqlDataReader)sds.Select(DataSourceSelectArguments.Empty)) {
					if (sdr != null) {
						foreach (DbDataRecord dr in sdr) {
							returnValue = dr[0].ToString();
						}
					}
				}
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetValue: " + e.Message + " in [" + sds.SelectCommand + "]";
				foreach (Parameter p in sds.SelectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new ProgrammingErrorException(errorMessage, e);
			}


			return returnValue;
		}
		#endregion

		// nice clean way of accessing multiple values in one record
		#region GetValues
		/// <summary>
		/// nice clean way of accessing multiple values in one record
		/// </summary>
		/// <example>
		/// Hashtable ht = GetValues("SELECT...")
		/// something = ht["ProductID"]
		/// </example>
		public static Hashtable GetValues(Sql sqlStatement) {
			return GetValues(sqlStatement, (string)null);
		}
		public static Hashtable GetValues(Sql sqlStatement, string connectionString) {
			return GetValues(sqlStatement.ToString(), connectionString);
		}
		/// <summary>
		/// nice clean way of accessing multiple values in one record
		/// </summary>
		/// <example>
		/// Hashtable ht = GetValues("SELECT...")
		/// something = ht["ProductID"]
		/// </example>
		[Obsolete("Deprecated - use GetValues(Sql) instead")]
		public static Hashtable GetValues(string sqlStatement) {
			return GetValues(sqlStatement, (string)null);
		}
		public static Hashtable GetValues(string sqlStatement, string connectionString) {
			// if you deprecate this function, re-work it so that GetValues(Sql sqlStatement, string connectionString) doesn't just call it - get rid of the warning
			ParameterCollection pc = new ParameterCollection();
			return GetValues(sqlStatement, pc, connectionString);
		}
		/// <summary>
		/// nice clean way of accessing multiple values in one record
		/// </summary>
		/// <example>
		/// Hashtable ht = GetValues("SELECT...",param)
		/// something = ht["ProductID"]
		/// </example>
		public static Hashtable GetValues(string sqlStatement, Parameter selectParameter) {
			return GetValues(sqlStatement, selectParameter, null);
		}
		public static Hashtable GetValues(string sqlStatement, Parameter selectParameter, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(selectParameter);
			return GetValues(sqlStatement, pc, connectionString);
		}
		/// <summary>
		/// get one row of data in a hashtable accessible via: hashtableName["fieldname"]
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <param name="selectParameters"></param>
		/// <returns></returns>
		public static Hashtable GetValues(string sqlStatement, ParameterCollection selectParameters) {
			return GetValues(sqlStatement, selectParameters, null);
		}
		public static Hashtable GetValues(string sqlStatement, ParameterCollection selectParameters, string connectionString) {
			Hashtable returnValue = new Hashtable();

			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataReader;
			ds.CancelSelectOnNullParameter = false;

			// check to see if name of string is passed (no ; in the string)
			if (connectionString.IsNotBlank() && !connectionString.Contains(";")) {
				connectionString = GetConnectionString(connectionString);
			}

			ds.ConnectionString = String.IsNullOrEmpty(connectionString) ? GetConnectionString() : connectionString;
			ds.Selecting += OnGetReaderSelecting; // sort out the command timeout 
			ds.SelectCommand = sqlStatement;
			if (selectParameters != null && selectParameters.Count > 0) {
				foreach (Parameter sp in selectParameters) {
					ds.SelectParameters.Add(sp);
				}
			}
			try {
				using (SqlDataReader dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty)) {
					if (dr != null) {
						while (dr.Read()) {
							for (int i = 0; i < dr.FieldCount; i++) {
								// sql joins can return two fields of the same name
								try {
									returnValue.Add(dr.GetName(i), dr.GetValue(i).ToString());
								} catch (Exception)//dont add same key twice
								{ }

							}
						}
						dr.Close();
					}
				}
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetValues: " + e.Message + " in [" + sqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new ProgrammingErrorException(errorMessage, e);
			}
			return returnValue;
		}
		#endregion

		// simple wrappers for using parameters to insert, update and delete records
		#region InsertRecord
		private static int insertedId;
		public static int InsertRecord(Sql sql) {
			ParameterCollection pc = new ParameterCollection();
			return InsertRecord(sql.Value, pc);
		}
		[Obsolete("Deprecated - use InsertRecord(Sql) instead")]
		public static int InsertRecord(string sqlStatement) {
			ParameterCollection pc = new ParameterCollection();
			return InsertRecord(sqlStatement, pc);
		}
		public static int InsertRecord(string sqlStatement, Parameter insertParameter) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(insertParameter);
			return InsertRecord(sqlStatement, pc);
		}
		public static int InsertRecord(string sqlStatement, ParameterCollection insertParameters) {
			return InsertRecord(sqlStatement, insertParameters, null);
		}

		public static int InsertRecord(string sqlStatement, ParameterCollection insertParameters, string connectionStringName) {
			SqlDataSource ds = new SqlDataSource();
			if (String.IsNullOrEmpty(connectionStringName)) {
				ds.ConnectionString = GetConnectionString();
			} else {
				ds.ConnectionString = GetConnectionString(connectionStringName);
			}
			ds.InsertCommand = sqlStatement.Trim();
			ds.InsertCommand += ds.InsertCommand.EndsWith(";") ? "" : ";"; // make sure the sql statement always ends with a semicolon

			// get the newly inserted Id stuff
			ds.InsertCommand += " SELECT @InsertedId = @@Identity"; // SELECT @InsertedId = SCOPE_IDENTITY
			ds.Inserted += InsertRecord_Inserted;
			Parameter InsertedId = new Parameter("InsertedId", TypeCode.Int32);
			InsertedId.Direction = ParameterDirection.Output;
			ds.InsertParameters.Add(InsertedId);

			foreach (Parameter sp in insertParameters) {
				ds.InsertParameters.Add(sp);
			}

			try {
				ds.Insert();
			} catch (SqlException e) {
				string errorMessage = "ERROR in BewebData.InsertRecord:\r\n" + e.Message + "\r\nSQL: " + sqlStatement;
				foreach (Parameter sp in insertParameters) {
					errorMessage += "\r\n" + sp.Name + " = [" + sp.DefaultValue + "]";
				}

				throw new BewebDataException(errorMessage);
			}

			return insertedId;
		}
		private static void InsertRecord_Inserted(object sender, SqlDataSourceStatusEventArgs e) {
			if (e.Command.Parameters["@InsertedId"].Value != DBNull.Value) // an exception will be thrown if this is DBNull
			{
				insertedId = Convert.ToInt32(e.Command.Parameters["@InsertedId"].Value);
			}
		}
		#endregion
		#region UpdateRecord
		public static int UpdateRecord(Sql sqlStatement) {
			return UpdateRecord(sqlStatement.ToString(), (string)null);
		}
		public static int UpdateRecord(Sql sqlStatement, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			return UpdateRecord(sqlStatement.ToString(), pc, connectionString);
		}
		[Obsolete("Deprecated - use UpdateRecord(Sql) instead")]
		public static int UpdateRecord(string sqlStatement) {
			return UpdateRecord(sqlStatement, (string)null);
		}
		public static int UpdateRecord(string sqlStatement, string connectionString) {
			ParameterCollection pc = new ParameterCollection();
			return UpdateRecord(sqlStatement, pc, connectionString);
		}
		public static int UpdateRecord(string sqlStatement, Parameter updateParameter) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(updateParameter);
			return UpdateRecord(sqlStatement, pc);
		}
		public static int UpdateRecord(string sqlStatement, ParameterCollection updateParameters) {
			return UpdateRecord(sqlStatement, updateParameters, null);
		}
		public static int UpdateRecord(string sqlStatement, ParameterCollection updateParameters, string connectionString) {
			SqlDataSource ds = new SqlDataSource();
			if (connectionString + "" != "") {
				if (connectionString.Contains("Initial Catalog")) {
					ds.ConnectionString = connectionString;
				} else {
					ds.ConnectionString = GetConnectionString(connectionString);			//connectionString is the name of the connectionString field in web_connectionstrings.config
				}

			} else {
				ds.ConnectionString = GetConnectionString();
			}

			ds.UpdateCommand = sqlStatement;
			ds.Updated += CountAffected;
			foreach (Parameter sp in updateParameters) {
				ds.UpdateParameters.Add(sp);
			}
			try {
				ds.Update();
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.UpdateRecord:\r\n" + e.Message + "\r\nSQL: " + sqlStatement;
				foreach (Parameter sp in updateParameters) {
					errorMessage += "\r\n" + sp.Name + " = [" + sp.DefaultValue + "]";
				}
				errorMessage += "\r\nCheck the format of the connection string. (see dlog)";
				Logging.dlog(errorMessage);
				Logging.dlog("cs:" + ds.ConnectionString);
				throw new Exception(errorMessage);
			}
			return countAffectedRows;
		}

		#endregion
		#region DeleteRecord
		public static int DeleteRecord(Sql sqlStatement) {
			ParameterCollection pc = new ParameterCollection();
			return DeleteRecord(sqlStatement.ToString(), pc);
		}
		public static int DeleteRecord(string sqlStatement, Parameter deleteParameter) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(deleteParameter);
			return DeleteRecord(sqlStatement, pc);
		}
		public static int DeleteRecord(string sqlStatement, ParameterCollection deleteParameters) {
			return DeleteRecord(sqlStatement, deleteParameters, null);
		}
		public static int DeleteRecord(string sqlStatement, ParameterCollection deleteParameters, string connectionStringName) {
			SqlDataSource ds = new SqlDataSource();
			if (String.IsNullOrEmpty(connectionStringName)) {
				ds.ConnectionString = GetConnectionString();
			} else {
				ds.ConnectionString = GetConnectionString(connectionStringName);
			}
			ds.DeleteCommand = sqlStatement;
			ds.Deleted += CountAffected;
			foreach (Parameter p in deleteParameters) {
				ds.DeleteParameters.Add(p);
			}
			try {
				ds.Delete();
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.DeleteRecord:\r\n" + e.Message + "\r\nSQL: " + sqlStatement;
				foreach (Parameter sp in deleteParameters) {
					errorMessage += "\r\n" + sp.Name + " = [" + sp.DefaultValue + "]";
				}

				throw new Exception(errorMessage);
			}
			return countAffectedRows;
		}
		#endregion
		#region CountAffected
		private static int countAffectedRows = 0;
		private static void CountAffected(object sender, SqlDataSourceStatusEventArgs e) {
			countAffectedRows = e.AffectedRows;
		}
		#endregion

		// more powerful function, allows multiple SQL statements to be run at once
		#region ExecuteSQL
		[Obsolete("Deprecated - use UpdateRecord(Sql) instead in case of sql injection from user input")]
		public static void ExecuteSQL(string sqlStatement) {
			ExecuteSQL(sqlStatement, true);
		}

		/// <summary>
		/// Use this if there are no user inputs in the sql text, and you want to execute raw sql, else you should use ExecuteSQL(Sql sqlStatement)
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <param name="StringHasBeenCheckedAgainstSQLInjection"></param>
		public static void ExecuteSQL(string sqlStatement, bool StringHasBeenCheckedAgainstSQLInjection) {
			var sql = new Sql();
			sql.SuppressQuoteChecking = StringHasBeenCheckedAgainstSQLInjection;
			sql.Add(sqlStatement);
			ExecuteSQL(sql);
		}

		public static int ExecuteSQL(string sqlStatement, TransConnection transaction) {
			var affectedRows = 0;
			using (var command = transaction.CreateCommand(sqlStatement)) {
				try {
					affectedRows = command.ExecuteNonQuery();
				} catch (Exception e) {
					string errorMessage = "ERROR in BewebData.ExecuteSQL: " + e.Message + " in [" + sqlStatement + "]";
					throw new Exception(errorMessage);
				}
			}
			return affectedRows;
		}

		/// <summary>
		/// executes the sql given - can perform multiple statements all at once
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <param name="timeout">timeout for running the query in seconds</param>
		public static void ExecuteSQL(string sqlStatement, int timeout) {
			ExecuteSQL(sqlStatement, timeout, null);
		}

		public static void ExecuteSQL(string sqlStatement, int timeout, string connectionString) {
			if (!connectionString.Contains("Initial Catalog")) {
				connectionString = GetConnectionString(connectionString);			//connectionString is the name of the connectionString field in web_connectionstrings.config
			}

			SqlConnection conn = new SqlConnection(connectionString ?? GetConnectionString());
			SqlCommand command = new SqlCommand(sqlStatement, conn);
			command.CommandTimeout = timeout;
			conn.Open();
			try {
				command.ExecuteNonQuery();
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.ExecuteSQL: " + e.Message + " in [" + sqlStatement + "]";
				throw new Exception(errorMessage);
			} finally {
				conn.Close();
			}
		}

		/// <summary>
		/// Executes the sql given - can perform multiple statements all at once
		/// </summary>
		/// <param name="sqlStatement"></param>
		public static void ExecuteSQL(Sql sqlStatement) {
			SqlConnection conn = new SqlConnection(GetConnectionString());
			SqlCommand command = new SqlCommand(sqlStatement.ToString(), conn);
			conn.Open();
			try {
				command.ExecuteNonQuery();
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.ExecuteSQL: " + e.Message + " in [" + sqlStatement + "]";
				throw new Exception(errorMessage);
			} finally {
				conn.Close();
			}
		}
		#endregion

		// intelligently adds a WHERE clause to a SQL statement
		#region AppendWhereClause
		/// <summary>
		/// will append ANDs or ORs to a sql string
		/// </summary>
		/// <param name="sqlString">the SQL string you are building up (doesn't need to be complete SQL, could just be the WHERE portion)</param>
		/// <param name="clauseType">AND or OR</param>
		/// <param name="condition">Person = 'fred'</param>
		/// <returns>the clause appended</returns>
		public static string AppendWhereClause(string sqlString, string clauseType, string condition) {
			return AppendWhereClause(sqlString, clauseType, condition, false, "");
		}
		public static string AppendWhereClause(string sqlString, string clauseType, string condition, bool isInBrackets, string preBracketClauseType) {
			string returnValue = sqlString;
			string openBracket = isInBrackets ? "(" : "";
			string closeBracket = isInBrackets ? ")" : "";

			if (clauseType.ToLower() != "or") clauseType = "AND"; // defaults to AND, only other accepted value is OR
			if (preBracketClauseType.ToLower() != "or") preBracketClauseType = "AND"; // defaults to AND, only other accepted value is OR

			// only add something if there is a condition to add
			if (!String.IsNullOrEmpty(condition)) {
				if (sqlString.ToLower().Contains("WHERE ".ToLower()) || sqlString.ToLower().Contains("HAVING ".ToLower())) {
					if (isInBrackets && sqlString.EndsWith(")")) {
						// insert our new value
						returnValue = " " + returnValue.Insert(returnValue.Length - 1, " " + clauseType + " " + condition);
					} else {
						// if no brackets already exist, but this condition is in brackets then use the preBracketClauseType instead
						clauseType = (isInBrackets) ? preBracketClauseType : clauseType;
						returnValue += " " + clauseType + " " + openBracket + condition + closeBracket;
					}
				} else {
					// just add a WHERE - we don't worry about HAVINGs with this function
					returnValue += " WHERE " + openBracket + condition + closeBracket;
				}

			}
			return returnValue;
		}
		#endregion

		// Load a sql statement, then move to a random record, and return the value of the named column
		#region GetRandomRecord
		/// <summary>
		/// Load a sql statement, then move to a random record, and return the value of the named column
		/// </summary>
		/// <param name="sql">sql to execute</param>
		/// <returns>value of named column, from a random sql statement</returns>
		/// <example><%=GetRandomRecord("select * from Quote");%></example>
		public static DataRow GetRandomRecord(string sql) {
			DataSet ds = Beweb.BewebData.GetDataSet(sql);
			int numItems = ds.Tables[0].Rows.Count;
			float res = new Random().Next(200);
			res = (res / 200);//number less than 1

			int posn = (int)(res * (float)numItems);

			DataRow dr = ds.Tables[0].Rows[posn];
			return dr;
		}

		/// <summary>
		/// Load a sql statement, then move to a random record, and return the value of the named column
		/// </summary>
		/// <param name="sql">sql to execute</param>
		/// <param name="colName">value of this column will be returned</param>
		/// <returns>value of named column, from a random sql statement</returns>
		/// <example><%=GetRandomRecord("select * from Quote", "QuoteText");%></example>
		public static string GetRandomRecord(string sql, string colName) {
			DataRow dr = GetRandomRecord(sql);
			return dr[colName].ToString();
		}
		#endregion


		public static string FmtGlossarize(string text) {
			// this function gets the values from the Glossary table, finds any instances and replaces with links with tooltips

			if (String.IsNullOrEmpty(text)) {
				//' don't do anything
			} else {
				string pattern, sql = "SELECT Title, DescriptionHTML FROM Glossary";

				// get all the glossary terms
				//DataBlock rsGlossary = db.execute(sql);
				using (SqlDataReader rsGlossary = GetRecords(sql)) {
					if (rsGlossary.HasRows) {
						while (rsGlossary.Read()) {

							//' do not replace any text inside a tag
							//' ^[^<]* matches words with no tags at the start
							//' >[^<]* makes sure the previous tag has been closed
							//' text to look for
							//'RegularExpressionObject.Pattern = "(^[^<]*|>[^<]*)\b(" & rsGlossary("Word") & "(s|ed|es)?)\b"
							//' replace, use back-reference $1 so that we leave case of text as it is 
							//'text = RegularExpressionObject.Replace(text, "$1<a href='pb_glossary.asp#$2' class='glossary_link' title='" & Server.HTMLEncode(rsGlossary("Description")) & "'>$2</a>")

							//' only replace text if its just inside these tags: <p>|<li>|<td>|<b>|<i>|<u>|<strong>|<em>|<span>|<div>
							//' this was because we don't want to replace inside H1, H2 etc tags or anchors
							//' text to look for
							// set up the regex object
							//pattern = "(<p>|<li>|<td>|<b>|<i>|<u>|<strong>|<em>|<span>|<div>)[^<]* (" + rsGlossary.GetValue("Word") + "(s|ed|es)?) ";
							//pattern = "(" + rsGlossary.GetValue("Word") + ")";
							//pattern = "(^[^<]*|>[^<]*)\b(" +rsGlossary.GetValue("Word")+ "(s|ed|es)?)\b"; // from matts asp
							//pattern = "(<p>)[^<]* (" + rsGlossary.GetValue("Word") + "(s|ed|es)?) ";
							pattern = "((<p>|<li>|<td>|<b>|<i>|<u>|<strong>|<em>|<span>|<div>)[^<]*)\\b(" + rsGlossary["Word"] + "(s|ed|es)?)\\b";
							//"((<p>|<li>|<td>|<b>|<i>|<u>|<strong>|<em>|<span>|<div>)[^<]*)\b(" & rsGlossary("Word") & "(s|ed|es)?)\b";


							Regex RegularExpressionObject = new Regex(pattern, RegexOptions.IgnoreCase);
							//' replace, use back-reference $1 so that we leave case of text as it is 

							//text = RegularExpressionObject.Replace(text, "<a class=\"tooltip\" href=\"#\">$0<span>" + Server.HtmlDecode(rsGlossary.GetValue("Description")) + "</span></a>");
							//text = RegularExpressionObject.Replace(text, "$0<a class=\"tooltip\" href=\"#\">$1<span>" + Server.HtmlDecode(rsGlossary.GetValue("Description")) + "</span></a>");
							//RegularExpressionObject.Replace(text, "<span>$0</span>");
							//text = RegularExpressionObject.Replace(text, "$1<a href='pb_glossary.asp#$3' class='glossary_link' title='" & Server.HTMLEncode(rsGlossary("Description")) & "'>$3</a>")
							//text = RegularExpressionObject.Replace(text, "<a class=\"tooltip\" href=\"#\">$0<span>" + Server.HtmlDecode(rsGlossary.GetValue("Description")) + "</span></a>");

							text = RegularExpressionObject.Replace(text, "$1<a class=\"tooltip\" onmouseover=\"showTip(this)\" onmouseout=\"hideTip(this)\" href=\"#\">$3<dfn>" + rsGlossary["Description"] + "</dfn></a>");
							//text = RegularExpressionObject.Replace(text, "-0:$0-1:$1-2:$2-3:$3-4:$4-");
							//rsGlossary.movenext();//.MoveNext
						}
					} else {
						//' recordset returned no values - just return the original text below
					}
					rsGlossary.Close();
				}
				//Set rsGlossary = nothing	
				//Set RegularExpressionObject = nothing
			}//end if

			return text;
		}

		private static List<string> fieldsThatExist = new List<string>();

		public static bool FieldExists(string tablename, string fieldname) {
			if (fieldsThatExist.Contains(tablename + "." + fieldname)) {
				return true;
			}

			bool result = false;
			using (var reader = new Sql("select * from ", tablename.SqlizeName(), "where 0=1").GetReader()) {
				result = FieldExists(reader, fieldname);
				reader.Close();
				reader.Dispose();
			}
			if (result) {
				fieldsThatExist.Add(tablename + "." + fieldname);
			}
			return result;
		}

		public static bool FieldExists(DbDataReader reader, string fieldname) {
			for (int i = 0; i < reader.VisibleFieldCount; i++) {
				if (reader.GetName(i).ToLower() == fieldname.ToLower()) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if the specified table (or view/stored proc) exists.
		/// Cached within the request lifetime for performance. Pass true to skip cache.
		/// </summary>
		public static bool TableExists(string checktable) {
			return TableExists(checktable, false);
		}

		public static bool TableExists(string checktable, bool skipCache) {
			var tables = GetTableAndViewNames();
			return tables.ContainsInsensitive(checktable);

			//bool result = false;
			//SqlConnection conn = new SqlConnection(GetConnectionString());
			//conn.Open();

			//System.Data.DataTable dt = conn.GetSchema("Tables");
			//foreach (System.Data.DataRow row in dt.Rows) {
			//  string holdName = "";
			//  foreach (System.Data.DataColumn col in dt.Columns) {
			//    if (col.ColumnName.Equals("TABLE_NAME")) {
			//      holdName = row[col].ToString();
			//      //}

			//      //if (col.ColumnName.Equals("TABLE_TYPE") && row[col].Equals("TABLE"))
			//      //{
			//      if (checktable.ToLower() == holdName.ToLower()) {
			//        result = true;
			//        break;
			//      }
			//    }
			//  }
			//}
			//conn.Close();
			//return result;
		}

		public static List<string> GetTableAndViewNames() {
			return GetTableAndViewNames(false);
		}

		public static List<string> GetTableAndViewNames(bool skipCache) {
			if (Web.PageGlobals["BewebData_TableAndViewNames"] == null || skipCache) {
				var result = new List<string> { };
				using (var conn = CreateNewConnection()) {
					DataTable dt = conn.GetSchema("Tables");
					foreach (DataRow row in dt.Rows) {
						string tableName = row["TABLE_NAME"].ToString();
						result.Add(tableName);
					}
					conn.Close();
					result.Sort();
					Web.PageGlobals["BewebData_TableAndViewNames"] = result;
				}
			}
			return Web.PageGlobals["BewebData_TableAndViewNames"] as List<string>;
		}

		public static List<string> GetTableNames() {
			return GetTableNames(false);
		}

		public static List<string> GetTableNames(bool skipCache) {
			if (Web.PageGlobals["BewebData_TableNames"] == null || skipCache) {
				var result = new List<string> { };
				using (var conn = CreateNewConnection()) {
					DataTable dt = conn.GetSchema("Tables");
					foreach (DataRow row in dt.Rows) {
						if (row["TABLE_TYPE"].ToString() == "TABLE" || row["TABLE_TYPE"].ToString() == "BASE TABLE") {
							string tableName = row["TABLE_NAME"].ToString();
							result.Add(tableName);
						}
					}
					conn.Close();
					result.Sort();
					Web.PageGlobals["BewebData_TableNames"] = result;
				}
			}
			return Web.PageGlobals["BewebData_TableNames"] as List<string>;
		}

		public static string Lookup(string field, string table, string lookupcol, string lookupvalue) {
			return BewebData.GetValue(new Sql("select " + field + " from " + table + " where " + lookupcol + "=", lookupvalue.SqlizeText()));
		}
		public static string Lookup(string field, string table, string lookupcol, int lookupvalue) {
			return BewebData.GetValue(new Sql("select " + field + " from " + table + " where " + lookupcol + "=", lookupvalue));
		}

		public static int? GetValueInt(string sql) {
			return GetValueInt(new Sql("", sql.SqlizeRAW(true)), 0);
		}

		public static int? GetValueInt(Sql sql) {
			return GetValueInt(sql, 0);
		}

		public static int? GetValueInt(Sql sql, int defaultValue) {
			var result = BewebData.GetValue(sql);
			if (result == "") result = "" + defaultValue;
			try {
				return (result == null) ? 0 : Convert.ToInt32(result);
			} catch (FormatException ex) {
				throw new Exception("result[" + result + "] " + ex.Message);
			}
		}
		/// <summary>
		/// call getvalueint
		/// </summary>
		/// <param name="sql">return a single value from the sql call</param>
		/// <returns></returns>
		public static int? GetInt(string sql) {
			return GetValueInt(sql);

		}

		public static string GetDelimitedRecordString(string sql, string delim) {
			string str = "";
			using (var reader = Beweb.BewebData.GetRecords(sql)) {
				foreach (DbDataRecord row in reader) {
					if (str != "") str += delim;
					str += row[0].ToString();
				}
				reader.Close();
				reader.Dispose();
			}
			return str;
		}
#if FileSystem
		/// <summary>
		/// Read files from the file system to create the database
		/// to invoke pass ?data=tablename to a list page 
		/// <example>
		/// admin/contactlist.aspx?data=contact
		/// </example>
		/// 
		/// </summary>
		public static void StructureCheck() {
			string path = HttpContext.Current.Server.MapPath(".");
			if (HttpContext.Current.Request["data"] + "" == "") return;
			string tableName = HttpContext.Current.Request["data"];

			FileSystem fs = FileSystem.OpenTextFile(path + "\\" + tableName + "_create.sql");
			string exec;
			string[] arr;


			if (fs.Exists()) {
				exec = fs.ReadAll();
				arr = exec.Replace("\r\n", "").Split(';');
				foreach (string frag in arr) {
					BewebData.ExecuteSQL(frag, true);
				}
			} else {
				fs = FileSystem.OpenTextFile(path + "\\" + tableName + "_update.sql");
				if (fs.Exists()) {
					exec = fs.ReadAll();
					arr = exec.Replace("\r\n", "").Split(';');
					foreach (string frag in arr) {
						BewebData.ExecuteSQL(frag, true);
					}
				}
			}
			fs = FileSystem.OpenTextFile(path + "\\" + tableName + "_defaultdata.sql");
			if (fs.Exists()) {
				exec = fs.ReadAll();
				arr = exec.Replace("\r\n", "").Split(';');
				foreach (string frag in arr) {
					BewebData.ExecuteSQL(frag, true);
				}
			} else if (FileSystem.FileExists(path + "\\" + tableName + "_defaultdata.csv")) {
				ImportCSV(path, tableName);
			} else if (FileSystem.FileExists(path + "\\" + tableName + "_defaultdata.tsv")) {
				ImportFile(path, tableName, "tsv", false);
			} else {
				// no file to import
			}
		}

		private static void ImportCSV(string path, string tableName) {
			ImportFile(path, tableName, "csv", true);
		}

		private static void ImportFile(string path, string tableName, string extn, bool preserveIdent) {
#if datablock_backcompat
			var fs = FileSystem.OpenTextFile(path + "\\" + tableName + "_defaultdata." + extn);
			if (fs.Exists()) {
				string text = fs.ReadAll();
				var arr = text.Split('\n');
				DataBlock db = new DataBlock();
				db.OpenDB();
				OleDbCommand sqlCmd = new OleDbCommand();

				sqlCmd.Connection = db.sqlConn;
				string sql = "";
				sql += "truncate table " + tableName + "; ";
				sql += "set identity_insert " + tableName + " on; ";
				foreach (string line in arr) {
					db.TableName = tableName;
					db.Load(line.Split('\t'), preserveIdent);
					//db.Create();
					sql += db.GetSqlInsertStatement(tableName);
				}
				sql += "set identity_insert " + tableName + " off; ";
				sqlCmd.CommandText = sql;
				//LastCreateSQL = sql;
				//try
				//{
				//Trace.Write("Exec sql["+sql+"]");
				Logging.trace("Exec sql[" + sql + "]");
				Logging.dlog("Exec sql[" + sql + "]");
				int numAffected = sqlCmd.ExecuteNonQuery();

				Logging.trace("res rows[" + numAffected + "]");
				Logging.dlog("res rows[" + numAffected + "]");
				//}catch(Exception ex)
				//{
				//  Trace.Write("Exec sql failed ["+ex.Message+"]");
				//}
				db.CloseDB();

				System.IO.FileInfo fi = new System.IO.FileInfo(fs.FileName);
				fs.Rename(fi.DirectoryName + "\\" + Fmt.DateTimeCompressed(DateTime.Now) + " " + fi.Name);
			}
#endif
		}
#endif
	}

	[Serializable]
	public class BewebDataException : Exception {
		public BewebDataException() { }
		public BewebDataException(string message) : base(message) { }
		public BewebDataException(string message, Exception innerException) : base(message, innerException) { }
		protected BewebDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

}
#if TestExtensions

namespace BewebTest {
	[TestClass]
	public class BewebDataTest {


		[TestMethod]
		public void TestGetTableNames() {
			Assert.IsTrue(BewebData.TableExists("person"), "Person table exists");
			Assert.IsTrue(BewebData.TableExists("ModificationLog"), "Person table exists");
			var start = DateTime.Now;
			for (int i = 0; i < 1000; i++) {
				var hash = BewebData.TableExists("person", true);
			}
			Web.Write(start.FmtMillisecondsElapsed());
			Assert.IsTrue((DateTime.Now - start).TotalMilliseconds < 100, "Speedy TableExists");
		}
		
		[SlowTestMethod]
		public void TestGetValuesConnectionsNotLeaking() {
			for (int i = 0; i < 10000; i++) {
				var hash = BewebData.GetValues(new Sql("select * from person order by newid()"));
				if (i % 100 == 0) {
					Web.Write(i + " ");
					Web.Flush();
				}
			}
			Assert.Pass();
		}

		[SlowTestMethod]
		public void TestGetValueConnectionsNotLeaking() {
			for (int i = 0; i < 10000; i++) {
				var name = BewebData.GetValue(new Sql("select firstname from person order by newid()"));
				if (i % 100 == 0) {
					Web.Write(i + " ");
					Web.Flush();
				}
			}
			Assert.Pass();
		}

		[SlowTestMethod]
		public void TestGetDelimitedRecordStringConnectionsNotLeaking() {
			for (int i = 0; i < 10000; i++) {
				var name = BewebData.GetDelimitedRecordString("select firstname from person order by newid()", ",");
				if (i % 100 == 0) {
					Web.Write(i + " ");
					Web.Flush();
				}
			}
			Assert.Pass();
		}


		[TestMethod]
		public void TestFieldExists() {
			Assert.AreEqual(false, BewebData.FieldExists("person", "nothere"));
			Assert.AreEqual(true, BewebData.FieldExists("person", "firstname"));
		}

	}
}
#endif

