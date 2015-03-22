#define Debug
#define JSONFunctions
//#define datablock_backcompat   // comment this in for old projects
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.IO;
using Beweb;

namespace Beweb {
	/// <summary>
	/// debugging functions
	/// </summary>
	public partial class Logging {
		/// <summary>
		/// Dump all the fields in a datareader
		/// </summary>
		/// <param name="dr">a data reader full of data</param>
		//protected void DumpFields(OleDbDataReader dr)
		//{
		//  for (int scan = 0; scan < dr.FieldCount; scan++)
		//  {
		//    Response.Write("f[" + dr.GetName(scan) + "]:data[" + dr[scan] + "]<br/>");
		//  }

		//}


		/// <summary>
		/// Container for logging information, useful to pass around logging information between functions (eg for passing to an error emailing function).
		/// </summary>
		public class DiagnosticData {
			public string StackTrace;
			public string SiteTitle;
			public string Url;
			public string SqlLog;
			public string FormData;
			public string Referrer;
			public string User;
			public string Source;
			public string ErrorMessage;
			public string AdditionalMessage;
			private Dictionary<string, string> ExtraInfo = new Dictionary<string, string>();
			public List<StackTraceReport> StackTraceReports = new List<StackTraceReport>();
			public int HttpStatusCode = 500;
			public DateTime Time;
			public bool WasLogged =  false;

			public class StackTraceReport {
				public string StackTrace;
				public string Source;
				public string ErrorMessage;
			}

			public DiagnosticData(bool gatherNow) {
				if (gatherNow) {
					Gather(null);
				}
			}

			public DiagnosticData(Exception exception) {
				Gather(exception);
			}

			public void Gather(Exception exception) {
				if (exception != null) {
					// find innermost exception - in future could walk thru and display all exceptions in the exception stack
					var walkException = exception;
					while (walkException != null) {
						// could add this code, assuming the BewebCore one will generally be the most relevant
						//if (exception.Source=="BewebCore" && exception.InnerException.Source.StartsWith("System")) {
						//	break;
						//}
						if (exception.Source.StartsWith("System") || exception.Source.StartsWith("mscorlib")) {
							// make this the primary exception
							exception = walkException;
						}

						if (!(walkException is HttpUnhandledException) && walkException.Message!="Error executing child request for handler 'System.Web.Mvc.HttpHandlerUtil+ServerExecuteHttpHandlerWrapper'.") {
							StackTraceReports.Add(new StackTraceReport() { StackTrace = walkException.StackTrace, ErrorMessage = walkException.Message, Source = walkException.Source + " (type: " + walkException.GetType() + ")" });
						}
						walkException = walkException.InnerException;
					}

					StackTrace = exception.StackTrace;
					Source = exception.Source + " (type: " + exception.GetType() + ")";
					ErrorMessage = exception.Message;
					// get extra data for known exception types
					if (exception is HttpException) {
						HttpException errorDetails = (HttpException)exception;
						HttpStatusCode = errorDetails.GetHttpCode();
						if (HttpStatusCode != 500 && HttpStatusCode != 0) {
							ErrorMessage = "[HTTP " + HttpStatusCode + "] " + ErrorMessage;
						}
					}
					WebException webException = null;
					if (exception is WebException ) {
						webException = (WebException)exception;
					}
					if (exception.InnerException != null && exception.InnerException is WebException ) {
						webException = (WebException)exception.InnerException;
					}
					if (exception.InnerException != null && exception.InnerException.InnerException != null && exception.InnerException.InnerException is WebException ) {
						webException = (WebException)exception.InnerException.InnerException;
					}
					if (webException != null) {
						if (webException.Response != null) {
							exception.Data["HttpGet Response ResponseUri"] = webException.Response.ResponseUri;
							exception.Data["HttpGet Response ContentType"] = webException.Response.ContentType;
							exception.Data["HttpGet Response Headers"] = webException.Response.Headers;
							Stream responseStream = webException.Response.GetResponseStream();
							if (responseStream != null) {
								StreamReader sr = new StreamReader(responseStream);
								exception.Data["HttpGet Response Text"] = sr.ReadToEnd();
								sr.Close();
								responseStream.Close();
							}
						}
					}
					// gather extra data
					foreach (var key in exception.Data.Keys) {
						ExtraInfo.Add(key.ToString(), exception.Data[key].ToString());
					}
				} else {
					// no exception
					ErrorMessage = "Diagnostics";
					StackTrace = Logging.LogStack();
				}
				SiteTitle = Util.GetSetting("SiteName", "UnknownSite") + " " + Util.ServerIs();
				Time = DateTime.Now;
				if (Web.IsRequestInitialised) {
					Url = Web.FullRawUrl;
					SqlLog = Sql.GetTraceLogHtml();
					if (ErrorMessage.DoesntContain("dangerous")) {
						FormData = DumpForm();
					}
					Referrer = (Web.Request.UrlReferrer == null) ? "Direct page hit OR no referrer available" : Web.Request.UrlReferrer.ToString();
					User = Web.UserIpAddress + (Util.IsBewebOffice ? " [Beweb Office]" : "") + "\n" + Web.ServerVars["HTTP_USER_AGENT"];
					if (Security.IsLoggedIn) {
						User += "\nLogged In User ID: " + Security.LoggedInUserID;
					} else {
						User += "\nNot Logged In";
					}
				}
				if (!SimilarErrors.Any()) {
					originalReportedGuid = instanceGuid;
				} else {
					originalReportedGuid = SimilarErrors.First().originalReportedGuid;
				}
				if (exception != null) {
					ErrorPile.Add(this);
					if (ErrorPile.Count > 99) {   // don't take up too much memory
						ErrorPile.RemoveAt(0);
					}
				}
			}

			public void AddExtraInfo(string title, string details) {
				ExtraInfo.Add(title, details);
			}

			public override string ToString() {
				string info = "---Diagnostic Data - " + SiteTitle + " ----\n";
				if (Url != null) info += "URL: " + Url + "\n";
				if (StackTrace != null) info += "StackTrace: " + StackTrace + "\n\n";
				return info;
			}

