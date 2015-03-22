<%@ Page Title="Admin - Gen Test View" Inherits="System.Web.Mvc.ViewPage<Models.GenTest>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">View Gen Test</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=record.Title%></td>
			</tr>
			<tr>
				<td class="label">Intro Copy:</td>
				<td class="field"><%= record.IntroCopyHtml.FmtHtmlText() %></td>
			</tr>
			<tr>
				<td class="label">Body Copy:</td>
				<td class="field"><%= record.BodyCopy.FmtPlainTextAsHtml() %></td>
			</tr>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%= record.BodyTextHtml.FmtHtmlText() %></td>
			</tr>
			<tr>
				<td class="label">Cost:</td>
				<td class="field">$<%= record.Cost %></td>
			</tr>
			<tr>
				<td class="label">Is Active:</td>
				<td class="field"><%=record.IsActive.FmtYesNo() %></td>
			</tr>
			<tr>
				<td class="label">Ratio:</td>
				<td class="field"><%= record.Ratio %></td>
			</tr>
					<tr>
						<td class="label">Gui:</td>
						<td class="field">GUID: <%= record.GUI %></td>
					</tr>
			<tr>
				<td class="label">Number Of Staff:</td>
				<td class="field"><%= record.NumberOfStaff %></td>
			</tr>
				<tr>
					<td class="label">Gen Test Cat:</td>
					<td class="field"><%= record.GenTestCatID%></td>
				</tr>
			<tr>
				<td class="label">Picture:</td>
				<td class="field"><img src="<%= record.Picture %>" width="30"></td>
			</tr>
			<tr>
				<td class="label">Attachment:</td>
				<td class="field"><a href="<%= record.Attachment %>" target="_blank">download link</a></td>
			</tr>
			<tr>
				<td class="label">Picture 1:</td>
				<td class="field"><img src="<%= record.Picture1 %>" width="30"></td>
			</tr>
			<tr>
				<td class="label">Attachment 1:</td>
				<td class="field"><a href="<%= record.Attachment1 %>" target="_blank">download link</a></td>
			</tr>
			<tr>
				<td class="label">Invoice Amount:</td>
				<td class="field">$<%= record.InvoiceAmount %></td>
			</tr>
			<tr>
				<td class="label">Attachment Pdftext:</td>
				<td class="field"><%= record.AttachmentPDFText.FmtPlainTextAsHtml() %></td>
			</tr>
			<tr>
				<td class="label">Attachment Rawtext:</td>
				<td class="field"><%= record.AttachmentRAWText.FmtPlainTextAsHtml() %></td>
			</tr>
			<tr>
				<td class="label">Attachment 1 Rawtext:</td>
				<td class="field"><%= record.Attachment1RAWText.FmtPlainTextAsHtml() %></td>
			</tr>
					<tr>
						<td class="label">Gen Test Has Cats:</td>
						<td class="field">
							<div style="height:200px;overflow-y:scroll;">
							list here - not available yet
							</div>
						</td>
					</tr>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%//=Html.SaveButton() %>
						<%//=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%//=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gen Test View")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

