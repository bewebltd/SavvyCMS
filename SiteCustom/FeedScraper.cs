using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Beweb;
using CloudinaryDotNet.Actions;
using Models;
using Site.Controllers;

namespace Site.SiteCustom {
	public class FeedScraper {

		public void ScrapeTwitter() {
			ScrapeTwitter(null);
		}

		public void ScrapeTwitter(Feed specificFeed) {
			var consumerKey = Util.GetSetting("TwitterOAuthConsumerKey", "");
			var consumerSecret = Util.GetSetting("TwitterOAuthConsumerSecret", "");

			FeedList feeds;

			if (specificFeed == null) {
				feeds = FeedList.LoadByFeedType("Twitter").Where(f => f.IsActive).ToList();
			} else {
				feeds = new FeedList();
				feeds.Add(specificFeed);
			}

			var twitter = new TwitterAPI(consumerKey, consumerSecret);
			var runResult = "";
			var feedIsActive = true;
			string jsonString = "";
			foreach (var feed in feeds) {
				try {
					jsonString = twitter.GetJson(feed.FeedUrl);
					Web.Write("<br><br><br>" + jsonString);
					dynamic json = JsonHelper.Parse(jsonString);
					dynamic tweets;
					if (jsonString.Trim().StartsWith("[")) {
						// json format is an array of tweets 
						tweets = json;
					} else {
						// json format is an object containing a list of tweets as 'statuses' property
						tweets = json.statuses;
					}
					
					const string twitterDateTimeFormat = "ddd MMM dd HH:mm:ss zzzz yyyy";

					foreach (var tweet in tweets) {
						var createdAt = DateTime.ParseExact(tweet.created_at, twitterDateTimeFormat, CultureInfo.InvariantCulture);
						if(createdAt < DateTime.Now.AddDays(-1)) continue;

						var uniqueStringValue = "twitter:" + tweet.id_str;
						
						var story = FeedStory.LoadByUniqueString(uniqueStringValue);

						if (story == null) {
							story = new FeedStory();
							story.UniqueString = uniqueStringValue;
							story.AuthorName = tweet.user.name + " via Twitter";
							story.ProfileImageUrl = tweet.user.profile_image_url;
							story.ImageUrl = tweet.entities.media != null ? tweet.entities.media[0].media_url : null;
							story.Message = tweet.text;
							story.FeedID = feed.FeedID;
							story.Status = "new";
							story.Priority = feed.DefaultPriority;

							// get location from tweet if supplied in "geo"
							// note: weirdly it seems that these checks on dynamic objects cannot put in a single if statement together, maybe the short cut logic does not work the same
							if (tweet.geo != null) {
								if (tweet.geo.type == "Point") {
									if (tweet.geo.coordinates != null) {
										if (tweet.geo.coordinates.Length == 2) {
											story.Latitude = tweet.geo.coordinates[0];
											story.Longitude = tweet.geo.coordinates[1];
										}
									}
								}
							}

							story.Save();
							story.ApplyFilters();

							// If it didn't match any filter and it's still null, get from the feed
							if (story.Latitude == null || story.Longitude == null) {
								story.Latitude = feed.Latitude;
								story.Longitude = feed.Longitude;
								story.Save();
							}

							Web.Write("<p><strong>Added new tweet as feed story (Twitter Feed ID: " + feed.FeedID + ")</strong></p><p>" + story.Message + "</p><hr/>");
						}
					}

					runResult = "Success";

				} catch (Exception ex) {
					Web.Write("<p><strong style='color:red'>Error! Twitter Feed ID: " + feed.FeedID + "</strong></p><p>" + ex.Message + "</p><hr/>");
					Web.Write(jsonString);
					runResult = ex.Message;
					//SendFeedErrorEmail(feed, ex);
					//feedIsActive = false;
				} finally {
					feed.LastResult = runResult;
					feed.LastTimeRunning = DateTime.Now;
					feed.IsActive = feedIsActive;
					feed.Save();
				}
			}
		}

		public void ScrapeEventFinder() {
			ScrapeEventFinder(null);
		}

