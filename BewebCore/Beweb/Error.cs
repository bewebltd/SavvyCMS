#define MVC
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;

using System.Net;
using System.Net.Mail;
using System.Web;

namespace Beweb {
	public class Error {
		/// <summary>
		/// After you catch a fatal error, this will send an email to programmers and redirect to a HTTP 500 error page.
		/// Only call this after you catch an exception.
		/// </summary>
		public static void Notify() {
			// if you tuns this off in web appsettings config it will show a standard .net error screen (yellow screen of death)
			// this is useful for debugging errors
			if (Util.GetSettingBool("UseErrorNotify", true)) Notify(true);
		}


		/// <summary>
		/// When an exception is caught, but it's not too serious, send an email.
		/// Sends an email about the last exception that was thrown.
		/// Only call this after you catch an exception.
		/// </summary>
		public static void SendExceptionEmail() {
			SendExceptionEmail(null);
		}

		/// <summary>
		/// When an exception is caught, but it's not too serious, send an email.
		/// Sends an email about the last exception that was thrown.
		/// Only call this after you catch an exception.
		/// </summary>
		public static void SendExceptionEmail(string additionalMessage) {
			Notify(false, additionalMessage);
		}

		public static void SendExceptionEmail(Exception exception, string additionalMessage) {
			Notify(false, additionalMessage, exception);
		}

		public static void Notify(bool allowRedirect) {
			Notify(allowRedirect, null);
		}

		public static void Notify(bool allowRedirect, string additionalErrorMessage) {
			Notify(allowRedirect, additionalErrorMessage, null);
		}

		public static void Notify(bool allowRedirect, string additionalErrorMessage, Exception lastError) {
			HttpRequest rq = HttpContext.Current.Request;
			HttpResponse rsp = HttpContext.Current.Response;
			HttpServerUtility sv = HttpContext.Current.Server;

			// get error details from system
			if (lastError == null) {
				lastError = sv.GetLastError();
			}

			// get full error details containing source snippets and pretty stack trace
			var dump = new Logging.DiagnosticData(lastError);
			dump.AdditionalMessage = additionalErrorMessage;
			sv.ClearError();

			// log the error report by sending to twitch or emailing
			bool wasLogged = LogErrorReport(dump);

			// show nice user friendly screen
			// for developers also show full details on screen - show error on screen for requests from beweb office
			if (dump.HttpStatusCode == 404) {
				PageNotFound(Web.LocalUrl, dump);
			} else {
				DisplayErrorPage(dump.HttpStatusCode, null, dump, wasLogged);
			}
		}

		private static bool LogErrorReport(Logging.DiagnosticData dump) {
			// note: normally 404s are not emailed to us, but if we have any processes that we need to ensure will send us an email on a 404 we can specify emailonfail=1 in the querystring
			if (!CheckIfErrorShouldBeLogged(dump)) return false;

			bool wasLogged = false;
			try {
				//Logging.dlog("LogErrorReport try logging");
				double secondsSinceLastReported = dump.SecondsSinceLastReport;
				var isNewError = secondsSinceLastReported == 0 || secondsSinceLastReported > Util.GetSetting("ThrottleSameError", "60").ToInt(60);

				if (!isNewError) {
					// assume already logged previously, so we want to display to the user that we have logged this error
					wasLogged = true;
				} else {
					wasLogged = PostErrorReport(dump);
					dump.WasLogged = wasLogged;
				}
				//Logging.dlog("LogErrorReport result: "+ wasLogged);
			} catch (Exception e) {
				//Logging.dlog("LogErrorReport problem [" + e.Message + "]");
				SendEMail.SimpleSendEmail("errors@beweb.co.nz", "LogErrorReport problem", e.Message);
			}
			return wasLogged;
		}

		/// <summary>
		/// Sends an error message to the reporting system (http twitch or errors email, whichever is reachable)
		/// </summary>
		public static bool PostErrorReport(string message) {
			var dump = new Logging.DiagnosticData(true);
			dump.AdditionalMessage = message;
			return PostErrorReport(dump);
		}

