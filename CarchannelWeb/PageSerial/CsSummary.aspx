<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsSummary" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=serialSeoName %>】最新<%=serialSeoName %>报价_参数_图片_<%=serialEntity.Brand.MasterBrand.Name+serialName %>论坛-易车网</title>
    <meta name="Keywords" content="<%=serialSeoName%>,<%= serialSeoName%>报价,<%= serialSeoName%>价格,<%= serialSeoName%>参数,<%= serialSeoName%>论坛,易车网,car.bitauto.com" />
    <meta name="Description" content="<%= serialEntity.Brand.MasterBrand.Name + serialName%>,易车提供全国官方4S店<%= serialSeoName%>报价,最新<%= serialEntity.Brand.MasterBrand.Name + serialName%>降价优惠信息。以及<%= serialSeoName%>报价,<%= serialSeoName%>图片,<%= serialSeoName%>在线询价服务,低价买车尽在易车网 " />
    <meta http-equiv="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=serialSpell %>/" />
    <link rel="alternate" media="only screen and (max-width: 640px)" href=" http://car.m.yiche.com/<%=serialSpell %>/">
    <link rel="canonical" href="http://car.bitauto.com/<%=serialSpell %>/" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_carsummary-724.shtml"-->
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <div class="bt_pageBox">
        <!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
    </div>
    <!--书角广告代码-->
    <ins id="div_0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a" type="ad_play" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a"></ins>
    <!-- 顶通新增收起广告(2596锐界) -->
    <ins id="div_2e7e5549-b9d7-4fb3-bec7-9f7b39efe715" type="ad_play" adplay_ip="" adplay_areaname=""
        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
        adplay_blockcode="2e7e5549-b9d7-4fb3-bec7-9f7b39efe715"></ins>
    <!-- 顶通新增收起广告(2596锐界)-->
    <div class="bt_ad">
        <ins id="div_2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02" type="ad_play" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
            adplay_blockcode="2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02"></ins>
    </div>
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_xdh_Manual.shtml"-->
    <div class="bt_ad">
        <%=serialTopAdCode%>
    </div>
    <!--header start-->
    <%= serialHeaderHtml%>
    <!--header end-->
    <div class="bt_page">
        <div class="col-all">
            <!-- Dec.6.2011 new AD -->
            <!-- baidu-tc begin {"action":"DELETE"} -->
            <ins id="div_d8728661-13a7-4a8c-8287-71f7e6d605c2" type="ad_play" adplay_ip="" adplay_areaname=""
                adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                adplay_blockcode="d8728661-13a7-4a8c-8287-71f7e6d605c2"></ins>
            <!-- baidu-tc end -->
            <!--首屏部分左侧 开始-->
            <div class="col-con shouping_con">
                <div class="line_box shouping_box line_box_top_b line-box_t0">
                    <div class="card-head-box clearfix card-head-box_t0">
                        <div class="l-box-sty">
                            <!--焦点图开始-->
                            <%=focusImagesHtml%>
                            <!--焦点图结束-->
                        </div>
                        <div class="txt-box zs-m-card">
                            <%if (serialPrice == "未上市" || serialPrice == "暂无报价" || serialPrice == "停售")
                              {%>
                            <p class="p-tit no-car">
                                <%=serialPrice%>
                                <%}
                              else
                              {%>
                            <p class="p-tit">
                                全国参考价：<strong><a href="/<%=serialSpell %>/baojia/" data-channelid="2.21.853" target="_blank"><%=serialPrice%></a></strong>
                                <%} %>
                                <a id="btnDirectSell" style="display: none;" href="#" data-channelid="2.21.1042" target="_blank" class="ico-shangchengtehui">直销</a> <a id="favstar" href="javascript:;" title="点击关注" data-channelid="2.21.799" class="shoucang"><span class="no-sc"></span>
                                </a><a href="javascript:;" class="fenxiang" id="fenxiang"><span></span></a>
                            </p>
                            <div class="tanchen" id="shareContent" style="display: none;">
                                <div class="news_fenxiang">
                                    <div class="bdsharebuttonbox fenxiang_box bdshare-button-style0-16" data-bd-bind="1435891808586">
                                        <a data-cmd="qzone" class="bds_qzone" href="javascript:;" title="分享到QQ空间">QQ空间</a>
                                        <a data-cmd="tsina" class="bds_tsina" href="javascript:;" title="分享到新浪微博">新浪微博</a>
                                        <a data-cmd="tqq" class="bds_tqq" href="javascript:;" title="分享到腾讯微博">腾讯微博</a>
                                        <a data-cmd="renren" class="bds_renren" href="javascript:;" title="分享到人人网">人人网</a>
                                        <a data-cmd="douban" class="bds_douban" href="javascript:;" title="分享到豆瓣">豆瓣</a>
                                    </div>
                                    <div class="fenxiang_box_big">
                                        <div class="bdsharebuttonbox bdshare-button-style0-16" data-bd-bind="1435891808586">
                                            <a class="bds_weixin" href="javascript:;" title="分享到微信">微信</a>
                                        </div>
                                        <div class="fenxiang_box_img" id="qrcode"></div>
                                        <div class="fenxiang_box_p">
                                            <p>
                                                微信扫一扫，将更全的车型信息分享给朋友，<a href="http://www.bitauto.com/zhuanti/other/2015weixinhelp/" target="_blank">不会用？&gt;&gt;</a>
                                            </p>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <ul>
                                <li><i>厂商指导价：</i><span id="guidPrice"><%=serialInfo.CsSaleState == "停销"?noSaleLastReferPrice:serialEntity.ReferPrice%></span> </li>
                                <li class="current"><i class="i-w">二手车：</i><span><%if (string.IsNullOrEmpty(serialUCarPrice))
                                                                                   { %>
										暂无报价
										<%}
                                                                                   else
                                                                                   { %>
                                    <a href="http://www.taoche.com/<%=serialSpell %>/" data-channelid="2.21.854" target="_blank">
                                        <%=serialUCarPrice%></a>
                                    <%} %></span> </li>
                                <% if (isElectrombile)
                                   {%>
                                <li><i>充电时间：</i><span><%=chargeTimeRange%></span> </li>
                                <li class="current"><i>快充时间：</i><span><%=fastChargeTimeRange%></span> </li>
                                <li><i>续航里程：</i><span><%=mileageRange%></span> </li>
                                <%}
                                   else
                                   {%>
                                <li><i>排&nbsp;&nbsp;&nbsp;&nbsp;量：</i><% if (serialInfo.CsSaleState == "停销")
                                                                         { %>
                                    <span title="<%= serialNoSaleDisplacementalt %>"><%= serialNoSaleDisplacement %></span>
                                    <% }
                                                                         else
                                                                         { %>
                                    <span title="<%= serialSaleDisplacementalt %>"><%= serialSaleDisplacement %></span>
                                    <% } %>
                                </li>
                                <li class="current"><i class="i-w">油&nbsp;&nbsp;&nbsp;&nbsp;耗：</i><span><a href="/<%=serialSpell %>/youhao/"
                                    target="_blank" data-channelid="2.21.855"><%=serialInfo.CsSummaryFuelCost%></a></span> </li>
                                <li><i>变速箱：</i><span><%=serialTransmission%></span> </li>
                                <%} %>
                                <li class="current"><i class="i-w">保&nbsp;&nbsp;&nbsp;&nbsp;修：</i><span><%=serialInfo.SerialRepairPolicy%></span>
                                </li>
                            </ul>
                            <div class="zh-btn-box">
                                <span class="button_orange btn-f-set"><a rel="nofollow" id="cardXunjia" data-channelid="2.21.98" href="http://dealer.bitauto.com/zuidijia/nb<%=serialId %>/?T=1&leads_source=p002001<%=zampdaXunjiaSourceId %>" target="_blank">询底价</a> </span>
                                <span id="divDemandCsBut" class="button_gray" data-channelid="2.21.105" style="display: none;"><a class="" href="#">特卖</a></span>

                                <%=shijiaOrHuimaiche %>

                                <span class="button_gray btn-f-set"><a rel="nofollow" id="cardCheDai" href="<%=chedaiADLink %>" data-channelid="2.21.100" class="dk-l" target="_blank">贷款</a> </span>

                                <%if (serialId != 3843)
                                  {%>
                                <span class="button_gray btn-f-set" id="btnZhihuan"><a rel="nofollow" href="http://zhihuan.taoche.com/?ref=chexizshuan&leads_source=p002004&serial=<%=serialId %>" data-channelid="2.21.101" target="_blank">置换</a> </span>
                                <%} %>

                                <span class="button_gray btn-f-set last"><a rel="nofollow" href="http://www.taoche.com/<%=serialSpell%>/?ref=chexizsmai&leads_source=p002020" data-channelid="2.21.102" target="_blank">二手车</a> </span>
                            </div>

                            <!--降价列表 start-->
                            <div id="jiangjia_box" class="jiangjia_box" data-channelid="2.21.800"></div>
                            <div class="wz-gg-box">
                                <!--广告开始-->
                                <ins id="Ins1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                                    adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="ab5d1cc0-0115-4e6b-b8fb-e8f68ed85efd"></ins>
                                <!--广告结束-->
                                <div class="diaocha-box">
                                    <a href="https://survey01.sojump.com/jq/10861494.aspx" target="_blank">满意度有奖小调查</a>
                                </div>
                            </div>
                            <!--降价列表 end-->
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div data-channelid="2.21.104" class="gouche-wrap" id="gouche-hmc" style="display: none;"></div>
                <div data-channelid="2.21.106" class="gouche-wrap" id="gouche-ych" style="display: none;"></div>
                <div data-channelid="2.21.995" class="gouche-wrap" id="gouche-xscg" style="display: none;"></div>
                <div class="line_box news_box">
                    <%--<!-- 购车服务 开始-->
					<div id="gouche-container" class="line_box" style="display: none">
						<div class="title-con" data-channelid="2.21.801">
							<div class="title-box" id="gouche-container_tgou">
								<h3><a href="http://gouche.yiche.com/sb<%=serialId %>/" target="_blank"><%=serialSeoName %>优惠购车</a></h3>

							</div>
						</div>
						<div class="gouche_box_new" id="gouche-box">
						</div>
					</div>
					<!-- 购车服务 结束-->--%>

                    <div class="title-con" data-channelid="2.21.802">
                        <div class="title-box">
                            <%=titleAndCNCAPHtml%>
                            <%=newsTagsHtml%>
                        </div>
                    </div>
                    <!--左侧开始-->
                    <div class="col-sub" data-channelid="2.21.803">
                        <!--车型详解、全系导购 开始-->
                        <%=carPingceHtml%>
                        <!--车型详解 结束-->
                    </div>
                    <!--左侧结束-->
                    <!--中间开始-->
                    <div class="col-main" data-channelid="2.21.804">
                        <!--新闻列表1 开始-->
                        <%=focusNewsHtml%>
                        <!--新闻列表1 结束-->
                    </div>
                    <!--中间结束-->
                    <div class="clear">
                    </div>
                </div>
                <!-- 音频 start  -->
                <div class="audio-box" style="display: none">
                    <div id="scrollBox">
                        <ul id="audio_list"></ul>
                    </div>
                    <!-- 翻页按钮 start-->
                    <div class="v-page">
                        <a href="###" id="focus_left" class="prev-btn">上一条</a>
                        <a href="###" id="focus_right" class="next-btn">下一条</a>
                    </div>
                    <!-- 翻页按钮 end-->
                </div>
                <!-- 音频 end  -->

                <div class="line_box news_box" data-channelid="2.21.805">
                    <div class="title-con">
                        <div class="title-box">
                            <h3>
                                <a href="<%=baaUrl %>" target="_blank">
                                    <%=serialSeoName %>论坛</a>
                            </h3>
                            <%=bbsMoreHtml%>
                        </div>
                    </div>
                    <%=bbsNewsHtml%>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <!--首屏部分左侧 结束-->
            <!--首屏部分右侧开始-->
            <div class="col-side">
                <!-- baidu-tc begin {"action":"DELETE"} -->
                <!-- ad -->
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e"></ins>
                </div>
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891"></ins>
                </div>
                <!--易湃-->
                <div class="col-side_ad" id="cmtadDiv">
                </div>
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
                </div>
                <!-- New AD Dec.20.2011 
                <div class="col-side_ad">
                    <ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db"></ins>
                </div>-->
                <div class="col-side_ad">
                    <ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853"></ins>
                </div>
                <%--<div class="col-side_ad">
					<ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
						adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
				</div>--%>
                <!-- 奖项 -->
                <%=awardHtml%>
                <!-- baidu-tc end -->
                <%=koubeiImpressionHtml%>
                <!-- 竞品口碑 -->
                <%=CompetitiveKoubeiHtml%>
                <!--特价车 开始-->
                <div class="tejiache_box2" style="display: none;" id="tejiache_box">
                </div>
                <!--特价车 结束-->
                <!--看了还看 开始-->
                <div class="line-box" id="serialtosee_box">
                    <div class="side_title">
                        <h4>看过此车的人还看
                        </h4>
                    </div>
                    <ul class="pic_list">
                    </ul>
                </div>
                <!--看了还看 结束-->
                <!--对比 开始-->
                <%=hotSerialCompareHtml%>
                <!--对比 结束-->
            </div>
            <!--右首屏部分侧结束-->
            <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
           <%-- <script type="text/javascript" src="/jsnew/city348idmapname.js"></script>
			<script type="text/javascript" src="/jsnew/serialexternalcommon.js"></script>--%>
            <script type="text/javascript">
                //添加统计链接末尾参数
                var tongJiEndUrlParam='&ref=car3&rfpa_tracker=1_8';

                if (typeof (bitLoadScript) == "undefined"){
                    bitLoadScript = function (url, callback, charset) {
                        var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
                        if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
                        else { s.onload = function () { if (callback) callback(); }; }
                        s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
                    };
                }
                bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/city348idmapname.js,carchannel/jsnew/serialexternalcommon.min.js?v=20161108",function () {
                    //getDemandAndJiangJia(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId);
                    //getDirectSell(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId,'yc-cxzs-nav-1');
                    getMallPartCar(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId,'yc-cxzs-nav-1');

                    getBuyHmc(<%=serialId %>, bit_locationInfo.cityId);
                    getBuyYch(<%=serialId %>, bit_locationInfo.cityId);
                    getBuyLimit(<%=serialId %>, bit_locationInfo.cityId);
                }, "utf-8");  
                bitLoadScript("http://img1.bitauto.com/bt/cmtad/adv.js?v=201412", function () {
                    try{ AdvObject.GetAdvByCityIdAndSerialId(<%=serialId%>);}catch(e){}
				}, "utf-8");
				 
				//今日头条
				var global_hash = window.location.hash;
				$(function(){
				    if(global_hash=="#jrtt")
				    {
				        $(".bit_top990,.bt_ad,.nav_small,.header_style,.foot_box").hide();
				        $("ins[id!='ep_union_123']").remove();
				    }
				});
            </script>
            <script type="text/lazyT-template" id="template_audios">
                {#~ D:item:index #}
                <li>
                    <div id="audio_{#=item.AudioId#}" class="jp-audio  audio_{#=item.AudioId#}" role="application" aria-label="media player">
                        <div class="audio-list">
                            <!-- 播放器 start -->
                            <div class="play-box">
                                <div class="jp-type-single">
                                    <div class="jp-controls">
                                        <img src="{#=item.AuthorImage#}" alt="" />
                                        <span></span>
                                        <div class="jp-zz"></div>
                                         <button class="jp-play" data-channelid="2.21.1029" style="cursor:pointer" role="button" data-id="audio_{#=item.AudioId#}" data-urls="{#=item.PlayLinks.join(',')#}" tabindex="0"></button>
                                    </div>
                                    <div class="jp-cont-warp">
                                        <div class="jp-progress">
                                            <div class="jp-seek-bar">
                                                <div class="jp-play-bar"></div>
                                            </div>
                                        </div>
                                        <div class="jp-time-holder">
                                            <div class="jp-current-time" role="timer" aria-label="time">{#=item.DurationString#}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- 播放器 end -->
                            <!-- 文字介绍 start -->
                            <div class="aud-txt-box">
                                <div class="prog-txt">
                                    <i class="t"></i>
                                    <i class="b"></i>
                                    <p>{#=item.Content#}</p>
                                </div>
                                <div class="source-box">
                                    <span class="name-box">{#=item.AuthorName#} - {#=item.Introduce#}</span><span class="time-box jp-current-time"></span>
                                </div>
                            </div>
                            <!-- 文字介绍 end -->
                        </div>
                    </div>
                </li>
                {#~#}
            </script>
            <!--2屏部分左侧 开始-->
            <div class="col-con">
                <script type="text/javascript">
                    var pageCarLevel='<%=serialEntity.Level.Name%>';
                    var adBlockCode = '62ee5eac-0d2b-4765-9226-9c60a2226948';

                    if (pageCarLevel == '中大型车' || pageCarLevel == '中型车' || pageCarLevel == '跑车' || pageCarLevel == '豪华车') {
                        adBlockCode = 'd9f00215-7c63-47e6-9deb-f5d4c46c5132';
                    }
                    else if (pageCarLevel == '微型车' || pageCarLevel == '小型车' || pageCarLevel == '紧凑型车') {
                        adBlockCode = 'f30ffee8-7792-4bf2-ba49-3f1a2eadbe3b';
                    }
                    else if (pageCarLevel == '概念车' || pageCarLevel == 'MPV' || pageCarLevel == '面包车' || pageCarLevel == '皮卡'|| pageCarLevel == '客车' || pageCarLevel == '卡车' || pageCarLevel == '其它') {
                        adBlockCode = '62ee5eac-0d2b-4765-9226-9c60a2226948';
                    }
                    else if (pageCarLevel == 'SUV') {
                        adBlockCode = 'd366c7e9-c0ec-408f-90a4-e71e6972d5b5';
                    } 
                    document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                </script>
                <%--<!-- baidu-tc begin {"action":"DELETE"} -->
                <div class="summaryMiddleAD ad_720">
                    <ins id="middleADForCar" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="37940534-3acb-4358-8f99-ac9abc6624ca"></ins>
                </div>
                <!-- 单一车型页（城市）/中部通栏 -->
                <div class="summaryMiddleAD ad_720">
                    <ins id="div_fefd085a-31d6-44f3-9ba0-e75930818d3f" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="fefd085a-31d6-44f3-9ba0-e75930818d3f"></ins>
                </div>
                <!-- baidu-tc end -->--%>
                <!--2屏部分左侧 结束-->

                <%if (isExistCarList)
                  { %>
                <div class="line_box" id="car_list" style="z-index: 10;">
                    <%=carListTableHtml%>
                    <!-- 筛选层 start -->
                    <div class="car-compare-filter" id="carCompareFilter" data-channelid="2.21.807" style="display: none;">
                        <div id="carFilterBox">
                            <div class="btn-filter" id="btnFilterShow">
                                筛选<em></em>
                            </div>
                            <div class="filter-list" id="filter-list">
                            </div>
                        </div>
                        <div class="btn-filter btn-filter-hide" style="display: none" id="btnFilterHide">
                            筛选<em></em>
                        </div>
                    </div>
                    <!-- 筛选层 end -->
                </div>
                <%} %>
                <%=photoListHtml%>
                <%=videosHtml%>
                <%=hexinReportHtml%>
                <%=editorCommentHtml%>
                <script type="text/javascript">
                    function showNewsInsCode(dxc, xxc, mpv, suv) {
                        var adBlockCode = xxc;
                        if (pageCarLevel == '中大型车' || pageCarLevel == '中型车' || pageCarLevel == '跑车' || pageCarLevel == '豪华车') {
                            adBlockCode = dxc;
                        }
                        else if (pageCarLevel == '微型车' || pageCarLevel == '小型车' || pageCarLevel == '紧凑型车') {
                            adBlockCode = xxc;
                        }
                        else if (pageCarLevel == '概念车' || pageCarLevel == 'MPV' || pageCarLevel == '面包车' || pageCarLevel == '皮卡'|| pageCarLevel == '客车' || pageCarLevel == '卡车'|| pageCarLevel == '其它') {
                            adBlockCode = mpv;
                        }
                        else if (pageCarLevel == 'SUV') {
                            adBlockCode = suv;
                        }
                        document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                    }
                    showNewsInsCode('037c7090-9b1c-4082-97c9-a05055f703c3', '18193574-4afa-48ec-b772-6bad17372728', '8d7e187c-6482-4a9d-b881-6f449db6a816', 'a54bb640-16c5-43e6-9c9a-0e7c508a8b20');
                </script>
                <%=koubeiDianpingHtml%>
                <!--#include file="~/ushtml/0000/yiche_2014_cube_carsummary_2015hotcar_style-891.shtml"-->
                <!--ad 2014.12.30-->
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
                <!-- 经销商 -->
                <div class="line_box" id="vendorInfo" data-channelid="2.21.819">
                    <div class="title-box">
                        <h3>
                            <a href="http://dealer.bitauto.com/<%= serialSpell %>/" target="_blank">
                                <%=serialSeoName%>经销商推荐</a></h3>
                        <div class="more">
                            <a target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">更多&gt;&gt;</a>
                        </div>
                    </div>
                    <%--<ins id="ep_union_4" partner="1" version="" isupdate="1" type="1" city_type="-1"
						city_id="0" city_name="0" car_type="2" brandid="0" serialid="<%= serialId %>"
						carid="0"></ins>--%>
                    <script type="text/javascript">
                        document.write('<ins Id=\"ep_union_123\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\"'+bit_locationInfo.cityId+'\" city_name=\"0\" car_type=\"2\" brandId=\"0\" serialId=\"<%=serialId%>\" carId=\"0\"></ins>');
                    </script>
                    <div class="clear">
                    </div>
                </div>
                <div class="line_box" id="pc-load" style="display: none;" data-channelid="2.21.820">
                    <div class="title-con">
                        <div class="title-box">
                            <h3><a href="http://www.daikuan.com/www/<%=serialSpell %>?from=yc36" target="_blank"><%=serialSeoName %>贷款推荐</a></h3>
                            <div class="more">
                                <a href="http://www.daikuan.com/www/<%=serialSpell %>?from=yc36" target="_blank">更多>></a>
                            </div>
                        </div>
                    </div>
                </div>
                <!--二手车-->
                <div class="line-box" id="ucarlist">
                </div>
                <%=askHtml%>
                <!-- baidu-tc begin {"action":"DELETE"} -->
                <div class="summaryMiddleAD">
                    <ins id="div_043ff6cd-b37a-4ca8-a25c-c81e8b05a94a" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="043ff6cd-b37a-4ca8-a25c-c81e8b05a94a"></ins>
                    <!--modified by sk 2013.04.10-->
                    <ins id="div_9710ab96-a655-4fcc-b2a3-5fb823705f6e" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="9710ab96-a655-4fcc-b2a3-5fb823705f6e"></ins>
                </div>
                <!-- baidu-tc end -->
                <!-- SEO导航 -->
                <div class="line_box">
                    <div class="title-box title-box2">
                        <h4>接下来要看</h4>
                    </div>
                    <div class="text-list-box-b" data-channelid="2.21.825">
                        <div class="text-list-box">
                            <ul class="text-list text-list-float text-list-float3">
                                <li><a href="/<%= serialSpell %>/peizhi/" target="_self">
                                    <%= serialShowName%>参数配置</a></li>
                                <li><a href="/<%= serialSpell %>/tupian/" target="_self">
                                    <%= serialShowName%>图片</a></li>
                                <%=nextSeePingceHtml%>
                                <li><a href="/<%= serialSpell %>/baojia/" target="_self">
                                    <%= serialShowName%>报价</a></li>
                                <li><a href="http://www.taoche.com/<%= serialSpell %>/" target="_blank">
                                    <%= serialShowName%>二手车</a></li>
                                <li><a href="/<%= serialSpell %>/koubei/" target="_self">
                                    <%= serialShowName%>怎么样</a></li>
                                <li><a href="/<%= serialSpell %>/youhao/" target="_self">
                                    <%= serialShowName%>油耗</a></li>
                                <li><a href="<%= baaUrl %>">
                                    <%= serialShowName%>论坛</a></li>
                                <%=nextSeeDaogouHtml%>
                                <%=pingceTagHtml %>
                            </ul>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <!-- SEO导航 -->
                <!-- SEO底部热门 -->
                <!-- baidu-tc begin {"action":"DELETE"} -->
                <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
                <!-- baidu-tc end -->
                <!-- SEO底部热门 -->
            </div>
            <div class="col-side">
                <!-- AD -->
                <!-- add by chengl Sep.13.2012 -->
                <%--<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c"></ins>--%>
                <!--其他车型 开始-->
                <%=brandOtherSerial%>
                <!--其他车型 结束-->
                <!--二手车 开始-->
                <%--<div class="line_box ucar_box">
			</div>--%>
                <!--二手车 结束-->
                <div class="col-side_ad">
                    <ins id="div_33883cb1-e98c-47d2-9dd3-e00d3034703e" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="33883cb1-e98c-47d2-9dd3-e00d3034703e"></ins>
                </div>
                <!-- baidu-tc begin {"action":"DELETE"} -->
                <div class="col-side_ad" style="margin-bottom: 10px; overflow: hidden">
                    <%--<script type="text/javascript" id="zp_script_94" src="http://mcc.chinauma.net/static/scripts/p.js?id=94&w=220&h=220&sl=1&delay=5"
						zp_type="1"></script>--%>
                    <script type="text/javascript" id="zp_script_242" src="http://mcc.chinauma.net/static/scripts/p.js?id=242&w=240&h=220&sl=1&delay=5"
                        zp_type="1"></script>
                </div>
                <!-- baidu-tc end -->
                <!-- baidu-tc begin {"action":"DELETE"} -->
                <div class="col-side_ad">
                    <ins id="div_149e57b4-e495-40e1-ae1c-30f4b80d955e" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="149e57b4-e495-40e1-ae1c-30f4b80d955e"></ins>
                </div>
                <div class="col-side_ad">
                    <ins id="div_65fd9a3a-89d8-403d-9c0a-a33686ec88d2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="65fd9a3a-89d8-403d-9c0a-a33686ec88d2"></ins>
                </div>
                <div class="col-side_ad">
                    <ins id="div_3a3bdb2a-ab81-415d-8d0a-a3395a4364d3" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="3a3bdb2a-ab81-415d-8d0a-a3395a4364d3"></ins>
                </div>
                <!-- baidu-tc end -->
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var serialId = "<%= serialId %>";
        <%= serialToSeeJson %>
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialtoseead.js"></script>
    <!-- baidu-tc end -->
    <!-- 导航脚本 -->
    <%= serialHeaderJs%>
    <script type="text/javascript">
        var CarCommonCSID = '<%= serialId.ToString() %>';
        var CarFilterData = <%=string.IsNullOrEmpty(carListFilterData)?"null":carListFilterData %>;
        var cityId = bit_locationInfo.cityId;
        var csSaleState ='<%=serialInfo.CsSaleState%>';
        var csAllSpell='<%=serialInfo.CsAllSpell%>';
        var priceRang='<%=serialPrice%>';
        //增加统计代码 by 2016.05.12
        $(".car_navigate").attr("data-channelid", "2.21.960");
        $("#sug_submit").attr("data-channelid", "2.21.961");
        $(".nav_small dd:first").attr("data-channelid", "2.21.957");
        $(".nav_small dd[class^='bit_']").attr("data-channelid", "2.21.958");
        $(".nav_small dd:last").attr("data-channelid", "2.21.959");
    </script>
    <script type="text/javascript" charset="utf-8" src="http://img4.bitautoimg.com/uimg/car/js/tabs_20140512_4.js"></script>
    <script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/csSummaryNew.min.js?v=2016110114"></script>
    <%--<script type="text/javascript" src="/jsnew/csSummaryNew.js"></script>--%>
    <script type="text/javascript">
        getLoanData();
        bitLoadScript("http://image.bitautoimg.com/carchannel/jsnew/jquery.lazyload.min.js",
	        function () {
	            if($(".car-pic20130802 img").length > 0){
	                $(".car-pic20130802 img").lazyload({
	                    placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
	                    threshold: 50
	                });
	            }
	            if($("#car-videobox img").length > 0){
	                $("#car-videobox img").lazyload({
	                    placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
	                    threshold: 50
	                });
	            }
			
	            if(typeof (bit_locationInfo) != 'undefined'){
	                cityId = bit_locationInfo.cityId;
	            } 
	            try {
	                searchEscByTypeRequest('<%=serialSpell %>');
	                if (document.getElementById('carYearList_all'))
	                { document.getElementById('carYearList_all').className = 'current'; }
	            }
	            catch (err) { }
	        }, "utf-8");
        

            //bitLoadScript("/jsnew/carcompareforminiV2.js?v=20141009",function(){
            //try { WaitCompareObj.Init(); } catch (e) { }
            //},'utf-8');

            function addClass(element, value) {
                if (!element.className) {
                    element.className = value;
                } else {
                    newClassName = element.className;
                    newClassName += " ";
                    newClassName += value;
                    element.className = newClassName;
                }
            }
            function removeClass(element, value) {
                var removedClass = element.className;
                var pattern = new RegExp("(^| )" + value + "( |$)");
                removedClass = removedClass.replace(pattern, "$1");
                removedClass = removedClass.replace(/ $/, "");
                element.className = removedClass;
                return true;
            }
            function TabBox(boxId, ulClassName, ulTagName, liTagName, liTagCurrentClassName, divClassName, divTagName) {
                var getByClassName = function(searchClass, node, tag) {
                    var classElements = new Array();
                    if (node == null)
                        node = document;
                    if (tag == null)
                        tag = '*';
                    var els = node.getElementsByTagName(tag);
                    var elsLen = els.length;
                    var pattern = new RegExp("(^|\\s)" + searchClass + "(\\s|$)");
                    for (i = 0, j = 0; i < elsLen; i++) {
                        if (pattern.test(els[i].className)) {
                            classElements[j] = els[i];
                            j++;
                        }
                    }
                    return classElements;
                };
                var oDiv = document.getElementById(boxId);
                if (oDiv) {
                    var aBtnUl = getByClassName(ulClassName, oDiv, ulTagName);
                    var aBtn = aBtnUl[0].getElementsByTagName(liTagName);
                    var displayDiv = getByClassName(divClassName, oDiv, divTagName);
                    //var timer = null;
                    for (var i = 0; i < aBtn.length; i++) {
                        (function(index) {
                            aBtn[i].onmouseover = function() {
                                var _this = this;
                                //timer = setTimeout(function(){
                                for (var i = 0; i < aBtn.length; i++) {
                                    //aBtn[i].className = "";
                                    removeClass(aBtn[i], liTagCurrentClassName);
                                    displayDiv[i].style.display = "none";
                                }

                                //_this.className = liTagCurrentClassName;
                                addClass(_this, liTagCurrentClassName);
                                displayDiv[index].style.display = "block";
                                //},500);
                            };

                            //aBtn[i].onmouseout = function(){
                            //				  clearTimeout(timer);
                            //			  };
                        })(i);
                    }
                }
            };
            TabBox("space_box", "v-t-list", "ul", "li", "current", "space_con_box", "div");
            TabBox("space_con_ltwo", "space_pic_info", "ul", "li", "current", "space_pic", "div");
            TabBox("vertical-tag-suv", "v-t-list", "ul", "li", "current", "suv-con-box", "div");
            (function () {                $(".more-c").mouseover(function () {
                    $(".more-c").addClass("m-current");
                    $("#more-color-sty").show();

                })
                $(".more-c").mouseout(function () {
                    $(".more-c").removeClass("m-current");
                    $("#more-color-sty").hide();
                });            })();
            //易团购
            (function(){
                var urlStr = "http://api.market.bitauto.com/MessageInterface/Product/GetProductUrl.ashx?CmdID="+<%=serialId %>+"&CityID="+cityId+"&MediaId=1&LocationId=2,1312";
                $.ajax({url: urlStr,cache: true,dataType: 'jsonp',jsonpCallback: "tgouCallback",success: function (data) {
                    if (data == null) {
                        return;
                    }
                    if (data.result=='yes') {
                        var tgouUrl = data.url;
                        var signName;
                        if(data.sign=="1"){
                            signName="团购";
                        }else if(data.sign=="2") {
                            signName="优惠"; 
                        }
                        $("#gouche-container_tgou").append("<div class=\"more\"><a href=\""+tgouUrl+"\" target=\"_blank\" class=\"tgou\"></a></div>");
                        $.each(data.carIds, function(i, n) {
                            $("#carlist_" + n).append("<a href=\""+tgouUrl+"\" target=\"_blank\" class=\"ad-yichehui-list\">"+signName+"</a>");
                        });
                    }
                }}); 
            })();
    </script>

    <!--本站统计代码-->
    <script type="text/javascript">
        bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsStat/StatisticJsOldPV.js,carchannel/jsStat/StatisticJs.js", function () {
            OldPVStatistic.ID1 = "<%=serialId %>";
            OldPVStatistic.ID2 = "0";
            OldPVStatistic.Type = 0;
            mainOldPVStatisticFunction();
        });
    </script>

    <script type="text/javascript" src="http://image.bitautoimg.com/stat/PageAreaStatistics.js"> </script>
    <script type="text/javascript">
        //二手车模块统计代码
        PageAreaStatistics.init("360,361");
    </script>
    <!--本站统计结束-->
    <!-- 广告代码要求提前Jul.14.2011 -->
    <script type="text/javascript">
        var adplay_CityName = ''; //城市
        var adplay_AreaName = ''; //区域
        var adplay_BrandID = '<%= serialId %>'; //品牌id 
        var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
        var adplay_BrandName = ''; //品牌
        var adplay_BlockCode = '48de7532-95b6-4100-9662-593c4f8533a2'; //广告块编号
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
    <script type="text/javascript">
        var adplay_CityName = ''; //城市
        var adplay_AreaName = ''; //区域
        var adplay_BrandID = '<%= serialId %>'; //品牌id 
        var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
        var adplay_BrandName = ''; //品牌
        var adplay_BlockCode = '0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a'; //广告块编号
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
    <!-- AD -->
    <script type="text/javascript">
        var adplay_CityName = ''; //城市
        var adplay_AreaName = ''; //区域
        var adplay_BrandID = '<%= serialId %>'; //品牌id 
        var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
        var adplay_BrandName = ''; //品牌
        var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
    <!--footer start-->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--footer end-->
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
    <%if (serialEntity.Level != null && serialEntity.Level.Name != "概念车")
      {%>
    <!-- baa 浏览过的车型-->
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%=serialId.ToString() %>');
        }
        catch (err)
        { }
    </script>
    <%} %>
    <!--footer end-->
    <%if (serialId == 1765 || serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388 || serialId == 2122 || serialId == 2196 || serialId == 1611 || serialId == 3152 || serialId == 2871 || serialId == 3382)
      { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <%} %>
    <!--提意见浮层-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
	         modelStr='<%=serialId%>-'+(zamCityId.length>=4?zamCityId.substring(0,2):zamCityId.substring(0,1))+'-'+zamCityId+'';
        var zamplus_tag_params = {
            modelId:modelStr,
            carId:0
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
    <div id="FocusCarFull" style="display: none;" class="tc-popup-box">
        <div class="tt">
            <h6>管理关注的车</h6>
            <a href="javascript:;" id="btn-focus-close" class="btn-close">关闭</a>
        </div>
        <div class="tc-popup-con">
            <div class="no-txt-box no-txt-error-mline">
                <p class="tit">
                    不能再继续添加了...
                </p>
                <p>
                    您最多可以添加 9辆关注车型，去车库删除一部分车型后就可以继续添加了。
                </p>
                <div class="tc_zf_box">
                    <div class="button_orange button_113_35">
                        <a href="javascript:;" id="a-focus-close">我知道了</a>
                    </div>
                    <div id="mangerCar_tc" class="button_gray button_113_35">
                        <a target="_blank" href="#">管理关注的车</a>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <ul id="right-side" style="display: none;">
        <li id="share_side" class="ewm-wrap clearfix"><a href="javascript:;" class="sjck">手机查看</a>
            <div class="tanchen" id="shareContent_side" style="display: none;">
                <div class="news_fenxiang">
                    <div class="bdsharebuttonbox fenxiang_box bdshare-button-style0-16" data-bd-bind="1435891808586">
                        <a data-cmd="qzone" class="bds_qzone" href="javascript:;" title="分享到QQ空间">QQ空间</a>
                        <a data-cmd="tsina" class="bds_tsina" href="javascript:;" title="分享到新浪微博">新浪微博</a>
                        <a data-cmd="tqq" class="bds_tqq" href="javascript:;" title="分享到腾讯微博">腾讯微博</a>
                        <a data-cmd="renren" class="bds_renren" href="javascript:;" title="分享到人人网">人人网</a>
                        <a data-cmd="douban" class="bds_douban" href="javascript:;" title="分享到豆瓣">豆瓣</a>
                    </div>
                    <div class="fenxiang_box_big">
                        <div class="bdsharebuttonbox bdshare-button-style0-16" data-bd-bind="1435891808586">
                            <a class="bds_weixin" href="javascript:;" title="分享到微信">微信</a>
                        </div>
                        <div class="fenxiang_box_img" id="qrcode-side"></div>
                        <div class="fenxiang_box_p">
                            <p>
                                微信扫一扫，将更全的车型信息分享给朋友，<a href="http://www.bitauto.com/zhuanti/other/2015weixinhelp/" target="_blank">不会用？&gt;&gt;</a>
                            </p>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
        </li>
    </ul>
    <!-- 对比浮动框 -->
    <div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none;" animateright="-533" animatebottom="229" data-page="summary">
        <div class="tt" id="bar_minicompare" style="cursor: move;">
            <h6>车型对比</h6>
            <a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
        </div>
        <div class="content">
            <ul id="idListULForWaitCompare" class="fixed-list"></ul>
            <div class="fixed-box">
                <div class="fixed-input" id="CarSelectSimpleContainerParent">
                    <input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly" />
                    <%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
                    <div class="right" userful="showcartypesim">
                        <div class="star">
                            <i class="star-i"></i>
                        </div>
                    </div>
                    <div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
                </div>
                <div class="clear"></div>
                <div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
            </div>
            <div class="wamp">
                <em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
                <div class="clear"></div>
            </div>
            <div class="alert-center" id="AlertCenterDiv" style="display: none;">
                <p>最多对比10个车款</p>
            </div>
        </div>
    </div>
    <!--漂浮层模板start-->
    <div class="effect" style="display: none;">
        <div class="car-summary-btn-duibi button_gray"><a href="javascript:void(0);" target="_self"><span>对比</span></a></div>
    </div>
    <!--漂浮层模板end-->
    <%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150733"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>
    <script type="text/javascript">
        $(function(){
            WaitCompareObj.Init();
        }); 

    </script>--%>
    <script type="text/javascript">
        (function(){
            bitLoadScript("http://image.bitautoimg.com/carchannel/gouche/pc/jquery.qrcode.min.js?v=20150424", function () {
                $('#qrcode').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%=h5SerialUrl%>?WT.mc_id=nbcar&ref=wk1" });
                $('#qrcode_header').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%=h5SerialUrl%>?WT.mc_id=nbcar&ref=wk2" });
                $('#qrcode-side').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%=h5SerialUrl%>?WT.mc_id=nbcar&ref=wk3" });
            }, "utf-8");
            $("#fenxiang,#shareContent").hover(function(){$("#shareContent").show();},function(){$("#shareContent").hide();});
            $("#share_header,#shareContent_header").hover(function(){$("#shareContent_header").show();},function(){$("#shareContent_header").hide();});
            $("#share_header,#shareContent_header").hover(function(){$("#shareContent_header").show();},function(){$("#shareContent_header").hide();});
            $("#right-side>li").insertBefore("#feedbackDiv");
            $("#share_side,#shareContent_side").hover(function(){$("#shareContent_side").show();},function(){$("#shareContent_side").hide();});
            window._bd_share_config = { "common": { "bdSnsKey": {}, "bdText": "", "bdMini": "2", "bdPic": "", "bdStyle": "0", "bdSize": "16" }, "share": {} }; with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'http://bdimg.share.baidu.com/static/api/js/share.js?v=89860593.js?cdnversion=' + ~(-new Date() / 36e5)];
        })();
        bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160715",function(){
            WaitCompareObj.Init();
        });
        var sideFeedLink="http://survey01.sojump.com/jq/7437387.aspx";
        if (CarCommonCSID == 2677){
            sideFeedLink="http://www.bitauto.com/topics/adtopic/bentengB50/";
        }
        else if (CarCommonCSID == 4283)
        { 
            sideFeedLink="http://www.bitauto.com/topics/adtopic/dihao/";
        }
        else if (CarCommonCSID == 3510)
        { 
            sideFeedLink="http://www.bitauto.com/topics/adtopic/qiyaK3/";
        }
        else if (CarCommonCSID == 3655)
        { 
            sideFeedLink="http://www.bitauto.com/topics/adtopic/langdong/";
        }
        else if (CarCommonCSID == 3924)
        { 
            sideFeedLink="http://www.bitauto.com/topics/adtopic/furuisi/";
        }
        else if (CarCommonCSID == 4588)
        { 
            sideFeedLink="http://www.bitauto.com/topics/adtopic/rongwei360/";
        } 
        //$("#feedbackDiv").before("<li class=\"w4 d11-backtop\"><a href=\""+sideFeedLink+"\"  title=\"\" target=\"_blank\">问卷调查</a></li>");
    </script>
