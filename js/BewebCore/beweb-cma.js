//beweb content management system routines
// copyright 2000-2015 beweb limited
//---------------------------------------------------------------------------
// revision history
// 03 dec 2014 - df_Callback now accepts a function as first param, which will be queued and guaranteed to complete before submitting; made cf_onchange return false work
// 11 jul 2013 - when getting/setting radio or checkbox value if passed a single field that is part of a group, assume we mean the group
// 19 mar 2013 - multiple forms - document.form no longer needed as functions find the form of the supplied field
// 20 jan 2012 - fixed accidental conversion of null to "null" in GetField
// 19 Jan 2012 - added cf_OnAddRow[row] to df_Addrow. It is called before the row is actually added.
// 02 oct 2011 - merged .net codelibmvc and pengellys versions
// 14 jun 2011 - added option df_autoSelectOnFocus and defaults to not automatically selecting field onfocus (due to chrome bug); don't use new row fadein/delete fadeout on IE
// 08 jun 2011 - use jQuery for get fields in all rows, field above/below; Chrome fixes; improved dirty checking with window.df_warnNotSaved
// 31 may 2011 - added option df_useJQueryClone to try to maintain attached events as well as supporting suffix string replacement
// 18 oct 2010 - sync between Pengellys, ASP.NET MVC, Models Forms and Classic ASP
// 03 may 2010 - sync between ASP.NET MVC, Models Forms and Classic ASP
// 10 feb 2009 - compatibility with jQuery: renamed $ function to E$, also has $ if jQuery is not included
// 15 oct 2008 - df_GetValue and df_SetValue now allow field name as param, num field eval is only called if not blank
// 30 may 2008 - added URL fields
// 26 jun 2008 - MK added cf_OnClick
// 21 may 2008 - MN fixed callback queuing bug
// 29 mar 2008 - added GetValue functions for checkbox and radio buttons
// 27 mar 2008 - added cf_OnBlur and cf_OnFocus, added dontPromptUser to deleterow
// 18 mar 2008 - does not set df_fieldlist if field is not there
// 18 feb 2008 - Callback: now calls BeforeSubmit (to support file upload components)
// 26 sep 2007 - firefox compat - added 'event' on event handlers, other ff fixes, added ESC ignore
// 08 aug 2007 - added base 10 to parseint, fixed df_fmttime and df_GetTimeEntryFieldValue to handle time in this form:  09:08am (activity log format)
// 18 sep 2007 - added df_savebutton variable which indicates which button clicked
// 14 sep 2007 - df_GetRowBySuffix: adds useful properties to row obj returned
// 07 aug 2007 - added df_GetValue and df_SetValue; df_GetMaxRowSuffix() no longer includes deleted rows (skips over them) - previously it could return deleted rows
// 02 aug 2007 - completed WriteTimeEntryField feature
// 11 jul 2007 - fixed call to fmtnumber in checknumberfield
// 04 jul 2007 - MN: changed to return empty string if no rows exist
// 28 jun 2007 - split javascript functions into separate js file


var df_currentForm, df_currentSubformCode, df_currentRowIndex, df_currentRowSuffix;
var df_subformLabelSepChar = "__";
var df_deleteRowColour = "#ffffcc";
var df_dirtyWarningText = "Any changes you made will NOT be saved."
var df_useJQueryClone = true
var df_autoSelectOnFocus = false

function df_CheckGlobals() {
	//set up default form (if not already exists), used in postback and submit form - typically only used in classic asp projects.
	if (!document.form) document.form = document["form"]
	if (!document.form) document.form = document["aspnetForm"]
	//if (!document.form) alert("Beweb CMA JS: Cannot find form")

	if (!df_subformLabelSepChar) df_subformLabelSepChar = "__";
}

if (jQuery) jQuery(document).ready(df_CheckGlobals)
df_AddLoadEvent(df_CheckGlobals);

function df_InitDirtyWarning(dirtyWarningText) {
	// setup warning system to check for unsaved changes
	// you should call this on page ready
	window.df_warnNotSaved = false
	if (dirtyWarningText + "" != "") df_dirtyWarningText = dirtyWarningText
	if (form.method.toUpperCase() == "POST") {
		// set up save warning onbeforeunload function
		if (typeof jQuery != "undefined") {
			jQuery("input,textarea,select").change(function () { window.df_warnNotSaved = true })
		} else {
			document.onkeyup = function () { window.df_warnNotSaved = true }
		}
		window.onbeforeunload = function () { if (window.df_warnNotSaved) return df_dirtyWarningText }
	}

}

function E$(elementId) {
	var element = document.getElementById(elementId);
	if (!element) {
		var msg = "bewebcma.js MVC E$: Element ID not found.";
		if (typeof (elementId)=="function") {
			// probably meant jquery
			msg += "\nelementID parameter is a function.";
			msg += "\nPossibly this is a $ call which was supposed to be jquery and was taken by beweb-cma E$ instead. You may not have jquery on the page? Or are you calling jQuery.noConflict when you should not be?\n";
			msg += "\n";
			msg += elementId.toString();
		} else {
			msg += "\nelementID parameter is "+typeof (elementId)+".";
			msg += "\n";
			msg += elementId.toString();
		}
		if (E$.caller) {
			msg += "\nCalled by: " + E$.caller;
		}
		alert(msg);
	}
	return element;
}

// only define $ if not already defined - to avoid conflict with jQuery
// if jQuery is included first, this will mean we do not define the $ function here - therefore $ = jQuery
// if jQuery is included after, this will mean jQuery redefines the $ function - therefore $ = jQuery
// if jQuery.noConflict() is called it will give us back the dollar sign so $ = E$
// do not call jQuery.noConflict() if you want to use $ for jQuery
if (typeof $ != "function") $ = E$;

function df_GetObjInSameRow(currentFieldObj, elementID) {
	// returns reference to field object in the same row as the current field
	var suffix = ""
	if (currentFieldObj.df_subformCode) {
		suffix = df_subformLabelSepChar + currentFieldObj.df_subformCode + df_subformLabelSepChar + currentFieldObj.df_rowIndex
	}
	var doc = currentFieldObj.document
	if (document.getElementById) {
		return document.getElementById(elementID + suffix)
	} else if (document.all) {
		return document.all(elementID + suffix)
	} else {
		alert("Your web browser does not meet the minimum requirements for features on this form. Please use a recent version of a mainstream browser.")
		return null
	}
}

function df_GetFieldInSameRow(currentFieldObj, fieldName) {
	// returns reference to field object in the same row as the current field
	var suffix = ""
	if (currentFieldObj == undefined || fieldName == undefined) {
		alert("df_GetFieldInSameRow: Parameter not supplied. currentFieldObj=[" + currentFieldObj + "], fieldName[" + fieldName + "]")
		return null
	} else if (!currentFieldObj || !fieldName) {
		alert("df_GetFieldInSameRow: Parameter is blank. currentFieldObj=[" + currentFieldObj + "], fieldName[" + fieldName + "]")
		return null
	}

	currentFieldObj = df_GetField(currentFieldObj)
	suffix = currentFieldObj.df_suffix

	//var returnField = currentFieldObj.form[fieldName+suffix]
	//var returnField = currentFieldObj.form.elements(fieldName+suffix)
	var returnField = currentFieldObj.form.elements[fieldName + suffix]

	if (!returnField) {
		if (!suffix) {
			alert("df_GetFieldInSameRow: Cannot find form field for return value. The supplied field was found, but it did not have a df_subformCode attribute, so you must supply the suffix as part of the field name. currentFieldObj=[" + currentFieldObj + "], currentFieldName=[" + currentFieldObj.name + "], to find fieldName[" + fieldName + "], suffix[" + suffix + "], currentFieldObj[" + currentFieldObj + "], returnField[" + returnField + "]")
		} else {
			alert("df_GetFieldInSameRow: Cannot find form field for return value. currentFieldObj=[" + currentFieldObj + "], fieldName[" + fieldName + "], suffix[" + suffix + "], currentFieldObj[" + currentFieldObj + "], returnField[" + returnField + "]")
		}
	}
	return returnField
	//		return df_currentForm[fieldName+df_currentRowSuffix]
}

