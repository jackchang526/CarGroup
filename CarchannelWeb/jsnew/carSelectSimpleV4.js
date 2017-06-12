/*saleStatus: 可选
    0:包含在销，待销，停销，待查；旗下是否有车型不限
    1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
    2:包含在销，待销；非概念车；旗下是否有车型不限；
    3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
    4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；
    5:包含：在销，待销，停销，待查，非概念车，旗下必须有车款；
    6:包含：包含在销；非概念车；旗下是否有车型不限；
    7:包含在销，待销，停销；非概念车；旗下是否有车型不限；
level:1:品牌，2：子品牌，3：车款. 可选
titleformat:{"master":{"deftxt":"车系","length":10,"moretxt":"..."},"serial":{"deftxt":"车款","length":10,"moretxt":"..."}}//deftxt为空，显示品牌名称，可选
callback:{"mastercallback":function(){},"serialcallback":function(){},"cartypecallback":function(){}} //回调函数，可选
container:"",//容器id，必须项
*/
function CarSelectSim(options) {
    this.url = "http://api.car.bitauto.com/CarInfo/masterbrandtoserialforsug.ashx?"
    this.parmas = "type={type}&pid={pid}&rt={rt}",
    this.iscontinue = true,
    this.defaults = {
        salestatus: 4,
        level: 3,
        titleformat: {},
        callback: {},
        container: "",
    },
    this.Data = {};
    $.extend(true, this.defaults, options);
    this.defaults.container = options.container;
    this.close();
    this.showMaster();
}
CarSelectSim.prototype.showMaster = function () {
    var _this = this;
    if (_this.defaults.container == "") {
        return;
    }
    if (!_this.iscontinue) {
        return;
    }
    _this.iscontinue = false;
    var url = _this.url + _this.format(_this.parmas, { "type": _this.defaults.salestatus, "pid": "0", "rt": "master", "callback": "CarSelectSim.FillMaster" });
    _this.getData(url, "master");
}
CarSelectSim.prototype.FillMaster = function (data) {//填充品牌
    if (!data) return;
    var _this = this;
    var charList = data["CharList"];
    var dataList = data["DataList"];
    var tempHtml = new Array();
    tempHtml.push("<div class=\"zcfcbox clearfix\" id=\"matser-div\" style=\"position:absolute\" group=\"master\">");
    tempHtml.push("<div class=\"fuctit\" group=\"master\"><ul class=\"ppname\"><li>品牌<i></i></li></ul></div>");
    tempHtml.push("<div class=\"doenbox clearfix\">");

    if (charList && charList.length > 0) {
        tempHtml.push("<div class=\"pinpzm\" id=\"master-index_letters\">");
        for (var i = 0; i < charList.length ; i++) {
            tempHtml.push("<div data-name=\"" + charList[i] + "\"" + (i == 0 ? " class=\"on\"" : "") + "><a href=\"javascript:;\">" + charList[i] + "</a></div>");
        }
        tempHtml.push("</div>");
    }
    tempHtml.push("<div class=\"pinp-rit\"><div class=\"pinp-main pinptext\" id=\"master-index_list\">");

    if (dataList && dataList.length > 0) {
        var charSpell = dataList[0]["tSpell"];
        tempHtml.push("<div class=\"pinp-main-zm\" id=\"master-indexletters_" + charSpell + "\">");
        for (var i = 0; i < dataList.length; i++) {
            if (charSpell != dataList[i]["tSpell"]) {
                charSpell = dataList[i]["tSpell"];
                tempHtml.push("</div><div class=\"pinp-main-zm\" id=\"master-indexletters_" + charSpell + "\">");
            }
            tempHtml.push("<p><a href=\"javascript:;\" data-id=\"" + dataList[i]["id"] + "\" data-name=\"" + dataList[i]["name"] + "\" name=\"master\">" + dataList[i]["tSpell"] + " " + dataList[i]["name"] + "</a></p>");
        }
        tempHtml.push("</div>");
    }

    tempHtml.push("<div></div></div></div>");
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();
    _this.addMasterEvent();
}

