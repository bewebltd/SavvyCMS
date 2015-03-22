using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class GalleryImage {

		public const string GALLERYIMAGEMEDIATYPEPHOTO = "Photo";
		public const string GALLERYIMAGEMEDIATYPEYOUTUBE = "YouTube";

		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new GalleryImage object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data

			Fields.Picture.MetaData = GetPicMetaData();
			MediaType = GALLERYIMAGEMEDIATYPEPHOTO;
			SortPosition = 50;

			DateAdded = DateTime.Now;

			PublishDate = DateTime.Today;


		}

		public static PictureMetaDataAttribute GetPicMetaData() {
			var isCropped = true;
			var isExact = true;
			return new DefaultPictureMetaData {
				// image 
				Width = 1000,
				Height = 1000,
				IsCropped = false,
				IsExact = false,
				DontResizeOriginal = true,
				BigWidth = 800,
				BigHeight = 600,
				IsBigCropped = isCropped,
				IsBigExact = isExact,

				MediumWidth = 300,
				MediumHeight = 300,
				IsMediumCropped = isCropped,
				IsMediumExact = isExact,

				// not medium at this stage

				// gallery page thumbnail / cover image
				SmallWidth = 200,
				SmallHeight = 150,
				IsSmallCropped = isCropped,
				IsSmallExact = isExact,

				// admin thumbnail
				ThumbnailWidth = 40,
				ThumbnailHeight = 30,
				IsThumbnailCropped = isCropped,
				IsThumbnailExact = isExact
				

			};

		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
		public override string GetDefaultOrderBy() {
			//return base.GetDefaultOrderBy();
			return " ORDER BY GalleryCategoryID, Title";
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
//	public class TestGalleryImage {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 28 May 2014 9:22:30am ]