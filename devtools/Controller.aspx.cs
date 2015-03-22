using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Beweb;

namespace Site.devtools {
	public partial class Controller : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			string mode = Request["mode"];

			if (mode == "GetCpuUsage") GetCpuUsage();
			else if (mode == "GetMemoryUsage") GetMemoryUsage();
			else if (mode == "GetNetworkUsage") GetNetworkUsage();
			else if (mode == "GetDiskUsage") GetDiskUsage();
			else if (mode == "AttachmentExistsOnDB") AttachmentExistsOnDB();
			else if (mode == "GetDBTableFields") GetDBTableFields();
			else if (mode == "ExecuteSQL") ExecuteSQL();
			else if (mode == "PrecacheAttachmentsDisk") PrecacheAttachmentsDisk();
			else if (mode == "PrecacheAttachmentsDB") PrecacheAttachmentsDB();
			else if (mode == "CompareAttachments") CompareAttachments();
			else if (mode == "MoveUnusedAttachments") MoveUnusedAttachments();
		}

		private void GetCpuUsage() {

			var cpuCounter = new PerformanceCounter();

			cpuCounter.CategoryName = "Processor";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";

			var perfCounterValue = cpuCounter.NextValue();
			System.Threading.Thread.Sleep(1000);
			perfCounterValue = cpuCounter.NextValue();

			Web.Write("{\"cpu\": " + perfCounterValue + "}");
		}

		private void GetMemoryUsage() {
			var proc = Process.GetCurrentProcess();
			Web.Write("{\"memory\": " + ((proc.PrivateMemorySize64 / 1024) / 1024) + "}");
		}

		private void GetNetworkUsage() {
			var monitor = new NetworkMonitor();
			NetworkAdapter[] adapters = monitor.Adapters;

			if (adapters.Length == 0) {
				throw new Exception("No network adapters found on this server");
			}

			monitor.StartMonitoring();

			for (int i = 0; i < 10; i++) {
				foreach (NetworkAdapter adapter in adapters) {
					var down = adapter.DownloadSpeedKbps;
					var up = adapter.UploadSpeedKbps;

					if (down > 0 || up > 0) {
						down = adapter.DownloadSpeedKbps;
						up = adapter.UploadSpeedKbps;

						monitor.StopMonitoring();
						Web.Write("{\"dl\": " + down + ", \"ul\": " + up + "}");
						return;
					}
				}

				System.Threading.Thread.Sleep(1000);

			}
		}

		private void GetDiskUsage() {
			string appPath = HttpRuntime.AppDomainAppPath;
			string diskLetter = appPath.Substring(0, 2);

			var driveInfo = new DriveInfo(diskLetter);

			var totalSpace = (((driveInfo.TotalSize / 1024) / 1024) / 1024);
			var freeSpace = (((driveInfo.TotalFreeSpace / 1024) / 1024) / 1024);
			var usedSpace = totalSpace - freeSpace;

			Web.Write("{\"total\": " + totalSpace.ToString("N1") + ", \"free\": " + freeSpace.ToString("N1") + ", \"used\": " + usedSpace.ToString("N1") + "}");
		}

		private void AttachmentExistsOnDB() {

			// @todo: Get a list of all attachments (disk) and put in a string array
			// @todo: Get a list of all attachments (db) and put in a string array
			// @todo: Using linq, merge (remove)

			var foundOnTable = "";

			try {
				var file = Request["filename"].Base64Decode();
				var attachmentName = file.RightFrom(Web.MapPath(Web.Attachments)).ToLower().Replace("\\", "/");

				foreach (var tableName in BewebData.GetTableNames()) {

					if (foundOnTable.IsNotBlank()) break;

					var fields = Cache[tableName + "Fields"] != null ? (string)Cache[tableName + "Fields"] : null;

					if (fields == null) {
						var sql = new Sql("select top 1 * from ", tableName.SqlizeName());
						var sb = new StringBuilder("");
						using (var reader = sql.GetReader()) {
							if (reader.HasRows) {
								int visibleFieldCount = reader.VisibleFieldCount;
								for (int i = 0; i < visibleFieldCount; i++) {	// for each column
									string dataType = reader.GetDataTypeName(i).ToLower();
									string fieldName = reader.GetName(i).ToLower();
									bool isAttach = (dataType.Contains("varchar") && (fieldName.Contains("attachment") || fieldName.Contains("picture")));
									bool isRich = ((dataType.Contains("varchar") || dataType.Contains("text")) && (fieldName.Contains("html") || fieldName.Contains("body") || fieldName.Contains("text")));
									if (isAttach || isRich) {
										sb.Append(fieldName).Append(",");
									}
								}
							}
						}
						fields = sb.ToString();
						fields = fields.IsNotBlank() ? fields.Substring(0, fields.Length - 1) : ""; // remove last comma
						Cache[tableName + "Fields"] = fields;
					}

					if (fields.IsNotBlank()) {
						var sql = new Sql("select " + fields + " from ", tableName.SqlizeName());

						using (var reader = sql.GetReader()) {
							foreach (DbDataRecord record in reader) {

								if (foundOnTable.IsNotBlank()) break;

								for (int i = 0; i < reader.VisibleFieldCount; i++) {	// for each column
									string fieldName = record.GetName(i).ToLower();
									bool isAttach = ((fieldName.Contains("attachment") || fieldName.Contains("picture")));
									if (!record.IsDBNull(i)) {
										string fieldValue = record.GetString(i);
										if (fieldValue.IsNotBlank()) {
											if (fieldValue == attachmentName || (!isAttach && fieldValue.ToLower().Contains(attachmentName))) {
												foundOnTable = tableName;
												break;
											}
										}
									}
								}
							}
						}
					}

				}
			} catch {
				Web.Write("{\"success\":false}");
				return;
			}

			Web.Write("{\"success\":true, \"foundOnTable\": \"" + foundOnTable + "\"}");
		}

		private void GetDBTableFields() {

			var table = Request["table"].Base64Decode();

			var fields = new Sql().AddRawSqlString("SELECT c.column_id as ColumnID, c.name as ColumnName, c.is_nullable as IsNullable, c.is_identity as IsIdentity, t.name as Type, c.max_length as MaxLength, i.is_unique as IsUnique, i.is_primary_key as IsPrimaryKey, object_definition(c.default_object_id) AS default_value, ep.value as Description FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id LEFT JOIN sys.index_columns ic ON c.object_id = ic.object_id AND c.column_id = ic.column_id LEFT JOIN sys.indexes i ON i.index_id = ic.index_id AND i.object_id = ic.object_id LEFT JOIN sys.extended_properties ep on c.column_id = ep.minor_id and ep.name = 'MS_Description' WHERE c.object_id = OBJECT_ID ('" + table + "') ORDER BY c.column_id").LoadPooList<TableFieldsPOO>();

			var json = new JavaScriptSerializer().Serialize(fields);
			Web.Write(json);
		}

		public class TableFieldsPOO {
			public int ColumnID { get; set; }
			public string ColumnName { get; set; }
			public bool IsNullable { get; set; }
			public bool IsIdentity { get; set; }
			public string Type { get; set; }
			public double MaxLength { get; set; }
			public bool IsUnique { get; set; }
			public bool IsPrimaryKey { get; set; }
			public string DefaultValue { get; set; }
			public string Description { get; set; }
		}

		private void ExecuteSQL() {

			var sql = Request["sql"].Base64Decode();

			if (sql.ToLower().Contains("select ")) {
				Fetch(sql);
			} else {
				var sw = Stopwatch.StartNew();
				sw.Start();
				//var affectedRows = new Sql().AddRawSqlString(s).Execute();
				sw.Stop();
				Web.Write("{\"success\": true, \"type\":\"nonquery\", \"time\": 3213132, \"affected\": 2}");
			}

		}

		private void Fetch(string sql) {

			OleDbDataReader reader = null;

			try {

				var db = new DataBlock();
				db.OpenDB(BewebData.GetConnectionString());
				var conn = new OleDbConnection(db.connString);
				conn.Open();
				var exec = new OleDbCommand(sql, conn);

				var sw = Stopwatch.StartNew();

				sw.Start();
				reader = exec.ExecuteReader();
				sw.Stop();

				var fields = new List<Dictionary<string, string>>();
				var data = new List<List<string>>();

				if (reader.FieldCount > 0) {

					for (int i = 0; i < reader.FieldCount; i++) {
						var f = new Dictionary<string, string>();
						f["name"] = reader.GetName(i);
						f["type"] = reader.GetFieldType(i).ToString().Replace("System.", "");
						fields.Add(f);
					}

					while (reader.Read()) {
						var row = fields.Select(field => reader[field["name"]].ToString()).ToList();
						data.Add(row);
					}

				}

				var jsonSerialiser = new JavaScriptSerializer();

				Web.Write("{\"success\": true, \"type\":\"query\", \"time\": " + sw.ElapsedMilliseconds + ", \"rows\": " + data.Count + ", \"fields\": " + jsonSerialiser.Serialize(fields) + ", \"data\": " + jsonSerialiser.Serialize(data) + "}");

			} catch (Exception ex) {
				Web.Write("{\"success\": false, \"error\":\"" + ex.Message + "\"}");
			} finally {
				if (reader != null && !reader.IsClosed) {
					reader.Close();
				}
			}
		}

		private void PrecacheAttachmentsDisk() {
			try {
				var excludedExtensions = new[] { ".config", ".txt" };
				var directory = new DirectoryInfo(Web.MapPath(Web.Attachments));
				var diskAttachments = directory.GetFiles("*.*", SearchOption.AllDirectories).Where(f => (f.Attributes & FileAttributes.Hidden) == 0 && !excludedExtensions.Contains(f.Extension) && f.DirectoryName.DoesntContain("unused_files")).OrderByDescending(f => f.Length).Select(file => file.FullName.RightFrom(Web.MapPath(Web.Attachments)).ToLower().Replace("\\", "/")).ToList();
				Session["diskAttachments"] = diskAttachments;
				Web.Write("{\"success\": true, \"result\":\"" + diskAttachments.Count + " files found\"}");
			} catch (Exception ex) {
				Web.Write("{\"success\": false, \"error\":\"" + ex.Message + "\"}");
			}
		}

		private void PrecacheAttachmentsDB() {
			try {
			
				var dbAttachments = new HashSet<string>();

				foreach (var tableName in BewebData.GetTableNames()) {

					var sql = new Sql("select top 1 * from ", tableName.SqlizeName());
					var sb = new StringBuilder("");
					using (var reader = sql.GetReader()) {
						if (reader.HasRows) {
							int visibleFieldCount = reader.VisibleFieldCount;
							for (int i = 0; i < visibleFieldCount; i++) {	// for each column
								string dataType = reader.GetDataTypeName(i).ToLower();
								string fieldName = reader.GetName(i).ToLower();
								bool isAttach = (dataType.Contains("varchar") && (fieldName.Contains("attachment") || fieldName.Contains("picture")));
								bool isRich = ((dataType.Contains("varchar") || dataType.Contains("text")) && (fieldName.Contains("html") || fieldName.Contains("body") || fieldName.Contains("text")));
								if (isAttach || isRich) {
									sb.Append(fieldName).Append(",");
								}
							}
						}
					}
					var fields = sb.ToString();
					fields = fields.IsNotBlank() ? fields.Substring(0, fields.Length - 1) : ""; // remove last comma

					if (fields.IsNotBlank()) {
						sql = new Sql("select " + fields + " from ", tableName.SqlizeName());
						using (var reader = sql.GetReader()) {
							foreach (DbDataRecord record in reader) {
								for (int i = 0; i < reader.VisibleFieldCount; i++) {	// for each column
									string fieldName = record.GetName(i).ToLower();
									bool isAttach = ((fieldName.Contains("attachment") || fieldName.Contains("picture")));
									if (!record.IsDBNull(i)) {
										string fieldValue = record.GetString(i);
										if (fieldValue.IsNotBlank()) {
											if (isAttach) {
												dbAttachments.Add(MakeFilePathRelativeToAttachments(fieldValue));
											} else {
												foreach (var extractedImage in ExtractImagesFromHTML(fieldValue)) {
													dbAttachments.Add(MakeFilePathRelativeToAttachments(extractedImage));	
												}
												foreach (var extractedDocument in ExtractDocumentsFromHTML(fieldValue)) {
													dbAttachments.Add(MakeFilePathRelativeToAttachments(extractedDocument));	
												}
											}
										}
									}
								}
							}
						}
					}
				}
				Session["dbAttachments"] = dbAttachments;
				Web.Write("{\"success\": true, \"result\":\"" + dbAttachments.Count + " references found\"}");
			} catch (Exception ex) {
				Web.Write("{\"success\": false, \"error\":\"" + ex.Message + "\"}");
			}
		}

		private void CompareAttachments() {

			var diskAttachments = (List<string>)Session["diskAttachments"];
			var dbAttachments = (HashSet<string>)Session["dbAttachments"];

			diskAttachments.RemoveAll(dbAttachments.Contains);
			var count = diskAttachments.Count;

			Session["diskAttachments"] = null;
			Session["dbAttachments"] = null;

			Session["unusedAttachments"] = diskAttachments;

			var attachmentsFolder = Web.MapPath(Web.Attachments);		
			int totalBytes = 0;

			var fileList = new StringBuilder("[");

			foreach(var attachment in diskAttachments) {
				var f = new FileInfo(Path.Combine(attachmentsFolder, attachment));
				totalBytes += f.Length.ToInt();
				if(fileList.Length > 1) fileList.Append(",");
				fileList.Append("\""+attachment+"\"");
			}

			fileList.Append("]");

			Web.Write("{\"success\": true, \"attachmentsUrl\": \""+Web.Attachments+"\", \"files\": "+fileList+", \"result\":\""+count+" files were marked to be deleted"+(count > 0 ? " ("+Fmt.FileSize(totalBytes, 2)+"). <br/><a href='#' onclick='Vacuum.showFiles();return false;'>Show files</a> | <a href='#' onclick='Vacuum.moveFiles();return false;'>Move to attachments/unused_files</a>" : "")+"\"}");
		}

		private void MoveUnusedAttachments() {

			var attachmentsFolder = Web.MapPath(Web.Attachments);			
			var unusedFilesDirectory = attachmentsFolder + "unused_files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

			if(!Directory.Exists(unusedFilesDirectory))
					Directory.CreateDirectory(unusedFilesDirectory);

			var attachments = (List<string>)Session["unusedAttachments"];

			foreach(var attachment in attachments) {
				var existing = Path.Combine(attachmentsFolder, attachment);

				if(File.Exists(existing)) {
					// Before moving, create all subfolders structure
					if(attachment.Contains("/")) {
						var subfolders = attachment.LeftUntilLast("/");	
						if(!Directory.Exists(unusedFilesDirectory + subfolders))
							Directory.CreateDirectory(unusedFilesDirectory + subfolders);
					}				
					
					var target = Path.Combine(unusedFilesDirectory, attachment);		

					if(File.Exists(target)) 
						File.Delete(target);	
					
					File.Move(existing, target);	
				}
			}

			Session["unusedAttachments"] = null;

			Web.Write("{\"success\": true, \"result\":\"All files have been moved\"}");
		}

		private List<string> ExtractImagesFromHTML(string html) {
			var images = new List<string>();

			var imgRegex = new Regex("<img\\s[^>]*?src\\s*=\\s*['\"]([^'\"]*?)['\"][^>]*?>", RegexOptions.IgnoreCase|RegexOptions.Multiline);
      MatchCollection matches = imgRegex.Matches(html);

      for (int i = 0; i < matches.Count; i++)
      {
					var img = matches[i].Value;
					var srcRegex = new Regex("src=(\"|')(?<src>[^(\"|')]*)(\"|')", RegexOptions.IgnoreCase);
					var src = srcRegex.Match(img).Groups[3].Value;
					images.Add(src);
      }

			return images;
		}

		private List<string> ExtractDocumentsFromHTML(string html) {
			var fileRegex = new Regex("<a\\s[^>]*?href\\s*=\\s*['\"]([^'\"]*?)['\"][^>]*?>", RegexOptions.IgnoreCase|RegexOptions.Multiline);
      MatchCollection matches = fileRegex.Matches(html);

			var files = new List<string>();
      for (int i = 0; i < matches.Count; i++)
      {
					var file = matches[i].Value;
					var hrefRegex = new Regex("href=(\"|')(?<href>[^(\"|')]*)(\"|')", RegexOptions.IgnoreCase);
					var href = hrefRegex.Match(file).Groups[3].Value;

					if(href != "#" && !href.StartsWith("mailto:")) {
						files.Add(href);	
					}
      }

			return files;
		}

		private string MakeFilePathRelativeToAttachments(string path) {
			// It will be relative to attachments folder
			if(path.StartsWith("~WebRoot") || path.StartsWith("attachments")) {
				path = path.RightFromFirst("attachments/");
			}

			// Remove the / at the beginning (if any)
			if(path[0] == '/') {
				path = path.Substring(1);
			}
			
			return path;
		}

	}
}