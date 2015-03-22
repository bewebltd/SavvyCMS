using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class GenTest {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new GenTest object.
		/// </summary>
		public override  void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						IsActive = true;

			DateAdded = DateTime.Now;

			PublishDate = DateTime.Today;

			Fields.Picture.MetaData = new DefaultPictureMetaData();
			Fields.Picture.MetaData.IsExact = false;
			Fields.Picture.MetaData.IsCropped = false;
			Fields.Picture.MetaData.Width = 300;
			Fields.Picture.MetaData.Height = 300;
			Fields.Picture.MetaData.ThumbnailWidth = 100;
			Fields.Picture.MetaData.ThumbnailHeight = 100;
			Fields.Picture.MetaData.ShowCropWindow = true;

			Fields.Picture1.MetaData = new PictureMetaDataAttribute();
			Fields.Picture1.MetaData.IsExact = false;
			Fields.Picture1.MetaData.IsCropped = false;
			Fields.Picture1.MetaData.Width = 300;
			Fields.Picture1.MetaData.Height = 300;
			Fields.Picture1.MetaData.ThumbnailWidth = 100;
			Fields.Picture1.MetaData.ThumbnailHeight = 100;


		}
		
		// You can put any business logic associated with this entity here
		
	}
}

// created: [ 18-May-2010 9:08:24am ]