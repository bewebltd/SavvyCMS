<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>
<ul id="main_nav">
	<%foreach (var navItem in Model.NavItems) {%>
		<li class="<%=navItem.CssClass%>"><a href="<%=navItem.Url%>"<%if(navItem.IsExternalUrl){%> target="_blank"<%} %>><%:navItem.Title%><span></span></a></li>
	<%} %>
</ul>
