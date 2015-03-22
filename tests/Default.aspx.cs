using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Beweb;
using Site.SiteCustom;

public partial class tests_Default : System.Web.UI.Page {
	protected List<Type> testClasses;
	protected string currentClass = Web.Request["class"];
	protected string mode = Web.Request["mode"];
	protected SavvyConsoleCommandStatus CommandStatus = new SavvyConsoleCommandStatus();

	public class SavvyConsoleCommandStatus {
		public bool success;
		public string message;
		public string data;
	}

	protected void Page_Load(object sender, EventArgs e) {

		this.testClasses = Assert.GetAllTestClasses();
		//Session["temp"] = "temp";
		Response.Buffer = true;

		// AF20150310 This code is outside WriteResults method because we don't want to include any HTML, just return a JSON
		if ((Request["mode"] + "").ToLower() == "savvyconsole" && (Request["command"] + "").IsNotBlank()) {
			mode = Request["command"];
			WriteResults();
			Response.Clear(); // Clear the output buffer because "WriteResults" methods output "OK"
			Response.ContentType = "application/json";
			Web.Write(new JavaScriptSerializer().Serialize(CommandStatus));
			Web.End();
		}

	}

	private string OKToImport(string otherServerEnvironment) {
		/* This can expanded upon as we think of more ways to prevent a bad import */
		string response = "OK";
		var referer = Web.ServerVars["HTTP_REFERER"] + "";
		var host = Web.ServerVars["HTTP_HOST"] + "";
		string localConnectionString = BewebData.GetConnectionString("ConnectionString" + Util.ServerIs());
		string remoteConnectionString = BewebData.GetConnectionString("ConnectionString" + otherServerEnvironment);
		if (!referer.Contains(host)) {
			response = "<p><strong>Did not import.</strong></p><p>You cannot import data by directly navigating to this page.</p>";
		} else if (localConnectionString == remoteConnectionString) {
			response = "<p><strong>Did not import.</strong></p><p>You cannot do an import where both connection strings are the same.</p>";
		} else if (BewebData.TableExists("Settings")) {

			// TO DISCUSS, THERE ARE LOTS OF WAYS THIS COULD BECOME PROBLEMATIC.

			/*
			Settings settings = Settings.Load(new Sql("Select top 1 * from Settings"));
			if (settings.FieldExists("ServerEnvironment")) {
				if (settings["ServerEnvironment"].ToString() == otherServerEnvironment){
					response = "<p><strong>Did not import.</strong></p><p>The 'ServerEnvironment' setting is the same as the current server. Please make sure you are on the correct server, the 'ServerEnvironment' setting is correct and try again.</p>";
				}
			} else {
				new Sql("ALTER TABLE settings ADD ServerEnvironment nvarchar(3);").Execute();
				settings["ServerEnvironment"].ValueObject = Util.ServerIs();
				settings.Save();
				// setting doesnt exisit so dont assum its ok. Let the user try again
				response = "<p><strong>Did not import.</strong></p><p>The 'ServerEnvironment' setting was missing from the settings. This setting has been added and set to '" + Util.ServerIs() + "'. If this is incorrect please correct the setting and try again.</p>";
			}
			*/
		}
		return response;
	}

