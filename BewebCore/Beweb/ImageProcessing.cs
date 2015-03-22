//comment out if image manip not allowed
#define ImageManipulationAvailable
#define JpegCompressAvailable
#define nQuantAvailable

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

using System.Web;
using Beweb.EXIF;

#if JpegCompressAvailable
using BitMiracle.LibJpeg;
#endif
#if ImageManipulationAvailable
using ImageManipulation;
#endif
#if nQuantAvailable
using nQuant;
#endif

//using Models;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;

namespace Beweb {
	/// <summary>
	/// Summary description for ImageProcessing
	/// </summary>
	public class ImageProcessing {
		public enum AnchorPosition { Top, Bottom, Left, Right }
		protected Image localImage;

		//public static Size GetImageDimensionsSlow(string filename) {
		//	Size result = new Size();
		//	filename = Web.MapPath(filename);
		//	if (File.Exists(filename)) {
		//		using (Image originalImage = Bitmap.FromFile(filename)) {
		//			result = originalImage.Size;
		//			originalImage.Dispose();
		//		}
		//	}
		//	return result;
		//}

		public static Size GetImageDimensions(string filename) {
			Size result = new Size();
			filename = Web.MapPath(filename);
			if (File.Exists(filename)) {
				using (var fileStream = new FileStream(filename, FileMode.Open)) {
					using (Image originalImage = Image.FromStream(fileStream, false, false)) {
						result = originalImage.Size;
						originalImage.Dispose();
					}
				}
			}
			return result;
		}

		public static string GetImageDimensionsCss(string filename) {
			var dims = GetImageDimensions(filename);
			string result = "width:" + dims.Width + "px;height:" + dims.Height + "px;";
			return result;
		}

		#region ResizeImageWithin
		/// <summary>
		/// scales an image within the specified box
		/// </summary>
		/// <param name="originalImage"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <param name="fileType">of the form "image/jpg"</param>
		/// <param name="isDatabaseStore">if true, the image will be saved to the database, otherwise to the fileNameWithPath specified</param>
		/// <param name="fileNameWithPath">of the form Web.Attachments + "image.jpg"</param>
		/// <returns>the fileNameWithPath or the newly inserted id if isDatabaseStore = true</returns>
		public static string ResizeImageWithin(Image originalImage, int maxWidth, int maxHeight, string fileType, bool isDatabaseStore, string fileNameWithPath) {
			return ResizeImageWithin(originalImage, maxWidth, maxHeight, fileType, Color.White, isDatabaseStore, fileNameWithPath);
		}

		//MN - Has not been tested
		public static string ResizeImageWithin(string existingImageFileName, int width, int height, string imageMineType, string desiredFileName) {
			string result = null;
			try {
				using (Image image = Bitmap.FromFile(existingImageFileName, false)) {
					result = ResizeImageWithin(image, width, height, imageMineType, false, desiredFileName);
				}
			} catch (System.ArgumentException e) {
				string getFileName = existingImageFileName;
				throw new UserErrorException("Sorry, we could not process that image. The format was not recognised. Please re-save as a standard JPG or GIF file, and try again. The file name is " + getFileName + " (file type is " + imageMineType + "). Error Code: 45BWB7. " + e.Message); //Failed to save this type of image ["+fileName+"] fileType["+fileType+"]");
			}
			return result;
		}

		//20131223jn added for backward compat
		public static bool ResizeImageWithin(Image originalImage, int maxWidth, int maxHeight, string savePath, string fileType) {
			ResizeImageWithin(originalImage, maxWidth, maxHeight, fileType, Color.White, false, savePath);
			return true;
		}


		/// <summary>
		/// scales an image within the specified box
		/// </summary>
		/// <param name="originalImage"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <param name="fileType">of the form "image/jpg"</param>
		/// <param name="isDatabaseStore">if true, the image will be saved to the database, otherwise to the fileNameWithPath specified</param>
		/// <param name="fileNameWithPath">of the form Web.Attachments + "image.jpg"</param>
		/// <returns>the fileNameWithPath or the newly inserted id if isDatabaseStore = true</returns>
		public static string ResizeImageWithin(Image originalImage, int maxWidth, int maxHeight, string fileType, Color backgroundColor, bool isDatabaseStore, string fileNameWithPath) {
			int oldWidth = originalImage.Width;
			int oldHeight = originalImage.Height;
			float newAspect = (float)maxWidth / (float)maxHeight;
			float oldAspect = (float)oldWidth / (float)oldHeight;
			int newWidth;
			int newHeight;

			if (oldWidth <= maxWidth && oldHeight <= maxHeight) {
				// don't need to resize - the image is smaller than our maxes
				// or exactly right
				newWidth = oldWidth;
				newHeight = oldHeight;
			} else if (oldAspect < newAspect) {
				// use height
				newHeight = maxHeight;
				newWidth = Convert.ToInt32(maxHeight * oldAspect);
			} else {
				// use width
				newWidth = maxWidth;
				newHeight = Convert.ToInt32(maxWidth / oldAspect);
			}

			// we know exactly the new width and height we want, so canvas size = new size
			return ResizeAndSaveImage(originalImage, newWidth, newHeight, newWidth, newHeight, backgroundColor, fileType, isDatabaseStore, fileNameWithPath);
		}
		#endregion

		#region ResizeImageTo

		public static string ResizeImageTo(string originalImageFileName, int maxWidth, int maxHeight, bool trimImage, string fileType, Color backgroundColor, bool isDatabaseStore, string outputOutputFileNameWithPath) {
			originalImageFileName = Web.MapPath(originalImageFileName);
			string result = null;
			using (Image originalImage = Image.FromFile(originalImageFileName)) {
				result = ImageProcessing.ResizeImageTo(originalImage, maxWidth, maxHeight, trimImage, fileType, backgroundColor, isDatabaseStore, outputOutputFileNameWithPath);
			}
			return result;
		}


		//20131223 js added for backwards compat with allpowerau
		public static bool ResizeImageTo(Image originalImage, int maxWidth, int maxHeight, bool trimImage, string savePath, string fileType, Color backgroundColor) {
			ResizeImageTo(originalImage, maxWidth, maxHeight, trimImage, fileType, backgroundColor, false, savePath);
			return true;
		}


		/// <summary>
		/// Resizes images to the dimensions specified, either with or without cropping (set trimImage=true to crop). 
		/// If trimImage is true, it will scale down until either the width or height fits and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. In either case, the image will be filled with the background colour if necessary to make it the exact size.
		/// </summary>
		/// <param name="originalImage"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <param name="trimImage">If trimImage is true, it will scale down until either the width or height fits and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. In either case, the image will be filled with the background colour if necessary to make it the exact size.</param>
		/// <param name="fileType">i.e. "image/gif" get from UploadField.PostedFile.ContentType</param>
		/// <param name="backgroundColor">i.e. Color.White or ColorTranslator.FromHtml("#F5F7F8") </param>
		/// <param name="isDatabaseStore">if true, the image will be saved to the database, if false the filesystem is used</param>
		/// <param name="outputFileNameWithPath">path and file name to save file as</param>
		/// <returns>the fileNameWithPath or the newly inserted id if isDatabaseStore = true</returns>
		public static string ResizeImageTo(Image originalImage, int maxWidth, int maxHeight, bool trimImage, string fileType, Color backgroundColor, bool isDatabaseStore, string outputFileNameWithPath) {
			return ResizeImageTo(originalImage, maxWidth, maxHeight, trimImage, fileType, backgroundColor, isDatabaseStore, outputFileNameWithPath, true);
		}

