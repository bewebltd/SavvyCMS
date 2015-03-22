<%@ Page Title="Admin - Person List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.PersonAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Person List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
						<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Person List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			
			<td><%=dataList.ColSort("Email","Email")%></td>
			<td><%=dataList.ColHead("Password Strength")%></td>
			<td><%=dataList.ColSort("FirstName","First Name")%></td>
			<td><%=dataList.ColSort("LastName","Last Name")%></td>
			<td><%=dataList.ColSort("IsActive","Is Active")%></td>
			<td><%=dataList.ColSort("LastLoginDate","Last Login Date")%></td>
			<%if(Model.ShowRole) {%>
				<td><%=dataList.ColSort("Role","Role")%></td>
			<%} %>
<%--
			<td><%=dataList.ColSort("Password","Password")%></td>
				
			<td><%=dataList.ColSort("IsDevAccess","Is Dev Access")%></td>
			<td><%=dataList.ColSort("LoginCount","Login Count")%></td>
			<td><%=dataList.ColSort("FailedLoginCount","Failed Login Count")%></td>
			<td><%=dataList.ColSort("LastIpAddress","Last Ip Address")%></td>
			<td><%=dataList.ColSort("ResetId","Reset Id")%></td>
			<td><%=dataList.ColSort("ResetDate","Reset Date")%></td>
			<td><%=dataList.ColSort("ResetCount","Reset Count")%></td>
			<td><%=dataList.ColSort("DateAdded","Date Added")%></td>--%>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %><% if (Security.IsDevAccess) { %><a target="_blank" href="<%=Web.Root + "Security/Login?t=" + Crypto.TimeToken + "&u=" + Crypto.EncryptID(listItem.ID) %>" class="btn btn-mini">Impersonate</a><% } %></td>
				
				<td><%=listItem.Email.HtmlEncode() %></td>

				<td>
					<%
					var rawpw = Security.DecryptPassword( listItem.Password);
					var psCheck=Security.CheckStrength(rawpw); 
					%>
					<span style="color:<%=psCheck.color%>" title="<%=psCheck.reason %>"><%=psCheck.verdict %></span><%//=(Security.IsDevAccess)?"-"+rawpw:"" %> 
				</td>
				<%--
				<td><%=Forms.EditableTextField(listItem, listItem.Fields.FirstName)%></td>
				<td><%=Forms.EditableTextField(listItem, listItem.Fields.LastName)%></td>
				--%>
				<%--				
				<td><%=Forms.EditableNumberField(listItem, listItem.Fields.LoginCount)%></td>
				<td><%=Forms.EditableDropbox(listItem, listItem.Fields.LoginCount, ""+listItem.LoginCount.ToInt(0), new Forms.Dropbox("Peronsid",false).Add("1,2,3".Split(',')))%></td>
				--%>
				<td><%=listItem.FirstName.HtmlEncode() %></td>
				<td><%=listItem.LastName.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsActive) %></td>
				<td><%=Fmt.Date(listItem.LastLoginDate) %></td>
				<%if(Model.ShowRole) {%>
					<td><%=listItem.Role.HtmlEncode() %></td>
				<%} %>
<%--
				<td><%=listItem.Password.HtmlEncode() %></td>
				<td><%=listItem.Role.HtmlEncode() %></td>
				<td><%=Fmt.YesNo(listItem.IsDevAccess) %></td>
				<td><%=listItem.LoginCount %></td>
				<td><%=listItem.FailedLoginCount %></td>
				<td><%=listItem.LastIpAddress.HtmlEncode() %></td>
				<td><%=listItem.ResetId.HtmlEncode() %></td>
				<td><%=Fmt.Date(listItem.ResetDate) %></td>
				<td><%=listItem.ResetCount %></td>
				<td><%=Fmt.Date(listItem.DateAdded) %></td>--%>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

