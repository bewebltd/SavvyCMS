using System;
using Beweb;

namespace Models {
	public partial class ContactUs {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new ContactUs object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						DateAdded = DateTime.Now;




			//Fields.Picture.MetaData = new PictureMetaDataAttribute();
			//Fields.Picture.MetaData.IsExact = false;
			//Fields.Picture.MetaData.IsCropped = false;
			//Fields.Picture.MetaData.Width = 300;
			//Fields.Picture.MetaData.Height = 300;
			//Fields.Picture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			//Fields.Picture.MetaData.ThumbnailHeight = 100;

		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert
		//protected override void OnAfterLoadData()		{		}

		// You can put any business logic associated with this entity here

		public override string GetDefaultOrderBy() {
			return "ORDER BY DateAdded DESC";
		}
	}
}

// created: [ 20-Jul-2011 1:56:51pm ]