////////////////////////////////////////////
// popups
////////////////////////////////////////////

var lastWin;

function PopupScreenCentre(url, name, width, height, scrollbars) {
	var titleBarHeight, windowBorderWidth;
	titleBarHeight = 24;
	windowBorderWidth = 4;
	var screenWidth, screenHeight;
	screenWidth = 800;
	screenHeight = 600;
	if (window.screen) {
		if (window.screen.availWidth) {
			// ok browser has the appropriate properties we need to centre it
			screenWidth = window.screen.availWidth;
			screenHeight = window.screen.availHeight;
		}
	}

	var windowWidth = windowBorderWidth + width + windowBorderWidth;
	var windowHeight = titleBarHeight + height + windowBorderWidth;
	var left = (screenWidth - windowWidth) / 2;
	var top = (screenHeight - windowHeight) / 2;
	if (windowHeight > screenHeight) height = screenHeight - titleBarHeight - windowBorderWidth
	if (windowWidth > screenWidth) width = screenWidth - windowBorderWidth - windowBorderWidth
	if (lastWin && !lastWin.closed) {
		lastWin.close();
	}
	lastWin = window.open(url, name.replace(/\s/g, ''), 'left=' + left + ',top=' + top + ',screenX=' + left + ',screenY=' + top + ',width=' + width + ',height=' + height + ',scrollbars=1,resizable=0,toolbar=0,location=0,directories=0,status=0,menubar=0,copyhistory=0');
	//do this to use colorbox instead
	//$.fn.colorbox({width: (width+70)+"px",height: height+"px", iframe: true, href:url+"", opacity: 0.1});
}

function ImagePopup(filename) {
	PopupScreenCentre("attachments/" + filename, "_blank", 780, 550, 0);
}

////////////////////////////////////////////
// column height fixing
////////////////////////////////////////////

$.fn.evenUpHeights = function () {
	EvenUpHeights($(this));
};

function EvenUpHeights(selector) {
	// get tallest
	var maxHeight = 0;
	$(selector).each(function (i) {
		if ($(this).height() > maxHeight) { maxHeight = $(this).height(); }
	});
	$(selector).height(maxHeight + "px");
}

////////////////////////////////////////////
// Placeholder Text
////////////////////////////////////////////

$(document).ready(function () {


	// AF20141017: Beweb placeholder should always be used if the browser doesn't support the HTML5 one
	if (!placeholderIsSupported()) {
		$('input[placeholder],textarea[placeholder]').each(function (i) {
			$(this).attr('data-placeholder', $(this).attr('placeholder'));
		});
	}

	$('input[data-placeholder], textarea[data-placeholder]').each(function (i) {
		DataPlaceHolderOn(this);
	}).focus(function () {
		DataPlaceHolderOff(this);
	}).blur(function () {
		DataPlaceHolderOn(this);
	}).change(function () {
		DataPlaceHolderOff(this);
	});

	$('input[data-placeholder], textarea[data-placeholder]').parents('form').submit(function () {
		// AF20141107: Only hide the placeholders of the form that was submitted, not all of them.
		$('input[data-placeholder], textarea[data-placeholder]', this).each(function (i) {
			DataPlaceHolderOff(this);
		});
	});

	if(window.websiteImagesBaseUrl) {
		$('img').error(function() {

			if($(this).data('reloaded')) {
				return;
			}

			$(this).data('reloaded', true);
			var src = $(this).attr('src');
			if(src[0]== '/') {
				src = window.websiteImagesBaseUrl +src.substr(1);
			} else {
				src = src.replace(window.websiteBaseUrl, window.websiteImagesBaseUrl);
			}
			$(this).attr('src', src);
		});
	}

});

function placeholderIsSupported() {
	var test = document.createElement('input');
	return ('placeholder' in test);
}

function DataPlaceHolderOn(ele) {
	if ($(ele).val() == $(ele).attr('data-placeholder')) {   // fix for firefox re-stuffing values into text boxes on refresh/back
		$(ele).val('');
	}
	if ($(ele).val() == '') {
		$(ele).val($(ele).attr('data-placeholder')).addClass('data-placeholder-on');
	}
}
function DataPlaceHolderOff(ele) {
	if ($(ele).val() == $(ele).attr('data-placeholder') && $(ele).hasClass('data-placeholder-on')) {
		$(ele).val('').removeClass('data-placeholder-on');
	}

	if ($(ele).val() != $(ele).attr('data-placeholder') && $(ele).hasClass('data-placeholder-on')) {
		$(ele).removeClass('data-placeholder-on');
	}
}

////////////////////////////////////////////
// Common functions
////////////////////////////////////////////

function goToByScroll(id) {
	$("html,body").animate({ scrollTop: $("#" + id).offset().top - 100 }, "slow");
}

function BackToTop() {
	$("html,body").animate({ scrollTop: 0 }, "slow");
}


