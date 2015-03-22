/// <reference path="../tapmodo-Jcrop/js/jquery.Jcrop.min.js" />
// ------------------------------------------------------
// This file contains ONLY generic Beweb Savvy functions
// ------------------------------------------------------

// ------------------------------------------
// For List Pages
// ------------------------------------------

//global - stores id of 'clicked-on' or active drag container
var svyCopyPasteDiv;
var svyDontUpload = false;

function svyIsDragAndDropable() {
	return (typeof (window.FileReader) != 'undefined');
}

function IsSvyHtml5Paste() {
	var isSvyChrome = $.browser.chrome;
	if (typeof isSvyChrome === 'undefined') {
		isSvyChrome = navigator.userAgent.indexOf('Chrome') != -1;
	}
	return isSvyChrome;
}

function IsDragAndPastEnabled(fieldName) {
	return $('#file_' + fieldName).attr('data-meta-allowpasteanddrag') == "true";
}

function IsForceAjax(fieldName) {
	return $('#file_' + fieldName).attr('data-meta-forceajax') == "true";
}

$(document).ready(function () {
	if (!svyIsDragAndDropable()) { 
		$(".svyHtml5").hide(); //hides all svyHtml5 elements from the page
		$(".svyPicContainer").each(function () {
			var fieldName = $(this).attr("id").substr(4);
			//$("#fileBrowseLink_" + fieldName).hide();                         //20150106jn removed - not showing on safari
			$("#file_" + fieldName).removeClass("svyAllowPasteAndDrag");
		});
	}
	$(".svyCancelPaste").hide();

	$(".svyMobileFileUpload").each(function () {
		var elId = this.id;
		var buttonId = elId + "Button";
		$("#" + buttonId).click(function () {
			$("#" + elId).trigger('click');
		});
	});
	try {
		$("a.colorbox").colorbox();
	} catch (e) {
		//oh well, no colourbox
	}
	//sets up the listeners for attachments - if you want to use this add a class of svyAttachmentAjax to the attachment field or use 'AllowAjax = true in the {}'
	$(".svyAttachmentAjax").each(function () {
		var inputName = $(this).attr("name");
		svyInitHtml(inputName); //this sets up listeners
	});

	// Setup Select2 Helper 
	// AF20140808: I've specified the elements to avoid JS errors
	// http://stackoverflow.com/questions/14483348/query-function-not-defined-for-select2-undefined-error
	if ($('select.svySelect2, input.svySelect2').length > 0) {
		$('select.svySelect2, input.svySelect2').select2({"width":"resolve"});
	}
	/*
	automatically filter a table to show only rows that match input box text
	example: <br>Filter table: <input type="text" class="svyFilterBox"/>
	<table class="databox"><tr class="row-<%=Beweb.Html.OddEven %> svyFilterRow"><td>blah</td></tr></table>
 	*/
	$('.svyFilterBox').on('keyup', function () {
		var filter = $(this).val(), count = 0;

		$(".svyFilterRow").each(function () {
			if ($(this).text().search(new RegExp(filter, "i")) < 0) {
				$(this).hide();
			} else {
				$(this).show();
				count++;
			}
		});
	});

});

$(document).on('keyup', '.svyCheckboxFilter', function () {
	var filter = $(this).val(), count = 0;
	// find checkbox group inside container or any on page
	var container = $(this).parents(".svyCheckboxContainer");
	if (container.length == 0) {
		container = $(document);
	}
	container.find(".svyCheckboxes .checkboxes").each(function () {
		if ($(this).text().search(new RegExp(filter, "i")) < 0) {
			$(this).hide();
		} else {
			$(this).show();
			count++;
		}
	});
});

function GoPageNum(PageNum) {
	var form = document.forms["form"];
	form["PageNum"].value = PageNum;
	form.submit();
}

function ajaxSave(saveData) {
	alert('ajaxSave incomplete:' + saveData)
}

function ColSortBy(sortFieldName) {
	var form = document.forms["form"]
	if (form["ColSortField"].value == sortFieldName) {
		if (form["ColSortDesc"].value == "1") {
			form["ColSortDesc"].value = "0";
		} else {
			form["ColSortDesc"].value = "1";
		}
	} else {
		form["ColSortField"].value = sortFieldName;
		form["ColSortDesc"].value = "0";
	}
	form.submit();
}

function svyInitSortPositionTableDrag(savingURL) {
	//drag sort position code
	var rd = REDIPS.drag, msg;
	rd.style.borderEnabled = "none";
	$("table.databox,table.svySubform").wrap("<div id='drag'>");											 //20140324jn added subform
	$("td.dataheading,tr.colhead td,td.colhead,tr.colheadfilters td,tr.row-last td").addClass("mark");
	$(".svyRowSort").each(function () {
		var sortgroup = $(this).data("sortgroup");
		$(this).parents("td").addClass("sortgroup" + sortgroup);
	});

	rd.init();
	rd.hover.colorTr = '#9BB3DA';
	rd.hover.borderTr = '2px solid #32568E';

	rd.event.rowClicked = function () {
		// mark all other rows as not allowed for dragging
		$("table.databox td").addClass("mark");
		// mark rows in this sortgroup as allowed
		var sortgroup = $(".svyRowSort", rd.obj).data("sortgroup");
		$("table.databox td.sortgroup" + sortgroup).removeClass("mark");
		//if (window.console) console.log("sortgroup sorting: " + sortgroup)
	};
	rd.event.rowMoved = function () {
		rd.rowOpacity(rd.obj, 85);
		rd.rowOpacity(rd.objOld, 20, 'White');
	};
	rd.event.rowDroppedSource = function () {
		rd.rowOpacity(rd.objOld, 100);
	};
	rd.event.rowDropped = function (droppedRow) {
		var sortgroup = $(".svyRowSort", droppedRow).data("sortgroup");
		$("td", droppedRow).addClass("svySlowColor");
		window.setTimeout(function () { $("td", droppedRow).addClass("svyFlashRow"); }, 100);
		window.setTimeout(function () { $("td", droppedRow).removeClass("svyFlashRow"); }, 300);

		$(".svyAjaxStatus").html("Saving...").show();
		var str = "";
		var pos = 1;
		$(".svyRowSort").each(function () {
			var sortSpan = this;
			if (str != "") str += ",";
			str += $(sortSpan).attr('data-pkid');
			sortSpan.innerHTML = "";
			$(sortSpan).hide();

			var myPos = pos;
			window.setTimeout(function () {
				$(sortSpan).html(10 * myPos).fadeIn(500);
				$(sortSpan).parent().find('input').val(10 * myPos);
			}, myPos * 50);
			pos++;
		});
		//if (window.console) console.log("saving sortgroup:"+sortgroup+", sort: "+str)
		$.get(savingURL + "&sortOrder=" + str, function (response) {
			$(".svyAjaxStatus").html(response).delay(2000).fadeOut(1000);
			if (response.indexOf("reload") > -1) {
				// ajax replace out the whole page
				$("table.databox").load(window.location.href + " table.databox", function () {
					// reintialise buttons and drag handlers
					$(".svyAdmin .row-odd td:first-child a, .row-even td:first-child a").each(function() {
						if (($(this).attr('class') + '').search('select2') == -1) {
							$(this).addClass("btn").addClass("btn-mini");
						}
					});
					svyInitSortPositionTableDrag(savingURL);
				});
			}
		});
		//$(".svyAjaxStatus").load(savingURL + "&sortOrder=" + str).delay(1000).fadeOut(1000);
	};

}


// ------------------------------------------
// For Edit Forms Pages
// ------------------------------------------

// ------------------------------------------
// For Rich Text Controls
// ------------------------------------------

