function changeTwoDecimal(x) {
	var f_x = parseFloat(x);
	if (isNaN(f_x))
	{ return x; }
	var f_x = Math.round(x * 100) / 100;
	var s_x = f_x.toString();
	var pos_decimal = s_x.indexOf('.');
	if (pos_decimal < 0) {
		pos_decimal = s_x.length;
		s_x += '.';
	}
	while (s_x.length <= pos_decimal + 2) {
		s_x += '0';
	}
	return s_x;
}
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
var firstShowDemandLink = false;
// 先取易集客没有的话取降价
function getDemandAndJiangJia(serialId, serialSpell, cityId) {
	//	if (typeof cityJsonData[cityId] == 'undefined' || cityJsonData[cityId] == null) {
	//		cityJsonData[cityId] = {};
	//		cityJsonData[cityId].hasDemand = false;
	var dateCache = new Date();
	var cacheFix = dateCache.getHours();
	var url = "http://m.h5.qiche4s.cn/temai/handler/ActiveCommonHandler.ashx?action=getcaractivebycityandserial&cityId=" + cityId + "&brandId=" + serialId + "";
	$.ajax({
		url: url, async: false, cache: true, dataType: "jsonp", jsonpCallback: "newDemandCallback", success:
			function (data) {
				var hasDemand = false;
				if (typeof data != 'undefined' && data != null
				&& typeof data.DealerCount != 'undefined' && data.DealerCount != null && data.DealerCount > 0)
				{ hasDemand = true; }
				if (hasDemand) {
					var cityName = "北京";
					if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined")
					{ cityName = cityIDMapName[cityId]; }
					var citySpell = "beijing";
					if (typeof cityIDMapSpell != 'undefined' && cityIDMapSpell != null && typeof cityIDMapSpell[cityId] != "undefined")
					{ citySpell = cityIDMapSpell[cityId]; }
					// 导航限时抢购
					//导航 第1次显示，再不变化
					if (!firstShowDemandLink) {
						//$("#thelogoID > div.class").after("<a class=\"ad-yichehui\" target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21006&city=" + citySpell + "\">限时特卖</a>");
						//$("#divDemandCsBut").html("<a target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21007&city=" + citySpell + "\">特卖</a>");
						//$("#divDemandCsBut").show();
						if (typeof data.CarList != 'undefined' && data.CarList != null) {
							$.each(data.CarList, function (i, n) {
								if (i >= 3) return;
								$("#carlist_" + n.CarID).append("<a target=\"_blank\"  href=\"" + n.pcUrl + "?leads_source=p002025#" + n.CarID + "\" class=\"ad-yichehui-list\">特卖</a>");
							});
						}
						// firstShowDemandLink = true;
					}
					firstShowDemandLink = true;
					var demandHtml = [], demandCount = 0;
					if (typeof data.DealerCount != 'undefined' && data.DealerCount != null)
					{ demandCount = data.DealerCount; }
					demandHtml.push("<div class=\"jiangjia_infobox\">");
					demandHtml.push("	<h5><a href=\"http://temai.yiche.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "\" target=\"_blank\">优惠信息</a></h5>");
					demandHtml.push("<div class=\"city_name\" id=\"city_name_boxhover\">");
					demandHtml.push("<a href=\"javascript:;\">" + cityName + "</a><em></em> <sub></sub>");
					demandHtml.push("<ul style=\"display: none;\" id=\"city_name_box\"><li><a href=\"javascript:;\" cv=\"201\">北京</a></li><li><a href=\"javascript:;\" cv=\"2601\">天津</a></li><li><a href=\"javascript:;\" cv=\"2401\">上海</a></li><li><a href=\"javascript:;\" cv=\"3001\">杭州</a></li><li><a href=\"javascript:;\" cv=\"3002\">宁波</a></li><li><a href=\"javascript:;\" cv=\"1501\">南京</a></li><li><a href=\"javascript:;\" cv=\"1502\">苏州</a></li><li><a href=\"javascript:;\" cv=\"101\">合肥</a></li><li><a href=\"javascript:;\" cv=\"501\">广州</a></li><li><a href=\"javascript:;\" cv=\"502\">深圳</a></li><li><a href=\"javascript:;\" cv=\"301\">福州</a></li><li><a href=\"javascript:;\" cv=\"601\">南宁</a></li><li><a href=\"javascript:;\" cv=\"1201\">武汉</a></li><li><a href=\"javascript:;\" cv=\"1001\">郑州</a></li><li><a href=\"javascript:;\" cv=\"1301\">长沙</a></li><li><a href=\"javascript:;\" cv=\"1601\">南昌</a></li><li><a href=\"javascript:;\" cv=\"3101\">重庆</a></li><li><a href=\"javascript:;\" cv=\"2501\">成都</a></li><li><a href=\"javascript:;\" cv=\"2301\">西安</a></li><li><a href=\"javascript:;\" cv=\"401\">兰州</a></li><li><a href=\"javascript:;\" cv=\"2901\">昆明</a></li><li><a href=\"javascript:;\" cv=\"901\">石家庄</a></li><li><a href=\"javascript:;\" cv=\"1101\">哈尔滨</a></li><li><a href=\"javascript:;\" cv=\"1701\">沈阳</a></li><li><a href=\"javascript:;\" cv=\"1708\">大连</a></li><li><a href=\"javascript:;\" cv=\"1401\">长春</a></li><li><a href=\"javascript:;\" cv=\"2101\">济南</a></li><li><a href=\"javascript:;\" cv=\"2102\">青岛</a></li><li><a href=\"javascript:;\" cv=\"2201\">太原</a></li><li><a href=\"javascript:;\" cv=\"1801\">呼和浩特</a></li></ul>");
					demandHtml.push("</div>");
					demandHtml.push("<p id=\"vendorCount\"></p>");
					demandHtml.push("</div>");
					demandHtml.push("<ul class=\"dealer_list2\">");
					var tempArray = new Array();
					if (typeof data.CarList != 'undefined' && data.CarList != null) {
						if (data.CarList.length > 0) {
							for (var i = 0; i < data.CarList.length > 0; i++) {
								if (tempArray.length < 3) {
									if (changeTwoDecimal(data.CarList[i].MaxRP) < 0.01
										&& typeof data.CarList[i].RP != 'undefined'
										&& data.CarList[i].RP != "") {
										tempArray.push("<span><a href=\"" + data.CarList[i].pcUrl
										+ "?leads_source=p002022#" + data.CarList[i].CarID + "\" target=\"_blank\">" + data.CarList[i].Name
										+ "</a></span><em class=\"bj-link\"><a target=\"_blank\" href=\"" + data.CarList[i].pcUrl
										+ "?leads_source=p002022#" + data.CarList[i].CarID + "\">" + data.CarList[i].RP + "万</a></em>");
									}
									else {
										tempArray.push("<span><a href=\"" + data.CarList[i].pcUrl
										+ "?leads_source=p002022#" + data.CarList[i].CarID + "\" target=\"_blank\">" + data.CarList[i].Name
										+ "</a></span><em><a target=\"_blank\" href=\"" + data.CarList[i].pcUrl
										+ "?leads_source=p002022#" + data.CarList[i].CarID + "\">" + changeTwoDecimal(data.CarList[i].MaxRP) + "万</a></em>");
									}
								}
							}
						}
					}
					if (tempArray.length > 0) {
						for (var i = 0; i < tempArray.length; i++) {
							if (i >= 3)
							{ break; }
							if (i == tempArray.length - 1) {
								demandHtml.push("<li class=\"last\">");
							}
							else { demandHtml.push("<li>"); }
							demandHtml.push(tempArray[i]);
							demandHtml.push("</li>");
						}
					}
					demandHtml.push("	</ul>");
					demandHtml.push("</div>");
					$("#jiangjia_box").html(demandHtml.join(""));
					$("#vendorCount").html("共有" + demandCount + "家");
					$("#city_name_boxhover").hover(function () { $(this).addClass("city_name_hover"); $("#city_name_box").show(); }, function () { $(this).removeClass("city_name_hover"); $("#city_name_box").hide(); });
					$("#city_name_box li a").bind("click", function (event) { event.preventDefault(); var cityId = $(this).attr("cv"); getDemandAndJiangJia(serialId, serialSpell, cityId); });
				}
				else {
					// 降价和经销商报价
					firstShowDemandLink = true;
					getDealerListNew(serialId, serialSpell, cityId);
				}
			}
	});

}
//获取经销商降价新闻
function getDealerListNew(serialId, serialSpell, cityId) {
	//http://m.h5.qiche4s.cn/jiangjiaapi/GetPromtionNews.ashx?cid=201&csid=3221&op=carlist
	$.ajax({
		url: 'http://m.h5.qiche4s.cn/jiangjiaapi/GetPromtionNews.ashx?csid=' + serialId + '&cid=' + cityId + '&op=carlist'
	, cache: true, dataType: "jsonp", jsonpCallback: "newDealerCallback", success:
	function (data) {
		if (typeof data == 'undefined' || data == null) return;
		var jiajiaHtml = [], newsCount = 0;
		//if (typeof data.count != 'undefined' && data.count != null)
		//{ newsCount = data.count; }
		var cityName = "北京";
		if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined")
		{ cityName = cityIDMapName[cityId]; }
		jiajiaHtml.push("<div class=\"jiangjia_infobox\">");
		jiajiaHtml.push("	<h5><a href=\"/" + serialSpell + "/jiangjia/\" target=\"_blank\">降价信息</a></h5>");
		jiajiaHtml.push("<div class=\"city_name\" id=\"city_name_boxhover\">");
		jiajiaHtml.push("<a href=\"javascript:;\">" + cityName + "</a><em></em> <sub></sub>");
		jiajiaHtml.push("<ul style=\"display: none;\" id=\"city_name_box\"><li><a href=\"javascript:;\" cv=\"201\">北京</a></li><li><a href=\"javascript:;\" cv=\"2601\">天津</a></li><li><a href=\"javascript:;\" cv=\"2401\">上海</a></li><li><a href=\"javascript:;\" cv=\"3001\">杭州</a></li><li><a href=\"javascript:;\" cv=\"3002\">宁波</a></li><li><a href=\"javascript:;\" cv=\"1501\">南京</a></li><li><a href=\"javascript:;\" cv=\"1502\">苏州</a></li><li><a href=\"javascript:;\" cv=\"101\">合肥</a></li><li><a href=\"javascript:;\" cv=\"501\">广州</a></li><li><a href=\"javascript:;\" cv=\"502\">深圳</a></li><li><a href=\"javascript:;\" cv=\"301\">福州</a></li><li><a href=\"javascript:;\" cv=\"601\">南宁</a></li><li><a href=\"javascript:;\" cv=\"1201\">武汉</a></li><li><a href=\"javascript:;\" cv=\"1001\">郑州</a></li><li><a href=\"javascript:;\" cv=\"1301\">长沙</a></li><li><a href=\"javascript:;\" cv=\"1601\">南昌</a></li><li><a href=\"javascript:;\" cv=\"3101\">重庆</a></li><li><a href=\"javascript:;\" cv=\"2501\">成都</a></li><li><a href=\"javascript:;\" cv=\"2301\">西安</a></li><li><a href=\"javascript:;\" cv=\"401\">兰州</a></li><li><a href=\"javascript:;\" cv=\"2901\">昆明</a></li><li><a href=\"javascript:;\" cv=\"901\">石家庄</a></li><li><a href=\"javascript:;\" cv=\"1101\">哈尔滨</a></li><li><a href=\"javascript:;\" cv=\"1701\">沈阳</a></li><li><a href=\"javascript:;\" cv=\"1708\">大连</a></li><li><a href=\"javascript:;\" cv=\"1401\">长春</a></li><li><a href=\"javascript:;\" cv=\"2101\">济南</a></li><li><a href=\"javascript:;\" cv=\"2102\">青岛</a></li><li><a href=\"javascript:;\" cv=\"2201\">太原</a></li><li><a href=\"javascript:;\" cv=\"1801\">呼和浩特</a></li></ul>");
		jiajiaHtml.push("</div>");
		jiajiaHtml.push("<p id=\"vendorCount\"><a href=\"/" + serialSpell + "/jiangjia/\" target=\"_blank\">查看更多</a></p>");
		jiajiaHtml.push("</div>");
		jiajiaHtml.push("<ul class=\"dealer_list2\">");
		var tempArray = new Array();
		if (typeof data.data != 'undefined' && data.data != null) {
			if (data.data.length > 0) {
				for (var i = 0; i < data.data.length > 0; i++) {
					if (tempArray.length < 3) {
						var isJiangJia = "0";
						if (typeof data.data[i].IsJJ != 'undefined' && data.data[i].IsJJ != null)
						{ isJiangJia = data.data[i].IsJJ; }
						var cssStr = "";
						var showInfo = "";
						if (isJiangJia == "1") {//降价信息
							var isPresent = "0";
							if (typeof data.data[i].isPresent != 'undefined' && data.data[i].isPresent != null)
							{ isPresent = data.data[i].isPresent; }
							if (isPresent == "1") {
								if (typeof data.data[i].Present != 'undefined' && data.data[i].Present != null) {
									showInfo = data.data[i].Present;
									cssStr = "class=\"bj-link\"";
								}
							}
							else {
								showInfo = changeTwoDecimal(data.data[i].FavorablePrice) + "万";
							}
						} else { //报价信息
							showInfo = changeTwoDecimal(data.data[i].vendorPrice) + "万";
							cssStr = "class=\"bj-link\"";
						}
						tempArray.push("<span><a href=\"" + data.data[i].href
					+ "?leads_source=p002005\" target=\"_blank\">" + data.data[i].CarName
					+ "</a></span><em " + cssStr + "><a target=\"_blank\" href=\"" + data.data[i].href
					+ "?leads_source=p002005\">" + showInfo + "</a></em>");
					}
				}
			}
		}
			//else if (typeof data.BJdata != 'undefined' && data.BJdata != null) {
			//	if (data.BJdata.length > 0) {
			//		for (var i = 0; i < data.BJdata.length > 0; i++) {
			//			if (tempArray.length < 3) {
			//				tempArray.push("<span><a href=\"" + data.BJdata[i].href
			//				+ "?leads_source=20008\" target=\"_blank\">" + data.BJdata[i].DealerName
			//				+ "</a></span><em class=\"bj-link\"><a href=\"" + data.BJdata[i].href
			//				+ "?leads_source=20008\" target=\"_blank\">" + changeTwoDecimal(data.BJdata[i].minPrice)
			//				+ "万-" + changeTwoDecimal(data.BJdata[i].maxPrice) + "万</a></em><a href=\"" + data.BJdata[i].href
			//				+ "?leads_source=20008#orderForm\" target=\"_blank\" class=\"link-xj\">询最低价&gt;&gt;</a>");
			//			}
			//		}
			//	}
			//}
		else { }
		if (tempArray.length > 0) {
			for (var i = 0; i < tempArray.length; i++) {
				if (i >= 3)
				{ break; }
				if (i == tempArray.length - 1) {
					jiajiaHtml.push("<li class=\"last\">");
				}
				else { jiajiaHtml.push("<li>"); }
				jiajiaHtml.push(tempArray[i]);
				jiajiaHtml.push("</li>");
			}
		} else {
			jiajiaHtml.push("<li class=\"last\"><span class=\"cGray\">暂无降价信息</span></li>");
		}
		jiajiaHtml.push("	</ul>");
		jiajiaHtml.push("</div>");
		$("#jiangjia_box").html(jiajiaHtml.join(""));
		//$("#vendorCount").html("共有" + newsCount + "家");
		$("#city_name_boxhover").hover(function () { $(this).addClass("city_name_hover"); $("#city_name_box").show(); }, function () { $(this).removeClass("city_name_hover"); $("#city_name_box").hide(); });
		$("#city_name_box li a").bind("click", function (event) { event.preventDefault(); var cityId = $(this).attr("cv"); getDemandAndJiangJia(serialId, serialSpell, cityId); });
	}
	});
}
//获取易车惠商品信息
function getSerialGoodsNew(serialId, serialName, imageUrl, cityId) {
	$.ajax({
		url: 'http://api.car.bitauto.com/Mai/GetSerialGoodsNew.ashx?serialid=' + serialId + '&cityid=' + cityId, cache: true, dataType: "jsonp", jsonpCallback: "mai_callback", success: function (data) {
			var rightHtml = [];
			if (!data.CarList || data.CarList.length <= 0) return;
			var minCarGoods = data.CarList[0];
			var goodsUrl = minCarGoods.GoodsUrl; // +"&WT.mc_id=car1";
			var carName = $("#carlist_" + data.CarList[0].CarId + " a:first").text();
			rightHtml.push("<div class=\"yichehui_box0\"><h3>易车惠</h3></div>");
			rightHtml.push("<div class=\"yichehui_text\">");
			rightHtml.push("<a href=\"" + goodsUrl + "\" target=\"_blank\"><img src=\"" + imageUrl + "\" width=180 height=120/></a>");
			rightHtml.push("<h4><a href=\"" + goodsUrl + "\" target=\"_blank\">" + serialName + "</a></h4>");
			rightHtml.push("<h5><a href=\"" + goodsUrl + "\" target=\"_blank\">" + carName + "</a></h5>");
			if (minCarGoods.MinBitautoPrice != minCarGoods.MinMarketPrice) {
				rightHtml.push("<p>原价：<del>" + minCarGoods.MinMarketPrice + "万</del></p>");
			}
			rightHtml.push("<p>现价：<strong><a href=\"" + goodsUrl + "\" target=\"_blank\">" + minCarGoods.MinBitautoPrice + "万</a></strong></p>");
			rightHtml.push("<p class=\"xiadan\"><a href=\"" + goodsUrl + "\" target=\"_blank\">马上下单</a></p>");
			rightHtml.push("<em><a href=\"http://mai.bitauto.com/\" target=\"_blank\">更多优惠车型，尽在易车惠&gt;&gt;</a></em>");
			rightHtml.push("</div>");
			rightHtml.push("</div>");
			//右侧
			$("#tejiache_box").show().html(rightHtml.join(""));
			//车型列表
			$.each(data.CarList, function (i, n) {
				// if (i >= 3) return;
				$("#carlist_" + n.CarId + " a:first").after(" <a target=\"_blank\" href=\"" + n.GoodsUrl + "\" class=\"ad-yichehui-list\">特惠</a>");
			});
			//导航
			$("#thelogoID > div.rank").before("<a class=\"ad-yichehui\" target=\"_blank\" href=\"" + goodsUrl + "\">易车惠有特价，立即查看&gt;&gt;</a>");
		}
	});
}
// 返现
function getSerialCashBack(serialId, serialName, cityId) {
	$.ajax({
		url: 'http://api.car.bitauto.com/Mai/GetSerialCashBacks.ashx?date=20140429&serialid=' + serialId + '&cityid=' + cityId, cache: true, dataType: "jsonp", jsonpCallback: "cashBack_callback", success: function (data) {
			//车型列表
			$.each(data.CarList, function (i, n) {
				// if (i >= 3) return;
				if (n.Url) {
					$("#carlist_" + n.CarId).append("&nbsp;<a target=\"_blank\" href=\"" + n.Url + "\" class=\"ad-yichehui-list\">底价</a>");
				}
			});
		}
	});
}
//直销
function getDirectSell(serialId, serialName, cityId, ref) {
	var refStr = (typeof (ref) != "undefined") ? "?source=" + ref : "";
	$.ajax({
		url: 'http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=' + serialId + '&cityid=' + cityId + '&d=' + (new Date()).Format("yyyyMMddhh"), cache: true, dataType: "jsonp", jsonpCallback: "getMallCallback", success: function (data) {
			if (data && data.CarList.length > 0) {
				//平行接口 优先易车惠，其他商城优先
				var flag = false;
				$.each(data.CarList, function (i, n) {
					if (n.CarType == 0) {
						flag = true;
					} else if (n.CarType == 1) {

					}
					if (flag) {
						$("#btnDirectSell").attr("href", "http://www.yichemall.com/car/detail/" + serialId + "/" + refStr).show();
					} else {
						getYichihuiInfo(serialId, cityId);
					}
				});
			}
			else {
				getYichihuiInfo(serialId, cityId);
			}
		}
	});
}

