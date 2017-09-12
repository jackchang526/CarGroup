$(function () {
    //奖项块
    if ($("#honor-list .honor-box").length > 0 && typeof serialAwardJson != undefined) {
        $("#honor-list li").each(function () {
            var id = $(this).attr("data-id");
            var award = serialAwardJson[id];
            $(this).hover(function () {
                $(this).siblings().removeClass("current");
                $(this).addClass("current");
                var h = [];
                h.push("<span class=\"figure\">");
                h.push("<img src=\"" + award["logo"] + "\" alt=\"\"/></span>");
                h.push("<h4>" + award["name"] + "</h4>");
                h.push("<ul>");
                if (award["yearinfo"].length > 0) {
                    for (var i = 0; i < award["yearinfo"].length ; i++) {
                        h.push("    <li><span class=\"year\">" + award["yearinfo"][i]["year"] + "年</span><span class=\"info\">" + award["yearinfo"][i]["desc"] + "</span></li>");
                    }
                }
                h.push("</ul>");
                $("#honor-list .popup-layout-1 .inner").html(h.join(""));
                $("#honor-list .popup-layout-1").css({ visibility: "visible" });
            }, function () {
                $(this).removeClass("current");
                $("#honor-list .popup-layout-1").css({ visibility: "hidden" });
            });
        });
    }
    //车款列表
    CarListLeftFilter.initFilterPage();
    //$("#compare_sale tr[id^='car_filter_id_'],#compare_wait tr[id^='car_filter_id_']").hover(
	//    function () {
	//        $(this).addClass('hover-bg-color');
	//    },
	//	function () {
	//	    $(this).removeClass('hover-bg-color');
	//	}
    //);
    if (typeof csSaleState != "undefined" && csSaleState == '停销') {
        $("#carlist_nosaleyear a:first").trigger("click");
    }
    //核心报告
    $("#hexin-report .tabs-left").find("li[data]").each(function () {
        $(this).click(function () {
            $(this).siblings().removeClass("current");
            $(this).addClass("current");
            var group = $(this).attr("data");
            $("#hexin-report").find("div.col-auto[group^='main-']").hide();
            $("#hexin-report").find("div.col-auto[group='" + group + "']").show();
        });
    });
    $("#tongguoxing,#kongjian").find("li[data]").each(function () {
        $(this).hover(function () {
            $(this).siblings().removeClass("current");
            $(this).addClass("current");
            var group = $(this).attr("data");
            var parent = $(this).parents(".tab-main");
            $(parent).find("div[group]").hide();
            $(parent).find("div[group='" + group + "']").show();
        }, function () {

        });
    });
    $("#houbeixiang-content .btn-tab a").each(function (i) {
        $(this).hover(function () {
            $(this).siblings().removeClass("btn-primary2").addClass("btn-default");
            $(this).removeClass("btn-default").addClass("btn-primary2");
            var img = $("#houbeixiang-content div.img");
            if (img.length >= i) {
                $(img).hide();
                $(img[i]).show();
            }
        }, function () {

        });
    });
    //车款列表，计算器弹层
    $("#compare_sale,#compare_nosale,#compare_wait").on("mouseover mouseout", "a[data-use='compute']", function (event) {
        if (event.type == "mouseover") {
            if ($(this).children(".popup-layout-1").length == 0) {
                var obj = this;
                var carId = $(obj).attr("carid");
                $.ajax({
                    url: "http://api.car.bitauto.com/carinfo/GetCarPriceComputer.ashx?carid=" + carId, dataType: "jsonp", cache: true, jsonpCallback: "GetCarPriceComputerCarCallBack", success: function (data) {
                        if (data.msg == "") {
                            var h = [],
                                target = event.srcElement || event.target,
                                showpop = $("#car_filter_id_" + carId + " .car-comparetable-ico-cal").attr("showpop");

                            h.push("<div class=\"popup-layout-1\" style=\"visibility: " + (showpop == "false" ? "hidden" : "visible") + ";\">");
                            h.push("<p>全款总价 <em>" + data.price.totalprice + "</em></p>");
                            h.push("<p>贷款首付－30% <em>" + data.price.loanfirstdownpayments + "</em></p>");
                            h.push("<p>月供－36期 <em>" + data.price.loanmonthpayments + "</em></p>");
                            h.push("<p>商业保险 <em>" + data.price.insurance + "</em></p>");
                            h.push("<p class=\"more\" onclick=\"javascript:;\">查看更多计算器&gt;&gt;</p>");
                            h.push("</div>");
                            $("#car_filter_id_" + carId + " .car-comparetable-ico-cal").html(h.join(""));
                        }
                    }
                });
            }
            else {
                $(this).children(".popup-layout-1").css("visibility", "visible");
            }
        } else if (event.type == "mouseout") {
            var pop = $(this).children(".popup-layout-1");
            if (pop.length > 0) {
                pop.css("visibility", "hidden");
            }
            else {
                $(this).attr("showpop", "false");
            }
        }
    });
    //团购，优惠
    var urlStr = "http://api.market.bitauto.com/MessageInterface/Product/GetProductUrl.ashx?CmdID=" + serialId + "&CityID=" + cityId + "&MediaId=1&LocationId=2,1312";
    $.ajax({
        url: urlStr, cache: true, dataType: 'jsonp', jsonpCallback: "tgouCallback", success: function (data) {
            if (data == null) {
                return;
            }
            if (data.result == 'yes') {
                var tgouUrl = data.url;
                var signName;
                //if (data.sign == "1") {
                //    signName = "团购";
                //} else if (data.sign == "2") {
                //    signName = "优惠";
                //}
                signName = data.tag;
                $.each(data.carIds, function (i, n) {
                    $("#carlist_" + n).append("<a href=\"" + tgouUrl + "\" target=\"_blank\" class=\"color-block\">" + signName + "</a>");
                });
            }
        }
    });
    //补贴
    $.ajax({
        url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + serialId + "&cityid=" + cityId,
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
                //if (carLine.find("a[name=\"butie\"]").length > 0) {
                //    carLine.find("a[name=\"butie\"]").attr("title", sub.join(","));
                //} else {
                    var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"color-block2\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
                    if (carLine.find("span.color-block2").length > 0) {
                        carLine.find("span.color-block2").after(b);
                    } else {
                        carLine.find("a:first").after(b);
                    }
                //}
            });
        }
    });
});
//促销信息
function GetPromotionNews() {
    $.ajax({
        url: "http://m.h5.qiche4s.cn/jiangjiaapi/GetPromtionNews.ashx?op=promotionnewsV1&csid=" + serialId + "&cid=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetPromotionNewsCallback",
        success: function (data) {
            var dataJson = null,
                isRecommend = false,
                startindex = 0;
            var h = [];
            if (data.PromotionList.length > 0) {
                dataJson = data.PromotionList;
                startindex = 1;
            }
            else if (data.RecommendPromotionList.length > 0) {
                dataJson = data.RecommendPromotionList;
                isRecommend = true;
            }
            h.push("<div class=\"list-txt list-txt-m list-txt-default\"><ul>");
            if (dataJson != null && dataJson.length > 0) {

                if (isRecommend) {
                    //h.push("<h3 class=\"special-type1\">暂无降价 为您推荐</h3>");
                    h.push("<li class=\"no-link\"><div class=\"txt\"><a>暂无降价，为您推荐以下内容：</a></div></li>");
                }
                //else {
                //    h.push("<h3 class=\"no-wrap\"><a href=\"" + dataJson[0].Url + "\" target=\"_blank\">" + dataJson[0].NewsTitle + "</a></h3>");
                //}
                //if (dataJson.length > startindex) {
                    for (var i = 0; i < dataJson.length; i++) {
                        if (i - startindex == 4) break;
                        h.push("<li><div class=\"txt\"><a href=\"" + dataJson[i].Url + "\" target=\"_blank\">" + dataJson[i].NewsTitle + "</a></div>");
                        h.push("<span>剩余" + dataJson[i].RemainDays + "天</span></li>");
                    }
                //}
            }
            else {
                //h.push("<h3 class=\"special-type1\">暂无促销信息</h3>");
                h.push("<li class=\"no-link\"><div class=\"txt\"><a>暂无促销信息</a></div></li>");
            }
            h.push("</ul></div>");
            $("#cuxiao-news").append(h.join(""));
        }
    });
}



