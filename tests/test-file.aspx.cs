using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Beweb;


public partial class tests_filetest : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		
		StreamWriter test = null;
		const string str = "Hello World";
		string file = Server.MapPath("~/attachments/text.txt");
		
		//test write
		try {
			test = File.CreateText(file);
			test.Write(str);
			if (test != null)test.Close();
			Response.Write("write success<br>");
		} catch (IOException e1) {
			Response.Write("File write error: "+e1.Message+"<br>");
		} 

		//test delete
		if(File.Exists(file)){
			File.Delete(file);
			Response.Write("delete success<br>");
		}else{
			Response.Write("file not found to be deleted<br>");
		}
		
	}
}
