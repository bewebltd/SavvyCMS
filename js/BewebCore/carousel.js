(function ($) {

	// Example Css for a basic slide carousel
	// Slides contain the all the content. Image, text, html etc.
	// Feel free to change the classes
	// Remember to Change the wrapper and slide width and height to what you require.
	//
	//.carousel-wrapper { position:relative;}
	//.carousel-wrapper .carousel{ width: 228px; height: 200px; clear: both; overflow: hidden; position: relative; margin:0;	padding:0;	list-style: none; }
	//.carousel-wrapper .carousel .slide{ position: absolute; }
	//.carousel-wrapper .carousel .slide.active{ display: block; z-index: 99; }
	//.carousel-tab { position: absolute; text-align: center; width: 228px; margin-top: -20px; z-index: 100; }
	//.carousel-tab a { background: #000000; display: inline-block; margin: 0 3px; width: 10px; height: 10px; -webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; opacity: 0.35; filter: alpha(opacity=35); }
	//.carousel-tab a.active {background: #ffffff; opacity: 0.60; filter: alpha(opacity=60); }
	//
	//<div class="carousel-wrapper">
	//	<ul class="carousel">
	//		<li class="slide"></li>
	//		<li class="slide"></li>
	//		<li class="slide"></li>
	//	</ul> 
	//				
	//	<div class="carousel-tab">
	//		<a href="#" class="tab"></a>
	//		<a href="#" class="tab"></a>
	//		<a href="#" class="tab"></a>
	//	</div>
	//				
	//	<div class="clear"></div>
	//</div>
	//
	// To run the carousel, there are also a number of defaults that are configurable.
	// Note: If you need to use multiple instances of carousel, initialise it for each one. Example:
	//		$('.carousel-1').svyCarousel({ effect: 'slide', delay: 2000, tabClass: '.carousel-tab a' });
	//		$('.carousel-2').svyCarousel({ effect: 'slide', delay: 2000, tabClass: '.carousel-tab a' });
	// DO NOT do this:
	//		$('.carousel-1, .carousel-2').svyCarousel({ effect: 'slide', delay: 2000, tabClass: '.carousel-tab a' });
	//
	//<script type="text/javascript">
	//	$(document).ready(function () {
	//		$('.carousel').svyCarousel({ effect: 'slide', delay: 2000, tabClass: '.carousel-tab a' });
	//	});
	//</script>

	// Effects: slide, slideUp, fade, slideParallax

	// TODO Auto generate tabs, next, prev controls

	$.fn.svyCarousel = function (options) {
		return new CarouselSlider(this, options);
	};

	// Carousel Defaults that can be overridden globally
	$.fn.svyCarousel.defaults = {
		'speed': 1000,
		'delay': 1000,
		'width': 0,
		'height': 0,
		'effect': 'slide',
		'pauseOnHover': true,
		'paused': false,
		'autoSlide': true,
		'animateBeginning': false,    // if true, the effect will run on startup
		'firstSlide': 0,             // index of slide to start on
		'prevClass': '',
		'nextClass': '',
		'tabClass': '.carousel-tab .tab',
		'direction': '',         // Static direction, overrides tab directioning "prev, next"
		'beforeSlide': function () { },
		'afterSlide': function () { },
		'debug': false
	};

	var CarouselSlider = function (element, options) {
		// Private Variables
		var data = {};
		var $carousel = $(element);
		var slidesCount = 0;
		var effect = '';
		var isRunning = false;
		var timer = '';

		// This is the easiest way to have default options.
		var settings = $.extend({}, $.fn.svyCarousel.defaults, options);

		var carousel = {
			// Initialises the carousel, runs the init's of the effect plugins
			init: function () {
				settings.width = settings.width == 0 ? $carousel.width() : settings.width;
				settings.height = settings.height == 0 ? $carousel.height() : settings.height;

				// Set default control selectors if they are using an id or class
				var name = $carousel.prop('id') != "" ? $carousel.prop('id') : $carousel.prop('class');
				settings.prevClass = settings.prevClass == '' ? '.' + name + '-prev' : settings.prevClass;
				settings.nextClass = settings.nextClass == '' ? '.' + name + '-next' : settings.nextClass;
				settings.tabClass = settings.tabClass == '' ? '.' + name + '-tabs' : settings.tabClass;

				effect = $.fn.svyCarousel[settings.effect];
				data.slides = [];

				$($carousel.children()).each(function (i) {
					var tab = $(settings.tabClass)
					tab.eq(i).addClass('tab');

					var slide = {
						ele: $(this),
						tab: tab.eq(i)
					}

					data.slides.push(slide);
					slidesCount++;

					// Set the first slider to active and run the effect init on the rest
					//debugger
					if (i === settings.firstSlide) {
						$(this).addClass('active');
						tab.eq(settings.firstSlide).addClass('active');
					} else {
						$(this).css({ 'display': 'none' })
					}
				}); // end loop of each slide

				if (slidesCount == 0) {
					// there are no slides so do not run
					if (window.console) window.console.log("SavvyCarousel - there are no slides so not running");
					return;
				}

				// Setup events
				if (settings.pauseOnHover) {
					$carousel.hover(function () {
						settings.paused = true;
						clearTimeout(timer);
					}, function () {
						settings.paused = false;
						clearTimeout(timer);
						if (settings.autoSlide) {
							carousel.run();
						}
					});
				}

				$carousel.on('beforeSlide', $.proxy(settings.beforeSlide, data));
				$carousel.on('afterSlide', $.proxy(settings.afterSlide, data));

				if($('body').hasClass('mobile')) {
					// Setup mobile swipe gestures
					$('.carousel-wrapper').swipe({
						swipe: function (event, direction, distance, duration, fingerCount) {
							if (direction == 'left') {
								clearTimeout(timer);
								carousel.run('next', 0, null, true);
							} else if (direction == 'right') {
								clearTimeout(timer);
								carousel.run('prev', 0, null, true);
							}
						},
						threshold: 100
					});
				} else {
					// Setup arrows
					$(settings.prevClass).on('click', function () {
						clearTimeout(timer);
						carousel.run('prev', 0, null, true);
						return false;
					});

					$(settings.nextClass).on('click', function () {
						clearTimeout(timer);
						carousel.run('next', 0, null, true);
						return false;
					});
				}

				// Hide arrows for a better mobile experience or when there's only one slide
				if($('body').hasClass('mobile') || slidesCount === 1) {
					$(settings.prevClass).hide();
					$(settings.nextClass).hide();
				}
				
				//debugger
				$(settings.tabClass).on('click', function () {    // MN 20130820 - changed from hover to mouseover, as it was not doign anything until mouse off
					clearTimeout(timer); // Not sure if this is needed?? // This is most definatly needed, if the tab is clicked or hovered and the current timer hasn't been cleared then it will skip/jump to the next slide JB 20131022 
					if ($(this).hasClass('active')) return false;
					carousel.run('tab', 0, $(this));
					return false;
				});

				//chuck the carousel jq obj in the settings
				settings.$carousel = $carousel;
				// load image for first slide only, and cascade load others after that
				carousel.loadSlideImages(data.slides[0].ele, 0);

				// Run carousel
				if (settings.autoSlide) {
					carousel.run();
				}
				if (effect.begin) {
					effect.begin(data.currentSlide, settings);
				}
				if (settings.animateBeginning) {
					$(window).load(function () {
						// start the slideshow once loaded, to ensure smoothness
						effect.setup(data.currentSlide, settings.width, "next");
						effect.run(data.currentSlide, data.currentSlide, settings, "next");
						$(":animated", $carousel).promise().always(function () {
							carousel.callBack();
						});
					});
				}
			},

			// load one slide
			loadSlideImages: function (ele, index) {
				var $img = ele.find('img').first();  // assume first image is the slide background
				var loadsrc = $img.data('loadsrc');
				if (loadsrc) {
					var img = $img[0];
					carousel.debug('begin load carousel image: ' + loadsrc);
					img.src = loadsrc;
					img.onload = function () {
						if (img.isLoaded) return;  // dont trigger more loading if already done this one (so can have fallback timer)
						img.isLoaded = true;
						carousel.debug('loaded carousel image: ' + img.src);
						$(img).addClass('imdone');
						// load next image
						var nextIndex = index + 1;
						if (nextIndex < slidesCount) {
							carousel.loadSlideImages(data.slides[nextIndex].ele, nextIndex);
						}
					}
					// fallback timer in case image is broken and never loads, therefore other images can still load
					window.setTimeout(img.onload, 5000);
				}

			},

			// Handles the running of the slider, (delays, pauses)
			run: function (action, delay, ele, forceRun) {
				// Is it not paused and not running
				if ((!settings.paused && !isRunning && slidesCount > 1) || (forceRun && !isRunning && slidesCount > 1)) {

					this.clearTimer();
					carousel.setSlides(action, ele);

					var timeDelay = delay != null ? delay : settings.delay;
					timer = setTimeout(function () {
						isRunning = true;
						carousel.slide();
					}, timeDelay);
				}
			},

			// Runs the slide effect plugins
			slide: function () {
				$carousel.trigger('beforeSlide');

				// Change tab
				carousel.tabControl();

				effect.run(data.currentSlide, data.nextSlide, settings, data.direction);

				carousel.checkAnimationHasFinished();
			},

			checkAnimationHasFinished: function () {
				// Wait for the animation to finish, then call the callback
				$(":animated", $carousel).promise().done(function () {
					if ($(":animated", $carousel).length > 0) {
						return carousel.checkAnimationHasFinished();
					}

					isRunning = false;

					data.currentSlide.removeClass('active');
					data.nextSlide.addClass('active');

					if (effect.complete) {
						effect.complete(data.currentSlide, settings);
					}

					carousel.callBack();
				});
			},

			setSlides: function (action, ele) {
				var currentSlide = $carousel.find('.active');
				var nextSlide = '';
				var direction = '';
				if (action == 'prev') {
					var prevEle = currentSlide.prev();

					if (prevEle.length === 0) {
						nextSlide = data.slides[slidesCount - 1].ele;
					} else {
						nextSlide = prevEle;
					}

					direction = 'prev';

				} else if (action == 'tab') {
					var tabIndex = ele.index();

					nextSlide = data.slides[tabIndex].ele;
					direction = ele.prevAll('.active').length > 0 ? 'next' : 'prev';

				} else {
					var nextEle = currentSlide.next();

					if (nextEle.length === 0) {
						nextSlide = data.slides[0].ele;
					} else {
						nextSlide = nextEle;
					}

					direction = 'next';
				}

				data.nextSlide = nextSlide;
				data.currentSlide = currentSlide;
				data.direction = settings.direction ? settings.direction : direction;
			},

			callBack: function () {
				$carousel.trigger('afterSlide');

				if (settings.autoSlide) {
					carousel.run();
				}
			},

			tabControl: function () {
				//debugger
				//var currentTab = $('.active', $(settings.tabClass));
				var currentTab = $(settings.tabClass).filter('.active')
				currentTab.removeClass('active');

				var nextTab = data.slides[data.nextSlide.index()].tab;
				$(nextTab, settings.tabClass).addClass('active');
			},

			settings: function (settingName, value) {
				if (settingName == null || value == null) {
					return settings;
				}

				settings[settingName] = value;
			},

			slides: data.slides,
			
			getNextSlide: function () { return data.nextSlide || []; },
			getCurrentSlide: function () { return data.currentSlide || []; },

			clearTimer: function () {
				clearTimeout(timer);
			},

			debug: function (msg) {
				if (settings.debug) {
					if (window.console) console.log(msg);
				}
			}
		};
		// Init Carousel
		carousel.init();

		return carousel;
	};

	$.fn.svyCarousel.slide = {
		run: function (currentSlide, nextSlide, settings, direction) {
			var offset = 0;

			offset = (direction == 'prev') ? settings.width : -settings.width;
			this.setup(nextSlide, offset);

			$(currentSlide).stop().animate({ 'left': offset + 'px' }, settings.speed);
			$(nextSlide).stop().animate({ 'left': '0px' }, settings.speed);
		},

		setup: function (ele, offset, direction) {
			offset = (offset > 0) ? -offset : Math.abs(offset);
			$(ele).css({ 'left': offset + 'px', 'display': 'block' });
		}
	};

	$.fn.svyCarousel.slideParallax = {
		// slides multiple layers with class "layer" at slightly offset times
		// timing can be set per layer using data-speed attrib
		// turns clipping on and off when animating or not, so you can have elements within the slide extend outside its bounds (eg hover tooltips)

		begin: function (currentSlide, settings) {
			// make sure overflow is clipped during animation 
			settings.$carousel.css({ "overflow": "hidden" });

			$(".layer", currentSlide).css({ 'left': settings.width + 'px', 'display': 'block' });
		},

		run: function (currentSlide, nextSlide, settings, direction) {
			var offset = 0;

			// make sure overflow is clipped during animation 
			settings.$carousel.css({ "overflow": "hidden" });

			offset = (direction == 'prev') ? settings.width : -settings.width;
			this.setup(nextSlide, offset);

			$(currentSlide).stop().animate({ 'left': offset + 'px' }, settings.speed);
			$(nextSlide).stop().animate({ 'left': '0px' }, settings.speed);
			// move layers too
			var layerNum = 0;
			$(".layer", nextSlide).each(function () {
				layerNum++;
				var speed = $(this).data("speed");
				if (!speed) {
					speed = layerNum * 250;
				}
				$(this).stop().animate({ 'left': '0px' }, settings.speed + speed);
			});

			$(":animated", settings.$carousel).promise().always(function () {
				// move it off screen so we can allow overflow now animation has completed
				$(currentSlide).css({ 'top': '-1000px', left: 0 });
				settings.$carousel.css({ "overflow": "visible" });
			});
		},

		setup: function (ele, offset) {
			offset = (offset > 0) ? -offset : Math.abs(offset);
			$(ele).css({ 'left': offset + 'px', 'display': 'block', 'top': '0' });
			// reset layers too
			$(".layer", ele).css({ 'left': offset + 'px', 'display': 'block' });
		}
	};

	$.fn.svyCarousel.slideUp = {
		run: function (currentSlide, nextSlide, settings, direction) {
			var offset = 0;

			offset = (direction == 'prev') ? -settings.height : settings.height;
			this.setup(nextSlide, offset);

			$(currentSlide).stop().animate({ 'bottom': offset + 'px' }, settings.speed);
			$(nextSlide).stop().animate({ 'bottom': '0px' }, settings.speed);
		},

		setup: function (ele, offset) {
			offset = (offset > 0) ? -offset : Math.abs(offset);
			$(ele).css({ 'bottom': offset + 'px', 'display': 'block' });
		}
	};


	$.fn.svyCarousel.fade = {
		run: function (currentSlide, nextSlide, settings) {
			this.setup(nextSlide);
			$(currentSlide).stop().animate({ 'opacity': 0 }, settings.speed, function () {
				$(this).hide();
			});
			$(nextSlide).show().css({ 'opacity': 0 });
			$(nextSlide).stop().animate({ 'opacity': 1 }, settings.speed);
		},

		setup: function (ele) {
			$(ele).css({ 'opacity': 0, 'display': 'block' });
		}
	};

	$.fn.svyCarousel.slice = {
		numOfSlices: 10,

		begin: function (currentSlide, settings) {
			this.createSlices(currentSlide, settings.width);
		},

		run: function (currentSlide, nextSlide, settings) {
			this.setup(nextSlide, settings.width);

			$('.slices .slice', currentSlide).each(function (i) {
				if (i == 0) {
					$(this).animate({ top: -settings.height }, settings.speed);
				} else {
					if (i % 2 == 0) {
						$(this).delay(200 * i).animate({ top: -settings.height }, settings.speed);
					} else {
						$(this).delay(200 * i).animate({ top: settings.height }, settings.speed);
					}
				}
			});
			$('img', currentSlide).css({ 'display': 'none' });
			$(nextSlide).css({ 'display': 'block' });
		},

		setup: function (ele, width) {
			$('img', ele).css({ 'display': 'block' });
			this.createSlices(ele, width);
		},

		complete: function (ele) {
			$('.slices', ele).remove();
		},

		createSlices: function (ele, width) {
			var sliceWidth = width / this.numOfSlices;
			var imgUrl = $('img', ele).attr('src');

			var sliceContainer = $('<div class="slices"></div>').appendTo(ele);

			for (var i = 1; i <= this.numOfSlices; i++) {
				var offset = ((sliceWidth * i) - sliceWidth);
				var slice = $('<div class="slice" style="background: url(\'' + imgUrl + '\') -' + offset + 'px 0 no-repeat; position: absolute; width: ' + sliceWidth + 'px; height: ' + $(ele).height() + 'px; top: 0; left: ' + offset + 'px;"></div>');

				slice.appendTo(sliceContainer);
			}
		}
	};

}(jQuery));


