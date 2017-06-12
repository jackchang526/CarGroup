Object.extend = function (destination, source) {
	if (!destination) return source;
	for (var property in source) {
		if (!destination[property]) {
			destination[property] = source[property];
		}
	}
	return destination;
};

function fullPageChildrenExtend(options) {
	var setting = {

	}
	options = Object.extend(options, setting);
	this.$context_scroll = options.$context_scroll;
}

fullPageChildrenExtend.prototype.init = function (options) {
	var setting = {
		touchstart: null
	}
	options = Object.extend(options, setting);
	var _this = this;
	_this.$context_scroll.scrollbox = _this.$context_scroll.children(0);
	_this.$context_scroll.$section = $('[data-name=' + _this.$context_scroll.attr('data-name') + ']', parent.document);
	_this.$context_scroll.oY = 0;
	var _clientHeight = document.documentElement.clientHeight;
	_this.$context_scroll.touches({
		touchstart: function (ev) {
			_this.$context_scroll.disY = ev.targetTouches[0].pageY - _this.$context_scroll.oY;
			_this.$context_scroll.site = '';
		},
		touchmove: function (ev) {
			var y = ev.targetTouches[0].pageY - _this.$context_scroll.disY;
			if (_this.$context_scroll.oY < y) {
				_this.$context_scroll.site = 'down';
			} else if (_this.$context_scroll.oY > y) {
				_this.$context_scroll.site = 'up';
			}
			clearTimeout(_this.$context_scroll.timeout);
			_this.$context_scroll.timeout = setTimeout(function () {
				var scrollTop = _this.$context_scroll[0].scrollTop || document.body.scrollTop;
				switch (_this.$context_scroll.site) {
					case 'up':
						if (scrollTop >= _this.$context_scroll.scrollbox.height() - document.documentElement.clientHeight - 5) {
							parent.next();
						}
						break;
					case 'down':
						if (scrollTop <= 5) {
							parent.prev();
						}
						break;
				}
			}, 300);
			_this.$context_scroll.oY = y;
		}
	});

	function toResize() {
		var $fullpage = $('#fullpage', parent.document);
		_this.$context_scroll.width($fullpage.width());
		_this.$context_scroll.height($fullpage.height() - 20);
	}

	$(parent.document).ready(function () { $(parent.window).resize(toResize).trigger('resize'); })
};

var classNames = ['Webkit', 'ms', 'Moz', 'O', ''];
var eventNames = ['webkit', 'moz', 'o'];
(function ($) {
	//添加事件
	$.fn.addEvent = function (name, fn) {
		var obj = this[0];
		var cName = name.charAt(0).toUpperCase() + name.substring(1);
		for (var i = 0; i < eventNames.length; i++) {
			obj.addEventListener(eventNames[i] + cName, fn, false);
		}
		obj.addEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
	}

	//删除事件
	$.fn.removeEvent = function (name, fn) {
		var obj = this[0];
		var cName = name.charAt(0).toUpperCase() + name.substring(1);
		for (var i = 0; i < eventNames.length; i++) {
			obj.removeEventListener(eventNames[i] + cName, fn, false);
		}
		obj.removeEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
	}

	//触摸屏事件
	$.fn.touches = function (options) {
		var setting = {
			init: null,//初始化
			touchstart: null,  //按下
			touchmove: null, //滑动
			touchend: null //抬起
		};
		options = Object.extend(options, setting);
		var $this = this, touchesDiv = $this[0];
		touchesDiv.addEventListener('touchstart', function (ev) {
			options.touchstart && options.touchstart.call($this, ev);
			function fnMove(ev) {
				options.touchmove && options.touchmove.call($this, ev);
			}

			function fnEnd(ev) {
				options.touchend && options.touchend.call($this, ev);
				document.removeEventListener('touchmove', fnMove, false);
				document.removeEventListener('touchend', fnEnd, false);
			}
			document.addEventListener('touchmove', fnMove, false);
			document.addEventListener('touchend', fnEnd, false);
			return false;
		}, false)
		options.init && options.init.call($this);
	}

	//transition事件监听
	$.fn.transitionEnd = function (options) {
		var setting = {
			listen: 'TransitionEnd',
			end: null
		}
		options = Object.extend(options, setting);
		var $this = this;
		function seatTransitionEnd() {
			for (var i = 0; i < eventNames.length; i++) {
				if (eventNames[i] == 'moz') {
					$this.removeEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
				} else {
					$this.removeEvent(eventNames[i] + options.listen, seatTransitionEnd);
				}
			}
			options.end && options.end.call($this);
		}
		for (var i = 0; i < eventNames.length; i++) {
			if (eventNames[i] == 'moz') {
				$this.addEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
			} else {
				$this.addEvent(eventNames[i] + options.listen, seatTransitionEnd);
			}
		}
	}

	//右侧弹出层
	$.fn.rightSwipeAction = function (options) {
		var setting = {
			show: 'swipeLeft-block',
			clickEnd: null
		};
		options = Object.extend(options, setting);
		var $child = $(this.children(1)), display = 'none';
		if ($child.hasClass(options.show)) {
			display = 'none';
		} else {
			display = 'block';
		}
		options.clickEnd && options.clickEnd.call($child, display);
	};

	//右侧附加选择层插件
	$.fn.rightSwipe = function (options) {
		var $temp = null;
		var setting = {
			isclick: null,
			zindex: 999999,
			back: '.leftmask',
			alert: '.leftPopup',
			clickEnd: null //打开关闭层回调事件
		};
		options = Object.extend(options, setting);
		this.each(function (index, curr) {
			var $curr = $(curr);
			(function ($this) {
				$this.isclick = true;
				$this.click(function (ev) {
					ev.preventDefault();
					options.isclick && ($this.isclick = options.isclick.call($this));
					if ($this.isclick == false) {
						return;
					}
					var $leftPopup = $('.leftPopup.' + $this.attr('data-action'));
					$leftPopup[0].style.zIndex = options.zindex;
					$leftPopup.rightSwipeAction({
						clickEnd: function (display) {
							var $back = $('.' + $leftPopup.attr('data-back')),
                                $swipeLeft = $leftPopup.find('.swipeLeft');
							$back.css('z-index', options.zindex - 10000);
							$back.show();
							$leftPopup.show();
							setTimeout(function () { $swipeLeft.addClass('swipeLeft-block'); }, 200);
							$back.parents('body').css('overflow', 'hidden');
							$back.on('close', function () {
								$(options.back).each(function (index, curr) {
									curr.style.display = 'none';
								})
								var $alert = $(options.alert).children().removeClass('swipeLeft-block').end();
								$back.parents('body').css('overflow', 'auto');
								setTimeout(function () { $alert.css('z-index', 0).hide(); }, 200);
							})

							$back.touches({
								touchstart: function () {
									$back.trigger('close');
								}
							})
							$swipeLeft.transitionEnd({ end: function () { options.clickEnd && options.clickEnd.call($leftPopup, true); } })
						}
					});
				})
			})($curr);
		})
	}
})(jQuery);
