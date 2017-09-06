var compareConfig = {
    serialid: apiSerialId
};
$(function () {
    WaitCompare.initCompreData(compareConfig);
    //RigthSwipeCity.initCity();

    //易团购
    //var serialShowName='<%=Ce.Serial.ShowName%>';
    var tuangouParam = 'mediaid=2&locationid=3&cmdId=' + apiSerialId + '&cityId=' + citycode;
    (function getYiTuanGou(params) {
        $.ajax({
            url: "http://api.market.bitauto.com/MessageInterface/YiTuanGou/GetYiTuanGouUrl.ashx?" + params,//?mediaid=2&locationid=1&cmdId=3999&cityId=201
            async: false,
            dataType: "jsonp",
            //jsonpCallback: "successHandler",
            //cache: true,
            success: function (data) {
                var h = [];
                if (data && data.result == "yes") {
                    var tuangouUrl = data.url;
                    var slogan = data.slogan;
                    var title = data.title;
                    var tuangouTag = data.tag;
                    h.push("<span class=\"yh-tap-txt\"><b>" + tuangouTag + "</b></span>");
                    h.push("<div class=\"cont-box\">");
                    h.push("<h4>" + title + "</h4>");
                    h.push("<p>" + slogan + "</p>");
                    h.push("</div>");

                    $(".prefer-tim").html(h.join(''));
                    $(".prefer-tim").on("click", function () {
                        window.location.href = tuangouUrl;
                        //window.open(tuangouUrl,"_blank");
                    });
                }
            }
        });
    })(tuangouParam)
});