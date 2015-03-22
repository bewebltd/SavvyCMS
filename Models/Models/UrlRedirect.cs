using System;
using Beweb;

namespace Models {
	public partial class UrlRedirect {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new UrlRedirect object.
		/// </summary>
		public new void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						IsActive = true;



		}
		//protected override void OnAfterLoadData()		{		}

		// You can put any business logic associated with this entity here
		public override string GetDefaultOrderBy() {
			return "order by UrlRedirectID";
		}
	}
}

// created: [ 06-Aug-2010 10:14:17pm ]