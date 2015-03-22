<%@ Page Title="Edit Banner" Inherits="System.Web.Mvc.ViewPage<Models.Banner>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("form")
			updateBannerType();
			updateBannerLinkType();
		});
		function updateBannerType() {
			if ($("#BannerType").val() == "pic") {
				$(".pic").show();
				$(".swf").hide();
			} else if ($("#BannerType").val() == "swf") {
				$(".swf").show();
				$(".pic").hide();
			} else {
				$(".swf").hide();
				$(".pic").hide();
			}
		}

		function updateBannerLinkType() {
			if ($("#BannerLinkType").val() == "url") {
				$(".url").show();
				$(".pdf").hide();
			} else if ($("#BannerLinkType").val() == "pdf") {
				$(".pdf").show();
				// still show url if its Flash & PDF so PDF link can be pasted into clicktagURL
				if ($("#BannerType").val() == "pic") {
					$(".url").hide();
				} else {
					$(".url").show();
				}
			} else {
				$(".pdf").hide();
				$(".url").hide();
			}
		}

		function ChangeSize(field) {
			$("#filedim_Picture").html("Dimensions: " + $(field).val());
		}

	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Banner</th>
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
				<td class="label">Banner Size (pixels):</td>
				<td class="field">
					<%= new Forms.Dropbox(record.Fields.Size, true, false){onchange = "ChangeSize(this)"}.Add("297x100").Add("297x322") %>							
				</td>
			</tr>
			<tr>
				<td class="label">Banner Type:</td>
				<td class="field">
					<%= new Forms.Dropbox(record.Fields.BannerType, true, true){onchange = "updateBannerType()"}.Add("pic","Image").Add("swf","Flash") %>							
				</td>
			</tr>
			<tr class="pic" style="display:none;">
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture ,false) %></td>
			</tr>
			<tr class="swf" style="display:none;">
				<td class="label">Flash (.SWF file) Attachment:</td>
				<td class="field">
					<%= new Forms.AttachmentField(record.Fields.BannerAttachment ,false) %>							
				</td>
			</tr>
			<tr>
				<td class="label">Banner Link Type:</td>
				<td class="field">
					<%= new Forms.Dropbox(record.Fields.BannerLinkType, true, true){onchange = "updateBannerLinkType()"}.Add("url","URL").Add("pdf","PDF") %>							
				</td>
			</tr>
			<tr class="pdf" style="display:none;">
				<td class="label">Upload Attachment:</td>
				<td class="field">
					<%= new Forms.AttachmentField(record.Fields.UploadAttachment ,false) %>							
				</td>
			</tr>
			<tr class="url" style="display:none;">
				<td class="label">Click Tag Url:</td>
				<td class="field">
					<%= new Forms.UrlField(record.Fields.ClickTagURL, false) %>
					<small>(To link to the uploaded attachment, right click on 'download attachment', copy & paste shortcut into this field)</small>
				</td>
			</tr>
			<tr>
				<td class="label">Start Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.StartDate, true) %></td>
			</tr>
			<tr>
				<td class="label">End Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.EndDate, false) %></td>
			</tr>
			<tr>
				<td class="label">Is Published:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsPublished, true) %></td>
			</tr>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Banner Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