function df_GetField(field, subformCode, rowIndex) {
	var fieldName, form, tempRowIndex, fieldNamePartArray, origFieldName
	if (typeof (field) == 'string') {
		// field is just a string
		// if using subforms you must supply the full field name with suffix
		if (!subformCode) {
			// param is the full fieldname, including suffix if any
			fieldName = field
			if (jQuery) {
				$field = $(":input[name='" + fieldName + "']")
				if ($field.length > 0) {
					field = $field[0]
					form = field.form
				} else {
					alert("df_GetField: field not found with name " + fieldName)
				}
			} else {
				form = document.form   // back compat non-jq version assumes only a single main form
				field = form[fieldName]
			}
		} else {
			// see if field exists given subformCode,rowIndex exist
			origFieldName = field
			var fullFieldName, suffix
			if (field.indexOf("__suffix__") == -1) {
				// assume suffix is on the end
				fullFieldName = origFieldName + "__suffix__"
			} else {
				fullFieldName = origFieldName + ""
			}
			if (!rowIndex) rowIndex = 1
			suffix = df_subformLabelSepChar + subformCode + df_subformLabelSepChar + rowIndex
			//				fieldName = origFieldName +df_subformLabelSepChar+ subformCode +df_subformLabelSepChar+ rowIndex
			fieldName = fullFieldName.replace("__suffix__", suffix)
			if (jQuery) {
				$field = $(":input[name='" + fieldName + "']")
				if ($field.length > 0) {
					field = $field[0]
					form = field.form
				}
			} else {
				form = document.form   // back compat non-jq version assumes only a single main form
			}
			if (form[fieldName]) {
				// found the field with the subform supplied
				field = form[fieldName]
			} else {
				// could not find the field, maybe no rows, try looking for the hidden adding row
				//fieldName = origFieldName +df_subformLabelSepChar+ subformCode +df_subformLabelSepChar+ "newindex"
				suffix = df_subformLabelSepChar + subformCode + df_subformLabelSepChar + "newindex"
				fieldName = fullFieldName.replace("__suffix__", suffix)
				if (form[fieldName]) {
					field = form[fieldName]
				} else {
					// could not find the field, try looking for it without appending the suffix
					fieldName = origFieldName
					field = form[fieldName]
				}
			}
		}
		if (!field) {
			alert("SAVVY ERROR: df_GetField - field name not found. If using subforms you must supply the full field name with suffix.")
			alert("df_GetField: Debug. field=[" + field + "], fieldName=[" + fieldName + "], origFieldName=[" + origFieldName + "], subformCode[" + subformCode + "], rowIndex[" + rowIndex + "]")
			debugger
		}
	} else if (field && field.name) {
		// field is a field object
		fieldName = field.name
		form = field.form
	} else {
		alert("SAVVY ERROR: df_GetField - field name not found. If using subforms you must supply the full field name with suffix.")
		alert("df_GetField: Debug. field=[" + field + "], fieldName=[" + fieldName + "], origFieldName=[" + origFieldName + "], subformCode[" + subformCode + "], rowIndex[" + rowIndex + "]")
		debugger
	}

	// for firefox, you cannot get expando properties that are defined in the html, they need to be got using getAttribute
	// so this code promotes the html attributes into real javascript properties
	// meaning that any subsequent code can simply refer to eg field.df_suffix
	if (!field.df_subformCode) {
		field.df_subformCode = field.getAttribute("df_subformCode")
		if (field.df_subformCode == "null") field.df_subformCode = null
	}
	if (!field.df_rowIndex) {
		field.df_rowIndex = field.getAttribute("df_rowIndex")
		if (field.df_subformCode == "null") field.df_subformCode = null
	}
	if (!field.df_suffix) {
		field.df_suffix = field.getAttribute("df_suffix")
		if (field.df_suffix == "null") field.df_suffix = null
	}
	if (!field.df_fieldName) {
		field.df_fieldName = field.getAttribute("df_fieldName")
		if (field.df_fieldName == "null") field.df_fieldName = null
	}

	// see if field has subformCode attribs and add if not
	if (!field.df_subformCode || !field.df_suffix || !field.df_rowIndex) {
		fieldNamePartArray = fieldName.split(df_subformLabelSepChar)
		if (fieldNamePartArray.length > 2) {
			// yes - field looks like it has subformcode and rowindex
			// this tricky logic is to allow database fieldnames to contain underscores
			// note we parseInt first so we only get the int part in case there are extra chars on the end
			tempRowIndex = parseInt(fieldNamePartArray[fieldNamePartArray.length - 1], 10)
			if (!isNaN(tempRowIndex)) {
				// yes - its really a rowindex
				// may as well add the attibutes to the field for next time
				field.df_rowIndex = tempRowIndex
				field.df_subformCode = fieldNamePartArray[fieldNamePartArray.length - 2]
				field.df_suffix = df_subformLabelSepChar + field.df_subformCode + df_subformLabelSepChar + field.df_rowIndex
				suffixLen = (field.df_suffix).length
				if (!field.df_fieldName) {
					field.df_fieldName = fieldName.substring(0, fieldName.length - suffixLen)
					//						field.df_fieldName = fieldName.replace(field.df_suffix, "__suffix__")
				}
			}
		} else {
			// presumably this is a non-subform field (ie most likely in the main record)
			if (!field.df_fieldName) {
				field.df_fieldName = fieldName
			}
			//alert("fn["+fieldName+"]")
		}
	}
	field.df_fieldType = field.getAttribute("df_fieldType")
	field.df_decimalPlaces = field.getAttribute("df_decimalPlaces")
	field.df_groupDigits = field.getAttribute("df_groupDigits")
	field.df_allowNegative = field.getAttribute("df_allowNegative")
	field.df_format = field.getAttribute("df_format")
	return field
}

function df_GetFields(field, subformCode) {
	// similar to df_GetFieldsInAllRows except that if rows have been inserted this returns an array of the fields in visual order (better) if possible
	// returns a zero based array without any gaps
	// note this relies on jQuery so if jQuery is not included this just calls df_GetFieldsInAllRows() - note the differences
	// returns an array of references to field objects in all non-deleted rows in the same subform as the current field
	// either pass in (a) a field object reference or (b) a field name and subform code
	var form, fieldName
	//20130319MN - always call getfield as it handles all cases -- update 20141203MN no it does not, reverted
	if (typeof (field) == 'string' && typeof (subformCode) == 'string') {
		fieldName = field
		form = document.form
	} else {
		// a field object was supplied or no subformCode
		field = df_GetField(field, subformCode)
		fieldName = field.df_fieldName
		subformCode = field.df_subformCode
		form = field.form
	}

	if (jQuery !== undefined) {
		var baseName = fieldName + df_subformLabelSepChar + subformCode
		//return jQuery('[name^="'+baseName+'"]').filter(":visible").toArray()
		var returnArray = new Array()
		var rowIndex = 0
		jQuery('[name^="' + baseName + '"]').each(function () {
			if (this && this.name && this.name.indexOf("newindex") == -1) {  // newindex is always the hidden adding template row
				var field = df_GetField(this)
				var suffix = field.df_suffix
				if (field.form["df_status" + suffix] && field.form["df_status" + suffix].value != 'deleted') {
					returnArray[rowIndex] = field
					rowIndex++
				}
			}
		})
		return returnArray
	}

	return df_GetFieldsInAllRows(field, subformCode)
}

function df_GetFieldsInAllRows(field, subformCode) {
	// similar to df_GetFields except that if rows have been inserted this returns an array of the fields in suffix order (not as good)
	// returns a one-based based with indexes corresponding to rowIndexes of the fields - this could contain gaps so you need to use "for (rowIndex in result)" to iterate it rather than "for (i=0; i<result.length; i++)"
	// returns an array of references to field objects in all non-deleted rows in the same subform as the current field
	// either pass in (a) a field object reference or (b) a field name and subform code
	var form, fieldName
	//20130319MN - always call getfield as it handles all cases -- update 20141203MN no it does not, reverted
	if (typeof (field) == 'string' && typeof (subformCode) == 'string') {
		fieldName = field
		form = document.form
	} else {
		// a field object was supplied or no subformCode
		field = df_GetField(field, subformCode)
		fieldName = field.df_fieldName
		subformCode = field.df_subformCode
		form = field.form
	}

	var maxIndexField = form["df_MaxRow" + df_subformLabelSepChar + subformCode]
	if (!maxIndexField) {
		alert("SAVVY ERROR: df_GetFieldsInAllRows - hidden field MaxRow not found for subform [" + subformCode + "]. This may not be a valid subform name.")
		debugger
	}
	var maxIndex = maxIndexField.value
	var returnArray = new Array()

	for (var rowIndex = 0; rowIndex <= maxIndex; rowIndex++) {
		var suffix = df_subformLabelSepChar + subformCode + df_subformLabelSepChar + rowIndex
		var field = form["df_status" + suffix]
		if (field && field.value != 'deleted') {
			field = form[fieldName + suffix]
			field = df_GetField(field)
			if (field) {
				returnArray[rowIndex] = field
			}
		}
	}
	return returnArray
}

function df_GetSubformTotal(field, subformCode) {
	// calc total of a numeric field in all rows - return as number
	// either pass in (a) a field object reference or (b) a field name and subform code
	return df_GetSubformTotal_Internal(field, subformCode, false)
}

function df_GetSubformTotalString(field, subformCode) {
	// calc total of a numeric field in all rows - return as string
	// either pass in (a) a field object reference or (b) a field name and subform code
	return df_GetSubformTotal_Internal(field, subformCode, true)
}

function df_GetSubformTotal_Internal(field, subformCode, returnFormattedString) {
	// private function
	var total = 0
	//var rows = df_GetFields(field, subformCode) //cool new way using jquery
	var rows = df_GetFieldsInAllRows(field, subformCode)  //old school way, but it's quicker
	for (rowIndex in rows) {
		field = rows[rowIndex]
		total += df_GetNumberFieldValue(field)
	}
	if (returnFormattedString) {      // format the result as a string with correct decimal places etc
		if (field && field.df_decimalPlaces != undefined) {
			total = df_FmtNumber(total, field.df_decimalPlaces, field.df_groupDigits)
		} else {
			total = Math.round(total * 1000000.0) / 1000000.0
		}
	}
	return total
}

function df_GetDropDownValue(field) {
	return field.options[field.selectedIndex].value
}
function df_GetDropDownText(field) {
	return field.options[field.selectedIndex].text
}
function df_SetDropDownValue(field, newValue) {
	// sets a dropdown select field to the given value (assuming the value is an option)
	for (var i = 0; i < field.options.length; i++) {
		if (field.options[i].value + "" == newValue + "") {
			field.selectedIndex = i
		}
	}
}
function df_SetDropDownText(field, textToFind) {
	// sets a dropdown select field by text (assuming the text is the text of an option)
	for (var i = 0; i < field.options.length; i++) {
		if (field.options[i].text + "" == textToFind + "") {
			field.selectedIndex = i
		}
	}
}

function df_SetFieldReadOnly(field, newState) {
	// if newState is true, sets field to readonly and grey
	// if newState is false, sets field to normal
	if (!field) {
		alert("df_SetFieldReadOnly: Field does not exist [" + field + "]")
	}
	if (newState == undefined) {
		alert("df_SetFieldReadOnly: second parameter missing (true or false)")
	}
	if (typeof (field) == "string") {
		field = df_GetField(field)
	}
	if (field.type == "hidden" && field.form["chbox_" + field.name]) {
		field.form["chbox_" + field.name].disabled = newState
	}
	if (newState) {
		field.readOnly = true
		field.style.backgroundColor = "#dddddd"
	} else {
		field.readOnly = false
		field.style.backgroundColor = "#ffffff"
	}
}

function df_DisableAllFields() {
	df_EnableAllFields(false)
}

function df_EnableAllFields(newState) {
	newState = newState || true
	if (jQuery) {
		$(":input").each(function () {
			var field = this
			if (field && (field.type || field.name)) {
				if (field.type == "text" || field.type == "button" || field.type == "submit" || field.type == "textarea") {
					df_SetFieldReadOnly(field, newState)     // this is better since it keeps the values able to post for callbacks
				} else if (field.type == "checkbox" || field.type == "select-one" || field.type == "select-multiple" || field.type == "radio") {
					field.disabled = newState
				}
			}
		});

	} else {
		var form = document.form
		var fieldName
		for (fieldName in form.elements) {
			var field = form.elements[fieldName]
			if (field && (field.type || field.name)) {
				if (field.type == "text" || field.type == "button" || field.type == "submit" || field.type == "textarea") {
					df_SetFieldReadOnly(field, newState)     // this is better since it keeps the values able to post for callbacks
				} else if (field.type == "checkbox" || field.type == "select-one" || field.type == "select-multiple" || field.type == "radio") {
					field.disabled = newState
				}
			}
		}
	}
}

function df_GetNumberFieldValue(field) {
	var str = StripNumber(field)
	if (str != "") {
		return parseFloat(str)
	} else {
		return 0
	}
}

