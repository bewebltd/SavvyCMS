using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Savvy;
using Beweb;

namespace Beweb {
	public class KeywordSearch {
		private string rawKeywords;
		private List<string> phrases;
		private static string _noiseWords;
		private static bool _disableNoiseWords = true;

		// constructor
		public KeywordSearch(string searchKeywords) {
			Web.Session["LastSearch"] = searchKeywords;										//jn session shown in last search in site.master
			rawKeywords = (searchKeywords + "").Trim();
			phrases = GetKeywordPhraseArray(searchKeywords);
		}

		public List<string> Keyphrases {
			get { return phrases; }
		}

		/// <summary>
		/// Returns a list of any words that were ignored in the query.
		/// </summary>
		public DelimitedString NoiseWordsRemoved {
			get {
				var str = new DelimitedString(", ");
				if (!_disableNoiseWords) {
					foreach (string phrase in phrases) {
						if (IsSqlServerNoiseWord(phrase)) {
							str += "\"" + phrase + "\"";
						}
					}
				}
				return str;
			}
		}

		public DelimitedString ScrubbedKeywords {
			get {
				var str = new DelimitedString(" ");
				foreach (string phrase in phrases) {
					if (_disableNoiseWords || !IsSqlServerNoiseWord(phrase)) {
						str += phrase;
					}
				}
				return str;
			}
		}

		public string OriginalKeywords {
			get {
				return rawKeywords;
			}
		}

		public bool IsBlank {
			get { return ScrubbedKeywords.IsBlank; }
		}

		/// <summary>
		/// If you have a fulltext index, this is much faster than the regular GetKeywordSearchSqlWhere().
		/// It searches whole words.
		/// Starts with AND.
		/// Removes any SQL Server noise words (as these will not be found in the index).
		/// If it is all  noise words, returns null.
		/// 
		/// You can set up a fulltext index using this SQL:
		/// sp_fulltext_database @action='enable'
		/// CREATE FULLTEXT CATALOG fulltextCatalog AS DEFAULT;
		/// CREATE FULLTEXT INDEX ON table(column1,column2,column3) KEY INDEX PK_table WITH CHANGE_TRACKING AUTO;
		/// </summary>
		/// <param name="keywords"></param>
		/// <param name="fieldsCSV"></param>
		/// <returns></returns>
		public string GetSqlFullTextWhere(string fieldsCSV) {
			if (fieldsCSV.IsBlank()) {
				return "";
			}
			string sql = null;
			var searchString = FullTextQuery;
			if (searchString.IsNotBlank()) {
				sql = " AND " +
							"CONTAINS((" + fieldsCSV + "), ";
				sql += searchString.value;
				sql += ") ";
			}
			return sql;
		}

		/// <summary>
		/// Build a string containing the keywords with AND between them, suitable for submitting to SQL Server Full Text Search CONTAINS or CONTAINSTABLE commands.
		/// Example: sql.Add("select page.* from page inner join CONTAINSTABLE(page, *, ",search.FullTextQuery,") as Search on Search.[Key]=page.pageID");
		/// </summary>
		public SqlizedValue FullTextQuery {
			get {
				var searchString = new DelimitedString(" AND ");
				searchString.throwExceptionOnStringContainsDelimiter = false;
				foreach (string phrase in phrases) {
					if (_disableNoiseWords || !IsSqlServerNoiseWord(phrase)) {
						searchString += "\"" + Fmt.SqlString(phrase) + "\"";
					}
				}
				if (searchString.IsBlank) return new SqlizedValue();
				return new SqlizedValue("'" + searchString + "'");
			}
		}

		/// <summary>
		/// Returns true if a SQL Server noise words (which will not be found in a fulltext index). 
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		public static bool IsSqlServerNoiseWord(string word) {
			if (_disableNoiseWords) return false;

			if (_noiseWords == null) {
				try {
					_noiseWords = new Sql("select stopword from sys.fulltext_system_stopwords where language_id = 1033").GetDelimited(",");
				} catch (Beweb.BewebDataException) {
					// stopword table doesn't exist
					_noiseWords = "a,the,and,i";
				}
				_noiseWords = "," + _noiseWords.ToLower() + ",";
			}
			return _noiseWords.Contains("," + word.ToLower() + ",");
		}

		public static bool IsSqlFullTextInstalled() {
			// also at the database level: select DatabaseProperty(db_name(), 'IsFulltextEnabled')
			return new Sql("select SERVERPROPERTY('IsFullTextInstalled')").FetchBool();
		}

