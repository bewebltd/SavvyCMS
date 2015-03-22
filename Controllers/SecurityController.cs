using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Beweb;
using Models;
using SavvyMVC.Helpers;
using Site.SiteCustom;
using Security = Beweb.Security;

namespace Site.Controllers {
	public class SecurityController : Controller {
		//
		// GET: /Security/
		public bool AutologinSkipLoginScreen = false; //set to false to show user login screen if they marked to remember their password
		public bool EnableMemberLogin = false; //set to true if users can log in, or false if admin section only
		public string MemberWelcomeUrl = "~/members/"; // you can change this to the default URL people go to after logging in - eg Members Section - this applies when browsing directly to /Security/Login
		public bool EnableSavvySingleSignOn = false;
		public string SSODomain = null;

		public ActionResult Login(string t, string u) {

			if (t.IsNotBlank() && u.IsNotBlank()) {

				bool isImpersonateOk = Crypto.CheckMinuteCypher(t, 1);

				if (!isImpersonateOk) {
					Web.ErrorMessage = "Impersonating failed";
					return Redirect(Web.Root);
				}

				int id = Crypto.DecryptID(u);
				Person p = Person.LoadByPersonID(id);
				if (p != null) {
					Web.Session.Add("Impersonating", true);

					//string passwordToken = "|" + "|" + "|" + "|";
					string password = Crypto.Decrypt(p.Password);

					return LoginSubmit(p.Email, password, true);
				} else {
					Web.ErrorMessage = "Impersonating failed";
					return Redirect(Web.Root);
				}
			}



			var data = new LoginFormViewData();
			TrackingBreadcrumb.Current.AddBreadcrumb(1, "Login");

			// save the lastUrl - for login redirect later
			//if (Request.UrlReferrer != null && !Request.UrlReferrer.AbsoluteUri.ContainsInsensitive("security/login")) {
			//	Session["LastUrl"] = Request.UrlReferrer.AbsoluteUri;
			//} else {
			//	Session["LastUrl"] = "~/admin/";
			//}
			if (!Request["ReturnUrl"].ContainsInsensitive("loginsubmit")) { // this will prevent login submit not found after first login fails(i.e. using the wrong username and password) JC 20140427
				Session["LastUrl"] = Request["ReturnUrl"];
			}

			if (Session["LastUrl"] + "" == "") {
				if (EnableMemberLogin) {
					Session["LastUrl"] = MemberWelcomeUrl; // you can change this to the default URL people go to after logging in - eg Members Section - this applies when browsing directly to login page
				} else {
					Session["LastUrl"] = Web.AdminRoot;  // by default, we go to admin menu after logging in
				}
			}

			// if logged in AND we have a ReturnUrl in the querystring, the person must not be authorised for that page - hopefully this assumption is always correct
			if (Security.IsLoggedIn && Request["ReturnUrl"].IsNotBlank()) {
				ModelState.AddModelError("Login", "Sorry, your user name doesn't have permission to access that area.");
			}

			// get the remembered values
			var s = new Beweb.Security();
			s.GetRemembered();
			data.Username = s.RememberedUser;
			data.ForgottenPasswordEmailAddress = s.RememberedUser;
			data.RememberPwd = s.IsRemembered;
			data.PCode = s.RememberedPassword;    // if cookied this will be an encrypted version
			if (s.IsRemembered && AutologinSkipLoginScreen && ModelState.Count == 0) {
				if (Request["logout"] == "1") {
					// user has just chosen to log out, so they will want to log in with a different user or at least not auto-login again
					Security.ClearSecurityCookies();
					data.Username = "";
					data.PCode = "";
					data.RememberPwd = false;
					Web.InfoMessage = "Your login details have been removed from this computer.";
				} else {
					return LoginSubmit(data.Username, data.PCode, true);
				}
			} else if (EnableSavvySingleSignOn && ModelState.Count == 0 && s.CheckSavvySingleSignOn()) {
				// single sign in Savvy Classic ASP to Savvy MVC .Net
				return LoginSubmit(s.RememberedUser, s.RememberedPassword, true);
			}

			return ReturnLoginView(data);
		}