		/// <summary>
		/// Sends a dump off to the reporting system (http twitch or errors email, whichever is reachable)
		/// </summary>
		public static bool PostErrorReport(Logging.DiagnosticData dump) {
			bool wasLogged = false;
			string subject = String.Format("ERROR on: {1} ({0})", dump.HttpStatusCode, ConfigurationManager.AppSettings.Get("ServerIsLVE") ?? Web.Request.ServerVariables["REMOTE_ADDR"]);
			subject += " | " + dump.ErrorMessage.StripTags();

			// try sending http post first
			string postUrl = Util.GetSetting("ErrorPostUrl", "(none)");
			if (postUrl != "(none)") {
				try {
					var throttle = Util.GetSettingInt("ThrottleSameError", 60);
					var client = new WebClient();
					var data = new NameValueCollection();
					data.Add("site", ConfigurationManager.AppSettings.Get("ServerIsLVE"));
					data.Add("sender", SendEMail.EmailFromAddress);
					data.Add("subject", subject.Left(168));
					data.Add("errorCode", dump.HttpStatusCode + "");
					data.Add("ErrorMessage", dump.ErrorMessage);
					data.Add("source", dump.Source);
					data.Add("FailureUrl", dump.Url);
					data.Add("WebsiteUrl", Web.BaseUrl);
					data.Add("user", dump.User);
					data.Add("dignostics", dump.ToHtml());
					data.Add("ErrorReportGuid", dump.ErrorReportGuid + "");
					data.Add("NumInstances", dump.NumberOfTimesSinceLastErrorReport + "");
					data.Add("Throttle", throttle + "");
					var response = client.UploadValues(postUrl, data);
					//var str = System.Text.Encoding.Default.GetString(response);

					if (response.Length > 0) {
						wasLogged = true;
					}
				} catch (Exception) {
				}
			}
			if (!wasLogged) {
				// if post failed, send email
				string emailTo = ConfigurationManager.AppSettings.Get("EmailAboutError");
				var email = new ElectronicMail();
				email.Subject = subject.Left(168);
				email.BodyHtml = dump.ToHtml();
				email.ToAddress = emailTo;
				wasLogged = email.Send(false);
			}
			return wasLogged;
		}
		/// <summary>
		/// called by LogErrorReport which sends emails
		/// </summary>
		/// <param name="dump"></param>
		/// <returns></returns>
		private static bool CheckIfErrorShouldBeLogged(Logging.DiagnosticData dump) {
			// check for scheduled task or other callback
			if (Util.ServerIsDev) return false;//dont email devs
			if (Util.ServerIsLive && Web.IsRequestInitialised && Web.Request["emailonfail"] + "" == "1") return true;  // always log failures on scheduled tasks (unless dev)
			if (Web.Response.IsRequestBeingRedirected) return false;
			string fullHtmlText = dump.ToHtml();
			if (fullHtmlText != null) {
				//exclusions - do not log these errors - this display as intermittent errors for the users.
				if (fullHtmlText.Contains("The client disconnected.")) return false; //exit as we dont care if the client disconnected during page load/render
				if (fullHtmlText.Contains("posted in a form with the wrong enctype.")) return false; //exit as we dont care (hack attempt)
				if (fullHtmlText.Contains("Session state has created")) return false; //exit as we dont care
				if (fullHtmlText.Contains("A potentially dangerous")) return false; //exit as we dont care
				if (fullHtmlText.Contains("state information is invalid for this page")) return false; //exit as we dont care
				if (fullHtmlText.Contains("CaptchaImageHandler") && fullHtmlText.Contains("Value cannot be null")) return false; //exit as we dont care if catcha fails
				if (fullHtmlText.Contains("Invalid character in a Base-64 string")) return false;
				if (fullHtmlText.Contains("anti-forgery token")) return false; // if the user doesn't allow cookies all the anti forgery tokens in MVC will break

				// errors that is should display as 404 for user
				if (fullHtmlText.Contains("The parameters dictionary contains a null entry for parameter")) {
					dump.HttpStatusCode = 404;  // mvc routing should be 404
					dump.AdditionalMessage = "Routing error - a route was matched but the parameters failed to convert";
				}
				if (fullHtmlText.Contains("A public action method") && fullHtmlText.Contains(" was not found on controller")) {
					//this mvc routing error is already correctly a 404
					dump.AdditionalMessage = "Routing error - check your global ASAX";
				}
			}

			// 404s and other ignorable errors
			int errorCode = dump.HttpStatusCode;
			if (errorCode == 404) return false;  // no need to log 404 not found

			// check for dev cases
			if (ShowDetailedErrors) return false;  // no need to log it if we are displaying full developer error details (this always includes dev server - unless debugging errors)

			// otherwise its an important error - usual case
			return true;
		}

