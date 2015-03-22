#define TestExtensions
#define ActiveRecord
#define BaseTypeExtensions
#define Logging
using System;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;

namespace Beweb {
	public partial class Sql : SqlStringBuilder {

		/// <summary>
		/// sql2005 uses a fancy new feature (WITH row_number() OVER...) which IS VERY SLOW with LIKE on NTEXT fields. It works with SQL 2005 and later. It is very good if you are expecting to display page 100 or page 1000 but usually you would sort descending and display the first few pages anyway.
		/// sql2000 uses TOP x intelligently and then skips thu to correct place. It works with any sql statement with any versions of SQL server and some other databases.
		/// software is VERY SLOW and completely useless - it brings ALL records in and then skips through to correct place. It works with any type of database.
		/// </summary>
		public enum PagingType { sql2005, sql2000, software };

		private static readonly PagingType DefaultResultSetPagingType = (PagingType)Enum.Parse(typeof(PagingType), (ConfigurationManager.AppSettings["ResultSetPagingType"] ?? "sql2005"));
		public PagingType ResultSetPagingType = DefaultResultSetPagingType;
		private string _sqlConnectionString { get; set; }
		public string SqlConnectionString {
			get { return _sqlConnectionString; }
			set {
				if (value!=null && value.Contains("Initial Catalog=")) {
					_sqlConnectionString = value;
				} else {
					_sqlConnectionString = BewebData.GetConnectionString(value);
				}
			}
		}
		public string lastError = "";
		// all the string sqlizing functionality is in base class SqlStringBuilder

		// constructors

		/// <summary>
		/// Creates a new empty SQL command string.
		/// </summary>
		public Sql() {
		}

		/// <summary>
		/// Creates a new SQL command string.
		/// </summary>
		/// <example>sql.Add("order by dateadded desc")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		public Sql(string sqlFragment)
			: this() {
			Add(sqlFragment);
		}

		public Sql(Sql sqlFragment) {
			AddSql(sqlFragment);
		}

		public static Sql operator +(Sql obj, string sqlFragment) {
			var newObj = new Sql();
			newObj.Value = obj.Value;
			newObj.Add(sqlFragment);
			return newObj;
		}

		public Sql AddSql(Sql anotherSqlObj) {
			this._value += " " + anotherSqlObj.Value;
			this._value = this._value.Trim();
			return this;
		}

		public Sql PrependSql(Sql anotherSqlObj) {
			this._value = anotherSqlObj.Value + " " + this._value;
			this._value = this._value.Trim();
			return this;
		}

		/// <summary>
		/// Appends a piece of SQL string plus a SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where name=", "mike".SqlizeText())</example>
		/// <example>sql.Add("and id=", 55)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public Sql(string sqlFragment, SqlizedValue sqlizedValue) {
			Add(sqlFragment, sqlizedValue);
		}