			public string ToHtml() {
				StringList info = "<style type='text/css'>.diaglabel {font-size:10px;color:#688B9A;margin-top:8px;font-weight:bold;margin-bottom:2px;} .diagdata {font-size:12px;color:#350608;}</style>";
				info += "<div style='font-family:lucida sans,Calibri,Century Gothic,arial,sans-serif;background:white;'>\n";
				info += "<div style='font-size:18px;color:#47697E;'>Diagnostic Data - " + SiteTitle + "</div>\n";
				info += "<div class=diagdata>" + Time.FmtDateTime() + "</div>\n";
				if (Util.ServerIsLive) {
					info += "<div style='background-color:#cc0;color:#fff;font-weight:bold;'>LIVE</div>\n";
				}
				int reportCountSinceLastReport = NumberOfTimesSinceLastErrorReport;
				if (reportCountSinceLastReport > 1) {
					string statistic = reportCountSinceLastReport.ToString();
					if (reportCountSinceLastReport >= 99) {
						statistic = "Over 100";
					}
					info += "<div class=diaglabel>Repeated Error:</div><div class=diagdata>" + statistic + " times in the past " + Fmt.Plural(Util.GetSettingInt("ThrottleSameError", 60), "second") + "</div>\n";
					
				}
				if (Source != null) info += "<div class=diaglabel>Source:</div><div class=diagdata>" + Source + "</div>\n";
				if (ErrorMessage != null) info += "<div class=diaglabel>Error Message:</div><div class=diagdata style='font-size:16px;font-weight:bold;'>" + ErrorMessage.HtmlEncode().Replace("\n", "<br>") + "</div>\n";

				string logStackHtml = null;
				if (StackTrace != null) {
					logStackHtml = Logging.LogStackHtml(StackTrace, ErrorMessage);
				}

				if (ErrorMessage != null && Util.ServerIsDev) {
					if (ErrorMessage.Contains("does not contain a definition for") && ErrorMessage.Contains("FieldReferences' could be found (are you missing a using directive or an assembly reference?)")) {
						var fieldName = ErrorMessage.ExtractTextBetween("definition for '", "' and no extension", false);
						var modelName = ErrorMessage.ExtractTextBetween("argument of type 'Models.", ".", false);
						string fixTitle = "Add field " + fieldName + " to model " + modelName + " and regenerate";
						string lineThatDied = logStackHtml.ExtractTextBetween(":>>", "</div>", true);
						//Web.Write(lineThatDied);
						//Web.End();
						info += "<div class=diaglabel>Fix:</div><div class=diagdata><a href='" + Web.Root + "Error/FixError?title=" + fieldName + "&message=" + ErrorMessage.UrlEncode() + "&lineThatDied=" + lineThatDied.UrlEncode() + "'>" + fixTitle + "</a></div>\n";
					}
				}

				if (AdditionalMessage != null) info += "<div class=diaglabel>Additional Message:</div><div class=diagdata>" + AdditionalMessage.HtmlEncode().Replace("\n", "<br>") + "</div>\n";
				if (HttpStatusCode != null) info += "<div class=diaglabel>Http Status Code:</div><div class=diagdata>" + HttpStatusCode + "</div>\n";
				foreach (var item in ExtraInfo) {
					if (item.Value.Contains("div class=diagdata") || item.Value.Contains(".expandable")) {
						var colour = "maroon";
						if (info.Contains("div class=diagdata style='border-left:3px solid " + colour)) colour = "darkorange";
						if (info.Contains("div class=diagdata style='border-left:3px solid " + colour)) colour = "darkgreen";
						if (info.Contains("div class=diagdata style='border-left:3px solid " + colour)) colour = "indigo";
						if (info.Contains("div class=diagdata style='border-left:3px solid " + colour)) colour = "brown";
						if (info.Contains("div class=diagdata style='border-left:3px solid " + colour)) colour = "orangered";
						info += "<div class=diaglabel>Related Error Report - " + item.Key + ":</div><div class=diagdata style='border-left:3px solid " + colour + ";margin-left:10px;padding-left:10px;'>" + item.Value + "</div>\n";
					} else {
						info += "<div class=diaglabel>" + item.Key + ":</div><div class=diagdata>" + item.Value.HtmlEncode().Replace("\n", "<br>") + "</div>\n";
					}
				}
				if (Url != null) info += "<div class=diaglabel>URL:</div><div class=diagdata>" + Url.HtmlEncode() + "</div>\n";
				if (User != null) info += "<div class=diaglabel>User:</div><div class=diagdata>" + User.HtmlEncode().Replace("\n", "<br>") + "</div>\n";
				if (Referrer != null) info += "<div class=diaglabel>Referer:</div><div class=diagdata>" + Referrer + "</div>\n";
				if (Referrer != null) info += "<div class=diaglabel>ErrorReportGuid:</div><div class=diagdata>" + ErrorReportGuid + "</div>\n";
				if (StackTrace != null) {
					info += "<div class=diaglabel>Stack Trace:</div><div class=diagdata>" + logStackHtml + "</div>\n\n";
				}

				if (StackTraceReports.Count > 0) {
					int i = 0;
					string kind = "Outer";
					info += "<div class=diaglabel>Exception Stack:</div>\n\n";
					foreach (var stack in StackTraceReports) {
						i++;
						info += "<div style='padding-left:" + (i * 20) + "px;border-left:2px solid #ccc'>";
						if (stack.ErrorMessage == ErrorMessage) {
							info += "<div class=diaglabel>Main Exception:</div><div class=diagdata>As above</div>\n\n";
						} else {
							info += "<div class=diaglabel>"+kind+" Exception:</div><div class=diagdata><b>" + stack.ErrorMessage + "</b></div>\n\n";
							info += "<div class=diaglabel>Source:</div><div class=diagdata>" + stack.Source + "</div>\n";
							var stackHtml = Logging.LogStackHtml(stack.StackTrace, stack.ErrorMessage);
							if (stackHtml.IsNotBlank()) {
								info += "<div class=diaglabel>Stack Trace:</div><div class=diagdata>" + stackHtml + "</div>\n\n";
							}
						}
						kind = "Inner";
						info += "</div>";
					}
				}

				if (FormData != null) info += "<div class=diaglabel>Form Data:</div><div class=diagdata>" + FormData.HtmlEncode().Replace("\n", "<br>") + "</div>\n";
				if (SqlLog != null) info += "<div class=diaglabel>Sql Log:</div><div class=diagdata>" + SqlLog + "</div>\n";
				if (StackTrace != null) info += "<div class=diaglabel>Raw Stacktrace:</div><div class=diagdata><pre style='font-size:9px'>" + StackTrace + "</pre></div>\n";
				//info += "<div class=diaglabel>Current Raw Stacktrace:</div><div class=diagdata><pre style='font-size:9px'>" + Environment.StackTrace + "</pre></div>\n";
				info += "</div>";
				return info;
			}

			public static List<DiagnosticData> ErrorPile = new List<DiagnosticData>();
			private readonly Guid instanceGuid = Guid.NewGuid();
			private Guid? originalReportedGuid = null;

			public Guid ErrorReportGuid {
				get {
					return originalReportedGuid ?? instanceGuid;
				}
			}

			private IEnumerable<DiagnosticData> SimilarErrors {
				get {
					return ErrorPile.Where(error => error.IsSameCause(this));
				}
			}

