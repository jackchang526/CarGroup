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

//打开更多条件窗口
function popload(i) {
	getElementById(i).style.display = 'block';
	pop_Box = document.createElement("div");
	pop_iframe = document.createElement("iframe");
	document.getElementsByTagName("body")[0].appendChild(pop_Box);
	pop_Box.id = "popBox";
	pop_Box.appendChild(pop_iframe);
	pop_Box.style.display = 'block';
	pop_Box.style.height = Math.max(document.documentElement.scrollHeight, document.documentElement.clientHeight) + 'px';
	pop_Box.style.width = document.documentElement.scrollWidth + 'px';
	getElementById('popWin').style.display = 'block';
	getElementById('popWin').style.top = document.documentElement.scrollTop + document.documentElement.clientHeight / 2 - getElementById('popWin').offsetHeight / 2 + 'px';
	getElementById('popWin').style.left = (document.documentElement.clientWidth / 2 - getElementById('popWin').offsetWidth / 2) + 'px';
}

//关闭更多条件窗口
function closex(i) {
	getElementById(i).style.display = 'none';
	getElementById('popWin').style.display = 'none';
	document.getElementsByTagName("body")[0].removeChild(document.getElementById('popBox'));
}

//打开或收起选车条件
function ShowOrHideSelectCondition() {
	var btnShowHide = getElementById("showhide");
	if (btnShowHide) {
		btnShowHide.onclick = function () {
			btnShowHide.innerHTML = (document.getElementById("showhide").innerHTML == '收起' ? '展开' : '收起');
			btnShowHide.className = (document.getElementById("showhide").className == 'sq' ? 'zk' : 'sq');
			var showHideCon = getElementById("showhideCon");
			if (showHideCon)
				showHideCon.style.display = showHideCon.style.display == 'none' ? '' : 'none';
			var tempNodborder = getElementById("tempNoborder");
			if (tempNodborder) {
				if (btnShowHide.innerHTML == '展开')
					tempNodborder.style.borderBottom = 'none';
				else
					tempNodborder.style.borderBottom = '1px solid #DEE3E7';
			}

			return false;
		}
	}
}

