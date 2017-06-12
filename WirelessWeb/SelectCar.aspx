<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCar.aspx.cs" Inherits="WirelessWeb.SelectCar" %>

<%@ Register TagPrefix="car" TagName="SelectCarTool" Src="~/UserControls/SelectCarTool.ascx" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <!--#include file="~/ushtml/0000/myiche2015_cube_xuanche_style-news-1004.shtml"-->
    <%=titleHtml %>
    <script type="text/javascript">
        var _SearchUrl = '<%=this._SearchUrl %>';
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"> </script>
</head>
<body>
    <!-- header start -->
	<div class="op-nav">
		<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
		<div class="tt-name">
			<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>条件选车</h1>
		</div>
		<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
	</div>
	<div class="op-nav-mark" style="display: none;"></div>
	<!-- header end -->
    <%-- <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/iscroll.js?v=2016020210"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=2016020210"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/rightswipe.js?v=2016020210"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js?v=20170105"></script> 
    <!-- 条件筛选 start -->
    <car:SelectCarTool ID="ucSelectTool" runat="server"></car:SelectCarTool>
    <!-- 条件筛选 end -->

    <!-- 车型图片 start -->
    <div class="searchResult"></div>  
 <%--   <%= serialListHtml%>--%>
    <!-- 车型图片 end -->
    <div class="clear"></div>

    <!-- 分页start -->
    <div class="m-pages" style="display: none">
        <a href="javascript:void(0)" class="m-pages-pre m-pages-none">上一页</a>
        <div class="m-pages-num">
            <div class="m-pages-num-con"><span id="curPageIndex">1</span>/<span id="totalPage">1</span></div>
            <div class="m-pages-num-arrow"></div>
        </div>
        <select class="m-pages-select">
        </select>
        <a href="javascript:void(0)" class="m-pages-next">下一页</a>
    </div>
    <a href="http://car.m.yiche.com/yagaoxuanche/" class="yicheren">
        <p>选车没主意? 试试这个工具！</p>
    </a>
    <!-- 分页 end -->
    <!--二手车 列表 start-->
    <div id="taoche_list" style="display: none">
        <div class="tt-first tt-first-no-bd" >
            <h3 id="taoche_city_title">推荐二手车</h3>
            <div class="opt-more opt-more-gray">
                <a id="a_moreUsedcarUrl" href="http://m.taoche.com/">更多</a>
            </div>
        </div>
        <div class="pic-txt pic-txt-car">
            <ul id="taoche_ulList" pageareaid="35">
            </ul>
            <div class="clear">
            </div>
            <div class="jump-pop" style="display: none;" id="taocheJump-pop">
                <span>您正在前往易车二手车<br>
                    跳转中...</span>
            </div>
        </div>
    </div>
    <!--二手车 列表 end-->
    <script type="text/javascript">
        var url = "http://m.yiche.com/";
        var listSerialAD =eval(<%=adCarListData%>);
    	var BitautoCityForAd = bit_locationInfo.cityName;
    </script>
    <div class="footer15">
         <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <div class="breadcrumb">
            <div class="breadcrumb-box">
		    <a href="http://m.yiche.com/">首页</a> &gt; <a href="http://car.m.yiche.com/brandlist/">选车</a> &gt; <span>条件选车</span>
            </div>
	    </div>
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
	    <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
    <div id="p_8to12" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_8to12_Manual.shtml"-->
    </div>
    <div id="p_12to18" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_12to18_Manual.shtml"-->
    </div>
    <div id="p_5to8" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_5to8_Manual.shtml"-->
    </div>
    <div id="l_3" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_jcx_Manual.shtml"-->
    </div>
      <div id="l_8" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_suv_Manual.shtml-->
    </div>
      <div id="l_7" style="display: none;">
       <!--#include file="/include/pd/2016/wap/00001/201703_wap_cxjg_mpv_Manual.shtml"-->
    </div>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=201209"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/ajax.js?v=201209"></script>
    <script type="text/javascript">
        $(function() {
            //监听最下方搜索文本处清空文本事件
            $(".m-btn-close").on('click', function() {
                $("input[name='txtkeyword']").val('');
            });
            getUsedCarData();
            //addCarListManual();
        });

        //获取二手车数据
        function getUsedCarData() {
            var cityId = 201;
            if (typeof (bit_locationInfo) != 'undefined') {
                cityId = bit_locationInfo.cityId;
            }
            var paraStr =  window.location.search;
            paraStr = paraStr.substring(1, paraStr.length);
            var urlStr = "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?cityid="+cityId+"&count=4&"+paraStr;
            $.ajax({
                url: urlStr,
                cache: true,
                dataType: 'jsonp',
                jsonp: 'callback',
                jsonpCallback: "tcCallback",
                success: function (result) {
                    var resultStr = "";
                    if (result!=null) {
                        var carList = result.CarListInfo;
                        if (carList == null||carList.length==0) {
                            carList = result.RecommendCarList;
                        }
                        if (carList!=null&&carList.length>0) {
                            var count = 0;
                            var mMoreTaoCheUrL = result.M_MoreTaoCheUrL;
                            $("#a_moreUsedcarUrl").attr("href", mMoreTaoCheUrL);
                            for (var i = 0; i < carList.length; i++) {
                                if (count>3) {
                                    break;
                                }
                                var carName = carList[i].BrandName;
                                var pictureUrl = carList[i].PictureUrl_240_160;
                                var displayPrice = carList[i].DisplayPrice;
                                var buyCarDate = carList[i].BuyCarDate;
                                var drivingMileage = carList[i].DrivingMileage;
                                var mCarlistUrl = carList[i].M_CarlistUrl;
                                resultStr += "<li>";
                                resultStr += "<a href=\"" + mCarlistUrl + "&leads_source=m008001\">";
                                resultStr += "<span><img src=\""+pictureUrl+"\"></span>";
                                resultStr += "<p>"+carName+"</p>";
                                resultStr += "<b>"+displayPrice+"</b>";
                                resultStr += "<em>"+buyCarDate+"/"+drivingMileage+"公里</em>";
                                resultStr += "</a>";
                                resultStr += "</li>";
                                count++;
                            }
                        }    
                        if (resultStr.length > 0) {
                            $("#taoche_ulList").html(resultStr);
                            $("#taoche_list").show();
                        }
                    }
                }
            });
        }
    </script>
</body>
</html>
