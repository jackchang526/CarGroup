// JScript File
//------------------ Common ------------------------------
function SetSpanValueByBrowerType(control, value)
{
    $("#"+control).html(value);
}

function GetIntValue(num) {
    num = num.toString().replace(/\,/g, '');
    return parseInt(num);
}

function formatCurrency(num) {
    num = num.toString().replace(/\$|\,/g,'');
    if(isNaN(num)) num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num*100+0.50000000001);
    num = Math.floor(num/100).toString();
    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
        num = num.substring(0,num.length-(4*i+3)) + ',' + num.substring(num.length-(4*i+3));
    return (((sign)?'':'-') + num);
}

function formatCurrencyWToK(num) {
    num = num.toString().replace(/\$|\,/g,'');
    if(isNaN(num)) num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 10000 + 0.50000000001).toString();
    return (((sign)?'':'-') + num);
}

//------------------ Common ------------------------------
function setCalcToolUrl(carId) {
    carId = parseInt(carId);
    var ulEle = document.getElementById("calcTools");
    if (!ulEle)
        return;
    var aLinks = ulEle.getElementsByTagName("A");
    for (i = 0; i < aLinks.length; i++) {
        var aLink = aLinks[i];
        var url = aLink.href;
        if (url.length == 0)
            continue;
        var paraIndex = url.indexOf("?");
        if (paraIndex > 0)
            url = url.substring(0, paraIndex);
        if (carId > 0) {
            url += "?carid=" + carId;
        }
        aLink.href = url;
    }
}


function setCalcToolUrlByPrice(hidCarPrice) {
    var ulEle = document.getElementById("calcTools");
    if (!ulEle)
        return;
    var aLinks = ulEle.getElementsByTagName("A");
    for (i = 0; i < aLinks.length; i++) {
        var aLink = aLinks[i];
        var url = aLink.href;
        if (url.length == 0)
            continue;
        var paraIndex = url.indexOf("?");
        if (paraIndex > 0)
            url = url.substring(0, paraIndex);
        url += "?CarPrice=" + hidCarPrice;
        aLink.href = url;
    }
}

//交强险

//function calcCompulsory() {
//    var compulsoryValue = document.getElementById("selCompulsory").value;
//    SetSpanValueByBrowerType('lblCompulsory', formatCurrency(compulsoryValue));
//    if (isNaN($('#txtMoney').val()) || $('#txtMoney').val().length == 0 || $('#txtMoney').val() == "0") {
//        jQuery("#lblTotalCompulsory").html("0");
//    } else {
//        SetSpanValueByBrowerType('lblTotalCompulsory', formatCurrency(compulsoryValue));
//    }
//}

//发动机特别损失险(车损险*5%)
function calcCarEngineDamage() {
    if ($("#chkEngine").prop("checked")) {
        var cDamage = GetIntValue(jQuery("#lblCarDamage").html()) * 0.05;
        SetSpanValueByBrowerType('engineDamage', formatCurrency(Math.round(cDamage)));
    } else {
        jQuery("#engineDamage").html("0");
    }
}
//司机责任险
function calcLimitofDriver() {
    if ($('#chkLimitofDriver').prop("checked") ) {
        if (jQuery('#selCompulsory').prop('selectedIndex') == 0) {   //6座以下
            //所选金额*费率*（座位数-1）。如果没有座位数，则*4
            var lvalue1 = Math.round(jQuery("#selLimitofDriver option:selected").val() * 0.0042);
            jQuery("#lblLimitOfDriver").html(lvalue1);
        } else {
            var lvalue2 = Math.round(jQuery("#selLimitofDriver option:selected").val() * 0.004);
            jQuery("#lblLimitOfDriver").html(lvalue2);
        }
    }
    else {
        jQuery("#lblLimitOfDriver").html("0");
    }

}
//第三者责任险
function calcTPL() {
    var selCompulsoryIndex = document.getElementById("selCompulsory").selectedIndex;
    if ($('#chkTPL').prop("checked")) {
        var selTPLValue = document.getElementById("selTPL").selectedIndex;
        if (selCompulsoryIndex == 0) {
            if (selTPLValue == 0) {
                jQuery("#lblTPL").html("710");
                return 710;
            }
            if (selTPLValue == 1) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1026));
                return 1026;
            }
            if (selTPLValue == 2) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1270));
                return 1270;
            }
            if (selTPLValue == 3) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1721));
                return 1721;
            }
            if (selTPLValue == 4) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(2242));
                return 2242;
            }
        } else if (selCompulsoryIndex == 1) {
            if (selTPLValue == 0) {
                jQuery("#lblTPL").html("659");
                return 659;
            }
            if (selTPLValue == 1) {
                jQuery("#lblTPL").html("928");
                return 928;
            }
            if (selTPLValue == 2) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1131));
                return 1131;
            }
            if (selTPLValue == 3) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1507));
                return 1507;
            }
            if (selTPLValue == 4) {
                SetSpanValueByBrowerType('lblTPL', formatCurrency(1963));
                return 1963;
            }
        }
    }
    else {
        jQuery('#lblTPL').html("0");
    }
}