			public double SecondsSinceLastHappened {
				get {
					if (!SimilarErrors.Any()) return 999;
					return SimilarErrors.Min(error => DateTime.Now - error.Time).TotalSeconds;
				}
			}

			public int NumberOfTimesPastMinute {
				get {
					return ErrorPile.Where(error => error.IsSameCause(this)).Count(error => (DateTime.Now - error.Time).TotalSeconds < 60);
				}
			}


			public double SecondsSinceLastReport {
				get {
					// find the latest logged error report
					if (!SimilarErrors.Any(error => error.WasLogged)) {
						return 0;
					}
					DiagnosticData lastReport = SimilarErrors.Where(error => error.WasLogged).OrderByDescending(error => error.Time).First();
					double seconds = (DateTime.Now - lastReport.Time).TotalSeconds;
					return seconds;
				}
			}


			public int NumberOfTimesSinceLastErrorReport {
				// 1 / +1 to include this error
				get {
					// find the last error report logged
					if (!SimilarErrors.Any(error => error.WasLogged)) {
					//Logging.dlog("NumberOfTimesSinceLastErrorReport (no logged errors) result: 1");
						return 1;
					}
					DateTime lastReportTime = SimilarErrors.Where(error => error.WasLogged).OrderByDescending(error => error.Time).First().Time;
					int count = SimilarErrors.Count(error => error.Time > lastReportTime) + 1;
					return count;

				}
			}


			private bool IsSameCause(DiagnosticData crash) {
				return this.ErrorMessage == crash.ErrorMessage && this.Url == crash.Url && this.Source == crash.Source && this.StackTrace == crash.StackTrace && this.FormData == crash.FormData && this.AdditionalMessage == crash.AdditionalMessage && this.instanceGuid != crash.instanceGuid;
			}
		}

		public static void DumpFormToFile() {
			StreamWriter fileWriter = File.AppendText(Web.Server.MapPath("~\\attachments") + "\\para.txt");

			fileWriter.WriteLine("------form----------------------------------");
			for (int i = 0; i < Web.Request.Form.AllKeys.Length; i++) {
				fileWriter.WriteLine(string.Format("{0} - {1}", Web.Request.Form.AllKeys[i], Web.Request.Form[i]));
			}
			fileWriter.WriteLine("------qs----------------------------------");
			for (int i = 0; i < Web.Request.QueryString.AllKeys.Length; i++) {
				fileWriter.WriteLine(string.Format("{0} - {1}", Web.Request.QueryString.AllKeys[i], Web.Request.QueryString[i]));
			}
			fileWriter.Flush();

			fileWriter.Close();

		}

#if datablock_backcompat
		public static string DumpFieldsHTML(DataBlock db) {
			return ""; // Fmt.Html(DumpFieldsToString(db));
		}

		/// <summary>
		/// dump out the first record - to a string
		/// </summary>
		public static string DumpFieldsToString(DataBlock db) {
			string result = "";
			int numcols = db.GetNumberOfColumns();
			result += "dumpfields\n";
			result += "----------\n";
			for (int sc = 0; sc < numcols; sc++) {
				string name = db.getColumnName(sc);
				result += "col[" + name + "], val[" + db.GetValue(name) + "]\n";

			}
			result += "----------\n";
			return result;
		}
		/// <summary>
		/// dump out the first record using dout
		/// </summary>
		public static void DumpFields(DataBlock db) {
			int numcols = db.GetNumberOfColumns();
			dout("dumpfields");
			dout("----------");
			for (int sc = 0; sc < numcols; sc++) {
				string name = db.getColumnName(sc);
				dout("col[" + name + "], val[" + db.GetValue(name) + "]");

			}
			dout("----------");
		}
#endif

		/// <summary>
		/// dump out a row using dout
		/// </summary>
		public static void DumpFields(DataRow row) {
			int numcols = row.Table.Columns.Count;
			dout("dumpfields");
			dout("----------");
			for (int sc = 0; sc < numcols; sc++) {
				string name = row.Table.Columns[sc].ColumnName;
				dout("col[" + name + "], val[" + row[sc].ToString() + "]");

			}
			dout("----------");
		}
		/// <summary>
		/// dump out a row 
		/// </summary>
		public static string DumpFieldsToString(DataRow row) {
			int numcols = row.Table.Columns.Count;
			var result = "";
			for (int sc = 0; sc < numcols; sc++) {
				string name = row.Table.Columns[sc].ColumnName;
				result += "col[" + name + "], val[" + row[sc].ToString() + "]  ";

			}
			return result;
		}
		/// <summary>
		/// dump out a table up to 10 lines
		/// </summary>
		public static string DumpTable(DataTable dt) {
			return DumpTableToHtml(dt);
		}
		/// <summary>
		/// dump out a table up to 10 lines
		/// </summary>
		public static string DumpTableToHtml(DataTable dt) {
			return DumpTableToHtml(dt, 10);
		}

		/// <summary>
		/// dump out a table up to 10 lines
		/// </summary>
		public static string DumpTableToHtml(DataTable dt, int maxDisplayRows) {
			string result = "";
			for (int rowIndex = 0; rowIndex < maxDisplayRows && rowIndex < dt.Rows.Count; rowIndex++) {
				var row = dt.Rows[rowIndex];
				int numcols = row.Table.Columns.Count;
				if (rowIndex == 0) {
					//header
					result += "<tr class=\"colhead\">";
					for (int sc = 0; sc < numcols; sc++) {
						string name = row.Table.Columns[sc].ColumnName;
						result += "<td class=\"label\">" + name + "</td>";

					}
					result += "</tr>";

				}

				result += "<tr>";
				for (int sc = 0; sc < numcols; sc++) {
					string name = row.Table.Columns[sc].ColumnName;
					result += "<td class=\"field\">" + row[sc].ToString() + "</td>";

				}
				result += "</tr>";
			}

			result = "<table>" + result + "</table>";
			//dout(result);
			return result;
		}
		/// <summary>
		/// dump out a table up to 10 lines to text with \n and ',' - for dlog logging
		/// </summary>
		public static string DumpTableToText(DataTable dt) {
			string result = "\n";
			for (int rowIndex = 0; rowIndex < 10 && rowIndex < dt.Rows.Count; rowIndex++) {
				var row = dt.Rows[rowIndex];
				int numcols = row.Table.Columns.Count;
				if (rowIndex == 0) {
					//header
					result += "";
					for (int sc = 0; sc < numcols; sc++) {
						if (result.IsNotBlank()) result += ", ";

						string name = row.Table.Columns[sc].ColumnName;
						result += "" + name + "";

					}
					result += "\n";

				}

				result += "";
				for (int sc = 0; sc < numcols; sc++) {
					if (result.IsNotBlank()) result += ", ";
					//string name = row.Table.Columns[sc].ColumnName;
					result += "" + row[sc].ToString() + "";

				}
				result += "\n";
			}

			result += "<table>" + result + "</table>";
			//dout(result);
			return result;
		}

