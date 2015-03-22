using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class CompetitionController : ApplicationController {
		
		public ActionResult Index(int id) {
			var data = new ViewModel();
			data.Comp = Competition.LoadID(id, Otherwise.NotFound);
			if (data.Comp.ExpiryDate != null && DateTime.Now > data.Comp.ExpiryDate) {
				data.IsExpired = true;
			} else {
				Security.RequirePublished(data.Comp);
			}
			data.AuthToken = CreateAuthToken();
			return View("CompetitionView", data);
		}

		[HttpPost]
		public ActionResult SubmitEntry(string auth, bool wantsToJoin) {
			if (!ValidateAuthToken(auth)) {
				return Json(new { kind = "display", text = "Sorry, your session has timed out. Please refresh the page and try again." });
			}
			
			var c = new CompetitionEntry();
			c.UpdateFromRequest();
			c.UserIPAddress = Security.UserIpV4Address();
			c.Save();

			string thanksGeneral = TextBlockCache.GetRich("Competition Thanks - General").BodyTextHtml;

			return Json(new { kind = "display", text = thanksGeneral });
		}

		private string CreateAuthToken() {
			var authToken = DateTime.Now.AddHours(4) + "%ziera";
			return Crypto.Encrypt(authToken);
		}

		private bool ValidateAuthToken(string postedAuthToken) {
			try {
				string authToken = Crypto.Decrypt(postedAuthToken);
				if (authToken != null && authToken.EndsWith("%ziera")) {
					var timestamp = authToken.RemoveSuffix("%ziera");
					var time = DateTime.Parse(timestamp);
					if (time > DateTime.Now) {
						return true;
					}
				}
			} catch (Exception) {  // eg date parsing error
				// ignore and return false
			}
			return false;
		}

		public class ViewModel : PageTemplateViewModel {
			public string AuthToken;
			public bool IsExpired;
			public Competition Comp;
			public Models.TextBlock FutureEmailsQuestion = TextBlockCache.GetPlain("Competition - FutureEmailsQuestion", "I agree to receiving future emails");
			public Models.TextBlock Terms = TextBlockCache.GetRich("Competition - Terms", null, "Terms and Conditions");
		} 

	}
}