// 车型频道全标签统计脚本
var CarStatForAllTag = {
	IsIE: true,
	CsID: 0,
	CurrentURL: "",
	TagID: 0,
	UserCode: "",
	UserInfoName: "CarStateForBitAuto",
	SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
	CityID: 0,
	IP: ""
}

function intiCarStatForAllTagFunction() {
	CarStatForAllTag.IsIE = CheckBrowserForCarStat();
	CarStatForAllTag.CurrentURL = encodeURIComponent(window.location);
	// 检查用户信息
	checkUserInfoForCarStat();
	// 检查用户IP定向城市
	getUserCityForCarStat();
	sentPageViewForCarStat("alltagurl");
}

// 检查用户信息
function checkUserInfoForCarStat() {
	var _userinfo = getCookieForCarStat(CarStatForAllTag.UserInfoName);
	if (_userinfo && _userinfo != '') {
		CarStatForAllTag.UserCode = _userinfo;
	}
	if (CarStatForAllTag.UserCode == '') {
		// 添加用户信息
		CarStatForAllTag.UserCode = generateUserCode();
		setCookieForCarStat(CarStatForAllTag.UserInfoName, CarStatForAllTag.UserCode);
	}
}

// 取用户IP定向城市
function getUserCityForCarStat() {
	if (typeof (bit_locationInfo) != "undefined") {
		if (typeof (bit_locationInfo.cityId) != "undefined")
		{ CarStatForAllTag.CityID = bit_locationInfo.cityId; }
		if (typeof (bit_locationInfo.IP) != "undefined")
		{ CarStatForAllTag.IP = bit_locationInfo.IP; }
	}
}

// 设置Cookie
function setCookieForCarStat(name, value) {
	var expiryday = new Date();
	expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
	document.cookie = name + "=" + escape(value) + ";path=/;domain=.bitauto.com;expires=" + expiryday.toGMTString();
}

// 取Cookie
function getCookieForCarStat(name) {
	var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
	if (_arr != null) {
		return unescape(_arr[2]);
	}
	return null;
}

// 发送页面PV
function sentPageViewForCarStat(logtype) {
	var param = "logtype=" + logtype;
	if (logtype == "alltagurl") {
		param += "&uid=" + CarStatForAllTag.UserCode + "&url=" + CarStatForAllTag.CurrentURL
		+ "&csid=" + CarStatForAllTag.CsID + "&cityid=" + CarStatForAllTag.CityID + "&tagid="
		+ CarStatForAllTag.TagID + "&ip=" + CarStatForAllTag.IP + "&" + Math.random();
	}
	if (param != "") {
		sentDataForCarStat(param);
	}
}

// 发送数据
function sentDataForCarStat(param) {
	var _sentImg = new Image(1, 1);
	_sentImg.src = CarStatForAllTag.SentURL + "?" + param;
}

// 生成用户ID
function generateUserCode() {
	var _guid = "";
	for (var i = 1; i <= 32; i++) {
		var n = Math.floor(Math.random() * 16.0).toString(16);
		_guid += n;
		if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
		{ _guid += "-"; }
	}
	return _guid;
}

// 检查浏览器
function CheckBrowserForCarStat() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}


var ClassForCarStatForAllTag = {
	create: function() {
		return function() {
			this.initialize.apply(this, arguments);
		}
	}
}

var DelegateCarStatForAllTag = ClassForCarStatForAllTag.create();

DelegateCarStatForAllTag.prototype = {
	initialize: function() {
		this.event = new Array();
	},
	add: function(fun, obj) {
		this.event[this.event.length] = function() {
			fun.apply(obj, arguments);
		};
	},
	exec: function() {
		for (var i = 0; i < this.event.length; i++) {
			this.event[i].apply(null, arguments);
		}
	},
	del: function(num) {
		if (num < this.event.length) {
			this.event.splice(num, 1);
		}
	}
}

window._onload = new DelegateCarStatForAllTag();
window._onload.add(intiCarStatForAllTagFunction, null);