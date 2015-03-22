<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="405.aspx.cs" Inherits="error_405" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Method Not Allowed</h1>
	<p>Something dodgy was passed by your browser, that our server didn't like.</p>
	<p>If you can't see any of our pages you will need to try a different browser or fix the problem with the one you have.</p>
	<!-- 
		The method specified in the Request-Line is not allowed for the resource identified by the Request-URI. 
		The response MUST include an Allow header containing a list of valid methods for the requested resource.	
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

