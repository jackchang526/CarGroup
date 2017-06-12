var loadJS = {
	lock: false,
	ranks: [],
	callback: function (startTime, callback) {
		this.lock = false;
		callback;
		this.read();
	},
	read: function () {
		if (!this.lock && this.ranks.length) {
			var head = document.getElementsByTagName("head")[0] || document.documentElement;
			if (!head) {
				ranks.length = 0, ranks = [];
				throw new Error('HEAD不存在');
			}
			var ranks = this.ranks.shift(), startTime = new Date;
			var wc = this;
			var script = document.createElement('script');
			this.lock = true;
			script.onload = script.onreadystatechange = function () {
				if (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") {
					wc.callback(startTime, ranks.callback);
					startTime = ranks = null;
					script.onload = script.onreadystatechange = null;
					if (head && script.parentNode) {
						head.removeChild(script);
					}
				}
			};
			script.charset = ranks.charset || 'gb2312';
			script.src = ranks.src;
			head.appendChild(script);
		}
	},
	push: function (src, charset, callback) {
		this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
		this.read();
	}
};
var LeftTree = LeftTree || {};
LeftTree.Tools = {
	attrStyle: function (elem, attr) {
		if (!elem) { return; }
		if (elem.style[attr]) {
			return elem.style[attr];
		} else if (elem.currentStyle) {
			return elem.currentStyle[attr];
		} else if (document.defaultView && document.defaultView.getComputedStyle) {
			attr = attr.replace(/([A-Z])/g, '-$1').toLowerCase();
			return document.defaultView.getComputedStyle(elem, null).getPropertyValue(attr);
		} else {
			return null;
		}
	},
	getQueryString: function (data) {
		var tdata = '';
		for (var key in data) {
			tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
		}
		tdata = tdata.replace(/^&/g, "");
		return tdata
	},
	addEvent: function (elm, type, fn, useCapture) {
		if (elm.addEventListener) {
			elm.addEventListener(type, fn, useCapture);
			return true;
		} else if (elm.attachEvent) {
			var r = elm.attachEvent('on' + type, fn);
			return r;
		} else {
			elm['on' + type] = fn;
		}
	},
	MathWinHeigth: function () {
		var winH = 0;
		if (window.innerHeight) {
			winH = Math.min(window.innerHeight, document.documentElement.clientHeight);
		} else if (document.documentElement && document.documentElement.clientHeight) {
			winH = document.documentElement.clientHeight;
		} else if (document.body) {
			winH = document.body.clientHeight;
		}
		return winH;
	},
	getElementByClassName: function (tagName, className) {
		var classObj = document.getElementsByTagName(tagName);
		var len = classObj.length;
		for (var i = 0; i < len; i++) {
			if (classObj[i].className == className) {
				return classObj[i];
				break;
			}
		}
	},
	getOffsetTop: function (element) {
		var top = 0;
		while (element) {
			if (LeftTree.Tools.attrStyle(element, "position") === "fixed") {
				break;
			}
			top += element.offsetTop;
			element = element.offsetParent;
		}
		return top;
	},
	hasClass: function (element, className) {
		var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
		return element.className.match(reg);
	},
	addClass: function (element, className) {
		if (!this.hasClass(element, className)) {
			element.className += " " + className;
		}
	},
	removeClass: function (element, className) {
		if (this.hasClass(element, className)) {
			var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
			element.className = element.className.replace(reg, ' ');
		}
	}
};
if (typeof (params) != "undefined") {
	var args = LeftTree.Tools.getQueryString(params);
	loadJS.push("http://api.car.bitauto.com/Exhibition/GetExhibitionLeftTreeJson.ashx?" + args, "utf-8", JsonpCallBack);
}

