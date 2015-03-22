/* Functions for the advlink plugin popup */

tinyMCEPopup.requireLangPack();

function insert(embedCode) {
	tinyMCEPopup.execCommand('mceInsertContent', false, embedCode)
	tinyMCEPopup.close();
}

$(document).ready(function () {
	$('.coOrds,.dimension').keyup(function (e) {
		reCalc(e);
	});

	$('.coOrds,.dimension').keydown(function (e) {
		reCalc(e);
	});
});


function reCalc(e) {
	var el = e.currentTarget;

	if (isNumber($(el).val())) {
		var num = parseInt($(el).val());
		if (e.keyCode == 38) {
			num++;
		} else if (e.keyCode == 40) {
			num--;
		} else {
			num = parseInt($(el).val());
		}

		$(el).val(num);

		if ($(".svyYouTubeIframe").length > 0) {
			if (el.id == "width") {
				$(".svyYouTubeIframe")[0].setAttribute("width", num);
			}
			if (el.id == "height") {
				$(".svyYouTubeIframe")[0].setAttribute("height", num);
			}
			if (el.id == "top") {
				$('.svyYouTubeIframe').css({ paddingTop: num });
			}
			if (el.id == "bottom") {
				$(".svyYouTubeIframe").css({ paddingBottom: num });
			}
			if (el.id == "left") {
				$(".svyYouTubeIframe").css({ paddingLeft: num });
			}
			if (el.id == "right") {
				$(".svyYouTubeIframe").css({ paddingRight: num });
			}
		}
	} else {
		$(el).val("0");
	}

}

var embedCode = '';

function showInput(obj) {
	$("#previewVideo").html("")
	$("#embed").hide();
	$("#error").hide();
	$("#error").html("");
	$(".inputWrapper").hide();

	var type = obj.id;
	switch (type) {
		case 'fullPathRadio':
			$(".fullPathInputWrapper").show();
			$(".dimensionsWrapper").show();
			break;
		case 'embedPathRadio':
			$(".embedPathInputWrapper").show();
			$(".dimensionsWrapper").show();
			break;
		case 'embedPathCompleteRadio':
			$(".embedPathCompleteInputWrapper").show();
			break;
	}
	$(".previewWrapper").show();
}

function GetClickedRadio() {
	return $('.embedRadio:checked').attr("id");
}

function makeEmbedForFull(code) {
	var width = $("#width").val();
	var height = $("#height").val();
	var top = 0;
	var bottom = 0;
	var left = 0;
	var right = 0;
	var padding = '';
	if ($("#autoplay").is(":checked")) {
		code += '?autoplay=1';
	}

	if ($("#top").val() != '') {
		top = $("#top").val();
	}
	if ($("#bottom").val() != '') {
		bottom = $("#bottom").val();
	}
	if ($("#left").val() != '') {
		left = $("#left").val();
	}
	if ($("#right").val() != '') {
		right = $("#right").val();
	}
	padding = 'padding:' + top + 'px ' + right + 'px ' + bottom + 'px ' + left + 'px;'

	var iframeStr = "<iframe class='svyYouTubeIframe' style='" + padding + "' width='" + width + "' height='" + height + "' src='//www.youtube.com/embed/" + code + "' frameborder='0' allowfullscreen></iframe>";
	embedCode = iframeStr;
	return iframeStr;
}

function hasErrors(id) {
	switch (id) {
		case 'fullPathRadio':
			if ($("#fullPathInput").val() == "") {
				$("#error").show();
				$("#error").html("Please enter the full path of the video");
				return true;
			}
			break;
		case 'embedPathRadio':
			if ($("#embedPathInput").val() == "") {
				$("#error").show();
				$("#error").html("Please enter the full code of the video");
				return true;
			}
			break;
		case 'embedPathCompleteRadio':
			if ($("#embedPathCompleteInput").val() == "") {
				$("#error").show();
				$("#error").html("Please enter the full iframe of the video");
				return true;
			}
			break;
	}

	if (($("#width").val() == "" || $("#height").val() == "") && id != 'embedPathCompleteRadio') {
		$("#error").show();
		$("#error").html("Please enter the width and height of the video");
		return true;
	}
	return false;
}

function renderVideo(embed) {

	$(".embedwWrapper").hide()
	var radioSelected = $('.embedRadio:checked').attr("id");
	if (!hasErrors(radioSelected)) {
		switch (radioSelected) {
			case 'fullPathRadio':
				var videoCode = matchYoutubeUrl($("#fullPathInput").val());
				embedCode = makeEmbedForFull(videoCode);
				if (!embed) {
					$("#previewVideo").html(embedCode)
				}
				break;
			case 'embedPathRadio':
				videoCode = $("#embedPathInput").val();
				embedCode = makeEmbedForFull(videoCode);
				if (!embed) {
					$("#previewVideo").html(embedCode)
				}
				break;
			case 'embedPathCompleteRadio':
				videoCode = $("#embedPathCompleteInput").val();
				if (!embed) {
					$("#previewVideo").html(videoCode);
				}
				embedCode = videoCode;
				break;
		}

		if (!embed) {
			$("#embed").show();
			$("#previewVideo").slideDown();
		}
		else {
			insert(embedCode);
		}
	}
}

function matchYoutubeUrl(url) {
	var p = /^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
	return (url.match(p)) ? RegExp.$1 : false;
}

function showPadding() {
	$(".padding").slideDown();
}

function showHidePadding() {
	if ($(".padding").is(":visible")) {
		$(".padding").slideUp();
	}
	else {
		$(".padding").slideDown();
	}
}

function isNumber(n) {
	return !isNaN(parseFloat(n)) && isFinite(n);
}

