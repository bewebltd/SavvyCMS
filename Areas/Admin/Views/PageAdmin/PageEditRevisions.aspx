<%@ Page Title="Edit Page" Inherits="System.Web.Mvc.ViewPage<Models.Page>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script src="<%=Web.Root %>js/select2/select2.js" type="text/javascript"></script>
	<link href="<%=Web.Root %>js/select2/select2.css" rel="stylesheet" type="text/css" media="screen" />
	<script type="text/javascript">
		<%=FileSystem.GetFileContents( Web.Root + "areas/admin/views/pageadmin/PageEdit.js",false)%>
		var $tabs = null;
		$(document).ready(function () {
			BewebInitForm("form");
			$('#ParentPageID').select2({ width: '430px' });
			//SetupSelect2InChildForm(false);
			ShowTemplate();		 
			//var $tabs = null;
			window.setTimeout(function () { 
				$tabs = $("#pagetabs").tabs();
				$("#pagetabs").show();
			}, 500);
		});
		function cf_OnAddRowRelatedPage() { //this is called by df_addrow

			//SetupSelect2InChildForm(true); //this is broken, needs to be fixed
		}
		function SetupSelect2InChildForm(preventSort) {
			window.setTimeout(function () {
				var maxRow = parseInt($('#df_MaxRow__RelatedPage').val(), 10);
				for (var sc = 0; sc <= maxRow; sc++) {
					if (!$('#LinkedPageID__RelatedPage__' + sc).hasClass('select2')) $('#LinkedPageID__RelatedPage__' + sc).select2({ width: '430px' });
					if (preventSort) {
						$('#df_SubformRow__RelatedPage__' + sc + ' .sortgroup .drag i').hide();								 //hide drag icon
						REDIPS.drag.enableDrag(false, '#df_SubformRow__RelatedPage__' + sc + ' .svyRowSort');							 //disable drag row
						$('#df_SubformRow__RelatedPage__' + sc + ' .drag').attr('style', 'border-style: none');							 //remove cursor
					}
				}
			}, 250);
		}
	</script>
	<style>
		.pageCarouselRow .svyPictureContainer,
		.pageCarouselRow .svyPicContainer { margin-top: 0; width: auto; }
	</style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<% var record = Model; %>
	<%= Html.InfoMessage() %>
	<%= Html.ValidationSummary("This record could not be saved.") %>
	<p class="formTitle">Page</p>
	<div id="pagetabs" style="display:none">
		<% if (Settings.All.EnablePageRevisions) { %>
		<ul>
			<li><a href="#fragment-1"><span>Edit</span></a></li>
			<li><a href="#fragment-2" onclick="loadRevisionView(<%= record.IsNewRecord ? "null" : "'" +record.GetUrl() + "'" %>)"><span>View</span></a></li>
			<li><a href="#fragment-3"><span>Revisions</span></a></li>
		</ul>
		<%} %>
			<div id="fragment-1">
				<form name="form" id="form" method="post" enctype="multipart/form-data">
				<table class="svyEdit" cellspacing="0">
					<% if (record.HistoryPageID != null) { %>
					<tr>
						<td colspan="2">
							<p style="text-align:center;font-weight:bold;">You're currently viewing a revision. <a href="<%= Web.AdminRoot %>PageAdmin/Edit/<%= record.HistoryPageID %>" class="revisionLink">Click here</a> to edit the published page.</p>
						</td>
					</tr>	
					<% } %>
					<tr>
						<td class="label">Template:</td>
						<td class="field">
							<% if (Security.IsDevAccess || record.TemplateCode != "special") { %>
								<% var templateDropbox = new Forms.Dropbox(record.Fields.TemplateCode, true, false) {onchange = "ShowTemplate()"};

								   if (Security.IsDevAccess || !record.IsNewRecord) {
									   templateDropbox.Add("special", "Special/System Page");
								   }

								   templateDropbox.Add("section", "Section")
									   .Add("detail", "Detail")
									   //.Add("wide", "Wide Page")
									   //.Add("subpanel", "Section Sub-Panel")
						
									   //.Add("gallery","Gallery Page")
									   //.Add("products","Products Page")
									   //.Add("resource","Resource Page")
									   .Add("link", "Link")
									   .Add("map", "Map")
									   .Add("infometrics", "InfoMetrics");

										 Web.Write(templateDropbox);

								%><% } else { %>
								<%= new Forms.DisplayField(record.Fields.TemplateCode) %>
							<% } %>
				<%--<div class="section-template">Set up a <b>Section Placeholder</b> with no content of its own which automatically links to the first child page.</div>--%>
							<div class="subpanel-template">A <b>Section Sub-Panel</b> must have a <b>Section Page</b> as its Parent Page.</div>
							<div class="page-template">A <b>Standard Page</b> can contain any general content. </div>
							<div class="wide-template">A <b>Wide Page</b> spans the full width and does not have any left-hand sub-navigation.</div>
							<div class="special-template"><b>Special/System Page</b>s are required for the site to function correctly. You may edit a limited number of fields such as  title, intro and SEO fields.</div>
							<div class="products-template">Set up a <b>Product Page</b> where you can place product categories.</div>
							<div class="resource-template">Set up a <b>Resource Page</b> where you can place resources.</div>
							<div class="gallery-template">Set up a <b>Gallery Page</b> where you can upload images for users to view.</div>
						</td>
					</tr>
					<tr class="section-template page-template section-template detail-template special-template infometrics-template">
						<td class="label">Parent Page:</td>
						<td class="field">
							<%= new Forms.ParentPageDropbox(record.Fields.ParentPageID, (Util.GetSetting("SiteNavigationDepth").ToInt() - 1)) {onchange = "GetSortOrder();", cssClass="required"} %>
						</td>
					</tr>
					<% if (Util.IsDevAccess()){ %>
						<tr class="special-template map-template infometrics-template">
							<td class="label">Page Code:  <%= Html.SavvyHelp("This applies to built-in pages and must not be changed.") %></td>
							<td class="field" style="background-color:#f33;padding-bottom:5px;color:white">
								PageCode controls which Controller gets the request<br />
								<%= new Forms.TextField(record.Fields.PageCode, false) %>
							</td>
						</tr>
					<% } else{ %>
						<tr>
							<td class="label">Page Code:  <%= Html.SavvyHelp("This applies to built-in pages and cannot not be changed.") %></td>
							<td class="field"><%= record.PageCode %></td>
						</tr>
					<% } %>
					<tr class="section-template page-template">
						<td class="label">Nav Title: <%= Html.SavvyHelp("If the page title is long, you can specify a shorter title to appear on navigation tabs. This is optional.") %></td>
						<td class="field"><%= new Forms.TextField(record.Fields.NavTitle, false) %></td>
					</tr>
						<tr>
							<td class="label">Title: <%= Html.SavvyHelp("Displayed as the H1 at the top of the page. This is optional, but if you do not specify it you should specify a Nav Title instead.") %></td>
							<td class="field"><%= new Forms.TextField(record.Fields.Title, true) %></td>
						</tr>
						<tr class="section-template">
							<td class="label">Sub Title: </td>
							<td class="field"><%= new Forms.TextField(record.Fields.SubTitle, false) %></td>
						</tr>
						<tr class="link-template map-template infometrics-template">
							<td class="label">Link URL:</td>
							<td class="field"><%= new Forms.UrlField(record.Fields.LinkUrl, false) %></td>
						</tr>
						<tr class="link-template">
							<td class="label">Link opens in a new window: <%= Html.SavvyHelp("Select Yes to have the link open in a new browser window - useful if the link is an external link to another site.") %></td>
							<td class="field"><%= new Forms.YesNoField(record.Fields.LinkUrlIsExternal) %></td>
						</tr>
						<tr class="page-template wide-template">
							<td class="label section"><strong>Page Content</strong></td>
							<td class="section"></td>
						</tr>
    	
						<tr class="page-template link-template detail-template special-template section-template">
							<td class="label">Introduction:</td>
							<td class="field"><%= new Forms.TextArea(record.Fields.Introduction, false) {maxlength = 500} %></td>
						</tr>
			