var IE6 = !!window.ActiveXObject && !window.XMLHttpRequest;
//计算当前节点到树形顶部距离
function MathCurrentTreeNodeTop(curNode) {
	var topHeight = 0;
	if (!curNode)
		return topHeight;
	while (curNode && curNode.id != "ztreev1") {
		topHeight += curNode.offsetTop;
		curNode = curNode.offsetParent;
	}
	return topHeight;
}
//滚动到指定位置
function scrollToCurrentTreeNode() {
	var currentNode = document.getElementById("curObjTreeNode");
	var topHeight = MathCurrentTreeNodeTop(currentNode);
	var treeBox = document.getElementById("ztreev1"); //树
	treeBox.scrollTop = topHeight;
}
function treeHref(container, curLitterNum) {
	var hideItemAllHeight = 0;
	for (var i = 1; i < curLitterNum; i++) {
		var hideItem = document.getElementById(container + "_" + i);
		if (!hideItem)
			continue;
		var hideItemHeight = hideItem.offsetHeight;
		hideItemAllHeight += hideItemHeight;
	}
	//console.log(hideItemAllHeight);
	var treeBox = document.getElementById(container); //树
	treeBox.scrollTop = hideItemAllHeight;
}
function mathTreeBoxHeight() {
	var topHeight = 0;
	//var tree1Brand = document.getElementById("treev1");
	var treev1 = document.getElementById("ztreev1");
	var tempTreev1;
	//判断标签切换获取元素距顶高度
	if (LeftTree.Tools.attrStyle(document.getElementById("ul_id_1_box_0"), "display") == "block")
		tempTreev1 = document.getElementById("ztreev1"); //树
	else
		tempTreev1 = document.getElementById("treev1"); //树

	if (tempTreev1 == null) return;
	while (tempTreev1) {
		topHeight += tempTreev1.offsetTop;
		tempTreev1 = tempTreev1.offsetParent;
	}
	var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	var winH = LeftTree.Tools.MathWinHeigth();
	//console.log(topHeight);
	if (topHeight < 1) {
		topHeight = 0;
	}
	if (winH <= topHeight) {
		winH = topHeight;
	}
	if (treev1 != null && treev1.nodeType == 1) {
		var tree1Height;
		//如果树形样式为fixed 固定时，设置树高度不加滚动高度
		if (LeftTree.Tools.attrStyle(document.getElementById("leftTreeBox"), "position") == "fixed")
			tree1Height = winH - topHeight;
		else
			tree1Height = winH - topHeight + scrollHeight;
		if (tree1Height < 1) { tree1Height = 0; }
		//console.log(winH + "," + topHeight);
		treev1.style.height = tree1Height + "px";
		//tree1Brand.style.height = tree1Height + "px";
	}
	var leftTreeBox = document.getElementById("leftTreeBox");
	var leftTreeBoxTop = LeftTree.Tools.getOffsetTop(leftTreeBox);
	var leftTreeHeight = winH - leftTreeBoxTop + scrollHeight;
	leftTreeBox.style.height = leftTreeHeight + "px";
}
function mathScorllSettingTagBar() {
	var topHeight = 0;
	var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	var leftTreeBox = document.getElementById("leftTreeBox");
	var tmpLeftTreeBox = LeftTree.Tools.getElementByClassName("div", "nav_swd"); //根据导航设置树形位置
	if (typeof tmpLeftTreeBox == "undefined")
		return;
	var offsetHeight = tmpLeftTreeBox.offsetHeight;
	while (tmpLeftTreeBox) {
		topHeight += tmpLeftTreeBox.offsetTop;
		tmpLeftTreeBox = tmpLeftTreeBox.offsetParent;
	}
	topHeight += offsetHeight; //导航本身高度

	if (LeftTree.Tools.attrStyle(leftTreeBox, "position") == "fixed") {
		//scrollHeight += offsetHeight;
		//leftTreeBox.style.zIndex = "9999";
	}
	//console.log(scrollHeight + "," + topHeight);
	if (scrollHeight > topHeight) {
		leftTreeBox.style.position = "fixed";
		leftTreeBox.style.top = "0px";
	} else {
		leftTreeBox.style.position = "absolute";
		leftTreeBox.style.top = "";
	}
	window.setTimeout(function () { mathTreeBoxHeight(); }, 250);
}
function JsonpCallBack(data) {
	var jsonTree = data;
	var treeHtml = getLeftTreeHtml(jsonTree);
	//console.log(treeHtml);
	var treeBox = document.getElementById("leftTreeBox");
	if (treeBox)
		treeBox.innerHTML = treeHtml;
	//bindChangeTab();
	scrollToCurrentTreeNode();
	LeftTree.Tools.addEvent(window, "load", mathTreeBoxHeight, false);
	LeftTree.Tools.addEvent(window, "resize", mathTreeBoxHeight, false);
	//滚动事件
	LeftTree.Tools.addEvent(window, "scroll", function () {
		if (!IE6) {
			mathScorllSettingTagBar();
		}
		else {
			var leftTreeBox = document.getElementById("leftTreeBox");
			leftTreeBox.style.top = 29 + ie6AdHeight + "px"; //29:登陆条高度
		}
	}, false);
	mathScorllSettingTagBar();
}

