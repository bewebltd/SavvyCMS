//o.content
function PastePreProcess(content) {
	//content = content.replace(/\<br\>\<br\>/gi, '</P><P>');
	//content = '<P>'+content+'</P>';
	return content;
}
//o.node.innerHTML
function PastePostProcess(html) {
	//alert('post')
	if (html.indexOf('<img') != -1) {
		alert('' +
			'You have pasted an image in with your content. \n\n' +
			'This will be saved as a link to the image on the source site you got it from. Make sure it is located on a site you trust to always be there. \n\n' +
			'If you cant be sure, save the image to your computer, then use the \'Insert/Edit image\' button to replace the image in the content.' +
			'');
		//return ""; //like return false - cancel paste
	}

	/*eg 
	paste html with embedded images linked to other sites.
	
	<p><a href="http://www.leadlight.co.nz/index.php?cPath=28"><img src="/images/upload/image/gsjbar031-s.JPG" width="96" height="98" alt=""></a>&nbsp; &nbsp; &nbsp;&nbsp;<a href="http://www.leadlight.co.nz/index.php?cPath=300"><img src="/images/upload/image/bjsb1525m-s.JPG" width="96" height="72" alt=""></a>&nbsp; &nbsp; &nbsp; &nbsp; <a href="http://www.leadlight.co.nz/index.php?cPath=352"><img src="/images/upload/image/t16011-s.JPG" width="96" height="83" alt=""></a>&nbsp; &nbsp; &nbsp; &nbsp;<a href="http://www.leadlight.co.nz/index.php?cPath=200"><img src="/images/upload/image/gspoa6755sf-s.JPG" width="96" height="96" alt=""></a></p>
	
	regex to find images:
		 <img\s[^>]*?src\s*=\s*['\"]([^'\"]*?)['\"][^>]*?>

	how to find image base?
	get the base url of the href - if there is one, or ask the user?
	call service to save images, then rewrite the html to refer to the local images

	*/
	var findText = '<a href=';
	var posn = html.indexOf(findText);

	var pastedBaseURL = "";
	if (posn > -1) {
		//found href, extract base

		var posn2 = html.indexOf('>', posn + findText.length + 1); //close href
		pastedBaseURL = html.substring(posn + findText.length + 1, posn2 - 1);
		//remove path
		var posnLastSlash = pastedBaseURL.indexOf('/', 7);
		pastedBaseURL = pastedBaseURL.substring(0, posnLastSlash + 1);
	}

	//find images in html
	var re = new RegExp(/src\=([^\s]*)/gim);		// other src= things?		 images only?
	var imagesInHTML = html.match(re);

	if (imagesInHTML != null) {
		var s = "\n";
		for (i = 0; i < imagesInHTML.length; i++) {
			var findText = 'src="';
			var url = imagesInHTML[i].substring(findText.length, imagesInHTML[i].length - 1);
			s = s + url + "\n";
			imagesInHTML[i] = url;
		}
		//alert(s);
		//if (confirm('New option: Do you want to copy images (' + s + ') to this site?', +pastedBaseURL)) {
		//if (prompt('New option: Do you want to copy images (' + s + ') to this site?', +pastedBaseURL)) {
		if (confirm('New option: Do you want to copy the images (' + imagesInHTML.length + ') to this site?')) {
			//walk the images
			for (i = 0; i < imagesInHTML.length; i++) {
				url = imagesInHTML[i];
				if (url.indexOf('http') == 0) {								//starts with
					//if(!confirm(url))break;

					var qs = "fullImageUrl=" + escape(url) + "";
					var ajaxurl = websiteBaseUrl + "common/SaveRemoteImageLocally";
					$.ajax({
						type: "POST",
						url: ajaxurl,
						data: qs,
						dataType: "json",
						async: false, //this means wait for each result
						success: function (data) {
							html = html.replace(url, data.newImage);
						},
						error: function (msg) {
							//forget it - no replace

							//alert("call failed: " + msg.responseText);
							//prompt('copy this',url+'?'+qs)
						}
					});
				}
			}
		}
	} else {
		//no images in html
	}
	return TidyEditor(html);
	//return html
}

var global_pasteMCEEditor = null;
function SavvySmartPasteWatcher(ed, e) {
	//try clipboard - look for images

	global_pasteMCEEditor = ed;
	if (e.clipboardData) {
		var items = e.clipboardData.items;
		if (items) {
			for (var i = 0; i < items.length; i++) {
				if (items[i].type.indexOf("image") !== -1) {
					var file = items[i].getAsFile();
					//var URLObj = window.URL || window.webkitURL;
					var createObjectURL = (window.URL || window.webkitURL || {}).createObjectURL || function () { };
					var source = createObjectURL(file);

					//creates a url like this: blob:http%3A//localhost/bf1ee883-9b1f-43cb-8933-1e2758e3e67e which can be ajaxed down to the server
					// goes to your existing save image from URL and upload via ajax

					//option 2
					//file = clipboardData.items[i].getAsFile();
					var reader = new FileReader();
					reader.onload = function (evt) {
						SaveFileToServer(global_pasteMCEEditor, {
							dataURL: evt.target.result,
							event: evt,
							file: file,
							name: file.name
						});
					};
					reader.readAsDataURL(file);

					//or   reader.readAsArrayBuffer(file);
				}
			}
		} else {

			// browser does not support pasting images in blobs
		}
	}

}

