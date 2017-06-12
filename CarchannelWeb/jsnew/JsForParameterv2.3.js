// 车型频道对比
var ComparePageObject = {
	PageDivContentID: "CarCompareContent",  // container id
	PageDivContentObj: null,   // container object
	ArrPageContent: new Array(),
	ValidCount: 0,
	AllCarJson: new Array(),
	ArrCarInfo: new Array(),
	IsOperateTheSame: false,
	IsDelSame: false,
	ArrTempBarHTML: new Array(),
	IsNeedDrag: true,
	ArrSelectExhaust: new Array(),
	ArrSelectTransmission: new Array(),
	FloatTop: new Array(),
	ArrTempBarForFloatLeftHTML: new Array(),
	FloatLeft: new Array(),
	IsNeedFirstColor: false,
	CurrentCarID: 0,
	CarIDAndNames: ""
}

function initPageForCompare() {
	ComparePageObject.ArrCarInfo.length = 0;
	ComparePageObject.ArrPageContent.length = 0;
	ComparePageObject.ValidCount = 0;
	if (document.getElementById(ComparePageObject.PageDivContentID)) {
		ComparePageObject.PageDivContentObj = document.getElementById(ComparePageObject.PageDivContentID);
	}
	else
	{ return; }
	if (typeof (carCompareJson) != "undefined")
	{ ComparePageObject.AllCarJson = carCompareJson; }
	if (ComparePageObject.CarIDAndNames != "" && ComparePageObject.AllCarJson.length > 0) {
		// 有车型数据
		var arrCompareCar = ComparePageObject.CarIDAndNames.split('|');
		if (arrCompareCar.length > 0) {
			for (var i = 0; i < arrCompareCar.length; i++) {
				var carCookie = arrCompareCar[i].split(',');
				var carid = carCookie[0].substring(2, carCookie[0].length);
				var carName = carCookie[1];
				var engineExhaustForFloat = carCookie[2];
				var transmissionType = carCookie[3];
				var carinfo = new CarCompareInfo();
				carinfo.CarID = carid;
				carinfo.CarName = carName;
				carinfo.EngineExhaustForFloat = engineExhaustForFloat;
				carinfo.TransmissionType = transmissionType;
				carinfo.IsDel = false;
				carinfo.CarInfoArray = getCarAllParamDataByCarID(carid); //ComparePageObject.AllCarJson[i] || null;
				carinfo.IsValid = true;
				ComparePageObject.ArrCarInfo.push(carinfo);
				ComparePageObject.ValidCount++;
			}
			if (ComparePageObject.ValidCount > 0) {
				setExhaustAndTransmissionType();
				createPageForCompare(ComparePageObject.IsOperateTheSame);
			}
		}
	}
	else {
		$(ComparePageObject.PageDivContentObj).html("无车型数据");
	}
}

