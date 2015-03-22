using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Microsoft.SqlServer.Server;
using Models;
using Site.SiteCustom;

// this controller should be used as it ensures filenames are not presented as direct links to guessable urls and user be logged in to download any files

namespace Site.Controllers {

	//[Authorize]   // must have some kind of login to download any files - delete this line if that's not the case
	public class DownloadController : ApplicationController {
	/*
		public ActionResult DownloadDocument(string encryptedID) {
			int decryptedID = Crypto.DecryptID(encryptedID);
			var document = Models.Document.LoadID(decryptedID, Otherwise.NotFound);
			Web.DownloadFile(Web.Attachments + document.FileAttachment, document.Title);
			DocumentDownload download = new DocumentDownload();
			download.DocumentID = document.ID;
			download.DateAdded = DateTime.Now;
			if (UserSession.Person != null) {
				download.PersonID = UserSession.Person.ID;
			}
			download.Save();
			return null;
		}
		
	 * note: see PdfController for example of downloading a PDF
		*/

	}
}