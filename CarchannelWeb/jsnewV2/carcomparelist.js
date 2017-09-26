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
    ArrTempLeftNavHTML: [],
    ArrLeftNavHTML: [],
    ArrTempHideLeftNavId: [],
    ArrHideLeftNavIds: [],
    FloatTop: new Array(),
    FloatLeft: new Array(),
    ValidCount: 0,
    MaxTDLeft: 5,
    NeedBlockTD: 0,
    DragID: "",
    DropID: "",
    OtherCarInterface: "http://api.car.bitauto.com/CarInfo/GetCarHotCompare.ashx?carid=",
    LastFirstInterface: Array(),
    IsCloseHotList: false,
    CreateSelectChange: new Array(),
    SelectControlTemp: "",
    BSelect: null,
    BaikeObj: null,
    IsShowFloatTop: false,
    arrCarIds: [],
    arrSerialIds: [],
    IsShowDiff: true, //差异显示
    IsVantage: true, //优势项 默认选中
    IE6: ! -[1, ] && !window.XMLHttpRequest,
    IE7: navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1,
    IsIE: (navigator.appName == "Microsoft Internet Explorer"),
    IsChange: false,
    OneLeftScrollFlag: false, //滚动菜单是否显示 ，用于 左侧滚动 > 居左距离
    MenuOffsetTop: 260, //滚动菜单 相对车款浮动头的高度偏移量
    MenuOffsetLeft: 80//滚动菜单 相对表格 左偏移量
    //DocumentWidthLimit: 1400 //屏幕宽度临界值
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
    var hash = window.location.hash;
    if (hash.indexOf("&vantage") != -1) ComparePageObject.IsVantage = true;
    if (hash.indexOf("&diff") != -1) ComparePageObject.IsOperateTheSame = true;
    if (hash.indexOf("&showdiff") != -1) ComparePageObject.IsShowDiff = true;
    //初始化 左上角多选框选中状态
    if (ComparePageObject.IsVantage)
        $("#smallfixed input[name='chkAdvantage']").attr("checked", true);
    if (ComparePageObject.IsOperateTheSame)
        $("#smallfixed input[name='checkboxForDelTheSame']").attr("checked", true);
    if (ComparePageObject.IsShowDiff)
        $("#smallfixed input[name='checkboxForDiff']").attr("checked", true);

    createPageForCompare(ComparePageObject.IsOperateTheSame);


    //	//添加滚动监听事件
    //	$('[data-spy="scroll"]').each(function () {
    //		var $spy = $(this);
    //		$spy.scrollspy($spy.data(), function () { scrollCallback(); });
    //	});
}
//构建空表格数据
function createEmptyTable() {
    var flag = false;
    for (var i = 0; i < arrField.length; i++) {
        if (arrField[i].sType == "bar" && arrField[i]["sFieldTitle"] == "车体") break;
        if (arrField[i].sType == "bar" && arrField[i]["sFieldTitle"] == "基本信息") {
            var arrFieldRow = arrField[i];
            flag = true;
            ComparePageObject.ArrPageContent.push("<tr id=\"" + arrFieldRow.scrollId + "\"><td class=\"pd0 td-tt\" colspan=\"" + (ComparePageObject.MaxTDLeft + 1) + "\"><h2><span>基本信息</span></h2></td></tr>");
        }
        ComparePageObject.ArrPageContent.push("<tr>");
        if (arrField[i].sType == "fieldPara" && flag) {
            var arrFieldRow = arrField[i];
            ComparePageObject.ArrPageContent.push("<th>" + checkBaikeForTitle(arrFieldRow) + "</th>");
            for (var j = 0; j < ComparePageObject.MaxTDLeft; j++) {
                ComparePageObject.ArrPageContent.push("<td name=\"td" + j + "\">&nbsp;</td>");
            }
        }
        ComparePageObject.ArrPageContent.push("</tr>");
    }
}
function createPageForCompare(isDelSame) {
    if (ComparePageObject.ValidCount <= 0) {
        $("#left-nav").hide();
    }
    ComparePageObject.IsDelSame = isDelSame;
    var loopCount = arrField.length;
    ComparePageObject.ArrPageContent.push("<table cellspacing=\"0\" cellpadding=\"0\" style=\"border-right:500px solid white;\"><tbody>");
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
    //创建空数据
    if (ComparePageObject.ValidCount <= 0) createEmptyTable();

    ComparePageObject.ArrPageContent.push("</tbody></table>");
    // end
    if (ComparePageObject.PageDivContentObj) {
        ComparePageObject.PageDivContentObj.html(ComparePageObject.ArrPageContent.join(""));
    }
    ComparePageObject.ArrPageContent.length = 0;
    // set img drag
    setImgDrag();

    bindSelectForCompare();
    changeWhenFloatTop();
    if (ComparePageObject.ValidCount > 0) {
        $("#leftfixed").html("<table cellpadding=\"0\" cellspacing=\"0\" class=\"floatTable floatTable_peizhi\">" + ComparePageObject.FloatLeft.join("") + "</table>");
        ComparePageObject.FloatLeft.length = 0;
    }
    //填充左侧滚动菜单
    if (ComparePageObject.ValidCount > 0) {
        $("#left-nav ul").html(ComparePageObject.ArrLeftNavHTML.join('')).find("li:last").addClass("last").show();
        ComparePageObject.ArrLeftNavHTML.length = 0;
    }

    ComparePageObject.ArrHideLeftNavIds.length = 0;
    changeCheckBoxStateByName("checkboxForDelTheSame", ComparePageObject.IsOperateTheSame);
    setTRColorWhenMouse();
    mathLeftMenu($(window).scrollLeft(), $("#topBox").offset().left, $(window).scrollTop(), $("#main_box").offset().top);
    //绑定事件
    bindEvent();
    //计算左侧浮动导航高度
    reCountLeftNavHeight();
    //设置广告
    setCarCompareAD();
    //级别广告
    setLevelAD();
    //
    setSpecialAD();
}

//计算左侧浮动导航高度
function reCountLeftNavHeight() {
    var $mainTableTh = $('#CarCompareContent').find(" > table > tbody > tr > th"),
        $leftTableTh = $('#leftfixed').find(' > table > tbody > tr > th');
    $leftTableTh.each(function (i) {
        this.style.height = $mainTableTh.eq(i + 1).outerHeight() + 'px';
    });
}

