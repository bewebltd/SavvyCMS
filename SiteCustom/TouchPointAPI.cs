using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Beweb;
using Models;

/* Instrucitons:
 
	Assuming your team will utilise the APIs they will need to follow the Lion process for adding/editing a contacts record
 
	Check to see if the contact already exists based on email address and/or mobile number
	if contact exists then
 1. add to the Brand list (ie: Steinlager) [MN: this is an update contact details call]
 2.	add them to the DDB campaign (which we will setup for you), 
 3. append/store any campaign data required (in the campaign) 
 4. add a record to the Brand Profile entity table (this tracks all interactions)
 
 else if contact does not exist then 
 1. create new contact (consumer), 
 2. add to Brand list (ie: Steinlager) [MN: this is included in the create contact call]
 3. add them to the DDB campaign (which we will setup for you), 
 4. append/store any campaign data required (in the campaign) and 
 5. add a record to the Brand Profile entity table (this tracks all interactions)
  
 * 
 * Note
 * 2.5.3  Validation Rules
o  If Email is different from existing, then new value must be unique if it is 
unique for partition
o  If Mobile is different from existing, then cannot exist for another registered 
contact if defined as unique for partition
o  If a userid is changed then it must be unique within partition
o  Any other extension attributes with unique constraints must contain a value 
unique for partition
o  User Password must be at least 6 chars long and contain at least 1 numeric 
and one alpha char
 * 
 * exampel config values:
 
 	<add key="EnableTouchpointAPIDEV" value="true" />
	<add key="EnableTouchpointAPISTG" value="true" />
	<add key="EnableTouchpointAPILVE" value="true" />

	<!--Touchpoint API for steinlager fan database-->
	<add key="TouchpointAPICompanyIDDEV" value="3868"/>
	<add key="TouchpointAPIPartitionIDDEV" value="2693"/>
	<add key="TouchpointAPISecretDEV" value="wkmbgY75AC96T-d2v1"/>
	<add key="TouchpointAPIKeyDEV" value="TP_REST_API_DDB"/>
	<add key="TouchpointAPIUrlDEV" value="http://api.tst.tx.co.nz/"/>
	<add key="TouchpointAPICampaignIDDEV" value="14652"/>
	
	<add key="TouchpointAPICompanyID" value="2286"/>
	<add key="TouchpointAPIPartitionID" value="4980"/>
	<add key="TouchpointAPISecret" value="Ps2bdi4-VrC8q3khdL6w"/>
	<add key="TouchpointAPIKey" value="TP_REST_API_DDB"/>
	<add key="TouchpointAPIUrl" value="https://api.touchpointplatform.com/"/>
	<add key="TouchpointAPICampaignID" value="46991"/>
 * 
 */

namespace Site.SiteCustom {
	public class TouchPointAPI {
		public static bool EnableTouchpointAPI = Util.GetSettingBool("EnableTouchpointAPI", false);
		private static string TouchpointAPICompanyID = Util.GetSetting("TouchpointAPICompanyID");
		private static string TouchpointAPISecret = Util.GetSetting("TouchpointAPISecret");
		private static string TouchpointAPIKey = Util.GetSetting("TouchpointAPIKey");
		private static string TouchpointAPIUrl = Util.GetSetting("TouchpointAPIUrl");
		private static string TouchpointAPIPartitionID = Util.GetSetting("TouchpointAPIPartitionID");
		private static string TouchpointAPICampaignID = Util.GetSetting("TouchpointAPICampaignID");

		#region API internals
		private static string GetAuthorization(string httpMethod, string endPointUrl, string remoteHost, string userAgent) {
			string timestamp = Dates.MillisecondsSince1970.ToString();
			String nonce = GetNonce();
			string authorization = GetSignatureString(httpMethod, endPointUrl, remoteHost, userAgent, timestamp, nonce);
			return authorization;
		}

