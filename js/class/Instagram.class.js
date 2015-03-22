// @class
function Instagram() {

	// @public
	this.init = function init() {

		var api = new InstagramAPI();

		api.get({
			tags: 'steinlager,allblacks',
			users: '306324963',
			sort: {
				sortBy: 'created_time',
				direction: 'desc',
				parseAsNumber: true
			},
			callback: callback,
			limit: 12
		});
	};

	// @private
	function callback(arr) {

		var html = '';

		console.log(arr[0]);

		for (var i = 0; i < arr.length; i++) {
			var date = timeSince(new Date(parseInt(arr[i].created_time) * 1000));
			html += '<div class="instagram-box"> \
								<p class="image"> \
									<a href="' + arr[i].link + '"> \
										<img src="' + arr[i].images.thumbnail.url + '"> \
									</a> \
								</p> \
								<p class="user-info clearfix"> \
									<img src="' + arr[i].user.profile_picture + '">  \
									<span>by <strong>' + arr[i].user.full_name + '</strong></span> \
									<span><small>' + date + '</small></span> \
								</p>  \
								<p class="message"> \
									' + (arr[i].caption ? highlightTags(arr[i].caption.text) : '') + '  \
								</p>  \
								<ul class="share clearfix"> \
									<li><a href="#" class="share-twitter"><i>Share on Twitter</i></a></li> \
									<li><a href="#" class="share-facebook"><i>Share on Facebook</i></a></li> \
									<li><a href="#" class="share-pinterest"><i>Share on Pinterest</i></a></li> \
									<li><a href="#" class="share-instagram"><i>Share on Instagram</i></a></li> \
									<li class="flag"><a href="#"><i>Flag as inappropriate</i></a></li> \
								</ul> \
							</div>';
		}

		$('#instagram-list').html(html);
		instagram.calculateHeight();
	}

	// @private
	function highlightTags(text) {
		return text.replace(/(^|\s|-)+(@|#)(\w+)/g, '<strong>$&</strong>');
	}

	// @private
	function timeSince(date) {
		if (typeof date !== 'object') {
			date = new Date(date);
		}

		var seconds = Math.floor((new Date() - date) / 1000);
		var intervalType;

		var interval = Math.floor(seconds / 31536000);
		if (interval >= 1) {
			intervalType = 'year';
		} else {
			interval = Math.floor(seconds / 2592000);
			if (interval >= 1) {
				intervalType = 'month';
			} else {
				interval = Math.floor(seconds / 86400);
				if (interval >= 1) {
					intervalType = 'day';
				} else {
					interval = Math.floor(seconds / 3600);
					if (interval >= 1) {
						intervalType = "hour";
					} else {
						interval = Math.floor(seconds / 60);
						if (interval >= 1) {
							intervalType = "minute";
						} else {
							if (interval === 0) return "Just now";
							intervalType = "second";
						}
					}
				}
			}
		}

		if (interval > 1) {
			intervalType += 's';
		}

		return interval + ' ' + intervalType + ' ago';
	}

	// @public
	this.calculateHeight = function calculateHeight() {

		// Reset margin
		$('#instagram-list .lastRowItem').removeClass('lastRowItem');

		var boxes = $('#instagram-list .instagram-box');
		var itemsPerRow = 0;
		var lastOffsetTop = 0;
		
		boxes.each(function () {
			var top = $(this).offset().top;
			if (lastOffsetTop != top && itemsPerRow > 0) {
				return false; // break;	
			}
			itemsPerRow++;
			lastOffsetTop = top;
		});

		var columnIndex = 1;
		var highestHeight = 0;

		boxes.each(function () {
			highestHeight = Math.max(highestHeight, $(this).find('.message').height());
			$(this).addClass('needsANewHeight');
			if (columnIndex === itemsPerRow) {
				$(this).addClass('lastRowItem');
				$('.needsANewHeight .message').css('height', highestHeight + 'px');
				$('.needsANewHeight').removeClass('needsANewHeight');
				columnIndex = 1;
				highestHeight = 0;
			} else {
				columnIndex++;
			}
		});
	};

	// @private 
	function ellipsis(src, len) {
		var result = src + "";
		if (result.length > len) {
			// try and split on a word if there is an obvious point to break on
			var spacePos = result.lastIndexOf(' ', len - 3);
			if (spacePos > len * 0.5) {
				result = result.substr(0, spacePos);
			}
			// make sure never longer than max len
			if (result.length > len - 3) {
				result = result.substr(0, len - 3);
			}
			// add ... because to indicate there was more
			result = result + "...";
		}
		return result;
	}

};

var instagram = new Instagram();