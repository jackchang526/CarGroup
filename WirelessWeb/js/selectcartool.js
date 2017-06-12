/// <reference path="jquery-1.4.1.min.js" />
/// <reference path="iscroll.js" />
/// <reference path="model.js" />

var SelectCarTool = {
	Price: ""
	, Level: 0					//0不限，1-10对应各级别
	, LevelName: { 1: '微型车', 2: '小型车', 3: '紧凑型车', 4: '中大型车', 5: '中型车', 6: '豪华车', 7: 'MPV', 8: 'SUV', 9: '跑车', 11: '面包车', 12: '皮卡', 63: '轿车', 13: '小型SUV', 14: '紧凑型SUV', 15: '中型SUV', 16: '中大型SUV', 17: '大型SUV' }		//级别名称
	, Displacement: ""
	, TransmissionType: 0		//0不限，1手动，2自动
	, TransmissionTypeName: { 1: "手动", 62: "自动", 32: "半自动（AMT）", 2: "自动（AT）", 4: "手自一体", 8: "无级变速", 16: "双离合" }
    , DriveType: 0	//驱动方式
	, DriveTypeName: { 1: "前驱", 2: "后驱", 252: "四驱", 4: "全时四驱", 8: "分时四驱", 16: "适时四驱", 32: "智能四驱", 64: "四轮驱动", 128: "前置四驱" }
	, FuelType: 0	//燃料类型
	, FuelTypeName: { 7: "汽油", 8: "柴油", 2: "油电混合", 16: "纯电动", 4: "油气混合" }
	, BodyDoors: ""	//车门数
	, PerfSeatNum: ""	//座位数
	, IsWagon: 0	//是否旅行版
	, MoreCondition: new Array()
	, MoreConditionName: new Array()
	, View: 0					//0默认大图显示，1列表显示
	, Sort: 0					//0默认显示关注高-低，1关注低-高，2按价格排列低-高，3价格高-低
	, Brand: 0					//0不限，1自主，2合资，3进口
	, BrandName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口", 8: "德系", 10: "美系", 9: "日韩", 11: "欧系", 12: "日系", 16: "韩系" }
	, BodyForm: 0				//0不限，1两厢及掀背，2三厢
	, BodyFormName: { 1: "两厢", 2: "三厢" }
	, Type: "car"
	, Domain: window.location.host
	, currentId: ""
	, MoreMaxNum: 28
	, MoreSelectNum: 0
    , MoreFilterNum: 0
	//初始化页面显示
	, initPageCondition: function () {
		this.initShowDefault();
		this.initPrice();
		this.initLevel();
		this.initDisplacement();
		this.initTransmisstionType();
		this.initBodyForm();
		this.initMoreCondition();
		this.initBrandType();
		this.initMoreFilter();
		this.bindFilterClickEvent();
		this.initRightSwipe();
	}
    , initRightSwipe: function () {
    	var isResultScroll = false, clientHeight = 0;
    	var $car = $('[data-action]');
    	$car.rightSwipe({
    		oneEnd: function () {
    			var $leftPopup = this;
    			var $swipeLeft = $leftPopup.find('.swipeLeft');
    			var $back = $('.' + $leftPopup.attr('data-back'))
    			$back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
    			$swipeLeft.touches({ touchstart: function (ev) { ev.stopPropagation(); }, touchmove: function (ev) { ev.preventDefault(); } });
    		},
    		clickEnd: function (b, elem) {
    			var $leftPopup = this;
    			if (b) {
    				var $swipeLeft = $leftPopup.find('.swipeLeft'), $currentElement = $swipeLeft;
    				if (elem.data("action") == "condition" || elem.data("action") == "more") {
    					$car.$y2015 = $swipeLeft.find('.y2015-car-02');
    					var $cbox = $car.$y2015.children(0);
    					$cbox.height(document.documentElement.clientHeight - 50);
    					$currentElement = $cbox;
    				}

    				if (!isResultScroll) {
    					//isResultScroll = true;
    					var myScroll = new iScroll($currentElement[0], {
    						snap: 'li',
    						momentum: true,
    						bounce: false,
    						click: true,
    						onBeforeScrollStart: function (ev) {

    						},
    						onScrollMove: function (ev) {
    						    //$currentElement.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
    						}
    					});
    					$currentElement.find('.swipeLeft-header').show();
    				}
    			}
    		}
    	});
    	$(window).resize(function () {
    		if ($car.$y2015) {
    			$car.$y2015.children(0).height(document.documentElement.clientHeight - 50);
    		}
    	}).trigger('resize');

    }
	//默认展开条件
	, initShowDefault: function () {
		var hash = window.location.hash;
		switch (hash.toLowerCase()) {
			case "#sd":
				this.currentId = "btnPrice";
				$(".leftPopup").css("zIndex", 199);
				var button = SelectCarTool.returnPopupItemId(this.currentId);
				SelectCarTool.toggle($("#" + button));
				window.setTimeout(function () { $("#p_min").get(0).focus(); }, 500);
				break;
		}
	}
	//绑定条件点击事件
	, bindFilterClickEvent: function () {
		var btnFilter = document.getElementById("m-btn-filter");
		if (btnFilter) {
			var liList = btnFilter.getElementsByTagName("li");
			for (var i = 0; i < liList.length; i++) {
				//if (liList[i].className === "m-btn" || liList[i].className === "m-btn current" || liList[i].className === "sub m-btn")
				//    liList[i].onclick = this.filterClickEvent;
			}
		}

		var btnPriceSubmit = document.getElementById("btnPriceSubmit");
		if (btnPriceSubmit) {
			addEvent(btnPriceSubmit, "click", function () {
				var minP = document.getElementById("p_min").value;
				var maxP = document.getElementById("p_max").value;
				if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0)))
					document.getElementById("p_alert").innerHTML = "价格不能为空。";
				else if (maxP != "" && parseInt(maxP) <= 0) { document.getElementById("p_alert").innerHTML = "请填写正确的价格区间。"; }
				else {
					if (minP != "" && parseInt(minP) > 0 && (maxP == "" || parseInt(maxP) <= 0)) {
						GotoPage("p" + minP + "-9999"); return false;
					}
					else if (maxP != "" && parseInt(maxP) > 0 && (minP == "" || parseInt(minP) <= 0)) {
						GotoPage("p0-" + maxP + ""); return false;
					}
					else if (minP != "" && parseInt(minP) > 0 && maxP != "" && parseInt(minP) > 0) {
						if (parseInt(maxP) > parseInt(minP)) {
							GotoPage("p" + minP + "-" + maxP + ""); return false;
						}
						else
							document.getElementById("p_alert").innerHTML = "请填写正确的价格区间。";
					}
				}
			}, true);
		}

		var checkList = $("#m-filter-condition-leftPopup .first-list li");
		for (i = 0; i < checkList.length; i++) {
			checkList[i].onclick = function (e) {
				e.preventDefault();
				var elem = this;
				if (!elem) return;
				var info = elem.childNodes[1].childNodes[1].childNodes[1];
				if (info.className == "radio-normal") {
					info.className = "radio-normal checked";
				} else {
					info.className = "radio-normal"
				}
				var count = 0;
				for (j = 0; j < 28; j++) {
					var mcCheckEle = $$("mcCheck" + j);
					if (mcCheckEle && mcCheckEle.className == "radio-normal checked") {
						count++;
					}
				}
				if (count > 0) {
					$$("m-btn-condition").innerHTML = "完成(" + count + ")";
					$("#m-btn-condition-clear").addClass("btn-clear-selected");
				}
				else {
					$$("m-btn-condition").innerHTML = "完成";
					$("#m-btn-condition-clear").removeClass("btn-clear-selected");
				}
			};
		}
		$(".leftmask").click(function () {
			$(this).hide();
			$(".swipeLeft").removeClass("swipeLeft-block");
			$(".leftPopup").css("zIndex", 0);
			$(".leftPopup").hide();

		});

		var self = this;
		$$("m-btn-condition").onclick = function () {
			//配置参数
			for (i = 0; i < 28; i++) {
				var mcCheckEle = $$("mcCheck" + i);
				if (mcCheckEle && mcCheckEle.className == "radio-normal checked")
					self.MoreCondition[i] = '1';
				else
					self.MoreCondition[i] = '0';
			}
			self.GotoPage();
		}
		$$("m-btn-condition-clear").onclick = function () {
			//清除配置参数
			for (i = 0; i < 28; i++) {
				var mcCheckEle = $$("mcCheck" + i);
				if (mcCheckEle && mcCheckEle.className == "radio-normal checked")
					mcCheckEle.className = "radio-normal";
			}
			$$("m-btn-condition").innerHTML = "完成"
			$("#m-btn-condition-clear").removeClass("btn-clear-selected");
		}
		$$("m-btn-more").onclick = function () {
			if (self.MoreFilterNum > 0) {
				$$("btnMore").childNodes[1].childNodes[1].innerHTML = "更多(" + self.MoreFilterNum + ")";
			}
			self.GotoPage();
		}
		$$("m-btn-more-clear").onclick = function () {
			SelectCarTool.DriveType = 0;
			SelectCarTool.FuelType = 0;
			SelectCarTool.BodyDoors = "";
			SelectCarTool.PerfSeatNum = "";
			$$("btnDrive").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnFuel").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnDoor").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "";
			$(".morestyle").removeClass("current");
		    //$("#drivetype" + SelectCarTool.DriveType.toString()).addClass("current");
			$(".drivetype" + SelectCarTool.DriveType.toString()).addClass("current");
			$("#fueltype" + SelectCarTool.FuelType.toString()).addClass("current");
			var doorNum = 0;
			if (SelectCarTool.BodyDoors != "") {
				doorNum = SelectCarTool.BodyDoors.charAt(0);
			}
			var seatNum = 0;
			if (SelectCarTool.PerfSeatNum != "") {
				seatNum = SelectCarTool.PerfSeatNum.charAt(0);
			}
			$("#doors" + doorNum.toString()).addClass("current");
			$("#seatnum" + seatNum.toString()).addClass("current");
			$$("m-btn-more").innerHTML = "完成";
			$("#m-btn-more-clear").removeClass("btn-clear-selected");
		}
	}

	//点击事件处理
	, filterClickEvent: function () {
		var id = this.id, prevClickElement;
		$(".leftPopup").css("zIndex", 199);

		var button = SelectCarTool.returnPopupItemId(id);
		SelectCarTool.toggle($("#" + button));
		SelectCarTool.currentId = id;
		window.scrollTo(0, 0);
	}
	//返回条件弹出层id
	, returnPopupItemId: function (cid) {
		var result = "";
		switch (cid) {
			case "btnPrice": result = "m-filter-price"; break;
			case "btnLevel": result = "m-filter-level"; break;
			case "btnLevelsuv": result = "m-filter-levelsuv"; break;
			case "btnBrandType": result = "m-filter-brandtype"; break;
			case "btnTransmisstionType": result = "m-filter-trans"; break;
			case "btnDisplacement": result = "m-filter-dis"; break;
			case "btnMore": result = "m-filter-more"; break;
			case "btnCondition": result = "m-filter-condition"; break;
			case "btnBodyform": result = "m-filter-body"; break;

			case "btnDrive": result = "m-filter-drive"; break;
			case "btnFuel": result = "m-filter-fuel"; break;
			case "btnDoor": result = "m-filter-door"; break;
			case "btnSeat": result = "m-filter-seat"; break;
		}
		return result;
	}
	//元素点击显示
	, toggle: function (elem) {
		var elemTemp = elem.get(0);
		var pop = $("#" + elemTemp.id + "-leftPopup");
		var mask = $("#m-filter-mask");
		if (pop && (elemTemp.style.display == "block" || elemTemp.className == "swipeLeft swipeLeft-block")) {
			pop.hide();
			elem.removeClass("swipeLeft-block");
			mask.hide();
		} else {
			pop.show();
			elem.addClass("swipeLeft-block");
			mask.show();
		}
	}
    , GotoPage: function () {
    	var queryString = this.GetSearchQueryString();
    	var toUrl = _SearchUrl;
    	if (queryString.length > 0)
    		toUrl += "?" + queryString;
    	location.href = toUrl;
    }
	//设置选中项不可用
	, setDisabled: function (elem) {
		if (!elem) return;
		//var info = elem.firstChild.innerHTML;
		//elem.innerHTML = "<a>" + info + "</a>";
	    //elem.className = "current";
		$(elem).addClass("current");
	}
	//初始化价格
	, initPrice: function () {
		var showName = "";
		switch (this.Price) {
			case "0-5":
				this.setDisabled($$("price1"));
				showName = "5万以下";
				break;
			case "5-8":
				this.setDisabled($$("price2"));
				showName = "5-8万";
				break;
			case "8-12":
				this.setDisabled($$("price3"));
				showName = "8-12万";
				break;
			case "12-18":
				this.setDisabled($$("price4"));
				showName = "12-18万";
				break;
			case "18-25":
				this.setDisabled($$("price5"));
				showName = "18-25万";
				break;
			case "25-40":
				this.setDisabled($$("price6"));
				showName = "25-40万";
				break;
			case "40-80":
				this.setDisabled($$("price7"));
				showName = "40-80万";
				break;
			case "80-9999":
				this.setDisabled($$("price8"));
				showName = "80万以上";
				break;
			default:
				if (this.Price != "") {
					showName = "自定义" + this.Price + "万";
					var arrayPrice = this.Price.split("-");
					if (arrayPrice.length == 2) {
						document.getElementById("p_min").value = arrayPrice[0];
						document.getElementById("p_max").value = arrayPrice[1];
					}
				}
				else
					this.setDisabled($$("price0"));
				break;
		}
		if (this.Price != "") {
			$$("btnPrice").childNodes[1].childNodes[1].innerHTML = showName;
			$$("btnPrice").className = "m-btn current";
		}
	}
	//初始化级别
	, initLevel: function () {
	    if (this.Level == "") 
	        this.Level = "0";
	    this.setDisabled($(".level" + this.Level));
		if (this.Level != "0") {
			$$("btnLevel").childNodes[1].childNodes[1].innerHTML = this.LevelName[this.Level];
			$$("btnLevel").className = "m-btn current";
			if (this.Level == "8" || this.Level == "13" || this.Level == "14" || this.Level == "15" || this.Level == "16" || this.Level == "17") {
				//$$("btnLevelsuv").childNodes[0].childNodes[1].innerHTML = this.LevelName[this.Level];
				if (this.Level == "8") {
					//$$("btnLevelsuv").childNodes[0].childNodes[1].innerHTML = "全部";
				}
			}
		}
	}
	//初始化车身
	, initBodyForm: function () {
		var str = "bodyform", v = this.BodyForm;
		if (this.IsWagon > 0) {
			str = "wagon";
			v = this.IsWagon;
		}
		var bodyFormEle = document.getElementById(str + v);
		if (bodyFormEle) {
			this.setDisabled(bodyFormEle);
		}
		if (this.IsWagon > "0") {
			$$("btnBodyform").childNodes[1].childNodes[1].innerHTML = "旅行版";
			$$("btnBodyform").className = "m-btn current";
		}
		else if (this.BodyForm != "0") {
			$$("btnBodyform").childNodes[1].childNodes[1].innerHTML = this.BodyFormName[this.BodyForm];
			$$("btnBodyform").className = "m-btn current";
		}
	}
	//初始化国别
	, initBrandType: function () {
		this.setDisabled($$("brandType" + this.Brand.toString()));
		if (this.Brand != "") {
			$$("btnBrandType").childNodes[1].childNodes[1].innerHTML = this.BrandName[this.Brand];
			$$("btnBrandType").className = "m-btn current";
		}
	}
	//初始化变速箱
	, initTransmisstionType: function () {
		this.setDisabled($$("trans" + this.TransmissionType.toString()));
		if (this.TransmissionType != "0") {
			$$("btnTransmisstionType").childNodes[1].childNodes[1].innerHTML = this.TransmissionTypeName[this.TransmissionType];
			$$("btnTransmisstionType").className = "m-btn current";
		}
	}
	//初始化排量
	, initDisplacement: function () {
		var showName = "";
		switch (this.Displacement) {
			case "0-1.3":
				this.setDisabled($$("dis1"));
				showName = "1.3L以下";
				break;
			case "1.3-1.6":
				this.setDisabled($$("dis2"));
				showName = "1.3-1.6L";
				break;
			case "1.7-2.0":
				this.setDisabled($$("dis3"));
				showName = "1.7-2.0L";
				break;
			case "2.1-3.0":
				this.setDisabled($$("dis4"));
				showName = "2.1-3.0L";
				break;
			case "3.1-5.0":
				this.setDisabled($$("dis5"));
				showName = "3.1-5.0L";
				break;
			case "5.0-9":
				this.setDisabled($$("dis6"));
				showName = "5.0L以上";
				break;
			default:
				this.setDisabled($$("dis0"));
				break;
		}
		if (this.Displacement != "") {
			$$("btnDisplacement").childNodes[1].childNodes[1].innerHTML = showName;
			$$("btnDisplacement").className = "m-btn current";
		}
	}
	//初始化配置选项
	, initMoreCondition: function () {
		for (i = 0; i < 28; i++) {
			if (this.MoreCondition[i] == '1') {
				this.MoreSelectNum++;
				var mcCheckEle = $$("mcCheck" + i.toString());
				if (mcCheckEle)
					mcCheckEle.className = "radio-normal checked";
			}
		}
		if (this.MoreSelectNum > 0) {
			$$("btnCondition").childNodes[1].childNodes[1].innerHTML = "配置(" + this.MoreSelectNum + ")";
			$$("m-btn-condition").innerHTML = "完成(" + this.MoreSelectNum + ")";
			$$("btnCondition").className = "m-btn current";
		}
		else {
			$("#m-btn-condition-clear").removeClass("btn-clear-selected");
		}
	}
    , initMoreFilter: function () {
    	this.initMore();
    	if (this.MoreFilterNum > 0) {
    		$$("btnMore").childNodes[1].childNodes[1].innerHTML = "更多(" + this.MoreFilterNum + ")";
    		$$("btnMore").className = "m-btn current";
    	}
    }
    , initMore: function () {
    	var total = 0;
    	if (this.DriveType && this.DriveType > 0) {
    		total++;
    		$$("btnDrive").childNodes[0].childNodes[1].innerHTML = this.DriveTypeName[this.DriveType];
    	}
    	else {
    		$$("btnDrive").childNodes[0].childNodes[1].innerHTML = "";
    	}

    	if (this.FuelType && this.FuelType > 0) {
    		total++;
    		$$("btnFuel").childNodes[0].childNodes[1].innerHTML = this.FuelTypeName[this.FuelType];
    	}
    	else {
    		$$("btnFuel").childNodes[0].childNodes[1].innerHTML = "";
    	}
    	if (parseInt(this.BodyDoors) > 0 && this.BodyDoors.length > 0) {
    		total++;
    		$$("btnDoor").childNodes[0].childNodes[1].innerHTML = this.BodyDoors + "门";;
    	}
    	else {
    		$$("btnDoor").childNodes[0].childNodes[1].innerHTML = "";
    	}
    	if (parseInt(this.PerfSeatNum) > 0 && this.PerfSeatNum.length > 0) {
    		total++;
    		$$("btnSeat").childNodes[0].childNodes[1].innerHTML = this.PerfSeatNum + "座";
    		if (this.PerfSeatNum == "8-9999") {
    			$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "7座以上";
    		}
    	}
    	else {
    		$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "";
    	}

    	if (total > 0) {
    		$$("m-btn-more").innerHTML = "完成(" + total + ")";
    		this.MoreFilterNum = total;
    		$("#m-btn-more-clear").addClass("btn-clear-selected");
    	}
    	else {
    		$$("m-btn-more").innerHTML = "完成";
    		$("#m-btn-more-clear").removeClass("btn-clear-selected");
    	}
    	$(".morestyle").removeClass("current");
        //$("#drivetype" + this.DriveType.toString()).addClass("current");
    	$(".drivetype" + this.DriveType.toString()).addClass("current");
    	$("#fueltype" + this.FuelType.toString()).addClass("current");
    	var doorNum = 0;
    	if (this.BodyDoors != "") {
    		doorNum = this.BodyDoors.charAt(0);
    	}
    	var seatNum = 0;
    	if (this.PerfSeatNum != "") {
    		seatNum = this.PerfSeatNum.charAt(0);
    	}
    	$("#doors" + doorNum.toString()).addClass("current");
    	$("#seatnum" + seatNum.toString()).addClass("current");
    }

	, SetMoreCondition: function (mcConditionStr) {
		var counter = 0;
		var maxNum = mcConditionStr.length;
		if (maxNum > 28)
			maxNum = 28;
		for (i = 0; i < maxNum; i++) {
			var mcChar = mcConditionStr.charAt(i);
			if (mcChar == '0' || mcChar == '1')
				this.MoreCondition[i] = mcChar;
		}
	}
	//获取请求参数
	, GetSearchQueryString: function () {
		var qsArray = new Array();
		if (this.Price.length > 0)
			qsArray.push("p=" + this.Price);
		if (this.Level != 0)
			qsArray.push("l=" + this.Level.toString());
		if (this.Displacement.length > 0)
			qsArray.push("d=" + this.Displacement);
		if (this.TransmissionType != 0)
			qsArray.push("t=" + this.TransmissionType.toString());
		var mc = this.MoreCondition.join('');
		if (mc.indexOf('1') >= 0)
			qsArray.push("m=" + mc);
		if (this.View != 0)
			qsArray.push("v=" + this.View.toString());
		if (this.Sort != 0)
			qsArray.push("s=" + this.Sort.toString());
		if (this.Brand != 0)
			qsArray.push("g=" + this.Brand.toString());
		if (this.BodyForm != 0)
			qsArray.push("b=" + this.BodyForm.toString());
		if (this.IsWagon && this.IsWagon == 1)
			qsArray.push("lv=" + this.IsWagon);
		if (this.DriveType && this.DriveType > 0)
			qsArray.push("dt=" + this.DriveType);
		if (this.FuelType && this.FuelType > 0)
			qsArray.push("f=" + this.FuelType);
		if (this.BodyDoors && this.BodyDoors.length > 0)
			qsArray.push("bd=" + this.BodyDoors);
		if (this.PerfSeatNum && this.PerfSeatNum.length > 0)
			qsArray.push("sn=" + this.PerfSeatNum);
		return qsArray.join('&');
	}
}
function $$(id) { return document.getElementById(id); }

