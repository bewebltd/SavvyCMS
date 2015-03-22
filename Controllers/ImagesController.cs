using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beweb;
using Models;
using Site.SiteCustom;

namespace Site.Controllers {
	public class ImagesController : ApplicationController {



		//		public ActionResult ByGalleryID(int id, int? width, int? height) {
		//			var galleryImage = Gallery.LoadID(id, Otherwise.NotFound);
		//			string imageFilePath = galleryImage.ImagePath;
		//			return GetImageSized(imageFilePath, width, height, null);
		//		}

		///// <summary>
		/// image downloading and scaling/cropping.
		/// note imageFilePath must be the path without any suffix
		/// </summary>
		public ActionResult GetImageSized(string imageFilePath, int width, int height, string suffix) {
			if (suffix.IsBlank()) {
				suffix = "_" + width + "x" + height;
			}

			string originalFilePath = Web.MapPath(imageFilePath);
			string croppedFileVirtualPath = ImageProcessing.InsertSuffix(imageFilePath, suffix);
			string proxyImageUrl = null;   // eg "http://images.cruisefactory.net/images/" + imageUrl;

			// see if cropped file already made previously
			if (!FileSystem.FileExists(croppedFileVirtualPath)) {
				// see if original file is already downloaded
				if (proxyImageUrl != null && !FileSystem.FileExists(originalFilePath)) {
					// download it by proxying it in from another server
					FileSystem.CreateFolder(FileSystem.GetParentPath(originalFilePath));
					var webClient = new WebClient();
					webClient.DownloadFile(proxyImageUrl, originalFilePath);
				}
				// make sure it exists first!
				if (!FileSystem.FileExists(originalFilePath)) throw new BadUrlException("Image not found");
				// crop (or could resize within)
				Image originalImage = Image.FromFile(originalFilePath);
				string contentType = FileSystem.GetMimeType(originalFilePath);

				ImageProcessing.ResizeImageTo(originalImage, width, height, true, contentType, ColorTranslator.FromHtml("#FFFFFF"), false, croppedFileVirtualPath);
				//ImageProcessing.ResizeImageWithin(originalImage, width, height, contentType, Color.Transparent, false, croppedFileVirtualPath);
			}
			Web.DisplayImage(croppedFileVirtualPath);
			return null;
		}

		public ActionResult GetImageStandard(string imageUrl) {
			var metaData = new DefaultPictureMetaData();
			return GetImageSized(imageUrl, metaData.Width, metaData.Height, "_st");
		}

		public ActionResult GetImageThumb(string imageUrl) {
			var metaData = new DefaultPictureMetaData();
			return GetImageSized(imageUrl, metaData.ThumbnailWidth, metaData.ThumbnailHeight, "_tn");
		}

		public ActionResult GetImagePreview(string imageUrl) {
			var metaData = new DefaultPictureMetaData();
			return GetImageSized(imageUrl, metaData.PreviewWidth, metaData.PreviewHeight, "_pv");
		}

		public ActionResult GetImageSquare(string imageUrl) {
			var metaData = new DefaultPictureMetaData();
			return GetImageSized(imageUrl, 120, 120, "_sq");
		}


		public ActionResult TestRenderedImage() {
			return Redirect("Text?text=TAKE THE KIDS TO FIJI&recache=true&fontname=Times New Roman");
		}

		public ActionResult Text(string text, string fontname = "Arial", int fontsize = 9, int angle = 270, bool recache = false, bool bold = false) {
			ImageProcessing.DrawAngleText(text, fontname, fontsize, angle, recache, bold);
			return null;
		}

		//------------------------------------------------------------
		// DYNAMIC IMAGES
		//------------------------------------------------------------

		public ActionResult RenderDynamicImage(int id, int version) {
			var imgRecord = Models.DynamicImage.LoadID(id);
			if (imgRecord == null) {
				return null;  // broken image - should not happen?
			}

			var cachedFilePath = Web.MapPath(Web.Attachments + "DynamicImage/" + id + "_" + version + ".png");
			if (!FileSystem.FileExists(cachedFilePath)) {
				string originalFilePath = Web.MapPath( Web.Attachments+imgRecord["OriginalFilename"].ToString());
				int width = imgRecord["Width"].ToInt(100);
				int height = imgRecord["Height"].ToInt(100);
				string cropStyle = imgRecord["CropStyle"].ToString();
				string contentType = "image/png";
				if (cropStyle == "crop") {
					ImageProcessing.ResizeImageTo(originalFilePath, width, height, true, contentType, Color.Transparent, false, cachedFilePath);
				} else if (cropStyle == "exact") {
					ImageProcessing.ResizeImageTo(originalFilePath, width, height, false, contentType, Color.Transparent, false, cachedFilePath);
				} else if (cropStyle == "within") {
					ImageProcessing.ResizeImageWithin(originalFilePath, width, height, contentType, cachedFilePath);
				}
			}

			Web.DisplayImage(cachedFilePath);
			return null;
		}

