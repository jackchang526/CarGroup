/// <reference path="csSummary.js" />
/// <reference path="CommonJs.js" />
/// <reference path="csSummary.js" />
var tempSerialId = 0, tempSerialName = "";
//var cityJson = { 201: " 北京", 2601: " 天津", 2201: " 太原", 901: " 石家庄", 910: " 保定", 902: " 唐山", 1801: " 呼和浩特", 2101: " 济南", 2102: " 青岛", 2109: " 淄博", 2103: " 烟台", 1101: "哈尔滨", 1102: "大庆", 1401: "长春", 1701: "沈阳", 1708: "大连", 2401: "上海", 3001: "杭州", 3003: "温州", 3002: "宁波", 3006: "金华", 1501: "南京", 1502: "苏州", 1518: "徐州", 1503: "无锡", 101: "合肥", 1001: "郑州", 1002: "洛阳", 1201: "武汉", 1207: "宜昌", 1301: "长沙", 1601: "南昌", 501: "广州", 502: "深圳", 504: "东莞", 518: "佛山", 601: "南宁", 801: "海口", 301: "福州", 302: "厦门", 307: "泉州", 401: "兰州", 1901: "银川", 2301: "西安", 2801: "乌鲁木齐", 3101: "重庆", 701: "贵阳", 2501: "成都", 2901: "昆明" };
Array.prototype.indexOf || (Array.prototype.indexOf = function (value) {
    for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i;
    return -1;
});

$("a.btn-menu span").on("click", function (event) {
    event.preventDefault();
    var menupop = $(this).parent().siblings("div.menu-pop");
    if ($(menupop).css("display") == "none") {
        $(menupop).css("display", "block");
        $(this).parent("a.btn-menu").addClass("btn-menu-show");
    } else {
        $(menupop).css("display", "none");
        $(this).parent("a.btn-menu").removeClass("btn-menu-show");
    }
});

$(document).on('touchstart', function (event) {
    event.stopPropagation();
    var clickEle = $(event.target).closest(".menu-pop").attr("id");
    if (clickEle != 'h_popNav' && $(event.target).closest(".btn-menu").attr("id") != "h_nav") {
        $("#h_popNav").hide();
        $("#h_nav").removeClass("btn-menu-show");
    }
});

$(document).ready(function () {
    var documentHeight = $(document).height();
    $('#pSearch').css('height', documentHeight);
});

