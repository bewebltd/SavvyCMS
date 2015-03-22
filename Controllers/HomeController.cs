//#define pages
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class HomeController : ApplicationController {
		//
		// GET: /Home/

		//[SiteCustom.SiteOutputCache]
		public ActionResult Index() {
			var data = new ViewModel();
			TrackingBreadcrumb.Current.AddBreadcrumb(0, "Home");
#if pages
			data.ContentPage = Models.Page.LoadOrCreatePageCode("Home");
			if (data.ContentPage==null) throw new Exception("Home page not found");
#endif

#if CarouselBootstrap || SavvyCarousel || DumbCarousel
			data.ShowSlideShow = true;
#endif

			return View("Home", data);
		}


		public class ViewModel : PageTemplateViewModel {
			public bool ShowSlideShow = false;
		} 

		public ActionResult Carousel() {
#if SavvyCarousel
			var data = new SlideShowViewModel();
			data.Slides = Models.HomepageSlideList.LoadActive();
			return View("SavvyCarousel", data);
#elif CarouselBootstrap
			var data = new SlideShowViewModel();
			data.Slides = Models.HomepageSlideList.LoadActive();
			return View("CarouselBootstrap", data);
#elif DumbCarousel 
			var data = new SlideShowViewModel();
			data.Slides = Models.HomepageSlideList.LoadActive();
			return View("Carousel", data);
#else
			return Content("");
#endif
		}

#if CarouselBootstrap || SavvyCarousel || DumbCarousel
		public class SlideShowViewModel {
			public HomepageSlideList Slides;
		}
#endif

		public ActionResult NotFound() {
			Response.TrySkipIisCustomErrors = true;
			Response.StatusCode = 404;
			return Index();
		}
	}
}
