var _localStorageIsAvailable = null;

function localStorageIsAvailable() {

	if (_localStorageIsAvailable === null) {
		if (!window.localStorage) {
			_localStorageIsAvailable = false;
		} else {
			try {
				window.localStorage.test = true;
			} catch (e) { }

			_localStorageIsAvailable = !!window.localStorage.test;
		}
	}
	
	// If the browser doesn't support local storage, create a temporary global variable
	if (!_localStorageIsAvailable && !window.temp) {
		window.temp = {};
	}

	return _localStorageIsAvailable;
}

/* @class */
function UserData() { }

UserData.Get = function (key) {
	var value = localStorageIsAvailable() ? window.localStorage[key] : window.temp[key];
	//if (!value && window.console) console.log('UserData.Get: ' + key + ' could not be found');
	return value;
};

UserData.Set = function (key, value) {
	if (localStorageIsAvailable()) {
		window.localStorage[key] = value;
	} else {
		window.temp[key] = value;
		UserData.SaveLocalStorageData();
	}
};

UserData.SetMany = function (keyValue) {
	if (localStorageIsAvailable()) {
		for (key in keyValue) {
			window.localStorage[key] = keyValue[key];
		}
	} else {
		for (key in keyValue) {
			window.temp[key] = keyValue[key];
		}
		UserData.SaveLocalStorageData();
	}
};

UserData.SaveLocalStorageData = function() {
	var name = "localStorage";
	var value = btoa(JSON.stringify(window.temp));
	var date = new Date();
	var days = 30; // The cookie will expire after 30 days
	date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
	var expires = "; expires=" + date.toGMTString();
	document.cookie = name + "=" + value + expires + "; path=/";
};

UserData.LoadLocalStorageData = function() {
	var c_name = "localStorage";
	var c_start, c_end = 0;

	if (document.cookie.length > 0) {
		c_start = document.cookie.indexOf(c_name + "=");
		if (c_start != -1) {
			c_start = c_start + c_name.length + 1;
			c_end = document.cookie.indexOf(";", c_start);
			if (c_end == -1) {
				c_end = document.cookie.length;
			}
			var json = unescape(atob(document.cookie.substring(c_start, c_end)));
			window.temp = eval('(' + json + ')');
		}
	}
};

//added JC 20140604
UserData.Delete = function(key) {
	if (localStorageIsAvailable()) {
		window.localStorage.removeItem(key);
	} else {
		delete window.temp[key]
		this.SaveLocalStorageData();
	}
}

if (!localStorageIsAvailable()) UserData.LoadLocalStorageData();
