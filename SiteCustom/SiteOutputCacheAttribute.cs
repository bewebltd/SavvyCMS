using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;

namespace Site.SiteCustom {

	public class SiteOutputCacheAttribute : SavvyMVC.ActionOutputCacheAttribute {
		public SiteOutputCacheAttribute()
			: base(60) {
		}
		 
		public SiteOutputCacheAttribute(int cacheDuration)
			: base(cacheDuration) {
		}

		protected override string ReplaceDonutHoles(string cachedOutput) {
			//example: if (Security.IsLoggedIn) return cachedOutput.Replace("{CacheReplaceFirstName}", UserSession.Person.FirstName);
			return cachedOutput;
		}

		protected override string CreateDonutHoles(string cachedOutput) {
			//example: if (Security.IsLoggedIn) return cachedOutput.Replace("<span id=\"CacheReplaceFirstName\">" + UserSession.Person.FirstName + "</span>", "{CacheReplaceFirstName}");
			return cachedOutput;
		}

	}
}
