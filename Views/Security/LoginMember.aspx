<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SecurityController.LoginFormViewData>" MasterPageFile="~/site.master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
<script>
//	$(document).onready(function () { $("Username")[0].select() })
</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<%=Html.InfoMessage() %>
	<%=Html.ValidationMessage("Login") %>
	<form name="loginform" method="post" action="LoginSubmit">
	<table class="svyLogin">
		<tr>
			<td class="dataheading" colspan=2>Login</td>
		</tr>
		<tr>
			<td width="33%" class="right">User name:</td>
			<td><%=new Forms.TextField("Username", Model.Username, true){maxlength = "50", cssClass = "loginfield"}%></td>
		</tr>
		<tr>
			<td class="right">Password:</td>
			<td><%=new Forms.PasswordField("PCode", Model.PCode, true){onfocus="this.select()", cssClass = "loginfield"}%></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td><%=new Forms.CheckboxField("RememberPwd", Model.RememberPwd){label= "Remember my password"}%></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>
				<input type=submit value="Login" />
			</td>
		</tr>
	</table>
	</form>
	<br />
	<br />
	<form name="forgotpasswordform" method="post" action="ForgotPassword">
		<table class="svyLogin">
				<tr>
					<td class="dataheading" colspan="2">Forgotten Your Password?</td>
				</tr>
			<tr>
				<td colspan="2">
					If you do not know your password, simply enter your email address here and click "Reset Password". A password reset link will be emailed to you.
				</td>
			</tr>
			<tr>
				<td  width="33%" class="right">Email&nbsp;Address:</td>
				<td><%=new Forms.TextField("ForgottenPasswordEmailAddress", Model.ForgottenPasswordEmailAddress, true){maxlength = "50",onfocus="this.select()", cssClass = "loginfield"}%></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>
					<input type="submit" value="Reset Password" />
				</td>
			</tr>
		</table>
	</form>
</asp:Content>
