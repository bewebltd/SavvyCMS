using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Beweb;
//using nQuant;
//using ImageMagick;

public partial class admin_tools_ImageCompressor : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		Security.RequireLogin(SecurityRolesCore.Roles.DEVELOPER);
	}

	//protected void CompressAttachments() {
	//	MagickImage image = new MagickImage(Web.MapPath("~/attachments/bigfoimage.png"));
	//	image.CompressionMethod = ImageMagick.CompressionMethod.ZipS;
	//	image.Resize(50);
	//	FileSystem.Delete(Web.MapPath("~/attachments/newbigfoimage.png"));
	//	image.Write(Web.MapPath("~/attachments/newbigfoimage.png"));
	//}

	protected void CompressMultiplePng() {

		var files = Directory.GetFiles(Web.MapPath(Web.Attachments), "*.*", SearchOption.AllDirectories);

		Web.Flush("Compressing Images into folder attachments/compressed/ <br>");
		foreach (var oldFile in files) {
			var attachmentName = oldFile.RightFrom(Web.MapPath(Web.Attachments)).ToLower();
			var ext = FileSystem.GetExtension(attachmentName);
			if (ext == ".png" || ext == ".jpeg" || ext == ".jpg") {
				if (!attachmentName.Contains("compressed\\")) {
					if (oldFile.Contains("nature_wallpaper")) {
						var originalFileSize = FileSystem.GetFileSize(oldFile);
						Web.Flush("Filename " + attachmentName + "... Orig File Size: " + originalFileSize + "kb");
						//FileSystem.CopyFile(file, Web.MapPath("~/attachments/originals/" + attachmentName));
						var newFile = Web.MapPath("~/attachments/compressed/" + attachmentName);
						ImageProcessing.CompressImage(oldFile, newFile);
						var fileSize = FileSystem.GetFileSize(newFile);
						Web.Flush(" New File Size: " + fileSize + "kb (" + Fmt.Number(Numbers.SafeDivide(fileSize, originalFileSize) * 100, 0) + "%) <br>");
					}

				}
			}
		}
	}


	//private static void CompressPng(string path, string newPath) {
	//	Bitmap image = new Bitmap(Image.FromFile(Web.MapPath(path)));
	//	WuQuantizer img = new WuQuantizer();
	//	Image newImg = img.QuantizeImage(image);
	//	FileSystem.Delete(newPath);
	//	newImg.Save(newPath);
	//}
}
