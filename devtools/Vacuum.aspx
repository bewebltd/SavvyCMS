<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vacuum.aspx.cs" Inherits="Site.devtools.Vacuum" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

<% var files = GetAttachmentsList(); %>

	<h2 class="vacuum">Filesystem Vacuum</h2>
	
	<div class="onoffswitch">
			<input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="vacuumSwitch">
			<label class="onoffswitch-label" for="vacuumSwitch">
					<div class="onoffswitch-inner">
							<div class="onoffswitch-active"><div class="onoffswitch-switch">ON</div></div>
							<div class="onoffswitch-inactive"><div class="onoffswitch-switch">OFF</div></div>
					</div>
			</label>
	</div>

	<% if(files.Count > 0) { %>
		<table id="vacuumTable" class="stdtable box">
			<thead>
				<tr>
					<td>View</td>
					<td>Filename</td>
					<td>Size</td>
					<td>Status</td>
					<td>Remove</td>
				</tr>
			</thead>
			<tbody>
				<% foreach (var file in files) { %>
					<tr class="notVerified" data-filename="<%=file.FullName.Base64Encode()%>">
						<td class="buttonColumn"><a href="<%=ConvertFilePathToURL(file.FullName)%>" target="_blank"><img src="images/view.png" /></a></td>
						<td><%=file.Name%></td>
						<td><%=Fmt.FileSize(Convert.ToInt32(file.Length), 3)%></td>
						<td class="status">Queued</td>
						<td><input type="checkbox" name="remove" /></td>
					</tr>
				<% } %>			
			</tbody>
		</table>	
	<% } else { %>
		<p>No attachments found.</p>
	<% } %>
	
	<script type="text/javascript" src="scripts/class/Vacuum.class.js"></script>

</asp:Content>
