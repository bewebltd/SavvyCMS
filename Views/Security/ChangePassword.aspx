<%@ Page Title="Change Password" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SecurityController.ChangePasswordViewModel>" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" Runat="Server">
	<script src="<%=Web.Root %>js/bewebcore/passwordmeter.js" language="javascript" type="text/javascript"></script>	
	<style type="text/css">
		.password_field {  }
		.password_meter { width: 100px; height: 19px; position: relative; display: block; float: left; border: solid 1px #dddddd; border-left: none; font-family: Arial; font-size: 12px; }
		.password_meter DIV { border-left: solid 1px #dddddd; width: 24px; height: 19px; float: left; }
		.password_meter .strength { display: block; position: absolute; top: 2px; left: 0; width: 100px; text-align: center; }
		.very_weak, .very_weak .first { background: red; }
		.weak, .weak .first, .weak .second { background: orange; }
		.acceptable, .acceptable .first, .acceptable .second, .acceptable .third { background: yellow; }
		.strong, .strong .first, .strong .second, .strong .third, .strong .fourth { background: #c1d72f; }
		#password_meter_panel{ float: left; margin: 8px 0 0 5px; }
		td.field{  width: 420px;}
		td.field input{ float: left;}
		td.field label{ float: left; margin: 5px 0 0 5px; }
	</style>
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("form");
			$("#ConfirmNewPassword").rules("add", { equalTo: "#NewPassword" });
			$("#NewPassword").keyup(function () { $('#password_meter_panel').show(500); CheckPassword(this); }).change(function () { CheckPassword(this); });
		});


		function CheckPassword(ele) 
		{
			var ps = new PasswordMeter().testPassword($(ele).val());
			var strength = ps.verdict;
			$('#strengthreason').attr("title", ps.reason);
			//$("#strengthreason").simpletooltip() 
			
			/*
			$('#strengthreason').attr("title",ps.reason.replace(new RegExp("\n","gm"), "<br>"))
			$("#strengthreason").mbTooltip({
				opacity : .90, //opacity
				wait:500, //before show
				ancor:"mouse", //"parent"
				cssClass:"default", // default = default
				timePerWord:70, //time to show in milliseconds per word
				hasArrow:false,
				color:"white",
				imgPath:"images/",
				shadowColor:"black",
				fade:500
				});
			*/
			
			var className = strength;
			if (className == "very weak") className = "very_weak";

			$('.strength', $("#password_meter").html(strength));
			// remove any classes and add the current one
			$("#password_meter").removeClass().addClass('password_meter').addClass(className);
		}
	</script>
	<form name="form" id="form" method="post" action="ChangePasswordSubmit" runat="server" >

	<h1>Change Password</h1>

	<%--<div style="font-weight:bold;"><%=Model.ChangePasswordMessage%></div>--%>
	<%=Html.InfoMessage() %>
	<%if (ViewData.ModelState.Count>0) { %>
		<div class="alert alert-error">
			<a class="close" data-dismiss="alert" href="#">×</a>
			<%=Html.ValidationMessage("Login") %>
		</div>
	<% } %>
	<table class="databox" style="margin-left: 0px;">
		<tr>
			<td class="dataheading" colspan="2">Change Your Password</td>
		</tr>
	  <tr>
			<td colspan="2">
				<%--<div style="width: 400px;"><%=Model.ChangePasswordMessage%></div>--%>
			</td>
	  </tr>
	  <%if(Model.ShowForm) {%>
			<% if (Model.ShowOldPassword) {%>
				<tr>
					<td class=label>Username / Email:</td>
					<td class="field"><%=Model.UserName%><!-- %=new Forms.EmailField("Username", Model.UserName, true) % --></td>
				</tr>
				<tr>
					<td class=label>Old password:</td>				
					<td class="field"><%=new Forms.PasswordField("OldPassword", Model.OldPassword, true)%></td>
				</tr>
			<%}%>
			<tr>
				<td  class=label valign="top">New password:</td>
				<td class="field">
				<%=new Forms.PasswordField("NewPassword", Model.NewPassword, true)%>
					<div style="display:none" id="password_meter_panel">
						<span class="password_meter" id="password_meter">
							<span class="strength"></span>
							<div class="first"></div><div class="second"></div><div class="third"></div><div class="fourth"></div>
						</span>
					</div>
				</td>
			</tr>
			<tr>
				<td  class=label style="white-space:nowrap;">Confirm new password:</td>
				<td class="field"><%=new Forms.PasswordField("ConfirmNewPassword", Model.ConfirmNewPassword, true)%></td>
			</tr>
			<tr>
				<td>
				<asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="please check the following" ShowMessageBox="true" ShowSummary="false" runat="server" />
				</td>
			</tr>
			<tr>
				<td></td>
				<td>
				<%=new Forms.HiddenField("p", Web.Request["p"]) %>
				<asp:Button ID="ChangePassword" Text="Change Password" UseSubmitBehavior="true" CausesValidation="true" runat="server" CssClass="securitybtn btnChangePwd" />
				</td>
			</tr>
		<%} else{%>
			<tr>
				<td></td>
				<td>			
			
				<input type="button" value="Click here to login" onclick="location='<%=Web.Root %>admin/'" />
				</td>
			</tr>
	  <%} %>
	</table>
</form>

	
</asp:Content>

