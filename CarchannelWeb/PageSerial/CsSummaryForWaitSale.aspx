<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummaryForWaitSale.aspx.cs"
Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsSummaryForWaitSale" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>
        【<%= serialSeoName %>】最新<%= serialSeoName %>报价_参数_图片_<%= serialEntity.Brand.MasterBrand.Name + serialName %>论坛-易车网
    </title>
    <meta name="Keywords" content="<%= serialSeoName %>,<%= serialSeoName %>报价,<%= serialSeoName %>价格,<%= serialSeoName %>参数,<%= serialSeoName %>论坛,易车网,car.bitauto.com"/>
    <meta name="Description" content="<%= serialEntity.Brand.MasterBrand.Name + serialName %>,易车提供全国官方4S店<%= serialSeoName %>报价，最新<%= serialEntity.Brand.MasterBrand.Name + serialName %>降价优惠信息。以及<%= serialSeoName %>报价，<%= serialSeoName %>图片，<%= serialSeoName %>在线询价服务，低价买车尽在易车网。"/>
    <meta http-equiv="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%= serialSpell %>/"/>
    <meta http-equiv="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/w/carserial.aspx?serialid=<%= serialId %>"/>
    <link rel="canonical" href="http://car.bitauto.com/<%= serialSpell %>/"/>
    <!--#include file="~/ushtml/0000/yiche_2014_cube_carsummary-724.shtml"-->
</head>
<body>
<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
<div class="bt_pageBox">
    <!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
</div>
<!--书角广告代码-->
<ins id="div_0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a" type="ad_play" adplay_ip="" adplay_areaname=""
     adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
     adplay_blockcode="0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a">
</ins>
<!-- 顶通新增收起广告(2596锐界) -->
<ins id="div_2e7e5549-b9d7-4fb3-bec7-9f7b39efe715" type="ad_play" adplay_ip="" adplay_areaname=""
     adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
     adplay_blockcode="2e7e5549-b9d7-4fb3-bec7-9f7b39efe715">
</ins>
<!-- 顶通新增收起广告(2596锐界)-->
<div class="bt_ad">
    <ins id="div_2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02" type="ad_play" adplay_ip="" adplay_areaname=""
         adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
         adplay_blockcode="2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02">
    </ins>
</div>
<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_xdh_Manual.shtml"-->
<div class="bt_ad">
    <%= serialTopAdCode %>
</div>
<!--header start-->
<%= serialHeaderHtml %>
<!--header end-->
<div class="bt_page">
<div class="col-all">
<!-- Dec.6.2011 new AD -->
<!-- baidu-tc begin {"action":"DELETE"} -->
<ins id="div_d8728661-13a7-4a8c-8287-71f7e6d605c2" type="ad_play" adplay_ip="" adplay_areaname=""
     adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
     adplay_blockcode="d8728661-13a7-4a8c-8287-71f7e6d605c2">
