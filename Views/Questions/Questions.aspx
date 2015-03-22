<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.QuestionsController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Models" %>

<asp:Content runat="server" ID="Content2" ContentPlaceHolderID="BodyContent">
	<script type="text/javascript">
		$(document).ready(function () {
			$('li.questions a').addClass('selected');
			$('.menuQn').addClass('selected');
			//$('#faqheader').html('')
			var linkID = 0;

			$('#searchResults').hide();
			$('.faq').hide();
			$('.faq').each(function () {
				var sec = this;
				$(this).find('h3').each(function () {
					$(sec).find('.faqheader').append('<a href="#' + linkID + '">' + $(this).html() + '</a><br/><br/>');
					$(this).before('<a name="' + linkID + '"></a>');
					linkID++;
				});
			});

			$('#faq1').show();
		});
		function searchFunction(txtObj) {
			$('.faq').hide();								//hide scections
			$('.faqheader').hide(); 							//hide nav items
			$('#searchResults').html('').show(); 						//clear last results

			if ($(txtObj).val().length > 2) {
				$('.col2 .faq span').each(function () {
					//check content
					if ($(this).html().toLowerCase().indexOf($(txtObj).val().toLowerCase()) != -1) {		 //if contains search text
						$('#searchResults').append($(this).html()); //add to results
					}
				});
			} else {
				//$('#searchResults').html('Search results...');
				$('#faq1').show(); //just make it show faq1
				$('.faqheader').show();
			}
		}
		function ShowSection(sectionNumber, ele)	{
			$('.faq').hide(500);
			$('.faqheader').show(); 							//hide nav items

			$('#faq' + sectionNumber).show(500);

			$.each($('.questionLinks a'), function () {
				if ($(this).hasClass('active')) {
					$(this).removeClass('active');
				}
			});
			
			$(ele).addClass('active');
			return false;
		} 
	</script>
	<div class="right_col_wrapper">
		<div class="top"></div>
		<div class="col2 right_col">
			<div id="searchResults" class="faq"></div>
				<%foreach (var faqSection in Model.Questions.GetResults()) {%> 	
					<div id="faq<%=faqSection.FAQSectionID%>" class="faq">
						<div class="faqheader"></div>
						<% int counter = 0; %>
						<%foreach (var faqItem in Models.FAQItemList.LoadByFAQSectionID(faqSection.FAQSectionID)) {%>
							<% if (counter < 1) {%>
							<% }%>
							<span><h3><%=faqItem.FAQTitle%></h3>
							<p><%=faqItem.BodyTextHTML%></p></span>
							<% counter++; %>	
						<% } %> 		
					</div>
				<%} %> 
		</div>		
		<div class="btm"></div>
	</div>

	<div class="left_col_wrapper">
		<div class="top"></div>
		<div class="col1 left_col">
			<h1>Got a Question?</h1>
			<form action="" class="searchForm" onsubmit="return false">
				<input type="text" name="searchTxt" value="" class="field" onkeyup="searchFunction(this)" />
				<input type="submit" id="search" value="" onclick="return false" />
			</form>

			<div class="questionLinks">
				<%foreach (var listItem in Model.Questions.GetResults()) {%> 
					<a href="" onclick="return ShowSection(<%=listItem.FAQSectionID%>, this)"><%=listItem.SectionName %> </a><br />
				<%} %> 
			</div>
			<p style="font-size: 12px; margin-top: 20px;">Can&#8217;t find the answer you need here? <a href="mailto:info@theindex.co.nz" style="font-weight: bold;">Ask us</a></p>

		</div>
		<div class="btm"></div>
	</div>
	<%if(Util.ServerIsDev){%>DEV: <a href="CreateSampleQuestions">CreateSampleQuestions</a><%} %>
</asp:Content>