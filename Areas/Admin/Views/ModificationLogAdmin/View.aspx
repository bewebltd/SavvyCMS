<%@ Page Title="Edit Modification Log" Inherits="System.Web.Mvc.ViewPage<Models.ModificationLog>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">View Modification Log</th>
			</tr>				
			
					<tr>
						<td class="label">Update Date:</td>
						<td class="field"><%= record.UpdateDate.FmtDateTime() %></td>
					</tr>
						<tr>
							<td class="label">Person:</td>
							<td class="field"><%= record.PersonID%></td>
						</tr>
					<tr>
						<td class="label">Table Name:</td>
						<td class="field"><%=record.TableName%></td>
					</tr>
						<tr>
							<td class="label">Record:</td>
							<td class="field"><%= record.RecordID%></td>
						</tr>
					<tr>
						<td class="label">Action Type:</td>
						<td class="field"><%=record.ActionType%></td>
					</tr>
					<tr>
						<td class="label">User Name:</td>
						<td class="field"><%=record.UserName%></td>
					</tr>
			<tr>
				<td colspan="2" class="footer">
					<%//=Html.SaveButton() %>
					<%//=Html.SaveAndRefreshButton() %>
					<%=Html.CancelButton() %>
					<%//=Html.DeleteButton() %>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

