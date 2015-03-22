<%@ Page Title="Edit Gallery Image" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GalleryImageAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script>
		$(document).ready(function(){
			UpdateMediaType()
		})
		function UpdateMediaType() {
			$(".mediatype").hide();
			var media = $("#MediaType").val().toLowerCase();
			$("."+media).show();
		}
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.GalleryImage; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Gallery Image</th>
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
				<td class="label">Gallery Category:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.GalleryCategoryID, true, true).Add(new Sql("SELECT GalleryCategoryID , Title FROM GalleryCategory"))%></td>
			</tr>
			<tr>
				<td class="label">Media:</td>
				<td class="field"><%=new Forms.Dropbox(record.Fields.MediaType, true){onclick = "UpdateMediaType();"}.Add(GalleryImage.GALLERYIMAGEMEDIATYPEPHOTO).Add(GalleryImage.GALLERYIMAGEMEDIATYPEYOUTUBE) %></td>
			</tr>
			<tr class="mediatype photo">
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture, true) %></td>
			</tr>
			<tr class="mediatype youtube">
				<td class="label">YouTube Video ID: <%=Html.SavvyHelp("The YouTube Video ID can be found in the share URL of a video on YouTube. View a video on YouTube and click the share button, the video ID is the text of the share URL after the last forward slash.") %></td>
				<td class="field"><%= new Forms.TextField(record.Fields.YouTubeVideoID, true) %></td>
			</tr>
			<tr>
				<td class="label">Caption:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.PictureCaption, true) %></td>
			</tr>
			<tr>
				<td class="label">Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.DateTaken, true) %></td>
			</tr>
			<%//Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true,"")); %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
			<tr>
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %> (enter 50 for alphabetical order, or a lower number to list first)</td>
			</tr>
			<tr class="mediatype photo">
				<td class="label">Is Cover Image:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsCoverImage, true) %></td>
			</tr>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gallery Image Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

