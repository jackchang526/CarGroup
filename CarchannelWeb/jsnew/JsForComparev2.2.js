// 车型频道对比
var ComparePageObject = {
	PageDivContentID: "CarCompareContent",  // container id
	PageDivContentObj: null,   // container object
	IsDelSame: false,
	IsOperateTheSame: false,
	AllCarJson: new Array(),
	ArrPageContent: new Array(),
	ArrTempBarHTML: new Array(),
	ArrTempBarForFloatLeftHTML: new Array(),
	FloatTop: new Array(),
	FloatLeft: new Array(),
	ValidCount: 0,
	MaxTDLeft: 5,
	NeedBlockTD: 0,
	DragID: "",
	DropID: "",
	OtherCarInterface: "/car/ajaxnew/GetCarHotCompareListv2.aspx?carid=",
	LastFirstInterface: Array(),
	IsCloseHotList: false,
	CreateSelectChange: new Array(),
	SelectControlTemp: "",
	BSelect: null,
	BaikeObj: null,
	IsShowFloatTop: false
}

function initPageForCompare() {
	intiSelectControl();
	if (document.getElementById(ComparePageObject.PageDivContentID))
	{ ComparePageObject.PageDivContentObj = $("#" + ComparePageObject.PageDivContentID); }
	if (typeof (carCompareJson) != "undefined" && carCompareJson != null)
	{ ComparePageObject.AllCarJson = carCompareJson; }
	ComparePageObject.ValidCount = ComparePageObject.AllCarJson.length;
	ComparePageObject.NeedBlockTD = ComparePageObject.ValidCount >= 5 ? 1 : 5 - ComparePageObject.ValidCount;
	if (ComparePageObject.ValidCount == 10)
	{ ComparePageObject.NeedBlockTD = 0; }
	checkIsCloseHotCompare();
	createPageForCompare(ComparePageObject.IsOperateTheSame);
}

function createPageForCompare(isDelSame) {
	if (ComparePageObject.ValidCount > 0) {
		$("#ListForCompare").hide();
	}
	else {
		pageSelectTagForList(3);
		$("#ListForCompare").show();
	}
	ComparePageObject.IsDelSame = isDelSame;
	var loopCount = arrField.length;
	ComparePageObject.ArrPageContent.push("<table cellspacing=\"0\" cellpadding=\"0\"><tbody>");
	for (var i = 0; i < loopCount; i++) {
		switch (arrField[i].sType) {
			case "fieldPara":
				if (ComparePageObject.ValidCount > 0) createPara(arrField[i]);
				break;
			case "fieldMulti":
				if (ComparePageObject.ValidCount > 0) createMulti(arrField[i]);
				break;
			case "bar":
				if (ComparePageObject.ValidCount > 0) createBar(arrField[i]);
				break;
			case "fieldPrice":
				if (ComparePageObject.ValidCount > 0) createPrice(arrField[i]);
				break;
			case "fieldPic":
				createPic();
				break;
			case "fieldOther":
				if (ComparePageObject.ValidCount > 0) createOther(arrField[i]);
				break;
			case "fieldIndex":
				if (ComparePageObject.ValidCount > 0) createIndex(arrField[i]);
				break;
		}
	}
	ComparePageObject.ArrPageContent.push("</tbody></table>");
	// end
	if (ComparePageObject.PageDivContentObj) {
		ComparePageObject.PageDivContentObj.html(ComparePageObject.ArrPageContent.join(""));
	}
	ComparePageObject.ArrPageContent.length = 0;
	// set img drag
	setImgDrag();
	if (!ComparePageObject.IsCloseHotList) {
		ajaxOtherCarList();
	}
	bindSelectForCompare();
	changeWhenFloatTop();
	if (ComparePageObject.ValidCount > 0) {
		$("#leftfixed").html("<table cellpadding=\"0\" cellspacing=\"0\" class=\"floatTable\">" + ComparePageObject.FloatLeft.join("") + "</table>");
		ComparePageObject.FloatLeft.length = 0;
	}
	changeCheckBoxStateByName("checkboxForDelTheSame", ComparePageObject.IsOperateTheSame);
	setTRColorWhenMouse();
}