<%--			<tr class="section-template detail-template">
				<td class="label">Tag Line:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.TagLine ,false){maxlength = 500} %></td>
			</tr>--%>
						<tr class="special-template  link-template section-template detail-template">
							<td class="label">Picture: <%=Html.SavvyHelp(@"When there are no carousel images loaded, this picture will be used. If no picture, parent page picture is used.", width:400) %></td>
							<td class="field"><%= new Forms.PictureField(record.Fields.Picture, false)%></td>
						</tr>

		  <tr class="page-template section-template detail-template map-template">
			  <td class="label">Body Text: <%= Html.SavvyHelp("The main content of the page. For built-in pages, this is sometimes used as an intro at the top of the page - but in other cases it is not applicable.") %> </td>
			  <td class="field"><%= new Forms.RichTextEditor(record.Fields.BodyTextHtml, false) {ExtraButtons = "code"} %></td>
		  </tr>
			
			
			<tr class="special-template">
				<td class="label section"><strong>Navigation Information</strong></td>
				<td class="section"></td>
			</tr>
		<%--	<tr class="special-template">
				<td class="label">Nav Introduction:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.NavIntroduction, false) %></td>
			</tr>
			<tr class="special-template">
				<td class="label">Nav Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.NavPicture, false) %></td>
			</tr>
			<tr class="special-template">
				<td class="label">Nav Link Title:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.NavLinkTitle, false) %></td>
			</tr>
			<tr class="special-template">
				<td class="label">Nav Link URL:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.NavLinkUrl, false) %></td>
			</tr>--%>
			<tr class="detail-template">
				<td class="label section"><strong>Side Bar Information</strong></td>
				<td class="section"></td>
			</tr>
			<tr class="detail-template">
				<td class="label">Side Bar Title:</td>
				<td class="field"><%= new Forms.TextField(record.Fields.SidebarTitle, false) %></td>
			</tr>
			<tr class="detail-template">
				<td class="label">Side Bar Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.SidebarTextHtml, false) %></td>
			</tr>
			<tr class="special-template section-template detail-template">
				<td class="label section"><strong>Extras</strong></td>
				<td class="section"></td>
			</tr>	
			<tr class="special-template section-template detail-template">
				<td class="label">Related pages</td>
				<td class="field">
					<div id="drag">
						<table class="svySubform" id="df_SubformTable_RelatedPage">
							<%--	
							<colgroup>
								<col width="45%" />
								<col width="5%" />
								<col width="5%" />
								<col width="5%" />
							</colgroup>--%>
							<thead>
								<tr>
									<td class="colhead">Page</td>
									<td class="colhead">Active?</td>
									<td class="colhead">Sort Order (lowest at top)</td>
									<td class="remove">Remove</td>
								</tr>
							</thead>
							<tbody>
								<%new Savvy.SavvyDataForm<Models.RelatedPageList,Models.RelatedPage>(record.RelatedPages, new Savvy.SubformOptions() 
									{ 
										DeleteButtonCaption = "x", 
										UseCssButtons = false, SubformName = "RelatedPage"
							
									}).Render(childRecord => 
									{ 
										%>
										<td><%=new Forms.ParentPageDropbox(childRecord.Fields.LinkedPageID, (Util.GetSetting("SiteNavigationDepth").ToInt())){onchange = "GetSortOrder();"} %></td>
										<td><%=Fmt.YesNoHtml( childRecord.GetIsActive()) %></td>
										<%=Html.DraggableSortPosition(childRecord, childRecord.SortPosition, null)%> <%--includes the TR--%>
										<% 
									}); 
								%>
							</tbody>
							<tfoot>
								<tr>
									<td colspan="7" class="addingRow">
										<input type=button onclick="df_AddRow('RelatedPage'); return false;" value="Add Page">
										<!--<a href="#" onclick="df_AddRow('Page');return false;">Add Page</a>-->

									</td>
								</tr>
							</tfoot>
						</table>
					</div>
				</td>
			</tr>
