//卡片区 车系区域报价
function GetSerialAreaPriceRange() {
    $.ajax({
        url: "http://frontapi.easypass.cn/ReferPriceAPI/GetReferPriceByCityCSFront/" + GlobalSummaryConfig.CityId + "/" + GlobalSummaryConfig.SerialId + "",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetSerialAreaPriceRangeCallback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
                var result;
                var minPrice = parseFloat(data[0].MinReferPrice);
                var maxPrice = parseFloat(data[0].MaxReferPrice);
                if (minPrice <= 0 && maxPrice <= 0) {
                    return;
                }
                if (minPrice >= 100) {
                    minPrice = minPrice.toFixed(0);
                }
                else {
                    minPrice = minPrice.toFixed(2);
                }
                if (maxPrice >= 100) {
                    maxPrice = maxPrice.toFixed(0);
                }
                else {
                    maxPrice = maxPrice.toFixed(2);
                }
                if (minPrice == maxPrice) {
                    result = minPrice + "万";
                }
                else {
                    result = minPrice + "-" + maxPrice + "万";
                }               
                if (data[0].ReturnType == 1) {
                    $("#cs-area-price").html(result);
                    $("#cs-area-name").html($("#cs-area-name").html().replace("全国参考价",GlobalSummaryConfig.CityName + "参考价"));
                }
                else if (data[0].ReturnType == 2) {
                    $("#cs-area-price").html(result);
                }
            }
        }
    });
}
//车款列表设置区域报价
function GetCarAreaPriceList(ids) {
    var cityId;
    if (typeof (bit_locationInfo) != "undefined") {
        cityId = bit_locationInfo.cityId;
    }
    if (typeof cityId == "undefined" || cityId == null || cityId == "") {
        return;
    }
    $.ajax({
        url: "http://frontapi.easypass.cn/ReferPriceAPI/GetReferPriceByCityCarFront/" + cityId + "/" + ids + "",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetCarAreaPriceListCallback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
                var isLocal = false;
                var div = GetTargetDiv();
                for (var i = 0; i < data.length; i++) {
                    var minPrice = parseFloat(data[i].MinReferPrice);
                    if (minPrice <= 0) {
                        continue;
                    }
                    if (minPrice >= 100) {
                        minPrice = minPrice.toFixed(0);
                    }
                    else {
                        minPrice = minPrice.toFixed(2);
                    }
                    minPrice = minPrice + "万";  
                    
                    if (typeof (div) != "undefined" && div != null){
                        if (div.find("#carlist_" + data[i].Id).children(":eq(1)").length > 0) {
                            div.find("#carlist_" + data[i].Id).children(":eq(1)").children(":eq(0)").html(minPrice);
                        }
                    }                   
                }
            }
        }
    });
}
function GetGroupPriceDec(list, dec) {
    list.each(function (i) {
        var element = $(this);
        if (typeof (element.children(":eq(4)")) != "undefined") {
            element.children(":eq(4)").html(dec);
        }
    });
}
function GetCarAreaPriceRange() {
    var styleIds = GetStyleIds();
    if (styleIds == null || styleIds == "") {
        return;
    }
    var result = GroupIds(styleIds, 30);
    for (var i = 0; i < result.length; i++)  {
        var ids = result[i].join(',');
        GetCarAreaPriceList(ids);
    }
}
//获取车款ID
function GetStyleIds() {
    var ids = new Array();
    var div = GetTargetDiv();
    if (typeof (div) != "undefined" && div != null)
        div.find("a[id ^= 'carlist']").each(function (i) {
        if (typeof (this.id) != "undefined" && this.id) {
            var id = this.id.replace("carlist_", "");
            ids.push(id);
        }
    });     
    return ids;
}
//获取当前年款内容块
function GetTargetDiv()
{
    var $targetDiv;
    $("#yeartag li").each(function (k, item) {
        if ($(item).attr("id") != "nosalelist" && $(item).attr("class") == "current")    //当前TAB 非停售车款
        {
            $targetDiv = $("#yearDiv" + k);
        }
    });
    return $targetDiv;    
}
function GroupIds(array, subGroupLength) {
    var index = 0;
    var newArray = [];
    while (index < array.length) {
        newArray.push(array.slice(index, index += subGroupLength));
    }
    return newArray;
}

