<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Areas.Admin.Controllers.CommonAdminController.SEOViewData>" %>
<%@ Import Namespace="Beweb" %>

<%Util.IncludeJavascriptFile("~/js/bewebcore/TextStatistics.js"); %>

<script>

	var seoPanelRecordURL = '<%=Model.Url%>';

	$(document).ready(function () {
		<% if (Model.DataRecord.FieldExists("PageTitleTag")) { %>
			$("#svyFocusResultWrapper").hide();
			$("#focusKeyWordPanel").hide();
			$("#FocusKeyword").keyup(showSuggest);
			svyShowFocusButton();
			$("#FocusKeyword").autocomplete({
				minLength: 3,
				delay: 1000,
				response: function (event, ui) {
					$('.svySpinner').hide();
				}
			});
		<% } %>
});

	function svyShowFocusButton() {		
		var keyword = GetFocusKeyword();
		if (keyword) {
			$("#focusKeywordScanBtn").show();
		} else {
			$(".focusKeywordBtn").hide();
			$("#focusKeyWordPanel").hide();
		}
		return false;
	}
	
	function svyInitFocusScan() {
		$("#focusKeyWordPanel").slideDown(250);	
		$('html,body').animate({ scrollTop: $("#focusKeyWordPanel").offset().top - 30 });
		$("#focusKeywordScanBtn").text("Analyse SEO Again");
		ShowFocusResults();
		ShowAnalysisResults();
		return false;
	}
	
	function svyHideFocusScan() {
		$("#focusKeyWordPanel").fadeOut(500);
		$("#focusKeywordScanBtn").show();
		$("#focusKeywordHideBtn").hide();
		$("#focusKeywordSaveBtn").hide();
		return false;
	}
	
	function svySaveFocus() {
		$("#focusSave").val("1");
		$("#form").submit();
	}

	function cleanText1(text) {
		// all these tags should be preceeded by a full stop. 
		var fullStopTags = ['li', 'p', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'dd'];

		fullStopTags.forEach(function (tag) {
			text = text.replace("</" + tag + ">", ".");
		})

		text = text
			.replace(/<[^>]+>/g, "")				// Strip tags
			.replace(/[,:;()\-]/, " ")				// Replace commans, hyphens etc (count them as spaces)
			.replace(/[\.!?]/, ".")					// Unify terminators
			.replace(/^\s+/, "")						// Strip leading whitespace
			.replace(/[ ]*(\n|\r\n|\r)[ ]*/, " ")	// Replace new lines with spaces
			.replace(/([\.])[\. ]+/, ".")			// Check for duplicated terminators
			.replace(/[ ]*([\.])/, ". ")				// Pad sentence terminators
			.replace(/\s+/, " ")						// Remove multiple spaces
			.replace(/\s+$/, "");					// Strip trailing whitespace

		text += "."; // Add final terminator, just in case it's missing.

		return text;
	}


	function  cleanText(text) {
		var inputText = text;
		var returnText = "" + inputText;

		//-- remove BR tags and replace them with line break
		returnText=returnText.replace(/<br>/gi, " ");
		returnText=returnText.replace(/<br\s\/>/gi, " ");
		returnText=returnText.replace(/<br\/>/gi, " ");

		//-- remove P and A tags but preserve what's inside of them
		returnText=returnText.replace(/<p.*>/gi, " ");
		returnText=returnText.replace(/<a.*href="(.*?)".*>(.*?)<\/a>/gi, " $2 ($1)");

		//-- remove all inside SCRIPT and STYLE tags
		returnText=returnText.replace(/<script.*>[\w\W]{1,}(.*?)[\w\W]{1,}<\/script>/gi, "");
		returnText=returnText.replace(/<style.*>[\w\W]{1,}(.*?)[\w\W]{1,}<\/style>/gi, "");
		//-- remove all else
		returnText=returnText.replace(/<(?:.|\s)*?>/g, " ");

		//-- get rid of more than 2 multiple line breaks:
		returnText=returnText.replace(/(?:(?:\r\n|\r|\n)\s*){2,}/gim, " ");

		//-- get rid of more than 2 spaces:
		returnText = returnText.replace(/ +(?= )/g,'');

		//-- get rid of html-encoded characters:
		returnText=returnText.replace(/&nbsp;/gi," ");
		returnText=returnText.replace(/&amp;/gi,"&");
		returnText=returnText.replace(/&quot;/gi,'"');
		returnText=returnText.replace(/&lt;/gi,'<');
		returnText=returnText.replace(/&gt;/gi,'>');
		//-- return
		return returnText;
	}

	function showSuggest() {
		var keyword = GetFocusKeyword();
		if (keyword.length >= 3) {
			var data = [];
			$('.svySpinner').show();
			$.ajax({
				type: "GET",
				async: true,
				url: websiteBaseUrl + "common/GetGoogleSuggest?searchTerm=" + keyword,
				dataType: "xml",
				success: function (xml) {
					$(xml).find('CompleteSuggestion').each(function () {
						data.push($(this).find('suggestion').attr('data'));
					});
					$("#FocusKeyword").autocomplete({
						source: data
					});
					$('.svySpinner').hide();
				}
			});
		}

	}

	function svyMatchedWordCount(string, substring) {
		if (substring == null || substring == 'undefined' || substring == '') {
			return 0;
		}
		var result = string.match(RegExp('(' + substring + ')', 'g'));
		return result ? result.length : 0;
	}

	function IsFocusKeywordMatch(field, focusKeyword) {
		var isMatch = false;
		if (focusKeyword == null || focusKeyword == 'undefined' || focusKeyword == '') {
			return false;
		}

		if (field.toLowerCase().indexOf(focusKeyword) > -1) {
			isMatch = true;
		}
		return isMatch;
	}
	
	function svyWriteFocusResult(label, foundMatch, count) {
		var answer = foundMatch ? "Yes" : "No";
		if (answer == "Yes" && count >= 1) {
			$("#focusKeywordResult").append("<span class='svyFocusTitle'>" + label + " : </span><span class='svyFocusKeywordOk'>" + answer + " </span>(<span class='svyFocusKeywordOk'>" + count + "</span>)<br>");
		} else {
			$("#focusKeywordResult").append("<span class='svyFocusTitle'>" + label + " : </span><span class='svyFocusKeywordError'>" + answer + " </span>(<span class='svyFocusKeywordError'>" + count + "</span>)<br>");
		}
	}

	function svyCountOcurrences(str, value) {
		var regExp = new RegExp(value, "gi");
		return str.match(regExp) ? str.match(regExp).length : 0;
	}

	function svyWriteAnalysisResult() {
		var content = svyGetAllMceContentPlainText().trim();
		var focusKeyword = GetFocusKeyword();
		if (focusKeyword) {
			svyKeywordCheck(content);
			svyMatchedWordCountCheck();
			svyAltTagCheck();
			svyMetaDescriptionCheck();
			svyMetaDescriptionKeywordCheck();
			svtTitleCheck();
			if ($("#PageUrl").length != 0) {
				svyUrlCheck();
			}
			svyH1Check();
			svyHtagContainKeywordCheck();
			svyReadabilityCheck(content);

		} else {
			$("#PageAnalysisResult").append("<span class='svyFocusTitle'>No focus keyword defined</span>");
		}
		// end reading ease score(s)

		$("#PageAnalysisResult").append("<span class='svyFocusTitle'></span>");

	}

	function svyKeywordCheck(content) {
		content = content.trim();
		var keywordDensity = 0;
		var wordCount = 0;
		var keywordCount = 0;
		var resultIcon = "";
		var textstats = new textstatistics(content);
		if (content.length > 0) {
			wordCount += textstats.wordCount();
		}
		keywordCount += svyCountOcurrences(content, GetFocusKeyword());

		if (wordCount > 0 && keywordCount > 0) {
			keywordDensity = (keywordCount * 100 / wordCount).toFixed(2);
		}
		var resultInfo = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Keyword Density</div>");
		if (keywordDensity < 2) {
			resultInfo = "This is low. You should use the keyword in the content more";
			resultIcon = "svyFocusOrange";
		}
		else if (keywordDensity >= 2 && keywordDensity < 10) {
			resultIcon = "svyFocusGreen";
			resultInfo = "This is good";
		}
		else if (keywordDensity >= 10) {
			resultIcon = "svyFocusOrange";
			resultInfo = "This is probably too much. You maybe penalised by google for overusing the keyword";
		}

		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Total words = " + wordCount + ". Total use of keyword = " + keywordCount + ". The keyword density is " + keywordDensity + "%, " + resultInfo + ".</span></div>");
	}

	function svyAltTagCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Alt Tag Results</div>");
		var altTagMessage = "";
		var isImagesWithNoAlt = false;
		var images = $("iframe").contents().find('#tinymce').find("img");
		if (images.length > 0) {
			$(images).each(function () {
				var imageHasAltTag = $(this).attr('alt');
				if (typeof imageHasAltTag == "undefined") {
					isImagesWithNoAlt = true;
					resultIcon = "svyFocusRed";
					altTagMessage += "<div class='svyfocusImageResultWrapper svyfocusImg'><span class='svyfocusIndent " + resultIcon + "'></span><span class='svyFocusInfo'><a target='_blank' href='" + $(this).attr('src') + "'>This image does not have an alt tag</a> <img align='middle' style='height:47px;padding-left:20px;' src = '" + $(this).attr('src') + "'/></span></div>";
				} else {
					if (imageHasAltTag != null && imageHasAltTag != 'undefined') {
						if (imageHasAltTag.toLowerCase().indexOf(GetFocusKeyword()) > -1) {
							resultIcon = "svyFocusGreen";
							altTagMessage += "<div class='svyfocusImageResultWrapper svyfocusImg'><span class='svyfocusIndent " + resultIcon + "'></span><span class='svyFocusInfo'><a target='_blank' href='" + $(this).attr('src') + "'>This image has an alt tag and contains your focus keyword</a> <img align='middle' style='height:47px;padding-left:20px;' src = '" + $(this).attr('src') + "'/></span></div>";	
						} else {
							resultIcon = "svyFocusOrange";
							altTagMessage += "<div class='svyfocusImageResultWrapper svyfocusImg'><span class='svyfocusIndent " + resultIcon + "'></span><span class='svyFocusInfo'><a target='_blank' href='" + $(this).attr('src') + "'>This image has an alt tag but does not contain your focus keyword</a> <img align='middle' style='height:47px;padding-left:20px;' src = '" + $(this).attr('src') + "'/></span></div>";
						}
					}
				}
			});
		} else {
			resultIcon = "svyFocusGreen";
			altTagMessage += "<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>No images found in the content.(This is ok)</span></div>";
		}

		if (isImagesWithNoAlt) {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Not all images have alt tags. We recommend all your images have alt tags.</span></div>");
		}
		$("#PageAnalysisResult").append(altTagMessage);
	}

	function svyHtagContainKeywordCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Other H Tags Results</div>");
		var hTagMessage = "";
		var allHTagsContainKeyword = true;
		var focusWord = GetFocusKeyword();
		var hTags = $("iframe").contents().find('#tinymce').find("H2,H3");
		var hText = "";
		if (hTags.length > 0) {
			$(hTags).each(function () {
				hText = cleanText($(this).html());
				//get first ten chars of the html
				var hTextValueShort = "";
				if (hText.length > 25) {
					hTextValueShort = hText.substring(0, 25) + "...";
				} else {
					hTextValueShort = hText;
				}

				if (hText.toLowerCase().indexOf(focusWord) > -1) {
					resultIcon = "svyFocusGreen";
					hTagMessage += "<div class='svyfocusImageResultWrapper'><span class='svyfocusIndent " + resultIcon + "'></span>" +
						"<span class='svyFocusInfo'>The h tag beginning with the text [" + hTextValueShort + "] contains the keyword</span></div>";
				} else {
					allHTagsContainKeyword = false;
					resultIcon = "svyFocusOrange";
					hTagMessage += "<div class='svyfocusImageResultWrapper'><span class='svyfocusIndent " + resultIcon + "'></span>" +
						"<span class='svyFocusInfo'>The h tag beginning with the text [" + hTextValueShort + "] does not contain the keyword</span></div>";
				}
			});
		} else {

			resultIcon = "svyFocusOrange";
			hTagMessage += "<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span>" +
								"<span class='svyFocusInfo'>No subheadings (H tags) found within the document body</span></div>";
		}

		if (!allHTagsContainKeyword) {
			resultIcon = "svyFocusOrange";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Not all subheadings (H tags) contain your keyword.</span></div>");
		}
		$("#PageAnalysisResult").append(hTagMessage);

	}

	function svyMetaDescriptionCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Meta Description Length</div>");
		var metaDescriptionLength = $("#MetaDescription").val().length;

		if (metaDescriptionLength < 120) {
			resultIcon = "svyFocusOrange";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your meta description length (" + metaDescriptionLength + ") is less that 150 charcters but you have up to 156 characters available.</span></div>");
		}
		else if (metaDescriptionLength >= 120 && metaDescriptionLength < 157) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your meta description length (" + metaDescriptionLength + ") is good.</span></div>");
		}
		else if (metaDescriptionLength >= 157) {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your meta description length (" + metaDescriptionLength + ") is too long. Make sure it does not exceed 157 characters</span></div>");
		}
	}

	function svyMetaDescriptionKeywordCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Meta Description Keyword Check</div>");
		var metaDescription = $("#MetaDescription").val();
		var keyword = GetFocusKeyword();
		if (metaDescription.toLowerCase().indexOf(keyword) > -1) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your keyword is in the meta description.</span></div>");
		}
		else {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your keyword is not in the meta description.</span></div>");
		}
	}

	function svyReadabilityCheck(content) {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Reading Ease</div>");
		
		var textstats = new textstatistics(content);
		var readability = textstats.fleschKincaidReadingEase(svyGetAllMceContentPlainText()).toFixed(2);
		var readabilityResultText = "";
		if (content.trim() == '') {
			resultIcon = "svyFocusRed";
			readabilityResultText = "Not available. You have added no content";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" + readabilityResultText + "</span></div>");
			return;
		} 
		if (readability < 29) {
			resultIcon = "svyFocusRed";
			readabilityResultText = "Your content is considered 'very hard' to read. Try using words with less syllables and shorten sentences.";
		} else if (readability >= 30 && readability < 50) {
			resultIcon = "svyFocusRed";
			readabilityResultText = "Your content is considered 'difficult' to read. Try using words with less syllables and shorten sentences.";
		} else if (readability >= 50 && readability < 60) {
			resultIcon = "svyFocusOrange";
			readabilityResultText = "Your content is considered 'fairly difficult' to read. Try using words with less syllables and shorten sentences.";
		} else if (readability >= 60 && readability < 70) {
			resultIcon = "svyFocusGreen";
			readabilityResultText = "Your content is considered 'standard' to read.";
		} else if (readability >= 70 && readability < 80) {
			resultIcon = "svyFocusGreen";
			readabilityResultText = "Your content is considered 'fairly easy' to read.";
		} else if (readability >= 80 && readability < 90) {
			resultIcon = "svyFocusGreen";
			readabilityResultText = "Your content is considered 'easy' to read.";
		} else if (readability >= 80) {
			resultIcon = "svyFocusGreen";
			readabilityResultText = "Your content is considered 'very easy' to read.";
		}
		
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"The copy scores " + readability + " in the Flesch Reading Ease test. " + readabilityResultText + "</span></div>");
	}

	function svyUrlCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Url Check</div>");
		var url = seoPanelRecordURL.toLowerCase();
		var focusWord = GetFocusKeyword();
		if (url.toLowerCase().indexOf(focusWord) > -1) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"The keyword appears in the url</span></div>");
		} else {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"The keyword does not appear in the url</span></div>");
		}
	}


	function svyMatchedWordCountCheck() {
		var resultIcon = "";
		var wordCount = 0;
		var content = svyGetAllMceContentPlainText().trim();
		var textstats = new textstatistics(content);
		if (content.length > 0) {
			var wordCount = textstats.wordCount();
		}
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Word Count Check</div>");

		if (wordCount >= 300) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"There are " + wordCount + " words in the main content which	is greater than the recomended minimum 300.</span></div>");
		} else if (wordCount < 300) {
			resultIcon = "svyFocusOrange";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"There are only " + wordCount + " words in the main content which	is less than the recomended minimum 300.</span></div>");
		}
	}

	function svyH1Check() {
		var resultIcon = "";
		//debugger JN - DONT PUT THIS IN JS CODE FFS
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>H1 Check</div>");
		var title = $("#Title").val().toLowerCase();
		var focusWord = GetFocusKeyword().toLowerCase();
		if (title.toLowerCase().indexOf(focusWord) > -1) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"The title appears in the H1 tag</span></div>");
		} else {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>" +
				"The keyword does not appear in the h1 tag</span></div>");
		}
	}

	function svtTitleCheck() {
		var resultIcon = "";
		$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper pageAnalysisHeader'>Title Length</div>");
		var titleLength = $("#Title").val().length;
		if (titleLength < 41) {
			resultIcon = "svyFocusOrange";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your title length (" + titleLength + ") is less that 40 charcters. You have up to 70 characters available.</span></div>");
		}
		else if (titleLength >= 41 && titleLength < 71) {
			resultIcon = "svyFocusGreen";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your title length (" + titleLength + ") is good.</span></div>");
		}
		else if (titleLength >= 71) {
			resultIcon = "svyFocusRed";
			$("#PageAnalysisResult").append("<div class='svyfocusImageResultWrapper'><span class='" + resultIcon + "'></span><span class='svyFocusInfo'>Your title length (" + titleLength + ") is too long. Make sure it does not exceed 70 characters</span></div>");
		}
	}

	//gets all current mce content - does not need a save first
	function svyGetAllMceContent() {
		return $("iframe").contents().find('.mceContentBody').html();
	}

		//gets all current mce content
	function svyGetAllMceContentPlainText() {
		//return $("iframe").contents().find('.mceContentBody').text(); //cant use this because the spacing comes back incorrectly (eg no space bewteen li tags)
		var content = "";
		$("iframe").contents().find('.mceContentBody').contents().each(function () {
			content += cleanText($(this).text() + " ");
		});
		return content;
	}

	//gets current mce content for a given id - does not need a save first
	function svyMceContent(id) {
		return $("#" + id + "_ifr").contents().find('#tinymce').html();
	}

	$.fn.hasAttr = function (name) {
		return this.attr(name) !== undefined;
	};

	function GetInputLabel(id) {
		//ideally we would get the label-for here but as our admin system does not use th 'label for's (or even a label tag) am using 'data' attr. 
		//If it doesnt have a data attr then fall back uses the id and inserts white space before each papital letter.
		var label = $('label[for="' + id + '"]').text();
		if (label == null || label == 'undefined' || label == '') {
			label = $("#" + id).attr('data-focusLabel');
		}
		if (label == null || label == 'undefined' || label == '') {
			label = id.replace(/([a-z])([A-Z])/g, '$1 $2');
		}
		return label;
	}

	function svyScanFocusFields(el,label) {
		var elId = el.attr("id");
		var text = "";
		if (label == '') {
			label = GetInputLabel(elId);
		}
		if ($("#" + elId).hasAttr('value')) {
			//text = cleanText($("#" + elId).attr('value'));
			text = $("#" + elId).val();
		} else {
			text = cleanText($("#" + elId).html());
		}
		GetFocusResults(text, label);
	}
	
	
	function GetFocusResults(text, label) {
		text = text.toLowerCase().trim();
		var focusKeyword = GetFocusKeyword();
		var foundMatch = IsFocusKeywordMatch(text, focusKeyword);
		var matchedCount = svyMatchedWordCount(text, focusKeyword);
		svyWriteFocusResult(label, foundMatch, matchedCount);
	}

	function ShowFocusResults() {
		$("#focusKeywordResult").html("");
		var focusKeyword = GetFocusKeyword();
		if (focusKeyword) {
			$("#svyFocusResultWrapper").show();
			$("#focusKeywordResult").html("");
			//get all elements with class of svyFocusCompareText
			$("#focusKeywordResult").append("<span class='focusKeyWordHeader'>Your focus keyword was found in:</span><br><br>");

			//do mce..
			var content = svyGetAllMceContentPlainText();
			GetFocusResults(content, "Body Content")
			//do the others with class of svyFocusField
			$('.svyFocusField:visible').each(function () { //cant use visible here beause text areas arent visible				
					svyScanFocusFields($(this),'');
			});
			ShowSnippet();
		} else {
			$("#focusKeywordResult").append("<span class='svyFocusTitle'>No focus keyword defined</span>");
		}
	}

	function ShowSnippet() {
		$("#focusKeywordSnippet").html("");

		var text = $("iframe").contents().find('.mceContentBody').first().text();
		//else use body text
		
		var focusText = GetFocusKeyword();

		//find keyword in text (if it exists)

		var regExpText = new RegExp(focusText, 'g');
		text = text.replace(regExpText, "<b>" + GetFocusKeyword() + "</b>");
		text = svyShortenSnippet(text);

		var url = seoPanelRecordURL.toLowerCase();
		var regExpTextUrl = new RegExp(focusText, 'g');
		url = url.replace(regExpTextUrl, "<b>" + GetFocusKeyword() + "</b>");

		var title = '<%=Model.Title%>' + ' - <%=Util.GetSiteName().HtmlEncode()%>';
		var regExpTextTitle = new RegExp(focusText, 'g');
		title = title.toLowerCase().replace(regExpTextTitle, "<b>" + GetFocusKeyword() + "</b>");


		$("#focusKeywordSnippet").html("<span class='snipperHeader'>" + title + "" +
			"</span><br><span class='snipperLink'>" + url + "" +
			"</span><br><span class='snipperDesc'>" + text + "</span>");
	}

	function GetFocusKeyword() {
		return $.trim($("#FocusKeyword").val().toLowerCase());
	}

	function svyShortenSnippet(text) {
		var textLen = text.length;
		if (textLen > 500) {
			//if the text contains the FIRST focus word
			var focusHightlightText = GetFocusKeyword();
			var wordPos = text.toLowerCase().indexOf(focusHightlightText);

			if (wordPos > -1 && wordPos > 500) {
				//find where its used and how much over the limit is the word?
				//var newWordPos = textLen - wordPos;
				//start the description with the word kinda!)
				text = text.substring(wordPos - 50, wordPos + 450);
				//rewind to the first space
				var spaceIndex = text.indexOf(" ");
				text = "..." + text.substring(spaceIndex, text.length);
			} else {
				text = text.substring(0, 500);
			}
		}
		return text;
	}

	function ShowAnalysisResults() {
		$("#PageAnalysisResult").html("");
		svyWriteAnalysisResult();
	}
	
