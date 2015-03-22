// Admin TextBlock Controller
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;
using TextBlock = Models.TextBlock;

namespace Site.Areas.Admin.Controllers {
	public class TextBlockAdminController : AdminBaseController {
		//
		// GET: /Admin/TextBlock/

		public ActionResult Index() {
			Breadcrumbs.Current.AddBreadcrumb(2, "Text Block List");
			Util.SetReturnPage((Request["TextBlockGroupID"].IsNotBlank()) ? 3 : 2);
			var dataList = new ListHelper();
			dataList.PageLoad();
			return View("TextBlockList", dataList);
		}

		public class ListHelper : SavvyDataList<Models.TextBlock> {
			public override Sql GetSql() {
				//Sql sql = new Sql("SELECT TextBlock.* FROM TextBlock  where 1=1"); //left join textblockpage on textblock.textblockpageid=textblockpage.textblockpageid
				Sql sql = new Sql("SELECT TextBlock.*, groupname FROM TextBlock left join textblockgroup on textblock.textblockgroupid=textblockgroup.textblockgroupid where 1=1"); //
				if (SearchText != "") {
					sql.Add(" and (sectioncode like ", SearchText.SqlizeLike(), " or title like ", SearchText.SqlizeLike(), " or bodytexthtml like ", SearchText.SqlizeLike(), ")");
					//sql.AddKeywordSearch(SearchText, new Models.TextBlock().GetNameField().Name, true);
				}
				//Logging.dout(sql);
				// handle an fk (rename fkid, then uncomment)
				if (Web.Request["TextBlockGroupID"].IsNotBlank()) {
					sql.Add("and TextBlockGroup.TextBlockGroupID=", Web.Request["TextBlockGroupID"].SqlizeNumber());
				}
				//sql.AddRawSqlString("and [extra raw sql here]");
				if (SortBy.IsBlank()) {
					// hint: to change the default order by for both admin and front end, override Destination.GetDefaultOrderBy in model partial 
					//sql.AddRawSqlString(new Models.TextBlock().GetDefaultOrderBy());   
					sql.Add(" order by SortPosition, GroupName, sectionCode");
				} else {
					sql.AddSql(GetOrderBySql());
				}
				return sql;
			}

		}


		/// <summary>
		/// Populates defaults and opens edit form to add new record
		/// GET: /Admin/TextBlock/Create
		/// </summary>
		public ActionResult Create() {
			Breadcrumbs.Current.AddBreadcrumb(Request["TextBlockGroupID"].IsNotBlank() ? 4 : 3, "Add Text Block");
			var record = new Models.TextBlock();
			if (Request["TextBlockPageID"].IsNotBlank()) record.TextBlockGroupID = Request["TextBlockGroupID"].ToString().ConvertToInt();
			// any default values can be set here or in partial class TextBlock.InitDefaults() 
			return View("TextBlockEdit", record);
		}

		/// <summary>
		/// Saves a new record
		/// POST: /Admin/TextBlock/Create
		/// </summary>
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return ProcessForm(new Models.TextBlock());
		}

		/// <summary>
		/// Loads existing record and shows edit form
		/// GET: /Admin/TextBlock/Edit/5
		/// </summary>
		public ActionResult Edit(int id) {
			Breadcrumbs.Current.AddBreadcrumb(3, "Edit Text Block");
			var record = Models.TextBlock.LoadID(id);
			CheckLock(record);
			return View("TextBlockEdit", record);
		}

		/// <summary>
		/// Saves an existing record
		/// POST: /Admin/TextBlock/Edit/5
		/// </summary>
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection) {
			return ProcessForm(Models.TextBlock.LoadID(id));
		}

		protected ActionResult ProcessForm(Models.TextBlock record) {
			try {
				record.UpdateFromRequest();
				Validate(record);
				if (ModelState.IsValid) {
					Save(record, record.IsNewRecord);
					Web.InfoMessage += "Text Block " + record.GetName() + " saved.";
				}
			} catch (UserErrorException e) {
				ModelState.AddModelError("Record", e.Message);
			}

			if (!ModelState.IsValid) {
				// invalid so redisplay form with validation message(s)
				return View("TextBlockEdit", record);
			} else if (Web.Request["SaveAndRefreshButton"] != null) {
				return RedirectToEdit(record.ID);
			} else if (Web.Request["DuplicateButton"] != null) {
				var newRecord = new Models.TextBlock();
				newRecord.UpdateFrom(record);
				newRecord.Save();
				Web.InfoMessage += "Copy of Text Block " + record.GetName() + " created. You are now editing the new copy.";
				return RedirectToEdit(newRecord.ID);
			} else {
				return RedirectToReturnPage();
			}
		}

		private void Validate(Models.TextBlock record) {
			// add any code to check for validity
			//ModelState.AddModelError("Record", "Suchandsuch cannot be whatever.");
		}

		private void Save(Models.TextBlock record, bool isNew) {
			// add any code to update other fields/tables here
			record.DateModified = DateTime.Now;
			record.Save();
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);

			TextBlockCache.Rebuild();
		}

		/// <summary>
		/// cancel out of a given record edit mode, and remove lock
		/// GET: /Admin/TextBlock/Cancel/5
		/// </summary>
		public override ActionResult Cancel(int id, string returnPage) {
			var record = Models.TextBlock.LoadID(id);
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			return Redirect(returnPage);

		}

		/// <summary>
		/// Deletes the given record or displays validation errors if cannot delete
		/// GET: /Admin/TextBlock/Delete/5
		/// </summary>
		public ActionResult Delete(int id, string returnPage) {
			var record = Models.TextBlock.LoadID(id);
			string issues = record.CheckForDependentRecords();
			if (issues != null) {
				Web.ErrorMessage = "Cannot delete this record. " + issues; return RedirectToEdit(record.ID);
				return View("TextBlockEdit");
			}
			CheckLock(record);
			lockobj.UnLockTable(record.GetTableName(), record.ID);
			//record.Deals.DeleteAll();
			record.Delete();
			TextBlockCache.Rebuild();
			return Redirect(returnPage);
		}


		public ActionResult SaveSortOrder(string sortOrder) {
			int pos = 1;
			List<int> ids = sortOrder.Split(",").ToList(s => s.ToInt(0));
			foreach (var id in ids) {
				Sql sql = new Sql("update TextBlock set SortPosition=", pos * 10, "where TextBlockID=", id);
				sql.Execute();
				pos++;
			}
			ActiveRecordLoader.ClearCache("TextBlock");
			return Content("Sort order saved.");
		}


	}
}
