﻿// 车型频道参数配置

var ComparePageObject = {
	CarInfoPath: "http://car.bitauto.com/car/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path 
	RootPath: "",    // root path
	ResourceDIR: "", // image dir
	PageDivContentID: "CarCompareContent",  // container id
	PageDivContentObj: null,   // container object
	IsIE: true, // client browser
	IsDelSame: false,  // is delete the same param
	IsDeployAll: false,
	IsOperateTheSame: false,
	XmlSrcLength: 0,
	XmlHttpForCompare: null,
	ArrCarInfo: new Array(),
	ArrPageContent: new Array(),
	ValidCount: 0,
	ShowCount: 0, // 呈现数
	MaxTDLeft: 5,
	IsNeedSecTH: false, //146px
	IsNeedSelect: false,
	IsNeedBlockTD: true,
	NeedBlockTD: 0, // 134px
	CarIDAndNames: "",
	TableWidth: 1634,
	IsNeedDrag: true,
	IsAutoNeedMoreTH: true,
	LoopTRColor: 0,
	ArrTempBarHTML: new Array(),
	ArrSelectExhaust: new Array(),
	ArrSelectTransmission: new Array(),
	ColorChangeIndex: -1,
	ColorTimer: null,
	IsNeedFirstColor: false,
	ComputeSum: 0,
	ComputeCount: 0,
	ComputeAvg: 0
}

// 车型对比信息
function CarCompareInfo(carid, carName, engineExhaustForFloat, transmissionType, xmlHttpObject, isValid, isDel, carInfoXML) {
	this.CarID = carid;
	this.CarName = carName;
	this.EngineExhaustForFloat = engineExhaustForFloat;
	this.TransmissionType = transmissionType;
	this.XmlHttpObject = xmlHttpObject;
	this.IsValid = isValid;
	this.IsDel = isDel;
	this.CarInfoXML = carInfoXML;
}

// 显示对比
function initPageForCompare(root) {
	ComparePageObject.ArrCarInfo.length = 0;
	ComparePageObject.ArrPageContent.length = 0;
	ComparePageObject.ValidCount = 0;

	var pageDiv = document.getElementById(ComparePageObject.PageDivContentID);
	if (pageDiv)
	{ ComparePageObject.PageDivContentObj = pageDiv; }
	else
	{ return; }
	ComparePageObject.IsIE = CheckBrowserForCompare();
	ComparePageObject.RootPath = root;
	var compare_msg
	if (ComparePageObject.CarIDAndNames && ComparePageObject.CarIDAndNames != "")
	{ compare_msg = ComparePageObject.CarIDAndNames; }
	else {
		if (document.getElementById("compareheadHasDataUL"))
		{ document.getElementById("compareheadHasDataUL").innerHTML = "<li>暂无参数</li>"; }
		return;
	}
	if (!compare_msg) {
		// 无车型 
	}
	else {
		var compare_msg_arr = compare_msg.split("|");
		ComparePageObject.XmlSrcLength = compare_msg_arr.length;
	}
	for (var i = 0; i < ComparePageObject.XmlSrcLength; i++) {
		var carCookie = compare_msg_arr[i].split(",");
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
		startRequestForCompare(carinfo);
	}
	if (ComparePageObject.ValidCount > 0) {
		setExhaustAndTransmissionType();
		resetTableWidth();
		createPageForCompare(false);
	}
}

// page method --------------------------

// 取需要对比车数据
function startRequestForCompare(carInfo) {
	carInfo.XmlHttpObject = createXMLHttpRequestForCompare();
	if (ComparePageObject.IsIE) {
		carInfo.XmlHttpObject.onreadystatechange = function() { handleStateChangeForGetCarCompareData(carInfo) };
	}
	carInfo.XmlHttpObject.open("GET", ComparePageObject.CarInfoPath + carInfo.CarID, false);
	carInfo.XmlHttpObject.send(null);
	if (!ComparePageObject.IsIE) {
		addCarXmlObjectToArray(carInfo, carInfo.XmlHttpObject.responseXML);
	}
}

function handleStateChangeForGetCarCompareData(carInfo) {
	if (carInfo.XmlHttpObject.readyState == 4 && carInfo.XmlHttpObject.status == 200) {
		addCarXmlObjectToArray(carInfo, carInfo.XmlHttpObject.responseXML);
	}
}

function addCarXmlObjectToArray(carInfo, xmlObj) {
	carInfo.CarInfoXML = xmlObj;
	carInfo.IsValid = true;
	ComparePageObject.ArrCarInfo.push(carInfo);
	ComparePageObject.ValidCount++;
}

function checkCarIsShowForeach(index) {
	return !ComparePageObject.ArrCarInfo[index].IsDel;
}

function createPageForCompare(isDelSame) {
	ComparePageObject.IsDelSame = isDelSame;
	var loopCount = arrField.length;
	ComparePageObject.ArrPageContent.push("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" id=\"compareTable\" style=\"width:" + ComparePageObject.TableWidth + "px;\">"); //width: 1634px;\" >");//style=\"width: 1634px;\"
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

	ComparePageObject.ArrPageContent.push("</table>");
	// end
	//    ComparePageObject.ArrPageContent.push("<div class=\"car_compare_listbtn\">");
	//    ComparePageObject.ArrPageContent.push("<ul>");
	//    //    ComparePageObject.ArrPageContent.push("<li id=\"LiIsHidAll\" class=\"s\"><a id=\"LiIsHidAllA\" href=\"javascript:showHidAll();\">收起列表</a></li>");    
	//    //    ComparePageObject.ArrPageContent.push("<li class=\"d\"><a href=\"javascript:window.print();\">打印列表</a></li>");    
	//    //    ComparePageObject.ArrPageContent.push("<li class=\"b\"><a href=\"javascript:document.execCommand('Saveas',false,'配置参数.html');\">保存列表</a></li>");   
	//    ComparePageObject.ArrPageContent.push("</ul><div class=\"clear\"></div></div>");

	if (ComparePageObject.PageDivContentObj) {
		ComparePageObject.PageDivContentObj.innerHTML = ComparePageObject.ArrPageContent.join("");
	}
	ComparePageObject.ArrPageContent.length = 0;
	if (ComparePageObject.IsNeedSelect) {
		if (ComparePageObject.XmlObjectForSelect) {
			// not first page load 
			// parseXMLForSelectControl(); 
		}
		else {
			// intiPageSelectControl(); 
		}
	}
}

