<%@ Page Title="Edit Client Contact Us Person" Inherits="System.Web.Mvc.ViewPage<Models.ClientContactUsPerson>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Client Contact Us Person</th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Client Contact Us Region:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.ClientContactUsRegionID, true, true).Add(new Sql("SELECT ClientContactUsRegionID , RegionName FROM ClientContactUsRegion"))%></td>
			</tr>
			<tr>
				<td class="label">Person Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.PersonName, true) %></td>
			</tr>
			<%--<tr>
				<td class="label">Photo Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.PhotoPicture ,true) %></td>
			</tr>--%>
			<tr>
				<td class="label">Telephone Number:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.TelephoneNumber, true) %></td>
			</tr>
			<tr>
				<td class="label">Email Address:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.EmailAddress, true) %></td>
			</tr>			
			<tr>
				<td class="label">Job Description:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.JobDescription, true) %></td>
			</tr>			
			<tr>
				<td class="label">Introduction:</td>
				<td class="field"><%=new Forms.TextArea(record.Fields.Introduction, false) %></td>
			</tr>
			<tr>
				<td class="label">Skype Address:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.SkypeAddress, false) %></td>
			</tr>
			<tr>
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %></td>
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
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Client Contact Us Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

