<%@ Page Title="Edit Document" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.DocumentAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.Document; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Document</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Document Category:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.DocumentCategoryID, true, true).AddHierarchy(DocumentCategoryList.Load(new Sql("SELECT DocumentCategoryID, Title, ParentDocumentCategoryID FROM DocumentCategory Order By SortPosition")))%>

				</td>
			</tr>

			<tr>
				<td class="label">Description:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Description, true) %></td>
			</tr>
			<tr>
				<td class="label">File Attachment:</td>
				<td class="field"><%= new Forms.AttachmentField(record.Fields.FileAttachment ,true) %></td>
			</tr>
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.ModificationHistoryPanel(record, true)); %>
			<tr>
				<td colspan="2" class="footer">
				<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Document Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
	<br/>
	<br/>
	<table class="svyEdit databox" cellspacing="0">
		<thead class="colheadfilters">
			<tr>
				<th colspan="2">The following people have downloaded this document:<div style="float:right;">Total Downloads: <%=Fmt.Number(DocumentDownload.DownloadCount(record.ID),0) %></div></th>
			</tr>
		</thead>
		<tr class="colhead">
			<td>Name</td>
			<td>Downloaded on:</td>
		</tr>
		<% var downloadList = record.DocumentDownloads; %>
		<% var i = 0; %>
		<% foreach (var download in downloadList) {%> 
			<tr class="row-<%= (i%2 == 0) ? "even" : "odd" %>">
				<td style="height:26px;"><% if (download.PersonID.HasValue) { %><%= download.Person.FullName %><% } else { %>Anonymous download<% } %></td>
				<td style="height:26px;"><%= Fmt.DateTime(download.DateAdded) %></td>
			</tr>
			<% i++; %>
		<%}%>
	</table>
</asp:Content>

