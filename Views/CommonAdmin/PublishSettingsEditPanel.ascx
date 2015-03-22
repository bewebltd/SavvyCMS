<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Areas.Admin.Controllers.CommonAdminController.PublishSettingsViewData>" %>
<%@ Import Namespace="Beweb" %>
<%var record = Model.DataRecord; %>
		<%if(record.Advanced.PublishDateField!=null || record.Advanced.ExpiryDate != null || record.Advanced.IsActiveFields.Any()){ %>
			<%if(Model.ShowHeader) {%>
				<tr class="<%=Model.CssTablerowClass %>">
					<td class="label section"><strong>Publish Settings</strong></td>
					<td class="section"></td>
				</tr>
			<%}%>
			<%if (record.Advanced.SortPositionField !=null && false){ %>
				<tr>
					<td class="label">Sort Position:</td>
					<td class="field"><%= new Forms.SortPositionField(record.Advanced.SortPositionField){ AutoIncrement = true} %> (enter 50 for alphabetical order, or a lower number to list first)</td>
				</tr>
			<%} %>
			<%if(record.Advanced.PublishDateField!=null){ %>
				<tr class="<%=Model.CssTablerowClass %>">
					<td class="label">Publish Date:</td>
					<td class="field"><%=new Forms.DateField(record.Advanced.PublishDateField, false) %>  (blank = don't publish)</td>
				</tr>
			<%} %>
			<%if(record.Advanced.ExpiryDateField!=null){ %>
				<tr>
					<td class="label">Expiry Date:</td>
					<td class="field"><%= new Forms.DateField(record.Advanced.ExpiryDateField, false) %>  (blank = don't expire)</td>
				</tr>
			<%} %>
			<%foreach (var field in record.Advanced.IsActiveFields){ %>
				<tr>
					<td class="label"><%=field.FriendlyName%>:</td>
					<td class="field"><%= new Forms.YesNoField(field, false) %> (must be 'Yes' to publish)</td>
				</tr>
			<%} %>
		<%} %>



