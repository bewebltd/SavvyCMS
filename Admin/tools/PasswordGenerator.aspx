<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordGenerator.aspx.cs"
	Inherits="admin_tools_PasswordGenerator" %>

<%@ Import Namespace="Beweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Password Generator</title>
</head>
<body style="background-color:#000;font-family:Arial;font-size:10pt;color:#00FF00;">
	<form id="form1" runat="server">
	<div>
		<%
			int startNumber = 0;
			if (Request["numPasswords"] + "" != "")
			{
				startNumber = Request["numPasswords"].ConvertToInt();
			}
			else
			{
				startNumber = 10;
			}
		%>
		
		<table class="databox" cellspacing="0" cellpadding="3">
			<tr>
				<td class="dataheading" colspan="2">
					Password Generator
				</td>
			</tr>
			<tr>
				<td class="label">
					How many passwords do you want?
				</td>
				<td class="field">
					<input name="NumPasswords" value="<%=startNumber%>">
				</td>
			</tr>
		</table>
		<br>
		Click the button below to create random passwords.<br>
		<br>
		<input type="submit" value="Generate Passwords">
		<br />
		<%
			int numPasswords = (Request["NumPasswords"] + "").ConvertToInt();
			if (numPasswords != 0)
			{
				for (int i = 1; i < numPasswords; i++)
				{
					Response.Write(RandomPassword.Generate(8) + "<br>");
				}
			}
		%>
		<h2>crypto noncrypted passwords - customers</h2>
		Click this to find passwords that are not encrypted <a href="?mode=findnocrypt">link</a><br>
		Click this to change passwords that are not encrypted so that they are encrypted <a href="?mode=findnocrypt&ac=encrypt">link</a><br>
		<%=findnocrypt() %>
		<h2>crypto noncrypted passwords - admin users</h2>
		Click this to find passwords that are not encrypted <a href="?mode=findnocryptadmin">link</a><br>
		Click this to change passwords that are not encrypted so that they are encrypted <a href="?mode=findnocryptadmin&ac=encrypt">link</a><br>
		<%=findnocryptadmin() %>
	</div>
	</form>
</body>
</html>
