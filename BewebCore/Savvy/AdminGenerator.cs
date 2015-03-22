using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;

namespace Savvy {
	public class AdminGenerator {

		// common properties
		protected static string connectionString = Beweb.BewebData.GetConnectionString();// ConfigurationManager.ConnectionStrings["ConnectionStringDEV"].ToString(); // we should only need CreateAdmin when on DEV right?
		protected string resultMessage = "";

		protected string targetfolder;
		protected string templateFolderName;
		protected bool overwrite = false;
		protected bool listpages = false;
		protected bool editpages = false;
		protected bool viewpages = false;
		protected bool exportpages = false;
		private List<string> createdFilenames;
		public string ChildTableUI = "subform";  // subform or link

		public AdminGenerator() {

		}

		public string Run(bool allpages, string singleTableName, string templateFolderName, string targetfolder, bool listpages, bool editpages, bool overwrite, string childTableName, bool viewpages, bool exportpages) {
			this.targetfolder = targetfolder;
			this.templateFolderName = templateFolderName;
			this.overwrite = overwrite;
			this.listpages = listpages;
			this.editpages = editpages;
			this.viewpages = viewpages;
			this.exportpages = exportpages;
			this.createdFilenames = new List<string>();

			var models = Beweb.BewebData.GetTableNames();
			if (!allpages) {
				if (singleTableName.IsNotBlank()) {
					// 20110629 MN - gen models does not work too well when generating only a single model as it does not include lazy load properties to other models
					//List<string> modelFileNames = ActiveRecordGenerator.RunTables(singleTableName, subformTableName);
					//createdFilenames.AddRange(modelFileNames);
					//resultMessage += "<br><b>GenModels</b> - model files generated: " + modelFileNames.Join(", ") + "<br>";
					resultMessage += "<br/><b>GenAdmin</b> - admin files generated:";
					CreateOne(singleTableName, childTableName);
					if (ChildTableUI == "link") {
						// also create the child controller and views
						CreateOne(childTableName);
					}
				} else {
					resultMessage = "Please select a table to generate.";
				}
			} else {
				//var models = Beweb.ActiveRecordDatabaseGenerator.GetModels();
				resultMessage = "Generating admin forms...<br>";
				foreach (string table in models) {
					resultMessage += "Table[" + table + "]<br>";
					CreateOne(table);

				}
			}

			if (templateFolderName == "pages_mvc") {
				if (this.createdFilenames.Count > 0 && UpdateProjectMakeFile()) {
					resultMessage += "<br/><b>Project Build File Updated</b> - updated project items in  Visual Studio site CSPROJ file.<br/>";
				} else {
					resultMessage += "<br/>Project Build File Skipped - no need to update project items in  Visual Studio site CSPROJ file.<br/>";
				}
			}

			return resultMessage;
		}

		private bool UpdateProjectMakeFile() {
			bool isUpdated = false;
			string projFilePath = Web.MapPath("~/Site.csproj");
			if (File.Exists(projFilePath)) {
				string projFileText = File.ReadAllText(projFilePath);

				string filesToCompile = "";
				foreach (var filename in this.createdFilenames) {
					string filepath = filename.TrimStart('~', '/', '\\');
					filepath = filepath.Replace("/", "\\");
					string itemType = "Compile";
					if (filepath.Contains("\\Views\\")) {
						itemType = "Content";
					}
					string itemIncludeLine = "<" + itemType + " Include=\"" + filepath + "\" />";
					if (!projFileText.Contains(itemIncludeLine)) {
						filesToCompile += itemIncludeLine + VB.crlf;
					}
				}

				if (filesToCompile != "") {
					projFileText = projFileText.ReplaceFirst("<ItemGroup>", "<ItemGroup>" + VB.crlf + filesToCompile);
					File.WriteAllText(projFilePath, projFileText);

					isUpdated = true;
				}
			}

			return isUpdated;
		}

		private void CreateOne(string tablename) {
			CreateOne(tablename, null);
		}

		protected void CreateOne(string tablename, string childTableName) {
			// get the information on the table selected
			DataSet ds = GetTableSchema(tablename);

			var activeRecord = ActiveRecordDatabaseGenerator.GetModelInstanceByTableName(tablename);
			if (activeRecord==null) {
				Web.WriteLine("Cannot find generated ActiveRecord model with table name "+tablename);
				Web.WriteLine("Running ActiveRecord Generator now... ");
				ActiveRecordGenerator.Run();
				Web.WriteLine(" .. done");
				Web.WriteLine("Please rebuild project in Visual Studio and then refresh this page.");
				Web.End();
			}
			activeRecord.InitDefaults();

			// create the list page
			if (listpages) CreateControllerAndListPage(ds, activeRecord, childTableName);

			// create the edit page
			if (editpages || viewpages) CreateEditAndViewPage(ds, activeRecord, ChildTableUI == "subform"?childTableName:null);
		}

		#region GetTableSchema
		/// <summary>
		/// 
		/// </summary>
		/// <param name="TableName"></param>
		/// <returns></returns>
		protected DataSet GetTableSchema(String TableName) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataSet;
			ds.ConnectionString = GetConnectionString();
			ds.SelectParameters.Add("table_name", TypeCode.String, TableName);
			ds.SelectCommand = "SELECT * FROM INFORMATION_SCHEMA.Columns WHERE table_name=@table_name ORDER BY ordinal_position";

			DataView view;
			try {
				view = (DataView)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetDataSet: " + e.Message + " in [" + ds.SelectCommand + "], TableName = [" + TableName + "]";
				throw new Exception(errorMessage);
			}

			// -----------------
			// to return a dataset we need to do this conversion
			DataTable table = view.ToTable();
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(table);
			// -----------------		
			ds.Dispose();
			return dataSet;
		}
		#endregion

		#region CreateListPage
		/// <summary>
		/// Creates the list page for the table
		/// </summary>
		/// <param name="schema"></param>

