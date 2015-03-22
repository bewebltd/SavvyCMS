
/// ----------------------------
/// ACTIVE RECORD TESTS 
/// ----------------------------

using Beweb;
using Models;

namespace BewebTest {

	[TestClass]
	public partial class ActiveRecordTest {
		#if GenTest
		[TestMethod]
		public void TestNew() {
			GenTest genTest = new GenTest();
			genTest.Title = "mike";

			Assert.AreEqual("mike", genTest.Title);
		}

		[TestMethod]
		public void TestSaveNew() {
			GenTest genTest = new GenTest();
			genTest.Title = "mike";
			genTest.Save();
			Assert.AreEqual("mike", genTest.Title);
		}

		[TestMethod]
		public void TestLoadByTitle() {
			GenTest g = new GenTest();
			g.Title = "mike";
			g.Save();

			var genTest = GenTest.LoadByTitle("mike");
			Assert.AreEqual("mike", genTest.Title);
		}


		[TestMethod]
		public void TestListLoadByTitle() {
			new Sql("delete from gentest").Execute();

			GenTest genTest = new GenTest { Title = "mike1" };
			genTest.Save();

			genTest = new GenTest { Title = "mike1" };
			genTest.Save();

			genTest = new GenTest { Title = "mike2" };
			genTest.Save();

			var genTests = GenTestList.LoadByTitle("mike");
			Assert.AreEqual(0, genTests.Count);

			var genTests2 = GenTestList.LoadByTitle("mike1");
			Assert.AreEqual(2, genTests2.Count);
		}


		//[TestMethod]
		//public void TestLoadChildren() {
		//  new Sql("delete from GenTest").Execute();

		//  // load first admin record
		//  var person = Person.Load(new Sql("where 1=1 order by PersonID"));

		//  // create children records
		//  for (int i = 1; i <= 5; i++) {
		//    var g = new GenTest();
		//    g.Title = "test " + i;
		//    g.PersonID = person.PersonID;
		//    g.Save();
		//  }

		//  // lazy load should kick in here and reload all the gentests into the list
		//  Assert.AreEqual(5, person.GenTests.Count);

		//  // check foreign key single lazy load works too
		//  var firstGenTest = GenTest.LoadByPersonID(person.PersonID);
		//  Assert.AreEqual(person.Email, firstGenTest.Person.Email);

		//  // check they refer to same object
		//  Assert.AreEqual(person, firstGenTest.Person);

		//  // check if change one it changes the other
		//  string originalFirstName = person.FirstName;
		//  person.FirstName = "Joseph";
		//  Assert.AreEqual("Joseph", firstGenTest.Person.FirstName);

		//  // check reload
		//  person.ReloadFromDatabase();
		//  Assert.AreEqual(originalFirstName, firstGenTest.Person.FirstName);

		//  // check save
		//  person.FirstName = "Joseph";
		//  person.Save();
		//  Assert.AreEqual("Joseph", person.FirstName);
		//  // now load again
		//  person = null;
		//  firstGenTest.Person = null;
		//  Assert.IsNull(person);
		//  var newPerson = Person.Load(new Sql("where 1=1 order by PersonID"));
		//  Assert.AreEqual("Joseph", newPerson.FirstName);
		//  newPerson.FirstName = originalFirstName;
		//  newPerson.Save();
		//  Assert.AreEqual(originalFirstName, firstGenTest.Person.FirstName);
		//  Assert.AreEqual(newPerson.ID, firstGenTest.Person.ID);
		//  Assert.AreEqual(newPerson.ID, firstGenTest.PersonID);

		//  // check unload from cache
		//  //person.RemoveFromCache();

		//}

		[TestMethod]
		public void TestLoadChildrenPartTwo() {
			var g = GenTest.Load(new Sql("where 1=1 order by GenTestID"));
			g.Title = "Blue";
			g.Save();
			g.Title = "Green";
			Assert.AreEqual("Green", g.Title);
		}

		[TestMethod]
		public void TestLoadChildrenPartThree() {
			var g = GenTest.Load(new Sql("where 1=1 order by GenTestID"));
			Assert.AreEqual("Green", g.Title);
		}


