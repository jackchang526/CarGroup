﻿function fillCarPart(data,type){
    if(typeof data == "undefined") return;
    var len=0,h=[];
    switch(type){
        case 1:
            if(data.isDouble11&&$("#s1-hui").length<=0){
                $("#s1-srclist").append("<dl id=\"s1-mall\" class=\"d11-price-h\"><dt>易车商城：</dt><dd><a href=\"" + data.WebSite + "\" target=\"_blank\">" + data.ShowTitle + "</a></dd></dl>");
                $("#s1-srclist").append("<span class=\"button_gray btn-d11\"><a href=\""+data.WebSite+"\" target=\"_blank\">立即去抢车</a></span>");
            }else if(data.isDouble11&&$("#s1-hui").length>0){
                $("#s1-hui").before("<dl id=\"s1-mall\" class=\"d11-price-h\"><dt>易车商城：</dt><dd><a href=\"" + data.WebSite + "\" target=\"_blank\">" + data.ShowTitle + "</a></dd></dl>");
            }
            break;
        case 2:
            if (data) {
                $("#s1-srclist").append("<dl id=\"s1-hui\" class=\"d11-price-h\"><dt>惠买车特惠价：</dt><dd><a href=\"" + data.link + "&tracker_u=120_gckhj\" target=\"_blank\">" + data.price + "万起</a></dd></dl>");
                $("#s1-srclist").append("<span class=\"button_gray btn-d11\"><a href=\"" + data.link + "&tracker_u=120_gckhj\" target=\"_blank\">立即去抢车</a></span>");
            }
            break;
        case 3:
            if (halfCarList[inlet_config.serialId]) {
                $("#s1-srclist > a").after("<dl id=\"s1-half\"  class=\"d11-price-h\"><dt>购车狂欢节：</dt><dd><a href=\"" + halfCarList[inlet_config.serialId].url + "\" target=\"_blank\">" + halfCarList[inlet_config.serialId].title + "</a></dd></dl>");
                $("#s1-srclist").append("<span class=\"button_gray btn-d11\"><a href=\"" + halfCarList[inlet_config.serialId].url + "\" target=\"_blank\">立即去抢车</a></span>");
            }
            break;
        default:break;
    }
    len=$("#s1-srclist .d11-price-h").length;
    if(len>2){
        $("#s1-srclist .d11-price-h").attr("class","d11-price-v").each(function(i,n){
            var str=$(this).find("dt").html();
            $(this).find("dt").html(str.replace("：",""));
        });
        $("#s1-srclist .btn-d11").hide();
    }else if(len==2){
        $("#s1-srclist .btn-d11").hide();
    }else if(len==1){
                       
    }
    if(len>0) $("#s1-container").show();
}
(function(){
    $.ajax({url:"http://ajax.huimaiche.com/GetCarBasicPrice.ashx?ccode="+bit_locationInfo.cityId+"&carid="+inlet_config.carId+"",cache:true,dataType:"jsonp",jsonpCallback:"shuiCallback",success:function(data){
        fillCarPart(data,2);
    }});
    $.ajax({
        url: "http://www.yichemall.com/double11/GetCarInfoById?carid=" + inlet_config.carId + "", cache: true, dataType: "jsonp", jsonpCallback: "smallCallback", success: function (data) {
        fillCarPart(data,1);
        }
    });
    $.ajax({
        url: "http://image.bitautoimg.com/carchannel/jsnew/halfprice_11.js?v=20141110", cache: true, dataType: "script", success: function (data) {
            fillCarPart("", 3);
        }
    });
})();