		public static string DumpTableToJSON(DataTable dt) {
			string result = "\n";
			for (int rowIndex = 0; rowIndex < 10 && rowIndex < dt.Rows.Count; rowIndex++) {
				var row = dt.Rows[rowIndex];
				int numcols = row.Table.Columns.Count;
				if (rowIndex == 0) {
					//header
					result += "\"header\":[";
					for (int sc = 0; sc < numcols; sc++) {
						if (sc > 0) result += ", ";
						string name = row.Table.Columns[sc].ColumnName;
						result += "\"" + name + "\"";
					}
					result += "]";
				}
				if (rowIndex == 0) result += ",\"rows\":[[";
				if (rowIndex > 0) result += ",[";
				for (int sc = 0; sc < numcols; sc++) {
					if (sc > 0) result += ", ";
					//string name = row.Table.Columns[sc].ColumnName;
					//result += "\"" + row[sc].ToString().Trim().Replace("\"", "\"\"") + "\"";
#if JSONFunctions
					result += row[sc].JsonStringify();
#else                                                                               
					result += "\"" + row[sc].ToString().Trim().Replace("\"", "\"\"") + "\"";	
#endif
				}
				result += "]";
			}
			result += "]";
			result = "{\"data\":{" + result + "}}";
			//dout(result);
			//Response.Write(result);
			return result;
		}                
		public static void DumpFields(DbDataReader reader) {
			int numcols = reader.VisibleFieldCount;
			dout("dumpfields");
			dout("----------");
			for (int sc = 0; sc < numcols; sc++) {
				string name = reader.GetName(sc);
				dout("col[" + name + "], val[" + reader[sc].ToString() + "]");

			}
			dout("----------");
		}

		public static string DumpFieldsToString(DbDataReader reader) {
			int numcols = reader.VisibleFieldCount;
			string result = "";
			result += "dumpfields\n";
			result += "----------\n";
			for (int sc = 0; sc < numcols; sc++) {
				string name = reader.GetName(sc);
				result += "col[" + name + "], val[" + reader[sc].ToString() + "]";
			}
			result += "----------";
			return result;
		}

		/// <summary>
		/// call dumpform and write to dout as well
		/// </summary>
		/// <returns></returns>
		public static string DumpFormHTML() {
			string res = Fmt.Text(DumpForm());
			dout(res);
			return res;
		}
		/// <summary>
		/// write a log into the attachments folder
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string dlog(string str) {
			return dlog(str, "");
		}
		public static string sqllog(string str) {
			return dlog(str, "sql-");
		}
		public static string dlog(string str, string filePrefix) {
			if (!Util.GetSettingBool("WriteDLog", true)) return str;
			string res = str;
			int dayNum = VB.Day(DateTime.Now);
			string logname = filePrefix + "dlog-" + VB.right("0" + dayNum, 2) + ".txt";
			var virtualPath = Web.Root + "attachments\\logs\\";
			FileSystem.CreateFolder(Web.MapPath(virtualPath)); //create if not exists
			StreamWriter f = null;
			try {
				f = System.IO.File.AppendText(Web.MapPath(virtualPath) + logname);
				//f.WriteLine(Fmt.DateTimeCompressed(DateTime.Now) + " - " + str + " (IP: "+Web.UserIpAddress+", Browser:"+Web.Request.UserAgent+")");
				f.WriteLine(Fmt.DateTime(DateTime.Now, Fmt.DateTimePrecision.Millisecond) + " - " + str);   // MN added milliseconds and more readable date format
				f.Close();
			} catch (IOException e) {
				//Check for : The process cannot access the file dlog-07.txt' because it is being used by another process.
				if (e.Message.Contains("because it is being used by another process")) {
					// this is ok, ish - ignore locked error - too many logs
				} else {
					throw;
				}
			} finally {
				if (f != null) {
					f.Close();
					f.Dispose();
				}

			}

			// delete previous day's (from the last month) logfile, to keep clean
			int prevDayNum = VB.Day(DateTime.Now.AddDays(1));			//note that +1 days is ok! JN 20121024
			string prevlogname = "dlog-" + VB.right("0" + prevDayNum, 2) + ".txt";
			string prevfilename = Web.MapPath(virtualPath + prevlogname);
			if (File.Exists(prevfilename)) {
				File.Delete(prevfilename);
			}

			return res;
		}

		/// <summary>
		/// Write the form and querystring to a string, then you should call dout to print it out
		/// </summary>
		/// <example>dout(DumpForm());</example>
		/// <returns>string of debug info</returns>
		public static string DumpForm() {
			string result = "";
			result += "Dump Form\n";
			for (int scan = 0; scan < HttpContext.Current.Request.Form.Count; scan++) {
				string formItem = HttpContext.Current.Request.Form[scan];
				string formItemName = HttpContext.Current.Request.Form.AllKeys[scan];

				//if(formItem=="")break;

				result += "[" + scan + "]: [" + formItemName + "] = [" + formItem + "]\n";
			}

			result += "Dump QueryString\n";
			for (int scan = 0; scan < HttpContext.Current.Request.QueryString.Count; scan++) {
				string formItem = HttpContext.Current.Request.QueryString[scan];
				string formItemName = HttpContext.Current.Request.QueryString.AllKeys[scan];
				//if(formItem=="")break;

				result += "[" + scan + "]: [" + formItemName + "] = [" + formItem + "]\n";
			}
			return result;
		}

