<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="401.aspx.cs" Inherits="error_401" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Unauthorised</h1>
	<p>You got your username/password wrong, and we can't show you the page you were trying to get to.</p>
	<!-- 
	The request requires user authentication. The response MUST include a WWW-Authenticate header field (section 14.47) containing a challenge 
	applicable to the requested resource. The client MAY repeat the request with a suitable Authorization header field (section 14.8). 
	If the request already included Authorization credentials, then the 401 response indicates that authorization has been refused for those 
	credentials. If the 401 response contains the same challenge as the prior response, and the user agent has already attempted authentication 
	at least once, then the user SHOULD be presented the entity that was given in the response, since that entity might include relevant 
	diagnostic information. HTTP access authentication is explained in "HTTP Authentication: Basic and Digest Access Authentication" [43].
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>

</asp:Content>