function setCursorPosition(editor, index) {
	//get the content in the editor before we add the bookmark... 
	//use the format: html to strip out any existing meta tags
	var content = editor.getContent({ format: "html" });

	//split the content at the given index
	var part1 = content.substr(0, index);
	var part2 = content.substr(index);

	//create a bookmark... bookmark is an object with the id of the bookmark
	var bookmark = editor.selection.getBookmark(0);

	//this is a meta span tag that looks like the one the bookmark added... just make sure the ID is the same
	var positionString = '<span id="' + bookmark.id + '_start" data-mce-type="bookmark" data-mce-style="overflow:hidden;line-height:0px"></span>';
	//cram the position string inbetween the two parts of the content we got earlier
	var contentWithString = part1 + positionString + part2;

	//replace the content of the editor with the content with the special span
	//use format: raw so that the bookmark meta tag will remain in the content
	editor.setContent(contentWithString, ({ format: "raw" }));

	//move the cursor back to the bookmark
	//this will also strip out the bookmark metatag from the html
	editor.selection.moveToBookmark(bookmark);

	//return the bookmark just because
	return bookmark;
}


function getCursorPosition(editor) {
	//set a bookmark so we can return to the current position after we reset the content later
	var bm = editor.selection.getBookmark(0);

	//select the bookmark element
	var selector = "[data-mce-type=bookmark]";
	var bmElements = editor.dom.select(selector);

	//put the cursor in front of that element
	editor.selection.select(bmElements[0]);
	editor.selection.collapse();

	//add in my special span to get the index...
	//we won't be able to use the bookmark element for this because each browser will put id and class attributes in different orders.
	var elementID = "######cursor######";
	var positionString = '<span id="' + elementID + '"></span>';
	editor.selection.setContent(positionString);

	//get the content with the special span but without the bookmark meta tag
	var content = editor.getContent({ format: "html" });
	//find the index of the span we placed earlier
	var index = content.indexOf(positionString);

	//remove my special span from the content
	editor.dom.remove(elementID, false);

	//move back to the bookmark
	editor.selection.moveToBookmark(bm);

	return index;
}


function SaveFileToServer(ed, data) {
	//svyUploadBase64File()
	//svyPrepUploadBase64File(fieldName, base64Str, fileName, targetId);
	//svyPrepUploadBase64File('BodyTextHTML', /*base64Str*/ data.dataURL, 'autofilename', null);
	//var dateStamp = new XDate().toString('YYYYMMMddhhmmTT'); 
	var dateStamp = new Date();
	var editor = ed;
	var fileName = "inlinepasted-"+dateStamp.yyyymmddhhmmss()+"";
	var url = websiteBaseUrl + "services/GetClipboardPicture.aspx";
	var postData = { data: data.dataURL /*base64 data*/ };
	url += "?uploadType=mce&fileName=" + fileName;
	var jqxhr = $.ajax({
		type: "POST",
		url: url,
		data: postData,
		async: false,
		success: function (result) {					//todo: pass ed here?
			var newImage = websiteBaseUrl + "attachments/" + result;
			UpDateMCEEditorWithNewImage(this.ed, newImage);
		}
	});
}

function UpDateMCEEditorWithNewImage(ed, newImage) {
	//var cursorPosn = ed.selection.getRng().startOffset;
	ed = global_pasteMCEEditor;																 //todo remove hacky global note: ed is undefined here ? 
	var cursorPosn = getCursorPosition(ed);
	var currContent = ed.getContent();
	//var newImage = "<img src=\"http://www.beweb.co.nz/images/paypal-small.png\"";
	newImage = "<img src=\""+newImage+"\"";
	var newContent = currContent.substring(0, cursorPosn) + newImage + currContent.substring(cursorPosn + 1);
	ed.setContent(newContent);
}
//fix the mce past bug
function TidyEditor(v) {
	//alert('html tidy off')
	//return v;

	///this is to fix a problem found when pasting from word to chrome - mce creates a bad div called _mcepaste. This removes it.
	var p4 = /<div id="_mcePaste[^>]*>(?!<div>)([\s\S]*)<\/div>([\s\S]*)$/i;
	v = v.replace(p4, '<div>$1</div>');
	var p5 = /<div id="_mcePaste[^>]*>/gi;
	v = v.replace(p5, '<div>');
	//alert(v)
	return v;
}