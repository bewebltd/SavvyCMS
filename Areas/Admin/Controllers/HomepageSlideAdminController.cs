// Admin HomepageSlide Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class HomepageSlideAdminController : AdminBaseController {
		//
		// GET: /Admin/HomepageSlide/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Homepage Slide List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("HomepageSlideList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.HomepageSlide> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM HomepageSlide where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					sql.AddKeywordSearch(SearchText, "Title,SlidePicture", true);  // search more than one field
					//sql.AddKeywordSearch(SearchText, new Models.HomepageSlide().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.HomepageSlide().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/HomepageSlide/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Homepage Slide");
			var record = new Models.HomepageSlide();
			// any default values can be set here or in partial class HomepageSlide.InitDefaults() 
			return View("HomepageSlideEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/HomepageSlide/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.HomepageSlide());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/HomepageSlide/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Homepage Slide");
			var record = Models.HomepageSlide.LoadID(id);
			return View("HomepageSlideEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/HomepageSlide/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Homepage Slide");
			var record = Models.HomepageSlide.LoadID(id);
			return View("HomepageSlideView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/HomepageSlide/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.HomepageSlide.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.HomepageSlide record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Homepage Slide "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("HomepageSlideEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.HomepageSlide();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Homepage Slide "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.HomepageSlide record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.HomepageSlide record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/HomepageSlide/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.HomepageSlide.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("HomepageSlideEdit", record);
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update HomepageSlide set SortPosition=",pos*10,"where HomepageSlideID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("HomepageSlide");
			return Content("Sort order saved.");
		}

	}
}
