﻿var PingceYouhui = {
    NavUlId: "leftUl",
    ContentDivId: "content_bit",
    SerialId: 0,
    CityId: 0,
    CarId: 0,
    BaseInfoHtmlArray: new Array(),
    SaleCarHtmlArray: new Array(),
    BaseInfoUrl: "http://api.gouche.yiche.com/PreferentialCar?serialid={serialId}&cityid={cityId}",
    SaleCarUrl: "http://api.gouche.yiche.com/SalesCar/GetRecommendCarByCarId/?carid={carId}&cityid={cityId}",
    CheDaiUrl: "http://openapi.chedai.bitauto.com/BuyCarService/GetCarLoanDetail?carId={carId}&cityId={cityId}",
    RemarkArray: ["", "覆盖千款车型，由多家认证经销商4s店为您实时报价，更有买车顾问免费陪同到店， 助您轻松底价买好车！"
        , "易车商城购车，保证支付款资金安全，未购车随时退款，4S店提供交车服务，购车全程有客服。"
        , "易车网特卖频道为您提供最新优惠促销信息。直观的折扣报价，最大程度的让利，为您能买到心仪的爱车提供便利。"
        , "在易车惠下单购车，在4S店优惠基础上叠加享受厂商补贴。不购车订金全额退款。"
        , "百款金融产品，低利率、放款快，为您量身定做的购车货款金融解决方案，轻松实现购车梦。"],
    Init: function (serialId, cityId, currentLi, baseUrl) {
        if (serialId < 1 || cityId < 1) {
            return;
        }
        var self = this;
        self.SerialId = serialId;
        self.CityId = cityId;
        self.AddNavLi(currentLi, baseUrl);
    },
    AddNavLi: function (currentLi, baseUrl) {
        var self = this;
        if (currentLi == "100") {
            $("#" + self.NavUlId).append("<li class=\"current\">优惠</li>");
            self.GetBaseInfo();
        }
        else {
            var rurl = self.BaseInfoUrl.replace("{serialId}", self.SerialId).replace("{cityId}", self.CityId);
            $.ajax({
                url: rurl,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "BaseInfo",
                success: function (data) {
                    if (data && data["CarId"] > 0) {
                        $("#" + self.NavUlId).append("<li><a href=\"" + baseUrl + "100/\">优惠</a></li>");
                    }
                }
            });
        }
    },
    AddLiClickEvent: function () {
        var self = this;
        $("#" + self.NavUlId + " > li[group='youhui']").click(function () {
            var curLi = $("#" + self.NavUlId + " > li[class='current']");
            $(curLi).html("<a href=\"" + window.location.href.replace(window.location.protocol + "//" + window.location.host, "") + "\">" + $(curLi).html() + "</a>").removeClass("current");
            $(this).addClass("current").removeAttr("group").html("优惠");
            $(this).unbind("click");
            self.GetYouhuiContent();
            $(window).scrollTop(0);
            $("#float").css("margin-top", "0");
        });
    },
    //GetYouhuiContent: function () {
    //    var self = this;
    //    var youhuili = $("#" + self.NavUlId + " > li[group='youhui']");
    //    $(youhuili).addClass("current").removeAttr("group").html("优惠");
    //    self.GetBaseInfo();
    //},
    GetBaseInfo: function () {
        var self = this;
        var url = self.BaseInfoUrl.replace("{serialId}", self.SerialId).replace("{cityId}", self.CityId);
        self.GetData(url, self.AddBaseInfoHtml, self.GetSaleCar, "baseinfo");
    },
    GetSaleCar: function () {
        var self = PingceYouhui;
        var rurl = self.SaleCarUrl.replace("{carId}", self.CarId).replace("{cityId}", self.CityId);
        self.GetData(rurl, self.AddSaleCarHtml, self.GetCheDai, "yuhuiData");
    },
    GetCheDai: function () {
        var self = PingceYouhui;
        var rurl = self.CheDaiUrl.replace("{carId}", self.CarId).replace("{cityId}", self.CityId);
        self.GetData(rurl, self.AddCheDaiHtml, self.FillArticleContent, "cheDaiData");
    },
    GetData: function (rurl, callback, complatecallback, jsonpCallbackStr) {
        $.ajax({
            url: rurl,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: jsonpCallbackStr,
            complete: function () {
                if (typeof complatecallback == "function") {
                    complatecallback();
                }
            },
            success: function (data) {
                if (typeof callback == "function") {
                    callback(data);
                }
            }
        });
    },
    AddBaseInfoHtml: function (data) {
        var self = PingceYouhui;
        if (data) {
            $("h1[class='tit-h1']").html("" + data["SerialName"] + "优惠信息");
            var nowDate = new Date();
            $("#time").html(nowDate.getFullYear() + "年" + (nowDate.getMonth() + 1) + "月" + nowDate.getDate() + "日");
            $("span[class='author']").remove();
            $("div[class='article_share']").remove();
            self.CarId = data["CarId"];
            self.BaseInfoHtmlArray.push("<div class=\"figure-240x160\">");
            self.BaseInfoHtmlArray.push("  <a href=\"/" + data["AllSpell"] + "\" class='figure' target=\"_blank\"><img src=\"" + data["ImageUrl"] + "\"></a>");
            self.BaseInfoHtmlArray.push("  <div class=\"txt\">");
            self.BaseInfoHtmlArray.push("     <h3 class='title'><a href=\"/" + data["AllSpell"] + "\" target=\"_blank\">" + data["SerialName"] + "</a></h3>");
            self.BaseInfoHtmlArray.push("    <p class='p-1'>参考优惠价：<a href=\"/" + data["AllSpell"] + "\" target=\"_blank\" class='price-now'>" + data["MinPrice"] + "万起</a></p>");
            self.BaseInfoHtmlArray.push("    <p class='p-2'>厂商指导价：<a href=\"/" + data["AllSpell"] + "\" target=\"_blank\" class='price-origin'>" + data["MinCarReferPrice"] + "万起</a></p></div>");
            self.BaseInfoHtmlArray.push("    <a href=\"/" + data["AllSpell"] + "/peizhi/\" target=\"_blank\" class=\"btn btn-default btn-sm\">参数配置</a>");
            self.BaseInfoHtmlArray.push(" </div>");         
        }
    },
    AddSaleCarHtml: function (data) {
        var self = PingceYouhui;
        if (data) {
            for (var i = 0; i < data.length; i++) {
                var ref = "";
                if (data[i]["ProductType"] == 1) {
                    ref = "&leads_source=p008001";
                }
                else if (data[i]["ProductType"] == 2) {
                    ref = "&leads_source=p008002";
                }
                else if (data[i]["ProductType"] == 3) {
                    ref = "&leads_source=p008003";
                }
                else if (data[i]["ProductType"] == 4) {
                    ref = "?leads_source=p008004";
                }
                self.SaleCarHtmlArray.push("<div class='special-layout-1'>");
                self.SaleCarHtmlArray.push("  <div class='main'>");
                self.SaleCarHtmlArray.push("      <div class=\"title\"> <a href=\"" + data[i]["Url"] + ref + "\"  target=\"_blank\"><img src=\"http://image.bitautoimg.com/uimg/news/images/pt_name0" + data[i]["ProductType"] + ".png\"></a></div>");
                self.SaleCarHtmlArray.push("        <p>" + self.RemarkArray[data[i]["ProductType"]] + "</p>");
                self.SaleCarHtmlArray.push("    </div>");
                self.SaleCarHtmlArray.push("  <div class=\"side\"><h5 class=\"price\"><a href=\"" + data[i]["Url"] + ref + "\ target=\"_blank\"> " + data[i]["MinPrice"] + "万起</a></h5>");
                self.SaleCarHtmlArray.push("     <a href=\"" + data[i]["Url"] + ref + "\" target=\"_blank\" class=\"btn btn-secondary btn-sm\">立即购买</a>");
                self.SaleCarHtmlArray.push("   </div>");
                self.SaleCarHtmlArray.push("   </div>");
            }
            //self.HtmlArray.push("</ul></div>");
        }
    },
    AddCheDaiHtml: function (data) {
        var self = PingceYouhui;
        if (data && data.Data) {
            var url = data.Data["PCLinkUrl"];
            var newUrl = "";
            if (url && url.indexOf("from=") > 0) {
                var urlArr = url.split("?");
                if (urlArr.length > 1) {
                    var parmArr = urlArr[1].split("&");
                    if (parmArr.length > 0) {
                        newUrl = urlArr[0] + "?";
                        for (var i = 0; i < parmArr.length; i++) {
                            if (parmArr[i].split("=")[0] == "from") {
                                continue;
                            }
                            else {
                                newUrl += parmArr[i] + "&";
                            }
                        }
                    }
                }
            }
            if (newUrl == "") {
                newUrl = url;
            }
            else {
                newUrl = newUrl.substr(0, newUrl.length - 1);
            }
            self.SaleCarHtmlArray.push("<div class='special-layout-1'>");
            self.SaleCarHtmlArray.push("  <div class='main'>");
            self.SaleCarHtmlArray.push("      <div class=\"title\"> <a href=\"" + newUrl + "&leads_source=p008005\"  target=\"_blank\"><img src=\"http://image.bitautoimg.com/uimg/news/images/pt_name05.png\"></a></div>");
            self.SaleCarHtmlArray.push("      <p>" + self.RemarkArray[3] + "</p>");
            self.SaleCarHtmlArray.push("  </div>");
            self.SaleCarHtmlArray.push("   <div class=\"side\"> <h5 class=\"price\"><a href=\"" + newUrl + "&leads_source=p008005\" target=\"_blank\">首付" + data.Data["DownPaymentText"] + "起</a></h5>");
            self.SaleCarHtmlArray.push("        <p class=\"price-info\">(月供" + data.Data["MonthlyPaymentMinText"] + "起)</p>");
            self.SaleCarHtmlArray.push("       <a class=\"btn btn-secondary btn-sm\"  href=\"" + newUrl + "&leads_source=p008005\" target=\"_blank\">立即购买</a>");
            self.SaleCarHtmlArray.push("   </div>");
            self.SaleCarHtmlArray.push("</div>");       
        }
    },
    FillArticleContent: function () {
        var self = PingceYouhui;
        var htmlArray = new Array();
        htmlArray.push(self.BaseInfoHtmlArray.join(""));
        htmlArray.push("<div class=\"list-box\">");
        htmlArray.push(self.SaleCarHtmlArray.join(""));
        htmlArray.push("</div>");
        $("#article-content").removeClass("article-content").html(htmlArray.join(""));
        $("#pagePagination").remove();
        if (carContainerHeight && carContainer) {
            carContainerHeight = carContainer.clientHeight;
        }
    }
}