// create pic for compare
function createPic() {
	// for FloatTop
	ComparePageObject.FloatTop.length = 0;
	ComparePageObject.FloatTop.push("<table cellpadding=\"0\" cellspacing=\"0\">");
	ComparePageObject.FloatTop.push("<tr>");
	ComparePageObject.FloatTop.push("<th class=\"pd0\">");
	ComparePageObject.FloatTop.push("<div class=\"tableHead_left\">");
	ComparePageObject.FloatTop.push("<input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDifferFloatTop\">");
	ComparePageObject.FloatTop.push("<label for=\"checkDifferFloatTop\">只显示不同点</label>");
	ComparePageObject.FloatTop.push("<div class=\"dashline\"></div>");
	ComparePageObject.FloatTop.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
	ComparePageObject.FloatTop.push("</div>");
	ComparePageObject.FloatTop.push("</th>");

	// for FloatLeft
	ComparePageObject.FloatLeft.length = 0;
	ComparePageObject.FloatLeft.push("<tr>");
	ComparePageObject.FloatLeft.push("<td class=\"pd0\">");
	ComparePageObject.FloatLeft.push("<div class=\"tableHead_left\">");
	ComparePageObject.FloatLeft.push("<dl>");
	ComparePageObject.FloatLeft.push("<dt>对比车型</dt>");
	ComparePageObject.FloatLeft.push("<dd>左右拖动图片可改变车型在列表中的位置</dd>");
	ComparePageObject.FloatLeft.push("</dl>");
	ComparePageObject.FloatLeft.push("<input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDifferFloatLeft\">");
	ComparePageObject.FloatLeft.push("<label for=\"checkDifferFloatLeft\">只显示不同点</label>");
	ComparePageObject.FloatLeft.push("<div class=\"dashline\"></div>");
	ComparePageObject.FloatLeft.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
	ComparePageObject.FloatLeft.push("</div>");
	ComparePageObject.FloatLeft.push("</td>");
	ComparePageObject.FloatLeft.push("</tr>");
	// ComparePageObject.FloatLeft.push("<tr id=\"trOtherCarListForFloatLeft\"></tr>");

	ComparePageObject.ArrPageContent.push("<tr id=\"trForPic\">");
	var classNameAdd = "";
	if (!ComparePageObject.IsCloseHotList) {
		// classNameAdd = " bdbottomnone";
	}
	ComparePageObject.ArrPageContent.push("<th class=\"pd0" + classNameAdd + "\">");
	ComparePageObject.ArrPageContent.push("<div class=\"tableHead_left\">");
	ComparePageObject.ArrPageContent.push("<dl><dt>对比车型</dt><dd>左右拖动图片可改变车型在列表中的位置</dd></dl>");
	ComparePageObject.ArrPageContent.push("<input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDiffer\"> <label for=\"checkDiffer\">只显示不同点</label>");
	ComparePageObject.ArrPageContent.push("<div class=\"dashline\"></div>");
	ComparePageObject.ArrPageContent.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p></div>");
	ComparePageObject.ArrPageContent.push("</th>");

	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		ComparePageObject.ArrPageContent.push("<td class=\"pd0" + classNameAdd + "\" name=\"td" + i + "\" id=\"td_" + i + "\" style=\"position:relative; z-index:" + (10 - i) + "\" >");
		if (i == 0)
		{ ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + i + "\" class=\"tableHead_item tableHead_item_first\">"); }
		else
		{ ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + i + "\" class=\"tableHead_item\">"); }
		var pic = ComparePageObject.AllCarJson[i][0][2] || "";
		pic == "" ? "http://image.bitautoimg.com/autoalbum/V2.1/images/120-80.gif" : pic;
		ComparePageObject.ArrPageContent.push("<div name=\"hasCarBox\" id=\"carBox_" + i + "\" class=\"carBox\">");
		ComparePageObject.ArrPageContent.push("<img id=\"imgdrag_" + i + "\" name=\"imgdrag\" alt=\"按住鼠标左键，可拖动到其他列\" src=\"" + pic + "\">");
		ComparePageObject.ArrPageContent.push("<dl><dt><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/\">" + ComparePageObject.AllCarJson[i][0][5] + "</a></dt>");
		ComparePageObject.ArrPageContent.push("<dd><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/\">" + ComparePageObject.AllCarJson[i][0][1] + (ComparePageObject.AllCarJson[i][0][7] == "" ? "" : " " + ComparePageObject.AllCarJson[i][0][7] + "款") + "</a></dd>");
		ComparePageObject.ArrPageContent.push("</dl></div>");
		ComparePageObject.ArrPageContent.push("<div class=\"optArea\">");

		// for FloatTop
		ComparePageObject.FloatTop.push("<td class=\"pd0\" style=\"position:relative; z-index:" + (10 - i) + "\" >");
		ComparePageObject.FloatTop.push("<div id=\"FloatTop_tableHead_" + i + "\" class=\"tableHead_item\">");
		ComparePageObject.FloatTop.push("<div name=\"hasCarBox\" id=\"FloatTop_carBox_" + i + "\" class=\"carBox\">");
		ComparePageObject.FloatTop.push("<dl><dt><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/\">" + ComparePageObject.AllCarJson[i][0][5] + "</a></dt>");
		ComparePageObject.FloatTop.push("<dd><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/\">" + ComparePageObject.AllCarJson[i][0][1] + (ComparePageObject.AllCarJson[i][0][7] == "" ? "" : " " + ComparePageObject.AllCarJson[i][0][7] + "款") + "</a></dd>");
		ComparePageObject.FloatTop.push("</dl></div>");
		ComparePageObject.FloatTop.push("<div id=\"FloatTop_OptArea_" + i + "\" class=\"optArea\">");

		if (i > 0) {
			ComparePageObject.ArrPageContent.push("<a class=\"left\" href=\"javascript:moveLeftForCarCompare('" + i + "');\">左移</a>");
			// for FloatTop
			ComparePageObject.FloatTop.push("<a class=\"left\" href=\"javascript:moveLeftForCarCompare('" + i + "');\">左移</a>");
		}
		// else {
		//		if (ComparePageObject.IsCloseHotList) {
		ComparePageObject.ArrPageContent.push("<a id=\"pkButton_" + i + "\" class=\"pk\" href=\"javascript:showHotCompare(" + i + ");\">pk</a>");
		//		}
		//		else {
		//			ComparePageObject.ArrPageContent.push("<a style=\"display:none;\" id=\"pkButton_" + i + "\" class=\"pk\" href=\"javascript:showHotCompare(" + i + ");\">pk</a>");
		//		}
		// }
		ComparePageObject.ArrPageContent.push("<a class=\"optBtn mgl20\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a>");
		ComparePageObject.ArrPageContent.push("<a class=\"optBtn\" href=\"javascript:setSelectControlForCompare('tableHead_" + i + "','HeadTemp')\">换车</a>");
		// for FloatTop
		ComparePageObject.FloatTop.push("<a class=\"optBtn mgl20\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a>");
		ComparePageObject.FloatTop.push("<a class=\"optBtn\" href=\"javascript:setSelectControlForCompare('tableHead_" + i + "','HeadTemp')\">换车</a>");
		if (i != ComparePageObject.ValidCount - 1) {
			ComparePageObject.ArrPageContent.push("<a class=\"right\" href=\"javascript:moveRightForCarCompare('" + i + "');\">右移</a>");
			// for FloatTop
			ComparePageObject.FloatTop.push("<a class=\"right\" href=\"javascript:moveRightForCarCompare('" + i + "');\">右移</a>");
		}
		ComparePageObject.ArrPageContent.push("</div>");
		if (i == 0) {
			ComparePageObject.ArrPageContent.push("<div class=\"sx\">首选车型</div>");
		}
		ComparePageObject.ArrPageContent.push("</div>");
		if (i == 0) {
			ComparePageObject.ArrPageContent.push("<div style=\"display:none;\" class=\"firstCar_pop\" id=\"firstCar_pop\"></div>");
		}
		ComparePageObject.ArrPageContent.push("</td>");

		// for FloatTop
		ComparePageObject.FloatTop.push("</div>");
		ComparePageObject.FloatTop.push("</td>");
	}

	//when less 
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrPageContent.push("<td class=\"pd0" + classNameAdd + "\" style=\"position:relative; z-index:" + (ComparePageObject.NeedBlockTD - i) + "\">");
			if (i == 0) {
				ComparePageObject.ArrPageContent.push("<div class=\"tableHead_item\">");
			}
			else {
				ComparePageObject.ArrPageContent.push("<div class=\"tableHead_item noBg\">");
			}
			ComparePageObject.ArrPageContent.push("<div id=\"TopSelect\" class=\"carBox\">");
			ComparePageObject.ArrPageContent.push("<img alt=\"按住鼠标左键，可拖动到其他列\" src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/120-80.gif\">");

			if (i == 0) {
				ComparePageObject.ArrPageContent.push("<div id=\"selectForMaster\"><select class=\"top105 car_select_w\" id=\"master4\" ><option>选择品牌</option></select></div>");
				ComparePageObject.ArrPageContent.push("<div id=\"selectForSerial\"><select class=\"top130 car_select_w\" id=\"serial4\"  onmouseover=\"FixWidth(this)\"><option>选择车型</option></select></div>");
				ComparePageObject.ArrPageContent.push("<div id=\"selectForCar\"><select class=\"top155 car_select_w\"  id=\"cartype4\"  onblur=\"fixWidthNormal(this)\" onmouseover=\"fixWidthAuto(this)\" onchange=\"onchangeCarForSelect(this);\" ><option>选择车款</option></select></div>");

				// for FloatTop
				ComparePageObject.FloatTop.push("<td class=\"pd0\" style=\"position:relative; z-index:" + (ComparePageObject.NeedBlockTD - i) + "\">");
				ComparePageObject.FloatTop.push("<div class=\"tableHead_item\">");
				ComparePageObject.FloatTop.push("<div id=\"FloatTopSelect\" class=\"carBox\">");
				ComparePageObject.FloatTop.push("</div>");
				ComparePageObject.FloatTop.push("</div>");
				ComparePageObject.FloatTop.push("</td>");
			}
			else {
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\" class=\"top105 car_select_w\"><option>选择品牌</option></select>");
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\" class=\"top130 car_select_w\"><option>选择车型</option></select>");
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\" class=\"top155 car_select_w\" ><option>选择车款</option></select>");

				// for FloatTop
				ComparePageObject.FloatTop.push("<td class=\"pd0\"><div class=\"tableHead_item noBg\">");
				ComparePageObject.FloatTop.push("<div class=\"carBox\"><select class=\"top105 car_select_w\" disabled=\"disabled\"><option>选择品牌</option></select><select class=\"top130 car_select_w\" disabled=\"disabled\"><option>选择车型</option></select><select class=\"top155 car_select_w\" disabled=\"disabled\"><option>选择车款</option></select></div>");
				ComparePageObject.FloatTop.push("</div></td>");
			}
			ComparePageObject.ArrPageContent.push("</div></div></td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");

	ComparePageObject.FloatTop.push("</tr>");
	ComparePageObject.FloatTop.push("</table>");
	$("#topfixed").html(ComparePageObject.FloatTop.join(""));
}

function changeWhenFloatTop() {
	if (ComparePageObject.IsShowFloatTop) {
		// show the Float Top
		if (ComparePageObject.AllCarJson.length > 0) {
			$("#selectForMaster").appendTo("#FloatTopSelect");
			$("#selectForSerial").appendTo("#FloatTopSelect");
			$("#selectForCar").appendTo("#FloatTopSelect");
			if (ComparePageObject.ValidCount > 0) {
				for (var i = 0; i < ComparePageObject.ValidCount; i++) {
					if (!document.getElementById("MasterSelectList_HeadTemp_" + i)
					|| document.getElementById("MasterSelectList_HeadTemp_" + i) == null)
					{ continue; }
					// if (typeof (document.getElementById("MasterSelectList_HeadTemp_" + i) != "undefined")) {
					// current change state
					$("#FloatTop_carBox_" + i + " dl").hide();
					$("#MasterSelectList_HeadTemp_" + i).appendTo("#FloatTop_carBox_" + i);
					$("#SerialSelectList_HeadTemp_" + i).appendTo("#FloatTop_carBox_" + i);
					$("#CarTypeSelectList_HeadTemp_" + i).appendTo("#FloatTop_carBox_" + i);
					// $("#FloatTop_OptArea_" + i).hide();
					$("#FloatTop_OptArea_" + i).empty();
				}
			}
		}
	}
	else {
		// hide the Float Top
		if (ComparePageObject.AllCarJson.length > 0) {
			$("#selectForMaster").appendTo("#TopSelect");
			$("#selectForSerial").appendTo("#TopSelect");
			$("#selectForCar").appendTo("#TopSelect");
			if (ComparePageObject.ValidCount > 0) {
				for (var i = 0; i < ComparePageObject.ValidCount; i++) {
					if (!document.getElementById("MasterSelectList_HeadTemp_" + i)
					|| document.getElementById("MasterSelectList_HeadTemp_" + i) == null)
					{ continue; }
					// if (typeof (document.getElementById("MasterSelectList_HeadTemp_" + i) != "undefined")) {
					// current change state
					$("#FloatTop_carBox_" + i + " dl").show();
					$("#MasterSelectList_HeadTemp_" + i).appendTo("#carBox_" + i);
					$("#SerialSelectList_HeadTemp_" + i).appendTo("#carBox_" + i);
					$("#CarTypeSelectList_HeadTemp_" + i).appendTo("#carBox_" + i);
					$("#FloatTop_OptArea_" + i).show();
					// }
				}
			}
		}
	}
}

