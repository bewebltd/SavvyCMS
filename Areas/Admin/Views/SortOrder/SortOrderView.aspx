<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.SortOrderController.ViewModel>" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent"></asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
	
  <%=Html.InfoMessage()%>
	

	<%Util.IncludeJavascriptFile("~/js/isotope/jquery.isotope.min.js"); %>	
	
	<style>
		.sortingpage ul, .sortingpage li { margin:0;padding: 0;list-style: none; }
		#items li { cursor: move; } 
		.sorthead { padding: 10px; font-size: 16px; font-weight: bold;}
	</style>
	<script>
		jQuery(document).ready(function () {
			// do stuff when DOM is ready
			Sort()
		});

		function Sort() {
		//	$("#masonry").isotope('destroy');
			$("#items").sortable({ distance: 30, cursor: 'pointer' });   //, update: Save
		}
		
		function Save()	 {
			var order = "";
			$("#items li").each(function () {
				if (order != "") order += ",";
				order += this.id.replace("item", "");
			});
			$("#SortOrder").val(order);
		}

	</script>
	
	<form name="form" id="form" method="post" action="<%=Web.FullRawUrl %>" onsubmit="Save()">
		<input type="hidden" name="SortOrder" id="SortOrder" value=""/>
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Arrange Sort Order <span style="color:#999;font-size: 20px;">&nbsp;</span></th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%//=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Sort Order View")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
	</form>
	
	<div class="sortingpage" style="background: white;">
		<div class="sortingbox">
			<div class="sorthead">Sort Order</div>
			<ul id="items">
				<%foreach (var item in Model.Items) { %>
					<li id="item<%=item.ID%>" style="padding: 10px;float: left;" title="Drag to change sort order">
						<img src="<%=item.ImageThumbPath%>">
					</li>
				<%} %>
			</ul>	
			<div style="clear:both"></div>
		</div>
		
	</div>

</asp:Content>
