using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class Testimonial {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new Testimonial object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						IsPublished = true;

			DateAdded = DateTime.Now;


		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
	}
}

//namespace BewebTest {
//	[TestClass]
//	public class TestTestimonial {
//		[TestMethod]
//		public static void TestSomething() {
//			var expectedValue = 5;
//			var actualValue = 5;
//			Assert.AreEqual(expectedValue,actualValue);
//		}
//	}
//}

// created: [ 28 Sep 2012 10:05:13am ]