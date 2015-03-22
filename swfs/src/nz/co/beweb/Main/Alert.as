package {
	/*
	
	// import and initialise the alert box ready for action
	import Alert;
	Alert.init(stage);
	
	// the simplest way to show an alert
	Alert.show({alerttitle:"Dang it", alertmessage:"You can't do that!"});
	
	// Showing alerts with custom buttons
	var buttons:Array = new Array();
	buttons.push("Yes");
	buttons.push("No");
	buttons.push("Don't know");
	Alert.show({alerttitle:"Click a button", alertmessage:"Let me think about it", buttons:buttons, callback:AlertResponse});
	
	// Showing alerts with custom options
	var mybordercolour:int=0x009900;
	var mypromptalpha:Number=0.9;
	var mybodycolour:int=0xDD5CDE;
	var mytextcolour:int=0x7BBFAA;
	var myheadcolour:int=0x3CFF3C;
	var myheadtextcolour:int=0xFAD541;
	var myshadowcolour:int=0x4303DA;
	Alert.show({alerttitle:"Click a button", alertmessage:"Let me think about it", bodycolour:mybodycolour, textcolour:mytextcolour, promptalpha:mypromptalpha, bordercolour:mybordercolour,headcolour:myheadcolour,headtextcolour:myheadtextcolour,shadowcolour:myshadowcolour});
	
	// have a response to the alert box
	function AlertResponse(response:String):void {
		Alert.show({alerttitle:"Response", alertmessage:"The response was '"+response+"'", colour:0xAAAAAA});
	};
	
	*/
	import fl.controls.Button;
	import flash.display.CapsStyle;
	import flash.display.JointStyle;
	import flash.display.Sprite;
	import flash.display.Stage;
	import flash.events.MouseEvent;
	import flash.filters.BitmapFilterQuality;
	import flash.filters.DropShadowFilter;
	import flash.geom.Rectangle;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	import flash.text.TextFormatAlign;

	public class Alert {

		private static var stage:Stage=null;
		private static var btnWidth:int=100;
		private static var btnHeight:int=22;
		private static var minimumWidths:Array=new Array(btnWidth*2,btnWidth*3,btnWidth*4);
		private static var alertOptions:AlertOptions;

		public static function init(stageReference:Stage):void {
			stage=stageReference;
		}

		public static function show(ALERTOPTIONS:Object = null):void {
			if (stage==null) {
				trace("Alert class has not been initialised!");
				return;
			}
			alertOptions=new AlertOptions(ALERTOPTIONS);
			var myAlert:Sprite = new Sprite();
			// disable the background
			var myBackground:Sprite = new Sprite();
			myBackground.graphics.beginFill(Theme.ALERTOVERLAYCOLOUR, Theme.ALERTOVERLAYALPHA);
			myBackground.graphics.drawRect(0, 0, stage.stageWidth, stage.stageHeight);
			myBackground.graphics.endFill();
			myAlert.addChild(myBackground);
			myAlert.addChild(getPrompt());
			assignListeners(myAlert);
			stage.addChild(myAlert);
		}

		private static function assignListeners(myAlert:Sprite):void {
			var promptBackground:* =myAlert.getChildByName("actual_prompt");
			var allButtons:Array = new Array();
			for (var n:int; n<alertOptions.buttons.length; n++) {
				var button:Button=promptBackground.getChildByName(alertOptions.buttons[n]);
				button.addEventListener(MouseEvent.CLICK, myFunction);
				allButtons.push(button);
			}
			function myFunction(e:MouseEvent):void {
				for (var i:int; i<allButtons.length; i++) {
					allButtons[i].removeEventListener(MouseEvent.CLICK, myFunction);
				}
				closeAlert(myAlert);
				if (alertOptions.callback!=null) {
					alertOptions.callback(e.target.name);
				}
			}
		}

		private static function closeAlert(myAlert:Sprite):void {
			var promptBackground:* =myAlert.getChildAt(1);
			stage.removeChild(myAlert);
			myAlert=null;
		}

		private static function createButton(buttonText:String):Button {
			var myButton:Button = new Button();
			myButton.label=buttonText;
			myButton.name=buttonText;
			return myButton;
		}

		private static function getPrompt():Sprite {
			var actualPrompt:Sprite=createPrompt();
			actualPrompt.name="actual_prompt";
			actualPrompt.x = int((stage.stageWidth/2)-(actualPrompt.width/2));
			actualPrompt.y = int((stage.stageHeight/2)-(actualPrompt.height/2));
			return actualPrompt;
		}

		private static function getDropShadowFilter():DropShadowFilter {
			var color:Number=alertOptions.shadowcolour;
			var angle:Number=90;
			var alpha:Number=0.6;
			var blurX:Number=12;
			var blurY:Number=4;
			var distance:Number=1;
			var strength:Number=1;
			var inner:Boolean=false;
			var knockout:Boolean=false;
			var quality:Number=BitmapFilterQuality.LOW;
			return new DropShadowFilter(distance, angle, color, alpha, blurX, blurY, strength, quality, inner, knockout);
		}

		private static function createPrompt():Sprite {
			var promptBackground:Sprite = new Sprite();
			var titleField:TextField=createTitleField();
			var messageField:TextField=createMessageField();
			var myWidth:int=messageField.width+20;
			var myHeight:int=messageField.height+90;
			if (myWidth<titleField.width+20) {
				myWidth=titleField.width+20;
			}
			if (myWidth<minimumWidths[alertOptions.buttons.length-1]) {
				myWidth=minimumWidths[alertOptions.buttons.length-1];
			}
			if (myHeight<100) {
				myHeight=100;
			}
			if (myHeight>stage.stageHeight) {
				myHeight=stage.stageHeight-20;
				messageField.autoSize=TextFieldAutoSize.NONE;
				messageField.height=stage.stageHeight-40;
			}

			//add the box
			promptBackground.graphics.clear();
			// header
			promptBackground.graphics.beginFill(alertOptions.headcolour,0.9);
			promptBackground.graphics.lineStyle(0,alertOptions.headcolour, 0);
			promptBackground.graphics.drawRect(0,0,myWidth,30);
			promptBackground.graphics.endFill();
			//  body
			promptBackground.graphics.beginFill(alertOptions.bodycolour,0.9);
			promptBackground.graphics.lineStyle(0,alertOptions.bodycolour, 0);
			promptBackground.graphics.drawRect(0,30,myWidth,myHeight-31);
			promptBackground.graphics.endFill();
			// add white keyline
			promptBackground.graphics.lineStyle(0,0xFFFFFF,1,false,"normal", CapsStyle.NONE, JointStyle.MITER);
			promptBackground.graphics.drawRect(1,1,myWidth-2,myHeight-2);
			// add color keyline
			promptBackground.graphics.lineStyle(0,alertOptions.bordercolour,1,false, "normal", CapsStyle.NONE, JointStyle.MITER);
			promptBackground.graphics.drawRect(0,0,myWidth,myHeight);
			//Add the textfields to the prompt
			titleField.x=10;
			titleField.y=5;
			messageField.x=10;
			messageField.y=35;
			//add the buttons to the prompt
			var alertButtons:Array = new Array();
			for (var n:int; n<alertOptions.buttons.length; n++) {
				alertButtons.push(createButton(alertOptions.buttons[n]));
			}
			promptBackground.filters=[getDropShadowFilter()];
			promptBackground.alpha=alertOptions.promptalpha;
			var actualPrompt:Sprite = new Sprite();
			actualPrompt.addChild(promptBackground);
			var yPos=int(actualPrompt.height-35);
			switch (alertButtons.length) {
				case 1 :
					alertButtons[0].move((actualPrompt.width/2)-(btnWidth/2),yPos);
					break;
				case 2 :
					alertButtons[0].move((actualPrompt.width/2)-(btnWidth+7),yPos);
					alertButtons[1].move((actualPrompt.width/2)+7,yPos);
					break;
				case 3 :
					var midX:Number =(actualPrompt.width/2)-(btnWidth/2);
					alertButtons[1].move(midX,yPos);
					alertButtons[0].move(midX-(btnWidth+15),yPos);
					alertButtons[2].move(midX+(btnWidth+15),yPos);
					break;
			}
			actualPrompt.addChild(titleField);
			actualPrompt.addChild(messageField);
			for (var i:int; i<alertButtons.length; i++) {
				actualPrompt.addChild(alertButtons[i]);
			}
			return actualPrompt;
		}

		private static function createMessageField():TextField {
			var Text:String=alertOptions.alertmessage;
			var myTextField:TextField = new TextField();
			myTextField.textColor=alertOptions.textcolour;
			myTextField.multiline=true;
			myTextField.selectable=false;
			myTextField.autoSize=TextFieldAutoSize.LEFT;
			myTextField.htmlText='<font face="Verdana">'+Text+'</font>';
			return myTextField;
		}

		private static function createTitleField():TextField {
			var Text:String=alertOptions.alerttitle;
			var myTextField:TextField = new TextField();
			myTextField.textColor=alertOptions.headtextcolour;
			myTextField.multiline=false;
			myTextField.selectable=false;
			myTextField.autoSize=TextFieldAutoSize.LEFT;
			myTextField.htmlText='<font face="Verdana"><b>'+Text+'</b></font>';
			return myTextField;
		}

	}

}

