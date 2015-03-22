//
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Text;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Beweb;
/*
		' search engine copyVB.right beweb limited 2000-2009
		' 11 aug 2009 - added RSS (real simple synonyms)
		' 11 aug 2009 - handle 'or' stmts (jn)
		' 02 dec 08 upgrade to dataset from datablock
		' 26 sep 07 - .net conversion
		' 17 jan 05 - fixed bug with non-integer scores
		' 23 nov 04 - added partial word search matching
		' 01 nov 04 - changed name of db field "Table" to "Tablename"
		' 10 oct 03 - added sortposition, fix bug with intro where empty
		' 21 aug 02 - takes intro from next text field if first one empty
		' 29 aug 01 - can search multiple database tables
		' 01 dec 00 - enhanced matching and added simple stemming
		' 08 may 00 - written
*/		

namespace Savvy
{
	public abstract class SavvySearch : System.Web.UI.Page// : CMA
	{
		public int hitsPerPage = 25;		 // change the number here or in the page that implements this
		protected string pageLink="";
		public  string SearchKeywords="";
		protected int NumberOfHits = 0;
		protected bool usingOrStmts = false;//using 'or' is default false. Usually 'and' each keyword together like google
		DataBlock db = null;

		public SavvySearch()
		{
			db = new DataBlock(BewebData.GetConnectionString());
			db.OpenDB(BewebData.GetConnectionString());
		}

		~SavvySearch()
		{
			db.CloseDB();
		}
		/// <summary>
		/// return reversed string
		/// </summary>
		/// <param name="str">delim value string</param>
		/// <param name="delim">char that separates items in the string (eg:' ')</param>
		/// <returns></returns>
		public string SortDelimitedString(string str, char delim) 
		{
			string []arr = str.Split(delim);
			ColumnSorter sorter = new ColumnSorter();
			Array.Sort(arr, sorter);
			Array.Reverse(arr);
			str = String.Join(" ",arr);
			return str;
		}

		protected class ColumnSorter : IComparer
		{
			public int CurrentColumn = 0;
			//public int Compare(object x, object y)
			//{
			//	ListItem rowA = (ListItem)x;
			//	ListItem rowB = (ListItem)y;
			//	return String.Compare(rowA.Text, rowB.Text);
			//}
			public int Compare(object x, object y)
			{
				return String.Compare(x.ToString(), y.ToString());
			}
		}

		public int CountMatches(string text, string regexpstr, string flags) 
		{
			int numMatches = 0;
			Regex rx = new Regex(regexpstr, RegexOptions.IgnoreCase );//,flags);


			// Find matches.
			MatchCollection matches = rx.Matches(text);

			// Report on each match.
			//foreach (Match match in matches)
			//{
			//	string word = match.Groups["word"].Value;
			//	int index = match.Index;
			//	Console.WriteLine("{0} repeated at position {1}", word, index);
			//}

			numMatches = matches.Count;//.VB.length;
			return numMatches;
		}

		public static double ScoreMatches(string txt,string keyword) 
		{
			int numMatches, result;
			// find whole word matches

			//MK: you can't just put the keyword(s) they searched for straight into a regex - characters need to be escaped first
			Regex cleanKeywords = new Regex("([\\[^$.|?*+()])");
			var keywordForRegex = cleanKeywords.Replace(keyword, "\\$1");
			// MN - todo: escape any bad chars - needs testing
			keywordForRegex = Fmt.RegExEscape(keyword);
			
			Regex rx = new Regex("\\b" + keywordForRegex + "\\b", RegexOptions.IgnoreCase);
			
			//var matchArray = new String("" + txt).match(re)
			MatchCollection matchArray = rx.Matches(txt);
			
			if (matchArray.Count==0)
				numMatches = 0;
			else
				numMatches = matchArray.Count;//.VB.length

			result = 0;
			if (numMatches == 0) {
				result += 0;
			} else if (numMatches == 1) {
				result += 5;
			} else if (numMatches == 2) {
				result += 7;
			} else if (numMatches < 5) {
				result += 8;
			} else {
				result += 9;
			}	

			if (numMatches==0) {
				// find partial word matches
				rx = new Regex(keywordForRegex, RegexOptions.IgnoreCase);
				//matchArray = new String("" + txt).match(re)
				matchArray = rx.Matches(txt);
				numMatches = matchArray.Count; //.VB.length

				if (numMatches == 0) {
					result += 0;
				} else if (numMatches == 1) {
					result += 1;
				} else if (numMatches == 2) {
					result += 2;
				} else if (numMatches < 5) {
					result += 3;
				} else {
					result += 4;
				}
			}

			return result;
		}