		private static string GetSignatureString(string httpMethod, string endPointUrl, string remoteHost, string userAgent, string timestamp, string nonce) {
			String signature = GetSignature(remoteHost, userAgent, timestamp, nonce, endPointUrl, httpMethod);

			var buffer = new StringBuilder();
			buffer.Append("ApplicationSignature");
			buffer.Append(" http-method=\"").Append(httpMethod).Append("\"");
			buffer.Append(" http-request-url=\"").Append(endPointUrl).Append("\"");
			buffer.Append(" timestamp=\"").Append(timestamp).Append("\"");
			buffer.Append(" nonce=\"").Append(nonce).Append("\"");
			buffer.Append(" signature=\"").Append(signature).Append("\"");
			buffer.Append(" signature-algorithm=\"md5-hash\"");

			return buffer.ToString();
		}


		// Create similar method to generate a random 64-bit, unsigned number encoded as an ASCII string in decimal 
		private static String GetNonce() {
			return new Random().Next(123400, int.MaxValue).ToString();
		}

		protected static String GetSignature(String remoteHost, String userAgent, String timestamp, String nonce, string endPointUrl, string httpMethod) {
			var buffer = new StringBuilder();

			buffer.Append(TouchpointAPIKey);
			buffer.Append(httpMethod);
			buffer.Append(endPointUrl);

			if (remoteHost.IsNotBlank()) {
				buffer.Append(remoteHost);
				buffer.Append(userAgent);
			}

			buffer.Append(timestamp);
			buffer.Append(nonce);
			buffer.Append(TouchpointAPISecret);

			return Crypto.CreateHashStandardMD5(buffer.ToString());

			/*
			 * 						byte[] computedHash = Md5Utils.computeMD5Hash(payloadJSON.getBytes("UTF-8"));
						String hexDigest = new String(
								Hex.encodeHex(computedHash));
			 */
			return Crypto.CreateHash(buffer.ToString());

		}
		#endregion

		public class TouchpointResponse {
			[ScriptIgnore]
			public string RequestUrl;
			[ScriptIgnore]
			public string RequestXml;
			[ScriptIgnore]
			public string ResponseXml;

			public string UserAccountToken {
				get { return this.ContactID != null ? Crypto.EncryptID(this.ContactID) : null; }
			}

			public string FirstName;
			public string LastName;

			public int? ContactID;
			public string ErrorMessage;
			public int? StatusCode;
		}

		/// <summary>
		/// Core API calling method
		/// </summary>
		private static TouchpointResponse SendTouchpointRequest(Http.HttpVerbConst httpMethod, string requestUrl, string data) {
			string baseUrl = TouchpointAPIUrl + "rest/core/" + TouchpointAPICompanyID + "/" + TouchpointAPIPartitionID + "/";
			string url = baseUrl + requestUrl;
			string userAgent = Web.Request.UserAgent;
			string remoteHost = "127.0.0.1";

			var headers = new WebHeaderCollection();
			headers["X-Touchpoint-API-Version"] = "1.1";
			headers["Remote-Host"] = remoteHost;
			headers["User-Agent"] = userAgent;

			headers["X-Touchpoint-Application-Key"] = TouchpointAPIKey;
			headers["Authorization"] = GetAuthorization(httpMethod, url, remoteHost, userAgent);
			headers["Accept"] = "application/xml";

			// call the API
			var xml = Http.Request(httpMethod, url, data, headers);

			bool isDebug = false;
			if (isDebug) {
				Web.WriteLine("Touchpoint Request: " + httpMethod + " url:" + url);
				Web.WriteLine("Data: " + bwbXml.PrettyPrintXml(data));
				Web.WriteLine("Response: " + bwbXml.PrettyPrintXml(xml));
			}else{
				Logging.dlog("Touchpoint Request: " + httpMethod + " url:" + url);
				Logging.dlog("Data: " + data);
				Logging.dlog("Response: " + xml);
			}

			// compose response
			var result = new TouchpointResponse();
			result.RequestUrl = url;
			result.RequestXml = data;
			result.ResponseXml = xml;
			if (xml.Contains("<errors>")) {
				result.ErrorMessage = xml;
				if (xml.Contains("request.attribute.uniqueconstraint.violated") && xml.Contains("attributeid=email")) {
					result.ErrorMessage = "There seems to be an existing Lion account that uses the email you entered.";
				} else if (xml.Contains("failedauthentication.invalidpassword")) {
					result.ErrorMessage = "Your password is incorrect.";
				} else if (xml.Contains("failedauthentication.invalidmatchvalue")) {
					result.ErrorMessage = "There does not seem to be an existing Lion account with the email you entered.";
				} else if (xml.Contains("failedauthentication.blankpassword")) {
					result.ErrorMessage = "Please enter your password.";
				} else if (xml.Contains("failedauthentication.maximumloginattemptsexceeded")) {
					result.ErrorMessage = "Maximum login attempts exceeded. Please wait 15 minutes and try again.";
				} else if (xml.Contains("failedauthentication.")) {
					result.ErrorMessage = "Failed authentication.";
				} else if (xml.Contains("request.attribute.uniqueconstraint.violated") && xml.Contains("attributeid=mobile")) {
					result.ErrorMessage = "There seems to be an existing Lion account that uses the mobile phone number you entered.";
				} else if (xml.Contains("request.attribute.invalidvalue") && xml.Contains("attributeid=userpassword")) {
					result.ErrorMessage = "Your password must be at least 6 chars long and contain at least one number and one letter.";
				} else if (xml.Contains("message")) {
					result.ErrorMessage = xml.ExtractTextBetween("<message>", "</message>");
				}
				if (result.ErrorMessage.IsBlank() && xml.Contains("errorcode")) {
					result.ErrorMessage = xml.ExtractTextBetween("<errorcode>", "</errorcode>");
				}
				if (result.ErrorMessage.IsBlank()) {
					result.ErrorMessage = "Unknown Touchpoint API error";
				}
			}
			if (xml.Contains("<contactid>")) {
				result.ContactID = xml.ExtractTextBetween("<contactid>", "</contactid>").ToInt();
			}
			if (xml.Contains("<statuscode>")) {
				result.StatusCode = xml.ExtractTextBetween("<statuscode>", "</statuscode>").ToInt();
			}
			return result;
		}

