Date.prototype.Format = function (fmt) {
	var o = {
		"M+": this.getMonth() + 1,                 //月份   
		"d+": this.getDate(),                    //日   
		"h+": this.getHours(),                   //小时   
		"m+": this.getMinutes(),                 //分   
		"s+": this.getSeconds(),                 //秒   
		"q+": Math.floor((this.getMonth() + 3) / 3), //季度   
		"S": this.getMilliseconds()             //毫秒   
	};
	if (/(y+)/.test(fmt))
		fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
	for (var k in o) {
		if (new RegExp("(" + k + ")").test(fmt)) {
			fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
		}
	}
	return fmt;
}
//获取贷款
function getLoanData() {
	$.ajax({
		url: "http://carapi.chedai.bitauto.com/api/SummarizeFinancialProducts/SearchSummarizeFinancialProducts?cityId=" + cityId + "&serialId=" + CarCommonCSID + "&pageSize=3", dataType: "jsonp", jsonpCallback: "creditinfo", cache: true, success: function (result) {
			var data = $.parseJSON(result);
			if (typeof data !== "undefined" && data.length > 0) {
				var h = [];
				$.each(data, function (i, n) {
					if (i >= 3) return;
					var classNameArray = ["daikuan-f", "", "daikuan-l"];
					h.push("<div class=\"daikuan-box " + classNameArray[i] + "\">");
					h.push("	<dl class=\"daikuan-name\">");
					h.push("		<img src=\"" + n.CompanyLogoUrl + "\" />");
					h.push("		<dt>" + n.CompanyName + "</dt>");
					h.push("		<dd>成功率：<span class=\"photo_box\"><span class=\"succ\" style=\"width: " + (n.SuccessScore / 5) * 100 + "%;\"></span></span>");
					h.push("		</dd>");
					h.push("	</dl>");
					h.push("	<div class=\"daikuan-money\">");
					h.push("		<p>");
					h.push("			<span>月供</span>");
					h.push("			<span><strong>" + n.MonthlyPaymentText + "</strong></span>");
					h.push("		</p>");
					h.push("		<p class=\"last\">");
					h.push("			<span class=\"tc-pop\">");
					h.push("				<em>总成本</em>");
					h.push("			</span>");
					h.push("			<span class=\"gray\"><strong>" + n.TotalCostText + "</strong> </span>");
					h.push("		</p>");
					h.push("		<div class=\"tc tc-zong\" style=\"display: none;\">");
					h.push("			<div class=\"tc-box\">");
					h.push("				<i></i>");
					h.push("				<p>月利率=" + n.InterestRateText + "</p>");
					h.push("				<p>总成本=" + n.TotalInterestText + " （利息）+" + n.ServiceFeeText + "（手续费）</p>");
					h.push("			</div>");
					h.push("		</div>");
					h.push("	</div>");
					var labelArray = (n.MultiLabel != null && n.MultiLabel != "") ? n.MultiLabel.split("||") : [];

					h.push("	<div class=\"daikuan-keywords\">");
					$.each(labelArray, function (ii, nn) {
						h.push("		<span>" + nn + "</span>");
					});
					h.push("	</div>");

					h.push("	<div class=\"daikuan-detail button_gray\">");
					h.push("		<a target=\"_blank\" href=\"" + n.PCDetailsUrl + "&leads_source=p002018\">查看详情</a>");
					h.push("	</div>");
					h.push("</div>");
				});
				Bglog_InitPostLog();
				h.push("<div class=\"clear\"></div>");
				$("#pc-load").append(h.join('')).show();
				$("#pc-load .tc-pop").hover(function () { $(this).parent().siblings(".tc-zong").show(); }, function () { $(this).parent().siblings(".tc-zong").hide(); });
			}
		}
	});
}
//请求二手车数据
function searchEscByTypeRequest(csSpell) {
	$.ajax({
		url: "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?sid=" + CarCommonCSID + "&cityid=" + cityId + "&count=8", dataType: "jsonp", cache: true, jsonpCallback: "escSearchCarCallBack", success: function (data) {
			//add citylist
			var dataRelateCityList = data.RelateCityList;
			var strHtml = [];
			strHtml.push("<div class=\"title-box\">");
			strHtml.push("<h3><a target=\"_blank\" data-channelid=\"2.21.821\" href=\"http://www.taoche.com/" + csSpell + "/\">相关二手车推荐</a></h3>");
			var isTongjiaweiShow = false;
			if (data.CarListInfo.length > 0) {
				strHtml.push("<ul id=\"data_tab1\" class=\"title-tab\"><li class=\"current\" ><a href=\"javascript:;\">同价位</a><em>|</em></li><li><a href=\"javascript:;\">同车系</a></li></ul>");
			} else {
				isTongjiaweiShow = true;
				strHtml.push("<ul id=\"data_tab1\" class=\"title-tab\"><li class=\"current\"><a href=\"javascript:;\">同价位</a><em>|</em></li><li  ><a href=\"javascript:;\">同车系</a></li></ul>");
			}
			strHtml.push("<div class=\"more\" data-channelid=\"2.21.823\"><a target=\"_blank\" href=\"http://www.taoche.com/" + csSpell + "/\">");
			if (dataRelateCityList.length > 0) {
				$.each(dataRelateCityList, function (index, res) {
					strHtml.push("<a href=\"" + res.CityUrl + "\" target=\"_blank\">" + res.CityName + "</a> | ");
				});
			}
			strHtml.push("<a target=\"_blank\" href=\"http://www.taoche.com/" + csSpell + "/\">更多&gt;&gt;</a></div>");
			strHtml.push("</div>");
			strHtml.push("<div class=\"tit-msg\">专家陪伴，买车无忧，<a href=\"http://bangmai.taoche.com/?ref=pc_zsy_tcx_bm\" target=\"_blank\">体验帮买车服务&gt;&gt;</a>上门评估，轻松卖高价，<a href=\"http://maiche.taoche.com/paimai/?ref=pc_zsy_tcx_gjmc\" target=\"_blank\">立即高价卖车&gt;&gt;</a></div>");
			$("#ucarlist").append(strHtml.join(''));
			//add  sameserial carlist
			renderEscDivHtml(data, 1, false);
			if (priceRang.indexOf("万") > 0) {
				priceRang = priceRang.replace(/万$/, "");
			}
			$.ajax({
				url: "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?cityid=" + cityId + "&p=" + priceRang + "&count=8", dataType: "jsonp", cache: true, jsonpCallback: "escSearchCarCallBack", success: function (data) {
					//add sameprice carlist
					renderEscDivHtml(data, 0, isTongjiaweiShow);
					tabs("data_tab1", "data_box1", null, true);//绑定效果
				}
			});
		}

	});
}
///同车系没有数据的时候，显示同价位二手车
function renderEscDivHtml(data, linum, isTongjiaweiShow) {
	var dataCarListInfo = data.CarListInfo;
	if (dataCarListInfo.length <= 0) {
		return;
	}
	var strHtml = [];
	if (linum == 0) {
		strHtml.push("<div data-channelid=\"2.21.822\" id=\"data_box1_" + linum + "\" class=\"cxdb-box taoche-box taoche-box-mline\" style=\"display:block\">");
	}
	else {
		if (isTongjiaweiShow) {
			strHtml.push("<div data-channelid=\"2.21.822\" id=\"data_box1_" + linum + "\" class=\"cxdb-box taoche-box taoche-box-mline\" style=\"display:block\">");
		} else {
			strHtml.push("<div data-channelid=\"2.21.822\" id=\"data_box1_" + linum + "\" class=\"cxdb-box taoche-box taoche-box-mline\" style=\"display:none\">");
		}
	}
	strHtml.push("<ul id=\"escinfo\">");
	$.each(dataCarListInfo, function (i, n) {
		if (i > 7) return;
		var className = "";
		if (i == 0 || i == 4) className = "first";
		if (i == 3 || i == 7) className = "last";
		strHtml.push("<li class=\"" + className + "\"><div class=\"img-box\"><a href=\"" + n.CarlistUrl + "&leads_source=p002019\" target=\"_blank\" class=\"pic\"><img width=\"150\" height=\"100\" src=\"" + n.PictureUrl + "\"></a></div>");
		strHtml.push("<div class=\"txt-box\"><a href=\"" + n.CarlistUrl + "&leads_source=p002019\" target=\"_blank\">" + n.BrandName + "</a></div>");
		strHtml.push("<strong>" + n.DisplayPrice + "</strong>");
		//strHtml.push("<span>" + n.BuyCarDate.substring(2) + "上牌 " + n.DrivingMileage + "公里</span></li>");
		strHtml.push("<span>" + (n.BuyCarDate == "未上牌" ? "未上牌" : (n.BuyCarDate.substring(2) + "上牌")) + " " + n.DrivingMileage + "公里</span></li>");
	});
	strHtml.push("</ul>");
	strHtml.push("</div>");
	strHtml.push("<div class=\"clear\"></div>");
	$("#ucarlist").append(strHtml.join(''));
	Bglog_InitPostLog();
}
//补贴接口
function getSubsidy(serialId, cityId) {
	$.ajax({
		url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + serialId + "&cityid=" + cityId + "",
		dataType: "jsonp",
		jsonpCallback: "getSubsidyCallback",
		cache: true,
		success: function (data) {
			if (!(data && data.length > 0)) return;
			$.each(data, function (i, n) {
				if (!(n.StateSubsidies > 0 && n.LocalSubsidy > 0)) return;
				var carLine = $("#carlist_" + n.CarId);
				var sub = [];
				if (n.StateSubsidies > 0)
					sub.push("国家补贴" + n.StateSubsidies + "万元");
				if (n.LocalSubsidy > 0)
					sub.push("地方补贴" + n.LocalSubsidy + "万元");
				if (carLine.find("a[name=\"butie\"]").length > 0) {
					carLine.find("a[name=\"butie\"]").attr("title", sub.join(","));
				} else {
					var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"butie\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
					if (carLine.find("span.hundong").length > 0) {
						carLine.find("span.hundong").after(b);
					} else {
						carLine.find("a:first").after(b);
					}
				}
			});
		}
	});
}

