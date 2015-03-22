<%@ Page Title="Edit News" Inherits="System.Web.Mvc.ViewPage<Models.News>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form");
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var record = Model; %>

	<%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.Root %>Admin/NewsAdmin/<%=((!Model.IsNewRecord)?"Edit/"+Model.ID:"Create") %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">News</th>
			</tr>
				<tr>
			<td colspan="2" class="header">
				<!--this replaced by .footer inner html-->
			</td>
		</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.Title ,true) %></td>
			</tr>
			<%--<tr>
				<td class="label">Title:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.Title ,true) %></td>
			</tr>
			<tr>
				<td class="label">Introduction Text:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.IntroductionText ,true) %></td>
			</tr>--%>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.BodyTextHtml ,true) %></td>
			</tr>
			<tr>
				<td class="label">Source:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Source, false) %></td>
			</tr>
			<tr>
				<td class="label">Article Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.ArticleDate, false) %>  (for display)</td>
			</tr>
			<tr>
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture ,false) { ShowDimensionMessage = true} %></td>
			</tr>
<%--			<tr>
				<td class="label">Large Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.LargePicture ,false) %></td>
			</tr>--%>
			<tr>
				<td class="label">Attachment:</td>
				<td class="field"><%= new Forms.AttachmentField(record.Fields.Attachment,false){AllowAjax=true,AllowDragArea = true,Subfolder = "News",AllowedMimetypes = "doc,pdf"} %></td>
			</tr>
			<tr>
				<td class="label">Link Url:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.LinkUrl, false) %></td>
			</tr>

			<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true,"")); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Help Text Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