//外观，内饰
function GetSerialWaiGuanNeiShi() {
    var colorHtmlArray = [];
    $.ajax({
        url: "http://webapi.photo.bitauto.com/datasource/api/serialbrand/GetSerialColors?serialBrandId=" + serialId, dataType: "jsonp", cache: true, jsonpCallback: "waiGuanNeiShiCallback",
        success: function (data) {
            if (data.ColorList.length == 0 && data.InnerColorList.length == 0) {
                $("#serialWaiGuanNeiShi").hide();
                if ($("#serialWaiGuanNeiShi").siblings().length == 0) {
                    $("#serialWaiGuanNeiShi").parent().hide();
                }
            }
            if (data.ColorList.length > 0) {
                var waiGuanArray = [];
                colorHtmlArray.push("<div class=\"col-auto block1\" data-channelid=\"2.21.810\"><div class=\"row\"><div class=\"col-auto title\">外观：</div>");
                for (var i = 0; i < data.ColorList.length; i++) {
                    if (data.ColorList[i].Colors.length > 0) {
                        for (var j = 0; j < data.ColorList[i].Colors.length; j++) {
                            if (waiGuanArray.length > 9) {
                                break;
                            }
                            var color = data.ColorList[i].Colors[j];
                            if (!waiGuanArray.contains(color.Id)) {
                                waiGuanArray.push(color.Id);
                                colorHtmlArray.push("<div class=\"col-auto color\" style=\"background-color: " + color.Rgb + "\">");
                                colorHtmlArray.push("    <a href=\"" + color.Link + "\" target=\"_blank\"></a>");
                                colorHtmlArray.push("    <span class=\"popup-layout-1\">" + color.Name + "</span>");
                                colorHtmlArray.push("</div>");
                            }
                        }
                    }
                }
                colorHtmlArray.push("</div></div>");
            }
            if (data.InnerColorList.length > 0) {
                var neiShiArray = [];
                colorHtmlArray.push("<div class=\"col-auto block2\" data-channelid=\"2.21.1533\"><div class=\"row\"><div class=\"col-auto title\">内饰：</div>");
                for (var i = 0; i < data.InnerColorList.length ; i++) {
                    for (var j = 0; j < data.InnerColorList[i].Colors.length; j++) {
                        if (neiShiArray.length > 4) {
                            break;
                        }
                        var color = data.InnerColorList[i].Colors[j];
                        if (!neiShiArray.contains(color.Id)) {
                            neiShiArray.push(color.Id);
                            var rgbArr = color.Rgb.split(',');
                            colorHtmlArray.push("<div class=\"col-auto color\" style=\"background-color: " + rgbArr[0] + "\">");
                            colorHtmlArray.push("    <a href=\"" + color.Link + "\" target=\"_blank\"></a>");
                            if (rgbArr.length > 2) {
                                colorHtmlArray.push("<span class=\"other\" style=\"background-color: " + rgbArr[2] + "\">");
                            }
                            if (rgbArr.length > 1) {
                                colorHtmlArray.push("    <span class=\"" + (rgbArr.length > 2 ? "third" : "other") + "\" style=\"background-color: " + rgbArr[1] + "\"></span>");
                            }
                            if (rgbArr.length > 2) {
                                colorHtmlArray.push("</span>");
                            }
                            //if (rgbArr.length > 1) {
                            //    colorHtmlArray.push("    <span class=\"other\" style=\"background-color: " + rgbArr[1] + "\">");
                            //    if (rgbArr.length > 2) {
                            //        colorHtmlArray.push("<span class=\"third\" style=\"background-color: " + rgbArr[2] + "\"></span>");
                            //    }
                            //    colorHtmlArray.push("</span>");
                            //}
                            colorHtmlArray.push("    <span class=\"popup-layout-1\">" + color.Name + "</span>");
                            colorHtmlArray.push("</div>");
                        }
                    }
                }
                colorHtmlArray.push("</div></div>");
            }

            if (colorHtmlArray.length > 0) {
                $("#serialWaiGuanNeiShi").html(colorHtmlArray.join(""));
                $("#serialWaiGuanNeiShi .col-auto").hover(
                    function () {
                        $(this).addClass("current");
                        $(this).find("popup-layout-1").show();
                    }, function () {
                        $(this).removeClass("current");
                        $(this).find("popup-layout-1").hide();
                    });
                if (typeof (Bglog_InitPostLog) != "undefined") {
                    Bglog_InitPostLog();
                }
            }
        }
    });
}

