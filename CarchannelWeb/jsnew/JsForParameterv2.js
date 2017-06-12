// 车型频道参数配置

var ComparePageObject = {
    CarInfoPath: "http://car.bitauto.com/car/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path
    // CarInfoPath : "http://localhost:1036/CarchannelWeb.sln/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path 
    RootPath: "",    // root path
    ResourceDIR: "", // image dir
    PageDivContentID: "CarCompareContent",  // container id
    PageDivContentObj: null,   // container object
    IsIE: true, // client browser
    IsDelSame: false,  // is delete the same param
    IsDeployAll: false,
    IsOperateTheSame: false,
    // IsHidAll : false,
    XmlSrcLength: 0,
    XmlHttpForCompare: null,
    ArrCarInfo: new Array(),
    ArrPageContent: new Array(),
    ValidCount: 0,
    MaxTDLeft: 6,
    IsNeedSecTH: false, //146px
    IsNeedSelect: false,
    IsNeedBlockTD: true,
    NeedBlockTD: 0, // 134px
    CarIDAndNames: "",
    TableWidth: 1634,
    IsNeedDrag: true,
    IsAutoNeedMoreTH: true,
    ArrTempBarHTML: new Array()
}

// 车型对比信息
function CarCompareInfo(carid, carName, xmlHttpObject, isValid, carInfoXML) {
    this.CarID = carid;
    this.CarName = carName;
    this.XmlHttpObject = xmlHttpObject;
    this.IsValid = isValid;
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
        if (document.getElementById("compareheadHasData"))
        { document.getElementById("compareheadHasData").innerHTML = "暂无参数"; }
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
        var carinfo = new CarCompareInfo();
        carinfo.CarID = carid;
        carinfo.CarName = carName;
        startRequestForCompare(carinfo);
    }
    if (ComparePageObject.ValidCount > 0) {
        // createPageForCompare(false);
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

function createPageForCompare(isDelSame) {
    ComparePageObject.IsDelSame = isDelSame;

    if (ComparePageObject.IsNeedBlockTD) {
        ComparePageObject.NeedBlockTD = ComparePageObject.ValidCount >= 6 ? 0 : 6 - ComparePageObject.ValidCount;
        if (ComparePageObject.ValidCount == 10)
        { ComparePageObject.NeedBlockTD = 0; }
    }
    else
    { ComparePageObject.NeedBlockTD = 0; }
    ComparePageObject.IsNeedSecTH = ComparePageObject.ValidCount >= 6 ? true : false;
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
    ComparePageObject.ArrPageContent.push("<div class=\"car_compare_listbtn\">");
    ComparePageObject.ArrPageContent.push("<ul>");
    //    ComparePageObject.ArrPageContent.push("<li id=\"LiIsHidAll\" class=\"s\"><a id=\"LiIsHidAllA\" href=\"javascript:showHidAll();\">收起列表</a></li>");    
    //    ComparePageObject.ArrPageContent.push("<li class=\"d\"><a href=\"javascript:window.print();\">打印列表</a></li>");    
    //    ComparePageObject.ArrPageContent.push("<li class=\"b\"><a href=\"javascript:document.execCommand('Saveas',false,'配置参数.html');\">保存列表</a></li>");   
    ComparePageObject.ArrPageContent.push("</ul><div class=\"clear\"></div></div>");

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

// create pic for compare
function createPic() {
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" >");
    ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置");
    ComparePageObject.ArrPageContent.push("</th>");

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            if (i != 0 && (i % 6) == 0) {
                ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置");
                ComparePageObject.ArrPageContent.push("</th>");
            }
        }
        else {
            // insert Sec TH 
            if (i == ComparePageObject.MaxTDLeft) {
                ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置");
                ComparePageObject.ArrPageContent.push("</th>");
            }
        }
        if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
            if (i == 0)
            { ComparePageObject.ArrPageContent.push("<td width=\"134\" id=\"td_" + i + "\" onMouseOver=\"checkIsChange(this);\" class=\"f\">"); }
            else
            { ComparePageObject.ArrPageContent.push("<td width=\"134\" id=\"td_" + i + "\" onMouseOver=\"checkIsChange(this);\">"); }
            try {
                var pic = "";
                if (ComparePageObject.IsIE)
                { pic = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/CarImg/@PValue").value; }
                else
                { pic = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/CarImg/@PValue"); }
                var altString = ComparePageObject.IsNeedDrag ? "按住鼠标左键，可拖动到其他列" : "";
                if (pic.length < 1)
                { ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><img width=\"120\" id=\"img_" + i + "\" name=\"dragImg\" height=\"80\" src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif\" alt=\"" + altString + "\" /></div>"); }
                else
                { ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><img width=\"120\" id=\"img_" + i + "\" name=\"dragImg\" height=\"80\" src=\"" + pic + "\" alt=\"" + altString + "\" /></div>"); }
                if (ComparePageObject.IsNeedDrag)
                { ComparePageObject.ArrPageContent.push("<div><a href=\"javascript:resetCompareCar('" + ComparePageObject.ArrCarInfo[i].CarID + "');\" class=\"g\"></a><a href=\"javascript:delCarToCompare('" + ComparePageObject.ArrCarInfo[i].CarID + "')\" class=\"del\"></a></div>"); }
                else
                { ComparePageObject.ArrPageContent.push("<div></div>"); }
            }
            catch (err)
             { }
            ComparePageObject.ArrPageContent.push("</td>");
        }
    }

    //when less 
    if (ComparePageObject.NeedBlockTD > 0) {
        for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
            if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置");
                ComparePageObject.ArrPageContent.push("<p class=\"cy\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" /> 只显示差异项目</p>");
                ComparePageObject.ArrPageContent.push("</th>");
            }
            ComparePageObject.ArrPageContent.push("<td width=\"134\">&nbsp;");
            ComparePageObject.ArrPageContent.push("</td>");
        }
    }

    ComparePageObject.ArrPageContent.push("");
    ComparePageObject.ArrPageContent.push("</tr>");
}

