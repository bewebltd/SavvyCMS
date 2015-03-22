package {

	/*
	 * Creates a Login Box and check there are values in fields when submitted.
	 * Requires Button component.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import fl.controls.Button;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.MouseEvent;
	import flash.display.Sprite;
	import flash.display.MovieClip;
	import flash.display.Shape;
	import flash.text.TextField;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.net.URLVariables;
	import flash.net.URLLoaderDataFormat;
	import flash.net.URLRequestMethod;
	import flash.text.TextFormat;

	public class LoginBox extends Sprite {

		private var _p:*;
		private var _m:*;
		private var _loginBox:MovieClip;
		private var _userName:TextField;
		private var _password:TextField;
		private var _loginButton:Button;
		private var _loader:URLLoader;

		public function LoginBox() {
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}
		function Init(e:Event):void {
			trace("Init: "+ this);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			_p = parent;
			_m = root;
			// create the box
			_loginBox = new MovieClip();
			_loginBox.x = (stage.stageWidth/2) - (Theme.LOGINBOXWIDTH/2);
			_loginBox.y = (stage.stageHeight/2) - (Theme.LOGINBOXHEIGHT/2);
			var boxHeadBG:Shape = new Shape();
			boxHeadBG.graphics.lineStyle();
			boxHeadBG.graphics.beginFill(Theme.LOGINBOXHEADCOLOUR, Theme.LOGINBOXFILLALPHA);
			boxHeadBG.graphics.drawRect(0, 0, Theme.LOGINBOXWIDTH, Theme.LOGINBOXHEADHEIGHT);
			boxHeadBG.graphics.endFill();
			_loginBox.addChild(boxHeadBG);
			var boxBG:Shape = new Shape();
			boxBG.graphics.lineStyle();
			/* for plain colour BG */
			boxBG.graphics.beginFill(Theme.LOGINBOXFILLCOLOUR, Theme.LOGINBOXFILLALPHA);
			boxBG.graphics.drawRect(0, Theme.LOGINBOXHEADHEIGHT, Theme.LOGINBOXWIDTH, Theme.LOGINBOXHEIGHT-Theme.LOGINBOXHEADHEIGHT);
			boxBG.graphics.endFill();

			_loginBox.addChild(boxBG);
			var boxKeyline:Shape = new Shape();
			boxKeyline.graphics.lineStyle(Theme.LOGINBOXKEYLINESIZE, Theme.LOGINBOXKEYLINECOLOUR);
			boxKeyline.graphics.drawRect(0, 0, Theme.LOGINBOXWIDTH, Theme.LOGINBOXHEIGHT);
			_loginBox.addChild(boxKeyline);
			// add the fields;
			var labelText:TextField;
			var labelTextFormat:TextFormat;
			labelText = Format.SingleLineText("Please log in to begin -",Format.SubHeadingFormat());
			labelTextFormat = labelText.getTextFormat();
			labelTextFormat.color = Theme.LOGINBOXHEADTEXTCOLOUR;
			labelText.setTextFormat(labelTextFormat);
			labelText.x = 25;
			labelText.y = 12;
			_loginBox.addChild(labelText);
			labelText = Format.SingleLineText("User Name:",Format.LabelFieldFormat());
			labelTextFormat = labelText.getTextFormat();
			labelTextFormat.color = Theme.LOGINBOXTEXTCOLOUR;
			labelText.setTextFormat(labelTextFormat);
			labelText.x = 25;
			labelText.y = 60;
			_loginBox.addChild(labelText);
			_userName = Format.UserNameField(Format.InputFieldFormat());
			_userName.x = 95;
			_userName.y = 60;
			_loginBox.addChild(_userName);
			labelText = Format.SingleLineText("Password:",Format.LabelFieldFormat());
			labelTextFormat = labelText.getTextFormat();
			labelTextFormat.color = Theme.LOGINBOXTEXTCOLOUR;
			labelText.setTextFormat(labelTextFormat);
			labelText.x = 25;
			labelText.y = 100;
			_loginBox.addChild(labelText);
			_password = Format.PasswordField();
			_password.x = 95;
			_password.y = 100;
			_loginBox.addChild(_password);
			/* button */
			_loginButton = new Button();
			_loginButton.label = "Login";
			_loginButton.emphasized = true;
			_loginButton.addEventListener(MouseEvent.CLICK, OnLoginClicked);
			_loginButton.x = 95;
			_loginButton.y = 140;
			_loginBox.addChild(_loginButton);
			addChild(_loginBox);
		}

		private function OnLoginClicked(e:MouseEvent):void {
			if (_userName.text == "" || _password.text == "") {
				Alert.show({alerttitle:"Oops, there was a problem!", alertmessage:"Please enter both your User Name and Password correctly."});
			} else {
				// log user in
				EnableInput(false);
				var loginVars:String = Crypto.Encrypt(_userName + "|" + _password);
				var loginRequest:URLRequest = new URLRequest(_m.baseURL + Constant.LOGINURL);
				_loader = new URLLoader();
				_loader.dataFormat = URLLoaderDataFormat.VARIABLES;
				var variables:URLVariables = new URLVariables();
				variables.e = loginVars;
				loginRequest.data = variables;
				_loader.addEventListener(Event.COMPLETE, LoginComplete);
				_loader.addEventListener(IOErrorEvent.IO_ERROR, LoginIOError);
				_loader.addEventListener(SecurityErrorEvent.SECURITY_ERROR, LoginSecurityError);
				loginRequest.method = URLRequestMethod.GET;
				_loader.load(loginRequest);
			}
		}

		private function LoginSuccess(userdetails):void {
			trace("userdetails = " +userdetails);
			_m.UserID = userdetails[0];
			_p.removeChild(this);
			trace("what do i do now? call a continue function?")
		}

		private function LoginFailure(msg):void {
			Alert.show({alerttitle:"Oops, there was a problem!", alertmessage:msg});
			_password.text = "";
			_loader = null
			EnableInput();
		}

		private function EnableInput(tf:Boolean = true):void {
			_loginButton.enabled = tf;
			_password.selectable = tf;
			_userName.selectable = tf;
		}


		private function LoginComplete(e:Event) {
			trace(this + ": LoginComplete"+e)
			//var loader:URLLoader = URLLoader(e.target);
			var resultArray = _loader.data.r.split("|");
			if (resultArray[0].toLowerCase() == "error") {
				LoginFailure(resultArray[1]);
			} else {
				LoginSuccess(resultArray);
			}
		}

		private function LoginIOError(e:Event) {
			trace(this + ": LoginIOError:" + e);
			LoginFailure("There was a problem logging in.\n\nPlease try again at a later time.");
		}
		
		function LoginSecurityError(e:Event):void {
			trace(this + ": LoginSecurityError: " + e);
			LoginFailure("A security error has occured loading the data needed.\n\nPlease try again at a later time.")
		}

		/* ----------------------------------*/
		private function OnRemoved(e:Event) {
			removeEventListener(Event.REMOVED_FROM_STAGE,OnRemoved);
			_loginButton.removeEventListener(MouseEvent.CLICK, OnLoginClicked);
			_loader.removeEventListener(Event.COMPLETE, LoginComplete);
			_loader.removeEventListener(IOErrorEvent.IO_ERROR, LoginIOError);
			_loader.removeEventListener(SecurityErrorEvent.SECURITY_ERROR, LoginSecurityError);
		}

	}
}