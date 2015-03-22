<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.HomeController.FacebookScriptViewModel>" %>
<div id="fb-root"></div>
	<script>

		window.fbAsyncInit = function () {
			// init the FB JS SDK
			FB.init({
				appId: '<%=Util.GetSetting("FacebookAppId") %>', // App ID from the App Dashboard
				channelUrl: '<%=Web.Root %>channel.html', // Channel File for x-domain communication
				status: true, // check the login status upon init?
				cookie: true, // set sessions cookies to allow your server to access the session?
				xfbml: true,  // parse XFBML tags on this page?
				oauth: true
			});

			// Additional initialization code such as adding Event Listeners goes here
			FB.Canvas.setSize();

			<%if (Model.IsAllowed) { %>

			FB.getLoginStatus(function (response) {
				if (response.status !== 'connected') {
					login();
				} else {
					register();
				}
			});

			function login() {
				FB.login(function (response) {
					if (response.authResponse) {
						register();
					}
				}, { scope: '' });
			}

			function register() {
				FB.getLoginStatus(function (response) {
					if (response.status === 'connected') {
						FB.api('/me', function (userInfo) {
							$.ajax({
								type: 'POST',
								url: websiteBaseUrl + 'Home/Register',
								data: userInfo,
								success: function (response) {
									var obj = $.parseJSON(response);
									if (obj.status === "saved" || document.location.href.search('GamePlay') == -1) {
										// If it's safari and didn't fix it yet
										if (!getCookieSupport() && !obj.cookieSet) {

											$('#wrapper').show();
											$('.likeus-message').css('visibility', 'hidden');

											$('#cookie-error').modal({
												closeButton: false,
												buttons: [
														{
															label: "Allow Wild Side Cider Cookie",
															click: function (modal) {
																openCookieFixWindow();
															}
														}
												]
											}).show();

										} else {
											document.location.href = document.location.href; // This is the only way to get reload working on Safari inside an iframe - AF20140515
										}
									} else {
										document.getElementById('wrapper').style.display = 'block';
									}
								},
								error: function (response) {
									console.log('Error: ' + response);
								}
							});
						});
					}
				});
			}

			<%} %>
		};

		var cookieFixPopup = null;
		function openCookieFixWindow() {
			cookieFixPopup = window.open(websiteBaseUrl + "Home/CookieFix", "CookieFix", "");
		}

		function cookieFixWindowClosed(cookieIsSupported) {
			window.setTimeout(function () {
				if (cookieFixPopup.closed) {
					if (cookieIsSupported) {
						$('#cookie-error_modal h3').html('<span style="color:green;font-size:18px">Thanks. The Wild Side Cider cookie has been allowed.<br/><br/>Please refresh the page to start the game.</span>');
					} else {
						$('#cookie-error_modal h3').html('<span style="font-size:18px"><span style="color:#c00;">Sorry. Your browser settings do not allow cookies to be set.</span><br/><br/>Please adjust your browser settings if you wish to play the game, and then refresh the page.</span>');
					}

					$('#cookie-error_modal .modal-buttons').remove();
				}
			}, 100);
		}

		function getCookieSupport() {
			var persist = true;
			do {
				var c = 'gCStest=' + Math.floor(Math.random() * 100000000);
				document.cookie = persist ? c + ';expires=Tue, 01-Jan-2030 00:00:00 GMT' : c;
				if (document.cookie.indexOf(c) !== -1) {
					document.cookie = c + ';expires=Sat, 01-Jan-2000 00:00:00 GMT';
					return persist;
				}
			} while (!(persist = !persist));
			return null;
		}

		(function (d, debug) {
			var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
			if (d.getElementById(id)) { return; }
			js = d.createElement('script'); js.id = id; js.async = true;
			js.src = "//connect.facebook.net/en_US/all" + (debug ? "/debug" : "") + ".js";
			ref.parentNode.insertBefore(js, ref);
		}(document, /*debug*/false));
	</script>