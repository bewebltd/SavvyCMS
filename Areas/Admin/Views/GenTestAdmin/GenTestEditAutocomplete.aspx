<%@ Page Title="Edit Gen Test" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GenTestAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>

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
		$(document).ready(function () {
			BewebInitForm("form");
		});
		function handleSelect(event, ui) {
			$('input[name=GenTestID]').val(ui.item.value);
			window.setTimeout(function () { $('#GenTestID').val(ui.item.label); }, 250);
		}


	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
	<%var record = Model.GenTest; %>

	<%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	<style>
		
	</style>

	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
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
			<td class="label">gen test person?:</td>
			<td class="field">
				autocomplete test: type mike or jeremy - replaces a dropbox				<br/>
				<%= new Forms.TextField(record.Fields.GenTestID, false).AutoComplete("GenTest","Title","GenTestID","select: handleSelect",replaceDropbox:true,textValue: ((record.GenTestID!=null)?record.Title+"":""))%>

			</td>
		</tr>

		<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true, "")); %>
		<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true, "")); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Gen Test Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>

			</td>
		</tr>
	</table>
	<%=Html.AntiForgeryToken() %>
	<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