$(document).ready(function () {

	// beweb error handling
	window.onerror = function (message, url, lineNumber) {
		var pageUrl = window.location.href;
		//var msg = "Savvy CMS Detected a Javascript Error:\n";
		//msg += "\nmessage: " + message;
		//msg += "\nurl: " + url;
		//msg += "\npage url: " + window.location.href;
		//msg += "\nline: " + lineNumber;
		//msg += "\nbrowser: " + navigator.userAgent;
		//msg += "\nPlease take a screenshot (ctrl-PrtScn) and report this to the developers.";

		var errorData = { message: message, url: url, line: lineNumber, browser: navigator.userAgent, pageUrl: pageUrl };

		if (window.showDetailedErrors) {
			throw errorData;
			//alert(msg);
			//debugger; // launches debugger generally only if F12 is open
		} else {
			// ignore unfixable errors
			if (message == "Script error." && lineNumber == 0) {
				return;
			} else if (message + "" == "" && lineNumber == 0) {
				return;
			} else if (message.indexOf("unsupported pseudo") > -1) {
				return;
			} else if (message.indexOf("connect.facebook.net") > -1 && message.indexOf("all.js") > -1) {
				return;
			}
			// send notifications of important errors
			$.post(websiteBaseUrl + "Error/LogJavascriptError", errorData);
		}
		//alert(printStackTrace({ guess: true }).join("\n"));
	};
	if (window.showDetailedErrors) {
		$(window).ajaxError(function(e, jqxhr, settings, exception) {
			alert("Ajax Error: The following ajax call failed. It will now be launched in a new window so you can see the error.\n\n" + settings.url);
			window.open(settings.url);
		});
	} else {
		$(window).ajaxError(function (e, jqxhr, settings, exception) {
			if (window.console) {
				console.error("Ajax Error: The following ajax call failed. " + settings.url);
			}
		});
	}

	// if needed
	window.isTouch = ("ontouchstart" in document.documentElement);
	document.documentElement.className += (window.isTouch) ? ' touch' : ' no-touch';

	// if needed
	if (window.isMobile) {
		$(".normal table").wrap("<div class='responsive-table-scroll' />");
		//$("img").wrap("<div class='responsive-image-scroll' />");
		$('.normal img').css({ "max-width": "100%", height: "auto" }).removeAttr("height").removeAttr("width"); //this may cause image with image resizing.

		var autoShrinkImages = function () {
			$("img.responsive-autoshrink").css({ "max-width": $("body").width(), "height": "auto" });
		};
		autoShrinkImages();
		$(window).bind("load resize orientationchange", autoShrinkImages);
	}

	// colorbox - if needed - used by admin for image previews
	if ($.fn.colorbox) {
		$("a[target='colorbox']").colorbox({ iframe: true, width: "600px", height: "70%", maxWidth: "100%", maxHeight: "100%" });
		$("a.colorbox,a.popup").colorbox({ photo: true, maxWidth: "100%", maxHeight: "100%" });
	}

	// to top - if needed
	$('.go-to-top').click(function () {
		$("html,body").animate({ scrollTop: 0 }, "slow");
		return false;
	});

	//Setup Select2 Helper -- MOVED TO Forms.js JC 20140703

	// savvy validate global settings
	window.savvyValidateUseLineByLineErrors = true;
	window.savvyValidateDisplayOverlaid = true;
	window.savvyValidateVerifyEmails = true;
});

function isNumericOnly(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}

var svyMceDefaultWidth = $("#BodyTextHtml_ifr").width();
var svyMceDefaultHeight = $("#BodyTextHtml_ifr").height();
var IsMobileView = false;

function svyMceMobileView(device) {

	$("#BodyTextHtml_ifr").contents().find("#tinymce").removeClass("svyIphone4 svyIphone5 svyiPad svyNexus4 svyGalaxy4 svyIphone");

	if (!IsMobileView) {
		$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyMobileLayout");
		var width = 320;
		var height = 480;
		if (device) {
			var deviceVal = device.value;
			if (deviceVal == "iphone 4") {
				$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyIphone4");
				width = 320;
				height = 480;
			} else if (deviceVal == "iphone 5") {
				$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyIphone5");
				width = 320;
				height = 568;
			} else if (deviceVal == "iPad 3/ 4") {
				$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyiPad");
				width = 768;
				height = 1024;
			} else if (deviceVal == "Nexus 4") {
				$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyNexus4");
				width = 384;
				height = 640;
			} else if (deviceVal == "Samsung Galaxy S4") {
				$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyGalaxy4");
				width = 360;
				height = 640;
			} else if (deviceVal == "Desktop") {
				width = svyMceDefaultWidth;
				height = svyMceDefaultHeight;
				$("#BodyTextHtml_ifr").contents().find("#tinymce").removeClass("svyIphone");
			}
		} else {
			$("#BodyTextHtml_ifr").contents().find("#tinymce").addClass("svyIphone");
			$("#BodyTextHtml_mobile-view img").attr('src', websiteBaseUrl + 'images/icon_monitor.png');
			IsMobileView = true;

		}
		tinyMCE.activeEditor.theme.resizeTo(width + 50, height);

	} else {

		$("#BodyTextHtml_ifr").contents().find("#tinymce").removeClass("svyIphone");
		tinyMCE.activeEditor.theme.resizeTo(svyMceDefaultWidth, svyMceDefaultHeight);
		$("#BodyTextHtml_mobile-view img").attr('src', websiteBaseUrl + 'images/icon_mobile.png');
		IsMobileView = false;
	}
	return false;
}

Date.prototype.yyyymmdd = function() {
   var yyyy = this.getFullYear().toString();
   var mm = (this.getMonth()+1).toString(); // getMonth() is zero-based
   var dd  = this.getDate().toString();
   return yyyy + (mm[1]?mm:"0"+mm[0]) + (dd[1]?dd:"0"+dd[0]); // padding
  };
Date.prototype.yyyymmddhhmmss = function() {
   var hour = this.getHours().toString();
   var min = (this.getMinutes()).toString();
   var sec  = this.getSeconds().toString();
   return this.yyyymmdd() +(hour[1]?hour:"0"+hour[0]) + (min[1]?min:"0"+min[0])  +(sec[1]?sec:"0"+sec[0]) ; // padding
  };



