var requestCount = 0;
var timeout = Config.timeout;
var chezhuka = {
    resetSlides: function(name) {
        //var index = Config.auchors.indexOf(name);
        //$("#fullpage").fullpage.resetSlides(index);
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    },
    removePage: function (name) {
        $("div[data-anchor='" + name + "']").remove();
        $(".indexmenu a[href='#" + name + "']").parent().remove();
        var index = Config.auchors.indexOf(name);
        Config.auchors.splice(index, 1);
    },
    //图片和视频
    waiguansheji: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetImageAndVideoData.ashx?csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data !== "" && data != null) {
                    $("#picvideotmpl").tmpl({ "data": data, "position": { "6": "bd1", "7": "bd2", "8": "bd3" } }).appendTo("div[data-anchor='" + name + "']");
                    $(".indexmenu a[href='#" + name + "']").parent().show();
                    chezhuka.resetSlides(name);
                } else {
                    chezhuka.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                    $("#video-pic-wall img").each(function() {
                        $(this).height($(this).width() / 1.7857142857);
                    });
                    break;
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                chezhuka.removePage(name);
            }
        });
    },

    //评测导购
    pingcedaogou: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetNewsList.ashx?top=10&csid=" + Config.serialId + "&v=" + Config.version + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                var length = data.length;
                var obj = {};
                obj["isCustomization"] = false; //是否为定制版
                if (Config.currentCustomizationType !== "User" || Config.isAd === 0) {
                    obj["isCustomization"] = true;
                }
                if (length > 0) {
                    var listgroup = [];
                    listgroup.push(data.slice(0, 3));
                    if (length > 3) {
                        if (obj["isCustomization"] === true) {
                            listgroup.push(data.slice(3, 6));
                        } else {
                            listgroup.push(data.slice(3, 5));
                        }
                    }

                    if (length > 6) {
                        if (obj["isCustomization"] === true) {
                            listgroup.push(data.slice(6, 9));
                        } else {
                            listgroup.push(data.slice(6));
                        }
                    }

                    obj["listgroup"] = listgroup;
                    $("#pingcetmp20160413").tmpl(obj).appendTo("div[data-anchor='" + name + "']");

                    $(".indexmenu a[href='#" + name + "']").parent().show();
                    chezhuka.resetSlides(name);
                } else {
                    chezhuka.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                chezhuka.removePage(name);
            }
        });
    },

    //亮点配置
    liangdianpeizhi: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data !== "" && data != null && data.length > 0) {
                    $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");
                    $(".indexmenu a[href='#" + name + "']").parent().show();
                    chezhuka.resetSlides(name);
                } else {
                    chezhuka.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                chezhuka.removePage(name);
            }
        });
    },

    //热门点评
    remendianping: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&v=" + Config.version + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data !== "" && data != null) {
                    $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='" + name + "']");
                    $(".indexmenu a[href='#" + name + "']").parent().show();
                    chezhuka.resetSlides(name);
                } else {
                    chezhuka.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                chezhuka.removePage(name);
            }
        });
    },

    //初始化页面
    initFullPage: function() {
        var $fullpage = $("#fullpage");
        var settimeForHide = 0;
        $fullpage.fullpage({
            anchors: Config.auchors,
            menu: "#menu",
            css3: true,
            normalScrollElementTouchThreshold: 5,
            scrollingSpeed: 700,
            animateAnchor: false,
            controlArrows: false,
            verticalCentered: false,
            loopHorizontal: false,
            fixedElements: ".fixed_box, .menu, .menu_box, .menu_box_bg ",
            slidesNavigation: true,
            slidesNavPosition: "bottom",
            afterSlideLoad: function(anchorLink, index, slideIndex, direction) {
                /*star 侧滑统计(分页)*/
                if (slideIndex.length > 0 && slideIndex.indexOf("-") > 0) {
                    var slideindex = slideIndex.split("-")[1];
                    if (slideindex && slideindex > 1) {
                        try {
                            bglogexec(); //统计数据方法 2015-10-12
                        } catch (e) {
                        }
                    }
                }
                /*end 侧滑统计*/
            },
            afterLoad: function(anchorLink, index) {
                var logowrapper = $("#logo");
                if (anchorLink === "page1" || anchorLink === "page2") {
                    logowrapper.fadeIn(); //淡出
                } else {
                    logowrapper.fadeOut(); //淡入
                }

                /*star 页面统计*/
                try {
                    bglogexec(); //统计数据方法 2015-10-09
                } catch (e) {
                }
                /*end 页面统计*/

                $("." + anchorLink).find("img[data-src]").each(function() {
                    $(this).attr("src", $(this).data("src"));
                    $(this).removeAttr("data-src");
                });

                /*star 分享*/
                //if (isMicroMessager) { //解决只在微信里弹出分享提示
                //    if (settimeForHide !== -1) {
                //        clearTimeout(settimeForHide);
                //        settimeForHide = setTimeout(function() {
                //            $("#sharefloat").show(800);
                //            setTimeout(function() { $("#sharefloat").hide(800); }, 4000);
                //            settimeForHide = -1;
                //        }, 3000);
                //    };
                //}
                /*end 分享*/
            },
            onLeave: function(index, nextIndex, direction) {
                var logowrapper = $(".fixed_box");
                if (Config.auchors.indexOf("page1") === (nextIndex - 1) || Config.auchors.indexOf("page2") === (nextIndex - 1)) {
                    logowrapper.fadeIn(); //淡出
                } else {
                    logowrapper.fadeOut(); //淡入
                }
            },
            afterRender: function() {
                $(".fixed_box").hide();
            }
        });
    }
};

//chezhuka.initFullPage();

chezhuka.waiguansheji("page3");
chezhuka.pingcedaogou("page4");
chezhuka.liangdianpeizhi("page5");
chezhuka.remendianping("page6");

var timeFun = setInterval(function() {
    if (requestCount === 4) {
        clearInterval(timeFun);
        $(".indexmenu").show();

        chezhuka.initFullPage();
    }
}, 5);