package {

	/*
	 * Provides formating and creation of fields, along with a few String related functions.
	 * @author Jonathan Brake
	 */

	import flash.text.*;
	import fl.controls.UIScrollBar;

	public class DateTime {
		
		private static var monthLongLabels:Array = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
		private static  var monthShortLabels:Array = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
		
		private static var dayLongLabels:Array = new Array("Sunday", "Monday", "Tuesday"," Wednesday", "Thursday", "Friday", "Saturday")
		private static var dayShortLabels:Array = new Array("Sun", "Mon", "Tue", " Wed", "Thu", "Fri", "Sat")
		
		public function DateTime() {
			trace("DateTime is a static class and should not be instantiated.");
		}
		
		// date functions
		public static function Now():String {
			// date in '18/02/2000 4:50pm' format
			var d:Date = new Date()
			var DD:String = CheckForLeadingZero(d.getDate().toString())
			var MM:String = CheckForLeadingZero((d.getMonth()+1).toString());
			var YYYY:String = d.getFullYear().toString();
			var hh:String
			var ampm:String = "a.m."
			if (d.getHours() > 12) {
				hh = (d.getHours() - 12).toString()
				ampm = "p.m."
				} else {
				hh = d.getHours().toString()
			}
			var mm:String = CheckForLeadingZero(d.getMinutes().toString())
			var ss:String = CheckForLeadingZero(d.getSeconds().toString()); 
			return  DD + "/" + MM + "/" + YYYY + " " + hh + ":" + mm + ":" + ss + " "+ ampm
		}
		
		public static function FmtDateTime(d:Date):String {
			// date in '18-Feb-2000 4:50pm' format
			var DD:String =CheckForLeadingZero(d.getDate().toString())
			var MMM:String = monthShortLabels[d.getMonth()];
			var YYYY:String = d.getFullYear().toString();
			var hh:String
			var ampm:String = "a.m."
			if (d.getHours() > 12) {
				hh = (d.getHours() - 12).toString()
				ampm = "p.m."
				} else {
				hh = d.getHours().toString()
			}
			var mm:String = CheckForLeadingZero(d.getMinutes().toString())
			var ss:String = CheckForLeadingZero(d.getSeconds().toString()); 
			return  DD + "-" + MMM + "-" + YYYY + " " + hh + ":" + mm + ":" + ss + " "+ ampm
		}
		
		public static function FmtDate(d:Date):String {
			// date in '18-Feb-2000' format
			var DD:String =CheckForLeadingZero(d.getDate().toString())
			var MMM:String = monthShortLabels[d.getMonth()];
			var YYYY:String = d.getFullYear().toString();
			return  DD + "-" + MMM + "-" + YYYY
		}	
		
		public static function FmtTime(d:Date):String{
			// time in '4:50 pm' format
			var hh:String
			var ampm:String = "a.m."
			if (d.getHours() > 12) {
				hh = (d.getHours() - 12).toString()
				ampm = "p.m."
				} else {
				hh = d.getHours().toString()
			}
			var mm:String = CheckForLeadingZero(d.getMinutes().toString())
			return hh + ":"+mm+" " + ampm
		}
		
		public static function FmtLongDateTime(d:Date):String{
			// date in 'Friday 18th February 2000' format
			return FmtLongDate(d) + " " + FmtTime(d)
		}
		
		public static function FmtDateMMMYYYY(d:Date):String {
			// date in 'February 2000' format
			var MMM:String = monthLongLabels[d.getMonth()];
			var YYYY:String = d.getFullYear().toString();
			return  MMM + " " + YYYY
			
		}	
		
		public static function FmtLongDate(d:Date):String {
			// date in 'Friday 18th February 2000' format
			var day:String = dayLongLabels[d.getDay()];
			var DD:String = d.getDate().toString();
			var lastnumber:String = DD.substring(DD.length-1)
			if(DD == "11" || DD == "12" || DD == "13"){
				DD += "th"			
			}else if (lastnumber == "1"){
				DD += "st"
			}else if (lastnumber == "2"){
				DD += "nd"
			}else if (lastnumber == "3"){
				DD += "rd"
			}else{
				DD += "th"
			}
			var MMM:String = monthLongLabels[d.getMonth()];
			var YYYY:String = d.getFullYear().toString();
			return  day + " " + DD + " " + MMM + " " + YYYY
		}

		public static function FmtShortDate(d:Date):String{
			// date in '18 Feb 00' format
			var DD:String = d.getDate().toString()
			var MMM:String = monthShortLabels[d.getMonth()];
			var year:String = d.getFullYear().toString();
			var YY:String = year.substring(year.length-2)
			return DD +" " + MMM + " " + YY
		}
		
		public static function FmtShortMonth(d:Date):String{
			return monthShortLabels[d.getMonth()];
		}
		public static function FmtLongMonth(d:Date):String{
			return monthLongLabels[d.getMonth()];
		}
		public static function FmtShortDay(d:Date):String{
			return dayShortLabels[d.getDay()];
		}
		public static function FmtLongDay(d:Date):String{
			return dayLongLabels[d.getDay()];
		}
		
		public static function FmtMonthYear(d:Date):String{
		  	// date in 'Feb 2000' format
			var MMM:String = monthShortLabels[d.getMonth()];
			var YYYY:String = d.getFullYear().toString();
			return MMM + " "+ YYYY
		}
		
		public static function FmtDateRange(validFrom:Date, validTo:Date):String{
			var range:String = ""
			if(validFrom == validTo){
				range = FmtShortDate(validFrom)
			}else{
				var isSameYear:Boolean = validFrom.getFullYear() == validTo.getFullYear()
				var isSameMonth:Boolean = validFrom.getMonth() == validTo.getMonth()
				if(isSameMonth){
					if(!isSameYear){
						range = FmtShortDate(validFrom)						
					}else{
						range = validFrom.getDate().toString()
					}
				}else if(isSameYear) {
					 range = validFrom.getDate().toString() + " " + monthShortLabels[validFrom.getMonth()];
				}else{
					range = FmtShortDate(validFrom)
				}				
				range += " - " + FmtShortDate(validTo)
			}
			return range
		}
		
		public static function GetWeekBegin(d:Date):Date{
			// return the date of the most recent monday
			do{ 
				d.setDate(d.getDate()-1)
			} while( d.getDay() != 1)
			
			return d
		}
		
		public static function GetMonthBegin(d:Date):Date{
			d.setDate(1)
			return d
		}
		
		public static function GetMonthEnd(d:Date):Date{
			d.setMonth(d.getMonth()+1)
			d.setDate(1)
			d.setDate(d.getDate()-1)		
			return d
		}
		
		public static function GetPreviousMonthBegin(d:Date):Date{
			d.setMonth(d.getMonth()-1)
			return GetMonthBegin(d)
		}
		
		public static function GetPreviousMonthEnd(d:Date):Date{
			var prevMonthEnd:Date = GetMonthBegin(d)
			prevMonthEnd.setDate(prevMonthEnd.getDate()-1)
			return prevMonthEnd
		}

		public static function GetNextMonthBegin(d:Date):Date{
			d.setMonth(d.getMonth()+1)
			return GetMonthBegin(d)
		}
		
		public static function GetNextMonthEnd(d:Date):Date{
			d.setMonth(d.getMonth()+2)
			d.setDate(1)
			d.setDate(d.getDate()-1)			
			return d
		}

		public static function CheckForLeadingZero(amount:String):String {
			if (amount.length<=1) {
					amount = "0" + amount;
			}
			return amount;
		}
		
		function isLeapYear( year : uint ) : Boolean {
		   return ( year % 4 == 0 ) && ( ( year % 100 != 0 ) || ( year % 400 == 0 ) )
		}
		
		
	}

}