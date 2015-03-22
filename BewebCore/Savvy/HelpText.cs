using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Beweb
{

	/// <summary>
	/// Read a HelpText from the database and slap it into memory for use on the page.
	/// </summary>
	public class HelpText
	{
		protected string _Title = "";
		public string Title{
			get { return _Title; }
			set { _Title = value; }
		}

		protected int _ID = -1;
		public int ID {
			get { return _ID; }
			set { _ID = value; }
		}

		protected string _BodyTextHTML = "";
		public string BodyTextHTML {
			get { return Fmt.HTMLText(_BodyTextHTML); }
			set { _BodyTextHTML = value; }
		}

		public string HelpTextCode;

		/// <summary>
		/// Loads an HTML HelpText with the given HelpTextname from the database.
		/// if the HelpText doenst exist, create it with the default value of 'There is no help text written for this as yet.'.
		/// </summary>
		/// <param name="helpTextCode"></param>
		public HelpText(string helpTextCode)
		{
			init(helpTextCode, null, "There is no help text written for " + helpTextCode + " yet.");
		}

		/// <summary>
		/// if the HelpText doenst exist, create it with the default value supplied
		/// </summary>
		/// <param name="helpTextCode"></param>
		/// <param name="defaultValue"></param>
		public HelpText(string helpTextCode, string defaultValue)
		{
			init(helpTextCode,null,defaultValue);
		}
		
		/// <summary>
		/// if the HelpText doenst exist, create it with the default value supplied
		/// </summary>
		/// <param name="helpTextCode"></param>
		/// <param name="defaultTitle"></param>
		/// <param name="defaultValue"></param>
		public HelpText(string helpTextCode,string defaultTitle, string defaultValue)
		{
			init(helpTextCode,defaultTitle,defaultValue);
		}

		/// <summary>
		/// if the HelpText doesn't exist, create it with the default value supplied
		/// </summary>
		/// <param name="helpTextCode"></param>
		/// <param name="defaultTitle"></param>
		/// <param name="defaultBodyText"></param>
		protected void init(string helpTextCode, string defaultTitle, string defaultBodyText)
		{
			this.HelpTextCode = helpTextCode;
			DataBlock db = new DataBlock();
			db.OpenDB();

			if(Web.Request["dropHelpTexts"]!=null&&Util.IsBewebOffice){db.execute("delete from HelpText where HelpTextCode='" + Fmt.SqlString(helpTextCode) + "'");}

			DataBlock rs = db.execute("select * from HelpText where HelpTextCode='" + Fmt.SqlString(helpTextCode) + "'");
			if (rs.eof()) {
				defaultTitle = (defaultTitle.IsBlank()) ? helpTextCode +" Help" : defaultTitle;
				//autocreate empty HelpText
				db.execute("insert into HelpText(HelpTextCode,title,bodytexthtml,dateadded)values('" + Fmt.SqlString(helpTextCode) + "','" + Fmt.SqlString(defaultTitle) + "','" + Fmt.SqlString(defaultBodyText) + "', " + Fmt.SqlDate(DateTime.Now) + ")");
				rs.close();//close previous rs
				//reopen newly created block
				rs = db.open("select * from HelpText where HelpTextCode='" + Fmt.SqlString(helpTextCode) + "'");
			} 

			_ID = rs.GetValueInt("HelpTextID");
			_BodyTextHTML = rs.GetValue("BodyTextHTML");
			_Title = rs.GetValue("Title");
			rs.close();
			db.CloseDB();
		}

	}
}