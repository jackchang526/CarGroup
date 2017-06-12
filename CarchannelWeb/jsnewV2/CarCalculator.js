// JScript File
//------------------------common function---------------------------------------
function SetSpanValueByBrowerType(control, value) {
    $("#" + control).html(value);
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
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num);
}
//35.3->353000
function formatCurrencyWToK(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num)) num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 10000 + 0.50000000001).toString();
    return (((sign) ? '' : '-') + num);
}
//4.784->4784
function GetIntValue(num) {
    num = num.toString().replace(/\,/g, '');
    return parseInt(num);
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//------------------ Common ------------------------------

//=========弹出提示=============================
var preText = null;
function showjs(j) {
    //if (preText)
    //    preText[0].style.display = 'none';
    //preText = $("#" + j);
    //$("#" + j)[0].style.display = '';

    //var height = $("#" + j).children("div").height();
    //$("#" + j).find("iframe").css("height", height);

    $("#" + j).show();
}

function closex(t) {
    event = event || window.event;
    event.preventDefault(); $("#" + t)[0].style.display = 'none';
}

//车船使用税减免信息
var vehicleAndVesselTaxRelief;
//排量
var exhaustforfloat;
//车船使用税信息
var vehicleAndVesselTaxInfos = {
    1: {
        Level: 1,
        MinDisplacement: 0,
        MaxDisplacement: 1.0,
        DisplacementDescription: "1.0L(含)以下",
        Tax: 300
    },
    2: {
        Level: 2,
        MinDisplacement: 1.0,
        MaxDisplacement: 1.6,
        DisplacementDescription: "1.0-1.6L(含)",
        Tax: 420,
        IsDefault: true
    },
    3: {
        Level: 3,
        MinDisplacement: 1.6,
        MaxDisplacement: 2.0,
        DisplacementDescription: "1.6-2.0L(含)",
        Tax: 480
    },
    4: {
        Level: 4,
        MinDisplacement: 2.0,
        MaxDisplacement: 2.5,
        DisplacementDescription: "2.0-2.5L(含)",
        Tax: 900
    },
    5: {
        Level: 5,
        MinDisplacement: 2.5,
        MaxDisplacement: 3.0,
        DisplacementDescription: "2.5-3.0L(含)",
        Tax: 1920
    },
    6: {
        Level: 6,
        MinDisplacement: 3.0,
        MaxDisplacement: 4.0,
        DisplacementDescription: "3.0-4.0L(含)",
        Tax: 3480
    },
    7: {
        Level: 7,
        MinDisplacement: 4.0,
        MaxDisplacement: Number.MAX_VALUE,
        DisplacementDescription: "4.0L以上",
        Tax: 5280
    }
};

//初始化车船使用税控件
function InitVehicleAndVesselTaxControl(controlContainer) {
    var buffer = [];
    buffer.push("<div class=\"form-group\">");
    buffer.push("<div class=\"input-group\">");
    buffer.push("<div id=\"vehicleTax\" class=\"input input-w-sm\" style=\"cursor: pointer\" onclick=\"showCheChuanList()\">");
    for (var taxLevelOne in vehicleAndVesselTaxInfos) {
        if (vehicleAndVesselTaxInfos[taxLevelOne].IsDefault) {
            buffer.push("<span id=" + taxLevelOne + ">" + vehicleAndVesselTaxInfos[taxLevelOne].DisplacementDescription + "</span>");
        }
    }
    buffer.push("<i class=\"input-arrow-down\"></i>");
    buffer.push("<div class=\"drop-layer\" style=\"display: none\" >");
    for (var taxLevelTwo in vehicleAndVesselTaxInfos) {
        buffer.push("<a id=" + taxLevelTwo + " href=\"javascript:\">" + vehicleAndVesselTaxInfos[taxLevelTwo].DisplacementDescription + "</a>");
    }
    buffer.push("</div>");
    buffer.push("</div>");
    buffer.push("</div>");
    buffer.push("</div>");
    document.getElementById(controlContainer).innerHTML = buffer.join("");
};



//根据排量获得车船使用税信息
function GetVehicleAndVesselTaxInfo(dispplacement) {
    for (var taxLevel in vehicleAndVesselTaxInfos) {
        if (dispplacement > vehicleAndVesselTaxInfos[taxLevel].MinDisplacement
            && dispplacement <= vehicleAndVesselTaxInfos[taxLevel].MaxDisplacement) {
            return vehicleAndVesselTaxInfos[taxLevel];
        }
    }
}

//-----------------------------------------
function resetPrice(carId, webSiteBaseUrl) {
    //清空车船税减免信息
    if (carId == -2) {
        return;
    }
    if (-1 != carId) {
        //var carReferPrice = jQuery("a[bita-value=" + carId + "]").children("strong").html();
        //carReferPrice = carReferPrice.substring(0, carReferPrice.length - 1);

        var carReferPrice = jQuery("#hidCarPrice").val();
        $('#txtMoney').val(formatCurrencyWToK(carReferPrice));
        $('#txtChePai').val(500);
        //通过车型选择的
        //自动匹配排量，座位数量，玻璃单独破碎险的进口或国产。
        //购置税中排量对应车型库中的排量，车船使用税和交强险中使用的座位数对应基本性能中的成员人数（含司机）；
        //如果库中的数据为空则使用默认设置。
        var data = { "carId": carId };
        $.ajax({
            url: webSiteBaseUrl + "ajaxnew/GetCarInfoForCalcTools.aspx?type=json",
            type: "get",
            data: data,
            async: false,
            success: function (res) {
                var myData = eval("(" + res + ")");

                //交强险
                var selCompulsory = document.getElementById("selCompulsory");
                if (myData[0].seatNum != "0") {
                    var nRS = parseInt(myData[0].seatNum);
                    document.getElementById("hidSeatNum").value = nRS;
                    if (nRS < 6) {
                        $(selCompulsory).children("span").text("家用6座以下").attr("id", 950);
                    } else {
                        $(selCompulsory).children("span").text("家用6座及以上").attr("id", 1100);
                    }
                } else {
                    $(selCompulsory).children("span").text("家用6座以下").attr("id", 950);
                }
                //玻璃单独破碎险
                var selBreakageOfGlass = document.getElementById("selBreakageOfGlass");
                if (myData[0].isGuoChan != "") {
                    if (myData[0].isGuoChan == "False") {
                        $(selBreakageOfGlass).children("span").text("进口").attr("id", 0);
                    } else {
                        $(selBreakageOfGlass).children("span").text("国产").attr("id", 1);
                    }
                } else {
                    $(selBreakageOfGlass).children("span").text("国产").attr("id", 1);
                }

                //排量 购置税计算使用(对购买1.6升及以下排量乘用车实施减半征收车辆购置税的优惠政策)
                exhaustforfloat = myData[0].exhaustforfloat;
                if (exhaustforfloat == "" || parseFloat(exhaustforfloat) <= 0) {
                    $("#vehicleTax").children("span").attr("id", 2).text(vehicleAndVesselTaxInfos[2].DisplacementDescription);
                } else {
                    var vehicleAndVesselTaxInfo = GetVehicleAndVesselTaxInfo(exhaustforfloat);
                    $("#vehicleTax").children("span").attr("id", vehicleAndVesselTaxInfo.Level).text(vehicleAndVesselTaxInfos[vehicleAndVesselTaxInfo.Level].DisplacementDescription);
                }
                //车船使用税减免信息
                vehicleAndVesselTaxRelief = myData[0].traveltax;
                //计算车船使用税
                CalculateVehAndVesselTax();
                //setCalcToolUrl(carId);
            }
        });
    }
}

function resetPriceInsurance(carId, webSiteBaseUrl) {
    $('#txtMoney').val(0);
    if (carId == -2) {
        return;
    }

    if (-1 != carId) {
        //var carReferPrice = jQuery("a[bita-value=" + carId + "]").children("strong").html();
        //carReferPrice = carReferPrice.substring(0, carReferPrice.length - 1);
        var carReferPrice = jQuery("#hidCarPrice").val();
        jQuery('#txtMoney').val(formatCurrencyWToK(carReferPrice));

        //通过车型选择的
        //自动匹配排量，座位数量，玻璃单独破碎险的进口或国产。
        //购置税中排量对应车型库中的排量，车船使用税和交强险中使用的座位数对应基本性能中的成员人数（含司机）；
        //如果库中的数据为空则使用默认设置。
        var data = { "carId": carId };
        $.ajax({
            url: webSiteBaseUrl + "/car/ajaxnew/GetCarInfoForCalcTools.aspx?type=json",
            type: "get",
            data: data,
            async: false,
            success: function (res) {
                var myData = eval("(" + res + ")");

                //交强险
                var selCompulsory = document.getElementById("selCompulsory");
                if (myData[0].seatNum != "0") {
                    var nRS = parseInt(myData[0].seatNum);
                    document.getElementById("hidSeatNum").value = nRS;
                    if (nRS < 6) {
                        $(selCompulsory).children("span").text("家用6座以下").attr("id",950);
                    } else {
                        $(selCompulsory).children("span").text("家用6座及以上").attr("id", 1100);
                    }
                } else {
                    $(selCompulsory).children("span").text("家用6座以下").attr("id", 950);
                }
                //玻璃单独破碎险
                var selBreakageOfGlass = document.getElementById("selBreakageOfGlass");
                if (myData[0].isGuoChan != "") {
                    if (myData[0].isGuoChan == "False") {
                        $(selBreakageOfGlass).children("span").text("进口").attr("id", 0);
                    } else {
                        $(selBreakageOfGlass).children("span").text("国产").attr("id", 1);
                    }
                } else {
                    $(selBreakageOfGlass).children("span").text("国产").attr("id", 1);
                }
                setCalcToolUrl(carId);
            }
        });
    }
}

function setCalcToolUrlByPrice(hidCarPrice) {
    var ulEle = document.getElementById("calcTools");
    if (!ulEle)
        return;
    var aLinks = ulEle.getElementsByTagName("A");
    for (i = 0; i < aLinks.length; i++) {
        var aLink = aLinks[i];
        var url = aLink.href;
        if (url.length == 0)
            continue;
        var paraIndex = url.indexOf("?");
        if (paraIndex > 0)
            url = url.substring(0, paraIndex);
        url += "?CarPrice=" + hidCarPrice;
        aLink.href = url;
    }
}

//必要花费
function calcEssentialCost() {
    var commonTotal = 0;
    commonTotal += parseInt(calcAcquisitionTax());
    if ($("#txtChePai").val().length == 0) { //光标在文本框中 backspace也需要实时计算
        commonTotal += 0;
    } else {
        commonTotal += parseInt($("#txtChePai").val());
    }
    if ($("#txtVehicleTax").val().length == 0) { //光标在文本框中 backspace也需要实时计算
        commonTotal += 0;
    } else {
        commonTotal += parseInt($("#txtVehicleTax").val());
    }
    commonTotal += GetIntValue(jQuery("#lblCompulsory").html());
    SetSpanValueByBrowerType('essentialCost', formatCurrency(commonTotal));
    return commonTotal;
}

//此函数不放入calcAutoCashAll或calcAutoLoanAll 避免手输值时被重置
function InitCheChuanValue() {
    if ($('#txtMoney').val() == 0) {
        $('#txtVehicleTax').val(0); //车船使用税
    }
    //车船使用税
    CalculateVehAndVesselTax();
}

//计算车船使用税
function CalculateVehAndVesselTax() {
    var taxLevel = $("#vehicleTax").children("span").attr("id");
    var vehicleAndVesselTaxValue = vehicleAndVesselTaxInfos[taxLevel].Tax;
    //车船使用税一般只能缴纳当年的，按月计算
    //vehicleAndVesselTaxValue = vehicleAndVesselTaxValue * (12 - new Date().getMonth()) / 12;
    //计算车船使用税减免
    if (vehicleAndVesselTaxRelief == "免征") {
        vehicleAndVesselTaxValue = 0;
    }
    else if (vehicleAndVesselTaxRelief == "减半") {
        vehicleAndVesselTaxValue = vehicleAndVesselTaxValue / 2;
    }
    $('#txtVehicleTax').val(Math.ceil(vehicleAndVesselTaxValue));
}

function setCalcToolUrl(carId, carPrice) {
    carId = parseInt(carId);
    var ulEle = document.getElementById("calcTools");
    if (!ulEle)
        return;
    var aLinks = ulEle.getElementsByTagName("A");
    for (i = 0; i < aLinks.length; i++) {
        var aLink = aLinks[i];
        var url = aLink.href;
        if (url.length == 0)
            continue;
        var paraIndex = url.indexOf("?");
        if (paraIndex > 0)
            url = url.substring(0, paraIndex);
        if (carId > 0) {
            url += "?carid=" + carId;
            if (carPrice > 0) {
                url += "&CarPrice=" + carPrice;
            }
        } else {
            if ($('#txtMoney').val() != "0") {
                url += "?CarPrice=" + $('#txtMoney').val();
            } else {
                if (aLink.id == "xinche_index") url = "/gouchejisuanqi/";
                if (aLink.id == "xinche_ssxc") url = "/qichedaikuanjisuanqi/";
                if (aLink.id == "xinche_1822") url = "/qichebaoxianjisuan/";
            }
        }
        aLink.href = url;
    }
}

//购置税：购置附加税＝购车款／（1＋17％）× 购置税率（税率：10%）
function calcAcquisitionTax() {
    var acquisitionTax = parseFloat($('#txtMoney').val()) / (1 + 0.17);
    var result = Math.round(acquisitionTax * 0.1);
    if (exhaustforfloat == "" || parseFloat(exhaustforfloat) == 0) {
        result = 0;
        $("#gouZhiShuiDesc").text("免征购置税");
    } else {
        if (parseFloat(exhaustforfloat) <= 1.6) {
            var beginTime = new Date('2017/01/01 00:00:00').getTime();
            var endTime = new Date('2017/12/31 23:59:59').getTime();
            var currentDate = new Date().getTime();
            if (currentDate > beginTime && currentDate < endTime) {
                result = Math.round(acquisitionTax * 0.075);
            }
        }
        $("#gouZhiShuiDesc").text("");
    }
    SetSpanValueByBrowerType("lblAcquisitionTax", formatCurrency(result));
    return result;
}

//商业保险统计
function calcBusinessTotal() {
    var commonTotal = 0;
    if ($('#chkTPL').prop("checked")) {
        commonTotal += parseFloat(GetIntValue($("#lblTPL").html()));
    }
    if ($('#chkCarDamage').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblCarDamage').html());
    }
    if ($('#chkCarTheft').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblCarTheft').html());
    }
    if ($('#chkBreakageOfGlass').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblBreakageOfGlass').html());
    }
    if ($('#chkSelfignite').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblSelfignite').html());
    }
    if ($('#chkAbatement').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblAbatement').html());
    }
    if ($('#chkLimitofPassenger').prop("checked")) {
        commonTotal += parseFloat(jQuery('#lblLimitOfPassenger').html());
    }
    if ($('#chkLimitofDriver').prop("checked")) {
        commonTotal += parseFloat(jQuery('#lblLimitOfDriver').html());
    }
    if ($('#chkCarDamageDW').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#lblCarDamageDW').html());
    }
    if ($('#chkEngine').prop("checked")) {
        commonTotal += GetIntValue(jQuery('#engineDamage').html());
    }
    SetSpanValueByBrowerType('lblCommonTotal', formatCurrency(Math.round(commonTotal)));
    return commonTotal;
}
//交强险
function calcCompulsory() {
    if ($('#txtMoney').val() == 0) {
        return;
    }
    var compulsoryValue = $("#selCompulsory").children("span").attr("id");
    SetSpanValueByBrowerType('lblCompulsory', formatCurrency(compulsoryValue));
}
//第三者责任险
function calcTPL() {
    var selCompulsoryValue = $("#selCompulsory").children("span").attr("id");;
    if ($('#chkTPL').prop("checked")) {
        if ($('#txtMoney').val() == 0) {
            return;
        }
        var selTPLValue = $("#selTPL").children("span").attr("id");
        if (selCompulsoryValue == 950) {
            if (selTPLValue == 50000) {
                jQuery("#lblTPL").html("710");
                return 710;
            }
            if (selTPLValue == 100000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1026));
                return 1026;
            }
            if (selTPLValue == 200000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1270));
                return 1270;
            }
            if (selTPLValue == 500000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1721));
                return 1721;
            }
            if (selTPLValue == 1000000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(2242));
                return 2242;
            }
        } else if (selCompulsoryValue == 1100) {
            if (selTPLValue == 50000) {
                jQuery("#lblTPL").html("659");
                return 659;
            }
            if (selTPLValue == 100000) {
                jQuery("#lblTPL").html("928");
                return 928;
            }
            if (selTPLValue == 200000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1131));
                return 1131;
            }
            if (selTPLValue == 500000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1507));
                return 1507;
            }
            if (selTPLValue == 1000000) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1963));
                return 1963;
            }
        }
    }
    else {
        jQuery('#lblTPL').html("0");
    }
}
//车辆损失险
function calcCarDamage() {
    if ($('#chkCarDamage').prop("checked")) {
        if ($('#txtMoney').val() == 0) {
            return;
        }
        var rate = 0.0095;
        var baseCost = 285;
        //没选车
        if (parseInt($("#hidCarID").val()) <= 0) {
            var compulsoryVal = $("#selCompulsory").children("span").attr("id");
            if (compulsoryVal == 1100) { //6座及以上
                rate = 0.009;
                baseCost = 342;
            }
        } else { //选车
            var seatNum = document.getElementById("hidSeatNum").value;
            if (seatNum >= 6 && seatNum < 10) {
                rate = 0.009;
                baseCost = 342;
            } else if (seatNum >= 10 && seatNum < 20) {
                rate = 0.0095;
                baseCost = 342;
            } else if (seatNum >= 20) {
                rate = 0.0095;
                baseCost = 357;
            }
        }
        var result = Math.round($('#txtMoney').val() * rate + baseCost);
        SetSpanValueByBrowerType('lblCarDamage', formatCurrency(result));
    } else {
        jQuery('#lblCarDamage').html("0");
    }
}

