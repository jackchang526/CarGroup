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
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
//var adObj = [{ Name: "Level", Pos: 3, SerialId: 3152, Url: "", Imageurl: "", Starttime: "2015-8-21", Endtime: "2015-8-31" }];

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
	, Brand: 0
    , Country: 0					//0不限，1自主，2合资，3进口
	, BrandName: { 7: "不限", 8: "德系", 9: "美系", 10: "日韩", 11: "欧系", 12: "日本", 16: "韩国" }//new Array('不限', '自主', '合资', '', '进口', "", "德系", "日韩", "美系", "欧系")
    , CountryName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口" }
	, BodyForm: 0				//0不限，1两厢及掀背，2三厢
	, BodyFormName: { 1: "两厢", 2: "三厢" }
 	, toolKey: true				//展开开关
	, showPeizhi: false          //是否显示配置
	, Type: "car"
	, Domain: window.location.host
	, PriceTimer: 0
    //, FuelConsumption: ""
    , apiUrl: "http://select.car.yiche.com/selectcartool/searchresult"
    , Page: 1
	//初始化页面显示
	, InitPageCondition: function () {
		this.InitPrice();
		this.InitLevel();
		this.InitDisplacement();
		this.InitTransmisstionType();
		this.InitMoreCondition();
		this.InitBrandType();
		this.InitCountry();
		this.InitBodyForm();
		this.InitDriveType();
		this.InitFuelType();
	    //this.InitFuelConsumption();
		this.InitEvent();
		$("li[id^=more_]").click(this.singleFilterClickEvent);
		$("input[type='checkbox']").click(this.filterClickEvent);
		SetClearCarConditionLink(_SearchUrl); //清空条件
		this.GetSearchResult();
	}
    , InitPageConditionV2: function () {
        this.InitEvent();
        $("input[type='checkbox']").click(this.filterClickEvent);
    }
	, InitEvent: function () {
		var self = this;
		var btnPriceSubmit = getElementById("btnPriceSubmit");
		if (btnPriceSubmit) {
			SelectCar.Tools.addEvent(btnPriceSubmit, "click", function () {
				var minP = getElementById("p_min").value;
				var maxP = getElementById("p_max").value;
				if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0)))
					getElementById("p_alert").innerHTML = "<div class=\"tc-box\"><p><em></em>价格不能为空。</p></div>";
				else if (maxP != "" && parseInt(maxP) <= 0) { getElementById("p_alert").innerHTML = "<div class=\"tc-box\"><p><em></em>请填写正确的价格区间。</p></div>"; }
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
							getElementById("p_alert").innerHTML = "<div class=\"tc-box\"><p><em></em>请填写正确的价格区间。</p></div>";
					}
				}
			}, true);
		}
		var btnPriceCus = getElementById("btnPriceCus");
		if (btnPriceCus) {
			SelectCar.Tools.addEvent(btnPriceCus, "click", function () {
				var customP = getElementById("p_custom");
				customP.style.display = "none";
				$("#p_custom_value").remove();
				var customN = getElementById("p_custom_null");
				customN.style.display = "block";
			})
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
    , filterClickEvent: function () {
    	var id, self = $(this)[0];
    	id = self.id;
    	var element = getElementById(id);
    	if (element) {
    		var conType = id.split('_')[0];
    		var conStr = id.split('_')[1];
    		if (conType != "more") {
    			return;
    		}
    		if (element.checked) {
    			if (conditionObj.MoreCondition.indexOf(conStr) > -1) {
    				return;
    			}
    			conditionObj.MoreCondition.push(conStr);
    		}
    		else {
    			if (conditionObj.MoreCondition.indexOf(conStr) > -1) {
    				conditionObj.MoreCondition.remove(conStr);
    			}
    		}
    		GotoPage("more");
    	}
    }
    , singleFilterClickEvent: function () {
    	var id, self = $(this)[0];
    	id = self.id;
    	var element = getElementById(id);
    	if (element) {
    		var conType = id.split('_')[0];
    		var conStr = id.split('_')[1];
    		if (conType != "more") {
    			return;
    		}
    		if ((conStr >= 262 && conStr <= 267) || conStr == 1) {
    			//座位数
    			for (var i = 262; i <= 267; i++) {
    				if (conStr != i && conditionObj.MoreCondition.indexOf(i) > -1) {
    					conditionObj.MoreCondition.remove(i);
    				}
    				else if (conStr == i && conditionObj.MoreCondition.indexOf(i) == -1 && conStr != 1) {
    					conditionObj.MoreCondition.push(conStr);

    				}
    			}
    		}
    		else if ((conStr >= 268 && conStr <= 270) || conStr == 0) {
    			//车门数
    			for (var i = 268; i <= 270; i++) {
    				if (conStr != i && conditionObj.MoreCondition.indexOf(i) > -1) {
    					conditionObj.MoreCondition.remove(i);
    				}
    				else if (conStr == i && conditionObj.MoreCondition.indexOf(i) == -1 && conStr != 0) {
    					conditionObj.MoreCondition.push(conStr);
    				}
    			}
    		}
                //排放
    		else if ((conStr >= 122 && conStr <= 127) || conStr == 2 || conStr.indexOf('.') > -1) {
    		    if (conStr != "126.123" && conditionObj.MoreCondition.indexOf("126_123") > -1) {
    		        conditionObj.MoreCondition.remove("126_123");
    		    }
    		    else if (conStr != "125.122" && conditionObj.MoreCondition.indexOf("125_122") > -1) {
    		        conditionObj.MoreCondition.remove("125_122");
    		    }
    		    else {
    		        for (var i = 122; i <= 127; i++) {
    		            if (conStr != i && conditionObj.MoreCondition.indexOf(i) > -1) {
    		                conditionObj.MoreCondition.remove(i);
    		            }
    		        }
    		    }
    		    if (conditionObj.MoreCondition.indexOf(conStr) == -1 && conStr != 2) {
    		        conditionObj.MoreCondition.push(conStr);
    		    }
    		}
    		GotoPage("more");
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
					var customN = getElementById("p_custom_null");
					customN.style.display = "none";
					var arrayPrice = this.Price.split("-");
					if (arrayPrice.length == 2) {
						getElementById("p_min").value = "";
						getElementById("p_max").value = "";
					}
					var customP = getElementById("p_custom");
					customP.style.display = "block";
					$("<li class='current'id='p_custom_value'><a href='javascript:;'>" + this.Price + "万" + "</a></li>").insertBefore(customP);

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
	////初始化油耗
	//, InitFuelConsumption: function () {
	//	switch (this.FuelConsumption) {
	//		case "0-6":
	//			getElementById("fc1").className = "current";
	//			break;
	//		case "6-8":
	//			getElementById("fc2").className = "current";
	//			break;
	//		case "8-10":
	//			getElementById("fc3").className = "current";
	//			break;
	//		case "10-12":
	//			getElementById("fc4").className = "current";
	//			break;
	//		case "12-15":
	//			getElementById("fc5").className = "current";
	//			break;
	//		case "15-9999":
	//			getElementById("fc6").className = "current";
	//			break;
	//		default:
	//			getElementById("fc0").className = "current";
	//			break;
	//	}
	//}
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
    , InitCountry: function () {
    	var brandEle = getElementById("country" + this.Country.toString());
    	if (brandEle)
    		brandEle.className = "current";
    }
	//初始化更多条件
	, InitMoreCondition: function () {
		for (i = 0; i < this.MoreCondition.length; i++) {
			var mcCheckEle = getElementById("more_" + this.MoreCondition[i].toString());
			if (mcCheckEle && mcCheckEle.type.toLowerCase() == "checkbox") {
				mcCheckEle.checked = true;
			}
			else if (mcCheckEle) {
			    if (this.MoreCondition[i].indexOf(".") < 0) {
			        $("#more_" + this.MoreCondition[i].toString()).addClass("current").siblings().removeClass("current");
			    } else {
			        $("#more_" + this.MoreCondition[i].toString().replace(".","\\.")).addClass("current").siblings().removeClass("current");
			    }
			}
		}
	}
	//设置更多条件
	, SetMoreCondition: function (mcConditionStr) {
		var moreCondition = mcConditionStr.split('_');
		if (moreCondition.indexOf(126) > -1 && moreCondition.indexOf(123) > -1) {
		    moreCondition.remove(126);
		    moreCondition.remove(123);
		    moreCondition.push("126.123");
		}
		if (moreCondition.indexOf(125) > -1 && moreCondition.indexOf(122) > -1) {
		    moreCondition.remove(125);
		    moreCondition.remove(122);
		    moreCondition.push("125.122");
		}
		for (i = 0; i < moreCondition.length; i++) {
			var mcChar = moreCondition[i];
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
	, GetSearchQueryString: function (isApi) {
		var qsArray = new Array();
		if (this.Price.length > 0)
			qsArray.push("p=" + this.Price);
		if (this.Level != 0)
			qsArray.push("l=" + this.Level.toString());
		if (this.Displacement.length > 0)
			qsArray.push("d=" + this.Displacement);
		if (this.TransmissionType != 0)
			qsArray.push("t=" + this.TransmissionType.toString());
	    //var mc = this.MoreCondition.join('_');
		var tempMoreCondition = this.MoreCondition;
		for (var i = 0; i < tempMoreCondition.length; i++) {
		    if (tempMoreCondition[i].indexOf('.') > -1) {
		        tempMoreCondition[i] = tempMoreCondition[i].replace('.', '_');
		    }
		}
		var mc = tempMoreCondition.join('_');
		if (this.MoreCondition.length > 0) {
			//2-3门
			if (this.MoreCondition.indexOf("268") > -1 && isApi) {
				mc = mc + "_269";
			}
			//4-6门
			if (this.MoreCondition.indexOf("270") > -1 && isApi) {
				mc = mc + "_271_272";
			}
			//4-5座
			if (this.MoreCondition.indexOf("263") > -1 && isApi) {
				mc = mc + "_264";
			}
			//天窗形式-单天窗、双天窗、全景
			if (this.MoreCondition.indexOf("204") > -1 && isApi) {
				mc = mc + "_205_206";
			}
			//四轮碟刹
			if (this.MoreCondition.indexOf("141") > -1 && isApi) {
				mc = mc + "_144_143_145_146_148_149_150";
			}
			qsArray.push("more=" + mc);
		}
		if (this.View != 0)
			qsArray.push("v=" + this.View.toString());
		if (this.Sort != 0)
			qsArray.push("s=" + this.Sort.toString());
		if (this.Brand != 0)
			qsArray.push("g=" + this.Brand.toString());
		if (this.Country != 0)
			qsArray.push("c=" + this.Country.toString());
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
		if (this.Page && this.Page > 1)
			qsArray.push("page=" + this.Page);
		//if (this.FuelConsumption.length > 0)
		//	qsArray.push("fc=" + this.FuelConsumption);
		if (isApi)
			qsArray.push("external=Car");
		return qsArray.join('&');
	}
	//是否有查询条件
	, HasSelectCondition: function () {
		return (this.Price.length > 0 || this.Level != 0
		 || this.Displacement.length > 0
		 || this.TransmissionType != 0
		 || this.MoreCondition.length > 0
         //|| this.FuelConsumption.length > 0
		 || this.Brand != 0 || this.Country != 0 || this.BodyForm != 0 || this.DriveType != 0 || this.FuelType != 0 || this.BodyDoors.length > 0 || this.PerfSeatNum.length > 0 || this.IsWagon == 1)
	}
    , GetSearchResult: function () {
    	var apiQueryString = this.GetSearchQueryString(true);
    	var url = this.apiUrl;
    	if (apiQueryString.length > 0) {
    		url += "?" + apiQueryString;
    	}
    	$.ajax({
    		url: url,
    		dataType: "jsonp",
    		jsonpCallback: "jsonpCallback",
    		cache: true,
    		success: function (json) {
    			var result = json;
    			if (typeof ad_carlistdata != "undefined" && ad_carlistdata.length > 0) {
    				result.ResList = GetAdPosition(json);
    			}
    			DrawUlContent(result);
    		}
    	});
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
			case "c":
				valueDes = this.CountryName[parseInt(valueStr)];
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
				conditionObj.Page = 1;
				break;
			case 'g':
				conditionObj.Brand = parseInt(conStr);
				break;
			case 'c':
				conditionObj.Country = parseInt(conStr);
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
                //case ''
			//case 'fc':
			//	conditionObj.FuelConsumption = conStr;
			//	break;
		}
	}
	var queryString = conditionObj.GetSearchQueryString(false);
	var toUrl = _SearchUrl;
	if (queryString.length > 0) {
	    toUrl += "?" + queryString + "#anchorTitle";
	}
	//alert(toUrl);
	window.location.href = toUrl;
}
//设置清空
function SetClearCarConditionLink(searchUrl) {
	var lastSel = document.getElementById("lastSelectCar");
	if (!lastSel)
		return;
	//设置清空是否显示
	var lastSCHtml = "";
	if (conditionObj.HasSelectCondition()) {
		lastSCHtml += '<a href="' + searchUrl + '" class="btu-remove"><i></i>清空条件</a>';
	}

	if (lastSCHtml.length > 0)
		lastSel.innerHTML = lastSCHtml;
}
//广告
function GetAdPosition(json) {
	for (var j = 0; j < ad_carlistdata.length; j++) {
		var flag = false;
		//投放位置
		for (var index = 0; index < json.ResList.length; index++) {
			if (json.ResList[index].SerialId == ad_carlistdata[j].SerialId) {
				if (ad_carlistdata[j].Pos > 0) {
					json.ResList.remove(json.ResList[index]);
 					json.ResList.splice(ad_carlistdata[j].Pos - 1, 0, ad_carlistdata[j]);
					flag = true;
				}
				break;
			}
		}
		if (!flag) {
			json.ResList.splice(json.ResList.length - 1, 1);
			json.ResList.splice(ad_carlistdata[j].Pos - 1, 0, ad_carlistdata[j]);
		}
	}
	return json.ResList;
}
//更新车型列表
function DrawUlContent(json) {
	if (json.Count > 0) {
		$("#styleTotal").html("共 " + json.Count + "个车型，" + json.CarNumber + "个车款");
	}

	if (json.ResList.length == "0") {
		$("#noResult").show();
		$("#params-styleList").hide();
	} else {
		$("#params-styleList").show();
		$("#noResult").hide();
		//初始化车款列表        
		var divContentArray = new Array();
		divContentArray.push("<ul>");
		var currentLineCount = 0;
		$(json.ResList).each(function (index) {

			divContentArray.push("<li>");
			divContentArray.push("<a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\">");
			divContentArray.push("<img src=\"" + json.ResList[index].ImageUrl.replace("_5.", "_1.") + "\" alt=\"" + json.ResList[index].ShowName + "报价_价格\"/>");
			divContentArray.push("</a>");
			divContentArray.push("<div class=\"title\"><a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\">" + json.ResList[index].ShowName + "</a></div>");
			divContentArray.push("<div class=\"txt\">" + json.ResList[index].PriceRange + "</div>");
			divContentArray.push("<div class=\"seach_more\" bit-seachmore><a href=\"javascript:;\" bit-serial=\"" + json.ResList[index].SerialId + "\" bit-car=\"" + json.ResList[index].CarIdList + "\" bit-line=\"" + (currentLineCount - 1) + "\" bit-allspell=\"" + json.ResList[index].AllSpell + "\" class=\"sub-color\"><em>" + json.ResList[index].CarNum + "</em>个车款符合条件<i></i></a></div>");
			divContentArray.push("</li>");
			if (index != 0 && (index + 1) % 4 == 0) {
				divContentArray.push("<li class=\"c-list-2014 c-list-2014-pop\"  bit-line=\"" + currentLineCount + "\"></li>");
				currentLineCount++;
			}
		});
		if (json.ResList.length > 0 && json.ResList.length % 4 != 0) {
			divContentArray.push("<li class=\"c-list-2014 c-list-2014-pop\"  bit-line=\"" + currentLineCount + "\"></li>");
		}
		divContentArray.push("</ul>");
		var divContent = divContentArray.join("");
		$("#divContent").html(divContent);

	    //call koubei start
		if (conditionObj.Sort > 4)
		{
		    getKouBeiItem();
		}
	    //call koubei end
		InitPageControl(json.Count);
		callbackGetItem();
	}
}
//车型列表分页
function InitPageControl(pageCount) {
	$("#divPage").pagination(pageCount, {
		items_per_page: 20,
		num_display_entries: 8,
		link_to: "javascript:;",
		current_page: (conditionObj.Page - 1) <= 0 ? 0 : (conditionObj.Page - 1),
		num_edge_entries: 1,
		callback: function (index) { PageClick(index + 1); return false; },
		prev_text: " 上一页",
		next_text: "下一页 "
	});
}
function PageClick(num) {
	conditionObj.Page = num;
	var obj = getSearchObject();
	obj["page"] = num;
	location.href = 'http://' + window.location.host + window.location.pathname + "?" + getQueryString(obj);
}
function getQueryString(data) {
	var tdata = '';
	for (var key in data) {
		tdata += "&" + (key) + "=" + (data[key]);
	}
	tdata = tdata.replace(/^&/g, "");
	return tdata
}

function getSearchObject() {
	var results = {};
	var url = window.location.search.substr(1);
	if (url) {
		var srchArray = url.split("&");
		var tempArray = new Array();

		for (var i = 0; i < srchArray.length; i++) {
			tempArray = srchArray[i].split("=");
			results[tempArray[0]] = tempArray[1];
		}
	}
	return results;
}
function callbackGetItem() {
	var carSynData = {}, IE6 = ! -[1, ] && !window.XMLHttpRequest, IE7 = navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1;
	$("div[bit-seachmore] a[bit-serial]").bind("click", function () {
		var self = $(this);
		var serialId = $(this).attr("bit-serial");
		var allSpell = $(this).attr("bit-allspell");
		var carIds = $(this).attr("bit-car");
		var line = parseInt($(this).attr("bit-line"));
		var container = $("li[bit-line='" + (line + 1) + "']");
		if ($(this).parent().hasClass("seach_dow") && carSynData[serialId]) {
			if (IE6 || IE7) {
				container.hide(); self.parent().removeClass("seach_dow");
			}
			else
				container.slideUp(200, function () { self.parent().removeClass("seach_dow"); });
			return;
		}
		$(this).parent().addClass("seach_dow").closest("li").siblings().find(".seach_more.seach_dow").removeClass("seach_dow");

		if (carSynData[serialId]) {
			if (IE6 || IE7)
				container.hide().html(carSynData[serialId]).show().siblings(".c-list-2014.c-list-2014-pop").html('');
			else
				container.hide().html(carSynData[serialId]).slideDown(200).siblings(".c-list-2014.c-list-2014-pop").html('');
			$(".btn-close").click(function () { $(this).closest(".c-list-2014.c-list-2014-pop").hide().html(''); self.parent().removeClass("seach_dow"); });
			initCarListHover();
			// 对比浮动框 初始
		    // WaitCompareObj.Init();
		    // 对比浮动框 初始
			if (WaitCompareObj) {
			    WaitCompareObj.ShowNameForMiniListWaitCompare();
			    WaitCompareObj.AddBtnCompareEvent();
			}
		} else {
			$.ajax({
				url: "http://api.car.bitauto.com/CarInfo/GetCarListForSelectCar.ashx?serialId=" + serialId + "&carids=" + carIds, dataType: "jsonp", jsonpCallback: "callback", cache: true,
				beforeSend: function (xhr) {
				    $("div[bit-line='" + (line + 1) + "']").html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\"><tbody><tr><th width=\"32%\" class=\"pdL10\">车款</th><th width=\"8%\" class=\"pd-left-one\">关注度</th><th width=\"10%\" class=\"pd-left-one\">变速箱</th><th width=\"10%\" class=\"pd-left-two\">指导价</th><th width=\"10%\" class=\"pd-left-three\">参考最低价</th><th width=\"18%\"><div class=\"wenh\" onmouseover=\"javascript:$(this).children('.tc-wenh').show();return false;\" onmouseout=\"javascript:$(this).children('.tc-wenh').hide();return false;\"><div class=\"tc tc-wenh\" style=\"display:none;\"><div class=\"tc-box\"><i></i><p>全国参考最低价</p></div></div></div></th></tr><tr><td colspan=\"6\"><div id=\"carlist_loading\" class=\"pdL10\">正在加载...</div></td></tr></tbody></table>");
					//$(".btn-close").click(function () { $(this).closest(".tool-filter-table").html(''); $(".tool-filter-car").removeClass("current"); });
				},
				error: function (XMLHttpRequest, textStatus, errorThrown) {
					//alert("textStatus: " + textStatus);
				},
				success: function (data) {
					if (data.CarList.length <= 0) { $("#carlist_loading").html("暂无车型数据"); return; }
					var content = [];

					content.push("<div class=\"c-list-box\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
					content.push("<tbody>");
					content.push("<tr>");
					content.push("<th width=\"34%\" class=\"pdL10\">车款</th>");
					content.push("<th width=\"10%\" class=\"pd-left-one\">关注度</th>");
					content.push("<th width=\"10%\" class=\"pd-left-one\">变速箱</th>");
					content.push("<th width=\"12%\" class=\"pd-left-two\">指导价</th>");
					content.push("<th width=\"13%\" class=\"pd-left-three\">参考最低价</th>");
					content.push("<th width=\"21%\"><div class=\"wenh\" onmouseover=\"javascript:$(this).children('.tc-wenh').show();return false;\" onmouseout=\"javascript:$(this).children('.tc-wenh').hide();return false;\"><div class=\"tc tc-wenh\" style=\"display:none;\"><div class=\"tc-box\"><i></i><p>全国参考最低价</p></div></div></div></th></th>");
					content.push("</tr>");
					$.each(data.CarList, function (i, n) {
						content.push("<tr id=\"car_filter_id_" + n.CarID + "\">");
						content.push("<td>");
						var yearType = n.CarYear.length > 0 ? n.CarYear + "款" : "未知年款";
						var strState = n.ProduceState == "停产" ? " <span class=\"tingchan\">停产</span>" : "";
						content.push("<div class=\"pdL10\"><a href=\"/" + allSpell + "/m" + n.CarID + "/\" target=\"_blank\">" + yearType + " " + n.CarName + " </a>" + strState + "</div>");
						content.push("</td>");
						content.push("<td>");
						var percent = data.MaxPv > 0 ? (n.CarPV / data.MaxPv * 100.0) : 0;
						content.push("<div class=\"w\"><div class=\"p\" style=\"width:" + percent + "%\"></div></div>");
						content.push("</td>");
						var gearNum = (n.UnderPan_ForwardGearNum != "" && n.UnderPan_ForwardGearNum != "待查" && n.UnderPan_ForwardGearNum != "无级") ? n.UnderPan_ForwardGearNum + "挡" : ""
						content.push("<td>" + gearNum + n.TransmissionType + "</td>");
						var referPrice = n.ReferPrice.length > 0 ? n.ReferPrice + "万" : "暂无";
						content.push("<td style=\"text-align: right\"><span>" + referPrice + "</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + n.CarID + "\" target=\"_blank\"></a></td>");
						//取最低报价
						var minPrice = n.CarPriceRange;
						if (minPrice.length <= 0)
						{ content.push("<td style=\"text-align: right\"><span>暂无报价</span></td>"); }
						else if (minPrice.indexOf("-") != -1) {
							minPrice = minPrice.substring(0, minPrice.indexOf('-'));
							content.push("<td style=\"text-align: right\"><span><a href=\"/" + allSpell + "/m" + n.CarID + "/baojia/\" target=\"_blank\">" + minPrice + "</a></span></td>");
						} else { content.push("<td style=\"text-align: right\"><span>" + minPrice + "</span></td>"); }
						content.push("<td>");
						content.push("<div class=\"car-summary-btn-xunjia button_gray\"><a href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + n.CarID + "/\" target=\"_blank\">询价</a></div><div class=\"car-summary-btn-duibi button_gray\" id=\"carcompare_btn_new_" + n.CarID + "\"><a target=\"_self\" href=\"javascript:;\" cid=\"" + n.CarID + "\"><span>对比</span></a></div>");
						content.push("</td>");
						content.push("</tr>");
					});
					content.push("</tbody>");
					content.push("</table></div>");
					content.push("<a href=\"javascript:;\" class=\"btn-close\">关闭</a>");
					carSynData[serialId] = content.join('');
					if (IE6 || IE7) {
						$("li[bit-line='" + (line + 1) + "']").hide().html(content.join('')).show().siblings(".c-list-2014.c-list-2014-pop").html('');
					} else {
						$("li[bit-line='" + (line + 1) + "']").hide().html(content.join('')).slideDown(200).siblings(".c-list-2014.c-list-2014-pop").html('');
					}
					$(".btn-close").click(function () { $(this).closest(".c-list-2014.c-list-2014-pop").hide().html(''); self.parent().removeClass("seach_dow"); });
				    // 对比浮动框 初始
					if (WaitCompareObj) {
					    WaitCompareObj.ShowNameForMiniListWaitCompare();
					    WaitCompareObj.AddBtnCompareEvent();
					}
					initCarListHover();
				}
			});
		}
		return false;
	});
	//滑动效果
	function initCarListHover() {
		//车型列表效果
		$('li.c-list-2014.c-list-2014-pop tr').hover(
			function () {
				$(this).addClass('hover-bg-color');
				$(this).find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
				if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
					$(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
			},
			function () {
				$(this).removeClass('hover-bg-color');
				$(this).find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
				if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
					$(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
			}
		);
	}
}

var arrKouBeiDict = { "综合": "Rating", "油耗": "YouHao", "动力": "DongLi", "性价比": "XingJiaBi", "配置": "PeiZhi", "操控": "CaoKong", "空间": "KongJian", "外观": "WaiGuan", "舒适度": "ShuShiDu", "内饰": "NeiShi" };
function getKouBeiItem() {
    var csIds = [];
    $(".seach_more a").each(function () {
        var curSerialId = $(this).attr("bit-serial");
        csIds.push(curSerialId);
    });
    var csParam = csIds.join(',');
    $.ajax({
        url: 'http://api.car.bitauto.com/carinfo/GetSerialInfo.ashx?dept=getcskoubeibaseinfo&csids=' + csParam,
        dataType: "jsonp",
        jsonpCallback: "koubeiCallBack",
        cache: true,
        success: function (json) {
            //根据当前选中项来匹配值
            var curSelectEm = $(".kb-px-box .kb-px-b .current-uparrow").find("em").text() || $(".kb-px-box .kb-px-b .current-downarrow").find("em").text();
            var curSelectEmValue = arrKouBeiDict[curSelectEm];
            $(csIds).each(function (index) {
                var curSerialObj = json[csIds[index]];
                if (curSerialObj && curSerialObj != "undefined") {
                    var curSerialPoint = curSelectEmValue == "Rating" ? curSerialObj.Rating : curSerialObj.Desc[curSelectEmValue];
                    var curPointHtml = [];
                    curPointHtml.push("<div class=\"stars\">");
                    curPointHtml.push("<i>口碑：</i>");
                    curPointHtml.push("<strong>");
                    curPointHtml.push("<span style=\"width:" + (curSerialPoint / 5).toFixed(2).slice(2, 4) + "%;\"></span>");
                    curPointHtml.push("</strong>");
                    curPointHtml.push("<em>" + curSerialPoint + "分</em>");
                    curPointHtml.push("</div>");
                    $("[bit-serial=" + csIds[index] + "]").parent().siblings().filter(".title").after(curPointHtml.join(''));
                }
            })
        }
    });
}


