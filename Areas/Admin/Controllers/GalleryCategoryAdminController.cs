// Admin GalleryCategory Controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class GalleryCategoryAdminController : AdminBaseController {
		//
		// GET: /Admin/GalleryCategory/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("GalleryCategoryList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.GalleryCategory> {
				#region custom filter examples
			//public string StatusFilter = Web.Request["StatusFilter"];         // example custom filter
			//public DateTime MinDateFilter = Web.Request["MinDateFilter"].ConvertToDate(Dates.GetPreviousMonthBegin(DateTime.Today));         // example custom filter
				#endregion

			public ListViewModel() {
				Title = "Gallery Category List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.GalleryCategory().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}

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

			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM GalleryCategory where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.GalleryCategory().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/GalleryCategory/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Add Gallery Category");
			var record = new Models.GalleryCategory();
			// any default values can be set here or in partial class GalleryCategory.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.GalleryCategory = record;
			return View("GalleryCategoryEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/GalleryCategory/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.GalleryCategory());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/GalleryCategory/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Edit Gallery Category");
			var record = Models.GalleryCategory.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Gallery Category not found (ID: "+id+")";
				return Redirect("~/Admin/GalleryCategoryAdmin");
			}
			//CheckLock(record);
			data.GalleryCategory = record;
			return View("GalleryCategoryEdit", data);
		}

		public class EditViewModel {
			public Models.GalleryCategory GalleryCategory;
			public int breadcrumbLevel = Web.Request["bread"].ToInt(3);
		}
	

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/GalleryCategory/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.GalleryCategory.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.GalleryCategory record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Gallery Category "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("GalleryCategoryEdit", new EditViewModel(){GalleryCategory = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.GalleryCategory();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Gallery Category "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.GalleryCategory record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.GalleryCategory record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("GalleryCategory");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.GalleryCategory.LoadID(id);
			//CheckLock(record);          // is this right? do we check lock on cancel?
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/GalleryCategory/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.GalleryCategory.LoadID(id);
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
			ActiveRecordLoader.ClearCache("GalleryCategory");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update GalleryCategory set SortPosition=",pos*10,"where GalleryCategoryID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("GalleryCategory");
			return Content("Sort order saved.");
		}

	}
}
