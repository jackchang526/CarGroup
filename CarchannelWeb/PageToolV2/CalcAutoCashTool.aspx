﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/PageToolV2/CalcAutoCashTool.aspx.cs"
Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.CalcAutoCashTool" %>
<%@ Import Namespace="BitAuto.CarChannel.Common" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head id="Head1">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
<title>【购车计算器】购车贷款计算器-购车费用计算器-易车网</title>
<meta name="Keywords" content="贷款购车计算器,购车费用计算器,购车计算器2010,按揭购车计算器,模拟购车计算器,购车保险计算器,购车价格计算器,搜狐购车计算器,分期购车计算器"/>
<meta name="Description" content="购车计算器：易车网购车小工具包括,贷款购车计算器、购车费用计算器,为您提供准确的购车费用信息。帮助您方便快捷的购车! "/>
<script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
<script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
<!--#include file="~/ushtml/0000/yiche_2016_cube_jisuanqi_style-1268.shtml"-->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/dropdownlistnew.min.js?v=20161130"></script>
<script type="text/javascript" src="<%= WebConfig.StaticFileBaseUrl %>jsnewV2/CarCalculator.min.js?v=20170324"></script>
<%--浮动JS--%>
<script type="text/javascript">
    function addEvent(obj, type, fn) {
        if (obj.attachEvent) {
            obj['e' + type + fn] = fn;
            obj[type + fn] = function() { obj['e' + type + fn](window.event); };
            obj.attachEvent('on' + type, obj[type + fn]);
        } else {
            obj.addEventListener(type, fn, false);
        }
    }

    var liDispaly2Height;
    function getPageScroll() {        
        if (!document.getElementById("liDispaly2")) return false;
        var theid = document.getElementById("liDispaly2");
        if (theid.className == "bg") {
            liDispaly2Height = $(theid).offset().top;
        }
        if ($(window).scrollTop() > liDispaly2Height) {
            if (theid.className == "bg fixed-top") {
                return false;
            }
            theid.className = "bg fixed-top";
        } else {
            if (theid.className == "bg") {
                return false;
            }
            theid.className = "bg";
        }
    }

    function float_box() {
        addEvent(window, "scroll", getPageScroll);
    }

    if (!document.getElementById("liDispaly2")) {
        addEvent(window, "load", float_box);
    }