// create bar for compare
function createBar(arrFieldRow) {
	ComparePageObject.ArrTempBarHTML.length = 0;
	ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
	if (ComparePageObject.ValidCount > 0) {
		ComparePageObject.ArrTempBarHTML.push("<tr>");
		var summaryColumn = 0;
		if (ComparePageObject.ValidCount < 5)
		{ summaryColumn = 6; }
		else
		{ summaryColumn = ComparePageObject.ValidCount + 2; }
		ComparePageObject.ArrTempBarHTML.push("<td class=\"pd0\" colspan=\"" + summaryColumn + "\">");
		ComparePageObject.ArrTempBarHTML.push("<h2><span>" + arrFieldRow["sFieldTitle"] + "</span></h2>");
		ComparePageObject.ArrTempBarHTML.push("</td>");
		ComparePageObject.ArrTempBarHTML.push("</tr>");

		ComparePageObject.ArrTempBarForFloatLeftHTML.push("<tr>");
		ComparePageObject.ArrTempBarForFloatLeftHTML.push("<td class=\"pd0\"><h2><span>" + arrFieldRow["sFieldTitle"] + "</span></h2></td>");
		ComparePageObject.ArrTempBarForFloatLeftHTML.push("</tr>");
	}
}

// create param for compare
function createPara(arrFieldRow) {
	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.AllCarJson[i]) {
			arrTemp.push("<td name=\"td" + i + "\" >");
			try {
				var sTrPrefix = arrFieldRow["sTrPrefix"];
				var index = arrFieldRow["sFieldIndex"];
				if (ComparePageObject.AllCarJson[i].length <= sTrPrefix)
				{ return; }
				if (ComparePageObject.AllCarJson[i][sTrPrefix].length <= index)
				{ return; }
				var field = ComparePageObject.AllCarJson[i][sTrPrefix][index] || "";
				if (field == "待查")
				{ field = ""; }
				if (field.length > 0) {
					if (arrFieldRow["unit"] != "") {
						field += "" + arrFieldRow["unit"];
					}
				}
				if (arrSame.length < 1) {
					arrSame.push(field);
				}
				else {
					if (field == arrSame[0])
					{ firstSame = firstSame && true; }
					else
					{ firstSame = firstSame && false; }
				}
				if (field.indexOf("待查") >= 0 || field == "") {
					isAllunknown = true && isAllunknown;
				}
				else {
					isAllunknown = false;
				}
				if (field.indexOf("有") == 0)
				{ field = "<span class=\"songti\">●</span>"; }
				if (field.indexOf("选配") == 0)
				{ field = "<span class=\"songti\">○</span>"; }
				if (field.indexOf("无") == 0)
				{ field = "<span class=\"songti\">-</span>"; }
				// modified by chengl Dec.28.2009 for calculator
				if (arrFieldRow["sFieldTitle"] == "厂家指导价" && field != "无" && field != "待查") {
					arrTemp.push("<b>" + field + "</b>");
					arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + ComparePageObject.AllCarJson[i][0][0] + "\"  target=\"_blank\"></a>");
				}
				else if (checkTitleIsThreeLines(arrFieldRow["sFieldTitle"])) {
					arrTemp.push("<span class=\"tdThreeLines\" title=\"" + field + "\">" + field + "</span>");
				}
				else if (checkTitleIsTwoLines(arrFieldRow["sFieldTitle"])) {
					arrTemp.push("<span class=\"tdTwoLines\" title=\"" + field + "\">" + field + "</span>");
				}
				else
				{ arrTemp.push(field); }
			}
			catch (err) {
				arrTemp.push("-");
				firstSame = firstSame && false;
			}
			arrTemp.push("</td>");
		}
		else {
			arrTemp.push("<td>&nbsp;");
			arrTemp.push("</td>");
		}
		isShowLoop++;
	}
	if (firstSame && ComparePageObject.IsDelSame) {
		return;
	}
	else {
		if (!isAllunknown) {
			// Is Need Show The Bar
			if (ComparePageObject.ArrTempBarHTML.length > 0) {
				ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
				ComparePageObject.ArrTempBarHTML.length = 0;
			}
			if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
				ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
				ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
			}

			ComparePageObject.ArrPageContent.push("<tr>");
			if (checkTitleIsThreeLines(arrFieldRow["sFieldTitle"])) {
				ComparePageObject.ArrPageContent.push("<th class=\"threeLines\">");
				ComparePageObject.FloatLeft.push("<tr><th class=\"threeLines\">" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th></tr>");
			}
			else {
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th></tr>");
			}
			ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow["sFieldTitle"]));
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
		}
		else {
			return;
		}
	}
	//when less
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if (i > 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"noBg\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			ComparePageObject.ArrPageContent.push("&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create multi param for compare
function createMulti(arrFieldRow) {
	var paramArray = arrFieldRow["sFieldIndex"].split(',');
	var unitArray = arrFieldRow["unit"].split(',');
	var joinCodeArray = arrFieldRow["joinCode"].split(',');
	var pidArray = arrFieldRow["sPid"].split(',');
	var prefixArray = arrFieldRow["sTrPrefix"].split(',');

	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;

	// loop every car
	var tempArrMultField = [];
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.AllCarJson[i]) {
			arrTemp.push("<td name=\"td" + i + "\" >");
			try {
				var multiField = "";
				for (var pint = 0; pint < paramArray.length; pint++) {
					// 组合类型
					if (pidArray[pint] == "577") {
						// 如果当前是 燃油标号 前1项不是汽油的话不显示
						if (multiField.indexOf("汽油") < 0)
						{ continue; }
					}
					if (pidArray[pint] == "408") {
						// 如果自然吸气直接显示，如果是增压则显示增压方式
						if (multiField.indexOf("增压") < 0)
						{ continue; }
						else { multiField = ""; }
					}
					// loop every param
					var sTrPrefix = arrFieldRow["sTrPrefix"];
					var index = paramArray[pint];
					if (ComparePageObject.AllCarJson[i].length <= prefixArray[pint])
					{ return; }
					if (ComparePageObject.AllCarJson[i][prefixArray[pint]].length <= index)
					{ return; }
					var field = ComparePageObject.AllCarJson[i][prefixArray[pint]][index] || "";
					// modified by chengl May.31.2012
					if (pidArray[pint] == "577") {
						// 燃油标号 90改为：89或90；93改为：92或93；97改为：95或97；
						switch (field) {
							case "90号": field = field + "(北京89号)"; break;
							case "93号": field = field + "(北京92号)"; break;
							case "97号": field = field + "(北京95号)"; break;
							default: break;
						}
					}
					if (field == "待查")
					{ field = ""; }
					if (field.length > 0) {
						if (pidArray[pint] == "659") {
							// 百公里等速油耗速度 合并
							if (field == "综合")
							{ field = "(" + field + ")"; }
							else
							{ field = "(" + field + unitArray[pint] + ")"; }
						}
						else {
							field += unitArray[pint];
						}
						multiField = multiField + joinCodeArray[pint] + field;
					}

					if (arrSame.length < 1) {
						arrSame.push(multiField);
					}
					else {
						if (multiField == arrSame[0])
						{ firstSame = firstSame && true; }
						else
						{ firstSame = firstSame && false; }
					}
					if (multiField.indexOf("待查") >= 0 || multiField == "") {
						isAllunknown = true && isAllunknown;
					}
					else {
						isAllunknown = false;
					}
					if (multiField.indexOf("有") == 0)
					{ multiField = "<span class=\"songti\">●</span>"; }
					if (multiField.indexOf("选配") == 0)
					{ multiField = "<span class=\"songti\">○</span>"; }
					if (multiField.indexOf("无") == 0 && multiField.length == 1)
					{ multiField = "<span class=\"songti\">-</span>"; }
				}
				//=================每项多个属性对比 2012-04-11 by songkai===================
				firstSame = true;
				if (ComparePageObject.IsDelSame) {
					tempArrMultField.push(multiField);
					var flag = true;
					if (i == ComparePageObject.ValidCount - 1) {
						for (var k = 0; k < tempArrMultField.length; k++) {
							if (tempArrMultField[k] != multiField) {
								flag = false;
								firstSame = false;
								break;
							}
						}
						if (flag) {
							continue;
						} else {
							//arrTemp.splice(0, arrTemp.length);
							arrTemp = [];
							for (var m = 0; m < tempArrMultField.length; m++) {
								arrTemp.push("<td name=\"td" + m + "\" >");
								arrTemp.push(tempArrMultField[m]);
								arrTemp.push("</td>");
							}
						}
					}
				} else {
					arrTemp.push(multiField);
				}
			}
			catch (err) {
				arrTemp.push("-");
				firstSame = firstSame && false;
			}
			if (!ComparePageObject.IsDelSame) arrTemp.push("</td>");
			//===========================================================
		}
		else {
			arrTemp.push("<td>&nbsp;");
			arrTemp.push("</td>");
		}
		isShowLoop++;
	}
	//-----------
	if (firstSame && ComparePageObject.IsDelSame) {
		return;
	}
	else {
		if (!isAllunknown) {
			// Is Need Show The Bar
			if (ComparePageObject.ArrTempBarHTML.length > 0) {
				ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
				ComparePageObject.ArrTempBarHTML.length = 0;
			}
			if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
				ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
				ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
			}
			ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th></tr>");
			ComparePageObject.ArrPageContent.push("<tr>");
			ComparePageObject.ArrPageContent.push("<th>");
			ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow["sFieldTitle"]));
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			ComparePageObject.LoopTRColor++;
		}
		else {
			return;
		}
	}
	//when less
	if (ComparePageObject.NeedBlockTD < ComparePageObject.MaxTDLeft) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if (i > 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"noBg\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			ComparePageObject.ArrPageContent.push("&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create price for compare
function createPrice(arrFieldRow) {
	ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th></tr>");
	ComparePageObject.ArrPageContent.push("<tr>");
	ComparePageObject.ArrPageContent.push("<th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.AllCarJson[i]) {
			ComparePageObject.ArrPageContent.push("<td name=\"td" + i + "\" >");
			try {
				var sTrPrefix = arrFieldRow["sTrPrefix"];
				var index = arrFieldRow["sFieldIndex"];
				var field = ComparePageObject.AllCarJson[i][sTrPrefix][index] || "";
			}
			catch (err) {
				ComparePageObject.ArrPageContent.push("-");
			}
			if (field.length < 1 || field == "无") {
				ComparePageObject.ArrPageContent.push("无");
			}
			else {
				if (arrFieldRow["sFieldTitle"] == "参考成交价") {
					ComparePageObject.ArrPageContent.push("<span class=\"cRed\"><a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.AllCarJson[i][0][0] + "&citycode=0\">" + field.replace('～', '-') + "</a></span>");
					ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"http://dealer.bitauto.com/zuidijia/nb" + (ComparePageObject.AllCarJson[i][0][3] || "") + "/nc" + ComparePageObject.AllCarJson[i][0][0] + "/\">询最低价>></a></p>");
				}
				else if (arrFieldRow["sFieldTitle"] == "降价优惠") {
					var csAllSpell = ComparePageObject.AllCarJson[i][0][6] || "";
					ComparePageObject.ArrPageContent.push("<span class=\"tdJiangjia\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + csAllSpell + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/jiangjia/\">" + field + "</a></span>");
				}
				else
				{ }
			}
			ComparePageObject.ArrPageContent.push("</td>");
		}
		isShowLoop++;
	}
	//when less
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if (i > 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"noBg\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

// more infomation
function createOther(arrFieldRow) {
	if (ComparePageObject.ArrTempBarHTML.length > 0) {
		ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
		ComparePageObject.ArrTempBarHTML.length = 0;
	}
	if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
		ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
		ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
	}
	ComparePageObject.FloatLeft.push("<tr><th class=\"threeLines\">" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : checkBaikeForTitle(arrFieldRow["sFieldTitle"])) + "</th></tr>");
	// more link
	ComparePageObject.ArrPageContent.push("<tr >");
	ComparePageObject.ArrPageContent.push("<th class=\"threeLines\">" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : checkBaikeForTitle(arrFieldRow["sFieldTitle"])) + "</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.AllCarJson[i]) {
			ComparePageObject.ArrPageContent.push("<td name=\"td" + i + "\" >");
			try {
				var sTrPrefix = arrFieldRow["sTrPrefix"];
				var index = arrFieldRow["sFieldIndex"];
				var csBBS = ComparePageObject.AllCarJson[i][sTrPrefix][index] || "";
				var csAllSpell = ComparePageObject.AllCarJson[i][0][6] || "";
				var csShowName = ComparePageObject.AllCarJson[i][0][5] || "";
				var t1 = csShowName.replace(/([\u0391-\uffe5])/ig, '$1a');
				var t2 = t1.substring(0, 12);
				var tempShowName = t2.replace(/([\u0391-\uffe5])a/ig, '$1');
				if (tempShowName == csShowName)
				{ tempShowName += ""; }
				else
				{ tempShowName += "…"; }
				// csShowName = csShowName.substring(0, 10).replace(/([\u0391-\uffe5])/ig, '$1a').substring(0, c).replace(/([\u0391-\uffe5])a/ig, '$1');
				var csName = ComparePageObject.AllCarJson[i][0][4] || "";
				ComparePageObject.ArrPageContent.push("<p><a title=\"" + csShowName + "\" alt=\"" + csShowName + "\" target=\"_blank\" href=\"/" + csAllSpell + "/koubei/\">" + tempShowName + "口碑</a></p>");
				ComparePageObject.ArrPageContent.push("<p><a title=\"" + csShowName + "\" alt=\"" + csShowName + "\" target=\"_blank\" href=\"/" + csAllSpell + "/youhao/\">" + tempShowName + "油耗</a></p>");
				ComparePageObject.ArrPageContent.push("<p><a title=\"" + csShowName + "\" alt=\"" + csShowName + "\" target=\"_blank\" href=\"" + csBBS + "\">" + tempShowName + "论坛</a></p>");
			}
			catch (err) {
				ComparePageObject.ArrPageContent.push("-");
			}
			ComparePageObject.ArrPageContent.push("</td>");
		}
		isShowLoop++;
	}
	//when less
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if (i > 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"noBg\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function createIndex(arrFieldRow) {
	if (ComparePageObject.ArrTempBarHTML.length > 0) {
		ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
		ComparePageObject.ArrTempBarHTML.length = 0;
	}
	if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
		ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
		ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
	}
	ComparePageObject.FloatLeft.push("<tr class=\"car-focus-index\"><th>" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : arrFieldRow["sFieldTitle"]) + "</th></tr>");
	// more link
	ComparePageObject.ArrPageContent.push("<tr class=\"car-focus-index\">");
	ComparePageObject.ArrPageContent.push("<th>" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : arrFieldRow["sFieldTitle"]) + "</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		var csid = ComparePageObject.AllCarJson[i][0][3] != "undefined" ? ComparePageObject.AllCarJson[i][0][3] : "";
		var csLevel = ComparePageObject.AllCarJson[i][0][12] != "undefined" ? ComparePageObject.AllCarJson[i][0][12] : "";
		if (ComparePageObject.AllCarJson[i]) {
			ComparePageObject.ArrPageContent.push("<td name=\"td" + i + "\" >");
			try {
				ComparePageObject.ArrPageContent.push("<span class=\"tdTwoLines\">");
				if (carIndexJson != "undefined") {
					if (carIndexJson[csid] != "undefined") {
						var uvRank = 0;
						var saleRank = 0;
						if (carIndexJson[csid].UVRank != "undefined") {
							uvRank = carIndexJson[csid].UVRank;
						}
						if (carIndexJson[csid].SaleRank != "undefined") {
							saleRank = carIndexJson[csid].SaleRank;
							ComparePageObject.ArrPageContent.push("");
						}
						if (uvRank > 0) {
							ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"http://index.bitauto.com/guanzhu/s" + csid + "/\">" + csLevel + "关注</a>");
							ComparePageObject.ArrPageContent.push("<span>第<em>" + uvRank + "</em>名</span>");
							ComparePageObject.ArrPageContent.push("</p>");
						}
						else
						{ ComparePageObject.ArrPageContent.push("<p>暂无关注数据</p>"); }
						if (saleRank > 0) {
							ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"http://index.bitauto.com/xiaoliang/s" + csid + "/\">" + csLevel + "销量</a>");
							ComparePageObject.ArrPageContent.push("<span>第<em>" + saleRank + "</em>名</span>");
							ComparePageObject.ArrPageContent.push("</p>");
						}
						else
						{ ComparePageObject.ArrPageContent.push("<p>暂无销量数据</p>"); }
					}
				}
				ComparePageObject.ArrPageContent.push("</span>");
			}
			catch (err) {
				ComparePageObject.ArrPageContent.push("-");
			}
			ComparePageObject.ArrPageContent.push("</td>");
		}
		isShowLoop++;
	}
	//when less
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if (i > 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"noBg\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

// when match title add link to baike
function checkBaikeForTitle(title) {
	if (ComparePageObject.BaikeObj == null
	&& typeof (CarChannelBaikeJson) != "undefined"
	&& CarChannelBaikeJson.length > 0) {
		ComparePageObject.BaikeObj = new Object;
		for (var i = 0; i < CarChannelBaikeJson.length; i++) {
			for (var key in CarChannelBaikeJson[i]) {
				ComparePageObject.BaikeObj[key] = CarChannelBaikeJson[i][key];
			}
		}
	}
	if (ComparePageObject.BaikeObj
	&& ComparePageObject.BaikeObj[title]) {
		return "<a href='" + ComparePageObject.BaikeObj[title] + "' target='_blank'>" + title + "</a>";
	}
	else
	{ return title; }
}

function checkTitleIsThreeLines(sFieldTitle) {
	var isThreeLines = false;
	if (sFieldTitle == "车身颜色" || sFieldTitle == "座椅颜色" || sFieldTitle == "内饰颜色") {
		isThreeLines = true;
	}
	return isThreeLines;
}

function checkTitleIsTwoLines(sFieldTitle) {
	var isTwoLines = false;
	if (sFieldTitle == "保修政策"
	|| sFieldTitle == "特有技术"
	|| sFieldTitle == "发动机型号"
	|| sFieldTitle == "主动安全-其他"
	|| sFieldTitle == "后悬挂类型"
	|| sFieldTitle == "前悬挂类型") {
		isTwoLines = true;
	}
	return isTwoLines;
}

// add car to compare
function addCarToCompareForSelect(id, name, csName) {
	var newCarID = new Array();
	if (ComparePageObject.AllCarJson.length > 0) {
		if (ComparePageObject.AllCarJson.length == 10) {
			alert("对比车型不能多于10个");
			return;
		}
		for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
			if (id == ComparePageObject.AllCarJson[i][0][0]) {
				alert("您选择的车型,已经在对比列表中!");
				return;
			}
			newCarID.push(ComparePageObject.AllCarJson[i][0][0]);
		}
	}
	newCarID.push(id);
	document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "#CarHotCompareList";
}

