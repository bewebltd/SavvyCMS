<%@ Page Language="C#" MasterPageFile="help.master" AutoEventWireup="true" CodeFile="culturecodes.aspx.cs" Inherits="admin_help_culturecodes" Title="" %>
<%@ Import Namespace="System.Globalization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<style>
		pre {
			font-family:"Lucida Console", Monaco, monospace
		}

	</style>
	<div id="content">
		<%
		List<string> list = new List<string>();
		foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures)){
			string specName = "(none)";
			try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
			list.Add(String.Format("{0,-12}{1,-12}{2}", ci.Name, specName, ci.EnglishName));
		}

		list.Sort();  // sort by name
		%>
		<pre>
CULTURE   SPEC.CULTURE  ENGLISH NAME
--------------------------------------------------------------
<%foreach (string str in list) {%>
<%=str %>

<%}%>
		</pre>
	</div>
</asp:Content>

