<%@ Page Title="Admin - News List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.NewsAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("News List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%--extra filter--%>
        <%}); %>
				<%=Html.SavvyHelpText(new HelpText("News List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td></td>
			
			<td><%=dataList.ColSort("Title")%></td>
			<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
			<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>
<%--
				<td><%=dataList.ColSort("Picture","Picture")%></td>
				<td><%=dataList.ColSort("LinkUrl","Link Url")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				
				<td><%=Html.PreviewLink(listItem)%></td>
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.Title.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<td><%=Fmt.Date(listItem.ExpiryDate) %></td>
<%--
				<td><%=listItem.Picture.HtmlEncode() %></td>
				<td><%=listItem.LinkUrl.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