internal class AlertOptions {

	public var alerttitle:String="Opps!";
	public var alertmessage:String="There was a problem.";
	public var buttons:Array=new Array();
	public var callback:Function;
	public var bordercolour:Number=Theme.ALERTBORDERCOLOUR;
	public var promptalpha:Number=Theme.ALERTBACKGROUNDALPHA;
	public var bodycolour:Number=Theme.ALERTBACKGROUNDCOLOUR;
	public var textcolour:Number=Theme.ALERTTEXTCOLOUR;
	public var headcolour:Number=Theme.ALERTHEADCOLOUR;
	public var headtextcolour:Number=Theme.ALERTHEADTEXTCOLOUR;
	public var shadowcolour:Number=Theme.ALERTSHADOWCOLOUR;

	public function AlertOptions(alertOptions:Object):void {

		if (alertOptions==null) {
			alertOptions={};
		}

		callback=alertOptions.callback;

		if (alertOptions.buttons==null) {
			buttons=["OK"];
		} else {
			if (alertOptions.buttons.length>3) {
				buttons=alertOptions.buttons.slice(0,2);
			} else {
				buttons=alertOptions.buttons;
			}
		}

		if (alertOptions.alerttitle!=null) {
			alerttitle=alertOptions.alerttitle;
		}

		if (alertOptions.alertmessage!=null) {
			alertmessage=alertOptions.alertmessage;
		}

		if (alertOptions.bordercolour!=null) {
			bordercolour=alertOptions.bordercolour;
		}

		if (alertOptions.promptalpha!=null) {
			promptalpha=alertOptions.promptalpha;
		}

		if (alertOptions.headcolour!=null) {
			headcolour=alertOptions.headcolour;
		}

		if (alertOptions.headtextcolour!=null) {
			headtextcolour=alertOptions.headtextcolour;
		}
		if (alertOptions.bodycolour!=null) {
			bodycolour=alertOptions.bodycolour;
		}

		if (alertOptions.textcolour!=null) {
			textcolour=alertOptions.textcolour;
		}

		if (alertOptions.shadowcolour!=null) {
			shadowcolour==alertOptions.shadowcolour;
		}

	}

}