		public void ScrapeEventFinder(Feed specificFeed) {

			FeedList feeds;

			if (specificFeed == null) {
				feeds = FeedList.LoadByFeedType("Eventfinder").ToList();
			} else {
				feeds = new FeedList();
				feeds.Add(specificFeed);
			}

			var runResult = "";
			var feedIsActive = true;

			foreach (var feed in feeds) {

				try {
					string username = "herepincom";
					string password = "b6qkhwqpg548";
					WebClient wc = new WebClient();
					wc.Credentials = new NetworkCredential(username, password);
					string url = "http://api.eventfinder.co.nz/v2/events.json?rows=20";
					url += "&fields=event:(id,url,name,description~200,datetime_summary,point,location_summary,address,username,images),images:(image),image:(transforms),transforms:(transform),transform:(url,width)";
					//url += "&order=popularity";
					//url += "&created_since=" + Fmt.DateISO(DateTime.UtcNow.Date);
					//url += "&end_date=" + Fmt.DateISO(DateTime.UtcNow.Date.AddDays(14));
					//url += "&point=-36.796581,174.777978&radius=100";
					url += "&point=" + feed.Latitude + "," + feed.Longitude + "&radius=" + feed.Radius;
					//url += "&free=1";
					var jsonString = wc.DownloadString(url);
					//Web.WriteLine(jsonString);
					//string currentUserToken = "52e0eaf6e4b07e1b6390a5dc";  //mikes

					dynamic json = JsonHelper.Parse(jsonString);

					foreach (var obj in json.events) {

						var uniqueStringValue = "eventfinda:" + obj.id;
						var story = FeedStory.LoadByUniqueString(uniqueStringValue);

						if (story == null) {

							string currentUserName = obj.username + " via Eventfinder";
							var shortUrl = UrlShortenerController.UrlShortener.Create(obj.url, currentUserName, true);

							string message = ((String)obj.name).StripTags() + " " + obj.datetime_summary + ". " + ((String)obj.description).StripTags();
							message = (message.Length > 130 ? Fmt.TruncHTML(message, 130) + "..." : message) + " #events " + shortUrl;
							//var imageUrl = node.ElementValue("enclosure");
							story = new FeedStory();
							story.UniqueString = uniqueStringValue;
							//story.Title = node.ElementValue("title");
							story.ImageUrl = GetImage(obj);
							story.Message = message;
							story.OriginalLinkUrl = obj.url;
							//story.DateAdded = node.ElementValue("pub-date").ConvertToDate(DateTime.Now);
							story.Status = "new";
							story.AuthorName = currentUserName;
							story.FeedID = feed.FeedID;
							story.ProfileImageUrl = "https://pbs.twimg.com/profile_images/2461203629/2kvvznykuqzo46kj6kec_normal.jpeg";
							story.Latitude = (decimal)obj.point.lat;
							story.Longitude = (decimal)obj.point.lng;
							story.Priority = feed.DefaultPriority;
							story.Save();
							story.ApplyFilters();

							Web.Write("<p><strong>Added new event as feed story (Feed ID: " + feed.FeedID + ")</strong></p><p>" + message + "</p><hr/>");
						}
					}

					runResult = "Success";

				} catch (Exception ex) {
					Web.Write("<p><strong style='color:red'>Error! Event Finder Feed ID: " + feed.FeedID + "</strong></p><p>" + ex.Message + "</p><hr/>");
					runResult = ex.Message;
					//SendFeedErrorEmail(feed, ex);
					//feedIsActive = false;
				} finally {
					feed.LastResult = runResult;
					feed.LastTimeRunning = DateTime.Now;
					feed.IsActive = feedIsActive;
					feed.Save();
				}
			}

		}

		private string GetImage(dynamic obj) {
			var imageUrl = "";
			var width = 0;
			if (obj.images != null && obj.images.images != null && obj.images.images.Length > 0) {
				foreach (var image in obj.images.images) {
					foreach (var tran in image.transforms.transforms) {
						if (tran.width > width) {
							imageUrl = tran.url;
						}
					}
				}
			}
			return imageUrl;
		}

		public void ScrapeRss() {
			ScrapeRss(null);
		}

