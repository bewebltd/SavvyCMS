<%@ Page Title="Admin - Modification Log List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ModificationLogAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Modification Log List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%--extra filter--%>
        <%}); %>	
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("UpdateDate","Update Date")%></td>
				<td><%=dataList.ColSort("PersonID","Person Id")%></td>
				<td><%=dataList.ColSort("TableName","Table Name")%></td>
				<td><%=dataList.ColSort("RecordID","Record Id")%></td>
				<td><%=dataList.ColSort("ActionType","Action Type")%></td>
				<td><%=dataList.ColSort("UserName","User Name")%></td>
<%--
				--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=Fmt.Date(listItem.UpdateDate) %></td>
				<td><%=listItem.PersonID %></td>
				<td><%=listItem.TableName %></td>
				<td><%=listItem.RecordID %></td>
				<td><%=listItem.ActionType %></td>
				<td><%=listItem.UserName %></td>
<%--
				--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

