$(function () {
    var pageIndex = 1;
    var pageSIndex = 1;
    var brandIndex = 0;
    //焦点区图片 效果
    $("#focus_color_box>a").hover(function () {
        var index = $(this).index();
        $(".focus_box>[id='focuscolor_" + (index + 1) + "']").show().siblings("div[class='img_box']").hide();
    }, function () {
        $(".focus_box>[id='focus_image_first']").show().siblings("div[class='img_box']").hide();
    });
    jqPage();
    //车型翻页
    $(".zaishou .page_box .left").click(function () {
        pageIndex = pageIndex - 1;
        if (pageIndex < 1) {
            pageIndex = 1;
        }
        jqPage();
    });
    $(".zaishou .page_box .right").click(function () {
        pageIndex = pageIndex + 1;
        var pageCount = $("#pageCount").html()
        if (pageIndex >= pageCount) {
            pageIndex = pageCount;
        }
        jqPage();
    });

    function jqPage() {
        $("#pageIndex").html(pageIndex);
        $("tr[class^='carlist_']").hide();
        $("tr[class='carlist_" + pageIndex + "']").show();
    };
    //切换品牌
    $(".cont_box .tabs_box .tab a").click(function () {
        var index = $(this).index();
        brandIndex = index;
        $(".tab>a").removeClass('bg_btn hover');
        $(".tab>a[id='brand_" + index + "']").addClass('bg_btn hover');
        $(".cont_img_box>ul[id='serialList_" + index + "']").show().siblings("ul[class='serialList']").hide();
        pageSIndex = 1;
        $("#serialList_" + brandIndex + ">.span_r").show();
        $("#serialList_" + brandIndex + ">.seiallist_" + pageSIndex).show();
        $("#serialList_" + brandIndex + ">.span_r>#pageIndex").html(pageSIndex);
    });
    $("#serialList_" + brandIndex + ">.seiallist_" + pageSIndex).show();
    $("#serialList_" + brandIndex + ">.span_r").show();
    //子品牌翻页
    $(".left_btn").click(function () {
        pageSIndex = pageSIndex - 1;
        if (pageSIndex < 1) {
            pageSIndex = 1;
        }
        var ulName = "serialList_" + brandIndex;
        var liClass = "seiallist_" + pageSIndex;
        if ($("#" + ulName + ">." + liClass + "") != "undifined" && $("#" + ulName + ">." + liClass + "").length != 0) {
            $("#" + ulName + ">.seiallist_" + (pageSIndex + 1)).hide().siblings(".seiallist_" + pageSIndex).show();
        }
        $("#serialList_" + brandIndex + ">.span_r>#pageIndex").html(pageSIndex);   
    });
    $(".right_btn").click(function () {
        var ulName = "serialList_" + brandIndex;
        var liClass = "seiallist_" + (pageSIndex+1);
        if ($("#" + ulName + ">." + liClass + "") != "undifined" && $("#" + ulName + ">." + liClass + "").length != 0) {
            pageSIndex = pageSIndex + 1;
            $("#" + ulName + ">.seiallist_" + (pageSIndex - 1)).hide().siblings(".seiallist_" + pageSIndex).show();
        }
        $("#serialList_" + brandIndex + ">.span_r>#pageIndex").html(pageSIndex);
    });
});

