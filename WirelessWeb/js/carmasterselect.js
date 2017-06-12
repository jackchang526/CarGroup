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

var CarMasterSelect = {
	masterId: 0,
	masterName: "",
	url: "http://api.car.bitauto.com/CarInfo/SerialListJson.aspx?bsid={bsid}&callback=JsonpCallBack",
	key: false,
	masterAnchor: window.location.hash,
	outclick: null,
	data: {}, //保存请求过的主品牌数据
	//绑定主品牌点击事件
	BindMasterClick: function () {
		var mbLogo = document.getElementById("m-car-logo");
		var liList = mbLogo.getElementsByTagName("li");
		for (var i = 0; i < liList.length; i++) {
			if (liList[i].id && liList[i].id.indexOf("mb_") != -1) {
				liList[i].onclick = this.MasterClickEvent;
			}
		}
		//主品牌锚点 且 展开某主品牌
		if (this.masterAnchor != "" && this.masterAnchor.indexOf("mb_") != -1) {
			var tempAnchorMaster = document.getElementById(this.masterAnchor.replace("#", ""));
			if (tempAnchorMaster != undefined) {
				if (tempAnchorMaster.click)
					tempAnchorMaster.click();
				else {
					try {
						var event = document.createEvent('Event'); //HTMLEvents
						event.initEvent('click', true, true); //事件冒泡
						tempAnchorMaster.dispatchEvent(event);
					}
					catch (e) {
						//alert(e)
					};
				}
			}
		}
	},
	//滚动动画效果
	scrollAnimate: function (currentY, scrollY) {
		var begin = +new Date();
		var from = scrollY;
		var to = currentY;
		var duration = currentY - scrollY;
		var easing = function (time, duration) {
			return -(time /= duration) * (time - 2);
		};
		var timer = setInterval(function () {
			var time = new Date() - begin;
			var pos, now;
			if (time > duration) {
				clearInterval(timer);
				now = to;
			}
			else {
				pos = easing(time, duration);
				now = pos * (to - from) + from;
			}
			if (typeof document.compatMode != 'undefined' && document.compatMode === 'CSS1Compat') {
				document.body.scrollTop = now;
			} else {
				document.documentElement.scrollTop = now;
			}
		}, 20);
	},
	//获取当前元素距顶高度
	getNodeTop: function (curNode) {
		var top = 0;
		if (!curNode)
			return top;
		while (curNode) {
			top += curNode.offsetTop;
			curNode = curNode.offsetParent;
		}
		return top;
	},
	//设置滚动高度
	setPosition: function (currNode) {
		var top = document.body.scrollTop || document.documentElement.scrollTop;
		var height = document.body.scrollHeight || document.documentElement.scrollHeight;
		var currNodeTop = parseInt(this.getNodeTop(currNode)) - 10; //10 是li paddingTop值
		if (top == currNodeTop) return;
		this.scrollAnimate(currNodeTop, top);
		//		if (document.body.scrollTop) {
		//			document.body.scrollTop = currNodeTop;
		//		} else {
		//			document.documentElement.scrollTop = currNodeTop;
		//		}
	},
	//主品牌点击事件
	MasterClickEvent: function () {
		var currNode = this;
		var self = CarMasterSelect;
		var mbId = currNode.id.replace("mb_", "");
		var popupItem = document.getElementById("popupitem" + mbId);
		//二次点击打开关闭
		if (popupItem) {
			var b = currNode.getElementsByTagName("b");
			if (!self.key) {
				popupItem.style.display = "block";
				if (b.length > 0) {
					b[0].style.display = "block";
				}
				self.key = true;
				return;
			} else {
				popupItem.style.display = "none";
				if (b.length > 0) {
					b[0].style.display = "none";
				}
				self.key = false;
				return;
			}
		}
		self.key = true;
		self.ChangeMaster(currNode, false);
		if (self.outclick != null) {
			self.outclick();
			return;
		}
		//如果此主品牌有数据直接加载
		if (self.data[self.masterId]) {
			self.setCallBack(self.data[self.masterId]);
			return;
		}
		var reqUrl = self.url.format({ "bsid": mbId });
		loadJS.push(reqUrl, "utf-8", JsonpCallBack);
	},
	//创建弹出层节点元素
	CreateElementNode: function (loadInfo) {
		//创建元素
		var popupItem = document.createElement("li");
		popupItem.className = "m-popup-item";
		popupItem.id = "popupitem" + this.masterId;

		var popupBox = document.createElement("div");
		popupBox.className = "m-popup-box";
		popupBox.innerHTML = loadInfo;
		popupBox.id = "popupbox" + this.masterId;
		popupItem.appendChild(popupBox);
		return popupItem;
	},
	//改变主品牌弹出层位置
	ChangeMaster: function (currNode, isorientationchange) {
		var loadInfo = "正在加载...";
		var tempCurrNode = currNode;
		if (isorientationchange) {
			if (this.masterId <= 0) return;
			var tempPopupBoxObj = document.getElementById("popupbox" + this.masterId);
			loadInfo = tempPopupBoxObj.innerHTML;
		}
		//删除之前弹出节点
		if (this.masterId > 0) {
			var popupItem = document.getElementById("popupitem" + this.masterId);
			if (popupItem) {
				popupItem.parentNode.removeChild(popupItem);
				var masterLi = document.getElementById("mb_" + this.masterId);
				masterLi.removeChild(masterLi.getElementsByTagName("b")[0]);
			}
		}
		this.setPosition(tempCurrNode); //定位
		this.masterId = tempCurrNode.id.replace("mb_", "");
		this.masterName = tempCurrNode.childNodes[1].innerHTML;
		var popupItem = this.CreateElementNode(loadInfo);
		var b = document.createElement("b");
		tempCurrNode.appendChild(b);
		//转屏显示判断
		if (this.key) {
			popupItem.style.display = "block";
			tempCurrNode.getElementsByTagName("b")[0].style.display = "block";
		}
		else {
			popupItem.style.display = "none";
			tempCurrNode.getElementsByTagName("b")[0].style.display = "none";
		}
		var leftNode = this.getMasterLeft(currNode);
		while (currNode) {
			var tempNode = currNode.nextSibling;
			if (!tempNode) {
				currNode.parentNode.appendChild(popupItem);
				break;
			}
			currNode = tempNode;
			var currLeftNode = this.getMasterLeft(currNode);
			//alert(currLeftNode + "|" + currNode.id);
			if (currLeftNode <= leftNode) {
				var appandNode = currNode.previousSibling;
				appandNode.parentNode.insertBefore(popupItem, appandNode.nextSibling);
				break;
			}
		}
	},
	//获取主品牌居左距离
	getMasterLeft: function (curNode) {
		var left = 0;
		if (!curNode)
			return left;
		while (curNode && curNode.tagName != "UL") {
			left += curNode.offsetLeft;
			curNode = curNode.offsetParent;
		}
		return left;
	},
	//设置回调数据
	setCallBack: function (strHtml) {
		var popupBoxObj = document.getElementById("popupbox" + this.masterId);
		if (popupBoxObj) {
			popupBoxObj.innerHTML = strHtml;
		}
	}
};
//jsonp回调函数
function JsonpCallBack(data) {
	var popupBox = SetJsonToData(CarMasterSelect.masterId, data);
	if (popupBox != "") {
		CarMasterSelect.setCallBack(popupBox);
	}
}