//车辆损失险
function calcCarDamage() {
    if (jQuery('#chkCarDamage').prop("checked") ) {
        var seatNum = document.getElementById("hidSeatNum").value;
        var rate = 0.0095;
        var baseCost = 285;
        if (seatNum >= 6 && seatNum < 10) {
            rate = 0.009;
            baseCost = 342;
        }
        else if (seatNum >= 10 && seatNum < 20) {
            rate = 0.0095;
            baseCost = 342;
        }
        else if (seatNum >= 20) {
            rate = 0.0095;
            baseCost = 357;
        }
        var result = Math.round(jQuery('#txtMoney').val() * rate + baseCost);
        SetSpanValueByBrowerType('lblCarDamage', formatCurrency(result));
    }
    else {
        jQuery('#lblCarDamage').html("0");
    }
}

//全车盗抢险
function calcCarTheft() {
    if ($('#chkCarTheft').prop("checked") && $('#chkCarDamage').prop("checked")) {
        var selCompulsoryIndex = document.getElementById("selCompulsory").selectedIndex;
        if (selCompulsoryIndex == 1)
            SetSpanValueByBrowerType('lblCarTheft', formatCurrency(Math.round($('#txtMoney').val() * 0.0044 + 140)));
        else
            SetSpanValueByBrowerType('lblCarTheft', formatCurrency(Math.round($('#txtMoney').val() * 0.0049 + 120)));
    }
}

//玻璃单独破碎险     
function calcBreakageOfGlass() {
    if ($('#chkBreakageOfGlass').prop("checked")) {
        var breakageOfGlassValue = document.getElementById("selBreakageOfGlass").value;
        var selCompulsoryIndex = document.getElementById("selCompulsory").selectedIndex;
        if (breakageOfGlassValue == 0)//进口
            if (selCompulsoryIndex == 1) { //6-10座客车
                SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.003)));
            } else {
                SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.0031)));
            }
        if (breakageOfGlassValue == 1)//国产
            SetSpanValueByBrowerType('lblBreakageOfGlass', formatCurrency(Math.round($('#txtMoney').val() * 0.0019)));
    } else {
        jQuery("#lblBreakageOfGlass").html("0");
    }
}

function calcSelfignite() {
    if ($('#chkSelfignite').prop("checked")) {
        SetSpanValueByBrowerType('lblSelfignite', formatCurrency(Math.round($('#txtMoney').val() * 0.0015)));
    }
    else {
        jQuery('#lblSelfignite').html("0");
    }
}

function calcAbatement() {
    if ($('#chkCarDamage').prop("checked") && $('#chkTPL').prop("checked") && $('#chkAbatement').prop("checked")) {
        var total = GetIntValue(jQuery("#lblCarDamage").html()) + GetIntValue($("#lblTPL").html());
        SetSpanValueByBrowerType('lblAbatement', formatCurrency(Math.round(total * 0.2)));
    }
    else {
        $('#chkAbatement').attr("checked", false);
        jQuery("#lblAbatement").html("0");
    }
}