		#region SendErrorEmail
		/// <summary>
		/// send error email from site to errors setting user email
		/// keep the error class self-dependant, don't use other classes if possible
		/// </summary>
		/// <param name="body"></param>
		/// <returns>error message or blank</returns>
		public static string SendErrorEmail(string body) {
			string to = Util.GetSetting("EmailAboutError", "errors@beweb.co.nz");
			string subject = Util.GetSiteName() + " - general error";
			return SendErrorEmail(to, subject, body, Util.ServerIs());
		}
		/// <summary>
		/// send error email 
		/// keep the error class self-dependant, don't use other classes if possible
		/// Send error email to programmers. This is what you should call to send a general notification which may or may not be related to an actual exception.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <returns>error message or blank</returns>
		public static string SendErrorEmail(string to, string subject, string body) {
			return SendErrorEmail(to, subject, body, Util.ServerIs());
		}

		/// <summary>
		/// send error email 
		/// </summary>
		/// <param name="to"></param>
		/// <param name="subject"></param>
		/// <param name="body"></param>
		/// <param name="serverType"></param>
		/// <returns>error message or blank</returns>
		public static string SendErrorEmail(string to, string subject, string body, string serverType) {
			string returnValue = "";
			if (!String.IsNullOrEmpty(to)) {
				//defaults
				string from = Beweb.Util.GetSetting("EmailFromAddress", "website@beweb.co.nz");
				string host = Beweb.Util.GetSetting("EmailHost", "localhost"); // local will possibly not work either 
				int port = Convert.ToInt32(Beweb.Util.GetSetting("EmailPort", "25"));
				//port = (port == 0) ? 25 : port;

				var email = new ElectronicMail();
				email.FromAddress = from;
				email.ToAddress = to;
				email.Subject = subject;
				email.BodyPlain = body;
				email.Send(false);
				returnValue = email.ErrorResult;

				//MailMessage message = new MailMessage();
				//message.From = new MailAddress(from);
				////message.To.Add(new MailAddress(to));
				//string[] toAddresses = to.Split(';');
				//foreach (string address in toAddresses)
				//{
				//  message.To.Add(new MailAddress(address));
				//}

				//message.Subject = subject;
				//message.Body = body;

				//SmtpClient client = new SmtpClient(host, port);

				//try 
				//{
				//  client.Send(message);
				//} catch (SmtpException e) 
				//{
				//  if (e.InnerException != null) 
				//  {
				//    returnValue = e.InnerException.Message;
				//  } else 
				//  {
				//    returnValue = e.Message;
				//  }
				//}

			}
			return returnValue;
		}
		#endregion

		#region ResolveUrl
		/// <summary>
		/// ResolveUrl that works when we have no Page or Control
		/// </summary>
		/// <param name="url">a url of the form '~/page.aspx?q=val'</param>
		/// <returns></returns>
		protected static string ResolveUrl(string url) {
			string urlWithNoQueryString = url;
			string queryString = "";
			if (url.Contains("?")) {
				urlWithNoQueryString = url.Substring(0, url.IndexOf("?"));
				queryString = url.Substring(url.IndexOf("?"));
			}
			string resolvedPath = VirtualPathUtility.ToAbsolute(urlWithNoQueryString, HttpContext.Current.Request.ApplicationPath);
			return resolvedPath + queryString;
		}

