<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<%@ Page Theme=""  Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Configuration" %>
<script runat="server">

	private string m_strBasePath = ConfigurationManager.AppSettings["tinyMCEImageBasePath"] + "docs/";
	private string m_strCurrentPath = null;
	private string m_strCurrentFile = null;
	private string pageLoadScript = "";

	protected override void OnLoad(EventArgs e)
	{
		if (m_strBasePath + "" == "") throw new Exception("config setting tinyMCEImageBasePath is missing");
		FileSystem.CreateFolder(m_strBasePath);

		if (IsPostBack)
		{
			HandlePostBack();
		}
		base.OnLoad(e);
	}
	
	protected override void Render(HtmlTextWriter writer)
	{
		LoadFiles();
		base.Render(writer);
	}

	private void HandlePostBack()
	{
		
		string strArg = Request["__EVENTARGUMENT"];
		if (strArg!=null)
		{

			if (strArg.StartsWith("CD|"))
			{
				string strFolder = strArg.Substring(3);
				if (strFolder.StartsWith("/")) strFolder = strFolder.Substring(1);
				if (!string.IsNullOrEmpty(strFolder))
				{
					m_strCurrentPath = m_strBasePath + "/" + strFolder;
				}
				m_strCurrentPath = m_strCurrentPath.Replace("//", "/");
				hdnPath.Value = m_strCurrentPath;
			}
			else if (strArg.StartsWith("UPLOAD"))
			{
				lblUpload.Text="";
				//changeTabJS = "mcTabs.displayTab('search_tab','search_panel');";
				m_strCurrentPath = m_strBasePath;
				if (!string.IsNullOrEmpty(hdnPath.Value))
					m_strCurrentPath = hdnPath.Value;
				string strExt = System.IO.Path.GetExtension(upload.FileName.ToLower());
				if (upload.FileName.Length == 0)
					lblUpload.Text = "No File!";
				else if (upload.FileContent.Length == 0)
					lblUpload.Text = "Nothing was done!";
				//else if (!strExt.Equals(".exe") && !strExt.Equals(".dll") && !strExt.Equals(".asp") && !strExt.Equals(".vbs"))
				else if (!FileSystem.IsDangerous(upload.FileName))
				{
					string strFilename="";
				
					// make unique
					strFilename = Beweb.Util.GetUniqueFilename(ResolveURL(m_strCurrentPath),upload.FileName);
					strFilename = Server.MapPath(m_strCurrentPath + "/" + strFilename);
					try
					{
						lblUpload.Text="<b style='color:green;'>Please wait...</b>";
						upload.SaveAs(strFilename);
						m_strCurrentFile = ResolveURL(CurrentPath) + "/" + System.IO.Path.GetFileName(strFilename);
						m_strCurrentFile = m_strCurrentFile.Replace("//", "/");
						lblUpload.Text="<b style='color:green;'>Upload complete.</b>";
					}
					catch (Exception ex)
					{
						lblUpload.Text = "<b>Error uploading:</b> " + ex.Message;
					}
				}
				else 
					lblUpload.Text = "<b>Sorry, "+strExt.ToUpper()+" files are not allowed</b>";
			}
			else if (strArg.StartsWith("DELETE"))
			{
				string filename = Request["href"];
				if (filename.Contains("attachments/docs/")) {
					filename = filename.RightFrom("attachments/docs/");
					FileSystem.DeleteAttachment("docs/"+filename);
					pageLoadScript = "window.setTimeout('removeLink()',500)";
				} else {
					error_panel.Visible = true;
					general_panel.Visible = false;
					error_panel.InnerText = "The file link '"+filename+"' is not able to be deleted. Only files in the attached documents folder can be deleted.";
				}
			}
		}
	}
	private static string BaseURL
	{
		get
		{
			string strURL = System.Web.HttpContext.Current.Request.ApplicationPath;
			if (!strURL.EndsWith("/"))
			{
				strURL += "/";
			}
			return strURL;
		}
	}
	 
	private static string ResolveURL(string strURL)
	{
		if (strURL.Contains("~/"))
		{
			strURL = strURL.Replace("~/", BaseURL);
		}
		return strURL;
	}

	private string CurrentPath
	{
		get
		{
			if (m_strCurrentPath == null) return m_strBasePath;
			return m_strCurrentPath;
		}
	}

	private void LoadFiles()
	{
		if (string.IsNullOrEmpty(CurrentPath))
		{
			plcFileList.Controls.Add(new LiteralControl("No files available"));
		}
		else
		{
			LinkButton lnk = new LinkButton();
			lnk.Text = "force!!!";
			plcHidden.Controls.Add(lnk);
			
			plcFileList.Controls.Add(new LiteralControl("<table id=\"filelist\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">"));
			string strBase = Server.MapPath(CurrentPath);
			int nCount = 0;

/*
			if (CurrentPath != m_strBasePath)
			{
				string strUp = CurrentPath;
				strUp = strUp.Substring(0, strUp.LastIndexOf("/"));
				strUp = strUp.Replace(m_strBasePath, "");
				string strStyle = nCount % 2 == 0 ? "SearchRow" : "SearchAltRow";
				//if (false )//show parent
				//{	
				//  plcFileList.Controls.Add(new LiteralControl("<tr onmouseover=\"this.className='SearchRowOver';\" onmouseout=\"this.className='" + strStyle + "';\" onclick=\"changeDir('" + strUp + "');\" class=\"" + strStyle + "\"><td><img align=\"top\" src=\"images/folderup.gif\" />&nbsp;"));
				//  plcFileList.Controls.Add(new LiteralControl("[ ... ]</td></tr>"));
				//}
				nCount++;
			}
			//load list of sub directories
			//if (false )
			//{	
			//  foreach (DirectoryInfo dir in new DirectoryInfo(Server.MapPath(CurrentPath)).GetDirectories())
			//  {
			//    string strFile = dir.FullName;
			//    strFile = strFile.Replace(Server.MapPath(m_strBasePath), "").Replace("\\", "/");
			//    string strStyle = nCount % 2 == 0 ? "SearchRow" : "SearchAltRow";
			//    plcFileList.Controls.Add(new LiteralControl("<tr onmouseover=\"this.className='SearchRowOver';\" onmouseout=\"SwitchClassBack(this,'" + strStyle + "');\" onclick=\"changeDir('" + strFile + "');\" class=\"" + strStyle + "\"><td><img align=\"top\" src=\"images/folder.gif\" />&nbsp;"));
			//    plcFileList.Controls.Add(new LiteralControl("[ " + dir.Name + " ]</td></tr>"));
			//    nCount++;
			//  }
			//}
*/

			string strStyle, strFile;
			if (m_strCurrentFile!=null) {
				// just uploaded a file
				strFile = m_strCurrentFile.ReplaceFirst(ResolveURL(CurrentPath),"");
				strFile = strFile.Replace("//", "/");
				nCount = 0;
				strStyle = "SearchAltRow";
				plcFileList.Controls.Add(new LiteralControl("<tr id=\"tr"+nCount+"\" onmouseover=\"this.className='SearchRowOver';\" onmouseout=\"SwitchClassBack(this,'" + strStyle + "');\" onclick=\"showPreview('" + strFile + "',this,'" + strStyle + "');\" class=\"" + strStyle + "\"><td>" + strFile + "</td></tr>"));
				pageLoadScript = "window.setTimeout(\"showPreview('" + m_strCurrentFile + "',document.getElementById('tr" + nCount + "'),'" + strStyle + "')\",800);";
				nCount++;
			}

			//load list of files
			foreach (FileInfo file in new DirectoryInfo(Server.MapPath(CurrentPath)).GetFiles())
			{
				string strExt = file.Extension.ToLower().Replace(".", "");
				
				//only show web friendly file types
				//if ((strExt.Equals("doc") || strExt.Equals("xls") || strExt.Equals("pdf") || strExt.Equals("zip") || strExt.Equals("txt"))
				if (!FileSystem.IsDangerous(file.Name)
					&&(file.Name.IndexOf("_tn")==-1)	//no thumbs
					&&(file.Name.IndexOf("_pv")==-1)	// no previews
					&&!(file.Name.Length==40 &&file.Name.IndexOf("-")!=-1)	//no guids
					)
				{
					strFile = ResolveURL(CurrentPath) + "/" + file.Name;
					strFile = strFile.Replace("//", "/");
					strStyle = nCount % 2 == 0 ? "SearchRow" : "SearchAltRow";
					if (strFile != m_strCurrentFile) {
						plcFileList.Controls.Add(new LiteralControl("<tr id=\"tr"+nCount+"\" onmouseover=\"this.className='SearchRowOver';\" onmouseout=\"SwitchClassBack(this,'" + strStyle + "');\" onclick=\"showPreview('" + strFile + "',this,'" + strStyle + "');\" class=\"" + strStyle + "\"><td>" + file.Name + "</td></tr>"));
						nCount++;
					} 
					
				}
			}
			plcFileList.Controls.Add(new LiteralControl("</table>"));
		}
	}

	protected void hdnButton_Click(object sender, EventArgs e)
	{
		//do nothing
	}



</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Insert downloadable document / file attachment</title>
	<script language="JavaScript" type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/mctabs.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/form_utils.js"></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/editable_selects.js" ></script>
	<script language="JavaScript" type="text/javascript" src="../../utils/validate.js"> </script>	
	<script language="JavaScript" type="text/javascript" src="js/mediaupload.js"></script>
	<link href="css/mediaupload.css" rel="stylesheet" type="text/css" />
	<base target="_self" />
	
<%--  <script type="text/javascript" src="<%=ResolveUrl("~/areas/admin/") %>help/helpwindow.js"></script>
	<link rel="stylesheet" href="<%=ResolveUrl("~/areas/admin/") %>help/helpwindow.css" type="text/css" />
	<link rel="stylesheet" href="<%=ResolveUrl("~/areas/admin/") %>help/help.css" type="text/css" />
--%>
	<script language="javascript" type="text/javascript" >
	var lastClass;
	var lastTD;
	
	function filterlist() 
	{
		var filterbox = document.getElementById('filterbox');
	
		for(var index=0;;index++)
		{
			var tr = document.getElementById('tr'+index);
			if(!tr)break;
			if(tr && tr.innerHTML.indexOf(filterbox.value)==-1)
			{
				tr.style.display='none';
			}else
			{
				tr.style.display='table-row';
			}
			
		}
	}
	
	function showPreview(src, td, Class) 
	{
		if (lastTD) 
		{
			lastTD.className = lastClass;
		}
		lastTD = td;
		lastClass = Class;
		td.className = 'SearchRowOver';
		//'select' in the general tab, but dont change to it
		var s = document.getElementById("href");
		s.value = src;


		//file name preview
		//var att = document.getElementById('filenamefull');
		//att.innerHTML = src
		//att = document.getElementById('filenamepreview');
		//att.innerHTML = td.firstChild.innerHTML //src

		//hide message about uploading file since this is no longer particularly relevant
		document.getElementById("lblUpload")

		return false;
	}
			
	
	function SwitchClassBack(td, Class)
	{
		if (td == lastTD) {
			return;
		}
		td.className = Class;
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

	function uploadAttachment() {
		var theForm = document.forms['form1'];
		if (!theForm) {
			theForm = document.form1;
		}
		theForm.__EVENTTARGET.value = 'plcFileList';
		theForm.__EVENTARGUMENT.value = "UPLOAD";
		document.getElementById("lblUpload").innerHTML = "<span class='loading'>Uploading...</span>";
		theForm.submit();
	}

	function deleteAttachment() {
		var theForm = document.forms['form1'];
		if (!theForm) {
			theForm = document.form1;
		}
		theForm.__EVENTTARGET.value = 'plcFileList';
		theForm.__EVENTARGUMENT.value = "DELETE";
		theForm.submit();
	}

	</script>
</head>
<body id="advlink" style="display: none">
	<form onsubmit="insertAction();return false;" action="attachment.aspx" name="form1" id="form1" runat="server">
		<input type="hidden" name="href" id="href" value="" />
		<input runat="server" id="hdnPath" type="hidden" />
		<div class="panel_wrapper">
			<div runat="server" id="error_panel" class="panel current" visible="false">
			</div>
			<div runat="server" id="general_panel" class="panel current">
				<div id="delete_panel" style="display:none">
					<b>Remove existing document link</b><br />
					<br />
					You have selected an existing downloadable document link. To remove this file from the server and the link to it, click 'Delete File'. If this file is being used in another place click 'Remove Link' instead.<br />
					<br />
					Note: to remove a file that is indexed by Google you must delete the file.<br />
					<br />
					Tip: to change a downloadable document, first delete it and then upload a new file.<br />
					<br />
					Document File Name: <span id="DeleteFilename" style="font-weight:bold"></span><br />
					<br />
				</div>
				<div id="upload_panel">
				<b>1.</b> Click 'Browse' (or 'Choose File') below to upload a file from your computer. If the file has already been uploaded, you can skip this step.<br />
				<br />
				<fieldset>
					<legend>Upload a file from your computer to the site</legend>
					<asp:FileUpload CssClass="upload" ID="upload" runat="server" onchange="uploadAttachment();" />
					<input class="uploadButton"  onclick="uploadAttachment();" type="button" name="btnUpload" value="Upload" style="display:none;float:left;margin-left:10px;width:100px;" />
					<asp:Label ID="lblUpload" runat="server" ForeColor="red" Text=""></asp:Label>
				</fieldset>
				<br />
				<b>2.</b> Select the file below that you wish to insert into the text.<br />
				<br />
				<fieldset>
					<legend>Files already uploaded to the site</legend>
					<table cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td xrowspan="2">
							<div class="search" style="overflow-y:auto;height:180px;" id="filelist">
								<asp:PlaceHolder ID="plcFileList" runat="server">
								</asp:PlaceHolder>
							</div>
						</td>
					</tr>
					<tr>
						<td valign="bottom" align="left">
						Find file name <input style="width:165px;" type="text" size="15" id="filterbox" onkeyup="filterlist();"/><!--<input style="width:25px;" onclick="filterlist();return false;"  type="button" value="go"/> -->
						</td>
					</tr>
					</table>
				</fieldset>
				<br />
				<b>3.</b> Click 'Insert' to insert the file download link into the text.<br />
				<br />

			</div>
			</div>

		</div>
		<div style="clear:both">

		<div class="mceActionPanel" style="margin-top: 60px;">
			<div style="float: left">
				<input type="submit" id="insert" name="insert" value="Insert" />
				<input type="button" id="delete" name="delete" value="Delete File" onclick="deleteAttachment()" style="display:none" />
				<input type="button" id="unlink" name="unlink" value="Remove Link" onclick="removeLink()" style="display:none" />
			</div>

			<div style="float: right">
				<input type="button" id="cancel" name="cancel" value="{#cancel}" onclick="tinyMCEPopup.close();" />
			</div>
		</div>
		<div style="display:none">
		<asp:PlaceHolder ID="plcHidden" runat="server"></asp:PlaceHolder></div>
				
<script language="javascript"  type="text/javascript">
<!-- //-------------------------------------------------------------------------
		<%=pageLoadScript %>
//-------------------------------------------------------------------------- -->
</script>
	</form>
</body>
</html>
