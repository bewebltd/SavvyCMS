//
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Beweb {


	/// <summary>
	/// Perform all the functions of a typical recordset
	/// </summary>
	/// 
	/// 
	public class DataBlock : DataSet {
		public enum DatabaseType {
			NotSet,
			MSAccess,
			MSSQLServer
		};
		public DatabaseType currentDatabaseType;
		public DataView currView = null;
		public int RecordCount = -1;
		public bool readerOpen = false;
		public string connString = "not set";
		public string lastSQL = "not set";
		public string LastError = "not set";
		public int NumberOfColumns = -1;
		public int RecordsAffected = -1; //set by db.execute
		protected bool isInitialized = false;
		protected bool moreRecordsAvailable = false;
		protected DataTable saveTable; //used if adding / editing a row in a table
		protected OleDbDataReader sqlReader = null; // current reader, with position
		//protected SqlDataReader sqlReader = null; // current reader, with position

		protected int curIndex = -1;
		public OleDbConnection sqlConn = null;
		//public SqlConnection sqlConn = null;
		//private string serverPath = "not set";



		protected int _LastInsertedID = -1;
		public int LastInsertedID {
			get { return _LastInsertedID; }
			set { _LastInsertedID = value; }
		}
		protected string _TableName = null;
		public string TableName {
			get { return _TableName; }
			set {
				_TableName = value;
				//create schema for that table (used for load, save, etc)

				CreateSchemaFromTable(_TableName);
			}
		}

		private void CreateSchemaFromTable(string _TableName) {
			OleDbCommand sqlCmd = new OleDbCommand();
			OleDbDataReader sqlReaderOpen;

			sqlCmd.Connection = this.sqlConn;
			sqlCmd.CommandText = "select top 1 * from " + _TableName + " where 1=0";
			try {
				sqlReaderOpen = sqlCmd.ExecuteReader();
				int NumberOfColumns = sqlReaderOpen.FieldCount;
				for (int fieldScan = 0; fieldScan < sqlReaderOpen.FieldCount; fieldScan++) {
					Columns[fieldScan] = new SchemaInfo();
					Columns[fieldScan].ColumnName = sqlReaderOpen.GetName(fieldScan);
					Columns[fieldScan].ColumnType = sqlReaderOpen.GetDataTypeName(fieldScan);
				}
			} catch (Exception ex) {
				throw new ProgrammingErrorException("Failed to CreateSchemaFromTable. Table was ["+_TableName+"] " , ex);
			}

		}

		/// <summary>
		/// index of the current record (zero based)
		/// </summary>
		public int CurrentIndex {
			get { return curIndex; }
			set { curIndex = value; }
		}

		/// <summary>
		/// list of columns in the datablock with their names and types
		/// </summary>
		public SchemaInfo[] Columns = new SchemaInfo[999];


		/// <summary>
		/// constructor
		/// </summary>
		public DataBlock() {

		}
		~DataBlock() {
			if (readerOpen) throw new Exception("Reader is still open after an execute call.");
		}

		/// <summary>
		/// constructor, set conn string, but dont connect
		/// </summary>
		/// <param name="connectionString"></param>
		public DataBlock(string connectionString) {
			connString = connectionString;
		}
		public DataBlock(string connectionString, bool open) {
			connString = connectionString;
			if (open) OpenDB(connectionString);
		}


		/// <summary>
		/// Open this datablock. assumes that the connection is already set
		/// </summary>
		/// <returns>true if no error</returns>
		public bool OpenDB() {
			return OpenDB("");
		}
		public bool IsConnectionClosed() {
			return (sqlConn.State == ConnectionState.Closed);
		}

		/// <summary>
		/// open this datablock
		/// </summary>
		/// <param name="dbConnStr"></param>
		/// <returns>true if no error</returns>		
		public bool OpenDB(string dbConnStr) {
			bool result = false;
			currentDatabaseType = DatabaseType.MSSQLServer;

			if (dbConnStr == "") dbConnStr = BewebData.GetConnectionString();
			if (dbConnStr.IndexOf(".mdb") > 0) {
				string pathToDB = Util.FindVirtualPath("_database|database", true);
				string physPath = Util.FindPhysicalPath(pathToDB);
				dbConnStr = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + physPath + "\\" + dbConnStr;
				dbConnStr = dbConnStr.Replace("/", "\\");
				dbConnStr = dbConnStr.Replace("\\\\", "\\");
				currentDatabaseType = DatabaseType.MSAccess;
			}


			if (dbConnStr == null || dbConnStr == "") {
				dbConnStr = connString; //use class property string
			}
			if (dbConnStr != null && dbConnStr != "") {
				//ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["ConnectionString"];
				//dbObjConnStr = cs.ConnectionString;

				if (dbConnStr.IndexOf("SQLOLEDB") < 0 && dbConnStr.IndexOf(".mdb") < 0) dbConnStr += ";Provider=SQLOLEDB;";

				connString = dbConnStr;

				if (sqlConn != null && sqlConn.State == ConnectionState.Open)//shared connection may already be open
				{
					//leave connection alone
				} else {
					//open connection
					sqlConn = new OleDbConnection(dbConnStr); //open single shared connection

					for (int retryCount = 0; retryCount < 6; retryCount++) {
						//sqlConn.Open();
						try {
							sqlConn.Open();
							result = true;
							break;
						} catch (Exception ex) {
							if (retryCount >= 5) {
								// MN 20130522 - changed to exception only so normal error reporting and friendly error page kicks in
								throw new Exception("Datablock InitCMA: Failed to open database connection");
								//response.redirect
								//eout("<!--InitCMA: Connection error["+ex.Message+"]["+dbConnStr+"]-->");
								//break;
							} else {
								System.Threading.Thread.Sleep(500); //half a second

							}
						}
					}//retry loop
					if (sqlConn.State == ConnectionState.Open) {
						if (HttpContext.Current.Application != null && HttpContext.Current.Application["sqlConnectionCount"] == null) {
							HttpContext.Current.Application.Add("sqlConnectionCount", (object)1);
						} else {
							HttpContext.Current.Application["sqlConnectionCount"] = ((int)HttpContext.Current.Application["sqlConnectionCount"]) + 1;
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Close the current connection
		/// </summary>
		/// <returns></returns>
		public bool CloseDB() {
			//if(sqlReader==null&&readerOpen)throw new Exception("try to close db while reader is open");
			try {
				if (sqlConn != null && sqlConn.State == ConnectionState.Open) sqlConn.Close();
			} catch (Exception) {
				//close failed
			}
			return true;
		}


		/// <summary>
		/// Print out the current field names and values to dout, current index
		/// <returns>output form dump</returns>
		/// </summary>
		public string DumpFields() {
			int numcols = GetNumberOfColumns();
			Logging.dout("dumpfields");
			Logging.dout("----------");
			string msg = "";
			for (int sc = 0; sc < numcols; sc++) {
				string name = getColumnName(sc);
				msg += "col[" + name + "], val[" + GetValue(name) + "]\n";
				Logging.dout(msg);

			}
			Logging.dout("----------");
			return msg;
		}

		/// <summary>
		/// Create a new datablock using the current open connection 
		/// 
		/// Read all data from the query into memory
		/// </summary>
		/// <param name="sqlText">sql to execute</param>
		/// <returns>a datablock</returns>
		public DataBlock open(string sqlText) {
			DataTable dt = new DataTable();
			//DataRow dr;
			int scan = 0;

			DataBlock dataObject = new DataBlock(); //create a new dataobject to return
			dataObject.sqlConn = this.sqlConn;
			dataObject.currentDatabaseType = this.currentDatabaseType;

			//DataBlock dataObject = this;				// dont do that!

			dataObject.lastSQL = sqlText;
			//dataObject.CreateDataSource(sqlText, this.connString); // dont call that - it does just about the same as execute does!

			//OleDbConnection sqlConn = new OleDbConnection(connString);
			//try{sqlConn.Open();} catch (Exception ex){if (currCMA != null) Debug.eout("Connection error [" + sqlText + "], ["+ex.Message+"]");}

			OleDbCommand sqlCmd = new OleDbCommand();
			OleDbDataReader sqlReaderOpen;

			sqlCmd.Connection = this.sqlConn;
			sqlCmd.CommandText = sqlText;
			try {
				sqlReaderOpen = sqlCmd.ExecuteReader();
				dataObject.NumberOfColumns = sqlReaderOpen.FieldCount;
				dataObject.curIndex = 0; //first record

				//add names / types of	fields to schema info array

				for (int fieldScan = 0; fieldScan < sqlReaderOpen.FieldCount; fieldScan++) {
					dataObject.Columns[fieldScan] = new SchemaInfo();
					dataObject.Columns[fieldScan].ColumnName = sqlReaderOpen.GetName(fieldScan);
					//Columns[fieldScan].ColumnSize = sqlReaderOpen.get.GetName(fieldScan);
					dataObject.Columns[fieldScan].ColumnType = sqlReaderOpen.GetDataTypeName(fieldScan);
				}

				// Always call Read before accessing data.
				while (sqlReaderOpen.Read()) {
					/*
					if (scan == 0) // only run this on first row
					{
						for (int fieldScan = 0; fieldScan < sqlReaderOpen.FieldCount; fieldScan++)
						{
							string colName = sqlReaderOpen.GetName(fieldScan);
							dt.Columns.Add(new DataColumn(colName));
						}
					}

					dr = dt.NewRow();
					for (int fieldScan = 0; fieldScan < sqlReaderOpen.FieldCount; fieldScan++)
					{
						string colName = sqlReaderOpen.GetName(fieldScan);
						dr[colName] = sqlReaderOpen[colName];
					}
					*/
					dt.Rows.Add(GetDataRow(dataObject, sqlReaderOpen, dt, (scan == 0)));
					scan++;
				}
				// always call Close when done reading.
				sqlReaderOpen.Close();

			} catch (OleDbException ex) {
				Console.Write("Error [" + ex.Message + "], sql[" + sqlText + "]");
				Logging.eout("Error db ex 1 [" + ex.Message + "], sql[" + sqlText + "]");
			} catch (Exception ex) {
				Console.Write("Error [" + ex.Message + "], sql[" + sqlText + "]");
				Logging.eout("Error db ex 2 [" + ex.Message + "], sql[" + sqlText + "]");
			}
			// Close the connection.
			//sqlCmd.Connection.Close(); dont close shared connection

			DataView dv = new DataView(dt);
			dataObject.currView = dv;
			dataObject.RecordCount = scan;

			return dataObject;
		}


		/// <summary>
		/// firehose read of data, open the reader, read a record, but 
		/// dont close it. note that you need to call rs.close() when using this. if not call, exception will fire on datablock destructor
		/// </summary>
		/// <param name="sqlText"></param>
		/// <returns></returns>
		public DataBlock execute(string sqlText) {
			readerOpen = true;
			DataTable dt = new DataTable();

			DataBlock dataObject = new DataBlock(); //create a new dataobject to return
			//DataBlock dataObject = this;				// dont do that!

			dataObject.lastSQL = sqlText;
			//dataObject.CreateDataSource(sqlText, this.connString); // dont call that - it does just about the same as execute does!

			//OleDbConnection sqlConn = new OleDbConnection(connString);
			//try{sqlConn.Open();} catch (Exception ex){if (currCMA != null) Debug.eout("Connection error [" + sqlText + "], ["+ex.Message+"]");}

			OleDbCommand sqlCmd = new OleDbCommand();

			sqlCmd.Connection = this.sqlConn;
			sqlCmd.CommandText = sqlText;
			try {
				dataObject.sqlReader = sqlCmd.ExecuteReader();

				dataObject.NumberOfColumns = dataObject.sqlReader.FieldCount;

				// Always call Read before accessing data.


				dataObject.moreRecordsAvailable = dataObject.sqlReader.Read(); //read first record. Note that movenext reads the other records
				dataObject.curIndex = 0; //first record
				//this section should be called by movenext, uncluding setting current view to dv
				//
				if (dataObject.NumberOfColumns > 0 && dataObject.moreRecordsAvailable) {
					dt.Rows.Add(GetDataRow(dataObject, dataObject.sqlReader, dt, true));
				}

			} catch (Exception ex) {
				throw new ProgrammingErrorException("SQL Execute Error [" + ex.Message + "], sql[" + sqlText + "]", ex);
			}
			// Close the connection.
			//sqlCmd.Connection.Close(); dont close shared connection

			DataView dv = new DataView(dt);
			dataObject.currView = dv;
			dataObject.RecordCount = -1; // no record count for reading forward

			return dataObject;
		}

		/// <summary>
		/// used by execute to create the datacol, schema info, fill cols
		/// </summary>
		/// <param name="dataObject"></param>
		/// <param name="sqlReaderGet"></param>
		/// <param name="dt"></param>
		/// <param name="addColsToDT"></param>
		/// <returns></returns>
		protected DataRow GetDataRow(DataBlock dataObject, OleDbDataReader sqlReaderGet, DataTable dt, bool addColsToDT) {
			DataRow dr = dt.NewRow();

			//add names / types of	fields to schema info array
			for (int fieldScan = 0; fieldScan < sqlReaderGet.FieldCount; fieldScan++) {
				string colName = sqlReaderGet.GetName(fieldScan);
				dataObject.Columns[fieldScan] = new SchemaInfo();
				dataObject.Columns[fieldScan].ColumnName = colName;
				dataObject.Columns[fieldScan].ColumnType = sqlReaderGet.GetDataTypeName(fieldScan);

				if (addColsToDT) {
					// add cols to datacolumn
					dt.Columns.Add(new DataColumn(colName));
				}
				try {
					dr[colName] = sqlReaderGet[colName];
				} catch (Exception ex) {
					Console.Write("Error [" + ex.Message + "]");
					Logging.eout("Error GetDataRow [" + ex.Message + "] - [" + colName + "]is the col in the sql twice?");
				}
			}

			//for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++)
			//{
			//  string colName = sqlReader.GetName(fieldScan);
			//  dr[colName] = sqlReader[colName];
			//}
			return dr;
		}
		/// <summary>
		/// sql server version of execute, using SqlConnection
		/// </summary>
		/// <param name="sqlText">eg "select * from page"</param>
		/// <param name="sqlServerConnString">eg Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;</param>
		/// <returns></returns>
		public DataBlock executeSQL(string sqlText, string sqlServerConnString) {
			DataTable dt = new DataTable();
			DataRow dr;
			int scan = 0;

			DataBlock dataObject = new DataBlock();
			dataObject.lastSQL = sqlText;

			SqlConnection sqlConn = new SqlConnection(connString);
			try { sqlConn.Open(); } catch (Exception ex) { Logging.eout("Connection error [" + sqlText + "], [" + ex.Message + "]"); }

			SqlCommand sqlCmd = new SqlCommand();
			SqlDataReader sqlReader;

			sqlCmd.Connection = sqlConn;
			sqlCmd.CommandText = sqlText;
			try {
				sqlReader = sqlCmd.ExecuteReader();
				NumberOfColumns = sqlReader.FieldCount;
				// Always call Read before accessing data.
				while (sqlReader.Read()) {
					if (scan == 0) // only run this on first row
					{
						for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
							string colName = sqlReader.GetName(fieldScan);
							dt.Columns.Add(new DataColumn(colName));
						}
					}

					dr = dt.NewRow();
					for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
						string colName = sqlReader.GetName(fieldScan);
						dr[colName] = sqlReader[colName];
					}
					dt.Rows.Add(dr);
					scan++;
				}
				// always call Close when done reading.
				sqlReader.Close();
			} catch (SqlException ex) {
				Console.Write("Error [" + ex.Message + "], sql[" + sqlText + "]");
				Logging.eout("Error ex sql [" + ex.Message + "], sql[" + sqlText + "]");
			} finally {
				// Close the connection.
				sqlCmd.Connection.Close();
			}
			DataView dv = new DataView(dt);
			dataObject.currView = dv;
			dataObject.RecordCount = scan;

			return dataObject;
		}

		/// <summary>
		/// Shortcut to getvalue, but always returns a string
		/// override the [] operators
		/// rs['startdate'].ToLongDate()
		/// rs['startdate'] = DataField(5)
		/// rs['startdate'] = DataField('blafdlk')
		/// rs['startdate'] = DataField(DateTime.now)
		/// </summary>
		/// <param name="colName">column name in question</param>
		/// <returns></returns>
		public string this[string colName] {
			get { return this.GetValue(colName); }
			set { SetValue(colName, value); }
		}

		public string this[int colIndex] {
			get { return this.GetValue(colIndex); }
			set { SetValue(colIndex, value); }
		}



		//sub MoveToRandomRecord(rs, recordCount)
		//	' chooses a random record and moves to it
		//	dim numberPicked, i
		//	randomize
		//	numberPicked = int(rnd(timer) * recordCount)
		//	i = 0
		//	do while not rs.eof and i < numberPicked
		//		i = i + 1
		//		rs.movenext
		//	loop
		//end sub

		/// <summary>
		///	chooses a random record and moves to it
		/// </summary>
		public void MoveToRandomRecord() {
			//' chooses a random record and moves to it
			int numberPicked;
			numberPicked = (int)(VB.rnd() * RecordCount);
			numberPicked = (numberPicked > 1) ? numberPicked : 1;
			this.curIndex = numberPicked;
		}


		/// <summary>
		/// return true if at end of rs or zero records - no more records
		/// </summary>
		/// <returns></returns>
		public bool eof() {
			if (!moreRecordsAvailable) {
				return true;
			}
			return (RecordCount == 0) ? true : this.curIndex == this.RecordCount;
		}
		/// <summary>
		/// return true if at start of rs
		/// </summary>
		/// <returns></returns>
		public bool bof() {
			return this.curIndex == 0;
		}
		/// <summary>
		/// move to the next record in the rs (used by getvalue)
		/// </summary>
		/// <returns>true if more records to read</returns>
		public bool movenext() {
			//if(sqlReader==null) throw new Exception("movenext called when using an open, use execute instead");
			curIndex++;
			if (sqlReader != null) {
				DataTable dt = new DataTable();
				try {
					moreRecordsAvailable = sqlReader.Read();
				} catch (OleDbException ex) {
					Console.Write("Error [" + ex.Message + "], sql[" + this.lastSQL + "]");
					Logging.eout("Error db ex 3b [" + ex.Message + "], sql[" + this.lastSQL + "]");
				} catch (Exception ex) {
					Console.Write("Error [" + ex.Message + "], sql[" + this.lastSQL + "]");
					Logging.eout("Error db ex 4b [" + ex.Message + "], sql[" + this.lastSQL + "]");
				}
				if (moreRecordsAvailable) {
					dt.Rows.Add(GetDataRow(this, this.sqlReader, dt, true));
					DataView dv = new DataView(dt);
					this.currView = dv;
				} else {
					RecordCount = curIndex; // we got to the end, set the record count
				}
			}
			return !eof();
		}

		/// <summary>
		/// move to first record
		/// </summary>
		/// <returns>true if there is a first record</returns>
		public bool movefirst() {
			curIndex = 0;
			//if(sqlReader==null) throw new Exception("movefirst called when using an open, use execute instead");

			return (RecordCount > 0);
		}

		public string getColumnName(int columnIndex) {
			//string result = currView.Table.Columns[columnIndex].ToString();
			//get from local schema info built in createdatasource
			string result = Columns[columnIndex].ColumnName;
			return result;
		}

		/// <summary>
		/// assumes data in the current view
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public int getColumnIndex(string columnName) {
			int result = -1;
			try {
				result = currView.Table.Columns[columnName].Ordinal;
			} catch (Exception) {
				//not used
				//if (currCMA != null) 
				//{
				//  this.Debug.dout("getColumnIndex: columnName["+columnName+"]ex[" + ex.Message + "]");
				//}
			}
			return result;
		}

		public string getColumnTypeName(string columnName) {
			return getColumnTypeName(getColumnIndex(columnName));
		}

		public string getColumnTypeName(int columnIndex) {
			//string result = Columns[columnIndex].ColumnType; //this is the raw type name 

			string result = "";
			if (columnIndex > 0 && currView.Table.Columns.Count > columnIndex) {
				result = currView.Table.Columns[columnIndex].DataType.ToString(); //this is the cs object type System.String
			}
			return result;
		}


		public int GetNumberOfColumns() {
			int result = NumberOfColumns;
			return result;
		}

		/// <summary>
		/// Given a column name, get the first record from that column as a string
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns>string value</returns>
		public string GetValue(string columnName) {
			string result = "";
			if (columnName != null && currView != null && currView.Table.Columns.Count > 0) {
				try {
					int colIndex = currView.Table.Columns[columnName].Ordinal;
					result = GetValue(this.curIndex, colIndex);
				} catch (Exception ex) {
					//if (currCMA != null) Debug.eout("getValue: exception trying to get col [" + columnName + "]: [" + ex.Message + "] st[" + ex.StackTrace.ToString() + "]");

					string msg = "GetValue: columnName[" + columnName + "] ex[" + ex.Message + "] ";
					Logging.trace(msg + " - sql[" + this.lastSQL + "]");
					throw new ProgrammingErrorException(msg,ex);

				}
			}
			return result;

		}

		/// <summary>
		/// return the value of the current data (index) as an int
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns>int</returns>
		public int GetValueInt(string columnName) {
			return GetValueInt(this.curIndex, columnName);
		}


		/// <summary>
		/// same as GetValueIsTrue
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public bool GetBool(string columnName) {
			return GetValueIsTrue(columnName);
		}


		public int GetValueInt(int rowIndex, string columnName) {
			int result = 0;
			result = VB.cint(GetValue(rowIndex, columnName));
			return result;
		}
		public DateTime GetValueDate(string columnName) {
			string dv = GetValue(columnName);
			return (dv == "") ? VB.DateNull : DateTime.Parse(dv);
		}
		public DateTime GetValueDate(int colIndex) {
			string result = GetValue(colIndex);
			return (result == "") ? VB.DateNull : DateTime.Parse(result);
		}
		public int GetValueInt(int index) {
			return Convert.ToInt32(GetValue(index));
		}
		/// <summary>
		///	return true if the value is true, convert the db value (1, bit, etc) to a boolean
		/// </summary>
		/// <param name="columnName">name of column to read</param>
		/// <returns>true (1,true) or false (null, 0)</returns>
		public Boolean GetValueIsTrue(string columnName) {
			return (GetValue(columnName) == "True");
		}
		/// <summary>
		/// return first col, current index, as an int assuming it's the pk
		/// </summary>
		/// <param name="colIndex"></param>
		/// <returns></returns>
		public int GetID(int colIndex) {
			return Convert.ToInt32(GetValue(this.curIndex, 0));
		}
		public string GetValue(int colIndex) {
			return GetValue(this.curIndex, colIndex);
		}

		public string GetValue(int rowIndex, string columnName) {
			string result = "";

			try {
				int colIndex = currView.Table.Columns[columnName].Ordinal;
				result = GetValue(rowIndex, colIndex); ;
			} catch (Exception ex) {
				Logging.eout("Exception trying to get col [" + columnName + "]: [" + ex.Message + "] st[" + ex.StackTrace.ToString() + "] sql[" + this.lastSQL + "]");
			}
			return result;
		}

		public object GetValueObj(int rowIndex, int columnIndex) {
			object result = null;
			try {
				result = currView.Table.Rows[rowIndex].ItemArray[columnIndex];
			} catch (Exception ex) {
				// failed 
				Logging.eout("gvo: Exception trying to get value at rowIndex[" + rowIndex + "], columnIndex[" + columnIndex + "]: [" + ex.Message + "] st[" + ex.StackTrace.ToString() + "] sql[" + this.lastSQL + "]");
			}
			return result;
		}

		public string GetValue(int rowIndex, int columnIndex) {
			string result = "";
			if (currView.Table.Rows.Count > 0) {
				try {
					if (this.sqlReader != null) rowIndex = 0; // set to zero as we are reading records (hosepipe rs.execute as opposed to rs.open)
					result = currView.Table.Rows[rowIndex].ItemArray[columnIndex].ToString();
				} catch (Exception ex) {
					// failed 
					Logging.eout("gv: Exception trying to get value at rowIndex[" + rowIndex + "], columnIndex[" + columnIndex + "]: [" + ex.Message + "] st[" + ex.StackTrace.ToString() + "], sql[" + this.lastSQL + "]");
				}
			}
			return result;
		}

		/// <summary>
		/// note: always return a new databobject - not the 'this' obj
		/// </summary>
		/// <param name="sqlText"></param>
		/// <returns></returns>
		public string FetchValue(string sqlText) {
			DataBlock dataObject = new DataBlock();
			dataObject.sqlConn = this.sqlConn;
			dataObject.lastSQL = sqlText;
			dataObject.CreateDataSource(sqlText);
			//dataObject = dataObject.open(sqlText);
			return dataObject.GetValue(0, 0);
		}

		public int FetchValueInt(string sqlText) {
			int result = 0; string hold = "";
			try {
				hold = FetchValue(sqlText);
				result = (hold != "") ? Convert.ToInt32(hold) : 0;
			} catch (Exception e) {
				throw new ProgrammingErrorException("error[" + sqlText + "]hold[" + hold + "][" + e.Message + "]",e);
			}
			return result;
		}

		public string Lookup(string field, string table, string lookupcol, string lookupvalue, string connStr) {
			return FetchValue("select " + field + " from " + table + " where " + lookupcol + "=" + lookupvalue + "");
		}

		/// <summary>
		/// get the raw data type from a table, fieldname
		/// </summary>
		/// <param name="tableName">table to open</param>
		/// <param name="fieldName">field name to get data type from</param>
		/// <returns></returns>
		public SchemaInfo GetSchemaInfo(string tableName, string fieldName) {
			string sqlText = "select top 1 " + fieldName + " from " + tableName + "";
			DataTable dt = new DataTable();
			string colType = "";
			int sizeInt = -1;
			SchemaInfo sc = new SchemaInfo();

			if (connString != null && connString != "") {
				OleDbCommand sqlCmd = new OleDbCommand();
				OleDbDataReader sqlReader;

				// Assign transaction object for a pending local transaction
				//sqlCmd.Connection = currCMA.sqlConn;
				sqlCmd.Connection = sqlConn; //use local datablock connection
				//sqlCmd.Transaction = sqlTrans;
				sqlCmd.CommandText = sqlText;
				try {
					sqlReader = sqlCmd.ExecuteReader();
					for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
						//string fieldName = sqlReader.
						string colName = sqlReader.GetName(fieldScan);
						colType = sqlReader.GetDataTypeName(fieldScan);
						DataTable schemaData = sqlReader.GetSchemaTable();
						NumberOfColumns = schemaData.Columns.Count;
						string size = schemaData.Rows[0]["ColumnSize"].ToString();
						sizeInt = Convert.ToInt32(size);
					}

					// always call Close when done reading.
					sqlReader.Close();

					//sqlTrans.Commit();
					Console.Write("Database commit");
				}
					//catch (SqlException ex)
					//{
					//	Console.Write("Database rolled back [" + ex.Message + "]");
					//	sqlTrans.Rollback();
					//}
				catch (OleDbException ex) {
					Console.Write("Error get sc inf [" + ex.Message + "], sql[" + sqlText + "]");
					//sqlTrans.Rollback();
				}
				// Close the connection.
				//sqlCmd.Connection.Close(); //this is shared
			} else {
				Console.Write("no conn str");
				Logging.eout("no conn str");
			}
			//SchemaInfo sc = new SchemaInfo();
			sc.ColumnSize = sizeInt;
			sc.ColumnType = colType;
			return sc;
		}


		public string GetRawDataType(string tableName, string fieldName) {
			SchemaInfo sc = GetSchemaInfo(tableName, fieldName);
			return sc.ColumnType;
		}


		//public DataBlock open(string sqlText)
		//{
		//  DataBlock result = new DataBlock(this.currCMA);
		//  result.CreateDataSource(sqlText);
		//  return result;
		//}

		public DataView CreateDataSource(string sqlText) {
			return CreateDataSource(sqlText, "");
		}

		public DataView CreateDataSource(string sqlText, string connStr) {
			lastSQL = sqlText;
			DataTable dt = new DataTable();
			DataRow dr;
			int scan = 0;
			isInitialized = true;

			//dt.Columns.Add(new DataColumn("counter", typeof(int)));
			OleDbCommand sqlCmd = new OleDbCommand();
			OleDbDataReader sqlReader;

			if (connStr != "") {
				//connString = connStr;
				OleDbConnection holdCon = new OleDbConnection(connStr);
				holdCon.Open();
				sqlCmd.Connection = holdCon;
			} else {
				sqlCmd.Connection = sqlConn;
			}
			// Assign transaction object for a pending local transaction
			//sqlCmd.Transaction = sqlTrans;
			sqlCmd.CommandText = sqlText;
			try {
				sqlReader = sqlCmd.ExecuteReader();
				NumberOfColumns = sqlReader.FieldCount;

				//add names / types of	fields to schema info array
				for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
					Columns[fieldScan] = new SchemaInfo();
					Columns[fieldScan].ColumnName = sqlReader.GetName(fieldScan);
					//Columns[fieldScan].ColumnSize = sqlReader.get.GetName(fieldScan);
					Columns[fieldScan].ColumnType = sqlReader.GetDataTypeName(fieldScan);
				}

				// Always call Read before accessing data.
				while (sqlReader.Read()) {
					if (scan == 0) {
						curIndex = 0; //set for getvalue
						for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
							//string fieldName = sqlReader.
							string colName = sqlReader.GetName(fieldScan);
							string colType = sqlReader.GetDataTypeName(fieldScan);
							Type useType;
							Type intType = System.Type.GetType("System.Int32");
							Type stringType = System.Type.GetType("System.String");
							Type dateType = System.Type.GetType("System.DateTime");
							Type boolType = System.Type.GetType("System.Boolean"); //TypeCode.Boolean
							Type moneyType = System.Type.GetType("System.Double"); //TypeCode.Double

							useType = stringType;
							if (colType == "DBTYPE_I4") useType = intType; //int
							if (colType == "DBTYPE_WVARCHAR") useType = stringType; //varchar / nvarchar
							if (colType == "DBTYPE_WLONGVARCHAR") useType = stringType; //memo / ntext
							if (colType == "DBTYPE_BOOL") useType = boolType; //bool
							if (colType == "DBTYPE_DATE") useType = dateType; //datetime
							if (colType == "DBTYPE_CY") useType = moneyType; //money / currency

							dt.Columns.Add(new DataColumn(colName, useType));
						}
					}

					//dataVal = (int)sqlReader[0];

					dr = dt.NewRow();
					//dr["counter"] = scan+1;
					for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
						string colName = sqlReader.GetName(fieldScan);
						dr[colName] = sqlReader[colName];
					}
					dt.Rows.Add(dr);
					scan++;
				}
				// always call Close when done reading.
				sqlReader.Close();

				//sqlTrans.Commit();
				Console.Write("Database commit");
			} catch (OleDbException ex) {
				Console.Write("Error [" + ex.Message + "], sql[" + sqlText + "]");
				Logging.eout("Error cr ds [" + ex.Message + "]<!--, sql[" + sqlText + "]-->");
				//sqlTrans.Rollback();
			} catch (SqlException ex) {
				Console.Write("Database rolled back [" + ex.Message + "]");
				//sqlTrans.Rollback();
			} catch (Exception ex) {
				//Console.Write("Database rolled back [" + ex.Message + "]");
				//sqlTrans.Rollback();
				Logging.eout("Error cr ds2 [" + ex.Message + "][" + ex.StackTrace + "]<!--, sql[" + sqlText + "]-->");

			}
			// Close the connection.
			//sqlCmd.Connection.Close();//now shared!


			DataView dv = new DataView(dt);
			currView = dv;
			RecordCount = scan;
			return dv;
		}

		/// <summary>
		/// This should always be called when using execute (hosepipe)
		/// for ease of port from asp to .net (rs.close())
		/// </summary>
		public void close() {
			readerOpen = false;
			if (sqlReader != null) {
				sqlReader.Close();
				sqlReader = null;

			}
		}

		public bool IsVarCharField(int colIndex) {
			// adLongVarChar || field.type = adLongVarWChar) field.type = adVarChar || field.type = adVarWChar)
			return this.getColumnTypeName(colIndex) == "System.String";
		}

		public bool IsBoolField(int colIndex) {
			return this.getColumnTypeName(colIndex) == "System.Boolean";
		}

		public bool IsMemoField(int colIndex) {
			return this.getColumnTypeName(colIndex) == "System.String";
		}

		public bool IsCurrencyField(int colIndex) {
			return this.getColumnTypeName(colIndex) == "System.Double";
		}

		public bool IsIntegerField(int colIndex) {
			//			return (ftype = adTinyInt || ftype = adSmallInt || ftype = adInteger || ftype = adBigInt || ftype = adUnsignedTinyInt || ftype = adUnsignedSmallInt || ftype = adUnsignedBigInt || ftype = adSingle || ftype = adDouble || ftype = adCurrency || ftype = adDecimal || ftype = adNumeric);
			return this.getColumnTypeName(colIndex) == "System.Int32";
		}

		public bool IsDateField(string columnName) {
			return IsDateField(getColumnIndex(columnName));
		}

		public bool IsDateField(int colIndex) {
			//return (ftype == adDate || ftype == adDBDate || ftype == adDBTime || ftype == adDBTimeStamp);
			return getColumnTypeName(colIndex) == "System.DateTime";
		}


		public void SetValue(string fieldName, int newValue) {
			//saveTable.DataSet
			//throw new Exception("The method or operation is not implemented.");

		}

		public void SetValue(string fieldName, string newValue) {
			int idx = GetSchemaIndex(fieldName);
			Columns[idx].Value = newValue;
			Columns[idx].IsValueSet = true;
		}

		/// <summary>
		/// read the schemainfo looking for a fieldname, return index
		/// </summary>
		protected int GetSchemaIndex(string fieldName) {
			int idx = -1;
			foreach (SchemaInfo sc in Columns) {
				if (sc != null) {
					idx++;
					if (Columns[idx].ColumnName.ToLower() == fieldName.ToLower()) { break; }
				}
			}
			return idx;
		}
		public void SetValue(string colName, object newValue) {
			int idx = GetSchemaIndex(colName);
			Columns[idx].Value = newValue;
			Columns[idx].IsValueSet = true;
		}
		public void SetValue(int idx, object value) {
			Columns[idx].Value = value + "";

		}

		public void SetValue(string fieldName, DateTime newValue) {
			int idx = GetSchemaIndex(fieldName);
			Columns[idx].Value = newValue;
			Columns[idx].IsValueSet = true;
		}

		public void SetValue(string fieldName, DataField value) {
			throw new Exception("The method or operation is not implemented.");
		}

		public void Delete() {
			throw new Exception("The method or operation is not implemented.");
		}

		public bool TableExists(string checktable) {
			//returns true if the specified table (or possibly view/stored proc) exists
			bool result = false;
			//OleDbConnection conn = new OleDbConnection(db.connString);
			//conn.Open();
			System.Data.DataTable dt = this.sqlConn.GetSchema("Tables");
			//int tablesIndex = -1;
			//locate tables index
			//foreach (System.Data.DataRow row in dt.Rows)
			//{
			//	tablesIndex++;
			//	if(row.ItemArray[0].ToString()=="Tables"){break;}
			//}
			//dt.Rows[tablesIndex]
			foreach (System.Data.DataRow row in dt.Rows) {
				string holdName = "";
				foreach (System.Data.DataColumn col in dt.Columns) {
					if (col.ColumnName.Equals("TABLE_NAME")) {
						holdName = row[col].ToString();
					}

					if (col.ColumnName.Equals("TABLE_TYPE") && row[col].Equals("TABLE")) {
						if (checktable.ToLower() == holdName.ToLower()) {
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		//function SaveNewRecord(sql)
		//	' save all fields that were posted in a new record
		//	dim rs, field, value, newID
		//	set rs = server.createobject("adodb.recordset")
		//	'response.write sql&"<br>"
		//	rs.open sql, db, 1, 3		' adOpenKeyset, adLockOptimistic
		//	rs.addnew
		//	for each field in rs.fields
		//		if Request(field.name).count > 0 then
		//			value = Request(field.name)
		//			if value = "" then value = null
		//			'response.write field.name &"="& value&"<br>"
		//			field.value = value
		//		end if
		//	next
		//	if(FieldExists(rs, "DateAdded"))then rs("DateAdded") = now()
		//	rs.update
		//	newID = rs(0)
		//	rs.close
		//	set rs = nothing
		//	SaveNewRecord = newID
		//end function

		/// <summary>
		/// Save a record by walking the request object
		/// </summary>
		/// <param name="tablename"></param>
		/// <returns></returns>
		public bool SaveNewRecord(string tablename) {
			return SaveNewRecord("select * from " + tablename, tablename, tablename + "ID");
		}
		/// <summary>
		/// Save a record by walking the request object
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="tablename"></param>
		/// <param name="pkname"></param>
		/// <returns>true if no error</returns>
		public bool SaveNewRecord(string sql, string tablename, string pkname) {
			bool result;
			DataBlock db = new DataBlock();
			DataBlock dataObject = db.open(sql);

			string colsCSV = "";
			int numColumns = dataObject.GetNumberOfColumns();
			if (numColumns > 0) {
				for (int scan = 0; scan < numColumns; scan++) {
					string colName = dataObject.getColumnName(scan);
					if (colName != pkname) {
						string dataValue = GetRequestValue(colName, HttpContext.Current.Request);
						if (dataValue != null)// && dataValue != "")  //if value is null, leave value in db (no form var passed). if blank, it was passed, so overwrite db value with blank
						{
							colsCSV += ((colsCSV == "") ? "" : ",") + "[" + colName + "]";
						}
						if ((HttpContext.Current.Request.Files[colName + "_Upload"] != null && HttpContext.Current.Request.Files[colName + "_Upload"].ContentLength > 0)) {
							// only add this if the uploaded file is >0 length
							colsCSV += ((colsCSV == "") ? "" : ",") + "[" + colName + "]";
						}
					}
					if (colName == "DateAdded") {
						//colsCSV += ((colsCSV == "") ? "" : ",") + colName;
					}
				}
				string sqlText = "insert into " + tablename + " (" + colsCSV + ") values (";
				result = SaveRecord(dataObject, sqlText, tablename, pkname, "insert");

			} else {
				throw new Exception("table missing[" + sql + "]");
			}

			return result;
		}

		/// <summary>
		/// Save an existing record
		/// </summary>
		/// <param name="dataObject"></param>
		/// <param name="sql"></param>
		/// <param name="tablename"></param>
		/// <param name="pkname"></param>
		/// <returns></returns>
		public bool SaveExistingRecord(string sql, string tablename, string pkname) {
			bool result = false;
			//DataBlock dataObject = new DataBlock(this.currCMA);
			//DataView rs = dataObject.CreateDataSource(sql);
			DataBlock dataObject = this.open(sql);
			if (dataObject.RecordCount == 1) {
				//string tablename = rs.Table.TableName;
				string sqlText = "update " + tablename + " set ";
				int numColumns = dataObject.GetNumberOfColumns();
				result = SaveRecord(dataObject, sqlText, tablename, pkname, "update");
			} else {
				dout("SaveExistingRecord: not exactly one record [" + sql + "] rs[" + dataObject.RecordCount + "]");
			}
			return result;
		}


		/// <summary>
		/// get the req value from the crazy asp content container 
		/// eg ctl00$BodyContent$City - get value of Request["City"]
		/// </summary>
		/// <param name="colName">name of col to find in the request object</param>
		/// <param name="req">request object</param>
		/// <returns>value of a form element with the given colname as it's id. if the form var was not passed, return null</returns>
		public static string GetRequestValue(string colName, HttpRequest req) {
			string result = req.Form[colName];								// value from form
			if (result == null) {
				result = req.Form[colName + "$" + colName];	// try the crazy 'asp:' control name
			}
			if (result == null)		 //still not found, try even crazier asp: tag in a container name
			{
				foreach (string reqKey in req.Form.Keys) {
					int posn = reqKey.LastIndexOf('$');
					if (posn != -1 && reqKey.Substring(posn + 1).ToLower() == colName.ToLower()) {
						result = req.Form[reqKey];
						break;
					}

					// try "ctl00$BodyContent$DevAccess$curCheck" looking for DevAccess - handle checkboxes
					if (reqKey == "ctl00$BodyContent$" + colName + "$curCheck") {
						result = (req.Form["ctl00$BodyContent$" + colName + "$curCheck"] == "on") ? "1" : "0";
						break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// insert or update
		/// </summary>
		/// <param name="dataObject"></param>
		/// <param name="sql"></param>
		/// <param name="tablename"></param>
		/// <param name="pkname"></param>
		/// <param name="mode">insert or update</param>
		/// <returns>true if ok, if not, error save in LastError property</returns>
		protected bool SaveRecord(DataBlock dataObject, string sql, string tablename, string pkname, string mode) {
			string result = "";
			string holdSql = "";
			string sqlText = sql;
			int addedCols = 0;

			//OleDbConnection sqlConn = new OleDbConnection(dbConnStr);
			//sqlConn.Open();

			bool skip = false;
			bool useCommonConnection = false;//true;//
			bool useTransaction = false;//true;//
			bool useFiles = (HttpContext.Current.Request.Files.Count > 0);


			//--------------			
			string colType = "";
			int sizeInt = -1;


			OleDbCommand sqlCmd = new OleDbCommand();
			OleDbTransaction sqlTrans = null;

			OleDbDataReader sqlReader;

			//note: loop the reader code based on getschemainfo

			// Begin the transaction.
			//sqlTrans = sqlConn.BeginTransaction();


			// Assign transaction object for a pending local transaction
			if (useCommonConnection) {
				sqlCmd.Connection = this.sqlConn; //use global connection
			} else {
				sqlCmd.Connection = new OleDbConnection(this.connString);
				sqlCmd.Connection.Open();
			}

			//sqlCmd.Transaction = sqlTrans;
			sqlCmd.CommandText = "select top 1 * from " + tablename;
			try {
				sqlReader = sqlCmd.ExecuteReader();
				for (int fieldScan = 0; fieldScan < sqlReader.FieldCount; fieldScan++) {
					//string fieldName = sqlReader.
					string colName = sqlReader.GetName(fieldScan);
					colType = sqlReader.GetDataTypeName(fieldScan);
					DataTable schemaData = sqlReader.GetSchemaTable();
					int NumberOfColumns = schemaData.Columns.Count;
					string size = schemaData.Rows[0]["ColumnSize"].ToString();
					sizeInt = Convert.ToInt32(size);
					skip = false;

					//not that this call returns null if the col was not found in the request
					string dataValue = GetRequestValue(colName, HttpContext.Current.Request);// value from form

					string origValue = "";
					if (mode == "update") { origValue = dataObject.GetValueObj(0, fieldScan).ToString(); } // value from database
					if (colName != "counter") {
						if (colName.ToLower() != pkname.ToLower()) {
							result += "save col[" + colName + "] value[" + dataValue + "]<br/>";

							result += "colType[" + colType + "]<br/>";

							OleDbParameter param = null;

							if (useFiles && dataValue + "" == "") {
								if (HttpContext.Current.Request.Files[colName + "_Upload"] != null) {
									dataValue = HttpContext.Current.Request.Files[colName + "_Upload"].FileName;
									if (dataValue != "") {
										string fileName = dataValue.Substring(dataValue.LastIndexOf("\\") + 1);
										//process the file, get the new file name
										//dataValue = currCMA.ProcessFile(fileName, colName);
										Logging.dout("process files removed here");
									} else {
										dataValue = null; //skip this - no uploaded file

									}
								}
								if (HttpContext.Current.Request[colName + "_UploadRemove"] != null) {
									dataValue = ""; //set to blank to clear the value
								}
							}

							dout("name[" + colName + "] value[" + dataValue + "], type[" + colType + "]");

							//note that a null value means not passed, skip - non null means was passed, use this value - even if blank
							if (dataValue != null)//&& dataValue != "")// check if request value was not passed: leave orig value
							{	//always try to save the value, as it may be newly null'ed
								if (origValue != dataValue || mode == "insert")	//check if different from orig, or insert mode (all)
								{
									dout("----diff or insert mode");
									if (dataValue + "" != "") // check if null
									{
										if (dataValue == "--remove--") dataValue = "";

										param = new OleDbParameter(colName, colType);//", System.Data.DbType.Int32);

										if (colType == "DBTYPE_I4")//"System.Int32")
										{
											dout("----set int");
											try { param.Value = Convert.ToInt32(dataValue); } catch (Exception ex) { dout("failed to convert to int [" + dataValue + "] [" + ex.ToString() + "]"); skip = true; }
										} else if (colType == "DBTYPE_WVARCHAR" || colType == "DBTYPE_WLONGVARCHAR" || colType == "DBTYPE_VARCHAR" || colType == "DBTYPE_LONGVARCHAR")//"System.String")
										{
											dout("----set string");
											param.Value = dataValue.ToString();
										} else if (colType == "DBTYPE_BOOL")//bool
										{
											dout("----set bool");
											param.Value = dataValue.ToString();
										} else if (colType == "DBTYPE_R8")//float
										{
											dout("----set float");
											param.Value = dataValue.ToString();
										} else if (colType == "DBTYPE_CY")//money
										{
											dout("----set money");
											param.Value = dataValue.ToString();
										} else if (colType == "DBTYPE_DBTIMESTAMP" || colType == "DBTYPE_DATE")//"System.DateTime")
										{
											dout("----set datetime");
											if (dataValue.ToString() != "") {
												dout("----set datetime");
												param.Value = DateTime.Parse(dataValue.ToString());
											} else {
												dout("----skip blank datetime");
												skip = true;
											}
										} else {
											dout("----skip [" + dataValue + "] [" + colName + "] type [" + colType + "]");
											skip = true;
										}
									} else //if new value is null, set db null value
									{
										dout("----null");
										param = new OleDbParameter();
										param.ParameterName = colName;
										param.Value = System.DBNull.Value;
									}
								} else {
									// could be same, check for blank date (set to null)

									if (dataValue == "" && (colType == "DBTYPE_DBTIMESTAMP" || colType == "DBTYPE_DATE"))//"System.DateTime")
									{
										//blank date (set to null)
										param = new OleDbParameter();
										param.ParameterName = colName;
										param.Value = System.DBNull.Value;
									} else {

										dout("----same");
										// same
										param = new OleDbParameter(colName, origValue.GetType());//", System.Data.DbType.Int32);
										param.Value = origValue;
									}
								}
							} else if (colName == "DateAdded") {
								DateTime nowval = DateTime.Now;
								param = new OleDbParameter(colName, colType);//
								//param.ParameterName = colName;
								param.Value = nowval;
								//sqlCmd.Parameters.Add(param);
								//addedCols++;
								skip = true;
							} else {
								skip = true;
								dout("----skip");
								// if null, it was not passed: leave orig value
								/*
								param = new OleDbParameter(colName, origValue.GetType());//", System.Data.DbType.Int32);
								param.Value = origValue;
								*/
							}

							if (!skip) {
								sqlText += (addedCols != 0) ? ", " : "";
								//sqlText += ""+colName+" = @"+colName+""; //for access
								if (mode == "insert") {
									sqlText += "?"; //for sql server
									dout("------------------------------------add qn for [" + colName + "]");
								} else {
									sqlText += "[" + colName + "]=?"; //for sql server
								}

								if (true) {
									//param.ParameterName = "@" + param.ParameterName;
								}
								sqlCmd.Parameters.Add(param);
								addedCols++;
							}
						} else {
							int pkvalue = VB.cint(dataValue);
							if (pkvalue == 0) {
								pkvalue = Crypto.DecryptID(dataValue);
							}

							holdSql = (mode == "insert") ? ")" : " where " + colName + " = " + pkvalue;
						}
					} else {
						//ignore the counter
					}
				}

				// always call Close when done reading.
				sqlReader.Close();

				//sqlTrans.Commit();
				//Console.Write("Database commit");
			} catch (OleDbException) {
				//Console.Write("Error [" + ex.Message + "], sql[" + sqlText + "]");
				//sqlTrans.Rollback();
			}

			// close read
			if (useCommonConnection) {
				sqlCmd.Connection.Close();
			}

			///-------------	
			/// handle the construction of the sql exec stmt
			/// 
			///----
			if (addedCols > 0) {
				sqlText += holdSql;
				result += "sqlText[" + sqlText + "]";
				dout("----sql[" + sqlText + "]");

				if (mode == "insert" && currentDatabaseType == DatabaseType.MSSQLServer) sqlText += ";SELECT @@IDENTITY AS LastID;";

				sqlTrans = null;
				if (true) {

					// Assign transaction object for a pending local transaction
					if (useCommonConnection) {
						sqlCmd.Connection = this.sqlConn;
					} else {
						// reopen the connection
						sqlCmd.Connection = new OleDbConnection(this.connString);
						sqlCmd.Connection.Open();
						// Begin the transaction.
						if (useTransaction) { sqlTrans = sqlCmd.Connection.BeginTransaction(); }
					}

					if (useTransaction) { sqlCmd.Transaction = sqlTrans; }
					sqlCmd.CommandText = sqlText;

					try {
						if (mode == "insert") {

							object execResult = sqlCmd.ExecuteScalar();
							_LastInsertedID = Convert.ToInt32(execResult);

							//if (currentDatabaseType == DatabaseType.MSAccess)
							//{
							//  _LastInsertedID = FetchValueInt("select max(" + pkname + ") from " + tablename);
							//}

							result += "\n Success - lastid[" + LastInsertedID + "]";
							dout("result[" + result + "]");

						} else {
							int numItems = sqlCmd.ExecuteNonQuery();

							result += "\n Success [" + numItems + "]";
							dout("result[" + result + "]");

						}
						if (sqlTrans != null) { sqlTrans.Commit(); }
						//dout("Database commit");
					} catch (OleDbException ex) {
						if (sqlTrans != null) { sqlTrans.Rollback(); }
						eout("Save record update: [" + ex.Message + "][" + sqlText + "] - Database transaction rolled back");
						result += "\n Failure 1[" + ex.Message + "][" + sqlText + "]";
						//eout("Database transaction rolled back");
					} finally {
						if (useCommonConnection) {
							// Close the connection.
							sqlCmd.Connection.Close(); //close  connection
						}
					}

				} else {
					//result += "disabled";
				}
			} else {
				dout("no changes");
			}
			LastError = result;
			return (result.IndexOf("Success") != -1);
		}

		//'-------------------------------------------------------------
		//' database functions
		//'-------------------------------------------------------------

		/// <summary>
		/// Return a string containing the type of database - from web.config
		/// </summary>
		/// <returns></returns>
		public DatabaseType GetDatabaseType() {
			return currentDatabaseType;
		}
		//function SqlNow
		//	dim dbtype
		//	dbtype = GetDatabaseType()
		//	if dbtype="access" then
		//		SqlNow = " now() "
		//	elseif dbtype="sqlserver" then
		//		SqlNow = " getdate() "
		//	else
		//		SqlNow = " current_timestamp "
		//	end if
		//end function

		/// <summary>
		/// return true, false, 1, 0 depending on the database type
		/// </summary>
		/// <param name="dataValue"></param>
		/// <returns></returns>
		public string FmtSQLBool(bool dataValue) {
			return (GetDatabaseType() != DatabaseType.MSAccess) ? ((dataValue) ? "1" : "0") : ((dataValue) ? "True" : "False");
			//return str.Replace("'", "''");
		}

		/// <summary>
		/// fmt sql date now
		/// </summary>
		/// <returns></returns>
		public string FmtSQLDate() {
			return FmtSQLDate(DateTime.Now);
		}
		/// <summary>
		/// fmt sql date now
		/// same as fmtsqldate with no parameters
		/// </summary>
		/// <returns></returns>
		//public string SqlNow()
		//{
		//  return FmtSQLDate(DateTime.Now);
		//}

		/// <summary>
		/// format a date for a database (use current database)
		/// </summary>
		/// <param name="d">string date to convert to a proper datetime - use format 12-dec-2008</param>
		/// <returns></returns>
		public string FmtSQLDate(string d) {
			string result;
			DatabaseType dbtype = GetDatabaseType();
			if ("" + d == "") {
				result = " null ";
			} else if (dbtype == DatabaseType.MSAccess) {
				result = " #" + d + "# "; //Fmt.FmtDateTime(d)
			} else if (dbtype == DatabaseType.MSSQLServer) {
				result = " convert(datetime,'" + d + "') "; //Fmt.FmtDateTime(d)
			} else {
				result = " '" + d + "' "; //Fmt.FmtDateTime(d)
			}
			return result;
		}



		public string FmtSQLDate(DateTime d) {
			return FmtSQLDate(d.ToLongDateString());
		}

		public string FmtSQLDateTime(DateTime d) {
			return FmtSQLDate(d.ToLongDateString() + " " + d.ToLongTimeString());
		}


		/// <summary>
		/// return string now, getdate etc
		/// </summary>
		/// <returns></returns>
		public string SqlNow() {
			string result;
			DatabaseType dbtype = GetDatabaseType();
			if (dbtype == DatabaseType.MSAccess) {
				result = " now() ";
			} else if (dbtype == DatabaseType.MSSQLServer) {
				result = " getdate() ";
			} else {
				result = " current_timestamp ";
			}
			return result;
		}
		//sub MoveToRandomRecord(rs, recordCount)
		//	' chooses a random record and moves to it
		//	dim numberPicked, i
		//	randomize
		//	numberPicked = int(rnd(timer) * recordCount)
		//	i = 0
		//	do while not rs.eof and i < numberPicked
		//		i = i + 1
		//		rs.movenext
		//	loop
		//end sub
		public static void MoveToRandomRecord(DataBlock rs) {
			rs.MoveToRandomRecord();
		}


		//function FieldExists(rs, fieldName)
		//	dim f
		//	FieldExists = false
		//	for each f in rs.fields
		//		if lcase(f.name) = lcase(fieldName) then
		//			FieldExists = true
		//		end if
		//	next
		//end function

		public bool FieldExists(string fieldName) {
			bool result = false;
			int colIndex = getColumnIndex(fieldName);
			result = (colIndex != -1);
			return result;
		}


		public static bool FieldExists(DataBlock db, string fieldName) {
			bool result = false;
			int colIndex = db.getColumnIndex(fieldName);
			result = (colIndex != -1);
			return result;
		}


		internal void eout(string msg) {
			Logging.eout(msg);
		}
		internal void dout(string msg) {
			Logging.dout(msg);
		}

		/// <summary>
		/// move the rs from the start (index 0) to the given index
		/// </summary>
		/// <param name="startPosn"></param>
		public void MoveToIndex(int startPosn) {
			for (int scan = 0; scan < startPosn; scan++) { movenext(); }
		}

		/// <summary>
		/// create a record based on whatever data is in the schemainfo
		/// </summary>
		public void Create() {
			if (TableName == null) throw new Exception("You need to set the tablename before calling create");
			var sConnectionString = Beweb.BewebData.GetConnectionString();
			SqlConnection objConn = new SqlConnection(sConnectionString);
			objConn.Open();
			SqlDataAdapter daTable = new SqlDataAdapter("Select top 1 * From " + TableName + " where 1=0", objConn);
			DataSet ds = new DataSet();
			daTable.FillSchema(ds, SchemaType.Source, TableName);
			daTable.Fill(ds, TableName);
			DataTable tblTable = ds.Tables[TableName];

			//	DataRow drCurrent = tblTable.Rows[0];//get first record
			DataRow drCurrent = tblTable.NewRow();// create new 

			var idx = 0;
			foreach (SchemaInfo sc in Columns) {
				if (sc == null) break;
				if (idx > 0 && sc.IsValueSet) {
					try { drCurrent[sc.ColumnName] = sc.Value; } catch (Exception) { }
				}
				idx++;
			}
			tblTable.Rows.Add(drCurrent);

			new SqlCommandBuilder(daTable);//look: return from this is disregarded
			daTable.Update(ds, TableName);

			objConn.Close();
		}


		/// <summary>
		/// update a record based on whatever data is in the schemainfo
		/// </summary>
		public void Update() {

			var sConnectionString = Beweb.BewebData.GetConnectionString();
			SqlConnection objConn = new SqlConnection(sConnectionString);
			objConn.Open();
			SqlDataAdapter daTable = new SqlDataAdapter("Select top 1 * From " + TableName + " where " + TableName + "ID=" + Columns[0].Value, objConn);
			DataSet ds = new DataSet();
			daTable.FillSchema(ds, SchemaType.Source, TableName);
			daTable.Fill(ds, TableName);
			DataTable tblTable = ds.Tables[TableName];

			DataRow drCurrent = tblTable.Rows[0];//get first record

			var idx = 0;
			foreach (SchemaInfo sc in Columns) {
				if (sc == null) break;
				if (idx > 0 && sc.IsValueSet) { drCurrent[sc.ColumnName] = sc.Value; }
				idx++;
			}

			new SqlCommandBuilder(daTable);//look: return from this is disregarded
			daTable.Update(ds, TableName);

			objConn.Close();
		}


		/// <summary>
		/// Populate the columns schema info structure based on the current table. You must set TableName to create the structure. You 
		/// can use this to create new records by setting tablename, load from array (eg
		/// read from a file) then create (or update, or delete)
		/// 
		/// </summary>
		/// <param name="colvalues"></param>
		public void Load(int id) {
			if (_TableName == null) throw new Exception("You need to set tablename before calling load");

			//todo:load
			throw new NotImplementedException();
		}
		/// <summary>
		/// load the current record (columns array) from an array 
		/// </summary>
		/// <param name="colvalues">array in same order as columns</param>
		public void Load(string[] colvalues, bool includeIDCol) {
			if (_TableName == null) throw new Exception("You need to set tablename before calling load");

			//skip the ident col and start at	1
			for (int sc = (includeIDCol) ? 0 : 1; sc < Columns.Length; sc++) {
				if (Columns[sc] == null) break;
				string newValue = colvalues[sc];
				newValue = newValue.Replace("\r", "");
				Columns[sc].Value = newValue;
				if (newValue != "NULL") Columns[sc].IsValueSet = true;
			}
		}

		//function GetDelimitedRecordString(sql, recordDelimiter)
		//	dim rs, str
		//	set rs = db.execute(sql)
		//	if rs.eof then
		//		str = ""
		//	else
		//		str = rs.GetString(,,,recordDelimiter)
		//		str = mid(str, 1, len(str)-len(recordDelimiter))
		//	end if
		//	rs.close
		//	set rs = nothing
		//	GetDelimitedRecordString = str
		//end function

		/// <summary>
		/// Given a query (one column), get the records separated by the delimiter
		/// </summary>
		/// <param name="dataObject"></param>
		/// <param name="sql">query returning one column</param>
		/// <param name="recordDelimiter"></param>
		/// <returns>records separated by the delimiter</returns>
		//public string GetDelimitedRecordString(string sql, string recordDelimiter)
		//{
		//  return GetDelimitedRecordString(this.db, sql, recordDelimiter, false);
		//}
		//public string GetDelimitedRecordString(string sql, string recordDelimiter, bool quotedValues)
		//{
		//  return GetDelimitedRecordString(this.db, sql, recordDelimiter, quotedValues);
		//}
		public static string GetDelimitedRecordString(DataBlock dataObject, string sql, string recordDelimiter) {
			return GetDelimitedRecordString(dataObject, sql, recordDelimiter, false);
		}
		public static string GetDelimitedRecordString(DataBlock dataObject, string sql, string recordDelimiter, bool quotedValues) {
			string result = "";

			DataView rs;
			rs = dataObject.CreateDataSource(sql);

			for (int scan = 0; scan < dataObject.RecordCount; scan++) {
				string value = dataObject.GetValue(scan, 0) + "";				// convert to string
				if (result != "") { result += recordDelimiter; }
				if (quotedValues) { result += "\"" + value + "\""; } //quoted
				result += value;
			}

			return result;
		}



		//public string FmtLongDate()
		//{
		//  return Fmt.LongDate((string)_dataValue);
		//}

		public string GetSqlInsertStatement(string tablename) {
			string result = "";
			string colsCSV = "";
			for (int scan = 0; scan < Columns.Length; scan++) {
				if (Columns[scan] == null) break;
				string colName = Columns[scan].ColumnName;
				//string dataValue = Columns[scan].Value;
				if (Columns[scan].IsValueSet)// && dataValue != "")  //if value is null, leave value in db (no form var passed). if blank, it was passed, so overwrite db value with blank
				{
					colsCSV += ((colsCSV == "") ? "" : ",") + "[" + colName + "]";
				}
			}
			string sqlText = "insert into " + tablename + " (" + colsCSV + ") values ";
			string valuesCSV = "";

			for (int scan = 0; scan < Columns.Length; scan++) {
				if (Columns[scan] == null) break;
				string colName = Columns[scan].ColumnName;
				object dataValue = Columns[scan].Value;
				if (dataValue != null && Columns[scan].IsValueSet)// && dataValue != "")  //if value is null, leave value in db (no form var passed). if blank, it was passed, so overwrite db value with blank
				{
					if (Columns[scan].ColumnType.ToLower().IndexOf("varchar") != -1) dataValue = Fmt.SqlText(dataValue + "");
					if (Columns[scan].ColumnType.ToLower().IndexOf("time") != -1) dataValue = Fmt.SqlDateTime(dataValue + "");
					if (Columns[scan].ColumnType.ToLower().IndexOf("bool") != -1) dataValue = Fmt.SqlBoolean(dataValue + "");
					valuesCSV += ((valuesCSV == "") ? "" : ",") + dataValue;
				}

			}
			result = sqlText + "(" + valuesCSV + "); ";
			return result;
		}
	}


	/// <summary>
	/// store a few bits of info
	/// </summary>
	public class SchemaInfo {
		public string ColumnType;
		public int ColumnSize;
		public string ColumnName;
		public object Value;
		public bool IsValueSet;
	}

	/// <summary>
	/// </summary>
	public class DataField {
		protected object _dataValue;
		protected int colIndex = -1;

		public object value {
			get { return _dataValue; }
			set { _dataValue = value; }
		}

		public DataField(DataBlock db, string colName) {

		}
		public new string ToString() {
			return ((string)_dataValue);
		}



	}


	/// <summary>
	/// paging for datablock objects
	/// </summary>
	public class Paging {

		protected int _itemsPerPage;
		public int ItemsPerPage {
			get { return _itemsPerPage; }
			set {
				_itemsPerPage = value;
				//init();
			}
		}
		protected DataBlock rs;
		protected int RecordCount;
		//protected HttpRequest oReq;
		public string pageName;
		public int curPage;
		public int startPosn;
		public int numPages;
		public int endPosn;
		public int modscan = 1;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rs">a forward reading (hosepipe)</param>
		/// <param name="RecordCount"></param>
		/// <param name="oReq"></param>
		public Paging(DataBlock rs, int RecordCount/*, HttpRequest oReq*/) {
			_itemsPerPage = 25; //default number of items
			this.rs = rs;
			this.RecordCount = RecordCount;
			//this.oReq=oReq;
			//init();
		}
		public void init(string pageName, int curPage) {
			//pageName = oReq.ServerVariables["Script_name"];
			//curPage = (oReq["pg"] + "" != "") ? Convert.ToInt32(oReq["pg"]) : 1;
			startPosn = (curPage - 1) * _itemsPerPage;
			numPages = (int)Math.Ceiling(((double)RecordCount) / ((double)_itemsPerPage));

			endPosn = startPosn + _itemsPerPage;
			endPosn = (endPosn > RecordCount) ? RecordCount : endPosn;

			rs.MoveToIndex(startPosn);
		}

		/// <summary>
		/// draw a 1.2.3.4.5 style paging
		/// </summary>
		/// <returns></returns>
		public string DrawPaging() {

			string str = "", url = "";
			int fp = (curPage - 5 <= 0) ? 1 : curPage - 5;
			int numDrawn = 0;
			if (curPage > 1) {
				url = pageName + "?pg=" + (curPage - 1);
				if (!String.IsNullOrEmpty(Web.Request["pid"])) url += "&pid=" + Web.Request["pid"];
				str += "<a href=\"" + url + "\" class=\"prev\">Previous</a> ";
			}
			if (fp > 1) {
				str += " ... ";
			}
			str += " | ";

			for (int pageNum = fp; pageNum <= numPages && numDrawn < 11; pageNum++) {
				if (pageNum > fp) {
					str += " | ";
				}
				if (pageNum == curPage) {
					str += " <b>" + pageNum + "</b> ";
				} else {
					url = pageName + "?pg=" + pageNum;
					if (!String.IsNullOrEmpty(Web.Request["pid"])) url += "&pid=" + Web.Request["pid"];
					str += " <a href=\"" + url + "\">" + pageNum + "</a> ";
				}
				numDrawn++;
			}
			str += " | ";
			if (numDrawn + fp < numPages) {
				str += " ... ";
			}
			if (curPage < numPages) {
				url = pageName + "?pg=" + (curPage + 1);
				if (!String.IsNullOrEmpty(Web.Request["pid"])) url += "&pid=" + Web.Request["pid"];
				str += " <a class=\"next\" href=\"" + url + "\">Next</a> ";
			}
			return str;
		}

	}
}