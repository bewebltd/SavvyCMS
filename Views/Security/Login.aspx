<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SecurityController.LoginFormViewData>" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %> 
<%@ Import Namespace="Models" %> 
<%@ Import Namespace="Site.SiteCustom" %> 
<%@ Import Namespace="Site.Controllers" %> 

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
<script type="text/javascript">
	$(document).ready(function () { $("#Username")[0].select(); })
</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<%=Html.InfoMessage() %>
	<%if (ViewData.ModelState.Count>0) { %>
		<div class="alert alert-error">
			<a class="close" data-dismiss="alert" href="#">×</a>
			<%=Html.ValidationMessage("Login") %>
		</div>
	<% } %>

	<%if (Model.Message.IsNotBlank()) { %>
		<p><%=Model.Message%></p>
	<% } %>
	<form name="loginform" method="post" action="LoginSubmit" class="loginform">
		<table class="svyLogin">
			<tr>
				<td class="dataheading" colspan="2">Login</td>
			</tr>
			<tr>
				<td width="33%" class="right">User name:</td>
				<td><%=new Forms.TextField("Username", Model.Username, true){maxlength = "50", cssClass = "loginfield"}%></td>
			</tr>
			<tr>
				<td class="right">Password:</td>
				<td><%=new Forms.PasswordField("PCode", Model.PCode, true){maxlength = "50", onfocus="this.select()", cssClass = "loginfield"}%></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td><%=new Forms.CheckboxField("RememberPwd", Model.RememberPwd){label= "Remember my password"}%></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>
					<input type="submit" value="Login" />
				</td>
			</tr>
		</table>
		<%=new Forms.HiddenField("ReturnURL", Web.Request["ReturnUrl"])%>	
		<%=Html.AntiForgeryToken() %>

	</form>
	<form name="forgotpasswordform" method="post" action="ForgotPassword" class="forgotpasswordform">
		<table class="svyLogin">
				<tr>
					<td class="dataheading" colspan="2">Forgotten Your Password?</td>
				</tr>
			<tr>
				<td colspan="2">
					<%if (new Security().IsPasswordReminderPossible){%>
						If you do not know your password, simply enter your email address here and click "Send Password". Your password will be emailed to you.
					<%}else{ %>
						If you do not know your password, simply enter your email address here and click "Reset Password". A password reset link will be emailed to you.
					<%} %>
				</td>
			</tr>
			<tr>
				<td  width="33%" class="right">Email&nbsp;Address:</td>
				<td><%=new Forms.TextField("ForgottenPasswordEmailAddress", Model.ForgottenPasswordEmailAddress, true){maxlength = "80",onfocus="this.select()", cssClass = "loginfield"}%></td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td>
					<input type="submit" value="<%if (new Security().IsPasswordReminderPossible) {%>Send<% }else{ %>Reset<%} %> Password" />
				</td>
			</tr>
		</table>
	</form>
</asp:Content>
