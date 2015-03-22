function DBManager() {};

DBManager.editor = null;
DBManager.shortcuts = null;

DBManager.init = function () {
	$('.toggleCollapse').bind('click', function () {
		$(this).siblings('ul').slideToggle();
	});

	$('.dbSubObjects li').bind('click', function () {
		DBManager.newTab($(this).text(), $(this).data('table'));
	});

	var langTools = ace.require("ace/ext/language_tools");
	DBManager.editor = ace.edit("sqlEditor");
	DBManager.editor.setTheme("ace/theme/chrome");
	DBManager.editor.getSession().setMode("ace/mode/sql");
	DBManager.editor.setFontSize(14);
	DBManager.editor.setOptions({
		enableBasicAutocompletion: true,
		enableLiveAutocomplete: true
	});

	var words = [];
	$('.dbTable li').each(function () {
		words.push({ name: $(this).text(), value: $(this).text(), score: 300, meta: "Tables" });
	});
	$('.dbView li').each(function () {
		words.push({ name: $(this).text(), value: $(this).text(), score: 300, meta: "Views" });
	});

	var tableCompleter = {
		getCompletions: function (editor, session, pos, prefix, callback) {
			if (prefix.length === 0) { callback(null, []); return; }
			callback(null, words);
		}
	}; 

	langTools.addCompleter(tableCompleter);

	$('.tabs ul li:first-child').bind('click', function() {
		DBManager.activateTab(1);
	});

	DBManager.shortcuts = $('#shortcuts').modal({
		width: '600px',
		closeButton: true,
		height: '400px'
	});

};

DBManager.newTab = function (name, table) {

	var tab = DBManager.getTab(name);

	if (!tab) {
		var li = $('<li/>');
		var i = $('<i/>');
		i.bind('click', function() {
			DBManager.closeTab(name);
		});
		li.html(name);
		li.data('table', table);
		li.append(i);
		li.bind('click', function() {
			DBManager.activateTab(name);
		});

		if (name == 'Query Results') {
			$('.tabs ul li:first').after(li);
		} else {
			$('.tabs ul').append(li);
		}

		tab = name;
	}

	DBManager.activateTab(tab);
};

DBManager.activateTab = function (index) {
	
	var tab = DBManager.getTab(index);

	if (tab) {
		$('.tabs .active').removeClass('active');
		tab.addClass('active');

		$('.tabContent').hide();

		if(index === 1 || (typeof (index) == 'object' && index.text() == 'SQL Editor')) {
			$('#sqlEditorWrapper').show();
		} else if (index == 'Query Results' || (typeof (index) == 'object' && index.text() == 'Query Results')) {
			$('#queryResults').show();
		} else {
			var id = tab.text() + "_content";
			if ($('#'+id).length > 0) {
				$('#' + id).show();
			} else {
				DBManager.loadTableFields(id, tab.data('table'));
			}
		}
	}
};

DBManager.closeTab = function (index) {
	var tab = DBManager.getTab(index);
	var previous = $('.tabs ul li').length == 2 ? 1 : tab.prev();
	$('#' + tab.text() + "_content").remove();
	tab.remove();
	DBManager.activateTab(previous);
};

DBManager.getTab = function (index) {
	var el;
	if (typeof (index) === 'number') {
		el = $('.tabs ul li:nth-child(' + index + ')');
	} else if (typeof (index) === 'string') {
		$('.tabs ul li').each(function () {
			if ($(this).text() == index) {
				el = $(this);
				return false;
			}
		});
	} else {
		el = index; // already a jQuery object
	}

	return el && el.length > 0 ? el : null;
};

DBManager.loadTableFields = function (id, table) {

	var loading = '<div class="loadingBig"></div>';

	if ($('#' + id).length == 0) {
		$('.content').append('<div id="' + id + '" class="tabContent">' + loading + '</div>');
	} else {
		$('#' + id).html(loading);
	}

	$.ajax({
		url: 'Controller.aspx?mode=GetDBTableFields',
		data: { table: table },
		success: function (response) {
			var arr = $.parseJSON(response);

			var html = '<div class="tabContentBar">';
			html += '<button class="btn left btnStructureOnly" onclick="addField()">Add field</button>';
			html += '<button class="btn left btnStructureOnly" onclick="addField()">Reorder fields</button>';
			html += '<button class="btn left btnDataOnly saveChanges" onclick="saveChanges()">Save Changes</button>';
			html += '<button class="btn left btnDataOnly" onclick="DBManager.showTableData(\'' + id + '\', \'' + table + '\')">Refresh</button>';
			html += '<button class="btn right dataButton" onclick="DBManager.showTableData(\'' + id + '\', \'' + table + '\')">Data</button>';
			html += '<button class="btn right structureButton" onclick="DBManager.showTableStructure(\'' + id + '\', \'' + table + '\')" disabled="disabled">Structure</button>';
			html += '</div>';
			html += '<table class="fieldList">';
			html += '<thead>';
			html += '<tr>';
			html += '<td></td>';
			html += '<td>Field Name</td>';
			html += '<td>Data Type</td>';
			html += '<td>Not Null</td>';
			html += '<td>Unique</td>';
			html += '<td>Identity</td>';
			html += '<td>Default Value</td>';
			html += '<td>Description</td>';
			html += '</tr>';
			html += '</thead>';
			html += '<tbody>';

			for (var i = 0; i < arr.length; i++) {
				html += '<tr>';
				html += '<td class="centered">' + (arr[i].IsPrimaryKey ? '<img src="images/pk.png" />' : '') + '</td>';
				html += '<td>' + arr[i].ColumnName + '</td>';
				html += '<td>' + arr[i].Type + (arr[i].Type.toLowerCase().search('char') != -1 ? '(' + arr[i].MaxLength + ')' : '') + '</td>';
				html += '<td class="centered"><input type="checkbox" ' + (!arr[i].IsNullable ? 'checked="checked"' : '') + ' /></td>';
				html += '<td class="centered"><input type="checkbox" ' + (arr[i].IsUnique ? 'checked="checked"' : '') + ' /></td>';
				html += '<td class="centered"><input type="checkbox" ' + (arr[i].IsIdentity ? 'checked="checked"' : '') + ' /></td>';
				html += '<td>' + (arr[i].DefaultValue ? arr[i].DefaultValue : '') + '</td>';
				html += '<td>' + (arr[i].Description ? arr[i].Description : '') + '</td>';
				html += '</tr>';
			}

			html += '</tbody>';
			html += '</table>';
			html += '<div class="tableData"></div>';

			$('#' + id).html(html);

			DBManager.showTableStructure(id);
		},
		error: function (err) {
			alert('<p>An error has ocurred. <a href="#" onclick="DBManager.loadTableFields(\'' + id + '\',\'' + table + '\');return false;">Try again</a></p>');
		}
	});
};

