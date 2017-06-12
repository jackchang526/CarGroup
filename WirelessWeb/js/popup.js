// 右侧弹出层
Object.extend = function (destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
};

var classNames = ['Webkit', 'ms', 'Moz', 'O', ''];
var eventNames = ['webkit', 'moz', 'o'];
var hv = 120;
(function ($) {
    //添加css3样式
    $.fn.addClass3 = function (name, value) {
        var o = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < classNames.length; i++) {
            o.style[classNames[i] + cName] = value;
        }
        return $(o);
    }

    //添加事件
    $.fn.addEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.addEventListener(eventNames[i] + cName, fn, false);
        }
        obj.addEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    //删除事件
    $.fn.removeEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.removeEventListener(eventNames[i] + cName, fn, false);
        }
        obj.removeEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    //transition事件监听
    $.fn.transitionEnd = function (options) {
        var setting = {
            listen: 'TransitionEnd',
            end: null
        }
        options = Object.extend(options, setting);
        var $this = this;
        function seatTransitionEnd() {
            for (var i = 0; i < eventNames.length; i++) {
                if (eventNames[i] == 'moz') {
                    $this.removeEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
                } else {
                    $this.removeEvent(eventNames[i] + options.listen, seatTransitionEnd);
                }
            }
            options.end && options.end.call($this);
        }
        for (var i = 0; i < eventNames.length; i++) {
            if (eventNames[i] == 'moz') {
                $this.addEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
            } else {
                $this.addEvent(eventNames[i] + options.listen, seatTransitionEnd);
            }
        }
    }

    //触摸屏事件
    $.fn.touches = function (options) {
        var setting = {
            init: null,//初始化
            touchstart: null,  //按下
            touchmove: null, //滑动
            touchend: null //抬起
        };
        options = Object.extend(options, setting);
        var $this = this, touchesDiv = $this[0];
        touchesDiv.addEventListener('touchstart', function (ev) {
            options.touchstart && options.touchstart.call($this, ev);

            function fnMove(ev) {
                options.touchmove && options.touchmove.call($this, ev);
            }

            function fnEnd(ev) {
                options.touchend && options.touchend.call($this, ev);
                document.removeEventListener('touchmove', fnMove, false);
                document.removeEventListener('touchend', fnEnd, false);
            }
            document.addEventListener('touchmove', fnMove, false);
            document.addEventListener('touchend', fnEnd, false);
            return false;
        }, false)
        options.init && options.init.call($this);
    }

    //右侧弹出层
    $.fn.rightSwipeAction = function (options) {
        var setting = {
            show: 'swipeLeft-block',
            clickEnd: null
        };
        options = Object.extend(options, setting);
        var $child = $(this.children(1)), display = 'none';
        if ($child.hasClass(options.show)) {
            display = 'none';
        } else {
            display = 'block';
        }
        options.clickEnd && options.clickEnd.call($child, display);
    };
    
    //右侧附加选择层插件
    $.fn.rightSwipe = function (options) {
        var $temp = null;
        var setting = {
            isclick: null,
            zindex: 999999,
            back: '.leftmask',
            alert: '.leftPopup',
            clickEnd: null //打开关闭层回调事件
        };
        options = Object.extend(options, setting);
        this.each(function (index, curr) {
            var $curr = $(curr);
            (function ($this) {
                $this.isclick = true;
                $this.click(function (ev) {
                    options.isclick && ($this.isclick = options.isclick.call($this));
                    if ($this.isclick == false) {
                        return;
                    }
                    ev.preventDefault();
                    var $leftPopup = $('.leftPopup.' + $this.attr('data-action'));
                    $leftPopup[0].style.zIndex = options.zindex;
                    $leftPopup.rightSwipeAction({
                        clickEnd: function (display) {
                            var $back = $('.' + $leftPopup.attr('data-back')),
                                $swipeLeft = $leftPopup.find('.swipeLeft');
                            $back.css('z-index', options.zindex - 10000);
                            $back.show();
                            $leftPopup.show();
                            setTimeout(function () { $swipeLeft.addClass('swipeLeft-block'); }, 200);

                            $back.on('close', function () {
                                $(options.back).each(function (index, curr) {
                                    curr.style.display = 'none';
                                })
                                var $alert = $(options.alert).children().removeClass('swipeLeft-block').end();
                                setTimeout(function () { $alert.css('z-index', 0).hide(); }, 200);
                            })

                            $back.touches({
                                touchstart: function () {
                                    $back.trigger('close');
                                }
                            })
                            $swipeLeft.transitionEnd({ end: function () { options.clickEnd && options.clickEnd.call($leftPopup, true); } })
                        }
                    });
                })
            })($curr);
        })
    }

    
    

    //拖拽内容
    $.fn.toucheContent = function (options) {
        var setting = {
            tt: '.tt-list',
            h: 45
        }
        options = Object.extend(options, setting);
        var disy = 0, y = 0, lastY = 0, speedY = 0;
        var $this = this;
        var $content = $this.find(options.tt);
        $content.on('st', function (event, t) {
            if (t >= 0) { t = 0; }
            if (Math.abs(t) > height()) { t = -height() }
            if (t <= 0) {
                $content.addClass3('transform', 'translateY(' + t + 'px)');
                y = t;
            }
        })
        if ($content.attr('checked') != 'checked') {
            $this.touches({
                touchstart: function (ev) {
                    speedY = 0;

                    disy = ev.targetTouches[0].pageY - y;
                },
                touchmove: function (ev) {
                    ev.preventDefault();
                    var t = ev.targetTouches[0].pageY - disy;
                    $content.addClass3('transition', '0ms all ease-out');
                    $content.trigger('st', t);
                    speedY = t - lastY;
                    lastY = t;
                },
                touchend: function (ev) {
                    if (speedY != 0) {
                        $content.addClass3('transition', '700ms all ease-out');
                        if (speedY > 0) {
                            $content.trigger('st', y + (Math.abs(speedY) * 4 - Math.abs(speedY)) * 4);
                        } else {
                            $content.trigger('st', y + -(Math.abs(speedY) * 4 - Math.abs(speedY)) * 4);
                        }

                    }
                }
            })
            $content.attr('checked', 'checked');
        }

        function height() {
            return $content.height() - (document.documentElement.clientHeight || document.body.clientHeight) + options.h;
        }
    }
})(jQuery);