//乘客责任险（算法参见计算器需求文档）
function calcLimitofPassenger() {
    var seatNum = document.getElementById("hidSeatNum").value;
    if (seatNum < 4) {
        seatNum = 4;
    }
    var calCount = seatNum - 1;
    if (jQuery('#chkLimitofPassenger').prop("checked")) {
        if (jQuery('#selCompulsory').prop('selectedIndex') == 0) { //6座以下
            var lvalue1 = Math.round(jQuery("#selLimitofPassenger option:selected").val() * 0.0027 * calCount);
            jQuery("#lblLimitOfPassenger").html(lvalue1);
        } else {
            var lvalue2 = Math.round(jQuery("#selLimitofPassenger option:selected").val() * 0.0026 * calCount);
            jQuery("#lblLimitOfPassenger").html(lvalue2);
        }
    } else {
        jQuery("#lblLimitOfPassenger").html("0");
    }
}

//车身划痕险
function calcCarDamageDW() {
    if ($('#chkCarDamage').prop("checked") && $('#chkCarDamageDW').prop("checked")) {
        var selCarDamageDWIndex = document.getElementById("selCarDamageDW").selectedIndex;
        if ($('#txtMoney').val() < 300000) {
            if (selCarDamageDWIndex == 0)
                jQuery('#lblCarDamageDW').html("400");
            if (selCarDamageDWIndex == 1)
                jQuery('#lblCarDamageDW').html("570");
            if (selCarDamageDWIndex == 2)
                jQuery('#lblCarDamageDW').html("760");
            if (selCarDamageDWIndex == 3)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1140));
        } else if ($('#txtMoney').val() > 500000) {
            if (selCarDamageDWIndex == 0)
                jQuery('#lblCarDamageDW').html("850");
            if (selCarDamageDWIndex == 1)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1100));
            if (selCarDamageDWIndex == 2)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1500));
            if (selCarDamageDWIndex == 3)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(2250));
        } else {
            if (selCarDamageDWIndex == 0)
                jQuery('#lblCarDamageDW').html("585");
            if (selCarDamageDWIndex == 1)
                jQuery('#lblCarDamageDW').html("900");
            if (selCarDamageDWIndex == 2)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1170));
            if (selCarDamageDWIndex == 3)
                SetSpanValueByBrowerType('lblCarDamageDW', formatCurrency(1780));
        }
    } else {
        $('#chkCarDamageDW').attr("checked", false);
        jQuery('#lblCarDamageDW').html("0");
    }
}

//司机责任险
function calcLimitofDriverNew() {
    if ($('#chkLimitofDriver').prop("checked")) {
        if (jQuery('#selCompulsory').prop('selectedIndex') == 0) { //6座以下
            //所选金额*费率*（座位数-1）。如果没有座位数，则*4
            var lvalue1 = Math.round(jQuery("#selLimitofDriver option:selected").val() * 0.0042);
            jQuery("#lblLimitOfDriver").html(lvalue1);
        } else {
            var lvalue2 = Math.round(jQuery("#selLimitofDriver option:selected").val() * 0.004);
            jQuery("#lblLimitOfDriver").html(lvalue2);
        }
    } else {
        jQuery("#lblLimitOfDriver").html("0");
    }
}

//=========弹出提示=============================
var preText = null;
function showjs(j) {
    if (preText)
        preText[0].style.display = 'none';
    preText = $("#" + j);
    $("#" + j)[0].style.display = '';
}
function closex(t) { $("#" + t)[0].style.display = 'none'; }

//---------------------------------------旧的JS
function getSerialByMasterBrandID(id) 
{
    
    $("ddlChexing").options.length = 1;
    $("ddlChekuan").options.length = 1;
    
    $('txtMoney').value = 0;
    if(id == -1 || id == -2)
    {
        return;
    }
    
    var myCarTypeOptions={ 
        parameters:"bsid=" + id, 
        method:"get", 
        asynchronous: false,
        onSuccess:function(res){ 
            var carTypeData = eval("(" + res.responseText + ")");
             for (var i=0; i< carTypeData.length; i++)
            {
                var cartype = carTypeData[i];
                $("ddlChexing").options.add(new Option(cartype.Name, cartype.ID));      
            } 
                        
         //   showLoading("false");
        } 
    }
    
    new Ajax.Request("/car/ajaxnew/GetSerialByMasterBrand.ashx?type=json&s"+Math.random(), myCarTypeOptions);
}
       
