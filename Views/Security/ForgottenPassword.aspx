<%@ Page Title="Forgotten Password" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SecurityController.ForgotPasswordViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Site.SiteCustom" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript" src="<%=Web.Root %>js/BewebCore/Savvy.validate.js"></script>
	<script type="text/javascript" src="<%=Web.Root %>js/BewebCore/beweb-cma.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("forgotpasswordform");
		});
		function CheckForm(form) {
			var msg = '';
			var result = CheckBasicFormValidation(form);
			if (result) result = SavvyBeforeFormSubmit(form);
			return result;
		}
	</script>
</asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">

	<div class="loginMember">

		<form name="forgotpasswordform" method="post" action="<%=Web.Root %>Security/ForgotPassword" id="forgotpasswordform"  onsubmit="return CheckForm(this)">
			
			<div class="validation-feedback" style="display: none;"><p style="font-size: 14px">Password is not in the correct format - please try again.
			<%--
			<ul>
				<li id="validation_Email" style="display: none;"></li>
			</ul>
			--%>
			</div>
			<h2>Forgotten your password?</h2>
			<div class="addressMsg"><%=TextBlockCache.Get("Forgotten Password Statement","Don’t worry, it happens to the best of us. Please enter your email below and we will send you your password.").BodyTextHTML %></div>

			<ul>
				<li>
					<label>Email:*</label>
					<%=new Forms.EmailField("ForgottenPasswordEmailAddress", true){maxlength = "100",onfocus="this.select()", cssClass = "loginfield email"}%>
				</li>
				<li>
					<hr />
					<input type="submit" value="Email Password" class="sendPasswordBtn gfxBtn" />
				</li>
			</ul>
			
		</form>

		<div class="clear"></div>
<%--
		<div class="havingProblem">
			<h2>Having Problems?</h2>
			<input type="button" value="Having Problems? Check out our FAQ's" onclick="window.location.href='<%=Web.Root%>FAQ'" class="checkOutFaqBtn gfxBtn" /> 
		</div>--%>

	</div>
</asp:Content>
