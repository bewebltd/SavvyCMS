<%@ Page Title="Edit Event" Inherits="System.Web.Mvc.ViewPage<Models.Event>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Event</th>
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
				<td class="label">Location:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Location, true) %></td>
			</tr>
			<tr>
				<td class="label">Start Date:</td>
				<td class="field"><%= new Forms.DateTimeField(record.Fields.StartDate, true) %></td>
			</tr>
			<tr>
				<td class="label">End Date:<br />(Optional)</td>
				<td class="field"><%= new Forms.DateTimeField(record.Fields.EndDate, false) %></td>
			</tr>
			<tr>
				<td class="label">Description:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.Description ,true) %></td>
			</tr>
			<tr>
				<td class="label">Link Url:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.LinkURL, false) %></td>
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
						<%=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Event Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

