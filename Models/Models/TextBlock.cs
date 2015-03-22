using System;
using Beweb;

namespace Models {
	public partial class TextBlock {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new TextBlock object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			if (Web.Request["TextBlockGroupID"] + "" != "") {
				TextBlockGroupID = Web.Request["TextBlockGroupID"].ToInt();
			}
			IsBodyPlainText = true;
			IsTitleAvailable = false;
			IsPictureAvailable = false;
			IsUrlAvailable = false;
			HasMailMergefields = false;
			Fields.Picture.MetaData = new PictureMetaDataAttribute() {
				IsExact = false,
				IsCropped = false,
				Width = 300,
				Height = 300,

				IsThumbnailExact = false,
				IsThumbnailCropped = false,
				ThumbnailWidth = 100,
				ThumbnailHeight = 100,

				IsPreviewCropped = false,
				PreviewWidth = 100,
				PreviewHeight = 100
			};
		}

		// You can put any business logic associated with this entity here
		public override string GetDefaultOrderBy() {
			return "order by SectionCode";
		}
	}

}

// created: [ 18-May-2010 9:08:24am ]