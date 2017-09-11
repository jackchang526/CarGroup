// 车型参数配置
var ComparePageObject = {
    CurrentCarIDs: "",
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
    IsNeedFirstColor: false,
    MaxTD: 4,
    CarListForSelect: new Array(),
    CurrentCarID: 0,
    CarIDAndNames: "",
    arrCarIds: [],
    ArrTempLeftNavHTML: [],
    ArrLeftNavHTML: [],
    IsShowDiff: true, //差异显示
    IsVantage: true, //优势项 默认选中
    DiffCount: 0,
    DiffList: [],
    currentIndexId: 0,

    ArrHeaderHTML: [],//header 内容
    ArrRightContentHTML: [],//右侧 内容
    ArrLeftTitleHtml: [],//左侧 title内容
    ArrHeaderLeftHTML: []//header 左侧内容
}
var ArrChannelNo = {
    "params-carinfo": "27.158.1620", "params-carbody": "27.158.1621", "params-carengine": "27.158.1622", "params-transmission": "27.158.1623",
    "params-bottomstop": "27.158.1624", "params-safeconfig": "27.158.1625", "params-wheel": "27.158.1626", "params-drivingassistance": "27.158.1627",
    "params-doorswindow": "27.158.1628", "params-lights": "27.158.1629", "params-innerconfig": "27.158.1630", "params-chair": "27.158.1631",
    "params-pastime": "27.158.1632", "params-air": "27.158.1633","params-optional":""
};

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

function initPageForCompare() {
    ComparePageObject.ArrCarInfo.length = 0;
    ComparePageObject.ArrPageContent.length = 0;
    ComparePageObject.ValidCount = 0;
    if (document.getElementById(ComparePageObject.PageDivContentID)) {
        ComparePageObject.PageDivContentObj = document.getElementById(ComparePageObject.PageDivContentID);
    }
    else { return; }
    if (typeof (carCompareJson) != "undefined")
    { ComparePageObject.AllCarJson = carCompareJson; }
    if (ComparePageObject.AllCarJson.length > 0) {
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            var carid = ComparePageObject.AllCarJson[i][0][0];
            var carName = ComparePageObject.AllCarJson[i][0][1];
            var engineExhaustForFloat = ComparePageObject.AllCarJson[i][1][3];
            var transmissionType = "";
            var carJsonObj = getCarAllParamDataByCarID(carid);
            if (ComparePageObject.arrCarIds.indexOf(carid) == -1) {
                ComparePageObject.arrCarIds.push(carid);
            }
            if (carJsonObj) {
                var carinfo = new CarCompareInfo();
                carinfo.CarID = carid;
                carinfo.CarName = carName;
                carinfo.EngineExhaustForFloat = engineExhaustForFloat;
                carinfo.TransmissionType = transmissionType;
                if (ComparePageObject.CurrentCarIDs != "") {
                    if (ComparePageObject.CurrentCarIDs.indexOf("," + carinfo.CarID + ",") >= 0) {
                        carinfo.IsDel = false;
                    }
                    else {
                        carinfo.IsDel = true;
                    }
                }
                else {
                    // 如果没有默认2个关注度高的
                    if (i <= ComparePageObject.MaxTD) {
                        carinfo.IsDel = false;
                    }
                    else {
                        carinfo.IsDel = true;
                    }
                }
                carinfo.CarInfoArray = carJsonObj;
                carinfo.IsValid = true;
                ComparePageObject.ArrCarInfo.push(carinfo);
                ComparePageObject.ValidCount++;
                // 车型选择列表
                var carid = carinfo.CarInfoArray[0][0];
                var carName = carinfo.CarInfoArray[0][1];
                var carYear = carinfo.CarInfoArray[0][7];
                //if (carinfo.IsDel) {
                //	ComparePageObject.CarListForSelect.push("<li><label><input onclick=\"selectCarByCarID(this);\" id=\"inputCar_" + carid
                //		+ "\" type=\"checkbox\">" + (carYear == "" ? "" : carYear + "款 ") + carName
                //		+ "</label></li>");
                //}
                //else {
                //	ComparePageObject.CarListForSelect.push("<li><label><input onclick=\"selectCarByCarID(this);\" checked=\"true\" id=\"inputCar_"
                //		+ carid + "\" type=\"checkbox\">" + (carYear == "" ? "" : carYear + "款 ") + carName
                //		+ "</label></li>");
                //}
            }
        }
        if (ComparePageObject.ValidCount > 0) {
            //createPageForCompare(ComparePageObject.IsOperateTheSame);
            //// 下拉列表
            //if (document.getElementById("ULCarListForSelect")) {
            //	document.getElementById("ULCarListForSelect").innerHTML = ComparePageObject.CarListForSelect.join("");
            //}
        }
    }
    else {
        //if (ComparePageObject.PageDivContentObj) {
        //	ComparePageObject.PageDivContentObj.innerHTML = "<p class=\"m-none-data\">暂无车型数据</p>";
        //}
    }

    createPageForCompare(ComparePageObject.IsOperateTheSame);
}

// 单个选车
function selectCarByCarID(obj) {
    var carID = obj.id.replace('inputCar_', '');
    var isShow = obj.checked;
    var currentCount = 0;
    var index = -1;
    if (ComparePageObject.ValidCount > 0) {
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (checkCarIsShowForeach(i)) {
                currentCount++;
            }
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][0] == carID)
            { index = i; }
        }
        if (isShow) {
            if (currentCount >= 2) {
                alert("亲！一次只能查看2款车。");
                obj.checked = false;
                return;
            }
            try {
                // 收起车型列表
                changeElementOrShow('m-popup-item,m-popup-item-b,m-popup-item-s');
                // 滚动到顶部
                window.scrollTo(0, 0);
            }
            catch (err) { }
        }
        if (index >= 0) {
            ComparePageObject.ArrCarInfo[index].IsDel = !isShow;
            createPageForCompare(ComparePageObject.IsOperateTheSame);
        }
    }
}

function createPageForCompare(isDelSame) {
    ComparePageObject.IsDelSame = isDelSame;
    var loopCount = arrField.length;
    //ComparePageObject.ArrPageContent.push("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">");
    for (var i = 0; i < loopCount; i++) {
        switch (arrField[i].sType) {
            case "fieldPara":
                if (ComparePageObject.ValidCount > 0) createPara(arrField[i]);
                break;
            case "fieldMulti":
                if (ComparePageObject.ValidCount > 0) createMulti(arrField[i]);
                break;
            case "fieldMultiValue":
                if (ComparePageObject.ValidCount > 0) fieldMultiValue(arrField[i]);
                break;
            case "bar":
                if (ComparePageObject.ValidCount > 0) createBar(arrField[i]);
                ComparePageObject.DiffList.push(ComparePageObject.DiffCount);
                ComparePageObject.DiffCount = 0;
                break;
            case "fieldPrice":
                if (ComparePageObject.ValidCount > 0) createPrice(arrField[i]);
                break;
            case "fieldPic":
                createPic(); //ComparePageObject.ArrPageContent.push("<tbody>");
                break;
            case "optional":
                createOptional(arrField[i]);
                break;
        }
    }
    if (ComparePageObject.ValidCount <= 0)
        createEmptyTable();

    ComparePageObject.ArrPageContent.push("<div class=\"section-tx\">");
    ComparePageObject.ArrPageContent.push("    <div class=\"tit\">");
    ComparePageObject.ArrPageContent.push("        <div class=\"tit-box\">");
    ComparePageObject.ArrPageContent.push("            <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">");
    ComparePageObject.ArrPageContent.push("                <tbody>");
    ComparePageObject.ArrPageContent.push(ComparePageObject.ArrLeftTitleHtml.join(''));
    ComparePageObject.ArrPageContent.push("                </tbody>");
    ComparePageObject.ArrPageContent.push("            </table>");
    ComparePageObject.ArrPageContent.push("        </div>");
    ComparePageObject.ArrPageContent.push("    </div>");
    ComparePageObject.ArrPageContent.push("    <div class=\"cont\">");
    ComparePageObject.ArrPageContent.push("        <!--右侧内容区域 start-->");
    ComparePageObject.ArrPageContent.push("        <div class=\"cont-box\">");
    ComparePageObject.ArrPageContent.push("            <table cellspacing=\"0\" cellpadding=\"0\">");
    ComparePageObject.ArrPageContent.push("                <tbody>");
    ComparePageObject.ArrPageContent.push(ComparePageObject.ArrRightContentHTML.join(''));
    ComparePageObject.ArrPageContent.push("                </tbody>");
    ComparePageObject.ArrPageContent.push("            </table>");
    ComparePageObject.ArrPageContent.push("        </div>");
    ComparePageObject.ArrPageContent.push("        <!--右侧内容区域 end-->");
    ComparePageObject.ArrPageContent.push("    </div>");
    ComparePageObject.ArrPageContent.push("</div>");


    //ComparePageObject.ArrPageContent.push("</tbody>");
    //ComparePageObject.ArrPageContent.push("</table>");
    // end
    if (ComparePageObject.PageDivContentObj) {
        ComparePageObject.PageDivContentObj.innerHTML = ComparePageObject.ArrPageContent.join("");
    }
    UpdateBarCount();
    //填充左侧滚动菜单
    if (ComparePageObject.ValidCount > 0) {
        if (ComparePageObject.ArrLeftNavHTML.length > 0) {
            $("#popup-menulist ul").html(ComparePageObject.ArrLeftNavHTML.join('')).show();
            $("#popup-menu").show();
            Bglog_InitPostLog();
            ComparePageObject.ArrLeftNavHTML.length = 0;
        } else {
            $("#popup-menulist").hide().find("ul").html("");
        }
    } else {
        if (ComparePageObject.ArrLeftNavHTML.length <= 0) {
            $("#popup-menu").hide();
            $("#popup-menulist").hide().find("ul").html("");
        }
    }
    ComparePageObject.DiffList.length = 0;
    ComparePageObject.ArrPageContent.length = 0;
    ComparePageObject.ArrLeftTitleHtml.length = 0;
    ComparePageObject.ArrRightContentHTML.length = 0;

    bindEvent();

    gotoSubMenu();
    callbackFunc();
}

function bindEvent() {
    $(".m-btn-duibi-close").on("click", function (e) {

        var index = $(this).data("index");
        delCarToCompare(index);

    });
    $(".car-item-add").on("click", function () {
        selectCar(serialId, 0);
    });
    var $menu_list = $("#popup-menulist");
    $("#popup-menu").off("click").on("click", function (event) {
        var target = event.srcElement || event.target;
        if (target.id != "popup-menu") return;
        if ($menu_list.is(":hidden")) {
            $menu_list.show();
            $("#popup-menumask").show();
        } else {
            $menu_list.hide();
            $("#popup-menumask").hide();
        }
    });
    //选配包事件
    bindOptionalEvent();
}
//选配包弹层
function bindOptionalEvent() {
    $('.pop-optional-mask').off("click").click(function () {
        $('body').css('overflow', 'auto');
        $('.pop-optional-mask').hide();
        $('.pop-optional').hide().removeClass('pop-optional-top pop-optional-bottom').find('ul').hide();
    });

    $('.optional.type1').off("click").on("click", function (e) {        e.stopPropagation();        var data = $(this).attr("data-optional");        if (data == "undefined" || data == "") return;        var fieldValue = data.split(',');
        var dataJson = [];
        for (var fieldIndex = 0; fieldIndex < fieldValue.length; fieldIndex++) {
            var fieldOptional = fieldValue[fieldIndex].split('|');
            if (fieldOptional.length > 1) {
                dataJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\",\"price\":" + fieldOptional[1] + "}"));
            }
            else {
                dataJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\"}"));
            }
        }        //var dataJson = JSON.parse(data);        var h = new Array();        h.push("<ul>");
        for (var i = 0; i < dataJson.length; i++) {
            h.push("<li><span class=\"l\">" + dataJson[i].text + "</span>");
            if (typeof (dataJson[i].price) != "undefined") {
                h.push("<span class=\"r\">￥" + formatCurrency(dataJson[i].price) + "</span>");
            }
            h.push("</li > ");
        }
        h.push("</ul>");        var halfScreenHeight = $(window).height() / 2;        var $popOptional = $('.pop-optional');        var thisTop = $(this).offset().top;
        var thisLeft = $(this).offset().left;        $popOptional.html(h.join(""));        $('body').css('overflow', 'hidden');
        var popOptionalHeight = $popOptional.height();
        if (e.clientY < halfScreenHeight) {
            $popOptional.css({
                'top': (thisTop + 30),
                'left': (thisLeft - 100)
            });
            $popOptional.addClass('pop-optional-top');
        } else {
            $popOptional.css({
                'top': (thisTop - popOptionalHeight - 15),
                'left': (thisLeft - 100)
            });
            $popOptional.addClass('pop-optional-bottom');
        }
        $('.pop-optional-mask').show();
        $popOptional.show();    });}

