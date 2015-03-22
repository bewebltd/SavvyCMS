<%@ Page Title="Admin - News Rss List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.NewsRSSAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	
	<%=Html.InfoMessage()%>
		
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("News Rss List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					 <%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("News RSS List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("FeedName","Feed Name")%></td>
			<td><%=dataList.ColSort("FeedURL","Feed Url")%></td>
			<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.FeedName.HtmlEncode() %></td>
				<td><%=listItem.FeedURL.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

