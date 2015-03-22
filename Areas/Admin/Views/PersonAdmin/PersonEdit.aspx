<%@ Page Title="Edit Person" Inherits="System.Web.Mvc.ViewPage<Models.Person>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate-vsdoc.js"></script><%}   // provides intellisense %>
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form");
			RoleCheck();
			$('#pwValue').on('keyup',function (){
				CheckStrength()
			})
		});

		function RoleCheck() {
			//var role = V$("Role");
			// here you could possibly show/hide some fields depending on the role
			// if (role.indexOf("student")!=-1) $(".course").show()

		}
	
		function ChangeTemplate(radioObj) {
			if ($('#Template__Accept')[0].checked) {
				$('#EmailCopy').val($('#acceptText').html());
			} else if ($('#Template__Decline')[0].checked) {
				$('#EmailCopy').val($('#declineText').html());
			} else {
				alert('invalid template selection');
			}
		}


		function SendEmail(uploadUrl) {
			alert('not available');
		}
		
		function ParseEmail() {
			var emailText = V$("Email");
			if (emailText.indexOf("<") != -1) {
				var name = df_Trim(emailText.substr(0, emailText.indexOf("<")));
				var email = df_Trim(emailText.substr(emailText.indexOf("<")));
				email = df_Trim(email.replace("<", "").replace(">", ""));
				V$("Email", email);
				if (name.indexOf(" ") != -1) {
					V$("FirstName", name.split(" ")[0]);
					V$("LastName", name.split(" ")[1]);
				} else {
					V$("FirstName", name);
				}
			}
			CheckEmailValid(V$("Email"));
		}

		function GenPass() {
			var qs = "";
			var url = websiteBaseUrl + "Admin/PersonAdmin/GeneratePassword";
			$.ajax({
				type: "POST",
				url: url,
				data: qs,
				success: function (msg) {
					$('#pwValue').val(msg);
				},
				error: function (msg) {
					alert("call failed 2: " + msg);
					//prompt('copy this',url+'?'+qs)
				}
			});
		}
		
		
		function CheckStrength() {
			var qs = "pw="+$('#pwValue').val();
			var url = websiteBaseUrl + "Admin/PersonAdmin/CheckStrengthPassword";
			$.ajax({
				type: "POST",
				url: url,
				data: qs,
				success: function (msg) {
					$('#pwstrength').html('<b>'+msg.verdict+'</b>').css('color',msg.color)
						.append('<br>What you did well:<br>'+msg.points.replace(new RegExp("\n","gm"), "<br>"))
						.append('<hr><br>'+msg.suggest.replace(new RegExp("\n","gm"), "<br>"));
				},
				error: function (msg) {
					alert("call failed 3: " + msg);
					//prompt('copy this',url+'?'+qs)
				}
			});
		}

		
		
		
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var record = Model; %>

	<%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		
		<!-- fake fields are a workaround for chrome autofill getting the wrong fields -->
		<input style="display:none" type="text" name="fakeusernameremembered"/>
		<input style="display:none" type="password" name="fakepasswordremembered"/>
		
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Person</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Email:</td>
				<td class="field">
					<%=new Forms.TextField(record.Fields.Email, true){onchange="ParseEmail()"} %>
					<span id="emailvalidserver"></span>
				</td>
			</tr>  
<!--
			<tr>
				<td class="label">Enter Email Again:</td>
					<td class="field"><%=new Forms.EmailField("emailConfirmed", Request["emailConfirmed"]??record.Email, true){maxlength = "50"} %></td>
			</tr>
