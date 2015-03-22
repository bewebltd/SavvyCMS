<%@ Page Title="Create Admin Pages" Language="C#"  AutoEventWireup="true" CodeFile="CreateAdmin.aspx.cs" Inherits="Savvy.admin_tools_CreateAdmin" %>

<form runat="server">
	<asp:PlaceHolder ID="choosetable" runat="server">
		Choose table from Active Record models: 
		<%=new Forms.Dropbox("ModelCreateTable", Web.Request["ModelCreateTable"], true){cssClass="svySelect2"}.Add(ActiveRecordDatabaseGenerator.GetModelTableNames()) %><br/>
		OR
		Choose table from db: 
		<asp:DropDownList ID="CreateTable" FirstItemText="-- please select --" FirstItemValue="" DataSourceID="ChooseTableDS" DataTextField="table_name" runat="server" /><br />
		<br/>
		<asp:CheckBox ID="includeSubform" runat="server" Text="Include a subform" /> 
		or <asp:CheckBox ID="includeChildLink" runat="server" Text="Include a link to child list page" /> 
        <br />- choose child table for subform or link from models
				<%=new Forms.Dropbox("ModelSubformTable", Web.Request["ModelSubformTable"], false){cssClass="svySelect2"}.Add("","--- please select ---").Add(ActiveRecordDatabaseGenerator.GetModelTableNames()) %>
				or from db
				<asp:DropDownList ID="SubformTable" FirstItemText="-- none --" FirstItemValue="" DataSourceID="ChooseTableDS" DataTextField="table_name" runat="server" /><br />

	</asp:PlaceHolder>
	<br />
	
	Choose template type:
	<asp:DropDownList ID="TemplateToUse" runat="server">
		<asp:ListItem Text="SavvyMVC" Value="pages_mvc"></asp:ListItem>
		<asp:ListItem Text="Models" Value="pages_models"></asp:ListItem>
		<asp:ListItem Text="Web Forms" Value="pages_webforms"></asp:ListItem>
	</asp:DropDownList><br />
	
	Generate to folder: 
	<asp:TextBox ID="targetfolder" runat="server"></asp:TextBox> (should end in a '/')<br />
	<asp:SqlDataSource ID="ChooseTableDS" OnLoad="SetupChooseTableDS" SelectCommand="SELECT DISTINCT table_name FROM INFORMATION_SCHEMA.Columns WHERE table_name NOT LIKE 'sys%' ORDER BY table_name" runat="server" />
	
	<asp:Button Text="Create Admin Pages" OnClick="Create_OnClick" runat="server" />
	<br /><br />
	<asp:CheckBox ID="allpages" runat="server" Text="Generate All Tables."  OnCheckedChanged="CheckedGenAll" AutoPostBack="true" /><br/>
	<asp:CheckBox ID="overwrite" runat="server" Text="Overwrite files if they exist." /><br/>
	<asp:CheckBox ID="listpages" runat="server" Text="Generate List Page (with export)." Checked="true" /><br/>
	<asp:CheckBox ID="editpages" runat="server" Text="Generate Edit Page." Checked="true" /><br/>
	<asp:CheckBox ID="viewpages" runat="server" Text="Generate View Page." Checked="false" /><br/>
	<asp:CheckBox ID="exportpages" runat="server" Text="Generate Export Detail Page." Checked="false" /><br/>
	<asp:Literal ID="ResultArea" runat="server" /><br/>
	<a href="createadmin.aspx">reset / refresh</a>
</form>