</script>
<%--保险的计算赋值--%>
<script type="text/javascript">
    // 车贷广告link
    var ADForCheDai = {
        CsAllSpellArray: new Array(),
        SetCheDaiADLink: function(carid) {
            var atrValue = jQuery("#serial4").children()[0].attributes["value"];
            if (!atrValue) {
                return;
            }
            var csId = atrValue.value;
            if (ADForCheDai.CsAllSpellArray && ADForCheDai.CsAllSpellArray.length > 0 && ADForCheDai.CsAllSpellArray[csId]) {
                var chedaiLink = $("#loanBuyCar");
                if (chedaiLink) {
                    if (carid <= 0) {
                        var hidCarId = document.getElementById("hidCarID");
                        if (hidCarId && hidCarId.value > 0) {
                            carid = hidCarId.value;
                        }
                    }
                    chedaiLink[0].href = "http://www.daikuan.com/www/" + ADForCheDai.CsAllSpellArray[csId] + "/m" + carid + "/?from=yc6&leads_source=p031001";
                }
            }
        }
    };

    var webSiteBaseUrl = "<%= WebConfig.WebSiteBaseUrl %>";

    function InitControl() {
        InitVehicleAndVesselTaxControl("divCashVehicleAndVesselTaxMessage");
        
        $("#vehicleTax .drop-layer a").on("click", function (event) {
            var spanText = $(this).text();
            var spanId = $(this).attr("id");
            var span = $("#vehicleTax").children("span");
            span.text(spanText);
            span.attr("id", spanId);
            $(this).parent().hide();
            CalculateVehAndVesselTax();
            calcAutoCashAll();
            event.stopPropagation();
        });

        //初始化下拉组件(品牌)文字颜色
        var spanTagMaster = $("#master4");
        spanTagMaster[0].style.color = "black";

        //初始化下拉组件(车系)文字颜色
        var spanTagSerial = $("#serial4");
        if (spanTagSerial.html() != "请选择系列") {
            spanTagSerial[0].style.color = "black";
        }

        //初始化下拉组件(车款)文字颜色
        var spanTagCarType = $("#cartype4");
        if (spanTagCarType.html() != "请选择车款") {
            spanTagCarType[0].style.color = "black";
        }
    }

    function initAutoCashData() {
        var hidCarPrice = $("#hidCarPrice").val();
        $('#txtMoney').val(hidCarPrice);
        var carId = document.getElementById("hidCarID").value;
        //var flag = parseInt(hidCarPrice) > 0; //可能从其他页面传递价格
        //if (!flag) {
        //	if (carId != -2) {
        //		resetPrice(carId, webSiteBaseUrl);
        //		ADForCheDai.SetCheDaiADLink(carId);
        //	}
        //} else {
        //	$("#txtChePai").val(500);
        //	setCalcToolUrlByPrice(hidCarPrice);
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
    }

    //在未选择车型的情况下初始化上牌费用及车船使用税
    //        function InitPaiAndChuan(v) {
    //            $("#txtMoney").val(v.replace(/(\D)/g, ''));
    //            $("#txtChePai").val(500);
    //            var cheChuan = 420 * (12 - new Date().getMonth()) / 12;
    //            $("#txtVehicleTax").val(Math.ceil(cheChuan));
    //            setCalcToolUrl(-1);
    //            calcAutoCashAll();
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
                    //var cheChuanValue1 = $("#vehicleTax option:selected").val();
                    var cheChuanValue1 = $("#vehicleTax").children("span").attr("id");
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
                    var cheChuanValue2 = $("#vehicleTax").children("span").attr("id");
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
        calcAutoCashAll();
    }

    function calcAutoCashAll() {
        if ($('#txtMoney').val() == 0) {
            if ($("#liDispaly2").length > 0) {
                document.getElementById('lblShouFu').innerHTML = 0; //首付
                document.getElementById('lblYueGong').innerHTML = 0; //月供   
            }
            document.getElementById('lblAcquisitionTax').innerHTML = 0;
            $('#txtVehicleTax').val(0); //车船使用税
            document.getElementById('lblCompulsory').innerHTML = 0; //交强险
            document.getElementById('lblCommonTotal').innerHTML = 0; //必要花费
            document.getElementById('lblTPL').innerHTML = 0; //第三方责任险
            document.getElementById('lblCarDamage').innerHTML = 0; //车辆损失险
            document.getElementById('lblCarTheft').innerHTML = 0; //全车盗抢险
            document.getElementById('lblBreakageOfGlass').innerHTML = 0; //玻璃破碎险
            document.getElementById('lblSelfignite').innerHTML = 0; //自燃险
            document.getElementById('lblAbatement').innerHTML = 0; //不计免赔特约险
            document.getElementById('lblCarDamageDW').innerHTML = 0; //车身划痕险
            document.getElementById('lblLimitOfPassenger').innerHTML = 0; //乘客坐位责任险
            document.getElementById('lblLimitOfDriver').innerHTML = 0; //司机坐位责任险
            document.getElementById('engineDamage').innerHTML = 0; //发动机特别损失险
            $('#txtChePai').val(0); //上牌费用
            document.getElementById('essentialCost').innerHTML = 0; //必要花费
            //全款购车
            calcTotalNew();
            //return;
        }

        //月供
        calcYueGong();
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
        //商业保险合计
        calcBusinessTotal();
        //全款购车
        calcTotalNew();
        //首付
        calcShouFu();
    }

    //首付(必要花费+商业保险+裸车价*30%)
    function calcShouFu() {
        var shouFu = calcEssentialCost() + calcBusinessTotal() + Math.round($('#txtMoney').val() * 0.3);
        SetSpanValueByBrowerType("lblShouFu", formatCurrency(shouFu));
        return shouFu;
    }

    //月供(按3年 首付30%计算)  利率值可参考"计算器需求文档"
    function calcYueGong() {
        var monthPercent = 0.00533333;
        var loanMonths = 36;
        var fenzi = parseFloat($('#txtMoney').val()) * 0.7 * monthPercent * Math.pow((1 + monthPercent), loanMonths);
        var fenmu = (Math.pow((1 + monthPercent), loanMonths) - 1);
        var result = Math.round(fenzi / fenmu);
        SetSpanValueByBrowerType("lblYueGong", formatCurrency(result));
        return result;
    }


    //全款购车=裸车价格+必要花费+商业保险
    function calcTotalNew() {
        var moneyTotal = Math.round(parseFloat($('#txtMoney').val()) + calcEssentialCost() + calcBusinessTotal());
        SetSpanValueByBrowerType('lblTopTotal', formatCurrency(moneyTotal));
        SetSpanValueByBrowerType('lblBottomTotal', formatCurrency(moneyTotal));
    }

    function ResetToDefault() {
        InitControl();
        initAutoCashData();
        InitCheChuanValue();
        calcAutoCashAll();
    }

    function HideById(id) {
        $("#" + id).hide();
    }

    $(function() {
        //$(".close").click(function(e) {
        //    e = e || window.event;
        //    e.preventDefault();
        //    e.stopPropagation();
        //    $(this).closest(".yiwenicon.z30").removeAttr("style");
        //    $(this).closest(".tc.tc-jsq").hide();
        //});
        $(document).click(function(e) {
            e = e || window.event;
            var target = e.srcElement || e.target;
            if ($(target).closest(".input-w-sm").length <= 0) {
                $(".drop-layer").hide();
            }
        });
    });

</script>
<script type="text/javascript">

    var tagIframe = null;
    var currentTagId = 32; //当前页的标签ID

</script>
<style type="text/css">
    .fixed-top {
        position: fixed;
        top: 0;
        z-index: 9999;
    }
</style>
</head>
<body onload="ResetToDefault();">
<span id="yicheAnchor" name="yicheAnchor" style="display: block; font-size: 0; height: 0; line-height: 0; width: 0;"></span>
<!--#include file="~/htmlV2/header2016.shtml"-->


<%--<div class="top-adv-box">
    <img src="http://temp.im/1200x80" alt="广告">
</div>--%>

