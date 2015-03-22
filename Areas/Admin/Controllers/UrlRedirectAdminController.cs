// Admin UrlRedirect Controller
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Beweb;
using Models;
using Savvy;
using SavvyMVC.Util;

namespace Site.Areas.Admin.Controllers
{
	public class UrlRedirectAdminController : AdminBaseController {
		//
		// GET: /Admin/UrlRedirect/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Url Redirect List");
			Util.SetReturnPage(2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("UrlRedirectList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.UrlRedirect> {
			public override Sql GetSql() {
				Sql sql = new Sql("SELECT * FROM UrlRedirect where 1=1");
				if (SearchText != "") {
					//sql.Add(" and ([--Field--] like ", SearchText.SqlizeLike() ")");
					sql.AddKeywordSearch(SearchText, new Models.UrlRedirect().GetNameField().Name, true);
				}
				// handle an fk (rename fkid, then uncomment)
				//sql.Add("and landingPageID=", landingPageID);
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					sql.AddRawSqlString(new Models.UrlRedirect().GetDefaultOrderBy());   
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/UrlRedirect/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(3, "Add Url Redirect");
			var record = new Models.UrlRedirect();
			// any default values can be set here or in partial class UrlRedirect.InitDefaults() 
			return View("UrlRedirectEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/UrlRedirect/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.UrlRedirect());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/UrlRedirect/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit UrlRedirect");
			var record = Models.UrlRedirect.LoadID(id);
			return View("UrlRedirectEdit", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/UrlRedirect/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.UrlRedirect.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.UrlRedirect record) {
			try {
				record.UpdateFromRequest();

				if (record.RedirectFromUrl.EndsWith("/")) {
					record.RedirectFromUrl = record.RedirectFromUrl.RemoveCharsFromEnd(1); //remove traling slash JC 20140423
				}
				
				if(record.RedirectToUrl == null) {
					record.RedirectToUrl = "";
				}


				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage = "Url Redirect "+record.GetName()+" saved.";
				}
			} catch (Exception e) {
				ModelState.AddModelError("Record", e.Message);
			}
			
			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("UrlRedirectEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.UrlRedirect();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage = "Copy of Url Redirect "+record.GetName()+" created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.UrlRedirect record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.UrlRedirect record, bool isNew) {
			// add any code to update other fields/tables here
			record.Save();

			// add redirects to Route table
			//var app = HttpContext.ApplicationInstance as MvcApplication;
			//var routes = RouteTable.Routes;
			//routes.RemoveRouteByName("urlRedirect"+record.ID);
			//var route = new Route(record.RedirectFromUrl, new RouteValueDictionary(){ controller = "UrlRedirect", action = "Index", id = record.ID }, MvcUtil.SiteControllerNamespaceArray);
			//routes.InsertRouteAfter("urlRedirectPlaceholder", );
			//routes.MapRoute("urlRedirect"+record.ID, );
		}

		/// <summary> 
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/UrlRedirect/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.UrlRedirect.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("UrlRedirectEdit");
			}
			//record.Deals.DeleteAll();
			record.Delete();
			return Redirect(returnPage);
		}

	}
}
