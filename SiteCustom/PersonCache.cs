using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;
using Models;

namespace Site.SiteCustom {
	public class PersonCache {
		public static void Check() {
			if (Web.Cache["PersonCache"]==null) {
				Rebuild();
			}
		}

		public static void Rebuild() {
			ActiveRecordLoader.ClearCache("Staff");
			Web.Cache ["PersonCache"] = PersonList.LoadAll();
		}

		public static PersonList All {
			get { Check(); return Web.Cache["PersonCache"] as PersonList; }
		}

		public static ActiveRecordList<Person> Active {
			get { return All.Filter(person=>person.GetIsActive()); }
		}

		public static ActiveRecordList<Person> GetActiveStaff { 
			get { return All.Filter (person => person.GetIsActive () && person.Role.DoesntContain("developer")); }
		}

		public static string GetName(int? personID) {
			var pers = All.Find(p => p.ID == personID);
			if (pers != null) {
				return pers.FullName;
			}
			return null;
		}

	}
}