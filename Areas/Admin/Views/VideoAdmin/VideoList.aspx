<%@ Page Title="Admin - Video List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.VideoAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Video List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
        <%dataList.Form(() => {%>
           <%=new Forms.Dropbox("StatusFilter", Model.StatusFilter, false, false).Add("New").Add("Approved").Add("Rejected")%>
        <%}); %>
				<%=Html.SavvyHelpText(new HelpText("Video List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<%--<td><%=dataList.ColSort("ThumbnailUrl","Thumbnail Url")%></td>--%>
				<td><%=dataList.ColSort("VideoPostedDate","Video Posted Date")%></td>
				<td><%=dataList.ColSort("Status","Status")%></td>
<%--
				<td><%=dataList.ColSort("VideoCode","Video Code")%></td>
				<td><%=dataList.ColSort("ThumbnailUrl","Thumbnail Url")%></td>
				<td><%=dataList.ColSort("Credit","Credit")%></td>
				<td><%=dataList.ColSort("IsAuto","Is Auto")%></td>
				<td><%=dataList.ColSort("ViewCount","View Count")%></td>
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
				<td><%=dataList.ColSort("Status","Status")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.Title.HtmlEncode() %></td>
				<%-- <td><img src="<%=listItem.ThumbnailUrl.HtmlEncode() %>" /></td> --%>
				<td><%=Fmt.Date(listItem.VideoPostedDate) %></td>
				<td><%if(listItem.Status == "Approved"){%><div style="color:Green;"><b><%} %><%=listItem.Status.HtmlEncode() %></b></div></td>
<%--
				<td><%=listItem.VideoCode.HtmlEncode() %></td>
				<td><%=listItem.ThumbnailUrl.HtmlEncode() %></td>
				<td><%=listItem.Credit.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsAuto) %></td>
				<td><%=listItem.ViewCount %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
				<td><%=listItem.Status.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

