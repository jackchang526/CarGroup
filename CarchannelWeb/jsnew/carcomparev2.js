var CarCompare = {
    Url: "/car/ajaxnew/GetCarCsInfoForCompare.aspx?carid=",
    ContainerId: "divWaitCompareLayer",
    ArrPageHtml: [],
    CarList: {}
};
function insertWaitCompareDiv() {
    var s = CarCompare, container = $("#" + s.ContainerId);
    if (container.length <= 0) return;
    s.ArrPageHtml.push("<div id=\"miniListWaitCompare\" class=\"comparebox\" style=\"display: none;z-Index:99\">");
    s.ArrPageHtml.push("<div class=\"tit\" id=\"bar_minicompare\" style=\"cursor:move;\">");
    s.ArrPageHtml.push("<h6>车型对比</h6>");
    s.ArrPageHtml.push("<div class=\"opArea\"><a id=\"waitForClearBut\"  target=\"_self\" href=\"javascript:delAllWaitCompare();\">清空</a> | <a target=\"_self\" class=\"hide\" href=\"javascript:closeTheWindowForWaitCompareDiv();\">隐藏</a></div>");
    s.ArrPageHtml.push("</div>");
    s.ArrPageHtml.push("<div id=\"waitForStartBut\" class=\"button_gray button_95_25\"><a href=\"javascript:;\" onclick=\"nowCompare();\" >开始对比</a></div>");
    s.ArrPageHtml.push("<ul id=\"idListULForWaitCompare\"></ul>");
    s.ArrPageHtml.push("</div>");
    s.ArrPageHtml.push("<div class=\"dbbtn\" onclick=\"showTheWindowForWaitCompareDiv();\" id=\"miniWaitCompare\">车型对比<em></em></div>");
    s.ArrPageHtml.push("</div>");
    container.html(s.ArrPageHtml.join(""));
    loadCompare();
    //setTimeout(loadCompare, 100);

    $(function () { showSideCompare(); });
}

function loadCompare() {
    var carList = getCarCompare(), arrHtml = [];
    if (carList.length > 0) {
        $("#idListULForWaitCompare").html("");
        for (var i = 0; i < carList.length; i++) {
            var id = carList[i].id, name = carList[i].name;
            if (existCar(id)) {
                arrHtml.push("<li><a href=\"http://car.bitauto.com/" + CarCompare.CarList[id].spell + "/m" + id + "/\">" + CarCompare.CarList[id].n + " " + name + "</a><a class=\"btn-close\" href=\"javascript:;\" onclick=\"javascript:delCompare('" + id + "');\">删除</a></li>");
            } else {
                requestData(id);
            }

            changeButton(id);
        }
        $("#idListULForWaitCompare").html(arrHtml.join(""));
        if (carList.length < 1) {
            $("#idListULForWaitCompare").html('<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>');
            $("#waitForStartBut,#waitForClearBut").hide();
        }
        else {
            $("#waitForStartBut,#waitForClearBut").show();
        }
    } else {
        $("#idListULForWaitCompare").html('<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>');
        $("#waitForStartBut,#waitForClearBut").hide();
    }
}
function addCarToCompare(id, name, csName) {
    var compare = CookieForCompare.getCookie("ActiveNewCompare");
    var com_arr = null;
    if (compare) {
        com_arr = compare.split("|");
        if (com_arr.length >= 10) {
            $("#" + CarCompare.ContainerId).show();
            showTheWindowForWaitCompareDiv();
            alert("对比车型不能多于10个");
            return;
        }
        if (compare.indexOf("id" + id + ",") >= 0) {
            alert("您选择的车型,已经在对比列表中!");
            $("#" + CarCompare.ContainerId).show();
            return;
        }
    }
    else {
        com_arr = new Array();
    }
    com_arr.push('id' + id + ',' + name);
    CookieForCompare.clearCookie("ActiveNewCompare");
    CookieForCompare.setCookie("ActiveNewCompare", com_arr.join("|"));

    loadCompare();
    showTheWindowForWaitCompareDiv();
    $("#" + CarCompare.ContainerId).show();
    try {
        $("#miniListWaitCompare").drag({
            handler: $('#bar_minicompare'),
            opacity: 0.7
        });
    } catch (e) { }
}

// 清除1个对比车型
function delCompare(id) {
    var compare = CookieForCompare.getCookie("ActiveNewCompare");
    com_new_arr = new Array();
    if (compare) {
        var com_arr = compare.split("|");
        for (var i = 0; i < com_arr.length; i++) {
            if (com_arr[i].indexOf("id" + id + ",") < 0) {
                com_new_arr.push(com_arr[i]);
            }
        }
    }
    changeNoneButton(id);
    CookieForCompare.clearCookie("ActiveNewCompare");
    CookieForCompare.setCookie("ActiveNewCompare", com_new_arr.join("|"));
    loadCompare();
}
function changeButton(carid) {
    $("#carcompare_btn_new_" + carid).attr("class", "car-summary-btn-duibi button_none").html("<a href=\"javascript:;\"><span>对比</span></a>");
}
function changeNoneButton(carid, name) {
    $("#carcompare_btn_new_" + carid).attr("class", "car-summary-btn-duibi button_gray").html("<a target=\"_self\" href=\"javascript:addCarToCompare('" + carid + "','" + name + "');\"><span>对比</span></a>");
}
// 清除所有待对比车型
function delAllWaitCompare() {
    var carList = getCarCompare();
    for (var i = 0; i < carList.length; i++) {
        changeNoneButton(carList[i].id, carList[i].name);
    }
    CookieForCompare.setCookie("ActiveNewCompare", "");
    $("#idListULForWaitCompare").html('<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>');
    $("#waitForStartBut,#waitForClearBut").hide();
}

