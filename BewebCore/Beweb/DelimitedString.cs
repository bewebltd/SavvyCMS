//#define TestExtensions
//#define JSONFunctions
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Beweb;

namespace Beweb {
	/// <summary>
	/// A basic string builder that puts commas between each string you add
	/// </summary>
	public class DelimitedString {
		private StringList Contents = new StringList();
		public string Delimiter = ",";
		//public int Count = 0;
		public static bool ThrowExceptionOnStringContainsDelimiter = false;   // false is the default, for backwards compatibility. This should be overridden in BewebCoreSettings. This is the global setting.
		internal protected bool throwExceptionOnStringContainsDelimiter = ThrowExceptionOnStringContainsDelimiter;          // this is the instance setting

		public DelimitedString() {
		}
		public DelimitedString(string delimiter) {
			Delimiter = delimiter;
		}
		/// <summary>
		/// Add an item to the string - regardless if it's already there or not. Does not increment count (as this is the static version)
		/// </summary>
		/// <param name="thisDelimitedString"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DelimitedString operator +(DelimitedString thisDelimitedString, object value) {
			if (value + "" == "") {
				return thisDelimitedString;
			}

			var result = new DelimitedString(thisDelimitedString.Delimiter);
			result.throwExceptionOnStringContainsDelimiter = thisDelimitedString.throwExceptionOnStringContainsDelimiter;
			if (thisDelimitedString.throwExceptionOnStringContainsDelimiter && (value + "").Contains(result.Delimiter)) throw new Exception("string contains delimiter value=[" + value + "] delimiter=[" + result.Delimiter + "]");
			result.Delimiter = thisDelimitedString.Delimiter;
			if (thisDelimitedString.Contents.IsBlank()) {
				result.Contents = value.ToString();
			} else {
				result.Contents += thisDelimitedString.Contents.ToString() + thisDelimitedString.Delimiter + value.ToString();
			}

			return result;
		}

		public bool IsBlank { get { return Contents.Count == 0; } }

		public bool IsNotBlank { get { return !IsBlank; } }

		public override string ToString() {
			if (Contents.Count == 0) return null;
			return Contents.ToString(Delimiter);
		}
		public string ToString(string defaultValueIfNone) {
			return (Contents.Count == 0) ? defaultValueIfNone : Contents.ToString();
		}

		/// <summary>
		/// add an item to the string, but only if it's not already in the string
		/// </summary>
		/// <param name="itemToAdd"></param>
		public void AddItemToString(string itemToAdd) {
			if (throwExceptionOnStringContainsDelimiter && (itemToAdd + "").Contains(Delimiter)) throw new Exception("itemToAdd contains delimiter");
			Contents.AppendUnique(itemToAdd);
			//if (throwExceptionOnStringContainsDelimiter && (itemToAdd + "").Contains(Delimiter)) throw new Exception("itemToAdd contains delimiter");
			//if ((Delimiter + this.contents + Delimiter).ToLower().Contains(Delimiter + itemToAdd.ToString().ToLower() + Delimiter)) {
			//	//already in itemToAdd
			//} else {
			//	var newRole = new DelimitedString();
			//	newRole.throwExceptionOnStringContainsDelimiter = false;
			//	newRole += this.contents;
			//	newRole += itemToAdd;
			//	this.contents = newRole.ToString();
			//	Count++;
			//}
		}

		public void RemoveItemFromString(string itemToRemove) {
			if ((itemToRemove + "").Contains(Delimiter)) throw new Exception("itemToRemove contains delimiter");
			Contents.Remove(itemToRemove);
			//if ((itemToRemove + "").Contains(Delimiter)) throw new Exception("itemToRemove contains delimiter");
			//if ((Delimiter + this.contents + Delimiter).ToLower().Contains(Delimiter + itemToRemove.ToString().ToLower() + Delimiter)) {
			//	var roles = this.contents.Split(',');
			//	var newRole = new DelimitedString();
			//	foreach (var role in roles) {
			//		if (role.ToLower() != itemToRemove.ToString().ToLower()) newRole += role;
			//	}
			//	this.contents = newRole.ToString();
			//	Count--;
			//} else {
			//	//doesnt contain the itemToAdd
			//}
		}


