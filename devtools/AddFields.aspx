<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFields.aspx.cs" Inherits="Site.devtools.Controller" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

<% var report = ActiveRecordDatabaseGenerator.CreateAnyMissingFieldsWithReport(); %>

	<h2 class="addFields">Add missing tables/fields to DB</h2>
	
	<% if(report.HasValues) { %>
		<table class="stdtable box">
			<thead>
				<tr>
					<td>Table</td>
					<td>Status</td>
				</tr>
			</thead>
			<tbody>
			
				<% foreach (var newTable in report.NewTables) { %>
					<tr>
						<td><%=newTable%></td>
						<td>Added new table</td>
					</tr>
				<% } %>			

				<% foreach (var newColumn in report.NewColumns) { %>
					<tr>
						<td><%=newColumn.Key%></td>
						<td>
							<p><strong>Added new fields</strong></p>
							<% foreach(var newColumnName in newColumn.Value) { %>
								<p><%=newColumnName%></p>
							<% } %>
						</td>
					</tr>			
				<% } %>
			</tbody>
		</table>	
	<% } else { %>
		<p>No changes have been made on database.</p>
	<% } %>

</asp:Content>
