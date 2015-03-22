<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.HomeController.ViewModel>" %>
<%@ Import Namespace="Models" %>

	<style>
		/* Move this into the site.css if using the carousel */
		/* Slider */
		#image-slide{ width:960px; height:243px; position:relative; z-index: 0; margin: -10px -10px 0 -10px; }
		#image-slide .slide{ display:none; position:absolute; top:0px; left:0px; }
		#image-slide .photo-credit{ background: url('images/photo-credit-gray-bg.png') 0 0; padding: 4px 4px; display: block; position: absolute; right: 12px; bottom: 16px; z-index: 200; color: #fff; }
	</style>

	<%Util.IncludeJavascriptFile("~/js/dumbcrossfade/jquery.dumbcrossfade-2.0.min.js"); %>
	<script type="text/javascript">
		$(document).ready(function () {
			var options = {
				'slideType': 'fade',
				'doHoverPause': false,
				'showTime': 2000,
				'transitionTime': 2000
			};
			$('#image-slide .slide').dumbCrossFade(options);
			EvenUpHeights($('.matchHeight'));
			EvenUpHeights($('.info .text'));
		})
	</script>
	
	<div id="image-slide">
		<%foreach(var slide in Model.Slides){ %>
			<div class="slide">
				<img src="<%=ImageProcessing.ImagePath(slide.SlidePicture) %>" width="960" height="243" alt="<%=slide.AltText %>" />
				<div class="photo-credit">Photo: <%=slide.AltText %></div>
			</div>
		<%} %>
	</div>