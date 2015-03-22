<%@ Page Title="Edit Document Category" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.DocumentCategoryAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("form");
			CheckCategory();
		});
		function CheckCategory() {
			var cat = $("#ParentDocumentCategoryID").val();
			if (cat == "") {
				$(".root-category").show();
			} else {
				$(".root-category").hide();
			}
		}
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.DocumentCategory; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Document Category</th>
			</tr>				

			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>

			<tr>
				<td class="label">Folder:</td>
				<td class="field">
					<%= new Forms.Dropbox(record.Fields.ParentDocumentCategoryID, false, false) { onchange = "CheckCategory();" }.Add(null, "Root Folder").AddHierarchy(DocumentCategoryList.Load(new Sql("SELECT DocumentCategoryID, Title, ParentDocumentCategoryID FROM DocumentCategory Order By SortPosition")))%>
				</td>
			</tr>
			<tr class="root-category">
				<td class="label">Page:  <%=Html.SavvyHelp("This applies to built-in pages and must not be changed.")%></td>
				<td class="field"><%=new Forms.Dropbox(record.Fields.PageID, true, true).Add(new Sql("Select PageID, Title from Page where TemplateCode = ", "documentrepository".SqlizeText())) %>
				</td>
			</tr>

			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Intro Text:</td>
				<td class="field"><%=new Forms.TextArea(record.Fields.IntroText, true){maxlength = 500} %></td>
			</tr>
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
			<%Html.RenderAction<CommonAdminController>(controller => controller.ModificationHistoryPanel(record, true)); %>
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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Help Text Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