//发动机特别损失险(车损险*5%)
function calcCarEngineDamage() {
    if ($('#chkCarDamage').prop("checked")) {
        $('#chkEngine').parent().attr("class", "");
        if ($("#chkEngine").prop("checked")) {
            var cDamage = GetIntValue(jQuery("#lblCarDamage").html()) * 0.05;
            SetSpanValueByBrowerType('engineDamage', formatCurrency(Math.round(cDamage)));
        } else {
            jQuery("#engineDamage").html("0");
        }
    } else {
        $('#chkEngine').parent().attr("class", "no_click");
        $('#chkEngine').attr("checked", false);
        jQuery("#engineDamage").html("0");
    }
}
//全车盗抢险
function calcCarTheft() {
    if ($('#chkCarDamage').prop("checked")) {
        $('#chkCarTheft').parent().attr("class", "");
        if ($('#chkCarTheft').prop("checked")) {
            if ($('#txtMoney').val() == 0) {
                return;
            }
            var selCompulsoryValue = $("#selCompulsory").children("span").attr("id");
            if (selCompulsoryValue == 1100) {
                SetSpanValueByBrowerType('lblCarTheft', formatCurrency(Math.round($('#txtMoney').val() * 0.0044 + 140)));
            } else {
                SetSpanValueByBrowerType('lblCarTheft', formatCurrency(Math.round($('#txtMoney').val() * 0.0049 + 120)));
            }
        } else {
            jQuery("#lblCarTheft").html("0");
        }
    } else {
        $('#chkCarTheft').parent().attr("class", "no_click");
        $('#chkCarTheft').attr("checked", false);
        jQuery("#lblCarTheft").html("0");
    }
}
//玻璃单独破碎险
function calcBreakageOfGlass() {
    if ($('#chkCarDamage').prop("checked")) {
        $('#chkBreakageOfGlass').parent().attr("class", "");
        if ($('#chkBreakageOfGlass').prop("checked")) {
            var breakageOfGlassValue = $("#selBreakageOfGlass").children("span").attr("id");
            var selCompulsoryValue = $("#selCompulsory").children("span").attr("id");
            if (breakageOfGlassValue == 0)//进口
                if (selCompulsoryValue == 1100) { //6-10座客车
                    SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.003)));
                } else {
                    SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.0031)));
                }
            if (breakageOfGlassValue == 1)//国产
                SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.0019)));
        } else {
            jQuery("#lblBreakageOfGlass").html("0");
        }
    } else {
        $('#chkBreakageOfGlass').parent().attr("class", "no_click");
        $('#chkBreakageOfGlass').attr("checked", false);
        jQuery("#lblBreakageOfGlass").html("0");
    }
}

