<%@ Page Title="Admin - Product List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ProductAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Product List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>		 
				<%=Html.SavvyHelpText(new Beweb.HelpText("Product List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("Price","Price")%></td>
				<td><%=dataList.ColSort("Reference","Reference")%></td>
				<td><%=dataList.ColSort("Description","Description")%></td>
				<td><%=dataList.ColSort("SortPosition","Position")%></td>
<%--
				<td><%=dataList.ColSort("Gst","Gst")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("Picture1","Picture 1")%></td>
				<td><%=dataList.ColSort("Active","Active")%></td>
				<td><%=dataList.ColSort("ModifiedDate","Modified Date")%></td>
				<td><%=dataList.ColSort("Type","Type")%></td>
				<td><%=dataList.ColSort("CategoryID","Category Id")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%:listItem.Title %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=listItem.Price.FmtCurrency() %></td>
				<td><%:listItem.Reference %></td>
				<td><%:Fmt.Ellipsis( listItem.Description.StripTags(),300) %></td>
				<%=Html.DraggableSortPosition(listItem,listItem.SortPosition,listItem.ProductCategoryID) %>
<%--
				<td><%=listItem.Gst.FmtCurrency() %></td>
				<td><%:listItem.Picture1 %></td>
				<td><%=Fmt.YesNo(listItem.Active) %></td>
				<td><%=Fmt.Date(listItem.ModifiedDate) %></td>
				<td><%:listItem.Type %></td>
				<td><%=listItem.CategoryID %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

