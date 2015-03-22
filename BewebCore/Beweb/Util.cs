#define Util
#define FileSystem
#define SecurityRoles
#define BaseTypeExtensions
#define Debug
#define TestExtensions

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Beweb;

namespace Beweb {
	public class Util {
		static string _serverIs = null;

		#region ServerIs
		/// <summary>
		/// Based on 'ServerStages' appsetting and the URL hostname contained in the 'ServerIs' appsettings this will tell you the server you are on.
		/// Standard server stages are DEV, STG or LVE.
		/// If host is not listed in config settings, ServerIs will return the last server stage (ie normally LVE).
		/// </summary>
		/// <returns>a string similar to DEV, STG or LVE (depends on ServerStages appsetting for return values)</returns>
		public static string ServerIs() {
			if (_serverIs != null && StagingHasOwnAppPool) {
				return _serverIs;
			}

			string returnValue = "";
			var context = HttpContext.Current;
			string currentHost = null;
			if (context != null) {
				// this checks the hostname or machine name only
				if (Web.IsRequestInitialised) {
					currentHost = context.Request.Url.Host.ToLower();
					returnValue = ServerIs_Internal(currentHost);
					_serverIs = returnValue;
				} else {
					// We may be on Application_Start or sometime when we have Context but don't have Request.
					// In this case use machine name as this will tell us if it should be DEV.
					// Unknown machine names will default to LVE.
					currentHost = context.Server.MachineName.ToLower();
					returnValue = ServerIs_Internal(currentHost);
				}
				//removed due to security hole currentURL += context.Request.Url.AbsolutePath.ToLower();
			} else {
				currentHost = Environment.MachineName.ToLower();
				returnValue = ServerIs_Internal(currentHost);
			}

			return returnValue;
		}

		/// <summary>
		/// Returns serverstage based on given hostname. Defaults to last stage (normally LVE) if unrecognised.
		/// </summary>
		/// <param name="currentHost"></param>
		/// <returns></returns>
		internal static string ServerIs_Internal(string currentHost) {
			string returnValue = "";
			string serverStages = ConfigurationManager.AppSettings.Get("ServerStages"); // stages is something like DEV,STG,LVE
			if (serverStages + "" == "") serverStages = "DEV,STG,LVE";
			string[] stages = serverStages.Split(','); // stages is something like DEV,STG,LVE
			currentHost = (currentHost + "").ToLower();

			// determine what server we are on)
			//if (context != null) {
			// this checks the host and the path (no port)
			//removed due to security hole currentURL += context.Request.Url.AbsolutePath.ToLower();

			string[] serverIndicator = new string[stages.Length];
			for (int i = 0; i < stages.Length; i++) {
				// put all the domains to check in another array
				serverIndicator[i] = ConfigurationManager.AppSettings.Get("ServerIs" + stages[i]).ToLower();
			}
			int maxStageIndex = stages.Length - 1; // the last value in ServerStages
			int stageIndex = maxStageIndex;

			for (int i = 0; i < serverIndicator.Length; i++) {
				// split the connection string name on the pipe symbol | - could add others here if needed
				string[] urlsToCheck = serverIndicator[i].Split('|');
				foreach (string s in urlsToCheck) {
					// look for URL part inside our current URL, if we find it use this connection string
					if (currentHost.IndexOf(s.ToLower()) > -1) {
						stageIndex = i;
						break; // may as well break out of this loop
					}
				}
				if (stageIndex != maxStageIndex) break; // we've found the lowest stage so quit
				// this is normally LVE which means that any unknown domain names will be considered LVE - this is important
			}

			// translate the found value into the correct string
			returnValue = stages[stageIndex];
			//} 
			//else {
			// no http context so we are assume we are in unit testing mode
			// pick the first option in ServerStages (which is DEV)
			//	returnValue = stages[0]; 
			//}

			return returnValue;
		}
		public static bool ServerIsDev { get { return ServerIs() == "DEV"; } }
		public static bool ServerIsStaging { get { return ServerIs() == "STG"; } }
		public static bool ServerIsLive { get { return ServerIs() == "LVE"; } }

		public static bool IsBewebOffice {
			get {
				return IsDeveloperOffice;
			}
		}
		
		public static bool IsDeveloperOffice {
			get {
				string ipAddress = "";
				if (Web.IsRunningOnWebServer && Web.IsRequestInitialised) {
					ipAddress = Web.UserIpAddress + "";
				}

				if (Web.Host == "localhost") return true;  //20140311jn added - handle ipv6 localhost (used by structure.aspx)
				if (ipAddress == "::1") return true;

				var officeIPs = Util.GetSettingPipeList("DeveloperOfficeIP", "");
				if (officeIPs.Contains(ipAddress)) return true;

				officeIPs = Util.GetSettingPipeList("BewebOfficeIP", "");
				if (officeIPs.Contains(ipAddress)) return true;

				return (
					ipAddress == "127.0.0.1" ||  // callback to own server, good for http get async
					Security.IsInRole(SecurityRolesCore.Roles.DEVELOPER)); //JN added this so that logged in devs see errors even if ip wrong
			}
		}

		private static bool? _stagingHasOwnAppPool = null;

		public static bool StagingHasOwnAppPool {
			get {
				if (_stagingHasOwnAppPool == null) {
					_stagingHasOwnAppPool = ConfigurationManager.AppSettings.Get("StagingHasOwnAppPool").ToBool();
				}
				return (bool)_stagingHasOwnAppPool;
			}
		}

		private static bool? _useCanonicalHost = null;

		public static bool UseCanonicalHost {
			get {
				if (_useCanonicalHost == null) {
					_useCanonicalHost = ConfigurationManager.AppSettings.Get("EnsureCanonicalHost").ToBool();
				}
				return (bool)_useCanonicalHost;
			}
		}

		#endregion

		#region IsDevAccess
		/// <summary>
		/// return true if session variable DevAccess = true
		/// </summary>
		/// <returns></returns>
		public static bool IsDevAccess() {
			bool result = false;
#if SecurityRoles
			result = (Security.IsInRole(SecurityRolesCore.Roles.DEVELOPER));
#endif
			if (!result) {
				if (HttpContext.Current.Session["DevAccess"] == null || HttpContext.Current.Session["DevAccess"].ToString() != "true") {
					result = false;
				} else {
					result = true;
				}
			}
			if (!result) {
				if ((Util.IsBewebOffice || ServerIsDev) && HttpContext.Current.Request["Dev"] + "" == "1") {
					result = true;
				} else {
					result = false;
				}
			}
			return result;
		}
		#endregion
		#region IsAdmin
		//public static bool IsAdministratorAccess() {
		//  return (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole(Security.ROLE_ADMINISTRATOR));
		//}
		#endregion


		#region HttpGet
		/// <summary>
		/// wraps up the .NET HttpWebRequest and HttpWebResponse to make a page request simpler
		/// </summary>
		/// <param name="URL">the URL to request (can be relative or absolute)</param>
		/// <returns>the response as a string</returns>
		public static string HttpGet(string URL) {
			//return HttpGet(URL, "HttpGet ERROR requesting: URL"); 20110106 MN/JN - breaking change
			return HttpGet(URL, "throw");
		}
		public string GetPageFullURL() {
			//' returns full URL including protocol, server, port, path, page and querystring
			string pageURL, query;
			if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on") {
				pageURL = "https://";
			} else {
				pageURL = "http://";
			}
			if (HttpContext.Current.Request.ServerVariables["SERVER_PORT"] != "80") {
				pageURL = pageURL + HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
			}
			//pageURL = pageURL & Request.ServerVariables("server_name")
			pageURL = pageURL + HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
			query = HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
			if (query != "") {
				pageURL = pageURL + "?" + query;
			}
			return pageURL;
		}

		/// <summary>
		/// Wraps up the .NET HttpWebRequest and HttpWebResponse to make a page request simpler.
		/// IMPORTANT: pass the string "throw" as second parameter to make it throw errors - otherwise you have to use the return value and manually handle errors.
		/// </summary>
		/// <param name="URL">the URL to request (can be relative or absolute)</param>
		/// <param name="ErrorFormat">the string to return if there is an error. "URL" will be matched and replaced with the requested URL if it is found</param>
		/// <returns>the response as a string</returns>
		public static string HttpGet(string URL, string ErrorFormat, object dummy) {
			return HttpGet(URL, ErrorFormat);
		}


		/// <summary>
		/// make an async call to a url, and don't return any result
		/// </summary>
		/// <param name="URL"></param>
		public static void HttpGetAsync(string URL) {
			Util.HttpGetAsync(URL, null);
		}
		/// <summary>
		/// make an async call to a url, and don't return any result
		/// </summary>
		/// <param name="URL"></param>
		/// <param name="ErrorFormat"></param>
		public static void HttpGetAsync(string URL, string ErrorFormat) {
			// Check Url is good

			if (!Uri.IsWellFormedUriString(URL, UriKind.Absolute)) {
				string newUrl = Web.ResolveUrlFull(URL);
				if (!Uri.IsWellFormedUriString(newUrl, UriKind.Absolute)) {
					throw new Beweb.ProgrammingErrorException("HttpGetAsync: Invalid URL - requires an absolute URL: " + URL);
				} else {
					URL = newUrl;
				}
			}

			//create the delegate
			HttpGetAsyncDelegate asyncAction = HttpGetAsyncAction;
			//invoke it asynchrnously, control passes to next statement
			Logging.DiagnosticData data = null;
			try {
				data = new Logging.DiagnosticData(true);
			} catch (Exception ex) {
				if (ex.Message.Contains("Thread was being aborted")) {									 //handle thread stop - in debugger
					return;
				}
				throw new ProgrammingErrorException("get async failed to url [" + URL + "]", ex);
			}

			if (data != null) {
				string diagnosticDataHtml = data.ToHtml();
				asyncAction.BeginInvoke(URL, ErrorFormat, diagnosticDataHtml, null, null);
			}
		}