//自燃损失险
function calcSelfignite() {
    if ($('#chkSelfignite').prop("checked")) {
        SetSpanValueByBrowerType('lblSelfignite', formatCurrency(Math.round($('#txtMoney').val() * 0.0015)));
    }
    else {
        jQuery('#lblSelfignite').html("0");
    }
}
//不计免赔特约险
function calcAbatement() {
    if ($('#chkCarDamage').prop("checked") && $('#chkTPL').prop("checked")) {
        $('#chkAbatement').parent().attr("class", "");
        if ($('#chkAbatement').prop("checked")) {
            var total = GetIntValue(jQuery("#lblCarDamage").html()) + GetIntValue($("#lblTPL").html());
            SetSpanValueByBrowerType('lblAbatement', formatCurrency(Math.round(total * 0.2)));
        }
        else {
            $("#lblAbatement").html("0");
        }
    } else {
        $('#chkAbatement').parent().attr("class", "no_click");
        $('#chkAbatement').attr("checked", false);
        $("#lblAbatement").html("0");
    }


}

function calcBlameless() {
    if ($('#chkTPL').prop("checked") && $('#chkBlameless').prop("checked")) {

        $('#txtBlameless').className = "";
        $('#txtBlameless').val(Math.round($('#txtTPL').val() * 0.2));
    }
    else {
        $('#chkBlameless').attr("checked", false);
        $('#txtBlameless').val("");
        $('#txtBlameless').className = "disablebox";
    }
}

