<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.NavigationController.NavViewModel>" %>

<div id="nav" class="clearfix">
	<div class="container">
		<ul id="main-nav">
		<%foreach (var navItem in Model.NavItems) {%>
			<li class="<%=navItem.CssClass%>"><a href="<%=navItem.Url%>"<%if(navItem.IsExternalUrl){%> target="_blank"<%} %>><%:navItem.Title%></a></li>
		<%} %>
		</ul>
	</div> <!-- /container -->
</div> <!-- /nav -->

<%--<div id="nav" class="clearfix">
	<div class="container">
		<ul id="main-nav">
			<li class="active"><a href="<%=Web.Root %>">Home</a></li>
			<li><a href="<%=Web.Root %>home/about">About</a></li>
			<li><a href="<%=Web.Root %>home/service">Services</a></li>
			<li><a href="<%=Web.Root %>home/pricing">Pricing</a></li>
			<li><a href="<%=Web.Root %>home/Faq">Faq</a></li>
			<li><a href="<%=Web.Root %>ContactUs">Contact</a></li>
			<li class="dropdown">
				<a href="javascript:;" data-toggle="dropdown">Dropdown<span class="caret"></span></a>
				<ul class="dropdown-menu">
					<li><a href="javascript:;"><i class="icon-home"></i> Dropdown #1</a></li>
					<li><a href="javascript:;"><i class="icon-beaker"></i> Dropdown #2</a></li>
					<li><a href="javascript:;"><i class="icon-bullhorn"></i> Dropdown #3</a></li>
					<li><a href="javascript:;"><i class="icon-cloud"></i> Dropdown #4</a></li>
				</ul>
			</li>
		</ul>
	</div> <!-- /container -->
</div> <!-- /nav -->--%>
