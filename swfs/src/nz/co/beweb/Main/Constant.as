package {
	
	/*
	 * Constant variables use in application.
	 * @author Jonathan Brake
	 *
	 */
	 
	public class Constant {
		// Stage
		public static const STAGEWIDTH:int = 960;
		public static const STAGEHEIGHT:int = 910;
		// Google tracking
		public static const GOOGLEANALYTICSUANUMBER:String = "";
		public static const GOOGLEANALYTICSTRACKINGMODE:String = "AS3";//"Bridge"
		public static const GOOGLEANALYTICSDEBUG:Boolean = true;// false
		// URLs
		public static const LOGINURL = "services/Login";
		// validation
		public static const EMAIL_REGEX:RegExp = /^[a-z][\w.-]+@\w[\w.-]+\.[\w.-]*[a-z][a-z]$/i;
		// login stuff                      
		public static const AESKEY:String = "24CharStrUsed4encryption";
		public static const AESMODE:String = "ECB";
		public static const AESKEYSIZE:Number = 192;
		public static const AESBLOCKSIZE:Number = 128;
		
		public function Constant() {
			trace("Constant is a static class and should not be initiated.");
		}


	}

}