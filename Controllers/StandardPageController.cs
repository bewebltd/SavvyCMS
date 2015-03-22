using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Site.SiteCustom;
using Beweb;

namespace Site.Controllers {
	public class StandardPageController : ApplicationController {
		//
		// GET: /StandardPage/

		//[SiteCustom.SiteOutputCache]
		public ActionResult ByPageID(int id) {
			var page = Models.Page.LoadID(id);
			if (page == null) {
				throw new Beweb.BadUrlException("Page not found with ID of [" + id + "]");
			}
			return ReturnView(page);
		}

		//[SiteCustom.SiteOutputCache]
		public ActionResult ByPageCode(string pageCode) {
			var page = PageCache.GetByPageCode(pageCode);
			//var page = Models.Page.LoadByPageCode(pageCode);
			if (page == null) {
				throw new Beweb.BadUrlException("Page not found with page code of [" + pageCode + "]");
			}
			return ReturnView(page);
		}

		//[SiteCustom.SiteOutputCache]
		public ActionResult Index(string urlRewriteTitle) {
			var page = Models.Page.LoadByURLRewriteTitle(urlRewriteTitle);
			if (page == null) {
				throw new HttpException(404, "Page not found. This page may have been removed.");
			}
			return ReturnView(page);
		}

		private ActionResult ReturnView(Page page) {
			var data = new ViewModel(page);
			if (page.TemplateCode == "sectionfront") { // you can add more template codes like this
				return View("SectionFront", data);
			} else {
				return View("StandardPage", data);
			}
		}

		public class ViewModel : PageTemplateViewModel {
			public ViewModel(Models.Page page) {
				// check for admin preview
				if (Web.Request["preview"] == "adminonly") {
					// force login if not already
					if (!Security.IsLoggedIn) {
						throw new Beweb.ProgrammingErrorException("force login required");
					}
				} else if (!page.GetIsActive()) {
					if (!Security.IsLoggedIn) {
						throw new Beweb.BadUrlException("Page not available with ID of [" + page.ID + "]");
					}
				}

				//page.CheckUserAccess();  			// uncomment this if using page user access control

				TrackingBreadcrumb.Current.AddBreadcrumb(1, page.Title);
#if pages
				ContentPage = page;
#endif
			}

			//public bool IsStory { get { return ContentPage.TemplateCode=="Story"; }}
			public string SectionTitle {
				get {
#if pages
					return ContentPage.SectionTitle;
#else
					return null;
#endif
				}
			}

#if DocumentRepository
			public bool HasDocumentCategories {
				get {
					return (this.ContentPage.DocumentCategories.Count > 0);
				}		
			}
#endif
		}
	}
}
