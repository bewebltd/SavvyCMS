<%@ Page Inherits="System.Web.Mvc.ViewPage<Site.Controllers.ContactUsController.ViewModel>" Language="C#" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function () {
			//BewebInitForm("form")
			$('li.contactus a').addClass('selected');
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<div class="wide_col_wrapper">	
		<div class="top"></div>
		<div class="col1 wide_col" style="width:650px">
			<h1><%=Model.ContentPage.Title %></h1>
			<%var record = Model.Record; %>
			<div class="clear"></div>
			<%=Html.InfoMessage()%>
			<br/>
			<%=Model.ContentPage.BodyTextHtml.FmtHtmlText() %>			
		</div>
		<div class="btm"></div>	
	</div>
</asp:Content>