function df_FmtNumber(numOrField, decimalPlaces, groupDigits) {
	// if the field can have any number of decimal places, pass in decimalPlaces = -1 
	if (decimalPlaces == undefined) {
		if (numOrField.value && numOrField.getAttribute('df_decimalPlaces')) {
			decimalPlaces = parseInt(numOrField.getAttribute('df_decimalPlaces'), 10)
		} else {
			decimalPlaces = 0
		}
	}
	if (groupDigits == undefined) {
		if (numOrField.value && numOrField.getAttribute('df_groupDigits')) {
			groupDigits = (numOrField.getAttribute('df_groupDigits') == "true")
		} else {
			groupDigits = true
		}
	}
	if (numOrField.value) {
		num = df_GetNumberFieldValue(numOrField)
	} else {
		num = parseFloat(numOrField)
	}
	if (decimalPlaces == -1) {
		// get rid of crapped out buggy rounding errors
		num = Math.round(num * 1000000.0) / 1000000.0
	} else {
		// negative numbers need to be turned positive for rounding - then back again.
		var isNeg = num < 0;
		if (isNeg) {
			num *= -1
		}
		num = Math.round(Math.pow(10, decimalPlaces) * num) / Math.pow(10, decimalPlaces)
		if (isNeg) {
			num *= -1
		}
	}
	if (num == Number.POSITIVE_INFINITY || num == Number.NEGATIVE_INFINITY) {
		str = num
	} else if (num == 0) {
		str = "0"
		if (decimalPlaces > 0) str += "."
		for (var i = 0; i < decimalPlaces; i++) {
			str += "0"
		}
	} else {
		// split apart whole number and decimal
		var str = num + ""
		var hasDecimalPoint = (str.indexOf('.') > -1)
		var decPart = ''
		var intPart = str
		if (hasDecimalPoint) {
			intPart = str.split('.')[0]
			decPart = str.split('.')[1]
		}
		// pad decimals with trailing zeros
		if (decimalPlaces > 0) {
			// truncate any extra decimals
			decPart = decPart.substring(0, decimalPlaces)
			var i, extraZeros
			extraZeros = decimalPlaces - decPart.length
			for (i = 0; i < extraZeros; i++) {
				decPart = decPart + '0'
			}
		}
		// group digits with commas
		if (groupDigits) {
			var firstDigitIndex = intPart.substr(0, 1) == "-" ? 1 : 0
			var groupedIntPart = ""
			for (var i = 0; i < intPart.length; i++) {
				if (i > firstDigitIndex && (intPart.length - i) % 3 == 0) groupedIntPart += ","
				groupedIntPart += intPart.substr(i, 1)
			}
			intPart = groupedIntPart
		}
		// piece back together
		str = intPart
		if (decPart != "") str += "." + decPart
	}
	return str
}

function df_KeyPressFieldHandler(event, fieldObj) {
	// check to see if first param was the event or field (for backwards compat)
	if (event.type != "keydown") {
		// its not an event object, it must be a field
		fieldObj = event
		if (window.event) {
			event = window.event
		} else {
			alert("This page will only work in Internet Explorer. You are running Firefox or another browser. Please use Internet Explorer instead.")
		}
	}

	// add properties to field if not yet defined
	fieldObj = df_GetField(fieldObj)

	// set global js vars
	df_currentForm = fieldObj.form
	if (fieldObj.df_subformCode) {
		df_currentSubformCode = fieldObj.df_subformCode
		df_currentRowSuffix = df_subformLabelSepChar + fieldObj.df_subformCode + df_subformLabelSepChar + fieldObj.df_rowIndex
	} else {
		df_currentRowSuffix = ""
	}

	if (event) {
		if (df_currentRowSuffix) {
			// we are in a subform field
			var keyCode = event.keyCode || event.which
			if (keyCode == 40 && fieldObj.df_fieldType != "Text") {
				// down arrow
				//					var maxRowField = document.form["df_MaxRow"+df_subformLabelSepChar+fieldObj.df_subformCode]
				//					var maxRowIndex = parseInt(maxRowField.value, 10)
				//					var rowIndex = parseInt(fieldObj.df_rowIndex, 10)
				//					rowIndex++
				//					if (rowIndex <= maxRowIndex) {
				//						var nextField = df_currentForm[fieldObj.df_fieldName+df_subformLabelSepChar+fieldObj.df_subformCode+df_subformLabelSepChar+rowIndex]
				var nextField = df_GetFieldBelow(fieldObj)
				if (nextField) {
					nextField.focus()
					// need to use timeout for chrome
					//window.setTimeout(function(){nextField.select()},10)
				}
			} else if (keyCode == 38 && fieldObj.df_fieldType != "Text") {
				//var rowIndex = parseInt(fieldObj.df_rowIndex, 10)
				//rowIndex--
				//if (rowIndex > 0) {
				//	var nextField = df_currentForm[fieldObj.df_fieldName+df_subformLabelSepChar+fieldObj.df_subformCode+df_subformLabelSepChar+rowIndex]
				var nextField = df_GetFieldAbove(fieldObj)
				if (nextField) {
					nextField.focus()
					// use timeout for chrome
					//window.setTimeout(function(){nextField.select()},10)
				}
			}
		}

		if (event.keyCode == 27) {   // escape
			return false     // stop default behavour, which is to reset the form
		}

		// then see if custom cf_OnKeyPress exists
		var functionName = fieldObj.df_fieldName + ""
		functionName = functionName.replace(" ", "")
		var func = eval("window.cf_OnKeyPress" + functionName)
		if (!func) return true    // valid value and no onkeypress so allow key
		// call function and return value
		return func(fieldObj, event)

	}  // end if event object exists
}

function df_GetFieldAbove(field, subformCode) {
	// returns reference to same field in the row above, or null if top row
	var field = df_GetField(field, subformCode)
	var rows = df_GetFields(field, subformCode)
	var lastField = null
	for (var i in rows) {
		var scanField = rows[i]
		if (scanField.df_suffix == field.df_suffix) {
			return lastField
		}
		lastField = scanField
	}
	/*
	var fieldName = field.df_fieldName
	var subformCode = field.df_subformCode
	var form = field.form
	var rowIndex = parseInt(field.df_rowIndex, 10)
	while (rowIndex > 0) {
		rowIndex--
		var suffix = df_subformLabelSepChar+subformCode+df_subformLabelSepChar+rowIndex
		var statusField = form["df_status"+suffix]
		if (statusField && statusField.value!='deleted') {
			return df_GetField(fieldName, subformCode, rowIndex)
		}
	}
	*/
	return null
}

function df_GetFieldBelow(field, subformCode) {
	// returns reference to same field in the row below, or null if bottom row
	var field = df_GetField(field, subformCode)
	var rows = df_GetFields(field, subformCode)
	var isNext = false
	for (var i in rows) {
		var scanField = rows[i]
		if (scanField.df_suffix == field.df_suffix) {
			isNext = true
		} else if (isNext) {
			return scanField
		}
	}
	/*
	var fieldName = field.df_fieldName
	var subformCode = field.df_subformCode
	var form = field.form
	var rowIndex = parseInt(field.df_rowIndex, 10)
	var maxIndex = df_GetMaxRowIndex(subformCode)
	while (rowIndex < maxIndex) {
		rowIndex++
		var suffix = df_subformLabelSepChar+subformCode+df_subformLabelSepChar+rowIndex
		var statusField = form["df_status"+suffix]
		if (statusField && statusField.value!='deleted') {
			return df_GetField(fieldName, subformCode, rowIndex)
		}
	}
	*/
	return null
}

function df_GetRowIndexFromSuffix(suffix) {
	// returns the rowIndex part of a suffix string
	var fieldNamePartArray = suffix.split(df_subformLabelSepChar)
	var tempRowIndex = fieldNamePartArray[fieldNamePartArray.length - 1]
	return parseInt(tempRowIndex, 10)
}

function df_GetSubformCodeFromSuffix(suffix) {
	// returns the SubformCode part of a suffix string
	var fieldNamePartArray = suffix.split(df_subformLabelSepChar)
	return fieldNamePartArray[fieldNamePartArray.length - 2]
}

function df_GetRowBySuffix(suffix) {
	// returns a subform row (TR object)
	var row = document.getElementById("df_SubformRow" + suffix)
	// adorn row object - add useful properties to row object
	row.df_suffix = suffix
	//var fieldNamePartArray = suffix.split(df_subformLabelSepChar)
	//var tempRowIndex = fieldNamePartArray[fieldNamePartArray.length-1]
	//row.df_rowIndex = parseInt(tempRowIndex)
	row.df_rowIndex = df_GetRowIndexFromSuffix(suffix)

	return row
}

function df_SetRowBgColor(rowSuffix, newColour) {
	// sets background color style of all the cells in the TR given a suffix
	var row = document.getElementById("df_SubformRow" + rowSuffix)
	var cellIndex, cell
	for (cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
		cell = row.cells[cellIndex]
		cell.style.backgroundColor = newColour
	}
}

function df_ChangeFieldHandler(event, fieldObj) {
	// called in onchange in WriteField etc

	if (event) {
		// check to see if first param was the event or field (for backwards compat)
		// see if it looks like an event object - "beforedeactivate" is coming through in IE8 triggered jQuery
		var isEvent = (event.clientX || event.type == "change" || event.type == "click" || event.type == "blur" || event.type == "DOMContentLoaded" || event.bubbles + "" == "")
		if (!isEvent) {
			// its not an event object, it must be a field
			fieldObj = event
			if (window.event) {
				event = window.event
			} else {
				// ignore - event is not actually needed by this function (yet) so it should run ok
				//alert("This page will only work in Internet Explorer. You are running Firefox or another browser. Please use Internet Explorer instead.")
			}
		}
	}

	// add properties to field if not yet defined
	fieldObj = df_GetField(fieldObj)

	// set global js vars
	df_currentForm = fieldObj.form
	if (fieldObj.df_subformCode) {
		df_currentRowSuffix = df_subformLabelSepChar + fieldObj.df_subformCode + df_subformLabelSepChar + fieldObj.df_rowIndex
	} else {
		df_currentRowSuffix = ""
	}

	// then see if custom onBeforeChange exists
	var functionName = fieldObj.df_fieldName + ""
	functionName = functionName.replace(" ", "")
	var func = eval("window.cf_OnBeforeChange" + functionName)
	if (func) {
		// call function and return if false (ie tell browser to cancel the change)
		if (func(fieldObj) == false) {
			fieldObj.df_isInvalid = true
			return false
		}
	}

	// first see if valid for data type
	if (fieldObj.df_fieldType) {
		if (fieldObj.df_fieldType == "Number") {
			// trim it first
			fieldObj.value = df_Trim(fieldObj.value)
			if (fieldObj.value != "") {   // only eval if not blank
				// strip out any commas before evalling
				fieldObj.value = fieldObj.value.replace(/,/g, "")
				fieldObj.value = fieldObj.value.replace(/\$/g, "")
				// eval numbers to allow users to enter arithmetic eg "250*12"
				try {
					fieldObj.value = eval(fieldObj.value)
				} catch (e) {
				}
			}
			// return false if not a valid number
			if (!CheckNumberField(fieldObj, parseInt(fieldObj.df_decimalPlaces, 10), (fieldObj.df_allowNegative == "true"), (fieldObj.df_groupDigits == "true"))) {
				// copy this validation to onblur so it keeps firing until the user changes it to a valid value, and ensure they don't leave the field by calling select()
				fieldObj.df_isInvalid = true
				//fieldObj.onblur = fieldObj.onchange
				fieldObj.select()
				return false
			}
		} else if (fieldObj.df_fieldType == "TimeEntry") {
			// return false if not a valid time
			if (!CheckTimeEntryField(fieldObj)) {
				// copy this validation to onblur so it keeps firing until the user changes it to a valid value, and ensure they don't leave the field by calling select()
				fieldObj.df_isInvalid = true
				//fieldObj.onblur = fieldObj.onchange
				fieldObj.select()
				return false
			}
		} else if (fieldObj.df_fieldType == "URL") {
			// return false if not a valid time
			if (!CheckURLField(fieldObj)) {
				// copy this validation to onblur so it keeps firing until the user changes it to a valid value, and ensure they don't leave the field by calling select()
				fieldObj.df_isInvalid = true
				//fieldObj.onblur = fieldObj.onchange
				fieldObj.select()
				return false
			}
		} else if (fieldObj.df_fieldType == "Date") {
			// return false if not a valid time
			if (!CheckDateField(fieldObj)) {
				// copy this validation to onblur so it keeps firing until the user changes it to a valid value, and ensure they don't leave the field by calling select()
				fieldObj.df_isInvalid = true
				//fieldObj.onblur = fieldObj.onchange
				fieldObj.select()
				return false
			}
		}
	}

	//		if (fieldObj.onblur == fieldObj.onchange) {
	//			fieldObj.onblur = null     // TODO: this should store and replace previous onblur
	//		}

	// then see if custom onchange exists
	var functionName = fieldObj.df_fieldName + ""
	functionName = functionName.replace(" ", "")
	var func = eval("window.cf_OnChange" + functionName)
	if (!func) {
		fieldObj.df_isInvalid = false
		return true    // valid value and no onchange so allow change
	}
	// call function and return value
	//return func(fieldObj)
	var isOK = func(fieldObj)
	if (isOK) {
		// "return true" from your custom function will allow the user to leave the field
		fieldObj.df_isInvalid = false
	} else if (isOK == false) {
		// "return false" from your custom function will prevent user leaving field (IE only)
		fieldObj.df_isInvalid = true
		// take user back to the field - onchange not cancelable except on IE, so this is needed for other browsers, could also restore old value
		if ('defaultValue' in fieldObj) {
			fieldObj.value = fieldObj.defaultValue;
		}
		try {
			fieldObj.focus();
			fieldObj.select();
		} catch (e) {
		}
	} else {
		// "return" from your custom function will keep focus on the field, but still allow the user to leave the field
		fieldObj.df_isInvalid = false
	}
	return isOK
}