//级别广告
function setLevelAD() {
    if (ComparePageObject.ValidCount <= 0 || ComparePageObject.ValidCount > 4) return;
    if (typeof adLevelJson == "undefined") {
        return;
    }
    var carLevel = "";
    var carPrice = 0;
    var carId = ComparePageObject.arrCarIds[ComparePageObject.arrCarIds.length - 1];
    for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
        if (ComparePageObject.AllCarJson[i][0][0] == carId) {
            carLevel = ComparePageObject.AllCarJson[i][0][12]; //车款
            carPrice = ComparePageObject.AllCarJson[i][1][0];//指导价
            break;
        }
    }
    if (carLevel == "") {
        return;
    }
    var adLevelItemObjArr = [];
    for (var i = 0; i < adLevelJson.length; i++) {
        var adObj = adLevelJson[i];
        if (adObj["level"] != carLevel) {
            continue;
        }
        if (adObj["serialId"] != undefined) {//有默认车款的，直接取默认车款
            adLevelItemObjArr.push(new LevelItemObj("csid", adObj["serialId"], adObj["defCarId"]));
        }
        else {
            adLevelItemObjArr.push(new LevelItemObj("carid", adObj["defCarId"]));
        }
    }
    if (adLevelItemObjArr.length == 0) {
        return;
    }
    var csIdArr = [];
    var carIdsArr = [];
    for (var i = 0; i < adLevelItemObjArr.length; i++) {
        if (adLevelItemObjArr[i].type == "csid") {
            csIdArr.push(adLevelItemObjArr[i].value);
        }
        else if (adLevelItemObjArr[i].type == "carid") {
            carIdsArr.push(adLevelItemObjArr[i].value);
        }
    }
    if (csIdArr.length > 0 || carIdsArr.length > 0) {
        $.ajax({
            url: "http://api.car.bitauto.com/CarInfo/GetCarList.ashx?saleState=1&csids=" + csIdArr.join(",") + "&carids=" + carIdsArr.join(","), cache: true, dataType: 'jsonp', jsonpCallback: "adCallback1", success: function (data) {
                if (!(data)) return;
                for (var i = 0; i < csIdArr.length; i++) {
                    var csCarInfo = data["serial"][csIdArr[i]];
                    if (!csCarInfo) {
                        continue;
                    }
                    var tempCsCarInfo = csCarInfo;
                    csCarInfo = [];
                    for (var j = 0; j < tempCsCarInfo.length; j++) { //删除已参加对比的车款
                        if (ComparePageObject.arrCarIds.indexOf(tempCsCarInfo[j].carId) < 0) {
                            csCarInfo.push(tempCsCarInfo[j]);
                        }
                    }
                    if (carPrice == "无") {//已选车款没有参考价，取车系下pv最高的车款
                        var isContinue = true;
                        for (var j = 0; j < adLevelItemObjArr.length; j++) {//有默认车款，并且默认车款已经参加对比，在车系里选择其他车款，否则，直接用默认车款
                            if (adLevelItemObjArr[j].type == "csid" && adLevelItemObjArr[j].value == csIdArr[i] && adLevelItemObjArr[j].adCarId != undefined) {
                                for (var k = 0; k < csCarInfo.length; k++) {
                                    if (csCarInfo[k].carId == adLevelItemObjArr[j].adCarId) {
                                        isContinue = false;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        if (!isContinue) {
                            continue;
                        }
                        csCarInfo = csCarInfo.sort(function (a, b) { var pv1 = parseInt(a["pv"]), pv2 = parseInt(b["pv"]); return ((pv1 < pv2) ? -1 : ((pv1 > pv2) ? 1 : 0)); });
                        for (var j = 0; j < adLevelItemObjArr.length; j++) {
                            if (adLevelItemObjArr[j].type == "csid" && adLevelItemObjArr[j].value == csIdArr[i]) {
                                //adLevelItemObjArr[j] = new LevelItemObj("carid", csCarInfo[csCarInfo.length - 1]);
                                adLevelItemObjArr[j].adCarId = csCarInfo[csCarInfo.length - 1].carId;
                                break;
                            }
                        }
                    }
                    else {//如果已选车款有参考价，取该车系下指导价与已选车款的指导价最接近的在销车款；如果指导价最接近的车款有多个，按关注度最高的取。
                        var adCarId = "";
                        csCarInfo = csCarInfo.sort(function (a, b) {//排序
                            var rp1 = parseFloat(a["referPrice"]), rp2 = parseFloat(b["referPrice"]), pv1 = parseInt(a["pv"]), pv2 = parseInt(b["pv"]);
                            return ((rp1 < rp2) ? -1 : ((rp1 > rp2) ? 1 : ((pv1 < pv2) ? -1 : ((pv1 > pv2) ? 1 : 0))));
                        });
                        var tempCarPrice = parseFloat(carPrice.replace("万", ""));
                        if (parseFloat(csCarInfo[0].referPrice) > tempCarPrice) {//小于最小价格
                            adCarId = csCarInfo[0].carId;
                        } else if (parseFloat(csCarInfo[csCarInfo.length - 1].referPrice) < tempCarPrice) {//大于最大价格
                            adCarId = csCarInfo[csCarInfo.length - 1].carId;
                        }
                        else {
                            for (var j = 0; j < csCarInfo.length; j++) {
                                if (tempCarPrice >= parseFloat(csCarInfo[j].referPrice) && tempCarPrice <= parseFloat(csCarInfo[j + 1].referPrice)) {
                                    var subPrice1 = tempCarPrice - parseFloat(csCarInfo[j].referPrice);
                                    var subPrice2 = parseFloat(csCarInfo[j + 1].referPrice) - tempCarPrice;
                                    if (subPrice1 > subPrice2) {
                                        adCarId = csCarInfo[j + 1].carId;
                                        break;
                                    }
                                    else if (subPrice1 < subPrice2) {
                                        adCarId = csCarInfo[j].carId;
                                        break;
                                    }
                                    else {
                                        if (parseFloat(csCarInfo[j].pv) >= parseFloat(csCarInfo[j + 1].pv)) {
                                            adCarId = csCarInfo[j].carId;
                                        }
                                        else {
                                            adCarId = csCarInfo[j + 1].carId;
                                        }
                                    }
                                }
                            }
                        }
                        for (var j = 0; j < adLevelItemObjArr.length; j++) {
                            if (adLevelItemObjArr[j].type == "csid" && adLevelItemObjArr[j].value == csIdArr[i]) {
                                adLevelItemObjArr[j].adCarId = adCarId;
                                break;
                            }
                        }
                    }
                }
                for (var j = 0; j < adLevelItemObjArr.length; j++) {
                    if (adLevelItemObjArr[j].type == "carid" && ComparePageObject.arrCarIds.indexOf(adLevelItemObjArr[j].value) < 0) {
                        var adCarId = adLevelItemObjArr[j].value;
                        var carJson = data["car"][adCarId];
                        if (!carJson) {
                            continue;
                        }
                        showRightAd(adCarId, carJson.CarName, carJson.ImageUrl, carJson.SerialName, carJson.AllSpell);
                        BglogPostLog("2.96.857");
                    }
                    else if (adLevelItemObjArr[j].type == "csid") {
                        var serialId = adLevelItemObjArr[j].value;
                        var adCarId = adLevelItemObjArr[j].adCarId;
                        var carListJson = data["serial"][serialId];
                        if (!carListJson) {
                            continue;
                        }
                        for (var k = 0; k < carListJson.length; k++) {
                            if (adCarId == carListJson[k].carId) {
                                showRightAd(carListJson[k].carId, carListJson[k].carName, carListJson[k].carPic, carListJson[k].csName, carListJson[k].allSpell);
                                BglogPostLog("2.96.857");
                                break;
                            }
                        }
                    }
                }
            }
        });
    }
}

function LevelItemObj(type, value, adCarId) {
    this.type = type;
    this.value = value;
    this.adCarId = adCarId;
}

function setSpecialAD() {
    if (typeof specialADConfig != "undefined") {
        var arrCarId = [], newCarID = [];
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            var carId = ComparePageObject.AllCarJson[i][0][0];
            arrCarId.push(carId);
        }
        var isFirst = ComparePageObject.AllCarJson.length < 5 ? true : false;
        var adCarId = getADCarId(arrCarId);
        if (adCarId > 0) {
            var adCarIndex = arrCarId.indexOf(adCarId);
            var adIndex = newCarID.indexOf(adCarId);
            if ((isFirst && adIndex == -1)) {
                arrCarId.push(adCarId);
            } else if (!isFirst && adIndex == -1) {
                arrCarId.splice(4, 0, adCarId);

            } else if (!isFirst && adIndex > 4) {
                arrCarId.remove(adCarId);
                arrCarId.splice(4, 0, adCarId);
            }
            if (arrCarId.length > 10) {
                arrCarId = arrCarId.slice(0, 10);
            }
            //删除添加车款后再添加相同车款 页面不刷新
            if (window.location.search == "?carIDs=" + arrCarId.join(",") + "") {
                //document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "&t=" + Math.random() + "#CarHotCompareList";
                //location.reload();
            }
            else {
                var paramsArr = [];
                if (ComparePageObject.IsOperateTheSame)
                    paramsArr.push("diff");
                if (ComparePageObject.IsShowDiff)
                    paramsArr.push("showdiff");
                if (ComparePageObject.IsVantage)
                    paramsArr.push("vantage");
                if (adCarIndex != -1 && adCarIndex <= 4) { }
                else
                    document.location.href = "/chexingduibi/?carIDs=" + arrCarId.join(",") + "#CarHotCompareList" + (paramsArr.length > 0 ? "&" + paramsArr.join("&") : "") + "";
            }
            $("#tuijian-" + adCarId).show();
        }
    }
}
function getNewArrCarId(newCarID) {
    if (typeof specialADConfig == "undefined")
        return newCarID;
    var isFirst = ComparePageObject.AllCarJson.length < 5 ? true : false;
    var adCarId = getADCarId(newCarID);
    if (adCarId > 0) {
        var adIndex = newCarID.indexOf(adCarId);
        if ((isFirst && adIndex == -1)) {
            newCarID.push(adCarId);
        } else if (!isFirst && adIndex == -1) {
            newCarID.splice(4, 0, adCarId);

        } else if (!isFirst && adIndex > 4) {
            newCarID.remove(adCarId);
            newCarID.splice(4, 0, adCarId);
        }
        if (newCarID.length > 10) {
            newCarID = newCarID.slice(0, 10);
        }
    }
    return newCarID;
}
function getADCarId(curArrCarId) {
    var carId = 0;
    if (typeof specialADConfig == "undefined")
        return carId;
    for (var key in specialADConfig) {
        var result = specialADConfig[key].intersect(curArrCarId);
        if (result.length > 0) {
            carId = key.replace("c", "");
            break;
        }
    }
    return carId;
}
//==============================设置广告 add 2014.12.28=========================
function setCarCompareAD() {
    $("#rightfixed-list ul").html('');
    $("#rightfixed-bar,#rightfixed").hide();
    if (ComparePageObject.ValidCount <= 0 || ComparePageObject.ValidCount > 4) return;
    var adArrCarId = [];
    //初始化有效车型ID
    ComparePageObject.arrCarIds.length = 0;
    ComparePageObject.arrSerialIds.length = 0;

    initCarIds();

    if (typeof adJson != "undefined" && adJson.Car) {
        for (var adCarId in adJson.Car) {
            var carIds = adJson.Car[adCarId];
            for (var i = 0; i < carIds.length; i++) {
                if (ComparePageObject.arrCarIds.indexOf(adCarId) == -1 && ComparePageObject.arrCarIds.indexOf(carIds[i]) != -1 && adArrCarId.indexOf(adCarId) == -1) {
                    adArrCarId.push(adCarId);
                    break;
                }
            }
        }
    }
    if (typeof adJson != "undefined" && adJson.Serial) {
        for (var adCarId in adJson.Serial) {
            var serialIds = adJson.Serial[adCarId];
            for (var i = 0; i < serialIds.length; i++) {
                if (ComparePageObject.arrCarIds.indexOf(adCarId) == -1 && ComparePageObject.arrSerialIds.indexOf(serialIds[i]) != -1 && adArrCarId.indexOf(adCarId) == -1) {
                    adArrCarId.push(adCarId);
                    break;
                }
            }
        }
    }
    if (adArrCarId.length > 0)
        requestAdCarData(adArrCarId);
}
function initCarIds() {
    for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
        var carId = ComparePageObject.AllCarJson[i][0][0];
        var serialId = ComparePageObject.AllCarJson[i][0][3];
        if (ComparePageObject.arrCarIds.indexOf(carId) == -1)
            ComparePageObject.arrCarIds.push(carId);
        if (ComparePageObject.arrSerialIds.indexOf(serialId) == -1)
            ComparePageObject.arrSerialIds.push(serialId);
    }
}
function requestAdCarData(arrCarId) {
    for (var i = 0; i < 4 && i < arrCarId.length; i++) {
        $.ajax({
            url: "http://api.car.bitauto.com/carinfo/carinfojson.ashx?action=baseinfo&carid=" + arrCarId[i], cache: true, dataType: 'jsonp', jsonpCallback: "adCallback" + i, success: function (data) {
                if (!(data)) return;
                showRightAd(data.CarId, data.CarName, data.ImageUrl, data.SerialName, data.AllSpell);
            }
        });
    }
}
function showRightAd(CarId, CarName, ImageUrl, SerialName, AllSpell) {
    $("#rightfixed").show();
    var arrHtml = [];

    arrHtml.push("<li>");
    arrHtml.push("<a href=\"/" + AllSpell + "/\" target=\"_blank\" data-channelid=\"2.96.858\"><img src=\"" + ImageUrl + "\"></a>");
    arrHtml.push("<dl>");
    arrHtml.push("<dt data-channelid=\"2.96.859\"><a target=\"_blank\" href=\"/" + AllSpell + "/\">" + SerialName + "</a></dt>");
    arrHtml.push("<dd data-channelid=\"2.96.860\"><a target=\"_blank\" href=\"/" + AllSpell + "/m" + CarId + "/\">" + CarName + "</a></dd>");
    arrHtml.push("</dl>");
    arrHtml.push("<div class=\"rightfixed-btn-duibi button_gray\" data-channelid=\"2.96.861\">");
    arrHtml.push("<a target=\"_self\" href=\"javascript:addCarToCompareForSelect(" + CarId + ",'','');\"><span>对比</span></a>");
    arrHtml.push("</div>");
    arrHtml.push("</li>");

    $("#rightfixed-list ul").append(arrHtml.join(''));
    //统计异步初始化
    Bglog_InitPostLog();
    //广告跟随右侧边栏移动
    setRightFixedAdAnimate();
}
//广告跟随右侧边栏移动
function setRightFixedAdAnimate() {
    var count = 0;
    var adAnimateInt = setInterval(function () {
        count++;
        if (count > 10) {
            clearInterval(adAnimateInt);
            return;
        }
        if ($("#rightfixed-list li").length > 0 && $("#bitautoSideBarContainer").width() > 0 && typeof top.window.Bitauto.iMediator != "undefined") {
            clearInterval(adAnimateInt);

            $("#rightfixed,#rightfixed-bar").css("right", $("#headerModulesContainer").width());
            top.window.Bitauto.iMediator.clear('sidebar.show');//sidebar.hide
            top.window.Bitauto.iMediator.subscribe("sidebar.show", function () {
                var scrollsLeft = $(this).scrollLeft();
                $("#rightfixed,#rightfixed-bar").animate({ "right": $("#bitautoSideBarContainer").width() - scrollsLeft }, 500);
                //$("#rightfixed-bar").animate({ "right": $("#bitautoSideBarContainer").width() }, 500);
            });
            top.window.Bitauto.iMediator.clear('sidebar.hide');//sidebar.hide
            top.window.Bitauto.iMediator.subscribe("sidebar.hide", function () {
                var scrollsLeft = $(this).scrollLeft();
                $("#rightfixed,#rightfixed-bar").animate({ "right": $("#headerModulesContainer").width() - scrollsLeft }, 500);
                //$("#rightfixed-bar").animate({ "right": $("#headerModulesContainer").width() }, 500);
            });
        }
    }, 100);
}

function InitRightFixedAdPosition() {
    if (typeof $("#navtool-ul") != undefined && $("#navtool-ul").length > 0 && $("#navtool-ul").parent().css("display") == "block") {
        $("#rightfixed,#rightfixed-bar").css("right", $("#navtool-ul").width() - $(this).scrollLeft());
    }
    else {
        $("#rightfixed,#rightfixed-bar").css("right", 0 - $(this).scrollLeft());
    }
}


//========================================================================
//绑定事件
function bindEvent() {
    //纠错
    var strError = "车款名称：\n问题描述：\n联系方式(电话/QQ)：";
    $("a[name='correcterror']").click(function () { $("#popup-box").showCenter(); $("#correctError").val(strError); });
    $("#popup-box .close,#btnErrorCancel").click(function () { $("#popup-box").hide(); });
    $("#popup-box-success .close,#btn-success-close").click(function () { $("#popup-box-success").hide(); });
    $("#popup-box a[name='btnCorrectError']").click(function (e) {
        e.preventDefault();
        var content = $("#correctError").val();
        if (content == "" || content == strError) {
            $("#popup-box .modal-content span").html("请输入提交内容。").show();
            return;
        } $("#popup-box .modal-content span").html("").hide();
        var url = "http://www.bitauto.com/FeedBack/api/CommentNo.ashx?txtContent=" + encodeURIComponent(content) + "&satisfy=1&ProductId=5&categorytype=2&refer=http://car.bitauto.com" + window.location.pathname;
        $.ajax({
            url: url, dataType: 'jsonp', jsonp: "XSS_HTTP_REQUEST_CALLBACK", jsonpCallback: "errorCallback", success: function (data) {
                if (data.status == "success") {
                    $("#popup-box").hide();
                    $("#popup-box-success").removeClass("note-error").showCenter().find("h3").html("提交成功！").siblings("p").html("侦察兵，好样的！");
                } else {
                    $("#popup-box").hide();
                    $("#popup-box-success").addClass("note-error").showCenter().find("h3").html("提交失败！").siblings("p").html(data.message);
                }
            }
        });
    });
    //左侧隐藏按钮
    $("#show-left-nav").click(function () {
        ComparePageObject.OneLeftScrollFlag = true;
        $("#left-nav").show();
        $("#close-left-nav").show();
        $(this).hide();
    });
    //菜单隐藏按钮
    $("#close-left-nav").click(function () {
        ComparePageObject.OneLeftScrollFlag = false;
        $("#left-nav").hide();
        $(this).hide();
        $("#show-left-nav").show();
    });

    //元素切换效果
    //$("div[id^='tableHead_']").clickChange({ "leftCallback": moveLeftForCarCompare, "rightCallback": moveRightForCarCompare });
    $(".tableHead_item").clickChange({ "leftCallback": moveLeftForCarCompare, "rightCallback": moveRightForCarCompare });

    //添加滚动监听事件
    $('[data-spy="scroll"]').each(function () {
        var $spy = $(this);
        $spy.scrollspy($spy.data(), function () { scrollCallback(); });
        $spy.scrollspy("refresh");
    });
    //点击空白 换车 推荐 弹出层隐藏
    $(document).click(function (e) {
        //e.preventDefault();
        e = e || window.event;
        var target = e.srcElement || e.target;
        //换车
        if (($(target).closest(".huanche-layer").length <= 0) && ($(target).closest("div[id^='change_car_']").length <= 0)) {
            $(".change-car-area div[id^='change_car_']").removeClass("btn-hide-car").siblings(".huanche-layer").hide();
        }
        //为我推荐
        if ($(target).closest(".recommend").length <= 0 && ($(target).closest(".btn-hide-car").length <= 0)) {
            $(".recommend").removeClass("btn-hide-car").children(".tuijian-layer").hide();
        }
    });
    $("#right-close").click(function () { $("#rightfixed").hide(); $("#rightfixed-bar").show(); });
    $("#rightfixed-bar").click(function () { $("#rightfixed").show(); $(this).hide(); });
}
//已选车款不可选
function setCarDisabled() {
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        var carId = ComparePageObject.AllCarJson[i][0][0],
            carName = ComparePageObject.AllCarJson[i][0][1],
            carPrice = ComparePageObject.AllCarJson[i][1][0];
        var carTypeObj = $("#cartype4 .zcfcbox .models_detail a[bita-value='" + carId + "']");
        $(carTypeObj).unbind("click").parent().html("<span>" + carName + "<strong>" + $(carTypeObj).attr("bita-price") + "</strong></span>");
        var carTypeSelect = $("div[id^='CarTypeSelectList_'] .zcfcbox .models_detail a[bita-value='" + carId + "']");
        $(carTypeSelect).unbind("click").parent().html("<span>" + carName + "<strong>" + $(carTypeSelect).attr("bita-price") + "</strong></span>");
    }
}
// create pic for compare
function createPic() {
    // for FloatTop
    ComparePageObject.FloatTop.length = 0;
    ComparePageObject.FloatTop.push("<table cellpadding=\"0\" cellspacing=\"0\" class=\"parameter\">");
    ComparePageObject.FloatTop.push("<tr>");
    ComparePageObject.FloatTop.push("<th class=\"pd0\">");
    ComparePageObject.FloatTop.push("<div class=\"tableHead_left\">");
    ComparePageObject.FloatTop.push("<h6>对比车型</h6>");
    //ComparePageObject.FloatTop.push("<p class=\"txt\">拖拽车款图片可调顺序</p>");
    ComparePageObject.FloatTop.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + "> <label for=\"chkAdvantage\">标识优势项 <em></em></label></div>");
    ComparePageObject.FloatTop.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + "> <label for=\"checkboxForDiff\">高亮不同项</label></div>");
    ComparePageObject.FloatTop.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" > <label for=\"checkDiffer\">隐藏相同项</label></div>");

    ComparePageObject.FloatTop.push("<div class=\"dashline\"></div>");
    ComparePageObject.FloatTop.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
    ComparePageObject.FloatTop.push("</div>");
    ComparePageObject.FloatTop.push("</th>");

    // for FloatLeft
    ComparePageObject.FloatLeft.length = 0;
    ComparePageObject.FloatLeft.push("<tr>");
    ComparePageObject.FloatLeft.push("<td class=\"pd0\">");
    ComparePageObject.FloatLeft.push("<div class=\"tableHead_left\">");
    ComparePageObject.FloatLeft.push("<h6>对比车型</h6>");
    ComparePageObject.FloatLeft.push("<p class=\"txt\">拖拽车款图片可调顺序</p>");


    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" id=\"chkAdvantage\" name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + "> <label for=\"chkAdvantage\">标识优势项 <em></em></label></div>");
    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" id=\"checkboxForDiff\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + "> <label for=\"checkboxForDiff\">高亮不同项</label></div>");
    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDiffer\"> <label for=\"checkDiffer\">隐藏相同项</label></div>");




    ComparePageObject.FloatLeft.push("<div class=\"dashline\"></div>");
    ComparePageObject.FloatLeft.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
    ComparePageObject.FloatLeft.push("</div>");
    ComparePageObject.FloatLeft.push("</td>");
    ComparePageObject.FloatLeft.push("</tr>");


    ComparePageObject.ArrPageContent.push("<tr id=\"trForPic\">");
    var classNameAdd = "";
    if (!ComparePageObject.IsCloseHotList) {
        // classNameAdd = " bdbottomnone";
    }
    ComparePageObject.ArrPageContent.push("<th class=\"pd0" + classNameAdd + " \">");
    ComparePageObject.ArrPageContent.push("<div class=\"tableHead_left\">");
    ComparePageObject.ArrPageContent.push("<h6>对比车型</h6><p class=\"txt\">拖拽车款图片可调顺序</p>");

    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + "> <label for=\"chkAdvantage\">标识优势项 <em></em></label></div>");
    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + "> <label for=\"checkboxForDiff\">高亮不同项</label></div>");
    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\"> <label for=\"checkDiffer\">隐藏相同项</label></div>");



    ComparePageObject.ArrPageContent.push("<div class=\"dashline\"></div>");
    ComparePageObject.ArrPageContent.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p></div>");
    ComparePageObject.ArrPageContent.push("</th>");

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        ComparePageObject.ArrPageContent.push("<td class=\"pd0" + classNameAdd + "\" name=\"td" + i + "\" id=\"td_" + i + "\" style=\"position:relative; z-index:" + (10 - i) + "\" >");
        if (i == 0) {
            ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + i + "\" class=\"tableHead_item tableHead_item_nopic\">");
        }
        else { ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + i + "\" class=\"tableHead_item tableHead_item_nopic\">"); }
        //ComparePageObject.ArrPageContent.push("<div id=\"FloatTop_tableHead_0\" class=\"tableHead_item tableHead_item_nopic\">");
        ComparePageObject.ArrPageContent.push("<div class=\"sel-car-box droppable\" id=\"carBox_" + i + "\">");
        var pic = ComparePageObject.AllCarJson[i][0][2] || "";
        pic = pic == "" ? "http://image.bitautoimg.com/autoalbum/V2.1/images/120-80.gif" : pic.replace("_2.", "_1.");
        //ComparePageObject.ArrPageContent.push("<div name=\"hasCarBox\" id=\"carBox_" + i + "\" class=\"carBox\">");
        ComparePageObject.ArrPageContent.push("<div class=\"sel-car-move\" id=\"draggcarbox_" + i + "\">");
        ComparePageObject.ArrPageContent.push("<div><img id=\"imgdrag_" + i + "\" name=\"imgdrag\" alt=\"按住鼠标左键，可拖动到其他列\" src=\"" + pic + "\"></div>");
        ComparePageObject.ArrPageContent.push("<div class=\"dl-box\" style=\"height:80px;\">");
        ComparePageObject.ArrPageContent.push("<div id=\"draggcarbox_" + i + "\" class=\"dl-box\" style=\"width:150px;\">");
        //ComparePageObject.ArrPageContent.push("<img id=\"imgdrag_" + i + "\" name=\"imgdrag\" alt=\"按住鼠标左键，可拖动到其他列\" src=\"" + pic + "\">");
        ComparePageObject.ArrPageContent.push("<dl><dd class=\"car-name\"><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/\">" + ComparePageObject.AllCarJson[i][0][5] + "</a></dd>");

        var yearType = ComparePageObject.AllCarJson[i][0][7];
        ComparePageObject.ArrPageContent.push("<dd><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/\">" + ((yearType == "" || yearType == 0) ? "" : "" + yearType + "款 ") + ComparePageObject.AllCarJson[i][0][1] + "</a></dd>");
        ComparePageObject.ArrPageContent.push("</dl>");
        //ComparePageObject.ArrPageContent.push("</div>");
        ComparePageObject.ArrPageContent.push("</div>");
        ComparePageObject.ArrPageContent.push("</div>");
        ComparePageObject.ArrPageContent.push("</div>");

        ComparePageObject.ArrPageContent.push("<div class=\"change-car-area\">");

        // for FloatTop
        ComparePageObject.FloatTop.push("<td class=\"pd0\" style=\"position:relative; z-index:" + (10 - i) + ";\" >");
        ComparePageObject.FloatTop.push("<div id=\"FloatTop_tableHead_" + i + "\" class=\"tableHead_item tableHead_item_nopic\">");
        ComparePageObject.FloatTop.push("<div class=\"sel-car-box\">");
        ComparePageObject.FloatTop.push("<div class=\"dl-box\" style=\"height:100px;\"><div  id=\"FloatTop_carBox_" + i + "\" class=\"dl-box\" style=\"width:150px;\">");
        ComparePageObject.FloatTop.push("<dl><dd class=\"car-name\"><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/\">" + ComparePageObject.AllCarJson[i][0][5] + "</a></dd>");
        ComparePageObject.FloatTop.push("<dd><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/\">" + ((yearType == "" || yearType == 0) ? "" : "" + yearType + "款 ") + ComparePageObject.AllCarJson[i][0][1] + "</a></dd>");

        var referPrice = ComparePageObject.AllCarJson[i][1][0];
        if (referPrice != "无") {
            ComparePageObject.FloatTop.push("<dd class=\"car-price\"><a target=\"_blank\" href=\"/" + ComparePageObject.AllCarJson[i][0][6] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/baojia/\">" + ComparePageObject.AllCarJson[i][1][0] + "</a> (厂家指导价)</dd>");
        }
        ComparePageObject.FloatTop.push("</dl>");
        ComparePageObject.FloatTop.push("</div></div>");
        ComparePageObject.FloatTop.push("<div id=\"FloatTop_OptArea_" + i + "\" class=\"change-car-area\">");

        if (i > 0) {
            ComparePageObject.ArrPageContent.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\" title=\"左移\"><</a></span>");
            // for FloatTop
            ComparePageObject.FloatTop.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\" title=\"左移\"><</a></span>");
        }
        ComparePageObject.ArrPageContent.push("<div id=\"change_car_" + i + "\" class=\"button_gray btn-compare-car\"><a href=\"javascript:setSelectControlForCompare('tableHead_" + i + "','HeadTemp')\"><span class=\"huanche\">换辆车</span></a></div>");
        // for FloatTop
        ComparePageObject.FloatTop.push("<div id=\"change_car_float_" + i + "\" class=\"button_gray btn-compare-car\"><a href=\"javascript:setSelectControlForCompare('FloatTop_tableHead_" + i + "','Float_HeadTemp')\"><span class=\"huanche\">换辆车</span></a></div>");

        if (i != ComparePageObject.ValidCount - 1) {
            ComparePageObject.ArrPageContent.push("<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
            // for FloatTop
            ComparePageObject.FloatTop.push("<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
        }
        ComparePageObject.ArrPageContent.push("</div>");

        //		if (i == 0) {
        //			ComparePageObject.ArrPageContent.push("<div class=\"sx\">首选车型</div>");
        //		}
        ComparePageObject.ArrPageContent.push("<a class=\"btn-close-car\" title=\"删除\" href=\"javascript:;\" onclick=\"delCarToCompare('" + i + "',event);\">删除</a>");
        ComparePageObject.ArrPageContent.push("<em id=\"tuijian-" + ComparePageObject.AllCarJson[i][0][0] + "\" class=\"tuijian\" style=\"display:none;\">推荐</em>");
        ComparePageObject.ArrPageContent.push("</div>");

        ComparePageObject.FloatTop.push("</div>");
        ComparePageObject.FloatTop.push("<a class=\"btn-close-car\" title=\"删除\" href=\"javascript:;\" onclick=\"delCarToCompare('" + i + "',event);\">删除</a>");
        ComparePageObject.ArrPageContent.push("</div>");
        if (i == 0) {
            //ComparePageObject.ArrPageContent.push("<div style=\"display:none;\" class=\"firstCar_pop\" id=\"firstCar_pop\"></div>");
        }
        ComparePageObject.ArrPageContent.push("</td>");

        // for FloatTop
        ComparePageObject.FloatTop.push("</div>");
        ComparePageObject.FloatTop.push("</td>");
    }

    //when less 
    if (ComparePageObject.NeedBlockTD > 0) {
        for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
            var index = (ComparePageObject.ValidCount + i);
            ComparePageObject.ArrPageContent.push("<td class=\"pd0" + classNameAdd + "\">");
            if (i == 0) {
                ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + index + "\" class=\"tableHead_item tableHead_item_nopic\" bit-disabled=\"\">");
            }
            else {
                ComparePageObject.ArrPageContent.push("<div id=\"tableHead_" + index + "\" class=\"tableHead_item tableHead_item_nopic\">");
            }
            ComparePageObject.ArrPageContent.push("<div class=\"sel-car-box droppable tableHead_item_duibi\">");
            //ComparePageObject.ArrPageContent.push("<div class=\"top-txt\">车款" + (index + 1) + "</div>");
            if (i == 0) {
                ComparePageObject.ArrPageContent.push("<div id=\"master4\" class=\"brand-form no-second\" style=\"z-index:50\"></div>");
                ComparePageObject.ArrPageContent.push("<div id=\"serial4\" class=\"brand-form no-second\" style=\"z-index:40\"></div>");
                ComparePageObject.ArrPageContent.push("<div id=\"cartype4\" class=\"brand-form no-second\" style=\"z-index:30\"></div>");
                if (index > 0)
                    ComparePageObject.ArrPageContent.push("<div class=\"recommend button_gray\"><a href=\"javascript:recomCarList('tableHead_" + index + "'," + (index - 1) + ");\"><span class=\"weiwotuijian\">为我推荐</span></a></div>");

                // for FloatTop
                ComparePageObject.FloatTop.push("<td class=\"pd0\" style=\"position:relative; z-index:" + (ComparePageObject.NeedBlockTD - i) + "\">");
                ComparePageObject.FloatTop.push("<div id=\"FloatTop_tableHead_" + index + "\" class=\"tableHead_item tableHead_item_nopic\" bit-disabled=\"\">");
                ComparePageObject.FloatTop.push("<div class=\"sel-car-box tableHead_item_duibi\">");
                //ComparePageObject.FloatTop.push("<div class=\"recommend button_gray\"><a href=\"javascript:recomCarList('FloatTop_tableHead_" + index + "'," + (index - 1) + ");\"><span class=\"weiwotuijian\">为我推荐</span></a></div>");
                ComparePageObject.FloatTop.push("</div>");
                ComparePageObject.FloatTop.push("</div>");
                ComparePageObject.FloatTop.push("</td>");
            }
            else {
                ComparePageObject.ArrPageContent.push("<div class=\"brand-form no-second\" style=\"z-index:50\"><span class=\"default\">请选择品牌</span><a href=\"javascript:;\" class=\"jt\"></a></div>");
                ComparePageObject.ArrPageContent.push("<div class=\"brand-form no-second\" style=\"z-index:40\"><span class=\"default\">请选择车型</span><a href=\"javascript:;\" class=\"jt\"></a></div>");
                ComparePageObject.ArrPageContent.push("<div class=\"brand-form no-second\" style=\"z-index:30\"><span class=\"default\">请选择车款</span><a href=\"javascript:;\" class=\"jt\"></a></div>");

                // for FloatTop
                ComparePageObject.FloatTop.push("<td class=\"pd0\"><div class=\"tableHead_item tableHead_item_nopic\">");
                ComparePageObject.FloatTop.push("<div class=\"sel-car-box tableHead_item_duibi\"><div class=\"brand-form no-second\"><span class=\"default activ\">请选择品牌</span><a href=\"javascript:;\" class=\"jt\"></a></div><div class=\"brand-form no-second\"><span class=\"default activ\">请选择车型</span><a href=\"javascript:;\" class=\"jt\"></a></div><div class=\"brand-form no-second\"><span class=\"default activ\">请选择车款</span><a href=\"javascript:;\" class=\"jt\"></a></div></div>");
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
            $("#master4,#serial4,#cartype4").prependTo("div[id^='FloatTop_tableHead_'][bit-disabled] div:first");
        }
    } else {
        $("#master4,#serial4,#cartype4").insertBefore("div[id^='tableHead_'][bit-disabled] .recommend");
    }
}

