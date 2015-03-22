// Admin ClientContactUsRegion Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC.Helpers;

namespace Site.Areas.Admin.Controllers
{
	public class ClientContactUsRegionAdminController : AdminBaseController {
		//
		// GET: /Admin/ClientContactUsRegion/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Client Contact Us Region List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ClientContactUsRegionList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ClientContactUsRegion> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ClientContactUsRegion where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.ClientContactUsRegion().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ClientContactUsRegion().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ClientContactUsRegion/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Client Contact Us Region");
			var record = new Models.ClientContactUsRegion();
			// any default values can be set here or in partial class ClientContactUsRegion.InitDefaults() 
			return View("ClientContactUsRegionEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ClientContactUsRegion/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ClientContactUsRegion());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ClientContactUsRegion/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Client Contact Us Region");
			var record = Models.ClientContactUsRegion.LoadID(id);
			return View("ClientContactUsRegionEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/ClientContactUsRegion/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Client Contact Us Region");
			var record = Models.ClientContactUsRegion.LoadID(id);
			return View("ClientContactUsRegionView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ClientContactUsRegion/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ClientContactUsRegion.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ClientContactUsRegion record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (BewebModelState.IsValid()) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Client Contact Us Region "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				BewebModelState.AddModelError("Record", e.Message);
			}
			
			if (!BewebModelState.IsValid()) {
				// invalid so redisplay form with validation message(s)
				return View("ClientContactUsRegionEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ClientContactUsRegion();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Client Contact Us Region "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ClientContactUsRegion record) {
			// add any code to check for validity
			//BewebModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ClientContactUsRegion record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ClientContactUsRegion/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ClientContactUsRegion.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("ClientContactUsRegionEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
