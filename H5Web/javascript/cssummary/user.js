define(["anchors", "jquery", "util", "jquerytmpl", "jqueryfullpage"], function(anchors, $, util) {

    return {

        //热门点评
        remendianping: function(name) {
            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function(data) {
                $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='" + name + "']");

                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        },

        //车款列表
        chekuanliebiao: function(name) {
            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/getcarlist.ashx?top=19&csid=" + Config.serialId + "&", function(data) {
                if (!data) {
                    return;
                }
                var length = data.carlist.length;
                var listGroup = [];
                var list = data.carlist;
                for (var i = 0; i < Math.ceil(length / 4); i++) {
                    listGroup.push({ "index": i, "carlist": list.slice(i * 4, (i + 1) * 4) });
                }
                $("#carlisttmp").tmpl({ "carcount": data.count, "listgroup": listGroup }).appendTo("div[data-anchor='" + name + "']");

                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        },

        //优惠购车
        youhuigouche: function(name) {

            var WTmc_id = "";
            var tempVarid = util.GetQueryStringByName("WT.mc_id");
            if (tempVarid != null) {
                WTmc_id = tempVarid;
            }
            var WTmc_jz = "";
            var tempVarjz = util.GetQueryStringByName("WT.mc_jz");
            if (tempVarjz != null) {
                WTmc_jz = tempVarjz;
            }


            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }

            $("div[data-anchor='" + name + "']").html($("#gouchetmplnew").html());

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }

            if (WTmc_id && WTmc_id != "") {
                $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_id=" + WTmc_id);
            } else if (WTmc_jz && WTmc_jz != "") {
                $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_jz=" + WTmc_jz);
            } else {
                $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId);
            }

            if (Config.isAd === 0) {
                $("#youhuiadwrapper").remove();
            } else {
                $("#div_655a7bd1-d2bb-48ad-bb54-7a0ab4e99a77").appendTo("#youhuiadwrapper").show();
            }

            //惠买车
            $.ajax({
                type: "GET",
                url: "http://www.huimaiche.com/api/GetCarSerialSaleDataNew.ashx?csid=" + Config.serialId + "&ccode=" + cityId,
                async: true,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "huimaiche",
                success: function(data) {
                    if (typeof data["SaveMoney"] != "undefined" && data["SaveMoney"] != "" && typeof data["Description"] != "undefined" && typeof data["OrderUrl"] != "undefined") {
                        var huimaiche = {};
                        huimaiche.SaveMoney = data["SaveMoney"];
                        huimaiche.Description = data["Description"];

                        var targetLink = "";
                        if (WTmc_id && WTmc_id != "") {
                            targetLink = data["OrderUrl"] + "&tracker_u=269_ycdsj_" + WTmc_id;
                        } else if (WTmc_jz && WTmc_jz != "") {
                            targetLink = data["OrderUrl"] + "&tracker_u=269_ycdsj_" + WTmc_jz;
                        } else {
                            targetLink = data["OrderUrl"] + "&tracker_u=269_ycdsj";
                        }
                        huimaiche.OrderUrl = targetLink;

                        $("#huimaichetmpl").tmpl(huimaiche).appendTo("#li_huimaiche");
                        $("#li_huimaiche").show();
                    }
                }
            });

            //商城
            $.ajax({
                type: "GET",
                url: "http://api.yichemall.com/forth/car/get?csId=" + Config.serialId + "&cityId=" + cityId,
                async: true,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "shangcheng",
                success: function(data) {
                    if (typeof data["Price"] != "undefined" && typeof data["Description"] != "undefined" && typeof data["Url"] != "undefined") {
                        var yicheshangcheng = {};
                        yicheshangcheng.Price = data["Price"];
                        yicheshangcheng.Description = data["Description"];
                        yicheshangcheng.Url = data["Url"];

                        $("#yicheshangchengtmpl").tmpl(yicheshangcheng).appendTo("#li_yicheshangcheng");
                        $("#li_yicheshangcheng").show();
                    }
                }
            });

            //易车惠
            $.ajax({
                type: "GET",
                url: "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=Disiji&csid=" + Config.serialId + "&cityid=" + cityId,
                async: true,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "yichehui",
                success: function(data) {
                    if (data["data"] != null && typeof data["data"]["OrderUrl"] != "undefined" && typeof data["data"]["Description"] != "undefined" && typeof data["data"]["YCHTitle"] != "undefined") {
                        var yichehui = {};
                        yichehui.OrderUrl = data["data"].OrderUrl;
                        yichehui.Description = data["data"].Description;
                        yichehui.YCHTitle = data["data"].YCHTitle;

                        $("#yichehuitmpl").tmpl(yichehui).appendTo("#li_yichehui");
                        $("#li_yichehui").show();
                    }
                }
            });

            //贷款
            $.ajax({
                type: "GET",
                url: "http://api.chedai.bitauto.com/api/other/GetDSJProducts?csid=" + Config.serialId + "&cityid=" + cityId,
                async: true,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "daikuai",
                success: function(data) {
                    if (data["Data"] != null && typeof data["Data"]["Downpayment"] != "undefined" && typeof data["Data"]["Feature"] != "undefined" && typeof data["Data"]["Monthpay"] != "undefined" && typeof data["Data"]["Url"] != "undefined") {
                        var yichedaikuai = {};
                        yichedaikuai.Downpayment = data["Data"]["Downpayment"];
                        yichedaikuai.Feature = data["Data"]["Feature"];
                        yichedaikuai.Monthpay = data["Data"]["Monthpay"];
                        yichedaikuai.Url = data["Data"]["Url"];

                        $("#yichedaikuaitmpl").tmpl(yichedaikuai).appendTo("#li_yichedaikuai");
                        $("#li_yichedaikuai").show();
                    }
                }
            });

        },

        //经销商
        jingxiaoshang: function(name) {
            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=userdealerstaticmap&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
                if (util.CheckData(data)) {
                    var html = $.trim(data);
                    var wrapper = $("div[data-anchor='" + name + "']");
                    $("#userdealertmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");
                    $("#map_detail,#map_img", wrapper).attr("href", "/V3/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&cityid=" + cityId);
                } else {
                    $("#userdealertmpl").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
                }
                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        },

        //养护
        yanghu: function(name) {

            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/GetDataAsynV3.ashx?service=yanghu&method=yanghu&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
                var html = data.replace(/\r\n/ig, "");
                if (html.length > 0 && util.CheckData(data)) {
                    $("#yanghutmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");
                    $("#fullpage").fullpage.resetSlides(index);
                    //最后一页去掉向下箭头
                    if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                        $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                    }
                } else {
                    $("div[data-anchor='" + name + "']").remove();
                    $("#fullpage").fullpage.reBuild();
                }

            });
        },

        //看了还看
        kanlehaikan: function(name) {

            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/SeeAgain.ashx?csid=" + Config.serialId + "&20151026&", function(data) {
                if (!data) {
                    return;
                }
                $("div[data-anchor='" + name + "']").html(data);

                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        },

        //二手车
        ershouche: function(name) {
            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            $.get("/handlers/GetDataAsynV3.ashx?service=ershouche&method=ershouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
                if (data != "" && data != null && util.CheckData(data)) {
                    $("#ershouchetmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
                    $("#fullpage").fullpage.resetSlides(index);
                    //最后一页去掉向下箭头
                    if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                        $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                    }
                } else {
                    //$("#nodatatmpl").tmpl({ title: "二手车" }).appendTo("div[data-anchor='" + name + "']");
                    $("div[data-anchor='" + name + "']").remove();
                }
            });
        },

        //保险
        baoxian: function(name) {
            var carCalculator = {
                //所有险种均按最低价算
                ReferPrice: 0, //车款裸价
                Compulsory: 0, //交强险
                CommonTotal: 0, //商业险合计
                GetCommonTotal: function() {
                    var calcTPL = carCalculator.CalcTPL();
                    var calcCarDamage = carCalculator.CalcCarDamage();
                    var calcAbatement = carCalculator.CalcAbatement(calcCarDamage, calcTPL);
                    var calcCarTheft = carCalculator.CalcCarTheft();
                    var calcBreakageOfGlass = carCalculator.CalcBreakageOfGlass();
                    var calcCarCalculatorignite = carCalculator.CalcCarCalculatorignite();
                    var calcCarEngineDamage = carCalculator.CalcCarEngineDamage(calcCarDamage);
                    var calcCarDamageDW = carCalculator.CalcCarDamageDW();
                    var calcLimitofDriver = carCalculator.CalcLimitofDriver();
                    var calcLimitofPassenger = carCalculator.CalcLimitofPassenger();
                    return calcTPL + calcCarDamage + calcAbatement + calcCarTheft + calcBreakageOfGlass + calcCarCalculatorignite + calcCarEngineDamage + calcCarDamageDW + calcLimitofDriver + calcLimitofPassenger;
                },
                CalcTPL: function() { //第三者责任险
                    return 710;
                },
                CalcCarDamage: function() { //车辆损失险
                    return Math.round(carCalculator.ReferPrice * 0.0095) + 285;
                },
                CalcAbatement: function(calcCarDamage, calcTPL) { //不计免赔特约险
                    return Math.round((util.GetIntValue(calcCarDamage) + util.GetIntValue(calcTPL)) * 0.2);
                },
                CalcCarTheft: function() { //全车盗抢险
                    return Math.round(carCalculator.ReferPrice * 0.0049 + 120);
                },
                CalcBreakageOfGlass: function() { //玻璃单独破碎险
                    return Math.round(carCalculator.ReferPrice * 0.0019);
                },
                CalcCarCalculatorignite: function() { //自燃损失险
                    return Math.round(carCalculator.ReferPrice * 0.0015);
                },
                CalcCarEngineDamage: function(calcCarDamage) { //发动机特别损失险(车损险*5%)
                    return Math.round(util.GetIntValue(calcCarDamage) * 0.05);
                },
                CalcCarDamageDW: function() { //车身划痕险
                    return 400;
                },
                CalcLimitofDriver: function() { //司机座位责任险
                    return 42; //10000*0.0042
                },
                CalcLimitofPassenger: function() {
                    return 10000 * 0.0027 * 4; //默认4个座
                },
                GetCompulsory: function() { //交强险
                    return 950;
                }
            };
            var index = anchors.Auchors.indexOf(name);
            if (index < 0) {
                $("div[data-anchor='" + name + "']").remove();
                return;
            }
            if (Config.carMinReferPrice && parseFloat(Config.carMinReferPrice) > 0) {
                carCalculator.ReferPrice = parseFloat(Config.carMinReferPrice) * 10000;
                $("#baoxiantmpl").tmpl({ Compulsory: carCalculator.GetCompulsory(), CommonTotal: carCalculator.GetCommonTotal() }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#baoxiantmpl").tmpl({ Compulsory: 0, CommonTotal: 0 }).appendTo("div[data-anchor='" + name + "']");
            }
            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        }

    };

})