<%--			<tr class="page-template subpanel-template">
							<td class="label">Picture 1:</td>
							<td class="field"><%= new Forms.PictureField(record.Fields.Picture1 ,true) %></td>
						<tr class="page-template">
							<td class="label">Picture 1 Caption:</td>
							<td class="field"><%=new Forms.TextField(record.Fields.Picture1Caption, false) %></td>
						</tr>
											<tr class="page-template">
							<td class="label">Picture 2:</td>
							<td class="field"><%= new Forms.PictureField(record.Fields.Picture2 ,true) %></td>
											</tr>
						<tr class="page-template">
							<td class="label">Picture 2 Caption:</td>
							<td class="field"><%=new Forms.TextField(record.Fields.Picture2Caption, false)%></td>
						</tr>
											<tr class="page-template">
							<td class="label">Picture 3:</td>
							<td class="field"><%= new Forms.PictureField(record.Fields.Picture3 ,true) %></td>
											</tr>
						<tr class="page-template">
							<td class="label">Picture 3 Caption:</td>
							<td class="field"><%=new Forms.TextField(record.Fields.Picture3Caption, false)%></td>
						</tr>

						<tr class="subpanel-template">
							<td class="label">Downloadble PDF:</td>
							<td class="field"><%= new Forms.AttachmentField(record.Fields.AttachmentPDF ,false) %></td>
						</tr>
						--%>
						<tr class="page-template special-template gallery-template">
							<td class="label section"><strong>Search Engine Optimisation</strong></td>
							<td class="section"></td>
						</tr>
						<% if (Model.PageCode.IsBlank())
						   { %>
							<tr class="page-template wide-template">
								<td class="label">Custom Page URL: <%= Html.SavvyHelp("You can set your own URL for any page you add by specifying it here. It must contain only letters, digits and dashes. Note this does not apply for built-in pages.") %></td>
								<td class="field"><%= Web.ResolveUrlFull("~/Page/") %><%= new Forms.TextField(record.Fields.URLRewriteTitle, false) {style = "width:200px"} %></td>
							</tr>
						<% }
						   else
						   { %>
							<tr class="page-template special-template gallery-template section-template detail-template">
								<td class="label">Page URL: <%= Html.SavvyHelp("This is a built-in page and the URL cannot be changed.") %></td>
								<td class="field"><%--<%=Web.ProtocolAndHost%>--%><%= Model.GetUrl() %>
									<% if (true)
									   { %>
										<div style="float:right;"><a href="" onclick="return ShowQR()"><img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=50x50&amp;chl=<%= Model.GetUrl() %>"></a>
										<%= Html.SavvyHelp(@"QR Code. Click on this code to show a large version of the code. You can screen capture it, and use it to advertise this page.", width: 200) %></div>
										<div id="colorbox" style="display:none">
											<div id="qr">
												<img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=400x400&amp;chl=<%= Model.GetUrl() %>">
											</div>
										</div>
									<% } %>
								</td>
							</tr>
						<% } %>
						<% Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record, true, "section-template detail-template")); %>
	
			
						<%--<tr class="page-template section-template special-template">
							<td class="label">Show in XML Sitemap: <%=Html.SavvyHelp("Select No if the page is not to be included in the XML sitemap.<br/><br/><strong>Note:</strong> By default, if the page is not located on HW it will not be included.")%><br /><small>(If this page is off-site (page is a link) it will not be included in the sitemap)</small></td>
							<td class="field"><%=new Forms.YesNoField(record.Fields.ShowInXMLSitemap) %></td>
						</tr>
						--%>
						<%-- enable this for site which have member-only content
						<tr class="">
							<td class="label">Accessible By:</td>
							<td class="field"><%= new Forms.Dropbox(record.Fields.RolesAllowed, false).Add("", "Everyone").Add("Non-Members", "Non-Members Only").Add(SecurityRoles.Roles.MEMBER, "Members Only") %></td>
						</tr>
						--%>			
						<tr>
							<td class="label section"><strong>Publish Settings</strong></td>
							<td class="section"></td>
						</tr>
						<% if (!Security.IsInRoleOnly(SecurityRoles.Roles.EDITOR)) { %>
							<tr>
								<td class="label">Publish Date:</td>
								<td class="field"><%= new Forms.DateField(record.Fields.PublishDate, false) %>  (blank = don't publish)
								</td>
							</tr>
							<tr>
								<td class="label">Expiry Date:</td>
								<td class="field"><%= new Forms.DateField(record.Fields.ExpiryDate, false) %>  (blank = don't expire)</td>
							</tr>
							<tr class="">
								<td class="label">Show In Main Nav:</td>
								<td class="field"><%= new Forms.YesNoField(record.Fields.ShowInMainNav, true) %> (shows in primary navigation - most pages should be Yes)</td>
							</tr>
							<%--<tr class="">
								<td class="label">Show In Small Secondary Nav:</td>
								<td class="field"><%=new Forms.YesNoField(record.Fields.ShowInSecondaryNav, true) %> (shows in supplementary utility navigation)</td>
							</tr>--%>
							<tr class="">
								<td class="label">Show In Footer Nav:</td>
								<td class="field"><%= new Forms.YesNoField(record.Fields.ShowInFooterNav, true) %> (shows in footer)</td>
							</tr>
						<%--	<tr class="section-template">
								<td class="label">Show In Side Panel:</td>
								<td class="field"><%= new Forms.YesNoField(record.Fields.ShowInSideBar, false) %> (shows in sidebar of section page)</td>
							</tr>--%>
							<tr class="">
								<td class="label">Sort Position:</td>
								<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %></td>
							</tr>
						<% } %>
						<% if (Security.IsInRoleOnly(SecurityRoles.Roles.EDITOR) && Settings.All.EnableWorkflow) { %>
							<tr>
								<td class="label">Request approval for:</td>
								<td class="field">
									<%= new Forms.Dropbox(record.Fields.RequestApprovalForPersonID, true, true).Add(new Sql().AddRawSqlString("SELECT PersonID, FirstName + ' ' + LastName FROM Person WHERE Role LIKE '%administrators%' ORDER BY FirstName")) %>
								</td>
							</tr>
						<% } %>
						<tr>
							<td class="label">Date Added:</td>
							<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded) %></td>
						</tr>
						<% if (Settings.All.EnableWorkflow) { %>
							<tr>
								<td class="label section"><strong>Revision Notes</strong></td>
								<td class="section"></td>
							</tr>
							<% if (Security.IsInRoleOnly(SecurityRoles.Roles.EDITOR)) { %>
							<tr>
								<td class="label">Editor notes:</td>
								<td class="field"><%= new Forms.TextArea(record.Fields.EditorNotes, false) %></td>
							</tr>
							<tr>
								<td class="label">Admin notes:</td>
								<td class="field"><%= (record.AdminNotes + "").Replace(Environment.NewLine, "<br/>") %></td>
							</tr>
							<% } else { %>
							<tr>
								<td class="label">Editor notes:</td>
								<td class="field"><%= (record.EditorNotes + "").Replace(Environment.NewLine, "<br/>") %></td>
							</tr>
							<tr>
								<td class="label">Admin notes:</td>
								<td class="field"><%= new Forms.TextArea(record.Fields.AdminNotes, false) %></td>
							</tr>
							<%} %>
						<%} %>
						<tr>
							<td colspan="2" class="footer">
									<%if(Security.IsInRoleOnly(SecurityRoles.Roles.EDITOR) && Settings.All.EnableWorkflow) { %>
										<%=Html.SaveButton("Save & Request Approval","SaveAndRequestApproval") %>
									<% } else if(Settings.All.EnablePageRevisions){ %>
										<%=Html.SaveButton("Save Draft","SaveDraft") %>
										<%=Html.SaveButton("Save & Publish","Publish") %>
									<% } else {%>
										<%=Html.SaveButton() %>
										<%=Html.SaveAndRefreshButton() %>
									<%} %>
									<%=Html.CancelButton() %>
									<%if (record.PageCode.IsBlank() || Util.IsDevAccess()) {%>
										<%if (Security.IsSuperAdminAccess) {%>
											<%=Html.DeleteButton() %>
										<%}%>
									<%}%>
							</td>
						</tr>
					</table>
					<%=Html.AntiForgeryToken() %>
					<%=Html.ReturnPageToken() %>
				</form>
			</div>
			
			<% if (Settings.All.EnablePageRevisions) { %>
				<div id="fragment-2">
					<% if (!record.IsNewRecord) { %>
						<iframe id="fraContents" width="100%" height="600" frameborder="0" src="about:blank"></iframe>
						<%--<input type="hidden" id="previewAddress" value="<%= record.GetUrl() %>" />--%>
					<% } else { %><p style="color:black">No preview available</p><% } %>
				</div>
				<div id="fragment-3" <%--xstyle="display:none"--%>>
					<% if (!record.IsNewRecord) { %>
						<iframe id="Iframe1" width="100%" height="600" frameborder="0" src="../pagerevisions?pageid=<%= record.HistoryPageID ?? record.PageID %>&ColSortField=DateModified&ColSortDesc=1"></iframe>
					<% } else { %><p style="color:black">No revision available for this new page.</p><% } %>
				</div>    
			<% } %>
		</div>  <%-- end pageTab div --%>
		
		<div id="revisionChanges"></div>

</asp:Content>

