<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerialV2.CsSummary" %>

<%@ Register Src="~/UserControls/NextToSee.ascx" TagPrefix="uc1" TagName="NextToSee" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=serialSeoName %>】最新<%=serialSeoName %>报价_参数_图片_<%=serialEntity.Brand.MasterBrand.Name+serialName %>社区-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta name="Keywords" content="<%=serialSeoName%>,<%= serialSeoName%>报价,<%= serialSeoName%>价格,<%= serialSeoName%>参数,<%= serialSeoName%>社区,易车网,car.bitauto.com" />
    <meta name="Description" content="<%= serialEntity.Brand.MasterBrand.Name + serialName%>,易车提供全国官方4S店<%= serialSeoName%>报价,最新<%= serialEntity.Brand.MasterBrand.Name + serialName%>降价优惠信息。以及<%= serialSeoName%>报价,<%= serialSeoName%>图片,<%= serialSeoName%>在线询价服务,低价买车尽在易车网 " />
    <meta http-equiv="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=serialSpell %>/" />
    <link rel="alternate" media="only screen and (max-width: 640px)" href=" http://car.m.yiche.com/<%=serialSpell %>/" />
    <link rel="canonical" href="http://car.bitauto.com/<%=serialSpell %>/" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <!--#include file="~/ushtml/0000/yiche_2016_cube_chexingzongshu_style-1264.shtml"-->
