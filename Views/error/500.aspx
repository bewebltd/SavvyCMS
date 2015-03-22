<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="500.aspx.cs" Inherits="error_500" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Oh no... Server Error</h1>
	<p>There has been a problem with the web site. The developers have been notified and will look into the problem immediately.</p>
	
	<asp:Literal ID="EmailMessage" Visible="false" runat="server"><p>Email not sent, check the settings (to address, host etc) to view the details.</p></asp:Literal>
	<asp:Literal ID="ErrorMessage" Visible="false" runat="server" />
	<asp:Literal ID="ServerIs" Visible="false" runat="server" />
	
	<!-- 
	The server encountered an unexpected condition which prevented it from fulfilling the request.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

