<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<%@ Page Theme=""  Language="C#" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

	<title>You Tube Embed</title>
	<link href="css/embed.css" rel="stylesheet" type="text/css" />
	<script language="JavaScript" type="text/javascript" src="../../tiny_mce_popup.js"></script>
	<%Util.IncludejQuery(); %>
	<%Util.IncludeJavascriptFile("js/youtube.js"); %>
	<link href="<%=Web.Root %>areas/admin/admin.css" rel="stylesheet" type="text/css" />


	<base target="_self" />

</head>

<body class="svyVideoEmbed" style="background-color: white !important;">
	
	<h1 class="dataheading">Video Embed</h1>
	
	<div class="mainContent">
		<div id="error" style="display: none;"></div>
		<div class="fullPath">
			<input id="fullPathRadio" type="radio" onclick="showInput(this)" name="embedType" class="embedRadio"/>
			Enter the 
			<a href="#" class="fullPathHelper"><div class="helper" style="display: none;">eg: 'https://www.youtube.com/watch?v=ZnuwB35GYMY'</div>full path</a> of the video
			
		</div>
		<div class="fullPathInputWrapper inputWrapper" style="display: none;">
			<input type ="text" value = "" id="fullPathInput" style="width: 350px;"/>
		</div>
		<span class="or">or</span>
		<div class="embedPath">
			<input id="embedPathRadio" onclick="showInput(this)" type="radio" name="embedType" class="embedRadio"/>
			Enter the
			<a href="#" class="fullPathHelper"><div class="helper" style="display: none;">eg: 'ZnuwB35GYMY'</div>code only</a>
		</div>
		<div class="embedPathInputWrapper inputWrapper" style="display: none;">
			<input type ="text" value = "" id="embedPathInput" style="width: 100px;"/>
		</div>
		<span class="or">or</span>
		<div class="embedPathComplete">
			<input id="embedPathCompleteRadio" type="radio" onclick="showInput(this)" name="embedType" class="embedRadio"/>
			Enter the
			<a href="#" class="fullPathHelper"><div class="helper" style="display: none;">eg: <img src="images/embed-example.png" align="top"/></div>full embedded code only</a>
		</div>
		<div class="embedPathCompleteInputWrapper  inputWrapper"  style="display: none;">
			<textarea name="address"cols="52" rows="5" id="embedPathCompleteInput"></textarea>
		</div>
	</div>
	
	<div class="dimensionsWrapper inputWrapper" style="display: none;">
		<span class="label">Width</span> <input class="dimension" type ="text" value = "" id="width"/> <span class="label">Height</span> <input class="dimension" type ="text" value = "" id="height" /> <span class="label">Autoplay</span> <input type ="checkbox" value = "" id="autoplay" class="autoplayCheck" />
		
		<a href="#" id="padding" class="" style="float: left;margin: 10px 0;" onclick="showHidePadding()">Padding around video?</a>
		
		
		<div class="padding" style="clear: both;margin-top: 10px;display: none;">
			
			<span class="label">Top</span> <input class="coOrds" type ="text" value = "0" id="top" style="width: 30px;"/>
			<span class="label">Bottom</span> <input class="coOrds" type ="text" value = "0" id="bottom" style="width: 30px;"/>
			<span class="label">Left</span> <input class="coOrds" type ="text" value = "0" id="left" style="width: 30px;"/>
			<span class="label">Right</span> <input class="coOrds" type ="text" value = "0" id="right" style="width: 30px;"/>
		</div>
		
	</div>
	
	<div class="previewWrapper" style="display: none;">
		<a href="#" class="btn" onclick="renderVideo(false)">Preview</a>
		<a href="#" id="embed" class="btn" style="float: right;display: none;" onclick="renderVideo(true)">Embed</a>
	</div>

	
	<div id="previewVideo" style="display: none;outline: 4px dotted #ccc;margin-top: 20px;min-width: 800px;max-width: 800px;">
	</div>
	

	

</body>
</html>
