<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarSummary.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageCar.CarSummary" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%= cbe.Serial.ShowName%><%=cbe.Name %>报价_参数_油耗_图片】_<%= cbe.Serial.Brand.Name %>-易车网BitAuto.com</title>

	<!--#include file="~/ushtml/0000/yiche_2014_cube_carchekuan-726.shtml"-->
	<meta name="Keywords" content="<%= cbe.Serial.ShowName%>,<%= cbe.Serial.ShowName%>报价,<%= cbe.Serial.ShowName%>论坛,<%= cbe.Serial.ShowName%>图片,<%= cbe.Serial.ShowName%>油耗,<%= cbe.Serial.ShowName%>口碑,<%= cbe.Serial.ShowName%>视频,<%= cbe.Serial.ShowName%>参数" />
	<meta name="Description" content="<%= cbe.Serial.ShowName%>:易车网(BitAuto.com)车型频道为您提供全国最新<%= cbe.Serial.ShowName%>报价,海量<%= cbe.Serial.ShowName%>图片,热门<%= cbe.Serial.ShowName%>论坛,权威<%= cbe.Serial.ShowName%>参数配置、安全评测、油耗、口碑、问答、视频、经销商等,是全国数千万购车意向客户首选汽车导购网站。" />
	<link rel="canonical" href="http://car.bitauto.com/<%=cbe.Serial.AllSpell %>/m<%=carID %>/" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%= cbe.Serial.AllSpell %>/m<%= carID %>/" />
	<meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/carserial.aspx?modelid=<%= carID %>" />
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<input id="hidCarType" type="hidden" value="默认" />
	<input id="hidCarID" type="hidden" value="<%=carID %>" />

	<input id="isElectric" type="hidden" value="<%=isElectrombile %>" />
	<input id="isTingShou" type="hidden" value="<%=cbe.SaleState.Trim() %>" />
	<script type="text/javascript">
		if ($("#isElectric").val() == "True") {
			$("#hidCarType").val("电动");
		} else if ($("#isTingShou").val() == "停销") {
			$("#hidCarType").val("停销");
		}
		function BtHide(id) {
		    var Div = document.getElementById(id);
		    if (Div) {
		        Div.style.display = "none";
		    }
		}
		function BtShow(id) {
		    var Div = document.getElementById(id);
		    if (Div) {
		        Div.style.display = "block";
		    }
		}
	</script>

	<!--#include file="~/html/header2014.shtml"-->
	<!--a_d start-->
	<div class="bt_ad">
		<%=serialTopAdCode%>
	</div>
	<!--a_d end-->
	<%= CarHeadHTML %>

	<!--内容 start-->
	<div class="bt_page">
		<div class="col-all  clearfix">
			<!--左边内容 start-->
			<div class="col-con ">
				<div class="title-con">
					<div class="title-box">
						<h3><a href="/<%=cbe.Serial.AllSpell %>/"><%=cbe.Serial.ShowName%></a></h3>
						<ul class="title-tab">
							<li class=""><a id="car-pop" href="javascript:;" class="pop"><%=carName%><strong></strong></a>
								<div id="car-popbox" class="title-popbox title-popbox-model" style="display: none;">
									<ul>
										<%=carListHtml%>
									</ul>
								</div>
							</li>
						</ul>
						<div class="more db-more" id="car_filter_id_<%=carID %>">
							<div class="db-box">
								<div class="button_gray db-btn" id="carcompare_btn_new_<%=carID %>">
									<a target="_self" cid="<%=carID %>" href="javascript:;"><span>对比</span></a>
								</div>
								<%--<div id="divPlan" class="button_orange jhgm-btn"><a id="planBtn" href="javascript:void(0)">计划购买</a></div>--%>
							</div>
						</div>
					</div>
					<%--<script type="text/javascript">
						if ($("#hidCarType").val() == "停销") {
							$("#divPlan").hide();
						}
					</script>--%>
				</div>
				<!--卡片 start-->
				<div class="card-head-box ck-card-h clearfix">
					<div class="lef-box">
						<div class="img-box focus-img-pos">
							<div class="color-img-box" id="colorImg">
								<%=carColorUrls %>
							</div>
							<div class="img-txt">
								<span class="rig-link box-l-set"><a target="_blank" href="<%=ImgLink %>"><%=CarPicName %></a></span>
							</div>
							<a target="_blank" href="<%=ImgLink %>">
								<img src="<%=PicUrl %>" alt="" width="300" height="199">
							</a>
						</div>
						<div class="color-box" id="colorNav">
							<div class="color-sty clearfix">
								<%=carColorHtml %>
							</div>
						</div>
					</div>

					<div class="txt-box zs-m-card">
						<div id="jiaGeInfo" class="p-tit p-b-set">
							<%=cbe.SaleState=="停销"?"二手车报价：<strong>"+ucarPrice+"</strong>":"全国参考价：<strong>"+carPrice +"</strong>" %>
							<%if (TaxContent != "" && cbe.SaleState == "在销")
		 { %>
							<div class="jshui">
								<i>减税</i><div class="tc tc-xunjia" style="display: none">
									<div class="tc-box">
										<i></i>
										<p><%=TaxContent%></p>
									</div>
								</div>
							</div>
							<%} %>
						</div>
						<div id="jiaGeDetail" class="p-jg-box clearfix">
							<span class="s1">
								<i><%=cbe.SaleState=="待销"?"预售价":"厂商指导价" %>：</i>
								<em><%= cfcs.ReferPrice == "" ? "" : cfcs.ReferPrice + "万元"%></em>
							</span>
							<%if (cbe.SaleState.Trim() != "停销")
		 {%>
							<div class="s1 s-w tc-w-box">
								<i>全款购车参考：</i>
								<em id="quanKuan">
									<%=cfcs.CarTotalPrice == "" ? "" : cfcs.CarTotalPrice + "元"%>
									<a class="jsq-ico-sty" href="/gouchejisuanqi/?carid=<%= carID.ToString() %>" target="_blank"></a>
								</em>
								<!-- 贷款弹层 start-->
								<div id="loanLayer" class="tc tc-js-box" style="display: none">
									<div class="tc-box">
										<i></i>
										<div class="tc-con">
											<ul>
												<li>
													<span class="s-t">裸车价格：</span>
													<span class="s-p"><%=priceComputer.FormatCarPrice %></span>
												</li>
												<li>
													<span class="s-t">必要花费：</span>
													<span class="s-p"><%=priceComputer.FormatEssentialPrice %></span>
												</li>
												<li class="z-t">(包含购置税、上牌、车船使用税、交强险)
												</li>
												<li class="b-set">
													<span class="s-t">商业保险：</span>
													<span class="s-p"><%=priceComputer.FormatInsurance %></span>
												</li>
												<li class="t-set">
													<span class="s-t">全款购车价：</span>
													<span class="s-p"><%=cfcs.CarTotalPrice %></span>
												</li>
												<li>以上均为预估费用，仅供参考。
                                     <%--<span class="s-p"><a href="#">查看详情&gt;&gt;</a></span>--%>
												</li>
											</ul>
										</div>
									</div>
								</div>
								<!--弹层 end-->
								<script type="text/javascript">
									(function() {
										$("#quanKuan").mouseover(function() {
											$("#loanLayer").show();
										}).mouseout(function() {
											if ($(this).closest("#loanLayer").length == 0) {
												$("#loanLayer").hide();    
											}
										});
									})();
								</script>
							</div>
							<%}%>
						</div>
						<%if (cbe.SaleState.Trim() != "停销")
		{%>
						<div id="loanInfo" class="dk-box">
							<span class="s-box">
								<i>贷款首付：</i><em><%= priceComputer.LoanFirstDownPayments > 0 ? ((double)(priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai) / 10000).ToString("F2") + "万元" : "暂无"%></em>
							</span>
							<span class="s-box">
								<i>月供：</i><em><%= priceComputer.LoanMonthPayments > 0 ? priceComputer.LoanMonthPayments + "元" : "暂无"%></em><i> (36期)</i>
							</span>
							<span class="s-link" data-channelid="2.152.1543">
								<a href="http://www.daikuan.com/www/<%= cbe.Serial.AllSpell %>/m<%= carID %>/?from=yc18"
									target="_blank">贷款买车&gt;&gt;</a>
							</span>
						</div>
						<%}%>

						<%if (isElectrombile)
		{ %>
						<ul class="ul-set">
							<li class="l-set"><i class="i-set">充电时间：</i><span class="s-set"><%= string.IsNullOrEmpty(chargeTime) ? "暂无" : chargeTime + "分钟"%></span> </li>
							<li class="r-set"><i class="i-w">百公里耗电：</i><span><%= string.IsNullOrEmpty(powerConsumptive100) ? "暂无" : powerConsumptive100 + "kW"%></span> </li>
							<li class="l-set"><i class="i-set">续航里程：</i><span class="s-set"><%= string.IsNullOrEmpty(mileage) ? "暂无" : mileage+"公里"%></span> </li>
							<li class="r-set"><i class="i-w">电池容量：</i>
								<span class="s1"><%= string.IsNullOrEmpty(batteryCapacity) ? "暂无" : batteryCapacity + "kW·h"%></span>
								<span class="s-more" data-channelid="2.152.1544"><a target="_blank" href="http://car.bitauto.com/<%=cbe.Serial.AllSpell %>/m<%=cbe.Id %>/peizhi/">全部参数&gt;&gt;</a></span>
							</li>
						</ul>
						<%}
		else
		{ %>
						<ul class="ul-set">
							<li class="l-set"><i class="i-set">油耗：</i><span class="s-set"><%= cfcs.CarSummaryFuelCost == "" ? "暂无" : cfcs.CarSummaryFuelCost + "/100km"%></span> </li>
							<li class="r-set"><i class="i-w">变速箱：</i><span><%= cfcs.TransmissionType == "" ? "暂无" : cfcs.TransmissionType%></span> </li>
							<li class="l-set"><i class="i-set">排量：</i><span class="s-set"><%= exhaust == "" ? "暂无" : exhaust%></span> </li>
							<li class="r-set"><i class="i-w">发动机：</i><span class="s1"><%= EngineAllString == "" ? "暂无" : EngineAllString%></span><span class="s-more" data-channelid="2.152.1544"><a target="_blank" href="http://car.bitauto.com/<%=cbe.Serial.AllSpell %>/m<%=cbe.Id %>/peizhi/">全部参数&gt;&gt;</a></span></li>
						</ul>
						<%} %>
						<div id="lianJie" class="sc-btn-box">
							<%if (cbe.SaleState == "停销")
		 {%>
							<span class="button_orange btn-xj-w"><a target="_blank" href="http://www.taoche.com/buycar/b-<%= cbe.Serial.AllSpell %>/?page=1&leads_source=p003005&carid=<%= carID %>&ref=pc_yc_tszs_escbuycar">买二手车</a></span><span class="button_gray btn-qt-w btn-bq-w"><a target="_blank" href="http://www.taoche.com/pinggu/?ref=pc_yc_tszs_gujia">二手车估价</a></span>
							<%}
		 else if (cbe.SaleState == "在销")
		 { %>
							<span class="button_orange btn-xj-w" data-channelid="2.152.1538"><a target="_blank" href="http://dealer.bitauto.com/zuidijia/nb<%=cbe.SerialId%>/nc<%=cbe.Id%>/?T=2&amp;leads_source=p003001">询底价</a></span><%--<span class="button_gray btn-qt-w" data-channelid="2.152.1539"><a target="_blank" href="http://www.huimaiche.com/<%=cbe.Serial.AllSpell%>?carid=<%=cbe.Id%>&amp;tracker_u=609_ckzs&amp;leads_source=p003002">买新车</a></span>--%><span class="button_gray btn-qt-w btn-86" data-channelid="2.152.1540"><a target="_blank" href="http://www.daikuan.com/www/<%=cbe.Serial.AllSpell%>/m<%=cbe.Id%>/?from=yc18&amp;leads_source=p003003">贷款</a></span><span class="button_gray btn-qt-w btn-86" data-channelid="2.152.1541"><a target="_blank" href="http://zhihuan.taoche.com/?leads_source=p003004&amp;ref=chekuanzshuan&amp;serial=<%=cbe.SerialId%>">置换</a></span><span class="button_gray btn-qt-w btn-86" data-channelid="2.152.1542"><a target="_blank" href="http://www.taoche.com/<%=cbe.Serial.AllSpell%>/?leads_source=p003005">二手车</a></span>
							<%} %>
						</div>
					</div>
				</div>
				<!--卡片 end-->

				<!--参数配置 start-->
				<%= carConfigData %>
				<div class="addd_more_link  load_more" style="display: none"><a data-channelid="2.152.1446" href="javascript:void(0);">加载更多配置<em></em></a></div>
				<div class="tc-popup-box" id="popup-box" style="display: none;">
					<div class="tt">
						<h6>参数纠错</h6>
						<a href="javascript:;" class="btn-close">关闭</a>
					</div>
					<div class="tc-popup-con tc-popup-error-correction">
						<textarea id="correctError"></textarea>
						<div class="alert">
							<span></span>
						</div>
						<div class="button_orange button_99_35">
							<a href="javascript:;" name="btnCorrectError">提交</a>
						</div>
						<div class="button_gray button_99_35">
							<a href="javascript:;" id="btnErrorCancel">取消</a>
						</div>
					</div>
				</div>
				<div class="tc-popup-box" id="popup-box-success" style="display: none;">
					<div class="tt">
						<h6>参数纠错</h6>
						<a href="javascript:;" class="btn-close">关闭</a>
					</div>
					<div class="tc-popup-con">
						<div class="no-txt-box have-txt-box">
							<p class="tit">
								提交成功
							</p>
							<p>
								您提交的纠错信息我们已经收集到，感谢您的纠错。
							</p>
							<div class="button_gray button_94_35">
								<a href="javascript:;" id="btn-success-close">关闭</a>
							</div>
						</div>
						<div class="clear">
						</div>
					</div>
				</div>
				<!--参数配置 end-->

				<!-- 热门车型对比 -->
				<%= carHotCompareHtml %>

				<script type="text/javascript">
					(function() {
						var navList=$("#colorNav").find("span");
						var conMain=$("#colorImg");
						var conList=$("#colorImg").find(".img-wrap-b");
						navList.each(function(ind) {
							$(this).bind("mouseover", function() {
								var $divImg = conList.eq(ind);
								var imgSrcV = $divImg.find("img").attr("src");
								if (imgSrcV) {
									conMain.css("display", "block");
									$divImg.css("display", "block");    
								}
							}).bind("mouseout", function() {
								conMain.css("display", "none");
								conList.eq(ind).css("display", "none");
							});
						});
					})()
				</script>
				<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
				<script type="text/javascript">
					var carlevel= '<%=cbe.Serial.Level.Name%>';
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
					showNewsInsCode('540f1a0b-4eaf-4297-804e-f703866ef3d6', '57869922-fb6f-4044-be64-27d3dfa13e80', 'd3032347-a8df-4fc4-883e-7737baf660f4', 'c6856633-1ab1-49f7-b442-995c53d7559d');
				</script>
				<%if (cbe.SaleState.Trim() == "停销")
	  { %>
				<!--二手车-->
				<div class="line-box" id="ucarlist">
				</div>
				<%}
	  else
	  { %>
				<!--经销商报价列表 start-->
				<div class="line-box">
					<div class="title-con">
						<div class="title-box title-box2">
							<h4><a href="http://car.bitauto.com/<%= cbe.Serial.AllSpell %>/m<%= carID %>/baojia/">经销商报价</a></h4>
							<div class="more">
								<a target="_blank" href="http://www.bitauto.com/feedback/FAQ.aspx?col=9&tab=5">温馨提示</a> | 
                                <a target="_blank" href="http://www.bitauto.com/zhuanti/daogou/gsqgl/ ">购车流程</a>
							</div>
						</div>
					</div>
					<div class="jxs-box">
						<%--<ins id="ep_union_6" partner="1" version="" isupdate="1" type="4" city_type="-1"
                            city_id="0" city_name="0" car_type="3" brandid="0" serialid="0" carid="<%= carID %>"></ins>--%>
						<script type="text/javascript">
						    document.write('<ins Id=\"ep_union_135\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"4\" city_type=\"1\" city_id=\"'+bit_locationInfo.cityId+'\" city_name=\"0\" car_type=\"3\" brandId=\"0\" serialId=\"0\" carId=\"<%= carID %>\"></ins>');
                        </script>
					</div>
					<div class="clear"></div>

				</div>
				<!--经销商报价列表 end-->
				<%} %>
				<%--<script type="text/javascript">
					(function() {
						var cityId = bit_locationInfo.cityId;
						//商城接口(包销)
						$.ajax({
							url: "http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=<%=cbe.SerialId %>&cityid="+cityId,
							async: false,
							dataType: "jsonp",
							jsonpCallback: "mall",
							success: function(data) {
								var mallPrice=0;//商城直销价
								//包销车或者平行进口车
								if (data.CarList.length > 0) {
									for (var i = 0; i < data.CarList.length; i++) {
										if (data.CarList[i].CarId == $("#hidCarID").val()) {
											if (data.CarList[i].CarType == 0) {
												$("#hidCarType").val("包销");
												mallPrice=data.CarList[i].Price;
												break;
											} else if (data.CarList[i].CarType == 1) {
												$("#hidCarType").val("平行进口");
												break;
											}
										}
									}
								}
								//console.log($("#hidCarType").val());

								//价格区
								var jiaGeArray = [];
								if ($("#hidCarType").val() == "停销") {
									jiaGeArray.push('二手车报价：<strong><%=ucarPrice %></strong>');
								} else {
									if ($("#hidCarType").val() == "包销") {
										jiaGeArray.push('直&#12288;销&#12288;价：<strong id=\"mallPrice\"><a target="_blank" href="http://www.yichemall.com/car/detail/c_<%=carID%>/?source=yc-cxzs-onsale-1">'+ mallPrice +'万</a></strong>');
                        			} else {
                        				jiaGeArray.push('全国参考价：<strong><%=carPrice %></strong>');
                        			}
								}
								if ("<%=TaxContent %>" != "" && "<%=cbe.SaleState%>" == "在销")
								{
									jiaGeArray.push("<div class=\"jshui\"><i>减税</i><div class=\"tc tc-xunjia\" style=\"display:none\"><div class=\"tc-box\"><i></i><p><%=TaxContent%></p></div></div></div>");
                        		}
								$("#jiaGeInfo").html(jiaGeArray.join(""));
								$(".jshui").hover(function () { $(this).find(".tc-xunjia").show(); }, function () { $(this).find(".tc-xunjia").hide(); })

								if ($("#hidCarType").val() == "停销") {
									var lianJieArray = [];
									lianJieArray.push("<span class=\"button_orange btn-xj-w\">");
									lianJieArray.push("<a target=\"_blank\" href=\"http://yiche.taoche.com/buycar/b-<%= cbe.Serial.AllSpell %>/?page=1&leads_source=p003005&carid=<%= carID %>\">买二手车</a>");
                        			lianJieArray.push("</span>");
                        			lianJieArray.push("<span class=\"button_gray btn-qt-w btn-bq-w\">");
                        			lianJieArray.push("<a target=\"_blank\" href=\"http://www.taoche.com/pinggu/\">二手车估价</a>");
                        			lianJieArray.push("</span>");
                        			$("#lianJie").html(lianJieArray.join(""));
								} else {
									if ($("#hidCarType").val() == "包销") {
										var lianJieArray1 = [];
										lianJieArray1.push("<span class=\"button_orange btn-xj-w btn-bq-w\">");
										lianJieArray1.push("<a target=\"_blank\" href=\"http://www.yichemall.com/car/detail/c_<%=carID%>/?source=yc-cxzs-onsale-1&leads_source=p003011\">直销特卖</a>");
										lianJieArray1.push("</span>");
										lianJieArray1.push("<span class=\"button_gray btn-qt-w btn-ct-w\">");
										lianJieArray1.push("<a target=\"_blank\" href=\"http://chedai.bitauto.com/<%=cbe.Serial.AllSpell %>/m<%= carID %>/chedai/?from=&leads_source=p003003\">贷款</a>");
										lianJieArray1.push("</span>");
										$("#lianJie").html(lianJieArray1.join(""));
									} 
									else if("<%=cbe.Serial.SaleState %>" == "待销"){   //车系的销售状态cssalestate=“待销”时，车款为“未上市”状态;未上市不显示按钮
										return;
									}
									else {
										var lianJieArray2 = [];
										lianJieArray2.push("<span class=\"button_orange btn-xj-w\">");
										lianJieArray2.push("<a target=\"_blank\" href=\"http://dealer.bitauto.com/zuidijia/nb<%=cbe.SerialId%>/nc<%=cbe.Id%>/?T=2&leads_source=p003001\">询底价</a>");
										lianJieArray2.push("</span>");
										lianJieArray2.push("<span class=\"button_gray btn-qt-w\">");
										lianJieArray2.push("<a target=\"_blank\" href=\"http://www.huimaiche.com/<%=cbe.Serial.AllSpell%>?carid=<%=carID%>&tracker_u=609_ckzs&leads_source=p003002\">买新车</a>");
                                    	lianJieArray2.push("</span>");
                                    	lianJieArray2.push("<span class=\"button_gray btn-qt-w\">");
                                    	lianJieArray2.push("<a target=\"_blank\" href=\"http://chedai.bitauto.com/<%=cbe.Serial.AllSpell%>/m<%=carID%>/chedai/?from=yc18&leads_source=p003003\">贷款</a>");
                                        lianJieArray2.push("</span>");
                                        lianJieArray2.push("<span class=\"button_gray btn-qt-w\">");
                                        lianJieArray2.push("<a target=\"_blank\" href=\"http://zhihuan.taoche.com/?leads_source=p003004&ref=chekuanzshuan&serial=<%=cbe.SerialId%>\">置换</a>");
                                        lianJieArray2.push("</span>");
                                        lianJieArray2.push("<span class=\"button_gray btn-qt-w\">");
                                        lianJieArray2.push("<a target=\"_blank\" href=\"http://www.taoche.com/<%=cbe.Serial.AllSpell%>/?leads_source=p003005\">二手车</a>");
                                        lianJieArray2.push("</span>");
                                        $("#lianJie").html(lianJieArray2.join(""));
									}
							}
							}
						});
					})();
				</script>--%> 
				<div class="summaryMiddleAD">
					<ins id="middleADForCar" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= cbe.Serial.Id %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="37940534-3acb-4358-8f99-ac9abc6624ca"></ins>
				</div>
				<script type="text/javascript">
				    $(".jshui").hover(function () { $(this).find(".tc-xunjia").show(); }, function () { $(this).find(".tc-xunjia").hide(); });
					(function() {
						$("#duibiAll").click(function() {
							var paraArray = [];
							var $trs = $("#duibiTable tr");
							$("#duibiTable tr").find("a[cid]").each(function(){
								paraArray.push($(this).attr("cid"));
							});
							if (paraArray.length > 0) {
								WaitCompareObj.AddMultiCarToCompare(paraArray.join(","));
							}
						});
					})()
				</script>
				<!-- SEO底部热门 -->
				<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
				<!-- SEO底部热门 -->
			</div>
			<!--左边内容 end-->
			<!--右边内容 start-->
			<div class="col-side">
				<!-- ad -->
				<div class="col-side_ad">
					<ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= cbe.Serial.Id %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e"></ins>
				</div>
				<div class="col-side_ad">
					<ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= cbe.Serial.Id %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891"></ins>
				</div>
				<div class="col-side_ad">
					<ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= cbe.Serial.Id %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
				</div>
				<!-- 热门车型 -->
				<div class="line-box">
					<%= hotCars %>
				</div>
				<!-- 子品牌还关注 -->
				<div class="line-box">
					<div class="side_title">
						<h4>看过此车的人还看</h4>
					</div>
					<%= SerialToSerialHtml %>
					<div class="clear">
					</div>
				</div>
				<!--此品牌下其别子品牌-->
				<div class="line-box">
					<%=GetBrandOtherSerial() %>
					<div class="clear">
					</div>
				</div>
				<!-- 在线答疑 -->
				<%--<%= carAsk %>--%>
				<!--二手车-->
				<%--<%=UCarHtml %>--%>
				<%if (cbe.SaleState.Trim() != "停销")
	  { %>
				<div class="line-box display_n" id="line_boxforucar_box" style="display: none;">
					<div class="side_title">
						<h4><a target="_blank" href=" http://www.taoche.com/<%=cbe.Serial.AllSpell %>/">
							<%= cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>二手车
						</a></h4>
					</div>
					<div class="theme_list play_ol">
						<ul class="secondary_list" id="ucar_serialcity">
						</ul>
					</div>
				</div>
				<%}
				%>

				<!--# include file="~/html/cheyimai_chexing.shtml"-->
				<%--<div class="line_box ucar_box">
				</div>--%>
				<div class="col-side_ad" style="margin-bottom: 10px; overflow: hidden; margin-top: 30px">
					<%--<script type="text/javascript" id="zp_script_98" src="http://mcc.chinauma.net/static/scripts/p.js?id=98&w=220&h=220&sl=1&delay=5"
						zp_type="1"></script>--%>
					<script type="text/javascript" id="zp_script_246" src="http://mcc.chinauma.net/static/scripts/p.js?id=246&w=240&h=220&sl=1&delay=5"
						zp_type="1"></script>
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
				<div class="clear">
				</div>
			</div>
			<!--右边内容 end-->
		</div>
		<!-- 调用尾 -->
		<script type="text/javascript">
			var CarCommonCSID = '<%=cbe.Serial.Id %>';
			var CarCommonCarID = '<%= carID.ToString() %>';
		</script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
		<script type="text/javascript">
			OldPVStatistic.ID1 = "<%=cbe.Serial.Id %>";
			OldPVStatistic.ID2 = "<%=carID.ToString() %>";
			OldPVStatistic.Type = 0;
			mainOldPVStatisticFunction();
		</script>
		<!-- baa 浏览过的车型-->
		<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
		<script type="text/javascript">
			try {
				Bitauto.UserCars.addViewedCars('<%=cbe.Serial.Id %>');
			}
			catch (err)
			{ }
		</script>
	</div>
	<!--#include file="~/html/footer2014.shtml"-->
	<%if (carID == 101688 || carID == 103759)
   { %>
	<!--百度热力图-->
	<div style="display: none">
		<script type="text/javascript">
			var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
			document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
		</script>
	</div>
	<%} %>
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=cbe.Serial.Id%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
	<!--内容 end-->
	<!-- 对比浮动框 -->
	<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none;" animateright="-533" animatebottom="180" data-page="compare">
		<div class="tt" id="bar_minicompare" style="cursor: move;">
			<h6>车型对比</h6>
			<a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
		</div>
		<div class="content">
			<ul id="idListULForWaitCompare" class="fixed-list"></ul>
			<div class="fixed-box">
				<div class="fixed-input" id="CarSelectSimpleContainerParent">
					<input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly" />
					<%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
					<div class="right" userful="showcartypesim">
						<div class="star">
							<i class="star-i"></i>
						</div>
					</div>
					<div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
				</div>
				<div class="clear"></div>
				<div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
			</div>
			<div class="wamp">
				<em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
				<div class="clear"></div>
			</div>
			<div class="alert-center" id="AlertCenterDiv" style="display: none;">
				<p>最多对比10个车款</p>
			</div>
		</div>
	</div>
	<!--漂浮层模板start-->
	<div class="effect" style="display: none;">
		<div class="car-summary-btn-duibi button_gray"><a href="###" target="_self"><span>对比</span></a></div>
	</div>
	<!--漂浮层模板end-->
