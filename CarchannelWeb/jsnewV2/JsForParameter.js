// 车型频道对比
var ComparePageObject = {
    PageDivContentID: "CarCompareContent",  // container id
    PageDivContentObj: null,   // container object
    ArrPageContent: new Array(),
    ValidCount: 0,
    AllCarJson: new Array(),
    ArrCarInfo: new Array(),
    IsOperateTheSame: false,
    IsDelSame: false,//隐藏相同项
    ArrTempBarHTML: new Array(),
    IsNeedDrag: true,
    DragID: "",
    DropID: "",
    ArrSelectExhaust: new Array(),
    ArrSelectTransmission: new Array(),
    ArrSelectYearType: new Array(),
    ArrSelectPower: new Array(),//电力
    Power: "纯电动",
    FloatTop: new Array(),
    ArrTempBarForFloatLeftHTML: new Array(),
    FloatLeft: new Array(),
    IsNeedFirstColor: false,
    CurrentCarID: 0,
    BaikeObj: null,
    CarIDAndNames: "",
    ArrTempLeftNavHTML: [],
    ArrLeftNavHTML: [],
    IsShowDiff: true, //差异显示
    IsVantage: true, //优势项 默认选中
    IE6: ! -[1, ] && !window.XMLHttpRequest,
    IE7: navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1,
    IsIE: (navigator.appName == "Microsoft Internet Explorer"),
    IsChange: false, //用于切换车款元素 效果
    OneLeftScrollFlag: false, //滚动菜单是否显示
    MenuOffsetTop: 175, //滚动菜单 相对车款头的高度偏移量
    MenuOffsetLeft: 60//滚动菜单 相对表格 左偏移量
    //DocumentWidthLimit: 1400 //屏幕宽度临界值
}
//头部广告回调函数
function pullTopAd(adHeight) {
    //添加滚动监听事件
    $('[data-spy="scroll"]').each(function () {
        var $spy = $(this);
        //$.extend(true, $spy.data(), { "adHeight": adHeight });
        $spy.scrollspy($spy.data(), function () { scrollCallback(); });
        $spy.scrollspy("refresh");
    });
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
                //var power = carCookie[4];
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
                    //carinfo.Power = power;
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
//滚动监听 滚动完成后回调函数
function scrollCallback() {
    var scrollsLeft = $(window).scrollLeft(); //窗口左卷动值
    var boxoffsetLeft = $("#box").offset().left; //计算box的offleft值
    if (scrollsLeft > boxoffsetLeft) {
        $("#left-nav").fadeOut(1000, function () {
            $("#show-left-nav").show();
            ComparePageObject.OneLeftScrollFlag = false;
        });
    }
}
function createPageForCompare(isDelSame) {
    ComparePageObject.IsDelSame = isDelSame;
    var loopCount = arrField.length;
    ComparePageObject.ArrPageContent.push("<table cellspacing=\"0\" cellpadding=\"0\" style=\"border-right:100px solid white;\"><tbody>");
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
            case "optional":
                createOptional(arrField[i]);
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
    //填充左侧滚动菜单
    if (ComparePageObject.ValidCount > 0) {
        if (ComparePageObject.ArrLeftNavHTML.length > 0) {
            $("#left-nav ul").html(ComparePageObject.ArrLeftNavHTML.join('')).find("li:last").addClass("last");//.show();
            ComparePageObject.ArrLeftNavHTML.length = 0;
        } else {
            $("#left-nav ul").html("").parent().hide();
            $("#show-left-nav").hide();
        }
    } else {
        if (ComparePageObject.ArrLeftNavHTML.length <= 0) {
            $("#left-nav ul").html("").parent().hide();
            $("#show-left-nav").hide();
        }
    }
    ComparePageObject.OneLeftScrollFlag = false;
    mathLeftMenu($(window).scrollLeft(), $("#box").offset().left, $(window).scrollTop(), $("#main_box").offset().top);
    ComparePageObject.ArrPageContent.length = 0;
    changeCheckBoxStateByName("checkboxForDelTheSame", ComparePageObject.IsOperateTheSame);
    setTRColorWhenMouse();
    setImgDrag();
    //绑定事件
    bindEvent();
    //购车服务回调方法
    //goucheCallback();
    reCountLeftNavHeight();
}
//计算左侧浮动导航高度
function reCountLeftNavHeight() {
    var $mainTableTh = $('#CarCompareContent').find(" > table > tbody > tr > th"),
        $leftTableTh = $('#leftfixed').find(' > table > tbody > tr > th');
    $leftTableTh.each(function (i) {
        this.style.height = $mainTableTh.eq(i).outerHeight() + 'px';
    })
}

//购车服务 找低价
function goucheCallback() {
    var zdjcityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201";
    $.ajax({
        url: "http://api.gouche.yiche.com/PreferentialCar/GetCarListBySerialId?serialid=" + CarCommonCSID + "&cityid=" + zdjcityId + "", cache: true, dataType: "jsonp", jsonpCallback: "getCarList", success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (i, n) {
                    $("#gc_" + n["CarId"]).show();
                });
            }
        }
    });
}
function setImgDrag() {
    $("div[id^='draggcarbox_'] dl").each(function (i) {
        $(this).parent().draggable({
            //proxy: 'clone',
            revert: true,
            cursor: 'move',
            //handle: "div dd",
            //cancel: "a",
            onStartDrag: function () {
                //$(this).draggable('proxy').css({ 'z-index': '999' });
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
                    swapArray(ComparePageObject.ArrCarInfo, ComparePageObject.DragID, ComparePageObject.DropID);
                    createPageForCompare(ComparePageObject.IsOperateTheSame);
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
            //$(source).draggable('proxy').remove();
            $(this).removeClass('moving');
            ComparePageObject.DropID = "";
            if (typeof (this.id) != "undefined" && this.id) {
                ComparePageObject.DropID = this.id.replace("carBox_", "");
            }
        }
    });
}

function bindEvent() {
    //纠错
    var strError = "车款名称：\n问题描述：\n联系方式(电话/QQ)：";
    $("a[name='correcterror']").click(function () { $("#popup-box").showCenter(); $("#correctError").val(strError); });
    $("#popup-box .close,#btnErrorCancel,#btn-success-close").click(function () {
        $("#popup-box").hide();
        $("#popup-box-content .form-group").show();
        $("#popup-box-success").hide();
    });
    //$("#popup-box-success .close,#btn-success-close").click(function () { $("#popup-box-success").hide(); });
    $("#popup-box a[name='btnCorrectError']").click(function (e) {
        e.preventDefault();
        var content = $("#correctError").val();
        if (content == "" || content == strError) {
            $("#alert-center").html("<p>请输入提交内容。</p>").show();
            setTimeout(function () {
                $("#alert-center").html("").hide();
            }, 1000);
            return;
        }
        var url = "http://www.bitauto.com/FeedBack/api/CommentNo.ashx?txtContent=" + encodeURIComponent(content) + "&satisfy=1&ProductId=5&categorytype=2&csid=" + (typeof (CarCommonCSID) != "undefined" ? CarCommonCSID : 0) + "&refer=http://car.bitauto.com" + window.location.pathname;
        $.ajax({
            url: url, dataType: 'jsonp', jsonp: "XSS_HTTP_REQUEST_CALLBACK", jsonpCallback: "errorCallback", success: function (data) {
                if (data && data.status == "success") {
                    //$("#popup-box").hide();
                    $("#popup-box-success").removeClass("note-error").addClass("note-ok");
                    $("#popup-box-success").find(".tit").html("提交成功！").siblings("p").html("您提交的纠错信息我们已经收集到，感谢您的纠错。");
                    //$("#popup-box-success").show();
                } else {
                    $("#popup-box-success").removeClass("note-ok").addClass("note-error");
                    $("#popup-box-success").find("h3").html("提交失败！").siblings("p.tip").html(data.message);
                    //$("#popup-box-success").show();
                }
                $("#popup-box-content .form-group").hide();
                $("#popup-box-success").show();
            }, error: function () {

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
    $(".tableHead_item").clickChange({ "leftCallback": moveLeftForCarCompare, "rightCallback": moveRightForCarCompare });
    if (ComparePageObject.ValidCount <= 0 || $("#left-nav ul li").length <= 0) {
        $(window).scrollTop(0);
    }

    //加对比
    typeof InitCompareEvent == "function" && InitCompareEvent();

    //添加滚动监听事件
    $('[data-spy="scroll"]').each(function () {
        var $spy = $(this);
        $spy.scrollspy($spy.data(), function () { scrollCallback(); });
        $spy.scrollspy("refresh");
    });
}
// create pic for compare
function createPic() {
    // for FloatTop
    ComparePageObject.FloatTop.length = 0;
    ComparePageObject.FloatTop.push("<table cellpadding=\"0\" cellspacing=\"0\">");
    ComparePageObject.FloatTop.push("<tr>");
    ComparePageObject.FloatTop.push("<th class=\"pd0\">");
    ComparePageObject.FloatTop.push("<div class=\"tableHead_left\">");

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
    ComparePageObject.FloatLeft.push("<th class=\"pd0\">");
    ComparePageObject.FloatLeft.push("<div class=\"tableHead_left\">");

    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" id=\"chkAdvantage\" name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + "> <label for=\"chkAdvantage\">标识优势项 <em></em></label></div>");
    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" id=\"checkboxForDiff\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + "> <label for=\"checkboxForDiff\">高亮不同项</label></div>");
    ComparePageObject.FloatLeft.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" id=\"checkDiffer\"> <label for=\"checkDiffer\">隐藏相同项</label></div>");

    ComparePageObject.FloatLeft.push("<div class=\"dashline\"></div>");
    ComparePageObject.FloatLeft.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p>");
    ComparePageObject.FloatLeft.push("</div>");
    ComparePageObject.FloatLeft.push("</th>");
    ComparePageObject.FloatLeft.push("</tr>");

    ComparePageObject.ArrPageContent.push("<tr class=\"trForPic\">");
    ComparePageObject.ArrPageContent.push("<th class=\"pd0\">");
    ComparePageObject.ArrPageContent.push("<div class=\"tableHead_left\">");

    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"chkAdvantage\" onclick=\"advantageForCompare();\" " + (ComparePageObject.IsVantage ? "checked" : "") + "> <label for=\"chkAdvantage\">标识优势项 <em></em></label></div>");
    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDiff\" onclick=\"showDiffForCompare();\" " + (ComparePageObject.IsShowDiff ? "checked" : "") + "> <label for=\"checkboxForDiff\">高亮不同项</label></div>");
    ComparePageObject.ArrPageContent.push("<div class=\"check-box-item\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\"> <label for=\"checkDiffer\">隐藏相同项</label></div>");

    ComparePageObject.ArrPageContent.push("<div class=\"dashline\"></div>");
    ComparePageObject.ArrPageContent.push("<p>●标配 ○选配&nbsp;&nbsp;- 无</p></div>");
    ComparePageObject.ArrPageContent.push("</th>");

    //已加入对比的车型
    var isShowLoop = 0;
    var com_new_arr = new Array();

    /*if (CookieForCompare) {
		var compare = CookieForCompare.GetCookie("ActiveNewCompare");
		if (compare) {
			var com_arr = compare.split("|");
			for (var i = 0; i < com_arr.length; i++) {
				var idAndName = com_arr[i].split(",");
				if (idAndName.length = 2) {
					var id = idAndName[0].replace("id", "");
					com_new_arr.push(id);
				}
			} 
		}
	}*/

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                ComparePageObject.ArrPageContent.push("<td class=\"pd0\" id=\"td_" + i + "\" ><div class=\"tableHead_item tableHead_item_nopic\">");
                try {
                    // car info
                    var carid = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][0];
                    var carName = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][1];
                    var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6];
                    var carYear = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][7];
                    var referPrice = ComparePageObject.ArrCarInfo[i].CarInfoArray[1][0];
                    ComparePageObject.ArrPageContent.push("<div class=\"sel-car-box\" id=\"carBox_" + i + "\">");
                    ComparePageObject.ArrPageContent.push("<div style=\"height:80px\"><div id=\"draggcarbox_" + i + "\" class=\"sel-car-move\" style=\"height:80px;width:150px \">");
                    ComparePageObject.ArrPageContent.push("<dl>");
                    ComparePageObject.ArrPageContent.push("<dd><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/\" title=\"" + (carYear == "" ? "" : "" + carYear + "款 ") + carName + "\">" + (carYear == "" ? "" : "" + carYear + "款 ") + carName + "</a></dd>");
                    if (referPrice != "无") {
                        ComparePageObject.ArrPageContent.push("<dd class=\"car-price\"><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/baojia/\">" + referPrice + "</a>" + (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][9] == "待销" ? "(预售价)" : "(厂商指导价)") + "</dd>");
                    }
                    ComparePageObject.ArrPageContent.push("</dl>");
                    ComparePageObject.ArrPageContent.push("</div></div>");
                    // for FloatTop
                    ComparePageObject.FloatTop.push("<td class=\"pd0\">");
                    ComparePageObject.FloatTop.push("<div id=\"FloatTop_tableHead_" + i + "\" class=\"tableHead_item tableHead_item_nopic\">");
                    ComparePageObject.FloatTop.push("<div class=\"sel-car-box\">");
                    ComparePageObject.FloatTop.push("<div class=\"dl-box\" style=\"height:80px;\"><div id=\"FloatTop_carBox_" + i + "\" class=\"dl-box\" style=\"width:150px;\">");
                    ComparePageObject.FloatTop.push("<dl><dd><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/\" title=\"" + (carYear == "" ? "" : "" + carYear + "款 ") + carName + "\">" + (carYear == "" ? "" : "" + carYear + "款 ") + carName + "</a></dd>");
                    if (referPrice != "无") {
                        ComparePageObject.FloatTop.push("<dd class=\"car-price\"><a target=\"_blank\" href=\"/" + csAllSpell + "/m" + carid + "/baojia/\">" + referPrice + "</a>" + (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][9] == "待销" ? "(预售价)" : "(厂商指导价)") + " </dd>");
                    }
                    ComparePageObject.FloatTop.push("</dl>");
                    ComparePageObject.FloatTop.push("</div></div>");

                    ComparePageObject.FloatTop.push("<div id=\"FloatTop_OptArea_" + i + "\" class=\"change-car-area\">");

                    ComparePageObject.ArrPageContent.push("<div class=\"change-car-area\">");
                    if (ComparePageObject.IsNeedDrag) {

                        var compareStr = "";
                        compareStr = "<div class=\"button_gray btn-compare-car\"><a title=\"对比\" href=\"javascript:;\" class=\"btn btn-xs\" data-use=\"compare\" data-id=\"" + carid + "\"><span>+对比</span></a></div>";
                        if (isShowLoop == 0) {
                            if (checkIsRightEnd(i)) {
                                ComparePageObject.ArrPageContent.push(compareStr);
                                ComparePageObject.FloatTop.push(compareStr);
                            }
                            else {
                                ComparePageObject.ArrPageContent.push(compareStr);
                                ComparePageObject.ArrPageContent.push("<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
                                ComparePageObject.FloatTop.push(compareStr);
                                ComparePageObject.FloatTop.push("<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
                            }
                        }
                        else {
                            if (checkIsRightEnd(i)) {
                                ComparePageObject.ArrPageContent.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\" title=\"左移\"><</a></span>");
                                ComparePageObject.FloatTop.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\"  title=\"左移\"><</a></span>");
                                ComparePageObject.ArrPageContent.push(compareStr);
                                ComparePageObject.FloatTop.push(compareStr);
                            }
                            else {
                                ComparePageObject.ArrPageContent.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\" title=\"左移\"><</a></span>" + compareStr + "<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
                                ComparePageObject.FloatTop.push("<span class=\"button_gray btn-pre-car\"><a href=\"javascript:;\"  title=\"左移\"><</a></span>" + compareStr + "<span class=\"button_gray btn-next-car\"><a href=\"javascript:;\" title=\"右移\">></a></span>");
                            }
                        }
                    }
                    ComparePageObject.ArrPageContent.push("</div>");
                    ComparePageObject.ArrPageContent.push("</div>");
                    ComparePageObject.ArrPageContent.push("<a class=\"btn-close-car\" title=\"删除\" href=\"javascript:;\" onclick=\"delCarToCompare('" + i + "',event);\">删除</a>");
                    // for FloatTop
                    ComparePageObject.FloatTop.push("</div></div>");
                    ComparePageObject.FloatTop.push("<a class=\"btn-close-car\" title=\"删除\" href=\"javascript:;\" onclick=\"delCarToCompare('" + i + "',event);\">删除</a>");
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
            ComparePageObject.ArrPageContent.push("<td class=\"pd0\"><div class=\"tableHead_item tableHead_item_nopic\"></div></td>");
            ComparePageObject.FloatTop.push("<td class=\"pd0\"><div class=\"tableHead_item tableHead_item_nopic\"></div></td>");
        }
    }
    ComparePageObject.ArrPageContent.push("</tr>");
    ComparePageObject.FloatTop.push("</tr>");
    ComparePageObject.FloatTop.push("</table>");
    $("#topfixed").html(ComparePageObject.FloatTop.join(""));
}

