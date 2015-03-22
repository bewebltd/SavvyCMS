<%@ Page Title="Edit Mail Log" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.MailLogAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/savvy.validate.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.4-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.MailLog; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Mail Log</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Email To:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailTo, true) %></td>
			</tr>
			<tr>
				<td class="label">Email Subject:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailSubject, true) %></td>
			</tr>
			<tr>
				<td class="label">Result:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.Result, true) %></td>
			</tr>
			<tr>
				<td class="label">Date Sent:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateSent, true) %></td>
			</tr>
			<tr>
				<td class="label">Email From:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailFrom, true) %></td>
			</tr>
			<tr>
				<td class="label">Email From Name:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailFromName, true) %></td>
			</tr>
			<tr>
				<td class="label">Email To Name:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailToName, true) %></td>
			</tr>
			<tr>
				<td class="label">Email Cc:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailCC, true) %></td>
			</tr>
			<tr>
				<td class="label">Email Body Plain:</td>
				<td class="field"><%=new Forms.DisplayField(record.Fields.EmailBodyPlain, true) %></td>
			</tr>
			<tr>
				<td class="label">Email Body Html:</td>
				<td class="field"><%=record.EmailBodyHtml %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Mail Log Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

