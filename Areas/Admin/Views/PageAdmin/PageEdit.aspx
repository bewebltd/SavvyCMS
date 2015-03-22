<%@ Page Title="Edit Page" Inherits="System.Web.Mvc.ViewPage<Models.Page>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%//Util.IncludeJavascriptFile(Page,"~/js/dirtychecker.js"); %>
	<%//Util.IncludeJavascriptFile(Page,"~/js/select2/select2.js"); %>
	
	<script src="<%=Web.Root %>js/select2/select2.js" type="text/javascript"></script>
	 <link href="<%=Web.Root %>js/select2/select2.css" rel="stylesheet" type="text/css" media="screen" />
	<script type="text/javascript">
		<%=FileSystem.GetFileContents( Web.Root + "areas/admin/views/pageadmin/PageEdit.js",false)%>
		var $tabs = null;
		$(document).ready(function () {
			BewebInitForm("form");

			$('#ParentPageID').select2({ width: '430px' });

			function cf_tinyMCEReady() {
				var pageWidth = <%= Util.GetSetting("defaultPageWidth","940")%>;
					tinyMCE.activeEditor.theme.resizeTo(pageWidth + 50, 400);
					svyDevices[0].width = pageWidth;
					svyDevices[0].height = 500;
				}

		

			if (window.SetupSelect2InChildForm) SetupSelect2InChildForm(false);
			//$('.parentpage').select2({width:'330px'});						 //dont do that!

			ShowTemplate();
			//var $tabs = null;
			window.setTimeout(function () { 
				$tabs = $("#pagetabs").tabs();
				$("#pagetabs").show();
			}, 500);
		});
		
		// DELETE IF NOT USING RELATED PAGES
		function cf_OnAddRowRelatedPage(){ //this is called by df_addrow
			//kill existing select2s
			//var maxRow = parseInt($('#df_MaxRow__RelatedPage').val(), 10);
			//for (var sc = 0; sc <= maxRow; sc++) {
			//	if($('#LinkedPageID__RelatedPage__' + sc).hasClass('select2-offscreen')){
			//		$('#LinkedPageID__RelatedPage__' + sc).select2('destroy');
			//	}
			//	
			//}

			SetupSelect2InChildForm(true);
		}
		function SetupSelect2InChildForm(preventSort) {
			window.setTimeout(function () {
				var maxRow = parseInt($('#df_MaxRow__RelatedPage').val(), 10);
				for (var sc = 0; sc <= maxRow; sc++) {
					if(!$('#LinkedPageID__RelatedPage__' + sc).hasClass('select2-offscreen'))$('#LinkedPageID__RelatedPage__' + sc).select2({ width: '430px' });
					//if(preventSort){
					//	$('#df_SubformRow__RelatedPage__' + sc +' .sortgroup .drag i').hide();								 //hide drag icon
					//	REDIPS.drag.enableDrag(false, '#df_SubformRow__RelatedPage__' + sc +' .svyRowSort');							 //disable drag row
					//	$('#df_SubformRow__RelatedPage__' + sc +' .drag').attr('style','border-style: none');							 //remove cursor
					//}
				}
			}, 250);
		}
		// END DELETE IF NOT USING RELATED PAGES

		function ShowTemplate() {
			$(".section-template").hide();
			$(".sectionpage-template").hide();
			$(".wide-template").hide();
			$(".subpanel-template").hide();
			$(".page-template").hide();
			$(".contact-template").hide();
			$(".link-template").hide();
			$(".special-template").hide();
			$(".products-template").hide();
			$(".resource-template").hide();
			$(".gallery-template").hide();
			$(".documentrepository-template").hide();
			$(".news-template").hide();
			var template = V$("TemplateCode").toLowerCase();
			$("." + template + "-template").show();
			
			// set appropriate fields to blank if not visible

			// set appropriate fields to No if not visible
			var form = document.forms["form"];
			if (!$("#ShowInMainNav_False").is(":visible") && form["ShowInMainNav"]) {
				form["ShowInMainNav"][1].checked = true;
			}
			if (!$("#ShowInSecondaryNav_False").is(":visible") && form["ShowInSecondaryNav"]) {
				form["ShowInSecondaryNav"][1].checked = true;
			}
			if (!$("#ShowInFooterNav_False").is(":visible") && form["ShowInFooterNav"]) {
				form["ShowInFooterNav"][1].checked = true;
			}
			if (!$("#LinkUrlIsExternal_False").is(":visible") && form["LinkUrlIsExternal"]) {
				form["LinkUrlIsExternal"][1].checked = true;
			}
			
		}

		function ShowQR() {
			$.fn.colorbox({ inline: true, href: "#qr", opacity: 0.7 });
			return false;
		}

		function GetSortOrder() {
			var selId = $("#ParentPageID option:selected").val();
			var qs = "";
			$.ajax({
				type: "POST",
				url: "<%=Web.AdminRoot%>PageAdmin/GetSortOrder?id=" + selId,
				data: qs,
				success: function (msg) {
					$("#SortPosition").val(msg);
				},
				error: function (msg) {

				}
			});
		}

	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
		<tr>
			<th colspan="2">Page</th>
		</tr>	
		<tr>
			<td colspan="2" class="header">
				<!--this replaced by .footer inner html-->
			</td>
		</tr>
		<tr>
			<td class="label">Template:</td>
			<td class="field">
				<%if(Security.IsDevAccess || record.TemplateCode != "special"){		%>
					<%=new Forms.Dropbox(record.Fields.TemplateCode, true, false){onchange = "ShowTemplate()"}
						.Add("page", "Standard Page")
						//.Add("articlePage", "Article Page")
						//.Add("wide", "Wide Page")
						//.Add("section", "Section Placeholder")
						//.Add("subpanel", "Section Sub-Panel")
						.Add("special", "Special/System Page")
						//.Add("gallery","Gallery Page")
						//.Add("products","Products Page")
						//.Add("resource","Resource Page")
						.Add("documentrepository","Document Repository")
						.Add("link", "Link")
						
						%>
				<%}else{ %>
					<%=new Forms.DisplayField(record.Fields.TemplateCode) %>
				<%} %>
				<div class="sectionpage-template">A <b>Section</b> page that lists pages below it with an intro.</div>
				<div class="section-template">Set up a <b>Section Placeholder</b> with no content of its own which automatically links to the first child page.</div>
				<div class="subpanel-template">A <b>Section Sub-Panel</b> must have a <b>Section Page</b> as its Parent Page.</div>
				<div class="contact-template">A <b>Special Contact us page</b> with map info. </div>
				<div class="page-template">A <b>Standard Page</b> can contain any general content. </div>
				<div class="wide-template">A <b>Wide Page</b> spans the full width and does not have any left-hand sub-navigation.</div>
				<div class="special-template"><b>Special/System Page</b>s are required for the site to function correctly. You may edit a limited number of fields such as  title, intro and SEO fields.</div>
				<div class="products-template">Set up a <b>Product Page</b> where you can place product categories.</div>
				<div class="resource-template">Set up a <b>Resource Page</b> where you can place resources.</div>
				<div class="gallery-template">Set up a <b>Gallery Page</b> where you can upload images for users to view.</div>
				<div class="news-template">Set up a <b>News Page</b> like a section page, but displays news items rather than sub-pages.</div>
			</td>
		</tr>
		<tr>
			<td class="label">Parent Page:</td>
			<td class="field">
				<%=new Forms.ParentPageDropbox(record.Fields.ParentPageID, (Util.GetSetting("SiteNavigationDepth").ToInt()-1)){onchange = "GetSortOrder();"} %>
			</td>
		</tr>
		<%if (Util.IsDevAccess()) {%>
			<tr class="special-template section-template">
				<td class="label">DEV: Page Code:  <%=Html.SavvyHelp("This applies to built-in pages and must not be changed.")%></td>
				<td class="field" style="background-color:#f33;padding-bottom:5px;color:white">
					PageCode controls which Controller gets the request<br />
					<%=new Forms.TextField(record.Fields.PageCode, false) %>
				</td>
			</tr>
		<%} else {%>
			<tr>
				<td class="label">Page Code:  <%=Html.SavvyHelp("This applies to built-in pages and cannot not be changed.")%></td>
				<td class="field"><%=record.PageCode %></td>
			</tr>
		<%} %>
			<tr>
				<td class="label">Title:* <%=Html.SavvyHelp("Displayed as the H1 at the top of the page. This is optional, but if you do not specify it you should specify a Nav Title instead.")%></td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true){ ExtraAttribs = "data-focusLabel=''"} %></td>
			</tr>
			<tr>
				<td class="label"><label for="NavTitle">Nav Title:</label> <%=Html.SavvyHelp("If the page title is long, you can specify a shorter title to appear on navigation tabs. This is optional.")%></td>
				<td class="field"><%=new Forms.TextField(record.Fields.NavTitle, false){} %></td>
			</tr>
			<tr class="link-template">
				<td class="label">Link URL:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.LinkUrl, false) %></td>
			</tr>
			<tr class="link-template">
				<td class="label">Link opens in a new window: <%=Html.SavvyHelp("Select Yes to have the link open in a new browser window - useful if the link is an external link to another site.")%></td>
				<td class="field"><%= new Forms.YesNoField(record.Fields.LinkUrlIsExternal) %></td>
			</tr>
			<tr class="page-template wide-template documentrepository-template">
				<td class="label section"><strong>Page Content</strong></td>
				<td class="section">
					View for <%= new Forms.Dropbox("svyMobileOptions",false){onchange = "svyMceMobileView(this.value);"}%>

				</td>
			</tr>
    	<%--
			<tr class="page-template section-template subpanel-template special-template">
				<td class="label">Introduction:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.Introduction ,false){maxlength = 500} %></td>
			</tr>
			--%>
		  <tr class="page-template wide-template special-template documentrepository-template">
			  <td class="label">Body Text: <%=Html.SavvyHelp("The main content of the page. For built-in pages, this is sometimes used as an intro at the top of the page - but in other cases it is not applicable.")%> </td>
			  <td class="field"><%= new Forms.RichTextEditor(record.Fields.BodyTextHtml ,false){ExtraButtons = "code"}%></td>
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

		<tr class="page-template special-template"><!--  DELETE IF NOT USING RELATED PAGES-->

			<td class="label">Related pages</td>
			<td class="field">
					
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
								<input type=button onclick="df_AddRow('RelatedPage');return false;" value="Add Page">
								<!--<a href="#" onclick="df_AddRow('Page');return false;">Add Page</a>-->

							</td>
						</tr>
					</tfoot>
				</table>	
					
			</td><!--  END IF NOT USING RELATED PAGES-->
		</tr>
			<tr class="page-template section-template special-template gallery-template documentrepository-template">
				<td class="label section"><strong>Search Engine Optimisation</strong></td>
				<td class="section"></td>
			</tr>
			<%if (Model.PageCode.IsBlank()) {%>
				<tr class="page-template wide-template documentrepository-template">
					<td class="label">Custom Page URL: <%=Html.SavvyHelp("You can set your own URL for any page you add by specifying it here. It must contain only letters, digits and dashes. Note this does not apply for built-in pages.") %></td>
					<td class="field"><%=Web.ResolveUrlFull("~/Page/")%><%= new Forms.TextField(record.Fields.URLRewriteTitle, false){style = "width:200px"}%></td>
				</tr>
			<%} else {%>
				<tr class="page-template section-template special-template gallery-template ">
					<td class="label">Page URL: <%=Html.SavvyHelp("This is a built-in page and the URL cannot be changed.") %></td>
					<td class="field"><%--<%=Web.ProtocolAndHost%>--%><span id="PageUrl" class="svyFocusField"><%=Model.GetUrl() %></span>
						<%if(true){%>
							<div style="float:right;"><a href="" onclick="return ShowQR()"><img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=50x50&amp;chl=<%=Model.GetUrl() %>"></a>
							<%=Html.SavvyHelp(@"QR Code. Click on this code to show a large version of the code. You can screen capture it, and use it to advertise this page.", width:200) %></div>
							<div id="colorbox" style="display:none">
								<div id="qr">
									<img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=400x400&amp;chl=<%=Model.GetUrl() %>">
								</div>
							</div>
						<%} %>
					</td>
				</tr>
			<%} %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record,false,"page-template section-template special-template gallery-template documentrepository-template")); %>
	
			
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
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowInMainNav, true) %> (shows in primary navigation - most pages should be Yes)</td>
			</tr>
			<%--<tr class="">
				<td class="label">Show In Small Secondary Nav:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowInSecondaryNav, true) %> (shows in supplementary utility navigation)</td>
			</tr>--%>
			<tr class="">
				<td class="label">Show In Footer Nav:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowInFooterNav, true) %> (shows in footer)</td>
			</tr>
			<tr class="">
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>
			<%//= SiteMain.ShowModificationLog(record)%>
			<tr>
				<td colspan="2" class="footer">
					<%=Html.SaveButton() %>
					<%=Html.SaveAndRefreshButton() %>
					<%=Html.DuplicateCopyButton() %>
					<%=Html.CancelButton() %>
					<%if (record.PageCode.IsBlank() || Util.IsDevAccess()) {%>
						<%if (Security.IsSuperAdminAccess) {%>
							<%=Html.DeleteButton() %>
						<%}%>
					<%}%>
					<%=Html.SavvyHelpText(new Beweb.HelpText("Page Edit")) %>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

