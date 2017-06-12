<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialYearNews.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageYearV2.SerialYearNews" %>
<%@ Register Src="~/UserControls/SerialToSeeNew.ascx" TagPrefix="uc1" TagName="SerialToSeeNew" %>
<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
     <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <title>
        <%=pageTitle%>
    </title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
   <!--#include file="~/ushtml/0000/yiche_2016_cube_xinwenzongshu_style-1291.shtml"-->
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->
<%--    <div class="bt_ad">
    <!-- AD New Dec.31.2011 -->
    <% if (newsType == "hangqing")
       { %>
        <ins id="div_d8a72a70-98ff-4cec-a4f7-c2bb07d10829" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d8a72a70-98ff-4cec-a4f7-c2bb07d10829"></ins>
    <% }
       else
       { %>
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
    <% } %>
</div>--%>
    <%= SerialNewHead %>
    <script type="text/javascript">
        <%= JsForCommonHead %>
    </script>
    <!--page start-->
    <div class="container">
        <div class="col-xs-9">
            <div class="article-section-main summary">
                <%=newsNavHtml %>
                <%= newsHtml %>
                <BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false"
                    NoLinkClassName="preview_off" />

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
                                        <a href="/<%= serialBase.AllSpell %>/peizhi/">
                                            <%= serialShowName %>参数配置
                                        </a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= serialBase.AllSpell %>/tupian/">
                                            <%= serialShowName %>图片
                                        </a>
                                    </div>
                                </li>
                                <%= nextSeePingceHtml %>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= serialBase.AllSpell %>/baojia/">
                                            <%= serialShowName %>报价
                                        </a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="http://www.taoche.com/<%= serialBase.AllSpell %>/" target="_blank">
                                            <%= serialShowName %>二手车
                                        </a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= serialBase.AllSpell %>/koubei/">
                                            <%= serialShowName %>怎么样
                                        </a>
                                    </div>
                                </li>
                                <li>
                                    <div class="txt">
                                        <a href="/<%= serialBase.AllSpell %>/youhao/">
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
                <!-- SEO底部热门推荐 -->
                <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"--> 
                <!-- SEO底部热门推荐 -->
            </div>
        </div>
        <div class="col-xs-3 article-section-right">
            <%--<div class="layout-1 adv-sidebar">
                <ins id="div_20b7e84d-7a5d-461b-b484-b3d0fec5a81e" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%=serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="20b7e84d-7a5d-461b-b484-b3d0fec5a81e"></ins>
            </div>
            <div class="layout-1 adv-sidebar">
                <ins id="div_9e066309-54d0-4928-a251-b2f3c86ddc4d" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%=serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="9e066309-54d0-4928-a251-b2f3c86ddc4d"></ins>
            </div>--%>
            <uc1:SerialToSeeNew runat="server" ID="ucSerialToSee"/>
            <%=carCompareHtml%>
            <%=GetBrandOtherSerial() %>
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
     <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newsViewCount.min.js"></script>
<%--    <script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnewV2/ucarserialcity.min.js?v=2016122815" type="text/javascript"></script>
    <script type="text/javascript">
        var CarCommonBSID = "<%=serialBase.Brand == null ? 0 : serialBase.Brand.MasterBrandId%>";
        var CarCommonCBID = "<%=serialBase.BrandId%>";
        var CarCommonCSID = "<%=serialId%>";
        (function(){
            $(".the_pages.pages_top").removeClass("pages_top");
            var cityId = 201;
            if(typeof (bit_locationInfo) != 'undefined'){
                cityId = bit_locationInfo.cityId;
            }
            if(typeof(showUCar)!="undefined"){ 
                showUCar(<%=serialId %>, cityId,'<%=serialBase.AllSpell %>','<%=serialBase.ShowName%>',getUCarForSider);
        }
    })();
        //二手车数据
        function getUCarForSider(data, csId, csSpell, csShowName) {
            try {
                data = data.CarListInfo;
                if (data.length <= 0) return;
                var strHtml = [];
                $.each(data, function(i, n) {
                    if (i > 6) return;
                    strHtml.push("<div class=\"img-info-layout img-info-layout-14093\">");  //去掉样式 no-wrap后，则页面中右侧文字处允许折行
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
    <!-- 调用尾 -->
    <!--#include file="~/htmlV2/rightbar.shtml"-->
    <!--#include file="~/htmlV2/footer2016.shtml"-->
    <!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
</body>
</html>
