<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>		
		global<br/>
		<%			 
			int sc = 0;				

			var globalFragment = new Action(() => {
				%>subtotal [<%=sc %>]<br/><%
			});
			
			for (sc = 2; sc < 10; sc++) {
				globalFragment.Invoke();
			}	
			%>
			
			<br>
			scoped / typed vars passed<br><%

			var typedFragment = new Action<int, string>((x, y) => {
				%>html[<%=y%>,<%=x %>]<br /><%
			});

			for (sc = 99; sc < 103; sc++) {
				typedFragment.Invoke(sc + 5, "324234");
			}
		%>
	</div>
	</form>
</body>
</html>
