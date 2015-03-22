<%@ Page Title="Admin - Gallery Image List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GalleryImageAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>

	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Gallery Images - search all") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") + Model.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(title:"Gallery Image Help",helpText: @"Help", width:400) %>
				<%dataList.Form(() => {%>
					<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
					--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Gallery Image List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
				<td><%=dataList.ColHead("Picture","Thumbnail")%></td>
				<td><%=dataList.ColHead("GalleryCategoryID","Gallery Category")%></td>
				<td><%=dataList.ColSort("Title","Title")%></td>
				<td><%=dataList.ColSort("DateTaken","Date Taken")%></td>
				<td><%=dataList.ColSort("MediaType","Media")%></td>
<%--				<td><%=dataList.ColSort("IsCoverImage","Cover Image")%></td>--%>
				<td><%=dataList.ColSort("SortPosition","Sort Position")%></td>
<%--
				<td><%=dataList.ColSort("Picture","Picture")%></td>
				<td><%=dataList.ColSort("PictureCaption","Picture Caption")%></td>
				<td><%=dataList.ColSort("IsCoverImage","Is Cover Image")%></td>
				<td><%=dataList.ColSort("DateTaken","Date Taken")%></td>
				<td><%=dataList.ColSort("DateAdded","Date Added")%></td>
				<td><%=dataList.ColSort("DateModified","Date Modified")%></td>
				<td><%=dataList.ColSort("PublishDate","Publish Date")%></td>
				<td><%=dataList.ColSort("ExpiryDate","Expiry Date")%></td>--%>
				<td><%=dataList.ColHead("Status")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td>
					<%//=Html.PreviewLink(listItem)%>
					<%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID, bread=(Model.BreadcrumbLevel+1) }) %>
					<%//ifchildlink: =Html.ChildListLink(listItem.example)%>
				</td>
				
				<td><a href="<%:ImageProcessing.ImagePath(listItem.Picture)%>" class="colorbox"><img src="<%:ImageProcessing.ImageThumbPath(listItem.Picture)%>" /></a></td>
				<td><a href="<%:listItem.GalleryCategory.GetAdminFullUrl()%>"><%:listItem.GalleryCategory.GetName()%></a></td>
				<td><%:listItem.Title %></td>
				<td><%:Fmt.Date(listItem.DateTaken) %></td>
				<td><%:listItem.MediaType %></td>
<%--				<td><%=Fmt.YesNo(listItem.IsCoverImage) %></td>--%>
				<%=Html.DraggableSortPosition(listItem, listItem.SortPosition, null)%>
<%--
				<td><%:listItem.PictureCaption %></td>
				<td><%=Beweb.Html.PicturePreview(listItem.Fields.Picture)%></td>
				<td><%=Fmt.Date(listItem.DateTaken) %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>
				<td><%=Fmt.Date(listItem.DateModified) %></td>
				<td><%=Fmt.Date(listItem.PublishDate) %></td>
				<td><%=Fmt.Date(listItem.ExpiryDate) %></td>--%>
				<td><%=Fmt.PublishStatusHtml(listItem.PublishDate, listItem.ExpiryDate) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

