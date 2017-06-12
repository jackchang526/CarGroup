<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterBrandPageNews.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageMaster.CarMasterBrandPageNews" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <title>
        <%=_Title%></title>
    <meta name="Keywords" content="<%=_MasterBrandName%>,<%=_MasterBrandName%>汽车,<%=_MasterBrandName%>汽车报价,易车网" />
    <meta name="Description" content="<%=_MasterBrandName%>汽车:易车网车型频道为您提供全国348个汽车市场<%=_MasterBrandName%>经销商的真实<%=_MasterBrandName%>汽车报价,<%=_MasterBrandName%>汽车图片,<%=_MasterBrandName%>汽车新闻,<%=_MasterBrandName%>视频,<%=_MasterBrandName%>口碑,<%=_MasterBrandName%>问答,<%=_MasterBrandName%>二手车等,是全国数千万购车意向客户的首选专业汽车导购网站" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingzhupinpaipinpai-771.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <!-- -->
    <div class="header_style">
        <div class="bitauto_logo">
        </div>
        <div class="pp-logo-img">
            <a href="/<%=_MasterBrandSpell %>/">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=_MasterBrandId %>_55.png"
                    alt=""></a>
        </div>
        <div class="pp-logo-tit">
            <h2>
                <a href="/<%=_MasterBrandSpell %>/">
                    <%=_MasterBrandName%></a>
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
                <li id="treeNav_chexing"><a target="_self" href="http://car.bitauto.com/<%=_MasterBrandSpell%>/">
                    首页</a></li>
                <li id="treeNav_wenzhang" class=" current"><a target="_self" href="http://car.bitauto.com/<%=_MasterBrandSpell%>/wenzhang/">
                    文章</a></li>
                <li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/master/<%=_MasterBrandId%>/">
                    图片</a></li>
                <li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/master/<%=_MasterBrandId%>_0_0.html">
                    视频</a></li>
                <li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/keyword.aspx?mb_id=<%=_MasterBrandId%>">
                    报价</a></li>
                <li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/mb<%=_MasterBrandId%>/">
                    降价</a></li>
                <li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=_MasterBrandSpell%>/">
                    经销商</a></li>
                <li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?master=<%=_MasterBrandId%>/">
                    置换</a></li>
                <li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=_MasterBrandSpell %>/">
                    二手车</a></li>
                <li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/mb_<%=_MasterBrandId%>/">
                    口碑</a></li>
                <li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=_MasterBrandId%>/solved/">
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
            <!--右侧内容 start-->
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
            <!--右侧内容 end-->
        </div>
        <%--<div class="col-all">
            <div class="line_box mainlist_box all_newslist all_carnewslist">
                <h3>
                    <span>
                        <%=_MasterBrandName%>汽车<%=_TagContent%></span></h3>
                <%=this._NewsList %>
            </div>
        </div>--%>
    </div>
    <script type="text/javascript" language="javascript">
        var CarCommonBSID = '<%= _MasterBrandId.ToString() %>';
    </script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newsViewCount.min.js"></script>
    <!--footer-->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