// create bar for compare
function createBar(arrFieldRow) {
    ComparePageObject.ArrTempBarHTML.length = 0;
    ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
    ComparePageObject.ArrTempLeftNavHTML.length = 0;

    if (ComparePageObject.ValidCount > 0) {
        ComparePageObject.ArrTempBarHTML.push("<tr id=\"" + arrFieldRow["scrollId"] + "\">");
        var summaryColumn = 0;
        if (ComparePageObject.ValidCount < 5)
        { summaryColumn = 6; }
        else
        { summaryColumn = ComparePageObject.ValidCount + 2; }
        ComparePageObject.ArrTempBarHTML.push("<td class=\"pd0 td-tt\" colspan=\"" + summaryColumn + "\">");
        ComparePageObject.ArrTempBarHTML.push("<h2><span>" + arrFieldRow["sFieldTitle"] + "<a href=\"javascript:;\" name=\"correcterror\">参数纠错</a></span></h2>");
        ComparePageObject.ArrTempBarHTML.push("</td>");
        ComparePageObject.ArrTempBarHTML.push("</tr>");

        ComparePageObject.ArrTempBarForFloatLeftHTML.push("<tr>");
        ComparePageObject.ArrTempBarForFloatLeftHTML.push("<td class=\"pd0 td-tt\"><h2><span>" + arrFieldRow["sFieldTitle"] + "<a href=\"javascript:;\" name=\"correcterror\">参数纠错</a></span></h2></td>");
        ComparePageObject.ArrTempBarForFloatLeftHTML.push("</tr>");
        //左侧菜单数据
        ComparePageObject.ArrTempLeftNavHTML.push("<li class=\"" + (arrFieldRow["sFieldTitle"] == "基本信息" ? "current" : "") + "\"><a data-target=\"" + arrFieldRow["scrollId"] + "\" href=\"javascript:;\" title1=\"" + arrFieldRow["sFieldTitle"] + "\">" + arrFieldRow["sFieldTitle"] + "<i></i></a></li>");
    }
}