function carModul(id, name, carreferprice)
{
    this.id = id;
    this.name = name;
    this.carreferprice = carreferprice;
}

//用作客户端保存车型数据
var cars = new Array();    
function getCarByCsID(id) 
{
   // showLoading("true");
    
    //清空缓存车型数据
    cars.length = 0;
    var ddlChekuan =  $("ddlChekuan")  
    var groupItem = ddlChekuan.firstChild;
    while(groupItem)
    {
		ddlChekuan.removeChild(groupItem);
		groupItem = ddlChekuan.firstChild;
    }
    var oItem =  document.createElement("OPTION");   
	oItem.setAttribute("value", -1);
	oItem.appendChild(document.createTextNode("选择车款"));            
	$("ddlChekuan").appendChild(oItem);
    
    
    $('txtMoney').value = 0;
    
    if(id == -1)
    {      
        return;
    }    
            
    var myoptions={ 
        parameters:"csid="+id, 
        method:"get", 
        asynchronous: false,
        onSuccess:function(res){ 
            var myData = eval("(" + res.responseText + ")"); 
            
            var yearList = new Object();
            yearList["YearList"] = new Array();
            for (var i=0; i< myData.length; i++)
            {
				var car = myData[i];
				if(!yearList[car.YearType])
				{
					yearList[car.YearType] = new Array();
					yearList["YearList"].push(car.YearType);
				}
					
				yearList[car.YearType].push(car);
            }
            for(var i=0;i<yearList["YearList"].length;i++)
            {
				var carYear = yearList["YearList"][i];
				var carsInYear = yearList[carYear];
				if(yearList["YearList"].length > 1)
				{
					var optionItem = document.createElement("OPTGROUP");
					optionItem.label = carYear + "款";
					optionItem.style.fontStyle="normal";
					optionItem.style.background="#CCCCCC";
					optionItem.style.textAlign="center";
					$("ddlChekuan").appendChild(optionItem);
				}
				for (var j=0; j< yearList[carYear].length; j++)
				{
					var car = yearList[carYear][j];
					var oItem =  document.createElement("OPTION");   
					oItem.setAttribute("value", car.ID);
					oItem.appendChild(document.createTextNode(car.Name));            
					$("ddlChekuan").appendChild(oItem);
					cars[cars.length] = new carModul(car.ID, car.Name, car.CarReferPrice);
				}
            }
                        
            //数组构建索引
            var carLength = cars.length;    
            for(var j = 0; j < carLength; j++)
            {
                cars[cars[j].id] = cars[j];
            } 
       //     showLoading("false");
    
        } 
    }
    
    new Ajax.Request("/car/ajaxnew/GetCarByCsID.aspx?type=json&s"+Math.random(),myoptions); 
}

/*  
*    ForDight(Dight,How):数值格式化函数，Dight要  
*    格式化的  数字，How要保留的小数位数。  
*/  
function ForDight(Dight,How)  
{  
   Dight  =  Math.round (Dight * Math.pow(10,How))/Math.pow(10,How);  
   return  Dight;  
}   

//-----------------------------------------

//层 隐藏显示
function showOrHideDiv(imgID){
    var imgClose = document.getElementById('imgClose');
    var imgOpen = document.getElementById('imgOpen');
    
    if(imgClose && imgOpen){
        var showDivs = false;
        
        if('imgClose'== imgID){
            imgClose.style.display  = 'none';
            imgOpen.style.display  = '';
            showDivs = false;            
        }
        if('imgOpen'== imgID){
            imgClose.style.display  = '';
            imgOpen.style.display  = 'none';
            showDivs = true;            
        }
        
        for(var i = 0; i < 9; i++)
        {
            var divCommonTotals = document.getElementById('divCommonTotals' + i);
            
            divCommonTotals.style.display = "none";
            if(showDivs == true)
                divCommonTotals.style.display = "";
        }
    }
}

function setSelected(oSel, val)
{
	if(val != "0" || val != "")
		oSel.value = val;
}