//视频播放次数和回复数
function GetVedioNum() {
    if (typeof vedioIds != "undefined" && vedioIds.length > 0) {
        $.ajax({
            url: "http://api.admin.bitauto.com/videoforum/Promotion/GetVideoByVideoIds?vIds=" + vedioIds, dataType: "jsonp", cache: true, jsonpCallback: "getvedionumcallback", success: function (data) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        $("#car-videobox div[data-id='" + data[i].VideoId + "'] .play").html(data[i].FormatPlayCount);
                        $("#car-videobox div[data-id='" + data[i].VideoId + "'] .comment").html(data[i].ReplyCount);
                    }
                }
            }
        });
    }
}
//销量
function GetXiaoliang() {
    var rurl = "http://api.easypass.cn/indexDataProvider/GetIndexData.ashx?indextype=sale&date={datestr}&csid=" + serialId;
    var now = new Date(),
        nowyear = now.getFullYear(),
        nowmonth = now.getMonth()
    datestr = "";

    if (nowmonth == 0) {
        nowmonth = 12;
        nowyear = nowyear - 1;
    }
    if (nowmonth < 10) {
        datestr = nowyear + "0" + nowmonth;
    }
    else {
        datestr = nowyear + "" + nowmonth;
    }
    $.ajax({
        url: rurl.replace("{datestr}", datestr),
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetXiaoliangCallback",
        success: function (data) {
            if (typeof data != "undefined") {
                $("#report-xiaoliang").html(data.count + "辆");
                $("#report-xiaoliang-month").html(nowmonth + "月销量");
            }
            else {
                nowmonth = nowmonth - 1;
                if (nowmonth == 0) {
                    nowmonth = 12;
                    nowyear = nowyear - 1;
                }
                if (nowmonth < 10) {
                    datestr = nowyear + "0" + nowmonth;
                }
                else {
                    datestr = nowyear + "" + nowmonth;
                }
                $.ajax({
                    url: rurl.replace("{datestr}", datestr),
                    cache: true,
                    dataType: "jsonp",
                    jsonpCallback: "GetXiaoliangback2",
                    success: function (data) {
                        $("#report-xiaoliang-month").html(nowmonth + "月销量");
                        if (typeof data != "undefined") {
                            $("#report-xiaoliang").html(data.count + "辆");
                        }
                        else {
                            $("#report-xiaoliang").addClass("none").html("暂无");
                        }
                    }
                });
            }
        }

    });
}
//限时抢购
function getBuyLimit() {
    $.ajax({
        url: "http://m.h5.qiche4s.cn/temai/handler/ActiveCommonHandler.ashx?action=getcaractivefor990sum&brandId=" + serialId + "&cityId=" + cityId,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "getGoucheXscgCallback",
        success: function (data) {
            if (!(data && data.length > 0)) {
                return;
            }
            var data = data[0];
            var h = [],
				url = data.PcUrl + (data.PcUrl.indexOf("?") > -1 ? "&" : "?") + "leads_source=p002024";

            h.push("<div class=\"col-auto left\">");
            h.push("    <div class=\"caption\">");
            h.push("        <em class=\"em\">限时抢购</em>");
            h.push("    </div>");
            h.push("</div>");
            h.push("<div class=\"col-auto mid\">");
            h.push("    <h5><a href=\"" + url + "\" target=\"_blank\">" + data.Title + "</a></h5>");
            h.push("    <div class=\"bottom\">");
            h.push("        <a class=\"btn btn-secondary btn-sm\" href=\"" + url + "\" target=\"_blank\">我要报名</a>");
            h.push("        <span class=\"info\">" + data.TitleDesc + "</span>");
            h.push("    </div>");
            h.push("</div>");
            h.push("<div class=\"col-auto right\">");
            h.push("    <h6 class=\"countdown\">" + data.LeftDays + "</h6>");
            h.push("</div>");

            $("#gouche-xscg").html(h.join('')).show();
        }
    });
}
//车贷
function GetCheDai() {
    $.ajax({
        url: "http://carapi.daikuan.com/api/SummarizeFinancialProducts/SearchSummarizeFinancialProducts?cityId=" + cityId + "&serialId=" + serialId + "&pagesize=4",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetCheDaiCallback",
        success: function (data) {
            //var data = $.parseJSON(result);
            if (typeof data == "undefined" || data.length == 0) {
                $("#gouche-chedai").hide();
                return;
            }
            var h = [];
            for (var i = 0; i < data.length; i++) {
                var obj = data[i];
                h.push("<div class=\"row inner\">");
                h.push("    <div class=\"col-auto left\">");
                h.push("        <a class=\"figure\" target=\"_blank\" href=\"" + obj.PCDetailsUrl + "\">");
                h.push("            <span class=\"img\">");
                h.push("                <img src=\"" + obj.CompanyLogoUrl + "\" alt=\"\">");
                h.push("            </span>");
                h.push("            <h4 class=\"title\">" + obj.PackageName + "</h4>");
                h.push("            <p class=\"info\">" + obj.CompanyName + "</p>");
                h.push("        </a>");
                h.push("    </div>");
                h.push("    <div class=\"col-auto mid\">");
                h.push("        <div class=\"condition\">");
                h.push("            申请条件：<span class=\"title" + (obj.CommonRequirement == "严格" ? " em":"") + "\">" + obj.CommonRequirement + "</span>");
                h.push("        </div>");
                h.push("        <div class=\"provide\">");
                h.push("            所需材料：<span class=\"title\">" + obj.PackageFeatureIcon2 + "</span>");
                h.push("        </div>");
                h.push("    </div>");
                h.push("    <div class=\"col-auto right\">");
                h.push("        <div class=\"price\">" + obj.MonthlyPaymentText.replace("元", "") + "<span class=\"title\">元/月</span>");
                h.push("        </div>");
                h.push("        <div class=\"total\">");
                h.push("            总成本 " + obj.TotalCostText);
                h.push("        </div>");
                h.push("    </div>");
                h.push("    <a href=\"" + obj.PCDetailsUrl + "\" target=\"_blank\" class=\"btn btn-default\">查看详情</a>");
                h.push("</div>");
            }
            $("#gouche-chedaicontent").html(h.join(""));
            GetRemenHuoDong();
        },
        error: function () {
            $("#gouche-chedai").hide();
        }
    });
}
//车贷热门活动
function GetRemenHuoDong() {
    $.ajax({
        url: "http://carapi.daikuan.com/api/SummarizeFinancialProducts/GetSummarizeFinancialActivities",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetRemenHuoDongCallback",
        success: function (data) {
            //var data = $.parseJSON(result);
            if (typeof data == "undefined" || data.length == 0) {
                $("#gouche-huodong").hide();
                return;
            }
            var h = [];
            h.push("<h6>贷款工具：</h6><ul class=\"list\">");
            for (var i = 0; i < data.length; i++) {
                var obj = data[i];
                h.push("<li><a target=\"_blank\" href=\"" + obj.ActivityLink + "\">" + obj.ActivityName + "</a></li>");
            }
            h.push("</ul>");
            $("#gouche-huodong").html(h.join(""));
        },
        error: function () {
            $("#gouche-huodong").hide();
        }
    });
}

