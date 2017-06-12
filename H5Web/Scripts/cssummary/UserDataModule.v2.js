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

//热门点评
function getKouBei() {
    var name = "page6";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function(data) {
        $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='" + name + "']");

        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    });
}

//获取车款列表
function getCarList() {
    var name = "page7";
    var index = Config.auchors.indexOf(name);
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
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    });
}

function getYouHuiGouChe() {
    var name = "page8";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }

    //$("#gouchetmplnew").tmpl("").appendTo("div[data-anchor='" + name + "']");
    $("div[data-anchor='" + name + "']").html($("#gouchetmplnew").html());

    $("#fullpage").fullpage.resetSlides(index);
    //最后一页去掉向下箭头
    if (Config.auchors[Config.auchors.length - 1] === name) {
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

}

//经销商
function getDealer() {
    var name = "page9";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetDataAsynV4.ashx?service=dealer&method=userdealerstaticmap&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
        if (checkData(data)) {
            var html = $.trim(data);
            var wrapper = $("div[data-anchor='" + name + "']");
            $("#userdealertmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");
            $("#map_detail,#map_img", wrapper).attr("href", "/V4/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&cityid=" + cityId);
        } else {
            $("#userdealertmpl").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
        }
        var index = Config.auchors.indexOf(name);
        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    });
}

//养护
function getYanghu() {
    var name = "page11";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetDataAsynV4.ashx?service=yanghu&method=yanghu&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
        var html = data.replace(/\r\n/ig, "");
        if (html.length > 0 && checkData(data)) {
            $("#yanghutmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");
            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        } else {
            $("div[data-anchor='" + name + "']").remove();
            $("#fullpage").fullpage.reBuild();
        }

    });
}

//获取看了还看信息
function getSeeAgain() {
    var name = "page12";
    var index = Config.auchors.indexOf(name);
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
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
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
        var name = "page10";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        if (Config.carMinReferPrice && parseFloat(Config.carMinReferPrice) > 0) {
            CarCalculator.ReferPrice = parseFloat(Config.carMinReferPrice) * 10000;
            $("#baoxiantmpl").tmpl({ Compulsory: CarCalculator.GetCompulsory(), CommonTotal: CarCalculator.GetCommonTotal() }).appendTo("div[data-anchor='" + name + "']");
        } else {
            $("#baoxiantmpl").tmpl({ Compulsory: 0, CommonTotal: 0 }).appendTo("div[data-anchor='" + name + "']");
        }
        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
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

function getErShouCheInfo() {
    var name = "page14";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetDataAsynV4.ashx?service=ershouche&method=ershouche&csid=" + Config.serialId + "&cityid=" + cityId + "&", function(data) {
        if (data != "" && data != null && checkData(data)) {
            $("#ershouchetmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        } else {
            //$("#nodatatmpl").tmpl({ title: "二手车" }).appendTo("div[data-anchor='" + name + "']");
            $("div[data-anchor='" + name + "']").remove();
        }
    });
}


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
//二手车
getErShouCheInfo();