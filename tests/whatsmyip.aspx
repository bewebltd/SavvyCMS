<%@ Page Language="C#" %>
<%@ Import Namespace="Beweb"%>
<h1>This server's IP is showing to the outside world as...</h1>
<%
string html = Http.Get("http://whatismyipaddress.com");

Response.Write(html.ExtractTextBetween("<!-- do not script -->", "<!-- do not script -->"));
 %>