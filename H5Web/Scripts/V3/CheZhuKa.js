define("chezhuka", function () {
    //热门点评
    function GetKouBei() {
        var index = Config.auchors.indexOf("page6");
        if (index < 0) {
            $("div[data-anchor='page6']").remove();
            return;
        }
        $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function (data) {
            $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='page6']");
            var index = Config.auchors.indexOf("page6");
            $("#fullpage").fullpage.resetSlides(index);

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }
     
    //点评信息
    GetKouBei();
});