		public static string DumpCache() {
			string result = "";
			result += "Dump Cache\n";
#if JSONFunctions
			int i = 0;
			var internalCache = Web.Cache.InternalCache;
			foreach (var t in internalCache) {
				i++;
				System.Collections.DictionaryEntry entry = (System.Collections.DictionaryEntry)t;
				object key = entry.Key;
				object obj = internalCache.GetType().GetMethod("Get", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(internalCache, new object[] { key, 1 });
				PropertyInfo prop = obj.GetType().GetProperty("UtcExpires", BindingFlags.NonPublic | BindingFlags.Instance);
				DateTime expire = (DateTime)prop.GetValue(obj, null);
				result += i + "." + " " + key + ". Data: " + entry.Value.ToString() + "\n";
				result += "\n";
			}
#endif
			return result;
		}

		/// <summary>
		/// Debug out
		/// </summary>
		/// <param name="str">string to write out</param>
		/// <param name="variable">variable to write out - assumes it has a readable .ToString() implemented</param>
		/// <returns></returns>
		public static void bout(string str, object variable) {
			dout(str + "[" + variable + "]");
		}
		/// <summary>
		/// write a blue line to screen, with flush
		/// </summary>
		/// <param name="str"></param>
		/// <param name="variable"></param>
		public static void dout(string str, object variable) {
			bout(str + "[" + variable + "]");
		}
		/// <summary>
		/// Debug out to the page(if appsetting WriteDOut is true) and trace. to view the trace, go to /trace.axd on the web site 
		/// </summary>
		/// <param name="str">string to write out</param>
		/// <returns></returns>
		public static void dout(string str) {
			if (Util.GetSettingBool("WriteDOut", true)) {
				HttpContext.Current.Response.Write("<font color=\"green\" size=\"1\" face=\"sans-serif\">DEBUG: " + str + " </font><br/><!-- -->" + VB.crlf);
				Web.Flush();
			}
			HttpContext.Current.Trace.Write("DEBUG:[" + str + "]");
		}
		/// <summary>
		/// write a blue line to screen, with flush
		/// 
		/// </summary>
		/// <param name="str"></param>
		public static void bout(string str) {
			HttpContext.Current.Response.Write("<font color=\"blue\" size=\"1\" face=\"sans-serif\">DEBUG: " + str + " </font><br/><!-- -->" + VB.crlf);
			Web.Flush();
		}

		/// <summary>
		/// Trace to the trace log. This is needed as the trace context is available in this class 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static void trace(string str) {
			HttpContext.Current.Trace.Write("Trace:[" + str + "]");
			Web.PageGlobals["TraceLog"] += "\n" + Fmt.DateTimeCompressed(DateTime.Now) + " - " + str;
		}
		public static string DumpTraceHTML() {
			return ("<br><br>Trace<br>" + Web.PageGlobals["TraceLog"] + "").Replace("\n", "<br>");
		}

		/// <summary>
		/// return a stack trace without having an exception occur (no line numbers though sorry)
		/// </summary>
		/// <returns>a stack trace, lines separated by \n</returns>
		public static string LogStack() {
			return Environment.StackTrace;    // this is better!
			//string result = "";
			//var trace = new System.Diagnostics.StackTrace(true);
			//foreach (var frame in trace.GetFrames())
			//{
			//  var method = frame.GetMethod();
			//  if (method.Name.Equals("LogStack")) continue;
			//  string fileName = frame.GetFileName();
			//  int lineNum = frame.GetFileLineNumber();
			//  int colNum = frame.GetFileColumnNumber();
			//  string methodName = method.ReflectedType != null ? method.ReflectedType.Name : string.Empty;
			//  result+="\n"+string.Format("{0}::{1} in {2} line {3} (col {4})", 
			//    methodName,
			//    method.Name, fileName, lineNum, colNum);
			//}
			//return result;
		}

		/// <summary>
		/// Pretty version of the stacktrace
		/// </summary>
		/// <returns></returns>
		public static string LogStackHtml() {
			return LogStackHtml(Environment.StackTrace, null);
		}

		/// <summary>
		/// Pretty version of the stacktrace
		/// </summary>
		/// <returns></returns>
		public static string LogStackHtml(string stackTrace) {
			return LogStackHtml(stackTrace, null);
		}

		/// <summary>
		/// Pretty version of the stacktrace
		/// </summary>
		/// <returns></returns>
		public static string LogStackHtml(string stackTrace, string errorMessage) {
			bool showSourceCode = true;  // this could be a param if needed later
			// grab the dev file path which is extra clutter
			var devMachinePath = ExtractDevMachinePath(stackTrace);

			string[] lines = stackTrace.Split('\n');
			string html = "";

			foreach (string line in lines) {
				string lineNumberColour = "color:#C73B0B;";
				string methodColour = "color:#978E43;";
				if (line.Trim().StartsWith("at System.Environment.")) {
					// skip stacktrace itself
				} else if (line.Trim().StartsWith("at lambda_method(Closure , ControllerBase , Object[] )")) {
					html += "from ASP.NET MVC Routing System" + "<br/>";
					//}else if (line.Trim().StartsWith("at System.Web.Mvc.ReflectedActionDescriptor.Execute(ControllerContext controllerContext")) {
					//	html += "from ASP.NET MVC Routing System" + "<br/>";
				} else if (line.Trim().StartsWith("at System.Web.Compilation.BuildManager.CompileWebFile")) {
					// grab the source and line of file it was trying to compile as the source
					//([a-z]:\\[a-z0-9_\\\-\.]*\.[a-z]+)\((\d*)\):
					string htmlLine = line + "<br/>";
					if (errorMessage.Substring(1, 2) == ":\\" && errorMessage.Contains("): error")) {
						string fileName = errorMessage.LeftUntil("): error");
						int lineNum = fileName.RightFrom("(").LeftUntil(")").ToInt(0);
						fileName = fileName.LeftUntilLast("(");
						htmlLine += "Error compiling view <b>" + fileName + "</b> line <b style='" + lineNumberColour + "font-size:13px;'>" + lineNum + "</b>";
						if (showSourceCode) {
							htmlLine += GrabSourceCodeSnippet(lineNum, fileName);
						}
						html += htmlLine + "<br/>";
					}
				} else if (line.Trim().StartsWith("at System.Web.")) {
					// skip web page internal crap
				} else if (line.Trim().StartsWith("at System.RuntimeMethodHandle.")) {
					// skip extra reflection farting round
				} else if (line.Trim().StartsWith("at System.Reflection.RuntimeMethodInfo.")) {
					// skip extra reflection farting round
				} else if (line.Trim().StartsWith("at Beweb.Logging.LogStack() in ") || line.Trim().StartsWith("at Beweb.Logging.DiagnosticData") || line.Trim().StartsWith("at Beweb.Logging.GetDiagnosticsDumpHtml") || line.Trim().StartsWith("at Beweb.Logging.OutputDiagnosticsDump")) {
					// ignore this call itself
				} else {
					string filePath = line.ExtractTextBetween(") in ", ":line", false);
					int? lineNumber = line.RightFrom(":line ").ToInt(null);
					string htmlLine = line.Trim();
					htmlLine = htmlLine.RemovePrefix("at");
					htmlLine = htmlLine.Replace("..ctor(", ".constructor(");
					if (devMachinePath.IsNotBlank()) {
						// remove from display to prettify
						htmlLine = htmlLine.ReplaceInsensitive(devMachinePath, "");

						if (Web.IsRunningOnWebServer) {
							// replace dev path with current server path
							string serverPath = Web.MapPath("~");
							if (!serverPath.EndsWith("\\")) serverPath += "\\";
							string correctedFilePath = filePath.ReplaceInsensitive(devMachinePath, serverPath);
							if (File.Exists(correctedFilePath)) {
								filePath = correctedFilePath;
							} else {
								if (Web.IsRequestInitialised && Web.Request["debugdevpath"] == "1") Web.Write("[using actual filePath=" + filePath + "]");
							}
						}
						//filePath = Web.MapPath(filePath);
						//Web.Write("[mapped="+filePath+"]");
					}

					// tone down some less important lines, which we don't want to completely ignore but just dim out
					if (line.Trim().StartsWith("at System.")) {
						methodColour = "";
						lineNumberColour = "";
					}
					if (line.Trim().StartsWith("at Beweb.") || line.Trim().StartsWith("at Savvy.")) {
						methodColour = "color:#63964B;";
						lineNumberColour = "";
					}

					htmlLine = System.Text.RegularExpressions.Regex.Replace(htmlLine, "Controllers\\\\([a-zA-Z0-9_]*)Controller.cs:line (\\d*)", "Controllers\\<span style='background:#FCF1D1'>$1Controller.cs:line $2</span>");
					htmlLine = System.Text.RegularExpressions.Regex.Replace(htmlLine, "Models\\\\([a-zA-Z0-9_]*).cs:line (\\d*)", "Models\\<span style='background:#FCF1D1'>$1.cs:line $2</span>");

					htmlLine = htmlLine.ReplaceLast("\\", "</span>\\<b style='" + lineNumberColour + "font-size:13px;'>") + "</b>";

					string methodName = htmlLine.LeftUntil("(").RightFrom(".");
					htmlLine = htmlLine.ReplaceFirst(methodName + "(", "<b style='" + methodColour + "font-size:14px;'>" + methodName + "(</b><span style='color:#333;'>");
					htmlLine = htmlLine.ReplaceFirst(") in ", "</span><b style='" + methodColour + "font-size:14px;'>)</b> - <span style='font-size:10px;'>");

					string className = htmlLine.LeftUntil(".<b ");
					className = className.RightFrom(".");
					htmlLine = htmlLine.ReplaceFirst("." + className + ".<b ", ".<b style='" + methodColour + "font-size:12px;'>" + className + ".</b><b ");

					htmlLine = htmlLine.ReplaceLast(":line", " line");

					if (showSourceCode && !line.Trim().StartsWith("at System.")) {
						htmlLine += GrabSourceCodeSnippet(lineNumber, filePath);
					}

					html += htmlLine + "<br/>";
				}
			}

			html = "<div style='color:#888;font-size:12px;'>" + html + "</div>";
			return html;
		}

