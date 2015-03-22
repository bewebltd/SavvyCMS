using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
//using Models;

namespace Beweb {
	public class ActiveRecordGenerator {

		// settings for backwards compatibility
		public static bool ReformatTableNames = false;

		//protected const string templatePath = "admin\\tools\\template\\LinqToSqlModels\\";
		protected static string templatePath = "~\\admin\\tools\\template\\ActiveRecordModels\\";
		protected static string generatedPath = "~\\App_Code\\Generated\\";
		protected static string userCodePath = "~\\App_Code\\Models\\";
		protected static string backupPath = "~\\_backup\\ActiveRecordModels\\";

		protected static string templateFullPath = Web.MapPath(templatePath);
		protected static string generatedFullPath = Web.MapPath(generatedPath);
		protected static string userCodeFullPath = Web.MapPath(userCodePath);
		protected static string backupFullPath = Web.MapPath(backupPath);

		protected const bool isDebug = false;

		//public List<string> tableNames = null; 20100520 removed - now superceded by tables
		protected List<ActiveRecordGenerator.Table> tables = new List<Table>();
		protected List<string> createdFilenames = new List<string>();

		private List<string> Warnings = new List<string>();   // contains any warnings from generator, to be displayed to user
		private bool allowOverwriteUserCode = false;
		public static bool ThrowExceptionOnFieldNameSameAsClass = false;

		public static bool AllowMessyNames { get { return Util.GetSettingBool("SavvyActiveRecord_UncleanModelProperties", false); } }
		public static bool AllowNullableBooleans { get { return Util.GetSettingBool("SavvyActiveRecord_AllowNullableBooleans", false); } }
		public bool GenForeignKeys { get { return Util.GetSettingBool("SavvyActiveRecord_GenForeignKeys", true); } }
		public List<string> GenForeignKeysCustom { get { return new List<string>(Util.GetSetting("SavvyActiveRecord_GenForeignKeysCustom", "").Split("|")); } }
		public string GenTables { get { return Util.GetSetting("SavvyActiveRecord_GenTables", "(all)"); } }
		public string GenViews { get { return Util.GetSetting("SavvyActiveRecord_GenViews", "(none)"); } }
		public string GenTableExclusions { get { return Util.GetSetting("SavvyActiveRecord_GenTableExclusions", "(none)"); } }
		public string GenDateAdded { get { return Util.GetSetting("SavvyActiveRecord_GenDateAdded", "(none)"); } }
		public string GenDateModified { get { return Util.GetSetting("SavvyActiveRecord_GenDateModified", "(none)"); } }
		public string GenIDs { get { return Util.GetSetting("SavvyActiveRecord_GenIDs", "(none)"); } }
		public string GenSEOFields { get { return Util.GetSetting("SavvyActiveRecord_GenSEOFields", "(none)"); } }
		public string GenPublishExpiryFields { get { return Util.GetSetting("SavvyActiveRecord_GenPublishExpiryFields", "(none)"); } }
		public string GenMapLocationFields { get { return Util.GetSetting("SavvyActiveRecord_GenMapLocationFields", "(none)"); } }

		public ActiveRecordGenerator() {
			// find models folder
			if (FileSystem.FolderExists(Web.MapPath("~/SiteCustom/Models/Generated"))) {
				// nonstandard folder configuration for hybrid projects eg RAL
				generatedPath = "~/SiteCustom/Models/Generated/";
				userCodePath = "~/SiteCustom/Models/Models/";
				generatedFullPath = Web.MapPath(generatedPath);
				userCodeFullPath = Web.MapPath(userCodePath);
			} else if (!FileSystem.FolderExists(Web.MapPath("~\\App_Code"))) {
				// standard method - use MVC folders instead
				generatedPath = "~/Models/Generated/";
				userCodePath = "~/Models/Models/";
				generatedFullPath = Web.MapPath(generatedPath);
				userCodeFullPath = Web.MapPath(userCodePath);
			} else if (FileSystem.FolderExists(Web.MapPath("~\\App\\Generated").Replace("Site\\", ""))) {
				// use App project folders instead
				string basePath = Web.MapPath("~\\App").Replace("\\Site\\App", "");
				generatedPath = basePath + "\\App\\Generated\\";
				userCodePath = basePath + "\\App\\Models\\";
				backupPath = basePath + "\\_backup\\ActiveRecordModels\\";
				generatedFullPath = generatedPath;
				userCodeFullPath = userCodePath;
				backupFullPath = backupPath;
			}
		}

		/// <summary>
		/// Generate Beweb Active Record model data classes
		/// </summary>
		/// <returns></returns>
		public static string Run() {
			var gen = new ActiveRecordGenerator();
			return gen.Generate();
		}

		public string Generate() {
			allowOverwriteUserCode = Web.Request["overwrite"] == "1";

			MakeBackup();
			var originalTableNames = ActiveRecordDatabaseGenerator.GetModelTableNames();
			originalTableNames.Sort();

			// generate code
			string code = RunTemplates();

			CheckFullTextIndexes();

			DeployUnitTests();
			Web.Write("<br><b>Backup</b> - if the generated code does not compile, there is a backup saved in the Generated folder. You need to reinstate this backup to get the site compiling in order to get this page to run again.<br>");

			if (Warnings.Count > 0) {
				Web.Write("<br><br><b style='color:red'>WARNING:</b>");
				foreach (string warning in Warnings) {
					Web.Write("<br>" + warning.HtmlEncode() + "<br>");
				}

			}

			if (UpdateProjectMakeFile(originalTableNames)) {
				Web.Write("<br><b>Project Build File</b> - updated project items in  Visual Studio site CSPROJ file.<br>");
			}

			Web.Write("<br>Time: " + DateTime.Now);
			return "done";
		}

		private void CheckFullTextIndexes() {

			var text = FileSystem.ReadTextFileAutoDetectEncoding("~/Web_AppSettings.config");
			var searchText = "<add key=\"SavvyActiveRecord_FullTextFields\" value=\"(autogen)\"/>";
				string fullTextColumns = ActiveRecordDatabaseGenerator.GetFullTextColumns();
			if (text.Contains(searchText)) {
				var replaceText = "<add key=\"SavvyActiveRecord_FullTextFields\" value=\"" + fullTextColumns + "\"/>";
				text = text.Replace(searchText, replaceText);
				FileSystem.WriteTextFile("~/Web_AppSettings.config", text);
			} else {
				var fields = Util.GetSetting("SavvyActiveRecord_FullTextFields","(none)");
				if (fields.IsNotBlank() && fields != "(none)" && fields!=fullTextColumns) {
					Web.WriteLine("<br><br><b>Full Text Indexes</b>");
					var tables = KeywordSearch.GetSqlFullTextTables();
					foreach (var table in tables) {
						KeywordSearch.CreateFullTextIndex(table);
					}
				}
			}
		}

		private bool UpdateProjectMakeFile(List<string> originalTableNames) {
			bool isUpdated = false;
			string newTableNames = tables.Select(t => t.Name).OrderBy(n => n).ToString(",");
			if (newTableNames != originalTableNames.ToString(",")) {
				string filepath = Web.MapPath("~/Site.csproj");
				if (!File.Exists(filepath)) {
					filepath = Web.MapPath("~/SiteCustom/SiteCustom.csproj");
				}
				if (File.Exists(filepath)) {
					string filesToCompile = "";
					foreach (var table in tables) {
						filesToCompile += "    <Compile Include=\"Models\\Generated\\" + table.ClassName + "Generated.cs\" />\r\n";
						filesToCompile += "    <Compile Include=\"Models\\Models\\" + table.ClassName + ".cs\" />\r\n";
					}
					if (BewebData.TableExists("GenTest") && BewebData.TableExists("Person")) {
						//filesToCompile += "    <Compile Include=\"Models\\Generated\\ActiveRecordTests.cs\" />\r\n";
					}
					string txt = File.ReadAllText(filepath);
					string pattern = @"<Compile Include=""Models\\(Generated|Models)\\(.+)\.cs"" />\r\n";
					txt = Regex.Replace(txt, pattern, "");
					txt = txt.ReplaceFirst("<ItemGroup>", "<ItemGroup>" + VB.crlf + filesToCompile);
					File.WriteAllText(filepath, txt);

					isUpdated = true;
					//<Compile Include="Models\Generated\ActiveRecordTests.cs" />
				}
			}

			return isUpdated;
		}

		private static void MakeBackup() {
			try {
				// create backup folder if doesn't exist 
				Directory.CreateDirectory(backupFullPath);
				// delete all files in backup folder
				//Directory.Delete(backupFullPath, true);
				var oldfiles = Directory.GetFiles(backupFullPath);
				foreach (var pathname in oldfiles) {
					string filename = pathname.RightFrom("\\");
					File.Delete(backupFullPath + filename);

				}
				// recreate backup folder
				//Directory.CreateDirectory(backupFullPath);

				// clear out any previously generated files
				var files = Directory.GetFiles(generatedFullPath);
				foreach (var pathname in files) {
					string filename = pathname.RightFrom("\\");
					File.Delete(backupFullPath + filename);
					File.Move(pathname, backupFullPath + filename);
				}

			} catch (System.UnauthorizedAccessException accessException) {
				throw new System.UnauthorizedAccessException("Beweb ActiveRecord Problem: you need to grant the user ASPNET read/write/delete permissions over the folders '_backup' and 'app_code/generated'.\n" + accessException.Message);
			}
		}

