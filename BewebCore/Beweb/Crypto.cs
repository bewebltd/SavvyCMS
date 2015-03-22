#define TestExtensions
#define Crypto
#define Numbers
//
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Beweb;

// 
namespace Beweb {
	public class Crypto {
		private const string defaultKey = "hghgd79ds6fg8hg6s89djk376edjg0tjjd67df874jnduftsdd";

		#region EncryptID and DecryptID
		public static string EncryptID(int ID) {

			return Keystone.Keystone.Encrypt(("" + ID), ConfigurationManager.AppSettings["CryptKey"]);//+DateTime.Now.ToShortDateString());
		}
		public static string EncryptID(int? ID) {
			return Keystone.Keystone.Encrypt(("" + ID.Value), ConfigurationManager.AppSettings["CryptKey"]);//+DateTime.Now.ToShortDateString());
		}

		public static string EncryptID(string ID) {
#if Numbers
			if (!Numbers.isNumeric(ID)) throw new Exception("You cannot encrypt a non numeric using EncryptID");
#endif
			return Keystone.Keystone.Encrypt(("" + ID), ConfigurationManager.AppSettings["CryptKey"]);//+DateTime.Now.ToShortDateString());
		}

		//function DecryptID(id)
		//	if id<>"" then
		//		DecryptID = mid(id, 3, len(id)-9)
		//		DecryptID = (clng(DecryptID) * 2 - encryptIDKey) / 8
		//	else
		//		DecryptID = ""
		//	end if
		//end function


		public static string RandomDigit() {
			return Keystone.Keystone.RandomDigit();
		}

		public static string RandomLetter() {
			return Keystone.Keystone.RandomLetter();
		}
		/// <summary>
		/// return true if the token is less than numMinutes old or cant be decrypted. token is in the form Crypto.Encrypt( Fmt.DateTime(DateTime.Now))
		/// </summary>
		/// <param name="token">use Crypto.TimeToken or Crypto.Encrypt( Fmt.DateTime(DateTime.Now)) when calling this</param>
		/// <param name="numMinutes">num mins token is valid for</param>
		/// <returns></returns>
		public static bool CheckMinuteCypher(string token, int numMinutes) {
			bool result = false;
			try {
				DateTime tokenTime = Convert.ToDateTime(Crypto.Decrypt(token));
				result = tokenTime > DateTime.Now.AddMinutes(-numMinutes);

			} catch (Exception) {
				//fail
			}

			return result;
		}
		/// <summary>
		/// used by CheckMinuteCypher - encrypted date
		/// </summary>
		public static string TimeToken {
			get { return Encrypt(Fmt.DateTime(DateTime.Now)); }
		}


		/// <summary>
		/// Decrypt a given id and return a number, or -1 if the data is invalid or blank. uses crypt key in web.config
		/// </summary>
		/// <param name="encryptedID">data to decrypt</param>
		/// <returns>the id, or -1 if invalid</returns>
		public static int DecryptID(string encryptedID) {
			return DecryptID(encryptedID, -1);
		}
		public static int DecryptID(string encryptedID, int defaultValue) {
			int result = defaultValue;
			if (!string.IsNullOrEmpty(encryptedID)) {
				string key = ConfigurationManager.AppSettings["CryptKey"];
				if (key == "") throw new Exception("missing CryptKey");
				//key+= DateTime.Now.ToShortDateString();
				string res = Decrypt(encryptedID + "", key);

				try {
					result = Convert.ToInt32(res);
				} catch (Exception) {
					// failed 
					//Console.WriteLine("Exception: ["+ex.Message+"]");
				}

			}

			return result;

		}
		#endregion

		#region Encrypt
		// Encrypt a string into a string using a password 
		//		Uses Encrypt(byte[], byte[], byte[]) 
		public static string Encrypt(string clearText) {
			if (String.IsNullOrEmpty(clearText)) return ""; // for blank values return empty string
			return Keystone.Keystone.Encrypt(clearText, ConfigurationManager.AppSettings["CryptKey"]);
		}

