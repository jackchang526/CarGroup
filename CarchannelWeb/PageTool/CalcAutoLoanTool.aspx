<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalcAutoLoanTool.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.CalcAutoLoanTool" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【贷款购车计算器】贷款购车利率-贷款购车条件-易车网</title>
    <meta name="Keywords" content="贷款购车计算器,贷款购车计算,购车贷款利率,哈尔滨贷款购车,贷款购车流程,长春贷款购车,长春贷款购车网,购车贷款条件,贷款购车手续,购车贷款如何办理" />
    <meta name="Description" content="贷款购车计算器：易车网购车小工具包括,贷款购车计算器、购车费用计算器,为您提供准确的购车费用信息。帮助您方便快捷的购车! " />
    <!--#include file="~/ushtml/0000/yiche_2015_jusuanqi_style-994.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlistnewV2.min.js?v=20160119"></script>
    <script type="text/javascript" src="<%= BitAuto.CarChannel.Common.WebConfig.StaticFileBaseUrl %>jsnew/CarCalculator.js?v=20160628"></script>
    <%--浮动JS--%>
    <script type="text/javascript">
        function addEvent(obj, type, fn) {
            if (obj.attachEvent) {
                obj['e' + type + fn] = fn;
                obj[type + fn] = function () { obj['e' + type + fn](window.event); }
                obj.attachEvent('on' + type, obj[type + fn]);
            } else {
                obj.addEventListener(type, fn, false);
            }
        }

        function getPageScroll() {
            if (!document.getElementById("liDispaly2")) return false;
            var liDispaly2 = document.getElementById("liDispaly2");
            var theanchor = document.getElementById("theanchor");
            var thetop = document.documentElement.scrollTop + document.body.scrollTop;
            if (thetop > theanchor.offsetTop + 50) {
                if (liDispaly2.className == "bg fixed-top") { return false; }
                liDispaly2.className = "bg fixed-top";
            } else {
                if (liDispaly2.className == "") { return false; }
                liDispaly2.className = "bg";
            }
        }

        function float_box() {
            addEvent(window, "scroll", getPageScroll);
        }
        if (!document.getElementById("liDispaly2")) {
            addEvent(window, "load", float_box);
        }
    </script>
    <%--贷款的计算赋值--%>
    <script type="text/javascript">
        // 车贷广告link
        var ADForCheDai = {
            CsAllSpellArray: new Array(),
            SetCheDaiADLink: function (carid) {
                var atrValue = jQuery("#serial4").children().children()[0].attributes["value"];
                if (!atrValue) {
                    return;
                }
                var csId = atrValue.value;
                if (ADForCheDai.CsAllSpellArray && ADForCheDai.CsAllSpellArray.length > 0 && ADForCheDai.CsAllSpellArray[csId]) {
                    var chedaiLink = $("#loanBuyCar");
                    if (chedaiLink) {
                        if (carid <= 0) {
                            var hidCarId = document.getElementById("hidCarID");
                            if (hidCarId && hidCarId.value > 0)
                            { carid = hidCarId.value; }
                        }
                        chedaiLink[0].href = "http://www.daikuan.com/www/" + ADForCheDai.CsAllSpellArray[csId] + "/m" + carid + "/?from=yc33&leads_source=p031002";
                    }
                }
            }
        };
        var webSiteBaseUrl = "<%= BitAuto.CarChannel.Common.WebConfig.WebSiteBaseUrl %>";
        function InitControl() {
            InitVehicleAndVesselTaxControl("divLoanVehicleAndVesselTaxMessage");

            //初始化下拉组件(品牌)文字颜色
            var spanTagMaster = $("#master4").children("span").children("span");
            spanTagMaster[0].style.color = "black";

            //初始化下拉组件(车系)文字颜色
            var spanTagSerial = $("#serial4").children("span").children("span");
            if (spanTagSerial.html() != "请选择系列") {
                spanTagSerial[0].style.color = "black";
            }

            //初始化下拉组件(车款)文字颜色
            var spanTagCarType = $("#cartype4").children("span").children("span");
            if (spanTagCarType.html() != "请选择车款") {
                spanTagCarType[0].style.color = "black";
            }
        }

        function InitAutoLoanData() {
            var hidCarPrice = $("#hidCarPrice").val();
            $('#txtMoney').val(hidCarPrice);
            var carId = document.getElementById("hidCarID").value;
            //var flag = parseInt(hidCarPrice) > 0; //可能从其他页面传递价格
            //if (!flag) {
            //    resetPrice(carId, webSiteBaseUrl);
            //    ADForCheDai.SetCheDaiADLink(carId);
            //} else {
            //    setCalcToolUrlByPrice(hidCarPrice);
            //}
            var carPricePara = getQueryString("CarPrice");
            if (parseInt(carId) > 0) {
                resetPrice(carId, webSiteBaseUrl);
                ADForCheDai.SetCheDaiADLink(carId);
                if (carPricePara) {
                    $('#txtMoney').val(carPricePara);
                    setCalcToolUrl(carId, carPricePara);
                } else {
                    setCalcToolUrl(carId, -1);
                }
            } else {
                if (carPricePara) {
                    $('#txtMoney').val(carPricePara);
                    setCalcToolUrl(-1, carPricePara);
                }
            }


            var downpaymentRatioInt = "<%= DownpaymentRatioInt %>";
            switch (downpaymentRatioInt) {
            case "30":
                $("#r1").prop("checked", true);
                break;
            case "40":
                $("#r2").prop("checked", true);
                break;
            case "50":
                $("#r3").prop("checked", true);
                break;
            case "60":
                $("#r4").prop("checked", true);
                break;
            }
            var repaymentYearsInt = "<%= RepaymentYearsInt %>";
            switch (repaymentYearsInt) {
            case "1":
                $("#y1").prop("checked", true);
                break;
            case "2":
                $("#y2").prop("checked", true);
                break;
            case "3":
                $("#y3").prop("checked", true);
                break;
            case "4":
                $("#y4").prop("checked", true);
                break;
            case "5":
                $("#y5").prop("checked", true);
                break;
            }
            calcYearRate();
        }

        //贷款购车总花费 首付款+贷款所花总钱数
        //贷款所花总钱数=月付款×还款年限×12
        //比全款购车多花费=贷款所花总钱数+首付金额-裸车价格。
        function calcTotalNew() {
            var moneyMonthPayments = calcMonthPayments();
            var periods = parseInt(jQuery('#yueGongQi').html());
            var moneySaved = formatCurrency(parseFloat(moneyMonthPayments * periods) + parseFloat($('#txtDownPayments').val()) - parseFloat($('#txtMoney').val()));
            if (isNaN($('#loanRate').val()) || $('#loanRate').val().length == 0 || $('#loanRate').val() == 0) {
                moneySaved = 0;
            }
            SetSpanValueByBrowerType('lblDuoHuaFei', moneySaved);
            var moneyTotal = calcFirstDownPayments() + parseInt(moneyMonthPayments * periods);
            SetSpanValueByBrowerType('lblTopTotal', formatCurrency(moneyTotal)); //购车总花费
            SetSpanValueByBrowerType('lblBottomTotal', formatCurrency(moneyTotal)); //购车总花费
        }

        function checkMoneyValidationNew() {
            if ($('#txtMoney').val() == "0") {
                $("#bottomSummary").hide();
                $('#liDispaly1')[0].style.display = '';
                $('#liDispaly2')[0].style.display = 'none';
            }
            else {
                $("#bottomSummary").show();
                $('#liDispaly1')[0].style.display = 'none';
                $('#liDispaly2')[0].style.display = '';
            }
            return true;
        }

        //计算年利率
        function calcYearRate() {
            var rdoPaymentYears = document.getElementsByName("rdoPaymentYears");
            for (i = 0; i < rdoPaymentYears.length; i++) {
                if (rdoPaymentYears[i].checked) {
                    switch (rdoPaymentYears[i].value) {
                    case "1":
                        jQuery("#loanRate").val(6.31);
                        break;
                    case "2":
                        jQuery("#loanRate").val(6.40);
                        break;
                    case "3":
                        jQuery("#loanRate").val(6.40);
                        break;
                    case "4":
                        jQuery("#loanRate").val(6.65);
                        break;
                    case "5":
                        jQuery("#loanRate").val(6.65);
                        break;
                    }
                }
            }
        }

        function calcAutoLoanAll() {
            if ($('#txtMoney').val().length == 0 || $('#txtMoney').val() == 0) {
                $('#rdoDownPaymentsOfSelf').attr("disabled", true);
                $('#txtDownPayments').attr("disabled", true);
            } else {
                $('#rdoDownPaymentsOfSelf').attr("disabled", false);
                if ($('#rdoDownPaymentsOfSelf').prop("checked"))
                    $('#txtDownPayments').attr("disabled", false);
                else
                    $('#txtDownPayments').attr("disabled", true);
            }

            if (!checkMoneyValidationNew()) {
                return false;
            }
            if ($('#txtMoney').val() == 0) {
                $('#txtDownPayments').val(0);
                jQuery('#lblYueGong,#lblBottomYueGong').html('0');
                jQuery('#lblShouFu').html('0'); //首付
                jQuery('#daikuanJinE').html('0'); //首付
                jQuery('#lblAcquisitionTax').html('0'); //购置税
                $('#txtVehicleTax').val(0); //车船使用税
                jQuery('#lblCompulsory').html('0'); //交强险
                jQuery('#lblCommonTotal').html('0'); //必要花费
                jQuery('#lblTPL').html('0'); //第三方责任险
                jQuery('#lblCarDamage').html('0'); //车辆损失险
                jQuery('#lblCarTheft').html('0'); //全车盗抢险
                jQuery('#lblBreakageOfGlass').html('0'); //玻璃破碎险
                jQuery('#lblSelfignite').html('0'); //自燃险
                jQuery('#lblAbatement').html('0'); //不计免赔特约险
                jQuery('#lblCarDamageDW').html('0'); //车身划痕险
                jQuery('#lblLimitOfPassenger').html('0'); //乘客坐位责任险
                jQuery('#lblLimitOfDriver').html('0'); //司机坐位责任险
                jQuery('#engineDamage').html('0'); //发动机特别损失险
                $('#txtChePai').val(0); //上牌费用
                jQuery('#essentialCost').html('0'); //必要花费
                //全款购车
                calcTotalNew();
                //return;
            }

            //首付金额
            calcDownPayments();
            //月付款
            calcMonthPayments();
            //------------------------
            //购置税
            calcAcquisitionTax();
            //交强险
            calcCompulsory();
            //------------------------
            //第三方责任险
            calcTPL();
            //车辆损失险
            calcCarDamage();
            //全车盗抢险
            calcCarTheft();
            //玻璃单独破碎险
            calcBreakageOfGlass();
            //自燃损失险
            calcSelfignite();
            //不计免赔特约险
            calcAbatement();
            //乘客责任险
            calcLimitofPassenger();
            //司机责任险
            calcLimitofDriver();
            //车身划痕险
            calcCarDamageDW();
            //发动机特别损失险
            calcCarEngineDamage();
            //------------------------
            //必要花费
            calcEssentialCost();
            //常规保险合计
            calcBusinessTotal();
            //首付款总额=首付款+必要花费+商业保险
            calcFirstDownPayments();

            calcTotalNew();
        }

        //----------------------------
        //首付金额
        function calcDownPayments() {
            var rdoDownPayments = document.getElementsByName("rdoDownPayments");
            if (!rdoDownPayments[4].checked) {
                for (i = 0; i < rdoDownPayments.length; i++) {
                    if (rdoDownPayments[i].checked) {
                        $('#txtDownPayments').val(Math.round($('#txtMoney').val() * rdoDownPayments[i].value));
                        break;
                    }
                }
            }
        }

        function calcMonthPayments() {
            var loanMonths = 12;
            var rdoPaymentYears = document.getElementsByName("rdoPaymentYears");
            for (i = 0; i < rdoPaymentYears.length; i++) {
                if (rdoPaymentYears[i].checked) {
                    loanMonths = rdoPaymentYears[i].value * 12;
                    break;
                }
            }
            if (isNaN($('#loanRate').val()) || $('#loanRate').val().length == 0 || $('#loanRate').val() == 0) {
                var r = Math.round(calcLoanValue() / loanMonths);
                SetSpanValueByBrowerType('lblYueGong', formatCurrency(r));
                SetSpanValueByBrowerType('lblBottomYueGong', formatCurrency(r));
                return r;
            } else {
                var yearRate = parseFloat($("#loanRate").val()) / 100;
                var monthPercent = yearRate / 12;
                jQuery('#yueGongQi').html(loanMonths);
                jQuery('#daikuanQi').html(loanMonths);
                var fenzi = calcLoanValue() * monthPercent * Math.pow((1 + monthPercent), loanMonths);
                var fenmu = (Math.pow((1 + monthPercent), loanMonths) - 1);
                var result = 0;
                if (fenmu != 0) {
                    result = Math.round(fenzi / fenmu);
                }
                SetSpanValueByBrowerType('lblYueGong', formatCurrency(result));
                SetSpanValueByBrowerType('lblBottomYueGong', formatCurrency(result));
                return result;
            }

        }

        function resetTxtDownPayments() {
            window.setTimeout(function () { $('#txtDownPayments').focus(); }, 0);
            $('#txtDownPayments').attr("disabled", false);
        }

        //首付金额
        function checkTxtDownPayments() {
            if (parseInt($('#txtDownPayments').val()) > parseInt($('#txtMoney').val())) {
                $('#txtDownPayments').val($('#txtMoney').val());
                window.setTimeout(function () { $('#txtDownPayments').focus(); }, 0);
            }
            //月付款
            calcMonthPayments();
            //首付额
            calcFirstDownPayments();
            calcTotalNew();
        }

        //贷款额=车辆购置价格-首付金额  注意首付金额同首付款的区别
        function calcLoanValue() {
            var downPayments = $('#txtDownPayments').val() == "" ? "0" : $('#txtDownPayments').val();
            var loanValue = parseInt($('#txtMoney').val()) - parseInt(downPayments);
            $("#daikuanJinE").text(formatCurrency(loanValue));
            return loanValue;
        }

        //首付款=首付金额+必要花费+商业保险
        function calcFirstDownPayments() {
            var downPayments = $('#txtDownPayments').val() == "" ? "0" : $('#txtDownPayments').val();
            var firstDownPayments = parseInt(downPayments) +
                parseInt(calcEssentialCost()) +
                parseInt(calcBusinessTotal());
            SetSpanValueByBrowerType('lblShouFu', formatCurrency(firstDownPayments));
            SetSpanValueByBrowerType('lblBottomShouFu', formatCurrency(firstDownPayments));
            return firstDownPayments;
        }

        //----------------------------

        //在未选择车型的情况下初始化上牌费用及车船使用税
        //        function InitPaiAndChuan(v) {
        //            $("#txtMoney").val(v.replace(/(\D)/g, ''));
        //            $("#txtChePai").val(500);
        //            var cheChuan = 420 * (12 - new Date().getMonth()) / 12;
        //            $("#txtVehicleTax").val(Math.ceil(cheChuan));
        //            setCalcToolUrl(-1);
        //            calcAutoLoanAll();
        //        }

        function InitPaiAndChuan(v) {
            var $txtMoney = $("#txtMoney");
            var $hidCarID = $("#hidCarID");
            var $txtChePai = $("#txtChePai");
            var $txtVehicleTax = $("#txtVehicleTax");

            $txtMoney.val(v.replace(/(\D)/g, ''));
            if (parseInt($hidCarID.val()) <= 0) {
                if ($txtMoney.val() != "" || parseInt($txtMoney.val()) > 0) {
                    if ($txtChePai.val() == "" || $txtChePai.val() == "0") {
                        $txtChePai.val(500);
                    }
                    if ($txtVehicleTax.val() == "" || $txtVehicleTax.val() == "0") {
                        var cheChuanValue1 = $("#vehicleTax option:selected").val();
                        switch (cheChuanValue1) {
                        case "1":
                            $txtVehicleTax.val("300");
                            break;
                        case "2":
                            $txtVehicleTax.val("420");
                            break;
                        case "3":
                            $txtVehicleTax.val("480");
                            break;
                        case "4":
                            $txtVehicleTax.val("900");
                            break;
                        case "5":
                            $txtVehicleTax.val("1920");
                            break;
                        case "6":
                            $txtVehicleTax.val("3480");
                            break;
                        case "7":
                            $txtVehicleTax.val("5280");
                            break;
                        default:
                            break;
                        }
                    }
                }
                setCalcToolUrl(-1, -1);
            } else {
                if ($txtMoney.val() != "" && parseInt($txtMoney.val()) > 0) {
                    if ($txtChePai.val() == "" || $txtChePai.val() == "0") {
                        $txtChePai.val(500);
                    }
                    if ($txtVehicleTax.val() == "" || $txtVehicleTax.val() == "0") {
                        var cheChuanValue2 = $("#vehicleTax option:selected").val();
                        switch (cheChuanValue2) {
                        case "1":
                            $txtVehicleTax.val("300");
                            break;
                        case "2":
                            $txtVehicleTax.val("420");
                            break;
                        case "3":
                            $txtVehicleTax.val("480");
                            break;
                        case "4":
                            $txtVehicleTax.val("900");
                            break;
                        case "5":
                            $txtVehicleTax.val("1920");
                            break;
                        case "6":
                            $txtVehicleTax.val("3480");
                            break;
                        case "7":
                            $txtVehicleTax.val("5280");
                            break;
                        default:
                            break;
                        }
                    }
                    setCalcToolUrl($hidCarID.val(), parseInt($txtMoney.val()));
                }
            }
            calcAutoLoanAll();
        }

        //--------------------------------------------------
        function ResetToDefault() {
            InitControl();
            InitAutoLoanData();
            InitCheChuanValue();
            calcAutoLoanAll();
        }

        $(function () {
            $(".close").click(function (e) {
                e = e || window.event;
                e.preventDefault();
                e.stopPropagation();
                $(this).closest(".yiwenicon.z30").removeAttr("style");
                $(this).closest(".tc.tc-jsq").hide();
            });
            $(document).click(function (e) {
                e = e || window.event;
                var target = e.srcElement || e.target;
                if ($(target).closest(".tc.tc-jsq").length <= 0)
                    $(".yiwenicon.z30").removeAttr("style");
                $(".tc.tc-jsq").hide();
            });
        });
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        <!--
        var tagIframe = null;
        var currentTagId = 33; 	//当前页的标签ID
        var cityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201";
        -->
    </script>
    <style type="text/css">
        #popBox { background: #000; position: absolute; z-index: 999; top: 0; left: 0; filter: alpha(opacity=30); opacity: 0.3; -moz-opacity: 0.3; }

        .iframe { width: 100%; border: none; position: absolute; z-index: 1; height: 100%; filter: alpha(opacity = 0); }

        .fixed-top { position: fixed; top: 0; z-index: 9999; }
    </style>
