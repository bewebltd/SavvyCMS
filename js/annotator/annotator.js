(function ($) {

	$.fn.annotator = function (options) {
		return new Annotator($(this), options);
	};

	// loader can be either a selector parent containing annotation children html or a url
	$.fn.annotator.defaults = {
		width: '',
		height: '',
		loader: '',
		containment: {
			left: 0,
			top: 0,
			right: 0,
			bottom: 0
		},
		templates: []
	};

	var Annotator = function (ele, options) {
		var rootEle = '';
		var templates = {};

		// This is the easiest way to have default options.
		var settings = $.extend({}, $.fn.annotator.defaults, options);

		function init() {
			if (!settings.loader) return alert('Loader must be set to either an element or url');

			settings.width = settings.width || ele.width();
			settings.height = settings.height || ele.height();

			ele.css({ width: settings.width, height: settings.height });
			rootEle = ele.wrap('<div class="annotator-wrap" style="width: ' + settings.width + 'px; height: ' + settings.height + 'px;" />').parent();
			rootEle.prepend('<div style="position: absolute; top: 0;left: 0; width: ' + settings.width + 'px; height: '+ settings.height +'px;"></div>');
			
			// Load in previous annotations
			load();

			var actions = '';
			$(settings.templates).each(function () {
				templates[this.name] = this;
				templates[this.name].class = $(this.html).attr('class');

				var count = this.limit - $('.' + this.class).length;
				var disabledClass = '';
				if (count <= 0) disabledClass = 'disabled';
				actions += '<div class="annotator-template annotator-template-add '+disabledClass+'" data-template="' + this.name + '">Add ' + this.name + ' (<span>' + (count) + '</span>)</div>';
			});
			rootEle.append('<div class="annotator-templates">' + actions + '</div>');

			// Bind events
			events();
			Drag.bind();
		};

		function events() {
			$('.annotator-template-add').on('click', function () {
				var template = $(this).data('template');
				add(template);
			});

			$(document).on('dblclick', '.annotation .editable', function () {
				var text = $(this).text();

				if ($(this).hasClass('textarea')) {
					$(this).html('<textarea class="annotation-edit">' + text + '</textarea>');
				} else {
					$(this).html('<input type="text" value="' + text + '" class="annotation-edit" />');
				}

				$('.annotation-edit').focus().val(text);
			});

			$(document).on('blur', '.annotation-edit', function () {
				var text = $(this).val().replace(/\r\n|\r|\n/g, '<br />');
				if($.trim(text) == '') {
					text = 'Double click to edit.';
				}
				$(this).parent().html(text);
			});

			$(document).on('mouseover', '.annotation', function () {
				if ($('.annotation-delete').length < 1) {
					$(this).append('<div class="annotation-delete-wrap"><div class="annotation-delete"><span>X</span></div></div>');
				}
			}).on('mouseleave', '.annotation', function () {
				$('.annotation-delete-wrap').remove();
			});

			$(document).on('mousedown', '.annotation-delete', function () {
				var template = '';
				var $ele = $(this).parents('.annotation');

				$(settings.templates).each(function () {
					if ($ele.find('.' + this.class).length > 0) {
						template = this;
					}
				});

				$ele.remove();
				updateButton(template);
			});
		}

		function load() {
			// String would mean, it is a URL
			if (typeof settings.loader === 'string') {
				// Need ajax add here, to complete
			} else {
				$(settings.loader).children().each(function () {
					create($(this));
				});
			}
		}

		function getAnnotationHtml() {
			var html = '';

			$('.annotation').each(function () {
				html += $(this)[0].outerHTML;
			});

			return html;
		}

		function create(html) {
			rootEle.append(html);

			if (!html.hasClass('annotation')) {
				html = html.wrap('<div class="annotation" />').parent();
			}

			if (typeof html.attr('style') === 'undefined') {
				html.css({ top: settings.containment.top, left: settings.containment.left });
			}

			return html;
		}

		function add(templateName) {
			var template = templates[templateName];
			var $html = $(template.html);

			if ($('.' + $html.attr('class')).length >= template.limit) return;
			$('.editable', $html).text('Double click to edit.');
			
			create($html);
			updateButton(template);
		}

		function updateButton(template) {
			var $btn = $('.annotator-template-add[data-template="' + template.name + '"]');
			var count = template.limit - $('.' + template.class, '.annotation').length;

			$('span', $btn).html(count);

			if(count <= 0) {
				$btn.addClass('disabled');
			} else {
				$btn.removeClass('disabled');
			}
		}

		var Drag = {
			containment: [],

			bind: function () {
				this.containment = [settings.containment.left || 0,
														settings.containment.top || 0,
														settings.containment.right || rootEle.width(),
														settings.containment.bottom || rootEle.height()];

				$(document).on('mousedown', '.annotation', function (downEvent) {
					Drag.onDrag.call(this, downEvent);

					$(document).on('mouseup', function () {
						Drag.offDrag.call(this);
					});
				});
			},

			onDrag: function (downEvent) {
				$(this).addClass('draggable');
				var parentOffset = rootEle.position();

				$(document).on('mousemove', function (moveEvent) {
					var draggableEle = $('.annotation.draggable');
					if (draggableEle.length < 1 || $('.annotation-edit').length > 0) return;

					var offsetY = downEvent.offsetY || downEvent.originalEvent.layerY;
					var offsetX = downEvent.offsetX || downEvent.originalEvent.layerX;

					var params = {
						moveY: moveEvent.pageY - parentOffset.top - offsetY,
						moveX: moveEvent.pageX - parentOffset.left - offsetX
					};

					if (downEvent.offsetY) {
						params.moveY -= downEvent.srcElement.offsetTop;
					}

					var movePos = Drag._generatePosition.call(draggableEle, params);

					draggableEle.css({ top: movePos.top });
					draggableEle.css({ left: movePos.left });
				});
			},

			offDrag: function () {
				$(document).unbind('mousemove');
				$('.annotation.draggable').removeClass('draggable');
			},

			_generatePosition: function (params) {
				var elePos = $(this).position();
				var nextX = params.moveX;
				var nextY = params.moveY;
				var eleWidth = $(this).width();
				var eleHeight = $(this).height();

				// Left
				if (elePos.left <= Drag.containment[0] && params.moveX <= Drag.containment[0]) {
					nextX = Drag.containment[0];
				}

				// Top
				if (elePos.top <= Drag.containment[1] && params.moveY <= Drag.containment[1]) {
					nextY = Drag.containment[1];
				}

				// Right
				if (elePos.left + eleWidth >= Drag.containment[2] && params.moveX >= Drag.containment[2] - eleWidth) {
					nextX = Drag.containment[2] - eleWidth;
				}

				// Bottom
				if (elePos.top + eleHeight >= Drag.containment[3] && params.moveY >= Drag.containment[3] - eleHeight) {
					nextY = Drag.containment[3] - eleHeight;
				}

				return {
					top: nextY,
					left: nextX
				};
			}
		};

		// Start the ball rolling
		init();

		return {
			load: load,
			create: create,
			getAnnotationHtml: getAnnotationHtml
		};
	};

}(jQuery));