$(document).ready(function () {
    var serialcount = $("#div_seriallist ul li").length;
    $("#em_serialcount").html(serialcount + "款");
    $("#div_brandList").hide();

    $("#btn_shaixuan").bind("click", function () {
        $("#div_brandList").show();
    });

    $("#div_brandList ul li").bind("click", function () {
        var brandName = $(this).attr("brand");

        $("#div_brandList ul li").removeClass("current");
        $(this).addClass("current");
        selectSerials(brandName, 4);
        var brandSpell = $(this).attr("spell");
        var href = $("#a_moreserials").attr("href");
        $("#a_moreserials").attr("href", href+"#"+ brandSpell);
        $("#div_brandList").hide();
    });

    //继续加载4条
    $("#a_more4").bind("click", function () {
        var brandName = $("#div_brandList ul li[class='current']").attr("brand");
        var lis = $("#div_seriallist ul li");
        $("#div_seriallist ul li").hide();
        var count = 0;
        if (brandName == "全部") {
            for (var j = 0; j < lis.length; j++) {
                $(lis[j]).show();
                count++;
                if (count >= 8) {
                    break;
                }
            }
        } else {
            for (var i = 0; i < lis.length; i++) {
                var name = $(lis[i]).attr("brand");
                if (brandName == name) {
                    $(lis[i]).show();
                    count++;
                }
                if (count >= 8) {
                    break;
                }
            }
        }
        $(this).hide();
        $("#a_moreserials").show();
    });

    function selectSerials(brandName, maxcount) {
        var count = 0;
        var lis = $("#div_seriallist ul li");
        $("#div_seriallist ul li").hide();
        if (brandName == "全部") {
            for (var j = 0; j < lis.length; j++) {
                if (count >= maxcount) {
                    count++;
                    continue;
                }
                $(lis[j]).show();
                count++;
            }
        } else {
            for (var i = 0; i < lis.length; i++) {
                var name = $(lis[i]).attr("brand");
                if (brandName == name) {
                    if (count >= maxcount) {
                        count++;
                        continue;
                    }
                    $(lis[i]).show();
                    count++;
                }
            }
        }
        if (count > 4) {
            $("#a_more4").show();
            $("#a_moreserials").hide();
        }
        $("#em_serialcount").html(count + "款");
    }
});