		private string RemoveCommonWords(string keywordstr)
		{
			//' returns the given string with common words removed - common words are ignored in the search
			string sql, newStr;
			string []keywords;
			newStr = "";
			keywords = keywordstr.Split(new Char[] { ' ' });
			foreach (string keyword in keywords)
			{
				sql = "select * from CommonWord where CommonWord = '" + Fmt.SqlString(keyword.ToLower()) +"'";
				//if (db.execute(sql).eof())
				{
					//' keyword is not in the common words list, so keep it
					newStr += keyword + " ";
				}
			}
			return newStr.Trim();
		}

		public string ScrubKeywords(string keywordstr)
		{
			//' change keywords into a space-delimited string, ready to be converted to an array

			//' change all keyword delimiters to spaces
			keywordstr = keywordstr.Replace(","," ");
			keywordstr = keywordstr.Replace(";"," ");
			keywordstr = keywordstr.Replace("+"," ");

			//' ensure only a single space between each keyword
			for(;keywordstr.IndexOf("	") > 0;)
			{
				keywordstr = keywordstr.Replace("	"," ");
			}

			//' remove any spaces in front or at end
			keywordstr = keywordstr.Trim();

			//' remove common words
			keywordstr = RemoveCommonWords(keywordstr);

			return keywordstr;
		}

		public bool ContainsKeyword(string text, string	keyword)
		{
			bool result = false;
			//' note: case behaviour is based on Altavista with slight modification:
			//' When you use lowercase text, the search service finds uppercase, lowercase and mixed case results. 
			//' When you use mixed case text, it finds mixed case and uppercase results. 
			//' When you use uppercase it finds only uppercase.
			//' Example: When you search for california, you'll find California, california, and CALIFORNIA in your results pages. However, when you search for California, you'll only see California and CALIFORNIA in the results pages.
			if (keyword == keyword.ToLower()) 
			{
				//' lower case keyword searches for any case
				result = VB.instr(VB.lcase(text),keyword) > 0;
			}else
			{	
				//' keyword containing uppercase letters searches for exact case OR all caps
				result = VB.instr(text,keyword) > 0;
				if (!result) result = VB.instr(text,VB.ucase(keyword)) > 0 ;
			}

			//' if the keyword ends in S, check the root word for a hit (eg text may contain "shaver" but you searched for "shavers")
			if (!result && VB.lcase(VB.right(keyword,1)) == "s")
			{
				keyword = VB.left(keyword, VB.len(keyword)-1);
				if (keyword == VB.lcase(keyword))
				{
					//' lower case keyword searches for any case
					result = VB.instr(VB.lcase(text),keyword) > 0;
				}else
				{
					//' keyword containing uppercase letters searches for exact case
					result = VB.instr(text,keyword) > 0;
					if (!result) { result = VB.instr(text,VB.ucase(keyword)) > 0 ;		}
				}
			}
			return result;
		}

		public static double CalculateScore(string title, string fulltext, DateTime? itemDate, string []keywords, string originalKeywords)
		{
			//' calculate score of this record
			//' give priority to whole search phrase, title matches, whole word matches, correct case matches
			double score = 0;

			//' see if it contains the whole exact phrase
			//'if ContainsKeyword(title,originalKeywords) then score = score + 10
			//'if ContainsKeyword(fulltext,originalKeywords) then score = score + 8
			score = score + ScoreMatches(title, originalKeywords) * 3;
			score = score + ScoreMatches(fulltext, originalKeywords) * 2;

			//' see if it contains any of the keywords
			foreach (string keyword in keywords)
			{
				//'if ContainsKeyword(title,keyword) then score = score + 4
				//'if ContainsKeyword(fulltext,keyword) then score = score + 3
				score = score + ScoreMatches(title, keyword) * 2;
				score = score + ScoreMatches(fulltext, keyword);
				//' include simple stems
				if (VB.right(keyword, 1) == "s" || VB.right(keyword, 1) == "x")
				{
					score = score + ScoreMatches(title, keyword + "es");
					score = score + ScoreMatches(fulltext, keyword + "es") * 0.5;
				}
				else
				{
					score = score + ScoreMatches(title, keyword + "s");
					score = score + ScoreMatches(fulltext, keyword + "s") * 0.5;
				}
				score = score + ScoreMatches(title, keyword + "ing");
				score = score + ScoreMatches(fulltext, keyword + "ing") * 0.5;
				score = score + ScoreMatches(title, keyword + "ed");
				score = score + ScoreMatches(fulltext, keyword + "ed") * 0.5;

				if (VB.right(keyword, 1) == "s")//then
				{
					score = score + ScoreMatches(title, VB.left(keyword, VB.len(keyword) - 1)) * 0.75;
					score = score + ScoreMatches(fulltext, VB.left(keyword, VB.len(keyword) - 1)) * 0.5;
				}//end if
			}//next
			//score = int(score)
			//'response.write "<br>score:" & score & " " & Title & "<br>" & Synopsis & "<br><br>"

			//' add bonus points if it is a recent article
			if (score > 0)// then
			{
				DateTime now = DateTime.Now;
				if (!IsNull(itemDate))// then
				{
					if (itemDate > (now.AddDays(-10)))// then 
					{
						score = score + 10;
					}
					else if (itemDate > (now.AddDays(-30)))// then 
					{
						score = score + 5;
					}
					else if (itemDate > (now.AddDays(-90)))// then 
					{
						score = score + 3;
					}
					else if (itemDate < (now.AddDays(- 365 * 2)))// then 
					{
						score = score - 10;
					}
					else if (itemDate < (now.AddDays(- 365)))// then 
					{
						score = score - 5;
					}//end if
				}//end if
			}//end if

			//'score= 0
			//'	score = ScoreMatches(title,originalKeywords)
			return score;
		}

