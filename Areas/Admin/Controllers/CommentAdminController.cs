// Admin Comment Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class CommentAdminController : AdminBaseController {
		//
		// GET: /Admin/Comment/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Comment List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("CommentList", dataList);
		}
		public ActionResult Moderate() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Comments To Moderate");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.StatusFilter = Comment.CommentStatus.Submitted.ToString();
			dataList.PageLoad();
			return View("CommentList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.Comment> {
			public string StatusFilter { get; set; }
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Comment where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, "CommentText,CommenterName,CommenterEmail", true);
				}
				if (Web.Request["auctionId"].IsNotBlank()) {
					// click link on auction list to get here - so filter by that auction
					sql.Add(" and AuctionID=", Web.Request["auctionId"].SqlizeNumber());
				}
				if(StatusFilter.IsNotBlank()) {
					sql.Add("AND Status=", StatusFilter.SqlizeText());
				}

	
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.Comment().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Comment/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Comment");
			var record = new Models.Comment();
			// any default values can be set here or in partial class Comment.InitDefaults() 
			return View("CommentEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Comment/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Comment());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Comment/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Comment");
			var record = Models.Comment.LoadID(id);
			// if we can't find the auction - just go back to auction list			
			//if (record.Auction == null) {
			//  Web.ErrorMessage = "Error loading the comment. The Auction has probably been deleted.";
			//  return Redirect("~/Admin/CommentAdmin/Moderate");
			//}
			
			return View("CommentEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Comment/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Comment");
			var record = Models.Comment.LoadID(id);
			return View("CommentView", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Comment/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Comment.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Comment record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					var returnUrl = Save(record, record.IsNewRecord);
					TempData["info"] = "Comment "+record.GetName()+" saved.";
					if(returnUrl.IsNotBlank()) {
						return Redirect(returnUrl);
					}
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("CommentEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Comment();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				TempData["info"] = "Copy of Comment "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Comment record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private string Save(Models.Comment record, bool isNew) {
			bool moveOnToNextItem = false;
			
			// send emails (only if DeclineReason has been changed):
			if(record.Fields.DeclineReason.IsDirty && (record.Status == Comment.CommentStatus.Approved.ToString() || record.Status == Comment.CommentStatus.Declined.ToString())) {
				
				if(record.Status == Comment.CommentStatus.Approved.ToString()){
					// mark as Approved
					record.ApprovedByPersonID = Security.LoggedInUserID;
					record.ApprovedDate = DateTime.Now;
				}
				
				// user email:
				//Beweb.SiteCustom.Emails.CommentApproveDeclineToPerson(record);
				moveOnToNextItem = true;
			}

			// add any code to update other fields/tables here
			record.Save();
			//record.RebuildCachedComments();
			// save subform or related checkboxes here eg record.Lines.Save();

			// regardless of what button they clicked - move on to the next item if they declined or approved something
			if(moveOnToNextItem) {
				// find the next item
				var nextCommentId = BewebData.GetValue(new Sql("SELECT TOP(1) CommentId FROM Comment WHERE Status=", Comment.CommentStatus.Submitted.ToString().SqlizeText()
					,"AND CommentID < ", record.CommentID, " ORDER BY CommentID DESC"));

				if(nextCommentId.IsBlank()) {
					nextCommentId = BewebData.GetValue(new Sql("SELECT TOP(1) CommentId FROM Comment WHERE Status=", Comment.CommentStatus.Submitted.ToString().SqlizeText()
						,"AND CommentID < ", record.CommentID, " ORDER BY CommentID ASC"));
				
					if(nextCommentId.IsBlank()) return "~/Admin/CommentAdmin/Moderate";
				}
				return "~/Admin/CommentAdmin/Edit/" + nextCommentId;
			}
			return "";
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Comment/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Comment.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("CommentEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
