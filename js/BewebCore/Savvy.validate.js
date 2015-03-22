// Savvy Validate v1.0.11 20130919
//<!--
//<script language="javascript" type="text/javascript" src="../jquery-1.4.2/jquery-1.4.2-vsdoc.js"></script>
//-->
/// <reference path="../jquery-1.4.2/jquery-1.4.2-vsdoc.js" />
/*

example A - full control:
-------------------------
	<script src="<%=Web.Root %>js/BewebCore/Savvy.validate.js"></script>
	<script>
		$(document).ready(function() {
			BewebInitForm("form")
		});

		function CheckForm(form) {
			var result = CheckBasicFormValidation(form)

			//extra validation here
			if (true) {
				var $op = $('#NumberOption1')
				if ($op.val().length < 5) {
					ShowValidationMessage("must be >=5chars", $op)
					return false
				}
			}

			if (result) result = SavvyBeforeFormSubmit(form);
			return result;
		}
	</script>

	<form action="" id="form">
		... more elements here ...
		<input type="submit" value="Send" onclick="return CheckForm(form)" />
	</form>


example B - shortcut using AutoValidate:
----------------------------------------
	<script src="<%=Web.Root %>js/BewebCore/Savvy.validate.js"></script>
	<script>
		function ExtraValidation(form, invalidFields) {
			//extra validation here
			return true;
		}
	</script>

	<form action="" id="form" class="AutoValidate">
		... more elements here ...
		<input type="submit" value="Send" />
	</form>
	

OPTIONS FOR DISPLAYING VALIDATION MESSAGES
------------------------------------------
		Savvy Validate is intended to easily support all these validation display styles:

		1. highlight invalid fields 
				- Savvy validate applies a .error class to each invalid field, so use CSS to style input.error, select.error, textarea.error
		2. show a static message at top of form and/or list of invalid fields
				- Savvy validate calls $('.validation-feedback', form).show() when there is an error, so put your message in a div with class 'validation-feedback' inside the Form tag
				- If you add spans with ids matching field names inside the validation-feedback div, these will be shown when the corresponding field is invalid
				- Example:
				<form action="" id="form" class="AutoValidate">
					<div class="validation-feedback">
						Oops!
						<span class='validation' id='validation_FirstName'>Please enter your first name.</span>
						<span class='validation' id='validation_LastName'>Please enter your surname.</span>
						<span class='validation' id='validation_Email'>Please enter a valid email address.</span>
					</div>
					Name: <%=new InputField("FirstName"){ShowValidation=false,dataplaceholder="First Name"}%> <%=new InputField("LastName"){ShowValidation=false,dataplaceholder="Last Name"}%>
					Email: <%=new EmailField("Email"){ShowValidation=false,dataplaceholder="Email Address"}%>
				</form>
		3. perform custom validation 
				- Define a function ExtraValidation(form, invalidFields) on your page
		4. show validation message beside each field
				- Validation spans can now be rendered server side, this way they will always be in the right place, straight after the control
				- You can set it individually as a property in <%=new InputField("Title"){ShowValidation=true}%>
				- Set the default on or off globally in BewebCoreSettings.cs or Application_Start() in global.asa.cs, the setting is Forms.DefaultShowInlineValidation
				- These are rendered as <span class='validation' id='validation_{FieldID}'></span>. Savvy Validate looks for this by ID and then stuffs the validation message into it.
				- To place the span somewhere else, turn off the server side rendering and add the span manually
				- Example:
				<form action="" id="form" class="AutoValidate">
					<div class="validation-feedback">
						Oops!
					</div>
					Name: <%=new InputField("FirstName"){ShowValidation=false,dataplaceholder="First Name"}%> <%=new InputField("LastName"){ShowValidation=false,dataplaceholder="Last Name"}%>
					<span class='validation' id='validation_FirstName'>Please enter your first name.</span>
					<span class='validation' id='validation_LastName'>Please enter your last name.</span>
					Email: <%=new EmailField("Email"){ShowValidation=true,dataplaceholder="Email Address"}%>
				</form>
		5. custom validation message styling and placement
				- To style these use CSS rule span.validation, example:
						span.validation{ display:none; color: red; font-weight: bold; }        ** Validation errors must be hidden by default so you need display:none
				- 2 extra spans with class='validation_inner' and class='validation_outer' is inserted around the text, so you can also use this for more complex CSS styling
				- If the validation message is in the wrong place, you can set ShowValidation=false and put your own span whereever you like so long as it has the corresponding ID 'validation_'+fieldID
		6. overlay validation message in arrow bubbles on each invalid field
				- Using method in 4 and 5 above
				- Add the following CSS rules in your stylesheet
					.validation{ display: none; position: relative;}
					.validation .validation_outer{ background: url(../../images/error_msg_arrow.png) 15px 19px no-repeat; padding-bottom: 12px;  position: absolute; top: -37px; left: -35px; z-index: 99; width: auto !important;}
					.validation .validation_inner{ background: #e30418; font-weight: bold; font-size: 11px;line-height: 15px;color: #fff; padding: 5px 10px;  border-radius: 4px; white-space:nowrap;}
					textarea + .validation .validation_outer{ top: -29px; }
					.validation.autoPosition{ position: absolute; }
					.validation.autoPosition .validation_outer{ left: auto; top: auto; }
		7. custom validation message translations
				- can be used in a multiligual site or just to change the message text in a programmable way
				- define a javascript function on the page which takes the standard error message as a parameter and optionally a field
				- your function must return the error message to display
				- example:
				function SavvyValidateTranslator(errorMessage, field) {
					if (errorMessage=="Email address is invalid" && field.name=="HomeEmail") {
						return "Wrong email!";
					} else if (errorMessage=="Please complete this field") {
						return "S'il vous plaît remplir ce champ";
					}
					return errorMessage;
				}



*/

