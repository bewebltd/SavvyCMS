// Admin ModificationLog Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.MobileControls;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers
{
	public class ModificationLogAdminController : AdminBaseController {
		//
		// GET: /Admin/ModificationLog/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Modification Log List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ModificationLogList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ModificationLog> {


			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ModificationLog where 1=1");
				if (SearchText != "") {
					//sql.Add("and (1=0");    // custom sql
					//sql.Add("or ([--Field--] like ", SearchText.SqlizeLike(), ")");    // custom sql
					//sql.Add(")");    // custom sql
					//sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);  // search more than one field
					sql.AddKeywordSearch(SearchText, new Models.ModificationLog().GetNameField().Name, true);  // just search by name
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");

				var weekEnding = WeekBeginning.AddDays(7);
				sql.Add("and UpdateDate >= ", WeekBeginning.SqlizeDate(), "and UpdateDate < ", weekEnding.SqlizeDate());
				if (SelectedTable != "(all)") {
					sql.Add("and TableName = ", SelectedTable.SqlizeText());
				}
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ModificationLog().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

			public Sql TableFilterSql {
				get { return new Sql("Select Distinct TableName from ModificationLog order by TableName"); }
			}

			public string SelectedTable {
				get { return (Web.Request["TableFilter"].IsNotBlank()) ? Web.Request["TableFilter"] + "" : "(all)"; }
			}

			private List<String> dateList;

			public  List<String> DateList {
				get {
					if (dateList == null) {
						//dateList = "";//new List<DateTime>();
						dateList = new List<String>();
						// go back 12 weeks
						DateTime weekBeginning = Dates.GetWeekBegin(DateTime.Now);
						for (var i=0;i<12;i++) {
							dateList.Add(Fmt.Date(weekBeginning.AddDays(-7*i)));
							/*if (i > 0) {
								dateList += ", " + Fmt.Date(weekBegining.AddDays(-7*i));
							} else {
								dateList = Fmt.Date(weekBegining.AddDays(-7*i));
							}
							*/
						}
					}
					return dateList;
				}
			}

			public DateTime WeekBeginning {
				get {
					return (Web.Request["WeekBeginning"].IsNotBlank()) ? Convert.ToDateTime(Web.Request["WeekBeginning"]) : Dates.GetWeekBegin(DateTime.Now);;
				}
			}

		}

		/*

		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ModificationLog/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Modification Log");
			var record = new Models.ModificationLog();
			// any default values can be set here or in partial class ModificationLog.InitDefaults() 
			return View("ModificationLogEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ModificationLog/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ModificationLog());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ModificationLog/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Modification Log");
			var record = Models.ModificationLog.LoadID(id);
			CheckLock(record);
			return View("ModificationLogEdit", record);
		}
		
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/ModificationLog/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Modification Log");
			var record = Models.ModificationLog.LoadID(id);
			return View("ModificationLogView", record);
		}
			/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/ModificationLog/Export/5
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.ModificationLog.LoadID(id);
			Web.SetHeadersForExcel("detail "+record.GetName()+".xls");
			return View("ModificationLogExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ModificationLog/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ModificationLog.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ModificationLog record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				//ifsubform: record.example.UpdateFromRequest();

				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Modification Log "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("ModificationLogEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ModificationLog();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Modification Log "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ModificationLog record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ModificationLog record, bool isNew) {
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
			var record = Models.TextBlock.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(),record.ID);
			return Redirect(returnPage);
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ModificationLog/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ModificationLog.LoadID(id);
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
			return Redirect(returnPage);
		}
		*/
	}
}
