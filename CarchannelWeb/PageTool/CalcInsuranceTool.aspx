<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalcInsuranceTool.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.CalcInsuranceTool" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【购车保险费用计算】购车哪些保险必须买？-易车网</title>
    <meta name="Keywords" content="购车保险费用计算,购车保险计算器,异地购车 保险,购车 保险计算,购车哪些保险必须买，购车保险种类,购车买保险,购车需要买哪些保险,贷款购车保险,购车保险哪家好" />
    <meta name="Description" content="购车保险费用计算：易车网购车小工具包括,贷款购车计算器、购车费用计算器,为您提供准确的购车费用信息。帮助您方便快捷的购车!  " />
    <!--#include file="~/ushtml/0000/yiche_2015_jusuanqi_style-994.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlistnewV2.min.js?v=20160119"></script>
    <script type="text/javascript" src="<%= BitAuto.CarChannel.Common.WebConfig.StaticFileBaseUrl %>jsnew/CarCalculator.js?v=201508061501"></script>
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
            var theid = document.getElementById("liDispaly2");
            var theanchor = document.getElementById("theanchor");
            var thetop = document.documentElement.scrollTop + document.body.scrollTop;
            if (thetop > theanchor.offsetTop + 60) {
                if (theid.className == "bg fixed-top") { return false; }
                theid.className = "bg fixed-top";
            } else {
                if (theid.className == "") { return false; }
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
        var webSiteBaseUrl = "<%= BitAuto.CarChannel.Common.WebConfig.WebSiteBaseUrl %>";
        function initData() {
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

            var hidCarPrice = $("#hidCarPrice").val();
            $('#txtMoney').val(hidCarPrice);
            var carId = document.getElementById("hidCarID").value;

            //var flag = parseInt(hidCarPrice) > 0; //可能从其他页面传递价格
            //if (!flag) {
            //    if (carId != -2) {
            //        resetPriceInsurance(carId, webSiteBaseUrl);
            //    }
            //} else {
            //    setCalcToolUrlByPrice(hidCarPrice);
            //}

            var carPricePara = getQueryString("CarPrice");
            if (parseInt(carId) > 0) {
                resetPriceInsurance(carId, webSiteBaseUrl);
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

        function calcCompulsoryNew() {
            var compulsoryValue = document.getElementById("selCompulsory").value;
            SetSpanValueByBrowerType('lblCompulsory', formatCurrency(compulsoryValue));
            if (isNaN($('#txtMoney').val()) || $('#txtMoney').val().length == 0 || $('#txtMoney').val() == "0") {
                jQuery("#lblCompulsory").html("0");
                jQuery("#lblTotalCompulsory").html("0");
            } else {
                SetSpanValueByBrowerType('lblTotalCompulsory', formatCurrency(compulsoryValue));
            }
        }

        function calcAll() {

            if (!checkMoneyValidationNew()) {
                return false;
            }

            if ($('#txtMoney').val() == 0) {
                jQuery('#lblTotalCompulsory').html('0'); //强制保险
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
                //市场报价=13*0.7     
                calcMarket();
                //return;
            }

            //if ($('#txtMoney').val() != "0") {
            //交强险
            calcCompulsoryNew();

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
            //商业保险合计
            calcCommonTotalNew();
            
            calcMarket();
            calcCompany();
            //}
        }

        //商业保险合计
        function calcCommonTotalNew() {
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
        //市场价
        function calcMarket() {
            var MarketTotal = formatCurrency(parseInt(GetIntValue(jQuery('#lblCompulsory').html()) + (parseFloat(calcCommonTotalNew()) * 0.9)));
            SetSpanValueByBrowerType('lblMarket', MarketTotal);
            SetSpanValueByBrowerType('lblBottomMarket', MarketTotal);
        }
        //官方指导价
        function calcCompany() {
            var companyTotal = parseInt(GetIntValue($('#lblCompulsory').html()) + parseFloat(calcCommonTotalNew()));
            $("#guanFangPrice").html(formatCurrency(companyTotal));
            $("#gFangPriceBottom").html(formatCurrency(companyTotal));
        }

        function resetValue() {
            initData();
            calcAll();
        }
        
        function InitPai(v) {
            var $txtMoney = $("#txtMoney");
            var $hidCarID = $("#hidCarID");
            var $txtChePai = $("#txtChePai");

            $txtMoney.val(v.replace(/(\D)/g, ''));
            if (parseInt($hidCarID.val()) <= 0) {
                if ($txtMoney.val() != "" || parseInt($txtMoney.val()) > 0) {
                    if ($txtChePai.val() == "" || $txtChePai.val() == "0") {
                        $txtChePai.val(500);
                    }
                }
                setCalcToolUrl(-1, -1);
            } else {
                if ($txtMoney.val() != "" && parseInt($txtMoney.val()) > 0) {
                    if ($txtChePai.val() == "" || $txtChePai.val() == "0") {
                        $txtChePai.val(500);
                    }
                    setCalcToolUrl($hidCarID.val(), parseInt($txtMoney.val()));
                }
            }
            calcAll();
        }

        $(function () {
            //解释说明
            $("a.yiwenicon.z30").bind("click", function (e) {
                $(".yiwenicon.z30").removeAttr("style"); //处理连续点击问号的情况
                e.stopPropagation();
                $(this).css("zIndex", 100);
            });
            
            $("p").bind("click", function (e) {
                e.stopPropagation();
            });
            
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
    <script type="text/javascript">
        <!--
        var tagIframe = null;
        var currentTagId = 34; 	//当前页的标签ID
        -->
    </script>
    <style type="text/css">
        .count { background: transparent url(/car/images/bg-btn.gif) repeat scroll 0 -121px; border: medium none; color: #FFFFFF; cursor: pointer; font-size: 14px; font-weight: bold; height: 26px; line-height: 26px; text-align: center; width: 86px; }
        .t01 .total span { font-size: 24px; font-weight: normal; }
        .t01 .tips { color: #999; padding: 7px; }
        .fixed-top { position: fixed; top: 0; z-index: 9999; }
    </style>
</head>
<body onload="resetValue();">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <input id="hidSeatNum" type="hidden" runat="server" value="0" />
    <input id="hidCarPrice" type="hidden" runat="server" value="0" />
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
                    <em>|</em><a href="#">购车计算器</a>
                </div>
                <!--页头导航_yiche_LOGO结束-->
                <div class="bt_searchNew">
                    <!--#include file="~/html/bt_searchV3.shtml"-->
                </div>
                <div id="divSerialSummaryMianBaoAD" class="top_ad02">
                    <ins id="div_eb7513a3-602b-4b14-812f-ba07a8104da9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="eb7513a3-602b-4b14-812f-ba07a8104da9"></ins>
                </div>
            </div>
            <div class="publicTabNew">
                <ul id="calcTools" class="tab">
                    <li id="xinche_index"><a href="/gouchejisuanqi/">全款</a></li>
                    <li id="xinche_ssxc"><a href="/qichedaikuanjisuanqi/">贷款</a></li>
                    <li id="xinche_1822" class="current"><a href="/qichebaoxianjisuan/">保险</a></li>
                </ul>
            </div>
        </div>
        <!--互联互通 end-->
        <div class="line-box">
            <div class="title-con">
                <input id="hidBsID" type="hidden" runat="server" value="-1" />
                <input id="hidCsID" type="hidden" runat="server" value="-1" />
                <input id="hidCarID" type="hidden" runat="server" value="-2" />
                <input id="hidRdoPaymentYearsIdx" runat="server" type="hidden" value="2" />
                <input id="hidRdoDownPaymentsIdx" runat="server" type="hidden" value="0" />
                <div class="title-box title-box2">
                    <h4>选择车款</h4>
                    <div class="tableHead_item">
                        <div class="jsq-tit-fbox">
                            <div id="master4" class="sel-item-box" style="z-index: 50">
                            </div>
                        </div>
                        <div class="jsq-tit-fbox">
                            <div id="serial4" class="sel-item-box" style="z-index: 40">
                            </div>
                        </div>
                        <div class="jsq-tit-fbox w236">
                            <div id="cartype4" class="sel-item-box" style="z-index: 30">
                            </div>
                        </div>
                    </div>
                </div>
                <script language="javascript" type="text/javascript">
                    var hidBsId = document.getElementById("hidBsID");
                    var hidCsId = document.getElementById("hidCsID");
                    var hidCarId = document.getElementById("hidCarID");
                    var mdvalue = "0", sdvalue = "0", cdvalue = "0";
                    if (hidCarId && hidCarId.value > 0) {
                        mdvalue = (hidBsId || hidBsId.value != "") ? hidBsId.value : 0;
                        sdvalue = hidCsId ? hidCsId.value : 0;
                        cdvalue = hidCarId ? hidCarId.value : 0;
                    }
                    //绑定下拉选择框
                    BitA.DropDownListNew({
                        container: { master: "master4", serial: "serial4", cartype: "cartype4" },
                        include: { serial: "1", cartype: "1" },
                        dvalue: { master: mdvalue, serial: sdvalue, cartype: cdvalue },
                        datatype: 5,
                        callback: {
                            cartype: function (data) {
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
                                //resetPriceInsurance(data.id, webSiteBaseUrl);
                                //calcAll();
                                location.href = "/qichebaoxianjisuan/?carid=" + data.id;
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
                                    onblur="if (value ==''){value='0';}InitPai(this.value);" onkeyup="value=value.replace(/(\D)/g,'');InitPai(this.value);" />
                                元</span>
                        </div>
                        <div class="r-box">
                        </div>
                        <div class="ad-js-300">
                            <ins id="div_34035aeb-58e3-4f22-a871-85a9bb97ca2c" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="34035aeb-58e3-4f22-a871-85a9bb97ca2c"></ins>
                        </div>
                        <div class="clear">
                        </div>
                    </li>
                    <li id="liDispaly1" class="bg">
                        <p class="no_mess">
                            请选择车款或输入裸车价格
                        </p>
                    </li>
                    <li id="liDispaly2" class="bg" style="display: none;">
                        <div class="padd10">
                            <div class="w360">
                                <span class="fonts2">新车市场价：<em id="lblMarket" class="red16"></em><em class="red16">元</em></span>
                                <p class="sm">官方指导价 <em id="guanFangPrice"></em>元(新车商业险9折)</p>
                            </div>
                            <div class="button_orange button_140_36"><a data-channelid="2.92.535" target="_blank" href="http://baoxian.daikuan.com?from=NPH0005">车主续险优惠</a></div> 
                            <div class="clear"></div>
                        </div>
                    </li>
                </ul>
                <p class="suomin txt_right">
                    此结果仅供参考，实际应缴费以当地为准
                </p>
            </div>
            <!------------------------强制保险开始------------------------->
            <div class="jsq_com_box">
                <div class="titbox">
                    <h4>强制保险</h4>
                    <div class="red_num">
                        <span>小计：</span>
                        <label id="lblTotalCompulsory">0</label>元
                    </div>
                </div>
                <table width="100%" id="datalist" cellspacing="0" cellpadding="0" border="0">
                    <colgroup>
                        <col width="200px">
                        <col width="180px">
                        <col width="210px">
                        <col>
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>交强险
                            </th>
                            <td class="r_align">
                                <div class="jiage">
                                    <em>
                                        <label id="lblCompulsory">
                                            0</label></em>元
                                </div>
                            </td>
                            <td>
                                <select id="selCompulsory" class="f-w150 f-curr" onchange="calcAll();">
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
            <!------------------------强制保险结束---------------------->
            <!------------------------商业保险开始------------------------>
            <div id="businessHeader" class="jsq_com_box">
                <div class="titbox">
                    <h4>商业保险</h4>
                    <div class="red_num">
                        <span>小计：</span>
                        <label id="lblCommonTotal">
                        </label>
                        元
                    </div>
                    <div class="tab">
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
                            calcAll();
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
                            calcAll();
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
                            calcAll();
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
                                <input id="chkTPL" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <select id="selTPL" class="f-w100 f-curr" onchange="calcAll();">
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
                                            <iframe style="position: absolute; z-index: -1; width: 400px; bottom: 0; left: 0; scrolling: no;"
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
                                <input id="chkCarDamage" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <input id="chkAbatement" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <input id="chkCarTheft" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <input id="chkBreakageOfGlass" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <select id="selBreakageOfGlass" class="f-w100 f-curr" onchange="calcAll()">
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
                                <input id="chkSelfignite" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <input id="chkEngine" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <input id="chkCarDamageDW" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <select id="selCarDamageDW" class="f-w100 f-curr" onchange="calcAll()">
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
                                <input id="chkLimitofDriver" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <select id="selLimitofDriver" class="f-w100 f-curr" onchange="calcAll()">
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
                                <input id="chkLimitofPassenger" type="checkbox" checked="checked" onclick="calcAll();" />
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
                                <select id="selLimitofPassenger" class="f-w100 f-curr" onchange="calcAll()">
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
            <div id="bottomSummary" class="summary-box">
                <div class="all-smmary">新车保险市场价：<em id="lblBottomMarket"></em><em>元</em></div>
                <div class="baoxian-txt">官方指导价：<span id="gFangPriceBottom"></span><span>元</span> （注：新车购商业险享9折优惠）</div>
            </div>
        </div>
    </div>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        var cityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201";

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
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/footer2014.shtml"-->
	<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
</body>
</html>