<!--头部开始-->
<header class="header-main special-header2">

    <div class="container section-layout top" id="topBox">
        <div class="col-xs-6 left-box">
            <div class="section-left">
                <h1 class="logo">
                    <a href="http://www.bitauto.com">易车yiche.com</a>
                </h1>
                <h2 class="title">购车计算器</h2>
            </div>
        </div>
        <div class="col-xs-6 right-box">
            <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
        </div>
    </div>

    <div id="navBox">
        <nav class="header-main-nav">
            <div class="container">
                <div class="col-auto left secondary">
                    <ul id="calcTools" class="list list-justified">
                        <li id="xinche_index" class="active">
                            <a href="/gouchejisuanqi/">全款</a>
                        </li>
                        <li id="xinche_ssxc">
                            <a href="/qichedaikuanjisuanqi/">贷款</a>
                        </li>
                        <li id="xinche_1822">
                            <a href="/qichebaoxianjisuan/">保险</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
</header>
<!--头部结束-->
<div class="container">
<div class="title-con">
    <input id="hidBsID" type="hidden" runat="server" value="-1"/>
    <input id="hidCsID" type="hidden" runat="server" value="-1"/>
    <input id="hidCarID" type="hidden" runat="server" value="-2"/>
    <input id="hidSeatNum" type="hidden" runat="server" value="0"/>
    <input id="hidCarPrice" type="hidden" runat="server" value="0"/>

    <div class="title-box title-box2">
        <h4>选择车款</h4>
        <!--品牌选择开始-->
        <div class="brand-form no-second" id="master4">
        </div>
        <!--品牌选择结束-->

        <!--车型选择开始-->
        <div class="brand-form no-second" id="serial4">
        </div>
        <!--车型选择结束-->

        <!--车款选择开始-->
        <div class="brand-form no-second sele-300" id="cartype4">
        </div>
        <!--车款选择结束-->
    </div>

    <script type="text/javascript">
        var hidBsId = document.getElementById("hidBsID");
        var hidCsId = document.getElementById("hidCsID");
        var hidCarId = document.getElementById("hidCarID");

        var mdvalue = "0", sdvalue = "0", cdvalue = "0", statId = 102;
        if (hidCarId && hidCarId.value > 0) {
            mdvalue = (hidBsId || hidBsId.value != "") ? hidBsId.value : 0;
            sdvalue = hidCsId ? hidCsId.value : 0;
            cdvalue = hidCarId ? hidCarId.value : 0;
            statId = 101;
        }

        //绑定下拉选择框
        BitA.DropDownListNew({
            container: { master: "master4", serial: "serial4", cartype: "cartype4" },
            include: { serial: "1", cartype: "1" },
            dvalue: { master: mdvalue, serial: sdvalue, cartype: cdvalue },
            datatype: 5,
            callback: {
                serial: function(data) {
                    for (var name in data) {
                        if (data[name].id && data[name].urlSpell && !ADForCheDai.CsAllSpellArray[data[name].id]) {
                            ADForCheDai.CsAllSpellArray[data[name].id] = data[name].urlSpell;
                        }
                    }
                }
            },
            onchange: {
                master: function(data) {
                    //初始化下拉组件(车系)文字颜色
                    var spanTag = $("#serial4");
                    spanTag[0].style.color = "black";
                },
                serial: function(data) {
                    //初始化下拉组件(车型)文字颜色
                    var spanTag = $("#cartype4");
                    spanTag[0].style.color = "black";
                },
                cartype: function(data) {
                    //$("#hidCarID").val(data.id);
                    //resetPrice(data.id, webSiteBaseUrl);
                    //InitCheChuanValue();
                    //calcAutoCashAll();
                    //ADForCheDai.SetCheDaiADLink(data.id);
                    location.href = "/gouchejisuanqi/?carid=" + data.id;
                }
            }
        });


    </script>
</div>
<div id="theanchor"></div>
<div class="jisuanqi-box">
    <ul class="rela-ul">
        <li>
            <div class="l-box">
                <span class="fl">裸车价格</span>
                <div class="form-group">
                    <div class="input-group">
                        <input id="txtMoney" type="text" class="input input-default" maxlength="8" onfocus="if (value == '0') {value = '';}"
                               onblur="if (value == '') {value = '0';}InitPaiAndChuan(this.value);" onkeyup="InitPaiAndChuan(this.value)">
                        元 <a href="#" class="butie" style="display: none;" title="这里请输入鼠标移上出现的相关文字 相关文字相关文字 相关文字 ">补贴</a>
                    </div>
                </div>
            </div>
            <div class="r-box"></div>
            <div class="ad-js-300">
                <ins id="div_50f239d7-bbe8-44c1-bf2e-7ef4061bb4bb" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="50f239d7-bbe8-44c1-bf2e-7ef4061bb4bb"></ins>
            </div>
            <div class="clear"></div>
        </li>
      <%--  <% if (int.Parse(hidCarID.Value) > 0 || hidCarPrice.Value != "0")
           { %>--%>
                <li id="liDispaly2" class="bg" <%= int.Parse(hidCarID.Value) > 0 || hidCarPrice.Value != "0" ? "":"style=\"display:none;\"" %>>
                    <div class="l-box w360">
                <p class="fonts2">
                    全款购车价：<em class="red16"><label id="lblTopTotal"></label>元</em>
                </p>
                <p class="suomin">
                    首付<label id="lblShouFu"></label>元，月供<label id="lblYueGong"></label>元 
                    <a id="loanBuyCar" target="_blank" href="/qichedaikuanjisuanqi/">
                        贷款购车&gt;&gt;
                    </a>
                </p>
            </div>
                    <div class="clear"></div>
                </li> 
         <%--  <% }
           else
           { %>--%>
               <li id="liDispaly1" class="bg h-atuo" <%= int.Parse(hidCarID.Value) > 0 || hidCarPrice.Value != "0" ? "style=\"display:none;\"":"" %>>
                    <p class="no_mess">
                        请选择车款或输入裸车价格
                    </p>
               </li>
          <%-- <% } %>--%>
    </ul>
    <p class="suomin txt_right">此结果仅供参考，实际应缴费以当地为准</p>
