<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellDataChart.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.SellDataChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>产销数据</title>
	<link href="/car/css/index_style.css" rel="stylesheet" type="text/css"/>
	<<link href="http://www.bitauto.com/themes/2009/common/css/reset.css" rel="stylesheet" type="text/css" />
	<link href="http://www.bitauto.com/themes/2009/common/css/common.css" rel="stylesheet" type="text/css" />
	<link href="/car/css/alen.css" rel="stylesheet" type="text/css" />
	<link href="/car/css/alen.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../jsnew/SellDataInterfacev2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:950px;margin:0 auto"><img src="../images/public_head.jpg"></div>
	<br />

	<div class="bt_page">

		<div class="col-sub">
		<!--focus_big start-->
    		<div class="line_box focus_big"> <img height="200" width="300" src="../images/common/w300_200.jpg"/> <a href="#">标致307两厢跌破10万元 恐难维持多久</a> </div>    <!--focus_big end-->
  		 <!--focus_big end-->
		</div>
		<div class="col-main">
			<div class="topnews2">
				<h2><a href="#">虚拟与现实 我们身边的汽车人</a></h2>
				<p>内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容内容...<a href="#">[详细]</a></p>
				<ul>
				  <li><span>[ <a href="#">北京</a> ]</span> <a target="_blank" href="#">迈腾1.8L精英型跌破20万</a><small>06月19日</small></li>
				  <li><span>[ <a href="#">北京</a> ]</span> <a target="_blank" href="#">迈腾1.8L精英型跌破20万</a><small>06月19日</small></li>
				  <li><span>[ <a href="#">北京</a> ]</span> <a target="_blank" href="#">迈腾1.8L精英型跌破20万</a><small>06月19日</small></li>
				  <li><span>[ <a href="#">北京</a> ]</span> <a target="_blank" href="#">迈腾1.8L精英型跌破20万</a><small>06月19日</small></li>
				  <li><span>[ <a href="#">北京</a> ]</span> <a target="_blank" href="#">迈腾1.8L精英型跌破20万</a><small>06月19日</small></li>
				</ul>
			</div>
		</div>
		
		<div class="col-side">
			<div class="line_box ranking_list" style="height:240px">
				<h3><span class="s2"><a href="#">产销报告</a></span></h3>
				<ul class="list">
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1828.html">MG7自动挡刚提车200公里</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1828.html">宝马X5最高降幅达到</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/2408.html">景程优惠2万元 力抢中低端</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/2289.html">志俊优惠1.38万 郊区成主力</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1810.html">迈腾1.8L精英型跌破20万</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/2410.html">索纳塔降1万 主推低端商务</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1748.html">同级车中的高品质！试驾江</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1617.html">八面玲珑 西班牙试驾雪佛</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1748.html">同级车中的高品质！试驾江</a></li>
				  <li><a target="_blank" href="http://car.bitauto.com/serial/1617.html">八面玲珑 西班牙试驾雪佛</a></li>
				</ul>
				<div class="more"><a href="#">更多>></a></div>
			  </div>
		  </div>
		  
		  <script language="javascript">
		  <!--
		  	var chart = new BAFlashChart();
		  	chart.Self = true;
		  	chart.GenterateSellQuery();
		  	chart.InitSellDataMapFrame();
		  	chart.GenerateSellDataTable();
		  	chart.GenerateSellDataTableWithDataType(["car", "suv", "mpv"])
		  -->
		  </script>
	</div>

	
    </form>
</body>
</html>