CarSelectSim.prototype.addMasterEvent = function () {
    var _this = this;
    $("#" + _this.defaults.container).find("div[id='master-index_letters'] > div").each(function () { //字母点击事件
        $(this).click(function (ev) {
            ev.stopPropagation();
            var charSpell = $(this).attr("data-name");
            //var elem = document.getElementById("master-indexletters_" + charSpell);
            var elem = $("#" + _this.defaults.container).find("div[id='master-indexletters_" + charSpell + "']");
            var top = _this.MathOffsetTop(elem[0], "matser-div");
            $("#" + _this.defaults.container).find("div[id='master-index_list']")[0].scrollTop = top;
            //document.getElementById("master-index_list").scrollTop = top;
            $("#" + _this.defaults.container).find("div[id='master-index_letters'] > div").removeClass("on");
            $(this).addClass("on");
        });
    });

    $("#" + _this.defaults.container).find("div[id='master-index_list']").find("a").each(function () {
        $(this).click(function (ev) {
            ev.stopPropagation();
            _this.Data.MasterId = $(this).attr("data-id");
            _this.Data.MasterName = $(this).attr("data-name");
            if (_this.defaults.level && _this.defaults.level > 1) {
                _this.showSerial();
            }
            else {
                _this.close();
            }
            if (_this.defaults.callback.mastercallback && typeof _this.defaults.callback.mastercallback == "function") {
                //_this.defaults.callback.mastercallback.apply(window, [_this.Data.MasterId, _this.Data.MasterName]);
                _this.defaults.callback.mastercallback([_this.Data.MasterId, _this.Data.MasterName]);
            }
        });
    });
}

CarSelectSim.prototype.showSerial = function () {
    var _this = this;
    if (!_this.iscontinue) {
        return;
    }
    _this.iscontinue = false;
    var url = _this.url + _this.format(_this.parmas, { "type": _this.defaults.salestatus, "pid": _this.Data.MasterId, "rt": "serial" });
    _this.getData(url, "serial");
}

CarSelectSim.prototype.FillSerial = function (data) {
    if (!data) return;
    var _this = this;
    var masterName = _this.Data.MasterName;
    var mastertitle = _this.defaults.titleformat.master;
    if (mastertitle) {
        if (mastertitle.deftxt && mastertitle.deftxt.length > 0) {
            masterName = mastertitle.deftxt;
        }
        if (mastertitle.length && mastertitle.length > 0 && masterName.length > mastertitle.length) {
            masterName = masterName.substr(0, mastertitle.length);
            if (mastertitle.moretxt) {
                masterName += mastertitle.moretxt;
            }
        }
    }
    _this.Data.MasterShortName = masterName;
    var tempHtml = new Array();
    tempHtml.push("<div class=\"zcfcbox clearfix\" style=\"position:absolute\" id=\"serial-div\" group=\"serial\">");
    tempHtml.push("<div class=\"fuctit\">");
    tempHtml.push("<ul class=\"ppname\">");
    tempHtml.push("<li><a href=\"javascript:;\" data-type=\"returnmaster\">品牌</a><i></i></li>");
    tempHtml.push("<li class=\"jomt\">&gt;</li>");
    tempHtml.push("<li class=\"current\"><a href=\"javascript:;\">" + masterName + "</a></li>");
    tempHtml.push("</ul>");
    tempHtml.push("</div>");

    tempHtml.push("<div class=\"h453\" group=\"serial\">");
    tempHtml.push("<div class=\"cxmian\">");
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            tempHtml.push("<div class=\"pinp-main-zm\">");
            tempHtml.push("<p><i>" + data[i]["gname"] + "</i></p>");
            var child = data[i]["child"];
            if (child && child.length > 0) {
                for (var j = 0; j < child.length; j++) {
                    if (child[j]["saleState"] == "停销") {
                        tempHtml.push("<p><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + child[j]["name"] + "\" data-spell=\""+child[j]["urlSpell"]+"\">" + child[j]["name"] + "<span>停售</span></a></p>");
                    }
                    else {
                        tempHtml.push("<p><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + child[j]["name"] + "\" data-spell=\"" + child[j]["urlSpell"] + "\">" + child[j]["name"] + "</a></p>");
                    }
                }
            }
            tempHtml.push("</div>");
        }
    }
    tempHtml.push("</div></div>");
    tempHtml.push("</div>");
    $("#" + _this.defaults.container + " > div[group='master']").hide();
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();

    _this.addSerialEvent();
}

