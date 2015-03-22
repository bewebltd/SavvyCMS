using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class PasswordLockController : ApplicationController {

		public ActionResult Index() {
			var data = new ViewModel();
			return View("PasswordLockView", data);
		}

		public static void CheckCookie() {// Uses PasswordLock controller
 			if (Web.FullUrl.ToLower().Contains("passwordlock")) return;

			//if (Security.IsAdministratorAccess) return;
			//if (Util.IsBewebOffice) return; //dont do that or we won't see the lock
			if (Util.GetSettingBool("LockSiteHomepage",false) && Web.Request.Cookies["pwlock"]==null) {
				Web.Redirect(Web.Root+"PasswordLock");
			}
		}

		public ActionResult Enter(string password) {
			if (password == Util.GetSetting("LockSitePassword") || Util.IsBewebOffice || Util.ServerIsDev) {
				Web.Response.Cookies.Add(new HttpCookie("pwlock", Crypto.Encrypt("aaahhhtiui!")));
				return Redirect(Web.Root);
			} else {
				var data = new ViewModel();
				data.message = "Incorrect password";
				return View("PasswordLockView", data);
			}
		}

		public class ViewModel : PageTemplateViewModel {
			public string message;
		}

	}
}