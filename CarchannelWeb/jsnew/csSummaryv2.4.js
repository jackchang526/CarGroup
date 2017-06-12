// JScript 文件

function addLoadEvent(func) {
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	}
	else {
		window.onload = function () {
			oldonload();
			func();
		}
	}
}

// 设置Cookie
function setCookieForSummary(name, value) {
	expiryday = new Date();
	expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
	document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + "; path=/ ; domain=car.bitauto.com";
}

// 取Cookie
function getCookieForSummary(name) {
	var _arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
	if (_arr != null) {
		return unescape(_arr[2]);
	}
	return null;
}

//显示经销商信息
function ShowVendorInfo(serialId, cityId) {
	var url = "http://api.car.bitauto.com/carinfo/iframefordealer.aspx?csId=" + serialId + "&cityId=" + cityId + "";
	loadJS.push(url, "utf-8", function () {
		var vendor = document.getElementById("vendorInfo");
		if (typeof dealerHtml == 'undefined' || dealerHtml == null || dealerHtml == "")
			vendor.style.display = "none";
		else
			vendor.innerHTML = dealerHtml;
	});
}

var curBigImg = null;

// modified Oct.12.2009
// window.onload = function()
function csSummaryJs() {
	curImg = document.getElementById("focusBigImg_0");
	for (j = 0; j < 3; j++) {
		var imgId = "focusSmallImg_" + j;
		var img = document.getElementById(imgId);
		if (img)
			img.onmouseover = ImageMouseoverHandler;
		else
			break;

	}
	var compareContainer = document.getElementById("compare");
	if (compareContainer == null) return;
	var trs = compareContainer.getElementsByTagName("tr");
	for (i in trs) {
		trs[i].onmouseover = function () {
			this.style.background = "#F8F8F8";
		};
		trs[i].onmouseout = function () {
			this.style.background = "";
		}
	}
}

// modified Oct.12.2009 end

function ImageMouseoverHandler() {
	var bigImg = document.getElementById(this.id.replace("Small", "Big"));
	var curSmallImg = document.getElementById(curImg.id.replace("Big", "Small"));

	curImg.style.display = "none";
	bigImg.style.display = "block";
	curSmallImg.className = "";
	this.className = "current";
	curImg = bigImg;
}

// add for csSurrary Hot Compare Serial
function ShowHotCompareSerialInfoList(serialId, csName) {
	var hotCompareSerial =
	{
		parameters: "csid=" + serialId + "&csName=" + csName,
		method: "get",
		onSuccess: onAjaxForHotCompareSerialSuccess
	}
	new Ajax.Request("/ajaxnew/GetHotCompareSerial.aspx", hotCompareSerial);
}

function onAjaxForHotCompareSerialSuccess(res) {
	var serialHotCompareList = document.getElementById("serialHotCompareList");
	if (res.responseText.length == 0)
		serialHotCompareList.style.display = "none";
	else
		serialHotCompareList.innerHTML = res.responseText;
}

// modified by Chengl Oct.20.2010
function BtHide(id) {
	var Div = document.getElementById(id);
	if (Div) {
		Div.style.display = "none";
	}
}
function BtShow(id) {
	var Div = document.getElementById(id);
	if (Div) {
		Div.style.display = "block";
	}
}
function BtTabRemove(index, head, divs) {
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var lis = tab_heads.getElementsByTagName("li");
		var as = tab_heads.getElementsByTagName("a");
		for (var i = 0; i < as.length; i++) {
			lis[i].className = "";
			BtHide(divs + "_" + i);
			if (i == index) {
				lis[i].className = "current";
			}
		}
		BtShow(divs + "_" + index);
	}
}
function TabOn(head, divs) {
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		BtTabRemove(0, head, divs);
		var alis = tab_heads.getElementsByTagName("a");
		for (var i = 0; i < alis.length; i++) {
			alis[i].num = i;
			var innerText = alis[i].innerText;
			if (!innerText)
				innerText = alis[i].innerHTML;
			if (innerText != "最新" && alis[i].className == "nolink")
				alis[i].style.display = "none";
			alis[i].onmouseover = function () {
				BtTabRemove(this.num, head, divs);
				this.blur();
				return false;
			}
			alis[i].onfocus = function () {
				BtTabRemove(this.num, head, divs);
			}
		}
	}
}
TabOn("toptab", "toptab");
var topa1Ele = document.getElementById("topa1");
if (topa1Ele) {
	var sfEls = topa1Ele.getElementsByTagName("li");
	for (var i = 0; i < (sfEls.length - 1); i++) {
		if ((i + 1) % 6 == 0) {
			sfEls[i].className += "span";
		}
	}
}

