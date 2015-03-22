using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.GData.Extensions;
using Google.YouTube;
using Models;
using Site.SiteCustom;
using Video = Models.Video;

namespace Site.Controllers {
	public class YouTubeSpiderController : Controller {
		//
		// GET: /YouTubeSpider/

		//public ActionResult Index() {
		//  var data = new ViewModel();
		//  data.ContentPage = Models.Page.LoadByPageCode("Home");
		//  if (data.ContentPage == null) throw new Exception("Home page not found");

		//  GetYoutubeVideos();

		//  //data.VideoFeed = Models.VideoList.LoadActive();
		//  data.VideoFeed = Models.VideoList.Load(new Sql("SELECT TOP(100) * FROM Video WHERE Status=", "New".Sqlize_Text(), "ORDER BY VideoId ASC"));
		//  return Content("done");
		//  return View("Test", data);
		//}

		 private static DateTime nextSpiderCheck = DateTime.Now.AddHours(23);
		 public static DateTime NextSpiderCheck { get { return nextSpiderCheck; } }
		 public static DateTime? LastSpiderCheck { get; private set; }

		 public static void CheckSpiderTask() {
			if (DateTime.Now > nextSpiderCheck) {
				Util.HttpGetAsync(Web.BaseUrl + "YouTubeSpider/RunSpiderTask");
				nextSpiderCheck = DateTime.Today.AddDays(1);
			}
		}

		public ActionResult RunSpiderTask() {
			int numAdded = GetYoutubeVideos();
			if(numAdded > 0) {
			string body = "We have found another "+numAdded+" videos on Youtube to be approved<br/><br/><a href='"+Web.BaseUrl+"/admin/videoAdmin/approval'>Approve Videos</a>";
				//SendEMail.SimpleSendHtmlEmail(SendEMail.EmailToAddress, "Youtube Videos to approve ", body);
				SendEMail.SimpleSendHtmlEmail("monique@beweb.co.nz", "Youtube Videos to approve", body);
			}
			YouTubeSpiderController.LastSpiderCheck = DateTime.Now;
			return Content("done");
		}

		public class ViewModel : PageTemplateViewModel {
			public VideoList VideoFeed;
		}

		private int GetYoutubeVideos() {
			int totalAdded = 0;
			//var bikes = BikeModelList.LoadActive();
			//foreach (var bike in bikes) {
			//	Feed<Google.YouTube.Video> vids = GetVideos(bike.Title);
			//
			//	// add video info to our database
			//	int howManyAdded = AddToDatabase(vids, bike.ID);
			//
			//	Web.Write("<br>"+bike.Title+": "+howManyAdded);
			//	totalAdded += howManyAdded;
			//}
			return totalAdded;
		}

	/*	private int GetYoutubeVideos(BikeModel bike, int howManyVideosToAdd = 900) { // 1000 is the max
			int pageSize = 50;
			int howManyAdded = 0;
			int loopNumber = 0;
			while (howManyAdded < howManyVideosToAdd) {

				Feed<Google.YouTube.Video> vids = GetVideos(bike.Title, (loopNumber * pageSize) + 1, pageSize);

				try {
					if (vids.TotalResults <= 0) break;
				} catch (Google.GData.Client.GDataRequestException) {
					// ran out of videos - so break
					break;
				}

				// add video info to our database
				howManyAdded += AddToDatabase(vids, bike.ID);
				loopNumber++;
				if (vids.TotalResults < loopNumber * pageSize) break;
			}
			return howManyAdded;
		}
*/
		
		private Feed<Google.YouTube.Video> GetVideos(string keyword) { //, int startIndex, int pageLength
			YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

			// tag search (lowercase - upper would mean category search)
			AtomCategory category1 = new AtomCategory(keyword, YouTubeNameTable.KeywordSchema);
			query.Categories.Add(new QueryCategory(category1));
			//query.Query = keyword; // normal 'everything' search
			query.NumberToRetrieve = 4;
			//query.StartIndex = startIndex;
			query.LR = "en";
			query.Time = YouTubeQuery.UploadTime.ThisWeek;
			query.SafeSearch = YouTubeQuery.SafeSearchValues.Moderate;
			query.OrderBy = "rating";

			YouTubeRequestSettings settings = new YouTubeRequestSettings("HowLong", Beweb.Util.GetSetting("YoutubeApi"));
			YouTubeRequest request = new YouTubeRequest(settings);
			Feed<Google.YouTube.Video> feed = request.Get<Google.YouTube.Video>(query);

			return feed;
		}

		private int AddToDatabase(Feed<Google.YouTube.Video> vids, int bikeModelID) {
			int entriesAdded = 0;
			foreach (var vid in vids.Entries) {
				int? checkVid = Beweb.BewebData.GetValueInt(new Sql("SELECT COUNT(VideoId) FROM Video WHERE VideoCode=", vid.VideoId.Sqlize_Text()));
				if (checkVid > 0) continue; // don't add one that already exists

				Models.Video v = new Video();
				v.SourceWebsiteCode = "youtube";   // actually all are from youtube
				v.VideoCode = vid.VideoId;
				v.Title = vid.Title.Left(150);
				v.VideoDescription = vid.Description;
				v.Credit = vid.Author; //vid.Uploader;
				v.ViewCount = vid.ViewCount;
				v.Status = "New";
				v.VideoPostedDate = Convert.ToDateTime(vid.YouTubeEntry.Published);
				v.DateAdded = DateTime.Now;
				v.ThumbnailUrl = vid.Thumbnails[0].Url;
				v.BikeModelID = bikeModelID;
				v.IsAuto = true;
				v.Save();
				entriesAdded++;
			}
			return entriesAdded;
		}


	}
}
