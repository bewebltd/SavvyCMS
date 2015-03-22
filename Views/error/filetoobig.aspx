<%@ Page Title="" Language="C#" MasterPageFile="error.master" AutoEventWireup="true" CodeFile="filetoobig.aspx.cs" Inherits="filetoobig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ErrorContent" Runat="Server">
	<h1>File Too Big</h1>
	<p>You just attempted to upload a <asp:Literal ID="ThisFileSize" runat="server" /> file, please make sure your file is less than <asp:Literal ID="MaxFileSize" runat="server" />.</p>
	
	<p>Please click here to <asp:HyperLink NavigateUrl="~/default.aspx" Text="return to the website" runat="server" /></p>
</asp:Content>

