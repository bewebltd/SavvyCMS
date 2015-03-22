// Admin Company Controller
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.Controllers;
using Site.SiteCustom;

namespace Site.Controllers {
	public class NewsController : ApplicationController {
		//
		// GET: /Admin/Company/


		public class ViewModel : PageTemplateViewModel {
			public News news;
			public News nextNews;
			public News prevNews;
			public NewsList newsList;
			public string nextNewsTitle, prevNewsTitle;
		}

		public ActionResult NewsDetail(int id) {

			var view = new ViewModel();

			view.ContentPage = Models.Page.LoadPageContent("News");

			//get company based on logged in users id (from person -> company id)
			Person p = Person.LoadByPersonID(Security.LoggedInUserID);
			view.news = Models.News.LoadID(id);
			var sql = new Sql("SELECT TOP 4 * FROM News WHERE NewsID<>", id.SqlizeNumber(), "AND PublishDate IS NOT NULL AND GETDATE() > PublishDate AND (ExpiryDate IS NULL OR GETDATE() < ExpiryDate+1) ORDER BY PublishDate Desc");
			view.newsList = Models.NewsList.Load(sql);

			//var nextSql = new Sql("SELECT TOP 1 NewsID, MIN(PublishDate) AS pDate FROM News WHERE PublishDate > (SELECT PublishDate FROM News WHERE NewsID = ",id.SqlizeNumber(),") GROUP BY PublishDate, NewsID ORDER BY PublishDate Asc");
			//view.nextNews = Models.News.Load(nextSql);
			//var prevSql = new Sql("SELECT TOP 1 NewsID, MAX(PublishDate) AS pDate FROM News WHERE PublishDate < (SELECT PublishDate FROM News WHERE NewsID = ",id.SqlizeNumber(),") GROUP BY PublishDate, NewsID ORDER BY PublishDate Desc");
			//view.prevNews = Models.News.Load(prevSql);
			//if(view.nextNews!=null) view.nextNewsTitle = News.LoadID(view.nextNews.ID).IntroductionText;
			//if(view.prevNews!=null) view.prevNewsTitle = News.LoadID(view.prevNews.ID).IntroductionText;

			//get all news sql

			var allNews = GetAllNewsSql();
			//DataTable dt = allNews.GetDataTable();
			//for(int sc = 0;sc<dt.Rows.Count;sc++)
			//{
			//  var dr = dt.Rows[sc];
			//  string value = dr["Picture"];
			//}

			int previd = -1;
			int nextid = -1;
			int currentID = -1;

			var list = Models.NewsList.Load(allNews);
			foreach (var newsItem in list) {
				currentID = newsItem.ID;
				if (currentID == id) {
					if (list.LoopIndex < list.Count - 1) {
						News newsPrev = list[list.LoopIndex + 1];
						previd = newsPrev.ID;
						view.prevNews = Models.News.LoadID(previd);
					} else {
						//previd = newsPrev.ID;
					}

					if (list.LoopIndex > 0) {
						News newsNext = list[list.LoopIndex - 1];
						nextid = newsNext.ID;
						view.nextNews = Models.News.LoadID(nextid);
					}
				}

			}




			// record prev id
			// walk the list looking for the current one
			// record next id
			// if found curr id, exit loop


			return View("NewsDetail", view);
		}

		public ActionResult Index() {

			var view = new ViewModel();
			view.news = new News();
			var sql = GetAllNewsSql();
			view.ContentPage = Models.Page.LoadPageContent("News");

			view.newsList = Models.NewsList.Load(sql);
			return View("News", view);
		}

		private Sql GetAllNewsSql() {
			return new Sql("SELECT * FROM News WHERE 1=1 AND PublishDate IS NOT NULL AND GETDATE() > PublishDate AND (ExpiryDate IS NULL OR GETDATE() < ExpiryDate+1) ORDER BY PublishDate Desc");
		}


		public ActionResult GetNextNews(string nextRow) {
			Sql sql = null;
			if (nextRow.IsBlank()) {
				sql = new Sql("select top 3 * from news");
			} else {
				sql = new Sql("select top 1 * from news");
			}

			sql.Add(" where 1 = 1 ");
			int? id = nextRow.ToInt(null);

			if (id.HasValue) {
				sql.Add(" and newsID < ", id.Value.SqlizeNumber());
			}

			//
			sql.Add(" order by newsID desc");
			//sql.Add(" order by datedded desc");

			NewsList newsList = NewsList.Load(sql);

			List<JsonNews> jsonNews = new List<JsonNews>();
			foreach (var news in newsList) {
				JsonNews jnews = new JsonNews();
				jnews.NewsID = news.ID;
				jnews.Title = news.Title;
				jnews.Body = news.BodyTextHtml;
				jnews.Date = news.DateAdded.FmtDate();
				jsonNews.Add(jnews);
			}
			return Json(jsonNews, JsonRequestBehavior.AllowGet);
		}


		private class JsonNews {
			public int NewsID { get; set; }
			public string Title { get; set; }
			public string Date { get; set; }
			public string Body { get; set; }

		}




	}
}
