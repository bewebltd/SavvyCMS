<%@ Page	 CodeFile="default.aspx.cs" Language="c#" AutoEventWireup="true" Inherits="Beweb.Admin.Default" Title="Administration Menu" %>
<%@ Import Namespace="Beweb" %>

	<table width="620"	border="0" cellpadding="10" cellspacing="0" bgcolor="#FFFFFF">
		<tr>
			<td>
				<table border="0" cellspacing="0" cellpadding="0" width=550>
					<tr>
						<td class="dataheading" colspan="2">Tools</td>
					</tr>
					<tr>
						<td class="label">
							<a href="email/emailtest.aspx">Email send test</a></td>
						<td class="field">
							Send a test email several different ways.
						</td>
					</tr>				
				
				</table>						
		
				<table class="databox" cellspacing="0" cellpadding="3" width=550>
				<%if(Util.IsDevAccess()){ %>
				<tr>
					<td class="dataheading" colspan="2">Dev access only</td>
				</tr>
				<tr>
					<td class="label">
						<a href="dbedit-sql.aspx">DB Editor</a></td>
					<td class="field">
						Edit DB.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="Structure.aspx">DB Structure</a></td>
					<td class="field">
						View structure.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="clearTables.aspx">Clear Tables</a></td>
					<td class="field">
						Empty tables of all data which are defined in appsettings under the key 'SavvyActiveRecord_ClearData' - Be careful!!!
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="sessionvars.aspx">Session</a></td>
					<td class="field">
						View Session vars.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="cookievars.aspx">Cookies</a></td>
					<td class="field">
						View cookies.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="Servervars.aspx">Server</a></td>
					<td class="field">
						View Server vars.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="../../tests">Tests and Model Gen</a></td>
					<td class="field">
						Generate models and run tests.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="CreateAdmin.aspx">CreateAdmin (Gen)</a></td>
					<td class="field">
						Generate pages.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="<%=Web.Root %>admin/personadmin/ConvertAllPlainTextPasswordsToHashed">Reset plain text passwords</a></td>
					<td class="field">
						Reset plain text passwords to the compiled password scheme.
						[<%=Security.PasswordMode %>]
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="../modules/linkcheck/linkcheck.aspx">Check urls in database</a></td>
					<td class="field">
						Walk all tables in the database looking for links, and testing them.
					</td>
				</tr>
			<%--	<tr>
					<td class="label">
						<a href="../modules/linkcheck/linkcheck.aspx">Check pictures in database</a></td>
					<td class="field">
						Walk all tables in the database looking for pictures, and testing them.
					</td>
				</tr>
				<tr>
					<td class="label">
						<a href="../modules/linkcheck/linkcheck.aspx">Check attachments in database</a></td>
					<td class="field">
						Walk all tables in the database looking for attachments, and check they exist. Also remove attachments that are not referenced by database fields.
					</td>
				</tr>--%>
				<tr>
					<td class="label">
						<a href="PasswordGenerator.aspx">Password Generator</a></td>
					<td class="field">
						Generate passwords.
					</td>
				</tr>
				<%--<tr>
					<td class="label">
						<a href="gen.aspx">Gen</a></td>
					<td class="field">
						Generate pages.
					</td>
				</tr>--%>
				<%}else{ %>
				<tr>
					<td class="dataheading" colspan="2">Sorry, no tools</td>
				</tr>
				<%} %>
				</table>
				<p>&nbsp;</p>
			</td>
		</tr>
	</table>


