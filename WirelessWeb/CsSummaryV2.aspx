<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummaryV2.aspx.cs" Inherits="WirelessWeb.CsSummaryV2" %>
<%@ Import Namespace="System.Globalization" %>
<% Response.ContentType = "text/html"; %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <title><%= _title %></title>
    <meta name="Keywords" content="<%= _keyWords %>"/>
    <meta name="Description" content="<%= _Description %>"/>
    <link rel="canonical" href="http://car.m.yiche.com/<%= _serialAllSpell %>/"/>
    <!--#include file="~/ushtml/0000/myiche2016_cube_summary-1154.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.yiche.com/iplocation/setcookie.ashx"> </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        var GlobalSummaryConfig = {
            SerialId:<% = _serialId %>,
            CityId:bit_locationInfo.cityId
        };
    </script>
</head>
<body>
<div id="container">
<!-- header start -->
<div class="op-nav-out">
    <div class="op-nav-static op-nav-fixed"> 

        <div class="op-nav">
            <a id="gobackElm" data-channelid="27.23.690" href="javascript:;" class="btn-return">返回</a>
            <div class="tt-name">
                <a data-channelid="27.23.1032" href="http://m.yiche.com/" class="yiche-logo">汽车</a>
                <h1><%= _serialShowName %></h1>
            </div>
            <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
        </div>
        <div class="op-nav-mark" style="display: none;"></div>
    </div>
</div>
<!-- header end -->
<%= CsHeadHTML %>
<!--nav fixed start-->
<div class="car-nav-fixed-box" id="carNavFixed" style="display: none">
    <div class="car-nav-rel">
        <div class="car-nav-fixed">
            <ul>
                <li class="current">
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/" data-channelid="27.23.1377">
                        <strong>综述<s></s></strong>
                    </a>
                </li>
                <li>
                    <a href="http://price.m.yiche.com/nb<%= _serialId %>/" data-channelid="27.23.1368">
                        <strong>报价</strong>
                    </a>
                </li>
                <li >
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/peizhi/" data-channelid="27.23.1369">
                        <strong>参数</strong>
                    </a>
                </li>
                <li>
                    <a href="http://photo.m.yiche.com/serial/<%= _serialId %>/" data-channelid="27.23.1370">
                        <strong>图片</strong>
                    </a>
                </li>
                <li>
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/wenzhang/" data-channelid="27.23.1371">
                        <strong>文章</strong>
                    </a>
                </li>
                <li>
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/koubei/" data-channelid="27.23.1372">
                        <strong>口碑</strong>
                    </a>
                </li>
                <li>
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/youhao/" data-channelid="27.23.1373">
                        <strong>油耗</strong>
                    </a>
                </li>
                <li>
                    <a href="<%= _baaUrl %>" data-channelid="27.23.1374">
                        <strong>论坛</strong>
                    </a>
                </li>
                <li>
                    <a href="http://m.taoche.com/<%= _serialAllSpell %>/?ref=Hlht_esc" data-channelid="27.23.1375">
                        <strong>二手车</strong>
                    </a>
                </li>
                <li>
                    <a href="http://yanghu.m.yiche.com/xuanze/0/<%= _serialId %>/?source=cxytab&amp;atype&amp;type=0" data-channelid="27.23.1376">
                        <strong>养护</strong>
                    </a>
                </li>
            </ul>
        </div>
        <div class="right-mask"></div>
    </div>
</div>
<!--nav fixed end-->
<div class="sum-car-img">
    <% if (focusImg.Count > 0)
       { %>
        <%= focusImg[0] %>
        <div class="right-area">
            <% for (var count = 2; count <= focusImg.Count; count++)
               { %>
                <%= focusImg[count - 1] %>
            <% } %>
        </div>
    <% } %>
</div>
<div class="clear"></div>
<div class="sum-info">
    <div class="car-main">
        <div class="car-logo">
            <a href="http://car.m.yiche.com/<%= _serialEntity.Brand.MasterBrand.AllSpell %>/">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= _serialEntity.Brand.MasterBrand.Id %>_100.png">
            </a>
        </div>
        <dl>
            <dt>
                <h2><%= _serialSeoName %></h2><%= _serialEntity.SaleState == "停销" ? "<em>停售</em>" : _serialEntity.SaleState == "待销" ? "<em>未上市</em>" : "" %>
            </dt>
            <dd>
                <a href="http://price.m.yiche.com/nb<%= _serialId %>/" data-channelid="27.23.952">
                    <% if (_serialEntity.SaleState == "停销")
                       { %>
                        二手价：<strong><%= string.IsNullOrEmpty(uCarPriceRange) ? "暂无" : uCarPriceRange %></strong>
                    <% }
                       else if (_serialEntity.SaleState == "待销")
                       { %>
                        预售价：<strong><%= string.IsNullOrEmpty(_serialPrice) ? "暂无" : _serialPrice %></strong>
                    <% }
                       else
                       { %>
                        参考成交价：<strong><%= string.IsNullOrEmpty(_serialPrice) ? "暂无" : _serialPrice %></strong>
                    <% }
                    %>
                    <i class="dujia" id="dujia" style="display: none">独家</i>
                </a>
            </dd>
        </dl>
        <a href="javascript:void(0);" id="favstar" data-channelid="27.23.726" class="ico-favorite">
            <i></i>
            关注
        </a>
        <%--<a href='#' class='ico-favorite ico-favorite-sel'>
                        <i></i>
                        已关注
                    </a>--%>
        <% if (_serialEntity.SaleState != "停销")
           { %>
            <a href="http://car.h5.yiche.com/<%= _serialAllSpell %>/" class="ico-laiyiche" data-channelid="27.23.1317"><i></i>来一车</a>
        <% }
        %>

    </div>
    <div class="car-price">
        <dl class="w">
            <dt class="w3">指导价：</dt>
            <dd><%= _serialRefPrice %></dd>
        </dl>
        <dl>
            <dt class="w2">排名：</dt>
            <dd><%= _serialTotalPV %></dd>

        </dl>
        <div class="clear"></div>

        <% if (isElectrombile)
           { %>
            <dl class="w">
                <dt class="w3">续航：</dt>
                <dd>
                    <% if (!string.IsNullOrEmpty(mileageRange))
                       { %>
                        <%= mileageRange %>
                    <% }
                       else
                       { %>
                        暂无
                    <% } %>
                </dd>
            </dl>
        <% }
           else
           { %>
            <dl class="w">
                <dt class="w3">油耗：</dt>
                <dd>
                    <% if (!string.IsNullOrEmpty(_serialFuelCost))
                       { %>
                        <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/youhao/?ref=Hlht_yh" class="" data-channelid="27.23.953"><%= _serialFuelCost %></a>
                    <% }
                       else
                       { %>
                        暂无
                    <% } %>
                </dd>
            </dl>
        <% } %>

        <dl>
            <dt class="w2">厂商：</dt>
            <dd><%= _chanDi %></dd>
        </dl>
        <div class="clear"></div>
        <% if (SerialColorList.Count > 0)
           { %>
            <dl class="line">
                <dt class="w3">颜色：</dt>
                <dd>
                    <% foreach (var serialColorEntity in SerialColorList)
                       { %>
                        <%= "<em style='background: " + serialColorEntity.ColorRGB + "'></em>" %>
                    <% } %>
                </dd>
            </dl>
        <% } %>
    </div>
    <div class="clear"></div>
    <%--			<% if (_serialEntity.SaleState == "停销")
      { %>
			<div class='sum-btn sum-btn-two' id="feibaoxiaoche" style="display: block;">
				<ul>
					<li><a data-channelid="27.23.118" href='http://m.taoche.com/pinggu/?ref=mchexizsgu'>二手车估价</a></li>
					<li class='btn-org'><a data-channelid="27.23.117" href='http://m.taoche.com/<%=_serialAllSpell %>/?ref=mchexizsmai&leads_source=m002014'>买二手车</a></li>
				</ul>
			</div>
			<% }
      else if (_serialEntity.SaleState == "待销")
      { }
      else
      { %>
			<div class='sum-btn' id="feibaoxiaoche" style="display: block;">
				<ul>
					<li><a data-channelid="27.23.114" href='http://chedai.m.yiche.com/<%= _serialAllSpell %>/chedai/?from=ycm1&leads_source=m002001'>贷款</a></li>
                    <li><a data-channelid="27.23.115" href='http://zhihuan.m.taoche.com/s<%= _serialId %>/?ref=mchexizshuan&leads_source=m002002'>置换</a></li>
					<li class='btn-org'><a data-channelid="27.23.116" href='http://price.m.yiche.com/zuidijia/nb<%= _serialId %>/?leads_source=m002003'>询底价</a></li>
				</ul>
			</div>
			<% } %>


			<div class='sum-btn sum-btn-two' id="baoxiaoche" style="display: none">
				<ul>
					<li><a data-channelid="27.23.114" href='http://m.yichemall.com/car/Detail/index?modelId=<%= _serialId %>&from=ycm1'>贷款</a></li>
					<li class='btn-org'><a data-channelid="27.23.120" href='http://m.yichemall.com/car/Detail/index?modelId=<%= _serialId %>&source=100064'>直销特卖</a></li>
				</ul>
			</div>--%>
    <div class="sum-btn" id="btn-business"></div>
