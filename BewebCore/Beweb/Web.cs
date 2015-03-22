#define PathAndFile
#define TestExtensions

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using Beweb;

namespace Beweb {
	/// <summary>
	/// Shortcut to HttpContext.Current, which allows you to access HTTP server related functions anywhere anytime.
	/// Provides access to Request, Response, Server, Cache, PageGlobals etc.
	/// </summary>
	public class Web {
		public static string AdminRoot = Web.Root + "admin/";

		public static string AdminHelp = Web.Root + "areas/admin/help/";

		/// <summary>
		/// return url path to attachments folder http://<site>/attachments/ with a trailing slash
		/// </summary>
		public static string Attachments = Web.Root + "Attachments/";       // MN 20120423 - no longer readonly

		public static string ClassicAttachments = Web.Root + "Classic/Attachments/"; // if this is a mixed site we need to be able to tell the difference

		private static string _theme;

		/// <summary>
		/// return url path to images folder http://<site>/images/ with a trailing slash
		/// </summary>
		public static string Images { get { return Web.Root + "images/"; } }

		/// <summary>
		/// return url path to the javascript folder http://<site>/js/ with a trailing slash
		/// </summary>
		public static string JS { get { return Web.Root + "js/"; } }

		/// <summary>
		/// return url path to the fonts folder http://<site>/fonts/ with a trailing slash
		/// </summary>
		public static string Fonts { get { return Web.Root + "fonts/"; } }

		/// <summary>
		/// return url path to images folder http://<site>/images/ with a trailing slash
		/// </summary>
		public static string ImagesFullUrl { get { return Web.ResolveUrlFull("~/images/"); } }

		/// <summary>
		/// return url path to specific theme folder with a trailing slash
		/// </summary>
		public static string Theme {
			get {
				if (_theme == null) {
					_theme = Util.GetSetting("Theme", "blanktheme");
				}
				return Web.Root + "theme/" + _theme + "/";
			}
		}

		/// <summary>
		/// return true if using a theme
		/// </summary>
		public static bool HasTheme { get { return _theme != "blanktheme"; } }

		/// <summary>
		/// Gets a request variable using the built in request collection (combines form and querystring variables together).
		/// It is generally better to use Web.RequestEx[] when expecting form variables as this will only look in the querystring if there is no form variable with the given name.
		/// Web.Request also has a number of properties (eg Web.Request.ContentType, Web.Request.Headers, Web.Request.Browser, etc).
		/// </summary>
		public static HttpRequest Request {
			get {
				try {
					return HttpContext.Current.Request;
				} catch (HttpException exception) {
					if (exception.Message == "Request is not available in this context") {
						throw new HttpException("Request is not available in this context. This may mean the page processing has not yet got up to the Request Initialisation phase. Perhaps this is being called from Application_Start or Begin_Request.");
					} else {
						// rethrow current exception
						throw;
					}
				}
				//return null; //never called
			}
		}

		public static TraceContext Trace {
			get { return HttpContext.Current.Trace; }
		}
		public static HttpResponse Response {
			get { return HttpContext.Current.Response; }
		}

		public static HttpServerUtility Server {
			get { return HttpContext.Current.Server; }
		}

		public static HttpApplicationState Application {
			get { return HttpContext.Current.Application; }
		}

		public static HttpSessionState Session {
			get { return HttpContext.Current.Session; }
		}

		//public static System.Web.Caching.Cache Cache {
		//	get { return HttpContext.Current.Cache; }
		//}

		public static WebCache Cache {
			get { return new WebCache(); }
		}

		public class WebCache : IEnumerable {
			public Cache InternalCache {
				get { return HttpContext.Current.Cache; }
			}
			public int Count {
				get { return InternalCache.Count; }
			}
			//public IDictionaryEnumerator GetEnumerator() {
			//	return InternalCache.GetEnumerator();
			//}
			public object Remove(string key) {
				if(!key.EndsWith(Util.ServerIs())) {
					key += Util.ServerIs();
				}
				return InternalCache.Remove(key);
			}
			public void Insert(string key, object obj, CacheDependency cacheDependency, DateTime expiration, TimeSpan cacheDuration) {
				key += Util.ServerIs();
				InternalCache.Insert(key, obj, cacheDependency, expiration, cacheDuration);
			}
			public object this[string key] {
				get {
					key += Util.ServerIs();
					return InternalCache[key];
				}
				set {
					key += Util.ServerIs();
					InternalCache[key] = value;
				}
			}
			public void ClearKeysStartingWith(string keyStart) {
				List<string> keys = new List<string>();
				// retrieve application Cache enumerator
				IDictionaryEnumerator enumerator = InternalCache.GetEnumerator();
				// copy all keys that currently exist in Cache
				while (enumerator.MoveNext()) {
					var key = enumerator.Key.ToString();
					if (key.StartsWith(keyStart)) {         //&& key.EndsWith(Util.ServerIs()) MN 20141022 - remove all regardless of serveris
						keys.Add(key);
					}
				}
				// delete every key from cache that matches
				for (int i = 0; i < keys.Count; i++) {
					Web.Cache.Remove(keys[i]);
				}
			}
			public void ClearAll() {
				CacheClearAll();
			}
			IEnumerator IEnumerable.GetEnumerator() {
				return InternalCache.GetEnumerator();
			}
		}

		/// <summary>
		/// Key/value storage for items that are globally available during the current page request lifetime. 
		/// This uses HttpContext.Current.Items for storage.
		/// </summary>
		public static System.Collections.IDictionary PageGlobals {
			get { return HttpContext.Current.Items; }
		}

		/// <summary>
		/// Shortcut for HttpContext.Current.Request.ServerVariables
		/// </summary>
		public static NameValueCollection ServerVars {
			get { return HttpContext.Current.Request.ServerVariables; }
		}

