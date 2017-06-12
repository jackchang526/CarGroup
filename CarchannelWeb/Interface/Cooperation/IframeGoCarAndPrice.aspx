<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IframeGoCarAndPrice.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.Cooperation.IframeGoCarAndPrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>福州新闻网合作块</title>
    <style type="text/css">
        body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6, pre, form, fieldset, input, textarea, p, blockquote, th, td
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            font-size: 12px;
            font-family: simSun, Arial, Helvetica, sans-serif;
            background: #fff;
            line-height: 150%;
        }
        .bitautobox
        {
            width: 743px;
            height: 30px;
            margin: auto;
        }
        ul
        {
            list-style: none;
        }
        .model_city
        {
            position: relative;
            z-index: 9999;
            background: url(http://image.bitautoimg.com/uimg/index091217//images/bitauto_h2_100118.png) no-repeat -5px -457px;
            height: 27px;
            padding: 3px 0 0 0;
            width: 743px;
            margin: 0 auto;
        }
        .model_city .select_model
        {
            width: 130px;
            float: left;
            margin-right: 5px;
            color: #666;
        }
        .model_city .select_car
        {
            background: url(http://image.bitautoimg.com/uimg/index091217/images/bitauto_h2_100118.png) no-repeat -346px -121px;
            width: 52px;
            border: 0;
            color: #fff;
            text-align: center;
            font-size: 12px;
            font-weight: 700;
            float: left;
            margin-right: 5px;
            padding: 3px 0 2px; *padding:4px01px;padding:6px03px0\0;cursor:pointer;}
        .model_city ul
        {
            float: left;
            margin-top: 2px;
            padding-left: 5px;
            display: inline;
        }
        .model_city ul li
        {
            float: left;
            background: url(http://image.bitautoimg.com/uimg/index091217/images/bit_icon091231.png) no-repeat -30px -393px;
            padding-left: 10px;
            white-space: nowrap;
            width: 57px;
            overflow: hidden;
        }
        .model_city ul li a:link, .model_city ul li a:visited
        {
            color: #fff;
            text-decoration: none;
        }
        .model_city .select_sea
        {
            width: 200px;
            float: left;
            margin-right: 5px;
            margin-top: 1px;
            border: 1px solid #3B566D;
            padding: 2px 0 1px 5px;
            color: #666;
        }
        .model_city .select_more
        {
            background: url(http://image.bitautoimg.com/uimg/index091217/images/bitauto_h2_100118.png) no-repeat -303px -122px;
            width: 42px;
            border: 0;
            color: #fff;
            text-align: center;
            font-size: 12px;
            font-weight: 700;
            padding: 3px 0 2px; *padding:4px01px;padding:5px03px0\0;cursor:pointer;}
        .model_city .search_text
        {
            position: relative;
            float: left;
            padding: 0 0 0 15px;
            margin-right: 15px;
        }
        .model_city .search_text a.search
        {
            float: left;
            display: block;
            height: 22px;
            width: 65px;
            background: url(http://image.bitautoimg.com/uimg/index091217/images/bitauto_h2_100118.png) no-repeat -225px -88px;
            line-height: 90px;
            overflow: hidden;
        }
        /*search pop*/#sug
        {
            border: 1px solid #817F82;
            position: absolute;
            z-index: 200;
            width: 350px;
            top: 21px;
            background: #fff;
            display: none;
            white-space: nowrap;
            text-align: left;
            overflow: hidden;
        }
        #sug ul
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        #sug ul li
        {
            background: #fff;
            color: #000;
            line-height: 21px;
            padding: 0 5px;
            margin: 0 0 1px 0;
            cursor: default;
            height: 21px;
            font-size: 14px;
            width: 190px;
            overflow: hidden;
        }
        #sug ul li.mo
        {
            background: #014da2;
            color: #fff;
        }
        #sug ul li.right
        {
            text-align: right;
        }
        #sug a:link, #sug a:visited
        {
            color: #014da2;
            text-decoration: none;
        }
        #sug a:hover
        {
            color: #d01d19;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <div class="bitautobox">
        <div class="model_city">
            <!--#  include file="~/ushtml/block/so/so_formonebutton_chexing.shtml"-->
            <!--块内容如下-->            
            <style  type="text/css">
            .sug_reset { background:#fff }
            .sug_onblur { background:#fff url(http://image.bitauto.com/bt/logo/so_search_bg.gif) no-repeat right center }
            </style>
            <div class="search_text">
                <a title="车易搜" href="http://www.cheyisou.com/" class="search" target="_blank">车易搜</a>
                <div style="position: relative; float: left;">
                    <form id="sug_form" name="sug_form" target="_blank" method="get" action="http://www.cheyisou.com/post.aspx">
                    <input type="text" value="" name="so_keywords" id="sug_txt_keyword" class="select_sea sug_onblur" autocomplete="off" onblur="blurlogo(this)" onfocus="focuslogo(this)">
                    <input type="submit" value="搜索" class="select_more" id="sug_submit" />
                    <input id="sug_datatype" name="sug_datatype" value="qiche" type="hidden" />
                    <input id="sug_encoding" name="sug_encoding" value="utf-8" type="hidden" />
                </div>
            </div>
            <script type="text/javascript">
                function blurlogo(obj) {
                    if (obj.value.length == 0)
                        obj.className = "select_sea sug_onblur";
                }
                function focuslogo(obj) {
                    obj.className = "select_sea sug_reset";
                }
            </script>
            <select class="select_model" id="MasterSelectList">
            </select>
            <select class="select_model" id="SerialSelectList">
                <option value="-1">请选择系列</option>
            </select>
            <input type="button" value="看车型" class="select_car" title="看车型" id="GotoCarButton">
            <input type="button" value="查报价" title="查报价" class="select_car" id="GotoPriceButton">

            <script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsCommon/jsGoCarAndPriceJsonNew.js"></script>

            <script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsnew/GoCarTypeAndPrice.js"></script>

            <script type="text/javascript" language="javascript">
            	var PageSelectObject;
            	if (JSonData) {
            		PageSelectObject = StartInit("select", "select", "MasterSelectList", "SerialSelectList", "GotoCarButton", "GotoPriceButton", "0");
            		PageSelectObject.Init();
            	}
            </script>

        </div>
    </div>
</body>
</html>

