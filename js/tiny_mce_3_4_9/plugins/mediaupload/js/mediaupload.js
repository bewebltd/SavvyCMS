/* Functions for the advlink plugin popup */

tinyMCEPopup.requireLangPack();

var templates = {
	"window.open" : "window.open('${url}','${target}','${options}')"
};

function preinit() {
}

function changeClass() {
	var f = document.forms[0];

	f.classes.value = getSelectValue(f, 'classlist');
}

function init() {
	tinyMCEPopup.resizeToInnerSize();

	var formObj = document.forms[0];
	var inst = tinyMCEPopup.editor;
	var elm = inst.selection.getNode();
	var action = "insert";
	var html;


	elm = inst.dom.getParent(elm, "A");
	if (elm != null && elm.nodeName == "A") {
		action = "update";
		formObj.insert.value = "Unlink"

		var href = inst.dom.getAttrib(elm, 'href');

		// Setup form data
		setFormValue('href', href);
		//setFormValue('title', inst.dom.getAttrib(elm, 'title'));
		//setFormValue('id', inst.dom.getAttrib(elm, 'id'));

		if (href + "" != "") {
			if (document.getElementById("DeleteFilename")) {
				var lastSegPos = href.lastIndexOf("/")
				if (lastSegPos > -1) {
					var filename = href.substr(lastSegPos + 1)
					document.getElementById("DeleteFilename").innerHTML = filename
					document.getElementById("delete_panel").style.display = ""
					document.getElementById("upload_panel").style.display = "none"
					document.getElementById("delete").style.display = ""
					document.getElementById("unlink").style.display = ""
					document.getElementById("insert").style.display = "none"
				}
			} else {
				document.getElementById("delete").style.display = "none"
				document.getElementById("unlink").style.display = ""
				document.getElementById("insert").style.display = "none"
			}
		}

	}
}

function checkPrefix(n) {
	if (n.value && Validator.isEmail(n) && !/^\s*mailto:/i.test(n.value) && confirm(tinyMCEPopup.getLang('advlink_dlg.is_email')))
		n.value = 'mailto:' + n.value;

	if (/^\s*www\./i.test(n.value) && confirm(tinyMCEPopup.getLang('advlink_dlg.is_external')))
		n.value = 'http://' + n.value;
}

function setFormValue(name, value) {
	document.forms[0].elements[name].value = value;
}


function getOption(opts, name) {
	return typeof(opts[name]) == "undefined" ? "" : opts[name];
}


function parseOptions(opts) {
	if (opts == null || opts == "")
		return [];

	// Cleanup the options
	opts = opts.toLowerCase();
	opts = opts.replace(/;/g, ",");
	opts = opts.replace(/[^0-9a-z=,]/g, "");

	var optionChunks = opts.split(',');
	var options = [];

	for (var i=0; i<optionChunks.length; i++) {
		var parts = optionChunks[i].split('=');

		if (parts.length == 2)
			options[parts[0]] = parts[1];
	}

	return options;
}


function setAttrib(elm, attrib, value) {
	var formObj = document.forms[0];
	var valueElm = formObj.elements[attrib.toLowerCase()];
	var dom = tinyMCEPopup.editor.dom;

	if (typeof(value) == "undefined" || value == null) {
		value = "";

		if (valueElm)
			value = valueElm.value;
	}

	// Clean up the style
	if (attrib == 'style')
		value = dom.serializeStyle(dom.parseStyle(value));

	dom.setAttrib(elm, attrib, value);
}

function removeLink() {
	var inst = tinyMCEPopup.editor;

	tinyMCEPopup.execCommand("mceBeginUndoLevel");

	inst.getDoc().execCommand("unlink", false, null);

	tinyMCEPopup.execCommand("mceEndUndoLevel");
	tinyMCEPopup.close();
}

