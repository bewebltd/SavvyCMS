using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Keystone {
	public static partial class Keystone {

		public static void Encrypt(string fileIn, string fileOut, string Password) {

			// First we are going to open the file streams 
			FileStream fsIn = new FileStream(fileIn,
																			 FileMode.Open, FileAccess.Read);
			FileStream fsOut = new FileStream(fileOut,
																				FileMode.OpenOrCreate, FileAccess.Write);

			// Then we are going to derive a Key and an IV from the
			// Password and create an algorithm 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
																												new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
			                                                              0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			Rijndael alg = Rijndael.Create();
			alg.Key = pdb.GetBytes(32);
			alg.IV = pdb.GetBytes(16);

			// Now create a crypto stream through which we are going
			// to be pumping data. 
			// Our fileOut is going to be receiving the encrypted bytes. 
			CryptoStream cs = new CryptoStream(fsOut,
																				 alg.CreateEncryptor(), CryptoStreamMode.Write);

			// Now will will initialize a buffer and will be processing
			// the input file in chunks. 
			// This is done to avoid reading the whole file (which can
			// be huge) into memory. 
			const int bufferLen = 4096;
			byte[] buffer = new byte[bufferLen];
			int bytesRead;

			do {
				// read a chunk of data from the input file 
				bytesRead = fsIn.Read(buffer, 0, bufferLen);

				// encrypt it 
				cs.Write(buffer, 0, bytesRead);
			} while (bytesRead != 0);

			// close everything 

			// this will also close the unrelying fsOut stream
			cs.Close();
			fsIn.Close();
		}

		public static string Encrypt(string clearText, string Password) {
			//if (Password == null) { Password = Crypto.defaultKey;}
			// hash the clearText and add to the text - so we can check it on decrypt


			// First we need to turn the input string into a byte array. 
			byte[] clearBytes =
				Encoding.Unicode.GetBytes(clearText);

			MD5 md5 = MD5.Create();
			byte[] hash = md5.ComputeHash(clearBytes);
			// BitConverter.ToString(hash); // the hash

			// put the hash and the clearText together
			byte[] hashAndClear = new byte[hash.Length + clearBytes.Length];
			Buffer.BlockCopy(hash, 0, hashAndClear, 0, hash.Length);
			Buffer.BlockCopy(clearBytes, 0, hashAndClear, hash.Length, clearBytes.Length);

			// Then, we need to turn the password into Key and IV 
			// We are using salt to make it harder to guess our key
			// using a dictionary attack - 
			// trying to guess a password by enumerating all possible words. 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
																												new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
			                                                              0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			// Now get the key/IV and do the encryption using the
			// function that accepts byte arrays. 
			// Using PasswordDeriveBytes object we are first getting
			// 32 bytes for the Key 
			// (the default Rijndael key length is 256bit = 32bytes)
			// and then 16 bytes for the IV. 
			// IV should always be the block size, which is by default
			// 16 bytes (128 bit) for Rijndael. 
			// If you are using DES/TripleDES/RC2 the block size is
			// 8 bytes and so should be the IV size. 
			// You can also read KeySize/BlockSize properties off
			// the algorithm to find out the sizes. 
			byte[] encryptedData = Encrypt(hashAndClear,
																		 pdb.GetBytes(32), pdb.GetBytes(16));

			// Now we need to turn the resulting byte array into a string. 
			// A common mistake would be to use an Encoding class for that.
			//It does not work because not all byte values can be
			// represented by characters. 
			// We are going to be using Base64 encoding that is designed
			//exactly for what we are trying to do. 
			string result = Convert.ToBase64String(encryptedData);

			// make it URL safe (this is effectively URL-safe Base64)
			result = result.Replace('/', '_').Replace('+', '-').Replace('=', '~');

			return result;
		}


		/// <summary>
		/// Decrypt a file into another file using a password
		/// </summary>
		/// <param name="cipherText"></param>
		/// <param name="Password"></param>
		/// <returns></returns>
		public static string Decrypt(string cipherText, string Password) {
			//if (Password == null) { Password = defaultKey; }
			if (cipherText == null) { return ""; }

			// restore to raw Base64 from URL-safe encoded Base64
			cipherText = cipherText.Replace('_', '/').Replace('-', '+').Replace('~', '=');

			// First we need to turn the input string into a byte array. 
			// We presume that Base64 encoding was used 
			string result = "";
			cipherText = cipherText.Replace(' ', '+');
			byte[] cipherBytes;
			try {
				cipherBytes = Convert.FromBase64String(cipherText);
			} catch (Exception) {
				//throw new Exception("ex["+e.Message+"], cipherText["+cipherText+"]");
				return result;
			}

			// Then, we need to turn the password into Key and IV 
			// We are using salt to make it harder to guess our key
			// using a dictionary attack - 
			// trying to guess a password by enumerating all possible words. 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
					new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
						0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			// Now get the key/IV and do the decryption using
			// the function that accepts byte arrays. 
			// Using PasswordDeriveBytes object we are first
			// getting 32 bytes for the Key 
			// (the default Rijndael key length is 256bit = 32bytes)
			// and then 16 bytes for the IV. 
			// IV should always be the block size, which is by
			// default 16 bytes (128 bit) for Rijndael. 
			// If you are using DES/TripleDES/RC2 the block size is
			// 8 bytes and so should be the IV size. 
			// You can also read KeySize/BlockSize properties off
			// the algorithm to find out the sizes. 
			byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
			// if it's null or not even long enough to contain the hash - it's been comprimised
			if (decryptedData == null || decryptedData.Length <= 16) return result;

			// get the hash at the start (always first 16 bytes)
			byte[] hash = new byte[16];
			Buffer.BlockCopy(decryptedData, 0, hash, 0, 16);
			//BitConverter.ToString(hash); // "9F-FC-FB-69-D3-5F-49-F2-3C-14-F7-74-6F-00-30-AA"
			// get encrypted data
			byte[] clearBytes = new byte[decryptedData.Length - 16];
			Buffer.BlockCopy(decryptedData, 16, clearBytes, 0, decryptedData.Length - 16);
			// check the hashes
			MD5 md5 = MD5.Create();
			byte[] checkHash = md5.ComputeHash(clearBytes);
			if (!Compare(hash, checkHash)) return result; // hash doesn't check out - possibly tampered with

			// Now we need to turn the resulting byte array into a string. 
			// A common mistake would be to use an Encoding class for that.
			// It does not work 
			// because not all byte values can be represented by characters. 
			// We are going to be using Base64 encoding that is 
			// designed exactly for what we are trying to do. 
			// TODO: isn't this exactly what the comment above says NOT to do????? delete if this has been checked
			result = Encoding.Unicode.GetString(clearBytes);

			return result;

		}

		public static byte[] Encrypt(byte[] clearData, string Password) {
			// We need to turn the password into Key and IV. 
			// We are using salt to make it harder to guess our key
			// using a dictionary attack - 
			// trying to guess a password by enumerating all possible words. 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
																												new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
			                                                              0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			// Now get the key/IV and do the encryption using the function
			// that accepts byte arrays. 
			// Using PasswordDeriveBytes object we are first getting
			// 32 bytes for the Key 
			// (the default Rijndael key length is 256bit = 32bytes)
			// and then 16 bytes for the IV. 
			// IV should always be the block size, which is by default
			// 16 bytes (128 bit) for Rijndael. 
			// If you are using DES/TripleDES/RC2 the block size is 8
			// bytes and so should be the IV size. 
			// You can also read KeySize/BlockSize properties off the
			// algorithm to find out the sizes. 
			return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV) {
			// Create a MemoryStream to accept the encrypted bytes 
			MemoryStream ms = new MemoryStream();

			// Create a symmetric algorithm. 
			// We are going to use Rijndael because it is strong and
			// available on all platforms. 
			// You can use other algorithms, to do so substitute the
			// next line with something like 
			//TripleDES alg = TripleDES.Create(); 
			Rijndael alg = Rijndael.Create();

			// Now set the key and the IV. 
			// We need the IV (Initialization Vector) because
			// the algorithm is operating in its default 
			// mode called CBC (Cipher Block Chaining).
			// The IV is XORed with the first block (8 byte) 
			// of the data before it is encrypted, and then each
			// encrypted block is XORed with the 
			// following block of plaintext.
			// This is done to make encryption more secure. 

			// There is also a mode called ECB which does not need an IV,
			// but it is much less secure. 
			alg.Key = Key;
			alg.IV = IV;

			// Create a CryptoStream through which we are going to be
			// pumping our data. 
			// CryptoStreamMode.Write means that we are going to be
			// writing data to the stream and the output will be written
			// in the MemoryStream we have provided. 
			CryptoStream cs = new CryptoStream(ms,
																				 alg.CreateEncryptor(), CryptoStreamMode.Write);

			// Write the data and make it do the encryption 
			cs.Write(clearData, 0, clearData.Length);

			// Close the crypto stream (or do FlushFinalBlock). 
			// This will tell it that we have done our encryption and
			// there is no more data coming in, 
			// and it is now a good time to apply the padding and
			// finalize the encryption process. 
			cs.Close();

			// Now get the encrypted data from the MemoryStream.
			// Some people make a mistake of using GetBuffer() here,
			// which is not the right way. 
			byte[] encryptedData = ms.ToArray();

			return encryptedData;
		}

		public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV) {
			// Create a MemoryStream that is going to accept the
			// decrypted bytes 
			MemoryStream ms = new MemoryStream();

			// Create a symmetric algorithm. 
			// We are going to use Rijndael because it is strong and
			// available on all platforms. 
			// You can use other algorithms, to do so substitute the next
			// line with something like 
			//		 TripleDES alg = TripleDES.Create(); 
			Rijndael alg = Rijndael.Create();

			// Now set the key and the IV. 
			// We need the IV (Initialization Vector) because the algorithm
			// is operating in its default 
			// mode called CBC (Cipher Block Chaining). The IV is XORed with
			// the first block (8 byte) 
			// of the data after it is decrypted, and then each decrypted
			// block is XORed with the previous 
			// cipher block. This is done to make encryption more secure. 
			// There is also a mode called ECB which does not need an IV,
			// but it is much less secure. 
			alg.Key = Key;
			alg.IV = IV;

			// Create a CryptoStream through which we are going to be
			// pumping our data. 
			// CryptoStreamMode.Write means that we are going to be
			// writing data to the stream 
			// and the output will be written in the MemoryStream
			// we have provided. 
			CryptoStream cs = new CryptoStream(ms,
																				 alg.CreateDecryptor(), CryptoStreamMode.Write);

			// Write the data and make it do the decryption 
			cs.Write(cipherData, 0, cipherData.Length);
			// Now get the decrypted data from the MemoryStream. 
			// Some people make a mistake of using GetBuffer() here,
			// which is not the right way. 
			byte[] decryptedData = null;

			try {
				// Close the crypto stream (or do FlushFinalBlock). 
				// This will tell it that we have done our decryption
				// and there is no more data coming in, 
				// and it is now a good time to remove the padding
				// and finalize the decryption process. 
				cs.Close();

				// Now get the decrypted data from the MemoryStream. 
				// Some people make a mistake of using GetBuffer() here,
				// which is not the right way. 
				decryptedData = ms.ToArray();
			} catch (Exception) {
			}

			return decryptedData;
		}

		public static byte[] Decrypt(byte[] cipherData, string Password) {
			// We need to turn the password into Key and IV. 
			// We are using salt to make it harder to guess our key
			// using a dictionary attack - 
			// trying to guess a password by enumerating all possible words. 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
																												new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
			                                                              0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

			// Now get the key/IV and do the Decryption using the 
			//function that accepts byte arrays. 
			// Using PasswordDeriveBytes object we are first getting
			// 32 bytes for the Key 
			// (the default Rijndael key length is 256bit = 32bytes)
			// and then 16 bytes for the IV. 
			// IV should always be the block size, which is by default
			// 16 bytes (128 bit) for Rijndael. 
			// If you are using DES/TripleDES/RC2 the block size is
			// 8 bytes and so should be the IV size. 

			// You can also read KeySize/BlockSize properties off the
			// algorithm to find out the sizes. 
			return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
		}

		public static bool Compare(byte[] b1, byte[] b2) {
			if (b1.Length != b2.Length)
				return false;
			for (int i = 0; i < b1.Length; ++i)
				if (b1[i] != b2[i])
					return false;
			return true;
		}


		public static string EncryptIDClassic(int id, int classicEncryptIDKey) {
			string result;
			if (classicEncryptIDKey % 2 == 0) {
				int encryptedInt = (id * 8 + classicEncryptIDKey) / 2;
				result = encryptedInt.ToString();
				result = RandomDigit() + RandomDigit() + result;
				result += RandomDigit() + RandomDigit() + RandomDigit() + RandomDigit() + RandomLetter() + RandomLetter() + RandomLetter();
			} else {
				int num = ((id + 5) * 17) + classicEncryptIDKey;
				result = num.ToString();
				result = ReverseString(result);
				result = "1" + result;
				result = RandomDigit() + RandomDigit() + result + RandomDigit() + RandomDigit() + RandomDigit();
			}
			return result;
		}

		public static int DecryptIDClassic(string encryptedID, int classicEncryptIDKey) {
			int result = -1;
			if (encryptedID + "" != "") {
				if (classicEncryptIDKey % 2 == 0) {
					string encryptedInt = encryptedID.Substring(2).RemoveCharsFromEnd(7);
					result = (Convert.ToInt32(encryptedInt) * 2 - classicEncryptIDKey) / 8;
				} else {
					char lastChar = encryptedID[encryptedID.Length - 1];
					if (lastChar >= '0' && lastChar <= '9') {
						string str = encryptedID.Substring(2).RemoveCharsFromEnd(3);
						if (str.StartsWith("1")) {
							str = str.Substring(1);
							str = ReverseString(str);
							int num = (Convert.ToInt32(str) - classicEncryptIDKey);
							if (num % 17 == 0) {
								num = num / 17;
								result = num - 5;
							}
						}
					}
				}
			}
			return result;
		}

		private static string ReverseString(string str) {
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		public static string RemoveCharsFromEnd(this string str, int numCharsToRemove) {
			return left(str, str.Length - numCharsToRemove);
		}
		public static string left(string source, int numChars) {
			if (source == null) return null;
			if (numChars > source.Length) return source;

			return (numChars < 1) ? "" : source.Substring(0, numChars);
		}

		public static string RandomDigit() {
			return "" + Math.Floor(Random() * 9);
		}

		public static string RandomLetter() {
			return chr(65 + (int)(Random() * 25)).ToString();
		}

		//from VB
		static Random randomGenerator = new Random();
		public static double Random() {
			return randomGenerator.NextDouble();
		}
		public static int RandomInt(int min, int max) {
			return randomGenerator.Next(min, max + 1);
		}
		public static float rnd() {
			float res = randomGenerator.Next(200);
			res = (res / 200);
			return res;//return number less than 1
		}
		public static char chr(int code) {
			char result = (char)code;
			return result;
		}


		public static void Decrypt(string fileIn, string fileOut, string Password) {

			// First we are going to open the file streams 
			FileStream fsIn = new FileStream(fileIn,
																			 FileMode.Open, FileAccess.Read);
			FileStream fsOut = new FileStream(fileOut,
																				FileMode.OpenOrCreate, FileAccess.Write);

			// Then we are going to derive a Key and an IV from
			// the Password and create an algorithm 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
																												new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
			                                                              0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
			Rijndael alg = Rijndael.Create();

			alg.Key = pdb.GetBytes(32);
			alg.IV = pdb.GetBytes(16);

			// Now create a crypto stream through which we are going
			// to be pumping data. 
			// Our fileOut is going to be receiving the Decrypted bytes. 
			CryptoStream cs = new CryptoStream(fsOut,
																				 alg.CreateDecryptor(), CryptoStreamMode.Write);

			// Now will will initialize a buffer and will be 
			// processing the input file in chunks. 
			// This is done to avoid reading the whole file (which can be
			// huge) into memory. 
			const int bufferLen = 4096;
			byte[] buffer = new byte[bufferLen];
			int bytesRead;

			do {
				// read a chunk of data from the input file 
				bytesRead = fsIn.Read(buffer, 0, bufferLen);

				// Decrypt it 
				cs.Write(buffer, 0, bytesRead);

			} while (bytesRead != 0);

			// close everything 
			cs.Close(); // this will also close the unrelying fsOut stream 
			fsIn.Close();
		}

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
		public static string GenerateKey(int keyLengthInChars) {
			int len = 128;
			if (keyLengthInChars > 0) len = keyLengthInChars;
			byte[] buff = new byte[len / 2];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(buff);
			StringBuilder sb = new StringBuilder(len);
			for (int i = 0; i < buff.Length; i++)
				sb.Append(String.Format("{0:X2}", buff[i]));
			return sb.ToString();
		}

		public static string CreateHash(string str) {
			// First we need to convert the string into bytes, which
			// means using a text encoder.
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			// Create a buffer large enough to hold the string
			byte[] unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

			// Now that we have a byte array we can ask the CSP to hash it
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			// Build the final string by converting each byte
			// into hex and appending it to a StringBuilder
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++) {
				sb.Append(result[i].ToString("X2"));
			}

			// And return it
			return sb.ToString();
		}

		//public static string  CreateLock(string command) {
		//	return Encrypt(command+":"+DateTime.Now,cryptkey);
		//}
	}
}
