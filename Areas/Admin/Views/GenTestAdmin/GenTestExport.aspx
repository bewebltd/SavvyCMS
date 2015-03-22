<%@ Page Title="Export Gen Test" Inherits="System.Web.Mvc.ViewPage<Models.GenTest>" Language="C#" MasterPageFile="~/Areas/Admin/admin-export.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>
	<table class="svyEdit" cellspacing="0">
		
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
	</table>
</asp:Content>