//---------------------友情链接用------------------------
function dealer_logo() {
	if (!document.getElementById("dealer_logo")) return false;
	var PMarquee = document.getElementById("dealer_logo");
	var lis = PMarquee.getElementsByTagName("li");
	var lisw = lis[0].clientWidth;
	var celnum = Math.floor(PMarquee.clientWidth / lisw);
	//cel mun

	var PLineCount = Math.ceil(lis.length / celnum);
	//line mun

	var copynum = (PLineCount * celnum) - lis.length;

	if (PLineCount > 1) {
		var Pjia;
		Pjia = PMarquee.innerHTML;

		for (i = 0; i < copynum + 1; i++) {

			PMarquee.innerHTML = PMarquee.innerHTML + Pjia;
			// double box
		}
		setTimeout("srun()", 5000);
		//start
	}
}
function srun() {
	if (!document.getElementById("dealer_logo")) return false;
	var PMarquee = document.getElementById("dealer_logo");
	var lis = PMarquee.getElementsByTagName("li");
	var lisw = lis[0].clientWidth;
	var PLineHeight = lis[0].clientHeight;

	var celnum = Math.floor(PMarquee.clientWidth / lisw);
	//cel mun
	var PLineCount = Math.ceil(lis.length / celnum);
	//line mun
	PMarquee.scrollTop++;
	if (PMarquee.scrollTop == PLineCount * PLineHeight - (2 * PLineHeight)) {
		PMarquee.scrollTop = 0;
		//re
	}
	if (PMarquee.scrollTop % PLineHeight == 0) {
		setTimeout("srun()", 5000);
	}
	else {
		setTimeout("srun()", 5);
		//one px time
	}
}
addLoadEvent(dealer_logo);


///------切换城市行情用----------------------------------------------------------

//window.onload = judgeNum;
addLoadEvent(judgeNum);
var moveing = false;
function judgeNum() {
	if (!document.getElementById("colorBox")) return false;
	if (!document.getElementById("LeftArr")) return false;
	if (!document.getElementById("RightArr")) return false;
	var allColor = document.getElementById("colorBox").getElementsByTagName("li");
	var leftArr = document.getElementById("LeftArr");
	var rightArr = document.getElementById("RightArr");
	if (allColor.length <= 6) {
		document.getElementById("LeftArr").className = "lGray";
		document.getElementById("RightArr").className = "rGray";
	} else {
		leftArr.onclick = function () { previous('LeftArr', 'RightArr', 'innerBox'); }
		//leftArr.onmouseover = function() {this.className='lHover'}
		//leftArr.onmouseout = function() {this.className='l'}
		rightArr.onclick = function () { next('LeftArr', 'RightArr', 'innerBox'); }
		rightArr.onmouseover = function () { this.className = 'rHover' }
		rightArr.onmouseout = function () { this.className = 'r' }
	}
}
function moveElement(elementID, final_x, final_y, interval) {
	if (!document.getElementById) return false;
	if (!document.getElementById(elementID)) return false;
	var elem = document.getElementById(elementID);
	if (elem.movement) {
		clearTimeout(elem.movement);
	}
	if (!elem.style.left) {
		elem.style.left = "0px";
	}
	if (!elem.style.top) {
		elem.style.top = "0px";
	}
	var xpos = parseInt(elem.style.left);
	var ypos = parseInt(elem.style.top);
	if (xpos == final_x && ypos == final_y) {
		moveing = false;
		return true;
	}
	if (xpos < final_x) {
		var dist = Math.ceil((final_x - xpos) / 10);
		xpos = xpos + dist;
	}
	if (xpos > final_x) {
		var dist = Math.ceil((xpos - final_x) / 10);
		xpos = xpos - dist;
	}
	if (ypos < final_y) {
		var dist = Math.ceil((final_y - ypos) / 10);
		ypos = ypos + dist;
	}
	if (ypos > final_y) {
		var dist = Math.ceil((ypos - final_y) / 10);
		ypos = ypos - dist;
	}
	elem.style.left = xpos + "px";
	elem.style.top = ypos + "px";
	var repeat = "moveElement('" + elementID + "'," + final_x + "," + final_y + "," + interval + ")";
	elem.movement = setTimeout(repeat, interval);
}

