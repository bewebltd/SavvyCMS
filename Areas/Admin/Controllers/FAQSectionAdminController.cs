// Admin FAQSection Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class FAQSectionAdminController : AdminBaseController {
		//
		// GET: /Admin/FAQSection/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("FAQSectionList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.FAQSection> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion

			public ListViewModel() {
				Title = "FAQ Section List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.FAQSection().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM FAQSection where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.FAQSection().GetTextFieldNames(), true);  // search on all text fields - change this if slow
				}
				sql.AddSql(FilterSql);
				#region custom filter examples
				//if (StatusFilter.IsNotBlank()) {
				//	FilterSql.Add("and x < y");
				//}
				//FilterSql.Add("and x <= ", MinDateFilter);
				#endregion
				sql.AddSql(base.GetOrderBySql());
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/FAQSection/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add FAQ Section");
			var record = new Models.FAQSection();
			// any default values can be set here or in partial class FAQSection.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.FAQSection = record;
			return View("FAQSectionEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/FAQSection/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.FAQSection());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/FAQSection/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit FAQ Section");
			var record = Models.FAQSection.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Faqsection not found (ID: "+id+")";
				return Redirect("~/Admin/FAQSectionAdmin");
			}
			CheckLock(record);
			data.FAQSection = record;
			return View("FAQSectionEdit", data);
		}

		public class EditViewModel {
			public Models.FAQSection FAQSection;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/FAQSection/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View FAQ Section");
			var record = Models.FAQSection.LoadID(id);
			return View("FAQSectionView", record);
		}
		
		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/FAQSection/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.FAQSection.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("FAQSectionExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/FAQSection/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.FAQSection.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.FAQSection record) {
			try {
				if(record==null)record=new FAQSection(); //check for deleted, or null, create a new one
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				record.FAQItems.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "FAQ Section "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("FAQSectionEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.FAQSection();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of FAQ Section "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.FAQSection record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.FAQSection record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			record.FAQItems.Save();
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("FAQSection");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.FAQSection.LoadID(id);
			CheckLock(record);          // is this right? do we check lock on cancel?
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/FAQSection/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.FAQSection.LoadID(id);
			// first delete any child records that are OK to delete
			record.FAQItems.DeleteAll();
			// then prevent deletion if any other related records exist
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues;
				return RedirectToEdit(record.ID);
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			record.FAQItems.Save();  // is this needed?
			record.Delete();
			ActiveRecordLoader.ClearCache("FAQSection");
			Web.InfoMessage =  "FAQ Section Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update FAQSection set SortPosition=",pos*10,"where FAQSectionID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("FAQSection");
			return Content("Sort order saved.");
		}

	}
}
