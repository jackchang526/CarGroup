//nav swiper组件初始化
var mySwiper = new Swiper('.js-type-top-nav', {
    freeMode: true,
    freeModeMomentumRatio: 0.5,
    slidesPerView: 'auto'
});

swiperWidth = mySwiper.container[0].clientWidth;
maxTranslate = mySwiper.maxTranslate();
maxWidth = -maxTranslate + swiperWidth / 2;

mySwiper.on('tap', function (swiper, e) {
    e.preventDefault();
    caclOffset(mySwiper, swiper.slides[swiper.clickedIndex]);
    //控制top nav激活样式
    $(".js-type-top-nav .active").removeClass('active');
    $(".js-type-top-nav .swiper-slide").eq(swiper.clickedIndex).addClass('active');
    //同步点击标签切换到对应tab页面
    swiperNewslist.slideTo(swiper.clickedIndex);

    //todo function
    console.log(swiper.clickedIndex);
    loadRanking(swiperNewslist.activeIndex);
});

//tab swiper组件初始化
var swiperNewslist = new Swiper('.js-type-tab-page', {
    autoHeight: true,
    onSlideChangeStart: function () {
        caclOffset(mySwiper, mySwiper.slides[swiperNewslist.activeIndex]);
        //同步top nav激活样式
        //swiperNewslist.activeIndex取滑动到的index
        $(".js-type-top-nav .active").removeClass('active');
        $(".js-type-top-nav .swiper-slide").eq(swiperNewslist.activeIndex).addClass('active');
        loadRanking(swiperNewslist.activeIndex);
        //todo function 
    }
});

window.addEventListener('scroll', function () {
    if ($(window).scrollTop() + $(window).height() + 340 >= $(document).height()) {
        var index = $(".swiper-wrapper .active").attr("tabindex");
        RequestData(index);
    }
}, false);

var titles = [
    { id: 3, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 8, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 1, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 2, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 5, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 4, total: 0, index: 0, size: 20, hasNext: true, loading: false },
    { id: 7, total: 0, index: 0, size: 20, hasNext: true, loading: false }
];
function loadRanking(index) {
    if ($("#level_rank_list_" + index + " li").length > 0) {
        return;
    }

    if (titles[index].index > 0) {
        return;
    }
    RequestData(index);
}
function RequestData(index) {
    if (!titles[index].hasNext) {
        return;
    }
    if (titles[index].loading) {
        console.log("正在加载-" + index);
        return;
    }
    var saleRankingUrl = "http://api174.car.bitauto.com/CarInfo/GetSalesRankingForPage.ashx?level=" + titles[index].id + "&pageindex=" + (titles[index].index + 1) + "&pagesize=" + titles[index].size;
    titles[index].loading = true;
    $("#level_rank_list_" + index + " .load-box").show();
    $.ajax({
        type: "get",
        url: saleRankingUrl,
        cache: true,
        dataType: 'jsonp',
        jsonp: "callback",
        jsonpCallback: "SaleRankingCallBack",
        timeout: 3000,
        contentType: "application/json",
        success: function (data) {
            if (data && data.code == 1) {
                //
                if (data.data.page.data.length > 0) {
                    var html = "";
                    var len = data.data.page.data.length;
                    for (var i = 0; i < len; i++) {
                        var dd = data.data.page.data[i];
                        html += '<li><div class="left">';
                        html += '    <div class="rank-number"><span>' + dd.rank + '</span></div>';
                        html += ('        <img class="car-img" src="' + dd.imgUrl + '"alt="">');
                        html += '         <div class="info-box">';
                        html += ('             <div class="title"><div class="text">' + dd.showName + '</div></div>');
                        html += ('            <div class="price">' + dd.priceRange + '</div>');
                        html += ('            <div class="type">全球销量：' + dd.sellNum + '辆</div>');
                        html += '        </div>';
                        html += '</div></li>';
                    }
                    $("#level_rank_list_" + index + " .load-box").before(html);
                    swiperNewslist.update();
                    var dateStr = data.data.month;
                    if (dateStr == "") {
                        $(".swiper-slide .phone-title span").text("销量排行");
                    } else {
                        var monthStr = dateStr.substr(dateStr.indexOf("-") + 1);
                        $(".swiper-slide .phone-title span").text(monthStr + "月销量排行");
                    }


                    titles[index].index++;
                    titles[index].total = data.data.page.total;
                    if (data.data.page.data.length < data.data.page.size) {
                        titles[index].hasNext = false;
                    } else {
                        titles[index].hasNext = true;
                    }
                } else {
                    titles[index].hasNext = false;
                }
            } else if (data && data.code == 0) {
                titles[index].hasNext = false;
            }
            console.log(data);
            console.log(titles[index].hasNext);
            titles[index].loading = false;
            $("#level_rank_list_" + index + " .load-box").hide();
        },
        error: function (msg) {
            titles[index].loading = false;
            $("#level_rank_list_" + index + " .load-box").hide();
            console.log(msg);
        }
    });
}
loadRanking(0);
//计算navSwiper移动点击标签滑动距离
//mySwiper: swiperTopNav对象
//slide:swiperTopNav当前激活的slider对象
var caclOffset = function (mySwiper, slide) {
    slideLeft = slide.offsetLeft;
    slideWidth = slide.clientWidth;
    slideCenter = slideLeft + slideWidth / 2;

    // 计算被点击slide的中心点，计算偏移
    mySwiper.setWrapperTransition(300);
    if (slideCenter < swiperWidth / 2) {
        mySwiper.setWrapperTranslate(0)
    } else if (slideCenter > maxWidth) {
        mySwiper.setWrapperTranslate(maxTranslate)
    } else {
        nowTlanslate = slideCenter - swiperWidth / 2
        mySwiper.setWrapperTranslate(-nowTlanslate)
    }
}