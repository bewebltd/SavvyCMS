<%@ Page Title="Admin - Gallery Image List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.GalleryImageAdminController.ListViewModel>" MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>

<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="SavvyMVC.Helpers" %>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
	<script src="<%=Web.Root%>js/masonry/jquery.imagesloaded.min.js"></script>
	<script src="<%=Web.Root%>js/masonry/jquery.masonry.min.js"></script>
	<script src="<%=Web.Root%>js/masonry/jquery.masonry.ordered.min.js"></script>
	<script type="text/javascript">				 
		var masonry=null;
		$(document).ready(function () {
		
			window.setTimeout(function (){
				$('.container').masonry({
					columnWidth: 305,
					itemSelector: '.item',
					gutter: 0
					,transitionDuration:'2s'
				},1500);
			
			});
		

		});
		

	</script>
	<style>
		.item
		{
			border: 1px solid black;
			border-radius: 3px;
			font-family: Arial;
			font-size: 10px;
		}

		.checkitem label
		{
			display: inline;
		}

		.checkitem
		{
			width: 136px;
			overflow: hidden;
		}
	</style>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
	<%var dataList = Model; %>
	<%=Html.InfoMessage()%>
	<%--style override for super wide list page <style>.svyWrapper .databox, .svyWrapper .svyEdit{   width: auto;  }  .svyWrapperOuter  {   width: auto;  }	</style>  --%>
	<%--<h1><%=Model.gal %></h1>--%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Gallery Image List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type="button" value="Create New" onclick="location='<%=Url.Action("Create") + Model.QueryString%>'" class="CreateNewButton" /><%//=Html.SavvyHelp(title:"Gallery Image Help",helpText: @"Help", width:400) %>
				<%dataList.Form(() => {%>
				<%--extra filter examples
					<%=new Forms.Dropbox("StatusFilter",Model.StatusFilter,false).Add("","(Any Status)").Add("Active").Add("Inactive")%>
					<%=new Forms.Dropbox("fieldnameID",false).Add("","(Any othertable)").Add(othertableList.LoadActive())%>
					<%=new Forms.Dropbox("textfieldname",false).Add("","(Any textfieldname)").Add(new Sql("select distinct textfieldname from "))%>
				--%>
				<%});%>
				<%=Html.SavvyHelpText(new HelpText("Gallery Image List")) %>
			</td>
		</tr>
		<tr class="colhead">
			<td colspan="99">
				<div class="container">
					<%foreach (var listItem in dataList.GetResults().Active) {%>

						<div style="" class="item">
							<a href="<%:ImageProcessing.ImagePath(listItem.Picture)%>" class="colorbox">
								<img src="<%:ImageProcessing.ImageMediumPath(listItem.Picture)%>" /></a>
								<%=(listItem.PictureCaption.IsNotBlank())?"<br>"+listItem.PictureCaption:"" %> 
								<br/><a href="<%=Web.Root %>Download/<%= listItem.Picture %>" onclick="alert('not available');return false;">download link</a>
								<br/><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID, bread=(Model.BreadcrumbLevel+1) }) %>
						</div>
					<%} %>
				</div>
			</td>
		</tr>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

