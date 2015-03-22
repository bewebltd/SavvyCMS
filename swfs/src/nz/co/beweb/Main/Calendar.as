package {
	
	/*
	 *
	 * Calender class.
	 * Requires DateTime Class
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.EventPhase;
	import flash.events.MouseEvent;
	import flash.geom.Point;
	import flash.text.TextField;
	import flash.text.TextFormat;

	public class Calendar extends MovieClip {
		private var _date:Date;
		private var _selectedDate:Date;
		private var _selectedDateCell:MovieClip;
		private var _today:Date;
		private var _displayMC:MovieClip;
		private var _monthMC:MovieClip;
		private var _calendarHead:MovieClip
		private var _monthLabels = new Array();
		private var _monthLongLabels:Array = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
		private var _monthShortLabels:Array = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
		private var _dayLabels:Array = new Array()
		private var _dayLongLabels:Array = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
		private var _dayShortLabels:Array = new Array("Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat");
		private var _dayAbbrievateLabels:Array = new Array("S", "M", "T", "W", "T", "F", "S");

		public function Calendar(date:Date = null):void {
			addEventListener(Event.ADDED_TO_STAGE,Init);	
			// if the date is null then set it to today
			_date = (date == null)? new Date() : date
			_selectedDate =  (date == null)? new Date() : date
			_today = new Date();
			// other things that could be set properties of this
			_monthLabels = _monthLongLabels;
			_dayLabels = _dayAbbrievateLabels
			//for testing
			//_date.setMonth(2)
		}
		
		private function Init(e:Event):void {
			trace("Init: "+ this);
			removeEventListener(Event.ADDED_TO_STAGE,Init);
			addEventListener(Event.REMOVED_FROM_STAGE, Removed);
			DrawCalendar()
		}
		
		private function DrawCalendar():void{
			// lay out buttons and days of the wee and any other constant visual elements.
			// work out the width 7*day width + 6*day padding + 2* calendarpadding
			var calendarWidth:int = (7*Theme.CALENDARCELLDIMENSIONS.x)+(12*Theme.CALENDARCELLSPACING)+(2*Theme.CALENDARPADDING)
			var calendarHeight:int = (6 * Theme.CALENDARCELLDIMENSIONS.y) + (5 * Theme.CALENDARCELLSPACING) + (2 * Theme.CALENDARPADDING) +( Theme.CALENDARCELLDIMENSIONS.y/2 + Theme.CALENDARCELLSPACING + Theme.CALENDARCELLBORDERWIDTH)
			// what ever we add to the top buttons + title....
			_displayMC = new MovieClip()
			// draw the heading bg
			var background:Sprite = new Sprite()
			background.graphics.beginFill(Theme.CALENDARBACKGROUNDCOLOUR, Theme.CALENDARBACKGROUNDALPHA)
			background.graphics.lineStyle(Theme.CALENDARBORDERWIDTH, Theme.CALENDARBORDERCOLOUR, Theme.CALENDARBORDERALPHA)
			background.graphics.drawRect(0, 0, calendarWidth, (calendarHeight+Theme.CALENDARHEADHEIGHT+Theme.CALENDARCELLSPACING*4));
			background.graphics.endFill()
			_displayMC.addChild(background)	
			// draw the calendar frame 
			_calendarHead = new MovieClip()
			_calendarHead.graphics.beginFill(Theme.CALENDARHEADBACKGROUNDCOLOUR, Theme.CALENDARHEADBACKGROUNDALPHA)
			_calendarHead.graphics.drawRect(0, 0, calendarWidth, Theme.CALENDARHEADHEIGHT);
			_calendarHead.graphics.endFill()
			// add the arrows to the header
			var leftbutton:MovieClip = MakeHeaderButton("previous")
			leftbutton.x = leftbutton.width;
			leftbutton.y = leftbutton.height;
			_calendarHead.addChild(leftbutton)
			var rightbutton:MovieClip = MakeHeaderButton("next")
			rightbutton.x = calendarWidth - rightbutton.width
			rightbutton.y = 0
			_calendarHead.addChild(rightbutton)
			// add the week labels to the header
			var dayLabelsLength:int = _dayLabels.length
			var xPos = Theme.CALENDARCELLSPACING *2
			var yPos = Theme.CALENDARHEADHEIGHT + Theme.CALENDARCELLSPACING
			for (var i = 0; i < dayLabelsLength; i++ ) {
				var dayHeader = new Sprite();
				dayHeader.graphics.beginFill(Theme.CALENDARHEADBACKGROUNDCOLOUR, Theme.CALENDARHEADBACKGROUNDALPHA)
				dayHeader.graphics.lineStyle(Theme.CALENDARCELLBORDERWIDTH, Theme.CALENDARCELLBORDERCOLOR, Theme.CALENDARCELLBORDERALPHA)
				dayHeader.graphics.drawRect(0, 0, Theme.CALENDARCELLDIMENSIONS.x, Theme.CALENDARCELLDIMENSIONS.y/2);
				dayHeader.graphics.endFill()
				// add the heading
				var heading:TextField = Format.SingleLineText(_dayLabels[i], Format.CalenderDayFormat());
				heading.x = (dayHeader.width/2) - (heading.width/2)
				heading.y = (dayHeader.height/2) - (heading.height/2)
				dayHeader.addChild(heading)
				xPos = (Theme.CALENDARCELLSPACING * 2) + ((Theme.CALENDARCELLDIMENSIONS.x + Theme.CALENDARCELLSPACING + Theme.CALENDARCELLBORDERWIDTH)*i)
				dayHeader.x = xPos
				dayHeader.y = yPos
				_calendarHead.addChild(dayHeader);	
			}
			
			
			_displayMC.addChild(_calendarHead)
			_monthMC = new MovieClip()
			_displayMC.addChild(_monthMC)
			DrawMonth()
			addChild(_displayMC);
		}
		
		private function MakeHeaderButton(direction:String):MovieClip {
			var button:MovieClip = new MovieClip()
			button.direction = direction;
			// add the arrow
			var arrow:Sprite = DrawArrow();
			var arrowPosition:Point = new Point((Theme.CALENDARCELLDIMENSIONS.x/2)-(arrow.width/2),(Theme.CALENDARCELLDIMENSIONS.y/2)-(arrow.height/2))
			arrow.x = arrowPosition.x
			arrow.y = arrowPosition.y
			button.arrow = arrow
			button.arrowPosition = arrowPosition
			button.addChild(arrow);
			// add the hit area
			var hitarea = new Sprite();
			hitarea.graphics.beginFill(0xFF00FF, 0);			
			hitarea.graphics.drawRect(0, 0, Theme.CALENDARCELLDIMENSIONS.x, Theme.CALENDARCELLDIMENSIONS.y)
			hitarea.graphics.endFill();
			button.addChild(hitarea);
			// add the events
			button.addEventListener(MouseEvent.MOUSE_OVER, OnHeadButtonOver);
			button.addEventListener(MouseEvent.MOUSE_OUT, OnHeadButtonOut);
			button.addEventListener(MouseEvent.CLICK, OnHeadButtonClick);
			button.addEventListener(Event.REMOVED_FROM_STAGE, OnHeadButtonRemoved);
			button.buttonMode = true;
			if (direction == "previous") {
				button.rotation = 180;
			}
			return button
		}
		
		private function DrawArrow(fill:Number = Theme.CALENDARARROWFILL, alpha:Number=Theme.CALENDARARROWFILLALPHA):Sprite {
			var arrow = new Sprite();
			arrow.graphics.beginFill(fill, alpha)
			arrow.graphics.moveTo(0, 0)
			var arrowHeight = Theme.CALENDARHEADHEIGHT/2
			arrow.graphics.lineTo(0,arrowHeight -  (Theme.CALENDARPADDING*2))
			arrow.graphics.lineTo((arrowHeight -  Theme.CALENDARPADDING*2)/1.2,(arrowHeight/2)-  (Theme.CALENDARPADDING))
			arrow.graphics.moveTo(0,0)
			arrow.graphics.endFill();
			return arrow;
			}
		
		private function DrawMonth():void {
			Common.RemoveAllChildren(_monthMC)
			Common.RemoveChildrenOfType(_calendarHead, TextField)
			_displayMC.removeChild(_monthMC)
			// add title
			var heading:TextField = Format.SingleLineText(_monthLabels[_date.getMonth()]+" "+_date.getFullYear(), Format.CalenderHeadFormat())
			heading.x = (_displayMC.width/2) -(heading.width/2)
			heading.y = (Theme.CALENDARHEADHEIGHT/2) - (heading.height/2)
			_calendarHead.addChild(heading)
			_calendarHead.heading = heading
			_monthMC = new MovieClip()
			_monthMC.x = (Theme.CALENDARCELLSPACING*2)
			_monthMC.y = _calendarHead.height + Theme.CALENDARCELLSPACING
			// work out the first day of the month
			var monthFirstDay:Number = GetMonthFirstDay(_date);
			var monthLastDay:Number = GetMonthLastDay(_date);
			var daysInMonth:Number = GetDaysInMonth(_date);
			var firstDateInMonth = monthFirstDay;
			var lastDateInMonth:Number = daysInMonth + monthFirstDay
			// there are 6 rows so 6x7= 42
			var row:int = 0
			var col:int = 0
			var day:int = 1
			var cell:MovieClip;
			var data:Object;
			for (var thisCell = 0; thisCell < 42; thisCell++ ) {
				data = new Object();
				data.isCurrentMonth = (thisCell >= firstDateInMonth && thisCell < lastDateInMonth);
				data.isToday = (_date.getDate() == day && _date.getMonth() == _today.getMonth() && _date.getFullYear() == _today.getFullYear());
				data.isSelectedDate = (day == _selectedDate.getDate() && _date.getMonth() == _selectedDate.getMonth() && _date.getFullYear() == _selectedDate.getFullYear());
				if (data.isCurrentMonth) {
					data.displayText = day
					day++;					
					cell = MakeCell(data)
					cell.addEventListener(MouseEvent.CLICK, OnCurrentMonthCellClick);
					if (data.isSelectedDate) { _selectedDateCell = cell }
				}else {
					var outsideMonthDate:Number 
					if (thisCell < firstDateInMonth) {
						outsideMonthDate = 1
						var previousMonth:Date = new Date(_date.getFullYear(), _date.getMonth() - 1);
						previousMonth.setDate(GetMonthLastDay(previousMonth) - (firstDateInMonth-(thisCell+1)));
						outsideMonthDate = previousMonth.date
						data.isNextMonth = false
					} else {
						outsideMonthDate = thisCell - lastDateInMonth + 1
						data.isNextMonth = true
					}
					data.displayText = outsideMonthDate
					cell = MakeCell(data)
					cell.addEventListener(MouseEvent.CLICK, OnNotCurrentMonthCellClick);
				}
				// check to see if this date is the selected date
				cell.buttonMode = true;
				cell.addEventListener(MouseEvent.MOUSE_OVER, OnCellMouseOver);
				cell.addEventListener(MouseEvent.MOUSE_OUT, OnCellMouseOut);
				cell.addEventListener(Event.REMOVED_FROM_STAGE, OnCellRemoved);
				cell.x = col * (Theme.CALENDARCELLDIMENSIONS.x + Theme.CALENDARCELLSPACING + Theme.CALENDARCELLBORDERWIDTH)
				cell.y = row * (Theme.CALENDARCELLDIMENSIONS.y + Theme.CALENDARCELLSPACING + Theme.CALENDARCELLBORDERWIDTH)
				_monthMC.addChild(cell)
				col++
				if(col%7 == 0){
					col = 0
					row++
				}
			}
			_displayMC.addChild(_monthMC)
		}
		
		private function MakeCell(data:Object, textFormat = ""):MovieClip {
				
			var box:MovieClip = new MovieClip();
			var bg:Sprite = new Sprite();
			var hitarea:Sprite = new Sprite();
			var bordercolor:Number = (data.isToday && data.isCurrentMonth)?Theme.CALENDARCELLBORDERCOLORTODAY:Theme.CALENDARCELLBORDERCOLOR;
			var backgroundcolor:Number = (data.isSelectedDate)?Theme.CALENDARCELLBACKGROUNDCOLORSELECTED:Theme.CALENDARCELLBACKGROUNDCOLOR;
			var textformat:TextFormat = Format.CalenderCellFormat()
			if (!data.isCurrentMonth) {
				backgroundcolor = Theme.CALENDARCELLBACKGROUNDCOLOROTHER;
				textformat = Format.CalenderCellFormat(Theme.CALENDARCELLCOLOROTHER)
			}
			var backgroundalpha:Number = (data.isCurrentMonth)?Theme.CALENDARCELLBACKGROUNDALPHA: Theme.CALENDARCELLBACKGROUNDALPHAOTHER;
			// draw the box
			bg.graphics.beginFill(backgroundcolor, backgroundalpha)
			bg.graphics.lineStyle(Theme.CALENDARCELLBORDERWIDTH,bordercolor, Theme.CALENDARCELLBORDERALPHA)
			bg.graphics.drawRect(0, 0, Theme.CALENDARCELLDIMENSIONS.x, Theme.CALENDARCELLDIMENSIONS.y);
			bg.graphics.endFill()
			box.addChild(bg)
			// add the text
			var display:TextField = Format.SingleLineText(data.displayText, textformat)
			display.x = (Theme.CALENDARCELLDIMENSIONS.x/2)-(display.width/2)
			display.y = (Theme.CALENDARCELLDIMENSIONS.y / 2) - (display.height / 2)
			box.addChild(display)
			// add a hit area
			hitarea.graphics.beginFill(0xFF00FF, 0)
			hitarea.graphics.drawRect(0, 0, Theme.CALENDARCELLDIMENSIONS.x, Theme.CALENDARCELLDIMENSIONS.y);
			hitarea.graphics.endFill()
			box.addChild(hitarea)
			box.background = bg
			box.displayText = display
			box.data = data;
			return box;
		}
		
		private function GetDaysInMonth(date):uint {
		   var daysOfMonths:Array = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
		   var dayCount = (IsLeapYear(date.getFullYear()) && date.getMonth()==1) ? 29 : daysOfMonths[date.getMonth()];
		   return dayCount
		}
		
		private function IsLeapYear(year:int):Boolean{
			   return ( year % 4 == 0 ) && ( ( year % 100 != 0 ) || ( year % 400 == 0 ))
		}
		
		private function GetMonthFirstDay(date):int {
			var tmpDate:Date = new Date(date.getFullYear(), date.getMonth(), 1)
			var day:int = tmpDate.getDay();
			return day;  
		}
		
		private function GetMonthLastDay(date):int {
			var tmpDate:Date = new Date(date.getFullYear(), date.getMonth())
			var day:int = GetDaysInMonth(tmpDate)
			return day;
		}
			
		private function ChangeSelectedDateCell(mc:MovieClip):void {
			// find the current selected date and redraw cell
			_selectedDateCell.data.isSelectedDate = false
			RedrawCell(_selectedDateCell)
			// set the selected date and redraw cell
			_selectedDateCell = mc
			_selectedDateCell.data.isSelectedDate = true
			// update the selected date
			_selectedDate = new Date(_date.getFullYear(),_date.getMonth(),_selectedDateCell.data.displayText)
			RedrawCell(_selectedDateCell, "hover")
		}
				
		private function RedrawCell(mc:MovieClip, state:String = ""):void {
			var backgroundcolor:Number;
			var backgroundalpha:Number;
			var bordercolor:Number;
			var borderalpha:Number;
			var color:Number;
			var textformat:TextFormat;
			var data:Object = mc.data;
			switch(state) {
				case "hover":
					backgroundcolor = Theme.CALENDARCELLBACKGROUNDCOLORHOVER;_selectedDate
					backgroundalpha = (!data.isCurrentMonth)?Theme.CALENDARCELLBACKGROUNDALPHAOTHER:Theme.CALENDARCELLBACKGROUNDALPHAHOVER;
					bordercolor = Theme.CALENDARCELLBORDERCOLORHOVER;
					bordercolor = (data.isToday && data.isCurrentMonth)?Theme.CALENDARCELLBORDERCOLORTODAY:Theme.CALENDARCELLBORDERCOLOR;
					borderalpha = Theme.CALENDARCELLBORDERALPHAHOVER;
					color = Theme.CALENDARCELLCOLORHOVER;
					textformat = Format.CalenderCellFormat(Theme.CALENDARCELLCOLORHOVER)
					break;
				default:
					backgroundcolor = (data.isSelectedDate)?Theme.CALENDARCELLBACKGROUNDCOLORSELECTED:Theme.CALENDARCELLBACKGROUNDCOLOR;
					if (data.isCurrentMonth) {
						backgroundalpha = Theme.CALENDARCELLBACKGROUNDALPHA;
						textformat = Format.CalenderCellFormat();
					} else {
						backgroundcolor = Theme.CALENDARCELLBACKGROUNDCOLOROTHER
						backgroundalpha = Theme.CALENDARCELLBACKGROUNDALPHAOTHER;
						textformat = Format.CalenderCellFormat(Theme.CALENDARCELLCOLOROTHER)
					}
					bordercolor = (data.isToday && data.isCurrentMonth)?Theme.CALENDARCELLBORDERCOLORTODAY:Theme.CALENDARCELLBORDERCOLOR;
					borderalpha = Theme.CALENDARCELLBORDERALPHA;
					color = Theme.CALENDARCELLCOLOR;
			}
			// draw the box
			var bg:Sprite = mc.background
			bg.graphics.clear();
			bg.graphics.beginFill(backgroundcolor, backgroundalpha)
			bg.graphics.lineStyle(Theme.CALENDARCELLBORDERWIDTH,bordercolor, borderalpha)
			bg.graphics.drawRect(0, 0, Theme.CALENDARCELLDIMENSIONS.x, Theme.CALENDARCELLDIMENSIONS.y);
			bg.graphics.endFill()
			// change the text color
			var display:TextField = mc.displayText
			display.setTextFormat(textformat)
		}		
	
		/* Events */
		
		
		private function OnHeadButtonOver(e:MouseEvent) {
			var btn:* = e.currentTarget;
			btn.removeChild(btn.arrow)
			var arrow:Sprite = DrawArrow(Theme.CALENDARARROWFILLHOVER, Theme.CALENDARARROWFILLHOVERALPHA)
			arrow.x = btn.arrowPosition.x
			arrow.y = btn.arrowPosition.y
			btn.addChildAt(arrow,0)
			btn.arrow = arrow
		}
		
		private function OnHeadButtonOut(e:MouseEvent) {
			var btn:* = e.currentTarget;
			btn.removeChild(btn.arrow)
			var arrow:Sprite = DrawArrow()
			arrow.x = btn.arrowPosition.x
			arrow.y = btn.arrowPosition.y
			btn.addChildAt(arrow,0)
			btn.arrow = arrow
			
			
		}
		
		/*
		 * 
		 * 
		 * 
		private function OnHeadButtonClick(e:MouseEvent):void {
			var btn:* = e.currentTarget;
			var month:int
			var year:int = _date.getFullYear();
			if (btn.direction == "next") {
				month = _date.getMonth() + 1
				if (month > 11) { 
					month = 0
					year = _date.getFullYear()+1
				}
			}else {
				month = _date.getMonth() - 1 
				if (month < 0) {
					month = 11;
					year = _date.getFullYear()-1
				}
			}
			_date = new Date(year, month)
			DrawMonth()
		}
		
		 * 
		 * */
		
		private function OnHeadButtonClick(e:MouseEvent) {
			var btn:* = e.currentTarget;
			var newMonth:int
			if (btn.direction=="next") {
				newMonth = _date.getMonth() + 1
				if (newMonth > 11) { 
					newMonth = 0
					_date.setFullYear(_date.getFullYear()+1)
				}
			}else {
				newMonth = _date.getMonth() - 1 
				if (newMonth < 0) {
					newMonth = 11;
					_date.setFullYear(_date.getFullYear()-1)
				}
			}
			_date.setMonth(newMonth);
			DrawMonth()
		}
		private function OnHeadButtonRemoved(e:Event) {
			var btn:* = e.currentTarget;
			btn.removeEventListener(MouseEvent.MOUSE_OVER, OnHeadButtonOver);
			btn.removeEventListener(MouseEvent.MOUSE_OUT, OnHeadButtonOut);
			btn.removeEventListener(MouseEvent.CLICK, OnHeadButtonClick);
			btn.removeEventListener(Event.REMOVED_FROM_STAGE, OnHeadButtonRemoved);
		}
		
		
		private function OnCellMouseOver(e:MouseEvent){
			var cell:* = e.currentTarget;
			RedrawCell(cell, "hover")
		}
		private function OnCellMouseOut(e:MouseEvent){
			var cell:* = e.currentTarget;
			RedrawCell(cell)
		}
		private function OnCurrentMonthCellClick(e:MouseEvent) {
			var cell:* = e.currentTarget;
			var data:Object = cell.data
			ChangeSelectedDateCell(cell)
		}
		
		private function OnNotCurrentMonthCellClick(e:MouseEvent) {
			var cell:* = e.currentTarget;
			var data:Object = cell.data			
			var newMonth:int
			if (data.isNextMonth) {
				newMonth = _date.getMonth() + 1
				if (newMonth > 11) { 
					newMonth = 0
					_date.setFullYear(_date.getFullYear()+1)
				}
			}else {
				newMonth = _date.getMonth() - 1 				
				if (newMonth < 0) {
					newMonth = 11;
					_date.setFullYear(_date.getFullYear()-1)
				}
			}
			_date = new Date(_date.getFullYear(), newMonth, data.displayText)
			_selectedDate = _date;
			DrawMonth()
		}
		
		private function OnCellRemoved(e:Event) {
			var cell:* = e.currentTarget;
			var data:Object = cell.data
			if (data.isCurrentMonth){
				cell.removeEventListener(MouseEvent.CLICK, OnCurrentMonthCellClick);
			}else {
				cell.removeEventListener(MouseEvent.CLICK, OnNotCurrentMonthCellClick);
			}
			cell.removeEventListener(MouseEvent.MOUSE_OVER, OnCellMouseOver);
			cell.removeEventListener(MouseEvent.MOUSE_OUT, OnCellMouseOut);
			cell.removeEventListener(Event.REMOVED_FROM_STAGE, OnCellRemoved);
		}
		
		/* End */
		private function Removed(e:Event):void {
			trace(this + ".Removed: "+e)
			removeEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}
		
	}
	
}