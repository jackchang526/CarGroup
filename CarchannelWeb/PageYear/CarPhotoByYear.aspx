<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhotoByYear.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.PageYear.CarPhotoByYear" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSee.ascx" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=strCs_SeoName%>
		<%= this.CarYear.ToString()+"款" %>图片】_<%=strCs_MasterName%><%=strCS_Name%>
		<%= this.CarYear.ToString()+"款" %>图片库-易车网</title>
	<meta name="keywords" content="<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>内饰图,<%=strCs_MasterName%><%=strCs_ShowName%> <%= this.CarYear.ToString()+"款" %>" />
	<meta name="description" content="<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片:频道为您提供<%=strCs_MasterName%><%=strCS_Name%> <%= this.CarYear.ToString()+"款" %>内饰,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>内部空间,<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>行驶,创意图,壁纸,活动等,查<%=strCs_SeoName%> <%= this.CarYear.ToString()+"款" %>图片,就上易车网" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car_pic-744.shtml"-->
	<script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<!--a_d start-->
	<div class="bt_ad">
		<%=serialTopAdCode%>
	</div>
	<!--a_d end-->
	<!--smenu start-->
	<%= CsHeadHTML %>
	<script type="text/javascript">
    <%= JsTagForYear %>
	</script>
	<!--smenu end-->
	<!--contain begin-->
	<div class="bt_page">
		<div class="col-all">
			<!--col-con start-->
			<div class="col-con">
				<%=SerialYearPhotoHtml%>
				<!-- SEO导航 -->
				<div class="line-box">
					<div class="title-con">
						<div class="title-box title-box2">
							<h4>
								接下来要看</h4>
						</div>
					</div>
					<div class="text-list-box-b">
						<div class="text-list-box">
							<ul class="text-list text-list-float text-list-float3">
								<li><a href="/<%= cse.Cs_AllSpell %>/peizhi/">
									<%= strCs_ShowName%>参数配置</a></li>
								<li><a href="/<%= cse.Cs_AllSpell %>/tupian/">
									<%= strCs_ShowName%>图片</a></li>
								<%=nextSeePingceHtml %>
								<li><a href="/<%= cse.Cs_AllSpell %>/baojia/">
									<%= strCs_ShowName%>报价</a></li>
								<li><a href="http://www.taoche.com/brand.aspx?spell=<%= cse.Cs_AllSpell %>">
									<%= strCs_ShowName%>二手车</a></li>
								<li><a href="/<%= cse.Cs_AllSpell %>/koubei/">
									<%= strCs_ShowName%>怎么样</a></li>
								<li><a href="/<%= cse.Cs_AllSpell %>/youhao/">
									<%= strCs_ShowName%>油耗</a></li>
								<li><a href="<%= baaUrl %>" target="_blank">
									<%= strCs_ShowName%>论坛</a></li>
								<%=nextSeeDaogouHtml %>
							</ul>
						</div>
					</div>
				</div>
 				<!-- SEO底部热门 -->
				<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
				<!-- SEO底部热门 -->
			</div>
			<!--col-con end-->
			<!--col-side end-->
			<div class="col-side">
				<!-- ad -->
				<div class="col-side_ad" style="width: 220px; overflow: hidden">
					<ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= CSID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
					</ins>
				</div>
				<div class="col-side_ad" style="width: 220px; overflow: hidden">
					<ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= CSID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
					</ins>
				</div>
				<div class="col-side_ad" style="width: 220px; overflow: hidden">
					<ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
						adplay_brandid="<%= CSID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
					</ins>
				</div>
				<!--车型图片对比-->
				<%= CsHotCompareCars%>
				<%--<div class="line-box">
					<div class="side_title">
						<h4>
							看了<%=strCs_ShowName.Replace("(进口)", "").Replace("（进口）", "")%>的还看</h4>
					</div>
					<ul class="pic_list">
						<%=serialToSeeHtml%>
					</ul>
				</div>--%>
				<car:SerialToSee ID="ucSerialToSee" runat="server"></car:SerialToSee>
				<!--此品牌下其别子品牌-->
				<div class="line-box">
					<%=GetBrandOtherSerial() %>
					<div class="clear">
					</div>
				</div>
				<!--二手车-->
				<%--<%=UCarHtml %>--%>
				<%--<div class="line_box ucar_box"></div>--%>
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
			<!--col-side end-->
			<div class="clear">
			</div>
		</div>
	</div>
	<!--#include file="~/html/footer2014.shtml"-->
	<!--本站统计代码-->
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript" language="javascript">
		OldPVStatistic.ID1 = "<%=CSID.ToString() %>";
		OldPVStatistic.ID2 = "0";
		OldPVStatistic.Type = 0;
		mainOldPVStatisticFunction();

		if (typeof (jQuery) != 'undefined') {
			//$(document).ready(function() {
			var tabsPhoto = $('dl.theme dd');
			if (tabsPhoto && tabsPhoto.length > 0) {
				//重构其它标签的内容
				if (tabsPhoto.length >= 8) {
					var html = '';
					$.each(tabsPhoto, function (k, v) {
						if (k > 5 && v.className != 'otherDD') {
							html += '<li>' + $(v).html() + '</li>';
							$(v).remove();
						}
					});
					$('#other_tab .other').html(html);
					$('#other_tab dd').css({ 'z-index': '999' }).appendTo($('dl.theme'));
				}

				//设置组选中
				if (typeof (groupId) != 'undefined' && groupId >= 0) {
					var current = $('#tabex_' + groupId);
					if (current[0] && current.parent().attr('tagName').toLowerCase() == 'li') {
						$('dl.theme dd:last').before('<dd>' + current.parent().html() + '</dd>');
						current.parent().remove();
					}
					$('#tabex_' + groupId).parent().text(current.text()).addClass('current');
				}
			}

			//弹出层
			$('.ico').parent().mouseover(function () {
				$(this).find('div').show();

			}).mouseout(function () {
				$(this).find('div').hide();
			});

			//设置车型选择
			if (typeof (carId) != 'undefined' && carId > 0) {
				var html = ' | <strong>' + $('#car_' + carId + ' a').attr('title') + '</strong>';
				$('#car_' + carId).parents('dd').append(html).find('.ico').addClass('current');
				if ($('#car_' + carId).parent().children().length == 1) {
					$('#car_' + carId).parents('dd').unbind('mouseover');
					$('#car_' + carId).parents('dd').find('.ico').removeClass('ico');
				}
				$('#car_' + carId).remove();
			}
			//});
		}
	</script>
	<!--本站统计结束-->
	<!--contain end-->
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=CSID%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
 		var zamplus_tag_params = {
			modelId: modelStr,
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
