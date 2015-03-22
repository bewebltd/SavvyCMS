<%@ Page Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%
Web.CacheClearAll();
Response.Redirect(Web.Root);
 %>