// create param for compare
function createPara(arrFieldRow) {
    var firstSame = true,
	isAllunknown = true,
	arrSame = [],
	arrTemp = [],
	isShowLoop = 0;
    var colorRow = 0;

    var chkResult = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 };
    var vantage = (ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1");
    if (vantage || ComparePageObject.IsShowDiff)
        chkResult = IsFieldSame(arrFieldRow);
    var parameterId = arrFieldRow["sPid"];

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.AllCarJson[i]) {
            if (!chkResult.IsSame && ComparePageObject.IsShowDiff)
                arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
            else if (!chkResult.IsSame && vantage && chkResult.CurrCarIndex != -1) {
                arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
            }
            else
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
                //				if (field.length > 0) {
                //					if (arrFieldRow["unit"] != "") {
                //						field += "" + arrFieldRow["unit"];
                //					}
                //				}
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
                ////add by sk 20141112
                //if (parameterId == "577") {
                //    // 燃油标号 90改为：89或90；93改为：92或93；97改为：95或97；
                //    switch (field) {
                //        case "90号": field = field + "(北京89号)"; break;
                //        case "93号": field = field + "(北京92号)"; break;
                //        case "97号": field = field + "(北京95号)"; break;
                //        default: break;
                //    }
                //}
                if (field.indexOf("有") == 0)
                { field = "<span class=\"songti\">●</span>"; }
                else if (field.indexOf("选配") == 0)
                {
                    var fieldInfo = field.split('|');
                    if (fieldInfo.length > 1) {
                        field = "<span class=\"songti\">○ 选配" + formatCurrency(fieldInfo[1]) + "元</span>";
                    }
                    else {
                        field = "<span class=\"songti\">○</span>";
                    }
                }
                else if (field.indexOf("无") == 0)
                { field = "<span class=\"songti\">-</span>"; }
                //modified by sk 2014.07.11 ie6 样式问题 空赋值 &nbsp;
                else if (field == "") field = "&nbsp;";
                //else {
                //    field = "<span class=\"songti\">●</span>" + field;
                //}
                // modified by chengl Dec.28.2009 for calculator
                if (arrFieldRow["sFieldTitle"] == "厂家指导价" && field != "无" && field != "待查") {
                    arrTemp.push("<b>" + field + "</b>");
                    arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + ComparePageObject.AllCarJson[i][0][0] + "\"  target=\"_blank\"></a>");
                    //arrTemp.push("<a class=\"right\" target=\"_blank\" href=\"http://gouche.yiche.com/" + bit_locationInfo["engName"] + "/sb" + ComparePageObject.AllCarJson[i][0][3] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/?leads_source=p046002\">找低价&gt;</a>");
                }
                else if (arrFieldRow["sFieldTitle"] == "车身颜色") {
                    if (field != "&nbsp;") {
                        var colorArr = field.split('|');
                        arrTemp.push("<ul class=\"bodycolor\">");
                        var tempcolorRow = colorArr.length % 5 == 0 ? colorArr.length / 5 : (parseInt(colorArr.length / 5) + 1);
                        if (tempcolorRow > colorRow) {
                            colorRow = tempcolorRow;
                        }
                        for (var c = 0; c < colorArr.length; c++) {
                            if (colorArr[c] == "") continue;
                            color = colorArr[c].split(',');
                            arrTemp.push("<li><a href=\"javascript:void(0);\" style=\"cursor:default;background-color:" + color[1] + "\" title=\"" + color[0] + "\"></a></li>");
                        }
                        arrTemp.push("</ul>");
                    } else { arrTemp.push(field); }
                }
                else if (arrFieldRow["sFieldTitle"] == "压缩比" && field != "&nbsp;" && field != "") {
                    arrTemp.push(field + ":1");
                }
                else if (checkTitleIsThreeLines(arrFieldRow["sFieldTitle"])) {
                    arrTemp.push("<span class=\"tdThreeLines\" title=\"" + field + "\">" + field + "</span>");
                }
                else if (checkTitleIsTwoLines(arrFieldRow["sFieldTitle"])) {
                    arrTemp.push("<span class=\"tdTwoLines\" title=\"" + field + "\">" + field + "</span>");
                }
                else {
                    //arrTemp.push(field);
                    if (!chkResult.IsSame && ComparePageObject.IsVantage) {
                        //只有一个值不标示优势项 -1代表一个值
                        var tempFlag = false;
                        if (!isNaN(chkResult.CurrParamsValue) && !isNaN(field)) {
                            tempFlag = (parseFloat(chkResult.CurrParamsValue) == parseFloat(field));
                        } else {
                            tempFlag = (chkResult.CurrParamsValue == field);
                        }
                        if (chkResult.CurrCarIndex != -1 && tempFlag && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1")
                            arrTemp.push("<strong class=\"best\">" + field + "</strong>");
                        else
                            arrTemp.push(field);
                    }
                    else
                        arrTemp.push(field);
                }
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
    if (!isAllunknown) {
        // Is Need Show The Bar
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;
            //添加左侧菜单
            ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
            ComparePageObject.ArrTempLeftNavHTML.length = 0;
        }
        if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
            ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
            ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
        }
    }
    if (firstSame && ComparePageObject.IsDelSame && isShowLoop > 1) {
        return;
    }
    else {
        if (!isAllunknown) {
            ComparePageObject.ArrPageContent.push("<tr" + (arrFieldRow["sFieldTitle"] == "车身颜色" ? " class=\"p-color" + (colorRow <= 1 ? "" : (" h" + parseInt(colorRow) * 30)) + "\"" : (arrFieldRow["sFieldTitle"] == "音响品牌" ? " class=\"h36\"" : "")) + ">");
            if (checkTitleIsThreeLines(arrFieldRow["sFieldTitle"])) {
                ComparePageObject.ArrPageContent.push("<th class=\"threeLines\">");
                ComparePageObject.FloatLeft.push("<tr" + (colorRow <= 1 ? "" : (" class=\"h" + parseInt(colorRow) * 30 + "\"")) + "><th class=\"threeLines\">" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");
            }
            else {
                ComparePageObject.ArrPageContent.push("<th>");
                ComparePageObject.FloatLeft.push("<tr " + (arrFieldRow["sFieldTitle"] == "音响品牌" ? " class=\"h36\"" : "") + "><th>" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");
            }
            ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow));
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

//判断参数值是否相同
function IsFieldSame(arrFieldRow) {
    var tempArray = new Array();
    var paramArray = arrFieldRow["sFieldIndex"].split(',');
    var unitArray = arrFieldRow["unit"].split(',');
    var joinCodeArray = arrFieldRow["joinCode"].split(',');
    var pidArray = arrFieldRow["sPid"].split(',');
    var prefixArray = arrFieldRow["sTrPrefix"].split(',');

    var result = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 }, tempField = null, allCarFieldArr = [];
    try {
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (!ComparePageObject.AllCarJson[i]) continue;
            var multiField = "";
            for (var pint = 0; pint < paramArray.length; pint++) {
                // loop every param
                var sTrPrefix = arrFieldRow["sTrPrefix"];
                var index = paramArray[pint];
                if (ComparePageObject.AllCarJson[i].length <= prefixArray[pint])
                { return; }
                if (ComparePageObject.AllCarJson[i][prefixArray[pint]].length <= index)
                { return; }
                var field = ComparePageObject.AllCarJson[i][prefixArray[pint]][index] || "";
                multiField = multiField + (joinCodeArray[pint] || "") + field;
            }
            //判断是否是数字 解决 8.2、8.20 现象
            if (multiField != "" && tempField != "" && !isNaN(multiField) && !isNaN(tempField)) {
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
    if ((ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1") || ComparePageObject.IsShowDiff)
        chkResult = IsFieldSame(arrFieldRow);

    // loop every car
    var tempArrMultField = [];
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        if (ComparePageObject.AllCarJson[i]) {
            if (!chkResult.IsSame)
                arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
            else
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
                    //档位数 无极变速 不显示
                    if (pidArray[pint] == "724") {
                        if (isNaN(field) || parseInt(field) <= 0)
                            continue;
                    }
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
                            field += unitArray[pint] || "";
                        }
                        multiField = (multiField.length > 0 ? (multiField + (joinCodeArray[pint] || "")) : "") + field;
                        //add by sk 2016.01.08 以下参数有值 直接显示 忽略第二个参数
                        if (pidArray[pint] == "509" || pidArray[pint] == "489" || pidArray[pint] == "555" || pidArray[pint] == "808") {
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
                    { multiField = "<span class=\"songti\">●</span>"; }
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
                                if (!chkResult.IsSame)
                                    arrTemp.push("<td name=\"td" + m + "\" class=\"bg-blue\">");
                                else
                                    arrTemp.push("<td name=\"td" + m + "\" >");
                                arrTemp.push(tempArrMultField[m]);
                                arrTemp.push("</td>");
                            }
                        }
                    }
                } else {
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
                            arrTemp.push("<strong class=\"best\">" + multiField + "</strong>");
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
    if (!isAllunknown) {
        // Is Need Show The Bar
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;
            //添加左侧菜单
            ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
            ComparePageObject.ArrTempLeftNavHTML.length = 0;
        }
        if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
            ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
            ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
        }
    }
    if (firstSame && ComparePageObject.IsDelSame) {
        return;
    }

    else {
        if (!isAllunknown) {
            ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");
            ComparePageObject.ArrPageContent.push("<tr>");
            ComparePageObject.ArrPageContent.push("<th>");
            ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow));
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
        //if (checkCarIsShowForeach(i)) {
            num++;
            if (!ComparePageObject.AllCarJson[i]) {
                arrTemp.push("<td>&nbsp;</td>");
            }
            else {
                if ((ComparePageObject.CurrentCarID == ComparePageObject.AllCarJson[i].CarID) && ComparePageObject.IsNeedFirstColor) {//车款配置页
                    arrTemp.push("<td name=\"td" + i + "\" class=\"f\">");
                }
                else if (!chkResult.IsSame) {
                    arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
                }
                else {
                    arrTemp.push("<td name=\"td" + i + "\">");
                }
                var field = ComparePageObject.AllCarJson[i][arrFieldRow["sTrPrefix"]][arrFieldRow["sFieldIndex"]];
                if (field != "") isAllunknown = false;
                var fieldValue = field.split(',');
                var standardJson = [],
                    optionalJson = [],
                    standardStrLength = 0;
                for (var fieldIndex = 0; fieldIndex < fieldValue.length; fieldIndex++) {
                    var fieldOptional = fieldValue[fieldIndex].split('|');
                    if (fieldOptional.length > 1) {
                        optionalJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\",\"price\":" + fieldOptional[1] + "}"));
                    }
                    else {
                        standardJson.push(JSON.parse("{\"text\":\"" + fieldOptional[0] + "\"}"));
                        standardStrLength += fieldOptional[0].length;
                    }
                }

                if (standardJson.length > 0) {
                    for (var staIndex = 0; staIndex < standardJson.length; staIndex++) {
                        if (standardJson[0].text == "无") {
                            if (standardJson.length == 1 && optionalJson.length == 0) {
                                arrTemp.push("<div class=\"optional type2 std\">");
                                arrTemp.push("<div class=\"l\"><i>-</i>&nbsp;</div>");
                                arrTemp.push("</div>");
                                break;
                            }
                            else {
                                continue;
                            }
                        }
                        arrTemp.push("<div class=\"optional type2 std\">");
                        if (standardJson[0].text == "有") {
                            arrTemp.push("<div class=\"l\"><i>●</i>&nbsp;</div>");
                        }
                        else {
                            arrTemp.push("<div class=\"l\"><i>●</i>" + (standardJson[staIndex].text.length > 0 ? standardJson[staIndex].text : "&nbsp;") + "</div>");
                        }
                        arrTemp.push("</div>");
                    }
                }
                if (optionalJson.length > 0) {
                    for (var optIndex = 0; optIndex < optionalJson.length; optIndex++) {
                        arrTemp.push("<div class=\"optional type2\">");
                        arrTemp.push("<div class=\"l\"><i>○</i>" + optionalJson[optIndex].text + "</div>");
                        arrTemp.push("<div class=\"r\">" + formatCurrency(optionalJson[optIndex].price) + "元</div>");
                        arrTemp.push("</div>");
                    }
                }
            }
        //}
    }
    if (!isAllunknown) {
        if (ComparePageObject.ArrTempBarHTML.length > 0) {
            ComparePageObject.ArrPageContent.push(ComparePageObject.ArrTempBarHTML.join(""));
            ComparePageObject.ArrTempBarHTML.length = 0;
            //添加左侧菜单
            ComparePageObject.ArrLeftNavHTML.push(ComparePageObject.ArrTempLeftNavHTML.join(''));
            ComparePageObject.ArrTempLeftNavHTML.length = 0;
        }
        if (ComparePageObject.ArrTempBarForFloatLeftHTML.length > 0) {
            ComparePageObject.FloatLeft.push(ComparePageObject.ArrTempBarForFloatLeftHTML.join(""));
            ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
        }
        ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
        ComparePageObject.ArrPageContent.push("<th>");
        ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow));
        ComparePageObject.ArrPageContent.push("</th>");
        ComparePageObject.ArrPageContent.push(arrTemp.join(""));
        ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");
    }
    else {
        return;
    }
    //when less 对比项小于5个时，填补对比项
    //if (num < 5) {
    //    for (var kk = 0; kk < 5 - num; kk++) {
    //        ComparePageObject.ArrPageContent.push("<td>&nbsp;");
    //        ComparePageObject.ArrPageContent.push("</td>");
    //    }
    //}
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
    ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");
    ComparePageObject.ArrPageContent.push("<tr>");
    ComparePageObject.ArrPageContent.push("<th>" + checkBaikeForTitle(arrFieldRow) + "</th>");
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
                if (arrFieldRow["sFieldTitle"] == "商家报价") {
                    var fieldTemp = field,
						 arrTemp = field.split('-');
                    if (arrTemp.length > 1)
                        fieldTemp = arrTemp[0].replace("万", "") + "-" + arrTemp[1];
                    ComparePageObject.ArrPageContent.push("<div class=\"xunjia\">");
                    ComparePageObject.ArrPageContent.push("<span class=\"cRed\"><a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.AllCarJson[i][0][0] + "&citycode=0\">" + fieldTemp + "</a></span>");
                    ComparePageObject.ArrPageContent.push("<div class=\"button_gray button_43_20\"><a target=\"_blank\" href=\"http://dealer.bitauto.com/zuidijia/nb" + (ComparePageObject.AllCarJson[i][0][3] || "") + "/nc" + ComparePageObject.AllCarJson[i][0][0] + "/?leads_source=p046001\">询价</a></div>");
                    ComparePageObject.ArrPageContent.push("</div>");
                }
                else if (arrFieldRow["sFieldTitle"] == "降价优惠") {
                    var csAllSpell = ComparePageObject.AllCarJson[i][0][6] || "";
                    ComparePageObject.ArrPageContent.push("<span class=\"tdJiangjia\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + csAllSpell + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/jiangjia/\">" + field + "</a></span>");
                }
                else { }
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
    ComparePageObject.FloatLeft.push("<tr class=\"car-focus-index-content h60\"><th class=\"threeLines\">" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : checkBaikeForTitle(arrFieldRow)) + "</th></tr>");
    // more link
    ComparePageObject.ArrPageContent.push("<tr class=\"car-focus-index-content h60\">");
    ComparePageObject.ArrPageContent.push("<th class=\"threeLines\">" + (arrFieldRow["sFieldTitle"] == "" ? "&nbsp;" : checkBaikeForTitle(arrFieldRow)) + "</th>");
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
                var t2 = t1.substring(0, 16);
                var tempShowName = t2.replace(/([\u0391-\uffe5])a/ig, '$1');
                if (tempShowName == csShowName)
                { tempShowName += ""; }
                else
                { tempShowName += "…"; }
                // csShowName = csShowName.substring(0, 10).replace(/([\u0391-\uffe5])/ig, '$1a').substring(0, c).replace(/([\u0391-\uffe5])a/ig, '$1');
                var csName = ComparePageObject.AllCarJson[i][0][4] || "";
                ComparePageObject.ArrPageContent.push("<p class=\"frist\"><a target=\"_blank\" href=\"/" + csAllSpell + "/koubei/\">" + tempShowName + "口碑</a></p>");
                ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"/" + csAllSpell + "/youhao/\">" + tempShowName + "油耗</a></p>");
                ComparePageObject.ArrPageContent.push("<p class=\"last\"><a target=\"_blank\" href=\"" + csBBS + "\">" + tempShowName + "论坛</a></p>");
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
                            ComparePageObject.ArrPageContent.push("<span class=\"value\">第<em>" + uvRank + "</em>名</span>");
                            ComparePageObject.ArrPageContent.push("</p>");
                        }
                        else { ComparePageObject.ArrPageContent.push("<span class=\"cGray\">暂无关注数据</span>"); }
                        if (saleRank > 0) {
                            ComparePageObject.ArrPageContent.push("<p><a target=\"_blank\" href=\"http://index.bitauto.com/xiaoliang/s" + csid + "/\">" + csLevel + "销量</a>");
                            ComparePageObject.ArrPageContent.push("<span class=\"value\">第<em>" + saleRank + "</em>名</span>");
                            ComparePageObject.ArrPageContent.push("</p>");
                        }
                        else { ComparePageObject.ArrPageContent.push("<span class=\"cGray\">暂无销量数据</span>"); }
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
function checkBaikeForTitle(arrFieldRow) {
    var title, unit;
    title = arrFieldRow["sFieldTitle"];
    unit = arrFieldRow["unit"];
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
        return "<a href='" + ComparePageObject.BaikeObj[title] + "' target='_blank'>" + title + "<em>" + (unit != "" && unit.indexOf(",") == -1 ? "(" + unit + ")" : "") + "</em></a>";
    }
    else { return title + "<em>" + (unit != "" && unit.indexOf(",") == -1 ? "(" + unit + ")" : "") + "</em>"; }
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
    if (id <= 0) return;
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
    //newCarID = getNewArrCarId(newCarID);
    //删除添加车款后再添加相同车款 页面不刷新
    if (window.location.search == "?carIDs=" + newCarID.join(",") + "") {
        //document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "&t=" + Math.random() + "#CarHotCompareList";
        location.reload();
    }
    else {
        var paramsArr = [];
        if (ComparePageObject.IsOperateTheSame)
            paramsArr.push("diff");
        if (ComparePageObject.IsShowDiff)
            paramsArr.push("showdiff");
        if (ComparePageObject.IsVantage)
            paramsArr.push("vantage");

        document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "#CarHotCompareList" + (paramsArr.length > 0 ? "&" + paramsArr.join("&") : "") + "";
    }
}