// create param for compare
function createPara(arrFieldRow) {
    var firstSame = true;
    var isAllunknown = true;
    var arrSame = new Array();
    var arrTemp = new Array();
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            if (i != 0 && (i % 6) == 0) {
                arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        else {
            // insert Sec TH 
            if (i == ComparePageObject.MaxTDLeft) {
                arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
            if (i == 0) {
                arrTemp.push("<td width=\"134\" class=\"f\">");
            }
            else {
                arrTemp.push("<td width=\"134\">");
            }

            try {
                var field = "";
                if (ComparePageObject.IsIE) {
                    field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue").value;
                    if (field.length > 0)
                    { field += " " + arrFieldRow["unit"]; }
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
                    field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue");
                    if (field.length > 0)
                    { field += " " + arrFieldRow["unit"]; }
                    if (arrSame.length < 1)
                    { arrSame.push(field); }
                    else {
                        if (field == arrSame[0])
                        { firstSame = firstSame && true; }
                        else
                        { firstSame = firstSame && false; }
                    }
                }
                if (field.indexOf("待查") >= 0) {
                    isAllunknown = true && isAllunknown;
                }
                else {
                    isAllunknown = false;
                }
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
            arrTemp.push("</td>");
        }
        else {
            arrTemp.push("<td width=\"134\">null");
            arrTemp.push("</td>");
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
            ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\"><th>");
            ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
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
            if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
            ComparePageObject.ArrPageContent.push("<td width=\"134\">&nbsp;");
            ComparePageObject.ArrPageContent.push("</td>");
        }
    }
    ComparePageObject.ArrPageContent.push("</tr>");
}

// create multi param for compare
function createMulti(arrFieldRow) {
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
    ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            if (i != 0 && (i % 6) == 0) {
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        else {
            // insert Sec TH 
            if (i == ComparePageObject.MaxTDLeft) {
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
            if (i == 0)
            { ComparePageObject.ArrPageContent.push("<td width=\"134\" class=\"f\">"); }
            else
            { ComparePageObject.ArrPageContent.push("<td width=\"134\" >"); }

            try {
                var field = "";
                var arrMuti = new Array();
                var arrPara = arrFieldRow["sFieldName"].split(",");
                var arrUnit = arrFieldRow["unit"].split(",");
                if (ComparePageObject.IsIE) {
                    for (var j = 0; j < arrPara.length; j++) {
                        arrMuti.push(ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrPara[j] + "/@PValue").value + "" + arrUnit[j]);
                    }
                }
                else {
                    for (var j = 0; j < arrPara.length; j++) {
                        arrMuti.push(selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrPara[j] + "/@PValue") + "" + arrUnit[j]);
                    }
                }
            }
            catch (err)
            { }
            ComparePageObject.ArrPageContent.push(arrMuti.join(arrFieldRow["joinCode"]) + "</td>");
        }
    }

    //when less 
    if (ComparePageObject.NeedBlockTD > 0) {
        for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
            if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
            ComparePageObject.ArrPageContent.push("<td width=\"134\" >&nbsp;");
            ComparePageObject.ArrPageContent.push("</td>");
        }
    }
    ComparePageObject.ArrPageContent.push("</tr>");
}

