using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using SavvyMVC;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class TextBlockController : Controller {

	[ActionOutputCache(60)]
	public ActionResult TextBlock(string sectionCode) {
			var data = Models.TextBlock.LoadBySectionCode(sectionCode);
			
			if(data == null) {
				// add it to the database
				var tb = new Models.TextBlock();
				tb.SectionCode = sectionCode;
				tb.IsTitleAvailable = false;
				tb.IsBodyPlainText = false;
				tb.IsUrlAvailable = false;
				tb.IsPictureAvailable = false;
				tb.Save();
				data = tb;
			}
			
			return View("Index",data);
		}
	}
}