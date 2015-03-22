<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Models.Page>" %>
<%@ Import Namespace="Site.SiteCustom" %>	
<% if (Security.IsAdministratorAccess && Util.GetSetting("ShowAdminPanel").ConvertToBool()) { %>
<div class="svyAdminBanner">
	<span class="svyAdminBannerImg"><img src="<%= Web.Root %>images/savvyiconblack.gif"/></span>
	<div class="svyAdminBannerNav">
		<span class="svyAdminBannerNavImg"><a href="<%= Web.AdminRoot %>PageAdmin/Edit/<%= Model.PageID %>" target="_blank"><img src="<%= Web.Root %>images/edit1.gif"/></a></span>
		<span class="svyAdminBannerLink"><a href="<%= Web.AdminRoot %>PageAdmin/Edit/<%= Model.PageID %>" target="_blank">Edit Page</a></span>
		<span class="svyAdminBannerNavImg"><a href="http://twitch.beweb.co.nz/" target="_blank"><img src="<%= Web.Root %>images/twitch.gif"/></a></span>
		<span class="svyAdminBannerLink"><a href="http://twitch.beweb.co.nz/" target="_blank">Twitch It</a></span>
		<span class="svyAdminBannerNavImg"><a href="http://twitch.beweb.co.nz/" target="_blank"><img src="<%= Web.Root %>images/mail.gif"/></a></span>
		<span class="svyAdminBannerLink"><a href="mailto:mike@beweb.co.nz?&subject=<%= Model.GetUrl() %>&body=<%= Model.GetUrl() %>">Email Page</a></span>
		<span class="svyAdminBannerNavImg"><a href="<%= Web.AdminRoot %>PageAdmin/Edit/<%= Model.PageID %>" target="_blank"><img src="<%= Web.Root %>images/admin.gif"/></a></span>
		<span class="svyAdminBannerLink"><a href="<%= Web.AdminRoot %>" target="_blank">Admin</a></span>
	</div>
		<span class="svyAdminBannerText">
		Logged in as : <%= UserSession.Person.FullName %> 
		<a style="color: white;" class="svyAdminBannerLogoutLink" href="<%= Web.Root %>security/logout" target="_blank"> Log Out</a>
	</span>
</div>
<% } %>