</div>

<!------------------------必要花费开始------------------------>
<div class="jsq-com-box">
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
            <col width="229px"/>
            <col width="232px"/>
            <col width="264px"/>
            <col/>
        </colgroup>
        <tbody>
        <tr>
            <th>购置税</th>
            <td class="r_align">
                <div class="jiage">
                    <em>
                        <label id="lblAcquisitionTax">
                        </label>
                    </em>元
                </div>
            </td>
            <td id="gouZhiShuiDesc">&nbsp;</td>
            <td>
                <span class="fl yiwen_box">购置附加税=购车型（1+17%）购置税率
                    <a class="yiwenicon" desvalue="购置税解释说明" href="javascript:;" onmouseover="showjs('AcquisitionTax');" onmouseout="HideById('AcquisitionTax')">?
                        <div id="AcquisitionTax" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>购置税</h6>
                            <p>
                                车辆购置税是对在我国境内购置规定车辆的单位和个人征收的一种税，它由车辆购置附加费演变而来。现行车辆购置税法的基本规范，是从2001年1月1日起实施的《中华人民共和国车辆购置税暂行条例》。车辆购置税的纳税人为购置（包括购买、进口、自产、受赠、获奖或以其他方式取得并自用）应税车辆的单位和个人，征税范围为汽车、摩托车、电车、挂车、农用运输车，税率为10%，应纳税额的计算公式为：应纳税额=计税价格×税率。（2009年1月20日至12月31日，对1.6升及以下排量乘用车减按5%征收车辆购置税。自2010年1月1日至12月31日，对1.6升及以下排量乘用车减按7.5%征收车辆购置税。从2015年10月1日到2016年12月31日，对购买1.6升及以下排量乘用车实施减半征收车辆购置税的优惠政策，从2017年1月1日到2017年12月31日，对购买1.6升及以下排量乘用车实施7.5%的税率征收车辆购置税
