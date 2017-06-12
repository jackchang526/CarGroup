<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WirelessWeb.SUVChannel._default" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <title>【SUV汽车大全】越野车哪个好_销量排行榜_评测_图片-易车网</title>
    <meta name="keywords" content="SUV,SUV大全,SUV报价,越野车,越野车哪款好,越野车排行榜,最便宜的越野车,最省油的越野车,越野车图片,越野车报价" />
    <meta name="description" content="SUV越野车:易车网SUV越野车频道提供最全面的SUV越野车排行榜,SUV越野车报价,图片,论坛等,SUV越野车哪款好?按网友关注度、品牌、车型等方式快速查询到您想要的车系。" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <!--#include file="/ushtml/0000/myiche2016_cube_pindaoye-1181.shtml"-->
</head>
<body>
    <!-- header start -->
    <div class="op-nav">
        <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
        <div class="tt-name">
            <a href="http://m.yiche.com/" class="yiche-logo">易车</a><h1>SUV频道</h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
    <div class="op-nav-mark" style="display: none;"></div>
    <!-- header end -->

    <!-- 头图 start -->
    <!--#include file="/include/pd/2016/wap/00001/201611_wap_suv_focus_Manual.shtml"-->
    <!-- 头图 end -->

    <!-- 小贴士 start -->
    <div class="car-tips-box b-shadow">
        <span>小贴士</span>
        <ul>
            <li data-channelid="27.149.1413"><a href="http://baa.bitauto.com/askbitauto/thread-9627201.html">现学现卖，如何更好的挑选一辆SUV！</a></li>
        </ul>
    </div>
    <!-- 小贴士 end -->


    <div class="second-tags mgt10" id="m-tabs-head">
        <div class="pd15">
            <ul>
                <li class="current" data-channelid="27.149.1412"><a href="javascript:void(0);"><span>热门品牌</span></a></li>
                <li data-channelid="27.149.1458"><a href="javascript:void(0);"><span>按价格</span></a></li>
                <li data-channelid="27.149.1460"><a href="javascript:void(0);"><span>爆款推荐</span></a></li>
            </ul>
        </div>
    </div>
    <div class="swiper-container-head">
        <div class="swiper-wrapper">
            <div class="swiper-slide">
                <div class="m-hot-brands">
                    <!--#include file="/html/SUVChannelHotBrand.shtml"-->
                    <div class="clear"></div>
                </div>
            </div>
            <div class="swiper-slide">
                <!--#include file="/include/pd/2016/wap/00001/2016_wap_suv_price_Manual.shtml"-->
            </div>
            <div class="swiper-slide">
                <div class="m-hot-car">
                    <ul id="baokuan-box">
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </div>
    </div>

    <div class="ad-txt-line" data-channelid="27.149.1395"><em>推广</em><a href="http://mai.m.yiche.com/">选suv折上折？去易车惠就购了！</a></div>
    <!-- 主题推荐 start -->
    <div class="tt-first">
        <h3>主题推荐</h3>
    </div>
    <% if (ver == "b")
       { %>
     <!--#include file="/include/pd/2016/wap/00001/201610_wap_suv_tjlb_Manual.shtml"-->
    <% }
       else
       { %>
    <div class="car-topic-box b-shadow">
        <ul>
            <li class="car-topic-1" data-channelid="27.149.1389">
                <a href="/suv/all/list/?id=1">
                    <span>5万元入手SUV</span>
                    <i></i>
                </a>
            </li>
            <li class="car-topic-2" data-channelid="27.149.1390">
                <a href="/suv/all/list/?id=2">
                    <span>10万元左右大空间</span>
                    <i></i>
                </a>
            </li>
            <li class="car-topic-3" data-channelid="27.149.1391">
                <a href="/suv/all/list/?id=3">
                    <span>低价高配中国造</span>
                    <i></i>
                </a>
            </li>
            <li class="car-topic-4" data-channelid="27.149.1392">
                <a href="/suv/all/list/?id=4">
                    <span>省油小能手SUV</span>
                    <i></i>
                </a>
            </li>
            <li class="car-topic-5" data-channelid="27.149.1393">
                <a href="/suv/all/list/?id=5">
                    <span>合资品牌高性价比</span>
                    <i></i>
                </a>
            </li>
            <li class="car-topic-6" data-channelid="27.149.1394">
                <a href="/suv/all/list/?id=6">
                    <span>越野烂路都搞定</span>
                    <i></i>
                </a>
            </li>
        </ul>
    </div>   
    <% } %>
    <!-- 主题推荐 end -->
    <div class="ad-line" data-channelid="27.149.1388">
        <a href="http://m.daikuan.com">
            <img src="http://image.bitautoimg.com/uimg/mbt2016/images/gg_750.jpg"></a>
    </div>

    <!-- 优惠购车 start -->
    <!--#include file="~/include/pd/2016/wap/00001/201608_wap_suv_tjc_Manual.shtml"-->
    <!-- 优惠购车 end -->

    <!-- 榜单评分 start -->
    <div class="tt-first">
        <h3>榜单评分</h3>
    </div>
    <div class="second-tags-scroll-box car-second-tags-scroll-box">
        <div class="second-tags-scroll">
            <ul id="bangdan-price">
                <li class="current"><a href="javascript:;" p="0-5"><span>5万以下</span></a></li>
                <li><a href="javascript:;" p="5-8"><span>5-8万</span></a></li>
                <li><a href="javascript:;" p="8-12"><span>8-12万</span></a></li>
                <li><a href="javascript:;" p="12-18"><span>12-18万</span></a></li>
                <li><a href="javascript:;" p="18-25"><span>18-25万</span></a></li>
                <li><a href="javascript:;" p="25-40"><span>25-40万</span></a></li>
                <li><a href="javascript:;" p="40-9999"><span>40万以上</span></a></li>
            </ul>
        </div>
        <div class="right-mask"></div>
    </div>
    <div class="car-sort-scroll">
        <ul id="bangdan-sort">
            <li><a href="javscript:;" s="6">综合评分</a></li>
            <li class="current"><a href="javscript:;" s="0">关注最高</a></li>
            <li><a href="javscript:;" s="24">最省油</a></li>
            <li><a href="javscript:;" s="2">价格最低</a></li>
            <li><a href="javscript:;" s="18">性价比</a></li>
        </ul>
    </div>
    <div class="buy-car b-shadow">
        <ul id="paihang-box">
        </ul>
    </div>
    <!-- 榜单评分 end -->

    <!-- footer start -->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <!-- footer end -->

    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/suvchannel/suvdefault.js?v=20160907" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/suvchannel/swiper-3.3.1.min.js?v=20160907" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            GetBaoKuan();
            var paihangPingfen = new PaihangPingfen();
            paihangPingfen.InitEvent();
            paihangPingfen.GetData();
        });
    </script>
    <script type="text/javascript">
        // 焦点图切换
        var mySwiperfocus = new Swiper('#m-focus-box', {
            loop: true,
            autoHeight: true,
            pagination: '.pagination-num',
            paginationType: 'fraction',
            paginationFractionRender: function (swiper, currentClassName, totalClassName) {
                return '<span class="' + currentClassName + '"></span>' + '/' + '<span class="' + totalClassName + '"></span>';
            }
        })
        // 头部切换标签
        var tabsSwiperHead = new Swiper('.swiper-container-head', {
            speed: 500,
            onSlideChangeStart: function () {
                $("#m-tabs-head .current").removeClass('current');
                $("#m-tabs-head li").eq(tabsSwiperHead.activeIndex).addClass('current');
            }
        })
        $("#m-tabs-head li").on('touchstart mousedown', function (e) {
            e.preventDefault();
            $("#m-tabs-head .current").removeClass('current');
            $(this).addClass('current');
            tabsSwiperHead.slideTo($(this).index());
        })
        $("#m-tabs-head li").click(function (e) {
            e.preventDefault();
        })

        // 优惠购车
        var mySwiperSalecar = new Swiper('#saleCar', {
            pagination: '.pagination-salecar',
            loop: true,
            paginationClickable: true
        });
    </script>

    <!--统计代码开始-->
    <!-- START OF SmartSource Data Collector TAG -->
    <script src="http://image.bitautoimg.com/bt/webtrends/dcs_tag_wapnew.js?20160602" type="text/javascript"></script>
    <!-- bitauto stat begin -->
    <script>
        var bit_stat_url = "url=" + escape(document.location);
        if (document.referrer != null) bit_stat_url += "&refer=" + escape(document.referrer)
        document.write("<img width=0 height=0 style='display:none;' src='http://log.bitauto.com/newsstat/log.aspx?" + bit_stat_url + "'/>");
    </script>
    <!-- bitauto stat end -->
    <!--airui-->
    <script src="http://img1.bitautoimg.com/stat/iwtToMSite20140218.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://g.yccdn.com"></script>
    <!--#include file="/include/special/stat/00001/stat_bglog_Manual.shtml"-->

    <script type="text/javascript">
        (function () {
            var dc = document.createElement('script'); dc.type = 'text/javascript'; dc.async = true;
            dc.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'js.ctags.cn/dc.js?10';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(dc, s);
        })();
    </script>
    <!--统计代码结束-->
    <%-- <script type="text/javascript">
        $(function () {
            PcPager.setCustomUrl("http://car.bitauto.com/suv/all/");
        })
    </script>--%>
</body>
</html>
