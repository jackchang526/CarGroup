<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPageNews.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageBrandV2.CarBrandPageNews" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <title>
        <%=_Title%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="Keywords" content="<%=_BrandName%>,<%=_BrandName%>汽车,<%=_BrandName%>汽车报价,易车网" />
    <meta name="Description" content="<%=_BrandName%>汽车:易车网车型频道为您提供全国348个汽车市场<%=_BrandName%>经销商的真实<%=_BrandName%>汽车报价,<%=_BrandName%>汽车图片,<%=_BrandName%>汽车新闻,<%=_BrandName%>视频,<%=_BrandName%>口碑,<%=_BrandName%>问答,<%=_BrandName%>二手车等,是全国数千万购车意向客户的首选专业汽车导购网站" />
   	<!--#include file="~/ushtml/0000/yiche_2016_cube_pinpai_style-1248.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <header class="header-main header-type1">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <h1 class="brand-logo">
                    <a href="/<%=_BrandSpell %>/">
                        <img class="logo" src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=_MasterBrandId %>_55.png" alt="<%=_BrandName%>汽车"></a>
                    <a href="/<%=_BrandSpell %>/"><%=_BrandName%></a>
                </h1>
            </div>
            <div class="col-xs-8 right-box">
                <ul class="list keyword-list">
                    <li class="dt">热门搜索：</li>
                    <li><a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a></li>
                    <li><a href="http://v.bitauto.com/ycjm.shtml" target="_blank">原创节目</a></li>
                    <li><a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></li>
                </ul>
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div> 
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul class="list" id="ulForTempClickStat">
                            <li id="treeNav_chexing"><a target="_self" href="http://car.bitauto.com/<%=_BrandSpell%>/">首页</a></li>
                            <li id="treeNav_wenzhang" class="active"><a target="_self" href="http://car.bitauto.com/<%=_BrandSpell%>/wenzhang/">文章</a></li>
                            <li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/brand/<%=_BrandId%>/">图片</a></li>
                            <li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/brand/<%=_BrandId%>_0_0.html">视频</a></li>
                            <li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/b<%=_BrandId%>/">报价</a></li>
                            <li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/b<%=_BrandId%>/">降价</a></li>
                            <li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=_BrandSpell%>/">经销商</a></li>
                            <li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?brand=<%=_BrandId%>/">置换</a></li>
                            <li id="treeNav_yanghu"><a target="_blank" href="http://yanghu.bitauto.com/?source=1">养护</a></li>
                            <li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=_BrandSpell %>/">二手车</a></li>
                            <li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/xuanche/?bid=<%=_BrandId%>">口碑</a></li>
                            <li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=_MasterBrandId%>/">问答</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    </header>
    <div class="container brand-article-page">
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="row article-section">
                        <%=_WenZhangHeader%>
                        <%if (string.IsNullOrEmpty(this._NewsList))
                          { %>
                        <div class="empty-box" style="margin-bottom: 50px;">
                            <div class="note-box note-empty type-2">
                                <div class="ico"></div>
                                <div class="info">
                                    <h3>抱歉，还没有该品牌相关的文章！</h3>
                                </div>
                            </div>
                        </div>
                        <%}
                          else
                          { %>
                        <%=this._NewsList %>
                        <BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false" NoLinkClassName="preview_off" />
                        <%} %>
                    </div>
                </div>
            </div>
            <div class="col-xs-3 section-right">
                <div class="row hotarticle-sidebar margin-bottom-sm">
                    <div class="section-header header3">
                        <div class="box">
                            <h2>热门文章排行</h2>
                        </div>
                    </div>
                    <div class="list-txt list-txt-m list-txt-style-num">
                        <ul>
                            <!--#include file="~/include/debris/ranks/Top10HotNewsPage_utf8.shtml"-->
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <input type="hidden" id="cheyisou" value="http://www.cheyisou.com/qiche/" />
    <script type="text/javascript" language="javascript">
        var CarCommonBSID = "<%= brandEntity.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = '<%= _BrandId.ToString() %>';
    </script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newsViewCount.min.js"></script>
    <!--footer-->
    <!--#include file="~/htmlV2/footer2016.shtml"-->
    <!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
</body>
</html>

