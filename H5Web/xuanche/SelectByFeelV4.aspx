<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectByFeelV4.aspx.cs" Inherits="H5Web.xuanche.SelectByFeelV4" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->

    <title>【需求选车|购车预算】-手机易车网</title>
    <meta name="Keywords" content="感觉控,购车预算,购车计算器,手机易车网"/>
    <meta name="Description" content="易车网购车预算小工具,为您提供准确的购车费用信息,帮助您方便快捷的购车!"/>
    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201604011615"></script>
    <script type="text/javascript">
        var par = util.GetKeyValueString(["ad", "order", "lg", "ly", "tele", "WT.mc_id"]); //lg是否需要logo
    </script>
</head>
<body class="zhixuanche ganjue-box">
<!-- 搜索 start -->
<!--#include file="/inc/search.v2.shtml"-->
<!-- 搜索 end -->
<header>
    <ul class="xuanche_class">
        <li>
            <a data-channelid="85.87.463" href="/chebiaodang/" onclick="MtaH5.clickStat('ab1')">品牌选车</a>
        </li>
        <li class="current">
            <a href="/ganjuekong/">需求选车</a>
        </li>
        <li>
            <a data-channelid="85.87.464" href="/fashaoyou/" onclick="MtaH5.clickStat('ab2')">配置选车</a>
        </li>
    </ul>
</header>

<div class="fashao">

    <h2>
        <span>请选择您的购车预算</span>
    </h2>

    <!--条 开始-->
    <div class="ruler">
        <ul class="line" data-width="1">
            <li></li>
            <li>
                <span>10</span></li>
            <li>
                <span>20</span></li>
            <li>
                <span>30</span></li>
            <li>
                <span>40</span></li>
            <li>
                <span>50</span></li>
            <li>
                <span>60</span></li>
            <li>
                <span>70</span></li>
            <li>
                <span>80</span></li>
            <li>
                <span>90</span></li>
        </ul>
        <div class="sliderbar" data-min="0" data-max="100">
            <div class="line-float max-line current" data-index="100" data-bar="2">
                <div id="max-dot" class="dot max-dot">
                    <span>100+</span></div>
            </div>
            <div class="line-float min-line current" data-index="0" data-bar="1">
                <div id="min-dot" class="dot min-dot">
                    <span>0</span></div>
            </div>
            <div class="dot touch-dot" style="display: none;">0</div>
        </div>
    </div>
    <!--条 结束-->
    <div class="keyword">
        <div class="box-center box-center1">
            <div>
                <span data-channelid="85.87.467" class="kw1" f="16,2">混动电动<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.470" class="kw2" w="1002">颜控<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.474" class="kw3" w="1006">苦逼上班族<em></em></span>
            </div>
        </div>

        <div class="box-center box-center2">
            <div>
                <span data-channelid="85.87.465" class="kw3" w="1000">超强推背感<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.1378" class="kw1" l="7,8">户外旅行<em></em></span>
            </div>
        </div>

        <div class="box-center box-center3">
            <div>
                <span data-channelid="85.87.478" class="kw3" more="174">安全至上<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.1379" class="kw1" l="4,6,7,15,16">商务座驾<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.473" class="kw2" w="1007">储物空间大<em></em></span>
            </div>
        </div>

        <div class="box-center box-center4">
            <div>
                <span data-channelid="85.87.475" class="kw1" more="266">我家人口多<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.1380" class="kw3" g="4">只买进口车<em></em></span>
            </div>

        </div>

        <div class="box-center box-center5">
            <div>
                <span data-channelid="85.87.476" class="kw3" g="1">国货当自强<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.477" class="kw2" w="1005">肌肉车<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.469" class="kw1" w="1004">操控性能好<em></em></span>
            </div>
        </div>

        <div class="box-center box-center6">
            <div>
                <span data-channelid="85.87.468" class="kw2" w="1003">大即是好<em></em></span>
            </div>
            <div>
                <span data-channelid="85.87.1381" class="kw3" g="2">合资最实惠<em></em></span>
            </div>
        </div>
    </div>