//查询条件对象
var conditionObj =
{
	Price: ""
	, Level: 0					//0不限，1-10对应各级别
	, LevelName: new Array('', '微型车', '小型车', '紧凑型车', '中大型车', '中型车', '豪华车', 'MPV', 'SUV', '跑车', '面包车')		//级别名称
	, Displacement: ""
	, TransmissionType: 0		//0不限，1手动，2自动
	, TransmissionTypeName: { 1: "手动", 62: "自动", 32: "半自动", 2: "自动(AT)", 4: "手自一体(A/MT)", 8: "无级变速(CVT)", 16: "双离合" }
	, MoreCondition: new Array()
	, MoreConditionName: new Array()
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
	//初始化对象
	, InitObject: function () {
		for (i = 0; i < 26; i++)
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
	}
	//初始化页面显示
	, InitPageCondition: function () {
		this.InitPrice();
		this.InitLevel();
		this.InitDisplacement();
		this.InitTransmisstionType();
		this.InitMoreCondition();
		this.InitBrandType();
		this.initSelected();
		this.bindToolSelectCar();
		SetLastSelectCarConditionLink(this.Type, _SearchUrl); //设置最后选车条件
		this.SaveConditionCookie(this.Type, this.Domain); //设置选车条件cookies
	},
	//展开按钮事件
	bindToolSelectCar: function () {
		getElementById("toolBtnControl").onclick = function () {
			var cookiesName, cookiesObj;
			if (typeof CookieHelper != "undefined") {
				cookiesName = conditionObj.GetCookieName(conditionObj.Type);
				cookiesObj = getQueryObject(CookieHelper.readCookie(cookiesName));
			}
			if (conditionObj.toolKey) {
				conditionObj.openToolSelectCar();
				conditionObj.toolKey = false;
				if (typeof CookieHelper != "undefined") {
					cookiesObj["k"] = "1";
					CookieHelper.setCookie(cookiesName, getQueryString(cookiesObj), { "expires": 360, "path": "/", "domain": conditionObj.Domain });
				}
			} else {
				conditionObj.closeToolSelectCar();
				conditionObj.toolKey = true;
				if (typeof CookieHelper != "undefined") {
					cookiesObj["k"] = "0";
					CookieHelper.setCookie(cookiesName, getQueryString(cookiesObj), { "expires": 360, "path": "/", "domain": conditionObj.Domain });
				}
			}
		}
	},
	//初始化选中项
	initSelected: function () {
		var flag = false; //隐藏部分是否有选中项
		var cookiesFlag = "0";
		var cookiesName, cookiesObj;
		if (typeof CookieHelper != "undefined") {
			cookiesName = conditionObj.GetCookieName(conditionObj.Type);
			cookiesObj = getQueryObject(CookieHelper.readCookie(cookiesName));
			cookiesFlag = typeof cookiesObj["k"] === "undefined" ? "0" : cookiesObj["k"];
			//cookiesFlag = CookieHelper.readCookie("select_car_tools");
		}
		var selectUL = document.getElementById("moreConditionUL");
		if (!selectUL) {
			selectUL = document.createElement("ul");
			selectUL.id = "moreConditionUL";
			selectUL.className = "select-list";
		}
		//		//国别
		//		if (this.Brand != 0) {
		//			flag = true;

		//			var liEle = document.createElement("li");
		//			liEle.id = "mcLi" + this.Brand;
		//			liEle.innerHTML = "<a href=\"javascript:DelCondition('" + i + "');\">" + this.GetConditionDescriptionByKeyValue('g', this.Brand) + "</a>";
		//			selectUL.appendChild(liEle);
		//			var mcDiv = getElementById("more-selected");
		//			if (!mcDiv)
		//				return;
		//			mcDiv.appendChild(selectUL);
		//		}
		//变速箱
		if (this.TransmissionType != 0) {
			flag = true;
			var liEle = document.createElement("li");
			liEle.id = "transmission_li_" + this.Brand;
			liEle.innerHTML = "<a href=\"javascript:GotoPage('t0');\">" + this.GetConditionDescriptionByKeyValue('t', this.TransmissionType) + "</a>";
			selectUL.appendChild(liEle);
		}
		//排量
		if (this.Displacement != "") {
			flag = true;
			var liEle = document.createElement("li");
			liEle.id = "displacement_li_" + this.Displacement;
			liEle.innerHTML = "<a href=\"javascript:GotoPage('d');\">" + this.GetConditionDescriptionByKeyValue('d', this.Displacement) + "</a>";
			selectUL.appendChild(liEle);
		}
		//配置
		for (i = 0; i < 26; i++) {
			if (this.MoreCondition[i] == '1') {
				flag = true;
			}
		}
		//车身
		if (this.BodyForm > 0) {
			flag = true;
			var bodyformFlag = false; //是否是2项都选中
			if (this.BodyForm >= 3) {
				bodyformFlag = true;
			}
			for (var key in this.BodyFormName) {
				if (this.BodyForm == key || bodyformFlag) {
					var mcCheckEle = getElementById("bodyform_" + key);
					if (mcCheckEle)
						mcCheckEle.checked = true;
					var liEle = document.createElement("li");
					liEle.id = "bodyform_li_" + key;
					liEle.innerHTML = "<a href=\"javascript:delBodyForm('" + key + "');\">" + this.GetConditionDescriptionByKeyValue('b', key) + "</a>";
					selectUL.appendChild(liEle);
				}
			}
		}
		var more_selected = getElementById("more-selected");
		if (more_selected) {
			more_selected.appendChild(selectUL);
		}
		//车型显示配置
		if (_SearchUrl.indexOf('xuanchegongju') > 0) {
			this.showPeizhi = true;
		}
		//		if (flag) {
		//			this.openToolSelectCar();
		//			conditionObj.toolKey = false;
		//		} else {
		//			this.closeToolSelectCar();
		//			getElementById("toolGuoBie").className += " last";
		//		}
		if (cookiesFlag == "1") {
			this.openToolSelectCar();
			conditionObj.toolKey = false;
		} else {
			this.closeToolSelectCar();
			conditionObj.toolKey = true;
			if (getElementById("moreConditionUL").innerHTML == "") {
				getElementById("toolGuoBie").className += " last";
			}
		}
		if (!this.showPeizhi) getElementById("toolPaiLiang").className += " last";
	},
	//展开选车工具
	openToolSelectCar: function () {
		try {
			getElementById("toolBtnControl").className = "tool-btn-hide";
			getElementById("toolBtnControl").firstChild.innerHTML = "收起条件";
			getElementById("toolGuoBie").className = "";
			getElementById("toolBianSuXiang").style.display = "block";
			getElementById("toolPaiLiang").style.display = "block";
			if (this.showPeizhi) { getElementById("toolPeiZhi").style.display = "block"; }
			getElementById("toolSelected").style.display = "none";
		} catch (err) { }
	},
	//关闭选车工具
	closeToolSelectCar: function () {
		try {
			getElementById("toolBtnControl").className = "tool-btn-show";
			getElementById("toolBtnControl").firstChild.innerHTML = "更多条件";
			getElementById("toolBianSuXiang").style.display = "none";
			getElementById("toolPaiLiang").style.display = "none";
			getElementById("toolPeiZhi").style.display = "none";
			if (getElementById("moreConditionUL").innerHTML != "") {
				getElementById("toolSelected").style.display = "block";
			} else {
				getElementById("toolGuoBie").className += " last";
			}
		} catch (err) { }
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
		//		if (this.TransmissionType >= 2) {
		//			transEle = getElementById("trans62_1");
		//			if (transEle)
		//				transEle.className = "current";
		//		}
		//		this.OpenOrCloseZidongBox();
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
		var mcUL = getElementById("moreConditionUL");
		for (i = 0; i < 26; i++) {
			if (this.MoreCondition[i] == '1') {
				if (!mcUL) {
					var mcDiv = getElementById("more-selected");
					if (!mcDiv)
						return;
					//插入UL节点
					mcUL = document.createElement("ul");
					mcUL.id = "moreConditionUL";
					mcUL.className = "select-list";
					mcDiv.appendChild(mcUL);
				}
				var liEle = document.createElement("li");
				liEle.id = "mcLi" + i.toString();
				liEle.innerHTML = "<a href=\"javascript:DelMoreCondition('" + i + "');\">" + this.MoreConditionName[i] + "</a>";
				mcUL.appendChild(liEle);
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
		if (maxNum > 26)
			maxNum = 26;
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
		return qsArray.join('&');
	}
	//是否有查询条件
	, HasSelectCondition: function () {
		return (this.Price.length > 0 || this.Level != 0
		 || this.Displacement.length > 0
		 || this.TransmissionType != 0
		 || this.MoreCondition.toString().indexOf('1') > -1
		 || this.Brand != 0)
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
		var cookiesObj = getQueryObject(CookieHelper.readCookie(cookiesName));
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
		}

		return valueDes;
	}
}

//初始化条件对象
conditionObj.InitObject();
//获取各个参数对象
function getQueryObject(queryString) {
	var result = {},
		  re = /([^&=]+)=([^&]*)/g, m;
	while (m = re.exec(queryString)) {
		result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
	}
	return result;
}
//根据对象拼参数
function getQueryString(data) {
	var tdata = '';
	for (var key in data) {
		tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
	}
	tdata = tdata.replace(/^&/g, "");
	return tdata
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
//确定设置更多条件
function SetMoreCondition() {
	for (i = 0; i < 26; i++) {
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
	location.href = toUrl;
}

function SelectFilterCondition(filterName) {
	var filterObj;
	var filterValue;
	if (filterName == "brandtype")
		filterObj = getElementById("selBrandType");
	else if (filterName == "bodyform")
		filterObj = getElementById("selBodyForm");
	if (filterObj) {
		filterValue = filterObj.options[filterObj.options.selectedIndex].value;
		if (filterName == "brandtype")
			GotoPage("g" + filterValue);
		else if (filterName == "bodyform")
			GotoPage("b" + filterValue);
	}
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
	var queryStr = getQueryObject(CookieHelper.readCookie(conditionObj.GetCookieName(pType)));
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

//-----------控制城市选择-----------------------------
function addLoadEvent(func) {
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	} else {
		window.onload = function () {
			oldonload();
			func();
		}
	}
}
function addClass(element, value) {
	if (!element.className) {
		element.className = value;
	} else {
		newClassName = element.className;
		newClassName += " ";
		newClassName += value;
		element.className = newClassName;
	}
}

function removeClass(element, value) {
	var removedClass = element.className;
	var pattern = new RegExp("(^| )" + value + "( |$)");
	removedClass = removedClass.replace(pattern, "$1");
	removedClass = removedClass.replace(/ $/, "");
	element.className = removedClass;
	return true;
}

/*=======================tab=============================*/
function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

function tabsRemove(index, head, divs, div2s) {
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			removeClass(alis[i], "current");

			hide(divs + "_" + i);
			if (div2s) { hide(div2s + "_" + i) };

			if (i == index) {
				addClass(alis[i], "current");
			}
		}

		show(divs + "_" + index);
		if (div2s) { show(div2s + "_" + index) };
	}
}

function cartabs(head, divs, div2s, over) {
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);

	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			alis[i].num = i;


			if (over) {
				alis[i].onmouseover = function () {
					var thisobj = this;
					thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
				}
				alis[i].onmouseout = function () {
					clearTimeout(thetabstime);
				}
			}
			else {
				alis[i].onclick = function () {
					if (this.className == "current" || this.className == "last current") {
						changetab(this);
						return true;
					}
					else {
						changetab(this);
						return false;
					}

				}
			}

			function changetab(thebox) {
				tabsRemove(thebox.num, head, divs, div2s);
				if (thebox.id == "btnShowSelectCar" && _SearchUrl)
					SetLastSelectCarConditionLink("hangqing", _SearchUrl);
				else {
					var lastSel = document.getElementById("lastSelectCar");
					if (lastSel)
						lastSel.innerHTML = "";

				}
			}
		}
	}
}


