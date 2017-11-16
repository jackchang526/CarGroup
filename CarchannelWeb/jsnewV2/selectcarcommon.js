function GetNewCarText(serialIds) {
    if (serialIds == "") return;
    var toUrl = "http://api.car.bitauto.com/carinfo/GetCarIntoMarketText.ashx";
    $.ajax({
        url: toUrl,
        cache: true,
        data: { csids: serialIds, isshowdate: 0, type:"serial" },
        dataType: 'jsonp',
        jsonpCallback: "GetNewCarTextCallback",
        success: function (data) {
            if (typeof data == "undefined" || data.length == 0) return;
            var container = $("#divContent");
            for (var i = 0; i < data.length; i++) {
                var imgObj = $(container).find("div[data-id='" + data[i].csid + "'] .img img");
                if ($(imgObj).length > 0) {
                    if (data[i].text == "即将上市"){
                        $(imgObj).before("<span class=\"spl-label type2\">即将上市</span>");
                    }
                    else {
                        $(imgObj).before("<span class=\"spl-label type1\">" + data[i].text + "</span>");
                    }
                }
            }
        }
    });
}