//乘客责任险（//所选金额*费率*（座位数-1）。如果没有座位数，则*4）
function calcLimitofPassenger() {
    var seatNum = document.getElementById("hidSeatNum").value;
    var calCount;
    if (seatNum < 4) {  //小于四座看做没有座位数
        calCount = 4;
    } else {
        calCount = seatNum - 1;
    }
    if ($('#chkLimitofPassenger').prop("checked")) {
        if ($('#txtMoney').val() == 0) {
            return;
        }
        if (jQuery('#selCompulsory').children("span").attr("id") == 950) { //6座以下
            var lvalue1 = Math.round($("#selLimitofPassenger").children("span").attr("id") * 0.0027 * calCount);
            jQuery("#lblLimitOfPassenger").html(lvalue1);
        } else {
            var lvalue2 = Math.round($("#selLimitofPassenger").children("span").attr("id") * 0.0026 * calCount);
            jQuery("#lblLimitOfPassenger").html(lvalue2);
        }
    } else {
        jQuery("#lblLimitOfPassenger").html("0");
    }
}
//司机责任险
function calcLimitofDriver() {
    if ($('#chkLimitofDriver').prop("checked")) {
        if ($('#txtMoney').val() == 0) {
            return;
        }
        if (jQuery('#selCompulsory').children("span").attr("id") == 950) {   //6座以下
            //所选金额*费率*（座位数-1）。如果没有座位数，则*4
            var lvalue1 = Math.round($("#selLimitofDriver").children("span").attr("id") * 0.0042);
            jQuery("#lblLimitOfDriver").html(lvalue1);
        } else {
            var lvalue2 = Math.round($("#selLimitofDriver").children("span").attr("id") * 0.004);
            jQuery("#lblLimitOfDriver").html(lvalue2);
        }
    }
    else {
        jQuery("#lblLimitOfDriver").html("0");
    }

}
//车身划痕险
function calcCarDamageDW() {
    if ($('#chkCarDamage').prop("checked")) {
        $('#chkCarDamageDW').parent().attr("class", "");
        if ($('#chkCarDamageDW').prop("checked")) {
            if ($('#txtMoney').val() == 0) {
                return;
            }
            var selCarDamageDWIndex = $("#selCarDamageDW").children("span").attr("id");
            if ($('#txtMoney').val() < 300000) {
                if (selCarDamageDWIndex == 2000)
                    jQuery('#lblCarDamageDW').html("400");
                if (selCarDamageDWIndex == 5000)
                    jQuery('#lblCarDamageDW').html("570");
                if (selCarDamageDWIndex == 10000)
                    jQuery('#lblCarDamageDW').html("760");
                if (selCarDamageDWIndex == 20000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1140));
            } else if ($('#txtMoney').val() > 500000) {
                if (selCarDamageDWIndex == 2000)
                    jQuery('#lblCarDamageDW').html("850");
                if (selCarDamageDWIndex == 5000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1100));
                if (selCarDamageDWIndex == 10000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1500));
                if (selCarDamageDWIndex == 20000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(2250));
            } else {
                if (selCarDamageDWIndex == 2000)
                    jQuery('#lblCarDamageDW').html("585");
                if (selCarDamageDWIndex == 5000)
                    jQuery('#lblCarDamageDW').html("900");
                if (selCarDamageDWIndex == 10000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1170));
                if (selCarDamageDWIndex == 20000)
                    SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1780));
            }
        } else {
            jQuery('#lblCarDamageDW').html("0");
        }
    } else {
        $('#chkCarDamageDW').parent().attr("class", "no_click");
        $('#chkCarDamageDW').attr("checked", false);
        jQuery('#lblCarDamageDW').html("0");
    }


}



