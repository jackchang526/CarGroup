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

//显示经销商信息
function ShowVendorInfo(serialId, cityId) {
    var url = "http://api.car.bitauto.com/carinfo/iframefordealer.aspx?csId=" + serialId + "&cityId=" + cityId + "";
    loadJS.push(url, "utf-8", function () {
        var vendor = document.getElementById("vendorInfo");
        if (typeof dealerHtml == 'undefined' || dealerHtml == null||dealerHtml=="")
            vendor.style.display = "none";
        else
            vendor.innerHTML = dealerHtml;
    });
}

var curBigImg = null;

// modified Oct.12.2009
// window.onload = function()
function csSummaryJs() {
	var trs = document.getElementById("compare").getElementsByTagName("tr");
	for (i in trs) {
		trs[i].onmouseover = function () {
			this.style.background = "#F8F8F8";
		};
		trs[i].onmouseout = function () {
			this.style.background = "";
		}
	}
	curImg = document.getElementById("focusBigImg_0");
	for (j = 0; j < 3; j++) {
		var imgId = "focusSmallImg_" + j;
		var img = document.getElementById(imgId);
		if (img)
			img.onmouseover = ImageMouseoverHandler;
		else
			break;

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
	newslistId: 'hangqinglist',
	cityId: '201',
	questUrl: 'http://api.car.bitauto.com/newsinfo/gethangqingnews.aspx?id=@serialId@&cityid=@cityid@',
	liFormart: '<li><a href="http://car.bitauto.com/@serialallspell@/hangqing/" class="fl">[行情]</a><a href="@url@" title="@title@">@title@</a><small>@time@</small></li>',
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
		if (typeof hqingCity == 'undefined' || hqingCity == null) return;
		var cityName = hqingCity["name"];
		var liCotent = [];
		var exitsContent = true;
		//如果存在行情列表
		if (typeof hqingCity["nlist"] != 'undefined'
                && hqingCity["nlist"] != null
                && hqingCity["nlist"].length > 0) {
			for (var i = 0; i < hqingCity["nlist"].length; i++) {
				var newsValue = hqingCity["nlist"][i];
				var nvlist = newsValue.split(',');
				if (nvlist == null || nvlist.length < 1) continue;
				var title = decodeURIComponent(nvlist[0]);
				var purl = decodeURIComponent(nvlist[1]);
				var time = nvlist[2];

				var cli = this.liFormart.replace(/@url@/g, purl);
				cli = cli.replace(/@title@/g, title);
				cli = cli.replace(/@time@/g, time);
				cli = cli.replace(/@serialallspell@/g, typeof serialallSpell != 'undefined' ? serialallSpell : "");
				liCotent.push(cli);
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

		document.getElementById(this.titleId).innerHTML = cityName + "行情";
		document.getElementById(this.newslistId).innerHTML = liCotent.join("");
	},
	createNoContentElement: function () {
		var control = document.createElement("div");
		control.id = "hangqing_nocontents";
		control.className = "car_nonedata";
		control.innerHTML = "暂无行情";
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