）
                            </p>
                        </div>
                    </a>
                </span>
            </td>
        </tr>
        <tr>
            <th>上牌费用</th>
            <td class="r_align">
                <div class="jiage">
                    <div class="form-group">
                        <div class="input-group">
                            <input maxlength="4" id="txtChePai" type="text" class="input input-default" value="500" onkeyup="value = value.replace(/(\D)/g, '');calcAutoCashAll();"
                                   onfocus="if (value == '0') {value = '';}" onblur="if (value == '') {value = '0';}calcAutoCashAll();"/>元
                        </div>
                    </div>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>
                <span class="fl yiwen_box">可手动修改，不同地区费用不同
                    <a class="yiwenicon" desvalue="上牌费用解释说明" href="javascript:;" onmouseover="showjs('shangPai');" onmouseout="HideById('shangPai')">?
                        <div id="shangPai" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>上牌费用</h6>
                            <p>
                                通常商家提供的一条龙服务收费约500元，个人办理约373元，其中工商验证、出库150元、移动证30元、环保卡3元、拓号费40元、行驶证相片20元、托盘费130元
                            </p>
                        </div>
                    </a>
                </span>
            </td>
        </tr>
        <tr>
            <th>车船使用税</th>
            <td class="r_align">
                <div class="jiage">
                    <div class="form-group">
                        <div class="input-group">
                            <input id="txtVehicleTax" class="input input-default" type="text" maxlength="4" onkeyup="value = value.replace(/(\D)/g, '');calcAutoCashAll();"
                                   onfocus="if (value == '0') {value = '';}"
                                   onblur="if (value == '') {value = '0';}calcAutoCashAll();"/>元
                        </div>
                    </div>
                </div>
            </td>
            <td class="w180" id="divCashVehicleAndVesselTaxMessage">
            </td>
            <script type="text/javascript">
                function showCheChuanList() {
                    $("#vehicleTax .drop-layer").toggle();
                    $(".drop-layer").not("#vehicleTax .drop-layer").hide();
                }
            </script>
            <td>
                <span class="fl yiwen_box">按排量收取费用
                    <a class="yiwenicon" desvalue="车船使用税解释说明" href="javascript:;" onmouseover="showjs('cheChuanTax');" onmouseout="HideById('cheChuanTax')">?
                        <div id="cheChuanTax" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>车船使用税</h6>
                            <p>
                                各省不统一，以北京为例(单位/年)。1.0L(含)以下300元；1.0-1.6L(含)420元；1.6-2.0L(含)480元；2.0-2.5L(含)900元；2.5-3.0L(含)1920元；3.0-4.0L(含)3480元；4.0L以上5280元；不足一年按当年剩余月算。<br />
                                车船使用税是对行驶于公共道路的车辆和航行于国内河流、湖泊或领海口岸的船舶,按照其种类(如机动车辆、非机动车辆、载人汽车、载货汽车等)、吨位和规定的税额计算征收的一种使用行为税。
                            </p>
                        </div>
                    </a>
                </span>
            </td>
        </tr>
        <tr>
            <th>交强险</th>
            <td class="r_align">
                <div class="jiage">
                    <em>
                        <label id="lblCompulsory">
                        </label>
                    </em>元
                </div>
            </td>
            <td class="w180">
                <div class="form-group">
                    <div class="input-group">
                        <div id="selCompulsory" class="input input-w-sm" style="cursor: pointer" onclick="showList()">
                            <span id="950">家用6座以下</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="950" href="javascript:;">家用6座以下</a>
                                <a id="1100" href="javascript:;">家用6座及以上</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showList() {
                                $("#selCompulsory .drop-layer").toggle();
                                $(".drop-layer").not("#selCompulsory .drop-layer").hide();
                            }

                            $("#selCompulsory .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selCompulsory").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                    </div>
                </div>
            </td>
            <td>
                <span class="fl yiwen_box">国家强制保险
                    <a class="yiwenicon" desvalue="交强险解释说明" href="javascript:;" onmouseover="showjs('jiaoQiagnX');" onmouseout="HideById('jiaoQiagnX')">?
                        <div id="jiaoQiagnX" class="prompt-paragraph-layer" style="display: none">
                         <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>交强险</h6>
                            <p>
                                家用6座以下950元/年，家用6座及以上1100元/年<br />
                                机动车交通事故责任强制保险是我国首个由国家法律规定实行的强制保险制度。《机动车交通事故责任强制保险条例》规定：交强险是由保险公司对被保险机动车发生道路交通事故造成受害人(不包括本车人员和被保险人)的人身伤亡、财产损失，在责任限额内予以赔偿的强制性责任保险。<br />
                                交强险有责限额分为死亡伤残赔偿限额110000元、医疗费用赔偿限额10000元、财产损失赔偿限额2000元以及被保险人在道路交通事故中无责任的赔偿限额。无责的赔偿限额死亡伤残赔偿限额11000元、医疗费用赔偿限额1000元、财产损失赔偿限额100元。<br />
                                责任限额是指被保险机动车发生道路交通事故，保险公司对每次保险事故所有受害人的人身伤亡和财产损失所承担的最高赔偿金额。
                            </p>
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
<div class="jsq-com-box jsq-mb0">
<div class="titbox">
    <h4>商业保险</h4>
    <div class="red_num">
        <span class="">小计：</span>
        <label id="lblCommonTotal"></label>元
    </div>
    <div id="businessHeader" class="tab">
        <a href="javascript:void(0);" onclick="JiBenBaoZ()">基本保障</a> | <a href="javascript:void(0);"
                                                                          onclick="GaoXingJ()">
            高性价比
        </a> | <a class="current" href="javascript:void(0);" onclick="calcBusinessTotalIncludeState()">新车主全面保障</a>
    </div>
    <script type="text/javascript">
        //车辆损失险 第三者责任险 不计免赔
        function JiBenBaoZ() {
            $("#businessHeader a").eq(0).attr("class", "current");
            $("#businessHeader a").eq(1).attr("class", "");
            $("#businessHeader a").eq(2).attr("class", "");
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
            calcAutoCashAll();
        }

        //第三者责任险 车辆损失险 不计免赔 乘客坐位责任险 司机坐位责任险
        function GaoXingJ() {
            $("#businessHeader a").eq(0).attr("class", "");
            $("#businessHeader a").eq(1).attr("class", "current");
            $("#businessHeader a").eq(2).attr("class", "");
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
            calcAutoCashAll();
        }

        function calcBusinessTotalIncludeState() {
            $("#businessHeader a").eq(0).attr("class", "");
            $("#businessHeader a").eq(1).attr("class", "");
            $("#businessHeader a").eq(2).attr("class", "current");
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
            calcAutoCashAll();
        }
    </script>
</div>
<table id="calBusiness" width="100%" cellspacing="0" cellpadding="0" border="0">
<colgroup>
    <col width="229px"/>
    <col width="232px"/>
    <col width="264px"/>
    <col/>