(function () {
    var mCsSummaryV2 = (function () {
        var s = this;
        //行情新闻
        s.loadNewsHangqingV2 = function (serialId, serialName, cityId, count) {
            $.ajax({
                url: "http://m.h5.qiche4s.cn/jiangjiaapi/GetPromtionNews.ashx?csid=" + serialId + "&cid=" + cityId + "&op=promotionnews&count=4", cache: true, dataType: "jsonp", jsonpCallback: "getHQCallback", success: function (data) {
                    var h = [];
                    if (data && data.length > 0) {
                        $.each(data, function (i, n) {
                            //var strClass;
                            //if (i >= 3) { strClass = "style=\"display:none;\""; }
                            //h.push("<li class=\"news-img-list\" " + strClass + ">");
                            h.push("<li class=\"news-img-list\">");
                            h.push("    <a href=\"" + n.URL + "\">");
                            h.push("        <h4 class=\"h25\">" + n.newsTitle + "</h4>");
                            h.push("        <em><span>" + n.NewsPubTime + "</span><span></span></em>");
                            h.push("    </a>");
                            h.push("</li>");
                        });
                    } else {
                        //h.push("<div class=\"none-con-list\">暂无降价行情</div>");
                        $("#no-hangqingmsg").show();
                        //$(".hangqing-box").hide();
                    }
                    $("#carnews1").html(h.join(''));
                }
            });
        };
        s.SortEmTags = function (carId, newTag) { //车款标签排序
            var tagJson = { "团购": 0, "混动": 0, "减税": 0, "补贴": 0, "免税": 0, "停产在售": 0, "停售": 0 };
            var emTags = $("#carlist_" + carId + " .car-info-bottom em");
            tagJson[newTag] = 1;
            if (emTags.length > 0)
            {
                for (var i = 0, length = emTags.length; i < length; i++) {
                    tagJson[$(emTags[i]).html()] = 1;
                }
            }
            var tagHtml = new Array();
            var tagCount = 0;
            for (var key in tagJson) {
                if (tagJson[key] == 1 && tagCount <2) {
                    tagCount++;
                    tagHtml.push("<em>"+key+"</em>");
                }
            }
            $(emTags).remove();
            $("#carlist_" + carId + " .gzd-box").after(tagHtml.join(""));
        };
        s.loadSubsidy = function (serialId, cityId) {
            var obj = this;
            $.ajax({
                url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + serialId + "&cityid=" + cityId + "",
                dataType: "jsonp",
                jsonpCallback: "getSubsidyCallback",
                cache: true,
                success: function (data) {
                    if (!(data && data.length > 0)) return;
                    $.each(data, function (i, n) {
                        if (!(n.StateSubsidies > 0 && n.LocalSubsidy > 0)) return;
                        obj.SortEmTags(n.CarId, "补贴");
                    });
                }
            });
        };
        s.loadHotNewsLast = function (serialId, cityId) {//热点新闻最后一条，替换成易车惠的
            var content = $("#m_article");
            if ($(content).length == 0) {
                return;
            }
            //var rurl = "http://api.admin.bitauto.com/news3/v1/news/get?categoryids=625&cityids=" + cityId + "&serialids=" + serialId + "&pagesize=1";
            //$.ajax({
            //    url: rurl,
            //    dataType: "jsonp",
            //    cache: true,
            //    jsonpCallback: "getfocusnewsback",
            //    success: function (data) {
            //        if (data.news.length == 0) {
            //            return;
            //        }
            //        var h = new Array();
            //        var time = data.news[0].publishTime;
            //        if (time.length >= 10) {
            //            time = time.substr(0, 10);
            //        }
            //        var img = data.news[0].imageCoverUrl;
            //        if (img.length > 0) {
            //            img = img.replace("_3.", "_1.");
            //        }
            //        h.push("<a href=\"" + data.news[0].url + "\" data-channelid=\"27.23.1788\">");
            //        if (img.length > 0) {
            //            h.push("<div class=\"img-box\"><span><img src=\"" + img + "\"></span></div>");
            //        }
            //        h.push("<div class=\"con-box\"><h4>" + data.news[0].title + "</h4><em><span>" + time + "</span><span>" + data.news[0].author + "</span><i class=\"ico-comment huifu comment_0_6583989\">" + data.news[0].pv + "</i></em></div></a>");

            //        var newsli = $(content).find("li");
            //        var newscount = $(newsli).length;
            //        if (newscount == 6) {
            //            $(newsli).last().html(h.join(""));
            //        }
            //        else {
            //            if (newscount < 3) {
            //                h.splice(0, 0, "<li" + (img.length > 0 ? "" : " class=\"news-noimg\"") + ">");
            //                h.push("</li>");
            //            }
            //            else {
            //                h.splice(0, 0, "<li style=\"display:none;\" " + (img.length > 0 ? "" : " class=\"news-noimg\"") + ">");
            //                h.push("</li>");
            //            }
            //            $(content).find("ul").append(h.join(""));
            //            if ($(content).find("li").length > 3 && $("#btn-hot-more").length == 0) {
            //                $(content).find(".btn-more").remove();
            //                $(content).append("<a href=\"javascript:void(0);\" id=\"btn-hot-more\" class=\"btn-more btn-add-more\"><i>加载更多</i></a>");
            //                s.init();
            //            }
            //        }
            //        Bglog_InitPostLog();
            //    }
            //});
        }
        //初始化页面
        s.init = function () {
            //绑定事件
            $("#btn-hot-more,#btn-pingce-more,#btn-daogou-more").one("click", function () {
                $(this).prev().find("ul li:gt(2)").show();
                typeof swiperNewslist.updateAutoHeight == "function" && swiperNewslist.updateAutoHeight();
                $(this).hide();
                var curBtnStr = $(this).attr("id");
                var curMoreLink = '';
                if (curBtnStr.indexOf("hot") > -1)
                    curMoreLink = 'wenzhang';
                else if (curBtnStr.indexOf("pingce") > -1)
                    curMoreLink = 'pingce';
                else if (curBtnStr.indexOf("daogou") > -1)
                    curMoreLink = 'daogou';
                $(this).after("<a href=\"http://car.m.yiche.com/" + serialAllSpell + "/" + curMoreLink + "/\"  class=\"btn-more\"><i>查看更多</i></a>");
            });
        };
        s.loadHotNewsLast(GlobalSummaryConfig.SerialId, GlobalSummaryConfig.CityId);
        s.init();
        return s;
    })();
    window.mCsSummaryV2 = mCsSummaryV2;
})();
if (typeof (module) !== 'undefined') {
    module.exports = window.mCsSummaryV2;
}
else if (typeof define === 'function' && define.amd) {
    define([], function () {
        'use strict';
        return window.mCsSummaryV2;
    });
}

//$(function () {
//    typeof swiperNewslist.updateAutoHeight == "function" && swiperNewslist.updateAutoHeight();//热点新闻，最后一个异步加载显示惠买车，加载完成，重新计算高度
//});

//加载降价新闻（最新行情）

//function loadNewsHangqingByCity(serialId, serialName, cityId, count) {
//	if (typeof serialId == "undefined" || serialId <= 0)
//		return;
//	if (count)
//		count = 4;
//	tempSerialId = serialId;
//	tempSerialName = serialName;
//	var url = "http://api.car.bitauto.com/newsinfo/getjiangjianews.ashx?iswireless=1&id=" + serialId + "&cityid=" + cityId + "&top=" + count;
//	loadJS.push(url, 'utf-8', jsonpCallBack);
//}

function loadViewedCars(serialIdList) {
    if (typeof serialIdList == "undefined" || serialIdList <= 0)
        return;
    var url = "http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csIDList=" + serialIdList + "&op=getviewedcar";
    loadJS.push(url, 'utf-8', jsonpCallBackForViewed);
}

function jsonpCallBackForViewed(data) {
    var viewed = document.getElementById("more1");
    if (viewed) {
        var tempHtml = typeof jjviewed == "undefined" ? "<div class=\"none-con-list\">无最近浏览</div>" : getViewedCarHtml(jjviewed);
        viewed.innerHTML = tempHtml;
    }
}

////回调函数

//function jsonpCallBack() {
//	var hangqing = document.getElementById("carnews1");
//	if (hangqing) {
//		var tempHtml = typeof jjnews == "undefined" ? "<div class=\"none-con-list\">暂无降价行情</div>" : getHangqingHtml(jjnews);
//		hangqing.innerHTML = tempHtml;
//		showHangqing();
//	}
//}

