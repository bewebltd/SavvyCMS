<%@ Page Title="Admin - Testimonial List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.TestimonialAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
	<%if(Util.IsDevAccess()){ %><a href="<%=Web.LocalPagePath+"SampleData" %>">create sample data</a><%} %>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Testimonial List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Testimonial List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<%--<td><%=dataList.ColSort("Comments","Comments")%></td>--%>
			<td><%=dataList.ColSort("Author","Author")%></td>
			<td><%=dataList.ColSort("AuthorRole","Author Role")%></td>
			<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
			<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td><%:listItem.Author %></td>
				<td><%:listItem.AuthorRole %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

