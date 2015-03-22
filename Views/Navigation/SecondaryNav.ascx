<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>

<ul id="top_nav" class="dontprint">
	<%foreach (var page in Model.NavItems) {%>
		<li><a href="<%=page.Url%>" class="<%=page.CssClass%>" title="<%:page.Title%>" <%if (page.IsExternalUrl){%>target="_blank"<%} %>><%:page.Title%></a></li>
	<%}	%>								
</ul>	

