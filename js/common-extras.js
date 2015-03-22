/* All code snippets you can use in projects.
	 Copy these into your common.js if you need them
*/

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
	lastWin = window.open(url, name.replace(/\s/g,''), 'left=' + left + ',top=' + top + ',screenX=' + left + ',screenY=' + top + ',width=' + width + ',height=' + height + ',scrollbars=1,resizable=0,toolbar=0,location=0,directories=0,status=0,menubar=0,copyhistory=0');
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

// functions for checking and scaling text width

$.fn.textWidth = function () {
	var html_org = $(this).html();
	var html_calc = '<span>' + html_org + '</span>';
	$(this).html(html_calc);
	var width = $(this).find('span:first').width();
	$(this).html(html_org);
	return width;
};

$.fn.autoFontSize = function (maxFontSize, maxWidth) {
	var titleWidth = 0;
	var fontSize = 10;
	do {
		fontSize++;
		$(this).css("font-size", fontSize + "px");
		titleWidth = $(this).textWidth();
	} while (titleWidth < maxWidth && fontSize <= maxFontSize);
	// step back down one to fit within
	fontSize--;
	$(this).css("font-size", fontSize + "px");
};

// css3 animation function
$.fn.animateCss = function (cssProperties, milliseconds, callback) {
	var jq = this;
	//if ($.browser.msie && $.browser.version < 10) {
	if (!document.createElement('p').style.transition) {
		jq.animate(cssProperties, milliseconds, callback);
	} else {
		var doAnimation = function () {
			var trans = "all " + milliseconds + "ms";
			jq.css({ "-webkit-transition": trans, "-moz-transition": trans, "-ms-transition": trans, "-o-transition": trans, "transition": trans });
			jq.css(cssProperties);
			window.setTimeout(function () {
				// clean up
				var trans = "none";
				jq.css({ "-webkit-transition": trans, "-moz-transition": trans, "-ms-transition": trans, "-o-transition": trans, "transition": trans });
				// call our callback
				if (callback && $.isFunction(callback)) {
					jq.each(callback);
				}
				// let jquery do the next thing in animation queue
				jq.dequeue("fx");
			}, milliseconds);
		};
		jq.queue("fx", doAnimation);
	}
	return jq;
};

$.fn.fadeInCss = function (milliseconds) {
	var jq = this;
	jq.css({ "opacity": 0 }).show().animateCss({ "opacity": 1 }, milliseconds);
	return jq;
};

$.fn.fadeOutCss = function (milliseconds) {
	var jq = this;
	jq.css({ "opacity": 1 }).animateCss({ "opacity": 1 }, milliseconds, function () { jq.hide(); });
	return jq;
};

////////////////////////////////////////////
// accordian code
////////////////////////////////////////////

var currentAccordian = null;

function ToggleAccordian(anchor) {
	var accordian = jQuery(anchor).parents(".hoteltitle").get(0);
	if (accordian == currentAccordian) {
		// currently selected, so hide it
		jQuery(currentAccordian).removeClass("selected");
		jQuery(".subtablesmall", currentAccordian).slideUp();
		jQuery(currentAccordian).next().hide();
		// now nothing is selected
		currentAccordian = null;
	} else {
		if (currentAccordian!=null) {
			// hide previous thing
			jQuery(currentAccordian).removeClass("selected");
			jQuery(".subtablesmall", currentAccordian).slideUp();
			jQuery(currentAccordian).next().hide();
		}
		// set selected to thing you clicked
		currentAccordian = accordian;
		// show thing you clicked
		jQuery(currentAccordian).addClass("selected");
		jQuery(".subtablesmall", currentAccordian).fadeIn();
		jQuery(currentAccordian).next().fadeIn();
	}
}

////////////////////////////////////////////
// Placeholder Text
////////////////////////////////////////////

$(document).ready(function () {

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
		$('input[data-placeholder], textarea[data-placeholder]').each(function (i) {
			DataPlaceHolderOff(this);
		});
	});
});


function isMobileJs() {
	var a = navigator.userAgent || navigator.vendor || window.opera;
	return /(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4));
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

function ShowHideElement(elementID) {
	var prop;
	if (document.getElementById)
		prop = document.getElementById(elementID).style;
	else if (document.all)
		prop = document.all[elementID].style;
	else if (document.layers)
		prop = document.layers[elementID];
	if (prop.display == 'none')
		prop.display = '';
	else
		prop.display = 'none';
}

function goToByScroll(id) {
	$("html,body").animate({ scrollTop: $("#" + id).offset().top - 100 }, "slow");
}

function BackToTop() {
	$("html,body").animate({ scrollTop: 0 }, "slow");
}