</body>
</html>

<div id="fade" style="background-color: #000000; filter: Alpha(Opacity=30); left: 0px; opacity: 0.3; position: absolute; text-align: center; top: 0px; vertical-align: middle; z-index: 10010;"></div>
<!--弹出层-->
<div class="tc-popup-box" id="tc-popup-box" style="display: none">
	<div class="tt">
		<h6>填写我想买的车</h6>
		<a href="javascript:void(0);" class="btn-close" onclick="javascript:$('#tc-popup-box,#fade').hide();">关闭</a>
	</div>
	<div class="tc-popup-con like-car">
		<h5>想买什么车，易车帮您找低价</h5>
		<p class="p-msg">提交您想买的车型后，可在“我的易车-计划购买”中查看优惠信息。</p>
		<ul>
			<li>
				<div class="f-name"><em>*</em> 品牌：</div>
				<div class="f-item">
					<select id="master3" class="w-198">
						<option>请选择品牌</option>
					</select>
					<span class="alert" style="display: none">请输入品牌</span>
				</div>
			</li>
			<li>
				<div class="f-name"><em>*</em> 车型：</div>
				<div class="f-item">
					<select id="serial3" class="w-198">
						<option>请选择车型</option>
					</select>
					<span class="alert" style="display: none">请选择车型</span>
				</div>
			</li>
			<li>
				<div class="f-name">车款：</div>
				<div class="f-item">
					<select id="cartype3" class="w-198">
						<option>请选择车款</option>
					</select>
				</div>
			</li>
			<li>
				<div class="f-name"><em>*</em> 购买时间：</div>
				<div class="f-item">
					<select id="gMSJ" class="w-198">
						<option value="-1">请选择购买时间</option>
						<option value="1">1个月内</option>
						<option value="3">3个月内</option>
						<option value="6">6个月内</option>
						<option value="max">6个月以上</option>
					</select>
					<span class="alert" style="display: none">请选择购买时间</span>
				</div>
			</li>
			<li>
				<div class="f-name"><em>*</em> 购买地：</div>
				<div class="f-item">
					<select id="proSel" class="w-94">
						<option value="-1">请选择省份</option>
					</select>
					<select id="citySel" class="w-94">
						<option value="-1">请选择城市</option>
					</select>
					<span class="alert" style="display: none">请选择购买地</span>
				</div>
			</li>
			<li>
				<div class="f-name">手机：</div>
				<div class="f-item">
					<span id="usermobile" class="set-n"></span>
					<a href="javascript:void(0);" class="set-link" id="changemobile">修改手机</a>
					<a id="shuaXin" class="set-link" href="javascript:void(0)">刷新</a>
				</div>
			</li>
		</ul>
		<div class="button_orange button_99_35"><a id="btnSubmit" href="javascript:void(0)">提交</a></div>
	</div>
