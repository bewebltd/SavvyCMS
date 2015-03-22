<%@ Page Title="Admin - Contact Us List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ContactUsAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Contact Us List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%--extra filter--%>
        <%}); %>
				<%=Html.SavvyHelpText(new HelpText("Contact Us List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Name","Name")%></td>
				<td><%=dataList.ColSort("Email","Email")%></td>
				<td><%=dataList.ColSort("Subject","Subject")%></td>
				<td><%=dataList.ColSort("Message","Message")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
<%--			
				<td><%=dataList.ColSort("AdminNotes","Admin Notes")%></td>
				<td><%=dataList.ColSort("PersonID","Person Id")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.Name.HtmlEncode() %></td>
				<td><%=listItem.Email.HtmlEncode() %></td>
				<td><%=listItem.Subject.HtmlEncode() %></td>
				<td><%=listItem.Message.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<%--<td><%=listItem.PersonID %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

