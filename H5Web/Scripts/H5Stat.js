var H5Statistic = {
    IsIE: true,
    URL: "",
    UserCode: "",
    UserInfoName: "BitAutoUserCode",
    SentURL: "http://carstat.bitauto.com/weblogger/img/c.gif",
    CityID: 0,
    CsID: 0,
    IP: "",
    Eventid: 0,
    Strtemp: "",
    UrlCode: "",
    IsWX: false,
    IsHost: false,
    IsNeedShare: false
}

function checkIsWeixn() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}

function checkUserInfoForH5Statistic() {
    H5Statistic.UserCode = getCookieForH5Statistic(H5Statistic.UserInfoName);
    if (!H5Statistic.UserCode || H5Statistic.UserCode == '') {
        // 添加用户信息
        H5Statistic.UserCode = generateUserCodeH5Statistic();
        setCookieForH5Statistic(H5Statistic.UserInfoName, H5Statistic.UserCode);
    }
}

function getUserCityForH5Statistic() {
    if (typeof (bit_locationInfo) != "undefined") {
        if (typeof (bit_locationInfo.cityId) != "undefined")
        { H5Statistic.CityID = bit_locationInfo.cityId; }
        if (typeof (bit_locationInfo.IP) != "undefined")
        { H5Statistic.IP = bit_locationInfo.IP; }
    }
}

function sentDataForH5Statistic(param) {
    if (H5Statistic.IsHost) {
        var _sentImg = new Image(1, 1);
        _sentImg.src = H5Statistic.SentURL + "?" + param;
    }
}

function sentEventForH5Statistic(eventid, strtemp) {
    var param = "logtype=h5eventlog"
	+ "&uid=" + H5Statistic.UserCode + "&url=" + H5Statistic.URL + "&csid=" + H5Statistic.CsID
	+ "&cityid=" + H5Statistic.CityID + "&ip=" + H5Statistic.IP + "&eventid=" + eventid + "&strtemp=" + encodeURIComponent(strtemp)
    + "&" + Math.random();
    sentDataForH5Statistic(param);
}

function H5StatisticForShare(eventid)
{
    if (typeof (summary) != "undefined" && typeof (summary.IsUserEdition) != "undefined" && summary.IsUserEdition) {
        var _sentImgShare = new Image(1, 1);
        _sentImgShare.src = "http://bglog.bitauto.com/bglogh5.gif?url=" + H5Statistic.URL + "&shareId=" + eventid + "&" + Math.random();
    }
}

function generateUserCodeH5Statistic() {
    var _guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        _guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
        { _guid += "-"; }
    }
    return _guid;
}

function CheckBrowserForH5Statistic() {
    if (window.ActiveXObject) {
        return true;
    }
    else {
        return false;
    }
}

function setCookieForH5Statistic(name, value) {
    expiryday = new Date();
    expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
    document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=.bitauto.com";
}

function getCookieForH5Statistic(name) {
    var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (_arr != null) {
        return unescape(_arr[2]);
    }
    return null;
}

