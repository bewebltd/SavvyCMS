using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Net;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;

public partial class ProxyImage : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
		bool usePost = false;
		//string returnValue = "";
		string URL = Request["url"];
		//string userName = "";
		//string password = "";
		
		ASCIIEncoding encoding=new ASCIIEncoding();
		//string queryString="address=8+nixon&suburb=otahuhu&town=auckland&sort=name&start=1&maxResults=10";
		string queryString=Request["qs"];
		if(Request["q"]+""!="")
		{
			queryString = "q="+Request["q"]+"&"+queryString;
		}
		//queryString+="&rn="+VB.rnd(); //cache buster

		string ContentUrl = URL+((!usePost && queryString+""!="")?"?"+queryString:"");

		//if (!ContentUrl.StartsWith("http")) Response.BinaryWrite(new byte[] { 0 });

		//WebClient wc = new WebClient();
		//wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
		//Byte[] result;
		//result = wc.DownloadData(ContentUrl);
		//Response.BinaryWrite(result);

		const int BUFFER_SIZE = 1024 * 1024;

		string imgExtension = ContentUrl.Substring(ContentUrl.Length - 3, 3);
		switch (imgExtension)
		{
			case "":
				//image/bmp
				Response.ContentType = "image/bmp";
				break;

			case "jpg":
				//image/jpeg
				Response.ContentType = "image/jpeg";
				break;

			case "gif":
				//image/gif
				Response.ContentType = "image/gif";
				break;

			default:
				Response.ContentType = "image/jpeg";
				break;
		}

		

		var req = WebRequest.Create(ContentUrl);
		using (var resp = req.GetResponse())
		{
			using (var stream = resp.GetResponseStream())
			{
				var bytes = new byte[BUFFER_SIZE];
				while (true)
				{
					var n = stream.Read(bytes, 0, BUFFER_SIZE);
					if (n == 0)
					{
						break;
					}
					Response.OutputStream.Write(bytes, 0, n);
				}
			}
		}

  }
}
