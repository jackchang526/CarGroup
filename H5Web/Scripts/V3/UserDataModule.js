define("userdata", function() {
    var cityId = !!bit_locationInfo ? bit_locationInfo.cityId : 0;

    var WTmc_id = "";
    var tempVarid = getQueryStringByName("WT.mc_id");
    if (tempVarid != null) {
        WTmc_id = tempVarid;
    }
    var WTmc_jz = "";
    var tempVarjz = getQueryStringByName("WT.mc_jz");
    if (tempVarjz != null) {
        WTmc_jz = tempVarjz;
    }

    function checkPage(pagename) {
        var index = Config.auchors.indexOf(pagename);
        if (index < 0) {
            $("div[data-anchor='" + pagename + "']").remove();
            return false;
        }
        return true;
    }

    //获取优惠购车
    function getYouHuiGouChe() {
        if (!checkPage("page8")) {
            return;
        }
        var logo = Config.DefaultCarPic;
        var huimaiche = { title: "获4S店底价，在线订车", html: "" };
        var yichemall = { title: "买车无难事，商城特卖", html: "" };
        var yichehui = { title: "品牌特卖-官方品牌旗舰店", html: "" };
        var yicheloan = { title: "贷款购车优惠方案", html: "" };
        var yicheershou = { title: "购买二手车，价低有保障", html: "" };
        var obj = {
            logo: logo,
            huimaiche: huimaiche,
            yichemall: yichemall,
            yichehui: yichehui,
            yicheloan: yicheloan,
            yicheershou: yicheershou
        };

        var num = 0, totalNum = 5;

        //惠买车
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=youhuigouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                var html = $.trim(data);

                var link = $(data).find("a").attr("href");
                var targetLink = "";
                if (WTmc_id && WTmc_id != "") {
                    targetLink = link + "&tracker_u=269_ycdsj_" + WTmc_id;
                } else if (WTmc_jz && WTmc_jz != "") {
                    targetLink = link + "&tracker_u=269_ycdsj_" + WTmc_jz;
                } else {
                    targetLink = link + "&tracker_u=269_ycdsj";
                }

                num++;
                obj.huimaiche.html = html;
                obj.huimaiche.targetLink = targetLink;
            } else {
                obj.huimaiche.html = "";
            }
        });
        //易车惠
        $.get("/handlers/GetDataAsynV3.ashx?service=market&method=youhuigouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                var html = $.trim(data);
                num++;
                obj.yichehui.html = html;
            } else {
                obj.yichehui.html = "";
            }
        });
        //贷款
        $.get("/handlers/GetDataAsynV3.ashx?service=daikuan&method=youhuigouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                num++;
                var html = $.trim(data);
                obj.yicheloan.html = html;
            } else {
                obj.yicheloan.html = "";
            }
        });
        //商城
        $.get("/handlers/GetDataAsynV3.ashx?service=yichemall&method=youhuigouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                var html = $.trim(data);
                num++;
                obj.yichemall.html = html;
            } else {
                obj.yichemall.html = "";
            }
        });
        //二手车
        $.get("/handlers/GetDataAsynV3.ashx?service=ershouche&method=youhuigouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                var html = $.trim(data);
                num++;
                obj.yicheershou.html = html;
            } else {
                obj.yicheershou.html = "";
            }
        });
        var seconds = 0;
        var timer = setInterval(function() {
            seconds += 100;
            if (num >= totalNum || seconds >= 5000) {
                $("#gouchetmpl").tmpl(obj).appendTo("div[data-anchor='page8']");
                //$("#fullpage").fullpage.resetSlides(7);
                var index = Config.auchors.indexOf("page8");
                $("#fullpage").fullpage.resetSlides(index);

                //如果只定制一页直接去掉向箭头
                if (Config.auchors.length == 1) {
                    $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
                }

                if (WTmc_id && WTmc_id != "") {
                    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_id=" + WTmc_id);
                } else if (WTmc_jz && WTmc_jz != "") {
                    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_jz=" + WTmc_jz);
                } else {
                    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId);
                }

                $("#huimaiche-div a").attr("href", obj.huimaiche.targetLink);
                if (Config.isAd == 0) {
                    $("#youhuiadwrapper").remove();
                } else {
                    $("#div_655a7bd1-d2bb-48ad-bb54-7a0ab4e99a77").appendTo("#youhuiadwrapper").show();
                }

                clearInterval(timer);
            }
        }, 100);
    }

    //获取看了还看信息
    function getSeeAgain() {
        if (!checkPage("page12")) {
            return;
        }
        $.get("/handlers/SeeAgain.ashx?csid=" + Config.serialId + "&20151026&", function(data) {
            if (!data) {
                return;
            }
            $("div[data-anchor='page12']").html(data);

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //经销商
    function getDealer() {
        if (!checkPage("page9")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=userdealerstaticmap&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            if (checkData(data)) {
                var html = $.trim(data);
                var wrapper = $("div[data-anchor='page9']");
                $("#userdealertmpl").tmpl({ html: html }).appendTo("div[data-anchor='page9']");
                $("#map_detail,#map_img", wrapper).attr("href", "/V3/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&cityid=" + cityId);

                //如果只定制一页直接去掉向箭头
                if (Config.auchors.length == 1) {
                    $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
                }
            } else {
                $("#userdealertmpl").tmpl({ html: "" }).appendTo("div[data-anchor='page9']");
            }
        });
    }

    //热门点评
    function getKouBei() {
        if (!checkPage("page6")) {
            return;
        }
        $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function(data) {
            $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='page6']");
            //$("#fullpage").fullpage.resetSlides(5);
            var index = Config.auchors.indexOf("page6");
            $("#fullpage").fullpage.resetSlides(index);

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //获取车款列表
    function getCarList() {
        if (!checkPage("page7")) {
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
            var tmpl = $("#carlisttmp").tmpl({ "carcount": data.count, "listgroup": listGroup }).appendTo("div[data-anchor='page7']");
            var index = Config.auchors.indexOf("page7");
            $("#fullpage").fullpage.resetSlides(index);
            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //养护
    function getYanghu() {
        if (!checkPage("page11")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=yanghu&method=yanghu&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
            var html = data.replace(/\r\n/ig, "");
            if (html.length > 0 && checkData(data)) {
                $("#yanghutmpl").tmpl({ html: html }).appendTo("div[data-anchor='page11']");
                var index = Config.auchors.indexOf("page11");
                $("#fullpage").fullpage.resetSlides(index);
                //如果只定制一页直接去掉向箭头
                if (Config.auchors.length == 1) {
                    $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
                }
            } else {
                $("div[data-anchor='page11']").remove();
            }
        });
    }

    function getIntValue(num) {
        num = num.toString().replace(/\,/g, "");
        return parseInt(num);
    }

    var CarCalculator = {
        //所有险种均按最低价算
        ReferPrice: 0, //车款裸价
        Compulsory: 0, //交强险
        CommonTotal: 0, //商业险合计
        Init: function() {
            if (!checkPage("page10")) {
                return;
            }
            if (Config.carMinReferPrice && parseFloat(Config.carMinReferPrice) > 0) {
                CarCalculator.ReferPrice = parseFloat(Config.carMinReferPrice) * 10000;
                $("#baoxiantmpl").tmpl({ Compulsory: CarCalculator.GetCompulsory(), CommonTotal: CarCalculator.GetCommonTotal() }).appendTo("div[data-anchor='page10']");
            } else {
                $("#baoxiantmpl").tmpl({ Compulsory: 0, CommonTotal: 0 }).appendTo("div[data-anchor='page10']");
            }
            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        },
        GetCommonTotal: function() {
            var calcTPL = CarCalculator.CalcTPL();
            var calcCarDamage = CarCalculator.CalcCarDamage();
            var calcAbatement = CarCalculator.CalcAbatement(calcCarDamage, calcTPL);
            var calcCarTheft = CarCalculator.CalcCarTheft();
            var calcBreakageOfGlass = CarCalculator.CalcBreakageOfGlass();
            var calcCarCalculatorignite = CarCalculator.CalcCarCalculatorignite();
            var calcCarEngineDamage = CarCalculator.CalcCarEngineDamage(calcCarDamage);
            var calcCarDamageDW = CarCalculator.CalcCarDamageDW();
            var calcLimitofDriver = CarCalculator.CalcLimitofDriver();
            var calcLimitofPassenger = CarCalculator.CalcLimitofPassenger();
            return calcTPL + calcCarDamage + calcAbatement + calcCarTheft + calcBreakageOfGlass + calcCarCalculatorignite + calcCarEngineDamage + calcCarDamageDW + calcLimitofDriver + calcLimitofPassenger;
        },
        CalcTPL: function() { //第三者责任险
            return 710;
        },
        CalcCarDamage: function() { //车辆损失险
            return Math.round(CarCalculator.ReferPrice * 0.0095) + 285;
        },
        CalcAbatement: function(calcCarDamage, calcTPL) { //不计免赔特约险
            return Math.round((getIntValue(calcCarDamage) + getIntValue(calcTPL)) * 0.2);
        },
        CalcCarTheft: function() { //全车盗抢险
            return Math.round(CarCalculator.ReferPrice * 0.0049 + 120);
        },
        CalcBreakageOfGlass: function() { //玻璃单独破碎险
            return Math.round(CarCalculator.ReferPrice * 0.0019);
        },
        CalcCarCalculatorignite: function() { //自燃损失险
            return Math.round(CarCalculator.ReferPrice * 0.0015);
        },
        CalcCarEngineDamage: function(calcCarDamage) { //发动机特别损失险(车损险*5%)
            return Math.round(getIntValue(calcCarDamage) * 0.05);
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

    //优惠购车
    getYouHuiGouChe();
    //看了还看
    getSeeAgain();
    //经销商
    getDealer();
    //点评信息
    getKouBei();
    //车款信息
    getCarList();
    //养护信息
    getYanghu();
    //汽车保险
    CarCalculator.Init();
});