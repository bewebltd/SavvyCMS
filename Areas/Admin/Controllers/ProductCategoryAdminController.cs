// Admin Category Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class ProductCategoryAdminController : AdminBaseController {
		//
		// GET: /Admin/Category/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Category List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ProductCategoryList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ProductCategory> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ProductCategory where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.ProductCategory().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ProductCategory().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Category/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Category");
			var record = new Models.ProductCategory();
			// any default values can be set here or in partial class Category.InitDefaults() 
			return View("ProductCategoryEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Category/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ProductCategory());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Category/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Category");
			var record = Models.ProductCategory.LoadID(id);
			return View("ProductCategoryEdit", record);
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Category/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Category");
			var record = Models.ProductCategory.LoadID(id);
			return View("ProductCategoryView", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Category/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ProductCategory.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ProductCategory record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Category "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ProductCategoryEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ProductCategory();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Category "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ProductCategory record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ProductCategory record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Category/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ProductCategory.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				ModelState.AddModelError("Record", "Cannot delete this  record. " + issues);
				return View("ProductCategoryEdit", record);
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