</colgroup>
<tbody>
<tr>
    <th>
        <input id="chkTPL" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/> 第三者责任险
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblTPL"></label>
            </em>元
        </div>
    </td>
    <td class="w140">
        <div class="form-group">
            <div class="input-group">
                <div id="selTPL" class="input input-w-sm" style="cursor: pointer" onclick="showTPLList()">
                            <span id="200000">20万</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="50000" href="javascript:;">5万</a>
                                <a id="100000" href="javascript:;">10万</a>
                                <a id="200000" href="javascript:;">20万</a>
                                <a id="500000" href="javascript:;">50万</a>
                                <a id="1000000" href="javascript:;">100万</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showTPLList() {
                                $("#selTPL .drop-layer").toggle();
                                $(".drop-layer").not("#selTPL .drop-layer").hide();
                            }

                            $("#selTPL .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selTPL").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                赔付额度
            </div>
        </div>
    </td>
    <td>
        <span class="fl yiwen_box">发生车险事故时，赔偿对第三方造成的人身及财产损失 
                    <a class="yiwenicon" desvalue="第三者责任险解释说明" href="javascript:;" onmouseover="showjs('diSanFang');" onmouseout="HideById('diSanFang')">?
                        <div id="diSanFang" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>第三方责任险</h6>
                            <p>
                                第三者责任险是指被保险人或其允许的驾驶人员在使用保险车辆过程中发生意外事故，致使第三者遭受人身伤亡或财产直接损毁，依法应当由被保险人承担的经济责任，保险公司负责赔偿。同时，若经保险公司书面同意，被保险人因此发生仲裁或诉讼费用的，保险公司在责任限额以外赔偿，但最高不超过责任限额的30％。因为交强险在对第三者的财产损失和医疗费用部分赔偿较低，可考虑购买第三者责任险作为交强险的补充。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkCarDamage" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkCarDamage">车辆损失险</label>
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblCarDamage">
                </label>
            </em>元
        </div>
    </td>
    <td>&nbsp;</td>
    <td>
        <span class="fl yiwen_box">车子发生碰撞，赔偿自己爱车损失的费用 
                    <a class="yiwenicon" desvalue="车辆损失险解释说明" href="javascript:;" onmouseover="showjs('sunshi');" onmouseout="HideById('sunshi')">?
                        <div id="sunshi" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                frameborder="0" src="about:blank"></iframe>
                                <em class="jt"></em><h6>车辆损失险</h6>                                
                                <p>
                                    车辆损失险-裸车价格*费率+基础保费<br />
                                    车辆损失险是车辆保险中用途最广泛的险种，它负责赔偿由于自然灾害和意外事故造成的自己车辆的损失。无论是小剐小蹭，还是损坏严重，都可以由保险公司来支付修理费用。<br />
                                    被保险人或其允许的合格驾驶员在使用保险车辆过程中，因下列原因造成保险车辆的损失，保险公司负责赔偿：1．碰撞、倾覆；2．火灾、爆炸；3．外界物体倒塌、空中运行物体坠落、保险车辆行驶中平行坠落；4．雷击、暴风、龙卷风、暴雨、洪水、海啸、地陷、冰陷、崖崩、雪崩、雹灾、泥石流、滑坡；5.
                                载运保险车辆的渡船遭受自然灾害（只限于有驾驶员随车照料者）。<br />
                                    发生保险事故时，被保险人或其允许的合格驾驶员对保险车辆采取施救、保护措施所支出的合理费用，保险公司负责赔偿。但此项费用的最高赔偿金额以责任限额为限。
                                </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkCarTheft" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkCarTheft">全车盗抢险</label>
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
                    <a class="yiwenicon" desvalue="*" href="javascript:;" onmouseover="showjs('daoQiang');" onmouseout="HideById('daoQiang')">?
                        <div id="daoQiang" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>全车盗抢险</h6>
                            <p>
                                全车盗抢险=裸车价格*费率+基础保费<br />
                                指保险车辆全车被盗窃、被抢劫、被抢夺，经县级以上公安刑侦部门立案侦查证实满一定时间没有下落的，由保险公司在保险金额内予以赔偿。如果是车辆的某些零部件被盗抢，如轮胎被盗抢、车内财产被盗抢、后备箱内的物品丢失，保险公司均不负责赔偿。
                                但是，对于车辆被盗抢期间内，保险车辆上零部件的损坏、丢失，保险公司一般负责赔偿。<br />
                                全车盗抢险为附加险，必须在投保车辆损失险之后方可投保该险种。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkBreakageOfGlass" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkBreakageOfGlass">玻璃单独破碎险</label>
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblBreakageOfGlass">
                </label>
            </em>元
        </div>
    </td>
    <td class="w140">
        <div class="form-group">
            <div class="input-group">
                <div id="selBreakageOfGlass" class="input input-w-sm" style="cursor: pointer" onclick="showBreakageOfGlassList()">
                            <span id="1">国产</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="1" href="javascript:;">国产</a>
                                <a id="0" href="javascript:;">进口</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showBreakageOfGlassList() {
                                $("#selBreakageOfGlass .drop-layer").toggle();
                                $(".drop-layer").not("#selBreakageOfGlass .drop-layer").hide();
                            }

                            $("#selBreakageOfGlass .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selBreakageOfGlass").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                赔付额度
            </div>
        </div>
    </td>
    <td>
        <span class="fl yiwen_box">负责赔偿保险车辆在使用过程中，发生车窗、挡风玻璃的单独破碎损失
                    <a class="yiwenicon" desvalue="玻璃单独破碎险解释说明" href="javascript:;" onmouseover="showjs('boLi');" onmouseout="HideById('boLi')">?
                        <div id="boLi" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>玻璃单独破碎险</h6>
                            <p>
                                玻璃单独破碎险=裸车价格*费率<br />
                                负责赔偿保险车辆在使用过程中，发生本车玻璃发生单独破碎的保险公司按照保险合同进行赔偿。玻璃单独破碎险中的玻璃是指风档玻璃和车窗玻璃，如果车灯、车镜玻璃破碎及车辆维修过程中造成的破碎，保险公司不承担赔偿责任。<br />
                                玻璃单独破碎险为附加险，必须在投保车辆损失险之后方可投保该险种。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkSelfignite" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkSelfignite">自燃损失险</label>
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
                    <a class="yiwenicon" desvalue="自燃损失险解释说明" href="javascript:;" onmouseover="showjs('ziRan');" onmouseout="HideById('ziRan')">?
                        <div id="ziRan" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                frameborder="0" src="about:blank"></iframe>                            
                            <em class="jt"></em><h6>自燃损失险</h6>
                            <p>
                                自燃损失险=裸车价格×0.15%<br />
                                负责赔偿因本车电器、线路、供油系统发生故障及运载货物自身原因起火造成车辆本身的损失。当车辆发生部分损失，按照实际修复费用赔偿修理费。如果车辆自燃造成整体烧毁或已经失去修理价值，则按照出险时车辆的实际价值赔偿，但不超过责任限额。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkAbatement" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkAbatement">不计免赔特约险</label>
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
                    <a class="yiwenicon" desvalue="不计免赔特约险解释说明" href="javascript:;" onmouseover="showjs('mianPei');" onmouseout="HideById('mianPei')">?
                        <div id="mianPei" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>不计免赔特约险</h6>
                            <p>
                                (车辆损失险+第三者责任险)×20%<br />
                                负责赔偿在车损险和第三者责任险中应由被保险人自己承担的免赔金额，即100%赔付。<br />
                                不计免赔特约险为附加险，必须在投保车损险和第三者责任险之后方可投保该险种。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkLimitofPassenger" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkLimitofPassenger">乘客座位责任险</label>
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblLimitOfPassenger">
                </label>
            </em>元
        </div>
    </td>
    <td class="w140">
        <div class="form-group">
            <div class="input-group">
                <div id="selLimitofPassenger" class="input input-w-sm" style="cursor: pointer" onclick="showLimitofPassengerList()">
                            <span id="20000">2万</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="10000" href="javascript:;">1万</a>
                                <a id="20000" href="javascript:;">2万</a>
                                <a id="30000" href="javascript:;">3万</a>
                                <a id="40000" href="javascript:;">4万</a>
                                <a id="50000" href="javascript:;">5万</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showLimitofPassengerList() {
                                $("#selLimitofPassenger .drop-layer").toggle();
                                $(".drop-layer").not("#selLimitofPassenger .drop-layer").hide();
                            }

                            $("#selLimitofPassenger .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selLimitofPassenger").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                赔付额度
            </div>
        </div>
    </td>
    <td>
        <span class="fl yiwen_box">发生车险事故时，赔偿车内乘客的伤亡和医疗赔偿费用 
                    <a class="yiwenicon" desvalue="乘客座位责任险解释说明" href="javascript:;" onmouseover="showjs('chengKe');" onmouseout="HideById('chengKe')">?
                        <div id="chengKe" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>乘客坐位责任险</h6>
                            <p>
                                乘客责任险=保额*费率<br />
                                被保险人允许的合格驾驶员在使用保险车辆过程中发生保险事故，致使车内乘客人身伤亡，依法应由被保险人承担的赔偿责任，保险人依照保险合同的约定给予赔偿。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkLimitofDriver" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkLimitofDriver">司机座位责任险</label>
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblLimitOfDriver">
                </label>
            </em>元
        </div>
    </td>
    <td class="w140">
        <div class="form-group">
            <div class="input-group">
                <div id="selLimitofDriver" class="input input-w-sm" style="cursor: pointer" onclick="showLimitofDriverList()">
                            <span id="20000">2万</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="10000" href="javascript:;">1万</a>
                                <a id="20000" href="javascript:;">2万</a>
                                <a id="30000" href="javascript:;">3万</a>
                                <a id="40000" href="javascript:;">4万</a>
                                <a id="50000" href="javascript:;">5万</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showLimitofDriverList() {
                                $("#selLimitofDriver .drop-layer").toggle();
                                $(".drop-layer").not("#selLimitofDriver .drop-layer").hide();
                            }
                            $("#selLimitofDriver .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selLimitofDriver").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                赔付额度
            </div>
        </div>
    </td>
    <td>
        <span class="fl yiwen_box">发生车险事故时，赔偿车内司机的伤亡和医疗赔偿费用 
                    <a class="yiwenicon" desvalue="司机座位责任险解释说明" href="javascript:;" onmouseover="showjs('siJi');" onmouseout="HideById('siJi')">?
                        <div id="siJi" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>司机坐位责任险</h6>
                            <p>
                                司机责任险=保额*费率<br />
                                统称为车上责任险，包括司机座位和乘客座位，主要是指在发生意外情况下，保险公司对司机座位的人员和乘客的人身安全进行赔偿。<br />
                                严格来说，司机责任险并不是一个独立的险种，而是商业车险中车上人员责任险的一部分，除此之外，车主还可以为乘客座位投保，一般选择的赔偿限额为1-5万元
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkCarDamageDW" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkCarDamageDW">车身划痕险</label>
    </th>
    <td class="r_align">
        <div class="jiage">
            <em>
                <label id="lblCarDamageDW">
                </label>
            </em>元
        </div>
    </td>
    <td class="w140">
        <div class="form-group">
            <div class="input-group">
                <div id="selCarDamageDW" class="input input-w-sm" style="cursor: pointer" onclick="showCarDamageDWList()">
                            <span id="5000">5千</span>
                            <i class="input-arrow-down"></i>
                            <div class="drop-layer" style="display: none">
                                <a id="2000" href="javascript:;">2千</a>
                                <a id="5000" href="javascript:;">5千</a>
                                <a id="10000" href="javascript:;">1万</a>
                                <a id="20000" href="javascript:;">2万</a>
                            </div>
                        </div>
                        <script type="text/javascript">
                            function showCarDamageDWList() {
                                $("#selCarDamageDW .drop-layer").toggle();
                                $(".drop-layer").not("#selCarDamageDW .drop-layer").hide();
                            }

                            $("#selCarDamageDW .drop-layer a").click(function (event) {
                                var spanText = $(this).text();
                                var spanId = $(this).attr("id");
                                var span = $("#selCarDamageDW").children("span");
                                span.text(spanText);
                                span.attr("id", spanId);
                                $(this).parent().hide();
                                calcAutoCashAll();
                                event.stopPropagation();
                            });
                        </script>
                赔付额度
           </div>
       </div>
    </td>
    <td>
        <span class="fl yiwen_box">负责无碰撞痕迹的车身表面油漆单独划伤的损失
                    <a class="yiwenicon" desvalue="车身划痕险解释说明" href="javascript:;" onmouseover="showjs('huaHen');" onmouseout="HideById('huaHen')">?
                        <div id="huaHen" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>车身划痕险</h6>
                            <p>
                                无明显碰撞痕迹的车身划痕损失，保险公司负责赔偿。<br />
                                车身划痕险为附加险，必须在投保车辆损失险之后方可投保该险种。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
