using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Beweb;

namespace Beweb {
	/// <summary>
	/// Summary description for Security
	/// </summary>
	public class Security {
		public enum PasswordModes {
			Level1Unencrypted,
			Level2ReversibleEncryptionAndUnencrypted,
			Level3ReversibleEncryption,
			Level4HashedOneWay

		}

		public static PasswordModes PasswordMode = PasswordModes.Level2ReversibleEncryptionAndUnencrypted;

		///// <summary>
		///// store passwords in db as plain text, if false, use one way hashed password
		///// </summary>
		//public static bool UsePlainPassword  { get { return Util.GetSettingBool("Beweb_Security_UsePlainPassword", false); } } 
		///// <summary>
		///// use two way encrypted password, if true, ignores UsePlainPassword
		///// </summary>
		//public static bool UseEncryptedPassword  { get { return Util.GetSettingBool("Beweb_Security_UseEncryptedPassword", true); } }

		public bool IsSuccess { get; set; }
		public string ResultMessage { get; set; }
		public string RedirectUrl { get; set; }

		public int RemoteLoggedInUserID { get; set; }
		public bool IsRemembered { get; set; }
		public string RememberedUser { get; set; }
		public string RememberedPassword { get; set; }
		public string ConnectionString { get; set; }
		public string UserRoles { get; set; } // we set this once logged in, so you can check the roles manually before a redirect has happened (before the cookie is set) if you have to

		// breaking change note: Beweb_Security_PersonTableName was Beweb.Security.PersonTableName - you need to change this in webappsettings.config too
		public static string PersonTableName { get { return Util.GetSetting("Beweb_Security_PersonTableName", "Person"); } }

		public static string AuthProvider { get { return Util.GetSetting("Beweb_Security_AuthProvider", "ASPNETAUTH"); } }

		public static bool AuthProviderIsSavvy { get { return AuthProvider == "SAVVYAUTH"; } }
		public static bool AuthProviderIsASPNET { get { return AuthProvider == "ASPNETAUTH"; } }

		/// <summary>
		/// Returns true if user is logged in.
		/// </summary>
		public static bool IsLoggedIn {
			//20110311 old code get {return Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminAdministratorID"]!=null; }
			get {
				if (AuthProviderIsSavvy) {
					return UserDetails.GetCurrent() != null;
				} else if (AuthProviderIsASPNET) {
					return HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated;
				} else {
					throw new ProgrammingErrorException("Unspecified Beweb_Security_AuthProvider. You can use ASPNETAUTH or SAVVYAUTH.");
				}
			}
		}

		public static bool IsDevAccess {
			get { return IsInRole(SecurityRolesCore.Roles.DEVELOPER); }
		}
		public static bool IsAdministratorAccess {
			// MN: 20110406 - added superadmin - administrator should always include superadmin, sometimes this is not correctly set up in the database but this is always the intention, so I think it is fine to include at this level
			get { return IsInRole(SecurityRolesCore.Roles.ADMINISTRATOR) || IsInRole(SecurityRolesCore.Roles.SUPERADMIN); }
		}

		public static bool IsSuperAdminAccess {
			get { return IsInRole(SecurityRolesCore.Roles.SUPERADMIN); }
		}

		/// <summary>
		/// Return true if the user is in the specified role, or is a dev user.
		/// (Use IsInRoleOnly if dev users should not be automatically included)
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public static bool IsInRole(string role) {
			if (!IsLoggedIn) return false;
			return IsInRoleOnly(role) || IsInRoleOnly(SecurityRolesCore.Roles.DEVELOPER);
		}

		/// <summary>
		/// return true if passed user is in role (they might be in other roles too). Sightly badly named - the "Only" should not have been included
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		public static bool IsUserInRoleOnly(int userID, string role) {
			var sql = new Sql("select role from ", PersonTableName.SqlizeName(), " where ", (PersonTableName + "ID").SqlizeName(), "=", userID.SqlizeNumber());
			var roles = sql.FetchString();
			return IsUserInRoleOnly(roles, role);
		}
		/// <summary>
		/// return true if role is in the userRoles string (proper delimiting done so "admin" will not be found in "superadmin")
		/// </summary>
		/// <param name="userRoles">comma separated string of roles</param>
		/// <param name="role"></param>
		/// <returns></returns>
		public static bool IsUserInRoleOnly(string userRoles, string role) {
			return ("," + userRoles + ",").Contains("," + role + ",");
		}

		/// <summary>
		/// Return true if the current user is the specified role (no special case true for dev users - see IsInRole for that).
		/// Note that 'Only' in this context means 'only users who have explicitly been given that role', it just means Dev users do not automatically inherit all roles.
		/// (Use IsInRole if dev users should be automatically included)
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public static bool IsInRoleOnly(string role) {
			if (!IsLoggedIn) return false;
			if (AuthProviderIsSavvy) {
				return UserDetails.GetCurrent().Roles.ContainsCommaSeparated(role);
			} else if (AuthProviderIsASPNET) {
				return (HttpContext.Current.User.IsInRole(role));
			} else {
				throw new ProgrammingErrorException("Unspecified Beweb_Security_AuthProvider. You can use ASPNETAUTH or SAVVYAUTH.");
			}
		}

		/// <summary>
		/// Returns PersonID of currently logged in user or ZERO if not logged in.
		/// </summary>
		public static int LoggedInUserID {
			// old session code: 	get { return Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminAdministratorID"].ToString().ToInt(0); }
			get {
				if (!IsLoggedIn) return 0;
				if (AuthProviderIsSavvy) {
					return UserDetails.GetCurrent().ID;
				} else if (AuthProviderIsASPNET) {
					return (HttpContext.Current.User.Identity.Name.ToInt(0));
				} else {
					throw new ProgrammingErrorException("Unspecified Beweb_Security_AuthProvider. You can use ASPNETAUTH or SAVVYAUTH.");
				}
			}
		}

		public bool IsPasswordReminderPossible {
			get {
				return (PasswordMode == PasswordModes.Level1Unencrypted || PasswordMode == PasswordModes.Level2ReversibleEncryptionAndUnencrypted || PasswordMode == PasswordModes.Level3ReversibleEncryption);
			}
		}

		public SecurityRolesCore AllCurrentRoles { get; set; }

		#region SavvySingleSignOn
		/// <summary>
		/// Savvy Single Sign On is for integrating Savvy Classic ASP with Savvy .NET apps.
		/// In the login-submit.asp page you must drop a cookie, then CheckSavvySingleSignOn() will pick it up.
		/// This method checks for the cookie. If found it sets RememberedUser and RememberedPassword, ready for the caller to then call Login(RememberedUser, RememberedPassword).
		/// 
		/// In classic ASP login-submit.asp add this code:
		/// Response.AddHeader "Set-Cookie", "SavvySingleSignOn="&EncryptID(rs(0))&"|"&server.urlencode(rs("email"))&"|"&EncryptID(DatePart("y", date()) * 42)&"; path=/; HttpOnly" 
		/// or Response.AddHeader "Set-Cookie", "SavvySingleSignOn="&EncryptID(rs(0))&"|"&server.urlencode(rs("email"))&"|"&EncryptID(DatePart("y", date()) * 42)&"; path=/; domain=example.co.nz; HttpOnly" 
		/// </summary>
		public bool CheckSavvySingleSignOn() {
			// single sign in Savvy Classic ASP to Savvy MVC .Net
			DebugLog("CheckSavvySingleSignOn");
			var classicSignInCookie = Web.Request.Cookies["SavvySingleSignOn"];
			if (classicSignInCookie != null && classicSignInCookie.Value.IsNotBlank()) {
				DebugLog("CheckSavvySingleSignOn - cookie found");
				var values = classicSignInCookie.Value.Split('|');
				int userID = Crypto.DecryptIDClassic(values[0]);
				string email = Web.Server.UrlDecode(values[1]);
				int token = Crypto.DecryptIDClassic(values[2]);
				if (token / 42 == DateTime.Today.DayOfYear) {
					// set the remembered email and password public properties, which enables the caller to then call Login(RememberedUser, RememberedPassword)
					RememberedUser = email;
					RememberedPassword = CreateEncryptedPasswordToken(userID.ToString());
					DebugLog("CheckSavvySingleSignOn - OK");
					return true;
				} else {
					DebugLog("CheckSavvySingleSignOn - SSO Token has expired");
					//Web.WriteLine("SSO Token has expired.");
				}
			}
			return false;
		}