</div>
<div class="tc-popup-box" id="submitsucceed" style="display: none">
	<div class="rest-box">
		<div class="tt">
			<h6>填写我想买的车</h6>
			<a href="javascript:void(0);" class="btn-close" onclick="javascript:$('#submitsucceed,#fade').hide();">关闭</a>
		</div>
		<div class="tc-popup-con">
			<h2>提交成功，已为您找到 <strong id="carname"></strong>优惠信息。</h2>
			<dl class="tehui_car">
				<dt>
					<img src="" id="imginfo">
				</dt>
				<dd>
					<p><strong>参考优惠价：</strong><em id="minmrice"></em><em class="fz14">万起</em></p>
					<p><strong>厂商指导价：</strong><del id="carprice"></del></p>
					<span class="button_orange w118_37"><a href="javascript:void(0);" id="todetial">查看详情</a></span>
				</dd>
			</dl>
			<div class="c_txt">您可在“<a id="myattsucc" href="javascript:void(0);">我的易车-计划购买</a>”中查看优惠信息。</div>
		</div>
	</div>
</div>
<div class="tc-popup-box" id="wuyouhui" style="display: none">
	<div class="rest-box">
		<div class="tt">
			<h6>填写我想买的车</h6>
			<a href="javascript:void(0);" class="btn-close" onclick="javascript:$('#wuyouhui,#fade').hide();">关闭</a>
		</div>
		<div class="tc-popup-con">
			<h2>提交成功！这辆车居然没有优惠，这就派人去帮您谈个优惠来。</h2>
			<p class="color9">您可在“<a id="iAttendtion" href="javascript:void(0);">我的易车-计划购买</a>”中查看优惠信息。</p>
			<div class="button_gray button_99_35"><a href="javascript:void(0);" onclick="javascript:$('#wuyouhui,#fade').hide();">我知道了</a></div>
		</div>
	</div>
