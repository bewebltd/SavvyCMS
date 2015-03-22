// Admin GalleryImage Controller
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
	public class GalleryImageAdminController : AdminBaseController {
		//
		// GET: /Admin/GalleryImage/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("GalleryImageList", dataList);
		}


		public ActionResult View() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("GalleryImageMasonry", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.GalleryImage> {
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
				Title = "Gallery Image List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.GalleryImage().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}

			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM GalleryImage where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.GalleryImage().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
		/// GET: /Admin/GalleryImage/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Add Gallery Image");
			var record = new Models.GalleryImage();
			// any default values can be set here or in partial class GalleryImage.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			data.GalleryImage = record;
			return View("GalleryImageEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/GalleryImage/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.GalleryImage());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/GalleryImage/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Edit Gallery Image");
			var record = Models.GalleryImage.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Gallery Image not found (ID: "+id+")";
				return Redirect("~/Admin/GalleryImageAdmin");
			}
			//CheckLock(record);
			data.GalleryImage = record;
			return View("GalleryImageEdit", data);
		}

		public class EditViewModel {
			public Models.GalleryImage GalleryImage;
			public int breadcrumbLevel = Web.Request["bread"].ToInt(3);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/GalleryImage/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.GalleryImage.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.GalleryImage record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Gallery Image "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("GalleryImageEdit", new EditViewModel(){GalleryImage = record});
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.GalleryImage();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Gallery Image "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.GalleryImage record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.GalleryImage record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			//CheckLock(record);
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			ActiveRecordLoader.ClearCache("GalleryImage");
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.GalleryImage.LoadID(id);
			//CheckLock(record);          // is this right? do we check lock on cancel?
			//lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/GalleryImage/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.GalleryImage.LoadID(id);
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
			ActiveRecordLoader.ClearCache("GalleryImage");
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update GalleryImage set SortPosition=",pos*10,"where GalleryImageID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("GalleryImage");
			return Content("Sort order saved.");
		}

	}
}
