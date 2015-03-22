<%@ Page Title="Edit Product" Inherits="System.Web.Mvc.ViewPage<Models.Product>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<%if (false) { %><script type="text/javascript" src="../../../../js/BewebCore/beweb-cma.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.6.2-vsdoc.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.14/jquery-ui.js"></script><%}   // provides intellisense %>
	<%if (false) { %><script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.8.1/jquery.validate-vsdoc.js"></script><%}   // provides intellisense %>
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form");
		});
		function ShowQR() {
			$.fn.colorbox({ inline: true, href: "#qr", opacity: 0.7 });
			return false;
		}

	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var record = Model; %>
  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
												
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2"><%=Model.Action == "AddNew" ? "Add" : "Edit" %> Product <%if(Model.ProductCategory!=null){ %> <%=Model.Action == "AddNew" ? "to" : "in" %> the <%=Model.ProductCategory.Title %> Category<%} %></th>
			</tr>				
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Product Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture1, false) { Html5 = true, ShowPasteButton=true, AllowPasteAndDrag = true, ShowDragButton=false}%></td>
			</tr>
			<tr>
				<td class="label">Description:</td>
				<td class="field"><%=new Forms.RichTextEditor(record.Fields.Description, true) %></td>
			</tr>
			<tr>
				<td class="label">Author:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Author, false) %></td>
			</tr>
			<tr>
				<td class="label">Price:</td>
				<td class="field">$<%= new Forms.MoneyField(record.Fields.Price, true) %></td>
			</tr>
			<tr>
				<td class="label">Extra Costs:</td>
				<td class="field">$<%= new Forms.MoneyField(record.Fields.Gst, false) %></td>
			</tr>
			<tr>
				<td class="label">Reference:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Reference, false) %></td>
			</tr>
			<tr>
				<td class="label">Category :</td>
				<td class="field">
					<%--Link/Dropbox/Radio list for table Category here?<br>
					<%=new Forms.TextField(record.Fields.ProductCategoryID, false) %>--%>
					<%if(Models.ProductCategoryList.LoadAll().Count>0){%>
						<%=new Forms.Dropbox(record.Fields.ProductCategoryID, true).Add(Models.ProductCategoryList.LoadAll()) %>
					<%}else{%>
						No categories defined. <a href="<%=Web.AdminRoot%>ProductCategoryAdmin/Create" target="_blank">Create one</a>
					<%}%>
				</td>
			</tr>
			<%if(!record.IsNewRecord){%>
				<tr class="">
					<td class="label">Page URL: <%=Html.SavvyHelp("This is a built-in page and the URL cannot be changed.") %></td>
					<td class="field"><%--<%=Web.ProtocolAndHost%>--%><%=Model.GetUrl() %>
						<%if(true){%>
							<div style="float:right;"><a href="" onclick="return ShowQR()"><img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=50x50&amp;chl=<%=Model.GetUrl() %>"></a>
							<%=Html.SavvyHelp(@"QR Code. Click on this code to show a large version of the code. You can screen capture it, and use it to advertise this page.", width:200) %></div>
							<div id="colorbox" style="display:none">
								<div id="qr">
									<img src="http://chart.apis.google.com/chart?cht=qr&amp;chld=H&amp;chs=400x400&amp;chl=<%=Model.GetUrl() %>">
								</div>
							</div>
						<%} %>
					</td>
				</tr>
			<%} %>
		
			<%Html.RenderAction<CommonAdminController>(controller => controller.SEOEditPanel(record,true,"")); %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record,true,"")); %>
	

			<tr>
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %></td>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Product Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

