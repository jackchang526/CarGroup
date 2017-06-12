Array.prototype.indexOf = function (value) {
	for (var i = 0, l = this.length; i < l; i++)
		if (this[i] == value) return i;
	return -1;
}
Array.prototype.remove = function (b) {
	var a = this.indexOf(b);
	if (a >= 0) {
		this.splice(a, 1);
		return true;
	}
	return false;
};

var WaitCompare = (function (module) {
	var self = module,
        compareData = [],
		arrSelectCarId = [],
        defaults = {
        	count: 4,
        	cookieName: "m_comparecarlist",
        	url: "/handlers/getcarinfoforcompare.ashx?carid=",        	selector: "a[id^='car-compare']",//绑定所有点击事件        	oneSelector: "#car-compare-",//绑定单个点击事件        	bind: function () {
        		//绑定时间
        		$(defaults.selector).off("click.addCompare").on("click.addCompare", function () {
        			var carId = $(this).data("id"), carName = $(this).data("name");        			WaitCompare.addCompareCar(carId, carName, $(this));
        		});
        	},        	selectedFunc: function (carId) {
        		//已添加的样式修改
        		$(defaults.oneSelector + carId).html("已加入").off("click").closest("li").addClass("btn-gray");
        	},        	delFunc: function (carId) {
        		//删除对比的调整
        		$(defaults.oneSelector + carId).html("加入对比").parent().removeClass("btn-gray");        		$(defaults.oneSelector + carId).off("click.addCompare").on("click.addCompare", function () {
        			var carId = $(this).data("id"), carName = $(this).data("name");        			WaitCompare.addCompareCar(carId, carName, $(this));
        		});
        	},        	clearFunc: function () {
        		//清空对比数据
        		$(defaults.selector).off("click.addCompare").html("加入对比").on("click.addCompare", function () {
        			var carId = $(this).data("id"), carName = $(this).data("name");        			WaitCompare.addCompareCar(carId, carName, $(this));
        		}).parent().removeClass("btn-gray");
        	},        	selectCarIdFunc: function (id, name) {
        		//选择车款的事件
        		var $addElem = $(defaults.oneSelector + id);        		WaitCompare.addCompareCar(id, name, $addElem);        		$("#container").show();        		$("#sel-container").hide();
        	}
        };
	//添加对比
	module.addCompareCar = function (id, name, elem) {
		var cookieCar = CookieHelper.getCookie(defaults.cookieName),
            arrCarId = [];
		if (cookieCar) {
			arrCarId = cookieCar.split('|');
		}

		if (arrCarId.length >= 4) {
			//最懂4款
			alert("最多添加4款车");
			return;
		}
		if (arrCarId.indexOf(id) != -1) {
			//已存在
			alert("添加车款已经存在");
			return;
		}
		arrCarId.push(id);

		CookieHelper.setCookie(defaults.cookieName, arrCarId.join('|'));
		compareData.push({
			CarId: id,
			CarName: name
		});
		drawUI(compareData);
		self.updateCount();
	};
	//清空对比
	module.clearCompareCarAll = function () {
		CookieHelper.clearCookie(defaults.cookieName);
		compareData = [];
		drawUI(compareData);
		self.updateCount();
		rightSwipe();
		if (defaults.clearFunc && defaults.clearFunc instanceof Function) {
			defaults.clearFunc();
		}
	};
	//删除对比车款
	module.delCompareCar = function (carId) {
		var cookieCar = CookieHelper.getCookie(defaults.cookieName),
            arrCarId = [],
            newCompareData = [];
		if (cookieCar) {
			arrCarId = cookieCar.split('|');
		}

		arrCarId.remove(carId);

		CookieHelper.setCookie(defaults.cookieName, arrCarId.join('|'));

		for (var i = 0; i < compareData.length; i++) {
			if (compareData[i].CarId == carId) continue;
			newCompareData.push(compareData[i]);

		};
		compareData = newCompareData;
		drawUI(newCompareData);
		self.updateCount();
		rightSwipe();
		if (defaults.delFunc && defaults.delFunc instanceof Function) {
			defaults.delFunc(carId);
		}
	};
	//开始对比
	module.submitCompare = function () {
		var cookieCar = CookieHelper.getCookie(defaults.cookieName),
			arrCarId = [];
		if (cookieCar) {
			arrCarId = cookieCar.split('|');
		}
		if (arrCarId.length < 1) {
			//请添加车款
			alert("至少选择1款车对比");
			return;
		}
		location.href = '/chexingduibi/?carids=' + arrCarId.join(',');
	}
	//初始化 对比数据
	module.initCompreData = function (options) {
		$.extend(true, defaults, options);

		var cookieCar = CookieHelper.getCookie(defaults.cookieName),
			arrCarId = [];
		if (cookieCar) {
			arrCarId = cookieCar.split('|');
		}
		if (arrCarId.length > 0) {
			getData(defaults.url + arrCarId.join(','), function (data) {
				if (data && data.length > 0) {
					compareData.length = 0;
					for (i = 0; i < (data.length) ; i++) {
						compareData.push({
							CarId: data[i].CarId,
							CarName: data[i].CarName
						});
					}
				}
				drawUI(data);
				self.updateCount();
			});
		} else {
			drawUI();
			self.updateCount();
		}
		rightSwipe();
		//bindInitEvent();
		if (defaults.bind && defaults.bind instanceof Function) {
			defaults.bind();
		}
	};
	//更新pk数量
	module.updateCount = function () {
		$("#compare-pk i").html(compareData.length);
	};
	//初始化绑定事件
	var bindInitEvent = function () {
		$(".btn-comparison").bind("click", function () {
			self.submitCompare();
		});

		$(".btn-clear").bind("click", function () {
			self.clearCompareCarAll();
		});
	};
	//组织DOM
	var drawUI = function (data) {
		var h = [],
            currentCount = 0, i = 0;
		arrSelectCarId.length = 0;
		if (data && data.length > 0) {
			for (i = 0; i < (data.length) ; i++) {
				h.push("<li><div class=\"line-box\"><a href=\"/m" + data[i].CarId + "/\">" + data[i].CarName + "</a><a href=\"javascript:;\" data-carid=\"" + data[i].CarId + "\" class=\"btn-close\"><i></i></a></div></li>");

				arrSelectCarId.push(data[i].CarId);

				if (defaults.selectedFunc && defaults.selectedFunc instanceof Function) {
					defaults.selectedFunc(data[i].CarId);
				}
			}
		}
		for (var j = 0; j < defaults.count - i; j++) {
			h.push("<li class=\"add\"><div class=\"line-box\"><a href=\"javascript:;\">添加对比车款</a></div></li>");
		}
		h.push("<li class=\"alert\">最多对比4个车款</li>");
		$(".car .first-list").html(h.join(''));

		bindEvent();
	};
	//绑定事件
	var bindEvent = function () {

		//$(".first-list .add a").on("click", function () {
		//	SelectCar.addCompare({ serialId: defaults.serialid, callbackFunc: defaults.selectCarIdFunc, arrCarId: arrSelectCarId });
		//});
		$(".first-list .add a").each(function (index, curr) {
			(function ($o) {
				$o.touches({
					touchstart: function () {
						$("body").css('overflow', 'inherit');
						SelectCar.addCompare({ serialId: defaults.serialid, callbackFunc: defaults.selectCarIdFunc, arrCarId: arrSelectCarId });
					}
				});
			})($(curr));
		})

		//$(".first-list li .btn-close").on("click", function () {
		//	var carId = $(this).data("carid");
		//	self.delCompareCar(carId);
		//	//$(this).closest("li").hide();
		//});

		$(".first-list li .btn-close").each(function (index, curr) {
			(function ($o) {
				$o.touches({
					touchstart: function () {
						var carId = $o.data("carid");
						self.delCompareCar(carId);
						//$(this).closest("li").hide();
					}
				});
			})($(curr));
		})

		if (compareData.length <= 0) {
			$(".btn-comparison").off("click").addClass("disable");
			$(".btn-clear").off("click").addClass("disable");
		} else {
			$(".btn-comparison").off("click").removeClass("disable").bind("click", function (e) {
				e.preventDefault();
				self.submitCompare();
			});

			$(".btn-clear").off("click").removeClass("disable").bind("click", function (e) {
				e.preventDefault();
				self.clearCompareCarAll();
			});
		}
	};
	//右侧弹出效果
	var rightSwipe = function () {
		var $car = $('[data-action=car]');
		$car.rightSwipe({
			clickEnd: function (b) {
				var $leftPopup = this;
				if (b) {
					var $back = $('.' + $leftPopup.attr('data-back'))
					$back.touches({
						touchstart: function (ev) {
							ev.preventDefault();
						},
						touchmove: function (ev) {
							ev.preventDefault();
						}
					});

					var $swipeLeft = $leftPopup.find('.swipeLeft');
					$car.$y2015 = $swipeLeft.find('.y2015-car-01');


					function resizeTo() {
						//alert(document.documentElement.clientHeight);
						$car.$y2015.height(document.documentElement.clientHeight - $swipeLeft.find('.swipeLeft-header').height());
					}
					$(window).resize(resizeTo).trigger('resize');

					$car.$y2015.touches({
						touchstart: function (ev) {
							ev.preventDefault();
						},
						touchmove: function (ev) {
							ev.preventDefault();
						}
					});

					$swipeLeft.find('.swipeLeft-header').show();
				}
			}
		});

	};

	var getData = function (url, callbackFunc, sync) {
		$.ajax({
			url: url,
			cache: true,
			async: (sync ? false : true),
			//dataType: "jsonp",
			//jsonpCallback: "getData",
			success: function (data) {
				var json = $.parseJSON(data);
				callbackFunc(json);
			}
		});
	};
	return module;
})(WaitCompare || {});

