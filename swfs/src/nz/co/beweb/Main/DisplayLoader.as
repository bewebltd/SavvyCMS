package {
	
	/*
	 * Loads a display item and returns it to a function in the calling object.
	 * @author Jonathan Brake
	 */
	 
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.IEventDispatcher;
	import flash.geom.Point;
	import flash.display.Loader;
	import flash.net.URLRequest;
	import flash.display.Bitmap;

	public class DisplayLoader extends MovieClip {
		
		private var _p:*;
		private var _loader:Loader;
		private var _fileToLoad:String;
		private var _startPos:Point;
		private var _titleText:String;
		private var _onComplete:Function;
		private var _display:Boolean;
		private var _animation:LoaderAnimation;

		public function DisplayLoader(fileToLoad:String, loaderTitle:String, startPos:Point, onComplete:Function, display:Boolean = true) {
			_fileToLoad = fileToLoad;
			_titleText=loaderTitle;
			_startPos=startPos;
			_onComplete = onComplete
			_display = display
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}
		
		function Init(e:Event):void {
			trace("Init: " + this + " | load: "+_fileToLoad);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE, Removed);
			_p=this.parent;
			var dataRequest:URLRequest=new URLRequest(_fileToLoad);
			_loader = new Loader();
			_loader.load(dataRequest);
			configureListeners(_loader.contentLoaderInfo)
			/* display */
			if(_display){
				_animation = new LoaderAnimation()
				_animation.loaderTitle.text=_titleText;
				_animation.loaderTitle.width=_animation.loaderTitle.textWidth+10;
				_animation.loaderTitle.x = (_animation.loaderTitle.width/2)*-1;
				addChild(_animation)
				_animation.x=_startPos.x;
				_animation.y=_startPos.y;
				_loader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS, OnProgress);
			}
		}
		function configureListeners(dispatcher:IEventDispatcher):void {
			dispatcher.addEventListener(Event.COMPLETE, OnComplete);
			dispatcher.addEventListener(IOErrorEvent.IO_ERROR, OnIOError);
			dispatcher.addEventListener(SecurityErrorEvent.SECURITY_ERROR, OnSecurityError);
		}
		
		function removeListeners(dispatcher:IEventDispatcher):void {
			dispatcher.removeEventListener(Event.COMPLETE, OnComplete);
			dispatcher.removeEventListener(IOErrorEvent.IO_ERROR, OnIOError);
			dispatcher.addEventListener(SecurityErrorEvent.SECURITY_ERROR, OnSecurityError);
		}
		
		function OnProgress(e:ProgressEvent):void {
			var percent:int=(e.bytesLoaded/e.bytesTotal)*100;
			_animation.percent.text=percent.toString();
		}

		function OnSecurityError(e:Event):void {
			trace("OnSecurityError: " + e);
			Alert.show( { alerttitle:"File Load Security Error", alertmessage:"A security error has occured loading the file '" + _fileToLoad + "'.\n\nPlease try again later.", callback:LoadFailure } )
		}
		
		
		private function OnIOError(e:IOErrorEvent):void {
			trace("ioErrorHandler: " + e);
			Alert.show( { alerttitle:"File Load IO Error", alertmessage:"An error has occured loading the file '" + _fileToLoad + "'.\n\nPlease try again later.", callback:LoadFailure } )
		}
		
		function OnComplete(e:Event):void {
			trace(this + ".OnComplete: "+e)
			var loadedData:* = e.target.content
			try {
				// this needs testing
				trace("typeof(loadedData) = "+typeof(loadedData))
				switch(typeof(loadedData)){
					case "bitmap": 
						loadedData.smoothing = true;
						break;
					default:
						//do nothing
				}
				_onComplete(loadedData);
			} catch (e:Error) {
				Alert.show({alerttitle:"Load Error", alertmessage:"An error has occurred in the loading '" + _fileToLoad + "'.  Please correct the error and load the data again.", callback:LoadFailure});
				return;
			}
			_p.removeChild(this);
		}
		
		function LoadFailure(e:String) {
			_p.removeChild(this);;
		}
		
		function Removed(e:Event):void{
			trace(this+".Removed: "+e)
			if(_display){
				removeChild(_animation)
				_loader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS, OnProgress);
			}
			removeListeners(_loader.contentLoaderInfo)
			removeEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}
	}

}