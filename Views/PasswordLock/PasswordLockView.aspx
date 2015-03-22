<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.PasswordLockController.ViewModel>" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Enter Password</title>
		<style>
		body{
			padding:0;
			margin:0;
			border:0;
			background-color:#FFF;
			color: #000;
			font: normal 13px Arial, "Helvetica Neue", Helvetica, sans-serif;
			line-height:23px;
			}
		#wrapper{
			width:100%;
			height:100%;
			position:relative;
			}
		#content{
			margin:100px auto;
			}
		h1 {text-align:center;}
		</style>
	</head>
	
<body>
<div id="wrapper">
	<div id="content">
		<h1>Website coming soon</h1>
		<br/>
		<br/>
		<form action="<%=Web.Root %>PasswordLock/Enter" method="POST" cellspacing="0" cellpadding="10">
			<table width="300" align="center">
				<tr>
					<td style="background-color: black;color:white;font-family: verdana,sans-serif;padding: 10px;">Enter Your Password</td>
				</tr>
				<tr>
					<td style="border:black 2px solid;padding: 10px;"><input type="password" value="" name="password"/><input type="submit" value="Enter" />
					<%if (Util.IsBewebOffice || Util.ServerIsDev) { %><br/>DEV/OFFICE: ok to leave blank<%} %>

					</td>
				</tr>
			</table>
		</form>
	</div>
</div>

		

	</body>
</html>