function addAllCarToCompare() {
	var currentArray = new Array();
	var tempArray = new Array();
	var endArray = new Array();
	$('#firstCar_pop ul').each(function () {
		if (this.style.display != "none") {
			$(this).find('li').each(function () {
				tempArray.push(this.id.replace("liHot_", "").replace("liSame_", ""));
			})
		}
	});
	if (tempArray.length > 0) {
		if (ComparePageObject.AllCarJson.length > 0) {
			for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
				if (("," + endArray.join(",") + ",").indexOf(ComparePageObject.AllCarJson[i][0][0]) < 0) {
					endArray.push(ComparePageObject.AllCarJson[i][0][0]);
					currentArray.push(ComparePageObject.AllCarJson[i][0][0]);
				}
			}
			for (var i = 0; i < tempArray.length; i++) {
				if (("," + endArray.join(",") + ",").indexOf(tempArray[i]) < 0) {
					endArray.push(tempArray[i]);
				}
			}
		}
	}
	if (endArray.length > 10)
	{ alert("对比车型不能多于10个"); }
	else if (endArray.length > 0 && currentArray.join(",") != endArray.join(",")) {
		closeHotCompare();
		// 推荐类不统计
		document.location.href = "/chexingduibi/?carIDs=" + endArray.join(",") + "&isrec=1#CarHotCompareList";
	}
	else
	{ }
}

