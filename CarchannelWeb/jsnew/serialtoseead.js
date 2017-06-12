if (typeof (SerialAdpositionContentInfo) != 'undefined' && SerialAdpositionContentInfo[serialId]) {//有广告，有看了又看
    var csIds = "";
    var adInfo = SerialAdpositionContentInfo[serialId];

    var tempSerialToSeeData;
    if (typeof (serialToSeeJson) != 'undefined' && serialToSeeJson.length > 0) {
        tempSerialToSeeData = serialToSeeJson;
        for (var i = 0; i < serialToSeeJson.length; i++) {
            for (var j = 0; j < adInfo.length; j++) {
                if (adInfo[j] && serialToSeeJson[i].serialId == adInfo[j].SerialID) {
                    tempSerialToSeeData.splice(i, 1);
                }
            }
        }
    }
    else {
        tempSerialToSeeData = [];
    }
    serialToSeeJson = tempSerialToSeeData;

    for (var i = 0; i < adInfo.length; i++) {
        if (adInfo[i]) {
            csIds += adInfo[i].SerialID + ",";
        }
    }
    if (csIds != "") {
        csIds = csIds.substr(0, csIds.length - 1);
        $.ajax({
            url: "http://api.car.bitauto.com/carinfo/GetSerialInfoByCsIds.ashx",
            cache: true,
            dataType: "jsonp",
            data: { "csids": csIds },
            jsonpCallback: "callback",
            success: function (data) {
                if (data) {
                    if (serialToSeeJson.length < 5) {
                        for (var i = 0; i < adInfo.length; i++) {
                            if (adInfo[i] && data[adInfo[i].SerialID]) {
                                var obj = {};
                                obj.showName = (!adInfo[i] || !adInfo[i].Text || adInfo[i].Text == "") ? data[adInfo[i].SerialID].showName : adInfo[i].Text;
                                obj.Link = (!adInfo[i] || !adInfo[i].Link || adInfo[i].Link == "") ? ("http://car.bitauto.com/" + data[adInfo[i].SerialID].allSpell + "/") : adInfo[i].Link;
                                obj.price = data[adInfo[i].SerialID].price;
                                obj.img = (!adInfo[i] || !adInfo[i].Image || adInfo[i].Image == "") ? data[adInfo[i].SerialID].img : adInfo[i].Image;
                                obj.serialId = adInfo[i].SerialID;
                                serialToSeeJson.push(obj);
                            }
                        }

                    }
                    else {
                        for (var i = 0; i < adInfo.length; i++) {
                            if (adInfo[i] && data[adInfo[i].SerialID]) {
                                var obj = {};
                                obj.showName = (!adInfo[i] || !adInfo[i].Text || adInfo[i].Text == "") ? data[adInfo[i].SerialID].showName : adInfo[i].Text;
                                obj.Link = (!adInfo[i] || !adInfo[i].Link || adInfo[i].Link == "") ? ("http://car.bitauto.com/" + data[adInfo[i].SerialID].allSpell + "/") : adInfo[i].Link;
                                obj.price = data[adInfo[i].SerialID].price;
                                obj.img = (!adInfo[i] || !adInfo[i].Image || adInfo[i].Image == "") ? data[adInfo[i].SerialID].img : adInfo[i].Image;
                                obj.serialId = adInfo[i].SerialID;
                                serialToSeeJson.splice(5 + i - 1, 0, obj);
                            }
                        }
                    }

                }
                ShowSerialToSeeHtml();
            }
        });
    }
    else {
        ShowSerialToSeeHtml();
    }
}
if (serialToSeeJson && serialToSeeJson.length > 0) {//有看了又看，没有广告
    ShowSerialToSeeHtml();
}
function ShowSerialToSeeHtml() {
    if (serialToSeeJson && serialToSeeJson.length > 0) {
        var serialToSeeHtmlArray = new Array();
        for (var i = 0; i < serialToSeeJson.length; i++) {
            if (i > 5) break;
            var obj = serialToSeeJson[i];
            var url = obj.Link ? obj.Link : ("/" + obj.allSpell + "/");
            serialToSeeHtmlArray.push("<li>");
            serialToSeeHtmlArray.push("<a href=\"" + url + "\" target=\"_blank\" data-channelidflag=\"1\" onclick=\"BglogPostLog('2.21.832');\">");
            serialToSeeHtmlArray.push("<img src=\"" + obj.img + "\"  width=\"90\" height=\"60\">");
            serialToSeeHtmlArray.push("</a>");
            serialToSeeHtmlArray.push("<p><a href=\"" + url + "\" target=\"_blank\" data-channelidflag=\"1\" onclick=\"BglogPostLog('2.21.832');\">" + obj.showName + "</a></p>");
            serialToSeeHtmlArray.push("<p><span>" + obj.price + "</span></p>");
            serialToSeeHtmlArray.push("</li>");
        }
        $("#serialtosee_box > ul").html(serialToSeeHtmlArray.join(""));
    }
    else {
        $("#serialtosee_box").hide();
    }
}