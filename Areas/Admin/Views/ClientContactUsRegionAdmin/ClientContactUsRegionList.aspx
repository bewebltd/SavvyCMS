<%@ Page Title="Admin - Client Contact Us Region List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ClientContactUsRegionAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	
	<%=Html.InfoMessage()%>
		
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Client Contact Us Regions") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					 <%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Client Contact Us Region List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td colspan="2"></td>
			
			<td><%=dataList.ColSort("RegionName","Region Name")%></td>
			<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
			<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td><a href="ClientContactUsPersonAdmin/?region=<%=listItem.ClientContactUsRegionID %>">People</a></td>
				
				<td><%=listItem.RegionName.HtmlEncode() %></td>
				<td><%=listItem.SortPosition %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

