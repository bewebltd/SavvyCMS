using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class HomepageSlide {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new HomepageSlide object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			Fields.SlidePicture.MetaData = new DefaultPictureMetaData();
			Fields.SlidePicture.MetaData.IsExact = true;
			Fields.SlidePicture.MetaData.IsCropped = true;
			Fields.SlidePicture.MetaData.Width = 960;
			Fields.SlidePicture.MetaData.Height = 243;
			Fields.SlidePicture.MetaData.ThumbnailWidth = 100;
			Fields.SlidePicture.MetaData.ThumbnailHeight = 100;

			PublishDate = DateTime.Today;
			
		}


		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
	}
}

namespace BewebTest {
	[TestClass]
	public class TestHomepageSlide {
		[TestMethod]
		public static void TestSomething() {
			var expectedValue = 5;
			var actualValue = 5;
			Assert.AreEqual(expectedValue,actualValue);
		}
	}
}

// created: [ 2 Jul 2012 3:55:23pm ]