<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPageNews.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageBrand.CarBrandPageNews" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <title>
        <%=_Title%></title>
    <meta name="Keywords" content="<%=_BrandName%>,<%=_BrandName%>汽车,<%=_BrandName%>汽车报价,易车网" />
    <meta name="Description" content="<%=_BrandName%>汽车:易车网车型频道为您提供全国348个汽车市场<%=_BrandName%>经销商的真实<%=_BrandName%>汽车报价,<%=_BrandName%>汽车图片,<%=_BrandName%>汽车新闻,<%=_BrandName%>视频,<%=_BrandName%>口碑,<%=_BrandName%>问答,<%=_BrandName%>二手车等,是全国数千万购车意向客户的首选专业汽车导购网站" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingzhupinpaipinpai-771.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>

    <!--#include file="~/html/header2014.shtml"-->
    <!-- -->
    <div class="header_style">
        <div class="bitauto_logo">
        </div>
        <div class="pp-logo-img">
            <a href="/<%=_BrandSpell %>/">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=_MasterBrandId %>_55.png"
                    alt=""></a>
        </div>
        <div class="pp-logo-tit">
            <h2>
                <a href="/<%=_BrandSpell %>/">
                    <%=_BrandName%></a>
            </h2>
        </div>
        <div class="bt_searchNew">
            <!--#include file="~/html/bt_searchV3.shtml"-->
        </div>
        <div class="bt_hot">
            热门搜索： <a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a>
            <a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/"
                target="_blank">降价</a>
        </div>
    </div>
    <div class="bt_pageBox">
        <div class="publicTabNew">
            <ul class="tab" id="ulForTempClickStat">
                <li id="treeNav_chexing"><a target="_self" href="http://car.bitauto.com/<%=_BrandSpell%>/">
                    首页</a></li>
                <li id="treeNav_wenzhang" class=" current"><a target="_self" href="http://car.bitauto.com/<%=_BrandSpell%>/wenzhang/">
                    文章</a></li>
                <li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/brand/<%=_BrandId%>/">
                    图片</a></li>
                <li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/brand/<%=_BrandId%>_0_0.html">
                    视频</a></li>
                <li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/keyword.aspx?b_id=<%=_BrandId%>">
                    报价</a></li>
                <li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/b<%=_BrandId%>/">
                    降价</a></li>
                <li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=_BrandSpell%>/">
                    经销商</a></li>
                <li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?brand=<%=_BrandId%>/">
                    置换</a></li>
                <li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=_BrandSpell %>/">
                    二手车</a></li>
                <li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/b_<%=_BrandId%>/">
                    口碑</a></li>
                <li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=_MasterBrandId%>/">
                    问答</a></li>
            </ul>
        </div>
    </div>
    <div class="bt_page">
        <div class="col-all clearfix">
            <!--左侧内容 start-->
            <div class="col-con">
                <!--文章列表 start-->
                <div class="line-box line-box_t0">
                    <!--标题 start-->
                    <div class="title-con">
                        <div class="title-box">
                            <ul class="title-tab title-tab-h3">
                                <%=_WenZhangHeader%>
                                <%--<li class="current"><a href="/<%=_BrandSpell %>/">全部</a><em>|</em></li>
                                <li class=""><a href="/<%=_BrandSpell %>/xinwen/">新闻</a><em>|</em></li>
                                <li class=""><a href="/<%=_BrandSpell %>/daogou/">导购</a><em>|</em></li>
                                <li class=""><a href="/<%=_BrandSpell %>/yongche/">用车</a></li>--%>
                            </ul>
                        </div>
                        <!--标题 end-->
                    </div>
                    <div class="tuwenlistbox">
                        <%=this._NewsList %>
                    </div>
                    <BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false"
					NoLinkClassName="preview_off" />
                </div>
                <!--文章列表 end-->
            </div>
            <!--左侧内容 end-->
            <%--<div class="col-all">
                <div class="line_box mainlist_box all_newslist all_carnewslist">
                    <h3>
                        <span>
                            <%=_BrandName%>汽车<%=_TagContent%></span></h3>
                    <%=this._NewsList %>
                </div>
            </div>--%>
            <div class="col-side">
                <div class="line-box line-box_t0">
                    <div class="side_title">
                        <h4>热门文章排行</h4>
                    </div>
                    <div class="rank-list-5 rank-list-10">
                        <ol class="rank-list">
                            <!--#include file="~/include/debris/ranks/Top10HotNewsPage.shtml"-->
                        </ol>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        var CarCommonCBID = '<%= _BrandId.ToString() %>';
    </script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newsViewCount.min.js"></script>
    <!--footer-->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
