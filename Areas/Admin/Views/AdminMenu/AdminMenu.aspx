<%@ Page Title="Admin" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.AdminMenuController.ViewModel>" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
<%if (Response.StatusCode==404) {%>
	<div class="ErrorMessage">Resource Not Found - <%=Request["message"]%></div>
<%} %>
<%if (Response.StatusCode==500) {%>
	<div class="ErrorMessage">Error - <%=Request["message"]%></div>
<%} %>

<%=Html.InfoMessage()%>

<%if(Model.UserName.Trim()!="") {%>
	<div class="InfoMessage">You are logged in as <%=Model.UserName%></div>
<%} %>
	<style>.devOnly{display:none;}</style>			
	<%=Html.SavvyHelpText(new Beweb.HelpText("Admin Main Menu")) %>
	<table class="svyEdit" cellspacing="0">
		<%if(Web.Request["dash"]!=null){ %>		<tr>
			<th colspan="2">Dashboard</th>
		</tr>	
		<tr>
			<th colspan="2">
				<table class="dashBoard">
					<tr>
						<td>
							<div class="dashTitle">Average players per day</div>
							<div class="description">15</div>
						</td>
						<td>
							<div class="dashTitle">% Winners</div>
							<div class="description"><span class="bigNumber">5</span> of 10</div>
							<%= Beweb.Html.BarGraph(amount: 50, outOfTotal: 100, showPercentage: true) %>
						</td>
						<td>
							<div class="dashTitle">% Winners</div>
							<div class="description"><span class="bigNumber">5</span> of 10</div>
							<%= Beweb.Html.BarGraph(amount: 50, outOfTotal: 100, showPercentage: true) %>
						</td>
						<td>
							<div class="dashTitle">% Winners</div>
							<div class="description"><span class="bigNumber">5</span> of 10</div>
							<%= Beweb.Html.BarGraph(amount: 50, outOfTotal: 100, showPercentage: true) %>
						</td>
					</tr>	
				</table>	
			</th>
		</tr><%} %>
		<tr><th colspan="2">Administration Menu</th></tr>
		
		<%if(Security.IsAdministratorAccess){ %>
			<%=Html.AdminMenuSectionTitle("Website Content")%>
			<%=Html.AdminMenuItem("Page", null, "View and edit website pages (the basis for the navigation and site map)")%>
			<%=Html.AdminMenuItem("Article", null, "View/Add/Edit Articles on the intranet")%>
			<%-- add menu items using this helper --%>
			<%--<%=Html.AdminMenuItem("City")%>--%>
			<%=Html.AdminMenuItem("HomepageSlideAdmin")%>
			<%=Html.AdminMenuItem("TextBlockAdmin", null, "Find and edit named areas of text that appear around the site")%>
			<%=Html.AdminMenuItem("ContactUsAdmin", null, "View contact us requests made by the public")%>
			<%=Html.AdminMenuItem("FAQSection", "Manage FAQ Sections", "Edit FAQ / Questions Sections")%>
			<%=Html.AdminMenuItem("FAQItem", "Manage FAQs", "Edit FAQ / Questions ")%>
			<%=Html.AdminMenuItem("NewsAdmin", null, "Edit News")%>
			<%=Html.AdminMenuItem("Testimonial", null, "Edit Testimonials")%>
			<%=Html.AdminMenuItem("NewsRSS", null, "Edit News RSS feeds that can generate news from external feeds")%>
			<%=Html.AdminMenuItem("Event", null, "Events")%>
			<%=Html.AdminMenuItem("ClientContactUsRegion", null, "Client Contact Regions")%>
			<%=Html.AdminMenuItem("ClientContactUsPerson", null, "Client Contact People")%>
			<%=Html.AdminMenuItem("Product", null, "Products")%>
			<%=Html.AdminMenuItem("ProductCategory", null, "Product Categories")%>
			<%=Html.AdminMenuItem("ShoppingCartOrder", null, "Shopping carts created by users")%>
			<%=Html.AdminMenuItem("BlogAdmin", null, "Blog and comments")%>
			<%=Html.AdminMenuItem("Comment", null, "Comments made around the site")%>
			<%=Html.AdminMenuItem("Banner", null, "Edit banners on the site")%>
			<%--
			<tr>
				<td class="label"><a href="NewsRSSAdmin">RSS Feed List</a>
					<%//=Html.SavvyHelp("This is the bulk of the content of the site, as well as the basis for the navigation and site map.") %>
				<td>View and edit data in feeds</td>
			</tr>
			<tr>
				<td class="label"><a href="NewsRSSAdmin/FeedEditor">RSS Feed Editor</a>
					<%//=Html.SavvyHelp("This is the bulk of the content of the site, as well as the basis for the navigation and site map.") %>
				<td>View and edit data in feeds</td>
			</tr>		
			--%>
			<%=Html.AdminMenuItem("Video", null, "Youtube Videos")%>
			<%= Html.AdminMenuItem("CompetitionAdmin", null, "Edit competition and download entries") %>
			<%=Html.AdminMenuSectionTitle("Galleries")%>
			<%=Html.AdminMenuItem("GalleryCategory", null, "View/Add/Edit Gallery Categories on the intranet")%>
			<%=Html.AdminMenuItem("GalleryImage", null, "View/Add/Edit Photos and Videos in galleries on the intranet")%>
			<%=Html.AdminMenuSectionTitle("Document Repositories")%>
			<%=Html.AdminMenuItem("DocumentCategoryAdmin", null, "Add and edit document categories on the site")%>
			<%=Html.AdminMenuItem("DocumentAdmin", null, "Find and edit documents that downloaded from the site")%>
		<%}// admin access %>
		<%if(Security.IsSuperAdminAccess){ %>
	
			<%if(Beweb.BewebData.TableExists("Settings")) { %>
				<tr>
					<td class="label section"><strong>General Site Settings</strong></td>
					<td class="section"></td>
				</tr>
				<tr>
					<td class="label"><a href="<% if (Models.SettingsList.LoadAll().Count == 0) {%>SettingsAdmin/Create<% }else{%>SettingsAdmin/Edit/<%=Models.SettingsList.LoadAll()[0].ID %><%} %>">Settings</a>
					<td>Edit global tags and adjust site settings and switches that control the site</td>
				</tr>
			<%} %>
		
			<%=Html.AdminMenuItem("TextBlockGroup", null, "Set up text block groups to make finding text blocks easier")%>
			
			<%if(Beweb.BewebData.TableExists("SavvyAdmin")) { %>
				<tr>
					<td class="label"><a href="<% if (Models.SavvyAdminList.LoadAll().Count == 0) {%>SavvyAdmin/Create<% }else{%>SavvyAdmin/Edit/<%=Models.SavvyAdminList.LoadAll()[0].ID %><%} %>">Admin Theme</a>
					<td>Edit admin site look</td>
				</tr>
			<%} %>			

			<%=Html.AdminMenuItem("UrlRedirect", null, "Create redirects to maintain search ranking when renaming page URLs")%>

		<%}//IsSuperAdminAccess access %>
		
		<%if (Model.IsDevAccess) {%>
			<tr>
				<td class="label section"><strong onclick="$('.devOnly').show();$('#devopt').html('Below are shown the developer only options')">Dev Only</strong></td>
				<td class="section" id="devopt">Click to show developer options</td>
			</tr>
			<%if (Beweb.BewebData.TableExists("Template")) { %>
				<tr class="devOnly">
					<td class="label"><a href="Template">Page Template List</a></td>
					<td>View and edit Templates</td>
				</tr>
			<%} %>
			<%if (true||Beweb.BewebData.TableExists("GenTest")) { %>
				<tr class="devOnly">
					<td class="label"><a href="GenTestAdmin">GenTest List</a></td>
					<td>View and edit GenTests</td>
				</tr>
				<tr class="devOnly">
					<td class="label"><a href="GenTestAdmin/GenTestEditAutocomplete">GenTest autocomplete</a></td>
					<td>View and edit GenTests</td>
				</tr>
				<tr class="devOnly">
					<td class="label"><a href="GenTestCatAdmin/">GenTest categories</a></td>
					<td>View and edit GenTests</td>
				</tr>
			
			<%} %>
			<%if (Beweb.BewebData.TableExists("TextBlockSection")) { %>
				<tr class="devOnly">
					<td class="label"><a href="modules/text/TextBlockSectionList.aspx">Text Block Pages</a></td>
					<td>List and edit the pages text blocks appear on.</td>
				</tr>
			<%} %>
			<%if (Beweb.BewebData.TableExists("SearchArea")) { %>
				<tr class="devOnly">
					<td class="label"><a href="SearchArea">Search Area List</a></td>
					<td>View and edit Search Areas</td>
				</tr>
			<%} %>
			<%if (Beweb.BewebData.TableExists("ModificationLog")) { %>
				<tr class="devOnly">
					<td class="label"><a href="ModificationLogadmin/">Modification Log</a></td>
					<td>View Modification Log</td>
				</tr>
			<%} %>
			<%if (Beweb.BewebData.TableExists("MailLog")) { %>
				<tr class="devOnly">
					<td class="label"><a href="MailLogAdmin/">Mail Log</a></td>
					<td>View emails sent by the site.</td>
				</tr>
			<%} %>
			<%if (Beweb.BewebData.TableExists("CommonWord")) { %>
				<tr class="devOnly">
					<td class="label"><a href="CommonWord">Common Word List</a></td>
					<td>View and edit Common Words to exclude from Search</td>
				</tr>
			<%} %>
			<%if (Beweb.BewebData.TableExists("SavvyAdmin") && Models.SavvyAdminList.LoadActive().Count() > 0) {%>
				<tr class="devOnly">
					<td class="label"><a href="SavvyAdmin/Edit/<%=Models.SavvyAdminList.LoadActive()[0].ID %>">SavvyAdmin</a></td>
					<td>Edit admin site look</td>
				</tr>
			<%} %>
			
			<tr class="devOnly">
				<td class="label"><a href="tools/default.aspx">DEV: Admin Tools</a></td>
				<td>Generate admin system</td>
			</tr>
			<tr class="devOnly">
				<td class="label section"><strong>Site Settings</strong></td>
				<td class="section"></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Server Is: </td>
				<td><%= Beweb.Util.ServerIs()%></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Emails Sent To:</td>
				<td><%= ConfigurationManager.AppSettings["EmailToAddress" + Beweb.Util.ServerIs()]%></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Server Date and Time:</td>
				<td><%= DateTime.Now.ToString("dddd d-MMM-yyyy HH:mm:ss (zzz)")%></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Database:</td>
				<td><%=Model.ConnectionStringDetails %></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Admin user role is :</td>
				<td><%=Model.Role %></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Server IP:</td>
				<td><%=Request.ServerVariables["LOCAL_ADDR"] %></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Your IP:</td>
				<td><%=Request.ServerVariables["REMOTE_ADDR"] %></td>
			</tr>
			<tr class="devOnly">
				<td class="label">IIS Instance:</td>
				<td><%=Request.ServerVariables["INSTANCE_ID"] %></td>
			</tr>
			<tr class="devOnly">
				<td class="label">Server Software:</td>
				<td><%=Request.ServerVariables["SERVER_SOFTWARE"] %></td>
			</tr>
		<%--	<tr class="devOnly">
				<td class="label">Scheduled Tasks:</td>
				<td>
					Next hourly run time: <%=ScheduledTaskController.nextHourlyRunTime.FmtDateTime()%>	<%//=Html.SavvyHelp(@"This updates these data sources hourly") %>
					
					
					From DB: <%=Settings.All.ScheduledTaskLastDailyRunTime.FmtDateTime() %>
				

				</td>
			</tr>--%>
			<tr class="devOnly">
				<td class="label">Cache:</td>
				<td>
					<%if(false){ %>
						<a href="<%=Web.Root %>?clearcache=1">Clear above items, and reload them from sources</a>
					<%} %>
				</td>
			</tr>
		<%} %>
		<%if(Security.IsSuperAdminAccess){ %>
			<tr>
				<td class="label section"><strong>Administrators</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">
					<a href="PersonAdmin?admin=true">Administrator List</a>
				</td>
				<td>View and edit the details of the people that have access to this administration area</td>
			</tr>
		<%} %>
		<tr>
			<td class="label section"><strong>User Tasks</strong></td>
			<td class="section"></td>
		</tr>
		<tr>
			<td class="label">
				<asp:HyperLink ID="HyperLink1" NavigateUrl="~/security/ChangePassword" Text="Change Password" runat="server" />
			</td>
			<td>If you need to change your password, use this feature</td>
		</tr>
		<tr>
			<td class="label">
				<asp:HyperLink ID="HyperLink2" NavigateUrl="~/security/logout" Text="Logout" runat="server" />
			</td>
			<td>Click here to log out of the admin system</td>
		</tr>
	</table>
</asp:Content>