		#endregion

		/// <summary>
		/// Returns true if we should show detailed errors on screen (ie we are a developer). 
		/// Returns false if we should show friendly errors on screen and send detailed reports via email (ie live or staging server real user mode).
		/// 
		/// This uses the setting ShowDetailedErrorsFromBewebOffice. This setting is true by default and can be set to false if you wish to see errors as users see them. 
		/// For example to show friendly errors to developers on Live, set ShowDetailedErrorsFromBewebOfficeLVE = false.
		/// </summary>
		public static bool ShowDetailedErrors {
			get {
				if (Util.ServerIsDev || Util.IsBewebOffice) {
					return Util.GetSettingBool("ShowDetailedErrorsFromBewebOffice", true);
				}
				return false;
			}
		}

		public static void NotifyJavascriptError(string message, string url, string line, string browser) {
			NotifyJavascriptError(message, url, line, browser, null);
		}

		public static void NotifyJavascriptError(string message, string url, string line, string browser, string pageUrl) {
			if (Util.GetSettingBool("SendJSErrorMessagesToServer", true)) return; //20140916 added new feature to allow turn this off
			if (Util.ServerIsDev) return; //20140326 jn add this to prevent js emails on dev server// MN 20140406 thanks!
			if (message.Contains("unrecognized expression: unsupported pseudo: after")) return; //20130415JN added this - old browser, nothing we can do
			if (message == "Script error." && line == "0") return; //20130415JN added this - dont know what it is

			//var User = Web.UserIpAddress + (Util.IsBewebOffice ? " [Beweb Office]" : "") + "\n" + Web.ServerVars["HTTP_USER_AGENT"];
			//if (Security.IsLoggedIn) {
			//	User += "\nLogged In User ID: " + Security.LoggedInUserID;
			//} else {
			//	User += "\nNot Logged In";
			//}
			//string info = "<style>.diaglabel {font-size:10px;color:#688B9A;margin-top:8px;font-weight:bold;margin-bottom:2px;} .diagdata {font-size:12px;color:#350608;}</style>";
			//info += "<div style='font-family:lucida sans,consolas,arial,sans-serif;background:white;'>\n";
			//info += "<div style='font-size:18px;color:#47697E;'>Javascript Error Report - " + Util.GetSiteName() + " " + Util.ServerIs() + "</div>\n";
			//info += "<div class=diagdata>" + DateTime.Now + "</div>\n";
			//if (Util.ServerIsLive) {
			//	info += "<div style='background-color:#cc0;color:#fff;font-weight:bold;'>LIVE</div>\n";
			//}
			//info += "<div class=diaglabel>Javascript Error Message:</div><div class=diagdata style='font-size:16px;'>" + message.HtmlEncode() + "</div>\n";
			//info += "<div class=diaglabel>Javascript URL Reported:</div><div class=diagdata>" + url.HtmlEncode() + "</div>\n";
			//
			//if(pageUrl != null) {
			//	info += "<div class=diaglabel>Javascript Page URL Reported:</div><div class=diagdata>" + pageUrl.HtmlEncode() + "</div>\n";
			//}
			//
			//info += "<div class=diaglabel>Javascript Line Number:</div><div class=diagdata>" + line.HtmlEncode() + "</div>\n";
			//info += "<div class=diaglabel>User:</div><div class=diagdata>" + Fmt.Text(User) + "</div>\n";
			////info += "<div class=diaglabel>Javascript Browser Reported:</div><div class=diagdata>" + browser.HtmlEncode() + "</div>\n";
			//info += "\n</div>";
			//info += "<div class=diagdata> browser: " + browser + "</div>\n";
			//
			//string subject = String.Format("ERROR on: {0} (Javascript)", ConfigurationManager.AppSettings.Get("ServerIsLVE") ?? Web.UserIpAddress);
			////new ElectronicMail() { ToAddress = SendEMail.EmailAboutError, Subject = subject, BodyHtml = info }.Send(false);      
			//Logging.dlog("JS Error: "+subject+": "+info);
			//Notify(false,subject+": "+info);  //20140916 jn changed to not send error via email, but use normal notify code

			// get full error details containing source snippets and pretty stack trace
			var dump = new Logging.DiagnosticData(null);
			dump.ErrorMessage = "Javascript Error Report";
			//dump.AdditionalMessage="Javascript Error Report";
			dump.AddExtraInfo("Javascript Error Message", message);
			dump.AddExtraInfo("Javascript URL Reported", url);
			dump.AddExtraInfo("Javascript Line Number", line);
			dump.AddExtraInfo("Page URL Reported", pageUrl);
			dump.AddExtraInfo("Browser (from js)", browser);
			//dump.AddExtraInfo("User",  Fmt.Text(User) );

			// log the error report by sending to twitch or emailing
			LogErrorReport(dump);
		}

