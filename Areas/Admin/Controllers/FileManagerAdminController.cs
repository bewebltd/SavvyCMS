using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Beweb;

namespace Site.Areas.Admin.Controllers {
	public class FileManagerAdminController : AdminBaseController {

		//
		// GET: /Admin/FileManager/

		[Authorize]
		public ActionResult Index() {
			var data = new ViewModel();
			return View("FileManager", data);
		}
		
		[Authorize]
		public ActionResult GetDirectories(string id = "/") {

			// If the path contains dot(s), replace with / because nobody is allowed to get up directories
			if(id.Contains(".")) {
				id = "/";
			}

			var exclusions = new[] { "logs" };

			var dirInfo = new DirectoryInfo(Web.MapPath(Web.Attachments + id));

			var directories = dirInfo.GetDirectories("*.*", SearchOption.TopDirectoryOnly).Where(dir => !exclusions.Contains(dir.Name.ToLower())).Select(dir => new FileManagerDirectory {
				id = dir.FullName.Replace("\\", "/").RightFrom("Attachments"), 
				name = dir.Name, 
				isParent = dir.GetDirectories().Any()
			}).ToList();

			return Content(new JavaScriptSerializer().Serialize(directories));
			//return Content("[{ id:'01',	name:'n1',	isParent:true},{ id:'02',	name:'n2',	isParent:true},{ id:'03',	name:'n3',	isParent:true},{ id:'04',	name:'n4', isParent:false}]");
		}

		//private bool Directory

		public class FileManagerDirectory {
			public string id;
			public string name;
			public bool isParent;
		}

		public class ViewModel {

		}



	}
}