function bindChangeTab() {
	try {
		$("#ul_id_1").changeTab("ul_id_1_box");
		$("#ul_id_2").changeTab("ul_id_2_box");
		$("#ul_id_3").changeTab("ul_id_3_box");
		$("#ul_id_4").changeTab("ul_id_4_box");
	} catch (e) { }
}
function getLeftTreeHtml(jsonTree) {
	var treeData;
	var html = "<div class=\"treeTabs treeTabs3\">";
	html += "<div class=\"h-line\">";
	html += "</div>";
	html += "<ul id=\"ul_id_1\">";
	if (typeof params != "undefined" && params.tagtype == "chexing") {
		html += "	<li class=\"current\">按展馆</li>";
		treeData = getPavilionHtml(jsonTree);
	}
	else {
		html += "	<li class=\"current\">按品牌</li>";
		treeData = getBrandTreeHtml(jsonTree);
	}
	html += "</ul>";
	html += "</div>";
	html += "<div style=\"display: block\" id=\"ul_id_1_box_0\">";
	html += treeData;
	html += "</div>";
	//	html += "<div style=\"display: none\" id=\"ul_id_1_box_1\">";
	//	html += getBrandTreeHtml(jsonTree);
	//	html += "</div>";
	return html;
}
function getPavilionNavHtml(pavilionObj) {
	var html = "<div class=\"treeSubNavv1\"><ul class=\"tree_zhanguan\">";
	var i = 0;
	for (var key in pavilionObj) {
		i++;
		html += "<li><a href=\"javascript:void(0);\" onclick=\"treeHref('ztreev1'," + i + ");return false;\">" + pavilionObj[key] + "馆</a></li>";
	}
	html += "</ul></div>";
	return html;
}

function getPavilionHtml(jsonTree) {
	if (typeof jsonTree.pavilion == "undefined")
		return "";
	var html = getPavilionNavHtml(jsonTree.pavilion);
	html += "<div class=\"treev1\" id=\"ztreev1\" style=\"height: 400px;\"><ul>";
	var i = 0;
	for (var pavilionId in jsonTree.pavilion) {
		i++;
		if (jsonTree.pbrand[pavilionId] == undefined)
			continue;
		html += "<li class=\"root\" id=\"ztreev1_" + i + "\"><b>" + jsonTree.pavilion[pavilionId] + "</b>";
		html += "<ul class=\"tree-item-box\">";
		//按字母输出主品牌
		var masterList = jsonTree.pbrand[pavilionId];
		for (var j = 0; j < masterList.length; j++) {
			var className = "mainBrand";
			var currentTreeNodeStr = "";
			if (typeof masterList[j].cur != "undefined" && masterList[j].cur == "1")
				currentTreeNodeStr = "id=\"curObjTreeNode\" class=\"current\""; //增加主品牌定位表示
			html += "<li " + currentTreeNodeStr + "><a href=\"" + masterList[j].url + "\" class=\"" + className + "\"><img src=\"" + masterList[j].logo + "\" /><big>" + masterList[j].name + "</big></a></li>";
		}
		html += "</ul></li>";
	}
	html += "<li id=\"tree-bottom\" style=\"height:300px; overflow:hidden\"></li></ul></div>";
	return html;
}
//获取字符html
function getCharNavHtml(charObj) {
	var html = "<div class=\"treeSubNavv1\"><ul id=\"tree_letter\">";
	var i = 0;
	for (var key in charObj) {
		i++;
		if (charObj[key] == 1)
			html += "<li  class=\"t-" + key.toLowerCase() + "\"><a href=\"javascript:void(0);\" onclick=\"treeHref('ztreev1'," + i + ");return false;\">" + key + "</a></li>";
		else
			html += "<li class=\"none t-" + key.toLowerCase() + "\">" + key + "</li>";
	}
	html += "</ul></div>";
	return html;
}
//获取左侧树html
function getBrandTreeHtml(jsonTree) {
	if (typeof jsonTree.char == "undefined")
		return "";
	var html = getCharNavHtml(jsonTree.char);
	html += "<div class=\"treev1\" id=\"ztreev1\" style=\"height: 400px;\"><ul>";
	var i = 0;
	for (var char in jsonTree.char) {
		i++;
		if (jsonTree.brand[char] == undefined)
			continue;
		html += "<li class=\"root\" id=\"ztreev1_" + i + "\"><b>" + char + "</b>";
		html += "<ul class=\"tree-item-box\">";
		//按字母输出主品牌
		var masterList = jsonTree.brand[char];
		for (var j = 0; j < masterList.length; j++) {
			var className = "mainBrand";
			var currentTreeNodeStr = "";
			if (typeof masterList[j].cur != "undefined" && masterList[j].cur == "1")
				currentTreeNodeStr = "id=\"curObjTreeNode\" class=\"current\""; //增加主品牌定位表示
			html += "<li " + currentTreeNodeStr + "><a href=\"" + masterList[j].url + "\" class=\"" + className + "\"><img src=\"" + masterList[j].logo + "\" /><big>" + masterList[j].name + "</big></a></li>";
		}
		html += "</ul></li>";
	}
	html += "<li id=\"tree-bottom\" style=\"height:300px; overflow:hidden\"></li></ul></div>";
	return html;
}

 