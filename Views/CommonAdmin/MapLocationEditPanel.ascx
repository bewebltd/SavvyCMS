<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Areas.Admin.Controllers.CommonAdminController.MapLocationViewData>" %>

	<script src="http://maps.googleapis.com/maps/api/js?sensor=false&libraries=places" type="text/javascript"></script> 
	<style type="text/css"> 
		#map_canvas {
			height: 400px;
			width: 600px;
			margin-top: 0.6em;
		}
	</style> 
 
	<script type="text/javascript">
		var map, marker, infowindow;
		var locAddressNames = "<%=Model.AddressFieldName%>";
		var locAddressNameArray = locAddressNames.split('|');
		var latFieldName = "<%=Model.LatitudeFieldName%>";
		var longFieldName = "<%=Model.LongitudeFieldName%>";

		function initialize() {
			// check we have a field to go off - we need one
			if (locAddressNames+'' == '') {
				alert('MapLocationEditPanel - cannot find address field using activerecord (by convention <%=ActiveRecord.PossibleFieldNamesForGeoAddress%>)');
				return;
			}

			// setup listeners onchange of address fields
			for (var i in locAddressNameArray) {
				$("#" + locAddressNameArray[i]).on('change', SetLocAuto);
			}

			var mapOptions = {
				center: new google.maps.LatLng(-36.8484597, 174.76333150000005),          // show NZ
				zoom: 16,
				mapTypeId: google.maps.MapTypeId.ROADMAP
			};

			map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

			infowindow = new google.maps.InfoWindow();
			marker = new google.maps.Marker({
				map: map, draggable: true
			});

			if ($("#"+latFieldName).val() == "" || V$("MapPositioning")=="Auto") {
				SetLocAuto();
				//mapOptions.center = new google.maps.LatLng(V$("MapLatitude"), V$("MapLongitude"));
				//mapOptions.zoom = 12;
			} else {
				// admin has already set it
				var centre = new google.maps.LatLng($("#" + latFieldName).val(), $("#" + longFieldName).val());
				map.setCenter(centre);
				marker.setPosition(centre);
				SetLocManual();
			}

			var input = document.getElementById('LatLongSearch');
			var autocomplete = new google.maps.places.Autocomplete(input);          //, {types: [ 'museum', 'park', 'place_of_worship', 'shopping_mall', 'stadium', 'subway_station', 'train_station', 'zoo', 'colloquial_area', 'locality', 'neighborhood', 'point_of_interest', 'sublocality' ]}
			$("#LatLongSearch").keypress(function(event) {
				if (event.which == 13) { // make enter key just search map and stop it from submitting form
					event.preventDefault();
				}
			});

			// bias towards current map view area
			autocomplete.bindTo('bounds', map);

			if ($("#"+latFieldName).val() != "-36.8484597" && $("#"+latFieldName).val() != "") {
				//	marker.setPosition(mapOptions.center);
			}

			google.maps.event.addListener(marker, 'position_changed', function () {
				$("#"+latFieldName).val(marker.getPosition().lat())
				$("#"+longFieldName).val(marker.getPosition().lng())
			});

			google.maps.event.addListener(marker, 'dragend', function () {
				// when user drags marker
				$("#PosManualCheck")[0].checked = true;
				SetLocManual();
			});

			google.maps.event.addListener(autocomplete, 'place_changed', function () {
				//infowindow.close();
				var place = autocomplete.getPlace();
				if (place.geometry.viewport) {
					map.fitBounds(place.geometry.viewport);
				} else {
					map.setCenter(place.geometry.location);
					map.setZoom(16);
				}

				marker.setPosition(place.geometry.location);
				$("#"+latFieldName).val(place.geometry.location.lat())
				$("#"+longFieldName).val(place.geometry.location.lng())

				var address = '';
				if (place.address_components) {
					address = [(place.address_components[0] &&
						place.address_components[0].short_name || ''),
						(place.address_components[1] &&
							place.address_components[1].short_name || ''),
						(place.address_components[2] &&
							place.address_components[2].short_name || '')
					].join(' ');
				}

				//infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
				//infowindow.open(map, marker);
			});

		}
		google.maps.event.addDomListener(window, 'load', initialize);

		function SetLocManual() {
			/* reset the manual search field */
			$("#LatLongSearch").val("")
			$(".AutoGeocodedAddress").hide();
			$(".ManualOverride").fadeIn(300);
		}

		function SetLocAuto() {
			var locAddress = '';
			for (var i in locAddressNameArray) {
				if (locAddress != '') locAddress += ', ';
				locAddress += df_GetText(locAddressNameArray[i]);
			}
			V$("GeocodedAddress", locAddress);
			$(".ManualOverride").hide();
			$(".AutoGeocodedAddress").show();
			var geocoder = new google.maps.Geocoder();
			geocoder.geocode({ 'address': locAddress }, function (results, status) {
				//infowindow.close();
				$(".geocoder-info").html("Searching").removeClass("bad").removeClass("good")
				if (status == google.maps.GeocoderStatus.OK) {
					map.setCenter(results[0].geometry.location);
					if (results[0].geometry.location_type == "APPROXIMATE") {
						$(".geocoder-info").html("Approx").addClass("bad");
					} else {
						$(".geocoder-info").html("Found").addClass("good");
					}
					marker.setPosition(results[0].geometry.location);
					$("#" + latFieldName).val(results[0].geometry.location.lat());
					$("#" + longFieldName).val(results[0].geometry.location.lng());
					/*update the info window with the address*/
					if (results[0].formatted_address) {
						var address = results[0].formatted_address;
						$('#foundaddress').html(address);
						//var name = address.split(",")[0];
						//infowindow.setContent('<div><strong>' + name + '</strong><br>' + address);
						//infowindow.open(map, marker);
					}
				} else {
					$(".geocoder-info").html(status).addClass("bad");
					//console.log("Geocode was not successful for the following reason: " + status);
				}
			});
		}
	</script> 
	<div class="AutoGeocodedAddress">
		<div><span style="width:100px;display:inline-block">Source address:</span> <%=new Forms.DisplayField("GeocodedAddress",Model.DataRecord.GetGeoAddress()) %></div>
		<div><span style="width:100px;display:inline-block">Found address:</span> <span id="foundaddress"></span> <span class="geocoder-info sticker"></span></div>
	</div> 
	<div class="ManualOverride"> 
		Location search: <input id="LatLongSearch" type="text" size="50"> 
	</div> 
	<div id="map_canvas"></div> 