function next(previousBtn, nextBtn, scrollID) {
	if (moveing) return;
	moveing = true;
	var vTop = parseInt(document.getElementById(scrollID).style.top);
	var vLeft = parseInt(document.getElementById(scrollID).style.left);
	var allColor = document.getElementById("colorBox").getElementsByTagName("li");

	var leftArr = document.getElementById("LeftArr");
	var rightArr = document.getElementById("RightArr");

	leftArr.onmouseover = function () { this.className = 'lHover' }
	leftArr.onmouseout = function () { this.className = 'l' }

	if (vLeft <= -((allColor.length - 6) * 103)) return moveing = false;
	if (vLeft == -((allColor.length - 7) * 103)) {
		document.getElementById(nextBtn).className = 'rGray';
		rightArr.onmouseover = function () { return false }
		rightArr.onmouseout = function () { return false }
	}
	var finalLeft = vLeft - 103;
	moveElement(scrollID, finalLeft, vTop, 5);
	document.getElementById(previousBtn).className = 'l';
}
function previous(previousBtn, nextBtn, scrollID) {
	var leftArr = document.getElementById("LeftArr");
	var rightArr = document.getElementById("RightArr");

	rightArr.onmouseover = function () { this.className = 'rHover' }
	rightArr.onmouseout = function () { this.className = 'r' }

	if (moveing) return;
	moveing = true;
	var vTop = parseInt(document.getElementById(scrollID).style.top);
	var vLeft = parseInt(document.getElementById(scrollID).style.left);
	if (vLeft >= 0) return moveing = false;
	if (vLeft == -103) {
		document.getElementById(previousBtn).className = 'lGray';
		leftArr.onmouseover = function () { return false }
		leftArr.onmouseout = function () { return false }
	}
	var finalLeft = vLeft + 103;
	moveElement(scrollID, finalLeft, vTop, 5);
	document.getElementById(nextBtn).className = 'r';
}


//异步加载JavaScript
var loadJS = {
	lock: false, ranks: []
		, callback: function (startTime, callback) {
			//载入完成
			this.lock = false;
			callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
			this.read(); //解锁，在次载入
		}
		, read: function () {
			//读取
			if (!this.lock && this.ranks.length) {
				var head = document.getElementsByTagName("head")[0];

				if (!head) {
					ranks.length = 0, ranks = null;
					throw new Error('HEAD不存在');
				}

				var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

				this.lock = true;

				script.onload = script.onreadystatechange = function () {
					if (script && script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') return;

					script.onload = script.onreadystatechange = script.onerror = null, script.src = ''
				, script.parentNode.removeChild(script), script = null; //清理script标记

					wc.callback(startTime, ranks.callback), startTime = ranks = null;
				};

				script.charset = ranks.charset || 'gb2312';
				script.src = ranks.src;

				head.appendChild(script);
			}
		}
		, push: function (src, charset, callback) {
			//加入队列
			this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
			this.read();
		}
}


//行情新闻列表
var ChangCityHangQing = {
	titleId: 'cityName',
	newslistId: 'jiangjialist',
	moreId: 'jiangjiamore',
	cityId: '201',
	questUrl: 'http://api.car.bitauto.com/newsinfo/getjiangjianews.ashx?id=@serialId@&cityid=@cityid@',
	liFormart: '<li><span><a href="http://jiangjia.bitauto.com/nb@serialid@/" class="fl">降价</a>| </span><a href="@url@" title="@title@">@title@</a><small>@time@</small></li>',
	moreFormart: '直降：<em><a href="http://jiangjia.bitauto.com/nb@serialid@/">@maxfav@万</a></em>',
	trim: function (value) {
		return (value || "").replace(/^\s+|\s+$/g, "");
	},
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
	},
	sendRequest: function () {
		var url = this.questUrl.replace(/@cityid@/g, this.cityId).replace(/@serialId@/g, typeof serialId != 'undefined' ? serialId : "");
		loadJS.push(url, "utf-8", function () { ChangCityHangQing.printNewsList(); });
	},
	printNewsList: function () {
		if (typeof jjnews == 'undefined' || jjnews == null) return;
		var cityName = jjnews["name"];
		var liCotent = [];
		var moreContent = "";
		var exitsContent = true;
		//如果存在行情列表
		if (typeof jjnews["nlist"] != 'undefined'
                && jjnews["nlist"] != null
                && jjnews["nlist"].length > 0) {
			for (var i = 0; i < jjnews["nlist"].length; i++) {
				var newsValue = jjnews["nlist"][i];
				var nvlist = newsValue.split(',');
				if (nvlist == null || nvlist.length < 1) continue;
				var title = decodeURIComponent(nvlist[0]);
				var purl = decodeURIComponent(nvlist[1]);
				var time = nvlist[2];

				var cli = this.liFormart.replace(/@url@/g, purl);
				cli = cli.replace(/@title@/g, title);
				cli = cli.replace(/@time@/g, time);
				cli = cli.replace(/@serialid@/g, typeof serialId != 'undefined' ? serialId : "");
				liCotent.push(cli);
			}
			if (!isNaN(jjnews["maxfav"])) {
				var maxfav = parseFloat(jjnews["maxfav"]);
				if (maxfav > 0) {
					moreContent = this.moreFormart.replace(/@serialid@/g, typeof serialId != 'undefined' ? serialId : "");
					moreContent = moreContent.replace(/@maxfav@/g, maxfav);
				}
			}
		}
		else {
			exitsContent = false;
			liCotent.push('');
		}

		var noContentControl = document.getElementById('hangqing_nocontents');
		var control = document.getElementById(this.newslistId);
		if (!control) { return; }
		if (exitsContent && noContentControl) {
			control.style.display = "block";
			noContentControl.style.display = "none";
		}
		else if (!exitsContent && noContentControl) {
			control.style.display = "none";
			noContentControl.style.display = "block";
		}
		else if (!exitsContent && !noContentControl) {
			control.style.display = "none";
			var nextControl = control.nextSibling;
			while (nextControl.nodeType != 1) {
				nextControl = nextControl.nextSibling;
			}
			var parentControl = control.parentNode;
			while (parentControl.nodeType != 1) {
				parentControl = parentControl.nextSibling;
			}

			var noControl = this.createNoContentElement();
			parentControl.insertBefore(noControl, nextControl);
		}

		document.getElementById(this.titleId).innerHTML = cityName + "降价";
		document.getElementById(this.newslistId).innerHTML = liCotent.join("");
		document.getElementById(this.moreId).innerHTML = moreContent;
	},
	createNoContentElement: function () {
		var control = document.createElement("div");
		control.id = "hangqing_nocontents";
		control.className = "car_nonedata";
		control.innerHTML = "本地暂无降价行情";
		return control;
	},
	getCityId: function () {
		var cityCookieValue = this.readCookie("bitauto_ipregion");
		if (cityCookieValue != null) {
			var cityEntity = cityCookieValue.split(";");
			if (cityEntity != null && cityEntity.length > 1) {
				var cityEntityElement = cityEntity[1].split(",");
				if (cityEntityElement != null && cityEntityElement.length > 1) {
					this.cityId = cityEntityElement[0];
				}
			}
		}
		this.sendRequest();
	}, //设置城市ID
	settingCityId: function (id) {
		this.cityId = id;
		this.sendRequest();
	}, //初始化对象
	init: function () {
		if (typeof serialId == 'undefined') return;
		this.getCityId();
	}
}

