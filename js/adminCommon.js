var svyDevices = [
	{ name: "Desktop", width: svyMceDefaultWidth, height: svyMceDefaultHeight },
	{ name: "iphone3/4", width: 320, height: 480 },
	{ name: "iphone5", width: 320, height: 568 },
	{ name: "iPad", width: 768, height: 1024 },
	{ name: "Nexus 10", width: 800, height: 1280 },
	{ name: "Nexus 4", width: 384, height: 598 },
	{ name: "Nexus 5", width: 360, height: 598 },
	{ name: "Nexus 7", width: 601, height: 962 },
	{ name: "Samsung Galaxy 5", width: 320, height: 427 },
	{ name: "Samsung Galaxy S3", width: 360, height: 640 },
	{ name: "Samsung Galaxy S4", width: 360, height: 640 }
]

$(document).ready(function () {
	var html = "";
	$.each(svyDevices, function(index,obj) {
		html += "<option value='" + index + "'>" + obj.name + "</option>";
	});




	$("#svyMobileOptions").html(html);
	//var page = document.URL;
	//if (window.isMobileJs() && page.indexOf('list.asp') > -1) {
	//if (window.isMobileJs()) {
	//if (window.isMobileJs()) {
	


	//if (window.isMobile) {
	if (true) {
		$(".databox, .normal table").wrap("<div class='responsive-table-scroll' />");
		$(".normal table").wrap("<div class='responsive-table-scroll' />");
	}
	//}
	DoFloatLabel();
});

$(window).resize(function () {
	DoFloatLabel();
});

var svyMceDefaultWidth = $("#BodyTextHtml_ifr").width();
var svyMceDefaultHeight = $("#BodyTextHtml_ifr").height();
var IsMobileView = false;

function svyMceMobileView(optionVal) {
	var mceEl = $("#BodyTextHtml_ifr").contents().find("#tinymce");
	mceEl.addClass("svyMobileLayout");
	if (optionVal != null) {	
		var width = 320;
		var height = 480;
		width = svyDevices[optionVal].width;
		height = svyDevices[optionVal].height;
		mceEl.css({
			height: height,
			width: width
		})
		tinyMCE.activeEditor.theme.resizeTo(width + 50, height);
	} else {
		var mceMobImg = $("#BodyTextHtml_mobile-view img");
		if (IsMobileView) {
			mceMobImg.attr('src', websiteBaseUrl + 'images/icon_mobile.png');
			svyMceMobileView(0)
			IsMobileView = false;
		} else {
			mceMobImg.attr('src', websiteBaseUrl + 'images/icon_monitor.png');
			svyMceMobileView(1) 
			IsMobileView = true;
		}
	}

	return false;
}

function DoFloatLabel() {
	var page = window.location.href;
	if (page.indexOf('Edit') > 1 && true) {
		AdjustFloatLabel();
	}
}

function isFloatScreen() {
	var tdWidth = $('td.label').width();
	return tdWidth < 200;
}

function AdjustFloatLabel() {
	if ($('td.label').width() < 200 && $('td.label').is(":visible")) {

		window.flabelWidth = $(window).width();

		$('.label').each(function () {
			var labelText = $(this).text();
			$(this).hide();

			var nextTd = $(this).next('td');
			if (nextTd.html().indexOf('flabel') == -1) {
				nextTd.prepend('<div class="flabel">' + labelText + '</div>')
			}
		});

		$('input,textarea,checkbox,select,radio').click(function () {
			$('.flabel').removeClass('flabelHighLight');
			$(this).prevAll('.flabel').addClass('flabelHighLight');

		});
		$('.flabel').show();
	}
	else if (window.flabelWidth && $(window).width() > window.flabelWidth) {
		$('td.label').show();
		$('.flabel').hide();
	}
}