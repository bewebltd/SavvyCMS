using System;
using Beweb;
#if MVC
using Site.SiteCustom;
#endif

namespace Models {
	public partial class Article {

		public static StringConst TEMPLATEMAINLYTEXT = new StringConst("Mainly Text");
		public static StringConst TEMPLATEMAINLYRESOURCES = new StringConst("Mainly Resources");
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Article object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			PublishDate = DateTime.Today;
			ShowArticleTitle = true;
			ShowArticleAuthor = true;
			Fields.Picture.MetaData = GetPicMetaData();

			SortPosition = 50;


		}

		public static PictureMetaDataAttribute GetPicMetaData() {
			var meta = new DefaultPictureMetaData();
			meta.IsExact = false;
			meta.IsCropped = false;
			meta.Width = 800;
			
			meta.IsSmallCropped = false;
			meta.IsSmallExact = false;
			meta.SmallHeight = 225;
			meta.SmallWidth = 225;
			
			meta.ThumbnailWidth = 100;
			meta.ThumbnailHeight = 100;
			return meta; //{Width = 123,Height = 456};
		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here

		public override string GetDefaultOrderBy() {
			return base.GetDefaultOrderBy();
		}

		public override string GetUrl() {
			//return base.GetUrl();
			return Web.ResolveUrl("~/ArticlePage/" + PageID + "/" + PathAndFile.CrunchFileName(GetName()) + "#Article" + ID);
		}

		public override void Save() {
			//AutocompletePhrase.AddPhrase(this, Fields.Title);
			base.Save();
			PageCache.Rebuild();
		}

		public override void Delete() {
			//AutocompletePhrase.DeletePhrase(this);
			base.Delete();
			PageCache.Rebuild();
		}
	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestArticle {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 13 May 2014 10:56:01am ]