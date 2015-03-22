<%@ Page Title="Edit Gen Test Has Cat" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GenTestHasCatAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
  <%var record = Model.GenTestHasCat; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Gen Test Has Cat</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Gen Test:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.GenTestID, true, true).Add(new Sql("SELECT GenTestID , Title FROM GenTest"))%></td>
			</tr>
			<tr>
				<td class="label">Gen Test Cat:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.GenTestCatID, true, true).Add(new Sql("SELECT GenTestCatID , Category FROM GenTestCat"))%></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>
			<tr>
				<td class="label">Date Modified:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.DateModified, true) %></td>
			</tr>
			<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true,"")); %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gen Test Has Cat Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

