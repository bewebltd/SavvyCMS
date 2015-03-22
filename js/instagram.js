// @class
function InstagramAPI() {

	// @private
	var results = null;
	var usersQueue = null;
	var tagsQueue = null;
	var sort = null;
	var callback = null;
	var limit = null;
	var cache = null;
	var exclusions = null;
	var self = this;

	var paginationQueue = null;
	var nextUrl = null; // Pagination

	// @public - params: tags, users, callback, sort, exclusions
	this.get = function (params) {
		usersQueue = params.users ? params.users.split(',') : [];
		tagsQueue = params.tags ? params.tags.split(',') : [];
		sort = params.sort;
		callback = params.callback;
		limit = params.limit;
		results = [];
		nextUrl = [];
		paginationQueue = [];
		cache = [];
		exclusions = params.exclusions ? params.exclusions.split(',') : [];

		window.instagramGenericCallback = genericCallback;
		request();
	};

	// @public
	this.getMore = function getMore() {
		results = [];

		presentResults();

		if (cache.length < 50) {  // to improve sorting we get more before finishing the cache
			paginationQueue = nextUrl.slice(0); // Slice(0) is used to duplicate the array
			nextUrl = [];
			requestPagination();
		}
	};

	// @private
	function insertScript(url, insertScriptCallback) {

		//console.log(url);

		var script = document.createElement("script");
		script.type = "text/javascript";

		if (script.readyState) {  //IE
			script.onreadystatechange = function () {
				if (script.readyState == "loaded" ||
								script.readyState == "complete") {
					script.onreadystatechange = null;
					if (typeof (insertScriptCallback) === 'function') insertScriptCallback();
				}
			};
		} else {  //Others
			script.onload = function () {
				if (typeof (insertScriptCallback) === 'function') insertScriptCallback();
			};
		}

		script.src = url;
		document.body.appendChild(script);
	}

	// @private
	function request() {
		//console.log("insta  request");
		if (queueIsNotEmpty()) {
			var apiURL = '';
			if (tagsQueue.length > 0) {
				apiURL = 'tags/' + tagsQueue.pop() + '/media/recent?client_id=587f6c54a721488b874868f462aed4f5';
			} else {
				apiURL = 'users/' + usersQueue.pop() + '/media/recent/?client_id=587f6c54a721488b874868f462aed4f5';
			}
			insertScript('https://api.instagram.com/v1/' + apiURL + '&callback=instagramGenericCallback');
		} else {
			finalize();
		}
	}

	// @private
	function requestPagination() {
		//console.log("insta  requestPagination");
		if (paginationQueueIsNotEmpty()) {
			// script does jsonp-ish request which then calls back genericCallback
			insertScript(paginationQueue.pop());
		} else {
			// all script callbacks have now come back
			finalize();
		}
	}

	// @private
	function genericCallback(response) {
		//console.log("insta  genericCallback");

		if(response.pagination && response.pagination.next_url) {
			nextUrl.push(response.pagination.next_url);
		}

		results = results.concat(response.data);
		if (queueIsNotEmpty()) {
			request();
		} else if (paginationQueueIsNotEmpty()) {
			requestPagination();
		} else {
			finalize();
		}
	};

	// @private
	function queueIsNotEmpty() {
		return usersQueue.length > 0 || tagsQueue.length > 0;
	}

	// @private
	function paginationQueueIsNotEmpty() {
		return paginationQueue.length > 0;
	}
	
	// @private
	function finalize() {
		// all API calls are complete, so the results array is now full
		// store this in cache and display a pageful of results
		if (results.length > 0) {
			if (exclusions.length > 0) {
				results = filter(results);
			}

			// If after the filter the list is empty, request more
			if(results.length == 0) {
//				self.getMore();
			}

			cache = cache.concat(results);

			if (sort) {
				cache = sortResults(cache);
			}
			
			//console.log("insta cache len=", cache.length);
		}

		presentResults();
	}

	// @private
	function presentResults() {
		// take results from cache and display
		var displayResults = [];
		if (cache.length < 50) {
			if (queueIsNotEmpty()) {
				request();
			} else if (paginationQueueIsNotEmpty()) {
				requestPagination();
			} else {
				displayResults = cache.splice(0, limit);  // pop fist 4 off cache and display
			}		
		} else {
			displayResults = cache.splice(0, limit);  // pop fist 4 off cache and display
		}
		
		if (typeof (callback) === 'function') callback(displayResults);
	}

	// @private
	function filter(arr) {
		//console.log('Filter: ' , exclusions);
		var aux = [];
		for (var i = 0; i < arr.length; i++) {
			if (exclusions.indexOf(arr[i].id) == -1) {
				aux.push(arr[i]);
				//console.log(arr[i].id);
			}
		}
		return aux;
	}

	// @private
	function sortResults(arr) {

		var fieldA, fieldB;

		arr.sort(function (a, b) {
			if (sort.parseAsNumber) {
				fieldA = parseInt(a[sort.sortBy]);
				fieldB = parseInt(b[sort.sortBy]);
				if (sort.direction == 'asc') {
					return fieldA - fieldB;
				} else {
					return fieldB - fieldA;
				}
			} else {
				fieldA = a[sort.sortBy].toLowerCase();
				fieldB = b[sort.sortBy].toLowerCase();

				if (sort.direction == 'asc') {
					if (fieldA < fieldB)
						return -1;
					if (fieldA > fieldB)
						return 1;
					return 0;
				} else {
					if (fieldA < fieldB)
						return 1;
					if (fieldA > fieldB)
						return -1;
					return 0;
				}
			}
		});

		return arr;

	};

};