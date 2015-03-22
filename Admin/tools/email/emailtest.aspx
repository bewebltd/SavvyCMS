<%@ Page Language="C#" AutoEventWireup="true" CodeFile="emailtest.aspx.cs" Inherits="emailtest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email Send Test Tool</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<input type="button" onclick="document.location.href = 'emailtest.aspx' ;" value="Refresh Page" /><br /><br />
		Send To Email: <asp:TextBox ID="ToEmailAddress" runat="server"></asp:TextBox> 
		<asp:Button Text="Go" runat="server" />
		<br /><br />
		<asp:PlaceHolder ID="ShowResults" Visible="false" runat="server">
			[Y] = it has worked at some point<br />
			[N] = it never has<br /><br />
			Test run time: <asp:Label ID="runtime" runat="server"></asp:Label> <br /><br />
			<asp:Label ID="testResults" runat="server"></asp:Label>
		</asp:PlaceHolder>
    </div>
    </form>
</body>
</html>