		private void DeployUnitTests() {
			if (!BewebData.TableExists("GenTest")) return;
			bool okToOverwrite = false;
			string templateFilePath = templateFullPath + "Tests-cs";
			if (!FileSystem.FileExists(templateFilePath)) {
				templateFilePath = templateFullPath + "Tests.cs";
			}
			string deployFilePath = generatedFullPath + "ActiveRecordTests.cs";

			string templateFileContents = File.ReadAllText(templateFilePath);
			string deployedFileContents = "";
			okToOverwrite = !File.Exists(deployFilePath);

			// check if file exists
			if (!okToOverwrite) {
				deployedFileContents = File.ReadAllText(deployFilePath);
				// check if contents all deleted
				okToOverwrite = (deployedFileContents.Trim() == "");
			}
			if (!okToOverwrite) {
				// check timestamp and size
				bool isTemplateNewer = File.GetLastWriteTime(templateFilePath) >= File.GetLastWriteTime(deployFilePath);
				bool isTemplateBigger = templateFileContents.Length >= deployedFileContents.Length;
				okToOverwrite = isTemplateNewer || isTemplateBigger;
			}
			if (okToOverwrite) {
				File.Copy(templateFilePath, deployFilePath, true);
				Web.Write("<br><br><b>Unit Tests Included</b> - copied from Template to Generated folder<br>");
			} else {
				Web.Write("<br><br><b>Unit Tests Not Included</b> - Didn't want to overwrite active record unit tests file: " + deployFilePath + " because it is newer and/or bigger than the version in " + templateFilePath + ". You should copy your changes into the template if you want to keep them. Otherwise you can delete the generated file if you want to get a fresh one from template.<br>");
			}
		}

		/// <summary>
		/// decides which table/view templates to generate based on web config settings, and then generate them
		/// </summary>
		/// <returns></returns>
		public string RunTemplates() {
			string result = "";
			result += GetMainTemplate();

			// add table templates
			string genTables = GenTables;
			// MN 20130220 - moved this down
			if (genTables != "(none)") {
				AddTableTemplates(genTables.Split('|'), false);
			}

			// add view templates
			string genViews = GenViews;
			if (genViews == "(all)") {
				// generate all views
				AddTableTemplates(true);
			} else if (genViews != "(none)") {
				AddTableTemplates(genViews.Split('|'), true);
			}
			Web.Write(tables.Count + " tables...<br>");
			RunAllTemplates();

			//result += RunManyToManyTemplates();

			result += "}\r\n";   // close namespace
			result += "#endregion";

			return result;
		}

		public string RunAllTemplates() {
			string result = "";

			string exclusions = "|" + GenTableExclusions + "|";
			int i = 1;
			foreach (var table in tables) {
				Web.Write("<br>[" + (i++) + "]Model for table " + table.Name + "");
				if (!exclusions.Contains("|" + table.Name + "|") && !table.Name.StartsWith("aspnet_")) {
					AddStandardFieldsTable(table);
					DiscoverForeignKeyLinks(table);
					GenerateFileForTable(table);
					GenerateUserCodeFileForTable(table);
					Web.Write("--ok");
				} else {
					tables.Remove(table);
					Web.Write("--skipped - in exclusions");
				}
				//result += "";//GetTableTemplate(tableName);
			}

			return result;
		}

		/// <summary>
		/// Add DateAdded, DateModified, ID, SEO fields if needed
		/// </summary>
		/// <param name="table"></param>
		private void AddStandardFieldsTable(Table table) {
			if (!table.IsView) {
				var sql = new Sql();
				if (table.IsInPipeList(GenDateAdded)) {
					if (!table.FieldExists("DateAdded")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD DateAdded DATETIME;");
					}
				}
				if (table.IsInPipeList(GenDateModified)) {
					if (!table.FieldExists("DateModified") && !table.FieldExists("LastModified")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD DateModified DATETIME;");
					}
				}
				if (table.IsInPipeList(GenIDs)) {
					var realTableName = table.Name.LeftUntil(".").Replace("[", "").Replace("]", "");
					var expectedPkName = realTableName + "ID";
					if (!table.FieldExists(expectedPkName) || table.PrimaryKey.isNullable) {
						if (table.FieldExists(expectedPkName)) {
							sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ALTER COLUMN " + expectedPkName + " [int] IDENTITY(1,1) NOT NULL;");
						} else if (table.FieldExists("id")) {
							// promote ID col to correct name tableID and autonumber it - so you just have to name the first column 'id' with any old datatype
							sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ALTER COLUMN id [int] IDENTITY(1,1) NOT NULL;");
							sql.AddRawSqlString("sp_RENAME '" + table.Name + ".[id]' , '[" + expectedPkName + "]', 'COLUMN';");
						} else {
							// just add the tableID col at the end
							sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD " + expectedPkName + " [int] IDENTITY(1,1) NOT NULL;");
						}
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD CONSTRAINT pk_" + realTableName + "_ID PRIMARY KEY CLUSTERED (" + expectedPkName + ");");
					}
					/* Too hard version!
					 * 
					 * BEGIN TRANSACTION
					GO
					CREATE TABLE dbo.Tmp_Table_1
						(
						TestID int NOT NULL IDENTITY (1, 1),
						test nchar(10) NULL,
						test2 nchar(10) NULL
						)  ON [PRIMARY]
					GO
					ALTER TABLE dbo.Tmp_Table_1 SET (LOCK_ESCALATION = TABLE)
					GO
					SET IDENTITY_INSERT dbo.Tmp_Table_1 OFF
					GO
					IF EXISTS(SELECT * FROM dbo.Table_1)
						 EXEC('INSERT INTO dbo.Tmp_Table_1 (test, test2)
							SELECT test, test2 FROM dbo.Table_1 WITH (HOLDLOCK TABLOCKX)')
					GO
					DROP TABLE dbo.Table_1
					GO
					EXECUTE sp_rename N'dbo.Tmp_Table_1', N'Table_1', 'OBJECT' 
					GO
					ALTER TABLE dbo.Table_1 ADD CONSTRAINT
						PK_Table_1 PRIMARY KEY CLUSTERED 
						(
						TestID
						) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

					GO
					 */
				}
				if (table.IsInPipeList(GenPublishExpiryFields)) {
					if (!table.FieldExists("PublishDate")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD PublishDate datetime;");
					}
					if (!table.FieldExists("ExpiryDate")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD ExpiryDate datetime;");
					}
				}
				if (table.IsInPipeList(GenSEOFields)) {
					if (!table.FieldExists("PageTitleTag")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD PageTitleTag nvarchar(150);");
					}
					if (!table.FieldExists("MetaKeywords")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD MetaKeywords nvarchar(250);");
					}
					if (!table.FieldExists("MetaDescription")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD MetaDescription nvarchar(MAX);");
					}
					if (!table.FieldExists("FocusKeyword")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD FocusKeyword nvarchar(150);");
					}
				}
				if (table.IsInPipeList(GenMapLocationFields)) {
					if (!table.FieldExists("Latitude")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD Latitude float;");
					}
					if (!table.FieldExists("Longitude")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD Longitude float;");
					}
					if (!table.FieldExists("GeocodedAddress")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD GeocodedAddress nvarchar(500);");
					}
					if (!table.FieldExists("GeocodingResult")) {
						sql.Add("ALTER TABLE", table.Name.SqlizeName(), "ADD GeocodingResult nvarchar(20);");
					}
				}
				if (sql.ToString().IsNotBlank()) {
					Web.Write("<br><br><b>WAIT A SECOND....</b> I need to add some fields first....<br><br>");
					Web.Write(sql.Value);
					sql.Execute();
					Web.Write("<br><br>OK, done that.... <a href='' onclick='location.reload()'><b>REFRESH TO GENERATE MODELS</b></a>");
					Web.End();
				}
			}
		}

		public void AddTableTemplates(string[] tableNames, bool isView) {
			foreach (string tableName in tableNames) {
				if (tableName == "(all)") {
					// generate all tables
					AddTableTemplates(isView);
				} else
					AddTable(tableName, isView);
			}
		}

		public void AddTableTemplates(bool isView) {
			string tableType = isView ? "VIEW" : "BASE TABLE";
			string sql = "SELECT DISTINCT table_name FROM INFORMATION_SCHEMA.Tables WHERE table_name NOT LIKE 'sys%' and table_name<>'dtproperties' and table_type='" + tableType + "' ORDER BY table_name";
			var reader = new Sql().AddRawSqlString(sql).GetReader();//BewebData.GetRecords(sql);
			string exclusions = "|" + GenTableExclusions + "|";
			foreach (DbDataRecord row in reader) {
				var tableName = row["Table_Name"].ToString();
				//if (Web.IsRunningOnWebServer) {
				//}
				if (!exclusions.Contains("|" + tableName + "|") && !tableName.StartsWith("aspnet_")) {
					Web.Write(tableName + "... ");
					AddTable(tableName, isView);
				} else {
					Web.Write("[" + tableName + " skipped]... ");

				}
			}
			reader.Dispose();
		}

