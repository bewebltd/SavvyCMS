<%@ Page Title="Edit Gallery Category" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GalleryCategoryAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.GalleryCategory; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Gallery Category</th>
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
			<%--<tr>
				<td class="label">Page:</td>
				<td class="field"><%=new Forms.Dropbox(record.Fields.PageID, true).Add(new Sql("Select PageID, Title from Page where TemplateCode = ","gallery".SqlizeText()))%></td>
			</tr>--%>
			<tr>
				<td class="label">BodyTextHtml:</td>
				<td class="field"><%=new Forms.RichTextEditor(record.Fields.BodyTextHtml, true)%></td>
			</tr>

			<%//Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true,"")); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gallery Category Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

