// 车型频道统计
var Statistic = {
	IsIE: true,
	IsFirstPage: false,
	RefURL: "",
	CurrentURL: "",
	TargetURL: '',
	ScrollDepth: 0,
	RemainTime: 0,
	CurrentPageType: 0,
	UserCode: "",
	UserInfoName: "CarChannelUserInfo",
	SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
	StartTime: 0,
	CityID: 0,
	IP: ""
}

// 主方法
function mainStatisticFunction() {
	// 设置当前毫秒数
	Statistic.StartTime = new Date().getTime();
	// 设置滚动深度
	Statistic.IsIE = CheckBrowserForStatistic();
	Statistic.RefURL = encodeURIComponent(document.referrer.length > 500 ? document.referrer.substring(0, 500) : document.referrer);
	Statistic.CurrentURL = encodeURIComponent(window.location);
	// 检查用户信息
	checkUserInfoForStatistic();
	// 检查用户IP定向城市
	getUserCityForStatistic();
	// 记录页面PV
	var type = Statistic.IsFirstPage ? 1 : 2;
	sentPageViewForStatistic("url", type, "1", Statistic.RefURL, Statistic.CurrentURL, 0);
}

// 检查用户信息
function checkUserInfoForStatistic() {
	var _userinfo = getCookieForStatistic(Statistic.UserInfoName);
	if (_userinfo && _userinfo != '') {
		if (_userinfo.indexOf(';')) {
			var _userinfoArr = new Array();
			_userinfoArr = _userinfo.split(';');
			if (_userinfoArr.length == 3) {
				Statistic.UserCode = _userinfoArr[0];
				Statistic.ScrollDepth = _userinfoArr[1];
				Statistic.RemainTime = _userinfoArr[2];
			}
		}
	}
	if (Statistic.UserCode == '') {
		// 添加用户信息
		Statistic.IsFirstPage = true;
		Statistic.UserCode = generateUserCode();
		setCookieForStatistic(Statistic.UserInfoName, Statistic.UserCode + ";" + Statistic.ScrollDepth + ";" + Statistic.RemainTime);
	}
}

// 取用户IP定向城市
function getUserCityForStatistic() {
	if (typeof (bit_locationInfo) != "undefined") {
		if (typeof (bit_locationInfo.cityId) != "undefined")
		{ Statistic.CityID = bit_locationInfo.cityId; }
		if (typeof (bit_locationInfo.IP) != "undefined")
		{ Statistic.IP = bit_locationInfo.IP; }
	}
}


// 注册页面点击事件
function pageOnClickForStatistic(ev) {
	ev = ev || window.event;
	var _eventSrc = ev.target || ev.srcElement;
	if (_eventSrc && _eventSrc.tagName.toLowerCase() == "a") {
		var _targetURL = _eventSrc.getAttribute('href', 2);
		if (!_targetURL || (_targetURL.toLowerCase().indexOf('javascript:') >= 0) || (_targetURL.toLowerCase().indexOf('#') >= 0)) {
			return;
		}
		// 记录点击取向
		sentPageViewForStatistic("url", 3, 1, Statistic.CurrentURL, encodeURIComponent(_targetURL), 0);
	}
}

// 注册页面滚动事件
function pageOnscrollForStatistic() {
	var diffY = 0;
	if (document.documentElement && document.documentElement.scrollTop)
	{ diffY = document.documentElement.scrollTop; }
	else if (document.body)
	{ diffY = document.body.scrollTop; }
	else
	{ }
	if (Statistic.ScrollDepth < diffY) {
		Statistic.ScrollDepth = diffY;
	}
}


// 注册页面关闭事件
function pageOnunloadForStatistic() {
	// 发送用户滚动深度
	sentUserScrollDepthForStatistic();
	// 发送用户浏览时间
	sentUserRemainTimeForStatistic();
}

// 发送用户滚动深度
function sentUserScrollDepthForStatistic() {
	var depth = parseInt(Statistic.ScrollDepth) + parseInt(document.documentElement.clientHeight);
	sentPageViewForStatistic("page", 0, 1, "", Statistic.CurrentURL, depth);
}

// 发送用户浏览时间
function sentUserRemainTimeForStatistic() {
	var currentTime = new Date().getTime() - Statistic.StartTime;
	if (currentTime > 1000 * 60 * 60 * 24) {
		currentTime = 0;
		return;
	}
	Statistic.RemainTime = Math.ceil(currentTime / 1000);
	if (Statistic.RemainTime > 0) {
		sentPageViewForStatistic("page", 0, 2, "", Statistic.CurrentURL, Statistic.RemainTime);
	}
}

// 发送页面PV
function sentPageViewForStatistic(logtype, type, flag, reURL, taURL, value) {
	var param = "";
	if (logtype == "url") {
		param = "logtype=" + logtype + "&uid=" + Statistic.UserCode + "&refer="
		+ reURL + "&url=" + taURL + "&type=" + type + "&flag=" + flag + "&cityid="
		+ Statistic.CityID + "&ip=" + Statistic.IP + "&" + Math.random();
	}
	else if (logtype == "page") {
		param = "logtype=" + logtype + "&uid=" + Statistic.UserCode + "&url="
		+ taURL + "&type=" + type + "&flag=" + flag + "&value=" + value + "&cityid="
		+ Statistic.CityID + "&ip=" + Statistic.IP + "&" + Math.random();
	}
	else
	{ }
	if (param != "") {
		sentDataForStatistic(param);
	}
}

// 发送数据
function sentDataForStatistic(param) {
	var _sentImg = new Image(1, 1);
	_sentImg.src = Statistic.SentURL + "?" + param;
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
function CheckBrowserForStatistic() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

// 设置Cookie
function setCookieForStatistic(name, value) {
	document.cookie = name + "=" + escape(value) + "; path=/ ; domain=.bitauto.com";
}

// 取Cookie
function getCookieForStatistic(name) {
	var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
	if (_arr != null) {
		return unescape(_arr[2]);
	}
	return null;
}

function attEvent(o, name, fun) {
	return document.all ? o.attachEvent(name, fun) : o.addEventListener(name.substr(2), fun, false);
}

attEvent(window, 'onload', mainStatisticFunction);
attEvent(document, 'onclick', pageOnClickForStatistic);
attEvent(window, 'onbeforeunload', pageOnunloadForStatistic);
attEvent(window, 'onscroll', pageOnscrollForStatistic);