		/// <summary>
		/// Create a file and generate the code for a single table.
		/// </summary>
		public void GenerateFileForTable(Table table) {
			// make a backup first
			string fileName = table.ClassName + "Generated.cs";
			string filePath = generatedFullPath + fileName;
			string fileBackupPath = generatedFullPath + "backup.exclude\\" + fileName;
			if (File.Exists(filePath)) {
				FileSystem.CreateFolder(generatedFullPath + "backup.exclude");
				File.Copy(filePath, fileBackupPath, true);
			}
			createdFilenames.Add(generatedPath + fileName);   // relative path of created file

			string code = "";
			code += GetMainTemplate();
			code += GetTableTemplate(table);
			code += "}\r\n";   // close namespace
			code += "#endregion";

			File.WriteAllText(filePath, code);
		}

		/// <summary>
		/// Create the customisable user code partial file for a single table.
		/// </summary>
		public void GenerateUserCodeFileForTable(Table table) {
			// make a backup first
			string fileName = table.ClassName + ".cs";
			string filePath = userCodeFullPath + fileName;
			createdFilenames.Add(userCodePath + fileName);   // relative path of created file

			// see if ok to replace
			if (this.allowOverwriteUserCode || !IsUserCodeFileCustomised(filePath)) {
				string code = GetUserCodeTableTemplate(table);
				File.WriteAllText(filePath, code);
			} else {
				Web.Write("<br>Model partial class for " + table.Name + " not overwritten as it was customised.");
			}
		}

		private bool IsUserCodeFileCustomised(string filePath) {
			if (!File.Exists(filePath)) {
				return false;    // file doesn't exist therefore is not modified and can overwrite
			}
			string text = File.ReadAllText(filePath);
			if (text.Contains("// created: [")) {
				string timestring = text.ExtractTextBetween("// created: [", "]");
				DateTime timestamp;
				bool isValidDate = DateTime.TryParse(timestring, out timestamp);
				if (isValidDate) {
					DateTime lastModified = File.GetLastWriteTime(filePath);
					// if lastmodified is after timestamp then it is modified - give 3 second tolerance
					if (lastModified > timestamp.AddSeconds(3)) {
						return true;
					} else {
						return false;
					}
				}
			}
			return true;  // file exists but has no valid timestamp - therefore is IS modified
		}

		// old method - does not handle getting primary key
		//
		//private static string RunColumnTemplates(string tableName)
		//{
		//    string result = "";

		//    string sql2 = "SELECT * FROM INFORMATION_SCHEMA.Constraints WHERE table_name='" + Fmt.SQLString(tableName) + "' ORDER BY table_name";


		//    string sql = "SELECT * FROM INFORMATION_SCHEMA.Columns WHERE table_name='"+Fmt.SQLString(tableName)+"' ORDER BY table_name";
		//    var reader = BewebData.GetRecords(sql);
		//    int colPos = 0;
		//    foreach (DbDataRecord row in reader)
		//    {
		//        string fieldName = row["Column_Name"].ToString();
		//        string dbType= row["Data_Type"].ToString();
		//        bool isNullable = row["IS_NULLABLE"].ToString() == "YES" ? true : false;
		//        int maxLength = 0;
		//        if (row["CHARACTER_MAXIMUM_LENGTH"].ToString()+""!="") {
		//            maxLength = int.Parse(row["CHARACTER_MAXIMUM_LENGTH"].ToString());
		//        }
		//        bool isPrimaryKey = (colPos==0);   // we always assume primary key is first column - beweb standard convention

		//        if (tableName == "GenTest") {
		//            var tableReader = BewebData.GetRecords("select top 1 * from ["+tableName+"]");
		//            var schemaTable = tableReader.GetSchemaTable();
		//            Logging.DumpFields(schemaTable);

		//            for (var i = 0; i < row.FieldCount; i++)
		//            {

		//            //	HttpContext.Current.Response.Write("<br>i=" + i + " name=" + row.GetName(i) + " value="+row.GetValue(i).ToString());
		//            }
		//        }

		//        result += GetColumnTemplate(tableName, fieldName, dbType, isNullable, maxLength, isPrimaryKey);
		//        colPos++;
		//    }
		//    return result;
		//}

		private string RunColumnTemplates(Table table) {
			string result = "";

			foreach (var column in table.Columns) {
				result += GetColumnTemplate(table, column);
			}

			return result;

			#region SchemaTable dumpfields reference

			/* SchemaTable dumpfields 
DEBUG: ---------- 
DEBUG: col[ColumnName], val[GenTestID], datatype[System.String] 
DEBUG: col[ColumnOrdinal], val[0], datatype[System.Int32] 
DEBUG: col[ColumnSize], val[4], datatype[System.Int32] 
DEBUG: col[NumericPrecision], val[10], datatype[System.Int16] 
DEBUG: col[NumericScale], val[255], datatype[System.Int16] 
DEBUG: col[IsUnique], val[False], datatype[System.Boolean] 
DEBUG: col[IsKey], val[], datatype[System.Boolean] 
DEBUG: col[BaseServerName], val[], datatype[System.String] 
DEBUG: col[BaseCatalogName], val[], datatype[System.String] 
DEBUG: col[BaseColumnName], val[GenTestID], datatype[System.String] 
DEBUG: col[BaseSchemaName], val[], datatype[System.String] 
DEBUG: col[BaseTableName], val[], datatype[System.String] 
DEBUG: col[DataType], val[System.Int32], datatype[System.Type] 
DEBUG: col[AllowDBNull], val[False], datatype[System.Boolean] 
DEBUG: col[ProviderType], val[8], datatype[System.Int32] 
DEBUG: col[IsAliased], val[], datatype[System.Boolean] 
DEBUG: col[IsExpression], val[], datatype[System.Boolean] 
DEBUG: col[IsIdentity], val[True], datatype[System.Boolean] 
DEBUG: col[IsAutoIncrement], val[True], datatype[System.Boolean] 
DEBUG: col[IsRowVersion], val[False], datatype[System.Boolean] 
DEBUG: col[IsHidden], val[], datatype[System.Boolean] 
DEBUG: col[IsLong], val[False], datatype[System.Boolean] 
DEBUG: col[IsReadOnly], val[True], datatype[System.Boolean] 
DEBUG: col[ProviderSpecificDataType], val[System.Data.SqlTypes.SqlInt32], datatype[System.Type] 
DEBUG: col[DataTypeName], val[int], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionDatabase], val[], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionOwningSchema], val[], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionName], val[], datatype[System.String] 
DEBUG: col[UdtAssemblyQualifiedName], val[], datatype[System.String] 
DEBUG: col[NonVersionedProviderType], val[8], datatype[System.Int32] 
DEBUG: col[IsColumnSet], val[False], datatype[System.Boolean] 
DEBUG: ---------- 
DEBUG: dumpfields 
DEBUG: ---------- 
DEBUG: col[ColumnName], val[Title], datatype[System.String] 
DEBUG: col[ColumnOrdinal], val[1], datatype[System.Int32] 
DEBUG: col[ColumnSize], val[50], datatype[System.Int32] 
DEBUG: col[NumericPrecision], val[255], datatype[System.Int16] 
DEBUG: col[NumericScale], val[255], datatype[System.Int16] 
DEBUG: col[IsUnique], val[False], datatype[System.Boolean] 
DEBUG: col[IsKey], val[], datatype[System.Boolean] 
DEBUG: col[BaseServerName], val[], datatype[System.String] 
DEBUG: col[BaseCatalogName], val[], datatype[System.String] 
DEBUG: col[BaseColumnName], val[Title], datatype[System.String] 
DEBUG: col[BaseSchemaName], val[], datatype[System.String] 
DEBUG: col[BaseTableName], val[], datatype[System.String] 
DEBUG: col[DataType], val[System.String], datatype[System.Type] 
DEBUG: col[AllowDBNull], val[True], datatype[System.Boolean] 
DEBUG: col[ProviderType], val[22], datatype[System.Int32] 
DEBUG: col[IsAliased], val[], datatype[System.Boolean] 
DEBUG: col[IsExpression], val[], datatype[System.Boolean] 
DEBUG: col[IsIdentity], val[False], datatype[System.Boolean] 
DEBUG: col[IsAutoIncrement], val[False], datatype[System.Boolean] 
DEBUG: col[IsRowVersion], val[False], datatype[System.Boolean] 
DEBUG: col[IsHidden], val[], datatype[System.Boolean] 
DEBUG: col[IsLong], val[False], datatype[System.Boolean] 
DEBUG: col[IsReadOnly], val[False], datatype[System.Boolean] 
DEBUG: col[ProviderSpecificDataType], val[System.Data.SqlTypes.SqlString], datatype[System.Type] 
DEBUG: col[DataTypeName], val[varchar], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionDatabase], val[], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionOwningSchema], val[], datatype[System.String] 
DEBUG: col[XmlSchemaCollectionName], val[], datatype[System.String] 
DEBUG: col[UdtAssemblyQualifiedName], val[], datatype[System.String] 
DEBUG: col[NonVersionedProviderType], val[22], datatype[System.Int32] 
DEBUG: col[IsColumnSet], val[False], datatype[System.Boolean] 
DEBUG: ---------- 
		 */
			#endregion
		}