		private delegate void HttpGetAsyncDelegate(string URL, string ErrorFormat, string diagnosticDataHtml); //delegate for the action

		private static void HttpGetAsyncAction(string URL, string ErrorFormat, string diagnosticDataHtml) {
			Util.HttpGet(URL, ErrorFormat ?? "sendemail", diagnosticDataHtml);
		}


		/// <summary>
		/// wraps up the .NET HttpWebRequest and HttpWebResponse to make a page request simpler
		/// </summary>
		/// <param name="URL">the URL to request (can be relative or absolute)</param>
		/// <param name="ErrorFormat">the string to return if there is an error. "URL" will be matched and replaced with the requested URL if it is found</param>
		/// <returns>the response as a string</returns>
		//public static string HttpPost(string URL, string postData, string ErrorFormat) {
		//	HttpWebRequest httpWReq =
		//	(HttpWebRequest)WebRequest.Create("http://domain.com/page.aspx");
		//
		//	ASCIIEncoding encoding = new ASCIIEncoding();
		//	//string postData = "username=user";
		//	//postData += "&password=pass";
		//	byte[] data = encoding.GetBytes(postData);
		//
		//	httpWReq.Method = "POST";
		//	httpWReq.ContentType = "application/x-www-form-urlencoded";
		//	httpWReq.ContentLength = data.Length;
		//
		//	using (Stream stream = httpWReq.GetRequestStream()) {
		//		stream.Write(data, 0, data.Length);
		//	}
		//
		//	HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
		//
		//	string responseString = "err";
		//	Stream responseStream = response.GetResponseStream();
		//	if (responseStream != null) {
		//		responseString = new StreamReader(responseStream).ReadToEnd();
		//	}
		//	return responseString;
		//}
		public static string HttpGet(string URL, string ErrorFormat) {
			return HttpGet(URL, ErrorFormat, null);
		}

		/// <summary>
		/// wraps up the .NET HttpWebRequest and HttpWebResponse to make a page request simpler
		/// </summary>
		/// <param name="URL">the URL to request (can be relative or absolute)</param>
		/// <param name="ErrorFormat">the string to return if there is an error. "URL" will be matched and replaced with the requested URL if it is found</param>
		/// <returns>the response as a string</returns>
		public static string HttpGet(string URL, string ErrorFormat, string diagnosticDataHtml) {
			string returnValue = "";

			// if no "http://" in URL then relative URL was meant
			//MN20101111 - this does not work very well at all!! It makes bad assumptions...
			//if(!URL.StartsWith("http://"))
			//{
			//  URL = URL.TrimStart(new Char[] {'/'}); // trim off a leading slash if there is one

			//  Uri currentUri = req.Url;
			//  //TODO: make the next line a bit more robust??
			//  string virtualPath = (ServerIs() != "LVE") ? currentUri.Segments[1] : ""; // this assumes dev and staging both use a virtual path

			//  URL = String.Format("http://{0}{1}{2}" + URL,
			//                      currentUri.Host,
			//                      currentUri.Segments[0],
			//                      virtualPath
			//    );
			//}

			HttpWebRequest httpRequest;
			try {
				httpRequest = WebRequest.Create(URL) as HttpWebRequest;
			} catch (UriFormatException exception) {
				throw new ProgrammingErrorException("Incorrect URL format, you must use a absolute full URL. URL is: " + URL, exception);
			}
			httpRequest.Timeout = 1000 * 60 * 10;//in millisecs														10 mins
			httpRequest.ReadWriteTimeout = 1000 * 60 * 10;//in millisecs									10 mins

			if (httpRequest != null) {
				httpRequest.Method = "GET";
				httpRequest.Accept = "*/*"; // any file type
				try {
					using (HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse) {
						if (httpResponse != null) {
							StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
							returnValue = sr.ReadToEnd();
							sr.Close();
							httpResponse.Close();
						}
					}
				} catch (Exception e) {
					// try and replace the URL if we find it - if not, just return the user's Error String
					if (ErrorFormat == "throw") {
						if (e is WebException) {
							throw;
						}
						var exception = new Exception("Get error[" + URL + "] \n" + e.Message + "\nType: " + e.GetType());
						exception.Data["HttpGet URL"] = URL;
						throw exception;
					} else if (ErrorFormat == "sendemail") {
						//string errorBody = "Http GET Error requesting URL [" + URL + "]<br><br>" + e.Message;
						//if (diagnosticDataHtml.IsNotBlank()) errorBody += "<br><br>" + diagnosticDataHtml;
						if (diagnosticDataHtml.IsNotBlank()) e.Data["Where HttpGetAsync was called"] = diagnosticDataHtml;
						var dump = new Logging.DiagnosticData(e);
						string errorBody = dump.ToHtml();
						new ElectronicMail() { ToAddress = SendEMail.EmailAboutError, Subject = "ERROR: " + Util.GetSiteName() + " " + Util.ServerIs() + " - Http GET Error", BodyHtml = errorBody }.Send(true);
						//new ElectronicMail() { ToAddress = "jonathan@beweb.co.nz", ToName = "Jonathan Brake", Subject = "ERROR: " + Util.GetSiteName() + " " + Util.ServerIs() + " - Http GET Error", BodyHtml = errorBody }.Send(true);
						//SendEMail.SimpleSendHTMLEmail(SendEMail.EmailAboutError, "Http GET Error - " + Util.GetSetting("SiteName") + " " + Util.ServerIs(), errorBody);
					} else {
						returnValue = ErrorFormat.Replace("URL", URL);
					}
				}
			}
			return returnValue;
		}
		#endregion

		#region File Includes

		/// <summary>
		/// Render script tag including jQuery library. 
		/// The version to be included is specified in the Web_AppSettings.config.
		/// This can be either version number only (eg 1.7.2) or full path to CDN hosted or a local file.
		/// Note that jQuery often needs to be included before other scripts and before the page loads.
		/// EXAMPLE: 
		/// <add key="jQueryLatest" value="1.7.2"/>
		/// <add key="jQueryLatest" value="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2.min.js"/>
		/// </summary>
		public static void IncludejQuery() {
			//IncludejQuery(null);  MN 20111114 - does this line below do anything under MVC, or does it just evaluate to null anyway? I think Current.Handler is not a Page but a Controller
			IncludejQuery(HttpContext.Current.Handler as System.Web.UI.Page);
		}

		public static void IncludejQuery(Page sender) {
			IncludejQuery(sender, IncludeRenderMode.Auto);
		}

		public static void IncludejQuery(Page sender, Util.IncludeRenderMode includeRenderMode) {
			// MN 20120704 - improved jquery CDN detection, allows just version number in web app settings
			string jQueryVersion = Util.GetSetting("jQueryLatest", "throw");
			string jQueryPath;
			if (jQueryVersion.ContainsOnly("0123456789.")) {
				// version number only - auto switch - first try Google CDN
				jQueryPath = Web.Protocol + "ajax.googleapis.com/ajax/libs/jquery/" + jQueryVersion + "/jquery.min.js";
				IncludeJavascriptFile(sender, jQueryPath);

				// auto switch to microsoft in case google CDN is down
				jQueryPath = Web.Protocol + "ajax.aspnetcdn.com/ajax/jQuery/jquery-" + jQueryVersion + ".min.js";
				IncludeJavascript(sender, "window.jQuery || document.write('<script src=\"" + jQueryPath + "\"><\\/script>')", true, includeRenderMode);

				// auto switch to local in case the internet is down
				jQueryPath = Web.Root + "js/jquery-" + jQueryVersion + "/jquery-" + jQueryVersion + ".min.js";
				if (File.Exists(Web.MapPath(jQueryPath))) {
					IncludeJavascript(sender, "window.jQuery || document.write('<script src=\"" + jQueryPath + "\"><\\/script>')", true, includeRenderMode);
				} else {
					// failed to find local version of jQuery
					IncludeJavascript(sender, "window.jQuery || alert('Util.IncludejQuery: jQuery " + jQueryVersion + " not available - CDN down and no local copy')", true, includeRenderMode);
				}

			} else {
				// full path - just include path as specified
				jQueryPath = jQueryVersion.Replace("~/", Web.Root);
				IncludeJavascriptFile(sender, jQueryPath, includeRenderMode);
			}
		}


		public static void InitRedactor(System.Web.UI.Page p) {
			InitRedactor(p, Util.IncludeRenderMode.Auto);
		}

		public static void InitRedactor(System.Web.UI.Page p, Util.IncludeRenderMode includeRenderMode) {
			string redactorPath = Util.GetSetting("RedactorFolderPath", Web.Root + "js/redactor");

			//include main css and js files
			Util.IncludeStylesheetFile(p, Web.ResolveUrl(redactorPath + "/Redactor.css"));
			string mainJsFileInclude = !Util.ServerIsLive ? Web.ResolveUrl(redactorPath + "/Redactor.js") : Web.ResolveUrl(redactorPath + "/Redactor.min.js");
			Util.IncludeJavascriptFile(p, mainJsFileInclude, includeRenderMode);
			Util.IncludeJavascriptFile(p, Web.ResolveUrl(redactorPath + "/RedactorHelper.js"));

			//aditional stylesheets
			string styleSheetsCsv = Util.GetSetting("RedactorStylesheets", "~/site.css");
			foreach (var styleSheet in styleSheetsCsv.Split(",")) {
				Util.IncludeStylesheetFile(p, redactorPath + "/" + styleSheet, includeRenderMode);
			}

			//aditional plugins
			string redactorPlugins = Util.GetSetting("RedactorPlugins", "");
			foreach (var jsFile in redactorPlugins.Split(",")) {
				if (jsFile.IsNotBlank()) {
					Util.IncludeJavascriptFile(p, redactorPath + "/" + jsFile, includeRenderMode);
				}
			}
		}