		/// <summary>
		/// You can use this if you need to drop a cookie for another Savvy .NET app to pick up. Currently classic ASP will not pick this up and auto-login, although it could.
		/// </summary>
		/// <param name="username"></param>
		public void SetSavvySingleSignOnCookie(string username) {
			SetSavvySingleSignOnCookie(username, null);
		}

		/// <summary>
		/// You can use this if you need to drop a cookie for another Savvy .NET app to pick up. Currently classic ASP will not pick this up and auto-login, although it could.
		/// </summary>
		/// <param name="username"></param>
		public void SetSavvySingleSignOnCookie(string username, string SSODomain) {
			string domainText = "";
			if (SSODomain != null) { domainText = "domain=" + SSODomain + ";"; }

			Web.Response.AddHeader("Set-Cookie", "SavvySingleSignOn=" + Crypto.EncryptIDClassic(LoggedInUserID) + "|" + Web.Server.UrlEncode(username) + "|" + Crypto.EncryptIDClassic(DateTime.Today.DayOfYear * 42) + ";" + domainText + " path=/; HttpOnly");
		}
		public void ClearSavvySingleSignOnCookie(string SSODomain) {
			var httpCookie = new HttpCookie("SavvySingleSignOn");
			httpCookie.Value = "";
			httpCookie.Domain = SSODomain;
			httpCookie.Expires = DateTime.Today.AddDays(-1);
			Web.Response.SetCookie(httpCookie);
		}
		#endregion

		#region Login
		public bool Login(string userName, string userEnteredPlainTextPassword) {
			return Login(userName, userEnteredPlainTextPassword, true);
		}

		public bool Login(ActiveRecord person) {
			if (PasswordMode == PasswordModes.Level4HashedOneWay) {
				throw new ProgrammingErrorException("Cannot use programmatic Login method with Level4HashedOneWay.");
			}
			string userEnteredPlainTextPassword = DecryptPassword(person["Password"].ToString());
			return Login(person["Email"].ToString(), userEnteredPlainTextPassword, true);
		}

		public bool Login(string userName, string userEnteredPlainTextPassword, bool isRememberMeTicked) {
			DebugLog("Security.Login userName[" + userName + "] userEnteredPlainTextPassword[" + userEnteredPlainTextPassword + "] isRememberMeTicked[" + isRememberMeTicked + "]");
			this.IsSuccess = false;
			HttpContext context = HttpContext.Current;
			bool isAllowedToTryLogin = false;
			string userIDText = "";
			string authenticatedPersonId = "";

			// try to decrypt the password (it might be a remembered value)
			string decpass = Crypto.Decrypt(userEnteredPlainTextPassword);
			string securedPassword = "";
			if (decpass == "") {
				// decrypt fails if the user just typed a plain password
				// so still allow them to continue
				isAllowedToTryLogin = true;
			} else {
				// decrypt returned a value
				string[] decryptedValues = decpass.Split(new Char[] { '|' });
				// decrypt looks like UserID|UserHostAddress|CreatedTime|UserAgent
				// verify the correct UserHostAddress is being used
				//if (decryptedValues.Length == 4 && decryptedValues[1] == UserIpV4Address() && CheckUserAgent(decryptedValues[3])) {   // version upgrades are annoying customers, do we need this UA check at all?
				//if (decryptedValues.Length == 4 && decryptedValues[1] == UserIpV4Address()) {   // IP address will change all the time on mobile browsers, this could be why twitch keeps forgetting my password

				// todo - test this puppy
				if (decryptedValues.Length == 4 && Numbers.isNumeric(decryptedValues[0]) && Dates.IsDate(decryptedValues[2])) {
					//				if (decryptedValues.Length == 4) {
					// get the password
					userIDText = decryptedValues[0];

					// is allowed if hashedPassword has a value
					if (String.IsNullOrEmpty(userIDText)) {
						ResultMessage = "There was a problem with your remembered password, please try again";
					} else {
						isAllowedToTryLogin = true;
					}
				} else {
					ResultMessage = "There was a problem with your remembered password, please try again.";
				}
			}

			DebugLog("Security.Login - isAllowedToTryLogin=" + isAllowedToTryLogin);
			if (isAllowedToTryLogin) {
				bool loginIsOk = false;
				//string securityRoles = ""; //this is now a property 20120601JN

				AddDefaultUsers();

				//if(!UseEncryptedPassword)
				//{
				//  if(UsePlainPassword)
				//  {
				//    if (String.IsNullOrEmpty(hashedPassword)) hashedPassword = password; //use the plain password
				//  }else
				//  {
				//    if (String.IsNullOrEmpty(hashedPassword)) hashedPassword = Crypto.CreateHash(password);
				//  }

				//}else
				//{
				//  if(hashedPassword.IsBlank()) //will not be blank if from cookie
				//  {
				//    hashedPassword = Crypto.Encrypt(password);
				//  }

				//}
				securedPassword = CreateSecuredPassword(userEnteredPlainTextPassword);
				//ParameterCollection sp = new ParameterCollection();
				//sp.Add("Username", userName);
				//sp.Add("Password", hashedPassword);
				var sql = new Sql("SELECT * FROM ", PersonTableName.SqlizeName(), " WHERE Email=", userName.Sqlize_Text(), " ");

				if (userIDText == "") {
					if (PasswordMode == PasswordModes.Level2ReversibleEncryptionAndUnencrypted) {//plain or hashed
						sql.Add("AND (Password=", securedPassword.Sqlize_Text(), " or Password=", userEnteredPlainTextPassword.Sqlize_Text(), ")");
					} else { //plain or encrypted, or hashed
						sql.Add("AND Password=", securedPassword.Sqlize_Text());
					}
				} else {
					sql.Add("AND " + PersonTableName + "ID=", userIDText.SqlizeNumber());
				}
				DebugLog("Security.Login - sql: " + sql.Value);

				var authenticatedPersonDetails = sql.GetHashtable();
				DebugLog("Security.Login - GetHashtable count: " + authenticatedPersonDetails.Count);

				if (authenticatedPersonDetails.Count > 0) {
					if (authenticatedPersonDetails.Contains("IsActive") && authenticatedPersonDetails["IsActive"].ToString().ToLower() == "false") {
						ResultMessage = "Your login is not active yet or your user account has been deactivated.";
					} else if (this.AllCurrentRoles != null && this.AllCurrentRoles.IsExludedLoginRole(authenticatedPersonDetails)) {
						ResultMessage = "Your login is does not have access.";
					} else {
						authenticatedPersonId = authenticatedPersonDetails[PersonTableName + "ID"].ToString();
						DebugLog("Security.Login - authenticatedPersonId: " + authenticatedPersonId);
						ParameterCollection pc = new ParameterCollection();
						pc.Add("LastLoginDate", TypeCode.DateTime, DateTime.Now.ToString());
						pc.Add("ID", TypeCode.String, authenticatedPersonId);
						//Web.Session[Beweb.Util.GetSiteCodeName() + "_UserLastLogin"] = Fmt.DateTime(authenticatedPersonDetails["LastLoginDate"]+"");
						BewebData.UpdateRecord(
							"UPDATE " + PersonTableName + " SET LastLoginDate=@LastLoginDate WHERE " + PersonTableName + "ID=@ID", pc,
							ConnectionString);

						UserRoles = authenticatedPersonDetails["Role"].ToString();
						RemoteLoggedInUserID = authenticatedPersonId.ToInt();
						// session stuff no longer needed

						//if (authenticatedPersonDetails["Role"].ToString() == SecurityRolesCore.Roles.DEVELOPER) {
						//	Web.Session["DevAccess"] = true;
						//}
						// see if the dev access box is ticked - this shouldn't be requried if roles are set up properly
						// securityRoles += (authenticatedPersonDetails["IsDevAccess"].ToString().ToLower() == "true") ? "," + SecurityRolesCore.Roles.DEVELOPER : "";

						// session vars are used in modification logging - 20140730 MN - do not comment out!!! 
						// AF20140827 For some reason on RinnaiIntranet Web.Session is null at this point and it's breaking the site
						if(Web.Session != null) {
							Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminFirstName"] = authenticatedPersonDetails["FirstName"];
							Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminLastName"] = authenticatedPersonDetails["LastName"];
							Web.Session[Beweb.Util.GetSiteCodeName() + "_AdminAdministratorID"] = authenticatedPersonId;							
						}

						loginIsOk = true;
					}
				}

				if (loginIsOk) {
					DebugLog("Security.Login - loginIsOk: " + authenticatedPersonId);

					//add a bunch of cookie values
					SetSecurityCookies(userName, isRememberMeTicked, authenticatedPersonId);

					if (AuthProviderIsSavvy) {
						//DebugLog("Security.Login - AuthProviderIsSavvy");
						var user = new UserDetails(authenticatedPersonDetails);
						user.SaveToCookie();
					} else if (AuthProviderIsASPNET) {
						CreateAuthCookie(UserRoles, authenticatedPersonId);
					} else {
						throw new ProgrammingErrorException("Unspecified Beweb_Security_AuthProvider. You can use ASPNETAUTH or SAVVYAUTH.");
					}

					string returnUrl = "";
					// Redirect to requested URL, or homepage if no previous page requested
					if (!context.Request["ReturnUrl"].ContainsInsensitive("loginSubmit")) {
						returnUrl = context.Request["ReturnUrl"];
					}

					if (returnUrl.IsBlank() && context.Session!=null && context.Session["LastUrl"] != null) returnUrl = context.Session["LastUrl"] + "";
					if (returnUrl == null || returnUrl.IsBlank() || returnUrl.Contains("logout")) returnUrl = Web.Root + "?mode=noslo"; //no session or try to get to logout
					// double check uer is allowed in admin
					//if (returnUrl.ToLower().Contains("/admin/") && !authenticatedPersonDetails["Role"].ToString().Contains(SecurityRolesCore.Roles.ADMINISTRATOR)) {
					//	returnUrl = "~/";
					//}


					// Don't call the FormsAuthentication.RedirectFromLoginPage since it could
					// replace the authentication ticket we just added...
					RedirectUrl = returnUrl;

					this.IsSuccess = true;
				} else {
					// error
					if (String.IsNullOrEmpty(ResultMessage)) {
						// standard error
						ResultMessage = "Either your username or password was incorrect. Passwords are case-sensitive.";
					}
				}
			}

			ParameterCollection pc1 = new ParameterCollection();

			if (this.IsSuccess) {
				pc1.Add("LastIpAddress", TypeCode.String, HttpContext.Current.Request.UserHostAddress);
				pc1.Add("ID", TypeCode.String, authenticatedPersonId);
				try {
					BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET LoginCount = ISNULL(LoginCount,0) + 1, LastIpAddress=@LastIpAddress, LastLoginDate=GETDATE() WHERE " + PersonTableName + "ID=@ID", pc1, ConnectionString);
				} catch (SqlException) {
					try {
						new Sql("alter table " + PersonTableName + " add column LoginCount int").Execute();
					} catch (SqlException) {
					}
					try {
						new Sql("alter table " + PersonTableName + " add column LastIpAddress nvarchar(50)").Execute();
					} catch (SqlException) {
					}
					try {
						new Sql("alter table " + PersonTableName + " add column LastLoginDate datetime").Execute();
					} catch (SqlException) {
					}
					try {
						new Sql("alter table " + PersonTableName + " add column FailedLoginCount int").Execute();
					} catch (SqlException) {
					}
				}
			} else {
				// unsuccesful login - clear out cookie info
				SetSecurityCookies("", false, "");

				// try and update the failed count based on the username - if it doesn't match anyone, ah well
				pc1.Add("Email", TypeCode.String, userName);
				try {
					BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET FailedLoginCount = ISNULL(FailedLoginCount,0) + 1 WHERE Email=@Email", pc1, ConnectionString);
				} catch (SqlException) {
					new Sql("alter table " + PersonTableName + " add column FailedLoginCount int").Execute();
				}
			}

			return this.IsSuccess;
		}

