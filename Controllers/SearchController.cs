using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class SearchController : ApplicationController {

		public ActionResult Search(string searchText) {

			var data = new ViewModel();
			data.ContentPage = Models.Page.LoadOrCreatePageCode("Search");
			if (data.ContentPage == null) throw new Exception("Search page not found");
			data.SearchText = searchText;
			data.SearchArea = String.IsNullOrEmpty(Web.Request["SearchArea"]) ? "All" : Web.Request["SearchArea"];
			int itemsPerPage;
			if (data.SearchArea == "All") {
				itemsPerPage = 5;
			} else {
				itemsPerPage = 25;
			}

			string keywords = searchText;
			var search = new KeywordSearch(keywords);
			//#if pages

			if (searchText.IsNotBlank()) {

				/*	if (data.SearchArea == "All" || data.SearchArea == "Pages") {
						Sql sql = new Sql("select * from Page");
						sql.AddSql(search.FullTextJoin("page"));
						sql.WhereIsActive<Page>();
						//sql.Add("and (rolesallowed is null)");   // if using page user access control
						sql.Add("order by Rank desc,Title");
						sql.Paging(itemsPerPage);
						data.PageResults = PageList.Load(sql);
						data.Counter += sql.FetchCount();
					}
					if (data.SearchArea == "All" || data.SearchArea == "Faqs") {
						Sql sql = new Sql("select * from FAQItem");
						sql.AddSql(search.FullTextJoin("FAQItem"));
						sql.WhereIsActive<FAQItem>();
						sql.Add("order by Rank,FAQTitle");
						sql.Paging(itemsPerPage);
						data.FaqItems = FAQItemList.Load(sql);
						data.Counter += sql.FetchCount();
					}*/
				//if (data.SearchArea == "All" || data.SearchArea == "News") {
				//	Sql sql = new Sql("select * from News");
				//	sql.AddSql(search.FullTextJoin("News"));
				//	//sql.WhereIsActive<News>();
				//	sql.Add("order by Rank,Source");
				//	sql.Paging(itemsPerPage);
				//	//data.NewsItems = NewsList.Load(sql);
				//	data.Counter += sql.FetchCount();
				//}
				//if (data.SearchArea == "All" || data.SearchArea == "Products") {
				//	Sql sql = new Sql("select * from Product");
				//	sql.AddSql(search.FullTextJoin("Product"));
				//	//sql.WhereIsActive<Product>();
				//	sql.Add("order by Rank desc,Title");
				//	sql.Paging(itemsPerPage);
				//	//data.ProductItems = ProductList.Load(sql);
				//	data.Counter += sql.FetchCount();
				//}
			}

			data.NumPages = Html.CalcPageCount(data.Counter, itemsPerPage);
			// note: for full text search, instead use 
			var keywordSearch = new Beweb.KeywordSearch(searchText);
			var columns = keywordSearch.GetSqlFullTextWhereForTable("Page");
			if (columns.IsNotBlank()) {
				var sql = new Sql("select * from Page where 1=1");
				sql.Add("and historypageid is null"); // if page revisions
				sql.AddRawSqlString(columns);
				sql.AndIsActive<Page>();
				data.PageResults = PageList.Load(sql);
			}

			columns = keywordSearch.GetSqlFullTextWhereForTable("Event");
			if (columns.IsNotBlank()) {
				var sql = new Sql("select * from event where 1=1");
				sql.AddRawSqlString(columns);
				sql.AndIsActive<Event>();
				data.EventResults = EventList.Load(sql);
			}

			columns = keywordSearch.GetSqlFullTextWhereForTable("News");
			if (columns.IsNotBlank()) {
				var sql = new Sql("select * from News where 1=1");
				sql.AddRawSqlString(keywordSearch.GetSqlFullTextWhereForTable("News"));
				sql.AndIsActive<News>();
				data.NewsResults = NewsList.Load(sql);
			}
			//#endif
			return View("SearchResults", data);
		}

		public ActionResult CreateFullTextIndex() {

			KeywordSearch.CreateFullTextIndex<Models.Page>();
			KeywordSearch.CreateFullTextIndex<Models.Event>();
			KeywordSearch.CreateFullTextIndex<Models.News>();

			return Content("Done");
		}

		public ActionResult SearchField() {
			var data = new ViewModel();
			if (Web.Request["searchText"].IsNotBlank()) {
				data.SearchText = Web.Request["searchText"];
			}
			return View(data);
		}

		private class PhraseList {
			public string Phrase { get; set; }
		}

		public ActionResult SearchAutoComplete(string term) {
			//#if AutocompletePhrase
			var sql = new Sql("Select distinct phrase from AutocompletePhrase where Phrase like", term.SqlizeLike());
			var result = sql.LoadPooList<PhraseList>();
			return Json(result, JsonRequestBehavior.AllowGet);
			//	#else
			//	return null;
			//	#endif
		}

		public class ViewModel : PageTemplateViewModel {
			public string SearchText;
			public string SearchArea;
			public int Counter = 0;
			public int NumPages = 1;
			public PageList PageResults = new PageList();
			public NewsList NewsResults = new NewsList();
			public EventList EventResults = new EventList();

			// delete this if not needed
			public bool ShowSidebar {
				get {
#if pages
					return ContentPage.SidebarTitle != null || ContentPage.SidebarPicture != null || ContentPage.SidebarTextHtml.TrimHtml().IsNotBlank();
#else
				return false;
#endif
				}
			}

			// delete this if not needed
			public string SidebarTitle {
				get {
#if pages
					return ContentPage.SidebarTitle ?? "Related Story";
#else
				return null;
#endif
				}
			}
		}


	}
}
