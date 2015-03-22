using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using Beweb;

namespace Site.Controllers {
	/// <summary>
	/// Contains partials for rendering common page elements such as individual panels that are used on several pages
	/// </summary>
	public class CommonController : ApplicationController {


		public ActionResult WarningBar() {
			return View();
		}

#if DocumentRepository
		public ActionResult DocumentCategory(DocumentCategory category) {
			/* Categories with Parent Categories */
			var data = new DocumentCategoryViewModel();
			data.Categories = category.ChildDocumentCategories;
			data.Documents = category.Documents;
			return View("DocumentCategory", data);
		}

		public ActionResult DocumentCategoryRoot(Page page) {
			/* Root Categories */
			var data = new DocumentCategoryViewModel();
			data.Categories = DocumentCategoryList.LoadByPageID(page.ID);
			data.Documents = new DocumentList();
			/* we dont want to get the documents at this stage as its the root and we are not in a category
			foreach (DocumentCategory category in data.Categories) {
				if (category.Documents.Count > 0) {
					foreach (var document in category.Documents) {
						data.Documents.Add(document);
					}
				}
			}
			*/
			return View("DocumentCategory", data);
		}

		public class DocumentCategoryViewModel {
			public DocumentCategoryList Categories;
			public DocumentList Documents;
		}
#endif

		public ActionResult GoogleAnalytics() {
			return View();
		}

		public ActionResult GoogleTagManager() {
			return View();
		}

		public ActionResult GetGoogleSuggest(string searchTerm) {
			var url = "http://google.com/complete/search?output=toolbar&q=" + searchTerm;
			var message = Util.HttpGet(url);
			return Content(message, "text/xml");
		}

		public ActionResult SaveRemoteImageLocally(string fullImageUrl) {
			var result = Http.DownloadImageFromRemoteSiteAndSaveToDisk(fullImageUrl);
			return Json(new { newImage = result });
		}

#if Pages
		public ActionResult SvyAdminBanner(Page contentPage) {
			return View("svyAdminBanner", contentPage);
		}
#endif

#if ArticlePage
		public ActionResult Resources(ArticleDocumentList documents, ArticleURLList urls, string template) {
			var data = new ResourceViewModel();
			data.Template = template;
			data.Documents = documents;
			data.Urls = urls;
			return View("ResourcesPanel", data);
		}

		public class ResourceViewModel {
			public string Template;
			public ArticleDocumentList Documents;
			public ArticleURLList Urls;

		}
#endif


#if Gallery     //put this stuff back if category has pageid
		//public class GalleryCategoryYearsViewModel {
		//	public Page ContentPage;
		//}

		//public ActionResult GalleryCategoryYears(Page contentPage) {
		//	var model = new GalleryCategoryYearsViewModel();
		//	model.ContentPage = contentPage;
		//	return View(model);
		//}
#endif

		public ActionResult TrackingGif(string guid) {
			var sql = new Sql("select * from MailLog where TrackingGuid = ", guid.SqlizeText());
			var record = new ActiveRecord("MailLog", "MailLogID");
			if (record.LoadData(sql)) {
				record["DateViewTracked"].ValueObject = DateTime.Now;
				record.Save();
			}
			var str = "R0lGODlhAQABAIABAP///wAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";
			var bytes = Convert.FromBase64String(str);
			Response.Clear();
			Response.ContentType = "image/gif";
			Response.BinaryWrite(bytes);
			return null;
		}
	}
}
