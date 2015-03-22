<%@ Page Title="Admin - Blog Comment List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.BlogCommentAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Blog Comment List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>?ID=<%=Web.Request["ID"] %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Blog Comment List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			

				<td><%=dataList.ColSort("Title","Title")%></td>
				<%--<td><%=dataList.ColSort("BodyText","Body Text")%></td>--%>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
<%--
				<td><%=dataList.ColSort("CommentByPersonID","Comment By Person Id")%></td>
				<td><%=dataList.ColSort("Company","Company")%></td>
				<td><%=dataList.ColSort("FirstName","First Name")%></td>
				<td><%=dataList.ColSort("LastName","Last Name")%></td>
				<td><%=dataList.ColSort("Email","Email")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%:listItem.Title %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
<%--
				<td><%=listItem.CommentByPersonID %></td>
				<td><%:listItem.Company %></td>
				<td><%:listItem.FirstName %></td>
				<td><%:listItem.LastName %></td>
				<td><%:listItem.Email %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

