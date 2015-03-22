using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Beweb;

namespace Site.SiteCustom {
	public class TrackingBreadcrumb : Savvy.Breadcrumbs {

		public TrackingBreadcrumb() {
			WebsiteBaseCodeName = "FrontEnd";
			CurrentRequestObject = Web.Request;
			CurrentSessionObject = Web.Session;
			IsAdminSection = false;
		}

		/// <summary>
		/// Returns a reference to the current breadcrumbs object so you can call its methods without having to use "new Breadcrumbs()"
		/// </summary>
		public new static TrackingBreadcrumb Current {get {return new TrackingBreadcrumb();}}

		public string GetFooterBreadcrumbLinks() {
			return GetBreadcrumbLinks("", true);
		}

		public override void InitBread() {
			SetBreadcrumb(0, "Home", Web.ResolveUrl("~/"));        // change "Home" to whatever you like
		}
	}
}
