<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompareList.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.CarCompareList" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <title>
        <%= titleForSEO %></title>
    <meta name="description" content="车型对比:易车网车型对比频道为您提供各种车型对比参数,包括最新汽车报价、基本性能、车身结构、内外尺寸、汽车参数、便利功能、安全配置、等车型对比信息……" />

    <!--#include file="~/ushtml/0000/yiche_2016_cube_dubi_chanshu_style-1263.shtml"-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript">
        var tagIframe = null;
        var currentTagId = 22; 	//当前页的标签ID
    </script>
</head>
<body data-target=".left-nav" data-spy="scroll">
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <ins id="div_4fe917c8-6ac2-46e4-a844-4bb387a1b639" type="ad_play" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="4fe917c8-6ac2-46e4-a844-4bb387a1b639"></ins>
    <!--smenu start-->
    <header class="header-main summary-box">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-6 left-box">
                <div class="crumbs h-line">
                    <div class="crumbs-txt">
                        <span>您当前的位置：</span><a rel="nofollow" href="http://www.bitauto.com/" target="_blank">易车</a>
                        &gt; <a rel="nofollow" href="/" target="_blank">车型</a> &gt; <a href="/duibi/" target="_blank">综合对比</a> &gt; <strong>参数对比</strong>
                    </div>
                </div>
            </div>
            <div class="col-xs-6 right-box">
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul id="calcTools" class="list list-justified">
                            <li><a href="/duibi/<%= carIDs==""?"":(listValidCsID.Count()>=2?(string.Join("-",listValidCsID.Take(2)))+"/":"") %><%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">综合对比</a></li>
                            <li class="active"><a href="/chexingduibi/">参数对比</a></li>
                            <li><a href="/tupianduibi/<%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">图片对比</a></li>
                            <% if(string.IsNullOrWhiteSpace(carIDs)){ %>
                                <li><a href="/koubeiduibi/">口碑对比</a></li>
                            <%}else{
                                  string[] caridArr = carIDs.Split(',');
                                   %>
                                 <li><a href="/koubeiduibi/?car_id_l=<%= caridArr[0] %><%= caridArr.Length>1?("&car_id_r="+caridArr[1]):"" %>">口碑对比</a></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    </header>
    <!--smenu end-->
    <div class="bt_page">
        <div class="container config-section summary">
            <div class="config-section-main">
                <div class="config-box">
                    <div class="line_box_compare line_box_compare_peizhi y2015" id="main_box">
                        <!-- 浮动Left -->
                        <div id="leftfixed" style="display: none;">
                        </div>
                        <div id="CarCompareContent">
                        </div>
                        <%if (!string.IsNullOrEmpty(carIDs))
                          { %>
                        <em class="btn-show-left-nav" style="display: none;" id="show-left-nav"></em>
                        <!-- 左侧浮动层 start -->
                        <div class="left-nav left-nav-duibi" id="left-nav" style="display: none; position: absolute; top: 260px; left: -80px;">
                            <%--<a href="https://survey01.sojump.com/jq/11684027.aspx" target="_blank" style="display:block;height:26px;line-height:26px;position:absolute;top:-36px;width:65px;font-size:12px;background:#ff4f53;text-align: center;color:#fff;">问卷调查</a>--%>
                            <ul>
                            </ul>
                            <a href="javascript:;" class="close-left-nav" id="close-left-nav" style="display: none;">关闭浮层</a>
                        </div>
                        <!-- 左侧浮动层 end -->
                        <%} %>
                        <div class="td-tips">
                            <div class="ts-box">
                                以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a target="_blank" href="http://www.bitauto.com/feedback/">欢迎您及时指正！</a>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- 浮动Top -->
                <div class="floatLayer floatLayer_peizhi line_box_compare line_box_compare_peizhi y2015" id="topfixed" style="display: none;">
                </div>
            </div>
        </div>
    </div>
    <div class="modal modal-md" id="popup-box" style="display: none;">
        <div class="modal-header">
            <h4>参数纠错</h4>
            <span class="close"></span>
        </div>
        <div class="modal-content">
            <div class="form-group">
                <textarea id="correctError" class="input textarea input-block error"></textarea>
                <span class="help help-block error" style="display: none;">&nbsp;</span>
            </div>
            <div class="form-group foot-btn">
                <a class="btn btn-primary btn-md" href="javascript:;" name="btnCorrectError">提交</a>
                <a class="btn btn-default btn-md" href="javascript:;" id="btnErrorCancel">取消</a>
            </div>
        </div>
    </div>
    <div class="modal modal-md" id="popup-box-success" style="display: none;">
        <div class="modal-header">
            <h4>参数纠错</h4>
            <span class="close"></span>
        </div>
        <div class="modal-content">
            <div class="form-group">
                <div class="ico"></div>
                <div class="info">
                    <h3>提交成功</h3>
                    <p class="tip">您提交的纠错信息我们已经收集到，感谢您的纠错。</p>
                </div>
            </div>
        </div>
    </div>
    <div class="rightfixed" id="rightfixed" style="display: none; top: 224px; right: 0px;">
        <div class="rightfixed-tt">
            <span>推荐对比</span>
            <a href="javascript:;" id="right-close">隐藏</a>
        </div>
        <div class="rightfixed-list" id="rightfixed-list">
            <ul>
            </ul>
        </div>
    </div>
    <div class="rightfixed-bar" id="rightfixed-bar" style="display: none; top: 224px; right: 0px;position:absolute;">
        <span>推荐对比</span>
        <div class="compare-arrow"></div>
    </div>

    <script type="text/javascript">
		<!--adid:[id,id]-->
    var adLevelJson = [{ "level": "中大型车", "serialId": "2109", "defCarId": "116181" }];
    //var specialADConfig={ "c114448": [110648,112888],"c114451":[110651,113913,115323],"c114449":[112890]};
    //var carCompareAdJson = {
    //    "0": [
    //    { serialids: [4418, 3152, 4616], carad: { masterid: 258, serialid: 4798} }

    //    ]
    //    //"201": { serialids: [2370, 2381, 2932], carad: { masterid: 127, serialid: 2731} }
    //    //"2601": { serialids: [2871, 2581], carad: { masterid: 26, serialid: 3532} }
    //};
    var allCarInfo=<%=allCarInfo %>;
		<%= serialLevelIndexRank %>
		<%= allCarJsArray %>
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/draggable.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/dropdownlistnew.min.js?v=20161201"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/CarChannelBaikeJson.js?v=20161201"></script>
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/carcomparelistv2.min.js?v=201711021427"></script>
    <%--<script type="text/javascript" src="/jsnewv2/carcomparelistv2.js?v=20161201"></script>--%>
    <script type="text/javascript">
        initPageForCompare();
      
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>

    <% if (!isRec)
       { %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/CarCompareStat.js?20120905"></script>
    <script type="text/javascript">
        CarCompareStatistic.CarIDs = "<%= carIDs %>";
        // 临时统计
        var movetimes = 0;
        $(document).mousemove(function (even) {
            movetimes++;
            if (movetimes > 100) {
                $(document).unbind("mousemove");
                mainCarCompareStatisticFunction();
            }
        });
    </script>
    <% } %>
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <div style="display: none;">
        <%= compareHTMLForSeo %>
    </div>
</body>
</html>

