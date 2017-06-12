(function ($) {
    //X向滚动
    $.fn.dragX = function (options) {
        var setting = {
            onstart: null,
            onmove: null,
            onend: null
        }
        $.extend(true, setting, options);
        var $this = this;
        $this.X = $this.disX = 0;
        $this.touches({
            touchstart: function(ev) {
                ev.preventDefault();
                $this.disX = ev.targetTouches[0].pageX - $this.X;
                setting.onstart && setting.onstart.call($this, $this.disX, ev.targetTouches[0].pageX);
            },
            touchmove: function(ev) {
                ev.preventDefault();
                $this.X = ev.targetTouches[0].pageX - $this.disX;
                setting.onmove && setting.onmove.call($this, $this.X);
            },
            touchend: function(ev) {
                setting.onend && setting.onend.call($this, $this.X, ev.changedTouches[0].pageX)
            }
        });
    }

    //触摸屏事件
    $.fn.touches = function (options) {
        var setting = {
            init: null,//初始化
            touchstart: null,  //按下
            touchmove: null, //滑动
            touchend: null //抬起
        };
        $.extend(true, setting, options);
        var $this = this, touchesDiv = $this[0];
        touchesDiv.addEventListener('touchstart', function(ev) {
            setting.touchstart && setting.touchstart.call($this, ev);

            function fnMove(ev) {

                setting.touchmove && setting.touchmove.call($this, ev);
            }

            function fnEnd(ev) {
                setting.touchend && setting.touchend.call($this, ev);
                document.removeEventListener('touchmove', fnMove, false);
                document.removeEventListener('touchend', fnEnd, false);
            }

            document.addEventListener('touchmove', fnMove, false);
            document.addEventListener('touchend', fnEnd, false);
            return false;
        }, false);
        setting.init && setting.init.call($this);
    }

    //手势方向(X轴)
    $.fn.directionX = function (options) {
        var setting = {
            init: null,
            selectfn: null,
            max: 30
        }
        $.extend(true, setting, options);
        var $this = this;
        if ($this.length == 0) return;
        $this.ox = 0;
        $this.dragX({
            onstart: function (x) {
                $this.ox = 0;
            },
            onmove: function (x) {
                if (x - $this.ox < -setting.max) {
                    clearTimeout($this.timeout)
                    $this.timeout = setTimeout(function () { setting.selectfn && setting.selectfn.call($this, 'left'); }, 300);

                } else if (x - $this.ox > setting.max) {
                    clearTimeout($this.timeout)
                    $this.timeout = setTimeout(function () { setting.selectfn && setting.selectfn.call($this, 'right'); }, 300);
                }
                if ($this.ox == 0) {
                    $this.ox = x;
                }
            },
            onend: function () {
                $this.ox = 0;
            }
        });
        setting.init && setting.init.call($this);
    }
})(jQuery);

var $boxbg = $("#menu_box_bg"), $menubox = $("#menu_box");

$(function () {
    $('.standard_car_pic_1').directionX({
        init: function () {
            var $this = this, imgs = $this.find('img'), $message = $this.next().children(0);
            $this.index = 0;
            $this.on('setIndex', function (event, i) {
                imgs.each(function (index, curr) {
                    var $current = $(curr);
                    if (index == i) {
                        $current.fadeIn();
                    } else {
                        $current.fadeOut();
                    }
                });
                $this.trigger('setMessage', i);
                $this.index = i;
            });

            $this.on('prev', function (event) {
                $this.index = $this.index - 1;
                if ($this.index < 0) {
                    $this.index = imgs.length - 1;
                }
                $this.trigger('setIndex', $this.index);
            });

            $this.on('next', function (event) {
                $this.index = $this.index + 1;
                if ($this.index > imgs.length - 1) {
                    $this.index = 0;
                }
                $this.trigger('setIndex', $this.index);
            });


            var as = $this.next().next().children();
            $this.on('setMessage', function (event, i) {
                var $a = as.eq(i);
                as.removeClass('current');
                $a.addClass('current');
                $message.html($a.children(0).attr('data-value'));
            }).trigger('setMessage', $this.index);

            as.each(function (index, curr) {
                var $current = $(curr);
                (function ($o, i) {
                    $o.on('click', function (ev) {
                        var $a = $(this);
                        ev.preventDefault();
                        $this.trigger('setIndex', i);

                    });
                })($current, index);

            });

        },
        selectfn: function (v) {
            var $this = this;
            switch (v) {
                case 'left':
                    $this.trigger('next');
                    break;
                case 'right':
                    $this.trigger('prev');
                    break;
            }
        }
    });
});

$boxbg.touches({
    touchstart: function (ev) {
        ev.preventDefault();
        $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
        $boxbg.stop().fadeOut();

        if ($menubox.attr("class").indexOf("menu_box_hover") > -1) {
            $menubox.removeClass("menu_box_hover").addClass("menu_box_down");
        }
    },
    touchmove: function (ev) {
        ev.preventDefault();
    }
});