//包销平行进口
function getMallPartCar(serialId, serialSpell, cityId, ref) {
	var refStr = (typeof (ref) != "undefined") ? "?source=" + ref + "&leads_source=p002026" : "?leads_source=p002026";
	$.ajax({
		url: 'http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=' + serialId + '&cityid=' + cityId + '&d=' + (new Date()).Format("yyyyMMddhh"), cache: true, dataType: "jsonp", jsonpCallback: "getMallCallback", success: function (data) {
			if (data && data.CarList.length > 0) {
				var vendorCount = data.CarList.length;
				var flag = false;
				//var isParallelImports = true;//是否是平行进口
				var h = [];
				h.push("<div class=\"jiangjia_infobox\">");
				h.push("<a href=\"http://www.yichemall.com/car/detail/" + serialId + "/?source=100014&leads_source=p002021\" target=\"_blank\"><div class=\"tap-tit\"><span>直销<br>特卖</span></div></a>");
				h.push("<p id=\"vendorCount\"><a href=\"http://www.yichemall.com/car/detail/" + serialId + "/?source=100014\" target=\"_blank\">" + vendorCount + "个车款&gt;&gt;</a></p>");
				h.push("</div>");
				h.push("<ul class=\"dealer_list2\">");
				var loop = 0;
				$.each(data.CarList, function (i, n) {
					if (n.CarType == 0) {
						$("#btn-xunjia-" + n.CarId + " a").attr("href", "http://www.yichemall.com/car/detail/c_" + n.CarId + "/?source=yc-cxzs-onsale-1&leads_source=p002010").html("直销");

						if (loop < 3) {
							var strClass = (loop == 2) ? "class=\"last\"" : "";
							h.push("<li " + strClass + "><span><a href=\"http://www.yichemall.com/car/detail/c_" + n.CarId + "/?source=yc-cxzs-onsale-1&leads_source=p002021\" target=\"_blank\">" + n.CarName + "</a></span><em class=\"bj-link\"><a target=\"_blank\" href=\"http://www.yichemall.com/car/detail/c_" + n.CarId + "/?source=yc-cxzs-onsale-1&leads_source=p002021\">" + n.Price + "万</a></em></li>");
						}
						loop++;
						flag = true;
						//isParallelImports = isParallelImports && false;
					}
					else if (n.CarType == 1) {
						$("#carlist_" + n.CarId + "").append("<a href=\"http://www.yichemall.com/car/detail/c_" + n.CarId + "/?source=yc-cxzs-onsale-1\" target=\"_blank\" class=\"ico-shangchengtehui\">直销</a>");
						//isParallelImports = isParallelImports && true;
					}
				});
				h.push("</ul>");
				if (flag) {
					//$("#cardXunjia").attr("href", "http://www.yichemall.com/car/detail/" + serialId + "/?source=yc-cxzs-cjzx-1")
					//                                .html("去商城购买");
					//$("#cardCheDai").attr("href", "http://www.yichemall.com/car/detail/" + serialId + "/");
					//$("#btnDijia").hide();
					//$("#btnZhihuan").hide();
					//$("#car_tag li").eq(7).find("a").removeAttr("href").addClass("nolink");
					//if (ref != "yc-cxzs-year-1") {
					//    if (serialId != 2750) {
					//        $("#vendorInfo").hide();
					//    }
					//}
					//$("#gouche-container").hide();
					$("#jiangjia_box").addClass("sc-hot-card");
					$("#jiangjia_box").html(h.join(''));
					$("#btnDirectSell").attr("href", "http://www.yichemall.com/car/detail/" + serialId + "/" + refStr).show();
				} else {
					getDemandAndJiangJia(serialId, serialSpell, cityId);
					//if (typeof fillCarPart != undefined)
					//    fillCarPart(data, 2);
					getYichihuiInfo(serialId, cityId);
				}
			} else {
				getYichihuiInfo(serialId, cityId);
				getDemandAndJiangJia(serialId, serialSpell, cityId);
			}
		}
	});
}
//获取“易车惠”标签及车款信息
function getYichihuiInfo(serialId, cityId) {
	$.ajax({
		url: 'http://api.car.bitauto.com/mai/GetSerialYichehui.ashx?date=20160929&serialId=' + serialId + '&cityid=' + cityId + '', cache: true, dataType: "jsonp", jsonpCallback: "getYichehuiCallback", success: function (data) {
			if (data && data.CarList.length > 0) {
				//var desc = data.Desc;
				//var activityId = data.CarList[0].ActivityId;//活动ID
				//var lowestPriceCarId = data.CarList[0].CarId;//取当前车价最低款的车款ID
				var urlYichehui = data.CarList[0]["Url"] + "?leads_source=p002027";
				$("#btnDirectSell").attr("href", urlYichehui + tongJiEndUrlParam);  //车系综述页“优惠”:&ref=car3&rfpa_tracker=1_8     车系树形页"优惠 "：ref=tree1&rfpa_tracker=1_9
				$("#btnDirectSell").text("特惠").show();
			}
		}
	});
}

