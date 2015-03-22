// Admin GenTestCat Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class GenTestCatAdminController : AdminBaseController {
		//
		// GET: /Admin/GenTestCat/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Gen Test Cat List");
			Util.SetReturnPage(2);
			var dataList = new ListViewModel();
			dataList.PageLoad();
			return View("GenTestCatList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.GenTestCat> {
			public ListViewModel() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM GenTestCat where 1=1");
				if (SearchText != "") {
					//sql.Add("and (1=0");    // custom sql
					//sql.Add("or ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.Add(")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.GenTestCat().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.GenTestCat().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/GenTestCat/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Gen Test Cat");
			var record = new Models.GenTestCat();
			// any default values can be set here or in partial class GenTestCat.InitDefaults() 
			data.GenTestCat = record;
			return View("GenTestCatEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/GenTestCat/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.GenTestCat());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/GenTestCat/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Gen Test Cat");
			var record = Models.GenTestCat.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Gen Test Cat not found (ID: "+id+")";
				return Redirect("~/Admin/GenTestCatAdmin");
			}
			CheckLock(record);
			data.GenTestCat = record;
			return View("GenTestCatEdit", data);
		}

		public class EditViewModel {
			public Models.GenTestCat GenTestCat;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/GenTestCat/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Gen Test Cat");
			var record = Models.GenTestCat.LoadID(id);
			return View("GenTestCatView", record);
		}
			/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/GenTestCat/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.GenTestCat.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("GenTestCatExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/GenTestCat/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.GenTestCat.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.GenTestCat record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Gen Test Cat "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("GenTestCatEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.GenTestCat();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Gen Test Cat "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.GenTestCat record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.GenTestCat record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
			//ifsubform: record.example.Save();
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.GenTestCat.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/GenTestCat/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.GenTestCat.LoadID(id);
			// first delete any child records that are OK to delete
			//ifsubform: record.example.DeleteAll();
			// then prevent deletion if any other related records exist
			string issues = record.CheckForDependentRecords();
			if (issues.IsNotBlank()) {
				Web.ErrorMessage = "Cannot delete this record. " + issues;
				return RedirectToEdit(record.ID);
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			//ifsubform: record.example.Save();  // is this needed?
			record.Delete();
			Web.InfoMessage =  "Record deleted.";
			return Redirect(returnPage);
		}

	}
}
