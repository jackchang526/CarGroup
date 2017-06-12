﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompare.aspx.cs" Inherits="WirelessWeb.CarCompare" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BitAuto.CarChannel.Model" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0"/>
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
    <title>【<%=ce.Serial.SeoName%><%=ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>参数配置表_<%=ce.Serial.SeoName %>发动机配置-手机易车网</title>
	<meta name="Keywords" content="<%= ce.Serial.SeoName%>参数,<%= ce.Serial.SeoName%>配置,<%=ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>,<%= ce.Serial.SeoName%>参数配置表,<%= ce.Serial.SeoName%>发动机配置,易车网,car.m.yiche.com" />
	<meta name="Description" content="<%=ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>配置,手机易车网<%=ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>配置参数表,包括,<%= ce.Serial.SeoName%>发动机,<%= ce.Serial.SeoName%>变速箱,<%= ce.Serial.SeoName%>车轮,<%= ce.Serial.SeoName%>灯光等配置参数。" />
	<!--#include file="~/ushtml/0000/myiche2015_cube_chekuanpeizhi-1020.shtml"-->   
</head>
<body>
	<script type="text/javascript">
		var carlevel = '<%=ce.Serial.Level.Name %>';
	</script>
    <div id="container">
	<div class="mbt-page">
 		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
 			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%=ce.Serial.ShowName %></h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
		<!-- 选择车款 start -->
        <a href="###" class="change-car-top" data-action="firstmodel" data-id="<%=ce.SerialId%>">
            <span>车款</span>
            <strong><%=ce.CarYear>0?ce.CarYear+"款 ":"" %><%=ce.Name %></strong>
        </a>
		<!-- 选择车款 end -->
		<!-- tabs start -->
		<div class="first-tags">
			<ul>
                <li><a href="/<%=ce.Serial.AllSpell%>/m<%= carID %>"><span>综述</span></a></li>
				<li><a href="http://price.m.yiche.com/nc<%= carID %>/"><span>报价</span></a></li>
				<li class="current"><a href="/<%=ce.Serial.AllSpell%>/m<%= carID %>/peizhi"><span>配置</span></a></li>
                <li><a href="http://photo.m.yiche.com/car/<%= carID %>/"><span>图片</span></a></li>
				<%--<li><a href="/<%=ce.Serial.AllSpell %>/m<%= carID %>/tupian"><span>图片</span></a></li>--%>
			</ul>
		</div>
		<!-- tabs end -->
		<div>
			<div class="chek-peizhi">
				<%=carAllParamHTML %>
				<div class="m-statement">
					以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a href="http://m.yiche.com/wap2.0/feedback/" style="color:#000">欢迎您及时指正！</a>
				</div>
			</div>
			<div class="sum-btn sum-btn-one">
                <ul>
                    <li>
                       <%-- <a href="/chexingduibi/?carIDs=<%=carID %>" data-action="car">加入对比</a>--%>
                         <a href="javascript:void(0)" id="car-compare-<%=carID%>" data-action="car" data-id="<%= carID %>" data-name="<%=ce.Serial.Name +" "+ ce.Name%>">加入对比</a>
                    </li>
                </ul>
			</div>
		</div>
        <div class="float-catalog" id="popMenu">
	        目录
	        <div class="catalog-list" id="menu" style="display:none">
		        <ul>
			          <%=carAllParamMenu %>
		        </ul>
		        <div class="ico-catalog-arrow"></div>
	        </div>
        </div>
	</div>
    <div class="float-catalog-mask" id="popup-menumask" style="display:none"></div>
	<!--#include file="~/include/pd/2012/wap/00001/201317_wap_cxy_adb_Manual.shtml"-->
    <!-- black popup end -->
	<!--新加弹出层 start-->
    <div id="master_container"  style="z-index:888888;display:none" class="brandlayer mthead">
        	<!--#include file="~/html/compareCarTemplate.html"-->
    </div>
    <!--新加弹出层 end-->
    <!--车款层 start-->
    <div class="leftmask mark leftmask1" style="display: none;"></div>
    <div id="popupCar" class="leftPopup car-model models original firstmodel" data-back="leftmask1" style="display: none" data-key="model">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--车款层 end-->
    <!--loading模板 start -->
    <div class="template-loading" style="display: none;">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt=""/>
            <p>正在加载...</p>
        </div>
    </div>
    <!--loading模板 end -->
    <!--车款模板 start-->
    <script type="text/template" id="firstModelTemplate">
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
    <!--车款模板 end-->

    <!--一级公共模板 start-->
    <script type="text/template" id="duibibtnTemplate">
        <div class="y2015-car-01">
            <div class="slider-box">
                <ul class="first-list">
                    {for(var i =0;i< list.SelectedCars.length;i++){}
                       <li><div class="line-box"><a class='select' href="/m{= list.SelectedCars[i].CarId }/">{=list.SelectedCars[i].CarName }</a><a href="javascript:;" data-carid="{=list.SelectedCars[i].CarId }" class="btn-close"><i></i></a></div></li>
                    {}}
                    {for(var i =0;i< list.AddLables.Count;i++){}
                        <li class="add"><div class="line-box"><a href="javascript:;">添加对比车款</a></div></li>
                    {}}
                    <li class="alert">最多对比4个车款</li>
                </ul>
            </div>
        </div>
        <div class="swipeLeft-header">
            <a href="#" class="btn-clear">清除</a>
            <!--btn-clear disable-->
            <a href="#" class="btn-comparison">开始对比</a>
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
        <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml "-->    
        <a id="compare-pk" href="#compare" data-action="duibicar" class="float-r-ball float-pk">    
            <span><p>对比</p></span>    
            <i></i>    
        </a>    
    </div>   
         
        <script type="text/javascript">
        	var CarCommonCarID = '<%= carID.ToString() %>';
        	var url = "http://car.m.yiche.com/<%= ce.Serial.AllSpell %>/";
	</script>
	<!--footer start-->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <!--footer end-->
