<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompare.aspx.cs" Inherits="WirelessWeb.CsCompare" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【<%=se.SeoName%>配置】<%=se.Brand.SeoName%><%=se.Name%>详细参数介绍-手机易车网</title>
	<meta name="keywords" content="<%=se.SeoName%>参数,<%=se.SeoName%>配置,<%=se.Brand.SeoName%><%=se.Name%>" />
	<meta name="description" content="<%=se.SeoName%>配置:<%=se.Brand.SeoName%><%=se.Name%>频道为您提供<%=se.SeoName%>综合配置信息,包括<%=se.SeoName%>安全装备,<%=se.SeoName%>操控配置,<%=se.SeoName%>内饰配置,<%=se.SeoName%>参数性能,<%=se.SeoName%>车型资料大全等,查<%=se.SeoName%>参数配置,就上易车网" />
	<!--#include file="~/ushtml/0000/myiche2015_cube_duibi_style-news-1003.shtml"-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js"></script>
</head>
<body>
 	<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
 			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%= se.ShowName %></h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
	<!-- 互联互通 start -->
	<%=CsHeadHTML %>
	<!-- 互联互通 end -->
	<div class="m-tool-compare y2015">
		<div id="CarCompareContent">
		</div>
		<div class="m-statement">以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a href="http://m.yiche.com/wap2.0/feedback/">“欢迎您及时指正！”</a></div>
	</div>
	<!-- black popup start -->
	<%--<div class="leftmask" style="display: none;">
	</div>
	<div class="leftPopup" id="carinfo_container" style="display: none;">
		<div class="swipeLeft">
		</div>
	</div>--%>
	<%--<div class="btn-more-peizhi" id="popMenu" style="display: none;">
	</div>
	<div class="more-peizhi-list" id="menu" style="display: none;">
		<ul>
		</ul>
	</div>--%>
    <!--车款层 start-->
    <div class="leftmask mark leftmask3" style="display: none;"></div>
    <div id="popupCar" class="leftPopup car-model models original" data-back="leftmask3" style="display: none" data-key="model">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" />
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--车款层 end-->
    <!--loading模板 start -->
    <div class="template-loading" style="display: none;">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" />
            <p>正在加载...</p>
        </div>
    </div>
    <!--loading模板 end -->
    <!--车款模板 start-->
    <script type="text/template" id="modelTemplate">
        { for(var n in CarList){ }
            {var iszx = arrMark[n.replace('s', '')]}
            <div class="tt-small" {= !iszx ? 'data-slider="pic-txt-h"':''} >
                <span>{= n.replace('s','')+'款' }
                    {if(!iszx){}
                    <em>[停售]</em>
                    {}}
                </span>
                {if(!iszx){}
                <i></i>
                {}}
            </div>

        <!-- 图文混排横向 start -->
        <div class="pic-txt-h pic-txt-9060 tone {=iszx||salesYearCount ==0?'open':''}">
        {salesYearCount++;}
            <ul>
                {for(var i=0;i < CarList[n].length;i++){}
                <li {= CarList[n][i].CarId.toString() == (api.model.currentid.toString()) ? 'class="current"':''}">
                    <a href="#" data-id="{= CarList[n][i].CarId}" >
                        <h4>{= CarList[n][i].CarName} {=CarList[n][i].SaleState =="在销"?"":(CarList[n][i].SaleState=="待销"?"[未上市]":"[停售]")}</h4>
                        <p><strong>{= CarList[n][i].ReferPrice.toString() == "" ? "暂无":CarList[n][i].ReferPrice+'万'}</strong></p>
                    </a>
                </li>
                {}}
            </ul>
        </div>
        {}}
       {salesYearCount=0;}
    </script>
	<!--分类 start-->
    <div class="float-catalog-mask" id="popup-menumask" style="display:none"></div>
    <div class="float-catalog" id="popup-menu" data-action="cact"> 
	    目录
	    <div class="catalog-list" id="popup-menulist" style="display:none">
		    <ul>
		    </ul>
		    <div class="ico-catalog-arrow"></div>
	    </div>
    </div>
    <%if (se.Id==3814)
      { %>
     <a href="<%=returnPeizhiUrl %>" class="return-peizhi" data-channelid="27.165.1755">查看全系标配</a>
    <%}else if(se.Id==3524)
    {%>
     <a href="<%=returnPeizhiUrl %>" class="return-peizhi" data-channelid="27.165.1758">查看全系标配</a>
    <%} %>
   

	<!--分类 end-->
	<!-- black popup end -->
	<div class="footer15">
         <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <div class="breadcrumb">
            <div class="breadcrumb-box">
		    <a href="http://m.yiche.com/">首页</a> &gt; <a href="http://car.m.yiche.com/brandlist/">选车</a> &gt; <a href="http://car.m.yiche.com/brandlist/<%= se.Brand.MasterBrand.AllSpell %>/"><%= se.Brand.MasterBrand.Name %></a> &gt; <a href="http://car.m.yiche.com/<%= se.AllSpell %>/"><%= se.Name %></a> &gt; <span>参数</span>
                </div>
	    </div>
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
	    <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
	<!-- 导航开始 -->
	<%--<!--#include file="~/include/pd/2012/wap/00001/201503_wap_zsy_cd_js_Manual.shtml" -->
	<script type="text/javascript">
		// 默认元素ID "m-car-nav"
		CarNavForWireless.DivID = "m-car-nav";
		// 导航当前标签索引(0:综述,1:配置,2:图片,3:油耗,4:详解,5:口碑,6:视频,7:论坛)
		CarNavForWireless.CurrentTagIndex = 1;
	</script>
	<script type="text/javascript" src="http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csid=<%= se.Id %>&op=GetCsForWireless&callback=CarNavForWireless.GenerateNav"
		charset="utf-8"></script>--%>
	<!-- 导航结束 -->
	<%--<% if (carIDAndName != "")
	{ %>
	<script type="text/javascript" src="http://api.car.bitauto.com/CarInfo/GetCarInfoForCompare.aspx?isParamPage=1&carids=<%= carIDs %>"></script>
	<% } %>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/mpeizhi.min.js?v=20170208"></script>