function setExhaustAndTransmissionType() {
	// alert("4.1L" > "4.0L"); sort
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
				//                arrExhaust.push("&nbsp;&nbsp;");
				//                arrExhaust.push("<input type=\"checkbox\" id=\"checkboxExhaust_" + ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + "\" value=\"" + ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + "\" onclick=\"checkExhaust(this);\" >");
				//                arrExhaust.push(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat);
				//                arrExhaust.push("&nbsp;&nbsp;");
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
			arrExhaust.push("&nbsp;");
			arrExhaust.push("<input type=\"checkbox\" id=\"checkboxExhaust_" + arrTempExhaust[i] + "\" value=\"" + arrTempExhaust[i] + "\" onclick=\"checkExhaust(this);\" >");
			arrExhaust.push(arrTempExhaust[i]);
			// arrExhaust.push("&nbsp;&nbsp;");
		}
	}
	// 变速器排序
	if (arrTempTransmissionType.length > 0) {
		arrTempTransmissionType.sort();
		for (var i = 0; i < arrTempTransmissionType.length; i++) {
			if (arrTempTransmissionType[i] == 1) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_1\" value=\"1\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("手动");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
			if (arrTempTransmissionType[i] == 2) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_2\" value=\"2\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("自动");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
			if (arrTempTransmissionType[i] == 3) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_3\" value=\"3\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("手自一体");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
			if (arrTempTransmissionType[i] == 4) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_4\" value=\"4\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("半自动");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
			if (arrTempTransmissionType[i] == 5) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_5\" value=\"5\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("CVT无级变速");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
			if (arrTempTransmissionType[i] == 6) {
				arrTransmissionType.push("&nbsp;");
				arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_6\" value=\"6\" onclick=\"checkTransmissionType(this);\">");
				arrTransmissionType.push("双离合");
				// arrTransmissionType.push("&nbsp;&nbsp;");
			}
		}
	}
	var tempCompareheadHasData = (exhaustCount > 1 ? arrExhaust.join("") : "") + (transmissionTypeCount > 1 ? arrTransmissionType.join("") : "");
	if (exhaustCount > 1 || transmissionTypeCount > 1) {
		document.getElementById("compareheadHasData").innerHTML = "<b>可选车型：</b>" + tempCompareheadHasData;
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
	resetTableWidth();
	// createPageForCompare(false);
	createPageForCompare(ComparePageObject.IsOperateTheSame);
}

