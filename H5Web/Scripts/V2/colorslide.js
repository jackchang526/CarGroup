/*默认值*/
Object.extend = function (destination, source) {
	if (!destination) return source;
	for (var property in source) {
		if (!destination[property]) {
			destination[property] = source[property];
		}
	}
	return destination;
};


(function ($) {
	$.fn.onload = function (fnEnd) {
		function e($o, end) {
			var obj = $o[0];
			obj.onerror = function () {
				end && end.call($o, 0);
				obj.onreadystatechange = null;
			}

			obj.onload = function () {
				end && end.call($o, 1);
				obj.onreadystatechange = null;
			}
			obj.onreadystatechange = function (ev) {
				if (obj.readyState == 'complete') {
					end && end.call($o, 1);
					obj.onload = null;
				}
			}
		}

		if (this.length > 0) {

			var i = 0;
			this.each(function (index, curr) {
				var $this = $(curr);
				e($this, function () {
					i++;
					if (i >= this.length) { fnEnd && fnEnd.call($this) }
				});
			})
		} else {
			e(this, fnEnd);
		}
	}

	//图片加载成功状态
	$.fn.imgSucceed = function (options) {
		var setting = {
			end: null
		}
		options = Object.extend(options, setting);
		var $this = this, index = 1;
		var imgs = $this.find('img');
		var len = imgs.length;
		if (len > 0) {
			imgs.each(function (index, curr) {
				var $img = $(curr);
				$img.onload(function () {
					if (index == (len - 1)) {
						options.end && options.end.call($this);
					}
					index++;
				})
			});
		} else {
			options.end && options.end.call($this);
		}
	}
	//触摸屏事件
	$.fn.touches = function (options) {
		var setting = {
			init: null,//初始化
			touchstart: null,  //按下
			touchmove: null, //滑动
			touchend: null //抬起
		};
		options = Object.extend(options, setting)
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

	//手势方向(X轴)
	$.fn.directionX = function (options) {
		var setting = {
			init: null,
			selectfn: null,
			max: 30
		}
		options = Object.extend(options, setting);
		var $this = this;
		if ($this.length == 0) return;
		$this.ox = 0;
		$this.dragX({
			onstart: function (x) {
				$this.ox = 0;
			},
			onmove: function (x) {
				if (x - $this.ox < -options.max) {
					clearTimeout($this.timeout)
					$this.timeout = setTimeout(function () { options.selectfn && options.selectfn.call($this, 'left'); }, 300);

				} else if (x - $this.ox > options.max) {
					clearTimeout($this.timeout)
					$this.timeout = setTimeout(function () { options.selectfn && options.selectfn.call($this, 'right'); }, 300);
				}
				if ($this.ox == 0) {
					$this.ox = x;
				}
			},
			onend: function () {
				$this.ox = 0;
			}
		});
		options.init && options.init.call($this);
	}

	//X向滚动
	$.fn.dragX = function (options) {
		var setting = {
			onstart: null,
			onmove: null,
			onend: null
		}
		options = Object.extend(options, setting);
		var $this = this;
		$this.X = $this.disX = 0;
		$this.disY = 0;
		$this.touches({
			touchstart: function (ev) {
				$this.Y = ev.targetTouches[0].pageY;
				$this.X = ev.targetTouches[0].pageX;

				$this.disX = ev.targetTouches[0].pageX - $this.X;
				options.onstart && options.onstart.call($this, $this.disX, ev.targetTouches[0].pageX);
			},
			touchmove: function (ev) {
				$this.X = ev.targetTouches[0].pageX - $this.X;
				$this.Y = ev.targetTouches[0].pageY - $this.Y;
				//console.log($this.X, $this.Y)
				if (Math.abs($this.X) - Math.abs($this.Y) > 50) {
					ev.preventDefault();
					options.onmove && options.onmove.call($this, $this.X);
				}
			},
			touchend: function (ev) {
				options.onend && options.onend.call($this, $this.X, ev.changedTouches[0].pageX)
			}
		})
	}
})(jQuery);

$(function () {
	$('#standard_car_pic_1').directionX({
		init: function () {
			var $this = this, imgs = $this.find('img'), $message = $("#car_color_text");
			$this.index = 0;



			$this.on('setIndex', function (event, i) {
				imgs.each(function (index, curr) {
					var $current = $(curr);
					if (index == i) { $current.fadeIn(); }
					else {
						$current.fadeOut();
					}
				})
				$this.trigger('setMessage', i);
				$this.index = i;
			})

			$this.on('prev', function (event) {
				$this.index = $this.index - 1;
				if ($this.index < 0) { $this.index = imgs.length - 1; }
				$this.trigger('setIndex', $this.index);
			})

			$this.on('next', function (event) {
				$this.index = $this.index + 1;
				if ($this.index > imgs.length - 1) { $this.index = 0; }
				$this.trigger('setIndex', $this.index);
			})


			var as = $this.next().next().children();
			$this.on('setMessage', function (event, i) {
				var $a = as.eq(i);
				as.removeClass('current');
				$a.addClass('current');
				if ($message.length > 0)
					$message.html($a.children(0).attr('data-value'));
			}).trigger('setMessage', $this.index);

			as.each(function (index, curr) {
				var $current = $(curr);
				(function ($o, i) {
					$o.on('click', function (ev) {
						var $a = $(this);
						ev.preventDefault();
						$this.trigger('setIndex', i);

					})
				})($current, index);

			});
			function resizeTo() {
				$this.imgSucceed({
					end: function () {
						$this.height(imgs.height());
					}
				})

			}


			$(window).resize(resizeTo).trigger('resize');

		},
		selectfn: function (v) {
			var $this = this;
			switch (v) {
				case 'left':
					$this.trigger('next');
					break;
				case 'right':
					$this.trigger('prev');
					break;
			}
		}
	});
})