		/// <summary>
		/// pass the name of the variable you want, or use the HttpServerVariable class. example: ServerVar(HttpServerVariable.LocalAddress)
		/// </summary>
		/// <param name="variableName"></param>
		/// <returns></returns>
		public static string ServerVar(string variableName) {
			return HttpContext.Current.Request.ServerVariables[variableName];
		}

		/// <summary>
		/// Shortcut for HttpContext.Current.Request.Params which is a combined collection of QueryString, Form, ServerVariables and Cookies
		/// </summary>
		public static NameValueCollection Params {
			get { return HttpContext.Current.Request.Params; }
		}

		/// <summary>
		/// Returns the current page file name in lowercase (eg news.aspx). 
		/// Same as Util.GetPageFileName()
		/// </summary>
		public static string PageFileName {
			get {
				string pageName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
				if (pageName.Contains("/")) {
					pageName = pageName.Split('/').Last();
				}
				return pageName.ToLowerInvariant();
			}
		}

		public static string PageFileNameNoExtn {
			get {
				string pageName = Web.PageFileName;
				if (pageName.Contains(".")) {
					pageName = pageName.Substring(0, pageName.IndexOf("."));
				}
				return pageName;
			}
		}

		/// <summary>
		/// Shortcut for HttpContext.Current.Response.Write()
		/// </summary>
		/// <param name="str">String to output into the page html output stream</param>
		public static void Write(string str) {
			Response.Write(str);
		}

		public static void Write(object str) {
			Response.Write(str);
		}

		/// <summary>
		/// Shortcut for HttpContext.Current.Response.Flush()
		/// </summary>
		public static void Flush() {
			try { Response.Flush(); } catch (Exception) { }
		}

		/// <summary>
		/// Write out a string and flush the buffer (ie start sending the response back to the browser)
		/// </summary>
		/// <param name="str">String to output</param>
		public static void Flush(object str) {
			Response.Write(str);
			Response.Flush();
		}

		/// <summary>
		/// Sends headers to redirect to another page.
		/// Shortcut for HttpContext.Current.Response.Redirect().
		/// NOTE: It's better to use return Redirect or otherwise return null after calling this, otherwise execution will continue and you can lose sessions
		/// HTTP 301 (temporary redirection) 
		/// </summary>
		/// <param name="url">New URL to go to</param>
		public static void Redirect(string url) {
			if (url.StartsWith("~/")) url = ResolveUrl(url);
			//Web.Response.CacheControl = "no-cache";20130228JN removed as caused http execption doing a write, then redirect			
			try {
				Web.Response.CacheControl = "no-cache";
			} catch (HttpException) {  // if headers already finished this error will happen
			}
			Response.Redirect(url, true);  // MN 20140407 breaking change, but good one - now ends the response, as it should have always done
		}

		/// <summary>
		/// Sends headers to redirect to another page and tells search engines never to come back to the current page (ie the current URL is old).
		/// HTTP 301 (Moved Permanently)
		/// </summary>
		/// <param name="url">New URL to permanently change to</param>
		public static void RedirectPermanently(string url) {
			Response.Status = "301 Moved Permanently";
			if (url.StartsWith("~/")) url = ResolveUrl(url);
			Response.AddHeader("Location", url);
			// MN 20110511: breaking change - now ends the response, as it should have always done
			Response.End();
		}

		/// <summary>
		/// Shortcut to HttpContext.Current.Server.MapPath().
		/// Returns the physical path for the given relative or absolute virtual path.
		/// </summary>
		/// <param name="virtualPath">A relative or absolute virtual path</param>
		/// <returns>An absolute physical path</returns>
		public static string MapPath(string virtualPath) {
			if (virtualPath.Length > 3 && (virtualPath.Substring(1, 2) == ":\\" || virtualPath.Substring(1, 2) == ":/" || virtualPath.Substring(0, 2) == "\\\\")) {       // this is escaped version of :\ and \\
				// it is already an absolute path eg C:\something
				// fix any accidental wrong way slashes
				return virtualPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
			}
			if (virtualPath.StartsWith("~/../")) {
				// special case - allows us to go up from the approot - it is not normally possible to go out of the site root
				string path = Server.MapPath("~/").RemoveSuffix("\\");
				virtualPath = virtualPath.RemovePrefix("~/");
				while (virtualPath.StartsWith("../")) {
					virtualPath = virtualPath.RemovePrefix("../");
					path = FileSystem.GetParentPath(path);
				}
				path = path + "\\" + virtualPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
				return path;
			}
			string result = null;
			try {
				result = Server.MapPath(virtualPath);
			} catch (Exception e) {
				throw new ProgrammingErrorException("Web.MapPath error in path: [" + virtualPath + "]" ,e);
			}
			return result;
		}

		/// <summary>
		/// Return a virtual path given a phycial path
		/// </summary>
		/// <param name="physicalPath"></param>
		/// <returns></returns>
		public static string UnMapPath(string physicalPath) {
			string siteRoot = Request.PhysicalApplicationPath;
			return Web.Root + physicalPath.RemovePrefix(siteRoot).Replace("\\", "/");
		}

		/// <summary>
		/// Converts a relative URL (must start with "~") to an server relative URL (ie one starting with a /).
		/// </summary>
		/// <example>ResolveUrl("~/images/mike.gif") - might return "/images/mike.gif"</example>
		/// <param name="relativeUrl"></param>
		/// <returns>Server relative URL (this is sometimes called absolute URL)</returns>
		public static string ResolveUrl(string relativeUrl) {
			if (relativeUrl.IsBlank()) {
				return relativeUrl;
			}
			if (relativeUrl.StartsWith("http")) {
				// it is already resolved
				return relativeUrl;
			} else {
				if (relativeUrl.StartsWith("~") && !relativeUrl.StartsWith("~/")) {  // be nice here, correct common mistake of forgetting a slash
					relativeUrl = relativeUrl.Insert(1, "/");
				}
				try {
					return VirtualPathUtility.ToAbsolute(relativeUrl);
				} catch (Exception) {
					throw new ProgrammingErrorException("Web.ResolveUrl(relativeUrl): The relativeUrl must start with ~/<br>You passed in [" + relativeUrl + "]");
				}
			}
		}

