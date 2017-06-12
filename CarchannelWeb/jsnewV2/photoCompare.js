(function () {
	var mdvalue = 0, sdvalue = 0, isIE6 = ! -[1, ] && !window.XMLHttpRequest, isIE7 = navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1;

	if (carInfoJson && carInfoJson.length > 0) {
		mdvalue = carInfoJson[carInfoJson.length - 1].MasterId;
		sdvalue = carInfoJson[carInfoJson.length - 1].SerialId;
	}
	//绑定下拉选择框
	BitA.DropDownListNew({
		url: "http://api.car.bitauto.com/CarInfo/masterbrandtoserialforphoto.ashx",
		container: { master: "master4", serial: "serial4", cartype: "cartype4" },
		deftext: { serial: { "value": "0", "text": "请选择车型"} },
		include: { serial: "1", cartype: "1" },
		dvalue: { master: mdvalue, serial: sdvalue },
		datatype: 5,
		callback: {
		     cartype: function(data) {
		          setCarDisabled();
		     }
		},
		onchange: {
		    master: function (data) {
		        $("#serial4").removeClass("brand-disabled");
		    },
		    serial: function (data) {
		        $("#cartype4").removeClass("brand-disabled");
		    },
		    cartype: function() {
		         addCarForSelect();
		    }
		}
	});
	//点击换车效果
	$("a[id^='btn-changecar-']").click(function (e) {
		if ($(this).hasClass("btn-show-car")) {
			var index = $(this).attr("bit-changeindex");
			var carInfo = carInfoJson[index];
			//绑定下拉选择框
			changeBindSelect(index, carInfo.MasterId, carInfo.SerialId, carInfo.CarId);
			//推荐车型
			getHotCarCompare(index, carInfo.SerialId);
		    //显示当前换车的下拉层
			$(this).removeClass("btn-show-car").addClass("btn-hide-car").siblings(".huanche-layer").show();
		    //隐藏另外两个下拉层
			$("div.huanche-layer[bit-popupindex!='" + index + "']").hide();
		    //将另外两个换车按钮样式变为btn-show-car
			$("a[id^='btn-changecar-'][bit-changeindex!='" + index + "']").removeClass("btn-hide-car").addClass("btn-show-car");
		}
		else {
		    $(this).removeClass("btn-hide-car").addClass("btn-show-car").siblings(".huanche-layer").hide();
		}
	});
	//请求 热门推荐车型
	if (carInfoJson && carInfoJson.length < 3) {
		var serialId = "";
		if (carInfoJson.length > 0)
			serialId = carInfoJson[carInfoJson.length - 1].SerialId
		$.ajax({ url: "/ajaxnew/gethotcarforphotocompare.ashx?serialIDs=" + serialId, cache: true, dataType: "json", success: function (data) {
			var arrHtml = [], carCount = 0;
			for (var i = 0; i < data.length; i++) {
				if (checkCarIdExist(data[i].CarId)) continue;
				arrHtml.push("<li>");
				arrHtml.push("<a href=\"javascript:addRemCarCompare(" + data[i].CarId + ");\"><img src=\"" + data[i].SerialImage + "\"></a>");
				arrHtml.push("<p><a href=\"javascript:addRemCarCompare(" + data[i].CarId + ");\">" + data[i].SerialName + "</a></p>");
				arrHtml.push("</li>");
				carCount++;
				if (carCount >= 6) break;
			}
			if (arrHtml.length > 0)
				$("#recommend-box").html("<h5>为您推荐</h5><ul>" + arrHtml.join('') + "</ul>");
		}
		});
	}
	var headerTop = $("#img-compare-header").offset().top,
		boxLeft = $(".container").offset().left,
		key = false;

	$(window).scroll(function () {
		var scrollTop = $(this).scrollTop();
		if (scrollTop > headerTop) {
			if (!key && !isIE6) $("#img-compare-header").before("<div class=\"img-compare-header\" id=\"img-compare-header-placeholder\"></div>");
			key = true;
			if (isIE6) {
				$("#img-compare-header").css({ "position": "relative", top: scrollTop - 205 });
				$(".left-nav").css({ "position": "absolute", top: scrollTop - 60 });
			}
			//else if (isIE7) {
			//    $("#img-compare-header").css({ "position": "relative", top: scrollTop - 355 });
			//    $(".left-nav").css({ "position": "absolute", top: scrollTop - 60 });
			    //} 
			else {
			    $("#img-compare-header").css({ "position": "fixed", top: "0px", width: "1200px", left: "50%", "margin-left": "-600px" });
			    $(".left-nav").css({ "position": "fixed", top: "187px", left: boxLeft - 75 + "px" });
			}
		} else {
			key = false;
			$("#img-compare-header-placeholder").remove();
			$("#img-compare-header").css({ "position": "", top: "", width: "", left: "", "margin-left": "" });
			$(".left-nav").css({ "position": "absolute", top: "187px", left: "-76px" });
		}
	});
})();
//获取推荐车型
function getHotCarCompare(index, serialId) {
	$.ajax({ url: "/ajaxnew/gethotcarforphotocompare.ashx?serialIDs=" + serialId, cache: true, dataType: "json", success: function (data) {
		var arrHtml = [], showCount = 0;

		for (var i = 0; i < data.length; i++) {
			if (checkCarIdExist(data[i].CarId)) continue;
			arrHtml.push("<li><div class=\"txt\">" + data[i].SerialName + "<a href=\"javascript:onchangeCarForSelect(" + index + "," + data[i].CarId + ");\">换车</a></div></li>");
			showCount++;
			if (showCount >= 4) break;
		}
		if (arrHtml.length > 0)
			$("#recom-car-list-" + index).html("<ul>" + arrHtml.join('') + "");
	}
	});
}
//换车 绑定 品牌选择框
function changeBindSelect(index, mdvalue, sdvalue, cardvalue) {
	var masterId = "change-master-" + index,
			 serialId = "change-serial-" + index,
			 carId = "change-car-" + index;

	BitA.DropDownListNew({
		url: "http://api.car.bitauto.com/CarInfo/masterbrandtoserialforphoto.ashx",
		container: { master: masterId, serial: serialId, cartype: carId },
		deftext: { serial: { "value": "0", "text": "请选择车型"} },
		include: { serial: "1", cartype: "1" },
		dvalue: { master: mdvalue, serial: sdvalue, cartype: cardvalue },
		datatype: 5,
		callback: {
		     cartype: function(data) {
		         setCarDisabled();
		         $("#cartypediv .models_detail_bg").css("height", "300px");
		     }
		},
		onchange: {
		    cartype: function() {
		        var carId = $("#change-car-" + index + " span").attr("value");
		        onchangeCarForSelect(index, carId);
		    }
		}
    });
}
//检查 车款是否存在
function checkCarIdExist(carId) {
	var flag = false;
	if (carId > 0 && carInfoJson && carInfoJson.length > 0) {
		for (var i = 0; i < carInfoJson.length; i++) {
			if (carId == carInfoJson[i].CarId) {
				flag = true; break;
			}
		}
	}
	return flag;
}
//添加车款 通过 选择框
function addCarForSelect() {
    var carId = $("#cartype4 span").attr("value");
	addRemCarCompare(carId);
}
//换车 通过选择框
function onchangeCarForSelect(index, carId) {
	if (carId <= 0) return;
	var arrCarIds = [], flag = false;
	if (carInfoJson) {
		for (var i = 0; i < carInfoJson.length; i++) {
			if (carInfoJson[i].CarId == carId) {
				alert("您选择的车型,已经在对比列表中!");
				return;
			}
			arrCarIds.push(carInfoJson[i].CarId)
		}
	}
	if (arrCarIds.length > index) {
		arrCarIds[index] = carId;
	}
	location.href = '/tupianduibi/?carids=' + arrCarIds.join();
}
//关闭 车款
function closeCarCompare(carId) {
	if (carId <= 0) return;
	var arrCarIds = [], flag = false;
	if (carInfoJson && carInfoJson.length > 0) {
		for (var i = 0; i < carInfoJson.length; i++) {
			if (carInfoJson[i].CarId != carId)
				arrCarIds.push(carInfoJson[i].CarId)
		}
		location.href = '/tupianduibi/?carids=' + arrCarIds.join();
	}
}
//添加 车款
function addRemCarCompare(carId) {
	if (carId <= 0) return;
	var arrCarIds = [], flag = false;
	if (carInfoJson) {
		for (var i = 0; i < carInfoJson.length; i++) {
			if (carInfoJson[i].CarId == carId)
			{ flag = true; break; }
			arrCarIds.push(carInfoJson[i].CarId)
		}
	}
	if (!flag) {
		arrCarIds.push(carId);
		location.href = '/tupianduibi/?carids=' + arrCarIds.join();
	}
}