//登录 车型关注
function initLoginFavCar() {
	try {
		var carLoginresult = Bitauto.Login.result;
		if (typeof carLoginresult != 'undefined' && typeof carLoginresult.plancar != 'undefined' && carLoginresult.plancar.length > 0) {
			for (var i = 0; i < carLoginresult.plancar.length; i++) {
				if (carLoginresult.plancar[i].CarSerialId == CarCommonCSID) { $("#favstar").attr("title", "取消关注").find("span").removeClass("no-sc"); break; }
			}
		}
	} catch (e) { }
}
//添加 取消关注车型
function FocusCar(obj) {
	Bitauto.Login.afterLoginDo(function () {
		var id = CarCommonCSID;
		obj.find("span").attr('class') == "no-sc" ? Bitauto.UserCars.addConcernedCar(id, function () {
			if (Bitauto.UserCars.plancar.message[0] == "已超过上限") {
				$("#mangerCar_tc a").attr("href", "http://i.yiche.com/u" + Bitauto.Login.result.userId + "/car/guanzhu/");
				$("#FocusCarFull").show();
			}
			else {
				obj.attr("title", "取消关注").find("span").removeClass("no-sc");
				Bitauto.UserCars.plancar.arrplancar.unshift(id);
			}
		}) : Bitauto.UserCars.delConcernedCar(id, function () {
			obj.attr("title", "点击关注").find("span").addClass("no-sc");
		});
	});
};
//====================add by 2014.05.19 车型列表过滤 start=============================
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.intersect = function (b) {
	var i = 0, result = [];
	while (i < this.length && i < b.length) {
		if (this.length > b.length && this.indexOf(b[i]) != -1) { result.push(b[i]); }
		else { if (b.indexOf(this[i]) != -1) { result.push(this[i]); } }
		i++;
	}
	return result;
}
var CarListLeftFilter = {
	Year: [],
	Tran: [],
	Exhaust: [],
	OffsetTop: 40,
	FilterHeight: 0,
	CarListHeight: 0,
	bindEvent: function () {
		var self = this;
		$("input[name='car-filter-year']").click(function () {
			if ($(this).prop("checked") == true) {
				self.Year.push($(this).val());
			} else {
				self.Year.remove($(this).val());
			}
			self.actionFilter();
			//statForTempString(100, CarCommonCSID, $(this).val());
		});
		$("input[name='car-filter-tran']").click(function () {
			if ($(this).prop("checked") == true) {
				self.Tran.push($(this).val());
			} else {
				self.Tran.remove($(this).val());
			}
			self.actionFilter();
			//statForTempString(100, CarCommonCSID, $(this).val());
		});
		$("input[name='car-filter-exhaust']").click(function () {
			if ($(this).prop("checked") == true) {
				self.Exhaust.push($(this).val());
			} else {
				self.Exhaust.remove($(this).val());
			}
			self.actionFilter();
			//statForTempString(100, CarCommonCSID, $(this).val());
		});
		$("#car-filter-clear").click(function () {
			$("input[name='car-filter-year']").prop("checked", false);
			$("input[name='car-filter-tran']").prop("checked", false);
			$("input[name='car-filter-exhaust']").prop("checked", false);
			self.Year.length = 0;
			self.Tran.length = 0;
			self.Exhaust.length = 0;
			self.actionFilter();
			//statForTempString(100, CarCommonCSID, "清除筛选");
		});
	},
	initFilterPage: function () {
		if (typeof CarFilterData == "undefined" || CarFilterData == null) return;
		this.initFilterHtml();
		this.bindEvent();
		this.SpecialEffects();
		this.FilterHeight = $("#carCompareFilter").get(0).offsetHeight;
		this.CarListHeight = $("#data_tab_jq5_1").length > 0 ? $("#data_tab_jq5_1").get(0).offsetHeight : $("#data_tab_jq5_0").get(0).offsetHeight;
	},
	initFilterHtml: function () {
		if (typeof CarFilterData == "undefined" || CarFilterData == null ||
		(CarFilterData.Year.length <= 0 && CarFilterData.Trans.length <= 0 && CarFilterData.Exhaust.length <= 0))
			return;

		var filterHtml = [];
		filterHtml.push("<ul id=\"car-filter-year-list\">");
		for (var i = 0; i < CarFilterData.Year.length; i++) {
			//if (i >= 2) break;
			filterHtml.push("<li><label><input type=\"checkbox\" value=\"" + CarFilterData.Year[i] + "\" name=\"car-filter-year\">" + CarFilterData.Year[i] + "款</label></li>");
		}
		filterHtml.push("</ul>");
		filterHtml.push("<ul id=\"car-filter-tran-list\">");
		for (var i = 0; i < CarFilterData.Trans.length; i++) {
			filterHtml.push("<li><label><input type=\"checkbox\" value=\"" + CarFilterData.Trans[i] + "\" name=\"car-filter-tran\">" + CarFilterData.Trans[i] + "</label></li>");
		}
		filterHtml.push("</ul>");
		filterHtml.push("<ul id=\"car-filter-exhaust-list\">");
		for (var i = 0; i < CarFilterData.Exhaust.length; i++) {
			filterHtml.push("<li><label><input type=\"checkbox\" value=\"" + CarFilterData.Exhaust[i] + "\" name=\"car-filter-exhaust\">" + CarFilterData.Exhaust[i] + "</label></li>");
		}
		filterHtml.push("</ul>");
		filterHtml.push("<a href=\"javascript:;\" id=\"car-filter-clear\" class=\"filter-clear\">清除筛选</a>");
		$("#filter-list").html(filterHtml.join(''));
		$("#carCompareFilter").show();
	},
	actionFilter: function () {
		for (var group in CarFilterData.GroupList) {
			$("#car_filter_gid_" + group).show();
			if (this.Year.length > 0 && this.Year.intersect(CarFilterData.GroupList[group].YearType).length <= 0) {
				$("#car_filter_gid_" + group).hide(); continue;
			}
			if (this.Exhaust.length > 0 && this.Exhaust.intersect(CarFilterData.GroupList[group].Exhaust).length <= 0) {
				$("#car_filter_gid_" + group).hide(); continue;
			}
			if (this.Tran.length > 0 && this.Tran.intersect(CarFilterData.GroupList[group].Transmission).length <= 0) {
				$("#car_filter_gid_" + group).hide(); continue;
			}
		}

		for (var carId in CarFilterData.CarList) {
			$("#car_filter_id_" + carId).show();
			if (this.Year.length > 0 && this.Year.indexOf(CarFilterData.CarList[carId].YearType) == -1) {
				$("#car_filter_id_" + carId).hide();
			}
			if (this.Exhaust.length > 0 && this.Exhaust.indexOf(CarFilterData.CarList[carId].Exhaust) == -1) {
				$("#car_filter_id_" + carId).hide();
			}
			if (this.Tran.length > 0 && this.Tran.indexOf(CarFilterData.CarList[carId].Transmission) == -1) {
				$("#car_filter_id_" + carId).hide();
			}
		}
		this.CarListHeight = $("#data_tab_jq5_1").length > 0 ? $("#data_tab_jq5_1").get(0).offsetHeight : $("#data_tab_jq5_0").get(0).offsetHeight;
	},
	SpecialEffects: function () {
		// 层浮动
		if ($("#carCompareFilter").length <= 0) return false;

		var carFilter = $("#carCompareFilter"),
			carCFtopHeight = carFilter.offset().top,
			iE6 = !!window.ActiveXObject && !window.XMLHttpRequest,
		//carListHeight = $("#data_tab_jq5_0").get(0).offsetHeight,
		//carCompareFilterHeight = carFilter.get(0).offsetHeight,//筛选浮动层的高度
			self = this;

		$(window).bind("scroll", function () {
			var carCFscrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
			if (iE6 || self.CarListHeight <= self.FilterHeight) {
				carFilter.attr("class", "car-compare-filter").css({ "top": "" + self.OffsetTop + "px", "bottom": "auto" });
				return false;
			} else {
				if (carCFscrollHeight > (carCFtopHeight + self.CarListHeight - self.FilterHeight)) {
					carFilter.attr("class", "car-compare-filter").css({ "top": "auto", "bottom": "0" });
				} else if ((carCFscrollHeight + self.OffsetTop) > carCFtopHeight) {
					carFilter.attr("class", "car-compare-filter car-compare-filter-fixed").css({ "top": "" + self.OffsetTop + "px", "bottom": "auto" });
				} else {
					carFilter.attr("class", "car-compare-filter").css({ "top": "" + self.OffsetTop + "px", "bottom": "auto" });
				}
			}
		});
		$("#btnFilterShow").click(function () {
			$("#carFilterBox").hide();
			$("#btnFilterHide").show();
			carCompareFilterHeight = carFilter.get(0).offsetHeight;
			//statForTempString(100, CarCommonCSID, "收起筛选");
		});
		$("#btnFilterHide").click(function () {
			$("#carFilterBox").show();
			$("#btnFilterHide").hide();
			carCompareFilterHeight = carFilter.get(0).offsetHeight;
			//statForTempString(100, CarCommonCSID, "展开筛选");
		});
	}
};
//=====================end==========================================================
//易车购车服务
function fillCarPart(data, type) {
	if (typeof data == "undefined") return;
	var len = 0, h = [];
	switch (type) {
		case 1:
			if ($("#gouche-box-mall").length <= 0) {
				$("#gouche-box").append("<div class=\"txt\"><a href=\"" + data.link + "&tracker_u=123_yccxfwl\" target=\"_blank\">易车·惠买车</a> 特惠价：<strong><a href=\"" + data.link + "&tracker_u=123_yccxfwl\" target=\"_blank\">" + data.price + "万起</a></strong></div>");
				$("#gouche-box").append("<span class=\"button_gray\" id=\"gouche-box-huibtn\"><a class=\"\" href=\"" + data.link + "&tracker_u=123_yccxfwl\" target=\"_blank\">立即购买&gt;&gt;</a></span>");
			} else {
				$("#gouche-box-mall").before("<div class=\"txt\"><a href=\"" + data.link + "&tracker_u=123_yccxfwl\" target=\"_blank\">易车·惠买车</a> 特惠价：<strong><a href=\"" + data.link + "&tracker_u=123_yccxfwl\" target=\"_blank\">" + data.price + "万起</a></strong></div>");
			}
			break;
		case 2:
			//if (data.CsUrl && data.CsUrl != "") {
			//	if (!(data && data.CarList && data.CarList.length > 0)) return;
			//	var carList = data.CarList;
			//	carList.sort(function (a, b) {
			//		return a.Price - b.Price;
			//	});
			//	var minPrice = Math.floor((carList[0].Price / 10000) * 100) / 100
			//	if (minPrice > 0) {
			//		$("#gouche-box").append("<div class=\"txt\" id=\"gouche-box-mall\"><a href=\"" + carList[0].Url + "?source=yc-zs-mall-01&carid=" + CarCommonCSID + "\" target=\"_blank\">易车商城</a> 直销价：<strong><a href=\"" + carList[0].Url + "?source=yc-zs-pp-01&carid=" + CarCommonCSID + "\" target=\"_blank\">" + minPrice + "万起</a></strong> </div>");
			//		$("#gouche-box").append("<span class=\"button_gray\" id=\"gouche-box-mallbtn\"><a class=\"\" href=\"" + carList[0].Url + "\" target=\"_blank\">立即购买&gt;&gt;</a></span>");
			//	}
			//}
			if (data && data.CarList && data.CarList.length > 0) {
				var carList = data.CarList;
				$("#gouche-box").append("<div class=\"txt\" id=\"gouche-box-mall\"><a href=\"http://www.yichemall.com/car/detail/c_" + carList[0].CarId + "/?source=yc-zs-mall-01&carid=" + CarCommonCSID + "\" target=\"_blank\">易车商城</a> 直销价：<strong><a href=\"http://www.yichemall.com/car/detail/c_" + carList[0].CarId + "/?source=yc-zs-pp-01&carid=" + CarCommonCSID + "\" target=\"_blank\">" + carList[0].Price + "万起</a></strong> </div>");
				$("#gouche-box").append("<span class=\"button_gray\" id=\"gouche-box-mallbtn\"><a class=\"\" href=\"http://www.yichemall.com/car/detail/c_" + carList[0].CarId + "/?source=yc-zs-pp-01&carid=" + CarCommonCSID + "\" target=\"_blank\">立即购买&gt;&gt;</a></span>");
			}
			break;
	}
	len = $("#gouche-box .txt").length;
	if (len >= 2) {
		$("#gouche-box .button_gray").hide();
	}
	if (len > 0)
		$("#gouche-container").show();
}

