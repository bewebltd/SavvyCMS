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

public partial class Proxy : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
		bool usePost = false;
		string returnValue = "";
		string URL = Request["url"];
		string userName = "";
		string password = "";
		
		ASCIIEncoding encoding=new ASCIIEncoding();
		//string queryString="address=8+nixon&suburb=otahuhu&town=auckland&sort=name&start=1&maxResults=10";
		string queryString=Request["qs"];
		if(Request["q"]+""!="")
		{
			queryString = "q="+Request["q"]+"&"+queryString;
		}
		//queryString+="&rn="+VB.rnd(); //cache buster

		string fullUrl = URL+((!usePost && queryString+""!="")?"?"+queryString:"");


		HttpWebRequest httpRequest = null;
		try
		{
			httpRequest = WebRequest.Create(fullUrl) as HttpWebRequest;

		}catch(Exception){}

		if (httpRequest != null)
		{
			if(userName!=""){ httpRequest.Credentials = new NetworkCredential(userName,password);}
			if(usePost)
			{
				httpRequest.Method = "POST";
				httpRequest.ContentType="application/x-www-form-urlencoded";
				byte[]  data = encoding.GetBytes(queryString);
				httpRequest.ContentLength = data.Length;
				Stream newStream=httpRequest.GetRequestStream();
				// Send the data.
				newStream.Write(data,0,data.Length);
				newStream.Close();
			}else
			{
				httpRequest.Method = "GET";
				
			}

			try
			{
				HttpWebResponse httpResponse = httpRequest.GetResponse() as HttpWebResponse;
				if (httpResponse != null)
				{
					StreamReader sr = new StreamReader(httpResponse.GetResponseStream());
					returnValue = sr.ReadToEnd();
					sr.Close();
					httpResponse.Close();
				}
			}
			catch	(Exception ex)
			{
				// try and replace the URL if we find it - if not, just return the user's Error String
				returnValue = "error "+ex.Message;
			}
		}

		
		Response.ContentType="image/jpeg";

		Response.Write(returnValue);

  }
}
