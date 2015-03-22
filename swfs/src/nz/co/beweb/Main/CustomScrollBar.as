package {
	/*
	
	  # requires UIScrollBar component. #
	  
	 * Usage:
	 * var scrollBar:CustomScrollBar = new CustomScrollBar(scrolltarget, scrolltargetmask);
	 * addChild(_scrollBar);
	 *
	 *
	 * Improvements:
	 *
	 * Could utilise the Scrollbar Direction for horizontal scrolling
	 * import fl.controls.ScrollBarDirection;
	 * _scrollBar.direction = ScrollBarDirection.HORIZONTAL;
	 * or 
	 * _scrollBar.direction = ScrollBarDirection.VERTICAL; 
	 * then set the widths instead of heights.
	 *
	 *
	 */
	
	import fl.controls.ScrollBar;
	import flash.display.Sprite;
	import flash.events.Event;
	import fl.events.ScrollEvent;

	public class CustomScrollBar extends Sprite {

		private var _myTarget:*;
		private var _targetMask:*
		private var _scrollBar:ScrollBar
		

		public function CustomScrollBar(target:*, targetMask:*):void {
			_myTarget=target;
			_targetMask=targetMask
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}

		function Init(e:Event):void {
			trace("init:"+this);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			_myTarget.mask = _targetMask
			_scrollBar = new ScrollBar();
			_scrollBar.x = _myTarget.x + _myTarget.width -1;
			_scrollBar.y = _myTarget.y;
			_scrollBar.height = _targetMask.height;
			_scrollBar.enabled = true;
			_scrollBar.setScrollProperties(_targetMask.height, 0, (_myTarget.height-_targetMask.height-22)); // 22 seams to be an offsett difference. prevents over scrolling
			_scrollBar.addEventListener(ScrollEvent.SCROLL, ScrollMC);
			_myTarget.parent.addChild(_scrollBar)

		}

		function ScrollMC(event:ScrollEvent):void{
			_myTarget.y = -event.position + _targetMask.y;
		}


		function OnRemoved(e:Event):void {
			removeEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			_scrollBar.removeEventListener(ScrollEvent.SCROLL, ScrollMC);
		}
		
	}
	
}