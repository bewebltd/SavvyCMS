<%@ Page Title="Shopping cart" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.ShoppingCartController.ViewModel>"
	MasterPageFile="~/site.master" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent">
	<%Util.IncludejQuery(); %>
	<link href="<%=Web.Root%>js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
	<link href="<%=Web.Root %>js/jquery-ui-1.8.4/css/redmond/jquery-ui-1.8.4.css" rel="stylesheet" type="text/css" />
	<script src="<%=Web.Root %>js/jquery-ui-1.8.4/js/jquery-ui-1.8.4.js" type="text/javascript"></script>
	<script src="<%=Web.Root%>js/colorbox/jquery.colorbox.js" type="text/javascript"></script>
	<script type="text/javascript">
	//<![CDATA[

		//global vars here
		var userToken = '<%=Crypto.EncryptID(Security.LoggedInUserID) %>';
		
		var isDevMode=<%=Util.ServerIsDev?"true":"false" %>;
		var rootURL='<%=Web.Root %>';
		var plus = "<img src='" + rootURL + "images/plus.gif'>"
		var arrow = "<img src='" + rootURL + "images/arrow.png'>"
		var spinner = "<img src='" + rootURL + "images/spinner.gif' width='16' height='16' class='spinner'>"
		var tickImage = new Image()
		tickImage.src=rootURL+'images/shopping_tick.gif';
		var encryptedUserToken = '<%=Crypto.EncryptID( Models.PersonList.LoadAll()[0].ID) %>'
		function pageInit(){
			showCart(false, encryptedUserToken) 
		}

function handleCartRemove(removeBtn, encryptedPartID, encryptedUserToken) {
	$(removeBtn).html(spinner);


	var url = rootURL + 'ShoppingCart/CartRemoveLine/?encryptedShoppingCartID=' + escape(encryptedPartID) + '&encryptedUserToken=' + escape(encryptedUserToken) + '&rnd='+Math.random(250);
	$.ajax(
	{
		type: "GET",
		url: url,
		success: function (msg) {
			if (msg.indexOf('OK')!=-1) { //ok there somewhere
				$(removeBtn).parent().parent().hide()								//<tr><td><a>  - tr is parent.parent of a
			} else {
				errorMsg("failed to remove from cart1: " + msg);
			}
		},
		error: function (msg) {
			errorMsg("failed to remove from cart2: " + msg);
			//prompt('copy this',url+'?'+qs)
		}
	});

	return false;
}
function handlePlaceOrder(placeButton, encryptedUserToken) {
	//if (CheckCustomerOrderReference()) {

		var url = rootURL + 'ShoppingCart/PlaceOrder/' +
			'?encryptedUserToken=' + escape(encryptedUserToken) +
			'&notes=' + escape($('#cartNotes').val()) +
			'&coref=' + escape($('#CustomerOrderReference').val()) + 
			'&rnd=' + Math.random(250) + '';
		var holdText = $(placeButton).val();
		$(placeButton).val(' Sending order request... ');
		$(placeButton).attr('disabled', 'disabled')
		$.ajax(
		{
			type: "GET",
			url: url,
			success: function (msg) {
				$('.CartContents').html(msg)
				$(placeButton).val(holdText);
				$(placeButton).removeAttr('disabled')
			},
			error: function (msg) {
				$(placeButton).val(holdText);
				$(placeButton).removeAttr('disabled')

				errorMsg("failed to place order: " + msg);
				//prompt('copy this',url+'?'+qs)
			}
		});
	//}
	return false;
}