//统计
var channelIDs = { "9": "2.21.104", "10": "2.21.105", "11": "2.21.106", "12": "2.21.199", "13": "2.21.107", "14": "2.21.995" };
var urlEndPartCode = { "9": "&tracker_u=123_yccxfwl&leads_source=p002006", "10": "?source=100569&leads_source=p002007", "11": "&ref=car1&rfpa_tracker=1_7&leads_source=p002008", "12": "?ref=chexizska&leads_source=p002009", "14": "?leads_source=p002024" };

(function () {
	//如果是停售车系，关于商机按钮的处理
	if (csSaleState == "停销") {
		var lianJieArray = [];
		lianJieArray.push("<span class=\"button_orange button_90_34\">");
		lianJieArray.push("<a target=\"_blank\" data-channelid=\"2.21.102\" href=\"http://yiche.taoche.com/" + csAllSpell + "/?ref=chexizsmai&leads_source=p002020\"" + ">买二手车</a>");
		lianJieArray.push("</span>");
		lianJieArray.push("<span class=\"button_gray button_90_34\">");
		lianJieArray.push("<a target=\"_blank\" data-channelid=\"2.21.103\" href=\"http://www.taoche.com/pinggu/?ref=chexizsgu\">二手车估价</a>");
		lianJieArray.push("</span>");
		$('.zh-btn-box').html(lianJieArray.join(""));
	}

	//购车服务
	$.ajax({
		url: "http://api.car.bitauto.com/api/GetBusinessService.ashx?action=pcserial&cityid=" + cityId + "&serialid=" + CarCommonCSID + "&d=" + (new Date()).Format("yyyyMMddhh"),
		async: false,
		dataType: "jsonp",
		jsonpCallback: "businessCarCallBack",
		cache: true,
		success: function (data) {
			if (data && data != null) {
				if (data && data.CornerButton.length > 0) {
					$("#btnDirectSell").html(data.CornerButton[0].ShortTitle);
					$("#btnDirectSell").attr("href", data.CornerButton[0].Url + "?leads_source=p002027" + tongJiEndUrlParam);
					$("#btnDirectSell").show();
				}

				var serviceHtml = [];
				if (data.BigButton) {
					var bigBtnCnt = data.BigButton.length; //购车服务数量
					//PC展示逻辑：
					//顺序：“惠买车”、“易车商城”、“易车惠”、“购车服务”；默认显示前三个，不足三个则依次补充，最少显示两个，少于两个则不显示该区域，其中包销，未上市车不展示该区域，

					var forend = bigBtnCnt;//循环终止标记
					if (bigBtnCnt > 3) {
						forend = 4;
						serviceHtml.push("<ul>");
					}
					else if (bigBtnCnt > 1) {
						serviceHtml.push("<ul>");
					}
					else {
						//购车服务块不显示
						return;
					}
					var i;
					for (i = 0; i < forend; i++) {
						var curDataChannelId = (channelIDs[data.BigButton[i].BusinessId] == undefined ? "" : channelIDs[data.BigButton[i].BusinessId]);//统计cid
						if (data.BigButton[i].BusinessId == "13") {  //购车服务Button
							serviceHtml.push("<li class=\"noborder\">");
							serviceHtml.push("<span>" + data.BigButton[i].Title + "</span>");
							serviceHtml.push("<strong><a class=\"all\" data-channelid=\"" + curDataChannelId + "\" href=\"" + data.BigButton[i].Url + "\" class=\"all\" target=\"_blank\">" + data.BigButton[i].Price + "</a></strong>");
							serviceHtml.push("</li>");
						}
						else {
							serviceHtml.push("<li>");
							////add by sk 2016.04.22 惠买车 周年
							//if (data.BigButton[i].BusinessId == 9) {
							//	serviceHtml.push("<span class=\"anniversary\">" + data.BigButton[i].LongTitle + "</span>");
							//} else {
							serviceHtml.push("<span>" + data.BigButton[i].LongTitle + "</span>");
							//}
							serviceHtml.push("<strong><a data-channelid=\"" + curDataChannelId + "\" href=\"" + data.BigButton[i].Url + (urlEndPartCode[data.BigButton[i].BusinessId] == undefined ? "" : urlEndPartCode[data.BigButton[i].BusinessId]) + "\" target=\"_blank\">" + data.BigButton[i].Price + "</a></strong>");
							serviceHtml.push("</li>");
						}

					}
					serviceHtml.push("</ul>");
					$(".gouche_box_new").html(serviceHtml.join(''));
					$("#gouche-container").show();
					//add log statistics,call this method
					Bglog_InitPostLog();
				}

			}
		}
	});
	//安康 补贴
	getSubsidy(CarCommonCSID, cityId);
	////惠买车
	//$.ajax({
	//	url: "http://www.huimaiche.com/api/GetCarSerialPrice.ashx?ccode=" + bit_locationInfo.cityId + "&csid=" + CarCommonCSID + "", cache: true, dataType: "jsonp", jsonpCallback: "shuiCallback", success: function (data) {
	//		fillCarPart(data, 1);
	//	}
	//});
	////直销
	//$.ajax({
	//	url: "http://api.car.bitauto.com/mai/GetSerialDirectSell.ashx?serialId=" + CarCommonCSID + "&cityid=" + bit_locationInfo.cityId + "", cache: true, dataType: "jsonp", jsonpCallback: "smallCallback", success: function (data) {
	//		fillCarPart(data, 2);

	//	}
	//});
	////购车服务 2015.5.16
	//$.ajax({
	//	url: "http://api.gouche.yiche.com/PreferentialCar/GetCarListBySerialId?serialid=" + CarCommonCSID + "&cityid=" + bit_locationInfo.cityId + "", cache: true, dataType: "jsonp", jsonpCallback: "guocheCallback", success: function (data) {
	//		if (data && data.length > 0) {
	//			if ([2381, 2932, 2714, 1905, 1909, 1765, 3544, 2168, 2167, 2848, 1879, 1873, 2701, 1977, 2407, 3466].indexOf(CarCommonCSID) != -1) {
	//				$("#thelogoID .rank").before("<a href=\"http://gouche.yiche.com/" + bit_locationInfo.engName + "/\" target=\"_blank\" class=\"part66_box\">6.6购车节</a>");
	//			} else {
	//				$("#thelogoID .rank").before("<a href=\"http://gouche.yiche.com/" + bit_locationInfo.engName + "/sb" + CarCommonCSID + "/\" target=\"_blank\" class=\"part66_box\">6.6购车节</a>");
	//				$.each(data, function (i, n) {
	//					$("#carlist_" + n.CarId).append("<a href=\"http://gouche.yiche.com/" + bit_locationInfo.engName + "/sb" + CarCommonCSID + "/m" + n.CarId + "/\" target=\"_blank\" class=\"ad-d66\">66购车节</a>");
	//				});
	//			}
	//		}
	//	}
	//});
})();
//--综述页 效果 车款 选项卡切换 停销年款款显示 jquery
$(function () {
	CarListLeftFilter.initFilterPage();
	initLoginFavCar();
	//添加关注
	$("#favstar").bind("click", function () {
		FocusCar($(this));
	});
	$("#a-focus-close,#btn-focus-close").click(function () { $("#FocusCarFull").hide(); });
	//焦点区 效果
	$("#focus_color_box li").hover(function () {
		var index = $(this).index();
		$(this).addClass("current").siblings().removeClass("current");
		$("#focus_images > div[class!='ad_300_30'][id='focuscolor_" + (index + 1) + "']").show().siblings("div[class!='ad_300_30']").hide();
	}, function () {
		$("#color-listbox li").removeClass("current");
		$("#focus_images > div[class!='ad_300_30'][id='focus_image_first']").show().siblings("div[class!='ad_300_30']").hide();
	});
	//颜色块 滑动效果
	var colorObj = $("#focus_color_box li");
	var wh = -colorObj.length * 23;
	var leftV = 0;
	if (colorObj.length > 8) {
		$("#focus_color_l").click(function () { leftV = leftV + 185; if (leftV >= 0) { leftV = 0; $(this).removeClass("a_l_hover"); $("#focus_color_r").addClass("a_r_hover"); } else { $("#focus_color_r").addClass("a_r_hover"); } $("#color-listbox").animate({ "left": leftV }, 300); });
		$("#focus_color_r").click(function () { leftV = leftV - 185; $("#focus_color_l").addClass("a_l_hover"); if (leftV - 185 < wh) { $(this).removeClass("a_r_hover"); } if (leftV < wh) { leftV = leftV + 185; return; } $("#color-listbox").animate({ "left": leftV }, 300); });
	}
	//车型列表效果
	$("#car_list tr[id^='car_filter_id_']").hover(
	function () {
		$(this).addClass('hover-bg-color');
		//$(this).find(".pdL10 a").eq(0).addClass('strong');
		//$(this).find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
		//if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
		//	$(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
	},
		function () {
			$(this).removeClass('hover-bg-color');
			//$(this).find(".pdL10 a").eq(0).removeClass('strong');
			//$(this).find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
			//if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
			//	$(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
		});
	//// 按钮点击状态
	//$('#car_list .car-summary-btn').hover(function () { $(this).addClass('car-summary-btn-hover'); },
	//	function () { $(this).removeClass('car-summary-btn-hover'); $(this).removeClass('car-summary-btn-active'); })
	//	.mousedown(function () { $(this).addClass('car-summary-btn-active'); })
	//	.mouseup(function () { $(this).removeClass('car-summary-btn-active'); });
	$("#car_nosaleyearlist").hover(function () { $(this).find("#carlist_nosaleyear").show(); }, function () { $(this).find("#carlist_nosaleyear").hide(); });
	//resolve lazyload
	$("#data_tab_jq5 li").eq(0).one("mouseover", function () {
		if ($("#data_tab_jq5_1").length == 0) return;
		var waitCarList = $("#data_tab_jq5_0").height();
		var saleCarList = $("#data_tab_jq5_1").height();
		if (waitCarList < saleCarList)
			$(window).scrollTop($(window).scrollTop() + 1);
	});
	jqTab("data_tab_jq5", "click");
	//jquery tab
	function jqTab(containerId, eventListener) {
		$("#" + containerId + ">li[id!='car_nosaleyearlist']").each(function (i) {
			$(this).bind(eventListener, function () {
				$(this).addClass("current").siblings().removeClass("current");
				$("#" + containerId + "_" + i + "").show().siblings("div[id^='" + containerId + "']").hide();
				$("#noSaleTitle").remove();
				if ($(this).children(":eq(0)").text() == "在售") {
					$("#carCompareFilter").show();
				}
				else {
					$("#carCompareFilter").hide();
				}
				$("#car_nosaleyearlist .pop").html("停售车款<strong></strong>");
			});
		});
	}
	//导航头 停销年款下拉
	$("#bt_car_spcar").hover(function () { $(this).find("dd").show(); },
				function () { $(this).find("dd").hide(); });
	$("#bt_car_spcar_table").hover(function () { $(this).find("dd").show(); },
				function () { $(this).find("dd").hide(); }
			);
	//停销年款筛选 
	$("#carlist_nosaleyear li a").bind("click", function () {
		$("#data_tab_jq5_2").show().siblings("div[id^='data_tab_jq5']").hide();
		$("#carCompareFilter").hide();
		var yearP = $(this).attr("id");
		if ($("#noSaleTitle").length == 0) {
			$("#car_nosaleyearlist").before("<li id=\"noSaleTitle\"><a href=\"javascript:;\">停售车款</a></li>");
		}
		$("#car_nosaleyearlist .pop").html(yearP + "款<strong></strong>");
		$("#noSaleTitle").addClass("current").siblings().removeClass("current");
		if ($("#" + yearP).length > 0 && yearP.length > 0) {
			getNoSaleYearData(yearP);
		}
	});	if (typeof csSaleState != "undefined" && csSaleState == '停销') {
		//var lastYear = $("#carlist_nosaleyear ul li:first").find("a").attr("id");
		//getNoSaleYearData(lastYear);
		$("#carlist_nosaleyear ul li:first").find("a").trigger("click");
	}	function getNoSaleYearData(year) {
		$.ajax({
			url: "/AjaxNew/GetNoSaleSerailListByYear.ashx?csID=" + CarCommonCSID + "&year=" + year, dataType: "json", cache: true,
			success: DrawNoSaleContent
		});
	}	function DrawNoSaleContent(json) {
		if (json.length > 0) {
			//初始化车款列表        
			var divContentArray = new Array();
			divContentArray.push("<tbody>");
			$(json).each(function (index, item) {
				divContentArray.push("<tr id=\"car_filter_gid_" + index + "\">");
				var MaxPower = item.MaxPower;
				if (MaxPower != "") {
					MaxPower = "<b>/</b>" + MaxPower
				}
				divContentArray.push("<th width=\"40%\" class=\"first-item\"><div class=\"pdL10\"><strong>" + item.Engine_Exhaust + "</strong>" + MaxPower + " " + item.InhaleType + "</div>");
				divContentArray.push("</th>");
				if (index == 0) {
					divContentArray.push("<th width=\"8%\" class=\"pd-left-one\">关注度</th>");
					divContentArray.push("<th width=\"14%\" class=\"pd-left-one\">变速箱</th>");
					divContentArray.push("<th width=\"10%\" class=\"pd-left-two\">指导价</th>");
					divContentArray.push("<th width=\"10%\" class=\"pd-left-three\">二手车报价</th>");
					divContentArray.push("<th width=\"18%\">&nbsp;</th>");
				}
				else {
					divContentArray.push("<th width=\"8%\" class=\"pd-left-one\"></th>");
					divContentArray.push("<th width=\"14%\" class=\"pd-left-one\"></th>");
					divContentArray.push("<th width=\"10%\" class=\"pd-left-two\"></th>");
					divContentArray.push("<th width=\"10%\" class=\"pd-left-three\"></th>");
					divContentArray.push("<th width=\"18%\">&nbsp;</th>");
				}
				divContentArray.push("</tr>");

				for (var i = 0; i < item.carList.length; i++) {
					var stopPrd = "";
					var strEnergySubsidy = "";
					var strTravelTax = "";
					var fuelTypeStr = "";
					var referPrice = "暂无"
					if (item.carList[i].StopPrd == "停产") {
						stopPrd = "<span class=\"tingchan\">停产</span>";
					}
					if (item.carList[i].hasEnergySubsidy == "True") {
						strEnergySubsidy = "<a href=\"http://news.bitauto.com/others/20150605/1006534895.html\" class=\"butie\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
					}
					if (item.carList[i].isTravelTax == "True") {
						strTravelTax = " <a target=\"_blank\" title=\"" + item.carList[i].strTravelTax + "\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\" class=\"jianshui\">减税</a>";
					}

					if (item.carList[i].Oil_FuelType == "油电混合动力" || item.carList[i].Oil_FuelType == "油气混合动力") {
						fuelTypeStr = "<span class=\"hundong\">混动</span>";
					}
					if (item.carList[i].ReferPrice != "") {
						referPrice = item.carList[i].ReferPrice
					}
					divContentArray.push("<tr id=\"car_filter_id_" + item.carList[i].CarID + "\">");
					divContentArray.push("<td>");
					divContentArray.push("<div class=\"pdL10\" id=\"carlist_" + item.carList[i].CarID + "\"><a href=\"/" + item.carList[i].Spell + "/m" + item.carList[i].CarID + "/\" data-channelid=\"2.21.848\" target=\"_blank\">" + item.carList[i].YearType + " " + item.carList[i].Name + "</a> " + fuelTypeStr + strTravelTax + strEnergySubsidy + stopPrd + "</div>");
					divContentArray.push("</td>");
					divContentArray.push("<td>");
					divContentArray.push("<div class=\"w\">");

					divContentArray.push("<div class=\"p\" style=\"width: " + item.carList[i].Percent + "%\"></div>");
					divContentArray.push("</div>");
					divContentArray.push("</td>");

					divContentArray.push("<td>" + item.carList[i].ForwardGearNum + item.carList[i].TransmissionType + "</td>");
					divContentArray.push("<td style=\"text-align: right\"><span>" + referPrice + "</span><a data-channelid=\"2.21.852\" title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + item.carList[i].CarID + "\" target=\"_blank\"></a></td>");
					if (item.carList[i].UCarPrice == "")
						divContentArray.push("<td style=\"text-align: right\"><span>暂无报价</span></td>");
					else {
						var minPrice = item.carList[i].UCarPrice.split('-')[0];
						divContentArray.push("<td style=\"text-align: right\"><span><a href=\"http://www.taoche.com/all/?carid=" + item.carList[i].CarID + "&ref=car3\" target=\"_blank\">" + minPrice + "万</a></span></td>");

					}
					divContentArray.push("<td>");
					divContentArray.push("<div class=\"car-summary-btn-xunjia button_orange\"><a href=\"http://www.taoche.com/all/?carid=" + item.carList[i].CarID + "&ref=car3&leads_source=p002023\" data-channelid=\"2.21.851\" target=\"_blank\">二手车</a></div>");
					divContentArray.push("<div class=\"car-summary-btn-duibi button_gray\" id=\"carcompare_btn_new_" + item.carList[i].CarID + "\"><a target=\"_self\" data-channelid=\"2.21.850\" href=\"javascript:WaitCompareObj.AddCarToCompare('" + item.carList[i].CarID + "','" + item.carList[i].Name + "');\"><span>对比</span></a></div>");
					divContentArray.push("</td>");
					divContentArray.push("</tr>");
				}
			});
			divContentArray.push("</tbody>");
			var divContent = divContentArray.join("");
			$("#compare_nosale").html(divContent);

			$("#data_tab_jq5_2 tr[id^='car_filter_id_']").hover(
				function () {
					$(this).addClass('hover-bg-color');
				},
				function () {
					$(this).removeClass('hover-bg-color');
				}
				);
			Bglog_InitPostLog();
			//二手车点击统计代码  开始
			//$("#data_tab_jq5_2 div[class^=car-summary-btn-xunjia]").bind('click', function() {
			//    var objid = 10001;
			//    var _sentImg = new Image(1, 1);
			//    _sentImg.src = "http://carstat.bitauto.com/weblogger/img/c.gif?logtype=temptypestring&objid=" + objid + "&str1=" + encodeURIComponent("") + "&str2=" + encodeURIComponent("") + "&" + Math.random();
			//} );
			//二手车点击统计代码  结束
			window.setTimeout(function () { WaitCompareObj.ShowNameForMiniListWaitCompare() }, 500);
		}
	}
});