// create pic for compare
function createPic() {
	ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" >");
	ComparePageObject.ArrPageContent.push("<th width=\"159\"><h2>参数配置列表</h2>");
	ComparePageObject.ArrPageContent.push("</th>");

	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {

			if (isShowLoop != 0 && (isShowLoop % ComparePageObject.MaxTDLeft) == 0) {
				ComparePageObject.ArrPageContent.push("<th width=\"159\"><h2>参数配置列表</h2>");
				ComparePageObject.ArrPageContent.push("</th>");
			}

			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (isShowLoop == 0 && ComparePageObject.IsNeedFirstColor)
				{ ComparePageObject.ArrPageContent.push("<td width=\"158\" id=\"td_" + i + "\" class=\"f\">"); }
				else
				{ ComparePageObject.ArrPageContent.push("<td width=\"158\" id=\"td_" + i + "\" >"); }
				try {
					var pic = "";
					if (ComparePageObject.IsIE)
					{ pic = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/CarImg/@PValue").value; }
					else
					{ pic = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/CarImg/@PValue"); }
					var altString = ComparePageObject.IsNeedDrag ? "按住鼠标左键，可拖动到其他列" : "";
					//                    if (pic.length < 1)
					//                    { ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><img width=\"120\" id=\"img_" + i + "\" name=\"dragImg\" height=\"80\" src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif\" alt=\"" + altString + "\" /></div>"); }
					//                    else
					//                    { ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><img width=\"120\" id=\"img_" + i + "\" name=\"dragImg\" height=\"80\" src=\"" + pic + "\" alt=\"" + altString + "\" /></div>"); }
					if (ComparePageObject.IsNeedDrag) {
						if (isShowLoop == 0) {
							if (checkIsRightEnd(i)) {
								ComparePageObject.ArrPageContent.push("<div></div>");
							}
							else {
								ComparePageObject.ArrPageContent.push("<div><a onmouseover=\"setColorByEleID('" + i + "','FF0000');\" onmouseout=\"setColorByEleID('" + i + "','164A84');\" href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"r\">右移</a><a class=\"d\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a></div>");
							}
						}
						else {
							if (checkIsRightEnd(i)) {
								ComparePageObject.ArrPageContent.push("<div><a onmouseover=\"setColorByEleID('" + i + "','FF0000');\" onmouseout=\"setColorByEleID('" + i + "','164A84');\" href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"l\">左移</a><a class=\"d\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a></div>");
							}
							else {
								ComparePageObject.ArrPageContent.push("<div><a onmouseover=\"setColorByEleID('" + i + "','FF0000');\" onmouseout=\"setColorByEleID('" + i + "','164A84');\" href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"l\">左移</a><a onmouseover=\"setColorByEleID('" + i + "','FF0000');\" onmouseout=\"setColorByEleID('" + i + "','164A84');\" href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"r\">右移</a><a class=\"d\" href=\"javascript:delCarToCompare('" + i + "');\">删除</a></div>");
							}
						}
					}
					else
					{ ComparePageObject.ArrPageContent.push("<div></div>"); }
				}
				catch (err)
                { }
				ComparePageObject.ArrPageContent.push("</td>");
			}
			isShowLoop++;
		}
	}

	//when less
	if (ComparePageObject.NeedBlockTD < ComparePageObject.MaxTDLeft) {
		for (var i = 0; i < ComparePageObject.MaxTDLeft - ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrPageContent.push("<td width=\"158\" >&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}

	ComparePageObject.ArrPageContent.push("");
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create param for compare
function createPara(arrFieldRow) {
	// when Perf_ZongHeYouHao compute avg
	if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
		ComparePageObject.ComputeSum = 0;
		ComparePageObject.ComputeCount = 0;
		ComparePageObject.ComputeAvg = 0;
	}

	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {

			if (isShowLoop != 0 && (isShowLoop % ComparePageObject.MaxTDLeft) == 0) {
				if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
					arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "##Avg##</th>");
				}
				else {
					arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
				}
			}

			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (isShowLoop == 0 && ComparePageObject.IsNeedFirstColor) {
					arrTemp.push("<td width=\"158\" class=\"f\"><div class=\"w158\">");
				}
				else {
					arrTemp.push("<td width=\"158\"><div class=\"w158\">");
				}

				try {
					var field = "";
					if (ComparePageObject.IsIE) {
						if (ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue")) {
							field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue").value;
							if (field.length > 0) {
								if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
									computeWhenNeedAvg(field);
								}
								field += " " + arrFieldRow["unit"];
							}
							if (arrSame.length < 1)
							{ arrSame.push(field); }
							else {
								if (field == arrSame[0])
								{ firstSame = firstSame && true; }
								else
								{ firstSame = firstSame && false; }
							}
						}
						else {
						}
					}
					else {
						field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue");
						if (field.length > 0) {
							if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
								computeWhenNeedAvg(field);
							}
							field += " " + arrFieldRow["unit"];
						}
						if (arrSame.length < 1)
						{ arrSame.push(field); }
						else {
							if (field == arrSame[0])
							{ firstSame = firstSame && true; }
							else
							{ firstSame = firstSame && false; }
						}
					}

					if (field.indexOf("待查") >= 0 || field == "") {
						isAllunknown = true && isAllunknown;
					}
					else {
						isAllunknown = false;
					}
					if (field.indexOf("有") == 0)
					{ field = "●"; }
					if (field.indexOf("选配") == 0)
					{ field = "○"; }
					if (field.indexOf("无") == 0)
					{ field = "-"; }
					arrTemp.push(field);
					// modified by chengl Dec.28.2009 for calculator
					if (arrFieldRow["sFieldName"] == "CarReferPrice" && field != "无" && field != "待查") {
						arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + ComparePageObject.ArrCarInfo[i].CarID + "\"  target=\"_blank\"></a>");
					}
				}
				catch (err) {
					arrTemp.push("-");
					firstSame = firstSame && false;
				}
				arrTemp.push("</div></td>");
			}
			else {
				arrTemp.push("<td width=\"158\">null");
				arrTemp.push("</td>");
			}
			isShowLoop++;
		}

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

			if (ComparePageObject.LoopTRColor % 2 == 1) {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			else {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}

			if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"]) && ComparePageObject.ComputeSum > 0 && ComparePageObject.ComputeCount > 1) {
				// 显示平均值
				ComparePageObject.ComputeAvg = (ComparePageObject.ComputeSum / ComparePageObject.ComputeCount).toFixed(1);
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"] + "<br/>均值:" + ComparePageObject.ComputeAvg + " " + arrFieldRow["unit"]);
				ComparePageObject.ArrPageContent.push("</th>");
				ComparePageObject.ArrPageContent.push(arrTemp.join("").replace(/##Avg##/g, "<br/>均值:" + ComparePageObject.ComputeAvg + " " + arrFieldRow["unit"]));
			}
			else {
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
				ComparePageObject.ArrPageContent.push("</th>");
				ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			}
			ComparePageObject.LoopTRColor++;
		}
		else {
			return;
		}
	}
	//when less
	if (ComparePageObject.NeedBlockTD < ComparePageObject.MaxTDLeft) {
		for (var i = 0; i < ComparePageObject.MaxTDLeft - ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrPageContent.push("<td width=\"158\" ><div class=\"w158\">&nbsp;");
			ComparePageObject.ArrPageContent.push("</div></td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function computeWhenNeedAvg(pvalue) {
	if (pvalue != "" && !isNaN(pvalue)) {
		var v = 1 * pvalue;
		if (v > 0) {
			ComparePageObject.ComputeSum += v;
			ComparePageObject.ComputeCount++;
		}
	}
}

function checkParamIsNeedComputeAvg(fieldName) {
	var isNeed = false;
	if (fieldName == "Perf_ZongHeYouHao") {
		isNeed = true;
	}
	return isNeed;
}

// create multi param for compare
function createMulti(arrFieldRow) {
	var paramArray = arrFieldRow["sFieldName"].split(',');
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

			if (isShowLoop != 0 && (isShowLoop % ComparePageObject.MaxTDLeft) == 0) {
				arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
			}
			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (isShowLoop == 0 && ComparePageObject.IsNeedFirstColor) {
					arrTemp.push("<td width=\"158\" class=\"f\"><div class=\"w158\">");
				}
				else {
					arrTemp.push("<td width=\"158\"><div class=\"w158\">");
				}
				try {
					var multiField = "";
					for (var pint = 0; pint < paramArray.length; pint++) {
						// loop every param
						var field = "";
						if (ComparePageObject.IsIE) {
							// IE
							if (ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + paramArray[pint] + "/@PValue")) {
								field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + paramArray[pint] + "/@PValue").value;
								if (field.length > 0) {
									field += unitArray[pint];
									multiField = multiField + joinCodeArray[pint] + field;
								}
							}
						}
						else {
							//FF
							field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + paramArray[pint] + "/@PValue");
							if (field.length > 0) {
								field += unitArray[pint];
								multiField = multiField + joinCodeArray[pint] + field;
							}
						}
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
					{ multiField = "●"; }
					if (multiField.indexOf("选配") == 0)
					{ multiField = "○"; }
					if (multiField.indexOf("无") == 0)
					{ multiField = "-"; }
					arrTemp.push(multiField);
				}
				catch (err) {
					arrTemp.push("-");
					firstSame = firstSame && false;
				}
				arrTemp.push("</div></td>");
			}
			else {
				arrTemp.push("<td width=\"158\">null");
				arrTemp.push("</td>");
			}
			isShowLoop++;
		}
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
			if (ComparePageObject.LoopTRColor % 2 == 1) {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			else {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			ComparePageObject.ArrPageContent.push("<th>");
			ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
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
		for (var i = 0; i < ComparePageObject.MaxTDLeft - ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrPageContent.push("<td width=\"158\" ><div class=\"w158\">&nbsp;");
			ComparePageObject.ArrPageContent.push("</div></td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
	//-----------

}

// create bar for compare
function createBar(arrFieldRow) {
	ComparePageObject.LoopTRColor = 0; // reset color for tr
	ComparePageObject.ArrTempBarHTML.length = 0;
	ComparePageObject.ArrTempBarHTML.push("<tr class=\"car_compare_name\">");
	ComparePageObject.ArrTempBarHTML.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_0\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {
			if (isShowLoop != 0 && (isShowLoop % ComparePageObject.MaxTDLeft) == 0) {
				ComparePageObject.ArrTempBarHTML.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_" + i + "\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
			}

			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (isShowLoop == 0 && ComparePageObject.IsNeedFirstColor)
				{ ComparePageObject.ArrTempBarHTML.push("<td width=\"158\" class=\"f\">"); }
				else
				{ ComparePageObject.ArrTempBarHTML.push("<td width=\"158\" >"); }
				var cs_Name = "";
				var car_year = "";
				var cs_allSpell = "";
				var carProduceState = "";
				try {
					if (ComparePageObject.IsIE) {
						cs_Name = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_ShowName/@PValue").value;
						car_year = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Car_YearType/@PValue").value;
						cs_allSpell = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_AllSpell/@PValue").value.toLowerCase();
						carProduceState = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Car_ProduceState/@PValue").value;
					}
					else {
						cs_Name = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_ShowName/@PValue");
						car_year = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Car_YearType/@PValue");
						cs_allSpell = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_AllSpell/@PValue").toLowerCase();
						carProduceState = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Car_ProduceState/@PValue");
					}
					if (carProduceState == "停产")
					{ carProduceState = " <span style=\"color:CC0000;\">停产</span>"; }
					else
					{ carProduceState = ""; }
					if (car_year.length >= 4) {
						car_year = " " + car_year + "款";
					}
					else {
						car_year = "";
					}
				}
				catch (err)
                { }
				if (ComparePageObject.ColorChangeIndex >= 0 && ComparePageObject.ColorChangeIndex == i && arrFieldRow["sTrPrefix"] == "1") {
					ComparePageObject.ArrTempBarHTML.push("<a style=\"color:#FF0000\" id=\"compareBar_" + arrFieldRow["sTrPrefix"] + "_index_" + i + "\" href=\"/" + cs_allSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/\" target=\"_blank\"><b>" + car_year + " " + ComparePageObject.ArrCarInfo[i].CarName + "</b></a></td>");
				}
				else {
					ComparePageObject.ArrTempBarHTML.push("<a id=\"compareBar_" + arrFieldRow["sTrPrefix"] + "_index_" + i + "\" href=\"/" + cs_allSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/\" target=\"_blank\"><b>" + car_year + " " + ComparePageObject.ArrCarInfo[i].CarName + carProduceState + "</b></a></td>");
				}
			}
			isShowLoop++;
		}

	}
	//when less
	if (ComparePageObject.NeedBlockTD < ComparePageObject.MaxTDLeft) {
		for (var i = 0; i < ComparePageObject.MaxTDLeft - ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrTempBarHTML.push("<td width=\"158\" >&nbsp;");
			ComparePageObject.ArrTempBarHTML.push("</td>");
		}
	}
	ComparePageObject.ArrTempBarHTML.push("</tr>");
}

// create price for compare
function createPrice(arrFieldRow) {
	if (ComparePageObject.LoopTRColor % 2 == 1) {
		ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
	}
	else {
		ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
	}
	ComparePageObject.LoopTRColor++;
	ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
	var isShowLoop = 0;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// check every Car is show
		if (checkCarIsShowForeach(i)) {

			if (isShowLoop != 0 && (isShowLoop % ComparePageObject.MaxTDLeft) == 0) {
				ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
			}

			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (isShowLoop == 0 && ComparePageObject.IsNeedFirstColor)
				{ ComparePageObject.ArrPageContent.push("<td width=\"158\" class=\"f\">"); }
				else
				{ ComparePageObject.ArrPageContent.push("<td width=\"158\">"); }

				try {
					var field = "";
					if (ComparePageObject.IsIE) {
						field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue").value;

					}
					else {
						field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue");
					}
				}
				catch (err)
            { ComparePageObject.ArrPageContent.push("-"); }
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
	if (ComparePageObject.NeedBlockTD < ComparePageObject.MaxTDLeft) {
		for (var i = 0; i < ComparePageObject.MaxTDLeft - ComparePageObject.NeedBlockTD; i++) {
			ComparePageObject.ArrPageContent.push("<td width=\"158\" >&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function checkCurrentActive() {
	return true;
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

// hidd or show TR
function hiddenTR(prefixName) {
	var boolIsHid = true;
	var tableObject = document.getElementById('compareTable');
	for (var i = 0; i < tableObject.rows.length; i++) {
		if (tableObject.rows[i].id.indexOf("tr" + prefixName + "_") == 0) {
			if (tableObject.rows[i].style.display == "none") {
				tableObject.rows[i].style.display = "";
				boolIsHid = false;
			}
			else {
				tableObject.rows[i].style.display = "none";
				boolIsHid = true;
			}
		}
	}
	// 
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		var tableObject = document.getElementById('TRForSelect_' + prefixName + '_' + i);
		if (tableObject) {
			if (boolIsHid) {
				tableObject.className = "close";
			}
			else {
				tableObject.className = "";
			}
		}
	}
}

// show or hid compare list
function showHidAll() {
	var tableObject = document.getElementById('compareTable');
	for (var i = 0; i < tableObject.rows.length; i++) {
		if (tableObject.rows[i].id.indexOf("tr") == 0) {
			if (ComparePageObject.IsDeployAll) {
				tableObject.rows[i].style.display = "block";

			}
			else {
				tableObject.rows[i].style.display = "none";
			}
		}
	}
	ComparePageObject.IsDeployAll = !ComparePageObject.IsDeployAll;
	// FF 刷table
	if (!ComparePageObject.IsIE) {
		createPageForCompare(false);
	}
	//    // 展开全部文字切换
	var isDeployObj = document.getElementById('LiIsHidAllA');
	if (isDeployObj) {
		if (ComparePageObject.IsDeployAll) {
			if (ComparePageObject.IsIE)
			{ isDeployObj.innerText = '展开列表'; }
			else
			{ isDeployObj.textContent = '展开列表'; }
		}
		else {
			if (ComparePageObject.IsIE)
			{ isDeployObj.innerText = '收起列表'; }
			else
			{ isDeployObj.textContent = '收起列表'; }
		}
	}
	var isDeployObjLi = document.getElementById('LiIsHidAll');
	if (isDeployObjLi) {
		if (ComparePageObject.IsDeployAll) {
			isDeployObjLi.className = 's2';
		}
		else {
			isDeployObjLi.className = 's';
		}
	}
}

// reset compare list
function resetCompareCar(index) {
	if (ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
		var firstIndex = -1;
		for (var i = 0; i < index; i++) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel) {
				firstIndex = i;
				break;
			}
		}
		if (firstIndex >= 0) {
			swapArray(ComparePageObject.ArrCarInfo, i, index);
			// update the same car 
			createPageForCompare(false);
		}
	}
}

function moveLeftForCarCompare(index) {
	if (index > 0 && ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
		for (var i = index - 1; i >= 0; i--) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel) {
				// modified by chengl May.5.2010
				ComparePageObject.ColorChangeIndex = i;
				swapArray(ComparePageObject.ArrCarInfo, i, index);
				createPageForCompare(ComparePageObject.IsOperateTheSame);
				// setColorByEleID(index, '164A84');
				checkColorByTimer();
				// ComparePageObject.ColorChangeIndex = 0;
				break;
			}
		}
	}
}

function moveRightForCarCompare(index) {
	if (index >= 0 && (ComparePageObject.ValidCount - 1) > index && (ComparePageObject.ArrCarInfo.length - 1) > index) {
		for (i = index * 1 + 1; i < ComparePageObject.ArrCarInfo.length; i++) {
			if (!ComparePageObject.ArrCarInfo[i].IsDel) {
				// modified by chengl May.5.2010
				ComparePageObject.ColorChangeIndex = i;
				swapArray(ComparePageObject.ArrCarInfo, i, index);
				createPageForCompare(ComparePageObject.IsOperateTheSame);
				// setColorByEleID(index, '164A84');
				checkColorByTimer();
				// ComparePageObject.ColorChangeIndex = 0;
				break;
			}
		}
	}
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

// swap Array object
function swapArray(obj, index1, index2) {
	var temp = obj[index1];
	obj[index1] = obj[index2];
	obj[index2] = temp;
}

function createXMLHttpRequestForCompare() {
	if (window.ActiveXObject) {
		var xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) {
		var xmlHttp = new XMLHttpRequest();
	}
	return xmlHttp;
}

function CheckBrowserForCompare() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

function selectSingleNodeForFirefox(xmldom, path) {
	var xpe = new XPathEvaluator();
	var nsResolver = xpe.createNSResolver(xmldom.ownerDocument == null ? xmldom.documentElement : xmldom.ownerDocument.documentElement);
	var results = xpe.evaluate(path, xmldom, nsResolver, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
	if (results.singleNodeValue)
	{ return results.singleNodeValue.value; }
	else
	{ return ""; }
}

// set link color
function setColorByEleID(index, color) {
	if (ComparePageObject.ColorChangeIndex >= 0)
	{ return; }
	var objColor = document.getElementById("compareBar_1_index_" + index);
	if (objColor) {
		objColor.style.color = eval("'#'+color");
	}
}

// check color by Timer
function checkColorByTimer() {
	if (ComparePageObject.ColorChangeIndex >= 0) {
		if (ComparePageObject.ColorTimer) {
			ComparePageObject.ColorTimer = null;
			// clearTimeout(ComparePageObject.ColorTimer);
			// ComparePageObject.ColorChangeIndex = 0;
		}
		changeColorByTimer();
		// ComparePageObject.ColorTimer = setTimeout("changeColorByTimer()", 3000);
	}
}

function changeColorByTimer() {
	if (document.getElementById("compareBar_1_index_" + ComparePageObject.ColorChangeIndex)) {
		var color = document.getElementById("compareBar_1_index_" + ComparePageObject.ColorChangeIndex).style.color;  //+ '0f';
		var currentColor = ComparePageObject.IsIE ? color.substring(1, 7) : color.colorHex().substring(1, 7);
		if (currentColor.length < 6)
		{ return; }
		var cR = (parseInt(currentColor.substring(0, 2), 16)).toString(10) * 1;
		var cG = (parseInt(currentColor.substring(2, 4), 16)).toString(10) * 1;
		var cB = (parseInt(currentColor.substring(4, 6), 16)).toString(10) * 1;
		var cRstep = parseInt((255 - 22) / 50) * 1;
		var cGstep = parseInt((74 - 0) / 50) * 1;
		var cBstep = parseInt((132 - 0) / 50) * 1;
		cR = cR - cRstep;
		cG = cG + cGstep;
		cB = cB + cBstep;
		if (cR <= 22)
		{ cR = 22; }
		if (cG >= 74)
		{ cG = 74; }
		if (cB >= 132)
		{ cB = 132; }
		currentColor = (cR.toString(16).length == 2 ? cR.toString(16) : "0" + cR.toString(16)) + (cG.toString(16).length == 2 ? cG.toString(16) : "0" + cG.toString(16)) + (cB.toString(16).length == 2 ? cB.toString(16) : "0" + cB.toString(16));
		if (cR <= 22) {
			document.getElementById("compareBar_1_index_" + ComparePageObject.ColorChangeIndex).style.color = "#" + currentColor; // eval("'#'+currentColor");
			if (ComparePageObject.ColorTimer) {
				ComparePageObject.ColorTimer = null;
				// clearTimeout(ComparePageObject.ColorTimer);
			}
			ComparePageObject.ColorChangeIndex = -1;
			return;
		}
		else {
			document.getElementById("compareBar_1_index_" + ComparePageObject.ColorChangeIndex).style.color = "#" + currentColor;  //eval("'#'+currentColor");
			ComparePageObject.ColorTimer = setTimeout("changeColorByTimer()", 50);
		}
	}
}

//-------------------- add car to compare 
// add compare to cookie
function addCarToCompare(id, name, csName) {
	var compare = CookieForCompare.getCookie("ActiveNewCompare");
	var com_arr = null;
	if (compare) {
		com_arr = compare.split("|");
		if (compare.indexOf("id" + id + ",") >= 0) {
			alert("您选择的车型,已经在对比列表中!");
			return;
		}
	}
	else {
		com_arr = new Array();
	}
	// set car serial name for cookie
	var carSerial = CookieForCompare.getCookie("ActiveCarSerialCompare");
	var carSerial_arr = null;
	if (carSerial) {
		carSerial_arr = carSerial.split("|");
		if (carSerial_arr.length >= 10) {
			alert("对比车型不能多于11个");
			return;
		}
	}
	else {
		carSerial_arr = new Array();
	}
	com_arr.push('id' + id + ',' + name);
	CookieForCompare.clearCookie("ActiveNewCompare");
	CookieForCompare.setCookie("ActiveNewCompare", com_arr.join("|"));

	carSerial_arr.push('id' + id + ',' + csName);
	CookieForCompare.clearCookie("ActiveCarSerialCompare");
	CookieForCompare.setCookie("ActiveCarSerialCompare", carSerial_arr.join("|"));

	initPageForCompare(ComparePageObject.RootPath);
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
			resetTableWidth();
			createPageForCompare(false);
		}
		else {
			alert('至少留1个对比车型');
		}
	}
}

function resetTableWidth() {
	if (ComparePageObject.ValidCount > 0) {
		if (ComparePageObject.IsAutoNeedMoreTH) {
			var isValidcount = 0;
			for (var i = 0; i < ComparePageObject.ValidCount; i++) {
				if (!ComparePageObject.ArrCarInfo[i].IsDel)
				{ isValidcount++; }
			}
			if (isValidcount > 5) {
				ComparePageObject.TableWidth = (Math.ceil(isValidcount / 5)) * 159 + (158 * isValidcount);
			}
			else
			{ ComparePageObject.TableWidth = 159 + (158 * 5); }
			ComparePageObject.NeedBlockTD = isValidcount;
		}
	}
}

function checkIsChange(tdObj) {
	targetNum = tdObj.id.replace('td_', '');
	if (targetNum == currentTD || targetNum == "" || currentTD == "") {
	}
	else {
		swapArray(ComparePageObject.ArrCarInfo, currentTD, targetNum);
		targetNum = "";
		currentTD = "";
		createPageForCompare(false);
	}
}

function changeTRColorWhenOnMouse(obj, color) {
	if (obj)
	{ obj.style.background = color; }
}

//-------------------------------------
//十六进制颜色值的正则表达式
var reg = /^#([0-9a-fA-f]{3}|[0-9a-fA-f]{6})$/;
/*RGB颜色转换为16进制*/
String.prototype.colorHex = function() {
	var that = this;
	if (/^(rgb|RGB)/.test(that)) {
		var aColor = that.replace(/(?:\(|\)|rgb|RGB)*/g, "").split(",");
		var strHex = "#";
		for (var i = 0; i < aColor.length; i++) {
			var hex = (parseInt(aColor[i], 10)).toString(16);
			if (hex.length == 1)
			{ hex = "0" + hex; }
			strHex += hex;
		}
		if (strHex.length !== 7) {
			strHex = that;
		}
		return strHex;
	} else if (reg.test(that)) {
		var aNum = that.replace(/#/, "").split("");
		if (aNum.length === 6) {
			return that;
		} else if (aNum.length === 3) {
			var numHex = "#";
			for (var i = 0; i < aNum.length; i += 1) {
				numHex += (aNum[i] + aNum[i]);
			}
			return numHex;
		}
	} else {
		return that;
	}
};

//-------------------------------------------------
/*16进制颜色转为RGB格式*/
String.prototype.colorRgb = function() {
	var sColor = this.toLowerCase();
	if (sColor && reg.test(sColor)) {
		if (sColor.length === 4) {
			var sColorNew = "#";
			for (var i = 1; i < 4; i += 1) {
				sColorNew += sColor.slice(i, i + 1).concat(sColor.slice(i, i + 1));
			}
			sColor = sColorNew;
		}
		//处理六位的颜色值 
		var sColorChange = [];
		for (var i = 1; i < 7; i += 2) {
			sColorChange.push(parseInt("0x" + sColor.slice(i, i + 2)));
		}
		return "RGB(" + sColorChange.join(",") + ")";
	} else {
		return sColor;
	}
};

// page method --------------------------
var arrField = [
   { sType: "fieldPic", sFieldName: "", sFieldTitle: "图片", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "基本信息", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "CarReferPrice", sFieldTitle: "厂家指导价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldName: "AveragePrice", sFieldTitle: "经销商报价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Car_RepairPolicy", sFieldTitle: "保修政策", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ExhaustForFloat", sFieldTitle: "排量（升）", sTrPrefix: "1", unit: "L", joinCode: "" },
// { sType: "fieldPara", sFieldName: "UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "1", unit: "", joinCode: "" },
   {sType: "fieldMulti", sFieldName: "UnderPan_ForwardGearNum,UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "1", unit: "档,", joinCode: ", " },
   { sType: "bar", sFieldName: "", sFieldTitle: "基本性能", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ZongHeYouHao", sFieldTitle: "综合工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiQuYouHao", sFieldTitle: "市区工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiJiaoYouHao", sFieldTitle: "市郊工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FuelCostPer100", sFieldTitle: "百公里等速油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_FuelCostPerRate100", sFieldTitle: "百公里等速油耗速度", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   {sType: "fieldPara", sFieldName: "Perf_MaxClimbDegree", sFieldTitle: "最大爬坡度", sTrPrefix: "2", unit: "%", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxClimbDegreeValue", sFieldTitle: "最大爬坡度(值)", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_MinWheelRadius", sFieldTitle: "最小转弯半径", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxPaddleDepth", sFieldTitle: "最大涉水深度", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_DriveType", sFieldTitle: "驱动方式", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_SeatNum", sFieldTitle: "乘员人数（含司机）", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_WindBlock", sFieldTitle: "风阻系数", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_SlackSpeedNoise", sFieldTitle: "车内怠速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_AveSpeedNoise", sFieldTitle: "车内等速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FEaxleWeight", sFieldTitle: "前轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_BEaxleWeight", sFieldTitle: "后轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_Weight", sFieldTitle: "整备质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_TotalWeight", sFieldTitle: "允许总质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_Tonnage", sFieldTitle: "最大承载质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "车身结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Doors", sFieldTitle: "车门数", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Type", sFieldTitle: "车身型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Islong", sFieldTitle: "车身加长型", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_TipType", sFieldTitle: "车顶型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Louver", sFieldTitle: "天窗型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_LouverOCType", sFieldTitle: "天窗开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_sailType", sFieldTitle: "车篷型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_CalashOCType", sFieldTitle: "车篷开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_CalashOCTime", sFieldTitle: "车篷开合时间", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Struc", sFieldTitle: "车体结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "外部尺寸", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Length", sFieldTitle: "长", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Width", sFieldTitle: "宽", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Height", sFieldTitle: "高", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_WheelBase", sFieldTitle: "轴距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_FrontTread", sFieldTitle: "前轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_BackTread", sFieldTitle: "后轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_FhangLength", sFieldTitle: "前悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_BhangLength", sFieldTitle: "后悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_NearCorner", sFieldTitle: "接近角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_AwayCorner", sFieldTitle: "离去角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_MinGapFromEarth", sFieldTitle: "最小离地间隙", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "内部尺寸", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FheadSpace", sFieldTitle: "前排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BheadSpace", sFieldTitle: "后排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FjianSpace", sFieldTitle: "前排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BjianSpace", sFieldTitle: "后排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FFootSpace", sFieldTitle: "前排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BFootSpace", sFieldTitle: "后排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdHeight", sFieldTitle: "第三排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdJSpace", sFieldTitle: "第三排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdFootSpace", sFieldTitle: "第三排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpwidth", sFieldTitle: "行李箱最大开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageMWidth", sFieldTitle: "行李箱最小开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpheight", sFieldTitle: "行李箱开口离地高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageHeight", sFieldTitle: "行李箱内部高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageDepth", sFieldTitle: "行李箱内部深度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageWidth", sFieldTitle: "行李箱内部最大宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_TrunkCapacity", sFieldTitle: "行李箱容积", sTrPrefix: "5", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_TrunkCapacityE", sFieldTitle: "行李箱最大拓展容积", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpOpenType", sFieldTitle: "行李箱打开方式", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "燃油&发动机", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelCapacity", sFieldTitle: "燃油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_SboxSpace", sFieldTitle: "副油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelType", sFieldTitle: "燃料类型", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelTab", sFieldTitle: "燃油标号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_SupplyType", sFieldTitle: "燃料供给型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Type", sFieldTitle: "型号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Company", sFieldTitle: "生产厂家", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Exhaust", sFieldTitle: "排量", sTrPrefix: "6", unit: "mL", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxSpeed", sFieldTitle: "最高转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxPower", sFieldTitle: "最大功率-功率值", sTrPrefix: "6", unit: "kW", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_PowerSpeed", sFieldTitle: "最大功率-转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxNJ", sFieldTitle: "最大扭矩-扭矩值", sTrPrefix: "6", unit: "Nm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_NJSpeed", sFieldTitle: "最大扭矩—转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderRank", sFieldTitle: "气缸排列型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Location", sFieldTitle: "发动机位置", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_InhaleType", sFieldTitle: "进气型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_AddPressType", sFieldTitle: "增压方式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Camshaft", sFieldTitle: "凸轮轴", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_SpTech", sFieldTitle: "特有技术", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderNum", sFieldTitle: "汽缸数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValvePerCylinder", sFieldTitle: "每缸气门数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderDM", sFieldTitle: "缸径", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Route", sFieldTitle: "行程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValveTM", sFieldTitle: "可变气门正时", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValveVTM", sFieldTitle: "可变气门升程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CompressRat", sFieldTitle: "压缩比", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderMaterial", sFieldTitle: "缸体材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderTMaterial", sFieldTitle: "缸盖材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_EnvirStandard", sFieldTitle: "环保标准", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_EGR", sFieldTitle: "废气再循环（EGR）", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Catalyze", sFieldTitle: "三元催化器", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_AIFluid_VL", sFieldTitle: "防冻液容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Eoil_VL", sFieldTitle: "机油容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "底盘操控", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RedirectorType", sFieldTitle: "转向机形式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_DriveAsistTurn", sFieldTitle: "转向助力", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_AsistTurnTune", sFieldTitle: "随速助力转向调节(EPS)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_circleNum", sFieldTitle: "方向盘回转总圈数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ForwardGearNum", sFieldTitle: "前进档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_GearChangePosition", sFieldTitle: "变速箱变速杆位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseGearNum", sFieldTitle: "分动箱档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseType", sFieldTitle: "分动箱型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseControl", sFieldTitle: "分动箱操纵", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ReductionRatio", sFieldTitle: "主减速比", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_CentralDiffLock", sFieldTitle: "中央差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontAxleDiffLock", sFieldTitle: "前轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearAxleDiffLock", sFieldTitle: "后轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontBrakeType", sFieldTitle: "前制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearBrakeType", sFieldTitle: "后制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ParkingBrake", sFieldTitle: "驻车制动器", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ShockAbsorberType", sFieldTitle: "减震器类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontSuspensionType", sFieldTitle: "前悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearSuspensionType", sFieldTitle: "后悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SuspensionHeightControl", sFieldTitle: "悬挂高度调节", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RimMaterial", sFieldTitle: "轮毂材料", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontRimStandard", sFieldTitle: "前轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearRimStandard", sFieldTitle: "后轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontTyreStandard", sFieldTitle: "前轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearTyreStandard", sFieldTitle: "后轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SpareWheelStandard", sFieldTitle: "备胎类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SpareWheelPosition", sFieldTitle: "备胎位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ZeroPressureDrive", sFieldTitle: "零压续行(零胎压继续行驶)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "外部配置", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BodyColor", sFieldTitle: "车身颜色", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_PaintType", sFieldTitle: "车身油漆", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_InductEmpennage", sFieldTitle: "后导流尾翼", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_TopSnelf", sFieldTitle: "车顶行李箱架", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Besiege", sFieldTitle: "运动包围", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Doorglasstype", sFieldTitle: "车门玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_CarWindow", sFieldTitle: "车窗", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Fwindglasstype", sFieldTitle: "前风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_LockCease", sFieldTitle: "电动窗锁止功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Bwindglasstype", sFieldTitle: "后风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_AvoidNipHead", sFieldTitle: "电动窗防夹功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BackCurtain", sFieldTitle: "后窗遮阳帘", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BWindHot", sFieldTitle: "后风窗加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BBrushSensor", sFieldTitle: "后雨刷器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FBrushSensor", sFieldTitle: "雨刷传感器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorDazzle", sFieldTitle: "内后视镜防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorElecTune", sFieldTitle: "外后视镜电动调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorFold", sFieldTitle: "外后视镜电动折叠功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorHot", sFieldTitle: "外后视镜加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrormemory", sFieldTitle: "外后视镜记忆功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FrontLightType", sFieldTitle: "前照灯类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightDazzle", sFieldTitle: "会车前灯防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightSteer", sFieldTitle: "前大灯随动转向", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightDelay", sFieldTitle: "前大灯延时关闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FfogLamp", sFieldTitle: "前雾灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightClose", sFieldTitle: "前大灯自动开闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightHeightTune", sFieldTitle: "前照灯照射高度调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightAutoClean", sFieldTitle: "前照灯自动清洗功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_SideLight", sFieldTitle: "侧转向灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_luggageLight", sFieldTitle: "行李箱灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_PerchStopLight", sFieldTitle: "高位(第三)制动灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "内饰", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerMaterial", sFieldTitle: "方向盘表面材料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerTuneDirection", sFieldTitle: "方向盘调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerRange", sFieldTitle: "方向盘幅数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerMomery", sFieldTitle: "方向盘记忆设置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerTuneType", sFieldTitle: "方向盘调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerEtc", sFieldTitle: "方向盘换档", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelDisplay", sFieldTitle: "仪表板显示型式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelLightColor", sFieldTitle: "仪表板背光颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelLumTune", sFieldTitle: "仪表板亮度可调", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Hud", sFieldTitle: "HUD抬头数字显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Computer", sFieldTitle: "行车电脑", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Clock", sFieldTitle: "时钟", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Otemperature", sFieldTitle: "车外温度显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_OilWarn", sFieldTitle: "燃油不足警告方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Tach", sFieldTitle: "转速表", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Guide", sFieldTitle: "罗盘/指南针", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Altitude", sFieldTitle: "海拔仪", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatColor", sFieldTitle: "座椅颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatMaterial", sFieldTitle: "座椅面料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SportSeat", sFieldTitle: "运动座椅", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatKnead", sFieldTitle: "座椅按摩功能", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatHot", sFieldTitle: "驾驶座座椅加热", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatProp", sFieldTitle: "驾驶座腰部支撑调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatTuneType", sFieldTitle: "驾驶座座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatTuneDirection", sFieldTitle: "驾驶座座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatMomery", sFieldTitle: "驾驶座椅调节记忆位置组数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DASeatTuneType", sFieldTitle: "副驾驶座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DASeatTuneDirection", sFieldTitle: "副驾驶座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FCenterArmrest", sFieldTitle: "前座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BCenterArmrest", sFieldTitle: "后座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSafePillow", sFieldTitle: "主动式安全头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatPillow", sFieldTitle: "后座椅头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FSeatPillowA", sFieldTitle: "前座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatPillowA", sFieldTitle: "后座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ChildSeatFix", sFieldTitle: "儿童安全座椅固定装置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatLieRate", sFieldTitle: "后排座位放倒比例", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "影音空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Cassette", sFieldTitle: "卡带", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Radio", sFieldTitle: "收音机", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MP3Player", sFieldTitle: "MP3", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_VCDPlayer", sFieldTitle: "VCD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CDPlayer", sFieldTitle: "CD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CDNum", sFieldTitle: "CD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DVDPlayer", sFieldTitle: "DVD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DVDNum", sFieldTitle: "DVD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Video", sFieldTitle: "车载电视", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CCEscreen", sFieldTitle: "中控台液晶屏", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BEscreen", sFieldTitle: "后排液晶显示器", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Audiobrand", sFieldTitle: "音响品牌", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_LoudHailer", sFieldTitle: "扬声器数量", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AirCSystem", sFieldTitle: "空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AirCType", sFieldTitle: "空调控制方式", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_HumidityTune", sFieldTitle: "湿度调节", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_TemperAreaCount", sFieldTitle: "温区个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BleakAirNum", sFieldTitle: "后排出风口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_leakAirNum", sFieldTitle: "出风口个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ExternalAudioInterface", sFieldTitle: "外接音源接口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "便利功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PowerNum", sFieldTitle: "车内电源插口数量", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_12VPower", sFieldTitle: "车内电源电压", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ReadingLight", sFieldTitle: "车厢前阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BReadingLight", sFieldTitle: "车厢后阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ILightCloseDelay", sFieldTitle: "车内灯光延时关闭", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SpeedCruise", sFieldTitle: "定速巡航系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_GPS", sFieldTitle: "GPS电子导航", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ElecLimitSpeed", sFieldTitle: "电子限速", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RRadar", sFieldTitle: "倒车雷达", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RImage", sFieldTitle: "倒车影像", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CenterControlLock", sFieldTitle: "中控门锁", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TyrePressureWatcher", sFieldTitle: "胎压检测装置", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_POwerCleaner", sFieldTitle: "车门/行李箱电吸", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_WireLink", sFieldTitle: "无线上网功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Tel", sFieldTitle: "车载电话", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Bluetooth", sFieldTitle: "蓝牙系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_carFridge", sFieldTitle: "车载冰箱", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MultiFuncSteer", sFieldTitle: "多功能方向盘", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MultiFuncsSteer", sFieldTitle: "多功能方向盘功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AIgnitionSys", sFieldTitle: "无钥匙点火系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Rckey", sFieldTitle: "遥控钥匙", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_RemoteTrunk", sFieldTitle: "遥控行李箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_COpenType", sFieldTitle: "行李箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_TrunkType", sFieldTitle: "行李箱盖开合方式", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_RemoteOilCover", sFieldTitle: "遥控油箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_OilLidOpen", sFieldTitle: "油箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CenterBox", sFieldTitle: "中央置物盒", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FCarShelf", sFieldTitle: "前排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BCarShelf", sFieldTitle: "后排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ClothesHook", sFieldTitle: "衣物挂钩", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FaceMirror", sFieldTitle: "遮阳板化妆镜", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "安全配置", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ABD", sFieldTitle: "ABD(自动制动差速器)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ABS", sFieldTitle: "ABS(刹车防抱死制动系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ASR", sFieldTitle: "ASR(驱动防滑装置)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_DSC", sFieldTitle: "DSC(动态稳定控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EBA", sFieldTitle: "EBA/EVA(紧急制动辅助系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EBD", sFieldTitle: "EBD/EBV(电子制动力分配)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EDS", sFieldTitle: "EDS(电子差速锁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ESP", sFieldTitle: "ESP(电子稳定程序)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_TCS", sFieldTitle: "TCS(牵引力控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_VATS", sFieldTitle: "电子防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EATS", sFieldTitle: "发动机防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_Other", sFieldTitle: "主动安全-其他", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_DriverGasBag", sFieldTitle: "驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_SubDriverGasBag", sFieldTitle: "副驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_FheadGasbag", sFieldTitle: "前排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BheadGasbag", sFieldTitle: "后排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_FsadGasbag", sFieldTitle: "前排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BsadGasbag", sFieldTitle: "后排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_SGasbagShrink", sFieldTitle: "副气囊锁止功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_GasbagNum", sFieldTitle: "气囊气帘数量", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BeltPosTune", sFieldTitle: "前安全带调节", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BeltPreTighten", sFieldTitle: "安全带预收紧功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_lifeBeltlimit", sFieldTitle: "安全带限力功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BackBelt", sFieldTitle: "后排安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BackthreespotBelt", sFieldTitle: "后排中间三点式安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ChildLock", sFieldTitle: "儿童锁", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ShrinkFootBrake", sFieldTitle: "溃缩式制动踏板", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_DoorAvoidHamCL", sFieldTitle: "车门防撞杆(防撞侧梁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_turnShrink", sFieldTitle: "可溃缩转向柱", sTrPrefix: "12", unit: "", joinCode: "" }
];