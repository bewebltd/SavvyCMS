$(function(){
	if ($('.monitor').length > 0) ServerMonitor.init();
	if ($('.vacuum').length > 0) Vacuum.init();
	if ($('.dbmanager').length > 0) DBManager.init();
	if ($('.textcompare').length > 0) TextCompare.init();
	if ($('.dbstructure').length > 0) DBStructure.init();
});