</div>
<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"> </script>
<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_iframedialog,ibt_useractivatordialog"> </script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlist.js?v=201310"> </script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/gouche/pc/cityjson.js?v=2015082517"></script>
<script type="text/javascript">
	var carIdList = [];
	var isActive = true;
	var userId = 0;
	var isLogined;
	var userName = "";
	var allSpellSel;//下拉选择的城市 弹出层的详情页跳转使用

	String.prototype.trim = function() {  
		return this.replace(/(^\s*)|(\s*$)/g, "");  
	}  

    
	Bitauto.Login.onComplatedHandlers.push(function (loginResult) {
		var serialIdStr = [];
		isLogined = loginResult.isLogined;
		//已经登录，直接在回调loginResult里获取浏览过的车
		if (isLogined) {
			//服务端获取到的当前登录用户浏览过的车
			var objList = loginResult.viewedcars;
			$.each(objList, function (i, item) {
				serialIdStr.push(item.CarSerialId);
			});
			userId = loginResult.userId;
			userName = loginResult.userName;
		}
	});

	$(function() {
		//判断是否显示“加载更多配置”
		var tableCount=$(".car_config").find("table").length;
		var tableConfig=$(".car_config").find("table").eq(1).html();
		var hasTableConfig=false;
		if(tableConfig&&tableConfig!=null)
		{
			hasTableConfig=tableConfig.trim().length>0?true:false;
		}
        
		//如果没有详细配置参数，则不显示“参数配置”块
		if(!hasTableConfig)
		{
			$("#DicCarParameter").hide();
		}

		if(tableCount>2&&hasTableConfig)
		{   $(".addd_more_link").show(); }

		//用户激活成功后隐藏弹出层
		Bitauto.UserActivatorDialog.bind('success', function () {
			Bitauto.UserActivatorDialog.close();
			//请求接口 获得手机号
			$.ajax({
				dataType: "jsonp",
				jsonpCallback: "userMobile",
				data: { userId: userId },
				url: "http://api.gouche.yiche.com/Remind/GetUserInfo",
				success: function (data) {
					if (data[userId].mobilebound == 1) {
						$("#usermobile").html(data[userId].mobile);
						$("#tc-popup-box").showCenter();
					}
				}
			});
		});
        
		$("#planBtn").click(function () {
			//主品牌to子品牌
			BitA.DropDownList({
				container: { master: "master3", serial: "serial3", cartype: "cartype3" },
				include: { serial: "1", cartype: "1" },
				dvalue: { master: <%=cbe.Serial.Brand.MasterBrandId%>, serial: <%=cbe.SerialId%>, cartype: $("#hidCarID").val() }
			});
			BindCity();
			Bitauto.Login.afterLoginDo(function () {
				GetUserInfo(userId);
			});
		});
        
		$("#proSel").bind("change", function () {
			$("#citySel option:gt(0)").remove();
			var id = $(this).val();
			for (var provinceId in cityJson) {
				if (id == provinceId) {
					var cityData = cityJson[provinceId].Child;
					for (var cityId = 0; cityId < cityData.length; cityId++) {
						$("#citySel").append("<option value=\"" + cityData[cityId].CityId + "\">" + cityData[cityId].CityName + "</option>");
					}
				}
			}
		});

		$("#changemobile").bind("click", function () {
			$("#tc-popup-box").hide();
			$("#fade").hide();
			Bitauto.UserActivatorDialog.show();
		});
        
		$("#shuaXin").bind("click", function () {
			GetUserInfo(userId);
		});

		$("#btnSubmit").bind("click", function() {
			if (!Validate()) {
				return;
			}
			var serialVal = $('#serial3').val();
			var carVal = $('#cartype3').val();
			var provinceVal = $("#proSel").val();
			var cityVal = $("#citySel").val();
			var childData = cityJson[provinceVal].Child;
			for (var index = 0; index < childData.length; index++) {
				if (childData[index].CityId == cityVal) {
					allSpellSel = childData[index].AllSpell;
					break;
				}
			}

			$.ajax({
				dataType: "jsonp",
				jsonpCallback: "att",
				data: { username: userName, serialId: serialVal, buyDate: $("#gMSJ").val(), cityId: cityVal, carId: carVal },
				url: "http://api.gouche.yiche.com/Remind/AddUserPlanCar",
				success: function(data) {
					if (data.Succeed) {
						$("#tc-popup-box").hide();
						showres(serialVal);
					}
				}
			});
		});

		$("#myattsucc").click(function () {
			$("#fade").hide();
			$("#submitsucceed").hide();
			window.open("http://i.yiche.com/u" + userId + "/Car/plan/", "", "");
		});
        
		$("#todetial").bind("click", function () {
			$("#submitsucceed").hide();
			$("#fade").hide();
			var tourl = "http://gouche.yiche.com/" + allSpellSel + "/sb" + $('#serial3').val();
			window.open(tourl, "", "", "");
		});
        
		$("#iAttendtion").bind("click", function() {
			iKnow(this);
			window.open("http://i.yiche.com/u" + userId + "/Car/plan/", "", "");
		});

        
       
		//绑定“加载更多配置”事件
		$(".addd_more_link").bind("click",function(){
			$(".car_config").find("table:last").show();
			$(this).hide();
		});
	});
    
	function Validate() {
		var masterVal = $('#master3').val();
		var serialVal = $('#serial3').val();
		if (masterVal == 0) {
			$('#master3').next().show();
			return false;
		} else {
			$('#master3').next().hide();
		}
		if (serialVal == 0) {
			$('#serial3').next().show();
			return false;
		} else {
			$('#serial3').next().hide();
		}
		var gMSJ = $("#gMSJ").val();
		if (gMSJ == -1) {
			$("#gMSJ").next().show();
			return false;
		} else {
			$("#gMSJ").next().hide();
		}
		var cityVal = $("#citySel").val();
		if (cityVal == -1) {
			$("#citySel").next().show();
			return false;
		} else {
			$("#citySel").next().hide();
		}
		return true;
	}

	function BindCity() {
		var cityId = bit_locationInfo.cityId;
		$("#proSel option:gt(0)").remove();
		$("#citySel option:gt(0)").remove();
		if (cityJson) {
			//绑定省
			for (var i in cityJson) {
				$("#proSel").append("<option value=\"" + i + "\">" + cityJson[i].CityFullName + "</option>");
			}
			for (var provinceId in cityJson) {
				var cityData = cityJson[provinceId].Child;
				for (var cityIdValue = 0; cityIdValue < cityData.length; cityIdValue++) {
					if (cityData[cityIdValue].CityId == cityId) {
						$("#proSel option[value=" + provinceId + "]").attr("selected", true);
						//绑定市
						for (var p = 0; p < cityData.length; p++) {
							$("#citySel").append("<option value=\"" + cityData[p].CityId + "\">" + cityData[p].CityName + "</option>");
						}
						$("#citySel option[value=" + cityData[cityIdValue].CityId + "]").attr("selected", true);
						return;
					}
				}
			}
		}
	}

	function GetUserInfo(userId) {
		if (userId > 0) {
			//用户信息
			$.ajax({
				dataType: "jsonp",
				jsonpCallback: "user",
				data: { userId: userId },
				url: "http://api.gouche.yiche.com/Remind/GetUserInfo",
				success: function (data) {
					if (data[userId].mobilebound == 1) {
						carIdList = [];
						carIdList = data[userId].plancar;
						$("#usermobile").html(data[userId].mobile);
						$("#tc-popup-box").showCenter();
					} else {
						isActive = false;
						$("#tc-popup-box").hide();
						$("#fade").hide();
						Bitauto.UserActivatorDialog.show();
					}
				}
			});
		}
	}

	///显示关注结果
	function showres(serialid) {
		var para, url;
		var citySelVal = $("#citySel").val();
		if ($("#cartype3").val() == "0") {
			para = { cityid: citySelVal, serialid: serialid };
			url = "http://api.gouche.yiche.com/PreferentialCar";
		} else {
			para = { cityid: citySelVal, carid: $("#cartype3").val() };
			url = "http://api.gouche.yiche.com/PreferentialCar/GetCarByCarId";
		}
		$.ajax({
			dataType: "jsonp",
			jsonpCallback: "youhui",
			data: para,
			url: url,
			success: function (data) {
				if (data && data.SerialId) {
					$("#carname").html(data.SerialName + "-" + jieMa(data.CarName));
					$("#imginfo").attr("src", data.ImageUrl);
					$("#carprice").html(data.MinCarReferPrice + "万起");
					$("#minmrice").html(data.MinPrice);
					$("#submitsucceed").showCenter();
                
				} else {
					$("#wuyouhui").showCenter();
				}
			}
		});
	}
    
	function jieMa(str) {
		str = str.replace(/\\/g, "%");
		return unescape(str);
	}
    