</head>
<body onload="ResetToDefault();">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <div class="bt_page">
        <!--互联互通 start-->
        <div class="bt_pageBox">
            <div class="col-ad">
                <ins id="div_f63a792d-c99e-4257-82d7-940a693efaac" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                    adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="f63a792d-c99e-4257-82d7-940a693efaac"></ins>
            </div>
            <div class="header_style">
                <div class="bitauto_logo">
                </div>
                <!--页头导航_yiche_LOGO开始-->
                <div class="yiche_logo">
                    <a href="http://www.bitauto.com">易车网</a>
                </div>
                <div class="yiche_lanmu">
                    <em>|</em><span>购车计算器</span>
                </div>
                <!--页头导航_yiche_LOGO结束-->
                <div class="bt_searchNew">
                    <!--#include file="~/html/bt_searchV3.shtml"-->
                </div>
                <div id="divSerialSummaryMianBaoAD" class="top_ad02">
					<ins id="div_def33be2-9c9c-4812-a2a2-6902b3232cb4" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="def33be2-9c9c-4812-a2a2-6902b3232cb4"> </ins>
                </div>
            </div>
            <div class="publicTabNew">
                <ul id="calcTools" class="tab">
                    <li id="xinche_index"><a href="/gouchejisuanqi/">全款</a></li>
                    <li id="xinche_ssxc" class="current"><a href="/qichedaikuanjisuanqi/">贷款</a></li>
                    <li id="xinche_1822"><a href="/qichebaoxianjisuan/">保险</a></li>
                </ul>
            </div>
        </div>
        <!--互联互通 end-->
        <div class="line-box">
            <div class="title-con">
                <input id="hidBsID" type="hidden" runat="server" value="-1" />
                <input id="hidCsID" type="hidden" runat="server" value="-1" />
                <input id="hidCsName" type="hidden" runat="server" />
                <input id="hidCarID" type="hidden" runat="server" value="-2" />
                <input id="hidSeatNum" type="hidden" runat="server" value="0" />
                <input id="hidCarPrice" type="hidden" runat="server" value="0" />
                <div class="title-box title-box2">
                    <h4>选择车款</h4>
                    <div class="tableHead_item">
                        <div class="jsq-tit-fbox">
                            <div id="master4" class="sel-item-box" style="z-index: 50; width: 150px;">
                            </div>
                        </div>
                        <div class="jsq-tit-fbox">
                            <div id="serial4" class="sel-item-box" style="z-index: 40; width: 150px;">
                            </div>
                        </div>
                        <div class="jsq-tit-fbox w236">
                            <div id="cartype4" class="sel-item-box" style="z-index: 30; width: 200px;">
                            </div>
                        </div>
                    </div>
                </div>
                <script language="javascript" type="text/javascript">
                    var hidBsId = document.getElementById("hidBsID");
                    var hidCsId = document.getElementById("hidCsID");
                    var hidCsName = document.getElementById("hidCsName");                    
                    var hidCarId = document.getElementById("hidCarID");
                    var mdvalue = "0", sdvalue = "0", cdvalue = "0", statId = 104;
                    if (hidCarId && hidCarId.value > 0) {
                        mdvalue = (hidBsId || hidBsId.value != "") ? hidBsId.value : 0;
                        sdvalue = hidCsId ? hidCsId.value : 0;
                        cdvalue = hidCarId ? hidCarId.value : 0;
                        statId = 103;
                    }

                    //绑定下拉选择框
                    BitA.DropDownListNew({
                        container: { master: "master4", serial: "serial4", cartype: "cartype4" },
                        include: { serial: "1", cartype: "1" },
                        dvalue: { master: mdvalue, serial: sdvalue, cartype: cdvalue },
                        datatype: 5,
                        callback: {
                            serial: function (data) {
                                for (var name in data) {
                                    if (data[name].id && data[name].urlSpell && !ADForCheDai.CsAllSpellArray[data[name].id]) {
                                        ADForCheDai.CsAllSpellArray[data[name].id] = data[name].urlSpell;
                                    }
                                }
                            }
                        },
                        onchange: {
                            master: function (data) {
                                //初始化下拉组件(车系)文字颜色
                                var spanTag = $("#serial4").children("span").children("span");
                                spanTag[0].style.color = "black";
                            },
                            serial: function (data) {
                                //初始化下拉组件(车型)文字颜色
                                var spanTag = $("#cartype4").children("span").children("span");
                                spanTag[0].style.color = "black";
                            },
                            cartype: function (data) {
                                //$("#hidCarID").val(data.id);
                                //resetPrice(data.id, webSiteBaseUrl);
                                //setCalcToolUrl(carId,-1);
                                ////初始化首付比例 否则从价格高选到底月供会出现负值
                                //$('#r1').prop("checked", "checked");
                                //InitCheChuanValue();
                                //calcAutoLoanAll();
                                //ADForCheDai.SetCheDaiADLink(data.id);
                                location.href = "/qichedaikuanjisuanqi/?carid=" + data.id;
                            }
                        }
                    });

                </script>
            </div>
            <div id="theanchor">
            </div>
            <div class="jisuanqi_box">
                <ul class="rela_ul">
                    <li>
                        <div class="l-box">
                            <span class="fl">裸车价格</span> <span class="fonts1" id="input-price-box">
                                <input id="txtMoney" type="text" class="f-w100" maxlength="8" onfocus="if (value =='0'){value ='';}"
                                    onblur="if (value ==''){value='0';}InitPaiAndChuan(this.value);" onkeyup="InitPaiAndChuan(this.value)" />
                                元</span>
                        </div>
                        <div class="r-box">
                        </div>
                        <div class="ad-js-300">
                            <ins id="div_b6c8ce12-eb33-4a98-a68e-d7d3ec3d53bd" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="b6c8ce12-eb33-4a98-a68e-d7d3ec3d53bd"></ins>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li id="liDispaly1" class="bg">
						<p class="no_mess">
							请选择车款或输入裸车价格
						</p>
					</li>
                    <li id="liDispaly2" class="bg" style="display: none">
                        <div class="l-box w360">
                            <p class="fonts2">首付款：<em class="red16"><label id="lblShouFu"></label>元</em></p>
                            <p class="suomin">首付金额+必要花费+商业保险</p>
                        </div>
                        <div class="l-box yuegong">
                            <p class="fonts2">月供：<em class="red16"><label id="lblYueGong"></label>元</em></p>
                            <p class="suomin">
                                <label id="yueGongQi">36</label>期
                            </p>
                        </div>
                        <div class="l-box zhonghuafei">
                            <p class="fonts2">
                                购车总花费：<em class="red16"><label id="lblTopTotal"></label>元</em>
                            </p>
                            <p class="suomin red">比全款多花<label id="lblDuoHuaFei">0</label>元</p>
                        </div>
                        <div class="button_orange button_115_36"><a id="loanBuyCar" href="http://www.daikuan.com" target="_blank">申请贷款</a></div>
                        <div class="clear"></div>
                    </li>
                </ul>
                <p class="suomin txt_right">
                    此结果仅供参考，实际应缴费以当地为准
                </p>
            </div>
            <!------------------------贷款明细开始------------------------>
            <div class="jsq_com_box">
                <div class="titbox">
                    <h4>贷款明细</h4>
                </div>
                <table id="loanDetail" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <colgroup>
                        <col width="200px" />
                        <col width="180px" />
                        <col width="210px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>首付金额
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <input id="txtDownPayments" type="text" class="f-w100 f-curr" onfocus="if (value =='0'){value ='';}"
                                        onblur="if (value ==''){value='0';}checkTxtDownPayments();calcAutoLoanAll();" />
                                    元
                                </div>
                            </td>
                            <td>
                                <input id="r1" name="rdoDownPayments" type="radio" checked="checked" value="0.3"
                                    onclick="calcAutoLoanAll()" />
                                <label for="r1">
                                    30%</label>
                                <input id="r2" name="rdoDownPayments" type="radio" value="0.4" onclick="calcAutoLoanAll()" />
                                <label for="r2">
                                    40%</label>
                                <input id="r3" name="rdoDownPayments" type="radio" value="0.5" onclick="calcAutoLoanAll()" />
                                <label for="r3">
                                    50%</label>
                                <input id="r4" name="rdoDownPayments" type="radio" value="0.6" onclick="calcAutoLoanAll()" />
                                <label for="r4">
                                    60%</label>
                                <br />
                                <input id="rdoDownPaymentsOfSelf" name="rdoDownPayments" onclick="resetTxtDownPayments()"
                                    type="radio" />
                                <label for="rdoDownPaymentsOfSelf">
                                    自定义</label>
                            </td>
                            <td>首付金额=首付比例*裸车价格
                            </td>
                        </tr>
                        <tr>
                            <th>贷款金额</th>
                            <td class="r_align">
                                <div class="jiage"><em id="daikuanJinE"></em>元</div>
                            </td>
                            <td></td>
                            <td>贷款金额=购车价格-首付款</td>
                        </tr>
                        <tr>
                            <th>贷款期限
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="daikuanQi">36</label></em>期
                                </div>
                            </td>
                            <td>
                                <input id="y1" name="rdoPaymentYears" type="radio" value="1" onclick="calcYearRate(); calcAutoLoanAll();" />
                                <label for="y1">
                                    1年</label>
                                <input id="y2" name="rdoPaymentYears" type="radio" value="2" onclick="calcYearRate(); calcAutoLoanAll();" />
                                <label for="y2">
                                    2年</label>
                                <input id="y3" name="rdoPaymentYears" type="radio" checked="checked" value="3" onclick="calcYearRate(); calcAutoLoanAll();" />
                                <label for="y3">
                                    3年</label>
                                <input id="y4" name="rdoPaymentYears" type="radio" value="4" onclick="calcYearRate(); calcAutoLoanAll();" />
                                <label for="y4">
                                    4年</label>
                                <br />
                                <input id="y5" name="rdoPaymentYears" type="radio" value="5" onclick="calcYearRate(); calcAutoLoanAll();" />
                                <label for="y5">
                                    5年</label>
                            </td>
                            <td>银行贷款基准利率：1年期6.31%；2年期6.4%；3年期6.4%；4年期6.65%；5年期6.65%；
                            </td>
                        </tr>
                        <tr>
                            <th>贷款利率
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <input id="loanRate" type="text" maxlength="5" class="f-w100 f-curr" value="6.4"
                                        onfocus="if (value =='0'){value ='';}" onblur="if (value ==''){value='0';}calcAutoLoanAll();" />
                                    %
                                </div>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>可按照实际贷款套餐利率修改
                            </td>
                        </tr>
                        <div id="loanLayer" class="newts_layer" style="display: none;">贷款利率上限为10%</div>
                    </tbody>
                </table>
            </div>
            <!------------------------贷款明细结束------------------------>
            <!------------------------必要花费开始------------------------>
            <div class="jsq_com_box">
                <div class="titbox">
                    <h4>必要花费</h4>
                    <div class="red_num">
                        <span>小计：</span>
                        <label id="essentialCost">
                        </label>
                        元
                    </div>
                </div>
                <table id="calEssentialCost" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <colgroup>
                        <col width="200px" />
                        <col width="180px" />
                        <col width="210px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <label>
                                    购置税</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblAcquisitionTax">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td id="gouZhiShuiDesc" style="color:#c00">&nbsp;
                            </td>
                            <td>
                                <span class="fl yiwen_box">购置附加税=裸车价格÷（1＋17％）× 10% 
                                    <a class="yiwenicon z30" desvalue="购置税解释说明" href="javascript:showjs('AcquisitionTax');">?
                                        <div id="AcquisitionTax" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>购置税</strong>
                                                <p>
                                                    车辆购置税是对在我国境内购置规定车辆的单位和个人征收的一种税，它由车辆购置附加费演变而来。现行车辆购置税法的基本规范，是从2001年1月1日起实施的《中华人民共和国车辆购置税暂行条例》。车辆购置税的纳税人为购置（包括购买、进口、自产、受赠、获奖或以其他方式取得并自用）应税车辆的单位和个人，征税范围为汽车、摩托车、电车、挂车、农用运输车，税率为10%，应纳税额的计算公式为：应纳税额=计税价格×税率。（2009年1月20日至12月31日，对1.6升及以下排量乘用车减按5%征收车辆购置税。自2010年1月1日至12月31日，对1.6升及以下排量乘用车减按7.5%征收车辆购置税。从2015年10月1日到2016年12月31日，对购买1.6升及以下排量乘用车实施减半征收车辆购置税的优惠政策）
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label>
                                    上牌费用</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <input maxlength="4" id="txtChePai" class="f-w100 f-curr" type="text" value="500"
                                        onkeyup="value=value.replace(/(\D)/g,'');calcAutoLoanAll();"
                                        onfocus="if (value =='0'){value ='';}" onblur="if (value ==''){value='0';}calcAutoLoanAll();" />
                                    元
                                </div>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>
                                <span class="fl yiwen_box">可手动修改，不同地区费用不同 
                                    <a class="yiwenicon z30" desvalue="上牌费用解释说明" href="javascript:showjs('shangPai');">?
                                        <div id="shangPai" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>上牌费用</strong>
                                                <p>
                                                    通常商家提供的一条龙服务收费约500元，个人办理约373元，其中工商验证、出库150元、移动证30元、环保卡3元、拓号费40元、行驶证相片20元、托盘费130元
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label>
                                    车船使用税</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <input id="txtVehicleTax" class="f-w100 f-curr" type="text" maxlength="4" onkeyup="value=value.replace(/(\D)/g,'');calcAutoLoanAll();"
                                        onfocus="if (value =='0'){value ='';}"
                                        onblur="if (value ==''){value='0';}calcAutoLoanAll();" />
                                    元
                                </div>
                            </td>
                            <td id="divLoanVehicleAndVesselTaxMessage"></td>
                            <td>
                                <span class="fl yiwen_box">按排量收取费用 
                                    <a class="yiwenicon z30" desvalue="车船使用税解释说明" href="javascript:showjs('cheChuanTax');">?
                                        <div id="cheChuanTax" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>车船使用税</strong>
                                                <p>
                                                    各省不统一，以北京为例(单位/年)。1.0L(含)以下300元；1.0-1.6L(含)420元；1.6-2.0L(含)480元；2.0-2.5L(含)900元；2.5-3.0L(含)1920元；3.0-4.0L(含)3480元；4.0L以上5280元；不足一年按当年剩余月算。<br />
                                                    车船使用税是对行驶于公共道路的车辆和航行于国内河流、湖泊或领海口岸的船舶,按照其种类(如机动车辆、非机动车辆、载人汽车、载货汽车等)、吨位和规定的税额计算征收的一种使用行为税。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <label>
                                    交强险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblCompulsory">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selCompulsory" class="f-w150 f-curr" onchange="calcAutoLoanAll();">
                                    <option value="950">家用6座以下</option>
                                    <option value="1100">家用6座及以上</option>
                                </select>
                            </td>
                            <td>
                                <span class="fl yiwen_box">国家强制保险 
                                    <a class="yiwenicon z30" desvalue="交强险解释说明" href="javascript:showjs('jiaoQiagnX');">?
                                        <div id="jiaoQiagnX" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>交强险</strong>
                                                <p>
                                                    家用6座以下950元/年，家用6座及以上1100元/年<br />
                                                    机动车交通事故责任强制保险是我国首个由国家法律规定实行的强制保险制度。《机动车交通事故责任强制保险条例》规定：交强险是由保险公司对被保险机动车发生道路交通事故造成受害人(不包括本车人员和被保险人)的人身伤亡、财产损失，在责任限额内予以赔偿的强制性责任保险。<br />
                                                    交强险有责限额分为死亡伤残赔偿限额110000元、医疗费用赔偿限额10000元、财产损失赔偿限额2000元以及被保险人在道路交通事故中无责任的赔偿限额。无责的赔偿限额死亡伤残赔偿限额11000元、医疗费用赔偿限额1000元、财产损失赔偿限额100元。<br />
                                                    责任限额是指被保险机动车发生道路交通事故，保险公司对每次保险事故所有受害人的人身伤亡和财产损失所承担的最高赔偿金额。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!------------------------必要花费结束------------------------>
            <!------------------------商业保险开始------------------------>
            <div class="jsq_com_box">
                <div class="titbox">
                    <h4>商业保险</h4>
                    <div class="red_num">
                        <span>小计：</span>
                        <label id="lblCommonTotal">
                        </label>
                        元
                    </div>
                    <div id="businessHeader" class="tab">
                        <a href="javascript:void(0);" onclick="JiBenBaoZ()">基本保障</a> | <a href="javascript:void(0);"
                            onclick="GaoXingJ()">高性价比</a> | <a href="javascript:void(0);" onclick="calcBusinessTotalIncludeState()">新车主全面保障</a>
                    </div>
                    <script type="text/javascript">
                        //车辆损失险 第三者责任险 不计免赔
                        function JiBenBaoZ() {
                            $('#chkTPL').prop("checked", true);
                            $('#chkCarDamage').prop("checked", true);
                            $('#chkAbatement').prop("checked", true);

                            $('#chkCarTheft').prop("checked", false);
                            $('#chkBreakageOfGlass').prop("checked", false);
                            $('#chkSelfignite').prop("checked", false);
                            $('#chkLimitofPassenger').prop("checked", false);
                            $('#chkLimitofDriver').prop("checked", false);
                            $('#chkCarDamageDW').prop("checked", false);
                            $('#chkEngine').prop("checked", false);

                            var commonTotal = 0;
                            if ($('#chkTPL').prop("checked")) {
                                commonTotal += GetIntValue(jQuery('#lblTPL').html());
                            }
                            if ($('#chkCarDamage').prop("checked")) {
                                commonTotal += GetIntValue(jQuery('#lblCarDamage').html());
                            }
                            if ($('#chkAbatement').prop("checked")) {
                                commonTotal += GetIntValue(jQuery('#lblAbatement').html());
                            }
                            jQuery('#lblCommonTotal').html(Math.round(commonTotal));
                            calcAutoLoanAll();
                        }
                        //第三者责任险 车辆损失险 不计免赔 乘客坐位责任险 司机坐位责任险
                        function GaoXingJ() {
                            $('#chkTPL').prop("checked", true);
                            $('#chkCarDamage').prop("checked", true);
                            $('#chkAbatement').prop("checked", true);
                            $('#chkLimitofPassenger').prop("checked", true);
                            $('#chkLimitofDriver').prop("checked", true);

                            $('#chkCarTheft').prop("checked", false);
                            $('#chkBreakageOfGlass').prop("checked", false);
                            $('#chkSelfignite').prop("checked", false);
                            $('#chkCarDamageDW').prop("checked", false);
                            $('#chkEngine').prop("checked", false);

                            var commonTotal = 0;
                            if ($('#chkTPL').prop("checked")) {
                                commonTotal += GetIntValue(jQuery('#lblTPL').html());
                            }
                            if ($('#chkCarDamage').prop("checked")) {
                                commonTotal += GetIntValue(jQuery('#lblCarDamage').html());
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
                            jQuery('#lblCommonTotal').html(Math.round(commonTotal));
                            calcAutoLoanAll();
                        }

                        function calcBusinessTotalIncludeState() {
                            $('#chkTPL').prop("checked", true);
                            $('#chkCarDamage').prop("checked", true);
                            $('#chkAbatement').prop("checked", true);
                            $('#chkLimitofPassenger').prop("checked", true);
                            $('#chkLimitofDriver').prop("checked", true);
                            $('#chkCarTheft').prop("checked", true);
                            $('#chkBreakageOfGlass').prop("checked", true);
                            $('#chkSelfignite').prop("checked", true);
                            $('#chkCarDamageDW').prop("checked", true);
                            $('#chkEngine').prop("checked", true);
                            calcBusinessTotal();
                            calcAutoLoanAll();
                        }
                    </script>
                </div>
                <table id="calBusiness" width="100%" cellspacing="0" cellpadding="0" border="0">
                    <colgroup>
                        <col width="200px" />
                        <col width="180px" />
                        <col width="210px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <input id="chkTPL" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkTPL">
                                    第三者责任险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblTPL">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selTPL" class="f-w100 f-curr" onchange="calcAutoLoanAll();">
                                    <option value="50000">5万</option>
                                    <option value="100000">10万</option>
                                    <option value="200000" selected="selected">20万</option>
                                    <option value="500000">50万</option>
                                    <option value="1000000">100万</option>
                                </select>
                                赔附额度
                            </td>
                            <td>
                                <span class="fl yiwen_box">发生车险事故时，赔偿对第三方造成的人身及财产损失 
                                    <a class="yiwenicon z30" desvalue="第三者责任险解释说明" href="javascript:showjs('diSanFang');">?
                                        <div id="diSanFang" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 100px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>第三方责任险</strong>
                                                <p>
                                                    第三者责任险是指被保险人或其允许的驾驶人员在使用保险车辆过程中发生意外事故，致使第三者遭受人身伤亡或财产直接损毁，依法应当由被保险人承担的经济责任，保险公司负责赔偿。同时，若经保险公司书面同意，被保险人因此发生仲裁或诉讼费用的，保险公司在责任限额以外赔偿，但最高不超过责任限额的30％。因为交强险在对第三者的财产损失和医疗费用部分赔偿较低，可考虑购买第三者责任险作为交强险的补充。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkCarDamage" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkCarDamage">
                                    车辆损失险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblCarDamage">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>
                                <span class="fl yiwen_box">车子发生碰撞，赔偿自己爱车损失的费用 
                                    <a class="yiwenicon z30" desvalue="车辆损失险解释说明" href="javascript:showjs('sunshi');">?
                                        <div id="sunshi" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>车辆损失险</strong>
                                                <p>
                                                    车辆损失险-裸车价格*费率+基础保费<br />
                                                    车辆损失险是车辆保险中用途最广泛的险种，它负责赔偿由于自然灾害和意外事故造成的自己车辆的损失。无论是小剐小蹭，还是损坏严重，都可以由保险公司来支付修理费用。<br />
                                                    被保险人或其允许的合格驾驶员在使用保险车辆过程中，因下列原因造成保险车辆的损失，保险公司负责赔偿：1．碰撞、倾覆；2．火灾、爆炸；3．外界物体倒塌、空中运行物体坠落、保险车辆行驶中平行坠落；4．雷击、暴风、龙卷风、暴雨、洪水、海啸、地陷、冰陷、崖崩、雪崩、雹灾、泥石流、滑坡；5.
                                                载运保险车辆的渡船遭受自然灾害（只限于有驾驶员随车照料者）。<br />
                                                    发生保险事故时，被保险人或其允许的合格驾驶员对保险车辆采取施救、保护措施所支出的合理费用，保险公司负责赔偿。但此项费用的最高赔偿金额以责任限额为限。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkAbatement" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkAbatement">
                                    不计免赔特约险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblAbatement">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td></td>
                            <td>
                                <span class="fl yiwen_box">保险条款约定事故发生后被保险人要自己承担一定比例的损失金额，购买此险这部分损失费用保险公司将同样给予赔偿 
                                    <a class="yiwenicon z30" desvalue="不计免赔特约险解释说明" href="javascript:showjs('mianPei');">?
                                        <div id="mianPei" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>不计免赔特约险</strong>
                                                <p>
                                                    (车辆损失险+第三者责任险)×20%<br />
                                                    负责赔偿在车损险和第三者责任险中应由被保险人自己承担的免赔金额，即100%赔付。<br />
                                                    不计免赔特约险为附加险，必须在投保车损险和第三者责任险之后方可投保该险种。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkCarTheft" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkCarTheft">
                                    全车盗抢险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblCarTheft">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td></td>
                            <td>
                                <span class="fl yiwen_box">赔偿全车被盗窃、抢劫、抢夺造成的车辆损失 
                                    <a class="yiwenicon z30" desvalue="全车盗抢险解释说明" href="javascript:showjs('daoQiang');">?
                                        <div id="daoQiang" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>全车盗抢险</strong>
                                                <p>
                                                    全车盗抢险=裸车价格*费率+基础保费<br />
                                                    指保险车辆全车被盗窃、被抢劫、被抢夺，经县级以上公安刑侦部门立案侦查证实满一定时间没有下落的，由保险公司在保险金额内予以赔偿。如果是车辆的某些零部件被盗抢，如轮胎被盗抢、车内财产被盗抢、后备箱内的物品丢失，保险公司均不负责赔偿。
                                                    但是，对于车辆被盗抢期间内，保险车辆上零部件的损坏、丢失，保险公司一般负责赔偿。<br />
                                                    全车盗抢险为附加险，必须在投保车辆损失险之后方可投保该险种。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkBreakageOfGlass" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkBreakageOfGlass">
                                    玻璃单独破碎险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblBreakageOfGlass">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selBreakageOfGlass" class="f-w100 f-curr" onchange="calcAutoLoanAll()">
                                    <option value="1">国产</option>
                                    <option value="0">进口</option>
                                </select>
                                赔附额度
                            </td>
                            <td>
                                <span class="fl yiwen_box">赔偿保险车辆在使用过程中，发生车窗、挡风玻璃的单独破碎损失 
                                    <a class="yiwenicon z30" desvalue="玻璃单独破碎险解释说明" href="javascript:showjs('boLi');">?
                                        <div id="boLi" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>玻璃单独破碎险</strong>
                                                <p>
                                                    玻璃单独破碎险=裸车价格*费率<br />
                                                    负责赔偿保险车辆在使用过程中，发生本车玻璃发生单独破碎的保险公司按照保险合同进行赔偿。玻璃单独破碎险中的玻璃是指风档玻璃和车窗玻璃，如果车灯、车镜玻璃破碎及车辆维修过程中造成的破碎，保险公司不承担赔偿责任。<br />
                                                    玻璃单独破碎险为附加险，必须在投保车辆损失险之后方可投保该险种。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkSelfignite" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkSelfignite">
                                    自燃损失险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblSelfignite">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td></td>
                            <td>
                                <span class="fl yiwen_box">赔偿车子因电器、线路、运载货物等自身原因引发火灾造成的损失 
                                    <a class="yiwenicon z30" desvalue="自燃损失险解释说明" href="javascript:showjs('ziRan');">?
                                        <div id="ziRan" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>自燃损失险</strong>
                                                <p>
                                                    自燃损失险=裸车价格×0.15%<br />
                                                    负责赔偿因本车电器、线路、供油系统发生故障及运载货物自身原因起火造成车辆本身的损失。当车辆发生部分损失，按照实际修复费用赔偿修理费。如果车辆自燃造成整体烧毁或已经失去修理价值，则按照出险时车辆的实际价值赔偿，但不超过责任限额。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkEngine" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkEngine">
                                    涉水险/发动机特别损失险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="engineDamage">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td></td>
                            <td>
                                <span class="fl yiwen_box">负责爱车被水淹导致发动机受损所造成的损失 
                                    <a class="yiwenicon z30" desvalue="发动机特别损失险解释说明" href="javascript:showjs('faDongJi');">?
                                        <div id="faDongJi" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>涉水险/发动机特别损失险</strong>
                                                <p>
                                                    发动机特别损失险=车损险*5%<br />
                                                    涉水险或称汽车损失保险、发动机特别损失险，各个保险公司叫法不一样但本质一致，这是一种新衍生的险种，均指车主为发动机购买的附加险。<br />
                                                    这个险种主要是指车主为发动机购买的附加险。它主要是保障车辆在积水路面涉水行驶或被水淹后致使发动机损坏可给予赔偿。即使被水淹后车主还强行启动发动机而造成了损害，保险公司仍然给予赔偿。当然保险公司不一样，条款就不大一样，投保时可以查阅下各个保险公司条款内容。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkCarDamageDW" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkCarDamageDW">
                                    车身划痕险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblCarDamageDW">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selCarDamageDW" class="f-w100 f-curr" onchange="calcAutoLoanAll()">
                                    <option value="2000">2千</option>
                                    <option value="5000" selected="selected">5千</option>
                                    <option value="10000">1万</option>
                                    <option value="20000">2万</option>
                                </select>
                                赔附额度
                            </td>
                            <td>
                                <span class="fl yiwen_box">负责无碰撞痕迹的车身表面油漆单独划伤的损失 
                                    <a class="yiwenicon z30" desvalue="车身划痕险解释说明" href="javascript:showjs('huaHen');">?
                                        <div id="huaHen" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>车身划痕险</strong>
                                                <p>
                                                    无明显碰撞痕迹的车身划痕损失，保险公司负责赔偿。<br />
                                                    车身划痕险为附加险，必须在投保车辆损失险之后方可投保该险种。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkLimitofDriver" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkLimitofDriver">
                                    司机座位责任险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblLimitOfDriver">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selLimitofDriver" class="f-w100 f-curr" onchange="calcAutoLoanAll()">
                                    <option value="10000">1万</option>
                                    <option selected="selected" value="20000">2万</option>
                                    <option value="30000">3万</option>
                                    <option value="40000">4万</option>
                                    <option value="50000">5万</option>
                                </select>
                                赔附额度
                            </td>
                            <td>
                                <span class="fl yiwen_box">发生车险事故时，赔偿车内司机的伤亡和医疗赔偿费用 
                                    <a class="yiwenicon z30" desvalue="司机座位责任险解释说明" href="javascript:showjs('siJi');">?
                                        <div id="siJi" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>司机坐位责任险</strong>
                                                <p>
                                                    司机责任险=保额*费率<br />
                                                    统称为车上责任险，包括司机座位和乘客座位，主要是指在发生意外情况下，保险公司对司机座位的人员和乘客的人身安全进行赔偿。<br />
                                                    严格来说，司机责任险并不是一个独立的险种，而是商业车险中车上人员责任险的一部分，除此之外，车主还可以为乘客座位投保，一般选择的赔偿限额为1-5万元
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <input id="chkLimitofPassenger" type="checkbox" checked="checked" onclick="calcAutoLoanAll();" />
                                <label for="chkLimitofPassenger">
                                    乘客座位责任险</label>
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblLimitOfPassenger">
                                        </label>
                                    </em>元
                                </div>
                            </td>
                            <td>
                                <select id="selLimitofPassenger" class="f-w100 f-curr" onchange="calcAutoLoanAll()">
                                    <option value="10000">1万</option>
                                    <option selected="selected" value="20000">2万</option>
                                    <option value="30000">3万</option>
                                    <option value="40000">4万</option>
                                    <option value="50000">5万</option>
                                </select>
                                赔附额度
                            </td>
                            <td>
                                <span class="fl yiwen_box">发生车险事故时，赔偿车内乘客的伤亡和医疗赔偿费用 
                                    <a class="yiwenicon z30" desvalue="乘客座位责任险解释说明" href="javascript:showjs('chengKe');">?
                                        <div id="chengKe" class="tc tc-jsq" style="display: none">
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
                                                frameborder="0" src="about:blank"></iframe>
                                            <div class="tc-box">
                                                <i></i><em class="close">关闭</em> <strong>乘客坐位责任险</strong>
                                                <p>
                                                    乘客责任险=保额*费率<br />
                                                    被保险人允许的合格驾驶员在使用保险车辆过程中发生保险事故，致使车内乘客人身伤亡，依法应由被保险人承担的赔偿责任，保险人依照保险合同的约定给予赔偿。
                                                </p>
                                            </div>
                                        </div>
                                    </a>
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!------------------------商业保险结束------------------------>
            <div id="bottomSummary" class="summary-box add-gray">
                <div class="all-smmary">购车总花费：<em><label id="lblBottomTotal"></label> 元</em></div>
                <div class="w230">首付款：<label id="lblBottomShouFu"></label> 元</div>
                <div class="w360">月供：<label id="lblBottomYueGong"></label> 元</div>
            </div>
        </div>
        <%--贷款推荐--%>
        <div class="add_dk_box" style="display: none">
		    <div class="line_box" id="pc-load">
			    <div class="title-con">
				    <div class="title-box">
					    <h3><a id="loanTJ" href="javascript:void(0)" target="_blank"></a></h3>
					    <div class="more">
						    <a id="loanMoreTJ" href="javascript:void(0)" target="_blank">更多&gt;&gt;</a>
					    </div>
				    </div>
			    </div>
			    
		    </div>
		</div>
    </div>
    <script type="text/javascript">
        (function ($) {
            $(function () {
                $("#loanRate").bind("keyup", function () {
                    $(this).val($(this).val().replace(/([^0-9.])/g, ''));
                    if (parseFloat($(this).val()) > 10) {
                        var $loanLayer = $("#loanLayer");
                        var scrollTop = $(document).scrollTop();
                        var top = $loanLayer.height();
                        $loanLayer.css({ "z-Index": 999, "top": (top + scrollTop) + "px", "left": "40%" }).show();
                        
                        $(this).val(10);
                        setTimeout(function () {
                            $loanLayer.hide();
                        }, 2000);
                        calcAutoLoanAll();
                        return;
                    }
                    calcAutoLoanAll();
                });

                $("#txtDownPayments").bind("keyup", function () {
                    $(this).val($(this).val().replace(/([^0-9])/g, ''));
                    var luochePrice = $("#txtMoney").val();
                    if (parseFloat($(this).val()) > parseFloat(luochePrice)) {
                        $(this).val(luochePrice);
                        calcAutoLoanAll();
                        return;
                    }
                    calcAutoLoanAll();
                });



                //解释说明
                $("a.yiwenicon.z30").bind("click", function (e) {
                    $(".yiwenicon.z30").removeAttr("style"); //处理连续点击问号的情况
                    e.stopPropagation();
                    $(this).css("zIndex", 100);
                });
                $("p").bind("click", function (e) {
                    e.stopPropagation();
                });
                //------------------------必要花费--------------------------------------------

                $(document).on("change", "#vehicleTax", function () {
                    CalculateVehAndVesselTax();
                    calcAutoLoanAll();
                });

                //------------------------商业保险--------------------------------------------

            });
        })(jQuery)
    </script>
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/footer2014.shtml"-->
    
    <script type="text/javascript">
        //获取贷款
        $.ajax({
            url: "http://carapi.chedai.bitauto.com/api/SummarizeFinancialProducts/SearchSummarizeFinancialProducts?cityId=" + cityId + "&serialId=" + hidCsId.value + "&pageSize=4&from=yc38", 
            dataType: "jsonp", 
            jsonpCallback: "creditinfo", 
            cache: true, 
            success: function (result) {
                var data = $.parseJSON(result);
                if (typeof data !== "undefined" && data.length > 0) {
                    var h = [];
                    $.each(data, function (i, n) {
                        if (i >= 4) return;
                        var classNameArray = ["daikuan-f","","","daikuan-l"];
                        h.push("<div class=\"daikuan-box " + classNameArray[i] + "\">");
                        h.push("	<dl class=\"daikuan-name\">");
                        h.push("		<img src=\"" + n.CompanyLogoUrl + "\" />");
                        h.push("		<dt>" + n.CompanyName + "</dt>");
                        h.push("		<dd>成功率：<span class=\"photo_box\"><span class=\"succ\" style=\"width: " + (n.SuccessScore / 5) * 100 + "%;\"></span></span>");
                        h.push("		</dd>");
                        h.push("	</dl>");
                        h.push("	<div class=\"daikuan-money\">");
                        h.push("		<p>");
                        h.push("			<span>月供</span>");
                        h.push("			<span><strong>" + n.MonthlyPaymentText + "</strong></span>");
                        h.push("		</p>");
                        h.push("		<p class=\"last\">");
                        h.push("			<span class=\"tc-pop\">");
                        h.push("				<em>总成本</em>");
                        h.push("			</span>");
                        h.push("			<span class=\"gray\"><strong>" + n.TotalCostText + "</strong> </span>");
                        h.push("		</p>");
                        h.push("		<div class=\"tc tc-zong\" style=\"display: none;\">");
                        h.push("			<div class=\"tc-box\">");
                        h.push("				<i></i>");
                        h.push("				<p>月利率=" + n.InterestRateText + "</p>");
                        h.push("				<p>总成本=" + n.TotalInterestText + " （利息）+" + n.ServiceFeeText + "（手续费）</p>");
                        h.push("			</div>");
                        h.push("		</div>");
                        h.push("	</div>");
                        var labelArray = (n.MultiLabel != null && n.MultiLabel != "") ? n.MultiLabel.split("||") : [];
                        h.push("	<div class=\"daikuan-keywords\">");
                        $.each(labelArray, function (ii, nn) {
                            h.push("		<span>" + nn + "</span>");
                        });
                        h.push("	</div>");

                        h.push("	<div class=\"daikuan-detail button_gray\">");
                        h.push("		<a target=\"_blank\" href=\"" + n.PCDetailsUrl + "\">查看详情</a>");
                        h.push("	</div>");
                        h.push("</div>");
                    });
                    h.push("<div class=\"clear\"></div>");
                    $("#pc-load").append(h.join('')).parent().show();
                    $("#pc-load .tc-pop").hover(
                        function () { $(this).parent().siblings(".tc-zong").show(); },
                        function () { $(this).parent().siblings(".tc-zong").hide(); }
                    );
                    $("#loanTJ").attr("href", "http://www.daikuan.com/www/"+ADForCheDai.CsAllSpellArray[hidCsId.value]+"?from=yc38").text(hidCsName.value + "贷款推荐");
                    $("#loanMoreTJ").attr("href", "http://www.daikuan.com/www/"+ADForCheDai.CsAllSpellArray[hidCsId.value]+"?from=yc38");
                } else {
                    $("#pc-load").parent().hide();
                }
            }
        });
        //补贴
        function getSubsidy(carId, cityId) {
            if (!(carId > 0)) return;
            $.ajax({
                url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcarsunsidy&carid=" +carId + "&cityid=" + cityId + "",
                dataType: "jsonp",
                jsonpCallback: "getSubsidyCallback",
                cache: true,
                success: function (data) {
                    if (!(data && data.length > 0)) return; 
                    if (!(data[0].StateSubsidies > 0 && data[0].LocalSubsidy > 0)) return;
                    var sub = [];
                    if (data[0].StateSubsidies > 0)
                        sub.push("国家补贴" + data[0].StateSubsidies + "万元");
                    if (data[0].LocalSubsidy > 0)
                        sub.push("地方补贴" + data[0].LocalSubsidy + "万元");
                    var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"butie\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
                    $("#input-price-box").append(b); 
                }
            });
        } 
        getSubsidy(hidCarId.value,cityId);
    </script>
</body>
</html>
