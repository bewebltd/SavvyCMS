using System;
using Beweb;

namespace Models {
	public partial class Settings {
		private static Settings instance = null;

		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Settings object.
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
		
		public static Settings All {
			get {
				if (instance == null) {
					instance = Settings.Load(new Sql("select top 1 * from settings"));
					if (instance==null) {
						instance = new Settings();
						instance.Save();
					}
				}
				return instance;
			}
		}
		public static void RebuildCache() {
			instance = null;
		}
	}
}

// created: [ 13-Apr-2011 3:26:31pm ]