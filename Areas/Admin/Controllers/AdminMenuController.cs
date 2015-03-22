using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Savvy;
using Site.SiteCustom;

namespace Site.Areas.Admin.Controllers {
	public class AdminMenuController : AdminBaseController {
		//
		// GET: /Admin/AdminMenu/


		public ActionResult Index() {
			if (Util.GetSettingBool("AdminBreadcrumbIncludeHome", true)){
				Breadcrumbs.Current.SetBreadcrumb(0, "Home", Web.Root);
			}
			Breadcrumbs.Current.AddBreadcrumb(1, "Admin Menu");
			Security.ResetLoginLoopChecker();

			var data = new ViewModel();
			//data.UserName = Session[Beweb.Util.GetSiteCodeName() + "_AdminFirstName"] + " " + Session[Beweb.Util.GetSiteCodeName() + "_AdminLastName"];
			Person person = Models.Person.LoadID(Security.LoggedInUserID);
			if(person==null) {
				return Redirect(Web.Root + "security/login?msg=dbusermissing");;
			}
			data.LoggedInPerson = person;
			data.UserName = person.FirstName + " " + person.LastName;
			data.IsDevAccess = Beweb.Util.IsDevAccess();
			data.LastLogin = person.LastLoginDate.FmtDateTime();
			//EmailToAddress.Text = ConfigurationManager.AppSettings["EmailToAddress"];
			data.Role = person.Role;
			SqlConnection sc = new SqlConnection(BewebData.GetConnectionString());
			//ConnectionStringDetails.Text = String.Format("Datasource: {1}, Database: {0}", sc.DataSource, sc.Database);
			data.ConnectionStringDetails = String.Format("Database: {1}, Server: {0}", sc.DataSource, sc.Database);

			// MN 20130214 - removed SavvyAdmin default add, no longer needed and always sets default colour to white!

			return View("AdminMenu", data);
		}

		public class ViewModel {
			public Person LoggedInPerson { get; set; }
			public string LastLogin { get; set; }
			public string UserName { get; set; }
			public string Role { get; set; }
			public bool IsDevAccess { get; set; }
			public string ConnectionStringDetails { get; set; }
		}

		public ActionResult RedirectToMenu() {
			return RedirectToAction("Index");
		}

		public ActionResult NotFound() {
			Response.StatusCode = 404;
			return Index();
		}

		public ActionResult ShowError() {
			Response.StatusCode = 500;
			return Index();
		}