</div>
<!-- 易车购车服务 start -->
<div class="car-service b-shadow" id="carservice" style="display: none">
    <div class="tt-first">
        <h3>易车购车服务</h3>
        <a href="http://11.m.yiche.com/" class="tt-first-double">11.11购车狂欢节</a>
        <div class="opt-more opt-more-gray">
            <a href="http://gouche.m.yiche.com/sb<%= _serialId %>/">更多</a>
        </div>
    </div>
    <ul id="buycar-youhui" class="one-service service-new">
    </ul>
</div>
<!-- 易车购车服务 end -->
<!-- 推广 start -->
<script type="text/javascript">
    var BitautoCityForAd = bit_locationInfo.cityName;
    var carlevel = '<%= _serialLevel %>';
    var uCarLowestPrice = '<%= uCarLowestPrice %>'; //二手车最低价
    var serialAllSpell = "<%= _serialAllSpell %>";

    function showNewsInsCode(dxc, xxc, mpv, suv) {
        var adBlockCode = xxc;
        if (carlevel == '中大型车' || carlevel == '中型车' || carlevel == '跑车' || carlevel == '豪华车') {
            adBlockCode = dxc;
        } else if (carlevel == '微型车' || carlevel == '小型车' || carlevel == '紧凑型车') {
            adBlockCode = xxc;
        } else if (carlevel == '概念车' || carlevel == 'MPV' || carlevel == '面包车' || carlevel == '皮卡' || carlevel == '其它') {
            adBlockCode = mpv;
        } else if (carlevel == 'SUV') {
            adBlockCode = suv;
        }
        document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
    }
    //showNewsInsCode('6a64c6c9-9936-4e56-adf5-71c83f4e07f9', 'dc0429a5-7e32-4adf-b29c-0f0683e64745', '24f397d8-4dc5-4388-ade9-27ced12938f2', '51346d54-224f-4fe4-a166-e41f5c307e28');
</script>
<!-- 推广 end -->
<!-- 增加车型详解区域 start -->
<%= _carDetailsViewZoneHtml %>
<!-- 增加车型详解区域 end -->
<script type="text/javascript">
    showNewsInsCode('db07f914-5b83-4921-bf2d-52a8574924b3', '35cae8c1-7ba0-4711-ace5-e2371ec97d6e', 'd215e33f-944e-48c6-ba50-3e272c728631', '85163aab-0294-4054-a82b-cfae3d3106ed');
</script>
<!-- 限时特惠 start -->
<div class="prefer-tim b-shadow">
    <%-- <span class="yh-tap"></span>
            <div class="cont-box">
                <h4>本地限时特惠</h4>
                <p>凯迪拉克团购召集令，<em>千元油卡</em>送送送！</p>
            </div>--%>
</div>
<!-- 限时特惠 end -->
<%= _carList %>
<%-- app下载--%>
<!--#include file="~/include/pd/2016/wap/00001/201606_yidongzhan_appdown_Manual.shtml" -->
<%-- app下载--%>
<%--<!-- 易车帮你找低价 start -->
		<span id="ad-img" data-channelid="27.23.729">
			<a href="http://gouche.m.yiche.com/home/YiShuBang/?csId=<%= _serialId %>" class="ad-img">
				<img src="http://image.bitautoimg.com/uimg/mbt2015/images/pic_zhaodijia.png">
			</a>
		</span>
		<!-- 易车帮你找低价 end -->--%>
<script type="text/javascript">
    showNewsInsCode('f327fc17-f1ad-4fcd-91d7-03e42c9b72b4', 'f31814e7-225c-4e92-8ea9-c2876315cee0', '20f3b410-22f8-4035-9bf7-3267a88bda6a', '2afe4673-39b3-4bad-9f48-38b0b6cb9513');
</script>
<!-- 口碑 start -->
<%= koubeiImpressionHtml %>
<!-- 口碑 end -->
<!-- 编辑评车 start -->
<%--<div class="voice-swiper-outer b-shadow" style="display:none">
            <div class="voice-swiper" id="voiceSwiper">
                <ul class="swiper-wrapper" id="audio_list">
                    
                </ul>
                <div class="pagination-voice pagination-swiper"></div>            </div>
        </div>--%>
<div class="tt-first" id="audio-title" style="display: none;">
    <h3>专家说车</h3>
</div>
<div class="talk-car-box b-shadow" id="audio_list"></div>
<!-- 编辑评车 start -->
<!-- 文章 start -->
<div class="tt-first" id="m-tabs-article" style='<%= _wenZhangShowFlag ? "" : "display:none" %>'>
    <h3>文章</h3>
    <div class="opt-more">
        <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/wenzhang/">更多</a>
    </div>
</div>
<div class="second-tags mgt-10" id="newslistTab" style='<%= _wenZhangShowFlag ? "" : "display:none" %>'>
    <div class="pd15">
        <ul>
            <% if (!string.IsNullOrEmpty(_serialNews))
               { %>
                <li class="current">
                    <a href="#">
                        <span>热门</span></a>
                </li>
            <% } %>
            <% if (!string.IsNullOrEmpty(newsPingceHtml))
               { %>
                <li <%= string.IsNullOrEmpty(_serialNews) ? "class='current'" : "" %>>
                    <a href="#">
                        <span>评测</span></a>
                </li>
            <% } %>
            <% if (!string.IsNullOrEmpty(newsDaogouHtml))
               { %>
                <li <%= string.IsNullOrEmpty(_serialNews) && string.IsNullOrEmpty(newsPingceHtml) ? "class='current'" : "" %>>
                    <a href="#">
                        <span>导购</span></a>
                </li>
            <% } %>
            <li>
                <a href="#">
                    <span>行情</span></a>
            </li>
        </ul>
    </div>
