using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Beweb;
using Savvy;

namespace SavvyMVC {
	/// <summary>
	/// This is the base class for all controllers in Admin section
	/// </summary>
	[ValidateInput(false)]            // disable request validation (ie the feature where HTML is not allowed to be posted in a form)
	public class AdminBaseController : Controller {
		protected ActionResult RedirectToEdit(int id2) {
			return RedirectToAction("Edit", new {id=id2, df_returnpage=Breadcrumbs.Current.GetReturnPage()});
			//return Redirect("" + id + "?df_returnpage=" + Web.Request["df_returnpage"].UrlEncode());
		}

		protected ActionResult RedirectToReturnPage() {
			return Redirect(Breadcrumbs.Current.GetReturnPage());
		}
	}

}
