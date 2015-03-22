<%@ Page Title="Admin - Document Category List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.DocumentCategoryAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Document Category List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Web.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(title:"Document Category Help",helpText: @"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Document Category List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<% // Todo: make this list more like page heirachy? %>
				<td><%=dataList.ColSort("Title","Title")%></td>
				<%--<td><%=dataList.ColSort("IntroText","Intro Text")%></td>--%>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td class="nowrap"><%=dataList.ColSort("IsActive","Is Active")%></td>
				<%--
				<td><%=dataList.ColSort("PageCode","Page Code")%></td>
				<td class="nowrap"><%=dataList.ColSort("ParentDocumentCategoryID","Parent Category")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("AddedByPersonID","Added By Person Id")%></td>
				<td><%=dataList.ColSort("ModifiedByPersonID","Modified By Person Id")%></td>
				--%>
		</tr>
		<%foreach (var listItem in dataList.DocumentCategoryHierarchy) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td class="nowrap">
					<a href="<%=Web.Root + "DocumentCategory/"+ Crypto.EncryptID(listItem.ID) +"?preview=adminonly" %>" target="_blank" class="btn btn-mini"><i class="icon-search"></i></a> 
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID, bread=(Model.BreadcrumbLevel+1) }) %> <a href="<%=Web.AdminRoot +"DocumentAdmin?bread="+ (Model.BreadcrumbLevel+1) +"&DocumentCategoryID="+ listItem.ID%>">Documents</a>
				</td>
				<td class="nowrap"><%for(int i=0; i < (int)listItem["Depth"].ValueObject; i++){%> &nbsp;&nbsp;- <%} %><%:listItem.Title %></td>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, listItem.ParentDocumentCategoryID) %>
				<%--<td><%:listItem.IntroText %></td>--%>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>
				<%--
				<td><%:listItem.PageCode %></td>
				<td class="nowrap"><%:(listItem.ParentDocumentCategoryID!= null)?listItem.ParentDocumentCategory.GetName(): ""%></td>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>
				<td><%=Fmt.Date(listItem.DateModified) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%:listItem.AddedByPerson.GetName()%></td>
				<td><%:listItem.ModifiedByPerson.GetName()%></td>
				--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

