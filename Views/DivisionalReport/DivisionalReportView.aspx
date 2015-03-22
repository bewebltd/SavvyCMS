<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<Site.Controllers.DivisionalReportController.ViewModel>" MasterPageFile="~/site.master" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="HeadContent">
	<!-- Bootstrap core CSS -->
	<%Util.IncludeStylesheetFile("~/css/bootstrap3/css/bootstrap.css"); %>	
	<style>
		.norm td { padding-top:15px; }
		.norm td div { background: #eee; font-size: 15px; padding: 2px 4px; }
		.sub td div { font-size: 11px; color: #666; padding: 0 4px; }
		.form-control {width:110px !important;float: left !important;margin-right: 10px;}
	</style>
	<style media="print">.dontprint { display: none; }</style>
	<%Util.IncludejQuery(); %>	
	<%Util.IncludeBewebForms(); %>	
</asp:Content>
<asp:Content runat="server" ID="Body" ContentPlaceHolderID="BodyContent">
	<div class="container">
		
		<div class="dontprint" style="float:right;padding-top:25px;"><input type="button"  class="btn btn-sm btn-success" value="Print" onclick="window.print()"/></div>
		
		<h1>Auckland Divisional Monthly Report</h1>

		<div class="dontprint">

			<form class="form-horizontal">
				<div class="form-group">
					<label for="ContactDate" class="col-sm-3 control-label">Report Month:</label>
					<div class="col-sm-9">
						<%=new Forms.DateField("SelectedMonth",Model.Month,false) { EarliestYear = 2000,DisplayMode = Forms.DateSelectorOptions.MonthYear, cssClass = "form-control"}%>
						<%=new Forms.Dropbox("ShowBreakdown",Model.ShowBreakdown,false){ cssClass = "form-control"}.Add("Summary").Add("Detail")%>
						<input type="submit"  class="btn btn-primary " value="GO →"/>
					</div>
				</div>
			</form>
		</div>

		<%foreach (var h2 in Model.RootLine.SubLines) {%>
			<h2><%:h2.Title%></h2>
			<table class="databox">
				<tr>
					<th></th>
					<%foreach (var per in Model.Periods) { %>
						<th style="width:100px;text-align: right">
							<%=per.Title %>
						</th>
					<% } %>
				</tr>
				<%foreach (var h3 in h2.SubLines) {%>
					<tr>
						<td colspan="5"><h3 style="margin-bottom: 0"><%:h3.Title%></h3></td>
					</tr>
					<%foreach (var line in h3.SubLines){ %>
						<tr class="<%=line.Format%>">
							<td><div><%=line.Title %></div></td>
							<%for (var i=0;i<4;i++) { %>
								<td style="text-align: right">
									<div><%=line.FmtValue(i)%></div>
								</td>
							<% } %>
						</tr>
						<%foreach (var subline in line.SubLines){ %>
							<tr class="<%=subline.Format%>">
								<td><div><%=subline.Title %></div></td>
								<%for (var i=0;i<4;i++) { %>
									<td style="text-align: right">
										<div><%=subline.FmtValue(i)%></div>
									</td>
								<% } %>
							</tr>
						<% } %>
					<% } %>
				<% } %>		
			</table>
		<% } %>
	</div>
	
	<div style="height:50px;"><br/><br/></div>
</asp:Content>
