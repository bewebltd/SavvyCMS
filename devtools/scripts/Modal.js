/*

    Modal: Created By Andre Feijo - Mar 2014
    Example Usage:
    
    <div id="example">Modal content goes here</div>
    
    var myModal = 
    $('#example').modal({
        width: '600px',
        closeButton: false,
        buttons: [
            {
                label: "Test Button",
                cssClass: 'myCustomClass',
                click: function(modal) {
                    alert($(this).text() + ' clicked');
                    modal.close(); // Closes modal
                }
            }
        ]
    }).show();
    
    // To close:
    myModal.close();

*/

(function ($) {

	$.fn.modal = function (options) {
		return new Modal($(this), options);
	};

	$.fn.modal.defaults = {
		top: '80px',
		left: null,
		width: '690px',
		height: null,
		closeButton: true,
		buttons: []
	};

	$.fn.modal.instances = {};
	$.fn.modal.previousModal = null;

	var Modal = function (ele, options) {

		var settings = $.extend({}, $.fn.modal.defaults, options);

		function init() {
			if ($('.overlay').length == 0) {
				$('body').append('<div class="overlay"></div>');
			}
		}

		function createIfNotExists() {
			var id = (ele.attr('id') ? ele.attr('id') : 'gen' + Math.floor(Math.random() * 1000) + 1) + '_modal';

			if ($('#' + id).length > 0) return;

			this.instance = $('<div class="modal" id="' + id + '"><div class="' + (ele.attr('class') ? ele.attr('class') : '') + '">' + ele.html() + '</div><div class="modal-buttons"></div></div>');
			$this = this;

			var btns = [];

			$(settings.buttons).each(function () {
				var btn = $('<span class="btn">No Label</span>');

				if (this.label) {
					btn.html(this.label);
				}

				if (this.cssClass) {
					btn.addClass(this.cssClass);
				}

				if (this.click && typeof (this.click === 'function')) {
					var fn = this.click;
					btn.bind('click', function () {
						fn.apply(this, [$this]);
					});
				}

				btns.push(btn);
			});

			var left = settings.left;
			if (!left) {
				left = (($(window).width() / 2) - (parseInt(settings.width.replace(/[^0-9]*/g, "")) / 2)) + 'px';
			}

			this.instance.css({
				top: settings.top,
				left: left,
				width: settings.width,
				height: settings.height,
				display: 'none'
			});

			$('body').append(this.instance);

			if (settings.closeButton) {
				var closeButton = $('<span class="btn close-button" title="Close">x</span>');
				closeButton.bind('click', function () {
					$this.close();
				});
				this.instance.prepend(closeButton);
			}

			$('.modal-buttons', this.instance).append(btns);

			$.fn.modal.instances[id] = this;
		}

		function show() {
			createIfNotExists.apply(this);

			var visibleModal = $('.modal:visible');

			// If it's currently showing another modal, hides to show again when this one get closed
			if (visibleModal.length > 0 && visibleModal.attr('id') != ele.attr('id')) {
				var instance = $.fn.modal.instances[visibleModal.attr('id')];
				if (instance) {
					$.fn.modal.previousModal = instance;
					instance.close();
				}
			}

			$('.overlay').show();
			$('body').addClass('modal-overflow');
			this.instance.show();
			scroll(0,0);
			return this;
		};

		function close() {

			$('.overlay').hide();
			$('body').removeClass('modal-overflow');
			var visibleModal = $('.modal:visible');

			// If there is a previous modal to show and we are not closing itself
			if ($.fn.modal.previousModal && $.fn.modal.previousModal.instance.attr('id') != visibleModal.attr('id')) {
				$.fn.modal.previousModal.show();
				$.fn.modal.previousModal = null;
			}

			visibleModal.hide();
			return this;
		}

		init();

		return {
			show: show,
			close: close
		};
	};

}(jQuery));