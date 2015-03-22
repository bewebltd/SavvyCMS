// Admin ShoppingCart Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class ShoppingCartAdminController : AdminBaseController {
		//
		// GET: /Admin/ShoppingCart/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Shopping Cart List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ShoppingCartList",dataList);
		}

		public class ListHelper : SavvyDataList<Models.ShoppingCart> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ShoppingCart where 1=1");
				if (SearchText != "") {
					sql.Add(" and (orderref like ", SearchText.SqlizeLike() ," or email like ", SearchText.SqlizeLike() ,"or firstname like ", SearchText.SqlizeLike() ,"or lastname like ", SearchText.SqlizeLike() ,")");
					//sql.AddKeywordSearch(SearchText, new Models.ShoppingCart().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ShoppingCart().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ShoppingCart/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Shopping Cart");
			var record = new Models.ShoppingCart();
			// any default values can be set here or in partial class ShoppingCart.InitDefaults() 
			return View("ShoppingCartEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ShoppingCart/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ShoppingCart());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ShoppingCart/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Shopping Cart");
			var record = Models.ShoppingCart.LoadID(id);
			return View("ShoppingCartEdit", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ShoppingCart/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ShoppingCart.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ShoppingCart record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Shopping Cart "+record.GetName()+" saved.";
				}
			} catch (Exception e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ShoppingCartEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ShoppingCart();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Shopping Cart "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ShoppingCart record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ShoppingCart record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ShoppingCart/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ShoppingCart.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("ShoppingCartEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
