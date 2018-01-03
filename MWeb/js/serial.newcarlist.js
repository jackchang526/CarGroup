// tab切换Swiper
var swiperNewslist = new Swiper('.js-swiper-container-news-carlist', {
    autoHeight: true,
    onSlideChangeStart: function () {
        $(".js-news-cartab .current").removeClass('current')
        $(".js-news-cartab li").eq(swiperNewslist.activeIndex).addClass('current')
    }
});
$(".js-news-cartab li").on('touchstart mousedown', function (e) {
    e.preventDefault()
    $(".js-news-cartab .current").removeClass('current')
    $(this).addClass('current')
    swiperNewslist.slideTo($(this).index())
    loadNewCarList($(this).index());
});
$(".js-news-cartab li").click(function (e) {
    e.preventDefault()
});


window.addEventListener('scroll', function () {
    if ($(window).scrollTop() + $(window).height() + 340 >= $(document).height()) {
        var index = $(".js-news-cartab .current").attr("tabindex");
        //console.log("scroll:"+index);
        RequestData(index);
    }
}, false);


var titles = [
    { id: 0, total: 0, index: 0, size: 10, hasNext: true, loading: false },
    { id: 1, total: 0, index: 0, size: 10, hasNext: true, loading: false }
];

function loadNewCarList(index) {
    if ($("#newscar_list_" + index + " li").length > 0) {
        return;
    }
    if (titles[index].index > 0) {
        return;
    }
    RequestData(index);
}
function newCarListCallBack0() {
}
function newCarListCallBack1() {
}
function RequestData(idx) {
    //console.log(idx);
    var curindex = idx;
    if (curindex == undefined) {
        return;
    }
    if (!titles[curindex].hasNext) {
        return;
    }
    if (titles[curindex].loading) {
        //console.log("正在加载-" + curindex);
        return;
    }
    var newCarListUrl = "http://api.car.bitauto.com/CarInfo/GetNewCarList.ashx?type=" + titles[curindex].id + "&pageindex=" + (titles[curindex].index + 1) + "&pagesize=" + titles[curindex].size;
    titles[curindex].loading = true;
    var cb = "newCarListCallBack" + curindex;
    $("#newscar_list_" + curindex + " .load-box").show();
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
                //
                if (data.data.data.length > 0) {
                    var html = "";
                    var len = data.data.data.length;
                    for (var i = 0; i < len; i++) {
                        var dd = data.data.data[i];
                        if (dd.AllSpell == "") {
                            continue;
                        }
                        html += ('<li><a href="/' + dd.AllSpell + '/" data-id="' + dd.CsId + '">');
                        html += '        <div class="left">';
                        html += ('            <img class="car-img" src="' + dd.CoverImage + '" alt="">');
                        html += '                <div class="info-box">';
                        html += '                    <div class="title">';
                        html += '                       <div class="text">' + dd.CsName + '</div>';
                        if (curindex == 0 && dd.Type == 2) {
                            html += '                       <div class="tag">' + dd.YearType + '款</div>';
                        } else if (curindex == 0 && dd.Type == 1) {
                            html += '                       <div class="tag">全新车型</div>';
                        }

                        html += '                    </div>';
                        var price = dd.RefPrice == '0' ? "暂无指导价" : (dd.RefPrice.indexOf('-') > 0 ? dd.RefPrice.replace("万", "") : dd.RefPrice);
                        html += '                    <div class="price">' + price + '</div>';
                        html += '                    <div class="type">' + dd.Level + '</div>';
                        html += '                </div>';
                        html += '        </div>';
                        if (curindex == 1 && dd.MarketDay != "") {
                            var marketDay = new Date(dd.MarketDay);
                            html += '            <div class="right"><div class="time">' + (marketDay.getMonth() + 1) + '月' + marketDay.getDate() + '日上市</div></div>';
                        }

                        html += '</a></li>';

                    }
                    $("#newscar_list_" + curindex + " .load-box").before(html);
                    swiperNewslist.update();

                    titles[curindex].index++;
                    titles[curindex].total = data.data.total;
                    if (data.data.data.length < data.data.size) {
                        titles[curindex].hasNext = false;
                    } else {
                        titles[curindex].hasNext = true;
                    }
                } else {
                    titles[curindex].hasNext = false;
                    //$("#newscar_list_" + curindex + " .load-box li").hide();
                    //$("#newscar_list_" + curindex + " .load-box span").text("全部加载完成");
                }
            } else if (data && data.code == 0) {
                titles[curindex].hasNext = false;
                //$("#newscar_list_" + curindex + " .load-box li").hide();
                //$("#newscar_list_" + curindex + " .load-box span").text("全部加载完成"); 
            }
            titles[curindex].loading = false;
            $("#newscar_list_" + curindex + " .load-box").hide();
        },
        error: function (msg) {
            titles[curindex].loading = false;
            $("#newscar_list_" + curindex + " .load-box").hide();
            //console.log(msg);
        }
    });
}
RequestData(0)