<%@ Page Language="C#" MasterPageFile="help.master" AutoEventWireup="true" CodeFile="richtexteditor.aspx.cs" Inherits="admin_help_richtexteditor" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
<div id="content">
	<div class="backlink">&middot; <a href="default.aspx" target="_self">contents</a></div>
	<h1>The Rich Text Editor</h1>
	<h2>Overview</h2>
	<p>The Savvy .NET Rich Text Editor sould be familiar to you if you have used Microsoft Word or a similar text editing application with a few exceptions. Click on a letter for an item below to find out more about that item.</p>
	<p>&nbsp;</p>
	<p><img src="images/RichTextEditor.gif" width="628" height="364" border="0" usemap="#editorMap" alt="The Rich Text Editor" title="The Rich Text Editor" /></p>
	<p>&nbsp;</p>
    <map name="editorMap" id="editorMap">
		<area shape="rect" coords="192,5,214,27" href="Javascript:;" onclick="GoAnchor('format');return false;" title="Format Dropdown Menu" alt="Format Dropdown Menu" />
		<area shape="rect" coords="253,5,275,27" href="Javascript:;" onclick="GoAnchor('b');" title="Bold Button" alt="Bold Button" />
		<area shape="rect" coords="276,5,298,27" href="Javascript:;" onclick="GoAnchor('i');" title="Italic Button" alt="Italic Button" />
		<area shape="rect" coords="299,5,321,27" href="Javascript:;" onclick="GoAnchor('u');" title="Underline Button" alt="Underline Button" />
		<area shape="rect" coords="323,5,345,27" href="Javascript:;" onclick="GoAnchor('hr');" title="Horizonal Rule Button" alt="Horizonal Rule Button" />
		<area shape="rect" coords="346,5,368,27" href="Javascript:;" onclick="GoAnchor('ul');" title="Unordered List Button" alt="Unordered List Button" />
		<area shape="rect" coords="369,5,391,27" href="Javascript:;" onclick="GoAnchor('ol');" title="Ordered List Button" alt="Ordered List Button" />
		<area shape="rect" coords="398,5,420,27" href="Javascript:;" onclick="GoAnchor('undo');" title="Undo Button" alt="Undo Button" />
		<area shape="rect" coords="422,5,444,27" href="Javascript:;" onclick="GoAnchor('redo');" title="Redo Button" alt="Redo Button" />
		<area shape="rect" coords="454,5,476,27" href="Javascript:;" onclick="GoAnchor('link');" title="Link Button" alt="Link Button" />
		<area shape="rect" coords="481,5,503,27" href="Javascript:;" onclick="GoAnchor('html');" title="HTML Button" alt="HTML Button" />
		<area shape="rect" coords="606,114,628,136" href="Javascript:;" onclick="GoAnchor('field');" title="Text Field" alt="Text Field" />
		<area shape="rect" coords="564,337,586,359" href="Javascript:;" onclick="GoAnchor('resize');" title="Resize Button" alt="Resize Button" />
		<area shape="rect" coords="116,337,138,359" href="Javascript:;" onclick="GoAnchor('path');" title="Path" alt="Path" />
	</map>
	<a id="format"></a>
	<p><strong>A. Format Dropdown Menu</strong> <img src="images/RichTextEditor-format.gif" alt="Format Dropdown Menu" title="Format Dropdown Menu" width="86" height="22" align="absmiddle" /></p>
	<p>Click on the Format Dropdown Menu and you will see the options available for changing the style of the text . Click on any of the options in the drop-down menu to select it. The selected style will apply to the entire paragraph that your cursor is currently in.</p>
	<p><br /></p>
	<p>By default the style is paragraph.</p>
	<p><div class="backlink">&middot; <a href="Javascript:;" onclick="GoAnchor('top');">top</a></div></p>
	<p><br /></p>
	<a id="b"></a>
	<p><strong>B. Bold Button</strong> <img src="images/RichTextEditor-b.gif" alt="Bold Button" title="Bold Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click on the Bold Button and the paragraph your cursor is in will become bolded. If you have some text selected then only that text will be bolded. If you click the Bold Button before you begin typing the text you then type will be bold.</p>
	<p><br /></p>
	<p>If bold is applied on the text you are currently on,  the Bold Button will be highlighted <img src="images/RichTextEditor-b_over.gif" alt="Bold Button Highlight" title="Bold Button Highlight" width="22" height="22" align="absmiddle" />. To remove the bold, select the text or paragraph you would like to remove it from and click the Bold Button again.</p>
	<p><br /></p>
	<a id="i"></a>
	<p><strong>C. Italic Button</strong> <img src="images/RichTextEditor-i.gif" alt="Italic Button" title="Italic Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click on the Italic Button and the paragraph your cursor is in will become italicised. If you have some text selected then only that text will be italicised. If you click the Italic Button before you begin typing the text you then type will be italicised.</p>
	<p><br /></p>
	<p>If italics is applied on the text you are currently on, the Italic Button will be highlighted <img src="images/RichTextEditor-i_over.gif" alt="Italic Button Highlight" title="Italic Button Highlight" width="22" height="22" align="absmiddle" />. To remove the italics, select the text or paragraph you would like to remove it from and click the Italic Button again.</p>
	<p><br /></p>
	<a id="u"></a>
	<p><strong>D. Underline Button</strong> <img src="images/RichTextEditor-u.gif" alt="Underline Button" title="Underline Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click on the Underline Button and the paragraph your cursor is in will become underlined. If you have some text selected then only that text will be underlined. If you click the Underline Button before you begin typing the text you then type will be underlined.</p>
	<p><br /></p>
	<p>If underline is applied on the text you are currently on, the Underline Button will be highlighted <img src="images/RichTextEditor-u_over.gif" alt="Underline Button Highlight" title="Underline Button Highlight" width="22" height="22" align="absmiddle" />. To remove the underline, select the text or paragraph you would like to remove it from and click the Underline Button again.</p>
	<p><div class="backlink">&middot; <a href="Javascript:;" onclick="GoAnchor('top');">top</a></div></p>
	<p><br /></p>
	<a id="hr"></a>
	<p><strong>E. Horizontal Rule Button</strong> <img src="images/RichTextEditor-hr.gif" alt="Horizontal Rule Button" title="Horizontal Rule Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click on the Horizontal Rule Button and a horizontal rule will be added to the text block. If you have some text selected then that text is deleted and replaced with the rule. The rule inserts a break into you text where you insert it.</p>
	<p><br /></p>
	<p>To remove the horzontal rule, place your cursor at the end of the rule and click the backspace button on your keyboard. Alternatively, you may place your cursor at the begining of the rule and click the delete button on your keyoard.</p>
	<p><br /></p>
	<a id="ul"></a>
	<p><strong>F. Unordered List Button</strong> <img src="images/RichTextEditor-ul.gif" alt="Unordered List Button" title="Unordered List Button" width="22" height="22" align="absmiddle" /></p>
	<p>To make an unordered list, click on the Unordered List Button and type in your item. Click enter at the end of your item to create and another list item. If you are at the end of your list click enter twice to end the list. You can also enter in all the items as seperate paragraphs, select all the paragrahs you would like as a list and click the Unordered List Button.</p>
	<p><br /></p>
	<p>If unordered list is applied to the text your cursor is currenly on, the Unordered List Button will be highlighted <img src="images/RichTextEditor-ul_over.gif" alt="Unordered List Highlight" title="Unordered List Highlight" width="22" height="22" align="absmiddle" />. To remove an item from a list place your cursor on the item in the list and click the Unordered List Button again. You can also select multiple items and click the Unordered List Button to remove them from the list.</p>
	<p><br /></p>
	<a id="ol"></a>
	<p><strong>G. Ordered List Button</strong> <img src="images/RichTextEditor-ol.gif" alt="Ordered List Button" title="Ordered List Button" width="22" height="22" align="absmiddle" /></p>
	<p>To make an unordered list, click on the Ordered List Button and type in your item. Click enter at the end of your item to create and another list item. If you are at the end of your list click enter twice to end the list. You can also enter in all the items as seperate paragraphs, select all the paragrahs you would like as a list and click the Ordered List Button.</p>
	<p><br /></p>
	<p>If ordered list is applied to the text your cursor is currenly on, the Ordered List Button will be highlighted <img src="images/RichTextEditor-ol_over.gif" alt="Ordered List Button Highlight" title="Ordered List Button Highlight" width="22" height="22" align="absmiddle" />. To remove an item from a list place your cursor on the item in the list and click the Ordered List Button again. You can also select multiple items and click the Ordered List Button to remove them from the list.</p>
	<p><br /></p>
	<a id="undo"></a>
	<p><strong>H. Undo Button</strong> <img src="images/RichTextEditor-undo.gif" alt="Undo Button" title="Undo Button" width="22" height="22" align="absmiddle" /></p>
	<p><br /></p>
	<p>Click on the Undo Button to undo the last thing that you did with your text. This is the same as clicking ctrl-z on your keyboard.</p>
	<p><div class="backlink">&middot; <a href="Javascript:;" onclick="GoAnchor('top');">top</a></div></p>
	<p><br /></p>
	<a id="redo"></a>
	<p><strong>I. Redo Button</strong> <img src="images/RichTextEditor-redo.gif" alt="Redo Button" title="Redo Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click on the Redo Button to redo the last thing that you used undo on (see above). When redo is availabel the Redo button will appear highlighted <img src="images/RichTextEditor-redo_over.gif" alt="Redo Button Highlight" title="Redo Button Highlight" width="22" height="22" align="absmiddle" />. This is the same a clicking ctrl-y on your keyboard.</p>
	<p><br /></p>
	<a id="link"></a>
	<p><strong>J. Link Button</strong> <img src="images/RichTextEditor-link.gif" alt="Link Button" title="Link Button" width="22" height="22" align="absmiddle" /></p>
	<p>To create a link, first select the text you would like to creat the link with. The Link Button will become highlighted <img src="images/RichTextEditor-link_over.gif" alt="Link Button Highlight" title="Link Button Highlight"  width="22" height="22" align="absmiddle" /> and clicking on it will open the link dialog box.</p>
	<p><br /></p>
	<p><img src="images/RichTextEditor-link_dialog.gif" width="330" height="240" alt="Link Dialog Box" title="Link Dialog Box" /></p>
	<ol>
		<li>Type in the url to the website or page you would like the link to go to in the Link URL field.</li>
		<li>Select the Target for the link. The target refers to the window in which to open the link typed above, either in the same window or in a new window. (optional)</li>
		<li>Enter in the Title of the link. the title is what will appear as a tooltip when the mouse is over the link. (optional)</li>
		<li>Select a Class for the link. The class is the style that you would like the link to display in. (optional)</li>
		<li>Click the Insert Button to insert the link into the selected text.</li>
	</ol>
	<p><div class="backlink">&middot; <a href="Javascript:;" onclick="GoAnchor('top');">top</a></div></p>
	<p><br /></p>
	<a id="html"></a>
	<p><strong>K. HTML Button</strong> <img src="images/RichTextEditor-html.gif" alt="HTML Button" title="HTML Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click the HTML Button to open the HTML Source Editor window.</p>
	<p><br /></p>
	<p><img src="images/RichTextEditor-htmleditor.gif" width="508" height="290" alt="HTML Source Editor" title="HTML Source Editor" /></p>
	<p><br /></p>
	<p>In the HTML Source Editor you can edit the source html of your text directly. If you are not familar with writing HTML you should avoid using this window when ever possible, in most cases you will not need to use the HTML Source Editor.</p>
	<p><br /></p>
	<p>For more information on HTML visit <a href="http://www.w3schools.com/html/" target="_blank">http://www.w3schools.com/html/</a>.</p>
	<p><div class="backlink">&middot; <a href="Javascript:;" onclick="GoAnchor('top');">top</a></div></p>
	<p><br /></p>
	<a id="field"></a>
	<p><strong>L. Text Field</strong></p>
	<p>The Text Field is where you will do most of your work as it is the content of this box that will appear on your website.</p>
	<p><br /></p>
	<a id="resize"></a>
	<p><strong>M. Resize Button</strong> <img src="images/RichTextEditor-resize.gif" alt="Resize Button" title="Resize Button" width="22" height="22" align="absmiddle" /></p>
	<p>Click and hold  the Resize Button and move your mouse to resize the Rich Text Editor. Release the mouse button to confirm the new size.</p>
	<p><br /></p>
	<a id="path"></a>
	<p><strong>N. Path</strong></p>
	<p>The path is an information area displaying the HTML tags surrounding the text your cursor is currently on out to the nearest paragraph tag.</p>
	<div class="backlink">&middot; <a href="default.aspx" target="_self">contents</a></div>
</div>
</asp:Content>