function checkIsCloseHotCompare() {
	if (CookieForTempSave.getCookie("UserCloseCarList") == "1")
	{ ComparePageObject.IsCloseHotList = true; }
}

function closeHotCompare() {
	// $("#trOtherCarListForFloatLeft").empty();
	$(".firstCar_pop").hide();
	//	$("#trForPic td,#trForPic th").each(function () {
	//		$(this).removeClass("bdbottomnone");
	//	});
	CookieForTempSave.setCookie("UserCloseCarList", "1");
	ComparePageObject.IsCloseHotList = true;
	$("#pkButton").show();
}

function showHotCompare(index) {
	$(".firstCar_pop").show();
	CookieForTempSave.clearCookie("UserCloseCarList");
	ComparePageObject.IsCloseHotList = false;
	// $("#pkButton_" + index).hide();
	ajaxOtherCarList(index);
}

// del car from compare
function delCarToCompare(index) {
	var newCarIDArr = new Array();
	if (ComparePageObject.ValidCount < 1) {
		alert('没有可删的了');
		return;
	}
	if (index >= 0 && ComparePageObject.ValidCount > index && ComparePageObject.AllCarJson.length > index) {
		// del Array index object
		ComparePageObject.AllCarJson.splice(index, 1);
		if (ComparePageObject.AllCarJson.length == 1) {
			ComparePageObject.IsOperateTheSame = false;
		}
		initPageForCompare();
	}
}

function moveLeftForCarCompare(index) {
	if (index > 0 && ComparePageObject.ValidCount > index && ComparePageObject.AllCarJson.length > index) {
		swapArray(ComparePageObject.AllCarJson, index - 1, index);
	}
}

function moveRightForCarCompare(index) {
	if (index >= 0 && (ComparePageObject.ValidCount - 1) > index && (ComparePageObject.AllCarJson.length - 1) > index) {
		swapArray(ComparePageObject.AllCarJson, index, index * 1 + 1);
	}
}

// swap Array object and reshow compare content
function swapArray(obj, index1, index2) {
	var temp = obj[index1];
	obj[index1] = obj[index2];
	obj[index2] = temp;
	createPageForCompare(ComparePageObject.IsOperateTheSame);
}

// 排除相同项
function delTheSameForCompare() {
	if (ComparePageObject.ValidCount > 1) {
		if (!ComparePageObject.IsOperateTheSame) {
			createPageForCompare(true);
			ComparePageObject.IsOperateTheSame = true;
			changeCheckBoxStateByName("checkboxForDelTheSame", true);
		}
		else {
			createPageForCompare(false);
			ComparePageObject.IsOperateTheSame = false;
			changeCheckBoxStateByName("checkboxForDelTheSame", false);
		}
	}
}

function changeCheckBoxStateByName(name, state) {
	var checkBoxs = document.getElementsByName(name);
	if (checkBoxs && checkBoxs.length > 0) {
		for (var i = 0; i < checkBoxs.length; i++) {
			checkBoxs[i].checked = state;
		}
	}
}
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
function bindSelectForCompare() {
	var mdvalue = "0", sdvalue = "0";
	if (CookieForTempSave.getCookie("TempSelectMasterID") != "") {
		mdvalue = CookieForTempSave.getCookie("TempSelectMasterID");
		if (CookieForTempSave.getCookie("TempSelectSerialID") != "")
		{ sdvalue = CookieForTempSave.getCookie("TempSelectSerialID"); }
	}
	//modified by sk 2013.03.27 广告
	var serialId = 0;
	if (typeof ComparePageObject.AllCarJson[0] != "undefined"
		&& typeof ComparePageObject.AllCarJson[0][0] != "undefined"
		&& ComparePageObject.AllCarJson[0][0][3] != "undefined") {
		serialId = ComparePageObject.AllCarJson[0][0][3];
	}
	if (ComparePageObject.ValidCount == 1
		&& typeof bit_locationInfo != "undefined"
		&& typeof carCompareAdJson != "undefined") {
		var cityId = bit_locationInfo.cityId;
		if (carCompareAdJson["0"] != undefined) {
			cityId = "0";
		}
		if (carCompareAdJson[cityId] != undefined) {
			for (var i = 0; i < carCompareAdJson[cityId].length; i++) {
				if (carCompareAdJson[cityId][i].serialids.indexOf(serialId) != -1) {
					mdvalue = carCompareAdJson[cityId][i].carad.masterid;
					sdvalue = carCompareAdJson[cityId][i].carad.serialid;
					break;
				}
			}
		}
	}
	ComparePageObject.BSelect = new BindSelect({
		container: { master: "master4", serial: "serial4", cartype: "cartype4" },
		include: { serial: "1", cartype: "1" },
		dvalue: { master: mdvalue, serial: sdvalue },
		deftext: {
			master: { "value": "0", "text": "选择品牌" },
			serial: { "value": "0", "text": "选择车型" },
			cartype: { "value": "0", "text": "选择车款" }
		},
		groupoprtionstyle: { "style": "normal", "color": "#CCCCCC", "align": "center" },
		background: "#ECF3F9"
	});
	ComparePageObject.BSelect.BindList();
	//	ComparePageObject.BSelect = new Version2BindSelect(BindSelectData);
	//	if (CookieForTempSave.getCookie("TempSelectMasterID") != "") {
	//		BindSelectData["selectlist"]["master"]["initValue"] = CookieForTempSave.getCookie("TempSelectMasterID");
	//		if (CookieForTempSave.getCookie("TempSelectSerialID") != "")
	//		{ BindSelectData["selectlist"]["serial"]["initValue"] = CookieForTempSave.getCookie("TempSelectSerialID"); }
	//	}
	//	ComparePageObject.BSelect.BindList();
}

function onchangeCarForSelect(selectObj) {
	var selectBsID = ComparePageObject.BSelect.GetValue("master");
	var selectCsID = ComparePageObject.BSelect.GetValue("serial");
	CookieForTempSave.setCookie("TempSelectMasterID", selectBsID);
	CookieForTempSave.setCookie("TempSelectSerialID", selectCsID);
	addCarToCompareForSelect(ComparePageObject.BSelect.GetValue("cartype"));
	fixWidthNormal(selectObj);
}

function setSelectControlForCompare(objID, suffix) {
	if (objID.indexOf('_') >= 0) {
		var index = objID.substring(objID.indexOf('_') + 1, objID.length);
		$("#" + objID).empty();
		$("#" + objID).html("<div id=\"carBox_" + index + "\" class=\"carBox\"><img src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/120-80.gif\" alt=\"按住鼠标左键，可拖动到其他列\"/><select id=\"MasterSelectList" + "_" + suffix + "_" + index + "\" class=\"top105 car_select_w\"></select><select id=\"SerialSelectList" + "_" + suffix + "_" + index + "\" class=\"top130 car_select_w\"></select><select onchange=\"onchangeCarForSelectByIndex('" + index + "');\" id=\"CarTypeSelectList" + "_" + suffix + "_" + index + "\" class=\"top155 car_select_w\"></select></div>");
		// $("#" + objID).append("<div id=\"carBox_" + index + "\" class=\"carBox\"><img src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/120-80.gif\" alt=\"按住鼠标左键，可拖动到其他列\"/><select id=\"MasterSelectList" + "_" + suffix + "_" + index + "\"></select><select id=\"SerialSelectList" + "_" + suffix + "_" + index + "\"></select><select onchange=\"onchangeCarForSelectByIndex('" + index + "');\" id=\"CarTypeSelectList" + "_" + suffix + "_" + index + "\"></select></div>");
		var currentSelect = ComparePageObject.SelectControlTemp;
		while (currentSelect.indexOf("_ParamSuffix") != -1) {
			currentSelect = currentSelect.replace("_ParamSuffix", "_" + suffix + "_" + index);
		}
		while (currentSelect.indexOf("_BsSuffix") != -1) {
			currentSelect = currentSelect.replace("_BsSuffix", "_" + suffix + "_" + index);
		}
		while (currentSelect.indexOf("_CsSuffix") != -1) {
			currentSelect = currentSelect.replace("_CsSuffix", "_" + suffix + "_" + index);
		}
		while (currentSelect.indexOf("_CarSuffix") != -1) {
			currentSelect = currentSelect.replace("_CarSuffix", "_" + suffix + "_" + index);
		}
		while (currentSelect.indexOf("[index]") != -1) {
			currentSelect = currentSelect.replace("[index]", "[" + index + "]");
		}
		eval(currentSelect);
		if (ComparePageObject.IsShowFloatTop)
		{ changeWhenFloatTop(); }
	}
}