//卡片区 车款区域报价
function GetStyleAreaPriceRange(styleId,isPeizhi) {
    var cityId, cityName;
    if (typeof (bit_locationInfo) != "undefined") {
        cityId = bit_locationInfo.cityId;
        cityName = bit_locationInfo.cityName;
    }
    if (typeof cityId == "undefined" || cityId == null || cityId == "") {
        return;
    }
    if (typeof styleId == "undefined" ||styleId == null || styleId == "" || styleId <= 0) {
        return;
    }
    $.ajax({
        url: "http://frontapi.easypass.cn/ReferPriceAPI/GetReferPriceByCityCarFront/" + cityId + "/" + styleId + "",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetStyleAreaPriceRangeCallback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
                var result;
                var minPrice = parseFloat(data[0].MinReferPrice);
                var maxPrice = parseFloat(data[0].MaxReferPrice);
                if (minPrice <= 0 && maxPrice <= 0) {
                    return;
                }
                if (minPrice >= 100) {
                    minPrice = minPrice.toFixed(0);
                }
                else {
                    minPrice = minPrice.toFixed(2);
                }
                if (maxPrice >= 100) {
                    maxPrice = maxPrice.toFixed(0);
                }
                else {
                    maxPrice = maxPrice.toFixed(2);
                }
                if (minPrice == maxPrice) {
                    result = minPrice + "万";
                }
                else {
                    result = minPrice + "-" + maxPrice + "万";
                }
                if (data[0].ReturnType == 1) {
                    if (isPeizhi) {
                        $("#car-area-priceitem").eq(0).html($("#car-area-priceitem").eq(0).html().replace("商家报价", cityName + "参考价"));
                        $("#car-area-priceitem").next().eq(0).html(result);
                    }
                    else {
                        $("#car-area-price").html(result);
                        if (typeof cityName != "undefined" && cityName != "") {
                            $("#car-area-name").html($("#car-area-name").html().replace("全国", cityName));
                        }
                    }
                }
                else if (data[0].ReturnType == 2) {
                    if (isPeizhi) {
                        $("#car-area-priceitem").eq(0).html($("#car-area-priceitem").eq(0).html().replace("商家报价", cityName + "全国参考价"));  
                        $("#car-area-priceitem").next().eq(0).html(result);
                    }
                    else {
                        $("#car-area-price").html(result);
                    }
                }
            }
        }
    });
}
//树形页卡片区 车系年款页  车系区域报价
function GetSerialTreeAreaPriceRange(csId) {
    var cityId, cityName;
    if (typeof (bit_locationInfo) != "undefined") {
        cityId = bit_locationInfo.cityId;
        cityName = bit_locationInfo.cityName;
    }
    if (typeof cityId == "undefined" || cityId == null || cityId == "") {
        return;
    }
    if (typeof csId == "undefined" || csId == null || csId == "" || csId<= 0) {
        return;
    }
    $.ajax({
        url: "http://frontapi.easypass.cn/ReferPriceAPI/GetReferPriceByCityCSFront/" + cityId + "/" + csId+ "",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetSerialTreeAreaPriceRangeCallback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
                var result;
                var minPrice = parseFloat(data[0].MinReferPrice);
                var maxPrice = parseFloat(data[0].MaxReferPrice);
                if (minPrice <= 0 && maxPrice <= 0) {
                    return;
                }
                if (minPrice >= 100) {
                    minPrice = minPrice.toFixed(0);
                }
                else {
                    minPrice = minPrice.toFixed(2);
                }
                if (maxPrice >= 100) {
                    maxPrice = maxPrice.toFixed(0);
                }
                else {
                    maxPrice = maxPrice.toFixed(2);
                }
                if (minPrice == maxPrice) {
                    result = minPrice + "万";
                }
                else {
                    result = minPrice + "-" + maxPrice + "万";
                }
                if (data[0].ReturnType == 1) {
                    $("#cs-area-price").children(":eq(0)").children(":eq(0)").html(result);
                    if (typeof cityName != "undefined" && cityName != "") {
                        $("#cs-area-price").html($("#cs-area-price").html().replace("参考成交价", cityName+"参考价"));
                    }
                }
                else if (data[0].ReturnType == 2) {
                    $("#cs-area-price").children(":eq(0)").children(":eq(0)").html(result);
                    if (typeof cityName != "undefined" && cityName != "") {
                        $("#cs-area-price").html($("#cs-area-price").html().replace("参考成交价", "全国参考价"));
                    }
                }
            }
        }
    });
}

//车系参数配置页
function GetCarAreaPriceForCSCompare(carIds) {
    var result = GroupIds(carIds, 30);
    for (var i = 0; i < result.length; i++) {
        var ids = result[i].join(',');
        GetCarAreaPriceListForCSCompare(ids);
    }
}
function GetCarAreaPriceListForCSCompare(ids) {
    var cityId, cityName;
    if (typeof (bit_locationInfo) != "undefined") {
        cityId = bit_locationInfo.cityId;
        cityName = bit_locationInfo.cityName;
    }
    if (typeof cityId == "undefined" || cityId == null || cityId == "") {
        return;
    }
    if (typeof ids == "undefined" || ids == null || ids == "") {
        return;
    }
    $.ajax({
        url: "http://frontapi.easypass.cn/ReferPriceAPI/GetReferPriceByCityCarFront/" + cityId + "/" + ids + "",
        cache: true,
        dataType: "jsonp",
        jsonpCallback: "GetCarAreaPriceListForCSCompareCallback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
                var isLocal = false;
                for (var i = 0; i < data.length; i++) {
                    var minPrice = parseFloat(data[i].MinReferPrice);
                    if (minPrice <= 0) {
                        continue;
                    }
                    if (minPrice >= 100) {
                        minPrice = minPrice.toFixed(0);
                    }
                    else {
                        minPrice = minPrice.toFixed(2);
                    }
                    minPrice = minPrice + "万"; 
                    if (data[i].ReturnType == 1) { isLocal = true; }
                    $("#car_aera_" + data[i].Id).html(minPrice);
                }
                if (isLocal == true) {
                    $("#car_aera_name").eq(0).html($("#car_aera_name").eq(0).html().replace("商家报价", cityName + "参考价"));                   
                }
                else {
                    $("#car_aera_name").eq(0).html($("#car_aera_name").eq(0).html().replace("商家报价", "全国参考价"));
                }
            }
        }
    });
}