//切换城市列表---
function AreasTrans(BtnChange, BtnClose, AreaList) {
	var div = document.getElementById(AreaList);
	var btn = document.getElementById(BtnChange);
	if ((div) && (btn)) {
		btn.onclick = function () {
			if (div.style.display == "none") {
				div.style.display = "block";
				btn.className = "change_btn_open";
			}
			else {
				div.style.display = "none";
				btn.className = "change_btn_close";
			}
		}
		var close_btn = document.getElementById(BtnClose);
		close_btn.onclick = function () {
			document.getElementById(AreaList).style.display = "none";
			btn.className = "change_btn_close";
		}
	}
}
//改变行情城市选择
function changehangqingnews(id) {
	document.getElementById("province_list").style.display = "none";
	document.getElementById("change_btn").className = "change_btn_close";
	ChangCityHangQing.settingCityId(id);
}
function AreasTransfunc() {
	AreasTrans("change_btn", "province_list_close", "province_list");
	ChangCityHangQing.init();
}
try {
	AreasTransfunc();
	// addLoadEvent(AreasTransfunc);
}
catch (err) { }
//切换城市列表结束-----

addLoadEvent(csSummaryJs);


//子品牌综述页  add by ddl 2011-09-14

//切换城市文字变色
function addUnderline(id) {
	var addArea = document.getElementById(id);
	if (!addArea)
	{ return; }
	var allItem = addArea.getElementsByTagName("s");
	if (!allItem)
	{ return; }
	for (var i = 0; i < allItem.length; i++) {
		allItem[i].onmouseover = function () {
			this.style.textDecoration = "underline";
			this.style.color = "#CC0000";
		}
		allItem[i].onmouseout = function () {
			this.style.textDecoration = "none";
			this.style.color = "#164A84";
		}
	}
}

