// 车型频道统计
var OldPVStatistic = {
	Type: -1, 	// 0:子品牌车型,1:厂商2:主品牌3:品牌4:树形
	IsIE: true,
	RefURL: "",
	CurrentURL: "",
	UserCode: "",
	UserInfoName: "BitAutoUserCode",
	SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
	CityID: 0,
	ID1: 0,
	ID2: 0,
	IP: ""
}

// 主方法
function mainOldPVStatisticFunction() {
	// 设置滚动深度
	OldPVStatistic.IsIE = CheckBrowserForOldPVStatistic();
	OldPVStatistic.RefURL = encodeURIComponent(document.referrer.length > 255 ? document.referrer.substring(0, 255) : document.referrer);
	OldPVStatistic.CurrentURL = encodeURIComponent(window.location.length > 255 ? window.location.substring(0, 255) : window.location);
	// 检查用户信息
	checkUserInfoForOldPVStatistic();
	// 检查用户IP定向城市
	getUserCityForOldPVStatistic();
	// 记录页面PV
	var type = OldPVStatistic.IsFirstPage ? 1 : 2;
	var param = "logtype=autocaroldpv&type=" + OldPVStatistic.Type
	+ "&uid=" + OldPVStatistic.UserCode + "&ref=" + OldPVStatistic.RefURL
	+ "&url=" + OldPVStatistic.CurrentURL + "&id1=" + OldPVStatistic.ID1
	+ "&id2=" + OldPVStatistic.ID2 + "&cityid=" + OldPVStatistic.CityID
	+ "&ip=" + OldPVStatistic.IP + "&" + Math.random();
	sentPageViewForOldPVStatistic("autocaroldpv", param);
}

// 检查用户信息
function checkUserInfoForOldPVStatistic() {
	OldPVStatistic.UserCode = getCookieForOldPVStatistic(OldPVStatistic.UserInfoName);
	if (!OldPVStatistic.UserCode || OldPVStatistic.UserCode == '') {
		// 添加用户信息
		OldPVStatistic.UserCode = generateUserCodeOldPV();
		setCookieForOldPVStatistic(OldPVStatistic.UserInfoName, OldPVStatistic.UserCode);
	}
}

// 取用户IP定向城市
function getUserCityForOldPVStatistic() {
	if (typeof (bit_locationInfo) != "undefined") {
		if (typeof (bit_locationInfo.cityId) != "undefined")
		{OldPVStatistic.CityID = bit_locationInfo.cityId; }
		if (typeof (bit_locationInfo.IP) != "undefined")
		{OldPVStatistic.IP = bit_locationInfo.IP; }
	}
}

// 发送页面PV
function sentPageViewForOldPVStatistic(logtype, param) {
	if (logtype == "autocaroldpv" && param != "") {
		sentDataForOldPVStatistic(param);
	}
}

// 发送数据
function sentDataForOldPVStatistic(param) {
	var _sentImg = new Image(1, 1);
	_sentImg.src = OldPVStatistic.SentURL + "?" + param;
}

// 生成用户ID
function generateUserCodeOldPV() {
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
function CheckBrowserForOldPVStatistic() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

// 设置Cookie
function setCookieForOldPVStatistic(name, value) {
	expiryday = new Date();
	expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
	document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=.bitauto.com";
}

// 取Cookie
function getCookieForOldPVStatistic(name) {
	var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
	if (_arr != null) {
		return unescape(_arr[2]);
	}
	return null;
}

