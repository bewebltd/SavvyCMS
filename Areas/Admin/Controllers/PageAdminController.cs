// Admin Page Controller
#define xPageRevisions
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
#if PageRevisions
using DiffMatchPatch; //this is a file in C:\data\Projects\CodelibMVC\SiteCustom\3rdParty\DiffMatchPatch.cs
#endif
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class PageAdminController : AdminBaseController {
		//
		// GET: /Admin/Page/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Page Hierarchy");
			Util.SetReturnPage(2);
			//var data = new PageHierarchyViewModel();
			//return View("PageHierarchy");
			var dataList = new ListHelper();
			dataList.PageLoad();
			if (Web.Request["code"] == null) {
				dataList.PageHierarchy = Page.GetPageHierarchy();//this is only called by the admin

				return View("PageHierarchy", dataList);
			} else {


				return View("PageList", dataList);
			}
		}
#if PageRevisions
		public ActionResult PageRevisions(int pageID) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Page Revisions");
			Util.SetReturnPage(3);

			var dataList = new ListHelper();
			dataList.PageIDForRevisions = pageID;
			dataList.PageLoad();

			return View("PageRevisionList", dataList);
		}
#endif
		public class ListHelper : SavvyDataList<Models.Page> {
#if PageRevisions
			public int? PageIDForRevisions;
#endif
			public ListHelper() {
				ShowSearch = false;
				ShowExport = false;
			}
			public PageList PageHierarchy;


			public override string PagingNav() {
				return "";
			}

			public override int RecordCount {
				get {
					if (Web.Request["code"] == null) {
						return PageHierarchy.Count;
					} else {
						return base.RecordCount;
					}
				}
			}

			public override Sql GetSql() {
				//Sql sql = new Sql("SELECT p.* FROM Page p left join Page pp on p.parentpageid=pp.pageid where 1=1");
				Sql sql = new Sql("SELECT p.* FROM Page p  where 1=1");
				sql.ResultSetPagingType = Sql.PagingType.sql2000;
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, "Title,NavTitle,PageCode,UrlRewriteTitle", true);
				}
#if PageRevisions
				if (PageIDForRevisions.HasValue) sql.Add("and historypageid=" + PageIDForRevisions);
#endif
				// handle an fk (rename fkid, then uncomment)
				if (Web.Request["code"].IsNotBlank()) {
					var parent = Models.Page.LoadByPageCode(Web.Request["code"]);
					sql.Add("and parentpageid=", parent.ID);
				}
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					//sql.AddRawSqlString("order by isnull(pp.SortPosition,p.SortPosition), isnull(pp.PageID,p.PageID)");
					sql.AddRawSqlString("order by PublishDate desc");
				} else {
					sql.AddSql(GetOrderBySql());
				}
				//Logging.dout(sql.ToString());
				return sql;
				return null;
			}


			/// <summary>
			/// custom filters to allow child page button or not
			/// </summary>
			/// <param name="page"></param>
			/// <param name="depth"></param>
			/// <returns></returns>
			public bool AllowAddChildPage(Models.Page page, int depth) {
				var result = true;
				if (depth >= Util.GetSetting("SiteNavigationDepth").ToInt() - 1) {
					result = false;
				}
				if (page.TemplateCode == "special") {
					result = false;
					if (page.PageCode == "something") { //controller
						result = true;
					}
				}
				if (page.TemplateCode == "link") {
					result = false;
				}
				if (page.TemplateCode == "page") {
					result = false;
				}
				if (page.TemplateCode == "page2col") {
					result = false;
				}
				if (page.TemplateCode == "contact") {
					result = false;
				}
				if (page.TemplateCode == "peoplelist") {
					result = false;
				}
				if (page.TemplateCode == "documentrepository") {
					result = false;
				}
				if (page.PageCode == "Search") {
					result = false;
				}
				return result;
			}

			/// <summary>
			/// custom filter to specify page template based on parent template or page code
			/// </summary>
			/// <param name="record"></param>
			/// <returns></returns>
			public static string GetDefaultTemplateCode(Models.Page record) {
				var result = "page";
				if (record.ParentPageID.HasValue) {
					var parent = Models.Page.LoadByPageID(record.ParentPageID.Value);
					if (parent != null) {
						var parenttemplateCode = parent.TemplateCode;
						if (parenttemplateCode == "section") {
							result = "page";
						}
					}
				}
				return result;
			}
		}

		public class PageHierarchyViewModel {
			public PageList PageHierarchy;

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Page/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Page");
			var record = new Models.Page();
			if (Web.Request["ParentPageID"] != null) record.ParentPageID = Web.Request["ParentPageID"].ToInt(null);

			if (Web.Request["code"].IsNotBlank()) {
				var parent = Models.Page.LoadByPageCode(Web.Request["code"]);
				record.ParentPageID = parent.ID;
			}
			record.TemplateCode = ListHelper.GetDefaultTemplateCode(record);
			// any default values can be set here or in partial class Page.InitDefaults() 
#if PageRevisions
			return View("PageEditRevisions", record);
#else
			return View("PageEdit", record);
#endif
		}


		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Page/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
