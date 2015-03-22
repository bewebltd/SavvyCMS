<%@ Page Title="Edit Url Redirect" Inherits="System.Web.Mvc.ViewPage<Models.UrlRedirect>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
			ShowHideURL('to')
			ShowHideURL('from')
		});

		function ShowHideURL(field) {
			var fieldVal = "";
			var label = "";
			if (field == "to") {
				fieldVal = $("#RedirectToUrl").val();
				label = "#redirectToLabel";
			}
			else if (field == "from") {
				fieldVal = $("#RedirectFromUrl").val();
				label = "#redirectFromLabel";
			}
			if (fieldVal.toLowerCase().substring(0, 7) == "http://" || fieldVal.toLowerCase().substring(0, 8) == "https://" || fieldVal.toLowerCase().substring(0, 2) == "//") {
				FadeLabelOut(label)
			} else {
				FadeLabelIn(label)
			}
		}
	
		function FadeLabelOut(label) {
			$(label).fadeOut(500);
		}
		function FadeLabelIn(label) {
			$(label).fadeIn(500);
		}

	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Url Redirect</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label" style="width:800px" colspan="2">
				Enables you to redirect any outdated URLs to the correct ones using HTTP 301 Permanent Redirects. This is mainly useful when migrating from an old site (eg flightdeals/default.aspx redirects to Flights).<br />
				You can also use this to add shortcuts for URLs (eg myspecialpromotion redirects to Page/july-promotion). 
				<br>
				You can use full URLs rather than just a page entry. To do this, type in a url which starts with "http://" or  "https://" or  "//".
				</td>
			</tr>
			
			<tr>
				<td class="label">Redirect From URL:</td>
				<td class="field"><span id = "redirectFromLabel"><%=Web.BaseUrl %></span><%= new Forms.TextField(record.Fields.RedirectFromUrl, true) { onkeyup = "ShowHideURL('from')"} %></td>
			</tr>
			<tr>
				<td class="label">Redirect To URL:</td>
				<td class="field"><span id = "redirectToLabel"><%=Web.BaseUrl %></span><%= new Forms.TextField(record.Fields.RedirectToUrl, false) { onkeyup = "ShowHideURL('to')"} %></td>
			</tr>
			<tr>
				<td class="label">Active:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsActive, true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("URL Redirect Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