		/// <summary>
		/// A simpler and faster version of the usual algorithm
		/// </summary>
		public static double CalculateScoreFast(string title, string fulltext, DateTime? itemDate, KeywordSearch keywords)
		{
			//' calculate score of this record
			//' give priority to whole search phrase, title matches, whole word matches, correct case matches
			double score = 0;

			if (keywords.Keyphrases.Count > 1) {
				// more than one phrase or word
				//' see if it contains the whole exact phrase
				score = score + ScoreMatches(title, keywords.OriginalKeywords) * 3;
				score = score + ScoreMatches(fulltext, keywords.OriginalKeywords) * 2;
			}
			
			//' see if it contains any of the keywords
			foreach (string keyword in keywords.Keyphrases)
			{
				score = score + ScoreMatches(title, keyword) * 2;
				score = score + ScoreMatches(fulltext, keyword);
			}//next

			//' add bonus points if it is a recent article
			if (score > 0)// then
			{
				DateTime now = DateTime.Now;
				if (!IsNull(itemDate))// then
				{
					double daysAgo = (now - itemDate.Value).TotalDays;
					if (daysAgo < 15) {
						score = score + 22 - daysAgo;
					} else if(daysAgo < 30){
						score = score + 5;
					} else if(daysAgo < 90){
						score = score + 3;
					} else if(daysAgo < 365){
						score = score + 0;
					} else if(daysAgo < 365*2){
						score = score - 5;
					} else{
						double quartersAgo = daysAgo / 90;
						score = score - quartersAgo;
					}
				
				}//end if
			}//end if

			return score;
		}

		private static bool IsNull(DateTime? itemDate)
		{
			return (itemDate==null || itemDate==VB.DateNull);
		}
		
