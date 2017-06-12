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
var CookieHelper = {
	trim: function (b) {
		return (b || "").replace(/^\s+|\s+$/g, "")
	},
	setCookie: function (b, d, a) {
		if (typeof d != "undefined") {
			a = a || {};
			if (d === null) {
				d = "";
				a.expires = -1
			}
			var c = "";
			if (a.expires && (typeof a.expires == "number" || a.expires.toUTCString)) {
				if (typeof a.expires == "number") {
					c = new Date;
					c.setTime(c.getTime() + a.expires * 24 * 60 * 60 * 1E3)
				} else c = a.expires;
				c = "; expires=" + c.toUTCString()
			}
			var e = a.path ? "; path=" + a.path : "",
        f = a.domain ? "; domain=" + a.domain : "";
			a = a.secure ? "; secure" : "";
			document.cookie = [b, "=", encodeURIComponent(d), c, e, f, a].join("")
		}
	},
	readCookie: function (b) {
		var d = null;
		if (document.cookie && document.cookie != "") for (var a = document.cookie.split(";"), c = 0; c < a.length; c++) {
			var e = this.trim(a[c]);
			if (e.substring(0, b.length + 1) == b + "=") {
				d = decodeURIComponent(e.substring(b.length + 1));
				break
			}
		}
		return d
	}
}
var LeftTree = LeftTree || {};
LeftTree.Tools = {
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
	}
};
if (typeof (params) != "undefined" && typeof (params.tagtype) != "undefined") {
	var args = LeftTree.Tools.getQueryString(params);
	loadJS.push("http://api.car.bitauto.com/CarInfo/getlefttreejson.ashx?" + args, "utf-8", JsonpCallBack);
}
var IE6 = !!window.ActiveXObject && !window.XMLHttpRequest;

function SetTagUrlCity(tagType, tagUrl, otherPara, allSpell) {
	var cityId = typeof params.cityid == 'undefined' ? "" : params.cityid;
	var cityCode = typeof params.citycode == 'undefined' ? "" : params.citycode;
	var keyWord = typeof params.keyword == 'undefined' ? "" : params.keyword;
	var showType = typeof params.showtype == 'undefined' ? "" : params.showtype;
	var objId = typeof params.objid == 'undefined' ? 0 : params.objid;
	allSpell = typeof allSpell == 'undefined' ? "" : allSpell;
	var tagEle = document.getElementById("treeNav_" + tagType);
	if (tagEle) {
		tagUrl = tagUrl.replace("@objid@", objId);
		tagUrl = tagUrl.replace("@objspell@", allSpell);
		tagUrl = tagUrl.replace("@citycode@", cityCode);
		tagUrl = tagUrl.replace("@cityid@", cityId);
		tagUrl = tagUrl.replace("@keyword@", keyWord);
		if (otherPara != null && otherPara.length > 0 && cityId > 0) {
			tagUrl += otherPara.replace("@cityid@", cityId);
			tagUrl = tagUrl.replace("@citycode@", cityCode);
			tagUrl = tagUrl.replace("@showtype@", showType);
		}
		tagEle.href = tagUrl;
	}
}
function modifyCrumbs(siteName, lastUrl, cityName) {
	var span = document.getElementById("treeCrumbs");
	if (span) {
		var originalHtml = span.innerHTML;
		var index = originalHtml.lastIndexOf('&gt;');
		if (index > 0) {
			var lastText = originalHtml.substring(index + 4);
			originalHtml = originalHtml.substring(0, index + 4);
			var linkHtml = "<a href=\"" + lastUrl + "\">" + lastText + "</a>";
			span.innerHTML = originalHtml + linkHtml + " > " + cityName;

		}
	}
	else {
		var h1s = document.getElementsByTagName("h1");
		if (h1s.length > 0) {
			var h1 = h1s[0];
			var aTag = h1.parentNode;
			var divTag = aTag.parentNode;
			var linkHtml = "<a href=\"" + lastUrl + "\">" + siteName + "</a>";
			var span = document.createElement("span");
			span.setAttribute("id", "treeCrumbs");
			span.setAttribute("class", "treeCrumbs");
			span.innerHTML = linkHtml + " > " + cityName;
			divTag.removeChild(aTag);
			divTag.appendChild(span);

		}
	}
}
//展开或关闭主品牌	
function expandMaster(sender, masterId) {
	if (!sender || masterId <= 0)
		return;
	//alert(sender.tagName + "-" + masterId);
	//查找父级<li>
	var masterRootLi = sender.parentNode;
	while (masterRootLi.tagName != "LI" && masterRootLi != null) {
		masterRootLi = masterRootLi.parentNode;
	}

	if (masterRootLi.tagName != "LI")
		return;

	var contentULs = masterRootLi.getElementsByTagName("UL");
	if (contentULs.length == 0) {
		var cityCode = params.citycode;
		var keyWord = params.keyword;
		//需要获取数据
		var dataUrl = "http://api.car.bitauto.com/CarInfo/getlefttreejson.ashx?tagtype=" + params.tagtype + "&objid=" + masterId + "&expand=1";
		if (cityCode.length > 0)
			dataUrl += "&cityCode=" + cityCode;
		if (keyWord.length > 0)
			dataUrl += "&keyword=" + keyWord;
		loadJS.push(dataUrl, "uft-8", function () { firstExpandMaster(masterRootLi); });
	}
	else {
		//如果已经存在，判断是否展开，如果展开则关闭
		var conUL = contentULs[0];
		if (conUL.style.display == "none")
			conUL.style.display = "block";
		else
			conUL.style.display = "none";
	}

}
function firstExpandMaster(masterRootLi) {
	if (masterContent == 'undefined')
		return;
	if (masterRootLi == null)
		return;
	masterRootLi.innerHTML += getBrandHtml(masterContent, params.tagtaype);
}
function treeHref(curLitterNum) {
	var hideItemAllHeight = 0;
	for (var i = 1; i < curLitterNum; i++) {
		var hideItem = document.getElementById("letter" + i);
		if (!hideItem)
			continue;
		var hideItemHeight = hideItem.offsetHeight - 1;
		hideItemAllHeight += hideItemHeight;
	}
	var treeBox = document.getElementById("treev1"); //树
	treeBox.scrollTop = hideItemAllHeight;
	//tree1高度不够滚动，重新计算高度 by sk 2013.03.20
	if (treeBox.scrollTop < hideItemAllHeight) {
		var treeBottom = document.getElementById("tree-bottom");
		treeBottom.style.height = parseInt(treeBottom.style.height) + (hideItemAllHeight - treeBox.scrollTop) + "px";
		treeBox.scrollTop = hideItemAllHeight;
	}
}