</ins>
<!-- baidu-tc end -->
<!--首屏部分左侧 开始-->
<div class="col-con shouping_con">
    <%--<!--收藏 开始-->
				<div class="favstar" title="加关注">
					<a href="http://i.bitauto.com/baaadmin/car/guanzhu_<%=serialId %>/" target="_blank">
						收藏</a></div>
				<!--收藏 结束-->--%>
    <div class="line_box shouping_box line_box_top_b line-box_t0">
        <div class="card-head-box clearfix card-head-box_t0">
            <div class="l-box-sty">
                <!--焦点图开始-->
                <%= focusImagesHtml %>
                <!--焦点图结束-->
            </div>
            <!--中间开始-->
            <div class="txt-box zs-m-card">
                <p class="p-tit no-car">
                    未上市
                    <span class="f-price" style="cursor: default">
                                    <%= serialEntity.ReferPrice == "暂无" ? "" : "预售价：<em>" + serialEntity.ReferPrice + "</em>" %>  
                                </span>
                    <%--<em><%=serialMarketDay%></em>--%>
                    <a class="jxs-tip" style="display: none" href="#" target="_blank" id="tehui"></a>
                    <a id="favstar" href="javascript:;" title="点击关注" class="shoucang">
                        <span class="no-sc"></span>
                    </a><a href="javascript:;" class="fenxiang" id="fenxiang">
                        <span></span></a>
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
                                    用微信扫一扫，分享至朋友圈或给您的好友，<a href="http://www.bitauto.com/zhuanti/other/2015weixinhelp/" target="_blank">不会用？&gt;&gt;</a>
                                </p>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                </div>
                <%= waitFocusNewsHtml %>
                <!--广告开始-->
                <div class="wz-gg-box">
                    <ins id="div_ab5d1cc0-0115-4e6b-b8fb-e8f68ed85efd" type="ad_play" adplay_ip="" adplay_areaname=""
                         adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                         adplay_blockcode="ab5d1cc0-0115-4e6b-b8fb-e8f68ed85efd">
                    </ins>
                </div>
                <!--广告结束-->
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="line_box news_box">
        <div class="title-con">
            <div class="title-box">
                <h3>
                    <a href="<%= baaUrl %>" target="_blank">
                        <%= serialSeoName %>论坛
                    </a>
                </h3>
                <%= bbsMoreHtml %>
            </div>
        </div>
        <%= bbsNewsHtml %>
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
             adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
        </ins>
    </div>
    <div class="col-side_ad">
        <ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
             adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
        </ins>
    </div>
    <!--易湃-->
    <div class="col-side_ad" id="cmtadDiv">
    </div>
    <div class="col-side_ad">
        <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
             adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
        </ins>
    </div>
    <!-- New AD Dec.20.2011
    <div class="col-side_ad">
        <ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db">
        </ins>
    </div>-->
    <div class="col-side_ad">
        <ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853">
        </ins>
    </div>
    <%--<div class="col-side_ad">
					<ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
						adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
				</div>--%>
    <!-- -->
    <!-- 奖项 -->
    <%= awardHtml %>
    <!-- baidu-tc end -->
    <%= koubeiImpressionHtml %>
    <!--特价车 开始-->
    <div class="tejiache_box2" style="display: none;" id="tejiache_box">
    </div>
    <!--特价车 结束-->
    <!--看了还看 开始-->
    <div class="line-box" id="serialtosee_box">
        <div class="side_title">
            <h4>
                看过此车的人还看
            </h4>
        </div>
        <ul class="pic_list">
        </ul>
    </div>
    <!--看了还看 结束-->
    <!--对比 开始-->
    <%= hotSerialCompareHtml %>
    <!--对比 结束-->
