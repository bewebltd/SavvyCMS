// Admin SavvyAdmin Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class SavvyAdminController : AdminBaseController {
		//
		// GET: /Admin/SavvyAdmin/

		//public ActionResult Index() {
		//  Breadcrumbs.Current.AddBreadcrumb(2, "Savvy Admin List");
		//  Util.SetReturnPage(2);
		//  var dataList = new ListHelper();
		//  dataList.PageLoad();
		//  return View(dataList);
		//}

		public class ListHelper : SavvyDataList<Models.SavvyAdmin> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM SavvyAdmin where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.SavvyAdmin().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.SavvyAdmin().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/SavvyAdmin/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Savvy Admin");
			var record = new Models.SavvyAdmin();
			// any default values can be set here or in partial class SavvyAdmin.InitDefaults() 
			return View("SavvyAdminEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/SavvyAdmin/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.SavvyAdmin());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/SavvyAdmin/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Savvy Admin");
			var record = Models.SavvyAdmin.LoadID(id);
			return View("SavvyAdminEdit", record);
		}
		///// <summary>
		///// Loads existing record and shows view form, with cancel button
		///// GET: /Admin/SavvyAdmin/View/5
		///// </summary>
		//public ActionResult View(int id) {
		//  Breadcrumbs.Current.AddBreadcrumb(3, "View Savvy Admin");
		//  var record = Models.SavvyAdmin.LoadID(id);
		//  return View("View", record);
		//}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/SavvyAdmin/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.SavvyAdmin.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.SavvyAdmin record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Savvy Admin "+record.GetName()+" saved.";
				}
			} catch (Exception e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("SavvyAdminEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.SavvyAdmin();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Savvy Admin "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.SavvyAdmin record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.SavvyAdmin record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/SavvyAdmin/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.SavvyAdmin.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("SavvyAdminEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