		/// <summary>
		/// note: if you pass SearchArea on the querystring, filter the search area
		/// </summary>
		/// <param name="originalKeywords"></param>
		/// <returns></returns>
		public string BuildHitString(string originalKeywords)
		{
			string []keywords;
			string hitstr;
			DataBlock rs;
			//int score, scorestr,id;
			string sql, scrubbedKeywords;
			int searchAreaID;
			int sortPosition;
			scrubbedKeywords = ScrubKeywords(originalKeywords);
			keywords = scrubbedKeywords.Split(new Char[] { ' ' });

			hitstr = "";

			//' search chosen areas of database
			sql = "select * from SearchArea  ";
			string useArea  = "none";
			if (HttpContext.Current.Request["SearchArea"]!=null) //then
			{
				useArea  =HttpContext.Current.Request["SearchArea"];
				sql = sql + " where AreaTitle='" + Fmt.SqlString(useArea) + "'";
			}
			sql = sql + " order by sortorder";
			rs = db.execute(sql);
			//rs = BewebData.GetDataSet(sql);//todo: convert to dataset usage or similar
			if(rs.eof())throw new Exception("missing search area data, area ["+useArea+"] not found");
			while (!rs.eof())
			{
				sql = GetAreaSql(rs);

				//' add a where clause, just to reduce the amount of data we need to trawl through
				//' GetKeywordSearchSqlWhere is in codelib (added 27 Jun 07)
			
				sql += " where 1=1 " + GetKeywordSearchSqlWhere(originalKeywords, rs.GetValue("TitleField") + "," + rs.GetValue("TextFields"), "stem");
				// MK change: removed TitleField - so that you can write "StockCode + ' ' + Heading AS sch" for the search results
				// does mean that you have to specify it again in the fields though
				//sql += " where 1=1" + GetKeywordSearchSqlWhere(originalKeywords,rs.GetValue("TextFields")+""+ rs.GetValue("TextFields"), "stem");
				string whereClauseFromDatabase = rs.GetValue("WhereClause") + "";
				if (whereClauseFromDatabase != "")
				{

					whereClauseFromDatabase = CustomWhereClauseReplace(whereClauseFromDatabase);
					sql += " and (" + whereClauseFromDatabase + ")";
				}

				if (rs.GetValue("DateField")!="")
				{
					sql = sql + " order by " + rs.GetValue("DateField") + " desc";
				}
				//Response.Write(sql + "<br>");

				searchAreaID = rs.GetValueInt("searchAreaID");

				sortPosition = rs.GetValueInt("SortOrder");
				if (sortPosition==0)sortPosition = 50;
				sortPosition = (int)((100.0 - (double)sortPosition) * 0.2);

				if (VB.IsNull(rs.GetValue("Tablename"))) {
					throw new Exception("Savvy Search: Tablename is blank. You need to specify a table name or the word 'file' to search.");
				} else if (rs.GetValue("Tablename") == "file") {
					hitstr = hitstr + BuildFileHitString(searchAreaID, rs.GetValue("Page"), rs.GetValue("PageTitle"), keywords, originalKeywords, sortPosition);
				}else			 //page?
				{
					Trace.Write("Area hit string["+sql+"]");
					hitstr = hitstr + BuildAreaHitString(searchAreaID, sql, keywords, originalKeywords, sortPosition);
				}
				rs.movenext();
			}//loop
			//rs.close
			//set rs = nothing

			// search html files
			hitstr = hitstr + SearchHTMLFiles(keywords, originalKeywords);

			//' sort the hits
			hitstr = VB.trim(hitstr);
			hitstr = SortDelimitedString(hitstr,' ');
			return hitstr;
		}

		public virtual string CustomWhereClauseReplace(string whereClauseFromDatabase)
		{
			return whereClauseFromDatabase;
		}
		
		public virtual string ApplySynonyms(string searchText)
		{
			return searchText;
		}
		
		/// <summary>
		/// split keywords on quoted phrases or individual words
		///	 create a sql string for where each keyword matches one or more fields in the fieldsCSV list
		///		 if options string contains "stem", any words ending in "s", "es", "ed" or "ing" will have that suffix removed
		///	 note this sql searches partial words - it does not attempt to match whole words only
		/// </summary>
		/// <param name="keywords"></param>
		/// <param name="fieldsCSV"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		protected static string GetKeywordSearchSqlWhere(string keywords, string fieldsCSV, string options)
		{
			return SqlStringBuilder.GetKeywordSearchSqlWhere( keywords,  fieldsCSV,  options);
		}

		private string GetAreaSql(DataBlock rs)
		{
			string sql = "select " + rs.GetValue("IDField") + ", " + rs.GetValue("TitleField");
			if (String.IsNullOrEmpty(rs.GetValue("DateField")))
			{
				sql = sql + ",null AS NullField";
			} else
			{
				sql = sql + ", " + rs.GetValue("DateField");
			}
			sql = sql + "," + rs.GetValue("TextFields");//		 ' textfields must be a list separated by commas
			sql = sql + " from " + rs.GetValue("Tablename");
			return sql;
		}

		private string GetHitSql(DataBlock rs, int idvalue)
		{
			string sql = GetAreaSql(rs) + " where " + rs.GetValue("IDField") + " = " + idvalue;
			return sql;
		}

		public string BuildAreaHitString(int searchAreaID, string sql, string[] keywords, string originalKeywords, int sortPosition)
		{
			//' returns hit string for a given search area
			string hitstr;
			DataBlock rs;
			int id;
			double score;
			string scorestr;
			int maxFieldNum = 0;
			string title;
			DateTime itemDate;
			string fulltext;
			hitstr = "";
			//'Response.Write("<font color=""red"" size=""1"" face=""sans-serif"">DEBUG: sql["& sql &"] </font><br><!-- -->"+VB.crlf)
			rs = db.execute(sql);
			if (!rs.eof())
			{
				maxFieldNum = rs.NumberOfColumns - 1;
			}

			if(rs.NumberOfColumns<0){Logging.eout("search: no columns");}
			while (!rs.eof())
			{
				id = rs.GetValueInt(0);
				title = rs.GetValue(1);
				itemDate = rs.GetValueDate(2);
				fulltext = "";
				for (int fieldNum = 3 ;fieldNum <= maxFieldNum;fieldNum++)
				{
					fulltext = fulltext + rs.GetValue(fieldNum) + VB.crlf + VB.crlf;
				}
				score = CalculateScore(title, fulltext, itemDate, keywords, originalKeywords);
				if (score > 0)
				{
					score = score + sortPosition;
				}
				scorestr = VB.right("00" + score, 3) + ":" + id + ":" + searchAreaID; //		 ' leading zeros to allow alphabetical sort

				//' append to results string
				if (score > 0)
				{
					hitstr = hitstr + scorestr + " ";
				}

				rs.movenext();
			}//loop
			//rs.close
			//set rs = nothing
			return hitstr;
		}