// create price for compare
function createPrice(arrFieldRow) {
    var isPrice = false;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\">");
            ComparePageObject.ArrPageContent.push("<th>" + checkBaikeForTitle(arrFieldRow) + "</th>");
            isPrice = true;
            break;
        }
    }
    if (!isPrice) {
        return;
    }
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
                    if (arrFieldRow["sFieldTitle"] == "商家报价") {
                        //modified by 2014.07.16 报价 第一个 万 去掉
                        var fieldTemp = field,
						 arrTemp = field.split('-');
                        if (arrTemp.length > 1)
                            fieldTemp = arrTemp[0].replace("万", "") + "-" + arrTemp[1];
                        ComparePageObject.ArrPageContent.push("<div class=\"xunjia\">");
                        ComparePageObject.ArrPageContent.push("<span class=\"cRed\"><a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.ArrCarInfo[i].CarID + "&citycode=0&leads_source=p042003\">" + fieldTemp + "</a></span>");
                        ComparePageObject.ArrPageContent.push("<div class=\"button_gray button_43_20\"><a target=\"_blank\" href=\"http://dealer.bitauto.com/zuidijia/nb" + (ComparePageObject.ArrCarInfo[i].CarInfoArray[0][3] || "") + "/nc" + ComparePageObject.ArrCarInfo[i].CarID + "/?leads_source=p042001\">询价</a></div>");
                        ComparePageObject.ArrPageContent.push("</div>");
                    }
                    else if (arrFieldRow["sFieldTitle"] == "降价优惠") {
                        var csAllSpell = ComparePageObject.ArrCarInfo[i].CarInfoArray[0][6] || "";
                        ComparePageObject.ArrPageContent.push("<span class=\"tdJiangjia\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + csAllSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/jiangjia/\">" + field + "</a></span>");
                    }
                    else { }
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
    var colorRow = 0;

    var chkResult = { IsSame: true, CurrCarIndex: 0, CurrParamsValue: 0 };
    var vantage = (ComparePageObject.IsVantage && arrFieldRow["isVantage"] && arrFieldRow["isVantage"] == "1");
    if (vantage || ComparePageObject.IsShowDiff)
        chkResult = IsFieldSame(arrFieldRow);
    var parameterId = arrFieldRow["sPid"];
    if (!chkResult) {
        return;
    }

    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
                { arrTemp.push("<td name=\"td" + i + "\" class=\"f\">"); }
                else if (!chkResult.IsSame && ComparePageObject.IsShowDiff)
                    arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
                    //else if (!chkResult.IsSame && vantage && chkResult.CurrCarIndex != -1) {
                    //	arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
                    //}
                    //else if (arrFieldRow["sFieldTitle"] == "车身颜色") {
                    //    arrTemp.push("<td name=\"td" + i + "\" class=\"f\">");
                    //}
                else { arrTemp.push("<td name=\"td" + i + "\" >"); }
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
                    //					if (field.length > 0) {
                    //						if (arrFieldRow["unit"] != "") {
                    //							field += "" + arrFieldRow["unit"];
                    //						}
                    //					}
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
                    if (field.indexOf("选配") == 0)
                    {
                        var fieldInfo = field.split('|');
                        if (fieldInfo.length > 1) {
                            field = "<span class=\"songti\">○ 选配" + formatCurrency(fieldInfo[1]) + "元</span>";
                        }
                        else{
                            field = "<span class=\"songti\">○</span>";
                        }
                    }
                    if (field.indexOf("无") == 0)
                    { field = "<span class=\"songti\">-</span>"; }
                    //modified by sk 2014.07.11 ie6 样式问题 空赋值 &nbsp;
                    if (field == "") field = "&nbsp;";
                    // modified by chengl Dec.28.2009 for calculator
                    if (arrFieldRow["sFieldTitle"] == "厂商指导价" && field != "无" && field != "待查") {
                        arrTemp.push("<b>" + field + (ComparePageObject.AllCarJson[i][0][9] == "待销" && field.indexOf('-') < 0 ? "(预售)" : "") + "</b>");
                        arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + ComparePageObject.AllCarJson[i][0][0] + "\"  target=\"_blank\"></a>");
                        //arrTemp.push("<a id=\"gc_" + ComparePageObject.AllCarJson[i][0][0] + "\" style=\"display:none;\" class=\"right\" target=\"_blank\" href=\"http://gouche.yiche.com/" + bit_locationInfo["engName"] + "/sb" + ComparePageObject.AllCarJson[i][0][3] + "/m" + ComparePageObject.AllCarJson[i][0][0] + "/?leads_source=p042002\">找低价&gt;</a>");
                    }
                    else if (arrFieldRow["sFieldTitle"] == "车身颜色" && field != "") {
                        var colorArr = field.split('|');
                        arrTemp.push("<ul class=\"bodycolor\">");
                        var tempcolorRow = colorArr.length % 5 == 0 ? colorArr.length / 5 : (parseInt(colorArr.length / 5) + 1);
                        if (tempcolorRow > colorRow) {
                            colorRow = tempcolorRow;
                        }
                        for (var c = 0; c < colorArr.length; c++) {
                            if (colorArr[c] == "") continue;
                            color = colorArr[c].split(',');
                            if (color.length != 2) continue;
                            arrTemp.push("<li><a href=\"javascript:void(0);\" style=\"cursor:default;background-color:" + color[1] + "\" title=\"" + color[0] + "\"></a></li>");
                        }
                        arrTemp.push("</ul>");
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
    if (isShowLoop < 5) {
        for (var i = 0; i < 5 - isShowLoop; i++) {
            ComparePageObject.ArrPageContent.push("<td>&nbsp;");
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
            if (!checkCarIsShowForeach(i)) continue;
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

    if (!chkResult) {
        return;
    }

    // loop every car
    var tempArrMultField = [], num = 0;
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            num++;
            if (ComparePageObject.ArrCarInfo[i].CarInfoArray) {
                if ((ComparePageObject.CurrentCarID == ComparePageObject.ArrCarInfo[i].CarID) && ComparePageObject.IsNeedFirstColor)
                { arrTemp.push("<td name=\"td" + i + "\" class=\"f\">"); }
                else if (!chkResult.IsSame)
                    arrTemp.push("<td name=\"td" + i + "\" class=\"bg-blue\">");
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
                                { field = "(" + field + (unitArray[pint] || "") + ")"; }
                            }
                            else {
                                field += unitArray[pint] || "";
                            }
                            // field += unitArray[pint];
                            multiField = multiField + (joinCodeArray[pint] || "") + field;
                            //add by sk 2016.01.08 以下参数有值 直接显示 忽略第二个参数
                            if (pidArray[pint] == "509" || pidArray[pint] == "489" || pidArray[pint] == "555" || pidArray[pint] == "808") {
                                isAllunknown = false;  
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
                        if (multiField.indexOf("有") == 0)
                        { multiField = "<span class=\"songti\">●</span>"; }
                        if (multiField.indexOf("选配") == 0)
                        {
                            var fieldInfo = multiField.split('|');
                            if (fieldInfo.length > 1) {
                                multiField = "<span class=\"songti\">○ 选配" + formatCurrency(fieldInfo[1]) + "元</span>";
                            }
                            else {
                                multiField = "<span class=\"songti\">○</span>";
                            }
                        }
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
                                if (!chkResult.IsSame)
                                    arrTemp.push("<td name=\"td" + m + "\" class=\"bg-blue\">");
                                else
                                    arrTemp.push("<td name=\"td" + m + "\" >");
                                arrTemp.push(tempArrMultField[m]);
                                arrTemp.push("</td>");
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
    if (firstSame && ComparePageObject.IsDelSame && num > 1) {
        return;
    }

    else {
        if (!isAllunknown) {
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
                        arrTemp.push("<div class=\"popup-control-box optional\">● " + standardJson[0].text + "等");
                        arrTemp.push("<div class=\"popup-layout-1\"><ul>");
                        for (var staIndex = 0; staIndex < standardJson.length; staIndex++) {
                            arrTemp.push("<li><span class=\"l\">" + standardJson[staIndex].text + "</span ></li>");
                        }
                        arrTemp.push("</ul></div></div>");
                    }
                }
                if (optionalJson.length == 1) {
                    arrTemp.push("<div>○ 选配" + optionalJson[0].text + "&nbsp;" + formatCurrency(optionalJson[0].price) + "元</div>");
                }
                else if (optionalJson.length > 1) {
                    arrTemp.push("<div class=\"popup-control-box optional\">○ 选装" + formatCurrency(optionalJson[0].price) + "元起");
                    arrTemp.push("<div class=\"popup-layout-1\"><ul>");
                    for (var optIndex = 0; optIndex < optionalJson.length; optIndex++) {
                        arrTemp.push("<li><span class=\"l\">" + optionalJson[optIndex].text + "</span ><span class=\"r\">￥" + formatCurrency(optionalJson[optIndex].price) + "</span></li>");
                    }
                    arrTemp.push("</ul></div></div>");
                }
            }
        }
    }
    if (!isAllunknown) {
        ComparePageObject.ArrPageContent.push("<tr id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldIndex"] + "\" >");
        ComparePageObject.ArrPageContent.push("<th>");
        ComparePageObject.ArrPageContent.push(checkBaikeForTitle(arrFieldRow));
        ComparePageObject.ArrPageContent.push("</th>");
        ComparePageObject.ArrPageContent.push(arrTemp.join(""));
        ComparePageObject.FloatLeft.push("<tr><th>" + checkBaikeForTitle(arrFieldRow) + "</th></tr>");

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
    else {
        return;
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

function createOptional(arrFieldRow) {
    if (ComparePageObject.ValidCount < 1 || typeof optionalPackageJson == "undefined" || optionalPackageJson.length == 0) {
        return;
    }
    createBar(arrFieldRow);
    var arrTemp = new Array();
    for (var opt = 0; opt < optionalPackageJson.length; opt++) {
        var showCarCount = 0;
        arrTemp.push("<tr class=\"multi-row2-start\">");
        arrTemp.push("<th rowspan=\"2\">" + optionalPackageJson[opt].name + "<em></em></th>");
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (checkCarIsShowForeach(i)) {
                showCarCount++;
                if (optionalPackageJson[opt].carid.contains(ComparePageObject.ArrCarInfo[i].CarID)) {
                    arrTemp.push("<td name=\"td" + i + "\"><span>○ 选装￥" + optionalPackageJson[opt].price + "</span></td>");
                }
                else {
                    arrTemp.push("<td name=\"td" + i + "\"><span class=\"songti\">-</span></td>");
                }
            }
        }
        if (showCarCount < 5) {
            for (var kk = 0; kk < 5 - showCarCount; kk++) {
                arrTemp.push("<td>&nbsp;</td>");
            }
        }
        arrTemp.push("<tr class=\"multi-row2-end\"><td colspan=\"" + (showCarCount < 5 ? 5 : showCarCount)  + "\"><span class=\"optional-note\">" + optionalPackageJson[opt].desc + "</span></td></tr>");
        arrTemp.push("</tr>");
    }
    

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
    ComparePageObject.ArrPageContent.push(arrTemp.join(""));
}

function createBar(arrFieldRow) {
    if (ComparePageObject.ValidCount < 1) {
        return;
    }
    ComparePageObject.ArrTempBarHTML.length = 0;
    ComparePageObject.ArrTempBarForFloatLeftHTML.length = 0;
    ComparePageObject.ArrTempLeftNavHTML.length = 0;
    var summaryColumn = 1;
    ComparePageObject.ArrTempBarHTML.push("<tr id=\"" + arrFieldRow["scrollId"] + "\">");
    for (var i = 0; i < ComparePageObject.ValidCount; i++) {
        // check every Car is show
        if (checkCarIsShowForeach(i)) {
            summaryColumn++;
        }
    }
    if (summaryColumn < 6)
    { summaryColumn = 6; }
    ComparePageObject.ArrTempBarHTML.push("<td class=\"pd0 td-tt\" colspan=\"" + summaryColumn + "\">");
    ComparePageObject.ArrTempBarHTML.push("<h2><span>" + arrFieldRow["sFieldTitle"] + "<a href=\"javascript:;\" name=\"correcterror\">参数纠错</a></span></span></h2>");
    ComparePageObject.ArrTempBarHTML.push("</td>");
    ComparePageObject.ArrTempBarHTML.push("</tr>");
    ComparePageObject.ArrTempBarForFloatLeftHTML.push("<tr>");
    ComparePageObject.ArrTempBarForFloatLeftHTML.push("<td class=\"pd0 td-tt\"><h2><span>" + arrFieldRow["sFieldTitle"] + "<a href=\"javascript:;\" name=\"correcterror\">参数纠错</a></span></span></h2></td>");
    ComparePageObject.ArrTempBarForFloatLeftHTML.push("</tr>");
    //左侧菜单数据
    ComparePageObject.ArrTempLeftNavHTML.push("<li class=\"" + (arrFieldRow["sFieldTitle"] == "基本信息" ? "current" : "") + "\"><a data-target=\"" + arrFieldRow["scrollId"] + "\" href=\"javascript:;\" title1=\"" + arrFieldRow["sFieldTitle"] + "\">" + arrFieldRow["sFieldTitle"] + "<i></i></a></li>");
}

// when match title add link to baike
function checkBaikeForTitle(arrFieldRow) {
    var title = arrFieldRow["sFieldTitle"],
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
        if (ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat != "" && ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat != "0.0L") {
            // new exhaust
            if (exhaust.indexOf(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",") < 0) {
                exhaust += ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat + ",";
                arrTempExhaust.push(ComparePageObject.ArrCarInfo[i].EngineExhaustForFloat);
                exhaustCount++;
            }
        }

        if (ComparePageObject.ArrCarInfo[i].CarInfoArray[3][19] == "电力") {
            if (exhaust.indexOf(ComparePageObject.Power + ",") < 0) {
                exhaust += ComparePageObject.Power + ",";
                arrTempExhaust.push(ComparePageObject.Power);
                exhaustCount++;
            }
        }
        //if(ComparePageObject.ArrCarInfo[i].
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
            if (arrTempExhaust[i] == "纯电动") {
                arrExhaust.push("<input type=\"checkbox\" id=\"checkboxExhaust_" + arrTempExhaust[i] + "\" value=\"电力\" onclick=\"checkPower(this);\" >");
            }
            else {
                arrExhaust.push("<input type=\"checkbox\" id=\"checkboxExhaust_" + arrTempExhaust[i] + "\" value=\"" + arrTempExhaust[i] + "\" onclick=\"checkExhaust(this);\" >");
            }
            arrExhaust.push("<label for=\"checkboxExhaust_" + arrTempExhaust[i] + "\">" + arrTempExhaust[i] + "</label>");
        }
    }
    // 变速器排序
    if (arrTempTransmissionType.length > 0) {
        arrTempTransmissionType.sort();
        for (var i = 0; i < arrTempTransmissionType.length; i++) {
            if (arrTempTransmissionType[i] == 1) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_1\" value=\"1\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_1\">手动</label>");
            }
            if (arrTempTransmissionType[i] == 2) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_2\" value=\"2\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_2\">自动</label>");
            }
            if (arrTempTransmissionType[i] == 3) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_3\" value=\"3\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_3\">手自一体</label>");
            }
            if (arrTempTransmissionType[i] == 4) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_4\" value=\"4\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_4\">半自动</label>");
            }
            if (arrTempTransmissionType[i] == 5) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_5\" value=\"5\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_5\">CVT无级变速</label>");
            }
            if (arrTempTransmissionType[i] == 6) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_6\" value=\"6\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_6\">双离合</label>");
            }
            if (arrTempTransmissionType[i] == 7) {
                arrTransmissionType.push("<input type=\"checkbox\" id=\"checkboxTransmissionType_7\" value=\"7\" onclick=\"checkTransmissionType(this);\">");
                arrTransmissionType.push("<label for=\"checkboxTransmissionType_7\">电动车单速</label>");
            }
        }
    }

    //年款
    if (arrTempYear.length > 1) {
        arrTempYear.sort(function (a, b) { return b - a; });
        for (var i = 0; i < arrTempYear.length && i < 3; i++) {
            arrYear.push("<input type=\"checkbox\" id=\"checkboxYearType_" + arrTempYear[i] + "\" value=\"" + arrTempYear[i] + "\" onclick=\"checkYearType(this);\">");
            arrYear.push("<label for=\"checkboxYearType_" + arrTempYear[i] + "\">" + arrTempYear[i] + "款</label>");
        }
    }
    var exW = 0, trW = 0;
    if (arrExhaust.length > 0) {
        //exW = $("#spanFilterForEE").html("排量：" + arrExhaust.join("")).width() + 20;
        $("#spanFilterForEE").html("发动机：" + arrExhaust.join(""))
    }
    if (arrTransmissionType.length > 0) {
        //trW = $("#spanFilterForTT").html("变速箱：" + arrTransmissionType.join("")).width() + 50;
        $("#spanFilterForTT").html("变速箱：" + arrTransmissionType.join(""))
    }
    if (arrYear.length > 0) {
        //		if ($("#divFilter").width() - exW - trW - ((13 + 36) * arrYear.length + 42 + 50) > 0) {
        //			$("#spanFilterForYear").html("年款：" + arrYear.join(""));
        //		}
        $("#spanFilterForYear").html("年款：" + arrYear.join(""));
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
//电力
function checkPower(obj) {
    if (obj.checked) {
        ComparePageObject.ArrSelectPower.push(obj.value.toString());
    }
    else {
        ComparePageObject.ArrSelectPower.pop();
    }
    resetSelectCarsByCheckbox();
}

function resetSelectCarsByCheckbox() {
    if (ComparePageObject.ArrSelectExhaust.length > 0 || ComparePageObject.ArrSelectTransmission.length > 0 || ComparePageObject.ArrSelectYearType.length > 0 || ComparePageObject.ArrSelectPower.length > 0) {
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

            if (ComparePageObject.ArrSelectPower.length > 0) {
                if (ComparePageObject.ArrSelectPower[0].toString() != ComparePageObject.ArrCarInfo[i].CarInfoArray[3][19].toString()) {
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
    //解决IE6删除车款 左侧浮动层卡住不动现象
    if (ComparePageObject.IsIE) { $(window).scrollTop($(window).scrollTop() + 1); }
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
    //this.Power = power;
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

function delCarToCompare(index, e) {
    e = e || window.event;
    var target = e.srcElement || e.target;
    if (ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
        var isHasOtherValid = false;
        for (var i = 0; i < ComparePageObject.ValidCount; i++) {
            if (index != i && !ComparePageObject.ArrCarInfo[i].IsDel) {
                isHasOtherValid = true;
                break;
            }
        }
        if (isHasOtherValid) {
            if (ComparePageObject.IsIE) {
                ComparePageObject.ArrCarInfo[index].IsDel = true;
                createPageForCompare(ComparePageObject.IsOperateTheSame);
                //解决IE6删除车款 左侧浮动层卡住不动现象
                if (ComparePageObject.IsIE) { $(window).scrollLeft($(window).scrollLeft() - 1); }
            } else {
                $("td[name='td" + index + "'][id!='td_" + index + "']").html("");
                //删除效果
                $(target).closest(".tableHead_item").fadeOut(300, function () {
                    ComparePageObject.ArrCarInfo[index].IsDel = true;
                    createPageForCompare(ComparePageObject.IsOperateTheSame);
                });
            }
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
//优势项
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

}
// 排除相同项
function delTheSameForCompare() {
    showLoading();
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
            //ComparePageObject.IsOperateTheSame = true;
            //changeCheckBoxStateByName("checkboxForDelTheSame", true);
        }
        else {
            createPageForCompare(false);
            //ComparePageObject.IsOperateTheSame = false;
            //changeCheckBoxStateByName("checkboxForDelTheSame", false);
        }
    }
    if (!ComparePageObject.IsOperateTheSame) {
        ComparePageObject.IsOperateTheSame = true;
        changeCheckBoxStateByName("checkboxForDelTheSame", true);
    } else {
        ComparePageObject.IsOperateTheSame = false;
        changeCheckBoxStateByName("checkboxForDelTheSame", false);
    }
}

function showLoading() {
    $("#selectCarLoadingDiv").show();
    setTimeout(function () { $("#selectCarLoadingDiv").hide(); }, 300);
}


// change checkbox state for delete the same param
function changeCheckBoxStateByName(name, state) {
    var checkBoxs = document.getElementsByName(name);
    if (checkBoxs && checkBoxs.length > 0) {
        for (var i = 0; i < checkBoxs.length; i++) {
            checkBoxs[i].checked = state;
            //console.log(name + ":" + state);
        }
    }
}

function setTRColorWhenMouse() {
    $("#CarCompareContent tr:not([id^='params-']):gt(0)").hover(
		function () {
		    $(this).addClass("bg-gray");
            if ($(this).hasClass("multi-row2-end")) {
                $(this).prev().addClass("bg-gray");
            }
		    var xjObj = $(this).find("div.xunjia");
		    if (xjObj.length > 0) {
		        xjObj.find("div.button_43_20").removeClass("button_gray").addClass("button_orange");
		    }
		},
		function () {
            $(this).removeClass("bg-gray");
            if ($(this).hasClass("multi-row2-end")) {
                $(this).prev().removeClass("bg-gray");
            }
		    var xjObj = $(this).find("div.xunjia");
		    if (xjObj.length > 0) {
		        xjObj.find("div.button_43_20").removeClass("button_orange").addClass("button_gray");
		    }
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
//数组扩展方法
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.max = function () { return Math.max.apply({}, this) }
Array.prototype.min = function () { return Math.min.apply({}, this) }
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
            $("#left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: (fixedOffsetLeft - 10) + "px" });
            $("#show-left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: (fixedOffsetLeft - 10) + "px" });
        } else {
            if ($("#left-nav ul li").length > 0) {
                if (currScrollTop > boxOffsetTop) {
                    if (currScrollTop > boxOffsetTop) {
                        if (ComparePageObject.IE6) {
                            $("#left-nav").css({ position: "absolute", top: currScrollTop - 275, left: currScrollLeft - boxOffsetLeft - 10 });
                            $("#show-left-nav").css({ position: "absolute", top: currScrollTop - 275, left: currScrollLeft - boxOffsetLeft });
                        }
                        else {
                            $("#left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: "0px" });
                            $("#show-left-nav").css({ position: "fixed", top: ComparePageObject.MenuOffsetTop + "px", left: (fixedOffsetLeft - 10) + "px" });
                        }
                    } else {
                        $("#left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: "0px" });
                        $("#show-left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: fixedOffsetLeft + "px" });
                    }
                } else {
                    $("#left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: "-70px" });
                    $("#show-left-nav").css({ position: "absolute", top: ComparePageObject.MenuOffsetTop + "px", left: "-70px" });
                }
            }
        }
    }
}
$(function () {
    if (isie(6)) {
        document.getElementById("left-nav").getElementsByTagName("i")[0].style.display = "block";
        setTimeout(function () { document.getElementById("left-nav").getElementsByTagName("i")[0].style.zoom = 1; }, 500);
    }

    //回顶按钮
    $(".left-nav .duibi-return-top").click(function () {
        $("html").animate({ scrollTop: 0 }, 300); //IE,FF
        $("body").animate({ scrollTop: 0 }, 300); //Webkit
    });
    var theid = $("#topfixed");
    var the_lid = $("#leftfixed");
    var thebox = $("#box");
    var thesmall = $("#smallfixed")
    var floatkey;
    //////20110811修改隐藏显示浮动层
    var idmainoffsettop = $("#main_box").offset().top; //id的 offsettop
    var idmainoffsettop_top = idmainoffsettop  //上浮动层出现top定位
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
        //左侧菜单 左卷动 > 居左距离 隐藏
        //if (!ComparePageObject.OneLeftScrollFlag) {
        //	if (scrollsLeft > boxoffsetLeft) {
        //		$("#left-nav").hide();
        //		ComparePageObject.OneLeftScrollFlag = true;
        //	}
        //}

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
        //左侧菜单操作
        mathLeftMenu(scrollsLeft, boxoffsetLeft, scrolls, idmainoffsettop_top);
        ////////////left浮动模式结束///////////////////
        ////////////////控制上下卷动屏幕，出现浮动效果
        //console.log(scrolls);
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
    //$(window).trigger("resize");
    //mathLeftMenu($(window).scrollLeft(), thebox.offset().left, $(window).scrollTop(), idmainoffsettop_top);
    ///////////////////////屏幕卷动结束
    //$("#left-nav a").first().html($("#left-nav a").first().attr("title1") + "<i></i>");
});
var carFuelType = ["汽油", "柴油", "纯电动", "油电混合", "插电混合", "客车", "卡车", "天然气"];

var arrField = [
    { sFieldTitle: "图片", sType: "fieldPic", sPid: "", sFieldIndex: "", sTrPrefix: "1", unit: "", joinCode: "" },
    { sFieldTitle: "基本信息", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carinfo" },
    { sFieldTitle: "厂商指导价", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "", sTrPrefix: "1", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "商家报价", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPrice", sPid: "", sTrPrefix: "1", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "", sTrPrefix: "1", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "排量", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "785", sTrPrefix: "1", sFieldIndex: "4", unit: "L", joinCode: "" },
    { sFieldTitle: "进气形式", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "425", sTrPrefix: "1", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", fuelType: "2", sType: "fieldPara", sPid: "1007", sTrPrefix: "1", sFieldIndex: "", unit: "6", joinCode: "" },
    { sFieldTitle: "燃油变速箱", fuelType: "0,1,3,4,5,6,7", sType: "fieldMulti", sPid: "724,712", sTrPrefix: "1,1", sFieldIndex: "7,8", unit: "挡,", joinCode: ", " },
    { sFieldTitle: "最高车速[km/h]", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "663", sTrPrefix: "1", sFieldIndex: "9", unit: "", joinCode: "", isVantage: "1", size: "1" },

    { sFieldTitle: "车身尺寸", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carbody" },
    { sFieldTitle: "长×宽×高[mm]", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldMulti", sPid: "588,593,586", sTrPrefix: "2,2,2", sFieldIndex: "0,1,2", unit: "", joinCode: ",x,x", isVantage: "1", size: "1" },
    { sFieldTitle: "轴距[mm]", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "592", sTrPrefix: "2", sFieldIndex: "3", unit: "", joinCode: "mm", isVantage: "1", size: "1" },
    { sFieldTitle: "整备质量[kg]", fuelType: "0,1,2,3,4,5,6", sType: "fieldPara", sPid: "669", sTrPrefix: "2", sFieldIndex: "4", unit: "", joinCode: "kg" },
    { sFieldTitle: "座位数[个]", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "665", sTrPrefix: "2", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "行李箱容积[L]", fuelType: "0,1,2,3,4,5,6", sType: "fieldPara", sPid: "465", sTrPrefix: "2", sFieldIndex: "6", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "油箱容积[L]", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "576", sTrPrefix: "2", sFieldIndex: "7", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "前轮胎规格", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "729", sTrPrefix: "2", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后轮胎规格", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "721", sTrPrefix: "2", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "备胎", fuelType: "0,1,2,3,4,5,6", sType: "fieldPara", sPid: "707", sTrPrefix: "2", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "保修政策", fuelType: "0,1,2,3,4,5,6,7", sType: "fieldPara", sPid: "398", sTrPrefix: "2", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "满载质量[kg]", fuelType: "5", sType: "fieldPara", sPid: "668", sTrPrefix: "2", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎规格", fuelType: "5,6", sType: "fieldPara", sPid: "1001", sTrPrefix: "2", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "载重质量[kg]", fuelType: "6", sType: "fieldPara", sPid: "974", sTrPrefix: "2", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "轮胎个数", fuelType: "6", sType: "fieldPara", sPid: "982", sTrPrefix: "2", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "货箱长×宽×高[mm]", fuelType: "6", sType: "fieldMulti", sPid: "966,969,970", sTrPrefix: "2", sFieldIndex: "16,17,18", unit: "mm", joinCode: "x", isVantage: "1", size: "1" },

    { sFieldTitle: "动力系统", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-carengine" },
    { sFieldTitle: "排气量[ml]", fuelType: "0,1,3,4,5,6", sType: "fieldMulti", sPid: "423,785", sTrPrefix: "3,3", sFieldIndex: "0,1", unit: ",L", joinCode: ",ml " }, /*1987ml 2.0L*/
    { sFieldTitle: "最大功率[kW(Ps)/rpm]", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "430", sTrPrefix: "3", sFieldIndex: "2", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "马力(Ps)", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "791", sTrPrefix: "3", sFieldIndex: "3", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大功率转速(rpm)", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "433", sTrPrefix: "3", sFieldIndex: "4", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "最大扭矩[N.m/rpm]", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "429", sTrPrefix: "3", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "最大扭矩转速(rpm)", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "432", sTrPrefix: "3", sFieldIndex: "6", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "缸体形式", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "418", sTrPrefix: "3", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "气缸数[缸]", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "417", sTrPrefix: "3", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "进气形式", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "425", sTrPrefix: "3", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "供油方式", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "580", sTrPrefix: "3", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "压缩比", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "414", sTrPrefix: "3", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "燃油标号[号]", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "577", sTrPrefix: "3", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "发动机启停", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "894", sTrPrefix: "3", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "燃油变速箱类型", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "712", sTrPrefix: "3", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "档位个数", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "724", sTrPrefix: "3", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "最高车速[km/h]", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "663", sTrPrefix: "3", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "0-100km/h加速时间[s]", fuelType: "0,1,3,4,5,6", sType: "fieldPara", sPid: "650", sTrPrefix: "3", sFieldIndex: "17", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "混合工况油耗(L/100km)", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "782", sTrPrefix: "3", sFieldIndex: "18", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "环保标准", fuelType: "0,1,3,4,5,6,7", sType: "fieldPara", sPid: "421", sTrPrefix: "3", sFieldIndex: "19", unit: "", joinCode: "" },
    { sFieldTitle: "电动机总功率[kW]", fuelType: "2,4", sType: "fieldPara", sPid: "870", sTrPrefix: "3", sFieldIndex: "20", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电动机总扭矩[N.m]", fuelType: "2,4", sType: "fieldPara", sPid: "872", sTrPrefix: "3", sFieldIndex: "21", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "前电动机最大功率[kW]", fuelType: "2,4", sType: "fieldPara", sPid: "1002", sTrPrefix: "3", sFieldIndex: "22", unit: "", joinCode: "" },
    { sFieldTitle: "前电动机最大扭矩[N.m]", fuelType: "2,4", sType: "fieldPara", sPid: "1004", sTrPrefix: "3", sFieldIndex: "23", unit: "", joinCode: "" },
    { sFieldTitle: "后电动机最大功率[kW]", fuelType: "2,4", sType: "fieldPara", sPid: "1003", sTrPrefix: "3", sFieldIndex: "24", unit: "", joinCode: "" },
    { sFieldTitle: "后电动机最大扭矩[N.m]", fuelType: "2,4", sType: "fieldPara", sPid: "1005", sTrPrefix: "3", sFieldIndex: "25", unit: "", joinCode: "" },
    { sFieldTitle: "电池容量[kWh]", fuelType: "2,4", sType: "fieldPara", sPid: "876", sTrPrefix: "3", sFieldIndex: "26", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电池充电时间[h]", fuelType: "2,4", sType: "fieldMulti", sPid: "878,879", sTrPrefix: "3", sFieldIndex: "27,28", unit: "", joinCode: "" },
    { sFieldTitle: "耗电量[kWh/100km]", fuelType: "2,4", sType: "fieldPara", sPid: "868", sTrPrefix: "3", sFieldIndex: "29", unit: "", joinCode: "", isVantage: "1", size: "0" },
    { sFieldTitle: "最大续航里程[km]", fuelType: "2,4", sType: "fieldPara", sPid: "883", sTrPrefix: "3", sFieldIndex: "30", unit: "", joinCode: "", isVantage: "1", size: "1" },
    { sFieldTitle: "电池组质保", fuelType: "2,4", sType: "fieldPara", sPid: "1006", sTrPrefix: "3", sFieldIndex: "31", unit: "", joinCode: "" },
    { sFieldTitle: "电动变速箱类型", fuelType: "2", sType: "fieldPara", sPid: "1007", sTrPrefix: "3", sFieldIndex: "32", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合功率[Kw]", fuelType: "2,4", sType: "fieldPara", sPid: "1008", sTrPrefix: "3", sFieldIndex: "33", unit: "", joinCode: "" },
    { sFieldTitle: "系统综合扭矩[N.m]", fuelType: "2,4", sType: "fieldPara", sPid: "1009", sTrPrefix: "3", sFieldIndex: "34", unit: "", joinCode: "" },
    { sFieldTitle: "发动机描述", fuelType: "6", sType: "fieldPara", sPid: "1010", sTrPrefix: "3", sFieldIndex: "35", unit: "", joinCode: "" },
    { sFieldTitle: "卡车变速箱描述", fuelType: "6", sType: "fieldPara", sPid: "1011", sTrPrefix: "3", sFieldIndex: "36", unit: "", joinCode: "" },
    { sFieldTitle: "卡车前进档位个数", fuelType: "6", sType: "fieldPara", sPid: "980", sTrPrefix: "3", sFieldIndex: "37", unit: "", joinCode: "" },
    { sFieldTitle: "卡车倒档位个数", fuelType: "6", sType: "fieldPara", sPid: "981", sTrPrefix: "3", sFieldIndex: "38", unit: "", joinCode: "" },

    { sFieldTitle: "底盘制动", sType: "bar", sPid: "", sTrPrefix: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-bottomstop" },
    { sFieldTitle: "驱动方式", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "655", sTrPrefix: "4", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "前悬架类型", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "728", sTrPrefix: "4", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "前悬架类型", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "720", sTrPrefix: "4", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "可调悬挂", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "708", sTrPrefix: "4", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "前轮制动器类型", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "726", sTrPrefix: "4", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "后轮制动器类型", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "718", sTrPrefix: "4", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "驻车制动类型", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "716", sTrPrefix: "4", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "车体结构", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "572", sTrPrefix: "4", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "差速器/差速锁", fuelType: "0,1,2,3,4", sType: "fieldPara", sPid: "733", sTrPrefix: "4", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "客车前悬架类型", fuelType: "5", sType: "fieldPara", sPid: "1012", sTrPrefix: "4", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "客车后悬架类型", fuelType: "5", sType: "fieldPara", sPid: "1013", sTrPrefix: "4", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "卡车驱动形式", fuelType: "6", sType: "fieldPara", sPid: "1014", sTrPrefix: "4", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "前桥描述", fuelType: "6", sType: "fieldPara", sPid: "975", sTrPrefix: "4", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "前桥允许载荷[kg]", fuelType: "6", sType: "fieldPara", sPid: "1015", sTrPrefix: "4", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后桥描述", fuelType: "6", sType: "fieldPara", sPid: "976", sTrPrefix: "4", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后桥允许载荷[kg]", fuelType: "6", sType: "fieldPara", sPid: "1016", sTrPrefix: "4", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "安全配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-safeconfig" },
    { sFieldTitle: "防抱死制动(ABS)", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "673", sTrPrefix: "5", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "制动力分配(EBD/CBC等)", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "685", sTrPrefix: "5", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "制动辅助(BA/EBA/BAS等)", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "684", sTrPrefix: "5", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "牵引力控制(ARS/TCS/TRC等)", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "698", sTrPrefix: "5", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "车身稳定控制(ESP/DSC/VSC等)", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sTrPrefix: "5", sPid: "700", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "主驾驶安全气囊", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "682", sTrPrefix: "5", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "副驾驶安全气囊", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "697", sTrPrefix: "5", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "前侧气囊", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "691", sTrPrefix: "5", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "后侧气囊", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "680", sTrPrefix: "5", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "侧安全气帘", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "690", sTrPrefix: "5", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "膝部气囊", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "835", sTrPrefix: "5", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "安全带气囊", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "845", sTrPrefix: "5", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "后排中央气囊", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1017", sTrPrefix: "5", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "胎压监测", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "714", sTrPrefix: "5", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "零胎压续航轮胎", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "715", sTrPrefix: "5", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后排儿童座椅接口(ISO FIX/LATCH)", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "495", sTrPrefix: "5", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "驾驶辅助", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-drivingassistance" },
    { sFieldTitle: "定速巡航", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "545", sTrPrefix: "6", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "车道保持/并线辅助", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sTrPrefix: "6", sPid: "898", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "碰撞报警/主动刹车", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sTrPrefix: "6", sPid: "818", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "疲劳提醒", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1018", sTrPrefix: "6", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "自动泊车", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "816", sTrPrefix: "6", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "遥控泊车", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "901", sTrPrefix: "6", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "自动驾驶辅助", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1019", sTrPrefix: "6", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "自动驻车", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "811", sTrPrefix: "6", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "上坡辅助", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "812", sTrPrefix: "6", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "陡坡缓降", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "813", sTrPrefix: "6", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "夜视系统", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "819", sTrPrefix: "6", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "可变齿比转向", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1020", sTrPrefix: "6", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "前倒车雷达", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "800", sTrPrefix: "6", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "后倒车雷达", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "702", sTrPrefix: "6", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "倒车影像", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "703", sTrPrefix: "6", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "驾驶模式选择", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1021", sTrPrefix: "6", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "外部配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-outerconfig" },
    { sFieldTitle: "前大灯", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "614", sTrPrefix: "7", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "LED日间行车灯", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "794", sTrPrefix: "7", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "自动大灯", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "609", sTrPrefix: "7", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "前雾灯", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "607", sTrPrefix: "7", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "大灯功能", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "612", sTrPrefix: "7", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "天窗类型", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "567", sTrPrefix: "7", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "前电动车窗", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "601", sTrPrefix: "7", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "后电动车窗", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "1038", sTrPrefix: "7", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜电动调节", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "622", sTrPrefix: "7", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜加热", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "624", sTrPrefix: "7", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "内后视镜自动防炫目", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "621", sTrPrefix: "7", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "外后视镜自动防炫目", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "1022", sTrPrefix: "7", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "隔热/隐私玻璃", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "796", sTrPrefix: "7", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "后排侧遮阳帘", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "797", sTrPrefix: "7", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后遮阳帘", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "595", sTrPrefix: "7", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "前雨刷器", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "606", sTrPrefix: "7", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "后雨刷器", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "596", sTrPrefix: "7", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "电吸门", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "821", sTrPrefix: "7", sFieldIndex: "17", unit: "", joinCode: "" },
    { sFieldTitle: "电动侧滑门", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "1023", sTrPrefix: "7", sFieldIndex: "18", unit: "", joinCode: "" },
    { sFieldTitle: "电动行李箱", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "556", sTrPrefix: "7", sFieldIndex: "19", unit: "", joinCode: "" },
    { sFieldTitle: "车顶行李架", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "627", sTrPrefix: "7", sFieldIndex: "20", unit: "", joinCode: "" },
    { sFieldTitle: "中控锁", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "493", sTrPrefix: "7", sFieldIndex: "21", unit: "", joinCode: "" },
    { sFieldTitle: "智能钥匙", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "952", sTrPrefix: "7", sFieldIndex: "22", unit: "", joinCode: "" },
    { sFieldTitle: "远程遥控功能", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "1024", sTrPrefix: "7", sFieldIndex: "23", unit: "", joinCode: "" },
    { sFieldTitle: "尾翼/扰流板", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1025", sTrPrefix: "7", sFieldIndex: "24", unit: "", joinCode: "" },
    { sFieldTitle: "运动外观套件", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "793", sTrPrefix: "7", sFieldIndex: "25", unit: "", joinCode: "" },

    { sFieldTitle: "内部配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-innerconfig" },
    { sFieldTitle: "内饰材质", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "1026", sTrPrefix: "8", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "车内氛围灯", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "795", sTrPrefix: "8", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "遮阳板化妆镜", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "512", sTrPrefix: "8", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘材质", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "548", sTrPrefix: "8", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "多功能方向盘", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "528", sTrPrefix: "8", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘调节", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "799", sTrPrefix: "8", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘加热", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "956", sTrPrefix: "8", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "方向盘换挡", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "574", sTrPrefix: "8", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "前排空调", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "839", sTrPrefix: "8", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后排空调", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "838", sTrPrefix: "8", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "香氛系统", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1027", sTrPrefix: "8", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "空气净化", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "905", sTrPrefix: "8", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "车载冰箱", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "485", sTrPrefix: "8", sFieldIndex: "12", unit: "", joinCode: "" },

    { sFieldTitle: "座椅配置", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-chair" },
    { sFieldTitle: "座椅材质", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "544", sTrPrefix: "9", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "运动风格座椅", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "546", sTrPrefix: "9", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "主副座椅电动调节", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "508", sTrPrefix: "9", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "副座椅电动调节", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "503", sTrPrefix: "9", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "主座椅调节方式", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "1028", sTrPrefix: "9", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "副座椅调节方式", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "1029", sTrPrefix: "9", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "第二排座椅电动调节", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "833", sTrPrefix: "9", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "第二排座椅调节方式", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "1030", sTrPrefix: "9", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "前排座椅功能", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "504", sTrPrefix: "9", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "后排座椅功能", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "1031", sTrPrefix: "9", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "前排中央扶手", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "514", sTrPrefix: "9", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "后排中央扶手", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "475", sTrPrefix: "9", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "第三排座椅", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "805", sTrPrefix: "9", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "座椅放倒方式", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "482", sTrPrefix: "9", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "后排杯架", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "474", sTrPrefix: "9", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "后排折叠桌版", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1032", sTrPrefix: "9", sFieldIndex: "15", unit: "", joinCode: "" },

    { sFieldTitle: "信息娱乐", sType: "bar", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-pastime" },
    { sFieldTitle: "中控彩色液晶屏", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "488", sTrPrefix: "10", sFieldIndex: "0", unit: "", joinCode: "" },
    { sFieldTitle: "全液晶仪表盘", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "988", sTrPrefix: "10", sFieldIndex: "1", unit: "", joinCode: "" },
    { sFieldTitle: "行车电脑显示屏", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "832", sTrPrefix: "10", sFieldIndex: "2", unit: "", joinCode: "" },
    { sFieldTitle: "HUD平视显示", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "518", sTrPrefix: "10", sFieldIndex: "3", unit: "", joinCode: "" },
    { sFieldTitle: "GSP导航", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "516", sTrPrefix: "10", sFieldIndex: "4", unit: "", joinCode: "" },
    { sFieldTitle: "智能互联定位", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1033", sTrPrefix: "10", sFieldIndex: "5", unit: "", joinCode: "" },
    { sFieldTitle: "语音控制", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1035", sTrPrefix: "10", sFieldIndex: "6", unit: "", joinCode: "" },
    { sFieldTitle: "手机互联(Carplay&Android)", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "1036", sTrPrefix: "10", sFieldIndex: "7", unit: "", joinCode: "" },
    { sFieldTitle: "手机无线充电", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1037", sTrPrefix: "10", sFieldIndex: "8", unit: "", joinCode: "" },
    { sFieldTitle: "手势控制系统", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "1034", sTrPrefix: "10", sFieldIndex: "9", unit: "", joinCode: "" },
    { sFieldTitle: "CD/DVD", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "510", sTrPrefix: "10", sFieldIndex: "10", unit: "", joinCode: "" },
    { sFieldTitle: "蓝牙/WIFI连接", fuelType: "0,1,2,3,4,7", sType: "fieldMultiValue", sPid: "479", sTrPrefix: "10", sFieldIndex: "11", unit: "", joinCode: "" },
    { sFieldTitle: "外接接口", fuelType: "0,1,2,3,4,5,7", sType: "fieldMultiValue", sPid: "810", sTrPrefix: "10", sFieldIndex: "12", unit: "", joinCode: "" },
    { sFieldTitle: "车载电视", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "559", sTrPrefix: "10", sFieldIndex: "13", unit: "", joinCode: "" },
    { sFieldTitle: "音响品牌", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "473", sTrPrefix: "10", sFieldIndex: "14", unit: "", joinCode: "" },
    { sFieldTitle: "扬声器数量(个)", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "523", sTrPrefix: "10", sFieldIndex: "15", unit: "", joinCode: "" },
    { sFieldTitle: "后排液晶屏/娱乐系统", fuelType: "0,1,2,3,4,7", sType: "fieldPara", sPid: "477", sTrPrefix: "10", sFieldIndex: "16", unit: "", joinCode: "" },
    { sFieldTitle: "车载220V电源", fuelType: "0,1,2,3,4,5,7", sType: "fieldPara", sPid: "467", sTrPrefix: "10", sFieldIndex: "17", unit: "", joinCode: "" },

    { sFieldTitle: "选装包", sType: "optional", sPid: "", sFieldIndex: "", unit: "", joinCode: "", scrollId: "params-optional" }
];
// page method --------------------------
var arrField2 = [
   { sType: "fieldPic", sFieldIndex: "", sFieldTitle: "图片", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息", sPid: "", sTrPrefix: "1", unit: "", joinCode: "", scrollId: "params-carinfo" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "厂商指导价", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
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

//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}

//元素居中
!function ($) {
    $.fn.showCenter = function () {
        var top = (($(window).height() - this.height()) / 2) + 18;
        var left = ($(window).width() - this.width()) / 2;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();

        if (ComparePageObject.IE6) {
            return this.css({ position: 'absolute', 'z-index': 1100, 'top': top + scrollTop + 60, left: left + scrollLeft }).show();
        }
        else {
            return this.css({ position: 'fixed', 'z-index': 1100, 'top': "218px", left: left }).show();
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
	      //var adHeight = 0;
	      //if ($("body").data("adHeight")) {
	      //    adHeight = $("body").data("adHeight");
	      //}
	      this.offsets = $([]);
	      this.targets = $([]);

	      $targets = this.$body
            .find(this.selector)
            .map(function (i, n) {
                var $el = $(this),
                targetName = $el.data('target'),
                targetHeight = $el.height(),
                $targetElement = $("#" + targetName);
                if ($targetElement
                  && ($targetElement.length > 0)) {
                    var ElementScrollTop = ($targetElement.offset().top + self.options.offset - (i * (targetHeight + 1)) - 40);
                    if (scrollTop >= ElementScrollTop) {
                        self.activate(targetName)
                    }
                    $el.unbind("click");
                    $el.bind("click", function (e) {
                        e.preventDefault();
                        ComparePageObject.OneLeftScrollFlag = false;
                        $("html,body").animate({ scrollTop: ElementScrollTop + 1 }, 300, function () {
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
	      //modified by sk 2014.03.11 页面长度不够 停留当前位置 不选中最后一项
	      //	  	if (scrollTop >= maxScroll) {
	      //	  		return activeTarget != (i = targets.last()[0])
	      //			  && this.activate(i)
	      //	  	}
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

	      /*$("#left-nav").find("I").each(function () {
              $(this).parent().html($(this).parent().attr("title1"));
          });
          $(currSelector).html($(currSelector).html() + "<i></i>");*/
	      //$("#left-nav").find("i").each(function () {
	      //    $(this).hide();
	      //});
	      //$(currSelector).find("i").show();
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
            _this.on('click', '.change-car-area .btn-pre-car', function (e) {
                e.stopPropagation();
                if (ComparePageObject.IsChange) return;
                ComparePageObject.IsChange = true;
                var parent = $(this).closest('.tableHead_item').find("div[id^='draggcarbox_'],div[id^='FloatTop_carBox_']").eq(0);
                if ($(parent).length == 0) {
                    return;
                }
                var prevItem = $(this).closest('.tableHead_item').parent().prev().find("div[id^='draggcarbox_'],div[id^='FloatTop_carBox_']").eq(0);
                if ($(prevItem).length == 0) return;
                var index = $(parent).attr("id").replace("draggcarbox_", "").replace("FloatTop_carBox_", "");
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

                parentCloneElement.insertAfter(parent).css({ position: 'absolute', display: 'block', 'z-index': '1000' }).animate({ left: parentLeft - 201 }, option.speed, function () {
                    parent.insertBefore(prevCloneElement).css('display', 'block');
                    animateCallback(prevCloneElement);
                    //标记可切换
                    ComparePageObject.IsChange = false;

                    if (index && option["leftCallback"]) {
                        option["leftCallback"](index);
                    }
                });
                prevCloneElement.insertAfter(prevItem).css({ position: 'absolute', display: 'block', 'z-index': '999' }).animate({ left: parentLeft + 201 }, option.speed, function () {
                    prevItem.insertBefore(parentCloneElement).css('display', 'block');
                    animateCallback(parentCloneElement);
                });
            });
            _this.on('click', '.change-car-area .btn-next-car', function (e) {
                e.stopPropagation();
                if (ComparePageObject.IsChange) return;
                ComparePageObject.IsChange = true;
                var parent = $(this).closest('.tableHead_item').find("div[id^='draggcarbox_'],div[id^='FloatTop_carBox_']").eq(0);
                if ($(parent).length == 0) {
                    return;
                }
                var nextItem = $(this).closest('.tableHead_item').parent().next().find("div[id^='draggcarbox_'],div[id^='FloatTop_carBox_']").eq(0);
                var index = $(parent).attr("id").replace("draggcarbox_", "").replace("FloatTop_carBox_", "");
                if ($(nextItem).length == 0) return;

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
                parentCloneElement.insertAfter(parent).css({ position: 'absolute', display: 'block', 'z-index': '1000' }).animate({ left: parentLeft + 201 }, option.speed, function () {
                    parent.insertBefore(nextItem).css('display', 'block');
                    animateCallback(nextCloneElement);
                    //标记可切换
                    ComparePageObject.IsChange = false;
                    if (index && option["rightCallback"]) {
                        option["rightCallback"](index);
                    }
                });

                nextCloneElement.insertAfter(nextItem).css({ position: 'absolute', display: 'block', 'z-index': '999' }).animate({ left: nextItemLeft - 201 }, option.speed, function () {
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