using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Site.SiteCustom;
using Beweb;

namespace Site.Controllers {
	public class GalleryCategoryController : ApplicationController {
		//
		// GET: /GalleryCategory/1/gallery-name

		//[SiteCustom.SiteOutputCache]

		public ActionResult Index(int id) {
			var category = Models.GalleryCategory.LoadID(id);
			if (category == null) {
				throw new Beweb.BadUrlException("Category not found with ID of [" + id + "]");
			}
			var data = new ViewModel(category);
			return View("GalleryCategory", data);
		}


		public class ViewModel : PageTemplateViewModel {

			public GalleryCategory Category;
			
			public ViewModel(Models.GalleryCategory category) {
				// check for admin preview
				if (Web.Request["preview"] == "adminonly") {
					// force login if not already
					if (!Security.IsLoggedIn) {
						throw new Beweb.ProgrammingErrorException("force login required");
					}
				} else if (!category.GetIsActive()) {
					throw new Beweb.BadUrlException("Category not available with ID of [" + category.ID + "]");
				}
				Category = category;
				PageTitleTag = category.Title;
				//category.CheckUserAccess();  			// uncomment this if using page user access control

				TrackingBreadcrumb.Current.AddBreadcrumb(2, category.Title);
			}

		}
	}
}
