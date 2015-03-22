// Admin Product Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class ProductAdminController : AdminBaseController {
		//
		// GET: /Admin/Product/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Product List");
			Util.SetReturnPage(2);

			var viewModel = new ViewModel();
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ProductList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.Product> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Product where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.Product().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.Product().GetDefaultOrderBy());
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}

		public class ViewModel : PageTemplateViewModel {

			public ListHelper ListHelper;
			public int CatId;
			public string CatTitle;
			public Product Product;
		}

		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Product/Create
		/// </summary>
		public ActionResult Create(int? categoryid, string catTitle) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Product");
			var record = new Models.Product();
			if (categoryid.HasValue) {
				var category = new ProductCategory();
				category.ID = categoryid.Value;
				category.Title = catTitle;
				record.ProductCategory = category;
				record.ProductCategoryID = categoryid;

			}
			record.Action = "AddNew";
			// any default values can be set here or in partial class Product.InitDefaults() 
			return View("ProductEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Product/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Product());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Product/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Product");
			var record = Models.Product.LoadID(id);
			return View("ProductEdit", record);
		}

		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Product/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Product");
			var record = Models.Product.LoadID(id);
			return View("ProductView", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Product/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Product.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Product record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Product " + record.GetName() + " saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ProductEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Product();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Product " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Product record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Product record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Product/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Product.LoadID(id);
			/*	string issues = record.CheckForDependentRecords();
				if (issues.IsNotBlank()) {
					ModelState.AddModelError("Record", "Cannot delete this  record. " + issues);
					return View("ProductEdit", record);
				}*/
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update Product set SortPosition=", pos * 10, "where ProductID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("Product");
			return Content("Sort order saved.");
		}

	}
}