		/// <summary>
		/// Converts a relative URL (can start with "~") to an absolute URL.
		/// </summary>
		/// <example>ResolveUrlFull("~/images/mike.gif") - might return "http://www.beweb.co.nz/images/mike.gif"</example>
		/// <param name="relativeUrl"></param>
		/// <returns>Full absolute URL eg http://www.beweb.co.nz/about.aspx</returns>
		public static string ResolveUrlFull(string relativeUrl) {
			if (relativeUrl.StartsWith("http")) {
				// it is already resolved
				return relativeUrl;
			} else {
				return ProtocolAndHost + ResolveUrl(relativeUrl);
			}
		}

		/// <summary>
		/// Returns the relative URL path between two relative URLs.
		/// </summary>
		/// <example>Web.GetRelativeUrl("images/static/icons/", "images") == "../../"</example>
		/// <param name="relativeUrl"></param>
		/// <returns>Absolute URL</returns>
		//	public static string GetRelativeUrl(string fromPath, string toPath) {
		//		return VirtualPathUtility.MakeRelative(fromPath, toPath);
		//	}

		/// <summary>
		/// Returns true if this app is running on a web server (eg IIS).
		/// Will return false if this is a windows app or running under a unit test tool.
		/// </summary>
		public static bool IsRunningOnWebServer {
			get { return HttpContext.Current != null; }
		}

		/// <summary>
		/// Returns true if the current request has begun and the Request object has been initialised. 
		/// Will return false in the Global Application_Start for example.
		/// Also returns false if there is no such thing as a request (eg if this is a console app).
		/// </summary>
		public static bool IsRequestInitialised {
			get {
				if (!IsRunningOnWebServer) return false;
				try {
					var r = HttpContext.Current.Request;
				} catch (HttpException) {
					return false;
				}
				return true;
			}
		}

		/// <summary>
		/// Returns server hostname (eg www.beweb.co.nz)
		/// </summary>
		public static string Host {
			get {
				if (IsRequestInitialised) {
					return Request.Url.Host;
				} else {
					// We may be on Application_Start or sometime when we have Context but don't have Request.
					// In this case use machine name as this seems to be same as the host.
					// Unknown machine names will default to LVE.
					return System.Environment.MachineName.ToLower();
				}
			}
		}

		/// <summary>
		/// Protocol part of current URL (eg http:// or https://)
		/// </summary>
		public static string Protocol {
			get { return Request.Url.Scheme + "://"; }
		}

		/// <summary>
		/// Protocol and host portions of current URL (eg http://www.beweb.co.nz/).
		/// Includes port as well if not default port (ie 80 for http or 443 for https).
		/// </summary>
		public static string ProtocolAndHost {
			get {
				string result = Request.Url.Scheme + "://" + Host;
				bool isDefaultPort = (Request.Url.Scheme == "http" && Request.Url.Port == 80) || (Request.Url.Scheme == "https" && Request.Url.Port == 443);
				if (!isDefaultPort) {
					result += ":" + Request.Url.Port;
				}
				return result;
			}
		}
		public static string ProtocolAndHostSSL {
			get { return "https://" + Host + ""; }
		}

		public static string ProtocolAndHostNoSSL {
			get { return "http://" + Host + ""; }
		}

		/// <summary>
		/// Returns Base URL of the site (eg http://www.beweb.co.nz/ or http://www.beweb.co.nz/intranet/).
		/// </summary>
		public static string BaseUrl {
			get { return ProtocolAndHost + VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute("~")); }
		}

