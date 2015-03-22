<%@ Page Language="C#" AutoEventWireup="true" Inherits="admin_tools_InitialUser" Codebehind="InitialUser.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    	<asp:placeholder id="ShowForm" visible="false" runat="server">
			user: <asp:textbox id="username" runat="server" /><br />
			pass: <asp:textbox id="password" textmode="Password" runat="server" /><br />
			<asp:button text="add user" onclick="AddInitialUser" runat="server" />
		</asp:placeholder>
		<asp:literal id="DoneMessage" text="Done" runat="server" />
    </div>
    </form>
</body>
</html>