//------------------ 旧的JS ------------------------------
function getSerialByMasterBrandID(id) {
    // showLoading("true");

    $("ddlChexing").options.length = 1;
    $("ddlChekuan").options.length = 1;

    $('#txtMoney').val(0);
    if (id == -1 || id == -2) {
        return;
    }

    var myCarTypeOptions = {
        parameters: "bsid=" + id,
        method: "get",
        asynchronous: false,
        onSuccess: function (res) {
            var carTypeData = eval("(" + res.responseText + ")");
            for (var i = 0; i < carTypeData.length; i++) {
                var cartype = carTypeData[i];
                $("ddlChexing").options.add(new Option(cartype.Name, cartype.ID));
            }

            //   showLoading("false");
        }
    }

    new Ajax.Request(webSiteBaseUrl + "ajaxnew/GetSerialByMasterBrand.ashx?type=json", myCarTypeOptions);
}
//用作客户端保存车型数据
var cars = new Array();

function getCarByCsID(id) {
    // showLoading("true");

    //清空缓存车型数据
    cars.length = 0;
    var ddlChekuan = $("#ddlChekuan");
    var groupItem = ddlChekuan.firstChild;
    while (groupItem) {
        ddlChekuan.removeChild(groupItem);
        groupItem = ddlChekuan.firstChild;
    }
    var oItem = document.createElement("OPTION");
    oItem.setAttribute("value", -1);
    oItem.appendChild(document.createTextNode("选择车款"));
    $("ddlChekuan").appendChild(oItem);


    $('#txtMoney').val(0);

    if (id == -1) {
        return;
    }

    var myoptions = {
        parameters: "csid=" + id,
        method: "get",
        asynchronous: false,
        onSuccess: function (res) {
            var myData = eval("(" + res.responseText + ")");

            var yearList = new Object();
            yearList["YearList"] = new Array();
            for (var i = 0; i < myData.length; i++) {
                var car = myData[i];
                if (!yearList[car.YearType]) {
                    yearList[car.YearType] = new Array();
                    yearList["YearList"].push(car.YearType);
                }

                yearList[car.YearType].push(car);
            }
            for (var i = 0; i < yearList["YearList"].length; i++) {
                var carYear = yearList["YearList"][i];
                var carsInYear = yearList[carYear];
                if (yearList["YearList"].length > 1) {
                    var optionItem = document.createElement("OPTGROUP");
                    optionItem.label = carYear + "款";
                    optionItem.style.fontStyle = "normal";
                    optionItem.style.background = "#CCCCCC";
                    optionItem.style.textAlign = "center";
                    $("ddlChekuan").appendChild(optionItem);
                }
                for (var j = 0; j < yearList[carYear].length; j++) {
                    var car = yearList[carYear][j];
                    var oItem = document.createElement("OPTION");
                    oItem.setAttribute("value", car.ID);
                    oItem.appendChild(document.createTextNode(car.Name));
                    $("ddlChekuan").appendChild(oItem);
                    cars[cars.length] = new carModul(car.ID, car.Name, car.CarReferPrice);
                }
            }

            //数组构建索引
            var carLength = cars.length;
            for (var j = 0; j < carLength; j++) {
                cars[cars[j].id] = cars[j];
            }
            //     showLoading("false");

        }
    }

    new Ajax.Request(webSiteBaseUrl + "ajaxnew/GetCarByCsID.aspx?type=json", myoptions);
}

