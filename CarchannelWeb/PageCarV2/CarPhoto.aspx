<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhoto.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageCarV2.CarPhoto" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSeeNew.ascx" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片】_<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片库-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="keywords" content="<%= cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim() %>图片,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内饰图,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>" />
    <meta name="description" content="<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片:易车网(BitAuto.com)图片频道为您提供专业高清<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片,展示各种角度<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片,包括<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>车身外观,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内饰,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内部空间,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>行驶,创意图,壁纸,活动等。" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_tupianshuxing_style-1276.shtml"-->
    <script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--a_d start-->
    <div style="margin: 10px auto 10px; width: 1200px;">
        <!-- AD New Dec.31.2011 -->
        <ins id="div_e457221e-faa7-4799-9172-b09cc8c30c91" type="ad_play" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
            adplay_blockcode="e457221e-faa7-4799-9172-b09cc8c30c91"></ins>
    </div>
    <!--a_d end-->
    <!--smenu start-->
    <%= CarPhotoHeadHTML%>
    <!--smenu end-->
    <!--contain begin-->
    <div class="container img-section summary">
        <div class="col-xs-9">
            <!--col-con start-->
            <div class="img-section-main">
                <%=CarPhotoHtml %>
                <!-- SEO底部热门 -->
                <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                <!-- SEO底部热门 -->
            </div>
            <!--col-con end-->
        </div>
        <div class="col-xs-3 img-section-right">
            <div class="advertise-top" style="width: 220px; overflow: hidden">
                <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                    adplay_brandid="<%= cbe.Cs_Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
            </div>
            <div class="other-car layout-1">
                <%= hotCarsHtml %>
            </div>
            <Car:SerialToSee ID="ucSerialToSee" runat="server">
            </Car:SerialToSee>
            <!--此品牌下其别子品牌-->
            <div class="other-car layout-1">
                <%=GetBrandOtherSerial() %>
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
    <script type="text/javascript" language="javascript">
        var CarCommonBSID = "<%= carEntity.Serial == null || carEntity.Serial.BrandId == null ? 0 : carEntity.Serial.Brand.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = '<%= carEntity.Serial == null ? 0 : carEntity.Serial.BrandId %>';
        var CarCommonCSID = '<%= carEntity.Serial == null? 0 : carEntity.Serial.Id %>';
        var CarCommonCarID = '<%= CarID.ToString() %>';
    </script>
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/360pano/vrImgForBitauto.js"></script>
    <script type="text/javascript">
        //vr
        if (vrImgForBitauto != undefined && vrImgForBitauto.IntiDataForEntry != undefined) {
            vrImgForBitauto.IntiDataForEntry(<%=cbe.Cs_Id%>, function (vrImgs) {
                if (vrImgs.length > 0) {
                    $("#group_container ul>li").eq(3).after("<li><a target=\"_blank\" href=\"" + vrImgs[0].PanoUrl + "\" class=\"vr-lnk\">VR</a></li>");
                }
            });
        }
    </script>
    <!--本站统计代码-->
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript" language="javascript">
        OldPVStatistic.ID1 = "<%=cbe.Cs_Id %>";
        OldPVStatistic.ID2 = "<%=CarID.ToString() %>";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!--本站统计结束-->
    <!-- baa 浏览过的车型-->
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%=cbe.Cs_Id %>');
        }
        catch (err)
        { }
    </script>
    <!--contain end-->
    <!--提意见浮层-->
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=cbe.Cs_Id%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
        var zamplus_tag_params = {
            modelId:modelStr,
            carId:<%=CarID%>
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
