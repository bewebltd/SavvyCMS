<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBUsage.aspx.cs" Inherits="Site.devtools.DBUsage" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

	<% var tables = GetTablesSize(); %>

	<h2 class="dbusage">DB Usage</h2>

	<% if(tables.Count > 0) { %>
		<table class="stdtable box">
			<thead>
				<tr>
					<td>Table</td>
					<td>Rows</td>
					<td>Size</td>
				</tr>
			</thead>
			<tbody>
				<% foreach (var table in tables) { %>
					<tr>
						<td><%=table.SchemaName%>.<%=table.TableName%></td>
						<td><%=table.Rows%></td>
						<td><%=Fmt.FileSize(table.Size, 3)%></td>
					</tr>
				<% } %>			
			</tbody>
		</table>	
	<% } else { %>
		<p>No tables found.</p>
	<% } %>
	
</asp:Content>