function createEmptyTable() {
    var flag = false;
    for (var i = 0; i < arrField.length; i++) {
        if (arrField[i].sType == "bar" && arrField[i]["sFieldTitle"] == "车体") break;
        if (arrField[i].sType == "bar" && arrField[i]["sFieldTitle"] == "基本信息") {
            //var arrFieldRow = arrField[i];
            flag = true; continue;
            //ComparePageObject.ArrPageContent.push("<tr id=\"" + arrFieldRow.scrollId + "\"><td class=\"pd0 td-tt\" colspan=\"" + (ComparePageObject.MaxTDLeft + 1) + "\"><h2><span>基本信息</span></h2></td></tr>");
        }
        if (flag) {
            //ComparePageObject.ArrLeftTitleHtml.push("<tr>");
            //ComparePageObject.ArrRightContentHTML.push("<tr>");
            if (arrField[i].sType == "fieldPara" && flag) {
                var arrFieldRow = arrField[i];
                //ComparePageObject.ArrLeftTitleHtml.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
                ////for (var j = 0; j < ComparePageObject.MaxTDLeft; j++) {
                //ComparePageObject.ArrRightContentHTML.push("<td name=\"td0\">&nbsp;</td>");
                ////}
                var leftTitle = arrFieldRow["sFieldTitle"],
				classStr = "";

                if (leftTitle.length > 10 && leftTitle.length < 20) {
                    classStr = "class=\"h2\"";
                } else if (leftTitle.length >= 20) {
                    classStr = "class=\"h3\"";
                }

                ComparePageObject.ArrLeftTitleHtml.push("<tr " + classStr + "><th>" + arrFieldRow["sFieldTitle"] + "</th></tr>");
                ComparePageObject.ArrRightContentHTML.push("<tr " + classStr + "><td name=\"td0\">&nbsp;</td></tr>");
            }
            //ComparePageObject.ArrLeftTitleHtml.push("</tr>");
            //ComparePageObject.ArrRightContentHTML.push("</tr>");
        }
    }
}

// create pic for compare
function createPic() {
    var tempArray = new Array();
    var isShowLoop = 0;
    var delCount = 0;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (!checkCarIsShowForeach(i)) {
            delCount++;
        }
    }
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {

                tempArray.push("<td>");
                tempArray.push("    <div class=\"car-item car-item-gray\">");

                try {
                    // car info
                    var carid = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][0];
                    var carName = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][1];
                    var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6];
                    var carYear = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7];
                    var csShowName = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][5];
                    var carRefPrice = ComparePageObject.ArrCarInfo[i].CarInfoArray[1][0];
                    tempArray.push("<a data-channelid=\"27.158.1615\" class=\"duibi-box\" href=\"###\" data-action=\"models\" data-car=\"" + carid + "\" data-id=\"" + serialId + "\" data-index=\"" + i + "\">");
                    tempArray.push("<h4>" + (carYear == "" ? "" : " " + carYear + "款 ") + carName + "</h4>");
                    tempArray.push("<em>" + carRefPrice + "</em>");
                    tempArray.push("</a>");

                    if (ComparePageObject.ValidCount - delCount > 1) {
                        // tempArray.push("<div class=\"m-btn-duibi-close\" onclick =\"javascript:delCarToCompare('" + i + "');\"></div>");
                    } else {
                        ComparePageObject.IsDelSame = false;
                        ComparePageObject.IsOperateTheSame = false;
                    }
                }
                catch (err)
                { }
                tempArray.push("        <div class=\"m-btn-duibi-close\"  data-channelid=\"27.158.1614\" data_cyslogclickflag=\"27.158.1614\" onclick=\"BglogPostLog('27.158.1614',this);\" data-index=\"" + i + "\"></div>");
                tempArray.push("        <i class=\"star\"></i>");
                tempArray.push("    </div>");
                tempArray.push("</td>");
            }
            isShowLoop++;
        }
    }

    //if (tempArray.length > 0) {
    ComparePageObject.ArrPageContent.push("<div class=\"flex\">");
    ComparePageObject.ArrPageContent.push("     <div class=\"section-flex\">");
    ComparePageObject.ArrPageContent.push("         <div class=\"flex-left\">");
    ComparePageObject.ArrPageContent.push("             <table width=\"120px\" cellspacing=\"0\" cellpadding=\"0\">");
    ComparePageObject.ArrPageContent.push("                 <thead>");
    ComparePageObject.ArrPageContent.push("                     <tr>");
    ComparePageObject.ArrPageContent.push("                         <td>");
                                                        //统计那边只支持a标签加data-channelid 可以直接统计到，如果换作非a标签加统计则需要手动加上data_cyslogclickflag="统计编号" onclick="BglogPostLog('统计编号',this);"  
    ComparePageObject.ArrPageContent.push("                             <div class=\"checkbox-box\" data-channelid=\"27.158.1617\"  data_cyslogclickflag=\"27.158.1617\" onclick=\"BglogPostLog('27.158.1617',this);\">");
    ComparePageObject.ArrPageContent.push("                                 <label>");
    ComparePageObject.ArrPageContent.push("                                     <div class=\"checkbox-normal " + (ComparePageObject.IsVantage ? "checked" : "") + "\">");
    ComparePageObject.ArrPageContent.push("                                         <input type=\"checkbox\"  name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + ">");
    ComparePageObject.ArrPageContent.push("                                     </div>");
    ComparePageObject.ArrPageContent.push("                                     <span>标识优势</span>");
    ComparePageObject.ArrPageContent.push("                                 </label>");
    ComparePageObject.ArrPageContent.push("                             </div>");
    ComparePageObject.ArrPageContent.push("                             <div class=\"checkbox-box\" data-channelid=\"27.158.1618\" data_cyslogclickflag=\"27.158.1618\" onclick=\"BglogPostLog('27.158.1618',this);\">");
    ComparePageObject.ArrPageContent.push("                                 <label>");
    ComparePageObject.ArrPageContent.push("                                     <div class=\"checkbox-normal " + (ComparePageObject.IsShowDiff ? "checked" : "") + "\">");
    ComparePageObject.ArrPageContent.push("                                         <input type=\"checkbox\" id=\"checkboxForDiff\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + ">");
    ComparePageObject.ArrPageContent.push("                                     </div>");
    ComparePageObject.ArrPageContent.push("                                     <span>高亮差异</span>");
    ComparePageObject.ArrPageContent.push("                                 </label>");
    ComparePageObject.ArrPageContent.push("                             </div>");
    ComparePageObject.ArrPageContent.push("                             <div class=\"checkbox-box\" data-channelid=\"27.158.1619\" data_cyslogclickflag=\"27.158.1619\" onclick=\"BglogPostLog('27.158.1619',this);\">");
    ComparePageObject.ArrPageContent.push("                                 <label>");
    ComparePageObject.ArrPageContent.push("                                     <div class=\"checkbox-normal " + (ComparePageObject.IsDelSame ? "checked" : "") + "\">");
    ComparePageObject.ArrPageContent.push("                                         <input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" " + (ComparePageObject.IsDelSame ? "checked" : "") + ">");
    ComparePageObject.ArrPageContent.push("                                     </div>");
    ComparePageObject.ArrPageContent.push("                                     <span>隐藏相同</span>");
    ComparePageObject.ArrPageContent.push("                                 </label>");
    ComparePageObject.ArrPageContent.push("                             </div>");
    ComparePageObject.ArrPageContent.push("                         </td>");
    ComparePageObject.ArrPageContent.push("                     </tr>");
    ComparePageObject.ArrPageContent.push("                 </thead>");
    ComparePageObject.ArrPageContent.push("             </table>");
    ComparePageObject.ArrPageContent.push("         </div>");
    ComparePageObject.ArrPageContent.push("         <div class=\"flex-right\">");
    ComparePageObject.ArrPageContent.push("             <!--右侧头区域 start-->");
    ComparePageObject.ArrPageContent.push("             <div class=\"flex-right-box\">");
    ComparePageObject.ArrPageContent.push("                 <table cellspacing=\"0\" cellpadding=\"0\">");
    ComparePageObject.ArrPageContent.push("                     <thead>");
    ComparePageObject.ArrPageContent.push("                         <tr>");

    ComparePageObject.ArrPageContent.push(tempArray.join(""));

    //when less
    if (isShowLoop < ComparePageObject.MaxTD) {
        ComparePageObject.ArrPageContent.push("<td>");
        ComparePageObject.ArrPageContent.push("    <div data-channelid=\"27.158.1616\" data_cyslogclickflag=\"27.158.1616\" onclick=\"BglogPostLog('27.158.1616',this);\" data-index=\"-1\" class=\"car-item-add\" data-action=\"models\" data-id=\"" + serialId + "\">");
        ComparePageObject.ArrPageContent.push("        <i></i>");
        ComparePageObject.ArrPageContent.push("        选择车款");
        ComparePageObject.ArrPageContent.push("    </div>");
        ComparePageObject.ArrPageContent.push("</td>");
    }

    ComparePageObject.ArrPageContent.push("                         </tr>");
    ComparePageObject.ArrPageContent.push("                     </thead>");
    ComparePageObject.ArrPageContent.push("                 </table>");
    ComparePageObject.ArrPageContent.push("             </div>");
    ComparePageObject.ArrPageContent.push("             <!--右侧头区域 end-->");
    ComparePageObject.ArrPageContent.push("         </div>");
    ComparePageObject.ArrPageContent.push("     </div>");
    ComparePageObject.ArrPageContent.push("     <div class=\"section-tt phone-title\" data-key=\"基本信息\" id=\"params-carinfo\">");
    ComparePageObject.ArrPageContent.push("         <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">");
    ComparePageObject.ArrPageContent.push("             <tbody>");
    ComparePageObject.ArrPageContent.push("                 <tr>");
    ComparePageObject.ArrPageContent.push("                     <td class=\"pd0 bgW\" colspan=\"3\">");
    ComparePageObject.ArrPageContent.push("                         <h3><span>基本信息</span></h3>");
    ComparePageObject.ArrPageContent.push("							<div class=\"r-txt\">");
    ComparePageObject.ArrPageContent.push("                             <ul class=\"config-icon-list\">");
    ComparePageObject.ArrPageContent.push("                                 <li class=\"bp\">标配</li>");
    ComparePageObject.ArrPageContent.push("                                 <li class=\"xp\">选配</li>");
    ComparePageObject.ArrPageContent.push("                                 <li class=\"w\">无</li>");
    ComparePageObject.ArrPageContent.push("                             </ul>");
    ComparePageObject.ArrPageContent.push("                         </div>");
    ComparePageObject.ArrPageContent.push("							<div class=\"clear\"></div>");
    ComparePageObject.ArrPageContent.push("                     </td>");
    ComparePageObject.ArrPageContent.push("                 </tr>");
    ComparePageObject.ArrPageContent.push("             </tbody>");
    ComparePageObject.ArrPageContent.push("         </table>");
    ComparePageObject.ArrPageContent.push("     </div>");
    ComparePageObject.ArrPageContent.push(" </div>");
    ComparePageObject.ArrPageContent.push(" <div class=\"flex-append\"></div>");

    //}

}

// create param for compare
function createPara(arrFieldRow) {
    var firstSame = true;
    var isAllunknown = true;
    var arrSame = new Array();
    var arrTemp = new Array();
    var isShowLoop = 0;
    var unit = arrFieldRow["unit"];
    var chkResult = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 };
    var vantage = (ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1");
    //    if (vantage || ComparePageObject.IsShowDiff)
    chkResult = IsFieldSame(arrFieldRow);
    var parameterId = arrFieldRow["sPid"];


    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                //                if (arrFieldRow["sFieldTitle"] == "车身颜色") {
                //                    arrTemp.push("<td name=\"td" + i + "\" class=\"m-car-color\">");
                //                }
                if (!chkResult.IsSame && ComparePageObject.IsShowDiff) {
                    arrTemp.push("<td name=\"td" + i + "\" class=\"cDiff\"><div class=\"txt c-box\">");
                }
                else { arrTemp.push("<td name=\"td" + i + "\" ><div class=\"txt c-box\">"); }

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
                        //						if (arrFieldRow["unit"] != "") {
                        //							field += "" + arrFieldRow["unit"];
                        //							field += "" + arrFieldRow["unit"];
                        //						}
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
                    // 车身颜色
                    if (arrFieldRow["sFieldTitle"] == "车身颜色") {
                        var tempColor = new Array();
                        var colorArray = field.split('|');
                        if (colorArray.length > 0) {
                            tempColor.push("<ul>");
                            for (var icolor = 0; icolor < colorArray.length; icolor++) {
                                var colorRGB = colorArray[icolor].split(',');
                                if (colorRGB.length == 2) {
                                    tempColor.push("<li><span title=\"" + colorRGB[0] + "\" style=\"background:" + colorRGB[1] + "\"></span></li>");
                                }
                            }
                            tempColor.push("</ul>");
                            field = tempColor.join("");
                        }
                    }
                    if (field.indexOf("有") == 0)
                    { field = "<span class=\"f-bold\">●</span>"; }
                    if (field.indexOf("选配") == 0) {
                        var fieldInfo = field.split('|');
                        if (fieldInfo.length > 1) {
                            field = "<span class=\"songti\">○ 选配" + formatCurrency(fieldInfo[1]) + "元</span>";
                        }
                        else {
                            field = "<span class=\"songti\">○</span>";
                        }
                    }
                    if (field.indexOf("无") == 0)
                    { field = "<span class=\"f-bold\">-</span>"; }

                    if (!chkResult.IsSame && ComparePageObject.IsVantage) {
                        //只有一个值不标示优势项 -1代表一个值
                        var tempFlag = false;
                        if (!isNaN(chkResult.CurrParamsValue) && !isNaN(field)) {
                            tempFlag = (parseFloat(chkResult.CurrParamsValue) == parseFloat(field));
                        } else {
                            tempFlag = (chkResult.CurrParamsValue == field);
                        }
                        if (chkResult.CurrCarIndex != -1 && tempFlag && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1")
                            arrTemp.push("<span class=\"cRed\">" + field + "</span>");
                        else
                            arrTemp.push(field);
                    }
                    else {
                        arrTemp.push(field);
                    }
                    if (arrFieldRow["sFieldTitle"] == "厂家指导价" && field != "无" && field != "待查") {
                        arrTemp.push("<a class=\"m-ico-calculator\" title=\"购车费用计算\" href=\"http://car.m.yiche.com/gouchejisuanqi/?carid=" + ComparePageObject.AllCarJson[i][0][0] + "\"></a>");
                        //arrTemp.push("<a data-channelid=\"27.158.1635\" href=\"http://gouche.m.yiche.com/sb" + ComparePageObject.AllCarJson[i][0][3] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/\" class=\"low-price\">找低价</a>");
                    }
                }
                catch (err) {
                    arrTemp.push("-");
                    firstSame = firstSame && false;
                }
                arrTemp.push("</div></td>");
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
                ComparePageObject.ArrLeftTitleHtml.push(ComparePageObject.ArrTempBarHTML.join(""));
                ComparePageObject.ArrTempBarHTML.length = 0;

                ComparePageObject.ArrRightContentHTML.push("<tr class=\"h25\"><td colspan=\"4\">&nbsp;</td></tr>");

                //添加左侧菜单
                ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
                ComparePageObject.ArrTempLeftNavHTML.length = 0;
            }
            var leftTitle = arrFieldRow["sFieldTitle"] + (unit != "" && unit.indexOf(",") == -1 ? "(" + unit + ")" : ""),
				classStr = "";
            //,leftTitleStr = leftTitle.length > 10 ? leftTitle : "<span>" + leftTitle + "</span>"

            if (leftTitle.length > 10 && leftTitle.length < 20) {
                classStr = "class=\"h2\"";
            } else if (leftTitle.length >= 20) {
                classStr = "class=\"h3\"";
            }

            ComparePageObject.ArrLeftTitleHtml.push("<tr " + classStr + ">");
            ComparePageObject.ArrLeftTitleHtml.push("<th>" + leftTitle + "</th>");

            ComparePageObject.ArrRightContentHTML.push("<tr " + classStr + ">");
            ComparePageObject.ArrRightContentHTML.push(arrTemp.join(""));
            if (!chkResult.IsSame) {
                ComparePageObject.DiffCount++;
            }
        }
        else {
            return;
        }
    }
    //when less
    if (isShowLoop < ComparePageObject.MaxTD) {

        ComparePageObject.ArrRightContentHTML.push("<td ><div class=\"txt c-box\">&nbsp;</div></td>");

    }
    ComparePageObject.ArrLeftTitleHtml.push("</tr>");

    ComparePageObject.ArrRightContentHTML.push("</tr>");
}
//判断参数值是否相同
function IsFieldSame(arrFieldRow) {
    var tempArray = new Array();
    var paramArray = arrFieldRow["sFieldIndex"].split(',');
    var unitArray = arrFieldRow["unit"].split(',');
    var joinCodeArray = arrFieldRow["joinCode"].split(',');
    var pidArray = arrFieldRow["sPid"].split(',');
    var prefixArray = arrFieldRow["sTrPrefix"].split(',');
    //var num = 0, multiFieldArray = [], IsSame = true;
    var result = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 }, tempField = null, allCarFieldArr = [];
    try {
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (!checkCarIsShowForeach(i)) continue;
            var multiField = "";
            for (var pint = 0; pint < paramArray.length; pint++) {
                // loop every param
                var sTrPrefix = arrFieldRow["sTrPrefix"];
                var index = paramArray[pint];
                if (ComparePageObject.ArrCarInfo[i].CarInfoArray.length <= prefixArray[pint])
                { return; }
                if (ComparePageObject.ArrCarInfo[i].CarInfoArray[prefixArray[pint]].length <= index)
                { return; }
                var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[prefixArray[pint]][index] || "";
                multiField = multiField + (joinCodeArray[pint] || "") + field;
            }
            //add by zhangll 2015/12/15 优势
            //判断是否是数字 解决 8.2、8.20 现象
            if (!isNaN(multiField) && !isNaN(tempField)) {
                if (parseFloat(multiField) != parseFloat(tempField) && tempField != null) result.IsSame = false;
            } else { if (multiField != tempField && tempField != null) result.IsSame = false; }
            if (arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1") {
                //空值排除比较
                var tempMultiField = "";
                if (multiField != "") {
                    //对取值范围值特殊处理
                    if (multiField.indexOf('-') != -1) {
                        if (arrFieldRow["size"] && arrFieldRow["size"] == "1")
                            tempMultiField = multiField.substring(multiField.indexOf('-') + 1);
                        else
                            tempMultiField = multiField.substring(0, multiField.indexOf('-'));
                    } else
                        tempMultiField = multiField;
                    //console.log(arrFieldRow["sFieldTitle"] + tempMultiField);
                    if (allCarFieldArr.indexOf(tempMultiField) == -1)
                        allCarFieldArr.push(tempMultiField);
                }
                if (arrFieldRow["size"] && arrFieldRow["size"] == "1") {
                    if (tempMultiField >= allCarFieldArr.max() && tempMultiField != "") {
                        result.CurrParamsValue = multiField;
                        result.CurrCarIndex = i;
                    }
                } else {
                    if (tempMultiField <= allCarFieldArr.min() && tempMultiField != "") {
                        //console.log(arrFieldRow["sFieldTitle"] + multiField + "come in");
                        result.CurrParamsValue = multiField;
                        result.CurrCarIndex = i;
                    }
                }
            }
            //上次临时值
            tempField = multiField;
        }
        if (allCarFieldArr.length <= 1) { result.CurrCarIndex = -1; result.CurrParamsValue = 0; }
    } catch (e) { }
    return result;
}
// create multi param for compare
function createMulti(arrFieldRow) {
    var tempArray = new Array();
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

    var chkResult = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 };
    //if ((ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1") || ComparePageObject.IsShowDiff)
    chkResult = IsFieldSame(arrFieldRow);

    // loop every car
    var tempArrMultField = [], num = 0, sameMultFieldArray = [];
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            num++;
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
                { arrTemp.push("<td name=\"td" + i + "\" class=\"f\"><div class=\"txt c-box\">"); }
                else if (!chkResult.IsSame && ComparePageObject.IsShowDiff)
                    arrTemp.push("<td name=\"td" + i + "\" class=\"cDiff\"><div class=\"txt c-box\">");
                else
                { arrTemp.push("<td name=\"td" + i + "\"><div class=\"txt c-box\">"); }
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
                        //// modified by chengl May.31.2012
                        //if (pidArray[pint] == "577") {
                        //    // 燃油标号 90改为：89或90；93改为：92或93；97改为：95或97；
                        //    switch (field) {
                        //        case "90号": field = field + "(北京89号)"; break;
                        //        case "93号": field = field + "(北京92号)"; break;
                        //        case "97号": field = field + "(北京95号)"; break;
                        //        default: break;
                        //    }
                        //}
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
                            // field += unitArray[pint];
                            multiField = multiField + joinCodeArray[pint] + field;
                        	//add by sk 2016.01.08 以下参数有值 直接显示 忽略第二个参数
                            if (pidArray[pint] == "509" || pidArray[pint] == "489" | pidArray[pint] == "555" || pidArray[pint] == "808") {
                            	break;
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
                        if (multiField.indexOf("有") >= 0)
                        { multiField = "<span class=\"songti f-bold\">●</span>"; }
                        if (multiField.indexOf("选配") >= 0 && multiField.indexOf("●") < 0)
                        {
                            var fieldInfo = multiField.split('|');
                            if (fieldInfo.length > 1) {
                                multiField = "<span class=\"songti\">○ 选配" + formatCurrency(fieldInfo[1]) + "元</span>";
                            }
                            else {
                                multiField = "<span class=\"songti\">○</span>";
                            }
                        }
                        if (multiField.indexOf("无") >= 0 && multiField.indexOf("●") < 0 && multiField.length == 1)
                        { multiField = "<span class=\"songti f-bold\">-</span>"; }

                        if (pint == 0) {
                            // 如果第1项是有，则不显示有，显示第2项
                            if (multiField == "<span class=\"songti f-bold\">●</span>")
                            { multiField = ""; }
                            else if (multiField == "<span class=\"songti f-bold\">○</span>" || multiField == "<span class=\"songti f-bold\">-</span>")
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
                                if (!chkResult.IsSame)
                                    arrTemp.push("<td name=\"td" + m + "\" class=\"cDiff\">");
                                else
                                    arrTemp.push("<td name=\"td" + m + "\" >");
                                arrTemp.push(tempArrMultField[m]);
                                arrTemp.push("</td>");
                            }
                        }

                    } else {
                        //arrTemp.push(multiField);
                        //arrTemp.push(multiField);
                        if (!chkResult.IsSame && ComparePageObject.IsVantage) {
                            //只有一个值不标示优势项 -1代表一个值
                            var tempFlag = false;
                            if (!isNaN(chkResult.CurrParamsValue) && !isNaN(multiField)) {
                                tempFlag = (parseFloat(chkResult.CurrParamsValue) == parseFloat(multiField));
                            } else {
                                tempFlag = (chkResult.CurrParamsValue == multiField);
                            }
                            if (chkResult.CurrCarIndex != -1 && tempFlag && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1")
                                arrTemp.push("<span class=\"cRed\">" + multiField + "</span>");
                            else
                                arrTemp.push(multiField);
                        } else
                            arrTemp.push(multiField);

                    }
                }
                catch (err) {
                    arrTemp.push("-");
                    firstSame = firstSame && false;
                }
                if (!ComparePageObject.IsDelSame) arrTemp.push("</div></td>");
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
                ComparePageObject.ArrLeftTitleHtml.push(ComparePageObject.ArrTempBarHTML.join(""));
                ComparePageObject.ArrTempBarHTML.length = 0;

                ComparePageObject.ArrRightContentHTML.push("<tr class=\"h25\"><td colspan=\"4\">&nbsp;</td></tr>");
            }
            //添加左侧菜单
            ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
            ComparePageObject.ArrTempLeftNavHTML.length = 0;

            var leftTitle = arrFieldRow["sFieldTitle"],
			classStr = "";
            //,leftTitleStr = leftTitle.length > 10 ? leftTitle : "<span>" + leftTitle + "</span>"

            if (leftTitle.length > 10 && leftTitle.length < 20) {
                classStr = "class=\"h2\"";
            } else if (leftTitle.length >= 20) {
                classStr = "class=\"h3\"";
            }

            ComparePageObject.ArrLeftTitleHtml.push("<tr " + classStr + ">");
            ComparePageObject.ArrLeftTitleHtml.push("<th>" + leftTitle + "</th>");

            ComparePageObject.ArrRightContentHTML.push("<tr " + classStr + " id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
            ComparePageObject.ArrRightContentHTML.push(arrTemp.join(""));
            if (!chkResult.IsSame) {
                ComparePageObject.DiffCount++;
            }
        }
        else {
            return;
        }
    }
    if (tempArray.length > 0) {
        //ComparePageObject.ArrPageContent.push(tempArray.join(""));
        //when less 对比项小于2个时，填补对比项
        if (num < ComparePageObject.MaxTD) {

            ComparePageObject.ArrRightContentHTML.push("<td><div class=\"txt c-box\">&nbsp;</div></td>");

        }
        ComparePageObject.ArrRightContentHTML.push("</tr>");
        ComparePageObject.ArrLeftTitleHtml.push("</tr>");
    }
}