$(function () {
	//添加滚动监听事件
	$('[data-spy="scroll"]').each(function () {
		var $spy = $(this);
		$spy.scrollspy($spy.data(), function () { /*scrollCallback();*/ });
		//$spy.scrollspy("refresh");
	});
	//返回顶部的处理
	$("#backtop").attr("href", "javascript:;").bind("click", function () {
		$("html,body").animate({ scrollTop: 0 }, 300, function () {
			//modified by 2014.07.17 ie6 7 8 修改 滚动监听 回不到基本信息
			if (! -[1, ]) {
				$("#left-nav li:first").addClass("current").siblings().removeClass("current");
			}
		});
	});
	//点击窗口外 隐藏弹层
	$(document).click(function (e) {
		e = e || window.event;
		var target = e.srcElement || e.target;
		if ($(target).closest(".huanche-layer").length <= 0 && $(target).closest("a[id^='btn-changecar']").length <= 0) {
		    $(".huanche-layer").hide();
			$("a[id^='btn-changecar-']").removeClass("btn-hide-car").addClass("btn-show-car");
		}
	});
	//设置不可选车型
	setCarDisabled();
});
function setCarDisabled() {
	if (carInfoJson && carInfoJson.length > 0) {
		for (var i = 0; i < carInfoJson.length; i++) {
		    var carId = carInfoJson[i].CarId;
		    //点击换辆车出现的车款下拉弹层
		    var aTagSmall = $("div[id^='change-car'] a[bita-value='" + carId + "']");
		    var ddTagSmall = $(aTagSmall).parent();
		    var priceSmall = $(aTagSmall).attr("bita-price");
			if (ddTagSmall.length > 0) {
			    $(ddTagSmall).html("<span>" + carInfoJson[i].CarName + "<strong>" + priceSmall + "</strong></span>");
		    }
		    //页面刚加载时的车款下拉弹层
		    var aTagBig = $("div[id='cartype4'] a[bita-value='" + carId + "']");
		    var ddTagBig = $(aTagBig).parent();
		    var priceBig = $(aTagBig).attr("bita-price");
		    if (ddTagBig.length > 0) {
		        $(ddTagBig).html("<span>" + carInfoJson[i].CarName + "<strong>" + priceBig + "</strong></span>");
		    }
		}
	}
}
//滚动监听
!function ($) {

	"use strict";

	function ScrollSpy(element, options) {
		var process = $.proxy(this.process, this)
		  , $element = $(element).is('body') ? $(window) : $(element)
		  , href;
		this.options = $.extend({}, $.fn.scrollspy.defaults, options);
		this.$scrollElement = $element.on('scroll.scroll-spy.data-api', process);
		this.selector = (this.options.target) + ' li > a';
		this.$body = $('body');
		this.refresh();
		this.process();
	}

	ScrollSpy.prototype = {

		constructor: ScrollSpy

	  , refresh: function () {
	  	var self = this,
		$targets,
		scrollTop = self.$scrollElement.scrollTop();

	  	this.offsets = $([]);
	  	this.targets = $([]);

	  	$targets = this.$body
          .find(this.selector)
          .map(function (i, n) {
          	var $el = $(this),
			 targetName = $el.data('target'),
			 targetHeight = $el.height(),
			 $targetElement = $("#" + targetName);
          	if ($targetElement
              && ($targetElement.length > 0)) {
          		var ElementScrollTop = ($targetElement.offset().top + self.options.offset - (i * (targetHeight)))-40;
          		//console.log(targetName + "|" + $targetElement.offset().top + "|" + self.options.offset + "|" + targetHeight);
          		if (scrollTop >= ElementScrollTop) {
          			self.activate(targetName)
          		}
          		$el.unbind("click");
          		$el.bind("click", function (e) {
          			e.preventDefault();
          			$("html,body").animate({ scrollTop: ElementScrollTop + 1 }, 300, function () {
          				if (typeof self.options["callback"] != "undefined") { self.options["callback"](); }
          			});
          		});
          		return ([[ElementScrollTop, targetName]])
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
	  	for (i = offsets.length; i--; ) {
	  		activeTarget != targets[i];
	  		if (scrollTop >= offsets[i] && (!offsets[i + 1] || scrollTop <= offsets[i + 1])) {
	  			this.activate(targets[i])
	  		}
	  	}
	  }

	  , activate: function (target) {

	  	this.activeTarget = target;

	  	$(this.selector)
          .parent('.current')
          .removeClass('current');

	  	var currSelector = this.selector + '[data-target="' + target + '"]';

	  	var active = $(currSelector)
          .parent('li')
          .addClass('current');

	  	active.trigger('activate');
	  }

	}

	var old = $.fn.scrollspy

	$.fn.scrollspy = function (option, callbackFunc) {
		if (callbackFunc && callbackFunc instanceof Function) option["callback"] = callbackFunc;
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
} (jQuery);