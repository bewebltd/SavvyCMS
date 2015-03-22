// Admin ShoppingCartOrder Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class ShoppingCartOrderAdminController : AdminBaseController {
		//
		// GET: /Admin/ShoppingCartOrder/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Shopping Cart Order List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ShoppingCartOrderList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ShoppingCartOrder> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ShoppingCartOrder where 1=1");
				if (SearchText != "") {
					sql.Add(" and (");
					sql.Add(" (CustomerOrderReference like ", SearchText.SqlizeLike(), ")");
					sql.Add(" or (OrderRef like ", SearchText.SqlizeLike(), ")");
					sql.Add(" or (Email like ", SearchText.SqlizeLike(), ")");
					sql.Add(" or (FirstName like ", SearchText.SqlizeLike(), ")");
					sql.Add(" or (LastName like ", SearchText.SqlizeLike(), ")");
					try
					{
						var searchDate = System.DateTime.Parse(SearchText);
						sql.Add(" or (DateOrdered between ", searchDate.SqlizeDate(), " and ", searchDate.AddDays(1).SqlizeDate(), " )");

					}
					catch (Exception)
					{
					
					}
					sql.Add(" )");

					//sql.AddKeywordSearch(SearchText, new Models.ShoppingCartOrder().GetNameField().Name, true);
				}
				//Logging.dout(sql.ToString());
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ShoppingCartOrder().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ShoppingCartOrder/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Shopping Cart Order");
			var record = new Models.ShoppingCartOrder();
			// any default values can be set here or in partial class ShoppingCartOrder.InitDefaults() 
			return View("ShoppingCartOrderEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ShoppingCartOrder/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ShoppingCartOrder());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ShoppingCartOrder/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Shopping Cart Order");
			var record = Models.ShoppingCartOrder.LoadID(id);
			return View("ShoppingCartOrderEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/ShoppingCartOrder/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Shopping Cart Order");
			var record = Models.ShoppingCartOrder.LoadID(id);
			return View("ShoppingCartOrderView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ShoppingCartOrder/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ShoppingCartOrder.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ShoppingCartOrder record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Shopping Cart Order "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ShoppingCartOrderEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ShoppingCartOrder();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Shopping Cart Order "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ShoppingCartOrder record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ShoppingCartOrder record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ShoppingCartOrder/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ShoppingCartOrder.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("ShoppingCartOrderEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
