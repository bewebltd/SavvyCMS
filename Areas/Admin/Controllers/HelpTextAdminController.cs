// Admin HelpText Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class HelpTextAdminController : AdminBaseController {
		//
		// GET: /Admin/HelpText/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("HelpTextList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.HelpText> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion

			public ListViewModel() {
				Title = "Help Text List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.HelpText().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM HelpText where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.HelpText().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/HelpText/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Help Text");
			var record = new Models.HelpText();
			// any default values can be set here or in partial class HelpText.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.HelpText = record;
			return View("HelpTextEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/HelpText/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.HelpText());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/HelpText/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Help Text");
			var record = Models.HelpText.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Help Text not found (ID: "+id+")";
				return Redirect("~/Admin/HelpTextAdmin");
			}
			CheckLock(record);
			data.HelpText = record;
			return View("HelpTextEdit", data);
		}

		public class EditViewModel {
			public Models.HelpText HelpText;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/HelpText/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Help Text");
			var record = Models.HelpText.LoadID(id);
			return View("HelpTextView", record);
		}
		
		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/HelpText/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.HelpText.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("HelpTextExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/HelpText/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.HelpText.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.HelpText record) {
			try {
				if (record == null) record = new Models.HelpText();; //check for deleted, or null, create a new one
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Help Text "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("HelpTextEdit", new EditViewModel(){HelpText = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.HelpText();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Help Text "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.HelpText record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.HelpText record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			CheckLock(record);
			//BewebCore.ThirdParty.SearchTextExtractor.CheckAttachmentsForDocOrPDFText(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("HelpText");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.HelpText.LoadID(id);
			CheckLock(record);          // is this right? do we check lock on cancel?
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/HelpText/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.HelpText.LoadID(id);
			// first delete any child records that are OK to delete
			//ifsubform: record.example.DeleteAll();
			// then prevent deletion if any other related records exist
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues;
				return RedirectToEdit(record.ID);
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			//ifsubform: record.example.Save();  // is this needed?
			record.Delete();
			ActiveRecordLoader.ClearCache("HelpText");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update HelpText set SortPosition=",pos*10,"where HelpTextID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("HelpText");
			return Content("Sort order saved.");
		}
	}
}