$(document).ready(function () {
	// save people from having to type BewebInitForm()
	$("form.AutoValidate").each(function () { BewebInitForm(this); });
});

var savvyDisableValidate = false;
var savvyValidateFadeOut = true;

function savvyDisableValidation(disableValidate) {
	savvyDisableValidate = disableValidate;
}

function BewebInitForm(form) {
	// check that form exists
	if (typeof form == "object" && form.tagName && form.tagName == "FORM") {
		// ok
	} else if (typeof form == "object" && form.appendTo) {
		// unwrap jquery object to get form dom element
		if (form.length > 0 && form[0].tagName && form[0].tagName == "FORM") {
			form = form[0];
		} else {
			alert("Savvy Javascript Validation Error (Savvy.validate.js):\nForm parameter incorrect. JQuery object did not contain a form element.\n\nPlease correct the call to BewebInitForm(formID).")
			debugger			
		}
	} else if (typeof form == "string") {
		if (form.charAt(0) == "#" && $(form).length > 0) {
			form = $(form)[0];
		} else if ($("#" + form).length > 0) {
			form = $("#" + form)[0];
		} else {
			alert("Savvy Javascript Validation Error (Savvy.validate.js):\nForm does not exist with ID [" + form + "].\n\nPlease either set the form ID in the <form> tag or change the ID in the call to BewebInitForm(formID).")
			return
		}
	} else {
		alert("Savvy Javascript Validation Error (Savvy.validate.js):\nForm parameter must be a form element, jquery object, or form ID.\n\nPlease correct the call to BewebInitForm(formID).")
		debugger
		return;
	}

	if (!form.doneBewebInitForm) {
		form.doneBewebInitForm = true;
	} else {
		// already called this function before
		return
	}

	//added conditional to stop IE errors
	try {
		document.form = form; // may not need this any more, but leave for backwards compatibility
	} catch (e) {
		// whatever
	}
	// setup global form variable used by beweb-cma.js (for non-IE browsers ony)
	//if (!document.form) document.form = document.getElementById(formID)

	// Not needed
	//var isHtml5Supported = false;
	//var tester = document.createElement('input');
	//tester.type = "date";
	//tester.value = ':(';
	//if (tester.type === "date") {
	//	if (tester.value === '') {
	//		isHtml5Supported = true;
	//	}
	//}
	
//	if (!$.fn.live) {   //todo - check if this actually works!
//		$.fn.live = function (types, data, fn) {
//			if (window.console && console.warn) {
//				console.warn("jQuery.live is deprecated. Use jQuery.on instead.");
//			}
//			jQuery(this.context).on(types, this.selector, data, fn);
//			return this;
//		};
//	}

	// see if date picker is needed
	if ($("input.datetime,input.date", form).length > 0) {
		if ($("input.datetime", form).datepicker || $("input.date", form).datepicker) {
			// setup date pickers
			BewebInitDatePickers();
		} else {
			if ($("input[type=text].datetime,input[type=text].date").length > 0) {
				alert("Savvy Javascript Validation Error (Savvy.validate.js):\nThis page contains Date input fields but jQuery.datepicker is not loaded. You need to include several js files and skin files.");
			}
		}
	}

	// see if beweb-cma.js is needed
	if ($("input[type=text].datetime,input[type=text].date,input[type=text].number,input[type=text].url", form).length > 0) {
		if (!window.df_FmtNumber) {
			alert("Savvy Javascript Validation Error (Savvy.validate.js):\nThis page contains Number or Date input fields but beweb-cma.js is not included.")
		}
	}

	// see if any picture fields
	if ($("input.picture", form).length > 0) {
		if (!window.handleSelectPicture && !window.svyHandleSelectPicture) {
			alert("Savvy Javascript Validation Error (Savvy.validate.js):\nThis page contains Picture Upload fields but forms.js is not included.")
			return;
		}
	}

	// setup autocorrect formatting
	$(form).on("blur", "input", function () {
	
		if ($(this).hasClass("svyMobileFileUpload")) {
			return;
		}

		if (this.type == "text" && this.value) {
			if ($(this).hasClass("date") || $(this).hasClass("datetime")) {
				CheckDateInput(this, false);
			} else if ($(this).hasClass("time") && window.CheckTimeEntryField) {
				window.CheckTimeEntryField(this);
			} else if ($(this).hasClass("url") && window.CheckURLField) {
				window.CheckURLField(this);
			} else if ($(this).hasClass("number") && window.df_FmtNumber) {
				this.value = window.df_FmtNumber(this);
			}
		}
	});

	$(form).on("change", "input,select,textarea,radio", function () {
		CheckBasicFieldValidation(this);
		ShowValidationFeedback(this.form); 
		return true;
	});

	// 20130628 Josh, Mike, Jonathan Binding now only on form
	$(form).bind("submit", function (event) { return BewebValidateSubmitHandler(this, event); });

	$('.validation-feedback .validation', form).hide(); // all validation errors should be hidden
}

