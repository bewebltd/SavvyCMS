<%@ Page Title="Admin - Url Redirect List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.UrlRedirectAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Url Redirect List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%--extra filter--%>
        <%}); %>
				<%=Html.SavvyHelpText(new HelpText("URL Redirect List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("RedirectFromUrl","Redirect From Url")%></td>
			<td><%=dataList.ColSort("RedirectToUrl","Redirect To Url")%></td>
			<td><%=dataList.ColSort("IsActive","Is Active")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.RedirectFromUrl.HtmlEncode() %></td>
				<td><%=listItem.RedirectToUrl.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

