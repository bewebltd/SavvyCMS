// Admin Settings Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;

namespace Site.Areas.Admin.Controllers
{
	public class SettingsAdminController : AdminBaseController {
		//
		// GET: /Admin/Settings/


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Settings/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Settings");
			var record = new Models.Settings();
			// any default values can be set here or in partial class Settings.InitDefaults() 
			return View("SettingsEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Settings/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Settings());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Settings/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Settings");
			var record = Models.Settings.LoadID(id);
			return View("SettingsEdit", record);
		}


		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Settings/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Settings.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.Settings record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Settings saved.";
				}
			} catch (Exception e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("SettingsEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Settings record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.Settings record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();
			Settings.RebuildCache();
		}


	}
}