		public static string Encrypt(string clearText, string password) {
			if (String.IsNullOrEmpty(clearText)) return ""; // for blank values return empty string
			return Keystone.Keystone.Encrypt(clearText, password);
		}

		// Encrypt bytes into bytes using a password 
		//		Uses Encrypt(byte[], byte[], byte[]) 

		// Encrypt a byte array into a byte array using a key and an IV 

		// Encrypt a file into another file using a password 

		#endregion

		#region Decrypt

		public static string Decrypt(string encryptedText) {
			return Decrypt(encryptedText, ConfigurationManager.AppSettings["CryptKey"]);
		}
		public static string Decrypt(string cipherText, string Password) {
			return Keystone.Keystone.Decrypt(cipherText, Password);
		}

		#endregion

		#region Compare

		#endregion

		#region Decrypt

		#endregion

		#region GenerateKey
		//20140520jn restored this for backward compat
		/// <summary>
		/// generates good keys for use in machinekeys
		/// For SHA1, set the validationKey to 64 bytes (128 hexadecimal characters).
		///	For AES, set the decryptionKey to 32 bytes (64 hexadecimal characters).
		///	For 3DES, set the decryptionKey to 24 bytes (48 hexadecimal characters).
		/// 
		/// so to create a new one for the machineKey setting, add a breakpoint, fire up the debugger, and type the following into your immediate window:
		/// Beweb.Crypto.GenerateKey(128)
		/// "49AD79CACDF7DEDDCFE33716FB0DC700B3FE6653373153C73B5BB287B2F726E250C8A55DEB4F82FC3DFBC382C6D9BBB2E3D1BA3C64D6E5696CB9FBE855C7704E"
		/// Beweb.Crypto.GenerateKey(64)
		/// "352273180EE6A92E2E41B3ECC0A6C837A6A8DA16D306FA4D4D8DFE0D5281CDD8"
		/// </summary>
		/// <param name="keyLengthInChars">pass in 64 for a 32 bit key</param>
		/// <returns></returns>
		public static string GenerateKey(int keyLengthInChars)
		{
			int len = 128;
			if (keyLengthInChars > 0) len = keyLengthInChars;
			byte[] buff = new byte[len / 2];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(buff);
			StringBuilder sb = new StringBuilder(len);
			for (int i = 0; i < buff.Length; i++)
				sb.Append(string.Format("{0:X2}", buff[i]));
			return sb.ToString();
		}
		
		#endregion

		#region CreateHash
		/// <summary>
		/// Takes a string and creates a hash of it. 
		/// Uses MD5 algorithm and returns as HEX string.
		/// If generating an API signature instead use CreateHashStandardMD5() which works very slightly differently and produces different results.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>a string 33 characters long such as: 1652CD7B754A33F07F8641EA2C48C77A</returns>
		public static string CreateHash(string str) {
			return Keystone.Keystone.CreateHash(str);
		}

		/// <summary>
		/// Takes a string and creates a hash of it.
		/// Uses MD5 algorithm and returns as HEX string.
		/// Similar to CreateHash() but works very slightly differently and produces different results. This one is the more standard and combatible with APIs.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>a string 33 characters long such as: 58321fff9c5a13d989d44d798a28f1af</returns>
		public static string CreateHashStandardMD5(string input) {
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++) {
				sb.Append(hash[i].ToString("x2"));
			}
			return sb.ToString();
		}

		#endregion

		private static readonly int classicEncryptIDKey = Util.GetSetting("Beweb.Crypto.EncryptIDClassicKey", "").ToInt(5762890);  // must be a multiple of 2
		public static string EncryptIDClassic(int id) {
			return Keystone.Keystone.EncryptIDClassic(id, classicEncryptIDKey);
		}

		public static int DecryptIDClassic(string encryptedID) {
			return Keystone.Keystone.DecryptIDClassic(encryptedID, classicEncryptIDKey);
		}

		public static double Random() {
			return Keystone.Keystone.Random();
		}

