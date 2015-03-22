﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBManager.aspx.cs" Inherits="Site.devtools.DBManager" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

	<% var structure = GetDatabaseStructure(); %>

	<h2 class="dbmanager">DB Manager</h2>
	
	<div class="dbManagerWrapper clearfix">

		<div class="tools box">
			<ul class="dbSchemas">
				<% foreach(var schema in structure.Keys) { %>
					<% var hasTables = structure[schema].Tables.Count > 0; %>
					<% var hasViews = structure[schema].Views.Count > 0; %>
					<li>
						<span class="toggleCollapse dbSchema"><%=schema%></span>
						<ul class="dbObjects <%if(schema != "dbo") { %>collapsed<% } %>">
							<li>
								<span class="toggleCollapse dbTables">Tables<%=hasTables ? " ("+structure[schema].Tables.Count+")" : "" %></span>
								<% if(hasTables) { %>
									<ul class="dbSubObjects dbTable <%if(schema != "dbo") { %>collapsed<% } %>">
										<% foreach(var table in structure[schema].Tables) { %>
											<li data-table="<%=(schema+"."+table).Base64Encode()%>"><%=table%></li>
										<% } %>
									</ul>								
								<% } %>
							</li>
							<li>
								<span class="toggleCollapse dbViews">Views<%=hasViews ? " ("+structure[schema].Views.Count+")" : "" %></span>
								<% if(hasViews) { %>
									<ul class="dbSubObjects dbView collapsed">
										<% foreach(var view in structure[schema].Views) { %>
											<li><%=view%></li>
										<% } %>
									</ul>								
								<% } %>
							</li>
						</ul>
					</li>						
				<% } %>
			</ul>
		</div>
		<div class="rightWrapper">
			<div class="tabs">
				<ul>
					<li class="active">SQL Editor</li>
				</ul>
			</div>
			<div class="content box">
				<div id="sqlEditorWrapper" class="tabContent">
					<div class="tabContentBar">
						<button class="btn left" onclick="DBManager.showShortcuts()">Shortcuts</button>
						<button class="btn right" onclick="DBManager.executeEditor()">Execute</button>
					</div>
					<pre id="sqlEditor" ></pre>	
				</div>
				<div id="queryResults" class="tabContent"></div>
			</div>
		</div>
	</div>
	
	<div id="shortcuts" class="shortcutsModal" style="display:none">
		<table>
			<thead>
				<tr>
					<td>Windows/Linux</td>
					<td>Mac OS</td>
					<td>Action</td>
				</tr>
			</thead>
			<tbody>
			<tr>
			<td align="left">Ctrl-,</td>
			<td align="left">Command-,</td>
			<td align="left">Show the settings menu</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Up</td>
			<td align="left">Ctrl-Option-Up</td>
			<td align="left">add multi-cursor above</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Down</td>
			<td align="left">Ctrl-Option-Down</td>
			<td align="left">add multi-cursor below</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Right</td>
			<td align="left">Ctrl-Option-Right</td>
			<td align="left">add next occurrence to multi-selection</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Left</td>
			<td align="left">Ctrl-Option-Left</td>
			<td align="left">add previous occurrence to multi-selection</td>
			</tr>
			<tr>
			<td align="left"></td>
			<td align="left">Ctrl-L</td>
			<td align="left">center selection</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-U</td>
			<td align="left">Ctrl-Shift-U</td>
			<td align="left">change to lower case</td>
			</tr>
			<tr>
			<td align="left">Ctrl-U</td>
			<td align="left">Ctrl-U</td>
			<td align="left">change to upper case</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-Down</td>
			<td align="left">Command-Option-Down</td>
			<td align="left">copy lines down</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-Up</td>
			<td align="left">Command-Option-Up</td>
			<td align="left">copy lines up</td>
			</tr>
			<tr>
			<td align="left">Delete</td>
			<td align="left"></td>
			<td align="left">delete</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-D</td>
			<td align="left">Command-Shift-D</td>
			<td align="left">duplicate selection</td>
			</tr>
			<tr>
			<td align="left">Ctrl-F</td>
			<td align="left">Command-F</td>
			<td align="left">find</td>
			</tr>
			<tr>
			<td align="left">Ctrl-K</td>
			<td align="left">Command-G</td>
			<td align="left">find next</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-K</td>
			<td align="left">Command-Shift-G</td>
			<td align="left">find previous</td>
			</tr>
			<tr>
			<td align="left">Alt-0</td>
			<td align="left">Command-Option-0</td>
			<td align="left">fold all</td>
			</tr>
			<tr>
			<td align="left">Alt-L, Ctrl-F1</td>
			<td align="left">Command-Option-L, Command-F1</td>
			<td align="left">fold selection</td>
			</tr>
			<tr>
			<td align="left">Down</td>
			<td align="left">Down, Ctrl-N</td>
			<td align="left">go line down</td>
			</tr>
			<tr>
			<td align="left">Up</td>
			<td align="left">Up, Ctrl-P</td>
			<td align="left">go line up</td>
			</tr>
			<tr>
			<td align="left">Ctrl-End</td>
			<td align="left">Command-End, Command-Down</td>
			<td align="left">go to end</td>
			</tr>
			<tr>
			<td align="left">Left</td>
			<td align="left">Left, Ctrl-B</td>
			<td align="left">go to left</td>
			</tr>
			<tr>
			<td align="left">Ctrl-L</td>
			<td align="left">Command-L</td>
			<td align="left">go to line</td>
			</tr>
			<tr>
			<td align="left">Alt-Right, End</td>
			<td align="left">Command-Right, End, Ctrl-E</td>
			<td align="left">go to line end</td>
			</tr>
			<tr>
			<td align="left">Alt-Left, Home</td>
			<td align="left">Command-Left, Home, Ctrl-A</td>
			<td align="left">go to line start</td>
			</tr>
			<tr>
			<td align="left">Ctrl-P</td>
			<td align="left"></td>
			<td align="left">go to matching bracket</td>
			</tr>
			<tr>
			<td align="left">PageDown</td>
			<td align="left">Option-PageDown, Ctrl-V</td>
			<td align="left">go to page down</td>
			</tr>
			<tr>
			<td align="left">PageUp</td>
			<td align="left">Option-PageUp</td>
			<td align="left">go to page up</td>
			</tr>
			<tr>
			<td align="left">Right</td>
			<td align="left">Right, Ctrl-F</td>
			<td align="left">go to right</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Home</td>
			<td align="left">Command-Home, Command-Up</td>
			<td align="left">go to start</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Left</td>
			<td align="left">Option-Left</td>
			<td align="left">go to word left</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Right</td>
			<td align="left">Option-Right</td>
			<td align="left">go to word right</td>
			</tr>
			<tr>
			<td align="left">Tab</td>
			<td align="left">Tab</td>
			<td align="left">indent</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-E</td>
			<td align="left"></td>
			<td align="left">macros recording</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-E</td>
			<td align="left">Command-Shift-E</td>
			<td align="left">macros replay</td>
			</tr>
			<tr>
			<td align="left">Alt-Down</td>
			<td align="left">Option-Down</td>
			<td align="left">move lines down</td>
			</tr>
			<tr>
			<td align="left">Alt-Up</td>
			<td align="left">Option-Up</td>
			<td align="left">move lines up</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Shift-Up</td>
			<td align="left">Ctrl-Option-Shift-Up</td>
			<td align="left">move multicursor from current line to the line above</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Shift-Down</td>
			<td align="left">Ctrl-Option-Shift-Down</td>
			<td align="left">move multicursor from current line to the line below</td>
			</tr>
			<tr>
			<td align="left">Shift-Tab</td>
			<td align="left">Shift-Tab</td>
			<td align="left">outdent</td>
			</tr>
			<tr>
			<td align="left">Insert</td>
			<td align="left">Insert</td>
			<td align="left">overwrite</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-Z, Ctrl-Y</td>
			<td align="left">Command-Shift-Z, Command-Y</td>
			<td align="left">redo</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Shift-Right</td>
			<td align="left">Ctrl-Option-Shift-Right</td>
			<td align="left">remove current occurrence from multi-selection and move to next</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Alt-Shift-Left</td>
			<td align="left">Ctrl-Option-Shift-Left</td>
			<td align="left">remove current occurrence from multi-selection and move to previous</td>
			</tr>
			<tr>
			<td align="left">Ctrl-D</td>
			<td align="left">Command-D</td>
			<td align="left">remove line</td>
			</tr>
			<tr>
			<td align="left">Alt-Delete</td>
			<td align="left">Ctrl-K</td>
			<td align="left">remove to line end</td>
			</tr>
			<tr>
			<td align="left">Alt-Backspace</td>
			<td align="left">Command-Backspace</td>
			<td align="left">remove to linestart</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Backspace</td>
			<td align="left">Option-Backspace, Ctrl-Option-Backspace</td>
			<td align="left">remove word left</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Delete</td>
			<td align="left">Option-Delete</td>
			<td align="left">remove word right</td>
			</tr>
			<tr>
			<td align="left">Ctrl-R</td>
			<td align="left">Command-Option-F</td>
			<td align="left">replace</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-R</td>
			<td align="left">Command-Shift-Option-F</td>
			<td align="left">replace all</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Down</td>
			<td align="left">Command-Down</td>
			<td align="left">scroll line down</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Up</td>
			<td align="left"></td>
			<td align="left">scroll line up</td>
			</tr>
			<tr>
			<td align="left"></td>
			<td align="left">Option-PageDown</td>
			<td align="left">scroll page down</td>
			</tr>
			<tr>
			<td align="left"></td>
			<td align="left">Option-PageUp</td>
			<td align="left">scroll page up</td>
			</tr>
			<tr>
			<td align="left">Ctrl-A</td>
			<td align="left">Command-A</td>
			<td align="left">select all</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-L</td>
			<td align="left">Ctrl-Shift-L</td>
			<td align="left">select all from multi-selection</td>
			</tr>
			<tr>
			<td align="left">Shift-Down</td>
			<td align="left">Shift-Down</td>
			<td align="left">select down</td>
			</tr>
			<tr>
			<td align="left">Shift-Left</td>
			<td align="left">Shift-Left</td>
			<td align="left">select left</td>
			</tr>
			<tr>
			<td align="left">Shift-End</td>
			<td align="left">Shift-End</td>
			<td align="left">select line end</td>
			</tr>
			<tr>
			<td align="left">Shift-Home</td>
			<td align="left">Shift-Home</td>
			<td align="left">select line start</td>
			</tr>
			<tr>
			<td align="left">Shift-PageDown</td>
			<td align="left">Shift-PageDown</td>
			<td align="left">select page down</td>
			</tr>
			<tr>
			<td align="left">Shift-PageUp</td>
			<td align="left">Shift-PageUp</td>
			<td align="left">select page up</td>
			</tr>
			<tr>
			<td align="left">Shift-Right</td>
			<td align="left">Shift-Right</td>
			<td align="left">select right</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-End</td>
			<td align="left">Command-Shift-Down</td>
			<td align="left">select to end</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-Right</td>
			<td align="left">Command-Shift-Right</td>
			<td align="left">select to line end</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-Left</td>
			<td align="left">Command-Shift-Left</td>
			<td align="left">select to line start</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-P</td>
			<td align="left"></td>
			<td align="left">select to matching bracket</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-Home</td>
			<td align="left">Command-Shift-Up</td>
			<td align="left">select to start</td>
			</tr>
			<tr>
			<td align="left">Shift-Up</td>
			<td align="left">Shift-Up</td>
			<td align="left">select up</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-Left</td>
			<td align="left">Option-Shift-Left</td>
			<td align="left">select word left</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Shift-Right</td>
			<td align="left">Option-Shift-Right</td>
			<td align="left">select word right</td>
			</tr>
			<tr>
			<td align="left"></td>
			<td align="left">Ctrl-O</td>
			<td align="left">split line</td>
			</tr>
			<tr>
			<td align="left">Ctrl-/</td>
			<td align="left">Command-/</td>
			<td align="left">toggle comment</td>
			</tr>
			<tr>
			<td align="left">Ctrl-T</td>
			<td align="left">Ctrl-T</td>
			<td align="left">transpose letters</td>
			</tr>
			<tr>
			<td align="left">Ctrl-Z</td>
			<td align="left">Command-Z</td>
			<td align="left">undo</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-L, Ctrl-Shift-F1</td>
			<td align="left">Command-Option-Shift-L, Command-Shift-F1</td>
			<td align="left">unfold</td>
			</tr>
			<tr>
			<td align="left">Alt-Shift-0</td>
			<td align="left">Command-Option-Shift-0</td>
			<td align="left">unfold all</td>
			</tr>
			</tbody>
		</table>
	</div>
	
	<script type="text/javascript" src="scripts/class/DBManager.class.js"></script>
	<script src="scripts/ace/ace.js" type="text/javascript" charset="utf-8"></script>
	<script src="scripts/ace/ext-language_tools.js" charset="utf-8"></script>
	
</asp:Content>
