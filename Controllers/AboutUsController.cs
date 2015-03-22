using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Site.SiteCustom;

namespace Site.Controllers {
	public class AboutUsController : ApplicationController {
		//
		// GET: /AboutUs/


		public AboutUsController() {


		}

		public ActionResult SubNav(int showingCurrentPageID) {
			var data = new ViewModel();
			data.CurrentID = showingCurrentPageID;
			data.Init();
			return View("SubNav", data);
		}

		public ActionResult Index() {
			var data = new ViewModel();
			data.ContentPage = Models.Page.LoadPageContent("AboutUs");

			return View("AboutUs", data);
		}
		public ActionResult LoadSample() {
			for (var i = 1; i < Util.GetRandomInt(3,6); i++) {
				var sec = new Models.ClientContactUsRegion();
				sec.RegionName = "Sample region " + i;
				sec.IsPublished=true;
				sec.Save();
				for (var j = 1; j < Util.GetRandomInt(3,6); j++) {
					var qn = new Models.ClientContactUsPerson();
					qn.PersonName = "Sample person " + j;
					qn.ClientContactUsRegionID = sec.ID;
					qn.IsPublished=true;
					qn.EmailAddress="sample@sample.com";
					qn.Introduction="intro Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut";
					qn.JobDescription="job descr Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euist";

					qn.Save();

				}
			}
			return Index();
		}

		public class ViewModel : PageTemplateViewModel {
			public Models.PageList SubNavMenu;
			public int CurrentID;
			public Models.ClientContactUsPersonList PersonList = Models.ClientContactUsPersonList.LoadActive();

			public void Init() {
				//SubNavMenu = Savvy.Site.PrepNavList(Models.Page.LoadByPageCode("Info").ID, "", CurrentID);

			}



		}

	}
}
