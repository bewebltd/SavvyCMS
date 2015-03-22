<%@ Page Language="C#" MasterPageFile="help.master" AutoEventWireup="true" CodeFile="richtexteditor-i.aspx.cs" Inherits="admin_help_richtexteditor_i" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
    <div id="content">
	    <h2>Italic Button <img src="<%=ResolveUrl("~/admin/help/") %>images/RichTextEditor-i.gif" alt="Italic Button" title="Italic Button" width="22" height="22" align="absmiddle" /></h2>
	    <p>Click on the Italic Button and the paragraph your cursor is in will become italicised. If you have some text selected then only that text will be italicised. If you click the Italic Button before you begin typing the text you then type will be italicised.</p>
	    <p><br /></p>
	    <p>If italics is applied on the text you are currently on, the Italic Button will be highlighted <img src="<%=ResolveUrl("~/admin/help/") %>images/RichTextEditor-i_over.gif" alt="Italic Button Highlight" title="Italic Button Highlight" width="22" height="22" align="absmiddle" />. To remove the italics, select the text or paragraph you would like to remove it from and click the Italic Button again.</p>
    </div>
</asp:Content>

