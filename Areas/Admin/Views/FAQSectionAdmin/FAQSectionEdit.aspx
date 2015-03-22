<%@ Page Title="Edit Faqsection" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.FAQSectionAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form");
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.FAQSection; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">FAQ Section</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Section Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.SectionName, true) %></td>
			</tr>
			<tr>
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %> (enter 50 for alphabetical order, or a lower number to list first)</td>
			</tr>
			<tr>
				<td class="label">Is Published:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsPublished, true) %></td>
			</tr>
			<tr>
				<td class="label">FAQ Items</td>
				<td class="field">
					
					<table class="svySubform" id="df_SubformTable_FAQItem">
						<colgroup>
							<col width="25%" />
							<col width="40%" />
							<col width="5%" />
						</colgroup>
						<thead>
							<tr>
								<td class="colhead">Title</td>
								<td class="colhead">Description</td>
								<td class="colhead">Sort position</td>
								<td class="remove">&nbsp;</td>
							</tr>
						</thead>
						<tbody>
							<%new Savvy.SavvyDataForm<Models.FAQItemList,Models.FAQItem>(record.FAQItems, new Savvy.SubformOptions() { 
									DeleteButtonCaption = "x", UseCssButtons = false 
								}).Render(childRecord => { 
										%>
										<td><%= new Forms.TextField(childRecord.Fields.FAQTitle, true) %></td>
										<td><%= new Forms.TextArea(childRecord.Fields.BodyTextHTML, true) %></td>
										<td><%= new Forms.SortPositionField(childRecord.Fields.SortPosition) %></td>
										<% 
									}); 
							%>
						</tbody>
						<tfoot>
							<tr>
								<td colspan="7" class="addingRow">
									<input type=button onclick="df_AddRow('FAQItem');return false;" value="Add FAQ Item">
								</td>
							</tr>
						</tfoot>
					</table>
				</td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>

			<%= SiteMain.ShowModificationLog(record)%>

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
						<%=Html.SavvyHelpText(new Beweb.HelpText("FAQ Section Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