		//public static implicit operator DelimitedString(string initialingString) {
		//  return new DelimitedString(initialingString);
		//}


	}

	public class StringList : IEnumerable<string> {

		private readonly List<string> _stringList = new List<string>();

		public StringList() { }

		public StringList(string str) {
			Append(str);
		}

		public static StringList operator +(StringList thisStringList, object value) {
			if (value != null) {
				thisStringList._stringList.Add(value.ToString());
			}
			return thisStringList;
		}

		public static StringList operator +(StringList thisStringList, string value) {
			if (value != null) {
				thisStringList._stringList.Add(value);
			}
			return thisStringList;
		}

		public static StringList operator +(StringList thisStringList, StringList value) {
			if (value != null) {
				thisStringList._stringList.AddRange(value);
			}
			return thisStringList;
		}

		public static bool operator ==(StringList a, StringList b) {

			// If both are null, or both are same instance, return true.
			if (Object.ReferenceEquals(a, b)) {
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null)) {
				return false;
			}

			// Return true if the fields match:
			return a.ToString() == b.ToString();
		}

		public static bool operator !=(StringList a, StringList b) {
			return !(a == b);
		}

		public static implicit operator string(StringList instance) {
			return instance.ToString();
		}

		public static implicit operator StringList(string str) {
			return new StringList(str);
		}

		public override bool Equals(Object obj) {
			if (obj == null) {
				return false;
			}

			// Return true if the fields match:
			return base.Equals(obj) || ToString() == obj.ToString();
		}

		public bool Equals(StringList s) {
			if (s == null) {
				return false;
			}
			// Return true if the fields match:
			return base.Equals(s) || ToString() == s.ToString();
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public StringList Append(string str) {
			_stringList.Add(str);
			return this;
		}

		public StringList Append(IEnumerable<string> str) {
			_stringList.AddRange(str);
			return this;
		}

		public StringList AppendLine(string str) {
			Append(str).Append(Environment.NewLine);
			return this;
		}

		public StringList AppendUnique(string str) {
			if (!_stringList.Contains(str)) {
				Append(str);
			}
			return this;
		}
		public StringList AppendLineUnique(string str) {
			if (!_stringList.Contains(str)) {
				Append(str).Append(Environment.NewLine);
			}
			return this;
		}

		public StringList Add(string str) {
			_stringList.Add(str);
			return this;
		}

		public StringList Add(IEnumerable<string> str) {
			_stringList.AddRange(str);
			return this;
		}

		public StringList AddLine(string str) {
			Add(str).Add(Environment.NewLine);
			return this;
		}

		public StringList AddUnique(string str) {
			if (!_stringList.Contains(str)) {
				Add(str);
			}
			return this;
		}
		public StringList AddLineUnique(string str) {
			if (!_stringList.Contains(str)) {
				Add(str).Add(Environment.NewLine);
			}
			return this;
		}

		public StringList Remove(string str) {
			_stringList.Remove(str);
			return this;
		}

		public StringList RemoveAll(string str) {
			var removed = _stringList.Remove(str);
			while (removed) {
				removed = _stringList.Remove(str);
			}
			return this;
		}

		public bool Contains(string str) {
			return _stringList.Contains(str);
		}

		public StringList Clear(string str) {
			_stringList.Clear();
			return this;
		}

		public int Count {
			get { return _stringList.Count; }
		}

		public bool IsBlank() {
			return _stringList.Count == 0;
		}
		public bool IsNotBlank() {
			return !IsBlank();
		}

		public StringList Sort() {
			_stringList.Sort();
			return this;
		}

		public StringList SortDescending() {
			_stringList.Sort();
			_stringList.Reverse();
			return this;
		}
		public StringList Reverse() {
			_stringList.Reverse();
			return this;
		}

		public List<string> ToList() {
			return _stringList;
		}

		public IEnumerator<string> GetEnumerator() {
			return _stringList.GetEnumerator();
		}
		public override string ToString() {
			return String.Join(null, _stringList.ToArray());									//20141114jn changed this for ral
		}

		public string ToString(string delimiter) {
			return String.Join(delimiter, _stringList.ToArray());					 	//20141114jn changed this for ral
		}
		#if JSONFunctions

		public string ToJson() {
			return _stringList.JsonStringify();
		}
#endif
	}

}

#if TestExtensions

namespace BewebTest {
	[TestClass]
	public class TestDelimStringAddRemove {
		[TestMethod]
		public static void TestAddItem() {
			var delimTest = new DelimitedString();
			delimTest.AddItemToString("Test1");
			delimTest.AddItemToString("Test2");
			delimTest.AddItemToString("Test3");
			delimTest.AddItemToString("Test3");
			Assert.AreEqual("Test1,Test2,Test3", delimTest.ToString());
			delimTest = new DelimitedString();
			delimTest.AddItemToString("A");
			delimTest.AddItemToString("B");
			delimTest.AddItemToString("C");
			delimTest.AddItemToString("D");
			Assert.AreEqual("A,B,C,D", delimTest.ToString());
		}