//function jsonpCallBackForSelect() {
//	location.hash = "";
//	var hangqing = document.getElementById("carnews1");
//	if (hangqing) {
//		var tempHtml = typeof jjnews == "undefined" ? "<div class=\"none-con-list\">暂无降价行情</div>" : getHangqingHtml(jjnews);
//		hangqing.innerHTML = tempHtml;
//		showHangqing();
//		location.hash = "hash_hangqing";
//	}
//}

//function showHangqing() {
//	var container = document.getElementById("container");
//	if (attrStyle(container, "display") == "none") {
//		container.style.display = "block";
//		var container_city = document.getElementById("container_city");
//		if (container_city) {
//			container_city.style.display = "none";
//			container_city.innerHTML = "";
//		}
//	}
//}

function getViewedCarHtml(jjviewed) {
    var html = [];
    if (typeof jjviewed["nlist"] != 'undefined'
        && jjviewed["nlist"] != null
        && jjviewed["nlist"].length > 0) {
        html.push("<dl>");
        html.push("<dt>最近看过：</dt>");
        for (var i = 0; i < jjviewed["nlist"].length; i++) {
            var viewedValue = eval(jjviewed["nlist"][i]);
            if (viewedValue == null) continue;
            var spell = viewedValue.CsAllSpell;
            var title = viewedValue.CsName;
            var image = viewedValue.CsImage;
            var price = viewedValue.CsPrice == "" ? "暂无报价" : viewedValue.CsPrice;
            html.push("<dd><a href='" + spell + "'>" + title + "</a></dd>");
        }
        html.push("</dl>");
    } else {
        html.push("<dl><dt>最近看过：</dt><dd>暂无浏览记录</dd></dl>");
    }
    return html.join("");
}

//function getHangqingHtml(jjnews) {
//	var html = [];
//	if (typeof jjnews["nlist"] != 'undefined'
//        && jjnews["nlist"] != null
//        && jjnews["nlist"].length > 0) {
//		html.push("<ul>");
//		for (var i = 0; i < jjnews["nlist"].length; i++) {
//			var newsValue = jjnews["nlist"][i];
//			var nvlist = newsValue.split(',');
//			if (nvlist == null || nvlist.length < 1) continue;
//			var title = decodeURIComponent(nvlist[0]);
//			var purl = decodeURIComponent(nvlist[1]);
//			var time = typeof nvlist[3] == "undefined" ? nvlist[2] : nvlist[3];
//			//去掉行情图片
//			//var image = typeof nvlist[4] == "undefined" ? "" : "<div class=\"img-box\"><img src=" + decodeURIComponent(nvlist[4]) + "></div>";
//			html.push("				<li><a href=\"" + purl + "\"><p>" + title + "</p><em>" + time + "</em></a></li>");
//		}
//		html.push("</ul>");
//	} else {
//		html.push("<div class=\"none-con-list\">本地暂无降价行情</div>");
//	}
//	return html.join("");
//}

//选择城市

//function selectCity() {
//    var container_city = document.getElementById("container_city");
//    var top = $("#m-tabs-article").offset().top;

//    BitAjax({
//        url: "/handlers/GetCityHTML.ashx",
//        cache: false,
//        success: function (data) {
//            if (data != "") {
//                container_city.innerHTML = data;

//                $(".leftPopup").css("zIndex", 199);
//                location.hash = "hash_hangqing";

//                //////////////////////
//                $('[data-action=popup-city]').rightSwipe({
//                    clickEnd: function (b) {
//                        var $leftPopup = this;
//                        if (b) {
//                            var $back = $('.' + $leftPopup.attr('data-back'));
//                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
//                            var $swipeLeft = $leftPopup.find('.swipeLeft');

//                            $leftPopup.myScroll = new IScroll($swipeLeft[0], {
//                                probeType: 1,
//                                snap: 'li',
//                                momentum: true,
//                                click: true
//                            });
//                            // console.log(myScroll);
//                            // $leftPopup.toucheContent({ tt:'.first-list' });
//                            //setTimeout(function () { $back.trigger('close') }, 2000)
//                        } else {
//                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
//                            // var $content = $leftPopup.find('.first-list');
//                            // $content.addClass3('transform', 'translateY(0px)');
//                        }
//                    }
//                });

//                $('[data-action^=popup-city]').rightSwipe({
//                    zIndex: 10000000,
//                    clickEnd: function (b) {
//                        var $leftPopup = this;
//                        if (b) {
//                            var $back = $('.' + $leftPopup.attr('data-back'));
//                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
//                            var $swipeLeft = $leftPopup.find('.swipeLeft');

//                            $leftPopup.myScroll = new IScroll($swipeLeft[0], {
//                                probeType: 1,
//                                snap: 'li',
//                                momentum: true,
//                                click: true
//                            });

//                            $leftPopup.find('.sub-return').on('click', function (ev) {
//                                ev.preventDefault();
//                                $leftPopup.children().removeClass('swipeLeft-block');
//                                setTimeout(function () { $leftPopup.css('z-index', 0).hide(); }, 200);
//                            });

//                        } else {
//                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
//                        }
//                    }
//                });
//                $("#city_title").trigger("click");
//                ///////////////////////////
//            }
//        }
//    });
//}

//切换城市

function changeCity(pvcId, cityId, cityName) {
    var title = document.getElementById("city_title");
    if (title)
        title.innerHTML = cityName + "<i></i>";
    var url = "http://api.car.bitauto.com/newsinfo/getjiangjianews.ashx?iswireless=1&id=" + tempSerialId + "&cityid=" + cityId + "&top=" + 4;
    loadJS.push(url, 'utf-8', jsonpCallBackForSelect);
    $(".swipeLeft").removeClass("swipeLeft-block");
    $(".leftmask").hide();
    $(".leftPopup").css("zIndex", 0);
    $(".leftPopup").hide();
    $("#popArea").show();
    document.documentElement.scrollTop = 0;
    document.body.scrollTop = 0;
}

