using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;
using WebSupergoo.ABCpdf8;


	public partial class test_pdf : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			byte[] pdfData;
			var url = Web.BaseUrl;

			using (var theDoc = new Doc()) {
				theDoc.MediaBox.String = "A4"; // this sets the page to A4 (also removes scrollbar showing in the pdf LOL)
				theDoc.TopDown = true;
				theDoc.Rect.Top = 5;
				theDoc.Rect.Left = -18;
				//clear caching?
				//theDoc.HtmlOptions.Engine = EngineType.Gecko;
				theDoc.HtmlOptions.PageCacheEnabled = false;
				theDoc.HtmlOptions.UseNoCache = true;
				theDoc.HtmlOptions.PageCacheClear();
				theDoc.HtmlOptions.PageCachePurge();
				theDoc.HtmlOptions.UseResync = true;
				theDoc.HtmlOptions.Timeout = 30 * 1000; // 30 seconds
				var id = theDoc.AddImageUrl(url);
				//add more pages if more than 1 page
				for (; theDoc.Chainable(id); ) {
					theDoc.Page = theDoc.AddPage();
					id = theDoc.AddImageToChain(id);
				}
				//theDoc.Save(fullServerPath);
				pdfData = theDoc.GetData();
				Web.Response.ContentType = "application/pdf";
				Web.Response.BinaryWrite(pdfData);
				theDoc.Clear();
			}

		}


	
}