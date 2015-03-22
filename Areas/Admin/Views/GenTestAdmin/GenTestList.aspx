<%@ Page Title="Admin - Gen Test List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GenTestAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<div class="page-header"></div>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Gen Test List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create")+Web.QueryString%>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new Beweb.HelpText("Gen Test List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("Cost","Cost")%></td>
				<td><%=dataList.ColSort("IsActive","Is Active")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
				<td><%=dataList.ColSort("Ratio","Ratio")%></td>
<%--
				<td><%=dataList.ColSort("GUI","Gui")%></td>
				<td><%=dataList.ColSort("NumberOfStaff","Number Of Staff")%></td>
				<td><%=dataList.ColSort("GenTestCatID","Gen Test Cat Id")%></td>
				<td><%=dataList.ColSort("Picture","Picture")%></td>
				<td><%=dataList.ColSort("Attachment","Attachment")%></td>
				<td><%=dataList.ColSort("Picture1","Picture 1")%></td>
				<td><%=dataList.ColSort("Attachment1","Attachment 1")%></td>
				<td><%=dataList.ColSort("InvoiceAmount","Invoice Amount")%></td>
				<td><%=dataList.ColSort("Latitude","Latitude")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>--%>
				<td><%=dataList.ColSort("SortPosition","Order")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><%:listItem.Title %></td>
				<td><%=listItem.Cost.FmtCurrency() %></td>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<td><%=Fmt.Number(listItem.Ratio)%></td>
<%--
				<td><%:listItem.GUI %></td>
				<td><%=Fmt.Number(listItem.NumberOfStaff)%></td>
				<td><%:listItem.GenTestCat.GetName()%></td>
				<td><%=Beweb.Html.PicturePreview(listItem.Fields.Picture)%></td>
				<td><%:listItem.Attachment %></td>
				<td><%=Beweb.Html.PicturePreview(listItem.Fields.Picture1)%></td>
				<td><%:listItem.Attachment1 %></td>
				<td><%=listItem.InvoiceAmount.FmtCurrency() %></td>
				<td><%=Fmt.Number(listItem.Latitude)%></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>--%>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

