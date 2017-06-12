<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="H5Web.V2.CsSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【<%= BaseSerialEntity.SeoName %>】报价及图片_评测_口碑_配置-易车网</title>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/style.css"/>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
<header style="z-index: 3" id="tagHeader">
    <div class="head_share" data-action='popup-share'></div>
</header>
<% if (BrokerId > 0)
   { %>
    <ul class="menu fouth base-col-3" style="z-index: 2">
        <li class="current menu1" data-index="1" data-op="look">
            <em>看车</em>
            <ul style="display: none;" data-type="child">
                <li>
                    <a href="#index">封面</a>
                </li>
                <li>
                    <a href="#photo">车型图片</a>
                </li>
                <li>
                    <a href="#cscompare">车型配置</a>
                </li>
                <li>
                    <a href="#article">相关文章</a>
                </li>
                <li>
                    <a href="#commentary">热门点评</a>
                </li>
                <li>
                    <a href="#carlist">车款价格</a>
                </li>
            </ul>
        </li>
        <li class="menu2" data-index="2" data-op="store">
            <em>
                <a href="#agent">购车</a>
            </em>
        </li>
        <li class="menu4" data-index="3" data-op="tool">
            <em>工具</em>
        </li>
    </ul>
<% }
   else if (DealerId > 0)
   { %>
    <ul class="menu fouth base-col-3" style="z-index: 2">
        <li class="current menu1" data-index="1" data-op="look">
            <em>看车</em>
            <ul style="display: none;" data-type="child">
                <li>
                    <a href="#index">封面</a>
                </li>
                <li>
                    <a href="#photo">车型图片</a>
                </li>
                <li>
                    <a href="#cscompare">车型配置</a>
                </li>
                <li>
                    <a href="#article">相关文章</a>
                </li>
                <li>
                    <a href="#commentary">热门点评</a>
                </li>
                <li>
                    <a href="#carlist">车款价格</a>
                </li>
            </ul>
        </li>
        <li class="menu2" data-index="2" data-op="store">
            <em>实体店</em>
            <ul style="display: none" data-type="child">
                <li>
                    <a href="#store">店内还有</a>
                </li>
                <li>
                    <a href="#sales">本店促销</a>
                </li>
                <li>
                    <a href="#map">经销商地图</a>
                </li>
                <li>
                    <a href="#serialmap">车款地图</a>
                </li>
            </ul>
        </li>
        <li class="menu4" data-index="3" data-op="tool">
            <em>工具</em>
        </li>
    </ul>
<% }
   else
   { %>
    <ul class="menu fouth" style="z-index: 2">
        <li class="current menu1" data-index="1" data-op="look">
            <em>看车</em>
            <ul style="display: none;" data-type="child">
                <li>
                    <a href="#index">封面</a>
                </li>
                <li>
                    <a href="#photo">车型图片</a>
                </li>
                <li>
                    <a href="#cscompare">车型配置</a>
                </li>
                <li>
                    <a href="#article">相关文章</a>
                </li>
                <li>
                    <a href="#commentary">热门点评</a>
                </li>
                <li>
                    <a href="#carlist">车款价格</a>
                </li>
                <li>
                    <a href="#forum">车型社区</a>
                </li>
                <li>
                    <a href="#seeagain">看了还看</a>
                </li>
            </ul>
        </li>
        <li class="menu2" data-index="2" data-op="buy">
            <em>购车</em>
            <ul style="display: none" data-type="child">
                <li>
                    <a href="#dealers">经销商铺</a>
                </li>
                <li>
                    <a href="#selloff">优惠购车</a>
                </li>

            </ul>
        </li>
        <li class="menu3" data-index="3" data-op="use">
            <em>用车</em>
            <ul style="display: none" data-type="child">
                <li>
                    <a href="#carinsurance">汽车保险</a>
                </li>
            </ul>
        </li>
        <li class="menu4" data-index="4" data-op="tool">
            <em>工具</em>
            <ul style="display: none;" data-type="child">
                <li>
                    <a href="#">二手车估价</a>
                </li>
                <li>
                    <a href="#">对比工具</a>
                </li>
                <li>
                    <a href="#">购车计算</a>
                </li>
            </ul>
        </li>
    </ul>
<% } %>
<% if (BrokerId > 0)
   { %>
    <div id="fullpage">
        <!--第一屏首页-->
        <div class="section" data-section="index" data-name="index" data-src="V2/module/index.aspx?serialbrandid=<%= SerialBrandId %>&dealerid=<%= DealerId %>&brokerid=<%= BrokerId %>">
        </div>
        <!--第二屏图片-->
        <div class="section" data-section="photo" data-name="photo" data-src="V2/photo.aspx?serialbrandid=<%= SerialBrandId %>">
        </div>
        <!--第三屏配置-->
        <div class="section" data-section="cscompare" data-name="cscompare" data-src="V2/CsCompare.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--第四屏文章-->
        <div class="section" data-section="article" data-name="article" data-src="V2/Article.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--第五屏评价-->
        <div class="section" data-section="commentary" data-name="commentary" data-src="V2/Commentary.aspx?csid=<%= SerialBrandId %>"></div>
        <!--车款列表-->
        <div class="section" data-section="carlist" data-name="carlist" data-src="V2/CarList.aspx?csid=<%= SerialBrandId %>&brokerid=<%= BrokerId %>"></div>
        <div class="section" data-section="agent" data-name="agent" data-src="V2/module/broker/agent.aspx?csid=<%= SerialBrandId %>&brokerid=<%= BrokerId %>&cityid=201">
        </div>

    </div>
<% }
   else if (DealerId > 0)
   { %>
    <div id="fullpage">
        <!--第一屏首页-->
        <div class="section" data-section="index" data-name="index" data-src="V2/module/index.aspx?serialbrandid=<%= SerialBrandId %>&dealerid=<%= DealerId %>">
        </div>
        <!--第二屏图片-->
        <div class="section" data-section="photo" data-name="photo" data-src="V2/photo.aspx?serialbrandid=<%= SerialBrandId %>">
        </div>
        <!--第三屏配置-->
        <div class="section" data-section="cscompare" data-name="cscompare" data-src="V2/CsCompare.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--第四屏文章-->
        <div class="section" data-section="article" data-name="article" data-src="V2/Article.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--第五屏评价-->
        <div class="section" data-section="commentary" data-name="commentary" data-src="V2/Commentary.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--车款列表-->
        <div class="section" data-section="carlist" data-name="carlist" data-src="V2/CarList.aspx?csid=<%= SerialBrandId %>">
        </div>
        <!--店内还有-->
        <div class="section" data-section="store" data-name="store" data-src="V2/module/dealer/DealerCarReference.aspx?csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>">
        </div>
        <!--店内促销-->
        <div class="section" data-section="sales" data-name="sales" data-src="V2/module/dealer/DealerNews.aspx?csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>">
        </div>
        <!--经销商地图-->
        <div class="section" data-section="map" data-name="map" data-src="V2/module/dealer/map.aspx?csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>">
        </div>
        <!--车款地图-->
        <div class="section" data-section="serialmap" data-name="serialmap" data-src="V2/module/dealer/serialmap.aspx?csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>">
        </div>
    </div>
<% }
   else
   { %>
    <div id="fullpage">
        <!--首页-->
        <div class="section" data-section="index" data-name="index" data-src="V2/index.aspx?serialbrandid=<%= SerialBrandId %>">
        </div>
        <!--图片-->
        <div class="section" data-section="photo" data-name="photo" data-src="V2/photo.aspx?serialbrandId=<%= SerialBrandId %>">
        </div>
        <!--配置-->
        <div class="section" data-section="cscompare" data-name="cscompare" data-src="V2/CsCompare.aspx?csid=<%= SerialBrandId %>">
        </div>
        <div class="section" data-section="article" data-name="article" data-src="V2/Article.aspx?csid=<%= SerialBrandId %>"></div>
        <!--评价-->
        <div class="section" data-section="commentary" data-name="commentary" data-src="V2/Commentary.aspx?csid=<%= SerialBrandId %>"></div>
        <!--车款列表-->
        <div class="section" data-section="carlist" data-name="carlist" data-src="V2/CarList.aspx?csid=<%= SerialBrandId %>"></div>
        <!--论坛-->
        <div class="section" data-section="forum" data-name="forum" data-src="V2/Forum.aspx?csid=<%= SerialBrandId %>"></div>
        <!--看了还看-->
        <div class="section" data-section="seeagain" data-name="seeagain" data-src="V2/SeeAgain.aspx?csid=<%= SerialBrandId %>"></div>
        <!--经销商-->
        <div class="section" data-section="dealers" data-name="dealers" data-src="V2/Dealers.aspx?csid=<%= SerialBrandId %>"></div>
        <!--优惠购车-->
        <div class="section" data-section="selloff" data-name="selloff" data-src="V2/SellOff.aspx?csid=<%= SerialBrandId %>"></div>
        <!--保险-->
        <div class="section" data-section="carinsurance" data-name="carinsurance" data-src="V2/CarInsurance.aspx?csid=<%= SerialBrandId %>"></div>
        <!--第三屏end-->
    </div>
<% } %>
<div class="loading">
    <img src="http://192.168.0.10:8888/m/2015插件/自由滚动/img/loading.gif"/>
