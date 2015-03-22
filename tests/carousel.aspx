<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="carousel.aspx.cs" Inherits="carousel" %>
<html>
<head>
	<script src="http://code.jquery.com/jquery-1.10.1.min.js"></script>
	<link href="<%=Web.Root %>site.css" rel="stylesheet" type="text/css" />
	<%Util.IncludeJavascriptFile("~/js/BewebCore/carousel.js"); %>
		
	<style>
		.carousel-wrapper .carousel{ width: 630px; height: 502px; clear: both; overflow: hidden; position: relative; margin: 50px auto; }
		.carousel-wrapper .carousel .slide{ position: absolute; }
		.carousel-wrapper .carousel .slide.active{ display: block; z-index: 99; }
		.carousel-wrapper .carousel-tab a{ color: black; }
		.carousel-wrapper .carousel-tab a.active{ font-weight: bold; }
	</style>

	<script>
		$(window).load(function() {
			$('.carousel').svyCarousel({ effect: 'fade', delay: 3000 });
		});
	</script>

</head>
<body>
		
	<div class="carousel-wrapper">
		
		<ul class="carousel">
				<li class="slide"><img src="http://upload.wikimedia.org/wikipedia/commons/4/46/Greenland_scenery.jpg" width="630" height="502" /></li>
				<li class="slide"><img src="http://www.desktopas.com/files/2013/06/Waterfall-Scenery-Wallpaper-1680x1050.jpg" width="630" height="502" /></li>
				<li class="slide"><img src="http://www.deshow.net/d/file/travel/2009-10/new-zealand-scenery-738-20.jpg" width="630" height="502" /></li>
				<li class="slide"><img src="http://2.bp.blogspot.com/-blvwA23LIc0/TdrEMYEnbQI/AAAAAAAAAaY/wvbpEBN8dA4/s1600/wallpapersmantra+%252811%2529.jpg" width="630" height="502" /></li>
		</ul>
		
		<div class="carousel-tab">
			<a href="">1</a>
			<a href="">2</a>
			<a href="">3</a>
			<a href="">4</a>
		</div>

		<div class="clear"></div>
	</div>

</body>
</html>