		/// <summary>
		/// Find user based on email address, and save into our local database (just as a backup - not really necessary)
		/// </summary>
		public static TouchpointResponse FindUser(string email) {
			var response = SendTouchpointRequest(Http.GET, "contacts?matchkey=email&matchvalue=" + email.Trim().UrlEncode() + "&attributes=*", null);
			if (response.ContactID == null) {
				response = SendTouchpointRequest(Http.GET, "contacts?matchkey=email&matchvalue=" + email.Trim().ToLower().UrlEncode() + "&attributes=*", null);
			}
			if (response.ContactID != null) {
				var fan = Fan.LoadByEmail(email);
				if (fan == null) {
					fan = new Fan();
					UpdateLocalFan(fan, response);
				}
			}
			return response;
		}

		/// <summary>
		/// Given a known contact ID (saved in localstorage), find user, and save into our local database (just as a backup - not really necessary)
		/// </summary>
		public static Fan FindUserByContactID(int touchPointContactID) {
			var response = SendTouchpointRequest(Http.GET, "contacts/" + touchPointContactID + "?attributes=*", null);
			var fan = Fan.LoadByTouchpointContactID(touchPointContactID);
			if (fan == null) {
				fan = new Fan();
				UpdateLocalFan(fan, response);
			}
			return fan;
		}

		private static TouchpointResponse FindUserMobile(string mobile) { //todo: number in db will be 64211885232, so create logic around making sure the number passed in is in this format 
			return SendTouchpointRequest(Http.GET, "contacts?matchkey=mobile&matchvalue=" + mobile.Trim().UrlEncode() + "&attributes=*", null);
		}