	protected void WriteResults() {
		string isOK;

		// Savvy Console JSON
		CommandStatus.success = true;

		if (mode == "GenModels") {
			Web.Write("<b>Generating Savvy Active Record Classes</b><br /><br />");
			ActiveRecordGenerator.Run();
			Web.Write("<br>Done.");
			CommandStatus.message = "Models updated successfully";
		} else if (mode == "AddDbFields") {
			Web.Write("<b>Creating any missing fields in Database from Savvy Active Record Classes</b><br /><br />");
			ActiveRecordDatabaseGenerator.CreateAnyMissingFields();
			Web.Write("<br>Done.");
			CommandStatus.message = "Database updated successfully";
		} else if (mode == "MinifyJsCss") {
			Web.Write("<b>Minifying JS and CSS files</b><br /><br />");
			MinifyJS();
			MinifyCSS();
			Web.Write("<br>Done. (Note: The script will only minify immediate children files)");
			CommandStatus.message = "Minified JS and CSS files created successfully";
		} else if (mode == "GenDatabaseScripts") {
			Web.Write("<b>Generating Database Create Statements from Savvy Active Record Classes</b><br /><br />");
			Web.Write(Fmt.Text(ActiveRecordDatabaseGenerator.GetSqlStatements()));
			Web.Write("<br>Done.");
		} else if (mode == "ImportFromLive") {
			isOK = OKToImport("LVE");
			if (isOK == "OK") {
				Web.Write("<b>Importing all data from Live and replacing local data</b><br /><br />");
				//ActiveRecordDatabaseGenerator.ImportDataFrom(Util.GetSetting("LinkedServerDatabaseLVE", "throw"));
				ActiveRecordDatabaseGenerator.ImportDataFrom("LVE");
			} else {
				Web.Write(isOK);
			}
		} else if (mode == "ImportFromStaging") {
			isOK = OKToImport("STG");
			if (isOK == "OK") {
				Web.Write("<b>Importing all data from Staging and replacing local data</b><br /><br />");
				ActiveRecordDatabaseGenerator.ImportDataFrom("STG");
				Web.Write("<br>Done.");
			} else {
				Web.Write(isOK);
			}
		} else if (mode == "LegacyMigration") {
			Web.Write("<b>Migrating all Old Data</b><br /><br />");
			//LegacyDataMigration.GenerateDataMigrationScripts("RinnaiWebsite", true);
			Web.Write("<br>Done.");
		} else if (mode == "ExportToStaging") {
			Web.Write("<b>Copying all local data up to Staging and replacing existing staging data</b><br /><br />");
			ActiveRecordDatabaseGenerator.ExportDataTo("STG");
			Web.Write("<br>Done.");
		} else if (mode == "ViewCache") {
			Web.Write("<b>Displaying all objects in ASP.NET Web Cache</b><br /><br />");
			Web.Write(Fmt.Text(Logging.DumpCache()));
			Web.Write("<br>Done.");
		} else if (mode == "ClearCache") {
			Web.Write("<b>Clearing all objects in ASP.NET Web Cache</b><br /><br />");
			Web.CacheClearAll();
			Web.Write("<br>Done.");
			CommandStatus.message = "Cache cleared successfully";
		} else if (mode == "UpdateDLLs") {
			Web.Write("<b>Updating DLLs</b><br /><br />");
			UpdateDLLs();
			Web.Write("<br>Done.");
			Web.Write("<br>");
			Web.Write("<br><br><a href='?mode=AddDbFields'>Add fields to database</a>");
			Web.Write("<br><br><a href='?mode=RollbackDLLs'>Rollback</a>");
			Web.Write("<br><br><a href='" + Web.Root + "'>Check homepage</a>");
		} else if (mode == "RollbackDLLs") {
			Web.Write("<b>Rolling back DLLs to backup</b><br /><br />");
			RollbackDLLs();
			Web.Write("<br>Done.");
			Web.Write("<br><br><a href='" + Web.Root + "'>Check homepage</a>");
		} else if (mode == "FixError") {
			Web.Write("<b>Applying a Fix</b><br /><br />");
			Beweb.Logging.FixError(Request["message"], Request["title"], Request["lineThatDied"]);
			Web.Write("<br>Done.");
		} else if (currentClass == "all") {
			Assert.RunAllTests();
			Web.Write("<br>Done.");
		} else if (currentClass != null) {
			Assert.RunTests(currentClass);
			Web.Write("<br>Done.");
		} else if (mode == "CreateTable") {
			if (Util.ServerIsDev) {
				var tableName = Request["tableName"].UpperCaseFirstLetter();
				var ar = new ActiveRecord(tableName, tableName + "ID");
				var sql = ar.GetSqlForCreate();
				sql.Execute();
				CommandStatus.message = "Created table " + tableName;
			} else {
				CommandStatus.success = false;
				CommandStatus.message = "Only allowed on dev.";
			}
		} else if (mode == "DropTable") {
			if (Util.ServerIsDev) {
				var tableName = Request["tableName"].UpperCaseFirstLetter();
				new Sql("DROP TABLE " + tableName).Execute();
				CommandStatus.message = "Dropped table " + tableName;
			} else {
				CommandStatus.success = false;
				CommandStatus.message = "Only allowed on dev.";
			}
		} else if (mode == "ListTables") {
			var sql = new Sql().AddRawSqlString("SELECT t.name as [Table Name] FROM sys.tables AS t INNER JOIN sys.schemas AS s ON t.[schema_id] = s.[schema_id] ORDER BY t.name");
			CommandStatus.message = "Done";
			CommandStatus.data = Logging.DumpTableToHtml(sql.GetDataTable());
		} else if (mode == "Sql") {
			var query = Request["query"];
			if (query.IsNotBlank()) {
				if (query.ToLower().DoesntContain("select")) {
					try {
						int affectedRows = new Sql().AddRawSqlString(query).Execute();
						CommandStatus.message = "Affected rows: " + affectedRows;
					} catch (Exception ex) {
						CommandStatus.success = false;
						CommandStatus.message = ex.Message;
					}
				} else {
					var sql = new Sql().AddRawSqlString(query);
					CommandStatus.message = query;
					CommandStatus.data = Logging.DumpTableToHtml(sql.GetDataTable());
				}
			} else {
				CommandStatus.success = false;
				CommandStatus.message = "Missing parameter";
			}
		} else if (mode == "ListTables") {
			var sql = new Sql().AddRawSqlString("SELECT t.name as [Table Name] FROM sys.tables AS t INNER JOIN sys.schemas AS s ON t.[schema_id] = s.[schema_id] ORDER BY t.name");
			CommandStatus.message = "Done";
			CommandStatus.data = Logging.DumpTableToHtml(sql.GetDataTable());
		} else if (mode == "SendEmail") {
			var email = Request["email"];
			var subject = Request["subject"];
			var message = Request["message"];
			if (email.IsNotBlank() && subject.IsNotBlank() && message.IsNotBlank()) {
				try {
					SendEMail.SimpleSendHTMLEmail(email, subject, message);
					CommandStatus.message = "Email sent to " + email;
				} catch (Exception ex) {
					CommandStatus.success = false;
					CommandStatus.message = ex.Message;
				}
			} else {
				CommandStatus.success = false;
				CommandStatus.message = "Missing parameters";
			}
		} else if (mode == "WhoAmI") {
			if (Security.IsLoggedIn) {
				CommandStatus.message = "You are logged in as " + UserSession.Person.FullName;
			} else {
				CommandStatus.message = "You are not logged in";
				CommandStatus.success = false;
			}
		} else if (mode == "gotoServerStage") {
			var serverStage = Request["serverStage"];
			CommandStatus.data = Util.GetSetting("WebsiteBaseUrl" + serverStage);
			CommandStatus.message = "Going to " + serverStage + " url " + CommandStatus.data;
		} else if (mode == "ServerIs") {
			CommandStatus.message = "Server is: " + Util.ServerIs();
		} else if (mode == "UploadFile") {
			try {
				var file = Request.Files[0];
				if (file.ContentLength > 0) {

					var fullPath = Server.MapPath(Path.Combine("~/attachments/", Path.GetFileName(file.FileName)));
					int count = 1;

					string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
					string extension = Path.GetExtension(fullPath);
					string path = Path.GetDirectoryName(fullPath);
					string newFullPath = fullPath;

					while (File.Exists(newFullPath)) {
						string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
						newFullPath = Path.Combine(path, tempFileName + extension);
					}

					file.SaveAs(newFullPath);

					CommandStatus.message = "The file was uploaded successfully. <a href='" + Web.Attachments + Path.GetFileName(newFullPath) + "' target='_blank'>" + Path.GetFileName(newFullPath) + "</a>";

				} else {
					throw new Exception("No file selected");
				}
			} catch (Exception ex) {
				CommandStatus.success = false;
				CommandStatus.message = ex.Message.Replace("\\", "\\\\");
			}
		} else if (mode == "UploadURL") {
			var url = Request["url"];
			if (url.IsNotBlank()) {
				try {
					var filename = url.RightFrom("/");

					if (!url.Replace("http://", "").Replace("https://", "").Contains("/") || !filename.Contains(".")) {
						throw new Exception("The URL doesn't point to any file");
					}

					var newFilename = FileSystem.GetUniqueAttachmentFilename(filename);

					using (var client = new WebClient()) {
						client.DownloadFile(url, Server.MapPath(Path.Combine("~/attachments/", newFilename)));
					}

					CommandStatus.message = "The file was uploaded successfully. <a href='" + Web.Attachments + newFilename + "' target='_blank'>" + newFilename + "</a>";

				} catch (Exception ex) {
					CommandStatus.success = false;
					CommandStatus.message = ex.Message;
				}
			} else {
				CommandStatus.success = false;
				CommandStatus.message = "Missing parameter";
			}
		} else if (mode == "EncodeKeystoneID") {
			CommandStatus.message = "Encoded ID: " + Crypto.EncryptID(Request["id"].ToInt(0));
		} else if (mode == "EncodeKeystoneIDClassic") {
			CommandStatus.message = "Encoded ID: " + Crypto.EncryptIDClassic(Request["id"].ToInt(0));
		} else if (mode == "Encrypt") {
			CommandStatus.message = "Encrypted: " + Crypto.Encrypt(Request["enc"]);
		} else if (mode == "Decrypt") {
			CommandStatus.message = "Decrypted: " + Crypto.Decrypt(Request["decr"]);
		} else if (mode == "DecodeKeystoneID") {
			try {
				var id = Crypto.DecryptID(Request["encid"]);
				if (id == -1) {
					throw new Exception();
				}
				CommandStatus.message = "Decoded ID: " + id;
			} catch {
				CommandStatus.success = false;
				CommandStatus.message = "Invalid ID";
			}
		} else if (mode == "DecodeKeystoneIDClassic") {
			try {
				var id = Crypto.DecryptIDClassic(Request["encid"]);
				if (id == -1) {
					throw new Exception();
				}
				CommandStatus.message = "Decoded ID: " + id;
			} catch {
				CommandStatus.success = false;
				CommandStatus.message = "Invalid ID";
			}
		} else if (mode == "LockSite") {
			try {
				ConfigurationManager.AppSettings["LockSiteHomepage" + Util.ServerIs()] = "true";
				ConfigurationManager.RefreshSection("appSettings");

				//edit the actual web config
				SetLockSiteHomepageAppSettingsValue(true);

				//clear the cookie so if it works the lockscreen will appear on next page load / refresh
				var httpCookie = Response.Cookies["pwlock"];
				if (httpCookie != null) {
					httpCookie.Expires = DateTime.Now.AddDays(-1);
				}

				CommandStatus.message = "Site locked. [PWD:" + Util.GetSetting("LockSitePassword") + "] Double check your app settings before your next deploy";
			} catch (Exception) {
				CommandStatus.success = false;
				CommandStatus.message = "Failed to lock site";
			}
		} else if (mode == "UnlockSite") {
			try {
				//update app settings in memory.
				ConfigurationManager.AppSettings["LockSiteHomepage" + Util.ServerIs()] = "false";
				ConfigurationManager.RefreshSection("appSettings");

				//edit the actual web config

				SetLockSiteHomepageAppSettingsValue(false);

				//var replaceText = fileContent.ExtractTextBetween(@"<add key=""LockSiteHomepage"+Util.ServerIs()+@""" value=""", @"""/>");
				//clear the cookie so if it fails the lockscreen will appear on next page load / refresh
				var httpCookie = Response.Cookies["pwlock"];
				if (httpCookie != null) {
					httpCookie.Expires = DateTime.Now.AddDays(-1);
				}

				CommandStatus.message = "Site unlocked. Double check your app settings before your next deploy.";
			} catch (Exception) {
				CommandStatus.success = false;
				CommandStatus.message = "Failed to unlock site";
			}
		}

	}

