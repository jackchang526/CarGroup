$(function () {
    //头部切换车分类
    $(".list-car-category li:not(.beijing-market)").on("mouseover", function (e) {
        //console.log(e);
        var $carListDetailsLi = $(".car-list-details > li"),
            thisIndex = $(this).index();
        $(this).siblings("li:not(.beijing-market)").removeClass("active").end().addClass("active");
        $carListDetailsLi.removeClass("active").eq(thisIndex).addClass("active");
    })
})