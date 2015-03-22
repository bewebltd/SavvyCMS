<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>
<ul id="footer_nav">
	<%foreach (var navItem in Model.NavItems) { %>
		<li><a href="<%=navItem.Url%>" title="<%:navItem.Title%>"<% if(!String.IsNullOrEmpty(navItem.CssClass)){%> class="<%=navItem.CssClass%>"<%} if (navItem.IsExternalUrl){%>target="_blank"<%} %>><%:navItem.Title%></a></li>
	<%}%>
</ul>
