// Admin FAQItem Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC.Helpers;

namespace Site.Areas.Admin.Controllers
{
	public class FAQItemAdminController : AdminBaseController {
		//
		// GET: /Admin/FAQItem/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(3, "FAQ Item List");
			Util.SetReturnPage((Request["FAQSectionID"].IsNotBlank())?3:2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("FAQItemList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.FAQItem> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM FAQItem where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					//sql.AddKeywordSearch(SearchText, new Models.FAQSection().GetNameField().Name, true);
				//	sql.AddKeywordSearch(SearchText, "SectionCode", true);
				}
				// handle an fk (rename fkid, then uncomment)
				if(Web.Request["FAQSectionID"].IsNotBlank())sql.Add("and FAQSectionID=", Web.Request["FAQSectionID"].SqlizeNumber());
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.FAQItem().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/FAQItem/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(4, "Add FAQ Item");
			var record = new Models.FAQItem();
			// any default values can be set here or in partial class FAQItem.InitDefaults() 
			return View("FAQItemEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/FAQItem/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.FAQItem());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/FAQItem/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(4, "Edit FAQ Item");
			var record = Models.FAQItem.LoadID(id);
			return View("FAQItemEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/FAQItem/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(4, "View FAQ Item");
			var record = Models.FAQItem.LoadID(id);
			return View("FAQItemView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/FAQItem/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.FAQItem.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.FAQItem record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (BewebModelState.IsValid()) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Faqitem "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				BewebModelState.AddModelError("Record", e.Message);
			}
			
			if (!BewebModelState.IsValid()) {
				// invalid so redisplay form with validation message(s)
				return View("FAQItemEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.FAQItem();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Faqitem "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.FAQItem record) {
			// add any code to check for validity
			//BewebModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.FAQItem record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/FAQItem/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.FAQItem.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("FAQItemEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}
		
		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update FaqItem set SortPosition=",pos*10,"where [---pkname---]=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("FaqItem");
			return Content("Sort order saved.");
		}

	}
}
