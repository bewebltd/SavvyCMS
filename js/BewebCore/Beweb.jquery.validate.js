//<!--
//<script language="javascript" type="text/javascript" src="../jquery-1.4.2/jquery-1.4.2-vsdoc.js"></script>
//-->
/// <reference path="../jquery-1.4.2/jquery-1.4.2-vsdoc.js" />

function BewebInitForm(formID) {
	// check that form exists
	if (formID.charAt(0) == "#") {
		alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nFormID parameter should not contain hash # when calling BewebInitForm(formID).")
		return
	} else if ($("#" + formID).length == 0) {
		alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nForm does not exist with ID ["+formID+"].\n\nPlease either set the form ID in the <form> tag or change the ID in the call to BewebInitForm(formID).")
		return
	}
	if (!$.browser.msie) document.form = $("#" + formID)[0]

	// setup global form variable used by beweb-cma.js (for non-IE browsers ony)
	//if (!document.form) document.form = document.getElementById(formID)

	// see if date picker is needed
	if ($("input.datetime,input.date").length > 0) {
		if ($("input.datetime").datepicker || $("input.date").datepicker) {
			// setup date pickers
			//note: perhaps next two lines are not needed
		//	$("input.datetime").removeClass("hasDatepicker").datepicker({ dateFormat: 'd M yy', duration: '', showTime: true, changeYear:'true',changeMonth:'true',constrainInput: false, stepMinutes: 1, stepHours: 1, altTimeField: '', time24h: false });
		//	$("input.date").removeClass("hasDatepicker").datepicker({ dateFormat: 'd M yy', duration: '', changeYear: 'true', changeMonth: 'true' });
			
			// apply minyear/maxyear to each date picker
			//$("input.date").each(function(){ BewebApplyDatePicker(this) });
			//$("input.datetime").each(function(){ BewebApplyDatePicker(this) });
			BewebInitDatePickers();
			//binds onchange to revalidate
			//$("input.datetime").bind("change", function () { $(this).valid(); })    // trigger validation to re-validate
			//$("input.date").bind("change", function () { $(this).valid(); })    // trigger validation to re-validate
		} else {
			alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nThis page contains Date input fields but jQuery.datepicker is not loaded. You need to include several js files and skin files.");
		}
	}

		// see if clock picker is needed
	if ($("input.time").length > 0) {
		if (!$.fn.clockpick) {
			alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nThis page contains Time input fields but jQuery.clockpick is not loaded. You need to IncludeClockPick.")
		}
	}
	
	// see if beweb-cma.js is needed
	if ($("input.datetime,input.date,input.number,input.url").length > 0) {
		if (typeof(df_FmtNumber)!='undefined') {
			// setup number field autocorrect validation/formatting
			$("input.number").live("blur", function () { if (this.value) this.value = df_FmtNumber(this) })
		} else {
			alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nThis page contains Number or Date input fields but beweb-cma.js is not included.")
		}
	}


	// see if any picture fields
	if ($("input.picture").length > 0) {
		if (!window.handleSelectPicture && !window.svyHandleSelectPicture) {
			alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nThis page contains Picture Upload fields but forms.js is not included.")
			return			
		}
	}

	// todo - our other stuff here
	
	// setup date field autocorrect formatting
	$("input.date,input.datetime").live("blur", function () { if (this.value) CheckDateInput(this, false); $(this).valid(); })
	// setup time field autocorrect formatting
	$("input.time").live("blur", function () { if (this.value) CheckTimeEntryField(this); $(this).valid(); })	
	// setup URL field autocorrect formatting
	$("input.url").live("blur", function () { if (this.value) CheckURLField(this); $(this).valid(); })

	// check that validate is included
	if ($.validator) {
		// setup validate
		//$.validator.classRuleSettings.date = { dateNZ: true }
		$.validator.classRuleSettings.datetime = { date: true }
		$.validator.classRuleSettings.money = { number: true }
		$.validator.classRuleSettings.integer = { integer: true }
		// note about debug setting: Prevents the form from submitting and tries to help setting up the validation with warnings about missing methods and other debug messages
	// MN 20110214 old code:	var validateOptions = { invalidHandler: BewebValidateInvalidHandler, ignore: ":hidden, .dontvalidate, .ignore", focusCleanup: true, submitHandler: BewebValidateSubmitHandler }  //debug: true
		var validateOptions = { 
			invalidHandler: BewebValidateInvalidHandler, 
			ignore: ":hidden, .dontvalidate, .ignore", 
			focusCleanup: true, 
			/*onsubmit: false,*/
			submitHandler: BewebValidateSubmitHandler,
			errorPlacement: function (error, element) {
				var findValidCol = $(element).parent().find('.dateerror')
				if (findValidCol.length==0) {
					findValidCol = $(element).parent().parent().parent().find('.validation')			//label/td/tr, then find validation class
				}
				if(findValidCol.length>0) {
					findValidCol.html(error)
				 	//error.appendTo(findValidCol);
				}else if (element.hasClass("yesno")) 
				{
					error.appendTo(element.parent("div").next());
				} else {
					error.insertAfter(element);
				}
			}
		}  //debug: true
		$("#" + formID).validate(validateOptions)
	} else {
		alert("Savvy Javascript Validation Error (Beweb.jquery.validate.js):\nThis page includes Beweb.jquery.validate.js but it also needs jquery.validate.js which is not included.")
		return
	}

	// don't use jquery validate for maxlength - just use built in maxlength
	delete $.validator.methods.maxlength;
}

