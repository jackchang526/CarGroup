define("brokerdata", function(require) {
    $(".fixed_box").remove();

    //热门点评
    function getKouBei() {
        var name = "page6";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function(data) {
            $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='" + name + "']");

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }

        });
    }

    getKouBei();

    //获取车款列表
    function getCarList() {
        var name = "page7";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/getcarlist.ashx?top=19&csid=" + Config.serialId + "&", function(data) {
            if (!data) {
                return;
            }
            var length = data.carlist.length;
            var listGroup = [];
            var list = data.carlist;
            for (var i = 0; i < Math.ceil(length / 4); i++) {
                listGroup.push({ "index": i, "carlist": list.slice(i * 4, (i + 1) * 4) });
            }
            $("#agentcarlisttmp").tmpl({ "carcount": data.count, "listgroup": listGroup }).appendTo("div[data-anchor='" + name + "']");

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }

        });
    }

    getCarList();

    function getBroker() {
        var name = "page8";
        var index = Config.auchors.indexOf(name);
        if (index < 0) {
            $("div[data-anchor='" + name + "']").remove();
            return;
        }
        $.get("/handlers/GetDataAsynV3.ashx?service=agent&method=brokerinfo&csid=" + Config.serialId + "&brokerid=" + Config.brokerId + "&type=0&", function(data) {
            if (checkData(data)) {
                $("div[data-anchor='" + name + "']").html(data);
            } else {
                $("div[data-anchor='" + name + "']").html("<div class='message-failure'><img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png' /> <h2>很遗憾！</h2> <p>数据抓紧完善中，敬请期待！</p> </div>");
            }

            $("#fullpage").fullpage.resetSlides(index);
            //最后一页去掉向下箭头
            if (Config.auchors[Config.auchors.length - 1] === name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }
        });
    }

    getBroker();

});