-->			
			<tr>
				<td class="label">Password:</td>
				<td class="field">
					<%//if(Util.IsDevAccess() || Security.IsSuperAdminAccess||record.IsNewRecord || Security.LoggedInUserID==record.ID)%>
					<% if(record.IsNewRecord) {%>
						<%=new Forms.TextField("pwValue",record.pwValue, true){style="width:430px;", maxlength="250"} %><input type="button" value="Generate" onclick="return GenPass();">
					<%} else if(Security.LoggedInUserID==record.ID) {%>
						You can reset your own password.
						
						<a href="<%=Web.Root%>Security/ChangePassword" onclick="$('.newPW').show();$(this).hide();$('#pwstars').hide();return false;">Reset</a>
							<div class="hide newPW"><%=new Forms.TextField("pwValue","", true){style="width:430px;", maxlength="250", placeholder = "Please enter a new password"} %><input type="button" value="Generate" onclick="return GenPass();"></div>
					<%} else if (Util.IsDevAccess()){%>
						<span id="pwstars"> ***********************</span> 
						<%if(Web.Request["dev"]!=null){ %>                 
							<span style="display:none" id="devpw">
								<%=new Forms.TextField("pwValue",record.pwValue, true){style="width:430px;", maxlength="250", placeholder = "Please enter a new password"} %>
								<input type="button" value="Generate" onclick="return GenPass();">
							</span> 
							<a href="#" onclick="$(this).hide();$('#devpw').show();$('#pwstars').hide();return false;">.</a>
						<%} else{%> 
							<a href="<%=Web.Root%>Security/ChangePassword" onclick="$('.newPW').show();$(this).hide();$('#pwstars').hide();return false;">Reset</a>
							<div class="hide newPW"><%=new Forms.TextField("pwValue","", true){style="width:430px;", maxlength="250", placeholder = "Please enter a new password"} %><input type="button" value="Generate" onclick="return GenPass();"></div>
						<%}%>
						
					<%} else{%>
						<%//=Security.DecryptPassword( record.Password) %>
						Password not available
					<%}%>
					
					<div id="pwstrength"></div>
				</td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.FirstName, true) %></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.LastName, true) %></td>
			</tr>
			
			<%if (Security.IsSuperAdminAccess) {%>
				<tr>
					<td class="label">Role:<br/>
					
					<small>currently: <%=record.Role %></small>
					</td>
							<td class="field">
							<%=new Forms.Dropbox(record.Fields.Role, true, false){onchange = "RoleCheck()"}.Add(SecurityRoles.GetRoleDropDownOptions()) %>
					</td>
				</tr>
				<%--<tr>
					<td class="label">Role:</td>
							<td class="field">
							<%
							var roleDropbox=new Forms.Checkboxes("Role", record.Role.Split(','),"");
							//var roleDropbox=new Forms.Checkboxes("Role", record.Role.Split(','),"").Add(Roles.GetAllRoles());
							roleDropbox.Add(Savvy.SiteCustom.SecurityRoles.Roles.GetAllRoles());

							%>
							<%=roleDropbox.GetHtml() %>
					</td>
				</tr>--%>
			<%} else {%>
				<tr>
					<td class="label">Role<%=(record.Role.Contains(",")?"s":"")%>:</td>
					<td class="field"><%=record.Role %></td>
				</tr>
			<%} %>

			<%if(Security.PasswordMode!=Security.PasswordModes.Level4HashedOneWay ){                       //true|| Security.IsInRole("special**user") 
				var tb  = TextBlockCache.GetPlain("Admin Password Email",defaultTitle:"Your password for " + Util.GetSiteName(),defaultTextHtml:@"Dear [firstname],

Your account has been activated, and you may now login to the web site.

Username : [username]
Password : [password]
Admin address: [baseurl]admin/

Thanks.

"); 
				var body = tb.BodyTextHtml;

				
			%>
			<tr>
				<td class="label section"><strong>Correspond</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Email this person:</td>
				<td class="field"><input type="button" value="Click to send email..." onclick="$(this).parent().parent().hide();$('#email').show();" /><br/>&nbsp;</td>
			</tr>
			<tr id="email" class="hide">
				<td class="label">Compose Email:</td>
				<td class="field">
					Subject:<br />
					<%= new Forms.TextField("EmailSubject",tb.Title,true){maxlength="50"}%><br />
					Body:<br />
					<%= new Forms.TextArea("EmailCopy",body,true) {rows=14}%><br />		 <br/>
					<%=Html.SaveButton("Save & Send Email", "sendemail") %><br/>
					<small>
						The fields in square brackets will automatically be replaced with the correct information eg. [password] will be replaced with the real password <br />
						Edit the default template for this email in Text Block admin.
					</small>
				</td>
			</tr>
			<%} %>
			<%if(record.FieldExists("StartDate")){ %>
				<tr>
					<td class="label section"><strong>Stint</strong></td>
					<td class="section"></td>
				</tr>
				<tr>
					<td class="label">Start Date:</td>
					<td class="field"><%=new Forms.DateField("StartDate",record["StartDate"].ConvertToDate(), true) %></td>
				</tr>
			<%} %>
			<%if(record.FieldExists("FinishDate")){ %>
				<tr>
					<td class="label">Finish Date:</td>
					<td class="field"><%=new Forms.DateField("FinishDate",record["FinishDate"].ConvertToDate(), false){onchange = "$('#IsActive_False').click();"} %></td>
				</tr>
			<%} %>
			<tr>
				<td class="label section"><strong>Login Information</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Active:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsActive, true) %></td>
			</tr>
			<tr>
				<td class="label">Last Login Date:</td>
				<td class="field"><%=Fmt.DateTime(record.LastLoginDate)%></td>
			</tr>
			<tr>
				<td class="label">Login Count:</td>
				<td class="field"><%=record.LoginCount%></td>
			</tr>
			<tr>
				<td class="label">Failed Login Count:</td>
				<td class="field"><%=record.FailedLoginCount%></td>
			</tr>
			<tr>
				<td class="label">Last IP Address:</td>
				<td class="field"><%=record.LastIpAddress%></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>
			<tr>
				<td class="label">Last Modified:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateModified)%> (<%=Fmt.TimeDiffText(record.DateModified) %>)</td>
			</tr>
			<%//= Savvy.Site.ShowModificationLog(record)%>

			<tr>
				<td colspan="2" class="footer">
				<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Person Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

