var WaitCompareObj = {
	ImagePath: "",                                     // 图片资源路径
	PageDivID: "divWaitCompareLayer",      // 页面浮动DIV ID
	PageDivObj: null,                                // 页面浮动DIV 对象
	IsNeedCookie: true,                             // 是否需要写cookie
	ArrPageHtml: new Array(),                  // 页面浮动DIV内容
	CarCsInfoURL: "http://api.car.bitauto.com/CarInfo/GetCarListInfoForCompare.ashx?carids=",
	CarCsInfoXmlHttp: null,
	CarInfoJson: null,
	IsIE: true,
	MiniWin: null,                                     // 浮动DIV内小窗口
	MiniListWin: null,                                // 浮动DIV内列表小窗口
	IDListULObj: null,
	IDList: "",                                            // 需要对比的ID列表(不写cookie使用)
	CarList: {},
	CompareUrl: "http://car.bitauto.com/chexingduibi/?carids=",
	AnimateStoped: false,
	IsCanAddCompare: true,
	MasterBrandId: null,
	MasterBrandName: null,
	SerialBrandId: null,
	SerialBrandName: null,
	IsCarTypeStopSale: null,
	Init: function () {
		var obj = this;
		obj.IsIE = obj.CheckBrowserForWaitCompare();
		obj.InsertWaitCompareDiv();
		obj.ShowNameForMiniListWaitCompare();

		obj.AddMoveEvent();

		var $rightfixed = $("#" + WaitCompareObj.PageDivID);

		$rightfixed.attr('data-width', $rightfixed.width()).attr('data-height', $rightfixed.height()).attr('data-margin-right', parseInt($rightfixed.css('margin-right')));
		$(function () {
			obj.ShowSideCompare();
		});
		obj.AddCarShowSimEvent();

		$("#b-close").click(function (event) {
			obj.HiddenCompare(event);
		});

		obj.AddIE6ScrollEvent();

		obj.AddBtnCompareEvent();
	},
	AddBtnCompareEvent: function () {
		var self = this;
		$("div[id^='carcompare_btn_new_']").each(function () {
			$(this).unbind("click").click(function (event) {
				var obj = $(event.target).parent();
				if (!$(obj).is("a[cid]")) {
					obj = event.target;
				}
				if ($(obj).is("a[cid]")) {
					var cid = $(obj).attr("cid");
					self.AddCarToCompare(cid);
				}
			});
		});
	},
	AddIE6ScrollEvent: function () {
		if (isie(6)) {
			var $rightfixed = $("#" + WaitCompareObj.PageDivID); // $('.right-fixed');
			$rightfixed.css({ 'position': 'absolute' });
			$(window).scroll(function (ev) {
				setTimeout(function () {
					var clientHeight = document.documentElement.clientHeight || document.body.clientHeight;
					var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
					var scrollh = document.documentElement.scrollHeight || document.body.scrollHeight;
					var clienth = document.documentElement.clientHeight || document.body.clientHeight;
					$rightfixed[0].style.top = (scrollT + clienth / 2 - $rightfixed.height() / 2 + 100) + 'px';
				}, 300);
			})
		}
	},
	AddCarShowSimEvent: function () {
		var self = this;
		$("#CarSelectSimpleContainerParent").find("input[userful='showcartypesim'],div[userful='showcartypesim']").click(function (event) {
			event.stopPropagation();
			self.GetYiCheSug()
		});
	},
	AddMoveEvent: function () {
		//拖动事件
		var $drag = $('[data-drag=true]');
		switch ($drag.attr('data-page')) {
			case 'compare':
				$drag.dragX({
					calldrag: function (l) {
						var $this = this;
						if (l < 0) l = 0;
						if (l > document.documentElement.clientWidth - $this.width())
							l = document.documentElement.clientWidth - $this.width();
						$this.css('left', l);
					}
				});
				break;
			case 'summary':
				$drag.dragX();
				break;
		}
	},
	InsertWaitCompareDiv: function () {
		var obj = this;
		if (!obj.PageDivObj) {
			if (document.getElementById(obj.PageDivID)) {
				obj.PageDivObj = document.getElementById(obj.PageDivID);
			}
			else {
				return;
			}
		}
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		var length = 0;
		if (compare) {
			length = compare.split("|").length;
		}
		if (length >= 10) {
			$("#CarSelectSimpleContainerParent").hide();
		}
	},
	AddCarToCompare: function (id) {
		var obj = WaitCompareObj;
		if (!WaitCompareObj.IsCanAddCompare) {
			return;
		}
		WaitCompareObj.IsCanAddCompare = false;
		if (obj.PageDivObj && obj.PageDivObj.style.display == "none") {
			var $rightfixed = $("#" + WaitCompareObj.PageDivID);
			$rightfixed.css({ width: parseInt($rightfixed.attr('data-width')), height: "auto", bottom: 0, 'margin-right': parseInt($rightfixed.attr('data-margin-right')) });
			obj.PageDivObj.style.display = "block";
		}
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		var com_arr = null;
		if (compare) {
			com_arr = compare.split("|");
			if (com_arr.length >= 10) {
				WaitCompareObj.IsCanAddCompare = true;
				obj.AlertCompareMsg("最多对比10个车款");
				return;
			}
			if (compare.indexOf("id" + id + ",") >= 0 || compare.indexOf("|" + id) >= 0 || compare.indexOf(id + "|") >= 0) {
				WaitCompareObj.IsCanAddCompare = true;
				obj.AlertCompareMsg("您选择的车型,已经在对比列表中!");
				return;
			}
		}
		else {
			com_arr = new Array();
		}
		com_arr.push(id);
		CookieForCompare.ClearCookie("ActiveNewCompare");
		CookieForCompare.SetCookie("ActiveNewCompare", com_arr.join("|"));

		if (com_arr.length >= 10) {
			$("#CarSelectSimpleContainerParent").hide();
		}

		//update count
		obj.UpdateSideCount();

		if (!isie([6, 7, 8])) {
			var $rightfixed = $("#" + WaitCompareObj.PageDivID); //var $rightfixed = $('.right-fixed')
			$fixedlist = $rightfixed.find('.fixed-list');
			$rightfixed.width(parseInt($rightfixed.attr('data-width')));
			$rightfixed.css('margin-right', parseInt($rightfixed.attr('data-margin-right')));
			$rightfixed.css({ 'bottom': 0, 'top': 'auto', 'height': 'auto' });
			$rightfixed.show();
			//动态效果
			var $tr = $('#car_filter_id_' + id);
			if ($tr && $($tr).length > 0) {//如果是页面加对比，有动态效果；如果是弹层，没有效果
				$fixedlist = $rightfixed.find('.fixed-list');
				$effect = $('.effect');
				//var $duibi = $tr.find('.' + CarCompareObj.divClass.split(' ')[0]);
				var $duibi = $tr.find("div[id^='carcompare_btn_new_']");
				var offset_start = $duibi.offset();
				$effect.css({ 'top': offset_start.top, 'left': offset_start.left, width: $duibi.width(), height: $duibi.height(), 'z-index': 200000000 });
				$effect.show();

				var offset_end = $rightfixed.offset();
				$effect.animate({ width: $duibi.width(), height: $duibi.height(), top: offset_end.top + $fixedlist.height() + $duibi.height(), left: offset_end.left + $fixedlist.width() / 2 }, 700, function () {
					$effect.hide();
					//添加数据
					obj.ShowNameForMiniListWaitCompare();
					//WaitCompareObj.IsCanAddCompare = true;
				});
			}
			else {
				//添加数据
				obj.ShowNameForMiniListWaitCompare();
				//WaitCompareObj.IsCanAddCompare = true;
			}
		} else {
			obj.ShowNameForMiniListWaitCompare();
			//WaitCompareObj.IsCanAddCompare = true;
		}
	},
	AddMultiCarToCompare: function (newcarinfo) {//添加多个车款
		var obj = WaitCompareObj;
		if (!WaitCompareObj.IsCanAddCompare) {
			return;
		}
		WaitCompareObj.IsCanAddCompare = false;
		if (obj.PageDivObj && obj.PageDivObj.style.display == "none") {
			var $rightfixed = $("#" + WaitCompareObj.PageDivID);
			$rightfixed.css({ width: parseInt($rightfixed.attr('data-width')), height: "auto", bottom: 0, 'margin-right': parseInt($rightfixed.attr('data-margin-right')) });
			obj.PageDivObj.style.display = "block";
		}
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		var com_arr = null;
		var newcar_arr = newcarinfo.split(',');
		if (compare) {
			com_arr = compare.split("|");
			if ((com_arr.length + newcar_arr.length) > 10) {
				WaitCompareObj.IsCanAddCompare = true;
				obj.AlertCompareMsg("最多对比10个车款");
				return;
			}
			var isContinue = true;
			for (var i = 0; i < newcar_arr.length; i++) {
				if (compare.indexOf(newcar_arr[i] + ",") == 0 || compare.indexOf("|" + newcar_arr[i]) >= 0) {
					WaitCompareObj.IsCanAddCompare = true;
					isContinue = false;
					obj.AlertCompareMsg("您选择的车型,已经在对比列表中!");
					return;
				}
			}
			if (!isContinue) {
				return;
			}
		}
		else {
			com_arr = new Array();
		}
		for (var i = 0; i < newcar_arr.length; i++) {
			var tempcar_arr = newcar_arr[i].split('|');
			com_arr.push(tempcar_arr[0]);
		}
		CookieForCompare.ClearCookie("ActiveNewCompare");
		CookieForCompare.SetCookie("ActiveNewCompare", com_arr.join("|"));

		if (com_arr.length >= 10) {
			$("#CarSelectSimpleContainerParent").hide();
		}

		//update count
		obj.UpdateSideCount();
		obj.ShowNameForMiniListWaitCompare();
	},
	DelAllWaitCompare: function () {
		var obj = this;
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		com_new_arr = new Array();
		if (compare) {
			var com_arr = compare.split("|");
			for (var i = 0; i < com_arr.length; i++) {
				var id = 0;
				if (com_arr[i].indexOf("id") >= 0) {
					var carinfo = com_arr[i].split(",")[0];
					id = carinfo.substr(2, carinfo.length);
				}
				else {
					id = com_arr[i];
				}
				if (document.getElementById("carcompare_btn_new_" + id)) {
					var btn = $("div[id='carcompare_btn_new_" + id + "']"); //综述页有2个id相同的
					if (btn.length > 0) {
						for (var j = 0; j < btn.length; j++) {
							//$(btn[j]).attr("class", CarCompareObj.divClass);
							$(btn[j]).removeClass("button_none");
							$(btn[j]).html("<a target=\"_self\" href=\"javascript:;\" cid=\"" + id + "\"><span>对比</span></a>");
						}
					}
				}
			}
		}
		CookieForCompare.SetCookie("ActiveNewCompare", "");
		document.getElementById('waitForClearBut').style.display = "none";
		obj.IDListULObj.innerHTML = "";
		$("#CarSelectSimpleContainerParent").show();
		//update count
		obj.UpdateSideCount();
		obj.AddBtnCompareEvent();
	},
	DelCompare: function (id) {
		var obj = this;
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		com_new_arr = new Array();
		if (compare) {
			var com_arr = compare.split("|");
			for (var i = 0; i < com_arr.length; i++) {
				if (com_arr[i].indexOf("id") == 0) {
					if (com_arr[i].indexOf("id" + id + ",") < 0) {
						com_new_arr.push(com_arr[i].split(",")[0].substr(2, com_arr[i].split(",")[0].length));
					}
				}
				else {
					if (com_arr[i] != id) {
						com_new_arr.push(com_arr[i]);
					}
				}
			}
		}
		// 新版综述页 by sk 2013.08.20
		if (document.getElementById("carcompare_btn_new_" + id)) {
			var btn = $("div[id='carcompare_btn_new_" + id + "']"); //综述页有2个id相同的
			if (btn.length > 0) {
				for (var j = 0; j < btn.length; j++) {
					//$(btn[j]).attr("class", CarCompareObj.divClass);
					$(btn[j]).removeClass("button_none");
					$(btn[j]).html("<a target=\"_self\" href=\"javascript:;\" cid=\"" + id + "\"><span>对比</span></a>");
				}
			}
		}
		CookieForCompare.ClearCookie("ActiveNewCompare");
		CookieForCompare.SetCookie("ActiveNewCompare", com_new_arr.join("|"));
		obj.ShowNameForMiniListWaitCompare();
		if (com_arr.length <= 10) {
			$("#CarSelectSimpleContainerParent").show();
		}
		//update count
		obj.UpdateSideCount();
	},
	UpdateSideCount: function () {
		var compareCar = CookieForCompare.GetCookie("ActiveNewCompare");
		if (compareCar) {
			var arr = compareCar.split("|");
			$("#side-compare em").show().html(arr.length);
		} else {
			$("#side-compare em").show().html(0);
		}
	},
	NowCompare: function () {
		var obj = this;
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		com_arr = new Array();
		if (compare) {
			var carids = "";
			com_arr = compare.split("|");
			if (com_arr.length < 1) {
				obj.AlertCompareMsg("至少选择1款车对比");
			}
			else {
				for (var i = 0; i < com_arr.length; i++) {
					if (carids != "") {
						if (com_arr[i].indexOf("id") >= 0) {
							carids += "," + com_arr[i].split(",")[0].substr(2, com_arr[i].split(",")[0].length);
						}
						else {
							carids += "," + com_arr[i];
						}
					}
					else {
						if (com_arr[i].indexOf("id") >= 0) {
							carids = com_arr[i].split(",")[0].substr(2, com_arr[i].split(",")[0].length);
						}
						else {
							carids += com_arr[i];
						}
					}
				}
				//新开页面在一个页面中打开改为全部新窗口打开 by sk 2015-12-30
				//window.open(obj.CompareUrl + carids, 'SameWindowCompare');
				window.open(obj.CompareUrl + carids, '');
			}
		}
		else {
			obj.AlertCompareMsg("至少选择1款车对比");
		}
	},
	ShowNameForMiniListWaitCompare: function () {// 显示对比浮动框
		var obj = this;
		var waitForClearBut = document.getElementById('waitForClearBut');
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		if (!obj.IDListULObj) {
			if (document.getElementById('idListULForWaitCompare')) {
				obj.IDListULObj = document.getElementById('idListULForWaitCompare');
			}
			else {
				return;
			}
		}
		if (compare) {
			var newcom_arr = new Array();//新的格式
			var com_arr = compare.split("|");
			var tempHTML = new Array();
			var tempCarId = new Array();
			for (var i = 0; i < com_arr.length; i++) {
				var id = 0;
				if (com_arr[i].indexOf("id") >= 0) {
					id = com_arr[i].split(",")[0].substr(2, com_arr[i].split(",")[0].length);
				}
				else {
					id = com_arr[i];
				}
				tempCarId.push(id);

				if (document.getElementById("carcompare_btn_new_" + id)) {
					var btn = $("div[id='carcompare_btn_new_" + id + "']");
					if (btn.length > 0) {
						for (var j = 0; j < btn.length; j++) {
							//$(btn[j]).attr("class", CarCompareObj.divClassAdd);
							$(btn[j]).addClass("button_none");
							$(btn[j]).html("<a href=\"javascript:;\"><span>已加入</span></a>");
						}
					}
				}
			}
			obj.InitCompareHtml(obj.CarCsInfoURL + tempCarId.join(","));

			if (com_arr.length < 1) {
				obj.IDListULObj.innerHTML = "";
				waitForClearBut.style.display = "none";
			}
			else {
				waitForClearBut.style.display = "";
			}
		}
		else {
			obj.IDListULObj.innerHTML = "";
			waitForClearBut.style.display = "none";
		}

	},
	InitCompareHtml: function (rurl) {
		var obj = this;
		$.ajax({
			url: rurl,
			cache: true,
			dataType: "jsonp",
			jsonpCallback: "getCarData",
			complete: function () {
				WaitCompareObj.IsCanAddCompare = true;
			},
			success: function (data) {
				if (!data) {
					return;
				}
				var tempHTML = new Array();
				for (var i = 0; i < data.length; i++) {
					var name = data[i]["CsName"] + "  " + data[i]["YearType"] + " " + data[i]["CarName"];
					tempHTML.push("<li><a class=\"title\" target=\"_blank\" href=\"http://car.bitauto.com/" + data[i]["CsAllSpell"] + "/m" + data[i]["CarID"] + "/\">" + name + "</a><a class=\"close\" href=\"javascript:;\"  id=\"" + data[i]["CarID"] + "\"></a></a><div class=\"clear\"></div></li>");
				}
				obj.IDListULObj.innerHTML = "";
				obj.IDListULObj.innerHTML += tempHTML.join("");
				obj.AddCartypeCloseEvent();
			}
		});
	},
	AddCartypeCloseEvent: function () {
		var self = this;
		$("#idListULForWaitCompare").find("a[class='close']").each(function () {
			$(this).click(function () {
				self.DelCompare($(this).attr("id"));
			});
		});
	},
	ShowSideCompare: function () {//
		var obj = this;
		var compareCar = CookieForCompare.GetCookie("ActiveNewCompare");
		if (compareCar) {
			var arr = compareCar.split("|");
			$("#yiche-side ul").prepend("<li id=\"side-compare\" data-channelid=\"2.21.835\" class=\"w4\"><em>" + arr.length + "</em><a href=\"javascript:;\" class=\"shoucang\" data=\"shoucang\" id=\"sideSkNews\" title=\"车型对比\">车型对比</a></li>");
			$("#side-compare").bind("click", function (event) {
				obj.ShowCompare(event);
			});
		} else {
			$("#yiche-side ul").prepend("<li id=\"side-compare\" data-channelid=\"2.21.835\" class=\"w4\"><em style=\"display:none;\"></em><a href=\"javascript:;\" class=\"shoucang\" data=\"shoucang\" id=\"sideSkNews\"  title=\"车型对比\">车型对比</a></li>")
			$("#side-compare").bind("click", function (event) {
				obj.ShowCompare(event);
			});
		}
		if (!document.getElementById("feedbackDiv")) {
			$("#side-compare").addClass('last');
		}
		obj.Initanimatebottom();
	},
	Initanimatebottom: function () {
		var obj = this;
		$("#" + obj.PageDivID).attr("animatebottom",
            $(window).height() - ($("#sideSkNews").offset() == undefined ? 0 : $("#sideSkNews").offset().top) -
            parseInt($("#sideSkNews").css("height")==undefined?0:($("#sideSkNews").css("height").substr(0, $("#sideSkNews").css("height").length - 2) / 2)) + $(document).scrollTop());
	},
	HiddenCompare: function (ev) {
		ev.preventDefault();
		var obj = this;
		if (obj.AnimateStoped) return;
		var $rightfixed = $("#" + WaitCompareObj.PageDivID); //var $rightfixed = $('.right-fixed');
		if (isie([6, 7, 8])) {
			$rightfixed.hide();
		} else {
			obj.AnimateStoped = true;
			//隐藏事件
			$rightfixed.attr('data-width', $rightfixed.width()).attr('data-height', $rightfixed.height());
			var fiexdOffset = $rightfixed.offset();
			$rightfixed.css('left', 'auto');
			$rightfixed.animate({ width: 0, height: 0, 'margin-right': $("#" + WaitCompareObj.PageDivID).attr("animateright"), 'bottom': $("#" + WaitCompareObj.PageDivID).attr("animatebottom") }, 600, function () {
				$rightfixed.hide();
				obj.AnimateStoped = false;
			});
		}
	},
	ShowCompare: function (ev) {

		var obj = this;
		//隐藏事件
		var $rightfixed = $("#" + WaitCompareObj.PageDivID); //var $rightfixed = $('.right-fixed');
		if (obj.AnimateStoped) return;
		ev.preventDefault();
		if ($rightfixed[0].style.display == 'none') {
			if (!isie([6, 7, 8])) {
				obj.AnimateStoped = true;
				if ($rightfixed.attr("width") == undefined) {
					$rightfixed.attr("style", "width: 0px; height: 0px; bottom: " + $("#" + WaitCompareObj.PageDivID).attr("animatebottom") + "px; margin-right: " + $("#" + WaitCompareObj.PageDivID).attr("animateright") + "px; left: auto; display: none; z-index: 100000000;");
				}

				$rightfixed.show();
				$rightfixed.animate({ width: parseInt($rightfixed.attr('data-width')), height: parseInt($rightfixed.attr('data-height')), bottom: 0, 'margin-right': parseInt($rightfixed.attr('data-margin-right')) }, 300, function () {
					obj.AnimateStoped = false;
					$rightfixed.css({ 'height': 'auto' });
				});
			}
			else {
				$rightfixed.show();
			}
		}
		else {
			obj.HiddenCompare(ev);
		}
	},
	CheckBrowserForWaitCompare: function () {
		if (window.ActiveXObject) {
			return true;
		}
		else {
			return false;
		}
	},
	CloseTheWindowForWaitCompareDiv: function () {
		document.getElementById("divWaitCompareLayer").style.display = "none";
		var carSelectSimpleContainer = document.getElementById("CarSelectSimpleContainer");
		carSelectSimpleContainer.innerHTML == "";
		carSelectSimpleContainer.style.display = "none";
	},
	StartCarCsInfoRequestForWaitCompare: function (carID) {// 取对比车型的子品牌数据
		WaitCompareObj.CarCsInfoXmlHttp = WaitCompareObj.CreateXMLHttpRequestForWaitCompare();
		if (WaitCompareObj.IsIE) {
			WaitCompareObj.CarCsInfoXmlHttp.onreadystatechange = WaitCompareObj.HandleStateChangeForWaitCompare;
		}
		WaitCompareObj.CarCsInfoXmlHttp.open("GET", WaitCompareObj.CarCsInfoURL + carID, false);
		WaitCompareObj.CarCsInfoXmlHttp.send(null);
		if (!WaitCompareObj.IsIE) {
			WaitCompareObj.ParserequestCarCsInfo(WaitCompareObj.CarCsInfoXmlHttp.responseText);
		}
	},
	AlertCompareMsg: function (msg) {
		var compareAlert = document.getElementById("AlertCenterDiv");
		compareAlert.innerHTML = "<p>" + msg + "</p>";
		compareAlert.style.display = "block";

		setTimeout(function () { document.getElementById("AlertCenterDiv").style.display = "none"; }, 1000);
	},
	GetYiCheSug: function () {
		var obj = this;
		var compareCar = CookieForCompare.GetCookie("ActiveNewCompare");
		if (compareCar) {
			var carArr = compareCar.split("|");
			if (carArr.length == 10) {
				obj.AlertCompareMsg("最多对比10个车款");
				return;
			}
		}
		if (CarSelectSim) {
			var container = "CarSelectSimpleContainer";

			CarSelectSim.Init(container, obj.AddCarToCompare, obj.PageDivID, obj.MasterBrandId, obj.MasterBrandName, obj.SerialBrandId, obj.SerialBrandName, obj.IsCarTypeStopSale);
		}
	}
}

var CarCompareObj = {
	divClass: "car-summary-btn-duibi button_gray",
	divClassAdd: "car-summary-btn-duibi button_none",
	btnClass: "",
	btnClassAdd: "",
	Init: function (divClass, divClassAdd) {
		var self = this;
		self.divClass = divClass || self.divClass;
		self.divClassAdd = divClassAdd || self.divClassAdd;
	}
}

//--------------------  Cookie  --------------------
var CookieForCompare = {
	SetCookie: function (name, value, expires, path, domain, secure) {
		expiryday = new Date();
		expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
		document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
			((path) ? "; path=" + path : "; path=/") +
			"; domain=car.bitauto.com" +
			((secure) ? "; secure" : "");
	},
	GetCookie: function (name) {
		var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
		if (arr != null) {
			return unescape(arr[2]);
		}
		return null;
	},
	ClearCookie: function (name, path, domain) {
		var obj = this;
		if (obj.GetCookie(name)) {
			document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
		}
	},
	ClearChildCookie: function (name, path, domain) {
		var obj = this;
		if (obj.GetCookie(name)) {
			document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
		}
	}
};

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
			}).mouseup(function () {
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