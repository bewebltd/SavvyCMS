//#define pages
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class ProductsController : ApplicationController {

		// GET: /Home
		public ActionResult Index() {
			var data = new ProductListViewModel();
			//data.ContentPage = new Page();
			//if (data.ContentPage == null) throw new Exception("Product page not found");
			data.Categories = ProductCategoryList.LoadActive();					
			if(data.Categories.Count==0){
				//load some temp data
				data.Categories.Add(new ProductCategory(){Title="test11"});
				data.Categories[0].Products.Add(new Product(){Title = "prod1"});
				data.Categories[0].Products.Add(new Product(){Title = "prod2"});
				data.Categories.Add(new ProductCategory(){Title="test12"});
				data.Categories[1].Products.Add(new Product(){Title = "prod11"});
				data.Categories[1].Products.Add(new Product(){Title = "prod12"});
				data.Categories[1].Products.Add(new Product(){Title = "prod13"});
				data.Categories.Add(new ProductCategory(){Title="test13"});
			}
			
			return View("Products", data);
		}

		public ActionResult Detail(int id) {
			var data = new ProductListViewModel();
			//if(id==0)

			//data.ContentPage = Models.Page.LoadID(id, Otherwise.NotFound);
			//if (data.ContentPage == null) throw new Exception("Product page not found");
			data.Categories = ProductCategoryList.LoadByPageID(id).Active;
			
			return View("Products", data);
		}

		public class ProductListViewModel : PageTemplateViewModel {
			public ActiveRecordList<ProductCategory> Categories;
		}
	}
}
