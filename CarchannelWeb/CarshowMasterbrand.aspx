<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarshowMasterbrand.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarshowMasterbrand" %>
<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="300" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=MasterName %>广州车展|广州车展<%=MasterName %>图片_2009广州车展】- 易车网BitAuto.com</title>
    <meta name="keywords" content="<%=MasterName %>车展,<%=MasterName %>广州车展,广州车展<%=MasterName %>图片,2009广州车展<%=MasterName %>美女车模" />
    <meta name="description" content="<%=MasterName %>车展,<%=MasterName %>广州车展,广州车展<%=MasterName %>图片,2009广州车展<%=MasterName %>美女车模" />
    <!--#include file="~/ushtml/autoshow_gz2009/autoshow_2009gz.shtml"-->
</head>
<body>
    <!--#include file="~/html/exhibitionheader.shtml"-->
    <div class="bt_page">
        <div class="col-all">
            <div class="lh_er_pic lh_er_pic_huanan p_mt10">
                <h1>
                    2009广州国际车展</h1>
            </div>
            <div class="lh_position">
                <p>
                    <a href="http://www.bitauto.com" target="_black">易车网</a> <a href="http://car.bitauto.com/"
                        target="_black">车型</a><em>&gt;</em><a href="http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/">2009广州车展</a><em>&gt;</em><span><a
                            href="<%=GetPavUrl%>"><%=modelPavilion.Name%></a></span><em>&gt;</em><span><a><%= MasterName%></a></span></p>
            </div>
        </div>
        
        <!--  -->
        <div class="col-all">
        <div class="publicTab1 pptab">
            <ul class="tab">
                <li class="on">
                    <h1>
                            <a><img alt="<%=MasterName %>" src="http://img1.bitauto.com/bt/car/default/images/carimage/m_<%= masterId %>_a.png"/><%=MasterName %></a></h1>
                </li>
            </ul>
            <div class="morelinks">
                <h4>
                    <a href="http://photo.bitauto.com/master/<%= masterId %>.html" target="_blank">图片</a></h4>
                |<h4>
                    <a href="http://price.bitauto.com/keyword.aspx?keyword=<%= Server.UrlEncode(MasterName) %>&mb_id=<%= masterId %>" target="_blank">
                        报价</a></h4>
                |<h4>
                    <a href="http://www.cheyisou.com/jingxiaoshang/<%= Server.UrlEncode(MasterName) %>/" target="_blank">经销商</a></h4>
                |<h4>
                    <a href="http://v.bitauto.com/car/master/<%= masterId %>" target="_blank">视频</a></h4>
                |<h4>
                    <a href="http://car.bitauto.com/<%= masterSpell %>/xinwen/" target="_blank">文章</a></h4>
                |<h4>
                    <a href="http://ask.bitauto.com/search?keyword=<%= Server.UrlEncode(MasterName) %>" target="_blank">问答</a></h4>
            </div>
        </div>
        </div>
        <!-- -->
        
        <div class="col-sub">
            <!--封面图-->
            <%=CoverFigureString%>
        </div>
        <div class="col-main">
            <!--顶部新闻-->
            <%=NewString %>
        </div>
        <div class="col-side">
            <!--品牌关注排行-->
            <div class="line_box">
                <h3>
                    <span>文章关注排行榜</span></h3>
                <div class="wd_unit">
                    <ol class="wd_hotrank">
                        <!--#include file="~/html/Quene.shtml"-->
                    </ol>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="col-all">
            <!--展出子品牌-->
            <%=IsCarTypeString %>
        </div>
        <div class="clear">
        </div>
        <div class="col-all">
            <!--展出车模-->
            <%=WomanModuleString %>
        </div>
        <div class="clear">
        </div>
        <div class="col-all">
            <!--视频-->
            <%=Video %>
        </div>
        <div class="clear">
        </div>
        <div class="col-all">
            <!--其他厂家-->
            <%=ShowCompany%>
        </div>
        <div class="clear">
        </div>        
    <!--#include file="~/html/chezhanallbrand.shtml"-->
    </div>
    <!--#include file="~/html/exhibitionfooter.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
