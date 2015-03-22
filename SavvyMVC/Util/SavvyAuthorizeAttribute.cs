using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Beweb;

namespace Beweb {
	public class SavvyAuthorizeAttribute : AuthorizeAttribute {
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			if (!Security.AuthProviderIsSavvy) {
				return base.AuthorizeCore(httpContext);
			}
			return Security.IsLoggedIn;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			if (Security.AuthProviderIsSavvy) {
				Security.KickOut("Sorry. Your login is not authorised to access this section.");
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