// create multi value param for compare
function fieldMultiValue(arrFieldRow) {
    var arrTemp = new Array(),
        num = 0,
        isAllunknown = true;

    var chkResult = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 };
    if ((ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1") || ComparePageObject.IsShowDiff)
        chkResult = IsFieldSame(arrFieldRow);

    if (!chkResult || (chkResult.IsSame && ComparePageObject.IsDelSame)) {//非法返回，或者隐藏相同项
        return;
    }

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            num++;
            if (!ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                arrTemp.push("<td>&nbsp;</td>");
            }
            else {
                if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor) {//车款配置页
                    arrTemp.push("<td name=\"td" + i + "\" class=\"f\">");
                }
                else if (!chkResult.IsSame) {
                    arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
                }
                else {
                    arrTemp.push("<td name=\"td" + i + "\">");
                }
                var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[arrFieldRow["sTrPrefix"]][arrFieldRow["sFieldIndex"]];
                if (field != "") isAllunknown = false;
                var fieldValue = field.split(',');
                var standardJson = [], optionalJson = [];
                for (var fieldIndex = 0; fieldIndex < fieldValue.length; fieldIndex++) {
                    var fieldOptional = fieldValue[fieldIndex].split('|');
                    if (fieldOptional.length > 1) {
                        optionalJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\",\"price\":" + fieldOptional[1] + "}"));
                    }
                    else {
                        standardJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\"}"));
                    }
                }

                if (standardJson.length == 1) {//共一项值
                    arrTemp.push("<div>" + standardJson[0].text + "</div>");
                }
                else if (standardJson.length > 1) {//多项
                    if (optionalJson.length == 0) {
                        arrTemp.push("<div>");
                        for (var staIndex = 0; staIndex < standardJson.length; staIndex++) {
                            arrTemp.push(standardJson[staIndex].text + "&nbsp;&nbsp;");
                        }
                        arrTemp.push("</div>");
                    }
                    else {
                        var jsonArr = [];
                        standardJson.forEach(function (value, index, array) {
                            jsonArr.push(array[index].text);
                        });
                        arrTemp.push("<div class=\"optional type1\" data-optional=\"" + jsonArr.join(",") + "\">● " + standardJson[0].text + "等</div>");
                        //arrTemp.push("<div class=\"popup-layout-1\"><ul>");
                        //for (var staIndex = 0; staIndex < standardJson.length; staIndex++) {
                        //    arrTemp.push("<li><span class=\"l\">" + standardJson[staIndex].text + "</span ></li>");
                        //}
                        //arrTemp.push("</ul></div></div>");
                    }
                }
                if (optionalJson.length == 1) {
                    arrTemp.push("<div>○ 选配" + optionalJson[0].text + "&nbsp;" + formatCurrency(optionalJson[0].price) + "元</div>");
                }
                else if (optionalJson.length > 1) {
                    var jsonArr = [];
                    optionalJson.forEach(function (value, index, array) {
                        jsonArr.push(array[index].text + "|" + array[index].price);
                    });
                    arrTemp.push("<div class=\"optional type1\" data-optional=\"" + jsonArr.join(",") + "\">○ 选配" + formatCurrency(optionalJson[0].price) + "元起</div>");
                    //arrTemp.push("<div class=\"popup-layout-1\"><ul>");
                    //for (var optIndex = 0; optIndex < optionalJson.length; optIndex++) {
                    //    arrTemp.push("<li><span class=\"l\">" + optionalJson[optIndex].text + "</span ><span class=\"r\">￥" + optionalJson[optIndex].price + "</span></li>");
                    //}
                    //arrTemp.push("</ul></div></div>");
                }
                arrTemp.push("</td>");
            }
        }
    }
    arrTemp.push("<td></td>");
    if (!isAllunknown) {
        // Is Need Show The Bar
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrLeftTitleHtml.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;

            ComparePageObject.ArrRightContentHTML.push("<tr class=\"h25\"><td colspan=\"4\">&nbsp;</td></tr>");
        }
        //添加左侧菜单
        ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
        ComparePageObject.ArrTempLeftNavHTML.length = 0;

        var leftTitle = arrFieldRow["sFieldTitle"],
            classStr = "";
        //,leftTitleStr = leftTitle.length > 10 ? leftTitle : "<span>" + leftTitle + "</span>"

        if (leftTitle.length > 10 && leftTitle.length < 20) {
            classStr = "class=\"h2\"";
        } else if (leftTitle.length >= 20) {
            classStr = "class=\"h3\"";
        }

        ComparePageObject.ArrLeftTitleHtml.push("<tr " + classStr + ">");
        ComparePageObject.ArrLeftTitleHtml.push("<th>" + leftTitle + "</th>");

        ComparePageObject.ArrRightContentHTML.push("<tr " + classStr + " id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
        ComparePageObject.ArrRightContentHTML.push(arrTemp.join(""));
        if (!chkResult.IsSame) {
            ComparePageObject.DiffCount++;
        }
    }
    else {
        return;
    }
}

