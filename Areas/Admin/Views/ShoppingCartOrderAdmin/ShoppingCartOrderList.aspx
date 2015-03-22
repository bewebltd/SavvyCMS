<%@ Page Title="Admin - Shopping Cart Order List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ShoppingCartOrderAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Shopping Cart Order List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%--extra filter--%>
        <%}); %>
				<%=Html.SavvyHelpText(new HelpText("Shopping Cart Order List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("OrderRef","Order Ref")%></td>
			<td><%=dataList.ColSort("DateOrdered","Date Ordered")%></td>
			<td><%=dataList.ColSort("PersonID","Person Id")%></td>
			<td><%=dataList.ColSort("Email","Email")%></td>
			<td><%=dataList.ColSort("FirstName","First Name")%></td>
			<td><%=dataList.ColSort("LastName","Last Name")%></td>
			<td><%=dataList.ColSort("IsCostEnquiry","Is Cost Enquiry")%></td>
			<td><%=dataList.ColSort("CustomerOrderReference","Customer Order Reference")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td><%=listItem.OrderRef.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.DateOrdered) %></td>
				<td><%=listItem.PersonID %></td>
				<td><%=listItem.Email.HtmlEncode() %></td>
				<td><%=listItem.FirstName.HtmlEncode() %></td>
				<td><%=listItem.LastName.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsCostEnquiry) %></td>
				<td><%=listItem.CustomerOrderReference.HtmlEncode() %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

