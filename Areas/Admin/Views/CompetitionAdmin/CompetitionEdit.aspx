<%@ Page Title="Edit Competition" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.CompetitionAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
	$(document).ready(function () {

	});
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.Competition; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>" class="AutoValidate">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Competition</th>
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
				<td class="label">Intro Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.IntroTextHtml , true) %></td>
			</tr>
			<tr>
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture , false) %></td>
			</tr>
			<tr>
				<td class="label">Publish Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.PublishDate, false) %>  (blank = don't publish)</td>
			</tr>
			<tr>
				<td class="label">Expiry Date:</td>
				<td class="field"><%= new Forms.DateTimeField(record.Fields.ExpiryDate, false) %>  (blank = don't expire)</td>
			</tr>
			<tr>
				<td class="label">Competition Closed Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.CompetitionClosedTextHtml ,true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Competition Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

