<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<%@ Page Theme=""  Language="C#" EnableViewState="true" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Site.SiteCustom" %>
<script runat="server">

	private string m_strAttachPath = ConfigurationManager.AppSettings["tinyMCEImageBasePath"];
	private string m_strBasePath = ConfigurationManager.AppSettings["tinyMCEImageBasePath"] + "pics";
	//private string m_strBasePath = ConfigurationManager.AppSettings["tinyMCEImageBasePath"];

	private string m_strCurrentPath = null;
	private string m_strCurrentFile = null;
	private string changeTabJS = "";
	private string fileType = "";
	public bool RetainOriginal = false;

	protected override void OnLoad(EventArgs e) {
		if (m_strBasePath + "" == "") throw new Exception("config setting tinyMCEImageBasePath is missing");
		FileSystem.CreateFolder(m_strBasePath);
		
		if (IsPostBack) {
			HandlePostBack();
		}
		base.OnLoad(e);
	}

	protected override void Render(HtmlTextWriter writer) {
		//LoadFiles();
		
		base.Render(writer);
	}

	private void HandlePostBack() {

		string strArg = Request["__EVENTARGUMENT"];
		//string filename = upload.FileName;
		string pastedFileName = Request.Form["imageName"];

		if (strArg != null) {
			general_tab.Attributes["class"] = "";
			general_panel.Attributes["class"] = "panel";

			if (strArg.StartsWith("CD|")) {
				string strFolder = strArg.Substring(3);
				if (strFolder.StartsWith("/")) strFolder = strFolder.Substring(1);
				if (!string.IsNullOrEmpty(strFolder)) {
					m_strCurrentPath = m_strBasePath + "/" + strFolder;
				}
				m_strCurrentPath = m_strCurrentPath.Replace("//", "/");
				hdnPath.Value = m_strCurrentPath;
			} else if (strArg.StartsWith("UPLOAD")) { }
		}
	}
	private static string BaseURL {
		get {
			string strURL = System.Web.HttpContext.Current.Request.ApplicationPath;
			if (!strURL.EndsWith("/")) {
				strURL += "/";
			}
			return strURL;
		}
	}

	private static string ResolveURL(string strURL) {
		if (strURL.Contains("~/")) {
			strURL = strURL.Replace("~/", BaseURL);
		}
		return strURL;
	}

	private string CurrentPath {
		get {
			if (m_strCurrentPath == null) return m_strBasePath;
			return m_strCurrentPath;
		}
	}

