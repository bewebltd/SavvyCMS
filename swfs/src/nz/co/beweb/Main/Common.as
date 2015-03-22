package {
	
	/*
	 *
	 * Common functions that are useful and accessible from any class.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import flash.display.Bitmap;
	import flash.display.BitmapData;
	
	import it.pacem.cryptography.Rijndael;

	public class Common {
		
		
		public function Common() {
			trace("Common is a static class and should not be initiated.");
		}
		
		// Math
		
		public static function RandomRange(min:Number, max:Number):Number {
			var randomNum:Number = Math.floor(Math.random()*(max-min+1))+min;
			return randomNum;
		}

		public static function ToRadian(a:Number):Number {
			return a * Math.PI / 180;
		}
		
		function SortNumeric(a, b):Number {
		  return a - b;
		}
		/*
		function Min(byval value1, byval value2)
			if value1 > value2 then
				Min = value2
			else
				Min = value1
			end if
		end function

		function Max(byval value1, byval value2)
			if value1 > value2 then
				Max = value1
			else
				Max = value2
			end if
		end function
*/
		// Bitmap
		
		public static function CopyImage(imgSource:Bitmap):Bitmap {
			return new Bitmap(imgSource.bitmapData.clone());
		}
		
		// Object
		
		public static function RemoveAllChildren(mc:*):void {
			while (mc.numChildren > 0) {
				mc.removeChildAt(0);
			}
		}

		public static function GetClass(o:Object):Class {
			return Object(o).constructor;
		}

		public static function GetChildrenOfType(mc:*, type:*):Array {
			var childArray:Array = new Array();
			var childCount:Number = mc.numChildren - 1;
			for (childCount; childCount >= 0; childCount += -1) {
				if (GetClass(mc.getChildAt(childCount)) == type) {
					childArray.push(mc.getChildAt(childCount));
				}
			}
			return childArray;
		}

		public static function RemoveChildrenOfType(mc:*, type:*):void {
			var childCount:Number = mc.numChildren - 1;
			for (childCount; childCount >= 0; childCount += -1) {
				if (GetClass(mc.getChildAt(childCount)) == type) {
					mc.removeChildAt(childCount);
				}
			}
		}

		public static function BringToFront(mc) {
			var lastChild = mc.parent.getChildAt(mc.parent.numChildren - 1);
			mc.parent.swapChildren(mc,lastChild);
		}

		public static function TraceAllChildren(mc:*):void {
			/* this could be improved to be recursive into child object and a ->,-->,--> type thing to show depth */
			trace("------Start------ CHILD TRACE");
			trace("parent = "+mc+" : "+mc.name);
			var childCount:Number = mc.numChildren;
			var thisChild:*;
			for (var i=0; i<childCount; i++) {
				thisChild = mc.getChildAt(i);
				trace(mc+" : "+mc.name+"."+thisChild.name+"  - "+Object(thisChild).constructor);
			}
			trace("-------End------- CHILD TRACE");
		}


	}

}