		private void CreateControllerAndListPage(DataSet schema, ActiveRecord activeRecord, string subformTableName) {
			DataTable schemaTable;
			String tableName;
			if (activeRecord!=null) {
				tableName = activeRecord.GetTableName();
			}else{
				schemaTable = schema.Tables[0];
				tableName=  schemaTable.Rows[0]["table_name"].ToString();
			}
			string orderByClause = "";
			string modelName = tableName;
			string pageTitle = tableName.SplitTitleCase();

			// we used to assume Primary Key is tableName + "ID", but now we can look it up
			using (var tableReader = new Sql("select top 1 * from ", tableName.SqlizeName()).GetReader()) {
				schemaTable = tableReader.GetSchemaTable();
			}
			string pkName = Beweb.ActiveRecordGenerator.GetPrimaryKey(tableName, schemaTable);
			if (activeRecord != null) {
				pkName = activeRecord.GetPrimaryKeyName();
				modelName = activeRecord.GetType().Name;
				pageTitle = activeRecord.GetFriendlyTableName();
			}

			string templateFileName, outputFileName;
			if (templateFolderName == "pages_mvc") {
				templateFileName = "Index.template-aspx";
				outputFileName = "Views/" + modelName + "Admin/" + modelName + "List.aspx";
			} else {
				templateFileName = "ListPage.template-aspx";
				outputFileName = modelName + "List.aspx";
			}
			// .aspx page
			string verifiedAspx = CheckTemplate(templateFolderName + "/" + templateFileName);
			if (verifiedAspx != "") {
				string newPage = File.ReadAllText(verifiedAspx);
				// build the bound fields
				StringBuilder boundFields = new StringBuilder("");
				StringBuilder modelheadercolumns = new StringBuilder("");
				StringBuilder modeldatacolumns = new StringBuilder("");
				int items = 0;
				bool hasPublishDate = false, hasExpiryDate = false, hasSortPosition = false, hasGeocodingResult = false;
				//foreach (DataRow dr in schemaTable.Rows) {
				foreach (var field in activeRecord.GetFields()) {
					//items++; if (items == 7) { modelheadercolumns.Append("\n<%--"); modeldatacolumns.Append("\n<%--"); boundFields.Append("\n<%--"); }
					string colName = field.Name;   //dr["column_name"].ToString()
					string dataType = field.ColumnType; // dr["data_type"].ToString();
					int maxLen = field.MaxLength; // dr["character_maximum_length"].ToInt(0);

					string colNameLowerCase = colName.ToLower();

					string propertyName = ActiveRecordGenerator.GetPropertyName(colName, tableName);
					ActiveFieldBase activeField = null;
					if (activeRecord.FieldExists(colName)) {
						activeField = activeRecord.GetFieldByName(colName);
					}


					if (colName == activeRecord.GetPrimaryKeyName()) {
						// colNameLowerCase == (tableName + "ID").ToLower() || colNameLowerCase == "id") //skip the id col
						continue; //skip the id col
					}
					/*
					if (colName.ToString().ToLower().EndsWith("id"))
					{
						continue; //skip some fks?
					}
					*/

					// MN 20140619 - moved exceptions to top so that col heads are correct, made exceptions for sortposition etc
					
					if (colNameLowerCase == "sortposition") {
						// skip it - we always draggable as last column below
						hasSortPosition = true;
						continue;
					}

					if (colNameLowerCase == "geocodingresult") {
						// skip it - add later
						hasGeocodingResult = true;
						continue;
					}

					if (dataType == "ntext" || (dataType == "nvarchar" && maxLen > 4000)) {
						// treat the same as ntext
						// too long to display in list, so skip it
						continue;
					}

					// ok, lets add it
					// if we have already added 7 columns, comment out the remaining ones
					items++; if (items == 7) { modelheadercolumns.Append("\n<%--"); modeldatacolumns.Append("\n<%--"); boundFields.Append("\n<%--"); }

					modelheadercolumns.AppendFormat(@"
				<td><%=dataList.ColSort(""{0}"",""{1}"")%></td>"
						 , colName
						 , SplitTitleCase(colName.ToString())
						);
				
					switch (dataType) {
						case "datetime":
						case "smalldatetime":
							//-----------------------------------------------------------
							// Date / Time field
							boundFields.AppendFormat(@"
						<asp:BoundField DataField=""{0}"" HeaderText=""{1}"" DataFormatString=""{{0:dd MMM yyyy HH:mm}}"" SortExpression=""{0}"" />"
								 , colName
								 , SplitTitleCase(colName.ToString())
								);
							modeldatacolumns.AppendFormat(@"
				<td><%=Fmt.Date(listItem.{0}) %></td>"
								 , propertyName
								);

							break;
						case "bit":
							//-----------------------------------------------------------
							// Yes/No
						{
								boundFields.AppendFormat(@"
							<asp:TemplateField HeaderText=""{1}"" SortExpression=""{0}"">
								<ItemTemplate>
									<asp:Literal Text=""Yes"" Visible='<%# Eval(""{0}"") %>' runat=""server"" />
									<asp:Literal Text=""No"" Visible='<%# !Convert.ToBoolean(Eval(""{0}"")) %>' runat=""server"" />
								</ItemTemplate>
							</asp:TemplateField>"
									 , colName
									 , SplitTitleCase(colName.ToString())
									);
								modeldatacolumns.AppendFormat(@"
				<td><%=Fmt.YesNo(listItem.{0}) %></td>"
									 , propertyName
									);

								break;
							}
						case "money":
						//-----------------------------------------------------------
						// Money field
						{
							boundFields.AppendFormat(@"
							<asp:BoundField DataField=""{0}"" HeaderText=""{1}"" DataFormatString=""{0:#,##0.00}"" ItemStyle-CssClass=""right"" SortExpression=""{0}"" />"
								 , colName
								 , SplitTitleCase(colName.ToString())
								);
							modeldatacolumns.AppendFormat(@"
				<td><%=listItem.{0}.FmtCurrency() %></td>"
								 , propertyName
								);
							break;
						}
						case "int":
						case "float":
						case "decimal":
						case "bigint":
						//-----------------------------------------------------------
						// int field
						{
							boundFields.AppendFormat(@"
							<asp:BoundField DataField=""{0}"" HeaderText=""{1}"" DataFormatString=""{0:#,##0.00}"" ItemStyle-CssClass=""right"" SortExpression=""{0}"" />"
								 , colName
								 , SplitTitleCase(colName.ToString())
								);
							if (colNameLowerCase == "sortposition") {
								modeldatacolumns.AppendFormat(@"
				<%=Html.DraggableSortPosition(listItem, listItem.{0}, null)%>"
									, propertyName
								);
							} else if (activeField != null && activeField.IsForeignKey) {
								// chcek if model exists
								modeldatacolumns.AppendFormat(@"
				<td><%:listItem.{0}!=null?listItem.{0}.GetName():""""%></td>"
									, propertyName.RemoveSuffix("ID")
								);
							} else {
								modeldatacolumns.AppendFormat(@"
				<td><%=Fmt.Number(listItem.{0})%></td>"
									, propertyName
								);
							}
							break;
						}
						case "ntext":
						case "text":
						//-----------------------------------------------------------
						// Rich text
						// do nothing - these normally have lots of text so we don't display on list pages by default
						break;
						default:
						if (colNameLowerCase.Contains("picture") && !colNameLowerCase.Contains("caption") && !colNameLowerCase.Contains("width") && !colNameLowerCase.Contains("height")) // don't want Picture1Caption fields!
						{
							//-----------------------------------------------------------
							// nvarchar, varchar and anything other normal field
							boundFields.AppendFormat(@"
					<asp:BoundField DataField=""{0}"" HeaderText=""{1}"" SortExpression=""{0}"" />"
								, colName
								, SplitTitleCase(colName.ToString())
								);
							/*
							throw new Exception("Not supported data format column_name[" + colName + "]data_type[" + dr["data_type"] + "]");
							 */
							modeldatacolumns.AppendFormat(@"
				<td><%=Beweb.Html.PicturePreview(listItem.Fields.{0})%></td>"
								 , propertyName
								);
						} else {
							//-----------------------------------------------------------
							// Picture
							boundFields.AppendFormat(@"
						<asp:TemplateField HeaderText=""{1}"">
							<ItemTemplate>
								<asp:Image ImageUrl='<%# Web.Attachments + Beweb.ImageProcessing.InsertSuffix(Eval(""{0}"").ToString(), ""_pv"") %>' Visible='<%# !String.IsNullOrEmpty(Eval(""{0}"").ToString()) %>' runat=""server"" />
							</ItemTemplate>
						</asp:TemplateField>"
									, colName
									, SplitTitleCase(colName.ToString())
									);
							modeldatacolumns.AppendFormat(@"
				<td><%:listItem.{0} %></td>"
								 , propertyName
								);
						}
						break;

					}
					if (colNameLowerCase == "sortposition") {
						orderByClause = " ORDER BY SortPosition ASC";
					}

					if (colNameLowerCase == "publishdate") {
						hasPublishDate = true;
					}
					if (colNameLowerCase == "expirydate") {
						hasExpiryDate = true;
					}


				}//end loop
				if (items > 6) { modelheadercolumns.Append("--%>"); modeldatacolumns.Append("--%>"); boundFields.Append("--%>"); }

				if (hasGeocodingResult) {
					modelheadercolumns.Append(@"
				<td><%=dataList.ColSort(""GeocodingResult"",""Map"")%></td>");
					modeldatacolumns.AppendFormat(@"
				<td><%=Fmt.GoodBad(listItem.GeocodingResult, ""Found|Manual"") %></td>");
				}
				if (hasPublishDate && hasExpiryDate) {
					modelheadercolumns.Append(@"
				<td><%=dataList.ColHead(""Status"")%></td>");
					modeldatacolumns.AppendFormat(@"
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>");
				}
				if (hasSortPosition) {
					modelheadercolumns.Append(@"
				<td><%=dataList.ColSort(""SortPosition"",""Order"")%></td>");
					modeldatacolumns.AppendFormat(@"
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>");					
				}

				newPage = newPage.Replace("[---pkname---]", pkName);
				newPage = newPage.Replace("[---tablename---]", tableName);
				newPage = newPage.Replace("[---modelname---]", modelName);
				newPage = newPage.Replace("[---pagetitle---]", pageTitle);
				newPage = newPage.Replace("[---boundfields---]", boundFields.ToString());
				newPage = newPage.Replace("[---modelheadercolumns---]", modelheadercolumns.ToString());
				newPage = newPage.Replace("[---modeldatacolumns---]", modeldatacolumns.ToString());
				newPage = newPage.Replace("[---orderbyclause---]", orderByClause);

				// subforms, child links etc
				newPage = MakeChildTableReplacements(subformTableName, newPage);

				WriteFile(tableName, outputFileName, newPage);
			}

			// .aspx.cs page
			if (templateFolderName == "pages_mvc") {
				templateFileName = "Controller.template-cs";
				outputFileName = "Controllers/" + modelName + "AdminController.cs";
			} else {
				templateFileName = "ListPage.template-aspx-cs";
				outputFileName = modelName + "List.aspx.cs";
			}
			string verifiedAspxCs = CheckTemplate(templateFolderName + "/" + templateFileName);
			if (verifiedAspxCs != "") {
				string newPage = File.ReadAllText(verifiedAspxCs);

				var manyToManyTables = GetManyToManyTables(tableName);
				foreach (KeyValuePair<string, string> manyToManyTable in manyToManyTables) {
					var joinTable = manyToManyTable.Key;
					var otherTable = manyToManyTable.Value;
					//todo: add update,save,delete to controller like for subforms
				}

				// subforms, child links etc
				newPage = MakeChildTableReplacements(subformTableName, newPage);

				// primary table
				newPage = newPage.Replace("[---pagetitle---]", pageTitle);
				newPage = newPage.Replace("[---tablename---]", tableName);
				newPage = newPage.Replace("[---modelname---]", modelName);
				newPage = newPage.Replace("[---orderbyclause---]", orderByClause);
				newPage = newPage.Replace("[---pkname---]", pkName);
				//newPage = newPage.Replace("[---primarykeyname---]", tableName + "ID");

				WriteFile(tableName, outputFileName, newPage);
			}
		}

		private string MakeChildTableReplacements(string subformTableName, string newPage) {
			if (subformTableName.IsNotBlank()) {
				if (ChildTableUI == "link") {
					newPage = newPage.Replace("//ifchildlink: ", "");
					newPage = newPage.Replace("//ifchildlink:", "");
					newPage = newPage.Replace("[---childlinktablenameplural---]", subformTableName.Plural());
				} else {
					newPage = newPage.Replace("//ifsubform: ", "");
					newPage = newPage.Replace("//ifsubform:", "");
					newPage = newPage.Replace("[---subformtablenameplural---]", subformTableName.Plural());
				}
				// in both cases
				newPage = newPage.Replace("//ifchild: ", "");
				newPage = newPage.Replace("//ifchild:", "");
				newPage = newPage.Replace("[---childtablenameplural---]", subformTableName.Plural());
			}
			// dumb down any remaining
			newPage = newPage.Replace("[---childlinktablenameplural---]", "example");
			newPage = newPage.Replace("[---subformtablenameplural---]", "example");
			newPage = newPage.Replace("[---childtablenameplural---]", "example");
			return newPage;
		}

		#endregion

		#region CreateEditPage

		/// <summary>
		/// creates an edit page for this table. Writing out specific controls for certain data types and naming conventions
		/// </summary>
		/// <param name="schema"></param>
		private void CreateEditAndViewPage(DataSet schema, ActiveRecord activeRecord, string subformTableName) {
			if (editpages) {
				CreateEditAndViewPage(schema, activeRecord, subformTableName, "Edit");
			}
			if (viewpages) {
				CreateEditAndViewPage(schema, activeRecord, subformTableName, "View");
			}
			if (exportpages) {
				CreateEditAndViewPage(schema, activeRecord, subformTableName, "Export");
			}
		}

		private void CreateSEOInfo(DataSet schema, string tableName) {
			var seoTableList = Util.GetSetting("SavvyActiveRecord_SEOTables", "(none)");
			DataTable dt = schema.Tables[0];
			//String tableName = dt.Rows[0]["table_name"].ToString();

			var list = seoTableList.ToLower().Split('|');
			if (list.Contains("(none)")) return;
			if (list.Contains("(all)") || list.Contains(tableName.ToLower())) {
				Sql sql = new Sql();

				//PageTitleTag
				//MetaKeywords
				//ALTER TABLE Page ADD [MetaDescription] ntext; 
				//ALTER TABLE Page ADD [MetaKeywords] nvarchar (250);
				//ALTER TABLE Page ADD [PageTitleTag] nvarchar (150); 
				if (!BewebData.FieldExists(tableName, "MetaDescription")) { sql.Add("ALTER TABLE ", tableName.SqlizeName(), " ADD [MetaDescription] ntext; 	"); }
				if (!BewebData.FieldExists(tableName, "MetaKeywords")) { sql.Add("ALTER TABLE ", tableName.SqlizeName(), " ADD [MetaKeywords] nvarchar (250); "); }
				if (!BewebData.FieldExists(tableName, "PageTitleTag")) { sql.Add("ALTER TABLE ", tableName.SqlizeName(), " ADD [PageTitleTag] nvarchar (150); "); }
				if (sql.ToString().IsNotBlank()) {
					sql.Execute();
				}
			}

		}

		/// <summary>
		/// creates an edit page for this table. Writing out specific controls for certain data types and naming conventions
		/// </summary>
		/// <param name="schema"></param>
		private void CreateEditAndViewPage(DataSet schema, ActiveRecord activeRecord, string subformTableName, string templatePageName) {
			bool isWebForms = (templateFolderName == "pages_webforms");
			DataTable dt = schema.Tables[0];
			String tableName;
			if (activeRecord!=null) {
				tableName = activeRecord.GetTableName();
			}else{
				tableName=  dt.Rows[0]["table_name"].ToString();
			}
			string modelName = tableName;
			string pageTitle = tableName.SplitTitleCase();
			if (pageTitle.Contains(".dbo.")) {
				pageTitle = pageTitle.RightFrom(".dbo.");
			}

			//get pkname
			DataTable schemaTable;
			using (var tableReader = new Sql("select top 1 * from ", tableName.SqlizeName()).GetReader()) {
				schemaTable = tableReader.GetSchemaTable();
			}
			string pkName = Beweb.ActiveRecordGenerator.GetPrimaryKey(tableName, schemaTable);
			if (activeRecord != null) {
				pkName = activeRecord.GetPrimaryKeyName();
				modelName = activeRecord.GetType().Name;
				pageTitle = activeRecord.GetFriendlyTableName();
			}

			// sets up variable to build non postback procedures in .cs file
			StringBuilder nonPostBackProcedures = new StringBuilder("");
			StringBuilder preSaveProcedures = new StringBuilder("");

			// .aspx page
			string orderByClause = "";

			string templateFileName, outputFileName;
			if (templateFolderName == "pages_mvc") {
				templateFileName = "" + templatePageName + ".template-aspx";
				outputFileName = "Views/" + modelName + "Admin/" + modelName + "" + templatePageName + ".aspx";
			} else {
				templateFileName = "" + templatePageName + "Page.template-aspx";
				outputFileName = modelName + "" + templatePageName + ".aspx";
			}
			string verifiedAspx = CheckTemplate(templateFolderName + "/" + templateFileName);
			if (verifiedAspx != "") {
				string newPage = File.ReadAllText(verifiedAspx);

				// loop through all the fields in our table
				StringBuilder editableFields = new StringBuilder("");
				StringBuilder modelEditableFields = new StringBuilder("");
				StringBuilder modelViewFields = new StringBuilder("");
				StringBuilder insertColumns = new StringBuilder("");
				StringBuilder insertValues = new StringBuilder("");
				StringBuilder updateColumnsAndValues = new StringBuilder("");
				StringBuilder warnings = new StringBuilder("");

//				foreach (DataRow dr in dt.Rows) {
				foreach (var field in activeRecord.GetFields()) {
					string colNameOriginal = field.Name;   //dr["column_name"].ToString()
					string colNameLower = field.Name.ToLower();   
					string propertyName = ActiveRecordGenerator.GetPropertyName(colNameOriginal, tableName);

					string dataType = field.ColumnType; // dr["data_type"].ToString();
					int maxLen = field.MaxLength; // dr["character_maximum_length"].ToInt(0);


					if (colNameOriginal == activeRecord.GetPrimaryKeyName()) {
						continue;
					}
					// don't do the ID field
					//if (colNameLower == tableName.ToLower() + "id" || colNameLower == "id") continue; // skip to the next one

					// Editable fields

					if (dataType == "nvarchar" && maxLen > 4000) {
						dataType = "ntext";  // treat the same as ntext
					}
					
					switch (dataType) {
						case "datetime":
						case "date":
							//-----------------------------------------------------------
							// Date / Time picker
							if (colNameLower == "publishdate" || colNameLower == "expirydate") {
								continue;  // just skip it because we now add these in the control at the bottom
							}
							if (colNameLower == "dateadded" || colNameLower == "datemodified") {
								continue;  // just skip it because we now add these in the control at the bottom

								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.DisplayField(record.Fields.{0})%></td>
			</tr>"
															, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
															, tableName.Trim().ToLower()
									);

								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%=record.{0}%></td>
			</tr>"
															, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
															, tableName.Trim().ToLower()
									);
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%# Eval(""{0}"", ""{{0:dd MMM yyyy HH:mm}}"") %><asp:HiddenField Value='<%# Bind(""{0}"", ""{{0:dd MMM yyyy HH:mm}}"") %>' ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);

							} else {
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><bwb:DatePicker DateValue='<%# Bind(""{0}"", ""{{0:dd MMM yyyy HH:mm}}"") %>' DisplayTime=""true"" ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);

								string comment = "";
								bool isRequired = true;
								if (colNameOriginal.ToString() == "PublishDate") {
									isRequired = false;
									comment = "  (blank = don\'t publish)";
								} else if (colNameOriginal.ToString() == "ExpiryDate") {
									isRequired = false;
									comment = "  (blank = don\'t expire)";
								}
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.DateField(record.Fields.{0}, {2}) %>{3}</td>
			</tr>"
												, propertyName
											, SplitTitleCase(colNameOriginal.ToString())
											, isRequired.ToString().ToLower()
											, comment
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0}.FmtDateTime() %>{3}</td>
			</tr>"
												, propertyName
											, SplitTitleCase(colNameOriginal.ToString())
											, isRequired.ToString().ToLower()
											, comment
									);
							}

							break;
						case "bit":
							if (colNameLower == "ispublished") {
								continue;  // just skip it because we now add these in the control at the bottom
							}
							//-----------------------------------------------------------
							// Check box
							editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}?</td>
				<td class=""field""><asp:CheckBox Checked='<%# Bind(""{0}"") %>' ID=""{0}"" runat=""server"" /></td>
			</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
								);

							modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%=new Forms.YesNoField(record.Fields.{0}, true) %></td>
			</tr>"
																, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
																, tableName.ToLower().Trim()
								);
							modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%=record.{0}.FmtYesNo() %></td>
			</tr>"
																, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
																, tableName.ToLower().Trim()
								);
							break;
						case "ntext":
						case "text":
							//-----------------------------------------------------------
							// Rich Text box
							if (colNameLower.IndexOf("html") != -1) {

								//-----------------------------------------------------------
								// Rich Text box - eg bodytexthtml
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><bwb:RichTextEditor Text='<%# Bind(""{0}"") %>' Rows=""10"" CssClass=""svyWideText"" ID=""{0}"" runat=""server"" /></td>
			</tr>"
								, colNameOriginal.ToString().Replace(" HTML", "")
								, SplitTitleCase(colNameOriginal.ToString())
								);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.RichTextEditor(record.Fields.{0} ,true) %></td>
			</tr>"
									, propertyName
									, SplitTitleCase(colNameOriginal.ToString().Replace("Html", ""))
									, tableName.ToLower().Trim()
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0}.FmtHtmlText() %></td>
			</tr>"
									, propertyName
									, SplitTitleCase(colNameOriginal.ToString().Replace("Html", ""))
									, tableName.ToLower().Trim()
									);

								//                            nonPostBackProcedures.AppendFormat(@"
								//                {0}.Text = {1}.{0};"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                );

								//                            preSaveProcedures.AppendFormat(@"
								//            {1}.{0} = {0}.Text;"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                );
							} else {
								//normal text box - eg introtext
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><asp:TextBox TextMode=""MultiLine"" Text='<%# Bind(""{0}"") %>' Rows=""5"" CssClass=""svyWideText"" ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.TextArea(record.Fields.{0} ,true) %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0}.FmtPlainTextAsHtml() %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);

							}
							break;
						case "money":
							//		//-----------------------------------------------------------
							//		// Money field
							editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field"">
					$ <asp:TextBox Text='<%# Bind(""{0}"", ""{{0:0.00}}"") %>' MaxLength=""30"" CssClass=""svyMediumText"" ID=""{0}"" runat=""server"" />
					<asp:RegularExpressionValidator ValidationExpression=""[0-9.]*"" ControlToValidate=""{0}"" ErrorMessage=""{1} can only contain numbers and a decimal"" SetFocusOnError=""true"" Display=""Dynamic"" runat=""server"" />
				</td>
			</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
								);
							modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field"">$<%= new Forms.MoneyField(record.Fields.{0}, true) %></td>
			</tr>"
								, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
								);
							modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field"">$<%= record.{0} %></td>
			</tr>"
								, propertyName
								, SplitTitleCase(colNameOriginal.ToString())
								);
							break;
						default:

							if (colNameLower.Contains("picture") && !colNameLower.Contains("caption") && !colNameLower.Contains("width") && !colNameLower.Contains("height")) // don't want Picture1Caption fields!
						{
								//-----------------------------------------------------------
								// Picture Uploader
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><bwb:ImageUpload ImageName='<%# Bind(""{0}"") %>' Width=""220"" Height=""300"" ThumbnailWidth=""22"" ThumbnailHeight=""30"" ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.PictureField(record.Fields.{0}, false) %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);

								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><img src=""<%= record.{0} %>"" width=""30""></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);
								//                            nonPostBackProcedures.AppendFormat(@"
								//                {0}.ImageName = {1}.{0};"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                );

								//                            preSaveProcedures.AppendFormat(@"
								//            if ({0}.SaveImageFile())
								//            {{
								//                {1}.{0} = {0}.ImageName;
								//            }}else
								//						{{
								//							result = false;
								//						}}"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                    );
							} else if (colNameLower.Contains("attachment")) {
								//-----------------------------------------------------------
								// attachment
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><bwb:FileUpload FileName='<%# Bind(""{0}"") %>' AllowedMimeTypesForErrorMessage=""pdf"" AllowedMimeTypes=""application/pdf"" ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.AttachmentField(record.Fields.{0} ,true) %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><a href=""<%= record.{0} %>"" target=""_blank"">download link</a></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);

								//                            nonPostBackProcedures.AppendFormat(@"
								//                {0}.FileName = {1}.{0};"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                );

								//                            preSaveProcedures.AppendFormat(@"
								//            {1}.{0} = {0}.FileName;"
								//                                , colNameOriginal
								//                                , tableName.ToLower().Trim()
								//                                );
							} else if (colNameLower.Contains("colour")) {
								//-----------------------------------------------------------
								// Colour Picker
								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><bwb:ColourPicker Text='<%# Bind(""{0}"") %>' ID=""{0}"" runat=""server"" /></td>
			</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.ColorPickerField(record.Fields.{0},true) %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{0}:</td>
				<td class=""field""><%=record.{0}%> /></td>
			</tr>"
																	, propertyName
									);

								nonPostBackProcedures.AppendFormat(@"
                {0}.Text = {1}.{0};"
										, colNameOriginal
										, tableName.Trim().ToLower()
										);

								preSaveProcedures.AppendFormat(@"
            {1}.{0} = {0}.Text;"
										, colNameOriginal
										, tableName.Trim().ToLower()
										);
							} else if (colNameOriginal.ToString() == "SortPosition") {
								//-----------------------------------------------------------
								// Sort Position field
								continue;  // just skip it because we now add these in the control at the bottom or in the drag and drop sort

								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">Sort Position:</td>
				<td class=""field""><bwb:SortPosition Text='<%# Bind(""SortPosition"") %>' CssClass=""svyShortText"" MaxValueTable=""{0}"" ID=""SortPosition"" runat=""server"" /></td>
			</tr>"
									, tableName
									);
								if (!isWebForms && dataType != "int") {
									string warning = "Cannot generate Sort Position input field for table " + tableName + " because data type is " + dataType + " and Model Forms only accept SortPosition of INT";
									warnings.Append(warning);
									modelEditableFields.Append(warning);
								} else {
									modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">Sort Position:</td>
				<td class=""field""><%= new Forms.SortPositionField(record.Fields.SortPosition) %> (enter 50 for alphabetical order, or a lower number to list first)</td>
			</tr>"
																		, tableName.ToLower().Trim()
																		);
									modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">Sort Position:</td>
				<td class=""field""><%= record.{0}.SortPosition %></td>
			</tr>"
																		, tableName.ToLower().Trim()
																		);
								}
							} else if (colNameLower.Contains("latitude")) {
								// skip
							} else if (colNameLower.Contains("longitude")) {
								//-----------------------------------------------------------
								// Lat long fields field
								var latField = propertyName.ToString().Replace("ongitude", "atitude");
								var addressField = activeRecord.GetFields().FirstOrDefault(f => (f.Name.Contains("Address") || f.Name.Contains("Location")) && f.IsTextField);
								string addressFieldName;
								if (addressField == null) {
									addressFieldName = "null";
								} else {
									addressFieldName = "\""+addressField.Name+"\"";
								}

								editableFields.AppendFormat(@"
			<tr>
				<td class=""label"">Map Location:</td>
				<td class=""field"">not available</td>
			</tr>"
									, tableName
									);
								
								if (addressField != null) {
									modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">Map Positioning:</td>
				<td class=""field"">
					<label><input type=""radio"" name=""MapPositioning"" value=""Auto"" <%=record.GeocodingResult!=""Manual""? ""checked=\""checked\"""":""""%> onclick=""SetLocAuto()""/> Automatic (based on address above)</label> &nbsp; &nbsp;
					<label><input type=""radio"" name=""MapPositioning"" id=""PosManualCheck"" value=""Manual"" <%=record.GeocodingResult==""Manual""? ""checked=\""checked\"""":""""%> onclick=""SetLocManual()""/> Manual (location search below)</label>
				</td>
			</tr>", latField);
								} 
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">Map Location:</td>
				<td class=""field"">
					<%Html.RenderAction<CommonAdminController>(controller => controller.MapLocationEditPanel(record, ""{0}"", ""{1}"", record.GetGeoAddressFieldNames().Join(""|""))); %>
				</td>
			</tr>
			<tr>
				<td class=""label"">{2}:</td>
				<td class=""field""><%= new Forms.FloatField(record.Fields.{0}, false){{DecimalPlaces = 6}} %></td>
			</tr>
			<tr>
				<td class=""label"">{3}:</td>
				<td class=""field""><%= new Forms.FloatField(record.Fields.{1}, false){{DecimalPlaces = 6}} %></td>
			</tr>"

																		, latField
																		, colNameOriginal
																		, SplitTitleCase(latField)
																		, SplitTitleCase(colNameOriginal.ToString()),
																		addressFieldName
																		);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">Map Location:</td>
				<td class=""field"">not available</td>
			</tr>"
																	, tableName.ToLower().Trim()
																	);
							} else if (colNameOriginal.ToString().EndsWith("ID")) {
								// the tableID field has already been skipped so this assumes anything else ending with "ID" must be a foreign key field
								//-----------------------------------------------------------
								// Foreign Key drop down
								string foreignKeyTable = colNameOriginal.ToString().Substring(0, colNameOriginal.ToString().IndexOf("ID"));

								if (BewebData.TableExists(foreignKeyTable)) {
									ParameterCollection descriptionParams = new ParameterCollection();
									descriptionParams.Add("foreign_table", TypeCode.String, foreignKeyTable);
									descriptionParams.Add("foreign_tableID", TypeCode.String, foreignKeyTable + "ID");
									string foreignKeyDescription = GetValue("SELECT TOP 1 COLUMN_NAME FROM INFORMATION_SCHEMA.Columns WHERE table_name=@foreign_table AND COLUMN_NAME <> @foreign_tableID ORDER BY ORDINAL_POSITION", descriptionParams);
									if (foreignKeyDescription == null) { foreignKeyDescription = "blah"; }
									editableFields.AppendFormat(@"
					<tr>
						<td class=""label"">{1}:</td>
						<td class=""field"">
							<bwb:svyDropDownList SelectedValue='<%# Bind(""{0}"")%>'  FirstItemText=""--please select--"" FirstItemValue="""" DataSourceID=""{0}_DS"" DataTextField=""Description"" DataValueField=""{0}"" ID=""{0}"" runat=""server"" />
							<bwb:svySqlDataSource ID=""{0}_DS"" SelectCommand=""SELECT {0}, {3} AS Description FROM {2}"" runat=""server"" />
						</td>
					</tr>"
										, colNameOriginal
										, SplitTitleCase(foreignKeyTable)
										, foreignKeyTable
										, foreignKeyDescription
										);
									modelEditableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field""><%= new Forms.Dropbox(record.Fields.{0}, true, true).Add(new Sql(""SELECT {0} {3} FROM {2}""))%></td>
				</tr>"
										, propertyName
										, SplitTitleCase(foreignKeyTable)
										, foreignKeyTable
										, (foreignKeyDescription != "") ? ", " + foreignKeyDescription : ""
										, tableName.ToLower().Trim()
										);
									modelViewFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field""><%= record.{0}%></td>
				</tr>"
										, propertyName
										, SplitTitleCase(foreignKeyTable)
										, foreignKeyTable
										, (foreignKeyDescription != "") ? ", " + foreignKeyDescription : ""
										, tableName.ToLower().Trim()
										);
								} else {
									editableFields.AppendFormat(@"
					<tr>
						<td class=""label"">{1}:</td>
						<td class=""field"">
							Link/Dropbox/Radio list for table {1} here?
						</td>
					</tr>"
										, colNameOriginal
										, SplitTitleCase(foreignKeyTable)
										, foreignKeyTable
										);
									modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{3}:</td>
				<td class=""field"">
					Link/Dropbox/Radio list for table {1} here?<br>
					<%//=new Forms.TextField(record.Fields.{0}, false) %>
					<%=new Forms.Dropbox(record.Fields.{0}, false).Add(Models.{2}List.LoadActivePlusExisting(record.{0})) %>
				</td>
			</tr>"
									, propertyName
									, SplitTitleCase(foreignKeyTable)
									, foreignKeyTable
									, SplitTitleCase(colNameOriginal.ToString())
									);

									modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0}%></td>
			</tr>"
										, propertyName
										, SplitTitleCase(foreignKeyTable)
										);
								}
							} else if (false)//todo: cat dropbox
						{
								throw new NotImplementedException("cat dropbox");
							} else if (dataType == "float" || dataType == "decimal") {
								//-----------------------------------------------------------
								// Number field
								editableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field"">
						<asp:TextBox Text='<%# Bind(""{0}"") %>' CssClass=""svyShortText"" ID=""{0}"" runat=""server"" />
						<asp:RegularExpressionValidator ControlToValidate=""{0}"" ValidationExpression=""^[0-9.]*$"" SetFocusOnError=""true"" ErrorMessage=""only numbers are allowed"" Display=""Dynamic"" runat=""server"" />
					</td>
				</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.FloatField(record.Fields.{0}, true) %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0} %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
							} else if (dataType == "int" || dataType == "bigint" || dataType == "float" || dataType == "decimal" || dataType == "smallint") {
								//-----------------------------------------------------------
								// Number field
								editableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field"">
						<asp:TextBox Text='<%# Bind(""{0}"") %>' CssClass=""svyShortText"" ID=""{0}"" runat=""server"" />
						<asp:RegularExpressionValidator ControlToValidate=""{0}"" ValidationExpression=""^[0-9.]*$"" SetFocusOnError=""true"" ErrorMessage=""only numbers are allowed"" Display=""Dynamic"" runat=""server"" />
					</td>
				</tr>"
									, colNameOriginal
									, SplitTitleCase(colNameOriginal.ToString())
									);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.IntegerField(record.Fields.{0}, true) %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0} %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
							} else if (colNameLower.IndexOf("url") != -1 && (dataType == "varchar" || dataType == "nvarchar")) {
								//todo: url
								editableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field"">URL: <asp:TextBox Text='<%# Bind(""{0}"") %>' {2} CssClass=""svyText"" ID=""{0}"" runat=""server"" /> should begin with http://</td>
				</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
								, (maxLen != 0) ? "MaxLength=\"" + maxLen + "\"" : ""
								);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= new Forms.UrlField(record.Fields.{0}, true) %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%= record.{0} %></td>
			</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
							} else if (dataType == "varchar" || dataType == "nvarchar") {
								int maxlen = Convert.ToInt32(maxLen);
								editableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field""><asp:TextBox Text='<%# Bind(""{0}"") %>' {2} CssClass=""svyText"" ID=""{0}"" runat=""server"" /></td>
				</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
									//, (maxLen + "" != "") ? "MaxLength=\"" + maxLen + "\"" : ""
								, (maxLen  != 0) ? "MaxLength=\"" + maxlen + "\"" : ""
								);
								modelEditableFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%=new Forms.TextField(record.Fields.{0}, true) %></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);
								modelViewFields.AppendFormat(@"
			<tr>
				<td class=""label"">{1}:</td>
				<td class=""field""><%=record.{0}%></td>
			</tr>"
																	, propertyName
																	, SplitTitleCase(colNameOriginal.ToString())
																	, tableName.ToLower().Trim()
									);
							} else if (dataType == "uniqueidentifier") {
								editableFields.AppendFormat(@"
				<tr>
					<td class=""label"">{1}:</td>
					<td class=""field"">GUID:<asp:TextBox Text='<%# Bind(""{0}"") %>' {2} CssClass=""svyText"" ID=""{0}"" runat=""server"" readonly=""True""/></td>
				</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
								, (maxLen  != 0) ? "MaxLength=\"" + maxLen + "\"" : ""
								);
								modelEditableFields.AppendFormat(@"
					<tr>
						<td class=""label"">{1}:</td>
						<td class=""field"">GUID: <%= new Forms.TextField(record.Fields.{0}, true) %></td>
					</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
								modelViewFields.AppendFormat(@"
					<tr>
						<td class=""label"">{1}:</td>
						<td class=""field"">GUID: <%= record.{0} %></td>
					</tr>"
																	, propertyName
																, SplitTitleCase(colNameOriginal.ToString())
									);
								//throw new NotImplementedException("guid is stuffed - dont gen it");//todo guid is stuffed
							} else if (dataType == "nchar") {
								//skip 
							} else if (dataType == "varbinary") {
								//skip 
							} else {
								//-----------------------------------------------------------
								// no defaults

								/*
								editableFields.AppendFormat(@"
					<tr>
						<td class=""label"">{1}:</td>
						<td class=""field""><asp:TextBox Text='<%# Bind(""{0}"") %>' {2} CssClass=""svyText"" ID=""{0}"" runat=""server"" /></td>
					</tr>"
								, colNameOriginal
								, SplitTitleCase(colNameOriginal.ToString())
								, (maxLen!=0)?"MaxLength=\""+maxLen+"\"":""
								);
								*/
								throw new Exception("Not supported data format table[" + tableName + "] column_name[" + colNameOriginal + "]data_type[" + dataType + "]");

							}

							break;
					}

					//-----------------------------------------------------------
					// SQL statement building
					insertColumns.Append(", " + colNameOriginal);
					if (dataType == "money") {
						insertValues.AppendFormat(", CONVERT(money, @{0})", colNameOriginal);
						updateColumnsAndValues.AppendFormat(", {0}=CONVERT(money, @{0})", colNameOriginal);
					} else {
						insertValues.Append(", @" + colNameOriginal);
						updateColumnsAndValues.AppendFormat(", {0}=@{0}", colNameOriginal);
					}

					if (colNameOriginal.ToString() == "SortPosition") {
						orderByClause = " ORDER BY SortPosition ASC";
					}
					if (colNameOriginal.ToString() == "DateAdded") {
						orderByClause = " ORDER BY DateAdded desc";
					}
				
				}


				//RelatedCheckboxes - walk the tables looking for tablename+'has' or tablename+'in'
				//todo: finish linked tables
				var manyToManyTables = GetManyToManyTables(tableName);
				foreach (KeyValuePair<string, string> manyToManyTable in manyToManyTables) {
					var joinTable = manyToManyTable.Key;
					var otherTable = manyToManyTable.Value;

					editableFields.AppendFormat(@"
										<tr>
											<td class=""label"">{0}:</td>
											<td class=""field"">{1}</td>
										</tr>"
									, tableName + " to " + otherTable
									, "has"
									);

					modelEditableFields.AppendFormat(@"
					<tr class=""svyCheckboxContainer"">
						<td class=""label"">{0}:<br/><br/>
							<input type=""text"" class=""placeholder svyCheckboxFilter"" placeholder=""filter..."" />
							<br><br>
							<a href=""#"" onclick=""$('#{1} .checkboxes input:visible').prop('checked',true);return false"">Check all</a> | 
							<a href=""#"" onclick=""$('#{1} .checkboxes input:visible').prop('checked',false);return false"">Uncheck all</a>
							<br><br>
							<a href=""#"" onclick=""$('#{1}').height($('#{1}')[0].scrollHeight);return false"">Expand</a>
						</td>
						<td class=""field"">
							<div style=""height:200px;overflow-y:scroll;"" id=""{1}"">
							<%= new Forms.Checkboxes(record.{1}, Models.{2}List.LoadAll()) %>
							</div>
						</td>
					</tr>"
							, Fmt.SplitTitleCase(joinTable.Plural()), joinTable.Plural(), otherTable);

					modelViewFields.AppendFormat(@"
					<tr>
						<td class=""label"">{0}:</td>
						<td class=""field"">
							<div style=""height:200px;overflow-y:scroll;"">
							list here - not available yet
							</div>
						</td>
					</tr>"
							, Fmt.SplitTitleCase(joinTable.Plural()), joinTable, otherTable);
				}

				//Subforms - generate if selected on gen form
				if (subformTableName.IsNotBlank()) {
					string otherTable = subformTableName;

					string fragment =
						String.Format(
							@"
					<table class=""svySubform"" id=""df_SubformTable_{0}"">
						<colgroup>
							<col width=""25%"" />
							<col width=""40%"" />
							<col width=""5%"" />
						</colgroup>
						<thead>
							<tr>
								<td class=""colhead"">
									TODO: add column headings you want here, eg:
									Ref #
								</td>
								<td class=""colhead"">Description</td>
								<td class=""remove"">&nbsp;</td>
							</tr>
						</thead>
						<tbody>
							<%new Savvy.SavvyDataForm<Models.{0}List,Models.{0}>(record.{1}, new Savvy.SubformOptions() 
								{{ 
									DeleteButtonCaption = ""x"", UseCssButtons = false 
								}}).Render(childRecord => 
									{{ 
										%>
										<td>
											TODO: add any subform fields you want editable here, eg:
											<%= new Forms.TextField(childRecord.Fields.somefieldname, true) %>
										</td>
										<td>
											<%= new Forms.TextField(childRecord.Fields.somefieldname, true) %>
										</td>
										<% 
									}}); 
							%>
						</tbody>
						<tfoot>
							<tr>
								<td colspan=""7"" class=""addingRow"">
									<input type=button onclick=""df_AddRow('{0}');return false;"" value=""Add {2}"">
									<!--<a href=""#"" onclick=""df_AddRow('{0}');return false;"">Add {2}</a>-->
									TODO: add saving code in Controller eg record.{1}.UpdateFromRequest(); record.{1}.Save();
								</td>
							</tr>
						</tfoot>
					</table>",
							otherTable, otherTable.Plural(), Fmt.SplitTitleCase(otherTable));

					modelEditableFields.AppendFormat(
						@"
			<tr>
				<td class=""label"">{0}</td>
				<td class=""field"">
					{1}
				</td>
			</tr>"
						, Fmt.SplitTitleCase(otherTable.Plural())
						, fragment
						);

					modelViewFields.AppendFormat(
						@"
			<tr>
				<td class=""label"">{0}</td>
				<td class=""field"">
					{1}
				</td>
			</tr>"
						, Fmt.SplitTitleCase(otherTable.Plural())
						, fragment
						);

					editableFields.AppendFormat(
						@"
			<tr>
				<td class=""label"">{0}</td>
				<td class=""field"">
					Sorry, Savvy Subforms are not available in Web Forms admin templates
				</td>
			</tr>"
						, Fmt.SplitTitleCase(otherTable.Plural())
						);

				}


				// replace markers
				newPage = newPage.Replace("[---tablename---]", tableName);
				newPage = newPage.Replace("[---modelname---]", modelName);
				newPage = newPage.Replace("[---pagetitle---]", pageTitle);
				newPage = newPage.Replace("[---editablefields---]", editableFields.ToString());
				newPage = newPage.Replace("[---modeleditablefields---]", modelEditableFields.ToString());
				newPage = newPage.Replace("[---modelviewfields---]", modelViewFields.ToString());
				newPage = newPage.Replace("[---insertcolumns---]", insertColumns.ToString().Remove(0, 2)); // remove the first comma and space
				newPage = newPage.Replace("[---insertvalues---]", insertValues.ToString().Remove(0, 2));
				newPage = newPage.Replace("[---updatecolumnsandvalues---]", updateColumnsAndValues.ToString().Remove(0, 2));
				newPage = newPage.Replace("[---orderbyclause---]", orderByClause);
				newPage = newPage.Replace("[---pkname---]", pkName);


				WriteFile(tableName, outputFileName, newPage);

				if (warnings + "" != "") {
					resultMessage += "<div class='svyNotification'><b>Warning:</b> " + warnings + "</div>";
				}
			}

			// .aspx.cs page
			if (templateFolderName != "pages_mvc") {
				string verifiedAspxCs = CheckTemplate(templateFolderName + "/" + templatePageName + "Page.template-aspx-cs");
				if (verifiedAspxCs != "") {
					string newPage = File.ReadAllText(verifiedAspxCs);

					newPage = newPage.Replace("[---pagetitle---]", pageTitle);
					newPage = newPage.Replace("[---tablename---]", tableName);
					newPage = newPage.Replace("[---modelname---]", modelName);
					newPage = newPage.Replace("[---pkname---]", pkName);
					newPage = newPage.Replace("[---tablenamelower---]", tableName.ToLower());
					newPage = newPage.Replace("[---nonpostbackprocedures---]", nonPostBackProcedures.ToString().Trim());
					newPage = newPage.Replace("[---presaveprocedures---]", preSaveProcedures.ToString().Trim());

					WriteFile(tableName, tableName + "" + templatePageName + ".aspx.cs", newPage);
				}
			}

		}
		#endregion

		// utility functions (some may be copied from other files to keep this page independent)

		#region CheckTemplate

		/// <summary>
		/// Check that a template file exists, and if it does return it's mapped path
		/// </summary>
		/// <param name="file">the file to find in the admin/tools/templates/ directory</param>
		/// <returns></returns>
		protected string CheckTemplate(string file) {
			string templateFile = Web.MapPath("template/" + file);
			if (!File.Exists(templateFile)) {
				resultMessage += "<br />" + templateFile + " not found";
				templateFile = ""; // clear it out - it doesn't exist
			}
			return templateFile;
		}

		#endregion

		#region WriteFile
		/// <summary>
		/// Write the file to the filesystem checking to see if it already exists
		/// </summary>
		/// <param name="tableName">used for the page's name eg Product</param>
		/// <param name="outputFileName">productlist.aspx, productlist.aspx.cs or similar</param>
		/// <param name="contents">the content to write into the page</param>
		protected void WriteFile(string tableName, string outputFileName, string contents) {
			string newFile = Web.MapPath(targetfolder + outputFileName);
			if (File.Exists(newFile) && !overwrite) {
				resultMessage += "<div class='svyNotification'>" + newFile + " already exists, it has not been overwritten.</div>";
			} else {
				try {
					// create folder if it doesn't exist
					string path = Path.GetDirectoryName(newFile);
					if (!Directory.Exists(path)) {
						Directory.CreateDirectory(path);
					}

					File.WriteAllText(newFile, contents);

					string url = Web.ResolveUrl(targetfolder) + outputFileName;
					if (templateFolderName == "pages_mvc") {
						url = Web.Root + "Admin/" + tableName + "Admin";
					}
					resultMessage += "<div class='svyNotificationSuccess'><a href=\"" + url + "\">" + newFile + "</a> created successfully.</div>";

					// add to list of files created for adding to project makefile
					this.createdFilenames.Add(targetfolder + outputFileName);

				} catch (System.UnauthorizedAccessException) {
					resultMessage += "<div class='svyNotificationSuccess'><a href=\"" + Web.ResolveUrl(targetfolder) + outputFileName + "\">" + newFile + "</a> failed - access denied.</div>";
				}
			}
		}
		#endregion

		#region SplitTitleCase
		// from App_Code/Beweb/Formatting.cs
		/// <summary>
		/// takes a string and seperates words where the case changes or where it turns to a number
		/// e.g. ThisIsTitle9Case becomes This Is Title 9 Case
		/// </summary>
		/// <param name="s">text to format</param>
		/// <returns></returns>
		protected static string SplitTitleCase(string s) {
			string returnValue = s;
			// find e3C in Picture3Caption
			Regex r1 = new Regex(@"([a-z])([0-9])([A-Z])", RegexOptions.Multiline);
			returnValue = r1.Replace(returnValue, "$1 $2 $3");
			// find e3, 2C, nL in Picture32CaptionLong
			Regex r2 = new Regex(@"([a-z0-9])([A-Z0-9])", RegexOptions.Multiline);
			returnValue = r2.Replace(returnValue, "$1 $2");
			// find e3, 2C, nL in Picture_32_Caption_Long
			Regex r3 = new Regex(@"([a-zA-Z0-9])_([a-zA-Z0-9])", RegexOptions.Multiline);
			returnValue = r3.Replace(returnValue, "$1 $2");

			returnValue = Fmt.TitleCase(returnValue);
			return returnValue;
		}
		#endregion

		#region GetConnectionString
		private static string GetConnectionString() {
			return connectionString;
		}
		#endregion

		#region GetValue
		// copied from App_Code/Beweb/BewebData.cs
		public static String GetValue(string SqlStatement) {
			ParameterCollection pc = new ParameterCollection();
			return GetValue(SqlStatement, pc);
		}
		public static String GetValue(string SqlStatement, Parameter selectParameter) {
			ParameterCollection pc = new ParameterCollection();
			pc.Add(selectParameter);
			return GetValue(SqlStatement, pc);
		}
		public static String GetValue(string SqlStatement, ParameterCollection selectParameters) {
			SqlDataSource ds = new SqlDataSource();
			ds.DataSourceMode = SqlDataSourceMode.DataReader;
			ds.ConnectionString = GetConnectionString();
			ds.SelectCommand = SqlStatement;
			foreach (Parameter sp in selectParameters) {
				ds.SelectParameters.Add(sp);
			}

			SqlDataReader dr;
			try {
				dr = (SqlDataReader)ds.Select(DataSourceSelectArguments.Empty);
			} catch (Exception e) {
				string errorMessage = "ERROR in BewebData.GetValue: " + e.Message + " in [" + SqlStatement + "]";
				foreach (Parameter p in selectParameters) {
					errorMessage += ", " + p.Name + " = [" + p.DefaultValue + "]";
				}
				throw new Exception(errorMessage);
			}

			String returnValue = "";
			if (dr != null) {
				foreach (DbDataRecord view in dr) {
					returnValue = view[0].ToString();
				}
			}
			return returnValue;
		}
		#endregion

		private Dictionary<string, string> GetManyToManyTables(string tableName) {
			var dic = new Dictionary<string, string>();
			SqlConnection conn = new SqlConnection(connectionString);
			conn.Open();
			System.Data.DataTable dts = conn.GetSchema("Tables");

			foreach (System.Data.DataRow row in dts.Rows) {
				string tableNameScan = "";
				foreach (System.Data.DataColumn col in dts.Columns) {
					if (col.ColumnName.Equals("TABLE_NAME")) {
						tableNameScan = row[col].ToString();

						if ((tableNameScan.ToLower().IndexOf(tableName.ToLower() + "has") != -1) || (tableNameScan.ToLower().IndexOf(tableName.ToLower() + "in") != -1)) {
							Beweb.Logging.dout("many to many table [" + tableNameScan + "]");
							string connector = "";
							if (tableNameScan.ToLower().IndexOf(tableName.ToLower() + "has") != -1) {
								connector = "has";
							}
							if (tableNameScan.ToLower().IndexOf(tableName.ToLower() + "in") != -1) {
								connector = "in";
							}
							string otherTable = tableNameScan.Substring((tableName.ToLower() + "has").Length);
							string joinTable = tableNameScan;

							dic.Add(joinTable, otherTable);

						}
					}
				}
			}
			conn.Close();
			return dic;
		}


	}
}
