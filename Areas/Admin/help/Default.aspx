<%@ Page Language="C#" MasterPageFile="help.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_help_Default" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
    <style type="text/css">
	    #wrapper{
		    min-width:430px;
	    }
	</style>
	<div id="head">
		<!--<div style="width:120px;height:30px;margin:10px;padding:20px 10px 10px;border:1px solid #FFF;color:#FFF;text-align:center;float:left;clear:left;">Your company logo here</div>-->
		<img src="images/beweb-savvy-logo.gif" alt="beweb savvy" title="beweb savvy" width="245" height="80" border="0" usemap="#HeadMap" />
		<map id="HeadMap">
			<area shape="rect" coords="0,0,95,80" href="http://www.beweb.co.nz" alt="Visit the beweb website." title="Visit the beweb website." target="_blank" />
		</map>
	</div>
	<div id="content">
		<h1>Table of Contents</h1>
		<h2>Welcome to the Help for the Savvy Content Management System(CMS).</h2>
		<p>Below is a list of topics and that you make need to review when using the Savvy CMS.</p>
		<p>Click on a topic to view the page:</p> 
		<ul>
			<li><a href="richtexteditor.aspx">The Rich Text Editor</a> (Overview)
				<ul>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','format');return false;" target="_self" title="Format Dropdown Menu" alt="Format Dropdown Menu">Format Dropdown Menu</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','b');return false;" target="_self" title="Bold Button" alt="Bold Button">Bold Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','i');return false;" target="_self" title="Italic Button" alt="Italic Button">Italic Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','u');return false;" target="_self" title="Underline Button" alt="Underline Button">Underline Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','hr');return false;" target="_self" title="Horizonal Rule Button" alt="Horizonal Rule Button">Horizonal Rule Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','ul');return false;" target="_self" title="Unordered List Button" alt="Unordered List Button">Unordered List Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','ol');return false;" target="_self" title="Ordered List Button" alt="Ordered List Button">Ordered List Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','undo');return false;" target="_self" title="Undo Button" alt="Undo Button">Undo Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','redo');return false;" target="_self" title="Redo Button" alt="Redo Button">Redo Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','link');return false;" target="_self" title="Link Button" alt="Link Button">Link Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','html');return false;" target="_self" title="HTML Button" alt="HTML Button">HTML Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','field');return false;" target="_self" title="Text Field" alt="Text Field">Text Field</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','resize');return false;" target="_self" title="Resize Button" alt="Resize Button">Resize Button</a></li>
					<li><a href="Javascript:;" onclick="GoLink('richtexteditor.aspx','path');return false;" target="_self" title="Path" alt="Path">Path</a></li>
				</ul>
			</li>
		</ul>
	</div>
</asp:Content>

