<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Areas.Admin.Controllers.CommonAdminController.ModificationHistoryViewData>" %>
<%@ Import Namespace="Site.SiteCustom" %>
			<%if(Model.ShowHeader) {%>
				<tr>
					<td class="label section"><strong>Modification History</strong></td>
					<td class="section"></td>
				</tr>
			<%}%>
			<%if(Model.DataRecord.Advanced.DateAddedField!=null){ %>
				<tr>
					<td class="label">Date Added:</td>
					<td class="field"><%=Fmt.DateTime(Model.DataRecord.Advanced.DateAdded)%></td>
				</tr>
			<%} %>
			<%if(Model.DataRecord.Advanced.DateModifiedField!=null){ %>
				<tr>
					<td class="label">Last Modified:</td>
					<td class="field">
						<%=Fmt.DateTime(Model.DataRecord.Advanced.DateModified)%>
						<%if(Model.DataRecord.FieldExists("ModifiedBy") && Model.DataRecord["ModifiedBy"].IsNotBlank){ %>
							by <%=Model.DataRecord["ModifiedBy"]+""%>
						<%} %>
					</td>
				</tr>
			<%} %>

			<%= SiteMain.ShowModificationLog(Model.DataRecord)%>