<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.SiteMapController.ViewModel>" MasterPageFile="~/site.master" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="BodyContent">


	<div class="wide_col_wrapper">	
		<div class="top"></div>	
		<div class="col1 wide_col sitemap" style="width:650px;">
			<%foreach (var page in Model.PageHierarchy) {%> 
				<%if(page.ShowInXMLSitemap == true){ %>
					<p><%for(int i=0; i < (int)page["Depth"].ValueObject; i++){%> &nbsp;&nbsp;&nbsp; <%} %><a href="<%=page.GetFullUrl() %>"><%:page.GetNavTitle() %></a></p>
				<%} %>
		<%} %>

		</div>
		<div class="btm"></div>
	</div>

</asp:Content>