		/// <summary>
		/// Returns Base URL of the site WITHOUT protocol "http:" or "https:" (eg //www.beweb.co.nz/ or //www.beweb.co.nz/intranet/).
		/// </summary>
		public static string BaseUrlNoProtocol {
			get { return "//" + Host + VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute("~")); }
		}

		/// <summary>
		/// switch out of ssl if we are in it
		/// </summary>
		public static string BaseUrlNoSSL {
			get { return ProtocolAndHostNoSSL + VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute("~")); }
		}

		/// <summary>
		/// use SSL if live, otherwise no SSL
		/// </summary>
		public static string BaseUrlSSL {
			get { return ((Util.ServerIsLive) ? ProtocolAndHostSSL : ProtocolAndHostNoSSL) + VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute("~")); }
		}

		/// <summary>
		/// Returns URL of App Root (eg /yourapp/ or /). This is the same as ResolveUrl("~/").
		/// </summary>
		public static string Root {
			get { return VirtualPathUtility.ToAbsolute("~/"); }
		}

		/// <summary>
		/// Returns absolute URL to current page with no querystring (eg http://www.beweb.co.nz/jobs.aspx)
		/// </summary>
		public static string PageUrl {
			get { return ProtocolAndHost + Request.Url.LocalPath; }
		}

		/// <summary>
		/// Full URL of current page, including everything http: through to querystring.
		/// </summary>
		public static string FullUrl {
			get { return Request.Url.ToString(); }
		}

		/// <summary>
		/// Original URL before any URL rewriting happened, including everything http: through to querystring.
		/// </summary>
		public static string FullRawUrl {
			get { return ProtocolAndHost + Request.RawUrl; }
		}

		/// <summary>
		/// Returns the full query string including the "?"
		/// </summary>
		public static string QueryString {
			get { return HttpContext.Current.Request.Url.Query; }
		}

		/// <summary>
		/// URL of current page from the site root only, including querystring. Does not start with slash. eg admin/deals/dealedit.aspx?id=3434
		/// </summary>
		public static string LocalUrl {
			get { return FullRawUrl.RemovePrefix(BaseUrl); }
		}

		/// <summary>
		/// Gets a request variable (first looks in Form and then in QueryString).
		/// This differs from Web.Request[] in the case where there is a form and querystring variable with the same name, as Web.Request[] will comma delimit them but Web.RequestEx[] will return the form variable and ignore the querystring (which is generally what you need).
		/// This also detects wacky old style Web Forms Control ID naming scheme.
		/// </summary>
		public static Web.RequestExCollection RequestEx {
			get { return new RequestExCollection(); }
		}

		public static string UserIpAddress {
			get { return Web.ServerVars["REMOTE_ADDR"]; }
		}

		/// <summary>
		/// Returns true if user is on a mobile browser or has chosen mobile version.
		/// Returns false if user is not on a mobile browser or has chosen standard version
		/// User can manually override the mobile/standard version by setting ?mobile=1 or ?mobile=0 (?mobile=auto reverts back to automatic mobile detection)
		/// </summary>
		public static bool IsMobile {
			get {
				// allow any page to switch to mobile or normal mode by passing a request variable
				// store this in a session variable
				if (Request["mobile"] + "" == "1") {
					Session["IsMobileMode"] = true;
				} else if (Request["mobile"] + "" == "0") {
					Session["IsMobileMode"] = false;
				} else if (Request["mobile"] + "" == "auto") {
					Session["IsMobileMode"] = null;
				}
				// check the session variable which means the mobile mode has been manually set on this or a previous page request
				if (Session["IsMobileMode"] + "" != "") {
					return (Session["IsMobileMode"] + "").ConvertToBool();
				} else {
					bool enableMobile = Util.GetSettingBool("EnableMobileSite", false);
					if (!enableMobile) {
						Session["IsMobileMode"] = false;
						return false;
					}
					return IsMobileDevice;
				}
			}
			set {
				Session["IsMobileMode"] = value;
			}
		}

		/// <summary>
		/// Returns true if user is on a mobile browser or has chosen mobile version.
		/// </summary>
		public static bool IsMobileDevice {
			get {
				bool enableMobile = Util.GetSettingBool("EnableMobileSite", false);
				if (!enableMobile) {
					return false;
				}

				// otherwise detect based on user agent
				var userAgent = Request.UserAgent;
				if (userAgent != null) {
					string strUserAgent = userAgent.ToLower();
					if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("iphone") || strUserAgent.Contains("android") ||
							strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile safari") ||
							(strUserAgent.Contains("mozilla") && strUserAgent.Contains("x11; linux x86_64") && strUserAgent.Contains("khtml, like gecko) chrome")) || //nexus4 is odd				//		Value	"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.59 Safari/537.36"	object {string}
							strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("mobile") ||
							strUserAgent.Contains("palm") || strUserAgent.Contains("windows phone") || strUserAgent.Contains("iemobile")) {
						if (!strUserAgent.Contains("tablet") && !strUserAgent.Contains("ipad")) {
							return true;
						}
					}
				}
				return false;
			}
		}


		public static bool IsTabletDevice {
			get {
				// otherwise detect based on user agent
				var userAgent = Request.UserAgent;
				if (userAgent != null) {
					string strUserAgent = userAgent.ToLower();
					if (strUserAgent.Contains("tablet") || strUserAgent.Contains("ipad") || (strUserAgent.Contains("android") && !strUserAgent.Contains("mobile"))) return true;
				}
				return false;
			}
		}

		/*		public static string InfoMessage {
					get { string msg = Web.Session["InfoMessage"]+""; Web.Session["InfoMessage"]=null; return msg; }
					set { Web.Session["InfoMessage"] = value; }
				}

				public static string ErrorMessage {
					get { string msg = Web.Session["ErrorMessage"]+""; Web.Session["ErrorMessage"]=null; return msg; }
					set { Web.Session["ErrorMessage"] = value; }
				}

				public static string WarningMessage {
					get { string msg = Web.Session["WarningMessage"]+""; Web.Session["WarningMessage"]=null; return msg; }
					set { Web.Session["WarningMessage"] = value; }
				}
		*/

		public class RequestExCollection {
			public string this[string requestParamName] {
				get {
					// MN 18-Dec-2010	changed form check to be first - this is to handle case edit pages where default value comes in querystring but user may change it on the form, which should take then precedence
					if (Request.Form[requestParamName] != null) return Request.Form[requestParamName];
					if (Request.QueryString[requestParamName] != null) return Request.QueryString[requestParamName];

					//make web forms work better
					foreach (string key in Request.Form.AllKeys) {
						if (key != null && key.EndsWith("$" + requestParamName)) return Request.Form[key];
					}
					foreach (string key in Request.QueryString.AllKeys) {
						if (key != null && key.EndsWith("$" + requestParamName)) return Request.QueryString[key];
					}

					//check for hidden field passed when checkbox was not checked therefore not passed (if was passed, would be returned in code above)
					foreach (string key in Request.Form.AllKeys) {
						if (key != null && key.StartsWith("checkboxposted_" + requestParamName) && Request.Form[key] == "y") return "false";
					}
					foreach (string key in Request.QueryString.AllKeys) {
						if (key != null && key.StartsWith("checkboxposted_" + requestParamName) && Request.Form[key] == "y") return "false";
					}


					return null;
				}
			}
		}


		/// <summary>
		/// Sets the ContentType to Excel, Content-Disposition to attachment, and filename to the one you supply. This is for rendering tab separated or CSV.
		/// Use SetHeadersForExcelHtml instead if you want formatting, and supply as html table.
		/// </summary>
		/// <param name="newFileName">Name to call the file which is being downloaded</param>
		public static void SetHeadersForExcel(string newFileName) {
			if (Request["debug"] != "1") {
				Response.ContentType = "application/vnd.ms-excel";
				Response.AddHeader("Content-Disposition", "attachment; filename=\"" + newFileName + "\"");
			}
		}

		/// <summary>
		/// Sets the ContentType to HTML, Content-Disposition to attachment, and filename to the one you supply but replaces the extension with .XLS so that Excel opens it. Your content should be an html table including formatting like bold, align etc.
		/// See also SetHeadersForExcel which is used for TSV or CSV.
		/// </summary>
		/// <param name="newFileName">Name to call the file which is being downloaded</param>
		public static void SetHeadersForExcelHtml(string newFileName) {
			if (Request["debug"] != "1") {
				Response.ContentType = "text/html";
				newFileName = newFileName.LeftUntilLast(".") + ".xls";
				Response.AddHeader("Content-Disposition", "attachment; filename=\"" + newFileName + "\"");
			}
		}

		/// <summary>
		/// Sets the ContentType to PDF, Content-Disposition to attachment, and filename to the one you supply.
		/// </summary>
		/// <param name="newFileName">Name to call the file which is being downloaded</param>
		public static void SetHeadersForPDF(string newFileName) {
			if (Request["debug"] != "1") {
				Response.ContentType = "application/pdf";
				Response.AddHeader("Content-Disposition", "attachment; filename=\"" + newFileName + "\"");
			}
		}

		/// <summary>
		/// Sets the ContentType to xml and adds XML declaration line. (TODO - incomplete)
		/// </summary>
		public static void SetHeadersForFlashXML() {
			if (Request["debug"] != "1") {
				Response.ContentType = "text/xml";
				Response.AddHeader("Content-Disposition", "inline");
			}
		}

		/// <summary>
		/// Sets the ContentType to xml and adds XML declaration line. (TODO - incomplete)
		/// </summary>
		public static void SetHeadersForXML() {
			if (Request["debug"] != "1") {
				Response.ContentType = "text/xml";
				Response.AddHeader("Content-Disposition", "inline");
			}
		}
