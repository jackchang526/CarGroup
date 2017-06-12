Object.extend = function (destination, source) {
	if (!destination) return source;
	for (var property in source) {
		if (!destination[property]) {
			destination[property] = source[property];
		}
	}
	return destination;
};

function fullPageExtend(options) {
	var setting = {
		$page: null,
		inc: '',
		anchorLink: 'page1',
		index: 1
	}
	options = Object.extend(options, setting);
	this.inc = options.inc;
	this.anchorLink = options.anchorLink;
	this.index = options.index;
	this.$page = options.$page;
}

fullPageExtend.prototype.getSrc = function () {
	var src = this.$page.attr("data-src");
	if (src.indexOf("http://") > -1)
		return src;
	return [this.inc, '/', this.$page.attr('data-src')].join('');
}

fullPageExtend.prototype.isLoadedPage = function () {
	var $frame = this.$page.find('iframe');
	return $frame.length > 0;
}

fullPageExtend.prototype.createFrame = function (src) {
	var $iframe = $('<iframe>');
	$iframe.attr('src', src)
        .attr('id', 'iframe_' + this.anchorLink)
        .attr('width', '100%')
        .height(document.documentElement.clientHeight);
	this.$page[0].appendChild($iframe[0]);
};


(function ($) {
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
})(jQuery);