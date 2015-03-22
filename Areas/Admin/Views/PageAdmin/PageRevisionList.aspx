<%@ Page Title="Admin - Page List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.PageAdminController.ListHelper>" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<style>
	<%=FileSystem.GetFileContents("~/areas/admin/admin.css")%>
</style>

	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	
	<table class="databox" cellpadding="0" cellspacing="0">

			<tr class="colhead">
				<td colspan="<%= Settings.All.EnableRevisionEditing ? 3 : 2 %>"><%=dataList.ColHead("Actions")%></td>
				<td><%=dataList.ColHead("No.")%></td>
				<td><%=dataList.ColHead("Modified by")%></td>
				<td><%=dataList.ColHead("Date Modified")%></td>
				<td><%=dataList.ColHead("Time since last update")%></td>
				<td><%=dataList.ColHead("Type")%></td>
		</tr>
		<%
			var results = dataList.GetResults();
			int revisionNumber = results.RecordCount; 
		%>
		<%foreach (var listItem in results) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<% if (Settings.All.EnableRevisionEditing) { %><td><a onclick="window.parent.loadRevisionEdit('<%=Web.AdminRoot%>PageAdmin/Edit/<%=listItem.ID%>');return false;" href="#">Edit</a></td><%} %>
				<td><a onclick="window.parent.loadRevisionView('<%= listItem.TemplateCode == "section" ? listItem.GetSectionUrl() : listItem.GetUrl()%>');return false;" href="#">View</a></td>
				<td><a onclick="window.parent.loadRevisionChanges('<%=Web.AdminRoot%>PageAdmin/CompareToLive/?revision1=<%=listItem.ID%>&revision1Number=<%:revisionNumber%>');return false;" class="btn btn-mini" href="#">Compare to live</a></td>
				<td><%:revisionNumber%></td>
				<td><%=listItem.ModifiedByPersonID != null ? Person.LoadByPersonID((int)listItem.ModifiedByPersonID).FullName : ""%></td>
				<td><%=String.Format("{0:dd MMM yyyy HH:mm}",listItem.DateModified) %></td>
				<td><%=Fmt.TimeDiffText(listItem.DateModified, DateTime.Now) %></td>
				<td><%=listItem.RevisionStatus %></td>
			</tr>
			<% revisionNumber--; %>
		<% } %>
		<%=dataList.ItemCountRow()%>
	</table>
