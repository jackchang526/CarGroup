<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsPhoto.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsPhoto" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=strCs_SeoName%>图片】_<%=cse.MasterName+cse.Cs_Name%>图片_<%=strCs_SeoName%>图片大全-易车网</title>
	<meta name="keywords" content="<%=strCs_SeoName%>图片,<%=strCs_SeoName%>外观,<%=strCs_SeoName%>内饰图,<%=cse.MasterName+cse.Cs_Name%>,易车网,car.bitauto.com" />
	<meta name="description" content="<%=cse.MasterName+cse.Cs_Name%>图片，展示各种角度<%=strCs_SeoName%>图片,包括<%=strCs_SeoName%>外观,<%=strCs_SeoName%>内饰,<%=strCs_SeoName%>内部空间,<%=strCs_SeoName%>行驶等最新<%=strCs_SeoName%>图片壁纸。" />
	<meta name="mobile-agent" content="format=html5; url=http://photo.m.yiche.com/serial/<%= serialId %>/" />
	<meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/photo.aspx?serialid=<%= serialId %>" />
	<link rel="canonical" href="http://car.bitauto.com/<%=cse.Cs_AllSpell %>/tupian/" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car_pic-744.shtml"-->
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<!--a_d start-->
	<div class="bt_ad">
		<!-- AD New Dec.31.2011 -->
		<ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
	</div>
	<!--a_d end-->
	<!--smenu start-->
	<%= CsHeadHTML %>
	<!--smenu end-->
	<!--contain begin-->
	<div class="bt_page">
		<%--<%=SerialInfoBarHtml%>--%>
		<div class="col-all">
		<!--col-con start-->
		<div class="col-con">
			<%=SerialPhotoHtml %>
			<!-- AD -->
			<div class="line_box">
				<ins id="div_406b829f-0166-4c17-bf92-2381668a81eb" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="406b829f-0166-4c17-bf92-2381668a81eb"></ins>
			</div>
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
					<li><a href="http://www.taoche.com/<%= cse.Cs_AllSpell %>/" target="_blank">
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
 			<div class="col-side_ad">
				<ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853"></ins>
			</div>
			<%--<div class="col-side_ad">
				<ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
			</div>--%>
			<div class="col-side_ad">
				<ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
				</ins>
			</div>
			<div class="col-side_ad">
				<ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
				</ins>
			</div>
			<div class="col-side_ad">
				<ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
				</ins>
			</div>
			<div class="col-side_ad">
				<ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db"></ins>
			</div>
			<!--车型图片对比-->
			<%= CsHotCompareCars%>
 			<div class="line-box" id="serialtosee_box">
					<div class="side_title">
						<h4>
							看过此车的人还看</h4>
					</div>
					<ul class="pic_list">
						<%--<%=serialToSeeHtml%>--%>
					</ul>
				</div>
			<%--<car:SerialToSee ID="ucSerialToSee" runat="server"></car:SerialToSee>--%>
			<!-- AD -->
			<!-- add by chengl Sep.13.2012 -->
			<%--<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
				adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
				adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c"></ins>--%>
			<!---->
			<!--此品牌下其别子品牌-->
 			<div class="line-box">
				<%=GetBrandOtherSerial() %>
				<div class="clear">
				</div>
			</div>
			<!--二手车-->
			<%--<%=UCarHtml %>--%>
			<%--<div class="line_box ucar_box">
			</div>--%>
			<div class="col-side_ad">
				<ins id="div_a9e57a09-3485-4fbe-a755-59d7e52ce486" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="a9e57a09-3485-4fbe-a755-59d7e52ce486"></ins>
			</div>
            <script type="text/javascript">
                var serialId = "<%= serialId %>";
                <%= serialToSeeJson %>
                //var SerialAdpositionContentInfo = {
                //    "2370": [
                //        {
                //            "SerialID": "2371",
                //            "Text": "速腾",
                //            "Link": "http://car.bitauto.com/suteng/",
                //            "Image": "http://img3.bitautoimg.com/autoalbum/files/20120816/666/0407166663_5.jpg"
                //        }, null]
                //};
            </script>
            <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialtoseead.js"></script>
			<div class="col-side_ad" style="margin-bottom: 10px; overflow: hidden">
				<%--<script type="text/javascript" id="zp_script_95" src="http://mcc.chinauma.net/static/scripts/p.js?id=95&w=220&h=220&sl=1&delay=5"
					zp_type="1"></script>--%>
				<script type="text/javascript" id="zp_script_243" src="http://mcc.chinauma.net/static/scripts/p.js?id=243&w=240&h=220&sl=1&delay=5"
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
		</div>
		<!--col-side end-->
			<div class="clear">
			</div>
		</div>
	</div>
	<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
	<script type="text/javascript" language="javascript">
		var CarCommonCSID = '<%= serialId.ToString() %>';
	</script>
	<script type="text/javascript">
		var adplay_CityName = ''; //城市
		var adplay_AreaName = ''; //区域
		var adplay_BrandID = '<%= serialId %>'; //品牌id 
		var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
		var adplay_BrandName = ''; //品牌
		var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
	</script>
	<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
	<!--#include file="~/html/footer2014.shtml"-->
	<!--本站统计代码-->
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript" language="javascript">
		OldPVStatistic.ID1 = "<%=serialId %>";
		OldPVStatistic.ID2 = "0";
		OldPVStatistic.Type = 0;
		mainOldPVStatisticFunction();
	</script>
	<!--本站统计结束-->
	<!-- baa 浏览过的车型-->
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
	<script type="text/javascript">
		try {
			Bitauto.UserCars.addViewedCars('<%=serialId.ToString() %>');
		}
		catch (err)
		{ }
	</script>
	<!--contain end-->
	<%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
   { %>
	<!--百度热力图-->
	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<%} %>
  	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=serialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
<%--<script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=2013112017"
	type="text/javascript"></script>
<script>
	var cityId = 201;
		if(typeof (bit_locationInfo) != 'undefined'){
			cityId = bit_locationInfo.cityId;
		}
    if(typeof(showUCar)!="undefined"){ 
            showUCar(<%=serialId %>, cityId,'<%= cse.Cs_AllSpell %>','<%=strCs_ShowName%>');
    }
</script>--%>
<script language="javascript" type="text/javascript">
	if (document.getElementById('carYearList_all')) { document.getElementById('carYearList_all').className = 'current'; }
	// 浏览过的子品牌
	// initPageForVisitSerial("<%= serialId %>");

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