function createPageForCompare(isDelSame) {
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
		}
	}

	ComparePageObject.ArrPageContent.push("</tbody></table>");
	// end
	if (ComparePageObject.PageDivContentObj) {
		$(ComparePageObject.PageDivContentObj).html(ComparePageObject.ArrPageContent.join(""));
	}
	if (ComparePageObject.ValidCount > 0) {
		$("#leftfixed").html("<table cellpadding=\"0\" cellspacing=\"0\" class=\"floatTable floatTable_peizhi\">" + ComparePageObject.FloatLeft.join("") + "</table>");
		ComparePageObject.FloatLeft.length = 0;
	}
	ComparePageObject.ArrPageContent.length = 0;
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
	ComparePageObject.FloatTop.push(" <label for=\"checkDifferFloatTop\">只显示不同点</label>");
	ComparePageObject.FloatTop.push("<div class=\"dashline\"></div>");
	ComparePageObject.FloatTop.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
	ComparePageObject.FloatTop.push("</div>");
	ComparePageObject.FloatTop.push("</th>");

	// for FloatLeft
	ComparePageObject.FloatLeft.length = 0;
	ComparePageObject.FloatLeft.push("<tr>");
	ComparePageObject.FloatLeft.push("<th class=\"pd0\">");
	ComparePageObject.FloatLeft.push("<div class=\"tableHead_left\">");
	ComparePageObject.FloatLeft.push("<input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDifferFloatLeft\">");
	ComparePageObject.FloatLeft.push(" <label for=\"checkDifferFloatLeft\">只显示不同点</label>");
	ComparePageObject.FloatLeft.push("<div class=\"dashline\"></div>");
	ComparePageObject.FloatLeft.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
	ComparePageObject.FloatLeft.push("</div>");
	ComparePageObject.FloatLeft.push("</th>");
	ComparePageObject.FloatLeft.push("</tr>");

	ComparePageObject.ArrPageContent.push("<tr class=\"carfloatLayer_peizhi\">");
	ComparePageObject.ArrPageContent.push("<th class=\"pd0\">");
	ComparePageObject.ArrPageContent.push("<div class=\"tableHead_left\">");
	ComparePageObject.ArrPageContent.push("<input type=\"checkbox\" id=\"checkDiffer\" onclick=\"delTheSameForCompare();\" name=\"checkboxForDelTheSame\"> ");
	ComparePageObject.ArrPageContent.push(" <label for=\"checkDiffer\">只显示不同点</label><div class=\"dashline\"></div>");
	ComparePageObject.ArrPageContent.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p></div>");
	ComparePageObject.ArrPageContent.push("</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
				ComparePageObject.ArrPageContent.push("<td class=\"pd0\" id=\"td_" + i + "\" ><div class=\"tableHead_item\">");
				try {
					// car info
					var carid = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][0];
					var carName = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][1];
					var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6];
					var carYear = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7];
					ComparePageObject.ArrPageContent.push("<div class=\"carBox\">");
					ComparePageObject.ArrPageContent.push("<dl>");
					ComparePageObject.ArrPageContent.push("<dd><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/\">" + carName + (carYear == "" ? "" : " " + carYear + "款") + "</a></dd>");
					ComparePageObject.ArrPageContent.push("</dl>");
					ComparePageObject.ArrPageContent.push("</div>");
					var pic = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][2] || "";
					var altString = ComparePageObject.IsNeedDrag ? "按住鼠标左键，可拖动到其他列" : "";

					// for FloatTop
					ComparePageObject.FloatTop.push("<td class=\"pd0\">");
					ComparePageObject.FloatTop.push("<div id=\"FloatTop_tableHead_" + i + "\" class=\"tableHead_item\">");
					ComparePageObject.FloatTop.push("<div name=\"hasCarBox\" id=\"FloatTop_carBox_" + i + "\" class=\"carBox\">");
					ComparePageObject.FloatTop.push("<dl><dd><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/\">" + carName + (carYear == "" ? "" : " " + carYear + "款") + "</a></dd>");
					ComparePageObject.FloatTop.push("</dl></div>");
					ComparePageObject.FloatTop.push("<div id=\"FloatTop_OptArea_" + i + "\" class=\"optArea\">");

					ComparePageObject.ArrPageContent.push("<div class=\"optArea\">");
					if (ComparePageObject.IsNeedDrag) {
						if (isShowLoop == 0) {
							if (checkIsRightEnd(i)) {
							}
							else {
								ComparePageObject.ArrPageContent.push(" <a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a> <a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"right\">右移</a>");
								ComparePageObject.FloatTop.push(" <a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a> <a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"right\">右移</a>");
							}
						}
						else {
							if (checkIsRightEnd(i)) {
								ComparePageObject.ArrPageContent.push(" <a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"left\">左移</a><a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a>");
								ComparePageObject.FloatTop.push(" <a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"left\">左移</a><a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a>");
							}
							else {
								ComparePageObject.ArrPageContent.push(" <a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"left\">左移</a> <a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a> <a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"right\">右移</a>");
								ComparePageObject.FloatTop.push(" <a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"left\">左移</a> <a class=\"optBtn mgl45\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a> <a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"right\">右移</a>");
							}
						}
					}
					ComparePageObject.ArrPageContent.push("</div>");
					// for FloatTop
					ComparePageObject.FloatTop.push("</div></div>");
					ComparePageObject.FloatTop.push("</td>");
				}
				catch (err)
                { }
				ComparePageObject.ArrPageContent.push("</div></td>");
			}
			isShowLoop++;
		}
	}
	//when less
	if (isShowLoop < 5) {
		for (var i = 0; i < 5 - isShowLoop; i++) {
			ComparePageObject.ArrPageContent.push("<td class=\"pd0\"><div class=\"tableHead_item\"></div></td>");
			ComparePageObject.FloatTop.push("<td class=\"pd0\"><div class=\"tableHead_item\"></div></td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
	ComparePageObject.FloatTop.push("</tr>");
	ComparePageObject.FloatTop.push("</table>");
	$("#topfixed").html(ComparePageObject.FloatTop.join(""));
}

// create price for compare
function createPrice(arrFieldRow) {
	ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\">");
	ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
				if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor) {
					ComparePageObject.ArrPageContent.push("<td name=\"td" + i + "\" class=\"f\">");
				}
				else {
					ComparePageObject.ArrPageContent.push("<td name=\"td" + i + "\">");
				}
				try {
					var sTrPrefix = arrFieldRow["sTrPrefix"];
					var index = arrFieldRow["sFieldIndex"];
					var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix][index] || "";
				}
				catch (err) {
					ComparePageObject.ArrPageContent.push("-");
				}
				if (field.length < 1 || field == "无") {
					ComparePageObject.ArrPageContent.push("无");
				}
				else {
					ComparePageObject.ArrPageContent.push("<a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.ArrCarInfo[i].CarID + "&citycode=0\">" + field.replace('～', '-') + "</a>");
				}
				ComparePageObject.ArrPageContent.push("</td>");
			}
			isShowLoop++;
		}
	}
	//when less
	if (isShowLoop < 5) {
		for (var i = 0; i < 5 - isShowLoop; i++) {
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.FloatLeft.push("<tr><th>" + arrFieldRow["sFieldTitle"] + "</th></tr>");
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create param for compare
function createPara(arrFieldRow) {
	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
				if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
				{ arrTemp.push("<td name=\"td" + i + "\" class=\"f\">"); }
				else
				{ arrTemp.push("<td name=\"td" + i + "\" >"); }
				try {
					var sTrPrefix = arrFieldRow["sTrPrefix"];
					var index = arrFieldRow["sFieldIndex"];
					if (ComparePageObject.ArrCarInfo[i].CarInfoArray.length <= sTrPrefix)
					{ return; }
					if (ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix].length <= index)
					{ return; }
					var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix][index] || "";
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
	}
	if (firstSame && ComparePageObject.IsDelSame && isShowLoop > 1) {
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
				ComparePageObject.FloatLeft.push("<tr><th class=\"threeLines\">" + arrFieldRow["sFieldTitle"] + "</th></tr>");
			}
			else {
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.FloatLeft.push("<tr><th>" + arrFieldRow["sFieldTitle"] + "</th></tr>");
			}
			ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
		}
		else {
			return;
		}
	}
	//when less
	if (isShowLoop < 5) {
		for (var i = 0; i < 5 - isShowLoop; i++) {
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
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
	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;
	// loop every car
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
				if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
				{ arrTemp.push("<td name=\"td" + i + "\" class=\"f\">"); }
				else
				{ arrTemp.push("<td name=\"td" + i + "\">"); }
				try {
					var multiField = "";
					for (var pint = 0; pint < paramArray.length; pint++) {
						// loop every param
						var sTrPrefix = arrFieldRow["sTrPrefix"];
						var index = paramArray[pint];
						if (ComparePageObject.ArrCarInfo[i].CarInfoArray.length <= sTrPrefix)
						{ return; }
						if (ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix].length <= index)
						{ return; }
						var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix][index] || "";
						if (field == "待查")
						{ field = ""; }
						if (field.length > 0) {
							field += unitArray[pint];
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
					arrTemp.push(multiField);
				}
				catch (err) {
					arrTemp.push("-");
					firstSame = firstSame && false;
				}
				arrTemp.push("</td>");
			}
			else {
				arrTemp.push("<td>&nbsp;</td>");
			}
			isShowLoop++;
		}
	}
	if (firstSame && ComparePageObject.IsDelSame && isShowLoop > 1) {
		return;
	}
	else {
		if (!isAllunknown) {
			// Is Need Show The Bar
			if (ComparePageObject.ArrTempBarHTML.length > 0) {
				ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
				ComparePageObject.ArrTempBarHTML.length = 0;
			}
			ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
			ComparePageObject.ArrPageContent.push("<th>");
			ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			ComparePageObject.FloatLeft.push("<tr><th>" + arrFieldRow["sFieldTitle"] + "</th></tr>");
		}
		else {
			return;
		}
	}
	//when less
	if (isShowLoop < 5) {
		for (var i = 0; i < 5 - isShowLoop; i++) {
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function createBar(arrFieldRow) {
	if (ComparePageObject.ValidCount < 1)
	{ return; }
	ComparePageObject.ArrTempBarHTML.length = 0;
	ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
	var summaryColumn = 1;
	ComparePageObject.ArrTempBarHTML.push("<tr>");
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			summaryColumn++;
		}
	}
	if (summaryColumn < 6)
	{ summaryColumn = 6; }
	ComparePageObject.ArrTempBarHTML.push("<td class=\"pd0\" colspan=\"" + summaryColumn + "\">");
	ComparePageObject.ArrTempBarHTML.push("<h2><span>" + arrFieldRow["sFieldTitle"] + "</span></h2>");
	ComparePageObject.ArrTempBarHTML.push("</td>");
	ComparePageObject.ArrTempBarHTML.push("</tr>");
	ComparePageObject.ArrTempBarForFloatLeftHTML.push("<tr>");
	ComparePageObject.ArrTempBarForFloatLeftHTML.push("<td class=\"pd0\"><h2><span>" + arrFieldRow["sFieldTitle"] + "</span></h2></td>");
	ComparePageObject.ArrTempBarForFloatLeftHTML.push("</tr>");
}

