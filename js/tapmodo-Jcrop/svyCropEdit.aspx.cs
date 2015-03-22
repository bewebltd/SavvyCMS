using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Web;
using Beweb;
using Site.SiteCustom;
using SD = System.Drawing;

public partial class svyCropEdit : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		var gocrop = Web.Request["gocrop"];
		if (gocrop != null) {
			goCrop( Web.Request["filename"]
				, Web.Request["w"].ToIntOrDie()
				, Web.Request["h"].ToIntOrDie()
				, Web.Request["x"].ToIntOrDie()
				, Web.Request["y"].ToIntOrDie()
				);
		}
	}


	protected void goCrop(string ImageName, int Width, int Height, int X, int Y) {
		 
		int w = Convert.ToInt32(Width);
		int h = Convert.ToInt32(Height);
		int x = Convert.ToInt32(X);
		int y = Convert.ToInt32(Y);

		byte[] CropImage = Crop(Web.MapPath( Web.Attachments )+ImageName, w, h, x, y);
		using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length)) {
			ms.Write(CropImage, 0, CropImage.Length);
			using (SD.Image CroppedImage = SD.Image.FromStream(ms, true)) {
				string SaveTo = Web.MapPath( Web.Attachments )+ "crop" + ImageName;
				CroppedImage.Save(SaveTo, CroppedImage.RawFormat);
				var newUrl = "images/crop" + ImageName;
				Web.Write(newUrl+"<br>");
				Web.Write("<img src=\"" + newUrl + "\">");
			}
		}
	}
	static byte[] Crop(string Img, int Width, int Height, int X, int Y) {
		try {
			using (SD.Image OriginalImage = SD.Image.FromFile(Img)) {
				using (SD.Bitmap bmp = new SD.Bitmap(Width, Height)) {
					bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
					using (SD.Graphics Graphic = SD.Graphics.FromImage(bmp)) {
						Graphic.SmoothingMode = SmoothingMode.AntiAlias;
						Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
						Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
						Graphic.DrawImage(OriginalImage, new SD.Rectangle(0, 0, Width, Height), X, Y, Width, Height, SD.GraphicsUnit.Pixel);
						MemoryStream ms = new MemoryStream();
						bmp.Save(ms, OriginalImage.RawFormat);
						return ms.GetBuffer();
					}
				}
			}
		} catch (Exception Ex) {
			throw (Ex);
		}
	}
}
