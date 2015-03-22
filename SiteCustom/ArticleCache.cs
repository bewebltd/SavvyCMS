using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;

namespace Site.SiteCustom {
	public class ArticleCache {
		public static void Check() {
			if (Web.Cache["ArticleCache"]==null) {
				Rebuild();
			}
		}

		public static void Rebuild() {
			ActiveRecordLoader.ClearCache("Article");
			Web.Cache ["ArticleCache"] = ArticleList.LoadAll();
		}

		public static ArticleList All {
			get { Check(); return Web.Cache["ArticleCache"] as ArticleList; }
		}

		public static ActiveRecordList<Article> Active {
			get { return All.Filter(a=>a.GetIsActive()); }
		}


	}
}