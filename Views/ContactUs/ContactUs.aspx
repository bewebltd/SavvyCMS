<%@ Page Title="Contact Us" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.ContactUsController.ViewModel>" Language="C#" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	
	<style>
.svyFileSelect {
	width: 600px;

}
</style>
	<script src="<%=Web.Root %>js/BewebCore/Savvy.validate.js" type="text/javascript"></script>
		<%if(Model.CompanyDetails.Fields.CompanyPicture.MetaData.AllowPasteAndDrag) {%>
			<%//Util.IncludeStylesheetFile("~/js/filedrag.css"); %>
		<% } %>
	<%Util.IncludeJavascriptFile("~/js/BewebCore/forms.js"); %>

	<script type="text/javascript">
		<%-- This is for bots. use it and the SetTimeout Code below as a smart captcha --%>
		var token = '<%=Crypto.TimeToken%>'; 

		$(document).ready(function () {
			BewebInitForm("form")
			$('li.contactus a').addClass('selected');

			<%-- This is for bots. use it and the SetTimeout Code below as a smart captcha --%>
			setTimeout(function () {
				$("#cd").val(token);
			}, 2000);

		});

		function CheckForm(form) {
			var msg = '';
			var result = CheckBasicFormValidation(form)
			//extra validation here
			if (result) result = SavvyBeforeFormSubmit(form);
			return result;
		}
	</script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

	<div id="page-title">
		<h1><%:Model.ContentPage.Title %></h1>
		<p><%:Model.ContentPage.Introduction %></p>
	</div> <!-- /page-title -->

	<div id="contact-map">
<%--		<iframe width="100%" height="180" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://maps.google.com/maps?&q=<%=Model.CompanyDetails.Latitude %>,<%=Model.CompanyDetails.Longitude %>&ie=UTF8&output=embed"></iframe>--%>
	</div> <!-- /contact-map -->

	<div id="content">
		<div class="container">
			<div class="row">
				
				<div class="grid-8">
					<h3><span class="slash">//</span> <%:Model.ContactUsTextBlock.Title %></h3>
					<p><%=Model.ContactUsTextBlock.BodyTextHtml %></p>
					
					
<%--
					<article>
  <div id="holder"></div> 
  <p id="status">File API &amp; FileReader API not supported</p>
  <p>Drag an image from your desktop on to the drop zone above to see the browser read the contents of the file and use it as the background - without uploading the file to any servers.</p>
</article>
--%>

					<form method="post" action="" name="form" id="form">
						<fieldset>
							<div class="clearfix">
								<label for="name"><span>Name:</span></label>
								<div class="input">
									<%=new Forms.TextField("Name", true){ tabindex = 1 } %> 
								</div>
							</div>
							
							<div class="clearfix">
								<label for="email"><span>Email:</span></label>
								<div class="input">
									<%=new Forms.EmailField("Email", true){ cssClass = "input-xlarge", tabindex = 2 } %>
								</div>
							</div>
							
							<div class="clearfix">
								<label for="subject"><span>Subject:</span></label>
								<div class="input">
									<%=new Forms.TextField("Subject", true){ cssClass = "input-xlarge", tabindex = 3 }  %>
								</div>
							</div>
							<div class="clearfix">
								<label for="subject"><span>Picture:</span></label>
								<div class="input">
									<%=new Forms.PictureField(Model.CompanyDetails.Fields.CompanyPicture, true){ cssClass = "svyFileSelect input-xlarge", tabindex = 3 }  %>
								</div>
							</div>
							<div class="clearfix">
								<label for="subject"><span>Picture1:</span></label>
								<div class="input">
									<%=new Forms.PictureField(Model.CompanyDetails.Fields.CompanyPicture1, true){ cssClass = "svyFileSelect input-xlarge", tabindex = 3 }  %>
								</div>
							</div>
							
							<div class="clearfix">
								<label for="message"><span>Message:</span></label>
								<div class="input">
									<%=new Forms.TextArea("Message", "", true){ cssClass = "input-xlarge", rows = 7, tabindex = 4 } %>
								</div>
							</div>

							<div class="actions">
								<button tabindex="5" type="submit" class="btn btn-warning btn-large" onclick="return CheckForm(form)">Send message</button>
							</div>
						</fieldset>
						<%=new Forms.HiddenField("Base64","") %> <%--this will be injected..--%>
						<%=new Forms.HiddenField("Screen","ContactUs") %>
						<%=new Forms.HiddenField("cd") %>
						<%=Html.AntiForgeryToken() %>
						<%=Html.ReturnPageToken() %>
					</form>
				</div> <!-- /grid-8 -->
				
				
				<div class="grid-4">
					<div class="sidebar">
						<h3><span class="slash">//</span> <%:Model.CompanyDetails.Title %></h3>
						<p>
							<strong>Address</strong> <br>
							<%=Model.CompanyDetails.Address.FmtPlainTextAsHtml() %>
						</p>
						<p>
							<strong>Phone</strong><br>
							<%=Model.CompanyDetails.Phone.FmtPlainTextAsHtml() %>
						</p>						
						<p>
							<strong>Email</strong><br>
							<a href="mailto:<%:Model.CompanyDetails.Email %>"><%:Model.CompanyDetails.Email %></a>
						</p>
					</div> <!-- /sidebar -->					
				</div> <!-- /grid-4 -->
				
			</div> <!-- /row -->
		</div> <!-- /container -->
	</div> <!-- /content -->
	

</asp:Content>
