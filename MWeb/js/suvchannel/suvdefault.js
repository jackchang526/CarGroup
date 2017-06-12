function GetData(requrl,callbackfunction,callback,obj) {
    $.ajax({
        url: requrl,
        cache: true,
        dataType: "jsonp",
        jsonpCallback: callback,
        success: function (data) {
            if (typeof callbackfunction == "function") {
                callbackfunction.call(obj, data);
                //callbackfunction(data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function CreateBaokuanHtml(data) {
    if (data && data.ResList && data.ResList.length > 0) {
        var htmlArray = new Array();
        for (var i = 0; i < data.ResList.length; i++) {
            var obj = data.ResList[i];
            htmlArray.push("<li  data-channelid=27.149." + (1406 + i) + ">");
            htmlArray.push("    <a href=\"/" + obj.AllSpell + "/\">");
            htmlArray.push("        <img src=\"" + obj.ImageUrl + "\">");
            htmlArray.push("        <p>" + obj.ShowName + "</p>");
            htmlArray.push("    </a>");
            htmlArray.push("</li>");
        }
        $("#baokuan-box").html(htmlArray.join(""));
        Bglog_InitPostLog();
    }
}

function GetBaoKuan() {
    var url = "http://select.car.yiche.com/selectcartoolv2/searchresult?l=8&pagesize=6&external=Carwareless";
    GetData(url, CreateBaokuanHtml, "baokuan");
}

function baokuan() {

}


function PaihangPingfen() {
    this.url = "http://select.car.yiche.com/selectcartoolv2/searchresult?l=8&pagesize=10&s={sort}&p={p}&external=Carwareless";
    this.pingfenurl = "http://api.car.bitauto.com/carinfo/GetSerialInfo.ashx?dept=getcskoubeibaseinfo&csids={csids}";
    this.sortJson = {
        "6": {"html":"<p class=\"car-score\">综合评分：<span>{point}</span>分</p>","data":["Rating"]},
        "24": { "html": "<p class=\"car-score\">油耗评分：<span>{point}</span>分</p>", "data": ["Desc","YouHao"] },
        "18": { "html": "<p class=\"car-score\">性价比评分：<span>{point}</span>分</p>", "data": ["Desc","XingJiaBi"] }
    };
    this.price = $("#bangdan-price .current a").attr("p");
    this.sort = $("#bangdan-sort .current a").attr("s");
}

PaihangPingfen.prototype.InitEvent = function () {
    var _this = this;
    $("#bangdan-price a").each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            _this.price = $(this).attr("p");
            $(this).parent().siblings().removeClass("current");
            $(this).parent().addClass("current");
            _this.GetData();
        });
    });

    $("#bangdan-sort a").each(function () {
        $(this).click(function (e) {
            e.preventDefault();
            _this.sort = $(this).attr("s");
            $(this).parent().siblings().removeClass("current");
            $(this).parent().addClass("current");
            _this.GetData();
        });
    });
}

PaihangPingfen.prototype.GetData = function () {
    var _this = this;
    var reqUrl = _this.url.replace("{sort}", _this.sort).replace("{p}", _this.price);
    $("#paihang-box").html("数据加载中...");
    GetData(reqUrl, _this.CreateHtml, "paihangp" + _this.price.replace("-","") + "s" + _this.sort,_this);
}

PaihangPingfen.prototype.CreateHtml = function (data) {
    var _this = this;
    if (data && data.ResList && data.ResList.length > 0) {

        var htmlArray = new Array();
        var csIdArray = new Array();
        for (var i = 0; i < data.ResList.length; i++) {
            var obj = data.ResList[i];
            csIdArray.push(obj.SerialId);
            htmlArray.push("<li csid=\"" + obj.SerialId + "\">");
            htmlArray.push("    <a href=\"/" + obj.AllSpell + "/\" class=\"car\">");
            htmlArray.push("        <img src=\"" + obj.ImageUrl.replace("_1.", "_3.") + "\">");
            htmlArray.push("        <strong>" + obj.ShowName + "</strong>");
            htmlArray.push("        <p><em>" + obj.PriceRange + "</em></p>");
            if (_this.sort == 0) {
                htmlArray.push("        <p class=\"car-score\"><p class=\"car-score\">一周关注度：第<span>" + (i + 1) + "</span>名</p></p>");
            }
            else {
                htmlArray.push("        <p class=\"car-score\">&nbsp;</p>");
            }
            htmlArray.push("    </a>");
            htmlArray.push("</li>");
        }
        
        $("#paihang-box").html(htmlArray.join(""));
        if (_this.sortJson[_this.sort] != undefined) {
            var pingfenurl = _this.pingfenurl.replace("{csids}", csIdArray.join(","));
            GetData(pingfenurl, _this.ShowPingfen, "showpingfenfun", _this);
        }
    }
}

PaihangPingfen.prototype.ShowPingfen = function (data) {
    var _this = this;
    if (data) {
        for (var csId in data) {
            var sortObj = _this.sortJson[_this.sort];
            var field = sortObj.data;
            var tempValue = data[csId];
            for (var i in field) {
                tempValue = tempValue[field[i]];
            }
            $("#paihang-box").find("li[csid='" + csId + "']").find(".car-score").html(sortObj.html.replace("{point}", tempValue));
        }
    }
}