function setExhaustAndTransmissionType() {
	var arrTempExhaust = new Array();
	var arrTempTransmissionType = new Array();
	var exhaust = "";
	var transmissionType = "";
	var arrExhaust = new Array();
	var arrTransmissionType = new Array();
	var exhaustCount = 0;
	var transmissionTypeCount = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat != "") {
			// new exhaust
			if (exhaust.indexOf(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",") < 0) {
				exhaust += ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",";
				arrTempExhaust.push(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat);
				exhaustCount++;
			}
		}
		if (ComparePageObject.ArrCarInfo[i].TransmissionType != "") {
			// new transmissionType
			if (transmissionType.indexOf(ComparePageObject.ArrCarInfo[i].TransmissionType + ",") < 0) {
				transmissionType += ComparePageObject.ArrCarInfo[i].TransmissionType + ",";
				arrTempTransmissionType.push(ComparePageObject.ArrCarInfo[i].TransmissionType);
				if (ComparePageObject.ArrCarInfo[i].TransmissionType != "9") {
					transmissionTypeCount++;
				}
			}
		}
	}
	// 排量排序
	if (arrTempExhaust.length > 0) {
		arrTempExhaust.sort();
		for (var i = 0; i < arrTempExhaust.length; i++) {
			arrExhaust.push(" <input type=\"checkbox\" id=\"checkboxExhaust_" + arrTempExhaust[i] + "\" value=\"" + arrTempExhaust[i] + "\" onclick=\"checkExhaust(this);\" >");
			arrExhaust.push(" <label for=\"checkboxExhaust_" + arrTempExhaust[i] + "\">" + arrTempExhaust[i] + "</label>");
		}
	}
	// 变速器排序
	if (arrTempTransmissionType.length > 0) {
		arrTempTransmissionType.sort();
		for (var i = 0; i < arrTempTransmissionType.length; i++) {
			if (arrTempTransmissionType[i] == 1) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_1\" value=\"1\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_1\">手动</label>");
			}
			if (arrTempTransmissionType[i] == 2) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_2\" value=\"2\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_2\">自动</label>");
			}
			if (arrTempTransmissionType[i] == 3) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_3\" value=\"3\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_3\">手自一体</label>");
			}
			if (arrTempTransmissionType[i] == 4) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_4\" value=\"4\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_4\">半自动</label>");
			}
			if (arrTempTransmissionType[i] == 5) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_5\" value=\"5\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_5\">CVT无级变速</label>");
			}
			if (arrTempTransmissionType[i] == 6) {
				arrTransmissionType.push(" <input type=\"checkbox\" id=\"checkboxTransmissionType_6\" value=\"6\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push(" <label for=\"checkboxTransmissionType_6\">双离合</label>");
			}
		}
	}
	if (arrExhaust.length > 0)
	{ $("#spanFilterForEE").html("排量筛选：" + arrExhaust.join("")); }
	if (arrTransmissionType.length > 0)
	{ $("#spanFilterForTT").html("变速箱筛选：" + arrTransmissionType.join("")); }
}

