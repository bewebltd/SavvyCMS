using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beweb;

namespace Site.SiteCustom {
	// this is the default size and config for all pictures in the site
	// this is a good place to set some of the odd settings like ShowFreeImageSearchLinks
	// it is often useful to have a number of images all sharing a standard size
	public class DefaultPictureMetaData:PictureMetaDataAttribute {
		public DefaultPictureMetaData() {
			DontResizeOriginal = true;
			IsExact = false;
			IsCropped = true;
			Width = 1000;
			Height = 1000;
			IsThumbnailExact = true;
			IsThumbnailCropped = true;
			ThumbnailWidth = 0;  // defualt to no thumbnail
			ThumbnailHeight = 0;
			IsPreviewCropped = true;
			PreviewWidth = 50;
			PreviewHeight = 50;
			ShowFreeImageSearchLinks = false;
			ShowCropResizeChoice = false;
			BackgroundColorTyped = System.Drawing.Color.Transparent;  // preserves PNG transparency
			ShowPreviewImageEnlargement = true;
			AllowSelectFromServer = true;
			AllowPasteAndDrag = true;
			ShowCropWindow = false; // mn 20141221 does a bad job -- need to fix this
		}

		// intialise the default meta data as default for all activefields (new idea, see MN 20141221)
		//private static initialiser _initialiser  = new initialiser();
		//private class initialiser {
		//	public initialiser() {
		//		PictureActiveField.DefaultMetaData = new DefaultPictureMetaData();
		//	}
		//}
	}
}