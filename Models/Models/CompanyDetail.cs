using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class CompanyDetail {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new CompanyDetail object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
			Fields.CompanyPicture.MetaData = new PictureMetaDataAttribute();
			Fields.CompanyPicture.MetaData.IsExact = false;
			Fields.CompanyPicture.MetaData.IsCropped = false;
			Fields.CompanyPicture.MetaData.Width = 300;
			Fields.CompanyPicture.MetaData.Height = 300;
			Fields.CompanyPicture.MetaData.AllowPasteAndDrag = true;
			Fields.CompanyPicture.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			Fields.CompanyPicture.MetaData.ThumbnailHeight = 100;

			Fields.CompanyPicture1.MetaData = new PictureMetaDataAttribute();
			Fields.CompanyPicture1.MetaData.IsExact = false;
			Fields.CompanyPicture1.MetaData.IsCropped = false;
			Fields.CompanyPicture1.MetaData.Width = 300;
			Fields.CompanyPicture1.MetaData.Height = 300;
			Fields.CompanyPicture1.MetaData.AllowPasteAndDrag = true;
			Fields.CompanyPicture1.MetaData.ThumbnailWidth = 100; //this is the medium size image, preview is the very small image
			Fields.CompanyPicture1.MetaData.ThumbnailHeight = 100;

		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here

	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestCompanyDetail {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 14 Dec 2012 4:45:19pm ]