// create bar for compare
function createBar(arrFieldRow) {
    ComparePageObject.ArrTempBarHTML.length = 0;
    ComparePageObject.ArrTempBarHTML.push("<tr class=\"car_compare_name\">");
    ComparePageObject.ArrTempBarHTML.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_0\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            if (i != 0 && (i % 6) == 0) {
                ComparePageObject.ArrTempBarHTML.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_" + i + "\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
            }
        }
        else {
            // insert Sec TH 
            if (i == ComparePageObject.MaxTDLeft) {
                ComparePageObject.ArrTempBarHTML.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_" + i + "\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
            }
        }
        if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
            if (i == 0)
            { ComparePageObject.ArrTempBarHTML.push("<td width=\"134\" class=\"f\">"); }
            else
            { ComparePageObject.ArrTempBarHTML.push("<td width=\"134\" >"); }
            var cs_Name = "";
            var car_year = "";
            var cs_allSpell = "";
            try {
                if (ComparePageObject.IsIE) {
                    cs_Name = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_ShowName/@PValue").value;
                    car_year = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Car_YearType/@PValue").value;
                    cs_allSpell = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_AllSpell/@PValue").value.toLowerCase();
                }
                else {
                    cs_Name = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_ShowName/@PValue");
                    car_year = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Car_YearType/@PValue");
                    cs_allSpell = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_AllSpell/@PValue").toLowerCase();
                }
                if (car_year.length >= 4) {
                    car_year = " " + car_year.substring(2, car_year.length) + "款";
                }
                else {
                    car_year = "";
                }
            }
            catch (err)
            { }
            ComparePageObject.ArrTempBarHTML.push("<a href=\"/" + cs_allSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/\" target=\"_blank\">" + cs_Name + " " + ComparePageObject.ArrCarInfo[i].CarName + car_year + "</a></td>");
        }
    }

    //when less 
    if (ComparePageObject.NeedBlockTD > 0) {
        for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
            if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
                // insert Sec TH
                ComparePageObject.ArrTempBarHTML.push("<th><h2><a href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
            }
            ComparePageObject.ArrTempBarHTML.push("<td width=\"134\">&nbsp;");
            ComparePageObject.ArrTempBarHTML.push("</td>");
        }
    }
    ComparePageObject.ArrTempBarHTML.push("</tr>");
}

