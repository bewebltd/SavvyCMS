<%@ Page Title="Edit Modification Log" Inherits="System.Web.Mvc.ViewPage<Models.ModificationLog>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
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
				<th colspan="2">Modification Log</th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
				</td>
			</tr>

			<tr>
				<td class="label">Date:</td>
				<td class="field"><%= record.UpdateDate.FmtDateTime() %></td>
			</tr>
				<tr>
					<td class="label">Person:</td>
					<td class="field"><%//= new Forms.Dropbox(record.Fields.PersonID, true, true).Add(new Sql("SELECT PersonID , Email FROM Person"))%>
					<%=(record.Person!=null)?record.Person.FullName:"unk" %> <%//=Html.ButtonLink("edit user",Web.Root+"Admin/PersonAdmin/"+record.PersonID) %>
				
					[<%=record.PersonID %>]
					</td>
				</tr>
<%--			
			<tr>
				<td class="label">User Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.UserName, true) %></td>
			</tr>
			--%>
			<tr>
				<td class="label">Table Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.TableName, true) %></td>
			</tr>
			<tr>
				<td class="label">Record Id:</td>
				<td class="field">
					<%=new Forms.TextField(record.Fields.RecordID, false){style="width:50px"} %>  <%=Html.ButtonLink("Go",Web.Root+"Admin/"+record.Fields.TableName+"Admin/"+record.RecordID) %>
				</td>
			</tr>
			<tr>
				<td class="label">Action Type:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.ActionType, true) %></td>
			</tr>
	
			<tr>
				<td class="label">Change Description:</td>
				<td class="field"><%=new Forms.TextArea(record.Fields.ChangeDescription, true) %></td>
			</tr>
			<tr>
				<td colspan="2" class="footer">
					<%=Html.SaveButton() %>
					<%=Html.SaveAndRefreshButton() %>
					<%=Html.CancelButton() %>
					<%//=Html.DeleteButton() %>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

