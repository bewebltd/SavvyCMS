using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Site.Controllers;
using Site.SiteCustom;

namespace Site.SiteCustom {
	public class ViewMaster : ViewMasterPage<PageTemplateViewModel> {
		protected override void Construct() {
#if Keystone
			#region              Stats
				Keystone.Keystone.Initialise(Beweb.Util.GetSiteName());
			#endregion
#endif
			base.Construct();
		}
	}
}
