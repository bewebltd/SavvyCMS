using System;
using System.Data.Common;
using Beweb;
using Models;

namespace Site.SiteCustom {
	/// <summary>
	/// Helper methods for the Site go here
	/// </summary>
	public class SiteMain {

		public static int UsersOnline = 0;

		public SiteMain() {
			//
			// site specific logic / helpers that do not fit into their own class go here
			//
		}

		/// <summary>
		/// create a reference number of the form 6 chars, dash, 3 chars. check it is unique across all records in database
		/// </summary>
		/// <returns></returns>
		//public static string GenerateRef()
		//{
		//  string result = "";
		//  for (int sc = 0; ; sc++)
		//  {
		//	if (sc > 20) throw new Exception("ref gen failed");
		//	//result = RandomPassword.Generate(6, 6, "Q", "QWERTYPASDFGHJKLZXBNM", "23456", "2") + "-" + RandomPassword.Generate(3, 3, "W", "WXYZ", "23456", "2");
		//	result = "IY" + Crypto.RandomDigit() + Crypto.RandomDigit() + Crypto.RandomDigit() + Crypto.RandomDigit() + Crypto.RandomDigit();
		//	if (QuoteList.LoadByInsureYouRef(result).RecordCount == 0)
		//	{
		//	  break;
		//	}
		//  }

		//  return result;
		//}


		public bool Test() {
			return true;
		}



		public static void CheckForRedirectUrls() {
			// only use this if there only one url for the site and all others should redirect to it

			if (Util.GetSettingBool("EnsureCanonicalHost", false)) {
				Util.EnsureCanonicalHost();
			}

			// ensure always uses non-www URL so there are not two URLs (can use/customise this code instead of using EnsureCanonicalHost for more control)
			//if (Web.FullRawUrl.StartsWith("http://www.")) {
			//  Web.RedirectPermanently(Web.FullRawUrl.ReplaceFirst("http://www.", "http://"));
			//  return;
			//}

			// this is DB driven - urlredirect table, or uncomment this code to hardcode old to new paths.
			Redirect[] redirects = {
					//new Redirect{newurl="/page/1000/registration.aspx"  ,oldurl="registrations/default.asp"},
					//new Redirect{newurl="/page/1000/registration.aspx"  ,oldurl="registrations/default.asp"},
					//new Redirect{newurl="/page/1000/registration.aspx"  ,oldurl="registrations/default.asp"},
					//new Redirect{newurl="/page/1000/registration.aspx"  ,oldurl="registrations/default.asp"}
				};
			foreach (var redirect in redirects) {
				if (Web.FullRawUrl.Contains(redirect.oldurl)) {
					//return "Redirect.aspx?newurl=" + redirect.newurl.UrlEncode();
					Web.RedirectPermanently(redirect.newurl);
					return;
					//break;
				}
			}
			//dont like classic asp files, etc - go to home.
			//if(
			//		//Web.FullRawUrl.ToLower().EndsWith(".asp") || 
			//		Web.FullRawUrl.ToLower().EndsWith(".php") || 
			//		Web.FullRawUrl.ToLower().EndsWith(".cfm")
			//		Web.FullRawUrl.ToLower().EndsWith(".py")
			//	)
			//{
			//	Web.RedirectPermanently(Web.BaseUrl);
			//}
			//Url.Rewrite(); // note: used by older codelib version, just return and the routing engine will handle it from here
		}
		private class Redirect {
			public string oldurl = "";
			public string newurl = "";
		}

		/// <summary>
		/// load the email template from a file into a cms textblock
		/// </summary>
		/// <returns></returns>
		public static string GetTemplate() {
			var tb2 = (new Beweb.TextBlock("Email Template HTML", FileSystem.ReadTextFile("~/sitecustom/emailtemplate2.html")));
			return tb2.RawBody;
		}

			
		public static bool ExportMode {
			//  Request["exportmode"][<%=Request["exportmode"]%>]							 <br/>
			//Request["exportmode"].IsBlank()[<%=Request["exportmode"].IsBlank() %>]							 <br/>
			//Request["exportmode"].Equals("false")[<%=Request["exportmode"].Equals("False") %>]							 <br/>
			get {
				bool b = Web.Request["exportmode"] == "True";
				return b;
			}
		}
		/// <summary>
		/// write the lg for a given record, or null if log is disabled
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ShowModificationLog(ActiveRecord obj) {
#if ModificationLog
			if (!Util.GetSettingBool("UseModificationLog", false)) return null;
			int pkvalue = obj.ID_Field.ToInt(0);
			string tableName = obj.GetTableName();
			string ModLogTableName = "ModificationLog";
			var sql = new Sql("select top 100 * from ", ModLogTableName.SqlizeName(), " where TableName=", tableName.Sqlize_Text(), " ");
			sql.Add(" and RecordID=", pkvalue, "");
			sql.Add(" order by updatedate desc");

			string res = @"<tr class=""dontprint"">
						<td class=label>Change History</td>
						<td class=field><a href="""" onclick=""$('.modlog').show();$(this).hide();return false;"">show</a><div class=""modlog"" style=""display:none"">";
			int scan = 0;
			using (DbDataReader dbDataReader = sql.GetReader()) {
				foreach (DbDataRecord rsLog in dbDataReader) {
					var person = Person.LoadID((rsLog["PersonID"] + "").ToInt(0));

					var UserName = (person != null) ? person.FullName : "not available";
					string descr = rsLog["ChangeDescription"].ToString().FmtPlainTextAsHtml();
					if (!(descr.Contains("Date Modified changed") && descr.Length < 94)) {						 //eg skip Date Modified changed from &quot;29 Oct 2013 19:49am&quot; to &quot;29 Oct 2013 10:51am&quot;
						//descr = descr.Replace("<br>", " | "); // removed and applied nice formatting below (JVB) 
						res += "<b>" + rsLog["ActionType"] + "</b> on " + Fmt.DateTime(rsLog["UpdateDate"] + "")+ " by " + UserName + ":<br/> - " + descr + "<br/><br/>";
						scan++;
						if (scan > 100) {
							res += "more...";
							break;
						}
					}
				}
			}
			res += @"</div>
						</td>
					</tr>";

			//todo: remove any old mod logs when drawing a mod log page.
			return res;
#else
			return "Not available.";
#endif
		}
	}

}