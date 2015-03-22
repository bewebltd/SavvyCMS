<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Models.TextBlock>" %>

<% 
	var t = Model; 

	if(t.IsTitleAvailable) {
		Response.Write("<h2>"+t.Title+"</h2>");
	}

	if(t.IsPictureAvailable) {
		Response.Write(Beweb.Html.Picture(t.Fields.Picture, ""));
	}

	Response.Write(Fmt.HTMLText(t.BodyTextHtml));
%>

