using System;
using Beweb;

namespace Models {
	public partial class ShoppingCart {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new ShoppingCart object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
			IsDeleted=false;



			//Fields.Picture.MetaData = new PictureMetaDataAttribute();
			//Fields.Picture.MetaData.IsExact = false;
			//Fields.Picture.MetaData.IsCropped = false;
			//Fields.Picture.MetaData.Width = 300;
			//Fields.Picture.MetaData.Height = 300;
			//Fields.Picture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			//Fields.Picture.MetaData.ThumbnailHeight = 100;

		}
		//protected override void OnAfterLoadData()		{		}

		/// <summary>
		/// generate a unique reference number for this cart using readble characters
		/// </summary>
		/// <returns></returns>
		public static string GenerateRef()
		{
			string result = "";
			for (int sc = 0; ; sc++)
			{
				if (sc > 20) throw new Exception("ref gen failed");
				result = RandomPassword.Generate(6, 6, "Q", "QWERTYPASDFGHJKLZXBNM", "23456", "2") + "-" + RandomPassword.Generate(3, 3, "W", "WXYZ", "23456", "2");
				if (ShoppingCartOrderList.LoadByOrderRef(result).RecordCount == 0)
				{
					break;
				}
			}

			return result;
		}																																														
		
	}
}

// created: [ 01-Dec-2010 11:35:28pm ]