		private static string GrabSourceCodeSnippet(int? lineNumber, string filePath) {
			string htmlLine = null;
			if (lineNumber != null && filePath.IsNotBlank() && File.Exists(filePath)) {
				string sourceText = File.ReadAllText(filePath);
				var sourceLines = sourceText.Split('\n');
				StringList sourceDisplay = "";
				for (int i = 1; i <= sourceLines.Length; i++) {
					if (i > lineNumber - 5 && i < lineNumber + 5) {
						string code = sourceLines[i - 1];
						code = code.Replace("\t", "  ");   // format tabs as 2 spaces to make more readable/fit better on iphone
						code = code.HtmlEncode();
						if (i == lineNumber) {
							sourceDisplay += "<div style='background-color:#ffd'>" + i + ":>>" + code + "</div>";
						} else {
							sourceDisplay += "<div>" + i + ":  " + code + "</div>";
						}
					}
				}
				htmlLine += "<br><pre style='background-color:#eee;color:#333;padding:6px;font-family: \"Lucida Console\", \"Courier New\", Courier;'>" + sourceDisplay + "</pre>";
			} else {
				if (lineNumber == null) {
					htmlLine += " <small>(line numbers not available)</small>";
				} else {
					htmlLine += " <small>(PDB out of date or source file not on server " + filePath + ")</small>";
				}
			}
			return htmlLine;
		}

		private static string ExtractDevMachinePath(string stackTrace) {
			string result = "";
			result = stackTrace.ExtractTextBetween("at Beweb.Logging.LogStack() in ", "\\BewebCore\\Beweb\\Debug.cs", false);
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at Beweb.Logging.DiagnosticData.ToHtml() in ", "\\BewebCore\\Beweb\\Debug.cs", false);
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at Site.Controllers.", "\\Controllers\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at Site.SiteCustom.", "\\SiteCustom\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at Models.", "\\Models\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at Beweb.", "\\BewebCore\\Beweb\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			// views
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at ASP.", "\\Areas\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at ASP.", "\\Views\\", false);
				if (result != "") {
					result = result.RightFrom(") in ");
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at ASP.", ".aspx", false);
				if (result != "") {
					result = result.RightFrom(") in ");
					result = result.LeftUntilLast("\\") + "\\";
				}
			}
			if (result == "") {
				result = stackTrace.ExtractTextBetween("at ASP.", ".master", false);
				if (result != "") {
					result = result.RightFrom(") in ");
					result = result.LeftUntilLast("\\") + "\\";
				}
			}

			// fallback
			if (result == "") {
				// this is not very good, it does not know how many folders to go up and just guesses at 3
				result = "C:\\" + stackTrace.ExtractTextBetween(") in C:\\", ":line ", false);
				for (int i = 0; i < 3 && result.Contains("\\"); i++) {
					result = result.Left(result.LastIndexOf("\\"));
				}
				if (Web.IsRequestInitialised && Web.Request["debugdevpath"] == "1") Web.Write("[fallbackC]");
			}
			if (result == "") {
				// this is not very good, it does not know how many folders to go up and just guesses at 3
				result = "F:\\" + stackTrace.ExtractTextBetween(") in F:\\", ":line ", false);
				for (int i = 0; i < 3 && result.Contains("\\"); i++) {
					result = result.Left(result.LastIndexOf("\\"));
				}
				if (Web.IsRequestInitialised && Web.Request["debugdevpath"] == "1") Web.Write("[fallbackF]");
			}
			if (result != "") {
				result += "\\";
				// this breaks Http Get Async - cannot call web.Request within an Async Thread 
				//	if (Web.IsRequestInitialised && Web.Request["debugdevpath"]=="1") Web.Write("[devMachinePath" + result + "]");
			}
			return result;
		}


		/// <summary>
		/// Error out, stop the response
		/// </summary>
		/// <param name="str"></param>
		/// <returns>nothing</returns>
		public static void eout(string str) {
			HttpContext.Current.Response.Write("<font color=\"red\" size=\"1\" face=\"sans-serif\">ERROR: " + str + " </font><br/><!-- -->" + VB.crlf);
			Console.Write("Error [" + str + "]");
			HttpContext.Current.Trace.Write("ERROR:[" + str + "]");
			HttpContext.Current.Response.End();
		}