var moveing = false;
function scrollCarColor() {
	if (!document.getElementById("arrowUp")) return false;
	if (!document.getElementById("arrowDown")) return false;
	if (!document.getElementById("colorBox")) return false;
	var arrowUp = document.getElementById("arrowUp"); //向上按钮
	var arrowDown = document.getElementById("arrowDown"); //向下按钮
	var allColor = document.getElementById("colorBox").getElementsByTagName("li");
	var allColorNum = allColor.length; //色块总数
	var overColorNum = allColorNum - 9 //隐藏的色块
	var normalMoveNum = parseInt(overColorNum / 9);
	var clickNum = 0;

	var vLeft = parseInt(document.getElementById("colorBox").style.left); //容器的left值
	var finalTop = 0;
	var itemHeight = 15 //每个颜色的高度

	var totalHeight = allColorNum * itemHeight; //色块的总高度
	var lastDistance = totalHeight - (allColorNum % 9) * itemHeight //最后一次移动的距离

	//判断箭头是否显示
	if (allColorNum <= 10) {
		arrowUp.style.display = "none";
		arrowDown.style.display = "none";
		moveing = false;
	}
	if (finalTop == 0) { arrowUp.style.display = "none"; }
	arrowUp.onclick = function () {
		if (moveing) return false;
		moveing = true;
		if (finalTop == 0) return moveing = false;
		arrowDown.style.display = "block";
		clickNum--;
		for (var i = 0; i < allColor.length; i++) {
			var cName = "", displayvalue = "none";
			if (i == clickNum * 9) {
				cName = "current";
				displayvalue = "block";
			}
			if (document.getElementById("exampleCarPic").getElementsByTagName("a")[i] != null) {
				document.getElementById("exampleCarPic").getElementsByTagName("a")[i].style.display = displayvalue;
			}
			if (allColor[i].getElementsByTagName("em") != null) {
				allColor[i].getElementsByTagName("em")[0].className = cName;
			}
			if (allColor[i].getElementsByTagName("b")[0] != null) {
				allColor[i].getElementsByTagName("b")[0].style.display = displayvalue;
			}
			if (allColor[i].getElementsByTagName("strong")[0] != null) {
				allColor[i].getElementsByTagName("strong")[0].style.display = displayvalue;
			}
		}
		//		if (finalTop != -15 && finalTop != -30) { //非最后一次点击
		//			finalTop = finalTop + itemHeight * 3;
		//		} else if (finalTop == -15) { //最后一次点击
		//			finalTop = finalTop + itemHeight;
		//		} else if (finalTop == -30) {
		//			finalTop = finalTop + itemHeight * 2;
		//		}
		finalTop = finalTop + itemHeight * 9;
		moveElement("colorBox", vLeft, finalTop, 5);
		if (finalTop == 0) { arrowUp.style.display = "none"; }
		return false;
	}
	arrowDown.onclick = function () {
		if (moveing) return false;
		moveing = true;
		if (finalTop < -((allColor.length - 9) * itemHeight)) return moveing = false;
		arrowUp.style.display = "block";
		clickNum++;
		for (var i = 0; i < allColor.length; i++) {
			var cName = "", displayvalue = "none";
			if (i == clickNum * 9) {
				cName = "current";
				displayvalue = "block";
			}
			if (document.getElementById("exampleCarPic").getElementsByTagName("a")[i] != null) {
				document.getElementById("exampleCarPic").getElementsByTagName("a")[i].style.display = displayvalue;
			}
			if (allColor[i].getElementsByTagName("em") != null) {
				allColor[i].getElementsByTagName("em")[0].className = cName;
			}
			if (allColor[i].getElementsByTagName("b")[0] != null) {
				allColor[i].getElementsByTagName("b")[0].style.display = displayvalue;
			}
			if (allColor[i].getElementsByTagName("strong")[0] != null) {
				allColor[i].getElementsByTagName("strong")[0].style.display = displayvalue;
			}
		}
		//		if (finalTop != -(normalMoveNum * 45)) { //非最后一次点击
		//			finalTop = finalTop - itemHeight * 3;
		//		} else if (finalTop == -(normalMoveNum * 45)) { //最后一次点击
		//			if (allColorNum % 3 == 0) {
		//				finalTop = finalTop - itemHeight * 3;
		//			} else if (allColorNum % 3 == 1) {
		//				finalTop = finalTop - itemHeight;
		//			} else if (allColorNum % 3 == 2) {
		//				finalTop = finalTop - itemHeight * 2;
		//			}
		//		}
		finalTop = finalTop - itemHeight * 9;
		moveElement("colorBox", vLeft, finalTop, 5);
		if (finalTop <= -((allColor.length - 9) * itemHeight)) { arrowDown.style.display = "none"; }
		return false;
	}
}
function exampleCarColor() {
	if (!document.getElementById("exampleCarPic")) return false;
	if (!document.getElementById("exampleCarColor")) return false;
	var exampleCarPic = document.getElementById("exampleCarPic"); //图片
	var exampleCarColor = document.getElementById("exampleCarColor"); //色块

	//色块居中显示
	var colorNum = exampleCarColor.getElementsByTagName("li").length;
	if (colorNum < 10) {
		var padTop = (200 - 16 * colorNum) / 2;
	} else {
		var padTop = 28;
	}
	exampleCarColor.style.marginTop = padTop + "px";
	//变换图片
	var allColor = exampleCarColor.getElementsByTagName("li");
	var allTxt = exampleCarColor.getElementsByTagName("b");
	var lookCarPic = exampleCarColor.getElementsByTagName("strong");
	var allColorBox = exampleCarColor.getElementsByTagName("em");
	var allImg = exampleCarPic.getElementsByTagName("a");
	var images = exampleCarPic.getElementsByTagName("img");
	var colorLen = allColor.length;
	for (var i = 0; i < colorLen; i++) {
		(function (arg) {
			allColorBox[i].onmouseover = function () {
				for (var j = 0; j < colorLen; j++) {
					allTxt[j].style.display = "none";
					allColorBox[j].className = "";
					allImg[j].style.display = "none";
					lookCarPic[j].style.display = "none";
				}
				allTxt[arg].style.display = "block";
				allColorBox[arg].className = "current";
				allImg[arg].style.display = "block";
				lookCarPic[arg].style.display = "block";
				if (!images[arg].getAttribute("src")) {
					images[arg].setAttribute("src", images[arg].getAttribute("data-original"));
				}
			}
		})(i);
	}
}
function carPicSize() {
	if (!document.getElementById("sizeMainPic")) return false;
	if (!document.getElementById("sizeSubPic")) return false;
	var sizeMainPic = document.getElementById("sizeMainPic");
	var sizeSubPic = document.getElementById("sizeSubPic");
	var allSizeMainPic = sizeMainPic.getElementsByTagName("div");
	var allSizeSubPic = sizeSubPic.getElementsByTagName("li");
	var picLen = allSizeSubPic.length;
	for (var i = 0; i < picLen; i++) {
		(function (arg) {
			allSizeSubPic[i].onmouseover = function () {
				for (var j = 0; j < picLen; j++) {
					allSizeMainPic[j].style.display = "none";
					allSizeSubPic[j].className = "";
				}
				allSizeMainPic[arg].style.display = "block";
				allSizeSubPic[arg].className = "current";
			}
		})(i);
	}
}
// addLoadEvent(exampleCarColor);
// addLoadEvent(carPicSize);
// addLoadEvent(scrollCarColor);

