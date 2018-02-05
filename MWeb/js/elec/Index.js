var GetNewsList = {
    category: { "xinwen": { cid: "", pageIndex: 1 }, "daogou": { cid: "637,638,639,640", pageIndex: 1 }, "pingce": { cid: "641,642,643", pageIndex: 1 } },
    pagesize : 10,
    newsurl:"http://api.admin.bitauto.com/news3/v1/news/get?carenergy=16,128&usertype=2&pageindex={pageindex}&pagesize={pagesize}&categoryids={cid}",
    Init: function () {
        var self = this;
        var type = "xinwen";
        //var temUrl = self.GetNewsUrl(type);
        self.GetNews(type);
        self.InitEvent();
    },
    InitEvent: function () {
        var self = this;
        $("#tabs-news ul li").click(function () {
            $(this).siblings().removeClass("current");
            $(this).addClass("current");
            var type = $(this).attr("data-type");
            var newslistul = $("#news-lists ul[data-type='" + type + "']");
            if ($(newslistul).length == 0 || $(newslistul).html() == "") {
                $("#news-lists ul").hide();
                $("#moreBtn").hide();
                $("#load-box").show();
                self.GetNews(type);
            }
            else {
                $("#news-lists ul").hide();
                $("#news-lists ul[data-type='" + type + "']").show();
            }
        });

        $("#moreBtn").click(function () {
            $(this).hide();
            $("#load-box").show();
            var type = $("#tabs-news ul li[class='current']").attr("data-type");
            self.GetNews(type);
        });
    },
    InitNewsEvent: function (type) {
        $("#news-lists ul[data-type='" + type + "'] .ico-news-close").unbind("click").click(function () {
            $(this).parent().hide();
        });
    },
    GetNews: function (type) {
        var self = this;
        if (typeof self.category[type] == "undefined") return "";
        var newsurl = self.newsurl.replace("{pageindex}", self.category[type].pageIndex).replace("{cid}", self.category[type].cid).replace("{pagesize}", self.pagesize);
        $.ajax({
            url: newsurl,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "GetNewsDataCallback",
            success: function (data) {
                if (data != null && typeof data != "undefined" && data.recordCount > 0) {
                    var list = data.news;
                    var h = [];
                    for (var i = 0; i < list.length; i++) {
                        var coverImg = list[i].imageCoverUrl;
                        if (coverImg == "") {
                            h.push("<li class=\"news-nopic\">");
                        }
                        else {
                            h.push("<li>");
                        }
                        h.push("<a href=\"" + list[i].url + "\">");
                        h.push("<div class=\"con-box\">");
                        h.push("<h4>" + list[i].title + "</h4>");
                        h.push("<em>");
                        h.push("<span class=\"name\">" + list[i].author + "</span>");
                        if (typeof list[i].source != "undefined" && typeof list[i].source["name"] != "undefiend") {
                            h.push("<span class=\"name\">" + list[i].source["name"]+"</span>");
                        }
                        h.push("<span>" + list[i].publishTime.substring(0, 10) + "</span>");
                        h.push("</em>");
                        h.push("</div>");
                        if (coverImg != "") {
                            h.push("<div class=\"img-box\"><img src=\"" + coverImg + "\"></div>");
                        }
                        h.push("</a>");
                        h.push("<i class=\"iconfont icon-close ico-news-close\"></i>");
                        h.push("</li>");
                    }

                    
                    if ($("#news-lists ul[data-type='" + type + "']").length == 0) {
                        h.splice(0, 0, "<ul data-type=\"" + type + "\">");
                        h.push("</ul>");
                        $("#news-lists").append(h.join(""));
                    }
                    else {
                        $("#news-lists ul[data-type='" + type + "']").append(h.join(""));
                    }
                    $("#load-box").hide();
                    $("#news-lists ul[data-type='" + type + "']").show();
                    if (data.recordCount > (self.category[type].pageIndex * self.pagesize)) {
                        $("#moreBtn").show();
                        self.category[type].pageIndex = self.category[type].pageIndex + 1;
                    }
                    else {
                        $("#moreBtn").hide();
                    }
                    self.InitNewsEvent(type);
                    Bglog_InitPostLog();
                }
            }
        });
    }
}

function InitEvent() {
    // 焦点图切换
    var mySwiperfocus = new Swiper('#m-focus-box', {
        loop: true,
        autoHeight: true,
        autoplay: 5000,
        pagination: '.pagination-num'
    });

    //热门新能源
    var hotelectabs = $("#hotelectabs ul li");
    var hotelecboxes = $("#hotelecboxes");
    if ($(hotelectabs).length > 1) {
        $(hotelectabs).each(function () {
            $(this).click(function () {
                $(hotelectabs).each(function () {
                    $(this).find("a").removeClass("current");
                });
                $(this).find("a").addClass("current");
                var type = $(this).attr("data-type");
                $(hotelecboxes).find("ul").hide();
                $(hotelecboxes).find("ul[data-type='" + type + "']").show();
            });
        });
    }
}