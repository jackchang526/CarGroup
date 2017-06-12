define("dealerdata", function() {

    function getDealerIndex() {
        var name = "page1";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealerdefault&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {

            $("div[data-anchor='page1']").html(data);

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerIndex();

    //获取车款列表
    function getCarList() {
        var name = "page6";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=carsprice&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {
            if (data != "" && data != null && checkData(data)) {
                $("#dealercarlisttmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#dealercarlisttmpl").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getCarList();

    //热门促销
    function getDealerActivity() {
        var name = "page7";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealernews&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {
            if (data != "" && data != null && checkData(data)) {
                $("#dealeractivity").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#dealeractivity").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerActivity();

    //店内还有
    function getDealerMore() {
        var name = "page8";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealercarreference&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {
            if (data != "" && data != null && checkData(data)) {
                $("#dealermoretmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#dealermoretmpl").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerMore();


    //商家店铺
    function getDealerShop() {
        var name = "page9";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealerstaticmap&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {
            var wrapper = $("div[data-anchor='" + name + "']");
            if (data != "" && data != null && checkData(data)) {
                wrapper.html(data);
                $("#map_detail,#map_img", wrapper).attr("href", "/V3/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&dealerid=" + Config.dealerId);
            } else {
                wrapper.html("");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerShop();

    //养护
    function getDealerYanHu() {
        var name = "page10";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=yanghu&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function(data) {
            if (data != "" && data != null && checkData(data)) {
                $("#dealeryanghu").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (Config.auchors[Config.auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            } else {
                //$("#dealeryanghu").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
                $("div[data-anchor='" + name + "']").remove();
            }
        });
    }

    getDealerYanHu();

    //ending
    function getDealerEnding() {
        var name = "page11";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=ending&dealerid=" + Config.dealerId + "&", function(data) {
            if (data != "" && data != null && checkData(data)) {
                $("#dealerending").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
                $("#fullpage").fullpage.resetSlides(index);
                //最后一页去掉向下箭头
                if (Config.auchors[Config.auchors.length - 1] === name) {
                    $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
                }
            } else {
                //$("#dealerending").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
                $("div[data-anchor='" + name + "']").remove();
            }
        });
    }

    getDealerEnding();
});