(function () {
	// Array.prototype.unique || (Array.prototype.unique = function() {
	//     this.sort();
	//     var re = [this[0]];
	//     for (var i = 1; i < this.length; i++) {
	//         if (this[i] !== re[re.length - 1]) {
	//             re.push(this[i]);
	//         }
	//     }
	//     return re;
	// });
	var newsVCCount = (function () {
		var s = this;
		var tools = {
			getData: function (e, callbackName, callbackFunc) {
				var t = document.getElementsByTagName("head")[0] || document.documentElement,
                    n = document.createElement("script"),
                    r = false;
				if (callbackName && callbackFunc instanceof Function) window[callbackName] = function (data) { callbackFunc(data); }
				n.src = e,
                    n.charset = "utf-8",
                    n.onerror = n.onload = n.onreadystatechange = function () {
                    	!r && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete") && (r = true, t.removeChild(n));
                    },
                    t.insertBefore(n, t.firstChild);
			},
			// getElementsByClassName: function(className, tag, parent) {
			//     parent = parent || document;
			//     if (!(parent = $(parent))) {
			//         return false;
			//     }

			//     //查找所有匹配的标签  
			//     var allTags = (tag == "*" && parent.all) ? parent.all : parent.getElementsByTagName(tag);
			//     var matchingElements = new Array();

			//     //创建一个正则表达示  
			//     className = className.replace(/\-/g, "\\-");
			//     var regex = new RegExp("(^|\\s)" + className + "(\\s|$)");

			//     var element;
			//     for (var i = 0; i < allTags.length; i++) {
			//         element = allTags[i];
			//         if (regex.test(element.className)) {
			//             matchingElements.push(element);
			//         }
			//     }
			//     return matchingElements;
			// },
			getElementsByAttribute: function (attrName) {
				var allTags = document.getElementsByTagName("*");
				var element, matchingElements = [];
				for (var i = 0; i < allTags.length; i++) {
					element = allTags[i];
					if (element.getAttribute(attrName)) {
						matchingElements.push(element);
					}
				}
				return matchingElements;
			},
			getElementByAttribute: function (attrName, attrVal) {
				var allTags = document.getElementsByTagName("*");
				var resultElement;
				for (var i = 0; i < allTags.length; i++) {
					element = allTags[i];
					if (element.getAttribute(attrName) == attrVal) {
						resultElement = element;
						break;
					}
				}
				return resultElement;
			},
			getIds: function () {
				var ids = [];
				if (typeof jQuery != "undefined") {
					var vnewsid = $("[data-vnewsid]");
					if (vnewsid.length > 0) {
						vnewsid.each(function (index, el) {
							ids.push($(this).data("vnewsid"));
						});
					} else {
						$("[data-cnewsid]").each(function (index, el) {
							ids.push($(this).data("cnewsid"));
						});
					}
				} else {
					var vnewsid = tools.getElementsByAttribute("data-vnewsid");
					if (vnewsid.length > 0) {
						for (var i = vnewsid.length - 1; i >= 0; i--) {
							ids.push(vnewsid[i].getAttribute("data-vnewsid"));
						};
					} else {
						var cnewsid = tools.getElementsByAttribute("data-cnewsid");
						if (cnewsid.length > 0) {
							for (var i = cnewsid.length - 1; i >= 0; i--) {
								ids.push(cnewsid[i].getAttribute("data-cnewsid"));
							};
						}
					}
				}
				return ids.join(',');
			}
		};
		//初始化
		s.initPage = function () {
			var ids = tools.getIds();
			if (ids == "") return;
			var url = "http://api.admin.bitauto.com/news3/v1/news/traffic?callback=getNewsVCCount&ids=" + ids;
			tools.getData(url, "getNewsVCCount", function (data) {
				for (var key in data) {
					if (typeof jQuery != "undefined") {
						$("[data-cnewsid='" + key + "']").html(data[key].comments);
						$("[data-vnewsid='" + key + "']").html(data[key].pv);
					} else {
						var commentElement = tools.getElementByAttribute("data-cnewsid", key);
						if (commentElement) commentElement.innerHTML = data[key].comments;
						var viewElement = tools.getElementByAttribute("data-vnewsid", key);
						if (viewElement) viewElement.innerHTML = data[key].pv;
					}
				}
			});
		};

		s.initPage();
		return s;
	})();
	window.newsView = newsVCCount;
})();
if (typeof (module) !== 'undefined') {
	module.exports = window.newsVCCount;
} else if (typeof define === 'function' && define.amd) {
	define([], function () {
		'use strict';
		return window.newsVCCount;
	});
}