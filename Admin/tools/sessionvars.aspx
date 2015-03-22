<%@ Page CodeFile="sessionvars.aspx.cs" EnableViewState="false" EnableViewStateMac="false" Language="c#" AutoEventWireup="true" Inherits="Beweb.Admin.SessionVars" %>
<HTML>
	<head runat="server">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body style="FONT-SIZE:xx-small;FONT-FAMILY:arial">
		<form runat="server" method="post">
			<table border="1" cellpadding="0" cellspacing="0">
				<%foreach(string key in Session.Keys){ %>
					<tr>
						<td valign="top"><%=key %></td>
						<td valign="top"><%=Session[key] %></td>
						<td valign="top"><input type="button" value="x" onclick="document.getElementById('ses_action').value='del:<%=key %>';document.forms[0].submit()" maxlength="50" size="120"></td>
						<td valign="top"><input type="text" name="<%=key %>" value="<%=Session[key] %>" maxlength="50" size="120"></td>
					</tr>
				<%} %>
					<tr>
						<td valign="top"><input type="text" name="ses_keyname" id="ses_keyname" value="" onchange="//this.id=this.value;this.name=this.value;" maxlength="50" size="50"></td>
						<td valign="top" colspan=2>new session var&nbsp;</td>
						<td valign="top"><input type="text" name="ses_newdata" id="ses_newdata" value="" maxlength="50" size="120"></td>
					</tr>
			</table><input type=submit name="ses_action" value="save" /><br>
			<br>
			<input type=submit name="ses_action" value="abandon" />

		</form>
	</body>
</HTML>