</div>

<!--内容结束-->
<div class="leftmask mark mark1" style="display: none;"></div>
<div class="leftPopup share-bottom popup-share" data-back="mark1" style="display: none;">
    <div class="swipeLeft">
        <!--去掉swipeLeft-block 样式就会隐藏-->
        <ul class="bdsharebuttonbox base-col-3" data-tag="share_1">
            <li>
                <a href="javascript:;" class="icon-sina" data-cmd="tsina">
                    新浪微博
                </a>
            </li>
            <li>
                <a href="javascript:;" class="icon-qq" data-cmd="qzone">
                    QQ空间
                </a>
            </li>
            <li>
                <a href="javascript:;" data-cmd="tqq" class="icon-wb">
                    腾讯微博
                </a>
            </li>
        </ul>
        <a href="###" class="close">取消</a>
        <script>
            window._bd_share_config = {
                share: [
                    {
                        "tag": "share_1",
                        "bdSize": 32,
                        "bdCustomStyle": "http://image.bitautoimg.com/uimg/css/0016/myiche2014_cube_newsshare_style-20140808140022-802.css"
                    }
                ]
            };
            with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'http://bdimg.share.baidu.com/static/api/js/share.js?cdnversion=' + ~(-new Date() / 36e5)];
        </script>
    </div>
