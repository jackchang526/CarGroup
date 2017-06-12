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
selected:{"cookiename":"","splitarray":[],"idindex":0,"removestr":[],"showtxt":"已添加",level:3}, //已添加项
isallclicked ： true //全部车型是否可点击
isbrandclicked : true //品牌是否可点击
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
        selected: {},
        container: "",
        isallclicked : true,
        isbrandclicked : true
    },
    this.Data = {};
    this.CookieIdArray = new Array();
    $.extend(true, this.defaults, options);
    this.defaults.container = options.container;
    this.close();
    //if (!this.initShow()) {
    this.showMaster();
    //}
    this.closeEvent();
}

CarSelectSim.prototype.initShow = function () {
    var _this = this;
    var groupdivs = $("#" + _this.defaults.container).find("div[group]");
    if ($(groupdivs).length > 0) {
        var isshow = false;
        $(groupdivs).each(function () { if ($(this).css("display") == "block") { isshow = true; } });
        if (isshow) {
            $(groupdivs).each(function () { $(this).hide(); });
        }
        else {
            var cartypegroup = $("#" + _this.defaults.container).find("div[group='cartype']");
            var serialgroup = $("#" + _this.defaults.container).find("div[group='serial']");
            var mastergroup = $("#" + _this.defaults.container).find("div[group='master']");
            if (cartypegroup.length > 0) {
                $(cartypegroup).each(function () { $(this).show(); });
            }
            else if ($(serialgroup).length > 0) {
                $(serialgroup).each(function () { $(this).show(); });
            }
            else if ($(mastergroup).length > 0) {
                $(mastergroup).each(function () { $(this).show(); });
            }
        }
        return true;
    }
    return false;
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
    tempHtml.push("<div class=\"zcfcbox sele-2016\" id=\"matser-div\" style=\"position:absolute\" group=\"master\">");
    tempHtml.push("<div class=\"fuctit\"><ul class=\"ppname\"><li>品牌<i></i></li></ul></div>");
    tempHtml.push("<div class=\"brand_bg\">");

    if (charList && charList.length > 0) {
        tempHtml.push("<div class=\"brand_letters\" id=\"master-index_letters\">");
        for (var i = 0; i < charList.length ; i++) {
            tempHtml.push("<span data-name=\"" + charList[i] + "\"" + (i == 0 ? " class=\"current\"" : "") + "><a href=\"javascript:;\">" + charList[i] + "</a></span>");
        }
        tempHtml.push("</div>");
    }
    tempHtml.push("<div class=\"brand_name_bg\"><div class=\"brand_name\" id=\"master-index_list\">");

    if (dataList && dataList.length > 0) {
        var charSpell = dataList[0]["tSpell"];
        tempHtml.push("<dl id=\"master-indexletters_" + charSpell + "\">");
        for (var i = 0; i < dataList.length; i++) {
            if (charSpell != dataList[i]["tSpell"]) {
                charSpell = dataList[i]["tSpell"];
                tempHtml.push("</dl><dl id=\"master-indexletters_" + charSpell + "\">");
            }
            tempHtml.push("<dd><a href=\"javascript:;\" data-id=\"" + dataList[i]["id"] + "\" data-name=\"" + dataList[i]["name"] + "\" name=\"master\">" + dataList[i]["tSpell"] + " " + dataList[i]["name"] + "</a></dd>");
        }
        tempHtml.push("</dl>");
    }

    tempHtml.push("</div></div>");
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();
    _this.addMasterEvent();
}