function df_BlurFieldHandler(event, fieldObj) {
	// add properties to field if not yet defined
	fieldObj = df_GetField(fieldObj)

	// if onchange was called and returned false, then this field is still in an invalid state but the browser will allow the user out
	// however, we don't allow the user out until they have corrected the data
	if (fieldObj.df_isInvalid) {
		fieldObj.df_isInvalid = !df_ChangeFieldHandler(event, fieldObj)
		if (fieldObj.df_isInvalid) {
			// don't let the user out of the field until they correct the data!
			fieldObj.select()
			return false
		}
	}

	// set global js vars
	df_currentForm = fieldObj.form
	if (fieldObj.df_subformCode) {
		df_currentSubformCode = fieldObj.df_subformCode
		df_currentRowSuffix = df_subformLabelSepChar + fieldObj.df_subformCode + df_subformLabelSepChar + fieldObj.df_rowIndex
	} else {
		df_currentRowSuffix = ""
	}

	// then see if custom onblur exists
	var functionName = fieldObj.df_fieldName + ""
	functionName = functionName.replace(" ", "")
	var func = eval("window.cf_OnBlur" + functionName)
	if (!func) return true    // valid value and no onblur so allow blur
	// call function and return value
	//		return func(fieldObj)
	var isOK = func(fieldObj)
	if (isOK) {
		// "return true" from your custom function will allow the user to leave the field
		fieldObj.df_isInvalid = false
	} else if (isOK == false) {
		// "return false" from your custom function will prevent user leaving field
		fieldObj.df_isInvalid = true
		fieldObj.focus()
	} else {
		// "return" from your custom function will allow the user to leave the field
		fieldObj.df_isInvalid = false
	}
	return isOK
}

function df_ClickFieldHandler(event, fieldObj) {
	// add properties to field if not yet defined
	fieldObj = df_GetField(fieldObj)

	// set global js vars
	df_currentForm = fieldObj.form
	if (fieldObj.df_subformCode) {
		df_currentSubformCode = fieldObj.df_subformCode
		df_currentRowSuffix = df_subformLabelSepChar + fieldObj.df_subformCode + df_subformLabelSepChar + fieldObj.df_rowIndex
	} else {
		df_currentRowSuffix = ""
	}

	// then see if custom onclick exists
	var functionName = fieldObj.df_fieldName + ""
	functionName = functionName.replace(" ", "")
	var func = eval("window.cf_OnClick" + functionName)
	if (!func) return true    // valid value and no onclick so allow click
	// call function and return value
	return func(fieldObj)
}

function df_FocusFieldHandler(event, fieldObj) {
	// add properties to field if not yet defined
	fieldObj = df_GetField(fieldObj)
	//auto select contents of a field when focused
	if (df_autoSelectOnFocus && fieldObj && fieldObj.select && fieldObj.tagName && fieldObj.tagName == "INPUT") {
		// fieldObj.select()

		//JN Removed 20110614
		//window.setTimeout(function(){fieldObj.select()},1)
    try{
  		fieldObj.select()
		} catch (e) {
		}
	}

	// set global js vars
	df_currentForm = fieldObj.form
	if (fieldObj.df_subformCode) {
		df_currentSubformCode = fieldObj.df_subformCode
		df_currentRowSuffix = df_subformLabelSepChar + fieldObj.df_subformCode + df_subformLabelSepChar + fieldObj.df_rowIndex
	} else {
		df_currentRowSuffix = ""
	}

	// then see if custom onFocus exists
	var functionName = fieldObj.df_fieldName + ""
	functionName = functionName.replace(" ", "")
	var func = eval("window.cf_OnFocus" + functionName)
	if (!func) return true    // valid value and no onFocus so allow Focus
	// call function and return value
	return func(fieldObj)
}

function df_DeleteRow(field, suffix, subformCode, dontPromptUser) {
	//Changed to parentNode for firefox.	
	//var td = field.parentElement
	//		var td = field.parentNode
	//var row = td.parentElement
	//		var row = td.parentNode
	var row = df_GetRowBySuffix(suffix);
	var cellIndex, cell;

	for (cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
		cell = row.cells[cellIndex];
		cell.oldBackgroundColor = cell.style.backgroundColor;
		cell.style.backgroundColor = df_deleteRowColour;
	}
	if (dontPromptUser || confirm("Are you sure you want to delete this row?")) {
		if (df_useJQueryClone && jQuery !== undefined && jQuery.support.opacity) {
			jQuery(row).children().animate({ opacity: 0 }, 400, function () { row.style.display = "none"; });
		} else {
			row.style.display = "none";
		}
		var form = document.form;
		if (field && field.form) {
			form = field.form;
		}
		var statusFld = form["df_status" + suffix];
		if (statusFld) statusFld.value = "deleted"
		// see if custom ondelete exists
		var func = eval("window.cf_OnDelete" + subformCode);
		if (func) func(row);
	} else {
		for (cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
			cell = row.cells[cellIndex]
			cell.style.backgroundColor = cell.oldBackgroundColor
		}
	}
}

function df_GetMaxRowIndex(subformCodeOrField) {
	// note: this includes deleted rows, not the same as df_GetMaxRowSuffix
	var form, maxRowField, subformCode
	if (subformCodeOrField.df_subformCode) { //supplied param is a field
		subformCode = subformCodeOrField.df_subformCode
		form = subformCodeOrField.form
	} else {  // assume it's a string - name of subform
		subformCode = subformCodeOrField
		form = document.form
	}
	var maxRowField = null;
	if (jQuery) {
		maxRowField = $("input[name=df_MaxRow" + df_subformLabelSepChar + subformCode + "]")[0] //look up by name (not id)
	} else {
		maxRowField = form["df_MaxRow" + df_subformLabelSepChar + subformCode]
	}
	if (!maxRowField) {
		alert("SAVVY ERROR: df_GetMaxRowIndex - hidden field MaxRow not found for subform [" + subformCode + "]. This may not be a valid subform name.")
		debugger
	}
	return parseInt(maxRowField.value, 10);
}

function df_GetMaxRowSuffix(subformCodeOrField) {
	// note: this does not include deleted rows. If you want to include deleted rows, use df_GetMaxRowIndex
	// MN: changed so it no longer includes deleted rows (skips over them) - previously it could return deleted rows
	var form, maxRowField, subformCode
	if (subformCodeOrField == undefined) {
		alert("df_GetMaxRowSuffix() called without parameter subformCodeOrField");
	} else if (subformCodeOrField.df_subformCode) {
		subformCode = subformCodeOrField.df_subformCode;
		form = subformCodeOrField.form;
	} else {
		subformCode = subformCodeOrField;
		form = document.form;
	}

	// MN: changed so it no longer includes deleted rows (skips over them)
	var maxRowField = null;
	if (jQuery) {
		maxRowField = $("input[name=df_MaxRow" + df_subformLabelSepChar + subformCode + "]")[0] //look up by name (not id)
	} else {
		maxRowField = form["df_MaxRow" + df_subformLabelSepChar + subformCode]
	}
	if (!maxRowField) {
		alert("SAVVY ERROR: df_GetMaxRowSuffix - hidden field MaxRow not found for subform [" + subformCode + "]. This may not be a valid subform name.")
		//debugger 
	}
	var rowIndex = parseInt(maxRowField.value, 10)
	//		if (rowIndex == 0) {
	//			return ""
	//		}
	//		var suffix = df_subformLabelSepChar + subformCode +df_subformLabelSepChar+ rowIndex
	//		var statusField = form["df_status"+suffix]
	while (rowIndex > 0) {
		suffix = df_subformLabelSepChar + subformCode + df_subformLabelSepChar + rowIndex
		statusField = form["df_status" + suffix]
		// MN: feb 2009 - if user hits Back button, any new row will disappear, so statusField = undefined
		if (!statusField || statusField.value == 'deleted') {
			rowIndex--
		} else {
			break
		}
	}

	if (rowIndex < 1) {
		// MN 4 jul 07 - changed to return empty string if no rows exist
		return "";
	} else {
		return suffix;
	}
}

function df_AddRow(subformCode) {
	// see if custom OnAddRow exists
	var func = eval("window.cf_OnAddRow" + subformCode);
	if (func) func(subformCode);
	// insert row at bottom
	return df_InsertRow(subformCode, -1);
	// moved redips drag code to proper place below with datepicker and select2
}

