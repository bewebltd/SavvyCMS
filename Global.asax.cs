using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Beweb;
using Models;
using SavvyMVC.Util;
using Site.SiteCustom;
using Site.Controllers;

namespace Site.SiteCustom {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterRoutes(RouteCollection routes) {
			string[] siteNamespace = new string[] {"Site.Controllers"};

			// when set to TRUE this means that if a file exists and also a route matches, the routes win
			routes.RouteExistingFiles = false;

			routes.MapRoute("Robots", "robots.txt", new { controller = "UrlRedirect", action = "Robotos" });

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			//routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*staticfiles}", new {staticfiles=@".*\.(css|js|gif|jpg|ico|txt)"});  // removed png for dynamicimages
			//routes.IgnoreRoute("{*allaspx}", new {allaspx=@".*\.aspx(/.*)?"});
			//routes.IgnoreRoute("{*allasp}", new {allasp=@".*\.asp"});        // leave classic asp
			//routes.IgnoreRoute("");                // don't take over the root

			// errors
			routes.MapRoute("LogJavascriptError", "Error/LogJavascriptError", new { controller = "Error", action = "LogJavascriptError" });
			routes.MapRoute("FixError", "Error/FixError", new { controller = "Error", action = "FixError" });
			if (Util.GetSettingBool("404GoesToHomepage", false)) {
				routes.MapRoute("errorNotFound", "Error/NotFound", new { controller = "Home", action = "NotFound" });
				routes.MapRoute("error404", "Error/404", new { controller = "Home", action = "NotFound" });
			} else {
				routes.MapRoute("errorNotFound", "Error/NotFound", new { controller = "Error", action = "NotFound" });
				routes.MapRoute("error404", "Error/404", new { controller = "Error", action = "NotFound" });
			}
			routes.MapRoute("error", "Error/{errorPage}", new { controller = "Error", action = "ShowErrorPage" });
			routes.MapRoute("trackingGif", "Track/{guid}", new { controller = "Common", action = "TrackingGif" });
			// images
			routes.MapRoute("RenderDynamicImage", "i/{id}_{version}/{crunched_title}.png", new { controller = "Images", action = "RenderDynamicImage" });

			// ----------------------

			// home
			routes.MapRoute("home", "", new { controller = "Home", action = "Index" });
			routes.MapRoute("default.aspx", "default.aspx", new { controller = "Home", action = "Index" });

			// ----------------------

			// document downloads
			// root categories, must be assigned to a page with a template of 'documentrepository'
			routes.MapRoute("documentrepository", "DocumentRepository", new { controller = "DocumentCategory", action = "ByPageID" });

			// all other sub categories and documents
			routes.MapRoute("documentcategory", "DocumentCategory/{enccategoryid}", new { controller = "DocumentCategory", action = "ByCategoryID" });
			routes.MapRoute("downloaddocument", "DownloadDocument/{encryptedID}", new { controller = "Download", action = "DownloadDocument" });
			
			// search
			routes.MapRoute("search", "Search", new {controller = "Search", action = "Search"});

			// XML Sitemap
			routes.MapRoute("XMLSitemap", "XMLSitemap", new {controller = "XML", action = "GetSitemapXML"});

			routes.MapRoute(
				"pageid", // Route name
				"Page/{id}/{crunched_title}", // URL with parameters
				new {controller = "StandardPage", action = "ByPageID"}, // match pattern
				new {id = @"\d+"} // Parameter defaults
				);

			//pages
			routes.MapRoute("page", "Page/{*urlRewriteTitle}", new {controller = "StandardPage", action = "Index"});
			
			// standard beweb route
			routes.MapRoute(
				"BewebModels", // Route name
				"{controller}/{id}/{crunched_title}", // URL with parameters
				new { action = "Index" }, // Parameter defaults
				new { controller = MvcUtil.GetSiteControllerNamesPattern(), id = @"\d+" }, // match pattern
				siteNamespace // namespaces
				);