</div>
<div class="button_fixed">
    <div class="button_del_box">
        <div class="button_w">
            <button class="button_del">删除</button>
            <!--<button class="button_del button_del_none">删除</button>-->
        </div>
        <div class="button_f">
            <button class="button_del_other">有&nbsp;&nbsp;&nbsp;&nbsp;款车型符合要求</button>
            <!--<button class="button_del_other button_none">有0款车型符合要求</button>-->
        </div>
    </div>
</div>

<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/scrollbarX.js?v=2015121410"></script>

<script src="http://image.bitautoimg.com/uimg/mbt2015/js/model20160224.js" type="text/javascript"></script>

<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/selectbyfeel.v1.js?v=201608101645"></script>
<%--<script src="/Scripts/xuanche/selectbyfeel.v1.js"></script>--%>

<script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>

<script type="text/javascript">
    var forshare = util.GetKeyValueString(["WT.mc_id"]);
    $(function() {
        $(".xuanche_class a").each(function(index, item) {
            var href = $(item).attr("href");
            var hrefLength = href.length;
            if (href.indexOf("?") >= 0) {
                if (href.indexOf("?") === (hrefLength - 1)) {
                    $(item).attr("href", href + par);
                } else {
                    $(item).attr("href", href + "&" + par);
                }
            } else {
                if (par != null && par.length > 0) {
                    $(item).attr("href", href + "?" + par);
                }
            }
        });
    });
</script>
<script type="text/javascript">
    $(function() {

        SelectByFeel.init();

        $(".box-center div").bind("click", function() {
            var wval = "";
            var moreval = "";
            var gval = "";
            var fval = "";
            var lval = "";

            if (!$(this).hasClass("current")) {
                $(this).addClass("current");
                wval = $($(this).children().get(0)).attr("w");
                if (typeof wval != "undefined" && wval != "") {
                    SelectByFeel.AddEleToArr(wArr, wval);
                }
                moreval = $($(this).children().get(0)).attr("more");
                if (typeof moreval != "undefined" && moreval != "") {
                    SelectByFeel.AddEleToArr(moreArr, moreval);
                }

                gval = $($(this).children().get(0)).attr("g");
                if (typeof gval != "undefined" && gval != "") {
                    SelectByFeel.AddEleToArr(gArr, gval);
                }

                fval = $($(this).children().get(0)).attr("f");
                if (typeof fval != "undefined" && fval != "") {
                    var fres = fval.split(',');
                    for (var f = 0; f < fres.length; f++) {
                        SelectByFeel.AddEleToArr(fArr, fres[f]);
                    }
                }

                lval = $($(this).children().get(0)).attr("l");
                if (typeof lval != "undefined" && lval != "") {
                    var lres = lval.split(',');
                    for (var l = 0; l < lres.length; l++) {
                        SelectByFeel.AddEleToArr(lArr, lres[l]);
                    }
                }
            } else {
                $(this).removeClass("current");
                wval = $($(this).children().get(0)).attr("w");
                if (typeof wval != "undefined" && wval != "") {
                    SelectByFeel.RemoveEleFromArr(wArr, wval);
                }
                moreval = $($(this).children().get(0)).attr("more");
                if (typeof moreval != "undefined" && moreval != "") {
                    SelectByFeel.RemoveEleFromArr(moreArr, moreval);
                }

                gval = $($(this).children().get(0)).attr("g");
                if (typeof gval != "undefined" && gval != "") {
                    SelectByFeel.RemoveEleFromArr(gArr, gval);
                }

                fval = $($(this).children().get(0)).attr("f");
                if (typeof fval != "undefined" && fval != "") {
                    var res = fval.split(',');
                    for (var i = 0; i < res.length; i++) {
                        SelectByFeel.RemoveEleFromArr(fArr, res[i]);
                    }
                }

                lval = $($(this).children().get(0)).attr("l");
                if (typeof lval != "undefined" && lval != "") {
                    var res2 = lval.split(',');
                    if (res2.length == 2) {
                        if (lArr.length == 2) {
                            lArr = [];
                        } else {
                            SelectByFeel.RemoveEleFromArr(lArr, "8");
                        }
                    }
                    if (res2.length == 5) {
                        if (lArr.length == 5) {
                            lArr=[];
                        } else {
                            for (var g = 0; g < res2.length; g++) {
                                if (res2[g] == "7") {
                                    continue;
                                }
                                SelectByFeel.RemoveEleFromArr(lArr, res2[g]);
                            }
                        }
                    }


                    
                }
            }
            if (wArr.length > 0 || moreArr.length > 0 || gArr.length > 0 || fArr.length > 0 || lArr.length > 0 || minprice !== 0 || maxprice !== 9999) {
                $(".button_del").attr("disabled", false);
                $(".button_del").removeClass("button_del_none");
            } else {
                $(".button_del").attr("disabled", true);
                $(".button_del").addClass("button_del_none");
            }
            SelectByFeel.getCarByFeel();
        });

        //删除按钮事件绑定
        $('.button_del').click(function (ev) {
            
            $(".box-center div").removeClass("current");
            $(this).addClass("button_del_none");
            $(this).blur();
            wArr = [];
            moreArr = [];
            gArr = [];
            fArr = [];
            lArr = [];
            minprice = 0;
            maxprice = 9999;

            var $ruler = $(".ruler");
            $ruler.trigger('setvalue', { min: 0, max: 100 });

            //初始化数据
            SelectByFeel.getCarByFeel();
        });

        //按钮跳转事件（去往列表页面）
        $(".button_del_other").bind("click", function() {
            if (par.length > 0) {
                window.location.href = "/carlist/?" + SelectByFeel.getCondition(true) + "&" + par;
            } else {
                window.location.href = "/carlist/?" + SelectByFeel.getCondition(true);
            }
        });
    });
