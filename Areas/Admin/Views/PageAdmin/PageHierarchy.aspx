<%@ Page Title="Admin - Page Hierarchy" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.PageAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="SavvyMVC.Helpers"%>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
<script type="text/javascript">
	$('#searchBox').live('keyup', function () {
		var filter = $(this).val(), count = 0;
		$(".filterRow").each(function () {
			if ($(this).text().search(new RegExp(filter, "i")) < 0) {
				$(this).hide();
			} else {
				$(this).show();
				count++;
			}
		});
	});
	$(document).ready(function () {
		$(".Expired,.Unpublished").hide();
	});
	function ShowHideExpiredPages() {
		if ($("#showHideExpired").is(':checked')) {
			$(".Expired,.Unpublished").fadeIn();
		} else {
			$(".Expired,.Unpublished").fadeOut();
		}
	}
</script>
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
	
	<table class="databox" cellpadding="0" cellspacing="0" style="min-width:980px;">
		<%=dataList.TitleRow("Page Hierarchy") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<%//if(false){//&&!Security.IsInRoleOnly(SecurityRoles.Roles.EDITOR)) { %>
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<% //} %>
		<%dataList.Form(() => {%>
		   <%--extra filter--%>
			 <label><input type="checkbox" id="showHideExpired" onclick="ShowHideExpiredPages()"><span style="margin-right: 15px;">Show expired/unpublished pages</span></label>
		   <input type="text" id="searchBox" class="placeholder" placeholder="quick find..." />
		<%}); %>	
				<%=Html.SavvyHelpText(new Beweb.HelpText("Page Hierarachy")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColHead("Title")%></td>
			<td><%=dataList.ColHead("SortPosition")%></td>
			<td><%=dataList.ColHead("Status")%></td>
			<%--<td><%=dataList.ColHead("Parent Page")%></td>--%>
			<td><%=dataList.ColHead("PublishDate")%></td>
			<td><%=dataList.ColHead("ExpiryDate")%></td>	
			<td><%=dataList.ColHead("Template")%></td>
			<%if(Util.IsDevAccess()){ %>
				<td class="dev"><%=dataList.ColHead("Page Code")%></td>
				<td class="dev"><%=dataList.ColHead("URL")%></td>
			<%}%>
		</tr>
		<%foreach (var listItem in dataList.PageHierarchy) {
			var depth = (int)listItem["Depth"].ValueObject;
			%> 
			<tr class="<%=dataList.RowClass(listItem)%> filterRow <%=Fmt.PublishStatus(listItem)%>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new {id = listItem.ID})%>
					<%=(Model.AllowAddChildPage(listItem,depth))?Html.ButtonLink("+", "PageAdmin/Create/?parentpageid="+listItem.ID,"btn-mini")+"":""%>
					<%=(listItem.TemplateCode == "gallery") ? " | <a href=\"" + Web.Root + "admin/gallery?pageID=" + listItem.ID + "\">Gallery Photos</a>" : "" %>
				</td>
				<td><%for(int i=0; i < depth; i++){%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <%} %><%:listItem.GetNavTitle() %></td>
				
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, listItem.ParentPageID) %>

				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate)%></td>
				<%--<td><%:(listItem.ParentPage==null)? "None": listItem.ParentPage.NavTitle%></td>--%>
				<td><%=listItem.PublishDate.FmtShortDate() %></td>
				<td><%=listItem.ExpiryDate.FmtShortDate() %></td>
				<td><%=listItem.TemplateCode.HtmlEncode() %></td>
				<%if(Util.IsDevAccess()){ %>
					<td class="dev"><%=listItem.PageCode.HtmlEncode() %></td>
					<td class="dev"><%:listItem.URLRewriteTitle %></td>
				<%}%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

