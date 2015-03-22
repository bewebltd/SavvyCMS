<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.HomeController.SlideShowViewModel>" %>
<%@ Import Namespace="Models" %>

	<style>
		/* Move this into the site.css if using the carousel */ 
		.carousel-wrapper { width: 979px; height: 500px; clear: both; overflow: hidden; position: relative; }
		.carousel-wrapper .slide{ position: absolute;  }
		.carousel-wrapper .slide.active{ display: block; z-index: 99; }

		.carousel-wrapper .moveLeft,.lookbook-wrapper .moveRight{ width: 60px; height:100%; position: absolute; top: 0; z-index: 0; cursor: pointer; }
		.carousel-wrapper .moveLeft:hover,.lookbook-wrapper .moveRight:hover{  background-color: rgba(0,0,0,.05); }
		.carousel-wrapper .moveLeft{ left: 0; margin-left: -60px }
		.carousel-wrapper .moveRight{ right: -60px;  }
		.carousel-wrapper .moveLeft .arrow,.lookbook-wrapper .moveRight .arrow{ width: 16px; height: 23px; margin: 0 auto; margin-top: 333px; }
		.carousel-wrapper .moveLeft .arrow{ background: url('images/carousel_arrows.png') 0 0 no-repeat; }
		.carousel-wrapper .moveRight .arrow{ background: url('images/carousel_arrows.png') -16px 0 no-repeat; }

		.carousel-tabs .slideShow-tab { clear:both; padding: 20px; left: auto; position: relative; }
		.carousel-tabs .slideShow-tab a { display: inline-block;  margin-left: 60px; border: 1px solid #ACA7A2; left: 50%; top: 50%; width: 12px; height: 12px; line-height: 12px; margin: -6px 0 0 -6px; z-index: 1; border-radius: 26px; -moz-border-radius: 26px; -webkit-border-radius: 26px; position: relative; }
		.carousel-tabs .slideShow-tab a:hover { background: #ddd; }
		.carousel-tabs .slideShow-tab a.active { background: #ACA7A2; }

		#image-slide .photo-credit{ background: rgba(0,0,0,0.5); padding: 4px 4px; display: block; position: absolute; right: 12px; bottom: 16px; z-index: 200; color: #fff; }
	</style>

	<%Util.IncludeJavascriptFile("~/js/BewebCore/carousel.min.js"); %>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#image-slide').svyCarousel({ effect: 'slideParallax', autoSlide: true, animateBeginning: true });
		})
	</script>

	<div class="carousel-wrapper">
		<a class="moveLeft slideShow-prev">
			<div class="arrow"></div>
		</a>		
		<div id="image-slide">
			<%foreach(var slide in Model.Slides){ %>
				<div class="slide">
					<img src="<%=ImageProcessing.ImagePath(slide.SlidePicture) %>" width="960" height="243" alt="<%=slide.AltText %>" />
					<div class="photo-credit">Photo: <%=slide.AltText %></div>
				</div>
			<%} %>
		</div>
		<a class="moveRight slideShow-next">
			<div class="arrow"></div>
		</a>
		<div class="clear"></div>
	</div>
	<div class="carousel-tabs">
		<div class="slideShow-tab">
			<%foreach(var slide in Model.Slides){ %>
				<a href=""><%=Model.Slides.LoopIndex+1%></a>
			<%} %>
		</div>
		<div class="clear"></div>
	</div>

