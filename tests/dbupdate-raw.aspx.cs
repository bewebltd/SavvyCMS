using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class test_Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string sConnectionString = "Password=StrongPassword;User ID=UserName;" +
			"Initial Catalog=pubs;"+
			"Data Source=(local)";
		sConnectionString = Beweb.BewebData.GetConnectionString();
		SqlConnection objConn = new SqlConnection(sConnectionString);
		objConn.Open();
		SqlDataAdapter daAuthors = new SqlDataAdapter("Select * From Authors", objConn);
		DataSet dsPubs = new DataSet();
		daAuthors.FillSchema(dsPubs, SchemaType.Source, "Authors");
		daAuthors.Fill(dsPubs, "Authors");
		DataTable tblAuthors = dsPubs.Tables["Authors"];
		
		DataRow drCurrent  = tblAuthors.Rows[0];//get first record
		////' Obtain a new DataRow object from the DataTable.
		//DataRow drCurrent = tblAuthors.NewRow();

		//' Set the DataRow field values as necessary.
		//drCurrent["au_id"] = "993-21-3427";
		//drCurrent["au_fname"] = "Jeremy";
		//drCurrent["au_lname"] = "Johnson";
		//drCurrent["phone"] = "800 226-0752";
		//drCurrent["address"] = "1956 Arlington Pl.";
		//drCurrent["city"] = "Winnipeg";
		//drCurrent["state"] = "MB";
		//drCurrent["contract"] = 1;

		//'Pass that new object into the Add method of the DataTable.Rows collection.
		//tblAuthors.Rows.Add(drCurrent);
		//MsgBox("Add was successful.")r

		//var objCommandBuilder = new SqlCommandBuilder(daAuthors);
		//daAuthors.Update(dsPubs, "Authors");


		//drCurrent = tblAuthors.Rows.Find("213-46-8915");
		//drCurrent.BeginEdit();
		drCurrent["phone"] = "555" + drCurrent["phone"].ToString().Substring(3);
		drCurrent["address"] = "wer234 Arlington Pl.";
		//drCurrent.EndEdit();
		//MsgBox("Record edited successfully")

		//remove the record
		//drCurrent.Delete();
			
		new SqlCommandBuilder(daAuthors);//look: return from this is disregarded
		daAuthors.Update(dsPubs, "Authors");
		//var s=daAuthors.InsertCommand.CommandText;
	}

	void test()
	{
		string sConnectionString = Beweb.BewebData.GetConnectionString();
		var objConn = new SqlConnection(sConnectionString);
		objConn.Open();
		var daAuthors = new SqlDataAdapter("Select * From Authors", objConn);
		var dsPubs =new DataSet();
		daAuthors.FillSchema(dsPubs, SchemaType.Source, "Authors");
		daAuthors.Fill(dsPubs, "Authors");
		DataTable tblAuthors = dsPubs.Tables["Authors"];
		DataRow drCurrent  = tblAuthors.Rows[0];//get first record
		drCurrent.BeginEdit();
		drCurrent["phone"] = "342" + drCurrent["phone"].ToString().Substring(3);
		drCurrent.EndEdit();
		var objCommandBuilder = new SqlCommandBuilder(daAuthors);
		daAuthors.Update(dsPubs, "Authors");
	}
}
