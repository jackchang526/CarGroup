<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhoto.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageCar.CarPhoto" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSee.ascx" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片】_<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片库-易车网
	</title>
	<meta name="keywords" content="<%= cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim() %>图片,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内饰图,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>" />
	<meta name="description" content="<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片:易车网(BitAuto.com)图片频道为您提供专业高清<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片,展示各种角度<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>图片,包括<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>车身外观,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内饰,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>内部空间,<%=cbe.Cs_Name.Trim() + " " + cbe.Car_Name.Trim()%>行驶,创意图,壁纸,活动等。" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car_pic-744.shtml"-->
	<script src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js" type="text/javascript"></script>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<!--a_d start-->
	<div class="bt_ad">
		<!-- AD New Dec.31.2011 -->
		<ins id="div_e457221e-faa7-4799-9172-b09cc8c30c91" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="e457221e-faa7-4799-9172-b09cc8c30c91"></ins>
	</div>
	<!--a_d end-->
	<!--smenu start-->
	<%= CarPhotoHeadHTML%>
	<!--smenu end-->
	<!--contain begin-->
	<div class="bt_page">
	<div class="col-all">
		<!--col-con start-->
		<div class="col-con">
			<%=CarPhotoHtml %>
			<!-- SEO底部热门 -->
			<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
			<!-- SEO底部热门 -->
		</div>
		<!--col-con end-->
		<!--col-side start-->
		<div class="col-side">
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= cbe.Cs_Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
				</ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= cbe.Cs_Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
				</ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= cbe.Cs_Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
				</ins>
			</div>
			<%--<!--车型图片对比-->
			<%= SerialHotCompare %>--%>
			<!-- 热门车型 -->
			<div class="line-box">
				<div class="side_title">
					<h4>
						<a href="javascript:;">
							<%= cbe.Cs_ShowName.Replace("(进口)", "").Replace("（进口）", "")%>
							热门车型</a></h4>
				</div>
				<ul class="text-list">
					<%= hotCarsHtml %>
				</ul>
				<div class="clear">
				</div>
			</div>
 			<%--<div class="line-box">
				<div class="side_title">
					<h4>
						看了<%=cbe.Cs_ShowName.Trim().Replace("(进口)", "").Replace("（进口）", "")%>的还看</h4>
				</div>
				<ul class="pic_list">
					<%=serialToSeeHtml%>
				</ul>
			</div>--%>
			<car:SerialToSee ID="ucSerialToSee" runat="server">
			</car:SerialToSee>
			<!--此品牌下其别子品牌-->
			<div class="line-box">
				<%=GetBrandOtherSerial() %>
				<div class="clear">
				</div>
			</div>
			<!-- 和车主聊聊 -->
			<!--%= UserBlock %-->
			<!--二手车-->
			<%--<%=UCarHtml %>--%>
			<%--<div class="line_box ucar_box">
			</div>--%>
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
	<script type="text/javascript" language="javascript">
		var CarCommonCarID = '<%= CarID.ToString() %>';
	</script>
	<!--#include file="~/html/footer2014.shtml"-->
	<!--本站统计代码-->
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript" language="javascript">
		OldPVStatistic.ID1 = "<%=cbe.Cs_Id %>";
		OldPVStatistic.ID2 = "<%=CarID.ToString() %>";
		OldPVStatistic.Type = 0;
		mainOldPVStatisticFunction();
		if (typeof (jQuery) != 'undefined') {
			//$(document).ready(function() {
			var tabsPhoto = $('#serialPhotoDiv dl.theme dd');
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
					$('#other_tab dd').css({ 'z-index': '999' }).appendTo($('#serialPhotoDiv dl.theme'));
				}

				//设置组选中
				if (typeof (groupId) != 'undefined' && groupId >= 0) {
					var current = $('#tabex_' + groupId);
					if (current[0] && current.parent().attr('tagName').toLowerCase() == 'li') {
						$('#serialPhotoDiv dl.theme dd:last').before('<dd>' + current.parent().html() + '</dd>');
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
	<!-- baa 浏览过的车型-->
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
	<script type="text/javascript">
		try {
			Bitauto.UserCars.addViewedCars('<%=cbe.Cs_Id %>');
		}
		catch (err)
		{ }
	</script>
	<!--contain end-->
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=cbe.Cs_Id%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
		var zamplus_tag_params = {
			modelId:modelStr,
			carId:<%=CarID%>
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
<%--<script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=2013020409"
	type="text/javascript"></script>
<script>
	var cityId = 201;
		if(typeof (bit_locationInfo) != 'undefined'){
			cityId = bit_locationInfo.cityId;
		}
    if(typeof(showUCar)!="undefined"){ 
            showUCar(<%=cbe.Cs_Id %>, cityId,'<%=cbe.Cs_AllSpell %>','<%=cbe.Cs_ShowName%>');
    }
</script>--%>
