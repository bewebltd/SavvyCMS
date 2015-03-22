package {
	
	/*
	 *
	 * Main Class of your flash application.
	 * @author Jonathan Brake (jonathan@beweb.co.nz)
	 *
	 */

	import flash.display.MovieClip;
	import flash.display.LoaderInfo;
	import flash.events.Event;
	import com.google.analytics.AnalyticsTracker; 
	import com.google.analytics.GATracker;
	import flash.events.TimerEvent;
	import flash.utils.Timer;

	public class Main extends MovieClip {

		private var _isDev:Boolean = true;
		private var _io:InputOutput;
		private var _baseURL:String;
		private var _userID:String;
		private var _tracker:AnalyticsTracker;

		public function Main():void {
			trace("\/\/\/\/\/\/ START \/\/\/\/\/\/")
			addEventListener(Event.ADDED_TO_STAGE,Init);
		}

		private function Init(e:Event):void {
			trace("Init: "+ this);
			removeEventListener(Event.ADDED_TO_STAGE,Init);
			addEventListener(Event.REMOVED_FROM_STAGE, Removed);
			Alert.init(stage);
			_io = new InputOutput();
			addChild(_io);	
			var location:String = LoaderInfo(stage.loaderInfo).parameters.baseURL;
			if (Validate.IsNullOrEmpty(location)) {
				location = root.loaderInfo.url
			}
			
			var locationArray:Array = location.split("/");
			_baseURL = location.slice(0, location.lastIndexOf(locationArray[locationArray.length - 1]));
			if (Constant.GOOGLEANALYTICSUANUMBER != "") {
				// set up google analytics
				_tracker = new GATracker(this, Constant.GOOGLEANALYTICSUANUMBER, Constant.GOOGLEANALYTICSTRACKINGMODE, Constant.GOOGLEANALYTICSDEBUG);
			}
			//addChild(new LoginBox())
			
			var calendar = new Calendar();
			addChild(calendar)
			
			var controlpanel = new ControlPanel("Properties");
			controlpanel.x = 250
			controlpanel.y = 30
			addChild(controlpanel)
			
		}
		
		public function Track(url):void {
			trace(this + ".Track: " + url + ")")
			_tracker.trackPageview(url);
		}
		
		/* Getters / Setters */
		public function get IsDev():Boolean {
			return _isDev;
		}

		public function get IO():InputOutput {
			return _io;
		}

		public function get UserID():String {
			return _userID;
		}
		
		public function set UserID(userID:String):void {
			_userID = userID;
		}
		
		/* End */
		private function Removed(e:Event) {
			/* clean up any listners */
			trace(this + ".Removed: "+e)
			removeEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}

	}

}