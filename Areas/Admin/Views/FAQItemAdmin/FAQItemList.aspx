<%@ Page Title="Admin - Faqitem List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.FAQItemAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("FAQ Item List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("FAQ Item List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
				<td><%=dataList.ColSort("FAQSection","FAQ section")%></td>
				<td><%=dataList.ColSort("FAQTitle","FAQ Title")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
				<%--<td><%=dataList.ColSort("BodyTextHTML","Body Text Html")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=listItem.FAQSection.SectionName %></td>
				<td><%=listItem.FAQTitle.HtmlEncode() %></td>
				<%=Html.DraggableSortPosition(listItem,listItem.SortPosition) %>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

