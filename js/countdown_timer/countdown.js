/*
Author:			Robert Hashemian (http://www.hashemian.com/)
Modified by:	Munsifali Rashid (http://www.munit.co.uk/)
Modified by:	Matt (http://lindsayandmatt.co.nz/)

Example of how to use:

		var num;
		var cd1 = new countdown('cd1');
		cd1.Div = "countDownWrapper";
		cd1.TargetDate = "<%=Settings.All.ClockDate %>";
		cd1.DisplayFormat = "%%D%%%%H%%%%M%%%%S%%";
		cd1.DuringMessage = "00000000";
		cd1.DurationMinutes = 540;
		cd1.callback = function (obj, str) {
			var nums = str.toString().split("");
			for (var i = 0; i < str.length; i++) {
				var num = str[i];
				if (document.getElementById("digit" + i)) document.getElementById("digit" + i).src = websiteBaseUrl + "/images/cd-" + num + ".png";
				num = nums[i];
				if ($("#digit" + i).length > 0) {
					$("#digit" + i).attr("src", websiteBaseUrl + "images/cd-" + num.toString() + ".png")
				}
			}
		};

		$(document).ready(function () {
			cd1.Setup();	
		});

*/

function countdown(obj) {
	this.obj = obj;
	this.Div = "clock";
	this.TargetDate = "5/31/2012 5:00 PM";
	this.DisplayFormat = "%%D%% Days, %%H%% Hours, %%M%% Minutes, %%S%% Seconds.";
	//this.DisplayFormat = "%%D%%%%H%%%%M%%%%S%%";
	this.DuringMessage = "The event is on!";
	this.DurationMinutes = 1;
	this.EndMessage = "The event is done";
	this.CountActive = true;

	this.DisplayStr = "";

	this.Calcage = cd_Calcage;
	this.CountBack = cd_CountBack;
	this.Setup = cd_Setup;
}

function cd_Calcage(secs, num1, num2) {
	s = ((Math.floor(secs / num1)) % num2).toString();
	if (s.length < 2) s = "0" + s;
	return (s);
}
function cd_CountBack(secs) {

	var ele = document.getElementById(this.Div);

	if (secs < 0 && secs > this.DurationMinutes * -60) {
		if (this.callback) {
			this.callback(this, this.DuringMessage);
		} else {
			ele.innerHTML = this.DuringMessage;
		}
	}
	else if (secs <= this.DurationMinutes * -60) {
		if (this.callback) {
			this.callback(this, this.EndMessage);
		} else {
			ele.innerHTML = this.EndMessage;
		}
	}
	else {
		this.DisplayStr = this.DisplayFormat.replace(/%%D%%/g, this.Calcage(secs, 86400, 100000));
		this.DisplayStr = this.DisplayStr.replace(/%%H%%/g, this.Calcage(secs, 3600, 24));
		this.DisplayStr = this.DisplayStr.replace(/%%M%%/g, this.Calcage(secs, 60, 60));
		this.DisplayStr = this.DisplayStr.replace(/%%S%%/g, this.Calcage(secs, 1, 60));

		if (this.callback) {
			this.callback(this, this.DisplayStr);
		} else {
			ele.innerHTML = this.DisplayStr;
		}
	}
		
	if (this.CountActive) setTimeout(this.obj + ".CountBack(" + (secs - 1) + ")", 990);
}

function cd_Setup() {
	var dthen = new Date(this.TargetDate);
	var dnow = new Date();
	ddiff = new Date(dthen - dnow);
	gsecs = Math.floor(ddiff.valueOf() / 1000);
	this.CountBack(gsecs);
}
