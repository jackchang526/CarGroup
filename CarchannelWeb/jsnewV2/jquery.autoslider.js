(function ($) {
	$.fn.autoslider = function (options) {
		var settings = {
			affect: 'scrollx', //效果  有scrollx|scrolly|fade|none
			speed: 500, //动画速度
			space: 5000, //时间间隔
			auto: false, //自动滚动
			trigger: 'click', //触发事件 注意用mouseover代替hover
			conbox: 'ul', //内容容器id或class
			ctag: 'li', //内容标签 默认为<a>
			switcher: '.switcher', //切换触发器id或class
			stag: 'a', //切换器标签 默认为a
			current: 'cur', //当前切换器样式名称
			rand: false, //是否随机指定默认幻灯图片
			bindleftright: true,//是否绑定左右按钮事件
			btnleft: ".focus_left",//向左按钮
			btnright: ".focus_right"//向右按钮
		};
		settings = $.extend({}, settings, options);
		var index = 1;
		var last_index = 0;
		var $conbox = $(this).find(settings.conbox), $contents = $conbox.find(settings.ctag);
		var $switcher = $(this).find(settings.switcher), $stag = $switcher.find(settings.stag);
		if (settings.rand) { index = Math.floor(Math.random() * $contents.length); slide(); }
		if (settings.affect == 'fade') {
			$.each($contents, function (k, v) {
				(k === 0) ? $(this).css({ 'position': 'absolute', 'z-index': 9 }) : $(this).css({ 'position': 'absolute', 'z-index': 1, 'opacity': 0 });
			});
		}
		function slide() {
			if (index >= $contents.length) index = 0;
			$stag.removeClass(settings.current).eq(index).addClass(settings.current);
			switch (settings.affect) {
				case 'scrollx':
					$conbox.width($contents.length * $contents.width());
					$conbox.stop().animate({ left: -$contents.width() * index }, settings.speed);
					break;
				case 'scrolly':
					$contents.css({ display: 'block' });
					$conbox.stop().animate({ top: -$contents.height() * index + 'px' }, settings.speed);
					break;
				case 'fade':
					$contents.eq(last_index).stop().animate({ 'opacity': 0 }, settings.speed / 2).css('z-index', 1)
							 .end()
							 .eq(index).css('z-index', 9).stop().animate({ 'opacity': 1 }, settings.speed / 2)
					break;
				case 'none':
					$contents.hide().eq(index).show();
					break;
			}
			last_index = index;
			index++;
		};
		if (settings.auto) var Timer = setInterval(slide, settings.space);
		$stag.bind(settings.trigger, function () {
			_pause()
			index = $(this).index();
			slide();
			_continue()
		});
		//向右按钮点击
		$(settings.btnright).bind(settings.trigger, function () {
			_pause();
			slide();
			if (index > 1) {
				$(settings.btnleft).removeClass("focus_left_span");
			} else if (index == 1) {
				$(settings.btnleft).addClass("focus_left_span");
			}
			_continue();
		});
		//向左按钮点击
		$(settings.btnleft).bind(settings.trigger, function () {
			_pause();
			index -= 2;
			if (index <= 0) {
				index = 0;
				$(this).addClass("focus_left_span");
			}
			//index = index <= 0 ? 0 : index;
			slide();
			_continue();
		});
		//只有一张图片 左右按钮样式调整
		if ($contents.length <= 1) {
			$(settings.btnleft).addClass("focus_left_span");
			$(settings.btnright).addClass("focus_right_span");
		}

		$conbox.hover(_pause, _continue);
		function _pause() {
			clearInterval(Timer);
		}
		function _continue() {
			if (settings.auto) Timer = setInterval(slide, settings.space);
		}
	}
})(jQuery);