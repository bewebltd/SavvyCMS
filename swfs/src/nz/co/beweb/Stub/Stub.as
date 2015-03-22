package {
	/**
	 * ...
	 * @author Jonathan Brake
	 */

	import flash.display.MovieClip;
	import flash.display.LoaderInfo;
	import flash.events.Event;
	import flash.geom.Point;

	public class Stub extends MovieClip {

		public function Stub() {
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}

		public function Init(e:Event):void {
			trace("Init: " + this);
			removeEventListener(Event.ADDED_TO_STAGE,Init);
			addEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			var baseURL:String = LoaderInfo(stage.loaderInfo).parameters.baseURL;
			if (baseURL == null) {
				var urlPath:String = LoaderInfo(stage.loaderInfo).url;
				var urlPathArray:Array = urlPath.split("/");
				baseURL = urlPath.slice(0,urlPath.lastIndexOf(urlPathArray[urlPathArray.length - 1]));
			}
			var loader:MainLoader =new MainLoader( baseURL + "main.swf","Loading", new Point(stage.stageWidth/2, stage.stageHeight/2), MainLoaded);
			addChild(loader);
		}
		
		function MainLoaded(mc):void {
			addChild(mc);
		}

		function OnRemoved(e:Event):void {
			/* clean up any listners */
			removeEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
		}
	}

}