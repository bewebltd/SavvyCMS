function ShowTemplate() {
	$(".section-template").hide();
	$(".detail-template").hide();
	$(".wide-template").hide();
	$(".subpanel-template").hide();
	$(".page-template").hide();
	$(".link-template").hide();
	$(".map-template").hide();
	$(".infometrics-template").hide();
	$(".special-template").hide();
	$(".products-template").hide();
	$(".resource-template").hide();
	$(".gallery-template").hide();
	var template = V$("TemplateCode").toLowerCase();
	$("." + template + "-template").show();
			
	// set appropriate fields to blank if not visible

	// set appropriate fields to No if not visible
	/*
	var form = document.forms["form"];
	if (!$("#ShowInMainNav_False").is(":visible") && form["ShowInMainNav"]) {
		form["ShowInMainNav"][1].checked = true;
	}*/
	//if (!$("#ShowInSecondaryNav_False").is(":visible") && form["ShowInSecondaryNav"]) {
	//	form["ShowInSecondaryNav"][1].checked = true;
	//}
	//if (!$("#ShowInFooterNav_False").is(":visible") && form["ShowInFooterNav"]) {
	//	form["ShowInFooterNav"][1].checked = true;
	//}
	/*
	if (!$("#LinkUrlIsExternal_False").is(":visible") && form["LinkUrlIsExternal"]) {
		form["LinkUrlIsExternal"][1].checked = true;
	}*/
			
}

function ShowQR() {
	$.fn.colorbox({ inline: true, href: "#qr", opacity: 0.7 });
	return false;
}

function GetSortOrder() {
	var selId = $("#ParentPageID option:selected").val();
	var qs = "";
	$.ajax({
		type: "POST",
		url: "../PageAdmin/GetSortOrder?id=" + selId,
		data: qs,
		success: function (msg) {
			$("#SortPosition").val(msg);
		},
		error: function (msg) {

		}
	});
}

	function textboxMultilineMaxNumber(txt,maxLen)
	{
		try{
			if(txt.value.length > (maxLen-1)) {
				var cont = txt.value;
				txt.value = cont.substring(0,(maxLen -1));
				return false;
			};
		}catch(e){
		}
	}
	
	/*
function loadPreview(url)
{
	//var previewAddress = document.getElementById('previewAddress');
	//var url = $('#previewAddress').val();
	var previewFrame = document.getElementById('fraContents');
	//prompt('url',url) 
	$(previewFrame).attr('src',url);
}*/

var isLoadingView = false;

//called by the revision frame
function loadRevisionView(url) {
	if (url == null || isLoadingView)
		return;

	isLoadingView = true;
	
	var previewFrame = document.getElementById('fraContents');
	previewFrame.src = url;
	$tabs.tabs('select', 1);//show preview

	setTimeout(function() {
		isLoadingView = false;
	}, 2000);
}

function loadRevisionEdit(url) {
	window.location = url;
}

function loadRevisionChanges(url) {
	var el = $("#revisionChanges");
	el.html('<iframe src="' + url + '" frameborder="0"></iframe>');
	el.dialog({
		title: 'Revision comparison',
		width: '80%',
		height: ($(window).height() * 0.8),
		closeOnEscape: true
	});
}
  
//function loadRevisionItems(historyID)
//{
//	//alert(tinyMCE)
//	$tabs.tabs('select',0);//show edit tab
//	//tinyMCE.activeEditor.setContent('<span>some</span> html');
		
//	//todo: not complete
//	//document.getElementById('ctl00_BodyContent_EditFV_Title').value='hey';
//}