<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPage.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageBrandV2.CarBrandPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=brandName%>】<%=brandName%>汽车报价_图片_<%=DateTime.Now.Year+brandName%>新款车型-易车网</title>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="Keywords" content="<%=brandName%>,<%=brandName%>汽车,<%=brandName%>汽车报价,<%=DateTime.Now.Year %><%=brandName %>新款车型,易车网,car.bitauto.com" />
	<meta name="Description" content="<%=brandName%>汽车:提供最新<%=brandName%>汽车报价,<%=brandName%>汽车图片,<%=brandName%>汽车新闻,视频,口碑,问答,二手车等。<%=brandName%>在线询价、低价买车尽在易车网。" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=brandSpell %>/" />
	<%--<meta name="mobile-agent" content="format= xhtml; url=http://m.bitauto.com/g/carbrand.aspx?brandid=<%=brandId %>" />--%>
    <!--#include file="~/ushtml/0000/yiche_2016_cube_pinpai_style-1248.shtml"-->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--#include file="~/htmlV2/header2016.shtml"-->
    <header class="header-main header-type1">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <h1 class="brand-logo">
                    <a href="/<%=brandSpell %>/">
                        <%if (brandId == 10038)
                            { %>
                        <img class="logo" width="55" height="55" src="<%=logo55 %>" alt="<%=brandName%>汽车">
                    <%}
    else
    { %>
                    <img class="logo" src="<%=logo55 %>" alt="<%=brandName%>汽车">
                    <%} %>
                    </a>
                    <a href="/<%=brandSpell %>/"><%=brandName%></a>
                </h1>
            </div>
            <div class="col-xs-8 right-box">
                <ul class="list keyword-list">
                    <li class="dt">热门搜索：</li>
                    <li><a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a></li>
                    <li><a href="http://v.bitauto.com/ycjm.shtml" target="_blank">原创节目</a></li>
                    <li><a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></li>
                </ul>
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul class="list" id="ulForTempClickStat">
                            <li id="treeNav_chexing" class="active"><a target="_self" href="http://car.bitauto.com/<%=brandSpell%>/">首页</a></li>
                            <li id="treeNav_wenzhang"><a target="_self" href="http://car.bitauto.com/<%=brandSpell%>/wenzhang/">文章</a></li>
                            <li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/brand/<%=brandId%>/">图片</a></li>
                            <li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/brand/<%=brandId%>_0_0.html">视频</a></li>
                            <li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/b<%=brandId%>/">报价</a></li>
                            <li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/b<%=brandId%>/">降价</a></li>
                            <li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=brandSpell%>/">经销商</a></li>
                            <li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?brand=<%=brandId%>/">置换</a></li>
                            <li id="treeNav_yanghu"><a target="_blank" href="http://yanghu.bitauto.com/?source=1">养护</a></li>
                            <li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=brandSpell %>/">二手车</a></li>
                            <li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/xuanche/?bid=<%=brandId%>">口碑</a></li>
                            <li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=masterId%>/">问答</a></li> 
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    </header>

    <div class="container carbrand-page">
        <div class="row section-layout">
            <div class="col-all clearfix">
                <div class="col-xs-9">
                    <div class="section-main">
                        <div class="row story-section margin-bottom-xlg">
                            <!--品牌简介-->
                            <%=brandIntroduce %>
                        </div>
                        <!--品牌最新资讯-->
                        <%if (!string.IsNullOrEmpty(brandTopNews))
                          { %>
                            <div class="row news-section margin-bottom-xlg">
                                <%=brandTopNews%>
                            </div>
                        <%}
                        %>
                       <%if (!string.IsNullOrEmpty(serialListByBrand))
                          { %>
                            <div class="row cartype-section margin-bottom-xlg">
                                <%=serialListByBrand%>
                            </div>
                        <%}
                        %>
                       <%if (!string.IsNullOrEmpty(videoHtml))
                          { %>
                                <%=videoHtml%>
                        <%}
                        %>
                        <!--经销商-->
                        <div class="row recommend-agent-section margin-bottom-sm">
                            <div class="section-header header2 mb0 h-default">
                                <div class="box">
                                    <h2><a href="http://dealer.bitauto.com/<%=brandSpell %>/" target="_blank"><%=brandName.Replace("·", "&bull;")%>-经销商推荐</a></h2>
                                </div>
                            </div>
                            <div class="row dealer-list list-box">
                                <script type="text/javascript">
                                    document.writeln("<ins Id=\"ep_union_140\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\"" + bit_locationInfo.cityId + "\" city_name=\"0\" car_type=\"1\" brandId=\"<%=brandId %>\" serialId=\"0\" carId=\"0\"></ins>");
                            </script>
                            </div>
                        </div>
                        <%--问答块--%>
                        <!-- # include file="~/include/pd/2012/wenda/00001/201405_ask_common_gb2312_Manual.shtml"-->
                        <%-- <%=askEntriesHtml %>--%>
                        <!-- SEO底部热门 -->
                        <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                        <!-- SEO底部热门 -->
                    </div>
                </div>
                <div class="col-xs-3 section-right">
                    <!-- 热门品牌 -->
                    <div class="row hotbrand-sidebar margin-bottom-sm">
                        <div class="section-header header3">
                            <div class="box">
                                <h2><a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a></h2>
                            </div>
                        </div>
                        <div class="list-box col3-55-box clearfix">
                            <%=hotMasterBrandHtml %>
                        </div>
                    </div>
                    <!--二手车-->
                    <div class="row oldcar-sidebar margin-bottom-sm line_box" id="ucar_box">
                    </div>
                    <!--百度推广-->
                    <div class="line-box baidupush">
                        <script type="text/javascript">
                            /*bitauto 200*200，导入创建于2011-10-17*/
                            var cpro_id = 'u646188';
                        </script>
                        <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
                    </div>
                    <!--百度推广-End -->
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="cheyisou" value="http://www.cheyisou.com/qiche/" />
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnewV2/taochev2.min.js?v=2016111614" type="text/javascript"></script>
   <%--      <script src="/jsnewV2/taochev2.js" type="text/javascript"></script>--%>
    <script>
        TaoChe.showBrandUCar('<%=brandId %>', bit_locationInfo.cityId, '<%=brandSpell %>', '<%=brandName %>', 5);
    </script>
    <!--本站统计代码-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript">
        OldPVStatistic.ID1 = "<%=brandId %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 3;
        mainOldPVStatisticFunction();
    </script>
    <script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
    <script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>
    <!-- 调用尾 -->
    <script type="text/javascript">
        var CarCommonBSID = "<%= brandEntity.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = '<%= brandId.ToString() %>';
    </script>
	<!--#include file="~/htmlV2/footer2016.shtml"-->
	<!-- 弹出 -->
	<script>
	    function BrandZhanKai() {
	        $("#shortBrandStory").hide();
	        $("#detailBrandStory").show();
	    }

	    function BrandShouQi() {
	        $("#shortBrandStory").show();
	        $("#detailBrandStory").hide();
	    }

	    function CheBiaoZhanKai() {
	        $("#shortLogoStory").hide();
	        $("#detailLogoStory").show();
	    }

	    function CheBiaoShouQi() {
	        $("#shortLogoStory").show();
	        $("#detailLogoStory").hide();
	    }
	</script>
	<%if (brandId == 20109)
   { %>
	<!--百度热力图-->
	<script type="text/javascript">
	    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
	    document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<%} %>

	<!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