DBManager.showTableStructure = function (id) {
	$('#' + id + ' .btnDataOnly').hide();
	$('#' + id + ' .btnStructureOnly').show();
	$('#' + id + ' .structureButton').attr('disabled', 'disabled');
	$('#' + id + ' .dataButton').removeAttr('disabled');
	$('#' + id + ' .tableData').hide();
	$('#' + id + ' .fieldList').show();
};

DBManager.showTableData = function (id, table) {
	var sql = 'SELECT TOP 1000 * FROM ' + atob(table);
	$('#' + id + ' .btnStructureOnly').hide();
	$('#' + id + ' .btnDataOnly').show();
	$('#' + id + ' .structureButton').removeAttr('disabled');
	$('#' + id + ' .dataButton').attr('disabled', 'disabled');
	$('#' + id + ' .saveChanges').attr('disabled', 'disabled');
	var container = $('#' + id + ' .tableData');
	container.html('<div class="loadingBig"></div>');
	$('#' + id + ' .fieldList').hide();
	container.show();
	DBManager.execute(sql, function (obj, html) {
		container.html(html);
	});
};

DBManager.executeEditor = function () {

	// Try to get selected text
	var sql = DBManager.editor.session.getTextRange(DBManager.editor.getSelectionRange());

	if(!sql) { // Get the whole thing
		sql = DBManager.editor.session.getValue();
	}

	sql = $.trim(sql);

	if(sql) {
		var sqls = sql.split(";");
		for (var i = 0; i < sqls.length; i++) {
			// remove comments
			var s = $.trim(($.trim(sqls[i] + '')).replace(/\-\-[^\n]*/gi, ''));

			if (s) {
				DBManager.newTab('Query Results');
				$('#queryResults').html('<div class="loadingBig"></div>');
				DBManager.execute(s, function (obj, html) {
					$('#queryResults').html(html);
				});
				return;
			}
		}
	}

};

DBManager.execute = function (sql, callback) {
	$.ajax({
		url: 'Controller.aspx?mode=ExecuteSQL',
		data: { sql: btoa(sql) },
		success: function (response) {

			var obj = $.parseJSON(response);

			var html = '';

			if (obj.success) {
				if (obj.type == 'query') {
					html = '<table>';

					html += '<thead>';
					html += '<tr>';
					for (var i = 0; i < obj.fields.length; i++) {
						html += '<td>' + obj.fields[i].name + '</td>';
					}
					html += '</tr>';
					html += '</thead>';

					html += '<tbody>';
					for (var i = 0; i < obj.data.length; i++) {
						html += '<tr>';
						for (var j = 0; j < obj.data[i].length; j++) {
							if (obj.fields[j].type == 'Boolean') {
								html += '<td class="centered"><input type="checkbox" ' + (obj.data[i][j] === 'True' ? 'checked="checked"' : '') + ' /></td>';
							} else {
								html += '<td>' + obj.data[i][j] + '</td>';
							}
						}
						html += '</tr>';
					}

					html += '</tbody>';
					html += '<tfoot>';
					html += '<tr>';
					html += '<td colspan="' + obj.fields.length + '">' + obj.rows + ' rows fetched (' + obj.time + ' ms)</td>';
					html += '</tr>';
					html += '</tfoot>';

					html += '</table>';
				} else {
					// @todo: non query
				}
			} else {
				html = obj.error;
			}

			if(callback && typeof(callback) == 'function') {
				callback(obj, html);
			}

		},
		error: function (err) {
			//console.log(err);
			alert('dbmanager:'+err);
		}
	});
};

DBManager.showShortcuts = function() {
	DBManager.shortcuts.show();
}