var CookieHelper = {
    SetCookie: function (name, value, path, secure) {
        expiryday = new Date();
        expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
        document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
			((path) ? "; path=" + path : "; path=/") +
			"; domain=car.bitauto.com" +
			((secure) ? "; secure" : "");
    },
    GetCookie: function (name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    },
    ClearCookie: function (name, path) {
        var obj = this;
        if (obj.GetCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    },
    ClearChildCookie: function (name, path) {
        var obj = this;
        if (obj.GetCookie(name)) {
            document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    }
};