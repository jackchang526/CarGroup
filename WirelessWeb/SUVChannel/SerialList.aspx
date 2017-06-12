<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialList.aspx.cs" Inherits="WirelessWeb.SUVChannel.SerialList" %>

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

    <div class="car-reco-list">
        <ul id="tag-box">
        </ul>
    </div>
    <div class="second-tags-scroll-box mgt10">
        <div class="pd15">
            <div class="second-tags-scroll">
                <ul id="sort-box">
                    <li class="current"><a href="javascript:;" sort="1"><span>最关注</span></a></li>
                    <li><a href="javascript:;" sort="2"><span>最便宜</span></a></li>
                    <li><a href="javascript:;" sort="3"><span>最贵</span></a></li>
                </ul>
            </div>
        </div>
        <div class="right-mask"></div>
    </div>

    <div class="buy-car">
        <ul id="serial-box">
        </ul>
    </div>
    <!-- footer start -->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <!-- footer end -->

    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/suvchannel/seriallist.js?v=20160907" type="text/javascript"></script>
    <script type="text/javascript">
        var channelId = "<%= channelId %>";
        $(function () {
            var suvList = new GetSUVList(channelId);
            suvList.GetData();
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
     <script type="text/javascript">
         $(function () {
             PcPager.setCustomUrl("http://car.bitauto.com/suv/all/");
         })
    </script>
</body>
</html>
