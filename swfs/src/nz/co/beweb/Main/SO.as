package {

	import flash.net.SharedObject
	import flash.events.Event

	public class SO {

		private static const SONAME:String = "VirtualTour"
		private var _so:SharedObject
		
		public function SO() {
			_so = SharedObject.getLocal(SONAME)
		}
		
		public function Save(property:String, data:*):void {
			_so.data[property] = data
			_so.flush()
		}
		
		public function Load(property:String):* {
			var value:* = _so.data[property]
			return value
		}
		
	}

}