//计算当前节点到树形顶部距离
function MathCurrentTreeNodeTop(curNode) {
	var topHeight = 0;
	if (!curNode)
		return topHeight;
	while (curNode && curNode.id != "treev1") {
		topHeight += curNode.offsetTop;
		curNode = curNode.offsetParent;
	}
	return topHeight;
}
//滚动到指定位置
function scrollToCurrentTreeNode() {
	var currentNode = document.getElementById("curObjTreeNode");

	var topHeight = MathCurrentTreeNodeTop(currentNode);
	var treeBox = document.getElementById("treev1"); //树

	if (typeof CookieHelper == "undefined") {
		treeBox.scrollTop = topHeight;
		return;
	}
	var ScrollTreeNodeTop = CookieHelper.readCookie("ScrollTreeNodeTop");
	var isMaster = 0;
	if (currentNode) {
		var child = currentNode.firstChild;
		if (child.className && child.className.indexOf("mainBrand") != -1) {
			isMaster = 1;
		}
	} else {
		ScrollTreeNodeTop = 0;
	}
	//复制黏贴以及外部点击进来的树形链接
	if (isMaster == 0 && /(mb_|b_|sb_|master|brand|serial)/gi.test(document.referrer) && document.referrer != "") {
		topHeight = parseInt(ScrollTreeNodeTop);
	}
	treeBox.scrollTop = topHeight;
	if (topHeight > parseInt(treeBox.scrollTop)) {
		var treeBottom = document.getElementById("tree-bottom");
		treeBottom.style.height = parseInt(treeBottom.style.height) + (topHeight - parseInt(treeBox.scrollTop)) + "px";
		treeBox.scrollTop = topHeight;
	}
	CookieHelper.setCookie("ScrollTreeNodeTop", parseInt(treeBox.scrollTop), { "expires": 360, "path": "/", "domain": "bitauto.com" });
}
//获取当前节点所在主品牌节点
function GetCurrentNodeMaster(curNode) {
	if (!curNode) {
		return null;
	}
	while (curNode) {
		var child = curNode.firstChild;
		if (child.className && child.className.indexOf("mainBrand") != -1) {
			break;
		}
		curNode = curNode.parentNode;
	}
	return curNode;
}
//计算当前展开容器的高度
function MathCurrentNodeMasterHeight(curNode) {
	var elemHeight = 0;
	if (!curNode) {
		return elemHeight;
	}
	while (curNode) {
		var child = curNode.firstChild;
		if (child.className && child.className.indexOf("mainBrand") != -1) {
			break;
		}
		curNode = curNode.parentNode;
	}
	elemHeight = curNode.offsetHeight;
	return elemHeight;
}
function treeBoxHeight() {

	var treeBox = document.getElementById("treev1"); //树
	var winH;
	var topHeight = 0;

	var bodyClientHeight = document.documentElement.clientHeight;
	var treeSubNavv1Height = LeftTree.Tools.getElementByClassName("div", "treeSubNavv1");
	var IFrameTomHeight = LeftTree.Tools.getElementByClassName("div", "treeNavv1");
	var treev1 = document.getElementById("treev1");
	var isIE6 = !!window.ActiveXObject && !window.XMLHttpRequest;


	if (treeBox == null) {
		return;
	}

	while (treeBox) {
		topHeight += treeBox.offsetTop;
		treeBox = treeBox.offsetParent;
	}
	winH = LeftTree.Tools.MathWinHeigth();

	if (topHeight < 1) {
		topHeight = 0;
	}
	if (winH <= topHeight) {
		winH = topHeight;
	}

	if (treev1 != null && treev1.nodeType == 1) {
		var tree1Height = winH - topHeight;
		if (tree1Height < 1) { tree1Height = 0; }
		treev1.style.height = tree1Height + "px";

	}
	if (treeSubNavv1Height != null && IFrameTomHeight != null && treev1 != null) {
		if (isIE6) {
			if (topHeight > (treeSubNavv1Height.clientHeight + IFrameTomHeight.clientHeight)) {
				if (bodyClientHeight - treeSubNavv1Height.clientHeight - IFrameTomHeight.clientHeight - 10 < 0) {
					treev1.style.height = 0;
				}
				else {
					treev1.style.height = bodyClientHeight - treeSubNavv1Height.clientHeight - IFrameTomHeight.clientHeight - 10;
				}
			}
		}
	}
	//树形高度 by sk 2012.07.16
	var tempBar = LeftTree.Tools.getElementByClassName("div", "bt_smenuNew");
	var leftTreeBox = document.getElementById("leftTreeBox");
	var bt_smenuNewHeight = LeftTree.Tools.getOffsetTop(tempBar);

	//bt_smenuNewHeight += bt_navigateNewHeight;
	var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	var treeFixedNav = document.getElementById("treeFixedNav");
	if (LeftTree.Tools.attrStyle(treeFixedNav, "position") == "fixed") { scrollHeight += treeFixedNavHeight; } //fixed 影响滚动高度
	var leftTreeHeight = winH - bt_smenuNewHeight + scrollHeight;
	leftTreeBox.style.height = leftTreeHeight + "px";
}
var treeFixedNavHeight = parseInt(LeftTree.Tools.attrStyle(document.getElementById("treeFixedNav"), "height")); //36; 导航栏高度
var bt_navigateNewHeight = parseInt(LeftTree.Tools.attrStyle(LeftTree.Tools.getElementByClassName("div", "bt_smenuNew"), "height")) + 5; //39 ;头部图标栏高度
//根据导航栏位置设置树形距顶高度
function treeFixedNavTop() {
	var treeFixedNav = document.getElementById("treeFixedNav");
	var bt_smenuNew = LeftTree.Tools.getElementByClassName("div", "bt_smenuNew");
	var toptreeFixedNavHeight = LeftTree.Tools.getOffsetTop(bt_smenuNew); //获取导航栏距离顶部距离
	if (treeFixedNav.className == "treeNavv1")//判断导航条是否是fixed by songkai 2013.01.21
	{
		document.getElementById("leftTreeBox").style.top = -bt_navigateNewHeight + "px";
		return;
	}
	var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	document.getElementById("leftTreeBox").style.top = toptreeFixedNavHeight - scrollHeight + "px";
}
//计算滚动对导航栏和设置
function MathScrollSettingTagBar() {
	var topHeight = 0;
	var carTagBar = document.getElementById("treeFixedNav");
	var tempBar = LeftTree.Tools.getElementByClassName("div", "bt_smenuNew");
	var leftTreeBox = document.getElementById("leftTreeBox");
	while (tempBar) {
		topHeight += tempBar.offsetTop;
		tempBar = tempBar.offsetParent;
	}
	topHeight += bt_navigateNewHeight;
	var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	var winH = LeftTree.Tools.MathWinHeigth(); //alert(winH);
	//页面较短时 定位跳动 2012.11.05 by sk
	if (LeftTree.Tools.attrStyle(carTagBar, "position") == "fixed") {
		scrollHeight += treeFixedNavHeight;
		carTagBar.style.zIndex = "9999";
	}
	//浮动导航栏
	if (scrollHeight > topHeight) {
		carTagBar.className = "treeNavv1";
	} else {
		carTagBar.className = "treeNavv1-org";
	}
	window.setTimeout(function () { treeBoxHeight(); }, 250);
	//计算树的高度
	treeFixedNavTop();
}
//调整树形导航的位置与树的高度
function treeHeight() {
	if (!IE6) {
		MathScrollSettingTagBar();
	}
	else {
		setTimeout(function () { leftTreeBox.style.top = 29 + ie6AdHeight + "px"; }, 500);
	}
};
var ie6AdHeight = 0;

