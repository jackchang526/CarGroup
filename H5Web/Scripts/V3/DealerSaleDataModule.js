define("dealersale", function() {

    function checkPage(pagename) {
        var index = Config.auchors.indexOf(pagename);
        if (index < 0) {
            $("div[data-anchor='" + pagename + "']").remove();
            return false;
        }
        return true;
    }

    //人+车+店定制版 车型报价
    function getDelarSaleCarPrice() {
        if (!checkPage("page6")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalecarprice&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersalecarprice").tmpl({ html: data.trim() }).appendTo("div[data-anchor='page6']");
                var index = Config.auchors.indexOf("page6");
                $("#fullpage").fullpage.resetSlides(index);
            } else {
                $("#dealersalecarprice").tmpl({ html: "" }).appendTo("div[data-anchor='page6']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 热门促销
    function getDelarSaleCarNews() {
        if (!checkPage("page7")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalenews&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersalenews").tmpl({ html: data.trim() }).appendTo("div[data-anchor='page7']");
            } else {
                $("#dealersalenews").tmpl({ html: "" }).appendTo("div[data-anchor='page7']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 热销车型(看了还看)
    function getDelarSaleSerial() {
        if (!checkPage("page8")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleserial&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersaleserial").tmpl({ html: data.trim() }).appendTo("div[data-anchor='page8']");
            } else {
                $("#dealersaleserial").tmpl({ html: "" }).appendTo("div[data-anchor='page8']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 商家店铺
    function getDelarSaleShop() {
        if (!checkPage("page9")) {
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleshop&dealerpersonid=" + Config.dealerpersonid + "&", function(data) {
            if (checkData(data)) {
                $("#dealersaleshop").tmpl({ html: data.trim() }).appendTo("div[data-anchor='page9']");
            } else {
                $("#dealersaleshop").tmpl({ html: "" }).appendTo("div[data-anchor='page9']");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

    getDelarSaleCarPrice();
    getDelarSaleCarNews();
    getDelarSaleSerial();
    getDelarSaleShop();
});