function checkExhaust(obj) {
	if (obj.checked) {
		if ((ComparePageObject.ArrSelectExhaust.join(",") + ",").indexOf(obj.value.toString()) < 0) {
			ComparePageObject.ArrSelectExhaust.push(obj.value.toString());
		}
	}
	else {
		if ((ComparePageObject.ArrSelectExhaust.join(",") + ",").indexOf(obj.value.toString()) >= 0) {
			for (var i = 0; i < ComparePageObject.ArrSelectExhaust.length; i++) {
				if (ComparePageObject.ArrSelectExhaust[i] == obj.value.toString()) {
					ComparePageObject.ArrSelectExhaust.splice(i, 1);
				}
			}
		}
	}
	resetSelectCarsByCheckbox();
}

function checkTransmissionType(obj) {
	if (obj.checked) {
		if ((ComparePageObject.ArrSelectTransmission.join(",") + ",").indexOf(obj.value.toString()) < 0) {
			ComparePageObject.ArrSelectTransmission.push(obj.value.toString());
		}
	}
	else {
		if ((ComparePageObject.ArrSelectTransmission.join(",") + ",").indexOf(obj.value.toString()) >= 0) {
			for (var i = 0; i < ComparePageObject.ArrSelectTransmission.length; i++) {
				if (ComparePageObject.ArrSelectTransmission[i] == obj.value.toString()) {
					ComparePageObject.ArrSelectTransmission.splice(i, 1);
				}
			}
		}
	}
	resetSelectCarsByCheckbox();
}

