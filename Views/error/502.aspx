<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="502.aspx.cs" Inherits="error_502" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Bad Gateway</h1>
	<p>The page or service the server attempted to get for you had a problem.</p>
	<!-- 
		The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

