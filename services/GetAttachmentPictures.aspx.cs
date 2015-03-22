using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Beweb;
public partial class svc_GetAttachmentPictures : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
		//load list of files		(from mce image.aspx)
		string subfolder = Web.Request["subfolder"];
		var currentPath = Server.MapPath(Web.Attachments + subfolder);
		FileSystem.CreateFolder(currentPath);    // in case does not exist
		int nCount=0;
		foreach (FileInfo file in new DirectoryInfo(currentPath).GetFiles())
		{
			string strExt = file.Extension.ToLower().Replace(".", "");
			
			//only show web friendly image types
			if ((strExt.Equals("jpg") || strExt.Equals("jpeg") || strExt.Equals("gif") || strExt.Equals("png") || strExt.Equals("bmp"))
				&&(file.Name.IndexOf("_tn")==-1)	//no thumbs
				&& (file.Name.IndexOf("_pv") == -1)	// no previews
				&& (file.Name.IndexOf("_original") == -1)	// no originals
				&& (file.Name.IndexOf("_med") == -1)	
				&& (file.Name.IndexOf("_sml") == -1)	
				&& (file.Name.IndexOf("_big") == -1)	
				&& (file.Name.IndexOf("_zm") == -1)	
				&& !file.Name.IsGuid()	//no guids
				)
			{
				string strFile = currentPath + "/" + file.Name;
				strFile = strFile.Replace("//", "/");
				string strStyle = nCount % 2 == 0 ? "SearchRow" : "SearchAltRow";
				
				//plcFileList.Controls.Add(new LiteralControl("<tr id=\"tr"+nCount+"\" onmouseover=\"this.className='SearchRowOver';\" onmouseout=\"SwitchClassBack(this,'" + strStyle + "');\" onclick=\"showPreview('" + strFile + "',this,'" + strStyle + "');\" class=\"" + strStyle + "\"><td>" + file.Name + "</td></tr>"));
				Response.Write(file.Name+"\n");
				nCount++;
			}
		}

  }
}