		public static int RandomInt(int min, int max) {
			return Keystone.Keystone.RandomInt(min, max);
		}

		public static string RandomChars(int length) {
			return RandomChars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ23456789", length);
		}

		public static string RandomChars(string allowedCharacters, int length) {
			string code = null;
			for (int i = 0; i < length; i++) {
				code += allowedCharacters[RandomInt(0, allowedCharacters.Length - 1)];
			}
			return code;
		}

		public static string EncryptIDShort(int id) {
			return EncryptIDShort(id, classicEncryptIDKey);
		}
		public static string EncryptIDShort(int id, int givenClassicEncryptIDKey) {
			int encryptedInt = (id * 8 + givenClassicEncryptIDKey) / 2;
			string result = Convert.ToBase64String(BitConverter.GetBytes(encryptedInt));
			result = result.RemoveSuffix("==");
			result = result.RemoveSuffix("A");
			result = result.RemoveSuffix("A");// second A
			return result;
		}

		public static int DecryptIDShort(string encryptedID) {
			return DecryptIDShort(encryptedID, classicEncryptIDKey);
		}

		public static int DecryptIDShort(string encryptedID, int givenClassicEncryptIDKey) {
			int result = -1;
			if (encryptedID + "" != "") {
				encryptedID = encryptedID.PadRight(6, 'A').PadRight(8, '=');
				int encryptedInt = BitConverter.ToInt32(Convert.FromBase64String(encryptedID), 0);
				result = (encryptedInt * 2 - givenClassicEncryptIDKey) / 8;
			}
			return result;
		}


	}
}
#if TestExtensions
namespace BewebTest {


	/// <summary>
	///This is a test class for CryptoTest and is intended
	///to contain all CryptoTest Unit Tests
	///</summary>
	[TestClass()]
	public class CryptoTest {


