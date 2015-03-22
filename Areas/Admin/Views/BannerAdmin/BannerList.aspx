<%@ Page Title="Admin - Banner List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.BannerAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Banner List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Banner List Page")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>	
				<td><%=dataList.ColSort("Picture","Picture")%></td>			
				<td><%=dataList.ColSort("BannerAttachment","Banner Attachment")%></td>
				<td><%=dataList.ColSort("ClickTagURL","Click Tag Url")%></td>				
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
				<%--				
				<td><%=dataList.ColSort("StartDate","Start Date")%></td>
				<td><%=dataList.ColSort("EndDate","End Date")%></td>
				--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>				
				<td><%=listItem.Title.HtmlEncode() %></td>		
				<td><%=listItem.Picture.HtmlEncode() %></td>		
				<td><%=listItem.BannerAttachment.HtmlEncode() %></td>
				<td><%=listItem.ClickTagURL.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
<%--				
				<td><%=Fmt.Date(listItem.StartDate) %></td>
				<td><%=Fmt.Date(listItem.EndDate) %></td>
				--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

