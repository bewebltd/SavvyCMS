window.fbAsyncInit = function () {
	FB.init({
		appId: FbAppID, // App ID
		channelUrl: FbchannelUrl, // Channel File
		status: true, // check login status
		cookie: true, // enable cookies to allow the server to access the session
		xfbml: true  // parse XFBML
	});

	FB.Event.subscribe('auth.login', function (response) {
		if (typeof customFB == 'function') {
			customFB(response);
		}
	});
};

var uiCount = 0;
var totalUiCount = 0;
function fbInviteMsg(fbUserID, fbName) {
	FB.ui({
		to: fbUserID,
		method: 'send',
		link: websiteBaseUrl + "Family/Join?InviteID=" + fbUserID
	},
	function (response) {
		uiCount++;
		
		if (response && response.success) {
			$.post(websiteBaseUrl + 'Invite/Add', { 'FacebookUserID': fbUserID, 'FacebookName': fbName, 'UserID': userID, 'TimeToken': timeToken });
		}
		
		if (uiCount >= totalUiCount) {
			if (typeof window.closePopup == 'function') {
				return window.closePopup();
			}

			window.location.href = websiteBaseUrl + 'Family/Tree';
		}
	});
}

function fblogin() {
	FB.getLoginStatus(function (response) {
		if (response.status === 'connected') {
			if (typeof customFB == 'function') {
				customFB(response);
			}
		} else if (response.status === 'not_authorized') {
			login();
		} else {
			login();
		}
	});
}

function login() {
	FB.login(function (response) { }, { scope: 'email, user_birthday, read_friendlists' });
}

function fillOutRegisterForm() {
	FB.api('/me', function (response) {
		$('form').show();
		//$('.aboutYou > :not(h2)').hide();

		$('.ui-datepicker-trigger').remove();
		$('.hasDatepicker').removeClass('hasDatepicker');
		$('#FirstName').val(response.first_name).attr('readonly', 'readonly').addClass('disabled');
		$('#LastName').val(response.last_name).attr('readonly', 'readonly').addClass('disabled');
		$('#Email').val(response.email).attr('readonly', 'readonly').addClass('disabled');


		if (response.birthday && response.birthday.indexOf('/') > -1) {
			var splitDate = response.birthday.split('/');
			var birthDay = new Date(splitDate[2], splitDate[0] - 1, splitDate[1]);
			$('#BirthDate').val(df_FmtDate(birthDay)).removeClass('data-placeholder-on').attr('readonly', 'readonly').addClass('disabled');
		}

		$('#Password').hide().prev().hide().next().hide();
		$('#ConfirmPassword').hide().prev().hide().next().hide();
		$('.svyPictureContainer').hide();
		$('.exampleProfileImage').hide();

		if ($('.facebookProfileImage').length < 1) {
			$('.svyPictureContainer').prev().append('<img src="https://graph.facebook.com/' + response.id + '/picture?type=large" width="88" class="facebookProfileImage" />');
		}

		$('#FacebookUserID').val(response.id);
	});
}

var excludedIdsCache = "";
function fbFriendSelector() {
	var excludeIDs = [];
	FB.api('/me/friends', function (response) {
		if (response.data) {
			
			var friendsIDs = $.map(response.data, function (obj, index) {
				return obj.id;
			}).join(",");

			if (excludedIdsCache == "") {
				$.post(websiteBaseUrl + 'Invite/ExcludeIDs', { 'friends': friendsIDs }, function(data) {
					excludedIdsCache = $.map(data, function(val, index) {
						return parseInt(val.FacebookUserID, 10);
					});
					
					showFriendSelector();
				});
			} else {
				showFriendSelector();
			}
		}
	});
}

function showFriendSelector() {
	totalUiCount = 0;
	uiCount = 0;

	$('.triggerFriendsList').fSelector({
		excludeIds: excludedIdsCache,
		max: 10,
		showButtonSelectAll: false,
		lang: {
			title: "Friend Selector <div style='font-size: 10px;'>Some users may not allow you to send them a private message therefore you may get an error.</div>",
			selectedLimitResult: "Limit is {0} people at a time.",
		},
		closeOnSubmit: true,
		onSubmit: function (response) {
			$.each(response, function(k, v) {
				totalUiCount++;
			});

			$.each(response, function (k, v) {
				FB.api('/'+v, function (response) {
					fbInviteMsg(v, response.name)
				});
			});
		}
	});
	
	$('.triggerFriendsList').click();
}