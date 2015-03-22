<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Site.devtools.Controller" MasterPageFile="DevTools.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">

	<h2 class="monitor">Realtime Monitor</h2>
	
	<div class="onoffswitch">
			<input type="checkbox" name="onoffswitch" class="onoffswitch-checkbox" id="monitorSwitch" checked>
			<label class="onoffswitch-label" for="monitorSwitch">
					<div class="onoffswitch-inner">
							<div class="onoffswitch-active"><div class="onoffswitch-switch">ON</div></div>
							<div class="onoffswitch-inactive"><div class="onoffswitch-switch">OFF</div></div>
					</div>
			</label>
	</div>

	<%--

	<p>
		Todo:
		<br />
		- attachments<br />
		- db usage, size, per table, rows<br />
		- tests
	</p>--%>

	<div class="chartWrapper box">
		<p class="chartError">An error has occurred. <span onclick="ServerMonitor.retryCpuUpdate()">Retry</span></p>
		<div id="cpuUsage" class="chart"></div>
	</div>
	<div class="chartWrapper box">
		<p class="chartError">An error has occurred. <span onclick="ServerMonitor.retryMemoryUpdate()">Retry</span></p>
		<div id="memoryUsage" class="chart"></div>
	</div>
	<div class="chartWrapper box clear">
		<p class="chartError">An error has occurred. <span onclick="ServerMonitor.retryNetworkUpdate()">Retry</span></p>
		<div id="networkUsage" class="chart"></div>
	</div>
	<div class="chartWrapper box">
		<p class="chartError">An error has occurred. <span onclick="ServerMonitor.retryDiskUpdate()">Retry</span></p>
		<div class="jqplot-target" id="diskUsageTitle"></div>
		<div id="diskUsage" class="chart"></div>
	</div>
	
	<link rel="stylesheet" type="text/css" href="styles/jquery.jqplot.min.css" />
	<script type="text/javascript" src="scripts/jquery.jqplot.min.js"></script>
	<script type="text/javascript" src="scripts/jqplot.pieRenderer.min.js"></script>
	<script type="text/javascript" src="scripts/class/ServerMonitor.class.js"></script>

</asp:Content>