package {
	
	/*
	 * Loads data and returns it to a function in the calling object.
	 * @author Jonathan Brake
	 */
	 
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.events.SecurityErrorEvent;
	import flash.events.ProgressEvent;
	import flash.geom.Point;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.net.URLLoaderDataFormat;
	import flash.net.URLVariables;
	import flash.net.URLRequestMethod;

	public class DataLoader extends MovieClip {
		
		private var _p:*;		
		private var _r:*;
		private var _loader:URLLoader;
		private var _fileToLoad:String;
		private var _startPos:Point;
		private var _titleText:String;
		private var _onComplete:Function;
		private var _display:Boolean;
		private var _animation:LoaderAnimation;
		private var _dataFormat:String;

		public function DataLoader(fileToLoad:String, loaderTitle:String, startPos:Point, onComplete:Function, dataFormat:String = URLLoaderDataFormat.TEXT, display:Boolean = true) {
			_fileToLoad = fileToLoad;
			_titleText = loaderTitle;
			_startPos = startPos;
			_onComplete = onComplete
			_dataFormat = dataFormat
			_display = display
			addEventListener(Event.ADDED_TO_STAGE, Init);
		}
		
		function Init(e:Event):void {
			trace("Init: " + this + " | load: "+_fileToLoad);
			removeEventListener(Event.ADDED_TO_STAGE, Init);
			addEventListener(Event.REMOVED_FROM_STAGE, Removed);
			_p = this.parent;
			_r = root
			var dataRequest:URLRequest=new URLRequest(_fileToLoad);
			dataRequest.method = URLRequestMethod.POST;
			var variables:URLVariables = new URLVariables();
			// add any variables for this request
			variables.r = DateTime.Now();
			dataRequest.data = variables;
			_loader = new URLLoader();
			_loader.dataFormat=_dataFormat;
			_loader.load(dataRequest);
			_loader.addEventListener(Event.COMPLETE, OnComplete);
			_loader.addEventListener(IOErrorEvent.IO_ERROR, OnIOError);
			_loader.addEventListener(SecurityErrorEvent.SECURITY_ERROR, OnSecurityError);
			/* display */
			if(_display){
				_animation = new LoaderAnimation()
				_animation.loaderTitle.text=_titleText;
				_animation.loaderTitle.width=_animation.loaderTitle.textWidth+10;
				_animation.loaderTitle.x = (_animation.loaderTitle.width/2)*-1;
				addChild(_animation)
				this.x=_startPos.x;
				this.y=_startPos.y;
				_loader.addEventListener(ProgressEvent.PROGRESS, OnProgress);
			}
		}

		function OnProgress(e:ProgressEvent):void {
			var percent:int=(e.bytesLoaded/e.bytesTotal)*100;
			_animation.percent.text = percent.toString();
		}

		function OnSecurityError(e:Event):void {
			trace(this + ".OnSecurityError: " + e);
			Alert.show( { alerttitle:"Data Load Security Error", alertmessage:"A security error has occured loading the data needed.\n\nPlease try again later.", callback:LoadFailure } )
		}
		
		private function OnIOError(e:IOErrorEvent):void {
			trace(this + ".OnIOError: " + e);
			Alert.show( { alerttitle:"Data Load IO Error", alertmessage:"An error has occured loading the data needed.\n\nPlease try again later.", callback:LoadFailure } )
		}

		function OnComplete(e:Event):void {
			//trace(this + ".OnComplete: "+e)
			var loadedData:*;
			if (_r.IsDev) {
				loadedData = ExtractData(e.target.data)
				_onComplete(loadedData);
			}else{
				try {
					loadedData = ExtractData(e.target.data)
					_onComplete(loadedData);
				} catch (e:Error) {
					Alert.show({alerttitle:"Data Error", alertmessage:"An error has occurred in the loaded data.  Please correct the error and load the data again.", callback:LoadFailure});
					return;
				}
		
		}
			_p.removeChild(this);
		}
		
		function ExtractData(data:*):* {
			var dataType = "string"
			if (Format.Trim(data).indexOf('<?xml') == 0 ) {
				dataType = "xml"
			}
			switch(dataType){
				case "xml":
					data=new XML(data);
					data.ignoreWhitespace=true;
					break;
				default:
					//do nothing
			}
			return data
		}
		
		function LoadFailure(e:String) {
			_p.removeChild(this);;
		}
		
		function Removed(e:Event):void{
			//trace(this + ".Removed: "+ e)
			if(_display){
				removeChild(_animation)
				_loader.addEventListener(ProgressEvent.PROGRESS, OnProgress);
			}
			_loader.removeEventListener(Event.COMPLETE, OnComplete);
			_loader.removeEventListener(IOErrorEvent.IO_ERROR, OnIOError);
			_loader.removeEventListener(SecurityErrorEvent.SECURITY_ERROR, OnSecurityError);
			removeEventListener(Event.REMOVED_FROM_STAGE, Removed);
		}
		
	}

}