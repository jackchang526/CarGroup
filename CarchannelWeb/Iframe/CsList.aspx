<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsList.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Iframe.CsList" %>

<% if (dept == "114la")
   { %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<style type="text/css">
body, div, dl, dt, dd, ul, ol, li, p{ margin:0; padding:0; }
ol, ul,li { list-style:none; }
body{ font-size:12px}
a:link, a:visited { color:#000; text-decoration:none; }
a:hover { color:#cc0000; text-decoration:underline; }
.hzmt_main{ width:678px; border:#dddddd 1px solid; background:#FFF; overflow:hidden;zoom:1; height:143px}
.hzmt_main .car_model_list{ height:31px;  background:#f9f9f9; line-height:31px; border-bottom:#dddddd 1px solid}
.hzmt_main .car_model_list ul{ overflow:hidden;zoom:1; font-size:14px; color:#313131 }
.hzmt_main .car_model_list ul a:hover{color:#0e6dbc;}
.hzmt_main .car_model_list ul li{ float:left; padding:0 5px; cursor:pointer}
.hzmt_main .car_model_list ul li.current{ border-top:#0e6dbc 2px solid; color:#0e6dbc; font-weight:bolder; border-left:1px solid #dddddd;border-right:1px solid #dddddd; background:#FFF}
.hzmt_main dl{ height:36px; line-height:36px; font-size:12px; border-bottom:1px dotted #dddddd; margin:0 5px}
.hzmt_main dt{ width:65px; text-align:right;  color:#666666; float:left; padding:0 10px}
.hzmt_main dt.carn{ text-align:left}
.hzmt_main dd{ width:80px; float:left; display:inline; text-align:left; overflow:hidden}
</style>
</head>
<body>
	<div class="hzmt_main">
		<!--#include file="~/include/partner/hezuo_part/00001/201210_hzwz_rmcx_Manual.shtml"-->
	</div>
	<script type="text/javascript">
		var ulbox = document.getElementById("data_tab0");
		var libox = ulbox.getElementsByTagName("li");
		for (var i = 0; i < libox.length; i++) {
			libox[i].onmouseover = (function tab(i) {
				return function () {
					for (var j = 0; j < libox.length; j++) {
						libox[j].className = "";
						document.getElementById("data_box0_" + j).style.display = "none";
					}
					libox[i].className = "current";
					document.getElementById("data_box0_" + i).style.display = "block";
				}
			})(i);
		}
	</script>
</body>
</html>
<% } %>