			// standard MVC route
			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
				new { controller = MvcUtil.GetSiteControllerNamesPattern() },
				siteNamespace // namespaces
				);
			
			// note this will take precedence over the UrlRedirectController below for any single word path (eg "/whatever")
			// good to use for small sites where UrlRedirectController is not used 
			//routes.MapRoute("pagecode", "{pagecode}", new {controller = "StandardPage", action = "ByPageCode"});

			routes.MapRoute("catchall", "{*incomingUrl}", new {controller = "UrlRedirect", action = "CantBeRouted"}, siteNamespace);

		}

		protected void Application_Start() {
			// Code that runs on application startup
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			if (Util.GetSettingBool("ShowRouteDebugger", false)) {
				RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
			}

			// set default core settings
			BewebCoreSettings.Settings = new BewebCoreSettings();
		}

		//------------------------------------------------------------
		// BEWEB Standard Global ASAX
		//------------------------------------------------------------

		void Application_End(object sender, EventArgs e) {
			//  Code that runs on application shutdown
		}

		void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started
			string sessionId = Session.SessionID;   // workaround to prevent asp.net error Session state has created a session id, but cannot save it because the response was already flushed by the application
			SiteMain.UsersOnline++;
		}

		void Session_End(object sender, EventArgs e) {
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.
			SiteMain.UsersOnline--; //20131017jn changed to --
		}

		protected void Application_BeginRequest(object sender, EventArgs e) {
			//HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
			// HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://AllowedDomain.com");
			// redirect all requests to a new site - this is useful for moving sites, eg while waiting for DNS to propagate
			//Web.Redirect(Web.FullRawUrl.Replace(Web.Host, "sitenamehere.beweb.co.nz"));
			//Web.End();

			// request file limit check (stops too big file uploads before they happen)
			string redirectUrl = String.Empty;
			var url = "~/error/filetoobig.aspx?max={0}&this={1}";
			int iMaxFileSize = Convert.ToInt32(Util.GetSetting("MaxFileUploadSizeInMegaBytes","50")) * 1024 * 1024; // convert to bytes
			HttpFileCollection rf = null;
#region              Stats
#if Keystone				
Keystone.Keystone.Initialise(Beweb.Util.GetSiteName());
#endif
#endregion 
			try {
				rf = Request.Files;
			} catch (HttpException) {
				url = String.Format(url, iMaxFileSize, Request.ContentLength);
				redirectUrl = Web.ResolveUrl(url);
			}

			if (rf != null) {
				for (int i = 0; i < rf.Count; i++) {
					if (Request.Files[i].ContentLength > iMaxFileSize) {
						url = String.Format(url, iMaxFileSize, Request.Files[i].ContentLength);
						redirectUrl = Web.ResolveUrl(url);
						break;
					}
				}
			}

			if(String.IsNullOrEmpty(redirectUrl)) {
				SiteMain.CheckForRedirectUrls();
			}else {
				Response.Redirect(redirectUrl);
			}
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
			Security.AuthenticateRequest();
			//	if (HttpContext.Current.User != null) {
			//	if (HttpContext.Current.User.Identity.IsAuthenticated) {
			//		if (HttpContext.Current.User.Identity is FormsIdentity) {
			//			// set the user role
			//			FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity; // Get Forms Identity From Current User
			//			FormsAuthenticationTicket ticket = id.Ticket; // Get Forms Ticket From Identity object
			//			// Retrieve stored user-data (contains comma separated roles)
			//			string userData = ticket.UserData;
			//			string[] roles = userData.Split(',');

			//			// Set the current user to the roles we specified
			//			// Create a new Generic Principal Instance and assign to Current User
			//			HttpContext.Current.User = new GenericPrincipal(id, roles);
			//		}
			//	}
			//}
		}

		protected void Application_Error() {
			// Code that runs when an unhandled error occurs
			Beweb.Error.Notify();
		}
	}
}