</script>

<script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115" type="text/javascript"></script>
<script type="text/javascript">
	(function(){
		var timer;
		$("#car-pop,#car-popbox").hover(function(){clearTimeout(timer);$("#car-popbox").show();},function(){timer = setTimeout(function () { $("#car-popbox").hide(); }, 500);});
		$("#color-sty em").hover(function(){$(this).addClass("current").find(".tc.tc-color-box").show();},function(){$(this).removeClass("current").find(".tc.tc-color-box").hide();});
		var cityId = 201,saleState="<%=cbe.SaleState.Trim()%>";
		if(typeof (bit_locationInfo) != 'undefined'){
			cityId = bit_locationInfo.cityId;
		}
		if(typeof(showUCar)!="undefined"){ 
			if(saleState!="停销")
				showUCar(<%=cbe.Serial.Id %>, cityId,'<%=cbe.Serial.AllSpell %>','<%=cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>',getUCarForSider,undefined,undefined,undefined,'chekuan');
			else
				showUCar(<%=cbe.Serial.Id %>, cityId,'<%=cbe.Serial.AllSpell %>','<%=cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>',getUCarForBottom,undefined,undefined,undefined,'chekuan'); 
		} 
	})();
	//二手车数据
	function getUCarForBottom(data, csId, csSpell, csShowName) {
		try {
			data = data.CarListInfo;
			if (data.length <= 0) return;
			var strHtml = [];
			strHtml.push("<div class=\"title-con\">");
			strHtml.push("<div class=\"title-box title-box2\">");
			strHtml.push("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/\">相关二手车</a></h4>");
			strHtml.push("</div>");
			strHtml.push("</div>");
			strHtml.push("<div class=\"cxdb-box taoche-box\">");
			strHtml.push("<ul>");
			$.each(data, function (i, n) {
				if (i > 3) return;
				var className = "";
				if (i == 0) className = "first";
				if (i == 3) className = "last";
				strHtml.push("<li class=\"" + className + "\"><div class=\"img-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\" class=\"pic\"><img width=\"150\" height=\"100\" src=\"" + n.PictureUrl + "\"></a></div>");
				strHtml.push("<div class=\"txt-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\">" + n.BrandName + "</a></div>");
				strHtml.push("<strong>" + n.DisplayPrice + "</strong>");
				strHtml.push("<span>" + n.BuyCarDate.substring(2) + "上牌 " + n.DrivingMileage + "公里</span></li>");
			});
			strHtml.push("</ul>");
			strHtml.push("</div>");
			strHtml.push("<div class=\"clear\"></div>");

			$("#ucarlist").html(strHtml.join(''));
		} catch (e) { }
	}
	function getUCarForSider(data, csId, csSpell, csShowName) {
		try {
			data = data.CarListInfo;
			if (data.length <= 0) return;
			var strHtml = [];
			$.each(data, function (i, n) {
				if (i > 6) return;
				strHtml.push("<li><a class=\"img\" title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "&leads_source=p003010\">");
				strHtml.push("<img src=\"" + n.PictureUrl + "\"></a><p class=\"tit\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "&leads_source=p003010\">" + n.BrandName + "</a></p>");
				strHtml.push("<p class=\"hui\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</p>");
				strHtml.push("<p class=\"red\">" + n.DisplayPrice + "</p>");
				strHtml.push("</li>");
			});
			$("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
		} catch (e) { }
	}
	(function () {
		$("#more-color").toggle(function () { $(this).addClass("more-current"); $("#more-color-sty").show(); }, function () { $(this).removeClass("more-current"); $("#more-color-sty").hide(); });
		//纠错
		var strError = "车款名称：<%=carFullName %>\n问题描述：\n联系方式(电话/QQ)：";
		$("a[name='correcterror']").click(function () { $("#popup-box").showCenter(); $("#correctError").val(strError); });
		$("#popup-box .btn-close,#btnErrorCancel").click(function() {
			$("#fade").hide();
			$("#popup-box").hide();
		});
		$("#popup-box-success .btn-close,#btn-success-close").click(function() {
			$("#fade").hide();
			$("#popup-box-success").hide();
		});
		$("#popup-box a[name='btnCorrectError']").click(function (e) {
			e.preventDefault();
			var content = $("#correctError").val();
			if (content == "" || content == strError) {
				$("#popup-box .alert span").html("请输入提交内容。");
				return;
			} $("#popup-box .alert span").html("");
			var url = "http://www.bitauto.com/FeedBack/api/CommentNo.ashx?txtContent=" + encodeURIComponent(content) + "&satisfy=1&ProductId=5&categorytype=2&csid=" + (typeof (CarCommonCSID) != "undefined" ? CarCommonCSID : 0);
			$.ajax({ url: url, dataType: 'jsonp', jsonp: "XSS_HTTP_REQUEST_CALLBACK", jsonpCallback: "errorCallback", success: function (data) {
				if (data.status == "success") {
					$("#popup-box").hide();
					$("#popup-box-success").showCenter().find(".tit").html("提交成功！").siblings("p").html("侦察兵，好样的！");
				} else {
					$("#popup-box-success").showCenter().find(".tit").html("提交失败！").siblings("p").html(data.message);
				}
			}
			});
		});
	})();
	//元素居中
	!function ($) {
		$.fn.showCenter = function() {
			$("#fade").show().css({ width: $(document).width(), height: $(document).height() });
			var top = ($(window).height() - this.height()) / 2;
			var left = ($(window).width() - this.width()) / 2;
			var scrollTop = $(document).scrollTop();
			var scrollLeft = $(document).scrollLeft();

			if (!-[1,] && !window.XMLHttpRequest) {
				return this.css({ position: 'absolute', 'z-index': 10020, 'top': top + scrollTop, left: left + scrollLeft }).show();
			} else {
				return this.css({ position: 'fixed', 'z-index': 10020, 'top': "150px", left: left }).show();
			}
		};
	} (jQuery);
</script>
<script type="text/javascript">
	function ImageMouseoverHandler() {
		var bigImg = document.getElementById(this.id.replace("Small", "Big"));
		var curSmallImg = document.getElementById(curImg.id.replace("Big", "Small"));
		var current = document.getElementById(this.id.replace("Small", "Current"));
		curImg.style.display = "none";
		bigImg.style.display = "block";
		//curSmallImg.className = "";
		//this.className = "current";
		curImg = bigImg;
	}
	var curImg = document.getElementById("focusBigImg_0");
	for (j = 0; j < 3; j++) {
		var imgId = "focusSmallImg_" + j;
		var img = document.getElementById(imgId);
		if (img)
			img.onmouseover = ImageMouseoverHandler;
		else
			break;
	}
	$(".img-list > div").hover(function () { $(this).find("a>span").show(); $(this).siblings().find("a>span").hide(); }, function () { })
</script>
<script type="text/javascript">
	if (typeof (bitLoadScript) == "undefined"){
		bitLoadScript = function (url, callback, charset) {
			var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
			if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
			else { s.onload = function () { if (callback) callback(); }; }
			s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
		};
	}
</script>
<script type="text/javascript">   
	bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160715",function(){
		WaitCompareObj.MasterBrandId = '<%= cbe.Serial.Brand.MasterBrandId %>';
		WaitCompareObj.MasterBrandName = '<%= cbe.Serial.Brand.MasterBrand.Name %>';
		WaitCompareObj.SerialBrandId = '<%= cbe.Serial.Id %>';
		WaitCompareObj.SerialBrandName = '<%= cbe.Serial.Name%>';
        
		if ($("#isTingShou").val() == "停销"){
			//console.log($("#isTingShou").val());
			WaitCompareObj.IsCarTypeStopSale = 0;
		}
		CarCompareObj.Init("button_gray db-btn","button_none db-btn");
		WaitCompareObj.Init();
	});
</script>
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
<%--<script type="text/javascript" src="/jsnew/commons.js"></script>
<script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150805"></script>
<script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>
<script type="text/javascript">
    $(function(){
        WaitCompareObj.MasterBrandId = '<%= cbe.Serial.Brand.MasterBrandId %>';
        WaitCompareObj.MasterBrandName = '<%= cbe.Serial.Brand.MasterBrand.Name %>';
        WaitCompareObj.SerialBrandId = '<%= cbe.Serial.Id %>';
        WaitCompareObj.SerialBrandName = '<%= cbe.Serial.Name%>';
        
        if ($("#isTingShou").val() == "停销"){
            //console.log($("#isTingShou").val());
            WaitCompareObj.IsCarTypeStopSale = 0;
        }
        CarCompareObj.Init("button_gray db-btn","button_none db-btn");
        WaitCompareObj.Init();
    });
</script>--%>
<!-- 经销商代码 -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
