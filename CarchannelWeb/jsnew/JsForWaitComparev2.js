// JScript 文件

var WaitCompareObj = {
    ImagePath: "",                                     // 图片资源路径
    PageDivID: "divWaitCompareLayer",      // 页面浮动DIV ID
    PageDivObj: null,                                // 页面浮动DIV 对象
    IsNeedCookie: true,                             // 是否需要写cookie
    ArrPageHtml: new Array(),                  // 页面浮动DIV内容
    CarCsInfoURL: "/car/ajaxnew/GetCarCsInfoForCompare.aspx?carid=",
    CarCsInfoXmlHttp: null,
    CsAllSpell: "",
    CsName: "",
    IsIE: true,
    MiniWin: null,                                     // 浮动DIV内小窗口
    MiniListWin: null,                                // 浮动DIV内列表小窗口
    IDListULObj: null,
    IDList: ""                                            // 需要对比的ID列表(不写cookie使用)
}

//添加浮动窗口
function insertWaitCompareDiv() {
    WaitCompareObj.IsIE = CheckBrowserForWaitCompare();
    if (!WaitCompareObj.PageDivObj) {
        if (document.getElementById(WaitCompareObj.PageDivID))
        { WaitCompareObj.PageDivObj = document.getElementById(WaitCompareObj.PageDivID); }
        else
        { return; }
    }
    WaitCompareObj.ArrPageHtml.push("<div id=\"miniListWaitCompare\" class=\"comparebox\" style=\"display: none;\">");
    WaitCompareObj.ArrPageHtml.push("<dl class=\"tit\">");
    WaitCompareObj.ArrPageHtml.push("<dt>车型对比</dt>");
    WaitCompareObj.ArrPageHtml.push("<dd><a target=\"_self\" href=\"javascript:closeTheWindowForWaitCompareDiv();\"><img src=\"/car/images/btn-hide.gif\" alt=\"\" /></a></dd>");
    WaitCompareObj.ArrPageHtml.push("</dl>");
    WaitCompareObj.ArrPageHtml.push("<ul id=\"idListULForWaitCompare\"></ul>");
    WaitCompareObj.ArrPageHtml.push("<div class=\"btn\">");
    WaitCompareObj.ArrPageHtml.push("<input id=\"waitForStartBut\" onclick=\"nowCompare();\" type=\"button\" value=\"开始对比\" class=\"inputbtn3\" /><input id=\"waitForClearBut\" type=\"button\" onclick=\"delAllWaitCompare();\" value=\"清空\" class=\"inputbtn4\" />");
    WaitCompareObj.ArrPageHtml.push("</div>");
    WaitCompareObj.ArrPageHtml.push("</div>");
    WaitCompareObj.ArrPageHtml.push("<div class=\"dbbtn\" onclick=\"showTheWindowForWaitCompareDiv();\" id=\"miniWaitCompare\">车型对比</div>");
    WaitCompareObj.PageDivObj.innerHTML = WaitCompareObj.ArrPageHtml.join("");
    showNameForMiniListWaitCompare();
}

// 取对比车型的子品牌数据
function startCarCsInfoRequestForWaitCompare(carID) {
    WaitCompareObj.CarCsInfoXmlHttp = createXMLHttpRequestForWaitCompare();
    if (WaitCompareObj.IsIE) {
        WaitCompareObj.CarCsInfoXmlHttp.onreadystatechange = handleStateChangeForWaitCompare;
    }
    WaitCompareObj.CarCsInfoXmlHttp.open("GET", WaitCompareObj.CarCsInfoURL + carID, false);
    WaitCompareObj.CarCsInfoXmlHttp.send(null);
    if (!WaitCompareObj.IsIE) {
        parserequestCarCsInfo(WaitCompareObj.CarCsInfoXmlHttp.responseText);
    }
}

function handleStateChangeForWaitCompare() {
    if (WaitCompareObj.CarCsInfoXmlHttp.readyState == 4 && WaitCompareObj.CarCsInfoXmlHttp.status == 200) {
        var requestText = WaitCompareObj.CarCsInfoXmlHttp.responseText;
        parserequestCarCsInfo(requestText);
    }
}