		public static bool Login(Guid ssoGuid) {
			string prepGuid = ssoGuid.ToString().Replace("}", "").Replace("{", "").ToLower();
			var sql = new Sql("select PersonID, Email, Role from Person where ssoguid=", prepGuid.Sqlize_Text(), "");

			var user = sql.GetHashtable();
			var authenticatedPersonId = user["PersonID"] + "";
			SetSecurityCookies(user["Email"] + "", false, authenticatedPersonId);

			CreateAuthCookie(user["Role"] + "", authenticatedPersonId);
			return true;
		}


		//if the admin table is person, create a new table and login
		private void AddDefaultUsers() {
			if (PersonTableName == "Person" && !BewebData.TableExists("Person")) {
				new Sql(@"
					CREATE TABLE Person ([PersonID] INTEGER IDENTITY(1,1), constraint pk_Person primary key (PersonID));
					ALTER TABLE Person ADD [Email] nvarchar (50);
					ALTER TABLE Person ADD [FailedLoginCount] int;
					ALTER TABLE Person ADD [FirstName] nvarchar (50);
					ALTER TABLE Person ADD [IsActive] bit;
					ALTER TABLE Person ADD [IsDevAccess] bit;
					ALTER TABLE Person ADD [LastIpAddress] nvarchar (50);
					ALTER TABLE Person ADD [LastLoginDate] datetime;
					ALTER TABLE Person ADD [DateAdded] datetime;
					ALTER TABLE Person ADD [LastName] nvarchar (50);
					ALTER TABLE Person ADD [LoginCount] int;
					ALTER TABLE Person ADD [Password] nvarchar (30);
					ALTER TABLE Person ADD [ResetCount] int;
					ALTER TABLE Person ADD [ResetDate] date;
					ALTER TABLE Person ADD [ResetId] nvarchar (50);
					ALTER TABLE Person ADD [Role] nvarchar (250);").Execute();
			}
			// MN 20140718 - no need for this as we now use twitch login
			//if (PersonTableName == "Person") {
			//	var sql = new Sql("SELECT count(*) FROM " + PersonTableName + " WHERE IsActive=1");
			//	if (sql.FetchIntOrZero() == 0) {
			//		// no users
			//		sql = new Sql();
			//		var ps1 = (Util.ServerIsDev) ? "mik" + "e1" + "23" : RandomPassword.Generate(3, 6);
			//		sql.AddRawSqlString("insert into Person (Email, Password, FirstName, LastName, [Role], IsActive) values ('mi'+'ke@bew'+'eb.co.nz', '" + CreateSecuredPassword(ps1) + "', 'Mi'+'ke', 'N'+'el'+'son', 'administrators,developers', 1)");
			//		Error.SendErrorEmail(Util.GetSiteName() + " - Default users: 1: " + ps1);
			//		Logging.trace("Create PW: " + ps1);
			//		sql.Execute();
			//		ps1 = (Util.ServerIsDev) ? "okl" + "ima" : RandomPassword.Generate(3, 6);
			//		sql.AddRawSqlString("insert into Person (Email, Password, FirstName, LastName, [Role], IsActive) values ('jer'+'emy@bew'+'eb.co.nz', '" + CreateSecuredPassword(ps1) + "', 'Jeremy', 'N'+'icholls', 'administrators,developers', 1)");
			//		Error.SendErrorEmail(Util.GetSiteName() + " - Default users: 2: " + ps1);
			//		Logging.trace("Create PW: " + ps1);
			//		sql.Execute();
			//	}
			//}
		}

		#endregion

		#region CookieStuff

		public static void CreateAuthCookie(string securityRoles, string personId) {
			FormsAuthentication.Initialize();
			FormsAuthentication.HashPasswordForStoringInConfigFile(personId, "sha1");
			// Create a new ticket used for authentication
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				1, // Ticket version
				personId, // Username to be associated with this ticket
				DateTime.Now, // Date/time issued
				DateTime.Now.AddMinutes(60), // Date/time for login to expire (web.config setting is ignored)
				true, // "true" for a persistent user cookie (could be a checkbox on form)
				securityRoles.Trim(new char[] { ',' }), // User-data (the roles from this user record in our database)
				FormsAuthentication.FormsCookiePath // Path cookie is valid for
			);

			// Hash the cookie for transport over the wire
			string hash = FormsAuthentication.Encrypt(ticket);
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
			cookie.HttpOnly = true;
			if (Util.GetSettingBool("Beweb_Security_AllowSubDomains", false)) {
				cookie.Domain = Util.GetPrimaryDomain();
			}
			HttpContext.Current.Response.Cookies.Add(cookie);

			// maybe add this?
			//HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(personId), securityRoles.Split(','));
		}

