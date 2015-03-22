<%@ Page Title="Edit Video" Inherits="System.Web.Mvc.ViewPage<Models.Video>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Video</th>
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
				<td class="label">Video Posted Date:</td>
				<td class="field"><%= new Forms.DateField(record.Fields.VideoPostedDate, true) %></td>
			</tr>
						
			<tr>
				<td class="label">Video Description:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.VideoDescription ,false) %></td>
			</tr>
			<tr>
				<td class="label">Source Website:</td>
				<td class="field"><%=new Forms.Dropbox(record.Fields.SourceWebsiteCode, false).Add("YouTube") %></td>
			</tr>
			<%if (Model.IsNewRecord){ %>
				<tr>
					<td class="label">Video URL</td>
					<td class="field">
						<div>Please copy and paste the URL in the address bar of the video page on YouTube</div>
						<input type="text" name="VideoUrl" maxlength="150" style="width:500px;" />
					</td>
				</tr>
			<%} else {%>
				<tr>
					<td class="label">Video Code:</td>
					<td class="field"><%=new Forms.TextField(record.Fields.VideoCode, true) %></td>
				</tr>
				<tr>
					<td class="label">Thumbnail Url:</td>
					<td class="field"><%= new Forms.UrlField(record.Fields.ThumbnailUrl, true) %></td>
				</tr>
			<%} %>
			<tr>
				<td class="label">Credit:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Credit, true) %></td>
			</tr>
			<tr>
				<td class="label">Status:</td>
				<td class="field"><%=new Forms.Radios(record.Fields.Status, true).Add("New").Add("Approved").Add("Rejected") %></td>
			</tr>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Video Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

