using System;
using System.Web.Mvc;
using Beweb;
using Models;

namespace Site.Areas.Admin.Controllers {
	/// <summary>
	/// Contains partials for rendering common page elements such as individual panels that are used on several pages
	/// </summary>
	public class CommonAdminController : AdminBaseController {
	
		public ActionResult SEOEditPanel(ActiveRecord record, bool showHeader, string cssTablerowClass) {
			if (!record.FieldExists("PageTitleTag")) {
				return null;
			}

			CheckForNoShowHeader(record, ref showHeader);

			var data = new SEOViewData();
			data.DataRecord = record;
			data.ShowHeader = showHeader;
			data.CssTablerowClass = cssTablerowClass;
			// AF20140926: Invoke GetFullUrl method by reflection to get the overriden method inside each model rather than the ActiveRecord generic one
			data.Url = record.GetType().GetMethod("GetFullUrl", new Type[] { }).Invoke(record, new object[] { }).ToString();
			return View(data);
		}

		public class SEOViewData {
			public ActiveRecord DataRecord;
			public bool ShowHeader=false;
			public string Url;
			public string CssTablerowClass { get; set; }
			public string Title {
				get {
					string result = "";
					try {
						result = DataRecord["Title"].HtmlEncode();

					} catch (Exception) {
					}
					return result;
				}
			}

			public string GetValue(ActiveRecord record, string fieldName) {
				string result = "";
				try {
					result = record[fieldName]+"";

				} catch (Exception) {
				}
				return result;
			}
		}

		public ActionResult PublishSettingsEditPanel(ActiveRecord record, bool showHeader, string cssTablerowClass) {
			CheckForNoShowHeader(record, ref showHeader);

			return View(new PublishSettingsViewData() { DataRecord = record, ShowHeader = showHeader, CssTablerowClass = cssTablerowClass });
		}

		private static void CheckForNoShowHeader(ActiveRecord record, ref bool showHeader) {
			if (record.Advanced.ExpiryDateField == null && record.Advanced.PublishDateField == null &&
			    record.Advanced.IsActiveFields.Count == 0) showHeader = false;
		}

		public class PublishSettingsViewData {
			public ActiveRecord DataRecord;
			public bool ShowHeader;
			public string CssTablerowClass { get; set; }
		}

		public ActionResult ModificationHistoryPanel(ActiveRecord record, bool showHeader) {
			CheckForNoShowHeader(record, ref showHeader);
			return View(new ModificationHistoryViewData() { DataRecord = record, ShowHeader = showHeader });
		}

		public class ModificationHistoryViewData {
			public ActiveRecord DataRecord;
			public bool ShowHeader;
		}

		public ActionResult MapLocationEditPanel(ActiveRecord record, string latitudeFieldName, string longitudeFieldName, string addressFieldNamesPipeSeparated) {
			return View(new MapLocationViewData(){DataRecord=record, LatitudeFieldName = latitudeFieldName,LongitudeFieldName = longitudeFieldName,AddressFieldName = addressFieldNamesPipeSeparated});
		}

		public class MapLocationViewData {
			public ActiveRecord DataRecord;
			public string LatitudeFieldName;
			public string LongitudeFieldName;
			public string AddressFieldName;
		}


	}

}
