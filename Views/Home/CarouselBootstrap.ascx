<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.HomeController.SlideShowViewModel>" %>
<%@ Import Namespace="Models" %>

	<div id="masthead-carousel" class="carousel slide">
		<div class="carousel-inner">
					
			<% foreach (var slide in Model.Slides) {%>
			<div class="item">
				<%if(slide.LinkURL.IsNotBlank()){ %><a href="<%=slide.LinkURL %>" target="_blank"><%} %>
					<img src="<%=ImageProcessing.ImagePath(slide.SlidePicture) %>" alt="<%:slide.AltText %>" />
				<%if(slide.LinkURL.IsNotBlank()){ %></a><%} %>
				<div class="masthead-details">
					<h2><%:slide.Title %></h2>
					<p><%=slide.BodyText.FmtPlainTextAsHtml() %></p>
				</div>
			</div>
			<%} %>

		</div> <!-- /carousel-inner -->
				
		<a class="carousel-control left" href="#masthead-carousel" data-slide="prev">&lsaquo;</a>
		<a class="carousel-control right" href="#masthead-carousel" data-slide="next">&rsaquo;</a>
	</div> <!-- /masthead-carousel -->