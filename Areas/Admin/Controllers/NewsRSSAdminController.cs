// Admin NewsRSS Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC;

namespace Site.Areas.Admin.Controllers
{
	public class NewsRSSAdminController : AdminBaseController {
		//
		// GET: /Admin/NewsRSS/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "News RSS List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("NewsRSSList", dataList);
		}

		public ActionResult FeedEditor() {
			Breadcrumbs.Current.AddBreadcrumb(2, "News RSS Results");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.ShowActiveOnly = true;
			dataList.PageLoad();
			return View("RSSFeedEditor", dataList);
		}

		public class ListHelper : SavvyDataList<Models.NewsRSS> {
			public ListHelper() {

			}

			public bool ShowActiveOnly = false;
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM NewsRSS where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.NewsRSS().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				if(ShowActiveOnly)sql.Add("and ispublished=",true);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.NewsRSS().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/NewsRSS/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add News Rss");
			var record = new Models.NewsRSS();
			// any default values can be set here or in partial class NewsRSS.InitDefaults() 
			return View("NewsRSSEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/NewsRSS/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.NewsRSS());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/NewsRSS/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit News Rss");
			var record = Models.NewsRSS.LoadID(id);
			return View("NewsRSSEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/NewsRSS/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View News Rss");
			var record = Models.NewsRSS.LoadID(id);
			return View("NewsRSSView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/NewsRSS/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.NewsRSS.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.NewsRSS record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "News Rss "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("NewsRSSEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.NewsRSS();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of News Rss "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.NewsRSS record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.NewsRSS record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/NewsRSS/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.NewsRSS.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("NewsRSSEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
