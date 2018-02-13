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
    loadNewCarList(swiper.clickedIndex);
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

    }
});

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

window.addEventListener('scroll', function () {
    if ($(window).scrollTop() + $(window).height() + 340 >= $(document).height()) {
        var index = $(".swiper-wrapper .active").attr("tabindex");
        RequestData(index);
    }
}, false);

var titles = [
    { id: 'elec', total: 0, index: 0, size: 10, hasNext: true, loading: false },
    { id: 'hybrid', total: 0, index: 0, size: 10, hasNext: true, loading: false }
];

function loadNewCarList(index) {
    if ($("#rank_list_" + index + " li").length > 0) {
        return;
    }
    if (titles[index].index > 0) {
        return;
    }
    RequestData(index);
}
function RequestData(idx) {
    var curindex = idx;
    if (curindex == undefined) {
        return;
    }
    if (!titles[curindex].hasNext) {
        return;
    }
    if (titles[curindex].loading) {
        return;
    }
    var newCarListUrl = "http://api.car.bitauto.com/CarInfo/GetSalesRankingForPage.ashx?type=newenergy&tab=" + titles[curindex].id + "&pageindex=" + (titles[curindex].index + 1) + "&pagesize=" + titles[curindex].size;
    titles[curindex].loading = true;
    var cb = "newCarListCallBack" + curindex;
    $("#newscar_list_" + curindex).show();
    $.ajax({
        type: "get",
        url: newCarListUrl,
        cache: true,
        dataType: 'jsonp',
        jsonp: "callback",
        jsonpCallback: cb,
        timeout: 5000,
        contentType: "application/json",
        success: function (data) {
            if (data && data.code == 1) {
                if (data.data.page.data.length > 0) {
                    var html = "";
                    var len = data.data.page.data.length;
                    for (var i = 0; i < len; i++) {
                        var dd = data.data.page.data[i];
                        if (dd.allSpell == "") {
                            continue;
                        }
                        html += ('<li><a href="/' + dd.allSpell + '/"><div class="left"><div class="rank-number"><span>' + dd.rank + '</span></div>');
                        html += ('    <img class="car-img" src="' + dd.imgUrl.replace("_2.", "_6.") + '" alt="">');
                        html += ('        <div class="info-box">');
                        html += ('            <div class="title"><div class="text">' + dd.showName + '</div></div>');
                        html += ('            <div class="price">' + dd.priceRange + '</div>');
                        html += ('            <div class="type">全国销量：' + dd.sellNum + '辆</div>');
                        html += ('        </div></div></a></li></ul>');
                    }
                    $("#rank_list_" + curindex).append(html);
                    swiperNewslist.update();
                    var dateStr = data.data.month;
                    if (dateStr == "") {
                        $(".swiper-slide .phone-title span").text("销量排行");
                    } else {
                        var monthStr = dateStr.substr(dateStr.indexOf("-") + 1);
                        $(".swiper-slide .phone-title span").text(monthStr + "月销量排行");
                    }

                    titles[curindex].index++;
                    titles[curindex].total = data.data.page.total;
                    if (data.data.page.data.length < data.data.page.size) {
                        titles[curindex].hasNext = false;
                        $("#newscar_list_" + curindex + " span").text("全部加载完成");
                        $("#newscar_list_" + curindex + " i").hide();
                    } else {
                        titles[curindex].hasNext = true;
                        //$("#newscar_list_" + curindex + " span").text("正在加载");
                        $("#newscar_list_" + curindex + " span").text("正在加载");
                        $("#newscar_list_" + curindex + " i").show();
                    }
                } else {
                    titles[curindex].hasNext = false;
                    //$("#newscar_list_" + curindex).hide();
                    $("#newscar_list_" + curindex + " span").text("全部加载完成");
                    $("#newscar_list_" + curindex + " i").hide();
                }
            } else if (data && data.code == 0) {
                titles[curindex].hasNext = false;
                //$("#newscar_list_" + curindex).hide();
                $("#newscar_list_" + curindex + " span").text("全部加载完成");
                $("#newscar_list_" + curindex + " i").hide();
            }
            titles[curindex].loading = false;
            //$("#newscar_list_" + curindex).hide();
        },
        error: function (msg) {
            titles[curindex].loading = false;
            //$("#newscar_list_" + curindex).hide();
            //console.log(msg);
        }
    });
}