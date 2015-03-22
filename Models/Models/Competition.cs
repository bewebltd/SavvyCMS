using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class Competition {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Competition object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			Fields.Picture.MetaData = GetPicMetaData();
			Fields.Picture.MetaData.IsExact = false;
			Fields.Picture.MetaData.IsCropped = false;
			Fields.Picture.MetaData.Width = 570;
			Fields.Picture.MetaData.Height = 570;
			Fields.Picture.MetaData.ThumbnailWidth = 320;
			Fields.Picture.MetaData.ThumbnailHeight = 320;

			PublishDate = DateTime.Today;


		}

		public static PictureMetaDataAttribute GetPicMetaData() {
			return new DefaultPictureMetaData(); //{Width = 123,Height = 456};
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
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
//	public class TestCompetition {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 29 Jan 2014 5:45:18pm ]