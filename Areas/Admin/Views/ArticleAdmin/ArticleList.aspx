<%@ Page Title="Admin - Article List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ArticleAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Article List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Model.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(title:"Article Help",helpText: @"Help", width:400) %>
				<script>
					$(document).ready(function() {
						$('#ParentPageID').on('change', function() {
							location.href = '<%=Web.LocalPageUrl %>?PageID='+$(this).val();
						});
					});
				</script>
				<% 
				var page = new Models.Page();
				page.ParentPageID = Request["PageID"].ToInt(0);
				%>
				
				<span style="display: inline-block; margin: 5px 0 0 15px;">Filter by Page: <%=new Forms.ParentPageDropbox(page.Fields.ParentPageID, Util.GetSetting("SiteNavigationDepth").ToInt(), true, "articlepage")%> 
					<% if(Request["PageID"].IsNotBlank() && Security.IsSuperAdminAccess){%>
						<a href="<%=Web.AdminRoot %>PageAdmin/Edit/<%=page.ParentPageID %>" target="_blank" class="btn" style="color: #333;margin: -5px 0 0 10px;">Edit Page</a>
					<%} %>
				</span>
				<%dataList.Form(() => {%>
				 <%=new Forms.HiddenField("PageID", Request["PageID"]) %>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Article List Page")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("PageID","Page")%></td>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
				<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>
<%--
				<td><%=dataList.ColSort("Picture","Picture")%></td>
				<td><%=dataList.ColSort("PhotoCaption","Photo Caption")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("MetaKeywords","Meta Keywords")%></td>
				<td><%=dataList.ColSort("ShowArticleTitle","Show Article Title")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>--%>
				<td><%=dataList.ColHead("Status")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID, bread=(Model.BreadcrumbLevel+1) }) %>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><%:listItem.Title %></td>
				<td><%:listItem.Page != null ? listItem.Page.GetName() : "Not Set"%></td>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<td><%=Fmt.Date(listItem.ExpiryDate) %></td>
<%--
				<td><%=Beweb.Html.PicturePreview(listItem.Fields.Picture)%></td>
				<td><%:listItem.PhotoCaption %></td>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>
				<td><%:listItem.MetaKeywords %></td>
				<td><%=Fmt.YesNo(listItem.ShowArticleTitle) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>--%>
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

