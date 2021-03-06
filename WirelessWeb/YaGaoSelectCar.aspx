﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YaGaoSelectCar.aspx.cs" Inherits="WirelessWeb.YaGaoSelectCar" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>易星人选车</title>
    <meta name="Description" content="根据科学测试，易星人发现用什么牙膏就开什么车！" />
     <!-- 线上 -->
    <!-- #include file="/ushtml/0000/2016-xiaobaixuanche-life_style-1149.shtml" -->
</head>
<body>
    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/fengmian.png" class="fengmian" />
    <!--loading加载浮层 start-->
    <div class="loading">
        <img data-loading="false" class="circle" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-yiche.png" />
        <div class="bar">
            <img data-loading="false" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-car.png" class="car" />
            <span style="width: 0px;"></span>
        </div>
        <div class="status">
            <i></i>
            <i></i>
        </div>
    </div>
    <!--loading加载浮层 end-->
    <!--第一片段 start-->
    <div class="page page1">
        <div class="title">
            <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page1-fk.png" data-animate="fadeInLeft animated" class="fk" />
            <div class="txt">
                <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page1-title.png" data-animate="fadeInRight animated" class="txt" />
                <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page1-jk.png" class="jk" />
            </div>
            <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page1-wen.png" data-animate="fadeInRight animated" class="wen" />
        </div>
        <a href="javascript:;" data-animate="tada animated show" data-animate-delay="1" class="btn-start"></a>
    </div>
    <!--第一片段 end-->
    <!--第二片段 start-->
    <div class="page page2">
        <a href="javascript:;" class="go"></a>
        <img class="title" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page2-title.png" />
        <ul class="btn-list">
            <li>
                <a href="javascript:;" class="btn" data-animate="bounceInOne animated" data-id="1">
                    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-glj.png" />
                </a>
            </li>
            <li>
                <a href="javascript:;" class="btn btn-hr" data-animate="bounceInOne animated delay-d2" data-id="2">
                    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-hr.png" />
                </a>
            </li>
            <li>
                <a href="javascript:;" class="btn" data-animate="bounceInOne animated delay-d3" data-id="3">
                    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-sw.png" />
                </a>
            </li>
            <li>
                <a href="javascript:;" class="btn" data-animate="bounceInOne animated delay-d4" data-id="4">
                    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-mrs.png" />
                </a>
            </li>
            <li>
                <a href="javascript:;" class="btn btn-bsy" data-animate="bounceInOne animated delay-d5" data-id="5">
                    <img src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/icon-bsy.png" />
                </a>
            </li>
        </ul>
    </div>
    <!--第二片段 end-->
    <!--第三片段 start-->
    <div class="page page3">
        <a href="javascript:;" class="go"></a>
        <img class="title" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page3-title.png" />
        <div class="btns-box">
            <a href="javascript:;" class="btn lnk1" data-animate-delay="0.2" data-animate="bounceInOne animated duration7" data-id="1"></a>
            <a href="javascript:;" class="btn lnk2" data-animate-delay="1.2" data-animate="bounceInOne animated duration7" data-id="6"></a>
            <a href="javascript:;" class="btn lnk3" data-animate-delay="1" data-animate="bounceInOne animated duration7" data-id="5"></a>
            <a href="javascript:;" class="btn-circle" data-animate="bounceInOne animated duration7" data-id="7"></a>
            <a href="javascript:;" class="btn rnk1" data-animate-delay="0.4" data-animate="bounceInOne animated duration7" data-id="2"></a>
            <a href="javascript:;" class="btn rnk2" data-animate-delay="0.6" data-animate="bounceInOne animated duration7" data-id="3"></a>
            <a href="javascript:;" class="btn rnk3" data-animate-delay="0.8" data-animate="bounceInOne animated duration7" data-id="4"></a>
        </div>
    </div>
    <!--第三片段 end-->
    <!--第四片段 start-->
    <div class="page page4">
        <a href="javascript:;" class="go"></a>
        <img class="title" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page4-title.png" />
        <div class="list">
            <ul>
                <li>
                    <a class="icon-nsj" data-animate-delay="0.1" href="javascript:;" data-animate="show bounceInOne animated duration7" data-id="2"></a>
                </li>
                <li>
                    <a class="icon-th" data-animate-delay="0.2" href="javascript:;" data-animate="show bounceInOne animated duration7" data-id="5"></a>
                </li>
                <li>
                    <a class="icon-dsw" data-animate-delay="0.3" href="javascript:;" data-animate="show bounceInOne animated duration7" data-id="3"></a>
                </li>
                <li>
                    <a class="icon-zccn" data-animate-delay="0.4" href="javascript:;" data-animate="show bounceInOne animated duration7" data-id="1"></a>
                </li>
                <li>
                    <a class="icon-xfq" data-animate-delay="0.5" href="javascript:;" data-animate="show bounceInOne animated duration7" data-id="4"></a>
                </li>
                <li>
                    <a class="icon-wss" data-animate-delay="0.6" href="javascript:;" data-animate="show bounceInOne animated" data-id="6"></a>
                </li>
            </ul>
        </div>
    </div>
    <!--第四片段 end-->
    <!--第五片段 start-->
    <div class="page page5">
        <a href="javascript:;" class="go"></a>
        <img class="title" src="http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/page5-title.png" />
        <div class="warp30">
            <div class="boardlist">
            </div>
        </div>
        <div class="btns">
            <ul>
                <li>
                    <a href="http://car.m.yiche.com/" class="btn-left" data-channelid="27.114.1066"></a>
                </li>
                <li>
                    <a href="javascript:;" class="btn-right" data-action="popup-share" data-channelid="27.114.1067"></a>
                </li>
            </ul>
        </div>
    </div>
    <!--第五片段 end-->

    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <%--<script type="text/javascript" src="/js/yagao.selectcar.js"></script>
    <script type="text/javascript" src="/js/yagao.commons.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/yagao.selectcar.min.js,carchannel/wirelessjs/yagao.commons.min.js?v=20170105"></script>
    <!--分享 start -->
    <script type="text/javascript">
        var share_config = {
            url: window.location.href,// 分享的网页链接
            title: "逗逼易星人用牙膏帮你选了款座驾，没想到你这么浪！",// 标题
            desc: "根据科学测试，易星人发现用什么牙膏就开什么车！",// 描述
            img: "http://image.bitautoimg.com/uimg/huodong/xiaobaixuanche/images/fengmian.png",// 图片
            img_title: "逗逼易星人用牙膏帮你选了款座驾，没想到你这么浪！",// 图片标题
            from: '易车' // 来源
        };
        var ua = window.navigator.userAgent.toLowerCase();
        if(ua.match(/MicroMessenger/i) == 'micromessenger'){
            document.title = share_config.title;
        }
    </script>
    <!-- 分享 start-->
    <!--#include file="/include/pd/2014/wapCommon/00001/201603_wap_fx_gg_Manual.shtml"-->
    <!--分享 end -->

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
</body>
</html>