		////////////////////////////////////////////

		private string RunManyToManyTemplates(Table table) {
			string result = "";
			if (table.Name.Contains("Has")) {
				if (GenForeignKeys) {
					string rightTableName = table.Name.RightFrom("Has");
					string leftTableName = table.Name.Replace("Has" + rightTableName, "");

					if (BewebData.TableExists(leftTableName) && BewebData.TableExists(rightTableName)) {

						result += GetManyToManyTemplate(table, tables.Find(t => t.Name == leftTableName), tables.Find(t => t.Name == rightTableName));
					}

				}
			}
			return result;
		}

		public string GetManyToManyTemplate(Table joinTable, Table table1, Table table2) {
			string path = templatePath + "ManyToMany.template-cs";

			if (table1 == null || table2 == null) return null;
			string result = FileSystem.GetFileContents(path, true);
			result = result.Replace("xxxtable1xxx", table1.Name);
			result = result.Replace("xxxtable2xxx", table2.Name);
			result = result.Replace("xxxtable1pkxxx", table1.pkName);
			result = result.Replace("xxxtable2pkxxx", table2.pkName);

			// foreign key to table1
			string table1fk;
			if (joinTable.Columns.Exists(field => field.fieldName == table1.pkName)) {
				table1fk = table1.pkName;
			} else if (joinTable.Columns.Exists(field => field.fieldName == table1.Name + "ID")) {
				table1fk = table1.Name + "ID";
			} else {
				string msg = "Warning: ActiveRecordGenerator - Skipping Many to Many Template - Unable to determine foreign key in many to many relationship for table [" + joinTable.Name + "]. Please check the field names in this table. The foreign key to table [" + table1.Name + "] must be either [" + table1.pkName + "] or [" + table1.Name + "ID" + "]. To avoid generating this table, add it to the exclusions in Web_AppSettings.config.";
				Warnings.Add(msg);
				return "";
			}
			result = result.Replace("xxxtable1fkxxx", table1fk);

			// foreign key to table2
			string table2fk;
			if (joinTable.Columns.Exists(field => field.fieldName == table2.pkName)) {
				table2fk = table2.pkName;
			} else if (joinTable.Columns.Exists(field => field.fieldName == table2.Name + "ID")) {
				table2fk = table2.Name + "ID";
			} else {
				string msg = "Warning: ActiveRecordGenerator - Skipping Many to Many Template - Unable to determine foreign key in many to many relationship for table [" + joinTable.Name + "]. Please check the field names in this table. The foreign key to table [" + table2.Name + "] must be either [" + table2.pkName + "] or [" + table2.Name + "ID" + "]. To avoid generating this table, add it to the exclusions in Web_AppSettings.config.";
				Warnings.Add(msg);
				return "";
			}
			result = result.Replace("xxxtable2fkxxx", table2fk);

			/*
						var table1fk = joinTable.Columns.Find(field => field.fieldName.ToLower() == table1.pkName.ToLower());
						if (table1fk==null) {
							table1fk = joinTable.Columns.Find(field => field.fieldName.ToLower() == table1.Name + "ID".ToLower());
						}
						if (table1fk==null) {
							string msg = "Warning: ActiveRecordGenerator - Skipping Many to Many Template - Unable to determine foreign key in many to many relationship for table [" + joinTable.Name + "]. Please check the field names in this table. The foreign key to table [" + table1.Name + "] must be either [" + table1.pkName + "] or [" + table1.Name + "ID" + "]. To avoid generating this table, add it to the exclusions in Web_AppSettings.config.";
							Warnings.Add(msg);
							return "";
						}
						result = result.Replace("xxxtable1fkxxx", table1fk.GetPropertyName());

						// foreign key to table2
						var table2fk = joinTable.Columns.Find(field => field.fieldName.ToLower() == table2.pkName.ToLower());
						if (table2fk==null) {
							table1fk = joinTable.Columns.Find(field => field.fieldName.ToLower() == table2.Name + "ID".ToLower());
						}
						if (table2fk==null) {
							string msg = "Warning: ActiveRecordGenerator - Skipping Many to Many Template - Unable to determine foreign key in many to many relationship for table [" + joinTable.Name + "]. Please check the field names in this table. The foreign key to table [" + table2.Name + "] must be either [" + table2.pkName + "] or [" + table2.Name + "ID" + "]. To avoid generating this table, add it to the exclusions in Web_AppSettings.config.";
							Warnings.Add(msg);
							return "";
						}
						result = result.Replace("xxxtable2fkxxx", table2fk.GetPropertyName());
			 */

			string propertyName = GetPropertyName(table2.Name.Plural(), table1.Name);
			while (BewebData.FieldExists(table1.Name, propertyName)) {
				propertyName = propertyName + "Records";
			}
			result = result.Replace("xxxpropertynamexxx", propertyName);

			string listName = GetPropertyName(table1.Name.Plural(), table2.Name);
			while (BewebData.FieldExists(table2.Name, listName)) {
				listName = listName + "Records";
			}
			result = result.Replace("xxxlistnamexxx", listName);

			return result;
		}

		////////////////////////////////////////////

		/// <summary>
		/// See which columns are foreign keys
		/// </summary>
		private void DiscoverForeignKeyLinks(Table table) {
			string result = "";
			if (GenForeignKeys) {
				foreach (var column in table.Columns) {
					if (!column.isPrimaryKey) {
						DiscoverForeignKeyLinks(table, column);
					}
				}
			}
		}

		private void DiscoverForeignKeyLinks(Table table, Column column) {
			string foreignTableNameString = GetForeignKeyTableName(column);
			if (foreignTableNameString != null) {
				string foreignIDFieldName = "";
				string foreignTableName = foreignTableNameString;

				var otherTable = tables.Find(t => t.Name.ToLower() == foreignTableName.ToLower());
				if (otherTable == null && foreignTableName.Contains(".")) {
					foreignIDFieldName = foreignTableName.RightFrom(".");
					foreignTableName = foreignTableName.LeftUntilLast(".");
				}
				otherTable = tables.Find(t => t.Name.ToLower() == foreignTableName.ToLower());
				if (otherTable == null) {
					throw new ActiveRecordException("Foreign table not included in generated models: " + foreignTableNameString);
				}
				if (foreignIDFieldName == "") {
					foreignIDFieldName = otherTable.PrimaryKey.fieldName;
				}
				column.foreignTableName = foreignTableName;
				column.foreignTableFieldName = foreignIDFieldName;
				column.foreignIDPropertyName = otherTable.Columns.Find(c => c.fieldName == foreignIDFieldName).GetPropertyName();
				column.foreignClassName = otherTable.ClassName;

				// the propertyName is the name of the model object property (eg Product) ie the new property being generated
				string propertyName = column.fieldName.RemoveSuffix("_id");
				propertyName = propertyName.RemoveSuffix("Code");
				propertyName = propertyName.RemoveSuffix("ID");
				propertyName = GetPropertyName(propertyName, column.tableName);
				while (BewebData.FieldExists(column.tableName, propertyName)) {
					propertyName = propertyName + "Record";
				}
				column.foreignRecordPropertyName = propertyName;
			}
		}

		/// <summary>
		/// See if column is a foreign key, and if so, run foreign key collection template
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private string CheckForForeignKey(Table table, Column column) {
			string result = "";
			if (GenForeignKeys) {
				if (column.foreignTableName.IsNotBlank()) {
					result += GetForeignKeyTemplate(column);
				}
			}
			return result;
		}

		private string GetForeignKeyTableName(Column column) {
			// very first check custom matches
			// pattern is like fieldname=>foreigntablename or table.fieldname=>foreigntablename
			// examples: 
			// <add key="SavvyActiveRecord_GenForeignKeysCustom" value="Product.CID=>Category|CatID=>Category|TrackPeng.dbo.Job.Client=>TrackPeng.dbo.Company|ClientCode=>TrackPeng.dbo.Company|Job=>TrackPeng.dbo.Job"/>
			var specificPattern = GenForeignKeysCustom.Find(p => p.StartsWith(column.tableName + "." + column.fieldName + "=>"));
			if (specificPattern != null) {
				return specificPattern.Split("=>")[1];
			}
			var fieldNamePattern = GenForeignKeysCustom.Find(p => p.StartsWith(column.fieldName + "=>"));
			if (fieldNamePattern != null) {
				return fieldNamePattern.Split("=>")[1];
			}

			// first check for exact match (eg CategoryID)
			foreach (Table otherTable in tables) {
				if (column.fieldName == otherTable.Name + "ID" && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName == otherTable.Name + "Id" && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName == otherTable.Name.RemoveSuffix("s") + "ID" && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName == otherTable.Name.RemoveSuffix("s") + "Id" && column.dbType == "int") {
					return otherTable.Name;
				}
				// app specific check for cruise ID
				if (column.fieldName == otherTable.Name.RemovePrefix("Cruise_").RemoveSuffix("s") + "_id" && column.dbType == "int") {
					return otherTable.Name;
				}
			}

			// then parent match (eg ParentCategoryID)
			foreach (Table otherTable in tables) {
				if (column.fieldName.Contains("Parent" + otherTable.Name + "ID") && column.dbType == "int") {
					return otherTable.Name;
				}
			}

			// then partial match (eg PreviousCategoryID or CategoryID2)
			foreach (Table otherTable in tables) {
				if (column.fieldName.Contains(otherTable.Name + "ID") && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName.Contains(otherTable.Name + "Id") && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName.Contains(otherTable.Name + "sID") && column.dbType == "int") {
					return otherTable.Name;
				}
				if (column.fieldName.Contains(otherTable.Name + "sId") && column.dbType == "int") {
					return otherTable.Name;
				}
			}
			return null;
		}

