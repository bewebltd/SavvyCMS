<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.AboutUsController.ViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Site.Controllers" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">	
	<script src="<%=Web.Root %>js/BewebCore/Savvy.validate.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			//BewebInitForm("form")
			$('li.info a').addClass('selected');
		});
		function CheckForm(form) {
			var msg = '';
			var result = CheckBasicFormValidation(form);
			if (result) result = SavvyBeforeFormSubmit(form);
			return result;
		}
	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
	<% var row = 0; %>	
<%--	<ul id="miniNav">
		<li><a href="<%=Web.Root %>" class="cursor">Home</a></li>
		<li><a href="<%=Web.Root %>AboutUs" class="cursor">About us</a></li>
	</ul>	--%>
	<div class="wide_col_wrapper">	
		<div class="top"></div>
		<div class="col1 wide_col" style="width:650px">
			<h1>
				<%=Model.ContentPage.Title.HtmlEncode() %>
				<%//Html.RenderAction<AboutUsController>(controller => controller.SubNav(Model.ContentPage.ID)); %>

				<%--<span>\ <a href="<%=Web.Root %>Info/HowItWorks">HOW IT WORKS</a></span>
				<span>\ <a href="<%=Web.Root %>Info/Overview">Overview</a></span>
				<span>\ <a href="<%=Web.Root %>NewsList">News</a></span>--%>
			</h1>
			<%=Model.ContentPage.BodyTextHtml.FmtHtmlText()	 %>			
			<ul class="aboutus">
				<% var i = 0; %>
				<% const string rowName = "row"; %>

				<%
				if(Model.PersonList.Count>0) {
					foreach (var person in Model.PersonList) {%>	
					<li <%="class=\"" + Beweb.Html.OddEven + " " + rowName + row + "\""%>>
						<a class="emailme" href="mailto:<%=person.EmailAddress%>?subject=Please contact me&body=Please contact me %0AThank you." title="Send EMail"></a> 
						<h3><%=person.PersonName%> - region <%=person.ClientContactUsRegion.RegionName %></h3>
						<p class="jobtitle"><%=person.JobDescription.FmtPlainTextAsHtml()%></p>
						<p class="intro"><%=person.Introduction.DefaultValue("not available").FmtPlainTextAsHtml()%></p>
					</li>	
					<%i++;%>
					<%
					if (i%2 == 0) row++;%>
					<%
					}
				}else {
					%><li>No people loaded 
					<%if(Util.ServerIsDev){%><br />DEV: <a href="aboutus/LoadSample">Load sample people</a><%} %>
					</li><%
				}%>
			</ul>

			
			<script type="text/javascript">
				$(document).ready(function () {
					var row = <%=row%> ;
					var rowName = "<%=rowName%>";
					for(var j = 0;j<=row;j++) {
						EvenUpHeights("."+rowName + j);
					}
				});
			</script>
		</div>
		<div class="btm"></div>	
	</div>
</asp:Content>