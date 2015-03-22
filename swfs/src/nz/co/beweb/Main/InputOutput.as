package {
	
	/*
	 *
	 * Input/OutPut functions for loading and saving data.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import flash.display.Sprite;
	import flash.events.Event;
	

	public class InputOutput extends Sprite {

		private var _p:*;
		private var _m:*;
		
		
		public function InputOutput():void {
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}
		
		private function Init(e:Event):void {
			trace("Init: "+ this);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE,Removed);
			_p = this.parent;
			_m = root;
		}
		
		private function Removed(e:Event) {
			trace(this + ".Removed: "+e)
			removeEventListener(Event.REMOVED_FROM_STAGE,Removed);
		}
		
	}
	
}