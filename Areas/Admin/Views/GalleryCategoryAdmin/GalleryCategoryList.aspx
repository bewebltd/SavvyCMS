<%@ Page Title="Admin - Gallery Category List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GalleryCategoryAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Gallery Category List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Model.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(title:"Gallery Category Help",helpText: @"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Gallery Category List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<%--<td><%=dataList.ColSort("PageID","Page")%></td>--%>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
				<%--<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>--%>
				<td><%=dataList.ColHead("Status")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%//=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID, bread=(Model.BreadcrumbLevel+1) }) %> 
					
					<a href="<%=Web.Root + "Admin/GalleryImageAdmin?bread="+(Model.BreadcrumbLevel+1)+"&GalleryCategoryID=" + listItem.ID %>">Files (<%=listItem.GalleryImages.Count %>)</a>
					<a href="<%=Web.Root + "Admin/GalleryImageAdmin/View?bread="+(Model.BreadcrumbLevel+1)+"&GalleryCategoryID=" + listItem.ID %>">View Grid</a>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><%:listItem.Title %></td>
				<%--<td><%=(listItem.PageID.HasValue)?listItem.Page.Title.HtmlEncode():"" %></td>--%>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<%--<td><%=Fmt.Date(listItem.ExpiryDate) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>--%>
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

