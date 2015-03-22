<%@ Page Title="Edit Homepage Slide" Inherits="System.Web.Mvc.ViewPage<Models.HomepageSlide>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Homepage Slide</th>
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
				<td class="label">Slide Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.SlidePicture ,true) %></td>
			</tr>			
			<tr>
				<td class="label">Picture Alt Text:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.AltText, false) %></td>
			</tr>			
			<tr>
				<td class="label">Message:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.BodyText, false) %></td>
			</tr>
			<tr>
				<td class="label">Link Url:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.LinkURL, false) %></td>
			</tr>
			<tr>
				<td class="label">Publish Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.PublishDate, false) %>  (blank = don't publish)</td>
			</tr>
			<tr>
				<td class="label">Expiry Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.ExpiryDate, false) %>  (blank = don't expire)</td>
			</tr>
			<%--
				<tr>
				<td class="label">Duration:</td>
				<td class="field"><%= new Forms.IntegerField(record.Fields.Duration, true) %> (1 sec = 1000)</td>
			</tr>
			--%>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Homepage Slide Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

