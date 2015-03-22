<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBStructure.aspx.cs" Inherits="Site.devtools.DBStructure" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
	
	<h2 class="dbstructure">DB Structure Comparison</h2>
		
	<div class="dbComparisonWrapper">
		<div class="dropdowns">
			<form id="comparisonForm" method="GET">
				<select name="left" class="databaseDropdown">
					<option value="">Select the database</option>
					<option value="DEV" <% if(Request["left"] == "DEV") { %> selected="selected" <% } %>>DEV</option>
					<option value="STG" <% if(Request["left"] == "STG") { %> selected="selected" <% } %>>STG</option>
					<option value="LVE" <% if(Request["left"] == "LVE") { %> selected="selected" <% } %>>LVE</option>
				</select>
				<select name="right" class="databaseDropdown">
					<option value="">Select the database</option>
					<option value="DEV" <% if(Request["right"] == "DEV") { %> selected="selected" <% } %>>DEV</option>
					<option value="STG" <% if(Request["right"] == "STG") { %> selected="selected" <% } %>>STG</option>
					<option value="LVE" <% if(Request["right"] == "LVE") { %> selected="selected" <% } %>>LVE</option>
				</select>
				<div class="clear"></div>
			</form>
		</div>
		<div id="view"></div>		
	</div>
	
	<link rel="stylesheet" type="text/css" href="styles/codemirror-diff-merge.css" />
	<script type="text/javascript" src="scripts/class/DBStructure.class.js"></script>
	<script src="scripts/codemirror-diff-merge.js" type="text/javascript" charset="utf-8"></script>
	
	<script>
		var comparisonLeft = "<%=ComparisonLeft%>";
		var comparisonRight = "<%=ComparisonRight%>";
	</script>

</asp:Content>