		public static void PageNotFound(string message) {
			PageNotFound(message, new Logging.DiagnosticData(true));
		}

		public static void PageNotFound(string message, Logging.DiagnosticData dump) {
			string url = "";
			if (Util.GetSettingBool("404GoesToHomepage", false) && Web.Response.ContentType == "text/html") {  // text/html stops images that are broken doing a redirect
				if (Web.Session != null) Web.Session["PageNotFoundMessage"] = message;
				Web.Redirect(NotFoundUrl);
			} else {
				DisplayErrorPage(404, message, dump, false);
			}
		}

		private static string NotFoundUrl {
			get { return Web.ResolveUrlFull("~/Home/NotFound"); }
		}

		public static void DisplayErrorPage(int errorCode, string details, Logging.DiagnosticData dump, bool wasLogged) {
			if (dump == null) {
				dump = new Logging.DiagnosticData(true);
			}

			bool showForm = wasLogged;
			string iframeFormUrl = Util.GetSetting("ErrorFormUrl", "");
			if (iframeFormUrl == "") {
				showForm = false;
			}

			string title = "Oops, there was a problem...";
			if (errorCode == 404) {
				title = "Page not found...";
				details = "<p><b>" + details.HtmlEncode() + "</b></p><p>Sorry, we couldn’t find the page you’re looking for. It may have been deleted or removed.</p>";
				if (ShowDetailedErrors && Util.GetSettingBool("404GoesToHomepage", false) && Web.Response.ContentType == "text/html") {
					details = @"<p><b>404 Goes To Homepage</b> - The standard users would be sent to the <b>homepage</b> (<a href=""" + NotFoundUrl + @""">like this</a>), except you are in developer mode so instead you see details below.</p>";
				}
			} else {
				if (dump.ErrorMessage != null && dump.ErrorMessage.Contains("anti-forgery token")) {
					details = @"<p>The most common cause of what just happened is that you have cookies turned off, or didn't allow this site to save one.</p>
			<p>This site needs to use a cookie to work, it's what provides security between you and us.</p>
			<h3 style=""margin-top: 30px;"">What can I do to fix it?</h3>
			<p>Here is how you can turn cookies on: <a href=""http://support.google.com/accounts/bin/answer.py?hl=en&answer=61416"" target=""_blank"">http://support.google.com/accounts/bin/answer.py?hl=en&answer=61416</a></p>
			<p>Then try either ""refresh"" or ""back"" on your browser, or return to the home page.</p>""";
				} else if (dump.ErrorMessage != null && dump.ErrorMessage.Contains("A potentially dangerous")) {
					details = @"<p>Your input was blocked. You might have submitted some HTML code or other unsafe characters.</p><p>Please hit ""back"" on your browser to correct your information and try submitting it again.</p>";
				} else if (wasLogged) {
					details = @"<p>There has been a problem with the web site. The issue has been logged for analysis.</p>";
					if (showForm) {
						details += @"<p>You can also increase the priority of this fault by reporting the issue using the form below.</p>";
					}
				} else if (ShowDetailedErrors) {
					details = @"<p>There has been a problem with the web site. The issue would be logged for standard users, except you are in developer mode so instead you see details below.</p>";
				} else {
					details = @"<p>A temporary problem was detected.</p><p>Please try either ""refresh"" or ""back"" on your browser, or return to the home page and try again.</p>";
				}
			}
			string finalSource = GetStandardErrorPageHtml();
			finalSource = finalSource.Replace("[title]", title);
			finalSource = finalSource.Replace("[details]", details);
			finalSource = finalSource.Replace("[DateTime.Now]", DateTime.Now.ToString());
			finalSource = finalSource.Replace("[ipaddress]", Web.ServerVars["REMOTE_ADDR"]);
			finalSource = finalSource.Replace("[errorcode]", dump.HttpStatusCode.ToString());
			finalSource = finalSource.Replace("[ref]", wasLogged ? dump.ErrorReportGuid.ToString() : "n/a");
			if (showForm) {
				finalSource = finalSource.Replace("[iframe]", "<iframe src='" + iframeFormUrl + "?ErrorReportGuid=" + dump.ErrorReportGuid + "' style='border:none;width:100%;height:300px;'></iframe>");
			} else {
				finalSource = finalSource.Replace("[iframe]", "");
			}

			// also output developer only detailed message if we are supposed to
			if (ShowDetailedErrors) {
				finalSource = finalSource.Replace("[devdetails]", "<a href='' onclick=''></a>" + dump.ToHtml());
			} else {
				finalSource = finalSource.Replace("[devdetails]", "");
			}

			// if possible, clear any content buffered but not yet output and set the status code header correctly
			try {
				Web.Response.ClearContent();
				Web.Response.TrySkipIisCustomErrors = true;
				Web.Response.StatusCode = errorCode; // if already sending content, this will fail, but we don't care, we still want to see the original error display
			} catch {
				// ignore
			}
			// output friendly error message
			Web.Write(finalSource);

			// end the response
			try {
				HttpContext.Current.Response.End();
			} catch {
				// ignore
			}
		}

