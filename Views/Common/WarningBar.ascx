<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
	<%if(true){ %>
		<%if (Web.IsOldBrowser) {%>
			<div id=buorg class=buorg>
				<div>Your browser (<%=Web.Request.Browser.Browser %> <%=Web.Request.Browser.MajorVersion %>) is <b>out of date</b>. It has known <b>security flaws</b> and may <b>not display all features</b> of this and other websites. <a href="http://www.google.com/chrome">Update your browser</a>
					<div id=buorgclose onclick="$('#buorg').slideUp()">X</div>
				</div>
			</div>
		<%}%>

		<%if (Util.ServerIsStaging) {%>
			<script>
				$(document).ready(function() {
					$(".StagingServer .close").click(function (e) {
						$(".StagingServer").hide();
					})
				});
			</script>
			<%if (BewebData.GetConnectionString("ConnectionString")==BewebData.GetConnectionString("ConnectionStringLVE")) {%>
				<div class="StagingServer ConnLVE"><b>STAGING SERVER</b> - connected to live database. Treat all data as real. Changes will not show on live immediately. <div class="close">[X]</div></div>
			<%} else {%>
				<div class="StagingServer"><b>STAGING SERVER</b> - connected to staging database. Treat all data as sandbox data.<div class="close">[X]</div></div>
			<%}%>
		<%}%>
		<%if (Util.ServerIsDev) {%>
			<%if (BewebData.GetConnectionString("ConnectionString")==BewebData.GetConnectionString("ConnectionStringLVE")) {%>
				<div class="StagingServer ConnLVE"><b>DEV SERVER</b> - connected to live database! Treat all data as real.<div class="close">[X]</div></div>
			<%}%>
		<%}%>
		<%if (Web.IsOldBrowser) {%>
			<div class="OldBrowser"><b>BROWSER NOT SUPPORTED</b> - Your browser is out of date. It may not display all features of this website. <A href="http://browser-update.org/en/update.html#5">Learn how to update your browser</A></div>
		<%}%>


<%--
		<DIV>Your browser (Internet Explorer 8) is <B>out of date</B>. It has known <B>security flaws</B> and may <B>not display all features</B> of this and other websites. <A href="http://browser-update.org/en/update.html#5">Learn how to update your browser</A>
<DIV id=buorgclose>X</DIV></DIV>
--%>
		<%--														
		 <script type="text/javascript">
		 	var $buoop = { vs: { i: 8, f: 3.6, o: 10.6, s: 4, n: 9} }
		 	$buoop.ol = window.onload;
		 	window.onload = function () {
		 		try { if ($buoop.ol) $buoop.ol(); } catch (e) { }
		 		var e = document.createElement("script");
		 		e.setAttribute("type", "text/javascript");
		 		e.setAttribute("src", "http://browser-update.org/update.js");
		 		document.body.appendChild(e);
		 	}  
</script> 
--%>
	<%} %>