/// Called automatically on form submit if form has class='AutoValidate'.
/// You could otherwise call it when form should be validated and then submitted.
function BewebValidateSubmitHandler(form, event) {
	var result = true;
	if (savvyDisableValidate) {
		return true;
	}
	if (!CheckBasicFormValidation(form)) {
		event.preventDefault();
		result = false;
	}

	// call user defined check form function if it exists.
	if (window.ExtraValidation) {
		// call user defined extra validation here
		if (!window.ExtraValidation(form, window.savvyValidateInvalidArray)) {
			event.preventDefault();
			result = false;
		}
	}
	// call user defined check form function if it exists. -- still need this? YES!!!!! for the deals page.
	if (window.CheckForm) {
		if (!window.CheckForm(form)) {
			return false;
		}
	}
	if(!result) {
		return false;
	}
	// prevent people clicking submit twice
	//$("input[type=submit],input[type=button]").click(function () { return false; });
	//$(".svySaveButton,.svySaveAndRefreshButton").unbind("click"); // need to unbind here rather than add a click function as the original event was a bind (quirk in <= IE8 where click wasn't reset)
	//$("input[type=submit],input[type=button]").attr("disabled", "disabled")  -- this causes values of submitted button not to come thru becaue disabled fields are not submitted - could use hidden field instead (as per savvy classic)
	//form.submit()
	return SavvyBeforeFormSubmit(form);
	//return true;
}

function ShowRequiredStars(form, html) {
	if (html == '') html = '<img src="' + websiteBaseUrl + 'images/red-star.png" class="redstar">'
	// show required fields
	$(form).find(".required:visible").each(function () {
		//$(this).after('<div class="red-star"></div>');
		$(this).after(html);
	});
}

function SavvyBeforeFormSubmit(form) {
	// prevent people clicking submit twice
	//$("input[type=submit],input[type=button]").click(function () { return false })

	//$("input[type=submit],input[type=button]").attr("disabled", "disabled")  -- this causes values of submitted button not to come thru becaue disabled fields are not submitted - could use hidden field instead (as per savvy classic)
	//form.submit()
	if (window.ExtraBeforeFormSubmit) {
		// Return false from function to stop form submitting
		// Useful when using Ajax to handle the form submit
		if (!ExtraBeforeFormSubmit()) {
			return false;
		}
	}

	if (form.isSubmitting) return false;
	form.isSubmitting = true;

	return true;
}

function BewebApplyDatePicker(element) {
	// now works with subforms!
	if (!$(element).hasClass("hasDatepicker")) {
		//$(element).removeClass("hasDatepicker")
		//$(".ui-datepicker-trigger", $(element).parent()).remove()
		var minYear = $(element).attr("earliestyear")
		var maxYear = $(element).attr("latestyear")
		if ($(element).hasClass("date")) {
			$(element).datepicker({ dateFormat: 'd M yy', duration: '', changeYear: 'true', changeMonth: 'true', yearRange: minYear + ":" + maxYear, showOn: "button", buttonImage: websiteBaseUrl + "images/ico-calendar.gif", buttonImageOnly: true
				/*, beforeShow: function (input, inst) {
					inst.dpDiv.css({ marginTop: -(input.offsetHeight+10) + 'px', marginLeft: (input.offsetWidth - 250) + 'px' });
				} */
			});
		} else if ($(element).hasClass("datetime")) {
			// Note Time Picker not tested... see example below
			$(element).datepicker({ dateFormat: 'd M yy', duration: '', changeYear: 'true', changeMonth: 'true', yearRange: minYear + ":" + maxYear, showOn: "button", buttonImage: websiteBaseUrl + "images/ico-calendar.gif", buttonImageOnly: true, showTime: true, constrainInput: false, stepMinutes: 1, stepHours: 1, altTimeField: '', time24h: false });
		}
	}
}
function BewebInitDatePickers() {
	$("input.date:visible,input.datetime:visible").each(function () { if (this.type == "text") BewebApplyDatePicker(this); });
}

