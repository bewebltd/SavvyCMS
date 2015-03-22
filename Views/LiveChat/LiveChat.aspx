<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" MasterPageFile="~/site.master" Title="" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="server">
	<h2>LiveChat</h2>

		<%Html.RenderAction<LiveChatController>(controller => controller.LiveChat()); %>

</asp:Content>
