
var mySwiperfocus = new Swiper('#m-focus-box', {
    pagination: '.pagination-focus',
    loop: true,
    grabCursor: true,
    paginationClickable: true,
    calculateHeight: true,
   
});
window.onorientationchange=function(){
	var mySwiperfocus = new Swiper('#m-focus-box', {
    calculateHeight: true
});
};

$(document).ready(function () {
    $("#a_more6").bind("click", function () {
        $("#div_carlist ul li").show();
        $(this).hide();
        $("#a_morecar").show();
    });
});

