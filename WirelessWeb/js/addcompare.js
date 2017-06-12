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

var SelectCar = (function (module) {
	var self = module,
        defaults = {
        	serialId: 0,
        	currentSerialName: "",
        	currentCarId: null,
        	cacheData: {},
        	callbackFunc: null,
        	footerCallback: null,
        	arrCarId: []
        };

	module.initHistory = function () {
		getHistroy();

		var cookieName = "m_comparecarlist",
            url = "/handlers/getcarinfoforcompare.ashx?carid=",
            arrCarId = [],
            h = [];

		var cookieCar = getCookie(cookieName);
		if (cookieCar) {
			arrCarId = cookieCar.split('|');
			//获取对比数据
			self.getLocalData(url + arrCarId.join(','), function (data) {
				if (!(data && data.length > 0)) return;
				for (var i = 0; i < data.length; i++) {
					h.push("            <li id=\"history-" + data[i].CarId + "\">");
					h.push("                <a href=\"javascript:;\" data-id=\"" + data[i].CarId + "\" data-name=\"" + data[i].CarName + "\">");
					h.push("                    <h4>" + data[i].CarName + "</h4>");
					h.push("                    <p><strong>" + data[i].CarReferPrice + "万</strong></p>");
					h.push("                </a>");
					h.push("            </li>");
				};
				$("#history-carlist").html(h.join(''));

				//已选择车型
				for (var i = 0; i < defaults.arrCarId.length; i++) {
					if (defaults.currentCarId && defaults.currentCarId > 0 && defaults.arrCarId[i] == defaults.currentCarId) {
						$("#history-" + defaults.arrCarId[i]).attr("class", "blue").find("h4").prepend("[当前]");
					} else {
						$("#history-" + defaults.arrCarId[i]).attr("class", "none").find("h4").prepend("[已添加]");
					}
				}
				$("#history-carlist").find("li:not(li[class]) a").on("click", function () {
					var carId = $(this).data("id"), carName = $(this).data("name");
					if (defaults.callbackFunc) {
						defaults.callbackFunc(carId, carName, defaults.currentIndex);
					}
				});
				if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
					defaults.footerCallback();
				}
			});
		} else {
			$("#history-carlist").attr("class", "none-block").html("<p>您还未对比过任何车款</p>");
		}

		//$("#history-carlist").html(h.join(''));
	}

	module.addCompare = function (options) {
		defaults.arrCarId.length = 0;
		$.extend(true, defaults, options);

		$("#container").hide();
		$("#sel-container").show();
		if (defaults.serialId > 0) {
			self.getData("http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid=" + defaults.serialId, function (data) {
				getCurrentSerial(data)
				self.initHistory();
				//$("#container").hide();
				//$("#sel-container").show();
				$("#sel-cur-serial").show().siblings().hide();
			});
		} else {
			reqMaster();
			self.initHistory();
		}
		document.documentElement.scrollTop = 0;
		document.body.scrollTop = 0;
	}

	module.init = function (options) {
		$.extend(true, defaults, options);
		if (defaults.serialId > 0) {
			self.getData("http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid=" + defaults.serialId, function (data) {
				getCurrentSerial(data)
				self.initHistory();
				//$("#container").hide();
				//$("#sel-container").show();
				//$("#sel-cur-serial").show().siblings().hide();
			});
		} else {
			reqMaster();
			self.initHistory();
		}

	}

	var reqMaster = function () {
		$("#sel-cur-serial").hide();
		$("#sel-master").show().siblings().hide();
		self.getData("http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=master", getMaster);
	};

	var getCurrentSerial = function (data) {
		defaults["currentSerialName"] = data["SerialName"];
		var h = [];
		h.push("<div class=\"b-return\">");
		h.push("        <a href=\"javascript:;\" id=\"btn-return-current\" class=\"btn-return\">返回</a>");
		h.push("        <span>选择车款</span>");
		h.push("    </div>");
		h.push("    <div class=\"brandfilter\">");
		h.push("        <div class=\"first-tags colums-3\">");
		h.push("            <ul>");
		h.push("                <li data-action=\"serial\" class=\"current\"><a href=\"javascript:;\"><span>" + data["SerialName"] + "</span></a></li>");
		h.push("                <li data-action=\"master\"><a href=\"javascript:;\"><span>按品牌查找</span></a></li>");
		h.push("                <li data-action=\"history\"><a href=\"javascript:;\"><span>历史记录</span></a></li>");
		h.push("            </ul>");
		h.push("        </div>");
		for (var year in data.CarList) {
			h.push("<div class=\"tt-small y2015\">");
			h.push("    <span>" + year.replace('s', '') + "款</span>");
			h.push("</div>");
			h.push("<div class=\"pic-txt-h pic-txt-9060 y2015\">");
			h.push("    <ul>");
			var list = data.CarList[year]
			for (var j = 0; j < list.length; j++) {
				//h.push("        <li class=\"hot\">");
				h.push("        <li id=\"li_" + list[j].CarId + "\">");
				h.push("            <a href=\"javascript:;\" data-id=\"" + list[j].CarId + "\" data-name=\"" + defaults["currentSerialName"] + " " + list[j].CarName + "\">");
				h.push("                <h4>" + list[j].CarName + "</h4>");
				h.push("                <p><strong>" + list[j].ReferPrice + "万</strong></p>");
				h.push("            </a>");
				h.push("        </li>");
			}
			h.push("    </ul>");
			h.push("</div>");
		}
		h.push("    </div>");
		$("#sel-cur-serial").html(h.join("")).show();

		$("#sel-history .first-tags li").eq(0).find("span").html(data["Name"]);
		//已选择车型
		for (var i = 0; i < defaults.arrCarId.length; i++) {
			if (defaults.currentCarId && defaults.currentCarId > 0 && defaults.arrCarId[i] == defaults.currentCarId) {
				$("#li_" + defaults.arrCarId[i]).attr("class", "current").find("h4").prepend("[当前]");
			}
			else {
				$("#li_" + defaults.arrCarId[i]).attr("class", "none").find("h4").prepend("[已添加]");
			}
		}
		$("#sel-cur-serial .pic-txt-h.pic-txt-9060").find("li:not(li[class]) a").on("click", function () {
			var carId = $(this).data("id"), carName = $(this).data("name");
			if (defaults.callbackFunc) {
				defaults.callbackFunc(carId, carName, defaults.currentIndex);
			}
		});

		$("#btn-return-current").on("click", function () {
			$("#container").show();
			$("#sel-container").hide();
		});

		if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
			defaults.footerCallback();
		}

		bindTagEvent("#sel-cur-serial .first-tags li", reqMaster);

		document.documentElement.scrollTop = 0;
		document.body.scrollTop = 0;
	};

	var getMaster = function (data) {
		var h = [];
		h.push("<div class=\"b-return\">");
		h.push("    <a href=\"javascript:;\" id=\"btn-return-master\" class=\"btn-return\">返回</a>");
		h.push("    <span>选择车款</span>");
		h.push("</div>");
		h.push("<div class=\"brandfilter\">");
		if (defaults["currentSerialName"] == "") {
			h.push("<div class=\"first-tags colums-2\">");
		}
		else {
			h.push("<div class=\"first-tags colums-3\">");
		}
		h.push("    <ul>");
		if (defaults["currentSerialName"] != "") {
			h.push("        <li data-action=\"serial\"><a href=\"javascript:;\"><span>" + defaults["currentSerialName"] + "</span></a></li>");
		}
		h.push("        <li data-action=\"master\" class=\"current\"><a href=\"javascript:;\"><span>按品牌查找</span></a></li>");
		h.push("        <li data-action=\"history\"><a href=\"javascript:;\"><span>历史记录</span></a></li>");
		h.push("    </ul>");
		h.push("</div>");
		h.push("<div class=\"brand-list bybrand_list\">");
		h.push("    <div class=\"tt-small phone-title\" data-key=\"#\">");
		h.push("        <span>推荐</span>");
		h.push("    </div>");
		h.push("    <div class=\"brand-hot\">");
		h.push("        <ul>");
		h.push("            <li>");
		h.push("                <a href=\"javascript:;\"  data-id=\"9\">");
		h.push("                    <img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_9_55.png\" />");
		h.push("                    <p>奥迪</p>");
		h.push("                </a>");
		h.push("            </li>");
		h.push("            <li>");
		h.push("                <a href=\"javascript:;\"  data-id=\"8\">");
		h.push("                    <img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_8_55.png\" />");
		h.push("                    <p>大众</p>");
		h.push("                </a>");
		h.push("            </li>");
		h.push("            <li>");
		h.push("                <a href=\"javascript:;\"  data-id=\"17\">");
		h.push("                    <img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_17_55.png\" />");
		h.push("                    <p>福特</p>");
		h.push("                </a>");
		h.push("            </li>");
		h.push("            <li>");
		h.push("                <a href=\"javascript:;\"  data-id=\"28\">");
		h.push("                    <img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_28_55.png\" />");
		h.push("                    <p>起亚</p>");
		h.push("                </a>");
		h.push("            </li>");
		h.push("            <li>");
		h.push("                <a href=\"javascript:;\"  data-id=\"13\">");
		h.push("                    <img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_13_55.png\" />");
		h.push("                    <p>现代</p>");
		h.push("                </a>");
		h.push("            </li>");
		h.push("        </ul>");
		h.push("    </div>");
		for (var key in data.MsterList) {
			if (data.MsterList[key].length <= 0) continue;
			h.push("<div class=\"tt-small phone-title\" data-key=\"" + key + "\">");
			h.push("    <span>" + key + "</span>");
			h.push("</div>");

			h.push("<div class=\"box\">");
			h.push("    <ul>");
			for (var i = 0; i < data.MsterList[key].length; i++) {
				h.push(" <li>");
				h.push("     <a href=\"javascript:;\" data-id=\"" + data.MsterList[key][i].MasterId + "\">");
				h.push("         <span class=\"brand-logo m_" + data.MsterList[key][i].MasterId + "_b\"></span>");
				h.push("         <span class=\"brand-name\">" + data.MsterList[key][i].MasterName + "</span>");
				h.push("     </a>");
				h.push(" </li>");
			}
			h.push("         </ul>");
			h.push("     </div>");
		}
		h.push("</div>");
		h.push("        <div class=\"fixed-nav\">");
		h.push("            <ul class=\"rows-box\">");
		h.push("                <li><a href=\"#\">#</a></li>");
		for (var key in data.CharList) {
			if (data.CharList[key] > 0) {
				h.push("                <li><a href=\"#\">" + key + "</a></li>");
			}
		}
		h.push("        </ul>");
		h.push("    </div>");
		h.push("    <div class=\"alert\" style=\"display: none;\">");
		h.push("        <span>A</span>");
		h.push("    </div>");
		h.push("</div>");

		$("#sel-master").html(h.join(''));
		$("#sel-master .brand-list li a").on("click", function () {
			var pid = $(this).data("id"),
                masterName = $(this).find(".brand-name").text();
			masterName = masterName == "" ? $(this).find("p").text() : masterName;
			self.getData("http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=serial&pid=" + pid, function (data) {
				getSerial(data, pid, masterName)
			});
		});
		$("#btn-return-master").on("click", function () {
			$("#container").show();
			$("#sel-container").hide();
		});
		//if (defaults.serialId <= 0) {
		//	$("#sel-master .first-tags li").eq(0).hide();
		//}

		bindTagEvent("#sel-master .first-tags li");

		phoneCallback();

		if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
			defaults.footerCallback();
		}
		document.documentElement.scrollTop = 0;
		document.body.scrollTop = 0;
	};

	var getSerial = function (data, masterId, masterName) {
		var h = [];
		h.push("<div class=\"b-return\">");
		h.push("    <a href=\"javascript:;\" id=\"btn-return-serial\" class=\"btn-return\">返回</a>");
		h.push("    <span>选择车型</span>");
		h.push("</div>");
		h.push("    <div class=\"choose-car-name-close bybrand_list\">");
		h.push("        <div class=\"brand-logo-none-border m_" + masterId + "_b\"></div>");
		h.push("        <span class=\"brand-name\">" + masterName + "</span>");
		h.push("        <!-- <a href=\"#\" class=\"choose-car-btn-close\">关闭</a> -->");
		h.push("    </div>");
		for (var i = 0; i < data.length; i++) {
			if (data[i].Child.length <= 0) continue;
			h.push("    <div class=\"tt-small\">");
			h.push("        <span>" + data[i].BrandName + "</span>");
			h.push("    </div>");
			h.push("    <div class=\"pic-txt-h pic-txt-9060\">");
			h.push("        <ul>");
			for (var j = 0; j < data[i].Child.length; j++) {
				h.push("            <li>");
				h.push("                <a href=\"javascript:;\" data-id=\"" + data[i].Child[j].SerialId + "\">");
				h.push("                    <img src=\"" + data[i].Child[j].ImageUrl + "\" />");
				h.push("                    <h4>" + data[i].Child[j].SerialName + "</h4>");
				h.push("                    <p><strong>" + data[i].Child[j].Price + "</strong></p>");
				h.push("                </a>");
				h.push("            </li>");
			}
			h.push("        </ul>");
			h.push("    </div>");
		}

		$("#sel-serial").siblings().hide();
		$("#sel-serial").html(h.join('')).show();
		$("#sel-serial .pic-txt-h.pic-txt-9060 li a").on("click", function () {
			var pid = $(this).data("id"),
                serialName = $(this).find("h4").text();
			self.getData("http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid=" + pid, function (data) {
				getCar(data, pid, serialName)
			});
		});
		$("#btn-return-serial").on("click", function () {
			$("#sel-master").show().siblings().hide();
		});
		document.documentElement.scrollTop = 0;
		document.body.scrollTop = 0;

		if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
			defaults.footerCallback();
		}
	};

	var getCar = function (data, serialId, serialName) {
		var h = [];
		h.push("<div class=\"b-return\">");
		h.push("    <a href=\"javascript:;\" id=\"btn-return-car\" class=\"btn-return\">返回</a>");
		h.push("    <span>选择车款</span>");
		h.push("</div>");
		h.push("<div class=\"choose-car-name-close brandimg_list\">");
		h.push("    <img src=\"" + data.ImageUrl + "\">");
		h.push("    <span class=\"brand-name\">" + serialName + "</span>");
		h.push("    <div class=\"clear\"></div>");
		h.push("    <!-- <a href=\"#\" class=\"choose-car-btn-close\">关闭</a> -->");
		h.push("</div>");
		h.push("<div class=\"clear\"></div>");
		for (var year in data.CarList) {
			h.push("<div class=\"tt-small y2015\">");
			h.push("    <span>" + year.replace('s', '') + "款</span>");
			h.push("</div>");
			h.push("<div class=\"pic-txt-h pic-txt-9060 y2015\">");
			h.push("    <ul>");
			var list = data.CarList[year]
			for (var j = 0; j < list.length; j++) {
				//h.push("        <li class=\"hot\">");
				h.push("        <li id=\"brand_" + list[j].CarId + "\">");
				h.push("            <a href=\"javascript:;\" data-id=\"" + list[j].CarId + "\" data-name=\"" + serialName + " " + list[j].CarName + "\">");
				h.push("                <h4>" + list[j].CarName + "</h4>");
				h.push("                <p><strong>" + list[j].ReferPrice + "万</strong></p>");
				h.push("            </a>");
				h.push("        </li>");
			}
			h.push("    </ul>");
			h.push("</div>");
		}
		$("#sel-car").siblings().hide();
		$("#sel-car").html(h.join('')).show();
		//已选择车型
		for (var i = 0; i < defaults.arrCarId.length; i++) {
			if (defaults.currentCarId && defaults.currentCarId > 0 && defaults.arrCarId[i] == defaults.currentCarId) {
				$("#brand_" + defaults.arrCarId[i]).attr("class", "current").find("h4").prepend("[当前]");
			} else {
				$("#brand_" + defaults.arrCarId[i]).attr("class", "none").find("h4").prepend("[已添加]");
			}
		}
		$("#sel-car .pic-txt-h.pic-txt-9060").find("li:not(li[class]) a").on("click", function () {
			var carId = $(this).data("id"), carName = $(this).data("name");
			if (defaults.callbackFunc) {
				defaults.callbackFunc(carId, carName, defaults.currentIndex);
			}
		});
		$("#btn-return-car").on("click", function () {
			$("#sel-serial").show().siblings().hide();
		});

		document.documentElement.scrollTop = 0;
		document.body.scrollTop = 0;

		if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
			defaults.footerCallback();
		}
	};

	var getHistroy = function () {
		var h = [];
		h.push("<div class=\"b-return\">");
		h.push("    <a href=\"javascript:;\" id=\"btn-return-history\" class=\"btn-return\">返回</a>");
		h.push("    <span>选择车款</span>");
		h.push("</div>");
		h.push("<div class=\"brandlist\">");
		if (defaults["currentSerialName"] == "") {
			h.push("<div class=\"first-tags colums-2\">");
		}
		else {
			h.push("<div class=\"first-tags colums-3\">");
		}
		//h.push("    <div class=\"first-tags colums-3\">");
		h.push("        <ul>");
		if (defaults["currentSerialName"] != "") {
			h.push("            <li data-action=\"serial\"><a href=\"javascript:;\"><span>" + defaults["currentSerialName"] + "</span></a></li>");
		}
		h.push("            <li data-action=\"master\"><a href=\"javascript:;\"><span>按品牌查找</span></a></li>");
		h.push("            <li class=\"current\" data-action=\"history\"><a href=\"javascript:;\"><span>历史记录</span></a></li>");
		h.push("        </ul>");
		h.push("    </div>");
		h.push("    <div class=\"pic-txt-h pic-txt-9060 y2015\">");
		h.push("        <ul id=\"history-carlist\">");
		//h.push("            <li>");
		//h.push("                <a href=\"#\">");
		//h.push("                    <h4>奥迪A6L TFSI 领先 进取版</h4>");
		//h.push("                    <p><strong>20.25万-30.23万</strong></p>");
		//h.push("                </a>");
		//h.push("            </li>");
		h.push("        </ul>");
		h.push("    </div>");
		h.push("</div>");

		$("#sel-history").html(h.join('')).show();
		$("#btn-return-history").on("click", function () {
			$("#container").show();
			$("#sel-container").hide();
		});

		bindTagEvent("#sel-history .first-tags li", reqMaster);

	};

	var bindTagEvent = function (selector, Func) {
		$(selector).on("click", function () {
			var tag = $(this).data("action");
			switch (tag) {
				case "serial":
					$("#sel-cur-serial").show().siblings().hide();
					break;
				case "master":
					$("#sel-master").show().siblings().hide();
					if (Func) Func();
					break;
				case "history":
					$("#sel-history").show().siblings().hide();
					break;
			}
			if (defaults.footerCallback && defaults.footerCallback instanceof Function) {
				defaults.footerCallback();
			}
		});
	};

	var initSelectedCar = function () {

	};

	var rotateEnd = function (fn) {
		function toResize() {
			var winW = document.documentElement.clientWidth,
			   winH = document.documentElement.clientHeight;
			setTimeout(function () { fn && fn(winW > winH ? "horizontal" : "vertical"); }, 200);
		}
		$(window).resize(toResize).trigger('resize');
	};

	var phoneCallback = function () {
		//模拟电话本
		var $body = $('body'),
            $navs = $body.find('.fixed-nav'),
            as = $navs.find('a'),
            $alert = $body.find('.alert'),
            	$brandList = $(".bybrand_list");

		//竖屏显示横屏隐藏
		rotateEnd(function (v) {
			switch (v) {
				case 'horizontal':
					$navs.hide();
					break;
				case "vertical":
					$navs.show();
					break;
			}
		});

		var rows = $('.phone-title', $brandList);
		var arr = [];
		for (var i = 0; i < rows.length; i++) {
			arr.push(rows[i].getAttribute('data-key'));
		}

		var $frist = rows.eq(0),
            istouch = false;

		//算高度居中
		var $rowsbox = $navs.find('.rows-box'),
			rowsA = $rowsbox.find('a');
		var height = rowsA.length * rowsA.height();
		$rowsbox.height(height);
		$navs.height(height);
		var clientHeight = document.documentElement.clientHeight;
		//自适应右侧导航

		clientHeight = document.documentElement.clientHeight;
		var $navli = $navs.find('li');
		var h = rowsA.height() * ((clientHeight / $navs.height()) * 0.8);
		$navli.height(h);
		height = rowsA.length * h;
		$navs.height(height);
		$navs[0].style.top = clientHeight / 2 - height / 2 + 'px';

		//导航插件
		rows.navigate({
			speed: 100,
			init: function () {
				var $navigate = this;
				$navs.gesture({
					offsetTop: clientHeight / 2 - height / 2,
					top: 0,
					init: function () {
						var $gesture = this;
						$navs.touches({
							touchstart: function (ev) {
								ev.preventDefault();
								istouch = true;
								$gesture.trigger('compareTop', ev);
							},
							touchmove: function (ev) {
								$gesture.trigger('compareTop', ev);
							},
							touchend: function () {
								istouch = false;
								setTimeout(function () { $alert.hide(); }, 200);
							}
						})
						$frist.on('select', function (event, idx) {
							as.removeClass('current');
							as.eq(idx).addClass('current');
						})
						$navs.css('opacity', 1);
					},
					selectFn: function (v) {
						var index = arr.indexOf(v);
						if (index != -1) {
							$navigate.trigger('to', index);
						}
						var $current = as.eq(index);
						$alert.html('<span>' + v + '</span>');
						$alert.show();
						$frist.trigger('select', index);
					}
				})
			}
		});
		$navs.show();
	};

	module.getData = function (url, callbackFunc) {
		$.ajax({
			url: url,
			cache: true,
			dataType: "jsonp",
			jsonpCallback: "getData",
			success: function (data) {
				callbackFunc(data);
			}
		});
	}
	module.getLocalData = function (url, callbackFunc) {
		$.ajax({
			url: url,
			cache: true,
			success: function (data) {
				var json = $.parseJSON(data);
				callbackFunc(json);
			}
		});
	}

	var getCookie = function (name) {
		var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

		if (arr != null) {
			return unescape(arr[2]);
		}
		return null;
	};

	return module;
})(SelectCar || {});
