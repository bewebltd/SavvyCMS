<%@ Page Title="Admin - Faqsection List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.FAQSectionAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("FAQ Section List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("FAQ Section List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td colspan="1"></td>	
				<td><%=dataList.ColSort("SectionName","Section Name")%></td>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
				<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
					<%=Html.ChildListLink(listItem.FAQItems)%>
				</td>
		
				<td><%=listItem.SectionName.HtmlEncode() %></td>
				<%=Html.DraggableSortPosition(listItem,listItem.SortPosition) %>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