		[TestMethod]
		public static void TestAddItemContainingDelimiter() {
			var delimTest = new DelimitedString();
			try {
				delimTest.AddItemToString("A,B");
				Assert.Fail("Should have thrown an exception");
			} catch (Exception) {
				Assert.Pass();
			}

			delimTest = new DelimitedString();
			delimTest.throwExceptionOnStringContainsDelimiter = false;
			try {
				delimTest.AddItemToString("A,B");
				delimTest.AddItemToString("C");
				delimTest.AddItemToString("D");
				Assert.AreEqual("A,B,C,D", delimTest.ToString());
			} catch (Exception) {
				Assert.Fail("Should NOT have thrown an exception");
			}
		}

		[TestMethod]
		public static void TestPlusWithDelim() {
			var delimTest = new DelimitedString();
			try {
				delimTest += "A,B";
				Assert.Fail("Should have thrown an exception");
			} catch (Exception) {
				Assert.Pass();
			}

			delimTest = new DelimitedString();
			delimTest.throwExceptionOnStringContainsDelimiter = false;
			try {
				delimTest += "A,B";
				delimTest += "C";
				delimTest += "D";
				Assert.AreEqual("A,B,C,D", delimTest.ToString());
			} catch (Exception) {
				Assert.Fail("Should NOT have thrown an exception");
			}
		}

		[TestMethod]
		public static void TestPlus() {
			var delimTest = new DelimitedString();
			delimTest += "A ";
			delimTest += "B";
			delimTest += "C";
			delimTest += "D";
			Assert.AreEqual(delimTest.ToString(), "A ,B,C,D");
		}

		[TestMethod]
		public static void TestPlusAnother() {
			var delimTest = new DelimitedString("-");
			delimTest += "A ";
			delimTest += "B";
			delimTest += "C";
			delimTest += "D";
			var delimTest2 = new DelimitedString(";");
			delimTest2 += "mike";
			delimTest2 += "andre";
			delimTest += delimTest2;
			Assert.AreEqual("A -B-C-D-mike;andre", delimTest.ToString());
		}

		[TestMethod]
		public static void TestAddRemoveItems() {
			var delimTest = new DelimitedString();
			delimTest.AddItemToString("Test1");
			delimTest.AddItemToString("Test2");
			delimTest.AddItemToString("Test3");
			delimTest.RemoveItemFromString("Test3");
			Assert.AreEqual(delimTest.ToString(), "Test1,Test2");

			delimTest = new DelimitedString();
			delimTest.AddItemToString("Test1");
			delimTest.AddItemToString("Test2");
			delimTest.AddItemToString("Test3");
			delimTest.RemoveItemFromString("Test2");
			Assert.AreEqual(delimTest.ToString(), "Test1,Test3");
			delimTest = new DelimitedString();
			delimTest.AddItemToString("Test1");
			delimTest.AddItemToString("Test2");
			delimTest.AddItemToString("Test3");
			delimTest.RemoveItemFromString("Test1");
			Assert.AreEqual(delimTest.ToString(), "Test2,Test3");
			delimTest = new DelimitedString();
			delimTest.AddItemToString("Test1");
			delimTest.AddItemToString("Test2");
			delimTest.AddItemToString("Test3");
			delimTest.RemoveItemFromString("Test1");
			delimTest.RemoveItemFromString("Test3");
			delimTest.RemoveItemFromString("Test2");
			Assert.IsNull(delimTest.ToString());
		}

	}

	[TestClass]
	public class TestStringList {
		[TestMethod]
		public static void TestAddingItems() {
			var list = new StringList();
			list.Append("A");
			list.Append("B");
			list += "C";
			list = list + "D";
			Assert.AreEqual("ABCD", list.ToString());

			var list2 = new StringList();
			list2.Append("E");
			list2.Append("F");
			list2 += "G";
			Assert.AreEqual("A,B,C,D,E,F,G", (list + list2).ToString(","));

			list2 = "mike";
			Assert.AreEqual("mike", list2.ToString(","));

		}