</div>
<!--右首屏部分侧结束-->
<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
<!--2屏部分左侧 开始-->
<div class="col-con">
    <%--<!-- baidu-tc begin {"action":"DELETE"} -->
				<div class="summaryMiddleAD">
					<ins id="middleADForCar" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="37940534-3acb-4358-8f99-ac9abc6624ca"></ins>
				</div>
				<!-- 单一车型页（城市）/中部通栏 -->
				<div class="summaryMiddleAD">
					<ins id="div_fefd085a-31d6-44f3-9ba0-e75930818d3f" type="ad_play" adplay_ip="" adplay_areaname=""
						adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="fefd085a-31d6-44f3-9ba0-e75930818d3f"></ins>
				</div>
				<!-- baidu-tc end -->--%>
    <!--2屏部分左侧 结束-->
    <% if (isExistCarList)
       { %>
        <div class="line_box" id="car_list">
            <%= carListTableHtml %>
        </div>
    <% } %>
    <%= photoListHtml %>
    <%= videosHtml %>
    <%= hexinReportHtml %>
    <%= editorCommentHtml %>
    <%= koubeiDianpingHtml %>
    <!--#include file="~/ushtml/0000/yiche_2014_cube_carsummary_2015hotcar_style-891.shtml"-->
    <!--ad 2014.12.30-->
    <% if (false)
       { %>
        <ins id="div_2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="2bb1b2cb-71f3-40e6-9e0a-c041133c5bfe"></ins>
    <% }
       else if (serialEntity.Level.Name == "紧凑型车")
       { %>
        <ins id="div_fc718ffd-e00f-499c-be58-54f7dde02556" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="fc718ffd-e00f-499c-be58-54f7dde02556"></ins>
    <% }
       else if (serialEntity.Level.Name == "面包车")
       { %>
        <ins id="div_9ff50cb9-76d8-4054-9aec-c46d38c07ae2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9ff50cb9-76d8-4054-9aec-c46d38c07ae2"></ins>
    <% }
       else if (serialEntity.Level.Name == "SUV")
       { %>
        <ins id="div_ea1bb82e-5ed2-4531-b960-8d354d3a011f" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="ea1bb82e-5ed2-4531-b960-8d354d3a011f"></ins>
    <% }
       else if (serialEntity.Level.Name == "中型车")
       { %>
        <ins id="div_1a1c2052-cf6f-4a94-970c-f2a4847a3e80" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="1a1c2052-cf6f-4a94-970c-f2a4847a3e80"></ins>
    <% }
       else if (serialEntity.Level.Name == "微型车")
       { %>
        <ins id="div_d3734235-3fbe-47fc-99ea-57669cedadf5" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d3734235-3fbe-47fc-99ea-57669cedadf5"></ins>
    <% }
       else if (serialEntity.Level.Name == "小型车")
       { %>
        <ins id="div_94042601-39b4-4161-892a-b0608e589e6e" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="94042601-39b4-4161-892a-b0608e589e6e"></ins>
    <% }
       else if (serialEntity.Level.Name == "豪华车")
       { %>
        <ins id="div_569fe415-15f5-4ad2-9028-a49dce5186f9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="569fe415-15f5-4ad2-9028-a49dce5186f9"></ins>
    <% }
       else if (serialEntity.Level.Name == "MPV")
       { %>
        <ins id="div_952ce0ee-507f-4f26-8f11-ce6c06812a49" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="952ce0ee-507f-4f26-8f11-ce6c06812a49"></ins>
    <% }
       else if (serialEntity.Level.Name == "跑车")
       { %>
        <ins id="div_abe53914-1717-427b-9251-b207798ebdca" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="abe53914-1717-427b-9251-b207798ebdca"></ins>
    <% }
       else if (serialEntity.Level.Name == "中大型车")
       { %>
        <ins id="div_be5865ff-b854-438d-9863-d54af38d21a7" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="be5865ff-b854-438d-9863-d54af38d21a7"></ins>
    <% } %>
    <!-- 经销商 -->
    <div class="line_box" id="vendorInfo">
        <div class="title-box">
            <h3>
                <a href="http://dealer.bitauto.com/<%= serialSpell %>/" target="_blank">
                    <%= serialSeoName %>经销商推荐
                </a>
            </h3>
            <div class="more">
                <a target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">更多&gt;&gt;</a>
            </div>
        </div>
        <%--<ins id="ep_union_4" partner="1" version="" isupdate="1" type="1" city_type="-1"
						city_id="0" city_name="0" car_type="2" brandid="0" serialid="<%= serialId %>"
						carid="0"></ins>--%>
        <script type="text/javascript">
            document.write('<ins Id=\"ep_union_123\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\"' + bit_locationInfo.cityId + '\" city_name=\"0\" car_type=\"2\" brandId=\"0\" serialId=\"<%= serialId %>\" carId=\"0\"></ins>');
        </script>
        <div class="clear">
        </div>
    </div>
    <%= askHtml %>
    <!-- baidu-tc begin {"action":"DELETE"} -->
    <div class="summaryMiddleAD">
        <ins id="div_043ff6cd-b37a-4ca8-a25c-c81e8b05a94a" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="043ff6cd-b37a-4ca8-a25c-c81e8b05a94a">
        </ins>
        <!--modified by sk 2013.04.10-->
        <ins id="div_9710ab96-a655-4fcc-b2a3-5fb823705f6e" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="9710ab96-a655-4fcc-b2a3-5fb823705f6e">
        </ins>
    </div>
    <!-- baidu-tc end -->
    <!-- SEO导航 -->
    <div class="line_box">
        <div class="title-box title-box2">
            <h4>
                <a href="javascript:;">接下来要看</a>
            </h4>
        </div>
        <div class="text-list-box-b">
            <div class="text-list-box">
                <ul class="text-list text-list-float text-list-float3">
                    <li>
                        <a href="/<%= serialSpell %>/peizhi/" target="_self">
                            <%= serialShowName %>参数配置
                        </a>
                    </li>
                    <li>
                        <a href="/<%= serialSpell %>/tupian/" target="_self">
                            <%= serialShowName %>图片
                        </a>
                    </li>
                    <%= nextSeePingceHtml %>
                    <li>
                        <a href="/<%= serialSpell %>/baojia/" target="_self">
                            <%= serialShowName %>报价
                        </a>
                    </li>
                    <li>
                        <a href="http://www.taoche.com/buycar/serial/<%= serialSpell %>/"
                           target="_blank">
                            <%= serialShowName %>二手车
                        </a>
                    </li>
                    <li>
                        <a href="/<%= serialSpell %>/koubei/" target="_self">
                            <%= serialShowName %>怎么样
                        </a>
                    </li>
                    <li>
                        <a href="/<%= serialSpell %>/youhao/" target="_self">
                            <%= serialShowName %>油耗
                        </a>
                    </li>
                    <li>
                        <a href="<%= baaUrl %>">
                            <%= serialShowName %>论坛
                        </a>
                    </li>
                    <%= nextSeeDaogouHtml %>
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
    <ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
         adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
         adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c">
    </ins>
    <!--其他车型 开始-->
    <%= brandOtherSerial %>
    <!--其他车型 结束-->
    <!--二手车 开始-->
    <%--<div class="line_box ucar_box">
			</div>--%>
    <!--二手车 结束-->
    <div class="col-side_ad" style="width: 220px; overflow: hidden">
        <ins id="div_33883cb1-e98c-47d2-9dd3-e00d3034703e" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="33883cb1-e98c-47d2-9dd3-e00d3034703e">
        </ins>
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
             adplay_blockcode="149e57b4-e495-40e1-ae1c-30f4b80d955e">
        </ins>
    </div>
    <div class="col-side_ad">
        <ins id="div_3a3bdb2a-ab81-415d-8d0a-a3395a4364d3" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="3a3bdb2a-ab81-415d-8d0a-a3395a4364d3">
        </ins>
    </div>
    <!-- baidu-tc end -->