function handleCart(basketHrefObj, partNum, encryptedUserToken, PartDescription, vehID) {
	if (partNum=='') alert('handleCart: part num is blank')
	var qs = ""+
		"partNumber=" + escape(partNum) +
		"&encryptedUserToken=" + escape(encryptedUserToken) +
		"&PartDescription=" + escape(PartDescription) +
		"&VehID=" + escape(vehID) +
		"&rnd=" + Math.random(250);

	var url = rootURL + "ShoppingCart/AddToCart/";				

	var animObj = $('img', basketHrefObj)[0]

	var productW = $(animObj).width();
	var productH = $(animObj).height();

	var productX = $(animObj).offset().left;
	var productY = $(animObj).offset().top;
		
	var basketX = $("#basketTitleWrap").offset().left;
	var basketY = $("#basketTitleWrap").offset().top;
	
	var gotoX = basketX - productX;
	var gotoY = basketY - productY;

	var newImageWidth = 10; //$("#productImageWrapID_" + productIDVal).width() / 3;
	var newImageHeight = 10; //$("#productImageWrapID_" + productIDVal).height() / 3;
	$(basketHrefObj).html(spinner)

	$(animObj)
		.clone()
		.prependTo(basketHrefObj)
		.css({ 'position': 'absolute'})
		.animate({ opacity: 0.5, marginLeft: gotoX, marginTop: gotoY, width: newImageWidth, height: newImageHeight }, 1200, function () {
			//anim completed
			$(this).remove();

			$.ajax(
			{
				type: "GET",
				url: url,
				data: qs,
				success: function (msg) {
					$('img', basketHrefObj).width(productW);
					$('img', basketHrefObj).height(productH);

					$('img',basketHrefObj).attr('src', tickImage.src)
				},
				error: function (msg) {
					errorMsg("failed to add to basket: " + msg);
					//prompt('copy this',url+'?'+qs)
				}
			});//ajax

		}); //anim


	return false;	
}


function showCart(showBtn, encryptedUserToken) {
	$('.CartPanel').show();
	$('.LowerPanel').hide();
	$('.FindByPartNumberPanel').hide();		//hide parts
	$('.IllustrationPanel').hide(); 			//hide img gallery
	$('.FindByPartNumberPanel').hide(); 	//hide partnum

	if (showBtn) {
		$(showBtn).parent().hide(); //hide button clicked, and line it's in
		$('#showpartsbtn').parent().show(); //show alternate button
	}
	$('.CartContents').html(spinner);

	var url = rootURL + 'ShoppingCart/GetCartContents/?encryptedUserToken=' + escape(encryptedUserToken)+'&rnd='+Math.random(250);
	$.ajax(
	{
		type: "GET",
		url: url,
		success: function (msg) {
			$('.CartContents').html(msg)
		},
		error: function (msg) {
			alert("failed to load cart: " + msg.responseText);
			//prompt('copy this',url+'?'+qs)
		}
	});

	return false;
}


		var blankResultsTemplate = null;// used to hold the html for the blank parts results panel until the user clears the search

		//when page is ready
		$(document).ready(function () {
			//set height for Chrome & Safari only
			if($.browser.webkit){setInterval ( "setAdvResultHeightWebKit()", 20);}
			pageInit();
			<%if(!Util.ServerIsDev){%>
				//prevent copy/paste
				$('body').bind('copy', function (e) {
					alert('Sorry, copy not allowed. Please note that Paste is allowed.')
					e.preventDefault();
				});

			<%} %>
			KeepUsersLoggedIn();
		})		
		function setAdvResultHeightWebKit(){			
			$("ul.veh li").css("height","22px")								
			$("ul.veh li.engine").css("height","53px")											
		}
		
		function KeepUsersLoggedIn() {
			$.get(rootURL+'services/keepSession.aspx', { "r": Math.random });
			window.setTimeout('KeepUsersLoggedIn()', 600000); // 10 mins
		}

	//]]>
	</script>
</asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">
	<form name="form" method="post" action="Browse"	id="form">

	<div id="header">
		<ul id="right-nav">
			<li><a href="#" title="open the shopping list" id="showcartbtn" onclick="return showCart(this,userToken);">
				<span id="carttotal"></span><span id="basketTitleWrap">Shopping List</span></a></li>
		</ul>
	</div>
	<div class="CartPanel">
		<input type="button" id="hidecartbtn" value="Back to Parts" onclick="return hideCart();"
			class="right" />
		<h2>
			Shopping List</h2>
		<div class="CartContents">
		</div>
	</div>
	</form>
</asp:Content>
