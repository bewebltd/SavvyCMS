<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="403.aspx.cs" Inherits="error_403" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Forbidden</h1>
	<p>The server has refused to serve the page you requested.</p>
	<!-- 
	The server understood the request, but is refusing to fulfill it. Authorization will not help and the 
	request SHOULD NOT be repeated. If the request method was not HEAD and the server wishes to make public 
	why the request has not been fulfilled, it SHOULD describe the reason for the refusal in the entity. 
	If the server does not wish to make this information available to the client, the status code 
	404 (Not Found) can be used instead.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>

</asp:Content>

