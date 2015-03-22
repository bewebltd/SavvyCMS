<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="412.aspx.cs" Inherits="error_412" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Precondition Failed</h1>
	<p>The test passed by your browser to the server failed.</p>
	<p>If you can't see any of our pages you will need to try a different browser or fix the problem with the one you have.</p>
	<!-- 
		The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server. 
		This response code allows the client to place preconditions on the current resource metainformation (header field data) 
		and thus prevent the requested method from being applied to a resource other than the one intended.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>
