// Admin Document Controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class DocumentAdminController : AdminBaseController {
		//
		// GET: /Admin/Document/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("DocumentList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.Document> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion

			private string queryString;

			public string QueryString {
				get {
					if (queryString == null) {
						queryString = "";
						if (Web.QueryString.IsNotBlank()) {
							foreach (var key in Web.Request.QueryString.AllKeys) {
								var keyValue = (key != "bread") ? Web.Request.QueryString[key] : (Web.Request.QueryString[key].ToInt(0) + 1).ToString();
								queryString += (key == Web.Request.QueryString.AllKeys.First()) ? "?" : "&";
								queryString += key + "=" + keyValue;
							}
						}
					}
					return queryString;
				}

			}
			public ListViewModel() {
				Title = "Document List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.Document().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Document where 1=1");
				if (Web.Request["DocumentCategoryID"] + "" != "") {
					sql.Add(" and DocumentCategoryID=", Web.Request["DocumentCategoryID"].ToInt().SqlizeNumber());
				}
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.Document().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/Document/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Add Document");
			var record = new Models.Document();
			// any default values can be set here or in partial class Document.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.Document = record;
			return View("DocumentEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Document/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Document());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Document/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Edit Document");
			var record = Models.Document.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Document not found (ID: "+id+")";
				return Redirect("~/Admin/DocumentAdmin");
			}
			//CheckLock(record);
			data.Document = record;
			return View("DocumentEdit", data);
		}

		public class EditViewModel {
			public Models.Document Document;
			public int breadcrumbLevel = Web.Request["bread"].ToInt(3);
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Document/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Document");
			var record = Models.Document.LoadID(id);
			return View("DocumentView", record);
		}
		
		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/Document/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.Document.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("DocumentExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Document/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Document.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Document record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Document "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("DocumentEdit", new EditViewModel(){Document = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Document();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Document "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Document record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Document record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("Document");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.Document.LoadID(id);
			//CheckLock(record);          // is this right? do we check lock on cancel?
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Document/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Document.LoadID(id);
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
			ActiveRecordLoader.ClearCache("Document");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update Document set SortPosition=",pos*10,"where DocumentID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("Document");
			return Content("Sort order saved.");
		}

	}
}
