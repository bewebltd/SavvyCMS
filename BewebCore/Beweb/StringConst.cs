using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beweb {
	public class StringConst : IEquatable<StringConst>, IEquatable<string> {
		readonly string code;
		string displayName;

		public StringConst(string code) {
			this.code = code;
			this.displayName = code;
		}
		public StringConst(string code, string displayName) {
			this.code = code;
			this.displayName = displayName;
		}


		/// <summary>
		/// Value of this displayed to user.
		/// </summary>
		public string DisplayName {
			get { return displayName; }
		}

		/// <summary>
		/// Returns value of this string as saved in the database.
		/// </summary>
		public string Code {
			get { return code; }
		}

		public override string ToString() {
			return code;
		}

		static public implicit operator string(StringConst value) {
			return value.code;
		}

		public static implicit operator StringConst(string str) {
			return new StringConst(str, str.SplitTitleCase());
		}

		public SqlizedValue Sqlize() {
			return code.Sqlize_Text();
		}

		static public implicit operator SqlizedValue(StringConst value) {
			return value.Sqlize();
		}
		//static public implicit operator SqlizedValue(List<StringConst> list) {
		// string result = "";
		// foreach (var stringConst in list) {
		// if (result != "") {
		// result += ", ";
		// }
		// result += stringConst.Sqlize();
		// }
		// return new SqlizedValue(result);
		//}

		public bool Equals(StringConst other) {
			return code == other.code;
		}

		public bool Equals(string other) {
			return code == other;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) {
				return false;
			} else if (obj is string) {
				return Equals((string)obj);
			} else if (obj is StringConst) {
				return Equals((StringConst)obj);
			}
			return false;
		}

		public override int GetHashCode() {
			return (code != null ? code.GetHashCode() : 0);
		}

		public static bool operator ==(StringConst lhs, StringConst rhs) { //jn removed as these refer to eachother, creating a stack overflow
			if ((object)rhs == null && (object)lhs == null) return true;
			if ((object)rhs == null || (object)lhs == null) return false;
			return lhs.Code == rhs.code;
			//return rhs != null && lhs != null && lhs.Code == rhs.code;
		}

		public static bool operator !=(StringConst lhs, StringConst rhs) {
			if ((object)rhs == null && (object)lhs == null) return false;
			if ((object)rhs == null || (object)lhs == null) return true;
			return lhs.Code != rhs.code;
		}


	}

	namespace BewebTest {
		public class StringConstTests {
			[TestMethod]
			public void TestStringConstEquality() {
				var x = new StringConst("x", "xx");
				var x2 = new StringConst("x", "xx");
				var y = new StringConst("y", "yy");
				var n = new StringConst("x", "xx");
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
	}
}