		private string BuildFileHitString(int searchAreaID, string filename, string title, string[] keywords, string originalKeywords, int sortPosition)
		{
			//' returns hit string for a given file search area
			string hitstr, scorestr;
			//DataBlock rs;
			double score;
			int id;//, fieldNum, maxFieldNum;
				 
			hitstr = "";
			//set fs = server.createobject("scripting.filesystemobject")


			//set stream = fs.OpenTextFile(server.mappath(fiVB.lename))
			//txt = stream.ReadAll
			//stream.close

			string loadPath = HttpContext.Current.Server.MapPath(filename);
			FileStream fs = null;
			try
			{

				// Open the stream and read it back.
				fs = File.Open(
					loadPath,
					FileMode.Open,
					FileAccess.Read,
					FileShare.ReadWrite);
			}catch (Exception)
			{
				Logging.eout("Search: Failed to open file ["+loadPath+"] ");
			}
			StringBuilder sb = new StringBuilder();
			if (fs != null)
			{
				byte[] b = new byte[1024];
				UTF8Encoding temp = new UTF8Encoding(true);

				while (fs.Read(b, 0, b.Length) > 0)
				{
					//fileSource += temp.GetString(b);
					sb.Append(temp.GetString(b));
				}
				fs.Close();
			}
			string txt = sb.ToString();

			if (VB.instr(txt, "CheckUserLogin") == 0) //then
			{
				//' not a members-only page
				id = 1;
				string fulltext = Fmt.StripTags(txt);
				DateTime itemDate = VB.DateNull;
				score = CalculateScore(title, fulltext, itemDate, keywords, originalKeywords);
				if (score > 0 )score = score + sortPosition;
				scorestr = VB.right("00"+score,3) + ":" + id + ":" + searchAreaID;//		 ' leading zeros to allow alphabetical sort

				//' append to results string
				if (score > 0)// then
				{
					hitstr = hitstr + scorestr + " ";
				}
			}

			return hitstr;
		}

		public string SearchHTMLFiles(string[] keywords, string originalKeywords)
		{
			//dim fs, file, folder, ext, txt, hitstr
			string hitstr = "";
			
			// Create a reference to the current directory.
			DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
			// Create an array representing the files in the current directory.
			FileInfo[] fi = di.GetFiles();
			Console.WriteLine("The following files exist in the current directory:");
			// Print out the names of the files in the current directory.
			foreach (FileInfo file in fi)
			{
				Console.WriteLine(file.Name);
			
			
				//set fs = server.createobject("scripting.filesystemobject");
				//set folder = fs.GetFolder(server.mappath("."));			//' current directory
				//for each file in folder.files
				string ext = VB.lcase(file.Extension);
				if (ext == "html" || ext == "htm" || ext == "asp")
				{
					//set stream = file.OpenAsTextStream
					//txt = stream.ReadAll
					//stream.close

					string loadPath = HttpContext.Current.Server.MapPath(file.Name);

					// Open the stream and read it back.
					FileStream fs = File.Open(
						loadPath,
						FileMode.Open,
						FileAccess.Read,
						FileShare.Read);

					StringBuilder sb = new StringBuilder();
					if (fs != null)
					{
						byte[] b = new byte[1024];
						UTF8Encoding temp = new UTF8Encoding(true);

						while (fs.Read(b, 0, b.Length) > 0)
						{
							//fileSource += temp.GetString(b);
							sb.Append(temp.GetString(b));
						}
						fs.Close();
					}
					string txt = sb.ToString();


					if (VB.instr(txt, "CheckUserLogin") == 0)
					{
						//' not a members-only page
						string table = "file";
						string id = file.Name;
						string title = file.Name;
						string fulltext = Fmt.StripTags(txt);
						DateTime itemDate = VB.DateNull;
						double score = CalculateScore(title, fulltext, itemDate, keywords, originalKeywords);
						string scorestr = VB.right("00" + score, 3) + ":" + id + ":" + table;	 //' leading zeros to allow alphabetical sort

						//' append to results string
						if (score > 0)
						{
							hitstr = hitstr + scorestr + " ";
						}
					}
				}
			}//next
			return hitstr;
		}

