using System;
using Beweb;

namespace Models {
	public partial class Banner {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Banner object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			IsPublished = true;
			StartDate = DateTime.Today;

			Fields.Picture.MetaData = new PictureMetaDataAttribute();
			Fields.Picture.MetaData.IsExact = false;
			Fields.Picture.MetaData.IsCropped = false;
			Fields.Picture.MetaData.Width = 297;
			Fields.Picture.MetaData.Height = 322;
			//Fields.Picture.MetaData.Width = 297;
			//Fields.Picture.MetaData.Height = 100;
			//Fields.Picture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			//Fields.Picture.MetaData.ThumbnailHeight = 100;

		}
		//protected override void OnAfterLoadData()		{		}

		// You can put any business logic associated with this entity here

		public string Width {
			get { return Size.Split('x')[0]; }
		}

		public string Height {
			get { return Size.Split('x')[1]; }
		}

	}
}

// created: [ 12-Jan-2011 12:18:32pm ]