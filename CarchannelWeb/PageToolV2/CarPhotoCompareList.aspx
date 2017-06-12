<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhotoCompareList.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.CarPhotoCompareList" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title><%= titleForSEO %>-易车网</title>
    <meta name="keywords" content="汽车图片,图比对比,易车网" />
    <meta name="description" content="汽车图片对比:易车网车型频道为您提供各种角度汽车图片对比。包括汽车内饰、外观、内部空间对比，正车头、正45度、正侧、后45度、中网、大灯、雾灯、雨刷器、轮胎、后视镜、拉手、尾灯、尾标、排气管等图片详细对比" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_duibigongju_style-1262.shtml"-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        <!--
    var tagIframe = null;
    var currentTagId = 23; 	//当前页的标签ID
    -->
    </script>
</head>
<body data-offset="-145" data-target=".left-nav" data-spy="scroll">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <ins id="div_4fe917c8-6ac2-46e4-a844-4bb387a1b639" type="ad_play" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="4fe917c8-6ac2-46e4-a844-4bb387a1b639"></ins>
    <!--smenu start-->
    <header class="header-main summary-box">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-6 left-box">
            <div class="crumbs h-line">
                <div class="crumbs-txt">
                    <span>您当前的位置：</span><a rel="nofollow" href="http://www.bitauto.com/" target="_blank">易车</a> &gt; <a rel="nofollow" href="/" target="_blank">车型</a> &gt; <a href="/duibi/" target="_blank">综合对比</a> &gt; <strong>图片对比</strong>
                </div>
            </div>
            </div>
            <div class="col-xs-6 right-box">
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
                <div id="divSerialSummaryMianBaoAD" class="top-ad">
                    <ins id="div_ba10f730-0c13-4dcf-aa81-8b5ccafc9e21" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="ba10f730-0c13-4dcf-aa81-8b5ccafc9e21"></ins>
                </div>
            </div>
        </div>
        <div id="navBox">
            <nav class="header-main-nav">
            <div class="container">
                <div class="col-auto left secondary">
                    <ul class="list list-justified">
                        <li><a href="/duibi/<%= carIDs==""?"":(paramsSerialIdList.Count()>=2?(string.Join("-",paramsSerialIdList.Take(2)))+"/":"") %><%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">综合对比</a></li>
                        <li><a href="/chexingduibi/<%= csIDs ==""?"":"?csids="+csIDs+"" %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">参数对比</a></li>
                        <li class="active"><a href="#">图片对比</a></li>
                        <li><a href="/koubeiduibi/<%= carIDs==""?"":(carIDs.Split(',').Length>=2?"?car_id_l="+ carIDs.Split(',')[0] +"&car_id_r="+carIDs.Split(',')[1]:"") %>">口碑对比</a></li>
                    </ul>
                </div>
            </div>
        </nav>
        </div>
    </header>
    
    <div class="container">
	        <div class="pic-db-box">
		        <%=photoHeaderHtml %>
                <%if (isHasContent)
                { %>
                    <%=photoCompare%>
                <%}
                else
                { %>
                    <div class="img-compare-left">
                        <div class="recommend-box" id="recommend-box">
                        </div>
                    </div>
                    <div class="img-compare-mid">
                        <div class="img-box">
                        </div>
                    </div>
                    <div class="img-compare-right">
                        <div class="img-box">
                        </div>
                    </div>
                <%} %>
            <div class="clear"></div>
            <%=menuSideHtml%>
	      </div>
        </div>
    <script type="text/javascript">
        var carInfoJson = <%=carInfoJson %>;
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/dropdownlistnew.min.js?v=20161130"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/photoCompare.min.js?v=201612011545"></script>

    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/CarCompareStat.js?20150303"></script>
    <script type="text/javascript" language="javascript">
        CarCompareStatistic.CarIDs = "<%= carIDs %>";
        // 临时统计
        var movetimes = 0;
        $(document).mousemove(function (even) {
            movetimes++;
            if (movetimes > 50) {
                $(document).unbind("mousemove");
                mainCarCompareStatisticFunction();
            }
        });
    </script>
    <!--#include file="~/htmlV2/footer2016.shtml"-->
    <% if (!isHasContent)
       { %>
            <script type="text/javascript">
                var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
                document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
            </script>
    <% } %>
    <!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
</body>
</html>
