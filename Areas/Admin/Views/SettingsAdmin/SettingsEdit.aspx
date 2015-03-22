<%@ Page Title="Edit Settings" Inherits="System.Web.Mvc.ViewPage<Models.Settings>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate-vsdoc.js"></script><%}   // provides intellisense %>
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form");

			$('input[name="EnablePageRevisions"]').change(function () {
				if ($(this).val() === "False") {
					$('#EnableWorkflow_False').click();
					$('#EnableRevisionEditing_False').click();
				}
			});
			
			$('#EnableWorkflow_True, #EnableRevisionEditing_True').change(function () {
				if ($('input[name="EnablePageRevisions"]:checked').val() === "False") {
					$('#EnableWorkflow_False').click();
					$('#EnableRevisionEditing_False').click();
				}
			});
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
				<th colspan="2">Global Settings</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label section"><strong>Analytics</strong></td>
				<td class="section"></td>
			</tr>		
			<tr>
				<td class="label">Analytics Tags:</td>
				<td class="field"><%=new Forms.TextArea(record.Fields.AnalyticsTags, false){rows = 6} %></td>
			</tr>
			<tr>
				<td class="label section"><strong>Search Engine Optimisation (SEO)</strong></td>
				<td class="section"></td>
			</tr>		
			<%--<tr>
				<td class="label">Page Title Tag Format:</td>
				<td class="field">
					All pages will have this format for title tags. These are displayed in Google search results.<br />
					<%=new Forms.TextField(record.Fields.pa, false) %> <small>eg [title] - [sitename]</small>
				</td>
			</tr>--%>

			<tr>
				<td class="label section"><strong>Revision Settings</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Enable Page Revisions:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.EnablePageRevisions, true) %></td>
			</tr>
			<tr>
				<td class="label">Enable Workflow: <br /><small>It will consider Publisher and Editor roles</small></td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.EnableWorkflow, true) %></td>
			</tr>
			<tr>
				<td class="label">Enable Revision Editing:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.EnableRevisionEditing, true) %></td>
			</tr>
<%--		
			<tr>
				<td class="label">Scheduled Task  - Last daily run time:</td>
				<td class="field"><%=record.ScheduledTaskLastDailyRunTime.FmtDateTime() %></td>
			</tr>


			<tr>
				<td class="label">Webmaster Email:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.WebmasterEmail, true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Settings Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