function readyAndConfigWeixin() {
    if (forweixinObj && (typeof wx == 'object')) {
        wx.config({
            debug: forweixinObj.debug,
            appId: forweixinObj.appId,
            timestamp: forweixinObj.timestamp,
            nonceStr: forweixinObj.nonceStr,
            signature: forweixinObj.signature,
            jsApiList: forweixinObj.jsApiList
        });
        wx.ready(function () {
            wx.checkJsApi({
                jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo'],
                success: function (res) {
                }
            });
            wx.onMenuShareAppMessage({
                title: pageShareContent.title,
                desc: pageShareContent.desc,
                link: pageShareContent.link + '&ref=wxpengyou&WT.mc_id=mtxwxddh5',
                imgUrl: pageShareContent.imgUrl,
                trigger: function (res) {
                    sentEventForH5Statistic(10, '用户点击发送给朋友');
                },
                success: function (res) {
                    sentEventForH5Statistic(11, '已分享');
                    H5StatisticForShare(1);
                },
                cancel: function (res) {
                    sentEventForH5Statistic(12, '已取消');
                },
                fail: function (res) {
                    sentEventForH5Statistic(13, JSON.stringify(res));
                }
            });

            wx.onMenuShareTimeline({
                title: pageShareContent.title,
                link: pageShareContent.link + '&ref=wxpengyouquan&WT.mc_id=mtxwxpyh5',
                imgUrl: pageShareContent.imgUrl,
                trigger: function (res) {
                    sentEventForH5Statistic(20, '用户点击分享到朋友圈');
                },
                success: function (res) {
                    sentEventForH5Statistic(21, '已分享');
                    H5StatisticForShare(2);
                },
                cancel: function (res) {
                    sentEventForH5Statistic(22, '已取消');
                },
                fail: function (res) {
                    sentEventForH5Statistic(23, JSON.stringify(res));
                }
            });

            wx.onMenuShareQQ({
                title: pageShareContent.title,
                desc: pageShareContent.desc,
                link: pageShareContent.link + '&ref=wxqq&WT.mc_id=mtxqqh5',
                imgUrl: pageShareContent.imgUrl,
                trigger: function (res) {
                    sentEventForH5Statistic(30, '用户点击分享到QQ');
                },
                complete: function (res) {
                },
                success: function (res) {
                    sentEventForH5Statistic(31, '已分享');
                    H5StatisticForShare(3);
                },
                cancel: function (res) {
                    sentEventForH5Statistic(32, '已取消');
                },
                fail: function (res) {
                    sentEventForH5Statistic(33, JSON.stringify(res));
                }
            });

            wx.onMenuShareWeibo({
                title: pageShareContent.title,
                desc: pageShareContent.desc,
                link: pageShareContent.link + '&ref=wxweibo&WT.mc_id=mtxwbh5',
                imgUrl: pageShareContent.imgUrl,
                trigger: function (res) {
                    sentEventForH5Statistic(40, '用户点击分享到微博');
                },
                complete: function (res) {
                },
                success: function (res) {
                    sentEventForH5Statistic(41, '已分享');
                    H5StatisticForShare(4);
                },
                cancel: function (res) {
                    sentEventForH5Statistic(42, '已取消');
                },
                fail: function (res) {
                    sentEventForH5Statistic(43, JSON.stringify(res));
                }
            });
        });

        wx.error(function (res) {
            sentEventForH5Statistic(9, res.errMsg);
        });
    }
}

(function intiAndH5StatisticFunction() {
    var hostName = window.location.hostname.toLowerCase();
    var allowArray = ['car.h5.yiche.com', 'news.h5.yiche.com'];
    if (allowArray.join(",").indexOf(hostName) >= 0)
    { H5Statistic.IsHost = true; }
    H5Statistic.IsWX = checkIsWeixn();
    if (typeof (summary.serialId) != "undefined")
    { H5Statistic.CsID = summary.serialId; }
    if (typeof (summary.IsNeedShare) != "undefined")
    { H5Statistic.IsNeedShare = summary.IsNeedShare; }
    H5Statistic.UrlCode = "http://" + window.location.hostname + window.location.pathname + window.location.search;
    H5Statistic.IsIE = CheckBrowserForH5Statistic();
    H5Statistic.URL = encodeURIComponent(window.location.length > 255 ? window.location.substring(0, 255) : window.location);
    // 检查用户信息
    checkUserInfoForH5Statistic();
    // 检查用户IP定向城市
    getUserCityForH5Statistic();
    var param = "logtype=h5pvlog"
	+ "&uid=" + H5Statistic.UserCode + "&url=" + H5Statistic.URL + "&csid=" + H5Statistic.CsID + "&ua=" + (H5Statistic.IsWX ? "weixin" : "")
	+ "&cityid=" + H5Statistic.CityID + "&ip=" + H5Statistic.IP + "&" + Math.random();
    sentDataForH5Statistic(param);
})();

(function () {
    if (H5Statistic.IsWX && H5Statistic.IsHost && H5Statistic.IsNeedShare) {
        $.ajax({
            type: 'get',
            url: 'http://api.car.bitauto.com/Cooperation/WeiXinConfig.ashx',
            cache: false,
            dataType: 'jsonp',
            data: { url: H5Statistic.UrlCode },
            jsonpCallback: 'funcweixin',
            success: function (data) {
                if (typeof forweixinObj == 'undefined')
                { window.forweixinObj = {}; }
                forweixinObj.timestamp = data["yiche.timestamp"];
                forweixinObj.nonceStr = data["yiche.nonceStr"];
                forweixinObj.signature = data["yiche.signature"];
                readyAndConfigWeixin();
            }
        });
    }
})();