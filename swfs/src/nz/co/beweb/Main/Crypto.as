package {
	
	/*
	 *
	 * Encrypt/Decrypt functions that are useful and accessible from any class.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import it.pacem.cryptography.Rijndael;

	public class Crypto {
		
		public function Crypto() {
			trace("Crypto is a static class and should not be initiated.");
		}
		
		public static function Encrypt(str:String):String{
			var aes:Rijndael = new Rijndael(Constant.AESKEYSIZE, Constant.AESBLOCKSIZE);
			return aes.encrypt(str, Constant.AESKEY, Constant.AESMODE);
			
		}
		
		public static function Decrypt(str:String):String{
			var aes:Rijndael = new Rijndael(Constant.AESKEYSIZE, Constant.AESBLOCKSIZE);
			return aes.encrypt(str, Constant.AESKEY, Constant.AESMODE);
		}

	}

}