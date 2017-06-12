<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhotoByYear.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageYearV2.CarPhotoByYear" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSeeNew.ascx" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=strCs_SeoName%>
        <%= this.CarYear.ToString()+"款" %>图片】_<%=strCs_MasterName%><%=strCS_Name%>
        <%= this.CarYear.ToString()+"款" %>图片库-易车网</title>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="keywords" content="<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>内饰图,<%=strCs_MasterName%><%=strCs_ShowName%> <%= this.CarYear.ToString()+"款" %>" />
    <meta name="description" content="<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片:频道为您提供<%=strCs_MasterName%><%=strCS_Name%> <%= this.CarYear.ToString()+"款" %>内饰,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>内部空间,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>行驶,创意图,壁纸,活动等,查<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片,就上易车网" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_tupianshuxing_style-1276.shtml"-->
    <script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>    
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--a_d start-->
    <div class="bt_ad">
        <%=serialTopAdCode%>
    </div>
    <!--a_d end-->
    <!--smenu start-->
    <%= CsHeadHTML %>
    <script type="text/javascript">
        <%= JsTagForYear %>
        var CarCommonBSID = "<%=serialEntity.Brand == null ? 0 : serialEntity.Brand.MasterBrandId%>";
        var CarCommonCBID = "<%=serialEntity.Brand == null ? 0 : serialEntity.BrandId%>";
        var CarCommonCSID = "<%=serialEntity.Id%>";
    </script>
    <!--smenu end-->
    <!--contain begin-->
    <div class="container img-section summary">
        <div class="col-xs-9">
            <!--col-con start-->
            <div class="img-section-main">
                <%=SerialYearPhotoHtml%>
                <!-- SEO导航 -->
                <div class="main-inner-section">
                    <div class="section-header header2">
                        <div class="box">
                            <h2>接下来要看</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="list-txt list-txt-m list-txt-default list-txt-style6">
                            <ul>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= cse.Cs_AllSpell %>/peizhi/">
                                            <%= strCs_ShowName%>参数配置</a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= cse.Cs_AllSpell %>/tupian/">
                                            <%= strCs_ShowName%>图片</a>
                                    </div>
                                </li>
                                <%=nextSeePingceHtml %>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= cse.Cs_AllSpell %>/baojia/">
                                            <%= strCs_ShowName%>报价</a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="http://www.taoche.com/brand.aspx?spell=<%= cse.Cs_AllSpell %>">
                                            <%= strCs_ShowName%>二手车</a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= cse.Cs_AllSpell %>/koubei/">
                                            <%= strCs_ShowName%>怎么样</a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= cse.Cs_AllSpell %>/youhao/">
                                            <%= strCs_ShowName%>油耗</a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="<%= baaUrl %>" target="_blank">
                                            <%= strCs_ShowName%>论坛</a>
                                    </div>
                                </li>
                                <%=nextSeeDaogouHtml %>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- SEO底部热门 -->
                <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                <!-- SEO底部热门 -->
            </div>
            <!--col-con end-->
            <!--col-side end-->

            <!--col-side end-->
            <div class="clear">
            </div>
        </div>
        <div class="col-xs-3 img-section-right">
            <!-- ad -->
            <div class="advertise-top" style="width: 220px; overflow: hidden">
                <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                    adplay_brandid="<%= CSID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
            </div>
            <!--车型图片对比-->
            <%= CsHotCompareCars%>
            <Car:SerialToSee ID="ucSerialToSee" runat="server"></Car:SerialToSee>
            <!--此品牌下其别子品牌-->
             <div class="other-car layout-1">
                <%= GetBrandOtherSerial() %>
            </div>
            <!--百度推广-->
            <%--<div class="line_box baidupush line_box_top_b">
                <script type="text/javascript">
                    /*bitauto 200*200，导入创建于2011-10-17*/
                    var cpro_id = 'u646188';
                </script>
                <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
            </div>--%>
            <!--百度推广-End -->
        </div>
    </div>
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/360pano/vrImgForBitauto.js"></script>
    <script type="text/javascript">
        //vr
        if (vrImgForBitauto != undefined && vrImgForBitauto.IntiDataForEntry != undefined) {
            vrImgForBitauto.IntiDataForEntry(<%=CSID%>, function (vrImgs) {
                if (vrImgs.length > 0) {
                    $("#group_container ul>li").eq(3).after("<li><a target=\"_blank\" href=\"" + vrImgs[0].PanoUrl + "\" class=\"vr-lnk\">VR</a></li>");
                }
            });
        }
    </script>
    <!--本站统计代码-->
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript" language="javascript">
        OldPVStatistic.ID1 = "<%=CSID.ToString() %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!--本站统计结束-->
    <!--contain end-->
    <!--提意见浮层-->
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
			modelStr = '<%=CSID%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
	    var zamplus_tag_params = {
	        modelId: modelStr,
	        carId: 0
	    };
    </script>
    <script type="text/javascript">
        var _zampq = [];
        _zampq.push(["_setAccount", "12"]);
        (function () {
            var zp = document.createElement("script"); zp.type = "text/javascript"; zp.async = true;
            zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
            var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(zp, s);
        })();
    </script>
</body>
</html>
