var PhotoComparePageObject = {
	SelectListInterface: "/car/ajaxnew/SelectListForCompare.aspx",
	TabListInterface: "/car/ajaxnew/ListForPhotoComparev1.aspx",
	TabListIndex: 1,
	TabListDivID: "ListForCompare_divContent",
	TabListHideLinkID: "TabListHideLink",
	SelectMasterObj: null,
	SelectSerialObj: null,
	ValidCount: 0,
	CategoryID: 0,
	CurrentCsIDs: "",
	HotOtherDivObj: null,
	TopHeight: 0,
	FirstCsPrice: "",
	AdSerialId: 0, //广告ID
	IsExistAd: false//对比中是否有广告ID
}

// 初始化
function intiPageSelectList() {
	$("select").each(function () {
		// 如果是有效的主品牌下拉框
		if (this.name == "selectMasterbrandForPhotoCompare" && !this.disabled) {
			PhotoComparePageObject.SelectMasterObj = this;
			$(PhotoComparePageObject.SelectMasterObj).change(function () { selectMasterChange(this); });
		}
		// 如果是有效的子品牌下拉框
		if (this.name == "selectSerialForPhotoCompare" && !this.disabled) {
			PhotoComparePageObject.SelectSerialObj = this;
			$(PhotoComparePageObject.SelectSerialObj).change(function () { selectSerialChange(this, -1); });
		}
	});
	if (PhotoComparePageObject.SelectMasterObj && PhotoComparePageObject.SelectSerialObj) {
		// 绑定主品牌列表
		ajaxForBindMasterSelect(PhotoComparePageObject.SelectMasterObj);
	}
	else
	{ /*已对比3个子品牌*/ }
	if (PhotoComparePageObject.ValidCount >= 1 && PhotoComparePageObject.ValidCount <= 2) {
		// 当对比子品牌多余1个少于3个时
		var nextIndex = (PhotoComparePageObject.ValidCount == 1 ? 1 : 2);
		PhotoComparePageObject.HotOtherDivObj
		= $("#waitCompareDiv_" + nextIndex).find(".com_car_item_con:first");
		ajaxForHotOtherSerial();
	}
	if (PhotoComparePageObject.ValidCount == 3) { getSerialAdParameters(); }
	if (PhotoComparePageObject.ValidCount < 2) {
		// 默认8-12万 modified by chengl Aug.15.2012
		var index = 3;
		if (PhotoComparePageObject.FirstCsPrice != ""
		&& PhotoComparePageObject.FirstCsPrice.indexOf("万") > 0) {
			var minPrice = PhotoComparePageObject.FirstCsPrice.substring(0, PhotoComparePageObject.FirstCsPrice.indexOf('万')) * 1;
			if (minPrice < 5) { index = 1; }
			else if (minPrice < 8) { index = 2; }
			else if (minPrice < 12) { index = 3; }
			else if (minPrice < 18) { index = 4; }
			else if (minPrice < 25) { index = 5; }
			else if (minPrice < 40) { index = 6; }
			else if (minPrice < 80) { index = 7; }
			else { index = 8; }
		}
		ajaxForTabListSelectSerial(index);
	}
}

// 绑定主品牌
function ajaxForBindMasterSelect(obj) {
	$.ajax({
		url: PhotoComparePageObject.SelectListInterface + "?type=1&level=1&" + Math.random(),
		type: 'GET',
		dataType: 'html',
		timeout: 1000,
		cache: true,
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		success: function (data) {
			$(obj).html(data);
		}
	});
}

