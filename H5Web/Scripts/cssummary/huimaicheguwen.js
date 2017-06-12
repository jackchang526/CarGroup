define("huimaicheguwen", function() {
    //购车点评
    function getCommentInfo() {
        var name = "page6";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=comment&agentid=" + Config.agentid + "&csid=" + Config.serialId + "&", function(data) {

            if (data !== "" && data != null && checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("#nodatatmpl").tmpl({ title: "购车点评" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //购车点评
    getCommentInfo();

    //车款报价
    function getCarInfo() {
        var name = "page7";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=carinfo&agentid=" + Config.agentid + "&csid=" + Config.serialId + "&", function(data) {

            if (data !== "" && data != null && checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("#nodatatmpl").tmpl({ title: "车款报价" }).appendTo("div[data-anchor='" + name + "']");
            }
            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //车款报价
    getCarInfo();

    //购车优惠
    function getDiscountInfo() {
        var name = "page8";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=discount&agentid=" + Config.agentid + "&csid=" + Config.serialId + "&", function(data) {

            if (data !== "" && data != null && checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("#nodatatmpl").tmpl({ title: "购车优惠" }).appendTo("div[data-anchor='" + name + "']");
            }
            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //购车优惠
    getDiscountInfo();

    //免费预约
    function getBookingInfo() {
        var name = "page9";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=booking&agentid=" + Config.agentid + "&csid=" + Config.serialId + "&", function(data) {

            if (data !== "" && data != null && checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("#nodatatmpl").tmpl({ title: "免费预约" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //免费预约
    getBookingInfo();

    //看了还看
    function getSeeAgainSerial() {
        var name = "page10";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=seeagain&agentid=" + Config.agentid + "&csid=" + Config.serialId + "&", function(data) {

            if (data !== "" && data != null && checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("#nodatatmpl").tmpl({ title: "看了还看" }).appendTo("div[data-anchor='" + name + "']");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    //看了还看
    getSeeAgainSerial();
});