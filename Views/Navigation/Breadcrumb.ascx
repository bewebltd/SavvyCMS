<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>
<% var position = 1; %>
<div class="breadcrumb">
	<ol itemscope itemtype="http://schema.org/BreadcrumbList">
		<%foreach (var page in Model.NavItems) { %>
			<li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem">
				<a itemprop="item" href="<%=page.Url%>" title="<%:page.Title%>"><span itemprop="name"><%:page.Title%></span></a>
				<meta itemprop="position" content="<%=position%>" />
			</li>
			<% position++; %>
		<% } %>
		<li itemprop="itemListElement" itemscope itemtype="http://schema.org/ListItem">
			<span itemprop="item"><%:ViewData["BreadcrumbCurrentPageTitle"] + ""%></span>
			<meta itemprop="position" content="<%=position%>" />
		</li>
	</ol>
</div>