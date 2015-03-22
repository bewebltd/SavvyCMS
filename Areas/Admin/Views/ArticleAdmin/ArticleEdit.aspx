<%@ Page Title="Edit Article" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.ArticleAdminController.EditViewModel>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Areas.Admin.Controllers" %>
<%@ Import Namespace="Site.SiteCustom" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var record = Model.Article; %>

  <%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data" class="AutoValidate" action="<%=Web.FullRawUrl %>">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">Article</th>
			</tr>				

			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
					<!--	<div class="adminnote"><strong>Note</strong>: Only awesomeness in this field please.</div> -->
				</td>
			</tr>
			<tr>
				<td class="label">Page:</td>
				<td class="field">
						Choose which page the article appears on.<br/><%=new Forms.ParentPageDropbox(record.Fields.PageID, Util.GetSetting("SiteNavigationDepth").ToInt(), false, "articlepage")%>
				</td>
			</tr>
			<tr>
				<td class="label">Layout</td>
				<td class="field"><%= new Forms.Dropbox(record.Fields.Template, true, true).Add(Article.TEMPLATEMAINLYTEXT).Add(Article.TEMPLATEMAINLYRESOURCES) %></td>
			</tr>
			<tr>
				<td class="label">Title:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Title, true) %></td>
			</tr>
			<tr>
				<td class="label">Show Title:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowArticleTitle, true) %></td>
			</tr>
					
			<tr>
				<td class="label">Author:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.Author, false) %></td>
			</tr>
			<tr>
				<td class="label">Show Author:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowArticleAuthor, true) %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(the person who originally created this article)</td>
			</tr>	

			<tr>
				<td class="label">Picture:</td>
				<td class="field"><%= new Forms.PictureField(record.Fields.Picture ,false) %></td>
			</tr>
			<tr>
				<td class="label">Picture Caption:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.PhotoCaption, false) %></td>
			</tr>
			<tr>
				<td class="label">YouTube Video ID: <%=Html.SavvyHelp("The YouTube Video ID can be found in the share URL of a video on YouTube. View a video on YouTube and click the share button, the video ID is the text of the share URL after the last forward slash.") %></td>
				<td class="field"><%=new Forms.TextField(record.Fields.YouTubeVideoID, false) %></td>
			</tr>
			<tr>
				<td class="label">Body Text:</td>
				<td class="field"><%= new Forms.RichTextEditor(record.Fields.BodyTextHtml ,true) %></td>
			</tr>
			<tr>
				<td class="label section"><strong>Article Documents</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="field" colspan="2">
					<table class="svySubform" id="df_SubformTable_ArticleDocument">
						<thead>
							<tr>
								<td class="colhead" style="width:156px;">Title</td>
								<td class="colhead" style="width:206px;">Description</td>
								<td class="colhead" style="width:350px;">File</td>
								<td class="colhead" style="width:105px;">Date</td>
								<td class="colhead" style="width:105px;">Expiry</td>
								<td class="colhead" style="width:45px;">&nbsp;</td>
								<td class="remove" style="width:22px;">&nbsp;</td>
							</tr>
						</thead>
						<tbody>
							<%new Savvy.SavvyDataForm<Models.ArticleDocumentList,Models.ArticleDocument>(record.ArticleDocuments, new Savvy.SubformOptions()
								{ 
									DeleteButtonCaption = "x", UseCssButtons = false
								}).Render(childRecord => 
									{ 
										%>
										<td>
											<%= new Forms.TextField(childRecord.Fields.Title, true){style = "width:150px"}%>
										</td>										
										<td>
											<%= new Forms.TextArea(childRecord.Fields.Description, false){style = "width:200px"}%>
										</td>
										<td>
											<%= new Forms.AttachmentField(childRecord.Fields.FileAttachment, true){style="width:350px;"} %>
										</td>
										<td>
											<%= new Forms.DateField(childRecord.Fields.PublishDate, false){style = "width:75px"} %>
										</td>
										<td>
											<%= new Forms.DateField(childRecord.Fields.ExpiryDate, false){style = "width:75px"} %>
										</td>
										<td style="text-align:center;">&nbsp;</td>
										<% 
									}); 
							%>
						</tbody>
						<tfoot>
							<tr>
								<td colspan="7" class="addRow">
									<input type=button onclick="df_AddRow('ArticleDocument');return false;" value="Add Document">
								</td>
							</tr>
						</tfoot>
					</table>
				</td>
			</tr>
			<tr>
				<td class="label section"><strong>Article URLs</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="field" colspan="2">
					<table class="svySubform" id="df_SubformTable_ArticleURL">
						<thead>
							<tr>
								<td class="colhead" style="width:156px">Title</td>
								<td class="colhead" style="width:206px">Description</td>
								<td class="colhead" style="width:350px">URL</td>
								<td class="colhead" style="width:105px;">Date</td>
								<td class="colhead" style="width:105px;">Expiry</td>
								<td class="colhead" style="width:45px;text-align:center;">New<br/>Window</td>
								<td class="remove" style="width:22px;">&nbsp;</td>
							</tr>
						</thead>
						<tbody>
							<%new Savvy.SavvyDataForm<Models.ArticleURLList,Models.ArticleURL>(record.ArticleURLs, new Savvy.SubformOptions() { 
									DeleteButtonCaption = "x", UseCssButtons = false
								}).Render(childRecord => 
									{ 
										%>
										<td>
											<%= new Forms.TextField(childRecord.Fields.Title, true){style = "width:150px;margin-top:2px;"}%>
										</td>
										<td>
											<%= new Forms.TextArea(childRecord.Fields.Description, false){style = "width:200px;margin-top:2px;"}%>
										</td>
										<td>
											<%= new Forms.UrlField(childRecord.Fields.URLLink, true){style = "width:260px;"} %>
										</td>
										<td>
											<%= new Forms.DateField(childRecord.Fields.PublishDate, false){style = "width:75px"} %>
										</td>
										<td>
											<%= new Forms.DateField(childRecord.Fields.ExpiryDate, false){style = "width:75px"} %>
										</td>
										<td style = "text-align:center;">
											<%= new Forms.CheckboxField(childRecord.Fields.IsNewWindow){style="text-align:center;"}%>
										</td>
										<% 
									}); 
							%>
						</tbody>
						<tfoot>
							<tr>
								<td colspan="7" class="addRow">
									<input type=button onclick="df_AddRow('ArticleURL');return false;" value="Add URL">
								</td>
							</tr>
						</tfoot>
					</table>
				</td>
			</tr>
			

			<tr>
				<td class="label section"><strong>Search Optimisation</strong></td>
				<td class="section"></td>
			</tr>
			<tr>
				<td class="label">Search Keywords:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.MetaKeywords, false) %>(comma separated, for search)</td>
			</tr>
			
			<%Html.RenderAction<CommonAdminController>(controller => controller.PublishSettingsEditPanel(record, true,"")); %>
			<tr>
				<td class="label">Show On Latest Articles:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.ShowOnLatestArticles, true) %></td>
			</tr>

			<%--
			<tr>
				<td class="label">Sort Position:</td>
				<td class="field"><%= new Forms.SortPositionField(record.Fields.SortPosition){AutoIncrement = true} %> (enter 50 for alphabetical order, or a lower number to list first)</td>
			</tr>
			--%>

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
						<%=Html.SavvyHelpText(new Beweb.HelpText("Article Edit")) %>
						<%//=Html.PreviewLink(record, "View this page")%> |
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