function BewebValidateInvalidHandler(form, errors) {
	if (errors) {
//		var message = errors == 1
//			? 'You missed a field. It has been highlighted'
		//			: 'You missed ' + errors + ' fields. They have been highlighted';
		var message = 'There was a problem: ' + errors 
		if ($("div.validation-summary-errors").length > 0) {
			$("div.validation-summary-errors").html(message);
			$("div.validation-summary-errors").show(500);
			// scroll to top so user can see div (not sure if this position is quite correct but it works for me)
			var divTop = $("div.validation-summary-errors").position().top;

			$('html').animate({ scrollTop: divTop }, 'slow');
			window.setTimeout('$("div.validation-summary-errors").hide("slow")',15000); //hide after 15 seconds, kind of pointless
		}
	} else {
		$("div.validation-summary-errors").hide();
	}
}

// TODO: need to check if ignore: ":hidden" works - otherwise include this somewhere: if (!$(element).is(":visible")) return;

//function isURL(element) {
//	// from microsoft http://msdn.microsoft.com/en-us/library/ff650303.aspx but with tilde and equals added (doh!)
//	return this.optional(element) || /^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#\~=_]*)?$/i.test(value);
//}

function CheckDateField(fieldObj, showAlert) {
	var form = fieldObj.form;
	//if (form == null) form=document.forms[0]//default to first form on page
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
		// removed: silly programmer alert('CheckDateField: failed to find dropdown [' + fieldname + ']');
		return CheckDateInput(fieldObj, showAlert);    // MN 20090423
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

	if (dateInput.type=="date" && dateInput.validity) {
		// html5 date - assume browser ensures date is valid for us
		return dateInput.validity.valid;
	}

	if (inputVal == "") {
		// don't do anything - no value
		
	} else {
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
				// MN 20110906 - this code was only a problem with jquery validate, seems fine with savvy validate
				YY = (YY <= 50) ? YY += 2000 : YY	//Problematic, people type 11 and get 20011
				YY = (YY < 100) ? YY += 1900 : YY;
				
				// if MM is a number, convert it to the month name
				// 20080725 MN: changed code below to this, fixes bug where '030808' did not work
				if (!isNaN(MM)) {
					MM = parseInt(MM, 10);
					MM = ConvertJsMonth(MM - 1);
				}
				inputVal = DD + " " + MM + " " + YY;
				d.setDate(inputVal);
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
		} else {
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


			var timefield = dateInput.form["timefield_" + dateInput.name];
			if (timefield && timefield.value + "" == "") {
				var defaultTime = timefield.getAttribute("data-default-time");
				if (defaultTime && defaultTime + "" != "") {
					timefield.value = defaultTime;
				}
			}
		}
	}
	return !isError;
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

// global variable, you can override this in a $ ready function
var savvyValidateUseLineByLineErrors = true;            // can also use server side switch in BewebCoreSettings.cs

