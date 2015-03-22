using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Beweb {
	public class ActiveRecordImporter<TActiveRecord> : Importer where TActiveRecord : ActiveRecord, new() {

		public ImportReport Report = new ImportReport();

		protected override bool ProcessLine(Line line) {
			var record = CreateOrFindRecord(line);
			foreach (var item in line.Items) {
				UpdateField(record, item.Key, item.Value);
			}
			//for (int sc = 0; sc < headerItems.Length; sc++) {
			//  string item = headerItems[sc];
			//  string newValue = lineItems[sc];

			//  UpdateField(record, item, newValue);

			//}
			SaveRecord(record, line);
			return true;
		}

		/// <summary>
		/// You can override this method to look up the record by the appropriate primary key or whatever other reference field that comes in the imported data.
		/// By default it will search by primary key - this assumes that the primary key is in the imported data. If not found already, a new record is added.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		protected virtual TActiveRecord CreateOrFindRecord(Line line) {
			var record = new TActiveRecord();
			// populate if key is found in db
			record.LoadDataByID(line.Items[record.GetPrimaryKeyName()]);
			return record;
		}

		protected virtual void UpdateField(TActiveRecord record, string fieldName, string newValue) {
			if (record.FieldExists(fieldName)) {					//20120625JN add this fix for when a field exists in the import, but not in the database
				record[fieldName].FromString(newValue);
			} else {
				//skipped import of that field as it exists ion the import data, but not in the database table
			}
		}

		protected virtual void SaveRecord(TActiveRecord record, Line line) {
			record.Save();
		}

		protected virtual void SaveRecord(TActiveRecord record, Line line, bool addToReport, string editUrl) {
			bool dirty = record.GetDirty();
			bool isNewRecord = record.IsNewRecord;
			string changesDescription = record.GetChangesDescription();

			record.Save();

			string url = editUrl + record.ID_Field.ValueObject;
			if (isNewRecord) {
				Report.AddSuccessLine(record.GetFriendlyTableName() + " Added", record.GetName(), null, url);
			} else if (dirty) {
				Report.AddSuccessLine(record.GetFriendlyTableName() + " Updated", record.GetName(), changesDescription, url);
			}

		}

		protected override void ProcessLineComplete(bool lineProcessedOk, Line line) {

		}

		protected override void ImportFailure(string pathname) {
		}		
		protected override void ImportFailure(string pathname, List<Line> lines) {
			ImportFailure(pathname);
		}

		protected override void ImportSuccess(string pathname) {
		}
		protected override void ImportSuccess(string pathname, List<Line> lines) {
			ImportSuccess(pathname);
		}

		protected override void SendProcessLineFailureEmail(int lineNumber, string errorMessage) {
		}


		protected override void SendFileFailureEmail(string errorMessage) {
		}


	}

	public abstract class Importer {


		protected List<Line> allLines = new List<Line>();
		protected string fileName;
		private string _incomingPath;
		public bool MoveImportedFiles { get; set; }

		public string IncomingPath {
			get { return _incomingPath; }
			set {
				ProcessedPath = IncomingPath + "/processed";
				RejectsPath = IncomingPath + "/rejects";
				_incomingPath = value;
			}
		}

		public string ProcessedPath { get; set; }
		public string RejectsPath { get; set; }
		public ErrorActions ErrorAction { get; set; }
		public Formats Format { get; set; }
		//public bool MoveFilesWhenDone { get; set; }
		public bool NoFilePermissionsMode { get; set; }
		//public bool LogInDatabase { get; set; }
		//public bool MoveFilesWhenProcessed { get; set; }
		//public bool MoveFilesWhenProcessed { get; set; }

		/// <summary>
		/// File path mask eg "*.txt"
		/// Default is "*.*"
		/// </summary>
		public string DirectoryFilter { get; set; }

		public enum ErrorActions {
			ThrowError, EmailDeveloper, Ignore
		}

		public enum Formats {
			TabSeparatedValues, CommaSeparatedValues, Xml
		}

		public Importer() {
			IncomingPath = "~/dropbox";
			ErrorAction = ErrorActions.ThrowError;
			Format = Formats.TabSeparatedValues;
		}

		/// <summary>
		/// Just returns
		/// </summary>
		/// <returns></returns>
		private bool IsDropboxNotEmpty() {
			string dropboxFullPath = Web.MapPath(IncomingPath);

			try {
				// create drop folder if doesn't exist 
				FileSystem.CreateFolder(dropboxFullPath);
				int numFiles = Directory.GetFiles(dropboxFullPath).Count();
				return numFiles > 0;

			} catch (System.UnauthorizedAccessException accessException) {
				throw new System.UnauthorizedAccessException("Drop folder problem: you need to grant the user ASPNET read/write/delete permissions over the folder '" + dropboxFullPath + "'.\n" + accessException.Message);
			}
		}

		/// <summary>
		/// Run the import. To tailor the import to handle a specific data format, create a derived class and override the various methods.
		/// </summary>
		public void ImportFilesInDropbox() {
			if (IsDropboxNotEmpty()) {
				string dropboxFullPath = Web.MapPath(IncomingPath);
				string dropboxProcessedFullPath = null;
				string dropboxRejectsFullPath = null;
				if (!NoFilePermissionsMode) {
					dropboxProcessedFullPath = Web.MapPath(ProcessedPath);
					dropboxRejectsFullPath = Web.MapPath(RejectsPath);
				}

				try {
					Init(dropboxFullPath, dropboxProcessedFullPath, dropboxRejectsFullPath);

					string[] files;
					if (DirectoryFilter.IsNotBlank()) {
						files = Directory.GetFiles(dropboxFullPath, DirectoryFilter);
					} else {
						files = Directory.GetFiles(dropboxFullPath);
					}

					foreach (var pathname in files) {
						string filename = pathname.RightFrom("\\");

						this.fileName = filename;

						if (!NoFilePermissionsMode || new Sql("select count(*) from DataImport where filename=", filename.SqlizeText()).FetchIntOrZero() == 0) {
							// process new file
							var ok = ProcessFile(dropboxFullPath, filename);

							bool useUniqueFiles = Util.GetSettingBool("ImporterUniqueFiles", true);
							if (ok) {
								if (NoFilePermissionsMode) {
									var log = new ActiveRecord("DataImport", "DataImportID");
									log["FileName"].ValueObject = filename;
									log["DateImported"].ValueObject = DateTime.Now;
									log["ImportedBy"].ValueObject = "Importer";
									log.Save();
								} else {
									//bool useUniqueFiles = Util.GetSettingBool("ImporterUniqueFiles", true);
									string destfilename = filename;
									if (useUniqueFiles) {
										destfilename = FileSystem.GetUniqueFilename(dropboxFullPath + "\\", filename, 50, true);
									}

									if (Util.GetSettingBool("ImporterMoveFiles", true) && MoveImportedFiles) {
										bool allowOverwrite = !useUniqueFiles;
										FileSystem.Move(dropboxFullPath + "\\" + filename, dropboxProcessedFullPath + "\\" + destfilename, allowOverwrite);
									}
								}

								//Web.Response.Write("Imported file: " + filename + "<br>");
								ImportSuccess(pathname);

							} else {
								if (ErrorAction == ErrorActions.ThrowError) {
									throw new ProgrammingErrorException("Data Import Error: could not import file [" + filename + "]");
								} else if (ErrorAction == ErrorActions.EmailDeveloper) {
									SendEMail.SimpleSendEmail(SendEMail.EmailAboutError, Util.GetSiteName() + " Import error", "Data Import Error: could not import file [" + filename + "]. Moved to Rejects folder.");
								}
								if (!NoFilePermissionsMode) {
									if (!IsIgnoredIfError) {
										if (useUniqueFiles) {
											filename = FileSystem.GetUniqueFilename(dropboxRejectsFullPath + "\\", filename, 50, true);
										}
										bool allowOverwrite = !useUniqueFiles;

										try {
											FileSystem.Move(dropboxFullPath + "\\" + filename, dropboxRejectsFullPath + "\\" + filename, allowOverwrite);
										} catch (Exception e) {

											Web.Response.Write("NOK:file did not process ok. Also failed to move file: " + filename + " exception[" + e.Message + "]<br>");
										}
									}
								}
								Web.Response.Write("Could not import file: " + filename + "<br>");
								//return; // just go on to the next file

								ImportFailure(pathname);

							}
						}
					}
				} catch (System.UnauthorizedAccessException accessException) {
					throw new System.UnauthorizedAccessException("Drop folder problem: you need to grant the user ASPNET read/write/delete permissions over the folder '" + dropboxFullPath + "'.\n" + accessException.Message);
				}

			}
		}

		protected abstract void ProcessLineComplete(bool lineProcessedOk, Line line);

		/// <summary>
		/// called when a file fails, implement in derived class
		/// </summary>
		/// <param name="pathname"></param>
		protected abstract void ImportFailure(string pathname, List<Line> lines);
		protected abstract void ImportFailure(string pathname);

		/// <summary>
		/// called when a file succeeds, implement in derived class
		/// </summary>
		/// <param name="pathname"></param>
		/// <param name="lines"> </param>
		protected abstract void ImportSuccess(string pathname, List<Line> lines);
		protected abstract void ImportSuccess(string pathname);

		protected abstract void SendProcessLineFailureEmail(int lineNumber, string errorMessage);

		protected abstract void SendFileFailureEmail(string errorMessage);

		private void Init(string dropboxFullPath, string dropboxProcessedFullPath, string dropboxRejectsFullPath) {
			if (NoFilePermissionsMode) {
				if (!BewebData.TableExists("DataImport")) {
					new Sql("create table DataImport (DataImportID int identity (1,1), FileName nvarchar(100), DateImported datetime, ImportedBy nvarchar(50), constraint PK_DataImport PRIMARY KEY CLUSTERED  (DataImportID))").Execute();
				}
			} else {
				// create drop folder if doesn't exist 
				FileSystem.CreateFolder(dropboxFullPath);
				FileSystem.CreateFolder(dropboxProcessedFullPath);
				FileSystem.CreateFolder(dropboxRejectsFullPath);
			}
		}


		public virtual bool ProcessFile(string dropboxFullPath, string filename) {
			var result = true;
			if (Format == Formats.TabSeparatedValues) {
				result = ProcessTabSeparatedFile(dropboxFullPath, filename);
			} else if (Format == Formats.CommaSeparatedValues) {
				result = ProcessCsvFile(dropboxFullPath, filename);
			} else if (Format == Formats.Xml) {
				result = ProcessXmlFile(dropboxFullPath, filename);
			} else {
				throw new NotImplementedException();
			}
			// do success/fail here
			var pathname = dropboxFullPath + "\\" + filename;
			if (result) {
				ImportSuccess(pathname, allLines);
			} else {
				ImportFailure(pathname, allLines);
			}
			return result;
		}

		protected virtual void BeforeFileProcess(string filename) {

		}


		public bool ProcessXmlFile(string dropboxFullPath, string filename) {
			throw new NotImplementedException();
			// open xml file
			// select all elements matching recordElementTag
			// foreach node ProcessLine()
			//   get all children nodenames and values as fieldnames and values
			// close xml file
		}

		public bool ProcessTabSeparatedFile(string dropboxFullPath, string filename) {
			bool result = false;

			using (StreamReader reader = new StreamReader(dropboxFullPath + "\\" + filename)) {
				// 3
				// Use while != null pattern for loop
				string line;
				int lineNumber = 0;
				string[] headerItems = null;
				while ((line = reader.ReadLine()) != null) {
					string[] lineItems = line.Split('\t');
					if (lineNumber == 0) {
						headerItems = lineItems;
						if (!ProcessHeader(headerItems)) {
							Web.Response.Write("Header wrong format");
							break;
						}
					} else if (line.IsNotBlank()) {
						result = CreateLine(lineNumber, lineItems, headerItems, filename);
						if (!result) break;
					}

					lineNumber++;

					//System.Threading.Thread.Sleep(100);

					if (lineNumber % 5 == 0) {
						//Web.Response.Write(".");
						//Web.Response.Flush();
					}

					if (lineNumber > 50) {
						//break;
					}
				}
			}

			if (result) {
				result = ProcessAllLinesInternal();
			}

			return result;
		}

		public bool IsIgnoredIfError;

		public bool ProcessCsvFile(string dropboxFullPath, string filename) {
			IsIgnoredIfError = false;
			bool result = false;
			List<List<string>> lines = null;
			try {
				lines = ThirdParty.CommaSeparatedValueFile.Parse(dropboxFullPath + "\\" + filename);
			} catch (Exception) {

				IsIgnoredIfError = true;
				//throw new FileLoadException("File [" + fileName + "] on path + [" + dropboxFullPath + "] was open on the server. Exception is [" + e + "]");
			}

			if (lines != null) {

				// Use while != null pattern for loop
				int lineNumber = 0;
				string[] headerItems = null;
				foreach (List<string> line in lines) {
					string[] lineItems = line.ToArray();
					if (lineNumber == 0) {
						headerItems = lineItems;
						if (!ProcessHeader(headerItems)) {
							Web.Response.Write("Header wrong format");
							break;
						}
					} else {
						result = CreateLine(lineNumber, lineItems, headerItems, filename);
						if (!result) break;
					}

					lineNumber++;

					//System.Threading.Thread.Sleep(100);

					if (lineNumber % 5 == 0) {
						//Web.Response.Write(".");
						//Web.Response.Flush();
					}

					if (lineNumber > 50) {
						//break;
					}
				}
			}

			if (result) {
				ProcessAllLinesInternal();
			}

			return result;
		}


		/// <summary>
		/// check header is correct
		/// </summary>
		/// <param name="lineNumber"></param>
		protected virtual bool ProcessHeader(string[] headerItems) {
			if (RequiredHeaders != null) {
				foreach (string requiredHeader in RequiredHeaders) {
					if (!headerItems.Contains(requiredHeader, StringComparer.CurrentCultureIgnoreCase)) {
						return false;
					}
				}
			}
			return true;
		}

		public virtual string[] RequiredHeaders {
			get {
				return null;
			}
		}

		private bool CreateLine(int lineNumber, string[] lineItems, string[] headerline, string fileName) {
			var line = new Line(lineNumber, lineItems, headerline, fileName);
			allLines.Add(line);
			return true;
		}

		private bool ProcessLineInternal(Line line) {
			bool lineProcessedOk = ProcessLine(line);
			ProcessLineComplete(lineProcessedOk, line);
			return lineProcessedOk;
		}

		private bool ProcessAllLinesInternal() {
			foreach (var line in allLines) {
				bool result = ProcessLineInternal(line);
				if (!result) return false;
			}
			return true;
		}

		protected abstract bool ProcessLine(Line line);


		public class Line {
			public int LineNumber;
			public Dictionary<string, string> Items = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
			public string FileName;

			public Line(int lineNumber, string[] lineItems, string[] headerline, string fileName) {
				LineNumber = lineNumber;
				FileName = fileName;
				int i = 0;
				foreach (string header in headerline) {
					string itemValue = null;
					if (i < lineItems.Count()) {
						itemValue = lineItems[i];
					}
					if (header.IsBlank()) {
						break;
					}
					Items.Add(header, itemValue);
					i++;
				}
			}

			//string[] Items;
			//string[] Header;
			//public string this[int i] {
			//  get { return Items[i]; }
			//}
			//public string this[string headerName] {
			//  get {
			//    int i = 0;
			//    foreach (string header in Header) {
			//      if (header.ToLower() == headerName.ToLower()) {
			//        return Items[i];
			//        i++;
			//      }
			//    }
			//    throw new ProgrammingErrorException("Data Import Error: could not find header value [" + headerName + "]");
			//  }
			//}

		}


	}



	public class ImportReport {
		public List<ImportReportLine> ImportReportLines = new List<ImportReportLine>();//this if for each instance of a report..

		/*public List<ImportReportLine> GetReportLines() {
			if (this.ImportReportLines == null) {
				this.ImportReportLines = new List<ImportReportLine>();
			}
			return this.ImportReportLines;
		}*/

		public StringConst StatusSuccess = new StringConst("Success", "color:green;font-weight:bold;");
		public StringConst StatusFailed = new StringConst("Failed", "color:red;font-weight:bold;");
		public StringConst StatusWarning = new StringConst("Warning", "color:orange;font-weight:bold;");
		public StringConst StatusInfo = new StringConst("Info", "color:blue;font-weight:bold;");

		public StringConst LineTypeTitle = new StringConst("Title", "font-size:18px !important;font-weight:bold !important;");
		public StringConst LineTypeSubtitle = new StringConst("Subtitle", "font-size:16px !important;font-weight:bold !important;");
		public StringConst LineTypeLine = new StringConst("Line", "font-size:12px !important;font-weight:bold !important;");

		public string ExtraReportData;
		public string ReportTitle;

		public ImportReport() {
		}
		public ImportReport(string title) {
			this.ReportTitle = title;
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeTitle, Title = title });
		}

		public void AddTitle(string title) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeTitle, Title = title });
		}

		public void AddSubtitle(string title) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeSubtitle, Title = title });
		}

		public void AddSuccessLine(string title, string ident, string desc) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusSuccess });
		}

		public void AddSuccessLine(string title, string ident, string desc, string url) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusSuccess, LinkUrl = url });
		}

		public void AddSuccessLine(string title, string ident, string desc, string url, string urlCaption) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusSuccess, LinkUrl = url, UrlCaption = urlCaption });
		}

		public void AddFailedLine(string title, string ident, string desc, string url) {
			AddFailedLine(title, ident, desc, url, null);
		}

		public void AddFailedLine(string title, string ident, string desc, string url, string urlCaption) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusFailed, LinkUrl = url, UrlCaption = urlCaption });
		}

		public void AddFailedLine(string title, string ident, string desc) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusFailed });
		}

		public void AddWarningLine(string title, string ident, string desc, string url) {
			AddWarningLine(title, ident, desc, url, null);
		}

		public void AddWarningLine(string title, string ident, string desc, string url, string urlCaption) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusWarning, LinkUrl = url, UrlCaption = urlCaption });
		}

		public void AddWarningLine(string title, string ident, string desc) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusWarning });
		}

		public void AddInfoLine(string title, string ident, string desc, string url) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusInfo, LinkUrl = url });
		}

		public void AddInfoLine(string title, string ident, string desc) {
			ImportReportLines.Add(new ImportReportLine() { LineType = LineTypeLine, Title = title, Identifier = ident, Description = desc, Status = StatusInfo });
		}

		public void AddTotalsLine(string count) {
			ImportReportLines.Add(new ImportReportLine() { LineType = "", Title = "", Identifier = "", Description = count, Status = "" });
		}


		public string ToHtml() {
			// return an html table

			var table = new HtmlTag("table style='xwidth: 1000px;	background-color: white ; border: 1px solid #CCC;'");

			//style='font-family: 'Open Sans', Trebuchet MS, sans-serif, Arial; font-weight: 300; font-size: 13px;color: #000;'

			var thStyle = "background-color: #e0e7a2;font-family: Open Sans, Trebuchet MS, sans-serif, Arial; font-weight: 300; font-size: 13px;color: #000;text-align:left;";
			var tdStyle = "font-family: Open Sans, Trebuchet MS, sans-serif, Arial; font-weight: 300; font-size: 12px;color: #000;border-bottom:1px solid #CCC;'";


			var tr = new HtmlTag("tr");
			HtmlTag th;

			//var th = new HtmlTag("th style='width:100px;" + thStyle + "'");
			//th.SetInnerText("Status");
			//tr.AddTag(th);

			th = new HtmlTag("th width=20% style='" + thStyle + "'");
			th.SetInnerText("Title");
			tr.AddTag(th);

			th = new HtmlTag("th width=20% style='" + thStyle + "'");
			th.SetInnerText("Code");
			tr.AddTag(th);

			th = new HtmlTag("th width=50% style='" + thStyle + "'");
			th.SetInnerText("Description");
			tr.AddTag(th);

			th = new HtmlTag("th width=10% style='" + thStyle + "'");
			th.SetInnerText("Url");
			tr.AddTag(th);

			table.AddTag(tr);


			foreach (var line in ImportReportLines) {
				// tr
				tr = new HtmlTag("tr");
				HtmlTag td;

				var textColour = "";
				if (line.Status == StatusFailed) {
					textColour = StatusFailed.DisplayName;
				} else if (line.Status == StatusWarning) {
					textColour = StatusWarning.DisplayName;
				} else if (line.Status == StatusInfo) {
					textColour = StatusInfo.DisplayName;
				} else if (line.Status == StatusSuccess) {
					textColour = StatusSuccess.DisplayName;
				} else {
					textColour = "";
				}
				//td = new HtmlTag("td style='" + textColour + "" + tdStyle + "'");
				//td.SetInnerText(line.Status);
				//tr.AddTag(td);

				if (line.LineType == LineTypeSubtitle) {
					td = new HtmlTag("td colspan='4' style ='" + LineTypeSubtitle.DisplayName + "" + tdStyle + "'");
					td.SetInnerText(line.Title);
					tr.AddTag(td);
				} else if (line.LineType == LineTypeTitle) {
					td = new HtmlTag("td colspan=4 style ='" + LineTypeTitle.DisplayName + "" + tdStyle + "'");
					var spanTag = new HtmlTag("span style = '" + textColour + "'");
					spanTag.SetInnerHtml(line.Title);
					td.AddTag(spanTag);
					tr.AddTag(td);

				} else {
					td = new HtmlTag("td style ='" + textColour + "" + tdStyle + "'");
					var spanTag = new HtmlTag("span style = '" + textColour + "'");
					spanTag.SetInnerHtml(line.Title);
					td.AddTag(spanTag);
					tr.AddTag(td);
				}

				td = new HtmlTag("td style='" + tdStyle + "'");
				td.SetInnerText(line.Identifier);
				tr.AddTag(td);

				td = new HtmlTag("td style='" + tdStyle + "'");
				if (line.Description.IsBlank()) {
					td.SetInnerText("");
				} else {
					td.SetInnerText(line.Description);
				}
				tr.AddTag(td);

				td = new HtmlTag("td style='" + tdStyle + "'");
				if (line.LinkUrl.IsNotBlank()) {
					td.AddRawHtml("<a href='" + Web.ResolveUrlFull(line.LinkUrl) + "' target='_blank'>" + line.UrlCaption.DefaultValue("View/edit") + "</a>");
				}
				tr.AddTag(td);

				table.AddTag(tr);
			}


			return table.ToString();
		}

		public void Add(ImportReport importReport) {
			foreach (var line in importReport.ImportReportLines) {
				ImportReportLines.Add(line);
			}
		}
		public bool HasLines {
			get { return (ImportReportLines.Count(l => l.LineType == LineTypeLine) > 0); }
		}
		public void SendIfHasLines() {
			SendIfHasLines(null);
		}
		public void SendIfHasLines(string emailTo) {
			if (HasLines) {
				var email = new ElectronicMail() { Subject = ReportTitle };
				email.ToAddress = emailTo ?? SendEMail.EmailToAddress;
				email.BodyHtml = ToHtml();
				email.Send(true);
			}
		}
	}

	public enum ReportLineResult {
		Success, Failure, Warning
	};

	public class ImportReportLine {
		public string LineType; // title, subtitle, totals or line
		public string Title; // email or title eg store title
		public string Identifier; //id of record
		public string Description; // what happen
		public string Status; //success fail etc
		//public string Colour; // red green orange
		public string LinkUrl; // link to the record in the cms to fix if needs fixing
		public string UrlCaption; //caption for link url
	}


}
