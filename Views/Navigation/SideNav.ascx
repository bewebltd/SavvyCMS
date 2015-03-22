<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.SideNavViewModel>" %>

<div><%:Model.PanelTitle %></div>
<ul>
	<%foreach (var navItem in Model.NavItems) {%>
		<li class="<%=navItem.CssClass%>"><a href="<%=navItem.Url%>"<%if(navItem.IsExternalUrl){%> target="_blank"<%} %>><%:navItem.Title%><span></span></a></li>
		<%foreach (var subItem in navItem.SubPages) {%>
			<li class="<%=subItem.CssClass%> sub"><a href="<%=subItem.Url%>"<%if(subItem.IsExternalUrl){%> target="_blank"<%} %>><%:subItem.Title%><span></span></a></li>
		<%} %>
	<%} %>
</ul>