// 绑定子品牌
function ajaxForBindSerialSelect(bsid, obj) {
	$.ajax({
		url: PhotoComparePageObject.SelectListInterface + "?type=1&level=2&masterID=" + bsid,
		type: 'GET',
		dataType: 'html',
		timeout: 1000,
		cache: true,
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		success: function (data) {
			$(obj).html(data);
		}
	});
}
Array.prototype.indexOf = function (e) { for (var i = 0; i < this.length; i++) { if (e === this[i]) { return i; } } return -1; }
//获取广告参数
function getSerialAdParameters() {
	var adParameters = "";
	try {
		if (typeof carCompareAdJson != 'undefined') {
			var cityId = 0;
			if (typeof bit_locationInfo != "undefined")
				cityId = bit_locationInfo.cityId;
			if (carCompareAdJson["0"] != undefined) {
				cityId = "0";
			}
			if (carCompareAdJson[cityId] != undefined) {
				for (var i = 0; i < carCompareAdJson[cityId].length; i++) {
					var serialIdArr = PhotoComparePageObject.CurrentCsIDs.split(',');
					if (serialIdArr.indexOf(carCompareAdJson[cityId][i].carad.serialid.toString()) == -1) {
						PhotoComparePageObject.IsExistAd = false;
						for (var j = 0; j < serialIdArr.length; j++) {
							var tempSerialId = parseInt(serialIdArr[j]);
							if (carCompareAdJson[cityId][i].serialids.indexOf(tempSerialId) != -1) {
								adParameters = "&adSerialId=" + carCompareAdJson[cityId][i].carad.serialid;
								PhotoComparePageObject.AdSerialId = carCompareAdJson[cityId][i].carad.serialid;
								break;
							}
						}
					} else {
						PhotoComparePageObject.AdSerialId = carCompareAdJson[cityId][i].carad.serialid;
						PhotoComparePageObject.IsExistAd = true;
					}
				}
			}
		}
	} catch (e) { }
	return adParameters;
}
// 其他热门对比的子品牌
function ajaxForHotOtherSerial() {
	var adParameters = getSerialAdParameters();
	$.ajax({
		url: PhotoComparePageObject.SelectListInterface + "?type=1&level=3&serialIDs=" + PhotoComparePageObject.CurrentCsIDs + adParameters,
		type: 'GET',
		dataType: 'html',
		timeout: 1000,
		cache: true,
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		success: function (data) {
			if (data != "") {
				var tempArray = new Array();
				tempArray.push("<div class=\"recommendationCar\">");
				tempArray.push("<h5>相关车型推荐</h5>");
				tempArray.push(data);
				tempArray.push("</div>");
				tempArray.push("<div class=\"clear\"></div>");
				$(PhotoComparePageObject.HotOtherDivObj).html(tempArray.join(""));
			}
		}
	});
}

// 绑定价格区间子品牌选择器
function ajaxForTabListSelectSerial(index) {
	PhotoComparePageObject.TabListIndex = index;
	$("#" + PhotoComparePageObject.TabListHideLinkID).removeClass();
	$("#" + PhotoComparePageObject.TabListHideLinkID).addClass("close");

	// 切换 tab
	for (var i = 1; i <= 12; i++) {
		$("#top_" + i).removeClass("current");
	}
	$("#top_" + index).addClass("current");

	$.ajax({
		url: PhotoComparePageObject.TabListInterface + "?type=" + index + "&" + Math.random(),
		type: 'GET',
		dataType: 'html',
		timeout: 1000,
		cache: true,
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		success: function (data) {
			$("#" + PhotoComparePageObject.TabListDivID).html(data);
			$("#" + PhotoComparePageObject.TabListDivID).show();
			resetTopHeightForFloat();
		}
	});
}

// 选子品牌工具 左侧选择
function photeCompareLeftSelect(index) {
	for (var i = 0; i <= 4; i++) {
		$("#SerialListForLevelUL_" + i).hide();
		$("#left_" + i).removeClass("current");
	}
	$("#SerialListForLevelUL_" + index).show();
	$("#left_" + index).addClass("current");
}

// 当选择主品牌时
function selectMasterChange(msObj) {
	var msSelectid = msObj.id;
	var serialSelectid = "";
	if (msSelectid != "") {
		serialSelectid = msSelectid.replace("selectMasterbrand_", "selectSerial_");
	}
	var bsid = $(msObj).val();
	if (bsid * 1 > 0 && serialSelectid != "") {
		ajaxForBindSerialSelect(bsid, $("#" + serialSelectid));
	}
}

// 当选择子品牌时
function selectSerialChange(csObj, index) {
	var csid = $(csObj).val();
	var csName = $(csObj).find("option:selected").text();
	selectThisSerial(csid, csName, index);
}

// 添加子品牌 索引小于0为新加 大于等于0为替换现有对比的子品牌 索引为替换子品牌的索引
function selectThisSerial(csID, csName, index) {
	var strTemp = PhotoComparePageObject.CurrentCsIDs + "";
	var arrIDs = strTemp.split(','), adParameters = "";
	if (strTemp.indexOf(csID) >= 0)
	{ alert("您已经对比了此子品牌。"); return; }
	if (PhotoComparePageObject.ValidCount * 1 >= 3 && index < 0)
	{ alert("最多对比3个子品牌。"); return; }
	//广告车型加广告后缀
	if (PhotoComparePageObject.AdSerialId == csID) adParameters = "&aid=" + PhotoComparePageObject.AdSerialId;
	//不是广告车型但是对比车型中有广告车型 加广告后缀
	if (PhotoComparePageObject.AdSerialId != csID && PhotoComparePageObject.IsExistAd) adParameters = "&aid=" + PhotoComparePageObject.AdSerialId;
	if (strTemp.length > 0) {
		if (index >= 0) {
			// 替换
			if (arrIDs.length > index) {
				arrIDs[index] = csID;
				var tempArray = new Array();
				for (var i = 0; i < arrIDs.length; i++) {
					tempArray.push(arrIDs[i]);
				}
				PhotoComparePageObject.CurrentCsIDs = tempArray.join(",");
			}
		}
		else {
			// 增加
			PhotoComparePageObject.CurrentCsIDs = PhotoComparePageObject.CurrentCsIDs + "," + csID;
		}
	}
	else
	{ PhotoComparePageObject.CurrentCsIDs = csID; }
	var url = "/tupianduibi/?csids=" + PhotoComparePageObject.CurrentCsIDs + "&categoryID=" + PhotoComparePageObject.CategoryID + adParameters;
	window.location.href = url;
	return false;
}