function df_InsertRow(subformCode, rowIndexToInsertBefore) {

	//var maxRowField = document.form["df_MaxRow" + df_subformLabelSepChar + subformCode];
	var maxRowField = null;
	if (window.jQuery) {
		maxRowField = $("input[name=df_MaxRow" + df_subformLabelSepChar + subformCode + "]")[0]; //look up by name (not id)
	} else {
		maxRowField = form["df_MaxRow" + df_subformLabelSepChar + subformCode];
	}

	if (!maxRowField) {
		alert("SAVVY ERROR: df_AddRow - hidden field MaxRow not found for subform [" + subformCode + "]. This may not be a valid subform name.")
		debugger
	}
	var newindex = parseInt(maxRowField.value, 10);
	newindex++
	maxRowField.value = newindex;

	var templateRow = document.getElementById("df_SubformRow" + df_subformLabelSepChar + subformCode + df_subformLabelSepChar + "newindex")
	var table = document.getElementById("df_SubformTable_" + subformCode)
	if (!table) {
		var table$ = jQuery("#df_SubformTable_" + subformCode)
		if (table$.length > -1) {
			table = table$[0]
		}
	}
	if (!table) {
		alert("Subform not found: " + subformCode)
	}
	//	var table = templateRow.parentTable
	var newRow
	if (rowIndexToInsertBefore == -1) {
		newRow = table.insertRow(templateRow.rowIndex)
	} else {
		newRow = table.insertRow(rowIndexToInsertBefore)
	}
	if (df_useJQueryClone) {
		// new jquery approach
		var cloneRow = jQuery(templateRow).clone(true, true)

		if ($('select.svySelect2', cloneRow).length > 0) {
			$('select.svySelect2', cloneRow).width($('.select2-container', cloneRow).width()); // Maintain the width of the select once it is recreated
			$('.select2-container', cloneRow).remove(); // Remove select2 dom elements from clone
		}

		//var wrapper = cloneRow.wrap("<div></div>").parent()
		//var html = wrapper.html()
		var html = cloneRow.html()
		html = html.replace(/newindex/ig, "" + newindex)
		//jQuery(newRow).replaceWith(html)
		jQuery(newRow).html(html)
		// copy attribs of TR
		if (templateRow.className) newRow.className = templateRow.className
		if (templateRow.colSpan) newRow.colSpan = templateRow.colSpan
		if (templateRow.rowSpan) newRow.rowSpan = templateRow.rowSpan
		if (templateRow.align) newRow.align = templateRow.align
		if (templateRow.vAlign) newRow.vAlign = templateRow.vAlign
		if (templateRow.id) newRow.id = templateRow.id.replace("newindex", "" + newindex)
		var df_suff = jQuery(templateRow).attr("df_suffix");
		if (df_suff) {
			newRow.df_suffix = df_suff.replace("newindex", "" + newindex);
		}
		var df_rowindex = jQuery(templateRow).attr("df_rowIndex");
		if (df_rowindex) {
			newRow.df_rowIndex = df_rowindex.replace("newindex", "" + newindex)
		}
		newRow.setAttribute("df_suffix", newRow.df_suffix)
		newRow.setAttribute("df_rowIndex", newRow.df_rowIndex)
		if (window.jQuery.support.opacity) {
			// fade in
			var visibleCells = jQuery(newRow).children().filter(":visible")
			visibleCells.hide()
			visibleCells.fadeIn().show()
		}
		newRow.style.display = ""
	} else {
		// copy attribs of TR
		if (templateRow.className) newRow.className = templateRow.className
		if (templateRow.colSpan) newRow.colSpan = templateRow.colSpan
		if (templateRow.rowSpan) newRow.rowSpan = templateRow.rowSpan
		if (templateRow.align) newRow.align = templateRow.align
		if (templateRow.vAlign) newRow.vAlign = templateRow.vAlign
		if (templateRow.id) newRow.id = templateRow.id.replace("newindex", "" + newindex)
		newRow.df_suffix = templateRow.getAttribute("df_suffix").replace("newindex", "" + newindex)
		newRow.df_rowIndex = templateRow.getAttribute("df_rowIndex").replace("newindex", "" + newindex)
		newRow.setAttribute("df_suffix", newRow.df_suffix)
		newRow.setAttribute("df_rowIndex", newRow.df_rowIndex)

		for (curr_cell = 0; curr_cell < templateRow.cells.length; curr_cell++) {
			//newCell = newRow.insertCell()  -- i think the -1 is the same anyway
			newCell = newRow.insertCell(-1)

			templateCell = templateRow.cells[curr_cell]
			// copy attribs of TD
			if (templateCell.className) newCell.className = templateCell.className
			if (templateCell.colSpan) newCell.colSpan = templateCell.colSpan
			if (templateCell.rowSpan) newCell.rowSpan = templateCell.rowSpan
			if (templateCell.align) newCell.align = templateCell.align
			if (templateCell.vAlign) newCell.vAlign = templateCell.vAlign
			if (templateCell.style.display) newCell.style.display = templateCell.style.display
			if (templateCell.onclick) newCell.onclick = templateCell.onclick
			if (templateCell.onmouseover) newCell.onmouseover = templateCell.onmouseover
			if (templateCell.onmouseout) newCell.onmouseout = templateCell.onmouseout
			if (templateCell.onkeyup) newCell.onkeyup = templateCell.onkeyup
			if (templateCell.onkeydown) newCell.onkeydown = templateCell.onkeydown
			if (templateCell.onkeypress) newCell.onkeypress = templateCell.onkeypress
			if (templateCell.id) newCell.id = templateCell.id.replace("newindex", "" + newindex)
			if (templateCell.df_suffix) newCell.df_suffix = templateCell.df_suffix.replace("newindex", "" + newindex)
			if (templateCell.df_rowIndex) newCell.df_rowIndex = templateCell.df_rowIndex.replace("newindex", "" + newindex)
			html = templateCell.innerHTML
			while (html.indexOf("newindex") > -1) {
				html = html.replace("newindex", "" + newindex)
			}
			newCell.innerHTML = html
		}
	}

	// try and scroll down a bit in case we clicked the Add Line button and then it got shoved down off the page (note: this only works in IE, should do firefox version too)
	// MN 20100805 - replaced scrollIntoView with jquery version
	//		if (window.event && window.event.srcElement && window.event.srcElement.scrollIntoView) {
	//			window.event.srcElement.scrollIntoView(true)
	//		}

	// removed - this is actually annoying most of the time
	//		if (window.event && window.event.srcElement && (typeof jQuery != "undefined") && $(window.event.srcElement).position) {
	//	  		var pos = jQuery(window.event.srcElement).position().top;
	//	  		jQuery('html').animate({ scrollTop: pos }, 'slow');
	//		}

	// now that we have just created a bunch of new fields, re-apply jquery events such as datepicker plugs etc that select all fields by classname
	if (window.BewebInitForm) {
		//todo:see if needed!			BewebInitForm(document.form.id)
	}
	if (window.BewebInitDatePickers) {
		BewebInitDatePickers()
	}

	if(typeof jQuery != "undefined") {
		if ($.fn.hasOwnProperty('select2') && $('.svySelect2', newRow).length > 0) {
			$('select.svySelect2', newRow).select2();
		}
				
		if ($('.svyNeedsInit', newRow).length > 0) {
			//svyInit($('.svyNeedsInit', newRow));
		}
	}
	
	if (window.REDIPS && REDIPS.drag) { //after the row is inserted re init the drag if sortable
		REDIPS.drag.init();
	}
	
	return df_subformLabelSepChar + subformCode + df_subformLabelSepChar + newindex
}

var df_pendingCallbacksQueue = new Array()
var df_isCallbackInProgress = false
var df_pendingWaitGraphicTimeout = null
var df_waitGraphicIframe = null
function df_Callback(field, param, subformCode, rowIndex) {
	// sends a callback to the server, ensuring that only one callback is in progress at a time (ie syncronous callbacks)

	//if (df_pendingCallbacksQueue.length > 0) { // 21 may 2007 - MN fixed this callback queuing bug
	if (df_isCallbackInProgress) {
		//callbacksPending[callbacksPending.length] = new Array(p1,p2,p3)
		df_pendingCallbacksQueue.push(new Array(field, param, subformCode, rowIndex))    // add to queue
	} else {
		df_CallbackNow(field, param, subformCode, rowIndex)
		// show waiting graphic
		if (!df_pendingWaitGraphicTimeout) {
			df_pendingWaitGraphicTimeout = window.setTimeout("df_ShowCallbackWaitGraphic()", 500)
		}
	}
}
function df_CallbackFinished() {
	// used internally - called by iframe onload
	if (df_pendingCallbacksQueue.length > 0) {
		params = df_pendingCallbacksQueue.shift()    // get first element and remove from queue
		df_CallbackNow(params[0], params[1], params[2], params[3])
	} else {
		// set global javascript variable so submit buttons can wait for callback completion
		df_isCallbackInProgress = false
		// cancel waiting graphic
		df_HideCallbackWaitGraphic()
	}
}
function df_ShowCallbackWaitGraphic() {
	// show wait box if function exists on page
	return //TODO - not working yet

	// show graphic to user
	if (df_waitGraphicIframe) {
		// reuse existing iframe
		df_waitGraphicIframe.style.top = 200
		df_waitGraphicIframe.style.display = ""
	} else {
		// create the iframe
		var div = document.getElementById("cf_WaitGraphic")
		if (div) {
			var iframe = document.createElement("iframe")
			iframe.style.position = "absolute"
			iframe.style.top = 200
			iframe.style.left = 200
			iframe.style.zIndex = 5
			iframe.style.border = "1px solid black"
			iframe.setAttribute('id', 'df_WaitGraphicIframe');
			df_waitGraphicIframe = document.body.appendChild(iframe)
			if (df_waitGraphicIframe.contentDocument) {
				// For NS6
				IFrameDoc = df_waitGraphicIframe.contentDocument;
			} else if (df_waitGraphicIframe.contentWindow) {
				// For IE5.5 and IE6
				IFrameDoc = df_waitGraphicIframe.contentWindow.document;
			} else if (df_waitGraphicIframe.document) {
				// For IE5
				IFrameDoc = df_waitGraphicIframe.document;
			} else {
				return true;
			}
			IFrameDoc.innerHTML = div.innerHTML
		}
	}
}
function df_HideCallbackWaitGraphic() {
	return //TODO - not working yet

	// hide graphic
	//var div = document.getElementById("cf_WaitGraphic")
	if (df_waitGraphicIframe) {
		df_waitGraphicIframe.style.display = "none"
		//df_waitGraphicIframe.style.display = "none"
	}
	// cancel timer of graphic
	if (df_pendingWaitGraphicTimeout) {
		window.clearTimeout(df_pendingWaitGraphicTimeout)
	}
}
function df_CallbackNow(field, param, subformCode, rowIndex) {
	// used internally - sends a callback to the server, killing any other callback that may be in progress at the time
	var fieldName, form, tempRowIndex

	if (typeof (field) === "function") {
		// allow just putting any function on the stack, if so just call it
		var func = field;
		var whenDone = df_CallbackFinished;
		func(whenDone);
		return;
	}

	field = df_GetField(field, subformCode, rowIndex)
	fieldName = field.name
	form = field.form

	/*		
			if (typeof(field)=='string') {
				// field is just a string
				// if using subforms you must supply the full field name with suffix
				fieldName = field
				form = document.form
				field = form[fieldName]
				if (!field && subformCode && rowIndex) {
					// see if field exists given subformCode,rowIndex exist
					fieldName = fieldName +df_subformLabelSepChar+ subformCode +df_subformLabelSepChar+ rowIndex
					field = form[fieldName]
				}
				if (!field) {
					alert("SAVVY ERROR: df_Callback - field name not found. If using subforms you must supply the full field name with suffix.")
				}
			} else {
				// field is a field object
				fieldName = field.name
				form = field.form
			}
			// see if field has subformCode attribs and add if not
			if (!field.df_subformCode) {
				fieldNamePartArray = fieldName.split(df_subformLabelSepChar)
				if (fieldNamePartArray.length > 2) {
					// yes - field looks like it has subformcode and rowindex
					// this tricky logic is to allow database fieldnames to contain underscores
					tempRowIndex = fieldNamePartArray[fieldNamePartArray.length-1]
					if (!isNaN(parseInt(tempRowIndex, 10))) {
						// yes - its really a rowindex
						// may as well add the attibutes to the field for next time
						field.df_rowIndex = tempRowIndex
						field.df_subformCode = fieldNamePartArray[fieldNamePartArray.length-2]
						suffixLen = (df_subformLabelSepChar+field.df_subformCode+df_subformLabelSepChar+field.df_rowIndex).length
						field.df_fieldName = fieldName.substring(0, fieldName.length-suffixLen)
					}
				}
			}
			*/
	if (field.df_fieldName)
		callbackFieldName = field.df_fieldName
	else
		callbackFieldName = fieldName
	if (param == undefined) param = ""
	if (!subformCode) subformCode = field.df_subformCode
	if (subformCode == undefined) subformCode = ""
	if (!rowIndex) rowIndex = field.df_rowIndex
	if (rowIndex == undefined) rowIndex = ""

	// set global javascript variable so submit buttons can wait for callback completion
	df_isCallbackInProgress = true

	form['df_callbackfield'].value = callbackFieldName
	form['df_callbackparam'].value = param
	form['df_callbacksubformcode'].value = subformCode
	form['df_callbackrowindex'].value = rowIndex

	form['df_mode'].value = 'callback'
	form.target = "df_hiddeniframe"
	df_BeforeSubmit()		// 20080218 mn - added
	form.submit()
	form.target = "_self"
	form['df_mode'].value = 'save'
}

