using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class Product {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Product object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
			IsActive = true;
			Fields.Picture1.MetaData = GetMetaData();
		}

		public string Action;

		public static PictureMetaDataAttribute GetMetaData() {
			var metaData = new PictureMetaDataAttribute();
			metaData.IsExact = false;
			metaData.IsCropped = false;
			metaData.Width = 1000;
			metaData.Height = 1000;
			metaData.DontResizeOriginal = true;

			metaData.ZoomWidth = 1000;
			metaData.ZoomHeight = 1000;
			metaData.IsZoomCropped = true;
			metaData.IsZoomExact = true;

			metaData.BigWidth = 800;
			metaData.BigHeight = 800;
			metaData.IsBigCropped = true;
			metaData.IsBigExact = true;

			metaData.SmallWidth = 400;
			metaData.SmallHeight = 400;
			metaData.IsSmallCropped = true;
			metaData.IsSmallExact = true;

			metaData.ThumbnailWidth = 200;
			metaData.ThumbnailHeight = 200;
			metaData.IsThumbnailCropped = true;
			metaData.IsThumbnailExact = true;

			metaData.PreviewWidth = 40;
			metaData.PreviewHeight = 40;
			metaData.IsPreviewCropped = false;
			metaData.IsPreviewExact = false;
			//metaData.UseSubfolder = false;
			//metaData.Subfolder = "mike";
			return metaData;
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here

		public string CategoryTitle {
			get {
				if (ProductCategory != null) {
					return ProductCategory.Title;
				}
				return null;
			}
		}
	}



}

//namespace BewebTest {
//	[TestClass]
//	public class TestProduct {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 2 Nov 2012 3:10:17pm ]