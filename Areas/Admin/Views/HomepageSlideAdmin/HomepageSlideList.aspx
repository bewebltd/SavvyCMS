<%@ Page Title="Admin - Homepage Slide List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.HomepageSlideAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Homepage Slide List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Homepage Slide List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("SlidePicture","Slide Picture")%></td>
				<td><%=dataList.ColSort("LinkURL","Link Url")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
<%--
				<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>
				<td><%=dataList.ColSort("Duration","Duration")%></td>--%>
				<td><%=dataList.ColHead("Status")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%:listItem.Title %></td>
				<td><img src="<%=ImageProcessing.ImageThumbPath(listItem.SlidePicture) %>" /></td>
				<td><%:listItem.LinkURL %></td>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition) %>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
<%--
				<td><%=Fmt.Date(listItem.ExpiryDate) %></td>
				<td><%=listItem.Duration %></td>--%>
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

