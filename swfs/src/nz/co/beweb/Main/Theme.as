package {
	import flash.geom.Point;

	/*
	 * Theme values for the Courseware Manager. Change the values below to change the look and feel of dynamically created objects.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	public class Theme {
		// Colours
		public static const BODYCOPYCOLOUR = 0x000000;
		// Alert Box
		public static const ALERTOVERLAYCOLOUR:Number=0xFFFFFF;
		public static const ALERTOVERLAYALPHA:Number=0.7;
		public static const ALERTBACKGROUNDCOLOUR:Number=0xFFFFFF;
		public static const ALERTBACKGROUNDALPHA:Number=0.9;
		public static const ALERTSHADOWCOLOUR:Number=0x000000;
		public static const ALERTBORDERCOLOUR:Number=0x999999;
		public static const ALERTHEADCOLOUR:Number=0xCCCCCC;
		public static const ALERTHEADTEXTCOLOUR:Number=0x000000;
		public static const ALERTTEXTCOLOUR:Number=0x000000;
		
		// Login Box
		public static const LOGINBOXWIDTH:Number=340;
		public static const LOGINBOXHEIGHT:Number=180;
		public static const LOGINBOXKEYLINECOLOUR:Number=0x002262;
		public static const LOGINBOXKEYLINESIZE:Number=1;
		public static const LOGINBOXELLIPSE:Number=10;
		public static const LOGINBOXFILLCOLOUR:Number=0x5279a9;
		public static const LOGINBOXFILLCOLOUR2:Number=0x002262;/*second colour for gradient */
		public static const LOGINBOXFILLALPHA:Number=1;
		public static const LOGINBOXFILLALPHA2:Number=1;/*second alpha for gradient */
		public static const LOGINBOXSHADOWCOLOUR:Number=0x000000;
		public static const LOGINBOXHEADCOLOUR:Number=0x002262;
		public static const LOGINBOXHEADTEXTCOLOUR:Number=0xFFFFFF;
		public static const LOGINBOXTEXTCOLOUR:Number=0x002262;
		public static const LOGINBOXHEADHEIGHT:Number=40;
		public static const LOGINBOXFIELDBORDERCOLOUR:Number=0x002262;
		public static const LOGINBOXFIELDBACKGROUNDCOLOUR:Number=0xFFFFFF;
		
		// Calendar
		public static const CALENDARHEADBACKGROUNDCOLOUR:Number = 0x000000;
		public static const CALENDARHEADBACKGROUNDALPHA:Number = 0.5;
		public static const CALENDARHEADHEIGHT:Number = 25
		public static const CALENDARBACKGROUNDCOLOUR:Number = 0x000000;
		public static const CALENDARBACKGROUNDALPHA:Number =0.5
		public static const CALENDARPADDING:int = 2;
		public static const CALENDARBORDERCOLOUR:Number = 0x000000;
		public static const CALENDARBORDERALPHA:Number = 1;
		public static const CALENDARBORDERWIDTH:Number = 1;
		public static const CALENDARCOLOUR:Number = 0xFFFFFF;
		
		public static const CALENDARARROWFILL:Number = 0xA0A0A0;
		public static const CALENDARARROWFILLALPHA:Number = 0.5;
		public static const CALENDARARROWFILLHOVER:Number = 0xC3C3C3;
		public static const CALENDARARROWFILLHOVERALPHA:Number = 0.5;
		
		public static const CALENDARCELLDIMENSIONS:Point = new Point(25,25);
		public static const CALENDARCELLSPACING:int = 1;
		
		public static const CALENDARCELLBACKGROUNDCOLOR:Number = 0xCCCCCC;
		public static const CALENDARCELLBACKGROUNDCOLORSELECTED:Number = 0x999999;
		public static const CALENDARCELLBACKGROUNDALPHA:Number = 1;
		
		public static const CALENDARCELLBORDERWIDTH:Number = 1;
		public static const CALENDARCELLBORDERCOLOR:Number = 0x000000;
		public static const CALENDARCELLBORDERALPHA:Number = 1;
		public static const CALENDARCELLCOLOR:Number = 0x666666;
		public static const CALENDARCELLBACKGROUNDCOLORHOVER:Number = 0xFFFFFF;
		public static const CALENDARCELLBACKGROUNDALPHAHOVER:Number = 1;
		public static const CALENDARCELLBORDERCOLORHOVER:Number = 0x000000;
		public static const CALENDARCELLBORDERCOLORTODAY:Number = 0xFFFFFF;
		public static const CALENDARCELLBORDERALPHAHOVER:Number = 1;
		public static const CALENDARCELLCOLORHOVER:Number = 0x000000;
		// other months
		public static const CALENDARCELLALPHAOTHER:Number = 0.7
		public static const CALENDARCELLBACKGROUNDCOLOUROTHER:Number = 0xCCFF00;
		public static const CALENDARCELLBACKGROUNDCOLOROTHER:Number = 0x333333;
		public static const CALENDARCELLBACKGROUNDALPHAOTHER:Number = .5;
		public static const CALENDARCELLCOLOROTHER:Number = 0x999999;
		
		// CUSTOM SLIDER
		public static const CUSTOMSLIDERBACKGROUNDCOLOR:Number = 0x000000;
		public static const CUSTOMSLIDERBACKGROUNDALPHA:Number = 1;
		public static const CUSTOMSLIDERHEIGHT:int = 15;
		public static const CUSTOMSLIDERBORDERCOLOR:Number = 0xFFFFFF;
		public static const CUSTOMSLIDERBARBACKGROUNDCOLOR:Number = 0xFFFFFF;
		public static const CUSTOMSLIDERBARBACKGROUNDALPHA:Number = 1;
		public static const CUSTOMSLIDERCOLOUR:Number = 0xFFFFFF;
		
		public function Theme() {
			trace("Theme is a static class and should not be initiated.");
		}


	}

}