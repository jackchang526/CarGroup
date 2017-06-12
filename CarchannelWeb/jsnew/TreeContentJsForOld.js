/* 老版本for 树形合作站 */
var DomHelper = {
	cancelClick: function (e) {
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
	addEvent: function (elm, evType, fn, useCapture) {
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
	removeEvent: function (elm, evType, fn, useCapture) {
		if (elm.removeEventLister)
			elm.removeEventLister(evType, fn, useCapture);
		else if (elm.detachEvent)
			elm.detachEvent('on' + evType, fn);
	}
}
var FrameJudge = {
	copyLink: "copylink",
	topId: "topContents",
	init: function (url) {
		if (self == top) {
			url = FrameJudge.trim(url);
			var urlreffer = FrameJudge.AddReffer();
			if (urlreffer != "") {
				if (url.indexOf("?") > -1) url += "&" + urlreffer;
				else url += "?" + urlreffer;
			}
			window.location.replace(url);
			return;
		}
	},
	copyToClipboard: function () {
		if (window.clipboardData) {
			window.clipboardData.clearData();
			if (window.clipboardData.setData("Text", window.location.href)) {
				alert("复制成功！");
			}
			else {
				alert("您点击不允许复制链接，如果想重新复制，请刷新页面重试！");
			}
		} else if (navigator.userAgent.indexOf("Opera") != -1) {
			window.location = window.location.href;
		} else if (window.netscape) {
			try {
				netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
			} catch (e) {
				alert("被浏览器拒绝！\n请在浏览器地址栏输入'about:config'并回车\n然后将'signed.applets.codebase_principal_support'设置为'true'");
			}
			var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
			if (!clip)
				return;
			var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
			if (!trans)
				return;
			trans.addDataFlavor('text/unicode');
			var str = new Object();
			var len = new Object();
			var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
			var copytext = window.location.href;
			str.data = copytext;
			trans.setTransferData("text/unicode", str, copytext.length * 2);
			var clipid = Components.interfaces.nsIClipboard;
			if (!clip)
				return false;
			clip.setData(trans, null, clipid.kGlobalClipboard);
			alert("复制成功！");
		}
	},
	load: function () {
		if (!document.getElementById || !document.createTextNode) {
			return;
		}

		var copyObject = document.getElementById(FrameJudge.copyLink);
		if (copyObject == null
            || copyObject.nodeType != 1) {
			return;
		}
		if (self != top && parent.window.frames[FrameJudge.topId] != null) {
			parent.document.title = document.title;
		}
		DomHelper.addEvent(copyObject, "click", FrameJudge.copyToClipboard, false);
	},
	AddReffer: function () {
		var pattern = new RegExp("http://.*\.(google|baidu)\.(com|cn)\/.*");
		var urlReffer = document.referrer;
		if (urlReffer == null || urlReffer == "" || !pattern.test(urlReffer))
			return "";
		return "reffer=" + urlReffer;
	},
	//截取字符串的空格符
	trim: function (value) {
		return (value || "").replace(/^\s+|\s+$/g, "");
	},
	//读取cookie
	readCookie: function (name, value, options) {
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
		} else { // only name given, get cookie
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
}
if (self != top && parent.window.frames[FrameJudge.topId] != null) {
	if (typeof parent.TreeTag != 'undefined') parent.TreeTag.settingDelearTag();
}
DomHelper.addEvent(window, "load", FrameJudge.load, false);