</div>
<div class="swiper-container swiper-container-newslist b-shadow" style='<%= _wenZhangShowFlag ? "" : "display:none" %>'>
    <div class="swiper-wrapper">
        <!-- 文章 start -->
        <% if (!string.IsNullOrEmpty(_serialNews))
           { %>
            <div class="swiper-slide" id="m_article" data-channelid="27.23.732">
                <%= _serialNews %>
                <%--<a href="http://car.m.yiche.com/<%= _serialAllSpell %>/wenzhang/" class="btn-more"><i>查看更多文章</i></a>--%>
            </div>
        <% } %>
        <!-- 文章 end -->
        <% if (!string.IsNullOrEmpty(newsPingceHtml))
           { %>
            <div class="swiper-slide" data-channelid="27.23.1341">
                <div class="card-news">
                    <ul>
                        <%= newsPingceHtml %>
                    </ul>
                </div>
                <% if (pingceLiFlag)
                   { %>
                    <a href="javascript:;" id="btn-pingce-more" class="btn-more btn-add-more">
                        <i>加载更多</i>
                    </a>
                <% }
                   else
                   { %>
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/pingce/" class="btn-more">
                        <i>查看更多</i>
                    </a>
                <% } %>
            </div>
        <% } %>
        <% if (!string.IsNullOrEmpty(newsDaogouHtml))
           { %>
            <div class="swiper-slide" data-channelid="27.23.1340">
                <div class="card-news">
                    <ul>
                        <%= newsDaogouHtml %>
                    </ul>
                </div>
                <% if (daogouLiFlag)
                   { %>
                    <a href="javascript:;" id="btn-daogou-more" class="btn-more btn-add-more">
                        <i>加载更多</i>
                    </a>
                <% }
                   else
                   { %>
                    <a href="http://car.m.yiche.com/<%= _serialAllSpell %>/daogou/" class="btn-more">
                        <i>查看更多</i>
                    </a>
                <% } %>
            </div>
        <% } %>
        <!-- 行情 start -->
        <div class="swiper-slide">
            <div class="card-news">
                <div class="hangqing-box">
                    <span>当前定位地区：</span>
                    <a href="javascript:;" id="m-hangqing-city" data-action="province">北京<i></i></a>
                </div>
                <div class="yxc-prompt-box mgt90" id="no-hangqingmsg" style="display: none; height: 300px;">
                    <div class="yxc-prompt-none"></div>
                    <p>暂无降价行情</p>
                </div>
                <ul id="carnews1" data-channelid="27.23.733">
                </ul>
            </div>
            <%-- <a href="javascript:;" id="btn-hq-more" class="btn-more btn-add-more"><i>加载更多</i></a>--%>
        </div>
        <!-- 行情 end -->
    </div>
</div>
<!-- 文章 end -->
<%--<!-- 主题推荐 start -->
<% if (DaogouDictionary.Count > 0)
   { %>
    <div class="tt-first">
        <h3>主题推荐</h3>
    </div>
    <div class="theme-tuijian b-shadow">
        <div class="swiper-container swiper-container-theme" id="themeTuijian">
            <ul class="swiper-wrapper">
                <%
                    Dictionary<string, string> tempDictionary = new Dictionary<string, string> {{"1", "dakongjiansuv"}, {"2", "xiaogangpao"}, {"3", "dakongjianjiaoche"}, {"4", "hezisuv"}};
                    Dictionary<string, string> tempTongji = new Dictionary<string, string> {{"1", "26.151.1442"}, {"2", "26.151.1441"}, {"3", "26.151.1443"}, {"4", "26.151.1448"}};
                    foreach (var item in DaogouDictionary)
                    {
                        if (item.Value.Contains(_serialId))
                        { %>
                        <li class="swiper-slide">
                            <a href="/daogou/<%= tempDictionary[item.Key] %>/" data-channelid="<%= tempTongji[item.Key] %>">
                                <img src="http://image.bitautoimg.com/uimg/mbt2016/images/pic_daogou<%= item.Key %>.jpg">
                            </a>
                        </li>
                <%
                        }
                    } %>
            </ul>
        </div>
        <div class="pagination-theme"></div>
    </div>
<% } %>
<!-- 主题推荐 end -->--%>
<!-- 视频 start -->
<% if (VideoList != null && VideoList.Count > 0)
   { %>
    <div class="tt-first" data-channelid="27.23.734">
        <h3>易车视频</h3>
        <div class="opt-more">
            <a href="http://v.m.yiche.com/car/serial/<%= _serialId %>_0_0.html">更多</a>
        </div>
    </div>
    <div class="video-box b-shadow" id="m_video" data-channelid="27.23.735">
        <ul>
            <%
                var ids = "";
                foreach (var videoEntity in VideoList)
                {
                    ids += videoEntity.VideoId + ",";
            %>
                <li>
                    <a href="<%= videoEntity.ShowPlayUrl.Replace("v.bitauto.com", "v.m.yiche.com") %>">
                        <span>
							<img src="<%= videoEntity.ImageLink.Replace("Video", "newsimg_300x168/Video").Replace("video", "newsimg_300x168/Video") %>">
							<i></i>
						</span>
                        <p><%= videoEntity.ShortTitle %></p>
                        <% var timeSpan = new TimeSpan(0, 0, videoEntity.Duration); %>
                        <em>
                            <i class="play-num" id="viewcount_<%= videoEntity.VideoId %>">0</i><i class="play-time"><%= timeSpan.ToString(@"mm\:ss") %></i>
                        </em>
                    </a>
                </li>
            <% } %>
        </ul>
        <input type="hidden" id="ids" value="<%= ids.Length > 0 ? ids.Substring(0, ids.Length - 1) : "" %>"/>
    </div>
<% } %>
<!-- 视频 end -->
<!-- 论坛热帖 start -->
<%= _forumNewsHtml %>
<!-- 论坛热帖 end -->
<ins id="div_bec90be3-b648-4cd9-8c33-96c6be62451d" type="ad_play" adplay_IP="" adplay_AreaName="" adplay_CityName="" adplay_BrandID="<%= _serialId %>" adplay_BrandName="" adplay_BrandType="" adplay_BlockCode="bec90be3-b648-4cd9-8c33-96c6be62451d"> </ins>
<a name="a-dealer"></a>
<script type="text/javascript">
    //app下载设置全局变量
    var global_app_download = false;
    //showNewsInsCode('7246b970-9063-47c3-b140-c9104515047e', '03011683-4c06-42b0-8f97-17687d814a7b', 'c392b629-2113-4dc0-8a2f-4ee04d6a561d', '9b714349-7d34-4389-be52-67985391a3cd');
    var citycode = 201, cityName = '北京';
    if (typeof(bit_locationInfo) != "undefined" && typeof(bit_locationInfo.cityId) != "undefined") {
        citycode = bit_locationInfo.cityId;
        cityName = bit_locationInfo.cityName;
    }
    $("#m-hangqing-city").html(cityName + "<i></i>");
    var csId = <%= _serialId %>;
    document.write('<scri' + 'pt src="http://m.h5.qiche4s.cn/priceapiv2/ajax/InitData.ashx?action=cardealerlist&brandid=<%= _serialId %>&citycode=' + citycode + '" type="text/javascript"></s' + 'cript>');
</script>
<!--贷款 start-->
<%--<div id="m-loan-title" class="tt-first" style="display: none;" data-channelid="27.23.739">
			<h3>贷款推荐</h3>
			<div class="opt-more opt-more-gray"><a href='http://chedai.m.yiche.com/<%= _serialAllSpell %>/chedai/?from=ycm37'>更多</a></div>
		</div>
		<div id="m-loan-con" class="m-loan" style="display: none;" data-channelid="27.23.740">
			<ul></ul>
		</div>--%>
<!--贷款 end-->
<!--养护 start-->
<%--<div id="yanghu"></div>--%>
<!--养护 end-->
<!--看了还看 start-->
<%= _serialToSee %>
<!--看了还看 end-->
<!--最近看过 start-->
<div class="browse-car  b-shadow" id="more1" data-channelid="27.23.744">
</div>
<!--最近看过 end-->
<!-- black popup start -->
<%--<div class="leftmask leftmask4" style="display: none;"></div>
		<div id="provinceList" class="leftPopup province" data-back="leftmask3" style="z-index: 40; display: none;">
		</div>
		<div id="cityList" class="leftPopup city" data-back="leftmask3" style="z-index: 50; display: none;">
			<div class="swipeLeft"></div>
		</div>--%>
<!--省份层 start-->
<div class="leftmask leftmask6" style="display: none;"></div>
<div id="provinceList" class="leftPopup province year" data-back="leftmask6" style="display: none;">
    <div class="swipeLeft swipeLeft-sub">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<!--省份层 end-->
<!--市层 start-->
<div id="cityList" class="leftPopup month model2 city" data-back="leftmask6" style="display: none;">
    <div class="swipeLeft swipeLeft-sub">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<!--市层 end-->
