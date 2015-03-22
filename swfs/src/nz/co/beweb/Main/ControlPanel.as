package {
	import flash.events.Event;
	import flash.display.Sprite;
	import flash.display.StageAlign;

	public class ControlPanel extends Sprite {

		private var _p:*;
		private var _r:*;
		private var _headingText:String;
		

		public function ControlPanel(headingText:String = ""):void {
			addEventListener(Event.ADDED_TO_STAGE, Init);
			_headingText = headingText;
		}

		private function Init(e:Event):void {
			trace("Init: "+this);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			_p = this.parent;
			_r = root;
			var speed:CustomSlider = new CustomSlider(100, 200, 100, 200, "Speed", StageAlign.TOP_LEFT)
			speed.x = 0
			speed.y = 0
			addChild(speed)
			
			var frequency:CustomSlider = new CustomSlider(100, 200, 100, 200, "Frequency", StageAlign.TOP_LEFT)
			frequency.x = speed.x
			frequency.y = Math.round(speed.y +speed.height);
			addChild(frequency)
			
			
			var amount:CustomSlider = new CustomSlider(100, 200, 100, 200, "Amount", StageAlign.TOP_LEFT)
			amount.x = frequency.x
			amount.y = Math.round(frequency.y + frequency.height);
			addChild(amount)
			
		}

		private  function OnRemoved(e:Event):void {
			removeEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
		}

	}

}