function createOptional(arrFieldRow) {
    if (ComparePageObject.ValidCount < 1 || typeof optionalPackageJson == "undefined" || optionalPackageJson.length == 0) {
        return;
    }
    var arrTitleTemp = new Array(),
        arrContentTemp = new Array();
        //isShow = false;
    for (var opt = 0; opt < optionalPackageJson.length; opt++) {
        var showCarCount = 0;
        //isShow = false;
        arrContentTemp.push("<tr>");
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (checkCarIsShowForeach(i)) {
                showCarCount++;
                if (optionalPackageJson[opt].carid.contains(ComparePageObject.ArrCarInfo[i].CarID)) {
                    arrContentTemp.push("<td name=\"td" + i + "\" class=\"optional\"><span>○ 选配￥" + optionalPackageJson[opt].price + "</span></td>");
                    //isShow = true;
                }
                else {
                    arrContentTemp.push("<td name=\"td" + i + "\"><span class=\"songti\">-</span></td>");
                }
            }
        }
        arrContentTemp.push("<td></td></tr>");
        arrContentTemp.push("<tr class=\"multi-row2-end\"><td colspan=\"" + (ComparePageObject.ValidCount + 1) + "\"><span class=\"optional-note\">" + optionalPackageJson[opt].desc + "</span></td></tr>");
        arrContentTemp.push("</tr>");

        arrTitleTemp.push("<tr class=\"optional-h\"><th>" + optionalPackageJson[opt].name + "</th></tr>");
    }
    //if (isShow) {
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrLeftTitleHtml.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;

            ComparePageObject.ArrRightContentHTML.push("<tr class=\"h25\"><td colspan=\"4\">&nbsp;</td></tr>");
        }
        //添加左侧菜单
        ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
        ComparePageObject.ArrTempLeftNavHTML.length = 0;

        ComparePageObject.ArrLeftTitleHtml.push(arrTitleTemp.join(""));

        ComparePageObject.ArrRightContentHTML.push(arrContentTemp.join(""));
   // }
}


function createBar(arrFieldRow) {
    if (ComparePageObject.ValidCount < 1)
    { return; }
    ComparePageObject.ArrTempBarHTML.length = 0;
    ComparePageObject.ArrTempLeftNavHTML.length = 0;
    //左侧菜单数据
    var fileldTitle = arrFieldRow["sFieldTitle"], arrOneTitle = [];
    for (var i = 0; i < fileldTitle.length; i++) {
        arrOneTitle.push(fileldTitle[i]);
    }
    if (arrOneTitle.length == 2) {
        fileldTitle = arrOneTitle.join("<em class=\"one\"></em><em class=\"one\"></em>")
    }
    if (arrOneTitle.length == 3) {
        fileldTitle = arrOneTitle.join("<em class=\"half\"></em>")
    }

    ComparePageObject.ArrTempLeftNavHTML.push("<li><a data-channelid=\""+ArrChannelNo[arrFieldRow["scrollId"]]+"\" data-target=\"" + arrFieldRow["scrollId"] + "\" href=\"javascript:;\" ><span>" + fileldTitle + "</span></a></li>");

    if (arrFieldRow["sFieldTitle"] == "基本信息") return;


    var summaryColumn = 1;
    ComparePageObject.ArrTempBarHTML.push("<tr  class=\"phone-title\" data-key=\"" + arrFieldRow["sFieldTitle"] + "\" id=\"" + arrFieldRow["scrollId"] + "\">");
    ComparePageObject.ArrTempBarHTML.push("<td class=\"pd0 bgW\" colspan=\"3\">");
    //if (arrFieldRow["sFieldTitle"] == "外部配置") {
    //    ComparePageObject.ArrTempBarHTML.push("<h3><span>" + arrFieldRow["sFieldTitle"] + "</span></h3><div class=\"r-txt r-diff\" data-width=\"160\" >●标配&nbsp;&nbsp;○选配&nbsp;&nbsp;-无 <em id = \"bar_" + (ComparePageObject.DiffList.length + 1) + "\"></em>");
    //}
    //else {
        ComparePageObject.ArrTempBarHTML.push("<h3><span>" + arrFieldRow["sFieldTitle"] + "</span></h3><div class=\"r-txt r-diff\" id = \"bar_" + (ComparePageObject.DiffList.length + 1) + "\"></div>");
    //}
    ComparePageObject.ArrTempBarHTML.push("</td>");
    ComparePageObject.ArrTempBarHTML.push("</tr>");

}

// create price for compare
function createPrice(arrFieldRow) {
    var tempArray = new Array();
    var isShowLoop = 0;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                tempArray.push("<td name=\"td" + i + "\">");
                try {
                    var sTrPrefix = arrFieldRow["sTrPrefix"];
                    var index = arrFieldRow["sFieldIndex"];
                    var field = ComparePageObject.ArrCarInfo[i].CarInfoArray[sTrPrefix][index] || "";
                }
                catch (err) {
                    tempArray.push("-");
                }
                if (field.length < 1 || field == "无") {
                    tempArray.push("无");
                }
                else {
                    if (arrFieldRow["sFieldTitle"] == "商家报价") {
                        var minPrice = field;
                        if (field.indexOf("-") != -1) {
                            minPrice = field.substring(0, field.indexOf("-"));
                        }
                        tempArray.push("<span class=\"cRed\">" + minPrice + "&nbsp;</span>");
                        tempArray.push("<a class=\"m-btn-xunjia\" href=\"http://price.m.yiche.com/zuidijia/nc" + ComparePageObject.ArrCarInfo[i].CarInfoArray[0][0] + "/?leads_source=m009001\">询价</a>");
                    }
                    else if (arrFieldRow["sFieldTitle"] == "降价优惠") {
                        var csAllSpell = ComparePageObject.AllCarJson[i][0][6] || "";
                        tempArray.push("<span class=\"m-ico-downprice\"><a href=\"http://jiangjia.m.yiche.com/nb" + ComparePageObject.ArrCarInfo[i].CarInfoArray[0][3] + "_c0/\">" + field + "</a></span>");
                    }
                    else if (arrFieldRow["sFieldTitle"] == "行情价") {
                        var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6] || "";
                        var citySpell = "quanguo";
                        tempArray.push(field);
                    }
                    else { }
                }
                tempArray.push("</td>");
            }
            isShowLoop++;
        }
    }
    if (tempArray.length > 0) {

        //add by sk 2014.12.26 隐藏相同项 前面项相同 添加bar
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrLeftTitleHtml.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;

            //添加左侧菜单
            ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
            ComparePageObject.ArrTempLeftNavHTML.length = 0;
        }

        ComparePageObject.ArrLeftTitleHtml.push("<tr>");
        ComparePageObject.ArrLeftTitleHtml.push("<th><span>" + arrFieldRow["sFieldTitle"] + "</span></th>");

        ComparePageObject.ArrRightContentHTML.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\">");
        ComparePageObject.ArrRightContentHTML.push(tempArray.join(""));
        //when less
        if (isShowLoop < ComparePageObject.MaxTD) {

            ComparePageObject.ArrRightContentHTML.push("<td>&nbsp;</td>");

        }
        ComparePageObject.ArrRightContentHTML.push("</tr>");
        ComparePageObject.ArrLeftTitleHtml.push("</tr>");
    }
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

function delCarToCompare(index) {
    if (ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
        ComparePageObject.ArrCarInfo[index].IsDel = true;
        var carid = ComparePageObject.ArrCarInfo[index].CarInfoArray[0][0];
        ComparePageObject.arrCarIds.splice(index, 1);
        ComparePageObject.AllCarJson.splice(index, 1);
        if (document.getElementById("inputCar_" + carid)) {
            document.getElementById("inputCar_" + carid).checked = false;
        }
        //createPageForCompare(ComparePageObject.IsOperateTheSame);
        initPageForCompare();
    }
}

