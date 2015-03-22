//#define HasModels
using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Beweb;

namespace Savvy
{

	/// <summary>
	/// Navigation and Page Hierarchy
	/// </summary>
	public class Hierarchy
	{		

		//-------------------------------------------------------------
		// navigation functions
		//-------------------------------------------------------------
		public static string GetEditRenderPage(int pageID)
		{
			string result = Beweb.BewebData.GetValue(new Sql("select editrenderpage from template inner join page on template.templateid=page.templateid  where historypageid is null and pageid=", pageID));
			if(result=="")result = "PageEdit.aspx";
			return result;
		}

		public static string GetRenderPage(int recordID)
		{
			return Web.Root  + GetRenderPageIterator(recordID, 0);//"page.aspx";
		}
		public static string GetRenderPage(string recordID)
		{
			return GetRenderPage(Convert.ToInt32(recordID));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sectionCode">sectionCode or pagecode in page table</param>
		/// <returns></returns>
		public static int GetRenderPageID(string sectionCode)
		{
			string sql = "select pageid from page  where historypageid is null and  pagecode='" + sectionCode + "'";
			return (new DataBlock(BewebData.GetConnectionString(),true)).open(sql).GetValueInt(0);//dont call execute (no close)
		}



		//-------------------------------------------------------------------------------
		protected static string GetRenderPageIterator(int? id, int depth)
		{
			int ?parentID;//, str, title;
			string result = "";
			string sql;
			if(id!=null)
			{
				sql = "select RenderPage from template inner join Page on template.templateid=page.templateid where  historypageid is null and PageID=";
				result = BewebData.GetValue(new Sql(sql, id.Value));
				if(""+result == "")
				{
					parentID = BewebData.GetInt("select ParentPageID from Page where PageID=" + id);
					if ((parentID!=null) && parentID+""!="" && parentID+""!="0")
					//if(parentID!=null)
					{	
						result = GetRenderPageIterator(parentID, depth+1);
					}else
					{
						result = "standardpage.aspx";
					}
				}
			}
			
			return result;
		}



		public static string GetRenderPageSectionURL(string sectionCode)
		{
			return GetRenderPageSectionURL(sectionCode, true);
		}

		public static  string GetRenderPageSectionURL(string sectionCode, bool encrypt)
		{
			string result = "";
			string linkValue = BewebData.GetValue(new Sql("select pageid from page where pagecode=", sectionCode.SqlizeText(), " and historypageid is null"));
			if(linkValue!="")
			{
				result = GetRenderPage(linkValue) + "?page=" + ((encrypt)?Crypto.EncryptID(linkValue):linkValue);
				//'Response.Write("<font color=""red"" size=""1"" face=""sans-serif"">DEBUG: linkValue["&linkValue&"]</font><br>"+vbcrlf)
			}else
			{
				Logging.eout("ERROR: failed to find page code ["+sectionCode+"]");
				HttpContext.Current.Response.End();
			}

			if(result.StartsWith("admin/../"))
			{
				result = result.Replace("admin/../","");
			}
			return result;
		}


		/// <summary>
		/// get a url to a page given an ID (also does urlrewrite conversion and shortcuts)
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// use Nav.LinkPage
		/*
		public static string GetRenderPageURL(int? idn)
		{
			int id=(idn==null)?0:(int)idn;
			string result="";
			bool useNormal = true;
			string sql = "select title,PageIsALink,URLRewriteTitle from Page where historypageid is null and PageID=" + id;
			DataBlock rs = new DataBlock(BewebData.GetConnectionString());
			rs.OpenDB();
			rs.open(sql);
			string title = rs["title"];
			bool PageIsALink = VB.cbool(rs["PageIsALink"]);
			if(PageIsALink)
			{
				useNormal = false;
				int ?targetID = BewebData.GetInt("select URL from Page where PageID=" + id);
				if(targetID==null)
				{
					//throw new Exception(" ERROR: pageid ["+id+"] page is a link, but url is blank");useNormal
					useNormal = true;
				}else
				{
					result = GetRenderPageURL(targetID);
				}
				//result = BewebData.GetValue("select URL from Page where PageID=" + id);
			}
			rs.CloseDB();
			

			if(useNormal)
			{
				string rewriteURL = rs["URLRewriteTitle"];
				if (rewriteURL=="")
				{
					result = GetRenderPage(id) + "?page=" + Crypto.EncryptID(id) ;
					if(title!="")result+= "&section=" + HttpContext.Current.Server.UrlEncode(title);
				}else
				{
					result = "~/"+rewriteURL+".aspx";
				}
			}

			//result = result.Replace("../admin/../","");
			return result;
		}
		*/

		/// <summary>
		/// get a pipe sep list of parents of the given id
		/// </summary>
		/// <example>
		/// string ancPages = currCMA.GetAncestorPages(pageID);
		/// dout("ancPages["+ancPages+"]");
		/// string []anc = ancPages.Split(new Char[] {'|'}, 6);
		/// topid = VB.cint(anc[anc.Length-3]);  //anc[anc.Length] overflow, anc[anc.Length-1] blank, anc[anc.Length-2] home, anc[anc.Length-3] below home
		/// dout("topid["+topid+"]");
		/// </example>
		/// <param name="id">id to find parents of</param>
		/// <returns>pipe sep list of parents of the given id</returns>
		public static string GetAncestorPages(int? id)
		{
			string result = "";
			int ?parentID;
			if(id!=-1)
			{
				parentID = BewebData.GetInt("select ParentPageID from Page where PageID=" + id);
				if (parentID==0)
				{
					result = "";
				}else
				{
					result = parentID + "|" + GetAncestorPages(parentID);
				}
				//response.Write("GetAncestorPages["&GetAncestorPages&"]")
			}
			return result;
		}



		public string WriteHierarchySelector( 
			string tablename,	
			string primaryKeyColName,	
			string displayTextColName,
			string parentcolname, 
			string SortColName,	
			string fieldName,	
			string fieldValue 
			)
		{
			DataBlock	rs;
			string result="";
			string sql;
			sql = "select * from "+tablename+" where "+parentcolname+" is null or "+parentcolname+" =0 ";
			sql = sql + " order by "+SortColName;
			rs = (new DataBlock(BewebData.GetConnectionString())).execute(sql);
			if(rs.eof())
			{
				result += "<input type=hidden name=\""+parentcolname+"\" value='0'>";
				result +=	"None";
			}else
			{
				result +=	"<select size=1 name=\""+fieldName+"\">";
				result +=	"<option value=\"\">-- please select --</option>";
				if(primaryKeyColName!=fieldName)
				{
					//result += WriteOptionWithDescription("0", fieldValue, "--Top Level--");
				}
				result += WriteHierarchyOptions(tablename, primaryKeyColName, displayTextColName, parentcolname,SortColName, 0, 0,fieldName,	fieldValue);

				result +=	"</select>";
			}
			rs.close();
			return result;
		}

		private string WriteHierarchyOptions( 
			string tablename,	
			string primaryKeyColName,	
			string displayTextColName,	
			string parentcolname, 
			string SortColName,	
			int parentID,	
			int level,	
			string fieldName,	
			string fieldValue 
			)
		{
			DataBlock rs;
			string sql, optionValue, description,result="";
		
			sql = "select * from "+tablename+"";
			if (parentID!=0)
			{
				sql = sql + " where "+parentcolname+"=" + parentID;
			}	else
			{
				sql = sql + " where "+parentcolname+" is null or "+parentcolname+"=0";
			}
		
			sql = sql + " order by "+SortColName;
		
			rs = (new DataBlock(BewebData.GetConnectionString())).execute(sql);
			while( !rs.eof())
			{
				optionValue = rs[primaryKeyColName];
				description = rs[displayTextColName];

				for (int i = 1;i<level;i++)
				{
					description = "" + VB.chr(183) + VB.chr(183) + description;
				}
			
				result +=	"<option ";
				if (fieldValue+"" == ""+optionValue && optionValue+""!="0") { result +=	"selected ";}
				result +=	"value=\"" + optionValue + "\">" + description + "</option>";

				result += WriteHierarchyOptions(tablename, primaryKeyColName, displayTextColName, parentcolname,SortColName, rs.GetValueInt(0), level+1, fieldName,fieldValue);
				rs.movenext();
			}
			rs.close();
			return result;
		}

#if HasModels
		public static string GetNavUrl(Models.Page p1)
		{
			string url = p1.URLRewriteTitle;
			if(url.IsBlank())url = Nav.LinkPage(p1.PageID);//) Savvy.Hierarchy.GetRenderPageURL(p1.PageID);
			//if(!url.StartsWith("~"))
			//{
			//  if(!url.StartsWith("/"))url = "/"+url;
			//  url = "~"+url;
			//}
			if(url.Contains("?"))
			{
				string[] path=url.Split('?');
				return Web.ResolveUrl(path[0])+"?"+path[1];
			}
			return Web.ResolveUrl(url);
		}

		public static string GetNavTitle(Models.Page p1)
		{
			return p1.NavTitle.DefaultValue(p1.Title);
		}

		public static Models.PageList GetChildPages(string sectionCode)
		{
			return GetChildPages(Savvy.Hierarchy.GetRenderPageID(sectionCode));
		}

		public static Models.PageList GetChildPages(int parentPageID)
		{
			return Models.PageList.Load(new Sql("select * from page where showinnav=1 and historypageid is null and  parentpageid="+parentPageID));
		}
#endif
	}
}