function getBuyHmc(serialId, cityId) {
	$.ajax({
		url: "http://platform.api.huimaiche.com/hmc/yc/v1/GetSerialCounselor.ashx?csid=" + serialId + "&ccode=" + cityId,
		cache: true,
		dataType: "jsonp",
		jsonpCallback: "getHmcCallback",
		success: function (data) {
			if (!data) return;
			var h = [],
				url = data.pclink +
					(data.pclink.indexOf("?") > -1 ? "&" : "?") +
					"tracker_u=123_yccxfwl&leads_source=p002006";
			h.push("<div class=\"gouche-wrap-tips\">");
			h.push("	购车服务");
			h.push("</div>");
			h.push("<div class=\"gouche-wrap-t\">");
			h.push("	<h5><a href=\"" + url + "\" target=\"_blank\">" + data.title + "</a></h5>");
			h.push("	<a href=\"" + url + "\" target=\"_blank\" class=\"gouche-go\">马上买车</a> " + data.desc + "");
			h.push("</div>");
			h.push("");
			h.push("<div class=\"gouche-wrap-img gouche-wrap-face\">");
			h.push("	<a href=\"" + url + "\" target=\"_blank\"><img class=\"people\" src=\"" + data.counselorimg + "\"></a>");
			h.push("	<div class=\"gouche-wrap-txt\">");
			h.push("		<h6><a href=\"" + url + "\" target=\"_blank\">" + data.counselorname + "</a></h6>");
			h.push("		<span>" + data.counselordesc + "</span>");
			h.push("	</div>");
			h.push("</div>");
			$("#gouche-hmc").html(h.join('')).show();
		}
	});
}
function getBuyYch(serialId, cityId) {
	$.ajax({
		url: "http://api.mai.yiche.com/api/ProductCar/GetProductPush?csid=" + serialId + "&cityId=" + cityId,
		cache: true,
		dataType: "jsonp",
		jsonpCallback: "getGoucheYchCallback",
		success: function (data) {
			if (!(data && data.Success)) return;
			var data = data.Result;
			var h = [],
				lastIndex = data.PcUrl.lastIndexOf("/"),
				url = data.PcUrl +
					(data.PcUrl.substring(lastIndex).indexOf("?") > -1 ? "&" : "?") +
					"ref=car1&rfpa_tracker=1_7&leads_source=p002008";
			h.push("<div class=\"gouche-wrap-tips\">");
			h.push("	限时特惠");
			h.push("</div>");
			h.push("<div class=\"gouche-wrap-t\">");
			h.push("	<h5><a href=\"" + url + "\" target=\"_blank\">" + data.Headline + "</a></h5>");
			h.push("	<a href=\"" + url + "\" target=\"_blank\" class=\"gouche-go\">立即下单</a> " + data.Description + "");
			h.push("</div>");
			h.push("<div class=\"gouche-wrap-img gouche-wrap-car\">");
			h.push("	<a href=\"" + url + "\" target=\"_blank\"><img src=\"" + data.Picture + "\" alt=\"\" width=\"90\" height=\"60\"></a>");
			h.push("	<div class=\"gouche-wrap-txt\">");
			h.push("		<h6>" + data.SmallDescription + "</h6>");
			h.push("		<span>" + data.TimeLimit + "</span>");
			h.push("	</div>");
			h.push("</div> ");
			$("#gouche-ych").html(h.join('')).show();
		}
	});
}
function getBuyLimit(serialId, cityId) {
	$.ajax({
		url: "http://m.h5.qiche4s.cn/temai/handler/ActiveCommonHandler.ashx?action=getcaractivefor990sum&brandId=" + serialId + "&cityId=" + cityId,
		cache: true,
		dataType: "jsonp",
		jsonpCallback: "getGoucheXscgCallback",
		success: function (data) {
			if (!(data && data.length > 0)) return;
			var data = data[0];
			var h = [],
				url = data.PcUrl + (data.PcUrl.indexOf("?") > -1 ? "&" : "?") + "leads_source=p002024";
			h.push("<div class=\"gouche-wrap-tips\">");
			h.push("	限时抢购");
			h.push("</div>");
			h.push("<div class=\"gouche-wrap-t\">");
			h.push("	<h5><a href=\"" + url + "\" target=\"_blank\">" + data.Title + "</a></h5>");
			h.push("	<a href=\"" + url + "\" target=\"_blank\" class=\"gouche-go\">我要报名</a> " + data.TitleDesc + "");
			h.push("</div>");
			h.push("<div class=\"gouche-wrap-img gouche-wrap-car\">");
			h.push("	");
			h.push("	<div class=\"gouche-wrap-time\">");
			h.push("		<h6>" + data.LeftDays + "</h6>");
			h.push("	</div>");
			h.push("</div>");
			$("#gouche-xscg").html(h.join('')).show();
		}
	});
}
