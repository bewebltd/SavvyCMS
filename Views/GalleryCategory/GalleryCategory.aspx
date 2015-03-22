<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.GalleryCategoryController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<% Util.IncludeColorbox(); %>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	
	<div id="panelLeft">
		<div id="page-title">
			<h1><%=Fmt.BoldAsterisk(Model.Category.Title)%></h1>
			<% if (Model.Category.BodyTextHtml.IsNotBlank()) {%>
				<%=Model.Category.BodyTextHtml.FmtHtmlText() %>
			<%} %>
		</div>
		<div id="content">
			<div class="container">
				<ul class="gallery-images">
					<% foreach (var image in Model.Category.GalleryImages) { %>
						<% var target = (image.MediaType == GalleryImage.GALLERYIMAGEMEDIATYPEPHOTO) ? ImageProcessing.ImagePath(image.Picture) : "http://www.youtube.com/embed/"+ image.YouTubeVideoID + "?rel=0&autoplay=1&wmode=transparent" ; %>
						<% var preview = (image.MediaType == GalleryImage.GALLERYIMAGEMEDIATYPEPHOTO) ? ImageProcessing.ImageSmallPath(image.Picture) : "http://img.youtube.com/vi/"+ image.YouTubeVideoID + "/0.jpg" ; %>
						<% var style = image.MediaType.ToLower(); %>
						<li>
							<a href="<%=target %>" class="gallery <%=style %>" data-title="<b><%:image.Title %></b><br/><small><%=Fmt.Date(image.DateTaken) %></small><br/><%:image.PictureCaption %>">
								<img src="<%=preview %>" width="200" height="150"  />
								<h3><b><%=image.Title %></b></h3>
								<div class="date"><%=Fmt.Date(image.DateTaken) %></div>
							</a>
						</li>
					<% } %>
				</ul>
			</div>
		</div>
	</div>

</asp:Content>