CarSelectSim.prototype.addMasterEvent = function () {
    var _this = this;

    $("#" + _this.defaults.container).find("div[id='master-index_letters'] > span").each(function () { //字母点击事件
        $(this).click(function (ev) {
            ev.stopPropagation();
            var charSpell = $(this).attr("data-name");
            var elem = $("#" + _this.defaults.container).find("#master-indexletters_" + charSpell + "");
            var top = _this.MathOffsetTop(elem[0], "matser-div");
            $("#" + _this.defaults.container).find("#master-index_list")[0].scrollTop = top;
            $("#" + _this.defaults.container).find("#master-index_letters > span").removeClass("current");
            $(this).addClass("current");
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
                if (_this.defaults.callback.mastercallback && typeof _this.defaults.callback.mastercallback == "function") {
                    _this.defaults.callback.mastercallback([_this.Data.MasterId, _this.Data.MasterName]);
                }
                _this.close();
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
    tempHtml.push("<li class=\"current\"><a href=\"javascript:;\">" + _this.Data.MasterName + "</a></li>");//显示到子品牌不截字，显示到车款在截字
    tempHtml.push("</ul>");
    tempHtml.push("</div>");

    tempHtml.push("<div class=\"brand_bg  models_bg\">");
    if (_this.defaults.isallclicked) {
        tempHtml.push("<h6><a href=\"javascript:;\">全部车型</a></h6>");
    }
    else {
        tempHtml.push("<h6 class=\"not-point\">全部车型</h6>");
    }
    tempHtml.push("<div class=\"models_detail_bg\" style=\"height: 330px;\">");
    if (data.length > 0) {
        if (_this.defaults.selected.level == 2) {
            _this.getCookieIdArray(2);
            if (_this.defaults.selected.showtxt == undefined || _this.defaults.selected.showtxt == "") {
                _this.defaults.selected.showtxt == "已添加";
            }
        }
        tempHtml.push("<div class=\"models_detail\">");
        for (var i = 0; i < data.length; i++) {
            tempHtml.push("<dl>");
            if (_this.defaults.isbrandclicked) {
                tempHtml.push("<dt><a href=\"javascript:;\" data-id=\"" + data[i]["gid"] + "\" data-spell=\"" + data[i]["gspell"] + "\">" + data[i]["gname"] + "</a></dt>");
            }
            else {
                tempHtml.push("<dt class=\"not-point\">" + data[i]["gname"] + "</dt>");
            }
            var child = data[i]["child"];
            if (child && child.length > 0) {
                for (var j = 0; j < child.length; j++) {
                    if (_this.CookieIdArray != undefined && _this.CookieIdArray.length > 0 && _this.CookieIdArray.contains(child[j]["id"])) {
                        if (child[j]["saleState"] == "停销") {
                            tempHtml.push("<dd><span>" + child[j]["name"] + " 停售 (" + _this.defaults.selected.showtxt + ")</span></dd>");
                        }
                        else {
                            tempHtml.push("<dd><span>" + child[j]["name"] + " (" + _this.defaults.selected.showtxt + ")</span></dd>");
                        }
                    }
                    else {
                        if (child[j]["saleState"] == "停销") {
                            tempHtml.push("<dd><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + child[j]["name"] + "\" data-spell=\"" + child[j]["urlSpell"] + "\">" + child[j]["name"] + "<span>停售</span></a></dd>");
                        }
                        else {
                            tempHtml.push("<dd><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + child[j]["name"] + "\" data-spell=\"" + child[j]["urlSpell"] + "\">" + child[j]["name"] + "</a></dd>");
                        }
                    }
                }
            }
            tempHtml.push("</dl>");
        }
        tempHtml.push("</div>");
    }
    tempHtml.push("</div></div>");
    tempHtml.push("</div>");
    $("#" + _this.defaults.container + " > div[group='master']").hide();
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();
    if (_this.defaults.callback.mastercallback && typeof _this.defaults.callback.mastercallback == "function") {
        _this.defaults.callback.mastercallback([_this.Data.MasterId, _this.Data.MasterName]);
    }
    _this.addSerialEvent();
}

CarSelectSim.prototype.addSerialEvent = function () {
    var _this = this;
    $("#" + _this.defaults.container).find("div[group='serial']").find("dd > a").each(function () {
        $(this).click(function (ev) {
            ev.stopPropagation();
            _this.Data.SerialId = $(this).attr("data-id");
            _this.Data.SerialName = $(this).attr("data-name");
            _this.Data.SerialSpell = $(this).attr("data-spell");
            if (_this.defaults.level && _this.defaults.level > 2) {
                _this.showCar();
            }
            else {
                if (_this.defaults.callback.serialcallback && typeof _this.defaults.callback.serialcallback == "function") {
                    _this.defaults.callback.serialcallback([_this.Data.SerialId, _this.Data.SerialName, _this.Data.SerialSpell]);
                }
                _this.close();
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
    tempHtml.push("<div class=\"brand_bg  models_bg style_bg\"><div class=\"models_detail_bg\"  style=\"height: 330px;\"><div class=\"models_detail\">");

    var hasNoSaleFlag = false;
    if (_this.defaults.selected.level == 3) {
        _this.getCookieIdArray(3);
    }
    for (var i = 0; i < data.length; i++) {
        if (data[i]["nosale"] == "true") {
            if (hasNoSaleFlag == false) {
                tempHtml.push("<p class=\"ts-tit\">以下车款停售</p>");
                hasNoSaleFlag = true;
            }
            //tempHtml.push("<div class=\"pinp-main-zm\">");
        }
        //else {
        //    tempHtml.push("<div class=\"pinp-main-zm\">");
        //}
        tempHtml.push("<dl>");
        tempHtml.push("<dt>" + data[i]["yeartype"] + "</dt>");
        var child = data[i]["child"];
        for (var j = 0; j < child.length; j++) {
            if (_this.CookieIdArray != undefined && _this.CookieIdArray.length > 0 && _this.CookieIdArray.contains(child[j]["id"])) {
                tempHtml.push("<dd><span>" + child[j]["name"] + "(" + (_this.defaults.selected.showtxt != undefined && _this.defaults.selected.showtxt.length > 0 ? _this.defaults.selected.showtxt : "已添加") + ")" + "</span></dd>");
            }
            else {
                tempHtml.push("<dd><a href=\"javascript:;\" data-id=\"" + child[j]["id"] + "\" data-name=\"" + (_this.Data.SerialName + " " + child[j]["yeartype"] + " " + child[j]["name"]) + "\" title=\"" + child[j]["name"] + "\">" + child[j]["name"] + "</a></dd>");
            }
        }
        tempHtml.push("</dl>");
        //tempHtml.push("</div>");
    }
    tempHtml.push("</div></div></div></div>");
    $("#" + _this.defaults.container + " > div[group='serial']").hide();
    $("#" + _this.defaults.container).append(tempHtml.join("")).show();
    if (_this.defaults.callback.serialcallback && typeof _this.defaults.callback.serialcallback == "function") {
        _this.defaults.callback.serialcallback([_this.Data.SerialId, _this.Data.SerialName, _this.Data.SerialSpell]);
    }
    _this.addCartypeEvent();
}

CarSelectSim.prototype.addCartypeEvent = function () {
    var _this = this;
    $("#" + _this.defaults.container).find("div[group='cartype']").find("dd > a").each(function () {
        $(this).click(function (ev) {
            ev.stopPropagation();
            _this.Data.CarId = $(this).attr("data-id");
            _this.Data.CarName = $(this).attr("data-name");
            if (_this.defaults.callback.cartypecallback && typeof _this.defaults.callback.cartypecallback == "function") {
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

CarSelectSim.prototype.closeEvent = function () {
    var _this = this;
    //$("div[group='master'],div[group='serial'],div[group='cartype']").each(function () {
    //    if ($(this).closest($("#" + _this.defaults.container)).length <= 0) {
    //        $(this).hide();
    //    }
    //});

    //空白地点击 隐藏下拉框
    $(document).click(function (e) {
        //e.preventDefault();
        e = e || window.event;
        var target = e.srcElement || e.target;
        if ($(target).closest("span.default").length <= 0 && $(target).closest(".zcfcbox").length <= 0) {
            $(".brand-form .zcfcbox").remove();
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
CarSelectSim.prototype.getCookieIdArray = function (level) {
    var _this = this;
    //var cookieIdArray = new Array();
    if (_this.defaults.selected && _this.defaults.selected.level == level) {
        var cookieStr = GetCookie(_this.defaults.selected.cookiename);
        var splitArray = _this.defaults.selected.splitarray;
        _this.defaults.selected.idindex = _this.defaults.selected.idindex || 0;
        if (cookieStr != null && cookieStr != "" && splitArray != undefined && splitArray.length > 0) {
            var cookieArray = cookieStr.split(splitArray[0]);
            for (var i = 0; i < cookieArray.length; i++) {
                var cookieId;
                if (splitArray.length > 1) {
                    var cookieObj = cookieArray[i].split(splitArray[1]);
                    if (cookieObj.length <= _this.defaults.selected.idindex) continue;
                    cookieId = cookieObj[_this.defaults.selected.idindex];
                }
                else {
                    cookieId = cookieArray[i];
                }
                var removeStr = _this.defaults.selected.removestr;
                if (removeStr != undefined && removeStr.length > 0) {
                    for (var j = 0; j < removeStr.length ; j++) {
                        cookieId = cookieId.replace(new RegExp(removeStr[j], "g"), "");
                    }
                }
                //cookieIdArray.push(cookieId);
                _this.CookieIdArray.push(cookieId);
            }
        }
    }
    //return cookieIdArray;
}
function GetCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) {
        return unescape(arr[2]);
    }
    return null;
}
Array.prototype.contains = function (item) {
    var flag = false;
    //return RegExp("\\b" + item + "\\b").test(this);
    var arr = $(this);
    for (var i = 0; i < arr.length; i++) {
        if (item == arr[i]) {
            flag = true;
            break;
        }
    }
    return flag;
};