#if PageRevisions
			bool usingRevision = Settings.All.EnablePageRevisions;
			var page = new Page();
			return usingRevision ? ProcessFormUsingRevision(page) : ProcessForm(page);			
#else
			return ProcessForm(new Models.Page());
#endif
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Page/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Page");
			var record = Models.Page.LoadID(id);
			CheckLock(record);
#if PageRevisions
			return View("PageEditRevisions", record);
#else
			return View("PageEdit", record);
#endif
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Page/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Page");
			var record = Models.Page.LoadID(id);
			return View("PageView", record);
		}

		public JsonResult GetSortOrder(int id) {
			int sortOrder = new Sql("select max(sortposition + 1) from dbo.Page where pageid =", id.SqlizeNumber()).FetchIntOrZero();
			return Json(sortOrder, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Page/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
#if PageRevisions
			bool usingRevision = Settings.All.EnablePageRevisions;
			var page = Page.LoadID(id);
			return usingRevision ? ProcessFormUsingRevision(page) : ProcessForm(page);
#else

			return ProcessForm(Models.Page.LoadID(id));
#endif
		}

#if PageRevisions
	/// <summary>
		/// Loads existing record and compares the content to the live page
		/// GET: /Admin/PageAdmin/CompareToLive/5
		/// </summary>
		public ActionResult CompareToLive(int revision1, int? revision2) {
			
			var data = new PageComparisonViewModel();

			// It will always be the one the person selected in the list
			data.Revision1 = Page.LoadID(revision1);

			// It will be Live by default, but the person can change
			data.Revision2 = Page.LoadID(revision2 == null ? GetOriginalPageID(data.Revision1) : revision2.Value);

			data.TitleDifferences = GetTextDifferences(data.Revision2.Title, data.Revision1.Title);
			data.SubTitleDifferences = GetTextDifferences(data.Revision2.SubTitle, data.Revision1.SubTitle);
			data.IntroductionDifferences = GetTextDifferences(data.Revision2.Introduction, data.Revision1.Introduction);
			data.BodyTextDifferences = GetTextDifferences(data.Revision2.BodyTextHtml, data.Revision1.BodyTextHtml);
			data.NavTitleDifferences = GetTextDifferences(data.Revision2.NavTitle, data.Revision1.NavTitle);
#if PageRevisions
			//data.NavIntroductionDifferences = GetTextDifferences(data.Revision2.NavIntroduction, data.Revision1.NavIntroduction);
			//data.NavLinkTitleDifferences = GetTextDifferences(data.Revision2.NavLinkTitle, data.Revision1.NavLinkTitle);
#endif
			data.PageTitleTagDifferences = GetTextDifferences(data.Revision2.PageTitleTag, data.Revision1.PageTitleTag);
			data.MetaKeywordsDifferences = GetTextDifferences(data.Revision2.MetaKeywords, data.Revision1.MetaKeywords);
			data.MetaDescriptionDifferences = GetTextDifferences(data.Revision2.MetaDescription, data.Revision1.MetaDescription);
			data.SideBarTitleDifferences = GetTextDifferences(data.Revision2.SidebarTitle, data.Revision1.SidebarTitle);
			data.SideBarTextHtmlDifferences = GetTextDifferences(data.Revision2.SidebarTextHtml, data.Revision1.SidebarTextHtml);

			var pageDataList = new ListHelper();
			pageDataList.PageIDForRevisions = GetOriginalPageID(data.Revision1);
			pageDataList.PageLoad();
			var results = pageDataList.GetResults();
			int revisionNumber = results.RecordCount; 

			data.RevisionDropbox.Add(GetOriginalPageID(data.Revision1).ToString(), "Live");
			foreach (var page in results) {
				data.RevisionDropbox.Add(page.PageID.ToString(), "Revision No. " + revisionNumber + " - " + page.DateModified.FmtDateTime());
				revisionNumber--;
			}
			
			return View("PageComparison", data);
		}
#endif
		protected ActionResult ProcessForm(Models.Page record) {
			try {
				record.UpdateFromRequest();
				//record.RelatedPages.UpdateFromRequestSubForm();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();
#if PageRevisions
				record.RevisionStatus = "Live";
#endif

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					record.RelatedPages.Save();
					Web.InfoMessage += "Page " + record.GetName() + " saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
#if PageRevisions
			return View("PageEditRevisions", record);
#else
				return View("PageEdit", record);
#endif
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Page();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Page " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

#if PageRevisions

		protected ActionResult ProcessFormUsingRevision(Page editingRecord) {
			try {

				bool isPublishing = Web.Request["Publish"] != null;
				
				// If it is a new record, we just need to put the correct status and save
				if (editingRecord.IsNewRecord) {
					editingRecord.UpdateFromRequest();
					editingRecord.RelatedPages.UpdateFromRequestSubForm();
					editingRecord.RevisionStatus = isPublishing ? "Live" : "Draft";
					editingRecord.ModifiedByPersonID = UserSession.Person.ID;
					editingRecord.Fields.Picture.MetaData = editingRecord.GetMetaData();
					editingRecord.Save();
					editingRecord.RelatedPages.Save();
					SetPageSavedInfoMessage(editingRecord);
				} else {

					var originalPageID = GetOriginalPageID(editingRecord);
					var newRevision = new Page();

					// User clicked publish button
					if (isPublishing) {
						
						// First step: Backup live data as History revision

						// If we are editing the original page, we don't need to get from DB again
						Page originalPage = (originalPageID == editingRecord.PageID) ? editingRecord : Page.LoadID(originalPageID);

						newRevision.UpdateFrom(originalPage);
						newRevision.RevisionStatus = "History";
						newRevision.ModifiedByPersonID = UserSession.Person.ID;
						newRevision.HistoryPageID = originalPageID;
						newRevision.Save();

						// Second step: Override live data using the form data
						originalPage.UpdateFrom(editingRecord);
						originalPage.UpdateFromRequest();
						originalPage.RelatedPages.UpdateFromRequestSubForm();
						originalPage.RevisionStatus = "Live";
						originalPage.ModifiedByPersonID = UserSession.Person.ID;
						originalPage.EditorNotes = null;
						originalPage.HistoryPageID = null;
						originalPage.Fields.Picture.MetaData = editingRecord.GetMetaData();
						originalPage.Save();
						originalPage.RelatedPages.Save();
						//originalPage.Resources.Save();
						//originalPage.PageCarousels.Save();

						SetPageSavedInfoMessage(originalPage);

					} else { // User clicked save draft (or Save & Request Approval, if the user is an editor)
						// Don't touch live data, just create a new Draft revision
						newRevision.UpdateFrom(editingRecord);
						newRevision.UpdateFromRequest();
						newRevision.ModifiedByPersonID = UserSession.Person.ID;
						newRevision.RevisionStatus = "Draft";
						newRevision.HistoryPageID = originalPageID;

						// If it's not an editor creating this draft, clear the Editor Notes
						if (Web.Request["SaveAndRequestApproval"] == null) {
							newRevision.EditorNotes = null;
						}

						newRevision.Save();

						Web.InfoMessage = "Page Draft saved.";

						// If it's an editor creating this draft, send email
						if (Web.Request["SaveAndRequestApproval"] != null) {
							SendApprovalEmail(newRevision);
							Web.InfoMessage = "Page saved and email sent.";
						}
					}
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			PageCache.Rebuild();

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
#if PageRevisions
				return View("PageEditRevisions",editingRecord);
#else
				return View("PageEdit", editingRecord);
#endif
			} else {
				//return RedirectToReturnPage();
				return RedirectToAction("index");
			}
		}

		private void SetPageSavedInfoMessage(Page record) {
			if (record.PublishDate == null || (record.ExpiryDate != null && record.ExpiryDate < DateTime.Today)) {
				Web.InfoMessage = "Page saved. Put valid dates to publish";
			} else if (record.PublishDate > DateTime.Today) {
				Web.InfoMessage = "Page saved. It will be published on " + record.PublishDate.FmtDate();
			} else {
				Web.InfoMessage = "Page saved and published";
			}
		}

		private int GetOriginalPageID(Page record) {

			// If the page that is being edited is the live page, use the PageID, 
			// otherwise use the HistoryPageID that will ALWAYS be the ID of live page.

			// If it does have a HistoryPageID, it means that we are not editing live page
			if (record.HistoryPageID != null) {
				return (int)record.HistoryPageID;
			} 
			
			// it means that we are editing the live page or there is no live page and there is a draft page
			return record.PageID;
		}

		private void SendApprovalEmail(Page record) {

			Person toPerson = Person.LoadByPersonID((int)record.RequestApprovalForPersonID);

			var email = new ElectronicMail();
			email.ToAddress = toPerson.Email;

			var textBlock = new Beweb.TextBlock("Email: Page Approval Request", "Page Approval Request",
				@"Dear [--publisherName--],

[--editorName--] requested approval on a page. Please click [--url--] to review.

Thanks.");

			email.Subject = textBlock.Title;

			var body = textBlock.RawBody;
			body = body.Replace("[--publisherName--]", toPerson.FirstName);
			body = body.Replace("[--editorName--]", UserSession.Person.FirstName);
			body = body.Replace("[--url--]", record.GetAdminFullUrl());

			email.FromAddress = Util.GetSetting("EmailFromAddress", "andre@beweb.co.nz");
			email.BodyPlain = body;
			email.Send(true);
		}
#endif
		private void Validate(Models.Page record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
			string url = record.URLRewriteTitle;
			//if (!IsRouteFree(url)) {
			//  ModelState.AddModelError("URLRewriteTitle", "URL is already in use.");
			//}
			if (url.IsNotBlank()) {
#if PageRevisions
				var sql = new Sql("select * from page where URLRewriteTitle=", url.Sqlize_Text() ," and RevisionStatus = ", "Live".SqlizeText() ,"");
#else
				var sql = new Sql("select * from page where URLRewriteTitle=", url.Sqlize_Text());
#endif
				if (!record.IsNewRecord) {
					sql.Add("and pageid<>", record.PageID);
				}
				if (PageList.Load(sql).Active.Count > 0) {
					ModelState.AddModelError("URLRewriteTitle", "URL is already in use.");
				}
			}
		}

		//private bool IsRouteFree(string url) {
		//  // make sure URL is unique and does not conflict with a controller
		//  foreach (Route route in RouteTable.Routes) {
		//    if (url == route.Defaults["controller"].ToString().ToLower())
		//      return false;
		//    if (url == route.Defaults["action"].ToString().ToLower())
		//      return false;
		//  }
		//  return true;
		//}


		private void Save(Models.Page record, bool isNew) {
			// add any code to update other fields/tables here

			//record.Fields.Picture.MetaData = record.GetMetaData();
			record.Save();
			//record.Resources.Save();
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			PageCache.Rebuild();
#if AutocompletePhrase	
			AutocompletePhrase.AddPagePhrase(record);
#endif
			// save subform or related checkboxes here eg record.Lines.Save();
		}
		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.Page.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			return Redirect(returnPage);

		}


		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Page/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Page.LoadID(id);
			//record.RelatedPages.DeleteAll();
			//record.RelatedPagesLinked.DeleteAll();
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				if (issues.Contains("Related records exist (RelatedPagesLinked)")) {
					Web.ErrorMessage = "Cannot delete this record. " + issues;
					return Redirect(Web.AdminRoot + "pageadmin");
				} else {
					record.RelatedPages.DeleteAll();
					record.RelatedPages.Save();
				}
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			//record.Deals.DeleteAll();
			record.Delete();
			PageCache.Rebuild();
			return Redirect(returnPage);
		}

#if PageRevisions

		private string GetTextDifferences(string a, string b) {
			var diff = new diff_match_patch();
			var differences = diff.diff_main(a+"",b+"");
			diff.diff_cleanupSemantic(differences);
			//Format as pretty html
			return diff.diff_prettyHtml(differences);
		}

		public class PageComparisonViewModel : PageTemplateViewModel {
			public Page Revision1; // It will always be the one the person selected in the list
			public Page Revision2; // It will be Live by default, but the person can change
			public String TitleDifferences;
			public String IntroductionDifferences;
			public String BodyTextDifferences;
			public String SubTitleDifferences;
			public String NavTitleDifferences;
			public String NavIntroductionDifferences;
			public String NavLinkTitleDifferences;
			public String PageTitleTagDifferences;
			public String MetaKeywordsDifferences;
			public String MetaDescriptionDifferences;
			public String SideBarTitleDifferences;
			public String SideBarTextHtmlDifferences;
			public Forms.SelectOptions RevisionDropbox = new Forms.SelectOptions();
		}
#endif


		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));

			if (Crypto.Decrypt(Web.Request["t"]) == "RelatedPage") return Content("skip");
			foreach (var id in ids) {
				Sql sql = new Sql("update " + Crypto.Decrypt(Web.Request["t"]) + " set SortPosition=", pos * 10, "where " + Crypto.Decrypt(Web.Request["p"]) + "=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("Page");
			return Content("reload");
		}

	}
}
