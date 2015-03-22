// Admin DocumentDownload Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class DocumentDownloadAdminController : AdminBaseController {
		//
		// GET: /Admin/DocumentDownload/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("DocumentDownloadList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.DocumentDownload> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion

			public ListViewModel() {
				Title = "Document Download List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.DocumentDownload().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM DocumentDownload where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.DocumentDownload().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/DocumentDownload/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Document Download");
			var record = new Models.DocumentDownload();
			// any default values can be set here or in partial class DocumentDownload.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.DocumentDownload = record;
			return View("DocumentDownloadEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/DocumentDownload/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.DocumentDownload());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/DocumentDownload/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Document Download");
			var record = Models.DocumentDownload.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Document Download not found (ID: "+id+")";
				return Redirect("~/Admin/DocumentDownloadAdmin");
			}
			//CheckLock(record);
			data.DocumentDownload = record;
			return View("DocumentDownloadEdit", data);
		}

		public class EditViewModel {
			public Models.DocumentDownload DocumentDownload;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/DocumentDownload/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Document Download");
			var record = Models.DocumentDownload.LoadID(id);
			return View("DocumentDownloadView", record);
		}
		
		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/DocumentDownload/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.DocumentDownload.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("DocumentDownloadExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/DocumentDownload/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.DocumentDownload.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.DocumentDownload record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Document Download "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("DocumentDownloadEdit", new EditViewModel(){DocumentDownload = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.DocumentDownload();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Document Download "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.DocumentDownload record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.DocumentDownload record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("DocumentDownload");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.DocumentDownload.LoadID(id);
			//CheckLock(record);          // is this right? do we check lock on cancel?
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/DocumentDownload/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.DocumentDownload.LoadID(id);
			// first delete any child records that are OK to delete
			//ifsubform: record.example.DeleteAll();
			// then prevent deletion if any other related records exist
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues;
				return RedirectToEdit(record.ID);
			}
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			//ifsubform: record.example.Save();  // is this needed?
			record.Delete();
			ActiveRecordLoader.ClearCache("DocumentDownload");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update DocumentDownload set SortPosition=",pos*10,"where DocumentDownloadID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("DocumentDownload");
			return Content("Sort order saved.");
		}

	}
}