function ShowValidationMessage(errorMsg, element) {
	element = $(element)
	var id = element[0].id;
	if (element.hasClass("svyPicBrowseFile") && !element.is(":visible")) {
		// it is a hidden input, probably inside our html5 picture button container
		var wrapper = $(element).parents(".svyPictureContainer");
		if (wrapper.length > 0) {
			element = wrapper;  // position validation relative to wrapper - cannot position against hidden things
		}
	}
	if (element.hasClass("svyPictureContainer")) {
		// html5 picture button container, get the ID from the hidden input inside
		var fileinput = $("input[type=file]", element);
		id = fileinput[0].id;
	}

	// apply error class - for example show a red border around all invalid elements
	if (errorMsg == null) {
		// null is passed in to mean hide the validation message
		element.removeClass('error');
	} else {
		// show the validation message
		element.addClass('error');
	}
	
	// find validation span
	if (element.hasClass("svyDate drop")) {
		// this is a day-month-year 3 dropdown box control, so just show the one message - which is associated with the hidden field
		// take off the suffix "_day", "_month" or "_year"
		if (EndsWith(id, "_day") || EndsWith(id, "_month") || EndsWith(id, "_year")) {
			id = id.substr(0, id.lastIndexOf("_"));
		}
		// check other drops before clearing error message
		if (errorMsg == null && ($("#" + id + "_day").hasClass("error") || $("#" + id + "_month").hasClass("error") || $("#" + id + "_year").hasClass("error"))) {
			return 
		}
	}

	//find validation span for ajax attachment 
	if (element.hasClass("svyAttachmentHidden")) {
		id = id.replace("attachmentFileName_", "");
		element = element.parents(".svyAttachmentCntr"); // go get svyAttachmentCntr
	}

	var validation = $("#validation_" + id.replace('validation_', '').replace('file_', ''));
	//var findValidCFCol = $('.validationChildform')
	if (validation.length == 0 && element[0].name) {
		validation = $("#validation_" + element[0].name);
	}
	if (validation.length == 0) {
		// cannot find the id, so assume we are not showing individual validation messages, at least not for this field
		return;
	}

	if (errorMsg == null) {
		// null is passed in to mean hide the validation message
		validation.fadeOut(500).children().fadeOut(500);  // note this is critically important that it does not disappear immediately, otherwise click target is lost!! MN
		// note IE8 bug means it disappear at all unless all child elements are also faded out
		if (savvyValidateUseLineByLineErrors) {
			//validation.html("");
		}
	} else {
		// show the validation message
		if (savvyValidateUseLineByLineErrors) {
			// look for translation function on the page
			if (window.SavvyValidateTranslator) {
				errorMsg = window.SavvyValidateTranslator(errorMsg);
			}
			var extraClass = "";
			if (validation.text() != "" && validation.find(".validation_autotext").length===0) {
				errorMsg = validation.text();
			} else {
				extraClass = " validation_autotext";
			}
			if (window.savvyValidateUseOldClassNames) {
				validation.html('<span class="outer"><span class="inner'+extraClass+'">' + errorMsg + '</span></span>');
			} else {
				validation.html('<span class="validation_outer"><span class="validation_inner'+extraClass+'">' + errorMsg + '</span></span>');
			}

			if (validation.hasClass('autoPosition')) {
				var topValPos = element.offset().top - 20;
				var leftValPos = element.offset().left;
				if (element.attr('type') == 'checkbox' || element.attr('type') == 'radio') {
					leftValPos += 40;
				} else if (element.hasClass("svyPictureContainer") && !element.hasClass("svyMobilePictureContainer")) {

					var numButtons = element.find(".svyPasteLink:visible").length;
					var lastButton = "";
				
					if (numButtons <= 4) {
						lastButton = element.find(".svyPasteLink:visible:last");
					} 
					if (numButtons > 4) {
						lastButton = element.find(".svyPasteLink:visible:eq(3)");
					} 				
					if (lastButton.length > 0) {
						leftValPos = lastButton.offset().left + lastButton.outerWidth() - 10;
					}		
				}
					else if (element.hasClass("svyMobilePictureContainer")) {
					 lastButton = element.find(".btn-mobile-takepicture:visible:last");
					if (lastButton.length > 0) {
						leftValPos = lastButton.offset().left + lastButton.outerWidth() + 25;
						topValPos = element.offset().top - 20;
					}			
				} 
				else {
						leftValPos += element.outerWidth() - 40;
				}

				// AF20141125: We need to have both .show and .stop().animate() to get it working properly
				// as making it visible by changing the opacity will make jQuery thinks it's not visible, 
				// causing the cumulative issue.
				validation.show();
				validation.stop().animate({ opacity: '100' });  // show() did not work when you click save while fading out
				validation.offset({ top: topValPos, left: leftValPos });
				//console.log("SavvyValidate: " + id + " validation offset=", validation.offset(), validation.offsetParent())
				var outer = $(validation).find('.validation_outer');
				if (outer.length==0){
					outer = $(validation).find('.outer');
				}
				var validationWidth = outer.width();
				var validationRightPos = validation.offset().left + validationWidth;
				var amountOffScreen = validationRightPos - $(window).width();
				if (amountOffScreen > 0) {
					//console.log("SavvyValidate: " + id + " validation offscreen, new offset=", validation.offset())
					validation.offset({ top: topValPos, left: leftValPos - amountOffScreen - 20 });
				}
				
				if (savvyValidateFadeOut) {
					if (element[0].savvyValidationTimeout) {
						window.clearTimeout(element[0].savvyValidationTimeout);
					}
					element[0].savvyValidationTimeout = window.setTimeout(function () {
						validation.fadeOut(2000).children().fadeOut(2000);   // need to fadeout children in some browsers/doctypes
					}, 4000 + 2000 * Math.random());
				}
			}
	
		}
		validation.show();
		if (!element.hasClass("date")) { //dont focus the dates -- why not?
			if (element.focus && !window.isMobile) {
				element.focus();
			}
		}
	}
}

function EndsWith(str, suffix) {
	return str.indexOf(suffix, str.length - suffix.length) !== -1;
}

//function ShowHideValidationContainer() {
//	$("input:visible,select:visible,textarea:visible,radio:visible", form).each(function () {
//		if (!CheckBasicFieldValidation(this)) {
//			result = false;
//			numErrors++;
//		}
//	});
//}

