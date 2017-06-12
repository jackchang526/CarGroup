Array.prototype.indexOf = function(value) {
    for (var i = 0, l = this.length; i < l; i++)
        if (this[i] == value) return i;
    return -1;
}
Array.prototype.remove = function(b) {
    var a = this.indexOf(b);
    if (a >= 0) {
        this.splice(a, 1);
        return true;
    }
    return false;
};

var WaitCompare = (function(module) {
    var self = module,
        compareData = [],
        defaults = {
            count: 4,
            cookieName: "m_comparecarlist",
            url: "/handlers/getcarinfoforcompare.ashx?carid="
        };
    //添加对比
    module.addCompareCar = function(id, name) {
        var cookieCar = CookieHelper.getCookie(defaults.cookieName),
            arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
        }
        if (arrCarId.length >= 4) {
            //最懂4款
        }
        if (arrCarId.indexOf(id) != -1) {
            //已存在
        }

        arrCarId.push(id);

        CookieHelper.setCookie(defaults.cookieName, arrCarId.join('|'));
        compareData.push({
            CarId: id,
            CarName: name
        });
        drawUI(compareData);
    };

    //清空对比
    module.clearCompareCarAll = function() {
        CookieHelper.clearCookie(defaults.cookieName);
        compareData = [];
        drawUI(compareData);
    };
    //删除对比车款
    module.delCompareCar = function(carId) {
        var cookieCar = CookieHelper.getCookie(defaults.cookieName),
            arrCarId = [],
            newCompareData = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
        }

        arrCarId.remove(carId);

        CookieHelper.setCookie(defaults.cookieName, arrCarId.join('|'));

        for (var i = 0; i < compareData.length; i++) {
            if (compareData[i].CarId == carId) continue;
            newCompareData.push(compareData[i]);

        };
        compareData = newCompareData;
        drawUI(newCompareData);
    };
    //开始对比
    module.submitCompare = function() {
            var cookieCar = CookieHelper.getCookie(defaults.cookieName),
                arrCarId = [];
            if (cookieCar) {
                arrCarId = cookieCar.split('|');
            }
            if (arrCarId.length <= 1) {
                //请添加车款
            }
            location.href = '/chexingduibi/?carid=' + arrCarId.join(',');
        }
        //初始化 对比数据
    module.initCompreData = function() {
        // var cookieCar = CookieHelper.getCookie(defaults.cookieName),
        //     arrCarId = [];
        // if (cookieCar) {
        //     arrCarId = cookieCar.split('|');
        // }
        // if (arrCarId.length > 0) {
        //     getData(defaults.url + arrCarId.join(','), function(data) {
        //         drawUI(data);
        //     });
        // } else {
        //     drawUI();
        // }
        // rightSwipe();
    };
    //组织DOM
    var drawUI = function(data) {
        var h = [],
            currentCount = 0;
        // if(!(data&&data.length>0)) return;
        for (var i = 0; i < (data && data.length); i++) {

            compareData.push({
                CarId: data[i].CarId,
                CarName: data[i].CarName
            });

            h.push("<li><div class=\"line-box\"><a href=\"/m" + data[i].CarId + "/\">" + data[i].CarName + "</a><a href=\"javascript:;\" data-carid=\"" + data[i].CarId + "\" class=\"btn-close\"><i></i></a></div></li>");
        }
        for (var j = 0; i < defaults.count - i; j++) {
            h.push("<li class=\"add\"><div class=\"line-box\"><a href=\"javascript:;\">添加对比车款</a></div></li>");
        }
        $(".first-list").html(h.join(''));

        bindEvent();
    };
    //绑定事件
    var bindEvent = function() {

        $(".first-list .add a").on("click", function() {

        });

        $(".first-list li .btn-close").on("click", function() {
            var carId = $(this).data("carid");
            self.delCompareCar(carId);
            $(this).closest("li").hide();
        });
    };

    // var getQueryObject = function(queryString) {
    //     var result = {},
    //         re = /([^&=]+)=([^&]*)/g,
    //         m;
    //     while (m = re.exec(queryString)) {
    //         result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
    //     }
    //     return result;
    // };
    // var getQueryString = function(data) {
    //         var tdata = '';
    //         for (var key in data) {
    //             tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
    //         }
    //         tdata = tdata.replace(/^&/g, "");
    //         return tdata
    //     };

    //右侧弹出效果
    var rightSwipe = function() {
        var $car = $('[data-action=car]');
        $car.rightSwipe({
            clickEnd: function(b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'))
                    $back.touches({
                        touchstart: function(ev) {
                            ev.preventDefault();
                        },
                        touchmove: function(ev) {
                            ev.preventDefault();
                        }
                    });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');
                    $swipeLeft.touches({
                        touchstart: function(ev) {
                            ev.preventDefault();
                        }
                    });
                    $car.$y2015 = $swipeLeft.find('.y2015-car-01');
                    $car.$y2015.height(document.documentElement.clientHeight - 50);
                    $swipeLeft.find('.swipeLeft-header').show();
                }
            }
        })
    };

    var getData = function(url, callbackFunc, sync) {
        $.ajax({
            url: url,
            cache: true,
            async: (sync ? false : true),
            dataType: "jsonp",
            jsonpCallback: "getData",
            success: function(data) {
                callbackFunc(data);
            }
        });
    };

    return module;
})(WaitCompare || {});

Object.extend = function(destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
}

var CookieHelper = (function(module) {
    var self = module,
        defaults = {
            //domoin: "car.m.yiche.com",
            expires: 30 * 30,
            path: "/"
        };
    // module.setCookie = function(name, value, expires, path, domain, secure) {
    //     expiryday = new Date();
    //     expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
    //     document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
    //         ((path) ? "; path=" + path : "; path=/") +
    //         "; domain=" + defaults.domoin + "" +
    //         ((secure) ? "; secure" : "");
    // };
    module.setCookie = function(name, value, options) {
        Object.extend(defaults, options);

        if (typeof value != 'undefined') { // name and value given, set cookie
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (defaults.expires && (typeof defaults.expires == 'number' || defaults.expires.toUTCString)) {
                var date;
                if (typeof defaults.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (defaults.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = defaults.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = defaults.path ? '; path=' + defaults.path : '';
            var domain = defaults.domain ? '; domain=' + defaults.domain : '';
            var secure = defaults.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        }
    };
    module.getCookie = function(name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    };
    module.clearCookie = function(name, path, domain) {
        if (self.getCookie(name)) {
            document.cookie = name + "=" +
                ((path) ? "; path=" + path : "; path=/") +
                "; domain=" + defaults.domoin + "" +
                ";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    }
    return module;
})(CookieHelper || {});
