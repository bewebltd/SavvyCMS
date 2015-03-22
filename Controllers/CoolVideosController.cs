using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;
using TextBlock = Beweb.TextBlock;

namespace Site.Controllers {
	public class CoolVideosController : ApplicationController {
		//
		// GET: /CoolVideos

		public ActionResult Index() {
			var data = new ViewModel();
			data.ContentPage = Models.Page.LoadOrCreatePageCode("CoolVideos");
			if (data.ContentPage==null) throw new Exception("Videos page not found");

			int itemsPerPage = 6;
			var sql = new Sql("SELECT * FROM Video WHERE Status = ","Approved".Sqlize_Text()," ORDER BY VideoPostedDate DESC");
			sql.Paging(itemsPerPage);
			data.LatestVideos = VideoList.Load(sql);
			int numVideos = sql.GetCountSql().FetchIntOrZero();
			int numPages = Html.CalcPageCount(numVideos, itemsPerPage);
			data.PageNum = Web.Request["PageNum"].ToInt(1);
			data.ShowNextPage = (numPages > data.PageNum);
			data.ShowPrevPage = (data.PageNum > 1);

			if (Request["video"].IsNotBlank()) {
				data.CurrentVideo = Video.Load(new Sql("SELECT * FROM Video WHERE VideoID=",Request["video"].SqlizeNumber()));
			} else {
				data.CurrentVideo = data.LatestVideos.First();
				//data.CurrentVideo = Video.Load(new Sql("SELECT TOP 1 * FROM Video WHERE Status = ","Approved".Sqlize_Text()," ORDER BY VideoPostedDate DESC"));
			}

			if (data.CurrentVideo!=null) {
				var title = "Cool Videos - ";
				//if(data.CurrentVideo.BikeModel!=null) {
				//	title += data.CurrentVideo.BikeModel.Title + " - ";
				//}
				title += data.CurrentVideo.Title;
				data.Title = title;
			}

			return View("CoolVideos", data);
		}

		public class ViewModel : PageTemplateViewModel {
			public Video CurrentVideo;
			public VideoList LatestVideos;
			public int PageNum;
			public string Title;
			public bool ShowNextPage;
			public bool ShowPrevPage;
		} 
	}
}
