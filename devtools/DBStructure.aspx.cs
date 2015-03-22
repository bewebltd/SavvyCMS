using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Beweb;

namespace Site.devtools {
	public partial class DBStructure : System.Web.UI.Page {
		
		public string ComparisonLeft = "";
		public string ComparisonRight = "";
		
		protected void Page_Load(object sender, EventArgs e) {
			if(!String.IsNullOrEmpty(Request["left"]) && !String.IsNullOrEmpty(Request["right"])) {
				ComparisonLeft = Http.Get(Util.GetNamedSetting("WebsiteBaseUrl" + Request["left"], "") + "admin/tools/Structure.aspx").Replace(Environment.NewLine, "<br>");
				ComparisonRight = Http.Get(Util.GetNamedSetting("WebsiteBaseUrl" + Request["right"], "") + "admin/tools/Structure.aspx").Replace(Environment.NewLine, "<br>");
			}
		}
	}
}