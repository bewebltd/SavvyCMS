<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="406.aspx.cs" Inherits="error_406" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>Not Acceptable</h1>
	<p>Our server can't send you the type of page your browser has asked for.</p>
	<p>If you can't see any of our pages you will need to try a different browser or fix the problem with the one you have.</p>
	<!-- 
		The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.

		Unless it was a HEAD request, the response SHOULD include an entity containing a list of available entity characteristics and location(s) from which the user or user agent can choose the one most appropriate. The entity format is specified by the media type given in the Content-Type header field. Depending upon the format and the capabilities of the user agent, selection of the most appropriate choice MAY be performed automatically. However, this specification does not define any standard for such automatic selection.

			  Note: HTTP/1.1 servers are allowed to return responses which are
			  not acceptable according to the accept headers sent in the
			  request. In some cases, this may even be preferable to sending a
			  406 response. User agents are encouraged to inspect the headers of
			  an incoming response to determine if it is acceptable.
		If the response could be unacceptable, a user agent SHOULD temporarily stop receipt of more data and query the user for a decision on further actions.
	-->
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

