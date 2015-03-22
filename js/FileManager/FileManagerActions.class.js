function FileManagerActions() {
	
	// @public
	this.cut = function cut() {
		alert('CUT!');
	}

	// @public
	this.selectAll = function selectAll() {
		$('#files ul li').addClass('selected');
		$('#files ul li').addClass('ui-selected');
	}

	// @public
	this.selectNone = function selectNone() {
		$('#files ul li').removeClass('selected');
		$('#files ul li').removeClass('ui-selected');
	}

	// @public
	this.invertSelection = function invertSelection() {
		$('#files ul li').removeClass('ui-selected');
		$('#files ul li').toggleClass('selected');
		$('#files ul li.selected').addClass('ui-selected');
	}

}

var fma = new FileManagerActions();