function addEvent(elm, type, fn, useCapture) {
	if (elm.addEventListener) {
		elm.addEventListener(type, fn, useCapture);
		return true;
	} else if (elm.attachEvent) {
		var r = elm.attachEvent('on' + type, fn);
		return r;
	} else {
		elm['on' + type] = fn;
	}
}
function GotoPage(conditionStr) {
	if (conditionStr.length >= 1) {
		var conType = conditionStr.charAt(0);
		var conStr = conditionStr.substr(1);
		switch (conType) {
			case 'p':
				SelectCarTool.Price = conStr;
				break;
			case 'l':
				SelectCarTool.Level = parseInt(conStr);
				break;
			case 'd':
				SelectCarTool.Displacement = conStr;
				break;
			case 't':
				SelectCarTool.TransmissionType = parseInt(conStr);
				break;
			case 'v':
				SelectCarTool.View = parseInt(conStr);
				break;
			case 's':
				SelectCarTool.Sort = parseInt(conStr);
				break;
			case 'g':
				SelectCarTool.Brand = parseInt(conStr);
				break;
			case 'b':
				var tmp_bodyform = 0;
				for (var key in SelectCarTool.BodyFormName) {
					var bodyform = getElementById("bodyform_" + key);
					if (bodyform && bodyform.checked) {
						tmp_bodyform = parseInt(tmp_bodyform) + parseInt(key);
					}
				}
				SelectCarTool.BodyForm = tmp_bodyform;
				break;
			case 'm':
				SelectCarTool.SetMoreCondition();
				break;
		}
	}
	var queryString = SelectCarTool.GetSearchQueryString();
	var toUrl = _SearchUrl;
	if (queryString.length > 0)
		toUrl += "?" + queryString;
	window.location.href = toUrl;
}

