<%@ page title="Crop" language="C#" CodeFile="svyCropEdit.aspx.cs" Inherits="svyCropEdit"%>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Import Namespace="System.IO" %>
<%@ import namespace="Beweb" %>
<!DOCTYPE html>
<html lang="en">
<head>
	<title>Jcrop</title>
	<meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
	<script src="js/jquery.min.js"></script>
	<script src="js/jquery.color.js"></script>
	<script src="js/jquery.Jcrop.min.js"></script>
	
	<% Util.Include("areas/admin/admin.css");%>
	<script type="text/javascript">
		$(document).ready(function () {
			fieldName = '<%= Request["fieldName"]%>';
			targetId = '<%= Request["targetId"]%>';
			readData(fieldName);
		});

		var base64Str;
		var fieldName;
		var targetId;
		var image;
		var jcropObj;

		var bigcanvas = document.createElement('canvas');
		var previewCanvas = document.createElement('canvas');
		var metaWidth = 0;
		var metaHeight = 0;
		var aspectRatio;
		var bgcolor = 'pink';

		function DoCrop() {
			var fieldName = '<%= Request["fieldName"]%>';
			var selection = jcropObj.tellSelect();
			imgSelect(selection, true);
			window.parent.svyDoCrop(fieldName, previewCanvas.toDataURL(), null, targetId);
		}

		function readData(fieldName) {
			var i = new Image();

			var field = window.parent.$("#file_" + fieldName);
			if (field.attr("data-meta-width")) {
				metaWidth = parseInt(field.attr("data-meta-width"));
			}
			if (field.attr("data-meta-height")) {
				metaHeight = parseInt(field.attr("data-meta-height"));
			}
			if (field.attr("data-meta-backgroundcolor")) {
				bgcolor = field.attr("data-meta-backgroundcolor");
			}
			aspectRatio = metaWidth / metaHeight;

			var data = window.parent.svyGetImagefromHTML5Preview(fieldName);
			i.onload = function () {
				var sourceWidth = i.width; 
				var sourceHeight = i.height;
				var canvasWidth = 300;
				if (sourceWidth > canvasWidth) {
					bigcanvas.width = canvasWidth;
				} else {
					$('#message').html('The image you have chosen is too small. Please choose a larger image.')
					bigcanvas.width = sourceWidth;
					$(".cropcolumn").hide();
				}
				bigcanvas.height = i.height * (bigcanvas.width / i.width);
				var ctx = bigcanvas.getContext('2d');
				ctx.drawImage(i, 0, 0, bigcanvas.width, bigcanvas.height);

				$('#image_input').html(['<img src="', bigcanvas.toDataURL(), '"/>'].join(''));
				//$('#image_input').html(['<img src="', data, '"/>'].join(''));
				// todo - this is the wrong source to use for cropping
				// MN 20141221 - I noticed the crop popup really degrades the quality to unusable. I've found the issue, it is pulling in the original image and then cutting it to 300px before all the cropping. This one is then used for the final crop, instead of referring back to the original. The "html5 preview" image is good, but the "bigcanvas" is only the crop area shown on screen. Presumably jcrop must have some way of using a bigger original than is shown and calculating the rectange to grab, or maybe we just need to calculate that I guess it isn't hard. Anyway in the meantime I recommend set ShowCropWindow = false as it is not good at all

				$('#image_input img').Jcrop({
					bgColor: bgcolor,
					bgOpacity: .6,
					setSelect: getDefaultRect(metaWidth, metaHeight, sourceWidth, sourceHeight),
					aspectRatio: aspectRatio,
					onSelect: imgSelect,
					onChange: imgSelect
				}, function() {
					jcropObj = this;
				});
				
			}
			if (data && data.indexOf('data:image') != -1) {
				//$('#target').attr('src', data);
				base64Str = data;
				i.src = data

			}
		}

		function imgSelect(selection, doFullSize) {
			var cropSize = 100;
			if (doFullSize) {
				cropSize = metaHeight;
			}
			previewCanvas.width = cropSize * (aspectRatio);
			previewCanvas.height = cropSize

			var img = $('#image_input img')[0];
			var context = previewCanvas.getContext('2d');
			context.drawImage(img, selection.x, selection.y, selection.w, selection.h, 0, 0, previewCanvas.width, previewCanvas.height);

			//$('#image_output').attr('src', previewCanvas.toDataURL());
		}

		function getDefaultRect(maxWidth, maxHeight, sourceWidth, sourceHeight) {
			var newAspect = maxWidth / maxHeight;
			var sourceAspect = sourceWidth / sourceHeight;
			var newWidth;
			var newHeight;
			var enlargeIfTooSmall = true;  // todo - not sure if we need a special case for too small?

			if (sourceAspect >= newAspect) {
				// trim sides
				if (sourceHeight < maxHeight && !enlargeIfTooSmall) {
					// don't enlarge it - the image is smaller than our max
					newHeight = sourceHeight;
				} else {
					newHeight = bigcanvas.height;
				}
				newWidth = (newHeight * newAspect);
			} else {
				// trim top and bottom
				if (sourceWidth < maxWidth && !enlargeIfTooSmall) {
					// don't enlarge it - the image is smaller than our max
					newWidth = sourceWidth;
				} else {
					newWidth = bigcanvas.width;
				}
				newHeight = (newWidth / newAspect);
			}

			var left = bigcanvas.width / 2 - newWidth / 2;
			var top = bigcanvas.height / 2 - newHeight / 2;
			var bottom = top + newHeight;
			var right = left + newWidth;
			var rect = [left, top, right, bottom];
			if(window.console)console.log(rect);
			return rect;
		}

	</script>


	<link rel="stylesheet" href="<%=Web.Root%>js/tapmodo-Jcrop/demos/demo_files/main.css" type="text/css" />
	<link rel="stylesheet" href="<%=Web.Root%>js/tapmodo-Jcrop/demos/demo_files/demos.css" type="text/css" />
	<link rel="stylesheet" href="<%=Web.Root%>js/tapmodo-Jcrop/css/jquery.Jcrop.css" type="text/css" />
	<link rel="stylesheet" href="<%=Web.Root%>css/custom-font.css" type="text/css" />
	<style type="text/css">
		/* Apply these styles only when #preview-pane has
   been placed within the Jcrop widget */
		.jcrop-holder #preview-pane
		{
			display: block;
			position: absolute;
			z-index: 2000;
			top: -7px;
			right: -273px;
			padding: 6px;
			border: 1px rgba(0,0,0,.4) solid;
			background-color: white;
			-webkit-border-radius: 6px;
			-moz-border-radius: 6px;
			border-radius: 6px;
			-webkit-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2);
			-moz-box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2);
			box-shadow: 1px 1px 5px 2px rgba(0, 0, 0, 0.2);
		}


		.cropcolumn { float:left;width:299px;max-width: 100%;padding: 10px; }
		#message {
		background: url(../../images/sadface.png) 50% 75px no-repeat;
		font-family: 'Gotham Rounded SSm A', 'Gotham Rounded SSm B';
		font-weight: 300;
		font-style: normal;
		font-size: 16px;
		color: #833384;
		text-decoration: none;
		padding: 10px 0;
		height: 250px;
		}

		.sbtn, .btn { margin: 0; background-color: #02aff0; color: #fff !important; display: block; border: none; cursor: pointer; text-align: center; height: 26px; padding: 0 0px; font-size: 14px;font-family: 'Calibri, Candara, Segoe, 'Segoe UI', Optima, Arial, sans-serif;'; font-weight: 300; font-style: normal; text-decoration: none !important; padding-top: 10px; width: 123px; }
		.sbtn-small { padding-top: 0; width: 70px; height: 24px; line-height: 24px; }
		.sbtn-normal { width: 81px; height: 28px; line-height: 20px; padding: 5px 10px; }
		.sbtn-normal:hover { background-color: #7e2b7f; }

	</style>

</head>
<body>
	
	<div>
		<div class="cropcolumn">
			<div id="image_input"></div>
		</div>	
		<div class="cropcolumn" style="padding-left:20px;width: 100px;">
			<img id="image_output" style="border:1px solid #000;max-width:100px;max-height: 300px;width:100px"/>
			<br/>	
			<a class="sbtn sbtn-small"  onclick="DoCrop();return false;" href="#" style="float: right;width: 100px;">OK</a>
		</div>

	</div>	
	
	<div class="clearfix"></div>
	<div id="message" style="display: inline-block;"></div>

</body>
</html>

