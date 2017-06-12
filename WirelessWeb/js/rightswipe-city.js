var RigthSwipeCity = (function (module) {
	var self = module,
        defaults = {
        	cityFn: defaultCity
        };

	module.initCity = function (options) {
		$.extend(true, defaults, options);
		getProvince();
		bindCity();
	}
	//默认城市点击
	function defaultCity($current) {
		var cityId = $current.data("cityid");
		if (cityId > 0) {
			var hashObj = {};//getHashObject();
			hashObj["city"] = cityId;
			location.hash = getHash(hashObj);
			window.location.reload();
 		}
	}
	//获取城市数据
	function getCity(parentId) {
		var cityHtml = [];
		var cityList = cityJson[parentId].Child;		cityHtml.push("<ul class=\"first-list absolute\">");		cityHtml.push("<li class=\"root\"><a href=\"javascript:void(0)\">" + cityJson[parentId].CityFullName + "<em></em></a></li>");		for (var index = 0; index < cityList.length; index++) {
			cityHtml.push("<li><a data-cityid=\"" + cityList[index].CityId + "\" href=\"javascript:;\">" + cityList[index].CityName + "<em></em></a></li>");
		}
		cityHtml.push("</ul>");
		$("#cityList .swipeLeft").html(cityHtml.join(""));
		$("#cityList").find("a").on("click", function () {
			var hashObj = getHashObject();
			var cityId = $(this).data("cityid");
			var isRoot = $(this).closest("li[class='root']").length > 0 ? true : false;
			if (isRoot) {
				$("#cityList").find(".swipeLeft").removeClass("swipeLeft-block").empty();
				$("#cityList").hide();
				return;
			}
			hashObj["city"] = cityId;
			window.location.hash = getHash(hashObj);
			window.location.reload();
		});
	}	//获取省份数据	function getProvince() {
		var provinceHtml = [];		provinceHtml.push("<div class=\"swipeLeft\">");		provinceHtml.push("<ul class=\"first-list absolute\">");		for (var id in cityJson) {
			if (cityJson[id].Child.length > 1) {
				provinceHtml.push("<li id=\"" + id + "\" class=\"sub\">");
				provinceHtml.push("<a data-action=\"city\" data-provinceid=\"" + id + "\" href=\"javascript:;\">" + cityJson[id].CityFullName + "<em></em></a></li>");
			} else {
				provinceHtml.push("<li id=\"" + id + "\">");
				provinceHtml.push("<a data-cityid=\"" + cityJson[id].Child[0].CityId + "\" data-cityname=\"" + cityJson[id].Child[0].CityName + "\" data-cityallspell=\"" + cityJson[id].Child[0].AllSpell + "\"  href=\"javascript:;\">" + cityJson[id].CityFullName + "<em></em></a></li>");
			}
		}		provinceHtml.push("</ul></div></div>");
		$("#provinceList").html(provinceHtml.join(""));
	}	//绑定效果	function bindCity() {
		$('[data-action=province]').rightSwipe({
			clickEnd: function (b, $current) {
				var $leftPopup = this;
				if (b) {
					var $back = $('.' + $leftPopup.attr('data-back'))
					$back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
					var $swipeLeft = $leftPopup.find('.swipeLeft');
					$swipeLeft.touches({ touchmove: function (ev) { ev.preventDefault(); } });
					var myScroll = new iScroll($swipeLeft[0], {
						snap: 'li',
						momentum: true,
						click: true,
						onBeforeScrollStart: function (ev) { }
					});
					$leftPopup.find("a[data-cityid]").on("click", function () {
						defaults.cityFn($(this));
 					});
					$('[data-action=city]').rightSwipe({
						zindex: 9999999,
						clickEnd: function (b, $current) {
							var $leftPopup = this;
							if (b) {
								var pid = $current.data("provinceid"), cityId = $current.data("cityid");
								if (cityId > 0) {
									defaults.cityFn($current);
									return;
								}
								getCity(pid);
								var $back = $('.' + $leftPopup.attr('data-back'))
								$back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
								var $swipeLeft = $leftPopup.find('.swipeLeft');
								$swipeLeft.touches({ touchmove: function (ev) { ev.preventDefault(); } });
								var myScroll = new iScroll($swipeLeft[0], {
									snap: 'li',
									momentum: true,
									click: true,
									onBeforeScrollStart: function (ev) { }
								});
							}
						}
					});
				}
			}
		});
	}
	return module;
})(RigthSwipeCity || {});