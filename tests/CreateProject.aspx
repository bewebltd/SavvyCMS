<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateProject.aspx.cs" Inherits="tests_create" %>
<%@ Import Namespace="Beweb"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Create Project</title>

<style>
	body{font-family:Lucida Sans, verdana;}
	.line td{border-top:1px solid black}
</style>
</head>
<body>
	<%Util.IncludejQuery(this.Page,Util.IncludeRenderMode.Inline); %>
	<form id="form" method="post">
		<div style="font-size:11px;">
			<%if(Util.IsBewebOffice||Util.ServerIsDev||Request["dev"]!=null){ %>
				<a href="CreateProject.aspx">reset</a> | <a href="../admin/default.aspx">admin</a> | <a href="../">home</a> | <a href="listfiles.aspx">list files</a><br />
			

				<h1>Create new project</h1>
				
			<p>
				This page will create or modify new or existing projects. Files are added or removed
				based on the manifests stored here:
				<b><%=Server.MapPath(Web.LocalPagePath)+"createproject" %></b>. Web config settings are
				modified to reflect the new project name, site.proj is modified to only include
				files that exist, or new files added. Any #defines are created or removed from the
				site.csproj as required by the manifest. Tables are not created in the database,
				but may be generated after new project is compiled (see below). Files that already
				exist are not overwritten. To create new manifests, BC compare listfiles.aspx in this project to listfiles.aspx in other projects, or existing manifests (see below)</p>

			<div style="font-size:12px; ">
					<script type="text/javascript">
						var defPath = '<%=DefPath.JsEncode() %>';
						
						function dopath(e, obj) {
							//alert(e.keyCode)
							//var curVal = $curObj.val();
							if ((/*(e.keyCode >= 48 && e.keyCode <= 57)  //zero to 9, or 
								||*/ e.keyCode == 190 //dot
								|| e.keyCode == 16 //shift 
								|| e.keyCode == 9 //tab 
								|| e.keyCode == 32 //del /backspace
								//|| e.keyCode == 46 || e.keyCode == 8 //del /backspace
								|| e.keyCode == 39 || e.keyCode == 37 //left/right
								)) {

								return false;
							} else {
								setpath();
								return true;
							}
						}
						
						function setpath() {
							var clientName = $('#clientname').val();
							var obj = document.getElementById('projname');
							var projName = obj.value.replace(' ', '');
							document.getElementById('path').value = defPath + clientName + "\\" + projName;
							obj.value = projName;
							$(".newproj-name").html(projName);							
						}


//						$(document).ready(function () {
//							
//						});

						function checkReq(radObj, depth) {
							//debugger 
							if(!depth)depth=1;
							if(depth>10)return ;/*Note: this may need to increase as more manifests are added*/
							if (radObj.value == "Create") {
								var lineIndex = $(radObj).parent().parent()[0].id.split('_')[1];
								var reqs = document.getElementById("req_" + lineIndex).innerHTML;
								var reqAr = reqs.split(',');
								//alert(reqs)
								for (var sc = 0; sc < reqAr.length; sc++) {															//walk the reqs
									//walk all radios
									$('input[value="Create"].manifestlist').each(function (index) {
										var radioName = this.name;
										if (radioName.toLowerCase().trim().indexOf(reqAr[sc].trim().toLowerCase()) > 0) {
											//document.getElementById("Manifest_11.news__Create").selected = true; // 	 Manifest_11.news__Create
											$(this).attr('checked', true);
											//var index = $(this).parent('td').id;
											checkReq(this,depth+1);
										}
									});
									
								}
							}
						}
					</script>
					
					<table>
						<tr>
							<td>
								Client Folder: <%=new Forms.Dropbox("clientname", Request["clientname"], true){onchange="setpath()"}.Add(ClientFolders)%><br/>
								Project: <input type="text" size="30" value="<%=Request["projname"].DefaultValue("newproj") %>" name="projname" id="projname" onkeyup="return dopath(event,this)"/> e.g. newproj<br />
								<input type="text" size="30" value="<%=Request["path"].DefaultValue(DefPath+"newproj") %>" name="path" id="path" style="width:300px;"/> e.g. <%=DefPath%>client\newproj<br />
							</td>
							<td width="100"></td>
							<td>
								Theme: <%=new Forms.Dropbox("theme", ThemeName, true).Add(ThemeFolders)%><br/>
								<%=new Forms.CheckboxField("showlog", Request["showlog"].ConvertToBool()){label = "Show Log"}%><br />
							</td>
						</tr>
					</table>
																																				
					<%=(Web.Request["create"]!=null)?"<a href=\"#\" onclick=\"document.getElementById('manifests').style.display='block'; return false;\">show hidden manifests</a>":""%>
					<table style="<%=(Web.Request["create"]!=null)?"display:none":""%>" id="manifests">
						<tr>
							<th>Module</th>
							<th>Action</th>
							<th>Description</th>
							<th>Tables</th>
							<th>#Defines</th>
							<th>Requires</th>
						</tr>
						<%
							int sc=0;
							foreach (var manifest in availableManifests){%>
							<tr class="line">
								<td style="font-size: 14px; width:200px; <%=(manifest.ErrorMsg.IsNotBlank()?"color:red;":"")%>"><a href="" onclick="$('.line_detail_<%=sc %>').toggle(); return false;" title="Click for more detail/less detail"><%=manifest.Name%></a> <%=(manifest.ErrorMsg.IsNotBlank()?"<span style=\"color:red\">!</span>":"")%></td>
								<td style="white-space:nowrap" id="rad_<%=sc %>">
									<%if(manifest.IsEnabled){ %>
									<%=new Forms.Radios("Manifest_" + manifest.Name, manifest.Action, true){onclick = "checkReq(this,"+sc+")", cssClass="manifestlist"}.Add("Create").Add("Leave").Add("Remove")%>
									<%}else{%>
										Disabled
									<%} %>
									</td>
								<td style="padding-left:10px;padding-top:5px;color:cadetblue; width:320px"><%:manifest.Description%></td>
								<td style="padding-left:10px;padding-top:5px;color:chocolate; width:200px"><%:manifest.Tables.Join(", ")%></td>
								<td style="padding-left:10px;padding-top:5px;color:brown"><%:manifest.Defines.Join(", ")%></td>
								<td style="padding-left:10px;padding-top:5px;color:brown" id="req_<%=sc%>"><%:manifest.Requires.Join(", ")%></td>
								
							</tr>
							<tr class="line_detail_<%=sc %>" style="display:none">
								<td style="width:200px;"></td> 
								<td style="font-size: 10px; ">
									Maintainer: <%=manifest.Maintainer %><br />
									<%=(manifest.SourceProject.IsNotBlank()?"<div style=\"\">Source: "+manifest.SourceProject+"</div>":"")%>
									<%=(manifest.ErrorMsg.IsNotBlank()?"<div style=\"color:red\">Error: "+manifest.ErrorMsg+"</div>":"")%>
									<small><%=FileSystem.GetFileContents(Web.MapPath( manifest.FileName), false).Replace("\n","<br>")%></small>
								</td>
							</tr>
							<%sc++; %>
				    <%}%>
					</table>		
					<%--<label><input type="checkbox" name="CreateDB" value="1" />	Create new database <span class="newproj-name" style="font-weight:bold"><%=Request["projname"].DefaultValue("newproj") %></span> on Thatcher if not already existing</label>--%>		
					<div style="clear: left;">
						<input type="submit" value="Go" name="create"/><br />
					</div>
				</div>
				<br/>

				<%WriteResults();%>
			<%} else
			{
				%>Beweb office only<%
			}%>
		</div>

	</form>
</body>
</html>
