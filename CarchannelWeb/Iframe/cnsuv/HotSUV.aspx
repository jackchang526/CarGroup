<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotSUV.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Iframe.cnsuv.HotSUV" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>最热门10个SUV子品牌</title>
    <style type="text/css">
        ul{list-style:none;}
        ul li {list-style-type:none;}
        *{padding:0;margin:0}
        .jz{margin:40px auto}
        .h370{width:958px;height:370px}
        .line_box{border:1px solid #dddddd;position:relative;clear:both;float:none;}
        .line_box h3{font-family:"Microsoft YaHei";font-weight:bold;font-size:18px;color:#1c3e83;padding-left:10px;background:url(http://image.bitautoimg.com/uimg/car/images/20120607_suv_title_bg.png) repeat-x;height:37px;line-height:37px}
        .line_box .more{position:absolute;right:10px;font-size:12px;top:13px;color:#1f376d;text-decoration:none}
        .cx{text-align:center;margin-top:10px}
        .cx li{width:190px;height:162px;float:left;overflow:hidden}
        .cx li img{width:150px;height:100px;border:1px solid #dddddd}
        .cx li .name{font-weight:bold;}
        .cx li .name a{color:#1f376d;text-decoration:none}
        .cx li .name a:hover{text-decoration:underline}
        .cx li .list_mxl{text-align:center;color:#cccccc;text-align:center;font-size:12px;height:20px;line-height:20px}
        .cx li .list_mxl a{margin:0 4px;color:#333;text-decoration:none}
        .cx li .list_mxl a:hover{text-decoration:underline}
        </style>

</head>
<body>
    <div class="line_box h370">
     <h3><span>热门SUV</span></h3>
     <a href="http://car.bitauto.com/suv/" target="_blank" class="more">更多&gt;&gt;</a>
     <ul class="cx">
         <%=showSerialSUV %>
     </ul>
</div>
</body>
</html>