function JsonpCallBack(data) {
	var jsonTree = data;
	var treeHtml = getTreeHtml(jsonTree);
	//console.log(treeHtml);
	var treeBox = document.getElementById("leftTreeBox");
	if (treeBox)
		treeBox.innerHTML = treeHtml;
	//标签间传递参数
	if (jsonTree.setcityurl != undefined) {
		var setcityurlObj = jsonTree.setcityurl;
		for (var i = 0; i < setcityurlObj.length; i++) {
			SetTagUrlCity(setcityurlObj[i].tagname, setcityurlObj[i].tagurl, setcityurlObj[i].otherpara, setcityurlObj[i].allspell);
		}
	}
	scrollToCurrentTreeNode();
	LeftTree.Tools.addEvent(window, "load", treeBoxHeight, false);
	LeftTree.Tools.addEvent(window, "resize", treeBoxHeight, false);
	//树滚动事件
	var treeBox = document.getElementById("treev1");
	LeftTree.Tools.addEvent(treeBox, "scroll", function () {
		if (typeof CookieHelper == "undefined") return;
		CookieHelper.setCookie("ScrollTreeNodeTop", parseInt(treeBox.scrollTop), { "expires": 360, "path": "/", "domain": "bitauto.com" });
	}, false);
	//滚动事件
	LeftTree.Tools.addEvent(window, "scroll", function () {
		if (!IE6) {
			MathScrollSettingTagBar();
		}
		else {
			var leftTreeBox = document.getElementById("leftTreeBox");
			leftTreeBox.style.top = 29 + ie6AdHeight + "px"; //29:登陆条高度
		}
	}, false);
	treeHeight();
}
//广告调用调整树高度
function pullTopAd(adHeight) {
	var leftTreeBox = document.getElementById("leftTreeBox").offsetTop;
	if (IE6) {
		var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
		leftTreeBox = leftTreeBox - scrollHeight;
		ie6AdHeight = adHeight
	}
	document.getElementById("leftTreeBox").style.top = leftTreeBox + adHeight + "px";
}
function pullFullScreenAd(adHeight) {
	treeFixedNavTop();
	MathScrollSettingTagBar();
	treeHeight();
}
//获取字符html
function getCharNavHtml(charObj) {
	var html = "<div class=\"treeSubNavv1\"><ul id=\"tree_letter\">";
	var i = 0;
	for (var key in charObj) {
		i++;
		if (charObj[key] == 1)
			html += "<li  class=\"t-" + key.toLowerCase() + "\"><a href=\"#\" onclick=\"treeHref(" + i + ");return false;\">" + key + "</a></li>";
		else
			html += "<li class=\"none t-" + key.toLowerCase() + "\">" + key + "</li>";
	}
	html += "</ul></div>";
	return html;
}
//获取主品牌html
function getMasterHtml(masterList) {
	var html = "";
	for (var j = 0; j < masterList.length; j++) {
		var className = "mainBrand";
		var curIdStr = "";
		if (masterList[j].cur == 1) {
			if (masterList[j].type == "mb") {
				className = "mainBrand current current_unfold";
				curIdStr = " id=\"curObjTreeNode\"";
			} else {
				className = "mainBrand current_unfold";
			}
		}
		if (masterList[j].expand != undefined && masterList[j].expand == 1) {
			html += "<li" + curIdStr + "><a href=\"#\" onclick=\"expandMaster(this," + masterList[j].id + ");return false;\" class=\"" + className + "\"><big>" + masterList[j].name + "</big><span>(" + masterList[j].num + ")</span></a>";
		}
		else
			html += "<li" + curIdStr + "><a href=\"" + masterList[j].url + "\" class=\"" + className + "\"><big>" + masterList[j].name + "</big><span>(" + masterList[j].num + ")</span></a>";
		//品牌
		if (masterList[j].child != undefined)
			html += getBrandHtml(masterList[j].child);
		html += "</li>";
	}
	return html;
}
//获取品牌html
function getBrandHtml(brandList) {
	var html = "<ul class=\"tree-items\">";
	for (var k = 0; k < brandList.length; k++) {
		if (brandList[k].type == "cs") {
			html += getSerialHtml(brandList[k]);
		}
		else {
			var className = "brandType";
			var curIdStr = "";
			if (brandList[k].cur == 1) {
				if (brandList[k].type == "cb") {
					className += " current";
					curIdStr = " id=\"curObjTreeNode\"";
				}
			}
			if (brandList.url == "")
				html += "<li" + curIdStr + "><a class=\"" + className + "\"><big>" + brandList[k].name + "</big><span>(" + brandList[k].num + ")</span></a>";
			else
				html += "<li" + curIdStr + "><a href=\"" + brandList[k].url + "\" class=\"" + className + "\"><big>" + brandList[k].name + "</big><span>(" + brandList[k].num + ")</span></a>";
			if (brandList[k].child != undefined) {
				html += "<ul>";
				for (var i = 0; i < brandList[k].child.length; i++)
					html += getSerialHtml(brandList[k].child[i]);
				html += "</ul>";
			}
			html += "</li>";
		}
	}
	html += "</ul>";
	return html;
}
//获取子品牌html
function getSerialHtml(serial) {
	var html = "";
	var className = "subBrand";
	var curIdStr = "";
	if (serial.cur == 1) {
		className += " current";
		curIdStr = " id=\"curObjTreeNode\"";
	}
	if (params.tagtype == "chexing") {
		if (serial.salestate == "停销")
			html += "<li" + curIdStr + " class=\"saleoff\">";
		else
			html += "<li" + curIdStr + ">"
		html += "<a href=\"" + serial.url + "\" class=\"" + className + "\"><big>" + serial.name + "</big>";
		if (serial.butie == 1)
			html += "<span class=\"green\">补贴</span>";
		html += "</a>";
	}
	else if (params.tagtype == "yanghu" || params.tagtype == "zhishu" || params.tagtype == "xiaoliang")
		html += "<li" + curIdStr + "><a href=\"" + serial.url + "\" class=\"" + className + "\"><big>" + serial.name + "</big></a>";
	else
		html += "<li" + curIdStr + "><a href=\"" + serial.url + "\" class=\"" + className + "\"><big>" + serial.name + "</big><span>(" + serial.num + ")</span></a>";
	html += "</li>";
	return html;
}
//获取左侧树html
function getTreeHtml(jsonTree) {
	if (typeof jsonTree["char"] == "undefined")
		return "";
	var html = getCharNavHtml(jsonTree["char"]);
	html += "<div class=\"treev1\" id=\"treev1\"><ul>";
	var i = 0;
	for (var firstChar in jsonTree["char"]) {
		i++;
		if (jsonTree.brand[firstChar] == undefined)
			continue;
		html += "<li class=\"root\" id=\"letter" + i + "\"><b>" + firstChar + "</b>";
		html += "<ul class=\"tree-item-box\">";
		//按字母输出主品牌
		if (typeof jsonTree.brand[firstChar] != "undefined")
			html += getMasterHtml(jsonTree.brand[firstChar]);
		html += "</ul></li>";
	}
	html += "<li id=\"tree-bottom\" style=\"height:300px; overflow:hidden\"></li></ul></div>";
	return html;
}