		public string GetForeignKeyTemplate(Column column) {
			string path = templatePath + "ForeignKey.template-cs";
			string result = FileSystem.GetFileContents(path, true);
			result = result.Replace("xxxtablenamexxx", column.tableName);

			string tableName = column.Table.Name;
			string fieldName = column.fieldName;
			string tableClassName = column.Table.ClassName;


			result = result.Replace("xxxtableclassnamexxx", tableClassName);
			result = result.Replace("xxxtablenamepluralxxx", tableClassName.Plural());
			result = result.Replace("xxxforeigntablenamexxx", column.foreignTableName);
			result = result.Replace("xxxforeignclassnamexxx", column.foreignClassName);

			result = result.Replace("xxxidfieldnamexxx", column.foreignTableFieldName);
			result = result.Replace("xxxidpropertynamexxx", column.foreignIDPropertyName);

			string pluralTable = tableName.Plural();

			//if (BewebData.TableExists(pluralTable)) {
			// can't use this as an identifier, have to add a 1
			//throw new Exception("GetForeignKeyTemplate: table named [" + pluralTable + "] already exists");
			//	pluralTable += "1";
			//}

			//result = result.Replace("xxxtablenamepluralxxx", pluralTable);  -- not currently used in template

			// the fieldname is actually the property name of the ID property (eg ProductID)
			result = result.Replace("xxxfieldnamexxx", column.GetPropertyName());
			if (column.isNullable && column.type != typeof(string)) {
				result = result.Replace("xxxfieldvaluexxx", column.GetPropertyName() + ".Value");
			} else {
				result = result.Replace("xxxfieldvaluexxx", column.GetPropertyName());
			}
			result = result.Replace("xxxdbfieldnamexxx", fieldName);

			// the propertyName is the name of the model object property (eg Product) ie the new property being generated
			result = result.Replace("xxxpropertynamexxx", column.foreignRecordPropertyName);

			// get first part of fk field name if it is not just the same as the pk name and put it at end
			// (eg for field name Page.UpdatedByAdministratorID the list would be called Administrator.PagesUpdatedByAdministrator)
			// (eg for field name Product.SecondaryCategoryID the list would be called Category.ProductsSecondary)
			// (eg for field name Product.CategoryID2 the list would be called Category.Products2)
			// (exception for "parent" eg for field name Category.ParentCategoryID the list would be called Category.ChildCategories)
			string extraDescriptor = fieldName.Replace(column.foreignTableName + "ID", "");
			string listName;
			if (extraDescriptor == "Parent") {
				listName = "Child" + pluralTable;
			} else {
				listName = pluralTable + extraDescriptor;  // fieldName.RemoveCharsFromEnd(2).Plural();
			}
			listName = GetPropertyName(listName, column.foreignTableName);
			result = result.Replace("xxxlistnamexxx", listName);

			return result;
		}

		public string GetMainTemplate() {
			string path = templatePath + "Main.template-cs";
			return FileSystem.GetFileContents(path, true);
		}

		public void AddTable(string tableName, bool isView) {
			AddTable(tableName, isView, null);
		}

		/// <summary>
		/// add new table or view, if SqlConnectionString is null, get it automatically
		/// </summary>
		/// <param name="tableName">name to create</param>
		/// <param name="isView">false unless view</param>
		/// <param name="sqlConnectionString">if SqlConnectionString is null, get it automatically</param>
		public void AddTable(string tableName, bool isView, string sqlConnectionString) {
			var table = new Table(tableName, isView, sqlConnectionString);
			tables.Add(table);
		}

		private string GetUserCodeTableTemplate(Table table) {
			string path = templatePath + "UserCodeTable.template-cs";
			string result = FileSystem.GetFileContents(path, true);
			result = result.Replace("xxxtablenamexxx", table.Name);
			result = result.Replace("xxxtableclassnamexxx", table.ClassName);
			result = result.Replace("xxxtablenamepluralxxx", table.ClassName.Plural());
			result = result.Replace("xxxcreatedatexxx", Fmt.DateTime(DateTime.Now, Fmt.DateTimePrecision.Second));

			string columnDefaults = "";
			foreach (var column in table.Columns) {
				string defaults = GetColumnDefaults(table, column);
				if (defaults.IsNotBlank()) {
					columnDefaults += defaults + "\r\n";
				}
			}

			result = result.Replace("xxxdefaultsxxx", columnDefaults);
			return result;
		}

		private string GetTableTemplate(Table table) {
			string path = templatePath + "Table.template-cs";
			string result = FileSystem.GetFileContents(path, true);
			result = result.Replace("xxxtableclassnamexxx", table.ClassName);
			result = result.Replace("xxxtablenamepluralxxx", table.ClassName.Plural());

			// get all col names and metadata
			string columnNames = "";
			string columnObjects = "";

			foreach (var column in table.Columns) {
				if (columnNames != "")
					columnNames += ",";
				columnNames += column.fieldName;

				// build columnObjects string
				columnObjects += GetColumnInit(column) + "\r\n";
			}

			if (table.pkType != "int") {
				// remove cache for non-int primary keys
				result = result.Replace("public int ID", "//public int ID");
				//result = result.Replace("record = xxxtableclassnamexxx.GetFromCache", "//record = xxxtableclassnamexxx.GetFromCache(id");

				//result = result.Replace("record = xxxtablenamexxx.", "//var record = xxxtablenamexxx.");
				//result = result.Replace("if (GetFromCache(this.xxxpknamexxx)", "//if (GetFromCache(this.xxxpknamexxx)");
				result = result.Replace("public static xxxtableclassnamexxxList LoadIDs(xxxpktypexxx[] ids)", "//public static xxxtableclassnamexxxList LoadIDs(xxxpktypexxx[] ids)");

				result = result.Replace("var sql = new Sql(\"where xxxpknamexxx=\", this.xxxpknamexxx);", "var sql = new Sql(\"where xxxpknamexxx=\", this.xxxpknamexxx.Sqlize_Text());");
			}
			result = result.Replace("xxxisviewxxx", table.IsView.ToString().ToLower());
			result = result.Replace("xxxpknamexxx", table.PrimaryKey.fieldName);
			// if the pk is ID, there will be a comflict with the model ID shortcut, so comment out the shortcut in the generated source
			if (table.pkName == "ID" || (table.pkName.ToUpper() == "ID" && !AllowMessyNames)) {
				result = result.Replace("public xxxpktypexxx ID", "//ID already exists -- public xxxpktypexxx ID");
				result = result.Replace("public int ID", "//ID already exists -- public int ID");
			}
			//these must be after the above replacements
			result = result.Replace("xxxtablenamexxx", table.Name);
			result = result.Replace("xxxpktypexxx", table.PrimaryKey.cSharpType);
			result = result.Replace("xxxcolumnnamesxxx", columnNames);
			result = result.Replace("xxxcolumnobjectsxxx", columnObjects);

			result += RunColumnTemplates(table);

			result += RunManyToManyTemplates(table);

			return result;
		}

		private string GetColumnDefaults(Table table, Column column) {
			string result = "";
			if (column.fieldName.Equals("DateAdded") || column.fieldName.Equals("CreateDate") || column.fieldName.Equals("DateCreated")) {
				result += "			" + column.GetPropertyName() + " = DateTime.Now;\r\n";
			}
			if (column.fieldName.Equals("PublishDate") || column.fieldName.Equals("DatePublished")) {
				result += "			" + column.GetPropertyName() + " = DateTime.Today;\r\n";
			}
			if (column.fieldName.Equals("SortPosition")) {
				result += "			" + column.GetPropertyName() + " = 50;\r\n";
			}
			if (column.fieldName.Equals("IsActive") || column.fieldName.Equals("IsPublished")) {
				result += "			" + column.GetPropertyName() + " = true;\r\n";
				// todo - perhaps include this code below (not sure yet)
				//} else if (column.cSharpType=="System.Boolean" && !AllowNullableBooleans) {
				//	result += "			"+column.GetPropertyName()+" = false;\r\n";
			}
			if (column.activeFieldType == "PictureActiveField") {
				result += "			Fields." + column.GetPropertyName() + ".MetaData = GetPicMetaData();\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData = new DefaultPictureMetaData();\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.IsExact = false;\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.IsCropped = false;\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.Width = 300;\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.Height = 300;\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.ThumbnailWidth = 100;\r\n";
				//result += "			Fields." + column.GetPropertyName() + ".MetaData.ThumbnailHeight = 100;\r\n";
				//result += "			throw new Exception(\"Specify picture size. Table [" + table.Name + "] Field [" + column.GetPropertyName() + "]  \"); //Remove this line once specified \r\n";
			}

			return result;
		}

