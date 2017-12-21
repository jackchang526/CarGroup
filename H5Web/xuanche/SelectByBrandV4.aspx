<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectByBrandV4.aspx.cs" Inherits="H5Web.xuanche.SelectByBrandV4" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <title>【品牌选车|汽车品牌大全】-手机易车网</title>
    <meta name="Keywords" content="车标党,品牌大全,选车工具,手机易车网"/>
    <meta name="Description" content="汽车标志大全，汽车品牌大全，尽在手机易车网"/>

    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <!--#include file="/ushtml/0000/4th_2015-2_brand_style-1009.shtml"-->
    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>
    <%--<script type="text/javascript">
        var baokuanche = [];
        var yiyuanshijia = [];
        $.ajax({
            type: "GET",
            url: "http://hd.api.market.bitauto.com/interface/GetInterfaceHandler.ashx?name=11laiyiche&key=cbb11ed87dc8a95d81400c7f33c7c171",
            timeout: 2000, //超时时间设置，单位毫秒
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "baokuaiche",
            success: function (data) {
                if (typeof data != "undefined") {
                    $.each(data, function (index, item) {
                        if (item["stype"] == "1") {
                            baokuanche.push(item);
                        }
                        if (item["stype"] == "2") {
                            yiyuanshijia.push(item);
                        }
                    });
                }
            }
        });
    </script>--%>
    <script type="text/javascript">
        var ad = util.GetQueryStringByName("ad");
        var ly = util.GetQueryStringByName("ly");
        var par = util.GetKeyValueString(["ad", "order", "lg", "ly", "tele", "WT.mc_id"]); //lg是否需要logo
        var forshare = util.GetKeyValueString(["WT.mc_id"]);
        $(function () {
            $(".xuanche_class a").each(function (index, item) {
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
</head>
<body class="zhixuanche chebiao">

<!-- 搜索 start -->
<!--#include file="/inc/search.v2.shtml"-->
<!-- 搜索 end -->

<header>
    <ul class="xuanche_class">
        <li class="current">
            <a href="/chebiaodang/">品牌选车</a>
        </li>
        <li>
            <a data-channelid="85.86.461" href="/ganjuekong/" onclick="MtaH5.clickStat('aa1')">需求选车</a>
        </li>
        <li>
            <a data-channelid="85.86.462"href="/fashaoyou/" onclick="MtaH5.clickStat('aa2')">配置选车</a>
        </li>
    </ul>
</header>
<div class="brandfilter">
    <!-- 字母导航 end -->
    <div class="alert" style="display: none;">
        <span>A</span>
    </div>
    <!-- 品牌列表 start -->
    <div class="brand-list bybrand_list">
        <%--<div class="tt-small phone-title" data-key="#">
            <span>推荐车型</span>
        </div>
        <div class="carlist-3c">

            <ins style="display: none" id="div_82cde721-4780-4526-9b56-b3a5d7837199" type="ad_play" adplay_IP="" adplay_AreaName="" adplay_CityName="" adplay_BrandID="" adplay_BrandName="" adplay_BrandType="" adplay_BlockCode="82cde721-4780-4526-9b56-b3a5d7837199"> </ins>

            <ins style="display: none" id="div_eb08a641-3b9a-421f-ba15-7c447dc71db7" type="ad_play" adplay_IP="" adplay_AreaName="" adplay_CityName="" adplay_BrandID="" adplay_BrandName="" adplay_BrandType="" adplay_BlockCode="eb08a641-3b9a-421f-ba15-7c447dc71db7"> </ins>

            <script type="text/javascript">
                var insIdIndex = localStorage.InsIdIndex;
                var insIdList = ['div_eb08a641-3b9a-421f-ba15-7c447dc71db7', 'div_82cde721-4780-4526-9b56-b3a5d7837199'];
                if (typeof insIdIndex !== "undefined" && insIdIndex != null && !isNaN(insIdIndex) && parseInt(insIdIndex) < insIdList.length) {
                    $.each(insIdList, function (index, item) {
                        if (parseInt(insIdIndex) === index) {
                            $("#" + item).show();
                            if (index < insIdList.length - 1) {
                                localStorage.InsIdIndex = insIdIndex + 1;
                            } else {
                                localStorage.InsIdIndex = 0;
                            }
                        }
                    });
                } else {
                    var maxIndex = insIdList.length - 1, minIndex = 0;
                    var Range = maxIndex - minIndex;
                    var Rand = Math.random();
                    var randomNum = minIndex + Math.round(Rand * Range);
                    $("#" + insIdList[randomNum]).show();
                    if (randomNum < insIdList.length - 1) {
                        localStorage.InsIdIndex = randomNum + 1;
                    } else {
                        localStorage.InsIdIndex = 0;
                    }
                }
            </script>
        </div>
        <script type="text/javascript">
            if (ad != null && ad === "0") {
                $(".tt-small").remove();
                $(".carlist-3c").remove();
                $("#div_217711bf-cad0-4360-ac95-b640b4592dff").remove();
            }
        </script>--%>
        <!-- 热门品牌 start-->
        <div class="tt-small phone-title" data-key="#">
            <span>热门品牌</span>
        </div>
        <div class="brand-hot">
            <ul>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="8">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_8_55.png"/>
                        <p data-key="name">大众</p>
                        <input type="hidden" class="brand-logo m_8_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="13">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_13_55.png"/>
                        <p data-key="name">现代</p>
                        <input type="hidden" class="brand-logo m_13_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="17">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_17_55.png"/>
                        <p data-key="name">福特</p>
                        <input type="hidden" class="brand-logo m_17_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="28">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_28_55.png"/>
                        <p data-key="name">起亚</p>
                        <input type="hidden" class="brand-logo m_28_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="127">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_127_55.png"/>
                        <p data-key="name">别克</p>
                        <input type="hidden" class="brand-logo m_127_b"/>
                    </a>
                </li>
            </ul>
            <ul>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="21">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_21_55.png"/>
                        <p data-key="name">长城</p>
                        <input type="hidden" class="brand-logo m_21_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="196">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_196_55.png"/>
                        <p data-key="name">哈弗</p>
                        <input type="hidden" class="brand-logo m_196_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="136">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_136_55.png"/>
                        <p data-key="name">长安</p>
                        <input type="hidden" class="brand-logo m_136_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="15">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_15_55.png"/>
                        <p data-key="name">比亚迪</p>
                        <input type="hidden" class="brand-logo m_15_b"/>
                    </a>
                </li>
                <li>
                    <a href="javascript:void(0);" data-action="car" data-id="49">
                        <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_49_55.png"/>
                        <p data-key="name">雪佛兰</p>
                        <input type="hidden" class="brand-logo m_49_b"/>
                    </a>
                </li>
            </ul>
        </div>
        <!-- 热门品牌 end-->

        <div class="content" data-key="brand">
        </div>
    </div>
    <!-- 品牌列表 end -->
</div>
<%--<div class="float-r-box">
    <div class="float-r-ball discovery">
        <div class="c-box">
            <ul>
                <li><a data-channelid="84.153.1553" href="/zhuanti/discovery/news/">新闻</a></li>
                <li><a data-channelid="84.153.1554" href="/zhuanti/discovery/video/">视频</a></li>
                <li><a data-channelid="84.153.1555" href="/zhuanti/discovery/activity/">活动</a></li>
            </ul>
        </div>
        <a href="###" class="cir">发现</a>
    </div>
</div>--%>
<div class="fixed-nav">
</div>
<!--公共底-->
<!--#include file="/inc/footer.html"-->
<!--/公共底-->
<%--<ins id="div_217711bf-cad0-4360-ac95-b640b4592dff" type="ad_play" adplay_IP="" adplay_AreaName="" adplay_CityName="" adplay_BrandID="" adplay_BrandName="" adplay_BrandType="" adplay_BlockCode="217711bf-cad0-4360-ac95-b640b4592dff"> </ins>--%>
<script type="text/javascript">
    if (ad != null && ad === "0") {
        $("#div_217711bf-cad0-4360-ac95-b640b4592dff").remove();
    }
</script>
<!--车型层 start-->
<div class="leftmask mark leftmask3" style="display: none;"></div>
<div class="leftPopup car-model car" data-back="leftmask3" style="display: none;" data-key="car">
    <div class="swipeLeft swipeLeft-sub">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif"/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<!--车型层 end-->

<!--loading模板 start -->
<div class="template-loading" style="display: none;">
    <div class="loading">
        <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif"/>
        <p>正在加载...</p>
    </div>
</div>
<!--loading模板 end -->
<!--车型模板 start-->
<script type="text/template" id="carTemplate">
    <div class="tt-list absolute">
        <div class="choose-car-name-close bybrand_list">
            <div class="brand-logo-none-border"><img /></div>
            <span class="brand-name"></span>
            <!-- <a href="#" class="choose-car-btn-close">关闭</a> -->
        </div>
        <div class="clear"></div>
        { for(var i = 0 ; i < list.length ; i++){ }
                        <!-- 车款列表 start -->
        {if(list[i].Child.length > 0){ }
        <div class="tt-small">
            <span>{= list[i].BrandName }</span>
        </div>
        {}}
        <!-- 图文混排横向 start -->
        <div class="pic-txt-h pic-txt-9060">
            <ul>
                { for(var j = 0 ; j < list[i].Child.length ; j++){ 
                    var url = (typeof ly != "undefined" && ly != null && ly === "xxj"?("http://dealer.h5.yiche.com/searchOrder/" + list[i].Child[j].SerialId.toString() + "/0/?leads_source=H001005&" + par):("/"+list[i].Child[j].Allspell+"/?h5from=brand&" + par));}
                            <li {= list[i].Child[j].SerialId.toString() == (api.car.currentid.toString()) ? 'class="current"':''}>
                                <div class="imgbox-2">                            
                                    <a class="l-content" data-action="models" data-id="{= list[i].Child[j].SerialId}" data-allspell="{= list[i].Child[j].Allspell}"  href="{= url}">
                                        <div class="img-box">
                                            <img src="{= list[i].Child[j].ImageUrl}" />
                                        </div>
                                        <div class="c-box">
                                            <h4>{= list[i].Child[j].SerialName }</h4>
                                            <p><strong>{= list[i].Child[j].Price }</strong></p>
                                        </div>
                                    </a>
                                    {if(list[i].Child[j].ad){}
                                       <a class="r-content {= list[i].Child[j].ad.className}" href="{=list[i].Child[j].ad.href}"></a>
                                    {}}
                                </div>
                            </li>
                {}}
            </ul>
        </div>
        <!-- 图文混排横向 end -->
        {}}
    </div>
</script>
<!--车型模板 end-->
<!--品牌列表模板 start-->
<script type="text/template" id="brandTemplate">
     { var number = 0; }
    { for(var n in MsterList){ }
        <div class="tt-small phone-title" data-key="{=n}">
            <span>{=n}</span>
        </div>
    <div class="box">
        <ul>
            {for(var i=0;i< MsterList[n].length;i++){}
                <li {=MsterList[n][i].MasterId.toString() == api.brand.currentid.toString() ? "class='current'":""}>
                    <a href="javascript:void(0)" data-action="car" data-id="{=MsterList[n][i].MasterId}">
                        <span class="brand-logo">
                            <img {=number++ < 15 ? 'src': 'data-original'}="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{=MsterList[n][i].MasterId}_100.png"/>
                        </span>
                        <span class="brand-name">{=MsterList[n][i].MasterName}</span>
                    </a>
                </li>
            {}}
        </ul>
    </div>
    {}}
</script>
<!--品牌列表模板 end-->
<!--字母列表模板 start-->
<script type="text/template" id="navTemplate">
    <ul class="rows-box">
        <li><a href="#">#</a></li>
        { for(var n in CharList){ }
            { if(CharList[n] != 0){ }
        <li><a href="#">{=n}</a></li>
        {}}
            {}}
    </ul>
</script>
<!--字母列表模板 end-->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
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
        title: '我走过最崎岖的路，就是买车的套路。来一车，买车无套路！',
        keywords: '车标党,品牌大全,选车工具,手机易车网',
        desc: '买车无忧、用车不愁，易车-来一车，你喜欢的我都有。',
        link: 'http://car.h5.yiche.com/chebiaodang/?' + forshare,
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

<%--    <script type="text/javascript" src="/scripts/carcompare/model.v1.js"></script>
    <script type="text/javascript" src="/scripts/carcompare/rightswipe.v1.js"></script>
    <script type="text/javascript" src="/scripts/carcompare/brand.v1.js"></script>--%>

    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjsv2/jquery.lazyload.min.js"></script>
    
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/iscroll20160224.js?20151117"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/underscore.js?20151117"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/model.v1.js"></script>

<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/note20160224.js?20151117"></script>

<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/rightswipe.v1.js?20160606"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/brand.v1.js?20160606"></script>

<script type="text/javascript">
    $(function () {
        /*接口默认配置 datatype=0 是在销 ，1 是包含停销*/
        api = {
            imgRoot: 'http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_id_100.png',
            'brand': {
                url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=master',
                callName: 'businessBrandCallBack',
                templteName: '#brandTemplate',
                currentid: '',
                clickEnd: function () {  }
            },
            'car': {
                url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=serial&pid={0}&datatype=0',
                callName: 'businessCarCallBack',
                templteName: '#carTemplate',
                currentid: '',
                clickEnd: null
            }
            //,
            //'model': {
            //    url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid={0}&datatype=1',
            //    callName: 'businessModelCallBack',
            //    templteName: '#modelTemplate',
            //    currentid: '',
            //    clickEnd: null
            //}
        };

        var $body = $('body');
        //初始化品牌
        $body.trigger('brandinit', {
            model_hide: true,
            onBeforeScrollStart: function (ev) { },
            flatFn1: function (data, paras) {
                $.each(data, function (index, item) {
                    $.each(item["Child"], function(i,childItem) {
                        //$.each(baokuanche, function (b, bkc) {
                        //    if (bkc["csid"] == childItem['SerialId']) {
                        //        childItem.ad = { className: 'bkc', href: bkc["wapurl"] };
                        //    }
                        //});
                        //$.each(yiyuanshijia, function (y, yysj) {
                        //    if (yysj["csid"] == childItem['SerialId']) {
                        //        childItem.ad = { className: 'yysj', href: yysj["wapurl"] };
                        //    }
                        //});
                    });
                });
                return { list: data }
            },
            fnEnd1: function () {

                //var $swipeLeft = this;
                //var list = $($swipeLeft).find("li a");
                //$.each(list, function (index, item) {
                //    var serialId = $(item).attr("data-id");

                //    $.each(baokuanche, function (b, bkc) {
                //        if (bkc["csid"] == serialId) {
                //            $(item).parent().addClass("bkc66");
                //            $(item).attr("href", bkc["wapurl"]);
                //        }
                //    });
                //    $.each(yiyuanshijia, function (y, yysj) {
                //        if (yysj["csid"] == serialId) {
                //            $(item).parent().addClass("yysj");
                //            $(item).attr("href", yysj["wapurl"]);
                //        }
                //    });

                //    //落地页地址切换逻辑
                //    if (typeof ly != "undefined" && ly != null && ly === "xxj") {
                //        $(item).attr("href", "http://dealer.h5.yiche.com/searchOrder/" + serialId + "/0/?leads_source=H001005&" + par);
                //    }
                //});
            }
        });

        //api.brand.currentid = '9';
        //api.car.currentid = '2593';
        api.car.clickEnd = function (paras) {
            var $leftPopup = this;
            //品牌ID
            //console.log('品牌ID：' + paras.masterid);
            //车型ID
            //console.log('车型ID ' + paras.carid);
            api.brand.currentid = paras.masterid;
            api.car.currentid = paras.carid;

            //var allspell = $('[data-id=' + paras.carid + ']', $leftPopup).attr('data-allspell');
            //var newurl = "/" + allspell + "/?h5from=brand" + par;
            //document.location.href = newurl;
            var $back = $('.' + $leftPopup.attr('data-back'));
            //关闭浮层
            $back.trigger('close');
        };



        /*关闭广告浮层*/
        $body.find('.ad-flex-footer .close').click(function (ev) {
            ev.preventDefault();
            $(this).parent().hide();
        });

        //右侧发现
        //var $discovery = $body.find('.discovery'),
        //    $cir = $discovery.find('.cir');
        //$cir.click(function(ev) {
        //    ev.preventDefault();
        //    var $this = $(this);
        //    if ($this.html() == '发现') {
        //        $this.parent().addClass('open');
        //        $this.html('关闭');
        //    } else {
        //        $this.parent().removeClass('open');
        //        $this.html('发现');
        //    }
        //});

    });

</script>
<!--按钮统计-->
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
</body>
</html>