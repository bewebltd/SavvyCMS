<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="error_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<h1>test</h1>
		<p>A very simple page with no database access</p>
		<p>Server is: <%= Beweb.Util.ServerIs() %></p>
		<p>You might need to disable global.asax stuff to get this page to run</p>
	
    </div>
    </form>
</body>
</html>
