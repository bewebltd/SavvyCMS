<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextCompare.aspx.cs" Inherits="Site.devtools.TextCompare" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

	<h2 class="textcompare">Text Compare</h2>
	
	<div id="view"></div>
	
	<link rel="stylesheet" type="text/css" href="styles/codemirror-diff-merge.css" />
	<script type="text/javascript" src="scripts/class/TextCompare.class.js"></script>
	<script src="scripts/codemirror-diff-merge.js" type="text/javascript" charset="utf-8"></script>
	
</asp:Content>