function CheckBasicFieldDigitsOnlyValidation(field) {
	var result = true;
	var fieldVal = $(field).val();
	var minlen = $(field).attr("minlength") || 0;
	if (fieldVal == null || fieldVal == "" || fieldVal.length < minlen || isNaN(fieldVal)) {
		ShowValidationMessage("Field should contain digits only", field);
		result = false;
		return result;
	}
	return result;
}

function CheckBasicFieldPhoneNumberValidation(field) {
	// allow spaces, dashes, parentheses, and digits
	var result = true;
	var fieldVal = $(field).val();
	var regExp = /\s+/g;
	fieldVal = fieldVal.replace(regExp, '');
	fieldVal = fieldVal.replace(/\-/g, '').replace(/\(/, '').replace(/\)/g, '');

	if (fieldVal.length < 6 || isNaN(fieldVal)) {
		ShowValidationMessage("Phone number is invalid", field);
		result = false;
		return result;
	}
	return result;
}

function CheckBasicFieldNumberValidation(field) {
	var result = true;
	var fieldVal = $(field).val();
	var num = df_GetNumberFieldValue(field);
//	if (!isNaN(fieldVal)) {
//		ShowValidationMessage("Amount is not valid", field);
//		result = false;
//		return result;
	//	}
	var min = $(field).attr("min");
	var max = $(field).attr("max");

	if (min + "" !== "" && num < min) {
		ShowValidationMessage("Amount must be greater than or equal to " + min, field);
		result = false;
	}
	if (max + "" !== "" && num > max) {
		ShowValidationMessage("Amount must be less than or equal to " + max, field);
		result = false;
	}
	return result;
}

function CheckBasicFieldEmailValidation(field) {
	var result = true;
	var emailValue = $(field).val();
	if (emailValue != "" && !validateEmail(emailValue)) {
		ShowValidationMessage("Email address is invalid", field);
		result = false;
	} else {
		if (window.savvyValidateVerifyEmails) {
			$.getJSON("http://twitch.beweb.co.nz/emailvalidator", { email: emailValue }, function (data) {
				if (!data.passesValidation) {
					ShowValidationMessage("Email address does not exist", field);
				}
			});
		}
	}
	return result;
}

function CheckBasicFieldAlphanumValidation(field) {
	// allow letters, digits only
	var result = true;
	var fieldVal = $(field).val();
	for (var i = 0; i < fieldVal.length; i++) {
		var c = fieldVal[i];
		if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".indexOf(c) == -1) {
			ShowValidationMessage("May contain letters and digits only", field);					
			return false;
		}
	}
	return true;
}

function CheckBasicFieldUrlComponentValidation(field) {
	// allow letters, digits and dashes only
	var result = true;
	var fieldVal = $(field).val();
	for (var i = 0; i < fieldVal.length; i++) {
		var c = fieldVal[i];
		if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-".indexOf(c) == -1) {
			ShowValidationMessage("May contain letters, digits and dashes only", field);					
			return false;
		}
	}
	return true;
}

function CheckBasicFieldDateValidation(field) {
	// make sure is a proper date
	var result = true;
	var fieldVal = $(field).val();
	if (!CheckDateField(field,false)) {
		ShowValidationMessage("Please enter a valid date", field);
		return false;
	}
	return true;
}

var savvyValidateClassRules = {};

function AddClassValidator(cssClassName, validationFunc, errorMessage) {
	// add a validation rule for a css class
	/* eg
	AddClassValidator("ppt", function (value, element) {
		return (value == "mike");
	}, "Name must be mike.");
	*/
	savvyValidateClassRules[cssClassName] = { cssClassName: cssClassName, validationFunc: validationFunc, errorMessage: errorMessage };
}

function AddEqualToValidator(confirmationFieldSelector, originalFieldSelector) {
	// eg AddEqualToValidator("#ConfirmPassword", "#Password")
	// jQuery Validate version: $("#password_confirm").rules("add", { equalTo: "#pwcode" });
	// Savvy Validate version: AddEqualToValidator("#password_confirm", "#pwcode" );
	$(confirmationFieldSelector).addClass("confirmationfield").data("originalFieldSelector", originalFieldSelector);
	// tell the original field that it needs to revalidate the confirmation field when it changes
	RevalidateRelatedSelector(originalFieldSelector, confirmationFieldSelector);
}

function RevalidateRelatedSelector(originalFieldSelector, relatedFieldSelector) {
	// tell the original field that it needs to revalidate the confirmation field when it changes
	$(originalFieldSelector).data("revalidateRelatedSelector", relatedFieldSelector);
}

//$.fn.validator = function () {
//	//alert("jQuery Validate call invalid. Switch to Savvy Validate!");
//	return this;     // chain
//};

//$.fn.validator.addMethod = function (a, b, c) {
//	alert(a)
//}

