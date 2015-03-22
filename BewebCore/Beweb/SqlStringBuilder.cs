//#define CopiedBaseTypeExtensions
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Threading;

namespace Beweb {
	public class SqlStringBuilder {
		protected string _value;
		protected DbParameterCollection _parameters;
		protected bool suppressQuoteChecking = false;

		/// <summary>
		/// Returns the final SQL command string. Can also be used to set the SQL string in its entirety.
		/// </summary>
		public string Value {
			get { return (_value != null) ? _value.Trim() : ""; }
			set { _value = value; }
		}

		/// <summary>
		/// Set this to true to allow raw SQL including quotes to be passed in. Defaults to false, which ensures that no quotes/apostrophes can be included in sqlFragment parameters.
		/// </summary>
		public bool SuppressQuoteChecking {
			get { return suppressQuoteChecking; }
			set { suppressQuoteChecking = value; }
		}

		//public DbParameterCollection Parameters
		//{
		//    get { return _parameters; }
		//    set { _parameters = value; }
		//}

		// constructors

		/// <summary>
		/// Creates a new empty SQL command string.
		/// </summary>
		public SqlStringBuilder() {
		}

		/// <summary>
		/// Appends a piece of SQL string to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("order by dateadded desc")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		public SqlStringBuilder(string sqlFragment) {
			Add(sqlFragment);
		}