/*
*    ForDight(Dight,How):数值格式化函数，Dight要
*    格式化的  数字，How要保留的小数位数。
*/
function ForDight(Dight, How) {
    Dight = Math.round(Dight * Math.pow(10, How)) / Math.pow(10, How);
    return Dight;
}



//层 隐藏显示
function showOrHideDiv(imgID) {
    var imgClose = document.getElementById('imgClose');
    var imgOpen = document.getElementById('imgOpen');

    if (imgClose && imgOpen) {
        var showDivs = false;

        if ('imgClose' == imgID) {
            imgClose.style.display = 'none';
            imgOpen.style.display = '';
            showDivs = false;
        }
        if ('imgOpen' == imgID) {
            imgClose.style.display = '';
            imgOpen.style.display = 'none';
            showDivs = true;
        }

        for (var i = 0; i < 9; i++) {
            var divCommonTotals = document.getElementById('divCommonTotals' + i);

            divCommonTotals.style.display = "none";
            if (showDivs == true)
                divCommonTotals.style.display = "";
        }
    }
}

function setSelected(oSel, val) {
    if (val != "0" || val != "")
        oSel.value = val;
}

//获得车船使用税提示信息
function GetVehicleAndVesselTaxMessage() {
    var messageBuffer = [];
    var counter = 0;
    messageBuffer.push("各省不统一，以北京为例(单位/年)。");
    messageBuffer.push("<br />");
    for (var taxLevel in vehicleAndVesselTaxInfos) {
        counter++;
        var currentVehicleAndVesselTaxInfo = vehicleAndVesselTaxInfos[taxLevel];
        messageBuffer.push(currentVehicleAndVesselTaxInfo.DisplacementDescription);
        messageBuffer.push(vehicleAndVesselTaxRelief == "减半" ? currentVehicleAndVesselTaxInfo.Tax / 2 : currentVehicleAndVesselTaxInfo.Tax);
        messageBuffer.push("元；");
        if (counter % 2 == 0) {
            messageBuffer.push("<br />");
        }
    }
    messageBuffer.push("不足一年按当年剩余月算。");
    return messageBuffer.join("");
}

