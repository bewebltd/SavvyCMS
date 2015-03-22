using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beweb;

namespace Savvy {
	public class YouTube {
		protected string videoUrl;
		public string VideoCode;

		public YouTube(string youTubeVideoUrl) {
			videoUrl = youTubeVideoUrl;
			VideoCode = GetVideoCode(videoUrl);
		}

		public static string GetVideoCode(string videoUrl) {
			string code = "";
			if (videoUrl.IsNotBlank()) {
				videoUrl = videoUrl.Trim();
				// eg http://www.youtube.com/watch?v=JXiFsB4SYlc&feature=popular
				// or http://www.youtube.com/watch?v=JXiFsB4SYlc
				// or http://youtu.be/JXiFsB4SYlc
				if (videoUrl.Contains("youtu.be/")) {
					code = videoUrl.RightFrom("youtu.be/");
				} else if (videoUrl.Contains("youtube.com/watch?v=")) {
					code = videoUrl.RightFrom("youtube.com/watch?v=");
				} else if (videoUrl.Contains("youtube.com/embed/")) {
					code = videoUrl.RightFrom("youtube.com/embed/");
				}
				if (code.Contains("&")) {
					code = code.Split('&')[0];
				}
				if (videoUrl.Length == 11) {
				  // already a code - codes must be 11 characters 
				  code = videoUrl;
				}
				//if (code.Length != 11) {
				//  // codes must be 11 characters 
				//  code = ""; // code is invalid
				//}
			}
			return code;
		}

		public string EmbedUrl {
			get {
				if (VideoCode.IsBlank()) return null;
				return "http://www.youtube.com/embed/" + VideoCode;
			}
		}

		public string GetEmbedHtml() {
			return GetEmbedHtml(320, 240);
		}

		public string GetEmbedHtml(int width, int height) {
			if (VideoCode.IsBlank()) return null;
			string html = "<iframe class=\"YouTubeVideo\" width=\""+width+"\" height=\""+height+"\" src=\"" + EmbedUrl + "?wmode=transparent&rel=0\" frameborder=\"0\" allowTransparency=\"true\" allowfullscreen></iframe>";
			return html;
		}

		public static string GetEmbedHtml(string youTubeVideoUrl, int width, int height) {
			var vid = new YouTube(youTubeVideoUrl);
			return vid.GetEmbedHtml(width, height);

			//string html;
			//if (youTubeVideoCode.IsNotBlank()) {
			//  string videoCode = youTubeVideoCode;
			//  html = "<iframe width=\""+width+"\" height=\""+height+"\" src=\"http://www.youtube.com/embed/"+videoCode+"\" frameborder=\"0\" allowfullscreen></iframe>";
			//}
			//return html;
		}

		
	}
}
