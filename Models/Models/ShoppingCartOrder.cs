using System;
using Beweb;

namespace Models {
	public partial class ShoppingCartOrder {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new ShoppingCartOrder object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			


			//Fields.Picture.MetaData = new PictureMetaDataAttribute();
			//Fields.Picture.MetaData.IsExact = false;
			//Fields.Picture.MetaData.IsCropped = false;
			//Fields.Picture.MetaData.Width = 300;
			//Fields.Picture.MetaData.Height = 300;
			//Fields.Picture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			//Fields.Picture.MetaData.ThumbnailHeight = 100;

		}
		//protected override void OnAfterLoadData()		{		}

		public override string GetDefaultOrderBy()
		{
			return "order by DateOrdered desc";
		}
	}
}

// created: [ 01-Dec-2010 11:35:28pm ]