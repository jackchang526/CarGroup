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
    ArrSelectYearType:new Array(),
	FloatTop: new Array(),
	ArrTempBarForFloatLeftHTML: new Array(),
	FloatLeft: new Array(),
	IsNeedFirstColor: false,
	CurrentCarID: 0,
	BaikeObj: null,
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
				// modified by chengl Jul.25.2012
				var carJsonObj = getCarAllParamDataByCarID(carid);
				if (carJsonObj) {
					var carinfo = new CarCompareInfo();
					carinfo.CarID = carid;
					carinfo.CarName = carName;
					carinfo.EngineExhaustForFloat = engineExhaustForFloat;
					carinfo.TransmissionType = transmissionType;
					carinfo.IsDel = false;
					carinfo.CarInfoArray = carJsonObj; //ComparePageObject.AllCarJson[i] || null;
					carinfo.IsValid = true;
					ComparePageObject.ArrCarInfo.push(carinfo);
					ComparePageObject.ValidCount++;
				}
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
	ComparePageObject.ArrPageContent.push("<th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th>");
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
					if (arrFieldRow["sFieldTitle"] == "参考成交价") {
						ComparePageObject.ArrPageContent.push("<span class=\"cRed\"><a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.ArrCarInfo[i].CarID + "&citycode=0\">" + field + "</a></span>");
						ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"http://dealer.bitauto.com/zuidijia/nb" + (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][3] || "") + "/nc" + ComparePageObject.ArrCarInfo[i].CarID + "/\">询最低价>></a></p>");
					}
					else if (arrFieldRow["sFieldTitle"] == "降价优惠") {
						var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6] || "";
						ComparePageObject.ArrPageContent.push("<span class=\"tdJiangjia\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + csAllSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/jiangjia/\">" + field + "</a></span>");
					}
					else
					{ }
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
	var pidArray = arrFieldRow["sPid"].split(',');
	var prefixArray = arrFieldRow["sTrPrefix"].split(',');

	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;
	// loop every car
	var tempArrMultField = [], num = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			num++;
			if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
				if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
				{ arrTemp.push("<td name=\"td" + i + "\" class=\"f\">"); }
				else
				{ arrTemp.push("<td name=\"td" + i + "\">"); }
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
						if (ComparePageObject.ArrCarInfo[i].CarInfoArray.length <= prefixArray[pint])
						{ return; }
						if (ComparePageObject.ArrCarInfo[i].CarInfoArray[prefixArray[pint]].length <= index)
						{ return; }
						var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[prefixArray[pint]][index] || "";
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
								{field = "(" + field + (unitArray[pint] || "") + ")"; }
							}
							else {
								field += unitArray[pint] || "";
							}
							// field += unitArray[pint];
							multiField = multiField + (joinCodeArray[pint] || "") + field;
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
	
						if (pint == 0) {
							// 如果第1项是有，则不显示有，显示第2项
							if (multiField == "<span class=\"songti\">●</span>")
							{ multiField = ""; }
							else if (multiField == "<span class=\"songti\">○</span>" || multiField == "<span class=\"songti\">-</span>")
							{ break; }
							else { }
							// 如果第1项是选配或无，则不显示第2项
						}
					}
					//=================每项多个属性对比 2012-04-11 by songkai===================
					firstSame = true;
					if (ComparePageObject.IsDelSame) {
						tempArrMultField.push(multiField);
						var flag = true;
						for (var j = 0; j < num; j++) {
							if (tempArrMultField[j] != multiField) {
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
				arrTemp.push("<td>&nbsp;</td>");
			}
			isShowLoop++;
		}
	}
	if (num == 1) {
		if (ComparePageObject.IsDelSame)
			arrTemp = [];
		for (var m = 0; m < tempArrMultField.length; m++) {
			arrTemp.push("<td name=\"td" + m + "\" >");
			arrTemp.push(tempArrMultField[m]);
			arrTemp.push("</td>");
		}
	}
	if (firstSame && ComparePageObject.IsDelSame && num > 1) {
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
			ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
			ComparePageObject.ArrPageContent.push("<th>");
			ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow["sFieldTitle"]));
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow["sFieldTitle"]) + "</th></tr>");
		}
		else {
			return;
		}
	}
	//when less 对比项小于5个时，填补对比项
	if (num < 5) {
		for (var kk = 0; kk < 5 - num; kk++) {
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

function setExhaustAndTransmissionType() {
	var arrTempExhaust = new Array();
	var arrTempTransmissionType = new Array();
	var exhaust = "";
	var transmissionType = "";
	var yearType = "";
	var arrExhaust = new Array();
	var arrTransmissionType = new Array();
	var exhaustCount = 0;
	var transmissionTypeCount = 0;
	var arrTempYear = new Array();
	var arrYear = new Array();
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
        if (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7] != "" && !isNaN(ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7])) {
            if (yearType.indexOf(ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7] + ",") < 0) {
                yearType += ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7] + ",";
                arrTempYear.push(ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7]);
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

    //年款
    if (arrTempYear.length > 1) {
        arrTempYear.sort(function (a, b) { return b - a; });
        for (var i = 0; i < arrTempYear.length && i < 3; i++) {
            arrYear.push(" <input type=\"checkbox\" id=\"checkboxYearType_" + arrTempYear[i] + "\" value=\"" + arrTempYear[i] + "\" onclick=\"checkYearType(this);\">");
            arrYear.push(" <label for=\"checkboxYearType_" + arrTempYear[i] + "\">" + arrTempYear[i] + "款</label>");
        }
    }
    var exW = 0, trW = 0;
	if (arrExhaust.length > 0)
	{ exW = $("#spanFilterForEE").html("排量筛选：" + arrExhaust.join("")).width()+20; }
	if (arrTransmissionType.length > 0)
	{ trW = $("#spanFilterForTT").html("变速箱筛选：" + arrTransmissionType.join("")).width()+50; }
	if (arrYear.length > 0) {
	    if ($("#divFilter").width() - exW - trW - ((13 + 36) * arrYear.length + 42 + 50) > 0) {
	        $("#spanFilterForYear").html("年款：" + arrYear.join(""));
	    }
	}
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
function checkYearType(obj) {
    if (obj.checked) {
		if ((ComparePageObject.ArrSelectYearType.join(",") + ",").indexOf(obj.value.toString()) < 0) {
			ComparePageObject.ArrSelectYearType.push(obj.value.toString());
		}
	}
	else {
		if ((ComparePageObject.ArrSelectYearType.join(",") + ",").indexOf(obj.value.toString()) >= 0) {
			for (var i = 0; i < ComparePageObject.ArrSelectYearType.length; i++) {
				if (ComparePageObject.ArrSelectYearType[i] == obj.value.toString()) {
					ComparePageObject.ArrSelectYearType.splice(i, 1);
				}
			}
		}
    }
    resetSelectCarsByCheckbox();
}
function resetSelectCarsByCheckbox() {
    if (ComparePageObject.ArrSelectExhaust.length > 0 || ComparePageObject.ArrSelectTransmission.length > 0 || ComparePageObject.ArrSelectYearType.length > 0) {
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (ComparePageObject.ArrSelectExhaust.length > 0) {
				if ((ComparePageObject.ArrSelectExhaust.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",") < 0) {
				    ComparePageObject.ArrCarInfo[i].IsDel = true;
				    continue;
				}
            }

            if (ComparePageObject.ArrSelectTransmission.length > 0) {
				if ((ComparePageObject.ArrSelectTransmission.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].TransmissionType + ",") < 0) {
				    ComparePageObject.ArrCarInfo[i].IsDel = true;
				    continue;
				}
            }

            if (ComparePageObject.ArrSelectYearType.length > 0) {
                if ((ComparePageObject.ArrSelectYearType.join(",") + ",").indexOf(ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7] + ",") < 0) {
                    ComparePageObject.ArrCarInfo[i].IsDel = true;
                    continue;
                }
            }
            ComparePageObject.ArrCarInfo[i].IsDel = false;
		}
	}
	else {
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
  { sType: "fieldPic", sFieldIndex: "", sFieldTitle: "图片", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "厂家指导价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "1", sFieldTitle: "参考成交价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "7", sFieldTitle: "降价优惠", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "电动车窗", sPid: "601", sTrPrefix: "8", unit: "", joinCode: "" },
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
   { sType: "fieldMulti", sFieldIndex: "4,5", sFieldTitle: "CD", sPid: "490,489", sTrPrefix: "10,10", unit: ",碟", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "6,7", sFieldTitle: "DVD", sPid: "510,509", sTrPrefix: "10,10", unit: ",碟", joinCode: "" },
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
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "第三排腿部空间", sPid: "892", sTrPrefix: "14", unit: "mm", joinCode: "" }
];