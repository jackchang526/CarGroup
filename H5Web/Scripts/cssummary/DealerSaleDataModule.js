define("dealersale", function() {
    //人+车+店定制版 车型报价
    function getDelarSaleCarPrice() {
        var name = "page6";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalecarprice&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersalecarprice").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
                
            } else {
                $("#dealersalecarprice").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 热门促销
    function getDelarSaleCarNews() {
        var name = "page7";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalenews&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersalenews").tmpl({ html: data.trim() }).appendTo("div[data-anchor='"+name+"']");
            } else {
                $("#dealersalenews").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='page7']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 热销车型(看了还看)
    function getDelarSaleSerial() {
        var name = "page8";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleserial&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&", function(data) {
            if (checkData(data)) {
                $("#dealersaleserial").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
            } else {
                $("#dealersaleserial").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='"+name+"']").find(".arrow_down").hide();
            }
        });
    }

    //人+车+店定制版 商家店铺
    function getDelarSaleShop() {
        var name = "page9";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleshop&dealerpersonid=" + Config.dealerpersonid + "&", function(data) {
            if (checkData(data)) {
                $("#dealersaleshop").tmpl({ html: data.trim() }).appendTo("div[data-anchor='"+name+"']");
            } else {
                $("#dealersaleshop").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getDelarSaleCarPrice();
    getDelarSaleCarNews();
    getDelarSaleSerial();
    getDelarSaleShop();
});