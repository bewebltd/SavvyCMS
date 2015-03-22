using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Site.SiteCustom;

namespace Site.Controllers {
	public class ApplicationController : Controller {
		//
		// GET: /Application/

		protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
			// 20140511 MN - removed, bugs could be lurking... if(Util.ServerIsDev)PageCache.Rebuild(); //20140326 jn added this to prevent stupid db cache on dev server
			base.Initialize(requestContext);
		}


		/// <summary>
		/// Checks that the current user is allowed access to this controller. 
		/// If you are an Administrator, SuperAdmin or Developer you have access to all admin pages (defined by AdminBaseController).
		/// Note: to allow custom roles to access a few pages of the admin, modify SecurityRoles.GetAllowedAdminControllers()
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnAuthorization(AuthorizationContext filterContext) {

			//if (Security.AuthProviderIsSavvy) {
			//	bool requireAuth = filterContext.ActionDescriptor.IsDefined(typeof(AuthorizeAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AuthorizeAttribute), true);
			//	requireAuth = requireAuth && !filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
			//	if (requireAuth && !Security.IsLoggedIn) {
			//		Security.KickOut("Please log in.");
			//		return;
			//	}
			//} else {
			//	base.OnAuthorization(filterContext);
			//}

			base.OnAuthorization(filterContext);

			string allowedControllerList = SecurityRoles.Roles.GetAllowedSiteControllers();
			if (allowedControllerList.Contains("(all)")) {
				//all good carry on
			} else if (allowedControllerList.Contains("(except[")) { 
				// JC added 20141105 - check for controller name and deny access if it exists within except[]
				// Example (except[securepage,supersecurepage])  securepagecontroller and supersecurepagecontrollers will be denied access.
				string[] allowedControllers = allowedControllerList.ExtractTextBetween("[", "]").Split(',');
				if (allowedControllers.Contains(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)) {
					if (Security.AuthProviderIsSavvy) {
						Security.KickOut("Your login is not authorised to access this section.");
						return;
					} else {
						filterContext.Result = new HttpUnauthorizedResult();
					}
				}
			} else {
				// check for controller name and deny access if it doesn't exist
				// Example - (page,event,news) pagecontroller, eventcontroller and newscontroller will be allowed access the rest will be denied.
				string[] allowedControllers = allowedControllerList.Split(',');
				if (!allowedControllers.Contains(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)) {
					if (Security.AuthProviderIsSavvy) {
						Security.KickOut("Your login is not authorised to access this section.");
						return;
					} else {
						filterContext.Result = new HttpUnauthorizedResult();
					}
				}
			}


			//if (Security.AuthProviderIsASPNET) {
			//	base.OnAuthorization(filterContext);
			//	string allowedControllerList = SecurityRoles.Roles.GetAllowedSiteControllers();
			//	if (!allowedControllerList.Contains("(all)")) {
			//		// check for controller name match 
			//		string[] allowedControllers = allowedControllerList.Split(',');
			//		if (!allowedControllers.Contains(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName)) {
			//			filterContext.Result = new HttpUnauthorizedResult();
			//		}
			//	}
			//}else if (Security.AuthProviderIsSavvy) {
			////todo: as above check if page requires authorisation, and if so, ask person for their password...
			//}


		}

		/// <summary>
		/// Called before the action method is invoked.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			if (Util.IsBewebOffice || Util.ServerIsDev) {
				Sql.LogPageProcessingStart();
			}
			base.OnActionExecuting(filterContext);
		}

		/// <summary>
		/// Called after the action method is invoked.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		protected override void OnActionExecuted(ActionExecutedContext filterContext) {
			base.OnActionExecuted(filterContext);
			if (Util.IsBewebOffice || Util.ServerIsDev) {
				Sql.LogPageProcessingEnd();
			}
		}

		/// <summary>
		/// Called after the action result that is returned by an action method is executed.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action result</param>
		protected override void OnResultExecuted(ResultExecutedContext filterContext) {
			if (Util.IsBewebOffice || Util.ServerIsDev) {
				if (Session != null) {
					Session["PreviousRequestUrl"] = Session["CurrentRequestUrl"];
					Session["PreviousTraceLogHtml"] = Session["CurrentTraceLogHtml"];
					Session["CurrentRequestUrl"] = Web.LocalUrl;
					Session["CurrentTraceLogHtml"] = Sql.GetTraceLogHtml();
				}
			}

			// MN 2015 - moved this to enable live sql logging for all users
			if (Util.GetSettingBool("DebugLogTraceSQL", false)) {
				Logging.sqllog(Sql.GetTraceLog());
			}
		}

		//protected override void OnException(ExceptionContext filterContext) {
		//  Beweb.Error.Notify();
		//  Redirect("~/error/500.aspx");
		//  filterContext.ExceptionHandled=true;
		//}


	}

	public class SavvyAuthorizeAttribute : AuthorizeAttribute {
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			if (!Security.AuthProviderIsSavvy) {
				return base.AuthorizeCore(httpContext);
			}
			return Security.IsLoggedIn;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			if (Security.AuthProviderIsSavvy) {
				Security.KickOut("Your login is not authorised to access this section.");
				return;
			} else {
				base.HandleUnauthorizedRequest(filterContext);
			}
		}

		public override void OnAuthorization(AuthorizationContext filterContext) {
			base.OnAuthorization(filterContext);
		}
	}

}