function BewebValidateSubmitHandler(form) {
	// call user defined check form function if it exists.
	if (window.CheckForm) {
		if (!CheckForm(form)) {
			return false;
		}
	}
	// prevent people clicking submit twice
	$("input[type=submit],input[type=button]").click(function () { return false })
	//$("input[type=submit],input[type=button]").attr("disabled", "disabled")  -- this causes values of submitted button not to come thru becaue disabled fields are not submitted - could use hidden field instead (as per savvy classic)
	form.submit()
}

//function BewebApplyDatePicker(element) {
//	$(element).removeClass("hasDatepicker")
//	var minYear = $(element).attr("earliestyear")
//	var maxYear = $(element).attr("latestyear")
//	$(element).datepicker({ dateFormat: 'd M yy', duration: '', changeYear: 'true', changeMonth: 'true', yearRange: minYear+":"+maxYear })
//}

function BewebApplyDatePicker(element) {
	if (!$(element).hasClass("hasDatepicker")) {
		var minYear = $(element).attr("earliestyear");
		var maxYear = $(element).attr("latestyear");
		$(element).datepicker({ 
			dateFormat: 'd M yy', 
			duration: '', 
			changeYear: 'true', 
			changeMonth: 'true', 
			yearRange: minYear + ":" + maxYear, 
			showOn: "button", 
			buttonImage: websiteBaseUrl +"images/ico-calendar.gif", 
			buttonImageOnly: true,
			beforeShow: function(input, inst)
			{
				inst.dpDiv.css({marginTop: -input.offsetHeight + 'px', marginLeft: input.offsetWidth + 'px'});
			} 
		})
		$(element).bind("change", function () { $(this).valid(); })    // trigger validation to re-validate
	}
	// Note Time Picker not tested... see example below
	//	$("input.datetime").removeClass("hasDatepicker").datepicker({ dateFormat: 'd M yy', duration: '', showTime: true, changeYear:'true',changeMonth:'true',constrainInput: false, stepMinutes: 1, stepHours: 1, altTimeField: '', time24h: false });

}
function BewebInitDatePickers() {
	$("input.date:visible,input.datetime:visible").each(function () { BewebApplyDatePicker(this) });
}

