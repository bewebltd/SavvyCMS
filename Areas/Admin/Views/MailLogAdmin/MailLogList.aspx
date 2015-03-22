<%@ Page Title="Admin - Mail Log List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.MailLogAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
  <%--style override for super wide list page--%>
	<%--<style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Mail Log List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" /><%//=Html.SavvyHelp(@"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Mail Log List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("EmailTo","Email To")%></td>
				<td><%=dataList.ColSort("EmailSubject","Email Subject")%></td>
				<td><%=dataList.ColSort("Result","Result")%></td>
				<td><%=dataList.ColSort("DateSent","Date Sent")%></td>
				<td><%=dataList.ColSort("EmailFrom","Email From")%></td>
<%--
				<td><%=dataList.ColSort("EmailFromName","Email From Name")%></td>
				<td><%=dataList.ColSort("EmailToName","Email To Name")%></td>
				<td><%=dataList.ColSort("EmailCC","Email Cc")%></td>
				<td><%=dataList.ColSort("EmailBodyPlain","Email Body Plain")%></td>
				<td><%=dataList.ColSort("EmailBodyHtml","Email Body Html")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%//=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
				</td>
				
				<td><%:listItem.EmailTo %></td>
				<td><%:listItem.EmailSubject %></td>
				<td><%:listItem.Result %></td>
				<td><%=Fmt.Date(listItem.DateSent) %></td>
				<td><%:listItem.EmailFrom %></td>
<%--
				<td><%:listItem.EmailFromName %></td>
				<td><%:listItem.EmailToName %></td>
				<td><%:listItem.EmailCC %></td>
				<td><%:listItem.EmailBodyPlain %></td>
				<td><%:listItem.EmailBodyHtml %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

