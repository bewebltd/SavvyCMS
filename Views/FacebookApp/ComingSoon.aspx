<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.HomeController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<script>
		
		$(function () {
			$('#submit').bind('click', function () {
				var $email = $('#email');
				var email = $.trim($email.val());
				if(email == '' || !validateEmail(email)) {
					$email.addClass('error');
				} else {
					$email.removeClass('error');
					$.ajax({
						type: 'POST',
						url: websiteBaseUrl + 'Home/Subscribe',
						data: { email: email },
						beforeSend: function () {
							$('#submit').hide();
							$('.loading').show();
						},
						success: function (response) {
							$('.loading').hide();
							$('.status').show();
							if (response == "OK") {
								$('.status').show().addClass('success');
							} else {
								$('.status').show().addClass('error');
							}
						},
						error: function () {
							$('.loading').hide();
							$('.status').show().addClass('error');
						}
					});
				}
			});
		});
		
		function validateEmail(email) { 
			var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
			return re.test(email);
		} 

	</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	
    <div class="comingsoon">

        <div class="box">
            <h1><img src="<%=ResolveUrl("~/images/logo.png") %>" /></h1>
            <% if(Model.ClosingDate <= DateTime.Now) { %>
							<h2>Game has ended</h2>
							<p class="text">Sorry, but this game has ended up on <%=((DateTime)Settings.All.ClosingDate).ToString("dd/MM/yyyy")%>.</p>
						<% } else { %>
							<h2>Coming Soon</h2>
							<p class="text">Our new cider launch competition starts on <%=((DateTime)Settings.All.OpeningDate).ToString("dd/MM/yyyy")%>. But you can register now by entering your email address.</p>
							<p class="fieldset">
								<input type="text" class="field" id="email" name="email" placeholder="Enter your email address" />
								<span id="submit" class="btn">Submit</span>
								<span class="loading" style="display:none"></span>
								<span class="status" style="display:none"></span>
							</p>
							<p class="smalltext">By opting into this notification you will be added to our email list. <br/> You can unsubscribe at any time.</p>
						<% } %>
        </div>
        
        <img class="dog-head" src="<%=ResolveUrl("~/images/dog-head.png") %>" />
        
    </div>
	
</asp:Content>