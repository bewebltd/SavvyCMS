<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.Controllers.SearchController.ViewModel>" %>
<%Util.IncludejQuery(); %>
<%Util.IncludeJQueryUI(); %>
<script type="text/javascript">
	$(document).ready(function () {
		$("#searchText").autocomplete({
			minLength: 2,
			source: '<%=Web.BaseUrl +"Search/SearchAutoComplete" %>',
			focus: function(event, ui) {
				$("#searchText").val(ui.item.searchText);
				return false;
			},
			select: function (event, ui) {
				$("#searchText").val(ui.item.Phrase);
				return false;
			}
		}).data("autocomplete")._renderItem = function(ul, item) {
			return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a>" + item.Phrase + "</a>")
				.appendTo(ul);
		};
	});
</script>
<div id="search">
	<form name="SearchForm" id="SearchForm" method="get" action="<%=Url.Action("Search", "Search") %>" class="AutoValidate">
		<input type="text" name="searchText" id="searchText" class="searchText" value="<%=Model.SearchText %>" data-placeholder="Site Search" />
		<input type="submit" name="submit" title="Search" class="submit" />
	</form>
</div>