		/// <summary>
		/// Appends a piece of SQL string plus a SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where name=", "mike".SqlizeText())</example>
		/// <example>sql.Add("and id=", 55)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue) {
			Add(sqlFragment, sqlizedValue);
		}

		/// <summary>
		/// Appends a SQL fragment plus a SqlizedValue plus another SQL fragment to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ")")</example>
		/// <example>sql.Add("and id=", 55, "or id is null")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2) {
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
		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2);
		}

		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3);
		}

		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3);
		}

		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4);
		}

		public SqlStringBuilder(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4) {
			Add(sqlFragment, sqlizedValue, sqlFragment2, sqlizedValue2, sqlFragment3, sqlizedValue3, sqlFragment4, sqlizedValue4);
		}


		// add methods

		/// <summary>
		/// Appends a piece of SQL string to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("order by dateadded desc")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		public virtual SqlStringBuilder Add(string sqlFragment) {
			CheckParams(sqlFragment);
			this._value += " " + sqlFragment.Trim();
			this._value.Trim();
			return this;
		}

		/// <summary>
		/// Appends a SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add(123)</example>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public virtual SqlStringBuilder Add(SqlizedValue sqlizedValue) {
			Add("", sqlizedValue);
			return this;
		}

		/// <summary>
		/// Appends a piece of SQL string plus a SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where name=", "mike".SqlizeText())</example>
		/// <example>sql.Add("and id=", 55)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue) {
			CheckParams(sqlFragment);
			try {
				this._value += " " + sqlFragment.Trim() + " " + sqlizedValue.value.Trim();
			} catch (Exception ex) {
				throw new Exception("Failed to add fragment: sqlFragment[" + sqlFragment + "]: [" + sqlizedValue.ToString() + "], ex:" + ex.Message);
			}
			this._value.Trim();
			return this;
		}

		/// <summary>
		/// Appends a SQL fragment plus a SqlizedValue plus another SQL fragment to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ")")</example>
		/// <example>sql.Add("and id=", 55, "or id is null")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2);
			return this;
		}

		/// <summary>
		/// Appends a SQL fragment, a SqlizedValue, another SQL fragment, plus another SqlizedValue to the current SQL command string.
		/// </summary>
		/// <example>sql.Add("where id in (", "1,2,3,4".SqlizeNumberList(), ") and dateadded>", DateTime.Now)</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		/// <param name="sqlFragment2">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue2">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3, sqlizedValue3);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3, sqlizedValue3);
			Add(sqlFragment4);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3, sqlizedValue3);
			Add(sqlFragment4, sqlizedValue4);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3, sqlizedValue3);
			Add(sqlFragment4, sqlizedValue4);
			Add(sqlFragment5);
			return this;
		}

		public virtual SqlStringBuilder Add(string sqlFragment, SqlizedValue sqlizedValue, string sqlFragment2, SqlizedValue sqlizedValue2, string sqlFragment3, SqlizedValue sqlizedValue3, string sqlFragment4, SqlizedValue sqlizedValue4, string sqlFragment5, SqlizedValue sqlizedValue5, string sqlFragment6) {
			Add(sqlFragment, sqlizedValue);
			Add(sqlFragment2, sqlizedValue2);
			Add(sqlFragment3, sqlizedValue3);
			Add(sqlFragment4, sqlizedValue4);
			Add(sqlFragment5, sqlizedValue5);
			Add(sqlFragment6);
			return this;
		}


		/// <summary>
		/// Inserts a piece of SQL string at the beginning of the current SQL command string.
		/// </summary>
		/// <example>sql.Prepend("select * from")</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		public virtual SqlStringBuilder Prepend(string sqlFragment) {
			CheckParams(sqlFragment);
			this._value = sqlFragment.Trim() + " " + this._value;
			this._value.Trim();
			return this;
		}

		/// <summary>
		/// Inserts a piece of SQL string plus a SqlizedValue at the beginning of the current SQL command string.
		/// </summary>
		/// <example>sql.Prepend("select * from gentest where name=", "mike".SqlizeText())</example>
		/// <param name="sqlFragment">A part of a SQL command string, which cannot contain any single quotes</param>
		/// <param name="sqlizedValue">Any string which has had a .SqlizeXXX() method called on it, or or a built in number, date or bool type.</param>
		public virtual SqlStringBuilder Prepend(string sqlFragment, SqlizedValue sqlizedValue) {
			CheckParams(sqlFragment);
			this._value = sqlFragment.Trim() + " " + sqlizedValue.value.Trim() + " " + this._value;
			this._value.Trim();
			return this;
		}

		// param check 
		protected void CheckParams(string str) {
			if (!suppressQuoteChecking && str!=null && str.Contains("'")) {
				throw new Exception("Beweb.SqlStringBuilder: SQL String contains an invalid quote [" + str + "]");
			}
		}

		/// <summary>
		/// Returns a COUNT(*) version of the current SQL command string as a new SQL command string.
		/// replaces "select .... from" with "select count(*) from" and chops off "ORDER BY ..."
		/// be aware that this is fairly simplistic and may mangle statements with subqueries in the select clause
		/// </summary>
		/// <example>new Sql("select * from gentest").GetCountSql() returns "select count(*) from gentest"</example>
		/// <returns>A SQL command which is the COUNT version of this current SQL command</returns>
		public Sql GetCountSql() {
			string str = this.Value + "";
			int pos = 0;

			// chop off "ORDER BY ..." from end
			string lower = str.ToLower();
			pos = lower.LastIndexOf(" order by ");
			if (pos > -1) {
				str = str.Substring(0, pos);
			}

			bool useCountSection;
			bool containsJoin = lower.Contains("inner join") || lower.Contains("left join") || lower.Contains("right join") || lower.Contains("outer join");
			if (lower.Contains("group by") && containsJoin) {
				// cannot use countsection as this will be a sql error (The multi-part identifier "destination.Title" could not be bound)
				useCountSection = false;
			} else if (lower.Contains("group by")) {
				// must use countsection - otherwise count will be incorrect
				useCountSection = true;
			} else if (lower.IndexOf("from") != lower.LastIndexOf("from")) {
				// contains more than one FROM - too hard to chop up correctly
				useCountSection = true;
			} else if (lower.StartsWith("select * from") && containsJoin) {
				// cannot use countsection as this will be a sql error (The column 'SubmitterID' was specified multiple times for 'countSection')
				useCountSection = false;
			} else {
				// use fastest method - which is it?
				useCountSection = true;
			}

			if (useCountSection) {
				// this is a more robust method than playing with sql string if the query is complex
				str = "select COUNT (*) from (" + str + ") as countSection ";
			} else {
				// this method works better sometimes
				// todo: check which method is faster
				// replace "select .... from" with "select count(*) from"
				pos = lower.IndexOf(" from ");
				if (pos > -1) {
					str = "select count(*) as numitemsforGetCountSQL " + str.Substring(pos);
				}
			}

			var returnValue = new Sql();
			returnValue.SuppressQuoteChecking = true;
			returnValue.Value = str;
			returnValue.suppressQuoteChecking = false;
			return returnValue;
		}

		/// <summary>
		/// split keywords on quoted phrases or individual words or quoted phrases
		///	 create a sql string for where each keyword matches one or more fields in the fieldsCSV list
		///		 if options string contains "stem", any words ending in "s", "es", "ed" or "ing" will have that suffix removed
		///	 note this sql searches partial words - it does not attempt to match whole words only
		///	 Starts with AND.
		/// </summary>
		/// <param name="keywords"></param>
		/// <param name="fieldsCSV"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static string GetKeywordSearchSqlWhere(string keywords, string fieldsCSV, string options) {
			bool useStemming = options.Contains("stem");
			return GetKeywordSearchSqlWhere(keywords, fieldsCSV, useStemming);
		}

		/// <summary>
		/// split keywords on quoted phrases or individual words or quoted phrases
		///	 create a sql string for where each keyword matches one or more fields in the fieldsCSV list
		///	 note this sql searches partial words - it does not attempt to match whole words only
		///	 Starts with AND.
		/// </summary>
		/// <param name="keywords"></param>
		/// <param name="fieldsCSV">comma separated list of fieldnames</param>
		/// <param name="allowPartialWordMatches">if true, any words ending in "s", "es", "ed" or "ing" will have that suffix removed</param>
		/// <returns></returns>
		public static string GetKeywordSearchSqlWhere(string keywords, string fieldsCSV, bool allowPartialWordMatches) {
			return GetKeywordSearchSqlWhere(keywords, fieldsCSV.Split(new Char[] { ',' }), allowPartialWordMatches);
		}

		///  <summary>
		///  split keywords on quoted phrases or individual words or quoted phrases
		/// 	 create a sql string for where each keyword matches one or more fields in the fieldsCSV list
		/// 	 note this sql searches partial words - it does not attempt to match whole words only
		/// 	 Starts with AND.
		///  </summary>
		///  <param name="keywords"></param>
		/// <param name="fieldsList">List of field names</param>
		/// <param name="allowPartialWordMatches">if true, any words ending in "s", "es", "ed" or "ing" will have that suffix removed</param>
		///  <returns></returns>
		public static string GetKeywordSearchSqlWhere(string keywords, IEnumerable<string> fieldsList, bool allowPartialWordMatches) {
			string remainingKeywords, keyword, sql;
			sql = "";
			//' do a keyword/phrase split
			remainingKeywords = VB.trim(keywords);
			for (; remainingKeywords != ""; ) {
				remainingKeywords = VB.trim(remainingKeywords) + " ";
				if (VB.left(remainingKeywords, 1) == "\"") {	//' strip off quote
					remainingKeywords = VB.mid(remainingKeywords, 2);
					//' look for end quote
					int endQuotePos;
					endQuotePos = VB.instr(remainingKeywords, "\"");
					if (endQuotePos > 0) {
						//' take this token
						keyword = VB.left(remainingKeywords, endQuotePos - 1);
						//' remove these from the keywords To Process
						remainingKeywords = VB.mid(remainingKeywords, endQuotePos + 1);
					} else {
						//' no end quote so just assume end
						keyword = remainingKeywords;
						remainingKeywords = "";
					}
				} else {
					//' does not start with a quote, so take first word
					int endWordPos;
					endWordPos = VB.instr(remainingKeywords, " ");
					//' take this token
					keyword = VB.left(remainingKeywords, endWordPos - 1);
					//' remove these from the keywords To Process
					remainingKeywords = VB.mid(remainingKeywords, endWordPos + 1);
				}//end if

				keyword = VB.trim(keyword);
				if (keyword != "") {
					if (allowPartialWordMatches && keyword.Length > 3) {
						//' remove common suffixes, which is fine since matching is always partial words anyway
						if (VB.right(keyword, 1) == "s" && keyword.Length > 5) {
							keyword = VB.left(keyword, VB.len(keyword) - 1);
						} else if (VB.right(keyword, 2) == "ed" || VB.right(keyword, 2) == "es") {
							keyword = VB.left(keyword, VB.len(keyword) - 2);
						} else if (VB.right(keyword, 3) == "ing") {
							keyword = VB.left(keyword, VB.len(keyword) - 3);
						}
					}//end if
					sql += " and (0=1";
					foreach (string fieldNameStr in fieldsList) {
						string fieldName = fieldNameStr.Trim();
						if (allowPartialWordMatches) {
							//sql += " or (" + fieldName + " like '%[^a-zA-Z]" + Fmt.SqlString(keyword) + "%')"; //charles: search problem in admin eg cant search London
							sql += " or (" + fieldName + " like '%" + Fmt.SqlString(keyword) + "%')";
						} else {
							sql += " or ('.'+convert(nvarchar(max)," + fieldName + ")+'.' like '%[^a-z]" + Fmt.SqlString(keyword) + "[^a-z]%')";
							//sql += " or (" + fieldName + " like '%" + Fmt.SqlString(keyword) + "%')";
						}
					}//next
					sql += ")";
				}//end if
			}//loop
			//Logging.dout("sql fragment["+sql+"]");
			return sql;
		}

		public void AddKeywordSearch(string keywords, IEnumerable<string> fieldsList, bool allowPartialWordMatches) {
			AddRawSqlString(GetKeywordSearchSqlWhere(keywords, fieldsList, allowPartialWordMatches));
		}

		public void AndKeywordSearch(string keywords, IEnumerable<string> fieldsList, bool allowPartialWordMatches) {
			AddRawSqlString(GetKeywordSearchSqlWhere(keywords, fieldsList, allowPartialWordMatches));
		}

		public void WhereKeywordSearch(string keywords, IEnumerable<string> fieldsList, bool allowPartialWordMatches) {
			string keywordSearchSqlWhere = GetKeywordSearchSqlWhere(keywords, fieldsList, allowPartialWordMatches);
			keywordSearchSqlWhere = keywordSearchSqlWhere.ReplaceFirst("and", "where");
			AddRawSqlString(keywordSearchSqlWhere);
		}

		public void AddKeywordSearch(string keywords, string fieldsCSV, bool allowPartialWordMatches) {
			AddRawSqlString(GetKeywordSearchSqlWhere(keywords, fieldsCSV, allowPartialWordMatches));
		}

		public void AndKeywordSearch(string keywords, string fieldsCSV, bool allowPartialWordMatches) {
			AddRawSqlString(GetKeywordSearchSqlWhere(keywords, fieldsCSV, allowPartialWordMatches));
		}

		public void WhereKeywordSearch(string keywords, string fieldsCSV, bool allowPartialWordMatches) {
			string keywordSearchSqlWhere = GetKeywordSearchSqlWhere(keywords, fieldsCSV, allowPartialWordMatches);
			keywordSearchSqlWhere = keywordSearchSqlWhere.ReplaceFirst("and", "where");
			AddRawSqlString(keywordSearchSqlWhere);
		}

		// 20110816 MN note: GetFullTextKeywordSearchSqlWhere has been moved
		// other methods	
		/// <summary>
		/// Returns final SQL command string.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Value;
		}

		/// <summary>
		/// Appends a sql fragment.
		/// </summary>
		/// <param name="likeThis"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
		public static SqlStringBuilder operator +(SqlStringBuilder likeThis, string sql) {
			likeThis.Add(sql);
			return likeThis;
		}
		/// <summary>
		/// Appends a SqlizedValue (or a built in number, date or bool type)
		/// </summary>
		/// <param name="likeThis"></param>
		/// <param name="sqlizedValue"></param>
		/// <returns></returns>
		public static SqlStringBuilder operator +(SqlStringBuilder likeThis, SqlizedValue sqlizedValue) {
			likeThis.Add("", sqlizedValue);
			return likeThis;
		}
		/// <summary>
		/// Appends another SQL string onto this SQL string.
		/// </summary>
		/// <param name="likeThis"></param>
		/// <param name="otherInstance"></param>
		/// <returns></returns>
		public static SqlStringBuilder operator +(SqlStringBuilder likeThis, SqlStringBuilder otherInstance) {
			likeThis._value += " " + otherInstance._value;
			return likeThis;
		}

		/// <summary>
		/// Return a SqlizedValue for use in constructing a SQL Statement with Sql or SqlStringBuilder class.
		/// Pass in any value type (eg bool, int, decimal, double)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue Sqlize<T>(string value) {
			Type targetType = typeof(T);
			TypeConverter tc = TypeDescriptor.GetConverter(targetType);
			T valueCorrectType = (T)tc.ConvertTo(value, targetType);
			return SqlStringBuilder.Sqlize(valueCorrectType);
		}

		public static SqlizedValue Sqlize(object value) {
			if (value == null || value == DBNull.Value) {
				return new SqlizedValue("null");
			}
			return Sqlize(value, value.GetType());

			//if (value == null || value == DBNull.Value) {
			//  return new SqlizedValue("null");
			//} else if (value is bool) {
			//  return ((bool)value).SqlizeBool();
			//} else if (value is string) {
			//  return ((string)value).SqlizeText();
			//} else if (value is DateTime) {
			//  //if(((DateTime)value)==new DateTime(1,1,1))return  new SqlizedValue("null");
			//  return ((DateTime)value).SqlizeDateTime();
			//} else if (value is Guid) {
			//  return ((Guid)value).SqlizeGuid();
			//} else if (value is ValueType) {
			//  // all other value types are generally numbers which don't need any sql escaping
			//  return new SqlizedValue(value + "");
			//} else if (value is bool[]) {
			//  return ((bool[])value).SqlizeBoolList();
			//} else if (value is string[]) {
			//  return ((string[])value).SqlizeTextList();
			//} else if (value is DateTime[]) {
			//  return ((DateTime[])value).SqlizeDateList();
			//} else if (value is Guid[]) {
			//  return ((Guid[])value).SqlizeGuidList();
			//} else if (value is int[]) {
			//  return ((int[])value).SqlizeNumberList();
			//} else if (value is double[]) {
			//  return ((double[])value).SqlizeNumberList();
			//} else if (value is decimal[]) {
			//  return ((decimal[])value).SqlizeNumberList();
			//} else if (value is float[]) {
			//  return ((float[])value).SqlizeNumberList();
			//} else if (value is short[]) {
			//  return ((short[])value).SqlizeNumberList();
			//} else if (value is long[]) {
			//  return ((long[])value).SqlizeNumberList();
			//} else {
			//  throw new Exception("Attempt to call Sqlize() on an invalid type type:["+value.GetType().Name+"] value:["+value.ToString()+"]");
			//}
		}

		public static SqlizedValue Sqlize(object value, Type type) {
			if (value is string && type != typeof(string)) {
				// convert it from a string to the correct type
				value = Conv.FromString((string)value, type);
			}

			if (value == null || value == DBNull.Value) {
				return new SqlizedValue("null");
			} else if (type == typeof(bool)) {
				return ((bool)value).SqlizeBool();
			} else if (type == typeof(string)) {
				return ((string)value).SqlizeText();
			} else if (type == typeof(DateTime)) {
				//if(((DateTime)value)==new DateTime(1,1,1))return  new SqlizedValue("null");
				return ((DateTime)value).SqlizeDateTime();
			} else if (type == typeof(DateTime?)) {
				return ((DateTime)value).SqlizeDateTime();
			} else if (type == typeof(Guid)) {
				return ((Guid)value).SqlizeGuid();
			} else if (type == typeof(Guid?)) {
				return ((Guid)value).SqlizeGuid();
			} else if (type.IsValueType) {
				// all other value types are generally numbers which don't need any sql escaping
				// need to check if object is string 
				// important - this was a security hole
				if (value is string) {
					// will never get here now due to convert at the top, but if it does it is an error
					throw new BewebDataException("Sqlize trying taking a string [" + value + "] but should be a " + type.Name);
				}
				return new SqlizedValue(value + "");
			} else if (type == typeof(bool[])) {
				return ((bool[])value).SqlizeBoolList();
			} else if (type == typeof(string[])) {
				return ((string[])value).SqlizeTextList();
			} else if (type == typeof(DateTime[])) {
				return ((DateTime[])value).SqlizeDateList();
			} else if (type == typeof(Guid[])) {
				return ((Guid[])value).SqlizeGuidList();
			}  else if (type == typeof(Guid?[])) {
				return ((Guid[])value).SqlizeGuidList();
			} else if (type == typeof(int[])) {
				return ((int[])value).SqlizeNumberList();
			} else if (type == typeof(double[])) {
				return ((double[])value).SqlizeNumberList();
			} else if (type == typeof(decimal[])) {
				return ((decimal[])value).SqlizeNumberList();
			} else if (type == typeof(float[])) {
				return ((float[])value).SqlizeNumberList();
			} else if (type == typeof(short[])) {
				return ((short[])value).SqlizeNumberList();
			} else if (type == typeof(long[])) {
				return ((long[])value).SqlizeNumberList();

			} else if (value is IEnumerable<bool>) {
				return (((IEnumerable<bool>)value).SqlizeBoolList());
			} else if (value is IEnumerable<bool?>) {
				return (((IEnumerable<bool?>)value).SqlizeBoolList());
			} else if (value is IEnumerable<string>) {
				return (((IEnumerable<string>)value).SqlizeTextList());
			} else if (value is IEnumerable<DateTime>) {
				return (((IEnumerable<DateTime>)value).SqlizeDateList());
			} else if (value is IEnumerable<DateTime?>) {
				return (((IEnumerable<DateTime?>)value).SqlizeDateList());
			} else if (value is IEnumerable<Guid>) {
				return (((IEnumerable<Guid>)value).SqlizeGuidList());
			}  else if (value is IEnumerable<Guid?>) {
				return (((IEnumerable<Guid?>)value).SqlizeGuidList());
			} else if (value is IEnumerable<int>) {
				return (((IEnumerable<int>)value).SqlizeNumberList());
			} else if (value is IEnumerable<int?>) {
				return (((IEnumerable<int?>)value).SqlizeNumberList());
			} else if (value is IEnumerable<double>) {
				return (((IEnumerable<double>)value).SqlizeNumberList());
			} else if (value is IEnumerable<decimal>) {
				return (((IEnumerable<decimal>)value).SqlizeNumberList());
			} else if (value is IEnumerable<float>) {
				return (((IEnumerable<float>)value).SqlizeNumberList());
			} else if (value is IEnumerable<short>) {
				return (((IEnumerable<short>)value).SqlizeNumberList());
			} else if (value is IEnumerable<long>) {
				return (((IEnumerable<long>)value).SqlizeNumberList());

			} else {
				throw new Exception("Attempt to call Sqlize() on an invalid type type:[" + value.GetType().Name + "] value:[" + value.ToString() + "]");
			}
		}

		public void AddRawSqlString(string sqlFragmentString) {
			this._value += " " + sqlFragmentString;
			this._value.Trim();
		}
	}

	public struct SqlizedValue {
		public string value;

		internal SqlizedValue(string value) {
			this.value = value;
		}

		public override string ToString() {
			throw new Exception("Beweb.SqlizedValue: Cannot call ToString() on SqlizedValue. This is to prevent you accidentally appending it to a string. You must append SQL strings using the Sql.Add() method (separated with commas not pluses).\nThe value you are trying to convert to string is: " + value + "");
			//return "";
		}

		// implicit converters
		static public explicit operator SqlizedValue(Sql otherSql) {
			return new SqlizedValue(otherSql.Value+"");
		}
		static public implicit operator SqlizedValue(int value) {
			return new SqlizedValue(value + "");
		}
		static public implicit operator SqlizedValue(double value) {
			return value.SqlizeNumber();
		}
		static public implicit operator SqlizedValue(decimal value) {
			return value.SqlizeNumber();
		}
		static public implicit operator SqlizedValue(bool value) {
			return value.SqlizeBool();
		}
		static public implicit operator SqlizedValue(DateTime value) {
			return value.SqlizeDateTime(); //must be datetime
		}
		static public implicit operator SqlizedValue(Guid value) {
			return value.SqlizeGuid();
		}

		// arrays
		static public implicit operator SqlizedValue(int?[] value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(int[] value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(double[] value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(decimal[] value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(bool[] value) {
			return value.SqlizeBoolList();
		}
		static public implicit operator SqlizedValue(DateTime[] value) {
			return value.SqlizeDateList();
		}
		static public implicit operator SqlizedValue(Guid[] value) {
			return value.SqlizeGuidList();
		}
		static public implicit operator SqlizedValue(Guid?[] value) {
			return value.SqlizeGuidList();
		}

		// lists
		static public implicit operator SqlizedValue(List<int> value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(List<double> value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(List<decimal> value) {
			return value.SqlizeNumberList();
		}
		static public implicit operator SqlizedValue(List<bool> value) {
			return value.SqlizeBoolList();
		}
		static public implicit operator SqlizedValue(List<DateTime> value) {
			return value.SqlizeDateList();
		}
		static public implicit operator SqlizedValue(List<Guid> value) {
			return value.SqlizeGuidList();
		}
		static public implicit operator SqlizedValue(List<Guid?> value) {
			return value.SqlizeGuidList();
		}

		public bool IsBlank() {
			return value.IsBlank();
		}
		public bool IsNotBlank() {
			return !IsBlank();
		}
	}

	public static class SqlizerExtensionMethods {
		//---------------------------------------------------------
		// EXTENSION METHODS TO ADD SQLIZE FEATURES
		//---------------------------------------------------------


		// number extensions

		public static SqlizedValue SqlizeNumber(this int number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		public static SqlizedValue SqlizeNumber(this double number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		public static SqlizedValue SqlizeNumber(this float number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		public static SqlizedValue SqlizeNumber(this short number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		public static SqlizedValue SqlizeNumber(this long number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		public static SqlizedValue SqlizeNumber(this decimal number) {
			return new SqlizedValue(Fmt.SqlNumber(number + ""));
		}

		// bool extensions

		public static SqlizedValue SqlizeBool(this bool value) {
			return new SqlizedValue(Fmt.SqlBoolean(value));
		}

		// date extensions

		/// <summary>
		/// Convert a datetime to a SQL date string (WITH TIME)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeDateTime(this System.DateTime value) {
			return new SqlizedValue(Fmt.SqlDateTime(value));
		}

		/// <summary>
		/// Convert a datetime to a SQL date string (WITHOUT TIME). unless Fmt.SqlDateIncludesTime = true
		/// Breaking change (2012?): previously this DID include the time.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeDate(this System.DateTime value) {
			return new SqlizedValue(Fmt.SqlDate(value));
		}

		// guid extensions

		public static SqlizedValue SqlizeGuid(this Guid value) {
			return new SqlizedValue(Fmt.SqlGuid(value));
		}


		// string extensions

		public static SqlizedValue SqlizeDate(this string value) {
			return new SqlizedValue(Fmt.SqlDateTime(value));
		}

		public static SqlizedValue SqlizeBool(this string value) {
			return new SqlizedValue(Fmt.SqlBoolean(value));
		}

		public static SqlizedValue SqlizeGuid(this string value) {
			return new SqlizedValue(Fmt.SqlGuid(value));
		}

		/// <summary>
		/// Calls fmtsqltext, surrounding with quotes  
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeText(this string value) {
			return new SqlizedValue(Fmt.SqlText(value));
		}

		/// <summary>
		/// Calls fmtsqltext, surrounding with quotes 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue Sqlize_Text(this string value) {
			return new SqlizedValue(Fmt.SqlText(value));
		}

		public static SqlizedValue SqlizeNumber(this string value) {
			return new SqlizedValue(Fmt.SqlNumber(value));
		}

		/// <summary>
		/// add no % on the left or right of the string - just ignore case
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeLikeIgnoreCase(this string value) {
			return value.SqlizeLike("", "");
		}

		/// <summary>
		/// Contains but whole words only - add % on left and right. 
		/// It is easier to use sql.AndKeywordSearch(keyword, fieldname, false)
		/// IMPORTANT: To make this work propery you need to surround the fieldname with dots or some other separator.
		/// Example: sql.AddRawSqlString("where '.'+BodyText+'.' like ").Add(Request["search"].SqlizeLikeWholeWords())
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[Obsolete("Too easy to misuse. Use sql.AndKeywordSearch(keyword, fieldname, false) instead.")]
		public static SqlizedValue SqlizeLikeWholeWords(this string value) {
			return value.SqlizeLike("%[^a-zA-Z]", "[^a-zA-Z]%");
		}

		/// <summary>
		/// Contains - add % on left and right
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeLike(this string value) {
			return value.SqlizeLike("%", "%");
		}

		/// <summary>
		/// allow you to specifiy what is on the left and right of the like - generally %
		/// </summary>
		/// <param name="value"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static SqlizedValue SqlizeLike(this string value, string left, string right) {
			string sqlFrag = "N'" + left + Fmt.SqlString(value) + right + "'";
			if (false && (value.Contains("[") || value.Contains("%"))) {													 // 20120928 jn removed this
				// need to escape it
				string escapeChar = "~";
				if (value.Contains(escapeChar)) escapeChar = "@";
				if (value.Contains(escapeChar)) escapeChar = "!";
				if (value.Contains(escapeChar)) escapeChar = "`";
				if (value.Contains(escapeChar)) escapeChar = "$";
				sqlFrag = sqlFrag.Replace("[", escapeChar);
				sqlFrag = sqlFrag.Replace("%", escapeChar);
				sqlFrag += " ESCAPE '" + escapeChar + "'";
			}
			return new SqlizedValue(sqlFrag);
		}

		public static SqlizedValue Sqlize<T>(this string value) {
			return SqlStringBuilder.Sqlize<T>(value);
		}

		// string extensions for comma separated strings
		public static SqlizedValue SqlizeList<T>(this string value) {
			return value.Split(',').SqlizeList<T>();
		}

		public static SqlizedValue SqlizeDateList(this string value) {
			return value.Split(',').SqlizeDateList();
		}

		public static SqlizedValue SqlizeBoolList(this string value) {
			return value.Split(',').SqlizeBoolList();
		}

		public static SqlizedValue SqlizeTextList(this string value) {
			return value.Split(',').SqlizeTextList();
		}

		public static SqlizedValue SqlizeNumberList(this string value) {
			return value.Split(',').SqlizeNumberList();
		}

		public static SqlizedValue SqlizeGuidList(this string value) {
			return value.Split(',').SqlizeGuidList();
		}


		// array extensions
		public static SqlizedValue SqlizeList<T>(this string[] array) {
			string result = "";
			foreach (string str in array) {
				if (!("," + result + ",").Contains("," + str + ",")) {
					if (result != "") {
						result += ",";
					}
					result += SqlStringBuilder.Sqlize<T>(str.Trim()).value;
				}
			}
			return new SqlizedValue(result);
		}


		/// <summary>
		/// Convert an array of string dates to a comma delimited list of SQL date strings (WITH TIME if specified) - for use in a SQL IN clause.
		/// </summary>
		/// <returns></returns>
		public static SqlizedValue SqlizeDateList(this string[] array) {
			string result = "";
			foreach (string str in array) {
				if (result != "") {
					result += ", ";
				}
				result += Fmt.SqlDateTime(str.Trim());
			}
			return new SqlizedValue(result);
		}

		/// <summary>
		/// Convert any kind of list of string dates to a comma delimited list of SQL date strings (WITH TIME if specified) - for use in a SQL IN clause.
		/// </summary>
		/// <returns></returns>
		public static SqlizedValue SqlizeDateList(this IEnumerable<DateTime> array) {
			string result = "";
			foreach (DateTime date in array) {
				if (result != "") {
					result += ", ";
				}
				result += Fmt.SqlDateTime(date);
			}
			return new SqlizedValue(result);
		}
		public static SqlizedValue SqlizeDateList(this IEnumerable<DateTime?> array) {
			string result = "";
			foreach (DateTime? date in array) {
				if (date.HasValue) {
					if (result != "") {
						result += ", ";
					}
					result += Fmt.SqlDateTime(date.Value);
				}
			}
			return new SqlizedValue(result);
		}

		/// <summary>
		/// Convert an array of strings to a comma delimited list of SQL strings (with quotes around them) - for use in a SQL IN clause.
		/// Checks that there are no quotes in the strings.
		/// </summary>
		/// <returns></returns>
		public static SqlizedValue SqlizeTextList(this string[] array) {
			string result = "";
			foreach (string str in array) {
				if (!("," + result + ",").Contains("," + str + ",") && str.IsNotBlank()) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlText(str.Trim());
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeTextList(this IEnumerable<string> array) {
			string result = "";
			foreach (string str in array) {
				if (!("," + result + ",").Contains("," + str + ",") && str.IsNotBlank()) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlText(str.Trim());
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeBoolList(this string[] array) {
			string result = "";
			foreach (string str in array) {
				if (!("," + result + ",").Contains("," + str + ",") && str.IsNotBlank()) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlBoolean(str.Trim());
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeBoolList(this IEnumerable<bool> array) {
			string result = "";
			foreach (bool value in array) {
				if (!("," + result + ",").Contains("," + value + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlBoolean(value);
				}
			}
			return new SqlizedValue(result);
		}
		public static SqlizedValue SqlizeBoolList(this IEnumerable<bool?> array) {
			string result = "";
			foreach (bool? value in array) {
				if (value.HasValue) {
					if (!("," + result + ",").Contains("," + value + ",")) {
						if (result != "") {
							result += ",";
						}
						result += Fmt.SqlBoolean(value.Value);
					}
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeGuidList(this string[] array) {
			string result = "";
			foreach (string str in array) {
				if (!("," + result + ",").Contains("," + str + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlGuid(str);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeGuidList(this IEnumerable<Guid> array) {
			string result = "";
			foreach (Guid value in array) {
				if (!("," + result + ",").Contains("," + value + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlGuid(value);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeGuidList(this IEnumerable<Guid?> array) {
			string result = "";
			foreach (Guid? value in array) {
				if(value != null) {
					if (!("," + result + ",").Contains("," + value + ",")) {
						if (result != "") {
							result += ",";
						}
						result += Fmt.SqlGuid(value.Value);
					}					
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this string[] array) {
			string result = "";
			foreach (string str in array) {
				string num = str + "".Trim();
				if (num != "") {
					if (!("," + result + ",").Contains("," + num + ",")) {
						result += Fmt.SqlNumber(num) + ",";
					}
				}
			}
			result = result.Trim().Trim(',');
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<int> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}
		public static SqlizedValue SqlizeNumberList(this IEnumerable<int?> array) {
			string result = "";
			foreach (var num in array) {
				if (num.HasValue && !("," + result + ",").Contains("," + num.Value + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num.Value);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<float> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<decimal> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<double> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<long> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeNumberList(this IEnumerable<short> array) {
			string result = "";
			foreach (var num in array) {
				if (!("," + result + ",").Contains("," + num + ",")) {
					if (result != "") {
						result += ",";
					}
					result += Fmt.SqlNumber(num);
				}
			}
			return new SqlizedValue(result);
		}

		public static SqlizedValue SqlizeRAW(this string value, bool IBloodyWellCheckedTheValueCANNOTContainSQLInjectionCode) {
			//Assert.IsTrue(IBloodyWellCheckedTheValueCANNOTContainSQLInjectionCode,"bloody well check it");
			return new SqlizedValue(value);
		}

		private static bool ContainsOnly(string str, string allowedChars) {
			if (str == null) {
				throw new Exception("string.ContainsOnly() called with a null string");
			}
			foreach (var c in str) {
				if (!allowedChars.Contains("" + c)) return false;
			}
			return true;
		}

		/// <summary>
		/// Allows only characters which are allowed as part of SQL identifiers (field names, table names, etc).
		/// Square brackets and dots are allowed so you can pass in qualified identifiers such as dbo.tablename.fieldname.
		/// Square brackets are added if the value doesn't contain them already.
		/// </summary>
		/// <example>"mike's cool".SqlizeName() would throw an exception</example>
		/// <example>"MyDb.dbo.Administrator.[First Name]".SqlizeName() would be valid</example>
		/// <param name="value"></param>
		/// <returns>Returns a Sqlizer which can be passed to Sql string builder to form a safe sql string. Throws an exception if unsafe data is passed in.</returns>
		public static SqlizedValue SqlizeName(this string value) {
			if (value + "" == "") {
				throw new Exception("SqlizeName: name parameter is null or empty string.");
			}
			// we allow space too
			if (ContainsOnly(value, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz[]. -_0123456789")) {
				if (!value.Contains("[") && !value.Contains("]")) {
					if (!value.Contains(".")) {
						value = "[" + value + "]";
					} else {
						value = "[" + value.Replace(".", "].[") + "]";
					}
				}
				return new SqlizedValue(value);
			} else {
				throw new Exception("SqlizeName: contains invalid characters [" + value + "]");
			}

		}

		// MN 20130702 - this is in a weird place - now calls Fmt.SqlBoolean instead
		//public static string FmtSqlBoolean(bool value) {
		//	return value ? "1" : "0";
		//}
	}

/* 20130702 MN - this whole partial class moved back into Fmt but for ease of diffing it remains here commented out

	public partial class Fmt {
		/// <summary>
		/// Encode a boolean value properly for including in a SQL string. Prevents SQL injection hacks.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SqlBoolean(string value) {
			bool result = false;
			if (value == "") value = "false";
			if (value == "1") value = "true";
			if (bool.TryParse(value, out result)) {
				return SqlBoolean(result);
			} else {
				// failed 
				throw new Exception("Failed to convert value [" + value + "] to a boolean");
			}
		}
		/// <summary>
		/// Encode a boolean value properly for including in a SQL string. Prevents SQL injection hacks.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SqlBoolean(bool value) {
			return SqlizerExtensionMethods.FmtSqlBoolean(value);
		}
		/// <summary>
		/// Enquote a GUID properly to include in a SQL string
		/// </summary>
		/// <param name="guid">A GUID of type System.Guid</param>
		/// <returns></returns>
		public static string SqlGuid(System.Guid guid) {
			// on sql server it is the same as a string (ie quoted) - if using Access we need to extend this
			return Fmt.SqlGuid("{" + guid.ToString().ToUpperInvariant() + "}");
		}

		/// <summary>
		/// Enquote a GUID properly to include in a SQL string
		/// </summary>
		/// <param name="guid">A string that should contain a GUID</param>
		/// <returns></returns>
		public static string SqlGuid(string guid) {
			// on sql server it is the same as a string (ie quoted) - if using Access we need to extend this
			if (guid.Contains("'")) throw new BewebDataException("An apostrophe was detected in a GUID by Beweb.Fmt.SqlGuid(). This is invalid and may be SQL injection. GUIDs cannot contain an apostrophe.");
			if (!guid.Trim().IsGuid()) throw new BewebDataException("An invalid GUID was detected by Beweb.Fmt.SqlGuid(). The invalid GUID was " + guid + ".");

			return "'" + guid.Trim() + "'";
		}


		/// <summary>
		/// convert datetime to '22-Jan-2008 10:57 am' format
		/// </summary>
		/// <param name="d">source date</param>
		/// <returns>standard format</returns>
		public static string SqlDateTime(string d) {
			return Fmt.SqlDateTime(System.DateTime.Parse(d));
		}

		/// <summary>
		///	convert a date to a common format string that sql server can read
		/// </summary>
		/// <param name="d">1 apr 2010</param>
		/// <returns>01 Apr 2009 12:00:00.000 am</returns>
		public static string SqlDateTimeLong(DateTime d) {
			return "'" + d.ToString("dd MMM yyyy hh:mm:ss.fff t") + "m'";
			//return "'"+Fmt.DateTime(d)+"'";
		}

		/// <summary>
		///	convert a date to a common format string that sql server can read
		/// </summary>
		/// <param name="d">1 apr 2010 5:19pm</param>
		/// <returns>01-Apr-2010 05:19 pm</returns>
		public static string SqlDateTime(DateTime d) {
			//return "'" +d.ToString("dd-MMM-yyyy hh:mm t") + "m" + "'"; //add am/pm
			//return "'"+Fmt.DateTime(d, Fmt.DateTimePrecision.Millisecond)+"'";
			// MK: there is no reason to do above - SQL doesn't care that much
			// 01-Apr-2010 05:19 pm doesn't make sense -> 05 = 5am - the pm is irrelavent
			return "'" + d.ToString("dd-MMM-yyyy HH:mm:ss.fff") + "'";
		}

		/// <summary>
		///	convert a date to a common format string that sql server can read.
		/// Breaking change 2012: this now cuts off the time, so only a date without time.
		/// Use Fmt.SqlDateTime to include the time.
		/// </summary>
		/// <param name="d">1 apr 2010</param>
		/// <returns>01-Apr-2010</returns>
		public static string SqlDate(DateTime d) {
			//return "'" +d.ToString("dd-MMM-yyyy")  + "'"; //add am/pm
			//return "'"+Fmt.DateTime(d)+"'";
			// MK: there is no reason to do above - SQL doesn't care that much
			return "'" + d.ToString("dd-MMM-yyyy") + "'";
		}
		/// <summary>
		/// Escape a numeric value properly for including in a SQL string. Makes sure this is a number (anti hack).
		/// </summary>
		/// <param name="value">String that should be a number</param>
		/// <returns></returns>
		public static string SqlNumber(string value) {
			double result = 0;
			if (double.TryParse(value, out result)) {
				return result.ToString();
			} else {
				// failed 
				throw new Exception("Failed to convert value [" + value + "] to a number");
			}
		}
		public static string SqlNumber(int value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(double value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(decimal value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(short value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(long value) {
			return Fmt.SqlNumber(value + "");
		}
		public static string SqlNumber(float value) {
			return Fmt.SqlNumber(value + "");
		}

		[Obsolete("Deprecated - use Fmt.SqlNumber instead - this method returns an Int but it should return a String")]
		public static int SQLNumber(string source) {
			int result = 0;
			try {
				if (source + "" != "") result = Convert.ToInt32(source);
			} catch (Exception ex) {
				// failed 
				Console.WriteLine("Exception: [{0}]", ex.Message);
				throw new Exception("Failed to convert value to a number");
			}

			return result;
		}

		[Obsolete("Use Fmt.SqlString")]
		public static string SQLString(string src) {
			return SqlString(src);
		}
		/// <summary>
		/// Formats a string ready for a SQL statement - puts appropriate quotes around value. Note that N converts the string to a ntext / nvarchar string
		/// </summary>
		/// <param name="dataValue"></param>
		/// <returns>string</returns>
		public static string SqlText(string dataValue) {
			return "N'" + SqlString(dataValue) + "'";
		}

		[Obsolete("Use Fmt.SqlText")]
		public static string SQLText(string dataValue) {
			return SqlText(dataValue);
		}

		/// <summary>
		/// Replace single quotes in a given string with '' quotes so it can be put in a sql string that has reserved single quotes in it	to prevent sql injection attacks
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string SqlString(string str) {
			return (str == null) ? "" : str.Replace("'", "''");
		}

	}
*/

#if CopiedBaseTypeExtensions

	public static class BaseTypeExtensions {
		/// <summary>
		/// Replaces the first occurrence of the specified string oldValue with newValue.
		/// This is not case-sensitive.
		/// </summary>
		/// <example>"deals $99, $55, $88".ReplaceFirst("$", "from $") returns "deals from $99, $55, $88"</example>
		/// <param name="str"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public static string ReplaceFirst(this string str, string oldValue, string newValue) {
			int pos = str.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);
			string result = str;
			if (pos != -1) {
				result = str.Substring(0, pos) + newValue + str.Substring(pos + oldValue.Length);
			}
			return result;
		}
		
		public static bool IsGuid(this string value) {
			if (value != null) {
				Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
				return guidRegEx.IsMatch(value);
			}
			return false;
		}
	}
#endif
}