function renderEscHtml(data) {
    if (data.length <= 0) {
        return "";
    }
    var strHtml = [];
    $.each(data, function (i, n) {
        if (i > 7) return;
        strHtml.push("<div class=\"img-info-layout-vertical img-info-layout-vertical-180120-2\">");
        strHtml.push("    <div class=\"img\">");
        strHtml.push("        <a href=\"" + n.CarlistUrl + "&leads_source=p002019\" target=\"_blank\">");
        strHtml.push("            <img src=\"" + n.PictureUrl.replace("/1f/", "/1d/") + "\">");
        strHtml.push("        </a>");
        strHtml.push("    </div>");
        strHtml.push("    <ul class=\"p-list\">");
        strHtml.push("        <li class=\"name\"><a href=\"" + n.CarlistUrl + "&leads_source=p002019\" target=\"_blank\">" + n.BrandName + "</a></li>");
        strHtml.push("        <li class=\"price\">￥" + n.DisplayPrice + "</li>");
        strHtml.push("        <li class=\"info no-wrap\">" + (n.BuyCarDate == "未上牌" ? "未上牌" : (n.BuyCarDate + "上牌")) + " " + n.DrivingMileage + "公里</li>");
        strHtml.push("    </ul>");
        strHtml.push("</div>");
    });
    return strHtml.join("");
}