		public static void UpdateLocalFan(Fan fan, TouchpointResponse response) {
			var xml = response.ResponseXml;
			if (xml.Contains("<contactid>")) {
				fan.TouchpointContactID = xml.ExtractTextBetween("<contactid>", "</contactid>").ToInt();
				if (xml.Contains("<firstname>")) {
					fan.FirstName = xml.ExtractTextBetween("<firstname>", "</firstname>");
				}
				if (xml.Contains("<lastname>")) {
					fan.LastName = xml.ExtractTextBetween("<lastname>", "</lastname>");
				}
				if (xml.Contains("<gender>")) {
					fan.Gender = xml.ExtractTextBetween("<gender>", "</gender>");
				}
				if (xml.Contains("<email>")) {
					fan.Email = xml.ExtractTextBetween("<email>", "</email>");
				}
				if (xml.Contains("<mobile>")) {
					fan.Mobile = xml.ExtractTextBetween("<mobile>", "</mobile>");
				}
				if (xml.Contains("<city>")) {
					fan.City = xml.ExtractTextBetween("<city>", "</city>");
				}
				if (xml.Contains("<postcode>")) {
					fan.PostCode = xml.ExtractTextBetween("<postcode>", "</postcode>");
				}
				if (xml.Contains("<country>")) {
					fan.Country = xml.ExtractTextBetween("<country>", "</country>");
				}
				if (xml.Contains("<birthdate>")) {
					fan.DateOfBirth = xml.ExtractTextBetween("<birthdate>", "</birthdate>").ConvertToDate(null);
				}
				if (xml.Contains("<mobilepermission>")) {
					fan.MobilePermission = xml.ExtractTextBetween("<mobilepermission>", "</mobilepermission>").ToBool();
				}
				if (xml.Contains("<emailpermission>")) {
					fan.EmailPermission = xml.ExtractTextBetween("<emailpermission>", "</emailpermission>").ToBool();
				}
				fan.LastSyncDate = DateTime.Now;
				fan.Save();
			}
		}

		/// <summary>
		/// Add new subscriber to Lion member database
		/// </summary>
		public static TouchpointResponse CreateUser(Fan fan) {
			var data = GetFanXml(fan, true);
			var response = SendTouchpointRequest(Http.POST, "contacts", data);
			if (response.ContactID != null) {
				fan.TouchpointContactID = response.ContactID;
				AddUserToCampaign(fan.TouchpointContactID.Value);
				CreateBrandProfile(fan.TouchpointContactID.Value);
			}
			return response;
		}

		/// <summary>
		/// Update existing user in Lion member database (note they may have signed up for a different Lion product - need to handle this)
		/// </summary>
		public static TouchpointResponse UpdateUser(Fan fan, bool updatePassword) {
			var data = GetFanXml(fan, updatePassword);
			var response = SendTouchpointRequest(Http.PUT, "contacts/" + fan.TouchpointContactID, data);
			if (response.ContactID != null) {
				fan.TouchpointContactID = response.ContactID;
				UpdateCampaign(fan.TouchpointContactID.Value);
				UpdateBrandProfile(fan.TouchpointContactID.Value);
			}
			return response;
		}

		private static string GetFanXml(Fan fan, bool updatePassword) {
			var xml = new HtmlTag("request");
			var contact = new HtmlTag("contact");
			//contact.AddTag("name", fan.FirstName+" "+fan.LastName); -- not needed will generate from firstname and last name attributes in extention attributes
			//contact.AddTag("userid", fan.Email); -- not needed
			if (updatePassword) {
				contact.AddTag("userpassword", Security.DecryptPassword(fan.Password)); //decrypt before sending to Touchpoint
			}
			contact.AddTag("emailpermission", fan.EmailPermission.ToStringLower());
			contact.AddTag("mobilepermission", fan.MobilePermission.ToStringLower());
			var ext = new HtmlTag("extensionattributes"); //extention attributes
			ext.AddTag("firstname", fan.FirstName);
			ext.AddTag("lastname", fan.LastName);
			ext.AddTag("email", fan.Email);
			ext.AddTag("mobile", fan.Mobile);
			ext.AddTag("city", fan.City);
			ext.AddTag("postcode", fan.PostCode);
			ext.AddTag("country", fan.Country);
			ext.AddTag("gender", fan.Gender);
			ext.AddTag("birthdate", Fmt.DateTimeISOZ(fan.DateOfBirth));
			contact.AddTag(ext);
			contact.AddRawHtml("<lists><list><alphaid>Steinlager</alphaid><status>S</status></list></lists>");
			xml.AddTag(contact);
			string data = "<?xml version=\"1.0\"?>\n" + xml.ToString();
			return data;
		}

		/// <summary>
		/// Delete a subscriber from Touchpoint Lion member database. We never need this in production, only for testing.
		/// </summary>
		public static TouchpointResponse DeleteUser(int touchpointContactID) {
			var xml = (SendTouchpointRequest(Http.DELETE, "contacts/" + touchpointContactID, null));
			return xml;
		}