// create price for compare
function createPrice(arrFieldRow) {
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
    ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            if (i != 0 && (i % 6) == 0) {
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        else {
            // insert Sec TH 
            if (i == ComparePageObject.MaxTDLeft) {
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
        }
        if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
            if (i == 0)
            { ComparePageObject.ArrPageContent.push("<td width=\"134\" class=\"f\">"); }
            else
            { ComparePageObject.ArrPageContent.push("<td width=\"134\">"); }

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
    }

    //when less 
    if (ComparePageObject.NeedBlockTD > 0) {
        for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
            if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
            }
            ComparePageObject.ArrPageContent.push("<td width=\"134\">&nbsp;");
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
        // 更改排重文字
        //       var thesameOperateObj = document.getElementById('thesameOperate');
        //       if(thesameOperateObj)
        //       {
        //           if(isOperateTheSame)
        //           {
        //              if(CheckBrowser())
        //              {thesameOperateObj.innerText = '恢复排除内容';}
        //              else
        //              {thesameOperateObj.textContent = '恢复排除内容';}
        //           }
        //           else
        //           {
        //              if(CheckBrowser())
        //              {thesameOperateObj.innerText = '排除相同内容';}
        //              else
        //              {thesameOperateObj.textContent = '排除相同内容';}
        //           }
        //       }
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
    // ComparePageObject.IsDeployAll = !ComparePageObject.IsDeployAll;
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
function resetCompareCar(id) {
    if (ComparePageObject.ValidCount < 2) {
        // only 1 car 
        return;
    }
    var num = -1;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.ArrCarInfo[i].CarID == id) {
            num = i;
        }
    }
    // not first object
    if (num > 0) {
        swapArray(ComparePageObject.ArrCarInfo, 0, num);
        // update the same car 
        createPageForCompare(false);
    }
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
    // return results.singleNodeValue.value;.
    if (results.singleNodeValue)
    { return results.singleNodeValue.value; }
    else
    { return ""; }
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
    // alert(CookieForCompare.getCookie("ActiveNewCompare"));
    // showName(); 

    // showTheWindowForList();
}

function delCarToCompare(caiID) {
    if (ComparePageObject.ValidCount < 2) {
        alert('至少留1个对比车型');
        return;
    }
    var num = -1;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.ArrCarInfo[i].CarID == caiID) {
            num = i; //alert(' yes :' + id + ' ' + i); 
        }
    }
    if (num >= 0) {
        // changeComparedShow(Compare.carID[num],Compare.carName[num]);
        ComparePageObject.ArrCarInfo.splice(num, 1);
        ComparePageObject.ValidCount--;
        // for reset compare cookie 
        // resetCookieForCompareCar(caiID); 
        resetTableWidth();
        createPageForCompare(false);
    }
}

