<%@ Page Title="MTL Quote" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.NewsController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Site.Controllers" %>

<%--<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
--%>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<%Util.IncludeBewebForms(); %>
	<%Util.IncludeSavvyValidate();%>
	<%Util.IncludeJavascriptFile("js/common-extras.js");%>

	<script>

		$(window).scroll(checkLoadMore);

		$(document).ready(function () {
			//$('.news-list li:last').css('border-bottom', '0');
			checkLoadMore();
		});


		function svyOnBeforeUpload() {
			if (window.console) {
				console.log("Before Upload")
			}
		}
		function svyOnAfterUpload() {
			if (window.console) {
				console.log("After Upload")
			}
		}
			

	</script>
	
	<style>
		.ulInfiniteList {
			border: 1px solid #ccc;
			list-style: none;
			padding:10px;
			margin-left: 0;
		}
		.ulInfiniteList h5 {
			font-size: 15px;
		}

		#svyFiledragLive_Attachment {
			background-image: url("images/fileUploadBg.png");	
		}

	</style>

</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">	
<%--	<ul id="miniNav">
		<li><a href="<%=Web.Root %>" class="cursor">Home</a></li>
		<li><a href="<%=Web.Root %>AboutUs" class="cursor">About</a></li>
		<li><a href="<%=Web.Root %>NewsList" class="cursor">News</a></li>
	</ul>	--%>
	<div class="wide_col_wrapper newslist_wrapper">
		<div class="top"></div>
		<div class="col1 wide_col" style="width:650px">
			<h1>
				<%=Model.ContentPage.Title.HtmlEncode() %>
				<%--<span>\ <a href="<%=Web.Root %>Info/HowItWorks">HOW IT WORKS</a></span>
				<span>\ <a href="<%=Web.Root %>Info/Overview">Overview</a></span>
				<span>\ <a href="<%=Web.Root %>AboutUs">About Us</a></span>--%>
				<%//Html.RenderAction<AboutUsController>(controller => controller.SubNav(Model.ContentPage.ID)); %>

			</h1>
			<%=Model.ContentPage.BodyTextHtml.FmtHtmlText() %>
		<%--	<ul class="news-list">
				<%foreach(var newsItem in Model.newsList) {%>
					<li>
						<h3><a href="<%=Web.Root%>News/NewsDetail?id=<%=newsItem.ID %>&title=<%=Web.Server.UrlEncode(newsItem.IntroductionText.StripTags()) %>"><%=newsItem.Title%></a></h3>
						<p class="source">Source: <a href="<%=newsItem.LinkUrl%>" target="_blank"><%=newsItem.Source%></a> <span class="date"><%=newsItem.PublishDate.FmtShortDate()%></span></p>
					</li>
				<%}%>
			</ul>--%>
		</div>
		
		<form class="AutoValidate" method="post" action="<%=Web.Root%>Register/Submit" enctype="multipart/form-data">
			<div class="chooseSimplePictureContainer">
				<label>Simple Picture Example</label>
				<%=new Forms.PictureField(Model.news.Fields.Picture,true){}%>
			</div>
			<div>
				<div class="chooseFileContainer"  style="float: left;width: 40%;">
					<label>Upload Attachment Example</label>
					<%=new Forms.AttachmentField(Model.news.Fields.Attachment,true){  AllowAjax=true,AllowDragArea = true,Subfolder = "CV",AllowedMimetypes = "doc,pdf"}%>
				</div>
				<div class="choosePictureContainer" style="float: right;width: 40%;">
					<label>Upload Picture Example</label>
					<%=new Forms.PictureField(Model.news.Fields.LargePicture,true){}%>
				</div>
			</div>

			<div style="clear:both;"></div>
			<input type="submit" value="Submit"/>
		</form>
		<div class="btm"></div>

		<input type="hidden" value="" name="nextRow" id="nextRow">
		<div id="infiniteNewsList"></div>
		
	</div>	
	
</asp:Content>
