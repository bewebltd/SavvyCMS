using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class PdfController : ApplicationController {
		
	//public ActionResult FactSheet(string id) {
		//	int decryptedID = Crypto.DecryptID(id);
		//	var document = Models.ProfileTemplate.LoadID(decryptedID, Otherwise.NotFound);
		//	Web.DownloadFile(Web.Attachments + document.Attachment, document.Title);
		//	return null;
		//}

		/*
		public ActionResult FactSheetPdf(string id) {
			int decryptedID = Crypto.DecryptID(id);
			XSettings.InstallLicense(@"WuJbSzVR9+1XpyYSM/zSObfMSeMZVIAhqsqxl7SwQh5YqX6WOi69N5FqZD09mwUJbtQ09T1HriMS1aEiT7F4yzkZWATfWHBOrCgxVcOyR0LmwU3y8sASXYXiZV2yYWFgGO5IUMBS/gJwaRx8rSiGKQRkYimKR2+vx84+6lFwbO7QzbBA2Afk4yzSiPhwjA9gezk5yFpzzlScIj51GLB6/jY=");
			var root = Web.BaseUrlNoSSL;


			var document = Models.ProfileTemplate.LoadID(decryptedID, Otherwise.NotFound);
			var pdf = BuildProposalPdf(document.DefaultTheme == "Light", document);

			Web.SetHeadersForPDF(document.Title);


			Response.BinaryWrite(pdf.GetData()); ;//File(Web.Root + "attachments/downloadPDF/" + pdfPath, "application/pdf");
			return null;
		}


		public ActionResult DownloadSinglePdfView(int downloadID) {
			ProfileTemplate template = null;
			PdfViewModel model = new PdfViewModel();
			template = ProfileTemplate.LoadByProfileTemplateID(downloadID);
			model.ProfileTemplate = template;
			return View("ProposalPdf", model);
		}


		public FileResult DownloadProposalPdf(int proposalID) {
			XSettings.InstallLicense(@"WuJbSzVR9+1XpyYSM/zSObfMSeMZVIAhqsqxl7SwQh5YqX6WOi69N5FqZD09mwUJbtQ09T1HriMS1aEiT7F4yzkZWATfWHBOrCgxVcOyR0LmwU3y8sASXYXiZV2yYWFgGO5IUMBS/gJwaRx8rSiGKQRkYimKR2+vx84+6lFwbO7QzbBA2Afk4yzSiPhwjA9gezk5yFpzzlScIj51GLB6/jY=");
			var root = Web.BaseUrlNoSSL;

			var propsoal = Proposal.LoadByProposalID(proposalID);
			var pdf = BuildProposalPdf(propsoal.DefaultTheme == "Light", propsoal.ProposalPages);
			Web.SetHeadersForPDF(propsoal.ProposalName);

			Response.BinaryWrite(pdf.GetData()); ;//File(Web.Root + "attachments/downloadPDF/" + pdfPath, "application/pdf");
			return null;
		}

		/*
				public FileResult DownloadSinglePdf(int downloadID) {
					XSettings.InstallLicense(@"WuJbSzVR9+1XpyYSM/zSObfMSeMZVIAhqsqxl7SwQh5YqX6WOi69N5FqZD09mwUJbtQ09T1HriMS1aEiT7F4yzkZWATfWHBOrCgxVcOyR0LmwU3y8sASXYXiZV2yYWFgGO5IUMBS/gJwaRx8rSiGKQRkYimKR2+vx84+6lFwbO7QzbBA2Afk4yzSiPhwjA9gezk5yFpzzlScIj51GLB6/jY=");
					var root = Web.BaseUrlNoSSL;
					var url = root + "Download/DownloadSinglePdfView?downloadID=" + downloadID;
					var document = Models.ProfileTemplate.LoadID(downloadID, Otherwise.NotFound);
					string pdfPath = BuildPdfAbcpdf(downloadID, url, document.DefaultTheme == "Light", null);
					return File(Web.Root + "attachments/downloadPDF/" + pdfPath, "application/pdf");
				}
		#1#

		public ActionResult DownloadPdfView(int downloadID) {
			/*Proposal proposal = null;
			PdfViewModel model = new PdfViewModel();
			proposal = Proposal.LoadByProposalID(downloadID);
			var proposalPages = proposal.ProposalPages.Active.OrderBy(p => p.SortPosition).ToList();
			model.ProposalPages = proposalPages;
			model.Proposal = proposal;
			return View("ProposalPdf", model);
		}

		public Doc BuildProposalPdf(bool isLight, ProfileTemplate template) {
			var proposal = new ProposalPageList();
			proposal.Add(new ProposalPage() { ProfileTemplate = template });
			return BuildProposalPdf(isLight, proposal);
		}

		private Doc BuildProposalPdf(bool isLight, ProposalPageList proposals) {
			var backgroundDoc = new Doc();
			if (isLight) {
				backgroundDoc.Read(Server.MapPath("../../attachments/templates/penlightTemplate.pdf"));
			} else {
				backgroundDoc.Read(Server.MapPath("../../attachments/templates/pendarkTemplate.pdf"));
			}

			backgroundDoc.MediaBox.String = "A4";

			var theDoc = new Doc();
			theDoc.MediaBox.String = "A4"; // sets page to A4 & removes scrollbar appearing
			theDoc.HtmlOptions.Engine = EngineType.MSHtml;
			theDoc.TopDown = true;
			theDoc.HtmlOptions.FontSubstitute = false;
			theDoc.HtmlOptions.FontProtection = false;
			theDoc.HtmlOptions.FontEmbed = true;
			//clear caching?
			theDoc.HtmlOptions.PageCacheEnabled = false;
			theDoc.HtmlOptions.UseNoCache = true;
			theDoc.HtmlOptions.PageCacheClear();
			theDoc.HtmlOptions.PageCachePurge();
			theDoc.HtmlOptions.UseResync = true;
			//theDoc.Rect.String = "50 50 550 800";

			theDoc.MediaBox.String = backgroundDoc.MediaBox.String;
			theDoc.Rect.String = backgroundDoc.MediaBox.String;

			/*		for (int i = 1; i <= 1; i++) { // need to chain correctly
						theDoc.AddImageDoc(theTemplateDoc, i, null);
						theDoc.FrameRect();
						theDoc.Flatten();
					}#2#
			const string contentArea = "25 100 570 800";
			const string fullPage = "0 0 595 852";

			//AddBackground(theDoc, backgroundDoc, 1);
			//theDoc.FrameRect();
			theDoc.Flatten();
			var root = Web.BaseUrlNoSSL;
			int pageNum = 1;
			foreach (var proposal in proposals) {
				var template = proposal.ProfileTemplate;
				var sourceDoc = new Doc();
				if (template.Format == "PDF") {
					sourceDoc.Read(Server.MapPath(Web.Attachments + template.Attachment));

					//sourceDoc.Rect.String = contentArea;
				} else if (template.Format == "HTML") {
					var url = root + "Download/DownloadSinglePdfView?downloadID=" + template.ID;
					var id = sourceDoc.AddImageUrl(url, true, 1140, true);
					//theDoc.AddPage(pageNum);
					//theDoc.Page = pageNum;
					//pageNum++;
					//	theDoc.Rect.Inset(20, 20);//bottom margin
					//add more pages if more than 1 page
		
					for (; sourceDoc.Chainable(id); ) {
						sourceDoc.Page = sourceDoc.AddPage();
						id = sourceDoc.AddImageToChain(id);
						
					}
				}

				// add each page to main pdf
				int existingPdfPageCount = sourceDoc.PageCount;
				for (var existingPageNum = 1; existingPageNum <= existingPdfPageCount; existingPageNum++) {
					if (pageNum > 1) {
						theDoc.Page = theDoc.AddPage();
					}
					XRect rc = new XRect();
					rc.String = contentArea;
					theDoc.Rect.String = contentArea;
					if (template.Format == "PDF" && template.PdfInset != null) {
						double inset = (double)template.PdfInset;
						theDoc.Rect.Inset (inset,inset);
					}
					theDoc.AddImageDoc(sourceDoc, existingPageNum, null); //add the pdf
					pageNum++;
				}

			}

			//add background as a layer
			int theID = 0;
			for (int i = 1; i <= theDoc.PageCount; i++) {
			theDoc.Rect.String = fullPage;
				theDoc.PageNumber = i;
				theDoc.Layer = theDoc.LayerCount + 1;
				if (i == 1) {
					string thePath = "";
					if (isLight) {
						thePath = Server.MapPath(Web.Attachments + "templates/peng-light-template.jpg");
					} else {
						thePath = Server.MapPath(Web.Attachments + "templates/peng-dark-template.jpg");
					}
					theID = theDoc.AddImageFile(thePath, 1);
				} else
					theDoc.AddImageCopy(theID);
			}
			//	theDoc.Save(fullServerPath);
			//	theDoc.Clear();#1#
			return theDoc;
		}


		/*		private void SetPageProperties (Doc theDoc) {
					var page = theDoc;
					page.MediaBox.String = "A4"; // sets page to A4 & removes scrollbar appearing
					page.HtmlOptions.Engine = EngineType.MSHtml;
					page.HtmlOptions.FontSubstitute = false;
					page.HtmlOptions.FontProtection = false;
					page.HtmlOptions.FontEmbed = true;
					//clear caching?
					page.HtmlOptions.PageCacheEnabled = false;
					page.HtmlOptions.UseNoCache = true;
					page.HtmlOptions.PageCacheClear();
					page.HtmlOptions.PageCachePurge();
					page.HtmlOptions.UseResync = true;
				}#1#

		/*
				public string BuildPdfAbcpdf(int downloadID, bool isLight, ProposalPageList proposals) {
					//create document
					//String htmlText = "";
					var url = root + "Download/DownloadSinglePdfView?downloadID=" + downloadID;
					var newFileName = "pengellys-" + downloadID + "-" + Fmt.DateTimeCompressed(DateTime.Now) + ".pdf";
					var fileroot = Web.Server.MapPath("~");
					if (fileroot.LastIndexOf('\\') != fileroot.Length - 1) fileroot += "\\";
					var folderPath = fileroot + "attachments\\downloadPDF\\"; //must end in 			FileSystem.CreateFolder(folderPath);
					var fullServerPath = folderPath + newFileName;

					var theTemplateDoc = new Doc();
					if (isLight) {
						theTemplateDoc.Read(Server.MapPath("../../attachments/templates/penlightTemplate.pdf"));
					} else {
						theTemplateDoc.Read(Server.MapPath("../../attachments/templates/pendarkTemplate.pdf"));
					}

					theTemplateDoc.MediaBox.String = "A4";

					try {
						var theDoc = new Doc();
						theDoc.MediaBox.String = "A4"; // sets page to A4 & removes scrollbar appearing
						theDoc.HtmlOptions.Engine = EngineType.MSHtml;
						theDoc.HtmlOptions.FontSubstitute = false;
						theDoc.HtmlOptions.FontProtection = false;
						theDoc.HtmlOptions.FontEmbed = true;
						//clear caching?
						theDoc.HtmlOptions.PageCacheEnabled = false;
						theDoc.HtmlOptions.UseNoCache = true;
						theDoc.HtmlOptions.PageCacheClear();
						theDoc.HtmlOptions.PageCachePurge();
						theDoc.HtmlOptions.UseResync = true;
						//theDoc.Rect.String = "50 50 550 800";

						theDoc.MediaBox.String = theTemplateDoc.MediaBox.String;
						theDoc.Rect.String = theTemplateDoc.MediaBox.String;

						/*		for (int i = 1; i <= 1; i++) { // need to chain correctly
									theDoc.AddImageDoc(theTemplateDoc, i, null);
									theDoc.FrameRect();
									theDoc.Flatten();
								}#2#

						theDoc.Rect.String = "50 50 550 730";

						var id = theDoc.AddImageUrl(url, true, 1000, true);

						//	theDoc.Rect.Inset(20, 20);//bottom margin
						//add more pages if more than 1 page
						int count = 1;
						for (; theDoc.Chainable(id); ) {
							theDoc.Page = theDoc.AddPage();

							theDoc.AddImageDoc(theTemplateDoc, count, null);
							theDoc.FrameRect();
							theDoc.Flatten();

							id = theDoc.AddImageToChain(id);
							count++;
						}

						theDoc.Save(fullServerPath);
						theDoc.Clear();
					} catch (Exception ex) {
						throw new BewebException(ex + "\n\n" + url + "\n\n" + fullServerPath);
					}
					//Web.Write(url);
					//Web.End();

					return newFileName;
				}
		#1#




		public class PdfViewModel : PageTemplateViewModel {
			public Proposal Proposal = new Proposal();
			public ActiveRecordList<ProposalPage> ProposalPages { get; set; }
			public ProfileTemplate ProfileTemplate { get; set; }
		}*/


	}
}