		#region IncludeThickbox
		[Obsolete("Thickbox is no longer supported and does not work that great with latest browsers. Use Colorbox instead.")]
		public static void IncludeThickbox(Page sender) {
			//Util.IncludeJavascriptFile(sender, ConfigurationManager.AppSettings.Get("jQueryLatest"));
			Util.IncludeJavascriptFile(sender, Web.Root + "/js/thickbox/thickbox-compressed.js");
			Util.IncludeStylesheetFile(sender, Web.Root + "/js/thickbox/thickbox.css");
			ClientScriptManager cm = sender.ClientScript;
			cm.RegisterStartupScript(sender.GetType(), "thickBoxParam", "tb_pathToImage = '" + ResolveUrl(Web.Root + "/js/thickbox/loadingAnimation.gif" + "'"), true);
		}
		#endregion

		public static void IncludeColorbox() {
			//Util.IncludeJavascriptFile(Web.Root+"js/colorbox/jquery.colorbox.js");
			Util.IncludeJavascriptFile(GetSetting("ColorboxLatest", Web.Root + "js/colorbox/jquery.colorbox-min.js"));
			Util.IncludeStylesheetFile(Web.Root + "js/colorbox/colorbox.css");
		}

		public static void IncludeColorbox(Page sender) {
			IncludeColorbox(sender, IncludeRenderMode.Auto);
		}

		public static void IncludeColorbox(Page sender, Util.IncludeRenderMode includeRenderMode) {
			//Util.IncludeJavascriptFile(sender, ConfigurationManager.AppSettings.Get("jQueryLatest"));
			Util.IncludeJavascriptFile(sender, GetSetting("ColorboxLatest", Web.Root + "js/colorbox/jquery.colorbox-min.js"), includeRenderMode);
			Util.IncludeStylesheetFile(sender, Web.Root + "js/colorbox/colorbox.css", null /*media type*/, includeRenderMode);
			//Util.IncludeJavascript(sender, "$(document).ready(function () {$('.popup').colorbox();});",true,IncludeRenderMode.Foot);
		}

		public static void IncludeSavvyValidate() {
			IncludeSavvyValidate(null);
		}

		public static void IncludeSavvyValidate(Page sender) {
			IncludeSavvyValidate(sender, IncludeRenderMode.Auto);
		}

		public static void IncludeSavvyValidate(Page sender, Util.IncludeRenderMode includeRenderMode) {
			Util.IncludeJavascriptFile(sender, Web.Root + "js/BewebCore/Savvy.validate.js", includeRenderMode);
		}

		public static void IncludeThemeStyles() {
			IncludeThemeStyles(null);
		}

		public static void IncludeThemeStyles(Page sender) {
			var folder = System.IO.Directory.GetFiles(Web.MapPath(Web.Theme), "*.css");
			foreach (string file in folder) {
				if (file.Contains("bootstrap")) {
					if (file.Contains(".min.css") || !folder.ContainsInsensitive(file.Replace(".css", ".min.css"))) {
						// this is a min version or there is no min version
						Util.IncludeStylesheetFile(sender, Web.UnMapPath(file));
					}
				}
			}
			foreach (string file in folder) {
				if (!file.Contains("bootstrap")) {
					if (file.Contains(".min.css") || !folder.ContainsInsensitive(file.Replace(".css", ".min.css"))) {
						// this is a min version or there is no min version
						Util.IncludeStylesheetFile(sender, Web.UnMapPath(file));
					}
				}
			}

		}

		public static void IncludeThemeJavascript() {
			IncludeThemeJavascript(null);
		}

		public static void IncludeThemeJavascript(Page sender) {
			var folder = System.IO.Directory.GetFiles(Web.MapPath(Web.Theme), "*.js");
			foreach (string file in folder) {
				if (file.Contains("bootstrap")) {
					if (file.Contains(".min.js") || !folder.ContainsInsensitive(file.Replace(".js", ".min.js"))) {
						// this is a min version or there is no min version
						Util.IncludeJavascriptFile(sender, Web.UnMapPath(file));
					}
				}
			}
			foreach (string file in folder) {
				if (file.Contains(".min.js") || !folder.ContainsInsensitive(file.Replace(".js", ".min.js"))) {
					// this is a min version or there is no min version
					if (!file.Contains("bootstrap")) {
						Util.IncludeJavascriptFile(sender, Web.UnMapPath(file));
					}
				}
			}
		}

		/// <summary>
		/// Render script tags for including latest jQuery Validation library plus Beweb customisations file (Beweb.jquery.validate.js).
		/// The version to be included is specified in the Web_AppSettings.config.
		/// This can be either CDN hosted or a local file.
		/// EXAMPLE: 
		/// <add key="jQueryValidateLatest" value="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate.min.js"/>
		/// </summary>
		public static void IncludejQueryValidate() {
			IncludejQueryValidate(null);
		}
		public static void IncludejQueryValidate(Page sender) {
			if (Util.GetSettingBool("UseSavvyValidateInsteadOfjQueryValidate", false)) {
				Util.IncludeJavascriptFile(sender, Web.Root + "js/BewebCore/Savvy.validate.js");
				if (FileSystem.FileExists(Web.Root + "js/CustomValidation.js")) {
					Util.IncludeJavascriptFile(sender, Web.Root + "js/CustomValidation.js");
				}
			} else {
				//Util.IncludeJavascriptFile(sender, ConfigurationManager.AppSettings.Get("jQueryLatest"));
				string js = Util.GetSetting("jQueryValidateLatest", Web.Root + "js/jquery.validate-1.6/jquery.validate.js");
				Util.IncludeJavascriptFile(sender, js);
				Util.IncludeJavascriptFile(sender, Web.Root + "js/BewebCore/Beweb.jquery.validate.js");
			}
		}

		/// <summary>
		/// Render script and CSS tags including jQuery UI library. 
		/// The version to be included is specified in the Web_AppSettings.config.
		/// This can be either CDN hosted or a local file.
		/// EXAMPLE: 
		/// <add key="jQueryUILatest" value="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/jquery-ui.min.js"/><!-- includes complete jQuery UI -->
		///	<add key="jQueryUILatestCSS" value="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/themes/ui-darkness/jquery-ui.css"/><!-- includes complete jQuery UI CSS and images -->
		///	(For a visual reference to all standard theme names see http://www.asp.net/ajaxlibrary/CDNjQueryUI1814.ashx)
		/// </summary>
		public static void IncludeJQueryUI() {
			IncludeJQueryUI(HttpContext.Current.Handler as System.Web.UI.Page, "redmond");
		}

		public static void IncludeJQueryUI(Page sender) {
			IncludeJQueryUI(sender, "redmond");
		}

		public static void IncludeJQueryUI(Page sender, string themeName) {
			IncludeJQueryUI(sender, themeName, IncludeRenderMode.Auto);
		}

		public static void IncludeJQueryUI(Page sender, Util.IncludeRenderMode includeRenderMode) {
			IncludeJQueryUI(sender, "redmond", includeRenderMode);
		}

		public static void IncludeJQueryUI(Page sender, string themeName, Util.IncludeRenderMode includeRenderMode) {
			string js = Util.GetSetting("jQueryUILatest", Web.Root + "js/jquery-ui-1.8.4/js/jquery-ui-1.8.4.custom.min.js");
			string css = Util.GetSetting("jQueryUILatestCSS", Web.Root + "js/jquery-ui-1.8.4/css/" + themeName + "/jquery-ui-1.8.4.css");

			Util.IncludeStylesheetFile(sender, css, includeRenderMode);
			Util.IncludeJavascriptFile(sender, js, includeRenderMode);
			// switch to HTTPS mode if we are on HTTPS page
			if (Web.IsAbsoluteUrl(js) && Web.IsSecureConnection) {
				js = js.Replace("http://", "https://");
			}
			if (js.Contains("ajax.aspnetcdn.com/ajax/jquery.ui")) {
				// using microsoft CDN, so auto switch to google in case it is down
				js = js.Replace("ajax.aspnetcdn.com/ajax/jquery.ui", "ajax.googleapis.com/ajax/libs/jqueryui");
				IncludeJavascript(sender, "window.jQuery || document.write('<script src=\"" + js + "\"><\\/script>')", true, includeRenderMode);
			} else if (js.Contains("ajax.googleapis.com/ajax/libs/jqueryui")) {
				// using google CDN, so auto switch to microsoft in case it is down
				js = js.Replace("ajax.googleapis.com/ajax/libs/jqueryui", "ajax.aspnetcdn.com/ajax/jquery.ui");
				IncludeJavascript(sender, "window.jQuery || document.write('<script src=\"" + js + "\"><\\/script>')", true, includeRenderMode);
			}

			//Util.IncludeJavascriptFile(sender, ConfigurationManager.AppSettings.Get("jQueryLatest"));
			//Util.IncludeJavascriptFile(sender, Web.Root+"/js/jquery-ui-1.8.1.autocomplete/js/jquery-1.4.2.min.js");
			//Util.IncludeJavascriptFile(sender, Web.Root+"js/jquery-ui-1.8.4/js/jquery-ui-1.8.4.custom.min.js");

			// we hardly ever use timepicker, so if you delete the js file it will just skip it
			if (FileSystem.FileExists(Web.Root + "js/jquery-ui-1.8.4/js/timepicker.js")) Util.IncludeJavascriptFile(sender, Web.Root + "js/jquery-ui-1.8.4/js/timepicker.js", includeRenderMode);

			//Util.IncludeStylesheetFile(sender, Web.Root+"js/jquery-ui-1.8.4/css/"+themeName+"/jquery-ui-1.8.4.css");

		}
		public static void IncludeBewebForms() {
			IncludeBewebForms(null);
		}