function checkCarIsShowForeach(index) {
    return true;// !ComparePageObject.ArrCarInfo[index].IsDel;
}
//优势项
function advantageForCompare() {
    if (ComparePageObject.ValidCount <= 1) return;
    //ComparePageObject.IsVantage = (!ComparePageObject.IsVantage) ? true : false;
    if (ComparePageObject.ValidCount > 1) {
        if (ComparePageObject.IsVantage) {
            ComparePageObject.IsVantage = false;
            $("input[type='checkbox'][name='chkAdvantage']").each(function (i, n) { this.checked = false; });
        } else {
            ComparePageObject.IsVantage = true;
            $("input[type='checkbox'][name='chkAdvantage']").each(function (i, n) { this.checked = true; });
        }
        createPageForCompare(ComparePageObject.IsDelSame);
    }

}
//显示差异
function showDiffForCompare() {
    if (ComparePageObject.ValidCount > 1) {
        var checkboxForDiff = document.getElementById("checkboxForDiff");
        if (checkboxForDiff) {
            if (!checkboxForDiff.checked) {
                checkboxForDiff.checked = false;
                ComparePageObject.IsShowDiff = false;
            }
            else {
                checkboxForDiff.checked = true;
                ComparePageObject.IsShowDiff = true;
            }
            createPageForCompare(ComparePageObject.IsDelSame);
        }
    }
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
function UpdateBarCount() {
    if (ComparePageObject.ValidCount > 0 && ComparePageObject.DiffList.length > 0) {
        for (var i = 1; i < ComparePageObject.DiffList.length + 1; i++) {
            if (ComparePageObject.DiffList[i] > 0) {
                var bar = document.getElementById("bar_" + i);
                if (bar) {
                    bar.innerText = "共" + ComparePageObject.DiffList[i] + "项不同";
                }
            }
        }
    }
}
//选择车款添加对比
function selectCarId(carId, currentIndex) {
    $(".leftmask3").trigger('close');
    //var container = document.getElementById("container");
    //var carinfo_container = document.getElementById("carinfo_container");
    //添加车款顺序问题
    if (currentIndex >= 0) {
        ComparePageObject.arrCarIds.splice(currentIndex, 1, carId);
    }
    else {
        //ComparePageObject.arrCarIds.unshift(carId);
        ComparePageObject.arrCarIds.push(carId);
    }
    $.getScript("http://api.car.bitauto.com/CarInfo/GetCarParameter.ashx?isParamPage=1&carids=" + ComparePageObject.arrCarIds.join(","), function (data) {
        //if (carinfo_container)
        //	carinfo_container.style.display = "none";
        //if (container)
        //	container.style.display = "block";
        document.documentElement.scrollTop = 0;
        document.body.scrollTop = 0;
        initPageForCompare();
        //$("#popMenu").show();
    });
}
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.max = function () { return Math.max.apply({}, this) }
Array.prototype.min = function () { return Math.min.apply({}, this) }

var arrField = [
    { sFieldTitle: "图片", sType: "fieldPic", sPid: "", sFieldIndex: "", sTrPrefix: "1", unit: "", joinCode: "" },
    { sFieldTitle: "基本信息", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carinfo" },
    { sFieldTitle: "厂商指导价", sType: "fieldPara", sPid: "", sTrPrefix: "1", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "商家报价", sType: "fieldPrice", sPid: "", sTrPrefix: "1", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", sType: "fieldPara", sPid: "", sTrPrefix: "1", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "排量", sType: "fieldPara", sPid: "785", sTrPrefix: "1", sFieldIndex: "4", unit: "L", joinCode: "" },
    { sFieldTitle: "进气形式", sType: "fieldPara", sPid: "425", sTrPrefix: "1", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", sType: "fieldPara", sPid: "1007", sTrPrefix: "1", sFieldIndex: "", unit: "6", joinCode: "" },
    { sFieldTitle: "燃油变速箱", sType: "fieldMulti", sPid: "724,712", sTrPrefix: "1,1", sFieldIndex: "7,8", unit: "挡,", joinCode: ", " },
    { sFieldTitle: "最高车速[km/h]", sType: "fieldPara", sPid: "663", sTrPrefix: "1", sFieldIndex: "9", unit: "", joinCode: "", isVantage: "1", size: "1" },

    { sFieldTitle: "车身尺寸", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carbody" },
    { sFieldTitle: "长×宽×高[mm]", sType: "fieldMulti", sPid: "588,593,586", sTrPrefix: "2,2,2", sFieldIndex: "0,1,2", unit: ",,", joinCode: ",x,x", isVantage: "1", size: "1" },
    { sFieldTitle: "轴距[mm]", sType: "fieldPara", sPid: "592", sTrPrefix: "2", sFieldIndex: "3", unit: "", joinCode: "mm", isVantage: "1", size: "1" },
    { sFieldTitle: "整备质量[kg]", sType: "fieldPara", sPid: "669", sTrPrefix: "2", sFieldIndex: "4", unit: "", joinCode: "kg" },
    { sFieldTitle: "座位数[个]", sType: "fieldPara", sPid: "665", sTrPrefix: "2", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "行李箱容积[L]", sType: "fieldPara", sPid: "465", sTrPrefix: "2", sFieldIndex: "6", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "油箱容积[L]", sType: "fieldPara", sPid: "576", sTrPrefix: "2", sFieldIndex: "7", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "前轮胎规格", sType: "fieldPara", sPid: "729", sTrPrefix: "2", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后轮胎规格", sType: "fieldPara", sPid: "721", sTrPrefix: "2", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "备胎", sType: "fieldPara", sPid: "707", sTrPrefix: "2", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", sType: "fieldPara", sPid: "398", sTrPrefix: "2", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "满载质量[kg]", sType: "fieldPara", sPid: "668", sTrPrefix: "2", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎规格",sType: "fieldPara", sPid: "1001", sTrPrefix: "2", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "载重质量[kg]", sType: "fieldPara", sPid: "974", sTrPrefix: "2", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎个数", sType: "fieldPara", sPid: "982", sTrPrefix: "2", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "货箱长×宽×高[mm]", sType: "fieldMulti", sPid: "966,969,970", sTrPrefix: "2", sFieldIndex: "16,17,18", unit: "mm", joinCode: "x", isVantage: "1", size: "1" },

    { sFieldTitle: "动力系统", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carengine" },
    { sFieldTitle: "排气量[ml]", sType: "fieldMulti", sPid: "423,785", sTrPrefix: "3,3", sFieldIndex: "0,1", unit: ",L", joinCode: ",ml " }, /*1987ml 2.0L*/
    { sFieldTitle: "最大功率[kW(Ps)/rpm]", sType: "fieldPara", sPid: "430", sTrPrefix: "3", sFieldIndex: "2", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "马力(Ps)", sType: "fieldPara", sPid: "791", sTrPrefix: "3", sFieldIndex: "3", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大功率转速(rpm)", sType: "fieldPara", sPid: "433", sTrPrefix: "3", sFieldIndex: "4", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大扭矩[N.m/rpm]", sType: "fieldPara", sPid: "429", sTrPrefix: "3", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "最大扭矩转速(rpm)", sType: "fieldPara", sPid: "432", sTrPrefix: "3", sFieldIndex: "6", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "缸体形式", sType: "fieldPara", sPid: "418", sTrPrefix: "3", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "气缸数[缸]", sType: "fieldPara", sPid: "417", sTrPrefix: "3", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "进气形式", sType: "fieldPara", sPid: "425", sTrPrefix: "3", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "供油方式", sType: "fieldPara", sPid: "580", sTrPrefix: "3", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "压缩比", sType: "fieldPara", sPid: "414", sTrPrefix: "3", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "燃油标号[号]", sType: "fieldPara", sPid: "577", sTrPrefix: "3", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "发动机启停", sType: "fieldPara", sPid: "894", sTrPrefix: "3", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "燃油变速箱类型", sType: "fieldPara", sPid: "712", sTrPrefix: "3", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "档位个数", sType: "fieldPara", sPid: "724", sTrPrefix: "3", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "最高车速[km/h]", sType: "fieldPara", sPid: "663", sTrPrefix: "3", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "0-100km/h加速时间[s]", sType: "fieldPara", sPid: "650", sTrPrefix: "3", sFieldIndex: "17", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "混合工况油耗(L/100km)", sType: "fieldPara", sPid: "782", sTrPrefix: "3", sFieldIndex: "18", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "环保标准", sType: "fieldPara", sPid: "421", sTrPrefix: "3", sFieldIndex: "19", unit: "", joinCode: "" },
    { sFieldTitle: "电动机总功率[kW]", sType: "fieldPara", sPid: "870", sTrPrefix: "3", sFieldIndex: "20", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电动机总扭矩[N.m]", sType: "fieldPara", sPid: "872", sTrPrefix: "3", sFieldIndex: "21", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "前电动机最大功率[kW]", sType: "fieldPara", sPid: "1002", sTrPrefix: "3", sFieldIndex: "22", unit: "", joinCode: "" },
    { sFieldTitle: "前电动机最大扭矩[N.m]", sType: "fieldPara", sPid: "1004", sTrPrefix: "3", sFieldIndex: "23", unit: "", joinCode: "" },
    { sFieldTitle: "后电动机最大功率[kW]", sType: "fieldPara", sPid: "1003", sTrPrefix: "3", sFieldIndex: "24", unit: "", joinCode: "" },
    { sFieldTitle: "后电动机最大扭矩[N.m]", sType: "fieldPara", sPid: "1005", sTrPrefix: "3", sFieldIndex: "25", unit: "", joinCode: "" },
    { sFieldTitle: "电池容量[kWh]", sType: "fieldPara", sPid: "876", sTrPrefix: "3", sFieldIndex: "26", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电池充电时间[h]", sType: "fieldMulti", sPid: "878,879", sTrPrefix: "3", sFieldIndex: "27,28", unit: "", joinCode: "" },
    { sFieldTitle: "耗电量[kWh/100km]", sType: "fieldPara", sPid: "868", sTrPrefix: "3", sFieldIndex: "29", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "最大续航里程[km]", sType: "fieldPara", sPid: "883", sTrPrefix: "3", sFieldIndex: "30", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电池组质保", sType: "fieldPara", sPid: "1006", sTrPrefix: "3", sFieldIndex: "31", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", sType: "fieldPara", sPid: "1007", sTrPrefix: "3", sFieldIndex: "32", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合功率[Kw]", sType: "fieldPara", sPid: "1008", sTrPrefix: "3", sFieldIndex: "33", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合扭矩[N.m]", sType: "fieldPara", sPid: "1009", sTrPrefix: "3", sFieldIndex: "34", unit: "", joinCode: "" },
    { sFieldTitle: "发动机描述", sType: "fieldPara", sPid: "945", sTrPrefix: "3", sFieldIndex: "35", unit: "", joinCode: "" },
    { sFieldTitle: "卡车变速箱描述", sType: "fieldPara", sPid: "1011", sTrPrefix: "3", sFieldIndex: "36", unit: "", joinCode: "" },
    { sFieldTitle: "卡车前进档位个数", sType: "fieldPara", sPid: "980", sTrPrefix: "3", sFieldIndex: "37", unit: "", joinCode: "" },
    { sFieldTitle: "卡车倒档位个数", sType: "fieldPara", sPid: "981", sTrPrefix: "3", sFieldIndex: "38", unit: "", joinCode: "" },

    { sFieldTitle: "底盘制动", sType: "bar", sPid: "", sTrPrefix: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-bottomstop" },
    { sFieldTitle: "驱动方式", sType: "fieldPara", sPid: "655", sTrPrefix: "4", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "前悬架类型", sType: "fieldPara", sPid: "728", sTrPrefix: "4", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "后悬架类型", sType: "fieldPara", sPid: "720", sTrPrefix: "4", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "可调悬挂", sType: "fieldPara", sPid: "708", sTrPrefix: "4", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "前轮制动器类型", sType: "fieldPara", sPid: "726", sTrPrefix: "4", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "后轮制动器类型", sType: "fieldPara", sPid: "718", sTrPrefix: "4", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "驻车制动类型", sType: "fieldPara", sPid: "716", sTrPrefix: "4", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "车体结构", sType: "fieldPara", sPid: "572", sTrPrefix: "4", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "差速器/差速锁", sType: "fieldPara", sPid: "733", sTrPrefix: "4", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "客车前悬架类型", sType: "fieldPara", sPid: "1012", sTrPrefix: "4", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "客车后悬架类型", sType: "fieldPara", sPid: "1013", sTrPrefix: "4", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "卡车驱动形式", sType: "fieldPara", sPid: "1014", sTrPrefix: "4", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "前桥描述", sType: "fieldPara", sPid: "975", sTrPrefix: "4", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "前桥允许载荷[kg]", sType: "fieldPara", sPid: "1015", sTrPrefix: "4", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后桥描述", sType: "fieldPara", sPid: "976", sTrPrefix: "4", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后桥允许载荷[kg]", sType: "fieldPara", sPid: "1016", sTrPrefix: "4", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "安全配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-safeconfig" },
    { sFieldTitle: "防抱死制动(ABS)", sType: "fieldPara", sPid: "673", sTrPrefix: "5", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "制动力分配(EBD/CBC等)", sType: "fieldPara", sPid: "685", sTrPrefix: "5", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "制动辅助(BA/EBA/BAS等)", sType: "fieldPara", sPid: "684", sTrPrefix: "5", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "牵引力控制(ARS/TCS/TRC等)", sType: "fieldPara", sPid: "698", sTrPrefix: "5", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "车身稳定控制(ESP/DSC/VSC等)", sType: "fieldPara", sTrPrefix: "5", sPid: "700", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "主驾驶安全气囊", sType: "fieldPara", sPid: "682", sTrPrefix: "5", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "副驾驶安全气囊", sType: "fieldPara", sPid: "697", sTrPrefix: "5", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "前侧气囊", sType: "fieldPara", sPid: "691", sTrPrefix: "5", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "后侧气囊", sType: "fieldPara", sPid: "680", sTrPrefix: "5", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "侧安全气帘", sType: "fieldPara", sPid: "690", sTrPrefix: "5", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "膝部气囊", sType: "fieldPara", sPid: "835", sTrPrefix: "5", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "安全带气囊", sType: "fieldPara", sPid: "845", sTrPrefix: "5", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "后排中央气囊", sType: "fieldPara", sPid: "1017", sTrPrefix: "5", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "胎压监测", sType: "fieldPara", sPid: "714", sTrPrefix: "5", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "零胎压续航轮胎",  sType: "fieldPara", sPid: "715", sTrPrefix: "5", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后排儿童座椅接口(ISO FIX/LATCH)", sType: "fieldPara", sPid: "495", sTrPrefix: "5", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "驾驶辅助", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-drivingassistance" },
    { sFieldTitle: "定速巡航", sType: "fieldPara", sPid: "545", sTrPrefix: "6", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "车道保持/并线辅助", sType: "fieldPara", sTrPrefix: "6", sPid: "898", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "碰撞报警/主动刹车", sType: "fieldPara", sTrPrefix: "6", sPid: "818", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "疲劳提醒", sType: "fieldPara", sPid: "1018", sTrPrefix: "6", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "自动泊车", sType: "fieldPara", sPid: "816", sTrPrefix: "6", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "遥控泊车", sType: "fieldPara", sPid: "901", sTrPrefix: "6", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "自动驾驶辅助", sType: "fieldPara", sPid: "1019", sTrPrefix: "6", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "自动驻车", sType: "fieldPara", sPid: "811", sTrPrefix: "6", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "上坡辅助", sType: "fieldPara", sPid: "812", sTrPrefix: "6", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "陡坡缓降", sType: "fieldPara", sPid: "813", sTrPrefix: "6", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "夜视系统", sType: "fieldPara", sPid: "819", sTrPrefix: "6", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "可变齿比转向", sType: "fieldPara", sPid: "1020", sTrPrefix: "6", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "前倒车雷达", sType: "fieldPara", sPid: "800", sTrPrefix: "6", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "后倒车雷达", sType: "fieldPara", sPid: "702", sTrPrefix: "6", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "倒车影像", sType: "fieldMultiValue", sPid: "703", sTrPrefix: "6", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "驾驶模式选择", sType: "fieldPara", sPid: "1021", sTrPrefix: "6", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "外部配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-outerconfig" },
    { sFieldTitle: "前大灯", sType: "fieldPara", sPid: "614", sTrPrefix: "7", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "LED日间行车灯", sType: "fieldPara", sPid: "794", sTrPrefix: "7", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "自动大灯", sType: "fieldMultiValue", sPid: "609", sTrPrefix: "7", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "前雾灯", sType: "fieldPara", sPid: "607", sTrPrefix: "7", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "大灯功能", sType: "fieldMultiValue", sPid: "612", sTrPrefix: "7", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "天窗类型", sType: "fieldPara", sPid: "567", sTrPrefix: "7", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "前电动车窗",sType: "fieldPara", sPid: "601", sTrPrefix: "7", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "后电动车窗",sType: "fieldPara", sPid: "1038", sTrPrefix: "7", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜电动调节", sType: "fieldMultiValue", sPid: "622", sTrPrefix: "7", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜加热", sType: "fieldPara", sPid: "624", sTrPrefix: "7", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "内后视镜自动防炫目", sType: "fieldPara", sPid: "621", sTrPrefix: "7", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜自动防炫目", sType: "fieldPara", sPid: "1022", sTrPrefix: "7", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "隔热/隐私玻璃", sType: "fieldPara", sPid: "796", sTrPrefix: "7", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "后排侧遮阳帘", sType: "fieldPara", sPid: "797", sTrPrefix: "7", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后遮阳帘", sType: "fieldPara", sPid: "595", sTrPrefix: "7", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "前雨刷器", sType: "fieldPara", sPid: "606", sTrPrefix: "7", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "后雨刷器", sType: "fieldMultiValue", sPid: "596", sTrPrefix: "7", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "电吸门", sType: "fieldPara", sPid: "821", sTrPrefix: "7", sFieldIndex: "17", unit: "", joinCode: "" },
    { sFieldTitle: "电动侧滑门", sType: "fieldPara", sPid: "1023", sTrPrefix: "7", sFieldIndex: "18", unit: "", joinCode: "" },
    { sFieldTitle: "电动行李箱", sType: "fieldMultiValue", sPid: "556", sTrPrefix: "7", sFieldIndex: "19", unit: "", joinCode: "" },
    { sFieldTitle: "车顶行李架", sType: "fieldPara", sPid: "627", sTrPrefix: "7", sFieldIndex: "20", unit: "", joinCode: "" },
    { sFieldTitle: "中控锁", sType: "fieldMultiValue", sPid: "493", sTrPrefix: "7", sFieldIndex: "21", unit: "", joinCode: "" },
    { sFieldTitle: "智能钥匙", sType: "fieldMultiValue", sPid: "952", sTrPrefix: "7", sFieldIndex: "22", unit: "", joinCode: "" },
    { sFieldTitle: "远程遥控功能", sType: "fieldMultiValue", sPid: "1024", sTrPrefix: "7", sFieldIndex: "23", unit: "", joinCode: "" },
    { sFieldTitle: "尾翼/扰流板", sType: "fieldPara", sPid: "1025", sTrPrefix: "7", sFieldIndex: "24", unit: "", joinCode: "" },
    { sFieldTitle: "运动外观套件", sType: "fieldPara", sPid: "793", sTrPrefix: "7", sFieldIndex: "25", unit: "", joinCode: "" },

    { sFieldTitle: "内部配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-innerconfig" },
    { sFieldTitle: "内饰材质", sType: "fieldMultiValue", sPid: "1026", sTrPrefix: "8", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "车内氛围灯", sType: "fieldPara", sPid: "795", sTrPrefix: "8", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "遮阳板化妆镜", sType: "fieldPara", sPid: "512", sTrPrefix: "8", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘材质", sType: "fieldMultiValue", sPid: "548", sTrPrefix: "8", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "多功能方向盘", sType: "fieldPara", sPid: "528", sTrPrefix: "8", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘调节", sType: "fieldMultiValue", sPid: "799", sTrPrefix: "8", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘加热", sType: "fieldPara", sPid: "956", sTrPrefix: "8", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘换挡", sType: "fieldPara", sPid: "574", sTrPrefix: "8", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "前排空调", sType: "fieldPara", sPid: "839", sTrPrefix: "8", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后排空调", sType: "fieldPara", sPid: "838", sTrPrefix: "8", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "香氛系统", sType: "fieldPara", sPid: "1027", sTrPrefix: "8", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "空气净化", sType: "fieldPara", sPid: "905", sTrPrefix: "8", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "车载冰箱", sType: "fieldPara", sPid: "485", sTrPrefix: "8", sFieldIndex: "12", unit: "", joinCode: "" },

    { sFieldTitle: "座椅配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-chair" },
    { sFieldTitle: "座椅材质", sType: "fieldPara", sPid: "544", sTrPrefix: "9", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "运动风格座椅", sType: "fieldPara", sPid: "546", sTrPrefix: "9", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "主副座椅电动调节", sType: "fieldMultiValue", sPid: "508", sTrPrefix: "9", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "副座椅电动调节",sType: "fieldMultiValue", sPid: "503", sTrPrefix: "9", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "主座椅调节方式", sType: "fieldMultiValue", sPid: "1028", sTrPrefix: "9", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "副座椅调节方式", sType: "fieldMultiValue", sPid: "1029", sTrPrefix: "9", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "第二排座椅电动调节", sType: "fieldMultiValue", sPid: "833", sTrPrefix: "9", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "第二排座椅调节方式", sType: "fieldMultiValue", sPid: "1030", sTrPrefix: "9", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "前排座椅功能", sType: "fieldMultiValue", sPid: "504", sTrPrefix: "9", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后排座椅功能", sType: "fieldMultiValue", sPid: "1031", sTrPrefix: "9", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "前排中央扶手", sType: "fieldPara", sPid: "514", sTrPrefix: "9", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "后排中央扶手", sType: "fieldPara", sPid: "475", sTrPrefix: "9", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "第三排座椅", sType: "fieldPara", sPid: "805", sTrPrefix: "9", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "座椅放倒方式", sType: "fieldPara", sPid: "482", sTrPrefix: "9", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后排杯架", sType: "fieldPara", sPid: "474", sTrPrefix: "9", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后排折叠桌版", sType: "fieldPara", sPid: "1032", sTrPrefix: "9", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "信息娱乐", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-pastime" },
    { sFieldTitle: "中控彩色液晶屏", sType: "fieldMultiValue", sPid: "488", sTrPrefix: "10", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "全液晶仪表盘", sType: "fieldPara", sPid: "988", sTrPrefix: "10", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "行车电脑显示屏", sType: "fieldPara", sPid: "832", sTrPrefix: "10", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "HUD平视显示", sType: "fieldPara", sPid: "518", sTrPrefix: "10", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "GSP导航", sType: "fieldPara", sPid: "516", sTrPrefix: "10", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "智能互联定位", sType: "fieldPara", sPid: "1033", sTrPrefix: "10", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "语音控制", sType: "fieldPara", sPid: "1035", sTrPrefix: "10", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "手机互联(Carplay&Android)", sType: "fieldMultiValue", sPid: "1036", sTrPrefix: "10", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "手机无线充电", sType: "fieldPara", sPid: "1037", sTrPrefix: "10", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "手势控制系统", sType: "fieldPara", sPid: "1034", sTrPrefix: "10", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "CD/DVD", sType: "fieldPara", sPid: "510", sTrPrefix: "10", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "蓝牙/WIFI连接", sType: "fieldMultiValue", sPid: "479", sTrPrefix: "10", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "外接接口", sType: "fieldMultiValue", sPid: "810", sTrPrefix: "10", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "车载电视", sType: "fieldPara", sPid: "559", sTrPrefix: "10", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "音响品牌", sType: "fieldPara", sPid: "473", sTrPrefix: "10", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "扬声器数量(个)", sType: "fieldPara", sPid: "523", sTrPrefix: "10", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "后排液晶屏/娱乐系统", sType: "fieldPara", sPid: "477", sTrPrefix: "10", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "车载220V电源", sType: "fieldPara", sPid: "467", sTrPrefix: "10", sFieldIndex: "17", unit: "", joinCode: "" },

    { sFieldTitle: "选配包", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-optional" },
    { sFieldTitle: "选配包", sType: "optional", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "" },
];
// page method --------------------------
var arrField2 = [
   { sType: "fieldPic", sFieldIndex: "", sFieldTitle: "图片", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息", sPid: "", sTrPrefix: "1", unit: "", joinCode: "", scrollId: "params-carinfo" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "厂家指导价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldIndex: "1", sFieldTitle: "商家报价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   //{ sType: "fieldPrice", sFieldIndex: "2", sFieldTitle: "降价优惠", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
//{ sType: "fieldAvgPrice", sFieldIndex: "0", sFieldTitle: "参考成交价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "保修政策", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "车船税减免", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "购置税减免", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   //{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "节能补贴", sPid: "853", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "新能源汽车国家补贴", sPid: "997", sTrPrefix: "1", unit: "万元", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "排量", sPid: "785", sTrPrefix: "1", unit: "L", joinCode: "" },
   { sType: "fieldMulti", sFieldIndex: "6,7", sFieldTitle: "变速箱", sPid: "724,712", sTrPrefix: "1,1", unit: "挡,", joinCode: ", " },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "市区工况油耗", sPid: "783", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "市郊工况油耗", sPid: "784", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "综合工况油耗", sPid: "782", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "网友油耗", sPid: "", sTrPrefix: "0", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "易车实测油耗", sPid: "788", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "官方0-100公里加速时间", sPid: "650", sTrPrefix: "1", unit: "s", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "易车实测0-100公里加速时间", sPid: "786", sTrPrefix: "1", unit: "s", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "最高车速", sPid: "663", sTrPrefix: "1", unit: "km/h", joinCode: "", isVantage: "1", size: "1" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "乘员人数（区间）", sPid: "951", sTrPrefix: "1", unit: "个", joinCode: "" },
 { sType: "bar", sFieldIndex: "", sFieldTitle: "车体", sPid: "", sTrPrefix: "3", unit: "", joinCode: "", scrollId: "params-carbody" },
 //{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "车身颜色", sPid: "598", sTrPrefix: "0", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车长", sPid: "588", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车宽", sPid: "593", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "车高", sPid: "586", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "轴距", sPid: "592", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前轮距", sPid: "585", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后轮距", sPid: "582", sTrPrefix: "2", unit: "mm", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "整备质量", sPid: "669", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "满载质量", sPid: "668", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "最小离地间隙", sPid: "589", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "最大涉水深度", sPid: "662", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "接近角", sPid: "591", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "通过角", sPid: "890", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "离去角", sPid: "581", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "行李厢容积", sPid: "465", sTrPrefix: "2", unit: "L", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "行李厢最大拓展容积", sPid: "466", sTrPrefix: "2", unit: "L", joinCode: "", isVantage: "1", size: "1" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "行李厢盖开合方式", sPid: "466", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李厢打开方式", sPid: "441", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "37", sFieldTitle: "感应行李厢", sPid: "990", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "车门数", sPid: "563", sTrPrefix: "2", unit: "个", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "车顶型式", sPid: "573", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "车篷型式", sPid: "570", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "38", sFieldTitle: "车篷开合方式", sPid: "562", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "车顶行李箱架", sPid: "627", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "36", sFieldTitle: "运动外观套件", sPid: "793", sTrPrefix: "2", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "后导流尾翼", sPid: "616", sTrPrefix: "2", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "运动包围", sPid: "597", sTrPrefix: "2", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "风阻系数", sPid: "670", sTrPrefix: "2", unit: "", joinCode: "", isVantage: "1", size: "0" },
{ sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "货厢形式", sPid: "964", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "26,27", sFieldTitle: "货厢长度", sPid: "965,966", sTrPrefix: "2,2", unit: ",", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldMulti", sFieldIndex: "28,29", sFieldTitle: "货厢宽度", sPid: "967,970", sTrPrefix: "2,2", unit: ",", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldMulti", sFieldIndex: "30,31", sFieldTitle: "货厢高度", sPid: "968,969", sTrPrefix: "2,2", unit: ",", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "车厢形式", sPid: "971", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "座位排列", sPid: "972", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "额定载重量", sPid: "973", sTrPrefix: "2", unit: "T", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "最大总重量", sPid: "974", sTrPrefix: "2", unit: "T", joinCode: "" },
 { sType: "bar", sFieldIndex: "", sFieldTitle: "发动机", sPid: "", sTrPrefix: "3", unit: "", joinCode: "", scrollId: "params-carengine" },
 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "发动机位置", sPid: "428", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "发动机型号", sPid: "436", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "排量", sPid: "785", sTrPrefix: "3", unit: "L", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "排量", sPid: "423", sTrPrefix: "3", unit: "mL", joinCode: "" },
 { sType: "fieldMulti", sFieldIndex: "4,5", sFieldTitle: "进气形式", sPid: "425,408", sTrPrefix: "3,3", unit: " ,", joinCode: "," },
 { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "气缸排列型式", sPid: "418", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "气缸数", sPid: "417", sTrPrefix: "3", unit: "个", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "每缸气门数", sPid: "437", sTrPrefix: "3", unit: "个", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "气门结构", sPid: "410", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "压缩比", sPid: "414", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "缸径", sPid: "415", sTrPrefix: "3", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "行程", sPid: "434", sTrPrefix: "3", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "最大马力", sPid: "791", sTrPrefix: "3", unit: "Ps", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "最大功率", sPid: "430", sTrPrefix: "3", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "最大功率转速", sPid: "433", sTrPrefix: "3", unit: "rpm", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "最大扭矩", sPid: "429", sTrPrefix: "3", unit: "Nm", joinCode: "", isVantage: "1", size: "1" },
	{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "最大扭矩转速", sPid: "432", sTrPrefix: "3", unit: "rpm", joinCode: "", isVantage: "1", size: "1" },
 	{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "特有技术", sPid: "435", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "燃料类型", sPid: "578", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "30", sFieldTitle: "新能源类型", sPid: "998", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "燃油标号", sPid: "577", sTrPrefix: "3", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "供油方式", sPid: "580", sTrPrefix: "3", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "燃油箱容积", sPid: "576", sTrPrefix: "3", unit: "L", joinCode: "", isVantage: "1", size: "1" },
 { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "缸盖材料", sPid: "419", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "缸体材料", sPid: "416", sTrPrefix: "3", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "环保标准", sPid: "421", sTrPrefix: "3", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "启停系统", sPid: "894", sTrPrefix: "3", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "电机最大功率-转速", sPid: "871", sTrPrefix: "3", unit: "kW/rpm", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "电机最大扭矩-转速", sPid: "873", sTrPrefix: "3", unit: "N·m/rpm", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "油箱材质", sPid: "978", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "bar", sFieldIndex: "", sFieldTitle: "电池/电动机", sPid: "", sTrPrefix: "16", unit: "", joinCode: "", scrollId: "params-electric" },
	 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "电机最大功率", sPid: "870", sTrPrefix: "16", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "电机最大功率-转速", sPid: "871", sTrPrefix: "3", unit: "kW/rpm", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电机最大扭矩", sPid: "872", sTrPrefix: "16", unit: "Nm", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "电机最大扭矩-转速", sPid: "873", sTrPrefix: "3", unit: "Nm/rpm", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电机额定功率", sPid: "869", sTrPrefix: "16", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "系统电压", sPid: "874", sTrPrefix: "16", unit: "V", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "电机类型", sPid: "866", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "电机数量", sPid: "983", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "电机位置", sPid: "984", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "充电方式", sPid: "954", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "普通充电时间", sPid: "879", sTrPrefix: "16", unit: "分钟", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "快速充电时间", sPid: "878", sTrPrefix: "16", unit: "分钟", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "电池电压", sPid: "877", sTrPrefix: "16", unit: "V", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "电池容量", sPid: "876", sTrPrefix: "16", unit: "kWh", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "电池类型", sPid: "875", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "百公里耗电量", sPid: "868", sTrPrefix: "16", unit: "kw/100km", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "纯电最高续航里程", sPid: "883", sTrPrefix: "16", unit: "km", joinCode: "", isVantage: "1", size: "1" },

	 { sType: "bar", sFieldIndex: "", sFieldTitle: "变速箱", sPid: "", sTrPrefix: "4", unit: "", joinCode: "", scrollId: "params-transmission" },
	 { sType: "fieldMulti", sFieldIndex: "1,0", sFieldTitle: "变速箱", sPid: "724,712", sTrPrefix: "4,4", unit: "挡,", joinCode: ", " },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "换挡拨片", sPid: "547", sTrPrefix: "4", unit: "", joinCode: "" },
 	 //{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电子挡杆", sPid: "844", sTrPrefix: "4", unit: "", joinCode: "" },
	 	 { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "变速箱型号", sPid: "979", sTrPrefix: "4", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "前进挡数", sPid: "980", sTrPrefix: "4", unit: "个", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "倒挡数", sPid: "981", sTrPrefix: "4", unit: "个", joinCode: "" },
	 { sType: "bar", sFieldIndex: "", sFieldTitle: "底盘制动", sPid: "", sTrPrefix: "5", unit: "", joinCode: "", scrollId: "params-bottomstop" },
	 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车体结构", sPid: "844", sTrPrefix: "5", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "最小转弯半径", sPid: "590", sTrPrefix: "5", unit: "m", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "转向助力", sPid: "735", sTrPrefix: "5", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前制动类型", sPid: "726", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "后制动类型", sPid: "718", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "驻车制动类型", sPid: "716", sTrPrefix: "5", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "驱动方式", sPid: "655", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "空气悬挂", sPid: "814", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "可调悬挂", sPid: "708", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "前悬挂类型", sPid: "728", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后悬挂类型", sPid: "720", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "中央差速器锁", sPid: "733", sTrPrefix: "5", unit: "", joinCode: "" },
       { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "前桥（轴）描述", sPid: "975", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后桥描述", sPid: "976", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "弹簧片数", sPid: "977", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "安全配置", sPid: "", sTrPrefix: "6", unit: "", joinCode: "", scrollId: "params-safeconfig" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "驾驶位安全气囊", sPid: "682", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "副驾驶位安全气囊", sPid: "697", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前排侧安全气囊", sPid: "691", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前排头部气囊(气帘)", sPid: "690", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "膝部气囊", sPid: "835", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后排侧安全气囊", sPid: "680", sTrPrefix: "6", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "后排头部气囊(气帘)", sPid: "679", sTrPrefix: "6", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "安全带气囊", sPid: "845", sTrPrefix: "6", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "安全带未系提示", sPid: "836", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "安全带限力功能", sPid: "701", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "安全带预收紧功能", sPid: "678", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "前安全带调节", sPid: "677", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后排安全带", sPid: "675", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后排中间三点式安全带", sPid: "676", sTrPrefix: "6", unit: "", joinCode: "" },
	  //{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "电子限速", sPid: "656", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "胎压监测装置", sPid: "714", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "零压续行(零胎压继续行驶)", sPid: "715", sTrPrefix: "6", unit: "", joinCode: "" },
	   //{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "可溃缩转向柱", sPid: "713", sTrPrefix: "6", unit: "", joinCode: "" },
	   //{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "溃缩式制动踏板", sPid: "696", sTrPrefix: "6", unit: "", joinCode: "" },
	   //{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "车内中控锁", sPid: "837", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "中控门锁", sPid: "493", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "儿童锁", sPid: "494", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "遥控钥匙", sPid: "538", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "无钥匙进入系统", sPid: "952", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "无钥匙启动系统", sPid: "469", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldMulti", sFieldIndex: "24,25", sFieldTitle: "发动机电子防盗", sPid: "699,683", sTrPrefix: "6,6", unit: ",", joinCode: "," },
		{ sType: "bar", sFieldIndex: "", sFieldTitle: "车轮", sPid: "", sTrPrefix: "7", unit: "", joinCode: "", scrollId: "params-wheel" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "前轮胎规格", sPid: "729", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "后轮胎规格", sPid: "721", sTrPrefix: "7", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前轮毂规格", sPid: "727", sTrPrefix: "7", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "后轮毂规格", sPid: "719", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "备胎类型", sPid: "707", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "轮毂材料", sPid: "704", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "轮胎数量", sPid: "982", sTrPrefix: "7", unit: "个", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "行车辅助", sPid: "", sTrPrefix: "8", unit: "", joinCode: "", scrollId: "params-drivingassistance" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "刹车防抱死(ABS)", sPid: "673", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电子制动力分配系统(EBD)", sPid: "685", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "刹车辅助(EBA/BAS/BA/EVA等)", sPid: "684", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "牵引力控制(ASR/TCS/TRC/ATC等)", sPid: "698", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "动态稳定控制系统（ESP）", sPid: "700", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "随速助力转向调节(EPS)", sPid: "732", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "自动驻车", sPid: "811", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "上坡辅助", sPid: "812", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "陡坡缓降", sPid: "813", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "泊车雷达(车前)", sPid: "800", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "倒车雷达(车后)", sPid: "702", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "倒车影像", sPid: "703", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "全景摄像头", sPid: "820", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "定速巡航", sPid: "545", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "自适应巡航", sPid: "893", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "GPS导航系统", sPid: "516", sTrPrefix: "8", unit: "", joinCode: "" },
 //{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "人机交互系统", sPid: "806", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "自动泊车入位", sPid: "816", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "并线辅助", sPid: "817", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "车道偏离预警系统", sPid: "955", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "主动刹车/主动安全系统", sPid: "818", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "整体主动转向系统", sPid: "841", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "夜视系统", sPid: "819", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "盲点检测", sPid: "898", sTrPrefix: "8", unit: "", joinCode: "" },
  //{ sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "发动机阻力矩控制系统（EDC/MSR）", sPid: "897", sTrPrefix: "8", unit: "", joinCode: "" },
  //{ sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "弯道制动控制系统（CBC）", sPid: "896", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "bar", sFieldIndex: "", sFieldTitle: "门窗/后视镜", sPid: "", sTrPrefix: "9", unit: "", joinCode: "", scrollId: "params-doorswindow" },
  { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "开门方式", sPid: "891", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电动车窗", sPid: "601", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "防紫外线/隔热玻璃", sPid: "796", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "隐私玻璃", sPid: "989", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电动窗防夹功能", sPid: "594", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "天窗开合方式", sPid: "568", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "天窗型式", sPid: "567", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "后窗遮阳帘", sPid: "595", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "后排侧遮阳帘", sPid: "797", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "后雨刷器", sPid: "596", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "感应雨刷", sPid: "606", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "电动吸合门", sPid: "821", sTrPrefix: "9", unit: "", joinCode: "" },
   //{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "行李厢电动吸合门", sPid: "822", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后视镜带侧转向灯", sPid: "830", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "外后视镜记忆功能	", sPid: "625", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "外后视镜加热功能", sPid: "624", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "外后视镜电动折叠功能", sPid: "623", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "外后视镜电动调节", sPid: "622", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "内后视镜防眩目功能", sPid: "621", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "遮阳板化妆镜", sPid: "512", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "灯光", sPid: "", sTrPrefix: "10", unit: "", joinCode: "", scrollId: "params-lights" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "前大灯类型", sPid: "614", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "选配前大灯类型", sPid: "953", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "前大灯自动开闭", sPid: " 609", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前大灯自动清洗功能", sPid: "608", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前大灯延时关闭", sPid: " 611", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前大灯随动转向", sPid: " 613", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "前大灯照射范围调整", sPid: " 612", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "会车前灯防眩目功能", sPid: " 610", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "前雾灯", sPid: "607", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "阅读灯", sPid: "539", sTrPrefix: "10", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "车厢后阅读灯", sPid: "480", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "车内氛围灯", sPid: "795", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "日间行车灯", sPid: "794", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "LED尾灯", sPid: " 846", sTrPrefix: "10", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "高位（第三）制动灯", sPid: " 620", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "转向辅助灯", sPid: " 828", sTrPrefix: "10", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "侧转向灯", sPid: "626", sTrPrefix: "10", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李厢灯", sPid: "618", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "内部配置", sPid: "", sTrPrefix: "11", unit: "", joinCode: "", scrollId: "params-innerconfig" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "方向盘前后调节", sPid: "799", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "方向盘上下调节", sPid: "798", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "方向盘调节方式", sPid: "552", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "方向盘记忆设置", sPid: "549", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "方向盘表面材料", sPid: "548", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "多功能方向盘", sPid: "528", sTrPrefix: "11", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "多功能方向盘功能", sPid: "527", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "方向盘加热", sPid: "956", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "全液晶仪表盘", sPid: "988", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "行车电脑显示屏", sPid: "832", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "HUD抬头数字显示", sPid: "518", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "内饰颜色", sPid: "801", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排杯架", sPid: "474", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "车内电源电压", sPid: "467", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "座椅", sPid: "", sTrPrefix: "12", unit: "", joinCode: "", scrollId: "params-chair" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "运动座椅", sPid: "546", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "座椅材料", sPid: "544", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "选配座椅材料", sPid: "948", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "座椅高低调节", sPid: "992", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "驾驶座座椅调节方式", sPid: "508", sTrPrefix: "12", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "驾驶座座椅调节方向", sPid: "507", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "副驾驶座椅调节方式", sPid: "503", sTrPrefix: "12", unit: "", joinCode: "" },
//{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "副驾驶座椅调节方向", sPid: "502", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "驾驶座腰部支撑调节", sPid: "506", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "驾驶座肩部支撑调节", sPid: "802", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "前座椅头枕调节", sPid: "515", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "后排座椅调节方式", sPid: "833", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排座位放倒比例", sPid: "482", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "后排座椅角度调节", sPid: "991", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "前座中央扶手", sPid: "514", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后座中央扶手", sPid: "475", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "座椅通风", sPid: "804", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "座椅加热", sPid: "504", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "座椅按摩功能", sPid: "543", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "电动座椅记忆", sPid: "803", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "儿童安全座椅固定装置", sPid: "495", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "第三排座椅", sPid: "805", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "娱乐通讯", sPid: "", sTrPrefix: "13", unit: "", joinCode: "", scrollId: "params-pastime" },
//{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车载电话", sPid: "554", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "定位互动服务", sPid: "834", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "蓝牙系统", sPid: "479", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "外接音源接口", sPid: "810", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "3,4", sFieldTitle: "内置硬盘", sPid: "808,807", sTrPrefix: "13,13", unit: "G,", joinCode: "," },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "车载电视", sPid: "559", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "扬声器数量", sPid: "523", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "音响品牌", sPid: "473", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "9,8", sFieldTitle: "DVD", sPid: "509,510", sTrPrefix: "13,13", unit: "碟,", joinCode: "," },
{ sType: "fieldMulti", sFieldIndex: "11,10", sFieldTitle: "CD", sPid: "489,490", sTrPrefix: "13,13", unit: "碟,", joinCode: "," },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "中控台液晶屏", sPid: "488", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后排液晶屏", sPid: "477", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "空调/冰箱", sPid: "", sTrPrefix: "15", unit: "", joinCode: "", scrollId: "params-air" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "空调控制方式", sPid: "471", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "2,1", sFieldTitle: "温度分区控制", sPid: "555,839", sTrPrefix: "15,15", unit: ",", joinCode: "," },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "后排独立空调", sPid: "838", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "后排出风口", sPid: "478", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "空气调节/花粉过滤", sPid: "840", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车内空气净化装置", sPid: "905", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "车载冰箱", sPid: "485", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "易车测试", sPid: "", sTrPrefix: "14", unit: "", joinCode: "", scrollId: "params-test" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "加速时间（0—100km/h）", sPid: "786", sTrPrefix: "14", unit: "s", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "制动距离（100—0km/h）", sPid: "787", sTrPrefix: "14", unit: "m", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "油耗", sPid: "788", sTrPrefix: "14", unit: "L/100km", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "18米绕桩速度", sPid: "861", sTrPrefix: "14", unit: "km/h", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "车内怠速噪音dB(A)", sPid: "789", sTrPrefix: "14", unit: "dB", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "车内等速（40km/h）噪音", sPid: "857", sTrPrefix: "14", unit: "dB", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车内等速（60km/h）噪音", sPid: "790", sTrPrefix: "14", unit: "dB", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "车内等速（80km/h）噪音", sPid: "858", sTrPrefix: "14", unit: "dB", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车内等速（100km/h）噪音", sPid: "859", sTrPrefix: "14", unit: "dB", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "车内等速（120km/h）噪音", sPid: "860", sTrPrefix: "14", unit: "dB", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "更多信息", sTrPrefix: "14", unit: "", joinCode: "" },
   { sType: "fieldIndex", sFieldIndex: "0", sFieldTitle: "车型指数排名", sTrPrefix: "0", unit: "", joinCode: "" },
   { sType: "fieldOther", sFieldIndex: "11", sFieldTitle: "推荐阅读", sTrPrefix: "0", unit: "", joinCode: "" }
];

function selectCar(serialId, curentIndex) {
    //$.ajax({
    //	url: "http://api01.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid=" + serialId, cache: true, dataType: "jsonp", jsonpCallback: "getCarListCallback", success: function (data) {
    //		if (!(data && data.CarList)) return;
    //		var h = [];
    //		h.push("<dl class=\"tt-list\">");
    //		for (var key in data.CarList) {
    //			h.push("    <dt><span>" + key.replace("s", "") + "款</span></dt>");
    //			for (var i = 0; i < data.CarList[key].length; i++) {
    //				h.push("    <dd class=\"current\">");
    //				h.push("        <a href=\"javascript:;\" onclick=\"selectCarId(" + data.CarList[key][i].CarId + ")\">");
    //				h.push("            <p>" + data.CarList[key][i].CarName + "</p>");
    //				h.push("            <strong>" + data.CarList[key][i].Price + "万</strong>");
    //				h.push("        </a>");
    //				h.push("    </dd>  ");
    //			}
    //		}
    //		h.push("</dl>");
    //		$(".swipeLeft").html(h.join(''));
    //	}
    //});
}
////选择某子品牌下车型
//function selectCar(serialId, curentIndex) {
//	var carinfo_container = document.getElementById("carinfo_container");
//	currentIndexId = curentIndex;
//	BitAjax({
//		url: "/handlers/GetCarListHtml.ashx?id=" + serialId,
//		cache: true,
//		success: function (data) {
//			if (data != "") {
//				document.documentElement.scrollTop = 0;
//				document.body.scrollTop = 0;
//				//                if (carinfo_container)
//				//                    carinfo_container.style.display = "block";
//				carinfo_container.childNodes[1].innerHTML = data;
//				//隐藏菜单
//				$("#popMenu").hide();
//				if (typeof ComparePageObject != "undefined" && typeof ComparePageObject.arrCarIds != "undefined") {
//					//已选择车型
//					for (var i = 0; i < ComparePageObject.arrCarIds.length; i++) {
//						if (i != curentIndex) {
//							var dlCar = $("#" + ComparePageObject.arrCarIds[i]);
//							dlCar.attr("class", "none");
//							var txtA = dlCar.children().html();
//							dlCar.html("<a>" + txtA + "</a>");
//							selectCarMenu("carinfo_container");
//						}
//						else {
//							var dlCar = $("#" + ComparePageObject.arrCarIds[i]);
//							dlCar.attr("class", "current");
//						}
//					}
//				}
//			}
//		}
//	});
//}
//function selectCarMenu(PopId) {
//	$("#" + PopId).show();
//	$("#" + PopId + " .swipeLeft").addClass("swipeLeft-block");
//	$(".leftmask").show();
//	$(".leftPopup").css("zIndex", 199);

//	var documentHeight = $(document).height(); // 页面内容的高度
//	var leftPopupHeight = $("#" + PopId + " .swipeLeft").height();
//	leftPopupHeight = (documentHeight > leftPopupHeight) ? documentHeight : leftPopupHeight; // 弹出层高度
//	$('.leftmask, .leftPopup').css('height', leftPopupHeight);

//}

//$(".leftmask").click(function () {
//	$(".swipeLeft").removeClass("swipeLeft-block");
//	$(this).hide();
//	$(".leftPopup").css("zIndex", 0).hide();
//	var top = $(document).scrollTop();
//	if (top > 40)
//		$("#popMenu").show();
//});

function gotoSubMenu() {
    $("#popup-menulist a").each(function (i, n) {
        var id = $(this).data("target"),
			tit = $(this).text(),
			top = $("#" + id).offset().top,
			headerHeight = $(".flex").height();
        $(this).on("click", function (e) {
            e.preventDefault();
            $("html,body").animate({ scrollTop: (top - headerHeight + 34) }, 300, function () {
                setTimeout(function () { $(".section-tt.phone-title span").html(tit); }, 200);
            });
            $("#popup-menulist li").removeClass("current");
            $(this).parent().addClass("current");
            $("#popup-menulist").hide();
            $("#popup-menumask").hide();
        });
    });
    //点击其他地方 隐藏
    $(document).on("touchstart", function (e) {
        var targetId = e.target.id;
        if (targetId != "popup-menu" && $(e.target).closest(".catalog-list").attr("id") != "popup-menulist") {
            $(".catalog-list").hide();
            $("#popup-menumask").hide();
        }
    });
}
function callbackFunc() {
    var $root = $('.m-tool-compare');
    //浮动层
    (function ($root) {
        var $flex = $root.find('.flex'),
			$header = $flex.find('.section-tt table'),
			$headerLi = $header.find('span'),
			$append = $root.find('.flex-append');
        var flexTop = $flex[0].offsetTop, flexHeight = $flex.height();

        var rows = $('.phone-title', $root);
        var arr = [];
        var startTop = 0;
        for (var i = 0; i < rows.length; i++) {
            arr.push(rows[i].getAttribute('data-key'));
        }

        //导航插件
        rows.navigate({
            speed: 30,
            top: $flex.offsetHeight() - 35,
            init: function () {
                $(document.body).touches({
                    touchstart: function (ev) {
                        startTop = $(window).scrollTop();
                    }
                })
            },
            selectFn: function (idx) {
                var $this = this;
                var k = arr[idx];
                clearTimeout($this.timeout)
                $this.timeout = setTimeout(function () { $headerLi.html(k); }, 50);
            },
            scrollTo: function () {
                var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
                $flex.css({ 'position': 'initial' });
                $append.hide();
                if (scrollT >= flexTop) {
                    $flex.css({ 'position': 'fixed', 'top': 0, 'left': 0 });
                    $append.show();
                }
            }
        });
    })($root);

    //左右滑动
    (function ($root) {
        var $contflex = $('.section-flex .flex-right', $root);
        var $contTx = $('.section-tx .cont', $root);

        function flexScrollTo(ev) {
            if ($contflex.site == '') {
                $contflex.site = Math.abs(this.x - $contflex.x) > Math.abs(this.y - $contflex.y) ? 'x' : 'y';
            }
            if ($contflex.site == 'x') {
                ev.preventDefault();
                txScroll.scrollTo(this.x, 0, 0, false);
            } else {
                this.disable();
            }
        }

        function flexTouchEnd() {
            var $this = this;
            clearInterval($this.interval);
            $this.interval = setInterval(function () {
                txScroll.scrollTo($this.x, 0, 0, false);
                if ($this.ox == $this.x) {
                    clearInterval($this.interval)
                }
                $this.ox = $this.x;
            }, 30);
        }
        var flexScroll = new iScroll($contflex[0], {
            scrollX: true,
            scrollY: false,
            mouseWheel: true,
            scrollbarClass: 'nonebar',
            bounce: false,
            momentum: true,
            lockDirection: true,
            //snap: 'td',
            userTransiton: true,
            onScrollMove: flexScrollTo,
            onTouchEnd: flexTouchEnd,
            onBeforeScrollStart: function (ev) {
                //ev.preventDefault();
                $contflex.x = this.x;
                $contflex.y = this.y;
            },
            onScrollStart: function () {
                $contflex.site = '';

            }
        });

        $contflex.touches({ touchstart: function () { flexScroll.enable(); }, touchend: function () { flexScroll.enable(); } })

        function txScrollTo(ev) {

            if ($contTx.site == '') {
                $contTx.site = Math.abs(this.x - $contTx.x) > Math.abs(this.y - $contTx.y) ? 'x' : 'y';
            }
            if ($contTx.site == 'x') {
                ev.preventDefault();
                flexScroll.scrollTo(this.x, 0, 0, false);
            } else {
                this.disable();
            }

        }

        function txTouchEnd() {
            var $this = this;
            clearInterval($this.interval);
            $this.interval = setInterval(function () {
                flexScroll.scrollTo($this.x, 0, 0, false);
                if ($this.ox == $this.x) {
                    clearInterval($this.interval)
                }
                $this.ox = $this.x;
            }, 30);

        }

        var txScroll = new iScroll($contTx[0], {
            scrollX: true,
            scrollY: false,
            mouseWheel: true,
            scrollbarClass: 'nonebar',
            momentum: true,
            bounce: false,
            lockDirection: true,
            //snap: 'td',
            userTransiton: true,
            onScrollMove: txScrollTo,
            onTouchEnd: txTouchEnd,
            onBeforeScrollStart: function (ev) {
                //ev.preventDefault();
                $contTx.x = this.x;
                $contTx.y = this.y;
            },
            onScrollStart: function () {
                $contTx.site = '';
            }
        });
        $contTx.touches({ touchstart: function () { txScroll.enable(); }, touchend: function () { txScroll.enable(); } });

        var $rtxt = $('.r-diff', $root);

        /*解决横竖屏幕宽度计算问题*/
        function resizeTo() {
            flexScroll.refresh();
            txScroll.refresh();
            //解决title具右侧问题
            //console.log($rtxt.width())
            $rtxt.each(function (idx, curr) {
                var $curr = $(curr);
                var width = $curr.data('width');

                if (!width) {
                    width = $curr.html().split('').length * 12;
                }
                $curr.css({ 'left': document.documentElement.clientWidth - width, 'right': 'auto', 'display': 'block', 'width': width });
            });
        }
        $(window).resize(resizeTo).trigger('resize');
    })($root);



    var isCarScroll = false, clientHeight = 0, myScroll = null;
    //$('[data-action=models]').rightSwipe({
    //	clickEnd: function (b, elem) {
    //		var $leftPopup = this, currentIndex = elem.data("index"), currentCarId = elem.data("car");
    //		if (b) {
    //			$.ajax({
    //				url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid=" + serialId, cache: true, dataType: "jsonp", jsonpCallback: "getCarListCallback", success: function (data) {
    //					if (!(data && data.CarList)) return;
    //					var h = [];
    //					h.push("<dl class=\"tt-list\">");
    //					for (var key in data.CarList) {
    //						h.push("    <dt><span>" + key.replace("s", "") + "款</span></dt>");
    //						for (var i = 0; i < data.CarList[key].length; i++) {
    //							if (ComparePageObject && ComparePageObject.arrCarIds.length > 0 && ComparePageObject.arrCarIds.indexOf(data.CarList[key][i].CarId) != -1) {
    //								if (currentCarId == data.CarList[key][i].CarId) {
    //									h.push("<dd class=\"current\">");
    //								} else {
    //									h.push("<dd class=\"none\">");
    //								}
    //								h.push("<a>");

    //							} else {
    //								h.push("    <dd>");
    //								h.push("        <a href=\"javascript:;\" data-car=\"" + data.CarList[key][i].CarId + "\" data-index=\"" + currentIndex + "\">");
    //							}
    //							h.push("            <p>" + data.CarList[key][i].CarName + "</p>");
    //							h.push("            <strong>" + data.CarList[key][i].ReferPrice + "万</strong>");
    //							h.push("        </a>");
    //							h.push("    </dd>  ");
    //						}
    //					}
    //					h.push("</dl>");
    //					$(".swipeLeft").html(h.join(''));

    //					var $back = $('.' + $leftPopup.attr('data-back'))

    //					$back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
    //					var $swipeLeft = $leftPopup.find('.swipeLeft');
    //					$swipeLeft.css('overflow-y', 'scroll');

    //					if (!isCarScroll) {
    //						//isCarScroll = true;
    //						myScroll = new iScroll($swipeLeft[0], {
    //							snap: 'dd',
    //							momentum: true,
    //							click: true,
    //							bounce: true,
    //							onBeforeScrollStart: function (ev) { ev.preventDefault(); }
    //						});
    //					}    //					$swipeLeft.find('a[data-car]').each(function (index, curr) {
    //						var $curr = $(curr);
    //						(function ($o) {
    //							$o.touches({
    //								touchstart: function (ev) {
    //									ev.preventDefault();
    //									$o.disY = ev.targetTouches[0].pageY;
    //									$o.disX = ev.targetTouches[0].pageX;
    //								},

    //								touchend: function (ev) {
    //									if ($o.disY == ev.changedTouches[0].pageY && $o.disX == ev.changedTouches[0].pageX) {
    //										ev.preventDefault();
    //										var carId = this.data("car"), currentIndex = this.data("index");
    //										selectCarId(carId, currentIndex);
    //									}
    //								}
    //							});
    //						})($curr);
    //					});
    //				}
    //			});
    //		} else {
    //			//myScroll.scrollTo(0, 0, 30, false);
    //		}
    //	}
    //});
    //自适应页脚
    $(document.body).footer({ footer: '.footer-box' });
    var $body = $('body');
    $body.trigger('rightswipe1',
     {
         selectmark: function () { },
         fnEnd: function (paras) {
             for (var i = 0; i < ComparePageObject.arrCarIds.length; i++) {
                 if ($('[data-id=' + ComparePageObject.arrCarIds[i] + ']') && ComparePageObject.arrCarIds[i] != paras.carobj.attr("data-car")) {
                     $('[data-id=' + ComparePageObject.arrCarIds[i] + ']').parent()[0].className = 'none';
                 }
                 if ($('[data-id=' + ComparePageObject.arrCarIds[i] + ']') && ComparePageObject.arrCarIds[i] == paras.carobj.attr("data-car")) {
                     $('[data-id=' + ComparePageObject.arrCarIds[i] + ']').parent()[0].className = 'current';
                 }
             }
         }
     });
}
//6701->6,701
function formatCurrency(num) {
    if (isNaN(num)) {
        return "";
    }
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num)) num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    num = Math.floor(num / 100).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num);
}
//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}