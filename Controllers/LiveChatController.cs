using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Beweb;
using Site.SiteCustom;

namespace Site.Controllers {
	/// <summary>
	/// Contains partials for rendering common page elements such as individual panels that are used on several pages
	/// </summary>
	public class LiveChatController : Controller {

		public ActionResult LiveChat() {
			/*NOTE: This requires the user to be logged in.... */
			// chats the last 20 chats for today
			var data = new LiveChatViewModel();
			if (UserSession.Person != null) {
				data.EncryptedPersonID = Crypto.EncryptID(UserSession.Person.ID);
				data.IsLoggedIn = true;
			}else {
				data.IsLoggedIn = false;
			}
				var sql = new Sql("select top 20 p.FirstName + ", " ".SqlizeText(), "+ p.lastname as FullName, lc.Post, Convert(varchar, lc.DateAdded,103) as Date, convert(varchar, lc.DateAdded, 108) as Time from LiveChat lc inner join Person p on p.PersonID = lc.PersonID Order by lc.DateAdded desc");
				var chatList = sql.LoadPooList<LatestChatList>();
				data.ChatList = chatList;
				data.ChatRefresh = 2000; //(data.ChatList.Count > 0)?2000:30000;
				data.LastUpdate = Fmt.DateTime(DateTime.Now, Fmt.DateTimePrecision.Millisecond);
		
			return View(data);
		}

		public ActionResult UpdateLiveChat(string lastUpdate) {
			var sql = new Sql("select p.FirstName + ", " ".SqlizeText(), "+ p.lastname as FullName, lc.Post, Convert(varchar, lc.DateAdded,103) as Date, convert(varchar, lc.DateAdded, 108) as Time from LiveChat lc inner join Person p on p.PersonID = lc.PersonID where lc.DateAdded > ", lastUpdate.SqlizeDate(), " Order by lc.DateAdded desc");
			var data = new { ChatList = sql.LoadPooList<LatestChatList>(), ChatRefresh = 2000, LastUpdate = Fmt.DateTime(DateTime.Now, Fmt.DateTimePrecision.Millisecond) };
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public class LatestChatList {
			public string FullName { get ; set; }
			public string Post { get; set; }
			public string Date { get; set; }
			public string Time { get; set; }
		}

		public class LiveChatViewModel {
			public List<LatestChatList> ChatList { get; set; }
			public string EncryptedPersonID { get; set; }
			public string LastUpdate { get; set; }
			public int ChatRefresh { get; set; }
			public Boolean IsLoggedIn { get; set; }

		}


		public ActionResult PostToLiveChat(string id, string post) {
			var chat = new LiveChat();
			chat.PersonID = Crypto.DecryptID(id);
			chat.Post = post;
			chat.DateAdded = DateTime.Now;
			chat.Save();
			var data = new { };
			return Json(data, JsonRequestBehavior.AllowGet);
		}

	}
	
}
