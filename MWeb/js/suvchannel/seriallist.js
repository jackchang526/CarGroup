function GetSUVList(channelId) {
    this.url = "/handlers/SUVChannel.ashx";
    this.sort = 0;
    this.channelId = channelId;
    this.tagClass = ["bg-green", "bg-blue", "bg-org"];
    this.InitEvent();
    //this.GetData();
}

GetSUVList.prototype.InitEvent = function () {
    var _this = this;
    $("#sort-box a").click(function (e) {
        e.preventDefault();
        $(this).parent().siblings().removeClass("current");
        $(this).parent().addClass("current");
        _this.sort = $(this).attr("sort");
        _this.GetData();
    });
}

GetSUVList.prototype.GetData = function () {
    var _this = this;
    var reqUrl = _this.url;
    var tagClass = _this.tagClass;
    $.ajax({
        type: "get",
        url: reqUrl,
        data: { channelId : _this.channelId,sort: _this.sort },
        dataType: "json",
        success: function (data) {
            if (data) {
                if (data.msg != "") {
                    console.log(data.msg);
                    return;
                }
                var tagHtmlArray = new Array();
                var tagObj = data.result.tag;
                if (tagObj != undefined && tagObj.length > 0) {
                    for (var i = 0; i < tagObj.length; i++) {
                        if (i > tagClass.length) break;
                        tagHtmlArray.push("<li class=\"" + tagClass[i] + "\"><strong>" + tagObj[i].title + "</strong><span>" + tagObj[i].content + "</span></li>");
                    }
                    $("#tag-box").html(tagHtmlArray.join(""));
                }

                var serialHtmlArray = new Array();
                var serialObj = data.result.serial;
                if (serialObj != undefined && serialObj.length > 0) {
                    for (var i = 0; i < serialObj.length; i++) {
                        serialHtmlArray.push("<li>");
                        serialHtmlArray.push("    <a href=\"/" + serialObj[i].allSpell + "/\" class=\"car\">");
                        serialHtmlArray.push("        <img src=\"" + serialObj[i].imgUrl + "\">");
                        serialHtmlArray.push("        <strong>" + serialObj[i].csName + "</strong>");
                        serialHtmlArray.push("        <p><em>" + serialObj[i].referPrice + "</em></p>");
                        serialHtmlArray.push("    </a>");
                        serialHtmlArray.push("</li>");
                    }
                    $("#serial-box").html(serialHtmlArray.join(""));
                }
            }
        },
        error: function (msg) {
            console.log(msg);
        }
    });
}