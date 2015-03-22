// Admin GenTestHasCat Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class GenTestHasCatAdminController : AdminBaseController {
		//
		// GET: /Admin/GenTestHasCat/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Gen Test Has Cat List");
			Util.SetReturnPage(2);
			var dataList = new ListViewModel();
			dataList.PageLoad();
			return View("GenTestHasCatList", dataList);
		}

		public class ListViewModel : SavvyDataList<Models.GenTestHasCat> {
			public ListViewModel() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM GenTestHasCat where 1=1");
				if (SearchText != "") {
					//sql.Add("and (1=0");    // custom sql
					//sql.Add("or ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.Add(")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.GenTestHasCat().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.GenTestHasCat().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/GenTestHasCat/Create
		/// </summary>
		public ActionResult Create() {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Gen Test Has Cat");
			var record = new Models.GenTestHasCat();
			// any default values can be set here or in partial class GenTestHasCat.InitDefaults() 
			data.GenTestHasCat = record;
			return View("GenTestHasCatEdit", data);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/GenTestHasCat/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.GenTestHasCat());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/GenTestHasCat/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			var data = new EditViewModel();
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Gen Test Has Cat");
			var record = Models.GenTestHasCat.LoadID(id);
			if(record == null)  {
				Web.ErrorMessage = "Gen Test Has Cat not found (ID: "+id+")";
				return Redirect("~/Admin/GenTestHasCatAdmin");
			}
			CheckLock(record);
			data.GenTestHasCat = record;
			return View("GenTestHasCatEdit", data);
		}

		public class EditViewModel {
			public Models.GenTestHasCat GenTestHasCat;
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/GenTestHasCat/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Gen Test Has Cat");
			var record = Models.GenTestHasCat.LoadID(id);
			return View("GenTestHasCatView", record);
		}
			/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/GenTestHasCat/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.GenTestHasCat.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("GenTestHasCatExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/GenTestHasCat/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.GenTestHasCat.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.GenTestHasCat record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Gen Test Has Cat "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("GenTestHasCatEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.GenTestHasCat();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Gen Test Has Cat "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.GenTestHasCat record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.GenTestHasCat record, bool isNew) {
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
			var record = Models.GenTestHasCat.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/GenTestHasCat/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.GenTestHasCat.LoadID(id);
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
