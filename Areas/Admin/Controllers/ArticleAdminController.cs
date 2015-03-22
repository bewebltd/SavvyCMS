// Admin Article Controller
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class ArticleAdminController : AdminBaseController {
		//
		// GET: /Admin/Article/

		public ActionResult Index() {
			var dataList = new ListViewModel();
			Breadcrumbs.Current.AddBreadcrumb(dataList.BreadcrumbLevel, dataList.Title);
			Util.SetReturnPage(dataList.BreadcrumbLevel);
			dataList.PageLoad();
			return View("ArticleList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.Article> {

			public int? PageID = Web.Request["PageID"].ToInt(null);
			public string SectionTitle = "All Pages";

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
				Title = "Article List";
				GetFiltersFromQueryString();
				DefaultSortBy = new Models.Article().GetDefaultOrderBy();       // hint: to change the default order by for both admin and front end, override GetDefaultOrderBy in model 
			}

			public Page pageRecord {
				get {
					return Models.Page.LoadID(Web.Request["PageID"].ToInt(0));
				}
			}
			public override Sql GetSql() {

				/*
				Sql sql = new Sql("SELECT * FROM Article where 1=1");
				if (SearchText != "") {
					sql.AddKeywordSearch(SearchText, new Models.Article().GetTextFieldNames(), true);  // search on all text fields - change this if slow
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
				*/
				Sql sql = new Sql("SELECT a.*, p.Title as ParentTitle FROM Article a left join Page p on a.pageid=p.pageid where 1=1");
				sql.ResultSetPagingType = Sql.PagingType.sql2000; //!askMike do I have to use this now? :)

				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, "a.Title, p.Title, a.BodyTextHtml", true);
				}
				// handle an fk (rename fkid, then uncomment)
				if (PageID != null) {
					sql.Add("and a.PageID=", PageID.Value);
				}
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (PageID != null && SortBy.IsBlank()) {
					if (pageRecord != null) {
						if (pageRecord.DisplayOrder == "Most Recent") {
							sql.AddRawSqlString("order by PublishDate desc, ArticleID desc");
						} else if (pageRecord.DisplayOrder == "Sort Order") {
							sql.AddRawSqlString("order by SortPosition asc");
						}
					}
				} else if (SortBy.IsBlank()) {
					sql.AddRawSqlString("order by SortPosition, PublishDate");
				} else {
					sql.AddSql(GetOrderBySql());
					sql.AddRawSqlString(", SortPosition, PublishDate");
				}
				return sql;
			}

		}


		/// <summary>                     
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Article/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Add Article");
			var record = new Models.Article();
			record.PageID = Request["PageID"].ToInt(null);
			// any default values can be set here or in partial class Article.InitDefaults() 
			record.UpdateFromRequest();  // grab any defaults from querystring
			record.Author = UserSession.Person.FullName;
			data.Article = record;
			return View("ArticleEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Article/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Article());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Article/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(data.breadcrumbLevel, "Edit Article");
			var record = Models.Article.LoadID(id);
			if (record == null) {
				Web.ErrorMessage = "Article not found (ID: " + id + ")";
				return Redirect("~/Admin/ArticleAdmin");
			}
			//CheckLock(record);
			data.Article = record;
			return View("ArticleEdit", data);
		}

		public class EditViewModel {
			public Models.Article Article;
			public int breadcrumbLevel = Web.Request["bread"].ToInt(3);
		}

		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Article/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Article");
			var record = Models.Article.LoadID(id);
			return View("ArticleView", record);
		}

		/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/Article/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.Article.LoadID(id);
			Web.SetHeadersForExcel("detail " + record.GetName() + ".xls");
			return View("ArticleExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Article/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Article.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Article record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				record.ArticleDocuments.UpdateFromRequestSubForm();
				record.ArticleURLs.UpdateFromRequestSubForm();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					record.ArticleDocuments.Save();
					record.ArticleURLs.Save();
					Web.InfoMessage += "Article " + record.GetName() + " saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ArticleEdit", new EditViewModel() { Article = record });
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Article();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Article " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Article record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Article record, bool isNew) {
			// add any code to update other fields/tables here
			record.DateAdded = DateTime.Now;
			record.Save();
			// delete all keyword for this article and its related urls and documents.
			AutocompletePhrase.AddPhrase("Article", record.ID, record.MetaKeywords, true);
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			foreach (var articleDocument in record.ArticleDocuments) {
				BewebCore.ThirdParty.SearchTextExtractor.CheckAttachmentsForDocOrPDFText(articleDocument);		
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			ArticleCache.Rebuild();
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.Article.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Article/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Article.LoadID(id);
			// first delete any child records that are OK to delete
			//ifsubform: record.example.DeleteAll();
			record.ArticleURLs.DeleteAll(false);
			record.ArticleDocuments.DeleteAll(false);
			// then prevent deletion if any other related records exist
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues;
				return RedirectToEdit(record.ID);
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			//ifsubform: record.example.Save();  // is this needed?
			// delete the keywords DeletePhrase(string tableName, int recordID) {
			AutocompletePhrase.DeletePhrase("Article", record.ID);
			// do the same for the document titles
			foreach (var document in record.ArticleDocuments) {
				AutocompletePhrase.DeletePhrase("ArticleDocument", document.ID);
			}
			// do the same for the url titles
			foreach (var url in record.ArticleURLs) {
				AutocompletePhrase.DeletePhrase("ArticleURL", url.ID);
			}
			record.ArticleURLs.Save();
			record.ArticleDocuments.Save();
			record.Delete();
			ArticleCache.Rebuild();
			Web.InfoMessage = "Record deleted.";
			return Redirect(returnPage);
		}

		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update Article set SortPosition=", pos * 10, "where ArticleID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("Article");
			return Content("Sort order saved.");
		}

	}
}