		public static void IncludeBewebForms(Page sender) {
			IncludeBewebForms(sender, IncludeRenderMode.Auto);
		}

		public static void IncludeBewebForms(Page sender, Util.IncludeRenderMode includeRenderMode) {
			Util.IncludeJavascriptFile(sender, Web.Root + "js/BewebCore/forms.js", includeRenderMode);
			Util.IncludeJavascriptFile(sender, Web.Root + "js/BewebCore/beweb-cma.js", includeRenderMode);
		}

		#region IncludeGoogleMaps
		public static void IncludeGoogleMaps(Page sender) {
			// if DEV look up host specific extension
			string settingName = "GoogleApi";
			string apiKey = GetNamedSetting(settingName, "", false);
			string hostName = HttpContext.Current.Request.Url.Host.ToUpper();
			if (String.IsNullOrEmpty(apiKey)) {
				apiKey = ConfigurationManager.AppSettings.Get(settingName + hostName);
			}

			if (String.IsNullOrEmpty(apiKey)) {
				throw new Exception(String.Format("no valid Google Maps API key found in AppSettings for [{0}{1}]", settingName, hostName));
			}

			Util.IncludeJavascriptFile(sender, "http://maps.google.com/maps?file=api&v=2&key=" + apiKey);
		}
		#endregion

		#region IncludeJavascript
		/// <summary>
		/// WARNING: UNTESTED
		/// will include a chunk of script in the head of a page and wraps it in script tags - does NOT check if a similar block already exists
		/// </summary>
		/// <param name="sender">pass 'this' or 'this.Page'</param>
		/// <param name="script">javascript code without wrapping script tags</param>
		public static void IncludeJavascript(string script) {
			IncludeJavascript(HttpContext.Current.Handler as System.Web.UI.Page, script);
		}

		/// <summary>
		/// will include a chunk of script in the head of a page and wraps it in script tags - does NOT check if a similar block already exists
		/// </summary>
		/// <param name="sender">pass 'this' or 'this.Page'</param>
		/// <param name="script">javascript code without wrapping script tags</param>
		public static void IncludeJavascript(Page sender, string script) {
			IncludeJavascript(sender, script, true);
		}

		public static void IncludeJavascript(Page sender, string script, bool addScriptTags) {
			IncludeJavascript(sender, script, addScriptTags, IncludeRenderMode.Auto);
		}

		//
		/// <summary>
		/// will include a chunk of script in the head of a page - does NOT check if a similar block already exists
		/// </summary>
		/// <param name="sender">pass 'this' or 'this.Page'</param>
		/// <param name="script"></param>
		/// <param name="addScriptTags"></param>
		//public static void IncludeJavascript(string script, bool addScriptTags)
		//{
		//  IncludeJavascript(HttpContext.Current.Handler as System.Web.UI.Page, script, addScriptTags);
		//}
		public static void IncludeJavascript(Page sender, string script, bool addScriptTags, IncludeRenderMode includeRenderMode) {
			if (addScriptTags && !script.TrimStart('\r', '\n', '\t', ' ').StartsWith("<script")) {      // MN 20120201 !script.Contains("<script")) {
				script = @"
				<script type='text/javascript' charset='utf-8'>
					" + script + @"
				</script>
				";
			} else {
				//check for script tags
				if (script.ToLower().IndexOf("<script") == -1) {
					throw new Exception("included script, but not in a script tag");
				}
			}

			// figure out Auto mode
			if (includeRenderMode == IncludeRenderMode.Auto) {
				includeRenderMode = GetAutoRenderMode(sender);
			}
			//includeRenderMode = IncludeRenderMode.Head;

			if (includeRenderMode == IncludeRenderMode.Head) {
				// test header is runat server
				if (sender.Page.Header == null) {
					throw new Exception(
						"Beweb.Util.IncludeJavascriptFile: You have to add the attribute RUNAT=SERVER to your HEAD tag (probably in the Master Page), or specify Inline mode.");
				}

				Literal thisInclude = new Literal();
				thisInclude.Text = script;
				/*
				HtmlGenericControl thisInclude = new HtmlGenericControl();
				thisInclude.TagName = "div";
				thisInclude.Attributes.Add("class", "hidden");
				thisInclude.InnerHtml=script;
				*/
				AddToPageHead(sender, thisInclude);

			} else {
				// include inline or foot
				IncludeFragment(script, includeRenderMode);
			}
		}

		#endregion

		#region IncludeJavascriptFile

		/// <summary>
		/// will include a javascript file (auto-detects whether to put either in the head of a page or inline)
		/// <param name="file">the filename to include, if it starts with ~ ResolveUrl will be run on it</param>
		/// </summary>
		public static void IncludeJavascriptFile(string file) {
			IncludeJavascriptFile(HttpContext.Current.Handler as System.Web.UI.Page, file, IncludeRenderMode.Auto);
		}

		/// <summary>
		/// will include a javascript file (either in the head of a page or inline depending on second parameter)
		/// </summary>
		/// <param name="file">the filename to include, if it starts with ~ ResolveUrl will be run on it</param>
		/// <param name="includeRenderMode">Auto, Head, Inline, Foot</param>
		public static void IncludeJavascriptFile(string file, IncludeRenderMode includeRenderMode) {
			IncludeJavascriptFile(HttpContext.Current.Handler as System.Web.UI.Page, file, includeRenderMode);
		}

		/// <summary>
		/// Include a javascript file in the head of a page
		/// </summary>
		/// <param name="sender">just pass "this" or "this.Page"</param>
		/// <param name="file">the filename to include, if it starts with ~ ResolveUrl will be run on it</param>
		public static void IncludeJavascriptFile(Page sender, string file) {
			IncludeJavascriptFile(sender, file, IncludeRenderMode.Auto);
		}

		/// <summary>
		/// Include a javascript file in the head of a page
		/// </summary>
		/// <param name="sender">just pass "this" or "this.Page"</param>
		/// <param name="file">the filename to include, if it starts with ~ ResolveUrl will be run on it</param>
		/// <param name="includeRenderMode">Auto, Head, Inline, Foot</param>
		public static void IncludeJavascriptFile(Page sender, string file, IncludeRenderMode includeRenderMode) {
			string fileWithPath = file.StartsWith("~") ? Web.ResolveUrl(file) : file;

			// use full version if DEV server
			bool isAbsoluteUrl = Web.IsAbsoluteUrl(fileWithPath);
			if (!Util.ServerIsLive && Util.GetSettingBool("SubstituteMinifiedWithFull", true) && (fileWithPath.ToLower().EndsWith(".min.js") || fileWithPath.ToLower().EndsWith("-min.js"))) {
				string fileWithPathFullVersion = fileWithPath;
#if BaseTypeExtensions
				fileWithPathFullVersion = fileWithPathFullVersion.ReplaceLast(".min.js", ".js");
				fileWithPathFullVersion = fileWithPathFullVersion.ReplaceLast("-min.js", ".js");
#endif
				if (!isAbsoluteUrl && !File.Exists(Web.MapPath(fileWithPathFullVersion))) {
#if Debug
					Beweb.Logging.trace("Could not find full version of minified js file [" + fileWithPath + "]. Full version will be used automatically on DEV servers if available.");
#endif
				} else {
					// use full version if available
					fileWithPath = fileWithPathFullVersion;
				}
			}

			// 20111114 MK / MN - breaking change
			// switch to HTTPS mode if we are on HTTPS page
			if (isAbsoluteUrl && HttpContext.Current.Request.IsSecureConnection) {
				// note: this will break some script includes if they don't just have https vesion (some CDN's?) - either change your include or add a special case here
				// however, it would otherwise show a mixed content some resources are insecure error, so this breaking change is justified
				fileWithPath = fileWithPath.Replace("http://", "https://");
			}

			if (!isAbsoluteUrl) {
				// local file - check file exists
				if (!File.Exists(Web.MapPath(fileWithPath))) {
					throw new Exception("Failed to load js file [" + fileWithPath + "] - server is [" + Util.ServerIs() + "]. Could not find file. Note: On dev, make sure you have full version available as well as min.");
				} else {
					// yes, file exists, now add cache busting 
					// (This caching technique is known as Url Fingerprinting - if you include all your js files this way you can set far future Expires date in web.config)
					if (fileWithPath.DoesntContain("?")) {
						if (Web.Request["cachebust"] != null || Web.Session["cachebust"] != null || !Util.ServerIsDev) {  //20150218jn added - turn on dev cachebust
							var modified = File.GetLastWriteTime(Web.MapPath(fileWithPath));
							Web.Session["cachebust"] = true;
							fileWithPath += "?v=" + Fmt.DateTimeCompressed(modified);
						} else {
							fileWithPath += "?v=devmodeNoDatestamp";
						}
					}
				}
			}

			// figure out Auto mode
			if (includeRenderMode == IncludeRenderMode.Auto) {
				includeRenderMode = GetAutoRenderMode(sender);
			}

			if (includeRenderMode == IncludeRenderMode.Head) {
				// test header is runat server
				if (sender.Page.Header == null) {
					throw new Exception("Beweb.Util.IncludeJavascriptFile: You have to add the attribute RUNAT=SERVER to your HEAD tag (probably in the Master Page), or specify Inline mode.");
				}

				if (!FileIsIncluded(sender.Page.Header.Controls, "src", fileWithPath)) {
					HtmlGenericControl thisInclude = new HtmlGenericControl();
					thisInclude.TagName = "script";
					thisInclude.Attributes.Add("type", "text/javascript");

					thisInclude.Attributes.Add("src", fileWithPath);
					AddToPageHead(sender, thisInclude);
				}
			} else {
				// include inline or foot
				string extraAttribs = "";
				List<string> deferList = Util.GetSettingPipeList("DeferJavaScriptLoading", "(none)");
				if (!Web.IsOldBrowser) {
					if (deferList.Contains("(all)") || deferList.Exists(s => fileWithPath.ContainsInsensitive(s))) {
						extraAttribs += " defer";
					}
					List<string> asyncList = Util.GetSettingPipeList("AsyncJavaScriptLoading", "(none)");
					if (asyncList.Contains("(all)") || asyncList.Exists(s => fileWithPath.ContainsInsensitive(s))) {
						extraAttribs += " defer";
					}
				}

				string htmlFragment = "<script type=\"text/javascript\" src=\"" + fileWithPath + "\"" + extraAttribs + "></script>\n\t\t";
				IncludeFragment(htmlFragment, includeRenderMode);
			}
		}

