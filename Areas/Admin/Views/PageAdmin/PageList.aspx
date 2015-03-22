<%@ Page Title="Admin - Page List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.PageAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Page List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Web.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(@"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new Beweb.HelpText("Page List"/*default no content*/)) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("ParentPageID","Parent Page Id")%></td>
				<td><%=dataList.ColSort("PageCode","Page Code")%></td>
				<td><%=dataList.ColSort("TemplateCode","Template Code")%></td>
				<td><%=dataList.ColSort("NavTitle","Nav Title")%></td>
				<td><%=dataList.ColSort("Title","Title")%></td>
<%--
				<td><%=dataList.ColSort("SubTitle","Sub Title")%></td>
				<td><%=dataList.ColSort("Introduction","Introduction")%></td>
				<td><%=dataList.ColSort("BodyTextHtml","Body Text Html")%></td>
				<td><%=dataList.ColSort("TagLine","Tag Line")%></td>
				<td><%=dataList.ColSort("PageIsALink","Page Is Alink")%></td>
				<td><%=dataList.ColSort("LinkUrlIsExternal","Link Url Is External")%></td>
				<td><%=dataList.ColSort("LinkUrl","Link Url")%></td>
				<td><%=dataList.ColSort("URLRewriteTitle","Urlrewrite Title")%></td>
				<td><%=dataList.ColSort("PageTitleTag","Page Title Tag")%></td>
				<td><%=dataList.ColSort("MetaKeywords","Meta Keywords")%></td>
				<td><%=dataList.ColSort("MetaDescription","Meta Description")%></td>
				<td><%=dataList.ColSort("PromoImage","Promo Image")%></td>
				<td><%=dataList.ColSort("PromoVideo","Promo Video")%></td>
				<td><%=dataList.ColSort("Picture","Picture")%></td>
				<td><%=dataList.ColSort("PhotoCredit","Photo Credit")%></td>
				<td><%=dataList.ColSort("PhotoCaption","Photo Caption")%></td>
				<td><%=dataList.ColSort("NavIntroduction","Nav Introduction")%></td>
				<td><%=dataList.ColSort("NavPicture","Nav Picture")%></td>
				<td><%=dataList.ColSort("NavLinkTitle","Nav Link Title")%></td>
				<td><%=dataList.ColSort("NavLinkUrl","Nav Link Url")%></td>
				<td><%=dataList.ColSort("SidebarTitle","Sidebar Title")%></td>
				<td><%=dataList.ColSort("SidebarTextHtml","Sidebar Text Html")%></td>
				<td><%=dataList.ColSort("RolesAllowed","Roles Allowed")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("ShowInMainNav","Show In Main Nav")%></td>
				<td><%=dataList.ColSort("ShowInSecondaryNav","Show In Secondary Nav")%></td>
				<td><%=dataList.ColSort("ShowInFooterNav","Show In Footer Nav")%></td>
				<td><%=dataList.ColSort("ShowInXMLSitemap","Show In Xmlsitemap")%></td>
				<td><%=dataList.ColSort("ShowInSideBar","Show In Side Bar")%></td>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
				<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>--%>
				<td><%=dataList.ColHead("Status")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><%=Fmt.Number(listItem.ParentPageID)%></td>
				<td><%:listItem.PageCode %></td>
				<td><%:listItem.TemplateCode %></td>
				<td><%:listItem.NavTitle %></td>
				<td><%:listItem.Title %></td>
<%--
				<td><%:listItem.SubTitle %></td>
				<td><%=Fmt.YesNo(listItem.PageIsALink) %></td>
				<td><%=Fmt.YesNo(listItem.LinkUrlIsExternal) %></td>
				<td><%:listItem.LinkUrl %></td>
				<td><%:listItem.URLRewriteTitle %></td>
				<td><%:listItem.PageTitleTag %></td>
				<td><%:listItem.MetaKeywords %></td>
				<td><%:listItem.PromoImage %></td>
				<td><%:listItem.PromoVideo %></td>
				<td><%:listItem.Picture %></td>
				<td><%:listItem.PhotoCredit %></td>
				<td><%:listItem.PhotoCaption %></td>
				<td><%:listItem.NavIntroduction %></td>
				<td><%:listItem.NavPicture %></td>
				<td><%:listItem.NavLinkTitle %></td>
				<td><%:listItem.NavLinkUrl %></td>
				<td><%:listItem.SidebarTitle %></td>
				<td><%:listItem.RolesAllowed %></td>
				<td><%=Fmt.Number(listItem.SortPosition)%></td>
				<td><%=Fmt.YesNo(listItem.ShowInMainNav) %></td>
				<td><%=Fmt.YesNo(listItem.ShowInSecondaryNav) %></td>
				<td><%=Fmt.YesNo(listItem.ShowInFooterNav) %></td>
				<td><%=Fmt.YesNo(listItem.ShowInXMLSitemap) %></td>
				<td><%=Fmt.YesNo(listItem.ShowInSideBar) %></td>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<td><%=Fmt.Date(listItem.ExpiryDate) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>--%>
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

