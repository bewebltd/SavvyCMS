package {
	
	/*
	 *
	 * Validation functions that are useful and accessible from any class.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */


	public class Validate {
		
		
		public function Validate() {
			trace("Validate is a static class and should not be initiated.");
		}

		
		public static function Email(str:String):Boolean{
			return Constant.EMAIL_REGEX.test(str);
		}
		
		public static function IsNumberic(str:String):Boolean {
			if (int(str) == 0) {
				return false;
			}
			return 	true;
		}
		
		public static function IsNullOrEmpty(str:String):Boolean {
			switch (str) {
				case "" :
				case null :
				case undefined :
					return true;
				default :
					return false;
			}

		}


	}

}