<!-- black popup end -->
<script type="text/javascript">
    //showNewsInsCode('893f9a91-0f1e-484c-8263-37d815e64d1b', '09dc099f-5b52-4a70-8edd-faa0d7ff4818', '469f45cc-1b72-461c-ba68-0b560c885455', 'cd11763a-ea3e-4315-b054-8baecd23cab8');
    showNewsInsCode('d919899b-178d-42c6-bfdf-13d8fe743450', 'e570bef3-1939-4012-9c2b-1de99256e34b', 'f034550f-7b8c-47f4-b6f3-63b7fb1026d3', '4a871eb9-835d-4207-a356-a035198b2035');
    var CarCommonCSID = '<%= _serialId.ToString(CultureInfo.InvariantCulture) %>';
</script>
<!--#include file="/include/pd/2016/wap/00001/201606_wap_common_ydyy_Manual.shtml" -->
<%--delete by zf 20160104
            <!--弹出层 start-->
        <div class="leftmask leftmaskduibibtn" style="display: none;"></div>
        <div class="leftPopup model" data-back="leftmaskduibibtn" data-zindex="777777" style="display: none;">
            <div class="swipeLeft">
                <div class="loading">
                    <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" />
                    <p>正在加载...</p>
                </div>
            </div>
        </div>
        <!--弹出层 end-->--%>
<!--一级公共模板 start-->
<script type="text/template" id="duibibtnTemplate">
            <div class="y2015-car-01">
                <div class="slider-box">
                    <ul class="first-list">
                        {for(var i =0;i< list.SelectedCars.length;i++){}
                       <li>
                           <div class="line-box"><a class='select' href="/m{= list.SelectedCars[i].CarId }/">{=list.SelectedCars[i].CarName }</a><a href="javascript:;" data-carid="{=list.SelectedCars[i].CarId }" class="btn-close"><i></i></a></div>
                       </li>
                        {}}
                    {for(var i =0;i< list.AddLables.Count;i++){}
                        <li class="add">
                            <div class="line-box"><a href="javascript:;">添加对比车款</a></div>
                        </li>
                        {}}
                    <li class="alert">最多对比4个车款</li>
                    </ul>
                </div>
            </div>
            <div class="swipeLeft-header">
                <a href="###" class="btn-clear">清除</a>
                <!--btn-clear disable-->
                <a href="###" class="btn-comparison">开始对比</a>
                <!--btn-comparison disable-->
                <div class="clear"></div>
            </div>
        </script>
<!--一级公共模板 end-->
<div class="leftmask duibi-leftmask leftmask2 " style="display: none;"></div>
<div class="leftPopup duibicar duibi-leftPopup" data-zindex="777777" data-back="leftmask2" style="display: none;">
    <div class="swipeLeft">
        <div class="y2015-car-01">
            <ul class="first-list">

            </ul>
        </div>
        <div class="swipeLeft-header">
            <a href="###" class="btn-clear">清空</a>
            <!--btn-clear disable-->
            <a href="###" class="btn-comparison">开始对比</a>
            <!--btn-comparison disable-->
            <div class="clear"></div>
        </div>
    </div>
</div>
<div class="float-r-box">
    <%--<a target="_blank" href="http://www.sojump.hk/jq/8992509.aspx" class="gg-float gg-float-show" id="ggFloat"><img src="http://image.bitautoimg.com/uimg/mbt2015/images/ico_float_wenjuan.png" /></a>--%>
    <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml "-->
    <%--<a id="compare-pk" data-channelid="27.23.747"  href="#compare"  data-action="duibicar" class="float-r-ball float-pk">
				<span><p>对比</p></span>
				<i></i>
			</a>--%>
</div>
<script type="text/javascript">
    showNewsInsCode('893f9a91-0f1e-484c-8263-37d815e64d1b', '09dc099f-5b52-4a70-8edd-faa0d7ff4818', '469f45cc-1b72-461c-ba68-0b560c885455', 'cd11763a-ea3e-4315-b054-8baecd23cab8');
</script>
<div class="footer15">
    <!--搜索框-->
    <!--#include file="~/html/footersearch.shtml"-->
    <div class="breadcrumb">
        <div class="breadcrumb-box">
            <a href="http://m.yiche.com/">首页</a> &gt; <a href="http://car.m.yiche.com/brandlist/">选车</a> &gt; <a href="http://car.m.yiche.com/brandlist/<%= _serialEntity.Brand.MasterBrand.AllSpell %>/"><%= _serialEntity.Brand.MasterBrand.Name %></a> &gt; <span><%= _serialShowName %></span>
        </div>
    </div>
    <!--#include file="~/html/footerV3.shtml"-->
</div>
</div>
<!--新加弹出层 start-->
<div id="master_container" style="display: none; z-index: 888888;" class="brandlayer mthead">
    <!--#include file="~/html/compareCarTemplate.html"-->
</div>
<!--新加弹出层 end-->
<!--弹出层 start-->
<div class="leftmask leftmaskcondition" style="display: none;"></div>
<div class="leftPopup stopyear" data-back="leftmaskcondition" style="display: none;">
    <div class="swipeLeft">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<div class="leftPopup  level" data-back="leftmaskcondition" style="display: none;">
    <div class="swipeLeft">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<div class="leftPopup bodyform" data-back="leftmaskcondition" style="display: none;">
    <div class="swipeLeft">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
</div>
<!--弹出层 end-->
<!--loading模板 start -->
<div class="template-loading" style="display: none;">
    <div class="loading">
        <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
        <p>正在加载...</p>
    </div>
