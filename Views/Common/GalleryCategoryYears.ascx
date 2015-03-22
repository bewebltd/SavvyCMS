<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.CommonController.GalleryCategoryYearsViewModel>" %>
<% foreach (var thisYear in Model.ContentPage.GalleryCategoryYears(Model.ContentPage.PageID)) { %>
	<% if (thisYear.Categories.Count > 0) {%>
	<div class="gallery-category">
		<h2 class="gallery-category-year"><%=thisYear.Title %></h2>
		<ul>
			<% foreach (var category in thisYear.Categories) {%>
				<li>
					<a href="<%=category.GetUrl() %>">
						<img src="<%=category.CoverImage.IsNotBlank() ? category.CoverImage : Web.Images + "placeholder-galleryimage.png"%>" width="200" height="150" />
						<h3><b><%=category.Title %></b></h3>
						<div class="date"><%=Fmt.Date(category.PublishDate) %></div>
					</a>
				</li>
			<% } %>
		</ul>
	</div>
	<%} %>
<% } %>