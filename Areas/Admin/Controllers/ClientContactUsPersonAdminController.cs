// Admin ClientContactUsPerson Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using SavvyMVC.Helpers;

namespace Site.Areas.Admin.Controllers
{
	public class ClientContactUsPersonAdminController : AdminBaseController {
		//
		// GET: /Admin/ClientContactUsPerson/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb((Web.Request["region"].IsBlank())?2:3, "Client Contact Us Person List"+(Web.Request["region"].IsNotBlank()?" for region "+Models.ClientContactUsRegion.LoadID(Web.Request["region"].ToInt()).RegionName:""));
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("ClientContactUsPersonList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.ClientContactUsPerson> {
			public ListHelper() {

			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM ClientContactUsPerson where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.ClientContactUsPerson().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				var reg = Web.Request["region"];
				if (reg.IsNotBlank()) {
					sql.Add("and ClientContactUsRegionID=", reg.SqlizeNumber());	
				}
				
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.ClientContactUsPerson().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/ClientContactUsPerson/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Client Contact Us Person");
			var record = new Models.ClientContactUsPerson();
			// any default values can be set here or in partial class ClientContactUsPerson.InitDefaults() 
			return View("ClientContactUsPersonEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/ClientContactUsPerson/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.ClientContactUsPerson());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/ClientContactUsPerson/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Client Contact Us Person");
			var record = Models.ClientContactUsPerson.LoadID(id);
			return View("ClientContactUsPersonEdit", record);
		}
		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/ClientContactUsPerson/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Client Contact Us Person");
			var record = Models.ClientContactUsPerson.LoadID(id);
			return View("ClientContactUsPersonView", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/ClientContactUsPerson/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.ClientContactUsPerson.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.ClientContactUsPerson record) {
			try {
				record.UpdateFromRequest();
				// read subform or related checkboxes here eg record.Lines.UpdateFromRequest();
				Validate(record);
				if (BewebModelState.IsValid()) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Client Contact Us Person "+record.GetName()+" saved.";
				}
			} catch (UserErrorException e) {
				BewebModelState.AddModelError("Record", e.Message);
			}
			
			if (!BewebModelState.IsValid()) {
				// invalid so redisplay form with validation message(s)
				return View("ClientContactUsPersonEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.ClientContactUsPerson();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Client Contact Us Person "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.ClientContactUsPerson record) {
			// add any code to check for validity
			//BewebModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.ClientContactUsPerson record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			// save subform or related checkboxes here eg record.Lines.Save();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/ClientContactUsPerson/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.ClientContactUsPerson.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("ClientContactUsPersonEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
