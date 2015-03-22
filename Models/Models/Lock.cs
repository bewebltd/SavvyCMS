using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class Lock {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Lock object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
			DateAdded = DateTime.Now;
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
//	public class TestLock {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 19 Apr 2013 1:31:24pm ]