		public static void SetSecurityCookies(string userName, bool isRememberMeTicked, string authenticatedPersonId) {
			HttpContext context = HttpContext.Current;
			HttpCookie co;

			if (isRememberMeTicked) {
				// user name cookie
				co = new HttpCookie(Util.GetSiteCodeName() + "_User");
				co.Expires = DateTime.Now.AddYears(1);
				co.Value = userName;
				context.Response.Cookies.Add(co);


				co = new HttpCookie(Util.GetSiteCodeName() + "_UserID");
				co.Expires = DateTime.Now.AddYears(1);

				co.Value = (authenticatedPersonId != "") ? Crypto.EncryptID(authenticatedPersonId) : authenticatedPersonId;
				context.Response.Cookies.Add(co);
				// remember the password if RememberPwd is checked
				co = new HttpCookie(Util.GetSiteCodeName() + "_UserP");
				co.Expires = DateTime.Now.AddYears(1);
				co.Value = CreateEncryptedPasswordToken(authenticatedPersonId);
				context.Response.Cookies.Add(co);

				// remember me option
				co = new HttpCookie(Util.GetSiteCodeName() + "_UserRemPass");
				co.Expires = DateTime.Now.AddYears(1);
				co.Value = true.ToString();
				context.Response.Cookies.Add(co);
			} else {
				// clear out the cookies - note this will only happen when the browser is restarted
				Security.ClearSecurityCookies();
			}

		}

		private static string CreateEncryptedPasswordToken(string authenticatedPersonId) {
			HttpContext context = HttpContext.Current;
			return Crypto.Encrypt(String.Format("{0}|{1}|{2:dd-MMM-yyyy hh:mm:ss}|{3}", authenticatedPersonId, UserIpV4Address(), DateTime.Now, context.Request.UserAgent));
		}

		public static void ClearSecurityCookies() {
			HttpContext context = HttpContext.Current;
			HttpCookie co;

			// clear out the cookies
			co = new HttpCookie(Util.GetSiteCodeName() + "_User");
			co.Value = false.ToString();
			co.Expires = DateTime.Now.AddDays(-1);
			context.Response.Cookies.Add(co);
			co = new HttpCookie(Util.GetSiteCodeName() + "_UserP");
			co.Value = false.ToString();
			co.Expires = DateTime.Now.AddDays(-1);
			context.Response.Cookies.Add(co);

			// set remember me to false
			co = new HttpCookie(Util.GetSiteCodeName() + "_UserRemPass");
			co.Expires = DateTime.Now.AddYears(1);
			co.Value = false.ToString();
			context.Response.Cookies.Add(co);

		}

		public void GetRemembered() {
			HttpContext context = HttpContext.Current;
			// remember me values
			HttpCookie detailsCookie = context.Request.Cookies.Get(Util.GetSiteCodeName() + "_User");
			if (detailsCookie != null) {
				RememberedUser = detailsCookie.Value;
			}
			detailsCookie = context.Request.Cookies.Get(Util.GetSiteCodeName() + "_UserRemPass");
			if (detailsCookie != null) {
				IsRemembered = Convert.ToBoolean(detailsCookie.Value);
			}
			detailsCookie = context.Request.Cookies.Get(Util.GetSiteCodeName() + "_UserP");
			if (detailsCookie != null) {
				RememberedPassword = detailsCookie.Value;
			}
		}
		#endregion

		#region SendPasswordReset

