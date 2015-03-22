package {
	
	/*
	 * Provides formating and creation of fields, along with a few String related function.
	 * @author Jonathan Brake
	 * 
	 * Extending dictionary in this case prevents random Compiler Error #1047 as explained - 
	 * http://www.senocular.com/flash/tutorials/compilererrors/
	 * 
	 */
	
	import flash.text.*;
	import fl.controls.UIScrollBar;
	import flash.utils.Dictionary;

	public class Format extends Dictionary {

		public function Format() {
			trace("Format is a static class and should not be instantiated.");
		}

		/* TEXT FORMATTING*/
		
		public static function ExampleFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			//var myFont:Font = new FontNameLinkage();
			//format.font = myFont.fontName;
			format.size = 15;
			format.color = Theme.BODYCOPYCOLOUR;
			return format;
		}
		
		public static function SubHeadingFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new ArialBold();
			format.font = myFont.fontName;
			format.bold = true;
			format.size = 14;
			format.color = 0x669966;
			return format;
		}

		public static function LabelFieldFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 12;
			format.color = 0x000000;
			return format;
		}

		public static function InputFieldFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 12;
			format.color = 0x000000;
			return format;
		}
		
		
		public static function CalenderCellFormat(fontcolor:Number = Theme.CALENDARCELLCOLOR):TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 10;
			format.color = fontcolor;
			return format;
		}

		public static function CalenderHeadFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 12;
			format.bold = true;
			format.color = Theme.CALENDARCOLOUR;
			return format;
		}
		
		public static function CalenderDayFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 7;
			format.color = Theme.CALENDARCOLOUR;;
			return format;
		}
		
		public static function CustomSliderLabelFormat():TextFormat {
			var format:TextFormat = new TextFormat();
			var myFont:Font = new Arial();
			format.font = myFont.fontName;
			format.size = 12;
			format.color = Theme.CUSTOMSLIDERCOLOUR;
			return format;
		}

		/* FIELD FORMATTING */
		
		public static function UserNameField(format:TextFormat, fieldWidth:Number = 220,  backgroundColour:Number = Theme.LOGINBOXFIELDBACKGROUNDCOLOUR, bordercolour:Number = Theme.LOGINBOXFIELDBORDERCOLOUR):TextField {
			var myField:TextField = new TextField();
			myField.type = TextFieldType.INPUT;
			myField.maxChars = 30;
			myField.defaultTextFormat = format;
			myField.setTextFormat(format);
			myField.border = true;
			myField.borderColor = bordercolour;
			myField.background = true;
			myField.backgroundColor = backgroundColour;
			myField.width = fieldWidth;
			myField.height = myField.textHeight + 4;
			return myField;
		}
		
		public static function PasswordField(fieldWidth:Number = 220,  backgroundColour:Number=Theme.LOGINBOXFIELDBACKGROUNDCOLOUR, bordercolour:Number = Theme.LOGINBOXFIELDBORDERCOLOUR):TextField {
			var myField:TextField = new TextField();
			myField.type = TextFieldType.INPUT;
			myField.maxChars = 30;
			myField.defaultTextFormat = InputFieldFormat();
			myField.setTextFormat(InputFieldFormat());
			myField.displayAsPassword = true;
			myField.border = true;
			myField.borderColor = bordercolour;
			myField.background = true;
			myField.backgroundColor = backgroundColour;
			myField.width = fieldWidth;
			myField.height = myField.textHeight + 4;
			return myField;
		}
		
		public static function SingleLineText(copy:String, format:TextFormat,isSelectable:Boolean = false):TextField {
			var myField:TextField = new TextField();
			myField.text = copy;
			myField.wordWrap = false;
			myField.selectable = isSelectable;
			myField.embedFonts = true;
			myField.defaultTextFormat = format;
			myField.setTextFormat(format);
			myField.height = myField.textHeight + 4;
			myField.width = myField.textWidth + 4;
			return myField;
		}


		public static function MultiLineText(copy:String, format:TextFormat, fieldWidth:Number, isSelectable:Boolean = false):TextField {
			var myField:TextField = new TextField();
			myField.text = copy;
			myField.wordWrap = true;
			myField.selectable = isSelectable;
			myField.embedFonts = true;
			myField.setTextFormat(format);
			myField.width = fieldWidth;
			myField.height = myField.textHeight + 4;
			return myField;
		}

		public static function addVerticalScrollbar(field:TextField){
			if (field.textHeight>field.height) {
				var fieldSB:UIScrollBar = new UIScrollBar();
				fieldSB.direction="Vertical";
				fieldSB.setSize(field.width, field.height);
				fieldSB.move(field.x+field.width, field.y);
				field.parent.addChild(fieldSB);
				fieldSB.scrollTarget=field;
			}
		}

		/* DATATYPE FORMATTING */
		public static function FormatBoolean(bool:*):Boolean {
			if (typeof(bool) == "xml") {
				bool = String(bool);
			}
			var returnVal:Boolean;
			switch (bool) {
				case true :
				case "True" :
				case "true" :
				case "Yes" :
				case "yes" :
				case 1 :
				case "1" :
					returnVal = true;
					break;
				case "" :
				case false :
				case "False" :
				case "false" :
				case "No" :
				case "no" :
				case 0 :
				case "0" :
					returnVal = false;
					break;
				default :
					trace("Failed to convert value ["+bool+"] of type "+typeof(bool)+" to a boolean.");
			}
			return returnVal;
		}

		/* STRING FORMATTING */

		public static function Trim(sourceString:String):String {
			sourceString = LeftTrim(sourceString);
			sourceString = RightTrim(sourceString);
			return sourceString;
		}

		public static function LeftTrim(sourceString:String):String {
			// remove leading spaces
			while (true) {
				if (sourceString.charAt(0) == " ") {
					sourceString = sourceString.substr(1,sourceString.length);
				} else {
					break;
				}
			}
			return sourceString;
		}

		public static function RightTrim(sourceString:String):String {
			// remove trailing spaces
			while (true) {
				if (sourceString.charAt(sourceString.length - 1) == " ") {
					sourceString = sourceString.substr(0,sourceString.length - 1);
				} else {
					break;
				}
			}
			return sourceString;
		}

		public static function StripString(sourceString:String,characterArray:Array):String {
			var illegalCharacters:Array = characterArray;
			var tmpString:String = decodeURIComponent(sourceString);
			var illegalCharactersLength:Number = illegalCharacters.length;
			for (var i:Number=0; i<illegalCharactersLength; i++) {
				var returnString:String = "";
				var thisChar:String = illegalCharacters[i];
				var tmpStringLength:Number = tmpString.length;
				for (var j:Number=0; j<tmpStringLength; j++) {
					var thisLetter:String = tmpString.charAt(j);
					if (thisLetter.indexOf(thisChar) == -1) {
						returnString +=  thisLetter;
					}
				}
				tmpString = returnString;
			}
			return returnString;
		}

		public static function RemoveCarriageReturns(txt:String,replaceChar:String):String {
			var returnString:String = "";
			var stringArray:Array = txt.split(String.fromCharCode(13));// 13 is the ascii CR
			var stingArrayLength:Number = stringArray.length;
			for (var thisString:Number=0; thisString<stingArrayLength; thisString++) {
				stringArray[thisString] = Trim(stringArray[thisString].substring(0));
				if (stringArray[thisString].substring(stringArray[thisString].length - 1,stringArray[thisString].length) == replaceChar) {
					stringArray[thisString] = Trim(stringArray[thisString].substring(0,stringArray[thisString].length - 1));
				} else {
					stringArray[thisString] = Trim(stringArray[thisString].substring(0));
				}
				if (thisString > 0) {
					returnString = returnString + replaceChar;
				}
				returnString = returnString + stringArray[thisString];
			}
			return returnString;
		}

		public static function RemoveDoubleCharacter(txt:String,char:String):String {
			var returnString:String = "";
			var doubleChar:String = char + char;
			if (txt.indexOf(doubleChar) != -1) {
				var stringArray:Array = txt.split(doubleChar);
				var stringArrayLength:Number = stringArray.length;
				for (var thisString:Number=0; thisString<stringArrayLength; thisString++) {
					if (thisString > 0) {
						returnString = returnString + char;
					}
					returnString = returnString + stringArray[thisString];
				}
			} else {
				returnString = txt;
			}
			if (txt.indexOf(doubleChar) != -1) {
				// check there are no double ups in new string
				returnString = RemoveDoubleCharacter(returnString,char);
			}

			return returnString;
		}

		public static function JSEscape(StringToEscape:String):String {
			//find and replace characters in a string
			var returnString:String = StringToEscape;
			returnString = RemoveCarriageReturns(returnString," ");
			returnString = FindAndReplace(returnString,"'","\\'");
			returnString = FindAndReplace(returnString,'"','\\"');
			returnString = FindAndReplace(returnString,"&","\\&");
			returnString = encodeURIComponent(returnString);
			return returnString;
		}
		
		/*
		 function UrlEncode(str)
			UrlEncode = server.UrlEncode(str&"")
		end function

		function HtmlEncode(str)
			HtmlEncode = server.HtmlEncode(str&"")
		end function

		*/
		
		
		public static function FriendlyHtml(originalHtml:String) {
			var returnHtml:String = originalHtml;
			// italics
			returnHtml = FindAndReplace(returnHtml,"<em>","<i>");
			returnHtml = FindAndReplace(returnHtml,"</em>","</i>");
			// bold
			returnHtml = FindAndReplace(returnHtml,"<strong>","<b>");
			returnHtml = FindAndReplace(returnHtml,"</strong>","</b>");
			return returnHtml;
		}

		public static function FindAndReplace(searchString:String,findString:String,replaceString:String) {
			return (searchString.replace(new RegExp(findString, "g"),replaceString));
		}


	}

}