function searchEsc(csSpell) {
    if (priceRang.indexOf("万") > 0) {
        priceRang = priceRang.replace(/万$/, "");
    }
    var tongjiaweiAjax = $.ajax({
        url: "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?cityid=" + cityId + "&p=" + priceRang + "&count=8"
            , dataType: "jsonp"
            , cache: true
            , jsonpCallback: "esctjwCarCallBack"
    }),
        tongpinpaiAjax = $.ajax({
            url: "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?sid=" + serialId + "&cityid=" + cityId + "&count=8"
            , dataType: "jsonp"
            , cache: true
            , jsonpCallback: "esctppCarCallBack"
        });
    $.when(tongjiaweiAjax, tongpinpaiAjax).done(function (tjwData, tppData) {
        if (tjwData[0].CarListInfo.length == 0 && tppData[0].CarListInfo.length == 0) {
            $("#ucarlist").hide();
            return;
        }
        var strHtml = [];
        strHtml.push("<div class=\"section-header header2\"><div class=\"box\">");
        strHtml.push("<h2><a target=\"_blank\" data-channelid=\"2.21.821\" href=\"http://www.taoche.com/" + csSpell + "/?ref=pc_yc_zs_tj_esc\">相关二手车推荐</a></h2>");
        strHtml.push("<ul id=\"data_tab1\" class=\"nav\">");
        if (tjwData[0].CarListInfo.length > 0) {
            strHtml.push("<li class=\"current\" ><a href=\"javascript:;\">同价位</a></li>");
        }
        if (tppData[0].CarListInfo.length > 0) {
            strHtml.push("<li class=\"" + (tjwData[0].CarListInfo.length == 0 ? "current" : "") + "\"><a href=\"javascript:;\">同车系</a></li>");
        }
        strHtml.push("</ul>");
        strHtml.push("</div><div class=\"more\" data-channelid=\"2.21.823\">");
        dataRelateCityList = tjwData[0].RelateCityList;
        if (dataRelateCityList.length > 0) {
            $.each(dataRelateCityList, function (index, res) {
                strHtml.push("<a href=\"" + res.CityUrl + "?ref=pc_yc_zs_tj_dq\" target=\"_blank\">" + res.CityName + "</a> ");
            });
        }
        strHtml.push("<a target=\"_blank\" href=\"http://www.taoche.com/" + csSpell + "/?ref=pc_yc_zs_tj_gd\">更多二手车&gt;&gt;</a></div>");
        strHtml.push("</div>");
        strHtml.push("<div class=\"top-info\"><span>专家陪伴，买车无忧，<a href=\"http://bangmai.taoche.com/?ref=pc_yc_zs_tj_bm\" target=\"_blank\">体验帮买车服务&gt;&gt;</a></span><span>上门评估，轻松卖高价，<a href=\"http://maiche.taoche.com/paimai/?ref=pc_yc_zs_tj_gjmc\" target=\"_blank\">立即高价卖车&gt;&gt;</a></span></div>");
        //$("#ucarlist").append(strHtml.join(''));
        var isShowTJW = true;
        if (tjwData[0].CarListInfo.length > 0) {
            strHtml.push("<div data-channelid=\"2.21.822\" id=\"data_box1_0\" class=\"row col4-180-box\" style=\"display:block\">");
            strHtml.push(renderEscHtml(tjwData[0].CarListInfo));
            strHtml.push("</div>");
        }
        if (tppData[0].CarListInfo.length > 0) {
            strHtml.push("<div data-channelid=\"2.21.822\" id=\"data_box1_1\" class=\"row col4-180-box\" style=\"display:" + (tjwData[0].CarListInfo.length == 0 ? "block" : "none") + ";\">");
            strHtml.push(renderEscHtml(tppData[0].CarListInfo));
            strHtml.push("</div>");
        }

        strHtml.push("<div class=\"btn-box1\"><a class=\"btn btn-default\" href=\"http://www.taoche.com/" + csSpell + "/?ref=pc_yc_zs_tj_db\" target=\"_blank\"><span class=\"more\">更多二手车</span></a></div>");
        $("#ucarlist").html(strHtml.join(""));
        if (tjwData[0].CarListInfo.length > 0 && tppData[0].CarListInfo.length > 0) {
            tabs("data_tab1", "data_box1", null, true);//绑定效果
        }
        if (typeof (Bglog_InitPostLog) != "undefined") {
            Bglog_InitPostLog();
        }
    });
}

