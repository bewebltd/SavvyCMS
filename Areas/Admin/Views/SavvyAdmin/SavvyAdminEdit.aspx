<%@ Page Title="Edit Savvy Admin" Inherits="System.Web.Mvc.ViewPage<Models.SavvyAdmin>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Savvy Admin</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Header Color:</td>
				<td class="field"><%=new Forms.ColorPickerField(record.Fields.HeaderColor, false) %></td>
			</tr>
			<tr>
				<td class="label">Client Logo Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.ClientLogoPicture ,false) %></td>
			</tr>
			<tr>
				<td class="label">Show Savvy Logo:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowSavvyLogo, true) %></td>
			</tr>
			<tr>
				<td class="label">Client Help Contact Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.ClientHelpContactName, false) %></td>
			</tr>
			<tr>
				<td class="label">Client Help Contact Email:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.ClientHelpContactEmail, false) %></td>
			</tr>

<%--					
					<tr>
						<td class="label">Text Color:</td>
						<td class="field"><%=new Forms.TextField(record.Fields.TextColor, true) %></td>
					</tr>
					<tr>
						<td class="label">Line Color:</td>
						<td class="field"><%=new Forms.TextField(record.Fields.LineColor, true) %></td>
					</tr>
					<tr>
						<td class="label">Cell Color:</td>
						<td class="field"><%=new Forms.TextField(record.Fields.CellColor, true) %></td>
					</tr>
					<tr>
						<td class="label">Cell Alt Color:</td>
						<td class="field"><%=new Forms.TextField(record.Fields.CellAltColor, true) %></td>
					</tr>
--%>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%//=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Savvy Admin Edit")) %>
						<%--<%//=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

