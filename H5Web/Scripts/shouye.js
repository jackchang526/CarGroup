$(document).ready(function () {
    var imgtoparr = [];
    $(".brand img").each(function (i, img) {
        imgtoparr.push($(this).offset().top);
    });

    window.addEventListener("scroll", function () {
        $(".brand img").each(function (i, img) {
            if (document.body.scrollTop >= (imgtoparr[i] - 10)) {
                $(this).addClass("fixed");
                $(this).siblings().addClass("fixed");
            }
            if (document.body.scrollTop < (imgtoparr[i] - 10)) {
                $(this).removeClass("fixed");
                $(this).siblings().removeClass("fixed");
            }
        });
    }, false);

});