using System;
using Beweb;
using Site.SiteCustom;

namespace Models {
	public partial class LiveChat {
		/// <summary>
		/// Sets all fields to default values and sets up meta data. This is called automatically when you instantiate a new LiveChat object.
		/// </summary>
		public override void InitDefaults() {
			// Alter this code to set appropriate defaults and meta data
						DateAdded = DateTime.Now;


		}

		// Overrides of default functionality go here (eg SortOrder, GetUrl, IsActive, Save)
		// Hint: to override methods using Resharper, hit Alt-Insert

		// You can put any business logic associated with this entity here
		
	}
}

namespace BewebTest {
	[TestClass]
	public class TestLiveChat {
		[TestMethod]
		public static void TestSomething() {
			var expectedValue = 5;
			var actualValue = 5;
			Assert.AreEqual(expectedValue,actualValue);
		}
	}
}

// created: [ 23 Mar 2012 4:34:35pm ]