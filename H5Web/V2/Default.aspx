<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="H5Web.V2.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【汽车大全_热门车型大全】-易车</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <script type="text/javascript">
        var date = new Date();
        var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
        document.write(unescape('%3Cscript type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/detectcache.js?v=' + version + '"%3E%3C/script%3E'));
    </script>
    <script type="text/javascript">
        var summary = { serialId: 0 };
    </script>
    <!-- #include file="/ushtml/0000/4th_2015-2_brand_style-1009.shtml" -->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min.js"></script>
</head>
<body>
<!--小头开始-->
<div class="t-header">
    选车<a href="http://m.yiche.com/" class="head_go"></a>
</div>
<!--小头结束-->
<script type="text/javascript">
    var WTmc_idForHide = "";

    function getQueryStringByName(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2];
        return null;
    }

    var tempVarForHide = getQueryStringByName("WT.mc_id");
    if (tempVarForHide != null) {
        WTmc_idForHide = tempVarForHide;
    }
    var hideWTArra = ["mjzgh5"];
    if (WTmc_idForHide && WTmc_idForHide != "") {
        for (var hideLoop = 0; hideLoop < hideWTArra.length; hideLoop++)
            if (hideWTArra[hideLoop] == WTmc_idForHide.toLowerCase()) {
                $(".t-header").hide();
                break;
            }
    }
</script>
<div class="brandfilter">
    <!-- 字母导航 end -->
    <div class="alert" style="display: none;">
        <span>A</span>
    </div>
    <!-- 品牌列表 start -->
    <div class="brand-list bybrand_list">
        <div class="tt-small phone-title" data-key="#">
            <span>热门品牌</span>
        </div>
        <div class="brand-hot">
            <ul>
                <!--#include file="~/inc/HomepageHotBrand.htm"-->
            </ul>
        </div>
        <div class="content" data-key="brand">
        </div>
    </div>
    <!-- 品牌列表 end -->
</div>
<div class="fixed-nav">
</div>
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
    <div class="choose-car-name-close bybrand_list">
        <div class="brand-logo-none-border m_9_b"></div>
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
            { for(var j = 0 ; j < list[i].Child.length ; j++){ }
                        <li {= list[i].Child[j].SerialId.toString() == (api.car.currentid.toString()) ? 'class="current"':''}>
                            <a class="imgbox-2" data-action="models" data-id="{= list[i].Child[j].SerialId}" data-allspell="{= list[i].Child[j].Allspell}"  href="###">
                                <div class="img-box">
                                    <img src="{= list[i].Child[j].ImageUrl}" />
                                </div>
                                <div class="c-box">
                                    <h4>{= list[i].Child[j].SerialName }</h4>
                                    <p><strong>{= list[i].Child[j].Price }</strong></p>
                                </div>
                            </a>
                        </li>
            {}}
        </ul>
    </div>
    <!-- 图文混排横向 end -->
    {}}
</script>
<!--车型模板 end-->
<!--品牌列表模板 start-->
<script type="text/template" id="brandTemplate">
    { for(var n in MsterList){ }
        <div class="tt-small phone-title" data-key="{=n}">
            <span>{=n}</span>
        </div>
    <div class="box">
        <ul>
            {for(var i=0;i< MsterList[n].length;i++){}
                <li {=MsterList[n][i].MasterId.toString() == api.brand.currentid.toString() ? "class='current'":""}>
                    <a href="#" data-action="car" data-id="{=MsterList[n][i].MasterId}">
                        <span class="brand-logo m_{=MsterList[n][i].MasterId}_b"></span>
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

<script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
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
        title: '【汽车大全_热门车型大全】-易车网',
        keywords: '掌上车型手册，360°解读你所关注的车型。',
        desc: '掌上车型手册，360°解读你所关注的车型。',
        link: 'http://car.h5.yiche.com/?',
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/yiche100.jpg'
    };
</script>
<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/H5Stat.js?20150716"></script>

<!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
<script type="text/javascript">
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
</script>

<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/iscroll20151119.js?20151117"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/underscore.js?20151117"></script>
<script src="http://image.bitautoimg.com/uimg/mbt2015/js/model20151112.js?20151119" type="text/javascript"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/note20151119.js?20151117"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/rightswipe20151119.js?20151117"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/brand20151119.js?20151117"></script>
<script type="text/javascript">
    $(function () {
        /*接口默认配置 datatype=0 是在销 ，1 是包含停销*/
        api = {
            'brand': {
                url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=master', callName: 'businessBrandCallBack', templteName: '#brandTemplate',
                currentid: ''
            },
            'car': {
                url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=serial&pid={0}', callName: 'businessCarCallBack', templteName: '#carTemplate',
                currentid: '',
                clickEnd: null
            }
            ,
            'model': {
                url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid={0}&datatype=1', callName: 'businessModelCallBack', templteName: '#modelTemplate',
                currentid: '',
                clickEnd: null
            }
        };

        var $body = $('body');
        //初始化品牌
        $body.trigger('brandinit', { model_hide: true });
        $(function() {
            //api.brand.currentid = '9';
            //api.car.currentid = '2593';
            api.car.clickEnd = function(paras) {
                var $leftPopup = this;
                //品牌ID
                //console.log('品牌ID：' + paras.masterid);
                //车型ID
                //console.log('车型ID ' + paras.carid);
                api.brand.currentid = paras.masterid;
                api.car.currentid = paras.carid;
                var par = "";
                if (tempVarForHide) {
                    par = "?WT.mc_id=" + tempVarForHide;
                }
                var ad = getQueryStringByName("ad");
                if (par == "") {
                    if (ad) {
                        par = ad != null ? "?ad=" + ad : "";
                    }
                } else {
                    par += ad != null ? "&ad=" + ad : "";
                }
                var allspell = $('[data-id=' + paras.carid + ']', $leftPopup).attr('data-allspell');
                window.location.href = "/" + allspell + "/" + par;
                var $back = $('.' + $leftPopup.attr('data-back'));
                //关闭浮层
                $back.trigger('close');
            };
        });
    })
</script>
</body>
</html>