		private static string MakeClassName(string tableName) {
			string result = tableName + "";
			result = result.Replace(".", "_");  //.RightFrom(".");
			result = result.Replace("dbo_", "");  //.RightFrom(".");
			if (ReformatTableNames) {
				if (result == result.ToLower() || result.Contains("_")) result = result.PascalCase();
			}
			return result;
		}



		public static string GetColumnInit(Column column) {
			string result = "";
			string foreignKeyStuff = "";
			if (column.foreignTableFieldName.IsNotBlank()) {
				foreignKeyStuff = ", GetForeignRecord = () => this." + column.foreignRecordPropertyName + ", ForeignClassName = typeof(Models." + column.foreignClassName + "), ForeignTableName = \"" + column.foreignTableName + "\", ForeignTableFieldName = \"" + column.foreignTableFieldName + "\"";
			}
			result += "\r\n\tfields.Add(\"" + column.fieldName + "\", new " + column.activeFieldType + "() { Name = \"" + column.fieldName + "\", ColumnType = \"" + column.dbType + "\", Type = typeof(" + column.cSharpType + "), IsAuto = " + column.isAuto.ToString().ToLower() + ", MaxLength=" + column.maxLength + ((column.decimalPlaces.HasValue) ? ", DecimalPlaces=" + column.decimalPlaces : "") + ", TableName=\"" + column.tableName + "\" " + foreignKeyStuff + " });";

			return result;
		}

		protected static string GetActiveFieldType(string cSharpType, string fieldName) {
			var result = "ActiveField<" + cSharpType + ">";
			if (cSharpType == "string" && (fieldName.Contains("Picture") || fieldName.Contains("Image")) && !fieldName.Contains("Caption") && !fieldName.Contains("Width") && !fieldName.Contains("Height")) {
				result = "PictureActiveField";
			}
			if (cSharpType == "string" && fieldName.Contains("Attachment")) {
				result = "AttachmentActiveField";
			}
			return result;
		}

		public static string GetPrimaryKey(string tableName, DataTable schemaTable) {
			// determine most probable primary key using heuristics - this may need some tweaking

			// look for a column that ADO recognises as either auto, identity or key
			// this seems a bit flaky - ie data sometimes not populated, may depend on ADO provider
			// To investigate - MSDN notes: make sure that metadata columns return the correct information, you must call ExecuteReader with the behavior parameter set to KeyInfo

			try //need this incase IsKey is not in the collection
			{
				// first check autoincrement columns - we typically add these as a surrogate PK in cases where there is a legacy database with multi-column primary keys (as ActiveRecord does not support multi-column primary keys)
				foreach (DataRow row in schemaTable.Rows) {
					if (!(bool)row["IsHidden"]) {
						if ((bool)row["IsAutoIncrement"] || (bool)row["IsIdentity"]) {
							return row["ColumnName"].ToString();
						}
					}
				}

				// next check actual primary keys
				foreach (DataRow row in schemaTable.Rows) {
					if (!(bool)row["IsHidden"]) {
						if (row["IsKey"].ToString() != "") {
							if ((bool)row["IsKey"]) {
								return row["ColumnName"].ToString();
							}
						}
					}
				}
			} catch (Exception) { }

			// look for column named tablename+ID (also "_id")
			foreach (DataRow row in schemaTable.Rows) {
				if (!(bool)row["IsHidden"]) {
					string fieldName = row["ColumnName"].ToString();
					if (fieldName == tableName + "ID" || fieldName == tableName + "Id" || fieldName == tableName + "_id") {
						return fieldName;
					}
				}
			}

			// default to first column in table
			return schemaTable.Rows[0]["ColumnName"].ToString();
		}

		//public static string GetColumnTemplate(string tableName, string fieldName, string dbType, bool isNullable, int? maxLength, bool isPrimaryKey)
		public string GetColumnTemplate(Table table, Column column) {
			string tableName = table.Name;
			bool isPrimaryKey = column.isPrimaryKey;
			string propertyName = GetPropertyName(column.fieldName, column.tableName);
			string dbTypeDeclaration = column.GetDbTypeDeclaration();

			string otherAttributes = "";
			if (isPrimaryKey) {
				otherAttributes += ", IsPrimaryKey = true, AutoSync = AutoSync.OnInsert";       // not sure if AutoSync thing should be only if Auto
			}
			if (column.isAuto) {
				otherAttributes += ", IsDbGenerated = true";
			}

			// error check
			if (isPrimaryKey && column.isNullable && !table.IsView) {
				throw new Exception("Beweb.ActiveRecordGenerator: You can't have a Primary Key which allows NULLs. Please correct the database (you may have missed an IDENTITY). The field is [" + column.fieldName + "] in table [" + tableName + "].");
			}

			string path = templatePath + "Column.template-cs";
			string result = FileSystem.GetFileContents(path, true);
			result = result.Replace("xxxtablenamexxx", tableName);
			string tableClassName = MakeClassName(tableName);
			result = result.Replace("xxxtableclassnamexxx", tableClassName);
			result = result.Replace("xxxtablenamepluralxxx", tableClassName.Plural());

			result = result.Replace("xxxfieldnamexxx", column.fieldName);
			result = result.Replace("xxxpropertynamexxx", propertyName);
			result = result.Replace("xxxpropertynamecamelxxx", propertyName.LowerCaseFirstLetter());
			result = result.Replace("xxxfieldnamecamelxxx", column.fieldName.LowerCaseFirstLetter());

			result = result.Replace("xxxtypexxx", column.cSharpType);
			result = result.Replace("xxxactivefieldtypexxx", column.activeFieldType);

			result = result.Replace("xxxdbtypexxx", dbTypeDeclaration);
			result = result.Replace("xxxotherattributesxxx", otherAttributes);
			result = result.Replace("xxxmaxlenxxx", column.maxLength + "");

			if (!isPrimaryKey) {
				result += CheckForForeignKey(table, column);
			}

			return result;
		}

		public static string GetPropertyName(string fieldName, string tableClassName) {
			string result = fieldName + "";

			// cannot contain dots - grab the part after the last dot (for example "mydb.dbo.customer"+"ID" => "customerID")
			result = result.RightFrom(".");   //Replace(".", "_");
			//result = result.Replace("dbo_", "");  //.RightFrom(".");

			// format nicely according to convention
			if (!AllowMessyNames) {
				if (fieldName == fieldName.ToLower()) {
					// all lowercase, so reformat to PascalCase
					result = result.PascalCase();
				}
				if (result.EndsWith("Id")) result = result.ReplaceLast("Id", "ID");
				// always begin public properties with a capital letter
				fieldName = fieldName.UpperCaseFirstLetter();
			}

			// alter if it is a reserved word etc

			// member names cannot be the same as their enclosing type
			if (result == tableClassName) {
				if (!ThrowExceptionOnFieldNameSameAsClass) {
					result = fieldName + "1";
				} else {
					//throw new BewebDataException("member names (column) cannot be the same as their class (table) [" + tableClassName + "]. To override this, set GenerateModelsAddSuffixToFieldsThatHaveSameNameAsTable true in Web_AppSettings.config");
					throw new BewebDataException("member names (column) cannot be the same as their class (table) [" + tableClassName + "]. To override this, set ThrowExceptionOnFieldNameSameAsClass true in BewebCoreSettings");
				}
				//
				// TODO: need a method to ensure it is unique - ie change to 2 if already a field of that name
				//} else if (fieldName.ToLower()=="id") {
				//  // we already have a standard generated property called ID, so lets call this one "id" lowercase -- not needed, now commenting out ID
				//  //result = "ID_savvy_internal"; not such a great idea
				//  result = "id";
			}
			return result;
		}

		public class Table {
			public readonly string Name;
			public List<Column> Columns = new List<Column>();
			public string pkName;
			public string pkType;
			public Column PrimaryKey;
			public readonly bool IsView;
			public string ClassName;

			public Table(string tableName, bool isView) : this(tableName, isView, null, null) { }
			public Table(string tableName, bool isView, String sqlConnectionString) : this(tableName, isView, null, sqlConnectionString) { }
			public Table(string tableName, bool isView, DbDataReader existingDataReader) : this(tableName, isView, existingDataReader, null) { }

			public Table(string tableName, bool isView, DbDataReader existingDataReader, String sqlConnectionString) {
				this.Name = tableName;
				this.IsView = isView;

				Init(existingDataReader, sqlConnectionString);
			}

