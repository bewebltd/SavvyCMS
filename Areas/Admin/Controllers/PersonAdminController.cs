// Admin Person Controller
using System;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class PersonAdminController : AdminBaseController {
		//
		// GET: /Admin/Person/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Person List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.ShowRole=false;
			dataList.PageLoad();
			return View("PersonList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.Person> {
			public bool ShowRole;
			public ListHelper() {
				ShowExport = false;
			}
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM Person where 1=1");
				if (Web.Request["admin"].ToBool()) {
					if (!Security.IsDevAccess) {
						sql.Add("and role not like", SecurityRolesCore.Roles.DEVELOPER.SqlizeLike());
					}
					sql.Add("and (role like", SecurityRolesCore.Roles.ADMINISTRATOR.SqlizeLike());
					sql.Add("or role like", SecurityRolesCore.Roles.SUPERADMIN.SqlizeLike(),")");
					ShowRole = true;
				//}else { // only show staff
				//	sql.Add("and role not like", SecurityRolesCore.Roles.DEVELOPER.SqlizeLike());
				//	sql.Add("and role like ", SecurityRoles.Roles.STAFF.SqlizeLike());
				}
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, "FirstName,LastName,Email", true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.Person().GetDefaultOrderBy());
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/Person/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Person");
			var record = new Models.Person();
			// any default values can be set here or in partial class Person.InitDefaults() 
			return View("PersonEdit", record);
		}
		[HttpPost]
		public ActionResult GeneratePassword() {
			return Content(RandomPassword.Generate(7, 12));
		}

		[HttpPost]
		public ActionResult CheckStrengthPassword(string pw) {
			
			return Json(Security.CheckStrength(pw), JsonRequestBehavior.AllowGet);
		}


		public ActionResult ConvertAllPlainTextPasswordsToHashed() {
			return Content("Converted: " + Security.ConvertAllPlainTextPasswordsToHashed());
		}


		/// <summary>
		/// Saves a new record
		/// POST: /Admin/Person/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.Person());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/Person/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Person");
			var record = Models.Person.LoadID(id);
			//record.Init();
			return View("PersonEdit", record);
		}

		/// <summary>
		/// Loads existing record and shows view form, with cancel button
		/// GET: /Admin/Person/View/5
		/// </summary>
		public ActionResult View(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "View Person");
			var record = Models.Person.LoadID(id);
			return View("PersonView", record);
		}
			/// <summary>
		/// Loads existing record and shows export form, with cancel button
		/// GET: /Admin/Person/Export/5?mode=export
		/// </summary>
		public ActionResult Export(int id) {
			var record = Models.Person.LoadID(id);
			Web.SetHeadersForExcel("Detail "+record.GetName()+".xls");
			return View("PersonExport", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/Person/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.Person.LoadID(id));
		}


		private string formatEMailTxt(string textTofrmt) {
			String emailBodyTxt = textTofrmt;
			emailBodyTxt = Server.HtmlEncode(emailBodyTxt);
			emailBodyTxt = emailBodyTxt.Replace(" ", "&nbsp;");
			emailBodyTxt = emailBodyTxt.Replace("\r\n", "<br>");
			return emailBodyTxt;
		}



		protected void ProcessEmail(Models.Person record) {

			var body = Request["EmailCopy"];
			body = body.Replace("[firstname]", record.FirstName);
			body = body.Replace("[username]", record.Email);
			body = body.Replace("[password]", (String.IsNullOrEmpty(Crypto.Decrypt(record.Password)) ? record.Password : Crypto.Decrypt(record.Password)));
			body = body.Replace("[baseurl]", Web.BaseUrl);

			string emailResult = SendEMail.SimpleSendHtmlEmail(record.Email, Request["EmailSubject"], formatEMailTxt(body));

			//SendEMail em = new SendEMail()
			if (emailResult == null) {
				TempData["emailMsg"] = " Notification Email sent to " + record.Email + ".";
				//record.AdminOnlyNotes+="\nEmail sent on "+DateTime.Now.FmtDateTime();
			} else {
				TempData["emailMsg"] = " Notification Email failed to send to " + record.Email + ".<!--[" + emailResult + "]-->";//
				//record.AdminOnlyNotes+="\nEmail send failed on "+DateTime.Now.FmtDateTime();
			}
		}

		protected ActionResult ProcessForm(Models.Person record) {
			try {
				Web.InfoMessage = null;
				record.UpdateFromRequest();

				if (record.Password.IsBlank()) {
					record.Password = Beweb.RandomPassword.Generate(8);
				}
			
				Validate(record);

				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);

					if (Web.Request["sendemail"] != null) {
						ProcessEmail(record);
					}

					Web.InfoMessage = "Person " + record.GetName() + " saved." + TempData["emailMsg"];
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("PersonEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.Person();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Person " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.Person record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");

			if (record.Email.IsBlank()) {
				ModelState.AddModelError("Record", "Email address cannot be blank.");
			}
			/*
			string EmailAgain = Request["emailConfirmed"];
			if (!EmailAgain.Equals(record.Email)) {
				ModelState.AddModelError("Record", "Emails do not match, please re enter.");
			}
			*/

		}

		private void Save(Models.Person record, bool isNew) {
			// add any code to update other fields/tables here
			if (Request["pwValue"] + "" != "") {
				record.Password = Security.CreateSecuredPassword(Request["pwValue"]);
			} 
			if (record.Password.IsBlank()) {
				// Person MUST have a password
				record.Password =  Beweb.RandomPassword.Generate(8);;
			}
			record.Save();
			PersonCache.Rebuild();
		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/Person/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.Person.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("PersonEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			PersonCache.Rebuild();
			return Redirect(returnPage);
		}

	}
}
