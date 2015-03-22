<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.CoolVideosController.ViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%=Web.Root %>js/swfobject/swfobject.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$(".video-list li:last-child").addClass("last");
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
	<div class="center-col-normal" style="width: 694px;">
		<div class="box_outer_border">
			<div class="box_inner_border">
				<div class="video-intro">
					<h1 class="contact pageTitle">
						<%:Model.ContentPage.Title %></h1>
					<p>
						<%:Model.ContentPage.Introduction %></p>
					<%--<div class="featuredbike">
						<% if(Model.CurrentVideo.BikeModel!=null) {%>
													<%=Beweb.Html.PicturePreview(Model.CurrentVideo.BikeModel.Fields.ModelPicture, Model.CurrentVideo.BikeModel.Title) %>
													<h2>FEATURED BIKE - <em><%:Model.CurrentVideo.BikeModel.Title %></em></h2>
													<p><%=Fmt.TruncHTML(Model.CurrentVideo.BikeModel.OverviewTextHTML.StripTags(),100)%>...</p>
													<a href="<%=Web.Root %>BikeModel/<%=Model.CurrentVideo.BikeModelID %>/<%=PathAndFile.CrunchFileName(Model.CurrentVideo.BikeModel.Title) %>" class="btn_standard viewbike_btn" style="float:left;margin-top:7px;">View Bike<span></span></a>
													<div class="clear"></div>
													<hr />
												<%}%>
					</div>--%>
				</div>
				<div class="coolvideos-wrapper">
					<div id="flashwrap">
					</div>
					<script type="text/javascript">
						var swfFileName = "http://www.youtube.com/v/<%=Model.CurrentVideo.VideoCode%>?fs=1&amp;hl=en_US&amp;rel=0&amp;version=3"
						var swfWidth = "482"
						var swfHeight = "296"
						var flashVersion = "10.0.0"
						var divToReplace = "flashwrap"
						var expressInstall = "<%=Web.Root %>swf/expressInstall.swf"
						var flashvars = {};
						var params = {};
						params.menu = "false"
						params.bgcolor = "#FFFFFF"
						params.allowfullscreen = "true"
						params.allowscriptaccess = "always"
						var attributes = {};
						attributes.menu = "false"
						attributes.bgcolor = "#FFFFFF"
						attributes.allowfullscreen = "true"
						attributes.allowscriptaccess = "always"
						swfobject.embedSWF(swfFileName, divToReplace, swfWidth, swfHeight, flashVersion, expressInstall, flashvars, params, attributes);
					</script>
					<h4 class="video-title">
						<%:Model.CurrentVideo.Title %></h4>
					<div class="video-details">
						<p class="video-posted">
							Posted:
							<%=Model.CurrentVideo.VideoPostedDate.FmtDate() %></p>
						<%--<p class="video-views"><%=Model.CurrentVideo.ViewCount%> views</p>--%>
						<p class="video-views">
							Author:
							<%:Model.CurrentVideo.Credit%></p>
						<div class="clear">
						</div>
					</div>
					<p>
						<%=Model.CurrentVideo.VideoDescription.FmtPlainTextAsHtml() %></p>
					<iframe class="likeiframe" src="http://www.facebook.com/plugins/like.php?href=<%:Web.PageUrl+"?video="+Model.CurrentVideo.ID%>&amp;layout=standard&amp;show_faces=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=26"
						scrolling="no" frameborder="0" allowtransparency="true"></iframe>
				</div>
				<div class="clear">
				</div>
			</div>
		</div>
	</div>
	<div class="right-col-normal">
		<div class="box_outer_border video-box">
			<div class="box_inner_border">
				<h2>
					<span class="instruction">Select a video to watch</span>VIDEO CLIPS</h2>
				<%
					var scan = 1;
					var currentMonth = "";
					var thisVideoMonth = "";
					foreach (var video in Model.LatestVideos) {
						thisVideoMonth = Fmt.LongDate(video.VideoPostedDate).Split(" ".ToCharArray())[1];
						if (thisVideoMonth != currentMonth) {
							currentMonth = Fmt.LongDate(video.VideoPostedDate).Split(" ".ToCharArray())[1];
							if (scan > 1) {
				%></ul><%
						}%>
				<h5>
					<%=currentMonth +" "+ video.VideoPostedDate.Value.Year %></h5>
				<ul class="video-list">
					<%}%>
					<li><a href="<%=Web.PageUrl%>?video=<%=video.ID%>">
						<img src="<%=video.ThumbnailUrl%>" alt="<%:video.Title %>" class="video-thumb" /></a>
						<p class="video-title">
							<%:video.Title %></p>
						<%-- <p class="video-count"><%:video.ViewCount %> views</p>--%>
						<a href="<%=Web.PageUrl%>?video=<%=video.ID%>" class="view">View Video</a>
						<div class="clear">
						</div>
					</li>
					<%
					scan++;
				} 
					%>
				</ul>
				<%if (Model.ShowNextPage) { %>
				<a href="CoolVideos?pagenum=<%=Model.PageNum+1%>" class="view" style="float: right;
					margin-top: 8px; margin-bottom: 5px;">Next page of videos</a>
				<%} %>
				<%if (Model.ShowPrevPage) { %>
				<a href="CoolVideos?pagenum=<%=Model.PageNum-1%>" class="view" style="float: right;
					margin-top: 8px; margin-bottom: 5px;">Previous page of videos</a>
				<%} %>
				<div class="clear">
				</div>
			</div>
		</div>
	</div>
</asp:Content>
