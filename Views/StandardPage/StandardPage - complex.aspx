<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.StandardPageController.ViewModel>" MasterPageFile="~/site.master" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">

		<div class="column double-col">
			<div class="normal">
				<div class="heading"><h2><%:Model.SectionTitle%></h2></div>
				<h1><%:Model.ContentPage.Title%></h1>
				<%if (Model.ContentPage.IsStory) {%>
					<p class="date"><%=Model.ContentPage.PublishDate.FmtShortDate()%></p>
				<%} %>
				<%if (Model.ContentPage.Picture!=null){%>
					<div class="credit-wrap">
						<%if (Model.ContentPage.PhotoCredit.IsNotBlank()) {%>				
							<div class="credits">Credit: <%:Model.ContentPage.PhotoCredit%></div>
						<%} %>
						<%=Beweb.Html.PictureThumb(Model.ContentPage.Fields.Picture, Model.ContentPage.Title)%>
						<%if (Model.ContentPage.PhotoCaption.IsNotBlank()) {%>
							<div class="photo-caption"><%:Model.ContentPage.PhotoCaption%></div>
						<%} %>
					</div>
					
				<%} %>
				<%=Model.ContentPage.BodyTextHtml.FmtHtmlText() %>

				<ul class="list-items">
					<%foreach (var story in Model.ContentPage.ChildPages.Active) {%>
						<li class="vote-container">
							<%if (story.Picture!=null){%>
								<%=Beweb.Html.PicturePreview(story.Fields.Picture, story.Title) %>
							<%} %>
							<h4><a href="<%=story.GetUrl()%>"><%=story.Title%></a></h4>
							<p><%:story.GetIntroHtml(300)%></p>
						</li>
					<%} %>
				</ul>

			</div>
		</div>
		<%if (Model.ShowSidebar) {%>
			<div class="column side-col">
				<div class="heading"><h2><%:Model.SidebarTitle%></h2></div>			
				<div class="credit-wrap">
					<%if (Model.ContentPage.SidebarPhotoCredit.IsNotBlank()) {%>				
						<div class="credits">Credit: <%:Model.ContentPage.SidebarPhotoCredit%></div>
					<%} %>
					<%=Model.ContentPage.Fields.SidebarPicture.ToHtml(Model.ContentPage.SidebarTitle)%>
				</div>
				<%if (Model.ContentPage.SidebarPhotoCaption.IsNotBlank()) {%>
					<div class="photo-caption"><%:Model.ContentPage.SidebarPhotoCaption%></div>
				<%} %>
				<%=Model.ContentPage.SidebarTextHtml.FmtHtmlText() %>
			</div>
		<%} %>
</asp:Content>