</div>
<div class="clear">
</div>
</div>
</div>
<!-- 对比浮动框 -->
<!-- baidu-tc begin {"action":"DELETE"} -->
<%--<div id="divWaitCompareLayer" class="comparebar comparebar-index" style="display: none;">
	</div>--%>
<!-- baidu-tc end -->
<!-- 导航脚本 -->
<%= serialHeaderJs %>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
<script type="text/javascript">
    var CarCommonCSID = '<%= serialId.ToString() %>';
    var cityId = bit_locationInfo.cityId;
    var csSaleState = '<%= serialInfo.CsSaleState %>';
</script>
     <script type="text/javascript">
         var serialId = "<%= serialId %>";
         <%= serialToSeeJson %>
         //var SerialAdpositionContentInfo = {
         //    "3545": [
         //        {
         //            "SerialID": "2371",
         //            "Text": "速腾",
         //            "Link": "http://car.bitauto.com/suteng/",
         //            "Image": "http://img3.bitautoimg.com/autoalbum/files/20120816/666/0407166663_5.jpg"
         //        }, null]
         //};
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialtoseead.js"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/csSummaryNew.js?v=20160815"></script>

<script type="text/javascript">
    //getDealerList(<%= serialId %>,'<%= serialSpell %>',bit_locationInfo.cityId);
    //getSerialGoodsNew(<%= serialId %>,'<%= serialShowName %>','<%= serialWhiteImageUrl %>',bit_locationInfo.cityId);
    //getSerialCashBack(<%= serialId %> , '<%= serialShowName %>', bit_locationInfo.cityId);
	
</script>
<script type="text/javascript">
    bitLoadScript("http://image.bitautoimg.com/carchannel/jsnew/jquery.lazyload.min.js?v=20151026", function() {
        $(".car-pic20130802 img, #car-videobox img").lazyload({
            placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
            threshold: 50
        });
        var cityId = 201;
        if (typeof (bit_locationInfo) != 'undefined') {
            cityId = bit_locationInfo.cityId;
        }
        try {
            if (document.getElementById('carYearList_all')) {
                document.getElementById('carYearList_all').className = 'current';
            }
        } catch (err) {
        }
    }, "utf-8");
    //bitLoadScript("http://image.bitautoimg.com/carchannel/jsnew/carcompareformini.js?v=20141009",function(){
    //	try { insertWaitCompareDiv(); } catch (e) { }
    //},'utf-8');
    bitLoadScript("http://img1.bitauto.com/bt/cmtad/adv.js?v=201412", function() {
        try {
            AdvObject.GetAdvByCityIdAndSerialId(<%= serialId %>);
        } catch (e) {
        }
    }, "utf-8");

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
    //今日头条
    var global_hash = window.location.hash;
    $(function() {
        if (global_hash == "#jrtt") {
            $(".bit_top990, .bt_ad, .nav_small, .header_style, .foot_box").hide();
            $("ins[id!='ep_union_123']").remove();
        }
    });
