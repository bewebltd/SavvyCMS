function DBStructure() {};
DBStructure.instance = null;

DBStructure.init = function () {

	$('.databaseDropdown').change(function () {
		var left = $('select[name=left]').val();
		var right = $('select[name=right]').val();
		if(left != '' && right != '' && left != right) {
			$('#comparisonForm').submit();
		}
	});

	if (!comparisonLeft || !comparisonRight) return;
	DBStructure.initUI();

	DBStructure.resize();
	$(window).bind('resize', DBStructure.resize);
};

DBStructure.initUI = function () {
	//if (value == null) return;
	var target = document.getElementById("view");
	//var target = orig1;
	//target.innerHTML = "";
	DBStructure.instance = CodeMirror.MergeView(target, {
		value: comparisonLeft.replace(/<br>/g, '\n'),
		//origLeft: panes == 3 ? orig1 : null,
		orig: comparisonRight.replace(/<br>/g, '\n'),
		lineNumbers: true,
		//mode: "text/html",
		//mode: "text",
		highlightDifferences: true
	});
};

DBStructure.resize = function () {
	$('.CodeMirror, .CodeMirror-merge').css('height', ($(window).height() - 245) + 'px');
};