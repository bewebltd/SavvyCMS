<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.CommonController.DocumentCategoryViewModel>" %>

<% if (Model.Categories.Count > 0 || (Model.Documents.Count > 0)){ %>
	<% Util.IncludeJavascriptFile("~/js/BewebCore/RepositoryDownloader.js"); %>
<% } %>
<% if (Model.Categories.Count> 0) {%>
	<ul class="categorylist repository">
		<%foreach (Models.DocumentCategory category in Model.Categories){%>
			<li data-category-id="<%=Crypto.EncryptID(category.ID) %>"><h3><%:category.Title %></h3><p><%:category.IntroText %></p></li>
		<%} %>
	</ul>
<%} %>
<% if (Model.Documents.Count > 0) {%>
	<ul class="categorylist documents">
		<%
		foreach (Models.Document download in Model.Documents){
			if (download.FileAttachment.IsNotBlank()) {
				// work out the file type and append the correct icon
				string fileIconStyle = download.FileAttachment.RightFrom(".");
				// get the uploader name if there is one available
				string uploaderName = "";
				if (download.ModifiedByPersonID.HasValue) {
					uploaderName = download.ModifiedByPerson.FullName;
				} else if (download.AddedByPersonID.HasValue && download.AddedByPerson!= null) {
					uploaderName = download.AddedByPerson.FullName;
				}
				%>
				<li data-document-id="<%=Crypto.EncryptID(download.ID) %>" class="file <%=" " + fileIconStyle%>"><div><h3><%:download.Title%> (<%=fileIconStyle%>)</h3><span class="author"><%=uploaderName%></span><span class="date"><%=(download.DateModified.HasValue)?Fmt.Date(download.DateModified):Fmt.Date(download.DateAdded)%></span><div class="clear"></div><p><%=download.Description%></p></div></li>
				<%
			}
		}
		%>
	</ul>
<%} %>		