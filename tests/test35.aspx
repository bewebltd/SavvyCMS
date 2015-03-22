<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="test35.aspx.cs" Inherits="Test35" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>net3.5 test</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    .net 3.5 test
			<asp:ListView runat="server" >
				<ItemTemplate>
					<asp:CheckBox Text="test" runat="server" Checked="true"  />
<%--					<asp:TextBox runat="server" Text="<%#Eval("//data/@blahname") %>"></asp:TextBox>
--%>				</ItemTemplate>
			</asp:ListView>
<%--    <asp:XmlDataSource ID="ds" runat="server">
			<Data>
				<data name="blahname">blahvalue</data>
			</Data>
    </asp:XmlDataSource>
--%>    .net 3.5 test done
    </div>
    </form>
</body>
</html>
