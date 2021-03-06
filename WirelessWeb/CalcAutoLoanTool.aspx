﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalcAutoLoanTool.aspx.cs"
    Inherits="WirelessWeb.CalcAutoLoanTool" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>计算器_贷款</title>
    <!--#include file="~/ushtml/0000/myiche2015_cube_jisuanqi_style-991.shtml"-->
    <style>
        #master_container {
            background-color: #fff;
        }
    </style>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/ajax.js?v=201209"></script>
</head>

<body>
    <div class="op-nav">
        <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
        <div class="tt-name">
            <a href="http://m.yiche.com/" class="yiche-logo">易车</a><h1><%= serialName %></h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
    <div class="op-nav-mark" style="display: none;"></div>
    <div class="masterBox">
        <input id="hidCarID" type="hidden" value="<%=carId %>" />
        <input id="hidCarPrice" type="hidden" value="<%=carReferPrice %>" />
        <input id="hidCarName" type="hidden" value="<%=title %>" />
        <input id="hidSeatNum" type="hidden" />
        <!--页签导航开始-->
        <div class="first-tags">
            <ul>
                <li><a id="quankuanUrl" href="javascript:void(0)"><span>全款计算</span></a></li>
                <li class="current"><a id="daikuanUrl" href="javascript:void(0)"><span>贷款计算</span></a></li>
                <li><a id="baoxianUrl" href="javascript:void(0)"><span>保险计算</span></a></li>
            </ul>
        </div>
        <!--页签导航结束-->

        <div class="top_layer2 dk-top-layer2">
            <div class="tit">贷款总价</div>
            <div class="num-box" id="totalPrice"><span>元</span></div>
            <p class="dk-top-info">
                首付 <span id="shoufu">0</span>月供(<strong id="yueShu">0</strong>个月) <span id="yueGong">0</span>利息 <span id="liXi">0</span>
            </p>
            <a id="chongZhi" data-channelid="108.108.1064" href="/qichedaikuanjisuanqical/" class="btn-resetting">重置</a>

        </div>
        <div id="floatLayer" class="top_layer2 daikuan-top-layer font-small jsq-top-layer" style="display:none;">
            <div class="four-box-list fout-box-w22">
                <p class="tit">首付</p>
                <p id="shoufuLayer" class="cont">0</p>
            </div>
            <div class="four-box-list fout-box-w28">
                <p class="tit">月供(<strong id="yueShuLayer">0</strong>个月)</p>
                <p id="yueGongLayer" class="cont">0</p>
            </div>
            <div class="four-box-list fout-box-w20">
                <p class="tit">利息</p>
                <p id="liXiLayer" class="cont">0</p>
            </div>
            <div class="four-box-list fout-box-w30">
                <p class="tit">总价</p>
                <p id="totalPriceLayer" class="cont">0</p>
            </div>
        </div>
        <div id="layerdetail" class="font-small">
            <div class="layer_detail">
                <ul>
                    <li>裸车: <span id="luochePrice1">0</span></li>
                    <li id="biYaoLi">必要花费: <span id="biYaoHuaFei1">0</span></li>
                    <li id="shangYeLi">商业险: <span id="shangYeXian1">0</span></li>
                </ul>
            </div>
        </div>
        <div id="topFlag"></div>
        <a id="daikuanGo" href="javascript:void(0)" class="jsq-dk-jiange">低息贷款，立刻申请<em></em></a>
        <div class="xj-form">
            <a href="javascript:void(0);" data-action="brandlayer" data-value="<%=carSerialId %>" class="xj-change-city">
                <span>选择车款</span>
                <strong id="select_carname" class="act"><%=title %></strong>
            </a>
            <div class="xj-user-info-box">
                <div class="xj-user-info">
                    <span>裸车价格</span>
                    <input id="luochePrice2" type="number" class="current">
                </div>
            </div>
        </div>
        <script type="text/javascript">
            var carName = $("#select_carname").text();
            //if (carName.length > 20) {
            //    $("#select_carname").attr("class", "act liahang");
            //}
            //手机大屏小屏判断
            var screenWidth = $(window).width();
            var layerClassName;

            $(function () {
                //jsq-top-layer
                $(document).scroll(function () {
                    var top = $("#topFlag").offset().top;
                    var scrollTop = $(this).scrollTop();
                    // " jsq-top-layer"
                    if (scrollTop >= top) {
                        $("#floatLayer").show();
                    }
                    if (scrollTop < top) {
                        $("#floatLayer").hide();
                    }
                });

                $("#luochePrice2").on("keyup input", function () {
                    var carPrice = $(this).val();
                    if (carPrice.length > 8) {
                        carPrice = carPrice.substr(0, 8);
                        $("#luochePrice2").val(carPrice);
                    }
                    $("#luochePrice1").text(formatCurrency(carPrice));
                    $('#hidCarPrice').val(carPrice);
                    setCalcToolUrl("caridAndPrice", $("#hidCarID").val());
                    calcLoanValue();
                    calcAutoLoanAll();
                });
            });


        </script>
        <div class="xj-form mt15">
            <div class="xj-user-info-box jsq-user-nobg">
                <div class="xj-user-info">
                    <span>首付比例</span>
                    <div id="shoufuDiv" class="jsq-cho-box">
                        <a href="javascript:void(0)">30%</a>
                        <a href="javascript:void(0)">40%</a>
                        <a href="javascript:void(0)">50%</a>
                        <a href="javascript:void(0)">60%</a>
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                $(function () {
                    $("#shoufuDiv a").on("click", function () {
                        $("#shoufuDiv a").attr("class", "");
                        $(this).attr("class", "current");

                        calcLoanValue();
                        calcAutoLoanAll();
                    });
                });
            </script>
            <div class="xj-user-info-box jsq-user-nobg">
                <div class="xj-user-info">
                    <span>还款年限</span>
                    <div id="yearDiv" class="jsq-cho-box jsq-cho-yaer">
                        <a href="javascript:void(0)">5</a>
                        <a href="javascript:void(0)">4</a>
                        <a href="javascript:void(0)">3</a>
                        <a href="javascript:void(0)">2</a>
                        <a href="javascript:void(0)">1</a>
                    </div>
                </div>
            </div>
            <%--<a href="javascript:void(0)" class="xj-change-city">
            <span>贷款利率</span>
            <input id="loanRate" type="text" class="current" value="6.4%">
        </a>--%>
            <div class="xj-user-info-box">
                <div class="xj-user-info">
                    <span>贷款利率</span>
                    <input id="loanRate" type="tel" class="current">
                    <em class="baifenhao current">%</em>
                </div>
            </div>
            <div id="loanFlag"></div>
            <div id="loanLayer" class="jump-pop" style="display: none">
                <span>贷款上限为10%</span>
            </div>
            <script type="text/javascript">
                $(function () {
                    $("#yearDiv a").on("click", function () {
                        $("#yearDiv a").attr("class", "");
                        $(this).attr("class", "current");

                        calcLoanValue();
                        calcAutoLoanAll();
                    });

                    $("#loanRate").on("keyup input", function () {
                        var loanRateValue = $(this).val();
                        if (isNaN(loanRateValue)) {
                            loanRateValue = parseFloat(loanRateValue);
                        }
                        if (loanRateValue.length > 6) {
                            loanRateValue = loanRateValue.substr(0, 6);
                        }
                        $(this).val(loanRateValue);
                        if (parseFloat(loanRateValue) > 10) {
                            var scrollTop = $(document).scrollTop();
                            var top = ($(window).height() - $("#topFlag").height() - 200) / 2;
                            $("#loanLayer").css({ 'z-Index': 999, 'top': (top + scrollTop) + "px" }).show();
                            calcLoanValue();
                            setTimeout(function () {
                                $("#loanLayer").hide();
                            }, 1000);
                        } else {
                            calcAutoLoanAll();
                        }
                    });
                });
            </script>
        </div>
        <div class="jsq-jiange-box">
            <span class="left-tab">必要花费</span>
            <span class="right-tab">小计：<strong id="biYaoHuaFei2">0</strong></span>
        </div>
        <div class="xj-form">
            <div class="xj-user-info-box jsq-user-nobg">
                <div class="xj-user-info">
                    <span>购置税</span>
                    <span id="gouZhiShuiDesc" style="color: #c00; font-size: 1.3rem">免征购置税</span>
                    <strong id="gouZhiShui">0</strong>
                </div>
            </div>
            <a class="xj-change-city">
                <span>交强险</span>
                <strong id="jiaoQiangX" class="act" data-action="popup-ZuoWeiS">0</strong>
            </a>
            <a class="xj-change-city">
                <span>车船使用税</span>
                <strong id="cheChuanTax" class="act" data-action="popup-chechuan">0</strong>
            </a>
            <div class="xj-user-info-box jsq-user-nobg">
                <div class="xj-user-info">
                    <span>上牌费用</span>
                    <strong id="shangPai">0</strong>
                </div>
            </div>
        </div>

        <div class="jsq-jiange-box">
            <span class="left-tab">商业保险</span>
            <span class="right-tab">小计：<strong id="shangYeXian2">0</strong></span>
        </div>

        <div class="jsq-tt-first">
            <h3>推荐套餐</h3>
            <div class="jsq-tab-box">
                <a id="jibenX" href="javascript:void(0);" onclick="JiBenX()">基本险</a>
                <a id="jingjiX" href="javascript:void(0);" onclick="JingJiX()">经济险</a>
                <a id="quanX" href="javascript:void(0);" onclick="QuanX()">全险</a>
            </div>
        </div>
        <script type="text/javascript">
            //第三者责任险 车辆损失险 不计免赔
            function JiBenX() {
                $('#chkDiSanZheX').attr("class", checkedClass);
                $('#chkCheSunShiX').attr("class", checkedClass);
                $('#chkBuJiX').attr("class", checkedClass);

                $('#chkQuanCheX').attr("class", uncheckedClass);
                $('#chkBoLiX').attr("class", uncheckedClass);
                $('#chkZiRanX').attr("class", uncheckedClass);
                $('#chkEngineX').attr("class", uncheckedClass);
                $('#chkChengKeX').attr("class", uncheckedClass);
                $('#chkSiJiX').attr("class", uncheckedClass);
                $('#chkCheShenX').attr("class", uncheckedClass);

                calcLoanValue();
                calcAutoLoanAll();
            }
            //第三者责任险 车辆损失险 不计免赔 乘客坐位责任险 司机坐位责任险
            function JingJiX() {
                $('#chkDiSanZheX').attr("class", checkedClass);
                $('#chkCheSunShiX').attr("class", checkedClass);
                $('#chkBuJiX').attr("class", checkedClass);
                $('#chkChengKeX').attr("class", checkedClass);
                $('#chkSiJiX').attr("class", checkedClass);

                $('#chkQuanCheX').attr("class", uncheckedClass);
                $('#chkBoLiX').attr("class", uncheckedClass);
                $('#chkZiRanX').attr("class", uncheckedClass);
                $('#chkCheShenX').attr("class", uncheckedClass);
                $('#chkEngineX').attr("class", uncheckedClass);

                calcLoanValue();
                calcAutoLoanAll();
            }

            function QuanX() {
                $('#chkDiSanZheX').attr("class", checkedClass);
                $('#chkCheSunShiX').attr("class", checkedClass);
                $('#chkBuJiX').attr("class", checkedClass);
                $('#chkChengKeX').attr("class", checkedClass);
                $('#chkSiJiX').attr("class", checkedClass);
                $('#chkQuanCheX').attr("class", checkedClass);
                $('#chkBoLiX').attr("class", checkedClass);
                $('#chkZiRanX').attr("class", checkedClass);
                $('#chkCheShenX').attr("class", checkedClass);
                $('#chkEngineX').attr("class", checkedClass);

                calcLoanValue();
                calcAutoLoanAll();
            }
        </script>
        <div id="shangyeDiv" class="jsq-box">
            <ul>
                <li id="liDiSanZheX" class="jsq-item-click">
                    <div id="chkDiSanZheX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <a href="###" data-action="popup-DiSanZheX">
                        <div class="jsq-item-txt">第三者责任险<span id="diSanZhePeiFu">赔付20万</span></div>
                        <div id="diSanZheX" class="jsq-item-num" data-action="popup-DiSanZheX">0</div>
                    </a>
                </li>
                <li id="liCheSunX">
                    <div id="chkCheSunShiX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <div class="jsq-item-txt">车辆损失险</div>
                    <div id="cheSunShiX" class="jsq-item-num">0</div>
                </li>
                <li id="liBuJiX">
                    <div id="chkBuJiX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <div class="jsq-item-txt">不计免赔特约险</div>
                    <div id="buJiX" class="jsq-item-num">0</div>
                </li>
                <li id="liQuanCheX">
                    <div id="chkQuanCheX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <div class="jsq-item-txt">全车盗抢险</div>
                    <div id="quanCheX" class="jsq-item-num">0</div>
                </li>
                <li id="liBoLiX" class="jsq-item-click">
                    <div id="chkBoLiX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <a href="###" data-action="popup-BoLiX">
                        <div class="jsq-item-txt">玻璃单独破碎险<span id="boLiPeiFu">国产玻璃</span></div>
                        <div id="boLiX" class="jsq-item-num">0</div>
                    </a>
                </li>
                <li id="liZiRanX">
                    <div id="chkZiRanX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <div class="jsq-item-txt">自燃损失险</div>
                    <div id="ziRanX" class="jsq-item-num">0</div>
                </li>
                <li id="liEngineX">
                    <div id="chkEngineX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <div class="jsq-item-txt">涉水险/发动机特别损失险</div>
                    <div id="engineX" class="jsq-item-num">0</div>
                </li>
                <li id="liCheShenX" class="jsq-item-click">
                    <div id="chkCheShenX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <a href="###" data-action="popup-CheShenX">
                        <div class="jsq-item-txt">车身划痕险<span id="cheShenPeiFu">赔付1万</span></div>
                        <div id="cheShenX" class="jsq-item-num">0</div>
                    </a>
                </li>
                <li id="liSiJiX" class="jsq-item-click">
                    <div id="chkSiJiX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <a href="###" data-action="popup-SiJiX">
                        <div class="jsq-item-txt">司机座位责任险<span id="sijiPeiFu">赔付2万</span></div>
                        <div id="siJiX" class="jsq-item-num">0</div>
                    </a>
                </li>
                <li id="liChengKeX" class="jsq-item-click">
                    <div id="chkChengKeX" class="jsq-item-check jsq-item-checked"><span></span></div>
                    <a href="###" data-action="popup-ChengKeX">
                        <div class="jsq-item-txt">乘客座位责任险<span id="chengkePeiFu">赔付3万</span></div>
                        <div id="chengKeX" class="jsq-item-num">0</div>
                    </a>
                </li>
            </ul>
        </div>

        <div class="summary-box summary-box-bx bor-top">
            <div class="t-box t-box2">
                <ul class="">
                    <li class="fout-box-w22">
                        <p class="tit">首付</p>
                        <p id="shoufuBottom" class="cont"></p>
                    </li>
                    <li class="fout-box-w28">
                        <p class="tit">月供 (<em id="yueShuBottom">0</em>个月)</p>
                        <p id="yueGongBottom" class="cont"></p>
                    </li>
                    <li class="fout-box-w20">
                        <p class="tit">利息</p>
                        <p id="liXiBottom" class="cont"></p>
                    </li>
                    <li class="fout-box-w30">
                        <p class="tit">总价</p>
                        <p id="totalPriceBottom" class="cont"></p>
                    </li>
                </ul>
            </div>
            <p class="b-box">此结果仅供参考，实际费用以当地缴费为准</p>
        </div>

        <script type="text/javascript">
            $(function () {
                $("#shangyeDiv div[id^=chk]").on("click", function () {
                    $(this).toggleClass("jsq-item-checked");

                    calcLoanValue();
                    calcAutoLoanAll();
                });
            });
        </script>

        <div id="m-loan-title" class="tt-first" style="display: none;">
            <h3>贷款推荐</h3>
            <div class="opt-more opt-more-gray">
                <a href='http://m.daikuan.com/www/<%=csAllSpell %>/?from=ycm45'>更多</a>
            </div>
        </div>
        <div id="m-loan-con" class="m-loan" style="display: none;">
            <ul></ul>
        </div>

        <div>
            <ins id="div_55dcf6a0-fbf4-41cd-9e6f-40d6a38f9689" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="55dcf6a0-fbf4-41cd-9e6f-40d6a38f9689"></ins>
        </div>

        <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
        <script type="text/javascript">
            //选择车款
            function selectCarId(carId) {
                location.href = "/qichedaikuanjisuanqi/?carid=" + carId;
            }

            var citycode = 201;
            if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
                citycode = bit_locationInfo.cityId;
            }

            //加载“贷款推荐”模块
            $.ajax({
                url: "http://carapi.chedai.bitauto.com/api/SummarizeFinancialProducts/SearchSummarizeFinancialProducts?cityId=" + citycode + "&serialId=" + <%=carSerialId %> + "&pageSize=4&from=ycm45",
                dataType: "jsonp",
                jsonpCallback: "creditinfo",
                cache: true,
                success: function (result) {
                    var data = $.parseJSON(result);
                    if (typeof data !== "undefined" && data.length > 0) {
                        var strHtml = [];
                        for (var i = 0; i < data.length; i++) {
                            strHtml.push("<li>");
                            strHtml.push("<a href=\"" + data[i].MDetailsUrl + "\">");
                            strHtml.push("<span class=\"img-box\"><img src=\"" + data[i].CompanyLogoUrl + "\"></span>");
                            strHtml.push("<dl class=\"loan-bank\">");
                            strHtml.push("<dt>" + data[i].CompanyName + "</dt>");
                            strHtml.push("<dd><span>成功率</span><div class=\"loan-block\"><em style=\"width: " + data[i].SuccessScore * 12 + "px\"></em></div></dd>");
                            strHtml.push("</dl>");
                            strHtml.push("<span class=\"loan-info\"><dl><dt>总成本</dt><dd>" + data[i].TotalCostText + "</dd></dl><dl><dt>月供</dt><dd><strong>" + data[i].MonthlyPaymentText + "</strong></dd></dl>");
                            strHtml.push("</span>");
                            strHtml.push("</a>");
                            strHtml.push("</li>");
                        }
                        $(".m-loan ul").html(strHtml.join(''));
                        $("#m-loan-title,#m-loan-con").show();
                    }
                }
            });
        </script>

        <!--footer start-->
        <div class="footer15">
            <!--搜索框-->
            <!--#include file="~/html/footersearch.shtml"-->
            <!--#include file="~/html/footerV3.shtml"-->
        </div>
        <div class="float-r-box">
            <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
        </div>
        <!--footer end-->
    </div>
    <div id="master_container" data-url="/html/selectCarTemplate.html?v=20151207" class="brandlayer">
    </div>

    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll-infinite.js?v=201508031842"> </script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/popup.js?v=201508031842"> </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CarCalculator.js?v=20173240946"></script>
    <!--车船弹出层-->
    <div id="backCheChuan" class="leftmask leftmaskCheChuan" style="display: none;"></div>
    <div id="cheChuanLayer" class="leftPopup popup-chechuan" data-back="leftmaskCheChuan" style="display: none; z-index: 10">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="cheChuanDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item1">1.0L(含)以下</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item2">1.0-1.6L(含)</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item3">1.6-2.0L(含)</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item4">2.0-2.5L(含)</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item5">2.5-3.0L(含)</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item6">3.0-4.0L(含)</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item7">4.0L以上</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-chechuan]').rightSwipe({
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#cheChuanDl a").on("click", function () {
                $("#cheChuanDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backCheChuan").trigger("close");
            });
        });
    </script>

    <!--座位数弹出层-->
    <div id="backZuoWei" class="leftmask leftmaskZuoWeiS" style="display: none;"></div>
    <div class="leftPopup popup-ZuoWeiS" data-back="leftmaskZuoWeiS" style="display: none; z-index: 20">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="zuoWeiSDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item950">家用6座以下</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item1100">家用6座及以上</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-ZuoWeiS]').rightSwipe({
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#zuoWeiSDl a").on("click", function () {
                $("#zuoWeiSDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backZuoWei").trigger("close");
            });
        });
    </script>

    <!--第三者责任险弹出层-->
    <div id="backDiSanZhe" class="leftmask leftmaskDiSanZheX" style="display: none;"></div>
    <div class="leftPopup popup-DiSanZheX" data-back="leftmaskDiSanZheX" style="display: none; z-index: 30">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="diSanZheXDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item50000">5万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item100000">10万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item200000">20万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item500000">50万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item1000000">100万</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-DiSanZheX]').rightSwipe({
                    isclick: function () {
                        if ($("#chkDiSanZheX").attr("class") == checkedClass) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#diSanZheXDl a").on("click", function () {
                $("#diSanZheXDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backDiSanZhe").trigger("close");
            });
        });
    </script>

    <!--玻璃单独破碎险弹出层-->
    <div id="backBoLi" class="leftmask leftmaskBoLiX" style="display: none;"></div>
    <div class="leftPopup popup-BoLiX" data-back="leftmaskBoLiX" style="display: none; z-index: 40;">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="boLiXDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="P1">国产</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item0">进口</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-BoLiX]').rightSwipe({
                    isclick: function () {
                        if ($("#chkBoLiX").attr("class") == checkedClass) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#boLiXDl a").on("click", function () {
                $("#boLiXDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backBoLi").trigger("close");
            });
        });
    </script>

    <!--车身划痕险弹出层-->
    <div id="backCheShen" class="leftmask leftmaskCheShenX" style="display: none;"></div>
    <div class="leftPopup popup-CheShenX" data-back="leftmaskCheShenX" style="display: none; z-index: 50;">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="cheShenXDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item2000">2千</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item5000">5千</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item10000">1万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item20000">2万</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-CheShenX]').rightSwipe({
                    isclick: function () {
                        if ($("#chkCheShenX").attr("class") == checkedClass) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#cheShenXDl a").on("click", function () {
                $("#cheShenXDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backCheShen").trigger("close");
            });
        });
    </script>

    <!--司机座位责任险弹出层-->
    <div id="backSiJi" class="leftmask leftmaskSiJiX" style="display: none;"></div>
    <div class="leftPopup popup-SiJiX" data-back="leftmaskSiJiX" style="display: none; z-index: 60">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="siJiXDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item10000">1万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item20000">2万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item30000">3万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item40000">4万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item50000">5万</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-SiJiX]').rightSwipe({
                    isclick: function () {
                        if ($("#chkSiJiX").attr("class") == checkedClass) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#siJiXDl a").on("click", function () {
                $("#siJiXDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backSiJi").trigger("close");
            });
        });
    </script>

    <!--乘客座位责任险弹出层-->
    <div id="backChengKe" class="leftmask leftmaskChengKeX" style="display: none;"></div>
    <div class="leftPopup popup-ChengKeX" data-back="leftmaskChengKeX" style="display: none; z-index: 70;">
        <div class="swipeLeft swipeLeft-sub">
            <dl id="chengKeXDl" class="tt-list">
                <dd class="current">
                    <a>
                        <p id="item10000">1万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item20000">2万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item30000">3万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item40000">4万</p>
                    </a>
                </dd>
                <dd>
                    <a>
                        <p id="item50000">5万</p>
                    </a>
                </dd>
            </dl>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //右侧侧附加选择层
            (function () {
                var iscroll = false;
                $('[data-action=popup-ChengKeX]').rightSwipe({
                    isclick: function () {
                        if ($("#chkChengKeX").attr("class") == checkedClass) {
                            return true;
                        } else {
                            return false;
                        }
                    },
                    clickEnd: function (b) {
                        var $leftPopup = this;
                        if (b) {
                            var $back = $('.' + $leftPopup.attr('data-back'));
                            $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                            var $swipeLeft = $leftPopup.find('.swipeLeft');

                            if (!iscroll) {
                                iscroll = true;
                                $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                                    probeType: 1,
                                    snap: 'dd',
                                    momentum: true,
                                    click: true
                                });
                            }
                        } else {
                            $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                        }
                    }
                });
            })();

            $("#chengKeXDl a").on("click", function () {
                $("#chengKeXDl dd").attr("class", "");
                $(this).parent().attr("class", "current");

                calcLoanValue();
                calcAutoLoanAll();
                $("#backChengKe").trigger("close");
            });
        });
    </script>

    <script type="text/javascript">
        //贷款购车总花费 首付款+贷款所花总钱数
        //贷款所花总钱数=月付款×还款年限×12
        //比全款购车多花费=贷款所花总钱数+首付金额-裸车价格。
        function calcLoanTotal() {
            var moneyMonthPayments = GetIntValue($("#yueShu").html()) * GetIntValue($("#yueGong").html());
            var totolCost = Math.round(GetIntValue($("#shoufu").html()) + moneyMonthPayments);
            $("#totalPrice").html(formatCurrency(totolCost) + "<span>元</span>");
            $("#totalPriceLayer").html(formatCurrency(totolCost));
            $("#totalPriceBottom").html(formatCurrency(totolCost));
        }


        function calcAutoLoanAll() {
            if ($('#hidCarPrice').val() != "0") {
                shangPai = 500;
                $("#shangPai").html("500");
            } else {
                shangPai = 0;
                $("#shangPai").html("0");
            }


            //购置税
            calcAcquisitionTax();
            //车船使用税
            CalculateVehicleAndVesselTax();
            //交强险
            calcCompulsory();
            //第三责任险
            calcTPL();
            //车辆损失险
            calcCarDamage();
            //不计免赔特约险
            calcAbatement();
            //全车盗抢险
            calcCarTheft();
            //玻璃单独破碎险
            calcBreakageOfGlass();
            //自燃损失险
            calcSelfignite();
            //涉水险/发动机特别损失险
            calcCarEngineDamage();
            //车身划痕险
            calcCarDamageDW();
            //司机座位责任险
            calcLimitofDriver();
            //乘客座位责任险
            calcLimitofPassenger();
            //必要花费小计
            calcEssentialCost();
            //商业保险小计
            calcCommonTotal();
            //首付款
            calcDownPayments();
            //月供
            calcMonthPayments();
            //计算总价
            calcLoanTotal();
            //格式化数字千位符
            //$("strong[id^='show_'],b[id^='show_']").html(function (index, html) { return formatCurrency(parseInt(html)) + "元"; });
        }

        var isHaveLoanRate = false;
        function InitTagStatus() {
            var carid = getQueryString("carid");
            var carPrice = getQueryString("carprice");
            if (carid) {
                if (carPrice) {
                    $("#hidCarPrice").val(carPrice);
                    $("#luochePrice1").html(formatCurrency(carPrice));
                    $("#luochePrice2").attr("value", carPrice);
                    setCalcToolUrl("caridAndPrice", $("#hidCarID").val());
                } else {
                    $("#luochePrice1").html(formatCurrency($("#hidCarPrice").val()));
                    $("#luochePrice2").attr("value", $("#hidCarPrice").val());
                    setCalcToolUrl("carid", $("#hidCarID").val());
                }
                GetCarInfo(<%=carId%>);
                $("#daikuanGo").attr("href", "http://m.daikuan.com/www/<%=csAllSpell%>/m<%=carId%>/?from=ycm33");
            } else {
                $("#hidCarPrice").val(carPrice);
                $("#luochePrice1").html(formatCurrency(carPrice));
                //$("#luochePrice2").val(formatCurrency(carPrice));
                $("#luochePrice2").attr("value", carPrice);
                $("#zuoWeiSDl dd").attr("class", "");
                $("#zuoWeiSDl dd").eq(0).attr("class", "current");
                $("#boLiXDl dd").attr("class", "");
                $("#boLiXDl dd").eq(0).attr("class", "current");
                $("#cheChuanDl dd").attr("class", "");
                $("#cheChuanDl dd").eq(1).attr("class", "current");
                $("#daikuanGo").attr("href", "http://m.daikuan.com/?from=ycm33");
                setCalcToolUrl("carPrice", carPrice);
            }

            //if (carPrice) {
            //$("#hidCarPrice").val(carPrice);
            //$("#luochePrice1").html(formatCurrency(carPrice));
            ////$("#luochePrice2").val(formatCurrency(carPrice));
            //$("#luochePrice2").attr("value", carPrice);
            //$("#zuoWeiSDl dd").attr("class", "");
            //$("#zuoWeiSDl dd").eq(0).attr("class", "current");
            //$("#boLiXDl dd").attr("class", "");
            //$("#boLiXDl dd").eq(0).attr("class", "current");
            //$("#cheChuanDl dd").attr("class", "");
            //$("#cheChuanDl dd").eq(1).attr("class", "current");
            //$("#daikuanGo").attr("href", "http://chedai.m.yiche.com/?from=ycm33");
            //setCalcToolUrl("carPrice", carPrice);

            //} else { //传入的是carid
            //$("#luochePrice1").html(formatCurrency($("#hidCarPrice").val()));
            //$("#luochePrice2").attr("value", $("#hidCarPrice").val());
            //GetCarInfo();
            //$("#daikuanGo").attr("href", "http://chedai.m.yiche.com/<%=csAllSpell%>/m<%=carId%>/chedai/?from=ycm33");
            //setCalcToolUrl("carid", $("#hidCarID").val());
            //}

            var shoufu = getQueryString("shoufu");
            if (shoufu) {
                $("#shoufuDiv a").attr("class", "");
                switch (shoufu) {
                    case "30%":
                        $("#shoufuDiv a").eq(0).attr("class", "current");
                        break;
                    case "40%":
                        $("#shoufuDiv a").eq(1).attr("class", "current");
                        break;
                    case "50%":
                        $("#shoufuDiv a").eq(2).attr("class", "current");
                        break;
                    case "60%":
                        $("#shoufuDiv a").eq(3).attr("class", "current");
                        break;
                }
            }
            else {
                $("#shoufuDiv a").eq(0).attr("class", "current");
            }
            var year = getQueryString("year");
            if (year) {
                $("#yearDiv a").attr("class", "");
                switch (year) {
                    case "5":
                        $("#yearDiv a").eq(0).attr("class", "current");
                        $("#yearRate").val("6.31%");
                        break;
                    case "4":
                        $("#yearDiv a").eq(1).attr("class", "current");
                        $("#yearRate").val("6.4%");
                        break;
                    case "3":
                        $("#yearDiv a").eq(2).attr("class", "current");
                        $("#yearRate").val("6.4%");
                        break;
                    case "2":
                        $("#yearDiv a").eq(3).attr("class", "current");
                        $("#yearRate").val("6.65%");
                        break;
                    case "1":
                        $("#yearDiv a").eq(4).attr("class", "current");
                        $("#yearRate").val("6.65%");
                        break;
                }
            }
            else {
                $("#yearDiv a").eq(3).attr("class", "current");
                $("#yearRate").val("6.4%");
            }
            var loanRate = getQueryString("loanRate");
            if (loanRate) {
                isHaveLoanRate = true;
                $("#loanRate").attr("value", loanRate);
            }
        }

        $("#luochePrice1").html(formatCurrency($("#hidCarPrice").val()));
        //$("#luochePrice2").val(formatCurrency($("#hidCarPrice").val()));
        $("#luochePrice2").attr("value", $("#hidCarPrice").val());
        GetCarInfo(<%=carId%>);
        $("#diSanZheXDl dd").attr("class", "");
        $("#diSanZheXDl dd").eq(2).attr("class", "current");

        $("#cheShenXDl dd").attr("class", "");
        $("#cheShenXDl dd").eq(1).attr("class", "current");

        $("#siJiXDl dd").attr("class", "");
        $("#siJiXDl dd").eq(1).attr("class", "current");

        $("#chengKeXDl dd").attr("class", "");
        $("#chengKeXDl dd").eq(1).attr("class", "current");
        InitTagStatus();//有参数传递时

        calcLoanValue();
        calcAutoLoanAll();
    </script>
    <%-- <script src="/js/v2/iscroll.js"></script>
    <script src="/js/v2/underscore.js"></script>
    <script src="/js/v2/model.js"></script>
    <script src="/js/v2/note.js"></script>
    <script src="/js/v2/rightswipe.js"></script>
    <script src="/js/v2/brand.js"></script>--%>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/iscroll.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/underscore.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/model.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/note.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/rightswipe.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/brand.js?v=2015112415"></script>
    <script type="text/javascript">
        var arrMarkSerial = [], saleYearCount = 0;  //非停销年款
        $(function () {
            api.brand.currentid = '<%=carBrandId%>';   //品牌
            api.car.currentid = '<%=carSerialId %>';  //车系
            api.model.currentid = $("#hidCarID").val();//车款
            var $body = $('body');
            //初始化品牌
            $('[data-action=brandlayer]', $body).rightSwipeAnimation({
                fnEnd: function () {
                    var curSerialId = $("[data-action=brandlayer]").data("value");
                    var $model = this;
                    $body.animate({ scrollTop: 0 }, 30);
                    var tags = $('.tag', $body);

                    //切换标签
                    $('.brandlist').tag({
                        tagName: '.first-tags',
                        fnEnd: function (idx) {
                            tags.hide();
                            if ((api.model.currentid == 0 || api.car.currentid == 0 || api.brand.currentid == 0)) {
                                idx = 1; $(".brandlist .first-tags ul li").eq(idx).addClass("current").siblings().remove();
                            }
                            tags.eq(idx).show();

                            if (idx == 0) {     //当前品牌
                                $('.curSerialSub', $body).swipeApi({
                                    url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&datatype=1&pid=" + curSerialId,
                                    templateid: '#curSerialTemplate',
                                    flatFn: function (data) {
                                        for (var n in data.CarList) {
                                            var SaleStateCount = 0;
                                            for (var i = 0; i < data.CarList[n].length; i++) {
                                                if (data.CarList[n][i].SaleState != '停销') {
                                                    SaleStateCount++;
                                                }
                                            }
                                            if (SaleStateCount > 0) {
                                                arrMarkSerial[n.replace('s', '')] = true;
                                                saleYearCount++;
                                            }
                                        }
                                        return data;
                                    },
                                    callback: function (html) {
                                        this.html(html);
                                        var curSerialName = $("#curSerialName").val();
                                        $('.curSerialName').html(curSerialName);

                                        //获取第二层的高度值
                                        var heights = [];
                                        function toResize() {
                                            heights.length = 0;
                                            $('.tag .y2015 ul').each(function (index, curr) {
                                                heights[index] = $(curr).height();
                                            })
                                        }

                                        $(window).resize(toResize).trigger('resize');
                                        $('[data-slider=pic-txt-hh]').sliderBox({
                                            heightFn: function (index) { return heights[index]; },
                                            isCloseFn: function (idx, index) {
                                                var isopen = !this.hasClass('open');
                                                if (isopen) {
                                                    this.prev().find('i').removeClass('up');
                                                }
                                                else {
                                                    this.prev().find('i').addClass('up');
                                                }
                                                return isopen;
                                            },
                                            onlyOne: settings.sliderBox.onlyOne,
                                            clickEnd: function (paras) {
                                                if (settings.sliderBox.onlyOne) {
                                                    this.parent().find('.tt-small i').removeClass('up');
                                                }
                                                if (paras.k == 'up') {
                                                    this.find('i').removeClass('up');
                                                } else {
                                                    this.find('i').addClass('up');
                                                }
                                                removeborder.call($model);
                                            }

                                        });
                                        removeborder.call($model);
                                        $(this).find(".pic-txt-h li a").on("click", function () {
                                            var curCarId = $(this).data("id");
                                            selectCarId(curCarId);
                                        });
                                        _commonSlider($model, $body);
                                    }
                                });
                            }
                            else if (idx == 1) {    //按品牌查找 
                                //初始化品牌
                                $body.trigger('brandinit');
                                //车款点击回调事件
                                api.model.clickEnd = function (paras) {
                                    api.brand.currentid = paras.masterid;
                                    api.car.currentid = paras.carid;
                                    api.model.currentid = paras.modelid;
                                    var curCarId = $(this).data("id");
                                    selectCarId(curCarId);
                                }
                            }

                            $model.find('.btn-return').click(function (ev) {
                                ev.preventDefault();
                                $(this).parents('.brandlayer').prev()[0].style.cssText = '';
                                $model.trigger('closeWindow');
                            })
                        }
                    });

                    $model.find('.btn-return').click(function () {
                        $model.trigger('close');
                    })
                }
            });
        });        //自适应页脚
        //层自适应        var _commonSlider = function ($model, $body) {
            if ($model.height() > $(document.body).height()) {
                $(document.body).height($model.height())
            } else if ($model.height() < $(document.body).height()) {
                $('.masterBox', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight - 40);
                $('.brandlist').height(document.documentElement.clientHeight - 40);
            }
        }        //折叠下划线控制
        function removeborder() {
            var $smalls = this.find('.tt-small');
            $smalls.each(function (index, curr) {
                var $current = $(curr);
                setTimeout(function () {
                    if (index < $smalls.length - 1) {
                        if ($current.next().height() == 0) {
                            $current.css('border-bottom', '1px solid #E9E9E9');
                        } else {
                            $current.css('border-bottom', '0px solid #E9E9E9');
                        }
                    }
                }, 300)
            })
        }
    </script>

</body>
</html>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