		public void ScrapeRss(Feed specificFeed) {

			FeedList feeds;

			if (specificFeed == null) {
				feeds = FeedList.LoadByFeedType("RSS").Where(f => f.IsActive).ToList();
			} else {
				feeds = new FeedList();
				feeds.Add(specificFeed);
			}

			var runResult = "";
			var xmlString = "";
			var feedIsActive = true;

			foreach (var feed in feeds) {

				try {

					string url = feed.FeedUrl;
					xmlString = Http.Get(url);
					//Web.WriteLine(xmlString);
					//string profileImageUrl = "https://pbs.twimg.com/profile_images/2461203629/2kvvznykuqzo46kj6kec_normal.jpeg";
					//string currentUserToken = "52e0eaf6e4b07e1b6390a5dc";  //mikes

					var xml = bwbXml.Parse(xmlString);
					foreach (var node in xml.Descendants("item")) {

						var uniqueStringValue = "rss:" + Crypto.CreateHash(node.ElementValue("guid") ?? node.ElementValue("link"));
						var story = FeedStory.LoadByUniqueString(uniqueStringValue);

						if (story == null) {

							string link = node.ElementValue("link");

							if (String.IsNullOrEmpty(link)) {
								link = ExtractUrlFromHTML(node.ElementValue("description"));
							}

							var shortUrl = "";

							if (!String.IsNullOrEmpty(link)) {
								shortUrl = UrlShortenerController.UrlShortener.Create(link, "RSS Feed", true);
							}

							//string message = Fmt.TruncHTML(node.ElementValue("title").StripTags() + ". " + node.ElementValue("description").StripTags(), 130) + " " + shortUrl;

							string message = node.ElementValue("title").StripTags() + ". " + node.ElementValue("description").StripTags();
							message = (message.Length > 130 ? Fmt.TruncHTML(message, 130) + "..." : message) + " " + shortUrl;

							//var imageUrl = node.ElementValue("enclosure");
							story = new FeedStory();
							story.UniqueString = uniqueStringValue;
							story.Title = node.ElementValue("title");
							story.Message = message;
							story.OriginalLinkUrl = node.ElementValue("link");
							story.DateAdded = node.ElementValue("pub-date").ConvertToDate(DateTime.Now);
							story.Status = "new";
							story.AuthorName = feed.Author.Name + (feed.Author.Via.IsNotBlank() ? " via " + feed.Author.Via : "");
							story.FeedID = feed.FeedID;
							story.ProfileImageUrl = feed.Author.ProfilePicUrl;
							story.Priority = feed.DefaultPriority;
							story.Save();
							story.ApplyFilters();

							// If it didn't match any filter and it's still null, get from the feed
							if (story.Latitude == null || story.Longitude == null) {
								story.Latitude = feed.Latitude;
								story.Longitude = feed.Longitude;
								story.Save();
							}

							Web.Write("<p><strong>Added new item as feed story (RSS Feed ID: " + feed.FeedID + ")</strong></p><p>" + message + "</p><hr/>");
						}
					}

					runResult = "Success";

				} catch (Exception ex) {
					Web.Write("<p><strong style='color:red'>Error! RSS Feed ID: " + feed.FeedID + "</strong></p><p>" + ex.Message + "</p><hr/>");
					runResult = ex.Message;
					//SendFeedErrorEmail(feed, ex);
					//feedIsActive = false;
					//SendEMail.SimpleSendHtmlEmail("andre@beweb.co.nz", "Herepin Feed error", "RSS Feed ID: " + feed.FeedID + "<br>"+ex.Message+"<br><br>" + xmlString);
				} finally {
					feed.LastResult = runResult;
					feed.LastTimeRunning = DateTime.Now;
					feed.IsActive = feedIsActive;
					feed.Save();
				}
			}
		}

		public void DeleteUnpublishedOldStories(int days) {

			Web.Write("<p><strong>Deleting unpublished stories older than " + days + " days</strong></p>");

			string sql = @" DELETE FROM FeedStory
											WHERE DATEADD(DAY, -" + days + @", GETDATE()) > DateAdded AND
														Status != 'published' AND
														Status != 'auto-published'";

			int rows = 0;

			try {
				rows = new Sql().AddRawSqlString(sql).Execute();
				Web.Write("<p>" + Fmt.Plural(rows, "story") + " deleted</p>");
			} catch (Exception ex) {
				Web.Write("<p><strong style='color:red'>Error! </strong>" + ex.Message + "</p>");
			}

			Web.Write("<hr/>");
		}

