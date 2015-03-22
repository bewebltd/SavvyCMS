//#define pages
using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class FacebookAppController : ApplicationController {

		//
		// GET: /Home/
		public ActionResult Index() {

			var data = new ViewModel();
			if (Web.Request["signed_request"].IsBlank()) {

				if(Web.Session["Player"] != null) {
					if(data.IsOpenForPlaying) {
						return Redirect(Web.Root+"GamePlay");
					} else {
						return View("ComingSoon", data);
					}
				}

				Web.Session.Abandon();
				data.IsLiked = false;

			} else {

			  var jObject = FacebookBeweb.ExtractSignedRequestJson();
			  if(jObject == null) throw new ProgrammingErrorException("Invalid payload from Facebook, probably because the appid/app secret were not correct - check your app settings or database settings. Current Server: " + Util.ServerIs());
			  data.FacebookAuth = new FacebookAuth(jObject);
			  
				if (!data.FacebookAuth.YouLikeIt) {
					Web.Session.Abandon();
					data.IsLiked = false;
			    return View("LikeUs", data);
				} 
				
				if(!data.IsOpenForPlaying) {
					return View("ComingSoon", data);
				}

				data.IsLiked = true;

				if(Web.Session["Player"] != null) {
					return Redirect(Web.Root+"GamePlay");
				}

			} 

			return View("LikeUs", data);
		}

		public ActionResult CookieFix() {
			// Just try to read the session to write a cookie
			if(Web.Session["Player"] != null) { } 
			Web.Session["CookieSet"] = true;
			return View("CookieFix");
		}

		[HttpPost]
		public ActionResult Register() {
			// takes a fb user object as post parameters
			bool saved = false;

			// Create a Player record if not exists
			var player = Player.LoadByFacebookUserID(Request["id"]);
			if(player == null) {
				player = new Player();
				player.FacebookUserID = Request["id"];
				player.FirstName = Request["first_name"];
				player.LastName = Request["last_name"];
				player.FacebookUsername = Request["username"];
				player.FacebookLink = Request["link"];
				player.Save();
				saved = true;
			}

			Web.Session["Player"] = player;
			// CookieSet is a hack to get it working on Safari - AF20140515
			return Content("{\"status:\": \""+(saved?"saved":"loaded")+"\", \"cookieSet\": "+(Web.Session["CookieSet"] != null).ToString().ToLower()+"}");
		}

		[HttpPost]
		public ActionResult Subscribe() {
			
			var firstName = "";
			var lastName = "";
			var entryId = 0;

			if(Web.Session["Player"] != null) {
				var player = UserSession.LoadPlayer();
				firstName = player.FirstName;
				lastName = player.LastName;
				entryId = player.PlayerID;
			}
			
			if(MailChimp.Subscribe(Request["email"], firstName, lastName, entryId)) {
				return Content("OK");
			}
			
			return Content("ERROR");
		}

		[HttpPost]
		public ActionResult SaveEmail() {
			try {

				if(Web.Session["Player"] == null) {
					throw new Exception();
				}

				var player = UserSession.LoadPlayer();

				var isWinner = Request["isWinner"] == "true";
				var email = Request["email"];

				if(isWinner) {

					var prizeTypeID = Prize.LoadByWinnerPlayerID(player.PlayerID).PrizeTypeID;

					var tb = TextBlockCache.Get("WinningEmail", FileSystem.ReadTextFile("~/SiteCustom/email.html"), "Steps to claim your prize");
					var description = "";

					if(prizeTypeID == 3) { // Limited Edition Cider Glasses
						description = "Wild Side glasses don’t grow on trees. They’re found underground. And with the help of the farm dog, you dug one up. If you think it looks good now, wait until you pour cider into it - it’ll still look good. All you have to do is reply to this email and let us know what address to send it to. ";
					} else if(prizeTypeID == 4) { // $100 Restaurant Voucher
						description = "You found a $100 restaurant voucher - possibly the most delicious thing ever to grow in paddock soil. Please reply with your postal address so we can get your prize out to you. ";
					} else if(prizeTypeID == 5) { // Wild Side Bottle Opener
						description = "You just found a bottle opener in the Paddock of Prizes. Of course, you did have the help of master digger, sheep enthusiast, and current world fetch champion, the farm dog. Please reply to with your postal address so we can get your prize out to you.";
					}

					SendEMail.SimpleSendHTMLEmail(email, tb.Title, tb.BodyTextHtml.Replace("[firstname]", player.FirstName).Replace("[description]", description));
				}

				player.Email = email;
				player.EmailSentDate = DateTime.Now;
				player.Save();
				Web.Session["Player"] = player;
				return Content("OK");
			}
			catch { }

			return Content("ERROR");
		}

		public ActionResult TestEmail() {

			var prizeTypeID = 3;

			var tb = TextBlockCache.Get("WinningEmail", FileSystem.ReadTextFile("~/SiteCustom/email.html"), "Claim your prize");
			var description = "";

			if(prizeTypeID == 3) { // $100 Restaurant Voucher
				description = "You found a $100 restaurant voucher - possibly the most delicious thing ever to grow in paddock soil. Please reply with your postal address so we can get your prize out to you. ";
			} else if(prizeTypeID == 4) { // Limited Edition Cider Glasses
				description = "Wild Side glasses don’t grow on trees. They’re found underground. And with the help of the farm dog, you dug one up. If you think it looks good now, wait until you pour cider into it - it’ll still look good. All you have to do is reply to this email and let us know what address to send it to. ";
			} else if(prizeTypeID == 5) { // Wild Side Bottle Opener
				description = "You just found a bottle opener in the Paddock of Prizes. Of course, you did have the help of master digger, sheep enthusiast, and current world fetch champion, the farm dog. Please reply to with your postal address so we can get your prize out to you.";
			}

			SendEMail.SimpleSendHTMLEmail("andre@beweb.co.nz", tb.Title, tb.BodyTextHtml.Replace("[firstname]", "André").Replace("[description]", description));

			return Content("OK");
		}

		public class ViewModel : PageTemplateViewModel {
			public FacebookAuth FacebookAuth;
		} 		
		
		public ActionResult NotFound() {
			Response.StatusCode = 404;
			return Index();
		}

		public ActionResult FacebookScript(bool isAllowed) {
			// isAllowed = Model.IsLiked && Model.IsOpenForPlaying
			var data = new FacebookScriptViewModel();
			data.IsAllowed = isAllowed;
			return View("FacebookScript", data);
		}

		public class FacebookScriptViewModel {
			public bool IsAllowed;
		}
	}
}