//// 收藏提示
//var CsSummaryFavorites = {
//	// 默认不显示
//	IsOtherRefer: false,
//	IsClosedCurrentCsID: false,
//	FavoritesTitle: "",
//	TestNum: /^\d+$/,
//	ReDomain: /(\w+):\/\/([^\:|\/]+)(\:\d*)?/i,
//	CurrentCsID: "",
//	CookieName: "CloseSerialList",
//	HtmlArray: new Array(),
//	CloseCsIDs: new Array(),
//	CheckIsOtherRefer: function () {
//		var lastURL = document.referrer.length > 500 ? document.referrer.substring(0, 500) : document.referrer;
//		if (lastURL == "")
//		{ return; }
//		var arrURL = lastURL.match(this.ReDomain);
//		if (typeof (arrURL) != 'undefined' && arrURL && arrURL.length > 2 && typeof (arrURL[2]) != 'undefined' && arrURL[2] && arrURL[2] != "") {
//			if (arrURL[2].indexOf("bitauto.com") < 0 && arrURL[2].indexOf("cheyisou.com") < 0 && arrURL[2].indexOf("yiche.com") < 0) {
//				// 非易车网来源
//				this.IsOtherRefer = true;
//			}
//		}
//	},
//	AddFavorites: function () {
//		var url = window.location.href;
//		if (window.sidebar) {
//			window.sidebar.addPanel(this.FavoritesTitle, url, "");
//		}
//		else {
//			try {
//				window.external.addFavorite(url, this.FavoritesTitle);
//			}
//			catch (e) {
//				alert("加入收藏失败，请使用Ctrl+D进行添加");
//			}
//		}
//	},
//	CloseCurrentSerial: function () {
//		if (this.CloseCsIDs.join(",").indexOf(this.CurrentCsID) >= 0)
//		{ /*包含当前子品牌*/return; }
//		if (this.CloseCsIDs.length >= 10)
//		{ this.CloseCsIDs.splice(0, 1); }
//		this.CloseCsIDs.push(this.CurrentCsID);
//		setCookieForSummary(this.CookieName, this.CloseCsIDs.join(","));
//		$("div.collectMe").hide();
//	},
//	CheckIsShowFavorites: function () {
//		this.CheckIsOtherRefer();
//		if (!this.IsOtherRefer)
//		{ /*易车网来源*/return; }
//		var cookieForCloseSerial = getCookieForSummary(this.CookieName);
//		if (cookieForCloseSerial && cookieForCloseSerial != "") {
//			var tempArray = new Array();
//			tempArray = cookieForCloseSerial.split(",");
//			for (var i = 0; i < tempArray.length; i++) {
//				if (tempArray[i] != null && this.TestNum.test(tempArray[i])) {
//					if (this.CloseCsIDs.join(",").indexOf(tempArray[i]) < 0 && this.CloseCsIDs.length < 10) {
//						this.CloseCsIDs.push(tempArray[i]);
//					}
//					if (tempArray[i] == this.CurrentCsID) {
//						this.IsClosedCurrentCsID = true;
//					}
//				}
//			}
//			if (this.IsClosedCurrentCsID)
//			{ /*当前关闭*/return; }
//		}
//		this.HtmlArray.push("<div class=\"collectMe\">");
//		this.HtmlArray.push("<div class=\"collectMeBox\">");
//		this.HtmlArray.push("<span>将本页 <a class=\"cRed\" onclick=\"CsSummaryFavorites.AddFavorites()\" target=\"_self\" href=\"javascript:void(0);\">加入收藏夹</a>");
//		this.HtmlArray.push(" 或 <a class=\"cRed\" target=\"_self\" href=\"/interfaceforbitauto/SavePageDestop.ashx?id=" + this.CurrentCsID + "\">保存到桌面</a>，方便下次查看。 ");
//		this.HtmlArray.push("<a onclick=\"CsSummaryFavorites.CloseCurrentSerial()\" target=\"_self\" href=\"javascript:void(0);\">不再提示</a></span>");
//		this.HtmlArray.push("<div onclick=\"CsSummaryFavorites.CloseCurrentSerial()\" class=\"collectMeClose\">关闭</div>");
//		this.HtmlArray.push("</div>");
//		this.HtmlArray.push("</div>");
//		$("div.bit_top").after(this.HtmlArray.join(""));
//	}
//};
//--核心看点
function tabs(head, divs, div2s, over) {
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			alis[i].num = i;
			if (over) {
				alis[i].onmouseover = function () {
					var thisobj = this;
					thetabstime = setTimeout(function () {
						changetab(thisobj);
					}, 150);
				}
				alis[i].onmouseout = function () {
					clearTimeout(thetabstime);
				}
			} else {
				alis[i].onclick = function () {
					if (this.className == "current" || this.className == "last current") {
						changetab(this);
						return true;
					} else {
						changetab(this);
						return false;
					}

				}
			}
			function changetab(thebox) {
				tabsRemove(thebox.num, head, divs, div2s);
			}
		}
	}
}
function tabsRemove(index, head, divs, div2s) {
	if (!document.getElementById(head))
		return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			removeClass(alis[i], "current");

			BtHide(divs + "_" + i);
			if (div2s) {
				BtHide(div2s + "_" + i)
			};

			if (i == index) {
				addClass(alis[i], "current");
			}
		}

		BtShow(divs + "_" + index);
		if (div2s) {
			BtShow(div2s + "_" + index)
		};
	}
}
function all_func() {
	tabs("car_core", "car_core_con", null, true);
}
addLoadEvent(all_func);
////获取易车惠商品信息
//function getSerialGoods(serialId, serialName, cityId) {
//	$.ajax({ url: 'http://api.car.bitauto.com/Mai/GetSerialGoods.ashx?serialid=' + serialId + '&cityid=' + cityId, cache: true, dataType: "jsonp", jsonpCallback: "mai_callback", success: function (data) {
//		var html = [];
//		$.each(data, function (i, n) {
//			var goodsUrl = n.GoodsUrl.replace('/detail', '/all/detail') + "?WT.mc_id=car1";
//			html.push("<div class=\"tejiache_t\"><h5>易车惠</h5></div>");
//			html.push("<div class=\"tejiache_con\">");
//			html.push("<ul>");
//			html.push("<li class=\"tejiache_pic\"><a href=\"" + goodsUrl + "\" target=\"_blank\"><img src=\"" + n.CoverImageUrl + "\" width=180 height=120/></a></li>");
//			html.push("<li class=\"tejiache_title\"><a href=\"" + goodsUrl + "\" target=\"_blank\">" + serialName + "</a></li>");
//			html.push("<li class=\"tejiache_title2\">" + n.PromotTitle + "</li>");
//			html.push("<li class=\"tejiache_price\">");
//			html.push("<a href=\"" + goodsUrl + "\" target=\"_blank\"><em>" + n.MinBitautoPrice + "万起</em></a>");
//			html.push("<del>" + n.MinMarketPrice + "万起</del>");
//			html.push("</li>");
//			html.push("<li class=\"tejiache_tag\">");
//			$.each(n.PromotionList, function (i, n) { if (i > 2) return; html.push("<span title=\"" + this.Description + "\">" + this.Name + "</span>"); });
//			html.push("</li>");
//			html.push("<li class=\"tejiache_time\">" + n.StartTime.replace(/-/g, '.') + " - " + (n.EndTime == '0001-01-01' ? "至今" : n.EndTime.replace(/-/g, '.')) + "</li>");
//			html.push("<li class=\"tejiache_submit\"><div class=\"button_orange button_122_28\"><a href=\"" + goodsUrl + "\" target=\"_blank\">立即报名&gt;&gt;</a></div></li>");
//			html.push("<li class=\"tejiache_date\"><a href=\"http://mai.bitauto.com/all/?WT.mc_id=car3\" target=\"_blank\">更多优惠车型，尽在易车惠&gt;&gt;</a></li>");
//			html.push("</ul>");
//			html.push("</div>");
//			$("#tejiache_box").show().html(html.join(""));
//		});
//	}
//	});
//}
function getSerialGoodsNew(serialId, serialName, imageUrl, cityId) {
	$.ajax({ url: 'http://api.car.bitauto.com/Mai/GetSerialGoodsNew.ashx?serialid=' + serialId + '&cityid=' + cityId, cache: true, dataType: "jsonp", jsonpCallback: "mai_callback", success: function (data) {
		var rightHtml = [];
		if (!data.CarList || data.CarList.length <= 0) return;
		var minCarGoods = data.CarList[0];
		var goodsUrl = minCarGoods.GoodsUrl + "&WT.mc_id=car1";
		rightHtml.push("<div class=\"tejiache_t\"><h5>易车惠</h5></div>");
		rightHtml.push("<div class=\"tejiache_con\">");
		rightHtml.push("<ul>");
		rightHtml.push("<li class=\"tejiache_pic\"><a href=\"" + goodsUrl + "\" target=\"_blank\"><img src=\"" + imageUrl + "\" width=180 height=120/></a></li>");
		rightHtml.push("<li class=\"tejiache_title\"><a href=\"" + goodsUrl + "\" target=\"_blank\">" + serialName + "</a></li>");
		rightHtml.push("<li class=\"tejiache_title2\"></li>");
		rightHtml.push("<li class=\"tejiache_price\">");
		rightHtml.push("<a href=\"" + goodsUrl + "\" target=\"_blank\"><em>" + minCarGoods.MinBitautoPrice + "万起</em></a>");
		if (minCarGoods.MinBitautoPrice != minCarGoods.MinMarketPrice) {
			rightHtml.push("<del>" + minCarGoods.MinMarketPrice + "万起</del>");
		}
		rightHtml.push("</li>");
		rightHtml.push("<li class=\"tejiache_tag\">");
		rightHtml.push("<span title=\"\">携程卡</span>");
		rightHtml.push("<span title=\"\">途家卡</span>");
		rightHtml.push("<span title=\"\">加油卡</span>");
		rightHtml.push("</li>");
		rightHtml.push("<li class=\"tejiache_time\">2013.11.11 - 2013.12.12</li>");
		rightHtml.push("<li class=\"tejiache_submit\"><div class=\"button_orange button_122_28\"><a href=\"" + goodsUrl + "\" target=\"_blank\">立即报名&gt;&gt;</a></div></li>");
		rightHtml.push("<li class=\"tejiache_date\"><a href=\"http://tuan.mai.bitauto.com/?WT.mc_id=car3\" target=\"_blank\">更多优惠车型，尽在易车惠&gt;&gt;</a></li>");
		rightHtml.push("</ul>");
		rightHtml.push("</div>");
		//右侧
		$("#tejiache_box").show().html(rightHtml.join(""));
		//车型列表
		$.each(data.CarList, function (i, n) {
			if (i >= 3) return;
			$("#carlist_" + n.CarId).append(" <a target=\"_blank\" href=\"" + n.GoodsUrl + "&WT.mc_id=car2\" class=\"ad-yichehui-list\">易车惠特价&gt;&gt;</a>");
		});
		//导航
		$("#thelogoID > div.rank").before("<a class=\"ad-yichehui\" target=\"_blank\" href=\"http://tuan.mai.bitauto.com/detail.aspx?carstyleid=" + serialId + "&WT.mc_id=car4\">易车惠有特价，立即查看&gt;&gt;</a>");
	}
	});
}
//--核心看点 end
//--综述页 车款 选项卡切换 停销年款款显示 jquery
$(function () {
	$('#car_list tr').hover(function () { $(this).addClass('hover-bg-color'); },
		function () { $(this).removeClass('hover-bg-color'); });
	// 按钮点击状态
	$('#car_list .car-summary-btn').hover(function () { $(this).addClass('car-summary-btn-hover'); },
		function () { $(this).removeClass('car-summary-btn-hover'); $(this).removeClass('car-summary-btn-active'); })
		.mousedown(function () { $(this).addClass('car-summary-btn-active'); })
		.mouseup(function () { $(this).removeClass('car-summary-btn-active'); });
	$("#car_nosaleyearlist").hover(function () { $(this).find("dl").show(); }, function () { $(this).find("dl").hide(); });
	//resolve lazyload
	$("#data_tab_jq5 li").eq(0).one("mouseover", function () {
		if ($("#data_tab_jq5_1").length == 0) return;
		var waitCarList = $("#data_tab_jq5_0").height();
		var saleCarList = $("#data_tab_jq5_1").height();
		if (waitCarList < saleCarList)
			$(window).scrollTop($(window).scrollTop() + 1);
	});
	jqTab("data_tab_jq5", "mouseover");
	//jquery tab
	function jqTab(containerId, eventListener) {
		$("#" + containerId + ">li[id!='car_nosaleyearlist']").each(function (i) {
			$(this).bind(eventListener, function () {
				$(this).addClass("current").siblings().removeClass("current");
				$("#" + containerId + "_" + i + "").show().siblings("div[id^='" + containerId + "']").hide();
			});
		});
	}
});