		public void DeleteUnusedAttachments() {
			bool verbose = true;
			var files = Directory.GetFiles(Web.MapPath(Web.Attachments), "*.*", SearchOption.AllDirectories);

			Web.Flush("Indexing files...<br>");
			var attachments = new List<string>();
			foreach (var file in files) {
				var attachmentName = file.RightFrom(Web.MapPath(Web.Attachments)).ToLower();
				if (!attachmentName.Contains("todelete\\")) {
					attachments.Add(attachmentName.Replace("\\", "/"));
				}
			}
			Web.Flush("Total " + attachments.Count + " files...<br>");

			foreach (var tableName in BewebData.GetTableNames()) {
				Web.Flush("Table: " + tableName + "<br>");
				var sql = new Sql("select top 1 * from ", tableName.SqlizeName());
				var fields = new DelimitedString(",");
				using (var reader = sql.GetReader()) {
					if (verbose) Web.Flush("Checking structure...<br>");
					int rec = 0;
					if (reader.HasRows) {
						int visibleFieldCount = reader.VisibleFieldCount;
						for (int i = 0; i < visibleFieldCount; i++) {	// for each column
							string dataType = reader.GetDataTypeName(i).ToLower();
							string fieldName = reader.GetName(i).ToLower();
							bool isAttach = (dataType.Contains("varchar") && (fieldName.Contains("attachment") || fieldName.Contains("picture")));
							bool isRich = ((dataType.Contains("varchar") || dataType.Contains("text")) && (fieldName.Contains("html") || fieldName.Contains("body") || fieldName.Contains("text")));
							if (isAttach || isRich) {
								fields += fieldName;
							}
						}
					}
				}
				if (fields.IsBlank) {
					Web.Flush("Skipping table as no relevant field names<br>");
				} else {
					sql = new Sql("select " + fields.ToString() + " from ", tableName.SqlizeName());
					Web.Flush("Searching table... " + sql.Value + "<br>");

					using (var reader = sql.GetReader()) {
						Web.Flush("Scanning records...<br>");
						int rec = 0;
						foreach (DbDataRecord record in reader) {
							rec++;
							var foundAttachments = new List<string>();
							int visibleFieldCount = reader.VisibleFieldCount;
							for (int i = 0; i < visibleFieldCount; i++) {	// for each column
								string fieldName = record.GetName(i).ToLower();
								bool isAttach = ((fieldName.Contains("attachment") || fieldName.Contains("picture")));
								if (!record.IsDBNull(i)) {
									string fieldValue = record.GetString(i);
									if (fieldValue.IsNotBlank()) {
										foreach (var attachmentName in attachments) {
											if (fieldValue == attachmentName || (!isAttach && fieldValue.ToLower().Contains(attachmentName))) {
												if (verbose) Web.WriteLine("&nbsp;&nbsp;Found: " + attachmentName + " in " + tableName);
												foundAttachments.Add(attachmentName);
											}
										}
									}
									attachments.RemoveAll(a => foundAttachments.Contains(a));
								}

							}
							if (rec % 100 == 0) {
								Web.Flush("Scanned: " + rec + " records<br>");
							}
						}
					}
				}

			}

			Web.Flush("Finished checking. Located " + attachments.Count + " unused attachments<br>");

			int totalSize = 0;
			int cnt = 0;
			foreach (var attachmentName in attachments) {
				var size = FileSystem.GetFileSizeBytes(Web.Attachments + attachmentName);
				totalSize += size;
				Web.WriteLine("Not found: " + attachmentName + " " + Fmt.FileSize(size, 2));
				if (Request["doit"]=="1") FileSystem.Move(Web.Attachments + attachmentName, Web.Attachments + "todelete", false);
				cnt++;
				if (cnt % 100 == 0) {
					Web.Flush("Archived: " + cnt + " files<br>");
				}
			}
			Web.WriteLine("Total size: " + Fmt.FileSize(totalSize, 2));

			//DirectoryInfo di = new DirectoryInfo(Server.MapPath(Web.Attachments+"trademe/"));

			////read all files into a list
			//var files = new List<string>();
			//foreach (var file in di.EnumerateFiles()) {
			//  Web.Write(file.Name);
			//  files.Add(file.Name);

			//}
			////read db records, remove from files list if they exist in the database
			//var sql = new Sql("select * from trademelisting");
			//foreach (DbDataRecord row in sql.GetReader()) {
			//  //int pageid = (int)row["pageID"];	
			//  //C:\data\dev\web\Honda\PublicServices\attachments\trademe\HAS1001_1709195_9_tn.jpg
			//  //C:\data\dev\web\Honda\PublicServices\attachments\trademe\HAS1001_1709181_8.jpg
			//  //
			//  for(int scanIndex=0;scanIndex<20;scanIndex++){
			//    var name = row["DealerCode"]+"_"+row["ID"]+"_"+scanIndex;
			//    var nameThumb = name+"_tn.jpg";
			//    if(files.Contains(nameThumb))files.Remove(nameThumb);
			//    name = name+".jpg";
			//    if(files.Contains(name))files.Remove(name);

			//  }
			//}	
			////delete all files remaining in the list
			//foreach(var file in files) {
			//  string filename = Server.MapPath(Web.Attachments+"trademe/")+file;
			//  FileSystem.Delete(filename);
			//}


			Web.InfoMessage = "Deleted Unused Images and Attachment Files";
		}	



	}
}