		/// <summary>
		/// Resizes images to the dimensions specified, either with or without cropping (set trimImage=true to crop). 
		/// If trimImage is true, it will scale down until either the width or height fits and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. In either case, the image will be filled with the background colour if necessary to make it the exact size.
		/// </summary>
		/// <param name="originalImage"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <param name="trimImage">If trimImage is true, it will scale down until either the width or height fits and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. In either case, the image will be filled with the background colour if necessary to make it the exact size.</param>
		/// <param name="fileType">i.e. "image/gif" get from UploadField.PostedFile.ContentType</param>
		/// <param name="backgroundColor">i.e. Color.White or ColorTranslator.FromHtml("#F5F7F8") </param>
		/// <param name="isDatabaseStore">if true, the image will be saved to the database, if false the filesystem is used</param>
		/// <param name="outputFileNameWithPath">path and file name to save file as</param>
		/// <param name="isExact">true to add padding when cropping and one side is smaller but the other bigger</param>
		/// <returns>the fileNameWithPath or the newly inserted id if isDatabaseStore = true</returns>
		public static string ResizeImageTo(Image originalImage, int maxWidth, int maxHeight, bool trimImage, string fileType, Color backgroundColor, bool isDatabaseStore, string outputFileNameWithPath, bool isExact) {
			// TODO: needs options to resize smaller images bigger - or place them on a canvas of a certain colour - or leave them

			int oldWidth = originalImage.Width;
			int oldHeight = originalImage.Height;
			float newAspect = (float)maxWidth / (float)maxHeight;
			float oldAspect = (float)oldWidth / (float)oldHeight;
			int newWidth;
			int newHeight;
			// we never want to enlarge images that are too small, but if we ever do in future, a new version of this function could be made with this as an additional parameter
			// it is better not to add new boolean parameters though as it will make it more confusing to call
			const bool enlargeIfTooSmall = false;

			if (trimImage) {  // crop
				if (oldAspect >= newAspect) {
					// trim sides
					if (oldHeight < maxHeight && !enlargeIfTooSmall) {
						// don't enlarge it - the image is smaller than our max
						newHeight = oldHeight;
					} else {
						newHeight = maxHeight;
					}
					newWidth = Convert.ToInt32(newHeight * oldAspect);
				} else {
					// trim top and bottom
					if (oldWidth < maxWidth && !enlargeIfTooSmall) {
						// don't enlarge it - the image is smaller than our max
						newWidth = oldWidth;
					} else {
						newWidth = maxWidth;
					}
					newHeight = Convert.ToInt32(newWidth / oldAspect);
				}
				
				// if it is crop but not exact, then this needs differnt treatment
				if (!isExact) {
					if (oldWidth < maxWidth) {
						// don't need to resize - the image is smaller than our maxes
						maxWidth = oldWidth;
					}
					if (oldHeight < maxHeight) {
						// don't need to resize - the image is smaller than our maxes
						maxHeight = oldHeight;
					}
				}

			} else {
				// make an image of the desired size - don't crop though
				if (oldWidth < maxWidth && oldHeight < maxHeight) {
					// don't need to resize - the image is smaller than our maxes
					newWidth = oldWidth;
					newHeight = oldHeight;
				} else if (oldAspect < newAspect) {
					// use height
					newHeight = maxHeight;
					newWidth = Convert.ToInt32(maxHeight * oldAspect);
				} else {
					// use width
					newWidth = maxWidth;
					newHeight = Convert.ToInt32(maxWidth / oldAspect);
				}
			}
			// canvas size = maxes, new image size = new sizes
			return ResizeAndSaveImage(originalImage, maxWidth, maxHeight, newWidth, newHeight, backgroundColor, fileType, isDatabaseStore, outputFileNameWithPath);
		}
		#endregion

		#region ResizeAndSaveImage
		/// <summary>
		/// a generic rezie - usually called by another function in this class
		/// </summary>
		/// <param name="originalImage"></param>
		/// <param name="canvasWidth"></param>
		/// <param name="canvasHeight"></param>
		/// <param name="newWidth"></param>
		/// <param name="newHeight"></param>
		/// <param name="bgColor"></param>
		/// <param name="fileType"></param>
		/// <param name="isDatabaseStore"></param>
		/// <param name="fileNameWithPath"></param>
		/// <returns>the fileNameWithPath or the newly inserted id if isDatabaseStore = true</returns>
		public static string ResizeAndSaveImage(Image originalImage, int canvasWidth, int canvasHeight, int newWidth, int newHeight, Color bgColor, string fileType, bool isDatabaseStore, string fileNameWithPath) {
			// this does two main things:
			// makes a high quality canvas
			// then positions the image on that (in the middle), resizing it at the same time			
			string returnVal = String.Empty;

			// make the canvas
			if (canvasWidth == 0 || canvasHeight == 0) { canvasWidth = 200; canvasHeight = 200; }//default picture - this would crash if these are zero

			using (Bitmap newImage = new Bitmap(canvasWidth, canvasHeight, PixelFormat.Format32bppArgb)) { //System.Drawing.Imaging.PixelFormat.Format32bppArgb
				//newImage.SetResolution(200, 200); // do not do this - the quantizer can't handle better resolutions
				using (Graphics new_g = Graphics.FromImage(newImage)) {
					new_g.SmoothingMode = SmoothingMode.HighQuality;
					new_g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					new_g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					new_g.CompositingQuality = CompositingQuality.HighQuality;
					if (bgColor != Color.Transparent) {
						new_g.Clear(bgColor); // set the canvas colour - this is the background colour if the resized image doesn't fit the canvas exactly
					}
					int posLeft = canvasWidth / 2 - newWidth / 2;
					int posTop = canvasHeight / 2 - newHeight / 2;
					// draw the original image on our new canvas, positioned in the centre, with the dimensions specified (these dims can be different to canvas size)
					new_g.DrawImage(originalImage, posLeft, posTop, newWidth, newHeight);

					// you can get transparent jpegs but we cannot save them, so save as png if we have transparent as default background - this means we are very often saving as pngs This is a bad IDEA
					if (Util.GetSettingBool("ForcePNG", false) && fileType == "image/jpeg") {
						fileType = "image/png";
						fileNameWithPath = fileNameWithPath.LeftUntilLast(".") + ".png";
					}

					if (isDatabaseStore) {
						returnVal = SaveImageToDatabase(newImage, fileNameWithPath, fileType);
					} else {
						returnVal = SaveImageToFile(newImage, fileNameWithPath, fileType);
					}
				}
			}
			return returnVal;
		}
		#endregion


		public static string SaveImageToFile(Image image, string fileName) {
			return SaveImageToFile(image, fileName, null);
		}

		public static string SaveImageToFile(Image image, string fileName, string fileType) {
			Bitmap bm = new Bitmap(image);
			return SaveImageToFile(bm, fileName, fileType);
		}

		public static string SaveImageToFile(Bitmap image, string fileName) {
			return SaveImageToFile(image, fileName, null);
		}

		public static string CompressImage(string oldFile, string path) {
			return CompressImage(oldFile, path, null);
		}

		public static string CompressImage(string oldFile, string path, string mimetype) {
			return SaveImageToFile(Image.FromFile(Web.MapPath(oldFile)), path, mimetype);
		}

		public static string SaveImageToFile(Bitmap image, string fileName, string mimeType) {
			// default mime type from filename
			if (mimeType == null) {
				if (fileName.EndsWith(".gif")) {
					mimeType = "image/gif";
				} else if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg")) {
					mimeType = "image/jpeg";
				} else {
					mimeType = "image/png";
					fileName = fileName.LeftUntilLast(".") + ".png";
				}
			}

			//Web.Write(" - Colors: - " +  CountImageColors(image) + " - Colors: - " ); // this is extremely slow.

			//image.RawFormat.Equals(ImageFormat.Png); will test the inital bits of an image for file type

			// for TIFFs, BMPs, WMFs etc save as PNG
			if (mimeType != "image/png" && mimeType != "image/gif" && mimeType != "image/jpeg") {
				mimeType = "image/png";
				fileName = fileName.LeftUntilLast(".") + ".png";
			}

			// check folder path is there
			var filePath = Web.MapPath(fileName);
			string folder = FileSystem.GetParentPath(filePath);
			if (!System.IO.Directory.Exists(folder)) {
				FileSystem.CreateFolder(folder);
			}
			if (!System.IO.Directory.Exists(folder)) { //double check create folder here
				throw new ProgrammingErrorException("Image Processing - Attempted to save image and could not create the directory. Directory [" + folder + "]");
			}

