$(function () {
    var $body = $('body');
    $body.trigger('rightswipe1', {
        actionName: '[data-action=firstmodel]',
        fliterTemplate: function (templateid, paras) {
            return "#firstModelTemplate";
        }
    });
    api.model.currentid =carId;
    //车款点击回调事件
    api.model.clickEnd = function (paras) {
        //车款ID
        //console.log('车款ID：' + paras.modelid)
        api.model.currentid = paras.modelid;
        var $back = $('.' + $leftPopupModels.attr('data-back'));
        //关闭浮层
        $back.trigger('close');
        _commonSlider($("[data-action=firstmodel]"), $body);
        setTimeout(function () { document.location.href = "/"+serialAllSpell+"/m" + paras.modelid + "/"; }, 500);
    }
})

//层自适应
var _commonSlider = function ($model, $body) {
    if ($model.height() > $(document.body).height()) {
        $(document.body).height($model.height())
    } else if ($model.height() < $(document.body).height()) {
        $('#container', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight);
        $('.brandlist').height(document.documentElement.clientHeight);
    }
}
// 应用下载
var mySwiperApp = new Swiper('#m-app-part-scroll', {
    pagination: '.pagination-app',
    loop: true,
    grabCursor: true,
    paginationClickable: true
});
$(document).ready(function () {
    if ($("#m-app-part-scroll").find("li").length < 4) {
        $(".pagination-app").hide();
    };
});

$(function () {
    //$('#master_container').loadHtml({end:function(html){ this.html(html);}});
    //右侧侧附加选择层
    (function () {
        $('[data-action=popup-car]').rightSwipe({
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'))
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                        probeType: 1,
                        snap: 'dd',
                        momentum: true,
                        click: true
                    });
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();
});