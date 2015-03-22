$(document).ready(function () {
	//EvenUpHeights("ul.categorylist li");
	$("ul.categorylist li").click(function () {
		if ($(this).attr("data-document-id")) {
			var downloadID = $(this).data("documentId") + "";
			var url = websiteBaseUrl + "DownloadDocument/" + downloadID;
			window.open(url, "download");
		} else {
			var categoryID = $(this).data("categoryId") + "";
			window.location.href = websiteBaseUrl + "DocumentCategory/" + categoryID;
		}
	});
});