<%@ Page Title="Admin - Text Block Group List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.TextBlockGroupAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  <%=Html.InfoMessage()%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Text Block Group List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Text Block Group List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("GroupName","Group Name")%></td>
			<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
					<%=Html.ButtonLink("Text blocks", Web.AdminRoot+"TextBlockAdmin/?textblockgroupid="+listItem.ID, "btn-mini") %>
				</td>
				<td><%:listItem.GroupName %></td>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>
				<%--<td><%=listItem.SortPosition %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

