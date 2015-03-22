using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class test_test_datablock_create : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		//test-datablock-create.aspx

		Beweb.DataBlock db = new Beweb.DataBlock();
		db.OpenDB();
		db.TableName = "Textblock";
		//db[0] = "1";
		db["Title"] = "hey";
		db["title"] = "hey3";
		db.Create();//.Update();
		db.CloseDB();
	}
}
