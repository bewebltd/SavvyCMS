<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="501.aspx.cs" Inherits="error_501" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Not Implemented</h1>
	<p>The server does not know how to process what you have just asked for.</p>
	<!-- 
		The server does not support the functionality required to fulfill the request. 
		This is the appropriate response when the server does not recognize the request method and is not capable of supporting it for any resource.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