</body>
</html>
<script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/jquery.jplayer.min.js?v=2016051617"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/audioJPlayer.js?v=2016051617"></script>
<script src="http://image.bitautoimg.com/uimg/index2014/js/jcarousellite_1.0.1.min.js"></script>
<script type="text/javascript">
    $(function () {
        var audioCreator;
        $.getJSON("http://api.admin.bitauto.com/audiobase/audio/GetAudioAudioResource?IsCarModel=1&pagesize=5&serialId=" +CarCommonCSID , function (data) {  //CarCommonCSID
            if(data.length>0)
            {
                $(".audio-box").show();
                var audioTemplate = document.getElementById('template_audios').innerHTML;
                audioCreator = lazyT.tmpl(audioTemplate);
                var audioHtml = audioCreator(data);
                $('#audio_list').html(audioHtml);
                var audioDom=$("#audio_list").find("li");
                var audioCount=0;
                if(audioDom&&audioDom.hasOwnProperty("length"))
                {
                    //控制音频简介字数限制
                    $.each(audioDom,function(index,item){
                        var audioContent=$(this).find(".aud-txt-box p").text(); 
                        var audioTxtCnt=0;
                        if(audioContent.length>0){
                            for(var i=0;i<audioContent.length;i++)
                            {
                                var curStr=audioContent[i];
                                if(isChn(curStr))
                                {
                                    audioTxtCnt+=2;
                                }
                                else
                                {
                                    audioTxtCnt+=1;
                                }
                                if(audioTxtCnt>=90)
                                {
                                    audioContent=audioContent.substr(0,i+1);
                                    $(this).find(".aud-txt-box p").html(audioContent+"...");
                                    break;
                                }
                            }
                        }
                    });

                    //
                    audioCount=audioDom.length;
                    if(audioCount>1)
                    {
                        $("#scrollBox").jCarouselLite({
                            btnNext: "#focus_right",
                            btnPrev: "#focus_left",
                            visible: 1,
                            beforeStart:function(){jp.stop();}
                        });
                        jp.init({swfPath: "../jsnew/audio"});
                    }
                    else
                    {
                        $("#focus_right").hide();
                        $("#focus_left").hide();
                        jp.init({swfPath: "../jsnew/audio"});
                    }
                }
            }  
        });
    });
    // 检查是否为中文
    function isChn(str){
        var reg = /^[\u4E00-\u9FA5]+$/;
        if(!reg.test(str)){
            return false;
        }
        return true;
    }
</script>

<!-- 经销商块改INS -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
<% if (serialId == 1765 || serialId == 3152 || serialId == 4169)
   { %>
<script>
    var _hmt = _hmt || [];
    (function() {
        var hm = document.createElement("script");
        hm.src = "https://hm.baidu.com/hm.js?7b86db06beda666182190f07e1af98e3";
        var s = document.getElementsByTagName("script")[0]; 
        s.parentNode.insertBefore(hm, s);
    })();
</script>
<% } %>