		/// <summary>
		/// this must be implemented by the derived class (it's abstract: duh)
		/// 
		/// it should draw one tag containing each search result
		/// </summary>
		/// <param name="id"></param>
		/// <param name="title"></param>
		/// <param name="datepub"></param>
		/// <param name="intro"></param>
		/// <param name="area"></param>
		/// <param name="filename"></param>
		/// <param name="hitNum"></param>
		/// <param name="score"></param>
		/// <param name="rsHit"></param>
		/// <returns></returns>
		//public abstract string CustomWriteHit(int id, string title, DateTime datepub, string intro, string area, string filename, int hitNum, double score, DataSet rsHit);
		
		/// <summary>
		/// draw one tag containing each search result. You can override this in the page that implements this class if you like
		/// </summary>
		/// <param name="id"></param>
		/// <param name="title"></param>
		/// <param name="datepub"></param>
		/// <param name="intro"></param>
		/// <param name="area"></param>
		/// <param name="filename"></param>
		/// <param name="hitNum"></param>
		/// <param name="score"></param>
		/// <param name="rsHit"></param>
		/// <returns>text for one tag containing a link to the search results</returns>
		public virtual string CustomWriteHit(
			int id, 
			string title, 
			DateTime datepub, 
			string intro, 
			string area, 
			string filename, 
			int hitNum, 
			double score, 
			DataBlock rsHit)
		{
			string res = "";
			int relevance = Convert.ToInt32(score * 4);
			if (relevance > 100)relevance = 100;

			if (filename != "")
			{
				pageLink = filename;
			}

			//			if(pageLink=="" && rsHit != null)
			//if(rsHit != null && FieldExists(rsHit, "RenderPage"))
			//{
			//		pageLink = ""+rsHit.GetValue("RenderPage");
			//}
			//res+="<tr><td class=\"search_result\" valign=top>" + hitNum + ". </td><td class=\"search_result\" valign=top>";

			res += "<li> ";
			if (pageLink != "")
			{
				res += "<a href=\"" + pageLink + "\">" + title + "</a>";
			}else
			{
				res+= "" + title + "";
			}
			if (rsHit != null)
			{
				res += " (" + area + "), " + relevance + "%)";
				//res += "<br/>";
				//string textSnippet = rsHit.GetValue("BodyTextHTML");
				//if(textSnippet.Length>0)
				//{
				//	//res += Fmt.TruncHTML(Fmt.StripTags(textSnippet), 300)+"...";
				//}
			}
			res += "</li>";
			return res;
		}

		private string WriteHit(int id, int searchAreaID, int hitNum, double score)
		{
			//' display a single item in the search results list
			DataBlock rsArea, rsHit;
			string sql, result="";
			string filename, txt;
			string title, intro, area;
			DateTime datepub = VB.DateNull;

			//'	if source="file" then
			//'		filename = id
			//'		set fs = server.createobject("scripting.filesystemobject")
			//'		set stream = fs.OpenTextFile(server.mappath(fiVB.lename))
			//'		txt = stream.ReadAll
			//'		stream.close
			//'		
			//'		title = id
			//'		abstract = VB.left(StripTags(txt),250)
			//'		CustomWriteFileHit fiVB.lename, title, abstract, score
			//'	else
			//		' source will be a table name
			//		' find in areas database
			//		'set rs = db.execute("select * from SearchArea where Table='" & source & "'")


			rsArea = db.execute("select * from SearchArea where searchAreaID=" + searchAreaID);
			if (rsArea.GetValue("Tablename") == "file" || VB.IsNull(rsArea.GetValue("Tablename")))
			{
				//set fs = server.createobject("scripting.filesystemobject")

				id = 1;
				filename = rsArea.GetValue("Page");
				area = rsArea.GetValue("AreaTitle");
				title = rsArea.GetValue("PageTitle");

				FileSystem stream = FileSystem.OpenTextFile(HttpContext.Current.Server.MapPath(filename));
				txt = stream.ReadAll();
				txt = Fmt.StripTags(txt);
				stream.close();
				intro = VB.left(txt, 250);
				//' strip out any breaks
				if (intro != "")
				{
					intro = intro.Replace(VB.crlf, " ");
				}

				result += CustomWriteHit(id, title, VB.DateNull, intro, area, filename, hitNum, score, null);
			} else
			{
				sql = GetHitSql(rsArea, id);
				rsHit = db.execute(sql);
				if (rsHit.eof() && rsHit.bof())
				{
					//' ID not found
					//' this is possible but very unlikely
					//' it could happen if an administrator deletes an article while a user is searching
					//' just skip it, no need for an error message
				} else
				{
					//'id = rsHit(0)
					int fieldcount;
					fieldcount = rsHit.NumberOfColumns;//.Columns.VB.length;//.fields.count
					title = rsHit.GetValue(1);
					datepub = rsHit.GetValueDate(2);
					intro = rsHit.GetValue(3) + "";
					if (intro == "" && fieldcount > 4)
					{
						intro = rsHit.GetValue(4) + "";
					}
					if (intro == "" && fieldcount > 5)
					{
						intro = rsHit.GetValue(5) + "";
					}
					area = rsArea.GetValue("AreaTitle");
					pageLink = rsArea.GetValue("Page") + "";
					if (pageLink != "")
					{
						if (VB.instr(pageLink, "?") != 0)
						{
							pageLink = pageLink + id;
						} else
						{
							string reqName = rsArea.GetValue("IDRequestField");
							if (reqName == "") { reqName = rsArea.GetValue("IDField"); }
							pageLink = pageLink + "?" + reqName + "=" + id;
						}
					}
					result += CustomWriteHit(id, title, datepub, intro, area, pageLink, hitNum, score, rsHit);
				}
				rsHit.close();
				//set rsHit = nothing
			}
			rsArea.close();
			//set rsArea = nothing
			return result;
		}