Object.extend = function (destination, source) {
	if (!destination) return source;
	for (var property in source) {
		if (!destination[property]) {
			destination[property] = source[property];
		}
	}
	return destination;
}

var CookieHelper = (function (module) {
	var self = module,
        defaults = {
        	domain: "car.m.yiche.com",
        	expires: 30 * 30,
        	path: "/"
        };
	// module.setCookie = function(name, value, expires, path, domain, secure) {
	//     expiryday = new Date();
	//     expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
	//     document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
	//         ((path) ? "; path=" + path : "; path=/") +
	//         "; domain=" + defaults.domoin + "" +
	//         ((secure) ? "; secure" : "");
	// };
	module.setCookie = function (name, value, options) {
		Object.extend(defaults, options);

		if (typeof value != 'undefined') { // name and value given, set cookie
			options = options || {};
			if (value === null) {
				value = '';
				options.expires = -1;
			}
			var expires = '';
			if (defaults.expires && (typeof defaults.expires == 'number' || defaults.expires.toUTCString)) {
				var date;
				if (typeof defaults.expires == 'number') {
					date = new Date();
					date.setTime(date.getTime() + (defaults.expires * 24 * 60 * 60 * 1000));
				} else {
					date = defaults.expires;
				}
				expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
			}
			var path = defaults.path ? '; path=' + defaults.path : '';
			var domain = defaults.domain ? '; domain=' + defaults.domain : '';
			var secure = defaults.secure ? '; secure' : '';
			document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
		}
	};
	module.getCookie = function (name) {
		var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

		if (arr != null) {
			return unescape(arr[2]);
		}
		return null;
	};
	module.clearCookie = function (name, path, domain) {
		if (self.getCookie(name)) {
			document.cookie = name + "=" +
                ((path) ? "; path=" + path : "; path=/") +
               (defaults.domain ? '; domain=' + defaults.domain : '') +
                ";expires=Fri, 02-Jan-1970 00:00:00 GMT";
		}
	}
	return module;
})(CookieHelper || {});
