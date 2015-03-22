<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.StandardPageController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">

	<div id="page-title">
		<h1><%:Model.ContentPage.Title%></h1>
		<p><%=Model.ContentPage.Introduction.FmtPlainTextAsHtml() %></p>
	</div>

	<div id="content">
		<div class="container">
			<div class="row">
				<div class="grid-12">
					<%=Model.ContentPage.BodyTextHtml.FmtHtmlText() %>
				</div>
			</div>
			<hr class="row-divider" />
			<% if (Model.HasDocumentCategories) {%>
				<div class="row">
					<div class="grid-12">
						<%Html.RenderAction<CommonController>(c => c.DocumentCategoryRoot(Model.ContentPage));%>
					</div>
				</div>
				<% } %>

		</div>
		<% Html.RenderAction<CommonController>(controller => controller.GalleryCategoryYears(Model.ContentPage)); %>
	</div>

</asp:Content>