		private string WriteHitsPage(int pagenum, string hitstr)
		{
			//' display a page of the list of search hits
			//' note that hitNum is zero-based, but pageNum is one-based (ie first page is 1)
			string scorestr, result="";
			double score;
			string[] hits;
			int fromHitNum, pos, toHitNum, hitNum, id, source ;

			if (pagenum==0) 
			{
				pagenum = 1;
				//throw new Exception("WriteHitsPage: pagenum cannot be zero"); 
			}

			fromHitNum = (pagenum - 1) * hitsPerPage;
			toHitNum = fromHitNum + hitsPerPage;
			if(hitstr!="")
			{
				hits = hitstr.Split(new Char[] { ' ' });
				NumberOfHits = hits.Length;  // corrected this, was -1
				if (toHitNum > hits.Length) {toHitNum = VB.ubound(hits);}
			
				for(int i = fromHitNum;i<toHitNum;i++)
				{
					scorestr = hits[i];
					score = VB.cint(VB.left(scorestr,3));
					string idtext = VB.mid(scorestr,5);
					pos = VB.instr(idtext,":");
					if (pos > 0)
					{
						//id = Convert.ToInt32(idtext);
						source = VB.cint(VB.mid(idtext, pos + 1));
						if (source == 0) throw new Exception("Failed to get source from idtext[" + idtext + "], scorestr[" + scorestr + "]");
						idtext = VB.left(idtext, pos - 1);
						id = VB.cint(idtext);
						hitNum = i + 1;
						result += WriteHit(id, source, hitNum, score);
					}
				}
			}
			return result;
		}

		public virtual string WriteNotFound()
		{
			return "Sorry, no items were found to match your keywords.";
		}

		public int GetHitCount(string hitstr)
		{
			string[] hits;
			hits = hitstr.Split(new Char[] { ' ' });
			return VB.ubound(hits);
		}
		
		public int GetPageCount(string hitstr)
		{
			int hitCount;
			hitCount = GetHitCount(hitstr);
			return VB.fix((hitCount - 1) / hitsPerPage) + 1;
		}