function resetSelectCarsByCheckbox() {
	if (ComparePageObject.ArrSelectExhaust.length > 0 || ComparePageObject.ArrSelectTransmission.length > 0) {
		for (var i = 0; i < ComparePageObject.ValidCount; i++) {
			if (ComparePageObject.ArrSelectExhaust.length > 0) {
				if ((ComparePageObject.ArrSelectExhaust.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",") < 0) {
					ComparePageObject.ArrCarInfo[i].IsDel = true;
				}
				else {
					if (ComparePageObject.ArrSelectTransmission.length > 0) {
						if ((ComparePageObject.ArrSelectTransmission.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].TransmissionType + ",") < 0) {
							ComparePageObject.ArrCarInfo[i].IsDel = true;
						}
						else {
							ComparePageObject.ArrCarInfo[i].IsDel = false;
						}
					}
					else {
						ComparePageObject.ArrCarInfo[i].IsDel = false;
					}
				}
			}
			else {
				if (ComparePageObject.ArrSelectTransmission.length > 0) {
					if ((ComparePageObject.ArrSelectTransmission.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].TransmissionType + ",") < 0) {
						ComparePageObject.ArrCarInfo[i].IsDel = true;
					}
					else {
						ComparePageObject.ArrCarInfo[i].IsDel = false;
					}
				}
			}
		}
	}
	if (ComparePageObject.ArrSelectTransmission.length <= 0 && ComparePageObject.ArrSelectExhaust.length <= 0) {
		for (var i = 0; i < ComparePageObject.ValidCount; i++) {
			ComparePageObject.ArrCarInfo[i].IsDel = false;
		}
	}
	createPageForCompare(ComparePageObject.IsOperateTheSame);
}

