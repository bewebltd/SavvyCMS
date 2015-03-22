<%@ Page Language="C#" AutoEventWireup="true" CodeFile="listfiles.aspx.cs" Inherits="Site.tests.listfiles" %>
<%if (Request["show"]!="orphans"){ %>
	<b>All files</b> | <a href="listfiles.aspx?show=orphans">Orphans (files not in manifests)</a>
<%}else{ %>
	<a href="listfiles.aspx">All files</a> | <b>Orphans (files not in manifests)</b>
<%}%><br />
<pre>
<%ProcessDir(Server.MapPath("~/"), 0);  %>
</pre>