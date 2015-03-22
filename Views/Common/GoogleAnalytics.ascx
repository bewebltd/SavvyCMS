<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% if (Util.ServerIsLive && Util.GetSetting("GoogleAnalyticsTrackingID", "") != "") { %>
	<!-- Google Analytics -->
	<script type="text/javascript">
		var _gaq = _gaq || [];
		_gaq.push(['_setAccount', '<%=Util.GetSetting("GoogleAnalyticsTrackingID")%>']);
		_gaq.push(['_setDomainName', '<%=Util.ServerIs() %>']);
		_gaq.push(['_trackPageview']);
		(function () {
			var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
			ga.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'stats.g.doubleclick.net/dc.js';
			var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
		})();
	</script>
	<!-- End Google Analytics -->
<% } %>
