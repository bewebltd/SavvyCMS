<%@ Page Title="Admin - Comment List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.CommentAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Comment List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<!-- <input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />-->
				<%dataList.Form(() => {%>
				   <%--extra filter--%>
				<%}); %>	
				<%=Html.SavvyHelpText(new HelpText("Comment List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("AuctionID","Auction/Buy Now")%></td>
				<td><%=dataList.ColSort("ClosingDate")%></td>
			<td><%=dataList.ColSort("CommentText","Comment Text")%></td>
			<td>Commented by</td>
			<td><%=dataList.ColSort("CommentDate","Comment Date")%></td>
			<td><%=dataList.ColSort("CommenterIP","Commenter IP")%></td>
			<td><%=dataList.ColSort("Status","Status")%></td>
			<td><%=dataList.ColSort("ApprovedByPersonID","Approved By")%></td>
			<td><%=dataList.ColSort("ApprovedDate","Approved Date")%></td>
			<%--<td><%=dataList.ColSort("ParentCommentID","Parent Comment Id")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				<td><%--<a href="<%=listItem.ArticleUrl%>" target="_blank"><%=listItem.ArticleTitle %></a>--%></td>
				<td><%--<%=listItem.Auction != null ? listItem.Auction.ClosingDate.FmtDateTime() : "NA" %>--%></td>
				<td><%=listItem.CommentTextShort.HtmlEncode() %></td>
				<td><%= listItem.CommenterFullName %></td>
				<td><%=Fmt.DateTime(listItem.CommentDate) %></td>
				<td><%if (listItem.PersonType=="Moderator"){ %>New World/Moderator<%}else{ %><%=listItem.CommenterIP %><%} %></td>
				<td><%=listItem.Status.HtmlEncode() %></td>
				<td><%=listItem.ApprovedByName %></td>
				<td><%=Fmt.DateTime(listItem.ApprovedDate) %></td>
				<%--<td><%=listItem.ParentCommentID %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

