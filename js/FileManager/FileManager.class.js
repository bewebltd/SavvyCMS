function FileManager() {
	
	// @public
	this.init = function init() {
		$('.top-tabs ul li').bind('click', showTabContent);

		$('.top-nav-options li').bind('click', function() {
			if ($(this).hasClass('disabled')) return;
			var action = $(this).data('action');
			if(action) executeAction(action);
		});

		initTree();
		resizePanels();
		$(window).resize(resizePanels);
		enableDisableNavButtons();
		initFiles();
	};

	// @private
	function resizePanels() {
		var newHeight = Math.max(200, $(window).height() - $('#navigation-pane').position().top) + 'px';
		$('#tree, #files, #files ul').css('height', newHeight);
	}

	// @private
	function initTree() {
		var setting = {
			async: {
				enable: true,
				url: fileManagerControllerUrl + "GetDirectories",
				autoParam: ["id", "name=n", "level=lv"],
				//otherParam: { "otherParam": "zTreeAsyncTest" },
				dataFilter: treeDataFilter
			},
			callback: {
				onClick: treeOnClick
			}
		};

		$.fn.zTree.init($("#tree"), setting);
		$("#navigation-pane").resizable({
			minWidth: 150,
			maxWidth: 300,
			handles: 'e'
		});
	}

	// @private
	function treeOnClick(a, b, c) {
		console.log(a);
		console.log(b);
		console.log(c);
	}

	// @private
	function treeDataFilter(treeId, parentNode, childNodes) {
		if (!childNodes) return null;
		for (var i = 0, l = childNodes.length; i < l; i++) {
			childNodes[i].name = childNodes[i].name.replace(/\.n/g, '.');
		}
		return childNodes;
	}

	// @private
	function initFiles() {
		$('#files ul li').bind('click', fileOnClick);
		$('#files ul li').bind('dblclick', fileOnDoubleClick);
		$("#files ul").selectable({
			selecting: function (event, ui) {
				$(ui.selecting).addClass('selected');
			},
			selected: function (event, ui) {
				enableDisableNavButtons();
			},
			unselected: function (event, ui) {
				$(ui.unselected).removeClass('selected');
			},
			cancel: '.ui-selected' 
		});

		$("#files ul li").draggable({
			//connectToSortable: "#sortable",
			//helper: "clone",
			//revert: "invalid"
		});
	}

	// @private
	function fileOnClick(e) {
		e = e || window.event;

		// If it's not holding CTRL key, clear other selections
		if (!e.ctrlKey) {
			$('#files ul li').removeClass('selected');
			$('#files ul li').removeClass('ui-selected');
		}

		$(this).addClass('selected');
		$(this).addClass('ui-selected');
		enableDisableNavButtons();
	}

	// @private
	function fileOnDoubleClick() {
		alert('double click');
		// @todo: set in the tree
		// @todo: load content/file itself
	}

	// @private
	function showTabContent() {
		// If it's active, don't do anything
		if ($(this).hasClass('active')) return;
		// Show the content
		var href = $(this).data('href');
		$('.top-nav-content').hide();
		$('#' + href).show();
		// Set active tab
		$('.top-tabs ul li').removeClass('active');
		$(this).addClass('active');
	}

	// @private
	function executeAction(action) {
		if (typeof (fma[action]) === 'function') fma[action]();
		else console.log("FileManagerActions." + action + " doesn't exist");
	}

	// @private
	function enableDisableNavButtons() {
		// Enable all of them
		$('.top-nav-options li').removeClass('disabled');

		// Disable all that need any sort of selection
		$('.requiresSelection, .requiresFileSelection, .requiresFolderSelection, .requiresSingleSelection').addClass('disabled');
		
		var selectedFolders = $('#files ul li.selected.folder').length;
		var selectedFiles = $('#files ul li.selected.file').length;
		
		// If there's ONLY ONE thing selected, enable the ones that require single selection
		if(selectedFolders + selectedFiles === 1) {
			$('.requiresSingleSelection').removeClass('disabled');
		}

		// If there are folders selected, enable the ones that require folder selection
		if (selectedFolders > 0) {
			$('.requiresFolderSelection').removeClass('disabled');
		}

		// If there are files selected, enable the ones that require file selection
		if (selectedFiles > 0) {
			$('.requiresFileSelection').removeClass('disabled');
		}

		// If there's anything selected, enable the ones that require selection
		if (selectedFolders + selectedFiles > 0) {
			$('.requiresSelection').removeClass('disabled');
		}
	}

}

var fm = new FileManager();
jQuery(fm.init);