//返回综述页
function goSummary() {
    var container = document.getElementById("container");
    var container_city = document.getElementById("container_city");
    if (container)
        container.style.display = "block";
    if (container_city) {
        container_city.style.display = "none";
        container_city.innerHTML = "";
    }
}

//function attrStyle(elem, attr) {
//	if (!elem) {
//		return;
//	}
//	if (elem.style[attr]) {
//		return elem.style[attr];
//	} else if (elem.currentStyle) {
//		return elem.currentStyle[attr];
//	} else if (document.defaultView && document.defaultView.getComputedStyle) {
//		attr = attr.replace(/([A-Z])/g, '-$1').toLowerCase();
//		return document.defaultView.getComputedStyle(elem, null).getPropertyValue(attr);
//	} else {
//		return null;
//	}
//}

var firstShowDemandLink = false;
// 特卖

function getDemandAndJiangJia(serialId, serialSpell, cityId) {
    $.ajax({
        url: 'http://api.car.bitauto.com/mai/GetSerialDemand.ashx?serialId=' + serialId + '&cityid=' + cityId,
        async: false,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "newDemandCallback",
        success:
        function (data) {
            var hasDemand = false;
            if (typeof data != 'undefined' && data != null
                && typeof data.DealerCount != 'undefined' && data.DealerCount != null && data.DealerCount > 0) {
                hasDemand = true;
            }
            if (hasDemand) {
                var cityName = "北京";
                if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined") {
                    cityName = cityIDMapName[cityId];
                }
                var citySpell = "beijing";
                if (typeof cityIDMapSpell != 'undefined' && cityIDMapSpell != null && typeof cityIDMapSpell[cityId] != "undefined") {
                    citySpell = cityIDMapSpell[cityId];
                }
                // 导航限时抢购
                //导航 第1次显示，再不变化
                if (!firstShowDemandLink) {
                    //			            $("#liDemand").html("<a href=\"http://mai.m.yiche.com/detail-" + serialId + ".html?leads_source=21101&city=" + citySpell + "\">特卖</a>");
                    //			            $("#liDemand").show();
                    //去掉车型特卖
                    //			            if (typeof data.CarList != 'undefined' && data.CarList != null) {
                    //			                $.each(data.CarList, function (i, n) {
                    //			                    $("em[id^='carlist_" +  n.CarID + "']").show();
                    //			                });
                    //			            }
                }
                firstShowDemandLink = true;
            }
        }
    });
}

var baoxiaoOrImport = [];

