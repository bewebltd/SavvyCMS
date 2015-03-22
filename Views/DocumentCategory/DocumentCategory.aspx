<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.DocumentCategoryController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<div class="std-page documentcategory">
		<div id="breadcrumb"><%=Model.CategoryBreadCrumb %></div>
		<div class="introduction">
			<h2><%:Model.CategoryTitle %></h2>
			<p><%:Model.Category.IntroText %></p>
		</div>
		<hr class="row-divider" />
			<%Html.RenderAction<CommonController>(c => c.DocumentCategory(Model.Category));%>
		<div class="clear"></div>
	</div>
</asp:Content>
