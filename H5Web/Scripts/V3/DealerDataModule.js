define("dealerdata", function() {

    function checkPage(pagename) {
        var index = Config.auchors.indexOf(pagename);
        if (index < 0) {
            $("div[data-anchor='" + pagename + "']").remove();
            return false;
        }
        return true;
    }

    function getDealerIndex() {
        if (!checkPage("page1")) {
            return;
        }
        $("div[data-anchor='page1']").html("");
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealerdefault&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function (data) {
            $("div[data-anchor='page1']").html(data);

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerIndex();

    //商家店铺
    function getDealerShop() {
        if (!checkPage("page9")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealerstaticmap&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function (data) {
            var wrapper = $("div[data-anchor='page9']");
            if (checkData(data)) {
                wrapper.html(data);
                $("#map_detail,#map_img", wrapper).attr("href", "/V3/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&dealerid=" + Config.dealerId);
            } else {
                wrapper.html("");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerShop();

    //店内还有
    function getDealerMore() {
        if (!checkPage("page8")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealercarreference&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function (data) {
            if (checkData(data)) {
                $("#dealermoretmpl").tmpl({ html: data }).appendTo("div[data-anchor='page8']");
            } else {
                $("#dealermoretmpl").tmpl({ html: "" }).appendTo("div[data-anchor='page8']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerMore();

    //本店活动
    function getDealerActivity() {
        if (!checkPage("page7")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=dealernews&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function (data) {
            if (checkData(data)) {
                $("#dealeractivity").tmpl({ html: data }).appendTo("div[data-anchor='page7']");
            } else {
                $("#dealeractivity").tmpl({ html: "" }).appendTo("div[data-anchor='page7']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getDealerActivity();

    //获取车款列表
    function getCarList() {
        if (!checkPage("page6")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealer&method=carsprice&dealerid=" + Config.dealerId + "&csid=" + Config.serialId + "&", function (data) {
            if (checkData(data)) {
                $("#dealercarlisttmpl").tmpl({ html: data }).appendTo("div[data-anchor='page6']");
                var index = Config.auchors.indexOf("page6");
                $("#fullpage").fullpage.resetSlides(index);
            } else {
                $("#dealercarlisttmpl").tmpl({ html: "" }).appendTo("div[data-anchor='page6']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getCarList();

});