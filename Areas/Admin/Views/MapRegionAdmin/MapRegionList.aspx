<%@ Page Title="Admin - Map Region List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.MapRegionAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Map Region List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Map Region List")) %>
			</td>
		</tr>
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("Title","Title")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td><%:listItem.Title %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