		#endregion

		#region "inline file including"

		public enum IncludeRenderMode { Auto, Head, Inline, Foot }

		private static IncludeRenderMode GetAutoRenderMode(Page sender) {
			// figure out Auto mode
			IncludeRenderMode includeRenderMode;
			if (sender == null && HttpContext.Current.Handler is Page) {
				// if it is a webforms page (does not work on MVC)
				// todo: try calling PageParser.GetCompiledPageInstance(virtualPath,aspxFileName,Context)
				sender = HttpContext.Current.Handler as Page;
			}
			if (sender == null || sender.Page.Header == null) {
				// cannot find page reference or does not have runat=server on HEAD tag
				includeRenderMode = IncludeRenderMode.Inline;
			} else {
				includeRenderMode = IncludeRenderMode.Head;
			}
			return includeRenderMode;
		}

		private static void IncludeFragment(string htmlFragment, IncludeRenderMode includeRenderMode) {
			List<string> includedScriptFragments = (List<string>)Web.PageGlobals["IncludedScriptFragments"];
			if (includedScriptFragments == null) {
				includedScriptFragments = new List<string>();
				Web.PageGlobals["IncludedScriptFragments"] = includedScriptFragments;
			}
			if (!includedScriptFragments.Contains(htmlFragment)) {
				includedScriptFragments.Add(htmlFragment);
				Web.Write("\r\n\t\t" + htmlFragment + "\r\n");
			}
		}
		#endregion

		#region FileIsIncluded
		private static bool FileIsIncluded(ControlCollection cc, string attributeToLookIn, string fileToLookFor) {
			bool isAlreadyIncluded = false;
			foreach (Control c in cc) {
				if (c.GetType().ToString().StartsWith("System.Web.UI.HtmlControls")) {
					HtmlControl hc = (HtmlControl)c;
					if (hc.Attributes[attributeToLookIn] != null && hc.Attributes[attributeToLookIn].ToLower() == fileToLookFor.ToLower()) {
						isAlreadyIncluded = true;
						break; // found it - no need to keep looping
					}
				} else if (c.GetType().ToString() == "System.Web.UI.LiteralControl"
						|| c.GetType().ToString() == "System.Web.UI.WebControls.Literal"
						|| c.GetType().ToString() == "System.Web.UI.ResourceBasedLiteralControl") {
					string literalText = "";
					switch (c.GetType().ToString()) {
						case "System.Web.UI.LiteralControl":
						case "System.Web.UI.ResourceBasedLiteralControl":
							literalText = ((LiteralControl)c).Text;
							break;
						case "System.Web.UI.WebControls.Literal":
							literalText = ((Literal)c).Text;
							break;
					}

					string pattern = attributeToLookIn + @"\W*=\W*[""']" + fileToLookFor;
					Match m = Regex.Match(literalText, pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
					isAlreadyIncluded = m.Success;
					if (isAlreadyIncluded) break; // found it - no need to keep looping
				} else if (c.GetType().ToString().StartsWith("System.Web.UI.WebControls.ContentPlaceHolder")) {
					// dig into it find out what's inside, using recursion
					isAlreadyIncluded = FileIsIncluded(c.Controls, attributeToLookIn, fileToLookFor);
				} else if (c.GetType().ToString().StartsWith("System.Web.UI.WebControls.PlaceHolder")) {
					// dig into it find out what's inside, using recursion
					isAlreadyIncluded = FileIsIncluded(c.Controls, attributeToLookIn, fileToLookFor);
				} else {
					//throw new Exception("FileIsIncluded error: unknown type: " + c.GetType());
				}
			}

			return isAlreadyIncluded;
		}
		#endregion

		#region IncludeStylesheetFile
		/// <summary>
		/// will include a stylesheet file in the head of a page
		/// </summary>
		/// <param name="sender">just pass 'this'</param>
		/// <param name="file">the filename to include, if it starts with ~ ResolveUrl will be run on it</param>
		public static void IncludeStylesheetFile(string file) {
			IncludeStylesheetFile(HttpContext.Current.Handler as System.Web.UI.Page, file);
		}
		public static void IncludeStylesheetFile(Page sender, string file) {
			IncludeStylesheetFile(sender, file, null);
		}
		public static void IncludeStylesheetFile(string file, string mediaType) {
			IncludeStylesheetFile(null, file, mediaType);
		}

		public static void IncludeStylesheetFile(Page sender, string file, string mediaType) {
			IncludeStylesheetFile(sender, file, mediaType, IncludeRenderMode.Auto);
		}

		public static void IncludeStylesheetFile(Page sender, string file, Util.IncludeRenderMode includeRenderMode) {
			IncludeStylesheetFile(sender, file, null, includeRenderMode);
		}

		public static void IncludeStylesheetFile(Page sender, string file, string mediaType, Util.IncludeRenderMode includeRenderMode) {
			string fileWithPath = file.StartsWith("~") ? Web.ResolveUrl(file) : file;
			// use full version if DEV server
			bool isAbsoluteUrl = Web.IsAbsoluteUrl(fileWithPath);
			if (!Util.ServerIsLive && Util.GetSettingBool("SubstituteMinifiedWithFull", true) && (fileWithPath.ToLower().EndsWith(".min.css") || fileWithPath.ToLower().EndsWith("-min.css"))) {
				string fileWithPathFullVersion = fileWithPath;
#if BaseTypeExtensions
				fileWithPathFullVersion = fileWithPathFullVersion.ReplaceLast(".min.css", ".css");
				fileWithPathFullVersion = fileWithPathFullVersion.ReplaceLast("-min.css", ".css");
#endif
				if (!isAbsoluteUrl && !File.Exists(Web.MapPath(fileWithPathFullVersion))) {
#if Debug
					Beweb.Logging.trace("Could not find full version of minified CSS file [" + fileWithPath + "]. Full version will be used automatically on DEV servers if available.");
#endif
				} else {
					// use full version if available
					fileWithPath = fileWithPathFullVersion;
				}
			}

			// 20120119 MN - breaking change
			// switch to HTTPS mode if we are on HTTPS page
			if (isAbsoluteUrl && HttpContext.Current.Request.IsSecureConnection) {
				// note: this will break some script includes if they don't just have https vesion (some CDN's?) - either change your include or add a special case here
				// however, it would otherwise show a mixed content some resources are insecure error, so this breaking change is justified
				fileWithPath = fileWithPath.Replace("http://", "https://");
			}

			if (!isAbsoluteUrl) {
				// local file - add cache busting 
				// (This caching technique is known as Url Fingerprinting - if you include all your js files this way you can set far future Expires date in web.config)
				fileWithPath = FileUrlWithFingerprint(fileWithPath);
			}

			if (sender == null || includeRenderMode == IncludeRenderMode.Inline) {
				string html = "<link href=\"" + fileWithPath + "\" rel=\"stylesheet\" type=\"text/css\"";
				if (!String.IsNullOrEmpty(mediaType)) {
					html += " media=\"" + mediaType + "\"";
				}
				html += " />";
				IncludeFragment(html, includeRenderMode);
			} else if (sender.Page.Header != null && !FileIsIncluded(sender.Page.Header.Controls, "href", fileWithPath)) {
				HtmlLink thisInclude = new HtmlLink();
				thisInclude.Href = fileWithPath;
				thisInclude.Attributes.Add("rel", "stylesheet");
				thisInclude.Attributes.Add("type", "text/css");
				if (!String.IsNullOrEmpty(mediaType)) {
					//html += " media=\"" + mediaType + "\"";
					thisInclude.Attributes.Add("media", mediaType);
				}
				if (!String.IsNullOrEmpty(mediaType)) thisInclude.Attributes.Add("media", mediaType);
				AddToPageHead(sender, thisInclude);
			}
		}

		public static string FullUrlWithFingerprint(string file) {
			return Web.ResolveUrlFull(FileUrlWithFingerprint(file));
		}


		public static string FileUrlWithFingerprint(string file) {
			string fileWithPath = file.StartsWith("~") ? Web.ResolveUrl(file) : file;
			if (fileWithPath.DoesntContain("?")) {
				//var modified = File.GetLastWriteTime(Web.MapPath(fileWithPath));

				if (Web.Request["cachebust"] != null || Web.Session["cachebust"] != null || !Util.ServerIsDev) {  //20150218jn added - turn on dev cachebust
					var modified = File.GetLastWriteTime(Web.MapPath(fileWithPath));
					Web.Session["cachebust"] = true;

					fileWithPath += "?v=" + Fmt.DateTimeCompressed(modified);
				} else {
					fileWithPath += "?v=devmodeNoFingerprint";
				}
			}
			return fileWithPath;
		}

		#endregion

		#region IncludeLinkRelTag
		/// <summary>
		/// add a link rel tag to the page header
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="rel"></param>
		/// <param name="file"></param>
		public static void IncludeLinkRelTag(Page sender, string rel, string file) {
			string fileWithPath = file.StartsWith("~") ? Web.ResolveUrl(file) : file;

			if (!FileIsIncluded(sender.Page.Header.Controls, "href", fileWithPath)) {
				HtmlLink thisInclude = new HtmlLink();
				thisInclude.Href = fileWithPath;
				thisInclude.Attributes.Add("rel", rel);
				AddToPageHead(sender, thisInclude);
			}
		}
		#endregion

		#region IncludeMetaTag
		/// <summary>
		/// will include a meta tag in the head of a page
		/// </summary>
		/// <param name="sender">just pass "this" or "this.Page"</param>
		/// <param name="Name">the name attribute of the metatag i.e. "keywords" or "description"</param>
		/// <param name="Content">the value of the content attribute or the metatag</param>
		public static void IncludeMetaTag(Page sender, string Name, string Content) {
			// check that this is not already included
			bool isAlreadyIncluded = false;
			for (int i = 0; i < sender.Page.Header.Controls.Count; i++) {
				if (sender.Page.Header.Controls[i].GetType() == typeof(HtmlMeta)) {
					HtmlMeta hm = (HtmlMeta)sender.Page.Header.Controls[i];
					if (hm.Name == Name && hm.Content == Content) {
						isAlreadyIncluded = true;
						break; // no need to keep checking
					}
				}
			}

			if (!isAlreadyIncluded) {
				HtmlMeta thisMetaTag = new HtmlMeta();
				thisMetaTag.Name = Name;
				thisMetaTag.Content = Content;
				AddToPageHead(sender, thisMetaTag);
			}
		}
		#endregion

		#region AddToPageHead
		protected static void AddToPageHead(Page sender, Control c) {
			// test header is runat server
			if (sender.Page.Header == null) {
				throw new Exception("Beweb.Util.AddToPageHead: Page.Header is null. You probably have to add the attribute RUNAT=SERVER to your HEAD tag (probably in the Master Page).");
			}

			var container = sender.Page.Header.FindControl("HeadFileIncludes");
			if (container != null) {
				container.Controls.AddAt(container.Controls.Count, c);      // add it last
			} else {
				// add this as the second-to-last thing in the header - our HeadContent needs to go last
				container = sender.Page.Header;
				container.Controls.AddAt(container.Controls.Count - 1, c);
			}
		}

		#endregion

		#endregion
		/// <summary>
		/// gets the html contents of the requested url
		/// </summary>
		/// <param name="url">The url of the html page </param>
		/// <returns>string represetation of the html page</returns>
		public static string GetHtmlPageAsString(string url) {
			string strResult;
			WebRequest objRequest = HttpWebRequest.Create(url);
			WebResponse objResponse = objRequest.GetResponse();
			using (StreamReader sr = new StreamReader(objResponse.GetResponseStream())) {
				strResult = sr.ReadToEnd();
				sr.Close();
			}
			return strResult;
		}
		/// <summary>
		/// get the req value from the crazy asp content container 
		/// eg ctl00$BodyContent$City - get value of Request["City"]
		/// </summary>
		/// <param name="colName">name of col to find in the request object</param>
		/// <param name="req">request object</param>
		/// <returns>value of a form element with the given colname as it's id. if the form var was not passed, return null</returns>
		public static string GetRequestValue(string colName, HttpRequest req) {
			string result = req.Form[colName];								// value from form
			if (result == null) {
				result = req.Form[colName + "$" + colName];	// try the crazy 'asp:' control name
			}
			if (result == null)		 //still not found, try even crazier asp: tag in a container name
			{
				foreach (string reqKey in req.Form.Keys) {
					int posn = reqKey.LastIndexOf('$');
					if (posn != -1 && reqKey.Substring(posn + 1).ToLower() == colName.ToLower()) {
						result = req.Form[reqKey];
						break;
					}

					// try "ctl00$BodyContent$DevAccess$curCheck" looking for DevAccess - handle checkboxes
					if (reqKey == "ctl00$BodyContent$" + colName + "$curCheck") {
						result = (req.Form["ctl00$BodyContent$" + colName + "$curCheck"] == "on") ? "1" : "0";
						break;
					}
				}
			}
			return result;
		}
		public static int GetRandomInt(int min, int max) {
			byte[] randomBytes = new byte[4];

			var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes[0] & 0x7f) << 24 |
									randomBytes[1] << 16 |
									randomBytes[2] << 8 |
									randomBytes[3];

			// Now, this is real randomization.
			Random random = new Random(seed);
			return random.Next(min, max);
		}