function onchangeCarForSelectByIndex(index) {
	var selectBsID = ComparePageObject.CreateSelectChange[index].GetValue("master");
	var selectCsID = ComparePageObject.CreateSelectChange[index].GetValue("serial");
	CookieForTempSave.setCookie("TempSelectMasterID", selectBsID);
	CookieForTempSave.setCookie("TempSelectSerialID", selectCsID);
	var newid = ComparePageObject.CreateSelectChange[index].GetValue("cartype");
	var newCarID = new Array();
	if (ComparePageObject.AllCarJson.length > 0) {
		for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
			if (ComparePageObject.AllCarJson[i][0][0] == newCarID) {
				alert("您选择的车型,已经在对比列表中!");
				return;
			}
			else
			{ newCarID.push(ComparePageObject.AllCarJson[i][0][0]); }
		}
	}

	if (newCarID.length > index) {

		for (var i = 0; i < newCarID.length; i++) {
			if (i == index) {
				// current index change
				newCarID[i] = newid;
				break;
			}
		}
	}
	document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "#CarHotCompareList";
}

function ajaxOtherCarList(index) {
	if (ComparePageObject.AllCarJson.length > 0 && ComparePageObject.AllCarJson.length > index) {
		if (ComparePageObject.LastFirstInterface.length > 0) {
			if (ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]]) {
				// 请求过
				$("#firstCar_pop").show();
				$("#firstCar_pop").html(ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]]);
				if (index >= 5) {
					$("#firstCar_pop")[0].className = 'firstCar_pop firstCar_pop_border_right car' + (index + 1);
					$(".tipbox_direction_up")[0].className = 'tipbox_direction_up car5';
				}
				else {
					$("#firstCar_pop")[0].className = 'firstCar_pop';
					$(".tipbox_direction_up")[0].className = 'tipbox_direction_up car' + ((index % 5) + 1);
				}
				return;
			}
		}

		$.ajax({
			url: ComparePageObject.OtherCarInterface + ComparePageObject.AllCarJson[index][0][0],
			type: 'GET',
			dataType: 'html',
			timeout: 1000,
			success: function (data) {
				$("#firstCar_pop").show();
				$("#firstCar_pop").html(data);
				if (index >= 5) {
					$("#firstCar_pop")[0].className = 'firstCar_pop firstCar_pop_border_right car' + (index + 1);
					$(".tipbox_direction_up")[0].className = 'tipbox_direction_up car5';
				}
				else {
					$("#firstCar_pop")[0].className = 'firstCar_pop';
					$(".tipbox_direction_up")[0].className = 'tipbox_direction_up car' + ((index % 5) + 1);
				}
				ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]] = data;
			}
		});
	}
}

// set img drag
function setImgDrag() {
	$("img[name='imgdrag']").each(function (i) {
		$(this).draggable({
			proxy: 'clone',
			revert: true,
			cursor: 'move',
			onStartDrag: function () {
				$(this).draggable('proxy').css({ 'z-index': '999' });
				// $(this).draggable('options').cursor = 'move';
				ComparePageObject.DragID = "";
				if (typeof (this.id) != "undefined" && this.id) {
					ComparePageObject.DragID = this.id.replace("imgdrag_", "");
				}
			},
			onStopDrag: function () {
				$(this).css({ 'left': '0px', 'top': '0px' });
				// $(this).draggable('options').cursor = 'move';
				$(".carBox").each(function () {
					$(this).removeClass('moving');
				});
				if (ComparePageObject.DragID != "" && ComparePageObject.DropID != "" && ComparePageObject.DragID != ComparePageObject.DropID) {
					swapArray(ComparePageObject.AllCarJson, ComparePageObject.DragID, ComparePageObject.DropID);
				}
				ComparePageObject.DragID = "";
				ComparePageObject.DropID = "";
			}
		});
	});

	$("div[name='hasCarBox']").droppable({
		onDragEnter: function (e, source) {
			$(this).addClass('moving');
		},
		onDragLeave: function (e, source) {
			$(this).removeClass('moving');
		},
		onDrop: function (e, source) {
			$(source).draggable('proxy').remove();
			ComparePageObject.DropID = "";
			if (typeof (this.id) != "undefined" && this.id) {
				ComparePageObject.DropID = this.id.replace("carBox_", "");
			}
		}
	});
}

// Fix select Width for IE6
function FixWidth(selectObj) {
	//	if ($.browser.msie && $.browser.version == "6.0") {
	//		// ie6
	//		if (selectObj.id == "cartype4") {
	//			if (selectObj.className != "undefined") {
	//				if (selectObj.className.indexOf("car_select_w") >= 0) {
	//					$(selectObj).removeClass("car_select_w");
	//					$(selectObj).addClass("car_select_auto");
	//				}
	//				else {
	//					$(selectObj).removeClass("car_select_auto");
	//					$(selectObj).addClass("car_select_w");
	//				}
	//			}
	//			else
	//			{ alert("undefined"); }
	//		}
	//	}
	//	else {
	//		// not ie6
	//	}
}

function fixWidthAuto(selectObj) {
	if ($.browser.msie && $.browser.version == "6.0") {
		$(selectObj).removeClass("car_select_w");
		$(selectObj).addClass("car_select_auto");
	}
}

function fixWidthNormal(selectObj) {
	if ($.browser.msie && $.browser.version == "6.0") {
		$(selectObj).removeClass("car_select_auto");
		$(selectObj).addClass("car_select_w");
	}
}

function intiSelectControl() {
	var tempLate = new Array();
	//2012-04-27 替换下拉列表调用方式 by sk
	tempLate.push('ComparePageObject.CreateSelectChange[index] = new BindSelect({');
	tempLate.push('	container: { master: "MasterSelectList_BsSuffix", serial: "SerialSelectList_CsSuffix", cartype: "CarTypeSelectList_CarSuffix" },');
	tempLate.push('	include: { serial: "1", cartype: "1" },');
	tempLate.push('	deftext: {');
	tempLate.push('		master: { "value": "0", "text": "选择品牌" },');
	tempLate.push('		serial: { "value": "0", "text": "选择车型" },');
	tempLate.push('		cartype: { "value": "0", "text": "选择车款" }');
	tempLate.push('	},');
	tempLate.push('	groupoprtionstyle: { "style": "normal", "color": "#CCCCCC", "align": "center" },');
	tempLate.push('	background: "#ECF3F9"');
	tempLate.push('});');
	if ($.browser.msie) {
		tempLate.push("window.setTimeout(function(){ComparePageObject.CreateSelectChange[index].BindList();},1);");
	}
	else {
		tempLate.push("ComparePageObject.CreateSelectChange[index].BindList();");
	}
	ComparePageObject.SelectControlTemp = tempLate.join("");
}


//--------------------  Cookie  --------------------
var CookieForTempSave = {
	setCookie: function (name, value) {
		document.cookie = name + "=" + escape(value) + "; domain=car.bitauto.com";
	},

	getCookie: function (name) {
		var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

		if (arr != null) {
			return unescape(arr[2]);
		}
		return null;
	},

	clearCookie: function (name) {
		if (CookieForTempSave.getCookie(name)) {
			document.cookie = name + "=;domain=car.bitauto.com";
		}
	}
};

function setTRColorWhenMouse() {
	$('#CarCompareContent tr:gt(0)').hover(
		function () {
			$(this).css({ 'background': '#F8F8F8' });
		},
		function () {
			$(this).css({ 'background': '#FFFFFF' });
		}
	);
}