		/// <summary>
		/// For old webforms projects
		/// </summary>
		/// <param name="emailAddress"></param>
#if MVC
		public void SendPasswordReset(string emailAddress) {
			SendPasswordReset(emailAddress, "security/ChangePassword");
		}
#else
		public void SendPasswordReset(string emailAddress) {
		  SendPasswordReset(emailAddress, "security/ChangePassword.aspx");
		}
#endif
		/// <summary>
		/// used by hased password model, so must be reset
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <param name="changePasswordURL"></param>
		public void SendPasswordReset(string emailAddress, string changePasswordURL) {
			IsSuccess = false;

			string retrievedPersonId = String.Empty;
			string retrievedUserName = String.Empty;
			Hashtable ud = BewebData.GetValues("SELECT TOP 1 " + PersonTableName + "ID, Email FROM " + PersonTableName + " WHERE Email=@Email", new Parameter("Email", TypeCode.String, emailAddress));
			if (ud.Count > 0) {
				retrievedPersonId = ud["" + PersonTableName + "ID"].ToString();
				retrievedUserName = ud["Email"].ToString();
			}

			if (String.IsNullOrEmpty(retrievedPersonId)) {
				// not sure which of these should be used...
				ResultMessage = "That email address could not be found in the system"; // this allows hackers to work out used email addresses
				//LoginMessage.Text = "Your reminder email has been sent"; // just say this anyway (even though it hasn't been sent)
			} else {
				string resetId = Guid.NewGuid().ToString().ToLower();
				// replace out characters that aren't 0-9 or a-f, they will create dodgy links in some email clients
				Regex r1 = new Regex(@"[^a-f0-9]", RegexOptions.Multiline);
				resetId = r1.Replace(resetId, "");

				Beweb.Email.PasswordReset(emailAddress, resetId, retrievedUserName, changePasswordURL);
				ResultMessage = "The password reset email has been sent";

				// add the resetId and the current time to the Person table
				ParameterCollection pc = new ParameterCollection();
				pc.Add("ResetId", TypeCode.String, resetId);
				pc.Add("ResetDate", TypeCode.DateTime, DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"));
				pc.Add("ID", TypeCode.Int32, retrievedPersonId);
				BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET ResetId=@ResetId, ResetDate=@ResetDate, ResetCount=ISNULL(ResetCount,0)+1 WHERE " + PersonTableName + "ID=@ID", pc);

				IsSuccess = true;
			}
		}

		/// <summary>
		/// Send a password reminder to the user with the given email address. 
		/// To specify a link back to the login page or the homepage, use the loginUrl parameter (this is appended to the website base URL so for example "security/login" or "" for homepage).
		/// </summary>
		public void SendPasswordReminder(string emailAddress, string loginUrl) {
			IsSuccess = false;

			string retrievedPersonId = String.Empty;
			string retrievedUserName = String.Empty;
			string retrievedPassword = String.Empty;
			Hashtable ud = BewebData.GetValues("SELECT TOP 1 " + PersonTableName + "ID, Email, Password FROM " + PersonTableName + " WHERE Email=@Email", new Parameter("Email", TypeCode.String, emailAddress));
			if (ud.Count > 0) {
				retrievedPersonId = ud["" + PersonTableName + "ID"].ToString();
				retrievedUserName = ud["Email"].ToString();
				retrievedPassword = ud["Password"].ToString();
			}

			if (String.IsNullOrEmpty(retrievedPersonId)) {
				// not sure which of these should be used...
				ResultMessage = "That email address could not be found in the system."; // this allows hackers to work out used email addresses
				//LoginMessage.Text = "Your reminder email has been sent"; // just say this anyway (even though it hasn't been sent)
			} else if (String.IsNullOrEmpty(retrievedPersonId)) {
				ResultMessage = "Sorry, the password reminder service is not available. Please contact us. [bws1]";
			} else {
				string clearTextPassword = Security.DecryptPassword(retrievedPassword);
				if (clearTextPassword.IsNotBlank()) {
					Beweb.Email.PasswordReminder(emailAddress, clearTextPassword, retrievedUserName, loginUrl);
					//Beweb.Email.PasswordReset(emailAddress, clearTextPassword, retrievedUserName, loginUrl);
					ResultMessage = "The password reminder email has been sent.";
					IsSuccess = true;
				} else {
					ResultMessage = "Sorry, the password reminder service is not available. Please contact us. [bws2]";
				}
			}
		}

		#endregion

		#region ChangePassword

		/// <summary>
		/// used by plain/encrypted password security model (cannot be used for hashed passwords)
		/// </summary>
		/// <param name="resetId"></param>
		/// <param name="newPassword"></param>
		/// <param name="confirmNewPassword"></param>
		/// <param name="currenPersonId"></param>
		/// <param name="oldPassword"></param>
		public void ChangePassword(string resetId, string newPassword, string confirmNewPassword, int currenPersonId, string oldPassword) {
			IsSuccess = false;
			int thisUserId = currenPersonId;
			if (String.IsNullOrEmpty(resetId)) {
				// normal change password - user has navigated here themselves

				// make sure new and confirm are the same
				if (newPassword != confirmNewPassword) {
					ResultMessage = "We're sorry, but your new password does not match the confirmation, please try again.";
					return;
				}

				//string decryptedPS = "";
				//string dbPassword = GetHashedPassword(thisUserId.ToString());
				//decryptedPS = GetComparablePasswordText(oldPassword);
				//if (decryptedPS != dbPassword)
				string dbPassword = GetStoredPasswordFromDatabase(thisUserId);
				if (!IsPasswordCorrect(oldPassword, dbPassword)) {
					ResultMessage = "We're sorry, but your old password was wrong - please try again.";
					return;
				}
			} else {
				// user has forgotten their password - check the resetId and time requested
				ParameterCollection pc1 = new ParameterCollection();
				pc1.Add("ResetId", TypeCode.String, resetId);
				pc1.Add("ResetDate", TypeCode.DateTime, DateTime.Now.AddDays(-1).ToString("dd MMM yyyy HH:mm:ss"));

				// check the reset code
				string checkResetCode =
					BewebData.GetValue("SELECT " + PersonTableName + "ID FROM " + PersonTableName + " WHERE ResetId=@ResetId AND ResetDate > @ResetDate", pc1);
				if (String.IsNullOrEmpty(checkResetCode)) {
					ResultMessage = "We're sorry, but the password reset link you clicked has already been used. If this is definitely the first time you've clicked the link, try copying it into your browser address bar instead of clicking it. You can also get a new reset link emailed to you.";
					return;
				}

				// save the personId - we won't be able to get it after the next step
				thisUserId = Convert.ToInt32(checkResetCode);

				// update the person record - wipe the resetId so it can't be used again
				BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET ResetId=NULL, ResetDate=NULL WHERE ResetId=@ResetId AND ResetDate > @ResetDate", pc1);

			}

			// change the password if we get to here

			ParameterCollection pc = new ParameterCollection();
			pc.Add("Password", TypeCode.String, CreateSecuredPassword(newPassword));
			pc.Add("ID", TypeCode.Int32, thisUserId.ToString());

			int updated = BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET Password=@Password WHERE " + PersonTableName + "ID=@ID", pc);

			if (updated == 1) {
				ResultMessage = "Thank you. Your password has been changed successfully.";
				IsSuccess = true;
				// clear remember password cookie data
				HttpContext.Current.Request.Cookies.Remove(Util.GetSiteCodeName() + "_UserP");
			} else {
				// left the page too long and the session timed out?
				ResultMessage = "We're sorry, but there was a problem changing your password - please refresh the page and try again.";
			}
		}

		public static string CreateSecuredPassword(string plainTextPassword) {
			string result = "";
			//if(!UseEncryptedPassword)?((UsePlainPassword)?newPassword:Crypto.CreateHash(newPassword)):Crypto.Encrypt(newPassword)
			if (PasswordMode == PasswordModes.Level1Unencrypted) {
				result = plainTextPassword;
			} else if (PasswordMode == PasswordModes.Level2ReversibleEncryptionAndUnencrypted || PasswordMode == PasswordModes.Level3ReversibleEncryption) {
				result = Crypto.Encrypt(plainTextPassword); //by default, new people get encrypted in mixed mode (Level2ReversibleEncryptionAndUnencrypted)
			} else if (PasswordMode == PasswordModes.Level4HashedOneWay) {
				result = Crypto.CreateHash(plainTextPassword); //one way
			}
			return result;
		}

		private static bool IsPasswordCorrect(string userEnteredPassword, string dbPassword) {
			bool result = false;
			// check old password


			// in mixed mode, we compare to either the plain value, or the secured value (encrypted), so cannot use random encryption
			if (PasswordMode == PasswordModes.Level2ReversibleEncryptionAndUnencrypted && userEnteredPassword == dbPassword) {
				result = true;
			} else {
				string securedPassword = CreateSecuredPassword(userEnteredPassword);
				result = (dbPassword == securedPassword);
			}

			return result;
		}

		//private bool IsPasswordCorrect(string userEnteredPassword, string dbPassword)
		//{
		//  bool result = false;
		//  // check old password

		//  if(PasswordMode==PasswordModes.Level4HashedOneWay)
		//  {
		//    string securedPassword = CreateSecuredPassword(userEnteredPassword);
		//    result = (dbPassword == securedPassword);
		//  }else
		//  {
		//    string decryptedPassword = Crypto.Decrypt();
		//  }

		//  return result;
		//}

		/// <summary>
		/// Given a secured password, returns the clear text password.
		/// (This is not possible for Level4HashedOneWay - this will return empty string)
		/// </summary>
		public static string DecryptPassword(string securedPassword) {
			string decryptedPS = "";
			// check old password

			string decryptedVersionOfPassword = Crypto.Decrypt(securedPassword);
			if (PasswordMode == PasswordModes.Level3ReversibleEncryption) {
				decryptedPS = decryptedVersionOfPassword;
			} else {
				if (PasswordMode == PasswordModes.Level3ReversibleEncryption || PasswordMode == PasswordModes.Level2ReversibleEncryptionAndUnencrypted) {
					decryptedPS = securedPassword;
					if (decryptedVersionOfPassword != "") {			  //could not be decryted, was already in plain text
						decryptedPS = decryptedVersionOfPassword;
					}
				} else if (PasswordMode == PasswordModes.Level4HashedOneWay) {
					//throw new ProgrammingErrorException("hashed password cannot be decrypted");
					//decryptedPS = Crypto.CreateHash(securedPassword);
					decryptedPS = "";
				} else {
					throw new ProgrammingErrorException("unknown enc meth");
				}

			}
			return decryptedPS;
		}

		#endregion

		#region Utilities

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId">this is a string because it's just easier</param>
		/// <returns></returns>
		public static string ResolveUserName(string userId) {
			if (String.IsNullOrEmpty(userId)) return "";
			return BewebData.GetValue("SELECT Email FROM " + PersonTableName + " WHERE " + PersonTableName + "ID=@UserId", new Parameter("UserId", TypeCode.Int32, userId));
		}
		public static string ResolveUserName(object obj) {
			if (obj == null) return "";
			return ResolveUserName(obj.ToString());
		}

		// use LoggedInUserID		
		//		public static int GetLoggedinUserID() {
		//			
		//			return HttpContext.Current.User.Identity.Name.ToInt();
		//		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static string GetHashedPassword(string userId) {
			if (String.IsNullOrEmpty(userId)) return "";
			return GetStoredPasswordFromDatabase(userId.ToIntOrDie());
		}

		/// <summary>
		/// Returns the encrypted password, exactly as it is stored in the database.
		/// To decrypt it, call DecryptPassword afterwards (which does not work for Level4HashedOneWay).
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static string GetStoredPasswordFromDatabase(int userId) {
			var shortPersonTableName = PersonTableName;
			if (shortPersonTableName.Contains(".")) shortPersonTableName = shortPersonTableName.Substring(shortPersonTableName.LastIndexOf(".") + 1);
			return BewebData.GetValue("SELECT Password FROM " + PersonTableName + " WHERE " + shortPersonTableName + "ID=@UserId", new Parameter("UserId", TypeCode.Int32, userId + ""));
		}

		public static void CreateInitialUser(string username, string password) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add("Email", TypeCode.String, username);
			pc.Add("Password", TypeCode.String, CreateSecuredPassword(password));
			pc.Add("FirstName", TypeCode.String, "initial");
			pc.Add("LastName", TypeCode.String, "user");
			pc.Add("IsActive", TypeCode.Boolean, true.ToString());
			pc.Add("Role", TypeCode.String, String.Format("{0},{1}", SecurityRolesCore.Roles.ADMINISTRATOR, SecurityRolesCore.Roles.DEVELOPER)); // this is enough to get into admin area and update with Person Edit page

			BewebData.InsertRecord("INSERT INTO " + PersonTableName + " (Email, Password, FirstName, LastName, IsActive, Role, DateAdded) VALUES (@Email, @Password, @FirstName, @LastName, @IsActive, @Role, GETDATE())", pc);
		}