function showHotCompare(index) {
    $(".firstCar_pop").show();
    CookieForTempSave.clearCookie("UserCloseCarList");
    ComparePageObject.IsCloseHotList = false;
    // $("#pkButton_" + index).hide();
    ajaxOtherCarList(index);
}

// del car from compare
function delCarToCompare(index, e) {
    var newCarIDArr = new Array();
    if (ComparePageObject.ValidCount < 1) {
        alert('没有可删的了');
        return;
    }

    e = e || window.event;
    var target = e.srcElement || e.target;
    if (index >= 0 && ComparePageObject.ValidCount > index && ComparePageObject.AllCarJson.length > index) {
        //		//$("td[name='td" + index + "']").html("");
        //		//删除效果
        //		$(target).closest(".tableHead_item").animate({ width: 0 }, 500, function () {
        //			// del Array index object
        //			ComparePageObject.AllCarJson.splice(index, 1);
        //			if (ComparePageObject.AllCarJson.length == 1) {
        //				ComparePageObject.IsOperateTheSame = false;
        //			}
        //			initPageForCompare();
        //		});
        if (ComparePageObject.IsIE) {
            // del Array index object
            ComparePageObject.AllCarJson.splice(index, 1);
            if (ComparePageObject.AllCarJson.length == 1) {
                ComparePageObject.IsOperateTheSame = false;
            }
            if (ComparePageObject.IE6) { $(document).scrollLeft(1); }
            initPageForCompare();
        }
        else {
            $("td[name='td" + index + "'][id!='td_" + index + "']").html("");
            //删除效果
            $(target).closest(".tableHead_item").fadeOut(300, function () {
                // del Array index object
                ComparePageObject.AllCarJson.splice(index, 1);
                if (ComparePageObject.AllCarJson.length == 1) {
                    ComparePageObject.IsOperateTheSame = false;
                }
                initPageForCompare();
            });
        }

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

function advantageForCompare() {
    showLoading();
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
    gotoCheckPage();
    //$(window).scrollTop($(window).scrollTop() + 1);
}
//显示差异
function showDiffForCompare() {
    showLoading();
    if (ComparePageObject.ValidCount <= 1) return;
    //ComparePageObject.IsShowDiff = (ComparePageObject.IsShowDiff) ? true : false;
    if (ComparePageObject.ValidCount > 1) {
        if (ComparePageObject.IsShowDiff) {
            ComparePageObject.IsShowDiff = false;
            $("input[type='checkbox'][name='checkboxForDiff']").each(function (i, n) { this.checked = false; });
        } else {
            ComparePageObject.IsShowDiff = true;
            $("input[type='checkbox'][name='checkboxForDiff']").each(function (i, n) { this.checked = true; });
        }
        createPageForCompare(ComparePageObject.IsDelSame);
    }
    gotoCheckPage();
    //$(window).scrollTop($(window).scrollTop() + 1);
}

function showLoading() {
    $("#selectCarLoadingDiv").show();
    setTimeout(function () { $("#selectCarLoadingDiv").hide(); }, 300);
}
//获取多选框参数
function gotoCheckPage() {
    var paramsArr = [];
    if (ComparePageObject.IsOperateTheSame)
        paramsArr.push("diff");
    if (ComparePageObject.IsShowDiff)
        paramsArr.push("showdiff");
    if (ComparePageObject.IsVantage)
        paramsArr.push("vantage");
    //刷新页面
    if (ComparePageObject.AllCarJson.length > 0) {
        var tempArr = [];
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            tempArr.push(ComparePageObject.AllCarJson[i][0][0])
        }
        document.location.href = "/chexingduibi/?carIDs=" + tempArr.join(",") + "" + "#CarHotCompareList" + (paramsArr.length > 0 ? "&" + paramsArr.join("&") : "") + "&t";
    }
}
// 排除相同项
function delTheSameForCompare() {
    showLoading();
    if (ComparePageObject.ValidCount <= 1) return;
    //ComparePageObject.IsOperateTheSame = (!ComparePageObject.IsOperateTheSame) ? true : false;

    //js 控制
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
    gotoCheckPage();
    //$(window).scrollTop($(window).scrollTop() + 1);
}

function changeCheckBoxStateByName(name, state) {
    var checkBoxs = document.getElementsByName(name);
    if (checkBoxs && checkBoxs.length > 0) {
        for (var i = 0; i < checkBoxs.length; i++) {
            checkBoxs[i].checked = state;
        }
    }
}
//数组扩展方法
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.max = function () { return Math.max.apply({}, this) }
Array.prototype.min = function () { return Math.min.apply({}, this) }
Array.prototype.intersect = function (b) {
    var i = 0, result = [];
    while (i < this.length && i < b.length) {
        if (this.length > b.length && this.indexOf(b[i]) != -1) { result.push(b[i]); }
        else { if (b.indexOf(this[i]) != -1) { result.push(this[i]); } }
        i++;
    }
    return result;
}
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };

//绑定下拉选择框
function bindSelectForCompare() {
    var mdvalue = 0, sdvalue = 0;
    //	if (CookieForTempSave.getCookie("TempSelectMasterID") != "") {
    //		mdvalue = CookieForTempSave.getCookie("TempSelectMasterID");
    //		if (CookieForTempSave.getCookie("TempSelectSerialID") != "")
    //		{ sdvalue = CookieForTempSave.getCookie("TempSelectSerialID"); }
    //	}
    //modified by sk 2014.01.02 默认选择框根据选择车款最后一个
    if (ComparePageObject.ValidCount > 0) {
        var lastCarJson = ComparePageObject.AllCarJson[ComparePageObject.AllCarJson.length - 1],
		carId = lastCarJson[0][0];
        mdvalue = getMasterId(carId);
        sdvalue = lastCarJson[0][3];
    }
    //modified by sk 2013.03.27 广告
    //modified by sk for wk 2016.09.08
    //var serialId = 0;
    //if (typeof ComparePageObject.AllCarJson[0] != "undefined"
    //	&& typeof ComparePageObject.AllCarJson[0][0] != "undefined"
    //	&& ComparePageObject.AllCarJson[0][0][3] != "undefined") {
    //	serialId = ComparePageObject.AllCarJson[0][0][3];
    //}
    var serialId = sdvalue;
    if (ComparePageObject.ValidCount < 10
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
    ComparePageObject.BSelect = BitA.DropDownListNew({
        container: { master: "master4", serial: "serial4", cartype: "cartype4" },
        include: { serial: "1", cartype: "1" },
        dvalue: { master: mdvalue, serial: sdvalue },
        callback: { cartype: function (data) { setCarDisabled(); } },
        onchange: { cartype: function () { onchangeCarForSelect(); } }
    });
}

function onchangeCarForSelect(selectObj) {
    var selectBsID = ComparePageObject.BSelect.getValue("master");
    var selectCsID = ComparePageObject.BSelect.getValue("serial");
    //CookieForTempSave.setCookie("TempSelectMasterID", selectBsID);
    //CookieForTempSave.setCookie("TempSelectSerialID", selectCsID);
    addCarToCompareForSelect(ComparePageObject.BSelect.getValue("cartype"));
    fixWidthNormal(selectObj);
}
//根据车型获取主品牌ID
function getMasterId(carId) {
    var masterId = 0;
    if (allCarInfo != undefined) {
        for (var i = 0; i < allCarInfo.length; i++) {
            if (allCarInfo[i].Key == carId) {
                masterId = allCarInfo[i].Value.MasterId;
                break;
            }
        }
    }
    return masterId;
}
//换车
function setSelectControlForCompare(objID, suffix) {
    if (objID.indexOf('_') >= 0) {
        var index = objID.substring(objID.lastIndexOf('_') + 1, objID.length);
        var serialId = ComparePageObject.AllCarJson[index][0][3];
        var carId = ComparePageObject.AllCarJson[index][0][0];
        var masterId = getMasterId(carId);

        var btnChangeObj = $("#" + objID + " .change-car-area div[id^='change_car_']").eq(0);
        if (btnChangeObj.hasClass("btn-hide-car")) {
            $(btnChangeObj).removeClass("btn-hide-car");
            $("#" + objID + " .change-car-area .huanche-layer").hide();
            return;
        }
        $("#" + objID + " .change-car-area .huanche-layer").remove();
        $(btnChangeObj).addClass("btn-hide-car");
        $("#" + objID).parent().siblings().find(".huanche-layer").hide().parent().children("div[id^='change_car_']").removeClass("btn-hide-car");
        $("#" + objID).parent().siblings().find(".tuijian-layer").hide().parent().children("div:first").removeClass("btn-hide-car");

        var tempHmtl = [];
        //tempHmtl.push("<div class=\"popup-change-car " + (rightWidth < 435 ? "right" : "") + "\">");
        tempHmtl.push("<div class=\"drop-layer huanche-layer\">");
        tempHmtl.push("<div class=\"db-car-box\">");
        tempHmtl.push("<div id=\"MasterSelectList" + "_" + suffix + "_" + index + "\" class=\"brand-form no-second\"><span class=\"default activ\">请选择品牌</span><a href=\"#\" class=\"jt\"></a></div>");
        tempHmtl.push("<div id=\"SerialSelectList" + "_" + suffix + "_" + index + "\" class=\"brand-form no-second\"><span class=\"default\">请选择车型</span><a href=\"#\" class=\"jt\"></a></div>");
        tempHmtl.push("<div id=\"CarTypeSelectList" + "_" + suffix + "_" + index + "\" class=\"brand-form no-second\"><span class=\"default\">请选择车款</span><a href=\"#\" class=\"jt\"></a></div>");
        tempHmtl.push("</div>");
        tempHmtl.push("<div class=\"db-txt-list\"><div class=\"list-txt list-txt-s list-txt-default list-txt-style4\"><ul id=\"recom-car-list-" + objID + "\"></ul></div></div>");
        tempHmtl.push("</div>");

        $("#" + objID + " .change-car-area").append(tempHmtl.join(''));
        //$("#" + objID + " .change-car-area #change_car_" + index).attr("class", "btn-hide-car").html("收起");

        var currentSelect = ComparePageObject.SelectControlTemp;
        while (currentSelect.indexOf("_ParamSuffix") != -1) {
            currentSelect = currentSelect.replace("_ParamSuffix", "_" + suffix + "_" + index);
        }
        while (currentSelect.indexOf("_BsSuffix") != -1) {
            currentSelect = currentSelect.replace("_BsSuffix", "_" + suffix + "_" + index);
            currentSelect = currentSelect.replace("[masterId]", masterId);
        }
        while (currentSelect.indexOf("_CsSuffix") != -1) {
            currentSelect = currentSelect.replace("_CsSuffix", "_" + suffix + "_" + index);
            currentSelect = currentSelect.replace("[serialId]", serialId);
        }
        while (currentSelect.indexOf("_CarSuffix") != -1) {
            currentSelect = currentSelect.replace("_CarSuffix", "_" + suffix + "_" + index);
            currentSelect = currentSelect.replace("[carId]", carId);
        }
        while (currentSelect.indexOf("[index]") != -1) {
            currentSelect = currentSelect.replace("[index]", "[" + index + "]");
        }
        //alert(currentSelect);
        //setTimeout(function () { eval(currentSelect); }, 200);
        eval(currentSelect);
        //为我推荐
        ajaxOtherCarList("recom-car-list-" + objID + "", index, 4, false);
        if (ComparePageObject.IsShowFloatTop)
        { changeWhenFloatTop(); }
    }
}
//换车根据对比车款 params:车款ID，车款索引位置
function changeCarByIndex(carId, index) {
    var carIds = [];
    if (ComparePageObject.AllCarJson.length > 0) {
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            if (ComparePageObject.AllCarJson[i][0][0] == carId) {
                alert("您选择的车型,已经在对比列表中!");
                return;
            }
            else { carIds.push(ComparePageObject.AllCarJson[i][0][0]); }
        }
    }
    if (carIds.length > index) {
        for (var i = 0; i < carIds.length; i++) {
            if (i == index) {
                carIds[i] = carId;
                break;
            }
        }
    }
    var paramsArr = [];
    if (ComparePageObject.IsOperateTheSame)
        paramsArr.push("diff");
    if (ComparePageObject.IsShowDiff)
        paramsArr.push("showdiff");
    if (ComparePageObject.IsVantage)
        paramsArr.push("vantage");

    document.location.href = "/chexingduibi/?carIDs=" + carIds.join(",") + "#CarHotCompareList" + (paramsArr.length > 0 ? "&" + paramsArr.join("&") : "") + "";
}
//换车根据下拉选择框 params:车款索引位置
function onchangeCarForSelectByIndex(index) {
    var selectBsID = ComparePageObject.CreateSelectChange[index].getValue("master");
    var selectCsID = ComparePageObject.CreateSelectChange[index].getValue("serial");
    //CookieForTempSave.setCookie("TempSelectMasterID", selectBsID);
    //CookieForTempSave.setCookie("TempSelectSerialID", selectCsID);
    var newid = ComparePageObject.CreateSelectChange[index].getValue("cartype");
    var newCarID = new Array();
    if (ComparePageObject.AllCarJson.length > 0) {
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            if (ComparePageObject.AllCarJson[i][0][0] == newCarID) {
                alert("您选择的车型,已经在对比列表中!");
                return;
            }
            else { newCarID.push(ComparePageObject.AllCarJson[i][0][0]); }
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
    var paramsArr = [];
    if (ComparePageObject.IsOperateTheSame)
        paramsArr.push("diff");
    if (ComparePageObject.IsShowDiff)
        paramsArr.push("showdiff");
    if (ComparePageObject.IsVantage)
        paramsArr.push("vantage");

    document.location.href = "/chexingduibi/?carIDs=" + newCarID.join(",") + "#CarHotCompareList" + (paramsArr.length > 0 ? "&" + paramsArr.join("&") : "") + "";
}
//上一个车型对应的 为我推荐
function recomCarList(objId, prevIndex) {
    var carId = ComparePageObject.AllCarJson[prevIndex][0][0],
	fixWidth = 238;

    //当前展开层元素的居右宽度
    var rightWidth = $(document).width() - ($("#" + objId).offset().left + $("#" + objId).outerWidth(true));
    //获取上个元素zindex值
    var prevzIndex = parseInt($("#" + objId).parent().prev().css("z-index"));

    var btnRecomObj = $("#" + objId + " .recommend");
    //if (btnRecomObj.hasClass("btn-show-car")) {
    //	btnRecomObj.addClass("btn-hide-car");
    //	//向左显示 父级zindex设置最大
    //	if (rightWidth < fixWidth) $("#" + objId).parent().css({ 'z-index': 11 });
    //}
    //else 
    if (btnRecomObj.hasClass("btn-hide-car")) {
        btnRecomObj.removeClass("btn-hide-car");
        if (rightWidth < fixWidth) $("#" + objId).parent().css({ 'z-index': prevzIndex - 1 });
    }
    else {
        btnRecomObj.addClass("btn-hide-car");
        if (rightWidth < fixWidth) $("#" + objId).parent().css({ 'z-index': 11 });
    }

    $("#" + objId).parent().siblings().find(".drop-layer").hide().parent().children("div[id^='change_car_']").removeClass("btn-hide-car");
    var changeCarObj = $("#" + objId + " .recommend .tuijian-layer");
    if (changeCarObj.length > 0) {
        changeCarObj.toggle();
        return;
    }

    var tempHmtl = [];
    tempHmtl.push("<div class=\"drop-layer tuijian-layer\">");

    tempHmtl.push("<h6>大家都用它和谁比</h6>");
    tempHmtl.push("<div class=\"list-txt list-txt-s list-txt-default list-txt-style4\">");
    tempHmtl.push("<ul id=\"recom-car-list-nocar-" + objId + "\"></ul>");
    tempHmtl.push("<a href=\"javascript:addAllCarToCompare('" + objId + "'," + prevIndex + ");\" class=\"btn btn-secondary btn-sm\">全部加入对比</a>");
    tempHmtl.push("</div>");
    //tempHmtl.push("<a href=\"javascript:closeRecomCarList(" + prevIndex + ");\" class=\"btn-close-car\">关闭</a>");
    tempHmtl.push("</div>");
    $("#" + objId + " .recommend").append(tempHmtl.join(''));
    if (carId > 0) {
        ajaxOtherCarList("recom-car-list-nocar-" + objId + "", prevIndex, 5, true);
    }
}
//换车关闭
function closeChangeCarList(index) {
    $("#tableHead_" + index + ",#FloatTop_tableHead_" + index + "").find(".popup-change-car").hide().parent().find("span[id='change_car_" + index + "'],span[id='change_car_float_" + index + "']").attr("class", "btn-show-car");
}
//推荐关闭
function closeRecomCarList(prevIndex) {
    $("#tableHead_" + (prevIndex + 1) + ",#FloatTop_tableHead_" + (prevIndex + 1) + "").find(".recom-car .popup-recom").hide().parent().find("a:first").attr("class", "btn-show-car").text("为我推荐");
}
//添加所有推荐车型
function addAllCarToCompare(objId, index) {
    var tempArr = [], tempRecomArr = [];
    for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
        tempArr.push(ComparePageObject.AllCarJson[i][0][0])
    }
    $("#recom-car-list-nocar-" + objId).find('li').each(function (i, n) {
        tempRecomArr.push(this.id.replace("recomCarList_", ""));
    });
    if (tempRecomArr.length <= 0) return;
    tempArr = tempArr.concat(tempRecomArr);
    if (tempArr.length > 10)
    { alert("对比车型不能多于10个"); }
    else if (tempArr.length > 0) {
        // 推荐类不统计
        if (window.location.search == "?carIDs=" + tempArr.join(",") + "&isrec=1") {
            location.reload();
        } else
            document.location.href = "/chexingduibi/?carIDs=" + tempArr.join(",") + "&isrec=1#CarHotCompareList";
    }
}
//请求推荐车型
function ajaxOtherCarList(id, index, top, isAdd) {
    if (ComparePageObject.AllCarJson.length > 0 && ComparePageObject.AllCarJson.length > index) {
        if (ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]]) {
            //存储请求数据
            var content = getCompareCarListHtml(index, ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]], top, isAdd);
            $("#" + id).html(content);
            //$("#recom-car-list-" + index).html(content);
            return;
        }
        $.ajax({
            url: ComparePageObject.OtherCarInterface + ComparePageObject.AllCarJson[index][0][0],
            cache: true,
            dataType: 'jsonp',
            jsonpCallback: "callback",
            success: function (data) {
                ComparePageObject.LastFirstInterface[ComparePageObject.AllCarJson[index][0][0]] = data;
                var content = getCompareCarListHtml(index, data, top, isAdd);
                $("#" + id).html(content);
                //$("#recom-car-list-" + index).html(content);
            }
        });
    }
}
//车款是否存在已选车款
function IsExistCarId(carId) {
    var result = false;
    if (ComparePageObject.AllCarJson.length > 0) {
        for (var i = 0; i < ComparePageObject.AllCarJson.length; i++) {
            if (carId == ComparePageObject.AllCarJson[i][0][0]) {
                result = true; break;
            }
        }
    }
    return result;
}
//大家跟谁比较
function getCompareCarListHtml(index, data, top, isAdd) {
    var carListHtml = [], loop = 0;
    if (top && top <= 0) top = 4;
    for (var i = 0; i < data.length; i++) {
        var carId = data[i].carId;
        var exist = IsExistCarId(carId);
        if (exist) { continue; }
        loop++;
        if (loop > top) break;
        var year = data[i].carYearType;
        if (year != 0) {
            year = " " + year + "款"
        }
        else {
            year = "";
        }
        if (isAdd) {
            carListHtml.push("<li id=\"recomCarList_" + data[i].carId + "\"><div class=\"txt\"><a href=\"javascript:addCarToCompareForSelect(" + data[i].carId + ",'" + data[i].carName + "','" + data[i].serialName + "');\"><strong>" + data[i].serialName + "</strong><span>" + year + " " + data[i].carName + "</span></a> <a class=\"hc-link\" href=\"javascript:addCarToCompareForSelect(" + data[i].carId + ",'" + data[i].carName + "','" + data[i].serialName + "');\">+对比</div></a></li>");
        }
        else {
            carListHtml.push("<li id=\"recomCarList_" + data[i].carId + "\"><div class=\"txt\"><a href=\"javascript:changeCarByIndex('" + data[i].carId + "'," + index + ");\"><strong>" + data[i].serialName + "</strong><span>" + year + " " + data[i].carName + "</span></a> <a href=\"javascript:changeCarByIndex('" + data[i].carId + "'," + index + ");\" class=\"hc-link\">换车</a></div></li>");
        }

        //if (isAdd)
        //	carListHtml.push("<li id=\"recomCarList_" + data[i].carId + "\"><a href=\"javascript:addCarToCompareForSelect(" + data[i].carId + ",'" + data[i].carName + "','" + data[i].serialName + "');\"><strong>" + data[i].serialName + "</strong><span>" + year + " " + data[i].carName + "</span> +对比</a></li>");
        //else
        //	carListHtml.push("<li id=\"recomCarList_" + data[i].carId + "\"><a href=\"javascript:changeCarByIndex('" + data[i].carId + "'," + index + ");\"><strong>" + data[i].serialName + "</strong><span>" + year + " " + data[i].carName + "</span> 换车</a></li>");
    }
    return carListHtml.join('');
}