(function () {
    var cityId = 201, cityAllSpell = 'beijing';
    if (bit_locationInfo && bit_locationInfo.cityId) {
        cityId = bit_locationInfo.cityId;
        cityAllSpell = bit_locationInfo.engName;
    }

    //商城接口（包销） songcl 2015-06-30
    $.ajax({
        url: "http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=" + CarCommonCSID + "&cityid=" + citycode,
        async: false,
        dataType: "jsonp",
        jsonpCallback: "baseinfo",
        cache: true,
        success: function (data) {
            var arrBaoxiao = [];
            var arrPingXingImport = [];
            //包销车或者平行进口车
            if (data.CarList.length > 0) {
                baoxiaoOrImport = [];
                for (var k in data.CarList) {
                    baoxiaoOrImport.push(data.CarList[k].CarId);
                    if (data.CarList[k].CarType == 0) {
                        arrBaoxiao.push(data.CarList[k].CarId);
                    }
                    if (data.CarList[k].CarType == 1) {
                        arrPingXingImport.push(data.CarList[k].CarId);
                    }
                    //$("a[id^='car_filter_id_" + data.CarList[k].CarId + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + data.CarList[k].CarId + "&source=myc-zs-loan-01");
                    $("a[id^='car_filterzuidi_id_" + data.CarList[k].CarId + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + data.CarList[k].CarId + "&source=100064&leads_source=m002016").html("直销特卖");
                }

                if (arrBaoxiao.length > 0) {
                    //$("#dujia").show();
                    //$("#baoxiaoche").show();
                    ////试驾
                    ////getShiJiaData();
                } else {
                    //$("#feibaoxiaoche").show();
                }
            } else {
                //$("#feibaoxiaoche").show();
            }
        }
    });

    //统计
    var channelIDs = { "3": "27.23.115", "9": "27.23.119", "10": "27.23.120", "11": "27.23.121", "12": "27.23.122", "13": "27.23.123", "14": "27.23.994" };
    var urlEndPartCode = { "3": "?ref=mchexizshuan&leads_source=m002002", "9": "&tracker_u=18_ycydcx&leads_source=m002004", "10": "&source=100064&leads_source=m002005", "11": "?ref=mcar1&rfpa_tracker=2_22&leads_source=m002006", "12": "?ref=mchexizska&leads_source=m002007", "14": "?leads_source=m002017" };
    //按钮统计
    var global_busbtn_arr = ["1", "2", "0", "5", "6"];
    var global_busbtn_channelids = { "0": "27.23.116", "1": "27.23.119", "2": "27.23.114", "5": "27.23.117", "6": "27.23.118" };
    var global_busbtn_code = { "0": "?leads_source=m002003", "1": "&tracker_u=613_cxzs&leads_source=m002004", "2": "?from=ycm1&leads_source=m002001", "5": "?ref=mchexizsmai&leads_source=m002014", "6": "?ref=mchexizsgu" };
    var global_duibiLevelBtn_style = { "0": "xunjia-btn", "1": "three-item item-xinche", "2": "three-item item-daikuan", "5": "xunjia-btn", "6": "two-item item-ershouche" };
    var global_duibiLevelBtn_channelids = { "0": "27.23.1357", "1": "27.23.1355", "2": "27.23.1356", "5": "27.23.1358", "6": "27.23.1359" };
    var global_duibiLevelBtn_code = { "0": "?leads_source=m003016", "1": "&tracker_u=611_gddf&leads_source=m003014", "2": "?from=229&leads_source=m003015", "5": "?leads_source=m003017", "6": "?leads_source=m003018" };
    // add by gux 20170425
    if (["4123", "4881", "2608", "1574", "2573", "3987", "2032", "1905", "4847", "1798"].indexOf(CarCommonCSID) != -1) {
        global_busbtn_code["0"] = "?leads_source=m002003&WT.mc_id=nbcjdx";
        global_duibiLevelBtn_code["0"] = "?leads_source=m003016&WT.mc_id=nbfdx";
    }
    //购车服务
    $.ajax({
        url: "http://api.car.bitauto.com/api/GetBusinessService.ashx?date=20160721&action=mserial&cityid=" + cityId + "&serialid=" + CarCommonCSID,
        async: false,
        dataType: "jsonp",
        jsonpCallback: "businessCarCallBack",
        cache: true,
        success: function (data) {
            if (data && data != null) {
                var serviceHtml = [],
                    btnHtml = [],
                    duibiLevelBtnHtml = [],
                    btnCount = 0;
                btnHtml.push("<ul>");
                $.each(data.Button, function (i, n) {
                    if (i > 2) return;
                    if (global_busbtn_arr.indexOf(n.BusinessId) != -1) {
                        btnHtml.push("<li " + ((n.BusinessId == "0" || n.BusinessId == "5") ? "class=\"btn-org\"" : "") + "><a data-channelid=\"" + (global_busbtn_channelids[n.BusinessId] || "") + "\" href=\'" + n.MobileUrl + (global_busbtn_code[n.BusinessId] || "") + "\'>" + n.LongTitle + "</a></li>");
                        //对比层按钮
                        duibiLevelBtnHtml.push('<li class="' + (global_duibiLevelBtn_style[n.BusinessId] || "") + '"><a data-channelid="' + (global_duibiLevelBtn_channelids[n.BusinessId] || "") + '"  href="' + n.MobileUrl + (global_duibiLevelBtn_code[n.BusinessId] || "") + '"><em></em>' + n.LongTitle + '</a></li>');
                        btnCount++;
                    }
                });
                btnHtml.push("</ul>");
                if (btnCount == 2) {
                    $("#btn-business").html(btnHtml.join('')).addClass("sum-btn-two");
                } else {
                    $("#btn-business").html(btnHtml.join(''));
                }
                $("#bottomFloat ul").append(duibiLevelBtnHtml.join(''));
                if ($("#liSCInfo").length > 0 && $("#bottomFloat .xunjia-btn").length > 0) {
                    var liScInfoClone = $("#liSCInfo").clone();
                    $("#liSCInfo").remove();
                    $(liScInfoClone).insertBefore($("#bottomFloat .xunjia-btn"));
                }

                if (data.BigButton) {
                    var bigBtnCnt = data.BigButton.length; //购车服务数量  
                    var forend = bigBtnCnt;//循环终止标记
                    if (bigBtnCnt > 2) {
                        forend = 3;
                        serviceHtml.push("<ul class=\"three-service\">");
                    }
                    else if (bigBtnCnt > 1) {
                        serviceHtml.push("<ul class=\"two-service\">");
                    }
                    else {
                        //购车服务块不显示
                        //$('.car-service').hide();
                        return;
                    }
                    var i;
                    for (i = 0; i < forend; i++) {
                        serviceHtml.push("<li>");
                        var curChannelId = (channelIDs[data.BigButton[i].BusinessId] == undefined ? "" : channelIDs[data.BigButton[i].BusinessId]);//统计
                        if (data.BigButton[i].BusinessId == "13") { //购车服务
                            //serviceHtml.push("<a  data-channelid=\"" + curChannelId + "\" href=\"" + data.BigButton[i].MobileUrl + "\"><strong>" + data.BigButton[i].Title + "</strong>");
                            //serviceHtml.push("<em class=\"cGray\">" + data.BigButton[i].Price + "</em>");
                            //add by 2016.05.18 66购车节
                            //serviceHtml.push("<a target=\"_blank\" href=\"http://11.m.yiche.com\" class=\"service-icon-11\"></a>");

                            serviceHtml.push("<a  data-channelid=\"" + curChannelId + "\" href=\"http://gouche.m.yiche.com/\"><strong>购车服务</strong>");
                            serviceHtml.push("<em class=\"cGray\">查看更多&gt;&gt;</em>");
                        }
                        else {
                            serviceHtml.push("<a  data-channelid=\"" + curChannelId + "\" href=\"" + data.BigButton[i].MobileUrl + (urlEndPartCode[data.BigButton[i].BusinessId] == undefined ? "" : urlEndPartCode[data.BigButton[i].BusinessId]) + "\"><strong>" + data.BigButton[i].LongTitle + "</strong>");
                            serviceHtml.push("<em>" + data.BigButton[i].Price + "</em>");
                        }
                        serviceHtml.push("</a>");
                        serviceHtml.push("</li>");
                    }
                    serviceHtml.push("</ul>");
                    $(".car-service").html(serviceHtml.join('')).show();
                    // add log statistics
                    Bglog_InitPostLog();
                }
            }
        }
    });

    ////加载“贷款推荐”模块
    //$.ajax({
    //	url: "http://carapi.chedai.bitauto.com/api/SummarizeFinancialProducts/SearchSummarizeFinancialProducts?cityId=" + citycode + "&serialId=" + CarCommonCSID + "&pageSize=2", dataType: "jsonp", jsonpCallback: "creditinfo", cache: true, success: function (result) {
    //		var data = $.parseJSON(result);
    //		if (typeof data !== "undefined" && data.length > 0) {
    //			var strHtml = [];
    //			for (var i = 0; i < data.length; i++) {
    //				strHtml.push("<li>");
    //				strHtml.push("<a href=\"" + data[i].MDetailsUrl + "&leads_source=m002012\">");
    //				strHtml.push("<span class=\"img-box\"><img src=\"" + data[i].CompanyLogoUrl + "\"></span>");
    //				strHtml.push("<dl class=\"loan-bank\">");
    //				strHtml.push("<dt>" + data[i].CompanyName + "</dt>");
    //				strHtml.push("<dd><span>成功率</span><div class=\"loan-block\"><em style=\"width: " + data[i].SuccessScore * 12 + "px\"></em></div></dd>");
    //				strHtml.push("</dl>");
    //				strHtml.push("<span class=\"loan-info\"><dl><dt>总成本</dt><dd>" + data[i].TotalCostText + "</dd></dl><dl><dt>月供</dt><dd><strong>" + data[i].MonthlyPaymentText + "</strong></dd></dl>");
    //				strHtml.push("</span>");
    //				strHtml.push("</a>");
    //				strHtml.push("</li>");
    //			}
    //			$(".m-loan ul").html(strHtml.join(''));
    //			$("#m-loan-title,#m-loan-con").show();
    //		}
    //	}
    //});
    //养护
    //$.ajax({
    //	url: "http://yanghu.m.yiche.com/GetGoodsDealer/GetGoodsDealer/?SerialId=" + CarCommonCSID + "&CityId=" + cityId + "",
    //	cache: true,
    //	dataType: "jsonp",
    //	jsonpCallback: "yanghuCallback",
    //	success: function (data) {
    //		fillYanghuPart(data);
    //	}
    //});

    //试驾按钮
    function getShiJiaData() {
        $.ajax({
            url: "http://192.168.87.22:8081/Yiche/AllowTestDrive?cityId=" + cityId + "&modelId=" + CarCommonCSID + "", cache: true, dataType: "jsonp", jsonpCallback: "shijiaCallback", success: function (data) {
                if (data && data.Allow) {
                    $("#baoxiaoche").removeClass("sum-btn-two");
                    $("#mall-shijia").show().find("a").attr("href", data.MUrl);
                }
            }
        });
    }
})();