function showOrHideElementById() {
	if ($("#" + PhotoComparePageObject.TabListHideLinkID)[0].className == "close") {
		$("#" + PhotoComparePageObject.TabListDivID).hide();
		$("#" + PhotoComparePageObject.TabListHideLinkID).removeClass();
		$("#" + PhotoComparePageObject.TabListHideLinkID).addClass("show");
		resetTopHeightForFloat();
	}
	else {
		$("#" + PhotoComparePageObject.TabListHideLinkID).removeClass();
		$("#" + PhotoComparePageObject.TabListHideLinkID).addClass("close");
		ajaxForTabListSelectSerial(PhotoComparePageObject.TabListIndex);
	}
}

// 更换子品牌
function changeSerial(index) {
	if ($("#divSelect_" + index)) {
		var tempArray = new Array();
		tempArray.push("<ul>");
		tempArray.push("<li><select name=\"selectMasterbrandForPhotoCompare\" id=\"selectMasterbrand_" + index + "\"><option value=\"-1\">选择品牌</option></select></li>");
		tempArray.push("<li><select name=\"selectSerialForPhotoCompare\" id=\"selectSerial_" + index + "\"><option value=\"-1\">选择车型</option></select></li>");
		tempArray.push("</ul>");
		$("#divSelect_" + index).html(tempArray.join(""));
		// 绑定新主品牌 子品牌 事件
		$("#selectMasterbrand_" + index).change(function () { selectMasterChange(this); });
		$("#selectSerial_" + index).change(function () { selectSerialChange(this, index); });
		ajaxForBindMasterSelect($("#selectMasterbrand_" + index));
	}
}

function addLoadEvent(func) {
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	} else {
		window.onload = function () {
			oldonload();
			func();
		}
	}
}
/*className*/
function getElementsByClass(searchClass, node, tag) {
	var classElements = new Array();
	if (node == null)
		node = document;
	if (tag == null)
		tag = '*';
	var els = node.getElementsByTagName(tag);
	var elsLen = els.length;
	var pattern = new RegExp("(^|\\s)" + searchClass + "(\\s|$)");
	for (var i = 0, j = 0; i < elsLen; i++) {
		if (pattern.test(els[i].className)) {
			classElements[j] = els[i];
			j++;
		}
	}
	return classElements;
}
/*鼠标滑过显示加号*/
function mouseHoverAdd() {
	var allRecommendationCar = getElementsByClass('hasCar', null, 'a');
	for (var i = 0, len = allRecommendationCar.length; i < len; i++) {
		allRecommendationCar[i].onmouseover = function () {
			this.lastChild.style.display = 'block';
		}
		allRecommendationCar[i].onmouseout = function () {
			this.lastChild.style.display = 'none';
		}
	}
}
addLoadEvent(mouseHoverAdd);

/*浮动条*/
function resetTopHeightForFloat() {
	if (!document.getElementById("com_select_car")) return false;
	var comSelectCar = document.getElementById("com_select_car");
	PhotoComparePageObject.TopHeight = 0;
	var tempBar = comSelectCar;
	while (tempBar) {
		PhotoComparePageObject.TopHeight += tempBar.offsetTop;
		tempBar = tempBar.offsetParent;
	}
}

(function () {
	resetTopHeightForFloat();
	var comSelectCar = document.getElementById("com_select_car");
	iE6 = !!window.ActiveXObject && !window.XMLHttpRequest;
	window.onscroll = function () {
		var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
		if (scrollHeight > PhotoComparePageObject.TopHeight) {
			if (iE6) {
				comSelectCar.className = "com_select_car com_select_car_fixed";
				comSelectCar.style.top = (scrollHeight - PhotoComparePageObject.TopHeight - 1) + "px";
			} else {
				comSelectCar.className = "com_select_car com_select_car_fixed";
			}
		} else {
			comSelectCar.className = "com_select_car";
		}
	}
})();