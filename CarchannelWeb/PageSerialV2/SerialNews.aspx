<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialNews.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerialV2.SerialNews" %>
<%@ Register Src="~/UserControls/SerialToSeeNew.ascx" TagPrefix="uc1" TagName="SerialToSeeNew" %>


<% Response.ContentType = "text/html"; %>
<%--<%@ OutputCache Duration="600" VaryByParam="*" %>--%>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit"/>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <title>
        <%= pageTitle %>
    </title>
    <meta name="Keywords" content="<%= pageKeywords %>"/>
    <meta name="Description" content="<%= pageDescription %>"/>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/CommonFunction.min.js?v=2061228"></script>
    <!--#include file="~/ushtml/0000/yiche_2016_cube_xinwenzongshu_style-1291.shtml"-->
</head>
<body>
<span id="yicheAnchor" style="display: block; font-size: 0; height: 0; line-height: 0; width: 0;"></span>
<!--#include file="~/htmlV2/header2016.shtml"-->
<!--a_d start-->
<div class="bt_ad">
    <!-- AD New Dec.31.2011 -->
    <% if (newsType == "hangqing")
       { %>
        <ins id="div_d8a72a70-98ff-4cec-a4f7-c2bb07d10829" type="ad_play" adplay_ip="" adplay_areaname="<%= cityIDForAD %>" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d8a72a70-98ff-4cec-a4f7-c2bb07d10829"></ins>
    <% }
       else
       { %>
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
    <% } %>
</div>
<!--a_d end-->
<!--smenu start-->
<%= SerialNewHead %>
<!--content-->
<div class="container">
    <div class="col-xs-9">
        <!--主体内容-->
        <div class="article-section-main summary">
            <%= newsNavHtml %>
            <div class="main-inner-section">

                <%= newsHtml %>

                <BitAutoControl:Pager ID="AspNetPager1" runat="server" CssClass="pagination" HrefPattern="String" Visible="false"
                                      NoLinkClassName="preview_off"/>

                <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/newsViewCount.min.js?v=2061228"></script>
            </div>
            
            <!--接下来要看-->
            <div class="main-inner-section ">
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
                                        <%= serialShowName %>参数配置
                                    </a>
                                </div>
                            </li>
                            <li>
                                <div class="txt">
                                    <a href="/<%= cse.Cs_AllSpell %>/tupian/">
                                        <%= serialShowName %>图片
                                    </a>
                                </div>
                            </li>
                            <%= nextSeePingceHtml %>
                            <li>
                                <div class="txt">
                                    <a href="/<%= cse.Cs_AllSpell %>/baojia/">
                                        <%= serialShowName %>报价
                                    </a>
                                </div>
                            </li>
                            <li>
                                <div class="txt">
                                    <a href="http://www.taoche.com/<%= cse.Cs_AllSpell %>/" target="_blank">
                                        <%= serialShowName %>二手车
                                    </a>
                                </div>
                            </li>
                            <li>
                                <div class="txt">
                                    <a href="/<%= cse.Cs_AllSpell %>/koubei/">
                                        <%= serialShowName %>怎么样
                                    </a>
                                </div>
                            </li>
                            <li>
                                <div class="txt">
                                    <a href="/<%= cse.Cs_AllSpell %>/youhao/">
                                        <%= serialShowName %>油耗
                                    </a>
                                </div>
                            </li>
                            <li>
                                <div class="txt">
                                    <a href="<%= baaUrl %>" target="_blank">
                                        <%= serialShowName %>论坛
                                    </a>
                                </div>
                            </li>
                            <%= nextSeeDaogouHtml %>
                        </ul>
                    </div>
                </div>
            </div>
            <!--/接下来要看-->

            <!-- SEO底部热门推荐 -->
            <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
            <!-- SEO底部热门推荐 -->
        </div>
        <!--/主体内容-->
    </div>
    <div class="col-xs-3 article-section-right">
        <!-- 广告 start -->

        <ins id="div_20b7e84d-7a5d-461b-b484-b3d0fec5a81e" data-type="ad_play" data-adplay_IP="" data-adplay_AreaName="" 
            data-adplay_CityName="" data-adplay_BrandID="<%= serialId %>" data-adplay_BrandName="" 
            data-adplay_BrandType="" data-adplay_BlockCode="20b7e84d-7a5d-461b-b484-b3d0fec5a81e"></ins>
        <ins id="div_9e066309-54d0-4928-a251-b2f3c86ddc4d" data-type="ad_play" data-adplay_IP="" data-adplay_AreaName="" 
            data-adplay_CityName="" data-adplay_BrandID="<%= serialId %>" data-adplay_BrandName="" 
            data-adplay_BrandType="" data-adplay_BlockCode="9e066309-54d0-4928-a251-b2f3c86ddc4d"></ins>
    
        <!-- 广告 end -->

        <!--看了还看-->
        <uc1:SerialToSeeNew runat="server" ID="ucSerialToSee"/>
        <!--看了还看-->

        <!--大家都用他和谁比-->
        <%= carCompareHtml %>
        <!--大家都用他和谁比-->

        <!-- AD -->
        <!-- add by chengl Sep.13.2012 -->
       <%-- <ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c">
        </ins>--%>

        <!--其他车型-->
        <%= GetBrandOtherSerial() %>
        <!--其他车型-->

        <!--二手车-->
        <div class="old-car layout-1" id="line_boxforucar_box" style="display: none;">
            <div class="section-header header3">
                <div class="box">
                    <h2><%= serialShowName %>二手车</h2>
                </div>
            </div>
            <div class="img-box" id="ucar_serialcity">
            </div>
        </div>
        <!--二手车-->

    </div>