function GotoPageMore(conditionStr, value) {
	var btnType = "";
	if (conditionStr.length >= 1) {
		switch (conditionStr) {
			case 'dt':
				if (value != "-1") {
					SelectCarTool.DriveType = parseInt(value);
				}
				btnType = "m-filter-drive";
				break;
			case 'f':
				if (value != "-1") {
					SelectCarTool.FuelType = parseInt(value);
				}
				btnType = "m-filter-fuel";
				break;
			case 'bd':
				if (value != "-1") {
					SelectCarTool.BodyDoors = value;
				}
				btnType = "m-filter-door";
				break;
			case 'sn':
				if (value != "-1") {
					SelectCarTool.PerfSeatNum = value;
				}
				btnType = "m-filter-seat";
				break;
		}
	}
	var pop = document.getElementById(btnType);
	var pop1 = document.getElementById(btnType + "-leftPopup");
	if (pop && (pop.style.display == "block" || pop.className == "swipeLeft swipeLeft-block")) {
		pop.className = "swipeLeft";
		$$("m-filter-more").className = "swipeLeft swipeLeft-block";
		$$("m-filter-more-leftPopup").style.display = "block";
		pop1.style.display = "none";
	}
	if (value != "-1") {
		SelectCarTool.initMore();
	}
}
function GotoPageSuv(conditionStr, value) {
	var btnType = "m-filter-levelsuv"
	if (conditionStr.length >= 1 && conditionStr == "l") {
		var pop = document.getElementById(btnType);
		var pop1 = document.getElementById(btnType + "-leftPopup");
		if (pop && (pop.style.display == "block" || pop.className == "swipeLeft swipeLeft-block")) {
			pop.className = "swipeLeft";
			$$("m-filter-level").className = "swipeLeft swipeLeft-block";
			$$("m-filter-level-leftPopup").style.display = "block";
			pop1.style.display = "none";
		}
	}
}