function resetTableWidth() {
    if (ComparePageObject.ValidCount > 0) {
        if (ComparePageObject.IsAutoNeedMoreTH) {
            //  
            if (ComparePageObject.ValidCount > 6) {
                // ComparePageObject.TableWidth = (Math.ceil(ComparePageObject.ValidCount/6)-1)*146 + (134*ComparePageObject.ValidCount) + 2*(ComparePageObject.ValidCount+Math.ceil(ComparePageObject.ValidCount/6));
                ComparePageObject.TableWidth = (Math.ceil(ComparePageObject.ValidCount / 6)) * 146 + (134 * ComparePageObject.ValidCount);
            }
            else
            { ComparePageObject.TableWidth = 146 + (134 * 6) + 2; }
        }
        else {
            if (ComparePageObject.ValidCount > 6) {
                ComparePageObject.TableWidth = (146 * 2) + (134 * ComparePageObject.ValidCount) + 2;
            }
            else {
                ComparePageObject.TableWidth = 146 + (134 * 6) + 2;
            }
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

//--------------------  Drag  --------------------
var ie = document.all;
var nn6 = document.getElementById && !document.all;

var isdrag = false;
var x, y;
var dobj;
var currentTD = "";
var targetNum = "";

function movemouse(e) {
    if (isdrag) {
        dobj.style.left = nn6 ? tx + e.clientX - x + "px" : tx + event.clientX - x;
        dobj.style.top = nn6 ? ty + e.clientY - y + "px" : ty + event.clientY - y;
        //window.status = "x:" + event.clientX + " y:" + event.clientY + "iWidth:" + iWidth + " iHeight:" + iHeight;
        return false;
    }
}

function selectmouse(e) {
    try {
        var fobj = nn6 ? e.target : event.srcElement;
        var topelement = nn6 ? "HTML" : "BODY";
        if (fobj.tagName);
        while (fobj.tagName != topelement && fobj.name != "dragImg")//fobj.className != "dragme")
        {
            if (fobj.tagName);
            fobj = nn6 ? fobj.parentNode : fobj.parentElement;
        }
        //  if (fobj.className=="dragme")
        if (fobj.name == "dragImg") {
            currentTD = fobj.id.replace('img_', '');

            isdrag = true;
            fobj.style.position = 'relative';
            fobj.style.zIndex = '90';
            dobj = fobj;
            tx = parseInt(dobj.style.left + 0);
            ty = parseInt(dobj.style.top + 0);
            x = nn6 ? e.clientX : event.clientX;
            y = nn6 ? e.clientY : event.clientY;
            // window.status = "x:" + x + " y:" + y;
            // alert('x:'+x+' y:'+y)
            document.onmousemove = movemouse;
            return false;
        }
    }
    catch (err)
    { return false; }
}
function resetImg(e) {
    try {
        var fobj = nn6 ? e.target : event.srcElement;
        isdrag = false;
        //    window.status = "x" + x + " y" + y;
        // alert(fobj.name);
        if (fobj.name == "dragImg") {
            fobj.style.position = '';
            fobj.style.left = 0;
            fobj.style.top = 0;
            setTimeout("currentTD = '';", 200);
        }
        else {
            if (currentTD != '') {
                var fobj2 = document.getElementById("img_" + currentTD);
                fobj2.style.position = '';
                fobj2.style.left = 0;
                fobj2.style.top = 0;
                currentTD = '';
            }
            // setTimeout("currentTD = '';",200);
        }
        return false;
    }
    catch (err) {
        return false;
    }
}
window.onload = function() {
    if (ComparePageObject.IsNeedDrag) {
        //        var imgAll = document.getElementsByName("dragImg");
        //        if(imgAll && imgAll.length>0)
        //        {
        //          for(var i=0;i<imgAll.length;i++)
        //          {
        //               imgAll[i].onmousedown = selectmouse;
        //               imgAll[i].onmouseup = resetImg;
        //          } 
        //        }
        document.onmousedown = selectmouse;
        document.onmouseup = resetImg;
    }
}

// page method --------------------------
var arrField = [
    { sType: "fieldPic", sFieldName: "", sFieldTitle: "图片", sTrPrefix: "1", unit: "", joinCode: "" },
    { sType: "bar", sFieldName: "", sFieldTitle: "基本信息", sTrPrefix: "1", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldName: "CarReferPrice", sFieldTitle: "厂家指导价", sTrPrefix: "1", unit: "", joinCode: "" },
    { sType: "fieldPrice", sFieldName: "AveragePrice", sFieldTitle: "经销商报价", sTrPrefix: "1", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldName: "Car_RepairPolicy", sFieldTitle: "保修政策", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "基本性能", sTrPrefix: "2", unit: "", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_MaxSpeed", sFieldTitle: "最高车速", sTrPrefix: "2", unit: "km/h", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_BrakeGap", sFieldTitle: "制动距离（100—0 km/h）", sTrPrefix: "2", unit: "m", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_AccelerateTime", sFieldTitle: "加速时间", sTrPrefix: "2", unit: "s", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_AccelerateDistance", sFieldTitle: "加速距离", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ZongHeYouHao", sFieldTitle: "综合工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiQuYouHao", sFieldTitle: "市区工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiJiaoYouHao", sFieldTitle: "市郊工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FuelCostPer100", sFieldTitle: "百公里等速油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FuelCostPerRate100", sFieldTitle: "百公里等速油耗速度", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxClimbDegree", sFieldTitle: "最大爬坡度", sTrPrefix: "2", unit: "%", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxClimbDegreeValue", sFieldTitle: "最大爬坡度(值)", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_MinWheelRadius", sFieldTitle: "最小转弯半径", sTrPrefix: "2", unit: "mm", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "Perf_Tonnage", sFieldTitle: "最小离地间隙", sTrPrefix: "4", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "Inset_TrunkCapacity", sFieldTitle: "行李箱容积", sTrPrefix: "5", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "Engine_ExhaustForFloat", sFieldTitle: "排量（升）", sTrPrefix: "6", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "Engine_CylinderNum", sFieldTitle: "气缸数", sTrPrefix: "6", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "UnderPan_RimMaterial", sFieldTitle: "轮辋材料", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontRimStandard", sFieldTitle: "前轮辋规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearRimStandard", sFieldTitle: "后轮辋规格", sTrPrefix: "7", unit: "", joinCode: "" },
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
   { sType: "fieldPara", sFieldName: "InStat_FaceMirror", sFieldTitle: "遮阳板化妆境", sTrPrefix: "11", unit: "", joinCode: "" },
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