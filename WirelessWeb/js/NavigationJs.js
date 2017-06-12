var CarNavForWireless =
{
	DivID: "m-car-nav", 		// 页面导航元素Div ID
	CurrentTagIndex: -1, 	// 导航当前标签索引(0:综述,1:配置,2:图片,3:油耗,4:详解,5:口碑,6:视频,7:论坛,8:报价,9:二手车)
	GenerateNav: function (data) {
		var tempArray = new Array();
		tempArray.push("<ul>");
		tempArray.push("<li " + (this.CurrentTagIndex == 0 ? "class='current'" : "") + "><a href='" + data.CsSummaryLink + "?ref=Hlht_zx' data-channelid=\"27.23.713\" ><strong>综述" + (this.CurrentTagIndex == 0 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 8 ? "class='current'" : "") + "><a href='" + data.CsBaoJiaLink + "?fc=car&ref=Hlht_bj' data-channelid=\"27.23.714\"><strong>报价" + (this.CurrentTagIndex == 8 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 1 ? "class='current'" : "") + "><a href='" + data.CsPeiZhiLink + "?ref=Hlht_cs' data-channelid=\"27.23.715\"><strong>参数" + (this.CurrentTagIndex == 1 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 2 ? "class='current'" : "") + "><a href='" + data.CsTuPianLink + "?ref=Hlht_tp' data-channelid=\"27.23.716\"><strong>图片" + (this.CurrentTagIndex == 2 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 4 ? "class='current'" : "") + "><a href='" + data.CsXiangJieLink + "?ref=Hlht_wz' data-channelid=\"27.23.717\"><strong>文章" + (this.CurrentTagIndex == 4 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 5 ? "class='current'" : "") + "><a href='" + data.CsKouBeiLink + "?ref=Hlht_kb' data-channelid=\"27.23.718\"><strong>口碑" + (this.CurrentTagIndex == 5 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 3 ? "class='current'" : "") + "><a href='" + data.CsYouHaoLink + "?ref=Hlht_yh' data-channelid=\"27.23.719\"><strong>油耗" + (this.CurrentTagIndex == 3 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 7 ? "class='current'" : "") + "><a href='" + data.CsBBS + "?ref=Hlht_lt' data-channelid=\"27.23.720\"><strong>论坛" + (this.CurrentTagIndex == 7 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 9 ? "class='current'" : "") + "><a href='" + data.CsUsedCarLink + "?ref=Hlht_esc' data-channelid=\"27.23.721\"><strong>二手车" + (this.CurrentTagIndex == 9 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("<li " + (this.CurrentTagIndex == 10 ? "class='current'" : "") + "><a href='" + data.CsYangHuLink + "&ref=Hlht_yh' data-channelid=\"27.23.722\"><strong>养护" + (this.CurrentTagIndex == 10 ? "<s></s>" : "") + "</strong></a></li>");
		tempArray.push("</ul>");
		var divObj = document.getElementById(this.DivID);
		if (divObj) {
			divObj.innerHTML = tempArray.join("");
		}
		try
		{ mainWirelessPVStatisticFunction(data.CsID, 0, this.CurrentTagIndex); }
		catch (err) { }
	}
}

var WirelessPVStatistic = {
	IsIE: true,
	Refer: "",
	Url: "",
	Uid: "",
	UserInfoName: "BitAutoUserCode",
	SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
	Cityid: 0,
	Csid: 0,
	Carid: 0,
	Tag: -1,
	IP: ""
}

// 主方法
function mainWirelessPVStatisticFunction(csid, carid, tagindex) {
	//报价、论坛不统计
	if (tagindex < 0 || tagindex >= 7)
	{ return; }
	if (csid > 0) {
		WirelessPVStatistic.Csid = csid;
	}
	if (tagindex >= 0) {
		WirelessPVStatistic.Tag = tagindex;
	}
	WirelessPVStatistic.IsIE = CheckBrowserForWirelessPVStatistic();
	WirelessPVStatistic.Refer = encodeURIComponent(document.referrer.length > 255 ? document.referrer.substring(0, 255) : document.referrer);
	WirelessPVStatistic.Url = encodeURIComponent(window.location.length > 255 ? window.location.substring(0, 255) : window.location);
	// 检查用户信息
	checkUserInfoForWirelessPVStatistic();
	// 检查用户IP定向城市
	getUserCityForWirelessPVStatistic();
	var param = "logtype=wirelesspv"
	+ "&uid=" + WirelessPVStatistic.Uid + "&refer=" + WirelessPVStatistic.Refer
	+ "&url=" + WirelessPVStatistic.Url + "&csid=" + WirelessPVStatistic.Csid
	+ "&carid=" + WirelessPVStatistic.Carid + "&cityid=" + WirelessPVStatistic.Cityid
	+ "&tag=" + WirelessPVStatistic.Tag
	+ "&ip=" + WirelessPVStatistic.IP + "&" + Math.random();
	sentPageViewForWirelessPVStatistic("wirelesspv", param);
}

// 检查用户信息
function checkUserInfoForWirelessPVStatistic() {
	WirelessPVStatistic.Uid = getCookieForWirelessPVStatistic(WirelessPVStatistic.UserInfoName);
	if (!WirelessPVStatistic.Uid || WirelessPVStatistic.Uid == '') {
		// 添加用户信息
		WirelessPVStatistic.Uid = generateUserCodeWirelessPV();
		setCookieForWirelessPVStatistic(WirelessPVStatistic.UserInfoName, WirelessPVStatistic.Uid);
	}
}

// 取用户IP定向城市
function getUserCityForWirelessPVStatistic() {
	// alert(bit_locationInfo);
	if (typeof (bit_locationInfo) != "undefined") {
		if (typeof (bit_locationInfo.cityId) != "undefined")
		{ WirelessPVStatistic.Cityid = bit_locationInfo.cityId; }
		if (typeof (bit_locationInfo.IP) != "undefined")
		{ WirelessPVStatistic.IP = bit_locationInfo.IP; }
	}
}

// 发送页面PV
function sentPageViewForWirelessPVStatistic(logtype, param) {
	if (logtype == "wirelesspv" && param != "") {
		sentDataForWirelessPVStatistic(param);
	}
}

// 发送数据
function sentDataForWirelessPVStatistic(param) {
	var _sentImg = new Image(1, 1);
	_sentImg.src = WirelessPVStatistic.SentURL + "?" + param;
}

// 生成用户ID
function generateUserCodeWirelessPV() {
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
function CheckBrowserForWirelessPVStatistic() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

// 设置Cookie
function setCookieForWirelessPVStatistic(name, value) {
	expiryday = new Date();
	expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
	document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=.yiche.com";
}

// 取Cookie
function getCookieForWirelessPVStatistic(name) {
	var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
	if (_arr != null) {
		return unescape(_arr[2]);
	}
	return null;
}