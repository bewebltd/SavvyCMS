package {

	import flash.text.StyleSheet;
	/*
	 * Provides flash stylesheets for HTML Textfields
	 * @author Jonathan Brake
	 */
	 
	public class CSS {

		public function CSS() {
			trace("Style is a static class and should not be instantiated.");
		}

		public static function HTMLBody():StyleSheet {

			var style:StyleSheet = new StyleSheet();
			var h1:Object = new Object();
			h1.fontFamily="Verdana, Arial, Helvetica, sans-serif";
			h1.fontWeight="bold";
			h1.fontSize="13";
			h1.color="#990000";
			h1.textAlign="left"
			style.setStyle("h1", h1);

			var p:Object = new Object();
			p.fontFamily="Verdana, Arial, Helvetica, sans-serif";
			p.fontSize="11";
			h1.color="#000000";
			h1.textAlign="left"
			style.setStyle("p", p);

			var important = new Object();
			important.color="#006699";
			style.setStyle(".important", important);
			return style;

		}


	}

}