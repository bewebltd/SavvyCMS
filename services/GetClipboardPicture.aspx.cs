using System;
using System.IO;
using Beweb;
using Site.SiteCustom;

public partial class GetClipboardPicture : System.Web.UI.Page {

	protected void Page_Load(object sender, EventArgs e) {

		if (Request["uploadType"] == "attachment") {

			var subFolder = "";
			if (Request["subFolder"].IsNotBlank()) {
				subFolder = Request["subFolder"];
			}
			var file = AttachmentUpload(subFolder, Request["data"], Request["filename"]);
			var filePath = subFolder.IsNotBlank() ? subFolder + "//" + file : file;
			Response.Clear();
			Response.Write(filePath);
			Response.End();
			return;

		} else if (Request["uploadType"] == "Redactor") {
			//Response.Clear();
			var filePath = ImageUploader("redactor");
			Response.Write(filePath);
			Response.End();
			return;

		} else if (Request["uploadType"] == "iframe") {
			Response.Clear();
			var filePath = ImageUploader("iframe");
			Response.Write(filePath);
			Response.End();
		} else {
			string fileName = null;
			bool isMceUpload = Request["uploadType"] == "mce";
			if (isMceUpload) { 
				fileName = UploadMCE(Request["data"], Request["filename"]);            // todo - implement mce image popup paste
			} else {
				fileName = DragDataUpload(Request["data"]);
			}
			Response.Clear();
			Response.Write(fileName);
			Response.End();
		}
	}



	private string ImageUploader(string type) {
		string fileName = null;
		int filesize = 0;

		try {

			var file = Request.Files[0];

			if (file.ContentLength > 0) {

				const string subfolder = "paste/";
				FileSystem.CreateFolder(Web.Attachments + subfolder);

				fileName = subfolder + FileSystem.GetUniqueFilename(Web.Attachments + subfolder, file.FileName.RightFrom("\\"), 50);

				var metaData = new DefaultPictureMetaData() {
					ThumbnailWidth = 260, ThumbnailHeight = 175, IsThumbnailExact = false, IsThumbnailCropped = false, PreviewWidth = 60, IsExact = false, IsCropped = false,
					Height = Util.GetSetting("MCEUploadedImageHeight", "960").ToInt(960),
					Width = Util.GetSetting("MCEUploadedImageWidth", "960").ToInt(960)
				};

				fileName = ImageProcessing.ResizeImageUsingMetaData(fileName, metaData, file);
				filesize = file.ContentLength;
			} else {
				throw new Exception("No file selected");
			}

		} catch (Exception ex) {
			return "{\"success\": false, \"error\": \"" + ex.Message.Replace("\\", "\\\\") + "\"}";
		}

		if (type == "redactor") {
			return "{\"filelink\": \""+Web.Attachments+"paste/" + Path.GetFileName(fileName) + "\"}";
		} else {
			return "{\"success\": true, \"filename\": \"" + Path.GetFileName(fileName) + "\", \"filepath\": \"paste/" + Path.GetFileName(fileName) + "\", \"filesize\": \"" + Fmt.FileSize(filesize, 2) + "\"}";
		}
	}



	private string AttachmentUpload(string subFolder, string b64String, string fileName) {
		b64String = b64String.Substring(b64String.IndexOf(",") + 1);
		var bytes = Convert.FromBase64String(b64String);
		return SaveAttachmentFile(subFolder, bytes, fileName);
	}

	private string DragDataUpload(string b64String) {
		b64String = b64String.Substring(b64String.IndexOf(",") + 1);
		var bytes = Convert.FromBase64String(b64String);
		return SaveFile("paste", bytes, null, null);
	}

	public string UploadMCE(string b64String, string fileName) {
		String file = null;
		String returnResult = null;

		var metaData = new DefaultPictureMetaData() {
			ThumbnailWidth = 260, ThumbnailHeight = 175, IsThumbnailExact = false, IsThumbnailCropped = false, PreviewWidth = 60, IsExact = false, IsCropped = false,
			Height = Util.GetSetting("MCEUploadedImageHeight", "960").ToInt(960),
			Width = Util.GetSetting("MCEUploadedImageWidth", "960").ToInt(960)
		};

		try {
			b64String = b64String.Substring(b64String.IndexOf(",") + 1);
			var bytes = Convert.FromBase64String(b64String);
			//file = SaveFile("paste", bytes, metaData, fileName.RightFrom("\\"));
			file = SaveFile("pics", bytes, metaData, fileName.RightFrom("\\"));    // MN 20150226 - go directly to correct folder, was staying in paste
			returnResult = file;
		} catch (Exception e) {
			returnResult = "Error uploading file: [" + e.Message + "]";
			throw new Beweb.ProgrammingErrorException(returnResult);
		}
		return returnResult;
	}

	private string SaveFile(string uploadDir, byte[] decodedArray, PictureMetaDataAttribute metaData, string fileName) {
		string path = Server.MapPath(Web.Attachments);
		FileSystem.CreateFolder(path + uploadDir);
		string file;
		if (string.IsNullOrEmpty(fileName)) {
			file = uploadDir + "\\" +Guid.NewGuid() + ".png";
		} else {
			file = FileSystem.GetUniqueAttachmentFilename(uploadDir, fileName);
			//fileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\s+", "-");//remove white space //MN 20150226 replaced with GetUniqueAttachmentFilename
			//fileName = fileName.LeftUntilLast(".") + ".png"
		}

		//string file = uploadDir + "\\" + fileName;  //MN 20150226 replaced with GetUniqueAttachmentFilename
		var stream = new MemoryStream(decodedArray);
		if (metaData == null) {
			// save temp file, full size, we will process this later using UpdateFromRequest ActiveRecord MetaData (in hidden field "paste_")
			stream.SaveStreamToFile(path + file);
		} else {
			// for rich text edit pasting - we are not using UpdateFromRequest ActiveRecord MetaData so this is the final processing
			//stream.SaveStreamToFile(path + file);
			ImageProcessing.ResizeImageUsingMetaData(file, metaData, stream, "image/png");
		}
		file = file.Replace("\\", "/");
		return file;
	}


	private string SaveAttachmentFile(string subFolder, byte[] decodedArray, string fileName) {
		var stream = new MemoryStream(decodedArray);
		return SaveAttachmentFile(subFolder, stream, fileName);
	}

	private string SaveAttachmentFile(string subFolder, Stream decodedArray, string fileName) {
		String path = Server.MapPath("~") + String.Format("\\attachments\\");
		FileSystem.CreateFolder(path + subFolder);
		path = path + subFolder + "\\";
		/*if (string.IsNullOrEmpty(fileName)) {
			fileName = Guid.NewGuid() + ".png";
		} else {
			fileName = fileName.LeftUntilLast(".") + ".png";
		}
*/
		string file = FileSystem.GetUniqueFilename(path, fileName);
		var stream = decodedArray;
		// save temp file, full size, we will process this later using UpdateFromRequest ActiveRecord MetaData (in hidden field "paste_")
		stream.SaveStreamToFile(path + file);
		stream.Close();
		return file;
	}


}