function fillYanghuPart(data) {
    if (typeof data == "undefined") return;
    if (data.IsSuccess != true) return;
    var html = [];
    html.push(" <div class='tt-first' data-channelid=\"27.23.741\">");
    html.push(" <h3>养护</h3>");
    html.push(" <div class='opt-more opt-more-gray'><a href='" + data.MoreUrl + "'>更多</a></div>");
    html.push(" </div>");
    html.push(" <div class='car-yh' data-channelid=\"27.23.742\">");
    html.push(" <ul>");

    if (typeof data.liGoods != 'undefined'
        && data.liGoods != null
        && data.liGoods.length > 0) {
        for (var i = 0; i < data.liGoods.length; i++) {
            var item = data.liGoods[i];

            html.push(" <li>");
            html.push(" <a href='" + item.OrderUrl + "&leads_source=m002013'>");
            html.push(" <h2>" + item.GoodsName + "</h2>");
            html.push(" <dl>");
            html.push(" <dd>");

            if (item.Is4s == true) {
                html.push(" <em>4S-</em>");
            }

            html.push(item.DealerName);
            html.push(" </dd>");

            html.push(" </dl>");

            if (item.TotalFavFee > 0) {
                html.push(" <b>" + item.TotalFavFee + "元</b>");
            }

            html.push(" </a>");
            html.push(" </li>");
        }
    }

    html.push(" </ul>");
    html.push(" </div>");

    var yanghu = document.getElementById("yanghu");
    ;
    if (yanghu) {
        yanghu.innerHTML = html.join("");
    }
    Bglog_InitPostLog();
}

//登录 车型关注
function initLoginFavCar(carLoginresult) {
    if (carLoginresult.isLogined) {
        Bitauto.UserCars.getConcernedCars(function () {
            try {
                var added = false;
                var concernedcar = Bitauto.UserCars.concernedcar.arrconcernedcar;
                if (typeof carLoginresult != 'undefined' && typeof concernedcar != 'undefined' && concernedcar.length > 0) {
                    for (var i = 0; i < concernedcar.length; i++) {
                        if (concernedcar[i] == GlobalSummaryConfig.SerialId) {
                            added = true;
                            $("#favstar").addClass("ico-favorite ico-favorite-sel");
                            break;
                        }
                    }
                }
                if (!added) {
                    var hash;
                    hash = window.location.hash;
                    if (hash && hash == "#add") {
                        var obj = $("#favstar");
                        FocusCar(obj);
                        location.hash = "";
                    }
                }
                $("#favstar").bind("click", function () {
                    FocusCar($(this));
                });
            } catch (e) {
            }
        });
    } else {
        $("#favstar").attr("href", 'http://i.m.yiche.com/authenservice/login.aspx?returnUrl=' + encodeURIComponent(location.href) + '#add');
    }
}

