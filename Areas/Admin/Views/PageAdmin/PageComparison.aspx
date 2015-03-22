<%@ Page Title="Edit Page" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.PageAdminController.PageComparisonViewModel>" Language="C#" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<style>
	<%=FileSystem.GetFileContents("~/areas/admin/admin.css")%>
</style>	

<%
if (Util.GetSettingBool("OverrideAdminStyles",true) && Beweb.BewebData.TableExists("SavvyAdmin")) {
	Hashtable styles = new Sql("select * from SavvyAdmin").GetHashtable();
	if(styles!=null) { %>
		<style>
			.svyAdmin .top-stripe { border-top-color: <%=styles["HeaderColor"] ?? "#551901" %>}
			.svyEdit th, .databox .dataheading, .label.section strong, .svyEdit > tbody > tr > th, .formTitle { color: <%=styles["HeaderColor"] ?? "#551901" %>; 	}
		</style>
<% } 
} %>

<script>
		function changeRevision2(select) {
			document.location = '<%=Web.AdminRoot%>PageAdmin/CompareToLive/?revision1=<%:Model.Revision1.PageID.ToString()%>&revision1Number=<%:Web.Request["revision1Number"] %>&revision2=' + select.value;
		}
</script>

<body id="pageComparison">
	
	<h3>Comparison between Revision No. <%:Web.Request["revision1Number"] %> and <%=new Forms.Dropbox("revision2", Model.Revision2.PageID.ToString(), false) { onchange = "changeRevision2(this)"}.Add(Model.RevisionDropbox) %></h3>

	<table class="svyEdit" cellspacing="0">
			<tr>
				<td class="label">Parent Page:</td>
				<td class="field">
					<% if (Model.Revision1.ParentPage.GetNavTitle() != Model.Revision2.ParentPage.GetNavTitle()) { %>
						<del style="background: #ffe6e6;"><%:Model.Revision2.ParentPage.GetNavTitle() %></del>
						<ins style="background:#e6ffe6;"><%:Model.Revision1.ParentPage.GetNavTitle() %></ins>
					<% } else { %>
						<%:Model.Revision2.ParentPage.GetNavTitle() %>
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=Model.TitleDifferences %></td>
			</tr>
			<tr>
				<td class="label">Sub Title:</td>
				<td class="field"><%=Model.SubTitleDifferences %></td>
			</tr>
			<tr>
				<td class="label">Introduction:</td>
				<td class="field"><%=Model.IntroductionDifferences %></td>
			</tr>
			<tr>
				<td class="label">Picture:</td>
				<td class="field">
					<% if (Model.Revision1.Picture != Model.Revision2.Picture) { %>
						<% if (Model.Revision1.Picture != null) { %>
							<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision1.Picture) %>" />	
						<% } %>
						<% if (Model.Revision2.Picture != null) { %>
							<span>
								<i class="removedPicture"></i>
								<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision2.Picture) %>" />	
							</span>
						<% } %>
					<% } else if(Model.Revision2.Picture != null) { %>
						<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision2.Picture) %>" />
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%=Model.BodyTextDifferences %></td>
			</tr>
			<tr>
				<td class="label section"><strong>Navigation Information</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Nav Title:</td>
				<td class="field"><%=Model.NavTitleDifferences %></td>
			</tr>
			<tr>
				<td class="label">Nav Introduction:</td>
				<td class="field"><%=Model.NavIntroductionDifferences %></td>
			</tr>
			<tr>
				<td class="label">Nav Picture:</td>
				<td class="field">
					<% if (Model.Revision1.NavPicture != Model.Revision2.NavPicture) { %>
						<% if (Model.Revision1.NavPicture != null) { %>
							<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision1.NavPicture) %>" />	
						<% } %>
						<% if (Model.Revision2.NavPicture != null) { %>
							<span>
								<i class="removedPicture"></i>
								<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision2.NavPicture) %>" />	
							</span>
						<% } %>
					<% } else if(Model.Revision2.NavPicture != null) { %>
						<img src="<%:ImageProcessing.ImagePreviewPath(Model.Revision2.NavPicture) %>" />
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label">Nav Link Title:</td>
				<td class="field"><%=Model.NavLinkTitleDifferences %></td>
			</tr>
			<tr>
				<td class="label">Nav Link URL:</td>
				<td class="field">
					<% if (Model.Revision1.NavLinkUrl != Model.Revision2.NavLinkUrl) { %>
						<% if (Model.Revision2.NavLinkUrl != null) { %><del style="background: #ffe6e6;"><%: Model.Revision2.NavLinkUrl %></del><% } %>
						<% if (Model.Revision1.NavLinkUrl != null) { %><ins style="background:#e6ffe6;"><%: Model.Revision1.NavLinkUrl %></ins><% } %>
					<% } else if(Model.Revision2.NavLinkUrl != null) { %>
						<%:Model.Revision2.NavLinkUrl %>
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label section"><strong>Search Engine Optimisation</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Page Title Tag:</td>
				<td class="field"><%=Model.PageTitleTagDifferences %></td>
			</tr>
			<tr>
				<td class="label">Meta Keywords:</td>
				<td class="field"><%=Model.MetaKeywordsDifferences %></td>
			</tr>
			<tr>
				<td class="label">Meta Description:</td>
				<td class="field"><%=Model.MetaDescriptionDifferences %></td>
			</tr>
			<tr>
				<td class="label section"><strong>Side Bar Information</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Side Bar Title:</td>
				<td class="field"><%=Model.SideBarTitleDifferences %></td>
			</tr>
			<tr>
				<td class="label">Side Bar Text:</td>
				<td class="field"><%=Model.SideBarTextHtmlDifferences %></td>
			</tr>
			<tr>
				<td class="label">Link URL:</td>
				<td class="field">
					<% if (Model.Revision1.LinkUrl != Model.Revision2.LinkUrl) { %>
						<% if (Model.Revision2.LinkUrl != null) { %><del style="background: #ffe6e6;"><%: Model.Revision2.LinkUrl %></del><% } %>
						<% if (Model.Revision1.LinkUrl != null) { %><ins style="background:#e6ffe6;"><%: Model.Revision1.LinkUrl %></ins><% } %>
					<% } else if(Model.Revision2.LinkUrl != null) { %>
						<%:Model.Revision2.LinkUrl %>
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label section"><strong>Publish Settings</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
			<td class="label">Publish Date:</td>
				<td class="field">
					<% if (Model.Revision1.PublishDate != Model.Revision2.PublishDate) { %>
						<% if(Model.Revision2.PublishDate != null) { %><del style="background: #ffe6e6;"><%:Model.Revision2.PublishDate.FmtDate() %></del><% } %>
						<% if(Model.Revision1.PublishDate != null) { %><ins style="background:#e6ffe6;"><%:Model.Revision1.PublishDate.FmtDate() %></ins><% } %>
					<% } else if(Model.Revision2.PublishDate != null) { %>
						<%:Model.Revision2.PublishDate.FmtDate() %>
					<% } %>
				</td>
			</tr>
			<tr>
				<td class="label">Expiry Date:</td>
				<td class="field">
					<% if (Model.Revision1.ExpiryDate != Model.Revision2.ExpiryDate) { %>
						<% if (Model.Revision2.ExpiryDate != null) { %><del style="background: #ffe6e6;"><%: Model.Revision2.ExpiryDate.FmtDate() %></del><% } %>
						<% if (Model.Revision1.ExpiryDate != null) { %><ins style="background:#e6ffe6;"><%: Model.Revision1.ExpiryDate.FmtDate() %></ins><% } %>
					<% } else if(Model.Revision2.ExpiryDate != null) { %>
						<%:Model.Revision2.ExpiryDate.FmtDate() %>
					<% } %>
				</td>
			</tr>
		</table>
</body>