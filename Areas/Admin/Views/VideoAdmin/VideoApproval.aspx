<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Models.VideoList>"
	MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Site.Controllers" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript" src="<%=Web.Root %>js/swfobject/swfobject.js"></script>
	<script type="text/javascript" src="<%=Web.Root %>js/colorbox/jquery.colorbox.js"></script>
	<link media="screen" rel="stylesheet" href="<%=Web.Root %>js/colorbox/colorbox.css" /> 
	<style>
	body, * { margin: 0px; }
	</style>
	<script type="text/javascript">
		var lastSelection = null;
		$(document).ready(function () {

			$("div img").colorbox({ inline: true, href: "#flashwrap", height: "360px", width: "510px" });

			$("div img").click(function () {
				//$(this).colorbox({ inline: true, href: "#flashwrap" });
				$(this).css("border", "2px solid red").css("padding", "2px");

				var videoCode = $(this).attr('name');

				if (lastSelection != null) {
					var prevItem = lastSelection;
					$("#section_" + prevItem + " img").css("border", "1px solid black").css("padding", "0px");
				}

				var currentItem = $(this).attr('alt');
				lastSelection = currentItem;

				var swfFileName = "http://www.youtube.com/v/" + videoCode + "?fs=1&amp;hl=en_US&amp;rel=0"
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
			});
		});

		function ApproveVideo(VideoID) {

			var qs = "" + "VID=" + VideoID + "" + "&Approved=" + $('input[name="approveVideo"]:checked').val();

			var url = "<%=Web.BaseUrl%>admin/VideoAdmin/ApproveVid";
			$.ajax({
				type: "POST",
				url: url,
				data: qs,
				success: function (data) {
					$("#section_"+data).fadeOut();
				},
				error: function (data) {
					alert(data.responseText);
				}
			});
		}

	</script>

</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">

<p>YouTube was last scanned at <%=YouTubeSpiderController.LastSpiderCheck.GetValueOrDefault(new DateTime(1970,1,1)).FmtDateTime() %> and will be scanned again at <%=YouTubeSpiderController.NextSpiderCheck.FmtDateTime() %>. <a href="<%=Web.Root %>YouTubeSpider/RunSpiderTask">Scan again</a>.</p>
<br>

<div style='display:none;'>
	<div id="flashwrap"></div>
</div>

<%foreach (var vid in Model) {%>
	<div id="section_<%=vid.ID%>" style="margin-bottom:20px; width:880px ">
		<img src="<%=vid.ThumbnailUrl %>" name="<%=vid.VideoCode %>" width="120" height="90" alt="<%=vid.ID%>" style="cursor: pointer;float:left;border:1px solid black;" />
		<div style="float:right;margin-left:20px;width:200px;">
			<<%--% if(vid.BikeModel!=null){ %>
				<b>Model:</b> <%=vid.BikeModel.Title%><br />
				(has <%=vid.BikeModel.Videos.Filter(v=>v.Status=="Approved").Count%> approved videos)<br />
				(has <%=vid.BikeModel.Videos.Filter(v=>v.Status=="New").Count%> new videos found)
			<% }else{%>
				No bike model found.
			<%}%>--%>
		</div>
		<div style="margin-left:140px;width:500px;">
			<p style="margin:4px 0;"><b>Title:</b> <%=vid.Title %></p>
			<p style="margin:4px 0;"><b>Description:</b> <%=vid.VideoDescription %></p>
			<p style="margin:4px 0;">
				<b>Credit:</b> <%=vid.Credit %> 
				&nbsp; &nbsp; <b>Posted:</b> <%=Fmt.Date(vid.VideoPostedDate)%>
			</p>
			<label><input type="radio" value="true" onclick="ApproveVideo(<%=vid.ID%>);return false" name="approveVideo" /><b style="color:Green;">Approve</b></label>
			<label><input type="radio" value="false" onclick="ApproveVideo(<%=vid.ID%>);return false" name="approveVideo" /><b style="color:Red;">Reject</b></label>
			<span style="margin-left:20px;"><%=Html.ActionLink("Edit", "Edit", new { id=vid.ID }) %></span>
		</div>
	</div>
	<div style="clear:both"></div>

<%} %>

<div style="clear:both;"></div>
<a href="<%=Web.Root%>admin/adminmenu"><b><< BACK</b></a>
<a href="#" style="margin-left:30px;"><b>TOP^</b></a>
</asp:Content>
