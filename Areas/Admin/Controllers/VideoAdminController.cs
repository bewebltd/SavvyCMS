// Admin Video Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class VideoAdminController : AdminBaseController {
		//
		// GET: /Admin/Video/

		public ActionResult Approval() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Video Approval");
			Util.SetReturnPage(2);
			//var dataList = VideoList.LoadByStatus("New");
			Sql sql = new Sql("SELECT video.* FROM Video  where 1=1");
			sql.Add("and status =","New".Sqlize_Text());
			var dataList = VideoList.Load(sql);
			return View("VideoApproval", dataList);
		}

		public string ApproveVid() {
			int vid = Request["VID"].ToInt();
			bool approved = Request["Approved"].ConvertToBool();

			Models.Video v = Models.Video.LoadByVideoID(vid);
			if(approved) {
				v.Status = "Approved";
			}	else {
				v.Status = "Rejected";
			}
			v.Save();

			return vid.ToString();
		}


		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Video List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("VideoList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.Video> {
			public string StatusFilter = Web.Request["StatusFilter"].DefaultValue("Approved");
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT video.* FROM Video  where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, "video.title", true);
				}
				sql.Add("and status =",StatusFilter.Sqlize_Text());
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				//if (SortBy.IsBlank()) {
				//  // hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
				//  sql.AddRawSqlString(new Models.Video().GetDefaultOrderBy());   
				//} else {
				//  sql.AddSql(GetOrderBySql());
				//}
				sql.Add("ORDER BY VideoPostedDate DESC");
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Video/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Video");
			var record = new Models.Video();
			// any default values can be set here or in partial class Video.InitDefaults() 
			return View("VideoEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Video/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Video());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Video/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Video");
			var record = Models.Video.LoadID(id);
			return View("VideoEdit", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Video/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Video.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Video record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					TempData["info"] = "Video "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("VideoEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Video();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				TempData["info"] = "Copy of Video "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Video record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Video record, bool isNew) {
			// add any code to update other fields/tables here
			if (isNew) {
				record.VideoCode = ParseVideoUrl("YouTube", Request["VideoURL"]);
			}
			record.ThumbnailUrl = "http://i.ytimg.com/vi/"+record.VideoCode+"/default.jpg";
			record.Save();
		}

		public string ParseVideoUrl(string website, string videoUrl) {
			string code = "";
			videoUrl = videoUrl.Trim();
			if (website == "YouTube") {
				// eg http://www.youtube.com/watch?v=JXiFsB4SYlc&feature=popular
				// or http://www.youtube.com/watch?v=JXiFsB4SYlc
				// or http://youtu.be/JXiFsB4SYlc
				if (videoUrl.Contains("youtu.be/")) {
					code = videoUrl.RightFrom("youtu.be/");
				} else if (videoUrl.Contains("youtube.com/watch?v=")) {
					code = videoUrl.RightFrom("youtube.com/watch?v=");
				}
				if (code.Contains("&")) {
					code = code.Split('&')[0];
				}
				if (code.Length != 11) {
					// codes must be 11 characters 
					code = ""; // code is invalid
				}
			}
			return code;
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Video/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Video.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("VideoEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
