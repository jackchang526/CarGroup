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
	IDList: "",                                            // 需要对比的ID列表(不写cookie使用)
	CarList: {}
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
	WaitCompareObj.ArrPageHtml.push("<div id=\"miniListWaitCompare\" class=\"comparebox\" style=\"display: none;z-Index:99\">");
	WaitCompareObj.ArrPageHtml.push("<div class=\"tit\" id=\"bar_minicompare\" style=\"cursor:move;\">");
	WaitCompareObj.ArrPageHtml.push("<h6>车型对比</h6>");
	WaitCompareObj.ArrPageHtml.push("<div class=\"opArea\"><a id=\"waitForClearBut\"  target=\"_self\" href=\"javascript:delAllWaitCompare();\">清空</a> | <a target=\"_self\" class=\"hide\" href=\"javascript:closeTheWindowForWaitCompareDiv();\">隐藏</a></div>");
	WaitCompareObj.ArrPageHtml.push("</div>");
	WaitCompareObj.ArrPageHtml.push("<div id=\"waitForStartBut\" class=\"button_gray button_95_25\"><a href=\"javascript:;\" onclick=\"nowCompare();\" >开始对比</a></div>");
	WaitCompareObj.ArrPageHtml.push("<ul id=\"idListULForWaitCompare\"></ul>");
	WaitCompareObj.ArrPageHtml.push("</div>");
	WaitCompareObj.ArrPageHtml.push("<div class=\"dbbtn\" onclick=\"showTheWindowForWaitCompareDiv();\" id=\"miniWaitCompare\">车型对比<em></em></div>");
	WaitCompareObj.ArrPageHtml.push("</div>");
	WaitCompareObj.PageDivObj.innerHTML = WaitCompareObj.ArrPageHtml.join("");
	setTimeout(showNameForMiniListWaitCompare, 100);
	try {
		$("#miniListWaitCompare").drag({
			handler: $('#bar_minicompare'),
			opacity: 0.7
		});
	} catch (e) { }
	//显示pk
	$(function () { showSideCompare(); });
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
			if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
			{ WaitCompareObj.PageDivObj.style.display = "block"; }
			showTheWindowForWaitCompareDiv();
			alert("对比车型不能多于10个");
			return;
		}
		if (compare.indexOf("id" + id + ",") >= 0) {
			alert("您选择的车型,已经在对比列表中!");
			if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
			{ WaitCompareObj.PageDivObj.style.display = "block"; }
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

	//update count
	updateSideCount();

	if (WaitCompareObj.PageDivObj && WaitCompareObj.PageDivObj.style.display == "none")
	{ WaitCompareObj.PageDivObj.style.display = "block"; }
 }

