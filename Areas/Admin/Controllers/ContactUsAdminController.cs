// Admin ContactUs Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC.Helpers;

namespace Site.Areas.Admin.Controllers {
	public class ContactUsAdminController : AdminBaseController {
		//
		// GET: /Admin/ContactUs/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Contact Us List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ContactUsList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ContactUs> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ContactUs where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					//sql.AddKeywordSearch(SearchText, new Models.ContactUs().GetNameField().Name, true);

					sql.Add(" and FirstName like ", SearchText.SqlizeLike());
					sql.Add(" or LastName like ", SearchText.SqlizeLike());
					sql.Add(" or Email like ", SearchText.SqlizeLike());
					sql.Add(" or Company like ", SearchText.SqlizeLike());
					sql.Add(" or Comments like ", SearchText.SqlizeLike());
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ContactUs().GetDefaultOrderBy());
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ContactUs/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Contact Us");
			var record = new Models.ContactUs();
			// any default values can be set here or in partial class ContactUs.InitDefaults() 
			return View("ContactUsEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ContactUs/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ContactUs());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ContactUs/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Contact Us");
			var record = Models.ContactUs.LoadID(id);
			return View("ContactUsEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/ContactUs/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Contact Us");
			var record = Models.ContactUs.LoadID(id);
			return View("ContactUsView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ContactUs/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ContactUs.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ContactUs record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (BewebModelState.IsValid()) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Contact Us " + record.GetName() + " saved.";
				}
			} catch (UserErrorException e) {
				BewebModelState.AddModelError("Record", e.Message);
			}

			if (!BewebModelState.IsValid()) {
				// invalid so redisplay form with validation message(s)
				return View("ContactUsEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ContactUs();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Contact Us " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ContactUs record) {
			// add any code to check for validity
			//BewebModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ContactUs record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ContactUs/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ContactUs.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("ContactUsEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