		#region AppendToQueryString
		/// <summary>
		/// adds a name/value pair to a URL which may or may not already contain a QueryString
		/// </summary>
		/// <param name="Url">the existing URL</param>
		/// <param name="Name"></param>
		/// <param name="Value">you need to urlencode this value</param>
		/// <returns></returns>
		public static string AppendToQueryString(string Url, string Name, string Value) {
			string appendFormat = "{0}?{1}={2}";
			if (Url.Contains("?")) {
				appendFormat = "{0}&{1}={2}";
			}

			return String.Format(appendFormat, Url, Name, Value); ;
		}

		#endregion

		#region ifnull
		/// <summary>
		/// if expression1 is not blank, return it, else return expression2 (eg ifnull(rs['hey'],request['hey']))
		/// </summary>
		/// <param name="expression1"></param>
		/// <param name="expression2"></param>
		/// <returns></returns>
		public string ifnull(string expression1, string expression2) {
			return (expression1 != "") ? expression1 : expression2;
		}

		#endregion

		#region DefaultValue
		/// <summary>
		/// Use first param unless it's null or blank in which case use the second param
		/// </summary>
		/// <param name="sourceValue"></param>
		/// <param name="defaultSourceValue"></param>
		/// <returns></returns>
		public static string DefaultValue(string sourceValue, string defaultSourceValue) {
			return (!String.IsNullOrEmpty(sourceValue)) ? sourceValue : defaultSourceValue;
		}

		/// <summary>
		/// if source value is not zero, return it else return defaultSourceValue
		/// </summary>
		/// <param name="sourceValue"></param>
		/// <param name="defaultSourceValue"></param>
		/// <returns></returns>
		public static int DefaultValue(int sourceValue, int defaultSourceValue) {
			return (sourceValue != 0) ? sourceValue : defaultSourceValue;
		}

		#endregion

		#region AppSettings access
		/// <summary>
		/// Get a compressed version of the web site name, good for cookie prefixes
		/// </summary>
		/// <returns></returns>
		public static string GetSiteCodeName() {
			return ConfigurationManager.AppSettings["SiteName"].Replace(" ", "").ToLower();
		}
		/// <summary>
		/// Get the site name from the web config
		/// </summary>
		/// <returns></returns>
		public static string GetSiteName() {
			return ConfigurationManager.AppSettings["SiteName"];
		}

		/// <summary>
		/// Read a setting from the web.config - calls GetNamedSetting
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or throw exception if missing</returns>
		[Obsolete("You must specify the default in case the config value is missing")]
		public static string GetSetting(string settingName) {
			return GetNamedSetting(settingName, "throw");
		}

		public static string GetRequiredSetting(string settingName) {
			return GetNamedSetting(settingName, "throw");
		}

		/// <summary>
		/// Read a setting from the web.config - will look for a -dev, stg, live appended if available
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or specified defaultValue if missing or blank</returns>
		public static string GetSetting(string settingName, string defaultValue) {
			return GetNamedSetting(settingName, defaultValue);
		}

		public static int GetSettingInt(string settingName, int defaultValue) {
			return GetNamedSetting(settingName, defaultValue + "").ToInt(defaultValue);
		}

		private static List<string> GetSettingPipeList(string settingName, string defaultValue) {
			var pipeSeparatedString = GetNamedSetting(settingName, defaultValue);
			return pipeSeparatedString.Split('|').ToList(s => s);
		}

		/// <summary>
		/// Read a setting from the web.config - calls GetNamedSetting
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or throw exception if missing</returns>
		[Obsolete("You must specify the default in case the config value is missing")]
		public static bool GetSettingBool(string settingName) {
			return Convert.ToBoolean(GetNamedSetting(settingName, "throw"));
		}

		public static bool GetRequiredSettingBool(string settingName) {
			return Convert.ToBoolean(GetNamedSetting(settingName, "throw"));
		}

		/// <summary>
		/// Read a setting from the web.config - calls GetNamedSetting
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or specified defaultValue if missing</returns>
		public static bool GetSettingBool(string settingName, bool defaultValue) {
			return Convert.ToBoolean(GetNamedSetting(settingName, defaultValue.ToString()));
		}

		/// <summary>
		/// Read a setting from the web.config - will look for a -dev, stg, live appended if available
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or throw exception if missing or blank</returns>
		[Obsolete("You must specify the default in case the config value is missing")]
		public static string GetNamedSetting(string settingName) {
			return GetNamedSetting(settingName, "throw");
		}