		/// <summary>
		///A test for Decrypt
		///</summary>
		[TestMethod()]
		public void DecryptReturnsCorrectClearText() {
			string encryptedText = "Hlo71eihqwh8n6JWICb27dCFUWiGDR+I0+o3LJvaU/0=";
			string expected = "asdf";
			string actual;
			actual = Crypto.Decrypt(encryptedText, "sadkyrwo3u4p1qwo3i12383810301293821");
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for Decrypt
		///</summary>
		[TestMethod()]
		public void DecryptReturnsEmptyStringWhenInvalidEncryptedStringOfTheRightLengthIsUsed() {
			string encryptedText = "Flo71eihqwh8n6JWICb27dCFUWiGDR+I0+o3LJvaU/0=";
			string expected = "";
			string actual;
			actual = Crypto.Decrypt(encryptedText, "sadkyrwo3u4p1qwo3i12383810301293821");
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for Decrypt
		///</summary>
		[TestMethod()]
		public void DecryptReturnsEmptyStringWhenSmallInvalidEncryptedStringIsUsed() {
			string encryptedText = "asdf";
			string expected = "";
			string actual;
			actual = Crypto.Decrypt(encryptedText, "sadkyrwo3u4p1qwo3i12383810301293821");
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for Encrypt
		///</summary>
		[TestMethod()]
		public void EncryptTest() {
			string clearText = "asdf";
			string expected = "Hlo71eihqwh8n6JWICb27dCFUWiGDR-I0-o3LJvaU_0~";
			string actual;
			actual = Keystone.Keystone.Encrypt(clearText, (string)"sadkyrwo3u4p1qwo3i12383810301293821");
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for EncryptID
		///</summary>
		[TestMethod()]
		public void EncryptIDClassicTest() {
			int ID = 2342;
			string encry = Crypto.EncryptIDClassic(ID);
			Assert.AreEqual(ID, Crypto.DecryptIDClassic(encry));

			// second test
			//Assert.AreEqual(125, Crypto.DecryptIDClassic("2728819457254TMT"));

			// tryu somethign
			//Web.Write(Crypto.EncryptIDClassic(125));
			//Web.Write("RandomDigit=" + Keystone.Keystone.RandomDigit());
			//Web.Write("=" + Keystone.Keystone.RandomLetter());

			for (int i = 0; i < 10; i++) {
				int n = Crypto.RandomInt(0, 99999);
				Assert.AreEqual(n, Crypto.DecryptIDClassic(Crypto.EncryptIDClassic(n)));
			}
		}

		/// <summary>
		///A test for DecryptID
		///</summary>
		[TestMethod()]
		public void DecryptIDTest() {
			string encryptedID = string.Empty;
			int expected = 0;
			int ID = 2343432;
			string actual = Crypto.EncryptID(ID);
			expected = Crypto.DecryptID(actual);
			Assert.AreEqual(ID, expected);
		}

		/// <summary>
		///A test for EncryptID
		///</summary>
		[TestMethod()]
		public void EncryptIDTest() {
			int ID = 2342;
			int expected = 0;
			string actual = Crypto.EncryptID(ID);
			expected = Crypto.DecryptID(actual);
			Assert.AreEqual(expected + "", "" + ID);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for Decrypt
		///</summary>
		[TestMethod()]
		public void DecryptTest() {
			string clearText = "this is a string test";
			Assert.AreEqual(clearText, Crypto.Decrypt(Crypto.Encrypt(clearText)));
			clearText = "thi\"\"\"\"s is a string\"@#$%#@\"#23'2''2 \"\"test";
			Assert.AreEqual(clearText, Crypto.Decrypt(Crypto.Encrypt(clearText)));
			clearText = "3412341212423412342342341234211234";
			Assert.AreEqual(clearText, Crypto.Decrypt(Crypto.Encrypt(clearText)));
			clearText = "3412341212423412342342341\\\n\n\t\t234211234";
			Assert.AreEqual(clearText, Crypto.Decrypt(Crypto.Encrypt(clearText)));
			clearText = @"3asdcasdc
sad
c
asdcasdLorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.

Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi. Ut wisi enim ad minim veniam, quis nostrud exerci taion ullamcorper suscipit lobortis nisl ut aliquip ex en commodo consequat. Duis te feugifacilisi per suscipit lobortis nisl ut aliquip ex en commodo consequat. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diem nonummy nibh euismod tincidunt ut lacreet dolore magna aliguam erat volutpat. Ut wisis enim ad minim veniam, quis nostrud exerci tution ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis te feugifacilisi. Duis autem dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zril delenit au gue duis dolore te feugat nulla facilisi.
asdc
asd

sdcaaaaaaaaaaaaaaasdacasdc


412341212423412342342341\\\n\n\t\t234211234";
			Assert.AreEqual(clearText, Crypto.Decrypt(Crypto.Encrypt(clearText)));
		}

		[TestMethod()]
		public void RandomIntTest() {
			bool got4 = false, got8 = false;
			for (int i = 0; i < 999; i++) {
				int randomInt = Crypto.RandomInt(4, 8);
				if (randomInt < 4) {
					Assert.Fail("Less than bottom bounds");
				}
				if (randomInt > 8) {
					Assert.Fail("More than top bounds");
				}
				if (randomInt == 4) {
					got4 = true;
				}
				if (randomInt == 8) {
					got8 = true;
				}
			}
			Assert.IsTrue(got4, "Very unlikely - never got a 4");
			Assert.IsTrue(got8, "Very unlikely - never got a 8");
		}

		[TestMethod()]
		public void RandomCharsTest() {
			bool got4 = false, got8 = false;
			for (int i = 0; i < 999; i++) {
				string randomInt = Crypto.RandomChars("45678", 1);
				if ("45678".DoesntContain(randomInt)) {
					Assert.Fail("Not in string");
				}
				if (randomInt == "4") {
					got4 = true;
				}
				if (randomInt == "8") {
					got8 = true;
				}
			}
			Assert.IsTrue(got4, "Very unlikely - never got a 4");
			Assert.IsTrue(got8, "Very unlikely - never got a 8");
		}
	}

}
#endif