		public static int ConvertAllPlainTextPasswordsToHashed() {
			int i = 0;
			var sdr = new Sql("SELECT * FROM " + PersonTableName).GetReader();
			if (sdr != null) {
				foreach (DbDataRecord dr in sdr) {
					string password = dr["Password"].ToString();
					if (password.Length < 32) // don't do 32 character passwords (already hashed?!) ** careful with this!
					//					if (password.Length != 32) // don't do 32 character passwords (already hashed?!) ** careful with this!
					{
						ParameterCollection pc = new ParameterCollection();
						pc.Add("Password", TypeCode.String, Security.CreateSecuredPassword(password));
						pc.Add("Id", TypeCode.String, dr[PersonTableName + "Id"].ToString());
						BewebData.UpdateRecord("UPDATE " + PersonTableName + " SET Password=@Password WHERE " + PersonTableName + "Id=@Id", pc);
						i++;
					}
				}
				sdr.Close();
				sdr.Dispose();
			}
			return i;
		}

		public static string UserIpV4Address() {
			string IP4Address = String.Empty;

			if (HttpContext.Current.Request.UserHostAddress != null) {
				foreach (System.Net.IPAddress IPA in System.Net.Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress)) {
					if (IPA.AddressFamily.ToString() == "InterNetwork") {
						IP4Address = IPA.ToString();
						break;
					}
				}
			}
			if (IP4Address != String.Empty) {
				return IP4Address;
			}
			System.Net.IPAddress[] hostAddresses = null;
			try {
				hostAddresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
			} catch (SocketException se) {

				throw;
			}
			if (hostAddresses != null) {
				foreach (System.Net.IPAddress IPA in hostAddresses) {
					if (IPA.AddressFamily.ToString() == "InterNetwork") {
						IP4Address = IPA.ToString();
						break;
					}
				}
			}
			return IP4Address;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userAgentToCheckAgainstCurrent"></param>
		/// <returns></returns>
		public static bool CheckUserAgent(string userAgentToCheckAgainstCurrent) {
			string simplifiedCurrent = HttpContext.Current.Request.UserAgent + ""; // blank UA's are allowed
			string simplifiedUserAgentToCheck = userAgentToCheckAgainstCurrent;

			// chrome constantly changes version - ignore minor version changes
			Regex chromeRegex = new Regex(@"^(.*AppleWebKit/[0-9.]+ .* Chrome/([0-9]+\.){1,2})([0-9.]+)(.*)$"); // Chrome/10.0.648.133 <-- clear last two minors
			if (chromeRegex.Replace(simplifiedCurrent, "$1$4") == chromeRegex.Replace(simplifiedUserAgentToCheck, "$1$4")) {
				return true;
			}

			// add other browsers here

			return userAgentToCheckAgainstCurrent == HttpContext.Current.Request.UserAgent;
		}

		#endregion

		/// <summary>
		/// Logs the user out. Calls FormsAuthentication.SignOut and Session.Abandon.
		/// To be aware of: at this point we are still authenticated
		/// User.Identity.IsAuthenticated = true
		/// we need to complete the postback operation, and we will be unauthenticated next request
		/// see this for more info: http://forums.asp.net/p/1402139/3037547.aspx#3037547
		/// So you need to redirect after this.
		/// </summary>
		public void Logout() {
			UserDetails.Logout();
			FormsAuthentication.SignOut();
			if (Util.GetSettingBool("Beweb_Security_AllowSubDomains", false)) {
				HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
				cookie.HttpOnly = true;
				cookie.Domain = Util.GetPrimaryDomain();
				cookie.Expires = DateTime.Today.AddYears(-10);
				HttpContext.Current.Response.Cookies.Add(cookie);
			}
			Web.Session.Abandon();
		}
		/// <summary>
		/// call this from a place that is logged in - eg adminmenu controller. It resets the session var that checks to see if we are in a login redirect loop
		/// </summary>
		public static void ResetLoginLoopChecker() {
			Web.Session["LoginAlreadyTried"] = null; //reset login loop check
		}

		/// <summary>
		/// Returns true if user is in any of the supplied roles, given a comma delimited string of allowed roles.
		/// (Note in this case Dev users do not automatically inherit all roles)
		/// </summary>
		/// <param name="allowedRoles"></param>
		/// <returns></returns>
		public static bool IsInAnyOfTheseRoles(string allowedRoles) {
			var roles = allowedRoles.Split(',');
			foreach (var role in roles) {
				if (Security.IsInRoleOnly(role.Trim())) {
					return true;
				}
			}
			return false;
		}

		public static void RequireLogin(string allowedRoles) {
			if (!IsLoggedIn) {
				KickOut("Please log in to view this page.");
			}
			if (!IsInAnyOfTheseRoles(allowedRoles)) {
				KickOut("Your account does not have permission to view this page. This page requires the " + allowedRoles + " role. You must log in under another account to view this page.");
			}
		}

		public static void RequireAdminLogin() {
			if (!IsLoggedIn) {
				KickOut("Please log in to view this page.");
			}
			if (!Security.IsAdministratorAccess) {
				KickOut("Your account does not have permission to view this page. Please log in as an Administrator to view this page.");
			}
		}

		/// <summary>
		/// If the user is not logged in, throws HTTP Unauthorised exception, which takes the user to the login page with a ReturnUrl param set to the current URL.
		/// </summary>
		public static void RequireLogin() {
			if (!Security.IsLoggedIn) {
				KickOut("Please log in to view this page.");
			}
		}

		/// <summary>
		/// Checks the supplied record is published, by calling GetIsActive(), and returns 404 if not. Allows an authenticated admin user to view preview links (if Request["preview"] == "adminonly") even when not active.
		/// </summary>
		/// <param name="record">an active record model</param>
		public static void RequirePublished(ActiveRecord record) {
			if (Web.Request["preview"] == "adminonly") {
				// force login if not already
				if (!Security.IsAdministratorAccess) {
					KickOut("This page is unpublished. Please log in to preview this page.");
				}
			} else if (!record.GetIsActive()) {
				throw new Beweb.BadUrlException("Sorry, this page is not available.");
			}
		}

		/// <summary>
		/// Takes the user to the login page with a ReturnUrl param set to the current URL.
		/// It is good practise to first set Web.InfoMessage to tell the user why they were kicked out.
		/// </summary>
		public static void KickOut(string friendlyMessage) {
			KickOut(friendlyMessage, false);
		}

		/// <summary>
		/// Takes the user to the login page with a ReturnUrl param set to the current URL.
		/// It is good practise to first set Web.InfoMessage to tell the user why they were kicked out.
		/// </summary>
		public static void KickOut(string friendlyMessage, bool kickToHomePage) {
			if (friendlyMessage.IsNotBlank()) {
				//Web.Response.ClearHeaders();
				Web.InfoMessage = friendlyMessage;
			}

			Web.Response.CacheControl = "no-cache";

			if(kickToHomePage) {
				Web.Redirect("~/");
				return;
			} else {
				var loginPage = Util.GetSetting("Beweb_Security_AdminLoginUrl", FormsAuthentication.LoginUrl ?? "~/security/Login");
				Web.Redirect(loginPage + "?ReturnUrl="+Web.FullRawUrl.UrlEncode());
				return;
				//FormsAuthentication.RedirectToLoginPage();
			}

			Web.End();
			//HttpContext.Current.Response.Redirect("~/security/login?ReturnUrl=" + Web.FullRawUrl.UrlEncode());
			//throw new HttpException(401, "Unauthorized access");
		}




