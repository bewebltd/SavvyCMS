using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SavvyMVC {
	public class SavvyBaseViewData {
		public string PageTitleTag { get; set; }
		public virtual string MetaDescription { get; set; }
		public string MetaKeywords { get; set; }
	}
}
