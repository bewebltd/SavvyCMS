<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetAutoComplete.aspx.cs" Inherits="services_GetDropboxInfo" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%
	// GetAutoComplete.aspx?data=encrypted[table:col]&q=searchtext
	string TableName = Request["data"];
	if(TableName==null)
	{
		throw new Exception("data missing");
	}
	string[] data;
	data = Crypto.Decrypt(Request["data"]).Split('|');
	string table=data[0];
	string col=data[1];
	string idcol=data[2];
	if(idcol.IsBlank())
	{
		idcol=col;
	}
	string select = idcol;
	if(idcol!=col)select+=","+col;
	string prefix="";
	if(Request["max"]!=null)prefix="top "+Fmt.SqlNumber(Request["max"]+"")+" ";

	string orderby = col;
	if(idcol!=col)orderby+=","+idcol;
	
	var sql = new Sql("SELECT "+prefix+"" +select+"  FROM ",table.SqlizeName(),"where 1=1");
	if(Request["term"].IsNotBlank())sql.Add("and ",col.SqlizeName(),"like",Request["term"].SqlizeLike());

	if(col!=null)
	{
		sql.Add(" group BY  "+orderby+" ");
		//sql.Add(" ORDER BY ",col.SqlizeName()," asc, ",idcol.SqlizeName(),"");
		//sql.Add(" group BY  ",idcol.SqlizeName(),",",col.SqlizeName()," ");
		//sql.Add(" ORDER BY ",col.SqlizeName()," asc, ",idcol.SqlizeName(),"");
	}
	DataSet ds = BewebData.GetDataSet(sql.ToString());
	int scan=0;
	Web.Write("[");
	var list = "";//new DelimitedString();
	int idx=0;
	foreach (DataRow dr in ds.Tables[0].Rows)
	{
		var descr = (dr[col]+"");
		if(descr.IsNotBlank()) {
			//Console.WriteLine(dr[0].ToString());
			var dataToShow = descr.HtmlEncode();
			dataToShow = dataToShow.Replace(", "," / ");	//remove , as this is a delimiter for autocomplete
			dataToShow = dataToShow.Replace(" ,"," / ");	//remove , as this is a delimiter for autocomplete
			dataToShow = dataToShow.Replace(",","/");			//remove , as this is a delimiter for autocomplete

			//if(idcol!=col) 
			dataToShow="{\"label\":\""+dataToShow+"\",\"value\":\""+dr[idcol]+"\"}";
			if(idx>0)list+=",";
			list+=""+dataToShow+""; //dr[idcol]
			idx++;
		}
 	scan++;
	}
	Web.Write(list);
	Web.Write("]");
%>