CarSelectSim.prototype.addSerialEvent = function () {
    var _this = this;
    $("#" + _this.defaults.container).find("div.h453[group='serial']").find("a").each(function () {
        $(this).click(function (ev) {
            ev.stopPropagation();
            _this.Data.SerialId = $(this).attr("data-id");
            _this.Data.SerialName = $(this).attr("data-name");
            _this.Data.SerialSpell = $(this).attr("data-spell");
            if (_this.defaults.level && _this.defaults.level > 2) {
                _this.showCar();
            }
            else {
                _this.close();
            }
            if (_this.defaults.callback.serialcallback && typeof _this.defaults.callback.serialcallback == "function") {
                //_this.defaults.callback.serialcallback.apply(window, [_this.Data.SerialId, _this.Data.SerialName]);
                _this.defaults.callback.serialcallback([_this.Data.SerialId, _this.Data.SerialName, _this.Data.SerialSpell]);
            }
        });
    });
    $("#" + _this.defaults.container).find("div[group='serial']").find("a[data-type='returnmaster']").click(function (ev) {
        ev.stopPropagation();
        $("#" + _this.defaults.container).find("div[group='serial']").remove();
        $("#" + _this.defaults.container).find("div[group='master']").show();
    });
}

CarSelectSim.prototype.showCar = function () {
    var _this = this;
    if (!_this.iscontinue) {
        return;
    }
    _this.iscontinue = false;
    var url = _this.url + _this.format(_this.parmas, { "type": _this.defaults.salestatus, "pid": _this.Data.SerialId, "rt": "cartype" });
    _this.getData(url, "cartype");
}

CarSelectSim.prototype.FillCarType = function (data) {
    if (!data) return;
    var _this = this;
    var serialName = _this.Data.SerialName;
    var serialtitle = _this.defaults.titleformat.serial;
    if (serialtitle) {
        if (serialtitle.deftxt && serialtitle.deftxt.length > 0) {
            serialName = serialtitle.deftxt;
        }
        if (serialtitle.length && serialtitle.length > 0 && serialName.length > serialtitle.length) {
            serialName = serialName.substr(0, serialtitle.length);
            if (serialtitle.moretxt) {
                serialName += serialtitle.moretxt;
            }
        }
    }
    _this.Data.SerialShortName = serialName;
    var tempHtml = new Array();
    tempHtml.push("<div class=\"zcfcbox clearfix\" style=\"position:absolute\" id=\"cartype-div\" group=\"cartype\">");
    tempHtml.push("<div class=\"fuctit\">");
    tempHtml.push("<ul class=\"ppname\">");
    tempHtml.push("<li><a href=\"javascript:;\" data-type=\"returnmaster\">品牌</a><i></i></li>");
    tempHtml.push("<li class=\"jomt\">&gt;</li>");
    tempHtml.push("<li><a href=\"javascript:;\" title=\"" + _this.Data.MasterName + "\" data-type=\"returnserial\">" + _this.Data.MasterShortName + "</a> <i></i></li>");
    tempHtml.push("<li class=\"jomt\">&gt;</li>");
    tempHtml.push("<li class=\"current\" title=\"" + _this.Data.SerialName + "\">" + _this.Data.SerialShortName + "</li>");
    tempHtml.push("</ul>");
    tempHtml.push("</div>");
    tempHtml.push("<div class=\"h330\" group=\"cartype\"><div class=\"cxmian\"><div class=\"pinp-main-zm\">");

    var hasNoSaleFlag = false;
    for (var i = 0; i < data.length; i++) {
        if (data[i]["nosale"] == "true") {
            if (hasNoSaleFlag == false) {
                tempHtml.push("<p class=\"ts-tit\"><span>以下车款停售</span></p>");
                hasNoSaleFlag = true;
            }
            //tempHtml.push("<div class=\"pinp-main-zm\">");
        }
        //else {
        //    tempHtml.push("<div class=\"pinp-main-zm\">");
        //}

        tempHtml.push("<p><i>" + data[i]["yeartype"] + "</i></p>");
        var child = data[i]["child"];
        for (var j = 0; j < child.length; j++) {
            tempHtml.push("<p><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + (_this.Data.SerialName + " " + child[j]["yeartype"] + " " + child[j]["name"]) + "\" title=\"" + child[j]["name"] + "\">" + child[j]["name"] + "</a></p>");
        }
        //tempHtml.push("</div>");
    }
    tempHtml.push("</div></div></div></div>");
    $("#" + _this.defaults.container + " > div[group='serial']").hide();
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();
    _this.addCartypeEvent();
}

