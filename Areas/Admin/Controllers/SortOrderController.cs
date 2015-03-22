using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms.VisualStyles;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class SortOrderController : AdminBaseController {
		
		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Arrange Sort Order");
			var data = new ViewModel();
			//data.Items = ProductList.LoadActive();
			return View("SortOrderView", data);
		}

		public class ViewModel : PageTemplateViewModel {
			public List<Item> Items;
		}

		public class Item {
			public int ID { get; set; }
			public string ImageThumbPath { get; set; }
		}

		/// <summary>
		/// Saves an existing record
		/// </summary>
		[HttpPost]
		public ActionResult Index(string sortOrder) {
			Web.InfoMessage = "Sort order saved.";
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update Product set SortPosition=",pos,"where ProductID=", id);
				sql.Execute();
				pos++;
			}
			// rebuild cache if necessary
			// go next page
			if (Web.Request["SaveAndRefreshButton"] != null) {
				return Index();
			} else {
				return Redirect(Web.AdminRoot);
			}
		}


		}
}