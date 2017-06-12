<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IframeForCompare.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.IframeForCompare" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
    <title>车型对比</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <!--#include file="~/ushtml/car/bitauto_car.shtml"-->
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsnew/JsForParameterv2.js"></script>  
</head>
<body>
<div class="bt_page">
    <style> 
        .bt_page{overflow:visible}
    </style>
        <!--[if IE 6]> <style>.bt_page{ overflow:hidden;} }</style> <![endif]-->
	<div class="car_compare_add">
		<ul>
			<li id="compareheadHasData"><input type="checkbox" name="checkboxForDelTheSame" onclick="delTheSameForCompare();" /> 只显示差异项目</li>
			<li class="r"></li>
		</ul>
	</div>
	<div id="CarCompareContent" class="car_compare_list" style="position:relative;clear:both">
	</div> 
	<!-- 等待 -->
    <div id="popWin" style="display:block;border:1px;width:300px">
	    <div class="line_box">
		    <h3><span><a>请稍候...</a></span></h3>		
		    <div id="aa" style="height: 100px; overflow: auto; padding-left: 125px;padding-top:60px">
			    <img src="http://car.bitauto.com/car/images/clip_image.gif" border="0" alt="请等待" />
		    </div>
	     </div>
    </div>
    <script type="text/javascript" language="javascript">
    	// 等待开始
    	var pop_Win = document.getElementById('popWin');
    	if (pop_Win) {
    		pop_Win.style.display = 'block';
    		pop_Win.style.top = document.documentElement.scrollTop + document.documentElement.clientHeight / 2 - pop_Win.offsetHeight / 2 + 'px';
    		pop_Win.style.left = (document.documentElement.clientWidth / 2 - pop_Win.offsetWidth / 2) + 'px';
    	}
    	ComparePageObject.CarIDAndNames = "<%= carIDAndName %>";
    	initPageForCompare("/car/");
    	// 等待结束
    	if (pop_Win)
    	{ pop_Win.style.display = 'none'; }
    </script>
</div>
</body>
</html>