CarSelectSim.prototype.addCartypeEvent = function () {
    var _this = this;
    $("#" + _this.defaults.container).find("div.h330[group='cartype']").find("a").each(function () {
        $(this).click(function (ev) {
            ev.stopPropagation();
            _this.Data.CarId = $(this).attr("data-id");
            _this.Data.CarName = $(this).attr("data-name");
            if (_this.defaults.callback.cartypecallback && typeof _this.defaults.callback.cartypecallback == "function") {
                //_this.defaults.callback.cartypecallback.apply(window, [_this.Data.CarId, _this.Data.CarName]);
                _this.defaults.callback.cartypecallback([_this.Data.CarId, _this.Data.CarName]);
            }
            _this.close();
        });
    });
    $("#" + _this.defaults.container).find("div[group='cartype']").find("a[data-type='returnmaster']").click(function (ev) {
        ev.stopPropagation();
        $("#" + _this.defaults.container).find("div[group='serial']").remove();
        $("#" + _this.defaults.container).find("div[group='cartype']").remove();
        $("#" + _this.defaults.container).find("div[group='master']").show();
    });
    $("#" + _this.defaults.container).find("div[group='cartype']").find("a[data-type='returnserial']").click(function (ev) {
        ev.stopPropagation();
        $("#" + _this.defaults.container).find("div[group='cartype']").remove();
        $("#" + _this.defaults.container).find("div[group='serial']").show();
    });
}

CarSelectSim.prototype.MathOffsetTop = function (curNode, id) {
    var topHeight = 0;
    if (!curNode)
        return topHeight;
    while (curNode && curNode.getAttribute("id") != id) {
        topHeight += curNode.offsetTop;
        curNode = curNode.offsetParent;
    }
    return topHeight - $(".fuctit").height();
}

CarSelectSim.prototype.getData = function (rurl, type) {
    var _this = this;
    $.ajax({
        url: rurl,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "getCarType",
        success: function (data) {
            switch (type) {
                case "master": _this.FillMaster(data); break;
                case "serial": _this.FillSerial(data); break;
                case "cartype": _this.FillCarType(data); break;
                default: break;
            }
            _this.iscontinue = true;
        }
    });
}

CarSelectSim.prototype.close = function () {
    $("#matser-div,#serial-div,#cartype-div").remove();
}

CarSelectSim.prototype.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0], obj = arguments[1];
    for (var key in obj) {
        var re = new RegExp('\\{' + key + '\\}', 'gi');
        str = str.replace(re, obj[key]);
    }
    return str;
}