//#define pages
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class CategoryController : ApplicationController {



	/*		public ActionResult Index(int? tcid, int? cid) {

			var data = new ProductListViewModel();
			data.HasValidProducts = true;
			if (cid == null) {
				data.HasValidProducts = false;
				return View("Product", data);
			}
*/

		// GET: /Home
		public ActionResult Index(int? pid) {

			var data = new ViewModel();
			data.HasValidCategories = false;
			


			if (pid == null) {
			
					//get catergory that has most products
					//int catId = new Sql("select top 1 CategoryId from Product group by CategoryId order by count(*) desc").FetchInt().Value;

					pid = new Sql("select top 1 CategoryId from  category where ParentCatergoryId = 0 order by sortposition").FetchInt();
			}

			if (pid == null) {
				data.HasValidCategories = false;

			} else {
				data.catId = pid.Value;
				data.HasValidCategories = true;
/*			if (pid == 0) {
				pid = new Sql("select top 1 * from  category where ParentCatergoryId != 0").FetchInt();
			}*/
				/*		if (pid == null) {
				data.HasValidCategories = false;
				return View("Category", data);
				 * 
				 * 
				 * 
			}*/
			
			
			List<Category> subCategories = new Sql("select * from category where ParentCatergoryId = ", pid.Value).LoadPooList<Category>();
			Category c = Category.LoadByCategoryID(pid.Value);

			data.Category = c;
	
			data.CategoryMenu = Category.GetMenu(pid.Value);
			data.subCategories = subCategories;
			
			
			}
			//	TrackingBreadcrumb.Current.AddBreadcrumb(0, "Home");
#if pages
			data.ContentPage = Models.Page.LoadOrCreatePageCode("Category");
			if (data.ContentPage == null) throw new Exception("Category page not found");
#endif

			return View("Category", data);
		}


		public ActionResult RenderMenu(int? catid) {



			var data = new ViewModel();
			List<Category> subCategories = new Sql("select * from category where ParentCatergoryId = ", catid.Value).LoadPooList<Category>();
			Category c = Category.LoadByCategoryID(catid.Value);
			data.Category = c;
			data.CategoryMenu = Category.GetMenu(catid.Value);
			data.subCategories = subCategories;
			data.catId = c.CategoryID;
			return View("~/Views/Common/SideMenu.ascx", data);
		}

		public class ViewModel : PageTemplateViewModel {
			public int catId = 0;
			public bool HasValidCategories;
			public Category Category;
			public List<Category> subCategories = new List<Category>();
			public Dictionary<Category, List<Category>> CategoryMenu;
		}


	}
}
