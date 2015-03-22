<%@ Page Title="Edit Map Location" Inherits="System.Web.Mvc.ViewPage<Models.MapLocation>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
intellisense %>
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Map Location</th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Map Location:</td>
				<td class="field">

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

						function initialize() {
							var mapOptions = {
								center: new google.maps.LatLng(-36.8484597, 174.76333150000005),          // show NZ
								zoom: 16,
								mapTypeId: google.maps.MapTypeId.ROADMAP
							};
							if ($("#MapLatitude").val() == "") {
								var geocoder = new google.maps.Geocoder();
								var address = "<%=Model.LocationAddress%>";
								geocoder.geocode({ 'address': address }, function (results, status) {
									if (status == google.maps.GeocoderStatus.OK) {
										map.setCenter(results[0].geometry.location);
										marker.setPosition(results[0].geometry.location);
										V$("MapLatitude", "");
										V$("MapLongitude", "");
									} else {
										alert("Geocode was not successful for the following reason: " + status);
									}
								});
								//mapOptions.center = new google.maps.LatLng(V$("Latitude"), V$("Longitude"));
								//mapOptions.zoom = 12;
							} else {
								// admin has already set it
								mapOptions.center = new google.maps.LatLng(V$("Latitude"), V$("Longitude"));
							}

							var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

							var input = document.getElementById('LatLongSearch');
							var autocomplete = new google.maps.places.Autocomplete(input);          //, {types: [ 'museum', 'park', 'place_of_worship', 'shopping_mall', 'stadium', 'subway_station', 'train_station', 'zoo', 'colloquial_area', 'locality', 'neighborhood', 'point_of_interest', 'sublocality' ]}
							$("#LatLongSearch").keypress(function (event) {
								if (event.which == 13) {    // make enter key just search map and stop it from submitting form
									event.preventDefault();
								}
							})

							// bias towards current map view area
							autocomplete.bindTo('bounds', map);

							var infowindow = new google.maps.InfoWindow();
							var marker = new google.maps.Marker({
								map: map, draggable: true
							});

							if ($("#Latitude").val() != "-36.8484597" && $("#Latitude").val() != "") {
								marker.setPosition(mapOptions.center);
							}

							google.maps.event.addListener(marker, 'position_changed', function () {
								$("#Latitude").val(marker.getPosition().lat())
								$("#Longitude").val(marker.getPosition().lng())
							});

							google.maps.event.addListener(autocomplete, 'place_changed', function () {
								infowindow.close();
								var place = autocomplete.getPlace();
								if (place.geometry.viewport) {
									map.fitBounds(place.geometry.viewport);
								} else {
									map.setCenter(place.geometry.location);
									map.setZoom(10);
								}

								marker.setPosition(place.geometry.location);
								$("#Latitude").val(place.geometry.location.lat())
								$("#Longitude").val(place.geometry.location.lng())

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

								infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
								infowindow.open(map, marker);
							});


						}
						google.maps.event.addDomListener(window, 'load', initialize);
					</script> 
					<div> 
						<input id="LatLongSearch" type="text" size="50"> 
					</div> 
					<div id="map_canvas"></div> 
				
				</td>
			</tr>


			<tr>
				<td class="label">Latitude:</td>
				<td class="field"><%= new Forms.FloatField(record.Fields.Latitude, true){DecimalPlaces = 10, }%></td>
			</tr>
			<tr>
				<td class="label">Longitude:</td>
				<td class="field"><%= new Forms.FloatField(record.Fields.Longitude, true){DecimalPlaces = 10} %></td>
			</tr>
				<tr>
					<td class="label">Map Region:</td>
					<td class="field"><%= new Forms.Dropbox(record.Fields.MapRegionID, true, true).Add(new Sql("SELECT MapRegionID , Title FROM MapRegion"))%></td>
				</tr>
			<tr>
				<td class="label">Event Type:</td>
				<td class="field"><%=new Forms.Dropbox(record.Fields.EventType, false).Add("Public").Add("School") %></td>
			</tr>
			<tr>
				<td class="label">Location Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.LocationName, true) %></td>
			</tr>
			<tr>
				<td class="label">Location Address:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.LocationAddress, false) %></td>
			</tr>
			<tr>
				<td class="label">Dates:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.Dates, false) %></td>
			</tr>
			<tr>
				<td class="label">Start Time:</td>
				<td class="field"><%= new Forms.TimeField(record.Fields.StartTime, false) %></td>
			</tr>
			<tr>
				<td class="label">More Info Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.MoreInfoTextHtml ,false) %></td>
			</tr>
			<tr>
				<td class="label">Link Url:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.LinkUrl, false) %></td>
			</tr>
			<tr>
				<td class="label">Is Active:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsActive, true) %></td>
			</tr>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Map Location Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

