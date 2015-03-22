// Admin BlogComment Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class BlogCommentAdminController : AdminBaseController {
		//
		// GET: /Admin/BlogComment/

		public ActionResult Index(int ID) {
			var blog = Models.Blog.LoadByBlogID(ID);
			Breadcrumbs.Current.AddBreadcrumb(3, blog.Title+" Comments");
			Util.SetReturnPage(3);
			var dataList = new ListHelper();
			dataList.blogid=ID;
			dataList.PageLoad();
			return View("BlogCommentList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.BlogComment> {
			public int blogid=0;
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM BlogComment where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.BlogComment().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				sql.Add("and blogid=", blogid);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.BlogComment().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/BlogComment/Create
		/// </summary>
		public ActionResult Create(int ID) {
			Breadcrumbs.Current.AddBreadcrumb(4, "Add Blog Comment");
			var record = new Models.BlogComment();
			record.BlogID = ID;
			// any default values can be set here or in partial class BlogComment.InitDefaults() 
			return View("BlogCommentEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/BlogComment/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.BlogComment());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/BlogComment/Edit/5
		/// </summary>
		public ActionResult EditEnc(string encID) {
			int id = Crypto.DecryptID(encID);
			return Edit(id);
		}
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(4, "Edit Blog Comment");
			var record = Models.BlogComment.LoadID(id);
			return View("BlogCommentEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/BlogComment/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(4, "View Blog Comment");
			var record = Models.BlogComment.LoadID(id);
			return View("BlogCommentView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/BlogComment/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.BlogComment.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.BlogComment record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Blog Comment "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("BlogCommentEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.BlogComment();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Blog Comment "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.BlogComment record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.BlogComment record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/BlogComment/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.BlogComment.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("BlogCommentEdit", record);
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