		private static string GetStandardErrorPageHtml() {
			string html = @"<!DOCTYPE html>
<html>
	<head>
		<title>[title]</title>
		<style>
		h1 { font-weight: normal; font-size:30px; color:#333; }
		</style>
	</head>
<body>
	<div style=""margin:auto; max-width:980px;font: normal 14px/23px Calibri, Helvetica Neue, Helvetica, Arial, sans-serif;"" id=usermsg>
		<img src=""~/images/maintenance.jpg"" width=400 style=""float:right;margin-left:20px;"" />
		<h1>[title]</h1>
		[details]
		<p>We are sorry for the inconvenience caused.</p>			
		<p><a href=""~/"" class=btn>Return to home page</a></p>			
		<p>
			Technical details:<br/>
			<span style=""color: #999"">
				Error ocurred: [DateTime.Now]<br/>
				IP address: [ipaddress]<br/>
				HTTP code: [errorcode]<br/>
				Reference: [ref]<br/>
			</span>
		</p>
		
		[iframe]
	</div>
	<div style=""margin:auto; max-width:980px;"" ondblclick=""this.style.maxWidth='1600px';document.getElementById('usermsg').style.display='none'"">
		[devdetails]
	</div>
</body>
</html>";
			html = html.Replace("~/", Web.Root);
			//html = html.Replace("[errorReportID]", Web.Root);
			return html;
		}


	}

	public class BewebException : ApplicationException {
		public BewebException(string message) : base(message) { }
		public BewebException(string message, Exception innerException) : base(message, innerException) { }
	}
}
