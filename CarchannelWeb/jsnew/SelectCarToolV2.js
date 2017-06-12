function getElementById(i) { return document.getElementById(i); }
//cookie 操作
var CookieHelper = {
	//截取字符串的空格符
	trim: function (value) {
		return (value || "").replace(/^\s+|\s+$/g, "");
	},
	//设置cookie
	setCookie: function (name, value, options) {
		if (typeof value != 'undefined') { // name and value given, set cookie
			options = options || {};
			if (value === null) {
				value = '';
				options.expires = -1;
			}
			var expires = '';
			if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
				var date;
				if (typeof options.expires == 'number') {
					date = new Date();
					date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
				} else {
					date = options.expires;
				}
				expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
			}
			var path = options.path ? '; path=' + options.path : '';
			var domain = options.domain ? '; domain=' + options.domain : '';
			var secure = options.secure ? '; secure' : '';
			document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
		}
	},
	// get cookie
	readCookie: function (name) {
		var cookieValue = null;
		if (document.cookie && document.cookie != '') {
			var cookies = document.cookie.split(';');
			for (var i = 0; i < cookies.length; i++) {
				var cookie = this.trim(cookies[i]);
				// Does this cookie string begin with the name we want?
				if (cookie.substring(0, name.length + 1) == (name + '=')) {
					cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
					break;
				}
			}
		}
		return cookieValue;
	}
}
var SelectCar = SelectCar || {};
SelectCar.Tools = {
	getQueryObject: function (queryString) {
		var result = {},
		  re = /([^&=]+)=([^&]*)/g, m;
		while (m = re.exec(queryString)) {
			result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
		}
		return result;
	},
	getQueryString: function (data) {
		var tdata = '';
		for (var key in data) {
			tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
		}
		tdata = tdata.replace(/^&/g, "");
		return tdata
	},
	addEvent: function (elm, type, fn, useCapture) {
		if (elm.addEventListener) {
			elm.addEventListener(type, fn, useCapture);
			return true;
		} else if (elm.attachEvent) {
			var r = elm.attachEvent('on' + type, fn);
			return r;
		} else {
			elm['on' + type] = fn;
		}
	},
	getElementByClassName: function (tagName, className) {
		var classObj = document.getElementsByTagName(tagName);
		var len = classObj.length;
		for (var i = 0; i < len; i++) {
			if (classObj[i].className == className) {
				return classObj[i];
				break;
			}
		}
	},
	hasClass: function (element, className) {
		var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
		return element.className.match(reg);
	},
	addClass: function (element, className) {
		if (!this.hasClass(element, className)) {
			element.className += " " + className;
		}
	},
	removeClass: function (element, className) {
		if (this.hasClass(element, className)) {
			var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
			element.className = element.className.replace(reg, ' ');
		}
	}
}
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
//查询条件对象
var conditionObj =
{
	Price: ""
	, Level: 0					//0不限，1-10对应各级别
	, LevelName: new Array('', '微型车', '小型车', '紧凑型车', '中大型车', '中型车', '豪华车', 'MPV', 'SUV', '跑车', '其他', '面包车', '皮卡', '小型SUV', '紧凑型SUV', '中型SUV', '中大型SUV', '大型SUV')		//级别名称
	, Displacement: ""
	, TransmissionType: 0		//0不限，1手动，2自动
	, TransmissionTypeName: { 1: "手动", 126: "自动", 32: "半自动（AMT）", 2: "自动（AT）", 4: "手自一体", 8: "无极变速（CVT）", 16: "双离合（DSG）" }
	, DriveType: 0	//驱动方式
	, DriveTypeName: { 1: "前驱", 2: "后驱", 252: "四驱", 4: "全时四驱", 8: "分时四驱", 16: "适时四驱", 32: "智能四驱", 64: "四轮驱动", 128: "前置四驱" }
	, FuelType: 0	//燃料类型
	, FuelTypeName: { 7: "汽油", 8: "柴油", 2: "混合动力", 16: "纯电力", 4: "油气混合" }
	, BodyDoors: ""	//车门数
	, PerfSeatNum: ""	//座位数
	, IsWagon: 0	//是否旅行版
	, MoreCondition: []
	, MoreConditionName: []
	, View: 0					//0默认大图显示，1列表显示
	, Sort: 0					//0默认显示关注高-低，1关注低-高，2按价格排列低-高，3价格高-低
	, Brand: 0					//0不限，1自主，2合资，3进口
	, BrandName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口", 8: "德系", 9: "美系", 10: "日韩", 11: "欧系", 12: "日本", 16: "韩国"}//new Array('不限', '自主', '合资', '', '进口', "", "德系", "日韩", "美系", "欧系")
	, BodyForm: 0				//0不限，1两厢及掀背，2三厢
	, BodyFormName: { 1: "两厢", 2: "三厢" }
 	, toolKey: true				//展开开关
	, showPeizhi: false          //是否显示配置
	, Type: "car"
	, Domain: window.location.host
	, PriceTimer: 0
	//初始化对象
	, InitObject: function () {
		for (i = 0; i < 28; i++)
			this.MoreCondition.push('0');
		this.MoreConditionName[0] = "涡轮增压";
		this.MoreConditionName[1] = "四轮驱动";
		this.MoreConditionName[2] = "四轮碟刹";
		this.MoreConditionName[3] = "天窗";
		this.MoreConditionName[4] = "前后电动车窗";
		this.MoreConditionName[5] = "皮座椅";
		this.MoreConditionName[6] = "电动座椅";
		this.MoreConditionName[7] = "座椅加热";
		this.MoreConditionName[8] = "自动空调";
		this.MoreConditionName[9] = "电动外后视镜";
		this.MoreConditionName[10] = "ESP";
		this.MoreConditionName[11] = "倒车影像";
		this.MoreConditionName[12] = "倒车雷达";
		this.MoreConditionName[13] = "GPS导航";
		this.MoreConditionName[14] = "自动泊车";
		this.MoreConditionName[15] = "定速巡航";
		this.MoreConditionName[16] = "无钥匙启动";
		this.MoreConditionName[17] = "安全带未系提示";
		this.MoreConditionName[18] = "主动安全头枕";
		this.MoreConditionName[19] = "儿童锁";
		this.MoreConditionName[20] = "儿童座椅固定";
		this.MoreConditionName[21] = "氙气大灯";
		this.MoreConditionName[22] = "5座位以上";
		this.MoreConditionName[23] = "胎压监测";
		this.MoreConditionName[24] = "新能源";
		this.MoreConditionName[25] = "空气净化器";
		this.MoreConditionName[26] = "电动窗防夹";
		this.MoreConditionName[27] = "换挡拨片";
	}
	//初始化页面显示
	, InitPageCondition: function () {
		this.InitPrice();
		this.InitLevel();
		this.InitDisplacement();
		this.InitTransmisstionType();
		this.InitMoreCondition();
		this.InitBrandType();
		this.InitBodyForm();
		this.InitDriveType();
		this.InitFuelType();
		this.InitBodyDoors();
		this.InitPerfSeatNum();
		this.InitEvent();
		SetLastSelectCarConditionLink(this.Type, _SearchUrl); //设置最后选车条件
		this.SaveConditionCookie(this.Type, this.Domain); //设置选车条件cookies
	}
	, InitEvent: function () {
		var self = this;
		var price = getElementById("p_custom");
		if (price) {
			SelectCar.Tools.addEvent(price, "mouseover", function () {
				clearTimeout(self.PriceTimer);
				var popup = getElementById("p_popup");
				popup.style.display = "block";
				if (!SelectCar.Tools.hasClass(price, "current"))
					SelectCar.Tools.addClass(price, "current");
			}, false);
			SelectCar.Tools.addEvent(price, "mouseout", function () {
				var popup = getElementById("p_popup");
				self.PriceTimer = setTimeout(function () {
					popup.style.display = "none";
					var priceArr = ["0-5", "5-8", "8-12", "12-18", "18-25", "25-40", "40-80", "80-9999"];
					if ((!self.Price || self.Price == "") || (self.Price && priceArr.indexOf(self.Price) > 0))
						SelectCar.Tools.removeClass(price, "current");
				}, 300);
			}, false);
		}
		var btnPriceSubmit = getElementById("btnPriceSubmit");
		if (btnPriceSubmit) {
			SelectCar.Tools.addEvent(btnPriceSubmit, "click", function () {
				var minP = getElementById("p_min").value;
				var maxP = getElementById("p_max").value;
				if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0)))
					getElementById("p_alert").innerHTML = "价格不能为空。";
				else if (maxP != "" && parseInt(maxP) <= 0) { getElementById("p_alert").innerHTML = "请填写正确的价格区间。"; }
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
							getElementById("p_alert").innerHTML = "请填写正确的价格区间。";
					}
				}
			}, true);
		}
		var transEle = getElementById("trans126");
		if (transEle) {
			SelectCar.Tools.addEvent(transEle, "mouseover", function () {
				var popup = getElementById("trans_popup");
				popup.style.display = "block";
				transEle.className = "last current";
			}, false);
			SelectCar.Tools.addEvent(transEle, "mouseout", function () {
				var popup = getElementById("trans_popup");
				popup.style.display = "none";
				if (!self.TransmissionType || self.TransmissionType < 2)
					transEle.className = "last";
			}, false);
		}
		var driveType = getElementById("drivetype252");
		if (driveType) {
			SelectCar.Tools.addEvent(driveType, "mouseover", function () {
				var popup = getElementById("drivetype_popup");
				popup.style.display = "block";
				driveType.className = "last current";
			}, false);
			SelectCar.Tools.addEvent(driveType, "mouseout", function () {
				var popup = getElementById("drivetype_popup");
				popup.style.display = "none";
				if (!self.DriveType || self.DriveType <= 2)
					driveType.className = "last";
			}, false);
		}

		var level = getElementById("level8");
		if (level) {
			SelectCar.Tools.addEvent(level, "mouseover", function () {
				var popup = getElementById("suv_popup");
				popup.style.display = "block";
				level.className = "last current";
			}, false);
			SelectCar.Tools.addEvent(level, "mouseout", function () {
				var popup = getElementById("suv_popup");
				popup.style.display = "none";
				if (!self.Level || [8, 13, 14, 15, 16, 17].indexOf(self.Level) == -1)
					level.className = "last";
			}, false);
		}
	}
	//初始化价格
	, InitPrice: function () {
		switch (this.Price) {
			case "0-5":
				getElementById("price1").className = "current";
				break;
			case "5-8":
				getElementById("price2").className = "current";
				break;
			case "8-12":
				getElementById("price3").className = "current";
				break;
			case "12-18":
				getElementById("price4").className = "current";
				break;
			case "18-25":
				getElementById("price5").className = "current";
				break;
			case "25-40":
				getElementById("price6").className = "current";
				break;
			case "40-80":
				getElementById("price7").className = "current";
				break;
			case "80-9999":
				getElementById("price8").className = "current";
				break;
			default:
				if (this.Price != "") {
					var customP = getElementById("p_custom");
					SelectCar.Tools.addClass(customP, "current");
					customP.firstChild.innerHTML = "自定义" + this.Price + "万";
					var arrayPrice = this.Price.split("-");
					if (arrayPrice.length == 2) {
						getElementById("p_min").value = arrayPrice[0];
						getElementById("p_max").value = arrayPrice[1];
					}
				}
				else
					getElementById("price0").className = "current";
				break;
		}
	}
	//初始化级别
	, InitLevel: function () {
		if (this.Level == "")
			this.Level = "0";
		var levelEle = getElementById("level" + this.Level);
		if (levelEle)
			levelEle.className = "current";
		if ([13, 14, 15, 16, 17].indexOf(this.Level) != -1) {
			levelSuvEle = getElementById("level8");
			if (levelSuvEle) {
				levelSuvEle.className = "last current";
				levelSuvEle.firstChild.innerHTML = this.LevelName[parseInt(this.Level)];
				if (levelEle && this.Level != 8) {
					levelEle.firstChild.href = "javascript:;";
					levelEle.firstChild.className = "none";
				}
			}
		}
		//		//轿车级别
		//		if (this.Level <= 6 && this.Level > 0 || this.Level == 63) {
		//			levelEle = getElementById("level63_1");
		//			if (levelEle)
		//				levelEle.className = "current";
		//		}
		//		this.OpenOrCloseJiaocheBox();
	}
	//初始化排量
	, InitDisplacement: function () {
		switch (this.Displacement) {
			case "0-1.3":
				getElementById("dis1").className = "current";
				break;
			case "1.3-1.6":
				getElementById("dis2").className = "current";
				break;
			case "1.7-2.0":
				getElementById("dis3").className = "current";
				break;
			case "2.1-3.0":
				getElementById("dis4").className = "current";
				break;
			case "3.1-5.0":
				getElementById("dis5").className = "current";
				break;
			case "5.0-9":
				getElementById("dis6").className = "current";
				break;
			default:
				getElementById("dis0").className = "current";
				break;
		}
	}
	//初始化变速箱类型
	, InitTransmisstionType: function () {
		var transEle = getElementById("trans" + this.TransmissionType.toString());
		if (transEle)
			transEle.className = "current";
		if (this.TransmissionType >= 2) {
			transATEle = getElementById("trans126");
			if (transATEle) {
				transATEle.className = "last current";
				transATEle.firstChild.innerHTML = this.TransmissionTypeName[this.TransmissionType];
				if (transEle && this.TransmissionType != 126) {
					transEle.firstChild.href = "javascript:;";
					transEle.firstChild.className = "none";
				}
			}
		}
	}
	, InitBodyForm: function () {
		var str = "bodyform", v = this.BodyForm;
		if (this.IsWagon > 0) {
			str = "wagon";
			v = this.IsWagon;
		}
		var bodyFormEle = getElementById(str + v);
		if (bodyFormEle)
			bodyFormEle.className = "current";
	}
	, InitDriveType: function () {
		var driveType = getElementById("drivetype" + this.DriveType);
		if (driveType)
			driveType.className = "current";
		if (this.DriveType > 2) {
			driveTypeEle = getElementById("drivetype252");
			if (driveTypeEle) {
				driveTypeEle.className = "last current";
				driveTypeEle.firstChild.innerHTML = this.DriveTypeName[this.DriveType];
				if (driveType && this.DriveType != 252) {
					driveType.firstChild.href = "javascript:;";
					driveType.firstChild.className = "none";
				}
			}
		}
	}
	, InitFuelType: function () {
		var fuelTypeEle = getElementById("fueltype" + this.FuelType);
		if (fuelTypeEle)
			fuelTypeEle.className = "current";
	}
	, InitBodyDoors: function () {
		switch (this.BodyDoors) {
			case "2-3":
				getElementById("doors1").className = "current";
				break;
			case "4-5":
				getElementById("doors2").className = "current";
				break;
			default:
				getElementById("doors0").className = "current";
				break;
		}
	}
	, InitPerfSeatNum: function () {
		switch (this.PerfSeatNum) {
			case "2":
				getElementById("seatnum1").className = "current";
				break;
			case "4-5":
				getElementById("seatnum2").className = "current";
				break;
			case "6":
				getElementById("seatnum3").className = "current";
				break;
			case "7":
				getElementById("seatnum4").className = "current";
				break;
			case "8-9999":
				getElementById("seatnum5").className = "current";
				break;
			default:
				getElementById("seatnum0").className = "current";
				break;
		}
	}
	//根据选中的规则打开或关闭自动的详细选项框
	, OpenOrCloseZidongBox: function () {
		var zidongBox = getElementById("zidongBox");
		if (!zidongBox)
			return;
		//打开详细选项
		if (this.TransmissionType >= 2)
			zidongBox.style.display = "block";
		else
			zidongBox.style.display = "none";
	}
	//打开或关闭轿车的层
	, OpenOrCloseJiaocheBox: function () {
		var jiaocheBox = getElementById("jiaocheBox");
		if (!jiaocheBox)
			return;
		if (this.Level > 6 && this.Level != 63)
			jiaocheBox.style.display = "none";
		else
			jiaocheBox.style.display = "block";
	}
	, InitBrandType: function () {
		var brandEle = getElementById("brandType" + this.Brand.toString());
		if (brandEle)
			brandEle.className = "current";
	}
	//初始化更多条件
	, InitMoreCondition: function () {
		for (i = 0; i < this.MoreCondition.length; i++) {
			if (this.MoreCondition[i] == '1') {
				var mcCheckEle = getElementById("mcCheck" + i.toString());
				if (mcCheckEle)
					mcCheckEle.checked = true;
			}
		}
	}
	//设置更多条件
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
	, GetMoreconditionDescription: function (mcConditionStr) {
		var counter = 0;
		var maxNum = mcConditionStr.length;
		if (maxNum > 26)
			maxNum = 26;
		var nameArray = new Array();
		for (i = 0; i < maxNum; i++) {
			var mcChar = mcConditionStr.charAt(i);
			if (mcChar == '1')
				nameArray.push(this.MoreConditionName[i]);
		}
		return nameArray.join(',');
	}
	//获取当前条件的查询字符串
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
	//是否有查询条件
	, HasSelectCondition: function () {
		return (this.Price.length > 0 || this.Level != 0
		 || this.Displacement.length > 0
		 || this.TransmissionType != 0
		 || this.MoreCondition.toString().indexOf('1') > -1
		 || this.Brand != 0 || this.BodyForm != 0 || this.DriveType != 0 || this.FuelType != 0 || this.BodyDoors.length > 0 || this.PerfSeatNum.length > 0 || this.IsWagon == 1)
	}
	//保存当前的搜索条件
	, SaveConditionCookie: function (pType, domain) {
		if (typeof CookieHelper == 'undefined')
			return;
		if (!this.HasSelectCondition())
			return;
		var cookiesName = this.GetCookieName(pType);
		var queryStr = this.GetSearchQueryString();
		var cOption = new Object();
		cOption["expires"] = 360;
		cOption["path"] = "/";
		cOption["domain"] = domain;
		var cookiesObj = SelectCar.Tools.getQueryObject(CookieHelper.readCookie(cookiesName));
		var strCookies = "";
		if (typeof cookiesObj["k"] === "undefined") {
			strCookies = "c=" + encodeURIComponent(queryStr) + "&k=0";
		} else {
			strCookies = "c=" + encodeURIComponent(queryStr) + "&k=" + cookiesObj["k"];
		}
		CookieHelper.setCookie(cookiesName, strCookies, cOption);
	}
	, GetCookieName: function (pType) {
		return pType + "_" + "LastSelectCarCondition";
	}
	//获取查询条件说明
	, GetConditionDescription: function (queryStr) {
		if (queryStr == null || typeof queryStr == 'undefined')
			return "";
		var desArray = new Array();
		var conArray = queryStr.split('&');
		for (i = 0; i < conArray.length; i++) {
			var queryKV = conArray[i].split('=');
			if (queryKV.length == 2) {
				var desc = this.GetConditionDescriptionByKeyValue(queryKV[0], queryKV[1]);
				desArray.push(desc);
			}
		}
		return desArray.join(',');
	}
	//获取选车条件的描述
	, GetConditionDescriptionByKeyValue: function (keyStr, valueStr) {
		var valueDes = "";
		switch (keyStr) {
			case "p":
				if (valueStr == '0-5')
					valueDes = "5万以下";
				else if (valueStr == '80-9999')
					valueDes = "80万以上";
				else
					valueDes = valueStr + "万";
				break;
			case "d":
				if (valueStr == '0-1.3')
					valueDes = "1.3L以下";
				else if (valueStr == '5.0-9')
					valueDes = "5.0L以上";
				else
					valueDes = valueStr + "L";
				break;
			case "l":
				if (valueStr == "63")
					valueDes = "轿车";
				else
					valueDes = this.LevelName[parseInt(valueStr)];
				break;
			case "t":
				valueDes = this.TransmissionTypeName[valueStr];
				break;
			case "g":
				valueDes = this.BrandName[parseInt(valueStr)];
				break;
			case "m":
				valueDes = this.GetMoreconditionDescription(valueStr);
				break;
			case "b":
				valueDes = this.BodyFormName[valueStr];
				break;
			case "dt":
				valueDes = this.DriveTypeName[valueStr];
				break;
			case "f":
				valueDes = this.FuelTypeName[valueStr];
				break;
			case "lv":
				if (valueStr == "1")
					valueDes = "旅行版";
				break;
			case "bd":
				if (valueStr != "")
					valueDes = valueStr + "门";
				break;
			case "sn":
				if (valueStr != "" && valueStr != "0")
					valueDes = valueStr + "座";
				break;
		}

		return valueDes;
	}
}
//初始化条件对象
conditionObj.InitObject();
//删除一个更多条件设置
function DelMoreCondition(mcIndex) {
	var mcLiEle = getElementById("mcLi" + mcIndex);
	//alert(mcLiEle);
	var mcCheckEle = getElementById("mcCheck" + mcIndex);
	if (mcLiEle)
		mcLiEle.parentNode.removeChild(mcLiEle);
	if (mcCheckEle)
		mcCheckEle.checked = false;
	mcIndex = parseInt(mcIndex);
	conditionObj.MoreCondition[mcIndex] = '0';
	GotoPage("m");
}
//删除一个车身选项
function delBodyForm(index) {
	var bodyform = getElementById("bodyform_" + index);
	if (bodyform) {
		bodyform.checked = false;
	}
	GotoPage("b");
}
//确定设置更多条件
function SetMoreCondition() {
	for (i = 0; i < 28; i++) {
		var mcCheckEle = getElementById("mcCheck" + i);
		if (mcCheckEle && mcCheckEle.checked)
			conditionObj.MoreCondition[i] = '1';
		else
			conditionObj.MoreCondition[i] = '0';
	}
}
//根据条件页面转向
function GotoPage(conditionStr) {
	if (conditionStr.length >= 1) {
		var conType = conditionStr.charAt(0);
		var conStr = conditionStr.substr(1);
		//		if (conType == 'p' || conType == 'l' || conType == 'd' || conType == 't' || conType == 'm') {
		//			conditionObj.BodyForm = 0;
		//		}
		switch (conType) {
			case 'p':
				conditionObj.Price = conStr;
				break;
			case 'l':
				conditionObj.Level = parseInt(conStr);
				break;
			case 'd':
				conditionObj.Displacement = conStr;
				break;
			case 't':
				conditionObj.TransmissionType = parseInt(conStr);
				break;
			case 'v':
				conditionObj.View = parseInt(conStr);
				break;
			case 's':
				conditionObj.Sort = parseInt(conStr);
				break;
			case 'g':
				conditionObj.Brand = parseInt(conStr);
				break;
			case 'b':
				var tmp_bodyform = 0;
				for (var key in conditionObj.BodyFormName) {
					var bodyform = getElementById("bodyform_" + key);
					if (bodyform && bodyform.checked) {
						tmp_bodyform = parseInt(tmp_bodyform) + parseInt(key);
					}
				}
				conditionObj.BodyForm = tmp_bodyform;
				//conditionObj.BodyForm = parseInt(conStr);
				break;
			case 'm':
				SetMoreCondition();
				break;
		}
	}
	var queryString = conditionObj.GetSearchQueryString();
	var toUrl = _SearchUrl;
	if (queryString.length > 0)
		toUrl += "?" + queryString;
	//alert(toUrl);
	window.location.href = toUrl;
}
//设置上一次选车条件
function SetLastSelectCarConditionLink(pType, searchUrl) {
	var lastSel = document.getElementById("lastSelectCar");
	if (!lastSel)
		return;

	var btnShowSelectCar = document.getElementById("btnShowSelectCar");
	if (btnShowSelectCar && btnShowSelectCar.className != "current")
		return;
	//从cookie中读上次选车条件
	var queryStr = SelectCar.Tools.getQueryObject(CookieHelper.readCookie(conditionObj.GetCookieName(pType)));
	queryStr = queryStr["c"];
	var queryDesc = conditionObj.GetConditionDescription(queryStr);
	var lastSCHtml = "";
	if (queryDesc.length > 0) {
		var selHref = searchUrl + "?" + queryStr;
		lastSCHtml = '<a href="' + selHref + '" title="' + queryDesc + '">我上次的选车条件</a>';
	}
	//设置清空是否显示
	if (conditionObj.HasSelectCondition()) {
		if (lastSCHtml.length > 0)
			lastSCHtml += ' | ';
		lastSCHtml += '<a href="' + searchUrl + '">清空条件</a>';
	}

	if (lastSCHtml.length > 0)
		lastSel.innerHTML = lastSCHtml;
}
