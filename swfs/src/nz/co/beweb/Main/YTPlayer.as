package {
	
	/**
	 * ...
	 * @author Jonathan Brake
	 * based on google src at http://code.google.com/apis/youtube/flash_api_reference.html
	 */
	 
	import flash.display.Loader;
	import flash.display.MovieClip;
	import flash.events.FullScreenEvent;
	import flash.events.Event;
	import flash.geom.Point;
	import flash.net.URLRequest;


	public class YTPlayer extends MovieClip {

		private var _YTLoader:Loader;
		private var _YTPlayer:*;
		private var _YTPlayerReady:Boolean = false;
		private var _myVideo:String;
		private var _wrapper:MovieClip;
		private var _YTURL:String = "http://www.youtube.com/v/";
		private var _YTURLAdditionalParams:String = "?fs=1&amp;autoplay=0&amp;version=3&amp;rel=0";
		private var _playerOrigin:Point = new Point(0, 0);
		private var _playerWidth:Number = 640;
		private var _playerHeight:Number = 385;


		public function YTPlayer(video:String):void {
			addEventListener(Event.ADDED_TO_STAGE, Init);
			_myVideo = video;
		}

		private function Init(e:Event):void {
			//trace("Init: "+ this)
			removeEventListener(Event.ADDED_TO_STAGE,Init);
			addEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			_wrapper = new MovieClip();
			addChild(_wrapper);
			CreatePlayer();
		}

		private function CreatePlayer():void {
			// set the shitsAndGigglesLoader to the correct size - loads nothing just for user feedback
			var url:String = _YTURL + _myVideo + _YTURLAdditionalParams;
			_YTLoader = new Loader();
			_YTLoader.contentLoaderInfo.addEventListener(Event.INIT, LoadClipInit);
			_YTLoader.load(new URLRequest(url));
			stage.addEventListener(FullScreenEvent.FULL_SCREEN, FullScreen);
		}


		private function FullScreen(e:FullScreenEvent) {
			//trace("FullScreen:" + e)
			if (e.fullScreen) {
				_YTPlayer.setSize(stage.stageWidth, stage.stageHeight);
				_YTLoader.x = 0;
				_YTLoader.y = 0;
			} else {
				_YTPlayer.setSize(_playerWidth, _playerHeight);
				_YTLoader.x = _playerOrigin.x;
				_YTLoader.y = _playerOrigin.y;
			}
		}

		private function LoadClipInit(e:Event):void {
			//trace("LoadClipInit:" + e)
			_YTLoader.contentLoaderInfo.removeEventListener(Event.INIT, LoadClipInit);
			_YTLoader.x = _playerOrigin.x;
			_YTLoader.y = _playerOrigin.y;
			_wrapper.addChild(_YTLoader);
			_YTLoader.content.addEventListener("onReady", PlayerReady);
			_YTLoader.content.addEventListener("onError", PlayerError);
			_YTLoader.content.addEventListener("onStateChange", PlayerStateChange);
			_YTLoader.content.addEventListener("onPlaybackQualityChange", VideoPlaybackQualityChange);
			_YTLoader.content.addEventListener(Event.REMOVED_FROM_STAGE, PlayerRemoved);
		}

		private function PlayerRemoved(e:Event) {
			//trace("PlayerRemoved:" + e)
			var item:* = e.currentTarget;
			item.removeEventListener("onReady", PlayerReady);
			item.removeEventListener("onError", PlayerError);
			item.removeEventListener("onStateChange", PlayerStateChange);
			item.removeEventListener("onPlaybackQualityChange", VideoPlaybackQualityChange);
			item.removeEventListener(Event.REMOVED_FROM_STAGE, PlayerRemoved);

		}

		private function PlayerReady(e:Event):void {
			//trace("PlayerReady:" + e)
			// Event.data contains the event parameter, which is the Player API ID 
			//trace("player ready:"+ Object(e).data);
			_YTPlayerReady = true;
			// Once this event has been dispatched by the player, we can use
			// cueVideoById, loadVideoById, cueVideoByUrl and loadVideoByUrl
			// to load a particular YouTube video.
			_YTPlayer = _YTLoader.content;
			// Set appropriate player dimensions for your application
			_YTPlayer.setSize(_playerWidth, _playerHeight);
			_YTPlayerReady = true;
		}

		private function PlayerError(e:Event):void {
			// Event.data contains the event parameter, which is the error code
			trace("PlayerError:"+ Object(e).data);
		}

		private function PlayerStateChange(e:Event):void {
			// Event.data contains the event parameter, which is the new player state
			trace("PlayerStateChange:" + e);
			var playerstate = Object(e).data;
			switch (playerstate) {
				case -1 :
					// unstarted
					break;
				case 0 :
					// ended
					break;
				case 1 :
					// playing
					break;
				case 2 :
					// paused
					break;
				case 3 :
					// buffering
					break;
				default :
					trace("The player state (" + playerstate + ") is invalid.");
					break;
			}
		}

		private function VideoPlaybackQualityChange(e:Event):void {
			// Event.data contains the event parameter, which is the new video quality
			trace("VideoPlaybackQualityChange:"+ e);
			var quality = Object(e).data
		}

		private function OnRemoved(e:Event):void {
			/* clean up any listners */
			removeEventListener(Event.REMOVED_FROM_STAGE, OnRemoved);
			stage.removeEventListener(FullScreenEvent.FULL_SCREEN, FullScreen);
			if (_YTLoader.contentLoaderInfo.hasEventListener(Event.INIT)) {
				_YTLoader.contentLoaderInfo.removeEventListener(Event.INIT, LoadClipInit);
			}
			if (_YTPlayerReady) {
				_YTPlayer.destroy();
			}
			_YTLoader = null;
		}

	}

}