</body>
</html>
<script type="text/javascript">
    //获取视频播放数
    var $videos = $("div[data-videoid]");
    var videoArr = [];
    $.each($videos, function (index, item) {
        var curVideoId = $(item).attr("data-videoid");
        videoArr.push(curVideoId);
    })
    var videoStr = videoArr.join(",");
    $.ajax({
        url: "http://api.admin.bitauto.com/videoforum/Promotion/GetVideoByVideoIds?vIds=" + videoStr,
        dataType: "jsonp",
        jsonpCallback: "getvedionumcallback",
        success: function (data) {
            if (data && data.length > 0) {
                $.each(data, function (index, item) {
                    var curVideo = item["VideoId"];
                    var formatPlayCount = item["FormatPlayCount"];
                    var replyCount = item["ReplyCount"];
                    var $curVideoElem = $("div[data-videoid='" + curVideo + "']");
                    if ($curVideoElem) {
                        $curVideoElem.find(".p-list .num").find(".play").html(formatPlayCount).end().find(".comment").html(replyCount);
                    }
                });
            }
        }
    });
    </script>
<script type="text/javascript">
	// JavaScript Document
    function addLoadEvent(func) {
        var oldonload = window.onload;
        if (typeof window.onload != 'function') {
            window.onload = func;
        } else {
            window.onload = function () {
                oldonload();
                func();
            }
        }
    }

    function addClass(element, value) {
        if (!element.className) {
            element.className = value;
        } else {
            newClassName = element.className;
            newClassName += " ";
            newClassName += value;
            element.className = newClassName;
        }
    }

    function removeClass(element, value) {
        var removedClass = element.className;
        var pattern = new RegExp("(^| )" + value + "( |$)");
        removedClass = removedClass.replace(pattern, "$1");
        removedClass = removedClass.replace(/ $/, "");
        element.className = removedClass;
        return true;
    }
    function all_func() {
        tabs("car_tab_BrandNews_ul", "data_table_BrandNews", "best_car", true);
    }
    addLoadEvent(all_func)

    /*=======================tab=============================*/
    function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
    function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

    function tabsRemove(index, head, divs, div2s) {
        if (!document.getElementById(head)) return false;
        var tab_heads = document.getElementById(head);
        if (tab_heads) {
            var alis = tab_heads.getElementsByTagName("li");
            for (var i = 0; i < alis.length; i++) {
                removeClass(alis[i], "current");
                hide(divs + "_" + i);
                if (div2s) { hide(div2s + "_" + i) };
                if (i == index) {
                    addClass(alis[i], "current");
                }
            }
            show(divs + "_" + index);
            if (div2s) { show(div2s + "_" + index) };
        }
    }

    function tabs(head, divs, div2s, over) {
        if (!document.getElementById(head)) return false;
        var tab_heads = document.getElementById(head);
        if (tab_heads) {
            var alis = tab_heads.getElementsByTagName("li");
            for (var i = 0; i < alis.length; i++) {
                alis[i].num = i;
                if (over) {
                    alis[i].onmouseover = function () {
                        var thisobj = this;
                        thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
                    }
                    alis[i].onmouseout = function () {
                        clearTimeout(thetabstime);
                    }
                }
                else {
                    alis[i].onclick = function () {
                        if (this.className == "current" || this.className == "last current") {
                            changetab(this);
                            return true;
                        }
                        else {
                            changetab(this);
                            return false;
                        }
                    }
                }
                function changetab(thebox) {
                    tabsRemove(thebox.num, head, divs, div2s);
                }
            }
        }
    }
    //无图片时的处理
    function nofind(imgSize) {
        var img = event.srcElement;
        if (imgSize == 1) {
            img.src = "http://www.cnblogs.com/sys/common/image/fileoperation/icon/default1.gif";
        }
        else if (imgSize == 2) {
            img.src = "http://www.cnblogs.com/sys/common/image/fileoperation/icon/default2.gif";
        }
        else {
        }
        img.onerror = null;// 控制不要一直跳动
    }
</script>