</div>
<!--菜单结束-->
<script type="text/javascript" src="http://192.168.0.10:8888/m/2015插件/自由滚动/js/public/jquery-2.1.4.min.js"></script>
<script type="text/javascript" src="http://192.168.0.10:8888/m/2015插件/自由滚动/js/public/jquery.fullPage.min.js"></script>
<script type="text/javascript" src="/scripts/base.js"></script>
<script type="text/javascript" src="/Scripts/V2/fullPage.extend.js"></script>
<% if (DealerId > 0)
   { %>
    <script type="text/javascript" src="/handlers/GetDateAsync.ashx?service=dealer&method=share&csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>&"></script>
<% }
   else if (BrokerId > 0)
   { %>
    <script type="text/javascript" src="/handlers/GetDateAsync.ashx?service=agent&method=share&brokerid=<%= SerialBrandId %>&csid=<%= BrokerId %>&type=4"></script>
<% } %>
<script type="text/javascript" src="/Scripts/V2/model.js"></script>
<script type="text/javascript">
    /*
		显示隐藏分享按钮
		*/
    var shareFunc = function() {
        $('[data-action=popup-share]').rightSwipe({
            clickEnd: function(b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function(ev) { ev.preventDefault(); }, touchmove: function(ev) { ev.preventDefault(); } });
                    $leftPopup.find('.close').on('click', function(ev) {
                        ev.preventDefault();
                        $back.trigger('close');
                    });
                }
            }
        });
    };
    /*
		全局配置信息
		*/
    var Config = (function() {
        var menu1, menu2, menu3, anchors;
        <% if (BrokerId > 0)
           { %>
        menu1 = ['index', 'photo', 'cscompare', 'article', 'commentary', 'carlist'];
        menu2 = ['agent'];
        anchors = menu1.concat(menu2);
        <% }
           else if (DealerId > 0)
           { %>
        menu1 = ['index', 'photo', 'cscompare', 'article', 'commentary', 'carlist'];
        menu2 = ['store', 'sales', 'map', 'serialmap'];
        anchors = menu1.concat(menu2);
        <% }
           else
           { %>
        menu1 = ['index', 'photo', 'cscompare', 'article', 'commentary', 'carlist', 'forum', 'seeagain'];
        menu2 = ['dealers', 'selloff'];
        menu3 = ['carinsurance'];
        anchors = menu1.concat(menu2, menu3);
        <% } %>
        return{
            menu1: menu1,
            menu2: menu2,
            menu3: menu3,
            anchors: anchors,
            loadeds: [],
            <% if (BrokerId > 0)
               { %>
            pages: {
                "index": { "title": "<%= BaseSerialEntity.ShowName %>", "share": "" },
                "photo": { "title": "车型图片", "share": "" },
                "cscompare": { "title": "车型配置", "share": "" },
                "article": { "title": "相关文章", "share": "" },
                "commentary": { "title": "热门点评", "share": "" },
                "carlist": { "title": "车款价格", "share": "" },
                "agent": { "title": "约我买车", "share": "" }
            }
            <% }
               else if (DealerId > 0)
               { %>
            pages: {
                "index": { "title": "<%= BaseSerialEntity.ShowName %>", "share": "" },
                "photo": { "title": "车型图片", "share": "" },
                "cscompare": { "title": "车型配置", "share": "" },
                "article": { "title": "相关文章", "share": "" },
                "commentary": { "title": "热门点评", "share": "" },
                "carlist": { "title": "车款价格", "share": "" },
                "store": { "title": "店内还有", "share": "" },
                "sales": { "title": typeof share_dealerInfo != "undefined" ? share_dealerInfo.name : "易车", "share": "" },
                "map": { "title": typeof share_dealerInfo != "undefined" ? share_dealerInfo.name : "易车", "share": "" },
                "serialmap": { "title": typeof share_dealerInfo != "undefined" ? share_dealerInfo.name : "易车", "share": "" }
            }
            <% }
               else
               { %>
            pages: {
                "index": { "title": "<%= BaseSerialEntity.ShowName %>", "share": "" },
                "photo": { "title": "车型图片", "share": "" },
                "cscompare": { "title": "车型配置", "share": "" },
                "article": { "title": "相关文章", "share": "" },
                "commentary": { "title": "热门点评", "share": "" },
                "carlist": { "title": "车款价格", "share": "" },
                "forum": { "title": "车型社区", "share": "" },
                "seeagain": { "title": "看了还看", "share": "" },
                "dealers": { "title": "经销商铺", "share": "" },
                "selloff": { "title": "优惠购车", "share": "" },
                "carinsurance": { "title": "汽车保险", "share": "" }
            }
            <% } %>
        };
    })();

    function menuHide(ev) {
        $(".menu.fouth ul").stop().slideUp();
    }

    function prev() {
        $.fn.fullpage.moveSectionUp();
    }

    function next() {
        $.fn.fullpage.moveSectionDown();
    }

    var cssummary = function() {
        var share = $("#tagHeader").html();
        return {
            setHeader: function(title, shareUrl) {
                $("#tagHeader").html(title + share);
                shareFunc();
            }
        };
    }();
    <%
               if (DealerId > 0)
               { %>
    $("div[data-section='serialmap']").attr("data-src", "V2/module/dealer/serialmap.aspx?csid=<%= SerialBrandId %>&dealerid=<%= DealerId %>" + "&cityid=" + bit_locationInfo.cityId);
    <% }
               else
               { %>
    $("div[data-section='dealers']").attr("data-src", "V2/Dealers.aspx?csid=<%= SerialBrandId %>" + "&cityid=" + bit_locationInfo.cityId);
    $("div[data-section='selloff']").attr("data-src", "V2/SellOff.aspx?csid=<%= SerialBrandId %>" + "&cityid=" + bit_locationInfo.cityId);
    <% } %>
