<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalcAutoCashTool.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.hsw.CalcAutoCashTool" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/jsnew/prototype.js"></script>
    <link rel="stylesheet" type="text/css" href="http://car.bitauto.com/car/interface/hsw/css/wd_iframe.css" />
    <script type="text/javascript" src="http://car.bitauto.com/car/interface/hsw/js/wd_iframe.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/GoCarTypeAndPrice.js"></script>
    <script type="text/javascript" src="http://car.bitauto.com/car/ajaxnew/GoCarTypeAndPrice_Json.ashx"></script>
    <script type="text/javascript" src="http://car.bitauto.com/car/ajaxnew/GetSerailNoCarType_JSon.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/Added_GoCarTypeAndPrice.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/CalculateToolJs.js"></script>
    
</head>
<body>
    <div class="bitauto_calc">
        <select id="ddlPinpai" name="ddlPinpai">
        </select>
        <select id="ddlChexing" name="ddlChexing" onchange="getCarByCsID(this.value)">
        </select>
        <select id="ddlChekuan" name="ddlChekuan" onchange="resetPrice(this.value);$('hiddenCarID').value=this.value;">
            <option>选择车型</option>
        </select>
        <input id="txtMoney" name="txtMoney" type="text" value="输入购车价格" onchange="checkCarPrice();" class="input_txt" maxlength="8" onfocus="if (value =='0'){value =''}" onblur="if (value ==''){value='0'}" />
        <input id="gotoCalc" type="submit" value="开始计算" class="input_submit" onclick="gotoCalcAutoCashTool();" />
        <input type="hidden" id="hiddenCarID" value="" />
        <input type="hidden" id="CarType" value="" />
    </div>
</body>
</html>

<script type="text/javascript">
	var priveRequest = "";
	//初始化主品牌列表
	var PageSelectObject = StartInit("select", "select", "ddlPinpai", "ddlChexing", "CarType", "", "");
	PageSelectObject.Init();

	function gotoCalcAutoCashTool() {
		if ($('hiddenCarID')) {
			if (($('hiddenCarID').value * 1) > 0) {
				var url = "http://car.bitauto.com/gouchejisuanqi/?carID=" + $('hiddenCarID').value
				var carprice = $('txtMoney').value;
				if (checkIsNum(carprice) && carprice != "0") {
					url += "&carPrice=" + carprice;
				}
				window.open(url, "", "", "");
			}
		}
	}

	function checkCarPrice() {
		var carprice = $('txtMoney').value;
		if (!checkIsNum(carprice)) {
			$('txtMoney').value = "0";
			alert("请输入正整数");
		}
	}

	function checkIsNum(str) {
		var ValidChars = "0123456789";
		IsNumber = true;
		var Char;
		for (i = 0; i < str.length && IsNumber == true; i++) {
			Char = str.charAt(i);
			if (ValidChars.indexOf(Char) == -1) {
				IsNumber = false;
			}
		}
		return IsNumber;
	}
</script>