function checkCarIsShowForeach(index) {
	return !ComparePageObject.ArrCarInfo[index].IsDel;
}

function checkTitleIsThreeLines(sFieldTitle) {
	var isThreeLines = false;
	if (sFieldTitle == "车身颜色" || sFieldTitle == "座椅颜色") {
		isThreeLines = true;
	}
	return isThreeLines;
}

function checkTitleIsTwoLines(sFieldTitle) {
	var isTwoLines = false;
	if (sFieldTitle == "保修政策"
	|| sFieldTitle == "特有技术"
	|| sFieldTitle == "型号"
	|| sFieldTitle == "主动安全-其他"
	|| sFieldTitle == "后悬挂类型"
	|| sFieldTitle == "前悬挂类型") {
		isTwoLines = true;
	}
	return isTwoLines;
}

// 车型对比信息
function CarCompareInfo(carid, carName, engineExhaustForFloat, transmissionType, isValid, isDel, carInfoArray) {
	this.CarID = carid;
	this.CarName = carName;
	this.EngineExhaustForFloat = engineExhaustForFloat;
	this.TransmissionType = transmissionType;
	this.IsValid = isValid;
	this.IsDel = isDel;
	this.CarInfoArray = carInfoArray;
}

function getCarAllParamDataByCarID(carid) {
	var carArrayData = null;
	if (carid > 0 && ComparePageObject.AllCarJson.length > 0) {
		for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
			if (ComparePageObject.AllCarJson[i][0][0] == carid) {
				carArrayData = ComparePageObject.AllCarJson[i];
				break;
			}
		}
	}
	return carArrayData;
}

function checkIsRightEnd(index) {
	var isRightEnd = true;
	if (index >= 0 && index < ComparePageObject.ArrCarInfo.length) {
		for (i = index * 1 + 1; i < ComparePageObject.ArrCarInfo.length; i++) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel)
			{ isRightEnd = false; }
		}
	}
	return isRightEnd;
}

function moveLeftForCarCompare(index) {
	if (index > 0 && ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
		for (var i = index - 1; i >= 0; i--) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel) {
				swapArray(ComparePageObject.ArrCarInfo, i, index);
				createPageForCompare(ComparePageObject.IsOperateTheSame);
				break;
			}
		}
	}
}

function moveRightForCarCompare(index) {
	if (index >= 0 && (ComparePageObject.ValidCount - 1) > index && (ComparePageObject.ArrCarInfo.length - 1) > index) {
		for (i = index * 1 + 1; i < ComparePageObject.ArrCarInfo.length; i++) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel) {
				swapArray(ComparePageObject.ArrCarInfo, i, index);
				createPageForCompare(ComparePageObject.IsOperateTheSame);
				break;
			}
		}
	}
}

