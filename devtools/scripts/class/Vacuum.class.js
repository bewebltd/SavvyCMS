function Vacuum(){ }

Vacuum.isRunning = false;
Vacuum.hasFinished = false;

Vacuum.init = function () {
	$('#vacuumSwitch').bind('change', function () {
		if ($(this).prop('checked')) Vacuum.execute();
	});
};

Vacuum.execute = function () {
	if (Vacuum.isRunning || Vacuum.hasFinished || !$('#vacuumSwitch').prop('checked')) return;
	Vacuum.isRunning = true;

	var row = $('.notVerified:first');
	var filename = row.data('filename');
	var status = row.find('.status');

	$.ajax({
		url: 'Controller.aspx?mode=AttachmentExistsOnDB',
		beforeSend: function () {
			status.addClass('statusColumn');
			status.html('<img src="images/loading.gif" />');
		},
		data: { filename: filename },
		success: function (response) {

			var obj = $.parseJSON(response);

			if (obj.success) {
				if (obj.foundOnTable) {
					status.addClass('result success');
					status.html('<img src="images/success.png" /> Attachment found on <strong>' + obj.foundOnTable + '</strong> table');
				} else {
					status.addClass('result notfound');
					status.html('<img src="images/error.png" /> Attachment not found');
					row.find('input[type=checkbox]').prop('checked', true);
				}
			} else {
				status.addClass('result error');
				status.html('<img src="images/error.png" /> An error has occurred');
			}

			row.removeClass('notVerified');
			Vacuum.isRunning = false;

			if ($('.notVerified').length == 0) Vacuum.hasFinished = true;
			else Vacuum.execute();
		},
		error: function (err) {
			status.addClass('result error');
			status.html('<img src="images/error.png" /> An error has occurred');
			row.removeClass('notVerified');
			Vacuum.isRunning = false;
		}
	});
};