function df_SubmitForm(buttonObj, funcNameScript) {
	// saves form, making sure any callbacks have finished first
	// buttonObj (optional) is the button that user clicked
	// funcNameScript (optional) is an extra piece of script to execute - it is passed to eval()
	if (buttonObj) {
		// buttonObj is optional but if exists then check it is a button
		if (buttonObj.type && (buttonObj.type == "button" || buttonObj.type == "submit")) {
			window.df_SubmitForm_buttonObj = buttonObj
		} else {
			alert("Savvy CMS Error: df_SubmitForm called with incorrect param. Should be buttonObj. Param was of type: " + buttonObj.type)
			debugger
		}
	} else {
		window.df_SubmitForm_buttonObj = null
	}
	window.df_SubmitForm_funcNameScript = funcNameScript
	df_SubmitForm_Internal()
}

function df_SubmitForm_Internal() {
	var form = document.form
	if (jQuery && window.df_SubmitForm_buttonObj && window.df_SubmitForm_buttonObj.form) {
		form = window.df_SubmitForm_buttonObj.form
	}

	if (!form) {
		alert("Error: Form not found")
		debugger
		return
	}
	// first check if any callbacks in progress
	if (df_isCallbackInProgress) {
		window.df_SubmitForm_TimeoutID = window.setTimeout("df_SubmitForm_Internal()", 100)
	} else {
		// no callbacks active now, so we can check form and then submit
		buttonObj = window.df_SubmitForm_buttonObj
		funcNameScript = window.df_SubmitForm_funcNameScript
		df_BeforeSubmit();
		if (CheckForm(form)) {
			if (buttonObj) buttonObj.disabled = true
			if (funcNameScript) {
				eval(funcNameScript)
			}

			// show wait box if function exists on page
			if (window.SetWaitboxMessage) {
				SetWaitboxMessage("Saving - please wait.")
				ShowWaitbox()
			}

			// remember text of button to make it easy to see which button pressed
			if (buttonObj) form["df_savebutton"].value = df_Trim(buttonObj.value)

			// don't show warning about saving
			window.df_warnNotSaved = false
			window.onbeforeunload = null

			form.submit()
		}
	}
}

function df_CancelFormSubmit() {
	// called by user code to stop a form being submitted in the case where a callback validation fails
	// all callback validations should call this
	window.clearTimeout(window.df_SubmitForm_TimeoutID)
}

function df_CancelButtonClick() {
	// don't show warning about saving
	window.df_warnNotSaved = false
	window.onbeforeunload = null
	// go to prev page
	eval(df_returnpageJs)
}

function df_AddLoadEvent(func) {
	// adds a function to the body onload without killing any existing onload
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	} else {
		window.onload = function () {
			oldonload();
			func();
		}
	}
}


function StripNumber(str) {
	// Pass in a string or a field object. Returns a string which contains only digits (and . or -) or empty string if no digits were found.
	var str2, aChar, i, donePoint
	if (str) {
		if (str.value) {
			str = str.value
		}
		if (typeof (str) == "function") {
			alert("Savvy CMS Error: StripNumber() parameter must be a string or a field. It is a 'function'.")
			debugger
			return ""
		}
		if (typeof (str) == "object" && typeof (str.value) == "undefined") {
			alert("Savvy CMS Error: StripNumber() parameter must be a string or a field. It is an 'object'. Possible cause: You may have two form elements with the same name.")
			return ""
		}
		str2 = ""
		// ignore all but digits, minuses and decimal points.
		for (i = 0; i < str.length; i++) {
			aChar = str.substring(i, i + 1);
			if (aChar >= "0" && aChar <= "9") {
				str2 += aChar;
			} else if (aChar == "." && !donePoint) {
				str2 += aChar;
				donePoint = true
			} else if (aChar == "-" && str2.length == 0) {
				str2 += aChar;
			} else if (aChar == "(" && str2.length == 0) {
				str2 += "-";
			}
		}
		if (str2 == "." || str2 == "-") {
			str2 = ""
		}
		return str2
	} else {
		return ""
	}
}


function CheckNumberField(fieldObj, decimalPlaces, allowNegative, groupDigits) {
	if (fieldObj.value.length == 0) return true
	//	fieldObj.value = df_FmtNumber(fieldObj)
	fieldObj.value = df_FmtNumber(fieldObj, decimalPlaces, groupDigits)
	if (StripNumber(fieldObj.value) != fieldObj.value && df_FmtNumber(fieldObj) != fieldObj.value) { alert('The value in this field should be a number, without any other characters. Please change the value.'); return false }
	if (isNaN(parseFloat(fieldObj.value))) { alert('The value in this field should be a number, without any other characters. Please change the value.'); return false }
	if (decimalPlaces == 0 && isNaN(parseInt(fieldObj.value, 10))) { alert('The value in this field should be a whole number, without any decimal places or other characters. Please change the value.'); return false }
	var hasDecimalPoint = (fieldObj.value.indexOf('.') > -1)
	if (hasDecimalPoint && decimalPlaces == 0) { alert('The value in this field should be a whole number, without any decimal places. Please change the value.'); return false }
	var decimalPart = ''
	if (hasDecimalPoint) decimalPart = fieldObj.value.split('.')[1]
	if (decimalPlaces != -1 && decimalPart.length > decimalPlaces) { alert('The value in this field is limited to ' + decimalPlaces + ' decimal places. Please change the value.'); return false }
	if (!allowNegative && (parseFloat(fieldObj.value) < 0.0)) { alert('The value in this field should be a positive number. Please change the value.'); return false }
	// pad decimals with trailing zeros
	if (decimalPlaces > 0) {
		//decimalPart = decimalPart.substring(0, decimalPlaces)
		var i, extraZeros
		extraZeros = decimalPlaces - decimalPart.length
		for (i = 0; i < extraZeros; i++)
			decimalPart = decimalPart + '0'
		fieldObj.value = fieldObj.value.split('.')[0] + '.' + decimalPart
	}
	return true
}

function CheckTimeEntryField(fieldObj) {
	if (fieldObj.value.length == 0) return true

	if (fieldObj.value.length > 10) {
		//alert('too long')
		var newval = fieldObj.value.replace(new RegExp("\n", "gm"), "")
		newval = newval.replace(new RegExp(".*?([0-9][0-9]:[0-9][0-9][a|p]).*", "gi"), "$1")
		fieldObj.value = newval
	}

	if (fieldObj.value.toLowerCase() == "n" || fieldObj.value.toLowerCase() == "now") {
		var hrs = (new Date).getHours()
		var mins = (new Date).getMinutes()
		fieldObj.value = df_FmtTime(hrs + ":" + mins)
	}
	if (fieldObj.value.toLowerCase().substring(0, 2) == "n-") {
		var hrs = (new Date).getHours()
		var mins = (new Date).getMinutes()
		var minus = parseInt(fieldObj.value.replace("n-", ""))
		mins -= minus
		if (mins < 0) {
			hrs--;
			mins += 60;
		}
		fieldObj.value = df_FmtTime(hrs + ":" + mins)
	}
	if (fieldObj.value.toLowerCase().substring(0, 2) == "n+") {
		var hrs = (new Date).getHours()
		var mins = (new Date).getMinutes()
		var plus = parseInt(fieldObj.value.replace("n+", ""))
		mins += plus
		if (mins > 60) {
			hrs++;
			mins -= 60;
		}
		fieldObj.value = df_FmtTime(hrs + ":" + mins)
	}
	var newValue = df_FmtTime(fieldObj)
	if (newValue.length == 0) {
		// there are now no characters that are valid
		alert('The value in this field should be a time, without any other characters. Please change the value.')
		return false
	}
	// apply formatting to field
	fieldObj.value = newValue
	return true
}

function CheckURLField(fieldObj) {
	if (fieldObj.value.length == 0) return true
	var linkUrl = fieldObj.value
	if (linkUrl.indexOf("http://") == 0 || linkUrl.indexOf("file://") == 0 || linkUrl.indexOf("rtsp://") == 0 || linkUrl.indexOf("https://") == 0 || linkUrl.indexOf("mailto:") == 0 || linkUrl.indexOf("asfunction:") == 0) {
		// link is ok
	} else {
		// add http prefix
		fieldObj.value = 'http://' + fieldObj.value
	}

	return true
}

