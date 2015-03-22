<%@ Page Title="Admin - Client Contact Us Person List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ClientContactUsPersonAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	
	<%=Html.InfoMessage()%>
		
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Client Contact Us Person List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					 <%--extra filter--%>
				<%}); %>
				
				<%=Html.SavvyHelpText(new HelpText("Client Contact Us List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<%--<td><%=dataList.ColSort("ClientContactUsRegionID","Client Contact Us Region Id")%></td>--%>
				<td><%=dataList.ColSort("PersonName","Person Name")%></td>
				<%--<td><%=dataList.ColSort("PhotoPicture","Photo Picture")%></td>--%>
				<td><%=dataList.ColSort("TelephoneNumber","Telephone Number")%></td>
				<td><%=dataList.ColSort("EmailAddress","Email Address")%></td>
<%--
				<td><%=dataList.ColSort("SkypeAddress","Skype Address")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
				<td><%=dataList.ColSort("JobDescription","Job Description")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<%--<td><%=listItem.ClientContactUsRegionID %></td>--%>
				<td><%=listItem.PersonName.HtmlEncode() %></td>
				<%--<td><%=listItem.PhotoPicture.HtmlEncode() %></td>--%>
				<td><%=listItem.TelephoneNumber.HtmlEncode() %></td>
				<td><%=listItem.EmailAddress.HtmlEncode() %></td>
<%--
				<td><%=listItem.SkypeAddress.HtmlEncode() %></td>
				<td><%=listItem.SortPosition %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
				<td><%=listItem.JobDescription.HtmlEncode() %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