		/// <summary>
		/// Add user to DDB campaign if not already
		/// </summary>
		public static TouchpointResponse UpdateCampaign(int contactID) {
			var url = "campaigns/" + TouchpointAPICampaignID + "/members?matchkey=contactid&matchvalue=" + contactID;
			var response = SendTouchpointRequest(Http.GET, url, null);
			if (response.ResponseXml.Contains("<campaignmemberid>")) {
				// no need to update, as we have already joined this campaign
				//int recordID = response.ResponseXml.ExtractTextBetween("<campaignmemberid>", "</campaignmemberid>").ToInt();
				//var data = GetCampaignXml(contactID);
				//response = SendTouchpointRequest(Http.PUT, "campaigns/" + TouchpointAPICampaignID + "/members/" + recordID, data);
			} else {
				response = AddUserToCampaign(contactID);
			}
			return response;
		}

		public static TouchpointResponse AddUserToCampaign(int contactID) {
			var data = GetCampaignXml(contactID);
			var response = SendTouchpointRequest(Http.POST, "campaigns/" + TouchpointAPICampaignID + "/members", data);
			return response;
		}

		private static string GetCampaignXml(int contactID) {
			var xml = new HtmlTag("request");
			var campaignMember = new HtmlTag("campaignmember");
			campaignMember.AddTag("contactid", contactID.ToString());
			campaignMember.AddTag("contactdate", Fmt.DateTimeISOZ(DateTime.Now));
			xml.AddTag(campaignMember);
			string data = "<?xml version=\"1.0\"?>\n" + xml.ToString();
			return data;
		}

		/// <summary>
		/// Update last interaction date for Steinlager in Brand Profile table
		/// </summary>
		/// <param name="contactID"></param>
		/// <returns></returns>
		public static TouchpointResponse UpdateBrandProfile(int contactID) {
			var url = "entities/brand-profile/records?matchkey=contactid&matchvalue=" + contactID + "&matchkey2=brandcode&matchvalue2=steinlager";
			var response = SendTouchpointRequest(Http.GET, url, null);
			if (response.ResponseXml.Contains("<entityrecordid>")) {
				int recordID = response.ResponseXml.ExtractTextBetween("<entityrecordid>", "</entityrecordid>").ToInt();
				var data = GetBrandProfileXml(contactID);
				response = SendTouchpointRequest(Http.PUT, "entities/brand-profile/records/" + recordID, data);
			} else {
				response = CreateBrandProfile(contactID);
			}
			return response;
		}

		public static TouchpointResponse RecordFanInteraction(int contactID) {
			return UpdateBrandProfile(contactID);
		}

		public static TouchpointResponse CreateBrandProfile(int contactID) {
			string data = GetBrandProfileXml(contactID);
			var response = SendTouchpointRequest(Http.POST, "entities/brand-profile/records", data);
			return response;
		}

		private static string GetBrandProfileXml(int contactID) {
			var xml = new HtmlTag("request");
			var entityrecord = new HtmlTag("entityrecord");
			var ext = new HtmlTag("extensionattributes"); //extention attributes
			ext.AddTag("brandcode", "steinlager");
			ext.AddTag("contactid", contactID.ToString());
			ext.AddTag("lastlogindate", Fmt.DateTimeISOZ(DateTime.Now));
			ext.AddTag("lastinteractiondate", Fmt.DateTimeISOZ(DateTime.Now));
			entityrecord.AddTag(ext);
			xml.AddTag(entityrecord);
			string data = "<?xml version=\"1.0\"?>\n" + xml.ToString();
			return data;
		}

		/// <summary>
		/// Authenticate user against Touchpoint Lion member database with email and password.
		/// Password is not necessarily stored in our local database, and does not come back from API calls so we call this method to check login.
		/// </summary>
		public static TouchpointResponse LoginUser(string email, string password) {
			var xml = new HtmlTag("request");
			xml.AddTag("token", "");
			xml.AddTag("matchkey", "email");
			xml.AddTag("matchvalue", email.Trim());
			xml.AddTag("password", password.Trim());
			string data = "<?xml version=\"1.0\"?>\n" + xml.ToString();
			//Web.Write(bwbXml.PrettyPrintXml(data));
			var response = SendTouchpointRequest(Http.POST, "contacts/authenticator", data);
			if (response.ContactID != null) {
				var fan = FindUserByContactID(response.ContactID.Value);
				response.FirstName = fan.FirstName;
				response.LastName = fan.LastName;
			}
			return response;
		}

