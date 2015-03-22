<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="tests_Default" %>
<%@ Import Namespace="Beweb"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Tests/Gen</title>
</head>
<body style="font-family:Lucida Sans, verdana;">
<form id="form1" runat="server">
<div style="font-size:11px;">
	<%if(Util.IsBewebOffice||Util.ServerIsDev||Request["dev"]=="somespecialthingjeremycantremember"){ %>
		<div style="float:left;width:230px; font-size:14px; line-height:180%;"><%
			if (Util.ServerIsDev||Request["dev"]!=null){ 
				%><a href="<%=Web.PageFileName%>?mode=GenModels">Gen Models</a><br /><%
			} 
			if(!Util.ServerIsDev){ 
				%><a href="<%=Web.PageFileName%>?mode=UpdateDLLs">Update DLLs</a><br /><%
			} 
			%>
			<a href="<%=Web.PageFileName%>?mode=AddDbFields">Add Fields to Database</a><br />
			<a href="<%=Web.PageFileName%>?mode=MinifyJsCss">Gen JS and CSS min files</a><br />


			<%if(Request.Path.ToLower().Contains("codelib")){ %>
				<a href="createproject.aspx">Create Project / Add Features</a><br />
			<%} %>
			
			<a href="<%=Web.PageFileName%>?mode=ViewCache">View Cache</a><br />
			<a href="<%=Web.PageFileName%>?mode=ClearCache">Clear Cache</a><br />
			
			<%--<a href="<%=Web.PageFileName%>?mode=GenDatabaseScript">Gen Database Script</a><br />--%>
			<br />
			 
		
			<!--
			<a href="http://adam/ccnet/ViewFarmReport.aspx">Cruise Control</a><br />
			<br />
			-->
			<a href="../admin/default.aspx">admin</a><br />
			<a href="../admin/tools/createadmin.aspx">admin/createadmin</a><br />
			<a href="../">home</a><br />
			Serveris[<%=Util.ServerIs()%>] 
			<br />
			<br />
			<a href="<%=Web.PageFileName%>?class=all">Run All Tests</a><br />
			<%foreach (var testClass in this.testClasses){%>
				<a href="<%=Web.PageFileName%>?class=<%=testClass.FullName%>"><%=testClass.Name %></a><br />
			<%}%>
			
		</div>
		<%if(Web.Request["mode"]!=null || Web.Request["class"]!=null){ %>
			<%WriteResults();%>
		<%}else{ %>		 
      <h2>You are on <%=Util.ServerIs()%></h2>
      
		
			DB Copy utils :	<br/><br/>
			<small>
			DEV	 database: <%=BewebData.GetConnectionString("ConnectionStringDEV").LeftUntil("Password")%> <br>
			STG	 database: <%=BewebData.GetConnectionString("ConnectionStringSTG").LeftUntil("Password")%> <br>
			LVE  database: <%=BewebData.GetConnectionString("ConnectionStringLVE").LeftUntil("Password")%> <br>
			<br/><br/>							
			
			<%--dev options--%>
			<%if (Util.ServerIsDev) { %>		 

				<p><strong>Import Options:</strong></p>
				<%if (BewebData.GetConnectionString("ConnectionStringDEV")==BewebData.GetConnectionString("ConnectionStringLVE")) { %>
					Import All Data from LVE to DEV: Sorry, DEV is connected to live. Cannot copy data.		<br />	 
				<%}else if (BewebData.GetConnectionString("ConnectionStringSTG")==BewebData.GetConnectionString("ConnectionStringDEV")) { %>
					Import All Data from LVE to DEV: Sorry, DEV is connected to STG. Cannot copy data.		<br />	 
				<%}else{ %>						
					<%if (BewebData.GetConnectionString("ConnectionStringSTG")==BewebData.GetConnectionString("ConnectionStringLVE")) { %>
						<h2><span style="color:red">Warning! STG==LVE database is same instance</span></h2><br />	 
					<%}%>

					<a href="<%=Web.PageFileName%>?mode=ImportFromLive" onclick="return confirm('are you sure?')">Import All Data from LVE</a><br />	 
				<%}%>
				
				<%if (BewebData.GetConnectionString("ConnectionStringDEV")==BewebData.GetConnectionString("ConnectionStringSTG")) { %>
					Import All Data from STG to DEV: Sorry, no STG database. Cannot copy data.						 <br />	 
				<%}else{ %>
					<%if (BewebData.GetConnectionString("ConnectionStringSTG")==BewebData.GetConnectionString("ConnectionStringLVE")) { %>
						<h2><span style="color:red">Warning! STG==LVE database is same instance</span></h2><br />	 
					<%}%>

					<a href="<%=Web.PageFileName%>?mode=ImportFromStaging" onclick="return confirm('are you sure?')">Import All Data from STG to DEV</a><br />
				<%}%>
				
				<p><strong>Export Options:</strong></p>
				<%if (BewebData.GetConnectionString("ConnectionStringDEV")==BewebData.GetConnectionString("ConnectionStringSTG")) { %>
					Export data to STG from DEV: Sorry, DEV==STG database. Cannot copy data.		<br />	 
				<%}else if (BewebData.GetConnectionString("ConnectionStringSTG")==BewebData.GetConnectionString("ConnectionStringLVE")) { %>
					Export data to STG from DEV: Sorry, STG==LVE database. Cannot copy data.		<br />	 
				<%}else{ %>
					<a href="<%=Web.PageFileName%>?mode=ExportToStaging" onclick="return confirm('are you sure?')">Export data to STG from DEV</a><br />
				<%}%>

				<%--<a href="<%=Web.PageFileName%>?mode=LegacyMigration">Legacy Migration</a><br />--%>
			<%}%>			 
			
			
			
			<%--staging options--%>
			<%if (Util.ServerIsStaging || BewebData.GetConnectionString("ConnectionStringDEV")==BewebData.GetConnectionString("ConnectionStringSTG")) { %>
				<%if (BewebData.GetConnectionString("ConnectionStringSTG")==BewebData.GetConnectionString("ConnectionStringLVE")) { %>
					<span style="color:red">Import All Data from LVE: Sorry, STG database is same as LIVE database. Cannot copy data.</span>	<br />	 
				<%}else{ %>			 
					<%if (BewebData.GetConnectionString("ConnectionStringDEV")==BewebData.GetConnectionString("ConnectionStringSTG")) { %>
						<h2><span style="color:red">Warning! DEV==STG database is same instance</span></h2><br />	 
					<%}%>																																																				 
					<%if(Util.ServerIsStaging){ %>
						<a href="<%=Web.PageFileName%>?mode=ImportFromLive" onclick="return confirm('are you sure?')">Import All Data LVE to STG</a><br />
					<%} %>
				<%}%>
			<%}%>							
			
			<%--live options--%>
			<%if (Util.ServerIsLive) { %>
				<h2><span style="color:red">Warning! no data copy options on LIVE, log into DEV or STG</span></h2>		 <br />	 
			<%}%>			 
			</small>
		<%} %>
	<%} else { %>
		Beweb office only
	<%}%>
</div>

</form>
</body>
</html>
