define("color", function () {
	var $boxbg = $("#menu_box_bg"), $menubox = $("#menu_box");
	
	(function ($) {
		//X向滚动
		$.fn.dragX = function (options) {
			var setting = {
				onstart: null,
				onmove: null,
				onend: null,
			}
			options = Object.extend(options, setting);
			var $this = this;
			$this.X = $this.disX = 0;
			$this.touches({
				touchstart: function (ev) {
					ev.preventDefault();
					$this.disX = ev.targetTouches[0].pageX - $this.X;
					options.onstart && options.onstart.call($this, $this.disX, ev.targetTouches[0].pageX);
				},
				touchmove: function (ev) {
					ev.preventDefault();
					$this.X = ev.targetTouches[0].pageX - $this.disX;
					options.onmove && options.onmove.call($this, $this.X);
				},
				touchend: function (ev) {
					options.onend && options.onend.call($this, $this.X, ev.changedTouches[0].pageX)
				}
			})
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
	})(jQuery);

	$(function () {
		$('.standard_car_pic_1').directionX({
			init: function () {
				var $this = this, imgs = $this.find('img'), $message = $this.next().children(0);
				;
				$this.index = 0;
				$this.on('setIndex', function (event, i) {
					imgs.each(function (index, curr) {
						var $current = $(curr);
						if (index == i) {
							$current.fadeIn();
						} else {
							$current.fadeOut();
						}
					});
					$this.trigger('setMessage', i);
					$this.index = i;
				});

				$this.on('prev', function (event) {
					$this.index = $this.index - 1;
					if ($this.index < 0) {
						$this.index = imgs.length - 1;
					}
					$this.trigger('setIndex', $this.index);
				});

				$this.on('next', function (event) {
					$this.index = $this.index + 1;
					if ($this.index > imgs.length - 1) {
						$this.index = 0;
					}
					$this.trigger('setIndex', $this.index);
				});


				var as = $this.next().next().children();
				$this.on('setMessage', function (event, i) {
					var $a = as.eq(i);
					as.removeClass('current');
					$a.addClass('current');
					$message.html($a.children(0).attr('data-value'));
				}).trigger('setMessage', $this.index);

				as.each(function (index, curr) {
					var $current = $(curr);
					(function ($o, i) {
						$o.on('click', function (ev) {
							var $a = $(this);
							ev.preventDefault();
							$this.trigger('setIndex', i);

						});
					})($current, index);

				});

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
	});

	$boxbg.touches({
		touchstart: function (ev) {
			ev.preventDefault();
			$("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
			$boxbg.stop().fadeOut();

			if ($menubox.attr("class").indexOf("menu_box_hover") > -1) {
				$menubox.removeClass("menu_box_hover").addClass("menu_box_down");
			}
		},
		touchmove: function (ev) {
			ev.preventDefault();
		}
	});

});