		public ActionResult SSODotNet() {

			// to use this call:
			// sso_classic.asp?ssoDotNet=Utils%2fsso_dotnet.aspx&success=PartSale.aspx&fail=login.asp
			// params:
			// success: page to redirect to if cookie is there and GUID matches, i.e. successful sso. Remember to UrlEncode this value ESPECIALLY if it contains query string stuff
			// fail: fail page

			string AdminId = String.Empty;
			//if(Beweb.Util.ServerIs() == "DEV")
			//{
			//  // don't bother checking - auto log in for dev
			//  //AdminId = Request.QueryString["administratorId"]; // 161 for matt
			//  AdminId = Beweb.BewebData.GetValue(
			//    "SELECT AdministratorID FROM Administrator WHERE Email=@Email",
			//    new Parameter("Email", TypeCode.String, "matt"),
			//    BewebData.GetConnectionString("ExtranetConnectionString"));

			//}
			//else
			//{
			if (String.IsNullOrEmpty(Request.QueryString["sso"])) return RedirectFail();
			string ssoGuid = Request.QueryString["sso"];

			// output.Text += String.Format("<br /><br />[{0}]", ssoGuid); 

			// check the database for the ssoGuid - make sure it was just set (with some leeway)
			AdminId = BewebData.GetValue(
					"SELECT AdministratorId FROM Administrator WHERE SsoGuid=@SsoGuid AND SsoSetTime>DATEADD(minute, -2, GETDATE())",
					new Parameter("SsoGuid", TypeCode.String, ssoGuid),
					BewebData.GetConnectionString("ExtranetConnectionString"));
			//}

			if (String.IsNullOrEmpty(AdminId)) return RedirectFail();

			string securityRoles = BewebData.GetValue(new Sql("select role from Administrator WHERE AdministratorID=", AdminId.SqlizeNumber()), BewebData.GetConnectionString("ExtranetConnectionString"));
			string name = BewebData.GetValue(new Sql().AddRawSqlString("select firstname+' ' +lastname as name from Administrator").Add(" WHERE AdministratorID=", AdminId.SqlizeNumber()), BewebData.GetConnectionString("ExtranetConnectionString"));

			//FormsAuthentication.Initialize();
			//FormsAuthentication.HashPasswordForStoringInConfigFile(AdminId, "sha1");
			//// Create a new ticket used for authentication
			//FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
			//  1, // Ticket version
			//  AdminId, // Username to be associated with this ticket
			//  DateTime.Now, // Date/time issued
			//  DateTime.Now.AddMinutes(60), // Date/time for login to expire (web.config setting is ignored)
			//  true, // "true" for a persistent user cookie (could be a checkbox on form)
			//  securityRoles, // User-data (the roles from this user record in our database)
			//  FormsAuthentication.FormsCookiePath); // Path cookie is valid for

			//// Hash the cookie for transport over the wire
			//string hash = FormsAuthentication.Encrypt(ticket);
			//HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
			//Response.Cookies.Add(cookie);

			Security.SetSecurityCookies(name, false, AdminId);
			Security.CreateAuthCookie(securityRoles, AdminId);
			//UserSession.Person = Models.Person.LoadID();
			//var login = new Security().Login(AdminId);
			//Security.LoggedInUserID = AdminId
			//UserSession.Person = Models.Person.LoadID(AdminId.ToIntOrDie());

			return RedirectSuccess();
		}

		protected ActionResult RedirectSuccess() {
			return Redirect(Request.QueryString["success"]);
		}

		protected ActionResult RedirectFail() {
			return Redirect(Request.QueryString["fail"]);
		}



		private ActionResult ReturnLoginView(LoginFormViewData data) {
			// For Custom site member logins, you can return a different view here
			// note you also need to change the default URL above
			if ((Session["LastUrl"] + "").Contains("admin") || !EnableMemberLogin) {
				return View("Login", data);
			}

			return View("LoginMember", data);
		}

		public class LoginFormViewData : PageTemplateViewModel {
			public string Username;
			public string Message;
			public string ForgottenPasswordEmailAddress;
			public bool RememberPwd;
			public string PCode;

			public LoginFormViewData() {

				this.PageTitleTag = "Log-in";
			}
		}

