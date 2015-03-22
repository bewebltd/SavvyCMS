<%@ Page Title="Edit Article Url" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ArticleURLAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.ArticleURL; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Article Url</th>
			</tr>				

			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
					<!--	<div class="adminnote"><strong>Note</strong>: Only awesomeness in this field please.</div> -->
				</td>
			</tr>

			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Description:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Description, true) %></td>
			</tr>
				<tr>
					<td class="label">Article:</td>
					<td class="field"><%= new Forms.Dropbox(record.Fields.ArticleID, true, true).Add(new Sql("SELECT ArticleID , Title FROM Article"))%></td>
				</tr>
			<tr>
				<td class="label">Publish Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.PublishDate, false) %>  (blank = don't publish)</td>
			</tr>
			<tr>
				<td class="label">Expiry Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.ExpiryDate, false) %>  (blank = don't expire)</td>
			</tr>
			<tr>
				<td class="label">Is New Window:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsNewWindow, true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Article URL Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

