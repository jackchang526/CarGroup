//图片
function getImageList() {
    var name = "page3";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetImageList.ashx?csid=" + Config.serialId + "&", function(data) {
        $("div[data-anchor='" + name + "']").html(data);

        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    });
}

//获取评测导购新闻
function getPingceNews() {
    var name = "page4";
    var index = Config.auchors.indexOf(name);
    if (index < 0) {
        $("div[data-anchor='" + name + "']").remove();
        return;
    }
    $.get("/handlers/GetNewsList.ashx?top=10&csid=" + Config.serialId + "&", function(data) {

        var length = data.length;
        var obj = {};

        obj["isCustomization"] = false; //是否为定制版
        if (Config.currentCustomizationType !== "User" || Config.isAd === 0) {
            obj["isCustomization"] = true;
        }

        if (length > 0) {
            if (length >= 3)
                obj["isSecondAd"] = true;
            else
                obj["isSecondAd"] = false;

            var listgroup = [];
            listgroup.push(data.slice(0, 3));
            if (length > 3) {
                listgroup.push(data.slice(3, 6));
            }

            if (length > 6) {
                if (obj["isCustomization"] === true) {
                    listgroup.push(data.slice(6, 9));
                } else {
                    listgroup.push(data.slice(6));
                }
            }

            obj["listgroup"] = listgroup;
        } else {
            obj["listgroup"] = [];
        }

        $("#pingcenewstmp").tmpl(obj).appendTo("div[data-anchor='" + name + "']");

        $("#fullpage").fullpage.resetSlides(index);
        //最后一页去掉向下箭头
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }

        if (obj["isCustomization"] === false) {
            //非定制版加广告
            $("#div_29e5455f-c705-4489-bdd8-f24f9607267a").appendTo("#addaogoufirst").show();
            $("#div_4d63ae1f-37bc-49e8-81c3-dd2fd040389a").appendTo("#addaogousecond").show();
        } else {
            //如果是定制版则移除广告标签
            $("#addaogoufirst").remove();
            $("#addaogousecond").remove();
        }
    });
}

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

getPingceNews();
getPeizhi();
getImageList();