</div>
<!--loading模板 end -->
<!--音频模板 start -->
<script type="text/lazyT-template" id="template_audios">
                {#~ D:item:index #}    
					<div class="talk-car jp-audio" id="audio_{#=item.AudioId#}" role="application" aria-label="media player">
						<div class="jp-jplayer" data-urls="{#=item.PlayLinks.join(',')#}" data-id="audio_{#=item.AudioId#}"></div>
						<div class="talk-img"><img src="{#=item.AuthorImage#}"></div>
						<div class="talk-time">
							<p>{#=item.AuthorName#}{#=item.Introduce#}</p>
							<div class="talk-v-box jp-type-single">
								<div class="talk-arrow"></div>
								<div class="jp-progress" data-duration="{#=item.Duration#}">
									<div class="talk-outer jp-seek-bar" style="width: 100%;">
										<div class="talk-inner jp-play-bar" style="width: 0%;"></div>
									</div>
								</div>
								<div class="ico-talk-voice"></div>
								<div class="jp-controls">
									<div class="ico-talk-play"></div>
									<button class="jp-play" role="button" tabindex="0" data-channelid="27.23.1365"></button>
								</div>
								<div class="time-box jp-current-time" role="timer" aria-label="time">00:00</div>
								<div class="time-box jp-duration" role="timer" aria-label="duration">{#=item.DurationString#}</div>
							</div>
						</div>
						<div class="clear"></div>
					</div>
                {#~#}
</script>
<!--音频模板 end -->
<!--公共模板 start-->
<script type="text/template" id="commonTemplate">
        <div class="ap">
            <ul class="y2015">
            <li><a href="#" data-value="不限">不限</a></li>
            { for(var j = 0 ; j < SubItem.length ; j++){ }
            <li {=(SubItem[j].toString()==selSubStopYear||SubItem[j].toString()==selSubLevel||SubItem[j].toString()==selSubBodyForm)?"class='current'":""}><a href="#" data-value="{=SubItem[j].toString().replace('款','')}">{=SubItem[j]}</a></li>
            {}}
            </ul>
        </div>
    </script>
<!--公共模板 end-->
<!--省模板start-->
<script type="text/template" id="provinceTemplate">
        <dl class="tt-list">
            { for(var n in provinces){}
            <dt><span>{=n}</span></dt>
                {for(var i=0;i<provinces[n].length;i++){}
                <dd>
                    <a href="#" {=provinces[n][i].children ? 'data-action="city"':''}  data-id="{=provinces[n][i].id}" class="{=provinces[n][i].children ? '':'nbg'} {= provinces[n][i].id.toString() == current_province.toString()  ? 'current':''}">
                        <p>{=provinces[n][i].name}</p>
                    </a>
                </dd>
               {}}
            {}}
        </dl>
    </script>
<!--省模板end-->
<!--市模板start-->
<script type="text/template" id="cityTemplate">
        <div class="ap">
            <ul class="first-list rp">
                <li class="root"><a>安徽</a></li>
                {for(var i=0;i  <citys.length;i++){}
                    <li><a data-id="{=citys[i].id}" class="{= citys[i].id.toString() == current_city.toString()  ? 'current':''}">{=citys[i].name}</a></li>
                {}}
            </ul>
        </div>
    </script>
<!--市模板end-->
<!--loading模板 start -->
<div class="template-loading" style="display: none;">
    <div class="loading">
        <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
        <p>正在加载...</p>
    </div>
</div>
<!--loading模板 end -->
<!-- 未上市 start -->
<%--  <div class="bottom-float" id="bottomFloatOfWaitSale" style="display:none;">
        <ul>
            <li class="three-item item-duibi" id="itemDuibi"><a id="compare-pk" data-channelid="27.23.747"  href="javascript:;"  data-action="duibicar"><em id="duibiAti"></em>对比<i></i></a></li>
            <li class="three-item item-xinche"><a href="javascript:;"><em></em>买新车</a></li>
            <li class="three-item item-daikuan"><a href="javascript:;"><em></em>贷款</a></li>
            <li class="xunjia-btn"><a href="javascript:;">询底价</a></li>
        </ul>
    </div>
    <!-- 未上市 end -->
    <!-- 停售 start -->
    <div class="bottom-float" id="bottomFloatOfNoSale" style="display:none;">
        <ul>
            <li class="two-item item-duibi" id="itemDuibi"><a id="compare-pk" data-channelid="27.23.747"  href="#compare"  data-action="duibicar"><em id="duibiAti"></em>对比<i></i></a></li>
            <li class="two-item item-ershouche"><a href="http://m.taoche.com/pinggu/?ref=mchexizsgu"><em></em>二手车估价</a></li>
            <li class="xunjia-btn"><a href="http://m.taoche.com/<%=_serialAllSpell %>/?ref=mchexizsmai&leads_source=m002014">买二手车</a></li>
        </ul>
    </div>--%>
<!-- 停售 end -->
<!-- 正常 end -->
<div class="bottom-float" id="bottomFloat" style="display: none">
    <ul>
        <% if (_serialEntity.SaleState == "停销")
           { %>
            <li class="two-item item-duibi" id="itemDuibi">
                <a id="compare-pk" data-channelid="27.23.747" href="#compare" data-action="duibicar"><em id="duibiAti"></em>对比<i></i></a>
            </li>
            <!--#include file="~/include/pd/2016/yipaicms/00001/201701_Mobile_Summary_SCInfoBtn_Manual.shtml"-->
        <% }
           else if (_serialEntity.SaleState == "在销")
           { %>
            <li class="three-item item-duibi" id="itemDuibi">
                <a id="compare-pk" data-channelid="27.23.747" href="#compare" data-action="duibicar"><em id="duibiAti"></em>对比<i></i></a>
            </li>
            <!--#include file="~/include/pd/2016/yipaicms/00001/201701_Mobile_Summary_SCInfoBtn_Manual.shtml"-->
        <% }
           else
           { %>
            <li class="three-item item-duibi" id="itemDuibi">
                <a id="compare-pk" data-channelid="27.23.747" href="javascript:;" data-action="duibicar"><em id="duibiAti"></em>对比<i></i></a>
            </li>
            <li class="three-item item-xinche item-xinche-none">
                <a href="javascript:;"><em></em>买新车</a>
            </li>
            <li class="three-item item-daikuan item-daikuan-none">
                <a href="javascript:;"><em></em>贷款</a>
            </li>
            <!--#include file="~/include/pd/2016/yipaicms/00001/201701_Mobile_Summary_SCInfoBtn_Manual.shtml"-->
            <li class="xunjia-btn xunjia-btn-none">
                <a href="javascript:;">询底价</a>
            </li>
        <% } %>
        
    </ul>
</div>
<!-- 正常 end -->
<!-- 买买买 层 start -->
<div class="leftmask leftmask1" style="display: none;"></div>
<div class="leftPopup mmm" data-back="leftmask1" style="display: none;">
    <div class="swipeLeft">
        <div class="ap">
            <div class="mmm-none">
                <div class="mmm-none-bg"></div>
                <p>非常抱歉，暂无优惠信息</p>
            </div>
            <div class="mmm-pop b-shadow" id="m-car-bijia" style="display: none" data-channelid="27.23.1335"></div>
            <div class="mmm-pop b-shadow" id="m-car-zhixiao" style="display: none" data-channelid="27.23.1336"></div>
            <div class="mmm-pop b-shadow" id="m-car-ych" style="display: none" data-channelid="27.23.1337"></div>
            <div class="mmm-pop b-shadow" id="m-car-daikuan" style="display: none" data-channelid="27.23.1338"></div>
            <div class="mmm-pop b-shadow" id="m-car-tuan" style="display: none" data-channelid="27.23.1339"></div>
        </div>
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif"/>
            <p>正在加载...</p>
        </div>
    </div>
</div>

<!-- 买买买 层 end -->
<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/model.js?v=2016011315"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/addcompare.js?v=201510271146"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompare.js?v=20151117"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll.js?v=20150828"></script>--%>
<script type="text/javascript">
    var _csAllSpell = '<%= _serialAllSpell %>';
    $("a.btn-menu span").on("click", function(event) {
        event.preventDefault();
        var menupop = $(this).parent().siblings("div.menu-pop");
        if ($(menupop).css("display") == "none") {
            $(menupop).css("display", "block");
            $(this).parent("a.btn-menu").addClass("btn-menu-show");
        } else {
            $(menupop).css("display", "none");
            $(this).parent("a.btn-menu").removeClass("btn-menu-show");
        }
    });

    $(document).on('touchstart', function(event) {
        event.stopPropagation();
        var clickEle = $(event.target).closest(".menu-pop").attr("id");
        if (clickEle != 'h_popNav' && $(event.target).closest(".btn-menu").attr("id") != "h_nav") {
            $("#h_popNav").hide();
            $("#h_nav").removeClass("btn-menu-show");
        }
        //if(clickEle != 'f_popNav' && $(event.target).closest(".btn-menu").attr("id") != "f_nav"){
        //	$("#f_popNav").hide();
        //	$("#f_nav").removeClass("btn-menu-show")
        //}
    });

    $(document).ready(function() {
        var documentHeight = $(document).height();
        $('#pSearch').css('height', documentHeight);
    });
</script>
<% if (_yearCount > 0)
   { %>
    <script type="text/javascript">
        // 临时统计方法 
        function statForTempString(objid, str1, str2) {
            var _sentImg = new Image(1,
                1);
            _sentImg.src = "http://carstat.bitauto.com/weblogger/img/c.gif?logtype=temptypestring&objid="
                + objid + "&str1=" + encodeURIComponent(str1) + "&str2=" + encodeURIComponent(str2)
                + "&" + Math.random();
        }

        var year = "all";
        var divNum = 0;

        //(function() {
        //    var tabs = document.getElementById("yeartag").getElementsByTagName("li");
        //    for (var i = 0; i < tabs.length; i++) {
        //        tabs[i].onclick = function() {
        //            if (this.className == 'current') return false;
        //            var x = 0;
        //            for (var j = 0; j < tabs.length; j++) {
        //                if (tabs[j] == this) {
        //                    x = j;
        //                } else {
        //                    tabs[j].className = "";
        //                    document.getElementById(("yearDiv" + j)).style.display = "none";
        //                }
        //            }
        //            this.className = "current";
        //            document.getElementById(("yearDiv" + x)).style.display = "";
        //            divNum = x;
        //        };
        //    }
        //}
        //)();
        //还原筛选项的值 
        function resetOptions() {
            $("[data-action=level]").removeClass("current").find("span").text("排量");
            $("[data-action=bodyform]").removeClass("current").find("span").text("变速箱");
            selSubStopYear = '', selSubLevel = '', selSubBodyForm = '';
        }
    </script>
<% } %>

<%--<!--导航开始-->
	<!--#include file="~/include/pd/2012/wap/00001/201503_wap_zsy_cd_js_Manual.shtml" -->--%>
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
 	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/csSummaryV2.min.js?v=2016101814"> </script>--%>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/common/cityjsonv2.js?v=2016012210"> </script>--%>

<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js?v=20130606"> </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/city348IDMapName.js?v=20140903"> </script>
    <script type="text/javascript" src="/Js/csSummaryV2.js"> </script>--%>
<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/commonjs.js,carchannel/wirelessjs/city348IDMapName.js,carchannel/wirelessjs/cssummaryv2.min.js?v=20170303"></script>
<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"> </script>
<script src="http://image.bitautoimg.com/uimg/wap/js/idangerous.swiper-2.0.min.js"></script>
<script type="text/javascript">
    <%--// 默认元素ID "m-car-nav"
    CarNavForWireless.DivID = "m-car-nav";
    // 导航当前标签索引(0:综述,1:配置,2:图片,3:油耗,4:详解,5:口碑,6:视频,7:论坛)
    CarNavForWireless.CurrentTagIndex = 0;
    //add by sk 2016.03.29 temp
    try
    { mainWirelessPVStatisticFunction(<%=_serialId%>, 0, this.CurrentTagIndex); }
    catch (err) { }--%>

    //行情
    //loadNewsHangqingByCity(<%= _serialId %>, '<%= _serialShowName %>', citycode, 4);
    mCsSummaryV2.loadNewsHangqingV2(<%= _serialId %>, '<%= _serialShowName %>', citycode, 4);
    mCsSummaryV2.loadSubsidy(<%= _serialId %>, citycode);
    //特卖
    getDemandAndJiangJia(<%= _serialId %>, '<%= _serialAllSpell %>', bit_locationInfo.cityId);
    // 浏览过的车型
    Bitauto.Login.onComplatedHandlers.push(function(loginResult) {
        Bitauto.UserCars.setUserLoginState({
            isLogin: loginResult.isLogined
        });
        //添加浏览过的车
        Bitauto.UserCars.addViewedCars(<%= _serialId %>);
    });
    var viewedList = "";
    for (var i = 0; i < Bitauto.UserCars.viewedcar.arrviewedcar.length && i < 4; i++) {
        viewedList += Bitauto.UserCars.viewedcar.arrviewedcar[i];
        if (i != Bitauto.UserCars.viewedcar.arrviewedcar.length - 1) {
            viewedList += ",";
        }
    }
    loadViewedCars(viewedList);
    //加统计代码
    addTrackingCode(<%= _serialId %>);
</script>
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"> </script>
<%--<script type="text/javascript" src="http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csid=<%= _serialId %>&op=GetCsForWireless&callback=CarNavForWireless.GenerateNav"
		charset="utf-8"> </script>--%>
<!-- footer begin -->
<script type="text/javascript" src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js"></script>
<script type="text/javascript" src="http://log.bitauto.com/easypassweblog/TelCallTracing/script/telcalltracing.js"> </script>
<script type="text/javascript">
    var curSerialId = <%= _serialId %>;
    var apiBrandId = '<%= _serialEntity.BrandId %>';
    var apiSerialId = curSerialId;
    var apiCarId = '';

    var pcount = <%= pageCount %>;
    var nearestYear = '<%= nearestYear %>';
    var tabs = document.getElementById("yeartag").getElementsByTagName("li");
    var btnMore = {};
</script>
<!--include file="~/html/uploadaladingapp.shtml"-->
<script type="text/javascript">
    // 看了还看
    if ($("#m-tabs-kankan li").length >= 2) {
        var tabsSwiperKankan = new Swiper('.swiper-container-kankan', {
            calculateHeight: true,
            loop: true,
            onInit: function(swiper) {
                $("#m-tabs-kankan li").eq((swiper.activeIndex - 1) % (swiper.slides.length - 2)).addClass('current');
            },
            onSlideChangeEnd: function(swiper) {
                $("#m-tabs-kankan .current").removeClass('current');
                $("#m-tabs-kankan li").eq((swiper.activeIndex - 1) % (swiper.slides.length - 2)).addClass('current');
            }
        });
        $("#m-tabs-kankan li").on('touchstart mousedown', function(e) {
            e.preventDefault();
            $("#m-tabs-kankan .current").removeClass('current');
            $(this).addClass('current');
            tabsSwiperKankan.swipeTo($(this).index());
        });
        $("#m-tabs-kankan li").click(function(e) {
            e.preventDefault();
        });
    }
    // 应用下载
    var mySwiperApp = new Swiper('#m-app-part-scroll', {
        pagination: '.pagination-app',
        loop: true,
        grabCursor: true,
        paginationClickable: true
    });

    $(document).ready(function() {
        if ($("#m-app-part-scroll").find("li").length < 4) {
            $(".pagination-app").hide();
        }
    });

</script>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/stat/PageAreaStatistics.js"> </script>--%>
<script type="text/javascript">
    var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
        modelStr = '<%= _serialId %>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
<script type="text/javascript">
    //登录 车型关注
    function initLoginFavCar(carLoginresult) {
        if (carLoginresult.isLogined) {
            try {
                var added = false;
                if (typeof carLoginresult != 'undefined' && typeof carLoginresult.plancar != 'undefined' && carLoginresult.plancar.length > 0) {
                    for (var i = 0; i < carLoginresult.plancar.length; i++) {
                        if (carLoginresult.plancar[i].CarSerialId == '<%= _serialId %>') {
                            added = true;
                            $("#favstar").html("<i></i>已关注").addClass("ico-favorite ico-favorite-sel");
                            break;
                        }
                    }
                }
                if (!added) {
                    var hash;
                    hash = window.location.hash;
                    if (hash && hash == "#add") {
                        var obj = $("#favstar");
                        FocusCar(obj);
                        location.hash = "";
                    }
                }
                $("#favstar").bind("click", function() {
                    FocusCar($(this));
                });
            } catch (e) {
            }
        } else {
            //location.hash = "add";
            $("#favstar").attr("href", 'http://i.m.yiche.com/authenservice/login.aspx?returnUrl=' + encodeURIComponent(location.href) + '#add');
        }
    }

    //添加 取消关注车型
    function FocusCar(obj) {
        var id = <%= _serialId %>;
        obj.attr('class') == "ico-favorite" ? Bitauto.UserCars.addConcernedCar(id, function() {
            if (Bitauto.UserCars.concernedcar.message[0] == "已超过上限") {
                alert("关注数量已达上限");
            } else {
                obj.addClass("ico-favorite ico-favorite-sel").html("<i></i>已关注");
                Bitauto.UserCars.concernedcar.arrconcernedcar.unshift(id);

            }
        }) : Bitauto.UserCars.delConcernedCar(id, function() {
            obj.removeClass("ico-favorite-sel").html("<i></i>关注");
        });
    }

    $(function() {
        Bitauto.Login.onComplatedHandlers.add("memory once", initLoginFavCar);
    });
</script>
<script type="text/javascript">
    $(function() {
        var ids = $("#ids").val();
        if ($("#ids").length > 0 && ids.length > 0) {
            $.ajax({
                url: "http://v.bitauto.com/vbase/CacheManager/GetVideoTotalVisitCommentCountByIds?ids=" + ids,
                async: false,
                dataType: "jsonp",
                jsonpCallback: "successHandler",
                cache: true,
                success: function(data) {
                    $(data).each(function(index, item) {
                        $("#viewcount_" + item.VideoId).html(item.TotalVisit);
                    });
                }
            });
        }
    })
</script>
<script type="text/javascript">
    <%--if ($('#focusNews2608').length > 0 && $('#carList2608').length > 0) {
        PageAreaStatistics.init("36,37,38,39,40,41,42,43,44,128,152,182,183,184,185,186");
    } else {
        PageAreaStatistics.init("36,37,38,39,40,41,42,43,44,128,152,182,183,184,185,186,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,342,350,351");
    }--%>
    function dateFormat(dateString, format) {
        if (!dateString)return "";
        var time = new Date(dateString.replace(/-/g, '/').replace(/T|Z/g, ' ').trim());
        var o = {
            "M+": time.getMonth() + 1, //月份
            "d+": time.getDate(), //日
            "h+": time.getHours(), //小时
            "m+": time.getMinutes(), //分
            "s+": time.getSeconds(), //秒
            "q+": Math.floor((time.getMonth() + 3) / 3), //季度
            "S": time.getMilliseconds() //毫秒
        };
        if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (time.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format)) format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return format;
    }

    //去除字符串头部空格或指定字符  
    String.prototype.TrimStart = function(c) {
        if (c == null || c == "") {
            var str = this.replace(/^\s*/, '');
            return str;
        } else {
            var rg = new RegExp("^" + c + "*");
            var str = this.replace(rg, '');
            return str;
        }
    };
    //判断json对象（返回格式:{}）是否为空
    var isEmptyObject = function(obj) {
        var name;
        for (name in obj) {
            return false;
        }
        return true;
    };

    function callCommonMethod(url, dataType, callBackName, callBackFunc) {
        $.ajax({
            url: url,
            async: false,
            cache: false,
            dataType: dataType,
            jsonpCallback: callBackName,
            success: function(data) {
                callBackFunc(data);
            }
        });
    }

    //贷款
    function getDaikuan(cityCode, curCarId, $leftPopup) {
        var url = 'http://carapi.chedai.bitauto.com/api/SummarizeFinancialProducts/SearchCityCarProduct?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
        callCommonMethod(url, "jsonp", "callgetDaikuan", function(data) {
            var h = [];
            var dataStr = eval("(" + data + ")");
            var flag = isEmptyObject(dataStr);
            if (dataStr && !flag) {
                h.push("<a href=\"" + dataStr.MDetailsUrl + "\"><img src=\"" + dataStr.CarImageUrl + "\"></a>");
                h.push("<h6>" + dataStr.PackageName + "</h6>");
                h.push("<div class=\"two-line\">");
                h.push("<p>首付：<strong>" + dataStr.DownPaymentText + "</strong></p>");
                h.push("<p>月供：<strong>" + dataStr.MonthlyPaymentText + "</strong></p>");
                h.push("<a href=\"" + dataStr.MDetailsUrl + "\" class=\"btn-mmm\">立即申请</a>");
                h.push("</div>");
                h.push("<div class=\"mmm-tit-box mmm-daikuan\">");
                h.push("<span>贷款买</span><i></i>");
                h.push("</div>");
                $('#m-car-daikuan').html(h.join('')).show();
                $('.mmm-none').hide();
                $leftPopup.find('.loading').hide();
            } else {
                $('#m-car-daikuan').hide();
                $leftPopup.find('.loading').hide();
            }
        });
    }

    //团购
    function getTuanGou(cityCode, curSerialId, $leftPopup) {
        var url = 'http://platform.api.huimaiche.com/m/tg/product/item?cityid=' + cityCode + '&serialid=' + curSerialId; //curSerialId
        callCommonMethod(url, "jsonp", "callgetTuanGou", function(data) {
            var h = [];
            var flag = isEmptyObject(data);
            if (data && !flag) {
                h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
                h.push("<h6>" + data.Description + "</h6>");
                h.push("<div class=\"two-line\">");
                h.push("<p>已报名：<strong>" + data.BuyerAmount + "人</strong></p>");
                h.push("<p>开团日：<strong>" + dateFormat(data.PurchaseTime, "MM").TrimStart('0') + "月" + dateFormat(data.PurchaseTime, "dd").TrimStart('0') + "日" + "</strong></p>");

                h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">立即报名</a>");
                h.push("</div>");
                h.push("<div class=\"mmm-tit-box mmm-tuangou\">");
                h.push("<span>团购</span><i></i>");
                h.push("</div>");
                $('#m-car-tuan').html(h.join('')).show();
                $leftPopup.find('.loading').hide();
                $('.mmm-none').hide();
            } else {
                $('#m-car-tuan').hide();
                $leftPopup.find('.loading').hide();
            }
        });
    }

    //直销
    function getDirectSell(cityCode, curCarId, $leftPopup) {
        var url = 'http://platform.api.huimaiche.com/m/zx/product/directsellingitem?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
        callCommonMethod(url, "jsonp", "callgetDirectMall", function(data) {
            var h = [];
            var flag = isEmptyObject(data);
            if (data && !flag) {
                h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
                h.push("<h6>" + data.ProductShowName + "</h6>");
                h.push("<div class=\"one-line\">");
                h.push("<p>直销价：<strong>" + data.DirectSalePrice + "</strong></p>");
                h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">立即抢购</a>");
                h.push("</div>");
                h.push("<div class=\"mmm-tit-box mmm-zhixiao\">");
                h.push("<span>直销价</span><i></i>");
                h.push("</div>");
                $('#m-car-zhixiao').html(h.join('')).show();
                $leftPopup.find('.loading').hide();
                $('.mmm-none').hide();
            } else {
                $('#m-car-zhixiao').hide();
                $leftPopup.find('.loading').hide();
            }
        });
    }

    //比价买
    function getComparisonBuy(cityCode, curCarId, $leftPopup) {
        var url = 'http://platform.api.huimaiche.com/m/c2b/product/item?cityid=' + cityCode + '&carid=' + curCarId; //curCarId
        callCommonMethod(url, "jsonp", "callgetComparisonBuy", function(data) {
            var h = [];
            var flag = isEmptyObject(data);
            if (data && !flag) {
                h.push("<a href=\"" + data.WapUrl + "\"><img src=\"" + data.WapPicUrl + "\"></a>");
                h.push("<h6>" + data.ProductShowName + "</h6>");
                h.push("<div class=\"one-line\">");
                h.push("<p><strong>" + data.Description + "</strong></p>");
                h.push("<a href=\"" + data.WapUrl + "\" class=\"btn-mmm\">获取底价</a>");
                h.push("</div>");
                h.push("<div class=\"mmm-tit-box mmm-bijiamai\">");
                h.push("<span>比价买</span><i></i>");
                h.push("</div>");
                $('#m-car-bijia').html(h.join('')).show();
                $leftPopup.find('.loading').hide();
                $('.mmm-none').hide();
            } else {
                $('#m-car-bijia').hide();
                $leftPopup.find('.loading').hide();
            }
        });
    }

    //优惠券
    function getCoupon(cityCode, curCarId, $leftPopup) {
        var url = "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=wapchexingye&cityid=" + cityCode + "&carid=" + curCarId; //curCarId
        callCommonMethod(url, "jsonp", "callgetCoupon", function(data) {
            var h = [];
            var result = data.data;
            var flag = isEmptyObject(result);
            if (result && !flag && data.success == true) {
                h.push("<a href=\"" + result.murl + "\"><img src=\"" + result.imgurl4 + "\"></a>");
                h.push("<h6>" + result.pb_name + "</h6>");
                h.push("<div class=\"one-line\">");
                h.push("<p><strong>" + result.p_deposit + "</strong></p>");
                h.push("<a href=\"" + result.murl + "\" class=\"btn-mmm\">立即抢购</a>");
                h.push("</div>");
                h.push("<div class=\"mmm-tit-box mmm-youhuiquan\">");
                h.push("<span>" + result.channelm + "</span><i></i>");
                h.push("</div>");
                $('#m-car-ych').html(h.join('')).show();
                $leftPopup.find('.loading').hide();
                $('.mmm-none').hide();
            } else {
                $('#m-car-ych').hide();
                $leftPopup.find('.loading').hide();
            }
        });
    }

    // 买买买 弹出层事件
    var _bindBtnMaiEvent = function() {
        var $body = $('body');
        $('.btn-mmm').click(function(ev) {
            var $click = $(this);
            ev.preventDefault();
            $body.trigger('fristSwipeOneNb', {
                //fristSwipeOneNb 一级不带按钮控件
                $swipe: $body.find('.mmm'), //弹出浮层
                $click: $click, //点击对象
                fnEnd: function() {
                    //层打开后回调
                    var $leftPopup = this;
                    //获取填充数据
                    var curCarId = $click.data("car");
                    var html = '';
                    $leftPopup.find('.ap').show();
                    $leftPopup.find('.mmm-none').show();
                    getComparisonBuy(citycode, curCarId, $leftPopup);
                    getDirectSell(citycode, curCarId, $leftPopup);
                    getCoupon(citycode, curCarId, $leftPopup);
                    getDaikuan(citycode, curCarId, $leftPopup);
                    getTuanGou(citycode, curSerialId, $leftPopup);
                },
                closeEnd: function() {
                    //关闭层回调
                    var $leftPopup = this;
                    $leftPopup.find('.loading').show();
                    $leftPopup.find('.mmm-pop').hide();
                    $leftPopup.find('.ap').hide();
                }
            });
        });
    };
    $(function() {
        // 底部浮动层
        var curSerialSaleState = '<%= _serialEntity.SaleState %>';
        var screenHeight = $(window).height();
        $(window).bind("scroll", function() {
            if ($(window).scrollTop() > screenHeight - 300) {
                $('#bottomFloat').show();
                $('#carNavFixed').show();
            } else {
                $('#bottomFloat').hide();
                $('#carNavFixed').hide();
            }
        });

        //空间详情
        $(".scroll-card .car-info-txt").find("em").on("click", function(e) {
            e.stopPropagation();
            $(this).find(".pop-box").show();
        });
        $(".scroll-card .car-info-txt").find(".pop-box").on("click", function(e) {
            e.stopPropagation();
            $(this).hide();
        });
        // 买买买 弹出层
        _bindBtnMaiEvent();
    });
</script>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/iscroll.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/underscore.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=20160128"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/rightswipe.js?v=20160128"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/note.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/brand.js?v=20160118"></script>
   <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompareV2.js?v=2016011818"></script>--%>

<%--    <script type="text/javascript" src="/js/waitcompareV2.js"></script>
    <script type="text/javascript" src="/js/rightswipe-cityV2.js?v=20160202"></script>--%>
<%--<script type="text/javascript" src="/js/csSummaryConditionV2.js"></script>--%>
<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/common/swiper-3.2.7.min.js,carchannel/wirelessjs/v2/iscroll.min.js,carchannel/wirelessjs/v2/underscore.min.js,carchannel/wirelessjs/v2/model.min.js,carchannel/wirelessjs/v2/rightswipe.min.js,carchannel/wirelessjs/v2/note.min.js,carchannel/wirelessjs/v2/brand.js,carchannel/wirelessjs/waitcompareV2.min.js,carchannel/common/cityjsonv2.js,carchannel/wirelessjs/rightswipe-cityv2.js,carchannel/wirelessjs/cssummaryconditionV2.min.js?v=20170105"></script>

<%--    <script src="http://image.bitautoimg.com/uimg/wap/js/idangerous.swiper-2.0.min.js"></script>--%>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/common/swiper-3.2.7.min.js"></script>--%>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/jquery.jplayer.min.js?v=2016050614"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/audioJPlayer.js?v=2016050614"></script>--%>
<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=audio/jplayer/jquery.jplayer.min.js,audio/jplayer/audioJPlayerMobile.js?v=20170105"></script>
<script type="text/javascript">
    var compareConfig = {
        serialid: CarCommonCSID
    };
    $(function() {
        WaitCompare.initCompreData(compareConfig);
        //RigthSwipeCity.initCity();
    });
    // 信息卡片滑动
    //var swiperImgBox = new Swiper('.swiper-img-box', {
    //    onSlideChangeStart: function () {
    //        $("#kongjianTab .current").removeClass('current');
    //        $("#kongjianTab li").eq(swiperImgBox.activeIndex).addClass('current');

    //        $("#kongjianList ul").hide();
    //        $("#kongjianList ul").eq(swiperImgBox.activeIndex).show();
    //    }
    //});
    var swiperInfoCard = new Swiper('.swiper-info-card', {
        pagination: '.pagination-info-card',
        slidesPerView: 'auto',
        paginationClickable: true,
        spaceBetween: 15,
    });
    $("#kongjianTab li").on('touchstart mousedown', function(e) {
        e.preventDefault();
        $("#kongjianTab .current").removeClass('current');
        $(this).addClass('current');
        $('.swiper-img-box li').hide();
        $('.swiper-img-box li').eq($(this).index()).show();

        $('#kongjianList ul').hide();
        $('#kongjianList ul').eq($(this).index()).show();
    });
    $("#kongjianTab li").click(function(e) {
        e.preventDefault();
    });

    // 文章切换
    var swiperNewslist = new Swiper('.swiper-container-newslist', {
        autoHeight: true,
        onSlideChangeStart: function() {
            $("#newslistTab .current").removeClass('current');
            $("#newslistTab li").eq(swiperNewslist.activeIndex).addClass('current');

            /* 2015-07-01 控制行情城市显示 start*/
            if (swiperNewslist.activeIndex == 0) {
                $("#changecity").hide();
            } else {
                $("#changecity").show();
            }
            /*end*/
        }
    });
    $("#newslistTab li").on('touchstart mousedown', function(e) {
        e.preventDefault();
        $("#newslistTab .current").removeClass('current');
        $(this).addClass('current');
        swiperNewslist.slideTo($(this).index());
    });
    $("#newslistTab li").click(function(e) {
        e.preventDefault();
    });
</script>
<script type="text/javascript">
    var tuanFlag = false;
    $(function() {
        //年款切换
        var csCondition = new CsSummaryCondition();
        csCondition.Init(["[data-action=stopyear]", "[data-action=level]", "[data-action=bodyform]"]);

        var tuangouParam = 'mediaid=2&locationid=1&cmdId=' + curSerialId + '&cityId=' + citycode;
        //易团购
        (function getYiTuanGou(params) {
            $.ajax({
                url: "http://api.market.bitauto.com/MessageInterface/YiTuanGou/GetYiTuanGouUrl.ashx?" + params, //?mediaid=2&locationid=1&cmdId=3999&cityId=201
                async: false,
                dataType: "jsonp",
                //jsonpCallback: "successHandler",
                //cache: true,
                success: function(data) {
                    var h = [];
                    if (data && data.result == "yes") {
                        tuanFlag = true;
                        var tuangouUrl = data.url;
                        var slogan = data.slogan;
                        h.push("<span class=\"yh-tap\"></span>");
                        h.push("<div class=\"cont-box\">");
                        h.push("<h4>本地限时特惠</h4>");
                        h.push("<p>" + slogan + "</p>");
                        h.push("</div>");
                        $(".prefer-tim").html(h.join(''));
                        $(".prefer-tim").on("click", function() {
                            window.location.href = tuangouUrl;
                        });
                        csCondition.getTuanGouTag();
                    }
                }
            });
        })(tuangouParam);

        //音频
        var audioCreator;
        $.getJSON("http://api.admin.bitauto.com/audiobase/audio/GetAudioAudioResource?IsCarModel=1&pagesize=5&serialId=" + CarCommonCSID, function(data) { //CarCommonCSID  3701  3777
            if (data.length > 0) {
                $("#audio-title, #audio_list").show();
                var audioTemplate = document.getElementById('template_audios').innerHTML;
                audioCreator = lazyT.tmpl(audioTemplate);
                var audioHtml = audioCreator(data);
                $("#audio_list").html(audioHtml);
                $(".jp-progress").each(function() {
                    var maxWidth = $(document).width() - 110;
                    var duration = $(this).data("duration");
                    var cssWidth = 150;
                    if (duration <= 60)cssWidth = 100;
                    if (duration >= 180)cssWidth = maxWidth;
                    if (duration > 60 && duration < 180) {
                        cssWidth = (duration - 60) / 120 * (maxWidth - 100) + 100;
                    }
                    $(this).css("width", cssWidth + "px");
                });
                jp.init({ swfPath: "/js/audio" });
            }
        });
    });

    // 检查是否为中文
    function isChn(str) {
        var reg = /^[\u4E00-\u9FA5]+$/;
        if (!reg.test(str)) {
            return false;
        }
        return true;
    }
</script>
<!--评论数JS-->
<!--#include file="/include/pd/2014/common/00001/201506_typls_js_Manual.shtml"-->
</body>
</html>