$(function () {
	var theid = $("#topfixed");
	var the_lid = $("#leftfixed");
	var thebox = $("#box");
	var thesmall = $("#smallfixed")
	var floatkey;
	//////20110811修改隐藏显示浮动层
	var idmainoffsettop = $("#main_box").offset().top; //id的 offsettop
	var idmainoffsettop_top = idmainoffsettop + 90 //上浮动层出现top定位
	var idleftoffsetheight = $("#tableHead_left").height(); //左侧浮动层出现的top定位
	var idleftwidth = the_lid.width(); //左侧浮动层的宽度
	////////////////屏幕改变大小开始
	$(window).resize(function () {
		var boxoffset = thebox.offset();
		var boxoffsetLeft = boxoffset.left; //计算box的offleft值
		var scrollsLeft = $(window).scrollLeft(); //计算窗口左卷动值
		var scrolls = $(this).scrollTop();
		////////////left 浮动模开始式///////////////////////////
		if (scrollsLeft > boxoffsetLeft) {//当窗口大小改变时，如果窗口左滚动大于左侧块左位移
			if (window.XMLHttpRequest) {	//非IE6	
				the_lid.css({
					position: "fixed",
					top: (idmainoffsettop - scrolls), //+1
					left: 0,
					display: "block"
				});
			} else {//IE6
				the_lid.css({
					position: "absolute",
					top: 0,
					left: scrollsLeft - boxoffsetLeft,
					display: "block"
				});
			}
		} else {
			the_lid.css({
				display: "none"
			});
		}
		////////////left浮动模式结束//////////////////
		////////////左上角开始
		if (scrolls > idmainoffsettop_top && scrollsLeft > boxoffsetLeft) {
			if (window.XMLHttpRequest) {
				thesmall.css({
					position: "fixed",
					left: 0,
					top: 0,
					display: "block"
				});
			} else {//IE
				thesmall.css({
					position: "absolute",
					top: scrolls,
					left: scrollsLeft,
					display: "block"
				});
			}
		} else {
			thesmall.css({
				display: "none"
			});
		}
		////////////左上角结束
		if (floatkey) {//如果是在浮动状态
			//如果box的offsetleft =0 说明窗口小， 那么定位left=0 或者 负的leftscroll
			//如果box的offsetleft >0 说明窗口大，那么定位left=offsetleft 或者 leftscroll-offsetleft
			if (boxoffsetLeft == 0) {//窗口小
				if (scrollsLeft > 0) {
					if (window.XMLHttpRequest) {
						theid.css({
							left: -scrollsLeft
						});
					}
					else {//IE
						theid.css({
							left: 0
						});
					}
				} else {
					theid.css({
						left: 0
					});
				}
			}
			if (boxoffsetLeft > 0) {//窗口大
				if (scrollsLeft < boxoffsetLeft) {
					if (window.XMLHttpRequest) {
						theid.css({
							left: boxoffsetLeft - scrollsLeft
						});
					} else {
						theid.css({
							left: boxoffsetLeft
						});
					}
				} else {
					if (window.XMLHttpRequest) {
						theid.css({
							left: -(scrollsLeft - boxoffsetLeft)
						});
					} else {
						theid.css({
							left: boxoffsetLeft
						});
					}
				}
			}
		}
	});
	////////////////屏幕改变大小结束
	///////////////////屏幕卷动
	$(window).scroll(function () {
		var scrolls = $(this).scrollTop(); //窗口上卷动
		var scrollsLeft = $(this).scrollLeft(); //窗口左卷动值
		var boxoffset = thebox.offset();
		var boxoffsetLeft = boxoffset.left; //计算box的offleft值
		////////////左上角开始
		if (scrolls > idmainoffsettop_top && scrollsLeft > boxoffsetLeft) {
			if (window.XMLHttpRequest) {
				thesmall.css({
					position: "fixed",
					left: 0,
					top: 0,
					display: "block"
				});
			} else {//IE
				thesmall.css({
					position: "absolute",
					top: scrolls,
					left: scrollsLeft,
					display: "block"
				});
			}
		} else {
			thesmall.css({
				display: "none"
			});
		}
		////////////左上角结束
		////////////left 浮动模开始式///////////////////////////
		if (scrollsLeft > boxoffsetLeft) {//当窗口大小改变时，如果窗口左滚动大于左侧块左位移
			if (window.XMLHttpRequest) {	//非IE6	
				the_lid.css({
					position: "fixed",
					top: (idmainoffsettop - scrolls), //+1
					left: 0,
					display: "block"
				});
			} else {//IE6
				the_lid.css({
					position: "absolute",
					top: 0,
					left: scrollsLeft - boxoffsetLeft,
					display: "block"
				});
			}
		} else {
			the_lid.css({
				display: "none"
			});
		}
		////////////left浮动模式结束///////////////////
		////////////////控制上下卷动屏幕，出现浮动效果
		if (scrolls > idmainoffsettop_top && ComparePageObject.ValidCount > 0) {//如果向上滚动大于id的top位置
			floatkey = true; //开启浮动模式
			if (!ComparePageObject.IsShowFloatTop) {
				ComparePageObject.IsShowFloatTop = true;
				changeWhenFloatTop();
			}
			if (window.XMLHttpRequest) {//非IE6						 	
				theid.css({
					position: "fixed",
					top: "0",
					left: boxoffsetLeft,
					display: "block"
				});
			} else {//IE6				 
				theid.css({
					position: "absolute",
					top: scrolls,
					left: boxoffsetLeft,
					display: "block"
				});
			}
		}
		else if (scrolls <= idmainoffsettop_top) {//如果向上滚动小于id的top位置
			floatkey = false; //关闭浮动模式。
			if (ComparePageObject.IsShowFloatTop) {
				ComparePageObject.IsShowFloatTop = false;
				changeWhenFloatTop();
			}
			theid.css({
				position: "relative",
				left: "0",
				top: 0,
				display: "none"
			});
		}
		/////////////////////控制左右卷动屏幕的效果	
		if (floatkey) {//如果处在浮动状态
			if (scrollsLeft > 0 && boxoffsetLeft > 0) {//有左滚动，窗口大于页面宽度
				if (window.XMLHttpRequest) {	//非IE6	
					theid.css({
						left: boxoffsetLeft - scrollsLeft
					});
				} else {//IE6
					theid.css({
						left: boxoffsetLeft
					});
				}
			}
			if (scrollsLeft > 0 && boxoffsetLeft == 0) {//有左滚动，窗口小于页面宽度
				if (window.XMLHttpRequest) {	//非IE6					
					theid.css({
						left: -scrollsLeft
					});
				} else {//IE6
					theid.css({
						left: boxoffsetLeft
					});
				}
			}
			if (scrollsLeft == 0) {//无左滚动，窗口小于或者大于页面宽度。或者拉到最左边。
				theid.css({
					left: boxoffsetLeft //left数值等于id原有的offsetleft
				});
			}
		}
	});
	///////////////////////屏幕卷动结束
});

