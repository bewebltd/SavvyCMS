<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="test35.aspx.cs" Inherits="Test35" %>
<html>
  <head>
    <title>Geocoding with GMap v3</title>
    
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    
		<script src="../js/jquery-1.4.2/jquery-1.4.2.js" type="text/javascript"></script>
		<script src="../js/jquery-ui-1.8.2/cool-blue-5/js/jquery-ui-1.8.2.custom.min.js"
			type="text/javascript"></script>
			<style>
			
			.ui-autocomplete {
				background-color: white;
				width: 300px;
				border: 1px solid #cfcfcf;
				list-style-type: none;
				padding-left: 0px;
			}
</style>
    <script type="text/javascript">
var geocoder;
var map;
var marker;

function initialize() {
  //MAP
	var latlng = new google.maps.LatLng(-36.84852838378392, 174.76222642988586);	 //sky tower
  var options = {
    zoom: 12,
    center: latlng,
    mapTypeId: google.maps.MapTypeId.SATELLITE
  };

  map = new google.maps.Map(document.getElementById("map_canvas"), options);

  //GEOCODER
  geocoder = new google.maps.Geocoder();

  marker = new google.maps.Marker({
    map: map,
    draggable: true
  });

	google.maps.event.addListener(marker, 'drag', function() {
			geocoder.geocode({'latLng': marker.getPosition()}, function(results, status) {
				if (status == google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						$('#address').val(results[0].formatted_address);
						$('#latitude').val(marker.getPosition().lat());
						$('#longitude').val(marker.getPosition().lng());
					}
				}
			});
		});
}
$(document).ready(function() { 
         
  initialize();
                  
  $(function() {
    $("#address").autocomplete({
      //This bit uses the geocoder to fetch address values
      source: function(request, response) {
        geocoder.geocode( {'address': request.term+', New Zealand' }, function(results, status) {
          response($.map(results, function(item) {
            return {
              label:  item.formatted_address,
              value: item.formatted_address,
              latitude: item.geometry.location.lat(),
              longitude: item.geometry.location.lng()
            }
          }));
        })
      },
      //This bit is executed upon selection of an address
      select: function(event, ui) {
        $("#latitude").val(ui.item.latitude);
        $("#longitude").val(ui.item.longitude);
        var location = new google.maps.LatLng(ui.item.latitude, ui.item.longitude);
        marker.setPosition(location);
        map.setCenter(location);
      }
    });
  });
	//$('#mapcanvas').find('Map Data');
});
		</script>
  </head>
  <body>
    <label>Address: </label><input id="address"  type="text"/>
    <div id="map_canvas" style="width:300px; height:300px"></div><br/>
    <label>latitude: </label><input id="latitude" type="text"/><br/>
    <label>longitude: </label><input id="longitude" type="text"/>
  </body>
</html>