	public static string UpdateDLLs() {
		string result = "";
		string binPath = Web.MapPath("~\\bin\\");
		string binPubPath = Web.MapPath("~\\bin-pub\\");
		string backupFilePath = Web.MapPath("~\\bin-pub\\backup\\");
		FileSystem.CreateFolder(backupFilePath);
		var files = Directory.GetFiles(binPubPath);
		foreach (string newFileName in files) {
			string liveFileName = newFileName.Replace("bin-pub", "bin");
			string backupFileName = newFileName.Replace("bin-pub", "bin-pub\\backup");

			if (File.Exists(liveFileName)) {
				if (File.Exists(backupFileName)) {
					File.Delete(backupFileName);//remove the old backup file
				}
				File.Copy(liveFileName, backupFileName);//backup the old file
				File.Delete(liveFileName);//remove the old file
			}
			File.Copy(newFileName, liveFileName);
			Web.Write(liveFileName.Replace(binPath, "") + " replaced with new version from bin-pub<br>");
		}
		return result;
	}

	public static void RollbackDLLs() {
		string binPath = Web.MapPath("~\\bin\\");
		string backupFilePath = Web.MapPath("~\\bin-pub\\backup\\");
		FileSystem.CopyFiles(backupFilePath, binPath, "*.dll", true);
		FileSystem.CopyFiles(backupFilePath, binPath, "*.pdb", true);
	}


