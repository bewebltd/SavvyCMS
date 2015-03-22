package {
	
	/*
	 * 
	 * Creates a Slider.
	 * @author Jonathan Brake
	 *
	 */
	
	import flash.display.MovieClip;
	import flash.display.Shape;
	import flash.geom.Point;
	import flash.events.MouseEvent;
	import flash.events.Event;
	import flash.text.TextField;
	import flash.display.StageAlign

	public class CustomSlider extends MovieClip {

		private var _p:*;
		private var _slider:MovieClip
		private var _bar:Shape;
		private var _myWidth:int;
		private var _minVal:int;
		private var _maxVal:int;
		private var _labelText:String;
		private var _labelPos:String;
		private var _isDisabled:Boolean;
		private var _defaultVal:Number;
		private var _myValue:int;
		
		public function CustomSlider(minVal:int, maxVal:int, defaultVal:int, myWidth:int = 100, labelText:String = "", labelPos:String=StageAlign.LEFT, isDisabled:Boolean = false):void {
			addEventListener(Event.ADDED_TO_STAGE, Init);
			_myWidth = myWidth
			_minVal = minVal;
			_maxVal = maxVal - minVal;
			_labelText = labelText;
			_labelPos = labelPos
			_isDisabled = isDisabled;
			_defaultVal = (defaultVal / maxVal);
		}

		private function Init(e:Event):void {
			trace(this + " Init")
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			_p = this.parent;
			_slider = new MovieClip();
			// background
			var sliderBG:Shape = new Shape();
			sliderBG.graphics.lineStyle(1,Theme.CUSTOMSLIDERBORDERCOLOR)
			sliderBG.graphics.beginFill(Theme.CUSTOMSLIDERBACKGROUNDCOLOR, Theme.CUSTOMSLIDERBACKGROUNDALPHA);
			sliderBG.graphics.drawRect(0,0, _myWidth,Theme.CUSTOMSLIDERHEIGHT);
			sliderBG.graphics.endFill();			
			_slider.addChild(sliderBG);
			// bar
			_bar = new Shape();
			_bar.graphics.beginFill(Theme.CUSTOMSLIDERBARBACKGROUNDCOLOR, Theme.CUSTOMSLIDERBARBACKGROUNDALPHA)
			_bar.graphics.drawRect(0, 0, _myWidth - 3, Theme.CUSTOMSLIDERHEIGHT - 3);
			_bar.graphics.endFill();
			Reset()
			_bar.x = 2;
			_bar.y = 2;
			_slider.addChild(_bar);
			addChild(_slider);
			if (!_isDisabled) {
				_slider.buttonMode = true;
				_slider.addEventListener(MouseEvent.MOUSE_DOWN, OnSliderMouseDown);
				_slider.addEventListener(MouseEvent.MOUSE_UP, OnSliderMouseUp);
			}
			// add the label
			if(_labelText != ""){
				var label:TextField = Format.SingleLineText(_labelText, Format.CustomSliderLabelFormat());
				switch(_labelPos) {
					case "TL":
						label.x = 0;
						label.y = label.height *-1
						break;
					case "T":
						label.x = (_slider.width / 2) - (label.width / 2);
						label.y = label.height * -1;
						break;
					case "TR":
						label.x = _slider.width - label.width;
						label.y = label.height * -1;
						break;
					case "R":
						label.x = _slider.width
						label.y = (_slider.height / 2) - (label.height / 2);
						break;
					case "BR":
						label.x = _slider.width - label.width;
						label.y = _slider.height;
						break;
					case "B":
						label.x = (_slider.width / 2) - (label.width / 2);
						label.y = _slider.height;
						break;
					case "BL":
						label.x = 0;
						label.y = _slider.height;
						break;
					case "L":
						label.x = label.width * -1;;
						label.y = (_slider.height / 2) - (label.height / 2);
						break;
					default:
						trace("No such label position exists: ["+_labelPos+"]")
				}
				addChild(label);
			}
		}
		
		public function Reset():void {
			UpdateDisplay(_defaultVal)
		}
		
		private function UpdateDisplay(percentage:Number):void {
			_myValue=Math.round(percentage * _maxVal)+_minVal
			_bar.scaleX = percentage;
			trace(this+".Value = "+_myValue)
		}
		
		private function OnSliderMouseDown(e:MouseEvent):void {
			addEventListener(Event.ENTER_FRAME, SliderEnterFrame);
		}

		private function OnSliderMouseUp(e:MouseEvent):void {
			removeEventListener(Event.ENTER_FRAME, SliderEnterFrame);
		}

		private function SliderEnterFrame(e:Event):void {
			var hitPoint:Point = new Point();
			hitPoint.x = mouseX + 1;//+1 allows you to get 0%
			hitPoint.y = mouseY + 1;
			hitPoint = localToGlobal(hitPoint)
			var hittest:Boolean = _slider.hitTestPoint(hitPoint.x, hitPoint.y, true);
			var total:int = _myWidth;
			var amount:int = _slider.mouseX;
			if (hittest){
				var percentage:Number = amount / total;
				if (percentage < 0){
					percentage = 0;
				}else if (percentage>1){
					percentage = 1;
				}
				UpdateDisplay(percentage)
			}else{
				removeEventListener(Event.ENTER_FRAME, SliderEnterFrame);
			}
		}
		
		public function get Value():int {
			return _myValue;
		}
		
		private function OnRemoved(e:Event):void {
			if (_slider.hasEventListener(MouseEvent.MOUSE_DOWN)) {
				_slider.removeEventListener(MouseEvent.MOUSE_DOWN, OnSliderMouseDown);
				_slider.removeEventListener(MouseEvent.MOUSE_UP, OnSliderMouseUp);
			}
			removeEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
		}

	}

}