function insertAction() {
	var inst = tinyMCEPopup.editor;
	var elm, elementArray, i;
	var ed = tinyMCEPopup.editor

	elm = inst.selection.getNode();
	checkPrefix(document.forms[0].href);

	elm = inst.dom.getParent(elm, "A");
	
	// Remove element if there is no href
	if (!document.forms[0].href.value) {
		tinyMCEPopup.execCommand("mceBeginUndoLevel");
		i = inst.selection.getBookmark();
		inst.dom.remove(elm, 1);
		inst.selection.moveToBookmark(i);
		tinyMCEPopup.execCommand("mceEndUndoLevel");
		tinyMCEPopup.close();
		return;
	}

	tinyMCEPopup.execCommand("mceBeginUndoLevel");
	
	// Create new anchor elements
	if (elm == null) {
		if (inst.selection.isCollapsed()) {
			// nothing is selected - so insert html for a link
			elm = null			
		} else {
			inst.getDoc().execCommand("unlink", false, null);
			tinyMCEPopup.execCommand("CreateLink", false, "#mce_temp_url#", { skip_undo: 1 });

			elementArray = tinymce.grep(inst.dom.select("a"), function (n) { return inst.dom.getAttrib(n, 'href') == '#mce_temp_url#'; });
			if (elementArray.length > 0) {
				for (i = 0; i < elementArray.length; i++) {
					setAllAttribs(elm = elementArray[i]);
				}
			} else {
				// nothing is selected or can't create a link using execCommand - so insert html for a link
				elm = null
			}
		}

		if (elm == null) {
			// nothing is selected - so insert html for a link
			var formObj = document.forms[0];
			var href = formObj.href.value;
			var html = '<a href="' + href + '" target="_blank">Download Document</a>'
			ed.execCommand("mceInsertContent", false, html);
		}

	} else {
		setAllAttribs(elm);
	}

	// Don't move caret if selection was image
	if (elm!=null && (elm.childNodes.length != 1 || elm.firstChild.nodeName != 'IMG')) {
		inst.focus();
		inst.selection.select(elm);
		inst.selection.collapse(0);
		tinyMCEPopup.storeSelection();
	}

	tinyMCEPopup.execCommand("mceEndUndoLevel");
	tinyMCEPopup.close();
}

function setAllAttribs(elm) {
	var formObj = document.forms[0];
	var href = formObj.href.value;
	var target = "_blank"     // getSelectValue(formObj, 'targetlist');

	setAttrib(elm, 'href', href);
	setAttrib(elm, 'title');
	setAttrib(elm, 'target', target == '_self' ? '' : target);
	setAttrib(elm, 'id');
	setAttrib(elm, 'style');
	setAttrib(elm, 'class', getSelectValue(formObj, 'classlist'));
	setAttrib(elm, 'rel');
	setAttrib(elm, 'rev');
	setAttrib(elm, 'charset');
	setAttrib(elm, 'hreflang');
	setAttrib(elm, 'dir');
	setAttrib(elm, 'lang');
	setAttrib(elm, 'tabindex');
	setAttrib(elm, 'accesskey');
	setAttrib(elm, 'type');
	setAttrib(elm, 'onfocus');
	setAttrib(elm, 'onblur');
	setAttrib(elm, 'onclick');
	setAttrib(elm, 'ondblclick');
	setAttrib(elm, 'onmousedown');
	setAttrib(elm, 'onmouseup');
	setAttrib(elm, 'onmouseover');
	setAttrib(elm, 'onmousemove');
	setAttrib(elm, 'onmouseout');
	setAttrib(elm, 'onkeypress');
	setAttrib(elm, 'onkeydown');
	setAttrib(elm, 'onkeyup');

	// Refresh in old MSIE
	if (tinyMCE.isMSIE5)
		elm.outerHTML = elm.outerHTML;
}

function getSelectValue(form_obj, field_name) {
	var elm = form_obj.elements[field_name];

	if (!elm || elm.options == null || elm.selectedIndex == -1)
		return "";

	return elm.options[elm.selectedIndex].value;
}

function getTargetListHTML(elm_id, target_form_element) {
	var targets = tinyMCEPopup.getParam('theme_advanced_link_targets', '').split(';');
	var html = '';

	html += '<select id="' + elm_id + '" name="' + elm_id + '" onf2ocus="tinyMCE.addSelectAccessibility(event, this, window);" onchange="this.form.' + target_form_element + '.value=';
	html += 'this.options[this.selectedIndex].value;">';
	html += '<option value="_self">' + tinyMCEPopup.getLang('advlink_dlg.target_same') + '</option>';
	html += '<option value="_blank">' + tinyMCEPopup.getLang('advlink_dlg.target_blank') + ' (_blank)</option>';
	html += '<option value="_parent">' + tinyMCEPopup.getLang('advlink_dlg.target_parent') + ' (_parent)</option>';
	html += '<option value="_top">' + tinyMCEPopup.getLang('advlink_dlg.target_top') + ' (_top)</option>';

	for (var i=0; i<targets.length; i++) {
		var key, value;

		if (targets[i] == "")
			continue;

		key = targets[i].split('=')[0];
		value = targets[i].split('=')[1];

		html += '<option value="' + key + '">' + value + ' (' + key + ')</option>';
	}

	html += '</select>';

	return html;
}

// While loading
preinit();
tinyMCEPopup.onInit.add(init);