</script>
<!--本站统计代码-->
<script type="text/javascript">
    bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsStat/StatisticJsOldPV.js", function() {
        OldPVStatistic.ID1 = "<%= serialId %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    });
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

<!--漂浮层模板start-->
<div class="effect" style="display: none;">
    <div class="car-summary-btn-duibi button_gray">
        <a href="###" target="_self">
            <span>对比</span></a>
    </div>
</div>
<!--漂浮层模板end-->
<ul id="right-side" style="display: none;">
    <li id="share_side" class="ewm-wrap clearfix">
        <a href="javascript:;" class="sjck">手机查看</a>
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
                            用微信扫一扫，分享至朋友圈或给您的好友，<a href="http://www.bitauto.com/zhuanti/other/2015weixinhelp/" target="_blank">不会用？&gt;&gt;</a>
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
<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none;" animateright="-533" animatebottom="180" data-page="summary">
    <div class="tt" id="bar_minicompare" style="cursor: move;">
        <h6>车型对比</h6>
        <a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
    </div>
    <div class="content">
        <ul id="idListULForWaitCompare" class="fixed-list"></ul>
        <div class="fixed-box">
            <div class="fixed-input" id="CarSelectSimpleContainerParent">
                <input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly"/>
                <%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
                <div class="right" userful="showcartypesim">
                    <div class="star">
                        <i class="star-i"></i>
                    </div>
                </div>
                <div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
            </div>
            <div class="clear"></div>
            <div class="btn-sty button_orange">
                <a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a>
            </div>
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
<%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150730"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>--%>
<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160120"></script>
<script type="text/javascript">
    (function() {
        bitLoadScript("http://image.bitautoimg.com/carchannel/gouche/pc/jquery.qrcode.min.js?v=20150424", function() {
            $('#qrcode').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%= h5SerialUrl %>?WT.mc_id=nbcar&ref=wk1" });
            $('#qrcode_header').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%= h5SerialUrl %>?WT.mc_id=nbcar&ref=wk2" });
            $('#qrcode-side').qrcode({ render: "table", typeNumber: 4, width: 130, height: 130, correctLevel: 1, text: "<%= h5SerialUrl %>?WT.mc_id=nbcar&ref=wk3" });
        }, "utf-8");
        $("#fenxiang, #shareContent").hover(function() { $("#shareContent").show(); }, function() { $("#shareContent").hide(); });
        $("#share_header, #shareContent_header").hover(function() { $("#shareContent_header").show(); }, function() { $("#shareContent_header").hide(); });
        $("#share_header, #shareContent_header").hover(function() { $("#shareContent_header").show(); }, function() { $("#shareContent_header").hide(); });
        $("#right-side > li").insertBefore("#feedbackDiv");
        $("#share_side, #shareContent_side").hover(function() { $("#shareContent_side").show(); }, function() { $("#shareContent_side").hide(); });
        window._bd_share_config = { "common": { "bdSnsKey": {}, "bdText": "", "bdMini": "2", "bdPic": "", "bdStyle": "0", "bdSize": "16" }, "share": {} };
        with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'http://bdimg.share.baidu.com/static/api/js/share.js?v=89860593.js?cdnversion=' + ~(-new Date() / 36e5)];
    })();
    $(function() {
        WaitCompareObj.Init();
    });
</script>

<!--footer start-->
<!--#include file="~/html/footer2014.shtml"-->
<!--footer end-->
<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
<% if (serialEntity.Level != null && serialEntity.Level.Name != "概念车")
   { %>
    <!-- baa 浏览过的车型-->
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%= serialId.ToString() %>');
        } catch (err) {
        }
    </script>
<% } %>
<!--footer end-->
<% if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388 || serialId == 2122 || serialId == 2196 || serialId == 1611 || serialId == 3152 || serialId == 2871 || serialId == 3382)
   { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
<% } %>
<!--提意见浮层-->
<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
<!--#include file="~/html/CommonBodyBottom.shtml"-->
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

</body>
</html>
<!-- 经销商块改INS -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>