		/// <summary>
		/// allow simple queries to be executed by supressing quote checking. Only use for things like this:
		/// <example>
		/// (new Sql("insert into synclog(syncdate,tablename,logmessage) values(getdate(),'opportunity','Sync start.')",true)).Execute();
		/// MN: Good idea but this actually clashes with new Sql("select * from page where showinNav=", true) so instead we could create a new subclass
		/// eg new SqlUnsafe("insert into synclog(syncdate,tablename,logmessage) values(getdate(),'opportunity','Sync start.')")
		/// </example>
		/// </summary>
		/// <param name="sqlFragment"></param>
		/// <param name="allowSingleQuotes"></param>
		//public Sql(string sqlFragment, bool allowSingleQuotes) {
		//	this.suppressQuoteChecking = allowSingleQuotes;
		//	Add(sqlFragment);
		//}
		/// <summary>
		/// Appends a SQL fragment plus a SqlizedValue plus another SQL fragment to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ")")</example>
		/// <example>sql.Add("and id=", 55, "or id is null")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2) {
			Add(sqlFragment, sqlizedValue, sqlFragment2);
		}

		/// <summary>
		/// Appends a SQL fragment, a SqlizedValue, another SQL fragment, plus another SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ") and dateadded>", DateTime.Now)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue2">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4, sqlFragment5);
		}

		public Sql(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5, SqlizedValue sqlizedValue5, string sqlFragment6) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4, sqlFragment5, sqlizedValue5, sqlFragment6);
		}

		public new Sql Add(string sqlFragment) {
			base.Add(sqlFragment);
			return this;
		}

		public new Sql Add(SqlizedValue sqlizedValue) {
			base.Add(sqlizedValue);
			return this;
		}

		public new Sql Add(Sql sqlFragment) {
			AddSql(sqlFragment);
			return this;
		}



		/// <summary>
		/// Appends a piece of SQL string plus a SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.base.Add("where name=", "mike".SqlizeText())</example>
		/// <example>sql.base.Add("and id=", 55)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue) {
			base.Add(sqlFragment, sqlizedValue);
			return this;
		}

		/// <summary>
		/// allow simple queries to be executed by supressing quote checking. Only use for things like this:
		/// <example>
		/// (new Sql("insert into synclog(syncdate,tablename,logmessage) values(getdate(),'opportunity','Sync start.')",true)).Execute();
		/// MN: Good idea but this actually clashes with new Sql("select * from page where showinNav=", true) so instead we could create a new subclass
		/// eg new SqlUnsafe("insert into synclog(syncdate,tablename,logmessage) values(getdate(),'opportunity','Sync start.')")
		/// </example>
		/// </summary>
		/// <param name="sqlFragment"></param>
		/// <param name="allowSingleQuotes"></param>
		//public new Sql Add(string sqlFragment, bool allowSingleQuotes) {
		//	this.suppressQuoteChecking = allowSingleQuotes;
		//	base.Add(sqlFragment);
		//return this; }
		/// <summary>
		/// Appends a SQL fragment plus a SqlizedValue plus another SQL fragment to the current SQL command string.
		/// </summary>
		/// <example>sql.base.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ")")</example>
		/// <example>sql.base.Add("and id=", 55, "or id is null")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2);
			return this;
		}

		/// <summary>
		/// Appends a SQL fragment, a SqlizedValue, another SQL fragment, plus another SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.base.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ") and dateadded>", DateTime.Now)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue2">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4, sqlFragment5);
			return this;
		}

		public new Sql Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5, SqlizedValue sqlizedValue5, string sqlFragment6) {
			base.Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4, sqlFragment5, sqlizedValue5, sqlFragment6);
			return this;
		}

		public new Sql AddRawSqlString(string sqlFragmentString) {
			base.AddRawSqlString(sqlFragmentString);
			return this;
		}

		public new Sql AddRaw(string sqlFragmentString) {
			base.AddRawSqlString(sqlFragmentString);
			return this;
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a nullable int value. If the database field type is not compatible an exception is thrown.
		/// Used for database field type INT.
		/// </summary>
		/// <example>
		/// int id? = new Sql("select top 1 id from invoice").FetchInt();
		/// if (id == 55) { Response.Write("its 55") }
		/// if (id == null) { Response.Write("its null") }
		/// </example>
		/// <example>int id = new Sql("select top 1 id from invoice").FetchInt();</example>
		/// <returns>A nullable int</returns>
		public int? FetchInt() {
			return FetchGenericValue<int?>(dr => dr.GetInt32(0));
		}

		public List<int> FetchIntList() {
			var returnVal = FetchGenericList<int>(dr => dr.GetInt32(0));
			return returnVal;
		}

		public List<DateTime> FetchDateList() {
			var returnVal = FetchGenericList<DateTime>(dr => dr.GetDateTime(0));
			return returnVal;
		}

		public List<string> FetchStringList() {
			var returnVal = FetchGenericList<string>(dr => dr.GetString(0));
			return returnVal;
		}

		public List<double> FetchDoubleList() {
			var returnVal = FetchGenericList<double>(dr => dr.GetDouble(0));
			return returnVal;
		}

		public List<decimal> FetchDecimalList() {
			var returnVal = FetchGenericList<decimal>(dr => dr.GetDecimal(0));
			return returnVal;
		}

		/// <summary>
		/// If you don't know exctly what field type it is, and want to convert whatever it is to a number.
		/// </summary>
		public decimal FetchNumber() {
			var val = FetchGenericValue<object>(dr => dr.GetValue(0));
			val = val.ToDecimal(0);
			return (decimal)val;
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a nullable Long (Int64) value. If the database field type is not compatible an exception is thrown.
		/// Used for database field type BIGINT.
		/// This is the return type from SQL ROW_NUMBER and RANK functions.
		/// </summary>
		public long? FetchLong() {
			return FetchGenericValue<long?>(dr => dr.GetInt64(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a nullable Double value. If the database field type is not compatible an exception is thrown.
		/// Used for database field type FLOAT.
		/// </summary>
		/// <example>
		/// double? ratio = new Sql("select top 1 ratio from invoice").FetchDouble();
		/// </example>
		/// <returns>A nullable double</returns>
		public double? FetchDouble() {
			return FetchGenericValue<double?>(dr => dr.GetDouble(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a nullable decimal value. If the database field type is not compatible an exception is thrown.
		/// This is used for database field types DECIMAL and MONEY.
		/// </summary>
		/// <example>decimal? cost = new Sql("select sum(cost) from invoice").FetchDecimal()</example>
		/// <returns>A nullable decimal</returns>
		public decimal? FetchDecimal() {
			return FetchGenericValue<decimal?>(dr => dr.GetDecimal(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a boolean value (non-nullable). If the database field type is not compatible an exception is thrown.
		/// This is used for database field type BIT (NOT NULL).
		/// Generally we don't allow NULLs in BIT columns but if there are NULLs you can use FetchBoolNullable() instead.
		/// If the database contains NULL the return value will be false. 
		/// </summary>
		/// <example>if (sql.FetchBool()) { Response.Write("it works!") }</example>
		/// <example>bool isPublished = new Sql("select ispublished from page where id=", id).FetchBool()</example>
		/// <returns>A non-nullable bool</returns>
		public bool FetchBool() {
			return FetchGenericValue<bool>(dr => dr.GetBoolean(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a string (or null). If the database field type is not compatible an exception is thrown.
		/// This is used for database field types NVARCHAR, VARCHAR, CHAR, NTEXT, and TEXT.
		/// </summary>
		/// <returns>A string (may be null)</returns>
		public string FetchString() {
			return FetchGenericValue<string>(dr => dr.GetString(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a DateTime (nullable). If the database field type is not compatible an exception is thrown.
		/// This is used for database field type DATETIME.
		/// </summary>
		/// <returns>A nullable System.DateTime</returns>
		public DateTime? FetchDate() {
			//return FetchGenericValue<DateTime?>(dr => dr.GetDateTime(0));
			return FetchGenericValue<DateTime?>(dr => (DateTime?)dr.GetValue(0));
		}

		/// <summary>
		/// Executes the SQL command and returns the first column of the first row as a GUID (nullable). If the database field type is not compatible an exception is thrown.
		/// This is used for database field type GUID or UNIQUEIDENTIFIER.
		/// </summary>
		/// <returns>A nullable System.Guid</returns>
		public Guid? FetchGuid() {
			return FetchGenericValue<Guid?>(dr => dr.GetGuid(0));
		}

		////////////////////////////////////////

		/// <summary>
		/// Same as FetchInt() but returns a non-nullable type.
		/// If the database contains NULL the return value will be 0. 
		/// You can use this when either (1) you know the database field contains no nulls or (2) you want any nulls to be defaulted to zero (eg in a count statement).
		/// </summary>
		/// <example>
		/// int id = new Sql("select count(*) from invoice where 0=1").FetchInt();    // returns 0
		/// </example>
		/// <returns>A non-nullable int</returns>
		public int FetchIntOrZero() {
			return FetchGenericValue<int>(dr => dr.GetInt32(0));
		}

		/// <summary>
		/// Same as FetchInt() but returns a user specified int if null would have been returned.
		/// If the database contains NULL the return value will be the parameter. 
		/// You can use this when either (1) you know the database field contains no nulls or (2) you want any nulls to be defaulted to a defined value (eg in a sql statement when you want a record count or sortorder to start at a certain position).
		/// </summary>
		/// <example>
		/// int id = new Sql("select count(*) from invoice where 0=1").FetchIntOrThis(1000);    // returns 0
		/// </example>
		/// <returns>A non-nullable int</returns>
		public int FetchIntOrThis(int defaultValue) {
			return FetchGenericValue<int>(dr => dr.GetInt32(defaultValue));
		}

		/// <summary>
		/// Same as FetchLong() but returns a non-nullable type.
		/// If the database contains NULL the return value will be 0. 
		/// </summary>
		public long FetchLongOrZero() {
			return FetchGenericValue<long>(dr => dr.GetInt64(0));
		}

		/// <summary>
		/// Same as FetchDouble() but returns a non-nullable type.
		/// If the database contains NULL the return value will be 0. 
		/// You can use this when either (1) you know the database field contains no nulls or (2) you want any nulls to be defaulted to zero (eg in a count statement).
		/// </summary>
		/// <returns>A non-nullable double</returns>
		public double FetchDoubleOrZero() {
			return FetchGenericValue<double>(dr => dr.GetDouble(0));
		}

		/// <summary>
		/// Same as FetchDecimal() but returns a non-nullable type.
		/// If the database contains NULL or there are no records the return value will be 0. 
		/// You can use when either (1) you know the database field contains no nulls or (2) you want any nulls to be defaulted to zero (eg in a SUM statement).
		/// </summary>
		/// <example>
		/// decimal total = new Sql("select sum(cost) from invoice").FetchDecimalNotNull();    // returns 0
		/// </example>
		/// <returns>A non-nullable decimal</returns>
		public decimal FetchDecimalOrZero() {
			return FetchGenericValue<decimal>(dr => dr.GetDecimal(0));
		}

		/// <summary>
		/// Same as FetchDate() but can only be used where you are sure that the field does not contain any nulls.
		/// Throws an exception if the field contains NULL or there are no records.
		/// </summary>
		/// <returns>A date (non-nullable)</returns>
		public DateTime FetchDateOrDie() {
			var result = FetchDate();
			if (result == null) { throw new Exception("Date in database is NULL. FetchDateNotNull() does not allow this."); }
			return result.Value;
		}

		/// <summary>
		/// Same as FetchGuid() but returns a non-nullable type.
		/// You can use when you know the database field contains no nulls.
		/// Throws an exception if the field contains NULL or there are no records.
		/// </summary>
		/// <returns>A non-nullable System.Guid</returns>
		public Guid FetchGuidOrDie() {
			var result = FetchGuid();
			if (result == null) { throw new Exception("GUID in database is NULL. FetchGuidNotNull() does not allow this."); }
			return result.Value;
		}

		///// <summary>
		///// Same as FetchBool() except it allows for NULL values to be returned.
		///// This is not the default option since generally we do not want to have NULLs in BIT fields. Wherever possible BIT database fields should be set to DEFAULT (0) so they do not allow NULLs.
		///// </summary>
		///// <example>if (sql.FetchBool()==true) { Response.Write("it works!") }</example>
		///// <example>bool? isPublished = new Sql("select ispublished from page where id=", id).FetchBool()</example>
		///// <returns>A nullable bool</returns>
		///// <summary>
		//public bool? FetchBoolNullable() {
		//  return FetchGenericValue<bool?>(dr => dr.GetBoolean(0));
		//}

		//////////////////////////////////////

		/// <summary>
		/// Loads data from the database into a plain old object (POO), where column names and types of the resultset match the properties of the object
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <returns></returns>
		public TModel LoadPoo<TModel>() where TModel : new() {
			TModel obj = default(TModel);
			using (var dr = this.GetReader()) {
				if (dr != null && dr.HasRows && dr.Read()) {
					obj = LoadPooProperties<TModel>(dr);
				}
				dr.Close();
				dr.Dispose();
			}
			return obj;
		}

		/// <summary>
		/// Loads data from the database into a list of plain old objects (POOs), where column names and types of the resultset match the properties of the object
		/// </summary>
		/// <example>
		/// 		public class Cat{public string CategoryName{get;set;} }
		/// 		public List<Cat> Categories {
		/// 			get {
		/// 				var sql = new Sql("select CategoryName from modelsummary group by categoryname order by categoryname"); 
		/// 				return sql.LoadPooList<Cat>();
		/// 			}
		/// 		}
		/// 	ui:	
		///		<% foreach (var cat in Model.Categories){
		/// 		%><%=cat.CategoryName %><br /><%
		/// 	}%>
		/// 		
		/// </example>
		/// <typeparam name="TModel"></typeparam>
		/// <returns></returns>
		public List<TModel> LoadPooList<TModel>() where TModel : new() {
			List<TModel> list;
			using (var dr = this.GetReader()) {
				list = new List<TModel>();
				int rowCount = 0;
				foreach (var row in dr) {
					var obj = LoadPooProperties<TModel>(dr);
					list.Add(obj);
					rowCount++;
					UpdateTotalRowCount(rowCount, dr);
				}
				dr.Close();
			}
			return list;
		}

		/// <summary>
		/// Loads data from the given SqlDataReader into a plain old object (POO), where column names and types of the resultset match the properties of the object
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="reader"></param>
		/// <returns></returns>
		public TModel LoadPooProperties<TModel>(DbDataReader reader) where TModel : new() {
			// for each property in TModel
			// see if field exists
			// set value to reader value

			var result = new TModel();
			Type objType = result.GetType();
			string objName = objType.Name;

			// TODO: may be more performant to use reader.GetValues() and store in an array of objects instead

			PropertyInfo[] fields = objType.GetProperties();

			//						 PopulateTypeException ex = null;
			//Exception ex = null;

			foreach (PropertyInfo property in fields) {
				//check the key
				//going to be forgiving here, allowing for full declaration 13                   //or just propname
				string propertyName = property.Name.ToLower();

				for (int i = 0; i < reader.FieldCount; i++) {
					if (reader.GetName(i).ToLower() == propertyName) {
						//if (reader.GetFieldType(i) == property.PropertyType || (property.PropertyType.IsNullableType() && property.PropertyType.GetGenericArguments()[0] == reader.GetFieldType(i))) {
						// decoupled from base type extensions
						var theType = property.PropertyType;
						var isNullable = ((theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
						var nullableAndType = isNullable && property.PropertyType.GetGenericArguments()[0] == reader.GetFieldType(i);

						if (reader.GetFieldType(i) == property.PropertyType || nullableAndType) {
							// matching property name with matching type
							object newValue = reader.GetValue(i);
							if (newValue == DBNull.Value) newValue = null;
							property.SetValue(result, newValue, null);
						} else {
							// try it anyway, using new fancy logic
							try {
								object newValue = ReadDatabaseValue(dr => Convert.ChangeType(dr.GetValue(i), theType), reader);
								property.SetValue(result, newValue, null);
							} catch (Exception e) {
								// ok, it didn't work
							}
						}
					}
				} // for each column
			} // foreach prop

			return result;
		}

		public T FetchGenericValue<T>(Func<DbDataReader, T> func) {
			T returnValue = default(T);
			//var dr = this.GetReader();

			DbDataReader dr;
			// var conn;

			DbConnection conn;
			if (SqlConnectionString == null) {
				conn = BewebData.CreateNewConnection();
			} else {
				conn = BewebData.CreateNewConnection(SqlConnectionString, (SqlConnectionString.Contains("Initial Catalog=")));
			}

			var cmd = conn.CreateCommand();
			cmd.CommandText = this.Value;
#if Logging
			LogStartQuery("Sql FetchValue");
#endif
			try {
				using (dr = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult)) {
					if (dr.Read()) {
						returnValue = ReadDatabaseValue(func, dr);
					}
					dr.Close();
				}
			} catch (SqlException e) {
				throw new Exception("Beweb SQL Fetch Error: " + e.Message + "\n\nSQL: " + this.Value, e);
			}

#if Logging
			LogEndQuery();
#endif
			return returnValue;
		}

		public List<T> FetchGenericList<T>(Func<DbDataReader, T> func) {
			var returnValue = new List<T>();

			DbDataReader dr;
			DbConnection conn;
			if (SqlConnectionString == null) {
				conn = BewebData.CreateNewConnection();
			} else {
				conn = BewebData.CreateNewConnection(SqlConnectionString, (SqlConnectionString.Contains("Initial Catalog=")));
			}

			var cmd = conn.CreateCommand();
			cmd.CommandText = this.Value;
#if Logging
			LogStartQuery("Sql FetchGenericList");
#endif

			try {
				using (dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
					if (dr.HasRows) { // don't use dr.Read() here - it will use up the first row!!
						foreach (var dataItem in dr) {
							returnValue.Add(ReadDatabaseValue(func, dr));
						}
					}
					dr.Close();
				}
			} catch (SqlException e) {
				throw new Exception("Beweb SQL Fetch Error: " + e.Message + "\n\nSQL: " + this.Value, e);
			}

#if Logging
			LogEndQuery();
#endif
			return returnValue;
		}

		private T ReadDatabaseValue<T>(Func<DbDataReader, T> func, DbDataReader dr) {
			T returnValue = default(T);
			Type returnType = typeof(T);

			var fieldType = dr.GetFieldType(0);
			var fieldTypeString = fieldType.ToString();
			bool isNumericField = (fieldTypeString.Contains("System.Decimal") || fieldTypeString.Contains("System.Double") || fieldTypeString.Contains("System.Int"));
			bool isDecimalReturn = typeof(T).ToString().Contains("System.Decimal");
			//				if (fieldType is T) {
			if (dr.IsDBNull(0)) {
				returnValue = default(T); // either null for a nullable type or 0, false, etc for a non-nullable
			} else if (returnType.IsAssignableFrom(fieldType)) {
				returnValue = (T)func.Invoke(dr);
			} else if (isDecimalReturn && isNumericField) {
				returnValue = (T)(object)func.Invoke(dr).ToDecimal(0);
			} else {
				TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
				if (conv.CanConvertFrom(fieldType)) {
					try {
						returnValue = (T)conv.ConvertFrom(func.Invoke(dr));
					} catch (Exception e) {
						// incorrect type
						string message = "Fetch error - the return variable type [" + returnType + "] does not match the database field type [" + fieldType.ToString() + "]. Tried to convert but failedt.";
						message += "\nSQL was [" + this.ToString() + "]";
						throw new BewebDataException(message, e);
					}
				} else {
					// incorrect type
					string message = "Fetch error - the return variable type [" + returnType + "] does not match the database field type [" + fieldType.ToString() + "]. No conversion exists.";
					message += "\nSQL was [" + this.ToString() + "]";
					throw new BewebDataException(message);
				}
			}
			return returnValue;
		}

		protected void OnGetReaderSelecting(object sender, SqlDataSourceCommandEventArgs e) {
			e.Command.CommandTimeout = 50; //seconds
		}

		/// <summary>
		/// Execute SQL and return resultset as a read-only forward-only DataReader.
		/// IMPORTANT: Ideally use a "using" or "foreach" otherwise, you MUST close this reader using reader.Close() when finished.
		/// </summary>
		/// <example>
		/*
			using (var reader = sql.GetReader()) {
				foreach (DbDataRecord row in reader) {
					int pageid = (int)row["pageID"];
				}
			}
		 */
		/// foreach (DbDataRecord row in sql.GetReader()) {
		///		int pageid = (int)row["pageID"];
		/// }

		/// </example>
		/// <returns></returns>
		public DbDataReader GetReader() {
			if (SqlConnectionString == null) {
				return GetReader(BewebData.GetConnectionString(), true, true);
			} else {
				return GetReader(SqlConnectionString, true, true);
			}

		}
		public DbDataReader GetReader(string connectionString, bool useCS, bool throwError) {
			string commandText = this.Value;
			if (EnablePaging) {
				commandText = AddPagingToSql();
			}
			DbDataReader dr = null;
			if (connectionString == null) { connectionString = BewebData.GetConnectionString(); useCS = true; }
			var conn = BewebData.CreateNewConnection(connectionString, useCS);
			var cmd = conn.CreateCommand();
			cmd.CommandText = commandText;
#if Logging
			LogStartQuery("Sql GetReader");
#endif
			try {
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.KeyInfo);
			} catch (SqlException e) {
				if (throwError) {
					throw new BewebDataException("Beweb SQL Error: " + e.Message + "\n\nSQL: " + commandText);
				} else {
					lastError = "Beweb SQL Error: " + e.Message + "\n\nSQL: " + commandText;
				}
			}

			if (dr == null || !dr.HasRows) {
				_totalRowCount = 0;     // there are none
			} else {
				_totalRowCount = -1;    // cannot be calculated yet - as this is a reader it cannot be done till the reader is reading so UpdateTotalRowCount is called later by callers eg activerecordloader
			}

			if (dr != null && EnablePaging && (ResultSetPagingType == PagingType.software || ResultSetPagingType == PagingType.sql2000)) {
				MoveToPage(dr, PageNum, ItemsPerPage);
			}
#if Logging
			LogEndQuery(commandText);
#endif
			return dr;
		}

		/// <summary>
		/// this has been copied from base type extn to avoid coupling
		/// </summary>
		private static DbDataReader MoveToPage(DbDataReader dataReader, int pageNum, int itemsPerPage) {
			// pageNum is one-based
			int pos = itemsPerPage * (pageNum - 1);
			for (int i = 1; i <= pos; i++) {
				if (!dataReader.Read()) break;
			}
			return dataReader;
		}


		private string AddPagingToSql() {
			// MN 20101216 now returns string instead of changing the property
			string sql = Value.Trim();
			string startBit = "select";
			if (sql.ToLower().StartsWith("select distinct")) {
				startBit = "select distinct";
			}
			if (ResultSetPagingType == PagingType.sql2005) {
				string orderby = GetOrderByClause();
				if (orderby.IsNotBlank()) {
					sql = sql.Replace(orderby, "");
				} else {
					throw new Exception("The Sql should have an ORDER BY clause if you are going to use paging. It is a requirement when using ResultSetPagingType of PagingType.sql2005. If you do not want an ORDER BY clause you can change the ResultSetPagingType to PagingType.sql2000 instead.\n\nSql: " + Value);
				}
				/*
								if (startBit=="select distinct") {
									throw new Exception("SELECT DISTINCT does not work when using ResultSetPagingType of PagingType.sql2005. Either change to use GROUP BY or change the ResultSetPagingType to PagingType.sql2000 instead.\n\nSql: "+Value);
								}
								sql = sql.ReplaceFirst(startBit, "select row_number() OVER (" + orderby + ") as row, ");
								sql = "WITH resultswithnumbers AS (" + sql;
								sql = sql + ") "+startBit+" * from resultswithnumbers where row between " + (((PageNum - 1) * ItemsPerPage) + 1).ToString() + " and " + (PageNum * ItemsPerPage).ToString();
				*/
				//MK: have wrapped in extra select RR so that distincts and sub-selects work OK - also simpler as initial Value stays intact (minus order by)
				string completeSql = "WITH resultswithnumbers AS ( select RR.*, row_number() OVER (" + orderby + ") as row, TotalRowCount=Count(*) OVER() FROM (";
				completeSql += sql;
				completeSql += ") RR ) select * from resultswithnumbers where row between " + (((PageNum - 1) * ItemsPerPage) + 1).ToString() + " and " + (PageNum * ItemsPerPage).ToString();
				sql = completeSql;

			} else if (ResultSetPagingType == PagingType.sql2000) {
				sql = sql.ReplaceFirst(startBit, startBit + " top " + (PageNum * ItemsPerPage).ToString());
			}
			return sql;
		}

		public string GetSeletedColumns() {
			int start = _value.ToLower().IndexOf("select") + 6;
			int end = _value.ToLower().IndexOf("from");
			return _value.Substring(start, (end - start)).Trim();
		}

		public string GetOrderByClause() {
			if (this._value.ToLower().Contains("order by")) {
				int idx = this._value.ToLower().IndexOf("order by");
				return this._value.Substring(idx).Trim();
			}

			return "";
		}

		public string GetSelectedTables() {
			string sqlLower = _value.ToLower();
			int start = sqlLower.IndexOf("from") + 4;
			int end;
			if (sqlLower.Contains("where")) {
				end = sqlLower.IndexOf("where");
			} else if (sqlLower.Contains("group by")) {
				end = sqlLower.IndexOf("group by");
			} else if (sqlLower.Contains("order by")) {
				end = sqlLower.IndexOf("order by");
			} else {
				end = sqlLower.Length;
			}

			return _value.Substring(start, (end - start)).Trim();
		}
		/// <summary>
		/// Execute SQL and return resultset as a read-write forward-back DataReader. 
		/// This reads the whole result set into memory. You don't need to close it after.
		/// <example>sql.GetDataTable()</example>
		/// </summary>
		/// <example>
		///<code>
		///<![CDATA[
		///DataTable dt = sql.GetDataTable();
		///for(int sc = 0;sc<dt.Rows.Count;sc++)
		///{
		///  var dr = dt.Rows[sc];
		///  string value = dr["Picture"];
		///}
		///var aircraftlist = new Sql("select ... group by aircrafttype order by aircrafttype").GetDataTable();
		///jobSelectorAircraftType.Items.Add(new ListItem("--please select--",""));
		///foreach(DataRow item in aircraftlist.Rows)
		///{
		///  jobSelectorAircraftType.Items.Add(new ListItem(item["aircrafttype"]));
		///}
		/// ]]>
		/// </code>
		/// </example>
		/// <returns>table</returns>
		public DataTable GetDataTable() {
			string connectionString = BewebData.GetConnectionString();
			if (this.SqlConnectionString.IsBlank()) this.SqlConnectionString = connectionString;
			return GetDataTable(this.SqlConnectionString);
		}

		public DataTable GetDataTable(string connectionString) {
			// 20120801 MN - possibly change to this more efficient code?
			//var dt = new DataTable();
			//dt.Load(GetReader());
			//LogEndQuery(this.Value);
			//return dt;

			SqlDataSource ds = new SqlDataSource();

#if Logging
			LogStartQuery("GetDataTable");
#endif
			string commandText = this.Value;
			if (EnablePaging) {
				commandText = AddPagingToSql();
			}
			ds.DataSourceMode = SqlDataSourceMode.DataSet;
			ds.CancelSelectOnNullParameter = false;
			ds.ConnectionString = connectionString;
			ds.Selecting += OnGetReaderSelecting;				//sort out the command timeout 
			ds.SelectCommand = commandText;

			DataView dataSet;
			try {
				dataSet = (DataView)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in Sql.GetDataTable: " + e.Message + " in [" + this._value + "]";
				throw new Exception(errorMessage, e);
			}
#if Logging
			LogEndQuery(commandText);
#endif

			DataTable dataTable = dataSet.Table;
			UpdateTotalRowCount(dataTable);
			return dataTable;
		}


		/// <summary>
		/// Execute SQL and return the first row as a Hashtable (ie a dictionary) containing key/value pairs of fieldnames/data. All data in the hashtable is returned as strings.
		/// </summary>
		/// <returns></returns>
		public Hashtable GetHashtable() {
#if Logging
			LogStartQuery("GetHashtable");
#endif

			Hashtable returnValue = new Hashtable();
			using (var dr = this.GetReader()) {
				if (dr != null) {
					if (dr.Read()) {
						for (int i = 0; i < dr.FieldCount; i++) {
							// sql joins can return two fields of the same name
							if (!returnValue.ContainsKey(dr.GetName(i))) {
								returnValue.Add(dr.GetName(i), dr.GetValue(i).ToString());
							}
						}
					}
					dr.Close();
				}
			}
#if Logging
			LogEndQuery();
#endif

			return returnValue;
		}

		/// <summary>
		/// Execute SQL SELECT statement and return a comma delimited string consisting of values in given field from all rows. The delimiter is ", " (ie comma-plus-space).
		/// </summary>
		/// <returns></returns>
		public string GetDelimited() {
			return GetDelimited(", ");
		}

		/// <summary>
		/// Execute SQL SELECT statement and return a delimited string consisting of values in given field from all rows. You can specify the delimiter.
		/// </summary>
		/// <param name="delim"></param>
		/// <returns></returns>
		public string GetDelimited(string delim) {
			string str = "";
			var sql = new Sql("", this._value.SqlizeRAW(true));
			sql.SqlConnectionString = this.SqlConnectionString;//preserve connstr in new raw sql object
			using (var reader = sql.GetReader()) {
				foreach (DbDataRecord row in reader) {
					if (str != "") {
						str += delim;
					}
					str += row[0].ToString();
				}
				reader.Close();
			}
			return str;
		}

		/// <summary>
		/// Executes the SQL statement and does not retreive any records. Returns number of records affected.
		/// This should be used for UPDATE, INSERT, DELETE, ALTER or any other non-SELECT statements.
		/// </summary>
		/// <returns></returns>
		public int Execute() {
			return Execute(null);
		}

		/// <summary>
		/// Executes the SQL statement and does not retreive any records. Returns number of records affected.
		/// This should be used for UPDATE, INSERT, DELETE, ALTER or any other non-SELECT statements.
		/// </summary>
		/// <returns></returns>
		public int Execute(int? timeoutSecs) {
			string connectionString = BewebData.GetConnectionString();
			if (this.SqlConnectionString != null) connectionString = this.SqlConnectionString;	//preserve connstr in new raw sql object
			var conn = new SqlConnection(connectionString);
			conn.Open();
			var cmd = conn.CreateCommand();
			cmd.CommandText = this._value;
			if (timeoutSecs != null) {
				// note the system default is 30 sec
				cmd.CommandTimeout = timeoutSecs.Value;
			}
#if Logging
			LogStartQuery("Sql Execute");    // MN 20130528 - removed this._value
#endif
			if (_value.IsBlank()) {
				throw new BewebDataException("Sql.Execute: Sql text is blank.");
			}
			int numRowsAffected = 0;
			try {
				numRowsAffected = cmd.ExecuteNonQuery();
			} catch (SqlException e) {
				throw new Exception(e.Message + "\nSql [" + _value + "]");
			} finally {
				conn.Close();
			}
#if Logging
			LogEndQuery();
#endif

			if (_value.StartsWith("CREATE TABLE") || _value.StartsWith("create table") || (_value.StartsWith("IF NOT EXISTS") && _value.Contains("CREATE TABLE"))) {
				// clear cache of tables that exist
				Web.PageGlobals["BewebData_TableNames"] = null;
				Web.PageGlobals["BewebData_TableAndViewNames"] = null;
			}

			return numRowsAffected;
		}
		public DbDataReader MoveToIndex(int moveToPosn) {
			DbDataReader result;
			using (result = GetReader()) {
				bool recordFound = false;
				for (int i = 0; i < moveToPosn; i++) {
					//moving
					recordFound = result.Read();
				}
				if (!recordFound) { result = null; }
				return result;
			}
		}

#if BaseTypeExtensions
		/// <summary>
		/// Enables paging. This means the query will only return a pageful of results, given the page number and page size.
		/// This feature applies only to sql.GetReader() and ActiveRecordList.Load(sql) at present.
		/// Page numbers are one-based (there is no page zero).
		/// </summary>
		public Sql Paging(int itemsPerPage) {
			return this.Paging(itemsPerPage, Web.Request["PageNum"].ToInt(1));
		}
#endif
		/// <summary>
		/// Enables paging. This means the query will only return a pageful of results, given the page number and page size.
		/// This feature applies only to sql.GetReader() and ActiveRecordList.Load(sql) at present.
		/// Page numbers are one-based (there is no page zero).
		/// </summary>
		public Sql Paging(int itemsPerPage, int pageNum) {
			this.PageNum = pageNum;
			this.ItemsPerPage = itemsPerPage;
			this.EnablePaging = true;
			return this;
		}

		public int PageNum { get; private set; }
		public int ItemsPerPage { get; private set; }
		public bool EnablePaging { get; private set; }

		/*
	declare @StartRow int
	declare @MaxRows int

	select @StartRow = 1
	select @MaxRows = 10

	 select *
		 from
		 (select o.*,u.FirstName,u.LastName,
				 TotalRows=Count(*) OVER(), 
				 ROW_NUMBER() OVER(ORDER BY o.CreateDateTime desc) as RowNum
				 from Orders o , Users u
	 WHERE o.CreateDateTime > getdate() -30
				AND (o.UserID = u.UserID)
		 )
		 WHERE RowNum BETWEEN @StartRow AND (@StartRow + @MaxRows) -1
*/

#if ActiveRecord
		/// <summary>
		/// Adds WHERE clause (beginning with WHERE) appropriate to the given active record type. 
		/// This defaults based on naming conventions (eg IsActive, PublishDate, ExpiryDate) or you can override MyModel.GetSqlWhereActive() in the model partial (this will then be used by MyModel.LoadActive() and other routines). If overriding MyModel.GetSqlWhereActive() you should also override MyModel.GetIsActive().
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		public Sql WhereIsActive<TActiveRecord>() where TActiveRecord : ActiveRecord, new() {
			var sqlWhere = (new TActiveRecord()).GetSqlWhereActive();
			this.AddSql(sqlWhere);
			return this;
		}

		/// <summary>
		/// Adds AND clause (beginning with AND) appropriate to the given active record type. 
		/// Same as sql.WhereIsActive except it starts with AND instead of WHERE.
		/// This defaults based on naming conventions (eg IsActive, PublishDate, ExpiryDate) or you can override MyModel.GetSqlWhereActive() in the model partial (this will then be used by MyModel.LoadActive() and other routines). If overriding MyModel.GetSqlWhereActive() you should also override MyModel.GetIsActive().
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		public Sql AndIsActive<TActiveRecord>() where TActiveRecord : ActiveRecord, new() {
			var sqlWhere = (new TActiveRecord()).GetSqlWhereActive();
			string andString = sqlWhere.Value.ReplaceFirst("where", "and");
			this.AddRawSqlString(andString);
			return this;
		}

#endif
		/// <summary>
		/// Adds AND clause (beginning with AND) for CURRENT DATE between first date and last date inclusive (ie including from the beginning of the first date up until the end of the last date), assuming date fields contain whole day values (ie NO TIMES STORED IN FIELDS).
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		public Sql AndDateRange(string beginDateFieldName, string endDateFieldName) {
			Add("and (", beginDateFieldName.SqlizeName(), " is not null and GETDATE() > ", beginDateFieldName.SqlizeName(), ")");
			Add("and (", endDateFieldName.SqlizeName(), " is null or GETDATE() < ", endDateFieldName.SqlizeName(), "+1)");
			return this;
		}

		/// <summary>
		/// Adds AND clause (beginning with AND) for GIVEN DATE between first date and last date inclusive (ie including from the beginning of the first date up until the end of the last date), assuming date fields contain whole day values (ie NO TIMES STORED IN FIELDS).
		/// </summary>
		/// <typeparam name="TActiveRecord"></typeparam>
		public Sql AndDateRange(string beginDateFieldName, string endDateFieldName, DateTime givenDate) {
			Add("and (", beginDateFieldName.SqlizeName(), " is not null and ", givenDate, " > ", beginDateFieldName.SqlizeName(), ")");
			Add("and (", endDateFieldName.SqlizeName(), " is null or ", givenDate, " < ", endDateFieldName.SqlizeName(), "+1)");
			return this;
		}


#if ActiveRecord

		public ActiveRecordList<ActiveRecord> GetActiveRecordList() {
			var list = new ActiveRecordList<ActiveRecord>();
			list.LoadRecords(this);
			return list;
		}

		public ActiveRecordList<ActiveRecord> GetActiveRecordList(string tableName, string pkName) {
			var list = new ActiveRecordList<ActiveRecord>();
			list.SetTableName(tableName);
			list.SetPrimaryKeyName(pkName);
			list.LoadRecords(this);
			return list;
		}

		public ActiveRecordList<TActiveRecord> GetActiveRecordList<TActiveRecord>() where TActiveRecord : ActiveRecord, new() {
			var list = new ActiveRecordList<TActiveRecord>();
			list.LoadRecords(this);
			return list;
		}

		public TActiveRecordList Load<TActiveRecordList>() where TActiveRecordList : ActiveRecordList<ActiveRecord>, new() {
			var list = new TActiveRecordList();
			list.LoadRecords(this);
			return list;
		}
#endif

#if Logging
		/// <summary>
		/// Logging and tracing internal utils
		/// </summary>

		private DateTime executionStartTime, executionEndTime;
		public bool DisableLogging { get; set; }

		protected void LogStartQuery(string message) {
			if (!DisableLogging && Web.IsRequestInitialised) {
				executionStartTime = DateTime.Now;
				// always save the latest query which can be used in error messages
				Web.PageGlobals["SqlTraceQuerySource"] = message;
				Web.PageGlobals["SqlTraceLastQuery"] = this.Value;
				// also log first and last query times
				LogPageProcessingStart();
			}
		}

		public static void LogPageProcessingStart() {
			if (Web.IsRequestInitialised) {
				if (Web.PageGlobals["SqlTraceFirstQueryTime"] == null) {
					Web.PageGlobals["SqlTraceFirstQueryTime"] = DateTime.Now;
					Web.PageGlobals["SqlTraceLastQueryTime"] = DateTime.Now;
					// need to log this here in case crashes during sql statement
				}
			}
		}

		public static void LogPageProcessingEnd() {
			if (Web.IsRequestInitialised) {
				Web.PageGlobals["SqlTraceLastQueryTime"] = DateTime.Now;
			}
		}

		protected void LogEndQuery() {
			LogEndQuery(this.Value);
		}

		private static void AddLogLine(string description, double? durationMilliseconds, string source) {
			string timing = durationMilliseconds == null ? null : Math.Round(durationMilliseconds.Value) + " ms";
			traceLog.Add(new string[] { description, timing, source });
		}

		private static List<string[]> traceLog {
			get {
				if (Web.PageGlobals["SqlTraceLog"] == null) {
					Web.PageGlobals["SqlTraceLog"] = new List<string[]>();
				}
				return (List<string[]>)Web.PageGlobals["SqlTraceLog"];
			}
		}

		protected void LogEndQuery(string sqlQuery) {
			if (!DisableLogging && Web.IsRequestInitialised) {
				executionEndTime = DateTime.Now;
				var duration = executionEndTime - executionStartTime;
				//string message = "<tr><td>" + sqlQuery + "</td><td nowrap>"+Math.Round(duration.TotalMilliseconds) + " ms</td></tr>";
				//if (Web.PageGlobals["SqlTraceLog"] == null) {
				//  Web.PageGlobals["SqlTraceLog"] = new StringBuilder();
				//}
				//Web.PageGlobals["SqlTraceLog"] = Web.PageGlobals["SqlTraceLog"] + message + "";
				//((System.Text.StringBuilder)Web.PageGlobals["SqlTraceLog"]).Append("<tr><td>" + sqlQuery + "</td><td nowrap>-- " + Math.Round(duration.TotalMilliseconds) + " ms</td><td width=100>"+Web.PageGlobals["SqlTraceQuerySource"]+"</td></tr>");
				AddLogLine(sqlQuery, duration.TotalMilliseconds, Web.PageGlobals["SqlTraceQuerySource"] + "");
				Web.PageGlobals["SqlTraceQuerySource"] = null;
				// add to aggregate total
				if (Web.PageGlobals["SqlTotalMilliseconds"] == null) {
					Web.PageGlobals["SqlTotalMilliseconds"] = (double)0.0;
				}
				Web.PageGlobals["SqlTotalMilliseconds"] = (double)Web.PageGlobals["SqlTotalMilliseconds"] + duration.TotalMilliseconds;
				// also log first and last query times
				LogPageProcessingEnd();
			}
		}

		internal void LogEndQueryRecordsLoaded(string description) {
			if (!DisableLogging && Web.IsRequestInitialised) {
				var duration = DateTime.Now - executionEndTime;
				AddLogLine(description, duration.TotalMilliseconds, "ActiveRecord");
				//if (Web.PageGlobals["SqlTraceLog"] == null) {
				//  Web.PageGlobals["SqlTraceLog"] = new StringBuilder();
				//}
				//((System.Text.StringBuilder)Web.PageGlobals["SqlTraceLog"]).Append("<tr><td bgcolor='#CCCCCC'>-- " + description + "</td><td nowrap>-- " + Math.Round(duration.TotalMilliseconds) + " ms</td></tr>");
				// add to aggregate loading total
				if (Web.PageGlobals["SqlTotalLoadingMilliseconds"] == null) {
					Web.PageGlobals["SqlTotalLoadingMilliseconds"] = (double)0.0;
				}
				Web.PageGlobals["SqlTotalLoadingMilliseconds"] = (double)Web.PageGlobals["SqlTotalLoadingMilliseconds"] + duration.TotalMilliseconds;
				// also update the last query time
				LogPageProcessingEnd();
			}
		}

		public static void LogOtherEventStart(string eventDescription) {
			if (Web.IsRequestInitialised) {
				if (EventTraceLog == null) return;
				var logEvent = EventTraceLog.Find(ev => ev.EventName == eventDescription);
				if (logEvent == null) {
					logEvent = new TraceEventLog();
					logEvent.EventName = eventDescription;
					EventTraceLog.Add(logEvent);
				}
				logEvent.StartTime = DateTime.Now;
				//if (Web.PageGlobals["SqlTrace_"+eventDescription]==null) {
				//  Web.PageGlobals["SqlTrace_"+eventDescription] = DateTime.Now;
				//}
			}
		}

		public static void LogOtherEventEnd(string eventDescription) {
			if (Web.IsRequestInitialised) {
				if (EventTraceLog == null) return;
				var logEvent = EventTraceLog.Find(ev => ev.EventName == eventDescription);
				if (logEvent != null) {
					logEvent.TimesOccurred++;
					logEvent.TotalMilliseconds += (DateTime.Now - logEvent.StartTime).TotalMilliseconds;
				}
				//if (Web.PageGlobals["SqlTrace_"+eventDescription]!=null) {
				//  DateTime timer = (DateTime) Web.PageGlobals["SqlTrace_"+eventDescription];
				//  Web.PageGlobals["SqlTrace_"+eventDescription] = timer - DateTime.Now;
				//}
			}
		}

		private static List<TraceEventLog> EventTraceLog {
			get {
				if (Web.PageGlobals == null) return null;
				if (Web.PageGlobals["SqlTraceEvents"] == null) {
					Web.PageGlobals["SqlTraceEvents"] = new List<TraceEventLog>();
				}
				return Web.PageGlobals["SqlTraceEvents"] as List<TraceEventLog>;
			}
		}

		private class TraceEventLog {
			public string EventName;
			public double TotalMilliseconds = 0;
			public int TimesOccurred = 0;
			public DateTime StartTime;
		}

		/// <summary>
		/// A utility function to output the log of sql queries to the page. Only outputs if Request["tracesql"]=="1"
		/// </summary>
		public static void OutputTraceLog() {
			if (Web.Request["tracesql"] == "1" && (Util.IsBewebOffice || Util.ServerIsDev)) {
				Web.Response.ContentType = "text/html";
				Web.Write(GetTraceLogHtml());
			}
		}

		/// <summary>
		/// A utility function to get the log of sql queries performed during this request.
		/// </summary>
		public static string GetTraceLogHtml() {
			//log += "<tr><td>Size of Web.Cache: " + Web.Cache.Count + " objects <a href='" + Web.Root + "Tests/?mode=ViewCache' target='_blank'>details...</a><td></tr>";
			var result = new StringBuilder();
			var log = GetTraceLogArray();
			if (log.Count > 0) {
				result.AppendLine("<table style='font-family:arial;font-size:10px;background-color:#eef;' cellspacing=8 id='savvyTraceSqlLog'>");
				foreach (var line in log) {
					string bgcolor = "";
					if (line[2] == "ActiveRecord") {
						bgcolor = " bgcolor='#CCCCCC'";
					} else if (line[2] == "Summary") {
						bgcolor = " bgcolor='#CCCCdd'";
					}
					result.AppendLine("<tr><td" + bgcolor + ">" + line[0] + "</td><td>" + line[1] + "</td><td>" + line[2] + "</td></tr>");
				}
				result.AppendLine("</table>");
			}
			return result.ToString();
		}

		/// <summary>
		/// A utility function to get the log of sql queries performed during this request.
		/// </summary>
		public static List<string[]> GetTraceLogArray() {
			GetLogDetail();
			return traceLog;
		}

		/// <summary>
		/// A utility function to get the log of sql queries performed during this request.
		/// </summary>
		public static string GetTraceLog() {
			var result = new StringBuilder();
			var log = GetTraceLogArray();
			foreach (var line in log) {
				result.AppendLine(line[0] + ": " + line[1] + " (" + line[2] + ")");
			}
			return result.ToString();

			//string log = Web.PageGlobals["SqlTraceLog"] + "";
			//log += "Size of Web.Cache: " + Web.Cache.Count + " objects";
			//if (Web.PageGlobals["SqlTraceFirstQueryTime"] != null) {
			//  double combinedQueryDuration = 0;
			//  if (Web.PageGlobals["SqlTotalMilliseconds"] != null) combinedQueryDuration = Math.Round((double)Web.PageGlobals["SqlTotalMilliseconds"]);
			//  double combinedLoadingDuration = 0;
			//  if (Web.PageGlobals["SqlTotalLoadingMilliseconds"] != null) combinedLoadingDuration = Math.Round((double)Web.PageGlobals["SqlTotalLoadingMilliseconds"]);
			//  DateTime firstQueryTime = ((DateTime)Web.PageGlobals["SqlTraceFirstQueryTime"]);
			//  DateTime lastQueryTime = ((DateTime)Web.PageGlobals["SqlTraceLastQueryTime"]);
			//  DateTime endRenderTime = DateTime.Now;
			//  double millisecondsTotal = Math.Round((endRenderTime - firstQueryTime).TotalMilliseconds);
			//  double millisecondsProcessing = Math.Round((lastQueryTime - firstQueryTime).TotalMilliseconds);
			//  double millisecondsRender = Math.Round((endRenderTime - lastQueryTime).TotalMilliseconds);
			//  double millisecondsUnknownProcessing = millisecondsProcessing - combinedQueryDuration - combinedLoadingDuration;
			//  log = GetLogDetail(log, millisecondsProcessing, combinedQueryDuration, combinedLoadingDuration, millisecondsUnknownProcessing, millisecondsRender, millisecondsTotal);
			//}
			//return log;
		}

		private static void GetLogDetail() {
			if (traceLog.Exists(l => l[0].StartsWith("Size of Web.Cache"))) {
				return;
			}
			AddLogLine("Size of Web.Cache: " + Web.Cache.Count + " objects", 0, "Web.Cache");
			if (Web.PageGlobals["SqlTraceFirstQueryTime"] != null) {
				double combinedQueryDuration = 0;
				if (Web.PageGlobals["SqlTotalMilliseconds"] != null) combinedQueryDuration = Math.Round((double)Web.PageGlobals["SqlTotalMilliseconds"]);
				double combinedLoadingDuration = 0;
				if (Web.PageGlobals["SqlTotalLoadingMilliseconds"] != null) combinedLoadingDuration = Math.Round((double)Web.PageGlobals["SqlTotalLoadingMilliseconds"]);
				DateTime firstQueryTime = ((DateTime)Web.PageGlobals["SqlTraceFirstQueryTime"]);
				DateTime lastQueryTime = ((DateTime)Web.PageGlobals["SqlTraceLastQueryTime"]);
				DateTime endRenderTime = DateTime.Now;
				double millisecondsTotal = Math.Round((endRenderTime - firstQueryTime).TotalMilliseconds);
				double millisecondsProcessing = Math.Round((lastQueryTime - firstQueryTime).TotalMilliseconds);
				double millisecondsRender = Math.Round((endRenderTime - lastQueryTime).TotalMilliseconds);
				double millisecondsUnknownProcessing = millisecondsProcessing - combinedQueryDuration - combinedLoadingDuration;

				AddLogLine("Time spent Sql queries", combinedQueryDuration, "Summary");
				AddLogLine("Time spent ActiveRecord loading", combinedLoadingDuration, "Summary");
				foreach (var loggedEvent in EventTraceLog) {
					AddLogLine("Time spent " + loggedEvent.EventName + " (x " + loggedEvent.TimesOccurred + ")", loggedEvent.TotalMilliseconds, "Summary");
					millisecondsUnknownProcessing -= loggedEvent.TotalMilliseconds;
				}
				AddLogLine("Time spent in other code", millisecondsUnknownProcessing, "Summary");
				AddLogLine("Total processing time (first query to last query)", millisecondsProcessing, "Summary");
				AddLogLine("Total render output time (last query to now)", millisecondsRender, "Summary");
				AddLogLine("Total time (first query to now)", millisecondsTotal, "Summary");
			}
		}
#endif
		/// <summary>
		/// Executes the query and returns true if the query returns any results.
		/// </summary>
		/// <returns></returns>
		public bool RecordExists() {
			bool returnVal;
			using (DbDataReader dbDataReader = this.GetReader()) {
				using (var numRows = dbDataReader) {
					returnVal = numRows.HasRows;
				}
			}
			return returnVal;
		}

		/// <summary>
		/// Executes a COUNT(*) version of the current SQL statement. Also stores this in RowCount property in case you need it again.
		/// If a FetchCount() was already called, the previous value is used and if will not be executed twice.
		/// </summary>
		/// <returns></returns>
		public int FetchCount() {
			if (_totalRowCount == -1) {
				_totalRowCount = this.GetCountSql().FetchIntOrZero();
			}
			return _totalRowCount;
		}

		/// <summary>
		/// Calculates the number of pages by executing a COUNT(*) version of the current SQL statement and dividing by the number of items per page - which you must previously set using sql.Paging(numItemsPerPage). 
		/// If a FetchCount() was already called, the previous value is used and if will not be executed twice.
		/// </summary>
		/// <returns></returns>
		public int FetchPageCount() {
			if (_totalRowCount == -1) {
				FetchCount();
			}
			return PageCount;
		}

		private int _totalRowCount = -1;

		/// <summary>
		/// Returns the number of records that match the SQL statement. If there is paging applied, this is the total number of records before paging was applied. This property is only available after you have called FetchCount() or FetchPageCount().
		/// </summary>
		public int TotalRowCount {
			get {
				if (!EnablePaging) {
					throw new ProgrammingErrorException("Beweb.Sql.PageCount: first you must enable paging on this sql command using sql.Paging(numItemsPerPage)");
				}
				if (_totalRowCount == -1) {
					throw new ProgrammingErrorException("Beweb.Sql.PageCount: first you must call FetchCount() or FetchPageCount()");
				}
				return _totalRowCount;
			}
			internal set { _totalRowCount = value; }
		}

		/// <summary>
		/// If paging is enabled, returns the number of pages of data that match the SQL statement, calculated from the ItemsPerPage property and the TotalRowCount.
		/// </summary>
		public int PageCount {
			get {
				if (!EnablePaging) {
					throw new ProgrammingErrorException("Beweb.Sql.PageCount: first you must enable paging on this sql command using sql.Paging(numItemsPerPage)");
				}
				if (_totalRowCount == -1) {
					throw new ProgrammingErrorException("Beweb.Sql.PageCount: first you must call FetchCount() or FetchPageCount()");
				}
				int itemsPerPage = this.ItemsPerPage;
				int numResults = this.TotalRowCount;
				double numPages = ((numResults - 1) / itemsPerPage) + 1;
				return Convert.ToInt32(Math.Floor(numPages));
			}
		}

		/// <summary>
		/// Hook to allow ActiveRecord or to update the record count.
		/// </summary>
		internal void UpdateTotalRowCount(int resultRowCount, DbDataReader reader) {
			if (!EnablePaging) {
				_totalRowCount = resultRowCount;
			} else if (_totalRowCount == -1 && reader != null && ResultSetPagingType == PagingType.sql2005 && BewebData.FieldExists(reader, "TotalRowCount")) {
				_totalRowCount = reader["TotalRowCount"].ToInt(0);
			}
		}

		/// <summary>
		/// Hook to allow GetDataTable to update the record count.
		/// </summary>
		private void UpdateTotalRowCount(DataTable dataTable) {
			if (!EnablePaging) {
				_totalRowCount = dataTable.Rows.Count;
			} else if (_totalRowCount == -1 && ResultSetPagingType == PagingType.sql2005 && dataTable.Columns.Contains("TotalRowCount")) {
				_totalRowCount = dataTable.Rows[0]["TotalRowCount"].ToInt(0);
			}
		}

	}
}
#if TestExtensions
namespace BewebTest {
	[TestClass]
	public class SqlTest {

		[TestMethod]
		public void TestSqlizeNull() {
			Assert.AreEqual("null", Sql.Sqlize(null).value);
		}

		[TestMethod]
		public void TestSqlFetch() {
			int? id = new Sql("select 5").FetchInt();
			Assert.AreEqual(5, id);
		}

		[TestMethod]
		public void TestNullableEquality() {
			int? ni = 88;
			int i = 88;
			HttpContext.Current.Response.Write(i == ni);
		}

		[TestMethod]
		public void TestAreEquality() {
			Assert.AreEqual(66, 66);
		}

		[TestMethod]
		public void TestAreEqualityString() {
			Assert.AreEqual("hw", "hw");
		}

		[TestMethod]
		public void TestAreEqualityNullable() {
			int? ni = 88;
			int i = 88;
			Assert.AreEqual(i, ni);
		}

		//[TestMethod]
		//public void TestAreEqualityStringNull() {
		//  Assert.AreEqual("ss", null);
		//}

		[TestMethod]
		public void TestAreEqualityNull2() {
			int? i = null;
			int? j = null;
			Assert.AreEqual(i, j);
		}

		[TestMethod]
		public void TestSqlFetchNone() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}

			int? id = new Sql("select * from gentest where bodycopy like ", "mikenotthere@b".SqlizeLike()).FetchInt();
			Assert.AreEqual(null, id);
		}

		[TestMethod]
		public void TestGetCountSql() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			var sql = new SqlStringBuilder("select * from gentest where title=", "mike".SqlizeText(), "and isactive=", true);
			sql.Add("order by isactive, name desc");
			var countSql = sql.GetCountSql();
			Assert.AreEqual("select COUNT (*) from (select * from gentest where title= N'mike' and isactive= 1) as countSection", countSql.ToString());
			Assert.AreEqual("select * from gentest where title= N'mike' and isactive= 1 order by isactive, name desc", sql.ToString());
		}

		[TestMethod]
		public void TestSqlFetchBool() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			bool isActive;
			//new Sql("update gentest set isactive=null").Execute();
			//bool isActive = new Sql("select isactive from gentest").FetchBool();
			//Assert.AreEqual(isActive, false);

			new Sql("update gentest set isactive=0").Execute();
			isActive = new Sql("select isactive from gentest").FetchBool();
			Assert.AreEqual(isActive, false);

			new Sql("update gentest set isactive=1").Execute();
			isActive = new Sql("select isactive from gentest").FetchBool();
			Assert.AreEqual(isActive, true);
		}

		[TestMethod]
		public void TestSqlFetchDate() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			new Sql("update gentest set dateadded=null").Execute();
			DateTime? date = new Sql("select dateadded from gentest").FetchDate();
			Assert.IsNull(date);

			new Sql("update gentest set dateadded=", DateTime.Parse("1 Jul 2010")).Execute();
			date = new Sql("select dateadded from gentest").FetchDate();
			Assert.AreEqual(date, DateTime.Parse("1 Jul 2010"));
		}

		[TestMethod]
		public void TestSqlFetchDateOrDie() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			if (new Sql("select count(*) from gentest").FetchIntOrZero() == 0) {
				new Sql("insert into gentest (title, dateadded) values (", "testdates".Sqlize_Text(), ",", DateTime.Now, ")").Execute();
			}


			new Sql("update gentest set dateadded=", DateTime.Parse("1 Jul 2010")).Execute();
			DateTime date = new Sql("select dateadded from gentest").FetchDateOrDie();
			Assert.AreEqual(date, DateTime.Parse("1 Jul 2010"));

			DateTime? dateNullable = new Sql("select dateadded from gentest").FetchDateOrDie();
			Assert.AreEqual(dateNullable, DateTime.Parse("1 Jul 2010"));

			new Sql("update gentest set dateadded=null").Execute();
			try {
				DateTime date2 = new Sql("select dateadded from gentest").FetchDateOrDie();
				Assert.Fail("Should have thrown an exception, actually was [" + date2.ToString() + "]");
			} catch (Exception) {
				Assert.Pass();
			}
		}

		[TestMethod]
		public void TestSqlFetchIntOrZero() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			new Sql("update gentest set NumberOfStaff=", 5).Execute();
			int value1 = new Sql("select NumberOfStaff from gentest").FetchIntOrZero();
			Assert.AreEqual(value1, 5);

			int? value2 = new Sql("select NumberOfStaff from gentest").FetchIntOrZero();
			Assert.AreEqual(value2, 5);

			//new Sql("update gentest set NumberOfStaff=null").Execute();
			//try {
			//  int value3 = new Sql("select NumberOfStaff from gentest").FetchIntNotNull();
			//  Assert.Fail("Should have thrown an exception, actually was [" + value3.ToString() + "]");
			//}
			//catch (Exception ex) {
			//  Assert.Pass();
			//}
		}

		[TestMethod]
		public void TestSqlFetchDecimalOrZero() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			new Sql("update gentest set Cost=", 5).Execute();
			Decimal value1 = new Sql("select Cost from gentest").FetchDecimalOrZero();
			Assert.AreEqual(value1, 5);

			Decimal? value2 = new Sql("select Cost from gentest").FetchDecimalOrZero();
			Assert.AreEqual(value2, 5);

			//new Sql("update gentest set Cost=null").Execute();
			//try {
			//  Decimal value3 = new Sql("select Cost from gentest").FetchDecimalNotNull();
			//  Assert.Fail("Should have thrown an exception, actually was [" + value3.ToString() + "]");
			//}
			//catch (Exception ex) {
			//  Assert.Pass();
			//}
		}

		[TestMethod]
		public void TestSqlFetchGuidOrDie() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			Guid newValue = new Guid();
			new Sql("update gentest set GUI=", newValue).Execute();
			Guid value1 = new Sql("select GUI from gentest").FetchGuidOrDie();
			Assert.AreEqual(value1, newValue);

			Guid? value2 = new Sql("select GUI from gentest").FetchGuidOrDie();
			Assert.AreEqual(value2, newValue);

			new Sql("update gentest set GUI=null").Execute();
			try {
				Guid value3 = new Sql("select GUI from gentest").FetchGuidOrDie();
				Assert.Fail("Should have thrown an exception, actually was [" + value3.ToString() + "]");
			} catch (Exception) {
				Assert.Pass();
			}
		}

		[TestMethod]
		public void TestSqlFetchDoubleOrZero() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			new Sql("update gentest set Ratio=", 5).Execute();
			double value1 = new Sql("select Ratio from gentest").FetchDoubleOrZero();
			Assert.AreEqual(value1, 5);

			double? value2 = new Sql("select Ratio from gentest").FetchDoubleOrZero();
			Assert.AreEqual(value2, 5);

			//new Sql("update gentest set Ratio=null").Execute();
			//try {
			//  double value3 = new Sql("select Ratio from gentest").FetchDoubleNotNull();
			//  Assert.Fail("Should have thrown an exception, actually was [" + value3.ToString() + "]");
			//} catch (Exception ex) {
			//  Assert.Pass();
			//}
		}

		[TestMethod]
		public void TestSqlExecute() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			new Sql("delete from GenTest").Execute();
			int? count = new Sql("select count(*) from GenTest").FetchInt();
			Assert.AreEqual(count, 0);
			new Sql("insert into GenTest (title) values (", "mike".SqlizeText(), ")").Execute();
			new Sql("insert into GenTest (title) values (", "jeremy".SqlizeText(), ")").Execute();
			int? count2 = new Sql("select count(*) from GenTest").FetchInt();
			Assert.AreEqual(count2, 2);
			Assert.AreEqual("jeremy", new Sql("select title from GenTest order by title").FetchString());

			HttpContext.Current.Response.Write("<br>Test GetReader()");
			using (var reader = new Sql("select * from GenTest").GetReader()) {
				while (reader.Read()) {
					HttpContext.Current.Response.Write(reader["Title"]);
				}
				reader.Close();
			}
			HttpContext.Current.Response.Write("<br>Test GetDelimited()");
			string titles = new Sql("select title from GenTest").GetDelimited();
			Assert.AreEqual("mike, jeremy", titles);
		}


		//[TestMethod]
		//public void TestSqlGetDataTable() {
		//  new Sql("delete from GenTest").Execute();
		//  new Sql("insert into GenTest (title) values (", "mike".SqlizeText(), ")").Execute();
		//  new Sql("insert into GenTest (title) values (", "jeremy".SqlizeText(), ")").Execute();

		//  var sql = new Sql("select * from GenTest");
		//  var dt = sql.GetDataTable();
		//  foreach (DataRow row in dt.Rows) {
		//    HttpContext.Current.Response.Write(row["Title"]);
		//  }
		//}


		[TestMethod]
		public void TestSqlize() {
			Assert.IsInstanceOfType("o'brien".SqlizeText(), typeof(SqlizedValue));
			Assert.AreEqual("N'o''brien'", "o'brien".SqlizeText().value);
			Assert.AreEqual("N'this is a test'", "this is a test".SqlizeText().value);
			if (Fmt.DefaultDateFormatHasDashes) {
				Assert.AreEqual("'25-May-2009'", new DateTime(2009, 5, 25).SqlizeDate().value);
				Assert.AreEqual("'25-May-2009 5:21:11.000pm'", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime().value);
				Assert.AreEqual("'25-May-2009 5:21:11.250pm'", new DateTime(2009, 5, 25, 17, 21, 11, 250).SqlizeDateTime().value);
			} else {
				Assert.AreEqual("'25 May 2009'", new DateTime(2009, 5, 25).SqlizeDate().value);
				Assert.AreEqual("'25 May 2009 5:21:11.000pm'", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime().value);
				Assert.AreEqual("'25 May 2009 5:21:11.250pm'", new DateTime(2009, 5, 25, 17, 21, 11, 250).SqlizeDateTime().value);
			}

			Assert.AreEqual("N'this is a test'", "this is a test".SqlizeText().value);
			Assert.AreEqual("1234.567", "1234.567".SqlizeNumber().value);
			Assert.AreEqual("1234.567", 1234.567.SqlizeNumber().value);
			Assert.AreEqual("1234", 1234.SqlizeNumber().value);
			Assert.AreEqual("1234", ((decimal)1234).SqlizeNumber().value);
			Assert.AreEqual("N'%something%'", "something".SqlizeLike().value);
			Assert.AreEqual("N'something%'", "something".SqlizeLike("", "%").value);
			Assert.AreEqual("N'some%thing%'", "thing".SqlizeLike("some%", "%").value);
			Assert.AreEqual("0", "false".SqlizeBool().value);
			Assert.AreEqual("0", false.SqlizeBool().value);
			Assert.AreEqual("1", true.SqlizeBool().value);

			int[] myInts = { 1, 2, 3 };
			Assert.AreEqual("1,2,3", myInts.SqlizeNumberList().value);
			Assert.AreEqual("1,2,3", "1,2,3".SqlizeNumberList().value);

			double[] myDoubles = { 1.0, 2.0, 3.5 };
			Assert.AreEqual("1,2,3.5", myDoubles.SqlizeNumberList().value);
			Assert.AreEqual("1,2,3.5", "1,2,3.5".SqlizeNumberList().value);

			string[] myStrings = { "red", "blue", "green", "yellow" };
			Assert.AreEqual("N'red',N'blue',N'green',N'yellow'", myStrings.SqlizeTextList().value);
			Assert.AreEqual("N'red',N'blue',N'green',N'yellow'", "red,blue,green,yellow".SqlizeTextList().value);
			Assert.AreEqual("N'red',N'blue',N'green',N'yellow'", "red, blue , green, yellow ".SqlizeTextList().value);

			Assert.AreEqual("'{7AB106C9-D9AF-453A-8654-514460643788}'", "{7AB106C9-D9AF-453A-8654-514460643788}".SqlizeGuid().value);
			Assert.AreEqual("'{7AB106C9-D9AF-453A-8654-514460643788}'", new Guid("{7AB106C9-D9AF-453A-8654-514460643788}").SqlizeGuid().value);

			Assert.AreEqual("'{7AB106C9-D9AF-453A-8654-514460643788}','{7AB106C9-D9AF-453A-8654-514460643788}'", "{7AB106C9-D9AF-453A-8654-514460643788}, {7AB106C9-D9AF-453A-8654-514460643788}".SqlizeGuidList().value);
		}
		[TestMethod]
		public void TestSqlizeIdentifier() {
			Assert.AreEqual("[GenTest]", "GenTest".SqlizeName().value);
		}

		[TestMethod]
		public void TestSqlAdd() {
			SqlStringBuilder sql = new SqlStringBuilder();
			sql.Add("select * from story where datepublished > ", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime(), " and name=", "mike".SqlizeText());
			string expected = "select * from story where datepublished > '25 May 2009 5:21:11.000pm' and name= N'mike'";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("25 May 2009 5:21:11.000pm", "25-May-2009 5:21:11.000pm");
			}
			Assert.AreEqual(expected, sql.Value);

			var sqlsb = new SqlStringBuilder("select * from story where datepublished > ", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime(), " and name=", "mike".SqlizeText());
			expected = "select * from story where datepublished > '25 May 2009 5:21:11.000pm' and name= N'mike'";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("25 May 2009 5:21:11.000pm", "25-May-2009 5:21:11.000pm");
			}
			Assert.AreEqual(expected, sqlsb.Value);

			sql = new SqlStringBuilder();
			sql.Add("select * from story where datepublished > ", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime(), " and name=", "mike".SqlizeText());
			expected = "select * from story where datepublished > '25 May 2009 5:21:11.000pm' and name= N'mike'";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("25 May 2009 5:21:11.000pm", "25-May-2009 5:21:11.000pm");
			}
			Assert.AreEqual(expected, sql.Value);

			sql = new SqlStringBuilder();
			sql.Add("select * from story where datepublished > ", new DateTime(2009, 5, 25, 17, 21, 11), " and name=", "mike".SqlizeText());
			expected = "select * from story where datepublished > '25 May 2009 5:21:11.000pm' and name= N'mike'";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("25 May 2009 5:21:11.000pm", "25-May-2009 5:21:11.000pm");
			}
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddMultiple() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where 1=1");
			sql.Add("and title like", "mike".SqlizeLike());
			sql.Add("and pubdate between", new DateTime(2009, 5, 25, 17, 21, 11).SqlizeDateTime(), "and", new DateTime(2010, 5, 25, 17, 21, 11).SqlizeDate());
			sql.Add("order by pubdate desc");
			string expected = "select * from story where 1=1 and title like N'%mike%' and pubdate between '25 May 2009 5:21:11.000pm' and '25 May 2010 5:21pm' order by pubdate desc";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("25 May 2009 5:21:11.000pm", "25-May-2009 5:21:11.000pm");
				expected = expected.Replace("25 May 2010 5:21pm", "25-May-2010 5:21pm");
			}
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddBool() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where isactive=", true);
			string expected = "select * from story where isactive= 1";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddInt() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where num=", 0);
			string expected = "select * from story where num= 0";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddDate() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where datepublished=", Convert.ToDateTime("1 apr 2009"));
			string expected = "select * from story where datepublished= '1 Apr 2009'";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("1 Apr 2009", "1-Apr-2009");
			}
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddDouble() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where num=", 4.5);
			string expected = "select * from story where num= 4.5";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddDecimal() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where num=", (decimal)4.5);
			string expected = "select * from story where num= 4.5";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddFloat() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where num=", (float)4.5);
			string expected = "select * from story where num= 4.5";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddByte() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story");
			sql.Add("where num=", (byte)4);
			string expected = "select * from story where num= 4";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlizeConversions() {
			var sql = new Sql("select * from story where ispublished=", true, "and num=", 55, "and name=", "mike".SqlizeText());
			string expected = "select * from story where ispublished= 1 and num= 55 and name= N'mike'";
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlAddMultipleValueTypes() {
			SqlStringBuilder sql = new SqlStringBuilder("select * from story where ispublished=", true, "and pubdate>", Convert.ToDateTime("1 apr 2009"));
			sql.Add("and num=", (decimal)4.5, "and name=", "mike".SqlizeText(), "order by", "pubdate".SqlizeName());
			string expected = "select * from story where ispublished= 1 and pubdate> '1 Apr 2009' and num= 4.5 and name= N'mike' order by [pubdate]";
			if (Fmt.DefaultDateFormatHasDashes) {
				expected = expected.Replace("1 Apr 2009", "1-Apr-2009");
			}
			
			Assert.AreEqual(expected, sql.Value);
		}

		[TestMethod]
		public void TestSqlQuotesSafety() {
			try {
				Sql sql = new Sql("select * from 'gentest'");
				Assert.Fail("Should have thrown an exception");
			} catch (Exception) {
				Assert.Pass();
			}
		}

		[TestMethod]
		public void TestSqlQuotesSafetyOff() {
			try {
				Sql sql = new Sql();
				sql.SuppressQuoteChecking = true;
				sql.Add("select * from 'gentest'");
				Assert.AreEqual("select * from 'gentest'", sql.ToString());
			} catch (Exception) {
				Assert.Fail("Should not have thrown an exception");
			}
		}

		[SlowTestMethod]
		public void SqlLeakTest() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			for (int i = 0; i < 200; i++) {
				Sql sql = new Sql("select title from gentest order by newid()");
				var name = sql.FetchString();
				if (i % 100 == 0) {
					Web.Write(i + " ");
					Web.Flush();
				}
			}
			Assert.Pass();
		}

		[TestMethod]
		public void PrimaryKeyActiveRecordGenTest() {
			//xxxpktypexxx
			IEnumerable<int> ids = new List<int>(){1,2,3};
			var sql = new Sql("id in (", SqlStringBuilder.Sqlize(ids,typeof(IEnumerable<int>)), ")");
			Assert.AreEqual("id in ( 1,2,3 )", sql.Value);
			IEnumerable<string> sids = new List<string>(){"1","2","3"};
			sql = new Sql("id in (", SqlStringBuilder.Sqlize(sids,typeof(IEnumerable<int>)), ")");
			Assert.AreEqual("id in ( N'1',N'2',N'3' )", sql.Value);
			IEnumerable<string> sids2 = new List<string>(){"o'neil","smith","cafe"};
			sql = new Sql("id in (", SqlStringBuilder.Sqlize(sids2,typeof(IEnumerable<int>)), ")");
			Assert.AreEqual("id in ( N'o''neil',N'smith',N'cafe' )", sql.Value);
		}

		[SlowTestMethod]
		public void ActiveRecordLeakTest() {
			if (!BewebData.TableExists("GenTest")) {
				Web.Write("Skipping SQL tests because GenTest table not available");
				Assert.Pass();
				return;
			}
			for (int i = 0; i < 200; i++) {
				Sql sql = new Sql("select * from gentest order by newid()");
				//var people = sql.Load<ActiveRecordList<TestPersonRecord>>();
				var person = new TestPersonRecord();
				person.LoadData(sql);
				int personID = person.ID;
				if (i % 100 == 0) {
					Web.Write(i + " (ID" + personID + ") ");
					Web.Flush();
				}
			}
			Assert.Pass();
		}



		public class TestPersonRecord : Beweb.ActiveRecord<int> {
			public TestPersonRecord()
				: base("GenTest", "GenTestID") {
			}

			//public override string GetTableName() {
			//  return "Person";
			//}
			//public override string GetPrimaryKeyName() {
			//  return "PersonID";
			//}
		}

	}

}
#endif