</div>
        
    <script type="text/javascript">    
    	$(document).on('touchstart', function (event) {
    		var clickEle = event.target.id;
    		if((clickEle!="popMenu"&&event.target.parentNode.id!="popMenu")&&
                ($(event.target).closest(".catalog-list").attr("id")!="menu")){
    			$(".catalog-list").hide();
    			$("#popup-menumask").hide();
    		}
    	});
    	//$("#popMenu").click(function () {
    	//    //$(".catalog-list").toggle();
    	//    var $menu_list = $(".catalog-list");
    	//     if ($menu_list.is(":hidden")) {
    	//            $menu_list.show();
    	//            $("#popup-menumask").show();
    	//        } else {
    	//            $menu_list.hide();
    	//            $("#popup-menumask").hide();
    	//        }
    	//});
    	$(function() {
    		var $menu_list = $("#menu");
    		$("#popMenu").off("click").on("click", function(event) {
    			var target = event.srcElement || event.target;
    			if (target.id != "popMenu") return;
    			if ($menu_list.is(":hidden")) {
    				$menu_list.show();
    				$("#popup-menumask").show();
    			} else {
    				$menu_list.hide();
    				$("#popup-menumask").hide();
    			}
    		});
    	});
    	$(".leftmask").click(function () {
    		$(".swipeLeft").removeClass("swipeLeft-block");
    		$(this).hide();
    		$(".leftPopup").css("zIndex", 0).hide();
    	});
    	//弹出层中菜单选项按下事件
    	$("#menu li").click(function() {
    		$("#menu li").removeClass("current");
    		$(this).addClass("current");
    		$(".catalog-list").hide();	
    		$("#popup-menumask").hide();
    	});
    </script>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=201209"></script>--%>
     <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll-infinite.js?v=201509020950"></script>--%>
   	<%--
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompare.js?v=2015090814"></script>--%>

    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/iscroll.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/underscore.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/rightswipe.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=20160118"></script> 
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/note.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/brand.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompareV2.js?v=2016011818"></script>--%>
 	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js,carchannel/wirelessjs/v2/note.js,carchannel/wirelessjs/v2/brand.js,carchannel/wirelessjs/waitcompareV2.js?v=20170105"></script>
    <script type="text/javascript">
    	$(function () {
    		var $body = $('body');
    		$body.trigger('rightswipe1',{
    			actionName: '[data-action=firstmodel]',
    			fliterTemplate:function (templateid, paras) {
    				return "#firstModelTemplate";
    			}});
    		api.model.currentid = '<%=carID %>';
            	//车款点击回调事件
                api.model.clickEnd = function (paras) {
                	//车款ID
                	api.model.currentid = paras.modelid;
                	var $back = $('.' + $leftPopupModels.attr('data-back'));
                	$back.trigger('close');
                	_commonSlider($("[data-action=firstmodel]"),$body);
                	setTimeout(function(){ document.location.href = "/<%=ce.Serial.AllSpell %>/m"+ paras.modelid +"/peizhi/";},500);    
                }
            })

			//层自适应			var _commonSlider = function ($model, $body) {
				if ($model.height() > $(document.body).height()) {
					$(document.body).height($model.height())
				} else if ($model.height() < $(document.body).height()) {
					$('#container', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight);
					$('.brandlist').height(document.documentElement.clientHeight);
				}
			}
    </script>
    <script type="text/javascript">
    	var apiBrandId='<%=ce.Serial.BrandId%>';
    	var apiSerialId='<%=ce.SerialId%>';
    	var apiCarId='<%=carID%>';
    	var CarCommonCSID = '<%= ce.Serial.Id.ToString(CultureInfo.InvariantCulture) %>';
    	var compareConfig = {
    		serialid: CarCommonCSID
    	};
    	$(function(){
    		WaitCompare.initCompreData(compareConfig);
    	}); 
    </script>

    <script type="text/javascript">
    	var citySpell = "beijing",cityId=201;
    	if (typeof bit_locationInfo != 'undefined' && bit_locationInfo != null && typeof bit_locationInfo.engName != "undefined")
    	{ citySpell = bit_locationInfo.engName; cityId = bit_locationInfo.cityId;}
    	function getDemand(serialId, serialSpell, cityId) {
    		$.ajax({ url: 'http://api.car.bitauto.com/mai/GetSerialDemand.ashx?serialId=' + serialId + '&cityid=' + cityId
			, async: false, cache: true, dataType: "jsonp", jsonpCallback: "newDemandCallback", success:
			function (data) {
				var hasDemand = false;
				if (typeof data != 'undefined' && data != null
				&& typeof data.DealerCount != 'undefined' && data.DealerCount != null && data.DealerCount > 0)
				{ hasDemand = true; }
				if (hasDemand) {
					var cityName = "北京";
					if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined")
					{ cityName = cityIDMapName[cityId]; }
					var citySpell = "beijing";
					if (typeof cityIDMapSpell != 'undefined' && cityIDMapSpell != null && typeof cityIDMapSpell[cityId] != "undefined")
					{ citySpell = cityIDMapSpell[cityId]; }

					//添加车型特卖
					var carId =<%=carID%>;
			        if (typeof data.CarList != 'undefined' && data.CarList != null) {
			        	$.each(data.CarList, function (i, n) {
			        		if ( n.CarID ==carId)
			        		{
			        			$("#liDemand").html("<a href=\"http://mai.m.yiche.com/detail-<%= ce.Serial.Id %>.html?leads_source=21102&city=" + citySpell + "#<%=carID %>\">特卖</a>");
			                }
			            });
					}
			        
				}
			}
            });
	}
	getDemand(<%=ce.Serial.Id %>,'<%=ce.Serial.AllSpell %>',bit_locationInfo.cityId);
    	<%--(function(){
			//购车服务 2015.5.16
			$.ajax({
				url: "http://api.gouche.yiche.com/PreferentialCar/GetCarByCarId?carid=" + CarCommonCarID + "&cityid=" + cityId + "", cache: true, dataType: "jsonp", jsonpCallback: "guocheCallback", success: function (data) {
					if (data != null) {
						$(".chek-card .fir-item").append("<a href=\"http://gouche.m.yiche.com/" + citySpell + "/sb<%=ce.Serial.Id %>/m"+CarCommonCarID+"/\" class=\"ico-66gouche\">66购车节</a>");
					}
				}
			});
		})();--%>
    </script>
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=ce.SerialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
		var zamplus_tag_params = {
			modelId:modelStr,
			carId:<%=carID%>
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



   

        



