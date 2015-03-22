<%@ Page Title="Edit Modification Log" Inherits="System.Web.Mvc.ViewPage<Models.ModificationLog>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate-vsdoc.js"></script><%}   // provides intellisense %>
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
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Modification Log</th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Update Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.UpdateDate, true) %></td>
			</tr>
				<tr>
					<td class="label">Person:</td>
					<td class="field"><%= new Forms.Dropbox(record.Fields.PersonID, true, true).Add(new Sql("SELECT PersonID , Email FROM Person"))%></td>
				</tr>
			<tr>
				<td class="label">Table Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.TableName, true) %></td>
			</tr>
			<tr>
				<td class="label">Record Id:</td>
				<td class="field">
					Link/Dropbox/Radio list for table Record here?<br>
					<%//=new Forms.TextField(record.Fields.RecordID, false) %>
					<%=new Forms.Dropbox(record.Fields.RecordID, false).Add(Models.RecordList.LoadActivePlusExisting(record.RecordID)) %>
				</td>
			</tr>
			<tr>
				<td class="label">Action Type:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.ActionType, true) %></td>
			</tr>
			<tr>
				<td class="label">User Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.UserName, true) %></td>
			</tr>
			<tr>
				<td class="label">Change Description:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.ChangeDescription ,true) %></td>
			</tr>
			<tr>
				<td class="label">Modification Log Filter:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.ModificationLogFilter, true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Modification Log Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