/**
* Function : dump()
* Arguments: The data - array,hash(associative array),object
*    The level - OPTIONAL
* Returns  : The textual representation of the array.
* This function was inspired by the print_r function of PHP.
* This will accept some data as the argument and return a
* text that will be a more readable version of the
* array/hash/object that is given.
* Docs: http://www.openjs.com/scripts/others/dump_function_php_print_r.php
*/
function dump(arr, level) {
	var dumped_text = "";
	if (!level) level = 0;

	//The padding given at the beginning of the line.
	var level_padding = "";
	for (var j = 0; j < level + 1; j++) level_padding += "    ";

	if (typeof (arr) == 'object') { //Array/Hashes/Objects 
		for (var item in arr) {
			var value = arr[item];

			if (typeof (value) == 'object') { //If it is an array,
				dumped_text += level_padding + "'" + item + "' ...\n";
				dumped_text += dump(value, level + 1);
			} else {
				dumped_text += level_padding + "'" + item + "' => \"" + value + "\"\n";
			}
		}
	} else { //Stings/Chars/Numbers etc.
		dumped_text = "===>" + arr + "<===(" + typeof (arr) + ")";
	}
	return dumped_text;
}

function IsFlashInstalled() {
	return (typeof navigator.plugins == "undefined" || navigator.plugins.length == 0) ? !!(new ActiveXObject("ShockwaveFlash.ShockwaveFlash")) : navigator.plugins["Shockwave Flash"];
}

$(document).ready(function () {
	if(window.isMobile) {
		$(".normal table").wrap("<div class='responsive-table-scroll' />");
		//$("img").wrap("<div class='responsive-image-scroll' />");
		$('.normal img').css({ "max-width": "100%", height: "auto" }).removeAttr("height").removeAttr("width"); //this may cause image with image resizing.
		
		var autoShrinkImages = function () {
			$("img.responsive-autoshrink").css({ "max-width": $("body").width(), "height": "auto" });
		};
		autoShrinkImages();
		$(window).bind("load resize orientationchange", autoShrinkImages);
	}

	if ($.fn.colorbox) {
		$("a[target='colorbox']").colorbox({ iframe: true, width: "600px", height: "70%" });
		$("a.colorbox").colorbox({ photo: true });
	}

	$('.go-to-top').click(function () {
		$("html,body").animate({ scrollTop: 0 }, "slow");
		return false;
	});

	//Setup Select2 Helper
	if ($('.svySelect2').length > 0) {
		$('.svySelect2').select2();
	}
	// detect flash
	var isFlashSupported = IsFlashInstalled();
	if (!isFlashSupported) {
		$(".flash-only").hide();
	}
	savvyValidateUseLineByLineErrors = true;
	savvyValidateDisplayOverlaid = true;
});
function isNumericOnly(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}

function grayOut(vis, options) {
	// Pass true to gray out screen, false to ungray
	// options are optional.  This is a JSON object with the following (optional) properties
	// opacity:0-100         // Lower number = less grayout higher = more of a blackout 
	// zindex: #             // HTML elements with a higher zindex appear on top of the gray out
	// bgcolor: (#xxxxxx)    // Standard RGB Hex color code
	// grayOut(true, {'zindex':'50', 'bgcolor':'#0000FF', 'opacity':'70'});
	// Because options is JSON opacity/zindex/bgcolor are all optional and can appear
	// in any order.  Pass only the properties you need to set.
	var options = options || {};
	var zindex = options.zindex || 50;
	var opacity = options.opacity || 70;
	var opaque = (opacity / 100);
	var bgcolor = options.bgcolor || '#000000';
	var dark = document.getElementById('darkenScreenObject');
	if (!dark) {
		// The dark layer doesn't exist, it's never been created.  So we'll
		// create it here and apply some basic styles.
		// If you are getting errors in IE see: http://support.microsoft.com/default.aspx/kb/927917
		var tbody = document.getElementsByTagName("body")[0];
		var tnode = document.createElement('div');           // Create the layer.
		tnode.style.position = 'absolute';                 // Position absolutely
		tnode.style.top = '0px';                           // In the top
		tnode.style.left = '0px';                          // Left corner of the page
		tnode.style.overflow = 'hidden';                   // Try to avoid making scroll bars            
		tnode.style.display = 'none';                      // Start out Hidden
		tnode.id = 'darkenScreenObject';                   // Name it so we can find it later
		tbody.appendChild(tnode);                            // Add it to the web page
		dark = document.getElementById('darkenScreenObject');  // Get the object.
	}
	if (vis) {
		// Calculate the page width and height 
		if (document.body && (document.body.scrollWidth || document.body.scrollHeight)) {
			var pageWidth = document.body.scrollWidth + 'px';
			var pageHeight = document.body.scrollHeight + 'px';
		} else if (document.body.offsetWidth) {
			var pageWidth = document.body.offsetWidth + 'px';
			var pageHeight = document.body.offsetHeight + 'px';
		} else {
			var pageWidth = '100%';
			var pageHeight = '100%';
		}
		//set the shader to cover the entire page and make it visible.
		dark.style.opacity = opaque;
		dark.style.MozOpacity = opaque;
		dark.style.filter = 'alpha(opacity=' + opacity + ')';
		dark.style.zIndex = zindex;
		dark.style.backgroundColor = bgcolor;
		dark.style.width = pageWidth;
		dark.style.height = pageHeight;
		dark.style.display = 'block';
	} else {
		dark.style.display = 'none';
	}
}

