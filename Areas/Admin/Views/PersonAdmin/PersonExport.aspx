<%@ Page Title="Export Person" Inherits="System.Web.Mvc.ViewPage<Models.Person>" Language="C#" MasterPageFile="~/Areas/Admin/admin-export.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>
	<table class="svyEdit" cellspacing="0">
		
			<tr>
				<td class="label">Email:</td>
				<td class="field"><%=record.Email%></td>
			</tr>
			<tr>
				<td class="label">Password:</td>
				<td class="field"><%=record.Password%></td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td class="field"><%=record.FirstName%></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td class="field"><%=record.LastName%></td>
			</tr>
			<tr>
				<td class="label">Is Active:</td>
				<td class="field"><%=record.IsActive.FmtYesNo() %></td>
			</tr>
			<tr>
				<td class="label">Role:</td>
				<td class="field"><%=record.Role%></td>
			</tr>
			<tr>
				<td class="label">Is Dev Access:</td>
				<td class="field"><%=record.IsDevAccess.FmtYesNo() %></td>
			</tr>
			<tr>
				<td class="label">Login Count:</td>
				<td class="field"><%= record.LoginCount %></td>
			</tr>
			<tr>
				<td class="label">Failed Login Count:</td>
				<td class="field"><%= record.FailedLoginCount %></td>
			</tr>
			<tr>
				<td class="label">Last Login Date:</td>
				<td class="field"><%= record.LastLoginDate.FmtDateTime() %></td>
			</tr>
			<tr>
				<td class="label">Last Ip Address:</td>
				<td class="field"><%=record.LastIpAddress%></td>
			</tr>
			<tr>
				<td class="label">Reset Id:</td>
				<td class="field"><%=record.ResetID%></td>
			</tr>
			<tr>
				<td class="label">Reset Date:</td>
				<td class="field"><%= record.ResetDate.FmtDateTime() %></td>
			</tr>
			<tr>
				<td class="label">Reset Count:</td>
				<td class="field"><%= record.ResetCount %></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%=record.DateAdded%></td>
			</tr>
	</table>
</asp:Content>