</script>
<script type="text/javascript">
    var forweixinObj = {
        debug: false,
        appId: 'wx0c56521d4263f190',
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
    };
    var pageShareContent = {
        title: '【<%= BaseSerialEntity.SeoName %>】车型手册-易车网',
        keywords: '<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑',
        desc: '易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。',
        link: 'http://car.h5.yiche.com/<%= BaseSerialEntity.AllSpell %>/?',
        imgUrl: 'http://image.bitautoimg.com/newsimg-100-w0-1-q80/carchannel/pic/cspic/<%= SerialBrandId %>.jpg'
    };
    var dealerid = <%= DealerId %>;
    var brokerid = <%= BrokerId %>;
    if (typeof (share_dealerInfo) != "undefined") {
        if (typeof (share_dealerInfo.shareTitle) != "undefined" && share_dealerInfo.shareTitle && share_dealerInfo.shareTitle != "") {
            pageShareContent.title = share_dealerInfo.shareTitle;
        }
        if (typeof (share_dealerInfo.shareDesc) != "undefined" && share_dealerInfo.shareDesc && share_dealerInfo.shareDesc != "") {
            pageShareContent.desc = share_dealerInfo.shareDesc;
        }
        pageShareContent.link = pageShareContent.link + "&dealerid=" + dealerid;
    }
    if (typeof (share_brokerInfo) != "undefined") {
        if (typeof (share_brokerInfo.shareTitle) != "undefined" && share_brokerInfo.shareTitle && share_brokerInfo.shareTitle != "") {
            pageShareContent.title = share_brokerInfo.shareTitle;
        }
        if (typeof (share_brokerInfo.shareDesc) != "undefined" && share_brokerInfo.shareDesc && share_brokerInfo.shareDesc != "") {
            pageShareContent.desc = share_brokerInfo.shareDesc;
        }
        pageShareContent.link = pageShareContent.link + "&brokerid=" + brokerid;
    }
</script>
<script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<script type="text/javascript" src="/Scripts/V2/csSummary.js"></script>
</body>
</html>