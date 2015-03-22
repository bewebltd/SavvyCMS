<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.CompetitionController.ViewModel>" MasterPageFile="~/site.master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent">
<style>
	p { margin: 0; }
	.span12 { width: 100%; }
	.ui-autocomplete { max-height: 400px; overflow-y: auto; }	
	#ThankYou { max-width: 310px; margin-top: 10px; }
	.form-horizontal .control-label { width: 80px; text-align: left; padding-left: 5px; }
	.form-horizontal .controls { margin-left: 90px; position: relative; }
	.competition-image { margin-bottom: 10px; }
	.competition-buttons { max-width: 310px; text-align: right; }
</style>

</asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
<div class="row std-page">
	
	<div class="span12">
		<!--Body content-->

		<%=Html.InfoMessage() %>

		<h1><%=Model.Comp.Title.HtmlEncode()%></h1>

		<%if (Model.Comp.Picture != null) { %>
			<img src="<%=Web.Attachments+Model.Comp.Picture %>" title="Ziera shoes competition" align="right" class="competition-image"/>
		<%} %>
		
		<%if (Model.IsExpired) {%>
			<%=Model.Comp.CompetitionClosedTextHtml.FmtHtmlText()%>
		<%} else{ %>
			<div id="ThankYou">
				<%=Model.Comp.IntroTextHtml.FmtHtmlText()%>

				<div style="margin-top:10px">
					<form class="form-horizontal AutoValidate" action="/">
					<fieldset>
						<div class="control-group">
							<label class="control-label" for="FirstName">First Name*</label>
							<div class="controls">
								<%=new Forms.TextField("FirstName", "", true) { cssClass = "inputField"} %>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="LastName">Last Name*</label>
							<div class="controls">
								<%=new Forms.TextField("LastName", "", true) { cssClass = "inputField"} %>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="Email">Email*</label>
							<div class="controls">
								<%=new Forms.TextField("Email", "", true){ cssClass = "inputField"} %>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="Phone">Phone*</label>
							<div class="controls">
								<%=new Forms.PhoneField("Phone", "", true){ cssClass = "inputField"} %>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="Email">Country*</label>
							<div class="controls">
								<div class="validation autoPosition" id="validation_Country"></div>
								<label class="radio"><input type="radio" name="Country" value="New Zealand" class="required"/>New Zealand</label>
								<label class="radio"><input type="radio" name="Country" value="Australia" class="required"/>Australia</label>
							</div>
						</div>
						<div class="control-group">
							<div class="controls ie7">
								<label class="checkbox">
									<%=new Forms.CheckboxField("OptInFutureEmails",true) %>
									<%=Model.FutureEmailsQuestion.BodyTextHtml%>
								</label>
								<label class="checkbox">
								<div class="validation autoPosition" id="validation_AgreedTerms">You must accept the terms and conditions to enter the competition</div>
									<%=new Forms.CheckboxField("AgreedTerms",false){IsRequired = true }%>
									I agree to the <a data-toggle="modal" href="#terms" >terms and conditions</a>
								</label>
							</div>
						</div>
				
						<div class="control-group competition-buttons">
								<input type="hidden" name="CompetitionID" value="<%=Model.Comp.ID %>"/>
								<input type="hidden" name="CompetitionTitle" value="<%=Model.Comp.Title %>"/>
								<input type="button" class="btn btn-primary btn-right-arrow" onclick=" AjaxSubmitEntry(this.form, false); return false;" value="Enter">
						</div>
					</fieldset>
					</form>
				</div>
			</div>
		<%} %>
	</div>	

</div>
	
<div id="terms" style="display: none;outline:none" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="terms" aria-hidden="true">
	<div class="modal-header">
		<a href="#" class="close" data-dismiss="modal">×</a>
		<h1><%=Model.Terms.Title%></h1>
	</div>
	<div class="modal-body">
		<%=Model.Terms.BodyTextHtml.FmtHtmlText() %>
	</div>
	<div class="modal-footer">
		<a href="#" class="btn btn-primary" onclick="$('#terms').modal('hide');">OK</a>
	</div>
</div>
<script>
	function AjaxSubmitEntry(form, wantsToJoin) {
		var isValid = CheckBasicFormValidation(form);
		if (isValid) {
			var url = "<%=Web.Root%>Competition/SubmitEntry?auth=<%=Model.AuthToken%>&wantsToJoin=" + wantsToJoin + "&" + $(form).serialize();
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: url,
				dataType: "json",
				success: function (response) {
					if (response.kind == "redirect") {
						window.location = response.redirectUrl;
					} else {
						$("#ThankYou").html(response.text);
					}
				}
			});
		}
		return false;
	}
</script>
<%Util.IncludeSavvyValidate(); %>
<%Util.IncludeBewebForms(); %>
<%Util.IncludeJQueryUI(); %>
</asp:Content>

