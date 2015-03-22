<%@ Page Title="Edit Blog Comment" Inherits="System.Web.Mvc.ViewPage<Models.BlogComment>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Blog Comment</th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
				<tr>
					<td class="label">Blog:</td>
					<td class="field"><%= new Forms.Dropbox(record.Fields.BlogID, true, true).Add(new Sql("SELECT BlogID , Title FROM Blog"))%></td>
				</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.BodyText ,true) %></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>
			<tr>
				<td class="label">Is Published:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsPublished, true) %></td>
			</tr>
			<%--<tr>
				<td class="label">Comment By Person Id:</td>
				<td class="field">
					Link/Dropbox/Radio list for table Comment By Person here?<br>
					<%=new Forms.TextField(record.Fields.CommentByPersonID, false) %>
				</td>
			</tr>--%>
			<tr>
				<td class="label">Company:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Company, true) %></td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.FirstName, true) %></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.LastName, true) %></td>
			</tr>
			<tr>
				<td class="label">Email:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Email, true) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Blog Comment Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

