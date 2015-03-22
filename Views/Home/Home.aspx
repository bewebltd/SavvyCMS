<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.HomeController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(function () {
			$('#masthead-carousel .item').eq(0).addClass('active');
			$('#masthead-carousel').carousel({ interval: 7000 });
		});
	</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<%if (Web.Response.StatusCode==404) {    // note: you need to style this up or provide an alternative 404 page %>
		<h2>Sorry, the page or resource was not found</h2>
		<h3><%=Request["message"] %></h3><br/><br/>
	<%} %>
	
	<div id="masthead">
		<div class="container">
		<%if (Model.ShowSlideShow) {%>
			<%Html.RenderAction<HomeController>(controller => controller.Carousel()); %>
		<%} %>
		</div> <!-- /container -->
	</div> <!-- /masthead -->

	<div id="content">
		<div class="container">
		
			<div class="row">
				<div id="welcome" class="grid-12">
					<h1><%=Model.ContentPage.Introduction.FmtPlainTextAsHtml() %></h1>
				</div>
			</div> <!-- /row -->

			<hr class="row-divider" />

			<div class="row divider service-container">
				<div class="grid-3">
					<h2><span class="slash">//</span> Our Services</h2>
					<p>Maecenas a mi nibh, eu euismod orci. Vivamus viverra lacus vitae.</p>
					<a href="javascript:;" class="btn btn-small btn-warning">More Services</a>
				</div>
				
				<div class="grid-3">
					<div class="service-item">
						<h3><i class="icon-tint"></i>Website Design</h3> <!-- /service-icon -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco.</p>
						<p><a href="javascript:;" class="">Learn More »</a></p>
					</div> <!-- /service -->
				</div>
				
				<div class="grid-3">
					<div class="service-item">
						<h3><i class="icon-map-marker"></i>Mobile Development</h3> <!-- /service-icon -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco.</p>
						<p><a href="javascript:;" class="">Learn More »</a></p>
					</div>
				</div>
				
				<div class="grid-3">
					<div class="service-item">
						<h3><i class="icon-cogs"></i>Web Development</h3> <!-- /service-icon -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco.</p>
						<p><a href="javascript:;" class="">Learn More »</a></p>
					</div>			
				</div>
			</div> <!-- /row -->

			<hr class="row-divider" />

			<div class="row work-container">
				<div class="grid-3">
					<h2><span class="slash">//</span> Our Work</h2>
					<p>Maecenas a mi nibh, eu euismod orci. Vivamus viverra lacus vitae.</p>
					<a href="javascript:;" class="btn btn-small btn-warning">More Work</a>
				</div> <!-- /grid-3 -->
				
				<div class="grid-3">
					<div class="work-item">
						<h3>Portfolio Item #1</h3>
						<a class="thumbnail"><img src="<%=Web.Root %>theme/focus/images/gallery/1_small.jpg" alt="" /></a> <!-- /img -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p><a href="javascript:;" class="">View Project »</a></p>			
					</div>
				</div> <!-- /grid-3 -->
				
				<div class="grid-3">
					<div class="work-item">
						<h3>Portfolio Item #1</h3>
						<a class="thumbnail"><img src="<%=Web.Root %>theme/focus/images/gallery/2_small.jpg" alt="" /></a> <!-- /img -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p><a href="javascript:;" class="">View Project »</a></p>			
					</div>
				</div> <!-- /grid-3 -->
				
				<div class="grid-3">
					<div class="work-item">
						<h3>Portfolio Item #1</h3>
						<a class="thumbnail"><img src="<%=Web.Root %>theme/focus/images/gallery/3_small.jpg" alt="" /></a> <!-- /img -->
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
						<p><a href="javascript:;" class="">View Project »</a></p>			
					</div>
				</div> <!-- /grid-3 -->
			</div> <!-- /row -->

			<hr class="row-divider" />

			<div class="row divider about-container">
				<div class="grid-3">				
					<h2><span class="slash">//</span> Our Story</h2>
					<p>Maecenas a mi nibh, eu euismod orci. Vivamus viverra lacus vitae.</p>
					<p><a href="javascript:;" class="btn btn-small btn-warning">More Story »</a></p>
				</div> <!-- /grid-3 -->
			
				<div class="grid-4">
					<div class="about-item">
						<h3>About Us</h3>
						<p style="">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.</p>
						<p>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
						<p><a href="javascript:;">Read More »</a></p>						
					</div> <!-- /about -->
				</div> <!-- /grid-4 -->
			
				<div class="grid-5">				
					<h3>Why Choose Us</h3>

					<div class="choose-item">
						<h3><i class="icon-star"></i>Awesome #1</h3>
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
					</div> <!-- /choose-item -->
			
					<div class="choose-item">
						<h3><i class="icon-star"></i>Awesome #2</h3>
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
					</div> <!-- /choose-item -->
				
					<div class="choose-item">
						<h3><i class="icon-star"></i>Awesome #3</h3>
						<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
					</div> <!-- /choose-item -->
					<p><a href="javascript:;">More Reasons »</a></p>
				</div> <!-- /grid-5 -->
				
			</div> <!-- /row -->
		</div> <!-- /container -->
	</div> <!-- /content -->

</asp:Content>