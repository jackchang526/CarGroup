<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompareList.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.CarCompareList" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <title>
        <%= titleForSEO %></title>
    <meta name="description" content="车型对比:易车网车型对比频道为您提供各种车型对比参数,包括最新汽车报价、基本性能、车身结构、内外尺寸、汽车参数、便利功能、安全配置、等车型对比信息……" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingduibigongju-772.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        var tagIframe = null;
        var currentTagId = 22; 	//当前页的标签ID
    </script>
</head>
<body data-offset="-150" data-target=".left-nav" data-spy="scroll">
    <!--#include file="~/html/header2014.shtml"-->
    <ins id="div_4fe917c8-6ac2-46e4-a844-4bb387a1b639" type="ad_play" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="4fe917c8-6ac2-46e4-a844-4bb387a1b639"></ins>
    <!--smenu start-->
    <div class="header_style" id="box">
        <div class="car_navigate">
            <div>
                <span>您当前的位置：</span><a rel="nofollow" href="http://www.bitauto.com/" target="_blank">易车</a>
                &gt; <a rel="nofollow" href="/" target="_blank">车型</a> &gt; <a href="/duibi/" target="_blank">综合对比</a> &gt; <strong>参数对比</strong>
            </div>
        </div>
        <div class="bt_searchNew">
            <!--#include file="~/html/bt_searchV3.shtml"-->
        </div>
        <div id="divSerialSummaryMianBaoAD" class="top_ad02">
            <ins id="div_ba10f730-0c13-4dcf-aa81-8b5ccafc9e21" type="ad_play" adplay_ip="" adplay_areaname=""
                adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                adplay_blockcode="ba10f730-0c13-4dcf-aa81-8b5ccafc9e21"></ins>
        </div>
    </div>
    <!--smenu end-->
    <!--page start-->
    <div class="bt_page">
        <!-- 收起 -->
        <style>
            .bt_page { overflow: visible; }
        </style>
        <!--[if IE 6]> <style>.bt_page{ overflow:hidden !important;}</style> <![endif]-->
        <div class="publicTabNew">
            <ul class="tab">
                <li><a href="/duibi/<%= carIDs==""?"":(listValidCsID.Count()>=2?(string.Join("-",listValidCsID.Take(2)))+"/":"") %><%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">综合对比</a></li>
                <li class="current"><a href="/chexingduibi/">参数对比</a></li>
                <li><a href="/tupianduibi/<%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">图片对比</a></li>
                <%--<li><a href="http://koubei.bitauto.com/duibi/<%= csIDs==""?"":csIDs+".html" %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"?carIDs="+carIDs) %>">口碑对比</a></li>
				<li><a href="/pingceduibi/<%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">评测对比</a></li>--%>
            </ul>
        </div>
        <!-- 和首选车型对比最多的车型 -->
        <div id="CarHotCompareList">
        </div>
        <!-- -->
        <div class="car_compare_list" style="position: relative; clear: both">
        </div>
        <!-- 车型对比 -->
        <div id="main_box" class="line_box_compare y2015">
            <!-- 浮动Left -->
            <div id="leftfixed" style="display: none;">
            </div>
            <div id="CarCompareContent">
            </div>
            <%if (!string.IsNullOrEmpty(carIDs))
              { %>
            <em class="btn-show-left-nav" style="display: none;" id="show-left-nav"></em>
            <!-- 左侧浮动层 start -->
            <div class="left-nav left-nav-duibi" id="left-nav" style="display: none;">
                <ul>
                </ul>
                <a href="javascript:;" class="close-left-nav" id="close-left-nav" style="display: none;">关闭浮层</a>
                <%--<em class="btn-hide-left-nav" style="display: none;"></em><a href="javascript:;"
					class="duibi-return-top" title="返回顶部">返回顶部</a>--%>
            </div>
            <!-- 左侧浮动层 end -->
            <%} %>
            <div class="td-tips">
                <div class="ts-box">
                    以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a target="_blank" href="http://www.bitauto.com/feedback/">欢迎您及时指正！</a>
                </div>
            </div>
        </div>
        <!-- 车型对比 -->
        <!--左上小浮动层开始-->
        <div id="smallfixed" class="floatLayer w170" style="display: none;">
            <table cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <th class="pd0">
                            <div class="tableHead_left">
                                <div class="check-box-item">
                                    <input type="checkbox" id="left_chkAdvantage" name="chkAdvantage" onclick="advantageForCompare();">
                                    <label for="left_chkAdvantage">
                                        标识优势项 <em></em>
                                    </label>
                                </div>
                                <div class="check-box-item">
                                    <input type="checkbox" id="left_checkboxForDiff" name="checkboxForDiff" onclick="showDiffForCompare();">
                                    <label for="left_checkboxForDiff">
                                        高亮不同项</label>
                                </div>
                                <div class="check-box-item">
                                    <input type="checkbox" name="checkboxForDelTheSame" onclick="delTheSameForCompare();"
                                        id="left_checkDiffer">
                                    <label for="left_checkDiffer">
                                        隐藏相同项</label>
                                </div>
                                <div class="dashline">
                                </div>
                                <p>
                                    ●标配 ○选配&nbsp;&nbsp;- 无
                                </p>
                            </div>
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
        <!--左上小浮动层结束-->
        <!-- 浮动Top -->
        <div class="floatLayer" id="topfixed" style="display: none;">
        </div>
        <div class="tc-popup-box" id="popup-box" style="display: none;">
            <div class="tt">
                <h6>参数纠错</h6>
                <a href="javascript:;" class="btn-close">关闭</a>
            </div>
            <div class="tc-popup-con tc-popup-error-correction">
                <textarea id="correctError"></textarea>
                <div class="alert">
                    <span></span>
                </div>
                <div class="button_orange button_99_35">
                    <a href="javascript:;" name="btnCorrectError">提交</a>
                </div>
                <div class="button_gray button_99_35">
                    <a href="javascript:;" id="btnErrorCancel">取消</a>
                </div>
            </div>
        </div>
        <div class="tc-popup-box" id="popup-box-success" style="display: none;">
            <div class="tt">
                <h6>参数纠错</h6>
                <a href="javascript:;" class="btn-close">关闭</a>
            </div>
            <div class="tc-popup-con">
                <div class="no-txt-box have-txt-box">
                    <p class="tit">
                        提交成功
                    </p>
                    <p>
                        您提交的纠错信息我们已经收集到，感谢您的纠错。
                    </p>
                    <div class="button_gray button_94_35">
                        <a href="javascript:;" id="btn-success-close">关闭</a>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="pop_compare_forcarlist" class="pop_compare" style="position: absolute; top: 120px; left: 500px; z-index: 111; display: none;">
        </div>

        <div style="display: none;">
            <%= compareHTMLForSeo %>
        </div>

        <!-- 推荐车型 -->

    </div>
    <div class="rightfixed" id="rightfixed" style="display: none;">
        <div class="rightfixed-tt">
            <span>推荐对比</span>
            <a href="javascript:;" id="right-close">隐藏</a>
        </div>
        <div class="rightfixed-list" id="rightfixed-list">
            <ul>
            </ul>
        </div>
    </div>
    <div class="rightfixed-bar" id="rightfixed-bar" style="display: none;">
        <span>推荐对比</span>
        <div class="compare-arrow"></div>
    </div>
    <div class="loading" style="left: 50%; top: 400px; display: none; position: fixed; z-index: 100; margin-left: -120px;" id="selectCarLoadingDiv">
        <i></i>
        <p>正在加载中...</p>
    </div>
    <script type="text/javascript">
		<!--adid:[id,id]-->
    //var adJson = { Serial: {}, Car: { 117374: [115813],120758:[120732],120755:[120734,120736],120757:[120172]} };
    var adLevelJson = [{ "level": "中大型车", "serialId": "2109", "defCarId": "116090" }];//
    //var specialADConfig={ "c114448": [110648,112888],"c114451":[110651,113913,115323],"c114449":[112890]};
    var carCompareAdJson = {
        "0": [
        { serialids: [4418, 3152, 4616], carad: { masterid: 258, serialid: 4798} }

        ]
        //"201": { serialids: [2370, 2381, 2932], carad: { masterid: 127, serialid: 2731} }
        //"2601": { serialids: [2871, 2581], carad: { masterid: 26, serialid: 3532} }
    };
    var allCarInfo=<%=allCarInfo %>;
		<%= serialLevelIndexRank %>
		<%= allCarJsArray %>
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/draggable.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlistnewV3.min.js?v=201509018"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/CarChannelBaikeJson.js?v=20150831"></script>
    <script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/carcomparelist.min.js?v=20161116"></script>
    <%--<script type="text/javascript" src="/jsnew/carcomparelist.js?v=20160225"></script>--%>
    <script type="text/javascript">
        initPageForCompare();
        (function () {
            if (window.navigator.userAgent.indexOf("Chrome") !== -1) {
                var count = 0;
                var timer = setInterval(function () {
                    count++;
                    if (count > 60) clearInterval(timer);
                    var obj = $("body>div>object[data*='irs01.net']");
                    if (obj.length > 0) {
                        clearInterval(timer);
                        obj.parent("div").css({ right: "" });
                    }
                }, 500);
            }
        })();
        $(function () {
            $("#backtop").attr("href", "javascript:;").bind("click", function () {
                $("html,body").animate({ scrollTop: 0 }, 300, function () {
                    //modified by 2014.07.17 ie6 7 8 修改 滚动监听 回不到基本信息
                    if (! -[1, ]) {
                        $("#left-nav li:first").addClass("current").siblings().removeClass("current");
                    }
                });
            });
        });
     
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
    <!--#include file="~/html/footer2014.shtml"-->
    <script type="text/javascript">
        $("#right-close").click(function () { $("#rightfixed").hide(); $("#rightfixed-bar").show(); });
        $("#rightfixed-bar").click(function () { $("#rightfixed").show(); $(this).hide(); });

        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <!--提意见浮层-->
    <!--# include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfhdbfdc_Manual.shtml"-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        $("#feedbackDiv").before("<li class=\"w4 d11-backtop\"><a href=\"http://survey01.sojump.com/jq/7445589.aspx\"  title=\"\" target=\"_blank\">问卷调查</a></li>");
    </script>
</body>
</html>
