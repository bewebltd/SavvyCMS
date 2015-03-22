<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="sound.aspx.cs" Inherits="sound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<div style="background: red">
		<script type="text/javascript" src="js/swfobject.js"></script>
		<script type="text/javascript">						
			function getFlashMovieObject(movieName) {
				if (window.document[movieName]) {
					return window.document[movieName];
				}
				if (navigator.appName.indexOf("Microsoft Internet") == -1) {
					if (document.embeds && document.embeds[movieName])
						return document.embeds[movieName];
				}
				else { // if (navigator.appName.indexOf("Microsoft Internet")!=-1)  
					return document.getElementById(movieName); 
				}
			}
			
			function Play() {
				var playSound = getFlashMovieObject("soundmaker");
				playSound.playEventSound("../audio/beep1.mp3");
			}
		</script>
		<script type="text/javascript">
			swfobject.embedSWF("../swf/vox.swf", "soundmaker", "400", "400", "8.0.0", "../swf/expressInstall.swf", {}, { wmode: "transparent" }, {});
		</script>
		<div id="soundmaker"></div>
		<br />	
		<input type="button" value="Play" onclick="Play()" />
	</div>
</asp:Content>

