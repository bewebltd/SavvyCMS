using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class GalleryCategory {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new GalleryCategory object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			PublishDate = DateTime.Today;

			DateAdded = DateTime.Now;


		}
		/* no image on gallery category - there is a setting in gallery image to select the cover image
		public static PictureMetaDataAttribute GetPicMetaData() {
			return new DefaultPictureMetaData(); //{Width = 123,Height = 456};
		}
		*/
		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here

		private string coverImage;
		public string CoverImage {
			get {
				if (coverImage == null) {
					GalleryImage image = GalleryImages.Find(i => i.IsCoverImage);
					if (image != null) {
						// yessssssssssssss cover image selected
						coverImage = ImageProcessing.ImageSmallPath(image.Picture);
					} else {
						// no images selected for cover so just find the first iamge
						image = GalleryImages.Find(i => i.MediaType == GalleryImage.GALLERYIMAGEMEDIATYPEPHOTO);
						if (image != null) {
							coverImage = ImageProcessing.ImageSmallPath(image.Picture);
						} else {
							// may not have an image at all such as a video only category (hopefully unlikely to happen)
							coverImage = "";
						}
					}
				}
				return coverImage;
			}
		}

		public override string GetDefaultOrderBy() {
			return base.GetDefaultOrderBy();
		}

		public override string GetUrl() {
			return base.GetUrl();
		}

		public override void Save() {
			//AutocompletePhrase.AddPhrase(this, Fields.Title);
			base.Save();
		}

		public override void Delete() {
			//AutocompletePhrase.DeletePhrase(this);
			base.Delete();
		}
	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestGalleryCategory {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 28 May 2014 10:03:05am ]