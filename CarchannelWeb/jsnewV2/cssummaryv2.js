﻿//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}

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

//添加 取消关注车型
function FocusCar(obj) {
    Bitauto.Login.afterLoginDo(function () {
        obj.attr('class').indexOf("focused") == -1 ? Bitauto.UserCars.addConcernedCar(serialId, function () {
            if (Bitauto.UserCars.concernedcar.message[0] == "已超过上限") {
                $("#mangerCar_tc").attr("href", "http://i.yiche.com/u" + Bitauto.Login.result.userId + "/car/guanzhu/");
                $("#FocusCarFull").show();
            }
            else {
                $(obj).addClass("focused").html("已关注").attr("title", "取消关注");
                Bitauto.UserCars.concernedcar.arrconcernedcar.unshift(serialId);
            }
        }) : Bitauto.UserCars.delConcernedCar(serialId, function () {
            obj.attr("title", "点击关注").removeClass("focused").html("+ 关注");
        });
    });
};

(function () {
    var hoverSetTimeOut = null;
    //焦点区 效果
    $("#focus_color_box li").hover(function () {
        clearTimeout(hoverSetTimeOut);
        var index = $(this).index();
        $(this).addClass("current").siblings().removeClass("current");
        $("#focus_images > div[class!='ad_300_30'][id='focuscolor_" + (index + 1) + "']").show().siblings("div[class!='ad_300_30']").hide();
    }, function () {
        hoverSetTimeOut = setTimeout(function () {
            $("#color-listbox li").removeClass("current");
            $("#focus_images > div[class!='ad_300_30'][id='focus_image_first']").show().siblings("div[class!='ad_300_30']").hide();
        }, 100);
    });

    //颜色块 滑动效果
    var colorObj = $("#focus_color_box li");
    var wh = -colorObj.length * 31;
    var leftV = 0;
    if (colorObj.length > 7) {
        var ulWidth = 210;
        $("#focus_color_l").click(function () {
            if ($(this).hasClass("noclick")) return;
            leftV = leftV + ulWidth;
            if (leftV >= 0) {
                leftV = 0;
                $(this).addClass("noclick");
                $("#focus_color_r").removeClass("noclick");
                //$("#focus_color_r").addClass("a_r_hover");
            }
            else {
                $(this).removeClass("noclick");
            }
            $("#color-listbox").animate({ "left": leftV }, 300);
        });
        $("#focus_color_r").click(function () {
            if ($(this).hasClass("noclick")) return;
            leftV = leftV - ulWidth;
            $("#focus_color_l").removeClass("noclick");
            if (leftV - ulWidth < wh) {
                $(this).addClass("noclick");
            }
            
            if (leftV < wh) {
                $(this).addClass("noclick");
                leftV = leftV + ulWidth;
                return;
            }
            $("#color-listbox").animate({ "left": leftV }, 300);
        });
    }
})();

$(function () {//右侧边栏在页面底部，等页面加载完成才能注册事件
    //降价切换城市
        $("#mp-jiangjianews .ch-cur-city").click(function (ev) {
            var City_Select_ChangeCityList = ["NavCity", "NavCity2"];
            City_Select.InitOtherCity(City_Select_ChangeCityList);
        // 回调方法 function citySelectSuccess(city)
        //   {
        //       document.getElementById("NavCity").innerText = city.name;
        //}

        });
    
});