function delCarToCompare(index) {
	if (ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
		var isHasOtherValid = false;
		for (var i = 0; i < ComparePageObject.ValidCount; i++) {
			if (index != i && !ComparePageObject.ArrCarInfo[i].IsDel) {
				isHasOtherValid = true;
				break;
			}
		}
		if (isHasOtherValid) {
			ComparePageObject.ArrCarInfo[index].IsDel = true;
			createPageForCompare(ComparePageObject.IsOperateTheSame);
		}
		else {
			alert('至少留1个对比车型');
		}
	}
}

// swap Array object
function swapArray(obj, index1, index2) {
	var temp = obj[index1];
	obj[index1] = obj[index2];
	obj[index2] = temp;
}

// 排除相同项
function delTheSameForCompare() {
	var validCount = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (!ComparePageObject.ArrCarInfo[i].IsDel)
			validCount++;
		if (validCount > 1) {
			break;
		}
	}
	if (validCount > 1) {
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

// change checkbox state for delete the same param
function changeCheckBoxStateByName(name, state) {
	var checkBoxs = document.getElementsByName(name);
	if (checkBoxs && checkBoxs.length > 0) {
		for (var i = 0; i < checkBoxs.length; i++) {
			checkBoxs[i].checked = state;
		}
	}
}

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

function changeCheckBoxStateByName(name, state) {
	var checkBoxs = document.getElementsByName(name);
	if (checkBoxs && checkBoxs.length > 0) {
		for (var i = 0; i < checkBoxs.length; i++) {
			checkBoxs[i].checked = state;
		}
	}
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
   { sType: "fieldPic", sFieldIndex: "", sFieldTitle: "图片", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "厂家指导价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "1", sFieldTitle: "商家报价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "保修政策", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "排量（升）", sTrPrefix: "1", unit: "L", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "4,5", sFieldTitle: "变速器型式", sTrPrefix: "1", unit: "档,", joinCode: ", " },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本性能", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "综合工况油耗", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "市区工况油耗", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "市郊工况油耗", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "百公里等速油耗", sTrPrefix: "2", unit: "L/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "网友油耗", sTrPrefix: "0", unit: "/100km", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "最大爬坡度", sTrPrefix: "2", unit: "%", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "最大爬坡度(值)", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "加速时间(0—100 km/h)", sTrPrefix: "2", unit: "s", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "制动距离(100—0 km/h)", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "最小转弯半径", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "最大涉水深度", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "驱动方式", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "乘员人数（含司机）", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "风阻系数", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "车内怠速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "车内等速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "前轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "后轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "整备质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "允许总质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "最大承载质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "车身结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车门数", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车身型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "车身加长型", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车顶型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "天窗型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "天窗开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车篷型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "车篷开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车篷开合时间", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "车体结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "外部尺寸", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "长", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "宽", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "高", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "轴距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "前悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "后悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "接近角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "离去角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "最小离地间隙", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "内部尺寸", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "前排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "后排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "后排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "第三排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "第三排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "第三排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "行李箱最大开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "行李箱最小开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "行李箱开口离地高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "行李箱内部高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "行李箱内部深度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "行李箱内部最大宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "行李箱容积", sTrPrefix: "5", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李箱最大拓展容积", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "行李箱打开方式", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "燃油&发动机", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "燃油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "副油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "燃料类型", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "燃油标号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "燃料供给型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "型号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "生产厂家", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "排量", sTrPrefix: "6", unit: "mL", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "最高转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "最大功率-功率值", sTrPrefix: "6", unit: "kW", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "最大功率-转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "最大扭矩-扭矩值", sTrPrefix: "6", unit: "Nm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "最大扭矩—转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "气缸排列型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "发动机位置", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "进气型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "增压方式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "凸轮轴", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "特有技术", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "汽缸数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "每缸气门数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "缸径", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "行程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "可变气门正时", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "可变气门升程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "压缩比", sTrPrefix: "6", unit: ":1", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "缸体材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "缸盖材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "环保标准", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "废气再循环（EGR）", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "三元催化器", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "防冻液容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "机油容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "底盘操控", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "转向机形式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "转向助力", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "随速助力转向调节(EPS)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "方向盘回转总圈数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "变速器型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "前进档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "变速箱变速杆位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "分动箱档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "分动箱型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "分动箱操纵", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "主减速比", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "中央差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "前轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "前制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "后制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "驻车制动器", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "减震器类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "前悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "后悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "悬挂高度调节", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "轮毂材料", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "前轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "后轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "前轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "后轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "备胎类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "备胎位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "零压续行(零胎压继续行驶)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "外部配置", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车身颜色", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车身油漆", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "后导流尾翼", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车顶行李箱架", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "运动包围", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "车门玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车窗", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "前风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "电动窗锁止功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "后风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "电动窗防夹功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "后窗遮阳帘", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后风窗加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后雨刷器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "雨刷传感器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "内后视镜防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "外后视镜电动调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "外后视镜电动折叠功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "外后视镜加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "外后视镜记忆功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "前照灯类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "会车前灯防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "前大灯随动转向", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "前大灯延时关闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "前雾灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "前大灯自动开闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "前照灯照射高度调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "前照灯自动清洗功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "侧转向灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "行李箱灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "高位(第三)制动灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "内饰", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "方向盘表面材料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "方向盘调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "方向盘幅数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "方向盘记忆设置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "方向盘调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "方向盘换档", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "仪表板显示型式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "仪表板背光颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "仪表板亮度可调", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "HUD抬头数字显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "行车电脑", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "时钟", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "车外温度显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "燃油不足警告方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "转速表", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "罗盘/指南针", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "海拔仪", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "座椅颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "座椅面料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "运动座椅", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "座椅按摩功能", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "驾驶座座椅加热", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "驾驶座腰部支撑调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "驾驶座座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "驾驶座座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "驾驶座椅调节记忆位置组数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "副驾驶座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "副驾驶座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "前座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "后座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "主动式安全头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "31", sFieldTitle: "后座椅头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "前座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "后座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "儿童安全座椅固定装置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "后排座位放倒比例", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "影音空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "卡带", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "收音机", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "MP3", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "VCD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "CD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "CD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "DVD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "DVD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车载电视", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "中控台液晶屏", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排液晶显示器", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "音响品牌", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "扬声器数量", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "空调控制方式", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "湿度调节", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "温区个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "后排出风口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "出风口个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "外接音源接口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "便利功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车内电源插口数量", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车内电源电压", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "车厢前阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "车厢后阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "车内灯光延时关闭", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "定速巡航系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "GPS电子导航", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "电子限速", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "倒车雷达", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "倒车影像", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "中控门锁", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "胎压检测装置", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "车门/行李箱电吸", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "无线上网功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "车载电话", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "蓝牙系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "车载冰箱", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "多功能方向盘", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "多功能方向盘功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "无钥匙点火系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "遥控钥匙", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "遥控行李箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "行李箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "行李箱盖开合方式", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "遥控油箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "油箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "中央置物盒", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "前排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "后排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "衣物挂钩", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "遮阳板化妆镜", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "安全配置", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "ABD(自动制动差速器)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "ABS(刹车防抱死制动系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "ASR(驱动防滑装置)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "DSC(动态稳定控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "EBA/EVA(紧急制动辅助系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "EBD/EBV(电子制动力分配)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "EDS(电子差速锁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "ESP(电子稳定程序)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "TCS(牵引力控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "电子防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "发动机防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "主动安全-其他", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "副驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "前排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "后排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "前排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "后排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "副气囊锁止功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "气囊气帘数量", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "前安全带调节", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "安全带预收紧功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "安全带限力功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "后排安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "后排中间三点式安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "儿童锁", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "溃缩式制动踏板", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "车门防撞杆(防撞侧梁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "可溃缩转向柱", sTrPrefix: "12", unit: "", joinCode: "" }
];