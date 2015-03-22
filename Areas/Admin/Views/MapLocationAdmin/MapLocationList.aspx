<%@ Page Title="Admin - Map Location List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.MapLocationAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Map Location List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Map Location List")) %>
			</td>
		</tr>
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("Latitude","Latitude")%></td>
				<td><%=dataList.ColSort("Longitude","Longitude")%></td>
				<td><%=dataList.ColSort("MapRegionID","Map Region Id")%></td>
				<td><%=dataList.ColSort("EventType","Event Type")%></td>
<%--
				<td><%=dataList.ColSort("LocationName","Location Name")%></td>
				<td><%=dataList.ColSort("LocationAddress","Location Address")%></td>
				<td><%=dataList.ColSort("StartTime","Start Time")%></td>
				<td><%=dataList.ColSort("MoreInfoTextHtml","More Info Text Html")%></td>
				<td><%=dataList.ColSort("LinkUrl","Link Url")%></td>
				<td><%=dataList.ColSort("IsActive","Is Active")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%:listItem.Title %></td>
				<td><%:listItem.Latitude %></td>
				<td><%:listItem.Longitude %></td>
				<td><%=listItem.MapRegionID %></td>
				<td><%:listItem.EventType %></td>
<%--
				<td><%:listItem.LocationName %></td>
				<td><%:listItem.LocationAddress %></td>
				<td><%=Fmt.Date(listItem.StartTime) %></td>
				<td><%:listItem.LinkUrl %></td>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