// 清除1个对比车型
function delCompare(id, name) {
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
	// 新版综述页 by sk 2013.08.20
	if (document.getElementById("carcompare_btn_new_" + id)) {
		var btnCar = document.getElementById("carcompare_btn_new_" + id);
		btnCar.className = "car-summary-btn-duibi button_gray";
		btnCar.innerHTML = "<a target=\"_self\" href=\"javascript:addCarToCompare('" + id + "','" + name + "');\"><span>对比</span></a>";
	}
	CookieForCompare.clearCookie("ActiveNewCompare");
	CookieForCompare.setCookie("ActiveNewCompare", com_new_arr.join("|"));
	showNameForMiniListWaitCompare();
	//update count
	updateSideCount();
}
//初始化对比按钮
function initCompareButton() {
	var compare = CookieForCompare.getCookie("ActiveNewCompare");
	if (compare) {
		var com_arr = compare.split("|");
		for (var i = 0; i < com_arr.length; i++) {
			var id = com_arr[i].split(",")[0].substring(2, com_arr[i].split(",")[0].length);
			// 新版综述页 by sk 2013.08.20
			if (document.getElementById("carcompare_btn_new_" + id)) {
				var btnCar = document.getElementById("carcompare_btn_new_" + id);
				btnCar.className = "car-summary-btn-duibi button_none";
				btnCar.innerHTML = "<a href=\"javascript:;\"><span>对比</span></a>";
			}
		}
	}
}
function updateSideCount() {
	var compareCar = CookieForCompare.getCookie("ActiveNewCompare");
	if (compareCar) {
		var arr = compareCar.split("|");
		$("#side-compare em").show().html(arr.length);
	} else {
		$("#side-compare em").show().html(0);
	}
}
function onBtnDisplayClick() {
	var obj = $("#divWaitCompareLayer");
	if (obj.is(":hidden")) {
		obj.show(); showTheWindowForWaitCompareDiv();
	} else {
		obj.hide(); closeTheWindowForWaitCompareDiv();
	}
}
function showSideCompare() {
	var compareCar = CookieForCompare.getCookie("ActiveNewCompare");
	if (compareCar) {
		var arr = compareCar.split("|");
		$("#yiche-side ul").prepend("<li id=\"side-compare\" class=\"w4\"><em>" + arr.length + "</em><a href=\"javascript:;\" title=\"车型对比\">车型对比</a></li>").find("li:first").bind("click", function () {
			onBtnDisplayClick(); showTheWindowForWaitCompareDiv();
		});
	} else {
		$("#yiche-side ul").prepend("<li id=\"side-compare\" class=\"w4\"><em style=\"display:none;\"></em><a href=\"javascript:;\" title=\"车型对比\">车型对比</a></li>").find("li:first").bind("click", function () {
			onBtnDisplayClick();
		});
	}
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
				tempHTML.push("<li><a href=\"http://car.bitauto.com/" + WaitCompareObj.CsAllSpell + "/m" + id + "/\">" + WaitCompareObj.CsName + " " + name + "</a><a class=\"btn-close\" href=\"javascript:;\" onclick=\"javascript:delCompare('" + id + "','" + name.replace("'", "’") + "');\">删除</a></li>");
			}
			else {
				tempHTML.push("<li><a href=\"http://car.bitauto.com/" + WaitCompareObj.CsAllSpell + "/m" + id + "/\">" + name + "</a><a class=\"btn-close\" href=\"javascript:;\" onclick=\"javascript:delCompare('" + id + "','" + name.replace("'", "’") + "');\">删除</a></li>");
			}
			// 新版综述页 by sk 2013.08.20
			if (document.getElementById("carcompare_btn_new_" + id)) {
				var btnCar = document.getElementById("carcompare_btn_new_" + id);
				btnCar.className = "car-summary-btn-duibi button_none";
				btnCar.innerHTML = "<a href=\"javascript:;\"><span>对比</span></a>";
			}
		}
		WaitCompareObj.IDListULObj.innerHTML += tempHTML.join("");
		if (com_arr.length < 1) {
			WaitCompareObj.IDListULObj.innerHTML = '<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
			waitForStartBut.style.display = "none";
			waitForClearBut.style.display = "none";
		}
		else {
			waitForStartBut.style.display = "";
			waitForClearBut.style.display = "";
		}
	}
	else {
		WaitCompareObj.IDListULObj.innerHTML = '<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
		waitForStartBut.style.display = "none";
		waitForClearBut.style.display = "none";
	}
}

// 清除所有待对比车型
function delAllWaitCompare() {
	var compare = CookieForCompare.getCookie("ActiveNewCompare");
	com_new_arr = new Array();
	if (compare) {
		var com_arr = compare.split("|");
		for (var i = 0; i < com_arr.length; i++) {
			var idAndName = com_arr[i].split(",");
			if (idAndName.length = 2) {
				var id = idAndName[0].replace("id", "");
				var name = idAndName[1];
				// 新版综述页 by sk 2013.08.20
				if (document.getElementById("carcompare_btn_new_" + id)) {
					var btnCar = document.getElementById("carcompare_btn_new_" + id);
					btnCar.className = "car-summary-btn-duibi button_gray";
					btnCar.innerHTML = "<a target=\"_self\" href=\"javascript:addCarToCompare('" + id + "','" + name + "');\"><span>对比</span></a>";
				}
			}
		}
	}
	CookieForCompare.setCookie("ActiveNewCompare", "");
	WaitCompareObj.IDListULObj.innerHTML = '<li class=\"nocar\"><p>您未选择车型，请先</p><a href=\"http://car.bitauto.com/chexingduibi/\" target=\"_blank\">选择车型</a>';
	document.getElementById('waitForStartBut').style.display = "none";
	document.getElementById('waitForClearBut').style.display = "none";
	//update count
	updateSideCount();
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
			window.open(compareUrl, 'SameWindowCompare', '');
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
	$("#divWaitCompareLayer,#miniListWaitCompare").hide();
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
