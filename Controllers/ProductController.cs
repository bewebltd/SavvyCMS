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
	public class ProductController : ApplicationController {

		// GET: /Home
		//param int tcid: top category id
		//param int cid:  category id
		public ActionResult Index(int? tcid, int? cid) {

			var data = new ProductListViewModel();
			data.HasValidProducts = true;
			if (cid == null) {
				data.HasValidProducts = false;
				return View("Product", data);
			}

#if pages
			data.ContentPage = Models.Page.LoadOrCreatePageCode("Product");
			if (data.ContentPage == null) throw new Exception("Home page not found");
#endif

			Sql sql = new Sql("select * from product where categoryid = ", cid.Value, " and Isactive =1");

			List<Product> products = sql.LoadPooList<Product>();
			//data.Category = ProductCategory.LoadID(cid.Value);
			data.CatId = tcid.Value;
			if (products.Count < 1) {
				data.HasValidProducts = false;
				return View("Product", data);
			}
			data.Products = products;
			//data.CategoryMenu = ProductCategory.GetMenu(tcid);


			return View("Product", data);
		}


		[HttpGet]
		public ActionResult ProductDetail(int? tcid, int? pid, int? cid) {

			var data = new ProductViewModel();
			data.HasValidProduct = true;
			if (pid == null) {
				data.HasValidProduct = false;
				return View("ProductDetail", data);
			}

#if pages
			data.ContentPage = Models.Page.LoadOrCreatePageCode("Product");
			if (data.ContentPage == null) throw new Exception("Home page not found");
#endif

			Product p = Product.LoadByProductID(pid.Value);


			if (p == null) {
				data.HasValidProduct = false;
				return View("ProductDetail", data);
			}

			data.Product = p;
			data.ProdId = p.ProductID;
			//data.CategoryMenu = ProductCategory.GetMenu(tcid);
			data.CatId = tcid.Value;
			return View("ProductDetail", data);
		}

		public class ProductViewCommon : PageTemplateViewModel {
			public int ProdId;
			public int CatId;
			public Dictionary<ProductCategory, List<ProductCategory>> CategoryMenu;
		}

		public class ProductListViewModel : ProductViewCommon {

			public List<Product> Products;
			public bool HasValidProducts;
			public ProductCategory Category;
		}

		public class ProductViewModel : ProductViewCommon {

			public bool HasValidProduct;
			public Product Product;
			public ProductCategory Category;

		}


		public ActionResult NotFound() {
			Response.StatusCode = 404;
			return Index(1, 1);
		}
	}
}
