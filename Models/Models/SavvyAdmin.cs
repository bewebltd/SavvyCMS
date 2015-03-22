using System;
using Beweb;

namespace Models {
	public partial class SavvyAdmin {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new SavvyAdmin object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						Fields.ClientLogoPicture.MetaData = new PictureMetaDataAttribute();
			Fields.ClientLogoPicture.MetaData.IsExact = false;
			Fields.ClientLogoPicture.MetaData.IsCropped = false;
			Fields.ClientLogoPicture.MetaData.Width = 600;
			Fields.ClientLogoPicture.MetaData.Height = 100;
			Fields.ClientLogoPicture.MetaData.AllowSelectFromServer = false;
			Fields.ClientLogoPicture.MetaData.ShowDimensionMessage = false;

			HeaderColor = "#551901";
			ShowSavvyLogo = true;

			//Fields.Picture.MetaData = new PictureMetaDataAttribute();
			//Fields.Picture.MetaData.IsExact = false;
			//Fields.Picture.MetaData.IsCropped = false;
			//Fields.Picture.MetaData.Width = 300;
			//Fields.Picture.MetaData.Height = 300;
			//Fields.Picture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			//Fields.Picture.MetaData.ThumbnailHeight = 100;

		}
		//protected override void OnAfterLoadData()		{		}

		// You can put any business logic associated with this entity here
		
	}
}

// created: [ 12-Apr-2011 4:37:54pm ]