<tr>
    <th>
        <input id="chkEngine" type="checkbox" checked="checked" onclick="calcAutoCashAll();"/>
        <label for="chkEngine">涉水险/发动机特别损失险</label>
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
                    <a class="yiwenicon" desvalue="发动机特别损失险解释说明" href="javascript:;" onmouseover="showjs('faDongJi');" onmouseout="HideById('faDongJi')">?
                        <div id="faDongJi" class="prompt-paragraph-layer" style="display: none">
                            <iframe style="bottom: 0; left: 0; position: absolute; scrolling: no; width: 400px; z-index: -1;"
                                                frameborder="0" src="about:blank"></iframe>
                            <em class="jt"></em><h6>涉水险/发动机特别损失险</h6>
                            <p>
                                发动机特别损失险=车损险*5%<br />
                                涉水险或称汽车损失保险、发动机特别损失险，各个保险公司叫法不一样但本质一致，这是一种新衍生的险种，均指车主为发动机购买的附加险。<br />
                                这个险种主要是指车主为发动机购买的附加险。它主要是保障车辆在积水路面涉水行驶或被水淹后致使发动机损坏可给予赔偿。即使被水淹后车主还强行启动发动机而造成了损害，保险公司仍然给予赔偿。当然保险公司不一样，条款就不大一样，投保时可以查阅下各个保险公司条款内容。
                            </p>
                        </div>
                    </a>
                </span>
    </td>