		public static PasswordMeter CheckStrength(string passwd) {
			var intScore = 0;
			var suggestions = "";
			var strColor = "";
			var strVerdict = "";
			var strLog = "";
			/*
		.password_field {  }
		.password_meter { width: 100px; height: 19px; position: relative; display: block; float: left; border: solid 1px #dddddd; border-left: none; font-family: Arial; font-size: 12px; }
		.password_meter DIV { border-left: solid 1px #dddddd; width: 24px; height: 19px; float: left; }
		.password_meter .strength { display: block; position: absolute; top: 2px; left: 0; width: 100px; text-align: center; }
		.very_weak, .very_weak .first { background: red; }
		.weak, .weak .first, .weak .second { background: orange; }
		.acceptable, .acceptable .first, .acceptable .second, .acceptable .third { background: yellow; }
		.strong, .strong .first, .strong .second, .strong .third, .strong .fourth { background: #c1d72f; }

			 */

			// PASSWORD LENGTH
			if (passwd.Length < 5)                         // length 4 or less
			{
				intScore = (intScore + 3);
				strLog = strLog + "3 points for length (" + passwd.Length + ")\n";
				suggestions += "Make password longer\n";
			} else if (passwd.Length < 8) // length between 5 and 7
			{
				intScore = (intScore + 6);
				strLog = strLog + "6 points for length (" + passwd.Length + ")\n";
				suggestions += "Make password longer\n";
			} else if (passwd.Length < 16)// length between 8 and 15
			{
				intScore = (intScore + 12);
				strLog = strLog + "12 points for length (" + passwd.Length + ")\n";
			} else                   // length 16 or more
			{
				intScore = (intScore + 30);
				strLog = strLog + "18 point for length (" + passwd.Length + ")\n";
				//suggestions+="Make password shorter\n"
			}
			//new reg

			// LETTERS (Not exactly implemented as dictacted above because of my limited understanding of Regex)
			if (Regex.Match(passwd, "[a-z]").Success)                              // [verified] at least one lower case letter
			{
				intScore = (intScore + 1);
				strLog = strLog + "1 point for at least one lower case char\n";
			} else {
				suggestions += "Add a lower case character\n";
			}

			if (Regex.Match(passwd, "[A-Z]").Success)                              // [verified] at least one upper case letter
			{
				intScore = (intScore + 5);
				strLog = strLog + "5 points for at least one upper case char\n";
			} else {
				suggestions += "Add an upper case character\n";
			}

			// NUMBERS
			if (Regex.Match(passwd, ".*[0-9].*").Success)                                // [verified] at least one number
			{
				intScore = (intScore + 5);
				strLog = strLog + "5 points for at least one number\n";
				if (Regex.Match(passwd, "(.*[0-9].*[0-9].*[0-9])").Success)             // [verified] at least three numbers
				{
					intScore = (intScore + 5);
					strLog = strLog + "5 points for at least three numbers\n";
				} else {
					suggestions += "Add three or more numbers\n";
				}

			} else {
				suggestions += "Add at least one number\n";
			}


			// SPECIAL CHAR
			if (Regex.Match(passwd, ".[!,@,#,$,%,^,&,*,?,_,~]").Success)            // [verified] at least one special character
			{
				intScore = (intScore + 5);
				strLog = strLog + "5 points for at least one special char\n";

				// [verified] at least two special characters
				if (Regex.Match(passwd, "(.*[!,@,#,$,%,^,&,*,?,_,~].*[!,@,#,$,%,^,&,*,?,_,~])").Success) {
					intScore = (intScore + 5);
					strLog = strLog + "5 points for at least two special chars\n";
				} else {
					suggestions += "Add at more than 2 special characters : !,@,#,$,%,^,&,*,?,_,~\n";
				}

			} else {
				suggestions += "Add at least one special character : !,@,#,$,%,^,&,*,?,_,~\n";
			}

			// COMBOS
			if (Regex.Match(passwd, "([a-z].*[A-Z])|([A-Z].*[a-z])").Success)        // [verified] both upper and lower case
			{
				intScore = (intScore + 2);
				strLog = strLog + "2 combo points for upper and lower letters\n";
			} else {
				suggestions += "Use upper and lower case together \n";
			}

			if (Regex.Match(passwd, "([a-zA-Z])").Success && Regex.Match(passwd, "([0-9])").Success) // [verified] both letters and numbers
			{
				intScore = (intScore + 2);
				strLog = strLog + "2 combo points for letters and numbers\n";
			} else {
				suggestions += "Use letters and numbers together \n";
			}

			// [verified] letters, numbers, and special characters
			if (Regex.Match(passwd, "([a-zA-Z0-9].*[!,@,#,$,%,^,&,*,?,_,~])|([!,@,#,$,%,^,&,*,?,_,~].*[a-zA-Z0-9])").Success) {
				intScore = (intScore + 2);
				strLog = strLog + "2 combo points for letters, numbers and special chars\n";
			} else {
				suggestions += "Use letters, numbers and special characters together \n";
			}
			if (commonPasswords.Replace(" ", "").Replace("\t", "").Replace("\r", "").Split('\n').ContainsInsensitive(passwd)) {
				intScore = (intScore - 15);
				strLog = strLog + "subtract 15 points for using a common password\n";
				suggestions += "Dont use any of the world's most common passwords \n";
			}

			//var strVerdict="";
			if (intScore < 16) {
				strVerdict = "very weak";
				strColor = "red";
			} else if (intScore < 25) {
				strVerdict = "weak";
				strColor = "orange";
			} else if (intScore < 35) {
				strVerdict = "acceptable";
				//strColor = "yellow";
				strColor = "#A7DDAA";

			} else {
				strVerdict = "strong";
				strColor = "#327E36";
			}
			//	else if (intScore > 34 && intScore < 45) {
			//		strVerdict = "strong"
			//	}
			//	else {
			//		strVerdict = "stronger"
			//	}

			//document.forms.passwordForm.score.value = (intScore)
			//document.forms.passwordForm.verdict.value = (strVerdict)
			//document.forms.passwordForm.matchlog.value = (strLog)
			var ps = new PasswordMeter();
			ps.verdict = strVerdict;
			ps.points = strLog;
			if (suggestions + "" != "") {
				suggestions = "Suggestions to improve password strength:\n" + suggestions + "\n\n";
			}
			ps.reason = suggestions + "Points summary:\n" + strLog;
			ps.suggest = suggestions;

			ps.color = strColor;

			return ps;
		}
		public const string commonPasswords = @" password  
123456
12345678
abc123
qwerty
monkey
letmein
dragon
111111
baseball
iloveyou
trustno1
1234567
sunshine
master
123123
welcome
shadow
ashley
football
jesus
michael
ninja
mustang
password1
password
123456
12345678
1234
qwerty
12345
dragon
pussy
baseball
football
letmein
monkey
696969
abc123
mustang
michael
shadow
master
jennifer
111111
2000
jordan
superman
harley
1234567
password
123456
12345678
abc123
qwerty
monkey
letmein
dragon
111111
baseball
iloveyou
trustno1
1234567
sunshine
master
123123
welcome
shadow
ashley
football
jesus
michael
ninja
mustang
password1
123456
password
123456789
welcome
ninja
abc123
qwerty
12345678
princess
sunshine
iloveyou
welcome
jesus
babygirl
12345
rockyou
Nichole
Daniel
money
monkey
freedom
654321
michael
1234567
love
master
ginger
11111
1234
dragon
batman
baseball
buster
starwars
dallas
summer
access
killer
mustang
2000
soccer
ranger
696969
tigger
pass
shadow
Jennifer
letmein
Joshua
merlin
Robert
hockey
666666
orange
jordan
trustno1
superman
computer
123123
thunder
internet
lifehack
0
gizmodo
whatever
cheese
nintendo
f You
blahblah
passwOrd
gawker
Password
pokemon
michelle
pepper
kotaku
F#ck
P#ssy
6969
1111
a##hole
golfer
austin
biteme
cowboy
silver
F#cker
bigdog
bl#wjob
yellow
131313
hello
please
scooter
dick
iwantu
sexy
panties
hammer
yankees";

		public static void DebugLog(string message) {
			if (false) {
				Logging.dlog(message + " - User:" + Security.LoggedInUserID + " IP:" + Web.UserIpAddress);
			}
		}

		/// <summary>
		/// PersonID	Email	Password	FirstName	LastName	lastlogin	logincount	DealerID	LastIpAddress	LastLoginDate	IsActive	FailedLoginCount	IsDevAccess	SsoGuid	SsoSetTime	Role	ForecastAccess	ForecastAdmin	DealerName	AccountNumber	AccountNumberMC	id	email	firstname	lastname
///198	Peter@hondahamilton.co.nz	xxxx	Peter	Bryant	2014-10-17 15:56:52.000	2061	358	192.168.201.65	2014-10-16 09:47:18.857	1	NULL	0	{B0E1700C-E212-459B-86DF-54568010901A}	2014-10-16 09:46:24.203	dealerPrincipal,dealers,forecasting,ExtranetAccess,WarrantiesAccess,CustomerLocationMappingAccess	1	0	Honda Hamilton	HON112	HON111	198	Peter@hondahamilton.co.nz	Peter	Bryant
		/// </summary>
		/// <returns></returns>