			public void Init(DbDataReader existingDataReader, String sqlConnectionString) {
				DbDataReader tableReader;
				string tableName = Name;
				if (existingDataReader == null) {
					var sql = new Sql();
					if (sqlConnectionString != null) sql.SqlConnectionString = sqlConnectionString;
					sql.Add("select top 1 * from ", tableName.SqlizeName(), "where 0=1");
					tableReader = sql.GetReader();
				} else {
					tableReader = existingDataReader;
				}
				DataTable schemaTable = tableReader.GetSchemaTable();

				pkName = GetPrimaryKey(tableName, schemaTable);
				pkType = "";

				int columnIndex = 0;
				foreach (DataRow row in schemaTable.Rows) {
					if (!(bool)row["IsHidden"]) {
						bool isPrimaryKey = (row["ColumnName"].ToString() == pkName);
						var column = new Column(tableName, row, isPrimaryKey, tableReader.GetFieldType(columnIndex), this);
						if (column.isPrimaryKey) {
							pkType = row["DataType"].ToString();
							PrimaryKey = column;
						}
						Columns.Add(column);
						columnIndex++;
					}
				}
				if (existingDataReader == null) {
					tableReader.Close();
					tableReader.Dispose();
				}

				// record the name for this class (usually the table name)
				ClassName = MakeClassName(tableName);
			}

			/// <summary>
			/// Returns true if this table name is in the supplied pipe-delimited string, with special case (all) to indicate all tables in the default databse.
			/// </summary>
			/// <param name="pipeSeparatedTablesString"></param>
			/// <returns></returns>
			public bool IsInPipeList(string pipeSeparatedTablesString) {
				return (pipeSeparatedTablesString == "(all)" && IsInDefaultDatabase) || pipeSeparatedTablesString.Replace("|", ",").ContainsCommaSeparated(Name);
			}

			/// <summary>
			/// Tables outside the default database can also be generated, using dot prefixes. These tables are not counted in (all).
			/// </summary>
			protected bool IsInDefaultDatabase {
				get {
					return Name.DoesntContain(".");
				}
			}

			public bool FieldExists(string fieldName) {
				return Columns.Exists(c => c.fieldName.ToLower() == fieldName.ToLower());
			}

		}

		public class Column {
			public readonly string tableName;
			public readonly string fieldName;
			public readonly string dbType;
			public readonly bool isNullable;
			public readonly int maxLength;
			public readonly int? decimalPlaces;
			public readonly bool isAuto;
			public readonly string cSharpType;
			public readonly Type type;
			public readonly string activeFieldType;
			public readonly bool isPrimaryKey;
			public Table Table;
			public string foreignTableName;
			public string foreignTableFieldName;
			public string foreignIDPropertyName;
			public string foreignClassName;
			public string foreignRecordPropertyName;

			public Column(string tableName, DataRow row, bool isPrimaryKey, Type fieldType, Table table) {
				this.tableName = tableName;
				this.isPrimaryKey = isPrimaryKey;
				this.Table = table;
				fieldName = row["ColumnName"].ToString();
				dbType = row["DataTypeName"].ToString();
				isNullable = (bool)row["AllowDBNull"];
				maxLength = (int)row["ColumnSize"];
				decimalPlaces = null;
				isAuto = (bool)row["IsAutoIncrement"] || (bool)row["IsIdentity"];

				// determine c# type and activefield type
				cSharpType = row["DataType"].ToString();
				if (cSharpType == "System.Boolean") {
					cSharpType = "bool";
				} else if (cSharpType == "System.Int32") {
					cSharpType = "int";
				} else if (cSharpType == "System.String") {
					cSharpType = "string";
				} else if (cSharpType == "System.Decimal") {
					if (dbType == "decimal") {
						decimalPlaces = (row["NumericScale"] + "").ToInt(null);						// NumericPrecision
					}
					cSharpType = "decimal";
				} else if (cSharpType == "System.Double") {
					cSharpType = "double";
					//} else if (cSharpType=="System.Single") {              // no - actually there is no such thing
					//	cSharpType = "single";
				} else if (cSharpType == "System.Byte") {
					cSharpType = "byte";
				} else if (cSharpType == "System.Byte[]") {
					cSharpType = "byte[]";
				}
				if (cSharpType == "bool" && !AllowNullableBooleans) {
					isNullable = false;
				}
				if (isNullable && cSharpType != "System.String" && cSharpType != "string" && cSharpType != "byte[]") {
					//cSharpType = "System.Nullable<" + cSharpType + ">";
					cSharpType = cSharpType + "?";
					try {
						fieldType = typeof(Nullable<>).MakeGenericType(fieldType);
					} catch {
						// we don't care - if it can't be made nullable it will just be a non-nullable type, which is fine
					}
				}
				activeFieldType = GetActiveFieldType(cSharpType, fieldName);

				type = fieldType;
			}

			public string GetPropertyName() {
				return ActiveRecordGenerator.GetPropertyName(fieldName, tableName);
			}

			public string GetDbTypeDeclaration() {
				// construct full db type spec (eg INT NOT NULL)
				string dbTypeDeclaration = dbType;
				if (isPrimaryKey && dbType == "int") {
					dbTypeDeclaration += " IDENTITY";
				}
				if (!isNullable) {
					dbTypeDeclaration += " NOT NULL";
				}
				return dbTypeDeclaration;
			}

			public static string GetCSharpTypeName(string fieldName, string tableName, string databaseColumnType, bool isNullable) {
				switch (databaseColumnType) {
					case "varchar":
					case "nvarchar":
					case "text":
					case "ntext":
					case "char":
					case "nchar":
						return "string";

					case "int":
						return isNullable ? "int?" : "int";

					case "smallint":
						return isNullable ? "System.Nullable<System.Int16>" : "System.Int16";

					case "tinyint":
						return isNullable ? "System.Nullable<System.Byte>" : "System.Byte";

					case "bigint":
						return isNullable ? "System.Nullable<System.Int64>" : "System.Int64";

					case "money":
					case "smallmoney":
					case "decimal":
					case "numeric":
						return isNullable ? "decimal?" : "decimal";

					case "real":
						return isNullable ? "single?" : "single";

					case "float":
						return isNullable ? "double?" : "double";

					case "bit":
						return isNullable ? "bool?" : "bool";

					case "date":
					case "datetime":
						return isNullable ? "System.Nullable<System.DateTime>" : "System.DateTime";

					case "datetimeoffset":
						return isNullable ? "System.Nullable<System.DateTimeOffset>" : "System.DateTimeOffset";

					case "time":
						return isNullable ? "System.Nullable<System.TimeSpan>" : "System.TimeSpan";

					case "uniqueidentifier":
						return isNullable ? "System.Nullable<System.Guid>" : "System.Guid";

					case "binary":
						return "byte[]";

					default:
						throw new Exception("GenLinq: can not handle Database Column Type [" + databaseColumnType + "] Table[" + tableName + "] Field[" + fieldName + "]");
					//return "string";

				}
			}

		}

		public static List<string> RunSingleTable(string singleTableName) {
			var generator = new ActiveRecordGenerator();
			generator.tables = new List<Table>();
			generator.AddTable(singleTableName, false);
			generator.RunAllTemplates();
			return generator.createdFilenames;
		}

		public static List<string> RunTables(params string[] tableNames) {
			var generator = new ActiveRecordGenerator();
			generator.tables = new List<Table>();
			foreach (string tableName in tableNames) {
				if (tableName.IsNotBlank()) {
					generator.AddTable(tableName, false);
				}
			}
			generator.RunAllTemplates();
			return generator.createdFilenames;
		}

	}

	public class ActiveRecordDatabaseGeneratorReport {
		public List<string> NewTables;
		public Dictionary<string, List<string>> NewColumns;
		public bool HasValues = false;
	}

	public class ActiveRecordDatabaseGenerator {

		public static void CreateAnyMissingFields() {
			var sql = new Sql();
			sql.Value = GetSqlStatements();
			sql.Execute();

			// todo: now check for any diffs and show warnings
		}

		public static ActiveRecordDatabaseGeneratorReport CreateAnyMissingFieldsWithReport() {
			var report = new ActiveRecordDatabaseGeneratorReport();
			var tablesBefore = GetAllTablesAndColumns();

			var sql = new Sql { Value = GetSqlStatements() };
			sql.Execute();

			var tablesAfter = GetAllTablesAndColumns();

			var newTables = new List<string>();
			var newColumns = new Dictionary<string, List<string>>();

			foreach (var tableAfter in tablesAfter) {

				var isNewTable = true;

				foreach (var tableBefore in tablesBefore) {
					if (tableBefore.Key == tableAfter.Key) {

						isNewTable = false;

						foreach (var tableAfterColumn in tableAfter.Value) {

							var isNewColumn = true;

							foreach (var tableBeforeColumn in tableBefore.Value) {
								if (tableAfterColumn == tableBeforeColumn) {
									isNewColumn = false;
									break;
								}
							}

							if (isNewColumn) {
								if (!newColumns.ContainsKey(tableAfter.Key)) {
									newColumns[tableAfter.Key] = new List<string>();
								}
								newColumns[tableAfter.Key].Add(tableAfterColumn);
								report.HasValues = true;
							}
						}
						break;
					}
				}

				if (isNewTable) {
					newTables.Add(tableAfter.Key);
					report.HasValues = true;
				}

			}

			report.NewTables = newTables;
			report.NewColumns = newColumns;
			return report;

		}

		private static Dictionary<string, List<string>> GetAllTablesAndColumns() {
			var models = GetModelInstances();
			var tables = new Dictionary<string, List<string>>();

			foreach (ActiveRecord model in models) {
				var tableName = model.GetTableName();
				tables[tableName] = new List<string>();

				foreach (var column in new Sql("SELECT [COLUMN_NAME] FROM information_schema.[COLUMNS] WHERE table_name = ", tableName.SqlizeText()).FetchStringList()) {
					if (!tables.ContainsKey(model.GetTableName())) {
						tables[model.GetTableName()] = new List<string>();
					}
					tables[model.GetTableName()].Add(column);
				}
			}

			return tables;
		}

