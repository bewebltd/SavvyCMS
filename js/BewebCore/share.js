(function ($) {

	/*
		Savvy Share Library - AF 2015
		
		Usage:
		- Create a target element ('div' or 'a' tag)
			- <div id="facebook-share-button"></div>
			- <a id="facebook-share-button"><img src="images/social/facebook-share.png" /></a>

			Initialisation 
			- Facebook (Parameters: URL)
				- $('#facebook-share-button').svyShare('facebook', { url: 'http://www.beweb.co.nz' });
			- Twitter (Parameters: URL, Title, Via)
				- $('#twitter-share-button').svyShare('twitter', { url: 'http://www.beweb.co.nz', title: 'Beweb Website', via: 'beweb' });
			- Google+ (Parameters: URL, Title, Via)
				- $('#googleplus-share-button').svyShare('google+', { url: 'http://www.beweb.co.nz' });
			- Pinterest (Parameters: URL, Description, Image)
				- $('#pinterest-share-button').svyShare('pinterest', { url: 'http://www.beweb.co.nz', description: 'Websites in Auckland', image: 'http://www.beweb.co.nz/images/logo.png' });
			- LinkedIn (Parameters: URL, Title, Description, Via)
				- $('#linkedin-share-button').svyShare('linkedin', { url: 'http://www.beweb.co.nz', title: 'Beweb Website', description: 'Websites in Auckland', via: 'beweb' });
			- All of them (Parameters: URL, Title, Description, Image, Via)
				- $('#share-buttons').svyShare('all', { url: 'http://www.beweb.co.nz', title: 'Beweb Website', description: 'Websites in Auckland', image: 'http://www.beweb.co.nz/images/logo.png', via: 'beweb' });	

			Notes: 
			* If you leave the URL parameter blank, the current URL will be used
			* Facebook and Google+ no longer support description and image parameters as they will be read from the meta tags in page being shared
			* Useful tags:
				<meta property="og:title" content="TITLE" />
				<meta property="og:type" content="article" />
				<meta property="og:url" content="FULL URL" />
				<meta property="og:description" content="SHORT DESCRIPTION" />
				<meta property="og:image" content="FULL URL TO IMAGE" />
				<meta itemprop="name" content="TITLE"/>
				<meta itemprop="url" content="FULL URL"/>
				<meta itemprop="author" content="WEBSITE AUTHOR"/>	*/

	$.fn.svyShare = function (type, options) {
		return new SavvyShare(this, type, options);
	};

	// SavvyShare defaults that can be overridden globally
	$.fn.svyShare.defaults = {
		//url: window.location.href,
	};

	var SavvyShare = function (element, type, options) {
		// Private Variables
		var $element = $(element);

		// This is the easiest way to have default options.
		var settings = $.extend({}, $.fn.svyShare.defaults, options);

		var main = {
			init: function () {

				if (!settings.url) {
					settings.url = window.location.href;
				}

				if (type == 'facebook' || type == 'all') initFacebook();
				if (type == 'twitter' || type == 'all') initTwitter();
				if (type == 'google+' || type == 'all') initGooglePlus();
				if (type == 'pinterest' || type == 'all') initPinterest();
				if (type == 'linkedin' || type == 'all') initLinkedIn();
			}
		};

		// Private
		function getUrl() {
			return encodeURIComponent(settings.url);
		}

		// Private
		function initFacebook() {
			var facebookURL = 'http://www.facebook.com/sharer.php?s=100&p[url]=' + getUrl();
			if ($element.is('a')) {
				$element.attr('href', facebookURL);
			} else {
				$element.html('<a href="' + facebookURL + '">' + window.websiteBaseUrl + 'images/social/facebook-share.png</a>');
			}
		}

		// Private
		function initTwitter() {

			var twitterURL = 'https://twitter.com/intent/tweet?original_referer=' + getUrl();

			if (settings.title) {
				twitterURL += '&text=' + encodeURIComponent(settings.title);
			}

			twitterURL += '&url=' + getUrl();

			if (settings.via) {
				twitterURL += '&via=' + encodeURIComponent(settings.via);
			}

			if ($element.is('a')) {
				$element.attr('href', twitterURL);
			} else {
				$element.html('<a href="' + twitterURL + '">' + window.websiteBaseUrl + 'images/social/twitter-tweet.png</a>');
			}

		}

		// Private
		function initGooglePlus() {
			var googlePlusURL = 'https://plus.google.com/share?url=' + getUrl();
			if ($element.is('a')) {
				$element.attr('href', googlePlusURL);
			} else {
				$element.html('<a href="' + googlePlusURL + '">' + window.websiteBaseUrl + 'images/social/googleplus-share.png</a>');
			}
		}

		// Private
		function initPinterest() {

			var pinterestURL = 'http://www.pinterest.com/pin/create/button/?url=' + getUrl();

			if (settings.image) {
				pinterestURL += '&media=' + encodeURIComponent(settings.image);
			}

			if (settings.description) {
				pinterestURL += '&description=' + encodeURIComponent(settings.description);
			}

			if ($element.is('a')) {
				$element.attr('href', pinterestURL);
			} else {
				$element.html('<a href="' + pinterestURL + '">' + window.websiteBaseUrl + 'images/social/pinterest-pinit.png</a>');
			}

		}

		// Private
		function initLinkedIn() {

			var linkedinURL = 'http://www.linkedin.com/shareArticle?mini=true&url=' + getUrl();

			if (settings.title) {
				linkedinURL += '&title=' + encodeURIComponent(settings.title);
			}

			if (settings.description) {
				linkedinURL += '&summary=' + encodeURIComponent(settings.description);
			}

			if (settings.via) {
				linkedinURL += '&source=' + encodeURIComponent(settings.via);
			}

			if ($element.is('a')) {
				$element.attr('href', linkedinURL);
			} else {
				$element.html('<a href="' + linkedinURL + '">' + window.websiteBaseUrl + 'images/social/linkedin-share.png</a>');
			}

		}

		// Init SvyShare
		main.init();
		return main;
	};

}(jQuery));