// set img drag
function setImgDrag() {
    $("img[name='imgdrag']").each(function (i) {
        $(this).parent().parent().draggable({
            proxy: 'clone',
            revert: true,
            cursor: 'move',
            handle: "img",
            onStartDrag: function () {
                $(this).draggable('proxy').css({ 'z-index': '999' });
                $(this).closest("td").css({ 'z-index': '999' });
                // $(this).draggable('options').cursor = 'move';
                ComparePageObject.DragID = "";
                if (typeof (this.id) != "undefined" && this.id) {
                    ComparePageObject.DragID = this.id.replace("draggcarbox_", "");
                }
            },
            onStopDrag: function () {
                $(this).css({ 'left': '0px', 'top': '0px' });
                //$(this).draggable('proxy').css({ 'z-index': '1' });
                $(this).closest("td").css({ 'z-index': '1' });

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

    $(".sel-car-box").droppable({
        onDragEnter: function (e, source) {
            $(this).addClass('moving');
        },
        onDragLeave: function (e, source) {
            $(this).removeClass('moving');
        },
        onDrop: function (e, source) {
            $(source).draggable('proxy').remove();
            $(this).removeClass('moving');
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
    if (ComparePageObject.IE6) {
        $(selectObj).removeClass("car_select_w");
        $(selectObj).addClass("car_select_auto");
    }
}

function fixWidthNormal(selectObj) {
    if (ComparePageObject.IE6) {
        $(selectObj).removeClass("car_select_auto");
        $(selectObj).addClass("car_select_w");
    }
}

function intiSelectControl() {
    var tempLate = [];
    //2013-12-23 替换下拉列表调用方式 by sk
    tempLate.push('ComparePageObject.CreateSelectChange[index] = BitA.DropDownListNew({');
    tempLate.push('	container: { master: "MasterSelectList_BsSuffix", serial: "SerialSelectList_CsSuffix", cartype: "CarTypeSelectList_CarSuffix" },');
    tempLate.push('	include: { serial: "1", cartype: "1" },');
    tempLate.push('	dvalue: { master: [masterId], serial: [serialId], cartype: [carId] },');
    tempLate.push('	callback: { cartype: function (data) { setCarDisabled(); } },');
    tempLate.push('	onchange: { cartype: function(){onchangeCarForSelectByIndex([index]);} }');
    tempLate.push('});');
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
    $("#CarCompareContent tr:not([id^='params-']):gt(0)").hover(
		function () {
		    $(this).addClass("bg-gray");
		    var xjObj = $(this).find("div.xunjia");
		    if (xjObj.length > 0) {
		        xjObj.find("div.button_43_20").removeClass("button_gray").addClass("button_orange");
		    }
		},
		function () {
		    $(this).removeClass("bg-gray");
		    var xjObj = $(this).find("div.xunjia");
		    if (xjObj.length > 0) {
		        xjObj.find("div.button_43_20").removeClass("button_orange").addClass("button_gray");
		    }
		}
	);
}

function mathLeftMenuForResize(currScrollLeft, boxOffsetLeft, currScrollTop, boxOffsetTop) {
    if (ComparePageObject.ValidCount <= 0) return;
    if (boxOffsetLeft < 100 && boxOffsetLeft > 0) {
        $("#show-left-nav").show();
        $("#left-nav").hide();
    } else if (boxOffsetLeft == 0) {
        $("#left-nav").hide();
    } else {
        $("#show-left-nav").hide();
        if (currScrollTop > boxOffsetTop) {
            $("#left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: (boxOffsetLeft - currScrollLeft - ComparePageObject.MenuOffsetLeft) });
        }
        $("#left-nav").show();
    }
}
//左侧菜单操作
//参数：（当前左侧滚动距离，居左距离，当前距顶距离，距顶距离）
function mathLeftMenu(currScrollLeft, boxOffsetLeft, currScrollTop, boxOffsetTop) {
    //对比车型大于1时，操作左侧滚动菜单
    if (ComparePageObject.ValidCount <= 0 || $("#left-nav ul li").length == 0) {
        $("#show-left-nav").hide();
        $("#left-nav").hide();
        return;
    }
    if (currScrollLeft > boxOffsetLeft) {
        if (!ComparePageObject.OneLeftScrollFlag) {
            $("#show-left-nav").show();
            $("#left-nav").hide();
        }
        else
            $("#show-left-nav").hide();
        if (currScrollTop > boxOffsetTop) {
            $("#left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: "0px" });
            $("#show-left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: "0px" });
        } else {
            $("#left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: currScrollLeft - boxOffsetLeft });
            $("#show-left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: (currScrollLeft - boxOffsetLeft) });
        }
    }
    else {
        var fixedOffsetLeft = boxOffsetLeft - currScrollLeft - ComparePageObject.MenuOffsetLeft;
        if (currScrollLeft == 0 && boxOffsetLeft > ComparePageObject.MenuOffsetLeft)	//左侧滚动0，菜单显示
        {
            if ($("#left-nav ul li").length > 0) {
                $("#left-nav").show();
                $("#close-left-nav").hide();
            }
            //左侧菜单 显示标示
            ComparePageObject.OneLeftScrollFlag = false;
        }
        else {
            $("#left-nav").show();
            $("#close-left-nav").hide();
        }
        if (currScrollTop > boxOffsetTop) {
            $("#left-nav").css({ position: "fixed", top: $("#topfixed").height() + "px", left: fixedOffsetLeft + "px" });
            $("#show-left-nav").css({ position: "fixed", top: $("#topfixed").height() + "px", left: fixedOffsetLeft + "px" });
        } else {
            if ($("#left-nav ul li").length > 0) {
                if (currScrollTop > boxOffsetTop) {
                    if (currScrollTop > boxOffsetTop) {
                        $("#left-nav").css({ position: "fixed", top: $("#topfixed").height() + "px", left: "0px" });
                        $("#show-left-nav").css({ position: "fixed", top: $("#topfixed").height() + "px", left: (fixedOffsetLeft) + "px" });
                    } else {
                        $("#left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: "0px" });
                        $("#show-left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: fixedOffsetLeft + "px" });
                    }
                } else {
                    $("#left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: (-1* ComparePageObject.MenuOffsetLeft) + "px" });
                    $("#show-left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: (-1* ComparePageObject.MenuOffsetLeft) + "px" });
                }

            }
        }
    }
}
//滚动监听 滚动完成后回调函数
function scrollCallback() {
    var scrollsLeft = $(window).scrollLeft(); //窗口左卷动值
    var boxoffsetLeft = $("#topBox").offset().left; //计算box的offleft值
    if (scrollsLeft > boxoffsetLeft) {
        $("#left-nav").fadeOut(1000, function () {
            $("#show-left-nav").show();
            ComparePageObject.OneLeftScrollFlag = false;
        });
    }
}

$(function () {
    //	//回顶按钮
    //	$(".left-nav .duibi-return-top").click(function () {
    //		$("html").animate({ scrollTop: 0 }, 300); //IE,FF
    //		$("body").animate({ scrollTop: 0 }, 300); //Webkit
    //	});

    var theid = $("#topfixed");
    var the_lid = $("#leftfixed");
    var thebox = $("#topBox");
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
        //左侧菜单操作
        mathLeftMenu($(this).scrollLeft(), thebox.offset().left, $(this).scrollTop(), idmainoffsettop_top);

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
                thesmall.css({ position: "fixed", left: 0, top: 0, display: "block" });
            } else {//IE
                thesmall.css({ position: "absolute", top: scrolls, left: scrollsLeft, display: "block" });
            }
        } else {
            thesmall.css({ display: "none" });
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
            the_lid.css({ display: "none" });
        }
        //左侧菜单操作
        //window.setTimeout(function () { mathLeftMenu(scrollsLeft, boxoffsetLeft, scrolls, idmainoffsettop_top); }, 200);
        mathLeftMenu(scrollsLeft, boxoffsetLeft, scrolls, idmainoffsettop_top);
        ////////////left浮动模式结束///////////////////
        ////////////////控制上下卷动屏幕，出现浮动效果
        if (scrolls > idmainoffsettop_top && ComparePageObject.ValidCount > 0) {//如果向上滚动大于id的top位置
            floatkey = true; //开启浮动模式
            if (!ComparePageObject.IsShowFloatTop) {
                ComparePageObject.IsShowFloatTop = true;
                //ComparePageObject.MenuOffsetTop = 230;
                changeWhenFloatTop();
            }
            if (window.XMLHttpRequest) {//非IE6						 	
                theid.css({ position: "fixed", top: "0", left: boxoffsetLeft, display: "block" });
            } else {//IE6				 
                theid.css({ position: "absolute", top: scrolls, left: boxoffsetLeft, display: "block" });
            }
        }
        else if (scrolls <= idmainoffsettop_top) {//如果向上滚动小于id的top位置
            floatkey = false; //关闭浮动模式。
            if (ComparePageObject.IsShowFloatTop) {
                ComparePageObject.IsShowFloatTop = false;
                //ComparePageObject.MenuOffsetTop = 260;
                changeWhenFloatTop();
            }
            theid.css({ position: "relative", left: "0", top: 0, display: "none" });

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
        //设置右侧广告的位置
        InitRightFixedAdPosition();
    });
    ///////////////////////屏幕卷动结束

    //mathLeftMenu($(window).scrollLeft(), thebox.offset().left, $(window).scrollTop(), idmainoffsettop_top);
    //设置右侧广告的位置
    InitRightFixedAdPosition();

});

// page method --------------------------
var arrField = [
    { sFieldTitle: "图片", sType: "fieldPic", sPid: "", sFieldIndex: "", sTrPrefix: "1", unit: "", joinCode: "" },
    { sFieldTitle: "基本信息", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carinfo" },
    { sFieldTitle: "厂商指导价", sType: "fieldPara", sPid: "", sTrPrefix: "1", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "商家报价", sType: "fieldPrice", sPid: "", sTrPrefix: "1", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", sType: "fieldPara", sPid: "398", sTrPrefix: "1", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "排量", sType: "fieldPara", sPid: "785", sTrPrefix: "1", sFieldIndex: "4", unit: "L", joinCode: "" },
    { sFieldTitle: "进气形式", sType: "fieldPara", sPid: "425", sTrPrefix: "1", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", sType: "fieldPara", sPid: "1007", sTrPrefix: "1", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "燃油变速箱", sType: "fieldMulti", sPid: "724,712", sTrPrefix: "1,1", sFieldIndex: "7,8", unit: "挡,", joinCode: ", " },
    { sFieldTitle: "最高车速[km/h]", sType: "fieldPara", sPid: "663", sTrPrefix: "1", sFieldIndex: "9", unit: "", joinCode: "", isVantage: "1", size: "1" },

    { sFieldTitle: "车身尺寸", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carbody" },
    { sFieldTitle: "长×宽×高[mm]", sType: "fieldMulti", sPid: "588,593,586", sTrPrefix: "2,2,2", sFieldIndex: "0,1,2", unit: "", joinCode: ",x,x", size: "1" },
    { sFieldTitle: "轴距[mm]", sType: "fieldPara", sPid: "592", sTrPrefix: "2", sFieldIndex: "3", unit: "", joinCode: "mm", isVantage: "1", size: "1" },
    { sFieldTitle: "整备质量[kg]", sType: "fieldPara", sPid: "669", sTrPrefix: "2", sFieldIndex: "4", unit: "", joinCode: "kg" },
    { sFieldTitle: "座位数[个]", sType: "fieldPara", sPid: "665", sTrPrefix: "2", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "行李箱容积[L]", sType: "fieldMulti", sPid: "465,466,1000", sTrPrefix: "2,2,2", sFieldIndex: "6,19,20", unit: "", joinCode: ",x,x", size: "1" },
    { sFieldTitle: "油箱容积[L]", sType: "fieldPara", sPid: "576", sTrPrefix: "2", sFieldIndex: "7", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "前轮胎规格", sType: "fieldPara", sPid: "729", sTrPrefix: "2", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后轮胎规格", sType: "fieldPara", sPid: "721", sTrPrefix: "2", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "备胎", sType: "fieldPara", sPid: "707", sTrPrefix: "2", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", sType: "fieldPara", sPid: "398", sTrPrefix: "2", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "满载质量[kg]", sType: "fieldPara", sPid: "668", sTrPrefix: "2", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎规格", sType: "fieldPara", sPid: "1001", sTrPrefix: "2", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "载重质量[kg]", sType: "fieldPara", sPid: "974", sTrPrefix: "2", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎个数", sType: "fieldPara", sPid: "982", sTrPrefix: "2", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "货箱长×宽×高[mm]", sType: "fieldMulti", sPid: "966,969,970", sTrPrefix: "2,2,2", sFieldIndex: "16,17,18", unit: "", joinCode: ",x,x", isVantage: "1", size: "1" },
    { sFieldTitle: "车身颜色", sType: "fieldPara", sPid: "598", sTrPrefix: "0", sFieldIndex: "13", unit: "", joinCode: "" },

    { sFieldTitle: "动力系统", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carengine" },
    { sFieldTitle: "排气量[ml]", sType: "fieldMulti", sPid: "423,785", sTrPrefix: "3,3", sFieldIndex: "0,1", unit: ",L", joinCode: ",ml " }, /*1987ml 2.0L*/
    { sFieldTitle: "最大功率[kW]", sType: "fieldPara", sPid: "430", sTrPrefix: "3", sFieldIndex: "2", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "马力(Ps)", sType: "fieldPara", sPid: "791", sTrPrefix: "3", sFieldIndex: "3", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大功率转速(rpm)", sType: "fieldPara", sPid: "433", sTrPrefix: "3", sFieldIndex: "4", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大扭矩[N.m]", sType: "fieldPara", sPid: "429", sTrPrefix: "3", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "最大扭矩转速(rpm)", sType: "fieldPara", sPid: "432", sTrPrefix: "3", sFieldIndex: "6", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "缸体形式", sType: "fieldPara", sPid: "418", sTrPrefix: "3", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "气缸数[缸]", sType: "fieldPara", sPid: "417", sTrPrefix: "3", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "进气形式", sType: "fieldPara", sPid: "425", sTrPrefix: "3", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "供油方式", sType: "fieldPara", sPid: "580", sTrPrefix: "3", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "压缩比", sType: "fieldPara", sPid: "414", sTrPrefix: "3", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "燃油标号[号]", sType: "fieldPara", sPid: "577", sTrPrefix: "3", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "发动机启停", sType: "fieldPara", sPid: "894", sTrPrefix: "3", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "燃油变速箱类型", sType: "fieldPara", sPid: "712", sTrPrefix: "3", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "挡位个数", sType: "fieldPara", sPid: "724", sTrPrefix: "3", sFieldIndex: "15", unit: "", joinCode: "" },
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
    { sFieldTitle: "电池充电时间", sType: "fieldMulti", sPid: "878,879", sTrPrefix: "3,3", sFieldIndex: "28,27", unit: "小时,小时", joinCode: "快充, 慢充" },
    { sFieldTitle: "耗电量[kWh/100km]", sType: "fieldPara", sPid: "868", sTrPrefix: "3", sFieldIndex: "29", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "最大续航里程[km]", sType: "fieldPara", sPid: "883", sTrPrefix: "3", sFieldIndex: "30", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电池组质保", sType: "fieldPara", sPid: "1006", sTrPrefix: "3", sFieldIndex: "31", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", sType: "fieldPara", sPid: "1007", sTrPrefix: "3", sFieldIndex: "32", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合功率[Kw]", sType: "fieldPara", sPid: "1008", sTrPrefix: "3", sFieldIndex: "33", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合扭矩[N.m]", sType: "fieldPara", sPid: "1009", sTrPrefix: "3", sFieldIndex: "34", unit: "", joinCode: "" },
    { sFieldTitle: "发动机描述", sType: "fieldPara", sPid: "945", sTrPrefix: "3", sFieldIndex: "35", unit: "", joinCode: "" },
    { sFieldTitle: "卡车变速箱描述", sType: "fieldPara", sPid: "1011", sTrPrefix: "3", sFieldIndex: "36", unit: "", joinCode: "" },
    { sFieldTitle: "卡车前进挡位个数", sType: "fieldPara", sPid: "980", sTrPrefix: "3", sFieldIndex: "37", unit: "", joinCode: "" },
    { sFieldTitle: "卡车倒挡位个数", sType: "fieldPara", sPid: "981", sTrPrefix: "3", sFieldIndex: "38", unit: "", joinCode: "" },

    { sFieldTitle: "底盘制动", sType: "bar", sPid: "", sTrPrefix: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-bottomstop" },
    { sFieldTitle: "驱动方式", sType: "fieldPara", sPid: "655", sTrPrefix: "4", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "前悬架类型", sType: "fieldPara", sPid: "728", sTrPrefix: "4", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "后悬架类型", sType: "fieldPara", sPid: "720", sTrPrefix: "4", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "可调悬挂", sType: "fieldPara", sPid: "708", sTrPrefix: "4", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "前轮制动器类型", sType: "fieldPara", sPid: "726", sTrPrefix: "4", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "后轮制动器类型", sType: "fieldPara", sPid: "718", sTrPrefix: "4", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "驻车制动类型", sType: "fieldPara", sPid: "716", sTrPrefix: "4", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "车体结构", sType: "fieldPara", sPid: "572", sTrPrefix: "4", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "差速器/差速锁", sType: "fieldMultiValue", sPid: "733", sTrPrefix: "4", sFieldIndex: "8", unit: "", joinCode: "" },
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
    { sFieldTitle: "前侧气囊", sType: "fieldPara", sPid: "691", sTrPrefix: "5", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后侧气囊", sType: "fieldPara", sPid: "680", sTrPrefix: "5", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "侧安全气帘", sType: "fieldPara", sPid: "690", sTrPrefix: "5", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "膝部气囊", sType: "fieldPara", sPid: "835", sTrPrefix: "5", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "安全带气囊", sType: "fieldPara", sPid: "845", sTrPrefix: "5", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "后排中央气囊", sType: "fieldPara", sPid: "1017", sTrPrefix: "5", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "胎压监测", sType: "fieldPara", sPid: "714", sTrPrefix: "5", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "零胎压续航轮胎", sType: "fieldPara", sPid: "715", sTrPrefix: "5", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后排儿童座椅接口(ISO FIX/LATCH)", sType: "fieldPara", sPid: "495", sTrPrefix: "5", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "驾驶辅助", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-drivingassistance" },
    { sFieldTitle: "定速巡航", sType: "fieldMultiValue", sPid: "545", sTrPrefix: "6", sFieldIndex: "0", unit: "", joinCode: "" },
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
    { sFieldTitle: "前大灯", sType: "fieldMultiValue", sPid: "614", sTrPrefix: "7", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "LED日间行车灯", sType: "fieldPara", sPid: "794", sTrPrefix: "7", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "自动大灯", sType: "fieldMultiValue", sPid: "609", sTrPrefix: "7", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "前雾灯", sType: "fieldPara", sPid: "607", sTrPrefix: "7", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "大灯功能", sType: "fieldMultiValue", sPid: "612", sTrPrefix: "7", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "天窗类型", sType: "fieldMultiValue", sPid: "567", sTrPrefix: "7", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "前电动车窗", sType: "fieldPara", sPid: "601", sTrPrefix: "7", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "后电动车窗", sType: "fieldPara", sPid: "1038", sTrPrefix: "7", sFieldIndex: "7", unit: "", joinCode: "" },
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
    { sFieldTitle: "方向盘换挡", sType: "fieldPara", sPid: "547", sTrPrefix: "8", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "前排空调", sType: "fieldPara", sPid: "839", sTrPrefix: "8", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后排空调", sType: "fieldPara", sPid: "838", sTrPrefix: "8", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "香氛系统", sType: "fieldPara", sPid: "1027", sTrPrefix: "8", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "空气净化", sType: "fieldPara", sPid: "905", sTrPrefix: "8", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "车载冰箱", sType: "fieldPara", sPid: "485", sTrPrefix: "8", sFieldIndex: "12", unit: "", joinCode: "" },

    { sFieldTitle: "座椅配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-chair" },
    { sFieldTitle: "座椅材质", sType: "fieldPara", sPid: "544", sTrPrefix: "9", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "运动风格座椅", sType: "fieldPara", sPid: "546", sTrPrefix: "9", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "主座椅电动调节", sType: "fieldMultiValue", sPid: "508", sTrPrefix: "9", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "副座椅电动调节", sType: "fieldMultiValue", sPid: "503", sTrPrefix: "9", sFieldIndex: "3", unit: "", joinCode: "" },
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
    { sFieldTitle: "后排折叠桌板", sType: "fieldPara", sPid: "1032", sTrPrefix: "9", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "信息娱乐", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-pastime" },
    { sFieldTitle: "中控彩色液晶屏", sType: "fieldMultiValue", sPid: "488", sTrPrefix: "10", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "全液晶仪表盘", sType: "fieldMultiValue", sPid: "988", sTrPrefix: "10", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "行车电脑显示屏", sType: "fieldMultiValue", sPid: "832", sTrPrefix: "10", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "HUD平视显示", sType: "fieldPara", sPid: "518", sTrPrefix: "10", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "GPS导航", sType: "fieldMultiValue", sPid: "516", sTrPrefix: "10", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "智能互联定位", sType: "fieldPara", sPid: "1033", sTrPrefix: "10", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "语音控制", sType: "fieldPara", sPid: "1035", sTrPrefix: "10", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "手机互联(Carplay&Android)", sType: "fieldMultiValue", sPid: "1036", sTrPrefix: "10", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "手机无线充电", sType: "fieldPara", sPid: "1037", sTrPrefix: "10", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "手势控制系统", sType: "fieldPara", sPid: "1034", sTrPrefix: "10", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "CD/DVD", sType: "fieldMultiValue", sPid: "510", sTrPrefix: "10", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "蓝牙/WIFI连接", sType: "fieldMultiValue", sPid: "479", sTrPrefix: "10", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "外接接口", sType: "fieldMultiValue", sPid: "810", sTrPrefix: "10", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "车载电视", sType: "fieldPara", sPid: "559", sTrPrefix: "10", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "音响品牌", sType: "fieldPara", sPid: "473", sTrPrefix: "10", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "扬声器数量(个)", sType: "fieldMultiValue", sPid: "523", sTrPrefix: "10", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "后排液晶屏/娱乐系统", sType: "fieldPara", sPid: "477", sTrPrefix: "10", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "车载220V电源", sType: "fieldMultiValue", sPid: "467", sTrPrefix: "10", sFieldIndex: "17", unit: "", joinCode: "" },

    { sFieldTitle: "选配包", sType: "optional", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-optional" }
];
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
 { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "车身颜色", sPid: "598", sTrPrefix: "0", unit: "", joinCode: "" },
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
	{ sType: "bar", sFieldIndex: "", sFieldTitle: "电池/电机", sPid: "", sTrPrefix: "16", unit: "", joinCode: "", scrollId: "params-electric" },
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
  { sType: "bar", sFieldIndex: "", sFieldTitle: "门窗/后镜", sPid: "", sTrPrefix: "9", unit: "", joinCode: "", scrollId: "params-doorswindow" },
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
//元素居中
!function ($) {
    $.fn.showCenter = function () {
        var top = ($(window).height() - this.height()) / 2;
        var left = ($(window).width() - this.width()) / 2;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();

        if (ComparePageObject.IE6) {
            return this.css({ position: 'absolute', 'z-index': 1100, 'top': top + scrollTop, left: left + scrollLeft }).show();
        }
        else {
            return this.css({ position: 'fixed', 'z-index': 1100, 'top': "200px", left: left }).show();
        }
    }
}(jQuery);
//滚动监听
!function ($) {

    "use strict";

    function ScrollSpy(element, options) {
        var process = $.proxy(this.process, this)
		  , $element = $(element).is('body') ? $(window) : $(element)
		  , href;
        this.options = $.extend({}, $.fn.scrollspy.defaults, options);
        this.$scrollElement = $element.on('scroll.scroll-spy.data-api', process);
        this.selector = (this.options.target) + ' li > a';
        this.$body = $('body');
        this.refresh();
        this.process();
    }

    ScrollSpy.prototype = {

        constructor: ScrollSpy

	  , refresh: function () {
	      var self = this,
          $targets,
          scrollTop = self.$scrollElement.scrollTop();

	      this.offsets = $([]);
	      this.targets = $([]);

	      $targets = this.$body
            .find(this.selector)
            .map(function (i, n) {
                var $el = $(this),
                 targetName = $el.data('target'),
                 targetHeight = $el.height(),
                 $targetElement = $("#" + targetName);
                if ($targetElement && ($targetElement.length > 0)) {
                    //var ElementScrollTop = ($targetElement.offset().top + self.options.offset - (i * (targetHeight)));
                    var ElementScrollTop = ($targetElement.offset().top + self.options.offset - (i * (targetHeight + 1)) - ComparePageObject.MenuOffsetTop + 78);
                    //ElementScrollTop = i > 0 ? ElementScrollTop + 30 : ElementScrollTop;
                    if (scrollTop >= ElementScrollTop) {
                        self.activate(targetName)
                    }
                    $el.unbind("click");
                    $el.bind("click", function (e) {
                        e.preventDefault();
                        ComparePageObject.OneLeftScrollFlag = false;
                        $("html,body").animate({ scrollTop: ElementScrollTop + 5 }, 300, function () {
                            if (typeof self.options["callback"] != "undefined") { self.options["callback"](); }
                        });
                    });
                    return ([[ElementScrollTop, targetName]])
                } else
                    return null;
            })
            .sort(function (a, b) { return a[0] - b[0] })
            .each(function () {
                self.offsets.push(this[0])
                self.targets.push(this[1])
            });
	  }

	  , process: function () {
	      var scrollTop = this.$scrollElement.scrollTop()
            , scrollHeight = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight
            , maxScroll = scrollHeight - this.$scrollElement.height()
            , offsets = this.offsets
            , targets = this.targets
            , activeTarget = this.activeTarget
            , i;
	      if (scrollTop >= maxScroll) {
	          return activeTarget != (i = targets.last()[0])
                && this.activate(i)
	      }
	      for (i = offsets.length; i--;) {
	          activeTarget != targets[i];
	          if (scrollTop >= offsets[i] && (!offsets[i + 1] || scrollTop <= offsets[i + 1])) {
	              this.activate(targets[i])
	          }
	      }
	  }

	  , activate: function (target) {

	      this.activeTarget = target;

	      $(this.selector)
            .parent('.current')
            .removeClass('current');

	      var currSelector = this.selector + '[data-target="' + target + '"]';

	      var active = $(currSelector)
            .parent('li')
            .addClass('current');

	      active.trigger('activate');
	  }

    }

    var old = $.fn.scrollspy

    $.fn.scrollspy = function (option, callbackFunc) {
        if (callbackFunc && callbackFunc instanceof Function) option["callback"] = callbackFunc;
        return this.each(function () {
            var $this = $(this)
			  , data = $this.data('scrollspy')
			  , options = typeof option == 'object' && option
            if (!data) $this.data('scrollspy', (data = new ScrollSpy(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }

    $.fn.scrollspy.Constructor = ScrollSpy

    $.fn.scrollspy.defaults = {
        offset: 0,
        offsetList: 0
    }


    $.fn.scrollspy.noConflict = function () {
        $.fn.scrollspy = old
        return this
    }


    //	$(function () {
    //		$('[data-spy="scroll"]').each(function () {
    //			var $spy = $(this)
    //			$spy.scrollspy($spy.data())
    //		})
    //	})

}(jQuery);
//元素交换
!function ($) {
    $.fn.clickChange = function (opts) {
        var defaults = {
            speed: 200, //移动速度
            offset: 0
        }
        var option = $.extend(defaults, opts);

        var animateCallback = function (obj) {
            $(obj).remove();
        }

        this.each(function () {
            var _this = $(this);
            _this.on('click', '.change-car-area .btn-pre-car', function () {
                if (ComparePageObject.IsChange) return;
                ComparePageObject.IsChange = true;
                var parent = $(this).closest('.tableHead_item').find(".sel-car-move,div[id^='FloatTop_carBox_']").eq(0);
                var prevItem = $(this).closest('.pd0').prev().find(".sel-car-move,div[id^='FloatTop_carBox_']").eq(0);
                if (prevItem.length == 0) return;
                var index = parent.attr("id").replace("draggcarbox_", "").replace("FloatTop_carBox_", "");
                //ie6 7 没有动画效果
                //if (ComparePageObject.IE6 || ComparePageObject.IE7) {
                if (ComparePageObject.IsIE) {
                    //标记可切换
                    ComparePageObject.IsChange = false;
                    if (index && option["leftCallback"]) {
                        option["leftCallback"](index);
                    }
                    return;
                }

                var parentLeft = parent.position().left;
                var prevItemLeft = prevItem.position().left;
                parent.css('display', 'none');
                prevItem.css('display', 'none');
                var parentCloneElement = parent.clone();
                var prevCloneElement = prevItem.clone();

                parentCloneElement.insertAfter(parent).css({ position: 'absolute', display: 'block', 'z-index': '1000' }).animate({ left: parentLeft - 200 }, option.speed, function () {
                    parent.insertBefore(prevCloneElement).css('display', 'block');
                    animateCallback(prevCloneElement);
                    //标记可切换
                    ComparePageObject.IsChange = false;

                    if (index && option["leftCallback"]) {
                        option["leftCallback"](index);
                    }
                });
                prevCloneElement.insertAfter(prevItem).css({ position: 'absolute', display: 'block', 'z-index': '999' }).animate({ left: parentLeft + 200 }, option.speed, function () {
                    prevItem.insertBefore(parentCloneElement).css('display', 'block');
                    animateCallback(parentCloneElement);
                });
            });
            _this.on('click', '.change-car-area .btn-next-car', function () {
                if (ComparePageObject.IsChange) return;
                ComparePageObject.IsChange = true;
                var parent = $(this).closest('.tableHead_item').find(".sel-car-move,div[id^='FloatTop_carBox_']").eq(0);
                var nextItem = $(this).closest('.pd0').next().find(".sel-car-move,div[id^='FloatTop_carBox_']").eq(0);
                var index = parent.attr("id").replace("draggcarbox_", "").replace("FloatTop_carBox_", "");
                if (nextItem.length == 0) return;

                //ie6 7 没有动画效果
                //if (ComparePageObject.IE6 || ComparePageObject.IE7) {
                if (ComparePageObject.IsIE) {
                    //标记可切换
                    ComparePageObject.IsChange = false;
                    if (index && option["rightCallback"]) {
                        option["rightCallback"](index);
                    }
                    return;
                }
                var parentLeft = parent.position().left;
                var nextItemLeft = nextItem.position().left;
                parent.css('display', 'none');
                nextItem.css('display', 'none');
                var parentCloneElement = parent.clone();
                var nextCloneElement = nextItem.clone();
                //alert(parentLeft);
                parentCloneElement.insertAfter(parent).css({ position: 'absolute', display: 'block', 'z-index': '1000' }).animate({ left: parentLeft + 200 }, option.speed, function () {
                    parent.insertBefore(nextItem).css('display', 'block');
                    animateCallback(nextCloneElement);
                    //标记可切换
                    ComparePageObject.IsChange = false;
                    if (index && option["rightCallback"]) {
                        option["rightCallback"](index);
                    }
                });

                nextCloneElement.insertAfter(nextItem).css({ position: 'absolute', display: 'block', 'z-index': '999' }).animate({ left: nextItemLeft - 200 }, option.speed, function () {
                    nextItem.insertBefore(parentCloneElement).css('display', 'block');
                    animateCallback(parentCloneElement);
                });
            });
        });
    }
}(jQuery)

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