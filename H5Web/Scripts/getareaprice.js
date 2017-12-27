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
        jsonpCallback: "GetCarAreaPriceListForH5Callback",
        success: function (data) {
            if (data != null && typeof data != "undefined" && data.length > 0) {
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
                    
                    if ($("#slide-carlist-" + data[i].Id).find(".now").length > 0) {
                        $("#slide-carlist-" + data[i].Id).find(".now").html(minPrice);
                    }
                }
            }
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
    $("li[id ^= 'slide-carlist-']").each(function (i) {
        if (typeof (this.id) != "undefined" && this.id) {
            var id = this.id.replace("slide-carlist-", "");
            ids.push(id);
        }
    })
    return ids;
}
function GroupIds(array, subGroupLength) {
    var index = 0;
    var newArray = [];
    while (index < array.length) {
        newArray.push(array.slice(index, index += subGroupLength));
    }
    return newArray;
}


