<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vacuum2.aspx.cs" Inherits="Site.devtools.Vacuum2" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
	
	<h2 class="vacuum">Filesystem Vacuum 2</h2>
	
	<div class="onoffswitch">
			<input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="vacuumSwitch">
			<label class="onoffswitch-label" for="vacuumSwitch">
					<div class="onoffswitch-inner">
							<div class="onoffswitch-active"><div class="onoffswitch-switch">ON</div></div>
							<div class="onoffswitch-inactive"><div class="onoffswitch-switch">OFF</div></div>
					</div>
			</label>
	</div>

		<table id="vacuumTable" class="stdtable box">
			<thead>
				<tr>
					<td>Step</td>
					<td>Description</td>
					<td>Status</td>
					<td>Result</td>
				</tr>
			</thead>
			<tbody>
				<tr data-action="PrecacheAttachmentsDisk" class="notVerified">
					<td>1</td>
					<td>Precaching attachments from file system</td>
					<td class="status">Queued</td>
					<td class="result"></td>
				</tr>
				<tr data-action="PrecacheAttachmentsDB" class="notVerified">
					<td>2</td>
					<td>Precaching attachments from database</td>
					<td class="status">Queued</td>
					<td class="result"></td>
				</tr>
				<tr data-action="CompareAttachments" class="notVerified">
					<td>3</td>
					<td>Comparing</td>
					<td class="status">Queued</td>
					<td class="result"></td>
				</tr>
			</tbody>
		</table>	
		
		<table id="vacuumFiles" class="stdtable box" style="display:none">
			<thead>
				<tr>
					<td>Files marked to be deleted</td>
				</tr>
			</thead>
			<tbody>
			</tbody>
		</table>	
		
		<script type="text/javascript" src="scripts/class/Vacuum2.class.js"></script>

</asp:Content>