		[HttpPost]
		public ActionResult LoginSubmit(string username, string pCode, bool? rememberPwd) {
			bool remember = rememberPwd ?? false;
			Beweb.Security s = new Beweb.Security();
			s.AllCurrentRoles = SecurityRoles.Roles;
			// Login user by other fields, eg FacebookID
			//var fbUserID = Request["FacebookID"];
			//if (fbUserID.IsNotBlank()) {
			//	var p = Person.LoadByFacebookUserID(fbUserID);
			//	if (p == null) {
			//		var data = new LoginFormViewData();
			//		Web.ErrorMessage = "Sorry, no family member found with the provided Facebook account. Please <a href=\""+Web.Root+"Family/Join\">Join the Family</a> first. ";
			//		return ReturnLoginView(data);
			//	}
			//	if (s.Login(p.Email, Crypto.Decrypt(p.Password), remember)) {
			//		return Redirect(s.RedirectUrl);
			//	}
			//}
			if (Web.Session["Impersonating"] == null) {
				RemoteTwitchLogin(s, username, ref pCode);
			} else {
				Web.Session.Remove("Impersonating");
			}
			if (s.Login(username, pCode, remember)) {
				// ok
				if (EnableSavvySingleSignOn) {
					s.SetSavvySingleSignOnCookie(username, SSODomain);
				}
				return Redirect(s.RedirectUrl);
			} else {
				// a problem
				ModelState.AddModelError("Login", s.ResultMessage);
				var data = new LoginFormViewData();
				data.Username = username;
				data.PCode = "";
				data.RememberPwd = remember;
				return ReturnLoginView(data);
			}
		}

		public void RemoteTwitchLogin(Security s, string username, ref string pCode) {
			if (username.ToLower().EndsWith("@beweb.co.nz")) {
				var remoteLogin = "Failed";
				var isRemoteLoginOnline = false;

				try {
					var twitchKey = Util.GetSetting("TwitchKey", "dsigbsd9uFSdsg897gasiu%%$#*gas79%*gakisfaf");
					remoteLogin = Http.Get("http://twitch.beweb.co.nz/Security/RemoteLogin?EncEmail=" + Crypto.Encrypt(username, twitchKey) + "&EncPassword=" + Crypto.Encrypt(pCode, twitchKey) + "&EncRemembered=" + Crypto.Encrypt(Crypto.Decrypt(pCode), twitchKey));
					isRemoteLoginOnline = true;
				} catch { }

				var localPerson = new ActiveRecord(Security.PersonTableName, Security.PersonTableName + "ID");
				var personExists = localPerson.LoadData(new Sql("where Email = ", username.SqlizeText()));

				// If twitch is online and rejects the user login, then setup to fail the login
				if (isRemoteLoginOnline && remoteLogin == "Failed") {
					pCode = "invalid user " + Crypto.Random();
					s.ResultMessage = "Invalid Twitch login";

					if (personExists) {
						localPerson["IsActive"].ValueObject = false;
						localPerson.Save();
					}
				}

				if (remoteLogin != "Failed") {
					if (!personExists) {
						localPerson["FirstName"].ValueObject = remoteLogin.Split("|")[0];
						localPerson["LastName"].ValueObject = remoteLogin.Split("|")[1] + "*";
						localPerson["Email"].ValueObject = username;
						localPerson["Role"].ValueObject = "administrators,superadmins,developers";
						localPerson["Password"].ValueObject = Security.CreateSecuredPassword(RandomPassword.Generate(5, 7));
						localPerson["IsActive"].ValueObject = true;
						localPerson.Save();

						s.ResultMessage = "Logged in via Twitch";
					} else {
						// log user in with existing account
						localPerson["IsActive"].ValueObject = true;
						localPerson.Save();
						s.ResultMessage = "Logged in via Twitch, using local person";
					}

					pCode = Security.DecryptPassword(localPerson["Password"].ToString());
				}
			}
		}

		/// <summary>
		/// Forgotten password. This presents a form for user to enter their email address.
		/// If password encryption mode allows it, a password reminder email is sent. Otherwise, a password reset email is sent.
		/// </summary>
		public ActionResult ForgottenPassword() {
			var data = new ForgotPasswordViewModel();
			TrackingBreadcrumb.Current.AddBreadcrumb(1, "Forgotten Password");
#if pages
			data.ContentPage = PageCache.GetByPageCode("ForgottenPassword");
			if (data.ContentPage == null) throw new Exception("Forgotten Password page not found");
#endif
			return View(data);
		}

