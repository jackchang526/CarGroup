
//亮点配置
function getPeizhi() {
    var name = "page5";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&", function(data) {
        $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");

        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    });
}

//图片和视频
function getImageAndVideo() {
    var name = "page3";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }

    $.ajax({
        type: "GET",
        url: "/handlers/GetImageAndVideoData.ashx?csid=" + Config.serialId + "&",
        success: function (data) {
            if (data !== "" && data != null) {
                $("#picvideotmpl").tmpl({ "data": data }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#nodatatmpl").tmpl({ title: "外观图片" }).appendTo("div[data-anchor='" + name + "']");
            }
            var index = Config.auchors.indexOf(name);
            $("#fullpage").fullpage.resetSlides(index);
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        },
        complete: function () {
            $("#video-pic-wall img").each(function () {
                $(this).height($(this).width() / 1.7857142857);
            });
        }
    });
}

getPeizhi();
getImageAndVideo();