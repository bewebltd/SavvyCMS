using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;

namespace Site.SiteCustom {
	public class TextBlockCache {
		public static void Check() {
			if (Web.Cache["TextBlockCache"] == null) {
				Rebuild();
			}
		}

		public static void Rebuild() {
			ActiveRecordLoader.ClearCache("TextBlock");
			Web.Cache["TextBlockCache"] = TextBlockList.LoadAll();
		}

		public static TextBlockList All {
			get { Check(); return Web.Cache["TextBlockCache"] as TextBlockList; }
		}

		public static Models.TextBlock GetRich(string sectionCode, string defaultTextHtml=null, string defaultTitle = null) {
			CheckForDrop(sectionCode);
			var tb = All.Filter(textBlock => textBlock.SectionCode == sectionCode).FirstOrDefault();
			if (tb == null) {
				// add it to the database
				tb = new Models.TextBlock();
				tb.SectionCode = sectionCode;
				if (defaultTitle != null) {
					tb.IsTitleAvailable = true;
					tb.Title = defaultTitle;
				} else {
					tb.IsTitleAvailable = false;
				}
				tb.IsBodyPlainText = false;
				tb.IsUrlAvailable = false;
				tb.IsPictureAvailable = false;
				tb.BodyTextHtml = defaultTextHtml ?? "<b>"+Fmt.SplitTitleCase(sectionCode) + "</b> content goes here";
				tb.Save();
				All.Add(tb);
			}
			return tb;
		}

		private static void CheckForDrop(string sectionName) {
			if (Web.Request["droptextblocks"] != null && Util.IsBewebOffice || Web.Request["clearcache"] != null) {
				new Sql("delete from TextBlock where SectionCode=", sectionName.Sqlize_Text(), "").Execute();
				Rebuild();
			}
		}

		public static Models.TextBlock GetPlain(string sectionCode, string defaultTextHtml = null, string defaultTitle = null) {
			CheckForDrop(sectionCode);
			var tb = All.Filter(textBlock => textBlock.SectionCode == sectionCode).FirstOrDefault();
			if (tb == null) {
				// add it to the database
				tb = new Models.TextBlock();
				tb.SectionCode = sectionCode;
				if (defaultTitle != null) {
					tb.IsTitleAvailable = true;
					tb.Title = defaultTitle;
				} else {
					tb.IsTitleAvailable = false;
				}
				tb.IsBodyPlainText = true;
				tb.IsUrlAvailable = false;
				tb.IsPictureAvailable = false;
				tb.BodyTextHtml = defaultTextHtml ?? Fmt.SplitTitleCase(sectionCode) + " content goes here";
				tb.Save();
				All.Add(tb);
			}
			return tb;
		}


	}
}