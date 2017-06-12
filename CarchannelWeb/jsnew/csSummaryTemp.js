// 易集客&降价
function changeTwoDecimalNew(x) {
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
var firstShowDemandLink = false;
// 先取易集客没有的话取降价
function getDemandAndJiangJia(serialId, serialSpell, cityId) {
	//	if (typeof cityJsonData[cityId] == 'undefined' || cityJsonData[cityId] == null) {
	//		cityJsonData[cityId] = {};
	//		cityJsonData[cityId].hasDemand = false;
	$.ajax({ url: 'http://api.car.bitauto.com/mai/GetSerialDemand.ashx?serialId=' + serialId + '&cityid=' + cityId
			, async: false, cache: true, dataType: "jsonp", jsonpCallback: "newDemandCallback", success:
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
						$("#thelogoID > div.class").after("<a class=\"ad-yichehui\" target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21006&city=" + citySpell + "\">限时抢购</a>");
						$("#divDemandCsBut").html("<a target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21007&city=" + citySpell + "\">抢购</a>");
						$("#divDemandCsBut").show();
						if (typeof data.CarList != 'undefined' && data.CarList != null) {
							$.each(data.CarList, function (i, n) {
								$("#carlist_" + n.CarID).append("&nbsp;<a target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21009&city=" + citySpell + "#" + n.CarID + "\" class=\"ad-yichehui-list\">抢购</a>");
							});
						}
						// firstShowDemandLink = true;
					}
					firstShowDemandLink = true;
					var demandHtml = [], demandCount = 0;
					if (typeof data.DealerCount != 'undefined' && data.DealerCount != null)
					{ demandCount = data.DealerCount; }
					demandHtml.push("<div class=\"jiangjia_infobox\">");
					demandHtml.push("	<h5><a href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "\" target=\"_blank\">优惠信息</a></h5>");
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
										tempArray.push("<span><a href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "#" + data.CarList[i].CarID
										+ "\" target=\"_blank\">" + data.CarList[i].Name
										+ "</a></span><em class=\"bj-link\"><a target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "#" + data.CarList[i].CarID
										+ "\">" + data.CarList[i].RP + "万</a></em>");
									}
									else {
										tempArray.push("<span><a href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "#" + data.CarList[i].CarID
										+ "\" target=\"_blank\">" + data.CarList[i].Name
										+ "</a></span><em><a target=\"_blank\" href=\"http://mai.bitauto.com/detail-" + serialId + ".html?leads_source=21008&city=" + citySpell + "#" + data.CarList[i].CarID
										+ "\">" + changeTwoDecimal(data.CarList[i].MaxRP) + "万</a></em>");
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
							else
							{ demandHtml.push("<li>"); }
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
	$.ajax({ url: 'http://jiangjia.bitauto.com/api/GetPromtionNews.ashx?csid=' + serialId + '&cid=' + cityId
	, cache: true, dataType: "jsonp", jsonpCallback: "newDealerCallback", success:
	function (data) {
		if (typeof data == 'undefined' || data == null) return;
		var jiajiaHtml = [], newsCount = 0;
		if (typeof data.count != 'undefined' && data.count != null)
		{ newsCount = data.count; }
		var cityName = "北京";
		if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined")
		{ cityName = cityIDMapName[cityId]; }
		jiajiaHtml.push("<div class=\"jiangjia_infobox\">");
		jiajiaHtml.push("	<h5><a href=\"/" + serialSpell + "/jiangjia/\" target=\"_blank\">降价信息</a></h5>");
		jiajiaHtml.push("<div class=\"city_name\" id=\"city_name_boxhover\">");
		jiajiaHtml.push("<a href=\"javascript:;\">" + cityName + "</a><em></em> <sub></sub>");
		jiajiaHtml.push("<ul style=\"display: none;\" id=\"city_name_box\"><li><a href=\"javascript:;\" cv=\"201\">北京</a></li><li><a href=\"javascript:;\" cv=\"2601\">天津</a></li><li><a href=\"javascript:;\" cv=\"2401\">上海</a></li><li><a href=\"javascript:;\" cv=\"3001\">杭州</a></li><li><a href=\"javascript:;\" cv=\"3002\">宁波</a></li><li><a href=\"javascript:;\" cv=\"1501\">南京</a></li><li><a href=\"javascript:;\" cv=\"1502\">苏州</a></li><li><a href=\"javascript:;\" cv=\"101\">合肥</a></li><li><a href=\"javascript:;\" cv=\"501\">广州</a></li><li><a href=\"javascript:;\" cv=\"502\">深圳</a></li><li><a href=\"javascript:;\" cv=\"301\">福州</a></li><li><a href=\"javascript:;\" cv=\"601\">南宁</a></li><li><a href=\"javascript:;\" cv=\"1201\">武汉</a></li><li><a href=\"javascript:;\" cv=\"1001\">郑州</a></li><li><a href=\"javascript:;\" cv=\"1301\">长沙</a></li><li><a href=\"javascript:;\" cv=\"1601\">南昌</a></li><li><a href=\"javascript:;\" cv=\"3101\">重庆</a></li><li><a href=\"javascript:;\" cv=\"2501\">成都</a></li><li><a href=\"javascript:;\" cv=\"2301\">西安</a></li><li><a href=\"javascript:;\" cv=\"401\">兰州</a></li><li><a href=\"javascript:;\" cv=\"2901\">昆明</a></li><li><a href=\"javascript:;\" cv=\"901\">石家庄</a></li><li><a href=\"javascript:;\" cv=\"1101\">哈尔滨</a></li><li><a href=\"javascript:;\" cv=\"1701\">沈阳</a></li><li><a href=\"javascript:;\" cv=\"1708\">大连</a></li><li><a href=\"javascript:;\" cv=\"1401\">长春</a></li><li><a href=\"javascript:;\" cv=\"2101\">济南</a></li><li><a href=\"javascript:;\" cv=\"2102\">青岛</a></li><li><a href=\"javascript:;\" cv=\"2201\">太原</a></li><li><a href=\"javascript:;\" cv=\"1801\">呼和浩特</a></li></ul>");
		jiajiaHtml.push("</div>");
		jiajiaHtml.push("<p id=\"vendorCount\"></p>");
		jiajiaHtml.push("</div>");
		jiajiaHtml.push("<ul class=\"dealer_list2\">");
		var tempArray = new Array();
		if (typeof data.JJdata != 'undefined' && data.JJdata != null) {
			if (data.JJdata.length > 0) {
				for (var i = 0; i < data.JJdata.length > 0; i++) {
					if (tempArray.length < 3) {
						var isPresent = "0";
						if (typeof data.JJdata[i].isPresent != 'undefined' && data.JJdata[i].isPresent != null)
						{ isPresent = data.JJdata[i].isPresent; }
						var showInfo = "";
						if (isPresent == "1") {
							if (typeof data.JJdata[i].Present != 'undefined' && data.JJdata[i].Present != null) {
								showInfo = data.JJdata[i].Present;
							}
						}
						else {
							showInfo = changeTwoDecimal(data.JJdata[i].FavorablePrice) + "万";
						}
						tempArray.push("<span><a href=\"" + data.JJdata[i].href
					+ "?leads_source=20008\" target=\"_blank\">" + data.JJdata[i].DealerName
					+ "</a></span><em><a target=\"_blank\" href=\"" + data.JJdata[i].href
					+ "?leads_source=20008\">" + showInfo + "</a></em>");
					}
				}
			}
		}
		else if (typeof data.BJdata != 'undefined' && data.BJdata != null) {
			if (data.BJdata.length > 0) {
				for (var i = 0; i < data.BJdata.length > 0; i++) {
					if (tempArray.length < 3) {
						tempArray.push("<span><a href=\"" + data.BJdata[i].href
						+ "?leads_source=20008\" target=\"_blank\">" + data.BJdata[i].DealerName
						+ "</a></span><em class=\"bj-link\"><a href=\"" + data.BJdata[i].href
						+ "?leads_source=20008\" target=\"_blank\">" + changeTwoDecimal(data.BJdata[i].minPrice)
						+ "万-" + changeTwoDecimal(data.BJdata[i].maxPrice) + "万</a></em>");
					}
				}
			}
		}
		else
		{ }
		if (tempArray.length > 0) {
			for (var i = 0; i < tempArray.length; i++) {
				if (i >= 3)
				{ break; }
				if (i == tempArray.length - 1) {
					jiajiaHtml.push("<li class=\"last\">");
				}
				else
				{ jiajiaHtml.push("<li>"); }
				jiajiaHtml.push(tempArray[i]);
				jiajiaHtml.push("</li>");
			}
		} else {
			jiajiaHtml.push("<li class=\"last\"><span class=\"cGray\">暂无降价信息</span></li>");
		}
		jiajiaHtml.push("	</ul>");
		jiajiaHtml.push("</div>");
		$("#jiangjia_box").html(jiajiaHtml.join(""));
		$("#vendorCount").html("共有" + newsCount + "家");
		$("#city_name_boxhover").hover(function () { $(this).addClass("city_name_hover"); $("#city_name_box").show(); }, function () { $(this).removeClass("city_name_hover"); $("#city_name_box").hide(); });
		$("#city_name_box li a").bind("click", function (event) { event.preventDefault(); var cityId = $(this).attr("cv"); getDemandAndJiangJia(serialId, serialSpell, cityId); });
	}
	});
}