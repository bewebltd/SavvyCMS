// Admin Blog Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class BlogAdminController : AdminBaseController {
		//
		// GET: /Admin/Blog/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Blog List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("BlogList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.Blog> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Blog where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.Blog().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.Blog().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Blog/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Blog");
			var record = new Models.Blog();
			// any default values can be set here or in partial class Blog.InitDefaults() 
			return View("BlogEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Blog/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Blog());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Blog/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Blog");
			var record = Models.Blog.LoadID(id);
			return View("BlogEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Blog/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Blog");
			var record = Models.Blog.LoadID(id);
			return View("BlogView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Blog/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Blog.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Blog record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Blog "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("BlogEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Blog();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Blog "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Blog record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Blog record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Blog/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Blog.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("BlogEdit", record);
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