			// save out using appropriate compression
			switch (mimeType) {
#if nQuantAvailable
				case "image/png": {
						WuQuantizer img = new WuQuantizer();
						using (Image newImg = img.QuantizeImage(image)) {
							FileSystem.Delete(filePath);
							try {
								newImg.Save(filePath);
							} catch (System.Runtime.InteropServices.ExternalException e) {
								throw new UserErrorException("Sorry, we could not save that image. The file name is " + fileName + " (file type is " + mimeType + ").  Error Code: 55BWB8 WuQuantizer_SAVE_ERROR.", e);
							}
						}
						break;
					}
#endif
#if ImageManipulationAvailable
				case "image/gif": {
						// for gifs we need to make up our own palette - this requires a reference to the ImageManipulation.dll
						OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
						using (Bitmap quantized = quantizer.Quantize(image)) {
							try {
								quantized.Save(filePath, ImageFormat.Gif);
							} catch (System.Runtime.InteropServices.ExternalException e) {
								throw new UserErrorException("Sorry, we could not save that image. The file name is " + fileName + " (file type is " + mimeType + ").  Error Code: 55BWB9 OctreeQuantizer_SAVE_ERROR.", e);
							}
						}
						break;
					}
#endif
#if JpegCompressAvailable
				case "image/jpeg": {
						// use libJpeg to compress image as it is faster
						using (JpegImage jpeg = new JpegImage(image)) {
							var imageStream = new MemoryStream();
							jpeg.WriteJpeg(imageStream, new CompressionParameters() { Quality = 85 });
							FileSystem.SaveStreamToFile(imageStream, filePath);

						}

						break;
					}
#endif
				default: {
						// use .net framework system default image compression/saving for JPEGs or if not using external quantizer DLLs
						long nQuality = 85; // set the quality of the image we are saving
						EncoderParameters encoderParameters = new EncoderParameters(2);
						encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, nQuality);
						encoderParameters.Param[1] = new EncoderParameter(Encoder.ColorDepth, 65536);
						ImageCodecInfo encoder = null;
						encoder = GetImageEncoder(mimeType);
						if (encoder == null) {
							throw new UserErrorException("Sorry, we could not process that image. The format was not recognised. Please re-save as a standard JPG, GIF or PNG file, and try again. You will need to convert it to an RGB JPG. The file name is " + fileName + " (file type is " + mimeType + ").  Error Code: 55BWB6 NO_ENCODER.");
						}

						if (File.Exists(filePath)) {
							throw new ProgrammingErrorException("File [" + filePath + "] already exists");
						}

						try {
							image.Save(filePath, encoder, encoderParameters);
						} catch (System.Runtime.InteropServices.ExternalException e) {
							throw new UserErrorException("Sorry, we could not save that image. The file name is " + fileName + " (file type is " + mimeType + ").  Error Code: 55BWB7 IMG_SAVE_ERROR.", e);
						}
					}
					break;
			}
			return fileName;
		}

		/// <summary>
		/// Get a GDI ImageEncoder by mime type / content type
		/// eg ImageCodecInfo gifEncoder = GetImageEncoder("image/gif");
		/// </summary>
		public static ImageCodecInfo GetImageEncoder(string mimeType) {
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo encoder in encoders) {
				if (encoder.MimeType == mimeType) {
					return encoder;
				}
			}
			return null;
		}

		/// <summary>
		/// Get a GDI ImageEncoder by ImageFormat
		/// eg ImageCodecInfo gifEncoder = GetImageEncoder(ImageFormat.Gif);
		/// </summary>
		public static ImageCodecInfo GetImageEncoder(ImageFormat format) {
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo encoder in encoders) {
				if (encoder.FormatID == format.Guid) {
					return encoder;
				}
			}
			return null;
		}

		public static string SaveImageToDatabase(Image image, string clientFileName, string fileType) {
			// we have to loop through all the encoders in the system to get the right one to save our image
			ImageCodecInfo foundEncoder = null;
			long nQuality = 100; // set the quality of the image we are saving
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
			EncoderParameters encoderParameters = new EncoderParameters(2);
			encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, nQuality);
			encoderParameters.Param[1] = new EncoderParameter(Encoder.ColorDepth, 65536);
			foreach (ImageCodecInfo encoder in encoders) {
				if (encoder.MimeType == fileType) {
					foundEncoder = encoder;
					break;
				}
			}
			if (foundEncoder == null) return "0"; //

			// we need the bitmap as a stream
			MemoryStream imageStream = new MemoryStream();
			image.Save(imageStream, foundEncoder, encoderParameters);
			imageStream.Position = 0; // reset the stream back to the start
			// we need the stream as a byte array
			byte[] imageBytes = new byte[imageStream.Length];
			imageStream.Read(imageBytes, 0, imageBytes.Length);

			string sql =
				"INSERT INTO TIMAGE (IMAGE_DATA, UPLOADED_TS, MIME_TYPE_TXT, IMAGE_FILE_NM, IMAGE_FILESIZE_MSMT) VALUES (@IMAGE_DATA, @UPLOADED_TS, @MIME_TYPE_TXT, @IMAGE_FILE_NM, @IMAGE_FILESIZE_MSMT)";
			// make it return the newly inserted id
			sql = sql.Trim();
			sql += sql.EndsWith(";") ? "" : ";"; // make sure the sql statement always ends with a semicolon
			sql += " SELECT InsertedId = @@Identity"; // SELECT @InsertedId = SCOPE_IDENTITY

			// have to use this way of inserting - BewebData.InsertRecord(sql, pc) doesn't seem to work
			var myConnection = new SqlConnection(BewebData.GetConnectionString());
			var myCommand = new SqlCommand(sql, myConnection);
			myCommand.Parameters.AddWithValue("@IMAGE_DATA", imageBytes);
			myCommand.Parameters.AddWithValue("@UPLOADED_TS", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss"));
			myCommand.Parameters.AddWithValue("@MIME_TYPE_TXT", fileType);
			myCommand.Parameters.AddWithValue("@IMAGE_FILE_NM", clientFileName);
			myCommand.Parameters.AddWithValue("@IMAGE_FILESIZE_MSMT", imageBytes.Length);
			myConnection.Open();
			string newlyInsertedId = myCommand.ExecuteScalar().ToString();
			myConnection.Close();

			imageStream.Close();

			return newlyInsertedId; // use this to show the image "~/images/dbimage.aspx?i=" + newlyInsertedId
		}

		#region InsertSuffix
		/// <summary>
		/// Inserts suffix such as "_tn" into the filename for getting a thumbnail etc.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="suffix">suffix such as "_tn"</param>
		/// <returns></returns>
		public static string InsertSuffix(string filename, string suffix) {
			// MN 20100908 now moved this method to FileSystem for fun
			return FileSystem.InsertSuffix(filename, suffix);
		}
		#endregion

		#region ImagePath
		/// <summary>
		/// Used for setting SRC attribute of IMG tag.
		/// Maps filename to attachments path (so you should supply filename only without any path or relative to attachments path).
		/// Also inserts suffix such as "_tn" into the filename for getting a thumbnail etc.
		/// If filename is blank, returns empty string.
		/// </summary>
		/// <param name="filename">filename of image (eg "this-is-a-duck.jpg")</param>
		/// <param name="suffix">suffix such as "_tn"</param>
		/// <returns></returns>
		public static string ImagePath(string filename, string suffix) {
			if (filename.IsBlank()) return "";
			if (suffix.IsNotBlank() && !suffix.StartsWith("_")) {
				suffix = "_" + suffix;
			}
			string returnValue = InsertSuffix(filename, suffix);

			if (Web.IsAbsoluteUrl(returnValue)) {
				return returnValue;
			}
			if (returnValue.ToLower().Contains(Web.Attachments)) returnValue = returnValue.Replace(Web.Attachments, "");
			if (returnValue.ToLower().Contains("attachments/")) returnValue = returnValue.Replace("attachments/", "");
			returnValue = Web.Attachments + returnValue;
			return returnValue;
		}

		public static string ImagePath(string filename) {
			return ImagePath(filename, null);
		}

		public static string ImageThumbPath(string filename) {
			return ImagePath(filename, "_tn");
		}

		public static string ImagePreviewPath(string filename) {
			return ImagePath(filename, "_pv");
		}

		public static string ImageMediumPath(string filename) {
			return ImagePath(filename, "_med");
		}

		public static string ImageSmallPath(string filename) {
			return ImagePath(filename, "_sml");
		}

		public static string ImageBigPath(string filename) {
			return ImagePath(filename, "_big");
		}

		public static string ImageZoomPath(string filename) {
			return ImagePath(filename, "_zm");
		}

		public static string ImagePanelDisplayPath(string filename) {
			return ImagePath(filename, "_pd");
		}

		#endregion

		// need to take the functionality of the following but use the better saving and quality of the methods above this line
		#region ScaleByPercent
		public static Image ScaleByPercent(Image imgPhoto, int Percent) {
			float nPercent = ((float)Percent / 100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0;
			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
				PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
				imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
		#endregion

		#region FixedSize
		public static Image FixedSize(Image imgPhoto, int Width, int Height)// 
		{
			//Image imgPhoto = this.localImage;
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width / (float)sourceWidth);
			nPercentH = ((float)Height / (float)sourceHeight);
			if (nPercentH < nPercentW) {
				nPercent = nPercentH;
				destX = System.Convert.ToInt16((Width -
					(sourceWidth * nPercent)) / 2);
			} else {
				nPercent = nPercentW;
				destY = System.Convert.ToInt16((Height -
					(sourceHeight * nPercent)) / 2);
			}

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height,
				PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
				imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			//grPhoto.Clear(Color.Red);
			grPhoto.InterpolationMode =
				InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
		#endregion

		#region Crop
		public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor) {
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width / (float)sourceWidth);
			nPercentH = ((float)Height / (float)sourceHeight);

			if (nPercentH < nPercentW) {
				nPercent = nPercentW;
				switch (Anchor) {
					case AnchorPosition.Top:
						destY = 0;
						break;
					case AnchorPosition.Bottom:
						destY = (int)
							(Height - (sourceHeight * nPercent));
						break;
					default:
						destY = (int)
							((Height - (sourceHeight * nPercent)) / 2);
						break;
				}
			} else {
				nPercent = nPercentH;
				switch (Anchor) {
					case AnchorPosition.Left:
						destX = 0;
						break;
					case AnchorPosition.Right:
						destX = (int)
							(Width - (sourceWidth * nPercent));
						break;
					default:
						destX = (int)
							((Width - (sourceWidth * nPercent)) / 2);
						break;
				}
			}

			int destWidth = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width,
				Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
				imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode =
				InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto,
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
		#endregion

		/// <summary>
		/// Opens given image filename, opens the existing file, resizes it if necessary and generates thumbnails, according to supplied metadata.
		/// existingImageFileName must be an existing file and is relative to the attachments folder.
		/// </summary>
		/// <param name="existingImageFileName">existingImageFileName must be an existing file and is relative to the attachments folder</param>
		/// <param name="metaData"></param>
		/// <returns></returns>
		public static string ResizeImageUsingMetaData(string existingImageFileName, PictureMetaDataAttribute metaData) {
			string result = null;
			using (FileStream fileStream = System.IO.File.Open(Web.MapPath(Web.Attachments + existingImageFileName), FileMode.Open)) {
				result = ResizeImageUsingMetaData(existingImageFileName, metaData, fileStream);
			}
			return result;
		}

		/// <summary>
		/// Opens given image filename, opens the existing file, resizes it if necessary and generates thumbnails, according to supplied metadata.
		/// existingImageFileName must be an existing file and is an absolute path to the image.
		/// desiredFileName is the filename to save as, relative to the attachments folder.
		/// The return value is the created filename (which may be different to desiredFileName if that file already exists).
		/// Note that the parameters are in an unexpected order!
		/// </summary>
		/// <param name="existingImageFileName">existingImageFileName must be an existing file and is relative to the attachments folder</param>
		/// <param name="desiredFileName"></param>
		/// <param name="metaData"></param>
		/// <returns></returns>
		public static string ResizeImageUsingMetaData(string desiredFileName, PictureMetaDataAttribute metaData, string existingImageFileName) {
			string result = null;
			using (FileStream fileStream = System.IO.File.Open(existingImageFileName, FileMode.Open)) {
				result = ResizeImageUsingMetaData(desiredFileName, metaData, fileStream);
			}
			return result;
		}

		/// <summary>
		/// Given a desired filename and an already open FileStream to an image file, resizes it if necessary and generates thumbnails, according to supplied metadata.
		/// ContentType is detected from the file name extension.
		/// desiredFileName must be relative to the attachments folder.
		/// </summary>
		/// <param name="desiredFileName">desiredFileName must be relative to the attachments folder</param>
		/// <param name="metaData"></param>
		/// <param name="fileStream">an already open FileStream to an image file</param>
		/// <returns></returns>
		public static string ResizeImageUsingMetaData(string desiredFileName, PictureMetaDataAttribute metaData, FileStream fileStream) {
			string result;
			string contentType = FileSystem.GetMimeType(FileSystem.GetExtension(desiredFileName));
			result = ResizeImageUsingMetaData(desiredFileName, metaData, fileStream, contentType);
			return result;
		}

		/// <summary>
		/// Given a desired filename and an HttpPostedFile (from Web.Request.Files[] collection), resizes image if necessary and generates thumbnails, according to supplied metadata.
		/// ContentType is detected from the HttpPostedFile.
		/// desiredFileName must be relative to the attachments folder.
		/// </summary>
		/// <param name="desiredFileName"></param>
		/// <param name="metaData"></param>
		/// <param name="httpPostedFile">A posted file from the Web.Request.Files[] collection</param>
		/// <returns></returns>
		/// <example>
		/// ImageProcessing.ResizeImageUsingMetaData(fileName, new Recipe.PictureMetaData(), Web.Request.Files["recipeImage"]);
		/// </example>
		public static string ResizeImageUsingMetaData(string desiredFileName, PictureMetaDataAttribute metaData, HttpPostedFile httpPostedFile) {
			string result;
			var stream = httpPostedFile.InputStream;
			var contentType = httpPostedFile.ContentType;
			result = ResizeImageUsingMetaData(desiredFileName, metaData, stream, contentType);
			return result;
		}

		/// <summary>
		/// Given a filename and open filestream to an image file, resizes image if necessary and generates thumbnails, according to supplied metadata.
		/// desiredFileName must be relative to the attachments folder.
		/// </summary>
		/// <param name="desiredFileName"></param>
		/// <param name="metaData"></param>
		/// <param name="stream">a stream of image data (eg FileStream or InputStream)</param>
		/// <param name="contentType">MIME type of file</param>
		/// <returns></returns>
		public static string ResizeImageUsingMetaData(string desiredFileName, PictureMetaDataAttribute metaData, Stream stream, string contentType) {
			string result;
			using (var imageUploader = new ImageUploader()) {
				imageUploader.FileName = desiredFileName;
				imageUploader.fileData = stream;
				imageUploader.fileContentType = contentType;
				imageUploader.MetaData = metaData;
				// preview/crop etc
				imageUploader.SetupImageFile();
				imageUploader.SaveImageFile();
				// correct filename
				result = imageUploader.ImageName;
			}

			if (Util.GetSettingBool("ForcePNG", false) && contentType == "image/jpeg") {
				result = result.LeftUntilLast(".") + ".png";
			}

			return result;
		}

		public static void DeleteImageAllVersions(string fileName) {
			var attachmentPath = HttpContext.Current.Server.MapPath(Web.Attachments) + "\\";
			File.Delete(attachmentPath + fileName);
			File.Delete(attachmentPath + InsertSuffix(fileName, "_tn"));
			File.Delete(attachmentPath + InsertSuffix(fileName, "_pv"));
			File.Delete(attachmentPath + InsertSuffix(fileName, "_med"));
			File.Delete(attachmentPath + InsertSuffix(fileName, "_sml"));
			File.Delete(attachmentPath + InsertSuffix(fileName, "_big"));
			File.Delete(attachmentPath + InsertSuffix(fileName, "_zm"));
		}

		public static string ContrastColour(string source) {
			Color sc = ColorTranslator.FromHtml(source);
			return HexConverter(ContrastColor(sc));
		}

		public static Color ContrastColor(Color color) {
			int d = 0;

			// Counting the perceptive luminance - human eye favors green color... 
			double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

			if (a < 0.5)
				d = 0; // bright colors - black font
			else
				d = 255; // dark colors - white font

			return Color.FromArgb(d, d, d);
		}

		private static String HexConverter(System.Drawing.Color c) {
			return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
		}

		private static String RGBConverter(System.Drawing.Color c) {
			return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
		}


		private static SizeF GetEvenTextImageSize(string text, Font font) {
			using (var image = new Bitmap(1, 1, PixelFormat.Format32bppArgb)) {
				using (Graphics graphics = Graphics.FromImage(image)) {
					return graphics.MeasureString(text, font);
				}
			}
		}

		private static SizeF GetRotatedTextImageSize(SizeF fontSize, int angle) {
			// Source: http://www.codeproject.com/KB/graphics/rotateimage.aspx


			double theta = angle * Math.PI / 180.0;

			while (theta < 0.0)
				theta += 2 * Math.PI;

			double adjacentTop, oppositeTop;
			double adjacentBottom, oppositeBottom;

			if ((theta >= 0.0 && theta < Math.PI / 2.0) || (theta >= Math.PI && theta < (Math.PI + (Math.PI / 2.0)))) {
				adjacentTop = Math.Abs(Math.Cos(theta)) * fontSize.Width;
				oppositeTop = Math.Abs(Math.Sin(theta)) * fontSize.Width;
				adjacentBottom = Math.Abs(Math.Cos(theta)) * fontSize.Height;
				oppositeBottom = Math.Abs(Math.Sin(theta)) * fontSize.Height;
			} else {
				adjacentTop = Math.Abs(Math.Sin(theta)) * fontSize.Height;
				oppositeTop = Math.Abs(Math.Cos(theta)) * fontSize.Height;
				adjacentBottom = Math.Abs(Math.Sin(theta)) * fontSize.Width;
				oppositeBottom = Math.Abs(Math.Cos(theta)) * fontSize.Width;
			}

			int nWidth = (int)Math.Ceiling(adjacentTop + oppositeBottom);
			int nHeight = (int)Math.Ceiling(adjacentBottom + oppositeTop);

			return new SizeF(nWidth, nHeight);

		}
		/// <summary>
		/// call from a controller like this:
		/// </summary>
		/// <example>
		/// 
		/// public ActionResult Text(string text, string fontname = "Arial", int fontsize = 9, int angle = 270, bool recache = false, bool bold = false) {
		/// 	(new ImageProcessing()).DrawAngleText(text, fontname, fontsize, angle, recache, bold);
		/// 	return null;
		/// }
		/// </example>
		/// <param name="text"></param>
		/// <param name="fontname"></param>
		/// <param name="fontsize"></param>
		/// <param name="angle"></param>
		/// <param name="recache"></param>
		/// <param name="bold"></param>
		public static void DrawAngleText(string text, string fontname, int fontsize, int angle, bool recache, bool bold) {
			string attachmentFileName = "gen/" + FileSystem.CleanFileName(text) + ".png";

			if (!recache && !FileSystem.FileExists(Web.Attachments + attachmentFileName)) {
				FileSystem.CreateFolder(Web.Server.MapPath(Web.Attachments) + "gen");
				var fontWeight = bold ? FontStyle.Bold : FontStyle.Regular;
				var font = new Font(fontname, fontsize, fontWeight);
				SizeF textSize = GetEvenTextImageSize(text, font);
				SizeF imageSize;
				if (angle == 0) {
					imageSize = textSize;
				} else {
					imageSize = GetRotatedTextImageSize(textSize, angle);
				}
				using (var canvas = new Bitmap((int)imageSize.Width, (int)imageSize.Height)) {
					using (var graphics = Graphics.FromImage(canvas)) {
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.SmoothingMode = SmoothingMode.AntiAlias;
						//graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;//blocky pixels
						graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; //	.AntiAliasSingleBitPerPixel

						SizeF textContainerSize = graphics.VisibleClipBounds.Size;
						graphics.TranslateTransform(textContainerSize.Width / 2, textContainerSize.Height / 2);
						graphics.RotateTransform(angle);
						graphics.DrawString(text, font, Brushes.Black, -(textSize.Width / 2), -(textSize.Height / 2));
					}

					canvas.Save(Web.Server.MapPath(Web.Attachments) + attachmentFileName, ImageFormat.Png);

					//var stream = new MemoryStream();
					//canvas.Save(stream, ImageFormat.Png);
					//stream.Seek(0, SeekOrigin.Begin);
					//return new FileStreamResult(stream, "image/png");
				}
			} else {
				//return Content("<img src=\""+Web.Attachments+attachmentFileName+"\">");
				/*var stream = new FileStream(Server.MapPath(Web.Attachments) + attachmentFileName,FileMode.Open);
				stream.Seek(0, SeekOrigin.Begin);
				return new FileStreamResult(stream, "image/png");
				 */
				//return Redirect(Web.Attachments+attachmentFileName);
			}
			Web.DisplayImage(Web.Attachments + attachmentFileName, DateTime.Now.AddYears(1));
		}

		public static int CountImageColors(Bitmap bmp) {
			int count = 0;
			HashSet<Color> colors = new HashSet<Color>();

			try {
				if (bmp != null) {
					for (int y = 0; y < bmp.Size.Height; y++) {
						for (int x = 0; x < bmp.Size.Width; x++) {
							colors.Add(bmp.GetPixel(x, y));
						}
					}
					count = colors.Count;
				}
			} catch {
				throw;
			} finally {
				colors.Clear();
			}

			return count;
		}
	}

	/// <summary>
	/// This class is for internal use only by ImageProcessing and the picture upload control. It should not generally be used.
	/// Use ImageProcessing.ResizeImageUsingMetaData() instead.
	/// </summary>
	public class ImageUploader : IDisposable {
		public string fileContentType { get; set; }
		public string ErrorMessage { get; set; }
		public string ImageName { get; private set; }
		public string FileName { get; set; }
		private string fileType { get; set; }
		public Stream fileData { get; set; }

		public PictureMetaDataAttribute MetaData { get; set; }

		public void SetupImageFile() {
			// check the file is a jpg, gif or png
			fileType = fileContentType;
			string fileExtension = "";
			switch (fileType) {
				case "image/pjpeg": // we don't have an encoder match for image/pjpeg
				case "image/jpeg":
				case "image/jpg":
				case "image/x-citrix-pjpeg":
				case "image/x-citrix-jpeg":
					fileExtension = "jpg";
					fileType = "image/jpeg";
					break;
				case "image/x-citrix-gif":
				case "image/gif":
					fileExtension = "gif";
					fileType = "image/gif";
					break;
				case "image/bmp":
					fileExtension = "bmp";
					fileType = "image/bmp";
					break;
				case "image/x-citrix-png":
				case "image/x-png": // we don't have an encoder match for image/x-png
				case "image/png":
					fileExtension = "png";
					fileType = "image/png";
					break;
			}

			//string defaultAllowedMimeTypes = "image/jpeg,image/gif,image/png";
			// allow only default types if AllowedMimeTypes is blank

			//if (!AllowedMimeTypes.Contains(fileType) || (String.IsNullOrEmpty(AllowedMimeTypes) && !defaultAllowedMimeTypes.Contains(fileType)))

			if (MetaData != null && MetaData.AllowedMimeTypes != null && !MetaData.AllowedMimeTypes.Contains(fileType)) {
				// error in filetype
				string allowedFiles = (MetaData.AllowedMimeTypesForErrorMessage == "") ? "authorised" : MetaData.AllowedMimeTypesForErrorMessage;
				ErrorMessage =
					String.Format(
						"<span class='svyError'>The file type '{0}' you just uploaded is not supported, please upload only {1} files.</span>"
						, fileContentType
						, allowedFiles);
			}			// MN: removed else - still set ImageName even if this check fails, just try it and it if dies, throw exception (which it already does)

			// file type is OK
			if (!String.IsNullOrEmpty(ImageName)) {
				if (MetaData != null && MetaData.IsDatabaseStore) {
					// TODO: delete the old image(s) out of the database
				} else {
					// delete existing files from file system
					FileSystem.DeletePictureAttachment(ImageName);
				}
			}

			//DoUpload = true;
			//NewFileName = Guid.NewGuid().ToString();
			if (MetaData != null && MetaData.UseGuidFileNaming) {
				ImageName = Guid.NewGuid() + "." + fileExtension;
				FileName = ImageName;
			} else {
				ImageName = FileName;
				// 20100614 MN - now assume filename is already correct
				//ImageName=FileSystem.GetUniqueFilename(Web.Attachments + Subfolder,ClientFilePath);
			}
			if (MetaData != null) {
			}

		}


		//public static void DeleteImageFiles(string ImageName) {
		//  // delete from file system
		//  if (File.Exists(Web.MapPath(Web.Attachments + ImageName)))
		//    File.Delete(Web.MapPath(Web.Attachments + ImageName));
		//  if (File.Exists(Web.MapPath(Web.Attachments + ImageProcessing.InsertSuffix(ImageName, "_pv"))))
		//    File.Delete(Web.MapPath(Web.Attachments + ImageProcessing.InsertSuffix(ImageName, "_pv")));
		//  if (File.Exists(Web.MapPath(Web.Attachments + ImageProcessing.InsertSuffix(ImageName, "_tn"))))
		//    File.Delete(Web.MapPath(Web.Attachments + ImageProcessing.InsertSuffix(ImageName, "_tn")));

		//}

		/// <summary>
		///
		/// </summary>
		public bool SaveImageFile() {
			// call this yourself if using database store for images and you have this control on an admin page (required for WebForms FormView-based admin pages only)
			var returnVal = false;

			System.Drawing.Image originalImage = null;
			try {
				try {
					originalImage = Bitmap.FromStream(fileData);
				} catch (System.ArgumentException e) {
					string getFileName = (String.IsNullOrEmpty(FileName)) ? ImageName : FileName;
					throw new UserErrorException("Sorry, we could not process that image. The format was not recognised. Please re-save as a standard JPG or GIF file, and try again. The file name is " + getFileName + " (file type is " + fileType + "). Error Code: 45BWB7. " + e.Message); //Failed to save this type of image ["+fileName+"] fileType["+fileType+"]");
				}

				//var image = new Bitmap("C:/Data/Projects/CodelibMVC/images/testPen.jpg");

				returnVal = CreateSpecifiedSizes(fileData, fileType, FileName, MetaData, ImageName, originalImage);

			} finally {
				if (originalImage != null) {
					originalImage.Dispose();
				}
			}

			return returnVal;
		}

		private static bool CreateSpecifiedSizes(Stream stream, string fileType, string fileName, PictureMetaDataAttribute metaData, string imageName, Image originalImage) {
			string filePath;
			var attachments = Web.Attachments;
			if (Path.IsPathRooted(imageName)) {
				attachments = "";  // already full path
			}
			filePath = attachments + imageName;

			if (Util.GetSettingBool("ForcePNG", false) && fileType == "image/jpeg") { //bad idea
				filePath = filePath.LeftUntilLast(".") + ".png";
				imageName = imageName.LeftUntilLast(".") + ".png";
			}

			string previewFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_pv");
			string thumbnailFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_tn");
			string mediumFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_med");
			string smallFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_sml");
			string bigFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_big");
			string zoomFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_zm");
			string panelDisplayFilePath = attachments + ImageProcessing.InsertSuffix(imageName, "_pd");
			if (metaData != null && metaData.IsDatabaseStore) {
				// saving to the database - just save the original name (we could change this to a GUID if we wanted)
				filePath = fileName;
				previewFilePath = filePath;
				thumbnailFilePath = filePath;
				mediumFilePath = filePath;
				smallFilePath = filePath;
				bigFilePath = filePath;
				zoomFilePath = filePath;
				panelDisplayFilePath = filePath;
			}

			// ** MAIN IMAGE **
			// if this original image is within our dimensions then just keep it! (This fixes resize transparency issues)
			string ImageId;
			if (metaData == null) {
				throw new Exception("metadata is null - possibly missing from the app_code/models code. picture added after first generation? you should manually add the picture metadata, or delete the model partial file and regenerate the model (tip: back it up and use BC if you are unsure about overwriting the partial)");
			}
			if (originalImage == null) return false;

			bool keepOriginalSize = false;
			if (metaData.DontResizeOriginal) {
				keepOriginalSize = true;
			} else if (metaData.IsExact || metaData.IsCropped) {
				if (originalImage.Width == metaData.Width && originalImage.Height == metaData.Height) {
					keepOriginalSize = true;
				}
			} else {
				if (originalImage.Width <= metaData.Width && originalImage.Height <= metaData.Height) {
					keepOriginalSize = true;
				}
			}

			// Read EXIF information and rotates if needed
			using (var bmpInput = (Image)originalImage.Clone()) {
				using (var bmpOutput = new Bitmap(bmpInput)) {
					foreach (var id in bmpInput.PropertyIdList) {
						bmpOutput.SetPropertyItem(bmpInput.GetPropertyItem(id));
					}
					var bmp = (Bitmap)bmpOutput.Clone();
					var exif = new EXIFextractor(ref bmp, "", "");
					if (exif["Orientation"] != null) {
						RotateFlipType flip = OrientationToFlipType(exif["Orientation"].ToString());
						if (flip != RotateFlipType.RotateNoneFlipNone) { // Don't flip if orientation is correct
							originalImage.RotateFlip(flip);
						}
					}
					bmp.Dispose();
				}
			}

			if (keepOriginalSize) {
				// keep it as it is - no resize operations!
				if (metaData.IsDatabaseStore) {
					ImageId = ImageProcessing.SaveImageToDatabase(originalImage, filePath, fileType);
				} else {
					// MN 20101221 - fixed this case
					if (stream is FileStream && FileSystem.FileExists(filePath)) {
						// file already exists and we dont want to overwrite file because this might be the stream we have opened
						// do nothing
					} else {
						// either file not existing or it is ok to overwrite - this is always the case if file was uploaded via HTTP
						stream.SaveStreamToFile(filePath);
					}
				}
			} else {
				// resize and save the main image
				if (metaData.IsExact || metaData.IsCropped) // cropped means exact
				{
					// exact dimensions
					ImageId = ImageProcessing.ResizeImageTo(originalImage, metaData.Width, metaData.Height, metaData.IsCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, filePath, metaData.IsExact);
				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					ImageId = ImageProcessing.ResizeImageWithin(originalImage, metaData.Width, metaData.Height, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, filePath);
				}
			}

			// ** PREVIEW IMAGE **
			// preview is always exact dimensions - it just may or may not be cropped
			//string PreviewId = ImageProcessing.ResizeImageTo(originalImage, MetaData.PreviewWidth, MetaData.PreviewHeight, MetaData.IsPreviewCropped, fileType, metaData.BackgroundColorTyped, MetaData.IsDatabaseStore, previewFilePath);

			if (metaData.PreviewWidth != 0 && metaData.PreviewHeight != 0) {
				string PreviewId;
				if (metaData.IsPreviewCropped || metaData.IsPreviewExact) // cropped means exact
				{
					// exact dimensions
					PreviewId = ImageProcessing.ResizeImageTo(originalImage, metaData.PreviewWidth, metaData.PreviewHeight, metaData.IsPreviewCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, previewFilePath, metaData.IsPreviewExact);

				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					PreviewId = ImageProcessing.ResizeImageWithin(originalImage, metaData.PreviewWidth, metaData.PreviewHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, previewFilePath);
				}
			}

			// ** THUMBNAIL IMAGE **
			if (metaData.ThumbnailWidth != 0 && metaData.ThumbnailHeight != 0) {
				string ThumbnailId;
				if (metaData.IsThumbnailCropped || metaData.IsThumbnailExact) // cropped means exact
				{
					// exact dimensions
					ThumbnailId = ImageProcessing.ResizeImageTo(originalImage, metaData.ThumbnailWidth, metaData.ThumbnailHeight, metaData.IsThumbnailCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, thumbnailFilePath,metaData.IsThumbnailExact);
				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					ThumbnailId = ImageProcessing.ResizeImageWithin(originalImage, metaData.ThumbnailWidth, metaData.ThumbnailHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, thumbnailFilePath);
				}
			}


			// ** MEDIUM IMAGE **
			if (metaData.MediumWidth != 0 && metaData.MediumHeight != 0) {
				string MediumId;
				if (metaData.IsMediumCropped || metaData.IsMediumExact) // cropped means exact
				{
					// exact dimensions
					MediumId = ImageProcessing.ResizeImageTo(originalImage, metaData.MediumWidth, metaData.MediumHeight, metaData.IsMediumCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, mediumFilePath,metaData.IsMediumExact);
				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					MediumId = ImageProcessing.ResizeImageWithin(originalImage, metaData.MediumWidth, metaData.MediumHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, mediumFilePath);
				}
			}

			// ** SMALL IMAGE **
			if (metaData.SmallWidth != 0 && metaData.SmallHeight != 0) {
				string SmallId;
				if (metaData.IsSmallCropped || metaData.IsSmallExact) // cropped means exact
				{
					// exact dimensions
					SmallId = ImageProcessing.ResizeImageTo(originalImage, metaData.SmallWidth, metaData.SmallHeight, metaData.IsSmallCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, smallFilePath,metaData.IsSmallExact);
				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					SmallId = ImageProcessing.ResizeImageWithin(originalImage, metaData.SmallWidth, metaData.SmallHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, smallFilePath);
				}
			}

			// ** BIG IMAGE **

			if (metaData.BigWidth != 0 && metaData.BigHeight != 0) {
				string BigId;
				if (metaData.IsBigCropped || metaData.IsBigExact) // cropped means exact
				{
					// exact dimensions
					BigId = ImageProcessing.ResizeImageTo(originalImage, metaData.BigWidth, metaData.BigHeight, metaData.IsBigCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, bigFilePath, metaData.IsBigExact);

				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					BigId = ImageProcessing.ResizeImageWithin(originalImage, metaData.BigWidth, metaData.BigHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, bigFilePath);
				}
			}


			// ** ZOOM IMAGE aka Extra Large**

			if (metaData.ZoomWidth != 0 && metaData.ZoomHeight != 0) {
				string ZoomId;
				if (metaData.IsZoomCropped || metaData.IsZoomExact) // cropped means exact
				{
					// exact dimensions
					ZoomId = ImageProcessing.ResizeImageTo(originalImage, metaData.ZoomWidth, metaData.ZoomHeight, metaData.IsZoomCropped, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, zoomFilePath);

				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					ZoomId = ImageProcessing.ResizeImageWithin(originalImage, metaData.ZoomWidth, metaData.ZoomHeight, fileType, metaData.BackgroundColorTyped, metaData.IsDatabaseStore, zoomFilePath);
				}
			}


			// ** Panel Display - For use on carousel panels
			if (metaData.PanelDisplayWidth != 0 && metaData.PanelDisplayHeight != 0) {
				string PanelId;
				if (metaData.IsPanelDisplayCropped || metaData.IsPanelDisplayExact) // cropped means exact
				{
					// exact dimensions
					PanelId = ImageProcessing.ResizeImageTo(originalImage, metaData.PanelDisplayWidth, metaData.PanelDisplayHeight, metaData.IsPanelDisplayCropped, fileType, ColorTranslator.FromHtml(metaData.BackgroundColor), metaData.IsDatabaseStore, panelDisplayFilePath, metaData.IsPanelDisplayExact);

				} else {
					// image will fit inside bounding box specified, but will not necessarily have those exact dimensions
					PanelId = ImageProcessing.ResizeImageWithin(originalImage, metaData.PanelDisplayWidth, metaData.PanelDisplayHeight, fileType, metaData.IsDatabaseStore, panelDisplayFilePath);
				}
			}

			return true;
		}

		// Match the orientation code to the correct rotation:
		private static RotateFlipType OrientationToFlipType(string orientation) {
			switch (int.Parse(orientation)) {
				case 1: return RotateFlipType.RotateNoneFlipNone;
				case 2: return RotateFlipType.RotateNoneFlipX;
				case 3: return RotateFlipType.Rotate180FlipNone;
				case 4: return RotateFlipType.Rotate180FlipX;
				case 5: return RotateFlipType.Rotate90FlipX;
				case 6: return RotateFlipType.Rotate90FlipNone;
				case 7: return RotateFlipType.Rotate270FlipX;
				case 8: return RotateFlipType.Rotate270FlipNone;
				default: return RotateFlipType.RotateNoneFlipNone;
			}
		}

		private void SaveStreamToDisk() {
			//UploadField.SaveAs(Web.MapPath(Web.Attachments + ImageName));
			fileData.SaveStreamToFile(Web.MapPath(Web.Attachments) + ImageName);
		}

		public void Dispose() {
			if (fileData != null) fileData.Dispose();
		}
	}

	public class PictureMetaDataAttribute : Attribute {
		public PictureMetaDataAttribute() {
			BackgroundColor = "#ffffff";  // default white for backwards compatibility
			Width = 300;
			Height = 1000;
			IsExact = false;
			IsCropped = false;
			DontResizeOriginal = false;
			IsDatabaseStore = false;
			AllowedMimeTypes = "image/jpeg,image/gif,image/png,image/bmp";
			AllowedMimeTypesForErrorMessage = "";

			IsZoomCropped = true;
			IsZoomExact = true;
			ZoomWidth = 0;
			ZoomHeight = 0;

			IsPanelDisplayCropped = true;
			IsPanelDisplayExact = true;
			PanelDisplayWidth = 0;
			PanelDisplayHeight = 0;

			IsBigCropped = true;
			IsBigExact = true;
			BigWidth = 0;
			BigHeight = 0;

			IsMediumCropped = true;
			IsMediumExact = true;
			MediumWidth = 0;
			MediumHeight = 0;

			IsSmallCropped = true;
			IsSmallExact = true;
			SmallWidth = 0;
			SmallHeight = 0;

			IsThumbnailCropped = true;
			IsThumbnailExact = true;
			ThumbnailWidth = 0;
			ThumbnailHeight = 0;

			IsPreviewCropped = false;
			IsPreviewExact = false;
			PreviewWidth = 30;
			PreviewHeight = 30;


			ShowDimensionMessage = true;
			ShowPreviewImage = true;
			//ShowThumbnailCropOption	= false; -- not used
			UseGuidFileNaming = false;
			AllowSelectFromServer = true;
			//BasePath = Web.Attachments;
			UseSubfolder = true;
			Subfolder = "";
			ShowFreeImageSearchLinks = false;
			ShowCropResizeChoice = false;
			AllowPasteAndDrag = false;
			ShowRemoveCheckbox = true;
			ShowPreviewImageEnlargement = false;
		}
		public int Width { get; set; }
		public int Height { get; set; }

		/// <summary>
		/// If true, a white (or other backgroundcolor) canvas it created at the size specified and the image is then either cropped or scaled down and placed in the middle. In other words you can rely on the IMG width and height being exactly what you say, the resulting image file will never be smaller. This is generally what you want.
		/// </summary>
		public bool IsExact { get; set; }

		/// <summary>
		/// If true, it will scale down until either the width or height fits and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. If aspect ratio is different, the image may be filled with the background colour to make it the exact size.
		/// </summary>
		public bool IsCropped { get; set; }

		/// <summary>
		/// If true, the original image will be saved exactly as is and will not be resized. In this case Width, Height, IsCropped and IsExact are not used. Default value is false. 
		/// This is useful for example if creating an image library, or when uploading transparent images, or when a graphic designer will be trusted to hand-optimise and resize images.
		/// </summary>
		public bool DontResizeOriginal { get; set; }

		public string AllowedMimeTypes { get; set; }
		public string AllowedMimeTypesForErrorMessage { get; set; }
		public bool IsDatabaseStore { get; set; }

		public int ThumbnailWidth { get; set; }
		public int ThumbnailHeight { get; set; }

		public bool ForceAjax { get; set; }

		/// <summary>
		/// If true, a white (or other backgroundcolor) canvas it created at the size specified and the image is then either cropped or scaled down to the thumbnail size and placed in the middle. In other words you can rely on the IMG width and height being exactly what you say, the resulting image file will never be smaller. This is generally what you want.
		/// </summary>
		public bool IsThumbnailExact { get; set; }

		/// <summary>
		/// If true, it will scale down until either the width or height fits the thumbnail size and then crop the excess (off the sides or top and bottom). If false, it will scale down until BOTH the width or height fit. If aspect ratio if different, the image may be filled with the background colour to make it the exact size.
		/// </summary>
		public bool IsThumbnailCropped { get; set; }

		public int ZoomWidth { get; set; }
		public int ZoomHeight { get; set; }
		public bool IsZoomCropped { get; set; }
		public bool IsZoomExact { get; set; }

		public int PanelDisplayWidth { get; set; }
		public int PanelDisplayHeight { get; set; }
		public bool IsPanelDisplayCropped { get; set; }
		public bool IsPanelDisplayExact { get; set; }

		public int BigWidth { get; set; }
		public int BigHeight { get; set; }
		public bool IsBigCropped { get; set; }
		public bool IsBigExact { get; set; }

		public int PreviewWidth { get; set; }
		public int PreviewHeight { get; set; }
		public bool IsPreviewCropped { get; set; }
		public bool IsPreviewExact { get; set; }

		public int MediumWidth { get; set; }
		public int MediumHeight { get; set; }
		public bool IsMediumExact { get; set; }
		public bool IsMediumCropped { get; set; }

		public int SmallWidth { get; set; }
		public int SmallHeight { get; set; }
		public bool IsSmallExact { get; set; }
		public bool IsSmallCropped { get; set; }

		/// <summary>
		/// Fill colour for when image is not the right aspect ratio, or smaller than the target size. RGB string, defaults to "#FFFFFF". Set BackgroundColorTransparent=true to ignore the colour and make it transparent.
		/// </summary>
		public string BackgroundColor { get; set; }

		private Color? _backgroundColorTyped = null;

		/// <summary>
		/// Set this if you wish to specify the background colour using a System.Drawing.Color value instead of HTML hex colour (using BackgroundColor). For example you can use this to set BackgroundColorTyped=System.Drawing.Color.Transparent to preserve PNG transparency when resizing PNGs. This overrides BackgroundColor.
		/// </summary>
		public Color BackgroundColorTyped {
			get {
				if (_backgroundColorTyped != null) {
					return _backgroundColorTyped.Value;
				}
				return ColorTranslator.FromHtml(BackgroundColor);
			}
			set {
				_backgroundColorTyped = value;
			}
		}

		/// <summary>
		/// Will show the width and height on the image upload/selector control.
		/// </summary>
		public bool ShowDimensionMessage { get; set; }

		private int _dimensionMessageWidth = 0;
		public int DimensionMessageWidth {
			get {
				if (_dimensionMessageWidth == 0) {
					_dimensionMessageWidth = Width;
				}
				return _dimensionMessageWidth;
			}
			set {
				_dimensionMessageWidth = value;
			}
		}

		private int _dimensionMessageHeight = 0;
		public int DimensionMessageHeight {
			get {
				if (_dimensionMessageHeight == 0) {
					_dimensionMessageHeight = Height;
				}
				return _dimensionMessageHeight;
			}
			set {
				_dimensionMessageHeight = value;
			}
		}


		public bool ShowPreviewImage { get; set; }
		//public bool ShowThumbnailCropOption { get; set; } -- not used
		public bool UseGuidFileNaming { get; set; }

		/// <summary>
		/// On the image upload/selector control, this will give the user the ability to choose from any images of the same size that they have already uploaded to the server.
		/// </summary>
		public bool AllowSelectFromServer { get; set; }

		/// <summary>
		/// On the image upload/selector control, shows links to Flickr and Creative Commons free image searches.
		/// </summary>
		public bool ShowFreeImageSearchLinks { get; set; }

		/// <summary>
		/// Will give the user a choice of cropping or scaling when uploading an image with the image upload/selector control. Cropping is good for photos while scaling is good for logos or diagrams.
		/// </summary>
		public bool ShowCropResizeChoice { get; set; }

		/// <summary>
		/// Allow user to crop in the preview popup
		/// </summary>
		public bool ShowCropWindow { get; set; }

		/// <summary>
		/// Shows a Paste and Drag Drop area to allow the user to paste or drag the image directly from the clipboard, if their browser supports it.
		/// </summary>
		public bool AllowPasteAndDrag { get; set; }
		public bool ShowDragButton { get; set; }
		public bool ShowPasteButton { get; set; }

		/// <summary>
		/// Same as AllowPasteAndDrag. Fixed unnecessary breaking change.
		/// </summary>
		public bool AllowPaste { get { return AllowPasteAndDrag; } set { AllowPasteAndDrag = value; } }


		/// <summary>
		/// True to save images in a subfolder underneath ~/attachments/ (which is the default). 
		/// By default, the subfolder name is "[width]x[height]". Or you can set your own subfolder with the "Subfolder" metadata property.
		/// </summary>
		public bool UseSubfolder { get; set; }

		private string _subfolder;

		/// <summary>
		/// True to show the 'Remove Picture' checkbox. Useful if the requirement is to anyways have a picture or the picture fields is in a subform;
		/// MN: badly named - was called ShowChangePictureLink, changed to ShowRemoveCheckbox
		public bool ShowRemoveCheckbox { get; set; }

		public string Subfolder {
			get {
				if (!UseSubfolder) {
					return "";
				} else if (_subfolder.IsBlank()) {
					return "svy" + Width + "x" + Height + "t" + ThumbnailWidth + "x" + ThumbnailHeight + "/";
				} else {
					return _subfolder.TrimEnd("/\\") + "/";
				}
			}
			set {
				_subfolder = value;
			}
		}

		/// <summary>
		/// Shows a popup enlargement off the preview image
		/// </summary>
		public bool ShowPreviewImageEnlargement { get; set; }

		public bool ShowFullDimensionMessage { get; set; }

		//public string BasePath	 { get; set; }  -- not implemented
	}

	public class TestImageProcessing {
		[TestMethod]
		public void TestSizing() {
			var start = DateTime.Now;
			//for (int i = 0; i < 100; i++) {
			//	var size = ImageProcessing.GetImageDimensionsSlow("~/images/maintenance.jpg");
			//	if (i==0) Web.Write(" "+size.Width + "x" + size.Height+" ");
			//}
			//Web.Write(start.FmtMillisecondsElapsed());
			start = DateTime.Now;
			for (int i = 0; i < 100; i++) {
				var size = ImageProcessing.GetImageDimensions("~/images/maintenance.jpg");
				if (i == 0) Web.Write(" " + size.Width + "x" + size.Height + " ");
			}
			Web.Write(start.FmtMillisecondsElapsed());
			Assert.Pass();
		}
	}
}