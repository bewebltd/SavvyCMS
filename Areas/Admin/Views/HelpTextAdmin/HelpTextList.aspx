<%@ Page Title="Admin - Help Text List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.HelpTextAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<div class="page-header"></div>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Help Text List") %>
		<tr class="colheadfilters">
			<td colspan="99">
			<%--	<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Web.QueryString%>'" class="CreateNewButton" />--%>
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Help Text List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("HelpTextCode","Help Text Code")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Last Modified")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><%:listItem.Title %></td>
				<td><%:listItem.HelpTextCode %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=(listItem.DateModified.HasValue)?Fmt.Date(listItem.DateModified):"" %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