		private string ExtractUrlFromHTML(string html) {

			MatchCollection m1 = Regex.Matches(html, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);

			foreach (Match m in m1) {
				string value = m.Groups[1].Value;

				Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);

				if (m2.Success) {
					return HttpUtility.HtmlDecode(m2.Groups[1].Value);
				}
			}

			return String.Empty;
		}

		private void SendFeedErrorEmail(Feed feed, Exception ex) {

			String emailBodyTxt = TextBlockCache.Get("FeedError", @"
Hello Herepin Admin,
	
	An error has occurred while scrapping [--FeedType--] Feed.
	
	Error details:
	[--ErrorMessage--]
	[--FeedURL--]

	<a href='[--RunFeedURL--]'>Run feed</a> | <a href='[--EditFeedURL--]'>Edit feed</a>

			").BodyTextHtml;

			emailBodyTxt = emailBodyTxt.Replace("[--FeedType--]", feed.FeedType);
			emailBodyTxt = emailBodyTxt.Replace("[--ErrorMessage--]", ex.Message);
			emailBodyTxt = emailBodyTxt.Replace("[--FeedURL--]", feed.FeedUrl);
			emailBodyTxt = emailBodyTxt.Replace("[--RunFeedURL--]", Web.BaseUrl + "ScheduledTask/RunFeed?feedID=" + feed.FeedID);
			emailBodyTxt = emailBodyTxt.Replace("[--EditFeedURL--]", Web.BaseUrl + "Admin/FeedAdmin/Edit/" + feed.FeedID);
			emailBodyTxt = emailBodyTxt.Replace("\r\n", "<br>");

			if (Util.ServerIsDev) {
				SendEMail.SimpleSendHtmlEmail("andre@beweb.co.nz", "Herepin Feed error", emailBodyTxt);
			} else {
				SendEMail.SimpleSendHtmlEmail(Util.GetSetting("EmailToAddress", "localhost"), "Herepin Feed error", emailBodyTxt);
				SendEMail.SimpleSendHtmlEmail("mike@beweb.co.nz", "Herepin Feed error", emailBodyTxt);
				SendEMail.SimpleSendHtmlEmail("andre@beweb.co.nz", "Herepin Feed error", emailBodyTxt);
			}

		}

		//public void PullEventfindaFeed() {
		//	string username = "herepincom";
		//	string password = "b6qkhwqpg548";
		//	WebClient wc = new WebClient();
		//	wc.Credentials = new NetworkCredential(username, password);
		//	string url = "http://api.eventfinder.co.nz/v2/events.json?rows=20";
		//	url += "&fields=event:(url,name,description~200,datetime_summary,point,location_summary,address,username,images),images:(image),image:(transforms),transforms:(transform),transform:(url,width)";
		//	//url += "&order=popularity";
		//	//url += "&created_since=" + Fmt.DateISO(DateTime.UtcNow.Date);
		//	//url += "&end_date=" + Fmt.DateISO(DateTime.UtcNow.Date.AddDays(14));
		//	url += "&point=-36.796581,174.777978&radius=100";
		//	//url += "&free=1";
		//	var jsonString = wc.DownloadString(url);
		//	Web.WriteLine(jsonString);
		//	string profileImageUrl = "https://pbs.twimg.com/profile_images/2461203629/2kvvznykuqzo46kj6kec_normal.jpeg";
		//	string currentUserToken = "52e0eaf6e4b07e1b6390a5dc";  //mikes

		//	dynamic json = JsonHelper.Parse(jsonString);
		//	foreach (var obj in json.events) {
		//		string currentUserName = obj.username + " via Eventfinder";
		//		var shortUrl = UrlShortenerController.UrlShortener.Create(obj.url, currentUserName, true);
		//		string message = Fmt.TruncHTML(obj.name.StripTags() + ". " + obj.datetime_summary + ". " + obj.description.StripTags(), 130) + " #events " + shortUrl;
		//		var imageUrl = GetImage(obj);
		//		//SubmitPin(message, (double)obj.point.lat, (double)obj.point.lng, (string)obj.location_summary, currentUserName, profileImageUrl, currentUserToken);
		//		//todo: stuff in nnew table feedstory
		//		//var story = new FeedStory();
		//		Web.WriteLine(message + "==" + imageUrl);
		//	}
		//}

	}
}