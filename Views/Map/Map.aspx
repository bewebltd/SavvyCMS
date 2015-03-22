<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.MapController.ViewModel>"  %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<!DOCTYPE html>
<html>
	<head>
		<meta http-equiv="content-type" content="text/html; charset=utf-8" />
		<title>MarkerClusterer v3 Example</title>

		<style type="text/css">
			@font-face {
				font-family: 'PFSquareSansProRegular';
				src: url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-regular-webfont.eot');
				src: url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-regular-webfont.eot?#iefix') format('embedded-opentype'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-regular-webfont.woff') format('woff'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-regular-webfont.ttf') format('truetype'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-regular-webfont.svg#PFSquareSansProRegular') format('svg');
				font-weight: normal;
				font-style: normal;
			}
			@font-face {
				font-family: 'PFSquareSansProBold';
				src: url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-bold-webfont.eot');
				src: url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-bold-webfont.eot?#iefix') format('embedded-opentype'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-bold-webfont.woff') format('woff'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-bold-webfont.ttf') format('truetype'),
						 url('<%=Web.BaseUrl%>fonts/pfsquaresanspro-bold-webfont.svg#PFSquareSansProBold') format('svg');
				font-weight: normal;
				font-style: normal;
			}

			#smoe-wrapper h2{ font-family: PFSquareSansProRegular; font-size: 24px; margin: 0 0 13px 0; color: #000; }
			#smoe-wrapper p{  margin: 0; font-size: 12px; line-height: 18px; }
			#smoe-wrapper strong{ margin: 0; font-size: 14px; line-height: 20px;  }
			#smoe-wrapper a{ color: #000; text-decoration: none;  }
			#smoe-wrapper img{ border: none; }

			#smoe-wrapper { background: #e6f8ff url('<%=Web.BaseUrl%>images/smoe_masthead.jpg') 0 0 no-repeat; margin: 0 auto; font-family: Arial; font-size: 16px; width: 960px; padding: 236px 0 30px 0;  }

			#left-col{ width: 330px; float: left; background: url('<%=Web.BaseUrl%>images/col-gradient.png') 0 40px repeat-x; }
			#left-col .status{ width: 278px; background: url('<%=Web.BaseUrl%>images/status-bg.png'); color: #000; padding: 16px 15px 15px 17px; margin-bottom: 11px; }
			#left-col .status h2{ font-size: 22px; font-weight: bold; margin-bottom: 4px; font-family: PFSquareSansProBold; }
			#left-col .status strong{ padding-top: 3px; display: block; }
			#left-col .news{ width: 300px; float: left; padding-left: 17px; }
			#left-col .news strong{ color: #00A9E0; }
			#left-col .news a{ color: #00A9E0; }
			#left-col .fb-link{ margin: 0 0 16px 18px; display: block; }

			#right-col{ width: 630px; float: left; padding-top: 40px; }
			#region-container{ background: #e6f8ff; padding: 10px 20px 15px 20px; }
			#region-links a{ line-height: 20px; display: block; }
			#region-links .region-col{ float: left; width: 140px; }
			#region-container .legend{ border-top: 2px solid #39bde8; margin-top: 16px; padding-top: 14px; }
			#region-container .legend-item{ width: 172px; float: left; font-size: 14px; }
			#region-container .legend-item img{ float: left; padding-right: 12px; }
			#region-container a{ color: #000; text-decoration: none; font-size: 14px; }
			#map-container { }
			#map { width: 630px; height: 670px; }

			.map_bubble{ width: 242px; } 
			.head_public, .head_school{ border-bottom: 1px solid #999; padding-bottom: 8px; margin-bottom: 9px; font-size: 14px; line-height: 18px; }
			.head_public small{ font-size: 12px; }
			.head_public .marker{ background: url('<%=Web.BaseUrl%>images/smaller-marker.png') -14px 0 no-repeat; width: 14px; height: 21px; float: left; margin-right: 10px; }
			.head_school .marker{ background: url('<%=Web.BaseUrl%>images/smaller-marker.png') 0 0 no-repeat; width: 14px; height: 21px; float: left; margin-right: 10px; }
			.viewInfo{ text-align: center;  }
			.viewInfo a{ background: url('<%=Web.BaseUrl%>images/view-details-btn.png'); width: 138px; display: inline-block; height: 32px; margin-top: 8px; }
			.bubble_title{ font-size: 16px; margin-bottom: 3px; }
			.bubble_text{ font-size: 12px; margin-bottom: 3px; }

			.popup{ width: 460px; padding:  3px 20px; }
			.popup .head_public, .popup .head_school{ padding-bottom: 15px; margin-bottom: 19px; }

			.clear{ clear: both; }
		</style>

		<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
		<script src="http://www.google.com/jsapi"></script>
		<!--<script type="text/javascript" src="http://google-maps-utility-library-v3.googlecode.com/svn/tags/markerclusterer/1.0/src/data.json"></script>-->
		<script type="text/javascript" src="<%=Web.BaseUrl%>Map/GetData"></script>
		<script type="text/javascript" src="http://google-maps-utility-library-v3.googlecode.com/svn/tags/markerclusterer/1.0/src/markerclusterer.js"></script>
		<script src="<%=Web.BaseUrl%>js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
		<link href="<%=Web.BaseUrl%>js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />

		<script type="text/javascript">
			google.load('maps', '3', {
				other_params: 'sensor=false'
			});
			google.setOnLoadCallback(initialize);

			// globals
			var map; 
			var markers = [];

			function initialize() {
				var center = new google.maps.LatLng(-40.88054143186031, 172.3203125);

				map = new google.maps.Map(document.getElementById('map'), {
					zoom: 5,
					center: center,
					mapTypeId: google.maps.MapTypeId.ROADMAP
				});

				// marker icons
				var schoolMarker = new google.maps.MarkerImage("<%=Web.BaseUrl %>images/orange-marker.png");
				var publicMarker = new google.maps.MarkerImage("<%=Web.BaseUrl %>images/blue-marker.png");
				var markerShadow = new google.maps.MarkerImage("<%=Web.BaseUrl %>images/google-shadow-marker.png",
													 new google.maps.Size(37, 26),
													 new google.maps.Point(0, 0),
													 new google.maps.Point(5, 21));

				// markers
				for (var i = 0, dataPhoto; dataPhoto = data.locations[i]; i++) {
					var latLng = new google.maps.LatLng(dataPhoto.latitude, dataPhoto.longitude);
					var marker = new google.maps.Marker({
						position: latLng,
						title: dataPhoto.hoverText, 
						shadow: markerShadow
					}); //, shadow: markerShadow, icon: markerImage
					if (dataPhoto.latitude == 0 || dataPhoto.longitude == 0) {
						var geocoder = new google.maps.Geocoder();
						geocoder.geocode({ 'address': dataPhoto.address }, function (results, status) {
							if (status == google.maps.GeocoderStatus.OK) {
								marker.setPosition(results[0].geometry.location);
							}
						});
					}
					var contentString = '<div class="map_bubble">';
					if (dataPhoto.eventType == "School") {
						marker.icon = schoolMarker;
						contentString += '<div class="head_school"><span class="marker"></span>School Event. <small>For school students.</small></div>';
					} else {
						marker.icon = publicMarker;
						contentString += '<div class="head_public"><span class="marker"></span>Public Event. <small>Open to everyone.</small></div>';
					}
					contentString += '<div class="bubble_title">' + dataPhoto.title + '</div>';
					contentString += '<div class="bubble_text">' + dataPhoto.location + '</div>';
					contentString += '<div class="bubble_text">' + dataPhoto.address + '</div>';
					contentString += '<div class="bubble_text">' + dataPhoto.dates + '</div>';
					if (dataPhoto.startTime + "" != "") {
						contentString += '<div class="bubble_text">Start Time: ' + dataPhoto.startTime + '</div>';
					}
					contentString += '<div class="viewInfo"><a href="#" onclick="ShowInfoPopup(' + i + ');return false;"></a></div>';
					contentString += '</div>';
 
					marker.info = new google.maps.InfoWindow({ content: contentString });
					google.maps.event.addListener(marker, 'click', function () {
						
						this.info.open(map, this);
					});

					// save rgion
					marker.regionID = dataPhoto.regionID;

					markers.push(marker);
				}

				var markerCluster = new MarkerClusterer(map, markers);

				// set up region links
				var html = "<div class='region-col'>";
				for (var i = 0, region; region = data.regions[i]; i++) {
					if (i % 4 == 0 && i != 0) {
						html += "</div><div class='region-col'>";
					}
					html += '<a href="#" onclick="ZoomRegion(' + region.regionID + ');return false;">' + region.title + '</a>';
				}
				html += "</div>";
			$("#region-links").prepend(html);

			}

			function ShowInfoPopup(markerNum) {

				var html = '<div class="popup">';
				if (data.locations[markerNum].eventType == "School") {
					html += '<div class="head_school"><span class="marker"></span>School Event. <small>For school students.</small></div>';
				} else {
					html += '<div class="head_public"><span class="marker"></span>Public Event. <small>Open to everyone.</small></div>';
				}
				html += '<div class="popup_title">' + data.locations[markerNum].html + '</div>';
				html += '</div>';

				$.colorbox({ html: html });
			}

			function ZoomRegion(regionID) {
				var wideMapBounds = new google.maps.LatLngBounds();
				for (var i in markers) {
					if (markers[i].regionID == regionID) {
						wideMapBounds.extend(markers[i].getPosition());
					}
				}
				map.fitBounds(wideMapBounds);
				if (map.getZoom() > 15){ // Change max/min zoom here
					map.setZoom(15);
				}
				return false;
			}

		</script>
	</head>
	<body>
		<div id="smoe-wrapper">
			<div id="left-col">
				<div class="status">
					<h2>LATEST STATUS</h2>
					<strong>DAY 1</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.</p>

					<strong>DAY 2 - NOTE MAY GO OVER MULTIPLE LINES</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.</p>

					<strong>DAY 3</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.</p>
				</div>
				
				<a href="" class="fb-link"><img src="<%=Web.BaseUrl %>images/fb-link-btn.png" width="156" height="27" /></a>

				<div class="news">
					<h2>NEWS</h2>
					<strong>Heading here</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor. Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.
						<br/><a href="">Read more...</a>
					</p><br />

					<strong>Heading here</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.
						<br/><a href="">Read more...</a>
					</p><br />

					<strong>Heading here</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor. Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.
						<br/><a href="">Read more...</a>
					</p><br />		
								
					<strong>Heading here</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor. Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.
						<br/><a href="">Read more...</a>
					</p><br />	
									
					<strong>Heading here</strong>
					<p>Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor. Duis aliquet egestas purus in blandit. Curabitur vulputate, ligula lacinia scelerisque tempor.
						<br/><a href="">Read more...</a>
					</p><br />
				</div>
			</div>

			<div id="right-col">
				<div id="region-container">
					<div id="region-links">
						
						<div class="clear"></div>
					</div>
					<div class="legend">
						<div class="legend-item">
							<img src="<%=Web.BaseUrl %>images/orange-marker.png" width="19" height="29" />
							School Events<br /> <span style="font-size: 12px;">For school students</span>
						</div>						
						<div class="legend-item">
							<img src="<%=Web.BaseUrl %>images/blue-marker.png" width="19" height="29" />
							Public Events<br /> <span style="font-size: 12px;">Open to everyone</span>
						</div>
						<div class="clear"></div>
					</div>
				</div>

				<div id="map-container">
					<div id="map"></div>
				</div>
			</div>
			
			<div class="clear"></div>
		</div>
	</body>
</html>