//添加 取消关注车型
function FocusCar(obj) {
    var id = GlobalSummaryConfig.SerialId;
    obj.attr('class') == "ico-favorite" ? Bitauto.UserCars.addConcernedCar(id, function () {
        if (Bitauto.UserCars.concernedcar.message[0] == "已超过上限") {
            alert("关注数量已达上限");
        } else {
            obj.addClass("ico-favorite ico-favorite-sel");
            Bitauto.UserCars.concernedcar.arrconcernedcar.unshift(id);
        }
    }) : Bitauto.UserCars.delConcernedCar(id, function () {
        obj.removeClass("ico-favorite-sel");
    });
}

$(function () {
    Bitauto && Bitauto.Login && Bitauto.Login.onComplatedHandlers && Bitauto.Login.onComplatedHandlers.add("memory once", initLoginFavCar);

    var ids = $("#ids").val();
    if ($("#ids").length > 0 && ids.length > 0) {
        $.ajax({
            url: "http://v.bitauto.com/vbase/CacheManager/GetVideoTotalVisitCommentCountByIds?ids=" + ids,
            async: false,
            dataType: "jsonp",
            jsonpCallback: "successHandler",
            cache: true,
            success: function (data) {
                $(data).each(function (index, item) {
                    $("#viewcount_" + item.VideoId).html(item.TotalVisit);
                });
            }
        });
    }

    // 底部浮动层
    var screenHeight = $(window).height();
    $('#bottomFloat').show();
   
    $(window).bind("scroll", function () {
        if ($(window).scrollTop() > screenHeight - 300) {
   //         $('#bottomFloat').show();
            $('#carNavFixed').show();
        } else {
         //   $('#bottomFloat').hide();
            $('#carNavFixed').hide();
        }
    });
   

    //空间详情
    $(".scroll-card .car-info-txt").find("em").on("click", function (e) {
        e.stopPropagation();
        $(this).find(".pop-box").show();
    });
    $(".scroll-card .car-info-txt").find(".pop-box").on("click", function (e) {
        e.stopPropagation();
        $(this).hide();
    });
    // 买买买 弹出层
    _bindBtnMaiEvent();



});

function dateFormat(dateString, format) {
    if (!dateString) return "";
    var time = new Date(dateString.replace(/-/g, '/').replace(/T|Z/g, ' ').trim());
    var o = {
        "M+": time.getMonth() + 1, //月份
        "d+": time.getDate(), //日
        "h+": time.getHours(), //小时
        "m+": time.getMinutes(), //分
        "s+": time.getSeconds(), //秒
        "q+": Math.floor((time.getMonth() + 3) / 3), //季度
        "S": time.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (time.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format)) format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return format;
}

//去除字符串头部空格或指定字符
String.prototype.TrimStart = function (c) {
    if (c == null || c == "") {
        var str = this.replace(/^\s*/, '');
        return str;
    } else {
        var rg = new RegExp("^" + c + "*");
        var str = this.replace(rg, '');
        return str;
    }
};
//判断json对象（返回格式:{}）是否为空
var isEmptyObject = function (obj) {
    var name;
    for (name in obj) {
        return false;
    }
    return true;
};

function callCommonMethod(url, dataType, callBackName, callBackFunc) {
    $.ajax({
        url: url,
        async: false,
        cache: false,
        dataType: dataType,
        jsonpCallback: callBackName,
        success: function (data) {
            callBackFunc(data);
            Bglog_InitPostLog();
        }
    });
}

//贷款
function getDaikuan(cityCode, curCarId, $leftPopup) {
    var url = 'http://carapi.daikuan.com/api/SummarizeFinancialProducts/SearchCityCarProduct?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
    callCommonMethod(url, "jsonp", "callgetDaikuan", function (dataStr) {
        var h = [];
        //var dataStr = eval("(" + data + ")");
        var flag = isEmptyObject(dataStr);
        if (dataStr && !flag) {
            h.push("<a href=\"" + dataStr.MDetailsUrl + "\"><img src=\"" + dataStr.CarImageUrl + "\"></a>");
            h.push("<h6>" + dataStr.PackageName + "</h6>");
            h.push("<div class=\"two-line\">");
            h.push("<p>首付：<strong>" + dataStr.DownPaymentText + "</strong></p>");
            h.push("<p>月供：<strong>" + dataStr.MonthlyPaymentText + "</strong></p>");
            h.push("<a href=\"" + dataStr.MDetailsUrl + "\" class=\"btn-mmm\">立即申请</a>");
            h.push("</div>");
            h.push("<div class=\"mmm-tit-box mmm-daikuan\">");
            h.push("<span>贷款买</span><i></i>");
            h.push("</div>");
            $('#m-car-daikuan').html(h.join('')).show();
            $('.mmm-none').hide();
            $leftPopup.find('.loading').hide();
        } else {
            $('#m-car-daikuan').hide();
            $leftPopup.find('.loading').hide();
        }
    });
}

