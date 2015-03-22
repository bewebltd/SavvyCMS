<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.CommonController.ResourceViewModel>" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="Site.Controllers" %>

<% var isMainlyText =  Model.Template == Article.TEMPLATEMAINLYTEXT; %>							

<% if(isMainlyText && (Model.Documents.Count > 0 || Model.Urls.Count > 0)) {%>
	<ul class="resources-compact">
		<li class="resources-title"><b>Related</b> Items</li>
		<% var rowNumber = 0; %>
		<% foreach (var resource in Model.Documents.OrderByDescending(d =>d.PublishDate)) { %>
			<% string documentURL = Web.BaseUrl + "DownLoadArticleDocument/" + Crypto.EncryptID(resource.ArticleDocumentID); %>
			<% string fileType =  resource.FileAttachment.RightFrom(".");%>
			<% rowNumber++; %>
			<li class="clearfix <%=rowNumber == 1 ? "nth-child2" : "" %>">
				<div class="resourceContent">
					<a class="resourceIcon" href="<%=documentURL %>" data-filetype="<%=fileType %>" title="View">View</a>
					<h3><a href="<%=documentURL %>"><%=resource.Title %></a></h3>
					<p><a href="<%=documentURL %>"><%=resource.Description %></a></p>
				</div>
			</li>
		<% } %>
		<% foreach (var resource in Model.Urls) { %>
			<% rowNumber++; %>
			<li class="clearfix <%=rowNumber == 1 ? "nth-child2" : "" %>">
				<div class="resourceContent">
					<a class="resourceIcon resourceIconURL" <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>" title="View" target="_blank">View</a>
					<h3><a <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>"><%=resource.Title %></a></h3>
					<p><a <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>"><%=resource.Description %></a></p>
				</div>
			</li>
		<% } %>
	</ul>
<% } %>
<% if(!isMainlyText && (Model.Documents.Count > 0 || Model.Urls.Count > 0)) { %>
	<% var columnNumber = 0; %>
	<% var rowNumber = 1; %>
	<table class="resources-full">
		<% foreach (var resource in Model.Documents.OrderByDescending(d => d.PublishDate)) { %>
			<% string documentURL = Web.BaseUrl + "DownLoadArticleDocument/" + Crypto.EncryptID(resource.ArticleDocumentID); %>
			<% string fileType =  resource.FileAttachment.RightFrom(".");%>
			<% columnNumber++; %>
			<% if(columnNumber == 1) { %> <tr <%=rowNumber == 1 ? "class='first'" : "" %>> <% } %>
			<td <%=columnNumber == 3 ? "class='last'" : "" %>>
				<div class="resourceContent">
					<a class="resourceIcon" href="<%=documentURL %>" data-filetype="<%=fileType %>" title="View" target="_blank">View</a>
					<h3><a href="<%=documentURL %>"><%=resource.Title %></a></h3>
					<p><a href="<%=documentURL %>"><%=resource.Description %></a></p>
				</div>
			</td>
			<% if(columnNumber == 3) { columnNumber = 0; rowNumber++; %> </tr> <% } %>
		<% } %>
		<% foreach (var resource in Model.Urls) { %>
			<% columnNumber++; %>
			<% if(columnNumber == 1) { %> <tr <%=rowNumber == 1 ? "class='first'" : "" %>> <% } %>
			<td <%=columnNumber == 3 ? "class='last'" : "" %>>
				<div class="resourceContent">
					<a class="resourceIcon resourceIconURL" <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>" title="View">View</a>
					<h3><a <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>"><%=resource.Title %></a></h3>
					<p><a <%=resource.IsNewWindow ? "target='_blank'" : "" %> href="<%=resource.URLLink %>"><%=resource.Description %></a></p>
				</div>
			</td>
			<% if(columnNumber == 3) { columnNumber = 0; rowNumber++; %> </tr> <% } %>
		<% } %>
		<% if(columnNumber == 1) { %> <td><div class="resourceContent">&nbsp;</div></td><td class="last"><div class="resourceContent">&nbsp;</div></td></tr> <% } %>
		<% if(columnNumber == 2) { %> <td class="last"><div class="resourceContent">&nbsp;</div></td></tr> <% } %>
	</table>
<% } %>