<%--    <script type="text/javascript" src="/Js/mpeizhi.js"></script>--%>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/iscroll.js?v=2015112366"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/underscore.js?v=2015112366"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/model.js?v=2015112366" ></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/rightswipe.js?v=2015112366"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js?v=20170105"></script>
        <!--车款模板 end-->
        <script type="text/javascript">
        	$(function () {
        		//api.model.currentid = '<%= carIDs %>';
            	//车款点击回调事件
            	api.model.clickEnd = function (paras) {
            		//车款ID
            		var isReturn = false;                    
            		for (var i = 0; i < ComparePageObject.arrCarIds.length; i++) {
            			if(ComparePageObject.arrCarIds[i] == paras.modelid)
            			{
            				isReturn = true;
            			}                        
            		}
            		if(isReturn)
            		{
            			return;
            		}
            		//api.model.currentid = paras.modelid;
            		var $back = $('.' + $leftPopupModels.attr('data-back'));
            		//关闭浮层
            		$back.trigger('close');
            		var currentIndex = "-1";
            		if(paras.carobj.attr("data-car"))
            		{
            			currentIndex = paras.carobj.attr('data-index');
            		}
            		selectCarId(paras.modelid, currentIndex);                   
            	}
            })
           
    </script>	
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/ajax.js?v=201209"></script>--%>
	<script type="text/javascript">
		<%--ComparePageObject.CarIDAndNames = "<%= carIDAndName %>";
		 	ComparePageObject.CurrentCarIDs = '<%= hotCarIDs %>';
		 	initPageForCompare();
		--%>
		var serialId=<%= se.Id %>;
		function initCarInfo(carId) {
			loadJS.push("http://api.car.bitauto.com/CarInfo/GetCarParameter.ashx?isParamPage=1&carids=" + carId, "utf-8", function () {
				initPageForCompare();
			});
		}
		var carIds = '<%=carIDs %>';
		var carArray = [];
		if (carIds.indexOf(",") != -1) {
			carArray = carIds.split(",");
		}else{carArray.push(carIds);}
		var topCarIds = carArray.length >=3 ? carArray.slice(0,3).join(",") : topCarIds = carArray.join(",");
		initCarInfo(topCarIds);
	</script>
	

	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=201209"></script>--%>
	<script type="text/javascript">
		var CarCommonCSID = '<%= csID.ToString() %>';
	    var url = "http://car.m.yiche.com/<%= se.AllSpell %>/";

        //set canshu button href
	    $(function() {
	        var dict = {"3814":"yidongliangxiang","3524":"qiruim16"};
	        if (CarCommonCSID == '3814' || CarCommonCSID == '3524') {
                var canshuBtn = $("#m-car-nav").find("ul li.current a");
                $(canshuBtn).attr("href","/"+dict[CarCommonCSID]+"/peizhi");
            }
        })
	</script>
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=csID%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
</body>
</html>

