define(["anchors", "jquery", "jquerytmpl", "jqueryfullpage"], function (anchors,$) {

    return {

        //外观设计
        waiguansheji: function (name) {
            //var name = "page3";
            var index = anchors.Auchors.indexOf(name);
            $.get("/handlers/GetImageList.ashx?csid=1765&", function (data) {
                $("div[data-anchor='" + name + "']").html(data);
                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        },

        //评测导购
        pingcedaogou: function (name) {
            var index = anchors.Auchors.indexOf(name);
            $.get("/handlers/GetNewsList.ashx?top=10&csid=1765&", function (data) {
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
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
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
        },

        //亮点配置
        liangdianpeizhi: function (name) {
            var index = anchors.Auchors.indexOf(name);
            $.get("/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&", function(data) {
                $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");

                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (anchors.Auchors[anchors.Auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            });
        }
        

    };

})