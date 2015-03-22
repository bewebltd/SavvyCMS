<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.ArticlePageController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<% Util.IncludeColorbox(); %>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	
	<div id="panelLeft" class="article-page">
		<div id="page-title">
			<h1><%=Fmt.BoldAsterisk(Model.ContentPage.Title)%></h1>
			<% if (Model.ContentPage.BodyTextHtml.IsNotBlank()) {%>
				<%=Model.ContentPage.BodyTextHtml.FmtHtmlText() %>
			<%} %>
		</div>
		<div id="content">
			<div class="container">			
				<% foreach (var article in Model.ContentPage.GetArticles()) { %>
					<% var isMainlyText = article.Template == Article.TEMPLATEMAINLYTEXT; %>			
					<div class="article" id="Article<%=article.ID%>">
						<% if (article.ShowArticleTitle) { %>
						<h2 class="article-title"><%= Fmt.BoldAsterisk(article.Title) %></h2>
						<% } %>
						<% if ((article.Author.IsNotBlank() && article.ShowArticleAuthor) && article.Page.ShowArticlePublishDates) { %>
							<% var author = "<b>Author:</b> "+ article.Author + " "; %>
							<% var publishedDate = (article.Page.ShowArticlePublishDates) ? "<b>&middot; Published:</b> " +article.PublishDate.FmtShortDate() : ""; %>
							<h5><%=author%><%=publishedDate %></h5>
						<% } %>
						<div class="articleBody">
							<% if(article.Picture.IsNotBlank()) {%>
									<a href="<%=ImageProcessing.ImagePath(article.Picture) %>" class="colorbox articleImage"><img src="<%=ImageProcessing.ImageSmallPath(article.Picture) %>" /></a>
								<% } %>
								<% if(article.YouTubeVideoID.IsNotBlank()) {%>
									<a href="<%="http://www.youtube.com/embed/"+ article.YouTubeVideoID + "?rel=0&autoplay=1&wmode=transparent" %>" class="gallery youtube articleImage">
										<img class="youtubeplay" src="<%=Web.Images%>youtube-play.png" />
										<img src="<%="http://img.youtube.com/vi/"+ article.YouTubeVideoID + "/0.jpg" %>" />
									</a>
								<% } %>	
							<% if (isMainlyText) {%>
								<% Html.RenderAction<CommonController>(controller => controller.Resources(article.ArticleDocuments, article.ArticleURLs,  Article.TEMPLATEMAINLYTEXT)); %>
							<%} %>
							<div class="articleText">
								<%=article.BodyTextHtml.FmtHtmlText() %>
							</div>
							<% if (!isMainlyText) {%>
								<% Html.RenderAction<CommonController>(controller => controller.Resources(article.ArticleDocuments, article.ArticleURLs, Article.TEMPLATEMAINLYRESOURCES)); %>
							<%} %>

						</div>
					</div>
				<% } %>
			</div>
		</div>
	</div>

</asp:Content>
