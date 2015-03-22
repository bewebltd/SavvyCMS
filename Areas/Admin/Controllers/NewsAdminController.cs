// Admin News Controller
using System;
using System.Globalization;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC;

namespace Site.Areas.Admin.Controllers
{
	public class NewsAdminController : AdminBaseController {
		//
		// GET: /Admin/News/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "News List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("NewsList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.News> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM News where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.News().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.News().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/News/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add News");
			var record = new Models.News();
			// any default values can be set here or in partial class News.InitDefaults() 

			return View("NewsEdit", record);
		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/News/PreLoad
		/// </summary>
		public ActionResult PreLoad(FormCollection collection) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add News from RSS");
			var record = new Models.News();
			// any default values can be set here or in partial class News.InitDefaults() 

			string text = "";
			if ((text=Request["title"]).IsNotBlank()) {
				record.IntroductionText = Server.UrlDecode(text);
			}
			if ((text=Request["feedname"]).IsNotBlank()) {
				record.Source = Server.UrlDecode(text);
			}
			if ((text=Request["link"]).IsNotBlank()) {
				record.LinkUrl = Server.UrlDecode(text);
			}
			if ((text=Request["description"]).IsNotBlank()) {
				record.BodyTextHtml = Server.UrlDecode(text) ;
			}
			if ((text=Request["pubDate"]).IsNotBlank()) {
				var dateText = Server.UrlDecode(text); //		Server.UrlDecode(text)	"Wed, 17 Aug 2011 21:54:44 EDT"	string
				var simpleDate = Rfc822DateTime.Parse(dateText);
				record.PublishDate = simpleDate;
			}

			return View("NewsEdit", record);
		}

		
		/// <summary>
		/// Saves a new record
		/// POST: /Admin/News/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.News());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/News/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit News");
			var record = Models.News.LoadID(id);
			return View("NewsEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/News/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View News");
			var record = Models.News.LoadID(id);
			return View("NewsView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/News/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.News.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.News record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "News "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("NewsEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.News();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of News "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.News record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.News record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/News/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.News.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("NewsEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