</script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/selectcar_barprice.v1.js?v=2015121410"></script>
<script type="text/javascript">
    var XCWebLogCollector = { area: '201' };
    if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
        XCWebLogCollector.area = bit_locationInfo.cityId;
    }
    if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
        XCWebLogCollector.serial = summary.serialId;
    }
</script>
<script type="text/javascript">
    var __zp_tag_params = {
        modelId: summary.serialId,
        carId: 0
    };
</script>
<script type="text/javascript">
    var forweixinObj = {
        debug: false,
        appId: 'wx0c56521d4263f190',
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
    };
    var pageShareContent = {
        title: '越野党？上班族？家人多？个性选车，就用来一车！',
        keywords: '感觉控,购车预算,购车计算器,手机易车网',
        desc: '选车、买车、用车，易车-来一车，你喜欢的车子我都有。',
        link: 'http://car.h5.yiche.com/ganjuekong/?' + forshare,
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/laiyiche.png'
    };
</script>
<!--#include file="~/inc/The3rdStat.html"-->
<!--#include file="~/inc/WeiXinJs.shtml"-->
<!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
<%--<script type="text/javascript">
    (function(param) {
        var c = { query: [], args: param || {} };
        c.query.push(["_setAccount", "12"]);
        c.query.push(["_setSiteID", "1"]);
        (window.__zpSMConfig = window.__zpSMConfig || []).push(c);
        var zp = document.createElement("script");
        zp.type = "text/javascript";
        zp.async = true;
        zp.src = ("https:" == document.location.protocol ? "https:" : "http:") + "//cdn.zampda.net/s.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(zp, s);
    })(window.__zp_tag_params);
</script>--%>
<!--按钮统计-->
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
</body>
</html>