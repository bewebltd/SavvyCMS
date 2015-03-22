function Vacuum(){ }

Vacuum.isRunning = false;
Vacuum.hasFinished = false;
Vacuum.stopwatch = null;

Vacuum.init = function () {
	$('#vacuumSwitch').bind('change', function () {
		if ($(this).prop('checked')) {
			$('#vacuumSwitch').unbind('change');
			Vacuum.execute();
		}
	});
};

Vacuum.execute = function () {
	if (Vacuum.isRunning || Vacuum.hasFinished || !$('#vacuumSwitch').prop('checked')) return;
	Vacuum.isRunning = true;

	var row = $('.notVerified:first');
	var action = row.data('action');
	var status = row.find('.status');
	var result = row.find('.result');

	$.ajax({
		url: 'Controller.aspx?mode='+action,
		beforeSend: function () {
			status.addClass('statusColumn');
			status.html('<img src="images/loading.gif" /><span style="vertical-align:bottom;padding-left:5px" id="stopwatch">0:00</span>');
			Vacuum.startStopwatch();
		},
		success: function (response) {
			
			var obj = $.parseJSON(response);

			if (obj.success) {

				Vacuum.stopStopwatch();

				status.addClass('result success');
				status.html('<img src="images/success.png" /> Success ('+$('#stopwatch').text()+')');
				result.html(obj.result);

				row.removeClass('notVerified');
				Vacuum.isRunning = false;

				// CompareAttachments will return the list of files
				if (obj.files && obj.files.length > 0) {
					var html = '';
					for (var i = 0; i < obj.files.length; i++) {
						html += '<tr><td><a href="' + obj.attachmentsUrl + obj.files[i] + '" target="_blank">' + obj.files[i] + '</a></td></tr>';
					}
					$('#vacuumFiles tbody').html(html);
				}

				if ($('.notVerified').length == 0) Vacuum.hasFinished = true;
				else Vacuum.execute();
			} else {
				status.addClass('result error');
				status.html('<img src="images/error.png" /> An error has occurred');
			}

		},
		error: function (err) {
			status.addClass('result error');
			status.html('<img src="images/error.png" /> An error has occurred');
			row.removeClass('notVerified');
			Vacuum.isRunning = false;
		}
	});
};

Vacuum.startStopwatch = function () {
	Vacuum.stopwatch = setInterval(Vacuum.stopwatchIncrement, 1000);
};

Vacuum.stopwatchIncrement = function () {
	var e = $('#stopwatch');
	var mins = parseInt(e.text().split(':')[0]);
	var secs = parseInt(e.text().split(':')[1]);

	if (secs == 59) {
		secs = 0;
		mins++;
	} else {
		secs++;
	}

	secs = secs < 10 ? '0' + secs : secs;
	e.text(mins + ':' + secs);
};

Vacuum.stopStopwatch = function () {
	if (Vacuum.stopwatch != null) {
		clearInterval(Vacuum.stopwatch);
		Vacuum.stopwatch = null;
	}
};

Vacuum.moveFiles = function () {

	$('#vacuumTable tbody').append('<tr id="last-action">' +
		'<td>4</td>' +
		'<td>Moving files</td>' +
		'<td class="status">Queued</td>' +
		'<td class="result"></td>' +
	'</tr>');

	var row = $('#last-action');
	var status = row.find('.status');
	var result = row.find('.result');

	$.ajax({
		url: 'Controller.aspx?mode=MoveUnusedAttachments',
		beforeSend: function () {
			status.addClass('statusColumn');
			status.html('<img src="images/loading.gif" /><span style="vertical-align:bottom;padding-left:5px" id="stopwatch">0:00</span>');
			Vacuum.startStopwatch();
		},
		success: function (response) {

			var obj = $.parseJSON(response);

			if (obj.success) {

				Vacuum.stopStopwatch();

				status.addClass('result success');
				status.html('<img src="images/success.png" /> Success (' + $('#stopwatch').text() + ')');
				result.html(obj.result);
			} else {
				status.addClass('result error');
				status.html('<img src="images/error.png" /> An error has occurred');
			}

		},
		error: function (err) {
			status.addClass('result error');
			status.html('<img src="images/error.png" /> An error has occurred');
		}
	});
};

Vacuum.showFiles = function () {
	$('#vacuumFiles').show();
}