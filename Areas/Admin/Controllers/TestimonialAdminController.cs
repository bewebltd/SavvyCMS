// Admin Testimonial Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class TestimonialAdminController : AdminBaseController {
		//
		// GET: /Admin/Testimonial/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Testimonial List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("TestimonialList", dataList);
		}
		public ActionResult SampleData() {
			var t = new Testimonial(){Author="Jeremy", AuthorRole="director,beweb", Comments = "This was great"};t.Save();
			t = new Testimonial(){Author="Mike", AuthorRole="director,beweb", Comments = "also, i thought it was great"};t.Save();
			return Index();
}
		public class ListHelper : SavvyDataList<Models.Testimonial> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Testimonial where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.Testimonial().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.Testimonial().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Testimonial/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Testimonial");
			var record = new Models.Testimonial();
			// any default values can be set here or in partial class Testimonial.InitDefaults() 
			return View("TestimonialEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Testimonial/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Testimonial());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Testimonial/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Testimonial");
			var record = Models.Testimonial.LoadID(id);
			return View("TestimonialEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Testimonial/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Testimonial");
			var record = Models.Testimonial.LoadID(id);
			return View("TestimonialView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Testimonial/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Testimonial.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Testimonial record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Testimonial "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("TestimonialEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Testimonial();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Testimonial "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Testimonial record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Testimonial record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Testimonial/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Testimonial.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("TestimonialEdit", record);
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