#if PathAndFile
		public static void DownloadFile(string filename) {
			DownloadFile(filename, "");
		}

		/// <summary>
		/// Gets a file on the server and downloads it to the browser. 
		/// Handles content type, content length, content disposition and filename.
		/// </summary>
		/// <param name="filename">Full path and file name of actual file on disk.</param>
		/// <param name="newFileName">Suggested name that users will see in Save File As dialog box when downloading.</param>
		public static void DownloadFile(string filename, string newFileName) {
			DownloadFile(filename, newFileName, false, new DateTime().AddMinutes(1));
		}

		/// <summary>
		/// Gets a file on the server and downloads it to the browser. 
		/// Handles content type, content length, content disposition and filename.
		/// </summary>
		/// <param name="filename">Full path and file name of actual file on disk.</param>
		/// <param name="newFileName">Suggested name that users will see in Save File As dialog box when downloading.</param>
		public static void DownloadFile(string filename, string newFileName, bool isInline, DateTime expires) {
			Response.ClearHeaders();
			Response.ClearContent();

			FileInfo fileInfo;
			try {
				filename = Web.MapPath(filename);
				fileInfo = new FileInfo(filename);
			} catch (System.ArgumentException e) {
				throw new BewebException("Web.DownloadFile: Argument exception. File name [" + filename + "] newFileName [" + newFileName + "]. " + e.Message);
			}
			if (!fileInfo.Exists) {
				// user attempted to download a file, the file is not found, so this semantically should be a 404, so throw a beweb 404
				// show more detailed exception for developers
				//throw new BadUrlException("Web.DownloadFile - Download file not found on server. Filename [" + filename + "]");
				Error.PageNotFound("Download file not found on server. Filename [" + filename + "]");
				return;
			}

			// check for if-modified-since header conditional GET caching
			var lastMod = fileInfo.LastWriteTime;
			var lastModClientStr = Request.Headers["If-Modified-Since"];
			DateTime lastModClient;
			bool debug = false;  // set this to true if debugging 304s
			bool isModified = true;
			if (debug) Logging.dlog("If-Modified-Since=" + lastModClientStr);
			if (!string.IsNullOrEmpty(lastModClientStr)) {
				bool isDate = DateTime.TryParseExact(lastModClientStr, "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out lastModClient);
				if (debug) Logging.dlog("isDate=" + isDate + " lastModClient=" + lastModClient);
				if (isDate) {
					if (debug) Logging.dlog("lastModFileDate=" + lastMod);
					if ((lastMod - lastModClient).Duration().TotalSeconds < 60) {
						if (debug) Logging.dlog("Not Modified");
						Response.ClearHeaders();
						Response.StatusCode = 304;
						Response.StatusDescription = "Not Modified";
						// Explicitly set the Content-Length header so the client doesn't wait for content but keeps the connection open for other requests
						Response.AddHeader("Content-Length", "0");
						Response.SuppressContent = true;
					} else {
						if (debug) Logging.dlog("Modified - serve the file");
					}
				}
			}

			// set client caching for a short while as images are unlikely to change very often
			Response.Cache.SetExpires(expires);

			if (Response.StatusCode != 304) {
				// set correct modified date conditional GET caching
				Response.AddFileDependency(filename);
				Response.Cache.SetLastModifiedFromFileDependencies();
				Response.Cache.SetCacheability(HttpCacheability.Public);
				Response.AddHeader("Content-Length", fileInfo.Length.ToString());

				if (isInline) {
					// inline eg displaying an image
					Response.ContentType = FileSystem.GetMimeType(fileInfo.Extension);
				} else {
					// attachment eg downloading a pdf
					if (filename.EndsWith(".pdf", true, null)) {
						Response.ContentType = "application/pdf";
					} else if (filename.EndsWith(".zip", true, null)) {
						Response.ContentType = "application/zip";
					} else if (filename.EndsWith(".xls", true, null) || filename.EndsWith(".xlsx", true, null)) {
						Response.ContentType = "application/vnd.ms-excel";
					} else if (filename.EndsWith(".doc", true, null) || filename.EndsWith(".docx", true, null)) {
						Response.ContentType = "application/msword";
					} else if (filename.EndsWith(".jpg", true, null) || filename.EndsWith(".jpeg", true, null) || filename.EndsWith(".jpe", true, null)) {
						Response.ContentType = "application/jpeg";
					} else {
						// note that for PNG, GIF, JPG etc we do not want to use the normal image MIME types because we want to ensure the browser downloads it rather than opening it
						Response.ContentType = "application/octet-stream";
					}
				}
				string contentDisposition = isInline ? "inline" : "attachment";
				if (newFileName != null) {
					if (newFileName == "") {
						newFileName = fileInfo.Name;
					}
					if (!newFileName.Contains(".")) {
						newFileName = PathAndFile.CrunchFileName(newFileName) + "." + PathAndFile.GetExtension(filename, "dat");
					}
					contentDisposition += "; filename=\"" + newFileName + "\"";
				}
				Response.AddHeader("Content-Disposition", contentDisposition);

				// finally... now send the actual file!
				Response.TransmitFile(filename);
			}

			Flush();       // using Flush() now instead of Response.Flush() as this was causing an error: The remote host closed the connection. The error code is 0x800703E3.
			Response.Close();
		}

		/// <summary>
		/// Displays an image or any file of content type that that browser will display inline.
		/// Gets a file on the server and displays it in the browser as the complete response. 
		/// This is for serving images.
		/// </summary>
		public static void DisplayImage(string filename) {
			DownloadFile(filename, null, true, DateTime.Now.AddMinutes(1));
		}
		/// <summary>
		/// Displays an image or any file of content type that that browser will display inline.
		/// Gets a file on the server and displays it in the browser as the complete response. 
		/// This is for serving images.
		/// </summary>
		public static void DisplayImage(string filename, DateTime expiryDate) {
			DownloadFile(filename, null, true, expiryDate);
		}

#endif

		public static void End() {
			try {
				Response.End();
			} catch (HttpException ex) {
				//dont care if it dies
			}
		}

		public static void WriteLine(string p) {
			Write(p + "<br>");
		}

		/// <summary>
		/// URL of parent of current page. Http path to the current page eg. /admin/personadmin/ when page is /admin/personadmin/personedit.aspx?id=5
		/// </summary>
		public static string LocalPagePath { get { return Web.Root + Web.LocalUrl.Substring(0, Web.LocalUrl.LastIndexOf("/")) + "/"; } }

		/// <summary>
		/// URL of current page, excluding querystring. Http path to the current page eg. /admin/personadmin/personedit.aspx when page is /admin/personadmin/personedit.aspx?id=5
		/// </summary>
		public static string LocalPageUrl { get { return Request.Url.LocalPath; } }

		/// <summary>
		/// Sets the TempData "info" message which remains in session until it is next used and then it automatically cleared.
		/// This is output and cleared by using Html.InfoMessage().
		/// </summary>
		public static string InfoMessage {
			get {
				return HttpUtility.HtmlDecode(InfoMessageHtml + "");
			}
			set {
				InfoMessageHtml = HttpUtility.HtmlEncode(value + "");
			}
		}

		/// <summary>
		/// Raw HTML version of Web.InfoMessage which can be used if you need to add html formatting in the info message. 
		/// CAUTION: This should ONLY be used on known safe HTML in order to prevent XSS vulnerabilities.
		/// </summary>
		public static string InfoMessageHtml {
			get {
				// first try the cookie, in case that was set
				var httpCookie = Web.Request.Cookies["InfoMessage"];
				if (httpCookie != null) {
					httpCookie.Expires = DateTime.Today.AddDays(-99);
					Web.Response.SetCookie(httpCookie);
					string html = Crypto.Decrypt(httpCookie.Value);
					return html;
				}
				// then try the cookie, in case that was set
				if (Web.Session["InfoMessage"] != null) {
					return Web.Session["InfoMessage"] + "";
				}
				return null;
			}
			set {
				Web.Response.CacheControl = "no-cache";
				string html = value;
				if (html == "") html = null;
				if (Web.Session == null) {
					Web.Response.SetCookie(new HttpCookie("InfoMessage", Crypto.Encrypt(html)));
				} else {
					if (html == null) {
						Web.Session["InfoMessage"] = null;	// clear it
					} else if (Web.Session["InfoMessage"] != null) {
						Web.Session["InfoMessage"] = html;  // set it
					} else {
						Web.Session["InfoMessage"] += html;   // add to it
					}
				}
			}
		}

		/// <summary>
		/// Sets the TempData "error" message which remains in session until it is next used and then it automatically cleared.
		/// This is output and cleared by using Html.InfoMessage().
		/// </summary>
		public static string ErrorMessage {
			get {
				return HttpUtility.HtmlDecode(ErrorMessageHtml + "");
			}
			set {
				ErrorMessageHtml = HttpUtility.HtmlEncode(value + "");
			}
		}

		/// <summary>
		/// Raw HTML version of Web.ErrorMessage which can be used if you need to add html formatting in the error message. 
		/// CAUTION: This should ONLY be used on known safe HTML in order to prevent XSS vulnerabilities.
		/// </summary>
		public static string ErrorMessageHtml {
			get {
				return Web.Session["ErrorMessage"] + "";
			}
			set {
				Web.Response.CacheControl = "no-cache";
				string html = value;
				if (html == "") html = null;
				if (html == null) {
					Web.Session["ErrorMessage"] = null;	// clear it
				} else if (Web.Session["ErrorMessage"] != null) {
					Web.Session["ErrorMessage"] = html;
				} else {
					Web.Session["ErrorMessage"] += html;
				}
			}
		}

		/// <summary>
		/// Returns true if given url starts with http:// https:// or //
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static bool IsAbsoluteUrl(string url) {
			if (url == null) return false;
			var lower = url.ToLower();
			return lower.StartsWith("http://") || lower.StartsWith("https://") || lower.StartsWith("//");
		}

		/// <summary>
		/// Returns true if current request is over HTTPS
		/// </summary>
		public static bool IsSecureConnection {
			get { return HttpContext.Current.Request.IsSecureConnection; }
		}

		public static bool IsAdminSection {
			get { return Web.PageUrl.ContainsInsensitive(Web.AdminRoot); }
		}

		public static bool IsOldBrowser {
			get {
				var isOld = Web.Request.Browser.Browser == "IE" && Web.Request.Browser.MajorVersion < Util.GetSettingInt("MinSupportedInternetExplorerVersion", 9);
				return isOld;
			}
		}

		public static void CacheClearAll() {
			List<string> keys = new List<string>();
			//IDictionaryEnumerator enumerator = Cache.InternalCache.GetEnumerator(); -- this does NOT work
			//while (enumerator.MoveNext()) {
			//	keys.Add(enumerator.Key.ToString());
			//}
			var internalCache = Web.Cache.InternalCache;
			foreach (var t in internalCache) {
				System.Collections.DictionaryEntry entry = (System.Collections.DictionaryEntry)t;
				object key = entry.Key;
				keys.Add(key.ToString());
			}
			for (int i = 0; i < keys.Count; i++) {
				Cache.InternalCache.Remove(keys[i]);
			}
		}

		public static void ClearOutputCache() {
			List<string> keys = new List<string>();
			var internalCache = Web.Cache.InternalCache;
			foreach (var t in internalCache) {
				System.Collections.DictionaryEntry entry = (System.Collections.DictionaryEntry)t;
				string key = entry.Key.ToString();
				if (key.StartsWith("BewebOutputCache:")){
					keys.Add(key);
				}
			}
			for (int i = 0; i < keys.Count; i++) {
				Cache.InternalCache.Remove(keys[i]);
			}
		}

		public static Dictionary<string, string> SplitQueryString(string queryString) {
			queryString = queryString.RemovePrefix("?");
			// split apart  query params
			var extraParams = queryString.Split('&');
			var queryDic = new Dictionary<string, string>();
			foreach (var extraParam in extraParams) {
				var nameValuePair = extraParam.Split('=');
				if (nameValuePair.Length == 2) {
					queryDic.Add(nameValuePair[0], nameValuePair[1]);
				}
			}
			return queryDic;
		}

		public static string JoinQueryString(Dictionary<string, string> queryStringDictionary) {
			string queryString = "";
			foreach (KeyValuePair<string, string> pair in queryStringDictionary) {
				if (queryString == "") {
					queryString += "?";
				} else {
					queryString += "&";
				}
				queryString += pair.Key + "=" + pair.Value;
			}
			return queryString;
		}

		public static string FullUrlPlus(string extraQueryString) {
			var existingUrl = FullUrl;
			string newUrl = existingUrl.LeftUntil("?") + "?";
			string newQuery = "";
			extraQueryString = extraQueryString.RemovePrefix("?");

			if (existingUrl.Contains("?")) {
				// split apart existing query params
				string existingQuery = existingUrl.RightFrom("?");
				var queryParams = SplitQueryString(existingQuery);
				var extraParams = SplitQueryString(extraQueryString);

				// reconstruct querystring by adding original params and substituting others
				foreach (KeyValuePair<string, string> existingParam in queryParams) {
					if (newQuery != "") {
						newQuery += "&";
					}
					string name = existingParam.Key;
					if (extraParams.ContainsKey(name)) {
						newQuery += name + "=" + extraParams[name];
					} else {
						newQuery += name + "=" + existingParam.Value;
					}
				}
				// add all other name/values that are not in the existing params
				foreach (KeyValuePair<string, string> newParam in extraParams) {
					newQuery += "&";
					string name = newParam.Key;
					if (!queryParams.ContainsKey(name)) {
						newQuery += name + "=" + newParam.Value;
					}
				}
				newUrl += newQuery;
			} else {
				newUrl += extraQueryString;
			}
			return newUrl;
		}
		/// <summary>
		/// get the value of a checkbox, whether it was posted or not.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		//public static bool? xGetCheckboxValue(string name, bool? defaultValue) {
		//	bool? result = null;
		//	string wasPosted = Web.Request["checkboxposted_" + name];
		//	if (wasPosted == null) {
		//		result = defaultValue;
		//	} else {
		//		result = Web.Request[name].ToBool();
		//	}
		//	return result;
		//}
		public static bool? GetCheckboxValue(string name, bool? defaultValue) {
			bool? result = null;
			bool wasPosted = Web.Request["checkboxposted_" + name] == "y";
			if (wasPosted) {
				result = Web.Request[name].ToBool();
			} else {
				result = defaultValue;
			}
			return result;
		}

		// MN 20140510 - removed RenderPartialViewToString - this should not be here as it has a dependency on MVC
		// has been moved to SavvyMVC

		public static Web.SessionCookieCollection SessionCookie {
			get { return new SessionCookieCollection(); }
		}

			public class SessionCookieCollection {
				public string this[string sessionVariableName] {
				get {
					if (Session[sessionVariableName] != null) return Web.Session[sessionVariableName].ToString();
					var cookie = Request.Cookies[sessionVariableName];
					if (cookie != null) {
						Session[sessionVariableName] = cookie.Value;
						return cookie.Value;
					}
					return null;
				}
				set {
					var cookie = Request.Cookies[sessionVariableName];
					if (cookie != null) {
						cookie.Value = value;
						Response.Cookies.Set(cookie);
					} else {
						cookie = new HttpCookie(sessionVariableName);
						cookie.Value = value;
						Response.Cookies.Add(cookie);
					}
					Session[sessionVariableName] = value;
				}
			}
		}


	}

	/// <summary>
	/// Represent a valid HTTP Server variable that can be passed into Server.ServerVariables collection
	/// </summary>
	public class HttpServerVariable {
		public const string All = "ALL_HTTP";
		public const string AllRaw = "ALL_RAW";
		public const string ApplicationPhysicalPath = "APPL_PHYSICAL_PATH";
		public const string AuthorizationPassword = "AUTH_PASSWORD";
		public const string AuthorizationType = "AUTH_TYPE";
		public const string AuthorizationUser = "AUTH_USER";
		public const string CertificateCookie = "CERT_COOKIE";
		public const string CertificationFlags = "CERT_FLAGS";
		public const string CertificationIssuer = "CERT_ISSUER";
		public const string CertificationKeySize = "CERT_KEYSIZE";
		public const string CertificationSecretKeySize = "CERT_SECRETKEYSIZE";
		public const string CertificationSerialNumber = "CERT_SERIALNUMBER";
		public const string SertificationServerIssuer = "CERT_SERVER_ISSUER";
		public const string CertificationServerSubject = "CERT_SERVER_SUBJECT";
		public const string ContentLength = "CONTENT_LENGTH";
		public const string ContentType = "CONTENT_TYPE";
		public const string GatewayInterface = "GATEWAY_INTERFACE";
		public const string HttpReferer = "HTTP_REFERER";
		public const string HttpUserAgent = "HTTP_USER_AGENT";
		public const string Https = "HTTPS";
		public const string HttpsKeySize = "HTTPS_KEYSIZE";
		public const string HttpsSecretKeySize = "HTTPS_SECRETKEYSIZE";
		public const string HttpsSerialNumber = "HTTPS_SERIALNUMBER";
		public const string HttpsServerIssuer = "HTTPS_SERVER_ISSUER";
		public const string HttpsServerSubject = "HTTPS_SERVER_SUBJECT";
		public const string InstanceId = "INSTANCE_ID";
		public const string InstanceMetaPath = "INSTANCE_META_PATH";
		public const string LocalAddress = "LOCAL_ADDR";
		public const string LogonUser = "LOGON_USER";
		public const string PathInfo = "PATH_INFO";
		public const string PathTranslated = "PATH_TRANSLATED";
		public const string QueryString = "QUERY_STRING";
		public const string RemoteAddress = "REMOTE_ADDR";
		public const string RemoteHost = "REMOTE_HOST";
		public const string RemotePort = "REMOTE_PORT";
		public const string RemoteUser = "REMOTE_USER";
		public const string RequestMethod = "REQUEST_METHOD";
		public const string ScriptName = "SCRIPT_NAME";
		public const string ServerName = "SERVER_NAME";
		public const string ServerPort = "SERVER_PORT";
		public const string ServerPortSecure = "SERVER_PORT_SECURE";
		public const string ServerProtocol = "SERVER_PROTOCOL";
		public const string ServerSoftware = "SERVER_SOFTWARE";
	}

}
#if TestExtensions
namespace BewebTest {
	[TestClass]
	internal class TestWeb {

