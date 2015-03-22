<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.FaqController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Site.Controllers" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent">
	<script>
		$('#searchBox').live('keyup', function () {
			var filter = $(this).val();

			$(".filterRow").each(function () {
				if ($(this).text().search(new RegExp(filter, "i")) < 0) {
					$(this).hide();
				} else {
					$(this).show();
				}
			});

			$('.faq h3').each(function () {
				if($('li:visible', $(this).next()).length < 1) {
					$(this).hide();
					$(this).next().next().hide();
				} else {
					$(this).show();
					$(this).next().next().show();
				}
			});
		});
	</script>
</asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
	
<div id="news">
	<div class="row clear">
		<div class="mainColumn">
			<div class="inner">
				<h1>Frequently Asked Questions</h1>
				<hr />
				<div class="faq">
					<input type="text" placeholder="Search faqs..." id="searchBox" />

					<% foreach(var section in Model.FaqSectionList){ %>
						<h3><%:section.SectionName %></h3>
						<ol>
							<% foreach (var item in section.FAQItems) { %>
							<li class="filterRow">
								<div class="question"><%:item.FAQTitle %></div>
								<div class="answer"><%=item.BodyTextHTML %><a href="#" class="close">Close &gt;</a></div>
							</li>
							<%} %>
						</ol>
						<hr />
					<%} %>

				</div>
			</div>
		</div>
		
		<%--<% Html.RenderAction<CommonController>(n => n.EventsSideBar(true)); %>--%>
		<% Html.RenderAction<CommonController>(n => n.SideBar(Model.ContentPage.PageID));%>
	</div>
</div>

</asp:Content>
