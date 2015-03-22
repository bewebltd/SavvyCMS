<%@ Page Title="Edit Shopping Cart Order" Inherits="System.Web.Mvc.ViewPage<Models.ShoppingCartOrder>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Shopping Cart Order</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Order Ref:</td>
						
				<td class="field"><%//=new Forms.TextField(record.Fields.OrderRef, true) %>
					<%=record.OrderRef %>
				</td>
			</tr>
			<tr>
				<td class="label">Date Ordered:</td>
				<td class="field"><%//= new Forms.DateField(record.Fields.DateOrdered, true) %>
					<%=(record.DateOrdered.HasValue)?record.DateOrdered.FmtLongDate():"" %>
				</td>
			</tr>
				<tr>
					<td class="label">Person (user logged in when ordering):</td>
					<td class="field"><%//= new Forms.Dropbox(record.Fields.PersonID, true, true).Add(new Sql("SELECT PersonID , Email FROM Person"))%>
						<%if(record.Person!=null){ %>
							<%=record.Person.FullName %> - 
							<a href="<%=Web.AdminRoot %>personadmin/edit/<%=record.PersonID %>">(edit)</a>
						<%} %>
						
					</td>
				</tr>
			<tr>
				<td class="label">Email:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Email, true) %></td>
			</tr>
			<tr>
				<td class="label">First Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.FirstName, true) %></td>
			</tr>
			<tr>
				<td class="label">Last Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.LastName, true) %></td>
			</tr>
			<tr>
				<td class="label">Cart:</td>
				<td class="field"><%
				if(record.PersonID.HasValue){
					string result="<table class=\"result\" style=\"width:100%\">";
					result+="<tr class=\"header\"><td>Part</td><td>QTY</td><td>Status</td><td>deleted?</td></tr>";
					var status = "Ordered";
					//var itemsInCart = Models.ShoppingCartList.Load(new Sql("where personid=",record.PersonID.Value," and Status=",status.Sqlize_Text()," and shop=",status.Sqlize_Text()," and isnull(isdeleted,0)=0 order by dateadded"));
					var itemsInCart = Models.ShoppingCartList.Load(new Sql("where personid=",record.PersonID.Value," and ShoppingCartOrderID=", record.ID," order by IsDeleted, dateadded"));
					if(itemsInCart.Count>0)
					{
						foreach (var cartItem in itemsInCart)
						{
							//var json = ""+cartItem.PartNumber.Value;
							result+="<tr><td>"+cartItem.PartDescription+"</td><td>"+cartItem.Quantity+"</td><td>"+cartItem.Status+"</td><td>"+cartItem.IsDeleted.FmtYesNo()+"</td></tr>";
								
						}
					}else
					{
						result+="<tr><td colspan=\"4\">Nothing in cart</td></tr>";
					}
					result+="</table>";%>
				<%=result %>
				<%} %>
				</td>
			</tr>
			<tr>
				<td class="label">Notes:</td>
				<td class="field"><%= new Forms.TextArea(record.Fields.Notes ,true) %></td>
			</tr>
			<tr>
				<td class="label">Customer Order Reference:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.CustomerOrderReference, true) %></td>
			</tr>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("Shopping Cart Order Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