		[TestMethod]
		public static void TestHost() {
			var expectedValue = Web.ServerVars["server_name"];
			var actualValue = Web.Host;
			Web.Write(actualValue);
			Assert.AreEqual(expectedValue, actualValue);
		}

		public static void TestCreateAndDownloadFile() {
			string path = Web.MapPath(Web.Attachments + "testfordownload.txt");
			File.WriteAllText(path, "this is a test");
			Web.DownloadFile(path);
			File.Delete(path);
		}

		[TestMethod]
		public static void TestPageFileName() {
			string str = Web.PageFileName;
			Assert.AreEqual(str, Util.GetPageFileName());
			Assert.IsInstanceOfType(Web.PageFileName, typeof(string));
		}

		public static string _hostname;

		[TestMethod]
		public static void TestHostSpeed() {
			
			Web.WriteLine("Test using Request");
			var sw = new Stopwatch();
			sw.Start();
			for(var i = 0; i < 10000000; i++) {
				var x = Web.Request.Url.Host;
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
			
			Web.WriteLine("Test using ServerVariables");
			sw = new Stopwatch();
			sw.Start();
			for(var i = 0; i < 10000000; i++) {
				var x = Web.Request.ServerVariables["SERVER_NAME"];
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
			
			Web.WriteLine("Test using PageGlobals");
			sw = new Stopwatch();
			sw.Start();
			Web.PageGlobals["hostname"] = Web.Request.Url.Host;
			for(var i = 0; i < 10000000; i++) {
				var x = Web.PageGlobals["hostname"];
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
					
			Web.WriteLine("Test using Cache");
			sw = new Stopwatch();
			sw.Start();
			Web.Cache["hostname"] = Web.Request.Url.Host;
			for(var i = 0; i < 10000000; i++) {
				var x = Web.Cache["hostname"];
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
								
			Web.WriteLine("Test using Raw Cache");
			sw = new Stopwatch();
			sw.Start();
			HttpContext.Current.Cache["hostname"] = Web.Request.Url.Host;
			for(var i = 0; i < 10000000; i++) {
				var x = HttpContext.Current.Cache["hostname"];
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);								

			Web.WriteLine("Test using Session");
			sw = new Stopwatch();
			sw.Start();
			Web.Session["hostname"] = Web.Request.Url.Host;
			for(var i = 0; i < 10000000; i++) {
				var x = Web.Session["hostname"];
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
			
			Web.WriteLine("Test using Static Variable");
			sw = new Stopwatch();
			sw.Start();
			_hostname = Web.Request.Url.Host;
			for(var i = 0; i < 10000000; i++) {
				var x = _hostname;
			}
			sw.Stop();
			Web.WriteLine("Time elapsed: " + sw.ElapsedMilliseconds);
			
			//string str = Web.PageFileName;
			//Assert.AreEqual(str, Util.GetPageFileName());
			//Assert.IsInstanceOfType(Web.PageFileName, typeof(string));
		}
	}
}
#endif