		/// <summary>
		/// Sends a password reminder via Touchpoint 
		/// </summary>
		public static TouchpointResponse ForgotPassword(string email) {
			var response = FindUser(email);
			if (response.ContactID != null) {
				var xml = new HtmlTag("request");
				xml.AddTag("messagesubmitteralphaid", "steinlager-password");
				string data = "<?xml version=\"1.0\"?>\n" + xml.ToString();
				response = SendTouchpointRequest(Http.POST, "contacts/" + response.ContactID + "/sentmessages", data);
			} else {
				response.ErrorMessage = "There is no existing Lion account with the email you entered.";
			}
			return response;
		}


		////Tests
		//private static string LastAddedID = "";
		//private static string LastAddedName = "";
		//private static string LastAddedEmail = "";
		//private static string LastAddedMobile = "";
		//public static Fan LastAddedFan;

		////[TestMethod]
		//public static void TestCreateLastFan() {
		//	if (LastAddedFan != null) {
		//		var fan = LastAddedFan;
		//		Web.Write(CreateUser(fan));
		//	} else {
		//		Assert.Fail("fan not initalized");
		//	}
		//}

		//[TestMethod]
		public static void TestFetchMikeLive() {
			var xml = (SendTouchpointRequest(Http.GET, "contacts/183626324?includelists=true&attributes=*", null));
			Web.Write(bwbXml.PrettyPrintXml(xml.ResponseXml));
		}

		[TestMethod]
		public static void TestCreateAndLogin() {
			var fan = new Fan();
			var rnd = Crypto.RandomChars(20);
			fan.FirstName = "josh";
			fan.LastName = "cave";
			fan.Email = "josh.cave" + rnd + "@beweb.co.nz";
			fan.Mobile = "+64211885232";
			fan.DateOfBirth = new DateTime(1970, 12, 21);
			fan.Password = "yellowst4r";
			fan.Gender = "M";
			fan.City = "Auckland";
			fan.Country = "NZ";
			fan.PostCode = "0626";
			var touchpointResponse = CreateUser(fan);
			Assert.AreEqual(201, touchpointResponse.StatusCode);

			//FindUserByContactID(touchpointResponse.ContactID.ToInt());

			var response = (LoginUser(fan.Email, fan.Password));
			Assert.IsNotNull(response.ContactID);

			response = (LoginUser(fan.Email, "bullshit"));
			Assert.IsNull(response.ContactID);

			// clean up
			if (fan.TouchpointContactID != null) {
				response = (DeleteUser(fan.TouchpointContactID.Value));
				Assert.AreEqual(200, response.StatusCode);
			} else {
				Assert.Fail("cant find to delete");
			}
		}



		//		//OLD Test Methods
		//		[TestMethod]
		//		public static void TestSignature() {
		//			string testerSig = "d82a61040cf3af8cd12c6fe1a73cb4ee";

		//			string httpMethod = "GET";
		//			string endPointUrl = "http://api.tst.tx.co.nz/rest/core/3868/2693/contacts?matchkey=email&matchvalue=steve@beweb.co.nz&attributes=email,name,extension.firstname,extension.lastname";
		//			string remoteHost = "127.0.0.1";
		//			string userAgent = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36";
		//			string timestamp = "1401484514859";
		//			string nonce = "16310162358376858000";
		//			var sig = GetSignature(httpMethod, endPointUrl, remoteHost, userAgent, timestamp, nonce);
		//			Assert.AreEqual(testerSig, sig);
		//		}


		//		[TestMethod]
		//		public static void TestFetchMikeLive() {
		//			var xml = (SendTouchpointRequest(Http.GET, "contacts/183626324?includelists=true&attributes=*", null));
		//			Web.Write(bwbXml.PrettyPrintXml(xml));
		//		}


	}
}