function df_FmtTime(strOrField) {
	// str can be a time string OR a field reference
	var str, field
	if (strOrField.value) {
		field = strOrField
		str = strOrField.value + ""
	} else {
		field = null
		str = strOrField + ""
	}
	str = str.toLowerCase()
	var hrs, mins, ampm, newStr
	ampm = ""
	if (str.indexOf(":") > -1) {
		hrs = str.split(":")[0]
		mins = str.split(":")[1]
		hrs = parseInt(hrs, 10)

		// check if mins has am or pm already
		if (mins.indexOf("a") > -1) mins = mins.substring(0, mins.indexOf("a"))
		if (mins.indexOf("p") > -1) mins = mins.substring(0, mins.indexOf("p"))
		//alert(mins)
		mins = parseInt(mins, 10)
	} else {
		hrs = parseInt(str, 10)
		mins = 0
		if (hrs > 100) {
			mins = hrs % 100
			hrs = (hrs - mins) / 100
		}
	}
	if (isNaN(parseInt(hrs, 10))) {
		// not valid
		return ""
	}
	if (isNaN(parseInt(mins, 10))) {
		// not valid
		return ""
	}
	if (mins > 59) {
		mins = 59;
	}
	if (hrs >= 24) {
		hrs = 23
		mins = 59
	}
	if (str.indexOf("a") > -1) {
		ampm = "am"
	} else if (str.indexOf("p") > -1) {
		ampm = "pm"
	} else if (hrs > 24) {
		if (field) {
			field.style.color = "red"
		}
	} else if (hrs > 12) {
		hrs -= 12
		ampm = "pm"
	} else if (hrs == 12) {
		ampm = "pm"
	} else if (hrs == 0) {
		hrs = 12
		ampm = "am"
	} else {
		ampm = "am"
	}
	newStr = hrs + ":" + df_Right("0" + mins, 2) + ampm
	if (field) {
		field.df_hours = hrs
		field.df_minutes = mins
		field.df_ampm = ampm
	}
	return newStr
}

function df_GetTimeEntryFieldValue(fieldObj) {
	// returns time in minutes since midnight
	var str = F$(fieldObj).value
	var hrs, mins, ampm, newStr
	if (str.indexOf(":") > -1) {
		hrs = str.split(":")[0]
		mins = str.split(":")[1]
		hrs = parseInt(hrs, 10)
		// check if mins has am or pm already
		if (mins.indexOf("a") > -1) mins = mins.substring(0, mins.indexOf("a"))
		if (mins.indexOf("p") > -1) mins = mins.substring(0, mins.indexOf("p"))

		mins = parseInt(mins, 10)
	} else {
		hrs = parseInt(str, 10)
		mins = 0
	}
	if (isNaN(parseInt(hrs, 10))) {
		// not valid
		return ""
	}
	if (isNaN(parseInt(mins, 10))) {
		// not valid
		return ""
	}
	if (str.indexOf("a") > -1) {
		ampm = "am"
	} else if (str.indexOf("p") > -1) {
		ampm = "pm"
	} else if (hrs > 12) {
		hrs -= 12
		ampm = "pm"
	} else if (hrs == 12) {
		ampm = "pm"
	} else if (hrs == 0) {
		hrs = 12
		ampm = "am"
	} else {
		ampm = "am"
	}
	// take hours forward to 24 hour time
	if (hrs == 12) {
		hrs = 0
	}
	if (ampm == "pm") {
		hrs += 12
	}
	return hrs * 60 + mins
}

function df_SetTimeEntryFieldValue(fieldObj, newValueMins) {
	// newValueMins is minutes since midnight
	var hrs = Math.floor(newValueMins / 60)
	var mins = Math.floor(newValueMins % 60)
	fieldObj.value = df_FmtTime(hrs + ":" + mins)
}

function LimitLength(ctrl, chars) {
	if (chars <= 0) return true
	var isOK = (ctrl.value.length <= chars)
	if (!isOK)
		alert('This text field cannot contain more than ' + chars + ' characters.')
	return isOK
}
var df_isPostbackUnderway = false
function Postback(field, param) {
	//note: this doesnt work with multiple forms on the page (assumes the form named 'form')
	if (df_isPostbackUnderway) return   // ensure can't postback twice (browser can take a second or so before actually posting form)
	df_isPostbackUnderway = true
	if (field != undefined) {
		if (field.name) field = field.name
		document.form['df_postbackfield'].value = field
	}
	if (param != undefined) document.form['df_postbackparam'].value = param
	document.form['df_mode'].value = 'postback'
	if (document.form['df_mode'].value != 'postback') {
		alert("Postback Error: could not set mode.")
		debugger
	}
	// don't show warning about saving
	window.df_warnNotSaved = false
	window.onbeforeunload = null
	document.form.submit()
}
function df_BeforeSubmit() {
	//note: this doesnt work with multiple forms on the page (assumes the form named 'form')
	var f = document.form
	if (f["df_fieldlist"]) {    // this hidden field only exists if using file upload component
		var str = ''
		var len = f.length
		for (var i = 0; i < len; i++) {
			if (str.length > 0) str += ','
			str += f[i].name
		}
		f["df_fieldlist"].value = str
	}
}

function df_Right(str, count) {
	return str.substr(str.length - count, str.substr.length)
}

//String.prototype.trim = function() {
//    return this.replace(/(^\s*)|(\s*$)/g, "");
//}

function df_Trim(str) {
	// Use a regular expression to replace leading and trailing 
	// spaces with the empty string
	return str.replace(/(^\s*)|(\s*$)/g, "");
}

function df_Replace(str, findStr, replaceWith) {
	// replaces all occurrences of a regex pattern with another substring and returns a new string (does not modify original string)
	// it ignores case
	var result = str + ""
	while (result.indexOf(findStr) != -1) {
		result = result.replace(findStr, replaceWith)
	}
	return result
}

function df_IsBlank(str) {
	// returns true if the string is empty "" or contains only spaces
	var str2, aChar, i
	str2 = ""
	// scan for non-space characters.
	for (i = 0; i < str.length; i++) {
		aChar = str.substring(i, i + 1);
		if (aChar != " ")
			return false;
	}
	return true;
}

function df_GetText(fieldObjOrName) {
	var fieldObj = df_GetField(fieldObjOrName)
	if (fieldObj.type == "select-one")
		return fieldObj.options[fieldObj.selectedIndex].text
	else
		return df_GetValue(fieldObj);
}

function df_SetText(fieldObjOrName, newValue) {
	var fieldObj = df_GetField(fieldObjOrName)
	if (fieldObj.type == "select-one")
		df_SetDropDownText(fieldObj, newValue)
	else
		df_SetValue(fieldObj, newValue);
}

function df_GetValue(fieldObjOrName) {
	var fieldType, fieldObj
	if (typeof (fieldObjOrName) == "string") {
		// can be a string value which is the field name instead of the field object
		//fieldObj = document.form[fieldObjOrName]
		fieldObj = df_GetField(fieldObjOrName)
		if (!fieldObj) {
			alert("df_GetValue Error: form field not found. [" + fieldObjOrName + "]")
			debugger
			return
		}
	} else if (fieldObjOrName === undefined) {
		alert("df_GetValue: field parameter is undefined")
	} else {
		fieldObj = fieldObjOrName
	}
	if (!fieldObj) {
		alert("df_GetValue Error: invalid param type. [" + fieldObjOrName + "][" + fieldObj + "]")
		debugger
	} else if (fieldObj.type) {
		fieldType = fieldObj.type
	} else if (fieldObj.length && fieldObj.length > 0 && fieldObj[0] && fieldObj[0].type) {
		// it looks like an array of fields (eg checkbox group)
		fieldType = fieldObj[0].type
	} else {
		alert("df_GetValue Error: invalid param type.")
		debugger
	}
	if (fieldType == "select-one")
		return fieldObj.options[fieldObj.selectedIndex == -1 ? 0 : fieldObj.selectedIndex].value         // fix IE9 literal interpretation of HTML spec
	else if (fieldType == "radio")
		return df_GetRadioValue(fieldObj)
	else if (fieldType == "checkbox")
		// ordinary input type=checkbox - returns checkbox value which will be submitted
		return df_GetCheckboxValue(fieldObj)
	else if (fieldObj.form["chbox_" + fieldObj.name])
		// beweb checkbox with accompanying hidden field - returns true or false
		return fieldObj.form["chbox_" + fieldObj.name].checked
	else if (fieldObj.df_fieldType == "Number")
		return df_GetNumberFieldValue(fieldObj)
	else
		return fieldObj.value;
}

function df_SetValue(fieldObjOrName, newValue) {
	var fieldType, fieldObj
	if (typeof (fieldObjOrName) == "string") {
		// can be a string value which is the field name instead of the field object
		//fieldObj = document.form[fieldObjOrName]
		fieldObj = df_GetField(fieldObjOrName)
		if (!fieldObj) {
			alert("df_SetValue Error: form field not found. [" + fieldObjOrName + "]")
			debugger
			return
		}
	} else {
		fieldObj = fieldObjOrName
	}
	if (!fieldObj) {
		alert("df_SetValue Error: fieldObj does not exist [" + fieldObjOrName + "]")
		debugger
	} else if (fieldObj.type) {
		fieldType = fieldObj.type
	} else if (fieldObj.length && fieldObj.length > 0 && fieldObj[0] && fieldObj[0].type) {
		// it looks like an array of fields (eg checkbox group)
		fieldType = fieldObj[0].type
	} else {
		alert("df_SetValue Error: invalid param type.")
		debugger
	}
	if (!fieldObj.form && fieldObj.length) {
		alert("df_SetValue Error: The supplied object is not a form field. It could be an array of form fields. Do you have several fields named the same thing? [" + fieldObjOrName + "]")
	}
	if (fieldType == "select-one")
		df_SetDropDownValue(fieldObj, newValue)
	else if (fieldObj.form["chbox_" + fieldObj.name])
		df_SetCheckboxValue(fieldObj, newValue)
	else if (fieldType == "hidden")
		df_SetHiddenValue(fieldObj, newValue)
	else if (fieldObj.df_fieldType == "Number")
		df_SetNumberFieldValue(fieldObj, newValue)
	else if (fieldType == "radio")
		return df_SetRadioValue(fieldObj, newValue)
	else if (fieldType == "checkbox")
		// ordinary input type=checkbox - returns checkbox value which will be submitted
		return df_SetCheckboxValue(fieldObj)
	else
		fieldObj.value = newValue;
}

function df_SetNumberFieldValue(fieldObj, newValue) {
	fieldObj = df_GetField(fieldObj);
	if (newValue + "" === "") {
		fieldObj.value = ""
	} else {
		var formattedNum = StripNumber(newValue + "")
		fieldObj.value = formattedNum
		fieldObj.value = df_FmtNumber(fieldObj)
		//		formattedNum = df_FmtNumber(formattedNum, fieldObj.df_allowNegative, (fieldObj.df_allowNegative=="true"), (fieldObj.df_groupDigits=="true"))
		//		fieldObj.value = formattedNum
	}
}

