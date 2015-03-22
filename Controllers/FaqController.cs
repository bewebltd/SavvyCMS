#define pages

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class FaqController : Controller {

		public ActionResult Index() {
			var data = new ViewModel();

			data.ContentPage = Models.Page.LoadPageContent("Faq");

			TrackingBreadcrumb.Current.AddBreadcrumb(1, "FAQs");

			data.FaqSectionList = FAQSectionList.LoadActive();

			return View("Faq", data);
		}


		public class ViewModel : PageTemplateViewModel {
			public FAQSectionList FaqSectionList;
		}


	}
}