</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head xrunat="server">
	<title>{#advimage_dlg.dialog_title}</title>
	<script language="JavaScript" type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/mctabs.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/form_utils.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/editable_selects.js" ></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/validate.js"> </script>	
	<%Util.IncludejQuery();%>
	<%Util.IncludeBewebForms();%>

	<script language="JavaScript" type="text/javascript" src="<%=Web.Root%>js/common.js"></script>
	<script type="text/javascript">	websiteBaseUrl = "<%=Web.BaseUrl%>"</script>	
	<script>var isMobile=<%=Web.IsMobile.ToString().ToLower() %></script>
	<script language="JavaScript" type="text/javascript" src="js/image.js"> </script>

	<link href="../../themes/advanced/skins/default/ui.css" rel="stylesheet" type="text/css" />
	<link href="css/advimage.css" rel="stylesheet" type="text/css" />	
	<link href="<%=Web.Root%>areas/admin/admin.css" rel="stylesheet" type="text/css" />
	<base target="_self" />
	
  <script type="text/javascript" src="<%=ResolveUrl("~/admin/") %>help/helpwindow.js"></script>
	<link rel="stylesheet" href="<%=ResolveUrl("~/admin/") %>help/helpwindow.css" type="text/css" />
  <link rel="stylesheet" href="<%=ResolveUrl("~/admin/") %>help/help.css" type="text/css" />
	<script language="javascript" type="text/javascript" >
	var lastClass;
	var lastTD;

	function SetImage(src){
		mcTabs.displayTab('general_tab','general_panel');
		var s = document.getElementById('src');
		s.value = src;
		s.value = convertURL(src);
		showPreviewImage(src, 0);
		ImageDialog.showPreviewImage(src)
	}
	
	function filterlist(){
		var filterbox = document.getElementById('filterbox');
		for(var index=0;;index){
			var tr = document.getElementById('tr'+index);
			if(!tr)break;
			if(tr && tr.innerHTML.indexOf(filterbox.value)==-1){
				tr.style.display='none';
			}else{
				tr.style.display='table-row';
			}
		}
	}
	
	function showPreview(src, td, Class){
		if (lastTD){
			lastTD.className = lastClass;
		}
		lastTD = td;
		lastClass = Class;
		td.className = 'SearchRowOver';
		//'select' in the general tab, but dont change to it
		var s = document.getElementById('src');
		s.value = src;
		ImageDialog.showPreviewImage(src)

		loadPreview(src);
		return false;
	}
	
	function loadPreview(src){
		var img = document.getElementById('prevImage');
	
		var imgPreloader = new Image();
		imgPreloader.onload = function(){
			
		imgPreloader.onload = null;
		
		var x = 190;
		var y = 220;
		var imageWidth = imgPreloader.width;
		var imageHeight = imgPreloader.height;
		var chkResize = document.getElementById('chkResize');
		if (chkResize.checked) {
			if (imageWidth > 190 | imageHeight > 220) {
				var wscale = x / imageWidth;
				var hscale = y / imageHeight;
				var scale = (hscale < wscale ? hscale : wscale);
				imageWidth *= scale;
				imageHeight *= scale;
			}
		}

		img.src = src;
		img.width = imageWidth;
		img.height = imageHeight;
		};
		
		if (!src){
			src = img.src;
		}
		imgPreloader.src = src;		
	}
	
	function SwitchClassBack(td, Class){
		if (td == lastTD) {
			return;
		}
		td.className = Class;
	}

	function selectPic() {
		var img = document.getElementById('prevImage');
		SetImage(img.src);
	}
	
	function changeDir(dir){
		var theForm = document.forms['form1'];
		if (!theForm) {
			theForm = document.form1;
		}
		theForm.__EVENTTARGET.value = 'plcFileList';
		theForm.__EVENTARGUMENT.value = "CD|"+dir;
		theForm.submit();
	}
	
	function uploadPic() {
		var theForm = document.forms['form1'];
		if (!theForm) {
			theForm = document.form1;
		}
		theForm.__EVENTTARGET.value = 'plcFileList';
		theForm.__EVENTARGUMENT.value = "UPLOAD";
		theForm.submit();
	}


	function ShowPrevImage() {
		$('#prevImage').show();
		$('#imagePaste').hide();
	}

	function ShowImagePaste() {
		$('#prevImage').hide();
		$('#imagePaste').show();
	}

	function CancelChangePicture() {
		$('#imagepreview').show();
		$('#imagechoose').hide();
		$("#preview-toggle-change").show();
		$("#preview-toggle-cancel").hide();
	}

	function ChangePicture() {
		$('#imagepreview').hide();
		$('#imagechoose').show();
		svyResetAfterCancelAction("mce_Paste");
		$("#preview-toggle-change").hide();
		$("#preview-toggle-cancel").show();
	}

	</script>
	<style>
		html { overflow-y: hidden !important;}
		a.pastelink span {text-decoration: none;color:#2B6FB6}
		a.pastelink:hover span {text-decoration: underline;color:#2B6FB6;cursor: pointer;}
		.panel_wrapper { width: 570px; }
		#general_panel { height: initial;}
		.svyPictureContainer {margin: 30px 0;}
		.margin-input { width: 50px;}
		.mceActionPanel {position: relative;bottom:-10px;right: -13px; float: right;}
		.steps { padding: 3px 0;}
	</style>
	<!--[if lt IE 10]>
		<style> input[type=file] { background: #fff; border: 1px solid #aaa; } </style>
	<![endif]-->
</head>

<body id="advimage" style="display: none;">
	<form  onsubmit="insertAction();return false;" action="image.aspx" name="form1" id="form1" runat="server">

		<div class="tabs hide" >
			<ul>
				<li class="current" runat="server" id="general_tab"><span><a href="javascript:mcTabs.displayTab('general_tab','general_panel');" onmousedown="return false;">{#advimage_dlg.tab_general}</a></span></li>
				<li runat="server" id="advanced_tab" style="display:none"><span><a href="javascript:mcTabs.displayTab('advanced_tab','advanced_panel');" onmousedown="return false;">{#advimage_dlg.tab_advanced}</a></span></li>
			</ul>
		</div>

		<input runat="server" id="hdnPath" type="hidden" />
		<div class="panel_wrapper">
			<div runat="server" id="general_panel" class="panel current">
				<div id="imagechoose">
				<span class="steps" style="padding-top: 0"><b>1.</b> Click 'Upload a file' below to upload a file from your computer. If the file has already been uploaded, you can use the 'select uploaded image' button. </span>
				
				<!-- File Upload Dialog Box -->
				<fieldset class="custom-fieldset" style="height: 310px;">
					<legend>Upload a picture from your computer to the site</legend>
					<table border=0>
						<tr>
							<td style="width:0px;"><br><br><br><br><br>
								<input type=hidden name="ContentWidth" value="<%=Request["ContentWidth"]%>" />
							</td>
							<td valign="top" class="defaultSkin">
								<%= new Forms.PictureField("mce_Paste","mce_Paste",true) { MetaData = new DefaultPictureMetaData(){AllowPasteAndDrag = true, IsExact = true, ShowDimensionMessage = false,ShowCropResizeChoice = true,ShowRemoveCheckbox = false,ShowFreeImageSearchLinks = true}} %>
								<input type="hidden" id="mceImagePasteUpload" name="mceImagePasteUpload" value="true" />
								<input type="hidden" id="imageName" name="imageName" value="" />
							</td>
						</tr>
					</table>
				</fieldset>
				</div>
				
				<div id="imagepreview" style="height: 358px;">
					<span class="steps" style="padding-top: 0"><b>1.</b> Preview the current image or hit the change button to replace it. <br/> &nbsp; </span>
					<fieldset class="custom-fieldset">
						<legend>{#advimage_dlg.preview}</legend>
						<div id="prev"></div>
					</fieldset>
				</div>
				<div id="changepicture" class="svyPicOptions svyLinkContainer" style="margin-top: -38px;position: relative;float: right;margin-right: 10px;">
					<a id="preview-toggle-change" class="svyPasteLink btn btn-mini" href="#" onclick=" ChangePicture();return false;"><i class="icon icon-picture"></i> Change picture</a>
					<a id="preview-toggle-cancel" class="svyPasteLink btn btn-mini" style="display: none;" href="#" onclick=" CancelChangePicture();return false;"><i class="icon icon-remove"></i> Cancel</a>
				</div>
				
				<span class="steps"><b>2.</b> Give your image alternative text to help its page rank. </span>

				<!-- Search Engine Optimisation / Alt tag -->
				<fieldset class="custom-fieldset">
						<legend> Search Engine Optimisation / Accessiblity </legend>
						<table class="properties">
							<tr style="display:none">
								<td class="column1"><label id="srclabel" for="src">{#advimage_dlg.src}</label></td>
								<td colspan="2"><table border="0" cellspacing="0" cellpadding="0">
									<tr> 
									  <td><input name="src" type="text" id="src" value="" onchange="ImageDialog.showPreviewImage(this.value);" /></td> 
									  <td id="srcbrowsercontainer">&nbsp;</td>
									</tr>
								  </table></td>
							</tr>
							<tr>
								<td><label for="src_list">{#advimage_dlg.image_list}</label></td>
								<td><select id="src_list" name="src_list" onchange="document.getElementById('src').value=this.options[this.selectedIndex].value;document.getElementById('alt').value=this.options[this.selectedIndex].text;document.getElementById('title').value=this.options[this.selectedIndex].text;ImageDialog.showPreviewImage(this.options[this.selectedIndex].value);"><option value=""></option></select></td>
							</tr>
							<tr> 
								<td class="column1"><label id="altlabel" for="alt">Image Alt Text</label></td> 
								<td colspan="2"><input id="alt" name="alt" type="text" value="" /></td> 
							</tr> 
							<tr style="display:none"> 
								<td class="column1"><label id="titlelabel" for="title">{#advimage_dlg.title}</label></td> 
								<td colspan="2"><input id="title" name="title" type="text" value="" /></td> 
							</tr>
						</table>
				</fieldset>



					<%--<bwb:Help ID="Help1" runat="server" Text="In the Link URL box, type mailto:testemail@emailcompany.co.nz, then click Insert."></bwb:Help>--%>										
			
				<span class="steps"><b>3.</b> Add additional settings to make your image look perfect on the page. (optional)</span>

				<!-- Appearance -->
				<fieldset class="custom-fieldset">
					<legend>{#advimage_dlg.tab_appearance}</legend>

					<table border="0" cellpadding="4" cellspacing="0">
						<tr> 
							<td class="column1"><label id="alignlabel" for="align">{#advimage_dlg.align}</label></td> 
							<td><select id="align" name="align" onchange="ImageDialog.updateStyle('align');ImageDialog.changeAppearance();"> 
									<option value="">{#not_set}</option> 
									<option value="baseline">{#advimage_dlg.align_baseline}</option>
									<option value="top">{#advimage_dlg.align_top}</option>
									<option value="middle">{#advimage_dlg.align_middle}</option>
									<option value="bottom">{#advimage_dlg.align_bottom}</option>
									<option value="text-top">{#advimage_dlg.align_texttop}</option>
									<option value="text-bottom">{#advimage_dlg.align_textbottom}</option>
									<option value="left">{#advimage_dlg.align_left}</option>
									<option value="right">{#advimage_dlg.align_right}</option>
								</select> 
							</td>

							<td rowspan="6" valign="top">
								<div class="alignPreview" style="display:none">
									<img id="alignSampleImg" src="img/sample.gif" alt="{#advimage_dlg.example_img}" />
									Lorem ipsum, Dolor sit amet, consectetuer adipiscing loreum ipsum edipiscing elit, sed diam
									nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.Loreum ipsum
									edipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam
									erat volutpat.
								</div>
							</td>
						</tr>

						<tr>
							<td class="column1"><label id="widthlabel" for="width">{#advimage_dlg.dimensions}</label></td>
							<td nowrap="nowrap" class="nowrap">
							<input name="width" type="text" id="width" value=""  size="5" maxlength="5" class="size" placeholder="width" onchange="ImageDialog.changeHeight();" /> x
								<input name="height" type="text" id="height" value="" size="5" maxlength="5" class="size" placeholder="height"  onchange="ImageDialog.changeWidth();" /> pixels (leave blank for actual size)
							</td>
						</tr>

						<tr style="display:none">
							<td>&nbsp;</td>
							<td><table border="0" cellpadding="0" cellspacing="0">
									<tr >
										<td><input id="constrain" type="checkbox" name="constrain" class="checkbox" /></td>
										<td><label id="constrainlabel" for="constrain">{#advimage_dlg.constrain_proportions}</label></td>
									</tr>
								</table></td>
						</tr>
						<tr>
							<td class="column1"><label id="Label1" for="align">Margins</label></td> 
								<%--<td class="column1"><label id="vspacetoplabel" for="vspacetop">Top</label></td> --%>
							<td>
								<input name="vspacetop" type="text" placeholder="top" id="vspacetop" value="" size="3" maxlength="3" class="number margin-input" onchange="ImageDialog.updateStyle('vspacetop');ImageDialog.changeAppearance();" onblur="ImageDialog.updateStyle('vspacetop');ImageDialog.changeAppearance();" />

								<%--<td class="column1"><label id="hspaceleftlabel" for="hspaceleft">Left</label></td> --%>
								<input name="hspaceleft" type="text" placeholder="left" id="hspaceleft" value="" size="3" maxlength="3" class="number margin-input" onchange="ImageDialog.updateStyle('hspaceleft');ImageDialog.changeAppearance();" onblur="ImageDialog.updateStyle('hspaceleft');ImageDialog.changeAppearance();" />

								<input name="hspaceright" type="text" placeholder="right" id="hspaceright" value="" size="3" maxlength="3" class="number margin-input" onchange="ImageDialog.updateStyle('hspaceright');ImageDialog.changeAppearance();" onblur="ImageDialog.updateStyle('hspaceright');ImageDialog.changeAppearance();" />

								<%--<td class="column1"><label id="vspacebottomlabel" for="vspacebottom">Bottom</label></td> --%>
								<input name="vspacebottom" type="text"  placeholder="bottom" id="vspacebottom" value="" size="3" maxlength="3" class="number margin-input" onchange="ImageDialog.updateStyle('vspacebottom');ImageDialog.changeAppearance();" onblur="ImageDialog.updateStyle('vspacebottom');ImageDialog.changeAppearance();" />
								pixels (leave blank for no margin)
							</td>
							<%--<td class="column1"><label id="hspacerightlabel" for="hspaceright">Right</label></td> --%>
						</tr>
						
						<tr style="display:none">
							<td class="column1"><label id="borderlabel" for="border">{#advimage_dlg.border}</label></td> 
							<td><input id="border" name="border" type="text" value="" size="3" maxlength="3" class="number" onchange="ImageDialog.updateStyle('border');ImageDialog.changeAppearance();" onblur="ImageDialog.updateStyle('border');ImageDialog.changeAppearance();" /></td> 
						</tr>
						<tr style="display:none">
							<td><label for="class_list">{#class_name}</label></td>
							<td colspan="2"><select id="class_list" name="class_list" class="mceEditableSelect"><option value=""></option></select></td>
						</tr>

						<tr style="display:none">
							<td class="column1"><label id="stylelabel" for="style">{#advimage_dlg.style}</label></td> 
							<td colspan="2"><input id="style" name="style" type="text" value="" onchange="ImageDialog.changeAppearance();" /></td> 
						</tr>

						 <tr style="display:none">
							<td class="column1"><label id="classeslabel" for="classes">{#advimage_dlg.classes}</label></td> 
							<td colspan="2"><input id="classes" name="classes" type="text" value="" onchange="selectByValue(this.form,'classlist',this.value,true);" /></td> 
						</tr> 
					</table>
				</fieldset>
			
					<div class="mceActionPanel">
						<input type="button" id="cancel" name="cancel" value="{#cancel}" onclick="tinyMCEPopup.close();" style="float:right;" />
						<input type="button" id="insert" name="insert" class="svySaveButton" value="{#insert}" onclick="ImageDialog.insert();" style="float:right;" />
					</div>
				</div>
			</div>

			<fieldset style="display:none">
				<div>
					<legend>Click the 'Select' button when you have clicked your image.</legend>
					<input class="uploadButton topSpace" onclick="selectPic();" type="button" name="btnSelect" value="Select" />
				</div>
			</fieldset>


			
			<div id="advanced_panel" class="panel" style="display:none">
				<fieldset>
					<legend>{#advimage_dlg.swap_image}</legend>

					<input type="checkbox" id="onmousemovecheck" name="onmousemovecheck" class="checkbox" onclick="ImageDialog.setSwapImage(this.checked);" />
					<label id="onmousemovechecklabel" for="onmousemovecheck">{#advimage_dlg.alt_image}</label>

					<table border="0" cellpadding="4" cellspacing="0" width="100%">
							<tr>
								<td class="column1"><label id="onmouseoversrclabel" for="onmouseoversrc">{#advimage_dlg.mouseover}</label></td> 
								<td><table border="0" cellspacing="0" cellpadding="0"> 
									<tr> 
									  <td><input id="onmouseoversrc" name="onmouseoversrc" type="text" value="" /></td> 
									  <td id="onmouseoversrccontainer">&nbsp;</td>
									</tr>
								  </table></td>
							</tr>
							<tr>
								<td><label for="over_list">{#advimage_dlg.image_list}</label></td>
								<td><select id="over_list" name="over_list" onchange="document.getElementById('onmouseoversrc').value=this.options[this.selectedIndex].value;"><option value=""></option></select></td>
							</tr>
							<tr> 
								<td class="column1"><label id="onmouseoutsrclabel" for="onmouseoutsrc">{#advimage_dlg.mouseout}</label></td> 
								<td class="column2"><table border="0" cellspacing="0" cellpadding="0"> 
									<tr> 
									  <td><input id="onmouseoutsrc" name="onmouseoutsrc" type="text" value="" /></td> 
									  <td id="onmouseoutsrccontainer">&nbsp;</td>
									</tr> 
								  </table></td> 
							</tr>
							<tr>
								<td><label for="out_list">{#advimage_dlg.image_list}</label></td>
								<td><select id="out_list" name="out_list" onchange="document.getElementById('onmouseoutsrc').value=this.options[this.selectedIndex].value;"><option value=""></option></select></td>
							</tr>
					</table>
				</fieldset>

				<fieldset>
					<legend>{#advimage_dlg.misc}</legend>

					<table border="0" cellpadding="4" cellspacing="0">
						<tr>
							<td class="column1"><label id="idlabel" for="id">{#advimage_dlg.id}</label></td> 
							<td><input id="id" name="id" type="text" value="" /></td> 
						</tr>

						<tr>
							<td class="column1"><label id="dirlabel" for="dir">{#advimage_dlg.langdir}</label></td> 
							<td>
								<select id="dir" name="dir" onchange="ImageDialog.updateStyle();ImageDialog.changeAppearance();"> 
										<option value="">{#not_set}</option> 
										<option value="ltr">{#advimage_dlg.ltr}</option> 
										<option value="rtl">{#advimage_dlg.rtl}</option> 
								</select>
							</td> 
						</tr>

						<tr>
							<td class="column1"><label id="langlabel" for="lang">{#advimage_dlg.langcode}</label></td> 
							<td>
								<input id="lang" name="lang" type="text" value="" />
							</td> 
						</tr>

						<tr>
							<td class="column1"><label id="usemaplabel" for="usemap">{#advimage_dlg.map}</label></td> 
							<td>
								<input id="usemap" name="usemap" type="text" value="" />
							</td> 
						</tr>

						<tr>
							<td class="column1"><label id="longdesclabel" for="longdesc">{#advimage_dlg.long_desc}</label></td>
							<td><table border="0" cellspacing="0" cellpadding="0">
									<tr>
									  <td><input id="longdesc" name="longdesc" type="text" value="" /></td>
									  <td id="longdesccontainer">&nbsp;</td>
									</tr>
								</table></td> 
						</tr>
					</table>
				</fieldset>
			</div>

		<div style="display:none">
			<asp:PlaceHolder ID="plcHidden" runat="server"></asp:PlaceHolder>
		</div>
        
		<script language="javascript"  type="text/javascript">
		<!-- //-------------------------------------------------------------------------
			window.setTimeout("<%=changeTabJS %>", 1250)
			//$(document).ready(function () {//hack to overwrite the stupid mce defaults
			//	setTimeout(function () { svyResetImageMceWindow() }, 50);
			//	setTimeout(function () { svyMoveWindow(window) }, 100);
			//});		
		//-------------------------------------------------------------------------- -->
		</script>
	</form>
</body>
</html>
