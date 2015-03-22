function TextCompare() {};

var value, orig1, orig2, dv, hilight = true;
TextCompare.init = function () {
	value = document.documentElement.innerHTML;
	orig1 = value.replace(/\.\.\//g, "codemirror/").replace("yellow", "orange");
	orig2 = value.replace(/\u003cscript/g, "\u003cscript type=text/javascript ")
    .replace("white", "purple;\n      font: comic sans;\n      text-decoration: underline;\n      height: 15em");
	TextCompare.initUI(2);
};

TextCompare.initUI = function (panes) {
	if (value == null) return;
	var target = document.getElementById("view");
	target.innerHTML = "";
	dv = CodeMirror.MergeView(target, {
		value: value,
		origLeft: panes == 3 ? orig1 : null,
		orig: orig2,
		lineNumbers: true,
		mode: "text/html",
		highlightDifferences: hilight
	});
};