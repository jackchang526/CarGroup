if (typeof GLOBAL == "undefined") {
    GLOBAL = {}
}
GLOBAL.Namespace = function (str, callFn) {
    var arr = str.split("."),
    o = GLOBAL;
    for (var i = (arr[0] == "GLOBAL") ? 1 : 0; i < arr.length; i++) {
        o[arr[i]] = o[arr[i]] || {};
        o = o[arr[i]];
    }
    var curObj = eval("GLOBAL." + str);
    if (typeof callFn == "function") {
        callFn.call(curObj);
    }
};
GLOBAL.Load = function (l, h, g) {
    var c, b = document.getElementsByTagName("head")[0],
    e = l instanceof Array,
    f = e ? l.length : 0,
    d = 0,
    a = 0;
    h = h ? h : "";
    var k = function (r) {
        var p = r.match(/[a-z0-9A-Z-_]*.js$/) ? "script" : "link",
        o = document.getElementsByTagName(p),
        n = false;
        for (var q = 0,
        m = o.length; q < m; q++) {
            (function (u) {
                var s = o[u],
                t = s.getAttribute("src") || s.getAttribute("href");
                if (t && r.toLowerCase() === t.toLowerCase()) {
                    s.parentNode.removeChild(s);
                    n = true;
                }
            })(q);
            if (n) {
                break;
            }
        }
        return n;
    };
    var i = function (o) {
        k(o);
        c = o.match(/\.js$/) ? document.createElement("script") : document.createElement("link");
        var m = navigator.userAgent.indexOf("MSIE") > -1;
        if (m) {
            c.onreadystatechange = function () {
                if (c.readyState && c.readyState == "loaded" || c.readyState == "complete") {
                    d++;
                    a++;
                    if (e && d <= (f - 1)) {
                        i(l[d]);
                    }
                }
            }
        } else {
            c.onload = function () {
                d++;
                a++;
                if (e && d <= (f - 1)) {
                    i(l[d]);
                }
            }
        }
        if (c.nodeName.toLowerCase() === "script") {
            c.setAttribute("type", "text/javascript");
            c.setAttribute("language", "javascript");
            var n = "?r=" + Math.random();
            if (o.indexOf("http://") > -1) {
                c.setAttribute("src", o + n);
            } else {
                c.setAttribute("src", h + o + n);
            }
        } else {
            c.setAttribute("type", "text/css");
            c.setAttribute("rel", "stylesheet");
            var n = "?r=" + Math.random();
            if (o.indexOf("http://") > -1) {
                c.setAttribute("href", o + n);
            } else {
                c.setAttribute("href", h + o + n);
            }
        }
        b.appendChild(c);
    },
    j = setInterval(function () {
        if (typeof g != "function") {
            clearInterval(j);
            return;
        }
        if ((e && a === f) || (!e && a === 1)) {
            clearInterval(j);
            g();
        }
    },
    1);
    if (e && f > 0) {
        i(l[d]);
    } else {
        i(l);
    }
};