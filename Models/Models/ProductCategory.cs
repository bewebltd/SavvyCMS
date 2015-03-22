using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class ProductCategory {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new ProductCategory object.
		/// </summary>
		public void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
			IsActive = true;
			Fields.Picture.MetaData = GetMetaData();
		}

		public string Action;

		public static PictureMetaDataAttribute GetMetaData() {
			var metaData = new PictureMetaDataAttribute();
			metaData.IsExact = true;
			metaData.IsCropped = true;
			metaData.IsThumbnailExact = true;
			metaData.IsThumbnailCropped = true;
			metaData.Width = 180;
			metaData.Height = 180;
			metaData.ThumbnailWidth = 75;
			metaData.ThumbnailHeight = 75;
			metaData.PreviewWidth = 40;
			metaData.PreviewHeight = 40;
			metaData.IsPreviewCropped = true;
			//metaData.UseSubfolder = false;
			//metaData.Subfolder = "mike";
			return metaData;
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestProductCategory {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 21 Dec 2012 4:39:49pm ]