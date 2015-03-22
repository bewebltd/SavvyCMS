<%@ Page Title="Admin - Modification Log List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ModificationLogAdminController.ListHelper>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="System.Activities.Expressions" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  <%=Html.InfoMessage()%>
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Modification Log List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<%dataList.Form(() => {%>
					Look in table:<%=new Forms.Dropbox("TableFilter", dataList.SelectedTable, true, false).Add("(all)").Add(dataList.TableFilterSql) %>
					where week begins: <%=new Forms.Dropbox("WeekBeginning", Fmt.Date(dataList.WeekBeginning), false).Add(dataList.DateList) %>
					<%--extra filter--%>
				<% }); %>
				<%=Html.SavvyHelpText(new HelpText("Modification Log List")) %>
			</td>
		</tr>	
		<tr class="colhead">
				<td><%=dataList.ColSort("UpdateDate","Update Date")%></td>
				<td><%=dataList.ColSort("UserName","Person")%></td>
				<td><%=dataList.ColSort("TableName","Table Name")%></td>
				<td><%=dataList.ColSort("RecordID","Record ID")%></td>
				<td><%=dataList.ColSort("ActionType","Action Type")%></td>
				<td><%=dataList.ColSort("ChangeDescription","Change Description")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td style="white-space: nowrap;"><%=Fmt.DateTime(listItem.UpdateDate) %></td>
				<td style="white-space: nowrap;"><%=(listItem.UserName.IsNotBlank())?listItem.UserName:(listItem.Person!=null)?listItem.Person.FullName:"" %></td>
				<td><%:listItem.TableName %></td>
				<td><%=listItem.RecordID %></td>
				<td><%:listItem.ActionType %></td>
				<td><%=Fmt.FmtText(listItem.ChangeDescription) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

