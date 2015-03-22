<%@ Page Title="Edit Text Block" Inherits="System.Web.Mvc.ViewPage<Models.TextBlock>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function () {
			BewebInitForm("form");
			window.setTimeout(function () {
				//$('td a.mce_image').parent().hide();
				//$('table .mce_formatselect').parent().hide();
				//$('td a.mce_attachment').parent().hide();
				//$('td a.mce_table').parent().hide();
				//$('td a.mce_code').parent().hide();
			}, 500);
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
			<th colspan="2">Text Block</th>
		</tr>
			<tr>
			<td colspan="2" class="header">
				<!--this replaced by .footer inner html-->
			</td>
		</tr>
		<%
		string style="";
		var sql = new Sql("SELECT TextBlockGroupID , GroupName FROM TextBlockGroup order by SortPosition,GroupName");	
		if(sql.FetchInt().HasValue)
		{
			%>
			<tr style="<%=style%>">
				<td class="label">Text Block Group:</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.TextBlockGroupID, false, true).Add(sql)%></td>
			</tr>
			<%
		} %>
		<%if(Util.IsDevAccess()||record.SectionCode.IsBlank()){ %>
			<tr style="<%=style%>">
				<td class="label">DEV: Section Code:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.SectionCode, true) %></td>
			</tr>
			<%
		}else
		{
			%>
			<tr style="<%=style%>">
				<td class="label">Section Code:</td>
				<td class="field"><%=record.SectionCode %></td>
			</tr>
			<%
		} %>
		<%style=Util.IsDevAccess()?"":"display:none"; %>
		<tr style="<%=style%>">
			<td class="label">DEV:Is Title Available:</td>
			<td class="field"><%=new Forms.YesNoField(record.Fields.IsTitleAvailable, false){onclick="if($(this).val()!='True'){$('.titlerow')[0].style.display='none';}else{$('.titlerow')[0].style.display='table-row';}"} %></td>
		</tr>
		<%style=record.IsTitleAvailable?"":"display:none"; %>
		<tr style="<%=style%>" class="titlerow">
			<td class="label">Title:</td>
			<td class="field"><%=new Forms.TextField(record.Fields.Title, false) %></td>
		</tr>
		<%style=Util.IsDevAccess()?"":"display:none"; %>
		<tr style="<%=style%>">
			<td class="label">DEV:Is Body Plain Text:</td>
			<td class="field"><%=new Forms.YesNoField(record.Fields.IsBodyPlainText, false) {onclick="if($(this).val()=='True'){$('#BodyTextHtml').show(500);$('#BodyTextHtml_parent').hide(500);}else{$('#BodyTextHtml').hide(500);$('#BodyTextHtml_parent').show(500);$('input[name=SaveAndRefreshButton]').click()}"} %>
				<%--<br>Note: page needs a save and refresh before entering plain text--%>
			</td>
		</tr>
		<tr class="textrow">
			<td class="label">Body Text:</td>
			<td class="field">
				<%if(record.IsBodyPlainText) {%><%=new Forms.TextArea(record.Fields.BodyTextHtml, false) {style="width:505px;height:420px"} %><%}else{%><%=new Forms.RichTextEditor(Page,record.Fields.BodyTextHtml, false) { style="width:505px;height:420px"}%><%} %>
			</td>
		</tr>
		<tr>
			<td class="label">Sort Position:</td>
			<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition) %></td>
		</tr>
		<%style=Util.IsDevAccess()?"":"display:none"; %>
		<tr style="<%=style%>">
			<td class="label">DEV:Is Picture Available:</td>
			<td class="field"><%=new Forms.YesNoField(record.Fields.IsPictureAvailable, true) {onclick="if($(this).val()!='True'){$('.picturerow').hide();}else{$('.picturerow').show();}"} %></td>
		</tr>
		<%style=record.IsPictureAvailable?"":"display:none"; %>
		<tr style="<%=style%>" class="picturerow">
			<td class="label">Picture:</td>
			<td class="field"><%= new Forms.PictureField(record.Fields.Picture, false) %></td>
		</tr>
		<tr style="<%=style%>" class="picturerow">
			<td class="label">Picture Caption:</td>
			<td class="field"><%=new Forms.TextField(record.Fields.PictureCaption, false) %></td>
		</tr>
		<%style=Util.IsDevAccess()?"":"display:none"; %>
		<tr	style="<%=style%>">
			<td class="label">DEV:Is Url Available:</td>
			<td class="field"><%=new Forms.YesNoField(record.Fields.IsUrlAvailable, false) {onclick="if($(this).val()!='True'){$('.urlrow').hide();}else{$('.urlrow').show();}"}%></td>
		</tr>
		<%style=record.IsUrlAvailable?"":"display:none"; %>
		<tr style="<%=style%>" class="urlrow">
			<td class="label">Url:</td>
			<td class="field"><%= new Forms.UrlField(record.Fields.Url, false) %></td>
		</tr>
		<tr style="<%=style%>" class="urlrow">
			<td class="label">Url Caption:</td>
			<td class="field"><%= new Forms.UrlField(record.Fields.UrlCaption, false) %></td>
		</tr>
		
		<%if(Util.IsDevAccess()){ %>
			<tr>
				<td class="label">DEV: Has Mail Merge fields:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.HasMailMergefields, false) %></td>
			</tr>
		<%} %>
		<%if(record.HasMailMergefields){ %>
			<tr>
				<td class="label">Merge Field Help:</td>
				<td class="field">
					<%if(Util.IsDevAccess()){ %>
						DEV:<br/> <%= new Forms.TextArea(record.Fields.MergefieldHelp ,false) %>
					<%}else{%>
				 		<%= new Forms.DisplayField(record.Fields.MergefieldHelp ,false) %>
				 <%} %>
				</td>
			</tr>		
		<%} %>		

		<tr>
			<td class="label">Admin Notes:</td>
			<td class="field"><%= new Forms.TextArea(record.Fields.AdminNotes ,false) %></td>
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
					<%=Html.SavvyHelpText(new Beweb.HelpText("Text Block Edit")) %>
					<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
				</div>
			</td>
		</tr>
	</table>
	<%=Html.AntiForgeryToken() %>
	<%=Html.ReturnPageToken() %>
</form>
</asp:Content>