		public static UserDetails LoggedUserDetails() {
			var result = new UserDetails();
			var person = new Sql("select " + PersonTableName + "ID as id,email,firstname,lastname from " + PersonTableName + " where " + PersonTableName + "ID=", Security.LoggedInUserID).GetHashtable();
			if (person != null) {
				result.Email = person["email"] + "";
				result.FirstName = person["firstname"] + "";
				result.LastName = person["lastname"] + "";
				result.ID = (person["id"] + "").ToIntOrDie();
				result.Roles = (person["role"] + "");

			} else {
				//HttpResponse.c(Web.Root + "security/login?msg=dbusermissing");;
			}
			return result;

		}


		/// <summary>
		/// Auto logs on a user based on their Windows credentials (useful for intranets running on IE and Active Directory).
		/// Call this from ApplicationController
		/// </summary>
		/// <returns></returns>
		public static bool CheckForWindowsLogin() {
			var userName = Web.Request.ServerVariables["LOGON_USER"];
			//DebugLog("CheckForWindowsLogin - LOGON_USER: " + userName);
			if (Util.ServerIsDev && Web.Request["wl"] != null) {
				userName = "FakeDomain\\" + Web.Request["wl"];
			}
			if (userName.IsNotBlank() && userName.Contains("\\")) {
				// Strip off the domain
				userName = userName.Split('\\')[1].ToLower();
				var person = new Sql("select * from " + PersonTableName + " where WindowsLogin=", userName.SqlizeText()).GetActiveRecordList();
				if (person.Count() > 0) {
					var s = new Security();
					DebugLog("CheckForWindowsLogin - person: " + person[0]);
					return s.Login(person.First());
				}
			}
			return false;
		}

		public static void AuthenticateRequest() {
			//DebugLog("AuthenticateRequest");
			if (AuthProviderIsSavvy) {
				if (Util.GetSettingBool("Beweb_Security_AutoWindowsLogin", false)) {
					//DebugLog("CheckForWindowsLogin");
					Security.CheckForWindowsLogin();
				}
				UserDetails.GetCurrent();
			} else if (AuthProviderIsASPNET) {
				//DebugLog("AuthProviderIsASPNET");
				if (HttpContext.Current.User != null) {
					if (HttpContext.Current.User.Identity.IsAuthenticated) {
						if (HttpContext.Current.User.Identity is FormsIdentity) {
							// set the user role
							FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity; // Get Forms Identity From Current User
							FormsAuthenticationTicket ticket = id.Ticket; // Get Forms Ticket From Identity object
							// Retrieve stored user-data (contains comma separated roles)
							string userData = ticket.UserData;
							string[] roles = userData.Split(',');

							// Set the current user to the roles we specified
							// Create a new Generic Principal Instance and assign to Current User
							HttpContext.Current.User = new GenericPrincipal(id, roles);
						}
					}
				}
			} else {
				throw new ProgrammingErrorException("Unspecified Beweb_Security_AuthProvider. You can use ASPNETAUTH or SAVVYAUTH.");
			}
		}
	}

	public class UserDetails {
		public int ID;
		public string TableName;
		public string Email;
		public string FirstName;
		public string LastName;
		public string FullName { get { return FirstName + " " + LastName; } }
		public string Roles;
		public bool AutoLogin;
		public bool RememberMe;
		public string ExtraData;

		public UserDetails(string valueString) {
			if (valueString.StartsWith("SAVVYAUTHTICKET||")) {
				var fields = valueString.Split("||");
				ID = fields[1].ToInt(0);
				TableName = fields[2];
				Email = fields[3];
				FirstName = fields[4];
				LastName = fields[5];
				Roles = fields[6];
				AutoLogin = fields[7].ToBool();
				RememberMe = fields[8].ToBool();
				ExtraData = fields[9];
			}
		}

		public UserDetails(ActiveRecord record) {
			ID = record.ID_Field.ValueObject.ToInt(0);
			TableName = record.GetTableName();
			Email = record["Email"].ToString();
			FirstName = record["FirstName"].ToString();
			LastName = record["LastName"].ToString();
			Roles = record["Role"].ToString();
		}

		public UserDetails(Hashtable authenticatedPersonDetails) {
			ID = authenticatedPersonDetails[Security.PersonTableName + "ID"].ToInt(0);
			TableName = Security.PersonTableName;
			Email = authenticatedPersonDetails["Email"].ToString();
			FirstName = authenticatedPersonDetails["FirstName"].ToString();
			LastName = authenticatedPersonDetails["LastName"].ToString();
			Roles = authenticatedPersonDetails["Role"].ToString();
		}

		public UserDetails() {
		}

		private static string CookieName {
			get { return Beweb.Util.GetSiteCodeName() + "_SAVVYAUTH"; }
		}

		public override string ToString() {
			var str = "SAVVYAUTHTICKET";
			str += "||" + ID;
			str += "||" + TableName;
			str += "||" + Email;
			str += "||" + FirstName;
			str += "||" + LastName;
			str += "||" + Roles;
			str += "||" + AutoLogin;
			str += "||" + RememberMe;
			str += "||" + ExtraData;
			return str;
		}

		public static UserDetails GetCurrent() {
			if (HttpContext.Current == null) return null;
			UserDetails user = (UserDetails)Web.PageGlobals["Beweb_Security_CurrentUser"];
			if (user == null) {
				user = GetFromCookie();
				Web.PageGlobals["Beweb_Security_CurrentUser"] = user;
			}
			return user;
		}

		private static UserDetails GetFromCookie() {
			if (HttpContext.Current == null) return null;
			var cookie = Web.Request.Cookies[CookieName];
			if (cookie != null) {
				var valueString = Crypto.Decrypt(cookie.Value);
				if (valueString.IsNotBlank() && valueString.StartsWith("SAVVYAUTHTICKET||")) {
					return new UserDetails(valueString);
				}
			}
			return null;
		}

		public static UserDetails LoadFromDatabase(int id, string tableName) {
			if (HttpContext.Current == null) return null;
			var record = new ActiveRecord(tableName, tableName + "ID");
			if (record.LoadDataByID(id)) {
				return new UserDetails(record);
			}
			return null;
		}

		public void SaveToCookie() {
			string user = this.ToString();
			var cookie = new HttpCookie(CookieName, Crypto.Encrypt(user));
			// add expires etc cookie logic here
			if (AutoLogin || RememberMe) {
				cookie.Expires = DateTime.Today.AddYears(10);
			}
			cookie.HttpOnly = true;
			if (Util.GetSettingBool("Beweb_Security_AllowSubDomains", false)) {
				cookie.Domain = Util.GetPrimaryDomain();
			}
			Web.Response.SetCookie(cookie);
			Web.PageGlobals["Beweb_Security_CurrentUser"] = this;
		}

		public static void Logout() {
			HttpCookie cookie = new HttpCookie(CookieName);
			cookie.HttpOnly = true;
			if (Util.GetSettingBool("Beweb_Security_AllowSubDomains", false)) {
				cookie.Domain = Util.GetPrimaryDomain();
			}
			cookie.Expires = DateTime.Today.AddYears(-10);
			Web.Response.SetCookie(cookie);
			Web.PageGlobals["Beweb_Security_CurrentUser"] = null;
		}


	}

	public class PasswordMeter {
		public string verdict;
		public string reason;
		public string suggest;
		public string color;
		public string points;
	}



}

namespace BewebTest {
	/// <summary>
	///This is a test class for FmtTest and is intended
	///to contain all FmtTest Unit Tests
	///</summary>
	//	[TestClass()] -- MN 20110520 removed this test class because we cannot test it along with all other tests: Server cannot modify cookies after HTTP headers have been sent.
	public class SecurityTest {

		/// <summary>
		///A test for Login - with an incorrect password
		///</summary>
		[TestMethod()]
		public void LoginFailTest() {
			string u = "mike@sitename.co.nz";
			string p = "youshallnotpass";
			Security s = new Security();
			Assert.IsFalse(s.Login(u, p), String.Format("Login for user: {0}, pass: {1} should have failed but it didn't!!", u, p));
		}

	}
}