</div>
<!--/content-->

<script type="text/javascript">
    var CarCommonBSID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.MasterBrandId %>"; //大数据组统计用
    var CarCommonCBID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.Id %>";
    var CarCommonCSID = '<%= serialId.ToString() %>';
</script>
<script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
<script src="http://image.bitautoimg.com/carchannel/jsnewv2/ucarserialcity.min.js?v=2061228" type="text/javascript"></script>
<script type="text/javascript">
    var cityId = 201;
    if (typeof (bit_locationInfo) != 'undefined') {
        cityId = bit_locationInfo.cityId;
    }
    if (typeof(showUCar) != "undefined") {
        showUCar(<%= serialId %>, cityId, '<%= cse.Cs_AllSpell %>', '<%= serialShowName %>', getUCarForSider, undefined, undefined, undefined, 'zsy_wz');
    }

    //二手车数据
    function getUCarForSider(data, csId, csSpell, csShowName) {
        try {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var strHtml = [];
            $.each(data, function(i, n) {
                if (i > 6) return;
                strHtml.push("<div class=\"img-info-layout img-info-layout-14093\">");
                strHtml.push("    <div class=\"img\">");
                strHtml.push("        <a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">");
                strHtml.push("            <img src=\"" + n.PictureUrl + "\">");
                strHtml.push("        </a>");
                strHtml.push("    </div>");
                strHtml.push("    <ul class=\"p-list\">");
                strHtml.push("        <li class=\"name\">");
                strHtml.push("            <a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.BrandName + "</a>");
                strHtml.push("        </li>");
                strHtml.push("        <li class=\"info\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</li>");
                strHtml.push("        <li class=\"price\">");
                strHtml.push("            <a target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.DisplayPrice + "</a>");
                strHtml.push("        </li>");
                strHtml.push("    </ul>");
                strHtml.push("</div>");
            });
            $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
        } catch (e) {
        }
    }
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
<% if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
   { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
<% } %>
<!-- 调用尾 -->
<!--#include file="~/htmlv2/rightbar.shtml"-->
<!--#include file="~/htmlV2/footer2016.shtml"-->
<!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
<!-- 晶赞 -->
<script type="text/javascript">
    var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
        modelStr = '<%= serialId %>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
    var zamplus_tag_params = {
        modelId: modelStr,
        carId: 0
    };
</script>
<script type="text/javascript">
    var _zampq = [];
    _zampq.push(["_setAccount", "12"]);
    (function() {
        var zp = document.createElement("script");
        zp.type = "text/javascript";
        zp.async = true;
        zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(zp, s);
    })();
</script>
<!-- 晶赞 -->
</body>
</html>
<script type="text/javascript">
    $(".the_pages.pages_top").removeClass("pages_top");
    if (document.getElementById("car_tab_ul3") && document.getElementById("data_table3_0")) {
        addLoadEvent(function() { tabs("car_tab_ul3", "data_table3", null, true) });
    }
</script>
<!--本站统计代码-->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript">
    OldPVStatistic.ID1 = "<%= serialId.ToString() %>";
    OldPVStatistic.ID2 = "0";
    OldPVStatistic.Type = 0;
    mainOldPVStatisticFunction();
</script>
<!--本站统计结束-->
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->