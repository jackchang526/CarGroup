<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsPhoto.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerialV2.CsPhoto" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=strCs_SeoName%>图片】_<%=cse.MasterName+cse.Cs_Name%>图片_<%=strCs_SeoName%>图片大全-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="keywords" content="<%=strCs_SeoName%>图片,<%=strCs_SeoName%>外观,<%=strCs_SeoName%>内饰图,<%=cse.MasterName+cse.Cs_Name%>,易车网,car.bitauto.com" />
    <meta name="description" content="<%=cse.MasterName+cse.Cs_Name%>图片，展示各种角度<%=strCs_SeoName%>图片,包括<%=strCs_SeoName%>外观,<%=strCs_SeoName%>内饰,<%=strCs_SeoName%>内部空间,<%=strCs_SeoName%>行驶等最新<%=strCs_SeoName%>图片壁纸。" />
    <meta name="mobile-agent" content="format=html5; url=http://photo.m.yiche.com/serial/<%= serialId %>/" />
    <meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/photo.aspx?serialid=<%= serialId %>" />
    <link rel="canonical" href="http://car.bitauto.com/<%=cse.Cs_AllSpell %>/tupian/" />
    
    <!--#include file="~/ushtml/0000/yiche_2016_cube_tupianshuxing_style-1276.shtml"-->
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--a_d start-->
    <div style="margin:10px auto 10px;width:1200px;">
        <!-- AD New Dec.31.2011 -->
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
            adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
    </div>
    <!--a_d end-->
    <!--smenu start-->
    <%= CsHeadHTML %>
    <!--smenu end-->
    <!--contain begin-->
    <div class="container img-section summary">
        <%--<%=SerialInfoBarHtml%>--%>
        <div class="col-xs-9">
            <!--col-con start-->
            <div class="img-section-main">
                <%=SerialPhotoHtml %>
                <!-- AD -->
                <div class="line_box">
                    <ins id="div_406b829f-0166-4c17-bf92-2381668a81eb" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="406b829f-0166-4c17-bf92-2381668a81eb"></ins>
                </div>
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
                                <li><div class="txt"><a href="/<%= cse.Cs_AllSpell %>/peizhi/">
                                    <%= strCs_ShowName%>参数配置</a></div></li>
                                <li><div class="txt"><a href="/<%= cse.Cs_AllSpell %>/tupian/">
                                    <%= strCs_ShowName%>图片</a></div></li>
                                <%=nextSeePingceHtml %>
                                <li><div class="txt"><a href="/<%= cse.Cs_AllSpell %>/baojia/">
                                    <%= strCs_ShowName%>报价</a></div></li>
                                <li><div class="txt"><a href="http://www.taoche.com/<%= cse.Cs_AllSpell %>/" target="_blank">
                                    <%= strCs_ShowName%>二手车</a></div></li>
                                <li><div class="txt"><a href="/<%= cse.Cs_AllSpell %>/koubei/">
                                    <%= strCs_ShowName%>怎么样</a></div></li>
                                <li><div class="txt"><a href="/<%= cse.Cs_AllSpell %>/youhao/">
                                    <%= strCs_ShowName%>油耗</a></div></li>
                                <li><div class="txt"><a href="<%= baaUrl %>" target="_blank">
                                    <%= strCs_ShowName%>论坛</a></div></li>
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
        </div>
        <div class="col-xs-3 img-section-right">
            <!--车型图片对比-->
            <%= CsHotCompareCars%>
            <div class="after-looking layout-1" id="serialtosee_box">
                <div class="section-header header3">
                    <div class="box">
                        <h2>看了还看</h2>
                    </div>
                </div>
                <div class="img-list clearfix" id="serialtosee_content">
                   <%-- <%= serialToSeeHtml %>--%>
                </div>
            </div>
            <%--<car:SerialToSee ID="ucSerialToSee" runat="server"></car:SerialToSee>--%>
            <!-- AD -->
            <!-- add by chengl Sep.13.2012-->
            <%--<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
                adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c"></ins>--%>
            <!---->
            <!--此品牌下其别子品牌-->

            <div class="other-car layout-1">
                <%= GetBrandOtherSerial() %>
            </div>
            <!--二手车-->
            <%--<%=UCarHtml %>--%>
            <%--<div class="line_box ucar_box">
			</div>--%>
            <script type="text/javascript">
                var serialId = "<%= serialId %>";
                <%= serialToSeeJson %>
                //var SerialAdpositionContentInfo = {
                //    "2370": [
                //        {
                //            "SerialID": "2371",
                //            "Text": "速腾",
                //            "Link": "http://car.bitauto.com/suteng/",
                //            "Image": "http://img3.bitautoimg.com/autoalbum/files/20120816/666/0407166663_3.jpg"
                //        },null]
                //};
            </script>
            <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/serialtoseead.min.js"></script>
            <div class="layout-2">
                <ins id="div_a9e57a09-3485-4fbe-a755-59d7e52ce486" type="ad_play" adplay_ip="" adplay_areaname=""
                    adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                    adplay_blockcode="a9e57a09-3485-4fbe-a755-59d7e52ce486"></ins>
            </div>
            <div class="layout-2">
                <%--<script type="text/javascript" id="zp_script_95" src="http://mcc.chinauma.net/static/scripts/p.js?id=95&w=220&h=220&sl=1&delay=5"
					zp_type="1"></script>--%>
                <script type="text/javascript" id="zp_script_243" src="http://mcc.chinauma.net/static/scripts/p.js?id=243&w=240&h=220&sl=1&delay=5"
                    zp_type="1"></script>
            </div>
            <!--百度推广-->
            <%--<div class="layout-2">
                <script type="text/javascript">
                    /*bitauto 200*200，导入创建于2011-10-17*/
                    var cpro_id = 'u646188';
                </script>
                <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
            </div>--%>
            <!--百度推广-End -->
        </div>
    </div>
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <script type="text/javascript" language="javascript">
        var CarCommonBSID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.Id %>";
        var CarCommonCSID = '<%= serialId.ToString() %>';
    </script>
    <script type="text/javascript">
        var adplay_CityName = ''; //城市
        var adplay_AreaName = ''; //区域
        var adplay_BrandID = '<%= serialId %>'; //品牌id 
        var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
        var adplay_BrandName = ''; //品牌
        var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
    <!--#include file="~/htmlv2/footer2016.shtml"-->

     <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/360pano/vrImgForBitauto.js"></script>
    <script type="text/javascript">
        //vr
        if (vrImgForBitauto != undefined && vrImgForBitauto.IntiDataForEntry != undefined) {
            vrImgForBitauto.IntiDataForEntry(<%=serialId%>, function (vrImgs) {
                if (vrImgs.length > 0) {
                    $("#group_container ul>li").eq(3).after("<li><a target=\"_blank\" href=\"" + vrImgs[0].PanoUrl + "\" class=\"vr-lnk\">VR</a></li>");
                }
            });
        }
    </script>

    <!--本站统计代码-->
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript" language="javascript">
        OldPVStatistic.ID1 = "<%=serialId %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!--本站统计结束-->
    <!-- baa 浏览过的车型-->
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%=serialId.ToString() %>');
        }
        catch (err)
        { }
    </script>
    <!--contain end-->
    <%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
      { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <%} %>
    <!--提意见浮层-->
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
			modelStr = '<%=serialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
