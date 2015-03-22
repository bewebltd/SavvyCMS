<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.HomeController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<script>

	</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<div class="likeus">
    <p class="likeus-message">Like us to enter</p>
    <h1 class="logo"><img src="<%=ResolveUrl("~/images/logo_shadow.png")%>" /></h1>
	</div>
</asp:Content>