// @class
function AgeGate() {
	
	// @public
	this.init = function init() {
		$('.date-fields input').bind('keypress', ageGate.validateNumber);
		$('.date-fields input').bind('focus', inputFocus);
		$('.date-fields input').bind('blur', inputBlur);
		$('form').bind('submit', checkGateQuestionAndAge);
	};

	// @private
	function inputFocus() {

		if ($(this).attr('name') == 'month') {

			var val = $.trim($(this).val());

			switch (val) {
				case 'JAN': val = '01'; break;
				case 'FEB': val = '02'; break;
				case 'MAR': val = '03'; break;
				case 'APR': val = '04'; break;
				case 'MAY': val = '05'; break;
				case 'JUN': val = '06'; break;
				case 'JUL': val = '07'; break;
				case 'AUG': val = '08'; break;
				case 'SEP': val = '09'; break;
				case 'OCT': val = '10'; break;
				case 'NOV': val = '11'; break;
				case 'DEC': val = '12'; break;
			}

			$(this).val(val);
		}

		if (/Android/i.test(navigator.userAgent)) {
			$(this).prop('type', 'number');
		} else {
			$(this).prop('pattern', '[0-9]*');
		}

	}

	// @private
	function inputBlur() {
		var val = $.trim($(this).val());
		if (val.length > 0) {
			$(this).removeClass('empty');

			if ($(this).attr('name') == 'year') {
				if(val.length === 2) {
					val = (parseInt(val) <= 15 ? '20' : '19') + val;
				}
			} else {
				val = val.length === 1 ? '0' + val : val;
			}

			if ($(this).attr('name') == 'month') {
				
				if (/Android/i.test(navigator.userAgent)) {
					$(this).prop('type', 'text');
				} else {
					$(this).prop('pattern', '[A-Z]*');
				}

				if (parseInt(val) >= 1 && parseInt(val) <= 12) {
					$(this).removeClass('small');
					$(this).addClass('large');
					$('.date-fields').addClass('expanded');

					switch (val) {
						case '01': val = 'JAN'; break;
						case '02': val = 'FEB'; break;
						case '03': val = 'MAR'; break;
						case '04': val = 'APR'; break;
						case '05': val = 'MAY'; break;
						case '06': val = 'JUN'; break;
						case '07': val = 'JUL'; break;
						case '08': val = 'AUG'; break;
						case '09': val = 'SEP'; break;
						case '10': val = 'OCT'; break;
						case '11': val = 'NOV'; break;
						case '12': val = 'DEC'; break;
					}
				} else {
					$(this).removeClass('large');
					$(this).addClass('small');
					$('.date-fields').removeClass('expanded');
				}
			}

			$(this).val(val);

		} else {
			$(this).addClass('empty');

			if ($(this).attr('name') == 'month') {
				$(this).removeClass('large');
				$(this).addClass('small');
				$('.date-fields').removeClass('expanded');
			}
			
		}
	}

	// @public
	this.validateNumber = function validateNumber(evt) {
		var theEvent = evt || window.event;
		var key = theEvent.keyCode || theEvent.which;
		key = String.fromCharCode(key);
		var regex = /[0-9]|\./;
		if (!regex.test(key)) {
			theEvent.returnValue = false;
			if (theEvent.preventDefault) theEvent.preventDefault();
		}
	};

	// @private
	function checkGateQuestionAndAge() {

		if (!formIsValid())
			return false;

		var birthDate = $('input[name=day]').val() + ' ' + $('input[name=month]').val().toLowerCase() + ' ' + $('input[name=year]').val();
		var answer = $('input[name=answer]:checked').val();

		$.ajax({
			type: 'POST',
			url: websiteBaseUrl + 'Home/CheckGateQuestionAndAge',
			data: {
				birthDate: birthDate,
				answer: answer
			},
			success: function (response) {

				var obj = $.parseJSON(response);

				if (obj.success) {
					document.location.href = websiteBaseUrl;
				} else {
					alert(obj.error);
				}

			}
		});

		return false;
	}

	// @private
	function formIsValid() {

		if ($('input[name=answer]:checked').length == 0) {
			alert('Please answer the question');
			return false;
		}

		if($('input.empty').length > 0) {
			alert('Please type your date of birth');
			return false;
		}

		var day = $('input[name=day]').val();
		var month = $('input[name=month]').val();
		var year = $('input[name=year]').val();

		if (day.length != 2 || parseInt(day) < 1 || parseInt(day) > 31) {
			alert('Please type the day correctly');
			return false;
		}
		if (!isNaN(month)) {
			alert('Please type the month correctly');
			return false;
		}
		if (year.length != 4) {
			alert('Please type the year correctly');
			return false;
		}

		return true;
	}

};

var ageGate = new AgeGate();