//计算车船使用税
function CalculateVehicleAndVesselTax() {
    var radioGroup = document.getElementsByName("rdoVehicleTax");
    for (var i = 0; i < radioGroup.length; i++) {
        if (radioGroup[i].checked) {
            var taxLevel = radioGroup[i].value;
        }
    }

    var vehicleAndVesselTaxValue = vehicleAndVesselTaxInfos[taxLevel].Tax;
    var vehicleAndVesselTaxMessage = GetVehicleAndVesselTaxMessage();
    //车船使用税一般只能缴纳当年的，按月计算
    //vehicleAndVesselTaxValue = vehicleAndVesselTaxValue * (12 - new Date().getMonth()) / 12;
    //计算车船使用税减免
    if (vehicleAndVesselTaxRelief == "免征") {
        vehicleAndVesselTaxValue = 0;
        vehicleAndVesselTaxMessage = "根据国家政策，该车无需缴纳车船税。";
    }
    else if (vehicleAndVesselTaxRelief == "减半") {
        vehicleAndVesselTaxValue = vehicleAndVesselTaxValue / 2;
        vehicleAndVesselTaxMessage = "该车享受车船税减半优惠。<br />" + vehicleAndVesselTaxMessage;
    }

    $('#txtVehicleTax').val(Math.ceil(vehicleAndVesselTaxValue));
    document.getElementById("tdVehicleAndVesselTaxMessage").innerHTML = vehicleAndVesselTaxMessage;
}

function carModul(id, name, carreferprice) {
    this.id = id;
    this.name = name;
    this.carreferprice = carreferprice;
}