		public class ForgotPasswordViewModel : PageTemplateViewModel {
		}

		[HttpPost]
		public ActionResult ForgotPassword(string forgottenPasswordEmailAddress, string returnurl) {

			//if there isnt a hidden on the page - in this case we always know the return url so dont need one
			if (returnurl == null || returnurl.IsBlank()) {
				returnurl = "Security/Login";
			}

			PersonList people = Models.PersonList.LoadByEmail(forgottenPasswordEmailAddress);
			if (people.Count > 1) {
				Web.ErrorMessage = "Sorry there are multiple users with this email address, therefore we are unable to automatically change your password, please contact the administrator.";
				return Redirect(Web.Root + "Security/Login?mode=norem");
			}

			Security s = new Security();
			if (s.IsPasswordReminderPossible) {
				s.SendPasswordReminder(forgottenPasswordEmailAddress, returnurl);
			} else {
				s.SendPasswordReset(forgottenPasswordEmailAddress, "security/ChangePassword");
			}
			//if (s.IsSuccess) {
			//	Web.InfoMessage = "The password reset email has been sent to your email. Please check your email and follow the link.";
			//} else {
			Web.InfoMessage = s.ResultMessage;
			//}
			return Redirect(Web.Root + "Security/Login");
		}

		/// <summary>
		/// Logout and return to login page
		/// </summary>
		public ActionResult Logout() {
			Beweb.Security s = new Beweb.Security();
			s.Logout();
			if (EnableSavvySingleSignOn) {
				s.ClearSavvySingleSignOnCookie(SSODomain);
			}
			Web.InfoMessage = "Logout complete.";
			string returnURL = Web.Request["returnURL"];
			return Redirect(Web.Root + "Security/Login?logout=1" + (returnURL != "" ? "&ReturnUrl=" + returnURL : ""));
		}

		/// <summary>
		/// Change password form
		/// </summary>
		// note: not sure if this requires [Authorize]?
		public ActionResult ChangePassword(string p) {
			var data = new ChangePasswordViewModel();
			TrackingBreadcrumb.Current.AddBreadcrumb(1, "Change Password");
			if (!String.IsNullOrEmpty(p)) {
				data.ChangePasswordMessage = "Please enter and confirm your new password then click 'Change Password'. We will then verify you, and then change your password immediately.";
			} else {
				data.ChangePasswordMessage = "";
			}
			return ShowChangePasswordForm(p, data);
		}

		private ActionResult ShowChangePasswordForm(string p, ChangePasswordViewModel data) {
			if (!String.IsNullOrEmpty(p)) {
				data.UserName = "";
				data.ShowOldPassword = false;
			} else {
				data.UserName = new Sql("select Email from Person where PersonID=", Security.LoggedInUserID).FetchString();
				data.ShowOldPassword = true;
				data.OldPassword = "";
			}

			data.NewPassword = "";
			data.ConfirmNewPassword = "";
			data.ShowForm = true;
			return View("ChangePassword", data);
		}

		public class ChangePasswordViewModel : PageTemplateViewModel {
			public bool ShowOldPassword;
			public string ChangePasswordMessage;
			public string NewPassword;
			public string ConfirmNewPassword;
			public string OldPassword;
			public bool ShowForm;
			public string UserName;
		}

		[HttpPost]
		public ActionResult ChangePasswordSubmit(string p, string newPassword, string confirmNewPassword, string oldPassword) {
			// public ActionResult ChangePasswordSubmit(string p, string newPassword, string confirmNewPassword, int personID, string oldPassword){
			var data = new ChangePasswordViewModel();
			string resetId = p;
			int personId = 0;
			// do we know their personId? - we won't for people that forgot their passwords.
			if (Security.IsLoggedIn) {
				personId = Security.LoggedInUserID;
			}

			var s = new Beweb.Security();
			s.ChangePassword(resetId, newPassword, confirmNewPassword, personId, oldPassword);
			data.ChangePasswordMessage = s.ResultMessage;
			Web.InfoMessage = data.ChangePasswordMessage;
			if (s.IsSuccess) {
				return Redirect(Web.Root + "security/login");
			} else {
				return ShowChangePasswordForm(p, data);
			}

		}
	}
}
