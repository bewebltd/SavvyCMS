<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.ProductsController.ProductListViewModel>" MasterPageFile="~/site.master" %>
<%@ Import Namespace="Site.Controllers" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent">
	
<script>
/*
	function bookQtyChange(ID, bookPrice) {

		var node = ID;
		var quantity = $("#quantity" + node).val();
		if (isNaN(quantity) || quantity < 1) {
			$("#price" + ID).html('$0.00')
			$("#quantity" + ID).val(0)
			return;
		}
		
		var qty = $("#quantity" + node).val()
		var totalPrice = qty * parseFloat(bookPrice);
		$("#price" + ID).html("$" + totalPrice.toFixed(2));
		$("#pricelab" + ID).show();
	}	
*/
</script>


</asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
		
		<div class="row standardContainer"><!-- Start Row -->
			
			<div class="span3"><!-- Side Menu -->
				<div class = "row">
					<div class= "span3">
						<div class="sideMenu">
							<%Html.RenderAction<NavigationController>(controller => controller.SideNavPages(Model.SectionCode, Model.ContentPage)); %>
						</div>
					</div>	
				</div>
			</div><!-- Side End -->
			
			<div class="span9"><!-- Body Start -->
				<div class = "row">
					<h2 style="margin-left: 20px;"><%=Model.ContentPage.Title %></h2>
				</div>	

				<%=Model.ContentPage.Introduction %>
	
				<%foreach(var cat in Model.Categories) {%>
				<div class="row">
					<hr>
					<%:cat.Title %>
					<%int count = 0;
						foreach(var p in cat.Products) {%>
							<%--<div class="span1 simpleCart_shelfItem" style="margin-top: 20px;">
								<img src="<%=ImageProcessing.ImagePath(p.Picture1)%>" style="padding-left: 10px;"/>
							</div>--%>
							<div class="span11 simpleCart_shelfItem">				
								<table style="margin-left: -10px;">

									<tr>
										<td rowspan="4" style="width: 100px;padding-top: 25px;" valign="top">
											<img class="item_image" src="<%=ImageProcessing.ImagePath(p.Picture1)%>" style="padding-left: 10px;vertical-align: top;padding-bottom: 5px;"/>
										</td>
										<td class="prodTitle" ><a href="detail/<%=p.ID %>"><%=p.Title %></a></td>
										<td class="prodLab1"></td>
										<td></td>
									</tr>	
									<tr>
										<td class="prodSubHeading">Author: <%=p.Author %></td>
										<td style="padding-left: 20px;">	<span class="item_price"><%=Fmt.Currency(p.Price) %></span><p id="price<%=count%>" ></p></td>
										<td></td>
									</tr>
									<tr>
										<td colspan="1" style="padding-left: 10px;width: 440px;" class=""><%= p.Description %></td>
										<td class="tdPrice">
											<div class="">
											
												<a class="item_add" onclick="ShowCart()" href="javascript:;"> <img style="margin-top: -10px; margin-left: 15px;" src="<%=Web.Root%>images/addtocart.png"/> </a>
												<span class="item_name" style="display: none;"><%=p.Title %></span>
												<span class="item_sku" style="display: none;"><%=p.ID %></span>
											</div>
										</td>
										<td></td>
									</tr>						
								<%--	<tr>
										<td colspan="3" class="">Price: <%=Fmt.Currency(p.Price) %> <small>+ postage and handling</small></td>
									</tr>--%>
								</table>
							</div>
							<hr>
							<%
							count++ ;
						} %>
							
				</div>
				<% } %>
					
			</div><!-- Body End -->
				
</div>				

</asp:Content>
