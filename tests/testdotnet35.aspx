<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="testdotnet35.aspx.cs" Inherits="TestDN35" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>test dot net net3.5 </title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    .net 3.5 test<br>
		Listview:<br>
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
--%>    
		<br>
    time [<%=DateTime.Now.ToShortDateString() %>]<br>
		<br>.net 3.5 test done
    </div>
    </form>
</body>
</html>