function df_SetHiddenValue(fieldObj, newValue) {
	fieldObj.value = newValue;
	var fieldName = fieldObj.name
	var displayDiv = document.getElementById(fieldName + "_Display")
	if (displayDiv) {
		if (displayDiv.length) { displayDiv = displayDiv[0] }
		displayDiv.innerHTML = newValue
	}
}

function df_GetRadioValue(radioControl) {
	// returns the value of the radio button in the radio group that is selected, or empty string if none selected
	var radioValue = ""
	if (!radioControl.length) {
		// if radio button is part of a group, grab whole group
		radioControl = radioControl.form[radioControl.name];
	}
	if (radioControl.length) {
		// it is a radio group
		for (var scan = 0; scan < radioControl.length; scan++) {
			if (radioControl[scan].checked) {
				radioValue = radioControl[scan].value
				break;
			}
		}
	} else {
		// a single radio button - only return its value if it is selected
		if (radioControl.checked) {
			radioValue = radioControl.value
		} else {
			radioValue = ""
		}
	}
	return radioValue
}

function df_SetRadioValue(radioControl, newValue) {
	// sets the value of the radio button in the radio group that is selected, or empty string if none selected
	// this also works for standard browser checkboxes
	if (!radioControl.length) {
		// if radio button is part of a group, grab whole group
		radioControl = radioControl.form[radioControl.name];
	}
	if (radioControl.length) {
		// it is a radio group
		for (var scan = 0; scan < radioControl.length; scan++) {
			if (radioControl[scan].value == newValue) {
				radioControl[scan].checked = true;
			} else {
				radioControl[scan].checked = false;
			}
		}
	} else {
		// a single radio button - only return its value if it is selected
		if (radioControl.value == newValue) {
			radioControl.checked = true;
		} else {
			radioControl.checked = false;
		}
	}
}

function df_GetCheckboxValue(checkboxControl) {
	// if checkbox is checked, returns value of the checkbox or empty string if not checked
	// if multiple checkboxes with same name, returns comma-space separated list of the values of the checkboxes which are checked
	var checkboxValue = ""
	if (!checkboxControl.length) {
		// if checkbox is part of a group, grab whole group
		checkboxControl = checkboxControl.form[checkboxControl.name];
	}
	if (checkboxControl.length) {
		// multiple checkboxes with same name (ie a checkbox group)
		for (var scan = 0; scan < checkboxControl.length; scan++) {
			if (checkboxControl[scan].checked) {
				if (checkboxValue != "") checkboxValue += ", "
				checkboxValue += checkboxControl[scan].value
			}
		}
	} else {
		// single checkbox
		if (checkboxControl.checked) {
			checkboxValue = checkboxControl.value
		} else {
			checkboxValue = ""
		}
	}
	return checkboxValue
}

function df_SetCheckboxValue(fieldObj, newValue) {
	// newValue must be true or false
	var field = fieldObj
	if (field.form["chbox_" + field.name]) {
		// this is a standard savvy checkbox which is actually a hidden field with the correct name, plus a checkbox with chbox_ prefix
		if (typeof (newValue) == "string") {
			newValue = newValue.toLowerCase()
			if (newValue == "true" || newValue == "1") {
				newValue = true
			} else if (newValue == "false" || newValue == "0" || newValue == "") {
				newValue = false
			} else {
				alert("df_SetCheckboxValue: invalid value for a boolean [" + newValue + "]")
				newValue = false
			}
		}
		field.value = (newValue == true) ? "1" : "0"
		field.form["chbox_" + field.name].checked = newValue
	} else if (field.name.indexOf("chbox_") == 0) {  // starts with chbox
		// this is a standard savvy checkbox which is actually a hidden field with the correct name, plus a checkbox with chbox_ prefix
		var hiddenfield = field.form[field.name.replace("chbox_", "")]
		hiddenfield.value = (newValue == true) ? "1" : "0"
		field.checked = newValue
	} else {
		if (!field.length && field.form) {
			// if checkbox is part of a group, grab whole group
			field = field.form[field.name];
		}
		if (field.length && fieldObj.length > 0) {
			// checkbox array
			// match exact value and also match any where value is a comma delimited string
			for (var scan = 0; scan < fieldObj.length; scan++) {
				if (fieldObj[scan].value == newValue || ("," + newValue + ",").indexOf("," + fieldObj[scan].value + ",") > -1 || (", " + newValue + ",").indexOf(", " + fieldObj[scan].value + ",") > -1) {
					fieldObj[scan].checked = true;
				} else {
					fieldObj[scan].checked = false;
				}
			}

			//alert("Savvy Error: df_SetCheckboxValue expects a checkbox and was passed an array. Perhaps you have more than one checkbox with the same name.")
			//debugger
		} else if (field.type && field.type == "checkbox") {
			field.checked = newValue
		} else {
			alert("Savvy Error: df_SetCheckboxValue expects a checkbox and was passed something else [" + field + "].")
			debugger
		}
	}
}

function df_FocusFirstField() {
	// todo: check for tabindex?
	try {
		var bFound = false;
		for (f = 0; f < document.forms.length; f++) {
			for (i = 0; i < document.forms[f].length; i++) {
				// now selects text of first field if possible (not just focus)
				var field = document.forms[f][i];
				if (field.type != "hidden" && document.forms[f][i].type != "button") {
					if (field.disabled != true) {
						field.focus();
						if (field.select) field.select()
						var bFound = true;
					}
				}
				if (bFound == true) {
					break;
				}
			}
			if (bFound == true) {
				break;
			}
		}
	} catch (e) {
		// do nothing
	}
}

function df_SaveAndRefresh() {
	var form = document.forms["form"]
	if (form["df_returnpage"].value.indexOf("self:") != 0) {
		form["df_returnpage"].value = "self:" + form["df_returnpage"].value
	}
	df_SubmitForm()
}
function df_SaveAndGoURL(url, ask) {
	//note: this assumes the form named 'form'
	var form = document.form
	form["df_returnpage"].value = url
	df_SubmitForm()
}

// copydown
function df_CopyValueAbove(field) {
	var fieldAbove = df_GetFieldAbove(field)
	if (fieldAbove) {
		df_SetValue(field, df_GetValue(fieldAbove))
	} else {
		df_SetValue(field, "")
	}
}

function df_SetAutoSave(seconds) {
	// automatically save the record(s) on this form at given interval (in seconds)
	// note that this can change the record from new to existing
	// you might want to save some kind of status field so you know whether the user clicked save or it was just autosaved
	window.setInterval(df_AutoSave, seconds * 1000)
}

function df_AutoSave() {
	// don't show warning about saving
	var mode = df_GetValue("df_mode")
	if (mode != "view") {
		window.df_warnNotSaved = false
		window.onbeforeunload = null
		window.status = "Autosaving..."
		df_Callback('df_tablename', 'autosave', '')
		//window.status = "Autosaving..."
	}
}

// shortcut functions
var F$ = df_GetField

function V$(field, newValue) {
	if (newValue === undefined) {
		return df_GetValue(field)
	} else {
		df_SetValue(field, newValue)
	}
}

function df_SetDisplay(element, newValue) {
	if (typeof element == "string") {
		element = E$(element)
	}
	if (newValue) {
		element.style.display = "none"
	} else {
		element.style.display = ""
	}
}

function df_GetDateFieldValue(field) {
	// return field value as a date type
	var value = df_GetValue(field)
	if (value == "") {
		return ""
	} else {
		value = df_Replace(value, "-", " ")
		return new Date(value)
	}
}

function df_GetMonthEnd(dateObject) {
	var newDateObject = new Date(dateObject)
	newDateObject.setDate(1)
	newDateObject.setMonth(newDateObject.getMonth() + 1)
	newDateObject.setDate(newDateObject.getDate() - 1)
	return newDateObject
}

function df_ForEachRow(subformCode, func) {
	// call a function or exec some code for each row
	// func is passed the status field object and a suffix eg MyFunc(field,suffix) or df_ForEachRow(function(field,suffix){ ... })
	if (subformCode === undefined) alert("df_ForEachRow(subformCode, func): Error - subformCode is undefined")
	if (subformCode == "") alert("df_ForEachRow(subformCode, func): Error - subformCode is blank")
	if (func === undefined) alert("df_ForEachRow(subformCode, func): Error - func is undefined")
	var rows = df_GetFields("df_status", subformCode)
	for (var rowIndex in rows) {
		var statusField = rows[rowIndex]
		//		func(statusField, df_currentSubformCode)
		//func(statusField, statusField.getAttribute("df_suffix"))
		func(statusField, statusField.df_suffix)
	}
}

function df_RoundCents(num) {
	return Math.round(num * 100.0) / 100.0
}

function df_FixRounding(num) {
	return Math.round(num * 1000000.0) / 1000000.0
}

function SplitTitleCase(str) {
	var returnValue = str + ""
	returnValue = returnValue.replace("_day", " Day")
	returnValue = returnValue.replace("_month", " Month")
	returnValue = returnValue.replace("_year", " Year")
	returnValue = returnValue.replace(/([a-z])([A-Z])/g, '$1 $2');
	return returnValue;
}

function df_SafeDivide(numerator, denominator) {
	// avoid divide by zero errors - just return zero in this case
	// also assume null or empty string are equivalent to zero
	if (denominator == 0 || denominator + "" == "" || numerator == 0 || numerator + "" == "") {
		return 0
	} else {
		return numerator / denominator
	}
}

function df_RowCount(subformCode) {
	var rowCount
	if (jQuery !== undefined) {
		rowCount = jQuery("#df_SubformTable_" + subformCode + " tr.svySubformRow:visible").length
	} else {
		// note: this is different in the .NET version - it is zero based where the ASP version is one based
		rowCount = df_GetMaxRowIndex(subformCode)
	}
	return rowCount
}

function df_AddRowIfNone(subformCode) {
	// note: this is different in the .NET version - it is zero based where the ASP version is one based
	if (jQuery !== undefined) {
		var rowCount = jQuery("#df_SubformTable_" + subformCode + " tr.svySubformRow:visible").length;
		if (rowCount == 0) {
			return df_AddRow(subformCode)
		}
	} else {
		if (df_GetMaxRowIndex(subformCode) == 0) {
			return df_AddRow(subformCode)
		}
	}
	return null;
}

function df_FmtDate(dateObj) {
	if (typeof dateObj === "string") {
		if (dateObj.indexOf("/Date(") == 0) {
			// ms ajax style date string
			dateObj = new Date(parseInt(dateObj.replace("/Date(", "").replace(")/", "")));
		}else{
			dateObj = new Date(dateObj);
		}
	}
	var dateStr = (dateObj == null) ? '' : dateObj.getDate() + ' ' + df_FmtMonth(dateObj.getMonth()) + ' ' + dateObj.getFullYear();
	return dateStr;
}

function df_FmtMonth(monthIndex) {
	var monthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
	return monthArr[monthIndex];
}

function df_FmtDateTime(dateObj) {
	return df_FmtDate(dateObj) + " " + df_Right("0" + dateObj.getHours(), 2) + ":" + df_Right("0" + dateObj.getMinutes(), 2);
}
