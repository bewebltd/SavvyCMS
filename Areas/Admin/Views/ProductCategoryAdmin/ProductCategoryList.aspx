<%@ Page Title="Admin - Category List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ProductCategoryAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="SavvyMVC.Helpers"%>


<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	
<script type="text/javascript">
	function PromptBeforeDelete(url) {
		var conf = confirm("Are you sure you want to delete this product?");
		if (conf == true) {
			window.open(url)
		}
		return false;
	}


</script>
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" border=0 cellpadding="0" cellspacing="0" width="75%" style="border-top: 1px solid #red;">

		<%=dataList.TitleRow("Category List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new Beweb.HelpText("Product Category List")) %>
			</td>
		</tr>	
		<tr>
			<td colspan="99" style="background: #000;height: 2px;"><hr></td>
		</tr>

		<%foreach (var listItem in dataList.GetResults()) {%> 
			
			<tr class="colhead">
			<td><h3>Category</h3></td>
				<td><b><%= dataList.ColHead("Title") %></b></td>
				<td><b><%= dataList.ColHead("On Page") %></b></td>
				<td><b><%= dataList.ColHead("Description", "Description") %></b></td>
				<td colspan="8"><b><%= dataList.ColHead("IsActive", "Is Published") %></b></td>
		</tr>
			<tr class="<%= dataList.RowClass(listItem) %>">
				<td><img src="<%= Web.Root %>images/arrow_red_left_menu.png" alt="" height="9" width="10" /><%= Html.ActionLink("Edit", "Edit", new {id = listItem.ID}) %></td>
				<td><%= listItem.Title %></td>
				<td style="font-size: 14px;font-weight: bold;">
				<% if (listItem.PageID != null) {%>
					<%= new Sql("select top 1 title from page where pageid = ", listItem.PageID.Value.SqlizeNumber()).FetchString() %> 
			<% } %>	
				</td>
				<td><%= listItem.Description.Length > 50 ? listItem.Description.Substring(0,50) + "" : listItem.Description %></td>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>
				<td></td>
				<td></td>
				<td><a href="<%= Web.Root %>admin/productadmin/Create?categoryid=<%= listItem.ID %>&catTitle=<%= listItem.Title %>">Add Product to <%=listItem.Title %></a></td>
			
			</tr>
			
			<%

			var products = ProductList.LoadByProductCategoryID(listItem.ID);
			if (products.Count > 0) { %>
					
			<tr style="background: #e1dfdf; height: 20px !important; ">
				<td></td>
				<td style="padding-left: 3px;"><h3><b>Products</b></h3></td>
				<td style="padding-left: 3px;"><b><%=dataList.ColHead("Title")%></b></td>
				<td style="padding-left: 3px;"><b><%=dataList.ColHead("Price","Price")%></b></td>
				<td style="padding-left: 3px;"><b><%=dataList.ColHead("Description","Description")%></b></td>
				<td style="padding-left: 3px;"><b><%=dataList.ColHead("IsActive","Is Published")%></b></td>
				<td></td><td></td>
			</tr>

				<%}
				foreach (var x in products) { %>
						<tr class="<%= dataList.RowClass(x) %>">
						<td></td>
							<td><img src="<%= Web.Root %>images/arrow_left_menu.png" alt="" height="9" width="10" /><a href ="<%=Web.Root%>admin/productadmin/edit/<%=x.ID %>">Edit</a> | <a href ="#" onclick="PromptBeforeDelete('<%=Web.Root%>admin/productadmin/delete?id=<%=x.ID %>&returnPage=<%=Web.Root%>admin/CategoryAdmin')">Delete</a></td>
						<td><p style="font-size: 12px;color: #840C0E"><%= x.Title %></p></td>
						<td><%= Fmt.Currency(x.Price) %></td>
						<td><%= x.Description != null && x.Description.Length > 50 ? x.Description.Substring(0,50) + "" : x.Description %></td>
						<td><%= Fmt.YesNo(x.IsActive) %></td>
						<td><img src="<%= ImageProcessing.ImageThumbPath(x.Picture1) %>"/></td>
					</tr>
				<%}	
				%>
				<tr>
					<td colspan="99" style="background: #fff;padding-top: 20px;"><hr</td>
				</tr>
				<tr>
					<td colspan="99" style="background: #111112;height: 5px;"><hr</td>
				</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>