		public static string GetDiagnosticsDumpHtml() {
			var data = new DiagnosticData(true);
			return data.ToHtml();
		}

		public static void OutputDiagnosticsDump() {
			Web.Write(GetDiagnosticsDumpHtml());
		}

		public static void dout(Sql sql) {
			dout(sql.ToString());
		}
		
		public static string DumpObject(object obj) {
#if JSONFunctions
			return Conv.ObjectToJSON(obj);
#else	
			var result = new StringBuilder();
			result.Append("{");

			Type objType = obj.GetType();
			string objName = objType.Name;

			PropertyInfo[] properties = objType.GetProperties();

			foreach (PropertyInfo property in properties) {
				if (result.Length > 1) result.Append(","); 
				string propertyName = property.Name;
				result.Append(propertyName.JsonStringify());
				result.Append(":");

				object value = null;
				try {
					value = property.GetValue(obj, null);
				} catch (Exception e) {

				}
				result.Append(value.JsonStringify());
			}
			result.Append("}");

			return result.ToString();
#endif	
		}

		public static void FixError(string message, string title, string lineThatDied) {
			if (message.Contains("does not contain a definition for") && message.Contains("FieldReferences' could be found (are you missing a using directive or an assembly reference?)")) {
				var fieldName = message.ExtractTextBetween("definition for '", "' and no extension", false);
				var modelName = message.ExtractTextBetween("argument of type 'Models.", ".", false);
				string fieldType = "nvarchar(100)";
				var callingFunc = lineThatDied.ExtractTextBetween("new Forms.", "(", false);
				if (callingFunc == "SortPositionField" || callingFunc == "IntegerField" || callingFunc == "NumberField") {
					fieldType = "int";
				} else if (callingFunc == "DateField" || callingFunc == "DateTimeField" || callingFunc == "TimeField") {
					fieldType = "datetime";
				} else if (callingFunc == "YesNoField" || callingFunc == "CheckboxField") {
					fieldType = "bit";
				} else if (callingFunc == "MoneyField") {
					fieldType = "money";
				} else if (callingFunc == "FloatField") {
					fieldType = "decimal(10,6)";
				} else if (callingFunc == "RichTextField"||callingFunc == "TextArea") {
					fieldType = "nvarchar(max)";
				}
				string fixTitle = "Add field " + fieldName + " to model " + modelName + " and regenerate";

				string tableName = modelName;
				var sql = new Sql("alter table ", tableName.SqlizeName(), "add ", fieldName.SqlizeName(), " " + fieldType);
				Web.WriteLine(title);
				Web.WriteLine(callingFunc);
				Web.WriteLine(message);
				Web.WriteLine(lineThatDied);
				Web.WriteLine(sql.Value);

				sql.Execute();
				ActiveRecordGenerator.Run();
			}
		}

	}

	/// <summary>
	/// Attribute to be used on test methods so they are automatically called by the test runner. Designed to be compatible syntax with VS test syntax.
	/// </summary>
	public class SlowTestMethodAttribute : Attribute { }

	/// <summary>
	/// Attribute to be used on test methods so they are automatically called by the test runner. Designed to be compatible syntax with VS test syntax.
	/// </summary>
	public class TestMethodAttribute : Attribute {
		public TestMethodAttribute() {

		}
		public TestMethodAttribute(string description) {
			Description = description;
		}
		public string Description { get; set; }
	}

	/// <summary>
	/// Attribute to be used on classes containing methods so they are automatically called by the test runner. Designed to be compatible syntax with VS test syntax.
	/// </summary>
	public class TestClassAttribute : Attribute { }

	/// <summary>
	/// Dummy class, simply so tests copied from MS VS test tool can be generally copied without change.
	/// </summary>
	public class TestContext { }

	/// <summary>
	/// Provides methods for unit testing and also runtime validity checking. Designed to be compatible syntax with VS test syntax.
	/// </summary>
	public static class Assert {

		/// <summary>
		/// Run all methods in the given class name that are marked with attribute [TestMethod]. Used for unit testing.
		/// </summary>
		public static void RunTests(string typeFullName) {
			Type classUnderTest = _testClasses.Find(t => t.FullName == typeFullName);
			if (classUnderTest == null) {
				Assert.Fail("Class " + typeFullName + " is not available for testing, check it is public and has public methods with [TestMethod] attributes.");
			} else {
				RunTests(classUnderTest);
			}
			//Assembly ass = Assembly.GetExecutingAssembly();
			//RunTests(ass.CreateInstance(typeName));
		}

		/// <summary>
		/// Run all methods in the given class that are marked with attribute [TestMethod]. Used for unit testing.
		/// </summary>
		public static void RunTests(Type classUnderTest) {
			RunTests(Activator.CreateInstance(classUnderTest));
		}

		/// <summary>
		/// Run all methods in the given class that are marked with attribute [TestMethod]. Used for unit testing.
		/// </summary>
		public static void RunTests(object objectUnderTest) {
			Type type = objectUnderTest.GetType();
			HttpContext.Current.Response.Write("<b>TEST CLASS: " + type.FullName + "</b><br>");
			var methods = type.GetMethods();
			foreach (var method in methods) {
				if (method.IsDefined(typeof(TestMethodAttribute), false)) {
					var description = "";
					TestMethodAttribute attr = (TestMethodAttribute)Attribute.GetCustomAttribute(method.GetType(), typeof(TestMethodAttribute));
					if (attr != null && attr.Description != null) {
						description = attr.Description;
					}
					HttpContext.Current.Response.Write(method.Name + "() <i>" + description + "</i> => ");
					method.Invoke(objectUnderTest, null);
					HttpContext.Current.Response.Write("<br>");
				}
			}
			HttpContext.Current.Response.Write("<br>");
		}

		/// <summary>
		/// Run single method in the given class that are marked with attribute [TestMethod]. Used for unit testing.
		/// </summary>
		public static void RunTest(object objectUnderTest, string methodName) {
			Type type = objectUnderTest.GetType();
			var methods = type.GetMethods();
			foreach (var method in methods) {
				if (method.IsDefined(typeof(TestMethodAttribute), false)) {
					if (methodName == null || methodName == method.Name) {
						method.Invoke(objectUnderTest, null);
					}
				}
			}
		}

		/// <summary>
		/// Run all methods in the given class that are marked with attribute [TestMethod]. Used for unit testing.
		/// </summary>
		public static void PrintTests(object objectUnderTest) {
			Type type = objectUnderTest.GetType();
			HttpContext.Current.Response.Write("<b>TEST CLASS: " + type.FullName + "</b><br>");
			HttpContext.Current.Response.Write("<ul class=testCases>");
			var methods = type.GetMethods();
			foreach (var method in methods) {
				if (method.IsDefined(typeof(TestMethodAttribute), false)) {
					var description = "";
					TestMethodAttribute attr = (TestMethodAttribute)Attribute.GetCustomAttribute(method.GetType(), typeof(TestMethodAttribute));
					if (attr != null && attr.Description != null) {
						description = attr.Description;
					}
					HttpContext.Current.Response.Write("<li>" + method.Name + "</li>");
				}
			}
			HttpContext.Current.Response.Write("</ul>");
		}