function parserequestCarCsInfo(csInfo) {
    if (csInfo && csInfo != "") {
        if (csInfo.indexOf("^") > 0) {
            WaitCompareObj.CsName = csInfo.split("^")[0];
            WaitCompareObj.CsAllSpell = csInfo.split("^")[1];
        }
        else {
            WaitCompareObj.CsName = "";
            WaitCompareObj.CsAllSpell = "";
        }
    }
    else {
        WaitCompareObj.CsName = "";
        WaitCompareObj.CsAllSpell = "";
    }
}

function createXMLHttpRequestForWaitCompare() {
    if (window.ActiveXObject) {
        var xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else if (window.XMLHttpRequest) {
        var xmlHttp = new XMLHttpRequest();
    }
    return xmlHttp;
}

function CheckBrowserForWaitCompare() {
    if (window.ActiveXObject) {
        return true;
    }
    else {
        return false;
    }
}

// 添加对比车型
function addCarToCompare(id, name, csName) {
    var compare = CookieForCompare.getCookie("ActiveNewCompare");
    var com_arr = null;
    if (compare) {
        com_arr = compare.split("|");
        if (com_arr.length >= 10) {
            alert("对比车型不能多于10个");
            if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
            { WaitCompareObj.PageDivObj.style.display = ""; }
            return;
        }
        if (compare.indexOf("id" + id + ",") >= 0) {
            alert("您选择的车型,已经在对比列表中!");
            if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
            { WaitCompareObj.PageDivObj.style.display = ""; }
            return;
        }
    }
    else {
        com_arr = new Array();
    }
    com_arr.push('id' + id + ',' + name);
    CookieForCompare.clearCookie("ActiveNewCompare");
    CookieForCompare.setCookie("ActiveNewCompare", com_arr.join("|"));

    showNameForMiniListWaitCompare();
    showTheWindowForWaitCompareDiv();

    if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
    { WaitCompareObj.PageDivObj.style.display = ""; }
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
    CookieForCompare.clearCookie("ActiveNewCompare");
    CookieForCompare.setCookie("ActiveNewCompare", com_new_arr.join("|"));
    showNameForMiniListWaitCompare();
}

// 显示对比浮动框
function showNameForMiniListWaitCompare() {
    var waitForStartBut = document.getElementById('waitForStartBut');
    var waitForClearBut = document.getElementById('waitForClearBut');
    var compare = CookieForCompare.getCookie("ActiveNewCompare");
    if (!WaitCompareObj.IDListULObj) {
        if (document.getElementById('idListULForWaitCompare'))
        { WaitCompareObj.IDListULObj = document.getElementById('idListULForWaitCompare'); }
        else
        { return; }
    }
    if (compare) {
        WaitCompareObj.IDListULObj.innerHTML = "";
        var com_arr = compare.split("|");
        WaitCompareObj.IDListULObj.innerHTML = '';
        var tempHTML = new Array();
        for (var i = 0; i < com_arr.length; i++) {
            var id = com_arr[i].split(",")[0].substring(2, com_arr[i].split(",")[0].length);
            var name = com_arr[i].split(",")[1];
            startCarCsInfoRequestForWaitCompare(id);
            if (WaitCompareObj.CsName != "" && WaitCompareObj.CsAllSpell != "") {
                tempHTML.push("<li><a target=\"_self\" href=\"javascript:delCompare('" + id + "');\" class=\"del\"></a><a href=\"http://car.bitauto.com/" + WaitCompareObj.CsAllSpell + "/m" + id + "/\">" + WaitCompareObj.CsName + " " + name + "</a></li>");
            }
            else {
                tempHTML.push("<li><a target=\"_self\" href=\"javascript:delCompare('" + id + "');\" class=\"del\"></a><a href=\"http://car.bitauto.com/" + WaitCompareObj.CsAllSpell + "/m" + id + "/\">" + name + "</a></li>");
            }
        }
        WaitCompareObj.IDListULObj.innerHTML += tempHTML.join("");
        if (com_arr.length < 1) {
            WaitCompareObj.IDListULObj.innerHTML = '<li>您未选择车型，请先<a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
            waitForStartBut.style.display = "none";
            waitForClearBut.style.display = "none";
        }
        else {
            waitForStartBut.style.display = "";
            waitForClearBut.style.display = "";
        }
    }
    else {
        WaitCompareObj.IDListULObj.innerHTML = '<li>您未选择车型，请先<a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
        waitForStartBut.style.display = "none";
        waitForClearBut.style.display = "none";
    }
}

// 清除所有待对比车型
function delAllWaitCompare() {
    CookieForCompare.setCookie("ActiveNewCompare", "");
    WaitCompareObj.IDListULObj.innerHTML = '<li>您未选择车型，请先<a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
    document.getElementById('waitForStartBut').style.display = "none";
    document.getElementById('waitForClearBut').style.display = "none";
}

// 开始对比  
function nowCompare() {
    var compare = CookieForCompare.getCookie("ActiveNewCompare");
    com_arr = new Array();
    if (compare) {
        var carids = "";
        com_arr = compare.split("|");
        if (com_arr.length < 1) {
            alert('至少选择1款车对比');
        }
        else {
            for (var i = 0; i < com_arr.length; i++) {
                if (carids != "")
                { carids += "," + com_arr[i].split(",")[0].substring(2, com_arr[i].split(",")[0].length); }
                else
                { carids = com_arr[i].split(",")[0].substring(2, com_arr[i].split(",")[0].length); }
            }
            //window.location.href = "http://car.bitauto.com/chexingduibi/?carids="+carids;
            var compareUrl = "http://car.bitauto.com/chexingduibi/?carids=" + carids;
            window.open(compareUrl, '', '');
        }
    }
    else {
        alert('至少选择1款车对比');
    }
}

// 展开浮动窗口
function showTheWindowForWaitCompareDiv() {
    if (!WaitCompareObj.MiniWin) {
        if (document.getElementById('miniWaitCompare'))
        { WaitCompareObj.MiniWin = document.getElementById('miniWaitCompare'); }
        else
        { return; }
    }
    if (!WaitCompareObj.MiniListWin) {
        if (document.getElementById('miniListWaitCompare'))
        { WaitCompareObj.MiniListWin = document.getElementById('miniListWaitCompare'); }
        else
        { return; }
    }
    WaitCompareObj.MiniWin.style.display = "none";
    WaitCompareObj.MiniListWin.style.display = "block";
}
// 关闭浮动窗口
function closeTheWindowForWaitCompareDiv() {
    if (!WaitCompareObj.MiniWin) {
        if (document.getElementById('miniWaitCompare'))
        { WaitCompareObj.MiniWin = document.getElementById('miniWaitCompare'); }
        else
        { return; }
    }
    if (!WaitCompareObj.MiniListWin) {
        if (document.getElementById('miniListWaitCompare'))
        { WaitCompareObj.MiniListWin = document.getElementById('miniListWaitCompare'); }
        else
        { return; }
    }
    WaitCompareObj.MiniWin.style.display = "block";
    WaitCompareObj.MiniListWin.style.display = "none";
}

//showName();

// 滚动
lastScrollY = 0;
function heartBeat() {
    var diffY;
    var oCompare = document.getElementById(WaitCompareObj.PageDivID);
    if (!oCompare)
        return;
    if (document.documentElement && document.documentElement.scrollTop)
        diffY = document.documentElement.scrollTop;
    else if (document.body)
        diffY = document.body.scrollTop
    else
    { /*Netscape stuff*/ }

    //alert(diffY);
    percent = .1 * (diffY - lastScrollY);
    if (percent > 0) percent = Math.ceil(percent);
    else percent = Math.floor(percent);
    document.getElementById(WaitCompareObj.PageDivID).style.top = parseInt(document.getElementById(WaitCompareObj.PageDivID).style.top) + percent + "px";
    // document.getElementById("lovexin14").style.top=parseInt(document.getElementById("lovexin12").style.top)+percent+"px";
    lastScrollY = lastScrollY + percent;
}
window.setInterval("heartBeat()", 10);


//--------------------  Cookie  --------------------
var CookieForCompare = {
    setCookie: function(name, value, expires, path, domain, secure) {
        expiryday = new Date();
        expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
        document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
			((path) ? "; path=" + path : "; path=/") +
			"; domain=car.bitauto.com" +
			((secure) ? "; secure" : "");
    },

    getCookie: function(name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    },

    clearCookie: function(name, path, domain) {
        if (CookieForCompare.getCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    },

    clearChildCookie: function(name, path, domain) {
        if (CookieForCompare.getCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    }
};
