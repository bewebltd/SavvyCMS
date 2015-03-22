using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using Beweb;

namespace Site.devtools {
	public partial class Vacuum : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public static List<FileInfo> GetAttachmentsList() {
			var excludedExtensions = new[] {".config",".txt"};
			var directory = new DirectoryInfo(Web.MapPath(Web.Attachments));
			var files = directory.GetFiles("*.*", SearchOption.AllDirectories).Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && !excludedExtensions.Contains(f.Extension)).OrderByDescending(f => f.Length);
			return files.ToList();
		}

		public static string ConvertFilePathToURL(string path) {
			return path.Replace(HttpRuntime.AppDomainAppPath, Web.BaseUrl);
		}

	}
}