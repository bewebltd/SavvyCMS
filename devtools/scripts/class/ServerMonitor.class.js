function ServerMonitor() {};

ServerMonitor.init = function () {

	ServerMonitor.data = {
		cpu: [[]],
		memory: [[]],
		network: [[],[]],
		disk: null
	};

	ServerMonitor.charts = {};
	ServerMonitor.isEnabled = {
		cpu: true,
		memory: true,
		network: true,
		disk: true
	};
	
	/*
	$(window).bind('blur', function () {
		if ($('#monitorSwitch').prop('checked')) {
			$('#monitorSwitch').click();
		}
	});*/

	ServerMonitor.updateAll();
	setInterval(ServerMonitor.updateAll, 1000);
};

ServerMonitor.updateAll = function () {

	if (!$('#monitorSwitch').prop('checked')) return;

	ServerMonitor.updateCpuUsage();
	ServerMonitor.updateMemoryUsage();
	ServerMonitor.updateNetworkUsage();
	ServerMonitor.updateDiskUsage();
	
};

ServerMonitor.updateCpuUsage = function () {
	
	if (!ServerMonitor.isEnabled.cpu) return;

	ServerMonitor.isEnabled.cpu = false;

	$.ajax({
		url: 'Controller.aspx?mode=GetCpuUsage',
		success: function (response) {

			var obj = $.parseJSON(response);

			if (ServerMonitor.data.cpu[0].length > 12) {
				ServerMonitor.data.cpu[0] = ServerMonitor.data.cpu[0].splice(1);
			}

			var lastData = ServerMonitor.data.cpu[0][ServerMonitor.data.cpu[0].length - 1];
			var lastIndex = lastData ? lastData[0] : -1;
			ServerMonitor.data.cpu[0].push([lastIndex + 1, obj.cpu]);

			ServerMonitor.recreateCpuChart();
			
			ServerMonitor.isEnabled.cpu = true;
		},
		error: function (err) {
			if (ServerMonitor.charts.cpu) {
				ServerMonitor.charts.cpu.destroy();
			}
			var chart = $('#cpuUsage');
			chart.html('');
			chart.hide();
			chart.siblings('.chartError').show();
		}
	});
};

ServerMonitor.recreateCpuChart = function () {
	if (ServerMonitor.charts.cpu) {
		ServerMonitor.charts.cpu.destroy();
	}

	ServerMonitor.charts.cpu = $.jqplot('cpuUsage', ServerMonitor.data.cpu, {
		title: 'Application CPU Usage',
		seriesColors: ["#cd0000"],
		axes: {
			yaxis: {
				min: 0,
				max: 100,
				tickOptions: {
					showGridline: false,
					formatString: '%d%'
				}
			}
		}
	});
};

ServerMonitor.updateMemoryUsage = function () {
	
	if (!ServerMonitor.isEnabled.memory) return;
	
	ServerMonitor.isEnabled.memory = false;

	$.ajax({
		url: 'Controller.aspx?mode=GetMemoryUsage',
		success: function (response) {

			var obj = $.parseJSON(response);

			if (ServerMonitor.data.memory[0].length > 12) {
				ServerMonitor.data.memory[0] = ServerMonitor.data.memory[0].splice(1);
			}

			var lastData = ServerMonitor.data.memory[0][ServerMonitor.data.memory[0].length - 1];
			var lastIndex = lastData ? lastData[0] : -1;
			ServerMonitor.data.memory[0].push([lastIndex + 1, obj.memory]);

			ServerMonitor.recreateMemoryChart();
			
			ServerMonitor.isEnabled.memory = true;
		},
		error: function (err) {
			if (ServerMonitor.charts.memory) {
				ServerMonitor.charts.memory.destroy();
			}
			var chart = $('#memoryUsage');
			chart.html('');
			chart.hide();
			chart.siblings('.chartError').show();
		}
	});
};

ServerMonitor.recreateMemoryChart = function () {
	if (ServerMonitor.charts.memory) {
		ServerMonitor.charts.memory.destroy();
	}

	ServerMonitor.charts.memory = $.jqplot('memoryUsage', ServerMonitor.data.memory, {
		title: 'Application Memory Usage',
		seriesColors: ["#ebbb16"],
		axes: {
			yaxis: {
				min: 0,
				tickOptions: {
					showGridline: false,
					formatString: '%dMB'
				}
			}
		}
	});
};