function IsZeroParagraphSpacing(sourceElement, sourceWindow) {
	// measure P inside the iframe to see if P tags have zero margins (in which case we convert them to single line break else we convert them to double line breaks)
	var usingZeroParaSpacing = false
	var p = null
	var paragraphElements = sourceElement.getElementsByTagName("p")
	if (paragraphElements && paragraphElements.length > 0) {
		p = paragraphElements[0]
	}
	if (p != null) {
		var marginTop, marginBottom
		if (sourceWindow.getComputedStyle) {
			marginTop = parseInt(sourceWindow.getComputedStyle(p, null).getPropertyValue("margin-top"))
			marginBottom = parseInt(sourceWindow.getComputedStyle(p, null).getPropertyValue("margin-bottom"))
		} else if (p.currentStyle) {
			marginTop = parseInt(p.currentStyle.marginTop)
			marginBottom = parseInt(p.currentStyle.marginBottom)
		} else {
			marginTop = parseInt(p.marginTop)
			marginBottom = parseInt(p.marginBottom)
		}
		if (marginTop == 0 && marginBottom == 0) {
			usingZeroParaSpacing = true
		}
	}
	return usingZeroParaSpacing
}

function CopyHtmlToPlainText(htmlFieldName, plainTextFieldName) {
	// get text version
	var sourceWindow = document.getElementById(htmlFieldName + '_ifr').contentWindow;
	var sourceElement = sourceWindow.document.body;
	var el = document.getElementById("PasteBoard")
	var usingZeroParaSpacing = IsZeroParagraphSpacing(sourceElement, sourceWindow)

	// get text version
	var html = sourceElement.innerHTML;
	html = html.replace(/<LI>/ig, "* ")
	html = html.replace(/<\/LI>/ig, "<br>")
	html = html.replace(/<UL>/ig, "")
	html = html.replace(/<\/UL>/ig, "")
	// put URLs in brackets after any links
	html = html.replace(/<A href=\"mailto:([^\"]*?)\".*?>(.*?)<\/A>/ig, "$2 \($1\)")
	html = html.replace(/<A mce_href=\"mailto:([^\"]*?)\".*?>(.*?)<\/A>/ig, "$2 \($1\)")
	html = html.replace(/<A href=\"([^\"]*?)\".*?>(.*?)<\/A>/ig, "$2 \($1\)")
	html = html.replace(/<A mce_href=\"([^\"]*?)\".*?>(.*?)<\/A>/ig, "$2 \($1\)")
	var txt
	if (el.textContent != undefined) {
		// normalise line breaks
		html = html.replace(/<br><\/P>/ig, "</P>")
		html = html.replace(/\r/ig, "")
		html = html.replace(/\n/ig, "")
		if (usingZeroParaSpacing) {
			html = html.replace(/<\/p>/ig, "</p>\r\n")
		} else {
			html = html.replace(/<\/p>/ig, "</p>\r\n\r\n")
		}
		html = html.replace(/<br>/ig, "<br>\r\n")
		html = html.replace(/<br \/>/ig, "<br />\r\n")
		el.innerHTML = html
		txt = el.textContent;
	} else if (el.innerText != undefined) {
		html = html.replace(/<P>/ig, "")
		if (usingZeroParaSpacing) {
			html = html.replace(/<\/P>/ig, "<br>")
		} else {
			html = html.replace(/<\/P>/ig, "<br><br>")
		}
		el.innerHTML = html
		txt = el.innerText;
	} else {
		alert("Sorry, your browser does not support converting HTML to plain text.")
		return
	}

	document.getElementById(plainTextFieldName).value = txt;
}

// ------------------------------------------
// For Picture Upload/Selector Controls
// ------------------------------------------

function svyResetAfterCancelAction(fieldName) {
	$('#ulc_' + fieldName).show();
	//if (svyIsMce()) {
	//	svyResetImageMceWindow();
	//}
	//show upload, swap select and file 'name' fields
	var fileObj = document.getElementById('file_' + fieldName);
	var listBoxDomObj = document.getElementById('select_' + fieldName); 

	if (fileObj.name.indexOf('svyPicInactive_') != -1) //only swap if not already swapped
	{
		// swap names (of "svyPicInactive_" to the active normal field name)
		var holdname = fileObj.name;
		fileObj.name = listBoxDomObj.name;
		listBoxDomObj.name = holdname;
	}
	//hide generic 	
	$('#svyFiledragLive_' + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyCancelPaste").hide();
	$("#ulc_" + fieldName).find(".svyLinkContainer").hide();
	$('#svyPasteAgainImageLink_' + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyCancelPaste").hide();
	//hide all cancel links

	$('#filedim_' + fieldName).show();
	//$('#file_' + fieldName).hide();
	$('#sel_' + fieldName).hide();
	$('#file_' + fieldName).show();

	$('#selectFromServer_' + fieldName).show();
	$('#selsv_' + fieldName).show();
	$('#fileBrowseLink_' + fieldName).show();

	if (!svyIsDragAndDropable() && !IsSvyHtml5Paste()) { //cant do $fn anything but svyApplet - so show paste svyApplet link only
		$('#fileBrowseLink_' + fieldName).show();
		$('#fileBrowseLink_' + fieldName + " .svyPasteLink").show();
	}
	else if (svyIsDragAndDropable() && !IsSvyHtml5Paste()) {
		$('#svyPasteImageLink_' + fieldName).show();
		$('#svyDragImageLink_' + fieldName).show();
	}
	else if (svyIsDragAndDropable() && IsSvyHtml5Paste()) { //show drag and svyHtml5 paste link

		$('#fileBrowseLink_' + fieldName).show();
		$('#fileBrowseLink_' + fieldName + " .svyPasteLink").show();

		$('#svyPasteImageLink_' + fieldName).show();
		$('#svyPasteImageLink_' + fieldName + " .svyPasteLink").show();

		$('#svyDragImageLink_' + fieldName).show();
		$('#svyDragImageLink_' + fieldName + " .svyPasteLink").show();
	}

	$('#svyImagePasteProgress_' + fieldName).hide();
	$('#paste_' + fieldName).val("");

	//if the preview pic is in the dom
	if ($('#shp_' + fieldName).length > 0) {
		$('#cancelPicture_' + fieldName).show();
	}
	if ($('#scale_' + fieldName).length > 0) {
		$('#scale_' + fieldName).show();
	}
	if ($('#svyFreeImageLink_' + fieldName).length > 0) {
		$('#svyImageLink_' + fieldName).show();
	}
	if ($('#scale_' + fieldName).length > 0) {
		$('#scale_' + fieldName).show();
	}
}

function svyRemovePicture(fieldName) {
	svyResetAfterCancelAction(fieldName);
	svyPictureRemove(fieldName, true);  ///$('#' + fieldName + "_remove").attr('checked', 'checked');
	$('#removePictureLink_' + fieldName).hide();
	$('#shp_' + fieldName).hide();
	$('#cancelPicture_' + fieldName).hide();
	$("#shp_" + fieldName + " img").remove();
}

function svyCancelSelectPicture(fieldName) {
	//rset preview
	var imgPreview = $('#pv_' + fieldName);
	$(imgPreview).attr("src", websiteBaseUrl + "admin/images/testpattern.gif");
	svyResetAfterCancelAction(fieldName);
}

function cancelChangePicture(fieldName) {
	$('#ulc_' + fieldName).hide();
	$('#cancelPicture_' + fieldName).hide();
	$('#shp_' + fieldName).show();
	$('#removePictureLink_' + fieldName).show();
	$('#changePictureLink_' + fieldName).show();
	$('#file_' + fieldName).val("");
	$('#paste_' + fieldName).val("");
}

function cancelFileSelect(fieldName) {
	svyResetAfterCancelAction(fieldName);
}

function svyCancelHtml5Paste(fieldName) {
	svyResetAfterCancelAction(fieldName);
}

function svyCancel(fieldName) {
	$('#file_' + fieldName).val("");
	$('#paste_' + fieldName).val("");
	$('#file_' + fieldName).replaceWith($('#file_' + fieldName).clone()); //mainly for IE8 (and probably other versions) as fileselects are read-only

	if ($(".svyFileNameText_" + fieldName).length > 0) {
		$(".svyFileNameText_" + fieldName).text("");
	}

	svyResetAfterCancelAction(fieldName);

	// If it's showing the field for the iframe uploader, remove it
	$('#svyIframeUploaderStatus').remove();
	$('#tempIframeUploaderForm').remove();

	svyRemovePictureClasses(fieldName);

}

function svyRemovePictureClasses(fieldName) {
	$("#svyFiledragLive_" + fieldName).removeClass('svyPasteArea');
	$("#svyFiledragLive_" + fieldName).removeClass('svyDragArea');
}

function svyCancelHtml5Drag(fieldName) {
	svyResetAfterCancelAction(fieldName);
}

// when user clicks "choose file"
function handleFileBrowse(fieldName) {
	svyDontUpload = false;
	$("#svyImagePasteProgress_" + fieldName).hide();
	//$('#selsv_' + fieldName).hide(); why did we do this? why did you do this? who did this?! cm <- didnt do this

	svyInitHtml(fieldName); //this sets up listeners
	return;

	if ((!IsDragAndPastEnabled(fieldName) || (!svyIsDragAndDropable() && IsDragAndPastEnabled(fieldName)))) { //not set to paste drag or drag paste not supported
		$('#svyPasteLink_' + fieldName).hide();

	}
	 else {
			svyInitHtml(fieldName); //this sets up listeners
	}
}

function svyPictureRemove(fieldName, newState) {
	var chk = document.getElementById(fieldName + "_remove");
	if (chk) {
		chk.checked = newState;
	}
}

function svyOpenFileSelect(fieldName) {
	$('#ulc_' + fieldName).show();
	$('#file_' + fieldName).show();
	svyPictureRemove(fieldName, false); //$('#' + fieldName + "_remove").attr('checked', '');
	$('#filedim_' + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyLinkContainer").hide();
	$('#svyCancelCustomeBrowseLink_' + fieldName).show();
}

function svyHandleDragDrop(fieldName) {
	$("#file_" + fieldName).hide();
	$('#filedim_' + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyLinkContainer").hide(); //hide all links
	var isPasteAndDragEnabled = $("#paste_" + fieldName).length > 0;
	if (isPasteAndDragEnabled) {
		svyInitHtml(fieldName);
	}
	svyPictureRemove(fieldName, false); //$('#' + fieldName + "_remove").attr('checked', '');

	$('#svyCancelCustomeBrowseLink_' + fieldName).show();

	$('#svyFiledragLive_' + fieldName).show();
	$('#svyFiledragLive_' + fieldName).html("<div class='svyDragPaste'></div>");
	//$('#svyFiledragLive_' + fieldName).removeClass("svyFancyBackground");
	//$('#svyFiledragLive_' + fieldName).removeClass("svyFancyBackgroundActive");
	$("#svyFiledragLive_" + fieldName).addClass('svyDragArea');
}

function ShowClientImage(fileName, targetId) {
	$("#" + targetId).show();
	$("#" + targetId).html("<img src='" + fileName + "'/>");
	/*$("#" + targetId).addClass("svyWhiteBg");
	$("#" + targetId).removeClass("svyFancyBackground");*/
}

// when user clicks "select from server"
function svyHandleSelectPicture(servicespath, attachmentPath, fieldName, subfolder) {
	if (svyIsMce()) {
		subfolder = "pics";
	}
	$("#svyFreeImageLink_" + fieldName).hide();

	var qs = "";
	var url = servicespath + "GetAttachmentPictures.aspx?subfolder=" + escape(subfolder) + "&rnd=" + Math.random();
	$.ajax({
		type: "GET",
		url: url,
		data: qs,
		success: function (msg) {
			// callback - shows the file selection list
			var obj = $('#select_' + fieldName)
			var res = msg.split('\n')
			obj.data["storedOptions"] = res
			var opt = ""
			for (var sc = 0; sc < res.length - 1; sc++) {
				opt += "<option>" + res[sc] + "</option>"
			}
			obj.html(opt)
			$('#sel_' + fieldName).show();
			$('#selsv_' + fieldName).hide();
			$('#file_' + fieldName).hide();
			$('#filedim_' + fieldName).hide();
			$('#svyFiledragLive_' + fieldName).hide();
			svyPictureRemove(fieldName, false);  //$('#' + fieldName + "_remove").attr('checked', '');
			$("#ulc_" + fieldName).find(".svyLinkContainer").hide();
			$('#cnlsv_' + fieldName).css('display', 'inline-block');

		},
		error: function (msg) {
			//alert("add failed 2: " + msg.responseText);
		}
	});
}

// getElementById
function $id(id) {
	return document.getElementById(id);
}

function svyHandleImagePasteSelect(fieldName) {
	$('#sel_' + fieldName).hide();
	$('#selsv_' + fieldName).hide();
	$('#file_' + fieldName).hide();
	$('#filedim_' + fieldName).hide();
	$('#fileBrowseLink_' + fieldName).hide();
	$(".svyFiledragProgess").hide();
	$(".svyFiledragLive").hide();
}

// when user clicks image name, display the preview
function handleImagePreviewSelect(attachmentPath, listBoxDomObj, subfolder) {
	//hide upload
	//swap 'name' fields with upload id 'file_"+this.name' and select id 'select_"+this.name'
	if (svyIsMce()) {
		subfolder = "pics/";
	}
	var fieldName = listBoxDomObj.id.replace("select_", "")       //.split("_")[1]
	var fileObj = document.getElementById('file_' + fieldName)
	//var fileName = $(listBoxDomObj).val()
	var fileName = listBoxDomObj.options[listBoxDomObj.selectedIndex].text
	if (fileName != null) {
		// show image as a preview
		//$('#pv_'+fieldName).attr('src',attachmentPath+(fileName+'').replace('.','_pv.'))
		var previewImg = $('#pv_' + fieldName)[0]
		pictureSelectorLoadPreview(previewImg, attachmentPath + subfolder + (fileName + ''))

		if (svyIsMce()) {
			var s = document.getElementById('src');
			var aSrc = websiteBaseUrl + "attachments/pics/" + fileName;
			s.value = aSrc;
			return;
		}
		// set value in case subfolder is different
		//$(listBoxDomObj)handleImagePreviewSelect.val(subfolder + fileName);
		listBoxDomObj.options[listBoxDomObj.selectedIndex].value = subfolder + fileName;
	}
	// this is tricky: we swap the name of the listbox with the name of the file upload input box
	// this is so the posted form field has the correct name
	if (listBoxDomObj.name.indexOf('svyPicInactive_') != -1) //only swap if not already swapped
	{
		$(fileObj).hide()
		// swap names (of "svyPicInactive_" to the active normal field name)
		var holdname = fileObj.name
		fileObj.name = listBoxDomObj.name;
		listBoxDomObj.name = holdname
	}
}

// when user enters a filter search (onkeyup)
function filterServerImageList(filterbox, fieldName) {
	var selbox = document.getElementById('select_' + fieldName);

	var obj = $(selbox)
	var res = obj.data["storedOptions"]
	var opt = ""
	for (var sc = 0; sc < res.length - 1; sc++) {
		var imageFilename = res[sc]
		if (imageFilename.toLowerCase().indexOf(filterbox.value.toLowerCase()) != -1) {
			opt += "<option>" + imageFilename + "</option>"
		}
	}
	obj.html(opt)
}

// load the image into a temp var to check its size, then display it
function pictureSelectorLoadPreview(img, src) {
	pictureSelectorLoadPreviewImage(img, src.replace('.', '_tn.'), 0);
}

// load the image into a temp var to check its size, then display it
function pictureSelectorLoadPreviewImage(img, src, recursionDepth) {
	if (recursionDepth > 2) return;
	var imgPreloader = new Image();
	imgPreloader.onerror = function () {
		if (src.indexOf("_tn.") > -1) {
			pictureSelectorLoadPreviewImage(img, src.replace('_tn.', '.'), recursionDepth + 1);
		} else {
			// image not found - give up - don't keep asking!
		}
	}
	imgPreloader.onload = function () {

		imgPreloader.onload = null;

		var x = 190;
		var y = 220;
		var imageWidth = imgPreloader.width;
		var imageHeight = imgPreloader.height;
		if (imageWidth > 220 | imageHeight > 180) {
			var wscale = x / imageWidth;
			var hscale = y / imageHeight;
			var scale = (hscale < wscale ? hscale : wscale);
			imageWidth *= scale;
			imageHeight *= scale;
		}

		img.src = src;
		img.width = imageWidth;
		img.height = imageHeight;
	};

	if (!src) {
		src = img.src;
	}
	imgPreloader.src = src;
}

function browseSelectPicture(fieldName) {

	$('#file_' + fieldName).hide();

	if (!svyIsDragAndDropable()) {
		$("#fileBrowseLink_" + fieldName).hide();
		$('#selsv_' + fieldName).hide();
		$("#svyCancelCustomeBrowseLink_" + fieldName).show();
		uploadUsingIframe(fieldName); // For old IE
		return;
	}

	if (!IsDragAndPastEnabled(fieldName)) {
		//$('#file_' + fieldName).removeClass("svyFileUpload");
		if (!$('#file_' + fieldName).val()) {
			$(".svyFileNameText_" + fieldName).text("")
		}else{
			$(".svyFileNameText_" + fieldName).text(GetFileName(fieldName) + " - Picture Selected")
		}
		$("#svyCancelCustomeBrowseLink_" + fieldName).show();
	} else {
		handleFileBrowse(fieldName); //cm refactor
	}
}

function browseMobileSelectPicture(fieldName) {
	browseSelectPicture(fieldName);
}

function svyMobileOverlay() {
	var displayBoxOverlay = document.getElementById('displaybox');

	if(window.console)console.log(displayBoxOverlay);

	if (displayBoxOverlay) {
		if (displayBoxOverlay.style.display == "none") {
			displayBoxOverlay.style.display = "";
			displayBoxOverlay.innerHTML = "<div id='svyMobileOverlay'>Please Wait...<div style='padding:10px';><img src='" + websiteBaseUrl + "images/spinnerLarge.gif'></span></div>Your Image is Being Processed</div>";
		} else {
			displayBoxOverlay.style.display = "none";
			displayBoxOverlay.innerHTML = '';
		}
	}
	return false;
}

// ------------------------------------------
// End Picture Upload/Selector Controls
// ------------------------------------------

// ------------------------------------------
// Month Year Controls
// ------------------------------------------

function handleDateSelector(fieldID) {
	var newVal = '';


	if ($('#' + fieldID + "_day").length > 0) {
		if ($('#' + fieldID + "_day") && $('#' + fieldID + "_day").val() != '' && $('#' + fieldID + "_month").val() != '' && $('#' + fieldID + "_year").val() != '') //handle dd/mon/yyyy
		{
			// MN - changed separator from '/' to ' ' as this was not a valid javascript date (except in Chrome where anything goes)
			newVal = '' + $('#' + fieldID + "_day").val() + ' ' + $('#' + fieldID + "_month").val() + ' ' + $('#' + fieldID + "_year").val()
		}
	} else {
		if ($('#' + fieldID + "_month").val() != '' && $('#' + fieldID + "_year").val() != '')		//handle mon-yyyy
		{
			newVal = '1 ' + $('#' + fieldID + "_month").val() + ' ' + $('#' + fieldID + "_year").val();
		}
	}

	if ($('#' + fieldID + "_day").length > 0) {
		//note: changed to CheckDateField from CheckDateInput to support dropdowns as well as text input
		if (!CheckDateField($('#' + fieldID)[0], true)) {//true = show alert
			if ($('#' + fieldID + "_day") != null) {
				//$('#' + fieldID + "_day").val('1')			//reset to 1 - always valid, but 31 feb is not for example
				$('#' + fieldID + "_day")[0].selectedIndex = 0; //reset to no selection
				$('#' + fieldID + "_day").focus();
			} else {
				$('#' + fieldID + "_month").focus();
			}
			newVal = "";
		}
	} else {
		//alert('handleDateSelector:failed to locate day dropbox ['+fieldID+']')
		// this is ok, if there is no day dropbox - month/year only
	}
	//date ok, put it in the hidden
	$('#' + fieldID).val(newVal);
}


function handleDaySelector(fieldID) {
	handleDateSelector(fieldID);
}

function handleMonthSelector(fieldID) {
	handleDateSelector(fieldID);
}
function handleYearSelector(fieldID) {
	handleDateSelector(fieldID);
}
function handleMonthYearSelector(fieldID) {
	//handleDateSelector(fieldID);
	var newVal = "";
	if ($('#' + fieldID + "_monthyear").length > 0) {
		newVal = $('#' + fieldID + "_monthyear");
	} else {
		//alert('handleDateSelector:failed to locate day dropbox ['+fieldID+']')
		// this is ok, if there is no day dropbox - month/year only
	}
	//date ok, put it in the hidden
	$('#' + fieldID).val(newVal);
}

// ------------------------------------------
// Month Year Controls
// ------------------------------------------

// ------------------------------------------
// TextArea Controls
// ------------------------------------------

function textboxMultilineMaxNumber(txt, maxLen, showIndicator) {
	try {
		if (showIndicator) {
			$('#maxlen_' + txt.id).show()
			$('#maxlen_' + txt.id).attr('title', 'number of characters left to type')
		}
		var numLeft = maxLen - txt.value.length;
		if (numLeft < 0) numLeft = 0;
		var newTxt = '(' + (numLeft) + '&nbsp;/&nbsp;' + maxLen + ')'
		newTxt = newTxt.replace(new RegExp("0", "gm"), "O")

		if (showIndicator) {
			$('#maxlen_' + txt.id).html(newTxt)
		}
		if (txt.value.length >= (maxLen)) {
			var cont = txt.value;
			txt.value = cont.substring(0, maxLen - 1);
			return false;
		};
		if (showIndicator) {
			$('#' + txt.id).blur(function () { $('#maxlen_' + txt.id).hide() })
		}
	} catch (e) {
	}
}

// ------------------------------------------
// TextArea Controls
// ------------------------------------------

// ------------------------------------------
// Yes No Controls
// ------------------------------------------

function YesNoShow(yesNoRadio, selector) {
	// call this onclick (not onchange!)
	if (yesNoRadio.value.toLowerCase() == 'true') {
		$(selector).show()
	} else {
		$(selector).hide()
	}
}

// ------------------------------------------
//
// ------------------------------------------

function InitClockPick(iconElement) {
	var inputElement = $(iconElement).prevAll("input.time:first")[0];
	if (!iconElement.hasClockPick) {
		iconElement.hasClockPick = true;
		//iconElement.click = null;   // remove this handler?
		$(iconElement).clockpick({
			valuefield: inputElement,
			starthour: $(inputElement).data("earliestHour") || 0,
			endhour: $(inputElement).data("latestHour") || 23
		});
	}
}

function svyPasteProcessEvent(e) {
	if (!e.clipboardData || !window.FileReader) {
		alert("Sorry your browser does not support clipboard pasting");
		return;
	}
	if (e.clipboardData.items.length == 0) {
		alert("Sorry, you cannot paste an image file. If you are wanting to copy an image from the file system please use the drag and drop feature.");
		e.stopPropagation();
		return;
	}
	var isAnImage = false;
	for (var i = 0; i < e.clipboardData.items.length; i++) {
		// get the clipboard item
		var clipboardItem = e.clipboardData.items[i];
		var type = clipboardItem.type;

		// if it's an image add it to the image field
		if (type.indexOf("image") != -1) {
			isAnImage = true;
			$(svyCopyPasteDiv).find("p").html("Loading clipboard data...");
			// get the image content and create an img dom element
			var file = clipboardItem.getAsFile();
			var reader = new FileReader();
			var base64Str = "";
			// Closure to capture the file information.
			reader.onload = (function (blob) {
				return function (e) {
					base64Str = e.target.result
					// MN 20140625 removed base64 split, now does this later
					var fieldName = svyCopyPasteDiv.replace("svyFiledragLive_", "");
					svyProcessNewImage(fieldName, base64Str, "", "svyFiledragLive_" + fieldName);

				};
			})(file);
			reader.readAsDataURL(file);
		}
	}
	if (!isAnImage) {
		alert("Sorry, you must paste an image");
		e.stopPropagation();
		return;
	}
	svyDeactivateHtml5Paste(null);
}

function svyShowUploadImage(base64Str, fieldName, resize) {
	// show the image client side while we upload it
	var targetId = fieldName;
	$("#" + targetId + " a").hide()
	$("#" + targetId).show();

	if (resize) {
		// note this code is no longer needed
		//svyResizeAndRenderImage(base64Str, targetId);
	} else {
		//$("#" + targetId).addClass("noBackground");
		var imgSrc = base64Str + '';
		if (imgSrc.indexOf(',') == -1) {
			imgSrc = 'data:image/gif;base64,' + base64Str;
		}
		$("#" + targetId).html("<img src='" + imgSrc + "' />");
	}

	/*$("#" + targetId).addClass("svyWhiteBg");
	$("#" + targetId).removeClass("fancyBackground");
	$("#" + targetId).removeClass("fancyBackgroundActive");*/
}

function svyResizeAndRenderImage(imgSrc, targetId) {
	var image = $('<img/>');
	image.on('load', function () {
		var maxWidth = 800; // Max width for the image
		var maxHeight = 800;    // Max height for the image
		var ratio = 0;  // Used for aspect ratio
		var width = this.width;    // Current image width
		var height = this.height;  // Current image height

		// Check if the current width is larger than the max
		if (width > maxWidth) {
			ratio = maxWidth / width;   // get ratio for scaling image
			height = height * ratio;    // Reset height to match scaled image
			width = width * ratio;    // Reset width to match scaled image
		}

		// Check if current height is larger than max
		if (height > maxHeight) {
			ratio = maxHeight / height; // get ratio for scaling image
			width = width * ratio;    // Reset width to match scaled image
			height = height * ratio;    // Reset height to match scaled image
		}

		var canvas = document.createElement("Canvas");
		canvas.width = width;
		canvas.height = height;
		var ctx = canvas.getContext("2d");
		ctx.drawImage(this, 0, 0, width, height);

		if (navigator.userAgent.match(/(iPad|iPhone|iPod)/i)) {
			var transparent = imageDetectTransparency(ctx);
			if (transparent) {
				//Redraw image, doubling the height seems to fix the iOS6 issue.
				ctx.drawImage(this, 0, 0, width, height * 2.041);
			}
		}

		var newBase64Str = canvas.toDataURL("image/jpeg");   // MN  20140626 note: what about fallback here for android < 4 and other browsers that support canvas but not toDataURL?
		var thumb = $('<img/>');
		thumb.attr('src', newBase64Str);
		//$("#" + targetId).css("background", "none");
		$("#" + targetId).html(thumb);
	
		targetId = targetId.replace("svyFiledragLive_", "");
		//svyProcessNewImage(targetId, newBase64Str);

	});
	image.attr('src', imgSrc);
}


function svyIsMce() {
	var s = document.getElementById('mceImagePasteUpload');
	if (s == null) {
		return false;
	}
	return true;
}

function svyIsImage(base64Str) {
	var isImage = false;
	if (base64Str.toLowerCase().indexOf("image/png") != -1) {
		isImage = true;
	} else if (base64Str.toLowerCase().indexOf("image/jpeg") != -1) {
		isImage = true;
	} else if (base64Str.toLowerCase().indexOf("image/gif") != -1) {
		isImage = true;
	} else if (base64Str.toLowerCase().indexOf("image/bmp") != -1) {
		isImage = true;
	}
	return isImage;
}

function IsAcceptableMimeType(fileName, fileTypes) {
	var ext = fileTypes.split(',');
	for (var i = 0; i < ext.length; i++) {
		if (fileName.toLowerCase().indexOf("." + ext[i]) != -1) {
			return true;
		}
	}
	return false;
}

function IsNotAcceptableMimeType(fileName, fileTypes) {
	return !IsAcceptableMimeType(fileName, fileTypes);
}
function GetFileName(fieldName) {
	return $('#file_' + fieldName).val().replace(/C:\\fakepath\\/i, '');
}


function CheckAllowedTypes(fieldName, mimeTypes) {
	//get fieldname 

	var fileName = $('#file_' + fieldName).val()
	var isNotAcceptable = IsNotAcceptableMimeType(fileName, mimeTypes);
	if (isNotAcceptable) {
		alert("That file type is not allowed. Please upload one of the following: " + mimeTypes);
		svyDontUpload = true;
		$('#file_' + fieldName).val("");
		return false;
	}
	return true;
}

function svyProcessNewImage(fieldName, base64Str, fileName, targetId) {
	if (svyDontUpload) {
		return;
	}

	var mimeTypes = $("#attachmentMimeTypes_" + fieldName).val();

	if (mimeTypes) {
		if (IsNotAcceptableMimeType(fileName, mimeTypes)) {
			alert("That file type is not allowed. Please upload one of the following: " + mimeTypes);
			return;
		}
	}

	if ($(".displaybox")) {
		$(".displaybox").hide();
	}

	if (!IsForceAjax(fieldName)) {
		$('#svyCancelCustomeBrowseLink_' + fieldName).show();
		$("#ulc_" + fieldName).find(".svyLinkContainer").hide();
	}

	var showCropWindow = $('#file_' + fieldName).attr('data-meta-showcropwindow') == "true";
	if (showCropWindow && !isMobile) { //use this if mobile should never use cropping cm-1814
	//if (showCropWindow || true) {
	//if (false) {
		// show crop/preview dialog
		svyCropPicture(null, fieldName, base64Str, fileName, targetId);
	} else {
		// go straight on to uploading the file
		svyPrepUploadBase64File(fieldName, base64Str, fileName, targetId);
	}
}

function ShowCancelUpload(fieldName) {
	$('.svyPasteLink').hide();
	$('#svyCancelCustomeBrowseLink_' + fieldName).show();
	$('#svyCancelCustomeBrowseLink_' + fieldName + " .svyPasteLink").show();
}

function svyDoCrop(fieldName, base64Str, fileName, targetId) {

	svyPrepUploadBase64File(fieldName, base64Str, fileName, targetId);
	var parent = $('#file_' + fieldName + '').closest('.svyPictureContainer');
	parent.find('.svyCropDlg').remove();
	$(".cropcolumn").show();
}

function svyPrepUploadBase64File(fieldName, base64Str, fileName, targetId) {
	var url = websiteBaseUrl + "services/GetClipboardPicture.aspx";
	if (!fileName) {
		fileName = GetFileName(fieldName);
	}

	if (!svyIsImage(base64Str)) { //is an attachment
		url += "?uploadType=attachment&fileName=" + fileName;
		var subFolder = $("#attachmentSubFolder_" + fieldName).val();
		if (subFolder) {
			url += "&subFolder=" + subFolder;
		}
	} else {//is an image
		///is it mce
		if (svyIsMce()) {
			url += "?uploadType=mce&fileName=" + fileName;
		} else {
			url += "?uploadType=picture&fileName=" + fileName;
		}
		if (base64Str.indexOf(",") > -1) {
			base64Str = base64Str.split(",")[1];
		}
		if(targetId)svyShowUploadImage(base64Str, targetId, false);
	}

	$("#svyImagePasteProgress_" + fieldName).show();

	svyUploadBase64File(fieldName, base64Str, url);
}

function svyUploadBase64File(fieldName, base64Str, url) {
	var postData = { data: base64Str };
	var progressStatus = $("#svyImagePasteProgress_" + fieldName);
	progressStatus.css("min-width", "140px !important");
	var pTag = $(progressStatus).find("p");
	var spanTag = $(progressStatus).find("span");
	var pasteHiddenField = $("#paste_" + fieldName);
	var form = pasteHiddenField.parents("form")[0];

	if (window.svyOnBeforeUpload) {
		window.svyOnBeforeUpload(form, fieldName);
	}

	var jqxhr = $.ajax({
		type: "POST",
		url: url,
		data: postData,
		success: function (result) {
			pasteHiddenField.val(result);

			//check for file name 
			var pictureUploadField = form["" + fieldName]; //actual upload to check for filename

			var realFileNameField = form["RealFileName_" + fieldName];
			if (pictureUploadField) {
				//remove fakepath from filename
				var realFilename = pictureUploadField.value;
				realFilename = realFilename.replace("C:\\fakepath\\", "");

				if (realFileNameField) {
					realFileNameField.value = realFilename;
				}
			}

			//tiny mce
			if (svyIsMce()) {
				var s = document.getElementById('src');
				var aSrc = websiteBaseUrl + "attachments/" + result;
				s.value = aSrc;
			} else { //not MCE
				var nonMCEFileUploadField = $("#attachmentFileName_" + fieldName);
				if (nonMCEFileUploadField.length > 0) {
					nonMCEFileUploadField.val(result);
				}
			}
			$("#svyImagePasteProgress_" + fieldName).css("width", "140px"); //restore the width
			if (!svyIsDragAndDropable()) {
				$("#svyPasteAgainImageLink_" + fieldName).show();
			}

			$(".svyFiledragProgess p").css("color", "green");
			$(pTag).html("Completed");
			$(spanTag).animate({ "width": "100%" });
			$(window).trigger('resize');   // do this so that sticky footers will be re-drawn - if there is one.
			svyRemovePictureClasses(fieldName);
			pictureUploadField.value = '';   // MN 20141221- dont post the file twice, it is already posted by ajax!
		},
		xhr: function () {
			var xhr = jQuery.ajaxSettings.xhr();
			if (xhr.upload) {
				var pc = 0;
				$(pTag).html("uploading... ");
				$(spanTag).css("width", "0");

				xhr.upload.onprogress = function (e) {
					if (e.lengthComputable) {
						pc = (e.loaded / e.total) * 100;
						//progressBar.textContent = progressBar.value; // Fallback for unsupported browsers.
						$(spanTag).css({ "width": pc + "%" })
					}
				}
			}
			return xhr;
		},
		complete: function () {
			if (window.svyOnAfterUpload) {
				window.svyOnAfterUpload(form, fieldName);
			}
		},
		error: function (result) {
			$(pTag).html("Failed");
			$(".svyFiledragProgess span").css("background", "red");
			$("#svyImagePasteSpan_" + fieldName).addClass("svyUploadError");
		},
		traditional: true
	});
}

// file selection
function svyFileDropHandler(e) {
	var targetId = e.target.DragLiveId;
	// cancel event and hover styling
	var targetEl = e.target.tagName.toLowerCase();

	if (targetEl == 'img' || targetEl == 'p' || targetEl == 'div') { //if the drag is over a image reset the dragarea to the parent
		targetId = e.currentTarget.DragLiveId;
	}

	//there should never be a css of fancyBackground here (because its a drop not a paste) but doesnt hurt to check.. 
	if ($("#" + targetId).hasClass("fancyBackground")) {
		$("#" + targetId).removeClass("fancyBackground");
	}

	svyFileDragHover(e);
	// fetch FileList object
	var files = e.target.files || e.dataTransfer.files;
	// process all File objects

	if (files.length > 1) {
		alert("Please only drag one file at a time.");
		e.stopPropagation();
		return;
	}
	if(!window.FileReader)return ; //not supported
	for (var i = 0, f; f = files[i]; i++) {
		var reader = new FileReader();
		if (e.currentTarget.id.toLowerCase().indexOf('picture') > -1) {
			if (f.type.indexOf("image") === -1) {
				var fileId = targetId.replace("svyFiledragLive_", "file_");
				$("#" + fileId).val("");
				alert("Please only drag images.");
				var fieldName = targetId.replace("svyFiledragLive_", "");
				svyCancel(fieldName);
				e.stopPropagation();
				break;
				return;
			}
		}

		// Closure to capture the file information (after readAsDataUrl completes).
		reader.onload = (function (theFile) {
			return function (e) {
				var base64Str = e.target.result;
				var fieldName = targetId.replace("svyFiledragLive_", "");
				svyProcessNewImage(fieldName, base64Str, theFile.name, "svyFiledragLive_" + fieldName);
			};
		})(f);
		// Read in the image file as a data URL.
		reader.readAsDataURL(f);
	}
}

// file drag hover
function svyFileDragHover(e) {
	e.stopPropagation();
	e.preventDefault();
}

// initialize
function svyInitHtml(fieldName) {
	var fs = "file_" + fieldName;

	var fileselect = $id(fs);
	if (!svyIsDragAndDropable()) return;    // for old browsers IE8 or less, just forget trying to attach a drag handler
	fileselect.DragLiveId = "svyFiledragLive_" + fieldName;
	fileselect.ProgressId = "svyImagePasteProgress_" + fieldName;
	fileselect.HiddenId = "paste_" + fieldName;

	if (IsDragAndPastEnabled(fieldName)) {
		var filedrag = $id("svyFiledragLive_" + fieldName);
		filedrag.ProgressId = "svyImagePasteProgress_" + fieldName;
		filedrag.DragLiveId = "svyFiledragLive_" + fieldName;
		filedrag.HiddenId = "paste_" + fieldName;
	}

	// file select
	fileselect.addEventListener("change", svyFileDropHandler, false);
	// is XHR2 available?
	var xhr = new XMLHttpRequest();
	if (xhr.upload) { // just means can upload
		// file drop
		if (IsDragAndPastEnabled(fieldName)) {
			filedrag.addEventListener("dragover", svyFileDragHover, false);
			filedrag.addEventListener("dragleave", svyFileDragHover, false);
			filedrag.addEventListener("drop", svyFileDropHandler, false);
		}
	}
}

function svyRemoveAllHtmlPaste(fieldName) {
	$("#ulc_" + fieldName).find(".svyFiledragLive").removeClass("svyFancyBackground");
	$("#ulc_" + fieldName).find(".svyFiledragLive").removeClass("svyPasteArea");
	//$("#ulc_" + fieldName).find(".svyFiledragLive").html("<div class='svyPasteAreaText'><p id = 'pText" + id + "'></small></p></div>");
}

function svyCancelHtml5Paste(fieldName) {
	$("#svyFiledragLive_" + fieldName).removeAttr("style");
	$("#svyFiledragLive_" + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyFiledragLive").removeClass("svyFancyBackground");
	$("#ulc_" + fieldName).find(".svyFiledragLive").children("p").remove();
	$("#ulc_" + fieldName).find(".svyFiledragProgess").hide();

	svyResetAfterCancelAction(fieldName);
	return false;
}

function svyDeactivateHtml5Paste(element) {
	window.removeEventListener("paste", svyPasteProcessEvent, false);
}

function svyRemovePasteBackGround(fieldName) {
	$("#svyFiledragLive_" + fieldName).removeClass("svyPasteArea");
}

function svyShowPaste(fieldName) {
	$("#file_" + fieldName).hide();
	var el = $("#svyFiledragLive_" + fieldName);
	//$('#svyFiledragLive_' + fieldName).removeClass("noBackground");
	if ($(el).find("img").length > 0) {
		var fileCntrl = $("#file_" + fieldName).val();
		if (fileCntrl != "") {
			return false;
		}
	}
	
	var dropTargetLive = el;
	$(dropTargetLive).show();
	svyCopyPasteDiv = "svyFiledragLive_" + fieldName;
	$("#" + svyCopyPasteDiv).focus();

	$('#filedim_' + fieldName).hide();
	$("#ulc_" + fieldName).find(".svyLinkContainer").hide();
	$('#svyCancelCustomeBrowseLink_' + fieldName).show();

	if (IsSvyHtml5Paste()) {
		$(dropTargetLive).html("<div class='svyPasteAreaText svyPasteText'><p>Paste (control-v)</p></div>");

		svyBlink(dropTargetLive);

		$(dropTargetLive).addClass("svyPasteArea");
/*		$(dropTargetLive).addClass("svyFancyBackgroundActive");
		$(dropTargetLive).find("p").css("color", "white");*/
		var wrapper = document.getElementById("ulc_" + fieldName);
		svyActivateHtml5Paste(wrapper);
	} else {
		svyHandleImagePasteSelect(fieldName)
		$("#svyCancelPasteImageLink_" + fieldName).hide();
	}
}

function svyActivateHtml5Paste(element) {
	if (!element.addEventListener) {
		return;
	}
	$(".svyFiledragLive").removeClass("fancyBackgroundActive");

	$(".svyFiledragLive").each(function () {

		if ($(this).find("img").length > 0) {
			//do something
		} else {
			// do something else
			$(this).addClass("svyFancyBackground");
		}
	});


	window.addEventListener("paste", svyPasteProcessEvent);
	$(".svyFiledragLive", element).addClass("fancyBackgroundActive");
}

function svyBlur(element) {
	//console.log("svyBlur")
	$(".svyFiledragLive", element).removeClass("fancyBackgroundActive");
	$(".svyFiledragLive", element).addClass("fancyBackground");
}

function svyFocus(element) {
	//svyDeactivatesvyHtml5Paste(element);.
	$(".svyFiledragLive", element).addClass("svyFancyBackgroundActive");
	$(".svyFiledragLive", element).removeClass("svyFancyBackground");
}

function svyChangePictureInit(fieldName) {
	$("#shp_" + fieldName).hide(200, function () {
		$("#ulc_" + fieldName).show(200);
		$("#svyRemovePictureLink_" + fieldName).show(200);
		return false
	});
}

//blinks text!
function svyBlink(selector) {
	var pTag = $(selector).find("p");
	$(pTag).stop().fadeOut(499, function () {
		$(this).fadeIn(500, function () {
			if ($(selector).hasClass("svyFancyBackground") || $(selector).hasClass("svyFancyBackgroundActive")) {
				svyBlink(selector);
			}
		});
	});
}

function svyPasteAndUpload(event, fieldName) {
	event.stopPropagation();
	return false;
}

var svyCropScriptLoaded = false;
function svyCropPicture(btnObj, fieldName, base64Str, fileName, targetId) {
	$('.svyCropDlg').remove();//kill others

	var parent = $('#file_' + fieldName + '').closest('.svyPictureContainer');


	var frameWidth = 450;
	var frameHeight = 460;

	svyShowUploadImage(base64Str, targetId, false);

	var dlgTop = 0;
	var dlgLeft = 0;
	//var html = $('head').html();
	//var scriptName='<script src="'+websiteBaseUrl+'js/tapmodo-jcrop/js/jquery.jcrop.min.js"></script>';
	var scriptName = '' + websiteBaseUrl + 'js/tapmodo-jcrop/js/jquery.jcrop.min.js';
	//if (html.toLowerCase().indexOf('tapmodo-jcrop') == -1) {
	if (!svyCropScriptLoaded) {
		//$('head').insertAfter(scriptName);
		$.getScript(scriptName, function () {
			svyCropScriptLoaded = true;
			//alert("Running test.js");
		});
	}
	var cropeditorUrl = websiteBaseUrl + 'js/tapmodo-Jcrop/svyCropEdit.aspx?file=' + fileName + '&fieldName=' + fieldName + '&targetID=' + targetId;
	var frameFrag = '<iframe id="svyCrop_' + fieldName + '_Frame"  width="100%" height="440px" frameborder="0" scrolling="no" src="' + cropeditorUrl + '"></iframe>';
	var btnHtml = '';
	btnHtml += '<span style="float:right"><a href="" onclick="svyCropClose(this);svyCancel(\'' + fieldName + '\');return false;">cancel</a></span>';
	//btnHtml += '<span style="float:right"><a href="" onclick="return svyCropSave(this)">crop</a> | </span>';
	var dialogFrame = '<div class="svyCropDlg" id="svyCrop_' + fieldName + '" style="position:absolute;background-color:white;width: 500px;height:460px;border: 10px dotted #ccc;padding: 10px;z-index: 999999;">' + btnHtml + '<br>' + frameFrag + '</div>';
	parent.prepend(dialogFrame);

	//cancel styles
	$(".svyFiledragLive_" + fieldName).removeClass("svyPasteArea");
	return false;
}
function svyCropClose(btnObj) {
	$(btnObj).parent().parent().remove();
	return false;
}
function svyCropSave(btnObj) {
	alert('inactive');
	return false;
}
function svyCropSaveDone(fieldName) {
	//svyCropClose();
	//alert('close frame');
	$('#svyCrop_' + fieldName).remove();
	return false;
}

function svyGetImagefromHTML5Preview(fieldName) {
	return $('#svyFiledragLive_' + fieldName + ' img').attr('src');
}

function svyChangePicture(fieldName) {
	$("#shp_" + fieldName).hide();
	svyResetAfterCancelAction(fieldName);
	$("#cancelPicture_" + fieldName).show();
}

// JC 20140630 - removed mce window code
//EditableNumberField cell in list view
function ip(obj, mode) {			 //input handler
	//alert('bla');
	//console.log(event.keyCode);
	var resetValue = $(obj).val();

	if ((event.keyCode == 13) || (event.keyCode == 27) || (event.keyCode == 9)) {//enter or escape, or tab

		var isAjaxOK = false;
		var tn = $(obj).attr("data-tn");				//	 tn 
		var rid = $(obj).attr("data-rid");			//	 rid
		var cn = $(obj).attr("data-cn");				//	 cn 
		var vl = $(obj).val();								//	 vl
		if (event.keyCode != 27) {				//check for not- escape

			//do ajax
			var qs = "tn=" + escape(tn) + "&rid=" + escape(rid) + "&cn=" + escape(cn) + "&vl=" + escape(vl) + "";
			var url = websiteBaseUrl + "Save/";
			$.ajax({
				type: "POST",
				url: url,
				data: qs,
				async: false,
				success: function (msg) {
					if (msg.indexOf("OK") != -1) {
						isAjaxOK = true;
						//alert('exec ok: ' + msg);
						//prompt('copy this',url+'?'+qs)
					}
				},
				error: function (msg) {
					if (window.console) console.log("call failed: " + msg.responseText);
					//prompt('copy this',url+'?'+qs)
				}
			});
		} else {
			//only if escape was pressed (27)
			resetValue = $(obj).attr("data-ov");
			isAjaxOK = true; //was not run, so ok
		}
		if (isAjaxOK) {
			$(obj).parent().html('<div data-tn="' + tn + '" data-rid="' + rid + '" data-cn="' + cn + '"  data-ov="' + resetValue + '" class="edfield" onclick="return dv(this,\'' + mode + '\')\" style="color:green;cursor:pointer">' + resetValue + '</div>');
		} else {
			$(obj).parent().html('<div data-tn="' + tn + '" data-rid="' + rid + '" data-cn="' + cn + '" data-ov="' + resetValue + '" class="edfield" onclick="return resetOV(this,\'' + mode + '\')\" style="color:red;cursor:pointer">save failed</div>');
		}
	}
	if (mode == 'text' || event.keyCode == 46 || event.keyCode == 8 //del /backspace
		|| event.keyCode == 35 || event.keyCode == 36 //home/end						 
		|| event.keyCode == 39 || event.keyCode == 37 //left/right						 
		|| event.keyCode == 189 //negative	'-'
		|| event.keyCode == 35 || event.keyCode == 40 || event.keyCode == 34 || event.keyCode == 37 || event.keyCode == 12 || event.keyCode == 39 || event.keyCode == 36 || event.keyCode == 38 || event.keyCode == 33 //numeric keypad ok
	) {
		return true;
	}
	if (mode == 'money' && (((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 190) || ((event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 110))) { 
		return true;
	}
	if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105))) { //outside of zero to 9 range
		return false;					 //cancel the keydown event
	}
	if (resetValue.length >= 6) {	 //text is too long
		return false;					 //cancel the keydown event
	}
	return true;
};
function resetOV(obj, mode) {						 //div handler
	var jqObj = $(obj);
	jqObj.css('color', 'black');
	jqObj.html(jqObj.attr('data-ov'));
	jqObj.attr('onclick', 'return dv(this,\'' + mode + '\')');
};
function dv(obj, mode) {						 //div handler
	var tn = $(obj).attr("data-tn");
	var cn = $(obj).attr("data-cn");
	var ov = $(obj).attr("data-ov");
	var rid = $(obj).attr('data-rid');
	if (mode == 'dropbox') {
		var dropboxObj = $('.dyn.drop[data-rid=\'' + rid + '\']');
		$(dropboxObj).show();
		$(obj).hide();
		$(dropboxObj).find('select').on('change', function () {
			//ajax save
			var isAjaxOK = false;
			var saveValue = $(this).val();								//	 vl
			var displayValue = $('#MerchandiseSizeID option:selected').text();								//	 vl

			//do ajax
			var qs = "tn=" + escape(tn) + "&rid=" + escape(rid) + "&cn=" + escape(cn) + "&vl=" + escape(saveValue) + "";
			var url = websiteBaseUrl + "Admin/Save/";
			$.ajax({
				type: "POST",
				url: url,
				data: qs,
				async: false,
				success: function (msg) {
					if (msg.indexOf("OK") != -1) {
						isAjaxOK = true;
					}
				},
				error: function (msg) {
					if (window.console) console.log("call failed: " + msg.responseText);
					//prompt('copy this',url+'?'+qs)
				}
			});
			if (isAjaxOK) {
				$(dropboxObj).hide();
				$(obj).html(displayValue);
				$(obj).show();
			} else {
				$(obj).parent().html('<div data-tn="' + tn + '" data-rid="' + rid + '" data-cn="' + cn + '" data-ov="' + resetValue + '" onclick="return resetOV(this,\'' + mode + '\')\" style="color:red;cursor:pointer">save failed</div>');
			}
		}).focus();;
	} else {
		var vl = $(obj).html();
		var extrahtml = '';
		if (mode == 'money') {
			extrahtml = '$'
		}
		var newInput = $(obj).parent().html(extrahtml+'<input type="text" style="width:52px;" value="' + vl + '" data-tn="' + tn + '" data-ov="' + ov + '" data-cn="' + cn + '" data-rid="' + rid + '" onkeydown="return ip(this,\''+mode+'\')">');
		//newInput is the TD, with an input in it.
		$(newInput).find('input').focus().select();
	}
}

//end of EditableNumberField cell in list view

var iframeUploaderReceivedResponse = false;

function uploadUsingIframe(fieldName) {

	var $element = $('#file_' + fieldName);

	if ($element.val() == '') return;

	$element.hide();

	var progress = $('#svyImagePasteProgress_' + fieldName);

	progress.show();
	progress.find('p').text('uploading...');
	progress.find('span').css("width", "0");

	var originalForm = $element.closest('form');

	// Create a temp form to handle this submit
	originalForm.after('<form id="tempIframeUploaderForm"></form>');

	// Move the file input element from one form to the other
	$element = $element.detach();
	$('#tempIframeUploaderForm').append($element);

	var form = $('#tempIframeUploaderForm')[0];

	// Create the iframe...
	var iframe = document.createElement("iframe");
	iframe.setAttribute("id", "upload_iframe_" + fieldName);
	iframe.setAttribute("name", "upload_iframe_" + fieldName);
	iframe.setAttribute("width", "0");
	iframe.setAttribute("height", "0");
	iframe.setAttribute("border", "0");
	iframe.setAttribute("style", "width: 0; height: 0; border: none;");

	// Add to document...
	form.parentNode.appendChild(iframe);
	window.frames['upload_iframe_' + fieldName].name = "upload_iframe_" + fieldName;

	iframeId = document.getElementById("upload_iframe_" + fieldName);

	// Add event...
	var eventHandler = function () {

		if (iframeId.detachEvent) iframeId.detachEvent("onload", eventHandler);
		else iframeId.removeEventListener("load", eventHandler, false);

		// Prevent from executing twice in IE9
		if (iframeUploaderReceivedResponse) {
			return;
		}

		iframeUploaderReceivedResponse = true;

		var response = '';

		// Message from server...
		if (iframeId.contentDocument) {
			response = iframeId.contentDocument.body.innerHTML;
		} else if (iframeId.contentWindow) {
			response = iframeId.contentWindow.document.body.innerHTML;
		} else if (iframeId.document) {
			response = iframeId.document.body.innerHTML;
		}

		var obj = $.parseJSON(response);

		// Move the file input element back to the original form
		$element = $element.detach();
		originalForm.find('#ulc_' + fieldName).prepend($element);
		//$element.show();

		if (obj && obj.success) {

			progress.find('p').text('Completed').css("color", "green");
			progress.find('span').animate({ "width": "100%" });
			progress.find('.svyPasteHidden').attr('value', obj.filepath);

			var fileName = websiteBaseUrl + 'attachments/' + obj.filepath;

			$('#svyFiledragLive_' + fieldName).html('<img src="' + fileName + '" />').show();

			if (svyIsMce()) {
				var s = document.getElementById('src');
				var aSrc = fileName;
				s.value = aSrc;
			}

			// Del the iframe...
			setTimeout(function () {
				$('#svyIframeUploaderStatus').remove();
				$('#tempIframeUploaderForm').remove();
				$(iframeId).remove();
			}, 250);

		}

	};

	if (iframeId.addEventListener) iframeId.addEventListener("load", eventHandler, true);
	if (iframeId.attachEvent) iframeId.attachEvent("onload", eventHandler);

	// Set properties of form...
	form.setAttribute("target", "upload_iframe_" + fieldName);
	form.setAttribute("action", websiteBaseUrl + 'services/GetClipboardPicture.aspx?uploadType=iframe');
	form.setAttribute("method", "post");
	form.setAttribute("enctype", "multipart/form-data");
	form.setAttribute("encoding", "multipart/form-data");

	// Reset the flag
	iframeUploaderReceivedResponse = false;

	// Submit the form...
	form.submit();
};

// AF20150304 Not being used anymore as the input field is now sitting on top of the button
//function openFileBroswer(el) {
//	$("#file_" + el).show();				//safari doesnt 'click' if not visible
//	$("#file_" + el).click();
//	$("#file_" + el).hide();			 	//safari doesnt 'click' if not visible - hide after 'click'
//}