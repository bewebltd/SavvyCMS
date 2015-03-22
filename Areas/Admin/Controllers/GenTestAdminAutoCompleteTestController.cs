// Admin GenTest Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public partial class GenTestAdminController : AdminBaseController {
		//
		// GET: /Admin/GenTest/

		public ActionResult GenTestEditAutocomplete() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Gen Test - Autocomplete");
			var record = new Models.GenTest();
			// any default values can be set here or in partial class GenTest.InitDefaults() 
			data.GenTest = record;
			return View("GenTestEditAutocomplete", data);
		}
				/// <summary>
		/// Saves a new record
		/// POST: /Admin/GenTest/Create
		/// </summary>
		[HttpPost]
		public ActionResult GenTestEditAutocomplete(FormCollection collection) {
			return ProcessForm(new Models.GenTest());
		}

	}
}