function SetJsonToData(bsid, data) {
	var popupBox = "";
	if (bsid > 0 && data) {
		popupBox += "<div class=\"m-popup m-cars\">";
		for (var i = 0; i < data.length; i++) {
			var brand = data[i];
			popupBox += "<dl>";
			var brandName = brand.Name.replace("进口", "");
			if (data.length == 1 && brandName == CarMasterSelect.masterName) { }
			else
				popupBox += "<dt><a href=\"/" + brand.AllSpell + "/\">" + brand.Name + "</a></dt>";
			for (var j = 0; j < brand.CsList.length; j++) {
				var serial = brand.CsList[j];
				popupBox += "<dd><a href=\"/" + serial.AllSpell + "/\">" + serial.Name + "</a></dd>";
			}
			popupBox += "</dl>";
		}
		popupBox += "<div class=\"clear\"></div>";
		popupBox += "</div>";
		CarMasterSelect.data[bsid] = popupBox;
	}
	return popupBox;
}

//文档加载完成事件
window.onload = function () {
	CarMasterSelect.BindMasterClick();
	// add by chengl inti to 30 mater data
	try {
		if (typeof (masterlist) != "undefined") {
			for (var i = 0; i <= masterlist.length; i++) {
				SetJsonToData(parseInt(masterlist[i].ID), masterlist[i].CBList);
			}
		}
	}
	catch (err) { }
};
//屏幕旋转事件
function orientationChange() {
	var master = document.getElementById("mb_" + CarMasterSelect.masterId);
	switch (window.orientation) {
		case 0:
		case 180:
		case -90:
		case 90:
			if (CarMasterSelect.masterId > 0) {
				CarMasterSelect.ChangeMaster(master, true);
			}
			break;
	}
}
//window.addEventListener("onorientationchange" in window ? "orientationchange" : "resize", orientationChange, false);
window.onorientationchange = orientationChange;

var sWidth = getWinWidth();
function resizePosition() {
	var resizeWidth = getWinWidth();
	if (sWidth != resizeWidth) {
		sWidth = resizeWidth;
		var master = document.getElementById("mb_" + CarMasterSelect.masterId);
		if (CarMasterSelect.masterId > 0) {
			CarMasterSelect.ChangeMaster(master, true);
		}
	}
}
addEvent(window, "resize", resizePosition, false);
String.prototype.format = function () {
	if (arguments.length == 0)
		return this;
	var str = this;
	var obj = arguments[0];
	for (var key in obj) {
		var re = new RegExp('\\{' + key + '\\}', 'gi');
		str = str.replace(re, obj[key]);
	}
	return str;
}
function addEvent(elm, type, fn, useCapture) {
	if (!elm) return;
	if (elm.addEventListener) {
		elm.addEventListener(type, fn, useCapture);
		return true;
	} else if (elm.attachEvent) {
		var r = elm.attachEvent('on' + type, fn);
		return r;
	} else {
		elm['on' + type] = fn;
	}
}
function getWinWidth() {
	var winW = 0;
	if (window.innerHeight) {
		winW = Math.min(window.innerWidth, document.documentElement.clientWidth);
	} else if (document.documentElement && document.documentElement.clientWidth) {
		winW = document.documentElement.clientWidth;
	} else if (document.body) {
		winW = document.body.clientWidth;
	}
	return winW;
}