//名片区-经销商数量
function GetDealerData(serialSpell) {
    $.ajax({
        url: "http://cdn.partner.bitauto.com/CarSerialPriceInfo/Handler/GetCsPriceCommon.ashx?v=1&action=GetMaxFavorAndDealerCount&brandId=" + serialId + "&cityId=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetDealerDataCallback",
        success: function (data) {
            if (typeof data != "undefined" && data.length > 0) {
                if (data[0].DealerCount > 0) {
                    $("#mp-dealer").html("(" + data[0].DealerCount + "家本地经销商)").attr("href", data[0].dealerurl);
                }
                var favorablePrice = parseFloat(data[0].MaxFavorablePrice);
                if (favorablePrice > 0) {
                    $("#mp-jiangjia .desc a").html("直降" + favorablePrice.toFixed(2) + "万>");
                }
                else {
                    $("#mp-jiangjia .desc").addClass("grey-txt").html("暂无");
                }
            }
        }
    });
}
//名片区-二手车
function GetErShouCheMinPrice() {
    //http://yicheapi.taoche.cn/CarSourceInterface/ForJson/CarSerialPriceRangeByCityId.ashx?csid=1648&cityid=201
    $.ajax({
        url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/CarSerialPriceRangeByCityId.ashx?csid=" + serialId + "&cityId=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetErShouCheMinPriceCallback",
        success: function (data) {
            if (typeof data != "undefined") {
                if (isNaN(data.MinPrice)) {
                    $("#mp-ershouche-minprice").parent().addClass("grey-txt").html("暂无");
                }
                else {
                    $("#mp-ershouche-minprice").html(parseFloat(data.MinPrice) + "万起>");
                    $("#mp-ershouche a").attr("href", data.CarListUrl + "?ref=pc_yc_zs_gs_esc&leads_source=p002020");
                }
            }
            else {
                $("#mp-ershouche-minprice").parent().addClass("grey-txt").html("暂无");
            }
        }
    });
}

