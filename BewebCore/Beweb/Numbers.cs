#define Numbers
using System;
using Beweb;

namespace Beweb {
	/// <summary>
	/// Summary description for Numbers
	/// </summary>
	public class Numbers {
		public Numbers() {
		}

		/// <summary>
		/// Returns true if a string value could be converted to a number. 
		/// Bug fixed: used to only allow Integers but now accepts floating points (attempts to convert to Double).
		/// </summary>
		public static bool IsNumeric(string source) {
			try {
				var rs = (Convert.ToDouble(source));
			} catch (Exception)//ex
			{
				//dout("isNumeric ex[" + ex.Message + "]");
				return false;
			}
			return true;
		}

		public static bool IsNumeric(ValueType source) {
			try
			{
				var rs = (Convert.ToDouble(source));
			}
			catch (Exception)//ex
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns true if a string value could be converted to a number. 
		/// Bug fixed: used to only allow Integers but now accepts floating points (attempts to convert to Double).
		/// </summary>
		public static bool isNumeric(string source) {
			return IsNumeric(source);
		}

		public static bool IsInteger(ValueType source) {
			try
			{
				var rs = (Convert.ToInt32(source));
			}
			catch (Exception)//ex
			{
				return false;
			}
			return true;
		}

		public static bool IsInteger(string source) {
			try {
				var rs = (Convert.ToInt32(source));
			} catch (Exception)//ex
			{
				return false;
			}
			return true;
		}

		public static double GetGSTComponentOfGSTInclusiveValue(double GSTInclusiveValue) {
			if (DateTime.Now <= new DateTime(2010, 10, 1, 0, 0, 0)) {
				return GetGSTComponentOfGSTInclusiveValue(GSTInclusiveValue, 12.5);
			} else {
				return GetGSTComponentOfGSTInclusiveValue(GSTInclusiveValue, 15);

			}
		}
		public static double GetGSTRateOnAGivenDate() {
			var dateTimeNow = DateTime.Now;
			return GetGSTRateOnAGivenDate(dateTimeNow);
		}
		public static double GetGSTRateOnAGivenDate(DateTime timeToUse) {
			if (timeToUse <= new DateTime(2010, 10, 1, 0, 0, 0)) {
				return 12.5;
			} else {
				return 15;

			}
		}

		public static decimal AddGSTToNonGSTInclusiveValue(decimal NonGSTInclusiveValue) {
			return AddGSTToNonGSTInclusiveValue(NonGSTInclusiveValue, GetGSTRateOnAGivenDate());
		}

		public static double AddGSTToNonGSTInclusiveValue(double NonGSTInclusiveValue) {
			return AddGSTToNonGSTInclusiveValue(NonGSTInclusiveValue, GetGSTRateOnAGivenDate());
		}

		public static decimal AddGSTToNonGSTInclusiveValue(decimal NonGSTInclusiveValue, double gstRatePercentage) {
			return (decimal)AddGSTToNonGSTInclusiveValue((double)NonGSTInclusiveValue, gstRatePercentage);
		}
		public static double AddGSTToNonGSTInclusiveValue(double NonGSTInclusiveValue, double gstRatePercentage) {
			if (gstRatePercentage < 0 || gstRatePercentage > 100) throw new Exception("gstRatePercentage [" + gstRatePercentage + "] should be between 0 and 100");
			return Math.Round( NonGSTInclusiveValue + NonGSTInclusiveValue * (gstRatePercentage / 100), 2);
		}
		public static double GetGSTToAddToNonGSTInclusiveValue(double NonGSTInclusiveValue, double gstRatePercentage) {
			if (gstRatePercentage < 0 || gstRatePercentage > 100) throw new Exception("gstRatePercentage [" + gstRatePercentage + "] should be between 0 and 100");
			return Math.Round(NonGSTInclusiveValue * (gstRatePercentage / 100),2);
		}

		public static double GetGSTComponentOfGSTInclusiveValue(double GSTInclusiveValue, double gstRatePercentage) {
			if (gstRatePercentage < 0 || gstRatePercentage > 100) throw new Exception("gstRatePercentage [" + gstRatePercentage + "] should be between 0 and 100");
			return Math.Round((double)GSTInclusiveValue - ((double)GSTInclusiveValue / (1.0 + (gstRatePercentage / 100.0))),2);
		}

		/// <summary>
		/// return amount with GST removed (for todays date)
		/// </summary>
		/// <param name="GSTInclusiveValue"></param>
		/// <returns>amount with GST removed</returns>
		public static double GetAmountExcludingGST(double GSTInclusiveValue) {
			return GetAmountExcludingGST(GSTInclusiveValue, GetGSTRateOnAGivenDate());
		}
		/// <summary>
		/// return amount with GST removed, you need to specify the rate, or call GetGSTRateOnAGivenDate
		/// </summary>
		/// <param name="GSTInclusiveValue"></param>
		/// <returns>amount with GST removed</returns>
		public static double GetAmountExcludingGST(double GSTInclusiveValue, double gstRatePercentage) {
			if (gstRatePercentage < 0 || gstRatePercentage > 100) throw new Exception("gstRatePercentage [" + gstRatePercentage + "] should be between 0 and 100");
			return Math.Round((GSTInclusiveValue) - ((double)GSTInclusiveValue - ((double)GSTInclusiveValue / (1.0 + (gstRatePercentage / 100.0)))),2);
		}

		public static int Ceiling(double d) {
			return Math.Ceiling(d).ToInt();
		}

		public static int Floor(double d) {
			return Math.Floor(d).ToInt();
		}

		public static int Ceiling(decimal d) {
			return Math.Ceiling(d).ToInt();
		}

		public static int Floor(decimal d) {
			return Math.Floor(d).ToInt();
		}

		public static int Round(decimal d) {
			return Math.Round(d).ToInt();
		}

		public static double Round(double d, int decimalPlaces) {
			return Math.Round(d, decimalPlaces);
		}

		public static decimal Round(decimal d, int decimalPlaces) {
			return Math.Round(d, decimalPlaces);
		}

		public static double SafeDivide(double numerator, double denominator) {
			if (denominator == 0) {
				return 0;
			}
			return numerator / denominator;
		}

		public static decimal SafeDivide(decimal numerator, decimal denominator) {
			if (denominator == 0) {
				return 0;
			}
			return numerator / denominator;
		}

		public static decimal SafeDivide(int numerator, int denominator) {
			if (denominator == 0) {
				return 0;
			}
			return (decimal)numerator / (decimal)denominator;
		}

		/// <summary>
		/// Round to the nearest half eg 3.25 will be rounded to 3.5
		/// </summary>
		public static double RoundHalf(double rating) {
			return Math.Round(rating * 2, MidpointRounding.AwayFromZero) / 2; // eg 3.25 will be rounded to 3.5
		}
		/// <summary>
		/// Round to the nearest half eg 3.25 will be rounded to 3.5
		/// </summary>
		public static decimal RoundHalf(decimal rating) {
			return Math.Round(rating * 2, MidpointRounding.AwayFromZero) / 2; // eg 3.25 will be rounded to 3.5
		}


	}
}



namespace BewebTest {
	[TestClass]
	public class TestNumbers {
		[TestMethod]
		public static void TestGetGSTComponentOfGSTInclusiveValue() {

			Assert.AreEqual(15d + "", "" + Numbers.GetGSTComponentOfGSTInclusiveValue(115d));		// this will die after 1 oct 2010
			Assert.AreEqual(12.5d + "", "" + Numbers.GetGSTComponentOfGSTInclusiveValue(112.5d, 12.5d));
			Assert.AreEqual(15.0d + "", "" + Numbers.GetGSTComponentOfGSTInclusiveValue(115.0d, 15.0d));
			Assert.AreEqual(11.09 + "", "" + Numbers.GetGSTComponentOfGSTInclusiveValue(85.0d, 15.0d));
			Assert.AreEqual(35.1 + "", "" + Numbers.GetGSTComponentOfGSTInclusiveValue(269.1, 15.0d));		//234
			Assert.AreEqual((269.1) + "", "" + Numbers.AddGSTToNonGSTInclusiveValue(234d, 15.0d));		//234
			Assert.AreEqual(35.1 + "", "" + Numbers.GetGSTToAddToNonGSTInclusiveValue(234, 15.0d));		//234
			Assert.AreEqual(396.52 + "", "" + Numbers.GetAmountExcludingGST(456, 15.0d));

		}
	}
}