		[TestMethod]
		public void TestEquality() {
			var x = new StringList("x");
			var x2 = new StringList("x");
			var y = new StringList("y");
			var n = new StringList("x");
			n = null;
			Assert.AreEqual(x, x);
			Assert.AreEqual(x.ToString(), "x");
			Assert.AreEqual(x, x2);
			Assert.IsTrue(x == x, "x == x");
			Assert.IsTrue(x == "x", "x == 'x'");
			Assert.IsTrue(x == x2, "x == x2");
			Assert.IsTrue(x != y, "x != y");
			Assert.IsTrue(x != null, "x != null");
			Assert.IsFalse(x == null, "x == null");
			Assert.IsTrue(n == null, "n == null");
			Assert.IsFalse(n != null, "n != null");
			Assert.IsFalse(x == n, "x == n");
			Assert.IsTrue(x != n, "x != n");
			Assert.IsFalse(n == x, "n == x");
			Assert.IsTrue(n != x, "n != x");
		}

	}

	[TestClass]
	public class TestStringClassesPerformance {

		private const int executions = 5000000;

		[TestMethod]
		public static void TestStringBuilder() {
			Web.WriteLine("Executing " + executions + " times");
			var sw = new Stopwatch();
			var start = DateTime.Now;
			sw.Start();
			var sb = new StringBuilder();
			for (var i = 0; i < executions; i++) {
				sb.Append("Lorem ipsum dolor sit amet");
			}
			var result = sb.ToString();
			sw.Stop();
			Web.WriteLine("Execution time: " + sw.ElapsedMilliseconds + "ms");
			Web.WriteLine("Execution timer: " + start.FmtMillisecondsElapsed());
			Assert.IsTrue(sw.ElapsedMilliseconds < 1000, "ElapsedMilliseconds < 1000");
		}

		[TestMethod]
		public static void TestStringList() {
			Web.WriteLine("Executing " + executions + " times");
			var sw = new Stopwatch();
			sw.Start();
			var list = new List<string>();
			for (var i = 0; i < executions; i++) {
				list.Add("Lorem ipsum dolor sit amet");
			}
			var result = list.Join("");
			sw.Stop();
			Web.WriteLine("Execution time: " + sw.ElapsedMilliseconds + "ms");
			Assert.IsTrue(sw.ElapsedMilliseconds < 1000, "ElapsedMilliseconds < 1000");
		}

		[TestMethod]
		public static void TestBewebStringList() {
			Web.WriteLine("Executing " + executions + " times (using Append)");
			var sw = new Stopwatch();
			sw.Start();
			var list = new StringList();
			for (var i = 0; i < executions; i++) {
				list.Append("Lorem ipsum dolor sit amet");
			}
			var result = list.ToString();
			sw.Stop();
			Web.WriteLine("Execution time: " + sw.ElapsedMilliseconds + "ms");
			Assert.IsTrue(sw.ElapsedMilliseconds < 1000, "ElapsedMilliseconds < 1000");

			Web.WriteLine("Executing " + executions + " times (using +=)");
			sw.Restart();
			var list3 = new StringList();
			for (var i = 0; i < executions; i++) {
				list3 += "Lorem ipsum dolor sit amet";
			}
			result = list3.ToString();
			sw.Stop();

			Web.WriteLine("Execution time: " + sw.ElapsedMilliseconds + "ms");
			Assert.IsTrue(sw.ElapsedMilliseconds < 1000, "ElapsedMilliseconds < 1000");

			var list2 = new StringList();
			list2 += "Andre";
			list2 += "Mike";
			list2 += "Josh";
			list2.SortDescending();
			Web.WriteLine(list2.ToString());

			if (list2.Equals(new StringList("MikeJoshAndre"))) {
						Assert.Pass();
			} else {
				Assert.Fail("Are different");
			}

			if (list2.Equals("MikeJoshAndre")) {
				Assert.Pass();
			} else {
				Assert.Fail("Are different");
			}

			if (list2 == "MikeJoshAndre") {
					Assert.Pass();
			} else {
				Assert.Fail("Are different");
			}

		}

		[TestMethod]
		public static void TestStringConcat() {
			Web.WriteLine("Executing 1000 times");
			var sw = new Stopwatch();
			sw.Start();
			var result = "";
			// AF20141017 With a number greater than 1000 it blows up
			for (var i = 0; i < 1000; i++) {
				result = String.Concat(result, "Lorem ipsum dolor sit amet", "asdasdasdsa", "asdasdasdsa", "asdasdasdsa", "asdasdasdsa");
			}
			sw.Stop();
			Web.WriteLine("Execution time: " + sw.ElapsedMilliseconds + "ms");
			Assert.IsTrue(sw.ElapsedMilliseconds < 1000, "ElapsedMilliseconds < 1000");
		}
	}

}
#endif