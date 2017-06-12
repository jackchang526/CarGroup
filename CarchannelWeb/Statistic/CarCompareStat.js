var CarCompareStatistic = {
    IsIE: true,
    UserCode: "",
    UserInfoName: "BitAutoUserCode",
    SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
    CityID: 0,
    CarID: "",
    CarIDs: "",
    CsIDs: "",
    IP: ""
}

function mainCarCompareStatisticFunction() {
    if (CarCompareStatistic.CarIDs != "" && CarCompareStatistic.CarIDs.length > 0) {
        var tempArray = CarCompareStatistic.CarIDs.split(',');
        if (tempArray.length < 2)
        { return; }
        CarCompareStatistic.CarID = tempArray[0];
    }
    else { return; }
    CarCompareStatistic.IsIE = CheckBrowserForCarCompareStatistic();
    checkUserInfoForCarCompareStatistic();
    getUserCityForCarCompareStatistic();
    var param = "logtype=comparecar"
	+ "&UserCode=" + CarCompareStatistic.UserCode
	+ "&car_id=" + CarCompareStatistic.CarID + "&car_ids=" + CarCompareStatistic.CarIDs
	+ "&cityID=" + CarCompareStatistic.CityID
	+ "&userip=" + CarCompareStatistic.IP + "&" + Math.random();
    sentDataForCarCompareStatistic(param);
}

function mainCsCompareStatisticFunction() {
    if (CarCompareStatistic.CsIDs != "" && CarCompareStatistic.CsIDs.length > 0) {
        var tempArray = CarCompareStatistic.CsIDs.split(',');
        if (tempArray.length < 2)
        { return; }
    }
    else { return; }
    CarCompareStatistic.IsIE = CheckBrowserForCarCompareStatistic();
    checkUserInfoForCarCompareStatistic();
    getUserCityForCarCompareStatistic();
    var param = "logtype=comparecs"
	+ "&UserCode=" + CarCompareStatistic.UserCode
	+ "&cs_ids=" + CarCompareStatistic.CsIDs
	+ "&cityID=" + CarCompareStatistic.CityID
	+ "&userip=" + CarCompareStatistic.IP + "&" + Math.random();
    sentDataForCarCompareStatistic(param);
}

function sentDataForCarCompareStatistic(param) {
    var _sentImg = new Image(1, 1);
    _sentImg.src = CarCompareStatistic.SentURL + "?" + param;
}

function getUserCityForCarCompareStatistic() {
    if (typeof (bit_locationInfo) != "undefined") {
        if (typeof (bit_locationInfo.cityId) != "undefined")
        { CarCompareStatistic.CityID = bit_locationInfo.cityId; }
        if (typeof (bit_locationInfo.IP) != "undefined")
        { CarCompareStatistic.IP = bit_locationInfo.IP; }
    }
}

function checkUserInfoForCarCompareStatistic() {
    CarCompareStatistic.UserCode = getCookieForCarCompareStatistic(CarCompareStatistic.UserInfoName);
    if (!CarCompareStatistic.UserCode || CarCompareStatistic.UserCode == '') {
        // 添加用户信息
        CarCompareStatistic.UserCode = generateUserCodeCarCompareStatistic();
        setCookieForCarCompareStatistic(CarCompareStatistic.UserInfoName, CarCompareStatistic.UserCode);
    }
}

// 生成用户ID
function generateUserCodeCarCompareStatistic() {
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
function CheckBrowserForCarCompareStatistic() {
    if (window.ActiveXObject) {
        return true;
    }
    else {
        return false;
    }
}

// 设置Cookie
function setCookieForCarCompareStatistic(name, value) {
    expiryday = new Date();
    expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
    document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=.bitauto.com";
}

// 取Cookie
function getCookieForCarCompareStatistic(name) {
    var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (_arr != null) {
        return unescape(_arr[2]);
    }
    return null;
}