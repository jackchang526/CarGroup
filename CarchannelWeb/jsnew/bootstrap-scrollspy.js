!function ($) {

	"use strict";

	function ScrollSpy(element, options) {
		var process = $.proxy(this.process, this)
		  , $element = $(element).is('body') ? $(window) : $(element)
		  , href
		this.options = $.extend({}, $.fn.scrollspy.defaults, options)
		this.$scrollElement = $element.on('scroll.scroll-spy.data-api', process)
		this.selector = (this.options.target) + ' li > a'
		this.$body = $('body')
		this.refresh()
		this.process()
	}

	ScrollSpy.prototype = {

		constructor: ScrollSpy

	  , refresh: function () {
	  	var self = this
          , $targets

	  	this.offsets = $([])
	  	this.targets = $([])

	  	$targets = this.$body
          .find(this.selector)
          .map(function (i, n) {
          	var $el = $(this),
			 targetName = $el.data('target'),
			 targetHeight = $el.height(),
			 $targetElement = $("#" + targetName);

          	$el.bind("click", function () {
          		$("html,body").animate({ scrollTop: ($targetElement.offset().top + self.options.offset - (i * (targetHeight + 6)) + 1) }, 1000);
          	});
          	//console.log($el.offset().top);
          	if ($targetElement
              && ($targetElement.length > 0)) {
          		return ([[$targetElement.offset().top + self.options.offset - (i * (targetHeight + 6)), targetName]])
          	} else
          		return null;
          })
          .sort(function (a, b) { return a[0] - b[0] })
          .each(function () {
          	self.offsets.push(this[0])
          	self.targets.push(this[1])
          });
	  }

	  , process: function () {
	  	var scrollTop = this.$scrollElement.scrollTop()
          , scrollHeight = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight
          , maxScroll = scrollHeight - this.$scrollElement.height()
          , offsets = this.offsets
          , targets = this.targets
          , activeTarget = this.activeTarget
          , i;
	  	if (scrollTop >= maxScroll) {
	  		return activeTarget != (i = targets.last()[0])
			  && this.activate(i)
	  	}
	  	//console.log(scrollTop);
	  	for (i = offsets.length; i--; ) {
	  		activeTarget != targets[i];
	  		if (scrollTop >= offsets[i] && (!offsets[i + 1] || scrollTop <= offsets[i + 1])) {
	  			this.activate(targets[i])
	  		}
	  	}
	  }

	  , activate: function (target) {
	  	var active
          , selector

	  	this.activeTarget = target

	  	$(this.selector)
          .parent('.current')
          .removeClass('current')

	  	selector = this.selector
          + '[data-target="' + target + '"],'
          + this.selector + '[href="' + target + '"]';

	  	active = $(selector)
          .parent('li')
          .addClass('current')

	  	//if (active.parent('.dropdown-menu').length)  {
	  	//  active = active.closest('li.dropdown').addClass('active')
	  	//}

	  	active.trigger('activate')
	  }

	}

	var old = $.fn.scrollspy

	$.fn.scrollspy = function (option) {
		return this.each(function () {
			var $this = $(this)
			  , data = $this.data('scrollspy')
			  , options = typeof option == 'object' && option
			if (!data) $this.data('scrollspy', (data = new ScrollSpy(this, options)))
			if (typeof option == 'string') data[option]()
		})
	}

	$.fn.scrollspy.Constructor = ScrollSpy

	$.fn.scrollspy.defaults = {
		offset: 0,
		offsetList: 0
	}


	$.fn.scrollspy.noConflict = function () {
		$.fn.scrollspy = old
		return this
	}


	$(function () {
		$('[data-spy="scroll"]').each(function () {
			var $spy = $(this)
			$spy.scrollspy($spy.data())
		})
	})

} (window.jQuery);