function ellipsis(src, len) {
	var result = src + "";
	if (result.length > len) {
		// try and split on a word if there is an obvious point to break on
		var spacePos = result.lastIndexOf(' ', len - 3);
		if (spacePos > len * 0.5) {
			result = result.substr(0, spacePos);
		}
		// make sure never longer than max len
		if (result.length > len - 3) {
			result = result.substr(0, len - 3);
		}
		// add ... because to indicate there was more
		result = result + "...";
	}
	return result;
}

/*
//Works out the depth of children inside hierarchical data
// #subcategory is the container for all the elements you will loop through
// $(ele).data('meta') is used to get data-meta value placed on a child that contains its parents ID
$('#subcategory .checkboxes').each(function () {
	$(this).css('margin-left', 20 * getDepth($('input', this), 0));
});

function getDepth(ele, depth) {
	var parentID = $(ele).data('meta');
	if (!parentID) {
		return depth;
	}

	var parentEle = $('#subcategory input[value="' + parentID + '"]').eq(0);
	return getDepth(parentEle, depth + 1);
}
*/


/* example of infinite scroll */
/*
$(window).scroll(checkLoadMore);
$(document).ready(checkLoadMore);
var itemPos = 0;

function checkLoadMore() {
	// Check if we are at the bottom and if so, add more tiles
	var pos = getDocumentHeight();
	var clientHeight = $(window).height();
	var scrollPos = $(document).scrollTop();
	var height = pos - clientHeight;

	if (scrollPos >= height - 600) {
		// We are at the bottom so attempt to load more
		$('#something-list').append('<div id="something-loading"></div>');
		loadMore();
	}

};
*/

/*function loadMore() {
	// load more from server
	var url = websiteBaseUrl + 'something/loadmore?filter=whatever&pos=' + itemPos;
	$.get(url, function (html) {
		$('#something-list').append(html)
	})
}*/

function getDocumentHeight() {
	var D = document;
	return Math.max(Math.max(D.body.scrollHeight, D.documentElement.scrollHeight), Math.max(D.body.offsetHeight, D.documentElement.offsetHeight), Math.max(D.body.clientHeight, D.documentElement.clientHeight));
};

var loadMoreInProgess = false;

function loadMore() {
	if (loadMoreInProgess) {
		return;
	}

	loadMoreInProgess = true;

	var url = websiteBaseUrl + 'news/GetNextNews?nextRow=' + $("#nextRow").val();

	$.get(url, function (data) {

		var objLen = data.length;

		for (var i = 0; i < objLen; i++) {
			var html = ReplaceHtml(GetBox(), data[i]);
			$('#infiniteNewsList').append(html)
			$("#nextRow").val(data[i].NewsID)
			//alert(data[i].DateAdded)
		}
		if (objLen > 0) {
			checkLoadMore();
		}
		loadMoreInProgess = false;
	});
}


function ReplaceHtml(html, obj) {
	var newsDate = obj.Date
	var newsTitle = obj.Title
	var newsBody = obj.Body
	
	html = html.replace("[--newsDate--]", newsDate);
	html = html.replace("[--newsTitle--]", newsTitle);
	html = html.replace("[--newsBody--]", newsBody);

	return html;
}


function GetBox() {
	var html = "<div class='infiniteBox clearfix'>";
	html += "<ul class='ulInfiniteList'>";
	html += "<li><h5>[--newsTitle--]</h5><div class='newsDate'>[--newsDate--]</div></li>";
	html += "<li>[--newsBody--]</li>";
	html += "<li><p><a href='#'>MORE</a></p></li>";
	html += "</ul>";
	html += "<div class='clear'></div>";
	html += "</div>";
	return html;
}

function checkLoadMore() {
	// Check if we are at the bottom and if so, add more tiles
	var pos = getDocumentHeight();
	var clientHeight = $(window).height();
	var scrollPos = $(document).scrollTop();
	var height = pos - clientHeight;

	if (scrollPos >= height - 600) {
		// We are at the bottom so attempt to load more
		//$('#teacherList').append('<div id="teacherList-loading"></div>');
		loadMore();
	}
};

function getDocumentHeight() {
	var D = document;
	return Math.max(Math.max(D.body.scrollHeight, D.documentElement.scrollHeight), Math.max(D.body.offsetHeight, D.documentElement.offsetHeight), Math.max(D.body.clientHeight, D.documentElement.clientHeight));
};

/* end example of infinite scroll */