ServerMonitor.updateNetworkUsage = function () {
	
	if (!ServerMonitor.isEnabled.network) return;
	
	ServerMonitor.isEnabled.network = false;
	
	$.ajax({
		url: 'Controller.aspx?mode=GetNetworkUsage',
		success: function (response) {
			
			var obj = $.parseJSON(response);

			if (ServerMonitor.data.network[0].length > 12) {
				ServerMonitor.data.network[0] = ServerMonitor.data.network[0].splice(1);
				ServerMonitor.data.network[1] = ServerMonitor.data.network[1].splice(1);
			}

			var lastData = ServerMonitor.data.network[0][ServerMonitor.data.network[0].length - 1];
			var lastIndex = lastData ? lastData[0] : -1;
			ServerMonitor.data.network[0].push([lastIndex + 1, obj.dl]);
			ServerMonitor.data.network[1].push([lastIndex + 1, obj.ul]);

			ServerMonitor.recreateNetworkChart();
			
			ServerMonitor.isEnabled.network = true;
		},
		error: function (err) {
			if (ServerMonitor.charts.network) {
				ServerMonitor.charts.network.destroy();
			}
			var chart = $('#networkUsage');
			chart.html('');
			chart.hide();
			chart.siblings('.chartError').show();
		}
	});
};

ServerMonitor.recreateNetworkChart = function () {
	if (ServerMonitor.charts.network) {
		ServerMonitor.charts.network.destroy();
	}

	ServerMonitor.charts.network = $.jqplot('networkUsage', ServerMonitor.data.network, {
		title: 'Network Usage',
		seriesColors: ["#1a9b6c", '#1a669b'],
		series:[
        {label:'Download'},
        {label: 'Upload'}
		],
		axes: {
			yaxis: {
				min: 0,
				tickOptions: {
					showGridline: false,
					formatString: '%dKb'
				}
			}
		},
		legend: {
			show: true,
			location: 'nw'
		}
	});
};

ServerMonitor.updateDiskUsage = function () {

	if (!ServerMonitor.isEnabled.disk) return;

	$.ajax({
		url: 'Controller.aspx?mode=GetDiskUsage',
		success: function (response) {
			var obj = $.parseJSON(response);
			
			var used = (obj.used * 100) / obj.total;
			var free = 100 - used;

			ServerMonitor.data.disk = [[['Used Space (' + obj.used + 'GB)', used], ['Free Space (' + obj.free + 'GB)', free]]];

			var title = $('#diskUsageTitle');
			title.siblings('.chart').addClass('smallChart');
			title.text('Disk Usage (Capacity: ' + obj.total + 'GB)');
			
			ServerMonitor.recreateDiskChart();
		},
		error: function (err) {
			ServerMonitor.isEnabled.disk = false;
			if (ServerMonitor.charts.disk) {
				ServerMonitor.charts.disk.destroy();
			}
			var chart = $('#diskUsage');
			chart.html('');
			$('#diskUsageTitle').html('');
			chart.hide();
			chart.siblings('.chartError').show();
		}
	});
};

ServerMonitor.recreateDiskChart = function () {
	if (ServerMonitor.charts.disk) {
		ServerMonitor.charts.disk.destroy();
	}
	
	ServerMonitor.charts.disk = $.jqplot('diskUsage', ServerMonitor.data.disk, {
		seriesColors: ["#0000ff", "#ff00ff"],
		seriesDefaults: {
			renderer: $.jqplot.PieRenderer,
			rendererOptions: {
				startAngle: -180
			}
		},
		legend: { show: true }
	});
};

ServerMonitor.retryCpuUpdate = function () {
	var chart = $('#cpuUsage');
	chart.siblings('.chartError').hide();
	chart.show();
	ServerMonitor.isEnabled.cpu = true;
	ServerMonitor.updateCpuUsage();
};

ServerMonitor.retryMemoryUpdate = function () {
	var chart = $('#memoryUsage');
	chart.siblings('.chartError').hide();
	chart.show();
	ServerMonitor.isEnabled.memory = true;
	ServerMonitor.updateMemoryUsage();
};

ServerMonitor.retryNetworkUpdate = function () {
	var chart = $('#networkUsage');
	chart.siblings('.chartError').hide();
	chart.show();
	ServerMonitor.isEnabled.network = true;
	ServerMonitor.updateNetworkUsage();
};

ServerMonitor.retryDiskUpdate = function () {
	var chart = $('#diskUsage');
	chart.siblings('.chartError').hide();
	chart.show();
	ServerMonitor.isEnabled.disk = true;
	ServerMonitor.updateDiskUsage();
};