// page method --------------------------
var arrField = [
   { sType: "fieldPic", sFieldIndex: "", sFieldTitle: "图片", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "厂家指导价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "1", sFieldTitle: "参考成交价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "7", sFieldTitle: "降价优惠", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldAvgPrice", sFieldIndex: "0", sFieldTitle: "参考成交价", sPid: "", sTrPrefix: "0", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "保修政策", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "排量（升）", sPid: "785", sTrPrefix: "1", unit: "L", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "4,5", sFieldTitle: "变速器型式", sPid: "724,712", sTrPrefix: "1,1", unit: "挡,", joinCode: ", " },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本性能", sPid: "", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "综合工况油耗", sPid: "782", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "市区工况油耗", sPid: "783", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "市郊工况油耗", sPid: "784", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "3,21", sFieldTitle: "百公里等速油耗", sPid: "658,659", sTrPrefix: "2,2", unit: "L,km/h", joinCode: "," },
// { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "百公里等速油耗速度", sPid: "659", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   {sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "CNCAP市区工况油耗", sPid: "854", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "CNCAP市郊工况油耗", sPid: "855", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "三部委检测油耗", sPid: "862", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "网友油耗", sPid: "", sTrPrefix: "0", unit: "/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "最大爬坡度", sPid: "661", sTrPrefix: "2", unit: "%", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "加速时间(0—100 km/h)", sPid: "650", sTrPrefix: "2", unit: "s", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "制动距离(100—0 km/h)", sPid: "653", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "最小转弯半径", sPid: "590", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "最大涉水深度", sPid: "662", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "驱动方式", sPid: "655", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "乘员人数（含司机）", sPid: "665", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "整备质量", sPid: "669", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "满载质量", sPid: "668", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "最高车速", sPid: "663", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "通过角", sPid: "890", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "车身结构", sPid: "", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车门数", sPid: "563", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车身型式", sPid: "574", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "天窗型式", sPid: "567", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "外部尺寸", sPid: "", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "长", sPid: "588", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "宽", sPid: "593", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "高", sPid: "586", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "轴距", sPid: "592", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前轮距", sPid: "585", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后轮距", sPid: "582", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "接近角", sPid: "591", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "离去角", sPid: "581", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "最小离地间隙", sPid: "589", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "内部尺寸", sPid: "", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "第三排腿部空间", sPid: "462", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "行李箱容积", sPid: "465", sTrPrefix: "5", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李箱最大拓展容积", sPid: "466", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "行李箱打开方式", sPid: "441", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "燃油&发动机", sPid: "", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "燃油箱容积", sPid: "576", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "2,3", sFieldTitle: "燃料类型", sPid: "578,577", sTrPrefix: "6,6", unit: " ,", joinCode: "," },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "供油方式", sPid: "580", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "发动机型号", sPid: "436", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "排量", sPid: "423", sTrPrefix: "6", unit: "mL", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "最大功率", sPid: "430", sTrPrefix: "6", unit: "kW", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "最大功率转速", sPid: "433", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "最大扭矩", sPid: "429", sTrPrefix: "6", unit: "Nm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "最大扭矩转速", sPid: "432", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "气缸排列型式", sPid: "418", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "发动机位置", sPid: "428", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "15,16", sFieldTitle: "进气型式", sPid: "425,408", sTrPrefix: "6,6", unit: " ,", joinCode: "," },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "汽缸数", sPid: "417", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "每缸气门数", sPid: "437", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "缸径", sPid: "415", sTrPrefix: "6", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "行程", sPid: "434", sTrPrefix: "6", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "压缩比", sPid: "414", sTrPrefix: "6", unit: ":1", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "缸体材料", sPid: "416", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "环保标准", sPid: "421", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "最大马力", sPid: "791", sTrPrefix: "6", unit: "Ps", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "底盘操控", sPid: "", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "转向助力", sPid: "735", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "变速箱类型", sPid: "712", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "挡位个数", sPid: "724", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "前制动类型", sPid: "726", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "后制动类型", sPid: "718", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "手刹类型", sPid: "716", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "前悬挂类型", sPid: "728", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "后悬挂类型", sPid: "720", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "轮毂材料", sPid: "704", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "前轮胎规格", sPid: "729", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "后轮胎规格", sPid: "721", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "备胎类型", sPid: "707", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "外部配置", sPid: "", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车身颜色", sPid: "598", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "后导流尾翼", sPid: "616", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车顶行李箱架", sPid: "627", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "运动包围", sPid: "597", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车窗", sPid: "601", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "电动窗锁止功能", sPid: "617", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "电动窗防夹功能", sPid: "594", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "后窗遮阳帘", sPid: "595", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后风窗加热功能", sPid: "600", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后雨刷器", sPid: "596", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "感应雨刷", sPid: "606", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "内后视镜防眩目功能", sPid: "621", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "外后视镜电动调节", sPid: "622", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "外后视镜电动折叠功能", sPid: "623", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "外后视镜加热功能", sPid: "624", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "外后视镜记忆功能", sPid: "625", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "前照灯类型", sPid: "614", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "前大灯随动转向", sPid: "613", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "前大灯延时关闭", sPid: "611", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "前雾灯", sPid: "607", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "前大灯自动开闭", sPid: "609", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "前照灯照射范围调整", sPid: "612", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "前照灯自动清洗功能", sPid: "608", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "侧转向灯", sPid: "626", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "行李箱灯", sPid: "618", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "高位(第三)制动灯", sPid: "620", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "电动吸合门", sPid: "821", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "日间行车灯", sPid: "794", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "车内氛围灯", sPid: "795", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "LED尾灯", sPid: "846", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "防紫外线/隔热玻璃", sPid: "796", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "36", sFieldTitle: "后排侧遮阳帘", sPid: "797", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "内饰", sPid: "", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "方向盘表面材料", sPid: "548", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "方向盘调节方式", sPid: "552", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "换挡拨片", sPid: "547", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "仪表板亮度可调", sPid: "534", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "HUD抬头数字显示", sPid: "518", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "行车电脑", sPid: "500", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "车外温度显示", sPid: "531", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "转速表", sPid: "553", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "座椅颜色", sPid: "542", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "座椅材质", sPid: "544", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "运动座椅", sPid: "546", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "座椅按摩功能", sPid: "543", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "座椅加热", sPid: "504", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "驾驶座腰部支撑调节", sPid: "506", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "驾驶座座椅调节方式", sPid: "508", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "驾驶座座椅调节方向", sPid: "507", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "驾驶座椅调节记忆位置组数", sPid: "505", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "副驾驶座椅调节方式", sPid: "503", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "副驾驶座椅调节方向", sPid: "502", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "前座中央扶手", sPid: "514", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "后座中央扶手", sPid: "475", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "主动式安全头枕", sPid: "481", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "后座椅头枕", sPid: "483", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "儿童安全座椅固定装置", sPid: "495", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "后排座位放倒比例", sPid: "482", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "36", sFieldTitle: "方向盘上下调节", sPid: "798", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "37", sFieldTitle: "方向盘前后调节", sPid: "799", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "38", sFieldTitle: "自适应巡航", sPid: "893", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "39", sFieldTitle: "内饰颜色", sPid: "801", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "40", sFieldTitle: "座椅通风", sPid: "804", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "41", sFieldTitle: "第三排座椅", sPid: "805", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "42", sFieldTitle: "电动座椅记忆", sPid: "803", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "影音空调", sPid: "", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "CD", sPid: "489", sTrPrefix: "10", unit: "碟CD", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "DVD", sPid: "509", sTrPrefix: "10", unit: "碟DVD", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车载电视", sPid: "559", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "中控台液晶屏", sPid: "488", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排液晶屏", sPid: "477", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "音响品牌", sPid: "473", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "扬声器数量", sPid: "523", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "空调", sPid: "470", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "空调控制方式", sPid: "471", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "温区个数", sPid: "555", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "后排出风口", sPid: "478", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "外接音源接口", sPid: "810", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "音频格式支持", sPid: "809", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "人机交互系统", sPid: "806", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "后排独立空调", sPid: "838", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "温度分区控制", sPid: "839", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "空气调节/花粉过滤", sPid: "840", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "车内空气净化装置", sPid: "905", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "便利功能", sPid: "", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "定速巡航", sPid: "545", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "GPS导航系统", sPid: "516", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "倒车雷达(车后)", sPid: "702", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "倒车影像", sPid: "703", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "中控门锁", sPid: "493", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "胎压监测装置", sPid: "714", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "车载电话", sPid: "554", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "蓝牙系统", sPid: "479", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "车载冰箱", sPid: "485", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "多功能方向盘", sPid: "528", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "无钥匙启动系统", sPid: "469", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "遥控钥匙", sPid: "538", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "遮阳板化妆镜", sPid: "512", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "泊车雷达(车前)", sPid: "800", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "自动泊车入位", sPid: "816", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "并线辅助", sPid: "817", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "主动刹车/主动安全系统", sPid: "818", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "夜视系统", sPid: "819", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "36", sFieldTitle: "全景摄像头", sPid: "820", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "37", sFieldTitle: "整体主动转向系统", sPid: "841", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "安全配置", sPid: "", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "自动制动差速器系统(ABD)", sPid: "672", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "刹车防抱死(ABS)", sPid: "673", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "ASR(驱动防滑装置)", sPid: "674", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车身稳定控制(ESP/DSC/VSC/ESC等)", sPid: "700", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "刹车辅助(EBA/BAS/BA/EVA等)", sPid: "684", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "制动力分配(EBD/CBC/EBV等)", sPid: "685", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "差速锁", sPid: "687", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "牵引力控制(ASR/TCS/TRC/ATC等)", sPid: "698", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "电子防盗系统", sPid: "699", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "发动机防盗系统", sPid: "683", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "驾驶位安全气囊", sPid: "682", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "副驾驶位安全气囊", sPid: "697", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "前排头部气囊(气帘)", sPid: "690", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "后排头部气囊(气帘)", sPid: "679", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "前排侧安全气囊", sPid: "691", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "后排侧安全气囊", sPid: "680", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "前安全带调节", sPid: "677", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "安全带预收紧功能", sPid: "678", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "安全带限力功能", sPid: "701", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "后排安全带", sPid: "675", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "后排中间三点式安全带", sPid: "676", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "儿童锁", sPid: "494", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "车门防撞杆(防撞侧梁)", sPid: "681", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "可溃缩转向柱", sPid: "713", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "膝部气囊", sPid: "835", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "安全带未系提示", sPid: "836", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "车内中控锁", sPid: "837", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "自动驻车", sPid: "811", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "上坡辅助", sPid: "812", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "陡坡缓降", sPid: "813", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "主动转向系统", sPid: "815", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "碰撞测试", sPid: "", sTrPrefix: "13", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "C-NCAP星级", sPid: "649", sTrPrefix: "13", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "NCAP碰撞测试", sPid: "637", sTrPrefix: "13", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "易车测试", sPid: "", sTrPrefix: "14", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "加速时间(0—100km/h)", sPid: "786", sTrPrefix: "14", unit: "s", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "制动距离(100—0km/h)", sPid: "787", sTrPrefix: "14", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "实测油耗", sPid: "788", sTrPrefix: "14", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车内怠速噪音", sPid: "789", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "车内等速(40km/h)噪音", sPid: "857", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "车内等速(60km/h)噪音", sPid: "790", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车内等速(80km/h)噪音", sPid: "858", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "车内等速(100km/h)噪音", sPid: "859", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车内等速(120km/h)噪音", sPid: "860", sTrPrefix: "14", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "18米绕桩速度", sPid: "861", sTrPrefix: "14", unit: "km/h", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "第二排腿部最大空间", sPid: "888", sTrPrefix: "14", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "第二排腿部最小空间", sPid: "889", sTrPrefix: "14", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "第三排腿部空间", sPid: "892", sTrPrefix: "14", unit: "mm", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "更多信息", sTrPrefix: "14", unit: "", joinCode: "" },
   { sType: "fieldIndex", sFieldIndex: "0", sFieldTitle: "车型指数排名", sTrPrefix: "0", unit: "", joinCode: "" },
   { sType: "fieldOther", sFieldIndex: "11", sFieldTitle: "推荐阅读", sTrPrefix: "0", unit: "", joinCode: "" }
];