
////////////////////////////////////////////////////////////

$(function () {
    var $ruler = $('.ruler'),
        $sliderbar = $ruler.find('.sliderbar'),
        $labels = $sliderbar.find('.line'),
        min = parseInt($sliderbar.attr('data-min')),
        max = parseInt($sliderbar.attr('data-max')),
        labelW = parseInt($labels.attr('data-width')),
        currents = $sliderbar.find('.current'),
        $dot = $sliderbar.find('.touch-dot');
    $dotNum = $("#max-dot");
    var slider = $sliderbar.scrollbarX({
        touchstart: function (v) {
            $dot.css('left', v.currentIndex + '%');
            // $dot.html(v.currentIndex);
            $dot.show();
            //100+
            if (v.currentIndex < 100) {
                $dot.html(v.currentIndex + "+");
            } else {
                $dot.html("不限");
            }
        },
        touchmove: function (v) {

            //console.log(v)

            var $current = this,
                $not = null;
            $span = $current.find('span');
            if (currents.length > 1) {
                currents.each(function (index, curr) {
                    if ($(curr).attr('data-bar') != $current.attr('data-bar')) {
                        $not = $(curr);
                    }
                })
                if ($current.attr('data-bar') == '1' && v.currentIndex >= parseInt($not.attr('data-index'))) {
                    return;
                } else if ($current.attr('data-bar') == '2' && v.currentIndex <= parseInt($not.attr('data-index'))) {
                    return;
                }
            }
            $dot.css('left', v.currentIndex + '%');
            $current.width(v.x);
            $current.attr('data-index', v.currentIndex);

            //100+
            if (v.currentIndex == 100) {
                $span.html("不限");
                $dot.html("不限");
            } else if (v.currentIndex > 90 && v.currentIndex < 100) {
                $span.html("90+");
                $dot.html("90+");
            } else {
                $span.html(v.currentIndex);
                $dot.html(v.currentIndex);
            }

            //console.log(v.currentIndex);
        },
        touchend: function () {
            $dot.hide();
            var max = parseInt($sliderbar.find('.max-dot span').html()),
                min = parseInt($sliderbar.find('.min-dot span').html());

            //console.log(min, max);
            selectElecCar.checkPrice();
            selectElecCar.selectCar(selectElecCar);
        }
    });

    $ruler.on('setvalue', function (ev, paras) {
        $sliderbar.find('[data-bar=1]').attr('data-index', paras.min).find('span').html(paras.min);
        $sliderbar.find('[data-bar=2]').attr('data-index', paras.max).find('span').html(paras.max);
        $sliderbar.trigger('refresh');
    })


    // 还原 

    //var btn = $("#clearSelected");
    //btn.on("click", function () {
    //    initFun();
    //})

    //function initFun() {
    //    $dot.html("不限").css("left", "0%");
    //    $sliderbar.find('[data-bar=1]').width(0).attr('data-index', 0).find('span').html(0);
    //    $sliderbar.find('[data-bar=2]').width($ruler.width()).attr('data-index', 100).find('span').html("不限"); 
    //    //slider.trigger('clear');
    //} 
});

function initScroll() {
    var $ruler = $('.ruler'),
        $sliderbar = $ruler.find('.sliderbar'), 
        $dot = $sliderbar.find('.touch-dot');
    $dot.html("不限").css("left", "0%");
    $sliderbar.find('[data-bar=1]').width(0).attr('data-index', 0).find('span').html(0);
    $sliderbar.find('[data-bar=2]').width($ruler.width()).attr('data-index', 100).find('span').html("不限");
     
} 