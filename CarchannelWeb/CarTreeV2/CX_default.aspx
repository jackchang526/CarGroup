<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_default.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTreeV2.CX_default" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head id="Head1">
    <title>【汽车大全|汽车标志_汽车标志大全_热门车型大全】-易车网</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="Keywords" content="汽车大全,汽车标志,车型大全,汽车标志列表" />
    <meta name="Description" content="汽车大全:易车网车型大全频道为您提供各种汽车品牌型号信息,包括汽车报价,汽车标志,汽车图片,汽车经销商,汽车油耗,汽车资讯,汽车点评,汽车问答,汽车论坛等等……" />
    <meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/" />
    <meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/car.aspx" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <%--<!--#include file="~/ushtml/0000/yiche_2014_cube_car-685.shtml" -->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->--%>   
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <!--#include file="~/ushtml/0000/yiche_2016_cube_chexingshuxing_style-1259.shtml"-->
    <script type="text/javascript">
        var _SearchUrl = '<%=this._SearchUrl %>';
    </script>
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--顶通-->
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <div style="width: 1200px; margin: 0 auto; overflow: hidden;">
        <ins id="div_22bf5713-566c-42e7-bd96-37d91bc6e48a" type="ad_play" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
            adplay_blockcode="22bf5713-566c-42e7-bd96-37d91bc6e48a"></ins>
    </div>
    <!--#include file="~/include/pd/2016/common/00001/201607_ejdh_common_Manual.shtml"-->
    <div class="container cartype-section">
        <div class="col-xs-3">
            <div class="treeNav" id="treeNav">
            </div>
        </div>
        <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
        <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/lefttree.v2.min.js"></script>
        <%--<script type="text/javascript" src="/jsnewv2/lefttree.v2.js"></script>--%>
        <script type="text/javascript">
            BitautoLeftTreeV2.init({
                likeDefLink: "http://car.bitauto.com/{allspell}/",
                params: {
                    tagtype: "chexing",
                    pagetype: "masterbrand",
                    objid: 0
                }
            });
        </script>
        <div class="col-xs-9">
            <!--右侧主体-->
            <div class="cartype-section-main">
                <ins style="display: none; margin: 0 0; float: left; width: 100%:" id="div_93824a04-0084-4532-bcd2-6ee1c21a21ba"
                    type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid=""
                    adplay_brandname="" adplay_brandtype="" adplay_blockcode="93824a04-0084-4532-bcd2-6ee1c21a21ba"></ins>
                <!--筛选-->
                <div class="main-inner-section condition-selectcar clearfix">
                    <div class="section-header header1">
                        <div class="box">
                            <h2>按条件选车</h2>
                        </div>
                        <div class="more"><%--<a href="#">我上次的选车条件</a>--%><a href="http://www.bitauto.com/zhuanti/daogou/gsqgl/" target="_blank">购车流程</a></div>
                    </div>
                    <div class="treeMainv1">
                        <!--#include file="~/htmlV2/selectCarTool.shtml"-->
                    </div>
                </div>
                <!--/筛选-->
                <!--热门品牌-->
                <div class="main-inner-section hot-brand">
                    <div class="section-header header1">
                        <div class="box">
                            <h2><a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a></h2>
                        </div>
                    </div>
                    <div class="brand_list">
                        <%=hotMasterBrandHtml %>
                    </div>
                </div>
                <!-- AD -->
                <ins id="div_3b69e344-7183-4cd4-8fc8-c3887f967cf4" type="ad_play" adplay_ip="" adplay_areaname=""
                    adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                    adplay_blockcode="3b69e344-7183-4cd4-8fc8-c3887f967cf4" style="margin: 0px 0px 10px; float: left; width: 100%;"></ins>
                <!-- AD -->
                <!--热门车型-->
                <div class="main-inner-section hot-cartype js-tab-container">
                    <div class="section-header header1 h-default">
                        <div class="box">
                            <ul class="nav">
                                <li class="js-tab-menu current"><a href="javascript:;">热门车型</a></li>
                                <li class="js-tab-menu"><a href="javascript:;">新车</a></li>
                                <li class="js-tab-menu"><a href="javascript:;">二手车</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="row block-4col-180 js-tab-content" id="data_box3_0">
                        <%=GetHotCarTypeNew()%>
                    </div>
                    <div class="row block-4col-180 js-tab-content hide" id="data_box3_1">
                        <%=GetNewCarTypeNew()%>
                    </div>
                    <div class="row block-4col-180 js-tab-content hide" id="data_box3_2">
                    </div>
                </div>
                <!--广告-->
                <div style="margin-bottom:30px;margin-top:-20px;">
                <ins id="div_fc509494-b0cc-461b-b8ef-8ae018603eb7" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="fc509494-b0cc-461b-b8ef-8ae018603eb7"></ins>
                    </div>
                <!--关注排行榜-->
                <div class="main-inner-section focus-ranking js-tab-container">
                    <div class="section-header header2 h-default">
                        <div class="box">
                            <h2>车型关注排行榜</h2>
                            <ul class="nav">
                                <li class="js-tab-menu current"><a href="javascript:;">按价位</a></li>
                                <li class="js-tab-menu"><a href="javascript:;">按级别</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="cont-list js-tab-content">
                        <%= GetPriceRangeList() %>
                    </div>
                    <div class="cont-list js-tab-content hide">
                        <%= GetLevelList() %>
                    </div>
                </div>

                <!--易车问答-->
                <%--<div class="main-inner-section ask-answer">
                    <%=GetAskHtml()%>
                </div>--%>
                <ins id="div_a5ace7c5-f758-45f9-917e-9aa6c4c43735"
                    type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid=""
                    adplay_brandname="" adplay_brandtype="" adplay_blockcode="a5ace7c5-f758-45f9-917e-9aa6c4c43735"></ins>
                <!-- add by sk 2013.02.28 -->
                <ins id="div_884b0e00-9cd4-486f-bb23-3557e1b2c079" type="ad_play" adplay_ip="" adplay_areaname=""
                    adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                    adplay_blockcode="884b0e00-9cd4-486f-bb23-3557e1b2c079"></ins>
                <!-- add by chengl Jun.27.2014 -->
                <div style="margin-bottom:30px;margin-top:-20px;">
                <ins id="div_ba1e16ec-8931-4e55-b4e3-d75b465e0077" type="ad_play" adplay_ip="" adplay_areaname=""
                    adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                    adplay_blockcode="ba1e16ec-8931-4e55-b4e3-d75b465e0077"></ins>
                    </div>
                <!-- 友情链接 -->
                <!--#include file="~/htmlV2/htmlFriendLink.shtml"-->

            </div>
            <!--#include file="~/htmlv2/treefooter2016.shtml"-->
        </div>
    </div>
    <script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js?v=20170307"></script>    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/newselectcartoolv4.min.js?d=201604211649"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/common.min.js"></script>

    <script type="text/javascript">
        var loadJS = {
            lock: false,
            ranks: [],
            callback: function (startTime, callback) {
                this.lock = false;
                callback;
                this.read();
            },
            read: function () {
                if (!this.lock && this.ranks.length) {
                    var head = document.getElementsByTagName("head")[0] || document.documentElement;
                    if (!head) {
                        ranks.length = 0, ranks = [];
                        throw new Error('HEAD不存在');
                    }
                    var ranks = this.ranks.shift(), startTime = new Date;
                    var wc = this;
                    var script = document.createElement('script');
                    this.lock = true;
                    script.onload = script.onreadystatechange = function () {
                        if (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") {
                            wc.callback(startTime, ranks.callback);
                            startTime = ranks = null;
                            script.onload = script.onreadystatechange = null;
                            if (head && script.parentNode) {
                                head.removeChild(script);
                            }
                        }
                    };
                    script.charset = ranks.charset || 'gb2312';
                    script.src = ranks.src;
                    head.appendChild(script);
                }
            },
            push: function (src, charset, callback) {
                this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
                this.read();
            }
        };
        var global_hash = window.location.hash;
        if (global_hash == "#jrtt") {
            $("#div_22bf5713-566c-42e7-bd96-37d91bc6e48a,.bit_top990").remove();
            $("#leftTreeBox").css({ top: "0px" });
            $(".header_style").css({ "overflow": "hidden", "height": "0", "padding": "0" });
            $("ins").remove();
        }
        conditionObj.Type = "car";
        conditionObj.InitPageConditionV2();
        loadJS.push("http://yicheapi.taoche.cn/CarSourceInterface/ForJson/HotSerialsForBitAuto.ashx?callback=showTaoChe", 'utf-8', showTaoChe);
        function showTaoChe(data) {
            if (!(data && data.HotSerialList && data.HotSerialList.length > 0)) return;
            var arrTaoHtml = [], dataList = data.HotSerialList;
            for (var i = 0; i < 8 && i < dataList.length; i++) {
                //arrTaoHtml.push("<li><a id=\"newCsID_2486\" href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\"><img src=\"" + dataList[i].CarPicUrl + "\" alt=\"" + dataList[i].CsName + "\"></a><div class=\"title\"><a href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\">" + dataList[i].CsName + "</a></div><div class=\"txt\">" + dataList[i].DisplayPrice + "</div></li>");
                arrTaoHtml.push("<div class=\"col-xs-3\">");
                arrTaoHtml.push("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                arrTaoHtml.push("<div class=\"img\">");
                arrTaoHtml.push("<a href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\"><img src=\"" + dataList[i].CarPicUrl.replace("/1f/", "/1d/") + "\" alt=\"" + dataList[i].CsName + "\"></a>");
                arrTaoHtml.push("</div>");
                arrTaoHtml.push("<ul class=\"p-list\">");
                arrTaoHtml.push("<li class=\"name\"><a href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\">" + dataList[i].CsName + "</a></li>");
                arrTaoHtml.push("<li class=\"price\"><a href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\">" + dataList[i].DisplayPrice + "</a></li>");
                arrTaoHtml.push("</ul>");
                arrTaoHtml.push("</div>");
                arrTaoHtml.push("</div>");
            }
            document.getElementById("data_box3_2").innerHTML = arrTaoHtml.join('');
        }
        //(function () {
        //    var cookiesName = "car_superlink", cObj = CookieHelper.readCookie(cookiesName), toolAlert = document.getElementById("tool-alert");
        //    ((cObj != null && cObj == "1") ? toolAlert.style.display = "none" : toolAlert.style.display = "block");
        //    SelectCar.Tools.addEvent(document.getElementById("tool-close"), "click", function () {
        //        CookieHelper.setCookie(cookiesName, "1", { "expires": 3600, "path": "/", "domain": "car.bitauto.com" }); toolAlert.style.display = "none";
        //    }, false);
        //})();
    </script>
    <script>
        jQuery(function () {
            /*公共tab功能*/
            YICHE_COMMON_FUNC.yicheTabFunc({
                removeClassName: "current",
                //切换classname 高亮当前选项
                controlType: "mouseover" //默认为点击事件
            });
        });
    </script>
    <script type="text/javascript" src="http://d2.yiche.com/js/senseNew.js"></script>
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
</body>
</html>
<!-- 右下广告 -->
<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = ''; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    var adplay_BlockCode = 'fb4948ab-c2d4-493a-ac7a-d87c6591b605'; //广告块编号
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>
<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = ''; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    var adplay_BlockCode = '6774e482-4280-420a-81fd-cec7edda6637'; //广告块编号
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>
<!-- add by chengl Jun.27.2012 -->
<%--<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = ''; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    if (global_hash != "#jrtt") {
        var adplay_BlockCode = '56dda5da-43a3-406d-9291-974566759ddd'; //广告块编号
    }
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>--%>
<!-- add by chengl May.7.2013 -->
<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = ''; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    var adplay_BlockCode = '8eba16c6-95bf-477c-8967-a611040a5762'; //广告块编号
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>
<!--车型栏目页/底部浮层-->
<script type="text/javascript">
    var adplay_CityName = '';//城市
    var adplay_AreaName = '';//区域
    var adplay_BrandID = '';//品牌id 
    var adplay_BrandType = '';//品牌类型：（国产）或（进口）
    var adplay_BrandName = '';//品牌
    var adplay_BlockCode = 'aa88205a-8a73-4bf9-8b9b-9232299bc59e';//广告块编号
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>

<%--<!--add by hepw Jan.26.2016 for select car condition's click count-->
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script> --%>