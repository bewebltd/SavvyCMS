<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="404.aspx.cs" Inherits="error_404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Page Not Found</h1>
	<!-- 
	The server has not found anything matching the Request-URI. No indication is given of whether the condition is 
	temporary or permanent. The 410 (Gone) status code SHOULD be used if the server knows, through some internally 
	configurable mechanism, that an old resource is permanently unavailable and has no forwarding address. 
	This status code is commonly used when the server does not wish to reveal exactly why the request has been 
	refused, or when no other response is applicable.
	-->
	<div class="normal">
	<p>Sorry, we couldn’t find the page you’re looking for. It may have been deleted or removed.</p>

	<p>Why don’t you try:</p>
	<ul>
		<li><a href="<%=Web.BaseUrl%>">Home Page</a></li>
	</ul>
	<br>
	</div>
	<asp:Literal ID="EmailMessage" Visible="false" runat="server"><p>Email not sent, check the settings (to address, host etc) to view the details</p></asp:Literal>
</asp:Content>