		/// <summary>
		/// Returns an img src for a crop of the given image at the given size.
		/// Auto cache date and file name.
		/// </summary>
		public static string DynamicCrop(PictureActiveField pictureField, int width, int height) {
			return DynamicImage(pictureField, width, height, "crop");
		}

		/// <summary>
		/// Returns an img src for proportionately resized version of given image at the given max size. May be smaller.
		/// Auto cache date and file name.
		/// </summary>
		public static string DynamicWithin(PictureActiveField pictureField, int maxWidth, int maxHeight) {
			return DynamicImage(pictureField, maxWidth, maxHeight, "within");
		}

		/// <summary>
		/// Returns an img src for a proportionately resized version of the given image. Will scale and add whitespace to pad to exact size.
		/// Auto cache date and file name.
		/// </summary>
		public static string DynamicExact(PictureActiveField pictureField, int width, int height) {
			return DynamicImage(pictureField, width, height, "crop");
		}

		private static string DynamicImage(PictureActiveField pictureField, int width, int height, string cropStyle) {
			var moddate = pictureField.Record.Advanced.DateModified;
			var uniqueKey = pictureField.Record.GetTableName() + "_" + pictureField.Record.ID_Field.ValueObject  + "_"  + pictureField.Name + "_"  + width + "x" + height + "_" + cropStyle;
			var keywords = (pictureField.Record.GetName());
			string originalFilename = pictureField.ToString();

			CheckTable();
			//var sql = new Sql();
			//var record = new ActiveRecord("DynamicImage", "");
			//record.LoadData();
			//var imgRecord = ActiveRecordLoader.LoadByField<ActiveRecord>("UniqueKey", uniqueKey, "DynamicImage", Otherwise.New);
			//var imgRecord = ActiveRecordLoader.Load<ActiveRecord>(new Sql("select * from DynamicImage where UniqueKey=", uniqueKey.SqlizeText()), "DynamicImage");
			var imgRecord = Models.DynamicImage.LoadByUniqueKey(uniqueKey);
			if (imgRecord == null) {
				imgRecord = new DynamicImage();
			}
			if (imgRecord.IsNewRecord || imgRecord["ImageModDate"].ConvertToDate() != moddate) {
				var currentVersion = imgRecord["Version"].ToInt(0);
				if (!imgRecord.IsNewRecord) {
					// delete existing file to save space
					FileSystem.DeleteAttachment("DynamicImage/" + imgRecord.ID_Field.ValueObject + "_" + currentVersion + ".png");
				}
				// create the record
				imgRecord["UniqueKey"].ValueObject = uniqueKey;
				imgRecord["ImageUrl"].ValueObject = Fmt.Crunch(keywords);
				imgRecord["Version"].ValueObject = currentVersion + 1;
				imgRecord["CropStyle"].ValueObject = cropStyle;
				imgRecord["Width"].ValueObject = width;
				imgRecord["Height"].ValueObject = height;
				imgRecord["OriginalFilename"].ValueObject = originalFilename;
				imgRecord["ImageModDate"].ValueObject = moddate;
				imgRecord.Save();
			}
			// return file name
			return Web.Root + "i/" + imgRecord.ID_Field.ValueObject + "_" + imgRecord["Version"] + "/" + imgRecord["ImageUrl"] + ".png";
		}

		private static void CheckTable() {
			if (!BewebData.TableExists("DynamicImage")) {
				new Sql("CREATE TABLE [dbo].[DynamicImage]([DynamicImageID] [int] IDENTITY(1,1) NOT NULL, [ImageUrl] [nvarchar](250) NULL, [UniqueKey] [nvarchar](150) NULL, [Version] [int] NULL,[Width] [int] NULL,[Height] [int] NULL, [CropStyle] [nvarchar](10) NULL, [OriginalFilename] [nvarchar](250) NULL, [ImageModDate] [datetime] NULL, [LastModified] [datetime] NULL,  CONSTRAINT [DynamicImage_PK] PRIMARY KEY NONCLUSTERED ([DynamicImageID] ASC))").Execute();
				ActiveRecordGenerator.Run();
			}
		}

	}

}