function CheckFieldClassValidation(field) {
	for (var cssClassName in savvyValidateClassRules) {
		if ($(field).hasClass(cssClassName)) {
			var rule = savvyValidateClassRules[cssClassName];
			// call validation function
			var isValid = rule.validationFunc($(field).val(), field);
			if (!isValid) {
				ShowValidationMessage(rule.errorMessage, field);
				return false;
			}
		}
	}
	return true;
}

function CheckFieldEqualToValidation(field) {
	if ($(field).hasClass("confirmationfield")) {
		var selector2 = $(field).data("originalFieldSelector");
		var isValid = ($(field).val() == $(selector2).val());
		if ($(selector2).val() != '' && $(field).val() != '') {
			if (!isValid) {
				ShowValidationMessage("Please enter the same again", field);
				return false;
			}
		}
	}
	return true;
}


// todo: revalidate all related controls
if (!window.df_GetRadioValue) {
	window.df_GetRadioValue = function df_GetRadioValue(radioControl) {
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
}

function hasPicturePasteSrc(fieldName) {
	if ($("#svyFiledragLive_" + fieldName + " img").length > 0) {
			return $(".svyFiledragLive_" + fieldName + " img");
	}
	return false;
}

function hasPictureSrc(fieldName) {
	return $("#shp_" + fieldName + " img").length > 0;
}

function hasPictureFile(fieldName) {
	return $('#file_' + fieldName).val().replace(/C:\\fakepath\\/i, '')
}

function CheckBasicFieldPictureRequired(fileUploadPic) {
	var fieldName = fileUploadPic.name;
	return hasPictureSrc(fieldName) || hasPicturePasteSrc(fieldName) || hasPictureFile(fieldName);
}

function CheckBasicFieldValidation(field) {
	var result = true;
	$("div.validation-summary-errors").hide(); //hide big msg
	if ($(field).hasClass("svyPictureContainer")) {
		field = $("input[type=file]", field)[0];
	}

	if ($(field).hasClass("svyAttachmentCntr") && $(field).hasClass("required")) { //for ajax file upload
		if ($(field).find(".svyAttachmentHidden").length > 0) {
			$(field).find(".svyAttachmentAjax").removeClass("required");
			field = $(field).find(".svyAttachmentHidden");
		} else {
			return result;
		}
	}

	var element = $(field);
	if (element.length > 0) {
		field = element[0];
	} else {
		// field not found, alert error?
	}
	element.removeClass('error');
	element.parent().find('.errorArrow').remove(); //remove the labels within elements parent

	//df_FmtNumberField(this)
	var currentValue = element.val();
	if (field.type == "radio") {
		var radioName = element[0].name;
		if (!radioName) {
			alert("Radio with no name");
		}
		var radioGroup = element[0].form[radioName];
		currentValue = df_GetRadioValue(radioGroup);
		var lastRadio = radioGroup[radioGroup.length - 1];

		if (lastRadio && field !== lastRadio) {
			// do not need to validate the radio group more than once, so have the last radio button be responsible for validation
			return CheckBasicFieldValidation(lastRadio);
		}
	}
	var isBlank = (currentValue == "" || currentValue == element.attr("data-placeholder") || currentValue == element.attr("placeholder"));
	var hasErrorBorder = element.parent().hasClass("borderError");
	
	if (field.type == "checkbox" && element.hasClass("required")) {
		isBlank = !field.checked;
	} else if (element.hasClass("svyPicBrowseFile") && element.hasClass("required")) {
		isBlank = !CheckBasicFieldPictureRequired(field);
	}

	if (isBlank && element.hasClass("required") && !element.hasClass("svyAttachmentHidden")) {
		if (element.hasClass("svyPicBrowseFile")) {
			ShowValidationMessage("Please upload an image", field);
		} else if ($(field)[0].name.indexOf('__') == -1) {
			ShowValidationMessage("Please complete this field", field);
		} else {
			ShowValidationMessage("Required", field); // this is just checking missing fields so text is 'Required'
		}
		result = false;
	} else if (!CheckFieldEqualToValidation(field)) {
		result = false;
	} else if (isBlank && !element.hasClass("required")) {
		// definately ok, as it is blank and not required and not a hidden attachment field
		// hide validation message
		ShowValidationMessage(null, field);		
	} else if (element.hasClass("email") && !CheckBasicFieldEmailValidation(field)) {
		result = false;
	} else if (element.hasClass("digitsonly") && !CheckBasicFieldDigitsOnlyValidation(field)) {
		result = false;
	} else if (element.hasClass("phonenumber") && !CheckBasicFieldPhoneNumberValidation(field)) {
		result = false;
	} else if (element.hasClass("postcode") && !CheckBasicFieldDigitsOnlyValidation(field)) { 
		result = false;
	} else if (element.hasClass("number") && !CheckBasicFieldNumberValidation(field)) {
		result = false;
	} else if (element.hasClass("alphanum") && !CheckBasicFieldAlphanumValidation(field)) {
		result = false;
	} else if (element.hasClass("urlcomponent") && !CheckBasicFieldUrlComponentValidation(field)) {
		result = false;
	} else if ((element.hasClass("date") || element.hasClass("datetime")) && !CheckBasicFieldDateValidation(field)) {
		result = false;
	} else if (!CheckFieldClassValidation(field)) {
		result = false;
	} else {
		// hide validation message
		ShowValidationMessage(null, field);
	}

	if (isBlank && element.hasClass("svyAttachmentHidden")) { //for ajax hidden attachment field validation
		result = false;
		ShowValidationMessage("Please upload a file", field);
	}

	// also re-validate related field in the case of confirmation fields
	if (element.data("revalidateRelatedSelector")) {
		CheckBasicFieldValidation(element.data("revalidateRelatedSelector"))
	}
	
	$('.svySelect2').each(function () {
		select2PostitionReset($(this).attr('id'));
	});

	return result;
}

function select2PostitionReset(validationID) {

	if(validationID.toLowerCase().search('s2id_') == -1) {
		return;
	}

	validationID = validationID.substr(5);

	var position = $('#s2id_' + validationID).position();
	var width = $('#s2id_' + validationID).outerWidth();

	var leftPos = (position.left + width) - 40;
	var topPos = position.top - 20;

//	var leftPos = position.left;
	//var topPos = position.top;

	//var topValPos = element.offset().top - 20;
	//var leftValPos = element.offset().left;


	$('#validation_' + validationID).css("left", leftPos + 'px').css("top", topPos + 'px');
}

svyFormErrorCount = 0;

/// Checks for required fields and correct datatypes (including date, time, email validation)
function CheckBasicFormValidation(form) {

	if($(form).find("form").length > 0) {
		alert("There is already another form inside this form. You cant nest forms.")
		return false;
	}

	var result = true;
	var numErrors = 0;
	$(form).find('.validation').hide()
	$("div.validation-summary-errors").hide(); //hide big msg
	$(form).find('.error').removeClass('error') //remove the error classes
	$(form).find('.errorArrow').remove(); //remove the labels from the dom
	$("input:visible,select:visible,textarea:visible,radio:visible,.svyPictureContainer:visible,.svyAttachmentCntr:visible", form).each(function () {
		if ($(this).hasClass('cancel') || $(this).hasClass('dontvalidate') || $(this).hasClass('ignore') || $(this).hasClass("svyAttachmentAjax")) { //IGNORE AJAX because validation on the hidden field (ajax uses hidden field to hold path) and not the input (The selector gets all visible input fields) JC 20140422
			return;
		}

		if (!CheckBasicFieldValidation(this)) {
			result = false;
			numErrors++;
			svyFormErrorCount = numErrors;
		}
	});
		
//	$(form).find(".number:visible").each(function () {
//		if(!CheckBasicFieldValidation(this)) {
//			result = false;
//			numErrors++;
//		}	
//	})
//	$(form).find(".yesno:visible").each(function () {
//		if (!CheckBasicFieldValidation(this)) {
//			result = false;
//			numErrors++;
//		}	
//	})
//	$(form).find(".required:visible").each(function () {
//		if (!CheckBasicFieldValidation(this)) {
//			result = false;
//			numErrors++;
//		}	
//	})
//	$(form).find(".email:visible").each(function () {
//		if (!CheckBasicFieldEmailValidation(this)) {
//			result = false;
//			numErrors++;
//		}
//	})

//	$(form).find(".postcode:visible").each(function () {
//		if (!CheckBasicFieldPostCodeValidation(this)) {
//			result = false;
//			numErrors++;
//		}
//	})

//	$(form).find(".phonenumber:visible").each(function () {
//		if (!CheckBasicFieldPhoneNumberCodeValidation(this)) {
//			result = false;
//			numErrors++;
//		}
//	})

	if (form) {
		//alert(result)
		if (!result) {
			$(".validation-feedback", form).fadeIn(500);
		} else {
			$(".validation-feedback", form).fadeOut(500);
		}
	}

	var firstField = $(form).find('input.error:first');
	if (firstField.focus) {
		if (!firstField.hasClass("date")) {
			firstField.focus();
		}
	}	
	
	return result;
}

function ShowValidationFeedback(form) {
	var anyErrors = $('.error', form).length > 0;
	if (anyErrors) {
		$(".validation-feedback", form).fadeIn(500);
	} else {
		$(".validation-feedback", form).fadeOut(500);
	}
}

function EmailIsValid(value) {
	// contributed by Scott Gonzalez: http://projects.scottsplayground.com/email_address_validation/
	return  /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(value);
}
function validateEmail(email) {
	//from stackoverflow: sectrean
	var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/ 
	return email.match(re) 
}


