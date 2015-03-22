using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;

namespace Site.SiteCustom {
	public class PageCache {
		public static void Check() {
			if (Web.Cache["PageCache"]==null) {
				Rebuild();
			}
		}

		public static void Rebuild() {
			Web.ClearOutputCache();
			ActiveRecordLoader.ClearCache("Page");
			Web.Cache["PageCache"] = PageList.LoadAll();
		}

		public static PageList All {
			get { Check(); return Web.Cache["PageCache"] as PageList; }
		}

		public static ActiveRecordList<Page> Active {
			get { return All.Filter(page=>page.GetIsActive()); }
		}

		public static Page Home {
			get { return All.FirstOrDefault(page => page.PageCode == "Home"); }
		}
		public static ActiveRecordList<Page> MainNav {
			get { return All.Filter(page=>page.GetIsActive() && page.ShowInMainNav && page.ParentPageID==null).OrderBy(page=>page.SortPosition).ToList(); }
		}

		public static Models.Page GetByPageCode(string pageCode) {
#if PageRevisions
			return All.Filter(page=>page.PageCode==pageCode && page.RevisionStatus == "Live").FirstOrNew(); 
#else
			return All.Filter(page=>page.PageCode==pageCode).FirstOrNew(); 
#endif
		}


	}
}