</tr>
</tbody>
</table>

</div>
<!------------------------商业保险结束------------------------>

   <%-- <% if (int.Parse(hidCarID.Value) > 0 || hidCarPrice.Value != "0")
           { %>--%>
                <div id="bottomSummary" class="summary-box" <%= int.Parse(hidCarID.Value) > 0 || hidCarPrice.Value != "0" ? "":"style=\"display:none;\"" %>>
                    <div class="all-smmary">
        全款购车价：<em>
            <label id="lblBottomTotal"></label>
            元
        </em>
    </div>
                </div>
           <%--<% }%>--%>
</div>
<script type="text/javascript">
    (function($) {
        $(function() {
            //解释说明
            //$("a[desValue]").click(function(e) {
            //    $(".yiwenicon.z30").removeAttr("style"); //处理连续点击问号的情况
            //    e.stopPropagation();
            //    $(this).css("zIndex", 100);

            //});
            $("p").bind("click", function(e) {
                e.stopPropagation();
            });
            //------------------------必要花费--------------------------------------------


            //$(document).on("change", "#vehicleTax", function() {
            //    CalculateVehAndVesselTax();
            //    calcAutoCashAll();
            //});
            
            

            //------------------------商业保险--------------------------------------------
        });
    })(jQuery)
</script>
<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
<script type="text/javascript">
    $("#txtMoney").bind("keyup", function () {
        var value = parseInt($(this).val().replace(/\b(0+) | ([^0-9.])/g, ''));
        if (isNaN(value)) {
            value = 0;
        }
        $(this).val(value);
        if (value > 0) {
            $("#liDispaly1").hide();
            $("#liDispaly2").show();
            $("#bottomSummary").show();
        }
        else {
            $("#liDispaly2").hide();
            $("#liDispaly1").show();
            $("#bottomSummary").hide();
        }
        calcAutoCashAll();
        //calcAutoLoanAll();
    });

    var cityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201";

    //补贴
    function getSubsidy(carId, cityId) {
        if (!(carId > 0)) return;
        $.ajax({
            url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcarsunsidy&carid=" + carId + "&cityid=" + cityId + "",
            dataType: "jsonp",
            jsonpCallback: "getSubsidyCallback",
            cache: true,
            success: function(data) {
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

    getSubsidy(hidCarId.value, cityId);
</script>
<!--#include file="~/htmlV2/footer2016.shtml"-->
</body>
</html>