		public static string GetSqlStatements() {
			var result = new StringBuilder();

			var models = GetModelInstances();

			foreach (ActiveRecord model in models) {
				result.AppendLine(model.GetSqlForCreate().Value);
			}
			if (result.ToString() == "") {
				throw new ActiveRecordException("There are no ActiveRecord model classes found in the current AppDomain.");
			}
			return result.ToString();
		}

		public static List<string> GetModelTableNames() {
			var result = new List<string>();
			var models = GetModelInstances();
			foreach (ActiveRecord model in models) {
				result.Add(model.GetTableName());
			}
			return result.OrderBy(s => s).ToList();
		}

		/// <summary>
		/// Deletes all data in the local database and copies it all down from the source server.
		/// For example to copy data from ziera live, call ImportDataFrom("LVE")
		/// BE VERY CAREFUL
		/// </summary>
		public static void ImportDataFrom(string sourceSrvCode) {
			ImportOrExportData(sourceSrvCode, true);
		}

		/// <summary>
		/// Deletes all data in the specified database and replaces it with the data in the local database.
		/// For example to copy to ziera staging server, call ExportDataTo("STG")
		/// BE VERY CAREFUL
		/// </summary>
		public static void ExportDataTo(string destSrvCode) {
			ImportOrExportData(destSrvCode, false);
		}

		private static void BackUpDatabase(string db, string name) {
			bool dobackup = Util.GetNamedSetting("BackupDatabasesOnDataCopy", "false").ToBool();
			BackUpDatabase(db, name, dobackup);
		}

		private static void BackUpDatabase(string db, string name, bool doBackup) {
			if (doBackup) {
				string timestamp = Fmt.DateTimeCompressed(DateTime.Now) + "s" + DateTime.Now.Second;
				string[] DBLocation = BewebData.GetValue("select top 1 filename from " + db + ".sys.sysfiles where filename like '%.mdf'").ToLower().Split(name.ToLower());
				string dbFileLocation = DBLocation[0]; // save it to the same folder as the mdb as other folders may not have permissions.
				string dbbakname = name + "_db_" + timestamp + ".bak";
				string dbbakFilePath = dbFileLocation + dbbakname;
				Sql dbSql = new Sql();
				dbSql.AddRawSqlString("BACKUP DATABASE " + db + " TO DISK ='" + dbbakFilePath + "' WITH FORMAT, MEDIANAME = '" + db + "_BACKUP', NAME = 'Full backup of the " + db + " database'");
				dbSql.Execute();
				if (BewebData.GetValue("SELECT recovery_model_desc FROM " + db + ".sys.databases where name = '" + name + "'") == "FULL") {
					string[] logLocation = BewebData.GetValue("select top 1 filename from " + db + ".sys.sysfiles where filename like '%.ldf'").ToLower().Split(name.ToLower());
					string logFileLocation = logLocation[0]; // save it to the same folder as the ldf as other folders may not have permissions.
					string logbakname = name + "_log_" + timestamp + ".bak";
					string logbakFilePath = logFileLocation + logbakname;
					Sql logSql = new Sql().AddRawSqlString("BACKUP LOG " + db + " TO DISK ='" + logbakFilePath + "'");
					logSql.Execute();
				}
			}

		}

		/// <summary>
		/// eg to import from live ImportOrExportData("LVE", true)
		/// </summary>
		private static void ImportOrExportData(string otherServerCode, bool isImport) {
			string remoteConnectionString = BewebData.GetConnectionString("ConnectionString" + otherServerCode);
			string remoteCatalog = remoteConnectionString.ExtractTextBetween("Initial Catalog=", ";").Trim();

			string localConnectionString = BewebData.GetConnectionString("ConnectionString" + Util.ServerIs());
			string localCatalog = localConnectionString.ExtractTextBetween("Initial Catalog=", ";").Trim();

			

			string otherServerDbName;
			if (Util.ServerIsDev) {
				// on dev server so we assume we will use the linked servers to import/export data
				var linkedServerDb = Util.GetSetting("LinkedServerDatabase" + otherServerCode, "throw");
				if (linkedServerDb.DoesntContain(".")) {
					linkedServerDb +=  "." + remoteCatalog;
				}
				otherServerDbName = linkedServerDb;
			} else {
				// on STG or LVE server so we assume we will use the linked servers to import/export data
				otherServerDbName = remoteCatalog;
			}

			if (isImport) {
				// local file
				BackUpDatabase("["+localCatalog+"]", localCatalog);
				//remote file /* Need to figure out a way to do this on the remote server - Service? wait for a response?*/
				//BackUpDatabase(otherServerDbName, remoteCatalog);
			}
			var scripts = GenerateDataCopyScripts(otherServerDbName, isImport);
			foreach (var script in scripts) {
				Web.Write(script + "<br>");
				//script.Execute(60*5);
			}
			Web.Write("DONE!<br>");
		}

		private static List<Sql> GenerateDataCopyScripts(string otherServerDbName, bool isImport) {
			var result = new List<Sql>();
			var models = GetModelInstances();
			var excludeTables = Util.GetSetting("DataCopyExcludeTables", "").Replace(",", "|").Split("|");
			foreach (ActiveRecord model in models) {
				if (model.GetIsView()) continue;
				var tableName = model.GetTableName();
				if (excludeTables.DoesntContain(tableName)) {
					tableName = tableName.SqlizeName().value;
					var fields = new DelimitedString(",");
					foreach (var field in model.GetFields()) {
						fields += field.Name.SqlizeName().value;
					}
					string sourceTable, destTable;
					bool truncAllowed = false;
					var sql = new Sql();
					if (isImport) {
						sourceTable = otherServerDbName + ".dbo." + tableName;
						destTable = tableName;
					} else {
						sourceTable = tableName;
						destTable = otherServerDbName + ".dbo." + tableName;
					}
					if (isImport) {
						sql.Add("truncate table " + destTable + ";");
					} else {
						// trunc is only allowed on local server, you seem to be not allowed to trunc tables on a linked server unless using RPC (so need RPC enabled in linked server config), or could just use delete
						sql.AddRawSqlString("exec " + otherServerDbName + ".dbo.sp_executesql N'truncate table " + tableName + "';");
						//						sql.Add("delete from " + destTable + ";");
					}
					result.Add(sql);
					sql = new Sql();
					if (isImport) {
						sql.AddRawSqlString("set identity_insert " + destTable + " on;");
					} else {
						sql.AddRawSqlString("exec " + otherServerDbName + ".dbo.sp_executesql N'set identity_insert " + destTable + " on';");
					}
					sql.Add("insert into " + destTable + " (" + fields + ") select " + fields + " from " + sourceTable + ";");
					if (isImport) {
						sql.AddRawSqlString("set identity_insert " + destTable + " off;");
					} else {
						sql.AddRawSqlString("exec " + otherServerDbName + ".dbo.sp_executesql N'set identity_insert " + destTable + " off';");
					}
					result.Add(sql);
				}
			}
			return result;
		}

		public static List<ActiveRecord> GetModelInstances() {
			var result = new List<ActiveRecord>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				try {
					foreach (Type type in assembly.GetTypes()) {
						if (type.Namespace == "Models" && type.IsSubclassOf(typeof(ActiveRecord)) && !type.ContainsGenericParameters) {
							ActiveRecord model = (ActiveRecord)Activator.CreateInstance(type);
							result.Add(model);
						}
					}
				} catch (System.Reflection.ReflectionTypeLoadException e) {
					// ignore
				}
			}
			return result;
		}

		public static ActiveRecord GetModelInstance(string className) {
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				try {
					foreach (Type type in assembly.GetTypes()) {
						if (type.Namespace == "Models" && type.IsSubclassOf(typeof(ActiveRecord)) && !type.ContainsGenericParameters && className == type.Name) {
							return (ActiveRecord)Activator.CreateInstance(type);
						}
					}
				} catch (System.Reflection.ReflectionTypeLoadException e) {
					// ignore
				}
			}
			return null;
		}

		public static ActiveRecord GetModelInstanceByTableName(string tableName) {
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				try {
					foreach (Type type in assembly.GetTypes()) {
						if (type.Namespace == "Models" && type.IsSubclassOf(typeof(ActiveRecord)) && !type.ContainsGenericParameters) {
							var rec = (ActiveRecord)Activator.CreateInstance(type);

							if (rec.GetTableName() == tableName) {
								return rec;
							}
						}
					}
				} catch (System.Reflection.ReflectionTypeLoadException e) {
					// ignore
				}
			}
			return null;
		}

		public static string GetFullTextColumns() {
			var fields = new Sql().AddRawSqlString("select distinct object_name(fic.[object_id]) + '.' + [name] column_name from sys.fulltext_index_columns fic inner join sys.columns c on c.[object_id] = fic.[object_id] and c.[column_id] = fic.[column_id] order by column_name").GetDelimited("|");
			return fields;
		}
	}

}