</head>
<body>

    <!--公共头部开始-->
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <div class="bt_pageBox">
        <!--#include file="~/htmlv2/header2016.shtml"-->
    </div>
    <!--书角广告代码-->
    <ins id="div_0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a" type="ad_play_fs" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a"></ins>
    <!-- 顶通新增收起广告(2596锐界) -->
    <ins id="div_2e7e5549-b9d7-4fb3-bec7-9f7b39efe715" type="ad_play_fs" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="2e7e5549-b9d7-4fb3-bec7-9f7b39efe715"></ins>
    <!-- 顶通新增收起广告(2596锐界)-->
    <div class="bt_ad" style="margin: 10px auto 10px; width: 1200px;">
        <ins id="div_2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02" type="ad_play_fs" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
            adplay_blockcode="2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02"></ins>
    </div>
    <div class="bt_ad" style="margin: 10px auto 10px; width: 1200px;">
        <!--综述页顶部通栏广告-->
        <ins id="topADLeftFromCar" type="ad_play_fs" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="80c10c31-34ab-4a36-bde5-549e292c5327"></ins>
    </div>
    <!--header start-->
    <%= serialHeaderHtml%>
    <!--/公共头部结束-->
    <!--通栏广告块-->
    <div class="top-col6-190" data-channelid="2.21.1524" style="display: none;">
        <div class="container">
            <div class="row col6-190-box">
                <div class="figure-box w190-h120 col-auto">
                    <ins id="div_040b5cb9-85ab-485f-a32b-5bfad0b9d891" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891"></ins>
                </div>
                <div class="figure-box w190-h120 col-auto">
                    <ins id="div_ae976a77-d988-46a4-8032-05997aa0a591" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="ae976a77-d988-46a4-8032-05997aa0a591"></ins>
                </div>
                <div class="figure-box w190-h120 col-auto">
                    <ins id="div_9957c7cc-f9ae-431e-bfc6-270e006a285e" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e"></ins>
                </div>
                <div class="figure-box w190-h120 col-auto">
                    <ins id="div_38796f7a-f21c-4644-93f4-527a483011c4" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="38796f7a-f21c-4644-93f4-527a483011c4"></ins>
                </div>
                <div class="figure-box w190-h120 col-auto">
                    <ins id="div_6fd6682f-3f99-4439-bd1a-346b128f017d" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="6fd6682f-3f99-4439-bd1a-346b128f017d"></ins>
                </div>
                <div id="cmtadDiv" class="figure-box w190-h120 col-auto">
                </div>
            </div>
        </div>
    </div>
    <!--/通栏广告块-->
    <div class="container cartype-section summary">
        <!--焦点图、名片区-->
        <div class="row card-head-box">
            <div class="l-box-sty col-auto">
                <%if (!string.IsNullOrEmpty(VRUrl))
                    {%>
                <a href="<%= VRUrl %>" target="_blank" class="zs-vr">VR看全景</a>
                <%} %>
                <% if (serialEntity.Brand.MasterBrandId == 3)
                    { %>
                <div class="bmw-ad-link">
                    <a href="http://c.ctags.cn/sy6/cu7/pc3/mt1?http://bmw.yiche.com/?rfpa_tracker=1_3_1" target="_blank" class="link">这里有关于宝马的一切 &gt;</a>
                </div>
                <%} %>
                <!--焦点图开始-->
                <%=focusImagesHtml%>
                <!--焦点图结束-->
            </div>
            <div class="r-box-sty col-auto">
                <div class="top">
                    <ul>
                        <li>
                            <div class="lowest-price">
                                <% if (serialPrice == "未上市" || serialPrice == "暂无报价" || serialPrice == "停售")
                                    { %>
                                <h2 class="not-onsale"><%= serialPrice %></h2>
                                <%}
                                    else
                                    { %>
                                <h2>
                                    <span class="note">全国参考价：</span><a href="http://car.bitauto.com/<%=serialSpell %>/baojia/" data-channelid="2.21.853" target="_blank" class="price"><%= serialPrice %></a>
                                    <a href="http://dealer.bitauto.com/<%=serialSpell %>/" target="_blank" id="mp-dealer" data-channelid="2.21.1526" class="local-agents"></a>
                                </h2>
                                <%} %>
                                <a class="btn btn-secondary2 btn-sm" href="javascript:;" data-channelid="2.21.799" id="favstar">+ 关注</a>
                            </div>
                        </li>
                        <li>
                            <div class="factory-price">
                                <h5 id="factory-price"><span class="note">厂商指导价：</span><span class="price<%= "暂无" ==(serialInfo.CsSaleState == "停销"?noSaleLastReferPrice:serialEntity.ReferPrice ) ? " grey-txt":"" %>"><%= serialInfo.CsSaleState == "停销"?noSaleLastReferPrice:serialEntity.ReferPrice %></span>
                                    <%= mpSerialYouHuiHtml %>
                                </h5>
                            </div>
                        </li>
                        <li>
                            <div class="info">
                                <ul class="list list-gapline sm">
                                    <% if (isElectrombile)
                                        {%>
                                    <li>
                                        <span class="note">续航里程: </span><span class="data<%= string.IsNullOrWhiteSpace(mileageRange) ? " grey-txt":"" %>"><%=string.IsNullOrWhiteSpace(mileageRange) ?"暂无":mileageRange %></span>
                                    </li>
                                    <li>
                                        <span class="note">充电时间: </span><span class="data<%= string.IsNullOrWhiteSpace(chargeTimeRange) ? " grey-txt":"" %>"><%=string.IsNullOrWhiteSpace(chargeTimeRange) ? "暂无":chargeTimeRange%></span>
                                    </li>
                                    <%--<li>
                                        <span class="note">快充时间: </span><span class="data"><%=fastChargeTimeRange%></span>
                                    </li>--%>
                                    <%= baoZhiLv %>
                                    <%}
                                        else
                                        { %>
                                    <li>
                                        <span class="note">排量: </span><% if (serialInfo.CsSaleState == "停销")
                                                                          { %>
                                        <span class="data<%=string.IsNullOrWhiteSpace(serialNoSaleDisplacement)?" grey-txt":"" %>" title="<%= serialNoSaleDisplacementalt %>"><%= string.IsNullOrWhiteSpace(serialNoSaleDisplacement) ? "暂无":serialNoSaleDisplacement %></span>
                                        <% }
                                            else
                                            { %>
                                        <span class="data<%=string.IsNullOrWhiteSpace(serialSaleDisplacementalt)?" grey-txt":"" %>" title="<%= serialSaleDisplacementalt %>"><%= string.IsNullOrWhiteSpace(serialSaleDisplacement)?"暂无":serialSaleDisplacement %></span>
                                        <% } %>
                                    </li>
                                    <li>
                                        <span class="note">变速箱: </span><span class="data<%=string.IsNullOrWhiteSpace(serialTransmission)?" grey-txt":"" %>"><%=string.IsNullOrWhiteSpace(serialTransmission) ? "暂无":serialTransmission%></span>
                                    </li>
                                    <%= baoZhiLv %>
                                    <li>
                                        <span class="note">油耗: </span>
                                        <% if (string.IsNullOrWhiteSpace(serialInfo.CsSummaryFuelCost))
                                            { %>
                                        <span class="data grey-txt">暂无</span>
                                        <%}
                                            else
                                            { %>
                                        <a class="data" data-channelid="2.21.855" target="_blank" href="http://car.bitauto.com/<%= serialSpell %>/youhao/"><%=serialInfo.CsSummaryFuelCost%> &gt;</a>
                                        <%} %>
                                    </li>
                                    <%} %>
                                </ul>
                            </div>
                        </li>
                    </ul>
                    <div class="mobile-qrcode">
                        <img src="/favicon.ico" id="qrcodelogo" style="display: none;" />
                        <a href="http://app.yiche.com/yiche/">
                            <img src="http://image.bitautoimg.com/cargroup/car/appdownloadv2.png" /></a>
                        <%--<a href="<%= wirelessSerialUrl %>?ref=pctowap" target="_blank" id="qrcode"><img src="http://image.bitautoimg.com/cargroup/car/qrimages/<%= serialId %>.png?v=1" /></a>--%>
                        <em>APP看车领福利</em>
                    </div>
                </div>

                <div class="row mid" id="mp-buttons">
                    <% if (serialInfo.CsSaleState != "停销")
                        { %>
                    <div class="col-xs-3" id="mp-jiangjia">
                        <div class="inner">
                            <span class="note">降价优惠</span>
                            <h5 class="desc"><a href="http://car.bitauto.com/<%= serialSpell %>/jiangjia/" data-channelid="2.21.1523" target="_blank">加载中...</a></h5>
                            <a class="btn btn-primary" target="_blank" data-channelid="2.21.98" href="http://dealer.bitauto.com/zuidijia/nb<%= serialId %>/?T=1&leads_source=p002001">询底价</a>
                        </div>
                    </div>
                    <div class="col-xs-3" id="mp-qianggou" style="display: none;">
                        <div class="inner">
                            <span class="note">厂商活动</span>
                            <%--<h5><a id="mp-qianggou-savemoney" target="_blank" data-channelid="2.21.1520" href="http://www.huimaiche.com/<%= serialSpell %>/?tracker_u=11_yccx&leads_source=p002002">加载中...</a></h5>
                            <a class="btn btn-default" target="_blank" data-channelid="2.21.99" href="http://www.huimaiche.com/<%= serialSpell %>/?tracker_u=11_yccx&leads_source=p002002">立即抢购</a>--%>
                        </div>
                    </div>
                    <div class="col-xs-3" id="mp-daikuan" downpayment="<%= serialMinPrice > 0 ? Math.Round(serialMinPrice*0.3,2) : 0 %>">
                        <div class="inner">
                            <span class="note">贷款</span>
                            <h5><a href="http://fenqi.taoche.com/www/<%= serialSpell %>/?from=yc9&leads_source=p002003" data-channelid="2.21.1521" target="_blank">加载中...</a></h5>
                            <%-- <h5 class="grey-txt">暂无</h5>--%>
                            <a class="btn btn-default" data-channelid="2.21.100" href="http://fenqi.taoche.com/www/<%= serialSpell %>/?from=yc9&leads_source=p002003" target="_blank">贷款</a>
                        </div>
                    </div>
                    <div class="col-xs-3" id="mp-ershouche">
                        <div class="inner">
                            <span class="note">二手车</span>
                            <h5 class=""><a id="mp-ershouche-minprice" href="http://www.taoche.com/<%= serialSpell %>/?ref=pc_yc_zs_gs_esc&leads_source=p002020" data-channelid="2.21.1522" target="_blank">加载中...</a></h5>
                            <a class="btn btn-default" href="http://www.taoche.com/<%= serialSpell %>/?ref=pc_yc_zs_gs_esc&leads_source=p002020" data-channelid="2.21.102" target="_blank">二手车</a>
                        </div>
                    </div>
                    <%}
                        else
                        { %>
                    <div class="col-xs-3" id="mp-ershouche">
                        <div class="inner">
                            <span class="note">二手车</span>
                            <h5><a id="mp-ershouche-minprice" href="http://yiche.taoche.com/<%= serialSpell %>/?ref=pc_yc_zs_gs_esc&leads_source=p002020" data-channelid="2.21.1522" target="_blank">&nbsp;</a></h5>
                            <a class="btn btn-primary" href="http://yiche.taoche.com/<%= serialSpell %>/?ref=pc_yc_zs_gs_esc&leads_source=p002020" data-channelid="2.21.102" target="_blank">买二手车</a>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="inner">
                            <span class="note">卖二手车</span>
                            <h5><a href="http://www.taoche.com/pinggu/?ref=chexizsgu" data-channelid="2.21.103" target="_blank">4种方式&gt;&gt;</a></h5>
                            <a class="btn btn-default" href="http://www.taoche.com/pinggu/?ref=chexizsgu" data-channelid="2.21.103" target="_blank">二手车估价</a>
                        </div>
                    </div>
                    <%} %>
                </div>
                <div class="bottom">
                    <div class="city-reduce-list" data-channelid="2.21.800">
                        <div class="row">
                            <span class="col-auto icon-rp">降价</span>
                            <ul class="list list-gapline sm col-auto" id="mp-jiangjiacity">
                            </ul>
                        </div>
                        <div class="cont">
                            <div class="list-txt list-txt-s list-txt-style">
                                <ul class="row" id="mp-jiangjianews">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="row bottom-list">
                        <%if (serialId != 3843)
                            { %>
                        <div class="col-xs-6">
                            <ul class="list">
                                <li>
                                    <a href="http://zhihuan.taoche.com/?ref=chexizshuan&leads_source=p002004&serial=<%= serialId %>" target="_blank" data-channelid="2.21.101"><i>置换</i>周期短 时间快</a>
                                </li>
                            </ul>
                        </div>
                        <% } %>
                        <div class="col-xs-6">
                            <ul class="list">
                                <li>
                                    <ins id="div_cda8ef3f-3747-4eee-afcc-77f5d7c253c2" data-type="ad_play_fs" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="cda8ef3f-3747-4eee-afcc-77f5d7c253c2"></ins>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <%-- <div class="row" id="mp-jiangjianews">
                        <%if (serialId != 3843)
                          { %>
                        <li>
                            <a href="http://zhihuan.taoche.com/?ref=chexizshuan&leads_source=p002004&serial=<%= serialId %>" target="_blank" data-channelid="2.21.101"><i>置换</i>周期短 时间快</a>
                        </li>
                        <% } %>
                        <li>
                            <ins id="div_cda8ef3f-3747-4eee-afcc-77f5d7c253c2" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="cda8ef3f-3747-4eee-afcc-77f5d7c253c2"></ins>
                        </li>
                        <li>
                            <a href="https://survey01.sojump.com/jq/11683995.aspx" target="_blank"><i>调查</i>易车邀请您参与</a>
                        </li>
                    </div>--%>
                </div>
            </div>
        </div>
        <!--/焦点图、名片区-->
        <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/cssummary.min.js?v=201707061432"></script>
        <%--<script type="text/javascript" src="/jsnewv2/cssummary.js?v=20170171032"></script>--%>
        <script type="text/javascript">
            var serialId = <%= serialId %> ,
                priceRang = '<%=serialPrice%>',
                cityId = 201,
                cityName = "北京";
            csSaleState = "<%= serialInfo.CsSaleState  %>",
                serialSpell = "<%= serialSpell  %>";
            if (typeof (bit_locationInfo) != "undefined") {
                cityId = bit_locationInfo.cityId;
                cityName = bit_locationInfo.cityName;
            }
            var GlobalSummaryConfig = {
                SerialId:<%= serialId %>,
                CityId: cityId,
                CityName: cityName
            };

            if (document.getElementById('carYearList_all'))
            { document.getElementById('carYearList_all').className = 'current'; }

            if (typeof (bitLoadScript) == "undefined") {
                bitLoadScript = function (url, callback, charset) {
                    var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
                    if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
                    else { s.onload = function () { if (callback) callback(); }; }
                    s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
                };
            }

            if (csSaleState != "停销") {
                GetDealerData("<%= serialSpell %>");
            }
            GetErShouCheMinPrice();
            GetDownPayment();
            InitTeHuiAndAdData();
            GetJiangjiaNews();
            GetHmcJiangJia();
            $("#qrcode img").bind("error", function () {
                this.style.display = "none";
                bitLoadScript("http://image.bitautoimg.com/carchannel/jsnewv2/jquery.qrcode.min.js", function () {
                    $('#qrcode').qrcode({ render: "canvas", size: 90, ecLevel: "H", mode: 4, image: $("#qrcodelogo")[0], text: $("#qrcode").attr("href") });
                }, "utf-8");
            });
            //通栏广告 显示
            function showTopLineAd(id, isAd) {
                if (isAd === true) {
                    $(".top-col6-190").show();
                }
            }
            bitLoadScript("http://img1.bitauto.com/bt/cmtad/advV1.js?v=20170330", function () {
                try {
                    AdvObject.GetAdvByCityIdAndSerialId(<%= serialId %>, cityId);
                } catch (e) { }
            }, "utf-8");
        </script>
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="article-section layout-1">
                        <div class="section-header header2" data-channelid="2.21.802">
                            <div class="box">
                                <h2><a href="/<%= serialSpell %>/wenzhang/" target="_blank"><%= serialShowName %>文章</a></h2>
                            </div>
                            <div class="more">
                                <a href="/<%= serialSpell %>/xinwen/" target="_blank">新闻</a>
                                <a href="/<%= serialSpell %>/shijia/" target="_blank">试驾</a>
                                <a href="/<%= serialSpell %>/daogou/" target="_blank">导购</a>
                                <a href="http://www.bitauto.com/zhuanti/daogou/gsqgl/" target="_blank">购车流程</a>
                                <a href="/<%= serialSpell %>/wenzhang/" target="_blank">更多&gt;&gt;</a>
                            </div>
                        </div>
                        <div class="row section-layout">
                            <!--车型详解、全系导购 开始-->
                            <%=carPingceHtml%>
                            <!--车型详解 结束-->
                            <!--新闻列表1 开始-->
                            <%=focusNewsHtml%>
                            <!--新闻列表1 结束-->
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-3">
                <div class="section-right">
                    <%if (!string.IsNullOrEmpty(koubeiReportHtml))
                        { %>
                    <%=koubeiReportHtml %>
                    <%}
                        else
                        { %>
                    <div class="layout-1">
                        <!--#include file="/include/pd/2014/koubei/00001/201703_PCzhan_chexingzongshuyezhanweitu_Manual.shtml"-->
                    </div>
                    <%} %>
                </div>
            </div>
        </div>
        <script type="text/javascript" src="http://gimg.bitauto.com/js/senseNewFs.js"></script>
        <%-- <script type="text/javascript">
            GetFocusNewsLast(csSaleState,8);
        </script>--%>
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="row col3-adv-1 layout-1" style="margin-top: -20px; margin-bottom: 30px;">
                        <div class="special-layout-3 ad-tag-box">
                            <cite class="ad-tag2" style="right: 5px; top: 5px;"></cite>
                            <ins id="div_084b9793-2721-4aa1-91d1-c61273417454" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="084b9793-2721-4aa1-91d1-c61273417454"></ins>
                        </div>
                        <div class="special-layout-3 ad-tag-box">
                            <cite class="ad-tag2" style="right: 5px; top: 5px;"></cite>
                            <ins id="div_96b0f212-b2c6-4ba9-8ab9-2acf88bd5f42" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="96b0f212-b2c6-4ba9-8ab9-2acf88bd5f42"></ins>
                        </div>
                    </div>

                    <div class="row col2-410-box layout-1 promote-section">
                        <div class="col-auto list-txt-layout1" id="cuxiao-news" data-channelid="2.21.1545">
                            <div class="section-header header2 mb0">
                                <div class="box">
                                    <h2><a href="http://car.bitauto.com/<%= serialSpell %>/baojia/" target="_blank"><%= serialShowName %>促销信息</a></h2>
                                </div>
                                <div class="more">
                                    <a href="http://car.bitauto.com/<%= serialSpell %>/baojia/" target="_blank">更多&gt;&gt;</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto list-txt-layout1" data-channelid="2.21.805">
                            <div class="section-header header2 mb0">
                                <div class="box">
                                    <h2><a href="<%=baaUrl %>" target="_blank">
                                        <% if (isHaveBaa)
                                            { %>
                                        <%=serialShowName %>社区</a>
                                        <%}
                                            else
                                            { %>
                                        <%= "社区精华推荐" %>
                                        <%} %>
                                        </a></h2>
                                </div>
                                <div class="more">
                                    <a href="<%=baaUrl %>" target="_blank">更多&gt;&gt;</a>
                                </div>
                            </div>
                            <% if (isHaveBaa)
                                { %>
                            <%= bbsNewsHtml %>
                            <%}
                                else
                                { %>
                            <div class="list-txt list-txt-m list-txt-default list-txt-style2 type-1">
                                <ul>
                                    <li class="no-link">
                                        <div class="txt">
                                            <a>暂无<%= serialShowName %>社区，为您推荐以下内容：</a>
                                        </div>
                                    </li>
                                    <%= bbsNewsHtml %>
                                </ul>
                            </div>
                            <%} %>
                        </div>
                    </div>
                    <%= serialHeaderJs%>
                    <div class="layout-1" style="margin-top: -20px; margin-bottom: 30px;">
                        <script type="text/javascript">
                            var pageCarLevel ='<%=serialEntity.Level.Name%>';
                            function showNewsInsCode(dxc, xxc, mpv, suv) {
                                var adBlockCode = xxc;
                                if (pageCarLevel == '中大型车' || pageCarLevel == '中型车' || pageCarLevel == '跑车' || pageCarLevel == '豪华车') {
                                    adBlockCode = dxc;
                                }
                                else if (pageCarLevel == '微型车' || pageCarLevel == '小型车' || pageCarLevel == '紧凑型车') {
                                    adBlockCode = xxc;
                                }
                                else if (pageCarLevel == '概念车' || pageCarLevel == 'MPV' || pageCarLevel == '面包车' || pageCarLevel == '皮卡' || pageCarLevel == '客车' || pageCarLevel == '卡车' || pageCarLevel == '其它') {
                                    adBlockCode = mpv;
                                }
                                else if (pageCarLevel == 'SUV') {
                                    adBlockCode = suv;
                                }
                                document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                            }
                            showNewsInsCode('d9f00215-7c63-47e6-9deb-f5d4c46c5132', 'f30ffee8-7792-4bf2-ba49-3f1a2eadbe3b', '62ee5eac-0d2b-4765-9226-9c60a2226948', 'd366c7e9-c0ec-408f-90a4-e71e6972d5b5');
                        </script>
                    </div>
                    <%= carListTableHtml %>

                    <div class="layout-1">
                        <%if (isElectrombile)
                            { %>
                        <ins id="div_2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "紧凑型车")
                            {%>
                        <ins id="div_fc718ffd-e00f-499c-be58-54f7dde02556" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="fc718ffd-e00f-499c-be58-54f7dde02556"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "面包车")
                            {%>
                        <ins id="div_9ff50cb9-76d8-4054-9aec-c46d38c07ae2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9ff50cb9-76d8-4054-9aec-c46d38c07ae2"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "SUV")
                            {%>
                        <ins id="div_ea1bb82e-5ed2-4531-b960-8d354d3a011f" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="ea1bb82e-5ed2-4531-b960-8d354d3a011f"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "中型车")
                            {%>
                        <ins id="div_1a1c2052-cf6f-4a94-970c-f2a4847a3e80" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="1a1c2052-cf6f-4a94-970c-f2a4847a3e80"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "微型车")
                            {%>
                        <ins id="div_d3734235-3fbe-47fc-99ea-57669cedadf5" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d3734235-3fbe-47fc-99ea-57669cedadf5"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "小型车")
                            {%>
                        <ins id="div_94042601-39b4-4161-892a-b0608e589e6e" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="94042601-39b4-4161-892a-b0608e589e6e"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "豪华车")
                            {%>
                        <ins id="div_569fe415-15f5-4ad2-9028-a49dce5186f9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="569fe415-15f5-4ad2-9028-a49dce5186f9"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "MPV")
                            {%>
                        <ins id="div_952ce0ee-507f-4f26-8f11-ce6c06812a49" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="952ce0ee-507f-4f26-8f11-ce6c06812a49"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "跑车")
                            {%>
                        <ins id="div_abe53914-1717-427b-9251-b207798ebdca" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="abe53914-1717-427b-9251-b207798ebdca"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "中大型车")
                            {%>
                        <ins id="div_be5865ff-b854-438d-9863-d54af38d21a7" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="be5865ff-b854-438d-9863-d54af38d21a7"></ins>
                        <%}
                            else if (serialEntity.Level.Name == "皮卡")
                            {%>
                        <ins id="div_2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe"></ins>
                        <%} %>
                    </div>

                    <% if (!string.IsNullOrWhiteSpace(photoListHtml))
                        { %>
                    <div class="layout-2 picture-section">
                        <%= photoListHtml %>
                        <div class="row special-layout-4" id="serialWaiGuanNeiShi">
                        </div>
                    </div>
                    <% } %>
                    <%= videosHtml %>

                    <div class="layout-1 key-report-section">

                        <div class="section-header header2 mb0">
                            <div class="box">
                                <h2><%= serialShowName %>关键报告</h2>
                            </div>
                        </div>

                        <%= serialSparkleHtml %>

                        <div class="special-layout-6">
                            <ul class="list">
                                <li class="li1">
                                    <span class="title" id="report-xiaoliang-month"></span>
                                    <h4 id="report-xiaoliang"></h4>
                                </li>
                                <li class="li2">
                                    <span class="title"><%= serialEntity.Level.Name %>周关注排行</span>
                                    <h4 <%= serialTotalPV =="暂无"?"class='none'":"" %>><%= serialTotalPV %></h4>
                                </li>
                                <li class="li3">
                                    <span class="title">安全碰撞</span>
                                    <h4 <%= CNCAPAndENCAPStr=="暂无" ? "class='none'":"" %>><%= CNCAPAndENCAPStr %></h4>
                                </li>
                                <%= awardHtml %>
                            </ul>
                        </div>

                        <%=hexinReportHtml%>


                        <!--评论-->
                        <%= editorCommentHtml %>
                        <!--/评论-->
                    </div>
                    <div style="margin-top: -20px; margin-bottom: 30px;">
                        <script type="text/javascript">
                            var pageCarLevel ='<%=serialEntity.Level.Name%>';
                            function showNewsInsCode(dxc, xxc, mpv, suv) {
                                var adBlockCode = xxc;
                                if (pageCarLevel == '中大型车' || pageCarLevel == '中型车' || pageCarLevel == '跑车' || pageCarLevel == '豪华车') {
                                    adBlockCode = dxc;
                                }
                                else if (pageCarLevel == '微型车' || pageCarLevel == '小型车' || pageCarLevel == '紧凑型车') {
                                    adBlockCode = xxc;
                                }
                                else if (pageCarLevel == '概念车' || pageCarLevel == 'MPV' || pageCarLevel == '面包车' || pageCarLevel == '皮卡' || pageCarLevel == '客车' || pageCarLevel == '卡车' || pageCarLevel == '其它') {
                                    adBlockCode = mpv;
                                }
                                else if (pageCarLevel == 'SUV') {
                                    adBlockCode = suv;
                                }
                                document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                            }
                            showNewsInsCode('037c7090-9b1c-4082-97c9-a05055f703c3', '18193574-4afa-48ec-b772-6bad17372728', '8d7e187c-6482-4a9d-b881-6f449db6a816', 'a54bb640-16c5-43e6-9c9a-0e7c508a8b20');
                        </script>
                    </div>
                    <%= koubeiDianpingHtml %>
                    <!--经销商开始-->
                    <script type="text/javascript">
                            document.write("<ins id=\"ep_union_137\" partner=\"1\" version=\"\" isupdate=\"1\" type=\"1\" city_type=\"1\" city_id=\"" + cityId + "\" city_name=\"0\" car_type=\"2\" brandid=\"0\" serialid=\"" + serialId + "\" carid=\"0\"></ins>");
                    </script>
                    <!--/经销商开始-->
                    <div class="layout-1">
                        <%--<div class="row special-layout-17" id="gouche-hmc" style="display: none;" data-channelid="2.21.104">
                        </div>--%>
                        <div class="row special-layout-17" id="gouche-ych" style="display: none;" data-channelid="2.21.106">
                        </div>
                        <%--  <div class="row special-layout-17" id="gouche-xscg" style="display: none;" data-channelid="2.21.995">
                        </div>--%>
                    </div>

                    <div class="layout-2 loan-section" id="gouche-chedai">
                        <div class="section-header header2 mb0">
                            <div class="box">
                                <h2><a target="_blank" href="http://fenqi.taoche.com/www/<%= serialSpell %>?from=yc36" data-channelid="2.21.1601"><%= serialShowName %>贷款方案</a></h2>
                            </div>
                        </div>
                        <div class="special-layout-5 type-1" id="gouche-huodong" data-channelid="2.21.1516">
                        </div>
                        <div class="special-layout-12" id="gouche-chedaicontent" data-channelid="2.21.820">
                        </div>
                        <div class="btn-box1">
                            <a class="btn btn-default" target="_blank" href="http://fenqi.taoche.com/www/<%= serialSpell %>?from=yc36" data-channelid="2.21.1601"><span class="more">更多贷款方案</span></a>
                        </div>
                    </div>

                    <div class="layout-1 oldcar-section" id="ucarlist">
                    </div>

                    <div class="layout-1">
                        <ins id="div_043ff6cd-b37a-4ca8-a25c-c81e8b05a94a" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="043ff6cd-b37a-4ca8-a25c-c81e8b05a94a"></ins>
                    </div>
                    <!-- 接下来看 -->
                    <uc1:NextToSee runat="server" ID="ucNextToSee" />
                    <!-- /接下来看 -->
                    <!-- SEO底部热门 -->
                    <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                    <!-- SEO底部热门 -->
                </div>
            </div>
            <div class="col-xs-3">
                <div class="section-right">
                    <div class="special-layout-3 sm layout-1 ad-tag-box" style="margin-top: -20px; margin-bottom: 30px;">
                        <cite class="ad-tag2" style="right: 5px; top: 5px;"></cite>
                        <ins id="div_2ed120ed-a613-4a43-84a2-3b7b5d0d8304" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="2ed120ed-a613-4a43-84a2-3b7b5d0d8304"></ins>
                    </div>
                    <div class="special-layout-13 layout-2" id="audio_list" data-channelid="2.21.1029" style="display: none;">
                        <h3>语音说车</h3>
                    </div>

                    <div class="layout-1 looking-sidebar" id="serialtosee_box" data-channelid="2.21.832">
                        <h3 class="top-title">看了还看</h3>
                        <div class="col2-140-box clearfix" id="serialtosee_content">
                        </div>
                    </div>

                    <%= CsHotCompareCars %>

                    <div class="layout-2 adv-sidebar">
                        <ins id="div_d3338d66-0526-48a5-a0da-9d3de90c28a7" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="d3338d66-0526-48a5-a0da-9d3de90c28a7"></ins>
                    </div>

                    <!--竞争车型-->
                    <%-- <%= competitiveKoubeiHtml %>--%>
                    <!--/竞争车型-->
                    <!--相关文章-->
                    <%= relatedNewsHtml %>
                    <!--/相关文章-->
                    <!--其他车型-->
                    <%= brandOtherSerial %>
                    <!--其他车型-->

                    <div class="layout-3 adv-sidebar">
                        <ins id="div_149e57b4-e495-40e1-ae1c-30f4b80d955e" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="149e57b4-e495-40e1-ae1c-30f4b80d955e"></ins>
                    </div>
                    <!--/经销商列表-->
                    <!--#include file="~/include/pd/2016/yipaicms/00001/201701_Summary_SCInfoList_Manual.shtml"-->
                    <%-- <div class="layout-1 adv-sidebar">
                        <img src="http://temp.im/300x310" alt="广告" />
                    </div>--%>
                    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/common.min.js"></script>
                    <!--#include file="~/include/pd/2016/00001/20170118_zongshu_likewhatcar_Manual.shtml"-->
                </div>
            </div>
        </div>
    </div>
    <!--页底浮层广告-->
    <ins id="div_c62213b4-2900-4ed8-967d-3f3866014dc5" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%= serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="c62213b4-2900-4ed8-967d-3f3866014dc5"></ins>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/cssummaryrest.min.js?v=20170906"></script>
    <%--<script type="text/javascript" src="/jsnewv2/cssummaryrest.js?v=20161230"></script>--%>
    <script type="text/javascript">
                        var CarCommonBSID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.MasterBrandId %>"; //大数据组统计用
                        var CarCommonCBID = "<%= serialEntity.Brand == null ? 0 : serialEntity.Brand.Id %>";
        var CarCommonCSID = "<%=serialId %>";
        var CarFilterData = <%=string.IsNullOrEmpty(carListFilterData)?"null":carListFilterData %>;
        (function () {
            GetPromotionNews();
            GetSerialWaiGuanNeiShi();
            GetCheDai();
            GetXiaoliang();
            //getBuyLimit();
            GetVedioNum();
            //searchEscByTypeRequest("<%=serialSpell %>");
            searchEsc("<%=serialSpell %>");
        })();
        //焦点颜色图src设置
        $("div[id^='focuscolor_'] img").each(function (i, n) {
            $(n).attr("src", $(n).data("original"));
        });
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/circle-progress.min.js?v=1"></script>
    <script type="text/javascript">
        //$(function(){
        /*判断浏览器是否支持canvas*/
        if (typeof ($("<canvas>")[0].getContext) != "undefined") {            var circleProgressJson = {
                value: 0.7399,
                size: 220,
                thickness: 10,
                reverse: false,
                lineCap: "round",
                emptyFill: "#fff",
                startAngle: -3.9,
                fill: { gradient: ["#7BD501", "#C5EA43", "#FFB451", "#FF4F53"] }
            };
            $("#circleProgress-youhao") && $("#circleProgress-youhao").circleProgress(circleProgressJson);
            $("#circleProgress-dongli") && $("#circleProgress-dongli").circleProgress(circleProgressJson);
            $("#circleProgress-zhidong") && $("#circleProgress-zhidong").circleProgress(circleProgressJson);
            //口碑报告
            $('#circleProgress-report') && $('#circleProgress-report').circleProgress({
                value: $('#circleProgress-report').attr("value"),
                size: 90,
                thickness: 4,
                reverse: false,
                lineCap: "round",
                startAngle: 5,
                emptyFill: "#e1e1e1",
                fill: {
                    color: "#4284d9"
                }
            });        }        //});        bitLoadScript("http://image.bitautoimg.com/carchannel/jsnewv2/jquery.lazyload.min.js?v=20151026",
            function () {
                if ($(".picture-section img").length > 0) {
                    $(".picture-section img").lazyload({
                        placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
                        threshold: 50
                    });
                }
                if ($("#car-videobox img").length > 0) {
                    $("#car-videobox img").lazyload({
                        placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
                        threshold: 50
                    });
                }
            }, "utf-8");    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/newsViewCount.min.js?v=20161228"></script>
    <script type="text/javascript" charset="utf-8" src="http://img4.bitautoimg.com/uimg/car/js/tabs_20140512_4.js"></script>
    <script type="text/javascript">
        var loadPingce = function () {
            var pingceurl ="http://pingce.bitauto.com"
            $.ajax({
                type: "GET",
                url: "http://api.car.bitauto.com/Assessment/GetEntranceData.ashx?csid=<%= serialId %>",
                dataType: "jsonp",
                jsonpCallback: "success_jsonpCallback_getEntranceData",
                success: function (data) {
                    if (data.EvaluationId > 0) {
                        var htmlArr = [];
                        htmlArr.push("    <div class='col-auto left'>");
                        htmlArr.push("        <a data-channelid='2.21.2152' target='_blank' href='" + pingceurl+"/details/" + data.EvaluationId + "-2.html'>");
                        htmlArr.push("            <div class='figure'>");
                        htmlArr.push("                <img src='" + data.ImageUrl + "' alt=''>");
                        htmlArr.push("            </div>");
                        htmlArr.push("            <div class='caption'>");
                        htmlArr.push("                <span class='title'>总体得分</span>");
                        htmlArr.push("                <h4>" + data.TotalScore + "<i> 分</i></h4>");
                        htmlArr.push("            </div>");
                        htmlArr.push("        </a>");
                        htmlArr.push("        <div class='bottom'>");
                        //htmlArr.push("            <a target='_blank' href=''>同级别排名第128位&gt;&gt;</a>");
                        htmlArr.push("        </div>");
                        htmlArr.push("    </div>");
                        htmlArr.push("    <div class='col-auto right'>");
                        htmlArr.push("        <span class='t-flag'>超级评测</span>");
                        htmlArr.push("        <h3><a data-channelid='2.21.2153' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-2.html'>" + unescape(data.CarName.replace(/\\/g, "%")) + "</a></h3>");
                        htmlArr.push("        <div class='info'><span class='view'> </span><span class='comment'> </span>");
                        htmlArr.push("        </div>");
                        htmlArr.push("        <ul class='list list-gapline sm'>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2154' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-2.html'>空间 " + data.BodyAndSpaceGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2155' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-4.html'>舒适 " + data.RidingComfortGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2156' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-5.html'>动力 " + data.DynamicPerformanceGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2157' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-6.html'>驾驶 " + data.JsBaseGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2158' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-3.html'>安全 " + data.SafetyGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2159' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-7.html'>油耗 " + data.YhBaseGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2160' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-8.html'>费用 " + data.CostBaseGroupScore + "分</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("            <li>");
                        htmlArr.push("                <a data-channelid='2.21.2161' target='_blank' href='" + pingceurl + "/details/" + data.EvaluationId + "-2.html'>更多&gt;&gt;</a>");
                        htmlArr.push("            </li>");
                        htmlArr.push("        </ul>");
                        htmlArr.push("        <div class='bottom'>");
                        htmlArr.push("            易车出品");
                        htmlArr.push("        </div>");
                        htmlArr.push("    </div>");                    
                        $("#pingce_left_top").addClass("super-test").html(htmlArr.join(""));
                    }
                },
                complete: function () {

                }
            });
        }
        loadPingce();
    </script>

    <!--看了还看js-->
    <script type="text/javascript">
        <%= serialToSeeJson %>
</script>
    <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/serialtoseead.min.js"></script>
    <!--/看了还看js-->
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/360pano/vrImgForBitauto.js"></script>
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
    <%if (serialEntity.Level != null && serialEntity.Level.Name != "概念车")
        {%>
    <!-- baa 浏览过的车型-->
    <script type="text/javascript">
        //登录 车型关注
        try {
            Bitauto.Login.onLoginedHandlers.push(function () {
                Bitauto.UserCars.getConcernedCars(function (obj) {
                    var concernedcar = Bitauto.UserCars.concernedcar.arrconcernedcar;
                    for (var i = 0; i < concernedcar.length; i++) {
                        if (concernedcar[i] == serialId) { $("#favstar").attr("title", "取消关注").addClass("focused").html("已关注"); break; }
                    }
                });
            });
        } catch (e) { }
        //关注
        $("#favstar").bind("click", function () {
            FocusCar($(this));
        });
        $("#a-focus-close,#btn-focus-close").click(function () { $("#FocusCarFull").hide(); });

        try {
            Bitauto.UserCars.addViewedCars('<%=serialId.ToString() %>');
        }
        catch (err)
        { }
        //vr
        if (vrImgForBitauto != undefined && vrImgForBitauto.IntiDataForEntry != undefined) {
            vrImgForBitauto.IntiDataForEntry(<%=serialId%>, function (vrImgs) {
                if (vrImgs.length > 0) {
                    $(".l-box-sty.col-auto").prepend("<a target=\"_blank\" href=\"" + vrImgs[0].PanoUrl + "\" class=\"zs-vr\">VR看全景</a>");
                }
            });
        }
    </script>
    <%} %>
    <!--/经销商弹层-->
    <!--#include file="~/include/pd/2016/yipaicms/00001/201701_Summary_SCInfoPopup_Manual.shtml"-->
    <!--#include file="~/htmlv2/rightbar.shtml"-->
    <%--<script type="text/javascript">
        (function(global) {
            var config =global.SidebarConfig||{};
            config.activate=false;
            global.SidebarConfig = config;
        })(window);
    </script>--%>
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <!--本站统计代码-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript">
        OldPVStatistic.ID1 = "<%= serialId %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!--本站统计结束-->
    <!--音频 start -->
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=audio/jplayer/jquery.jplayer.min.js,audio/jplayer/audioJPlayerMobile.js?v=20160905"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/jquery.jplayer.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/audioJPlayerMobile.js?v=20160905"></script>
    <script type="text/lazyT-template" id="template_audios">
        {#~ D:item:index #} 
            <div class="inner-box jp-audio" id="audio_{#=item.AudioId#}" role="application" aria-label="media player">
				<div class="jp-jplayer" data-urls="{#=item.PlayLinks.join(',')#}" data-id="audio_{#=item.AudioId#}"></div>
                <a href="javascript:" class="figure jp-play" role="button">
                    <span class="img">
                        <img src="{#=item.AuthorImage#}" alt="" width="60" height="60">
                        <!--切换play和pause-->
                        <i class="pause" ></i>
                    </span>
                </a>
                <h5>{#=item.AuthorName#} <em>{#=item.Introduce#}</em></h5> 
                <div class="indicator">
                    <span class="data jp-play-bar" style="width: 0%;"></span>
                    <span class="time jp-current-time" role="timer" aria-label="time">00:00</span>
					<span class="time jp-duration" role="timer" aria-label="duration">{#=item.DurationString#}</span>
                    <span class="note popup-control-box">
                        <div class="popup-layout-1">
                            <p>{#=item.Content#}
                            </p>
                        </div>
                    </span>
                </div>
            </div>
        {#~#}
    </script>
    <script type="text/javascript">
        $(function () {
            var audioCreator;
            $.getJSON("http://api.admin.bitauto.com/audiobase/audio/GetAudioAudioResource?IsCarModel=1&serialId=" + <%= serialId %>, function (data) {
                if (data.length > 0) {
                    $("#audio_list").show();
                    var audioTemplate = document.getElementById('template_audios').innerHTML;
                    audioCreator = lazyT.tmpl(audioTemplate);
                    var audioHtml = audioCreator(data);
                    $('#audio_list').append(audioHtml);
                    jp.init({ swfPath: "/jsnewv2/audio/", playCssSelect: "a.jp-play" });
                }
            });
        });    </script>
    <!--音频 end -->
    <script type="text/javascript">
        var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
            modelStr ='<%=serialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
    <!--关注弹出提示层-->
    <div class="modal modal-default" id="FocusCarFull" style="position: fixed; display: block; left: 50%; top: 50%; margin-top: -160px; margin-left: -275px; display: none;">
        <div class="modal-header">
            <h4>管理关注的车</h4>
            <span class="close" id="btn-focus-close"></span>
        </div>
        <div class="modal-content">
            <div class="note-box note-empty type-1">
                <div class="ico"></div>
                <div class="info">
                    <h3>不能再继续添加了...</h3>
                    <p class="tip">您最多可以添加 9 辆关注车型，去车库删除一部分车型后就可以继续添加了。</p>
                </div>
                <div class="action">
                    <!--按钮间不留空格-->
                    <a href="javascript:;" class="btn btn-primary2" id="a-focus-close">我知道了</a><a href="#" id="mangerCar_tc" class="btn btn-default">管理关注的车</a>
                </div>
            </div>
        </div>
    </div>
    <!--/关注弹出提示层-->

    <script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
    <%if (serialId == 2921 || serialId == 3999 || serialId == 2593 || serialId == 2382 || serialId == 2573 || serialId == 2837 || serialId == 2070 || serialId == 3395 || serialId == 2855 || serialId == 1994)
        { %>
    <script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?7b86db06beda666182190f07e1af98e3";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>

    <%} %>
</body>
</html>
