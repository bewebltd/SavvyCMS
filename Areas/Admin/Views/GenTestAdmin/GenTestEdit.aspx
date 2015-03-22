<%@ Page Title="Admin - Gen Test Edit" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GenTestAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
$(document).ready(function() {
});
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.GenTest; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<div class="page-header"></div>
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Gen Test</th>
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
				<td class="label">Intro Copy:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.IntroCopyHtml ,true) %></td>
			</tr>
			<tr>
				<td class="label">Body Copy:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.BodyCopy ,true) %></td>
			</tr>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.BodyTextHtml ,true) %></td>
			</tr>
			<tr>
				<td class="label">Cost:</td>
				<td class="field">$<%= new Forms.MoneyField(record.Fields.Cost, true) %></td>
			</tr>
			<tr>
				<td class="label">Is Active:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsActive, true) %></td>
			</tr>
			<tr>
				<td class="label">Ratio:</td>
				<td class="field"><%= new Forms.FloatField(record.Fields.Ratio, true) %></td>
			</tr>
					<tr>
						<td class="label">Gui:</td>
						<td class="field">GUID: <%= new Forms.TextField(record.Fields.GUI, true) %></td>
					</tr>
			<tr>
				<td class="label">Number Of Staff:</td>
				<td class="field"><%= new Forms.IntegerField(record.Fields.NumberOfStaff, true) %></td>
			</tr>
				<tr>
					<td class="label">Gen Test Cat:</td>
					<td class="field"><%= new Forms.Dropbox(record.Fields.GenTestCatID, true, true).Add(new Sql("SELECT GenTestCatID , Category FROM GenTestCat"))%></td>
				</tr>
			<tr>
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture, false) %></td>
			</tr>
			<tr>
				<td class="label">Attachment:</td>
				<td class="field"><%= new Forms.AttachmentField(record.Fields.Attachment ,true) %></td>
			</tr>
			<tr>
				<td class="label">Picture 1:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture1, false) %></td>
			</tr>
			<tr>
				<td class="label">Attachment 1:</td>
				<td class="field"><%= new Forms.AttachmentField(record.Fields.Attachment1 ,true) %></td>
			</tr>
			<tr>
				<td class="label">Invoice Amount:</td>
				<td class="field">$<%= new Forms.MoneyField(record.Fields.InvoiceAmount, true) %></td>
			</tr>
			<tr>
				<td class="label">Attachment Pdftext:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.AttachmentPDFText ,true) %></td>
			</tr>
			<tr>
				<td class="label">Attachment Rawtext:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.AttachmentRAWText ,true) %></td>
			</tr>
			<tr>
				<td class="label">Attachment 1 Rawtext:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.Attachment1RAWText ,true) %></td>
			</tr>
					<tr class="svyCheckboxContainer">
						<td class="label">Gen Test Has Cats:<br/><br/>
							<input type="text" class="placeholder svyCheckboxFilter" placeholder="filter..." />
							<br><br>
							<a href="#" onclick="$('#GenTestHasCats .checkboxes input:visible').prop('checked',true);return false">Check all</a> | 
							<a href="#" onclick="$('#GenTestHasCats .checkboxes input:visible').prop('checked',false);return false">Uncheck all</a>
							<br><br>
							<a href="#" onclick="$('#GenTestHasCats').height($('#GenTestHasCats')[0].scrollHeight);return false">Expand</a>
						</td>
						<td class="field">
							<div style="height:200px;overflow-y:scroll;" id="GenTestHasCats">
							<%= new Forms.Checkboxes(record.GenTestHasCats, Models.GenTestCatList.LoadAll()) %>
							</div>
						</td>
					</tr>
			<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true,"")); %>
			<%//Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gen Test Edit")) %>
						<% if(!record.IsNewRecord) { %>
							<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
						<% } %>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