//团购
function getTuanGou(cityCode, curSerialId, $leftPopup) {
    var url = 'http://platform.api.huimaiche.com/m/tg/product/item?cityid=' + cityCode + '&serialid=' + curSerialId; //curSerialId
    callCommonMethod(url, "jsonp", "callgetTuanGou", function (data) {
        var h = [];
        var flag = isEmptyObject(data);
        if (data && !flag) {
            h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
            h.push("<h6>" + data.Description + "</h6>");
            h.push("<div class=\"two-line\">");
            h.push("<p>已报名：<strong>" + data.BuyerAmount + "人</strong></p>");
            h.push("<p>开团日：<strong>" + dateFormat(data.PurchaseTime, "MM").TrimStart('0') + "月" + dateFormat(data.PurchaseTime, "dd").TrimStart('0') + "日" + "</strong></p>");

            h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">立即报名</a>");
            h.push("</div>");
            h.push("<div class=\"mmm-tit-box mmm-tuangou\">");
            h.push("<span>团购</span><i></i>");
            h.push("</div>");
            $('#m-car-tuan').html(h.join('')).show();
            $leftPopup.find('.loading').hide();
            $('.mmm-none').hide();
        } else {
            $('#m-car-tuan').hide();
            $leftPopup.find('.loading').hide();
        }
    });
}

//直销
function getDirectSell(cityCode, curCarId, $leftPopup) {
    var url = 'http://platform.api.huimaiche.com/m/zx/product/directsellingitem?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
    callCommonMethod(url, "jsonp", "callgetDirectMall", function (data) {
        var h = [];
        var flag = isEmptyObject(data);
        if (data && !flag) {
            h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
            h.push("<h6>" + data.ProductShowName + "</h6>");
            h.push("<div class=\"one-line\">");
            h.push("<p>直销价：<strong>" + data.DirectSalePrice + "</strong></p>");
            h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">立即抢购</a>");
            h.push("</div>");
            h.push("<div class=\"mmm-tit-box mmm-zhixiao\">");
            h.push("<span>直销价</span><i></i>");
            h.push("</div>");
            $('#m-car-zhixiao').html(h.join('')).show();
            $leftPopup.find('.loading').hide();
            $('.mmm-none').hide();
        } else {
            $('#m-car-zhixiao').hide();
            $leftPopup.find('.loading').hide();
        }
    });
}

//比价买
function getComparisonBuy(cityCode, curCarId, $leftPopup) {
    var url = 'http://platform.api.huimaiche.com/m/c2b/product/item?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
    callCommonMethod(url, "jsonp", "callgetComparisonBuy", function (data) {
        var h = [];
        var flag = isEmptyObject(data);
        if (data && !flag) {
            h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
            h.push("<h6>" + data.ProductShowName + "</h6>");
            h.push("<div class=\"one-line\">");
            h.push("<p><strong>" + data.Description + "</strong></p>");
            h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">获取底价</a>");
            h.push("</div>");
            h.push("<div class=\"mmm-tit-box mmm-bijiamai\">");
            h.push("<span>比价买</span><i></i>");
            h.push("</div>");
            $('#m-car-bijia').html(h.join('')).show();
            $leftPopup.find('.loading').hide();
            $('.mmm-none').hide();
        } else {
            $('#m-car-bijia').hide();
            $leftPopup.find('.loading').hide();
        }
    });
}

//优惠券
function getCoupon(cityCode, curCarId, $leftPopup) {
    var url = "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=wapchexingye&cityid=" + cityCode + "&carid=" + curCarId; //curCarId
    callCommonMethod(url, "jsonp", "callgetCoupon", function (data) {
        var h = [];
        var result = data.data;
        var flag = isEmptyObject(result);
        if (result && !flag && data.success == true) {
            h.push("<a href=\"" + result.murl + "\"><img src=\"" + result.imgurl4 + "\"></a>");
            h.push("<h6>" + result.pb_name + "</h6>");
            h.push("<div class=\"one-line\">");
            h.push("<p><strong>" + result.p_deposit + "</strong></p>");
            h.push("<a href=\"" + result.murl + "\" class=\"btn-mmm\">立即抢购</a>");
            h.push("</div>");
            h.push("<div class=\"mmm-tit-box mmm-youhuiquan\">");
            h.push("<span>" + result.channelm + "</span><i></i>");
            h.push("</div>");
            $('#m-car-ych').html(h.join('')).show();
            $leftPopup.find('.loading').hide();
            $('.mmm-none').hide();
        } else {
            $('#m-car-ych').hide();
            $leftPopup.find('.loading').hide();
        }
    });
}

// 买买买 弹出层事件
var _bindBtnMaiEvent = function () {
    var $body = $('body');
    $('.btn-mmm').click(function (ev) {
        var $click = $(this);
        ev.preventDefault();
        $body.trigger('fristSwipeOneNb', {
            //fristSwipeOneNb 一级不带按钮控件
            $swipe: $body.find('.mmm'), //弹出浮层
            $click: $click, //点击对象
            fnEnd: function () {
                //层打开后回调
                var $leftPopup = this;
                //获取填充数据
                var curCarId = $click.data("car");
                var html = '';
                $leftPopup.find('.ap').show();
                $leftPopup.find('.mmm-none').show();
                //getComparisonBuy(citycode, curCarId, $leftPopup);
                getDirectSell(citycode, curCarId, $leftPopup);
                getCoupon(citycode, curCarId, $leftPopup);
                getDaikuan(citycode, curCarId, $leftPopup);
                getTuanGou(citycode, curSerialId, $leftPopup);
            },
            closeEnd: function () {
                //关闭层回调
                var $leftPopup = this;
                $leftPopup.find('.loading').show();
                $leftPopup.find('.mmm-pop').hide();
                $leftPopup.find('.ap').hide();
            }
        });
    });
};

function addTrackingCode(serialId) {
    //科鲁兹加统计代码
    if (serialId == 2608) {
        $(".sum-news2").attr("id", "focusNews" + serialId);
    }
}
addTrackingCode(GlobalSummaryConfig.SerialId);