/*
* @fileOverview TouchSwipe - jQuery Plugin
* @version 1.6.6
*
* @author Matt Bryson http://www.github.com/mattbryson
* @see https://github.com/mattbryson/TouchSwipe-Jquery-Plugin
* @see http://labs.rampinteractive.co.uk/touchSwipe/
* @see http://plugins.jquery.com/project/touchSwipe
*
* Copyright (c) 2010-2015 Matt Bryson
* Dual licensed under the MIT or GPL Version 2 licenses.
*
*/
(function (a) { if (typeof define === "function" && define.amd && define.amd.jQuery) { define(["jquery"], a) } else { a(jQuery) } }(function (f) { var p = "left", o = "right", e = "up", x = "down", c = "in", z = "out", m = "none", s = "auto", l = "swipe", t = "pinch", A = "tap", j = "doubletap", b = "longtap", y = "hold", D = "horizontal", u = "vertical", i = "all", r = 10, g = "start", k = "move", h = "end", q = "cancel", a = "ontouchstart" in window, v = window.navigator.msPointerEnabled && !window.navigator.pointerEnabled, d = window.navigator.pointerEnabled || window.navigator.msPointerEnabled, B = "TouchSwipe"; var n = { fingers: 1, threshold: 75, cancelThreshold: null, pinchThreshold: 20, maxTimeThreshold: null, fingerReleaseThreshold: 250, longTapThreshold: 500, doubleTapThreshold: 200, swipe: null, swipeLeft: null, swipeRight: null, swipeUp: null, swipeDown: null, swipeStatus: null, pinchIn: null, pinchOut: null, pinchStatus: null, click: null, tap: null, doubleTap: null, longTap: null, hold: null, triggerOnTouchEnd: true, triggerOnTouchLeave: false, allowPageScroll: "auto", fallbackToMouseEvents: true, excludedElements: "label, button, input, select, textarea, a, .noSwipe", preventDefaultEvents: true }; f.fn.swipe = function (G) { var F = f(this), E = F.data(B); if (E && typeof G === "string") { if (E[G]) { return E[G].apply(this, Array.prototype.slice.call(arguments, 1)) } else { f.error("Method " + G + " does not exist on jQuery.swipe") } } else { if (!E && (typeof G === "object" || !G)) { return w.apply(this, arguments) } } return F }; f.fn.swipe.defaults = n; f.fn.swipe.phases = { PHASE_START: g, PHASE_MOVE: k, PHASE_END: h, PHASE_CANCEL: q }; f.fn.swipe.directions = { LEFT: p, RIGHT: o, UP: e, DOWN: x, IN: c, OUT: z }; f.fn.swipe.pageScroll = { NONE: m, HORIZONTAL: D, VERTICAL: u, AUTO: s }; f.fn.swipe.fingers = { ONE: 1, TWO: 2, THREE: 3, ALL: i }; function w(E) { if (E && (E.allowPageScroll === undefined && (E.swipe !== undefined || E.swipeStatus !== undefined))) { E.allowPageScroll = m } if (E.click !== undefined && E.tap === undefined) { E.tap = E.click } if (!E) { E = {} } E = f.extend({}, f.fn.swipe.defaults, E); return this.each(function () { var G = f(this); var F = G.data(B); if (!F) { F = new C(this, E); G.data(B, F) } }) } function C(a4, av) { var az = (a || d || !av.fallbackToMouseEvents), J = az ? (d ? (v ? "MSPointerDown" : "pointerdown") : "touchstart") : "mousedown", ay = az ? (d ? (v ? "MSPointerMove" : "pointermove") : "touchmove") : "mousemove", U = az ? (d ? (v ? "MSPointerUp" : "pointerup") : "touchend") : "mouseup", S = az ? null : "mouseleave", aD = (d ? (v ? "MSPointerCancel" : "pointercancel") : "touchcancel"); var ag = 0, aP = null, ab = 0, a1 = 0, aZ = 0, G = 1, aq = 0, aJ = 0, M = null; var aR = f(a4); var Z = "start"; var W = 0; var aQ = null; var T = 0, a2 = 0, a5 = 0, ad = 0, N = 0; var aW = null, af = null; try { aR.bind(J, aN); aR.bind(aD, a9) } catch (ak) { f.error("events not supported " + J + "," + aD + " on jQuery.swipe") } this.enable = function () { aR.bind(J, aN); aR.bind(aD, a9); return aR }; this.disable = function () { aK(); return aR }; this.destroy = function () { aK(); aR.data(B, null); aR = null }; this.option = function (bc, bb) { if (av[bc] !== undefined) { if (bb === undefined) { return av[bc] } else { av[bc] = bb } } else { f.error("Option " + bc + " does not exist on jQuery.swipe.options") } return null }; function aN(bd) { if (aB()) { return } if (f(bd.target).closest(av.excludedElements, aR).length > 0) { return } var be = bd.originalEvent ? bd.originalEvent : bd; var bc, bb = a ? be.touches[0] : be; Z = g; if (a) { W = be.touches.length } else { bd.preventDefault() } ag = 0; aP = null; aJ = null; ab = 0; a1 = 0; aZ = 0; G = 1; aq = 0; aQ = aj(); M = aa(); R(); if (!a || (W === av.fingers || av.fingers === i) || aX()) { ai(0, bb); T = at(); if (W == 2) { ai(1, be.touches[1]); a1 = aZ = au(aQ[0].start, aQ[1].start) } if (av.swipeStatus || av.pinchStatus) { bc = O(be, Z) } } else { bc = false } if (bc === false) { Z = q; O(be, Z); return bc } else { if (av.hold) { af = setTimeout(f.proxy(function () { aR.trigger("hold", [be.target]); if (av.hold) { bc = av.hold.call(aR, be, be.target) } }, this), av.longTapThreshold) } ao(true) } return null } function a3(be) { var bh = be.originalEvent ? be.originalEvent : be; if (Z === h || Z === q || am()) { return } var bd, bc = a ? bh.touches[0] : bh; var bf = aH(bc); a2 = at(); if (a) { W = bh.touches.length } if (av.hold) { clearTimeout(af) } Z = k; if (W == 2) { if (a1 == 0) { ai(1, bh.touches[1]); a1 = aZ = au(aQ[0].start, aQ[1].start) } else { aH(bh.touches[1]); aZ = au(aQ[0].end, aQ[1].end); aJ = ar(aQ[0].end, aQ[1].end) } G = a7(a1, aZ); aq = Math.abs(a1 - aZ) } if ((W === av.fingers || av.fingers === i) || !a || aX()) { aP = aL(bf.start, bf.end); al(be, aP); ag = aS(bf.start, bf.end); ab = aM(); aI(aP, ag); if (av.swipeStatus || av.pinchStatus) { bd = O(bh, Z) } if (!av.triggerOnTouchEnd || av.triggerOnTouchLeave) { var bb = true; if (av.triggerOnTouchLeave) { var bg = aY(this); bb = E(bf.end, bg) } if (!av.triggerOnTouchEnd && bb) { Z = aC(k) } else { if (av.triggerOnTouchLeave && !bb) { Z = aC(h) } } if (Z == q || Z == h) { O(bh, Z) } } } else { Z = q; O(bh, Z) } if (bd === false) { Z = q; O(bh, Z) } } function L(bb) { var bc = bb.originalEvent; if (a) { if (bc.touches.length > 0) { F(); return true } } if (am()) { W = ad } a2 = at(); ab = aM(); if (ba() || !an()) { Z = q; O(bc, Z) } else { if (av.triggerOnTouchEnd || (av.triggerOnTouchEnd == false && Z === k)) { bb.preventDefault(); Z = h; O(bc, Z) } else { if (!av.triggerOnTouchEnd && a6()) { Z = h; aF(bc, Z, A) } else { if (Z === k) { Z = q; O(bc, Z) } } } } ao(false); return null } function a9() { W = 0; a2 = 0; T = 0; a1 = 0; aZ = 0; G = 1; R(); ao(false) } function K(bb) { var bc = bb.originalEvent; if (av.triggerOnTouchLeave) { Z = aC(h); O(bc, Z) } } function aK() { aR.unbind(J, aN); aR.unbind(aD, a9); aR.unbind(ay, a3); aR.unbind(U, L); if (S) { aR.unbind(S, K) } ao(false) } function aC(bf) { var be = bf; var bd = aA(); var bc = an(); var bb = ba(); if (!bd || bb) { be = q } else { if (bc && bf == k && (!av.triggerOnTouchEnd || av.triggerOnTouchLeave)) { be = h } else { if (!bc && bf == h && av.triggerOnTouchLeave) { be = q } } } return be } function O(bd, bb) { var bc = undefined; if ((I() || V()) || (P() || aX())) { if (I() || V()) { bc = aF(bd, bb, l) } if ((P() || aX()) && bc !== false) { bc = aF(bd, bb, t) } } else { if (aG() && bc !== false) { bc = aF(bd, bb, j) } else { if (ap() && bc !== false) { bc = aF(bd, bb, b) } else { if (ah() && bc !== false) { bc = aF(bd, bb, A) } } } } if (bb === q) { a9(bd) } if (bb === h) { if (a) { if (bd.touches.length == 0) { a9(bd) } } else { a9(bd) } } return bc } function aF(be, bb, bd) { var bc = undefined; if (bd == l) { aR.trigger("swipeStatus", [bb, aP || null, ag || 0, ab || 0, W, aQ]); if (av.swipeStatus) { bc = av.swipeStatus.call(aR, be, bb, aP || null, ag || 0, ab || 0, W, aQ); if (bc === false) { return false } } if (bb == h && aV()) { aR.trigger("swipe", [aP, ag, ab, W, aQ]); if (av.swipe) { bc = av.swipe.call(aR, be, aP, ag, ab, W, aQ); if (bc === false) { return false } } switch (aP) { case p: aR.trigger("swipeLeft", [aP, ag, ab, W, aQ]); if (av.swipeLeft) { bc = av.swipeLeft.call(aR, be, aP, ag, ab, W, aQ) } break; case o: aR.trigger("swipeRight", [aP, ag, ab, W, aQ]); if (av.swipeRight) { bc = av.swipeRight.call(aR, be, aP, ag, ab, W, aQ) } break; case e: aR.trigger("swipeUp", [aP, ag, ab, W, aQ]); if (av.swipeUp) { bc = av.swipeUp.call(aR, be, aP, ag, ab, W, aQ) } break; case x: aR.trigger("swipeDown", [aP, ag, ab, W, aQ]); if (av.swipeDown) { bc = av.swipeDown.call(aR, be, aP, ag, ab, W, aQ) } break } } } if (bd == t) { aR.trigger("pinchStatus", [bb, aJ || null, aq || 0, ab || 0, W, G, aQ]); if (av.pinchStatus) { bc = av.pinchStatus.call(aR, be, bb, aJ || null, aq || 0, ab || 0, W, G, aQ); if (bc === false) { return false } } if (bb == h && a8()) { switch (aJ) { case c: aR.trigger("pinchIn", [aJ || null, aq || 0, ab || 0, W, G, aQ]); if (av.pinchIn) { bc = av.pinchIn.call(aR, be, aJ || null, aq || 0, ab || 0, W, G, aQ) } break; case z: aR.trigger("pinchOut", [aJ || null, aq || 0, ab || 0, W, G, aQ]); if (av.pinchOut) { bc = av.pinchOut.call(aR, be, aJ || null, aq || 0, ab || 0, W, G, aQ) } break } } } if (bd == A) { if (bb === q || bb === h) { clearTimeout(aW); clearTimeout(af); if (Y() && !H()) { N = at(); aW = setTimeout(f.proxy(function () { N = null; aR.trigger("tap", [be.target]); if (av.tap) { bc = av.tap.call(aR, be, be.target) } }, this), av.doubleTapThreshold) } else { N = null; aR.trigger("tap", [be.target]); if (av.tap) { bc = av.tap.call(aR, be, be.target) } } } } else { if (bd == j) { if (bb === q || bb === h) { clearTimeout(aW); N = null; aR.trigger("doubletap", [be.target]); if (av.doubleTap) { bc = av.doubleTap.call(aR, be, be.target) } } } else { if (bd == b) { if (bb === q || bb === h) { clearTimeout(aW); N = null; aR.trigger("longtap", [be.target]); if (av.longTap) { bc = av.longTap.call(aR, be, be.target) } } } } } return bc } function an() { var bb = true; if (av.threshold !== null) { bb = ag >= av.threshold } return bb } function ba() { var bb = false; if (av.cancelThreshold !== null && aP !== null) { bb = (aT(aP) - ag) >= av.cancelThreshold } return bb } function ae() { if (av.pinchThreshold !== null) { return aq >= av.pinchThreshold } return true } function aA() { var bb; if (av.maxTimeThreshold) { if (ab >= av.maxTimeThreshold) { bb = false } else { bb = true } } else { bb = true } return bb } function al(bb, bc) { if (av.preventDefaultEvents === false) { return } if (av.allowPageScroll === m) { bb.preventDefault() } else { var bd = av.allowPageScroll === s; switch (bc) { case p: if ((av.swipeLeft && bd) || (!bd && av.allowPageScroll != D)) { bb.preventDefault() } break; case o: if ((av.swipeRight && bd) || (!bd && av.allowPageScroll != D)) { bb.preventDefault() } break; case e: if ((av.swipeUp && bd) || (!bd && av.allowPageScroll != u)) { bb.preventDefault() } break; case x: if ((av.swipeDown && bd) || (!bd && av.allowPageScroll != u)) { bb.preventDefault() } break } } } function a8() { var bc = aO(); var bb = X(); var bd = ae(); return bc && bb && bd } function aX() { return !!(av.pinchStatus || av.pinchIn || av.pinchOut) } function P() { return !!(a8() && aX()) } function aV() { var be = aA(); var bg = an(); var bd = aO(); var bb = X(); var bc = ba(); var bf = !bc && bb && bd && bg && be; return bf } function V() { return !!(av.swipe || av.swipeStatus || av.swipeLeft || av.swipeRight || av.swipeUp || av.swipeDown) } function I() { return !!(aV() && V()) } function aO() { return ((W === av.fingers || av.fingers === i) || !a) } function X() { return aQ[0].end.x !== 0 } function a6() { return !!(av.tap) } function Y() { return !!(av.doubleTap) } function aU() { return !!(av.longTap) } function Q() { if (N == null) { return false } var bb = at(); return (Y() && ((bb - N) <= av.doubleTapThreshold)) } function H() { return Q() } function ax() { return ((W === 1 || !a) && (isNaN(ag) || ag < av.threshold)) } function a0() { return ((ab > av.longTapThreshold) && (ag < r)) } function ah() { return !!(ax() && a6()) } function aG() { return !!(Q() && Y()) } function ap() { return !!(a0() && aU()) } function F() { a5 = at(); ad = event.touches.length + 1 } function R() { a5 = 0; ad = 0 } function am() { var bb = false; if (a5) { var bc = at() - a5; if (bc <= av.fingerReleaseThreshold) { bb = true } } return bb } function aB() { return !!(aR.data(B + "_intouch") === true) } function ao(bb) { if (bb === true) { aR.bind(ay, a3); aR.bind(U, L); if (S) { aR.bind(S, K) } } else { aR.unbind(ay, a3, false); aR.unbind(U, L, false); if (S) { aR.unbind(S, K, false) } } aR.data(B + "_intouch", bb === true) } function ai(bc, bb) { var bd = bb.identifier !== undefined ? bb.identifier : 0; aQ[bc].identifier = bd; aQ[bc].start.x = aQ[bc].end.x = bb.pageX || bb.clientX; aQ[bc].start.y = aQ[bc].end.y = bb.pageY || bb.clientY; return aQ[bc] } function aH(bb) { var bd = bb.identifier !== undefined ? bb.identifier : 0; var bc = ac(bd); bc.end.x = bb.pageX || bb.clientX; bc.end.y = bb.pageY || bb.clientY; return bc } function ac(bc) { for (var bb = 0; bb < aQ.length; bb++) { if (aQ[bb].identifier == bc) { return aQ[bb] } } } function aj() { var bb = []; for (var bc = 0; bc <= 5; bc++) { bb.push({ start: { x: 0, y: 0 }, end: { x: 0, y: 0 }, identifier: 0 }) } return bb } function aI(bb, bc) { bc = Math.max(bc, aT(bb)); M[bb].distance = bc } function aT(bb) { if (M[bb]) { return M[bb].distance } return undefined } function aa() { var bb = {}; bb[p] = aw(p); bb[o] = aw(o); bb[e] = aw(e); bb[x] = aw(x); return bb } function aw(bb) { return { direction: bb, distance: 0 } } function aM() { return a2 - T } function au(be, bd) { var bc = Math.abs(be.x - bd.x); var bb = Math.abs(be.y - bd.y); return Math.round(Math.sqrt(bc * bc + bb * bb)) } function a7(bb, bc) { var bd = (bc / bb) * 1; return bd.toFixed(2) } function ar() { if (G < 1) { return z } else { return c } } function aS(bc, bb) { return Math.round(Math.sqrt(Math.pow(bb.x - bc.x, 2) + Math.pow(bb.y - bc.y, 2))) } function aE(be, bc) { var bb = be.x - bc.x; var bg = bc.y - be.y; var bd = Math.atan2(bg, bb); var bf = Math.round(bd * 180 / Math.PI); if (bf < 0) { bf = 360 - Math.abs(bf) } return bf } function aL(bc, bb) { var bd = aE(bc, bb); if ((bd <= 45) && (bd >= 0)) { return p } else { if ((bd <= 360) && (bd >= 315)) { return p } else { if ((bd >= 135) && (bd <= 225)) { return o } else { if ((bd > 45) && (bd < 135)) { return x } else { return e } } } } } function at() { var bb = new Date(); return bb.getTime() } function aY(bb) { bb = f(bb); var bd = bb.offset(); var bc = { left: bd.left, right: bd.left + bb.outerWidth(), top: bd.top, bottom: bd.top + bb.outerHeight() }; return bc } function E(bb, bc) { return (bb.x > bc.left && bb.x < bc.right && bb.y > bc.top && bb.y < bc.bottom) } } }));