//车款筛选
var CarListLeftFilter = {
    Year: [],
    Tran: [],
    Exhaust: [],
    OffsetTop: $("#car_tag").height(),
    FilterHeight: 0,
    CarListHeight: 0,
    ShowFilterWidth:1300,
    bindEvent: function () {
        var self = this,
            hoverSetTimeOut;
        //停售年款弹层
        $("#car_nosaleyearlist").hover(function () {
            clearTimeout(hoverSetTimeOut);
            $("#carlist_nosaleyear").show();
        }, function () {
            hoverSetTimeOut = setTimeout(function () {
                $("#carlist_nosaleyear").hide();
            }, 100);
        });
        //年款过滤
        $("input[name='car-filter-year']").click(function () {
            if ($(this).prop("checked") == true) {
                self.Year.push($(this).val());
            } else {
                self.Year.remove($(this).val());
            }
            self.actionFilter();
            //statForTempString(100, CarCommonCSID, $(this).val());
        });
        //变速箱过滤
        $("input[name='car-filter-tran']").click(function () {
            if ($(this).prop("checked") == true) {
                self.Tran.push($(this).val());
            } else {
                self.Tran.remove($(this).val());
            }
            self.actionFilter();
        });
        //排量过滤
        $("input[name='car-filter-exhaust']").click(function () {
            if ($(this).prop("checked") == true) {
                self.Exhaust.push($(this).val());
            } else {
                self.Exhaust.remove($(this).val());
            }
            self.actionFilter();
        });
        //清空筛选条件
        $("#car-filter-clear").click(function () {
            $("input[name='car-filter-year']").prop("checked", false);
            $("input[name='car-filter-tran']").prop("checked", false);
            $("input[name='car-filter-exhaust']").prop("checked", false);
            self.Year.length = 0;
            self.Tran.length = 0;
            self.Exhaust.length = 0;
            self.actionFilter();
        });
        $("#carlist_nosaleyear a").click(function () {
            $("#data_tab_jq5_2").show().siblings("div[id^='data_tab_jq5']").hide();
            $("#carCompareFilter").hide();
            var yearP = $(this).attr("id");
            if ($("#noSaleTitle").length == 0) {
                $("#car_nosaleyearlist").before("<li id=\"noSaleTitle\"><a href=\"javascript:;\">停售车款</a></li>");
            }
            $("#car_nosaleyearlist .arrow-down").html(yearP + "款<strong></strong>");
            $("#noSaleTitle").addClass("current").siblings().removeClass("current");
            if ($("#" + yearP).length > 0 && yearP.length > 0) {
                self.getNoSaleYearData(yearP);
            }
        });

        $("#data_tab_jq5>li[id!='car_nosaleyearlist']").each(function (i) {
            $(this).bind("click", function () {
                $(this).addClass("current").siblings().removeClass("current");
                $("#data_tab_jq5_" + i + "").show().siblings("div[id^='data_tab_jq5']").hide();
                $("#noSaleTitle").remove();
                if ($(this).children(":eq(0)").text() == "全部在售" && $(window).width() > CarListLeftFilter.ShowFilterWidth) {
                    $("#carCompareFilter").show();
                }
                else {
                    $("#carCompareFilter").hide();
                }
                $("#car_nosaleyearlist .arrow-down").html("停售车款<strong></strong>");
                $("#car_nosaleyearlist a").removeClass("current");
            });
        });
    },
    initFilterPage: function () {
        if (typeof CarFilterData != "undefined" && CarFilterData != null && $(window).width() > CarListLeftFilter.ShowFilterWidth) {
            this.initFilterHtml();
            this.FilterHeight = $("#carCompareFilter").get(0).offsetHeight;
            this.SpecialEffects();
            this.CarListHeight = $("#data_tab_jq5_1").length > 0 ? $("#data_tab_jq5_1").get(0).offsetHeight : $("#data_tab_jq5_0").get(0).offsetHeight;
        }
        else {
            $("#carCompareFilter").hide();
        }
        this.bindEvent();
    },
    initFilterHtml: function () {
        if (typeof CarFilterData == "undefined" || CarFilterData == null ||
        (CarFilterData.Year.length <= 0 && CarFilterData.Trans.length <= 0 && CarFilterData.Exhaust.length <= 0))
            return;

        var filterHtml = [];
        filterHtml.push("<div class=\"input-group\" id=\"car-filter-year-list\">");
        for (var i = 0; i < CarFilterData.Year.length; i++) {
            //if (i >= 2) break;
            filterHtml.push("<label><input type=\"checkbox\" value=\"" + CarFilterData.Year[i] + "\" name=\"car-filter-year\"> " + CarFilterData.Year[i] + "</label>");
        }
        filterHtml.push("</div >");
        if (CarFilterData.Trans.length > 0) {
            filterHtml.push("<div class=\"input-group\" id=\"car-filter-tran-list\">");
            for (var i = 0; i < CarFilterData.Trans.length; i++) {
                filterHtml.push("<label><input type=\"checkbox\" value=\"" + CarFilterData.Trans[i] + "\" name=\"car-filter-tran\"> " + CarFilterData.Trans[i] + "</label>");
            }
            filterHtml.push("</div>");
        }
        if (CarFilterData.Exhaust.length > 0) {
            filterHtml.push("<div class=\"input-group\" id=\"car-filter-exhaust-list\">");
            for (var i = 0; i < CarFilterData.Exhaust.length; i++) {
                filterHtml.push("<label><input type=\"checkbox\" value=\"" + CarFilterData.Exhaust[i] + "\" name=\"car-filter-exhaust\"> " + CarFilterData.Exhaust[i] + "</label>");
            }
            filterHtml.push("</div>");
        }
        filterHtml.push("<div class=\"input-group\">");
        filterHtml.push("    <a class=\"action-btn\" href=\"javascript:;\" id=\"car-filter-clear\">清空</a>");
        filterHtml.push("</div>");
        $("#carCompareFilter").html(filterHtml.join('')).show();;
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
        this.CarFilterPosition($("#carCompareFilter"), $("#carCompareFilter").offset().top);
    },
    getNoSaleYearData: function (year) {
        $.ajax({
            url: "/AjaxNew/GetNoSaleSerailListByYear.ashx?csID=" + serialId + "&year=" + year, dataType: "json", cache: true,
            success: function (json) {
                if (json.length > 0) {
                    //初始化车款列表        
                    var divContentArray = new Array();
                    divContentArray.push("<colgroup><col width=\"40%\"><col width=\"8%\"><col width=\"14%\"><col width=\"10%\"><col width=\"11%\"><col width=\"17%\"></colgroup>");
                    divContentArray.push("<tbody>");
                    $(json).each(function (index, item) {
                        divContentArray.push("<tr id=\"car_filter_gid_" + index + "\" class=\"table-tit\">");
                        var MaxPower = item.MaxPower;
                        if (MaxPower != "") {
                            MaxPower = "<b>/</b>" + MaxPower
                        }
                        divContentArray.push("<th class=\"first-item\"><div class=\"pdL10\"><strong>" + item.Engine_Exhaust + "</strong>" + MaxPower + " " + item.InhaleType + "</div>");
                        divContentArray.push("</th>");
                        divContentArray.push("<th>关注度</th>");
                        divContentArray.push("<th>变速箱</th>");
                        divContentArray.push("<th class=\"txt-right txt-right-padding\">指导价</th>");
                        divContentArray.push("<th class=\"txt-right\">二手车报价</th>");
                        divContentArray.push("<th >&nbsp;</th>");
                        divContentArray.push("</tr>");

                        for (var i = 0; i < item.carList.length; i++) {
                            var stopPrd = "";
                            var strEnergySubsidy = "";
                            var strTravelTax = "";
                            var fuelTypeStr = "";
                            var referPrice = "暂无"
                            if (item.carList[i].StopPrd == "停产") {
                                stopPrd = "<span class=\"color-block3\">停产</span>";
                            }
                            if (item.carList[i].hasEnergySubsidy == "True") {
                                strEnergySubsidy = "<a href=\"http://news.bitauto.com/others/20150605/1006534895.html\" class=\"color-block2\" title=\"可获得3000元节能补贴\" target=\"_blank\">补贴</a>";
                            }
                            if (item.carList[i].isTravelTax == "True") {
                                strTravelTax = " <a target=\"_blank\" title=\"" + item.carList[i].strTravelTax + "\" href=\"http://news.bitauto.com/others/20120308/0805618954.html\" class=\"color-block2\">减税</a>";
                            }

                            if (item.carList[i].Oil_FuelType == "油电混合动力" || item.carList[i].Oil_FuelType == "油气混合动力") {
                                fuelTypeStr = "<span class=\"color-block2\">混动</span>";
                            }
                            if (item.carList[i].ReferPrice != "") {
                                referPrice = item.carList[i].ReferPrice
                            }
                            divContentArray.push("<tr id=\"car_filter_id_" + item.carList[i].CarID + "\">");
                            divContentArray.push("<td class=\"txt-left\">");
                            divContentArray.push("<a id=\"carlist_" + item.carList[i].CarID + "\" href=\"/" + item.carList[i].Spell + "/m" + item.carList[i].CarID + "/\" target=\"_blank\" data-channelid=\"2.21.807\"  class=\"txt\">" + item.carList[i].YearType + " " + item.carList[i].Name + "</a> " + fuelTypeStr + strTravelTax + strEnergySubsidy + stopPrd);
                            divContentArray.push("<a href=\"/" + item.carList[i].Spell + "/m" + item.carList[i].CarID + "/\" target=\"_blank\" class=\"abs-a\"></a>");
                            divContentArray.push("</td>");
                            divContentArray.push("<td>");
                            divContentArray.push("<div class=\"w\">");
                            divContentArray.push("<div class=\"p\" style=\"width: " + item.carList[i].Percent + "%\"></div>");
                            divContentArray.push("</div>");
                            divContentArray.push("</td>");

                            divContentArray.push("<td>" + item.carList[i].ForwardGearNum + item.carList[i].TransmissionType + "</td>");
                            divContentArray.push("<td class=\"txt-right overflow-visible\"><span>" + referPrice + "</span><a class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + item.carList[i].CarID + "\" target=\"_blank\" data-channelid=\"2.21.852\" data-use=\"compute\" carid=\"" + item.carList[i].CarID + "\"></a></td>");
                            if (item.carList[i].UCarPrice == "")
                                divContentArray.push("<td class=\"txt-right\"><span>暂无报价</span></td>");
                            else {
                                var minPrice = item.carList[i].UCarPrice.split('-')[0];
                                divContentArray.push("<td class=\"txt-right\"><span><a href=\"http://www.taoche.com/all/?carid=" + item.carList[i].CarID + "&ref=pc_yc_zs_tsck_esc\" target=\"_blank\">" + minPrice + "万</a></span></td>");

                            }
                            divContentArray.push("<td class=\"txt-right\">");
                            divContentArray.push("<a href=\"http://www.taoche.com/all/?carid=" + item.carList[i].CarID + "&ref=pc_yc_zs_tsck_esc&leads_source=p002023\" class=\"btn btn-primary btn-xs\" data-channelid=\"2.21.851\" target=\"_blank\">二手车</a>");
                            divContentArray.push(" <a target=\"_self\" class=\"btn btn-secondary btn-xs\" data-use=\"compare\" data-id=\"" + item.carList[i].CarID + "\" href=\"javascript:;\" data-channelid=\"2.21.850\"><span>+对比</span></a>");
                            divContentArray.push("</td>");
                            divContentArray.push("</tr>");
                        }
                    });
                    divContentArray.push("</tbody>");
                    var divContent = divContentArray.join("");
                    $("#compare_nosale").html(divContent);
                    //$("#data_tab_jq5_2 tr[id^='car_filter_id_']").hover(
				    //    function () {
				    //        $(this).addClass('hover-bg-color');
				    //    },
				    //    function () {
				    //        $(this).removeClass('hover-bg-color');
				    //    }
				    //);
                    Bglog_InitPostLog();
                    typeof InitCompareEvent == "function" && InitCompareEvent();
                }
            }
        });
    },
    SpecialEffects: function () {
        // 层浮动
        if ($("#carCompareFilter").length <= 0) return false;

        var carFilter = $("#carCompareFilter"),
			carCFtopHeight = carFilter.offset().top,
			self = this;

        $(window).bind("scroll", function () {
            self.CarFilterPosition(carFilter, carCFtopHeight);
        });

        $(window).bind("resize", function () {
            self.CarFilterPosition(carFilter, carCFtopHeight);
        });
    },
    CarFilterPosition: function (carFilter, carCFtopHeight) {
        var carCFscrollHeight = document.body.scrollTop || document.documentElement.scrollTop,
            self = this;

        if (self.CarListHeight <= self.FilterHeight) {
            carFilter.css({ "position": "absolute", "top": "0px", "left": "-75px" });
            return false;
        } else {
            if (carCFscrollHeight > (carCFtopHeight + self.CarListHeight - self.FilterHeight)) {
                carFilter.css({ "position": "absolute", "top": (self.CarListHeight - self.FilterHeight) + "px", "left": "-75px" });
            } else if ((carCFscrollHeight + self.OffsetTop) >= carCFtopHeight) {
                carFilter.css({ "position": "fixed", "top": self.OffsetTop + "px", "left": ($(".list-table").offset().left - $(".input-group").width() - 23) + "px" });
            } else {
                carFilter.css({ "position": "absolute", "top": "0px", "left": "-75px" });
            }
        }
    }
}