		public static void RunAllTests() {
			GetAllTestClasses();
			foreach (Type type in _testClasses) {
				// found a test class
				RunTests(type);
			}
		}

		private static List<Type> _testClasses;

		public static List<Type> GetAllTestClasses() {
			if (_testClasses == null) {
				_testClasses = new List<Type>();
				//Assembly assembly = Assembly.GetExecutingAssembly();
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
					// this is making the assumption that all assemblies we need are already loaded.
					//HttpContext.Current.Response.Write(assembly.FullName + "<br>");
					try {
						foreach (Type type in assembly.GetTypes()) {
							// old way: look at custom attributes [TestClass]
							//var attribs = type.GetCustomAttributes(typeof(TestClassAttribute), false);
							//if (attribs != null && attribs.Length > 0) {
							//	// found a test class
							//	_testClasses.Add(type);
							//}
							// new way just look for test methods [TestMethod]
							var methods = type.GetMethods();
							foreach (var method in methods) {
								if (method.IsDefined(typeof(TestMethodAttribute), false)) {
									// found a test method
									_testClasses.Add(type);
									break;
								}
							}

						}
					} catch (System.Reflection.ReflectionTypeLoadException) {
					}
				}
			}
			return _testClasses;
		}

		public static void AreEqual<T>(T expectedValue, T actualValue) {
			AreEqual<T>(expectedValue, actualValue, null);
		}

		public static void AreEqual<T>(T expectedValue, T actualValue, string description) {
			bool eq = false;
			if (expectedValue == null || actualValue == null) {
				eq = Object.Equals(expectedValue, actualValue);
			} else {
				eq = EqualityComparer<T>.Default.Equals(expectedValue, actualValue);
				//eq = expectedValue.Equals(actualValue);
			}
			if (eq) {
				Pass();
			} else {
				if (expectedValue == null || actualValue == null) {
					//Fail("Expected value [" + expectedValue.ToString().HtmlEncode() + "] but actually was [" + actualValue.ToString().HtmlEncode()  + "]"); //removed 20111229 JN doesnt null.ToString() throw an execption? MN yes good point!

					Fail(description + " Expected value [" + expectedValue + "] but actually was [" + actualValue + "]");
				} else {
					//Fail("Expected value [" + expectedValue.ToString().HtmlEncode()  + "] but actually was [" + actualValue.ToString().HtmlEncode()  + "] of type [" + actualValue.GetType().FullName + "]");
					Fail(description +"Expected value [" + expectedValue + "] but actually was [" + actualValue + "] of type [" + actualValue.GetType().FullName + "]");
				}
			}
		}

		public static void IsInstanceOfType(object actualValue, Type T) {
			if (actualValue.GetType() == T) {
				Pass();
			} else {
				//Fail("Value [" + actualValue.ToString().HtmlEncode()  + "] is not of expected type [" + T.FullName + "]");
				Fail("Value [" + actualValue + "] is not of expected type [" + T.FullName + "]");
			}
		}

		//public static void AreEqual(string expectedValue, string actualValue) {
		//    if (Object.Equals(expectedValue, actualValue)) {
		//        HttpContext.Current.Response.Write("Pass");
		//    } else {
		//        AssertTrue(false, "Expected value [" + expectedValue + "] of type [" + expectedValue.GetType().FullName + "] but actually was [" + actualValue + "] of type [" + actualValue.GetType().FullName + "]");
		//    }
		//}

		//public static void AreEqual(ValueType expectedValue, ValueType actualValue) {
		//    if (Object.Equals(expectedValue, actualValue)) {
		//    //if (expectedValue == actualValue) {
		//        //if (expectedValue+"" == actualValue+"") {
		//        HttpContext.Current.Response.Write("Pass");
		//    } else {
		//        AssertTrue(false, "Expected value [" + expectedValue + "] of type [" + expectedValue.GetType().FullName + "] but actually was [" + actualValue + "] of type [" + actualValue.GetType().FullName + "]");
		//    }
		//}

		public static void IsTrue(bool condition, string message) {
			if (condition) {
				Pass();
			} else {
				Fail(message);
			}
		}

		public static void IsFalse(bool condition, string message) {
			if (condition) {
				Fail(message);
			} else {
				Pass();
			}
		}

		public static void IsNotNull(object actualValue) {
			IsNotNull(actualValue, "");
		}

		public static void IsNotNull(object actualValue, string message) {
			if (actualValue == null) {
				Fail("Expected a non-null value, but actually was null. " + message);
			} else {
				Pass();
			}
		}

		public static void IsNull(object actualValue) {
			IsNull(actualValue, "");
		}

		public static void IsNull(object actualValue, string message) {
			if (actualValue == null) {
				Pass();
			} else {
				Fail("Expected null, was actually [" + actualValue.ToString() + "]. " + message);
			}
		}

		public static void Fail(string message) {
			Web.PageGlobals["Debug:Failed"] = true;
			HttpContext.Current.Response.Write(" <b style='color:red'>TEST FAILED:</b> " + message);
		}

		public static void Pass() {
			HttpContext.Current.Response.Write(" <b style='color:green'>Pass</b> ");
		}

		public static void Note(string message) {
			HttpContext.Current.Response.Write(" <b style='color:darkblue'>"+message+"</b> ");
		}
	}
}

namespace BewebTest {
	[TestClass]
	public class TestLogging {
		[TestMethod]
		public void TestStackTraceHasLineNumbers() {
			// Logging.LogStack() Does not contain line numbers
			//	Assert.IsTrue(Logging.LogStack().Contains("Debug.cs:line "), "Does not contain line numbers. Ensure you copy the same version of the PDBs with the DLLs and have all projects set to debug level pdbonly.");
		
		}

		[SlowTestMethod]
		public void DumpTableToJSON() {
#if JSONFunctions
			var dt = new Sql("select * from gentest").GetDataTable();
			Web.Write(Logging.DumpTableToJSON(dt));
#endif
		}

		[TestMethod]
		public void LogStackHtml() {
			//Web.Write(Logging.LogStackHtml());
		}
	}

	[TestClass]
	public class TestExceptionLineNumbers {
		[TestMethod]
		public void TestError() {
			try {
				throw new Exception("original exception");
			} catch (Exception e) {
				Web.Write("this is a test - does it have line numbers? StackTrace: " + e.StackTrace);
			}
		}
	}

}