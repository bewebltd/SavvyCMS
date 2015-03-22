<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SearchController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %>
<%@ Import Namespace="Site.Controllers" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
			
		

	<style>
		/* Search Results Styles */
		.result{width:610px;}
		.result-wrapper{ float: left; margin-left: 19px;}
		.result{ border-bottom: 1px solid #dfdfdf; width: 483px; padding-bottom: 20px; margin-bottom: 10px;}
		.result h2{font-size: 16px;}
		.result .text{ padding-right: 25px;}
		.result .text a{ color: #c70001; text-decoration:underline; }
	</style>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<div id="searchWrapper">
		<div class="row clear">
				<div class="mainColumn">
					<div class="inner">
						<h1>Search results</h1>
				<%//=Model.ContentPage.BodyTextHtml %>
				<form name="SearchForm" id="SearchAgainForm" style="display: none;" method="get" action="<%=Url.Action("Search", "Search") %>">
					<%=Beweb.Html.HiddenPageNumField() %>
					<%=new Forms.HiddenField("searchText",Web.Request["SearchText"]) %>
					<%=new Forms.HiddenField("SearchArea", Model.SearchArea)%>
				</form>
				<div class="paging">
					<%if (Model.SearchArea!="All") {%>
						<%=Beweb.Html.PagingNav(Model.NumPages)%>
					<%} %>
				</div>
				<div class="clear"></div>
				<div class="searchResults">
					<%--<p>Your search for <%:Model.SearchText%> matches <%=Model.Counter %> item<%if(Model.Counter == 0 || Model.Counter>1){ %>s<%} %> </p><br>--%>
						<%if (Model.EventResults.Count > 0 ) { %>
							<%--Showing results 1 to 10--%>
							<%--<h2>Events: <small><%=Model.EventResults.TotalRowCount%> found</small>--%>			 
							<h2>Events: <small><%=Model.EventResults.Count%> found</small></h2>
							<%if (Model.SearchArea=="All" && Model.EventResults.TotalRowCount > Model.EventResults.Count) {%>
								<a class="moreinfo" href="<%=Web.Root%>Search?SearchArea=Events&searchtext=<%=Model.SearchText.UrlEncode()%>">Show All &gt;</a>		
							<%} %>				
							
							<ul>
						
								<%foreach (var result in Model.EventResults) { %>				
									<div class="newsItem clear">
										<p>
											<a href="<%= result.GetUrl() %>" class="searchTitle"><%= result.Title %></a>
											<i><%=Fmt.TruncHTML(result.Description.StripTags(), 200)%>&hellip;&nbsp;</i> 
											<a href="<%= result.GetUrl() %>" class="readMore">MORE&gt;</a>
										</p>
										<%--<a href="<%= result.GetUrl() %>"><img src="<%= ImageProcessing.ImageThumbPath(result.Picture) %>" alt="" /></a>--%>
									</div>
							

								<%} %>
							</ul>
						<% } %>
						<%if (Model.PageResults.Count > 0 ) { %>
							<%--Showing results 1 to 10--%>
							<%--<h2>Pages: <small><%=Model.PageResults.TotalRowCount%> found</small>--%>						
							<h2>Pages: <small><%=Model.PageResults.Count%> found</small></h2>
							<%if (Model.SearchArea=="All" && Model.PageResults.TotalRowCount > Model.PageResults.Count) {%>
								<a class="moreinfo" href="<%=Web.Root%>Search?SearchArea=Pages&searchtext=<%=Model.SearchText.UrlEncode()%>">Show All &gt;</a>		
							<%} %>				
							
							<ul>
						
								<%foreach (var result in Model.PageResults) { %>				
									<div class="newsItem clear">
										<p>
											<a href="<%= result.GetUrl() %>" class="searchTitle"><%= result.Title %></a>
											<i><%=Fmt.TruncHTML(result.SearchResultsText.StripTags(), 200)%>&hellip;&nbsp;</i> 
											<a href="<%= result.GetUrl() %>" class="readMore">MORE&gt;</a>
										</p>
										<a href="<%= result.GetUrl() %>"><img src="<%= ImageProcessing.ImageThumbPath(result.Picture) %>" alt="" /></a>
									</div>
							

								<%} %>
							</ul>
						<% } %>
						
						<%if (Model.NewsResults.Count > 0 ) { %>
							<%--Showing results 1 to 10--%>
							<%--<h2>Newss: <small><%=Model.NewsResults.TotalRowCount%> found</small>--%>						
							<h2>News: <small><%=Model.NewsResults.Count%> found</small></h2>
							<%if (Model.SearchArea=="All" && Model.NewsResults.TotalRowCount > Model.NewsResults.Count) {%>
								<a class="moreinfo" href="<%=Web.Root%>Search?SearchArea=News&searchtext=<%=Model.SearchText.UrlEncode()%>">Show All &gt;</a>		
							<%} %>				
							
							<ul>
						
								<%foreach (var result in Model.NewsResults) { %>				
									<div class="newsItem clear">
										<p>
											<a href="<%= result.GetUrl() %>" class="searchTitle"><%= result.Title %></a>
											<i><%=Fmt.TruncHTML(result.BodyTextHtml.StripTags(), 200)%>&hellip;&nbsp;</i> 
											<a href="<%= result.GetUrl() %>" class="readMore">MORE&gt;</a>
										</p>
										<a href="<%= result.GetUrl() %>"><img src="<%= ImageProcessing.ImageThumbPath(result.Picture) %>" alt="" /></a>
									</div>
							

								<%} %>
							</ul>
						<% } %>
					</div>	
				</div>
				<div class="pagination-wrapper pagingnav-light-theme">
					
				</div>
			</div>
		</div>
</asp:Content>
