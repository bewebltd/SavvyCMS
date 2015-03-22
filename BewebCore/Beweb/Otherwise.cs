using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Beweb {
	/*public enum Otherwise {
		Null, New, NotFound, Error
	}

	public class OtherwisePerformer {
		public void DoAction(Otherwise otherwise) {
					if (otherwise == Otherwise.NotFound) {
						throw new HttpException(404, "Resource Not Found: Deal");
					} else if (otherwise == Otherwise.Error) {
						throw new Exception("Resource Not Found: Deal");
					} else if (otherwise == Otherwise.Null) {
						return null;
					}
		}
	}*/

	public class Otherwise {
		internal enum OtherwiseAction {
			Null, New, NotFound, ProgrammingError, AdminError, NullButNotifyProgrammer, NullButNotifyAdmin
		}

		internal OtherwiseAction actionType;

		private Otherwise(OtherwiseAction actionType) {
			this.actionType = actionType;
		}

		/// <summary>
		/// If there is a problem returning the data, just return null instead. Your code then needs to check for null and do the appropriate thing (eg skip displaying something, provide a default, throw a descriptive error, or send an email).
		/// </summary>
		public static Otherwise Null { get { return new Otherwise(OtherwiseAction.Null); } }

		/// <summary>
		/// If there is a problem returning the data, create a new object of this type (set to default values) and return that instead.
		/// </summary>
		public static Otherwise New { get { return new Otherwise(OtherwiseAction.New); } }

		/// <summary>
		/// If there is a problem returning the data, go to the 404 Not Found page. This should be done when the URL entered is invalid (eg data has been deleted or renamed or never existed). This will cause search engines to remove this URL from their index.
		/// </summary>
		public static Otherwise NotFound { get { return new Otherwise(OtherwiseAction.NotFound); } }

		/// <summary>
		/// If there is a problem returning the data, it is a fatal error which needs a programmer to fix. A 500 server error page will be displayed and the error message will be sent to the programmers.
		/// </summary>
		public static Otherwise ProgrammingError { get { return new Otherwise(OtherwiseAction.ProgrammingError); } }

		/// <summary>
		/// If there is a problem returning the data, it is fatal error, but the error notification should be directed to the website CMS administrator to correct.
		/// </summary>
		public static Otherwise AdminError { get { return new Otherwise(OtherwiseAction.AdminError); } }

		/// <summary>
		/// If there is a problem returning the data, return null, but also email a notification to the programmer to fix.
		/// </summary>
		public static Otherwise NullButNotifyProgrammer { get { return new Otherwise(OtherwiseAction.NullButNotifyProgrammer); } }

		/// <summary>
		/// If there is a problem returning the data, return null, but also email a notification to the website CMS administrator.
		/// </summary>
		public static Otherwise NullButNotifyAdmin { get { return new Otherwise(OtherwiseAction.NullButNotifyAdmin); } }

		//public static Otherwise GoReturnPage { get { return new Otherwise(OtherwiseAction.GoReturnPage); } }

		public T Execute<T>(T example) where T : class, new() {
			return Execute<T>();
		}

		// note that T must be a class that can be "newed" because one of the otherwise options is "new"
		public T Execute<T>() where T : class, new() {
			string typeDescription = Fmt.SplitTitleCase(typeof(T).Name);
			// the next line wasn't being used - it caused a call to "Request" which isn't available all the time (eg AppStart)
			//string detailedMessage = "Resource type: " + typeDescription + "\nURL: " + Web.FullRawUrl + "\nTime: " + DateTime.Now + "\n";
			string message = typeDescription + " not found: " + Web.LocalUrl;
			if (actionType == OtherwiseAction.Null) {
				return null;
			} else if (actionType == OtherwiseAction.New) {
				return new T();
			} else if (actionType == OtherwiseAction.NotFound) {
				//TODO: MN 20140407 Web.Redirect("~/CantBeRouted?incomingUrl=" + Web.LocalUrl.UrlEncode());
				Error.PageNotFound("Resource not found. " + message);
				return null;
				//throw new BadUrlException(message);
			} else if (actionType == OtherwiseAction.ProgrammingError) {
				throw new ProgrammingErrorException(message);
			} else if (actionType == OtherwiseAction.AdminError) {
				throw new AdminErrorException(message);
			} else if (actionType == OtherwiseAction.NullButNotifyProgrammer) {
				new ElectronicMail() { ToAddress = SendEMail.EmailAboutError, Subject = Util.GetSiteName() + " Website Non-fatal Error Notification", BodyHtml = "Please address the following non-fatal issue with the website:<br>" + message + "<br>" + Logging.GetDiagnosticsDumpHtml() }.Send(false);
				//SendEMail.SimpleSendHtmlEmail(SendEMail.EmailAboutError, Util.GetSiteName() + " Website Non-fatal Error Notification", "Please address the following non-fatal issue with the website:\n" + message+Logging.GetDiagnosticsDumpHtml());
				return null;
			} else if (actionType == OtherwiseAction.NullButNotifyAdmin) {
				new ElectronicMail() { ToAddress = SendEMail.EmailAboutError, Subject = Util.GetSiteName() + " Website Problem Notification", BodyHtml = "Please address the following issue with the website:<br>" + message }.Send(false);
				//SendEMail.SimpleSendEmail(SendEMail.EmailToAddress, Util.GetSiteName() + " Website Problem Notification", "Please address the following issue with the website:\n" + message);
				return null;
			} else {
				// eat own dog food
				throw new ProgrammingErrorException("Otherwise: Invalid otherwise case");
			}
			//if (actionType == OtherwiseAction.NotFound) {
			//  throw new HttpException(404, "Resource Not Found: "+typeDescription);
			//} else if (actionType == OtherwiseAction.Error) {
			//  throw new Exception("Resource Not Found: "+typeDescription);
			//} else if (actionType == OtherwiseAction.GoReturnPage) {
			//  Web.InfoMessage = "Cannot find  "+typeDescription+".";
			//  Web.Redirect(Savvy.Breadcrumbs.Current.GetReturnPage());
			//}

		}
	}

	/// <summary>
	/// Sends an email to CMS administrator and goes to 500 page. This should be thrown when this is a content or administrative issue that the CMS administrator or website owner needs to correct. 
	/// </summary>
	public class AdminErrorException : Exception {
		public AdminErrorException(string message) : base(message) { }
	}

	/// <summary>
	/// Goes to 404 page but also shows given message. This should be thrown when the current URL is invalid because its parameters point to data that does not exist. Therefore user needs to be told this page doesn't exist and this is not a programming error that Beweb needs to fix.
	/// </summary>
	public class BadUrlException : HttpException {
		public BadUrlException(string message) : base(404, message) { }
		public BadUrlException(string message, Exception innerException) : base(404, message, innerException) { }
	}

	/// <summary>
	/// Sends an email to developers and goes to 500 page. This should be thrown when this is a programming error that Beweb needs to fix.
	/// </summary>
	public class ProgrammingErrorException : Exception {
		public ProgrammingErrorException(string message) : base(message) { }
		public ProgrammingErrorException(string message, Exception innerException) : base(message, innerException) { }
	}

	/// <summary>
	/// Validation error which the user can correct themselves. This should be thrown when the error should be shown on-screen for the user.
	/// On a standard CMS form there is a "catch" which displays this error to the user. In other code it will actually act like a Programming Error.
	/// </summary>
	public class UserErrorException : Exception {
		public UserErrorException(string message) : base(message) { }
		public UserErrorException(string message, Exception innerException) : base(message, innerException) { }
	}


}

