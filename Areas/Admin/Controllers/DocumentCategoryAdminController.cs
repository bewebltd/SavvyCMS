// Admin DocumentCategory Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class DocumentCategoryAdminController : AdminBaseController {
		//
		// GET: /Admin/DocumentCategory/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			dataList.DocumentCategoryHierarchy = DocumentCategory.GetDocumentCategoryHierarchy();//this is only called by the admin

			return View("DocumentCategoryList", dataList);

		}

		public class ListViewModel : SavvyDataList<Models.DocumentCategory> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion
			public DocumentCategoryList DocumentCategoryHierarchy;

			public ListViewModel() {
				Title = "Document Category List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.DocumentCategory().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM DocumentCategory where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.DocumentCategory().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/DocumentCategory/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Document Category");
			var record = new Models.DocumentCategory();
			// any default values can be set here or in partial class DocumentCategory.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.DocumentCategory = record;
			return View("DocumentCategoryEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/DocumentCategory/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.DocumentCategory());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/DocumentCategory/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Document Category");
			var record = Models.DocumentCategory.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Document Category not found (ID: "+id+")";
				return Redirect("~/Admin/DocumentCategoryAdmin");
			}
			//CheckLock(record);
			data.DocumentCategory = record;
			return View("DocumentCategoryEdit", data);
		}

		public class EditViewModel {
			public Models.DocumentCategory DocumentCategory;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/DocumentCategory/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Document Category");
			var record = Models.DocumentCategory.LoadID(id);
			return View("DocumentCategoryView", record);
		}
		
		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/DocumentCategory/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.DocumentCategory.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("DocumentCategoryExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/DocumentCategory/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.DocumentCategory.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.DocumentCategory record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Document Category "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("DocumentCategoryEdit", new EditViewModel(){DocumentCategory = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.DocumentCategory();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Document Category "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.DocumentCategory record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.DocumentCategory record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			if (record.ParentDocumentCategoryID.HasValue && record.PageID.HasValue) {
				record.PageID = null;
				record.Save();
			}
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("DocumentCategory");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.DocumentCategory.LoadID(id);
			//CheckLock(record);          // is this right? do we check lock on cancel?
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/DocumentCategory/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.DocumentCategory.LoadID(id);
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
			ActiveRecordLoader.ClearCache("DocumentCategory");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update DocumentCategory set SortPosition=",pos*10,"where DocumentCategoryID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("DocumentCategory");
			return Content("Sort order saved.");
		}

	}
}
