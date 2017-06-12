var PingCeComparePageObject = {
	SelectListInterface: "/car/ajaxnew/SelectListForCompare.aspx",
	TabListInterface: "/car/ajaxnew/ListForPingCeComparev1.aspx",
	TabListIndex: 1,
	TabListDivID: "ListForCompare_divContent",
	TabListHideLinkID: "TabListHideLink",
	SelectMasterObj: null,
	SelectSerialObj: null,
	ValidCount: 0,
	TagID: 0,
	CurrentCsIDs: "",
	TopHeight: 0,
	FirstCsPrice: ""
}

// 初始化
function intiPageSelectList() {
	$("select").each(function () {
		// 如果是有效的主品牌下拉框
		if (this.name == "selectMasterbrandForPhotoCompare" && !this.disabled) {
			PingCeComparePageObject.SelectMasterObj = this;
			$(PingCeComparePageObject.SelectMasterObj).change(function () { selectMasterChange(this); });
		}
		// 如果是有效的子品牌下拉框
		if (this.name == "selectSerialForPhotoCompare" && !this.disabled) {
			PingCeComparePageObject.SelectSerialObj = this;
			$(PingCeComparePageObject.SelectSerialObj).change(function () { selectSerialChange(this, -1); });
		}
	});
	if (PingCeComparePageObject.SelectMasterObj && PingCeComparePageObject.SelectSerialObj) {
		// 绑定主品牌列表
		ajaxForBindMasterSelect(PingCeComparePageObject.SelectMasterObj);
	}
	else
	{ /*已对比2个子品牌*/ }
	if (PingCeComparePageObject.ValidCount < 2) {
		// 默认8-12万 modified by chengl Aug.15.2012
		var index = 3;
		if (PingCeComparePageObject.FirstCsPrice != ""
		&& PingCeComparePageObject.FirstCsPrice.indexOf("万") > 0) {
			var minPrice = PingCeComparePageObject.FirstCsPrice.substring(0, PingCeComparePageObject.FirstCsPrice.indexOf('万')) * 1;
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

// 绑定价格区间子品牌选择器
function ajaxForTabListSelectSerial(index) {
	PingCeComparePageObject.TabListIndex = index;
	$("#" + PingCeComparePageObject.TabListHideLinkID).removeClass();
	$("#" + PingCeComparePageObject.TabListHideLinkID).addClass("close");

	// 切换 tab
	for (var i = 1; i <= 12; i++) {
		$("#top_" + i).removeClass("current");
	}
	$("#top_" + index).addClass("current");

	$.ajax({
		url: PingCeComparePageObject.TabListInterface + "?type=" + index + "&" + Math.random(),
		type: 'GET',
		dataType: 'html',
		timeout: 1000,
		cache: true,
		contentType: "application/x-www-form-urlencoded; charset=utf-8",
		success: function (data) {
			$("#" + PingCeComparePageObject.TabListDivID).html(data);
			$("#" + PingCeComparePageObject.TabListDivID).show();
			resetTopHeightForFloat();
		}
	});
}

// 绑定主品牌
function ajaxForBindMasterSelect(obj) {
	$.ajax({
		url: PingCeComparePageObject.SelectListInterface + "?type=2&level=1&" + Math.random(),
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
		url: PingCeComparePageObject.SelectListInterface + "?type=2&level=2&masterID=" + bsid,
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

// 选子品牌工具 左侧选择
function pingceCompareLeftSelect(index) {
	for (var i = 0; i <= 4; i++) {
		$("#SerialListForLevelUL_" + i).hide();
		$("#left_" + i).removeClass("current");
	}
	$("#SerialListForLevelUL_" + index).show();
	$("#left_" + index).addClass("current");
}

function showOrHideElementById() {
	if ($("#" + PingCeComparePageObject.TabListHideLinkID)[0].className == "close") {
		$("#" + PingCeComparePageObject.TabListDivID).hide();
		$("#" + PingCeComparePageObject.TabListHideLinkID).removeClass();
		$("#" + PingCeComparePageObject.TabListHideLinkID).addClass("show");
		resetTopHeightForFloat();
	}
	else {
		$("#" + PingCeComparePageObject.TabListHideLinkID).removeClass();
		$("#" + PingCeComparePageObject.TabListHideLinkID).addClass("close");
		ajaxForTabListSelectSerial(PingCeComparePageObject.TabListIndex);
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

function selectThisSerial(csID, csName, index) {
	var strTemp = PingCeComparePageObject.CurrentCsIDs + "";
	var arrIDs = strTemp.split(',');
	if (strTemp.indexOf(csID) >= 0)
	{ alert("您已经对比了此子品牌。"); return; }
	if (PingCeComparePageObject.ValidCount >= 2 && index < 0)
	{ alert("最多对比2个子品牌。"); return; }
	if (strTemp.length > 0) {
		if (index >= 0) {
			// 替换
			if (arrIDs.length > index) {
				arrIDs[index] = csID;
				var tempArray = new Array();
				for (var i = 0; i < arrIDs.length; i++) {
					tempArray.push(arrIDs[i]);
				}
				PingCeComparePageObject.CurrentCsIDs = tempArray.join(",");
			}
		}
		else {
			// 增加
			PingCeComparePageObject.CurrentCsIDs = PingCeComparePageObject.CurrentCsIDs + "," + csID;
		}
	}
	else
	{ PingCeComparePageObject.CurrentCsIDs = csID; }
	var url = "/pingceduibi/?csids=" + PingCeComparePageObject.CurrentCsIDs + "&tagID=" + PingCeComparePageObject.TagID;
	window.location.href = "/pingceduibi/?csids=" + PingCeComparePageObject.CurrentCsIDs + "&tagID=" + PingCeComparePageObject.TagID;
}

/*浮动条*/
function resetTopHeightForFloat() {
	if (!document.getElementById("com_select_car")) return false;
	var comSelectCar = document.getElementById("com_select_car");
	PingCeComparePageObject.TopHeight = 0;
	var tempBar = comSelectCar;
	while (tempBar) {
		PingCeComparePageObject.TopHeight += tempBar.offsetTop;
		tempBar = tempBar.offsetParent;
	}
}

(function () {
	resetTopHeightForFloat();
	var comSelectCar = document.getElementById("com_select_car");
	iE6 = !!window.ActiveXObject && !window.XMLHttpRequest;
	window.onscroll = function () {
		var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
		if (scrollHeight > PingCeComparePageObject.TopHeight) {
			if (iE6) {
				comSelectCar.className = "com_select_car com_select_car_fixed";
				comSelectCar.style.top = (scrollHeight - PingCeComparePageObject.TopHeight - 1) + "px";
			} else {
				comSelectCar.className = "com_select_car com_select_car_fixed";
			}
		} else {
			comSelectCar.className = "com_select_car";
		}
	}
})();