//限时特惠
function getBuyYch(data) {
    var h = [],
        lastIndex = data.PcUrl.lastIndexOf("/"),
        url = data.PcUrl +
            (data.PcUrl.substring(lastIndex).indexOf("?") > -1 ? "&" : "?") +
            "ref=car1&rfpa_tracker=1_7&leads_source=p002008";
    h.push("<div class=\"col-auto left\">");
    h.push("    <div class=\"caption\">");
    h.push("        <em class=\"em\">限时特惠</em>");
    h.push("    </div>");
    h.push("</div>");
    h.push("<div class=\"col-auto mid\">");
    h.push("    <h5><a href=\"" + url + "\" target=\"_blank\">" + data.Headline + "</a></h5>");
    h.push("    <div class=\"bottom\">");
    h.push("        <a class=\"btn btn-secondary btn-sm\" href=\"" + url + "\" target=\"_blank\">立即下单</a>");
    h.push("        <span class=\"info\">" + data.Description + "</span>");
    h.push("    </div>");
    h.push("</div>");
    h.push("<div class=\"col-auto right\">");
    h.push("    <a class=\"figure w90x60\" href=\"" + url + "\" target=\"_blank\">");
    h.push("        <span class=\"img\">");
    h.push("            <img src=\"" + data.Picture + "\" alt=\"\">");
    h.push("        </span>");
    h.push("        <h6>" + data.SmallDescription + "</h6>");
    h.push("        <p>" + data.TimeLimit + "</p>");
    h.push("    </a>");
    h.push("</div>");
    $("#gouche-ych").html(h.join('')).show();

}
//降价
function GetJiangjiaNews() {
    if (priceRang == "未上市") {
        return;
    }
    var mpjiangjia = $("#mp-jiangjianews");
    $(mpjiangjia).find(".cur-city").html(GlobalSummaryConfig.CityName);
    $.ajax({
        url: "http://m.h5.qiche4s.cn/jiangjiaapi/GetPromtionNews.ashx?op=carlist&count=4&csid=" + serialId + "&cid=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "getJiangJiaCallback",
        success: function (data) {
            var h = [];
            count = data.data.length;
            if (count > 0) {
                for (var jiangjiaCount = 0; jiangjiaCount < count; jiangjiaCount++) {
                    var obj = data.data[jiangjiaCount];

                    var youhuiStr = "",
                        //youhuiUrl= "",
                        className = "";
                    if (obj.IsJJ == 1) { //降价
                        if (obj.showtype == 1) {
                            youhuiStr = obj.FavorablePrice + "万";
                            className = "price-reduction";
                            //youhuiUrl = obj.zuidijiaUrl;
                        }
                        else { //送礼包
                            youhuiStr = "送礼包";
                            className = "price-reduction type1";
                            //youhuiUrl = obj.href;
                        }
                    }
                    else { //报价
                        var vendorPrice = parseFloat(obj.vendorPrice).toFixed(2);
                        youhuiStr = vendorPrice + "万";
                        className = "price-reduction type2";
                        //youhuiUrl = obj.zuidijiaUrl;
                    }
                    h.push("<li>");
                    h.push("<a href=\"" + obj.href + "?leads_source=p002005\" target=\"_blank\" class=\"txt\">" + obj.CarName + "</a>");
                    h.push("<a target=\"_blank\" href=\"" + obj.href + "?leads_source=p002005\" class=\"" + className + "\">" + youhuiStr + "</a>");
                    h.push("<a class=\"btn btn-primary btn-xs\" href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + obj.carId + "/?T=2&leads_source=p002011\" target=\"_blank\">询价</a>");
                    h.push("</li>");
                }
                $(mpjiangjia).find(".cont ul").html(h.join(""));
                $(mpjiangjia).find(".spl-22").html("<a href=\"http://car.bitauto.com/" + serialSpell + "/jiangjia/c" + cityId + "/?leads_source=p002005#\" target=\"_blank\">更多降价</a>").show();
            }
            else {
                $(mpjiangjia).find(".cont ul").html("<li>本地暂无降价信息</li>")
            }
        }
    });
}
//贷款首付
function GetDownPayment() {
    if ($("#mp-daikuan").length == 0) return;
    $.ajax({
        url: "http://carapi.daikuan.com/api/SummarizeFinancialProducts/GetSerialDefualtCarLoanInfo?cityId=" + GlobalSummaryConfig.CityId + "&serialId=" + GlobalSummaryConfig.SerialId + "&from=yc9&leads_source=p002003",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetDownPaymentCallback",
        success: function (data) {
            //var data = $.parseJSON(result);
            if (data != null && typeof data != "undefined" && typeof data.DownPayment != "undefined" && !isNaN(data.DownPayment) && parseFloat(data.DownPayment) > 0) {
                var downPayment = parseFloat(data.DownPayment);
                if (downPayment >= 100) {
                    downPayment = downPayment.toFixed(0);
                }
                else {
                    downPayment = downPayment.toFixed(2);
                }
                $("#mp-daikuan h5 a").attr("href", data.PcListUrl).html("首付" + downPayment + "万起>");
            }
            else {
                SetDefaultDownPayment();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            SetDefaultDownPayment();
        }
    });
}
//默认首付
function SetDefaultDownPayment() {
    var downPayment = $("#mp-daikuan").attr("downPayment");
    if (parseFloat(downPayment) > 0) {
        $("#mp-daikuan h5 a").html("首付" + downPayment + "万起>");
    }
    else {
        $("#mp-daikuan h5").html("暂无").addClass("grey-txt");
    }
}

//function InitJiangjiaHtml(h, count) {
//	var adCount = $("#mp-jiangjianews").find("li").length;
//	var endHtml = "</ul></div><div class=\"col-xs-6\"><ul class=\"list\">";
//	if (adCount > 0) {
//		var adHtml = $("#mp-jiangjianews").find("li");
//		for (var i = 0; i < adCount; i++) {
//			if (count + i == 3) {
//				h.push(endHtml);
//			}
//			h.push(adHtml[i].outerHTML);
//		}
//	}
//	$("#mp-jiangjianews").html(h.join(""));
//	if (typeof (Bglog_InitPostLog) != "undefined") {
//		Bglog_InitPostLog();
//	}
//}

function InitTeHuiAndAdData() {
    $.ajax({
        url: "http://api.mai.yiche.com/api/ProductCar/GetProductPush?csid=" + serialId + "&cityId=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetTeHuiAjaxCallback",
        success: function (tehuidata) {
            if (tehuidata && tehuidata.Success) {
                var obj = tehuidata.Result,
                    lastIndex = obj.PcUrl.lastIndexOf("/"),
                    url = obj.PcUrl + (obj.PcUrl.substring(lastIndex).indexOf("?") > -1 ? "&" : "?") + "ref=car1&rfpa_tracker=1_7&leads_source=p002008";

                //名片区
                if (typeof obj.Slogan != "undefined" && obj.Slogan != null && obj.Slogan != "") {
                    var html = "<a target=\"_blank\" data-channelid=\"2.21.1520\" href=\"" + url + "\" class=\"youhui\">" + obj.Slogan + "</a></h5>";
                    var youhuitag = $("#factory-price").find(".youhui");
                    if ($(youhuitag).length > 0) {
                        $(youhuitag).before(html);
                    }
                    else {
                        $("#factory-price").append(html);
                    }
                    //$("#mp-qianggou .note").append("<h5><a target=\"_blank\" data-channelid=\"2.21.1520\" href=\"" + url + "\">" + obj.Slogan + "</a></h5><a class=\"btn btn-default\" target=\"_blank\" data-channelid=\"2.21.99\" href=\"" + url + "\">立即抢购</a>");
                    //$("#mp-qianggou").show();
                }
                var xianshitehui = setInterval(function () {
                    if ($("#gouche-ych").length > 0) {
                        clearInterval(xianshitehui);
                        getBuyYch(tehuidata.Result);
                    }
                }, 100);
            }
        }
    });
}

function GetHmcJiangJia() {
    $.ajax({
        url: "http://platform.api.huimaiche.com/hmc/yc/v1/GetSerialCounselor.ashx?csid=" + serialId + "&ccode=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "getHmcCallback",
        success: function (data) {
            if (typeof data == "undefined") {
                return;
            }
            //if (parseFloat(data.savemoney) > 0) {
            //    $("#mp-dijia-savemoney") && $("#mp-dijia-savemoney").html("省" + data.savemoney + ">>");
            //    $("#mp-dijia") && $("#mp-dijia").show()
            //}
            //var hmcjiangjia = setInterval(function () {
            //	if ($("#gouche-hmc").length > 0) {
            //		clearInterval(hmcjiangjia);
            //		GetBuyHmc(data);
            //	}
            //}, 100);
        }
    });
}

//购车服务
function GetBuyHmc(data) {
    if (typeof data == "undefined") {
        return;
    }
    var h = [],
        url = data.pclink +
            (data.pclink.indexOf("?") > -1 ? "&" : "?") +
            "tracker_u=123_yccxfwl&leads_source=p002006";
    h.push("<div class=\"col-auto left\">");
    h.push("<div class=\"caption\">");
    h.push("<em class=\"em\">购车服务</em>");
    h.push("</div>");
    h.push("</div>");
    h.push("<div class=\"col-auto mid\">");
    h.push("<h5><a href=\"" + url + "\" target=\"_blank\">" + data.title + "</a></h5>");
    h.push("<div class=\"bottom\">");
    h.push("<a class=\"btn btn-secondary btn-sm\" href=\"" + url + "\" target=\"_blank\">马上买车</a>");
    h.push("<span class=\"info\">" + data.desc + "</span>");
    h.push("</div>");
    h.push("</div>");
    h.push("<div class=\"col-auto right type-1\">");
    h.push("<a class=\"figure\" href=\"" + url + "\" target=\"_blank\">");
    h.push("<span class=\"img\">");
    h.push("<img src=\"" + data.counselorimg + "\" alt=\"\">");
    h.push("</span>");
    h.push("<h6>" + data.counselorname + "</h6>");
    h.push("<p>" + data.counselordesc + "</p>");
    h.push("</a>");
    h.push("</div>");

    $("#gouche-hmc").html(h.join('')).show();

}

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

function FocusNews(totalCount, data) {
    var content = $("#focusNewsContent");
    var h = new Array();
    if ($(content).is("h3")) {
        h.push("<div class=\"col-auto list-txt-layout1 section-right\" data-channelid=\"2.21.1787\" id=\"focusNewsContent\"><h3 class=\"no-wrap\">");
        h.push("<a href=\"" + data.news[0].url + "\" target=\"_blank\">" + data.news[0].title + "</a></h3></div>");
        $(h.join("")).insertAfter(content);
        $(content).remove();
    }
    else {
        var time = data.news[0].publishTime;
        if (time.length > 9) {
            time = time.substr(5, 5);
        }
        h.push("<div class=\"txt\" data-channelid=\"2.21.1787\"><strong><a href=\"http://news.bitauto.com/list/cc1175/\" target=\"_blank\">行情</a>|</strong><a href=\"" + data.news[0].url + "\" target=\"_blank\">" + data.news[0].title + "</a></div><span>" + time + "</span>");
        var newsul = $(content).find("ul");
        var newsli = $(newsul).find("li");
        if ($(newsli).length == totalCount) {
            $(newsli).last().html(h.join(""));
        }
        else {
            $(newsul).append("<li>" + h.join("") + "</li>");
        }
    }
}
function FocuNewsForWaitSale(newsCount, data) {
    var content = $("#focusNewsContent");
    var h = new Array();

    if ($(content).siblings(".head").length > 0) {
        newsCount = 6;
    }
    var time = data.news[0].publishTime;
    if (time.length > 9) {
        time = time.substr(5, 5);
    }
    //h.push("<div class=\"txt\"><strong><a class=\"no-link\" target=\"_blank\">行情</a>|</strong><a href=\"" + data.News[0].url + "\" target=\"_blank\">" + data.News[0].title + "</a></div><span>" + time + "</span>");
    h.push("<div class=\"txt\" data-channelid=\"2.21.1787\"><strong><a href=\"http://news.bitauto.com/list/cc1175/\" target=\"_blank\">行情</a>|</strong><a href=\"" + data.news[0].url + "\" target=\"_blank\">" + data.news[0].title + "</a></div><span>" + time + "</span>");
    var newsli = $(content).find("li");
    if ($(newsli).length < newsCount) {
        if ($(newsli).length == 1) {
            if ($(newsli).find("a").html() == "暂无内容") {
                $(newsli).first().html(h.join(""));
            }
            else {
                $("<li>" + h.join("") + "</li>").insertAfter($(newsli).last());
            }
        }
    }
    else {
        $(newsli).last().html(h.join(""));
    }
}
//焦点新闻最后一条
function GetFocusNewsLast(csSaleState, totalCount) {
    var rurl = "http://api.admin.bitauto.com/news3/v1/news/get?categoryids=625&cityids=" + GlobalSummaryConfig.CityId + "&serialids=" + GlobalSummaryConfig.SerialId + "&pagesize=1";
    //var rurl = "http://api.admin.bitauto.com/news3/v1/news/get?categoryids=625&cityids=201&serialids=4751&pagesize=1";
    $.ajax({
        url: rurl,
        dataType: "jsonp",
        cache: true,
        jsonpCallback: "getfocusnewsback",
        success: function (data) {
            if (data.news.length > 0) {
                if (csSaleState == "待销") {
                    FocuNewsForWaitSale(totalCount, data);
                }
                else {
                    FocusNews(totalCount, data);
                }
            }
        }
    });
}

//双11入口
//function Get1111Entrance() {
//    if ($(".bmw-ad-link").length > 0) return;
//    $.ajax({
//        url: "http://api.mai.yiche.com/api/ProductCar/GetCs11?csid=" + GlobalSummaryConfig.SerialId,
//        cache: true,
//        dataType: "jsonp",
//        jsonpCallback: "Get1111EntranceCallback",
//        success: function (data) {
//            if (!data.Success || data.Result == null) {
//                //console.log(data.Msg);
//                return;
//            }
//            var html = [];
//            html.push("<div class=\"bmw-ad-link type-1\">");
//            html.push("<a href=\"" + data.Result.pcUrl + "\" target=\"_blank\" class=\"link\">" + data.Result.propagate + "</a >");
//            html.push("</div>");
//            $("#focus_images").before(html.join(""));
//        }
//    });
//}

function GetVr() {
    if ($("#focus_images").siblings("zs-vr") > 0) return;
    $.ajax({
        url: "http://webapi.photo.bitauto.com/photoApi/api/v1/Pano/GetAlbumList?ModelId=" + GlobalSummaryConfig.SerialId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetVrCallback",
        success: function (data) {
            if (data.Code != 0 || data.Data.Total == 0) {
                return;
            }
            $("#focus_images").parent().prepend("<a href=\"" + data.Data.DataList[0].Url + "\"  data-channelid=\"2.21.2213\" target=\"_blank\" class=\"zs-vr\">VR看全景</a>");
        }
    });
}
//限时抢ad
function LoadXianShiQiang() {
    $.ajax({
        url: "http://frontapi.easypass.cn/activityapi/TimeLimitRobbing/JsonpTimeLimitRobbingUrl?eplb_source=ebit&CityId=" + GlobalSummaryConfig.CityId + "&CsId=" + GlobalSummaryConfig.SerialId,
        cache: true,
        dataType: "jsonp",//text
        jsonpCallback: "LoadXianShiQiangCallback",
        success: function (data) {
            if (typeof data != "undefined" && data.IsTimeLimitRobbing == 1) {
                $("#factory-price").append("<a class=\"youhui type1\" target=\"_blank\" href=\"" + data.PcActivityUrl + "\">限时抢</a>");
            }
        }
    });
}