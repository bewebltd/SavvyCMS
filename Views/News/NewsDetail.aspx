<%@ Page Title="MTL Quote" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.NewsController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Site.Controllers" %>

<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
--%><asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
	
	</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
<%--	<ul id="miniNav">
		<li><a href="<%=Web.Root %>" class="cursor">Home</a></li>
		<li><a href="<%=Web.Root %>" class="cursor">About</a></li>
		<li><a href="<%=Web.Root %>NewsList" class="cursor">News</a></li>
	</ul>	--%>
	<%var newsItem = Model.news; %>
	<div class="wide_col_wrapper news_details">	
		<div class="top"></div>
		<div class="col1 wide_col" style="width:650px">
			<h2 class="alt">
				<h1>
				<%=Model.ContentPage.Title.HtmlEncode() %>
				<%--<span>\ <a href="<%=Web.Root %>Info/HowItWorks">HOW IT WORKS</a></span>
				<span>\ <a href="<%=Web.Root %>Info/Overview">Overview</a></span>
				<span>\ <a href="<%=Web.Root %>AboutUs">About Us</a></span>--%>
				<%//Html.RenderAction<AboutUsController>(controller => controller.SubNav(Model.ContentPage.ID)); %>

			</h1>
			</h2>
			<%if (Model.news != null) {%>
				<h1 class="alt"><%=Model.news.IntroductionText.FmtPlainTextAsHtml()%></h1>
				<p class="source">Source: <a href="<%=newsItem.LinkUrl%>" target="_blank"><%=newsItem.Source%></a><span class="date"> <%=newsItem.PublishDate.FmtShortDate()%></span></p>
				<div class="details">
					<%=newsItem.BodyTextHtml.FmtHtmlText()%>
					<p class="readmore">Read more: <a href="<%=newsItem.LinkUrl%>" target="_blank">Click here</a></p>
				</div>
				<div class="left">
				<%if (newsItem.Picture.IsNotBlank()) {%>
				<img src="<%=Web.Root + "attachments/" + newsItem.Picture%>" width="230" height="230" alt="" />
				<br/>
				<% } %>
				<ul class="news_list_small">
				<%foreach(var news in Model.newsList) {%>
					<li>
						<a href="<%=Web.Root%>News/?id=<%=news.ID %>&title=<%=Web.Server.UrlEncode(news.IntroductionText.StripTags()) %>"><%=news.IntroductionText%></a>
						<p class="date"><%=news.PublishDate.FmtShortDate()%></p>
						<div class="clear"></div>
					</li>
				<%}%>
			</ul>
			<%} else {%>
				Not available
			<%}%>
			</div>
			<div class="clear" style="height:30px;"></div>
			<%if (Model.prevNews != null) { %>
				<a class="btn_older" href="<%=Web.Root%>News/NewsDetail?id=<%=Model.prevNews.ID%>&title=<%=Web.Server.UrlEncode(Model.prevNews.IntroductionText.StripTags()) %>"></a>
			<% } %>
			<a class="btn_backtolist" href="<%=Web.Root %>News"></a>
			<%if (Model.nextNews != null) { %>
				<a class="btn_newer" href="<%=Web.Root%>News/NewsDetail?id=<%=Model.nextNews.ID %>&title=<%=Web.Server.UrlEncode(Model.nextNews.IntroductionText.StripTags()) %>"></a>
			<% } %>
		</div>
		<div class="btm"></div>
	</div>
</asp:Content>
