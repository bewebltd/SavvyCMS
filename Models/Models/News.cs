using System;
using Beweb;

namespace Models {
	public partial class News {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new News object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			PublishDate = DateTime.Now;

			Fields.Picture.MetaData = new PictureMetaDataAttribute();
			Fields.Picture.MetaData.IsExact = false;
			Fields.Picture.MetaData.IsCropped = false;
			Fields.Picture.MetaData.Width = 300;
			Fields.Picture.MetaData.Height = 300;
			Fields.Picture.MetaData.ThumbnailWidth = 100;
			Fields.Picture.MetaData.ThumbnailHeight = 100;
			Fields.Picture.MetaData.ShowPreviewImageEnlargement = true;
			Fields.Picture.MetaData.ShowDimensionMessage = true;
			Fields.Picture.MetaData.AllowPasteAndDrag = true;
			Fields.Picture.MetaData.ShowCropWindow = true;
			Fields.Picture.MetaData.ShowCropResizeChoice = true;
			Fields.Picture.MetaData.AllowSelectFromServer = false;
			Fields.Picture.MetaData.ForceAjax = true;

			Fields.LargePicture.MetaData = new PictureMetaDataAttribute();
			Fields.LargePicture.MetaData.IsExact = false;
			Fields.LargePicture.MetaData.IsCropped = false;
			Fields.LargePicture.MetaData.Width = 300;
			Fields.LargePicture.MetaData.Height = 300;
			Fields.LargePicture.MetaData.ShowRemoveCheckbox = true;
			Fields.LargePicture.MetaData.ThumbnailWidth = 100;
			Fields.LargePicture.MetaData.ThumbnailHeight = 100;
			Fields.LargePicture.MetaData.ShowCropWindow = true;
			Fields.Attachment.MetaData = new AttachmentMetaDataAttribute();
			Fields.Attachment.MetaData.AllowPasteAndDrag = true;

			//Fields.Attachment.MetaData.AllowedFileTypes = "doc,pdf";
			DateAdded = DateTime.Now;
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert
		//protected override void OnAfterLoadData()		{		}

		// You can put any business logic associated with this entity here

		public static string SearchableFieldNames {
			get { return new Page().GetTextFieldNames().Join(","); }
		}
	}
}

// created: [ 17-Aug-2011 9:52:52pm ]