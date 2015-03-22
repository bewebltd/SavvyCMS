<%@ Page Title="Admin - News Rss List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.NewsRSSAdminController.ListHelper>" MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>

<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="SavvyMVC.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
	<script type="text/javascript" language="javascript" src="<%=Web.Root %>js/RSSFeed.js"></script>
	<script type="text/javascript" language="javascript">
		var pathToRoot = '<%=Web.Root %>';
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%
			int sc = 0;
			foreach (var listItem in dataList.GetResults()) {
		%>
		<tr class="<%=dataList.RowClass(listItem) %>" onclick="$('#Feed<%=listItem.ID %>').toggle();getRSSFeed(<%=listItem.ID %>,'<%=Web.Root %>','<%=listItem.FeedURL%>','<%=listItem.FeedName%>');return false;" style="cursor: pointer">
			<td>
				<%=listItem.FeedName.HtmlEncode() %>
			</td>
		</tr>
		<tr class="<%=dataList.RowClass(listItem) %>" id="Feed<%=listItem.ID %>" style="display: none">
			<td>
				<div style="height: auto; width: 650px" id="feedContent<%=listItem.ID %>">
					Content</div>
			</td>
		</tr>
		<%
			listItem.LastUpdated = DateTime.Now;
			listItem.Save();
			sc++;
		} %>
		<%=dataList.ItemCountRow()%>
	</table>
</asp:Content>
