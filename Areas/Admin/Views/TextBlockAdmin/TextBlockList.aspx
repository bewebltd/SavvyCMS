<%@ Page Title="Admin - Text Block List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.TextBlockAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Text Block List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<%if(Security.IsDevAccess){ %>
					DEV: <input type=button value="Create New" onclick="location='<%=Url.Action("Create") %><%=(Web.Request["TextBlockGroupID"]+"" !="")?"?TextBlockGroupID="+Web.Request["TextBlockGroupID"]:""%>'" class="CreateNewButton" />
				<%} %>
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Text Block List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("Title","Title")%></td>
			<td><%=dataList.ColSort("GroupName")%></td>
			<% if (Security.IsDevAccess) { %><td><%= dataList.ColSort("SectionCode", "Section Code") %></td><% } %>
			<td><%=dataList.ColHead("SortPosition","Order")%></td>

		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td>
					<%if(listItem.Title.IsNotBlank()){ %>
						<%= listItem.Title.HtmlEncode() %>
					<% }else{ %>
						<%= listItem.SectionCode.HtmlEncode() %>
					<% } %>
				</td>
				<td><%=listItem["GroupName"].HtmlEncode() %></td>
				<% if (Security.IsDevAccess) { %><td><%=listItem.SectionCode.HtmlEncode() %></td><% } %>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition) %>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>
	<%if(Security.IsDevAccess){ %>
		
		Recreate blocks: <%=Html.SavvyHelp(@"You can run ?droptextblocks=1 on any page to drop the blocks on that page and force a re-create.", width:400) %><br />
		Drop all blocks <a href="<%=Web.AdminRoot+"TextblockAdmin/dropall" %>" onclick="return confirm('are you sure?')">go</a>
	<%} %>

</asp:Content>

