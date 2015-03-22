using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;

namespace Site.Controllers {
	public class UrlRedirectController : ApplicationController {
		//public ActionResult Index(int id) {
		//  var urlRedirect = UrlRedirect.LoadID(id);
		//  if (urlRedirect==null) throw new HttpException(404, "UrlRedirect not found");
		//  string url = Web.BaseUrl + urlRedirect.RedirectToUrl;
		//  Web.RedirectPermanently(url);
		//}
	
		// note: do not cache this
		public ActionResult CantBeRouted(string incomingUrl) {
			// note for future development
			// any other dynamic url routing can be done here
			// could even have this loop through all the controllers and ask them if they want the request

			// check Url Redirects loaded into CMS
			//var urlRedirect = UrlRedirect.LoadByRedirectFromUrl(incomingUrl);   // relative url without querystring? to test to see if it finds the one without qs first
			string localPageUrl = Web.LocalUrl.LeftUntil("?");
			if(localPageUrl.EndsWith("/")) {
				localPageUrl = localPageUrl.RemoveCharsFromEnd(1); //remove traling slash as the next line is like url anyway JC 20140423
			}
			var urlRedirects = UrlRedirectList.Load(new Sql("where RedirectFromUrl like", localPageUrl.SqlizeLike(), " order by UrlRedirectID")); // absolute url with querystring
			if (urlRedirects.Count > 0) {
				var urlRedirect = urlRedirects.FirstOrDefault(redir => redir.RedirectFromUrl == Web.FullRawUrl); // absolute url with querystring
				if (urlRedirect == null) {
					urlRedirect = urlRedirects.FirstOrDefault(redir => redir.RedirectFromUrl == Web.LocalUrl); // relative url with querystring
				}
				if (urlRedirect == null) {
					urlRedirect = urlRedirects.FirstOrDefault(redir => redir.RedirectFromUrl == Web.PageUrl); // absolute url without querystring
				}
				if (urlRedirect == null) {
					urlRedirect = urlRedirects.FirstOrDefault(redir => redir.RedirectFromUrl == localPageUrl); // relative url without querystring
				}
				if (urlRedirect != null) {
					string newUrl = urlRedirect.RedirectToUrl;
					if (!Web.IsAbsoluteUrl(newUrl)) {
						newUrl = Web.BaseUrl + newUrl;
					}
					Web.RedirectPermanently(newUrl);
					// MK 20110531: Don't return content after RedirectPermanently
					return null;
				}
			}

			// check for urls from old site
			if (RedirectLegacyUrls(incomingUrl)) return null;

			// page not found
			Error.PageNotFound(incomingUrl);
			return null;
		}

		private static string robotos = null;

		public ActionResult Robotos() {
			if (robotos == null) {
				string filename = "~/robots" + Util.ServerIs() + ".txt";
				if (FileSystem.FileExists(filename)) {
					robotos = FileSystem.ReadTextFileAutoDetectEncoding(filename);
				} else {
					if (!Util.ServerIsLive) { // dont do this on live as it breaks default.
						robotos = "User-agent: *\nDisallow: /";
					}
				}
			}
			return Content(robotos, "text/plain");
		}

		public bool RedirectLegacyUrls(string incomingUrl) {
			// check for old URLs and redirect them
			bool isLegacyUrlFound = false;
			/* example code:
			if (incomingUrl.Contains("bikedetails.asp")) {
				string modelID = Web.Request["modelID"];
				var bike = BikeModel.LoadID(modelID.ToInt(0), Otherwise.NotFound);
				Web.RedirectPermanently(bike.GetUrl());
				isLegacyUrlFound = true;
			} else if (incomingUrl.Contains("newsitem.asp")) {
				string storyID = Web.Request["newsid"];
				var story = News.LoadID(storyID.ToInt(0), Otherwise.NotFound);
				Web.RedirectPermanently(story.GetUrl());
				isLegacyUrlFound = true;

			} else if (incomingUrl.Contains(".asp") && incomingUrl.DoesntContain(".aspx")) {
				Web.RedirectPermanently(Web.BaseUrl);
				isLegacyUrlFound = true;
			}
			*/
			return isLegacyUrlFound;
		}

		/// <summary>
		/// All controllers that handle a route but then decide they cannot find the actual content (eg record not found) should pass over to here
		/// </summary>
		public static ActionResult TakeOverRouting() {
			new UrlRedirectController().CantBeRouted(Web.LocalUrl);
			return null;
		}
	}
}