function BewebValidateInvalidHandler(form, validator) {
	var errors = validator.numberOfInvalids();
	if (errors) {
		var message = errors == 1
			? 'You missed a field. It has been highlighted'
			: 'You missed ' + errors + ' fields. They have been highlighted';
		if ($("div.validation-summary-errors").length > 0) {
			$("div.validation-summary-errors").html(message);
			$("div.validation-summary-errors").show(500);
			// scroll to top so user can see div (not sure if this position is quite correct but it works for me)
			var divTop = $("div.validation-summary-errors").position().top;

			$('html').animate({ scrollTop: divTop }, 'slow');
			window.setTimeout('$("div.validation-summary-errors").hide("slow")',15000); //hide after 15 seconds
		}
	} else {
		$("div.validation-summary-errors").hide();
	}
}

// TODO: need to check if ignore: ":hidden" works - otherwise include this somewhere: if (!$(element).is(":visible")) return;

jQuery.validator.methods.url = function (value, element) {
	// from microsoft http://msdn.microsoft.com/en-us/library/ff650303.aspx but with tilde and equals added (doh!)
	value = jQuery.trim(value)
	return this.optional(element) || /^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#\~=_]*)?$/i.test(value);
}

jQuery.validator.methods.date = function (value, element) {
	return this.optional(element) || (CheckDateInput(element, false) && !/Invalid|NaN/.test(new Date(value)));
}

jQuery.validator.messages.date = "Please enter a valid date in the format dd mmm yyyy";


// Leaving this out because it's annoying when you're still typing.
// this is necessary because our dateNZ validation does autocorrection and this for some reason triggers validation before you have finished typing the date
// this could be resolved by splitting our date check function into two parts - the autocorrection which would be applied using $("#date").change(BewebDateAutoCorrect) and the actual checking
//jQuery.validator.defaults.onkeyup = function (element) {
//	return true;
////	
//}

//// custom implementation of date checker - not needed - here for example purposes
//jQuery.validator.addMethod(
//	"dateNZ",
//	function(value, element) {
//		// handle jquery validate optional property
//		if (this.optional(element)) {
//			return true
//		}

//		return CheckDateInput(element, false)
//	},
//	"Please enter a correct date"
//);

/*
* Dates
*/


function CheckDateField(fieldObj, showAlert) {
	var form = fieldObj.form
	if (form == null) form=document.forms[0]//default to first form on page
	//var fieldname = fieldObj.name
	var fieldname = fieldObj.id	 //use id as name may have changed (in RAL)
	if ($('#' + fieldname + "_day").length > 0) { // if dropdowns exist
		return CheckDateFields(
			$("#" + fieldname + "_day")[0], 
			$("#" + fieldname + "_month")[0],
			$("#" + fieldname + "_year")[0],
			$("#" + fieldname )[0],
			$("#" + fieldname + "_hour")[0],
			$("#" + fieldname + "_min")[0],
			showAlert)
	} else {
		//CheckDateInput(fieldObj)   // note: this could be change to return true only if date ok
		//return true
		alert('CheckDateField: failed to find dropdown [' + fieldname + ']')
		return CheckDateInput(fieldObj, showAlert)   // MN 20090423
	}
}

function CheckDateFields(dayField, monthField, yearField, dateField, hourField, minField, showAlert) {
	// check that day is valid for the specified month and year
	// also check that fields are not blank
	if (dayField && dayField.selectedIndex < 1) { dateField.value = ""; return true }
	if (monthField.selectedIndex < 1) { dateField.value = ""; return true }
	if (yearField.selectedIndex < 1) { dateField.value = ""; return true }
	if (hourField) {
		if (hourField.selectedIndex < 0) { dateField.value = ""; return true }
		if (minField.selectedIndex < 0) { dateField.value = ""; return true }
	}

	// check validity of date
	// NOTE: this routine uses the built-in javascript function to check validity
	var day = 1
	if (dayField && dayField.selectedIndex) {
		day = dayField[dayField.selectedIndex].value
	}
	var month = monthField[monthField.selectedIndex].value
	var monthNum = monthField.selectedIndex - 1

	//alert('yearField.selectedIndex[' + yearField.selectedIndex + ']')
	var year = yearField[yearField.selectedIndex].value
	//alert('day[' + day + ']month[' + month + ']monthNum[' + monthNum + ']year[' + year + ']')


	var datestr = day + ' ' + month + ' ' + year

	//alert('datestr1[' + datestr + ']')

	if (hourField) {
		if (hourField.selectedIndex > 0 || minField.selectedIndex > 0) {
			datestr += ' ' + hourField.options[hourField.selectedIndex].value
			datestr += ':' + minField.options[minField.selectedIndex].value
		}
	}
	//alert('datestr2[' + datestr + ']')
	var d = new Date(datestr)

	//alert('d.getMonth()[' + d.getMonth() + '], monthNum[' + monthNum + ']')
	if (d.getMonth() != monthNum) {
		// month has spilled over - therefore invalid
		dateField.value = ""
		if (showAlert) alert("This is not a valid date. Please select a new date.")
		//alert("This is not a valid date. Please select a new date.")
		dayField.focus()
		return false
	}
	dateField.value = datestr
	return true
}