		protected string WritePageNavigator(int currentPageNum, string hitstr, int searchID, int pageID)
		{
			int hitCount=0, maxPage=0;
			string querystr="", html = "";
			bool isPrevDisplayed = false;

			hitCount = GetHitCount(hitstr);
			maxPage = GetPageCount(hitstr);

			// there are mulitple pages of results
			html += "			<br style=\"clear:both\" /><div class=\"search_paging\">";
			html += hitCount + " result" + (hitCount == 1 ? "" : "s") + " found (page " + currentPageNum + " of " + maxPage + ")";
			
			html+=WriteSearchHiddens(hitstr,SearchKeywords,pageID, currentPageNum);
			if (maxPage > 1)
			{				
				querystr = "&hitstr=" + Server.UrlEncode(hitstr);
				querystr += "&keywords=" + Server.UrlEncode(SearchKeywords);
				string baseUrl = "search.aspx?pid=" + Crypto.EncryptID(pageID) + querystr; 

				isPrevDisplayed = false;
				if (currentPageNum > 1)
				{
					isPrevDisplayed = true;
					//html+=" | ";
					string url = baseUrl + "&pagenum=" + (currentPageNum-1);
					html+=WriteSearchPreviousButton(hitstr,SearchKeywords,pageID,currentPageNum-1);
				}
				if (currentPageNum < maxPage) 
				{ 
					if (isPrevDisplayed) 
					{
						//html+=" | ";
					}
					string url = baseUrl + "&pagenum=" + (currentPageNum+1);
					html+=WriteSearchNextButton(hitstr,SearchKeywords,pageID,currentPageNum+1);
				}
			}

			html += "</div>";
			return html;
		}
		public virtual string WriteSearchHiddens(string hitstr,string SearchKeywords,int pageID, int currentPageNumber) 
		{
			// overrideable method to return the html for the Next Page button
			string result = "";
			result += "<input type=\"hidden\" name=\"hitstr\" id=\"hitstr\" value=\""+Server.HtmlEncode(hitstr)+"\">";
			//result += "<input type=\"hidden\" name=\"keywords\" id=\"keywords\" value=\""+Server.HtmlEncode(SearchKeywords)+"\">";
			result += "<input type=\"hidden\" name=\"pid\" id=\"pid\" value=\""+Crypto.EncryptID(pageID)+"\">";
			result += "<input type=\"hidden\" name=\"pagenum\" id=\"pagenum\" value=\""+currentPageNumber+"\">";
			return result; 
		}


		public virtual string WriteSearchNextButton(string hitstr,string SearchKeywords,int pageID,int newPageNum) 
		{
			// overrideable method to return the html for the Next Page button
			string result = "";
			//result += "<input type=\"hidden\" name=\"pagenum\" id=\"pagenum\" value=\""+newPageNum+"\">";
			result += "<input type=\"submit\" name=\"search_next\" id=\"search_next\" class=\"search_next\"  value=\"Next &gt;\" onclick=\"document.getElementById('pagenum').value="+newPageNum+"\">";
			return result; 
		}
		public virtual string WriteSearchPreviousButton(string hitstr,string SearchKeywords,int pageID,int newPageNum) 
		{
			// overrideable method to return the html for the Next Page button
			string result = "";
			//result += "<input type=\"hidden\" name=\"pagenum\" id=\"pagenum\" class=\"btn btn2\" value=\""+newPageNum+"\">";
			result += "<input type=\"submit\" name=\"search_prev\" id=\"search_prev\" class=\"search_prev\" value=\"&lt; Prev\" onclick=\"document.getElementById('pagenum').value="+newPageNum+"\">";
			return result; 
		}

		//dont allow customise on this
		protected string WriteSearchResultsPage(int pagenum, string hitstr, string originalKeywords, int searchID, int pageID)
		{
			string result = "";
			if (GetHitCount(hitstr) > 0)
			{
				//result+=WriteSearchHeader();
				//result+=WritePageNavigator(pagenum, hitstr, searchID, pageID);
				//result+=WriteDivLineThin();
				result+=WriteHitsPage(pagenum, hitstr);
				//result+=WriteDivLineThin();
				result+="<div class=\"searchbreak\"></div>";
				result+="<div class=\"searchnav\">"+WritePageNavigator(pagenum, hitstr, searchID, pageID)+"</div>";
				//result+=WriteDivLineThick();
			}else
			{
				result+=WriteNotFound();
			}
			return result;
		}

		/// <summary>
		/// Write the search results
		/// </summary>
		public string WriteSearchResults()
		{
			return WriteSearchResults(0,0);
		}
		
		/// <summary>
		/// Write the search results
		/// </summary>
		/// <param name="pageID">database id in page table of the contents of the search results text page (shown above results)</param>
		/// <param name="pagenum">search page number - zero based for pageing</param>
		/// <returns></returns>
		public string WriteSearchResults(int pageID, int pagenum)
		{
			string originalKeywords, hitstr, result = "";	//		, searchfields, datefilter, section, 
			int searchID = 0;
			originalKeywords = HttpContext.Current.Request["keywords"];

			//set member
			SearchKeywords = ApplySynonyms(originalKeywords);

			if (HttpContext.Current.Request["hitstr"] != null)
			{
				//' an existing search, just changing page
				hitstr = HttpContext.Current.Request["hitstr"];
				searchID = 1; //'request("searchID")
				pagenum = Convert.ToInt32(Request["pagenum"]);

				result += WriteSearchResultsPage(pagenum, hitstr, SearchKeywords, searchID, pageID);
			} else if (SearchKeywords != null)
			{

				//' search the database
				hitstr = BuildHitString(SearchKeywords);

				//' write first page of search results to browser
				pagenum = 1;
				result += WriteSearchResultsPage(pagenum, hitstr, SearchKeywords, searchID, pageID);
			}
			return result;
		}
	}
}
