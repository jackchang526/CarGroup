var CarCompareStatisticForWireless = {
    IsIE: true,
    UserCode: "",
    UserInfoName: "BitAutoUserCode",
    SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
    CityID: 0,
    CarID: "",
    CarIDs: "",
    IP: ""
}

function mainCarCompareStatisticForWirelessFunction(carids) {
    if (typeof (carids) != "undefined") {
        CarCompareStatisticForWireless.CarIDs = carids;
    }
    else { return;}
    if (CarCompareStatisticForWireless.CarIDs != "" && CarCompareStatisticForWireless.CarIDs.length > 0) {
        var tempArray = CarCompareStatisticForWireless.CarIDs.split(',');
        if (tempArray.length < 2)
        { return; }
        CarCompareStatisticForWireless.CarID = tempArray[0];
    }
    else { return; }
    // 设置滚动深度
    CarCompareStatisticForWireless.IsIE = CheckBrowserForCarCompareStatisticForWireless();
    // 检查用户信息
    checkUserInfoForCarCompareStatisticForWireless();
    // 检查用户IP定向城市
    getUserCityForCarCompareStatisticForWireless();
    var param = "logtype=comparecarwireless"
	+ "&UserCode=" + CarCompareStatisticForWireless.UserCode
	+ "&car_id=" + CarCompareStatisticForWireless.CarID + "&car_ids=" + CarCompareStatisticForWireless.CarIDs
	+ "&cityID=" + CarCompareStatisticForWireless.CityID
	+ "&userip=" + CarCompareStatisticForWireless.IP + "&" + Math.random();
    sentDataForCarCompareStatisticForWireless(param);
}

// 发送数据
function sentDataForCarCompareStatisticForWireless(param) {
    var _sentImg = new Image(1, 1);
    _sentImg.src = CarCompareStatisticForWireless.SentURL + "?" + param;
}


// 取用户IP定向城市
function getUserCityForCarCompareStatisticForWireless() {
    if (typeof (bit_locationInfo) != "undefined") {
        if (typeof (bit_locationInfo.cityId) != "undefined")
        { CarCompareStatisticForWireless.CityID = bit_locationInfo.cityId; }
        if (typeof (bit_locationInfo.IP) != "undefined")
        { CarCompareStatisticForWireless.IP = bit_locationInfo.IP; }
    }
}

// 检查用户信息
function checkUserInfoForCarCompareStatisticForWireless() {
    CarCompareStatisticForWireless.UserCode = getCookieForCarCompareStatisticForWireless(CarCompareStatisticForWireless.UserInfoName);
    if (!CarCompareStatisticForWireless.UserCode || CarCompareStatisticForWireless.UserCode == '') {
        // 添加用户信息
        CarCompareStatisticForWireless.UserCode = generateUserCodeCarCompareStatisticForWireless();
        setCookieForCarCompareStatisticForWireless(CarCompareStatisticForWireless.UserInfoName, CarCompareStatisticForWireless.UserCode);
    }
}

// 生成用户ID
function generateUserCodeCarCompareStatisticForWireless() {
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
function CheckBrowserForCarCompareStatisticForWireless() {
    if (window.ActiveXObject) {
        return true;
    }
    else {
        return false;
    }
}

// 设置Cookie
function setCookieForCarCompareStatisticForWireless(name, value) {
    expiryday = new Date();
    expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
    document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=.yiche.com";
}

// 取Cookie
function getCookieForCarCompareStatisticForWireless(name) {
    var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (_arr != null) {
        return unescape(_arr[2]);
    }
    return null;
}