	private void MinifyJS() {
#if JsCompressor
		var jsDirectory = new DirectoryInfo(Web.MapPath(Web.Root + "js/"));
		var files = jsDirectory.GetFiles("*.js", SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && !f.Name.EndsWith(".min.js")).ToList();
		
		foreach(var file in files) {
			var data = FileSystem.ReadTextFileAutoDetectEncoding(jsDirectory + file.Name);
			data = new JsCompressor().Compress(data);
			var minifiedName = (jsDirectory + file.Name).Replace(".js", ".min.js");
			SaveMinified(minifiedName, data);
		}
#else
		Web.Write("JsCompressor Disabled.");
#endif
	}

	private void MinifyCSS() {
		var cssDirectory = new DirectoryInfo(Web.MapPath(Web.Root));
		var files = cssDirectory.GetFiles("*.css", SearchOption.TopDirectoryOnly).Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && !f.Name.EndsWith(".min.css")).ToList();

		foreach (var file in files) {
			var data = FileSystem.ReadTextFileAutoDetectEncoding(cssDirectory + file.Name);

			data = Regex.Replace(data, @"[a-zA-Z]+#", "#");
			data = Regex.Replace(data, @"[\n\r]+\s*", string.Empty);
			data = Regex.Replace(data, @"\s+", " ");
			data = Regex.Replace(data, @"\s?([:,;{}])\s?", "$1");
			data = data.Replace(";}", "}");
			data = Regex.Replace(data, @"([\s:]0)(px|pt|%|em)", "$1");
			data = Regex.Replace(data, @"/\*[\d\D]*?\*/", string.Empty);

			var minifiedName = (cssDirectory + file.Name).Replace(".css", ".min.css");
			SaveMinified(minifiedName, data);
		}
	}

	private void SaveMinified(string filename, string data) {
		if (FileSystem.FileExists(filename)) {
			FileSystem.Delete(filename);
		}
		FileSystem.WriteTextFile(filename, data);
	}

	private void SetLockSiteHomepageAppSettingsValue(bool valueToSet) {
		var filePath = "~/Web_AppSettings.config";
		var fileContent = FileSystem.GetFileContentsAutoDetectEncoding(filePath);
		string oldValue = "<add key=\"LockSiteHomepage" + Util.ServerIs() + "\" value=\"" + (!valueToSet).ToString().ToLower() + "\"/>";
		string newValue = "<add key=\"LockSiteHomepage" + Util.ServerIs() + "\" value=\"" + valueToSet.ToString().ToLower() + "\"/>";
		var newfileContent = fileContent.Replace(oldValue, newValue);
		if (fileContent != newfileContent) {
			FileSystem.WriteTextFile(filePath, newfileContent);
		}
	}

}

