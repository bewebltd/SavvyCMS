using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class RelatedPage {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new RelatedPage object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			SortPosition = 50;
			DateAdded = DateTime.Now;
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
//	public class TestRelatedPage {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 28 Mar 2014 9:30:46pm ]