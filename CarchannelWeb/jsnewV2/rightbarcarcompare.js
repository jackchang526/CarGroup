Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };

var BarCompare = function (options) {
    this.container = "",
    this.carcontent = "",
    this.CarCsInfoURL = "http://api.car.bitauto.com/CarInfo/GetCarListInfoForCompare.ashx?carids=",
    this.CompareUrl = "http://car.bitauto.com/chexingduibi/?carids=",
    this.IsCanAddCompare = true,
    this.win = window.parent || window,
    this.defaults = {
        salestatus: 7,
        level: 3,
        titleformat: { "master": { "length": 3, "moretxt": "..." }, "serial": { "length": 3, "moretxt": "..." } },
        selected: { "cookiename": "ActiveNewCompare", "level": 3, "splitarray": ['|'] },
        isallclicked: false,
        isbrandclicked: false
    }

    if (typeof options != 'undefined') {
        $.extend(true, this.defaults, options);
        if (typeof options.container != 'undefined') {
            this.container = options.container;
        }
        if (typeof options.carcontent != 'undefined') {
            this.carcontent = options.carcontent;
        }
    }
    this.InitEvent();
    this.InitData();
}

BarCompare.prototype = {
    InitEvent: function () {
        var _this = this;
        $("#" + _this.container + " >span").click(function (ev) {
            ev.stopPropagation();
            var selecter = new CarSelectSim({
                "container": _this.container
                , "level": _this.defaults.level
                , "salestatus": _this.defaults.salestatus
                , callback: {
                    "mastercallback": function () {
                        $("#" + _this.container + " >span").html(arguments[0][1]);
                    }, "serialcallback": function () {
                        $("#" + _this.container + ">span").html(arguments[0][1]);
                    }, "cartypecallback": function () {
                        $("#" + _this.container + " >span").html("请选择品牌");
                        var carId = arguments[0][0];
                        _this.AddCar(carId);
                    }
                }
                , "titleformat": _this.defaults.titleformat
                , "isbrandclicked": _this.defaults.isbrandclicked
                , "selected": _this.defaults.selected
            });
        });

        $("#btn-pk").click(function (ev) {//开始对比
            var com_arr = _this.GetCookieContent();
            if (com_arr.length > 0) {
                window.open(_this.CompareUrl + com_arr.join(","), '');
            }
            else {
                _this.ShowAlertLayer("至少选择1款车对比");
            }
        });

        $("#trashcompare").click(function () {//清空车款
            _this.CarTypeTrashEvent();
        });
    },
    InitData: function () {
        var _this = this,
        com_arr = _this.GetCookieContent();
        if (com_arr.length > 0) {
            _this.GetCarInfo(com_arr.join(","));
        }
    },
    AddCar: function (carId) {
        var _this = this,
            cookieName = _this.defaults.selected.cookiename;
        if (typeof carId == "undefined" || carId == null || !_this.IsCanAddCompare) {
            return;
        }

        var com_arr = _this.GetCookieContent();
        if (com_arr.length > 0) {
            if (com_arr.length >= 10) {
                _this.ShowAlertLayer("最多只能添加10款车");
                _this.IsCanAddCompare = true;
                return;
            }
            if (com_arr.contains(carId)) {
                _this.ShowAlertLayer("该车款已加入对比列表");
                _this.IsCanAddCompare = true;
                return;
            }
        }
        com_arr.push(carId);
        CookieHelper.ClearCookie(cookieName);
        CookieHelper.SetCookie(cookieName, com_arr.join("|"));
        //var win = window.parent || window;
        var carcomapre_btn = $(_this.win.document).find("[data-use='compare'][data-id='" + carId + "']");
        if ($(carcomapre_btn).length > 0) {
            $(carcomapre_btn).addClass("disabled").html("已加入");
        }
        _this.GetCarInfo(com_arr.join(","));
    },
    GetCarInfo: function (carids) {
        var _this = this,
            carcontent = _this.carcontent,
            rurl = _this.CarCsInfoURL + carids;
        if (carids == "") {
            $("#" + carcontent).html("");
            return;
        }
        _this.IsCanAddCompare = false;
        $.ajax({
            url: rurl,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "GetCarInfoCallback",
            complete: function () {
                _this.IsCanAddCompare = true;
            },
            success: function (data) {
                if (!data) {
                    return;
                }
                var tempHTML = new Array();
                for (var i = 0; i < data.length; i++) {
                    var name = data[i]["CsName"] + "  " + data[i]["YearType"] + " " + data[i]["CarName"];
                    tempHTML.push("<li><a class=\"title\" target=\"_blank\" href=\"http://car.bitauto.com/" + data[i]["CsAllSpell"] + "/m" + data[i]["CarID"] + "/\">" + name + "</a><a class=\"del\" href=\"javascript:;\"  id=\"" + data[i]["CarID"] + "\"></a></li>");
                }
                $("#" + carcontent).html(tempHTML.join(""));
                _this.CarTypeDelEvent();
            }
        });
    },
    CarTypeDelEvent: function () {
        var _this = this,
            cookieName = _this.defaults.selected.cookiename;
        $("#" + _this.carcontent + " a.del").each(function () {
            $(this).click(function () {
                var carId = $(this).attr("id");
                var com_arr = _this.GetCookieContent();
                com_arr.remove(carId);
                CookieHelper.ClearCookie(cookieName);
                CookieHelper.SetCookie(cookieName, com_arr.join("|"));
                var doc = _this.win.document;
                var carcomapre_btn = $(doc).find("[data-use='compare'][data-id='" + carId + "']");
                if ($(carcomapre_btn).length > 0) {
                    $(carcomapre_btn).removeClass("disabled").html("+对比");
                }
                _this.GetCarInfo(com_arr.join(","));
            });
        });
    },
    CarTypeTrashEvent: function () {
        var _this = this,
            com_arr = _this.GetCookieContent();
        if (com_arr.length > 0) {
            var doc = window.parent.document || window.document;
            for (var i = 0; i < com_arr.length; i++) {
                var carcomapre_btn = $(doc).find("[data-use='compare'][data-id='" + com_arr[i] + "']");
                if ($(carcomapre_btn).length > 0) {
                    $(carcomapre_btn).removeClass("disabled").html("+对比");
                }
            }
        }
        var cookieName = _this.defaults.selected.cookiename;
        CookieHelper.ClearCookie(cookieName);
        _this.GetCarInfo("");
    },
    GetCookieContent: function () {
        var _this = this,
            cookieName = _this.defaults.selected.cookiename;

        var compare = CookieHelper.GetCookie(cookieName);
        var com_arr = null;
        if (compare) {
            com_arr = compare.split(_this.defaults.selected.splitarray[0]);
            for (var i = 0; i < com_arr.length; i++) { //兼容老格式
                if (com_arr[i].indexOf("id") >= 0) {
                    var id = com_arr[i].split(",")[0].substr(2, com_arr[i].split(",")[0].length);
                    com_arr[i] = id;
                }
            }
        }
        else {
            com_arr = new Array();
        }
        return com_arr;
    },
    ShowAlertLayer: function (msg) {
        $("#alert-layer").html(msg).show();
        setTimeout(function () {
            $("#alert-layer").hide();
        }, 1000);
    }
}

//--------------------  Cookie  --------------------

var barCompare;

function SubAddCar(data) {
    var carId = data.carid;
    barCompare = GetBarCompare();
    barCompare.AddCar(carId);
}

////接受消息事件
//top.window.Bitauto.iMediator.subscribe('comparecar', 
//    SubAddCar
//);

function GetBarCompare() {
    if (barCompare == null) {
        barCompare = new BarCompare({ "container": "rightbar-brand-form", "carcontent": "rightbar-comparecar" });
    }
    return barCompare;
}

function GetCarInfoCallback() { }