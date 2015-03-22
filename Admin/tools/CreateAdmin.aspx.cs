#define MVC
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;
using Savvy;

namespace Savvy {
	public partial class admin_tools_CreateAdmin : System.Web.UI.Page {
		// common properties
		protected static string connectionString = Beweb.BewebData.GetConnectionString();// ConfigurationManager.ConnectionStrings["ConnectionStringDEV"].ToString(); // we should only need CreateAdmin when on DEV right?
		protected string resultMessage = "";

		protected void Page_Load(object sender, EventArgs e) {
			Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);

			Savvy.Breadcrumbs bread = new Savvy.Breadcrumbs(Request, Session);
			bread.AddBreadcrumb(3, "Generate Admin System");
			if (!IsPostBack) {
				#if MVC
				targetfolder.Text = "~/Areas/Admin/";
				#else
				targetfolder.Text = "~/admin/";

				#endif
			}

		}

		protected void Page_PreRender(object sender, EventArgs e) {
			CreateTable.SelectedValue = Web.RequestEx["CreateTable"];
			SubformTable.SelectedValue = Web.RequestEx["SubformTable"];
			//if (!IsPostBack) {
				CreateTable.Items.Insert(0, new ListItem("--select--"));
			//}
		}


		protected void SetupChooseTableDS(object sender, EventArgs e) {
			((SqlDataSource)sender).ConnectionString = connectionString;
		}

		protected void CheckedGenAll(object sender, EventArgs e) {
			choosetable.Visible = !allpages.Checked;
		}

		protected void Create_OnClick(object sender, EventArgs e) {
			string tableName = Web.Request["ModelCreateTable"];
			if (tableName.IsBlank() && CreateTable.SelectedValue+""!="--select--") {
				tableName = CreateTable.SelectedValue;
			}
			if (tableName.IsNotBlank()) {
				string childTableName = null;
				if (includeSubform.Checked || includeChildLink.Checked) {
					childTableName = Web.Request["ModelSubformTable"];
					if (childTableName.IsBlank()) {
						childTableName = SubformTable.SelectedValue;
					}
				}

				var gen = new Savvy.AdminGenerator();
				if (includeChildLink.Checked) {
					gen.ChildTableUI = "link";
				} else if (includeSubform.Checked) {
					gen.ChildTableUI = "subform";
				}
				ResultArea.Text = gen.Run(allpages.Checked, tableName, TemplateToUse.SelectedValue, targetfolder.Text, listpages.Checked, editpages.Checked, overwrite.Checked, childTableName, viewpages.Checked,exportpages.Checked);
			}

			//if (!allpages.Checked) {
			//	if (CreateTable.SelectedIndex > 0) {
			//		CreateOne(CreateTable.SelectedValue);
			//	} else {
			//		ResultArea.Text = "Please select a table to generate.";
			//	}
			//} else {
			//	//var models = Beweb.ActiveRecordDatabaseGenerator.GetModels();
			//	var models = Beweb.BewebData.GetTableNames();
			//	ResultArea.Text = "Generating tables...<br>" + models.Count;
			//	foreach (string table in models) {
			//		ResultArea.Text += "Table[" + table + "]<br>";
			//		CreateOne(table);

			//	}
			//}
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



		// utility functions (some may be copied from other files to keep this page independent)



		#region GetConnectionString
		private static string GetConnectionString() {
			return connectionString;
		}
		#endregion

	}
}