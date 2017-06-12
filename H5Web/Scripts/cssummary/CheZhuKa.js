define("chezhuka", function() {
    //热门点评
    function GetKouBei() {
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
            if (Config.auchors[Config.auchors.length - 1] == name) {
                $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
            }

        });
    }

    //点评信息
    GetKouBei();
});