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
	public class ErrorController : Controller {
		//
		// GET: /Error/

		public ActionResult ShowErrorPage(string errorPage) {
			var data = new ViewModel();
//#if pages
//      data.ContentPage = new Page();
//#endif
			Response.StatusCode = errorPage.ToInt(500);
			return View(errorPage.RemoveSuffix(".aspx"), data);
		}

		public ActionResult NotFound(string message) {
			// note: if you want to go to homepage instead of 404 page, simply change the Error/NotFound route in global.asax
			var data = new ViewModel();	
			data.Message = message;
			Response.StatusCode = 404;
			Response.Status = "404 Not Found";
			return View("404", data);
		}

		public class ViewModel : PageTemplateViewModel {
			public string Message = "";
		}

		public ActionResult SiteError() {
			var data = new ViewModel();
			data.Message = "The application developers have been notified and will correct the problem.";
			Response.StatusCode = 500;
			return View("500", data);
		}

		public ActionResult AdminError() {
			var data = new ViewModel();
			data.Message = "The site administrator has been notified and will correct the problem.";
			Response.StatusCode = 500;
			return View("500", data);
		}

		public ActionResult LogJavascriptError(string message, string url, string line, string browser, string pageUrl) {
			Error.NotifyJavascriptError(message, url, line, browser, pageUrl);
			return Content("Done");
		}
		
		public ActionResult FixError(string message, string title, string lineThatDied) {
			Beweb.Logging.FixError(message, title, lineThatDied);
			return Content("Done");
		}
	}
}