</script>

<%var record = Model.DataRecord; %>
<%if(Model.DataRecord.FieldExists("PageTitleTag")){ %>
	<input type="hidden" name="focusSave" id="focusSave" value="0"/>
	<%if(Model.ShowHeader) {%>
		<tr class="<%=Model.CssTablerowClass %>">
			<td class="label section"><strong>Search Engine Optimisation</strong></td>
			<td class="section"></td>
		</tr>
	<%}%>
	<tr class="<%=Model.CssTablerowClass %>">
		<td class="label">Page Title Tag:</td>
		<td class="field"><%=new Forms.TextField("PageTitleTag",Model.GetValue(record,"PageTitleTag"), false){cssClass = "svyWideText"}  %></td>
	</tr>
	<tr class="<%=Model.CssTablerowClass %>">
		<td class="label">Meta Keywords:</td>
		<td class="field"><%=new Forms.TextField("MetaKeywords",Model.GetValue(record,"MetaKeywords"), false){cssClass = "svyWideText"} %></td>
	</tr>
	<tr class="<%=Model.CssTablerowClass %>">
		<td class="label">Meta Description:</td>
		<td class="field"><%= new Forms.TextArea("MetaDescription",Model.GetValue(record,"MetaDescription") ,false){cssClass = "svyWideText" }  %></td>
	</tr>
	<tr class="<%= Model.CssTablerowClass %>">
		<td class="label">
			Focus Keyword:  <%=Html.SavvyHelp("The focus keyword is the topic of your page or post. Restrict this to one word as a good page only explores one topic. The Analyse SEO feature will scan your page for correct SEO usage of your keyword.")%>
		</td>
		<td class="field focusSearch">
			<span style="float:left;">
				<%= new Forms.TextField("FocusKeyword", Model.GetValue(record,"FocusKeyword"), false) {onblur = "svyShowFocusButton()"} %>
			</span>				
			<div class="svyScanButtonWrapper"><a href="#" class="focusKeywordBtn btn btn-primary btn-link" id="focusKeywordScanBtn" onclick="return svyInitFocusScan();">Analyse SEO</a></div> 					
	
			<span  style="float:left;" class="svySpinner"></span>
		</td>
	</tr>		
			
	<tr id="focusKeyWordPanel">
		<td class="label"></td>
		<td>
			<div id="svyFocusResultWrapper">
										
				<div class="focusHeader" style="padding-top: 20px;">Snippet Search Preview</div>
				<div id="focusKeywordSnippet"></div>
									
			</div>
			<div class="focusHeader">Focus Keyword Results</div>
			<div class="svyFocasResultWrapper">
				<div id="focusKeywordResult"></div>
			</div>

			<div class="focusHeader">Page Analysis Results</div>
			<div class="svyFocasResultWrapper">
				<div id="PageAnalysisResult"></div>
			</div>
		</td>
	</tr>
		
<% } %>
	