function CheckDateInput(dateInput, showAlert) {
	// this takes a date value (any recognized form (ideally!)) and converts to DD MMM YYYY
	var time = ""
	var inputVal = dateInput.value;
	var allowTime = dateInput.getAttribute("showTime") == "true";
	// error check variable
	var isError = false;


	if (inputVal == "") {
		// don't do anything - no value
	}
	else {
		var d = new Date();
		// special values!
		switch (inputVal.toLowerCase()) {
			case "now": // now
			case "n": // now
				if (allowTime) {
					time = "n"
				}
			case "t": // today
				// already set to current date
				break;
			case "w": // tomorrow
				d.setDate(d.getDate() + 1)
				break;
			case "y": // yesterday
				d.setDate(d.getDate() - 1)
				break;
			default:
				// do some date processing
				var DD, MM, YY
				// check for six DIGITs. Regex makes sure they are digits - only 250407 is acceptable
				var re_1 = /^[0-9]{6}$/gi
				var m_1 = re_1.exec(inputVal)
				if (m_1 != null) {
					DD = inputVal.substr(0, 2);
					MM = inputVal.substr(2, 2);
					YY = inputVal.substr(4, 2);
				}
				else {
					var re = /([^ \/\\.-]*)[ \/\\.-]([^ \/\\.-]*)[ \/\\.-]([^ \/\\.-]*)(.*)/gi
					var m = re.exec(inputVal);
					if (m == null) {
						// regex failed
						isError = true
						break
					}
					else {
						DD = m[1];
						//alert(DD);
						MM = m[2];
						//alert(MM);
						YY = m[3];
						//alert(YY);
						time = m[4];
						if (time != "") {
							if (window.df_FmtTime) { // only if function exists (ie beweb-cma.js is included)
								time = df_FmtTime(time)
							} else {
								time = ""
							}
						}
					}
				}
				// change the year - 06 should not become 1906
				// TODO: make this dependent on max year passed to server side function
				YY = parseInt(YY, 10);
				//YY = (YY >= 20000) ? YY -= 18000 : YY 
				//YY = (YY <= 50) ? YY += 2000 : YY	//Problematic, people type 11 and get 20011
				//YY = (YY < 100) ? YY += 1900 : YY;

				// if MM is a number, convert it to the month name
				// 20080725 MN: changed code below to this, fixes bug where '030808' did not work
				if (!isNaN(MM)) {
					MM = parseInt(MM, 10);
					MM = ConvertJsMonth(MM - 1);
				}
				inputVal = DD + " " + MM + " " + YY;

				//				if(isNaN(DD) || isNaN(MM)){
				//					// either DD or MM is a word - use our fixed year though
				//					// we need to make sure year is four digit at this point
				//					//YY = (YY < 2000) ? YY += 1900 : YY;
				//					inputVal = DD + " " + MM + " " + YY;
				//				}
				//				else {
				//					// parse expects in MM/DD/YY format - we need to switch our formatting to that
				//					inputVal = MM + "-" + DD + "-" + YY;
				//				}

				if (allowTime && time != "") {
					inputVal += " " + time
				}
				d.setTime(Date.parse(inputVal));

				// make sure dates haven't wrapped around - 32 Jan = 1 Feb, 13/13 = 1 Jan Following year
				if (isNaN(MM)) {
					isError = MM.substr(0, 3).toLowerCase() != ConvertJsMonth(d.getMonth()).toLowerCase(); // has month word changed?
				}
				else if (isNaN(DD)) {
					isError = DD.substr(0, 3).toLowerCase() != ConvertJsMonth(d.getMonth()).toLowerCase(); // has month word changed?
				}
				else {
					isError = parseInt(MM) != d.getMonth() + 1; // has month number wrapped?
				}

				break;
		}

		if (!isError) {
			isError = isNaN(d) // bad date altogether
		}

		if (!isError) {			//check for stupid year
			if (YY > 2200 || YY < 1756 ) {
				isError = true;
			}
		}

		if (isError) {
			if (showAlert) alert('ERROR: This is not a recognised date format. Please correct the date before proceeding.')
			dateInput.focus()
		}
		else {
			var formattedDate = d.getDate() + ' ' + ConvertJsMonth(d.getMonth()) + ' ' + d.getFullYear();
			if (allowTime && time != "") {
				//formattedDate += ' ' + time
				var currhr = d.getHours()
				if (currhr < 10) {
					currhr = "0" + currhr
				}
				var currmin = d.getMinutes()
				if (currmin < 10) {
					currmin = "0" + currmin
				}
				formattedDate += ' ' + currhr + ':' + currmin
			}
			dateInput.value = formattedDate;
		}
	}
	return !isError
}

