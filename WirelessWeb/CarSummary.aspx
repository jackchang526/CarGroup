<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarSummary.aspx.cs" Inherits="WirelessWeb.CarSummary" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>【<%=Ce.Serial.SeoName + Ce.Name%>车款综述】_<%=Ce.Serial.Brand.SeoName %>-手机易车网</title>
    <meta name="Keywords" content="<%=Ce.Serial.SeoName%>,<%=Ce.Serial.SeoName%>车款,<%=Ce.Serial.SeoName%>参数,<%=Ce.Serial.SeoName%>配置,<%=Ce.Serial.Brand.SeoName+Ce.Serial.SeoName%>" />
    <meta name="Description" content="<%=Ce.Serial.SeoName%>:手机易车网车型频道为您提供全国最新<%=Ce.Serial.SeoName%>报价,权威<%=Ce.Serial.SeoName%>配置,海量<%=Ce.Serial.SeoName%>图片" />
    <!--#include file="~/ushtml/0000/myiche2015_cube_chekuan-1019.shtml"-->
    <style>
    	.ep_line_box h1 { font-size: 1.4rem; padding: 15px; background: #fff; }
    </style>
	 <script type="text/javascript" charset="utf-8" src="http://ip.yiche.com/iplocation/setcookie.ashx"></script>
</head>

<body>
    <!--选车款 start-->
   <%-- <div id="sel-container" style="display: none;">
        <div id="sel-cur-serial"></div>
        <div id="sel-master"></div>  
        <div id="sel-serial"></div>
        <div id="sel-car"></div>
        <div id="sel-history"></div>
    </div>--%>
    <!--选车款 end-->

    <div id="container">
 		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
 			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><a data-channelid="27.24.1322" href="/<%=Ce.Serial.AllSpell %>/"><%=Ce.Serial.ShowName %></a></h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
        <input id="hidCsId" type="hidden" value="<%=Ce.SerialId%>" />
        <input id="hidCarType" type="hidden" value="默认" />
        <input id="hidCarID" type="hidden" value="<%=CarId %>" />
        <input id="isElectric" type="hidden" value="<%=IsElectrombile %>" />
        <input id="isTingShou" type="hidden" value="<%=Ce.SaleState.Trim() %>" />
        <input id="hidAllSpell" type="hidden" value="<%=Ce.Serial.AllSpell %>" />
        <script type="text/javascript">
        	var BitautoCityForAd = bit_locationInfo.cityName;
        	var carlevel='<%=Ce.Serial.Level.Name%>';
        	if ($("#isElectric").val() == "True") {
        		$("#hidCarType").val("电动");
        	} else if ($("#isTingShou").val() == "停销") {
        		$("#hidCarType").val("停销");
        	}
        	function showNewsInsCode(dxc, xxc, mpv, suv) {
        		var adBlockCode = xxc;
        		if (carlevel == '中大型车' || carlevel == '中型车' || carlevel == '跑车' || carlevel == '豪华车') {
        			adBlockCode = dxc;
        		}
        		else if (carlevel == '微型车' || carlevel == '小型车' || carlevel == '紧凑型车') {
        			adBlockCode = xxc;
        		}
        		else if (carlevel == '概念车' || carlevel == 'MPV' || carlevel == '面包车' || carlevel == '皮卡' || carlevel == '其它') {
        			adBlockCode = mpv;
        		}
        		else if (carlevel == 'SUV') {
        			adBlockCode = suv;
        		}
        		document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
        	}
        </script>
        <!-- return block end -->

        <!-- 切换年款 start -->
        <a href="###" class="change-car-top" data-action="firstmodel"  data-channelid="27.24.1325"  data-id="<%=Ce.SerialId%>">
            <span>车款</span>
            <strong><%=Ce.CarYear>0?Ce.CarYear+"款 ":"" %><%=Ce.Name %></strong>
        </a>
        <!-- 切换年款 end -->

        <div class="first-tags">
            <ul>
                <li class="current"><a href="http://car.m.yiche.com/<%=Ce.Serial.AllSpell %>/m<%=Ce.Id %>"><span>综述</span></a></li>
                <li><a href="http://price.m.yiche.com/nc<%=Ce.Id %>/" data-channelid="27.24.1323"><span>报价</span></a></li>
                <li><a href="/<%=Ce.Serial.AllSpell %>/m<%=Ce.Id %>/peizhi/" data-channelid="27.24.1326"><span>配置</span></a></li>
                <li><a href="http://photo.m.yiche.com/car/<%=Ce.Id %>/"  data-channelid="27.24.1327"><span>图片</span></a></li>
            </ul>
        </div>

        <%--<div class="m-focus">
            <div class="m-focus-box">
                <div class="m-focus-scroll-box swiper-container">
                    <ul class="swiper-wrapper">
                        <li class="swiper-slide">
                            <a href="<%=ImgLink %>" data-channelid="27.24.1328">
                                <img src="<%=PicUrl %>" alt="<%=Ce.Name %> 图片">
                                <div class="mask"></div>
                                <div class="mask-tt"><span><%=CarName %></span></div>
                            </a>
                        </li>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </div>--%>

        <!-- 价格信息 start -->
        <ul id="ulJiaGeInfo" class="chek-card">
        </ul>

        <%--<div id="erShouCheDiv" class="sum-btn sum-btn-two" style="display: none">
            <ul>
                <li><a data-channelid="27.24.128" href="http://m.taoche.com/pinggu/?ref=mchekuanzsgu">二手车估价</a></li>
                <li class="btn-org"><a data-channelid="27.24.127" href="http://m.taoche.com/all/?carid=<%= CarId %>/?ref=mchekuanzsmai&leads_source=m003013">买二手车</a></li>
            </ul>
        </div>

        <div id="notBaoXiaoDiv" class="sum-btn" style="display: none">
            <ul>
                 <li><a data-channelid="27.24.124" href="http://chedai.m.yiche.com/<%=Ce.Serial.AllSpell %>/m<%= CarId %>/chedai/?from=ycm34&leads_source=m003001">贷款</a></li>
                <li><a data-channelid="27.24.125" href="http://zhihuan.m.taoche.com/c<%= CarId %>/?ref=mchekuanzshuan&leads_source=m003002">置换</a></li>
                <li class="btn-org"><a data-channelid="27.24.126" href="http://price.m.yiche.com/zuidijia/nc<%= CarId %>/?leads_source=m003003">询底价</a></li>
            </ul>
        </div>

        <div id="baoxiaoDiv" class="sum-btn sum-btn-two" style="display: none">
            <ul>
                <li><a data-channelid="27.24.124" href="http://chedai.m.yiche.com/<%=Ce.Serial.AllSpell %>/m<%= CarId %>/chedai/?from=ycm34&leads_source=m003001">贷款</a></li>
                <li class="btn-org"><a data-channelid="27.24.130" href="http://m.yichemall.com/car/Detail/Index?carId=<%=CarId %>&source=100546&leads_source=m003012">直销特卖</a></li>
            </ul>
        </div>--%>
		<div id="btn-business" class="sum-btn"></div>
        <!-- 价格信息 end -->
        <!-- 经销商推荐 start -->
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/common/cityjsonv2.js?v=2016012210"> </script>
        <div id="tuiJianHeader" class="tt-first" data-channelid="27.24.1332">
            <h3>经销商推荐</h3><div id="changecity" class="opt-city"><a href="javascript:;" id="m-dealer-city" data-action="province">北京<i></i></a></div>
        </div>
        <script type="text/javascript">
        	var citycode = 201,cityName='北京';
        	if (typeof(bit_locationInfo) != "undefined" && typeof(bit_locationInfo.cityId) != "undefined") {
        		citycode = bit_locationInfo.cityId;
        		cityName=bit_locationInfo.cityName;
        	}
        	$("#m-dealer-city").html(cityName+"<i></i>");
        	document.write('<ins Id="ep_union_126" Partner="6" Version="" isUpdate="1" type="4" city_type="1" city_id="' + citycode + '" city_name="0" car_type="3" brandId="0" serialId="0" carId="<%=CarId %>"></ins>');
        	document.write('<a id="moreJingXS" href="http://price.m.yiche.com/nc<%=CarId %>_c' + citycode + '/" class="btn-more"><i>查看更多经销商</i></a>');
         </script>
        <!-- 经销商推荐 end -->
        <!--替换原“ 购车服务” 块 -->
        <div class="car-service" style="display: none">
        </div>

        <!-- 限时特惠 start -->
        <div class="prefer-tim"></div>
        <!-- 限时特惠 end -->
		<script type="text/javascript">
			showNewsInsCode('480997b8-dfa9-46d1-aa99-57f78a566320', '5d4ecc24-ea5c-41f7-ad42-a29102ce1e08', '72d7d82c-dd8a-4f98-88b8-2c6f221aed7f', 'b13b0006-3a09-4f2e-982f-453bc9569144');
		</script>
        <!-- 参数配置 start -->
        <div class="tt-first tt-first-no-bd">
            <h3>参数配置</h3>
            <div class="opt-more opt-more-gray"><a href="/<%=Ce.Serial.AllSpell %>/m<%=CarId %>/peizhi/">更多</a></div>
        </div>
        <div class="car-canshu">
            <%if (IsElectrombile)
			  { %>
            <dl>
                <dt class="w5">百公里耗电：</dt>
                <dd><%= string.IsNullOrEmpty(PowerConsumptive100) ? "" : PowerConsumptive100 + "kW"%></dd>
            </dl>
            <dl>
                <dt class="w4">电池容量：</dt>
                <dd><%= string.IsNullOrEmpty(BatteryCapacity) ? "" : BatteryCapacity + "kW·h"%></dd>
            </dl>
            <dl>
                <dt class="w4">续航里程：</dt>
                <dd><%= string.IsNullOrEmpty(Mileage) ? "" : Mileage+"公里"%></dd>
            </dl>
            <dl>
                <dt class="w4">充电时间：</dt>
                <dd><%= string.IsNullOrEmpty(ChargeTime) ? "" : ChargeTime + "分钟"%></dd>
            </dl>
            <%}
			  else
			  { %>
            <dl>
                <dt class="w2">排量：</dt>
                <dd><%=Exhaust %></dd>
            </dl>
            <dl>
                <dt class="w2">油耗：</dt>
                <dd><%= Cfcs.CarSummaryFuelCost == "" ? "暂无" : Cfcs.CarSummaryFuelCost + "/100km"%></dd>
            </dl>
            <dl>
                <dt class="w3">发动机：</dt>
                <dd><%= EngineAllString%></dd>
            </dl>
            <dl>
                <dt class="w3">变速箱：</dt>
                <dd><%= Cfcs.TransmissionType%></dd>
            </dl>
            <%} %>
            <dl class="line">
                <dt class="w3">颜色：</dt>
                <%=ColorHeader %>
            </dl>
        </div>

        <div class="sum-btn sum-btn-one">
            <ul>
                <li data-channelid="27.24.1331">
                    <a id="car-compare-<%=CarId %>" href="javascript:;" data-action="car" data-id="<%=CarId %>" data-name="<%=Ce.Serial.Name + Ce.Name %>">加入对比</a>
                </li>
            </ul>
        </div>
        <!-- 参数配置 end -->

        <!-- 二手车 start -->
        <div id="ershouCheT" class="tt-first" style="display: none">
        </div>
        <div id="ershouCheContent" class="pic-txt pic-txt-car" style="display: none">
        </div>
        <!-- 二手车 end -->
		
       
        <%--<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>--%>
        <script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/eppu.js?v=20151104"></script>
        <script type="text/javascript">
        	(function () {
        		var cityId = 201, cityAllSpell = 'beijing';
        		if (bit_locationInfo && bit_locationInfo.cityId) {
        			cityId = bit_locationInfo.cityId;
        			cityAllSpell = bit_locationInfo.engName;
        		}
        		//商城接口(包销)
        		$.ajax({
        			url: "http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=" + <%=Ce.SerialId %> + "&cityid=" + cityId,
        			async: false,
        			dataType: "jsonp",
        			jsonpCallback: "mall",
        			success: function (data) {
        				var mallPrice="暂无";
        				//包销车或者平行进口车
        				if (data.CarList.length > 0) {
        					for (var i = 0; i < data.CarList.length; i++) {
        						if (data.CarList[i].CarId == $("#hidCarID").val()) {
        							if (data.CarList[i].CarType == 0) {
        								$("#hidCarType").val("包销");
        								mallPrice=data.CarList[i].Price;
        								break;
        							}
        							else if (data.CarList[i].CarType == 1) {
        								$("#hidCarType").val("平行进口");
        								break;
        							}
        						}
        					}
        				}

        				//价格区
        				if ($("#hidCarType").val() == "停销") {
        					var erShouCheHtml = [];
        					erShouCheHtml.push("<li>二手车报价：");
        					erShouCheHtml.push("<a href=\"http://yiche.taoche.com/buycar/b-<%=Ce.Serial.AllSpell %>/?page=1&carid=<%= CarId %>\"><strong><%=UcarPrice %></strong></a>");
        					erShouCheHtml.push("<em>(指导价<%= Ce.ReferPrice > 0 ? Ce.ReferPrice+"万元" : "暂无"%>)</em></li>");
        					erShouCheHtml.push("<li>");
        					$("#ulJiaGeInfo").html(erShouCheHtml.join(""));
        					$("#erShouCheDiv").show();
        				} else {
        					var contentHtml = [];
        					if ($("#hidCarType").val() == "包销") {
        						contentHtml.push("<li><i>商城直销价：</i>");
        						contentHtml.push("<a href=\"http://m.yichemall.com/car/Detail/Index?carId=<%=CarId %>&source=100546\"><strong>"+mallPrice+"万元</strong></a>");
							} else {
								contentHtml.push("<li><i>参考成交价：</i>");
								contentHtml.push("<a data-channelid=\"27.24.1329\" href=\"/<%=Ce.Serial.AllSpell %>/m<%=Ce.Id %>/baojia/\"><%= CankaoPrice == "暂无"?"暂无": "<strong>"+CankaoPrice + "元</strong>" %></a>");
							}
                        
							contentHtml.push("<em>(指导价<%= Ce.ReferPrice > 0 ? Ce.ReferPrice+"万元" : "暂无"%>)</em></li>");
        					contentHtml.push("<li>全款：<span>约<%=TotalPay%></span> <em>(仅供参考)</em><a href=\"/gouchejisuanqi/?carid=<%=CarId %>\" data-channelid=\"27.24.1330\" class=\"m-ico-calculator\"></a></li>");
        					contentHtml.push("<li>贷款：<span>首付<%=LoanDownPay %>，月供<%=MonthPay %></span> <em>(36期)</em></li>");
        					if("<%=TaxContent %>" !="" &&"<%=Ce.SaleState.Trim() %>" == "在销")
        					{
        						contentHtml.push("<li><em class=\"m-ico-jianshui\">减税</em><%= TaxContent%></li>");
        					}
							$("#ulJiaGeInfo").html(contentHtml.join(""));
							if ($("#hidCarType").val() == "包销") {
								$("#baoxiaoDiv").show();
							}
							else if("<%=Ce.Serial.SaleState %>" == "待销"){   //车系的销售状态cssalestate=“待销”时，车款为“未上市”状态;未上市不显示按钮
								return;
							}
							else {
								$("#notBaoXiaoDiv").show();
							}
						}

        				//二手车源
        				if ($("#hidCarType").val() == "停销") {
        					$("#ershouCheT,#ershouCheContent").show();
        					$.ajax({
        						url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/CarListForYiChe.ashx?sid=<%=Ce.SerialId %>",
        						cache: true,
        						dataType: "jsonp",
        						jsonpCallback: "callback",
        						success: function (data) {
        							data = data.CarListInfo;
        							if (data.length <= 0) return;
        							var strHtml1 = [];
        							var strHtml2 = [];
        							strHtml1.push("<div class=\"tt-first\">");
        							strHtml1.push("<h3>二手车源</h3>");
        							strHtml1.push("<div class=\"opt-more opt-more-gray\"><a href=\"http://m.taoche.com/" + $("#hidAllSpell").val() + "/?WT.mc_id=\">更多</a></div>");
        							strHtml1.push("</div>");

        							strHtml2.push("<div class=\"pic-txt pic-txt-car\">");
        							strHtml2.push("<ul>");
        							$.each(data, function (i, n) {
        								if (i > 3) return;
        								strHtml2.push("<li>");
        								strHtml2.push("<a href=\"" + n.CarlistUrl.replace(".taoche.com", ".m.taoche.com") + "\">");
        								strHtml2.push("<span>");
        								strHtml2.push("<img src=\"" + n.PictureUrl + "\">");
        								strHtml2.push("</span>");
        								strHtml2.push("<p>" + n.CarName + "</p>");
        								strHtml2.push("<b>" + n.DisplayPrice + "</b>");
        								strHtml2.push("<em>" + n.BuyCarDate + "/" + n.DrivingMileage + "公里</em>");
        								strHtml2.push("</a>");
        								strHtml2.push("</li>");
        							});
        							strHtml2.push("</ul>");
        							strHtml2.push("</div>");

        							$("#ershouCheT").html(strHtml1.join(""));
        							$("#ershouCheContent").html(strHtml2.join(""));
        						}
        					});
						}

        				//购车服务
        				//GetCarServiceData();    
        				var curCarId=<%=CarId %>;    
        				var curSerialId = <%=Ce.SerialId %>;   
        				//统计
        				var channelIDs = {"3":"27.24.125",  "12": "27.24.132", "9": "27.24.129", "10": "27.24.130", "11": "27.24.131", "13": "27.24.133" };
        				var urlEndPartCode = { "3":"?ref=mchekuanzshuan&leads_source=m003002", "9": "&tracker_u=318_ckzs&leads_source=m003004", "10": "&source=100546&leads_source=m003005","11": "?ref=mcar2&rfpa_tracker=2_23&leads_source=m003006",  "12": "?ref=mchekuanzska&leads_source=m003007" };

        				//按钮统计
        				var global_busbtn_arr = ["1", "8", "0","6","5","7"];
        				var global_busbtn_channelids = { "0": "27.24.126", "1": "27.24.129", "8": "27.24.124","5":"27.24.128","6":"27.24.127","7":"27.24.130" };
        				var global_busbtn_code = { "0": "?leads_source=m003003", "1": "&tracker_u=614_ckzs&leads_source=m003004", "8": "?from=ycm34&leads_source=m003001","5":"?ref=mchekuanzsmai&leads_source=m003013","6":"?ref=mchekuanzsgu","7":"&source=100546&leads_source=m003012" };
    
        				$.ajax({    
        					url: "http://api.car.bitauto.com/api/GetBusinessService.ashx?date=20160721&action=mcar&cityid=" + cityId + "&serialid=" + curSerialId + "&carid="+curCarId,   
        					async: false,    
        					dataType: "jsonp",    
        					jsonpCallback: "businessCarCallBack",    
        					cache: true,    
        					success: function (data) {   
        						if(data&&data!=null){ 
        							var serviceHtml=[],						
        							btnHtml = [],
									btnCount = 0;
        							btnHtml.push("<ul>");
        							$.each(data.Button, function (i, n) {
        								if (i > 2) return;
        								if (global_busbtn_arr.indexOf(n.BusinessId) != -1) {
        									btnHtml.push("<li " + ((n.BusinessId == "0" || n.BusinessId == "5"|| n.BusinessId == "7") ? "class=\"btn-org\"" : "") + "><a data-channelid=\"" + (global_busbtn_channelids[n.BusinessId] || "") + "\" href=\'" + n.MobileUrl + (global_busbtn_code[n.BusinessId] || "") + "\'>" + n.LongTitle + "</a></li>");
        									btnCount++;
        								}
        							});
        							btnHtml.push("</ul>");
        							if (btnCount == 2) {
        								$("#btn-business").html(btnHtml.join('')).addClass("sum-btn-two");
        							} else {
        								$("#btn-business").html(btnHtml.join(''));
        							}
        							if(data.BigButton){
        								var bigBtnCnt=data.BigButton.length; //购车服务数量  
        								var forend=bigBtnCnt;//循环终止标记
        								if(bigBtnCnt>2)
        								{
        									forend=3;
        									serviceHtml.push("<ul class=\"three-service\">");
        								}
        								else if(bigBtnCnt>1){
        									serviceHtml.push("<ul class=\"two-service\">");
        								}
        								else
        								{
        									//购车服务块不显示
        									//$('.car-service').hide();
        									return; 
        								}
        								var i;
        								for(i=0;i<forend;i++){
        									var curChannelId=channelIDs[data.BigButton[i].BusinessId];//统计
        									serviceHtml.push("<li>");
                                        
        									if(data.BigButton[i].BusinessId=="13"){
        										serviceHtml.push("<a data-channelid=\""+curChannelId+"\" href=\""+data.BigButton[i].MobileUrl+"\"><strong>"+data.BigButton[i].Title+"</strong>");
        										serviceHtml.push("<em class=\"cGray\">"+data.BigButton[i].Price+"</em>");
        									}
        									else{
        										serviceHtml.push("<a data-channelid=\""+curChannelId+"\" href=\""+data.BigButton[i].MobileUrl+urlEndPartCode[data.BigButton[i].BusinessId]+"\"><strong>"+data.BigButton[i].LongTitle+"</strong>");
        										serviceHtml.push("<em>"+data.BigButton[i].Price+"</em>");
        									}
        									serviceHtml.push("</a>");
        									serviceHtml.push("</li>");
        								}
        								serviceHtml.push("</ul>");
        								$(".car-service").html(serviceHtml.join('')).show();
        								//add log statistic
        								Bglog_InitPostLog();
        							}
        						}
        					}    
        				});    

        				//经销商
        				if ($("#hidCarType").val() == "停销" || $("#hidCarType").val() == "包销") {
        					$("#tuiJianHeader,#ep_union_119,#moreJingXS").hide();
        				}
        			}
        		});
        	})()
        </script>
		<script type="text/javascript">
			showNewsInsCode('d5b63897-b9fe-4de8-a5ee-ea4f0ab97d56', '058f9880-76ca-4a3a-bfa4-9d355e1f7395', '81c5ea90-e1d3-44ea-a957-36beea9ec7aa', 'cd6edaab-a803-41a9-8b49-a699cc632537');
		</script>
        <!-- 口碑 start -->
        <%=KouBeiHtml %>
        <!-- 口碑 end --> 
        <!-- 应用推荐 start -->
          <!--# include file="/include/pd/2016/wap/00001/201606_wap_common_ydyy_Manual.shtml" -->
		<!--#include file="~/include/pd/2014/wap/00001/201507_wap_index_ydtj_Manual.shtml" -->
        <!-- 应用推荐 end -->
		<script type="text/javascript">
			showNewsInsCode('05aa42cc-b6f2-49cf-af36-a781a77fc39f', '9dd45090-2a81-4f59-a362-c1648fc37c57', 'e2dec2ca-5515-4fe2-9538-78dc97d8f1c2', '8c72285e-5e1b-4cdf-bdac-a8f36346bd00');
		</script>
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
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
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

         <!--弹出层 start-->
    <div class="leftmask leftmaskduibibtn" style="display: none;"></div>
    <div class="leftPopup model" data-back="leftmaskduibibtn" data-zindex="777777"  style="display: none;">
        <div class="swipeLeft">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--弹出层 end-->          <!--一级公共模板 start-->
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
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
            <p>正在加载...</p>
        </div>
    </div>
    <!--loading模板 end -->

    <!--footer start-->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <!--footer end-->

        <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
        <script src="http://image.bitautoimg.com/uimg/wap/js/idangerous.swiper-2.0.min.js"></script>
        <%--<script src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll-infinite.js?v=201509020950"> </script>
        <script src="http://image.bitautoimg.com/carchannel/WirelessJs/popup.js?v=201509020950"> </script>--%>

        <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js,carchannel/wirelessjs/v2/note.js,carchannel/wirelessjs/v2/brand.js,carchannel/wirelessjs/waitcompareV2.js,carchannel/wirelessjs/rightswipe-cityv2.js?v=20170105"></script>

     <%--   <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/iscroll.js?v=20160118"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/underscore.js?v=20160118"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/rightswipe.js?v=20160128"></script>--%>
        <!--车款模板 end-->
        <script type="text/javascript">
        	$(function () {
        		var $body = $('body');
        		$body.trigger('rightswipe1',{
        			actionName: '[data-action=firstmodel]',
        			fliterTemplate:function (templateid, paras) {
        				return "#firstModelTemplate";
        			}});
        		api.model.currentid = '<%=CarId %>';
        		//车款点击回调事件
        		api.model.clickEnd = function (paras) {
        			//车款ID
        			//console.log('车款ID：' + paras.modelid)
        			api.model.currentid = paras.modelid;
        			var $back = $('.' + $leftPopupModels.attr('data-back'));
        			//关闭浮层
        			$back.trigger('close');
        			_commonSlider($("[data-action=firstmodel]"),$body);
        			setTimeout(function(){ document.location.href = "/<%=Ce.Serial.AllSpell %>/m"+ paras.modelid +"/";},500);
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
        	// 应用下载
        	var mySwiperApp = new Swiper('#m-app-part-scroll', {
        		pagination: '.pagination-app',
        		loop: true,
        		grabCursor: true,
        		paginationClickable: true
        	});
        	$(document).ready(function () {
        		if ($("#m-app-part-scroll").find("li").length < 4) {
        			$(".pagination-app").hide();
        		};
        	});

        	$(function () {
        		//$('#master_container').loadHtml({end:function(html){ this.html(html);}});
        		//右侧侧附加选择层
        		(function () {
        			$('[data-action=popup-car]').rightSwipe({
        				clickEnd: function (b) {
        					var $leftPopup = this;
        					if (b) {
        						var $back = $('.' + $leftPopup.attr('data-back'))
        						$back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
        						var $swipeLeft = $leftPopup.find('.swipeLeft');

        						$leftPopup.myScroll = new IScroll($swipeLeft[0], {
        							probeType: 1,
        							snap: 'dd',
        							momentum: true,
        							click: true
        						});
        					} else {
        						$leftPopup.myScroll.scrollTo(0, 0, 0, false);
        					}
        				}
        			});
        		})();
        	});
        </script>
        <!-- black popup start -->
		<%--<div class="leftmask leftmas2" style="display: none;"></div>
		<div id="provinceList" class="leftPopup province" data-back="leftmas2" style="z-index: 40; display: none;">
		</div>
		<div id="cityList" class="leftPopup city" style="z-index: 50; display: none;">
			<div class="swipeLeft"></div>
		</div>--%>
        <!--省份层 start-->
    <div class="leftmask leftmask6" style="display: none;"></div>
    <div id="provinceList" class="leftPopup province year" data-back="leftmask6" style="display: none;">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--省份层 end-->

    <!--市层 start-->
    <div id="cityList" class="leftPopup month model2 city" data-back="leftmask6" style="display: none;">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--市层 end-->
        

		<!-- black popup end -->
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
			<a id="compare-pk"  href="#compare"  data-action="duibicar" class="float-r-ball float-pk">
				<span><p>对比</p></span>
				<i></i>
			</a>
		</div>
    </div>


    <%--<script src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=20160128"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/note.js?v=20160118"></script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/brand.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/wirelessjs/waitcompareV2.js?v=2016011818"></script>
	<script type="text/javascript" src="/js/rightswipe-cityV2.js?v=20160202"></script>--%>
    <script type="text/javascript">
    	var apiBrandId='<%=Ce.Serial.BrandId%>';
    	var apiSerialId='<%=Ce.SerialId%>';
    	var apiCarId='<%=CarId%>';
    	var compareConfig = {
    		serialid: <%=Ce.SerialId%>
    		};
    	$(function(){
    		WaitCompare.initCompreData(compareConfig);
    		//RigthSwipeCity.initCity();
            
    		//易团购
    		//var serialShowName='<%=Ce.Serial.ShowName%>';
    		var tuangouParam = 'mediaid=2&locationid=3&cmdId='+apiSerialId+'&cityId='+citycode ;
    		(function getYiTuanGou(params)
    		{
    			$.ajax({
    				url: "http://api.market.bitauto.com/MessageInterface/YiTuanGou/GetYiTuanGouUrl.ashx?" + params,//?mediaid=2&locationid=1&cmdId=3999&cityId=201
    				async: false,
    				dataType: "jsonp",
    				//jsonpCallback: "successHandler",
    				//cache: true,
    				success: function(data) {
    					var h=[];
    					if(data&&data.result=="yes"){
    						var tuangouUrl=data.url;
    						var slogan=data.slogan;
    						h.push("<span class=\"yh-tap\"></span>");
    						h.push("<div class=\"cont-box\">");
    						h.push("<h4>本地限时特惠</h4>");
    						h.push("<p>"+slogan+"</p>");
    						h.push("</div>");

    						$(".prefer-tim").html(h.join(''));
    						$(".prefer-tim").on("click",function(){
    							window.location.href=tuangouUrl;
    							//window.open(tuangouUrl,"_blank");
    						});
    					}
    				}
    			});
    		})(tuangouParam)
    	}); 
    </script>
   <%-- <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=2015070911"> </script>--%>
    <script type="text/javascript">
    	var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=Ce.SerialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
    	var zamplus_tag_params = {
    		modelId:modelStr,
    		carId:<%=CarId%>
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
