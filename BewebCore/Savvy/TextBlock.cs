using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Beweb
{

	/// <summary>
	/// Read a textblock from the database and slap it into memory for use on the page.
	/// </summary>
	public class TextBlock
	{
		protected string _Title = "";
		public string Title
		{
			get { return _Title; }
			set { _Title = value; }
		}
		protected int _ID = -1;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		protected string _URL = "";
		public string URL
		{
			get { return Fmt.Text(_URL); }
			set { _URL = value; }
		}
		protected string _URLCaption = "";
		public string URLCaption
		{
			get { return Fmt.Text(_URLCaption); }
			set { _URLCaption = value; }
		}

		protected string _BodyText = ""; public string BodyText { get { return _BodyText; } set { _BodyText = value; } }
		protected string _BodyTextHTML = ""; public string BodyTextHTML { get { return Fmt.HTMLText(_BodyTextHTML); } set { _BodyTextHTML = value; } }
		protected string _Picture = ""; public string Picture { get { return _Picture; } set { _Picture = value; } }
		protected string _PictureCaption = ""; public string PictureCaption { get { return _PictureCaption; } set { _PictureCaption = value; } }
		protected int _PictureHeight = 0; public int PictureHeight { get { return _PictureHeight; } set { _PictureHeight = value; } }
		protected int _PictureWidth = 0; public int PictureWidth { get { return _PictureWidth; } set { _PictureWidth = value; } }
		public string SubTitle;
		public string SectionCode;


		/// <summary>
		/// Loads an HTML textblock with the given textblockname from the database.
		/// if the textblock doenst exist, create it with the default value of '[textblockname] content goes here'.
		/// </summary>
		/// <param name="sectionName"></param>
		public TextBlock(string sectionName)
		{
			//init(sectionName,null,"auto-created section named ["+sectionName+"]");
			init(sectionName,null, Fmt.SplitTitleCase(sectionName)+" content goes here",false);
		}

		/// Loads a plain text or HTML textblock with the given code from the database.
		/// If the textblock doenst exist, create it with the default value of '[textblockname] content goes here'.
		/// If isPlainText=false then it is HTML.
		public TextBlock(string sectionName, bool isPlainText)
		{
			//init(sectionName,null,"auto-created section named ["+sectionName+"]");
			init(sectionName,null, Fmt.SplitTitleCase(sectionName)+" "+(isPlainText?"text":"content")+" goes here",isPlainText);
		}

		/// <summary>
		/// if the textblock doenst exist, create it with the default value supplied
		/// </summary>
		/// <param name="sectionName"></param>
		/// <param name="defaultValue"></param>
		public TextBlock(string sectionName, string defaultValue)
		{
			init(sectionName,null,defaultValue,false);
		}
		public TextBlock(string sectionName, string defaultValue, bool isPlainText)
		{
			init(sectionName,null,defaultValue,isPlainText);
		}

		public TextBlock(string sectionName,string defaultTitle, string defaultBodyText)
		{
			init(sectionName,defaultTitle,defaultBodyText,false);
		}
		public TextBlock(string sectionName,string defaultTitle, string defaultBodyText, bool isPlainText)
		{
			init(sectionName,defaultTitle,defaultBodyText,isPlainText);
		}

		/// <summary>
		/// if the textblock doesn't exist, create it with the default value supplied
		/// </summary>
		/// <param name="sectionName"></param>
		/// <param name="defaultBodyText"></param>
		/// <param name="isPlainText"></param>
		protected void init(string sectionName, string defaultTitle, string defaultBodyText, bool isPlainText)
		{
			this.SectionCode = sectionName;
			DataBlock db = new DataBlock();
			db.OpenDB();

			if(Web.Request["droptextblocks"]!=null&&Util.IsBewebOffice){db.execute("delete from TextBlock where SectionCode='" + Fmt.SqlString(sectionName) + "'");}

			DataBlock rs = db.execute("select * from TextBlock where SectionCode='" + Fmt.SqlString(sectionName) + "'");
			if (rs.eof())
			{
				//autocreate empty textblock
				if(defaultTitle!=null)
				{
					db.execute("insert into TextBlock(sectioncode,title,bodytexthtml,isbodyplaintext,isurlavailable,istitleavailable,ispictureavailable)values('" + Fmt.SqlString(sectionName) + "','" + Fmt.SqlString(defaultTitle) + "','" + Fmt.SqlString(defaultBodyText) + "',"+Fmt.SqlBoolean(isPlainText)+",0,1,0)");
				}else
				{
					db.execute("insert into TextBlock(sectioncode,bodytexthtml,isbodyplaintext,isurlavailable,istitleavailable,ispictureavailable)values('" + Fmt.SqlString(sectionName) + "','" + Fmt.SqlString(defaultBodyText) + "',"+Fmt.SqlBoolean(isPlainText)+",0,0,0)");
				}
				rs.close();//close previous rs

				//reopen newly created block
				rs = db.open("select * from TextBlock where SectionCode='" + Fmt.SqlString(sectionName) + "'");
			} 

			_ID = rs.GetValueInt("TextBlockID");
			RawBody = rs.GetValue("BodyTextHTML");
			_BodyText = Fmt.FmtText(RawBody);
			// todo: add fmttext back (abstract?)
			_BodyTextHTML = RawBody;//currCMA.FmtHtmlText();
			_Title = rs.GetValue("Title");
			if (!rs.FieldExists("SubTitle")) {
				new Sql("alter table textblock add SubTitle nvarchar(250);");
			} else {
				SubTitle = rs.GetValue("SubTitle");
			}
			if (rs.GetValueIsTrue("IsPictureAvailable"))
			{
				if (rs.GetValue("Picture").IsNotBlank()) {
					_Picture = Util.GetAttachmentVPath() + rs.GetValue("Picture");
					if (rs.FieldExists("PictureCaption")) {
						_PictureCaption = rs.GetValue("PictureCaption");
					}
					if (rs.FieldExists("PictureWidth")) {
						_PictureWidth = rs.GetValue("PictureWidth").ToInt(0);
						_PictureHeight = rs.GetValue("PictureHeight").ToInt(0);
					}
				}
			}
			_URL = rs.GetValue("URL");
			_URLCaption = rs.GetValue("URLCaption");
			//}else
			//{
			//	//bugger
			//	//throw new Exception("not found sectionName[" + sectionName + "]count[" + rs.RecordCount+ "]");
			//	rs.execute("insert into textblock(sectioncode)values()")
			//}
			rs.close();
			db.CloseDB();
		}

		public string RawBody
		{
			get ;
			set ;
		}


		public string DrawPicture()
		{
			return "<img src=\""+Picture+"\" alt=\""+PictureCaption+"\" width=\""+PictureWidth+"\" align=\"left\">";
		}
	}
}