function ConvertJsMonth(m) {
	var returnVal = "";

	switch (m) {
		case 0:
			returnVal = "Jan"
			break;
		case 1:
			returnVal = "Feb"
			break;
		case 2:
			returnVal = "Mar"
			break;
		case 3:
			returnVal = "Apr"
			break;
		case 4:
			returnVal = "May"
			break;
		case 5:
			returnVal = "Jun"
			break;
		case 6:
			returnVal = "Jul"
			break;
		case 7:
			returnVal = "Aug"
			break;
		case 8:
			returnVal = "Sep"
			break;
		case 9:
			returnVal = "Oct"
			break;
		case 10:
			returnVal = "Nov"
			break;
		case 11:
			returnVal = "Dec"
			break;
	}
	return returnVal;
}

function formatTime(str) {
	var hrs, mins, ampm;
	ampm = "";
	if (str.indexOf(":") > -1) {
		hrs = str.split(":")[0];
		mins = str.split(":")[1];
		hrs = parseInt(hrs, 10);

		// check if mins has am or pm already
		if (mins.indexOf("a") > -1) mins = mins.substring(0, mins.indexOf("a"));
		if (mins.indexOf("p") > -1) mins = mins.substring(0, mins.indexOf("p"));
		//alert(mins)
		mins = parseInt(mins, 10);
	} else {
		hrs = parseInt(str, 10);
		mins = 0;
	}
	if (isNaN(parseInt(hrs, 10))) {
		// not valid
		return "";
	}
	if (isNaN(parseInt(mins, 10))) {
		// not valid
		return "";
	}
	if (str.indexOf("a") > -1) {
		ampm = "am";
	} else if (str.indexOf("p") > -1) {
		ampm = "pm";
	} else if (hrs > 24) {          // default to midnight
		hrs = 12;
		ampm = "am";
	} else if (hrs > 12) {
		hrs -= 12;
		ampm = "pm";
	} else if (hrs == 12) {
		ampm = "pm";
	} else if (hrs == 0) {
		hrs = 12;
		ampm = "am";
	} else {
		ampm = "am";
	}

	mins = "0" + mins;
	hrs = "0" + hrs;
	return hrs.substr(hrs.length - 2, hrs.substr.length) + ":" + mins.substr(mins.length - 2, mins.substr.length) + ' ' + ampm;
}

