<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialYearNews.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageYear.SerialYearNews" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSee.ascx" %>
<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=pageTitle%>
    </title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_car_wenzhang-746.shtml"-->
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <%= SerialNewHead %>
    <script type="text/javascript">
        <%= JsForCommonHead %>
    </script>
    <!--page start-->
    <div class="bt_page">
        <div class="col-all">
            <div class="col-con">
                <div class="line-box line-box_t0" id="dglist">
                    <%=newsNavHtml %>
                    <%=newsHtml %>
                    <BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false"
                        NoLinkClassName="preview_off" />
                </div>
                <!-- SEO底部热门 -->
                <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
                <!-- SEO底部热门 -->
            </div>
            <div class="col-side">
                <!--子品牌相关活动-->
                <!--<script type="text/javascript" src="http://life.bitauto.com/huodong/recommend/forum_recommend.aspx?serial=<%= serialId %>&count=2&source=youce"></script>-->

                <!-- 看过某车的还看过 -->
                <%--<div class="line-box">
				<div class="side_title">
					<h4>
						看了<%=serialShowName.Replace("(进口)", "").Replace("（进口）", "") %>的还看</h4>
				</div>
				<ul class="pic_list">
					<%=serialToSeeHtml%>
				</ul>
			</div>--%>
                <Car:SerialToSee ID="ucSerialToSee" runat="server"></Car:SerialToSee>
                <%=carCompareHtml%>
                <!--浏览过的品牌-->
                <!--<script charset="gb2312" type="text/javascript" src="http://go.bitauto.com/handlers/ModelsBrowsedRecently.ashx"></script>-->
                <!--此品牌下其别子品牌-->
                <div class="line-box">
                    <%=GetBrandOtherSerial() %>
                    <div class="clear">
                    </div>
                </div>
                <!--二手车-->
                <%--<%=UCarHtml %>--%>
                <div class="line-box display_n" id="line_boxforucar_box" style="display: none;">
                    <div class="side_title">
                        <h4><a target="_blank" href=" http://www.taoche.com/<%=serialBase.AllSpell %>/"><%=serialBase.ShowName %>二手车</a></h4>
                    </div>
                    <div class="theme_list play_ol">
                        <ul class="secondary_list" id="ucar_serialcity">
                        </ul>
                    </div>
                </div>
                <!--百度推广-->
                <div class="line_box baidupush line_box_top_b">
                    <script type="text/javascript">
                        /*bitauto 200*200，导入创建于2011-10-17*/
                        var cpro_id = 'u646188';
                    </script>
                    <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
                </div>
                <!--百度推广-End -->
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115" type="text/javascript"></script>
    <script type="text/javascript">
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
            $.each(data, function (i, n) {
                if (i > 6) return;
                strHtml.push("<li><a class=\"img\" title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">");
                strHtml.push("<img src=\"" + n.PictureUrl + "\"></a><p class=\"tit\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.BrandName + "</a></p>");
                strHtml.push("<p class=\"hui\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</p>");
                strHtml.push("<p class=\"red\">" + n.DisplayPrice + "</p>");
                strHtml.push("</li>");
            });
            $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
        } catch (e) { }
    }
    </script>
    <!-- 调用尾 -->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--提意见浮层-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
