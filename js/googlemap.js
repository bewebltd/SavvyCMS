/// <reference path="jQuery/jquery-1.3.2-vsdoc2.js" />
/* address stuff */
var geocoder;
var map;
var marker;

function initialize(lat, lng) {
	//MAP
	if (google && google.maps) {
		var latlng = new google.maps.LatLng(-36.84852838378392, 174.76222642988586);  //sky tower
		var zoomLevel = 12
		if (lat != null) {
			latlng = new google.maps.LatLng(lat, lng);
			zoomLevel = 18
		}
		var options = {
			zoom: zoomLevel,
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


		google.maps.event.addListener(marker, 'drag', function () {
			geocoder.geocode({ 'latLng': marker.getPosition() }, function (results, status) {
				if (status == google.maps.GeocoderStatus.OK) {
					if (results[0]) {
						$('#address').val(results[0].formatted_address);
						$('#HomeAddressLatitude').val(marker.getPosition().lat());
						$('#HomeAddressLongitude').val(marker.getPosition().lng());
					}
				}
			});
		});
	} else {
		//hide search
		$('#PostalSearchField,#PostalSearchLabel').hide(); $('#PostalSearchField input').val('')
		$('#HomeSearchField,#HomeSearchLabel').hide(); $('#HomeSearchField input').val('')
	}
}
function TermsProblemMessage() {
	//alert('You must read the terms and conditions to continue');
	$('#colorboxServerValidationDiv').html('You must read and agree to the Duty of Disclosure and the Terms and Conditions');
	$.fn.colorbox({ innerWidth: "378px", inline: true, href: "#colorboxServerValidationDisplay", opacity: 0.7 })
}
function GetCompIndex(item, fieldName) {
	var result = -1;
	if (item && item.address_components) {
		for (var sc = 0; sc < item.address_components.length; sc++) 
		{
			if (item.address_components[sc].types && item.address_components[sc].types[0] == fieldName) 
			{
				result = sc;
				break;
			}
		}
	}
	return result;
}
function GetCompName(item, fieldName) {
	var result = '';
	var index = GetCompIndex(item,fieldName) 
	try {
		if (item && item.address_components) {
			if (item.address_components.length > index && item.address_components[index].long_name) {
				result = item.address_components[index].long_name
			}
		}

	} catch (e) { }
	return result;
}
function AddressAndMapSetup() {

	initialize();

	if (google) {
		$(function () {
			$("#address").autocomplete({
				//This bit uses the geocoder to fetch address values
				source: function (request, response) {
					geocoder.geocode({ 'address': request.term + ', New Zealand' }, function (results, status) {
						response($.map(results, function (item) {
							var suburb = GetCompName(item, 'sublocality');
							if(suburb=='')	 suburb = GetCompName(item, 'locality');

							return {
								label: item.formatted_address,
								value: item.formatted_address,
								latitude: item.geometry.location.lat(),
								longitude: item.geometry.location.lng(),
								street: GetCompName(item, 'street_number') + ' ' + GetCompName(item, 'route'),
								suburb: suburb,
								city: GetCompName(item, 'administrative_area_level_1'),
								region: GetCompName(item, 'administrative_area_level_3'), 
								postcode: GetCompName(item, 'postal_code')
							}
						}));
					})
				},
				//This bit is executed upon selection of an address
				select: function (event, ui) {
					$("#HomeAddressLatitude").val(ui.item.latitude);
					$("#HomeAddressLongitude").val(ui.item.longitude);
					$("#HomeAddressStreet").val(ui.item.street);
					$("#HomeAddressSuburb").val(ui.item.suburb);
					$("#HomeAddressCity").val(ui.item.city);
					$("#HomeAddressRegion").val((ui.item.region+"" != "") ? ui.item.region : ui.item.city);
					if (ui.item.postcode != "New Zealand") $("#HomeAddressPostCode").val(ui.item.postcode);

					var location = new google.maps.LatLng(ui.item.latitude, ui.item.longitude);
					marker.setPosition(location);
					map.setCenter(location);

					window.setTimeout(function () { $('#address').val('') }, 5000)
				}
			});
		});
		/*
		$(function () {
			$("#postaladdress").autocomplete({
				//This bit uses the geocoder to fetch address values
				source: function (request, response) {
					geocoder.geocode({ 'address': request.term + ', New Zealand' }, function (results, status) {
						response($.map(results, function (item) {
							var suburb = GetCompName(item, 'sublocality');
							if(suburb=='')	 suburb = GetCompName(item, 'locality');
							return {
								label: item.formatted_address,
								value: item.formatted_address,
								latitude: item.geometry.location.lat(),
								longitude: item.geometry.location.lng(),
								street: GetCompName(item, 'street_number') + ' ' + GetCompName(item, 'route'),
								suburb: suburb,
								city: GetCompName(item, 'administrative_area_level_1'),
								region: GetCompName(item, 'administrative_area_level_3'),
								postcode: GetCompName(item, 'postal_code')
							}
						}));
					})
				},
				//This bit is executed upon selection of an address
				select: function (event, ui) {
					$("#PostalAddressLatitude").val(ui.item.latitude);
					$("#PostalAddressLongitude").val(ui.item.longitude);
					$("#PostalAddressStreet").val(ui.item.street);
					$("#PostalAddressSuburb").val(ui.item.suburb);
					$("#PostalAddressCity").val(ui.item.city);
					$("#PostalAddressRegion").val(ui.item.region);
					if (ui.item.postcode != "New Zealand") $("#PostalAddressPostCode").val(ui.item.postcode);

					var location = new google.maps.LatLng(ui.item.latitude, ui.item.longitude);
					marker.setPosition(location);
					map.setCenter(location);
					window.setTimeout(function () { $('#postaladdress').val('') }, 5000)
				}
			});
		});
		*/
	} else {
		$("#address").hide()
		$("#postaladdress").hide()
	}
	//$('#mapcanvas').find('Map Data');
	//$('#HomeAddressStreet').focus(function () { $('#HomeSearchField,#HomeSearchLabel').show(); }).focus();
	//$('#HomeSearchField input').blur(function () { window.setTimeout(function () { $('#HomeSearchField,#HomeSearchLabel').hide(); $('#HomeSearchField input').val('') }, 10000) })
	//$('#PostalAddressStreet').focus(function () { $('#PostalSearchField,#PostalSearchLabel').show(); }).focus();
	//$('#PostalSearchField input').blur(function () { window.setTimeout(function () { $('#PostalSearchField,#PostalSearchLabel').hide(); $('#PostalSearchField input').val('') }, 10000) })
}
