$(function () {
	var carSynData = {}, IE6 = ! -[1, ] && !window.XMLHttpRequest, IE7 = navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1;
	$("div[bit-seachmore] a[bit-serial]").bind("click", function () {
		var self = $(this);
		var serialId = $(this).attr("bit-serial");
		var allSpell = $(this).attr("bit-allspell");
		var carIds = $(this).attr("bit-car");
		var line = parseInt($(this).attr("bit-line"));
		var container = $("li[bit-line='" + (line + 1) + "']");
		if ($(this).parent().hasClass("seach_dow") && carSynData[serialId]) {
			if (IE6 || IE7) {
				container.hide(); self.parent().removeClass("seach_dow");
			}
			else
				container.slideUp(200, function () { self.parent().removeClass("seach_dow"); });
			return;
		}
		$(this).parent().addClass("seach_dow").closest("li").siblings().find(".seach_more.seach_dow").removeClass("seach_dow");

		if (carSynData[serialId]) {
			if (IE6 || IE7)
				container.hide().html(carSynData[serialId]).show().siblings(".c-list-2014.c-list-2014-pop").html('');
			else
				container.hide().html(carSynData[serialId]).slideDown(200).siblings(".c-list-2014.c-list-2014-pop").html('');
			$(".btn-close").click(function () { $(this).closest(".c-list-2014.c-list-2014-pop").hide().html(''); self.parent().removeClass("seach_dow"); });
			initCarListHover();
			// 对比浮动框 初始
			initCompareButton();
		} else {
			$.ajax({ url: "http://api.car.bitauto.com/CarInfo/GetCarListForSelectCar.ashx?serialId=" + serialId + "&carids=" + carIds, dataType: "jsonp", jsonpCallback: "callback", cache: true,
				beforeSend: function (xhr) {
					$("div[bit-line='" + (line + 1) + "']").html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\"><tbody><tr><th width=\"32%\" class=\"pdL10\">车款</th><th width=\"8%\" class=\"pd-left-one\">关注度</th><th width=\"10%\" class=\"pd-left-one\">变速箱</th><th width=\"10%\" class=\"pd-left-two\">指导价</th><th width=\"10%\" class=\"pd-left-three\">参考最低价</th><th width=\"18%\">&nbsp;</th></tr><tr><td colspan=\"6\"><div id=\"carlist_loading\" class=\"pdL10\">正在加载...</div></td></tr></tbody></table>");
					//$(".btn-close").click(function () { $(this).closest(".tool-filter-table").html(''); $(".tool-filter-car").removeClass("current"); });
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					//alert("textStatus: " + textStatus);
				},
				success: function (data) {
					if (data.CarList.length <= 0) { $("#carlist_loading").html("暂无车型数据"); return; }
					var content = [];

					content.push("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
					content.push("<tbody>");
					content.push("<tr>");
					content.push("<th width=\"40%\" class=\"pdL10\">车款</th>");
					content.push("<th width=\"11%\" class=\"pd-left-one\">关注度</th>");
					content.push("<th width=\"11%\" class=\"pd-left-one\">变速箱</th>");
					content.push("<th width=\"10%\" class=\"pd-left-two\">指导价</th>");
					content.push("<th width=\"10%\" class=\"pd-left-three\">参考最低价</th>");
					content.push("<th width=\"18%\">&nbsp;</th>");
					content.push("</tr>");
					$.each(data.CarList, function (i, n) {
						content.push("<tr>");
						content.push("<td>");
						var yearType = n.CarYear.length > 0 ? n.CarYear + "款" : "未知年款";
						var strState = n.ProduceState == "停产" ? " <span class=\"tingchan\">停产</span>" : "";
						content.push("<div class=\"pdL10\"><a href=\"/" + allSpell + "/m" + n.CarID + "/\" target=\"_blank\">" + yearType + " " + n.CarName + " </a>" + strState + "</div>");
						content.push("</td>");
						content.push("<td>");
						var percent = data.MaxPv > 0 ? (n.CarPV / data.MaxPv * 100.0) : 0;
						content.push("<div class=\"w\"><div class=\"p\" style=\"width:" + percent + "%\"></div></div>");
						content.push("</td>");
						var gearNum = (n.UnderPan_ForwardGearNum != "" && n.UnderPan_ForwardGearNum != "待查" && n.UnderPan_ForwardGearNum != "无级") ? n.UnderPan_ForwardGearNum + "挡" : ""
						content.push("<td>" + gearNum + n.TransmissionType + "</td>");
						content.push("<td style=\"text-align: right\"><span>" + n.ReferPrice + "万</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + n.CarID + "\" target=\"_blank\"></a></td>");
						//取最低报价
						var minPrice = n.CarPriceRange;
						if (minPrice.length <= 0)
						{ content.push("<td style=\"text-align: right\"><span>暂无报价</span></td>"); }
						else if (minPrice.indexOf("-") != -1) {
							minPrice = minPrice.substring(0, minPrice.indexOf('-'));
							content.push("<td style=\"text-align: right\"><span><a href=\"/" + allSpell + "/m" + n.CarID + "/baojia/\" target=\"_blank\">" + minPrice + "</a></span></td>");
						} else { content.push("<td style=\"text-align: right\"><span>" + minPrice + "</span></td>"); }
						content.push("<td>");
						content.push("<div class=\"car-summary-btn-xunjia button_gray\"><a href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + n.CarID + "/\" target=\"_blank\">询价</a></div><div class=\"car-summary-btn-duibi button_gray\" id=\"carcompare_btn_new_" + n.CarID + "\"><a target=\"_self\" href=\"javascript:addCarToCompare('" + n.CarID + "','" + n.CarName + "');\"><span>对比</span></a></div>");
						content.push("</td>");
						content.push("</tr>");
					});
					content.push("</tbody>");
					content.push("</table>");
					content.push("<a href=\"javascript:;\" class=\"btn-close\">关闭</a>");
					carSynData[serialId] = content.join('');
					if (IE6 || IE7) {
						$("li[bit-line='" + (line + 1) + "']").hide().html(content.join('')).show().siblings(".c-list-2014.c-list-2014-pop").html('');
					} else {
						$("li[bit-line='" + (line + 1) + "']").hide().html(content.join('')).slideDown(200).siblings(".c-list-2014.c-list-2014-pop").html('');
					}
					$(".btn-close").click(function () { $(this).closest(".c-list-2014.c-list-2014-pop").hide().html(''); self.parent().removeClass("seach_dow"); });
					// 对比浮动框 初始
					initCompareButton();
					initCarListHover();
				}
			});
		}
		return false;
	});
	//滑动效果
	function initCarListHover() {
		//车型列表效果
		$('li.c-list-2014.c-list-2014-pop tr').hover(
			function () {
				$(this).addClass('hover-bg-color');
				$(this).find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
				if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
					$(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
			},
			function () {
				$(this).removeClass('hover-bg-color');
				$(this).find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
				if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
					$(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
			}
		);
	}
});

 