		/// <summary>
		/// Read a setting from the web.config - will look for a -dev, stg, live appended if available
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <returns>value, or throw exception if missing or blank</returns>
		public static string GetRequiredNamedSetting(string settingName) {
			return GetNamedSetting(settingName, "throw");
		}

		/// <summary>
		/// Read a setting from the web.config - will look for a -dev, stg, live appended if available
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <param name="defaultValue">default Value</param>
		/// <returns>value, or specified defaultValue if missing or blank</returns>
		public static string GetNamedSetting(string settingName, string defaultValue) {
			return GetNamedSetting(settingName, defaultValue, true);
		}

		/// <summary>
		/// Read a setting from the web.config - will look for a -dev, stg, live appended if available
		/// </summary>
		/// <param name="settingName">setting to read</param>
		/// <param name="defaultValue">default Value</param>
		/// <param name="isTryLive">should it try the LVE suffix if it can't find another value</param>
		/// <returns>value, or specified defaultValue if missing or blank</returns>
		public static string GetNamedSetting(string settingName, string defaultValue, bool isTryLive) {
			if (HttpContext.Current != null) {
				// cache results just within current request for speed
				var setting = HttpContext.Current.Items["GetNamedSetting_" + settingName];
				if (setting != null) {
					return setting.ToString();
				}
			}

			string result = ConfigurationManager.AppSettings.Get(settingName + Web.Host + "");
			if (String.IsNullOrEmpty(result)) {
				result = ConfigurationManager.AppSettings.Get(settingName + Util.ServerIs() + "");
			}
			string useName = settingName;
			if (isTryLive && String.IsNullOrEmpty(result)) {
				// try live suffix
				useName = settingName + "LVE";
				result = ConfigurationManager.AppSettings.Get(useName);
			}
			if (String.IsNullOrEmpty(result)) {
				// try no suffix
				result = ConfigurationManager.AppSettings.Get(settingName);
			}
			if (String.IsNullOrEmpty(result)) {
				// use default value if setting still not found (or found but value is empty string)
				result = defaultValue;
				if (defaultValue == "throw") {
					throw new Exception("Missing config value for app settings key named [" + settingName + "]");
				}
			}
			if (HttpContext.Current != null) {
				HttpContext.Current.Items["GetNamedSetting_" + settingName] = result;
			}
			return result;
		}

		#endregion


		#region Base and Virtual Paths

#if FileSystem
		/// <summary>
		/// Locates a folder, which may be in the same folder as the current page or any parent folder above that
		/// pathNames can be a pipe separated list and can be a partial path containing slashes
		/// </summary>
		/// <param name="pathNames"></param>
		/// <returns></returns>
		public static string FindVirtualPath(string pathNames) {
			return FindVirtualPath(pathNames, true);
		}
		/// <summary>
		/// Locates a folder, which may be in the same folder as the current page or any parent folder above that
		/// pathNames can be a pipe separated list and can be a partial path containing slashes
		/// </summary>
		/// <param name="pathNames"></param>
		/// <param name="isCritical"></param>
		/// <returns></returns>
		public static string FindVirtualPath(string pathNames, bool isCritical) {
			//' locates a folder, which may be in the same folder as the current page or any parent folder above that
			//' pathNames can be a pipe separated list and can be a partial path containing slashes
			//dim fs, i, filename, vPath, result, path, isFound, pPath, isFolder, pathNameArray
			bool isFound = false;
			bool isFolder = false;
			//string pPath = HttpContext.Current.Server.MapPath(".");
			string pPath = HttpContext.Current.Request.PhysicalApplicationPath;

			pPath = pPath.Replace("/", "\\");		 //' in case we are on unix
			//FileSystem fs = new FileSystem();
			string vPath = "";
			string result = "";
			string[] pathNameArray = pathNames.Split('|');  // MN 20110523 - fixed this, it looks like never worked
			for (int i = 1; i < 99; i++) {
				foreach (string path in pathNameArray) {
					result = vPath + path.Trim();
					//'if fs.folderexists(server.mappath(result)) or fs.fileexists(server.mappath(result)) then 
					if (FileSystem.FolderExists(pPath + "\\" + path)) //then
					{
						isFolder = true;
						isFound = true;
						break;//exit for
					} else if (FileSystem.FileExists(pPath + "\\" + path))// then 
					{
						isFolder = false;
						isFound = true;
						break;//exit for
					}//end if			
				}//next
				if (isFound) break;// then exit for

				//' look up one level
				int pos = pPath.LastIndexOf("\\");
				if (pos == 0 || pos == -1) break;// then exit for
				pPath = pPath.Substring(0, pos - 1);
				vPath = vPath + "../";//'
			}//next
			if (isFound && isFolder)// then
			{
				result = result + "/";
			}//end if	
			if (!isFound)// then
			{
				result = "(not found)";
				if (isCritical)//then
				{
					throw new Exception("SAVVY CMS ERROR: FindVirtualPath - could not find path [" + pathNames + "]. Check that this folder actually exists and then try again.");
					//response.end
				}//end if
			}//end if
			//set fs = nothing
			return result;
		}
		/// <summary>
		/// Given a vpath with no ../ in it, return a physical path
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string FindPhysicalPath(string path) {
			return Web.MapPath(FindVirtualPath(path));

		}
		public static void SetReturnPage(int level) {
			HttpContext.Current.Session[GetSiteCodeName() + "breadcrumbs_pageURL_level" + level] = GetPageFileName();
		}

		/// <summary>
		/// NOTE: this does not work as well as Breadcrumbs.Current.GetReturnPage() - but for existing applications you should keep it as is since the behaviour has some differences
		/// get the address of the page we **were** on before here
		/// add a random variable on there too - for cache busting
		/// </summary>
		/// <returns>that address of the page we **were** on</returns>
		[Obsolete("Instead you should probably use Breadcrumbs.Current.GetReturnPage() but note that this is actually different logic")]
		public static string GetReturnPage() {
			string returnpage = "";
			//' determine calling page to return back to
			if (!String.IsNullOrEmpty(HttpContext.Current.Request["df_returnpage"])) {
				returnpage = HttpContext.Current.Request["df_returnpage"];
			} else {
				returnpage = HttpContext.Current.Request.ServerVariables["http_referer"];
				if (String.IsNullOrEmpty(returnpage) && HttpContext.Current.Session[GetSiteCodeName() + "breadcrumbs_pageURL_level2"] != null) {
					//' blank referer - use breadcrumb instead
					returnpage = HttpContext.Current.Session[GetSiteCodeName() + "breadcrumbs_pageURL_level2"].ToString();
				}
				if (String.IsNullOrEmpty(returnpage) && HttpContext.Current.Session[GetSiteCodeName() + "breadcrumbs_pageURL_level1"] != null) {
					//' blank referer - use breadcrumb instead - level 1
					returnpage = HttpContext.Current.Session[GetSiteCodeName() + "breadcrumbs_pageURL_level1"].ToString();
				}
				if (String.IsNullOrEmpty(returnpage)) {
					//' blank breadcrumb - use admin/default.aspx instead
					returnpage = Web.Root + "admin/";
				}
			}

			// todo - why are we appending this rnd to querystring?
			if (!String.IsNullOrEmpty(returnpage) && returnpage.IndexOf("rnd=x") == -1) {
				//returnpage += ((returnpage.IndexOf("?") == -1) ? "?" : "&") + "rnd=x" + (VB.rnd() * 1250) + "x";
			} else if (!String.IsNullOrEmpty(returnpage)) {
				int posn = returnpage.IndexOf("rnd=x");
				int posn2 = returnpage.IndexOf("x", posn + 6) + 1;
				//returnpage = returnpage.Substring(0, posn) + "rnd=x" + (VB.rnd() * 1250) + "x" + returnpage.Substring(posn2);
			}
			if (!String.IsNullOrEmpty(returnpage) && returnpage.IndexOf("login.aspx") != -1) {
				returnpage = Web.Root + "?mode=loginredir";//"default.aspx";
			}

			return returnpage;
		}

		//todo make GetAttachmentVPath more efficient
		public static string GetAttachmentVPath() {
			return Util.FindVirtualPath("attachments");
		}

		//todo make WebsiteVPath more efficient
		public static string WebsiteVPath() {
			string att = Util.FindVirtualPath("attachments");
			return att.Substring(0, att.Substring(0, att.Length - 2).LastIndexOf("/") + 1);
			//HttpContext.Current.Application.
		}

		/// <summary>
		/// get full path to admin: eg: http://www.sitename.com/admin/
		/// </summary>
		/// <returns>full path to admin</returns>
		///<example>eg: http://www.sitename.com/admin/</example>
		public static string GetFullAdminWebPath() {
			string port = "";
			string curport = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
			port = curport != "80" ? ":" + curport : "";
			return "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + "/" + FindVirtualPath("admin") + "";
		}

		public static string AdminVPath() {
			return FindVirtualPath("admin") + "";
		}



		public static string GetCurrentPage() {
			return HttpContext.Current.Request.ServerVariables["path_info"];
		}

		[Obsolete("Moved into FileSystem class")]
		public static string GetUniqueAttachmentFilename(string filename) {
			return GetUniqueFilename(Web.Root + "/attachments/", filename);
		}