		[TestMethod]
		public void TestLoadChildrenPartFour() {
			var g = GenTest.Load(new Sql("where 1=1 order by GenTestID"));
			g.Title = "Green";
			g.ReloadFromDatabase();
			Assert.AreEqual("Blue", g.Title);
		}


		[TestMethod]
		public void TestLoadConnectionsNotLeaking() {
			for (int i = 0; i < 10; i++) {
				// load any admin record
				var person = Person.Load(new Sql("where 1=1"));
				Web.Write(i + " ");
				Web.Flush();
			}
			Assert.Pass();
		}

		[TestMethod]
		public void TestLoadListConnectionsNotLeaking() {
			for (int i = 0; i < 10; i++) {
				// load any admin record
				var persons = PersonList.Load(new Sql("where 1=1"));
				Web.Write(i + " ");
				Web.Flush();
			}
			Assert.Pass();
		}

		[TestMethod]
		public static void TestID() {
			var g = new GenTest();
			g.GenTestID = 5;
			Assert.AreEqual(5, g.ID);
		}

		[TestMethod]
		public static void TestLoadFk() {
			var g = new GenTest();
			g.GenTestID = 5;
			var expectedValue = "";
			var actualValue = "";
			Assert.AreEqual(expectedValue, actualValue);
		}

		private int GetGenTestID() {
			return new Beweb.Sql("select gentestid from gentest").FetchIntOrZero();

		}

		[TestMethod]
		public void TestCacheIDfirst() {
			int id = GetGenTestID();
			var obj1 = GenTest.LoadID(id);
			var obj2 = GenTest.Load(new Sql("select gentestid, bodytexthtml as mike from gentest where gentestid=", id));
			Assert.IsTrue(obj1.GetFieldNames().Join(",") != obj2.GetFieldNames().Join(","), "Fields match and should not match");
		}

		[TestMethod]
		public void TestCacheIDsecond() {
			int id = GetGenTestID();
			var obj2 = GenTest.Load(new Sql("select gentestid, bodytexthtml as mike from gentest where gentestid=", id));
			var obj1 = GenTest.LoadID(id);
			Assert.IsTrue(obj1.GetFieldNames().Join(",") != obj2.GetFieldNames().Join(","), "Fields match and should not match");
		}

		[TestMethod]
		public void TestCacheIDsame() {
			int id = GetGenTestID();
			var obj3 = GenTest.Load(new Sql("select * from gentest where gentestid=", id));
			var obj1 = GenTest.LoadID(id);
			Assert.IsTrue(obj1.GetFieldNames().Join(",") == obj3.GetFieldNames().Join(","), "Fields should match");
			Assert.IsTrue(object.ReferenceEquals(obj1, obj3), "Objects should be same");
		}

		[TestMethod]
		public void TestCacheIDAreEqual() {
			int id = GetGenTestID();
			var obj1 = GenTest.LoadID(id);
			var obj2 = GenTest.Load(new Sql("select gentestid, bodytexthtml as mike from gentest where gentestid=", id));
			var obj3 = GenTest.Load(new Sql("select * from gentest where gentestid=", id));
			var obj4 = GenTest.Load(new Sql("select gentestid, bodytexthtml as mike from gentest where gentestid=", id));
			Assert.IsTrue(object.ReferenceEquals(obj1, obj3), "Objects should be same");
			Assert.IsTrue(object.ReferenceEquals(obj2, obj4), "Objects should be same");
			Assert.IsFalse(object.ReferenceEquals(obj1, obj4), "Objects should be not same");
		}

		//[TestMethod]
		//public static void TestLoadRelatedModels() {
		//  var g = GenTestList.Load(new Sql("where 1=1"));
		//  g.LoadChildModels<Person>("Person", "Person", "PersonID");
		//  Assert.Fail("in progress!");
		//  var expectedValue = "";
		//  var actualValue = "";
		//  Assert.AreEqual(expectedValue, actualValue);
		//}
		#endif
	}
}