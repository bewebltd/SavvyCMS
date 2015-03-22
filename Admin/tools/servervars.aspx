<%@ Page CodeFile="servervars.aspx.cs" Language="c#" AutoEventWireup="true" Inherits="Beweb.Admin.servervars" %>
<HTML>
	<head runat="server">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body style="FONT-SIZE:xx-small;FONT-FAMILY:arial">
		<form runat="server">
		
		Request.ServerVariables[server environment variable]
		<br>
		Annotated
		<table style="FONT-SIZE:xx-small">
			<tr>
				<td>ALL_HTTP
				</td>
				<td><%//=Request.ServerVariables["ALL_HTTP"]								%></td>
				<td>All HTTP headers sent by the client.
				</td>
			</tr>
			<tr>
				<td>ALL_RAW
				</td>
				<td><%//=Request.ServerVariables["ALL_RAW"]								 %></td>
				<td>Retrieves all headers in the raw-form. The difference between ALL_RAW and 
					ALL_HTTP is that ALL_HTTP places an HTTP_ prefix before the header name and the 
					header-name is always capitalized. In ALL_RAW the header name and values appear 
					as they are sent by the client.
				</td>
			</tr>
			<tr>
				<td>APPL_MD_PATH
				</td>
				<td><%=Request.ServerVariables["APPL_MD_PATH"]						%></td>
				<td>Retrieves the metabase path for the [WAM] Application for the ISAPI DLL.
				</td>
			</tr>
			<tr>
				<td>APPL_PHYSICAL_PATH
				</td>
				<td><%=Request.ServerVariables["APPL_PHYSICAL_PATH"]			%></td>
				<td>Retrieves the physical path corresponding to the metabase path. IIS converts 
					the APPL_MD_PATH to the physical [directory] path to return this value.
				</td>
			</tr>
			<tr>
				<td>AUTH_PASSWORD
				</td>
				<td><%=Request.ServerVariables["AUTH_PASSWORD"]					 %></td>
				<td>The value entered in the client's authentication dialog. This variable is 
					only available if Basic authentication is used.
				</td>
			</tr>
			<tr>
				<td>AUTH_TYPE
				</td>
				<td><%=Request.ServerVariables["AUTH_TYPE"]							 %></td>
				<td>The authentication method that the server uses to validate users when they 
					attempt to access a protected script.
				</td>
			</tr>
			<tr>
				<td>AUTH_USER
				</td>
				<td><%=Request.ServerVariables["AUTH_USER"]							 %></td>
				<td>Raw authenticated user name.
				</td>
			</tr>
			<tr>
				<td>CERT_COOKIE
				</td>
				<td><%=Request.ServerVariables["CERT_COOKIE"]						 %></td>
				<td>Unique ID for client certificate, Returned as a string. Can be used as a 
					signature for the whole client certificate.
				</td>
			</tr>
			<tr>
				<td>CERT_FLAGS
				</td>
				<td><%=Request.ServerVariables["CERT_FLAGS"]							%></td>
				<td>bit0 is set to 1 if the client certificate is present. bit1 is set to 1 if 
					the Certifying Authority of the client certificate is invalid [not in the list 
					of recognized CA on the server].
				</td>
			</tr>
			<tr>
				<td>CERT_ISSUER
				</td>
				<td><%=Request.ServerVariables["CERT_ISSUER"]						 %></td>
				<td>Issuer field of the client certificate [O=MS, OU=IAS, CN=user name, C=USA].
				</td>
			</tr>
			<tr>
				<td>CERT_KEYSIZE
				</td>
				<td><%=Request.ServerVariables["CERT_KEYSIZE"]						%></td>
				<td>Number of bits in Secure Sockets Layer connection key size. For example, 128.
				</td>
			</tr>
			<tr>
				<td>CERT_SECRETKEYSIZE
				</td>
				<td><%=Request.ServerVariables["CERT_SECRETKEYSIZE"]			%></td>
				<td>Number of bits in server certificate private key. For example, e.g. 1024.
				</td>
			</tr>
			<tr>
				<td>CERT_SERIALNUMBER
				</td>
				<td><%=Request.ServerVariables["CERT_SERIALNUMBER"]			 %></td>
				<td>Serial number field of the client certificate.
				</td>
			</tr>
			<tr>
				<td>CERT_SERVER_ISSUER
				</td>
				<td><%=Request.ServerVariables["CERT_SERVER_ISSUER"]			%></td>
				<td>Issuer field of the server certificate.
				</td>
			</tr>
			<tr>
				<td>CERT_SERVER_SUBJECT
				</td>
				<td><%=Request.ServerVariables["CERT_SERVER_SUBJECT"]		 %></td>
				<td>Subject field of the server certificate.
				</td>
			</tr>
			<tr>
				<td>CERT_SUBJECT
				</td>
				<td><%=Request.ServerVariables["CERT_SUBJECT"]						%></td>
				<td>Subject field of the client certificate.
				</td>
			</tr>
			<tr>
				<td>CONTENT_LENGTH
				</td>
				<td><%=Request.ServerVariables["CONTENT_LENGTH"]					%></td>
				<td>The length of the content as given by the client.
				</td>
			</tr>
			<tr>
				<td>CONTENT_TYPE
				</td>
				<td><%=Request.ServerVariables["CONTENT_TYPE"]						%></td>
				<td>The data type of the content. Used with queries that have attached 
					information, such as the HTTP queries GET, POST, and PUT.
				</td>
			</tr>
			<tr>
				<td>GATEWAY_INTERFACE
				</td>
				<td><%=Request.ServerVariables["GATEWAY_INTERFACE"]			 %></td>
				<td>The revision of the CGI specification used by the server. The format is 
					CGI/revision.
				</td>
			</tr>
			<tr>
				<td>HTTP_<HeaderName>
				</td>
				<td><%//=Request.ServerVariables["HTTP_<HeaderName>"]			%></td>
				<td>The value stored in the header HeaderName. Any header other than those listed 
					in this table must be prefixed by HTTP_ in order for the ServerVariables 
					collection to retrieve its value. Note The server interprets any underscore [_] 
					characters in HeaderName as dashes in the actual header. For example if you 
					specify HTTP_MY_HEADER, the server searches for a header sent as MY-HEADER.
				</td>
			</tr>
			<tr>
				<td>HTTPS
				</td>
				<td><%=Request.ServerVariables["HTTPS"]									 %></td>
				<td>Returns ON if the request came in through secure channel [SSL] or it returns 
					OFF if the request is for a non-secure channel.
				</td>
			</tr>
			<tr>
				<td>HTTPS_KEYSIZE
				</td>
				<td><%=Request.ServerVariables["HTTPS_KEYSIZE"]					 %></td>
				<td>Number of bits in Secure Sockets Layer connection key size. For example, 128.
				</td>
			</tr>
			<tr>
				<td>HTTPS_SECRETKEYSIZE
				</td>
				<td><%=Request.ServerVariables["HTTPS_SECRETKEYSIZE"]		 %></td>
				<td>Number of bits in server certificate private key. For example, 1024.
				</td>
			</tr>
			<tr>
				<td>HTTPS_SERVER_ISSUER
				</td>
				<td><%=Request.ServerVariables["HTTPS_SERVER_ISSUER"]		 %></td>
				<td>Issuer field of the server certificate.
				</td>
			</tr>
			<tr>
				<td>HTTPS_SERVER_SUBJECT
				</td>
				<td><%=Request.ServerVariables["HTTPS_SERVER_SUBJECT"]		%></td>
				<td>Subject field of the server certificate.
				</td>
			</tr>
			<tr>
				<td>INSTANCE_ID
				</td>
				<td><%=Request.ServerVariables["INSTANCE_ID"]						 %></td>
				<td>The ID for the IIS instance in textual format. If the instance ID is 1, it 
					appears as a string. You can use this variable to retrieve the ID of the 
					Web-server instance (in the metabase] to which the request belongs.
				</td>
			</tr>
			<tr>
				<td>INSTANCE_META_PATH
				</td>
				<td><%=Request.ServerVariables["INSTANCE_META_PATH"]			%></td>
				<td>The metabase path for the instance of IIS that responds to the request.
				</td>
			</tr>
			<tr>
				<td>LOCAL_ADDR
				</td>
				<td><%=Request.ServerVariables["LOCAL_ADDR"]							%></td>
				<td>Returns the Server Address on which the request came in. This is important on 
					multi-homed machines where there can be multiple IP addresses bound to a 
					machine and you want to find out which address the request used.
				</td>
			</tr>
			<tr>
				<td>LOGON_USER
				</td>
				<td><%=Request.ServerVariables["LOGON_USER"]							%></td>
				<td>The Windows NT® account that the user is logged into.
				</td>
			</tr>
			<tr>
				<td>PATH_INFO
				</td>
				<td><%=Request.ServerVariables["PATH_INFO"]							 %></td>
				<td>Extra path information as given by the client. You can access scripts by 
					using their virtual path and the PATH_INFO server variable. If this information 
					comes from a URL, it is decoded by the server before it is passed to the CGI 
					script.
				</td>
			</tr>
			<tr>
				<td>PATH_TRANSLATED
				</td>
				<td><%=Request.ServerVariables["PATH_TRANSLATED"]				 %></td>
				<td>A translated version of PATH_INFO that takes the path and performs any 
					necessary virtual-to-physical mapping.
				</td>
			</tr>
			<tr>
				<td>QUERY_STRING
				</td>
				<td><%=Request.ServerVariables["QUERY_STRING"]						%></td>
				<td>Query information stored in the string following the question mark [?] in the 
					HTTP request.
				</td>
			</tr>
			<tr>
				<td>REMOTE_ADDR
				</td>
				<td><%=Request.ServerVariables["REMOTE_ADDR"]						 %></td>
				<td>The IP address of the remote host making the request.
				</td>
			</tr>
			<tr>
				<td>REMOTE_HOST
				</td>
				<td><%=Request.ServerVariables["REMOTE_HOST"]						 %></td>
				<td>The name of the host making the request. If the server does not have this 
					information, it will set REMOTE_ADDR and leave this empty.
				</td>
			</tr>
			<tr>
				<td>REMOTE_USER
				</td>
				<td><%=Request.ServerVariables["REMOTE_USER"]						 %></td>
				<td>Unmapped user-name string sent in by the User. This is the name that is 
					really sent by the user as opposed to the ones that are modified by any 
					authentication filter installed on the server.
				</td>
			</tr>
			<tr>
				<td>REQUEST_METHOD
				</td>
				<td><%=Request.ServerVariables["REQUEST_METHOD"]					%></td>
				<td>The method used to make the request. For HTTP, this is GET, HEAD, POST, and 
					so on.
				</td>
			</tr>
			<tr>
				<td>SCRIPT_NAME
				</td>
				<td><%=Request.ServerVariables["SCRIPT_NAME"]						 %></td>
				<td>A virtual path to the script being executed. This is used for 
					self-referencing URLs.
				</td>
			</tr>
			<tr>
				<td>SERVER_NAME
				</td>
				<td><%=Request.ServerVariables["SERVER_NAME"]						 %></td>
				<td>The server's host name, DNS alias, or IP address as it would appear in 
					self-referencing URLs.
				</td>
			</tr>
			<tr>
				<td>SERVER_PORT
				</td>
				<td><%=Request.ServerVariables["SERVER_PORT"]						 %></td>
				<td>The port number to which the request was sent.
				</td>
			</tr>
			<tr>
				<td>SERVER_PORT_SECURE
				</td>
				<td><%=Request.ServerVariables["SERVER_PORT_SECURE"]			%></td>
				<td>A string that contains either 0 or 1. If the request is being handled on the 
					secure port, then this will be 1. Otherwise, it will be 0.
				</td>
			</tr>
			<tr>
				<td>SERVER_PROTOCOL
				</td>
				<td><%=Request.ServerVariables["SERVER_PROTOCOL"]				 %></td>
				<td>The name and revision of the request information protocol. The format is 
					protocol/revision.
				</td>
			</tr>
			<tr>
				<td>SERVER_SOFTWARE
				</td>
				<td><%=Request.ServerVariables["SERVER_SOFTWARE"]				 %></td>
				<td>The name and version of the server software that answers the request and runs 
					the gateway. The format is name/version.
				</td>
			</tr>
			<tr>
				<td>URL
				</td>
				<td><%=Request.ServerVariables["URL"]										 %></td>
				<td>Gives the base portion of the URL.
				</td>
			</tr>
		</table>
		<p>&nbsp;</p>
		<p>&nbsp;</p>
		<p>&nbsp;</p>
		<p>&nbsp;</p>
		<p>&nbsp;</p>
		<hr />
		Generated
		<table style="FONT-SIZE:xx-small">

		<%for(int sc=0;sc<Request.ServerVariables.Count;sc++)
		 {
			%>
			<tr>
				<td>
					<%=Request.ServerVariables.Keys[sc] %>
				</td>
				<td>
					<%=Request.ServerVariables[sc]%>
				</td>
			</tr>
			<%
		 }
		%>
		</table>
				
		</form>
	</body>
</HTML>
