function KeepAlive() {
	$.get(websiteBaseUrl + 'Services/KeepAlive.aspx', { "r": Math.random });    
	window.setTimeout('KeepAlive()', 600000); // 10 mins
}
$(document).ready(function () {
	window.setTimeout('KeepAlive()', 600000); // 10 mins
});