var DomHelper = {
    cancelClick: function(e) {
        if (window.event && window.event.cancelBubble
            && window.event.returnValue) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
            return;
        }
        if (e && e.stopPropagation && e.preventDefault) {
            e.stopPropagetion();
            e.preventDefault();
        }
    },
    addEvent: function(elm, evType, fn, useCapture) {
        if (elm.addEventListener) {
            elm.addEventListener(evType, fn, useCapture);
            return true;
        }
        else if (elm.attachEvent) {
            var r = elm.attachEvent('on' + evType, fn);
            return r;
        }
        else {
            elm['on' + evType] = fn;
        }
    },
    removeEvent: function(elm, evType, fn, useCapture) {
        if (elm.removeEventLister)
            elm.removeEventLister(evType, fn, useCapture);
        else if (elm.detachEvent)
            elm.detachEvent('on' + evType, fn);
    }
}

//cookie 操作
var CookieHelper = {
    //截取字符串的空格符
    trim: function (value)
    {
      	return (value || "").replace(/^\s+|\s+$/g, "");
      },
	//设置cookie
    setCookie: function(name, value, options) {
        if (typeof value != 'undefined') { // name and value given, set cookie
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
                var date;
                if (typeof options.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = options.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = options.path ? '; path=' + options.path : '';
            var domain = options.domain ? '; domain=' + options.domain : '';
            var secure = options.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        }
	} ,
	// get cookie
	readCookie: function(name)
	{
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = this.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
       }
	}



