<%@ Page Title="Admin - Gen Test Has Cat List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GenTestHasCatAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
  <%--style override for super wide list page--%>
	<%--<style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Gen Test Has Cat List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" /><%//=Html.SavvyHelp(@"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Gen Test Has Cat List")) %>
			</td>
		</tr>
		<tr class="colhead">
			<td></td>
				<td><%=dataList.ColSort("GenTestID","Gen Test Id")%></td>
				<td><%=dataList.ColSort("GenTestCatID","Gen Test Cat Id")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
				</td>
				
				<td><%=Fmt.Number(listItem.GenTestID)%></td>
				<td><%=Fmt.Number(listItem.GenTestCatID)%></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

