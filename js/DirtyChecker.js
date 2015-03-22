var origValues = new Object();
var debug = false;
var checkForPageDirty = true;
$(document).ready(function() {
	buildValuesCache();

	// AF20140808: Some buttons are being added by JS, so it's safer to give it a time before applying the listeners
	setTimeout(function () {
		//list of clicks that dont need a dirty check
		$("#form .svySaveButton, .cancel, .delete").click(function (event) {
			checkForPageDirty = false;
		});
	}, 1000);

	//ie only?
	window.onbeforeunload = function () {
		if (checkForPageDirty&&stopIfDirty()) {
			return 'Continue? If you do, any changes may not be saved. This can lead to an incomplete record being created.';
		}
		return ''; //nothing

	};
});

function rebuildValuesForElement(ele) {
	var name = $(ele).attr("name");
	if(!name)return ;
	if (debug) alert("Building values cache for element: " + name);
	if ($(ele).is(":checkbox")) {
		if ($(ele).is(":checked")) {
			var val = "";
			if (origValues[name] != null) {
				val = origValues[name].val;
			}

			val = (val == '') ? $(ele).attr("value") : val + '|' + $(ele).attr("value");
			origValues[name] = { type: 'cb', val: val };
		} else {
			if (origValues[name] == null) {
				origValues[name] = { type: 'cb', val: '' };
			}
		}
	} else if ($(ele).is(":radio")) {
		if ($(ele).is(":checked")) {
			origValues[name] = { type: 'rb', val: $(ele).attr("value") };
		} else {
			if (origValues[name] == null) {
				origValues[name] = { type: 'rb', val: '' };
			}
		}
	} else {
		try {
			if (!$(ele).is(":submit") && name.substr(0, 2) != "__" && name != '' && name != 'df_returnpage' && !$(ele).hasClass("ignore_dirty")) {
				origValues[name] = { type: 'df', val: $(ele).val() };
			}
		} catch (e) { }
	}
}

function buildValuesCache() {
	$("#form input, #form textarea, #form select").each(function(i) {
		rebuildValuesForElement(this);
	});

	if (debug == true) {
		var db = "";
		for (key in origValues) {
			db += key + ": " + origValues[key] + "\n";
		}

		alert(db);
	}
}

function stopIfDirty() {
	var dirty = false;
	for (key in origValues) {
		var item = origValues[key];
		if (item.type == "cb") {
			var val = "";
			$("input[name='" + key + "']:checked").each(function(i) {
				val += '|' + $(this).attr("value");
			});

			if (item.val != val) {
				if (debug) alert("Dirty element " + key + " found. Original value: " + item.val + "; Current value: " + val);
				dirty = true;
				break;
			}
		} else if (item.type == 'rb') {
			var inp = $("input[name='" + key + "']:checked");
			var val = (inp.length > 0) ? inp.attr("value") : "";
			if (item.val != val) {
				if (debug) alert("Dirty element " + key + " found. Original value: " + item.val + "; Current value: " + val);
				dirty = true;
				break;
			}
		} else {
			var inp = $(":input[name='" + key + "']");
			if (item.val != inp.val()) {
				if (debug) alert("Dirty element " + key + " found. Original value: " + item.val + "; Current value: " + inp.val());
				dirty = true;
				break;
			}
		}
	}

	if (dirty) {
		//return !confirm("You have unsaved changes.. are you sure you want to leave this page?");
	}

	//return false;
	return dirty;
}