		public static bool IsSqlFullTextEnabled() {
			// also at the database level: select DatabaseProperty(db_name(), 'IsFulltextEnabled')
			return IsSqlFullTextInstalled() && new Sql("select DatabaseProperty(db_name(), 'IsFulltextEnabled')").FetchBool();
		}

		public static List<string> GetKeywordPhraseArray(string keywords) {
			string remainingKeywords, keyword;
			List<string> result = new List<string>();
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
					result.Add(keyword);
				}//end if
			}//loop
			return result;
		}

		public static void CreateFullTextIndex<TActiveRecord>() where TActiveRecord : ActiveRecord, new() {
			var record = new TActiveRecord();
			string table = record.GetTableName();
			CreateFullTextIndex(table);
		}

		public static void CreateFullTextIndex(string tableName) {
			Web.WriteLine("-- DEV:CreateFullTextIndex");
			new Sql().AddRawSqlString("sp_fulltext_database @action='enable'").Execute();
			try {
				new Sql().AddRawSqlString("CREATE FULLTEXT CATALOG fulltextCatalog AS DEFAULT").Execute();
			} catch (Exception e) {
				Web.WriteLine(e.Message);
			}
			// turn off stopwords
			if (_disableNoiseWords) {
				try {
					new Sql().AddRawSqlString("sp_configure 'show advanced options', 1; RECONFIGURE;").Execute();
				} catch (Exception e) {
					Web.WriteLine(e.Message);
				}
				try {
					new Sql().AddRawSqlString("sp_configure 'transform noise words', 1; RECONFIGURE;").Execute();
				} catch (Exception e) {
					Web.WriteLine(e.Message);
				}
			}
			// create index for given table

			string table = tableName;
			string index = "PK_" + table;


			try {
				new Sql().AddRawSqlString("drop FULLTEXT INDEX on " + table + ";").Execute();
			} catch (Exception e) {
				Web.WriteLine(e.Message);
			}

			string fields = GetSqlFullTextFieldsForTable(table);
			string sql = "CREATE FULLTEXT INDEX ON " + table + "(" + fields + ") KEY INDEX " + index + " WITH CHANGE_TRACKING AUTO";


			if (Util.IsBewebOffice || Util.ServerIsDev) {
				Web.WriteLine(sql);
			}
			try {
				new Sql().AddRawSqlString(sql).Execute();
			} catch (Exception e) {
				Web.WriteLine(e.Message);
			}
			if (_disableNoiseWords) {
				string indexSql = "ALTER FULLTEXT INDEX ON [dbo].[" + table + "] SET STOPLIST = OFF";
				try {
					new Sql().AddRawSqlString(indexSql).Execute();
				} catch (Exception e) {
					Web.WriteLine(e.Message);
				}
			}
		}

		public Sql FullTextJoin(string tableName) {
			return FullTextJoin(tableName, tableName + "ID");
		}

		public Sql FullTextJoin(string tableName, string pkName) {
			return new Sql("inner join CONTAINSTABLE(", tableName.SqlizeName(), ", *, ", FullTextQuery, ") as Search on Search.[Key]=", tableName.SqlizeName(), ".", pkName.SqlizeName());
		}

		public Sql FullTextWhereContains(string tableName) {
			return new Sql("WHERE CONTAINS(", tableName.SqlizeName(), ", *, ", FullTextQuery, ")");
		}

		public Sql FullTextAndContains(string tableName) {
			return new Sql("AND CONTAINS(", tableName.SqlizeName(), ".*, ", FullTextQuery, ")");
		}

		public Sql FullTextAndContains() {
			return new Sql("AND CONTAINS(*, ", FullTextQuery, ")");
		}

		public string GetSqlFullTextWhereForTable(string tableName) {
			return GetSqlFullTextWhere(GetSqlFullTextFieldsForTable(tableName));
		}

		public static string GetSqlFullTextFullFieldsForTable(string tableName) {
			var fields = Util.GetSetting("SavvyActiveRecord_FullTextFields").Split("|").Where(s => s.StartsWith(tableName + ".")).ToString(",");
			return fields;
		}

		public static string GetSqlFullTextFieldsForTable(string tableName) {
			var fields = GetSqlFullTextFullFieldsForTable(tableName).Remove(tableName + ".");
			return fields;
		}

		public static List<string> GetSqlFullTextTables() {
			var fields = Util.GetSetting("SavvyActiveRecord_FullTextFields").Split("|").Select(t => t.Split(".")[0]).Distinct().ToList();
			return fields;
		}
	}
}