// 开始对比  
function nowCompare() {
    var carList = getCarCompare();
    if (carList.length > 0) {
        var arrCarId = [];
        for (var i = 0; i < carList.length; i++) {
            arrCarId.push(carList[i].id);
        }
        var compareUrl = "http://car.bitauto.com/chexingduibi/?carids=" + arrCarId.join();
        window.open(compareUrl, 'SameWindowCompare', '');
    } else {
        alert('至少选择1款车对比');
    }
}
// 展开浮动窗口
function showTheWindowForWaitCompareDiv() {
    $("#miniWaitCompare").hide();
    $("#miniListWaitCompare").show();
}
// 关闭浮动窗口
function closeTheWindowForWaitCompareDiv() {
    $("#miniWaitCompare").show();
    $("#miniListWaitCompare").hide();
}

function getCarCompare() {
    var compare = CookieForCompare.getCookie("ActiveNewCompare"), carList = [];
    if (compare) {
        var com_arr = compare.split("|");
        for (var i = 0; i < com_arr.length; i++) {
            var id = com_arr[i].split(",")[0].substring(2, com_arr[i].split(",")[0].length);
            var name = com_arr[i].split(",")[1];
            var obj = {};
            obj.id = id;
            obj.name = name;
            carList.push(obj);
            //carList[id] = name;
        }
    }
    return carList;
}
function requestData(carId) {
    $.ajax({
        url: CarCompare.Url + carId, cache: true, success: function (data) {
            if (data != "") {
                var arr = data.split("^");
                name = "";
                CarCompare.CarList[carId] = { n: arr[0], spell: arr[1] };
                $("#idListULForWaitCompare").append("<li><a href=\"http://car.bitauto.com/" + arr[1] + "/m" + carId + "/\">" + arr[0] + " " + name + "</a><a class=\"btn-close\" href=\"javascript:;\" onclick=\"javascript:delCompare('" + carId + "');\">删除</a></li>");
            }
        }
    });
}
function existCar(carId) {
    var result = false;
    for (var key in CarCompare.CarList) {
        if (key == carId) {
            result = true; break;
        }
    }
    return result;
}

function showSideCompare() {
    var compareCar = CookieForCompare.getCookie("ActiveNewCompare");
    if (compareCar) {
        var arr = compareCar.split("|");
        $("#yiche-side ul").prepend("<li id=\"\" class=\"w4\"><em>" + arr.length + "</em><a href=\"javascript:;\" title=\"车型对比\" target=\"_blank\">车型对比</a></li>").first().bind("click", function () { $("#divWaitCompareLayer").show(); showTheWindowForWaitCompareDiv(); });
    }
}
//--------------------  Cookie  --------------------
var CookieForCompare = {
    setCookie: function (name, value, expires, path, domain, secure) {
        expiryday = new Date();
        expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
        document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
			((path) ? "; path=" + path : "; path=/") +
			"; domain=car.bitauto.com" +
			((secure) ? "; secure" : "");
    },
    getCookie: function (name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    },
    clearCookie: function (name, path, domain) {
        if (CookieForCompare.getCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    },
    clearChildCookie: function (name, path, domain) {
        if (CookieForCompare.getCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    }
};
//====================drag add by sk 2014.05.09=========================
(function ($) {
    $.fn.drag = function (options) {
        var defaults = {
            handler: false,
            opacity: 0.5
        };
        var opts = $.extend(defaults, options);

        this.each(function () {
            var isMove = false,
 					handler = opts.handler ? $(this).find(opts.handler) : $(this),
					_this = $(this), //移动的对象
					dx, dy;

            $(document).mousemove(function (event) {
                if (isMove) {
                    var eX = event.pageX, eY = event.pageY;
                    _this.css({ 'left': eX - dx, 'top': eY - dy });
                }
            })
					.mouseup(function () {
					    if (isMove)
					        _this.fadeTo('fast', 1);
					    isMove = false;
					});

            handler.mousedown(function (event) {
                if ($(event.target).is(handler)) {
                    isMove = true;
                    $(this).css('cursor', 'move');
                    _this.fadeTo('fast', opts.opacity);
                    //取鼠标相对元素的位置
                    if (_this.css("left") == "auto")
                        dx = event.pageX - parseInt(_this.position().left);
                    else
                        dx = event.pageX - parseInt(_this.css("left"));
                    dy = event.pageY - parseInt(_this.css("top"));
                }
            });
        });
    };
})(jQuery);