<%@ Page Language="C#" MasterPageFile="help.master" AutoEventWireup="true" CodeFile="Woeid.aspx.cs" Inherits="admin_help_woeid" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.js" type="text/javascript"></script>
	<script>
		$(document).ready(function () {
			$('#find').click(function (e) {
				e.preventDefault();
				$('#WOEID-results').empty();
				var handle_results = function (data) {
					var resultsTable = "<table cellpadding='0' cellspacing='0' border='0'>"
					if (data.query.count == 0) {
						resultsTable += "<tr><td>No Results</td></tr>";
					} else {
						console.log(data)
						$('#results').children().remove();
						resultsTable += "<tr><th><strong>Name</strong></th><th><strong>Description</strong></th><th>WOEID</th></tr>";
						for (result in data.query.results.place) {
							result = data.query.results.place[result];
							code = (result.admin1.code == "") ? "" : " (" + result.admin1.code + ")";
							resultsTable += "<tr><td>" + result.name + "</td><td>" + result.admin1.content + code + "</td><td class='woeid'>" + result.woeid + "</td></tr>";
						}
					}
					resultsTable += "</table>"
					$('#WOEID-results').append(resultsTable);
				};

				$.getJSON('http://query.yahooapis.com/v1/public/yql', { 'q': 'select * from geo.places where text="' + $('#city').val() + '"', 'format': 'json' }, handle_results);
			});
		});
	</script>
	<style>
		#WOEID-results {
			margin-top:20px;
		}
		#WOEID-results th,
		#WOEID-results td {
			padding: 5px;	
		}
		#WOEID-results th{
			font-weight: bold;
			text-align: left;
			color: #8e463b;
			border-bottom: 1px solid #1a2631;
		}
		#WOEID-results td {
			border-bottom: 1px solid #999;
		}
		#WOEID-results td.woeid {
			font-weight: bold;
		}
		#city {
			font-size: 11px;
		}
	</style>
	<div id="content">
		<p>City:&nbsp;<input type="text" id="city" /><input type="button" id="find" value="Look up" /></p>
		<div id="WOEID-results"></div>
	</div>
</asp:Content>

