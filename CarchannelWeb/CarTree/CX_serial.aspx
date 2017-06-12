<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTree.CX_serial" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=_SerialSeoName %>】<%=_BrandName%><%=_SerialName%>_<%=_SerialSeoName %>报价_导购_评测_行情-易车网
	</title>
	<meta name="Keywords" content="<%=_SerialSeoName %>,<%=_SerialSeoName %>报价,<%=_MetaKeyWordArea %>,易车网" />
	<meta name="Description" content="<%=_SerialSeoName %>:易车网车型频道为您提供<%=_SerialSeoName %>报价/价格,<%=_SerialSeoName %>图片,<%=_SerialSeoName %>参数配置,经销商,视频,导购,评测,图解,行情,更多<%=_SerialSeoName %>汽车信息尽在易车网" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car-685.shtml" -->
	<script type="text/javascript">
		var _SearchUrl = '<%=this._SearchUrl %>';
	</script>
	<style type="text/css">
		.onsale { overflow: hidden; }
		.c0624_06 label { font-weight: normal; color: #999999; }
		.comparetable { float: left; margin: 0 0 -1px -1px; overflow: hidden; width: 718px; }
		.compare2 { margin: -1px 0 0 1px; width: 718px; overflow: hidden; }
			.compare2 td { border-left: none; border-right: none; }
			.compare2 th { background: #F5F7F9; border: none; color: #999999; font-weight: normal; height: 27px; text-align: center; }
				.compare2 th.firstItem { color: #333333; padding-left: 10px; text-align: left; }
		.tc-popup-box { position: fixed; margin: -131px 0 0 -249px; z-index: 99999; left: 50%; top: 50%; }
	</style>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--顶通-->
	<!--#include file="~/html/tree_header2014.shtml"-->
	<!--#include file="~/include/pd/2014/common/00001/201402_shuxing_nav_chexing_Manual.shtml" -->
	<div class="tree_wrap_box">
		<!--左侧树形-->
		<div id="leftTreeBox" class="treeBoxv1">
		</div>
		<!--右侧内容-->
		<div class="treeMainv1">
			<%=NavPathHtml %>
			<!-- 子品牌介绍 -->
			<%-- <div class="line_box c0624_01">
            <%=serialDescribeHtml%> 
           
            <div class="clear">
            </div>
        </div>--%>
			<div class="line-box">
				<div class="title-con">
					<div class="title-box">
						<h3>
							<a href="/<%=_SerialSpell %>/" target="_blank">
								<%=_SerialName %></a></h3>
						<div class="more">
							<a href="/<%=_SerialSpell %>/peizhi/" target="_blank">参数</a> | <a href="/<%=_SerialSpell %>/tupian/"
								target="_blank">图片</a> | <a href="/<%=_SerialSpell %>/baojia/" target="_blank">报价</a>
							| <a href="/<%=_SerialSpell %>/jiangjia/" target="_blank">降价</a> | <a href="/<%=_SerialSpell %>/koubei/"
								target="_blank">口碑</a> | <a href="/<%=_SerialSpell %>/baoyang/" target="_blank">养护</a>
							| <a href="<%=baaUrl %>" target="_blank">论坛</a> | <a href="http://www.taoche.com/<%=_SerialSpell %>/" target="_blank">二手车</a>
						</div>
					</div>
				</div>
				<div class="card-head-box ck-card-h clearfix">
					<div class="img-box">
						<a href="/<%=_SerialSpell %>/" target="_blank">
							<img src="<%=serialImageUrl %>" alt="" /></a>
					</div>
					<div class="txt-box zh-card">
						<p class="p-tit">
							<%if (!(sic.CsPriceRange == "未上市" || sic.CsPriceRange == "停售" || string.IsNullOrEmpty(sic.CsPriceRange) || sic.CsPriceRange == "暂无报价"))
		 { %>
							全国参考价：<strong><a href="http://price.bitauto.com/brand.aspx?newbrandId=<%=_SerialId %>"
								target="_blank"><%=  sic.CsPriceRange%></a></strong><%}
		 else
		 { %>
							<strong>
								<%=string.IsNullOrEmpty(sic.CsPriceRange) ? "暂无报价": sic.CsPriceRange %></strong>
							<%} %>
							<%if (sic.CsPriceRange != "停售")
		 { %>
							<a id="btnDirectSell" style="display: none;" href="#" target="_blank" class="ico-shangchengtehui">直销</a><%} %>
							<a id="serial_guanzhu" href="javascript:;" title="点击关注"><span class="no-sc"></span></a>
						</p>
						<ul>
							<li>
                                <i><%= sic.CsPriceRange == "未上市"?"预售价":"厂商指导价"%>:</i>
                                <span><%=serialEntity.ReferPrice%></span> 
							</li>
							<li class="current"><i class="i-w">二手车：</i><span><a href="http://www.taoche.com/buycar/serial/<%=_SerialSpell %>/"
								target="_blank"><%=serialUCarPrice %></a></span> </li>
							<%if (isElectrombile)
		 {%>
							<li><i>充电时间：</i><span><%=chargeTimeRange%></span> </li>
							<li class="current"><i class="i-w">快充时间：</i><span><%=fastChargeTimeRange%></span>
							</li>
							<li><i>续航里程：</i><span><%=mileageRange%></span> </li>
							<%}
		 else
		 { %>
							<li><i>排&nbsp;&nbsp;&nbsp;&nbsp;量：</i><span title="<%=serialSaleDisplacementalt %>"><%=serialSaleDisplacement%></span>
							</li>
							<li class="current"><i class="i-w">油&nbsp;&nbsp;&nbsp;&nbsp;耗：</i><span><a href="/<%=_SerialSpell %>/youhao/"
								target="_blank"><%=string.IsNullOrEmpty(sic.CsSummaryFuelCost) ? "暂无" : sic.CsSummaryFuelCost%></a></span>
							</li>
							<li><i>变速箱：</i><span><%=serialTransmission%></span> </li>
							<%} %>
							<li class="current"><i class="i-w">保&nbsp;&nbsp;&nbsp;&nbsp;修：</i><span><%=string.IsNullOrEmpty(sic.SerialRepairPolicy) ? "暂无" : sic.SerialRepairPolicy%></span>
							</li>
						</ul>
						<div class="sc-btn-box">
							<span class="button_orange btn-xj-w"><a id="car_zixun" href="http://dealer.bitauto.com/zuidijia/nb<%=_SerialId %>/?T=1&leads_source=p015001"
								target="_blank" class="" data-channelid="2.22.108">询底价</a> </span><span id="divDemandCsBut" class="button_gray btn-qt-w"
									style="display: none;"><a class="" href="#" data-channelid="">特卖</a> </span><%=shijiaOrHuimaiche%><span class="button_gray btn-qt-w">
										<a id="car_chedai" href="http://www.daikuan.com/www/<%=_SerialSpell %>/?from=yc9&leads_source=p015003"
											class="" target="_blank" data-channelid="2.22.110">贷款</a> </span><span class="button_gray btn-qt-w" id="btnZhihuan"><a
												href="http://maiche.taoche.com/zhihuan/?serial=<%=_SerialId %>&leads_source=p015004&ref=chexisxhuan"
												target="_blank" data-channelid="2.22.111">置换</a>  </span>
							<span class="button_gray btn-qt-w"><a
								href="http://www.taoche.com/<%=_SerialSpell %>/?ref=chexizsmai&leads_source=p015005"
								target="_blank" data-channelid="2.22.112">二手车</a>  </span>

						</div>
					</div>
				</div>
				<div class="clear">
				</div>
			</div>
			<%=carListTableHtml%>
			<script type="text/javascript">
				//添加统计链接末尾参数
				var tongJiEndUrlParam='&ref=tree1&rfpa_tracker=1_9';
				var params = {};
				params.tagtype = "chexing";
				params.pagetype = "serial";
				params.objid = <%=_SerialId %>;
			</script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/lefttreenew.js?v=2015121715"></script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
			<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialexternalcommon.min.js?v=20160929"></script>
			<%--<script type="text/javascript" src="/jsnew/serialexternalcommon.js?v=20151020"></script>--%>
			<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
			<script type="text/javascript">
				//直销
				getDirectSell(<%=_SerialId %>,'<%=_SerialSpell %>',bit_locationInfo.cityId,'yc-car-tree-1');
				getDemandAndJiangJia(<%=_SerialId %>,'<%=_SerialSpell %>',bit_locationInfo.cityId);
				//登录 车型关注
				function initLoginFavCar() {
					try {
						var carLoginresult = Bitauto.Login.result;
						if (typeof carLoginresult != 'undefined' && typeof carLoginresult.plancar != 'undefined' && carLoginresult.plancar.length > 0) {
							for (var i = 0; i < carLoginresult.plancar.length; i++) {
								if (carLoginresult.plancar[i].CarSerialId == CarCommonCSID) { $("#serial_guanzhu > span").removeClass("no-sc").attr("title", "管理关注的车"); break; }
							}
						}
					} catch (e) { }
				}
				//添加 取消关注车型
				function FocusCar(obj) {
					Bitauto.Login.afterLoginDo(function () {
						var id = CarCommonCSID;
						obj.find("span").attr('class') == "no-sc" ? Bitauto.UserCars.addConcernedCar(id, function () {
							if (Bitauto.UserCars.plancar.message[0] == "已超过上限") {
								$("#mangerCar_tc a").attr("href", "http://i.yiche.com/u" + Bitauto.Login.result.userId + "/car/guanzhu/");
								$("#FocusCarFull").show();
							}
							else {
								obj.attr("title", "取消关注").find("span").removeClass("no-sc");
								Bitauto.UserCars.plancar.arrplancar.unshift(id);
							}
						}) : Bitauto.UserCars.delConcernedCar(id, function () {
							obj.attr("title", "点击关注").find("span").addClass("no-sc");
						});
					});
				};
				//添加关注
				$("#serial_guanzhu").bind("click", function () {
					FocusCar($(this));
				});

				$(function () {
					initLoginFavCar();
					//车型列表效果
					$('div.c-list-2014 tr').hover(
		function () {
			$(this).addClass('hover-bg-color');
			$(this).find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
			if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
				$(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
		},
		function () {
			$(this).removeClass('hover-bg-color');
			$(this).find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
			if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
				$(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
		}
	);
					$("#pop_nosale").hover(
		function () { $("#pop_nosalelist").show(); },
		function () { $("#pop_nosalelist").hide(); }
	);
					$("#a-focus-close,#btn-focus-close").click(function () { $("#FocusCarFull").hide(); });
				});
				//补贴接口
				function getSubsidy(serialId, cityId) {
				    $.ajax({
				        url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + serialId + "&cityid=" + cityId + "",
				        dataType: "jsonp",
				        jsonpCallback: "getSubsidyCallback",
				        cache: true,
				        success: function (data) {
				            if (!(data && data.length > 0)) return;
				            $.each(data, function (i, n) {
				                if (!(n.StateSubsidies > 0 && n.LocalSubsidy > 0)) return;
				                var carLine = $("#carlist_" + n.CarId);
				                var sub = [];
				                if (n.StateSubsidies > 0)
				                    sub.push("国家补贴" + n.StateSubsidies + "万元");
				                if (n.LocalSubsidy > 0)
				                    sub.push("地方补贴" + n.LocalSubsidy + "万元");
				                if (carLine.find("a[name=\"butie\"]").length > 0) {
				                    carLine.find("a[name=\"butie\"]").attr("title", sub.join(","));
				                } else {
				                    var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"butie\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
				                    if (carLine.find("span.hundong").length > 0) {
				                        carLine.find("span.hundong").after(b);
				                    } else {
				                        carLine.find("a:first").after(b);
				                    }
				                }
				            });
				        }
				    });
				}
				getSubsidy(<%=_SerialId %>,bit_locationInfo.cityId);
				(function () {
					//如果是停售车系，关于商机按钮的处理
					var saleState = '<%=sic.CsPriceRange.ToString()%>';
                	if (saleState == "停售") {
                		var lianJieArray = [];
                		lianJieArray.push("<span class=\"button_orange btn-xj-w\">");
                		lianJieArray.push("<a target=\"_blank\" data-channelid=\"2.22.112\" href=\"http://yiche.taoche.com/<%=_SerialSpell %>/?ref=chexizsmai&leads_source=p015005\"" + ">买二手车</a>");
		            lianJieArray.push("</span>");
		            lianJieArray.push("<span class=\"button_gray btn-qt-w btn-bq-w\">");
		            lianJieArray.push("<a target=\"_blank\" data-channelid=\"\" href=\"http://www.taoche.com/pinggu/?ref=chexizsgu\">二手车估价</a>");
		            lianJieArray.push("</span>");
		            $('.sc-btn-box').html(lianJieArray.join(""));
				}
                })();
            </script>
			<script type="text/javascript" charset="gb2312" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
			<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"></script>
			<!-- footer -->
			<script type="text/javascript">
				var CarCommonCSID = '<%= _SerialId.ToString() %>';
			</script>
			<!--#include file="~/html/treefooter2014.shtml"-->
		</div>
	</div>


	<!-- 对比浮动框 -->
	<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none; margin-right: -380px;" animateright="-533" animatebottom="180" data-page="summary">
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
		<div class="car-summary-btn-duibi button_gray"><a href="javascript:;" target="_self"><span>对比</span></a></div>
	</div>
	<%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150733"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160120"></script>
	<script type="text/javascript">
		$(function(){
			WaitCompareObj.Init();
		});
	</script>


	<!-- 对比浮动框 -->
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/carcompareformini.js?v=20141009"></script>
	<div id="divWaitCompareLayer" class="comparebar" style="display: none;">
	</div>--%>
	<script type="text/javascript">
		// 对比浮动框
		//insertWaitCompareDiv();

		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<div id="FocusCarFull" style="display: none;" class="tc-popup-box">
		<div class="tt">
			<h6>管理关注的车</h6>
			<a href="javascript:;" id="btn-focus-close" class="btn-close">关闭</a>
		</div>
		<div class="tc-popup-con">
			<div class="no-txt-box no-txt-error-mline">
				<p class="tit">
					不能再继续添加了...
				</p>
				<p>
					您最多可以添加 9辆关注车型，去车库删除一部分车型后就可以继续添加了。
				</p>
				<div class="tc_zf_box">
					<div class="button_orange button_113_35">
						<a href="javascript:;" id="a-focus-close">我知道了</a>
					</div>
					<div id="mangerCar_tc" class="button_gray button_113_35">
						<a target="_blank" href="#">管理关注的车</a>
					</div>
				</div>
			</div>
			<div class="clear">
			</div>
		</div>
	</div>
</body>
</html>
<!--本站统计代码-->
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript">
	OldPVStatistic.ID1 = "<%=_SerialId.ToString() %>";
	OldPVStatistic.ID2 = "0";
	OldPVStatistic.Type = 4;
	mainOldPVStatisticFunction();
</script>
<!--本站统计结束-->
