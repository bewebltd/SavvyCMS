<%@ Page CodeFile="cookievars.aspx.cs" EnableViewState="false" EnableViewStateMac="false" Language="c#" AutoEventWireup="true" Inherits="Beweb.Admin.CookieVars" Title="CookieVars" %>
<%@ Import Namespace="Beweb" %>
<html>
	<head>
	<title>Cookie Vars</title>
	</head>
	<body style="font-size: xx-small; font-family: arial">
	<form runat="server" method="post">
		num cookies[<%=Request.Cookies.Count%>]<br>
		num keys[<%=Request.Cookies.AllKeys.Length%>]<br>
		<table class="pagecontent" border="1" style="font-size: xx-small; font-family: arial">
			<% 
			int numcookies = Request.Cookies.Count;	
			%>
			<tr>
				<td>
					<table border="1" cellpadding="0" cellspacing="0" style="font-size: x-small; font-family: arial;color:Blue">
					<%
					foreach(string item in Request.Cookies)
					{
						//'response.write "<li>" & item & ": " 
						//'response.write request.cookies(item)(item) 
						//'response.write "</li>"
						%>
						<tr>
							<td valign="top">
								<%=item%>
							</td>
							<td valign="top">
								<%=Fmt.Ellipsis(Request.Cookies[item].Value) %>
								<%
								if(Request.Cookies[item].HasKeys)
								{
									%>
									<br>
									<table border="1" cellpadding="0" cellspacing="0" style="font-size: x-small; font-family: arial;color:Green">
										<%
										foreach(string subitem in Request.Cookies[item].Values)
										{
											%>
											<tr>
												<td valign="top">
													<%=subitem%>
												</td>
												<td valign="top">
													<%=Fmt.Ellipsis(Request.Cookies[item].Values[subitem]) %>
												</td>
											</tr>
											<%
										}
										%>
									</table>
									<%
									
								}
								%>
							</td>
							<td valign="top">
								<%=Request.Cookies[item].HasKeys%>
							

							</td>
						</tr>
						<%
					}
					%>
					</table>
				</td>
			</tr>
		</table>
		
		<%//todo: cookie var add / remove %>
<%--		<table class="pagecontent" border="1" style="font-size: xx-small; font-family: arial">

			<tr>
				<td>
					Add
				</td>
				<td>
					<input type="text" name="newname" value="<%=""%>" maxlength="50" size="20">
				</td>
				<td>
					<input type="text" name="newvalue" value="<%=""%>" maxlength="50" size="20">
					
					<input type="button" name="Add" value="Add">
				</td>
			</tr>
		</table>
--%>	</form>
</body>
</html>
