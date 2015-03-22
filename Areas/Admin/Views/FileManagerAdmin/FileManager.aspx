<%@ Page Title="Admin" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.FileManagerAdminController.ViewModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
   <title>Savvy File Manager</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf8" />
		<script type="text/javascript">
			websiteBaseUrl = "<%=Web.BaseUrl%>";
			fileManagerControllerUrl = "<%=Web.BaseUrl + "admin/FileManagerAdmin/"%>";
		</script>
		<%Util.IncludejQuery(); %>
		<%Util.IncludeJQueryUI(); %>
    <%Util.IncludeStylesheetFile("~/js/FileManager/FileManager.css"); %>
    <%Util.IncludeJavascriptFile("~/js/FileManager/FileManagerActions.class.js"); %>
    <%Util.IncludeJavascriptFile("~/js/FileManager/FileManager.class.js"); %>
		
		<!-- Tree -->
		<%Util.IncludeStylesheetFile("~/js/FileManager/zTreeStyle/zTreeStyle.css"); %>
    <%Util.IncludeJavascriptFile("~/js/FileManager/jquery.ztree.core-3.5.min.js"); %>

   </head>
  <body>
		<div class="wrapper">
			<div class="top">Savvy - File Manager</div>
			<div class="top-tabs">
				<ul class="clearfix">
					<li class="active" data-href="home-nav">Home</li>
					<li data-href="view-nav">View</li>
				</ul>
			</div>
			<div class="top-nav">
				<div id="home-nav" class="top-nav-content clearfix">
					<ul class="top-nav-groups">
						<li>
							<ul class="top-nav-options clearfix">
								<li class="requiresSelection" data-action="copy"><i class="copy"></i>Copy</li>
								<li class="requiresSelection" data-action="paste"><i class="paste"></i>Paste</li>
								<li class="requiresSelection small" data-action="cut"><i class="cut"></i>Cut</li>
							</ul>
							<p>Clipboard</p>
						</li>
						<li>
							<ul class="top-nav-options clearfix">
								<li class="requiresSelection" data-action="delete"><i class="delete"></i>Delete</li>
								<li class="requiresSingleSelection" data-action="rename"><i class="rename"></i>Rename</li>
							</ul>
							<p>Organize</p>
						</li>
						<li class="twoLinesLabel">
							<ul class="top-nav-options clearfix">
								<li data-action="newFolder"><i class="new-folder"></i>New<br/>folder</li>
							</ul>
							<p>New</p>
						</li>
						<li>
							<ul class="top-nav-options clearfix">
								<li class="requiresFileSelection" data-action="download"><i class="download"></i>Download</li>
								<li data-action="upload"><i class="upload"></i>Upload</li>
							</ul>
							<p>Transfer</p>
						</li>
						<li>
							<ul class="top-nav-options clearfix">
								<li class="requiresSingleSelection" data-action="open"><i class="open"></i>Open</li>
							</ul>
							<p>Open</p>
						</li>
						<li>
							<ul class="top-nav-options clearfix">
								<li class="requiresSelection" data-action="compress"><i class="compress"></i>Add to zip</li>
								<li data-action="extract"><i class="extract"></i>Extract all</li>
							</ul>
							<p>Compression</p>
						</li>
						<li class="twoLinesLabel">
							<ul class="top-nav-options no-float clearfix">
								<li data-action="selectAll" class="small"><i class="select-all"></i>Select all</li>
								<li data-action="selectNone" class="small"><i class="select-none"></i>Select none</li>
								<li data-action="invertSelection" class="small"><i class="invert-selection"></i>Invert selection</li>
							</ul>
							<p>Select</p>
						</li>
					</ul>
				</div>
				<div id="view-nav" class="top-nav-content clearfix" style="display:none">
					<ul class="top-nav-groups">
						<li class="twoLinesLabel">
							<ul class="top-nav-options clearfix">
								<li data-action="navigationPane"><i class="navigation-pane"></i>Navigation<br/>pane</li>
							</ul>
							<p>Panes</p>
						</li>
						<li class="twoColumns">
							<ul class="top-nav-options no-float clearfix">
								<li data-action="extraLargeIcons" class="small"><i class="extra-large-icons"></i>Extra large icons</li>
								<li data-action="mediumIcons" class="small"><i class="medium-icons"></i>Medium icons</li>
								<li data-action="details" class="small"><i class="details"></i>Details</li>
							</ul>
							<ul class="top-nav-options no-float clearfix">
								<li data-action="largeIcons" class="small"><i class="large-icons"></i>Large icons</li>
								<li data-action="smallIcons" class="small"><i class="small-icons"></i>Small icons</li>
								<li data-action="tiles" class="small"><i class="tiles"></i>Tiles</li>
							</ul>
							<p>Layout</p>
						</li>
					</ul>
				</div>
			</div>
			<div class="content">
				<div id="navigation-pane">
					<ul id="tree" class="ztree"></ul>
				</div>
				<div id="files">
					
					
					<ul class="clearfix">
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
						<li class="object large-icon folder">folder name</li>
					</ul>
					

				</div>
			</div>
		</div>
  </body>
</html>