		/// <summary>
		/// given a path and a filename, return a uniquely named filename that can go in that path
		/// </summary>
		/// <param name="virtualPath">path from root of web site (not drive) (server.mappath will be called internally)</param>
		/// <param name="filename">filename that may already exist</param>
		/// <returns>filename-i where i is a unique number</returns>
		[Obsolete("Moved into FileSystem class")]
		public static string GetUniqueFilename(string virtualPath, string filename) {
			return FileSystem.GetUniqueFilename(virtualPath, filename);

			// old method
			//string basename, ext;
			//filename = RemoveBadCharacters(filename);
			//if (File.Exists(BuildPath(virtualPath, filename)))
			//{
			//  FileInfo fo = new FileInfo(filename);
			//  basename = fo.Name.Substring(0, fo.Name.LastIndexOf(".")); 
			//  ext = fo.Extension;
			//  for (int scan = 1; scan < 999999; scan++)
			//  {
			//    filename = basename + "-" + scan + ext.ToLower();
			//    if (!File.Exists(BuildPath(virtualPath, filename))) break;
			//  }
			//}
			//return filename.Replace(' ','-');
		}

		/// <summary>
		/// remove characters from uploaded file that may interfere with the download by making a bad url
		///  /:*?"<>|`~!@#$%^&+={};
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		[Obsolete("Use FileSystem.CrunchFileName instead as it is better.")]
		private static string RemoveBadCharacters(string filename) {
			//  /:*?"<>|`~!@#$%^&+={};
			filename = filename.Replace(' ', '-');
			filename = filename.Replace('\'', '-');
			filename = filename.Replace("/", "-");
			filename = filename.Replace("'", "");
			filename = filename.Replace(":", "");
			filename = filename.Replace("*", "");
			filename = filename.Replace("?", "");
			filename = filename.Replace("<", "");
			filename = filename.Replace(">", "");
			filename = filename.Replace("|", "");
			filename = filename.Replace("`", "");
			filename = filename.Replace("~", "");
			filename = filename.Replace("!", "");
			filename = filename.Replace("@", "");
			filename = filename.Replace('#', '-');
			filename = filename.Replace("$", "");
			filename = filename.Replace("%", "");
			filename = filename.Replace("^", "");
			filename = filename.Replace("&", "");
			filename = filename.Replace("+", "");
			filename = filename.Replace("=", "");
			filename = filename.Replace("{", "");
			filename = filename.Replace("}", "");
			return filename;

		}

		public static string BuildPath(string virtualPath, string filename) {
			return Web.MapPath(virtualPath) + filename;
		}

		/// <summary>
		/// return the name of the page - not including the extension
		/// </summary>
		/// <returns></returns>
		public static string GetPageName() {
			//' returns page filename without extension eg "news"
			string pageName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"]; //dont use request directly (eg masterpages)
			pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
			pageName = pageName.Replace(".aspx", "");
			return pageName.ToLower();
		}

		/// <summary>
		/// returns page filename including extension eg "news.asp"
		/// </summary>
		/// <returns></returns>
		public static string GetPageFileName() {
			string pageName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
			pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
			return pageName.ToLower();
		}
#endif

		#endregion


		#region WebsiteBaseUrl
		/// <summary>
		/// returns the base Url of the site - use ~/ where possible - but sometimes you just want to know this
		/// Preferred that you use Web.BaseUrl
		/// </summary>
		/// <returns>http://server/application/</returns>
		[Obsolete("Use Web.BaseUrl")]
		public static string WebsiteBaseUrl() {
			string returnValue = "";
			HttpContext context = HttpContext.Current;
			int curport = context.Request.Url.Port;
			string port = (curport != 80) ? ":" + curport : "";
			string isHttps = (curport == 443) ? "s" : "";
			returnValue = String.Format("http{0}://{1}{2}{3}"
				, isHttps
				, context.Request.Url.Host.ToLower()
				, port
				, Util.ResolveUrl(Web.Root + "/")
				);

			return returnValue;
		}

		#endregion

		#region ResolveUrl
		/// <summary>
		/// ResolveUrl for when you don't have access to the page object
		/// Preferred that you use Web.ResolveUrl
		/// </summary>
		/// <param name="originalUrl"></param>
		/// <returns></returns>
		[Obsolete("Use Web.ResolveUrl")]
		public static string ResolveUrl(string originalUrl) {
			if (originalUrl == null)
				return null;

			// *** Absolute path - just return
			if (originalUrl.IndexOf("://") != -1)
				return originalUrl;

			// *** Fix up image path for ~ root app dir directory
			if (originalUrl.StartsWith("~")) {
				// chop off and save any querystring
				string mainUrl = originalUrl;
				string qs = String.Empty;
				if (originalUrl.Contains("?")) {
					mainUrl = originalUrl.Substring(0, originalUrl.IndexOf("?"));
					qs = originalUrl.Substring(originalUrl.IndexOf("?"));
				}
				return VirtualPathUtility.ToAbsolute(mainUrl) + qs;
			}

			return originalUrl;
		}

		#endregion

		/// <summary>
		/// If live, checks to make sure we are on the correct primary domain name (ie if site has multiple domains it redirects to the primary one).
		/// Primary (or canonical) domain and host (eg www or bare) is determined from Web App Setting "WebsiteBaseUrlLVE".
		/// If staging or dev, these checks are skipped.
		/// </summary>
		public static void EnsureCanonicalHost() {
			bool forceHttps = Util.GetSettingBool("ForceHttps", false);
			EnsureCanonicalHost(forceHttps);
		}

		/// <summary>
		/// If live, checks to make sure we are on the correct primary domain name (ie if site has multiple domains it redirects to the primary one).
		/// Primary (or canonical) domain and host (eg www or bare) is determined from Web App Setting "ServerIsLVE". (If multiple, first is assumed)
		/// If staging or dev, these checks are skipped.
		/// </summary>
		/// <param name="forceHttps"></param>
		public static void EnsureCanonicalHost(bool forceHttps) {
			// redirect to https if http, www if none, and from alias domains to cannonical domain
			if (Util.ServerIsLive) {
				bool wrongProtocol = forceHttps && !Web.IsSecureConnection;
				//string canonicalHost = Util.GetSetting("WebsiteBaseUrl").RightFromFirst("://").TrimEnd("/ ")+"/";
				//bool wrongHost = !canonicalHost.StartsWith(Web.Request.Url.Host+"/");
				string canonicalHost = GetPrimaryDomain();//Util.GetSetting("ServerIsLVE").LeftUntil("|");
				bool wrongHost = Web.Request.Url.Host != canonicalHost;
				if (wrongProtocol || wrongHost) {
					string protocol;
					if (wrongProtocol) {
						protocol = "https://";
					} else {
						protocol = Web.Protocol;
					}
					Web.RedirectPermanently(protocol + canonicalHost + "/" + Web.LocalUrl);
				}
			}
		}

		public static string GetPrimaryDomain() {
			string domain = Util.GetSetting("ServerIs" + ServerIs(), "throw").LeftUntil("|"); ;
			return domain;
		}

		/// <summary>
		/// Basic Helper for the select2 plugin, use the cssclass svySelect2 to apply select2 on any supported input. Check Common.js for Javascript init code.
		/// http://ivaynberg.github.io/select2/
		/// </summary>
		public static void IncludeSelect2() {
			IncludeSelect2(null);
		}

		/// <summary>
		/// Basic Helper for the select2 plugin, use the cssclass svySelect2 to apply select2 on any supported input. Check Common.js for Javascript init code.
		/// http://ivaynberg.github.io/select2/
		/// </summary>
		public static void IncludeSelect2(Page sender) {
			IncludeSelect2(sender, IncludeRenderMode.Auto);
		}

		/// <summary>
		/// Basic Helper for the select2 plugin, use the cssclass svySelect2 to apply select2 on any supported input. Check Common.js for Javascript init code.
		/// http://ivaynberg.github.io/select2/
		/// </summary>
		public static void IncludeSelect2(Page sender, Util.IncludeRenderMode includeRenderMode) {
			//TODO: needs a bit more than just adding the files, feel free to extend JC 20140324
			Util.IncludeStylesheetFile(sender, Web.Root + "js/select2/select2.css", includeRenderMode);
			Util.IncludeJavascriptFile(sender, Web.Root + "js/select2/select2.min.js", includeRenderMode);
		}

		/// <summary>
		/// Simpler syntax for include stylesheet or javascript file (auto detects from extension).
		/// Always cache busts, adds tilde at start if not there, and renders inline (ie Response.Writes it out).
		/// Designed for use in views. Do not use in codebehind.
		/// </summary>
		/// <param name="jsOrCssPath"></param>
		public static void Include(string jsOrCssPath) {
			if (!jsOrCssPath.StartsWith("http") && !jsOrCssPath.StartsWith("~") && !jsOrCssPath.StartsWith("/") && !jsOrCssPath.StartsWith(".")) {
				jsOrCssPath = "~/" + jsOrCssPath;
			}
			if (jsOrCssPath.Contains(".css")) {
				IncludeStylesheetFile(null, jsOrCssPath, null, IncludeRenderMode.Inline);
			} else {
				IncludeJavascriptFile(null, jsOrCssPath, IncludeRenderMode.Inline);
			}
		}

	}
}

#if TestExtensions
namespace BewebTest {
	[TestClass]
	internal class TestUtil {

		[TestMethod]
		public static void TestServerIs() {
			string expectedValue, actualValue;
			expectedValue = "DEV";
			actualValue = Util.ServerIs_Internal("drunk");
			Assert.AreEqual(expectedValue, actualValue);
			expectedValue = "DEV";
			actualValue = Util.ServerIs_Internal("dRuNk");
			Assert.AreEqual(expectedValue, actualValue);
			expectedValue = "LVE";
			actualValue = Util.ServerIs_Internal("whatver.com");
			Assert.AreEqual(expectedValue, actualValue);
			expectedValue = "LVE";
			actualValue = Util.ServerIs_Internal("some unknown host");
			Assert.AreEqual(expectedValue, actualValue);
		}



	}

}

#endif
